using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class TemplateMasterReq
    {
        [Required(ErrorMessage = "1,Enter Required Parameter Value,4700")]
        [EnumDataType(typeof(EnTemplateType), ErrorMessage = "1,Invalid Parameter Value,4618")]
        public EnTemplateType TemplateID { get; set; }

        [Required]
        public long CommServiceTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string TemplateName { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [StringLength(200)]
        public string AdditionalInfo { get; set; }
    }

    public class TemplateMasterRes: BizResponseClass
    {
        public  TemplateMaster TemplateMasterObj{ get;set;}
    }
}
