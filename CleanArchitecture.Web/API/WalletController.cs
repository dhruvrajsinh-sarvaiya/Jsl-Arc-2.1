using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CleanArchitecture.Web.Helper;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Core.ApiModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class WalletController : Controller
    {
        private readonly IWalletService _walletService;
        private readonly IBasePage _basePage;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<WalletController> _logger;
        public WalletController(ILogger<WalletController> logger, IBasePage basePage, UserManager<ApplicationUser> userManager, IWalletService walletService)
        {
            _logger = logger;
            _basePage = basePage;
            _userManager = userManager;
            _walletService = walletService;
        }

        #region"Methods"
        /// <summary>
        /// vsolanki 12-10-2018 List  All Wallet Of Particular User
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ListWallet()
        {
            // ApplicationUser user = new ApplicationUser();user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListWalletResponse Response = new ListWalletResponse();
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                HelperForLog.WriteLogIntoFile(1, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, "", accessToken);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.ListWallet(user.Id);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        /// <summary>
        /// vsolanki 1-10-2018 Create Wallet
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost("{Coin}")]
        public async Task<IActionResult> CreateWallet([FromBody]CreateWalletRequest Request, string Coin)
        {
            //  ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            //HelperForLog.WriteLogIntoFile(1, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Request), accessToken);
            CreateWalletResponse Response = new CreateWalletResponse();
            try
            {
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.InsertIntoWalletMaster(Request.WalletName, Coin, Request.IsDefaultWallet, Request.AllowTrnType, Convert.ToInt64(user.Id), accessToken, 0);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                Response.ReturnCode = enResponseCode.InternalError;
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        /// <summary>
        /// vsolanki 12-10-2018 Get Wallet by coin name
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet("{Coin}")]
        public async Task<IActionResult> GetWalletByType(string Coin)
        {
            //ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListWalletResponse Response = new ListWalletResponse();
            try
            {
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetWalletByCoin(user.Id, Coin);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        /// <summary>
        /// vsolanki 13-10-2018 Get Wallet by coin name
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet("{Coin}/{WalletId}")]
        public async Task<IActionResult> GetWalletByWalletId(string Coin, string WalletId)
        {
            //ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListWalletResponse Response = new ListWalletResponse();
            try
            {
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetWalletById(user.Id, Coin, WalletId);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-16
        [HttpGet("{FromDate}/{ToDate}")]
        public async Task<IActionResult> DepositHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status)
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            DepositHistoryResponse response = new DepositHistoryResponse();
            try
            {
                if (user == null)
                {
                    response.ReturnCode = enResponseCode.Fail;
                    response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    response = _walletService.DepositHistoy(FromDate, ToDate, Coin, Amount, Status, user.Id);
                }

                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(response), "");
                return Ok(response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-16
        [HttpGet("{FromDate}/{ToDate}")]
        //  [AllowAnonymous]
        public async Task<IActionResult> WithdrawalHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status)
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            DepositHistoryResponse response = new DepositHistoryResponse();
            try
            {
                if (user == null)
                {
                    response.ReturnCode = enResponseCode.Fail;
                    response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    response = _walletService.WithdrawalHistoy(FromDate, ToDate, Coin, Amount, Status, user.Id);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(response), "");
                return Ok(response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-24
        [HttpGet("{WalletId}")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAvailableBalance(string WalletId)
        {
            //ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetAvailableBalance(user.Id, WalletId);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAvailableBalance()
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            TotalBalanceRes Response = new TotalBalanceRes();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetAllAvailableBalance(user.Id);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-24
        [HttpGet("{WalletId}")]
        public async Task<IActionResult> GetUnSettledBalance(string WalletId)
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 16;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetUnSettledBalance(user.Id, WalletId);
                }

                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUnSettledBalance()
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetAllUnSettledBalance(user.Id);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-24
        [HttpGet("{WalletId}")]
        public async Task<IActionResult> GetUnClearedBalance(string WalletId)
        {
            //ApplicationUser user = new ApplicationUser(); user.Id = 20;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetUnClearedBalance(user.Id, WalletId);
                }

                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUnClearedBalance()
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 20;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetAllUnClearedBalance(user.Id);
                }

                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-24
        [HttpGet("{WalletId}")]
        public async Task<IActionResult> GetStackingBalance(string WalletId)
        {
            //ApplicationUser user = new ApplicationUser(); user.Id = 20;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListStackingBalanceRes Response = new ListStackingBalanceRes();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetStackingBalance(user.Id, WalletId);
                }

                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStackingBalance()
        {
            //ApplicationUser user = new ApplicationUser(); user.Id = 20;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListStackingBalanceRes Response = new ListStackingBalanceRes();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetAllStackingBalance(user.Id);
                }

                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-24
        [HttpGet("{WalletId}")]
        public async Task<IActionResult> GetShadowBalance(string WalletId)
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 20;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetShadowBalance(user.Id, WalletId);
                }

                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShadowBalance()
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 20;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetAllShadowBalance(user.Id);
                }

                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-24
        //[AllowAnonymous]
        // [AllowAnonymous]
        [HttpGet("{WalletId}")]
        public async Task<IActionResult> GetAllBalances(string WalletId)
        {
            //ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            AllBalanceResponse Response = new AllBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetAllBalances(user.Id, WalletId);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 25-10-2018
        //[AllowAnonymous]
        //[AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAvailbleBalTypeWise()
        {
            //  ApplicationUser user = new ApplicationUser(); user.Id = 35;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            BalanceResponseWithLimit Response = new BalanceResponseWithLimit();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetAvailbleBalTypeWise(user.Id);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-25
        //[AllowAnonymous]
        [HttpGet("{WalletType}")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAllBalancesTypeWise(string WalletType)
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 35;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListAllBalanceTypeWiseRes Response = new ListAllBalanceTypeWiseRes();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetAllBalancesTypeWise(user.Id, WalletType);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                // HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-26
        // [AllowAnonymous]
        // [AllowAnonymous]
        [HttpGet("{FromDate}/{ToDate}/{WalletId}")]
        public async Task<IActionResult> GetWalletLedger(DateTime FromDate, DateTime ToDate, string WalletId, int Page)
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListWalletLedgerRes Response = new ListWalletLedgerRes();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetWalletLedger(FromDate, ToDate, WalletId, Page);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateDefaulWallet()
        {
            //ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            BizResponseClass Response = new BizResponseClass();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            try
            {
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.CreateDefaulWallet(user.Id,accessToken);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                // HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        // [AllowAnonymous]
        [HttpPost("{WalletType}")]
        public async Task<IActionResult> CreateWalletForAllUser_NewService(string WalletType)
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            BizResponseClass Response = new BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.CreateWalletForAllUser_NewService(WalletType);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                // HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddBizUserTypeMapping([FromBody] AddBizUserTypeMappingReq req)
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 1;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            BizResponseClass Response = new BizResponseClass();
            try
            {
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.AddBizUserTypeMapping(req);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-10-29
        //[AllowAnonymous]
        //[AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetIncomingTransaction(string Coin)
        {
            //ApplicationUser user = new ApplicationUser(); user.Id = 35;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListIncomingTrnRes Response = new ListIncomingTrnRes();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetIncomingTransaction(user.Id, Coin);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                // HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //Uday 30-10-2018
        [HttpGet("{TrnType}/{CoinName}")]
        public ActionResult GetServiceLimitChargeValue(enTrnType TrnType, string CoinName)
        {
            ServiceLimitChargeValueResponse Response = new ServiceLimitChargeValueResponse();
            ServiceLimitChargeValueResponseStr ResponseStr = new ServiceLimitChargeValueResponseStr();
            ServiceLimitChargeValueStr subObj = new ServiceLimitChargeValueStr();
            try
            {
                var responseData = _walletService.GetServiceLimitChargeValue(TrnType, CoinName);
                if (responseData != null)
                {
                    Response.response = responseData;
                    Response.ReturnCode = enResponseCode.Success;
                    subObj.ChargeType = Response.response.ChargeType;
                    subObj.ChargeValue = Response.response.ChargeValue;
                    subObj.CoinName = Response.response.CoinName;
                    subObj.MaxAmount = Response.response.MaxAmount.ToString();
                    subObj.MinAmount = Response.response.MinAmount.ToString();
                    subObj.ChargeType = Response.response.ChargeType;

                    ResponseStr.ReturnCode = Response.ReturnCode;
                    ResponseStr.ErrorCode = Response.ErrorCode;
                    ResponseStr.ReturnMsg = Response.ReturnMsg;


                }
                else
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.NotFound;
                    Response.ErrorCode = enErrorCode.NotFound;
                    ResponseStr.ReturnCode = enResponseCode.Fail;
                    ResponseStr.ReturnMsg = EnResponseMessage.NotFound;
                    ResponseStr.ErrorCode = enErrorCode.NotFound;
                }
                return Ok(ResponseStr);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsolanki 2018-11-02
       // [AllowAnonymous]
        [HttpGet]
       // [AllowAnonymous]
        public async Task<IActionResult> GetOutGoingTransaction(string Coin)
        {
            // ApplicationUser user = new ApplicationUser(); user.Id = 35;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListOutgoingTrnRes Response = new ListOutgoingTrnRes();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetOutGoingTransaction(user.Id, Coin);
                }
                //HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
                return Ok(Response);
            }
            catch (Exception ex)
            {
                // HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsoalnki 2018-11-3
        [HttpGet("{FromDate}/{ToDate}")]
        //[AllowAnonymous]
        public async Task<IActionResult> ConvertFundHistory(DateTime FromDate, DateTime ToDate, string Coin)
        {
           // ApplicationUser user = new ApplicationUser(); user.Id = 35;
             ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ListTokenConvertHistoryRes Response = new ListTokenConvertHistoryRes();
            try
            {
                if (user == null)
                {
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.BizResponseObj.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.ConvertFundHistory(user.Id, FromDate, ToDate, Coin);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsoalnki 2018-11-3
        [HttpPost("{SourcePrice}")]
        //[AllowAnonymous]
        public async Task<IActionResult> ConvertFund(decimal SourcePrice)
        {           
            decimal DestinationPrice = 0;
            BizResponseClass c = null;
            try
            {
                DestinationPrice = _walletService.ConvertFund(SourcePrice);
                if(DestinationPrice==0)
                {
                    c= new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidAmt, ErrorCode = enErrorCode.InvalidAmount };
                }
                else
                {
                    return Ok("DestinationPrice:" +DestinationPrice);
                }
                return Ok(c);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        //vsoalnki 2018-11-3
        [HttpPost]
       //[AllowAnonymous]
        public async Task<IActionResult> AddIntoConvertFund(ConvertTockenReq Request)
        {
            //ApplicationUser user = new ApplicationUser(); user.Id = 35;
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            BizResponseClass Response = new BizResponseClass();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            try
            {
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response =_walletService.AddIntoConvertFund(Request,user.Id, accessToken);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BizResponseClass { ReturnCode = enResponseCode.InternalError, ReturnMsg = ex.ToString(), ErrorCode = enErrorCode.Status500InternalServerError });
            }
        }

        /// <summary>
        /// vsolanki 8-10-2018 Get the coin list 
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public IActionResult CoinList()
        //{
        //    var items = _walletService.GetWalletTypeMaster();
        //    HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(items), "");
        //    return Ok(items);
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetAllAvailableBalance()
        //{
        //    ListBalanceResponse Response = new ListBalanceResponse();
        //    try
        //    {
        //        var user = new ApplicationUser();
        //        user.Id = 1;//await _userManager.GetUserAsync(HttpContext.User);
        //        if (user == null)
        //        {
        //            Response.ReturnCode = enResponseCode.Fail;
        //            Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
        //            Response.ErrorCode = enErrorCode.StandardLoginfailed;
        //        }
        //        else
        //        {
        //            Response = _walletService.GetAllAvailableBalance(user.Id);
        //        }
        //        HelperForLog.WriteLogIntoFile(2, _basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, JsonConvert.SerializeObject(Response), "");
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperForLog.WriteErrorLog(_basePage.UTC_To_IST(), this.ControllerContext.RouteData.Values["action"].ToString(), this.GetType().Name, ex.ToString());
        //        return BadRequest();
        //    }
        //}



        ///// <summary>
        ///// vsolanki 1-10-2018 Add Wallet
        ///// </summary>
        ///// <param name="Request"></param>
        ///// <returns></returns>
        //[HttpPost("{coin}")]
        //public async Task<IActionResult> AddWallet([FromBody]AddWalletRequest Request)
        //{
        //    try
        //    {
        //        string requeststring = "{'wallet':{'_wallet':{'id':'591a40dd9fdde805252f0d8aefed79b3','users':[{'user':'55cce42633dc60ca06db38e643622a86','permissions':['admin','view','spend']}],'coin':'teth','label':'My Wallet','m':2,'n':3,'keys':['591a40dc422326ff248919e62a02b2be','591a40dd422326ff248919e91caa8b6a','591a40dc9fdde805252f0d87f76577f8'],'tags':['591a40dd9fdde805252f0d8a'],'disableTransactionNotifications':false,'freeze':{},'deleted':false,'approvalsRequired':1,'isCold':false,'coinSpecific':{'deployedInBlock':false,'deployTxHash':'0x37b4092509254d60a4c29464f6979dcdaa3b10bd7fa5e388380f30b94efa43bf','lastChainIndex':{'0':-1,'1':-1},'baseAddress':'0x10c208fa7afe710eb47272c0827f58d3d524932a','feeAddress':'0xb0e3a0f647300a1656c1a46c21bbb9ed93bf19ab','pendingChainInitialization':true,'creationFailure':[]},'balance':0,'confirmedBalance':0,'spendableBalance':0,'balanceString':'0','confirmedBalanceString':'0','spendableBalanceString':'0','pendingApprovals':[]}}}";
        //        AddWalletResponse Response = new AddWalletResponse();
        //        Response = JsonConvert.DeserializeObject<AddWalletResponse>(requeststring);
        //        Response.ReturnCode = enResponseCode.Success;
        //        var respObj = JsonConvert.SerializeObject(Response, Newtonsoft.Json.Formatting.Indented,
        //                                                  new JsonSerializerSettings
        //                                                  {
        //                                                      NullValueHandling = NullValueHandling.Ignore
        //                                                  });
        //        dynamic respObjJson = JObject.Parse(respObj);
        //        return returnDynamicResult(respObjJson);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
        //        return BadRequest();
        //    }
        //}

        ///// <summary>
        ///// vsolanki 1-10-2018 Update Wallet
        ///// </summary>
        ///// <param name="Request"></param>
        ///// <returns></returns>
        //[HttpPut("{coin}")]
        //public async Task<IActionResult> UpdateWallet([FromBody]UpdateWalletRequest Request)
        //{
        //    try
        //    {
        //        string requeststring = "{'id':'585c51a5df8380e0e3082e46','users':[{'user':'55e8a1a5df8380e0e30e20c6','permissions':['admin','view','spend']}],'coin':'tbtc','label':'My first wallet','m':2,'n':3,'keys':['585951a5df8380e0e304a553','585951a5df8380e0e30d645c','585951a5df8380e0e30b6147'],'tags':['585951a5df8380e0e30a198a'],'disableTransactionNotifications':false,'freeze':{},'deleted':false,'approvalsRequired':1,'coinSpecific':{},'balance':0,'confirmedBalance':0,'spendableBalance':0,'balanceString':0,'confirmedBalanceString':0,'spendableBalanceString':0,'receiveAddress':{'address':'2MyzG53Z6nF7UdNt7otEMtGNiEAEe2t2eSY','chain':0,'index':3,'coin':'tbtc','wallet':'597a1eb8a4db5fb37729887e87d18ab5','coinSpecific':{'redeemScript':'52210338ee0dce7dd0ce8bba6686c0d0ef6d55811b7369144367ae11f70a361a390d812103ebc32642cba79aefb993f7646a923a2163cbd0134a2b0cf5f71c55a80b067bef210348487f5f97bc53e0155516cbb41650686988ae87de105f6035bd444cd2de605f53ae'}},'admin':{'policy':{'id':'597a1eb8a4db5fb37729887f0c3c042b','label':'default','version':4,'date':'2017-05-12T17:57:21.800Z','rules':[{'id':'pYUq7enNoX32VprHfWHuzFyCHS7','coin':'tbtc','type':'velocityLimit','action':{'type':'getApproval'},'condition':{'amountString':'100000000','timeWindow':0,'groupTags':[':tag'],'excludeTags':[]}}]}}}";
        //        UpdateWalletResponse Response = new UpdateWalletResponse();
        //        Response = JsonConvert.DeserializeObject<UpdateWalletResponse>(requeststring);
        //        Response.ReturnCode = enResponseCode.Success;
        //        var respObj = JsonConvert.SerializeObject(Response, Newtonsoft.Json.Formatting.Indented,
        //                                                                 new JsonSerializerSettings
        //                                                                 {
        //                                                                     NullValueHandling = NullValueHandling.Ignore
        //                                                                 });
        //        dynamic respObjJson = JObject.Parse(respObj);
        //        return returnDynamicResult(respObjJson);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
        //        return BadRequest();
        //    }
        //}

        ///// <summary>
        ///// vsolanki 1-10-2018 Get Wallet By Address
        ///// </summary>
        ///// <param name="Request"></param>
        ///// <returns></returns>
        //[HttpGet("{coin}/{address}")]
        //public async Task<IActionResult> GetWalletByAddress(string address, string coin)
        //{
        //    try
        //    {
        //        string requeststring = "{'id':'585951a5df8380e0e3063e9f','users':[{'user':'55e8a1a5df8380e0e30e20c6','permissions':['admin','view','spend']}],'coin':'tbtc','label':'My first wallet','m':2,'n':3,'keys':['585951a5df8380e0e304a553','585951a5df8380e0e30d645c','585951a5df8380e0e30b6147'],'tags':['585951a5df8380e0e30a198a'],'disableTransactionNotifications':false,'freeze':{},'deleted':false,'approvalsRequired':1,'coinSpecific':{},'balance':0,'confirmedBalance':0,'spendableBalance':0,'balanceString':0,'confirmedBalanceString':0,'spendableBalanceString':0,'receiveAddress':{'address':'2MyzG53Z6nF7UdNt7otEMtGNiEAEe2t2eSY','chain':0,'index':3,'coin':'tbtc','wallet':'597a1eb8a4db5fb37729887e87d18ab5','coinSpecific':{'redeemScript':'52210338ee0dce7dd0ce8bba6686c0d0ef6d55811b7369144367ae11f70a361a390d812103ebc32642cba79aefb993f7646a923a2163cbd0134a2b0cf5f71c55a80b067bef210348487f5f97bc53e0155516cbb41650686988ae87de105f6035bd444cd2de605f53ae'}},'admin':{'policy':{'id':'597a1eb8a4db5fb37729887f0c3c042b','label':'default','version':4,'date':'2017-05-12T17:57:21.800Z','rules':[{'id':'pYUq7enNoX32VprHfWHuzFyCHS7','coin':'tbtc','type':'velocityLimit','action':{'type':'getApproval'},'condition':{'amountString':'100000000','timeWindow':0,'groupTags':[':tag'],'excludeTags':[]}}]}}}";
        //        GetWalletByAddressResponse Response = new GetWalletByAddressResponse();
        //        Response = JsonConvert.DeserializeObject<GetWalletByAddressResponse>(requeststring);
        //        Response.ReturnCode = enResponseCode.Success;
        //        var respObj = JsonConvert.SerializeObject(Response, Newtonsoft.Json.Formatting.Indented,
        //                                                                                 new JsonSerializerSettings
        //                                                                                 {
        //                                                                                     NullValueHandling = NullValueHandling.Ignore
        //                                                                                 });
        //        dynamic respObjJson = JObject.Parse(respObj);
        //        return returnDynamicResult(respObjJson);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
        //        return BadRequest();
        //    }
        //}
        #endregion
    }
}
