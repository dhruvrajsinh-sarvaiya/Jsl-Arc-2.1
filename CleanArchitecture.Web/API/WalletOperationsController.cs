using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.ViewModels.WalletOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Core.ViewModels.WalletOperations;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CleanArchitecture.Core.ViewModels.WalletOperations.CreateWalletAddressRes;
using static CleanArchitecture.Core.ViewModels.WalletOperations.GetWalletAddressRes;
using static CleanArchitecture.Core.ViewModels.WalletOperations.GetWalletTransBySeqRes;
using static CleanArchitecture.Core.ViewModels.WalletOperations.GetWalletTransferRes;
using static CleanArchitecture.Core.ViewModels.WalletOperations.ListWalletTransfersResponse;
using static CleanArchitecture.Core.ViewModels.WalletOperations.SendTranToManyRes;
using static CleanArchitecture.Core.ViewModels.WalletOperations.UpdateWalletAddressRes;
using static CleanArchitecture.Core.ViewModels.WalletOperations.WithdrawalRes;

namespace CleanArchitecture.Web.API
{
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class WalletOperationsController : Controller
    {

        private readonly IBasePage _basePage;
        private readonly ILogger<WalletOperationsController> _logger;
        public WalletOperationsController(ILogger<WalletOperationsController> logger, IBasePage basePage)
        {
            _logger = logger;
            _basePage = basePage;
        }

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


        [HttpGet("{coin}/{id}")]
        //[Route("{coin}/{id}")]
        public ActionResult ListwalletTransfer(string id, string coin, string prevId = null, bool allToken = false, bool includeHex = false, string searchLabel = null, string type = null)
        {
            try
            {
                string requeststring = "{'coin':'tbtc','transfers':[{'id':'591623989c043ab2079857ee53d812f0','coin':'tbtc','wallet':'59161139c86cffa0074b614d07dfc29b','txid':'cc21875eb303e5efda9056585d68c79b10345585213a62c9c1a7bc331dfd5d93','normalizedTxHash':'e0e4bada8332ed254c20c4c1d2c25e5f13386509e7ac21d005a169925d07889a','height':1122442,'date':'2017-05-12T21:05:28.130Z','confirmations':1,'type':'receive','value':100000,'bitgoFee':0,'usd':1.79092,'usdRate':1790.92,'state':'confirmed','vSize':224,'nSegwitInputs':0,'tags':['591623989c043ab2079857ee'],'sequenceId':'','history':[{'date':'2017-05-12T21:05:28.130Z','action':'confirmed'},{'date':'2017-05-12T21:05:28.130Z','action':'created'}],'entries':[{'address':'mi4NNLqqPVwQZGc6ZX4S3FG1fL3AnVCaug','value':-369600},{'address':'2MxYEMY8UKFPj8Ps5BXSHj3FLA2QfD9n7K1','wallet':'59161139c86cffa0074b614d07dfc29b','value':100000},{'address':'mxjBshD14gvw1JkKmBLyEJbz5bigNE357y','value':259600}],'outputs':[{'id':'761774a49a1d9d820572f326dc4b99bfd26dcd6661574660995decc612718e0b:0','address':'2MxYEMY8UKFPj8Ps5BXSHj3FLA2QfD9n7K1','value':100000,'valueString':'100000','wallet':'59161139c86cffa0074b614d07dfc29b','chain':0,'index':0},{'id':'761774a49a1d9d820572f326dc4b99bfd26dcd6661574660995decc612718e0b:1','address':'mxjBshD14gvw1JkKmBLyEJbz5bigNE357y','value':259600,'valueString':'259600','wallet':'59161139c86cffa0074b614d07dfc29b','chain':1,'index':147}],'inputs':[{'id':'778af3e4a2f0cfcebc28502ae5ee5d2edbec894eb07d967d6bb873720d33287a:1','address':'mi4NNLqqPVwQZGc6ZX4S3FG1fL3AnVCaug','value':369600,'valueString':'369600','wallet':'59161139c86cffa0074b614d07dfc29b','chain':1,'index':114}],'confirmedTime':'2017-05-12T21:05:28.130Z','createdTime':'2017-05-12T21:05:28.130Z'}],'count':5}";
                ListWalletTransfersRootObject Response = new ListWalletTransfersRootObject();
                Response = JsonConvert.DeserializeObject<ListWalletTransfersRootObject>(requeststring);
                Response.ReturnCode = enResponseCode.Success;
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                //return Ok(respObjJson);
                return returnDynamicResult(respObjJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }


        [HttpGet("{coin}/{walletId}/{id}")]
        public ActionResult GetwalletTransfer(string coin, string walletId, string id)
        {
            try
            {
                string requeststring = "{'coin':'tbtc','transfers':[{'id':'591623989c043ab2079857ee53d812f0','coin':'tbtc','wallet':'59161139c86cffa0074b614d07dfc29b','txid':'cc21875eb303e5efda9056585d68c79b10345585213a62c9c1a7bc331dfd5d93','normalizedTxHash':'e0e4bada8332ed254c20c4c1d2c25e5f13386509e7ac21d005a169925d07889a','height':1122442,'date':'2017-05-12T21:05:28.130Z','confirmations':1,'type':'receive','value':100000,'bitgoFee':0,'usd':1.79092,'usdRate':1790.92,'state':'confirmed','vSize':224,'nSegwitInputs':0,'tags':['591623989c043ab2079857ee'],'sequenceId':'','history':[{'date':'2017-05-12T21:05:28.130Z','action':'confirmed'},{'date':'2017-05-12T21:05:28.130Z','action':'created'}],'entries':[{'address':'mi4NNLqqPVwQZGc6ZX4S3FG1fL3AnVCaug','value':-369600},{'address':'2MxYEMY8UKFPj8Ps5BXSHj3FLA2QfD9n7K1','wallet':'59161139c86cffa0074b614d07dfc29b','value':100000},{'address':'mxjBshD14gvw1JkKmBLyEJbz5bigNE357y','value':259600}],'outputs':[{'id':'761774a49a1d9d820572f326dc4b99bfd26dcd6661574660995decc612718e0b:0','address':'2MxYEMY8UKFPj8Ps5BXSHj3FLA2QfD9n7K1','value':100000,'valueString':'100000','wallet':'59161139c86cffa0074b614d07dfc29b','chain':0,'index':0},{'id':'761774a49a1d9d820572f326dc4b99bfd26dcd6661574660995decc612718e0b:1','address':'mxjBshD14gvw1JkKmBLyEJbz5bigNE357y','value':259600,'valueString':'259600','wallet':'59161139c86cffa0074b614d07dfc29b','chain':1,'index':147}],'inputs':[{'id':'778af3e4a2f0cfcebc28502ae5ee5d2edbec894eb07d967d6bb873720d33287a:1','address':'mi4NNLqqPVwQZGc6ZX4S3FG1fL3AnVCaug','value':369600,'valueString':'369600','wallet':'59161139c86cffa0074b614d07dfc29b','chain':1,'index':114}],'confirmedTime':'2017-05-12T21:05:28.130Z','createdTime':'2017-05-12T21:05:28.130Z'}],'count':5}";
                GetWalletTransferRootObject Response = new GetWalletTransferRootObject();
                Response = JsonConvert.DeserializeObject<GetWalletTransferRootObject>(requeststring);
                Response.ReturnCode = enResponseCode.Success;
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                //return Ok(respObjJson);
                return returnDynamicResult(respObjJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }


        [HttpGet("{coin}/{walletId}/{sequenceId}")]
        public ActionResult GetWalletTransBySeq(string coin, string walletId, string sequenceId)
        {
            try
            {
                string requeststring = "{'id':'591623989c043ab2079857ee53d812f0','coin':'tbtc','wallet':'59161139c86cffa0074b614d07dfc29b','txid':'cc21875eb303e5efda9056585d68c79b10345585213a62c9c1a7bc331dfd5d93','height':1122442,'date':'2017-05-12T21:05:28.130Z','confirmations':1,'type':'receive','value':100000,'bitgoFee':0,'usd':1.79092,'usdRate':1790.92,'state':'confirmed','tags':['591623989c043ab2079857ee'],'sequenceId':'hello123','history':[{'date':'2017-05-12T21:05:28.130Z','action':'confirmed'},{'date':'2017-05-12T21:05:28.130Z','action':'created'}],'entries':[{'address':'mi4NNLqqPVwQZGc6ZX4S3FG1fL3AnVCaug','value':-369600},{'address':'2MxYEMY8UKFPj8Ps5BXSHj3FLA2QfD9n7K1','wallet':'59161139c86cffa0074b614d07dfc29b','value':100000},{'address':'mxjBshD14gvw1JkKmBLyEJbz5bigNE357y','value':259600}],'outputs':[{'id':'761774a49a1d9d820572f326dc4b99bfd26dcd6661574660995decc612718e0b:0','address':'2MxYEMY8UKFPj8Ps5BXSHj3FLA2QfD9n7K1','value':100000,'valueString':'100000','wallet':'59161139c86cffa0074b614d07dfc29b','chain':0,'index':0},{'id':'761774a49a1d9d820572f326dc4b99bfd26dcd6661574660995decc612718e0b:1','address':'mxjBshD14gvw1JkKmBLyEJbz5bigNE357y','value':259600,'valueString':'259600','wallet':'59161139c86cffa0074b614d07dfc29b','chain':1,'index':147}],'inputs':[{'id':'778af3e4a2f0cfcebc28502ae5ee5d2edbec894eb07d967d6bb873720d33287a:1','address':'mi4NNLqqPVwQZGc6ZX4S3FG1fL3AnVCaug','value':369600,'valueString':'369600','wallet':'59161139c86cffa0074b614d07dfc29b','chain':1,'index':114}],'confirmedTime':'2017-05-12T21:05:28.130Z','createdTime':'2017-05-12T21:05:28.130Z'}";
                GetWalletTransBySeqRootObject Response = new GetWalletTransBySeqRootObject();
                Response = JsonConvert.DeserializeObject<GetWalletTransBySeqRootObject>(requeststring);
                Response.ReturnCode = enResponseCode.Success;
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                //return Ok(respObjJson);
                return returnDynamicResult(respObjJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }


        [HttpPost("{coin}/{id}/{wallet}")]
        public ActionResult CreateWalletAddress(string coin, string wallet, string id, [FromBody]CreateWalletAddressReq Request)
        {
            try
            {
                string requeststring = "{'address':'2Mz7x1a5df8380e0e30yYc6e','coin':'tbtc','label':'My address','wallet':'585c51a5df8380e0e3082e46','coinSpecific':{'chain':0,'index':1,'redeemScript':'522101a5df8380e0e30453ae'}}";
                CreateWalletAddressRootObject Response = new CreateWalletAddressRootObject();
                Response = JsonConvert.DeserializeObject<CreateWalletAddressRootObject>(requeststring);
                Response.ReturnCode = enResponseCode.Success;
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                //return Ok(respObjJson);
                return returnDynamicResult(respObjJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }


        [HttpGet("{coin}/{walletId}/{addressOrId}")]
        public ActionResult GetWalletAddress(string coin, string walletId, string addressOrId)
        {
            try
            {
                string requeststring = "{'address':'2Mz7x1a5df8380e0e30yYc6e','coin':'tbtc','label':'My address','wallet':'585c51a5df8380e0e3082e46','coinSpecific':{'chain':0,'index':1,'redeemScript':'522101a5df8380e0e30453ae'}}";
                GetWalletAddressRootObject Response = new GetWalletAddressRootObject();
                Response = JsonConvert.DeserializeObject<GetWalletAddressRootObject>(requeststring);
                Response.ReturnCode = enResponseCode.Success;
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                //return Ok(respObjJson);
                return returnDynamicResult(respObjJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }


        [HttpPut("{coin}/{walletId}/{addressOrId}")]
        public ActionResult UpdateWalletAddress(string coin, string walletId, string addressOrId, [FromBody]UpdateWalletAddressReq Request)
        {
            try
            {
                string requeststring = "{'address':'2NCzBK2Yf7PFAAfKsgc6cfTSG8FxtgMGG9C','chain':0,'index':0,'coin':'tbtc','label':'My address','wallet':'5935dfef695bc5d30732ba88342405dd','coinSpecific':{'redeemScript':'52210…953ae'}}";
                UpdateWalletAddressRootObject Response = new UpdateWalletAddressRootObject();
                Response = JsonConvert.DeserializeObject<UpdateWalletAddressRootObject>(requeststring);
                Response.ReturnCode = enResponseCode.Success;
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                //return Ok(respObjJson);
                return returnDynamicResult(respObjJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }


        [HttpPost("{coin}/{walletid}")]
        public ActionResult Withdrawal(string coin, string walletid, [FromBody]WithdrawalReq Request)
        {
            try
            {
                string requeststring = "{'address':'2NCzBK2Yf7PFAAfKsgc6cfTSG8FxtgMGG9C','chain':0,'index':0,'coin':'tbtc','label':'My address','wallet':'5935dfef695bc5d30732ba88342405dd','coinSpecific':{'redeemScript':'52210…953ae'}}";
                WithdrawalRootObject Response = new WithdrawalRootObject();
                Response = JsonConvert.DeserializeObject<WithdrawalRootObject>(requeststring);
                Response.ReturnCode = enResponseCode.Success;
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                //return Ok(respObjJson);
                //string con = coin + "\t" + walletid + "\t";
                //return Ok(con);
                return returnDynamicResult(respObjJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }


        [HttpPost("{coin}/{walletid}")]
        public ActionResult SendToMany(string coin, string walletid, [FromBody]SendTranToManyReq Request)
        {
            try
            {
                string requeststring = "{'txid':'a4b50c3f7fb5dd9273f5be69661b79eed61570421f76ec903ad914d39980549e','status':'signed'}";
                SendTranToManyRootObject Response = new SendTranToManyRootObject();
                Response = JsonConvert.DeserializeObject<SendTranToManyRootObject>(requeststring);
                Response.ReturnCode = enResponseCode.Success;
                var respObj = JsonConvert.SerializeObject(Response);
                dynamic respObjJson = JObject.Parse(respObj);
                //return Ok(respObjJson);
                return returnDynamicResult(respObjJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Date: " + _basePage.UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nControllername=" + this.GetType().Name, LogLevel.Error);
                return BadRequest();
            }
        }
    }
}

