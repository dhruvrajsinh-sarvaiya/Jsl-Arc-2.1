using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;



namespace CleanArchitecture.Core.Entities
{
    public class ThirPartyAPIServiceMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ThirdPartyAPIServiceID { get; set; }

        [Required]
        [StringLength(30)]
        public string ThirdPartyAPIServiceName { get; set; }

        [Required]
        [Url]
        public string ThirdPartyAPISendURL { get; set; }
       
        [Url]
        public string ThirdPartyAPIValidateURL { get; set; }
       
        [Url]
        public string ThirdPartyAPIBalURL { get; set; }

        [Url]
        public string ThirdPartyAPIStatusCheckURL { get; set; }
       
        public string TransactionIdPrefix { get; set; }

        public string MerchantCode { get; set; }

        public string UserID { get; set; }

        public string Password { get; set; }

        public string ResponseSuccess { get; set; }

        public string ResponseFailure { get; set; }
              
        public string ResponseHold { get; set; }

        public string ReferenceNo { get; set; }

        public string AuthHeader { get; set; }

        public string Optional1 { get; set; }

        public string Optional2 { get; set; }

        public string MobileNoPrefix { get; set; }

        public string ContentType { get; set; }

        public string MethodType { get; set; }

        public string HashCode { get; set; }

        public string HashCodeRecheck { get; set; }

        public short HashType { get; set; }

        public short AppType { get; set; }

        public string BalanceRegex { get; set; }

        public string StatusRegex { get; set; }
         
        public string StatusMsgRegex { get; set; }

        public string TrnRefNoRegex { get; set; }

        public string OprTrnRefNoRegex { get; set; }

        public string Param1Regex { get; set; }

        public string Param2Regex { get; set; }

        public string Param3Regex { get; set; }
    }
}
