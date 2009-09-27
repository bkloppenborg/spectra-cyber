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
