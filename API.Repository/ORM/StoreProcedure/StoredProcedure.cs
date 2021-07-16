using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoredProcedureEFCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class StoredProcedures : IStoredProcedures
    {
        private readonly AppDbContext db;
        private readonly IConfiguration configuration;

        public StoredProcedures(AppDbContext context, IConfiguration configuration)
        {
            db = context;
            this.configuration = configuration;
        }

     
        #region Payments Stored Procedure
        public async Task<(List<Payment_Get_Result> list, int totalRecords)> Payments_Get(int pageNumber, int pageSize, string sortBy)
        {
            int totalRecords = 0;
            SqlParameter[] sqlParameters =
                {
                    
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@OrderBy", sortBy), 
                    new SqlParameter("@TotalRecords", totalRecords),
                };

            var totalRecordsParam = sqlParameters.FirstOrDefault(x => x.ParameterName == "@TotalRecords");
            totalRecordsParam.Direction = ParameterDirection.Output;

            List<Payment_Get_Result> result = await db.ExecuteReaderSingleDSAsync<Payment_Get_Result>("Payments_Get", sqlParameters);
            return (result, totalRecordsParam.Value == null ? 0 : (int)totalRecordsParam.Value);
        }
         
        #endregion
 
    }
}
