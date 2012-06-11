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
    class cDatabaseQueue : List<cDatabaseItem>
    {
        System.Threading.Semaphore msemaNotProcessed;

        public cDatabaseQueue()
        {
            msemaNotProcessed = new System.Threading.Semaphore(0, int.MaxValue);
        }

        // Add the item to the queue, increment the semaphore
        new public void Add(cDatabaseItem pItem)
        {
            base.Add(pItem);
            msemaNotProcessed.Release();
        }

        // Return the first cDBInsertItem in the list
        public cDatabaseItem GetFirstItem()
        {
            // Init vars
            cDatabaseItem pItem;

            // Block the thread until an item is in the queue
            msemaNotProcessed.WaitOne();

            // Get a reference to the item, remove it from the list, return the first item in the queue.
            pItem = this[0];
            Remove(pItem);
            return pItem;
        }
    }
}
