using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class TradePairConfigRequest
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4530")]
        public string PairName { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4531")]
        public long SecondaryCurrencyId { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4532")]
        public long WalletMasterID { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4533")]
        public long BaseCurrencyId { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4534")]
        [Range(0, 9999999999.99999999,ErrorMessage = "1,Please enter a valid parameters,4549"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Currentrate { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4535")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4550"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal BuyMinQty { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4536")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4551"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal BuyMaxQty { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4537")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4552"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SellMinQty { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4538")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4553"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SellMaxQty { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4539")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4554"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DailyHigh { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4540")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4555"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DailyLow { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4541")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4556"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal CurrencyPrice { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4542")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4557"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Volume { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4543")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4558"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SellPrice { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4544")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4559"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal BuyPrice { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4545")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4560"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal BuyMinPrice { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4546")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4561"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal BuyMaxPrice { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4547")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4562"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SellMinPrice { get; set; }
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4548")]
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4563"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal SellMaxPrice { get; set; }
        [Range(0, 9999999999.99999999, ErrorMessage = "1,Please enter a valid parameters,4564"), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Fee { get; set; }
        public short FeeType { get; set; }
    }
}
