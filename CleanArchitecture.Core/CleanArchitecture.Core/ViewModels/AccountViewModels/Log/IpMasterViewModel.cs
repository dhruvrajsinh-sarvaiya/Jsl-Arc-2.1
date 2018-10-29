using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;

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
        public DateTime? UpdatedDate { get; set; }//rita 29-10-2018 change in bizbase so need to change here for converting error in : IpAddressService

        public short Status { get; set; }
    }

    public class IpAddressReqViewModel : TrackerViewModel
    {
        [Required(ErrorMessage = "1,User selected ip address not found,4044")]
        [StringLength(15, ErrorMessage = "1,Invalid user selected ip address,4045")]
        public string SelectedIPAddress { get; set; }
    }

    public class IpAddressResponse : BizResponseClass
    {
        public int TotalRow { get; set; }
        public List<IpMasterViewModel> IpList { get; set; }

    }
}
