using BarcodeDrugCheckerLib.DataAccess.Interface;

namespace BarcodeDrugCheckerLib.DataAccess
{
    public class DatabaseFactory
    {
        public static IDatabase getDatabase(DatabaseType DBType)
        {
            return new MySQLDatabase();
            /*
             * Multiple Database Support if needed.
            switch (DBType)
            {
                case DatabaseType.mysql:
                    return new MySQLDatabase();
                case DatabaseType.sqlserver:
                default:
                    return new SQLServerDatabase();
            }
            */
        }
    }
}