using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class WalletController : ControllerBase
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
        /// vsolanki 8-10-2018 Get the coin list 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CoinList()
        {
            var items = _walletService.GetWalletTypeMaster();
            return Ok(items);
        }

        /// <summary>
        /// vsolanki 12-10-2018 List  All Wallet Of Particular User
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ListWallet()
        {
            //ApplicationUser user = new ApplicationUser();
            ListWalletResponse Response = new ListWalletResponse();
            try
            {
                //user.Id = 1;
                // var items;
                var user = await _userManager.GetUserAsync(HttpContext.User);
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
                return Ok(Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }

        /// <summary>
        /// vsolanki 1-10-2018 Create Wallet
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost("{coin}")]
        public async Task<IActionResult> CreateWallet([FromBody]CreateWalletRequest Request, string coin)
        {
            CreateWalletResponse Response = new CreateWalletResponse();
            try
            {
               // ApplicationUser user = new ApplicationUser();
              //  user.Id = 1;
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.InsertIntoWalletMaster(Request.WalletName, coin, Request.IsDefaultWallet, Request.AllowTrnType, Convert.ToInt64(user.Id));
                }

                return Ok(Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                Response.ReturnCode = enResponseCode.InternalError;
                return BadRequest(Response);
            }
        }

        /// <summary>
        /// vsolanki 12-10-2018 Get Wallet by coin name
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet("{coin}")]
        public async Task<IActionResult> GetWalletByCoin(string coin)
        {
            ListWalletResponse Response = new ListWalletResponse();
            try
            {
                //ApplicationUser user = new ApplicationUser();
                //user.Id = 1;
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetWalletByCoin(user.Id, coin);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }

        /// <summary>
        /// vsolanki 13-10-2018 Get Wallet by coin name
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet("{coin}/{walletId}")]
        public async Task<IActionResult> GetWalletByWalletId(string coin,string walletId)
        {
            ListWalletResponse Response = new ListWalletResponse();
            try
            {
                //ApplicationUser user = new ApplicationUser();
                //user.Id = 1;
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                }
                else
                {
                    Response = _walletService.GetWalletById(user.Id, coin,walletId);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }

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
