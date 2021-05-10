using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Data;
using System.Data.OracleClient;
using System.Globalization;
using System.Threading;

namespace UPDATE_TERMINAL
{
    class classDisableCase
    {
        public static bool _exitThread = false;
        public static string _updateDateTime = null;
        private static classDataAccess _dataAccess = new classDataAccess();
        //private static List<string[]> _currencyMapping = new List<string[]>();
        //private static List<string[]> _specialCardList = new List<string[]>();
        //private static string DEFAULT_CRNCY_ALPA = "MTT";
        public static string SMS_TYPE = "DIS_CA";
        public static string SMS_TYPE_SN = "DIS_SN";//S-care new
        public static string SMS_TYPE_SC = "DIS_SC";//S-care change
        public static string SMS_TYPE_PN = "DIS_PN";//Premier new

        private static string SCBPhone = "";

        public static void RunService()
        {
            try
            {
                int minute = 0;
                int hour = 0;
                int value = 0;
                int sleep = 0;
                DataTable table = new DataTable();
                DataTable tableSCare_N = new DataTable();
                DataTable tableSCare_C = new DataTable();
                DataTable tablePremier_N = new DataTable();
                SCBPhone = classUtilities.GetStringValueFromConfig("SCB_Contact_Phone");
                value = classUtilities.GetIntValueFromConfig("Disable_Case_Minute");
                sleep = classUtilities.GetIntValueFromConfig("Disable_Case_Second");
                while (_exitThread == false)
                {
                    minute = DateTime.Now.Minute;
                    if (minute % value == 0)
                    {

                        classDisableCaseLogWriter.WriteLog("----------------Begin Process-----------------");                       

                        _dataAccess = new classDataAccess();
                        table.Rows.Clear();
                        table = Get_DisableCase();
                        if (table.Rows.Count > 0)
                        {
                            Process_Disable_Case(table);
                        }
                        ///////////
                        tablePremier_N.Rows.Clear();
                        tablePremier_N = Get_DisableCase_Premier_New();
                        if (tablePremier_N.Rows.Count > 0)
                        {
                            Process_Disable_Case_Premier_New(tablePremier_N);
                        }
                        ////////////
                        //tableSCare_C.Rows.Clear();
                        //tableSCare_C = Get_DisableCase_SCare_Change();
                        //if (tableSCare_C.Rows.Count > 0)
                        //{
                        //    Process_Disable_Case_SCare_Change(tableSCare_C);
                        //}
                        /////////////
                        tableSCare_N.Rows.Clear();
                        tableSCare_N = Get_DisableCase_SCare_New();
                        if (tableSCare_N.Rows.Count > 0)
                        {
                            Process_Disable_Case_SCare_New(tableSCare_N);
                        }                     

                        classDisableCaseLogWriter.WriteLog("----------------End Process----------------- at: " + DateTime.Now.ToString());
                        Thread.Sleep(1000 * sleep);

                    }

                    if (value > 2)
                    {
                        if ((value - (minute % value) - 1) > 0)
                        {
                            classDisableCaseLogWriter.WriteLog("sleep " + (value - (minute % value) - 1) + " minute");
                            Thread.Sleep(1000 * (value - (minute % value) - 1) * 55);
                        }
                        else
                        {
                            Thread.Sleep(1000 * 10);// truong hop start vao -1 ph
                        }
                    }
                    else
                        Thread.Sleep(1000 * sleep); // 10 giay moi check 1 lan



                }



            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error RunService(), " + ex.Message);
            }
        }
        private static void Process_Disable_Case_SCare_Change(DataTable table)
        {
            try
            {
                string from_email = classUtilities.GetStringValueFromConfig("from_email");
                string pass_email = classUtilities.GetStringValueFromConfig("pass_email");
                string ad_email = classUtilities.GetStringValueFromConfig("ad_email");


                foreach (DataRow row in table.Rows)
                {



                    string create_time_email = row.ItemArray[11].ToString().Substring(8, 2) + ":" + row.ItemArray[11].ToString().Substring(10, 2) + " " + row.ItemArray[11].ToString().Substring(6, 2) + "/" + row.ItemArray[11].ToString().Substring(4, 2) + "/" + row.ItemArray[11].ToString().Substring(0, 4);
                    int flag = _dataAccess.Insert_Disable_Case(SMS_TYPE_SC, "no_data", "no_data", DateTime.Today, row.ItemArray[4].ToString().Substring(0,19), row.ItemArray[11].ToString());

                    if (flag > 0)
                    {
                        if (row.ItemArray[8].ToString() == "A")//da approval
                        {
                            send_SCB_mail_approval_cardtype_SCare(row.ItemArray[7].ToString()
                            , from_email, pass_email, row.ItemArray[5].ToString()
                            , row.ItemArray[0].ToString(), row.ItemArray[4].ToString(), row.ItemArray[3].ToString(),
                            create_time_email, ad_email, row.ItemArray[13].ToString());

                        }
                        else
                        {
                            Disable_Case_SCare_Change(row.ItemArray[3].ToString(), row.ItemArray[4].ToString(), row.ItemArray[11].ToString(), row.ItemArray[0].ToString(), row.ItemArray[4].ToString().Substring(0,19));
                            send_SCB_mail_cancel_cardtype_SCare(row.ItemArray[1].ToString(), from_email, pass_email, row.ItemArray[0].ToString()
                            , row.ItemArray[4].ToString(), row.ItemArray[3].ToString(), create_time_email, ad_email, row.ItemArray[13].ToString());

                        }
                    }


                }

            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Process_Disable_Case_SCare(), " + ex.Message);
            }

        }
        private static void Process_Disable_Case_Premier_New(DataTable table)
        {
            try
            {
                string from_email = classUtilities.GetStringValueFromConfig("from_email");
                string pass_email = classUtilities.GetStringValueFromConfig("pass_email");
                string ad_email = classUtilities.GetStringValueFromConfig("ad_email");
                string TD_email = classUtilities.GetStringValueFromConfig("TD_email");
                string TD_email2 = classUtilities.GetStringValueFromConfig("TD_email2");  

                foreach (DataRow row in table.Rows)
                {



                    string create_time_email = row.ItemArray[11].ToString().Substring(8, 2) + ":" + row.ItemArray[11].ToString().Substring(10, 2) + " " + row.ItemArray[11].ToString().Substring(6, 2) + "/" + row.ItemArray[11].ToString().Substring(4, 2) + "/" + row.ItemArray[11].ToString().Substring(0, 4);
                    int flag = _dataAccess.Insert_Disable_Case(SMS_TYPE_PN, "no_data", "no_data", DateTime.Today, row.ItemArray[4].ToString(), row.ItemArray[11].ToString());

                    if (flag > 0)
                    {
                        if (row.ItemArray[8].ToString() == "A")//da approval
                        {
                            send_SCB_mail_approval_cardtype_Premier(row.ItemArray[7].ToString()
                            , from_email, pass_email, row.ItemArray[5].ToString()
                            , row.ItemArray[0].ToString(), row.ItemArray[4].ToString(), row.ItemArray[3].ToString(),
                            create_time_email, ad_email, row.ItemArray[13].ToString(), TD_email, TD_email2);

                        }
                        else
                        {
                            Disable_Case_Premier_New(row.ItemArray[3].ToString(), row.ItemArray[4].ToString(), row.ItemArray[11].ToString(), row.ItemArray[0].ToString());
                            send_SCB_mail_cancel_cardtype_Premier(row.ItemArray[1].ToString(), from_email, pass_email, row.ItemArray[0].ToString()
                            , row.ItemArray[4].ToString(), row.ItemArray[3].ToString(), create_time_email, ad_email, row.ItemArray[13].ToString(), TD_email);

                        }
                    }


                }

            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Process_Disable_Case_Premier_New(), " + ex.Message);
            }

        }

        private static void Process_Disable_Case_SCare_New(DataTable table)
        {
            try
            {
                string from_email = classUtilities.GetStringValueFromConfig("from_email");
                string pass_email = classUtilities.GetStringValueFromConfig("pass_email");
                string ad_email = classUtilities.GetStringValueFromConfig("ad_email");             


                foreach (DataRow row in table.Rows)
                {

                 
                   
                        string create_time_email = row.ItemArray[11].ToString().Substring(8, 2) + ":" + row.ItemArray[11].ToString().Substring(10, 2) + " " + row.ItemArray[11].ToString().Substring(6, 2) + "/" + row.ItemArray[11].ToString().Substring(4, 2) + "/" + row.ItemArray[11].ToString().Substring(0, 4);
                        int flag = _dataAccess.Insert_Disable_Case(SMS_TYPE_PN, "no_data", "no_data", DateTime.Today, row.ItemArray[4].ToString(), row.ItemArray[11].ToString());
                        
                        if (flag > 0)
                        {
                            if (row.ItemArray[8].ToString() == "A")//da approval
                            {
                                send_SCB_mail_approval_cardtype_SCare(row.ItemArray[7].ToString()
                                , from_email, pass_email, row.ItemArray[5].ToString()
                                , row.ItemArray[0].ToString(), row.ItemArray[4].ToString(), row.ItemArray[3].ToString(),
                                create_time_email, ad_email, row.ItemArray[13].ToString());

                            }
                            else
                            {
                                Disable_Case_SCare_New(row.ItemArray[3].ToString(), row.ItemArray[4].ToString(), row.ItemArray[11].ToString(),row.ItemArray[0].ToString());
                                send_SCB_mail_cancel_cardtype_SCare(row.ItemArray[1].ToString(), from_email, pass_email, row.ItemArray[0].ToString()
                                , row.ItemArray[4].ToString(),row.ItemArray[3].ToString(), create_time_email, ad_email,row.ItemArray[13].ToString());

                            }
                        }
                  
                   
                }

            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Process_Disable_Case_SCare(), " + ex.Message);
            }

        }
        private static void Process_Disable_Case(DataTable table)
        {
            try
            {
                string from_email = classUtilities.GetStringValueFromConfig("from_email");
                string pass_email = classUtilities.GetStringValueFromConfig("pass_email");              
                string ad_email = classUtilities.GetStringValueFromConfig("ad_email");
                string ope_email1 = classUtilities.GetStringValueFromConfig("Ope_email1");
                string ope_email2 = classUtilities.GetStringValueFromConfig("Ope_email2");
                string ope_email3 = classUtilities.GetStringValueFromConfig("Ope_email3");
                string ope_email4 = classUtilities.GetStringValueFromConfig("ope_email4");
                string ope_email5 = classUtilities.GetStringValueFromConfig("ope_email5");
                
                
                foreach (DataRow row in table.Rows)
                {
                   
                    if (row.ItemArray[5].ToString().Substring(0, 6) == "550796") //the debit
                    {
                        string create_time_email = row.ItemArray[13].ToString().Substring(8, 2) + ":" + row.ItemArray[13].ToString().Substring(10, 2) + " " + row.ItemArray[13].ToString().Substring(6, 2) + "/" + row.ItemArray[13].ToString().Substring(4, 2) + "/" + row.ItemArray[13].ToString().Substring(0, 4);
                        int flag = _dataAccess.Insert_Disable_Case(SMS_TYPE, "no_data", "no_data", DateTime.Today, row.ItemArray[15].ToString(), row.ItemArray[13].ToString());
                        //string create_time = row.ItemArray[13].ToString();
                        if (flag > 0)
                        {
                            if (row.ItemArray[10].ToString() == "A")//da approval
                            {
                                send_SCB_mail_approval_cardtype(row.ItemArray[9].ToString()
                                , from_email, pass_email, row.ItemArray[7].ToString()
                                , row.ItemArray[0].ToString(), row.ItemArray[4].ToString(), row.ItemArray[5].ToString(),
                                row.ItemArray[3].ToString(), row.ItemArray[1].ToString(), row.ItemArray[15].ToString(), create_time_email, ad_email, ope_email1, ope_email2, row.ItemArray[16].ToString(), ope_email3, ope_email4, ope_email5);

                            }
                            else
                            {
                                Disable_Case(row.ItemArray[3].ToString(), row.ItemArray[4].ToString(), row.ItemArray[15].ToString(), row.ItemArray[13].ToString(), row.ItemArray[0].ToString());
                                send_SCB_mail_cancel_cardtype(row.ItemArray[1].ToString(), from_email, pass_email, row.ItemArray[0].ToString()
                                , row.ItemArray[4].ToString(), row.ItemArray[5].ToString(), row.ItemArray[3].ToString(), row.ItemArray[15].ToString(), create_time_email, ad_email, ope_email1, ope_email2, ope_email3, ope_email4, row.ItemArray[16].ToString(), ope_email5);

                            }
                        }
                    }
                    else //the tin dung
                    {
                        //khong dung chi nhanh tao the
                        if (row.ItemArray[2].ToString() != row.ItemArray[6].ToString() && (row.ItemArray[3].ToString() == "Card Renewal" || row.ItemArray[3].ToString() == "Card Upgrade/Downgrade" || row.ItemArray[3].ToString() == "Special Limit"))
                        {
                            string create_time_email = row.ItemArray[13].ToString().Substring(8, 2) + ":" + row.ItemArray[13].ToString().Substring(10, 2) + " " + row.ItemArray[13].ToString().Substring(6, 2) + "/" + row.ItemArray[13].ToString().Substring(4, 2) + "/" + row.ItemArray[13].ToString().Substring(0, 4);
                            int flag = _dataAccess.Insert_Disable_Case(SMS_TYPE, "no_data", "no_data", DateTime.Today, row.ItemArray[15].ToString(), row.ItemArray[13].ToString());
                            //string create_time = row.ItemArray[13].ToString();
                            if (flag > 0)
                            {
                                if (row.ItemArray[10].ToString() == "A")//da approval
                                {
                                    send_SCB_mail_approval(row.ItemArray[9].ToString()
                                    , from_email, pass_email, row.ItemArray[7].ToString()
                                    , row.ItemArray[0].ToString(), row.ItemArray[4].ToString(), row.ItemArray[5].ToString(),
                                    row.ItemArray[3].ToString(), row.ItemArray[1].ToString(), row.ItemArray[15].ToString(), create_time_email, ad_email, ope_email1, ope_email2, ope_email3, ope_email4, ope_email5);

                                }
                                else
                                {
                                    Disable_Case(row.ItemArray[3].ToString(), row.ItemArray[4].ToString(), row.ItemArray[15].ToString(), row.ItemArray[13].ToString(), row.ItemArray[0].ToString());
                                    send_SCB_mail_cancel(row.ItemArray[1].ToString(), from_email, pass_email, row.ItemArray[0].ToString()
                                    , row.ItemArray[4].ToString(), row.ItemArray[5].ToString(), row.ItemArray[3].ToString(), row.ItemArray[15].ToString(), create_time_email, ad_email, ope_email1, ope_email2, ope_email3, ope_email4, ope_email5);

                                }
                            }
                        }
                        else //sai card type
                        {
                            string create_time_email = row.ItemArray[13].ToString().Substring(8, 2) + ":" + row.ItemArray[13].ToString().Substring(10, 2) + " " + row.ItemArray[13].ToString().Substring(6, 2) + "/" + row.ItemArray[13].ToString().Substring(4, 2) + "/" + row.ItemArray[13].ToString().Substring(0, 4);
                            int flag = _dataAccess.Insert_Disable_Case(SMS_TYPE, "no_data", "no_data", DateTime.Today, row.ItemArray[15].ToString(), row.ItemArray[13].ToString());
                            //string create_time = row.ItemArray[13].ToString();
                            if (flag > 0)
                            {
                                if (row.ItemArray[10].ToString() == "A")//da approval
                                {
                                    send_SCB_mail_approval_cardtype(row.ItemArray[9].ToString()
                                    , from_email, pass_email, row.ItemArray[7].ToString()
                                    , row.ItemArray[0].ToString(), row.ItemArray[4].ToString(), row.ItemArray[5].ToString(),
                                    row.ItemArray[3].ToString(), row.ItemArray[1].ToString(), row.ItemArray[15].ToString(), create_time_email, ad_email, ope_email1, ope_email2, row.ItemArray[16].ToString(), ope_email3, ope_email4, ope_email5);

                                }
                                else
                                {
                                    Disable_Case(row.ItemArray[3].ToString(), row.ItemArray[4].ToString(), row.ItemArray[15].ToString(), row.ItemArray[13].ToString(), row.ItemArray[0].ToString());
                                    send_SCB_mail_cancel_cardtype(row.ItemArray[1].ToString(), from_email, pass_email, row.ItemArray[0].ToString()
                                    , row.ItemArray[4].ToString(), row.ItemArray[5].ToString(), row.ItemArray[3].ToString(), row.ItemArray[15].ToString(), create_time_email, ad_email, ope_email1, ope_email2, ope_email3, ope_email4, row.ItemArray[16].ToString(), ope_email5);

                                }
                            }
                        }
                        //if (row.ItemArray[2].ToString() == row.ItemArray[6].ToString())//dung ma chi nhanh => sai card type
                        //{
                        //    string create_time_email = row.ItemArray[13].ToString().Substring(8, 2) + ":" + row.ItemArray[13].ToString().Substring(10, 2) + " " + row.ItemArray[13].ToString().Substring(6, 2) + "/" + row.ItemArray[13].ToString().Substring(4, 2) + "/" + row.ItemArray[13].ToString().Substring(0, 4);
                        //    int flag = _dataAccess.Insert_Disable_Case(SMS_TYPE, "no_data", "no_data", DateTime.Today, row.ItemArray[15].ToString(), row.ItemArray[13].ToString());
                        //    //string create_time = row.ItemArray[13].ToString();
                        //    if (flag > 0)
                        //    {
                        //        if (row.ItemArray[10].ToString() == "A")//da approval
                        //        {
                        //            send_SCB_mail_approval_cardtype(row.ItemArray[9].ToString()
                        //            , from_email, pass_email, row.ItemArray[7].ToString()
                        //            , row.ItemArray[0].ToString(), row.ItemArray[4].ToString(), row.ItemArray[5].ToString(),
                        //            row.ItemArray[3].ToString(), row.ItemArray[1].ToString(), row.ItemArray[15].ToString(), create_time_email, ad_email, ope_email1, ope_email2, row.ItemArray[16].ToString(), ope_email3, ope_email4, ope_email5);

                        //        }
                        //        else
                        //        {
                        //            Disable_Case(row.ItemArray[3].ToString(), row.ItemArray[4].ToString(), row.ItemArray[15].ToString(), row.ItemArray[13].ToString(), row.ItemArray[0].ToString());
                        //            send_SCB_mail_cancel_cardtype(row.ItemArray[1].ToString(), from_email, pass_email, row.ItemArray[0].ToString()
                        //            , row.ItemArray[4].ToString(), row.ItemArray[5].ToString(), row.ItemArray[3].ToString(), row.ItemArray[15].ToString(), create_time_email, ad_email, ope_email1, ope_email2, ope_email3, ope_email4, row.ItemArray[16].ToString(), ope_email5);

                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    string create_time_email = row.ItemArray[13].ToString().Substring(8, 2) + ":" + row.ItemArray[13].ToString().Substring(10, 2) + " " + row.ItemArray[13].ToString().Substring(6, 2) + "/" + row.ItemArray[13].ToString().Substring(4, 2) + "/" + row.ItemArray[13].ToString().Substring(0, 4);
                        //    int flag = _dataAccess.Insert_Disable_Case(SMS_TYPE, "no_data", "no_data", DateTime.Today, row.ItemArray[15].ToString(), row.ItemArray[13].ToString());
                        //    //string create_time = row.ItemArray[13].ToString();
                        //    if (flag > 0)
                        //    {
                        //        if (row.ItemArray[10].ToString() == "A")//da approval
                        //        {
                        //            send_SCB_mail_approval(row.ItemArray[9].ToString()
                        //            , from_email, pass_email, row.ItemArray[7].ToString()
                        //            , row.ItemArray[0].ToString(), row.ItemArray[4].ToString(), row.ItemArray[5].ToString(),
                        //            row.ItemArray[3].ToString(), row.ItemArray[1].ToString(), row.ItemArray[15].ToString(), create_time_email, ad_email, ope_email1, ope_email2, ope_email3, ope_email4, ope_email5);

                        //        }
                        //        else
                        //        {
                        //            Disable_Case(row.ItemArray[3].ToString(), row.ItemArray[4].ToString(), row.ItemArray[15].ToString(), row.ItemArray[13].ToString(), row.ItemArray[0].ToString());
                        //            send_SCB_mail_cancel(row.ItemArray[1].ToString(), from_email, pass_email, row.ItemArray[0].ToString()
                        //            , row.ItemArray[4].ToString(), row.ItemArray[5].ToString(), row.ItemArray[3].ToString(), row.ItemArray[15].ToString(), create_time_email, ad_email, ope_email1, ope_email2, ope_email3, ope_email4, ope_email5);

                        //        }
                        //    }
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Get_DisableCase(), " + ex.Message);
            }

        }
        public static DataTable Get_DisableCase_SCare_Change()
        {
            string maxUpdateDT = null;
            DataTable table = new DataTable();
            if (string.IsNullOrEmpty(_updateDateTime) == false)
            {
                maxUpdateDT = _updateDateTime;
                _updateDateTime = null;
            }
            else
            {
                maxUpdateDT = _dataAccess.Get_Max_UpdateTime_Dis(SMS_TYPE_SC);

            }
            if (string.IsNullOrEmpty(maxUpdateDT) == false)
            {
                try
                {
                    long MaxUpdateTime = long.Parse(maxUpdateDT);
                }
                catch (Exception ex)
                {
                    classDisableCaseLogWriter.WriteLog("Error Get_DisableCase_SCare_Change(), " + ex.Message);
                    return null;
                }
                if (maxUpdateDT != null)
                    table = _dataAccess.GetDisableCase_SCare_Change(maxUpdateDT);
            }
            //////////
            ///table = _dataAccess.GetDisableCase(maxUpdateDT);
            return table;
        }
        public static DataTable Get_DisableCase_Premier_New()
        {
            string maxUpdateDT = null;
            DataTable table = new DataTable();
            if (string.IsNullOrEmpty(_updateDateTime) == false)
            {
                maxUpdateDT = _updateDateTime;
                _updateDateTime = null;
            }
            else
            {
                maxUpdateDT = _dataAccess.Get_Max_UpdateTime_Dis(SMS_TYPE_PN);

            }
            if (string.IsNullOrEmpty(maxUpdateDT) == false)
            {
                try
                {
                    long MaxUpdateTime = long.Parse(maxUpdateDT);
                }
                catch (Exception ex)
                {
                    classDisableCaseLogWriter.WriteLog("Error Get_DisableCase_Premier_New(), " + ex.Message);
                    return null;
                }
                if (maxUpdateDT != null)
                    table = _dataAccess.GetDisableCase_Premier_New(maxUpdateDT);
            }
            
            return table;
        }
        public static DataTable Get_DisableCase_SCare_New()
        {
            string maxUpdateDT = null;
            DataTable table = new DataTable();
            if (string.IsNullOrEmpty(_updateDateTime) == false)
            {
                maxUpdateDT = _updateDateTime;
                _updateDateTime = null;
            }
            else
            {
                maxUpdateDT = _dataAccess.Get_Max_UpdateTime_Dis(SMS_TYPE_SN);

            }
            if (string.IsNullOrEmpty(maxUpdateDT) == false)
            {
                try
                {
                    long MaxUpdateTime = long.Parse(maxUpdateDT);
                }
                catch (Exception ex)
                {
                    classDisableCaseLogWriter.WriteLog("Error Get_DisableCase_SCare_New(), " + ex.Message);
                    return null;
                }
                if (maxUpdateDT != null)
                    table = _dataAccess.GetDisableCase_SCare_New(maxUpdateDT);
            }
            //////////
            ///table = _dataAccess.GetDisableCase(maxUpdateDT);
            return table;
        }
        public static DataTable Get_DisableCase()
        {
            string maxUpdateDT = null;
            DataTable table = new DataTable();
            if (string.IsNullOrEmpty(_updateDateTime) == false)
            {
                maxUpdateDT = _updateDateTime;
                _updateDateTime = null;
            }
            else
            {
                maxUpdateDT = _dataAccess.Get_Max_UpdateTime_Dis(SMS_TYPE);

            }
            if (string.IsNullOrEmpty(maxUpdateDT) == false)
            {
                try
                {
                    long MaxUpdateTime = long.Parse(maxUpdateDT);
                }
                catch (Exception ex)
                {
                    classDisableCaseLogWriter.WriteLog("Error Get_DisableCase(), " + ex.Message);
                    return null;
                }
                if (maxUpdateDT != null)
                    table = _dataAccess.GetDisableCase(maxUpdateDT);
            }
            //////////
            ///table = _dataAccess.GetDisableCase(maxUpdateDT);
            return table;
        }
        private static int Disable_Case_SCare_Change(string case_type, string case_id, string create_time, string user_lv1, string case_pan)
        {
            try
            {
                string date_time = create_time.Substring(0, 8);
                int flag_sta_case = 0;
                int flag_dis_case = 0;
                //if (case_type == "Card Replacement" || case_type == "Card Renewal" || case_type == "Card Upgrade/Downgrade")
                {
                    if (_dataAccess.Update_Status_CASE_CARD(case_id) == 0)
                        classDisableCaseLogWriter.WriteLog("Err: can't update case status for " + case_type + " ID:" + case_id);
                    else
                    {
                        flag_sta_case = 1;
                        classDisableCaseLogWriter.WriteLog("Update status case " + case_type + " success ID:" + case_id);
                    }
                    if (_dataAccess.Update_DIS_Rep_Renew_up_dow(case_pan, user_lv1, date_time) == 0)                    
                        classDisableCaseLogWriter.WriteLog("Err: can't disable case SCare " + case_type + " Case ID:" + case_id);
                    else
                    {
                        flag_dis_case = 1;
                        classDisableCaseLogWriter.WriteLog("Disable case " + case_type + " success with ID:" + case_id);
                    }

                }


                if (flag_dis_case == 1)
                    return 1;
                else
                    return 0;



            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Disable_Case_SCare_Change() , " + ex.Message);
                return 0;
            }
        }
        private static int Disable_Case_Premier_New(string case_type, string case_id, string create_time, string user_lv1)
        {
            try
            {
                string date_time = create_time.Substring(0, 8);
                int flag_sta_case = 0;
                int flag_dis_case = 0;                
           
                if (_dataAccess.Update_Status_CASE_CARD(case_id) == 0)
                    classDisableCaseLogWriter.WriteLog("Err: can't update case status for " + case_type + " ID:" + case_id);
                else
                {
                    flag_sta_case = 1;
                    classDisableCaseLogWriter.WriteLog("Update status Premier case " + case_type + " success ID:" + case_id);
                }

                if (_dataAccess.Update_DISABLE_Premier(case_id, date_time, user_lv1) == 0)
                    classDisableCaseLogWriter.WriteLog("Err: can't disable case SCare " + case_type + " Case ID:" + case_id);
                else
                {
                    flag_dis_case = 1;
                    classDisableCaseLogWriter.WriteLog("Disable case " + case_type + " success with ID:" + case_id);
                }         


                if (flag_dis_case == 1)
                    return 1;
                else
                    return 0;



            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Disable_Case_Premier_New() , " + ex.Message);
                return 0;
            }
        }
        private static int Disable_Case_SCare_New(string case_type, string case_id, string create_time, string user_lv1)
        {
            try
            {
                string date_time = create_time.Substring(0, 8);
                int flag_sta_case = 0;
                int flag_dis_case = 0;
                //if (case_type == "Card Replacement" || case_type == "Card Renewal" || case_type == "Card Upgrade/Downgrade")
                {
                    if (_dataAccess.Update_Status_CASE_CARD(case_id) == 0)
                        classDisableCaseLogWriter.WriteLog("Err: can't update case status for " + case_type + " ID:" + case_id);
                    else
                    {
                        flag_sta_case = 1;
                        classDisableCaseLogWriter.WriteLog("Update status case " + case_type + " success ID:" + case_id);
                    }

                    if (_dataAccess.Update_DISABLE_SCare(case_id,date_time, user_lv1) == 0)
                        classDisableCaseLogWriter.WriteLog("Err: can't disable case SCare " + case_type + " Case ID:" + case_id);
                    else
                    {
                        flag_dis_case = 1;
                        classDisableCaseLogWriter.WriteLog("Disable case " + case_type + " success with ID:" + case_id);
                    }

                }


                if (flag_dis_case == 1)
                    return 1;
                else
                    return 0;



            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Disable_Case_SCare_New() , " + ex.Message);
                return 0;
            }
        }
        private static int Disable_Case(string case_type, string case_id, string case_pan, string create_time, string user_lv1)
        {
            try
            {
                string date_time = create_time.Substring(0, 8);
                int flag_sta_case = 0;
                int flag_dis_case = 0;
                if (case_type == "Card Replacement" || case_type == "Card Renewal" || case_type == "Card Upgrade/Downgrade")
                {
                    if (_dataAccess.Update_Status_CASE_CARD(case_id) == 0)
                        classDisableCaseLogWriter.WriteLog("Err: can't update case status for " + case_type + " ID:" + case_id);
                    else
                    {
                        flag_sta_case = 1;
                        classDisableCaseLogWriter.WriteLog("Update status case " + case_type + " success ID:" + case_id);
                    }

                    if (_dataAccess.Update_DIS_Rep_Renew_up_dow(case_pan, user_lv1, date_time) == 0)
                        classDisableCaseLogWriter.WriteLog("Err: can't disable case " + case_type + " pan:" + case_pan);
                    else
                    {
                        flag_dis_case = 1;
                        classDisableCaseLogWriter.WriteLog("Disable case " + case_type + " success pan:" + case_pan);
                    }

                }

                if (case_type == "Special Limit")
                {
                    if (_dataAccess.Update_Status_CASE_CARD(case_id) == 0)
                        classDisableCaseLogWriter.WriteLog("Err: can't update case status for " + case_type + " ID:" + case_id);
                    else
                    {
                        flag_sta_case = 1;
                        classDisableCaseLogWriter.WriteLog("Update status case " + case_type + " success ID:" + case_id);
                    }

                    if (_dataAccess.Update_DISABLE_SpecialLimit(case_pan, date_time, user_lv1) == 0)
                        classDisableCaseLogWriter.WriteLog("Err: can't disable case " + case_type + " pan:" + case_pan);
                    else
                    {
                        flag_dis_case = 1;
                        classDisableCaseLogWriter.WriteLog("Disable case " + case_type + " success pan:" + case_pan);
                    }
                }
                if (flag_sta_case + flag_dis_case == 2)
                    return 1;
                else
                    return 0;



            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Disable_Case() , " + ex.Message);
                return 0;
            }
        }
        static void send_SCB_mail_cancel_cardtype_Premier(string to_email, string from_email, string pw, string user_lv1, string key_id, string case_type, string cre_time, string ad_email, string card_type, string TD_email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);
                mail.CC.Add(TD_email);
                //mail.CC.Add(ope_email2);
                //mail.CC.Add(ope_email3);
                //mail.CC.Add(ope_email4);
                //mail.CC.Add(ope_email5);
                mail.CC.Add(ad_email);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Tao Case Premier khong dung han muc";
                mail.Body = "Dear " + user_lv1 + "!\nĐây là email tự động!\nUser " + user_lv1 + " đã tạo case " + case_type + " trên CW với ID là " + key_id + " vào lúc " + cre_time + ", card type: " + card_type
                + ".\nCase này không có hạn mức thẻ nhỏ hơn hạn mức của KH VIP Premier nên hệ thống đã tự hủy.\nĐơn vị thực hiện lại theo đúng quy định tại các thông báo triển khai thẻ Premier";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classDisableCaseLogWriter.WriteLog("case " + case_type + " voi ID " + key_id);
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error send_SCB_mail_cancel_cardtype_Premier() , " + ex.Message);
            }
        }
        static void send_SCB_mail_cancel_cardtype_SCare(string to_email, string from_email, string pw, string user_lv1, string key_id, string case_type, string cre_time, string ad_email, string card_type)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);
                //mail.CC.Add(ope_email1);
                //mail.CC.Add(ope_email2);
                //mail.CC.Add(ope_email3);
                //mail.CC.Add(ope_email4);
                //mail.CC.Add(ope_email5);
                mail.CC.Add(ad_email);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Tao Case S-Care khong co image ID";
                mail.Body = "Dear " + user_lv1 + "!\nĐây là email tự động!\nUser " + user_lv1 + " đã tạo case " + case_type + " trên CW với ID là " + key_id + " vào lúc " + cre_time + ", card type: " + card_type
                + ".\nCase này không có Image ID nên hệ thống đã tự hủy.\nĐơn vị thực hiện lại theo đúng quy định tại các thông báo triển khai thẻ S-Care";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classDisableCaseLogWriter.WriteLog("case " + case_type + " voi ID " + key_id );
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error send_SCB_mail_cancel_cardtype_SCare() , " + ex.Message);
            }
        }
        static void send_SCB_mail_cancel_cardtype(string to_email, string from_email, string pw, string user_lv1, string key_id, string pan, string case_type, string e_pan, string cre_time, string ad_email, string ope_email1, string ope_email2, string ope_email3, string ope_email4, string card_type, string ope_email5)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);                
                mail.CC.Add(ope_email1);
                mail.CC.Add(ope_email2);
                mail.CC.Add(ope_email3);
                mail.CC.Add(ope_email4);
                mail.CC.Add(ope_email5);
                mail.CC.Add(ad_email);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Tao Case sai ma don vi";
                mail.Body = "Dear " + user_lv1 + "!\nĐây là email tự động!\nUser " + user_lv1 + " đã tạo case " + case_type + " trên CW với ID là " + key_id + " vào lúc " + cre_time + " cho thẻ " + pan + ", card type: " + card_type
                + ".\nCase này trái với quy định về triển khai thẻ CONTACTLESS nên hệ thống đã tự hủy.\nĐơn vị thực hiện lại theo đúng quy định tại các thông báo triển khai thẻ Contactles:"
                //+ "\n+Thông báo số 22463/TB-TGĐ.18 ngày 02/11/2018 V/v triển khai phát hành thẻ thanh toán quốc tế Mastercard Contactless."
                + "\n+ Thông báo số 22533/TB-TGĐ.18 ngày 05/11/2018 V/v triển khai thử nghiệm thẻ tín dụng quốc tế Visa Contactless cho CBNV SCB."
                + "\n+ Thông báo số 23444/TB-TGĐ.18 ngày 03/12/2018 V/v triển khai thẻ tín dụng quốc tế Mastercard Contactless hạng Chuẩn/Vàng, cập nhật đặc tính sản phẩm và chương trình ưu đãi của thẻ quốc tế."
                + "\n+ Thông báo số 23972/TB-TGĐ.18 ngày 19/12/2018 V/v triển khai thẻ tín dụng quốc tế Visa Contactless hạng Chuẩn, cập nhật đặc tính sản phẩm và chương trình ưu đãi của thẻ quốc tế.";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classDisableCaseLogWriter.WriteLog("case " + case_type + " voi ID " + key_id + " pan " + e_pan);
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error send_SCB_mail_cancel_cardtype() , " + ex.Message);
            }
        }

        static void send_SCB_mail_cancel(string to_email, string from_email, string pw, string user_lv1, string key_id, string pan, string case_type, string e_pan, string cre_time, string ad_email, string ope_email1, string ope_email2, string ope_email3, string ope_email4, string ope_email5)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);            
                mail.CC.Add(ope_email1);
                mail.CC.Add(ope_email2);
                mail.CC.Add(ope_email3);
                mail.CC.Add(ope_email4);
                mail.CC.Add(ope_email5);
                mail.CC.Add(ad_email);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Tao Case sai ma don vi";
                mail.Body = "Dear " + user_lv1 + "!\nĐây là email tự động!\nUser " + user_lv1 + " đã tạo case " + case_type + " trên CW với ID là " + key_id + " vào lúc " + cre_time + " cho thẻ " + pan
                + ".\nCase này trái với quy định vì user tạo case có mã đơn vị khác với đơn vị của thẻ chính.\nCase này đã được hệ thống tự hủy.\nVui lòng liên hệ Đơn vị phát hành thẻ chính để phối hợp thực hiện";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classDisableCaseLogWriter.WriteLog("case " + case_type + " voi ID " + key_id + " pan " + e_pan);
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error send_SCB_mail_cancel() , " + ex.Message);
            }
        }
        static void send_SCB_mail_approval_cardtype(string to_email, string from_email, string pw, string user_lv2, string user_lv1, string key_id, string pan, string case_type, string cc_email, string e_pan, string cre_time, string ad_email, string ope_email1, string ope_email2, string card_type, string ope_email3, string ope_email4, string ope_email5)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);
                mail.CC.Add(ope_email1);
                mail.CC.Add(ope_email2);
                mail.CC.Add(ope_email3);
                mail.CC.Add(ope_email4);
                mail.CC.Add(ope_email5);
                mail.CC.Add(ad_email);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Duyet Case sai ma don vi";
                mail.Body = "Dear " + user_lv2 + "!\nĐây là email tự động!\nUser " + user_lv2 + " đã duyêt case " + case_type + " trên CW với ID là " + key_id + " được tạo vào lúc " + cre_time + " cho thẻ " + pan + ", card type: " + card_type + " do user " + user_lv1
                + " tạo.\nCase này trái với quy định theo Thông báo 22653/TB-TGĐ.18 ngày 09/11/2018 v/v liên quan công tác phát hành thẻ quốc tế Contactless.";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classDisableCaseLogWriter.WriteLog("case " + case_type + " voi ID " + key_id + " pan " + e_pan + "đã duyệt sai quy định");
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error send_SCB_mail_approval_CardType() , " + ex.Message);
            }
        }
        static void send_SCB_mail_approval_cardtype_Premier(string to_email, string from_email, string pw, string user_lv2, string user_lv1, string key_id, string case_type, string cre_time, string ad_email, string card_type, string TD_email1, string TD_email2)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);
                mail.CC.Add(ad_email);
                mail.CC.Add(TD_email1);
                mail.CC.Add(TD_email2);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Case Premier khong dung han muc the VIP";
                mail.Body = "Dear " + user_lv2 + "!\nĐây là email tự động!\nUser " + user_lv2 + " đã duyêt case " + case_type + " trên CW với ID là " + key_id + " được tạo vào lúc " + cre_time + " cho thẻ Premier nhưng hạn mức nhỏ hơn hạn mức của KH VIP Premier)" + ", card type: " + card_type + " do user " + user_lv1
                + " tạo.\nVui lòng kiểm tra lại thông tin.";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classDisableCaseLogWriter.WriteLog("case " + case_type + " voi ID " + key_id + "da duyet S-Care khong co image ID");
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error send_SCB_mail_approval_cardtype_Premier() , " + ex.Message);
            }
        }
        static void send_SCB_mail_approval_cardtype_SCare(string to_email, string from_email, string pw, string user_lv2, string user_lv1, string key_id, string case_type, string cre_time, string ad_email, string card_type)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);               
                mail.CC.Add(ad_email);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Case SCare khong co image ID";
                mail.Body = "Dear " + user_lv2 + "!\nĐây là email tự động!\nUser " + user_lv2 + " đã duyêt case " + case_type + " trên CW với ID là " + key_id + " được tạo vào lúc " + cre_time + " cho thẻ S-Care nhưng không có image ID " + ", card type: " + card_type + " do user " + user_lv1
                + " tạo.\nVui lòng kiểm tra gấp trước khi thẻ được dập!";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classDisableCaseLogWriter.WriteLog("case " + case_type + " voi ID " + key_id + "da duyet S-Care khong co image ID");
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error send_SCB_mail_approval_cardtype_SCare() , " + ex.Message);
            }
        }
        static void send_SCB_mail_approval(string to_email, string from_email, string pw, string user_lv2, string user_lv1, string key_id, string pan, string case_type, string cc_email, string e_pan, string cre_time, string ad_email, string ope_email1, string ope_email2, string ope_email3, string ope_email4, string ope_email5)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);
                mail.CC.Add(ope_email1);
                mail.CC.Add(ope_email2);
                mail.CC.Add(ope_email3);
                mail.CC.Add(ope_email4);
                mail.CC.Add(ope_email5);
                mail.CC.Add(ad_email);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Duyet Case sai ma don vi";
                mail.Body = "Dear " + user_lv2 + "!\nĐây là email tự động!\nUser " + user_lv2 + " đã duyêt case " + case_type + " trên CW với ID là " + key_id + " được tạo vào lúc " + cre_time + " cho thẻ " + pan + " do user " + user_lv1
                + " tạo.\nCase này trái với quy định vì user tạo case có mã đơn vị khác với mã đơn vị của thẻ chính.";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classDisableCaseLogWriter.WriteLog("case " + case_type + " voi ID " + key_id + " pan " + e_pan + "đã duyệt sai quy định");
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error send_SCB_mail_approval() , " + ex.Message);
            }
        }
      
    }
}
