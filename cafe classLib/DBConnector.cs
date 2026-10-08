using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace cafe_classLib
{
     class DBConnector
    {
        private SqlConnection _conn;

        public void ConnectToDatabase()
        {
            string connectionString =
    "Data Source=mssql9.unoeuro.com;" +
    "Initial Catalog=vilken_enterprises_dk_db_DBMS;" +
    "User ID=vilken_enterprises_dk;" +
    "Password=Vilken2720;" +
    "Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;";


            _conn = new SqlConnection(connectionString);

            _conn.Open();
        }
        public void DisconnectFromDatabase()
        {
            if (_conn != null)
            {
                _conn.Close();
            }
        }
    }
}
