using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public  class TransactionRequest : BizBase
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long Id { get; set; }

        [Required]
        public long TrnNo { get; set; }
        public long ServiceID { get; set; }
        public long SerProID { get; set; }
        public long SerProDetailID { get; set; }
        //IsStatusCheck         
        //[Required]
        //[DataType(DataType.DateTime)]
        //public DateTime RequestTime { get; set; }

        [Required]
        public string RequestData { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime ResponseTime { get; set; }
        
        public string ResponseData { get; set; }
        //public short Status { get; set; }
        public string TrnID { get; set; }
        public string OprTrnID { get; set; }

        public void MakeTransactionSuccess()
        {
            Status = Convert.ToInt16(enTransactionStatus.Success);
            AddValueChangeEvent();
        }      
        public void MakeTransactionHold()
        {
            Status = Convert.ToInt16(enTransactionStatus.Hold);
            AddValueChangeEvent();
        }
        public void MakeTransactionOperatorFail()
        {
            Status = Convert.ToInt16(enTransactionStatus.OperatorFail);
            AddValueChangeEvent();
        }
        public void SetResponse(string Response)
        {
            ResponseData = Response;
            AddValueChangeEvent();
        }
        public void SetTrnID(string sTrnID)
        {
            TrnID = sTrnID;
            AddValueChangeEvent();
        }
        public void SetOprTrnID(string sOprTrnID)
        {
            OprTrnID = sOprTrnID;
            AddValueChangeEvent();
        }
        public void AddValueChangeEvent()
        {
            Events.Add(new ServiceStatusEvent<TransactionRequest>(this));
        }
    }
}
