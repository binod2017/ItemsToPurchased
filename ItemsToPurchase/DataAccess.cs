using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace ItemsToPurchase
{
    public static class DataAccess
    {
        /// <summary>
        /// Connection string
        /// </summary>
        private static string ConnectionString
        {
            get
            {
                return ConfigurationManager
                    .ConnectionStrings["ItemPurchasingConnectionString"]
                    .ToString();
            }
        }
        /// <summary>
        /// Get the last Id No
        /// </summary>
        /// <returns></returns>
        internal static DataTable GetLastId()
        {
            DataTable dataTable = new DataTable();
            using (OleDbDataAdapter OleDbDbDataAdapter = new OleDbDataAdapter())
            {
                // Create the command and set its properties
                OleDbDbDataAdapter.SelectCommand = new OleDbCommand();
                OleDbDbDataAdapter.SelectCommand.Connection = new OleDbConnection(ConnectionString);
                OleDbDbDataAdapter.SelectCommand.CommandType = CommandType.Text;
                // Assign the SQL to the command object
                OleDbDbDataAdapter.SelectCommand.CommandText = " SELECT COUNT(*) From [Purchasing$]";
                // Fill the datatable from adapter
                OleDbDbDataAdapter.Fill(dataTable);
            }
            return dataTable;
        }
        /// <summary>
        /// Get all the items from DB
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllItems()
        {
            DataTable dataTable = new DataTable();

            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter())
            {
                // Create the command and set its properties
                dataAdapter.SelectCommand = new OleDbCommand();
                dataAdapter.SelectCommand.Connection = new OleDbConnection(ConnectionString);
                dataAdapter.SelectCommand.CommandType = CommandType.Text;
                dataAdapter.SelectCommand.CommandText = "SELECT * FROM [Purchasing$]";

                // Fill the datatable From adapter
                dataAdapter.Fill(dataTable);

                return dataTable;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchasing"></param>
        /// <returns></returns>
        internal static bool AddItems(Purchasing purchasing)
        {
            var rowsAffected = 0;

            using (OleDbCommand dbCommand = new OleDbCommand())
            {
                // Set the command object properties
                dbCommand.Connection = new OleDbConnection(ConnectionString);
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = "Insert Into [Purchasing$] (SlNo, ItemName, Purchased, DateCreated, DateModified)" +
                                        " Values (@slno, @itemName, @purchased, @dateCreated, @dateModified)";

                // Update the input parameters to the parameter collection
                dbCommand.Parameters.AddWithValue("@slno", purchasing.SlNo);
                dbCommand.Parameters.AddWithValue("@itemName", purchasing.ItemName);
                dbCommand.Parameters.AddWithValue("@purchased", purchasing.Purchased);
                dbCommand.Parameters.AddWithValue("@dateCreated", purchasing.DateCreated.ToShortDateString());
                dbCommand.Parameters.AddWithValue("@dateModified", purchasing.DateModified.ToShortDateString());


                // Open the connection, execute the query and close the connection
                dbCommand.Connection.Open();
                rowsAffected = dbCommand.ExecuteNonQuery();
                dbCommand.Connection.Close();
            }
            return rowsAffected > 0;
        }
        public static bool ActiveDeactive(bool purchased, DateTime dateModified, int slno)
        {
            using (OleDbCommand dbCommand = new OleDbCommand())
            {
                // Set the command object properties
                dbCommand.Connection = new OleDbConnection(ConnectionString);
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = "Update [Purchasing$] Set [Purchased] = @purchased, [DateModified] = @dateModified Where ([SlNo] = @slno)";
 
                dbCommand.Parameters.AddWithValue("@purchased", purchased);
                dbCommand.Parameters.AddWithValue("@dateModified", dateModified.ToShortDateString());
                dbCommand.Parameters.AddWithValue("@slno", slno);
                // Open the connection, execute the query and close the connection
                dbCommand.Connection.Open();
                var rowsAffected = dbCommand.ExecuteNonQuery();
                dbCommand.Connection.Close();

                return rowsAffected > 0;
            }
        }

    }
}
