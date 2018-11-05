using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.Complaint;
using CleanArchitecture.Core.ViewModels.Complaint;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]")]
    [ApiController]

    public class ComplaintController : ControllerBase
    {
        private readonly ITypemaster _typemaster;
        private readonly IComplainmaster _Icomplainmaster;
        private readonly ICompainTrail _IcompainTrail;
        public ComplaintController(ITypemaster typemaster, IComplainmaster complainmaster, ICompainTrail compainTrail)
        {
            _typemaster = typemaster;
            _Icomplainmaster = complainmaster;
            _IcompainTrail = compainTrail;
        }
        [HttpGet("GetTypeMaster")]
        public async Task<IActionResult> GetTypeMaster(string Type)
        {
            try
            {
                if (!string.IsNullOrEmpty(Type))
                {
                    var DeviceMasterList = _typemaster.GettypeMaster(Type);
                    if (DeviceMasterList.Count > 0)
                        return Ok(new TypeMasterResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessGetDeviceData, TypeMasterList = DeviceMasterList });
                    else

                        return BadRequest(new TypeMasterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.complaintTypeNotavailable, ErrorCode = enErrorCode.status4121complaintTypeNotavailable });
                }
                else
                {
                    return BadRequest(new TypeMasterResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.Typemasterrequired, ErrorCode = enErrorCode.Typemasterrequired4109 });
                }


            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }

        }
        [HttpPost("Raisecomplaint")]
        public async Task<IActionResult> Raisecomplaint(ComplainmasterViewModel model)
        {
            try
            {
                if (model != null)
                {
                    long id = _Icomplainmaster.AddComplainmaster(model);

                    if (id > 0)
                    {
                        var compainTrail = new CompainTrailVirewModel()
                        {
                            ComplainId = id,
                            Description = model.Description,
                            UserID = model.UserID
                        };
                        long idcompainTrail = _IcompainTrail.AddCompainTrail(compainTrail);

                        return Ok(new ComplainmasterResonse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessAddComplain });
                    }
                    else
                    {
                        return BadRequest(new ComplainmasterResonse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.TypemasterInsertError, ErrorCode = enErrorCode.Status4110TypemasterInsertError });
                    }

                }
                return BadRequest(new ComplainmasterResonse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.AddCompainrequired, ErrorCode = enErrorCode.Status4111AddCompainrequired });
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }


        [HttpPost("AddCompainTrail")]
        public async Task<IActionResult> AddCompainTrail(CompainTrailVirewModel model)
        {
            try
            {
                if (model != null)
                {
                    long id = _IcompainTrail.AddCompainTrail(model);
                    if (id > 0)
                    {
                        return Ok(new CompainTrailResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessCompainTrail });
                    }
                    else
                    {
                        return BadRequest(new CompainTrailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.CompainTrailInsertError, ErrorCode = enErrorCode.Status4114CompainTrailInsertError });
                    }
                }
                else
                {
                    return BadRequest(new CompainTrailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.AddCompainTrail, ErrorCode = enErrorCode.Status4113AddCompainTrail });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }

        [HttpGet("GetComplain")]
        public async Task<IActionResult> GetComplain(int ComplainId)
        {
            try
            {
                var response = _Icomplainmaster.GetComplain(ComplainId);
                if (response.Count > 0)
                {
                    return Ok(new ComplainmasterResonse { ReturnCode = enResponseCode.Success, compainDetailResponses = response, ReturnMsg = EnResponseMessage.SuccessGetCompainDetail });

                }
                else
                {
                    return BadRequest(new CompainTrailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.Complaintdatanotavailable, ErrorCode = enErrorCode.status4116Complaintdatanotavailable });
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }

        [HttpGet("GetUserWiseComplain")]
        public async Task<IActionResult> GetUserWiseComplain(int UserId)
        {
            try
            {


                var response = _Icomplainmaster.GetComplainByUserWise(UserId);
                if (response.Count > 0)
                {
                    return Ok(new ComplainmasterResonse { ReturnCode = enResponseCode.Success, userWiseCompaintDetailResponces = response, ReturnMsg = EnResponseMessage.SuccessGetCompainDetail });

                }
                else
                {
                    return BadRequest(new CompainTrailResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.Complaintdatanotavailable, ErrorCode = enErrorCode.status4116Complaintdatanotavailable });
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
    }
}