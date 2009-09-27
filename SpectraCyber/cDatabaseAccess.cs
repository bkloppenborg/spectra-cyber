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
