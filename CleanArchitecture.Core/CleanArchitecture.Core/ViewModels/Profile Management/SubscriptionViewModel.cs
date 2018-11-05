using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Profile_Management
{
    public class SubscriptionViewModel : TrackerViewModel
    {
        public int UserId { get; set; }

        public long ProfileId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        public bool ActiveStatus { get; set; }
    }

    public class MultipalSubscriptionReqViewModel : TrackerViewModel
    {
        [Required]
        public int ProfileId { get; set; }
    }




    public class SubscriptionResponse : BizResponseClass
    {
        //public List<ProfileMasterViewModel> ProfileList { get; set; }
    }
}
