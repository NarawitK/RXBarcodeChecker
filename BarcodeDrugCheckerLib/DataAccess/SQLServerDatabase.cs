using System.Data;
using System.Data.SqlClient;
using BarcodeDrugCheckerLib.DataAccess.Interface;

namespace BarcodeDrugCheckerLib.DataAccess
{
    public class SQLServerDatabase : IDatabase
    {
        private IDbConnection _Connection = null;

        public IDbConnection Connection
        {
            get
            {
                /*
                if (_Connection == null)
                {
                */
                string conString = DBConnector.GetConnectionString("mssql");
                    _Connection = new SqlConnection(conString);
                //}
                return _Connection;
            }
        }
    }
}
