using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SpectraCyber
{
    abstract class cSpectraCyberBase
    {
        // #### Datamembers #### //

        protected bool      mbOnline;					// Whether or not the unit is online (able to send/receive commands, inited, etc)
        protected bool      mbScanning;                 // Whter or not the SpectraCyber is in the middle of a scan.
        protected int       miVersion;					// The version of the unit.
        protected bool      mbNoiseSourceStatus;		// Noise source on (true) or off (false)
        protected double    mdRestCorrection;           // The rest correction for the unit.

        // Data Voltage
        protected double    mdVoltageStepSize;          // The size (in volts) of each step of the voltage reply.
        protected double    mdVoltageMax;               // The maximum output voltage
        protected int       miVoltageNumSteps;          // The number of voltage steps to get from zero to the maximum voltage

        // The IF Gain 
        protected double    mdIFGain;					
        protected double    mdIFGainMin; 
        protected double    mdIFGainMax;

        // The PLL Frequency
        protected double mdPLLRestFrequency;            // The center of the PLL clock range plus or minus the rest correction
        protected double    mdPLLFrequency;			    // The Frequency of the PLL Clock (formula for actual observation frequency in cpp file)
        protected double    mdPLLStepSize;              // The Size of a step that the PLL chip uses
        protected double    mdPLLMaxOffset;             // The maximum offset from the rest frequency
        protected double    mdPLLLowerBound;            // The Lower bound for the scan
        protected double    mdPLLUpperBound;            // The Upper boudn for the scan

        //eMode meMode;						            // The SC's Mode (Continuum (!D000) or Spectrum (!D001 (default)).
        protected enumMode  meMode;
        protected enumMode[] meModes;

        // some comment about the stuff below here...
        protected cCommunication mcommCommunication;
        protected cPriorityQueue mCommandQueue;          // The command queue
        protected cPriorityQueue mReplyQueue;
        protected cDatabaseQueue mDatabaseQueue;

        // Datamembers for the Serial Port connection
        protected string    mstrCommPort;
        protected int       miBaudRate = 2400;
        protected int       miDataBits = 8;
        protected System.IO.Ports.Parity  mparParity = System.IO.Ports.Parity.None;
        protected int miCharInputBufferSize = 4;

        // Temporary (?) Datamembers
        protected SpectraCyber1Form mpForm;

        // Temporary functions:
        public void SetCommandWindowText(string strCommand)
        {
            if (!(mpForm.IsClosing))
            {
                SetCommandWindowDelegate pDelegate = new SetCommandWindowDelegate(mpForm.SetCommandWindowText);
                mpForm.Invoke(pDelegate, new object[] { strCommand });
            }
        }

        // Delegates
        public delegate void SetCommandWindowDelegate(string strText);
        public delegate void VoltageTextboxDelegate(string strText);
        public delegate void FrequencyTextboxDelegate(string strText);

        public cSpectraCyberBase()
        {

            // Change communication settings on the communication object
            // TODO: Eventually a communication object will be inited to determine which
            // SpectraCyber unit we are dealing with, until then we have to init one temporarly.
            mcommCommunication = new cCommunication();
            mcommCommunication.BaudRate = miBaudRate;
            mcommCommunication.DataBits = miDataBits;
            mcommCommunication.Parity = System.IO.Ports.Parity.None;
            mcommCommunication.BufferSize = miCharInputBufferSize;
            mcommCommunication.SpectraCyber = this;

            mCommandQueue = mcommCommunication.CommandQueue;
            mReplyQueue = mcommCommunication.ReplyQueue;
            mDatabaseQueue = mcommCommunication.DatabaseQueue;
            
            // Init datamembers
            mbOnline = false;
            mbScanning = false;
        }

        public virtual void Reset()
        {
            mcommCommunication.Reset();
            // Send the rest command to the SpectraCyber unit.
            SendCommand("!R000", 0, true, 4, eCommandType.Reset);
        }

        public virtual void Scan()
        {
            // Send the commands to the SpectraCyber to being a scan
            
        }

        public virtual void Stop()
        {
            SendCommand("_STOP", 0, false, 0, eCommandType.ScanStop);
        }

        // Processes noise source replies
        public virtual void ProcessReply(string strReply, eCommandType eType)
        {
            // TODO: if a reply to a reset command is received (R000 or R001), we need to mark the unit as being online.
            if (strReply[0] == 'R')
                mbOnline = true;
        }

        // #### Temporary Methods #### //
        public void SetParent(SpectraCyber1Form pForm)
        {
            mpForm = pForm;
        }

        // Append strCommand to the queue, wait 55 milliseconds, a reply is not expected.
        public void SendCommand(string strCommand)
        {
            cCommandItem pCommand = new cCommandItem(strCommand, 0, false, 0, eCommandType.GeneralCommunication);
            mCommandQueue.Add(pCommand);
        }

        // Append strCommand to the queue with the default wait time as a eCommandType.Communication command
        public void SendCommand(string strCommand, int iTimeToWaitMS, bool bExpectReply, int iNumCharsToRead)
        {
            cCommandItem pCommand = new cCommandItem(strCommand, iTimeToWaitMS, bExpectReply, iNumCharsToRead, eCommandType.GeneralCommunication);
            mCommandQueue.Add(pCommand);
        }

        // Append strCommand to the queue, wait for iTimeToWait milliseconds (minimum 55 ms), and either expect or disregard a reply.
        public void SendCommand(string strCommand, int iTimeToWaitMS, bool bExpectReply, int iNumCharsToRead, eCommandType eCommType)
        {
            cCommandItem pCommand = new cCommandItem(strCommand, iTimeToWaitMS, bExpectReply, iNumCharsToRead, eCommType);
            mCommandQueue.Add(pCommand);
        }

        // #### Methods #### //
        
        // Return the version number for the SpectraCyber
        public int Version
        {
            get
            {
                return miVersion;
            }
        }

        // Get mode in which the SpectraCyber is currently operating
        public enumMode Mode
        {
            get
            {
                return meMode;
            }
        }

        // Turn on (true) or off (false) the noise source power
        public void SetNoiseSourceStatus(bool bNoiseSourceOn)
        {
            string strCommand = "!N000"; // By default, the noise source is off

            if (bNoiseSourceOn)    // Noise source on
                strCommand = "!N001";

            SendCommand(strCommand, 0, false, 0, eCommandType.ChangeSetting);
        }

        // Turn on or off the noise source based upon the value of the mbNoiseSourceStatus datamember
        public void SetNoiseSourceStatus()
        {
            string strCommand = "!N000";

            if (mbNoiseSourceStatus) // Noise source is on
                strCommand = "!N001";

            SendCommand(strCommand, 0, false, 0, eCommandType.ChangeSetting);
        }

        // Find out whether or not the noise source is on
        public bool GetNoiseSourceStatus()
        {
            return this.mbNoiseSourceStatus;
        }

        // #### Conversion Methods #### //
        // Convert an integer to a Hexadecimal value, returned as a string.
        protected string IntToHexString(int iValue, int iTotalLength)
        {
            string strHex = "0123456789ABCDEF";
            string strOutput = "";

            // First, encode the integer into hex (but backward)
            while (iValue > 0)
            {
                strOutput = strHex[iValue % 16] + strOutput;
                iValue /= 16;
            }

            // Now, pad the string with zeros to make it the correct length.
            for (int i = strOutput.Length; i < iTotalLength; i++)
            {
                strOutput = "0" + strOutput;
            }

            // Finally, reverse the direction of the string and return it
            return strOutput;
        }

        // Convert a Hex string to an integer
        protected int HexStringToInt(string strHexInput)
        {
            return Convert.ToInt32(strHexInput, 16);
        }

        // #### Communication Settings and Methods#### //
        public void Connect()
        {
            // Connect to the unit and send a reset command
            mcommCommunication.Connect();
            Reset();
            mbOnline = true;
        }

        public void Disconnect()
        {
            // Close the connection to the unit
            SendCommand("_DISCONNECT", 0, false, 0, eCommandType.Termination);
            
            mbOnline = false;
        }
        
        // Get the Comm Port
        public string CommPort
        {
            get
            {
                return mcommCommunication.CommPort;
            }
            set
            {
                mcommCommunication.CommPort = value;
            }
        }

        // See if the unit is online
        public bool Online
        {
            get
            {
                return mbOnline;
            }
        }

        // See if the unit is scanning
        public bool Scanning
        {
            get
            {
                return mbScanning;
            }
        }

        // Return the 
        public double PLLStepSize
        {
            get
            {
                return mdPLLStepSize;
            }
        }

        public double PLLMaxOffset
        {
            get
            {
                return mdPLLMaxOffset;
            }
        }

        // Set or Get the Lower Bound for the PLL Frequency.  Valid values -2 ... + 2.
        public double PLLLowerBound
        {
            get
            {
                return mdPLLLowerBound;
            }
            set
            {
                double dPLLLowerBound = value;

                if (Math.Abs(dPLLLowerBound) > mdPLLMaxOffset)
                    System.Windows.Forms.MessageBox.Show("Lower PLL bound is out of range.");
                else
                    mdPLLLowerBound = mdPLLRestFrequency + dPLLLowerBound;
            }
        }

        // Set or Get the Upper Bound for the PLL Frequency.  Valid values -2 ... + 2.
        public double PLLUpperBound
        {
            get
            {
                return mdPLLUpperBound;
            }
            set
            {
                double dPLLUpperBound = value;

                if (Math.Abs(dPLLUpperBound) > mdPLLMaxOffset)
                    System.Windows.Forms.MessageBox.Show("Upper PLL bound is out of range.");
                else
                    mdPLLUpperBound = mdPLLRestFrequency + dPLLUpperBound;
            }
        }

        // Events:

        // A scan has been completed
        protected void ScanComplete()
        {

        }

        // The unit is online
        protected void UnitOnline()
        {

        }
    }
}
