using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ServiceConfigurationRequest
    {
        public long ServiceId { get; set; }
        [StringLength(30)]
        [Required]
        public string Name { get; set; }
        [StringLength(5)]
        [Required]
        public string SMSCode { get; set; }
        public short Type { get; set; }
        public string ImageUrl { get; set; }
        public long TotalSupply { get; set; } 
        public long MaxSupply { get; set; }
        [Required]
        public DateTime IssueDate { get; set; }
        public decimal IssuePrice { get; set; }
        public long CirculatingSupply { get; set; }
        [Required]
        public string ProofType { get; set; }
        public string EncryptionAlgorithm { get; set; }
        [Required]
        public string WebsiteUrl { get; set; }
        public List<ExplorerData> Explorer { get; set; }
        public List<CommunityData> Community { get; set; }
        [Required]
        public string WhitePaperPath { get; set; }
        [Required]
        public string Introduction { get; set; }
       
    }

    public class ExplorerData
    {
        public string Data { get; set; }
    }
    public class CommunityData
    {
        public string Data { get; set; }
    }
}
