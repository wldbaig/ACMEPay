using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repository
{
    public interface IStoredProcedures
    { 
        Task<(List<Payment_Get_Result> list, int totalRecords)> Payments_Get(int pageNumber, int pageSize, string sortBy);
    }
}
