
using API.Model;
using System;
using System.Threading.Tasks;
using static API.Common.Enumeration;

namespace API.BusinessLogic
{
    public interface IServiceBusinessLogic
    {
        #region Payment Services
        Task<AuthorizationResponseDto> AuthorizeTransaction(AuthorizationRequestDto dto);

        Task<AuthorizationResponseDto> UpdateTransaction(Guid id, UpdateTransactionRequestDto dto, PaymentStatus paymentStatus); 

        Task<TransactionResponseDto> GetTransactions(PaginationRequestDto paginationRequestDto);
        #endregion

    }
}
