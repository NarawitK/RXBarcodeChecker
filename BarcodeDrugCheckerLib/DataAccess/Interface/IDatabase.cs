using System.Data;

namespace BarcodeDrugCheckerLib.DataAccess.Interface
{
    public interface IDatabase
    {
        IDbConnection Connection { get; }
        
        IDbCommand Command { get; }
    }
}
