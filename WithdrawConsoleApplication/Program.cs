using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Configuration;
using System.Timers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Text;
using DepositStatusCheckApp;
using Microsoft.Extensions.Configuration;
using WithdrawConsoleApplication;

namespace DepositConsoleApplication
{
    public class Program
    {
        static DataCommon dComm = new DataCommon();
        static CommonFunctions logs = new CommonFunctions();
        public static IConfiguration Configuration { get; set; }

        static System.Timers.Timer TopupTick = new System.Timers.Timer();
        static System.Timers.Timer TransactionTick = new System.Timers.Timer();
        //string Url = ConfigurationManager.AppSettings["CallURL"];
        static bool IsProcessing = false;
        [STAThread]
        public static void Main(string[] args)
        {
            Console.Write("start");
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json");

            //Configuration = builder.Build();


            //Console.WriteLine("Hello World!");
            // TransactionTick.Interval = Convert.ToInt32(Configuration["Interval"]);
             TransactionTick.Interval =3000;
            TransactionTick.Elapsed += new ElapsedEventHandler(transaction_tick);
            TransactionTick.Start();

            Console.WriteLine("Press \'q\' to quit");
            while (Console.Read() != 'q') ;

        }


        #region TimerTick
        private static void transaction_tick(object sender, System.EventArgs e)
        {
            try
            {
                TransactionTick.Stop();
                TransactionTick.Interval = 1000;//1800000; // For Testing 1000
               // logs.WriteRequestLog("Generate Response :  Transaction Detail not found", "CallThirdPartyAPI", "test");
                 CallAPI();
                //TransactionTick.Interval = 1800000;
            }
            catch (Exception ex)
            {
                logs.WriteErrorLog(ex, "transaction_tick", ex.Source.ToString());
                ex = null;
            }
            finally
            {
                TransactionTick.Start();
            }
        }
        #endregion

        #region CallBackProcess

        private static void CallAPI()
        {
            string SqlStr = string.Empty;
            DataSet dSet = new DataSet();
            try
            {
                //Rushabh 27-06-2018 for RouteTag Wise MasterFile Reading
                //SqlStr = "SELECT w.ServiceID , s.SMSCode, w.AppType , w.Status AS WalletStatus , s.Status AS ServiceStatus FROM WalletMaster w INNER JOIN ServiceMaster s ON s.Serviceid = w.ServiceId WHERE w.IsDepositAPI = 1";
                //SqlStr = "SELECT w.ServiceID,s.SMSCode,w.AppType,w.Status AS WalletStatus ,s.Status AS ServiceStatus,trt.RouteTag,trt.CovertAmount FROM WalletMaster w INNER JOIN ServiceMaster s ON s.Serviceid = w.ServiceId inner join TradeRouteTrnMaster trt on trt.Wallet = w.WalletName and trt.TrnType =" + (int)EnTrnType.Withdraw + " WHERE w.IsDepositAPI = 1 and w.status=1 and trt.status=1";
                //Rushabh 09-07-2018 fetch extra SerProID for TradeWithdrawHistoryInsertion condition checking
                SqlStr = "select rt.ConvertAmount,rt.ServiceID,sp.ProviderName as 'RouteTag',sm.SMSCode,rt.Status,spd.ServiceProID,a.Id as 'AppType' from RouteConfiguration rt inner join ServiceMaster sm on sm.Id=rt.ServiceID inner join ServiceProviderDetail spd on spd.Id=rt.SerProDetailID inner join AppType a on a.Id=spd.AppTypeID inner join ServiceProviderMaster sp on sp.Id=spd.ServiceProID where rt.TrnType=6 and rt.status=1";
                dSet = (new DataCommon()).OpenDataSet("RouteConfiguration", SqlStr, dSet, 30);
                if (dSet.Tables[0].Rows.Count > 0)
                {
                    StringBuilder output = new StringBuilder();
                    foreach (DataRow dRow in dSet.Tables[0].Rows)
                    {

                        // Need common row object for each SMScode
                        walletServiceData walletServiceDataObj = new walletServiceData();
                        walletServiceDataObj.ServiceID = Convert.ToInt32(dRow["ServiceID"]);
                        walletServiceDataObj.SerProID = Convert.ToInt64(dRow["ServiceProID"]);//Rushabh 09-07-2018 for Checking condition in TradeWithdrawHistoryInsertion 
                        walletServiceDataObj.SMSCode = dRow["SMSCode"].ToString();
                        walletServiceDataObj.WallletStatus = Convert.ToInt16(dRow["Status"]);
                        //walletServiceDataObj.ServiceStatus = Convert.ToInt16(dRow["ServiceStatus"]);
                        walletServiceDataObj.AppType = Convert.ToInt16(dRow["AppType"]); // 2 for Local Coin
                        walletServiceDataObj.RouteTag = dRow["RouteTag"].ToString();
                        walletServiceDataObj.CAmount = Convert.ToDecimal(dRow["ConvertAmount"]);//Rushabh 04-07-2018 for passing value to CallAPISingle
                        lock (walletServiceDataObj)
                        {
                            if (walletServiceDataObj.SMSCode == "BTC") // for testing
                            {
                                WaitCallback callBack;
                                callBack = new WaitCallback(CallAPISingle); // create thread for each SMSCode
                                ThreadPool.QueueUserWorkItem(callBack, walletServiceDataObj);
                                Console.WriteLine(walletServiceDataObj.SMSCode);
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ex = null;
                logs.WriteErrorLog(ex, "Program", "CallAPI");
            }
            finally
            {
                TransactionTick.Start(); //ntrivedi temperory
            }
        }

        private static void CallAPISingle(object RefObj)
        {
            try
            {
                walletServiceData walletServiceDataObj = (walletServiceData)RefObj;
                CommonMethods CommonMethod = new CommonMethods();
                //RespTransfers resp = new RespTransfers();
                CommonMethod.Transfers = new List<RespTransfers>(); // Need object of RespTransfers  for Thirdparty ApI response
                //CommonMethod.RespLocalCoins = new List<RespLocalCoin>(); // Need object of RespLocalCoins  for Thirdparty ApI response
                CommonMethod.SMSCode = walletServiceDataObj.SMSCode;
                CommonMethod.SerProID = walletServiceDataObj.SerProID; // Rushabh 09-07-2018 for Checking condition in TradeWithdrawHistoryInsertion 
                CommonMethod.CovertAmount = walletServiceDataObj.CAmount;
                CommonMethod.RouteTag = walletServiceDataObj.RouteTag;
                string path = walletServiceDataObj.SMSCode + "_" + walletServiceDataObj.RouteTag;//Rushabh(27-06-2018) for RouteTag Base MasterFile Reading
                ReadMasterFile(path, ref CommonMethod); // Read  Master File RouteTag Base
                if (!string.IsNullOrEmpty(CommonMethod.Path_AddressGenerate))
                {
                    ReadTransactionalFile(CommonMethod.Path_AddressGenerate, ref CommonMethod); // Read Transaction file for specific coin               
                    if (!string.IsNullOrEmpty(CommonMethod.Str_URL) && !string.IsNullOrEmpty(CommonMethod.Str_RequestType) && !string.IsNullOrEmpty(CommonMethod.ContentType))
                    {
                        logs.WriteRequestLog("Proceed for ", "CallAPISingle " + walletServiceDataObj.RouteTag, CommonMethod.SMSCode);
                        //Rushabh 27-06-2018 for RouteTag Base MasterFile Reading
                        if (walletServiceDataObj.RouteTag.Equals("BITGO"))
                        {
                            //vsolanki
                            DataSet dSet = new DataSet();
                           GetHistory(ref CommonMethod, walletServiceDataObj, CommonMethod.TrnId);
                            //TradeDepositHistoryUpdationForBitgo(ref CommonMethod);
                            //CallThirdPartyAPI(ref CommonMethod, CommonMethod.TrnId, CommonMethod.Authorization, CommonMethod.enterprise, dSet); // Generate ThirdParty API Response 
                            if (!string.IsNullOrEmpty(CommonMethod.DepositHistoryResponse))
                            {
                                ParseBitGoAPIResponse(ref CommonMethod);//Parse Response
                                TradeWithdrawHistoryInsertion(ref CommonMethod); // Insert Trade Deposit History
                            }
                            else
                            {
                                logs.WriteRequestLog("Response Null", "CallAPISingle " + walletServiceDataObj.RouteTag, CommonMethod.SMSCode);
                            }
                        }
                        else if (walletServiceDataObj.RouteTag.Equals("CRYPTO") && (!IsProcessing))
                        {
                            if (!string.IsNullOrEmpty(CommonMethod.UserName) && !string.IsNullOrEmpty(CommonMethod.Password) && !string.IsNullOrEmpty(CommonMethod.RequestBody))
                            {
                                GetHistory(ref CommonMethod,walletServiceDataObj, CommonMethod.TrnId); // Get History From Deposit History SMScode Wise
                               // TradeDepositHistoryUpdationForCryptoCoin(ref CommonMethod); // Update Crypto coin into Trade Deposit History
                            }
                            else
                            {
                                logs.WriteRequestLog("Transaction Detail not found", "CallAPISingle " + walletServiceDataObj.RouteTag, CommonMethod.SMSCode);
                            }
                        }
                        else
                        {
                            logs.WriteRequestLog("Route Detail not found", "CallAPISingle " + walletServiceDataObj.RouteTag, CommonMethod.SMSCode);
                        }
                        #region Old Code Comments
                        // Commented By Rushabh-28/06/2018
                        //switch (walletServiceDataObj.AppType)
                        //{
                        //    case (int)EnAppType.BitGoAPI:
                        //        CallThirdPartyAPI(ref CommonMethod); // Generate ThirdParty API Response
                        //        TradeDepositHistoryInsertion(ref CommonMethod); // Insert Trade Deposit History
                        //        break;
                        //    case (int)EnAppType.CryptoAPI:
                        //        if (!string.IsNullOrEmpty(CommonMethod.UserName) && !string.IsNullOrEmpty(CommonMethod.Password) && !string.IsNullOrEmpty(CommonMethod.RequestBody))
                        //        {
                        //            GetHistory(ref CommonMethod); // Get History From Deposit History SMScode Wise
                        //            TradeDepositHistoryUpdationForCryptoCoin(ref CommonMethod); // Update Crypto coin into Trade Deposit History
                        //        }
                        //        else
                        //        {
                        //            logs.WriteRequestLog("Transaction Detail not found", "CallAPISingle", CommonMethod.SMSCode);
                        //        }
                        //        break;
                        //    default:
                        //        break;
                        //}
                        #endregion 
                    }
                    else
                    {
                        logs.WriteRequestLog("Transaction Detail not found", "CallAPISingle " + walletServiceDataObj.RouteTag, CommonMethod.SMSCode);
                    }
                }
                else
                {
                    logs.WriteRequestLog("Master File Detail not found", "CallAPISingle " + walletServiceDataObj.RouteTag, CommonMethod.SMSCode);
                }
            }
            catch (Exception ex)
            {
                //ex = null;
                logs.WriteErrorLog(ex, "Program", "CallAPISingle");
            }
        }

        private static int CheckStatus(int confirmation)//Rushabh 06-07-2018
        {
            int ActionType = 0;
            //CommonMethods CommonMethod = new CommonMethods();
            if (confirmation >= 3)
            {
                ActionType = 4;
            }
            return ActionType;
        }

        private static void GetHistory(ref CommonMethods CommonMethod ,walletServiceData walletServiceDataObj, string trnID)
        {
            DataSet dSet = new DataSet();
            DataRow dRows = null;
            string SqlStr = string.Empty;
            string SourceAddress = string.Empty;
            try
            {
                //if (IsProcessing)
                //    return;
                //SqlStr = "SELECT AutoNo, TrnID , Address, SMSCode,Value, Amount, Confirmations,OrderID From WithdrawHistory WHERE SMSCode = '" + CommonMethod.SMSCode + "' AND Status = 0 AND IsProcessing = 0";
                //dSet = (new DataCommon()).OpenDataSet("WithdrawHistory", SqlStr, dSet, 30);
                //SqlStr = "SELECT TrnNo FROM TransactionQueue WHERE SMSCode = '" + CommonMethod.SMSCode + "' AND Status = 4 AND TrnType=" + (int)EnTrnType.Withdraw +" AND SerProID="+CommonMethod.SerProID;
                SqlStr = "select top 5 TrnNo,Charge,TrnID,ToAddress,Address,Amount from WithdrawHistory where status=6 AND IsProcessing = 0  order by UpdatedDate";
                dSet = (new DataCommon()).OpenDataSet("WithdrawHistory", SqlStr, dSet, 30);
                if (dSet.Tables[0].Rows.Count > 0)
                {
                    IsProcessing = true;
                    foreach (DataRow dRow in dSet.Tables[0].Rows)
                    {
                        //SqlStr = "SELECT * FROM WithdrawHistory WHERE SMSCode='" + CommonMethod.SMSCode + "' AND TrnID = '" + dRow["TrnID"].ToString() + "' AND Address='" + dRow["Address"].ToString() + "' AND SerProID=" + CommonMethod.SerProID;
                        //dSet = (new DataCommon()).OpenDataSet("WithdrawHistory", SqlStr, dSet, 30);
                        if (dSet.Tables[0].Rows.Count > 0)
                        {
                            SqlStr = "UPDATE WithdrawHistory SET IsProcessing = 1,UpdatedDate=dbo.getistdate() WHERE TrnID = '" + dRow["TrnID"].ToString() +"'";
                            (new DataCommon()).ExecuteQuery(SqlStr);
                        }
                        //else
                        //{
                        //    //SqlStr = "INSERT INTO WithdrawHistory(TrnID,SMSCode,Wallet,Address,Status,Confirmations,Value,confirmedTime,unconfirmedTime,createdTime,State,CreatedDateTime,TrnNo,Amount,SystemRemarks,ToAddress,Charge,RouteTag,SerProID) " +
                        //    //   " VALUES('" + item.id + "','" + CommonMethod.SMSCode.ToUpper() + "','" + item.WalletID + "','" + item.address + "'," + Constatus + "," + item.confirmations + "," + item.value + ",'" + item.confirmedTime + "','" + item.unconfirmedTime + "','" + item.createdTime + "','" + item.state + "', dbo.GetISTDate()," + item.TrnNo + "," + item.Amount + ",'Inserted','" + item.address + "'," + item.Fee + ",'" + CommonMethod.RouteTag + "'," + CommonMethod.SerProID + ")";
                        //    //(new DataCommon()).ExecuteQuery(CommonMethod.SqlStr);
                        //    SqlStr = "INSERT INTO WithdrawHistory(TrnID,SMSCode,Wallet,Address,Status,Confirmations,Value,confirmedTime,unconfirmedTime,createdTime,State,CreatedDateTime,TrnNo,Amount,SystemRemarks,ToAddress,Charge,RouteTag,SerProID,IsProcessing) " +
                        //            " VALUES('" + dRow["TrnID"].ToString() + "','" + CommonMethod.SMSCode.ToUpper()
                        //            + "','" + CommonMethod.orgwallet + "','" + dRow["Address"].ToString()
                        //            + "',4,0," + Convert.ToDecimal(dRow["Amount"].ToString()) + ",null,null,dbo.GetISTDate(),null,dbo.GetISTDate(),"
                        //            + Convert.ToInt64(dRow["TrnNo"].ToString()) + "," + Convert.ToDecimal(dRow["Amount"].ToString())
                        //            + ",'Inserted','" + dRow["address"].ToString() + "'," + Convert.ToDecimal(dRow["Charge"].ToString())
                        //            + ",'" + CommonMethod.RouteTag + "'," + CommonMethod.SerProID + ",1)";
                        //    (new DataCommon()).ExecuteQuery(SqlStr);
                        //}
                    }
                    //IsProcessing = false;
                    foreach (DataRow dRow in dSet.Tables[0].Rows)
                    {
                        //SqlStr = "Select Top 1 AccountNo From MemberBankMaster Where IsDefaultAddress=1 AND Type=1 AND Status=1 AND  MemberID=1 AND BranchName = '" + CommonMethod.SMSCode.ToUpper() + "' AND SerProID=" + dRow["SerProID"].ToString() + "";
                        //SqlStr = "Select Top 1 AccountNo From MemberBankMaster Where IsDefaultAddress=1 AND Type=1 AND Status=1 AND  MemberID=1 AND BranchName = '" + CommonMethod.SMSCode.ToUpper() + "' AND SerProID=" + CommonMethod.SerProID;
                        //SourceAddress = (new DataCommon()).ExecuteScalarWDM(SqlStr);
                        //String Response = CallThirdPartyCryptoAPI(ref CommonMethod, dRow["Address"].ToString(), dRow["TrnID"].ToString(), dRow["TrnNo"].ToString(), SourceAddress, Convert.ToDecimal(dRow["Amount"]));
                        string Response= CallThirdPartyAPI(ref CommonMethod, CommonMethod.TrnId, CommonMethod.Authorization, CommonMethod.enterprise, dSet);
                        BitGoRes TPGenerateResponse = new BitGoRes();
                        RespTransfers transferObj = new RespTransfers();
                        TPGenerateResponse = JsonConvert.DeserializeObject<BitGoRes>(Response);
                        //JObject GenerateResponse = JObject.Parse(Response);
                        if (TPGenerateResponse != null)
                        {
                            //if (TPGenerateResponse.coin.ToLower() == CommonMethod.SMSCode.ToLower() && TPGenerateResponse.type == "receive") // For BTC , BTG , BCH Response 
                            //{
                            if (TPGenerateResponse.entries != null)
                            {
                                string trnid = dRow["TrnID"].ToString();
                                //trnid = dSet.Tables[0].Rows[0]["Tr"].ToString();

                                if (TPGenerateResponse.id == (dRow["TrnID"]).ToString())
                                {
                                    if (TPGenerateResponse.entries != null)
                                    {
                                        foreach (var iv in TPGenerateResponse.entries)
                                        {
                                            if (!string.IsNullOrEmpty(iv.address) && iv.value > 0)
                                            {
                                                if (iv.value <= 0)
                                                {
                                                    SqlStr = "UPDATE WithdrawHistory SET IsProcessing = 0,Status=9 WHERE  TrnID = '" + dRow["TrnID"].ToString() + "'";
                                                    (new DataCommon()).ExecuteQuery(SqlStr);
                                                    continue;
                                                }
                                                if (iv.address.ToString().Equals(dSet.Tables[0].Rows[0]["Address"].ToString()))
                                                {
                                                    // iv["address"] = iv["address"].ToString();
                                                    //iv["Amount"] = Convert.ToDecimal(iv["value"]) / Convert.ToDecimal(Configuration[Convert.ToString(iv["btc"])]);
                                                    //decimal amount = Math.Round((iv.value / CommonMethod.CovertAmount), 8);
                                                    transferObj.id = dRow["TrnID"].ToString();
                                                    transferObj.Amount = CommonMethod.CovertAmount; //Convert.ToDecimal(amount);
                                                    transferObj.coin = CommonMethod.SMSCode;
                                                    transferObj.txid = TPGenerateResponse.id;
                                                    // transferObj.Value = Convert.ToDecimal(dRow["Value"]);
                                                    // transferObj. = Convert.ToInt64(dRow["OrderID"]);
                                                    transferObj.address = dRow["Address"].ToString();
                                                    // transferObj. = dRow["FromAddress"].ToString();
                                                    transferObj.confirmations = TPGenerateResponse.confirmations;
                                                    if (transferObj.confirmations < 0) //ntrivedi treat as failed 26-07-2018
                                                    {
                                                        logs.WriteRequestLog("confirmations < 0 :  " + transferObj.txid, "GetHistory", CommonMethod.SMSCode);
                                                        SqlStr = "UPDATE WithdrawHistory SET IsProcessing = 0,Status=9, WHERE  TrnID = '" + dRow["TrnID"].ToString() + "'";
                                                        (new DataCommon()).ExecuteQuery(SqlStr);
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        logs.WriteRequestLog("confirmations  :  " + transferObj.txid, "GetHistory", CommonMethod.SMSCode);
                                                        if(transferObj.confirmations>=3)
                                                        {
                                                            SqlStr = "UPDATE WithdrawHistory SET confirmations = " + TPGenerateResponse.confirmations + " , Status=1,SystemRemarks='Success' WHERE  TrnID = '" + dRow["TrnID"].ToString() + "'";
                                                        }
                                                        else
                                                        {
                                                            SqlStr = "UPDATE WithdrawHistory SET confirmations = " + TPGenerateResponse.confirmations + " , SystemRemarks='Success' WHERE  TrnID = '" + dRow["TrnID"].ToString() + "'";
                                                        }
                                                      
                                                        (new DataCommon()).ExecuteQuery(SqlStr);
                                                        //continue;
                                                    }
                                                    //System.DateTime dateTime1 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                                                    //transferObj.valueStr = TPGenerateResponse.valueString;
                                                    //transferObj.unconfirmedTime = TPGenerateResponse.unconfirmedTime.ToString();
                                                    //transferObj.confirmedTime = TPGenerateResponse.confirmedTime.ToString();
                                                    CommonMethod.Transfers.Add(transferObj);
                                                }
                                                else
                                                {
                                                    SqlStr = "UPDATE WithdrawHistory SET IsProcessing = 1,Status=9 WHERE Address='" + iv.address.ToString() + "' and  TrnID = '" + dRow["TrnID"].ToString() + "'";
                                                    (new DataCommon()).ExecuteQuery(SqlStr);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                SqlStr = "UPDATE WithdrawHistory SET IsProcessing = 1,Status=9  WHERE TrnID = '" + dRow["TrnID"].ToString() + "'";
                                (new DataCommon()).ExecuteQuery(SqlStr);
                            }
                        
                        //}

                        //else
                        //{
                        //    SqlStr = "UPDATE DepositHistory SET IsProcessing = 1,Status=9,StatusMsg='error not null Thirpdarty response' WHERE ID = " + dRow["ID"].ToString() + "";
                        //    (new DataCommon()).ExecuteQuery(SqlStr);
                        //}
                            //logs.WriteRequestLog("Loop done :" + dRow["TrnID"].ToString() + " IsProcessing :" + IsProcessing.ToString() + "", "GetHistory", CommonMethod.SMSCode, Convert.ToInt16(Configuration["AllowLog"]), Action: 2);
                        }
                        else
                        {
                            SqlStr = "UPDATE WithdrawHistory SET IsProcessing = 0 WHERE TrnID = '" + dRow["TrnID"].ToString() + "' AND Address='" + dRow["Address"].ToString() + "' AND SerProID=" + CommonMethod.SerProID;
                            (new DataCommon()).ExecuteQuery(SqlStr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logs.WriteErrorLog(ex, "Program", "GetHistory");
            }
        }

        private static void TradeDepositHistoryUpdationForCryptoCoin(ref CommonMethods CommonMethod)
        {
            DataSet dSet = new DataSet();
            string SqlStr = string.Empty;
            Int32 actionType;
            try
            {
                if (CommonMethod.Transfers.Count > 0)
                {
                    foreach (var item in CommonMethod.Transfers)
                    {
                        #region OLD Code
                        // update
                        //CommonMethod.SqlStr = "UPDATE WithdrawHistory SET Confirmations =" + item.confirmations + ", confirmedTime ='" + item.confirmedTime + "', unconfirmedTime  ='" + item.unconfirmedTime + "', UpdatedDateTime = dbo.GetISTDate() WHERE TrnID = '" + item.txid + "' AND Address='" + item.address + "'";
                        //(new DataCommon()).ExecuteQuery(CommonMethod.SqlStr);
                        //logs.WriteRequestLog("Update Deposit History :  " + item.txid, "TradeDepositHistoryInsertion", CommonMethod.SMSCode);

                        //if ((new DataCommon()).ExecuteScalarWDM("Select Count(TrnID) From TradeDepositCompletedTrn Where TrnID='" + item.txid + "' AND Address='" + item.address + "'") == "0")
                        //{
                        //    if (item.TrnNo == 0)
                        //    {
                        // create Order
                        // CreateOrder(ref CommonMethod, item);
                        //}

                        // Delivery Process Order
                        //if (item.confirmations >= 3)
                        //{
                        //DeliveryProcessOrder(ref CommonMethod, item, 1);
                        //    }
                        //}
                        //else // Order Already Intialized
                        //{
                        //    SqlStr = "UPDATE WithdrawHistory SET status = 9,SystemRemarks='Order already proceed Crypto' WHERE TrnID = '" + item.txid + "' AND Address='" + item.address + "'";
                        //    (new DataCommon()).ExecuteQuery(SqlStr);
                        //    logs.WriteRequestLog("Order Already Intialized | " + item.txid, "TradeDepositHistoryUpdationForCryptoCoin", CommonMethod.SMSCode);
                        //}

                        //SqlStr = "UPDATE WithdrawHistory SET IsProcessing = 0 WHERE TrnID = '" + item.txid + "' AND Address='" + item.address + "'";
                        //(new DataCommon()).ExecuteQuery(SqlStr);
                        #endregion
                        actionType = CheckStatus(item.confirmations);
                        if (item.confirmations >= 3)
                        {
                            // update
                            CommonMethod.SqlStr = "UPDATE WithdrawHistory SET Confirmations =" + item.confirmations + ", confirmedTime ='" + item.confirmedTime + "', unconfirmedTime  ='" + item.unconfirmedTime + "', UpdatedDateTime = dbo.GetISTDate(),status=1 WHERE TrnID = '" + item.txid + "' AND Address='" + item.address + "' AND SerProID=" + CommonMethod.SerProID;
                            (new DataCommon()).ExecuteQuery(CommonMethod.SqlStr);
                            logs.WriteRequestLog("Update Withdraw History :  " + item.txid, "TradeDepositHistoryInsertion", CommonMethod.SMSCode);

                            if (actionType == 4)
                            {
                                ReconAction(item.TrnNo, actionType, CommonMethod.SMSCode);
                            }
                        }
                        //Rita 14-7-18 common update , release this record from processing
                        SqlStr = "UPDATE WithdrawHistory SET IsProcessing = 0 WHERE TrnID = '" + item.txid + "' AND Address='" + item.address + "' AND SerProID=" + CommonMethod.SerProID;
                        (new DataCommon()).ExecuteQuery(SqlStr);
                    }
                }
                IsProcessing = false;
            }
            catch (Exception ex)
            {
                logs.WriteErrorLog(ex, "Program", "TradeDepositHistoryUpdationForCryptoCoin");
            }
        }

        //private static void CreateOrder(ref CommonMethods CommonMethod, RespTransfers item)
        //{
        //    long MemberID;
        //    long WalletID;
        //    int RetCode = 0;
        //    string RetMsg = string.Empty;
        //    long OrderID = 0;
        //    int BankID = 0;
        //    string BranchName = string.Empty;
        //    DataSet dSet = new DataSet();
        //    DataRow dRows = null;
        //    string SqlStr = string.Empty;
        //    try
        //    {
        //        SqlStr = "SELECT m.MemberID, w.WalletID ,m.BranchName FROM MemberBankMaster AS m INNER JOIN Wallet As w ON m.MemberID = w.MemberId AND m.BankID=w.Serviceid WHERE m.Type=1 AND m.MemberID<>1 AND AccountNo = '" + item.address + "' AND m.BranchName='" + item.coin.ToUpper() + "'";
        //        dSet = (new DataCommon()).OpenDataSet("MemberBankMaster", SqlStr, dSet, 30);
        //        if (dSet.Tables[0].Rows.Count > 0)
        //        {
        //            dRows = dSet.Tables[0].Rows[0];
        //            MemberID = Convert.ToInt64(dRows["MemberID"]);
        //            WalletID = Convert.ToInt64(dRows["WalletID"]);
        //            //BankID = Convert.ToInt16(dRows["BankID"]);
        //            BranchName = Convert.ToString(dRows["BranchName"]);
        //            if (MemberID != 0 && WalletID != 0 && !string.IsNullOrEmpty(BranchName))
        //            {
        //                SqlParameter[] Params = new SqlParameter[] {
        //                    new SqlParameter("@OrderID", SqlDbType.BigInt, 10, ParameterDirection.Output, false, 0, 0, String.Empty, DataRowVersion.Default, 0),
        //                    new SqlParameter("@OrderDate", SqlDbType.DateTime, 4, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, logs.UTC_To_IST()),
        //                    new SqlParameter("@TrnMode", SqlDbType.TinyInt, 4, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, 11),
        //                    new SqlParameter("@OMemberID", SqlDbType.BigInt, 10, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, MemberID),
        //                    new SqlParameter("@PayMode", SqlDbType.TinyInt, 4, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, 3), // 3 for Transfer
        //                    //new SqlParameter("@OrdAmount", SqlDbType.Money, 10, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, item.Amount),
        //                    new SqlParameter("@OrdAmount", SqlDbType.Decimal, 18, ParameterDirection.Input, true, 18, 8, String.Empty, DataRowVersion.Default, item.Amount),
        //                    new SqlParameter("@DiscPer", SqlDbType.Float, 6, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, 0),
        //                    new SqlParameter("@DiscRs", SqlDbType.Money, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, 0),
        //                    new SqlParameter("@OBankID", SqlDbType.SmallInt, 8, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, 0),
        //                    new SqlParameter("@OBranchName", SqlDbType.VarChar, 50, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, BranchName),
        //                    new SqlParameter("@OAccountNo", SqlDbType.VarChar, 250, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, item.address),
        //                    new SqlParameter("@OChequeDate", SqlDbType.DateTime, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value),
        //                    new SqlParameter("@OChequeNo", SqlDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, ""),
        //                    new SqlParameter("@DMemberID", SqlDbType.BigInt, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, 1),
        //                    new SqlParameter("@DBankID", SqlDbType.SmallInt, 8, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, 0),
        //                    new SqlParameter("@DAccountNo", SqlDbType.VarChar, 25, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, ""),
        //                    new SqlParameter("@Status", SqlDbType.TinyInt, 4, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, 0),
        //                    new SqlParameter("@OrderType", SqlDbType.TinyInt, 4, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, 1),
        //                    new SqlParameter("@ORemarks", SqlDbType.VarChar, 250, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, "System Transaction Order"),
        //                    //new SqlParameter("@DeliveryAmt", SqlDbType.Money, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, item.value),
        //                    new SqlParameter("@DeliveryAmt", SqlDbType.Decimal, 18, ParameterDirection.Input, true, 18, 8, String.Empty, DataRowVersion.Default, item.Amount),
        //                    new SqlParameter("@DRemarks", SqlDbType.VarChar, 250, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default,""),
        //                    new SqlParameter("@DeliveryGivenBy", SqlDbType.BigInt, 10, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, 1),
        //                    new SqlParameter("@DeliveryGivenDate", SqlDbType.DateTime, 4, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value),
        //                    new SqlParameter("@AlertRec", SqlDbType.TinyInt, 4, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, 0),
        //                    new SqlParameter("@CashChargePer", SqlDbType.Float, 6, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, 0),
        //                    new SqlParameter("@CashChargeRs", SqlDbType.Money, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default,0),
        //                    new SqlParameter("@WalletAmt", SqlDbType.Money, 10, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, 0),
        //                    new SqlParameter("@ReturnCode", SqlDbType.Int, 8, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, RetCode),
        //                    new SqlParameter("@ReturnMsg", SqlDbType.VarChar, 500, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, RetMsg),
        //                    new SqlParameter("@OWalletID", SqlDbType.BigInt, 10, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, WalletID)
        //               };

        //                (new DataCommon()).ExecuteSP("sp_CreateOrder", ref Params);
        //                RetCode = Convert.ToInt16(Params[27].Value);
        //                RetMsg = Convert.ToString(Params[28].Value);
        //                if (!(Params[0].Value is DBNull))
        //                {
        //                    OrderID = Convert.ToInt64(Params[0].Value);
        //                    item.OrderId = OrderID;//rita 21-5-18 assign for next use
        //                }
        //                if (RetCode == 0 && OrderID > 0)
        //                {
        //                    CommonMethod.SqlStr = "Update WithdrawHistory set OrderID =" + OrderID + ",SystemRemarks='Order Created', UpdatedDateTime = dbo.GetISTDate() where TrnID = '" + item.txid + "' AND Address='"+ item.address +"'";
        //                    (new DataCommon()).ExecuteQuery(CommonMethod.SqlStr);
        //                }
        //                logs.WriteRequestLog("sp_CreateOrder :  " + RetMsg + " | " + item.txid, "CreateOrder", CommonMethod.SMSCode);
        //            }
        //            else
        //            {
        //                logs.WriteRequestLog("CreateOrder : Member not found or Wallet not found | " + item.txid + " Address:" + item.address, "CreateOrder", CommonMethod.SMSCode);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logs.WriteErrorLog(ex, "Program", "CreateOrder");
        //    }
        //}


        //private static void DeliveryProcessOrder(ref CommonMethods CommonMethod, RespTransfers item, short IsCryptoAPI = 0)
        //{
        //    int ReturnCode = 0;
        //    string ReturnMsg = string.Empty;
        //    long MemberID;
        //    long OrderID = 0;
        //    int Status = 0;
        //    DataSet dSet = new DataSet();
        //    DataRow dRows = null;
        //    string SqlStr = string.Empty;
        //    try
        //    {
        //        SqlStr = "SELECT TOP 1 m.MemberID , d.OrderID , d.Status FROM MemberBankMaster AS m INNER JOIN WithdrawHistory AS d ON m.AccountNo = d.Address AND m.Branchname=d.SMScode AND m.MemberID<>1 WHERE m.Type=1 and d.TrnID = '" + item.txid + "' AND d.Address='" + item.address + "'";//on txnid
        //        dSet = (new DataCommon()).OpenDataSet("MemberBankMaster", SqlStr, dSet, 30);
        //        if (dSet.Tables[0].Rows.Count > 0)
        //        {

        //            dRows = dSet.Tables[0].Rows[0];
        //            MemberID = Convert.ToInt64(dRows["MemberID"]);
        //            OrderID = Convert.ToInt64(dRows["OrderID"]);
        //            item.OrderId = OrderID;//rita 21-5-18 assign for next use
        //            Status = Convert.ToInt16(dRows["Status"]);
        //            if (OrderID > 0 && Status == 0 && MemberID != 0)
        //            {
        //                if (IsCryptoAPI == 1)//rita
        //                {
        //                    if (!AssetFromRequest(ref CommonMethod, item))
        //                    {
        //                        //return from here if fail
        //                        logs.WriteRequestLog("DeliveryProcessOrder : Crypto ThirdParty Revert Topup Call Fail, no delivery procced " + item.txid, "DeliveryProcessOrder", CommonMethod.SMSCode);
        //                        return;
        //                    }
        //                }
        //                SqlParameter[] Params = new SqlParameter[] {
        //                new SqlParameter("@OrderID", SqlDbType.BigInt, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, OrderID),
        //                new SqlParameter("@MemberID", SqlDbType.BigInt, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, MemberID),
        //                new SqlParameter("@Action", SqlDbType.TinyInt, 4, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, 1), // accept
        //                new SqlParameter("@DeliveryAmt", SqlDbType.Decimal, 18, ParameterDirection.Input, false, 18, 8, String.Empty, DataRowVersion.Default,item.Amount), // value converted
        //                new SqlParameter("@Remarks", SqlDbType.VarChar, 250, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, "System Delivery Process Order"),
        //                new SqlParameter("@GivenBy", SqlDbType.BigInt, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, 1), // by Org
        //                new SqlParameter("@ReturnCode", SqlDbType.Int, 8, ParameterDirection.Output, false, 0, 0, String.Empty, DataRowVersion.Default, 0),
        //                new SqlParameter("@ReturnMsg", SqlDbType.VarChar, 500, ParameterDirection.Output, false, 0, 0, String.Empty, DataRowVersion.Default, "")
        //                };

        //                (new DataCommon()).ExecuteSP("sp_DeliveryProcess", ref Params);

        //                ReturnCode = Convert.ToInt32(Params[6].Value);
        //                ReturnMsg = Convert.ToString(Params[7].Value);

        //                if (ReturnCode == 0) //Success
        //                {
        //                    CommonMethod.SqlStr = "UPDATE WithdrawHistory SET Status =" + 1 + ",SystemRemarks='Order Proceed Done', UpdatedDateTime = dbo.GetISTDate() where TrnID = '" + item.txid + "' AND Address='" + item.address + "'" +
        //                    ";INSERT INTO TradeDepositCompletedTrn(TrnID,Address, Status, CreatedTime) VALUES('" + item.txid + "','" + item.address + "', 1 , dbo.GetISTDate())";
        //                    (new DataCommon()).ExecuteQuery(CommonMethod.SqlStr);
        //                }
        //                else
        //                {
        //                    CommonMethod.SqlStr = "UPDATE WithdrawHistory SET Status =" + 9 + ",SystemRemarks='Order Rejected', UpdatedDateTime = dbo.GetISTDate() where TrnID = '" + item.txid + "' AND Address='" + item.address + "'";
        //                    (new DataCommon()).ExecuteQuery(CommonMethod.SqlStr);
        //                }

        //                logs.WriteRequestLog("sp_DeliveryProcess :  " + ReturnMsg + " | " + item.txid + " Address:" + item.address, "DeliveryProcessOrder", CommonMethod.SMSCode);
        //                //Console.WriteLine(CommonMethod.Transfers.Count + "  " + ReturnMsg);
        //            }
        //            else
        //            {
        //                logs.WriteRequestLog("DeliveryProcessOrder :  Member not found or Order not found " + item.txid + " Address:" + item.address, "DeliveryProcessOrder", CommonMethod.SMSCode);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logs.WriteErrorLog(ex, "Program", "DeliveryProcessOrder");
        //    }
        //}
        //rita 18-5-2018 added function for Send Notification for deduct amount from member and add in ORG
        //private static Boolean AssetFromRequest(ref CommonMethods CommonMethod, RespTransfers item)
        //{
        //    bool retValue = false;
        //    string DestAddress = string.Empty;
        //    string SqlStr = string.Empty;
        //    try
        //    {
        //        SqlStr = "Select Top 1 AccountNo From MemberBankMaster Where IsDefaultAddress=1 AND Type=1 AND Status=1 AND  MemberID=1 AND BranchName = '" + item.coin.ToUpper() + "'";
        //        DestAddress = (new DataCommon()).ExecuteScalarWDM(SqlStr);

        //        CommonMethod.RequestBody = CommonMethod.AssetFromRequest;//over right new request
        //        CallThirdPartyCryptoAPI(ref CommonMethod, item.address, item.txid, item.OrderId.ToString(), DestAddress, item.Amount);
        //        //logs.WriteRequestLog("AssetFrom API Response :", "AssetFromRequest", CommonMethod.DepositHistoryResponse);

        //        JObject GenerateResponse = JObject.Parse(CommonMethod.DepositHistoryResponse);
        //        // if (GenerateResponse["result"]!=null) //this will not work
        //        if (!string.IsNullOrEmpty(GenerateResponse["result"].ToString()))//rita 21-5-18 if fail response
        //        {
        //            retValue = true;
        //            SqlStr = "UPDATE WithdrawHistory SET APITopUpRefNo='" + GenerateResponse["result"].ToString() + "' WHERE TrnID = '" + item.txid + "' AND Address='" + item.address + "'";
        //            (new DataCommon()).ExecuteQuery(SqlStr);

        //        }
        //        //GenerateResponse["result"]["txid"] = dRow["TrnID"].ToString();
        //        //GenerateResponse["result"]["value"] = Convert.ToDecimal(dRow["Value"]);
        //        //GenerateResponse["result"]["Amount"] = dRow["Amount"].ToString();
        //        //GenerateResponse["result"]["OrderID"] = dRow["OrderID"].ToString();
        //        //GenerateResponse["result"]["address"] = dRow["Address"].ToString();
        //        //GenerateResponse["result"]["confirmations"] = GenerateResponse["result"]["confirmations"];
        //        //GenerateResponse["result"]["unconfirmedTime"] = GenerateResponse["result"]["blocktime"];
        //        //GenerateResponse["result"]["confirmedTime"] = GenerateResponse["result"]["timereceived"];

        //    }
        //    catch (Exception ex)
        //    {
        //        logs.WriteErrorLog(ex, "Program", "AssetFromRequest", item.OrderId.ToString());
        //    }
        //    return retValue;
        //}

        // For BitGo API


        private static void TradeWithdrawHistoryInsertion(ref CommonMethods CommonMethod)
        {
            DataSet dSet = new DataSet();
            //Int64 txno;
            Int32 actionType;
            //DataRow dRows = null;
            string SqlStr = string.Empty;
            try
            {
                foreach (var item in CommonMethod.Transfers)
                {
                    //Rushabh 09-07-2018 for ReconAction
                    item.TrnNo = GetTrnNo(item.id, CommonMethod.SMSCode, item.address);
                    actionType = CheckStatus(item.confirmations);
                    int Constatus = 0;
                    if (actionType > 3)
                    {
                        Constatus = 1;
                    }
                    else
                    {
                        Constatus = 4;
                    }
                    if (item.TrnNo > 0)
                    {
                        //if (Convert.ToInt16((new DataCommon()).ExecuteScalarWDM("Select Count(AccountNo) From MemberBankMaster Where AccountNo='" + item.address + "' AND BranchName = '" + item.coin.ToUpper() + "'")) > 0) 
                        //CommonMethod.SqlStr = "Select Count(AccountNo) From MemberBankMaster Where AccountNo = '" + item.address + "' AND BranchName = '" + CommonMethod.SMSCode.ToUpper() + "' AND SerProID=" + CommonMethod.ProviderID;
                        //if(Convert.ToInt32(new DataCommon().ExecuteScalarWDM(CommonMethod.SqlStr))>0)
                        //Rushabh 09-07-2018 Added provider condition
                        //{

                        CommonMethod.SqlStr = "SELECT TOP 1 AutoNo , Status From WithdrawHistory WHERE TrnID = '" + item.id + "' AND Address='" + item.address + "'";//Rita Added PK with ID and address , bcz now Same txnid with diff Address
                        dSet = (new DataCommon()).OpenDataSet("WithdrawHistory", CommonMethod.SqlStr, dSet, 30);

                        if (dSet.Tables[0].Rows.Count > 0)
                        {
                            // update
                            CommonMethod.SqlStr = "UPDATE WithdrawHistory SET Value =" + item.value + ", Amount =" + item.Amount + ", Address ='" + item.address + "', Confirmations =" + item.confirmations + ", confirmedTime ='" + item.confirmedTime + "', unconfirmedTime  ='" + item.unconfirmedTime + "',TrnNo=" + item.TrnNo + ",Charge=" + item.Fee + ",RouteTag='" + CommonMethod.RouteTag + "',SerProID=" + CommonMethod.SerProID + ", UpdatedDateTime = dbo.GetISTDate() WHERE TrnID = '" + item.id + "' AND Address='" + item.address + "'";
                            (new DataCommon()).ExecuteQuery(CommonMethod.SqlStr);
                            //(new CommonFunctions()).AddToLog(item.coin + " : " + i++);
                            logs.WriteRequestLog("Update Withdraw History :  " + item.id + " Address:" + item.address + " TrnNo:" + item.TrnNo, "TradeWithdrawHistoryInsertion", CommonMethod.SMSCode);
                        }
                        else
                        {
                            // insert
                            CommonMethod.SqlStr = "INSERT INTO WithdrawHistory(TrnID,SMSCode,Wallet,Address,Status,Confirmations,Value,confirmedTime,unconfirmedTime,createdTime,State,CreatedDateTime,TrnNo,Amount,SystemRemarks,ToAddress,Charge,RouteTag,SerProID) " +
                            " VALUES('" + item.id + "','" + CommonMethod.SMSCode.ToUpper() + "','" + item.WalletID + "','" + item.address + "'," + Constatus + "," + item.confirmations + "," + item.value + ",'" + item.confirmedTime + "','" + item.unconfirmedTime + "','" + item.createdTime + "','" + item.state + "', dbo.GetISTDate()," + item.TrnNo + "," + item.Amount + ",'Inserted','" + item.address + "'," + item.Fee + ",'" + CommonMethod.RouteTag + "'," + CommonMethod.SerProID + ")";
                            (new DataCommon()).ExecuteQuery(CommonMethod.SqlStr);

                            // create Order
                            //CreateOrder(ref CommonMethod, item);
                            logs.WriteRequestLog("Insert Withdraw History with Txnid :  " + item.txid + " Address:" + item.address + " TrnNo:" + item.TrnNo, "TradeWithdrawHistoryInsertion", CommonMethod.SMSCode);
                        }

                        // Delivery Process Order                    
                        //if ((new DataCommon()).ExecuteScalarWDM("Select Count(TrnID) From TradeDepositCompletedTrn Where TrnID='" + item.txid + "' AND Address='" + item.address + "'") == "0")
                        //{
                        //if (item.TrnNo > 0 && actionType == 4)
                        //{
                        //DeliveryProcessOrder(ref CommonMethod, item);
                        //Add here TQ condition if status=4
                        CommonMethod.SqlStr = "SELECT Status FROM TransactionQueue WHERE TrnNo=" + item.TrnNo + " AND SMSCode='" + CommonMethod.SMSCode + "'";
                        Int32 Status = Convert.ToInt32(new DataCommon().ExecuteScalarWDM(CommonMethod.SqlStr));
                        if (Status == 4 && actionType == 4)
                        {
                            ReconAction(item.TrnNo, actionType, CommonMethod.SMSCode);//Rushabh 09-07-2018 Replace DeliveryProcessOrder With ReconAction
                        }

                        //}
                        //}
                        //else // Order Already Intialized
                        //{
                        //    SqlStr = "UPDATE WithdrawHistory SET status = 9,SystemRemarks='Txn Already Proceed BitGo' WHERE TrnID = '" + item.txid + "'  AND Address='" + item.address + "'";
                        //    (new DataCommon()).ExecuteQuery(SqlStr);
                        //    logs.WriteRequestLog("Order Already Intialized | " + item.txid + " Address:" + item.address, "TradeDepositHistoryUpdationForCryptoCoin", CommonMethod.SMSCode);
                        //}
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                logs.WriteErrorLog(ex, "Program", "TradeDepositHistoryInsertion");
            }
        }

        //Rushabh 05-07-2018 for BitGo API
        private static Int64 GetTrnNo(string txid, string wallet, string address)//Rushabh 05-07-2018 for getting TrnNo which will be passed to ReconAction
        {
            Int64 no = 0;
            try
            {
                string SqlStr = "SELECT TQ.TrnNo FROM TransactionQueue TQ INNER JOIN TransactionRequest TR ON TR.TrnNo=TQ.TrnNo and TR.OprTrnID='" + txid + "' WHERE TQ.TrnType=6 and TQ.Status=4 and TQ.SMSCode='" + wallet + "'" + " and TQ.CustomerMobile='" + address + "'";
                string txno = (new DataCommon()).ExecuteScalarWDM(SqlStr);
                no = Convert.ToInt64(txno);
            }
            catch (Exception ex)
            {
                logs.WriteErrorLog(ex, "Program", "GetTrnNo");
            }
            return no;
        }


        private static void ReconAction(Int64 TrnNo, Int32 ActionType, string SMSCode)
        {
            string RetMsg = "";
            Int32 RetCode = 0;
            //, string ActionRemarks, Int16 RetCode, string RetMsg
            try
            {
                SqlParameter[] Params = new SqlParameter[]
                {
                    new SqlParameter("@TrnNo", SqlDbType.BigInt, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, TrnNo),
                    new SqlParameter("@ActionType", SqlDbType.TinyInt, 4, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, ActionType),
                    new SqlParameter("@ActionBy", SqlDbType.BigInt, 10, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, 999),
                    new SqlParameter("@ActRemarks", SqlDbType.VarChar, 150, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, "System.Withdraw.Success"),
                    new SqlParameter("@ReturnCode", SqlDbType.Int, 8, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, 0),
                    new SqlParameter("@ReturnMsg", SqlDbType.VarChar, 500, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, "")
                };
                (new DataCommon()).ExecuteSP("sp_ReconAction", ref Params);

                if (Params[4].Value != DBNull.Value)
                    RetCode = Convert.ToInt16(Params[4].Value);

                if (Params[5].Value != DBNull.Value)
                    RetMsg = Convert.ToString(Params[5].Value);

                logs.WriteRequestLog("Recon Completed RetCode:" + RetCode.ToString() + " RetMsg:" + RetMsg, "ReconAction", SMSCode);
            }
            catch (Exception ex)
            {
                logs.WriteErrorLog(ex, "Program", "ReconAction");
            }
        }



        #endregion

        #region "CallThirdPartyAPI"

        private static string CallThirdPartyAPI(ref CommonMethods CommonMethod, string trnID, string Authorization, string enterprise, DataSet dSet)
        {
            try
            {
                //CommonMethods CommonMethod = (CommonMethods)CommonMethodObj;
                if (!string.IsNullOrEmpty(CommonMethod.Str_URL) && !string.IsNullOrEmpty(CommonMethod.Str_RequestType) && !string.IsNullOrEmpty(CommonMethod.ContentType))
                {
                    //vsolanki
                    if (CommonMethod.Str_URL.Contains("#trnID#"))
                    {
                        CommonMethod.Str_URL = CommonMethod.Str_URL.Replace("#trnID#", trnID);
                    }
                    //CommonMethod.Str_URL = CommonMethod.Str_URL.Replace("#ProviderWalletID#", CommonMethod.ProviderWalletID);
                    logs.WriteRequestLog("Request :  " + CommonMethod.Str_URL, "CallThirdPartyAPI", CommonMethod.SMSCode);

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(CommonMethod.Str_URL);
                    httpWebRequest.ContentType = CommonMethod.ContentType;
                    httpWebRequest.Method = CommonMethod.Str_RequestType;
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.Timeout = 180000;
                    //vsolanki
                    httpWebRequest.Headers.Add("Authorization", Authorization);
                    httpWebRequest.Headers.Add("enterprise", enterprise);

                    HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                    using (StreamReader StreamReaderObj = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        CommonMethod.DepositHistoryResponse = StreamReaderObj.ReadToEnd();
                        StreamReaderObj.Close();
                        StreamReaderObj.Dispose();
                    }
                    httpWebResponse.Close();
                    //logs.WriteRequestLog("Response :  " + CommonMethod.DepositHistoryResponse, "CallThirdPartyAPI", CommonMethod.SMSCode);                   

                }
                else
                {
                    logs.WriteRequestLog("Generate Response :  Transaction Detail not found", "CallThirdPartyAPI", CommonMethod.SMSCode);
                }
                return CommonMethod.DepositHistoryResponse;
            }
            catch (Exception ex)
            {
                logs.WriteErrorLog(ex, "Program", "CallThirdPartyAPI");
                return ex.ToString();
            }
        }

        private static void ParseBitGoAPIResponse(ref CommonMethods CommonMethod)
        {
            try
            {
                JObject GenerateResponse = JObject.Parse(CommonMethod.DepositHistoryResponse);
                //JArray jarr = (JArray)person1.SelectToken("transfers");
                if (Convert.ToString(GenerateResponse.SelectToken("coin")).ToLower() == CommonMethod.SMSCode.ToLower())
                {
                    if (CommonMethod.Category == 1) // For BTC , BTG , BCH , XRP , LTC Response 
                    {
                        //Rushabh 02-07-2018 as We get 'transactions' token instead of 'transfer' in withdraw response.
                        //foreach (var item in GenerateResponse.SelectToken("transfers").Select((val, i) => new { i, val }))
                        //foreach (var item in GenerateResponse.SelectToken("transactions").Select((val, i) => new { i, val }))
                        //{
                            //item.val["IsValid"] = false;
                            
                               
                                    foreach (var item in GenerateResponse.SelectToken("outputs")) 
                                    {
                                        if (!IsNullOrEmpty(item["id"]) && !IsNullOrEmpty(item["address"]) && !IsNullOrEmpty(item["value"]) && !IsNullOrEmpty(item["valueString"]) && Convert.ToBoolean(item["isSegwit"]) == false && IsNullOrEmpty(item["wallet"]) && IsNullOrEmpty(item["redeemScript"]))
                                        {
                                            //if (Convert.ToString(item1["address"]) == CommonMethod.orgwallet && Convert.ToDecimal(item1["value"]) > 0)
                                            //{
                                            string value = (string)item.SelectToken("address");
                                            item["address"] = value;
                                            item["value"] = Convert.ToDecimal(item["value"]);
                                            item["Amount"] = Convert.ToDecimal(item["value"]) / CommonMethod.CovertAmount;
                                            item["confirmedTime"] = Convert.ToString(item["date"]);
                                            item["Fee"] = Convert.ToDecimal(item["fee"]);
                                            item["WalletID"] = Convert.ToString(item["fromWallet"]);
                                            item["IsValid"] = true;
                                            //}
                                        }
                                    }
                                
                            

                            //if (Convert.ToBoolean(item.val["IsValid"]))
                            //{
                            //    CommonMethod.Transfers.Add(JsonConvert.DeserializeObject<RespTransfers>(JsonConvert.SerializeObject(item.val)));
                            //    Console.WriteLine(CommonMethod.SMSCode + " Count : " + CommonMethod.Transfers.Count);
                            //}
                       // }
                    }
                    #region crypto
                    //else if (CommonMethod.Category == 2) // For LTC , XRP Response
                    //{
                    //    foreach (var item in GenerateResponse.SelectToken("transfers").Select((val, i) => new { i, val }))
                    //    {
                    //        item.val["IsValid"] = false;
                    //        if (!IsNullOrEmpty(item.val["entries"]) && !IsNullOrEmpty(item.val["id"]) && !IsNullOrEmpty(item.val["txid"]) && !IsNullOrEmpty(item.val["coin"]) && !IsNullOrEmpty(item.val["wallet"]) && !IsNullOrEmpty(item.val["value"]) && !IsNullOrEmpty(item.val["state"]))
                    //        {

                    //            foreach (var item1 in item.val.SelectToken("entries"))
                    //            {
                    //                if (Convert.ToString(item1["value"]) == Convert.ToString(item.val["value"]) && !IsNullOrEmpty(item1["wallet"]))
                    //                {
                    //                    string value = (string)item1.SelectToken("address");
                    //                    item.val["address"] = value;
                    //                    if (Convert.ToString(item.val["coin"]) == "xrp" && !Convert.ToString(item1.SelectToken("address")).Contains("?dt="))
                    //                    {
                    //                        item.val["IsValid"] = false;
                    //                        continue;
                    //                    }
                    //                    item.val["Amount"] = Convert.ToDecimal(item.val["value"]) / Convert.ToDecimal(ConfigurationManager.AppSettings[Convert.ToString(item.val["coin"])]);
                    //                    item.val["IsValid"] = true;
                    //                }
                    //            }
                    //        }
                    //        if (Convert.ToBoolean(item.val["IsValid"]))
                    //        {
                    //            CommonMethod.Transfers.Add(JsonConvert.DeserializeObject<RespTransfers>(JsonConvert.SerializeObject(item.val)));
                    //            Console.WriteLine(CommonMethod.SMSCode + " Count : " + CommonMethod.Transfers.Count);
                    //        }
                    //    }
                    //}
                }
                //logs.WriteRequestLog("Generate Response :  " + JsonConvert.SerializeObject(CommonMethod.Transfers), "CallThirdPartyAPI", CommonMethod.SMSCode);
                #endregion
            }
            catch (Exception ex)
            {
                logs.WriteErrorLog(ex, "Program", "ParseBitGoAPIResponse");
            }
        }

        private static string CallThirdPartyCryptoAPI(ref CommonMethods CommonMethod, string Address, string TrnID, string AutoNo, string SourceAddress = "", decimal Amount = 0)
        {
            try
            {
                //CommonMethods CommonMethod = (CommonMethods)CommonMethodObj;
                if (!string.IsNullOrEmpty(CommonMethod.Str_URL) && !string.IsNullOrEmpty(CommonMethod.Str_RequestType) && !string.IsNullOrEmpty(CommonMethod.ContentType))
                {

                    string authInfo = CommonMethod.UserName + ":" + CommonMethod.Password;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                    WebRequest myReqrpc = WebRequest.Create(CommonMethod.Str_URL);
                    myReqrpc.Headers["Authorization"] = "Basic " + authInfo;
                    myReqrpc.Method = CommonMethod.Str_RequestType;

                    string ReqStr = @"" + CommonMethod.RequestBody; //@"{""id"":""" + 1 + @""",""method"":""getnewaddress"",""params"":[]}";
                    ReqStr = ReqStr.Replace("#Address#", Address);//Address
                    ReqStr = ReqStr.Replace("#TrnID#", TrnID);//TrnID
                    ReqStr = ReqStr.Replace("#AutoNo#", AutoNo);//TrnNo
                    ReqStr = ReqStr.Replace("#SourceAddress#", SourceAddress);//rita 18-5-18 needed for Deduction call to RPC                    
                    ReqStr = ReqStr.Replace("#Amount#", Amount.ToString());//rita 18-5-18 needed for Deduction call to RPC                    
                    logs.WriteRequestLog("RPC Address generate Request :  " + ReqStr, "CallThirdPartyCryptoAPI", CommonMethod.SMSCode);
                    StreamWriter sw = new StreamWriter(myReqrpc.GetRequestStream());
                    sw.Write(ReqStr);
                    sw.Close();

                    WebResponse response = myReqrpc.GetResponse();

                    StreamReader StreamReader = new StreamReader(response.GetResponseStream());
                    CommonMethod.DepositHistoryResponse = StreamReader.ReadToEnd();
                    StreamReader.Close();
                    response.Close();

                    logs.WriteRequestLog("RPC Address Response :  " + CommonMethod.DepositHistoryResponse, "CallThirdPartyCryptoAPI", CommonMethod.SMSCode);
                    return CommonMethod.DepositHistoryResponse;
                }
                else
                {
                    logs.WriteRequestLog("Generate Response :  Transaction Detail not found", "CallThirdPartyCryptoAPI", CommonMethod.SMSCode);
                    return CommonMethod.DepositHistoryResponse;
                }
            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string Text = reader.ReadToEnd();
                    if (Text.ToLower().Contains("code"))
                        CommonMethod.DepositHistoryResponse = Text;
                    webex = null;
                    logs.WriteRequestLog("BlockChainTransfer exception : " + CommonMethod.DepositHistoryResponse, "CallThirdPartyCryptoAPI", CommonMethod.SMSCode);
                }

                logs.WriteRequestLog("webex : " + webex, "CallThirdPartyCryptoAPI", CommonMethod.SMSCode);
                return CommonMethod.DepositHistoryResponse;
            }
        }

        #endregion


        #region "ReadFiles"

        public static void ReadMasterFile(string APIName, ref CommonMethods CommonMethod)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            string FilePath = Configuration["MainPath"] + "\\Configuration\\MasterFile_" + APIName + ".txt";

            try
            {
                if (System.IO.File.Exists(FilePath) == true)
                {
                    CommonMethod.StaticArray[0] = "0";
                    CommonMethod.TransactionFile = Configuration["MainPath"] + "\\Configuration"; //FilePath

                    string[] lines = System.IO.File.ReadAllLines(FilePath);
                    foreach (string line in lines)
                    {
                        CommonMethod.LeftTitle = line.Substring(0, line.IndexOf(CommonMethod.MainSaperator)).ToLower();

                        if (CommonMethod.LeftTitle.ToUpper().Contains("STATIC"))
                        {
                            //Start with #1 Position
                            CommonMethod.StaticArray[CommonMethod.StaticCnt++] = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                            //StaticCnt++;
                        }
                        else if (CommonMethod.LeftTitle == "apiname")
                        {
                            CommonMethod.TransactionFile = CommonMethod.TransactionFile + "\\" + line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1) + "\\";
                        }
                        else if (CommonMethod.LeftTitle == "requesttype")
                        {
                            CommonMethod.RequestType = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.LeftTitle == "transactionfilepath")
                        {
                            CommonMethod.Path_AddressGenerate = CommonMethod.TransactionFile + line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                    }

                    logs.WriteRequestLog("Transaction File Path :  " + CommonMethod.Path_AddressGenerate, "ReadMasterFile", CommonMethod.SMSCode);
                }
                else
                {

                    logs.WriteRequestLog(FilePath + " File Not Found", "ReadMasterFile", CommonMethod.SMSCode);
                }
            }
            catch (Exception ex)
            {
                logs.WriteErrorLog(ex, "Program", "ReadMasterFile");
            }
        }

        public static void ReadTransactionalFile(string Path, ref CommonMethods CommonMethod)
        {
            try
            {
                //CommonMethods CommonMethod = (CommonMethods)CommonMethodObj;
                if (System.IO.File.Exists(Path) == true)
                {
                    string[] lines = System.IO.File.ReadAllLines(Path);
                    foreach (string line in lines)
                    {
                        CommonMethod.TrnLeftTitle = line.Substring(0, line.IndexOf(CommonMethod.MainSaperator)).ToLower();

                        if (CommonMethod.TrnLeftTitle.ToUpper().Contains("URL")) //Read URL
                        {
                            CommonMethod.Str_URL = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("requesttype")) //Read Request Type 
                        {
                            CommonMethod.Str_RequestType = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("contenttype")) //Read Content Type
                        {
                            CommonMethod.ContentType = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("category")) //Read Category Type
                        {
                            CommonMethod.Category = Convert.ToInt16(line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1));
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("orgwallet")) //Read orgwallet Type  -- Parth 27-06-2018 Wallet Compare for BitgoAPI
                        {
                            CommonMethod.orgwallet = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("assetname")) //Read AssetName
                        {
                            CommonMethod.AssetName = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("username")) //Read Username
                        {
                            CommonMethod.UserName = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("password")) //Read Password
                        {
                            CommonMethod.Password = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("requestbody")) //Read RequestBody
                        {
                            CommonMethod.RequestBody = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("assetfromrequest")) //Read assetfromrequest Rita 18-5-18 second RPC call
                        {
                            CommonMethod.AssetFromRequest = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("authorization"))
                        {
                            CommonMethod.Authorization = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("trnid"))
                        {
                            CommonMethod.TrnId = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        else if (CommonMethod.TrnLeftTitle.Contains("enterprise"))
                        {
                            CommonMethod.enterprise = line.Substring(line.IndexOf(CommonMethod.MainSaperator) + 1);
                        }
                        //Console.WriteLine(CommonMethod.Str_URL + CommonMethod.Str_RequestType + CommonMethod.ContentType);
                    }

                    logs.WriteRequestLog("Transaction URL :  " + CommonMethod.Str_URL + " Request Type : " + CommonMethod.Str_RequestType + " Content Type : " + CommonMethod.ContentType, "ReadTransactionalFile", CommonMethod.SMSCode);
                }
                else
                {
                    logs.WriteRequestLog(Path + " File Not Found", "ReadTransactionalFile", CommonMethod.SMSCode);
                }
            }
            catch (Exception ex)
            {
                logs.WriteErrorLog(ex, "Program", "ReadTransactionalFile");
            }
        }
        #endregion

        public class walletServiceData
        {
            public int RecordCount { get; set; }
            public Int32 ServiceID { get; set; }
            public string SMSCode { get; set; }
            public int WallletStatus { get; set; }
            public int ServiceStatus { get; set; }
            public int AppType { get; set; }
            public string RouteTag { get; set; }
            public decimal CAmount { get; set; }
            public Int64 SerProID { get; set; }//Rushabh 09-07-2018 for chaking condition in TradeWithdrawHistoryInsertion 
        }

        public class CommonMethods
        {
            public string TransactionFile;
            public string line;
            public string LeftTitle = null;
            public string[] StaticArray = new string[20];
            public char MainSaperator = ':';
            public int StaticCnt = 1;
            public string TrnLeftTitle = null;
            public StreamReader MsgFile;
            public string RequestType;
            public string Path_AddressGenerate, Path_CustomerDetail, Path_CustomerValidate, Path_CustomerRegistration, Path_BeneRegistration, Path_VerifyBeneficiary, Path_DeleteBeneficiary, Path_VerifyDeleteBeneficiary, PubKey;
            public string Str_URL = string.Empty;
            public string Str_RequestType;
            public string ContentType;
            public string Address1, Address2;
            public string ReturnMsg1, ReturnMsg2;
            public string SuccAPICodes;
            public string[] SuccAPICodesArray = new string[10];
            public string MatchRegex2;
            public string BeneActiveCode;
            public string DepositHistoryResponse;
            public List<RespTransfers> Transfers { get; set; }
            public string SqlStr = string.Empty;
            public DataSet dSet = new DataSet();
            public DataRow dRows = null;
            public string SMSCode = string.Empty;
            public int Category;
            public string UserName;
            public string AssetName;
            public string Password;
            public string RequestBody;
            public string AssetFromRequest;
            public List<RespLocalCoin> RespLocalCoins { get; set; }
            public string orgwallet;
            public Int64 SerProID;
            public string RouteTag;
            public decimal CovertAmount=100000;//Rushabh 04-07-2018 to divide outputs(JToken)'s value with it instead of AppSettings[Convert.ToString(item.val["coin"])] .
            //vsolanki
            public string TrnId { get; set; }
            public string Authorization;
            public string enterprise;

        }


        enum EnAppType
        {
            BITGO = 1,
            CRYPTO = 2
        }
        enum EnTrnType
        {
            //Rushabh 28-06-2018
            Transaction = 1,
            Buy_Trade = 4,
            Sell_Trade = 5,
            Withdraw = 6,
            Shoping_Cart = 7,
            Deposit = 8,
            Generate_Address = 9
        }

        public class RespTransfers
        {
            public string id { get; set; }
            public string coin { get; set; }
            public string WalletID { get; set; }
            public string txid { get; set; }
            public decimal Fee { get; set; }
            public string address { get; set; }
            //public int height { get; set; }
            //public DateTime date { get; set; }
            public int confirmations { get; set; }
            public long value { get; set; }
            //public string valueString { get; set; }
            //public string feeString { get; set; }
            //public int payGoFee { get; set; }
            //public string payGoFeeString { get; set; }
            //public double usd { get; set; }
            //public double usdRate { get; set; }
            public string state { get; set; }
            //public IList<string> tags { get; set; }
            public string confirmedTime { get; set; }
            public string unconfirmedTime { get; set; }
            public string createdTime { get; set; }
            public bool IsValid { get; set; }
            public decimal Amount { get; set; }
            //public long OrderId { get; set; }
            public long TrnNo { get; set; }
        }

        public class RespLocalCoin
        {
            public int confirmations { get; set; }
            public string txid { get; set; }
            public string address { get; set; }
            public string confirmedTime { get; set; }
            public string unconfirmedTime { get; set; }
            public int value { get; set; }
            public decimal Amount { get; set; }
        }

        public static bool IsNullOrEmpty(JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }
    }
}