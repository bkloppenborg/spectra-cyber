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
using System.Text;

namespace SpectraCyber
{
    abstract class SpectraCyberBase
    {
        // #### Datamembers #### //

        protected bool      mbOnline;					// Whether or not the unit is online (able to send/receive commands, inited, etc)
        protected int       miVersion;					// The version of the unit.
        protected bool      mbNoiseSourceStatus;		    // Noise source on (true) or off (false)
        protected double    mdIFGain;					// The IF Gain 
        protected double    mdPLLFrequency;			// The Frequency of the PLL Clock (formula for actual observation frequency in cpp file)
        //eMode meMode;						        // The SC's Mode (Continuum (!D000) or Spectrum (!D001 (default)).
        protected double    mdContinuumIntegration;	// Values: 0.3, 1, 10 seconds for SC I, "Fixed" 0.47 for SCII (switch on the front... TODO Fix this)
        protected double    mdSpectrumIntegration;		// Values: 0.3, .5, 1 seconds for SC I, "Fixed" 0.47 for SCII (switch on the front... TODO Fix this)

        // Thread pointers and bool status values.
        protected System.IO.Ports.SerialPort mrSerialPort;

       // CWinThread* mpReaderThread;
       // CWinThread* mpCommunicationThread;
        protected bool      mbRunReaderThread;
        protected bool      mbRunCommunicationThread;


        protected int       miMaxQueueItems;			    // Maximum number of items in the queue.
        protected int       miTimeBetweenDataPoints;	    // In milliseconds (55 milliseconds minimum).
        protected int       miTimeBetweenDataSets;		// In milliseconds
        protected int       miQueueEmptySleepTime;		// In milliseconds

        // Datamembers for the Serial Port connection
        protected string    mstrCommPort;
        protected int       miBaudRate;
        protected int       miBitData;
        protected bool      mbParity;

        public SpectraCyberBase()
        {
            // Init Datamembers:
        }


        // #### Methods #### //
        public int GetVersion()
        {
            return this.miVersion;
        }

        public void SetNoiseSourceStatus(bool bNoiseSourceOn)
        {
            
        }

        public bool GetNoiseSourceStatus()
        {
            return this.mbNoiseSourceStatus;
        }


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

    }
}
