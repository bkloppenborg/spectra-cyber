using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpectraCyber
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SpectraCyber1Form frmSC1 = new SpectraCyber1Form(cboCommPorts.SelectedItem.ToString());
            frmSC1.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string strCommPort in System.IO.Ports.SerialPort.GetPortNames())
                cboCommPorts.Items.Add(strCommPort);

            if (cboCommPorts.Items.Count > 0)
                cboCommPorts.SelectedIndex = 0;
            else  // No comm ports were detected, notify the user and prohibit them from continuing.
            {
                System.Windows.Forms.MessageBox.Show("No COMM ports were detected on your system.  Please check the device manager.");
                btnConnect.Enabled = false;
            }

        }
    }
}