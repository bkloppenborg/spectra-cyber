/* 
 * Copyright (c) 2007 Brian Kloppenborg
 *
 * If you use this software as part of a scientific publication, please cite as:
 *
 * Kloppenborg, B. (2012), "SpectraCyber Control Software"
 * (Version X). Available from  <https://github.com/bkloppenborg/spectra-cyber>.
 *
 * This file is part of the SpectraCyber Control Software (SCCS).  
 *
 * The SpectraCyber was developed by Dr. John Bernard and gifted to
 * Radio Astronomy Supplies (RAS).  "SpectraCyber" is used with 
 * permission from RAS.
 * 
 * SCCS is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License 
 * as published by the Free Software Foundation version 3.
 * 
 * SCCS is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public 
 * License along with SCCS.  If not, see <http://www.gnu.org/licenses/>.
 */

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
