using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class RouteConfigurationRequest
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4511")]
        [StringLength(30,ErrorMessage = "1,Please enter a valid  parameters,4512")]
        public string RouteName { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4509")]
        public long ServiceID { get; set; } // spelling mistake ntrivedi 03-10-2018
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4513")]
        public long ServiceProDetailId { get; set; }  
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4514")]
        public long ProductID { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4515")]
        public short Priority { get; set; }
        public string StatusCheckUrl { get; set; }
        public string ValidationUrl { get; set; }
        public string TransactionUrl { get; set; }
        public long LimitId { get; set; }

        [StringLength(50, ErrorMessage = "1,Please enter a valid  parameters,4516")]
        public string OpCode { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4517")]
        public enTrnType TrnType { get; set; }

        [DefaultValue(0)]
        public byte IsDelayAddress { get; set; }

        [StringLength(100, ErrorMessage = "1,Please enter a valid  parameters,4518")]
        public string ProviderWalletID { get; set; }
    }
}
