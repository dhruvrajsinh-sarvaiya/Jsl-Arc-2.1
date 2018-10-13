using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CleanArchitecture.Core.ApiModels;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Log
{
    public class IpMasterViewModel
    {
        public long Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [StringLength(15)]
        public string IpAddress { get; set; }
        public bool IsEnable { get; set; }
        public bool IsDeleted { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public long? UpdatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set; }

        public short Status { get; set; }
    }

    public class IpAddressReqViewModel
    {
        [Required(ErrorMessage = "1,IPAddress Not Found,4019")]
        [StringLength(15, ErrorMessage = "1,Invalid IPAddress,4020")]
        public string IPAddress { get; set; }
    }

    public class IpAddressResponse : BizResponseClass
    {
    }
}
