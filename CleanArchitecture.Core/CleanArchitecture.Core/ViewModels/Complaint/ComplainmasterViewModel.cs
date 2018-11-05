using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Complaint
{
   public class ComplainmasterViewModel
    {

        [Required(ErrorMessage = "1,Please Enter complaint Subject,4118")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "2,Please Enter complaint Description,4117")]
        public string Description { get; set; }
        public int TypeId { get; set; } 
       // public int UserID { get; set; }
    }


    public class ComplainmasterReqViewModel
    {

        [Required(ErrorMessage = "1,Please Enter complaint Subject,4118")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "2,Please Enter complaint Description,4117")]
        public string Description { get; set; }
        public int TypeId { get; set; }
         public int UserID { get; set; }
    }

    public class ComplainmasterResonse  :  BizResponseClass
    {
        public List<CompainDetailResponse> compainDetailResponses { get; set; }
        public List<UserWiseCompaintDetailResponce> userWiseCompaintDetailResponces { get; set; }
    }
        
}
