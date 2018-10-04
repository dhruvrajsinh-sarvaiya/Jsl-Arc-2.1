using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp
{
    public class TempUserRegisterViewModel : BizBase
    {
        public int RegTypeId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStemp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
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
        public RegisterType RegisterType { get; set; }
    }
}
