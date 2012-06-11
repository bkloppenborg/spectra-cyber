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
    enum eCommandType
    {
        Termination,            // Level 0 (Highest Priorty)
        Reset,     
        ChangeSetting,
        ScanStart, 
        ScanStop,
        Rescan,
        DataRequest,            
        FrequencySet,
        DataDiscard,
        GeneralCommunication
    }

    class cCommandItem
    {
        private int miTimeToNextCommand = 0;    //
        protected string mstrCommand;
        protected string mstrReply;
        protected bool mbCommandRead;
        protected int miWaitTime = 70;          // All commands take at least 70 milliseconds to process 
        protected bool mbReplySet;
        protected bool mbExpectReply;
        protected int miNumCharsToRead;
        protected eCommandType menCommandType;
        protected int miPriority;

        // Constructors:
        public cCommandItem()
        {
            mstrCommand = "";
            mstrReply = "";
            mbCommandRead = false;
            mbReplySet = false;
            miWaitTime = 0;
            mbExpectReply = false;
            miNumCharsToRead = 0;
            menCommandType = eCommandType.GeneralCommunication;
            miPriority = SetPriority();
        }

        public cCommandItem(string strCommand, int iTimeToNextCommand, bool bExpectReply, int iNumCharsToRead, eCommandType eCommandType)
        {
            mstrCommand = strCommand;
            mstrReply = "";
            mbCommandRead = false;
            mbReplySet = false;
            mbExpectReply = bExpectReply;

            if(iTimeToNextCommand > 0)
                miTimeToNextCommand = iTimeToNextCommand;

            if(iNumCharsToRead > 0)
                miNumCharsToRead = iNumCharsToRead;

            menCommandType = eCommandType;
            miPriority = SetPriority();
        }

        // Set the priority of this command
        protected int SetPriority()
        {
            switch (menCommandType)
            {
                // Cover the most standard cases first:
                case eCommandType.DataDiscard:
                case eCommandType.DataRequest:
                case eCommandType.FrequencySet:
                case eCommandType.GeneralCommunication:
                case eCommandType.Rescan:
                    return 5;
                    
                case eCommandType.ScanStart:
                case eCommandType.ScanStop:
                    return 3;
                    
                case eCommandType.ChangeSetting:
                    return 2;
                    
                case eCommandType.Reset:
                    return 1;
                    
                case eCommandType.Termination:
                    return 0;
                    
                default:
                    return 5;
                    
            }
        }

        // Find out whether or not this command has been read
        public bool CommandRead
        {
            get
            {
                return mbCommandRead;
            }
        }

        // Find out whether or not the reply has been set on this command
        public bool ReplySet
        {
            get
            {
                return mbReplySet;
            }
            
        }

        // Get the amount of time to wait between when the command is sent and the command should be read.
        public int CommandWaitTime
        {
            get
            { 
                return miWaitTime;
            }
           
        }

        public int TimeToNextCommand
        {
            get
            {
                return miTimeToNextCommand;
            }
        }

        public bool ExpectReply
        {
            get
            {
                return mbExpectReply;
            }
        }

        // Get the number of characters to read from the serial port
        public int NumCharactersToRead
        {
            get
            {
                return miNumCharsToRead;
            }
            
        }

        // Get or Set the Command
        public string Command
        {
            get
            {
                mbCommandRead = true;
                return mstrCommand;
            }
            set
            {
                mstrCommand = value;
            }
        }

        // Get or Set the Reply
        public string Reply
        {
            get
            {
                return mstrReply;
            }
            set
            {
                mbReplySet = true;
                mstrReply = value;
            }
        }

        // Get the command type
        public eCommandType CommandType
        {
            get
            {
                return menCommandType;
            }
        }

        // Ge the command's priority
        public int Priority
        {
            get
            {
                return miPriority;
            }
        }
    }
}
