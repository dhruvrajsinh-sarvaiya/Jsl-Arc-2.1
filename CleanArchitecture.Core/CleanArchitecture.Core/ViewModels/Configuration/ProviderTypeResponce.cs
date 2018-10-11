using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ProviderTypeResponce : BizResponseClass
    {
        public IEnumerable <ProviderTypeViewModel> responce { get; set; }
    }
    public class ProviderTypeResponceData : BizResponseClass
    {
        public ProviderTypeViewModel responce { get; set; }
    }
    public class ProviderTypeRequest
    {
        public long Id { get; set; }
        [Required]
        [StringLength(20)]
        public string ServiveProTypeName { get; set; }
    }
}
