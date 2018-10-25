using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class ThirdPartyAPIConfiguration : BizBase
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long ThirPartyAPIID { get; set; }

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

        public string APIRequestBody { get; set; }

        public string TransactionIdPrefix { get; set; }

        public string MerchantCode { get; set; }

        //public string UserID { get; set; }

        //public string Password { get; set; }
        
        //public long SerProConfigurationID { get; set; } Rita 25-10-2018 already in Provider detail exists

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
        public string TimeStamp { get; set; }

        public void SetActive()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<ThirdPartyAPIConfiguration>(this));
        }
        public void SetInActive()
        {
            Status = Convert.ToInt16(ServiceStatus.InActive);
            Events.Add(new ServiceStatusEvent<ThirdPartyAPIConfiguration>(this));
        }
    }
}
