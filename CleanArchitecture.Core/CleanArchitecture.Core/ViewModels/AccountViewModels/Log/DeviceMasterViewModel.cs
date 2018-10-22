using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Log
{
   public class DeviceMasterViewModel
    {
        public long Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [StringLength(250)]
        public string DeviceId { get; set; }
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

    public class DeviceIdReqViewModel : TrackerViewModel
    {        
        [Required(ErrorMessage = "1,User selected deviceID not found,4055")]
        [StringLength(250, ErrorMessage = "1,User selected deviceID not valid,4056")]
        public string SelectedDeviceId { get; set; }
    }

    public class DeviceIdResponse : BizResponseClass
    {
        public int TotalRow { get; set; }
        public List<DeviceMasterViewModel> DeviceList { get; set; }

    }
}
