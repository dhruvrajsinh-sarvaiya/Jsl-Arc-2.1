using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ServiceDetailJsonData
    {
        public string ImageUrl { get; set; }
        public long TotalSupply { get; set; }
        public long MaxSupply { get; set; }
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
}
