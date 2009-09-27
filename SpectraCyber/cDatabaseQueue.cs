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
