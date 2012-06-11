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
using System.Threading;
using System.IO.Ports;
using System.Data.OleDb;

namespace SpectraCyber
{
    class cCommunication
    {
        // Datamembers:
        protected string    mstrCommPort;
        protected int       miBaudRate;
        protected Parity    mparParity;
        protected int       miDataBits;
        protected SerialPort mrSerialPort;
        protected int       miCharInputBufferSize;
        protected Thread    mCommunicationThread;
        protected Thread    mReaderThread;
        protected Thread    mDatabaseThread;
        protected cSpectraCyberBase mscSpectraCyber;
        protected int       miTimeout;

        // The Queues
        protected cPriorityQueue mCommandQueue;
        protected cPriorityQueue mReplyQueue;
        protected cDatabaseQueue mDatabaseQueue;

        // Database access items:
        System.Data.DataTable mdtDataTable;

        // Delegates
        public delegate void SetCommandWindowDelegate(string strText);

        public cCommunication()
        {
            // Init the queues
            mCommandQueue = new cPriorityQueue();
            mReplyQueue = new cPriorityQueue();
            mDatabaseQueue = new cDatabaseQueue();

            // Init defaults:
            mstrCommPort = "";
            miBaudRate = 2400;
            miDataBits = 8;
            mparParity = System.IO.Ports.Parity.None;
            miCharInputBufferSize = 7;
            miTimeout = 1000;
        }

        // Get or Set the SpectraCyber to which this command object belongs
        public cSpectraCyberBase SpectraCyber
        {
            get
            {
                return mscSpectraCyber;
            }
            set
            {
                mscSpectraCyber = value;
            }
        }

        // Get or Set the command queue
        public cPriorityQueue CommandQueue
        {
            get
            {
                return mCommandQueue;
            }
            set
            {
                mCommandQueue = value;
            }
        }

        // Get or Set the Reply Queue
        public cPriorityQueue ReplyQueue
        {
            get
            {
                return mReplyQueue;
            }
            set
            {
                mReplyQueue = value;
            }
        }

        // Get or Set the Database Queue
        public cDatabaseQueue DatabaseQueue
        {
            get
            {
                return mDatabaseQueue;
            }
            set
            {
                mDatabaseQueue = value;
            }
        }

        // Get or set the buffer size.
        public int BufferSize
        {
            get
            {
                return miCharInputBufferSize;
            }
            set
            {
                miCharInputBufferSize = value;
            }
        }

        // Get or Set the Comm port
        public string CommPort
        {
            get
            {
                return mstrCommPort;
            }
            set
            {
                // Close the comm port, change the comm port, re-open the comm port, reset the unit.
                mstrCommPort = value;
            }
        }

        // Get or set the Baud Rate
        public int BaudRate
        {
            get
            {
                return miBaudRate;
            }
            set
            {
                miBaudRate = value;
            }
        }

        // Get or set the Databits
        public int DataBits
        {
            get
            {
                return miDataBits;
            }
            set
            {
                miDataBits = value;
            }
        }

        // Get or set the parity
        public System.IO.Ports.Parity Parity
        {
            get
            {
                return mparParity;
            }
            set
            {
                mparParity = value;
            }
        }

        public void Connect()
        {
            // Init the communication thread
            ThreadStart tdComm = new ThreadStart(CommunicationThread);
            mCommunicationThread = new Thread(tdComm);

            // Init the reader thread
            ThreadStart tdRead = new ThreadStart(ReaderThread);
            mReaderThread = new Thread(tdRead);

            // Init the database thread
            ThreadStart tdDatabase = new ThreadStart(DatabaseThread);
            mDatabaseThread = new Thread(tdDatabase);

            // Reduce / Clear the queues
            mCommandQueue.Reduce();
            mReplyQueue.Clear();

            // Start the Threads
            mCommunicationThread.Start();
            mReaderThread.Start();
            mDatabaseThread.Start();
       }

        public void Disconnect()
        {
            // Stop the Threads
        }

        public void Reset()
        {
            // Clear the queues
            mCommandQueue.Clear();
            mReplyQueue.Clear();
        }

        // #### Thread functions #### //
        public void CommunicationThread()
        {
            string strCommand;

            // Init the serial port, set the timeout value, open the port.
            mrSerialPort = new System.IO.Ports.SerialPort(mstrCommPort, miBaudRate, mparParity, miDataBits);
            mrSerialPort.ReadTimeout = miTimeout;
            mrSerialPort.Open();

            for (; ; )
            {
                // Get the first unread item off of the queue (which blocks this thread if no item exists)
                cCommandItem oCommandItem = mCommandQueue.GetFirstItem();
                strCommand = oCommandItem.Command;

                // Break out of the loop if a eCommandType.Termination command is sent
                if (oCommandItem.CommandType == eCommandType.Termination)
                {
                    // Copy the command into the reply for this queue item.
                    oCommandItem.Reply = strCommand;
                    mReplyQueue.Add(oCommandItem);
                    break;
                }
                else if (oCommandItem.CommandType == eCommandType.ScanStop)
                {
                    // Let the other thread know to stop
                    oCommandItem.Reply = strCommand;
                    mReplyQueue.Add(oCommandItem);

                    // Clear the queue
                    mCommandQueue.Clear();
                }

                // Write the command to the serial port
                mrSerialPort.Write(strCommand);

                // If we are to discard any reply in the buffer.
                if (oCommandItem.CommandType == eCommandType.DataDiscard)
                    mrSerialPort.DiscardInBuffer();

                // Temporary: Set the command window text:
                mscSpectraCyber.SetCommandWindowText(strCommand);

                // Give the SpectraCyber some time to process the command.
                System.Threading.Thread.Sleep(oCommandItem.CommandWaitTime);
                
                // Handle termination first
                if (oCommandItem.ExpectReply)
                {
                    string strReply = "";
                    
                    try
                    {
                        // Create a character array in which to store the buffered characters
                        char[] carrInBuffer = new char[miCharInputBufferSize];

                        // Read characters from the input buffer
                        mrSerialPort.Read(carrInBuffer, 0, oCommandItem.NumCharactersToRead);
                        foreach (char cCharacter in carrInBuffer)
                            strReply += cCharacter;

                        // Clear the input buffer
                        mrSerialPort.DiscardInBuffer();
                    }
                    catch (TimeoutException e)
                    {
                        System.Windows.Forms.MessageBox.Show("Communication timeout reached");
                        strReply = "COMM_TIMEOUT_ERROR";
                    }

                    // Set the reply
                    oCommandItem.Reply = strReply;
                }
                else // Remove the first character from the command string and use that as the reply.
                {
                    oCommandItem.Reply = strCommand.Remove(0, 1);
                }

                // Now, move the command to the other queue
                mReplyQueue.Add(oCommandItem);

                // Finally, sleep until the next command should be processed
                System.Threading.Thread.Sleep(oCommandItem.TimeToNextCommand);
            }
            
            // The thread is done running. Close the connection to the serial port
            mrSerialPort.Close();
        }

        public void ReaderThread()
        {
            for (; ; )
            {
                // Get the first unread reply off of the queue, send that to the SpectraCyber object for processing
                cCommandItem oCommandItem = mReplyQueue.GetFirstItem();
                mscSpectraCyber.ProcessReply(oCommandItem.Reply, oCommandItem.CommandType);

                // The program is closing, gracefully stop this thread
                if (oCommandItem.CommandType == eCommandType.Termination)
                {
                    // Tell the Database I/O Thread to stop as well.
                    cDatabaseItem dbiCommand = new cDatabaseItem(eDBCommandType.Terminate);
                    mDatabaseQueue.Add(dbiCommand);
                    break;
                }
            }

            // The thread is done running, empty out the command queue
            mCommandQueue.Clear();
        }

        public void DatabaseThread()
        {
            // init vars:
            int iScanID = 0;        // The current Scan number... basically which scan are we doing.
            int iSettingID = 0;    // The current Settings number... which settings are we using
            string strSQL;
            cDatabaseAccess pDBAccess = new cDatabaseAccess("./SCData.mdb");
            System.Data.DataTable pDataTable = new System.Data.DataTable();
            pDBAccess.Open();

            for (; ; )
            {
                // Clear out some vars
                strSQL = "";

                // Pull the first item off of the database queue
                cDatabaseItem pItem = mDatabaseQueue.GetFirstItem();

                if (pItem.Type == eDBCommandType.Data)
                {
                    // Build a query to insert the data.
                    strSQL = "INSERT INTO tData (fSettingsID, " + pItem.Fields + ") " +
                        "VALUES (" + iSettingID.ToString() + ", " + pItem.Values + ")";

                    pDBAccess.Insert(strSQL);
                } 
                else if (pItem.Type == eDBCommandType.Settings)
                {
                    // Build the query
                    strSQL = "INSERT INTO tSettings (fScanID, " + pItem.Fields + ") VALUES (" + iScanID.ToString() + ", " + pItem.Values + ")";

                    // Run the insert statment
                    pDBAccess.Insert(strSQL);

                    // Now, build another query to find the setting ID
                    // Build another query to get the row number
                    strSQL = "SELECT MAX(fSettingID) AS SettingID FROM tSettings";

                    // Get the scan number
                    pDBAccess.FillDataTable(pDataTable, strSQL);
                    iSettingID = Convert.ToInt32(pDataTable.Rows[0]["SettingID"]);
                }
                else if (pItem.Type == eDBCommandType.ScanStart)
                {
                    strSQL = "INSERT INTO tScans (fStartTime) VALUES (NOW())";

                    // Run the insert statement
                    pDBAccess.Insert(strSQL);

                    // Build another query to get the row number
                    strSQL = "SELECT MAX(fScanID) AS ScanID FROM tScans";

                    // Get the scan number
                    pDBAccess.FillDataTable(pDataTable, strSQL);
                    iScanID = Convert.ToInt32(pDataTable.Rows[0]["ScanID"]);
                }
                else if (pItem.Type == eDBCommandType.ScanStop)
                {
                    // Create an update query
                    strSQL = "UPDATE tScans SET fEndTime = NOW() WHERE fScanID = " + iScanID.ToString();
                    pDBAccess.Update(strSQL);
                }
                else if (pItem.Type == eDBCommandType.Terminate)
                {
                    break;
                }
                else
                {
                    // This is an unsupported type... the programmer needs to implement it
                    System.Windows.Forms.MessageBox.Show("This type of database command type is not supported, it must be implmented before it is used.");
                    break;
                }
            }

            // Close the connection to the database
            pDBAccess.Close();
        }
    }
}
