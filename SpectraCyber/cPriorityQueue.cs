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
    // Extends the List Class presumably to implement methods that would allow for long-term command
    // queuing... this would provide mode-switching commands and the methods to handle them.
    class cPriorityQueue : List<cCommandItem>
    {
        protected System.Threading.Semaphore msemaNotProcessed;
        //protected System.Threading.Mutex mutListAccess;

        public cPriorityQueue()
        {
            // init the semaphore object so that they may hold one entire data run at a maximum.
            msemaNotProcessed = new System.Threading.Semaphore(0, int.MaxValue);
        }

        new public void Add(cCommandItem pCommand)
        {
            // Init vars
            bool bInsertedCommand = false;

            // If there are elements in the list, compare their priority to the new commands's priorty
            // Insert pCommand into the correct location in the list
            for (int i = 0; i < this.Count; i++)
            {
                if (pCommand.Priority < this[i].Priority)
                {
                    base.Insert(i, pCommand);
                    bInsertedCommand = true;
                    break;
                }
            }

            // Otherwise, just append the command to the list.
            if (!(bInsertedCommand))
                base.Add(pCommand);

            // This is a new command, increment the Communication semaphore
            msemaNotProcessed.Release();
        }

        // Return a reference to the first unread item and remove it from the queue
        public cCommandItem GetFirstItem()
        {
            // Decrement the Semaphore (block the calling thread if the semaphore is fully decremented)
            msemaNotProcessed.WaitOne();

            cCommandItem oCommandItem = this[0];

            Remove(oCommandItem);

            // Return the first cCommandItem
            return oCommandItem;
        }

        // Reduce the command list to the minimum number of elements
        public void Reduce()
        {
            string strCommand;
            List<char> clistCommandStartChars = new List<char>();

            // First, loop through the queue backwards and remove any commands that do a similar function
            for (int i = (Count - 1); i >= 0; i--)
            {
                strCommand = this[i].Command;

                // If this type of a command is in the list, we should remove any earlier instances of this command
                if (clistCommandStartChars.IndexOf(strCommand[1]) != -1)
                {
                    RemoveAt(i);
                    msemaNotProcessed.WaitOne();
                }
                else       // Otherwise, append this type of a command to the list
                {
                    clistCommandStartChars.Add(strCommand[1]);
                }       
            }
        }

        // Clear out the command queue
        new public void Clear()
        {
            // Decrement the semaphore
            for (int i = 0; i < Count; i++)
                msemaNotProcessed.WaitOne();
                
            // Clear the queue
            base.Clear();

        }

    }
}
