using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Profile_Management
{
    public class ProfileMasterViewModel : TrackerViewModel
    {
        public string TypeId { get; set; }

        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Price { get; set; }

        [StringLength(2000)]
        [Required]
        public string Description { get; set; }

        public int Level { get; set; }

        [StringLength(150)]
        [Required]
        public string LevelName { get; set; }

        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal DepositFee { get; set; }

        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Withdrawalfee { get; set; }

        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Tradingfee { get; set; }

        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal WithdrawalLimit { get; set; }

        public bool EnableStatus { get; set; }

        public bool ActiveStatus { get; set; }

    }

    public class ProfileMasterResponse : BizResponseClass
    {
        public List<ProfileMasterViewModel> ProfileList { get; set; }
    }
}
