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
using System.Data.OleDb;

namespace SpectraCyber
{
    class cDatabaseAccess
    {
        // Datamembers (with some initing):
        protected OleDbDataAdapter mdaDataAdaptor;
        protected OleDbConnection mdbconnDBConnection;
        protected OleDbCommand mdbCommand;
        protected int miAffectedRows;

        protected string mstrDataSource;

        public cDatabaseAccess(string strDataSource)
        {
            mstrDataSource = strDataSource;
            mdbconnDBConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strDataSource);
            miAffectedRows = 0;
        }

        // Run an Insert SQL Statement
        public void Insert(string strSQL)
        {
            // Zero out the affected rows, create the command.
            miAffectedRows = 0;
            mdbCommand = new OleDbCommand(strSQL, mdbconnDBConnection);

            // Execute the command
            miAffectedRows = mdbCommand.ExecuteNonQuery();

            // Dispose of the command
            mdbCommand.Dispose();
        }

        // Run an Update SQL statement.
        public void Update(string strSQL)
        {
            // For MS Access databases, and update and an insert use the ExecuteNonQuery() function.
            Insert(strSQL);
        }

        public void FillDataTable(System.Data.DataTable dtDataTable, string strSQL)
        {
            dtDataTable.Clear();
            mdaDataAdaptor = new OleDbDataAdapter(strSQL, mdbconnDBConnection);
            mdaDataAdaptor.Fill(dtDataTable);
            mdaDataAdaptor.Dispose();
        }

        public void FillDataGrid(System.Windows.Forms.DataGrid dgDataGrid, System.Data.DataTable dtDataTable, string strSQL)
        {
            FillDataTable(dtDataTable, strSQL);
            dgDataGrid.DataSource = dtDataTable;
        }

        // Open the connection to the database
        public void Open()
        {
            mdbconnDBConnection.Open();
        }

        // Close the connection to the database
        public void Close()
        {
            mdbconnDBConnection.Close();
        }

        // Get the number of rows affected by the previous query.
        public int AffectedRows
        {
            get
            {
                return miAffectedRows;
            }
        }
    }
}
