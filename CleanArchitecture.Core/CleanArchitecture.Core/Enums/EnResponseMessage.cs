﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Enums
{
    public class EnResponseMessage
    {
        // ================================ Communnication ========================= //

        public static string SMSSuccessMessage = "The message has been sent successfully.";
        public static string EMailSuccessMessage = "The Email has been sent successfully.";
        public static string NotificationSuccessMessage = "The Notification has been sent successfully.";

        public static string SMSFailMessage = "The message has been Failed.";
        public static string EmailFailMessage = "The Email has been Failed.";
        public static string NotificationFailMessage = "The Notification has been Failed.";

        public static string SMSExceptionMessage = "Sorry! A technical error occurred while processing your request.";
        public static string EmailExceptionMessage = "Sorry! A technical error occurred while processing your request.";
        public static string NotificationExceptionMessage = "Sorry! A technical error occurred while processing your request.";
        //=====================Common for all internal use only
        public static string CommSuccessMsgInternal = "Success";
        public static string CommFailMsgInternal = "Fail";
        //=========================Transactional Msg
        //public static string CreateTrnSuccessMsg = "Success";
        //public static string CreateTrnFailMsg = "Fail";
        public static string CreateTrnNoPairSelectedMsg = "No Pair Selected";
        public static string CreateTrnInvalidQtyPriceMsg = "Invalid Qty or Price";
        public static string CreateTrnInvalidQtyNAmountMsg = "Invalid Order Qty and Amount";
        public static string CreateTrn_NoCreditAccountFoundMsg = "No Credit Account Found";
        public static string CreateTrn_NoDebitAccountFoundMsg = "No Debit Account Found";
        public static string CreateTrnInvalidAmountMsg = "Invalid Amount";
        public static string CreateTrnDuplicateTrnMsg = "Duplicate Transaction for Same Address, Please Try After 10 Minutes";
        public static string CreateTrn_NoSelfAddressWithdrawAllowMsg = "Invalid Amount";
        //============================

        //============================walelt=================================//
        public static string CreateWalletSuccessMsg = "Wallet is Successfully Created.";
        public static string CreateWalletFailMsg = "Fail";
        public static string CreateAddressSuccessMsg = "Address is Successfully Created.";
        public static string CreateAddressFailMsg = "Failed to generate Address.";
        public static string InvalidWallet = "Invalid Wallet or wallet is disabled.";
        public static string ItemOrThirdprtyNotFound = "Unable to Process your request please contact admin.";

        //========================My Account===============================//
        public static string SendMailSubject = "Registration confirmation email";
        public static string ReSendMailSubject = "Registration confirmation resend email";
        public static string SendMailBody = "use this code: ";
        public static string SendSMSSubject = "SMS for use this code ";
        public static string LoginEmailSubject = "Login With Email Otp ";
    }
}
