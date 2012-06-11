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
    class cQueueItem
    {
        protected string mstrCommand;
        protected string mstrReply;
        protected bool mbCommandRead;
        protected bool mbExpectReply;
        protected bool mbReplySet;

        // Constructors:
        public cQueueItem()
        {
            mstrCommand = "";
            mstrReply = "";
            mbCommandRead = false;
            mbReplySet = false;
            mbExpectReply = true;
        }

        public cQueueItem(string strCommand, bool bExpectReply)
        {
            mstrCommand = strCommand;
            mstrReply = "";
            mbCommandRead = false;
            mbReplySet = false;
            mbExpectReply = bExpectReply;
        }

        // Get the Command String
        public string GetCommand()
        {
            mbCommandRead = true;
            return mstrCommand;
        }

        // Get the Reply
        public string GetReply()
        {
            return mstrReply;
        }

        // Set the Reply string
        public void SetReply(string strReply)
        {
            mbReplySet = true;
            mstrReply = strReply;
        }

        // Find out whether or not this command has been read
        public bool GetReadCommandStatus()
        {
            return mbCommandRead;
        }

        // Find out whether or not the reply has been set on this command
        public bool GetReplyStatus()
        {
            return mbReplySet;
        }
    }
}
