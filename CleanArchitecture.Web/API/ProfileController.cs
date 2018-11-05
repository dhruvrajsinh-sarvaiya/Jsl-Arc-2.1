using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.Profile_Management;
using CleanArchitecture.Core.ViewModels.Profile_Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        #region Field
        private readonly IProfileMaster _IprofileMaster;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISubscriptionMaster _IsubscriptionMaster;
        #endregion

        #region Ctore
        public ProfileController(IProfileMaster IprofileMaster, UserManager<ApplicationUser> userManager,ISubscriptionMaster IsubscriptionMaster)
        {
            _IprofileMaster = IprofileMaster;
            _userManager = userManager;
            _IsubscriptionMaster = IsubscriptionMaster;
        }
        #endregion

        #region Methods
        [HttpGet("GetProfileData")]
        public async Task<ActionResult> GetProfileData()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var ProfileList = _IprofileMaster.GetProfileData(user.Id);
                    if (ProfileList != null)
                        return Ok(new ProfileMasterResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessGetProfilePlan, ProfileList = ProfileList });
                    else
                        return BadRequest(new ProfileMasterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ProfilePlan, ErrorCode = enErrorCode.Status4112ProfilePlan });
                }
                else
                    return BadRequest(new ProfileMasterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4063UserNotRegister });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProfileMasterResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpPost("AddProfile")]
        public async Task<ActionResult> AddProfile(MultipalSubscriptionReqViewModel model)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    bool Status = _IsubscriptionMaster.GetSubscriptionData(user.Id, model.ProfileId);
                    if (Status)
                    {
                        _IsubscriptionMaster.AddMultiSubscription(user.Id, model.ProfileId);
                        return Ok(new SubscriptionResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessAddProfile });
                    }
                    else
                        return BadRequest(new SubscriptionResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.NotAddedProfile, ErrorCode = enErrorCode.Status4122NotAddedProfile });
                }
                else
                    return BadRequest(new SubscriptionResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.SignUpUser, ErrorCode = enErrorCode.Status4063UserNotRegister });
            }
            catch (Exception ex)
            {
                return BadRequest(new SubscriptionResponse { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        #endregion

        #region Helpers
        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        #endregion
    }
}