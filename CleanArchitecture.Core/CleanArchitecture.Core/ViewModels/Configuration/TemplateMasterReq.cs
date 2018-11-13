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
        [EnumDataType(typeof(EnTemplateType), ErrorMessage = "1,Invalid Parameter Value,4701")]
        public EnTemplateType TemplateID { get; set; }

        [Required(ErrorMessage = "1,Enter Required Parameter Value,4702")]
        public long CommServiceTypeID { get; set; }

        [Required(ErrorMessage = "1,Enter Required Parameter Value,4703")]
        [StringLength(50, ErrorMessage = "1,Invalid Parameter Value,4704")]
        public string TemplateName { get; set; }

        [Required(ErrorMessage = "1,Enter Required Parameter Value,4705")]
        public string Content { get; set; }

        [Required(ErrorMessage = "1,Enter Required Parameter Value,4706")]
        [StringLength(200, ErrorMessage = "1,Invalid Parameter Value,4707")]
        public string AdditionalInfo { get; set; }
    }

    public class TemplateMasterRes: BizResponseClass
    {
        public TemplateResponse TemplateMasterObj { get;set;}
    }

    public class ListTemplateMasterRes : BizResponseClass
    {
        public List<TemplateResponse> TemplateMasterObj { get; set; }
    }
    public class TemplateResponse
    {
        public long TemplateID { get; set; }
        public string TemplateType { get; set; }
        public long CommServiceTypeID { get; set; }
        public string CommServiceType { get; set; }
        public string TemplateName { get; set; }
        public string Content { get; set; }
        public string AdditionalInfo { get; set; }
        public short Status { get; set; }
    }
}
