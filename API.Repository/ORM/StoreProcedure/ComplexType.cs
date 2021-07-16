using System;
namespace API.Repository
{
    public interface ISPComplexType
    {
    }

     
    #region Product
    public class Payment_Get_Result : ISPComplexType
    {
        public long RowNumber { get; set; }
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string OrderReference { get; set; }
        public short Status { get; set; }  
    }
     
    #endregion
 
}
