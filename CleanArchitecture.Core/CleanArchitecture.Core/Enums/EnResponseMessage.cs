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
        public static string ProcessTrn_OprFailMsg = "Operator Fail";
        public static string TradeRecon_InvalidTransactionNo = "Invalid Transaction No";
        public static string TradeRecon_After7DaysTranDontTakeAction = "After 7 days of transaction you can not take action, Please contact admin";
        public static string TradeRecon_InvalidTransactionStatus = "Invalid Transaction Status";
        public static string TradeRecon_CancelRequestAlreayInProcess = "Transaction Cancellation request is already in processing.";
        public static string TradeRecon_TransactionAlreadyInProcess = "Transaction Already in Process, Please try After Sometime";
        public static string TradeRecon_OrderIsFullyExecuted = "Can not initiate Cancellation Request.Your order is fully executed";
        public static string TradeRecon_InvalidDeliveryAmount = "Invalid Delivery Amount";
        public static string TradeRecon_CencelRequestSuccess = "Order Cancellation request done successfully.";
        public static string TradeRecon_InvalidActionType = "Invalid ActionType Value";
        public static string FavPair_InvalidPairId = "Invalid PairId";
        public static string FavPair_AlreadyAdded = "Pair Already added as favourite";
        public static string FavPair_AddedSuccess = "Pair Added as favourite pair";
        public static string FavPair_RemoveSuccess = "Pair Remove from favourite pair";
        public static string FavPair_NoPairFound = "No Favourites pair found";
        public static string InValidDebitAccountIDMsg = "Invalid Debit Account ID";        
        public static string InValidCreditAccountIDMsg = "Invalid Credit Account ID";
        public static string CreateTrn_WithdrawAmountBetweenMinAndMax = "Amount Must be Between: @MIN AND @MAX";
        //============================

        //============================walelt=================================//       

        public static string CreateWalletSuccessMsg = "Wallet is Successfully Created.";
        public static string DefaultCreateWalletSuccessMsg = "Default Wallets are Successfully Created.";
        public static string NewCreateWalletSuccessMsg = "Wallet: #WalletName# Successfully Created.";
        public static string ConvertFund = "Convert #SourcePrice# to #DestinationPrice# Submit Successfully!!";
        public static string SetWalletLimitCreateMsg = "Limit Created Successfully";
        public static string SetUserPrefSuccessMsg = "User Preference is Successfully Created.";
        public static string SetWalletLimitUpdateMsg = "Limit Updated Successfully";
        public static string SetUserPrefUpdateMsg = "User Preference Is Updated Successfully";
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
        public static string InvalidLimit = "Invalid Limit.";
        public static string NotFoundLimit = "Not Found Limit.";

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
        public static string DuplicateRecord = "Duplicate Record";    
        public static string InvalidAddress = "Invalid Addess";
        public static string OrgIDNotFound = "Org record not found";
        public static string InternalError = "Internal Error";
        public static string BalMismatch = "Settled Balance Mismatch";
        public static string ShadowLimitExceed = "Exceed Shadow Limit";
        public static string CreditWalletMsg = "Your #Coin# wallet is Credited for #TrnType# Transaction TrnNo:#TrnNo#";
        public static string DebitWalletMsg = "Your #Coin# wallet is Debited for #TrnType# Transaction TrnNo:#TrnNo#";
        public static string GenerateAddressNotification = "New Address Created Successfully For Wallet:#WalletName#";
        public static string CWalletLimitNotification = "New Limit Created Successfully For Wallet:#WalletName#";
        public static string UWalletLimitNotification = "New Limit Updated Successfully For Wallet:#WalletName#";
        public static string AddBeneNotification = "New Beneficiary Added Successfully For Wallet Type:#WalletName#";
        public static string UpBeneNotification = "Beneficiary Details Updated Successfully For Wallet Type:#WalletName#";
        public static string UserPreferencesNotification = "Your Whitelisting Is Switched #ONOFF# Successfully";
        //public static string AddBeneNotification = "New Beneficiary Added Successfully For Wallet Type:#WalletName#";

        //========================My Account===============================//
        public static string SendMailSubject = "Registration confirmation email";
        public static string ReSendMailSubject = "Registration confirmation resend email";
        public static string ForgotPasswordMail = "Forgot password email";
        public static string ResetPasswordMail = "Reset password conformation email";
        public static string SendMailBody = "Do not Share this code with anyone for security reasons. Your unique verfication code is ";
        public static string SendSMSSubject = "Do not Share this code with anyone for security reasons. Your unique verfication code is ";
        public static string LoginEmailSubject = "Login With Email Otp ";
        public static string StandardSignUpPhonevalid = "Please Enter Valid Mobile Number";
        public static string StandardSignUp = "Your account has been created, please verify it by clicking the activation link that has been send to your email.";
        public static string SignUpValidation = "This username or email is already registered.";
        public static string SignUpUser = "This user data not available.";
        public static string SignWithEmail = "Successfull send otp on your register email.";
        public static string SignUpEmailValidation = "This email id already exist input other emai id.";
        public static string SignUpUserNotRegister = "User not register and verifay.";
        public static string SignUpEmailConfirm = "Your account has been activated, you can now login.";
        public static string SignUpEmailExpired = "Reset links immediately not valid or expired.";
        public static string SignEmailLink = "This email code can't be balck.";
        public static string SignEmailUser = "This email link using user not valid.";
        public static string StandardResendSignUp = "Your account has been created, please verify it by clicking the activation link that has been send to your email.";
        public static string StandardLoginSuccess = "User Login Successfull.";
        public static string StandardLoginLockOut = "User account locked out for two hours.";
        public static string StandardLoginfailed = "Login failed : Invalid username or password.";
        public static string LoginWithOtpSuccessSend = "You have send OTP on email.";
       // public static string LoginWithEmailSuccessSend = " User Login with Email Send Success.";
       // public static string LoginWithOtpLoginFailed = "Login failed: Invalid email.";
        public static string LoginWithOtpInvalidAttempt = "Invalid login attempt.";
        public static string LoginWithOtpDatanotSend = "User Otp Data Not Send.";
        public static string SignUPMobileValidation = "This mobile number is already registered.";
        public static string SignUpWithMobile = "Successfull send otp on your register mobile.";
        public static string SignUpWithMobileValid = "This mobile number is not valid.";
        public static string SignUPVerification = "You have successfully verified.";
        public static string SignUpOTP = "Invalid OTP ,resend OTP immediately.";
        public static string SignUpResendOTP = "Resend OTP immediately , this opt is expired.";
        public static string SignUpRole = "This User roles not available.";
        public static string SignUpWithResendEmail = "You have successfully resend Otp in email.";
        public static string SignUpWithResendMobile = "You have successfully resend Otp in mobile.";
        public static string OTPSendOnMobile = "You have send OTP on mobile.";
        public static string OTPNotSendOnMobile = "Not send OTP on mobile.";
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
        public static string SuccessupdateIpData = "Success full Update Ip Address data.";
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
        public static string SuccessDesableIpStatus = "Success full disable ip address.";
        public static string SuccessDeleteIpAddress = "Success full remove ip address.";
        public static string IpAddressdeleteError = "Ip address not remove.";
        public static string SuccessGetIpData = "Success full Get Ip Address.";
        public static string InvalidAppkey = "Invalid appkey or password.";
        public static string Appkey = "This appkey data not available.";
        public static string InvalidUser = "The username/password couple is invalid.";
        public static string RefreshToken = "The refresh token is no longer valid.";
        public static string UserToken = "The user is no longer allowed to sign in.";
        public static string Granttype = "The specified grant type is not supported.";
        public static string EnableTwoFactor = "User two factor authentication successfully activates";
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
        public static string TwoFaVerification = "TwoFA Acviate Redirect Verify method";
        public static string Userpasswordnotupdated = "User password not updated!";
        public static string TwoFactorVerificationDisable = "Two factor authentication Verification code is invalid Can't disable two factor authentication.";
        public static string InvalidGoogleToken = "Invalid Google access token.";
        public static string InvalidGoogleProviderKey = "Invalid Google provider key.";
        public static string SocialUserInsertError = "Social register not inserted.";
        public static string UnLockUser = "Successfull unlock user.";
        public static string UnLockUserError = "User not unlock.";

        public static string IpAlreadyExist = "Ip Address already exist.";
        public static string DeviceIdAlreadyExist = "DeviceId already exist.";

        public static string SuccessEnableIpStatus = "Success full enable ip address.";
        public static string SuccessEnableDeviceId = "Success full enable device Id.";
        public static string SuccessDisableDeviceId = "Success full disable device id.";

        public static string InvalidFaceBookToken = "Invalid FaceBook access token.";
        public static string InvalidFaceBookProviderKey = "Invalid FaceBook provider key.";

        public static string SignUpBizUserEmailExist = "This email id already registered.";
        public static string SignUpBizUserNameExist = "This Uaser name already registered.";

        public static string SignUpTempUserEmailExist = "This email id already exist input other emai id.";
        public static string SignUpTempUserNameExist = "This Uaser name already exist input other user name.";

        public static string SignUpTempUserEmailVerifyPending = "This email id already exist and  verify pending.";
        public static string SignUpTempUserNameVerifyPending = "This Uaser name already exist and verify pending.";
        public static string SignUpUserRegisterError = "User not register.";

        public static string SocialLoginKey = "Successfully get social provider detail";

        public static string provideDetailNotAvailable = "Provider detail not available.";

        public static string InputProvider = "Please input provider name.";


        public static string SignUpTempUserMobileExist = "This mobile number already exist input other mobile number.";
        public static string SignUpTempUserMobileExistAndVerificationPending = "This mobile number already exist and verify pending.";

        public static string TokenCreationUserDataNotAvailable = "User data not avaialable.";
        public static string LoginWithEmailSuccessSend = " Successfull send otp on your email id.";

        public static string LoginWithOtpLoginFailed = "Login failed: User email id not available.";
        public static string LoginWithMobileOtpLoginFailed = "Login failed: User mobile number not available.";

        public static string TwoFactorActiveRequest = "User two factor authentication reguest successfully send.";
        public static string TwoFAalreadyDisable = "Two factor authentication already disable.";
        public static string FactorKeyFail = "Invalid two factor key.";
        public static string SuccessGetIpHistory = "Success full Get Ip History.";
        public static string SuccessGetLoginHistory = "Success full Get Login History.";


        public static string ProfilePlan = "Profile plan not available.";
        public static string SuccessGetProfilePlan = "Success full get raise profile plan.";
        public static string SuccessAddProfile = "Success full add profile plan.";
        public static string NotAddedProfile = "This profile plan already exists or selected plan not allow lower level.";
        public static string InvalidProfileId = "Invalid profileId.";
        public static string Typemasterrequired = "Please Enter complaint type";
        public static string TypemasterInsertError = "raise complaint not added.";
        public static string complaintTypeNotavailable = "Complaint type not available";
        public static string SuccessAddComplain = "Success full add raise complaint.";
        public static string AddCompainrequired = "Please Provice Compaint raise Detail.";
        public static string AddCompainTrail = "Please Provice CompainTrail Detail.";
        public static string CompainTrailInsertError = "CompainTrail not added.";
        public static string SuccessCompainTrail = "Success full add CompainTrail.";

        public static string SuccessGetCompainDetail = "Success full get complaint data.";

        public static string Complaintdatanotavailable = "Complaint not available.";

        public static string ImageNotAvailable = "Image not availalbe.";
        public static string FrontImageSizeLarger = "Front image size lager.";
        public static string BackImageSizeLarger = "Back image size lager.";
        public static string SelfieImageSizeLarger = "Selfie image size lager.";
        public static string Surname = "Please enter surname.";
        public static string GivenName = "Please enter given name.";
        public static string ValidIdentityCard = "Please enter valid identity card.";
        public static string PersonalIdentityInsertSuccessfull = "Personal identity KYC request success full insert.";
        public static string PersonalIdentityNotInserted = "Personal identity KYC request success full not inserted.";
        // ================================ SignalR ========================= //
        public static string SignalRTrnSuccessfullyCreated = "Your Transacton Successfully created Price=#Price# ,Qty=#Qty#.";
        public static string SignalRTrnSuccessfullySettled = "Your Transacton settled. Price=#Price# ,Qty=#Qty# ,Total=#Total#";
        public static string SignalRTrnSuccessPartialSettled = "Your Transacton Partial settled. Price=#Price# ,Qty=#Qty# ,Total=#Total#";


        public static string PushNotificationSubscriptionSuccess = "Notification Subscribed Successfully.";
        public static string PushNotificationunsubscriptionSuccess = "Notification Unsubscribed Successfully.";
        public static string PushNotificationSubscriptionFail = "Notification Subscription Failed.";
        public static string PushNotificationUnsubscriptionFail = "Notification Unsubscription Failed.";



    }


}
