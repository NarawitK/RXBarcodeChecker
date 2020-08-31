using BarcodeDrugCheckerLib.DataAccess.Interface;
using System.Data;
using MySql.Data.MySqlClient;

namespace BarcodeDrugCheckerLib.DataAccess
{
    public class MySQLDatabase : IDatabase
    {
        private IDbConnection _Connection = null;

        public IDbConnection Connection 
        {
            get 
            {
                /*
                if(_Connection == null)
                {

                }
                */
                string conString = DBConnector.GetConnectionString("mysql");
                    _Connection = new MySqlConnection(conString);
                return _Connection;
            }
        }
    }
}