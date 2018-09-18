using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Services;
using CleanArchitecture.Core.SharedKernel;
using Microsoft.Extensions.Logging;


namespace CleanArchitecture.Core.Entities
{
    public class WalletOrder : BizBase // Similler to MemberOrder table
    {
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public EnOrderType OrderType { get; set; }

        [Required]
        public long OWalletMasterID { get; set; }

        [Required]
        public long DWalletMasterID { get; set; }

        [Required]       
        [Range(1, 99999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal OrderAmt { get; set; }

        [Required]
        public new EnOrderStatus Status { get; set; }

        //public double DiscPer { get; set; }

        //public decimal DiscRs { get; set; }

        //public long? OBankID { get; set; }

        //public string OBranchName { get; set; }

        //public string OAccountNo { get; set; }

        //public string OChequeNo { get; set; }

        //public DateTime? OChequeDate { get; set; }

        //public long DMemberID { get; set; }

        //public long DBankID { get; set; }

        //public string DAccountNo { get; set; }

        //public byte Status { get; set; }
        [Required]
        [StringLength(100)]
        public string ORemarks { get; set; }

        [Required]
        [Range(1, 99999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal DeliveryAmt { get; set; }
       
        public string DRemarks { get; set; }
                
        public long? DeliveryGivenBy { get; set; }

        public DateTime? DeliveryGivenDate { get; set; }

        //public byte AlertRec { get; set; }

        //public double CashChargePer { get; set; }

        //public decimal CashChargeRs { get; set; }

        //public decimal WalletAmt { get; set; }

        //public int? PGId { get; set; }

        //public long? CouponNo { get; set; }

        //public bool? IsChargeAccepted { get; set; }

        //public bool? IsDebited { get; set; }

        readonly ILogger<ExceptionLog> _log;

        public WalletOrder(ILogger<ExceptionLog> log)
        {
            _log = log;
        }

        public void SetAsSuccess()
        {
            try
            {
                Status = EnOrderStatus.Success;
                Events.Add(new ServiceStatusEvent<WalletOrder>(this));
            }
            catch(Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name,LogLevel.Error);
            }

        }
        public void SetAsRejected()
        {
            Status = EnOrderStatus.Rejected;
            Events.Add(new ServiceStatusEvent<WalletOrder>(this));
        }

    }



}
