using BarcodeDrugCheckerLib.DataAccess.Interface;
using BarcodeDrugCheckerLib.Model;
using BarcodeDrugCheckerLib.Model.Interface;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BarcodeDrugCheckerLib.DataAccess.QueryBuilder
{
    public class DBHandler
    {
        private IDatabase Db { get; set; }

        public DBHandler()
        {
            Db = DBConnector.InitConnection();
        }

        public bool ReloadConnection()
        {
            try
            {
                Db = null;
                Db = DBConnector.InitConnection();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetDBConnectionStatus()
        {
            return Db.Connection.State.ToString();
        }

        public bool TestOpenConnection()
        {
            using (IDbConnection db = Db.Connection)
            {
                try
                {
                    db.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }  
        }

        public async Task<IPatient> GetPatient(string hn)
        {
            using (IDbConnection conn = Db.Connection)
            {
                var param = new DynamicParameters();
                param.Add("@PatientHN", hn);
                IPatient result = await conn.QueryFirstOrDefaultAsync<Patient>("sp_GetPatient", param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<List<DrugVisit>> GetDrugListfromPatient(string hn)
        {
            using (IDbConnection conn = Db.Connection)
            {
                string storeProcName = "sp_PullPatientDrugList";
                DynamicParameters param = new DynamicParameters();
                param.Add("@PatientHN", hn);
                var result = await conn.QueryAsync<DrugVisit>(storeProcName, param, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<IDrugImage> GetDrugImage(string icode)
        {
            using (IDbConnection conn = Db.Connection)
            {
                string storeProcName = "sp_GetDrugImage";
                DynamicParameters param = new DynamicParameters();
                param.Add("@ICode", icode);
                var result = await conn.QueryFirstOrDefaultAsync<DrugImage>(storeProcName, param, commandType: CommandType.StoredProcedure);
                return result; 
            }
        }

        public async Task<List<PatientAllergy>> GetPatientAllergy(string hn)
        {
            using (IDbConnection conn = Db.Connection)
            {
                string storeProcName = "sp_GetPatientAllergy";
                DynamicParameters param = new DynamicParameters();
                param.Add("@PatientHN", hn);
                var result = await conn.QueryAsync<PatientAllergy>(storeProcName, param, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<DrugInteraction>> GetDrugInteractionList()
        {
            using (IDbConnection conn = Db.Connection)
            {
                string storeProcName = "sp_GetAllDrugInteraction";
                var result = await conn.QueryAsync<DrugInteraction>(storeProcName, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}
