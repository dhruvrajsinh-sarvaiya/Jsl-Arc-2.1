using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CleanArchitecture.Core.Enums.Modes;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities.User
{
    public class TempUserRegister : BizBase
    {
        [Required]
        public int RegTypeId { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStemp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public bool RegisterStatus { get; set; }
        public bool IsDeleted { get; set; }
        //[DataType(DataType.DateTime)]
        //public DateTime CreatedDate { get; set; }
        //public string CreatedBy { get; set; }
        //[DataType(DataType.DateTime)]
        //public DateTime UpdatedDate { get; set; }
        //public string UpdateBy { get; set; }

        //public RegisterType RegisterType { get; set; }

        public void SetAsStatus()
        {
            RegisterStatus = Convert.ToBoolean(ModeStatus.True);
            Events.Add(new ServiceStatusEvent<TempUserRegister>(this));
        }
    }
}
