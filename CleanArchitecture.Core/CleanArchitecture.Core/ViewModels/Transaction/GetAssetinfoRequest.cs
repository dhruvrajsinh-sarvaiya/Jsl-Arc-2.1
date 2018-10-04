using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetAssetInfoRequest
    {
        [DefaultValue("info")]
        public string info { get; set; }

        [DefaultValue("currency")]
        public string aclass { get; set; }

        public string asset { get; set; }
    }
}
