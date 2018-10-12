using System;
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
        public static string FindRecored = "Successfully Find Recored";
        public static string NotFound = "Not Found Recored";
        public static string RecordAdded = "Recored Added Successfully!";
        public static string RecordUpdated = "Recored Updated Successfully!";
        public static string RecordDisable = "Recored Disable Successfully!";


        public static string InvalidAmt = "Invalid Amount.";
        public static string InsufficientBal = "Insufficient Balance.";
        public static string BatchNoFailed = "Batch No Generation Failed.";
        public static string DefaultWallet404 = "Default Wallet Not Found.";
        public static string InvalidReq = "Invalid Request Detail.";
        public static string InvalidTrnType = "Invalid TrnType.";
        public static string NotAllowedTrnType = "TrnType Not Allowed to wallet.";

        public static string InvalidCoin = "Invalid CoinName.";

        //========================My Account===============================//
        public static string SendMailSubject = "Registration confirmation email";
        public static string ReSendMailSubject = "Registration confirmation resend email";
        public static string SendMailBody = "use this code: ";
        public static string SendSMSSubject = "SMS for use this code ";
        public static string LoginEmailSubject = "Login With Email Otp ";
        public static string StandardSignUpPhonevelid = "Please Enter Valid Mobile Number";
        public static string StandardSignUp = "Your account has been created, please verify it by clicking the activation link that has been send to your email.";
        public static string SignUpValidation = "This username or email is already registered.";
        public static string SignUpUser = "This user data not available.";
        public static string SignWithEmail = "Please verify it by clicking the otp that has been send to your email.";
        public static string SignUpEmailValidation = "This email is already registered.";
        public static string SignUpEmailConfirm = "Your account has been activated, you can now login.";
        public static string SignUpEmailExpired = "Reset links immediately not valid or expired.";
        public static string SignEmailLink = "This email code can't be balck.";
        public static string SignEmailUser = "This email link using user not valid.";
        public static string StandardResendSignUp = "Your account has been created, please verify it by clicking the activation link that has been send to your email.";
        public static string StandardLoginSuccess = "User Login Successfull.";
        public static string StandardLoginLockOut = "User account locked out for two hours.";
        public static string StandardLoginfailed = "Login failed : Invalid username or password.";
        public static string LoginWithOtpSuccessSend = "You have send OTP on email.";
        public static string LoginWithEmailSuccessSend = " User Login with Email Send Success.";
        public static string LoginWithOtpLoginFailed = "Login failed: Invalid email.";
        public static string LoginWithOtpInvalidAttempt = "Invalid login attempt.";
        public static string LoginWithOtpDatanotSend = "User Otp Data Not Send.";
        public static string SignUPMobileValidation = "This mobile number is already registered.";
        public static string SignUpWithMobile = "Please verify it by clicking the otp that has been send to your mobile.";
        public static string SignUpWithMobileValid = "This mobile number is not valid.";
        public static string SignUPVerification = "You have successfully verified.";
        public static string SignUpOTP = "Invalid OTP or expired, resend OTP immediately.";
        public static string SignUpResendOTP = "Resend OTP immediately not valid or expired.";
        public static string SignUpRole = "This User roles not available.";
        public static string SignUpWithResendEmail = "You have successfully resend Otp in email.";
        public static string SignUpWithResendMobile = "You have successfully resend Otp in mobile.";
        public static string OTPSendOnMobile = "You have send OTP on mobile";
        public static string OTPNotSendOnMobile = " Not send OTP on mobile";
        public static string LoginUserEmailOTP = "User Login with Email OTP Send Success.";
        public static string LoginEmailOTPNotsend = "User Login with Email OTP not Send Successfully.";
        public static string EmailFail = "Email Address Invalid";
        public static string ResetConfirmed = "Reset confirmed";
        public static string LoginMobileNumberInvalid = "Invalid mobileno.";
    }
}
