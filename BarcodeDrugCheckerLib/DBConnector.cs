using BarcodeDrugCheckerLib.DataAccess;
using BarcodeDrugCheckerLib.DataAccess.Interface;
using System;
using System.Configuration;

namespace BarcodeDrugCheckerLib
{
    public static class DBConnector
    {
        public static DatabaseType currentDBType;
       // public static string Phase = ConfigurationManager.AppSettings["data-stage"];
        public static IDatabase InitConnection()
        {
            Enum.TryParse(ConfigurationManager.AppSettings["db-use"], out currentDBType);
            return DatabaseFactory.getDatabase(currentDBType);
        }
        
        public static string GetConnectionString(string name)
        {
            string Phase = ConfigurationManager.AppSettings["data-stage"];
            string combinedCoonectionStrings = $"{name}-{Phase}";
            return ConfigurationManager.ConnectionStrings[combinedCoonectionStrings].ConnectionString;
        }
    }
}
