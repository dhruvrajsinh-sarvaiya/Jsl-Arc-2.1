using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Core.ApiModels;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Core.ViewModels.ManageViewModels
{
    public class IndexViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        //public string RedisDBKey { get; set; }
    }

    public class UserInfoResponse : BizResponseClass
    {
        public IndexViewModel UserData { get; set; }
    }
}
