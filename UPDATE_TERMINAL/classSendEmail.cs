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
using System.IO;

namespace UPDATE_TERMINAL
{
    class classSendEmail
    {
        public static bool _exitThread = false;
        public static string _updateDateTime = null;
        private static classDataAccess _dataAccess = new classDataAccess();       
        public static string SMS_TYPE = "SEND_E";
        public static string SMS_TYPE_Main = "SEND_M";

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
                DataTable table_maincard = new DataTable();

                SCBPhone = classUtilities.GetStringValueFromConfig("SCB_Contact_Phone");
                value = classUtilities.GetIntValueFromConfig("Send_Email_Minute");
                sleep = classUtilities.GetIntValueFromConfig("Send_Email_Second");
                while (_exitThread == false)
                {
                    minute = DateTime.Now.Minute;
                    if (minute % value == 0) 
                    //if (1==1)
                    {

                        classSendEmailLogWriter.WriteLog("----------------Begin Process-----------------");                       
                        _dataAccess = new classDataAccess();
                        table_maincard.Rows.Clear();
                        table_maincard = Get_SendEmail_MainCard();
                        if (table_maincard.Rows.Count > 0)
                        {
                            Process_Send_Email_MainCard(table_maincard);
                        }

                        table.Rows.Clear();
                        table = Get_SendEmail();
                        if (table.Rows.Count > 0)
                        {
                            Process_Send_Email(table);
                        }

                        classSendEmailLogWriter.WriteLog("----------------End Process----------------- at: " + DateTime.Now.ToString());
                        //Thread.Sleep(1000 * 30);

                    }

                    if (value > 2)
                    {
                        if ((value - (minute % value) - 1) > 0)
                        {
                            classSendEmailLogWriter.WriteLog("sleep " + (value - (minute % value) - 1) + " minute");
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
                classSendEmailLogWriter.WriteLog("Error RunService(), " + ex.Message);
            }
        }

        private static void Process_Send_Email_MainCard(DataTable table)
        {
            try
            {
                string from_email = classUtilities.GetStringValueFromConfig("from_email");
                string pass_email = classUtilities.GetStringValueFromConfig("pass_email");
                string ad_email = classUtilities.GetStringValueFromConfig("ad_email");
                string DN_email1 = classUtilities.GetStringValueFromConfig("DN_email1");
                string DN_email2 = classUtilities.GetStringValueFromConfig("DN_email2");
                string DN_email3 = classUtilities.GetStringValueFromConfig("DN_email3");
               


                foreach (DataRow row in table.Rows)
                {




                    int flag = _dataAccess.Insert_Send_Email(SMS_TYPE_Main, "no_data", "no_data", DateTime.Today, row.ItemArray[9].ToString(), row.ItemArray[1].ToString());
                        //string create_time = row.ItemArray[13].ToString();
                        if (flag > 0)
                        {

                            send_SCB_mail_approval_MainCard(row.ItemArray[4].ToString()
                            , from_email, pass_email, row.ItemArray[2].ToString()
                            , row.ItemArray[5].ToString(), row.ItemArray[10].ToString(), row.ItemArray[8].ToString(),
                            row.ItemArray[9].ToString(), ad_email, DN_email1,DN_email2,DN_email3, row.ItemArray[6].ToString(), row.ItemArray[7].ToString());

                        }
                    
                }

            }
            catch (Exception ex)
            {
                classSendEmailLogWriter.WriteLog("Error Process_Send_Email_MainCard(), " + ex.Message);
            }

        }

        private static void Process_Send_Email(DataTable table)
        {
            try
            {
                string from_email = classUtilities.GetStringValueFromConfig("from_email");
                string pass_email = classUtilities.GetStringValueFromConfig("pass_email");              
                string ad_email = classUtilities.GetStringValueFromConfig("ad_email");
                string TD_email1 = classUtilities.GetStringValueFromConfig("TD_email");
                string TD_email2 = classUtilities.GetStringValueFromConfig("TD_email2");
                //string ope_email3 = classUtilities.GetStringValueFromConfig("Ope_email3");
                //string ope_email4 = classUtilities.GetStringValueFromConfig("ope_email4");
                //string ope_email5 = classUtilities.GetStringValueFromConfig("ope_email5");
                
                
                foreach (DataRow row in table.Rows)
                {

                    if (row.ItemArray[8].ToString().Substring(0, 6) == "NWAPPB") //the chinh
                    {
                        
                        int flag = _dataAccess.Insert_Send_Email(SMS_TYPE, "no_data", "no_data", DateTime.Today, row.ItemArray[9].ToString(), row.ItemArray[1].ToString());
                        //string create_time = row.ItemArray[13].ToString();
                        if (flag > 0)
                        {
                            
                                send_SCB_mail_approval_cardtype(row.ItemArray[4].ToString()
                                ,from_email, pass_email, row.ItemArray[2].ToString()
                                ,row.ItemArray[5].ToString(), row.ItemArray[10].ToString(), row.ItemArray[8].ToString(),
                                row.ItemArray[9].ToString(), ad_email, TD_email1, TD_email2, row.ItemArray[6].ToString(), row.ItemArray[7].ToString());

                        }
                    }
                    else //the phu
                    {
                        
                        int flag = _dataAccess.Insert_Send_Email(SMS_TYPE, "no_data", "no_data", DateTime.Today, row.ItemArray[9].ToString(), row.ItemArray[1].ToString());
                        //string create_time = row.ItemArray[13].ToString();
                        if (flag > 0)
                        {

                            send_SCB_mail_approval_subcardtype(row.ItemArray[4].ToString()
                            , from_email, pass_email, row.ItemArray[2].ToString()
                            , row.ItemArray[5].ToString(), row.ItemArray[10].ToString(), row.ItemArray[8].ToString(),
                            row.ItemArray[9].ToString(), ad_email, TD_email1, row.ItemArray[6].ToString(), row.ItemArray[7].ToString());

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                classSendEmailLogWriter.WriteLog("Error Get_SendEmail(), " + ex.Message);
            }

        }
        public static DataTable Get_SendEmail_MainCard()
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
                maxUpdateDT = _dataAccess.Get_Max_UpdateTime_SendEmail(SMS_TYPE_Main);

            }
            if (string.IsNullOrEmpty(maxUpdateDT) == false)
            {
                try
                {
                    long MaxUpdateTime = long.Parse(maxUpdateDT);
                }
                catch (Exception ex)
                {
                    classSendEmailLogWriter.WriteLog("Error Get_SendEmail_MainCard(), " + ex.Message);
                    return null;
                }
                if (maxUpdateDT != null)
                    table = _dataAccess.GetSendEmail_MaiCard(maxUpdateDT);
            }
          
            return table;
        }
       
        public static DataTable Get_SendEmail()
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
                maxUpdateDT = _dataAccess.Get_Max_UpdateTime_SendEmail(SMS_TYPE);

            }
            if (string.IsNullOrEmpty(maxUpdateDT) == false)
            {
                try
                {
                    long MaxUpdateTime = long.Parse(maxUpdateDT);
                }
                catch (Exception ex)
                {
                    classSendEmailLogWriter.WriteLog("Error Get_SendEmail(), " + ex.Message);
                    return null;
                }
                if (maxUpdateDT != null)
                    table = _dataAccess.GetSendEmail(maxUpdateDT);
            }
            //////////
            ///table = _dataAccess.GetSendEmail(maxUpdateDT);
            return table;
        }


        static void send_SCB_mail_approval_MainCard(string to_email, string from_email, string pw, string f_name, string cif, string key_id, string case_type, string e_pan, string ad_email, string DN_email1, string DN_email2, string DN_email3, string card_type, string han_muc)
        {
            try
            {
                int length = to_email.IndexOf("@");
                string name;
                if (length > 0)
                    name = to_email.Substring(0, length);
                else
                    name = to_email;
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);
                mail.CC.Add(DN_email1);
                mail.CC.Add(DN_email2);
                mail.CC.Add(DN_email3);
                mail.CC.Add(ad_email);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Thông báo phát hành thẻ Doanh Nghiệp thiếu check MAIN" + f_name;
                mail.Body = "Kính gửi " + name + ",\nĐây là email tự động! Vui lòng không Reply email này.\nThẻ KHDN vừa được phát hành thành công trên hệ thống Cardworks, cụ thể:\n"
                + "Họ tên KH: " + f_name + "\n"
                + "CIF: " + cif + "\n"
                + "Cardtype: " + card_type + "\n"
                + "Anh/chị vui lòng truy cập vào hệ thống (màn hình New application enquiry) để kiểm tra trường MAIN của thẻ.\n"
                + "Trân trọng,\n"
                + "(Đây là email được gửi theo yêu cầu của P.KHDN cho NV tạo thẻ sau khi thẻ được duyệt trên hệ thống nhưng thiếu trường MAIN)";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classSendEmailLogWriter.WriteLog("Main case " + case_type + " voi ID " + key_id + " pan " + e_pan + "đã duyệt sai quy định");
            }
            catch (Exception ex)
            {
                classSendEmailLogWriter.WriteLog("ID " + key_id + " Error send_SCB_mail_approval_MainCard() , " + ex.Message);
            }
        }
      
        static void send_SCB_mail_approval_subcardtype(string to_email, string from_email, string pw, string f_name, string cif, string key_id, string case_type, string e_pan, string ad_email, string TD_email1, string card_type, string han_muc)
        {
            try
            {
                int length = to_email.IndexOf("@");
                string name;
                if (length > 0)
                    name = to_email.Substring(0, length);
                else
                    name = to_email;
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);
                //mail.CC.Add(TD_email1);
                //mail.CC.Add(ope_email2);
                //mail.CC.Add(ope_email3);
                //mail.CC.Add(ope_email4);
                //mail.CC.Add(ope_email5);               
                //mail.CC.Add(ad_email);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Thông báo phát hành thẻ TDQT_KH " + f_name;
                mail.Body = "Kính gửi " + name + ",\nĐây là email tự động! Vui lòng không Reply email này.\nThẻ TDQT vừa được phát hành thành công trên hệ thống Cardworks, cụ thể:\n"
                + "Họ tên KH: " + f_name + "\n"
                + "CIF: " + cif + "\n"
                + "Cardtype: " + card_type + "\n"
                + "Anh/chị vui lòng truy cập vào hệ thống (màn hình New application enquiry) để kiểm tra và lưu chứng từ theo đúng quy định.\n"
                + "Trân trọng,\n"
                +"(Đây là email được gửi theo yêu cầu của P.TĐ&PD cho NV kinh doanh sau khi thẻ được duyệt trên hệ thống)";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classSendEmailLogWriter.WriteLog("case " + case_type + " voi ID " + key_id + " pan " + e_pan + "đã duyệt sai quy định");
            }
            catch (Exception ex)
            {
                classSendEmailLogWriter.WriteLog("ID " + key_id + " Error send_SCB_mail_approval_subcardtype() , " + ex.Message);
            }
        }
        static void send_SCB_mail_approval_cardtype(string to_email, string from_email, string pw, string f_name, string cif, string key_id, string case_type, string e_pan,string ad_email, string TD_email1,string TD_email2,string card_type,string han_muc )
        {
            try
            {
                double temp = double.Parse(han_muc);
                string temp_hm = string.Format("{0:#,##0.##}", temp);  
                int length = to_email.IndexOf("@");
                string name;
                if (length > 0)
                    name = to_email.Substring(0, length);
                else
                    name = to_email;
                MailMessage mail = new MailMessage();
                MailAddress mailAddress = new MailAddress(to_email);
                mail.To.Add(mailAddress);
                //mail.CC.Add(TD_email1);
                mail.CC.Add(TD_email2);                                           
                //mail.CC.Add(ad_email);
                mail.From = new MailAddress(from_email);
                mail.Subject = "Thông báo phát hành thẻ TDQT_KH " + f_name;
                mail.Body = "Kính gửi " + name + ",\nĐây là email tự động! Vui lòng không Reply email này.\nThẻ TDQT vừa được phát hành thành công trên hệ thống Cardworks, cụ thể:\n" 
                + "Họ tên KH: " + f_name + "\n" 
                + "CIF: " + cif + "\n"
                + "Cardtype: " + card_type +"\n"
                + "Hạn mức thẻ chính: " + temp_hm + " đồng\n"
                + "Anh/chị vui lòng truy cập vào hệ thống (màn hình New application enquiry) để kiểm tra và lưu chứng từ theo đúng quy định.\n"
                + "Trân trọng,";
                NetworkCredential credentials = new NetworkCredential(from_email, pw);
                SmtpClient mailClient = new SmtpClient("mail.scb.com.vn", 587);
                mailClient.Credentials = credentials;
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mail);
                classSendEmailLogWriter.WriteLog("case approval " + case_type + " voi ID " + key_id + " pan " + e_pan + ": already send email");
            }
            catch (Exception ex)
            {
                classSendEmailLogWriter.WriteLog("Error send_SCB_mail_approval_CardType() , " + ex.Message);
            }
        }
       
      
    }
}
