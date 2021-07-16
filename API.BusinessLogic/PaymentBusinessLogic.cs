
using API.Model;
using API.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using static API.Common.Enumeration;

namespace API.BusinessLogic
{
    public partial class ServiceBusinessLogic : IServiceBusinessLogic
    {
        public async Task<AuthorizationResponseDto> AuthorizeTransaction(AuthorizationRequestDto dto)
        {
            Payment payment = new()
            {
                Amount = dto.Amount,
                CardHolderName = dto.CreditCard.HolderName,
                CardNumber = dto.CreditCard.CardHolderNumber,
                ExpiryMonth = dto.CreditCard.ExpiryMonth,
                ExpiryYear = dto.CreditCard.ExpiryYear,
                Cvv = dto.CreditCard.CVV,
                Currency = dto.Currency.ToUpper(),
                Status = (short)PaymentStatus.Authorized,
                OrderReference = dto.OrderRef,
                CreatedOn = System.DateTime.Now
            };
            db.Payments.Add(payment);
            db.SaveChanges();

            AuthorizationResponseDto responseDto = new()
            {
                Id = payment.PaymentId,
                Status = ((PaymentStatus)payment.Status).ToString()
            };
            return responseDto;
        }

        public async Task<AuthorizationResponseDto> UpdateTransaction(Guid id, UpdateTransactionRequestDto dto, PaymentStatus paymentStatus)
        {
            var payment = db.Payments.FirstOrDefault(c => c.PaymentId == id);
            if (payment != null)
            {
                payment.Status = (short)paymentStatus;
                payment.OrderReference = dto.OrderRef;
                payment.ModifiedOn = System.DateTime.Now;
                db.Entry(payment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                throw new System.Exception("Invalid Payment Id");
            }

            AuthorizationResponseDto responseDto = new()
            {
                Id = payment.PaymentId,
                Status = ((PaymentStatus)payment.Status).ToString()
            };
            return responseDto;
        }

        public async Task<TransactionResponseDto> GetTransactions(PaginationRequestDto paginationRequestDto)
        {
            var queryResult = await storedProcedures.Payments_Get(paginationRequestDto.PageNumber, paginationRequestDto.PageSize, "");

            var list = queryResult.list.Select(s => new TransactionDto()
            {
                CardHolderNumber = s.CardNumber,
                HolderName = s.CardHolderName,
                OrderRef = s.OrderReference,
                PaymentId = s.PaymentId,
                Status = ((PaymentStatus)s.Status).ToString(),
                Amount = s.Amount,
                Currency = s.Currency
            }).ToList();

            TransactionResponseDto authorizationResponseDto = new()
            {
                TotalRecords = queryResult.totalRecords,
                transactionDtos = list
            };

            return authorizationResponseDto;
        }

    }
}
