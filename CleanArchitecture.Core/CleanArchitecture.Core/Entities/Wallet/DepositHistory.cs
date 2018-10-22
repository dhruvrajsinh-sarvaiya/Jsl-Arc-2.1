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
    public class DepositHistory : BizBase
    {
                
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new long Id { get; set; }

        [Key]
        [StringLength(100)]
        public string TrnID { get; set; }

        [Required]
        public string SMSCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        public long Confirmations { get; set; }

        //[Required]
        //public decimal Value { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(100)]
        public string StatusMsg { get; set; }

        [Required]
        public string TimeEpoch { get; set; }

        [Required]
        public string ConfirmedTime { get; set; }

        [Required]
        public string EpochTimePure { get; set; } // time converted from epoch time 
               
        public long OrderID { get; set; }

        public byte IsProcessing { get; set; }

        [Required]
        [StringLength(50)]
        public string FromAddress { get; set; }
        
        public string APITopUpRefNo { get; set; }

        public string SystemRemarks { get; set; }

        public string RouteTag { get; set; }

        public long SerProID { get; set; }

        //vsolanki 2018-10-16
        public long userId { get; set; }

        //public long SerProId { get; set; }



        public void OrderIdUpdated(long orderid)
        {
            try
            {
                OrderID = orderid;
                Events.Add(new ServiceStatusEvent<DepositHistory>(this));
            }
            catch (Exception ex)
            {                
                throw ex;
            }

        }

        public void SetAsSuccess(string msg)
        {
            try
            {
                StatusMsg = msg;
                Status = Convert.ToInt16(enOrderStatus.Success);
                Events.Add(new ServiceStatusEvent<DepositHistory>(this));
            }
            catch (Exception ex)
            {
                // _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name,LogLevel.Error);
                throw ex;
            }

        }
        public void SetAsRejected(string msg)
        {
            try
            {
                StatusMsg = msg;
                Status =  Convert.ToInt16(enOrderStatus.Rejected);
                Events.Add(new ServiceStatusEvent<DepositHistory>(this));
            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public void ConfirmationUpdated(long Confirmation)
        {
            try
            {
                Confirmations = Confirmation;
                Events.Add(new ServiceStatusEvent<DepositHistory>(this));
            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

    }

}
