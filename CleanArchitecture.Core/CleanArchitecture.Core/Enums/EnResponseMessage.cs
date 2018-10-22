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
        public static string CreateTrnNoPairSelectedMsg = "Invalid Pair Selected";
        public static string CreateTrnInvalidQtyPriceMsg = "Invalid Qty or Price";
        public static string CreateTrnInvalidQtyNAmountMsg = "Invalid Order Qty and Amount";
        public static string CreateTrn_NoCreditAccountFoundMsg = "No Credit Account Found";
        public static string CreateTrn_NoDebitAccountFoundMsg = "No Debit Account Found";
        public static string CreateTrnInvalidAmountMsg = "Invalid Amount";
        public static string CreateTrnDuplicateTrnMsg = "Duplicate Transaction for Same Address, Please Try After 10 Minutes";
        public static string CreateTrn_NoSelfAddressWithdrawAllowMsg = "Invalid Amount";
        public static string ProcessTrn_InsufficientBalanceMsg = "Insufficient Wallet Balance";
        public static string ProcessTrn_AmountBetweenMinMaxMsg = "Amount Must be Between: @MIN AND @MAX";
        public static string ProcessTrn_PriceBetweenMinMaxMsg = "Price Must be Between: @MIN AND @MAX";
        public static string ProcessTrn_InvalidBidPriceValueMsg = "Invalid BidPrice Value";
        public static string ProcessTrn_PoolOrderCreateFailMsg = "Order Creation Fail";
        public static string ProcessTrn_InitializeMsg = "Initialize";
        public static string ProcessTrn_ServiceProductNotAvailableMsg = "Service or Product Not Available";
        public static string ProcessTrn_WalletDebitFailMsg = "Wallet Debit Fail";
        public static string ProcessTrn_HoldMsg = "Hold";
        public static string ProcessTrn_ThirdPartyDataNotFoundMsg = "Third Party Data Not Found";
        //============================

        //============================walelt=================================//       

        public static string CreateWalletSuccessMsg = "Wallet is Successfully Created.";
        public static string SetWalletLimitCreateMsg = "Limit Created Successfully";
        public static string SetWalletLimitUpdateMsg = "Limit Updated Successfully";
        public static string CreateWalletFailMsg = "Fail";
        public static string CreateAddressSuccessMsg = "Address is Successfully Created.";
        public static string CreateAddressFailMsg = "Failed to generate Address.";
        public static string InvalidWallet = "Invalid Wallet or wallet is disabled.";
        public static string ItemOrThirdprtyNotFound = "Unable to Process your request please contact admin.";
        public static string FindRecored = "Record Found Successfully!";
        public static string NotFound = "Record Not Found";
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
        public static string InvalidTradeRefNo = "Invalid Trade RefNo.";
        public static string AlredyExist = "Duplicate Request for same Ref No.";
        public static string InsufficantBal = "Insufficeint Balance";
        public static string SuccessDebit = "Balance Debited Successfully";
        public static string SuccessCredit = "Balance Credited Successfully";



        //========================My Account===============================//
        public static string SendMailSubject = "Registration confirmation email";
        public static string ReSendMailSubject = "Registration confirmation resend email";
        public static string ForgotPasswordMail = "Forgot password email";
        public static string ResetPasswordMail = "Reset password conformation email";
        public static string SendMailBody = "Do not Share this code with anyone for security reasons. Your unique verfication code is ";
        public static string SendSMSSubject = "Do not Share this code with anyone for security reasons. Your unique verfication code is ";
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
        public static string IpAddressInvalid = "Invalid IPAddress.";
        public static string SuccessfullGetUserData = "Success full get user data.";
        public static string SuccessfullUpdateUserData = "Success full update user data.";
        public static string Unableupdateuserinfo = "Unable to update user info";

        public static string SuccessAddIpData = "Success full add Ip Address.";
        public static string IpAddressInsertError = "Ip address not inserted.";
        public static string Verificationpending = "User register verification pending.";

       
        public static string ResetConfirmedLink = "Reset password link send on your email, please click that link and confirmed.";
        public static string ResetResendEmail = "You have successfully resend New password in email.";
        public static string ChangePassword = "User changed their password successfully.";
        public static string UnableChangePassword = "Unable to change password";
        public static string EmailNewPassword = "Use this New Password to Login. Your New Password is";
        public static string ForgotPassLink = "Please verify it by clicking for regenerated password .";
        public static string ResetUserNotAvailable = "Don't reveal that the user does not exist or is not confirmed.";
        public static string ResetEmailMessage = "Please reset your password by clicking here.";
        public static string ResetPasswordUseNotexist = "Don't reveal that the user does not exist.";
        public static string ResetPasswordEmailExpire = "This email link was expired.";
        public static string ResetPasswordEmailLinkBlank = "Reset password email link should not be empty.";
        public static string ResetConfirm = "You have already confirm reset password link, please check in your mail if was not received mail then perform to forgot opration.";
        public static string ResetConfirmPassMatch = "New password and confirmation password do not match.";
        public static string ResetConfirmOldNotMatch = "Old password does not match";
        public static string InvalidUserSelectedIp = "Invalid User Selected IPAddress";
        public static string IpAddressUpdateError = "Ip address status not update.";
        public static string SuccessDesableIpStatus = "Success full desable ip address.";
        public static string SuccessDeleteIpAddress = "Success full remove ip address.";
        public static string IpAddressdeleteError = "Ip address not remove.";
        public static string SuccessGetIpData = "Success full Get Ip Address.";
        public static string InvalidAppkey = "Invalid appkey or password.";
        public static string Appkey = "This appkey data not available.";
        public static string InvalidUser = "The username/password couple is invalid.";
        public static string RefreshToken = "The refresh token is no longer valid.";
        public static string UserToken = "The user is no longer allowed to sign in.";
        public static string Granttype = "The specified grant type is not supported.";
        public static string EnableTroFactor = "User two factor authentication successfully activates";
        public static string DisableTroFactor = "User two factor authentication successfully disable";
        public static string DisableTroFactorError = "Unexpected error occured disabling 2FA for user with ID";
        public static string FactorFail = "Invalid authenticator code ";
        public static string FactorRequired = "Two factor authentication is activated please verify your code";
        public static string TwoFactorVerification = "Two factor authentication Verification code is invalid.";
        public static string SuccessAddDeviceData = "Success full add device id.";
        public static string DeviceidInsertError = "Device id not added.";
        public static string DeviceAddressUpdateError = "Device address status not update.";
        public static string SuccessDeleteDevice = "Success full remove device address.";
        public static string DeviceAddressdeleteError = "Device address not remove.";
        public static string SuccessGetDeviceData = "Success full Get Device Address.";

    }
}
