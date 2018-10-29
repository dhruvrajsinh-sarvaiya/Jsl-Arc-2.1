using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction.BackOffice
{
    public class TradeReconRequest
    {
        public long TranNo { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4592")]
        public string ActionMessage { get; set; }

        [EnumDataType(typeof(enTradeReconActionType), ErrorMessage = "1,Invalid Parameter Value,4618")]
        public enTradeReconActionType ActionType { get; set; }
    }
}
