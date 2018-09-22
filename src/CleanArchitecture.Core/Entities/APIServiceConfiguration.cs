using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class APIServiceConfiguration : BizBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long APIServiceID { get; set; }

        [Required]
        [StringLength(30)]
        public string APIName { get; set; }

        [Required]
        [Url]
        public string APISendURL { get; set; }
       
        [Url]
        public string APIValidateURL { get; set; }
       
        [Url]
        public string APIBalURL { get; set; }

        [Url]
        public string APIStatusCheckURL { get; set; }
       
        public string TransactionIdPrefix { get; set; }

        public string MerchantCode { get; set; }

        public string UserID { get; set; }

        public string Password { get; set; }

        public string ResponseSuccess { get; set; }

        public string ResponseFailure { get; set; }
              
        public string ResponseHold { get; set; }

        public string AuthHeader { get; set; }

        public string ContentType { get; set; }

        public string MethodType { get; set; }

        public string HashCode { get; set; }

        public string HashCodeRecheck { get; set; }

        public short HashType { get; set; }

        public short AppType { get; set; }

        public long ParsingDataID { get; set; }
    }
}
