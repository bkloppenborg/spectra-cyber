using System;
using System.Collections.Generic;
using System.Text;

enum enumMode  // An enumeration for the modes of the SpectraCyber unit.               
{
    Spectrum,
    Continuum
}

namespace SpectraCyber
{
    class cSpectraCyber1 : cSpectraCyberBase
    {
        // Datamembers

        // DC Gain for Spectrometer and Continuum modes
        protected double mdDCOffsetCont;
        protected double mdDCOffsetSpec;
        protected double mdDCOffsetMin;
        protected double mdDCOffsetMax;
        protected double mdDCOffsetStepSize;

        protected double[] marrContinuumIntegrationValues = new double[3] {0.3, 1, 10};
        protected double[] marrSpectrumIntegrationValues = new double[3] { .3, .5, 1 };
        protected double mdContinuumIntegration;	    
        protected double mdSpectrumIntegration;		    

        protected int[] marrGainValues = new int[6] { 1, 5, 10, 20, 50, 60 };  // n -> times n. See documentation for reference.

        protected int miContinuumGain;
        protected int miSpectrumGain;

        protected int[] marrBandwithValues = new int[2] { 15, 30 }; // Valid bandwith values
        protected int miBandwidth;
        

        // The constructor with a call to the non-standard base-class constructor
        public cSpectraCyber1(string strCommPort)
        {
            miCharInputBufferSize = 4;
            CommPort = strCommPort;

            InitDatamembers();
        }

        private void InitDatamembers()
        {

            // Init remaining datamembers:
            miVersion = 1;
            mbNoiseSourceStatus = false;

            // IF Gain
            mdIFGain = 10.0;
            mdIFGainMin = 10.0;
            mdIFGainMax = 25.75;

            // PLL Frequency
            mdPLLRestFrequency = 48.6;
            mdPLLFrequency = 46.6;
            mdPLLMaxOffset = 2;
            mdPLLStepSize = .005;

            // DC Offsets:
            mdDCOffsetCont = 0;
            mdDCOffsetSpec = 0;
            mdDCOffsetMin = 0;
            mdDCOffsetMax = 4.096;
            mdDCOffsetStepSize = (mdDCOffsetMax / 0xFFF); // About 0.0010002 volts per step (in 4095 steps)

            // Integration Times:
            mdContinuumIntegration = 0.3;
            mdSpectrumIntegration = 0.3;

            // Gain Values:
            miContinuumGain = marrGainValues[0];
            miSpectrumGain = marrGainValues[0];

            // Voltage settings and step size
            mdVoltageMax = 10;
            miVoltageNumSteps = 0xFFF;  // 4095 Steps
            mdVoltageStepSize = (mdVoltageMax / miVoltageNumSteps);

            // Bandwidth
            miBandwidth = marrBandwithValues[0];
        }

        public override void Reset()
        {
            // Call the base-class reset command
            base.Reset();
        }

        public override void ProcessReply(string strReply, eCommandType eType)
        {
            // Temp: Show the replies in the command window box:
            if (!(mpForm.IsClosing))
            {
                SetCommandWindowDelegate pDelegate = new SetCommandWindowDelegate(mpForm.SetCommandWindowText);
                mpForm.Invoke(pDelegate, new object[] { strReply });
            }

            // Handle special (non-supported command) cases first
            if (eType == eCommandType.ScanStart)
            {
                // We are starting a scan.  Make a new entry in the database for the scan and for the settings
                cDatabaseItem dbiCommand = new cDatabaseItem(eDBCommandType.ScanStart);
                mDatabaseQueue.Add(dbiCommand);
                SettingsChanged();

                mbScanning = true;
            }
            else if (eType == eCommandType.ScanStop)
            {
                // The scan has stopped, note so
                cDatabaseItem dbiCommand = new cDatabaseItem(eDBCommandType.ScanStop);
                mDatabaseQueue.Add(dbiCommand);

                mbScanning = false;
                base.ScanComplete();
            }
            else if (eType == eCommandType.Rescan)
            {
                Scan();
            }
            else
            {
                // Init vars
                int iValue;
                char cFirstCharacter = strReply[0];
                strReply = strReply.Remove(0, 1);

                // If we reach this point, we either have a data point, or a setting change.  The majority
                // of times we come in here will be for data points but the majority of the code below is
                // for settings changes.
                bool bSettingsChanged = true;
                
                // Ready for a long set of if statments?  Go!
                if (cFirstCharacter == 'A')         // IF Gain
                {
                    iValue = HexStringToInt(strReply);
                    mdIFGain = 10 + iValue * 0.25;
                }
                else if (cFirstCharacter == 'B')    // Change of Bandwidth (not all SpectraCyber 1 units support this command.
                {
                    int iIndex = int.Parse(strReply);
                    miBandwidth = marrBandwithValues[iIndex];
                }
                else if (cFirstCharacter == 'D')    // Data point
                {
                    // eCommandType.DataRequests and eCommandType.DataDiscards get us into this block
                    // Neither of which is a settings change
                    bSettingsChanged = false;

                    // Data requests are the only data command that is processed.
                    if(eType == eCommandType.DataRequest) 
                    {
                        // A few more vars:
                        int iVoltage = HexStringToInt(strReply);
                        double dVoltage = Math.Round(iVoltage * mdVoltageStepSize, 6);
                        double dFrequency = 0;

                        // Setup the vars for the database item a little more
                        string[] arrFields = new string[] { "fFrequency", "fValue" };
                        string[] arrValues;
                        eDBCommandType eDBType = eDBCommandType.Data;

                        // Now, build the values for the database item, Continuum and Spectrum modes do this differently.
                        if (meMode == enumMode.Continuum)
                            arrValues = new string[] { null, dVoltage.ToString() };
                        else
                        {
                            // Calculate the observation frequency:
                            dFrequency = 1000 * mdPLLFrequency + 1371805;  // In GHz
                            arrValues = new string[] { dFrequency.ToString(), dVoltage.ToString() };
                        }

                        if (!(mpForm.IsClosing))
                        {
                            VoltageTextboxDelegate pDelegate = new VoltageTextboxDelegate(mpForm.SetVoltageTextbox);
                            mpForm.Invoke(pDelegate, new object[] { dVoltage.ToString() });

                            FrequencyTextboxDelegate pFreqDelegate = new FrequencyTextboxDelegate(mpForm.SetFrequencyTextbox);
                            mpForm.Invoke(pFreqDelegate, new object[] { dFrequency.ToString() });
                        }

                        cDatabaseItem dbiCommandItem = new cDatabaseItem(arrFields, arrValues, eDBType);
                        mDatabaseQueue.Add(dbiCommandItem);
                    }
                }
                else if (cFirstCharacter == 'F')    // PLL Frequency Change
                {
                    iValue = HexStringToInt(strReply);
                    mdPLLFrequency = Math.Round(46 + ((iValue - .05) / 200), 3);
                }
                else if (cFirstCharacter == 'G')    // Continuum DC Gain
                {
                    int iIndex = int.Parse(strReply);
                    miContinuumGain = marrGainValues[iIndex];
                }
                else if (cFirstCharacter == 'I')    // Continuum Integration Setting
                {
                    int iIndex = int.Parse(strReply);
                    mdContinuumIntegration = marrContinuumIntegrationValues[iIndex];
                }
                else if (cFirstCharacter == 'J')    // Spectrum DC Voltage Offset
                {
                    iValue = HexStringToInt(strReply);
                    mdDCOffsetSpec = iValue * mdDCOffsetStepSize;
                }
                else if (cFirstCharacter == 'K')    // Spectrometer DC Gain
                {
                    int iIndex = int.Parse(strReply);
                    miSpectrumGain = marrGainValues[iIndex];
                }
                else if (cFirstCharacter == 'L')    // Spectrometer Integration Setting
                {
                    int iIndex = int.Parse(strReply);
                    mdSpectrumIntegration = marrSpectrumIntegrationValues[iIndex];
                }
                else if (cFirstCharacter == 'N')    // Noise Source Status changed
                {
                    bool bNoiseSourceOn = false;

                    if (strReply[2] == 1)   // Look at the last character in the string
                        mbNoiseSourceStatus = true;

                    mbNoiseSourceStatus = bNoiseSourceOn;
                }
                else if (cFirstCharacter == 'O')    // Continuum DC Voltage Offset
                {
                    iValue = HexStringToInt(strReply);
                    mdDCOffsetCont = iValue * mdDCOffsetStepSize;
                }
                else if (cFirstCharacter == 'R')    // SpectraCyber Reset.
                {
                    mcommCommunication.Reset();
                    InitDatamembers();

                    // Even though the settings have been changed we don't invoke the SettingsChanged method
                    // The eCommandType.ScanStart "command" takes care of sending the settings off to the database
                    bSettingsChanged = false;
                }
                else // The command is not implemented.  There is not a settings change associated with this item.
                {
                    // System.Windows.Forms.MessageBox.Show("The specified command is not implemented for this unit.  Please verify the command structure.");
                    bSettingsChanged = false;
                }

                // If some setting on the SpectraCyber has changed, update all of the settings.
                if (bSettingsChanged && mbScanning)
                    SettingsChanged();

            }
        }

        protected void SettingsChanged()
        {
            // These are the fields we are updating
            string[] arrFields = new string[] {
                    "fIFGain", 
                    "fBandwidth", 
                    "fDCGainContinuum", 
                    "fDCGainSpectrum", 
                    "fIntegrationContinuum", 
                    "fIntegrationSpectrum", 
                    "fNoiseSourceOn", 
                    "fDCOffsetContinuum", 
                    "fDCOffsetSpectrum",
                    "fMode"
                };

            // These are their corresponding values (as strings).
            object[] arrValues = new object[] {
                    mdIFGain,
                    miBandwidth,
                    miContinuumGain,
                    miSpectrumGain,
                    mdContinuumIntegration,
                    mdSpectrumIntegration,
                    mbNoiseSourceStatus,
                    mdDCOffsetCont,
                    mdDCOffsetSpec,
                    meMode.ToString()
                };

            // This is a settings type database item
            eDBCommandType eDBType = eDBCommandType.Settings;

            // Build the DB item and add the item to the queue:
            cDatabaseItem dbiCommandItem = new cDatabaseItem(arrFields, arrValues, eDBType);
            mDatabaseQueue.Add(dbiCommandItem);
        }

        // ########### Sets and Gets ###########

        // Set the mode in which the SpectraCyber will operate
        private void SetMode(enumMode eMode)
        {
            // TODO: Allow mode switching to take place in the Command Queue
            // so that one may schedule scanning proceesses
            meMode = eMode;
        }

        // Set the IF gain equal to the current datamember
        private void SetIFGain()
        {
            // Set the IF gain to the current value of the datamember:
            SetIFGain(mdIFGain);
        }

        // Set the IF gain to the dIFGainValue
        private void SetIFGain(double dIFGain)
        {
            // Conversion formula: Hex = IntToHexString( 4 * (IFGain - 10))
            string strCommand = "!A0" + IntToHexString((int)Math.Floor(4 * (dIFGain - 10)), 2);
            SendCommand(strCommand, 0, false, 0, eCommandType.ChangeSetting);
        }

        // Set the PLL Frequency
        private void SetFrequency(double dPLLFrequency)
        {
            // Determine the integration time for after this command is sent
            int iIntegrationTime = (int)(1000 * mdSpectrumIntegration);

            // If the PLL Frequency is within bounds, set the PLL frequency and wait for iIntegrationTime milliseconds.
            if(dPLLFrequency >= 46.6 && dPLLFrequency < 50.6)
                SendCommand("!F" + IntToHexString((int) Math.Floor((200 * (dPLLFrequency - 46)) + 0.5), 3), iIntegrationTime, false, 0, eCommandType.FrequencySet);
        }

        // Set the Integration Time to the current datamember's value
        private void SetIntegration()
        {
            if (meMode == enumMode.Continuum)
                SetIntegration(mdContinuumIntegration);
            else
                SetIntegration(mdSpectrumIntegration);
        }

        // Set the integration value to dIntegration
        private void SetIntegration(double dIntegration)
        {
            int iIndex;
            string strCommand = "";

            if (meMode == enumMode.Continuum)
            {
                if (dIntegration != mdContinuumIntegration)
                {
                    iIndex = Array.IndexOf(marrContinuumIntegrationValues, dIntegration);
                    strCommand = "!I00" + iIndex.ToString();
                }
            }
            else
            {
                if(dIntegration != mdSpectrumIntegration)
                {
                    iIndex = Array.IndexOf(marrSpectrumIntegrationValues, dIntegration);
                    strCommand = "!L00" + iIndex.ToString();
                }
            }

            if(strCommand.Length > 0)
                SendCommand(strCommand, 0, false, 0, eCommandType.ChangeSetting);
        }

        // Set the Gain to the value of the current datamember
        private void SetGain()
        {
            // We either set the Continuum or Spectrum Mode
            if (meMode == enumMode.Continuum)
                SetGain(miContinuumGain);
            else
                SetGain(miSpectrumGain);
        }

        // Set the Gain to one of the settings in the marrGainValues array.
        private void SetGain(int iValue)
        {
            // init var
            string strCommand;

            // We either set the Continuum or Spectrum Mode
            if (meMode == enumMode.Continuum)
                strCommand = "!G00";
            else
                strCommand = "!K00";

            strCommand += Array.IndexOf(marrGainValues, iValue);

            // Add the index number (which corresponds to the setting) to the end of the string.
            SendCommand(strCommand, 70, false, 0, eCommandType.ChangeSetting);
        }

        // Set the DC offset to the value of the current datamember
        private void SetDCOffset()
        {
            // We either set the Continuum or Spectrum Mode
            if (meMode == enumMode.Continuum)
                SetDCOffset(mdDCOffsetCont);
            else
                SetDCOffset(mdDCOffsetSpec);
        }

        // Set the DC offset to dOffset
        private void SetDCOffset(double dOffset)
        {
            // init var
            string strCommand;

            // We either set the Continuum or Spectrum Mode
            if (meMode == enumMode.Continuum)
                strCommand = "!O";
            else
                strCommand = "!J";

            strCommand += IntToHexString((int) (dOffset / mdDCOffsetStepSize), 3);

            SendCommand(strCommand, 0, false, 0, eCommandType.ChangeSetting);
        }

        // Set the bandwidth to the value of the current datamember
        private void SetBandwidth()
        {
            SetBandwidth(miBandwidth);
        }

        // Set the Bandwidth to one of the settings in the marrBandwidthValues array.
        private void SetBandwidth(int iBandwidth)
        {
            string strCommand = "!B00" + Array.IndexOf(marrBandwithValues, iBandwidth);
            SendCommand(strCommand, 0, false, 0, eCommandType.ChangeSetting);
        }

        // Start a scan with the current settings
        public override void Scan()
        {
            // If we are not currently scanning, we need to send the settings to the unit.
            if (!(mbScanning))
            {                
                // Note that we are starting a new data run
                SendCommand("_SCAN_START", 0, false, 0, eCommandType.ScanStart);

                // Send all current settings
                SetNoiseSourceStatus();
                SetIFGain();
                SetGain();
                SetIntegration();
                SetDCOffset();
                SetBandwidth();
            }

            // Now, start taking data
            TakeData();
        }

        private void TakeData()
        {
            // Init datamembers:
            int iIntegration;

            // 
            if (meMode == enumMode.Continuum)
            {
                iIntegration = (int) (1000 * mdContinuumIntegration);

                // If we are not scanning, we need to send a data point (which we discard)
                if (!(mbScanning))
                    SendCommand("!D000", iIntegration, false, 4, eCommandType.DataDiscard);

                // Take 60 Continuum-mode data points (don't flood the queue)
                for (int i = 0; i < 60; i++)
                    SendCommand("!D000", iIntegration, true, 4, eCommandType.DataRequest);
            }
            else
            {
                // If we are not scanning, we need to send a data point (which we discard)
                if (!(mbScanning))
                {
                    // Set the PLL Frequency
                    SetFrequency(mdPLLLowerBound);
                    SendCommand("!D001", 0, false, 4, eCommandType.DataDiscard);
                }
                else  // Reset the PLL frequency
                {
                    mdPLLFrequency = mdPLLLowerBound;
                }

                // Do a linear scan from the lowest bound to the highest bound:
                for( int i = 0; (mdPLLFrequency < mdPLLUpperBound); i++)
                {
                    SetFrequency(mdPLLLowerBound + (mdPLLStepSize * i));

                    // TEMP: Setting the PLL Frequency to keep loop within bounds
                    mdPLLFrequency = mdPLLLowerBound + (mdPLLStepSize * i);

                    SendCommand("!D001", 70, true, 4, eCommandType.DataRequest);
                }
            }

            // Lastly, tell the software to start a new scan.
            SendCommand("_RESCAN", 0, false, 0, eCommandType.Rescan);

        }

        // ########### ###########

        // Get or Set the IF Gain (a set is processed through the SpectraCyber unit)
        public double IFGain
        {
            get
            {
                return mdIFGain;
            }
            set
            {
                // The SC1 unit only supports IF gains between 10.0 and 25.75, anything outside of that is invalid.
                if ((10.0 <= value) && (value <= 25.75))
                {
                    if(value != mdIFGain)
                        SetIFGain(value);
                }
                else
                    System.Windows.Forms.MessageBox.Show("IF Gain is out of Range");
            }
        }

        // Get the minimum IF Gain
        public double IFGainMin
        {
            get
            {
                return mdIFGainMin;
            }
        }

        // Get the Maximum IF Gain
        public double IFGainMax
        {
            get
            {
                return mdIFGainMax;
            }
        }

        // Returns an array with the valid integration values for the current Mode.
        public Array ValidIntegrationValues
        {
            get
            {
                if (meMode == enumMode.Continuum)
                    return marrContinuumIntegrationValues;
                else
                    return marrSpectrumIntegrationValues;
            }

        }

        // Get or Set the Integration value for the current Mode.
        public double Integration
        {
            // TODO: Check for valid input values
            get
            {
                if (meMode == enumMode.Continuum)
                    return mdContinuumIntegration;
                else
                    return mdSpectrumIntegration;         
            }
            set
            {
                SetIntegration(value);
            }
        }

        // Get or Set the DC Offset for the current Mode.
        public double DCOffset
        {
            // TODO: Check for valid input values:
            get
            {
                if (meMode == enumMode.Continuum)
                    return mdDCOffsetCont;
                else
                    return mdDCOffsetSpec;
            }
            set
            {
                if (value != DCOffset)
                    SetDCOffset(value);
            }
        }

        // Get the Minimum value for the DC Offset
        public double DCOffsetMinValue
        {
            get
            {
                return mdDCOffsetMin;
            }
        }

        // Get the Maximum value for the DC Offset
        public double DCOffsetMaxValue
        {
            get
            {
                return mdDCOffsetMax;
            }
        }

        // Get the minimum increase in a DC offset
        public double DCOffsetStepSize
        {
            get
            {
                return mdDCOffsetStepSize;
            }
        }

        // Get an array of the accepted gain values.
        public int[] GainValues
        {
            get
            {
                return marrGainValues;
            }
        }

        // Get or set the Gain for the current Mode
        public int Gain
        {
            // TODO: Check for valid input values
            get
            {
                if (meMode == enumMode.Continuum)
                    return miContinuumGain;
                else
                    return miSpectrumGain;
            }
            set
            {
                // Only set the gain if we have to.
                if(value != Gain)
                    SetGain(value);
            }
        }

        // Get or set the mode in which the SpectraCyber is operating
        new public enumMode Mode
        {
            get
            {
                return meMode;
            }
            set
            {
                SetMode(value);
            }
        }

        // Get or Set the bandwith
        public int Bandwith
        {
            get
            {
                return miBandwidth;
            }
            set
            {
                if (value != Bandwith)
                    SetBandwidth(value);
            }
        }

        // Get an array of valid bandwith values
        public Array BandwidthValues
        {
            get
            {
                return marrBandwithValues;
            }
        }

    }
}
