using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpectraCyber
{
    public partial class SpectraCyber1Form : Form
    {
        // Datamembers
        private cSpectraCyber1 mscSpectraCyber;
        protected bool mbIsClosing;
        protected bool mbIsLoading;
        protected bool mbnudIFGainChanging;
        protected bool mbnudDCOffsetChanging;

        public SpectraCyber1Form(string strCommPort)
        {
            // Init datamembers:
            mscSpectraCyber = new cSpectraCyber1(strCommPort);
            mscSpectraCyber.SetParent(this);
            mbIsClosing = false;
            mbnudIFGainChanging = false;
            mbnudDCOffsetChanging = false;

            InitializeComponent();
        }

        // TEMPORARY METHOD: (to see commands)

        // Turn on/off the noise source:
        private void rdoNoiseSourceOn_CheckedChanged(object sender, EventArgs e)
        {
            // init vars
            bool bNoiseOn = false;  // By default, the noise source is off

            if (rdoNoiseSourceOn.Checked)
                bNoiseOn = true;

            mscSpectraCyber.SetNoiseSourceStatus(bNoiseOn);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Reset the SpectraCyber and the form's controls
            mscSpectraCyber.Reset();
            SetFormControlValues();
            ToggleScanControls(false);
        }

        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            mscSpectraCyber.SendCommand(txtCommand.Text, 80, chkExpectReply.Checked, 4);
        }

        // A method and delegate for setting the window text.
        delegate void CommandWindowDelegate(string strText);
        public void SetCommandWindowText(string strText)
        {
            lstCommandWindow.Items.Add(lstCommandWindow.Items.Count.ToString() + ": " + strText);
            lstCommandWindow.SelectedIndex = (lstCommandWindow.Items.Count - 1);
        }

        // A method and delegate for setting the Voltage Textbox text
        delegate void VoltageTextboxDelegate(string strText);
        public void SetVoltageTextbox(string strText)
        {
            txtVoltage.Text = strText;
        }

        // A method and delegate for setting the frequency textbox
        delegate void FrequencyTextboxDelegate(string strText);
        public void SetFrequencyTextbox(string strText)
        {
            txtFrequency.Text = strText;
        }

        // Load values into the form's controls based upon the current SpectraCyber settings:
        private void SpectraCyber1Form_Load(object sender, EventArgs e)
        {
            SetFormOnLoadControlValues();
            SetFormControlValues();
        }

        private void menuConnect_Click(object sender, EventArgs e)
        {
            if (!(mscSpectraCyber.Online))
            {
                mscSpectraCyber.Connect();
                ToggleConnectionControls(true);
            }
            else
            {
                mscSpectraCyber.Disconnect();
                ToggleConnectionControls(false);
            }
        }

        // Tell the SpectraCyber unit to either Start the Scan or to stop:
        private void btnScanStop_Click(object sender, EventArgs e)
        {
            // If we aren't scanning, start a scan and change the textbox's settings.
            if (!(mscSpectraCyber.Scanning))
            {
                bool bStartScan = true;

                // See if we need to set the PLL frequency ranges:
                if (mscSpectraCyber.Mode == enumMode.Spectrum)
                {
                    // The SpectraCyber makes steps in 5 Khz increments, inforce this
                    if ((nudPLLLowerFrequency.Value % 5 != 0) || (nudPLLUpperFrequency.Value % 5 != 0))
                    {
                        nudPLLUpperFrequency.Value %= 5;
                        nudPLLLowerFrequency.Value %= 5;
                    }

                    // Check the frequency boxes.  Make sure the lower bound is greater than the upper bound
                    if (nudPLLLowerFrequency.Value > nudPLLUpperFrequency.Value)
                    {
                        decimal dPLLFrequency = nudPLLUpperFrequency.Value;
                        nudPLLUpperFrequency.Value = nudPLLLowerFrequency.Value;
                        nudPLLLowerFrequency.Value = dPLLFrequency;
                    }

                    if (nudPLLLowerFrequency.Value == nudPLLUpperFrequency.Value) // No scan range specified.
                    {
                        bStartScan = false;
                        System.Windows.Forms.MessageBox.Show("The lower and upper bounds for the scan are the same value.");
                        nudPLLLowerFrequency.Focus();
                    }

                    // Set the datamembers
                    mscSpectraCyber.PLLLowerBound = (double)(nudPLLLowerFrequency.Value / 1000);
                    mscSpectraCyber.PLLUpperBound = (double)(nudPLLUpperFrequency.Value / 1000);
                }

                // Change the labels or function of a few controls
                ToggleScanControls(bStartScan);

                // If everything on the form is okay, Tell the SpectraCyber to Scan
                if (bStartScan)
                    mscSpectraCyber.Scan();
            }
            else
            {
                // Change the labels or function of a few controls
                ToggleScanControls(false);
                mscSpectraCyber.Stop();
            }
        }

        // Toggle the Enabled value of some controls.
        private void ToggleScanControls(bool bScanning)
        {
            if (bScanning)
                btnScanStop.Text = "Stop";
            else
                btnScanStop.Text = "Scan";

            // Enable/Disable some controls:
            cboMode.Enabled = !(bScanning);
            cboIntegration.Enabled = !(bScanning);
            nudPLLLowerFrequency.Enabled = !(bScanning);
            nudPLLUpperFrequency.Enabled = !(bScanning);
        }

        private void ToggleConnectionControls(bool bOnline)
        {
            // Either/Disable some controls
            if (bOnline)
                menuConnect.Text = "Disconnect";
            else
                menuConnect.Text = "Connect";

            btnReset.Enabled = bOnline;
            btnScanStop.Enabled = bOnline;
            menuReset.Enabled = bOnline;
        }

        private void cboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            enumMode eMode;

            if((enumMode) cboMode.SelectedItem == enumMode.Spectrum)
            {
                eMode = enumMode.Spectrum;
                grpFrequencyBounds.Show();
            }
            else
            {
                eMode = enumMode.Continuum;
                grpFrequencyBounds.Hide();
            }

            // Change the SpectraCyber's Mode, refresh the form's controls.
            mscSpectraCyber.Mode = eMode;
            SetFormControlValues();

        }

        // Set the values of the form's controls on load
        private void SetFormOnLoadControlValues()
        {
            // Show the port to which the SpectraCyber is connected.
            txtCommPort.Text = mscSpectraCyber.CommPort;
            
            // Set the Mode
            cboMode.Items.Add(enumMode.Continuum);
            cboMode.Items.Add(enumMode.Spectrum);
            cboMode.SelectedItem = mscSpectraCyber.Mode;

            // Setup the IF Gain Spin Box
            nudIFGain.Minimum = (decimal)mscSpectraCyber.IFGainMin;
            nudIFGain.Maximum = (decimal)mscSpectraCyber.IFGainMax;

            // Set the lower and upper bound of the frequency boxes as well as the step size:
            decimal dMinValue = (decimal)(-1000 * mscSpectraCyber.PLLMaxOffset);
            decimal dMaxValue = (decimal)(1000 * mscSpectraCyber.PLLMaxOffset);
            decimal dIncrement = (decimal)(1000 * mscSpectraCyber.PLLStepSize);

            nudPLLLowerFrequency.Minimum = dMinValue;
            nudPLLLowerFrequency.Maximum = dMaxValue;
            nudPLLLowerFrequency.Increment = dIncrement;

            nudPLLUpperFrequency.Minimum = dMinValue;
            nudPLLUpperFrequency.Maximum = dMaxValue;
            nudPLLUpperFrequency.Increment = dIncrement;

            // Set the DC Offset's spinbox properties
            nudDCOffset.Minimum = (decimal)mscSpectraCyber.DCOffsetMinValue;
            nudDCOffset.Maximum = (decimal)mscSpectraCyber.DCOffsetMaxValue;
            nudDCOffset.Increment = (decimal)mscSpectraCyber.DCOffsetStepSize;

            // Setup the Gain Combo box
            foreach (int iValue in mscSpectraCyber.GainValues)
                cboGain.Items.Add(iValue);

            if(mscSpectraCyber.GetNoiseSourceStatus())
                rdoNoiseSourceOn.Checked = true;
            else
                rdoNoiseSourceOff.Checked = true;

            // Lastly, disable the Scan and Reset controls until the connection with the unit is established
            ToggleConnectionControls(false);
        }
        
        // Set the values of the form's controls based upon the SpectraCyber's current settings
        private void SetFormControlValues()
        {
            // Setup the integration combo box
            cboIntegration.Items.Clear();
            foreach (double dValue in mscSpectraCyber.ValidIntegrationValues)
                cboIntegration.Items.Add(dValue);

            cboIntegration.SelectedItem = mscSpectraCyber.Integration;

            // Set the value for the DC Offset spinbox
            nudDCOffset.Value = (decimal)mscSpectraCyber.DCOffset;

            // Set the value for the Gain combo box
            cboGain.SelectedItem = mscSpectraCyber.Gain;
        }

        public bool IsClosing
        {
            get
            {
                return mbIsClosing;
            }
        }

        private void cboIntegration_SelectedIndexChanged(object sender, EventArgs e)
        {
            mscSpectraCyber.Integration = (double)cboIntegration.SelectedItem;
        }

        private void cboGain_SelectedIndexChanged(object sender, EventArgs e)
        {
            mscSpectraCyber.Gain = (int) cboGain.SelectedItem;
        }

        private void SpectraCyber1Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close the connection
            mbIsClosing = true;
            mscSpectraCyber.Disconnect();
        }

        // ^^^^^ Event handlers for the IF Gain box: ^^^^^
        private void nudIFGain_ValueChanged(object sender, EventArgs e)
        {
            // Only change the value of the IFGain if the value isn't still changing.
            if(!(mbnudIFGainChanging))
                IFGainBoxChanged();
        }

        private void nudIFGain_MouseUp(object sender, MouseEventArgs e)
        {
            mbnudIFGainChanging = false;
            IFGainBoxChanged();
        }

        private void nudIFGain_MouseDown(object sender, MouseEventArgs e)
        {
            mbnudIFGainChanging = true;
        }

        // Used to Notify the SpectraCyber that the value has changed.
        private void IFGainBoxChanged()
        {
            mscSpectraCyber.IFGain = (double)nudIFGain.Value;
        }

        // ^^^^^ Event handlers for the IF Gain box  ^^^^^

        // ^^^^^ Event handlers for the DC Offset box: ^^^^^
        private void nudDCOffset_ValueChanged(object sender, EventArgs e)
        {
            // Only change the value of the DC offset if the value is not still changing.
            if (!(mbnudDCOffsetChanging))
                DCOffsetBoxChanged();
        }

        private void nudDCOffset_MouseDown(object sender, MouseEventArgs e)
        {
            mbnudDCOffsetChanging = true;
        }

        private void nudDCOffset_MouseUp(object sender, MouseEventArgs e)
        {
            mbnudDCOffsetChanging = false;
            DCOffsetBoxChanged();
        }

        // Used to Notify the SpectraCyber that the value has changed.
        private void DCOffsetBoxChanged()
        {
            mscSpectraCyber.DCOffset = (double)nudDCOffset.Value;
        }

        // ^^^^^ Event handlers for the DC Offset box  ^^^^^

        // Custom Event Handlers
        private void mscSpectraCyber_ScanComplete(object sender, EventArgs e)
        {
            ToggleScanControls(mscSpectraCyber.Scanning);
        }
    }
}