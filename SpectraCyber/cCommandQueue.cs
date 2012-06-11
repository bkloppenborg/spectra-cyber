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
    class cCommandQueue : List<cCommandItem>
    {
        protected System.Threading.Semaphore msemaNotProcessed;
        //protected System.Threading.Mutex mutListAccess;

        public cCommandQueue()
        {
            // init the semaphore object so that they may hold one entire data run at a maximum.
            msemaNotProcessed = new System.Threading.Semaphore(0, int.MaxValue);
        }

        new public void Add(cCommandItem pCommand)
        {
            // Add the command to the queue
            base.Add(pCommand);

            // This is a new command, increment the Communication semaphore
            msemaNotProcessed.Release();
        }

        // Return a reference to the first unread item in the queue.
        public cCommandItem GetFirstItem()
        {
            // Decrement the Semaphore (block the calling thread if the semaphore is fully decremented)
            msemaNotProcessed.WaitOne();

            // Return the first cCommandItem
            return this[0];
        }
    }
}
