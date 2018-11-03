using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.Wallet;
using System;
using System.Data;
using System.Threading;
using System.Timers;

namespace StatusCheckDeposition
{
    public class Program
    {
        static System.Timers.Timer TopupTick = new System.Timers.Timer();
        static System.Timers.Timer TransactionTick = new System.Timers.Timer();
        static IWebApiRepository _webApiRepository;
        static bool IsProcessing = false;

        public Program(IWebApiRepository webApiRepository)
        {
            _webApiRepository = webApiRepository;
        }
        public static void Main(string[] args)
        {
            TransactionTick.Interval = Convert.ToInt32(1000); // 1000;
            TransactionTick.Elapsed += new ElapsedEventHandler(transaction_tick);
            TransactionTick.Start();
            //SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true); //process rudely exits, 
            Console.WriteLine("Press \'q\' to quit");
            while (Console.Read() != 'q') ;
        }
        #region TimerTick
        private static void transaction_tick(object sender, System.EventArgs e)
        {
            try
            {
                TransactionTick.Stop();
                TransactionTick.Interval = 1800000; // For Testing temperory 
                CallAPI();
            }
            catch (Exception ex)
            {
                ex = null;
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
                var items = _webApiRepository.StatusCheck();
                if(items.Count>0)
                {
                    for(int i=0;i<= items.Count-1;i++)
                    {
                        WalletServiceData walletServiceDataObj = new WalletServiceData();
                        walletServiceDataObj.ServiceID =Convert.ToInt32(items[0]);
                        walletServiceDataObj.RecordCount = 5;
                        walletServiceDataObj.SMSCode = items[1].ToString();
                        walletServiceDataObj.WallletStatus = Convert.ToInt32(items[2]);
                        walletServiceDataObj.ServiceStatus = Convert.ToInt32(items[3]);
                        lock (walletServiceDataObj)
                        {
                            if (IsProcessing == false)
                            {
                                WaitCallback callBack;
                                callBack = new WaitCallback(CallAPISingle); // create thread for each SMSCode
                                ThreadPool.QueueUserWorkItem(callBack, walletServiceDataObj);
                            }
                            else
                            {
                                //logs.WriteRequestLog("IsProcessing = true so return ", "CallAPI", walletServiceDataObj.SMSCode, Action: 2);
                            }                           
                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                TransactionTick.Start();
            }
        }
        private static void CallAPISingle(object RefObj)
        {

        }
        #endregion        
    }
}
