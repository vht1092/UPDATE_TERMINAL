using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.ServiceProcess;
using System.Diagnostics;


namespace UPDATE_TERMINAL
{
    class classCheckErr
    {
        public static bool _exitThread = false;
        public static string _updateDateTime = null;

        public static classDataAccess _dataAccess = new classDataAccess();
        public static string SMS_TYPE1 = "TXNMSG";
        public static string SMS_TYPE2 = "MANPAY";

        private static string SCBPhone = "";       
        private static string mobile  = classUtilities.GetStringValueFromConfig("MyPhone1");
        private static string mobile2 = classUtilities.GetStringValueFromConfig("MyPhone2");
        private static string mobile3 = classUtilities.GetStringValueFromConfig("MyPhone3");
        //private static string mobile4 = classUtilities.GetStringValueFromConfig("MyPhone4");
        //private static string mobile5 = classUtilities.GetStringValueFromConfig("MyPhone5");
        //private static string mobile6 = classUtilities.GetStringValueFromConfig("MyPhone6");
        private static string mobile7 = classUtilities.GetStringValueFromConfig("MyPhone7");
        //private static string mobile8 = classUtilities.GetStringValueFromConfig("MyPhone8");
        //private static string mobile9 = classUtilities.GetStringValueFromConfig("MyPhone9");
        //private static string mobile10 = classUtilities.GetStringValueFromConfig("MyPhone10");

        public static string CheckServiec(string SERVICENAME)
        {
            ServiceController sc = new ServiceController(SERVICENAME);

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    return "Running";
                case ServiceControllerStatus.Stopped:
                    return "Stopped";
                case ServiceControllerStatus.Paused:
                    return "Paused";
                case ServiceControllerStatus.StopPending:
                    return "Stopping";
                case ServiceControllerStatus.StartPending:
                    return "Starting";
                default:
                    return "Status Changing";
            }
        }

        public static int CheckProcess(string proces_name)
        {
            try
            {
                int flag = 0;
                string message = null;
                Process[] pname = Process.GetProcessesByName(proces_name);
                if (pname.Length == 0)
                {
                    message = "Process SendSMSForMC not running! " + DateTime.Now.ToString();
                    SendSmsForCheckProcess(mobile, message);
                    SendSmsForCheckProcess(mobile2, message);
                    SendSmsForCheckProcess(mobile3, message);
                    classCheckErrLogWriter.WriteLog("Process SendSMSForMC not running! " + DateTime.Now.ToString());
                    flag= 1;
                }
                if (pname.Length > 1)
                {
                    message = "SendSMSForMC have " + pname.Length + " process runing " + DateTime.Now.ToString();
                    SendSmsForCheckProcess(mobile, message);
                    SendSmsForCheckProcess(mobile2, message);
                    SendSmsForCheckProcess(mobile3, message);
                    classCheckErrLogWriter.WriteLog("SendSMSForMC have " + pname.Length + " process runing " + DateTime.Now.ToString());
                    flag = 1;
                }
               
                return flag;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error CheckProcess():" + ex.Message);
                return 0;
            }
        }

        public static void SendSmsForCheckProcess(string mobile, string message)
        {
            try
            {

                _dataAccess.OpenConnectionOracle("EBANK_GW");
                string SMS_TYPE = "SMSME";

                _dataAccess.InsertSMSMessateToEBankGW(classDataAccess.IDALERT
                                                            , mobile
                                                            , message
                                                            , 'N'//Y: (se ko gui tin nhan),//N: se gui tin nhan
                                                            , SMS_TYPE);
                _dataAccess.CloseConnectionOracle();
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error SendSmsForCheckProcess():" + ex.Message);
            }
        }
        
        public static void RunService()
        {
           try
           {

                int minute = 0;
                int hour = 0;               
                int value = 0;
               
                SCBPhone = classUtilities.GetStringValueFromConfig("SCB_Contact_Phone");
                string minute_run = classUtilities.GetStringValueFromConfig("Exp_User_Minute");
                string hour_run=classUtilities.GetStringValueFromConfig("Exp_User_Hour");
                string min_check_batch = classUtilities.GetStringValueFromConfig("Check_Batch_Minute");
                string Hour_check_batch = classUtilities.GetStringValueFromConfig("Check_Batch_Hour");
                int flag = 0; 
                int flag1 = 0;
                int flag2 = 0;
                int flag3 = 0;
                int flag4 = 0;
                int flag5 = 0;
                int flag6 = 0;
                int flag7 = 0;
                int flag8 = 0;
                int flag9 = 0;
                int flag10 = 0;
                int flag11 = 0;
                int flag12 = 0;
                int flag13 = 0;
                
                

                while (_exitThread == false)
                {
                    minute = DateTime.Now.Minute;
                    hour = DateTime.Now.Hour;
                    flag = flag1 + flag2 + flag3 + flag4 + flag5 + flag6 + flag7 + flag8 + flag9 + flag10 + flag11 + flag12 + flag13;
                    if (flag == 0)
                        value = classUtilities.GetIntValueFromConfig("CheckErr_Minute");
                    else
                        value = classUtilities.GetIntValueFromConfig("CheckErr_Minute2");
                    //if (hour == int.Parse(Hour_check_batch) && minute % int.Parse(min_check_batch) == 0)
                    if (hour == int.Parse(Hour_check_batch) && minute == int.Parse(min_check_batch))
                    {                         
                        string sms_str = "";
                        sms_str = Check_Start_Batch();
                        if (sms_str != "")
                        {
                            SendSMS_With_Mess(mobile, sms_str);
                            SendSMS_With_Mess(mobile2, sms_str);
                            SendSMS_With_Mess(mobile3, sms_str);
                            SendSMS_With_Mess(mobile7, sms_str);
                            classCheckErrLogWriter.WriteLog("Err check start batch" + sms_str);
                            //Thread.Sleep(1000 * 55);
                        }
                    }
                    if (hour == int.Parse(hour_run) && minute % int.Parse(minute_run) == 0)
                    //if(1==1)//hhhh
                    {
                        //////////check user expiry
                        string sms_str = "";
                        sms_str = Check_Expiry_User();
                        if (sms_str != "")
                        {
                            SendSMS_With_Mess(mobile, sms_str);
                            SendSMS_With_Mess(mobile2, sms_str);
                            SendSMS_With_Mess(mobile3, sms_str);
                            SendSMS_With_Mess(mobile7, sms_str);
                            classCheckErrLogWriter.WriteLog("Err Expiry User" + sms_str);
                            flag11 = 1;
                        }
                        else
                            flag11 = 0;

                    }
                    if(minute % value == 0)// && flag==0)
                    {
                        
                        classCheckErrLogWriter.WriteLog("----------------Begin Process-----------------");
                        //////////////check diff Branch                        
                        if (Check_Diff_Branch() != "")
                        //if (1==1)
                        {
                            string sms_str = Check_Diff_Branch();
                            SendSMS_With_Mess(mobile, sms_str);
                            SendSMS_With_Mess(mobile2, sms_str);
                            SendSMS_With_Mess(mobile3, sms_str);
                            SendSMS_With_Mess(mobile7, sms_str);
                            flag10 = 1;
                            classCheckErrLogWriter.WriteLog("Err diff Branch");
                        }
                        else
                            flag10 = 0;                       
                        //////////////check diff MSL                        
                        if (Check_Diff_MSL() != "")
                        //if (1==1)
                        {
                            string sms_str = Check_Diff_MSL();
                            SendSMS_With_Mess(mobile, sms_str);
                            SendSMS_With_Mess(mobile2, sms_str);
                            SendSMS_With_Mess(mobile3, sms_str);
                            SendSMS_With_Mess(mobile7, sms_str);
                            flag9 = 1;
                            classCheckErrLogWriter.WriteLog("Err diff MSL");
                        }
                        else
                            flag9 = 0;
                        /////////////
                        if (Check_Err_DEKM_IM() == 1)
                        {
                            SendSMSForDEKM(mobile,"IM");
                            SendSMSForDEKM(mobile2, "IM");
                            SendSMSForDEKM(mobile3, "IM");
                            SendSMSForDEKM(mobile7, "IM");
                            flag8 = 1;
                            classCheckErrLogWriter.WriteLog("Err DEKM IM");
                        }
                        else
                            flag8 = 0;
                        if (Check_Err_DEKM_AM() == 1)
                        {
                            SendSMSForDEKM(mobile, "AM");
                            SendSMSForDEKM(mobile2, "AM");
                            SendSMSForDEKM(mobile3, "AM");
                            SendSMSForDEKM(mobile7, "AM");
                            flag12 = 1;
                            classCheckErrLogWriter.WriteLog("Err DEKM AM");
                        }
                        else
                            flag12 = 0;

                        if (Check_Err_DEKM_DW() == 1)
                        {
                            SendSMSForDEKM(mobile, "DW");
                            SendSMSForDEKM(mobile2, "DW");
                            SendSMSForDEKM(mobile3, "DW");
                            SendSMSForDEKM(mobile7, "DW");
                            flag13 = 1;
                            classCheckErrLogWriter.WriteLog("Err DEKM DW");
                        }
                        else
                            flag13 = 0;
                        /////////
                        int min_ago = Check_Connect_SML();
                        if (min_ago > 0)
                        {
                            SendSMSForConnect_SML(mobile, min_ago);
                            SendSMSForConnect_SML(mobile2, min_ago);
                            SendSMSForConnect_SML(mobile3, min_ago);
                            //SendSMSForConnect_SML(mobile4, min_ago);
                            SendSMSForConnect_SML(mobile7, min_ago);
                            //SendSMSForConnect_SML(mobile8, min_ago);
                            flag7 = 1;
                            classCheckErrLogWriter.WriteLog("Err connect SML");
                        }
                        else
                            flag7 = 0;

                        //hhhh
                        if (CheckProcess("SendSMSForMC") != 0)
                        {
                            flag6 = 1;
                        }
                        else
                            flag6 = 0;
                        ///// 

                        int count_txnMSG = CheckErr_TXNMSG();
                        if (count_txnMSG > 0)//check loi SMS thay doi du no 
                        {
                            SendSMSForTXNMSG(mobile, count_txnMSG);
                            SendSMSForTXNMSG(mobile2, count_txnMSG);
                            SendSMSForTXNMSG(mobile3, count_txnMSG);
                            SendSMSForTXNMSG(mobile7, count_txnMSG);
                            //SendSMSForTXNMSG(mobile8, count_txnMSG);
                            classCheckErrLogWriter.WriteLog("Err OutstandingChange!");
                            flag1 = 1;
                        }
                        else
                            flag1 = 0;

                        int count_txn_pay = CheckErr_Payment();
                        if (count_txn_pay > 0)//check loi SMS payment tai quay
                        {
                            SendSMSForManPay(mobile, count_txn_pay);
                            SendSMSForManPay(mobile2, count_txn_pay);
                            SendSMSForManPay(mobile3, count_txn_pay);
                            SendSMSForManPay(mobile7, count_txn_pay);
                            //SendSMSForManPay(mobile8, count_txn_pay);
                            classCheckErrLogWriter.WriteLog("Err SMS Payment!");
                            flag2 = 1;
                        }
                        else
                            flag2 = 0;


                        int txn_pending = Check_Pending_TXN_IPC();
                        //if (txn_pending > 10)// check loi IPC
                        if (txn_pending > 0)
                        {
                            SendSMSForTXN_PENDING(mobile, txn_pending);
                            SendSMSForTXN_PENDING(mobile2, txn_pending);
                            SendSMSForTXN_PENDING(mobile3, txn_pending);
                            //SendSMSForTXN_PENDING(mobile4, txn_pending);
                            SendSMSForTXN_PENDING(mobile7, txn_pending);
                            //SendSMSForTXN_PENDING(mobile8, txn_pending);
                            classCheckErrLogWriter.WriteLog("Err TXN pending at IPC!");
                            flag3 = 1;
                        }
                        else
                            flag3 = 0;

                        if (Check_Err_TXN_ISO() == 1)//check loi ISO
                        {
                            SendSMSForTXN_ISO(mobile);
                            SendSMSForTXN_ISO(mobile2);
                            SendSMSForTXN_ISO(mobile3);
                            //SendSMSForTXN_ISO(mobile4);
                            //SendSMSForTXN_ISO(mobile5);
                            SendSMSForTXN_ISO(mobile7);
                            //SendSMSForTXN_ISO(mobile8);
                            //SendSMSForTXN_ISO(mobile9);
                            //SendSMSForTXN_ISO(mobile10);
                            classCheckErrLogWriter.WriteLog("Err TXN ISO!");
                            flag4 = 1;
                        }
                        else
                            flag4 = 0;

                        if (Check_Err_TXN_WEB() == 1)//check loi WEB
                        {
                            SendSMSForTXN_WEB(mobile);
                            SendSMSForTXN_WEB(mobile2);
                            SendSMSForTXN_WEB(mobile3);
                            //SendSMSForTXN_WEB(mobile4);
                            //SendSMSForTXN_WEB(mobile5);
                            SendSMSForTXN_WEB(mobile7);
                            //SendSMSForTXN_WEB(mobile8);
                            //SendSMSForTXN_WEB(mobile9); 
                            //SendSMSForTXN_WEB(mobile10);
                            classCheckErrLogWriter.WriteLog("Err TXN WEB!");
                            flag5 = 1;
                        }
                        else
                            flag5 = 0;
                     
                        classCheckErrLogWriter.WriteLog("----------------End Process-----------------");
                      
                        //if (value > 2)
                        //{
                        //    classCheckErrLogWriter.WriteLog("sleep1 " + (value - (minute % value) - 1) + " minute");
                        //    Thread.Sleep(1000 * (value - (minute % value) - 1) * 55);// sleep (value - 1) phut de troi qua thoi gian lap lai
                        //    //flag = 1;
                        //}
                        //else
                        //{
                        //    classCheckErrLogWriter.WriteLog("sleep2 55s");
                        //    Thread.Sleep(1000 * 55);
                        //}

                        
                    }
                    //flag = flag1 + flag2 + flag3 + flag4 + flag5;

                    /*
                    //if (value > 2 && flag ==0) 
                    if (value > 2)                    
                    {
                        if ((value - (minute % value) - 1) > 0)
                        {
                            //classCheckErrLogWriter.WriteLog("sleep3 " + (value - (minute % value) - 1) + " minute");
                            Thread.Sleep(1000 * (value - (minute % value) - 1) * 55);
                        }
                        else
                        {
                            Thread.Sleep(1000 * 10);// truong hop start vao -1 ph
                        }
                    }
                    else
                    {
                        //classCheckErrLogWriter.WriteLog("sleep4 55s");
                        Thread.Sleep(1000 * 55);
                    }
                     */
                    classCheckErrLogWriter.WriteLog("sleep4 55s");
                    Thread.Sleep(1000 * 55);
                }
        }
        catch (Exception ex)
        {
            classCheckErrLogWriter.WriteLog("Error RunService(), " + ex.Message);
        }
    }

        //private static void Check_Space_Disk(string mobile, int num_row)
        //{
        //    try
        //    {
        //        ProcessStartInfo procStartInfo = new ProcessStartInfo("/bin/bash", "-c ls");
        //        procStartInfo.RedirectStandardOutput = true;
        //        procStartInfo.UseShellExecute = false;
        //        procStartInfo.CreateNoWindow = true;

        //        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        //        proc.StartInfo = procStartInfo;
        //        proc.Start();

        //        String result = proc.StandardOutput.ReadToEnd();


        //        classDataAccess ebankDataAccess = new classDataAccess();
        //        ebankDataAccess.OpenConnectionOracle("EBANK_GW");

        //        string SMS_TYPE = "SMSME";
        //        string message = "SMS OutstandingChange have " + num_row + " SMS not send " + DateTime.Now.ToString();

        //        ebankDataAccess.InsertSMSMessateToEBankGW(classDataAccess.IDALERT
        //                                                    , mobile //classUtilities.GetRandomMobile()
        //                                                    , message
        //                                                    , 'N'//Y: (se ko gui tin nhan),//N: se gui tin nhan
        //                                                    , SMS_TYPE);
        //        ebankDataAccess.CloseConnectionOracle();
        //    }
        //    catch (Exception ex)
        //    {
        //        classCheckErrLogWriter.WriteLog("Error SendSMSForSMS():" + ex.Message);
        //    }
        //}
        private static void SendSMS_With_Mess(string mobile, string message)
        {
            try
            {
                classDataAccess ebankDataAccess = new classDataAccess();
                ebankDataAccess.OpenConnectionOracle("EBANK_GW");

                string SMS_TYPE = "SMSME"; 
                ebankDataAccess.InsertSMSMessateToEBankGW(classDataAccess.IDALERT
                                                            , mobile //classUtilities.GetRandomMobile()
                                                            , message
                                                            , 'N'//Y: (se ko gui tin nhan),//N: se gui tin nhan
                                                            , SMS_TYPE);
                ebankDataAccess.CloseConnectionOracle();
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error SendSMS_With_Mess():" + ex.Message);
            }
        }
        private static void SendSMSForDEKM(string mobile, string data)
        {
            try
            {
                classDataAccess ebankDataAccess = new classDataAccess();
                ebankDataAccess.OpenConnectionOracle("EBANK_GW");

                string SMS_TYPE = "SMSME";
                string message = "Pls check DEKM service, can not decode num pan on DB "+data +" :" + DateTime.Now.ToString();

                ebankDataAccess.InsertSMSMessateToEBankGW(classDataAccess.IDALERT
                                                            , mobile //classUtilities.GetRandomMobile()
                                                            , message
                                                            , 'N'//Y: (se ko gui tin nhan),//N: se gui tin nhan
                                                            , SMS_TYPE);
                ebankDataAccess.CloseConnectionOracle();
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error SendSMSForDEKM():" + ex.Message);
            }
        }
        private static void SendSMSForTXNMSG(string mobile, int num_row)
        {
            try
            {
                classDataAccess ebankDataAccess = new classDataAccess();
                ebankDataAccess.OpenConnectionOracle("EBANK_GW");
                              
                string SMS_TYPE = "SMSME";
                string message = "SMS OutstandingChange have "+num_row +" SMS not send " + DateTime.Now.ToString();

                ebankDataAccess.InsertSMSMessateToEBankGW(classDataAccess.IDALERT
                                                            , mobile //classUtilities.GetRandomMobile()
                                                            , message
                                                            , 'N'//Y: (se ko gui tin nhan),//N: se gui tin nhan
                                                            , SMS_TYPE);
                ebankDataAccess.CloseConnectionOracle();
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error SendSMSForSMS():" + ex.Message);
            }
        }

        private static void SendSMSForManPay(string mobile, int num_row)
        {
            try
            {
                classDataAccess ebankDataAccess = new classDataAccess();
                ebankDataAccess.OpenConnectionOracle("EBANK_GW");

                string SMS_TYPE = "SMSME";
                string message = "SMS Outstanding Balance Payment Manual have " + num_row + " SMS not send " + DateTime.Now.ToString();

                ebankDataAccess.InsertSMSMessateToEBankGW(classDataAccess.IDALERT
                                                            , mobile //classUtilities.GetRandomMobile()
                                                            , message
                                                            , 'N'//Y: (se ko gui tin nhan),//N: se gui tin nhan
                                                            , SMS_TYPE);
                ebankDataAccess.CloseConnectionOracle();
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error SendSMSForManPay():" + ex.Message);
            }
        }

        private static void SendSMSForTXN_ISO(string mobile)
        {
            try
            {
                classDataAccess ebankDataAccess = new classDataAccess();
                ebankDataAccess.OpenConnectionOracle("EBANK_GW");

                string SMS_TYPE = "SMSISO";
                string message = "TXN Local Card on ISO have problem! " + DateTime.Now.ToString();

                ebankDataAccess.InsertSMSMessateToEBankGW(classDataAccess.IDALERT
                                                            , mobile //classUtilities.GetRandomMobile()
                                                            , message
                                                            , 'N'//Y: (se ko gui tin nhan),//N: se gui tin nhan
                                                            , SMS_TYPE);
                ebankDataAccess.CloseConnectionOracle();
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error SendSMSForTXN():" + ex.Message);
            }
        }
        private static void SendSMSForTXN_PENDING(string mobile,int p_count)
        {
            try
            {
                classDataAccess ebankDataAccess = new classDataAccess();
                ebankDataAccess.OpenConnectionOracle("EBANK_GW");

                string SMS_TYPE = "SMSPEN";
                string message = "IPC have problem! have " + p_count+ " TXN pending " + DateTime.Now.ToString();

                ebankDataAccess.InsertSMSMessateToEBankGW(classDataAccess.IDALERT
                                                            , mobile //classUtilities.GetRandomMobile()
                                                            , message
                                                            , 'N'//Y: (se ko gui tin nhan),//N: se gui tin nhan
                                                            , SMS_TYPE);
                ebankDataAccess.CloseConnectionOracle();
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error SendSMSForTXN():" + ex.Message);
            }
        }
        private static void SendSMSForConnect_SML(string mobile, int min)
        {
            try
            {
                classDataAccess ebankDataAccess = new classDataAccess();
                ebankDataAccess.OpenConnectionOracle("EBANK_GW");

                string SMS_TYPE = "SMSCON";
                string message = "Pls check connection to SML. Don't have transaction "+ min+ " min ago. " + DateTime.Now.ToString();

                ebankDataAccess.InsertSMSMessateToEBankGW(classDataAccess.IDALERT
                                                            , mobile //classUtilities.GetRandomMobile()
                                                            , message
                                                            , 'N'//Y: (se ko gui tin nhan),//N: se gui tin nhan
                                                            , SMS_TYPE);
                ebankDataAccess.CloseConnectionOracle();
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error SendSMSForConnect_SML():" + ex.Message);
            }
        }
        private static void SendSMSForTXN_WEB(string mobile)
        {
            try
            {
                classDataAccess ebankDataAccess = new classDataAccess();
                ebankDataAccess.OpenConnectionOracle("EBANK_GW");

                string SMS_TYPE = "SMSWEB";
                string message = "TXN Local Card on Web Service have problem! " + DateTime.Now.ToString();

                ebankDataAccess.InsertSMSMessateToEBankGW(classDataAccess.IDALERT
                                                            , mobile //classUtilities.GetRandomMobile()
                                                            , message
                                                            , 'N'//Y: (se ko gui tin nhan),//N: se gui tin nhan
                                                            , SMS_TYPE);
                ebankDataAccess.CloseConnectionOracle();
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error SendSMSForTXN_WEB():" + ex.Message);
            }
        }
        
        private static int Check_Pending_TXN_IPC()
        {
            try
            {
                string t_num_err = classUtilities.GetStringValueFromConfig("Err_IPC");
                int p_num_err = int.Parse(t_num_err);
                int num_err = _dataAccess.CHECK_TXN_IPC_PENDING();
                if (num_err > p_num_err)
                    return num_err;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Pending_TXN_IPC(), " + ex.Message);
                return 0;
            }
            
        }
        private static string Check_Start_Batch()
        {
            try
            {
                string sms_str = "";
                int flag=_dataAccess.Get_time_StartBatch();
                if (flag == 0)
                {
                    sms_str = "please start batch today!";
                }
                return sms_str;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Start_Batch(), " + ex.Message);
                return "";
            }

        }
        private static string Check_Expiry_User()
        {
            try
            {
                DataTable table_AM = new DataTable();
                DataTable table_IM = new DataTable();
                DataTable table_DW = new DataTable();
                table_AM.Rows.Clear();
                table_IM.Rows.Clear();
                table_DW.Rows.Clear();
                table_AM = _dataAccess.Get_Expiry_User_AM();
                table_IM = _dataAccess.Get_Expiry_User_IM();
                table_DW = _dataAccess.Get_Expiry_User_DW();
                string sms_str = "";
                if (table_AM.Rows.Count > 0)
                {

                    foreach (DataRow row in table_AM.Rows)
                    {
                        sms_str = sms_str + "User " + row.ItemArray[0].ToString()+" on AM" + " Expiry " + row.ItemArray[1].ToString()+ ",";
                        string log = "User: " + row.ItemArray[0].ToString() + " will expiry on " + row.ItemArray[1].ToString();
                        classCheckErrLogWriter.WriteLog("Error: " + sms_str);

                    }
                }
                if (table_DW.Rows.Count > 0)
                {

                    foreach (DataRow row in table_DW.Rows)
                    {
                        sms_str = sms_str + "User " + row.ItemArray[0].ToString() +" on DW"+ " Expiry " + row.ItemArray[1].ToString() + ",";
                        string log = "User: " + row.ItemArray[0].ToString() + " will expiry on " + row.ItemArray[1].ToString();
                        classCheckErrLogWriter.WriteLog("Error: " + sms_str);

                    }
                }
                if (table_IM.Rows.Count > 0 )
                {

                    foreach (DataRow row in table_IM.Rows)
                    {
                        sms_str = sms_str + "User " + row.ItemArray[0].ToString()+" on IM" + " Expiry " + row.ItemArray[1].ToString() + ",";
                        string log = "User: " + row.ItemArray[0].ToString() + " will expiry on " + row.ItemArray[1].ToString();
                        classCheckErrLogWriter.WriteLog("Error: " + sms_str);

                    }
                }

                return sms_str;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Expiry_User(), " + ex.Message);
                return "";
            }

        }
        private static string Check_Diff_Branch()
        {
            try
            {
                DataTable table = new DataTable();
                table.Rows.Clear();
                table = _dataAccess.Get_Diff_Branch();
                string sms_str = "";
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        sms_str = sms_str + "Branch on IR025 or IR275 and OA051 is difference. Please check!";                     
                      
                        string log = "LOC: " + row.ItemArray[1].ToString() + ". Branch on FE: " + row.ItemArray[3].ToString() + ". Branch on SWT: " + row.ItemArray[2].ToString();
                        classCheckErrLogWriter.WriteLog(log);

                    }
                }


                return sms_str;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Diff_Branch(), " + ex.Message);
                return "";
            }

        }
        private static string  Check_Diff_MSL()
        {
            try
            {              
                DataTable table = new DataTable();
                table.Rows.Clear();
                table = _dataAccess.Get_Diff_MSL();
                string sms_str = "";
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        sms_str = sms_str +"MSL on IR056,OA052 is difference. Pls check cif: "
                        + row.ItemArray[0].ToString()+", ";                       
                        //update
                        string log = "CIF: " + row.ItemArray[0].ToString() + ". MSL on FE: " + row.ItemArray[2].ToString()+". MSL on SWT: " + row.ItemArray[3].ToString();
                        classCheckErrLogWriter.WriteLog(log);
                       
                    }
                }


                return sms_str;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Diff_SML(), " + ex.Message);
                return "";
            }

        }

        private static int Check_Err_TXN_ISO()
        {
            try
            {
                string t_num_err = classUtilities.GetStringValueFromConfig("Err_ISO");
                int num_err = int.Parse(t_num_err);
                string t_num_suc = classUtilities.GetStringValueFromConfig("Suc_ISO");
                int num_suc = int.Parse(t_num_suc);
                int count = 0;
                int count_num = 0;
                DataTable table = new DataTable();
                table.Rows.Clear();
                table = _dataAccess.Get_Inf_TXN_ISO();
                string year_max = table.Rows[0].ItemArray[4].ToString().Substring(0, 4);
                string month_max = table.Rows[0].ItemArray[4].ToString().Substring(4, 2);
                string day_max = table.Rows[0].ItemArray[4].ToString().Substring(6, 2);
                string h_max = table.Rows[0].ItemArray[4].ToString().Substring(8, 2);
                string m_max = table.Rows[0].ItemArray[4].ToString().Substring(10, 2);
                string s_max = table.Rows[0].ItemArray[4].ToString().Substring(12, 2);
                string str_date_max = day_max + "/" + month_max + "/" + year_max + " " + h_max + ":" + m_max + ":" + s_max;
                DateTime date_max = Convert.ToDateTime(str_date_max);
                DateTime date_now = System.DateTime.Now;
                TimeSpan Time = date_now - date_max;
                int time_late = Convert.ToInt32(Time.TotalMinutes);                
                string min_iso = classUtilities.GetStringValueFromConfig("Min_ISO");
                foreach (DataRow row in table.Rows)
                {
                    
                    if (row.ItemArray[2].ToString() == "96" && (row.ItemArray[3].ToString().Trim() == "" || row.ItemArray[3].ToString().Trim() == "24"))
                        count_num++;
                    if (row.ItemArray[2].ToString() == "00")
                         count++;
                }

                //if (count > 9)
                //if (count_num > 7 &&  count < 1)
                if (count_num > num_err && count < num_suc && (time_late < int.Parse(min_iso)))
                    return 1;
                else
                    return 0;
                

            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Err_TXN_ISO(), " + ex.Message);
                return 0;
            }

        }
        private static int Check_Err_DEKM_IM()
        {
            try
            {      
                             
               int flag = _dataAccess.Check_Inf_DEKM_IM();      

               return flag;


            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Err_DEKM_IM(), " + ex.Message);
                return 0;
            }

        }
        private static int Check_Err_DEKM_AM()
        {
            try
            {

                int flag = _dataAccess.Check_Inf_DEKM_AM();

                return flag;


            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Inf_DEKM_AM(), " + ex.Message);
                return 0;
            }

        }

        private static int Check_Err_DEKM_DW()
        {
            try
            {

                int flag = _dataAccess.Check_Err_DEKM_DW();

                return flag;


            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Err_DEKM_DW(), " + ex.Message);
                return 0;
            }

        }
        private static int Check_Connect_SML()
        {
            try
            {
               
                DataTable table = new DataTable();
                table.Rows.Clear();
                table = _dataAccess.Get_Inf_TXN_SML();
                if (table.Rows.Count > 0)
                {
                    string year_max = table.Rows[0].ItemArray[4].ToString().Substring(0, 4);
                    string month_max = table.Rows[0].ItemArray[4].ToString().Substring(4, 2);
                    string day_max = table.Rows[0].ItemArray[4].ToString().Substring(6, 2);
                    string h_max = table.Rows[0].ItemArray[4].ToString().Substring(8, 2);
                    string m_max = table.Rows[0].ItemArray[4].ToString().Substring(10, 2);
                    string s_max = table.Rows[0].ItemArray[4].ToString().Substring(12, 2);
                    string str_date_max = day_max + "/" + month_max + "/" + year_max + " " + h_max + ":" + m_max + ":" + s_max;
                    DateTime date_max = Convert.ToDateTime(str_date_max);
                    DateTime date_now = System.DateTime.Now;
                    TimeSpan Time = date_now - date_max;
                    int time_late = Convert.ToInt32(Time.TotalMinutes);
                    string begin_h = classUtilities.GetStringValueFromConfig("Be_SML");
                    string end_h = classUtilities.GetStringValueFromConfig("En_SML");
                    string min_sml = classUtilities.GetStringValueFromConfig("Min_SML");
                    DateTime dateTime = DateTime.Now;
                    string str_hour = dateTime.ToString("HH");//24 hours format

                    if (int.Parse(str_hour) >= int.Parse(begin_h) && int.Parse(str_hour) < int.Parse(end_h))
                    {
                        if (time_late > int.Parse(min_sml))
                            return time_late;

                    }


                    //string h_now = System.DateTime.Now.Hour.ToString();
                    //string m_now = System.DateTime.Now.Minute.ToString();
                    //string s_now = System.DateTime.Now.Second.ToString();
                    //int s_sum = ((int.Parse(s_now) - int.Parse(s_max)) + (int.Parse(m_now) - int.Parse(m_max)) * 60 + (int.Parse(h_now) - int.Parse(h_max)) * 3600);

                    //string begin_h = classUtilities.GetStringValueFromConfig("Be_SML");
                    //string end_h = classUtilities.GetStringValueFromConfig("En_SML");
                    //string min_sml = classUtilities.GetStringValueFromConfig("Min_SML");
                    //if (int.Parse(System.DateTime.Now.Hour.ToString()) >= int.Parse(begin_h) && int.Parse(System.DateTime.Now.Hour.ToString()) <= int.Parse(end_h))
                    //{
                    //    if (s_sum > (int.Parse(min_sml) * 60))
                    //        if (s_sum % 60 > 30)
                    //            return (s_sum / 60) + 1;
                    //        else
                    //            return (s_sum / 60);
                    //}
                }
               
                return 0;

            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Connect_SML(), " + ex.Message);
                return 0;
            }

        }

        private static int Check_Info_MSL()
        {
            try
            {
                string t_num_err = classUtilities.GetStringValueFromConfig("Err_WEB");
                int num_err = int.Parse(t_num_err);
                string t_num_suc = classUtilities.GetStringValueFromConfig("Suc_WEB");
                int num_suc = int.Parse(t_num_suc);
                int count = 0;
                int count_num = 0;
                DataTable table = new DataTable();
                table.Rows.Clear();               
                table = _dataAccess.Get_Inf_TXN_WEB();
                string year_max = table.Rows[0].ItemArray[4].ToString().Substring(0, 4);
                string month_max = table.Rows[0].ItemArray[4].ToString().Substring(4, 2);
                string day_max = table.Rows[0].ItemArray[4].ToString().Substring(6, 2);
                string h_max = table.Rows[0].ItemArray[4].ToString().Substring(8, 2);
                string m_max = table.Rows[0].ItemArray[4].ToString().Substring(10, 2);
                string s_max = table.Rows[0].ItemArray[4].ToString().Substring(12, 2);
                string str_date_max = day_max + "/" + month_max + "/" + year_max + " " + h_max + ":" + m_max + ":" + s_max;
                DateTime date_max = Convert.ToDateTime(str_date_max);
                DateTime date_now = System.DateTime.Now;
                TimeSpan Time = date_now - date_max;
                int time_late = Convert.ToInt32(Time.TotalMinutes);
                string min_web = classUtilities.GetStringValueFromConfig("Min_WEB");
                
                foreach (DataRow row in table.Rows)
                {
                    if (row.ItemArray[2].ToString() == "96" && (row.ItemArray[3].ToString().Trim() == "24" || row.ItemArray[3].ToString().Trim() == ""))
                        count_num++;
                    if (row.ItemArray[2].ToString() == "00")
                        count++;
                }
                if (count_num > num_err && count < num_suc && (time_late < int.Parse(min_web)))
                    return 1;
                else
                    return 0;
               
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Err_TXN_WEB(), " + ex.Message);
                return 0;
            }

        }

        private static int Check_Err_TXN_WEB()
        {
            try
            {
                string t_num_err = classUtilities.GetStringValueFromConfig("Err_WEB");
                int num_err = int.Parse(t_num_err);
                string t_num_suc = classUtilities.GetStringValueFromConfig("Suc_WEB");
                int num_suc = int.Parse(t_num_suc);
                int count = 0;
                int count_num = 0;
                DataTable table = new DataTable();
                table.Rows.Clear();               
                table = _dataAccess.Get_Inf_TXN_WEB();
                string year_max = table.Rows[0].ItemArray[4].ToString().Substring(0, 4);
                string month_max = table.Rows[0].ItemArray[4].ToString().Substring(4, 2);
                string day_max = table.Rows[0].ItemArray[4].ToString().Substring(6, 2);
                string h_max = table.Rows[0].ItemArray[4].ToString().Substring(8, 2);
                string m_max = table.Rows[0].ItemArray[4].ToString().Substring(10, 2);
                string s_max = table.Rows[0].ItemArray[4].ToString().Substring(12, 2);
                string str_date_max = day_max + "/" + month_max + "/" + year_max + " " + h_max + ":" + m_max + ":" + s_max;
                DateTime date_max = Convert.ToDateTime(str_date_max);
                DateTime date_now = System.DateTime.Now;
                TimeSpan Time = date_now - date_max;
                int time_late = Convert.ToInt32(Time.TotalMinutes);
                string min_web = classUtilities.GetStringValueFromConfig("Min_WEB");
                
                foreach (DataRow row in table.Rows)
                {
                    if (row.ItemArray[2].ToString() == "96" && (row.ItemArray[3].ToString().Trim() == "24" || row.ItemArray[3].ToString().Trim() == ""))
                        count_num++;
                    if (row.ItemArray[2].ToString() == "00")
                        count++;
                }
                if (count_num > num_err && count < num_suc && (time_late < int.Parse(min_web)))
                    return 1;
                else
                    return 0;
               
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Err_TXN_WEB(), " + ex.Message);
                return 0;
            }

        }

      
        private static int CheckErr_TXNMSG()
        {
           
            try
            {
                DataTable table = new DataTable();
                string t_num_err = classUtilities.GetStringValueFromConfig("Err_TXNMSG");
                int num_err = int.Parse(t_num_err);

                string maxUpdateDT = null;
                table.Rows.Clear();
                if (string.IsNullOrEmpty(_updateDateTime) == false)
                {
                    maxUpdateDT = _updateDateTime;
                    _updateDateTime = null;
                }
                else
                {
                    //maxUpdateDT = _dataAccess.Get_Max_UpdateTime(SMS_TYPE1);
                    maxUpdateDT = _dataAccess.Get_Max_UpdateTimeErr(SMS_TYPE1);
                    /*
                    DataTable updateTime = _dataAccess.Get_Max_OutBal_UpdateTime(SMS_TYPE);
                    if (updateTime.Rows.Count == 1 || updateTime.Rows.Count >= 2)
                        maxUpdateDT = updateTime.Rows[0].ItemArray[0].ToString();
                     */
                }
                //if (string.IsNullOrEmpty(maxUpdateDT) == false)
                //{
                   
                //    long maxUpdateTime = long.Parse(maxUpdateDT);
                //}//hoand rem 02102015

                    if(maxUpdateDT != null)
                        table = _dataAccess.GetOutstandingChange_Err(maxUpdateDT);

                    if (table.Rows.Count > num_err)//check loi SMS thay doi du no 
                        return table.Rows.Count;
                    else
                        return 0;
                    
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_CheckErr_TXNMSG(), " + ex.Message);                
                return 0;
            }
           
        }


        private static int CheckErr_Payment()
        {
            
            try
            {
                DataTable table = new DataTable();
                string t_num_err = classUtilities.GetStringValueFromConfig("Err_Payment");
                int num_err = int.Parse(t_num_err);
                string maxUpdateDT = null;                
                table.Rows.Clear();
                if (string.IsNullOrEmpty(_updateDateTime) == false)
                {
                    maxUpdateDT = _updateDateTime;
                    _updateDateTime = null;
                }
                else
                {
                    //maxUpdateDT = _dataAccess.Get_Max_UpdateTime(SMS_TYPE2);
                    maxUpdateDT = _dataAccess.Get_Max_UpdateTimeErr(SMS_TYPE2);
                    
                    /*
                    DataTable updateTime = _dataAccess.Get_Max_OutBal_UpdateTime(SMS_TYPE);
                    if (updateTime.Rows.Count == 1 || updateTime.Rows.Count >= 2)
                        maxUpdateDT = updateTime.Rows[0].ItemArray[0].ToString();
                     */
                }
                if (string.IsNullOrEmpty(maxUpdateDT) == false)
                {

                    long maxUpdateTime = long.Parse(maxUpdateDT);
                }
                if (maxUpdateDT != null)
                {
                    //table = _dataAccess.GetPaymentManual(maxUpdateDT);
                    table = _dataAccess.GetPaymentManualErr(maxUpdateDT);
                }
                if (table.Rows.Count > num_err)//check loi SMS payment tai quay
                    return table.Rows.Count;
                else
                    return 0;
                
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_CheckErr_Paymen(), " + ex.Message);                
                return 0;
            }

        }
    }
    
}
