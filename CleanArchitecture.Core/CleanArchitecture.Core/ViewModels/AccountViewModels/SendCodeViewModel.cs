using System.Collections.Generic;
using CleanArchitecture.Core.ApiModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
    public class SendCodeResponse : BizResponseClass
    {

    }
}
