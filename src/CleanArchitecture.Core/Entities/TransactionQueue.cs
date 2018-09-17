using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{   
    public class TransactionQueue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TrnNo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TrnDate { get; set; }
        //[Required]
        //public DateTime SMSDate { get; set; }
        [Required]
        public short TrnMode { get; set; }
        [Required]
        public short TrnType { get; set; }
        [Required]
        public long MemberID { get; set; }
        [Required]
        public string MemberMobile { get; set; }
        [Required]
        public string SMSText { get; set; }

        [Required]
        [StringLength(10)]
        public string SMSCode { get; set; }

        [Required]
        [StringLength(15)]
        public string CustomerMobile { get; set; }

        [Required]
        //[Range(0, 9999999999.99999999)]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(4)]
        public string SMSPwd { get; set; }

        //[Required]
        //public short IsSTV { get; set; }
       
        public long ServiceID { get; set; }
       
        public long SerProID { get; set; }
        
        public int ProductID { get; set; }
       
        //public int ItemID { get; set; }
        public int RoutID { get; set; }//change column as new structure
        [Required]
        public short Status { get; set; }

        [Required]
        public short StatusCode { get; set; }

        [Required]
        public string StatusMsg { get; set; }
        //[Required]
        //public double DiscPer { get; set; }
        //[Required]
        //public decimal DiscRs { get; set; }
        //[Required]
        //public double ORDiscPer { get; set; }
        //[Required]
        //public decimal ORDiscRs { get; set; }
        //[Required]
        //public double SPDiscPer { get; set; }
        //[Required]
        //public decimal SPDiscRs { get; set; }
        //[Required]
        //public double SDDiscPer { get; set; }
        //[Required]
        //public decimal SDDiscRs { get; set; }
        //[Required]
        //public double DTDiscPer { get; set; }
        //[Required]
        //public decimal DTDiscRs { get; set; }
        //[Required]
        //public double RTDiscPer { get; set; }
        //[Required]
        //public decimal RTDiscRs { get; set; }
       
        public short VerifyDone { get; set; }

        public string TrnRefNo { get; set; }

        //public decimal? OfferedFare { get; set; }

        //public decimal? MarkupValue { get; set; }

        public string AdditionalInfo { get; set; }

        //public long? CouponNo { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal? ChargePer { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal? ChargeRs { get; set; }

        public short? ChargeType { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();

        public void MakeTransactionSuccess()
        {
            Status = Convert.ToInt16(TransactionStatus.Success);
            AddValueChangeEvent();
        }
        public void SetTransactionCode(short statuscode)
        {
            StatusCode = statuscode;
            AddValueChangeEvent();
        }
        public void SetTransactionStatusMsg(string statusMsg)
        {
            StatusMsg = statusMsg;
            AddValueChangeEvent();
        }
        public void AddValueChangeEvent()
        {
            Events.Add(new ServiceStatusEvent<TransactionQueue>(this));
        }

    }
}
