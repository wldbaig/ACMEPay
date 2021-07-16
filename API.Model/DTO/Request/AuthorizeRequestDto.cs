using System;
using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class UpdateTransactionRequestDto
    { 
        /// <summary>
        /// Order Reference, Max Length 50
        /// </summary>
        [Display(Name = "Order Reference")]
        [Required]
        [MaxLength(50, ErrorMessage = "Length cannot be greater than 50")]
        public string OrderRef { get; set; }
    }

    public class PaginationRequestDto
    {
        /// <summary>
        /// page size
        /// </summary>
        [Required]
        public int PageSize { get; set; }

        /// <summary>
        /// page number
        /// </summary>
        [Required]
        public int PageNumber { get; set; }

        public string OrderBy { get; set; }

        public PaginationRequestDto()
        {
            PageNumber = 1;
            PageSize = 10; 
        }
    }

    public class AuthorizationRequestDto
    {
        /// <summary>
        /// Payment Amount
        /// </summary>
        [Required]
        public decimal Amount { get; set; }
        /// <summary>
        /// Payment Currency
        /// Max Length 3 Character
        /// </summary>

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Length should be equal to 3")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Currency { get; set; }

        /// <summary>
        /// Order Reference, Max Length 50
        /// </summary>
        [Display(Name ="Order Reference")]
        [Required]
        [MaxLength(50, ErrorMessage = "Length cannot be greater than 50")]
        public string OrderRef { get; set; }
        public CreditCardDto CreditCard { get; set; }

    }


    public class CreditCardDto
    {
        /// <summary>
        /// Card Holder Name
        /// </summary>
        [Required]
        [Display(Name = "Holder Name")]
        [MaxLength(50, ErrorMessage = "Length cannot be greater than 50")]
        public string HolderName { get; set; }

        /// <summary>
        /// PAN- Card Number
        /// </summary>
        [Required]
        [MaxLength(16, ErrorMessage = "Length cannot be greater than 16")]
        [DataType(DataType.CreditCard)]
        [Display(Name = "Card Number")]
        public string CardHolderNumber { get; set; }

        /// <summary>
        /// Card Expiry Month between 1-12
        /// </summary>
        [Required]
        [Display(Name = "Expiry Month")]
        [Range(1, 12, ErrorMessage = "Expiry month should be between 1-12")]
        public short ExpiryMonth { get; set; }

        /// <summary>
        /// Card Expiry Year between 2021-2031
        /// </summary>
        [Required]
        [Display(Name = "Expiry Year")]
        [Range(2021, 2031, ErrorMessage = "Expiry year should be between 2021-2031")]
        public short ExpiryYear { get; set; }

        /// <summary>
        /// Card verification value, should be 3 digits
        /// </summary>
        [Required]
        [RegularExpression(@"^([0-9]{3,4})$", ErrorMessage = "Invalid CVV")]
        public short CVV { get; set; }
    }
}
