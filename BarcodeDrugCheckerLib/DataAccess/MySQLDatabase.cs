using BarcodeDrugCheckerLib.DataAccess.Interface;
using System.Data;
using MySqlConnector;

namespace BarcodeDrugCheckerLib.DataAccess
{
    public class MySQLDatabase : IDatabase
    {
        private IDbConnection _Connection = null;
        private IDbCommand _Command = null;

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
        public IDbCommand Command
        {
            get {
                /*
                if(_Command == null)
                {

                }
                */
                _Command = new MySqlCommand();
                _Command.Connection = Connection;
                return _Command;
            }
            set => _Command = value;
        }
    }
}