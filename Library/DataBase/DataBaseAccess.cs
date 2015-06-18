using System;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;

namespace DigitalProduction.DataBase
{
	/// <summary>
	/// Summary description for DataBaseAccess.
	/// </summary>
	public class DataBaseAccess
	{

		#region Members / Variables.

		/// <summary>Data adapter.</summary>
		public OleDbDataAdapter					_dataadapter;

		/// <summary>Data set.</summary>
		public DataSet							DataSet;
		
		private OleDbConnection					_connection;
		private static string					_providerstring			= @"Provider=Microsoft.Jet.OLEDB.4.0";	
		private string							_databaselocation;
		private static string					_datasource				= ";Data Source=";
		private string							_connectionstring;
		private string							_table;

		#endregion
		
		#region Construction / Disposing.

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="DataBaseLocation">Location of database to connect to.</param>
		/// <param name="Table">Table in the database to connect to.</param>
		public DataBaseAccess(string DataBaseLocation, string Table)
		{
			_databaselocation = DataBaseLocation;
			_table = Table;
			_connectionstring = _providerstring + _datasource + _databaselocation;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
		}

		#endregion

		#region Connection.

		/// <summary>
		/// Creates a connection to a table and fills the dataset.
		/// </summary>
		/// <returns>True if connection succeeded, false otherwise.</returns>
		public bool GetDataConnection()
		{	
			try 
			{	 
				_connection = new OleDbConnection(_connectionstring);  		  
				_dataadapter = new OleDbDataAdapter("select * from " + _table, _connection);
				DataSet = new DataSet(); 
				//refreshes rows in the DataSet 			
				_dataadapter.Fill(DataSet, _table);
				_connection.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show("Database Could Not Connect.\nError: " + ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			// Connected ok.
			return true;
		}

		#endregion

		#region Static class functions.

		/// <summary>
		/// Get the names of the tables in the database.
		/// </summary>
		/// <param name="databasepath">Path to database.</param>
		/// <returns>An array of strings with the names of the tables.</returns>
		public static string[] GetTableNames(string databasepath)
		{
			try
			{
				OleDbConnection connection = new OleDbConnection(_providerstring + _datasource + databasepath);
				connection.Open();

				DataTable schematable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] {null, null, null, "TABLE"});

				string[] tablenames = new string[schematable.Rows.Count];

				//List the table name from each row in the schema table.
				for (int i = 0; i < schematable.Rows.Count; i++) 
				{
					tablenames[i] = schematable.Rows[i].ItemArray[2].ToString();
				}

				//Explicitly close - don't wait on garbage collection.
				connection.Close();

				return tablenames;
			}
			catch(Exception ex)
			{
				MessageBox.Show("Database Could Not Connect.\nError: " + ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return null;
			}
		}

		#endregion

	} // End class.
} // End namespace.