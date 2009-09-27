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
