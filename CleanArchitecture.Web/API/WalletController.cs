using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.ViewModels.Wallet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CleanArchitecture.Core.ViewModels.Wallet.AddWalletResponse;
using static CleanArchitecture.Core.ViewModels.Wallet.CreateWalletResponse;
using static CleanArchitecture.Core.ViewModels.Wallet.GetWalletByAddressResponse;
using static CleanArchitecture.Core.ViewModels.Wallet.GetWalletResponse;
using static CleanArchitecture.Core.ViewModels.Wallet.ListWalletResponse;
using static CleanArchitecture.Core.ViewModels.Wallet.UpdateWalletResponse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]/[action]")]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;

        public WalletController(ILogger<WalletController> logger)
        {
            _logger = logger;
        }
        static int i = 1;
        private ActionResult returnDynamicResult(dynamic respObjJson)
        {
            i++;
            if (i % 2 == 0)
            {
                return Ok(respObjJson);
            }
            else if (i % 3 == 0)
            {
                return BadRequest();
            }
            else if (i % 5 == 0)
            {
                return Unauthorized();
            }
            else if (i % 7 == 0)
            {
                return NotFound();
            }
            else if (i % 9 == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(respObjJson);
            }

        }
        #region"Methods"

        /// <summary>
        /// vsolanki 1-10-2018 Create Wallet
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet("{coin}")]
       // [Route("{coin}")]
        public ActionResult ListWallet(string coin, int limit=25,string prevId=null,bool allTokens=false)
        {
            try
            {
                string requeststring = "{'wallets':[{'id':'585951a5df8380e0e3063e9f','users':[{'user':'55e8a1a5df8380e0e30e20c6','permissions':['admin','view','spend']}],'coin':'tbtc','label':'Alexs first wallet','m':2,'n':3,'keys':['585951a5df8380e0e304a553','585951a5df8380e0e30d645c','585951a5df8380e0e30b6147'],'tags':['585951a5df8380e0e30a198a'],'disableTransactionNotifications':false,'freeze':{},'deleted':false,'approvalsRequired':1,'coinSpecific':{}}]}";
                ListWalletRootObject Response = new ListWalletRootObject();
                Response = JsonConvert.DeserializeObject<ListWalletRootObject>(requeststring);
                Response.StatusCode = 200;
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                return returnDynamicResult(respObjJson);           
            }
            catch(Exception ex)
            {
                _logger.LogError(1, DateTime.Now + "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }    
        }

        /// <summary>
        /// vsolanki 1-10-2018 Create Wallet
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost("{coin}")]
        public ActionResult CreateWallet([FromBody]CreateWalletRequest Request,string coin)
        {
            string requeststring = "{'wallet':{'_wallet':{'id':'591a40dd9fdde805252f0d8aefed79b3','users':[{'user':'55cce42633dc60ca06db38e643622a86','permissions':['admin','view','spend']}],'coin':'tltc','label':'My Test Wallet','m':2,'n':3,'keys':['591a40dc422326ff248919e62a02b2be','591a40dd422326ff248919e91caa8b6a','591a40dc9fdde805252f0d87f76577f8'],'tags':['591a40dd9fdde805252f0d8a'],'disableTransactionNotifications':false,'freeze':{},'deleted':false,'approvalsRequired':1,'isCold':false,'coinSpecific':{},'balance':0,'confirmedBalance':0,'spendableBalance':0,'balanceString':'0','confirmedBalanceString':'0','spendableBalanceString':'0','receiveAddress':{'address':'QRWXF9VxJnbSx5uYbX99kuw55pUW4DrmTx','chain':0,'index':0,'coin':'tltc','wallet':'591a40dd9fdde805252f0d8aefed79b3','coinSpecific':{'redeemScript':'522103e8bc…c7ac0e53ae'}},'pendingApprovals':[]}}}";
            CreateWalletRootObject Response = new CreateWalletRootObject();
            Response = JsonConvert.DeserializeObject<CreateWalletRootObject>(requeststring);
            Response.StatusCode = 200;
            var respObj = JsonConvert.SerializeObject(Response);
            dynamic respObjJson = JObject.Parse(respObj);
            return returnDynamicResult(respObjJson);
        }

        /// <summary>
        /// vsolanki 1-10-2018 Add Wallet
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost("{coin}")]
        public ActionResult AddWallet([FromBody]AddWalletRequest Request)
        {
            string requeststring = "{'wallet':{'_wallet':{'id':'591a40dd9fdde805252f0d8aefed79b3','users':[{'user':'55cce42633dc60ca06db38e643622a86','permissions':['admin','view','spend']}],'coin':'teth','label':'My Wallet','m':2,'n':3,'keys':['591a40dc422326ff248919e62a02b2be','591a40dd422326ff248919e91caa8b6a','591a40dc9fdde805252f0d87f76577f8'],'tags':['591a40dd9fdde805252f0d8a'],'disableTransactionNotifications':false,'freeze':{},'deleted':false,'approvalsRequired':1,'isCold':false,'coinSpecific':{'deployedInBlock':false,'deployTxHash':'0x37b4092509254d60a4c29464f6979dcdaa3b10bd7fa5e388380f30b94efa43bf','lastChainIndex':{'0':-1,'1':-1},'baseAddress':'0x10c208fa7afe710eb47272c0827f58d3d524932a','feeAddress':'0xb0e3a0f647300a1656c1a46c21bbb9ed93bf19ab','pendingChainInitialization':true,'creationFailure':[]},'balance':0,'confirmedBalance':0,'spendableBalance':0,'balanceString':'0','confirmedBalanceString':'0','spendableBalanceString':'0','pendingApprovals':[]}}}";
            AddWalletRootObject Response = new AddWalletRootObject();
            Response = JsonConvert.DeserializeObject<AddWalletRootObject>(requeststring);
            Response.StatusCode = 200;
            var respObj = JsonConvert.SerializeObject(Response);
            dynamic respObjJson = JObject.Parse(respObj);
            return returnDynamicResult(respObjJson);
        }

        /// <summary>
        /// vsolanki 1-10-2018 Get Wallet
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult GetWallet(string id,bool allTokens=false)
        {
            string requeststring = "{'id':'585c51a5df8380e0e3082e46','users':[{'user':'55e8a1a5df8380e0e30e20c6','permissions':['admin','view','spend']}],'coin':'tbtc','label':'My first wallet','m':2,'n':3,'keys':['585951a5df8380e0e304a553','585951a5df8380e0e30d645c','585951a5df8380e0e30b6147'],'tags':['585951a5df8380e0e30a198a'],'disableTransactionNotifications':false,'freeze':{},'deleted':false,'approvalsRequired':1,'coinSpecific':{},'balance':0,'confirmedBalance':0,'spendableBalance':0,'balanceString':0,'confirmedBalanceString':0,'spendableBalanceString':0,'receiveAddress':{'address':'2MyzG53Z6nF7UdNt7otEMtGNiEAEe2t2eSY','chain':0,'index':3,'coin':'tbtc','wallet':'597a1eb8a4db5fb37729887e87d18ab5','coinSpecific':{'redeemScript':'52210338ee0dce7dd0ce8bba6686c0d0ef6d55811b7369144367ae11f70a361a390d812103ebc32642cba79aefb993f7646a923a2163cbd0134a2b0cf5f71c55a80b067bef210348487f5f97bc53e0155516cbb41650686988ae87de105f6035bd444cd2de605f53ae'}},'admin':{'policy':{'id':'597a1eb8a4db5fb37729887f0c3c042b','label':'default','version':4,'date':'2017-05-12T17:57:21.800Z','rules':[{'id':'pYUq7enNoX32VprHfWHuzFyCHS7','coin':'tbtc','type':'velocityLimit','action':{'type':'getApproval'},'condition':{'amountString':'100000000','timeWindow':0,'groupTags':[':tag'],'excludeTags':[]}}]}}}";
            GetWalletRootObject Response = new GetWalletRootObject();
            Response = JsonConvert.DeserializeObject<GetWalletRootObject>(requeststring);
            Response.StatusCode = 200;
            var respObj = JsonConvert.SerializeObject(Response);
            dynamic respObjJson = JObject.Parse(respObj);
            return returnDynamicResult(respObjJson);
        }

        /// <summary>
        /// vsolanki 1-10-2018 Update Wallet
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPut("{coin}")]
        public ActionResult UpdateWallet([FromBody]UpdateWalletRequest Request)
        {
            string requeststring = "{'id':'585c51a5df8380e0e3082e46','users':[{'user':'55e8a1a5df8380e0e30e20c6','permissions':['admin','view','spend']}],'coin':'tbtc','label':'My first wallet','m':2,'n':3,'keys':['585951a5df8380e0e304a553','585951a5df8380e0e30d645c','585951a5df8380e0e30b6147'],'tags':['585951a5df8380e0e30a198a'],'disableTransactionNotifications':false,'freeze':{},'deleted':false,'approvalsRequired':1,'coinSpecific':{},'balance':0,'confirmedBalance':0,'spendableBalance':0,'balanceString':0,'confirmedBalanceString':0,'spendableBalanceString':0,'receiveAddress':{'address':'2MyzG53Z6nF7UdNt7otEMtGNiEAEe2t2eSY','chain':0,'index':3,'coin':'tbtc','wallet':'597a1eb8a4db5fb37729887e87d18ab5','coinSpecific':{'redeemScript':'52210338ee0dce7dd0ce8bba6686c0d0ef6d55811b7369144367ae11f70a361a390d812103ebc32642cba79aefb993f7646a923a2163cbd0134a2b0cf5f71c55a80b067bef210348487f5f97bc53e0155516cbb41650686988ae87de105f6035bd444cd2de605f53ae'}},'admin':{'policy':{'id':'597a1eb8a4db5fb37729887f0c3c042b','label':'default','version':4,'date':'2017-05-12T17:57:21.800Z','rules':[{'id':'pYUq7enNoX32VprHfWHuzFyCHS7','coin':'tbtc','type':'velocityLimit','action':{'type':'getApproval'},'condition':{'amountString':'100000000','timeWindow':0,'groupTags':[':tag'],'excludeTags':[]}}]}}}";
            UpdateWalletRootObject Response = new UpdateWalletRootObject();
            Response = JsonConvert.DeserializeObject<UpdateWalletRootObject>(requeststring);
            Response.StatusCode = 200;
            var respObj = JsonConvert.SerializeObject(Response);
            dynamic respObjJson = JObject.Parse(respObj);
            return returnDynamicResult(respObjJson);
        }

        /// <summary>
        /// vsolanki 1-10-2018 Get Wallet By Address
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet("{coin}/{address}")]
        public ActionResult GetWalletByAddress(string address,string coin)
        {
            string requeststring = "{'id':'585951a5df8380e0e3063e9f','users':[{'user':'55e8a1a5df8380e0e30e20c6','permissions':['admin','view','spend']}],'coin':'tbtc','label':'My first wallet','m':2,'n':3,'keys':['585951a5df8380e0e304a553','585951a5df8380e0e30d645c','585951a5df8380e0e30b6147'],'tags':['585951a5df8380e0e30a198a'],'disableTransactionNotifications':false,'freeze':{},'deleted':false,'approvalsRequired':1,'coinSpecific':{},'balance':0,'confirmedBalance':0,'spendableBalance':0,'balanceString':0,'confirmedBalanceString':0,'spendableBalanceString':0,'receiveAddress':{'address':'2MyzG53Z6nF7UdNt7otEMtGNiEAEe2t2eSY','chain':0,'index':3,'coin':'tbtc','wallet':'597a1eb8a4db5fb37729887e87d18ab5','coinSpecific':{'redeemScript':'52210338ee0dce7dd0ce8bba6686c0d0ef6d55811b7369144367ae11f70a361a390d812103ebc32642cba79aefb993f7646a923a2163cbd0134a2b0cf5f71c55a80b067bef210348487f5f97bc53e0155516cbb41650686988ae87de105f6035bd444cd2de605f53ae'}},'admin':{'policy':{'id':'597a1eb8a4db5fb37729887f0c3c042b','label':'default','version':4,'date':'2017-05-12T17:57:21.800Z','rules':[{'id':'pYUq7enNoX32VprHfWHuzFyCHS7','coin':'tbtc','type':'velocityLimit','action':{'type':'getApproval'},'condition':{'amountString':'100000000','timeWindow':0,'groupTags':[':tag'],'excludeTags':[]}}]}}}";
            GetWalletByAddressRootObject Response = new GetWalletByAddressRootObject();
            Response = JsonConvert.DeserializeObject<GetWalletByAddressRootObject>(requeststring);
            Response.StatusCode = 200;
            var respObj = JsonConvert.SerializeObject(Response);
            dynamic respObjJson = JObject.Parse(respObj);
            return returnDynamicResult(respObjJson);
        }

        #endregion
    }
}
