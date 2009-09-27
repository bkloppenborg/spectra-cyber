using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCyber
{
    // Extends the List Class presumably to implement methods that would allow for long-term command
    // queuing... this would provide mode-switching commands and the methods to handle them.
    class cCommandList : List<cCommandItem>
    {
        protected System.Threading.Semaphore msemaNotSentItems;
        protected System.Threading.Semaphore msemaNotProcessedItems;

        public cCommandList()
        {
            // init the semaphore object so that they may hold one entire data run at a maximum.
            msemaNotSentItems = new System.Threading.Semaphore(0, 321);
            msemaNotProcessedItems = new System.Threading.Semaphore(0, 321);
        }

        public void Append(cCommandItem pCommand)
        {
            // Add the command to the queue
            base.Add(pCommand);

            // This is a new command, increment the Communication semaphore
            msemaNotSentItems.Release();
        }

        // Return a reference to the first unread item in the queue.
        public int GetFirstNotSentCommandItemID()
        {
            // Decrement the Semaphore (block the calling thread if the semaphore is fully decremented)
            msemaNotSentItems.WaitOne();

            // Loop through the list, find the first not-sent command
            for (int i = 0; i < this.Count; i++)
            {
                if (!(this[i].GetReadCommandStatus()))
                {
                    return i;
                }
            }

            // We will always find an unread command so we should never get to this spot (due to the 
            // way the Semaphore works) but this is required to keep the compiler from complaining.
            return 0;
        }

        // Get the cCommandItem object corresponding to the iItemInList-th item.
        public cCommandItem GetCommandItem(int iItemInList)
        {
            if (iItemInList <= this.Count)
                return this[iItemInList];

            return null;
        }

        // Return a reference to the first unread item in the queue.
        public string GetFirstUnprocessedReply()
        {
            // Decrement the Semaphore (block the calling thread if the semaphore is fully decremented)
            msemaNotProcessedItems.WaitOne();
            string strReply;

            // Loop through the list, find the first not-processed reply
            // Copy out the reply, remove the item, return the reply.
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].GetReplyStatus())
                {
                    strReply = this[i].GetReply();
                    this.RemoveAt(i);
                    return strReply;
                }
            }

            // We will always find an unprocessed reply so we should never get to this spot (due to the 
            // way the Semaphore works) but this is required to keep the compiler from complaining.
            return null;
        }

        // Return the number of unread items in the list.
        public int CountUnreadItems()
        {
            int iNumUnread = 0;

            for (int i = 0; i < this.Count; i++)
            {
                if (!(this[i].GetReadCommandStatus()))
                    iNumUnread++;
            }

            return iNumUnread;
        }

        // Set the reply for the iItemNum-th item (this method unblocks the reader thread)
        public void SetReply(int iItemNum, string strReply)
        {
            this[iItemNum].SetReply(strReply);
            msemaNotProcessedItems.Release();
        }
    }
}
