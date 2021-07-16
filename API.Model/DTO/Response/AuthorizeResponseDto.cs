

using API.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class AuthorizationResponseDto
    {
        /// <summary>
        /// Payment ID, received in authorization response. Route parameter.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Order Reference, Max Length 50
        /// </summary>
        [Display(Name = "Order Reference")]
        public string Status { get; set; }
    }

    public class TransactionDto
    {
        /// <summary>
        /// Payment ID, received in authorization response. Route parameter.
        /// </summary>
        public Guid PaymentId { get; set; }

        /// <summary>
        /// Holder Name
        /// </summary>
        [Display(Name = "Holder Name")]
        public string HolderName { get; set; }

        /// <summary>
        /// Card Number
        /// </summary>
        [Display(Name = "Card Number")]
        public string CardHolderNumber { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        /// <summary>
        /// Order Reference, Max Length 50
        /// </summary>
        [Display(Name = "Order Reference")]
        public string OrderRef { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }

    public class TransactionResponseDto
    {
        public int TotalRecords { get; set; }

        [Display(Name = "Transactions")]
        public List<TransactionDto> transactionDtos { get; set; }
    }

}
