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
    class classDataAccess
    {
        public static string IDALERT = "MASTER_CARD_ALERT";
        OracleConnection connectionOracle;
        //OracleConnection connectionOracle1;
        //OracleConnection connectionOracle2;
        //OracleConnection connectionOracle3;
        //OracleConnection connectionOracle4;
        //static private OracleConnection connectionOracle_STAN;
        static private SqlConnection connectionSQL;
      
      

        void classDateAccess()
        {
            connectionOracle = new OracleConnection();
            //connectionOracle1 = new OracleConnection();
            //connectionOracle2 = new OracleConnection();
            //connectionOracle3 = new OracleConnection();
            //connectionOracle4 = new OracleConnection();
            connectionSQL = new SqlConnection();
        }

        public int CHECK_TXN_IPC_PENDING()
        {
            try
            {
                int flag = 0;
                bool temp = OpenConnectionSQL();
                SqlCommand cmd = new SqlCommand("CHECK_TXN_PENDING", connectionSQL);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                SqlParameter sqlParam = new SqlParameter("@Result", SqlDbType.Int);
                sqlParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sqlParam);
                cmd.ExecuteNonQuery();

                flag = int.Parse(cmd.Parameters["@Result"].Value.ToString());
                if(temp==true)
                    CloseConnectionSQL();
                return flag;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Err Check_TXN_PENDING() at DataAccess " + ex.ToString());
                return 0;
            }

        }

        public int Get_time_StartBatch()
        {
            bool flag = false;
            try
            {
                int temp = 0;
                flag = OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL
                OracleCommand cmd = new OracleCommand("Get_time_StartBatch", connectionOracle);           
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter time_p = new OracleParameter("time_p", OracleType.Number);
                time_p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(time_p);
                
                cmd.ExecuteNonQuery();
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                temp = int.Parse(cmd.Parameters["time_p"].Value.ToString());
                return temp;

            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_time_StartBatch(), " + ex.Message);
                CloseConnection();// dong ket noi CSDL  
                return 0;
            }

        }
       
        public DataTable Get_Expiry_User_AM()
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                flag=OpenConnectionOracle("CW_AM");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_AM");// mo ket noi CSDL
                OracleCommand cmd = new OracleCommand("Get_User_Expiry", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);
                if(flag==true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_Expiry_User_AM_() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                data.Clear();
                return data;

            }

        }
        public DataTable Get_Expiry_User_DW()
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                flag = OpenConnectionOracle("CCPS_DW_STAN");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CCPS_DW_STAN");// mo ket noi CSDL
                OracleCommand cmd = new OracleCommand("Get_User_Expiry", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_Expiry_User_DW() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                data.Clear();
                return data;

            }

        }
        public DataTable Get_Expiry_User_IM()
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                flag = OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL
                OracleCommand cmd = new OracleCommand("Get_User_Expiry", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_Expiry_User_IM_() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                data.Clear();
                return data;

            }

        }
        public DataTable Get_Diff_MSL()
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                //OpenConnectionOracle("CW_IM");// mo ket noi CSDL                
                flag=OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_IM_STAN");// mo ket noi
                OracleCommand cmd = new OracleCommand("Get_Diff_MSL", connectionOracle);
                
                //OracleCommand cmd = new OracleCommand("Get_Diff_MSL_STAN", connectionOracle_STAN);
                
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                 
                cmd.Parameters.Add(results);
                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_Diff_MSL() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                data.Clear();
                return data;

            }

        }
        public DataTable Get_Diff_Branch()
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                //OpenConnectionOracle("CW_IM");// mo ket noi CSDL             
                flag = OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL
                OracleCommand cmd = new OracleCommand("Get_Diff_Branch", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
               
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_Diff_Branch() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                data.Clear();
                return data;

            }

        }
        public DataTable Get_Inf_TXN_ISO()
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                //OpenConnectionOracle("CW_IM");// mo ket noi CSDL   
                flag=OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL   
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL 
                OracleCommand cmd = new OracleCommand("Get_Inf_TXN_ISO", connectionOracle);
                
                //OracleCommand cmd = new OracleCommand("Get_Inf_TXN_ISO_STAN", connectionOracle_STAN);
                
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                  
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_Inf_TXN_ISO() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                data.Clear();
                return data;

            }

        }
        public int Check_Inf_DEKM_IM()        {
            
            bool flag = false;
            try
            {
                flag = OpenConnectionOracle("CW_IM");// mo ket noi CSDL    
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_IM");// mo ket noi CSDL   
                OracleCommand cmd = new OracleCommand("Get_Inf_DEKM", connectionOracle);
                //OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL
                //OracleCommand cmd = new OracleCommand("Get_Inf_DEKM_STAN", connectionOracle_STAN);                

                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter card_num = new OracleParameter("flag_check", OracleType.Number);
                card_num.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(card_num);               
                cmd.ExecuteNonQuery();
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL    
                string temp = cmd.Parameters["flag_check"].Value.ToString();
                if (temp == "0")
                    return 0;//binh thuong
                else
                {
                    if (temp == "2")
                    {
                        classCheckErrLogWriter.WriteLog("exception store Check_Inf_DEKM_IM, " + temp);
                        return 2;
                    }
                    else
                    {
                        classCheckErrLogWriter.WriteLog("DEKM not running, " + temp);
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Inf_DEKM_IM() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                return 2;

            }

        }
        public int Check_Inf_DEKM_AM()
        {
            //DataTable data = new DataTable();
            bool flag = false;
            try
            {
                flag = OpenConnectionOracle("CW_AM");// mo ket noi CSDL    
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_AM");// mo ket noi CSDL   
                OracleCommand cmd = new OracleCommand("Get_Inf_DEKM", connectionOracle);                          

                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter card_num = new OracleParameter("flag_check", OracleType.Number);
                card_num.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(card_num);
                cmd.ExecuteNonQuery();
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL    
                string temp = cmd.Parameters["flag_check"].Value.ToString();
                if (temp == "0")
                    return 0;//binh thuong
                else
                {
                    if (temp == "2")
                    {
                        classCheckErrLogWriter.WriteLog("exception store Check_Inf_DEKM_AM, " + temp);
                        return 2;
                    }
                    else
                    {
                        classCheckErrLogWriter.WriteLog("DEKM not running, " + temp);
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Inf_DEKM_AM() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                return 0;

            }

        }

        public int Check_Err_DEKM_DW()
        {
            //DataTable data = new DataTable();
            bool flag = false;
            try
            {
                flag = OpenConnectionOracle("CCPS_DW");// mo ket noi CSDL    
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CCPS_DW");// mo ket noi CSDL   
                OracleCommand cmd = new OracleCommand("Get_Inf_DEKM", connectionOracle);
                //OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL
                //OracleCommand cmd = new OracleCommand("Get_Inf_DEKM_STAN", connectionOracle_STAN);                

                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter card_num = new OracleParameter("flag_check", OracleType.Number);
                card_num.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(card_num);
                cmd.ExecuteNonQuery();
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL    
                string temp = cmd.Parameters["flag_check"].Value.ToString();
                if (temp == "0")
                    return 0;//binh thuong
                else
                {
                    if (temp == "2")
                    {
                        classCheckErrLogWriter.WriteLog("exception store Check_Inf_DEKM_IM, " + temp);
                        return 2;
                    }
                    else
                    {
                        classCheckErrLogWriter.WriteLog("DEKM not running, " + temp);
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Check_Inf_DEKM_DW() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();                
                return 0;

            }

        }
        public string Get_Max_UpdateTimeErr(string smsType)
        {
            bool flag = false;
            try
            {
                string temp = null;
                //OpenConnectionOracle("CW_DW");// mo ket noi CSDL  
                flag=OpenConnectionOracle("CW_DW_STAN");// mo ket noi CSDL 
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_DW_STAN");// mo ket noi CSDL 
                OracleCommand cmd = new OracleCommand("Get_Max_UpdateTimeErr", connectionOracle);
                
                //OracleCommand cmd = new OracleCommand("Get_Max_UpdateTimeErr_STAN", connectionOracle_STAN);
                OracleParameter sms_type_p = new OracleParameter("sms_type_p", smsType);
                sms_type_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(sms_type_p);

                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter update_time = new OracleParameter("update_time_p", OracleType.NVarChar, 17);
                update_time.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(update_time);
                
                cmd.ExecuteNonQuery();
                if (flag == true)
                    CloseConnectionOracle();
                temp = cmd.Parameters["update_time_p"].Value.ToString();
                return temp;

            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_Max_UpdateTimeErr()at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL  
                return null;
            }

        }

        public DataTable GetOutstandingChange_Err(string updateDateTime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                //OpenConnectionOracle("CW_IM");// mo ket noi CSDL    
                flag=OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL   
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL  
                OracleCommand cmd = new OracleCommand("Get_Outstanding_Bal_Change_Err", connectionOracle);
                 
                //OracleCommand cmd = new OracleCommand("Get_Out_Bal_Chan_Err_STAN", connectionOracle_STAN);
                
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updateTime = new OracleParameter("update_time", updateDateTime);
                updateTime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updateTime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                
                da.Fill(data);
                if(flag==true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error GetOutstandingChange_Err() at DataAccess, " + ex.Message);
                if (flag == true)
                CloseConnectionOracle();
                data.Clear();
                return data;

            }

        }

        public DataTable Get_Inf_TXN_SML()
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                //OpenConnectionOracle("CW_IM");// mo ket noi CSDL     
                flag=OpenConnectionOracle("CW_AM_STAN");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_AM_STAN");// mo ket noi CSDL
                OracleCommand cmd = new OracleCommand("Get_Inf_TXN_SML", connectionOracle);
                
                //OracleCommand cmd = new OracleCommand("Get_Inf_TXN_SML_STAN", connectionOracle_STAN);                
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
               
                da.Fill(data);
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_Inf_TXN_SML() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                data.Clear();
                return data;

            }

        }

        public DataTable Get_Inf_TXN_WEB()
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                //OpenConnectionOracle("CW_IM");// mo ket noi CSDL   
                flag=OpenConnectionOracle("CW_AM_STAN");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_AM_STAN");// mo ket noi CSDL
                OracleCommand cmd = new OracleCommand("Get_Inf_TXN_WEB", connectionOracle);
                
                //OracleCommand cmd = new OracleCommand("Get_Inf_TXN_WEB_STAN", connectionOracle_STAN);
                
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
               
                da.Fill(data);
                if (flag == true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error Get_Inf_TXN_WEB() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                data.Clear();
                return data;

            }

        }
        public DataTable GetPaymentManualErr(string updateDateTime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                OpenConnectionOracle("CW_IM");// mo ket noi CSDL 
               
                //flag=OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL   
                //if (connectionOracle.State == ConnectionState.Closed)
                //    flag = OpenConnectionOracle("CW_IM_STAN");// mo ket noi CSDL
                OracleCommand cmd = new OracleCommand("Get_Outstanding_Pay_Man_Err", connectionOracle);
                
                //OracleCommand cmd = new OracleCommand("Get_Out_Pay_Man_Err_STAN", connectionOracle_STAN);
                
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updateTime = new OracleParameter("update_time", updateDateTime);
                updateTime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updateTime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);                
                da.Fill(data);
            
                if(flag==true)
                    CloseConnectionOracle();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error GetPaymentManual() at data access, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                data.Clear();
                return data;
            }

        }
        public bool OpenConnectionOracle(string DBname)
        {
            
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.AppSettings[DBname];
                connectionOracle = new OracleConnection(connectionString);
                connectionOracle.Open();
                //classDataAccessLogWriter.WriteLog("Open connection Oracle successful for " + connectionOracle.ConnectionString);
       
                return true;
            }
            catch (Exception ex)
            {
                classDataAccessLogWriter.WriteLog("Error OpenConnectionOracle(), " + ex.Message);
                return false;
            }

        }
       
        private  bool OpenConnectionSQL()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.AppSettings["SQLServerIPC"].ToString();
                connectionSQL = new SqlConnection(connectionString);
                connectionSQL.Open();
                //classDataAccessLogWriter.WriteLog("Open connection SQL successful for " + connectionSQL.ConnectionString);
                return true;
            }
            catch (Exception ex)
            {
                classDataAccessLogWriter.WriteLog("Error OpenConnectionSQL(), " + ex.Message);
                return false;
            }

        }
       
        public  bool CloseConnectionOracle()
        {
            try
            {
                connectionOracle.Close();
                //classDataAccessLogWriter.WriteLog("Close connection Oracle successful for " + connectionOracle.ConnectionString);
                return true;
            }
            catch (Exception ex)
            {
                classDataAccessLogWriter.WriteLog("Error CloseConnectionOracle(), " + ex.Message);
                return false;
            }
        }
        private static bool CloseConnectionSQL()
        {
            try
            {
                connectionSQL.Close();
                //classDataAccessLogWriter.WriteLog("Close connection SQL successful for " + connectionSQL.ConnectionString);
                return true;
            }
            catch (Exception ex)
            {
                classDataAccessLogWriter.WriteLog("Error CloseConnectionSQL(), " + ex.Message);
                return false;
            }
        }

        public DataTable GET_INF_TERMINAL_FROM_MER(string mer_id)
        {
            DataTable data = new DataTable();
            bool flag = false;
            if (mer_id == null || mer_id == "")
            {
                classUpdateTerminalLogWriter.WriteLog("Error GET_INF_TERMINAL_FROM_MER(): MerchantID null");
                return data;
            }
            try
            {
                flag=OpenConnectionOracle("FCSCB");
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("FCSCB");
                OracleCommand cmd = new OracleCommand("GET_INF_TERMINAL_FROM_MER", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter MID = new OracleParameter("Mer_id", mer_id);
                MID.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(MID);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                
                da.Fill(data);
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error GET_INF_TERMINAL_FROM_MER() ," + ex.Message);
                return data;
            }
            if(flag==true)
                CloseConnectionOracle();
            return data;
        }

        public DataTable Get_inf_ter(string updateDatetime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                flag=OpenConnectionOracle("FCSCB");
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("FCSCB");
                OracleCommand cmd = new OracleCommand("GET_INF_TERMINAL", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updatetime = new OracleParameter("update_time", updateDatetime);
                updatetime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updatetime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
               
                da.Fill(data);
                if(flag==true)
                    CloseConnectionOracle();
                return data;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Get_inf_ter() ," + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                return data;
               
            }
            
        }

        public DataTable Get_inf_mer_train(string updateDatetime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                flag=OpenConnectionOracle("train");
               
                OracleCommand cmd = new OracleCommand("GET_INF_MERCHANT", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updatetime = new OracleParameter("update_time", updateDatetime);
                updatetime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updatetime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("train");
                da.Fill(data);
                if(flag==true)
                    CloseConnectionOracle();
                return data;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Get_inf_mer_train() ," + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                return data;

            }

        } 
        public DataTable Get_inf_mer(string updateDatetime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                flag=OpenConnectionOracle("FCSCB");
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("FCSCB");
                OracleCommand cmd = new OracleCommand("GET_INF_MERCHANT", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updatetime = new OracleParameter("update_time", updateDatetime);
                updatetime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updatetime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
               
                da.Fill(data);
                if(flag==true)
                    CloseConnectionOracle();
                return data;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Get_inf_mer() ," + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                return data;

            }
           
        }         

       
       
       
        /*
        public bool Insert_Terminal(DataTable table)
        {
            try
            {
                bool temp = OpenConnectionSQL();
                SqlCommand cmd = new SqlCommand("insert_terminal", connectionSQL);
                cmd.CommandType = CommandType.StoredProcedure;               
                foreach (DataRow row in table.Rows)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("TERMINALID", row.ItemArray[0].ToString()));
                    cmd.Parameters.Add(new SqlParameter("TERMINALGL", row.ItemArray[1].ToString()));
                    cmd.Parameters.Add(new SqlParameter("DESTID", row.ItemArray[2].ToString()));
                    cmd.Parameters.Add(new SqlParameter("SUSPENDACCT", row.ItemArray[3].ToString()));
                    cmd.Parameters.Add(new SqlParameter("SOURCESYSTEM", row.ItemArray[4].ToString()));
                    cmd.Parameters.Add(new SqlParameter("BRANCH", row.ItemArray[5].ToString()));
                    cmd.ExecuteNonQuery();

                }
                CloseConnectionSQL();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        */

      
        public int Check_Exist_Ter(string Ter_id)
        {
           
            if (Ter_id == null || Ter_id=="")
            {
                classUpdateTerminalLogWriter.WriteLog("Err Check_Exist(): ter_id null ");
                return 0;
            }
            try
            {   
                int flag = 0;
                bool temp = OpenConnectionSQL();

                SqlCommand cmd = new SqlCommand("check_exist_terminal", connectionSQL);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@TER_ID", Ter_id));

                SqlParameter sqlParam = new SqlParameter("@Result", SqlDbType.Int);
                sqlParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sqlParam);
                cmd.ExecuteNonQuery();

                flag = int.Parse(cmd.Parameters["@Result"].Value.ToString());
                if(temp==true)
                    CloseConnectionSQL();
                return flag;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Err Check_Exist() " + ex.ToString());                
                return 0;
            }

        }
 
        public string Get_Max_UpdateTime_Ter()
        {

            try
            {
                string time = null;
                bool temp = OpenConnectionSQL();
                SqlCommand cmd = new SqlCommand("Get_Max_Uptime_Ter", connectionSQL);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                SqlParameter sqlParam = new SqlParameter("@Max_Up_time", SqlDbType.VarChar, 50);
                sqlParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sqlParam);
                cmd.ExecuteNonQuery();

                time = cmd.Parameters["@Max_Up_time"].Value.ToString();
                if(temp==true)
                    CloseConnectionSQL();
                return time;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Get_Max_Uptime_Ter(), " + ex.Message);               
                return null;
            }

        }
        public string Get_Max_UpdateTime_Mer()
        {
            try
            {
                string time = null;
                bool temp = OpenConnectionSQL();
                SqlCommand cmd = new SqlCommand("Get_Max_Uptime_Mer", connectionSQL);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                SqlParameter sqlParam = new SqlParameter("@Max_Up_time", SqlDbType.VarChar,50);
                sqlParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sqlParam);
                cmd.ExecuteNonQuery(); 
               
                time=cmd.Parameters["@Max_Up_time"].Value.ToString();
                if(temp==true)
                    CloseConnectionSQL();
                return time;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Get_Max_Uptime_Mer(), " + ex.Message);               
                return null;
            }

        }
        public int CHECK_USED_MER(string mer_id)
        {
            bool flag = false;
            if (mer_id == null || mer_id=="")
            {
                classUpdateTerminalLogWriter.WriteLog("Error CHECK_USE_MER(): mer_id null");
                return 0;
            }
            try
            {
                flag=OpenConnectionOracle("FCSCB");
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("FCSCB");
                OracleCommand cmd = new OracleCommand("CHECK_USE_MER", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter merid = new OracleParameter("mer_id", mer_id);
                merid.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(merid);

                OracleParameter count = new OracleParameter("p_count", OracleType.Int16);
                count.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(count);
               
                cmd.ExecuteNonQuery();
             
                int  temp = 0;            
                temp = int.Parse(cmd.Parameters[1].Value.ToString());
                if(flag==true)
                    CloseConnectionOracle();
                return temp;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error CHECK_USE_MER() ," + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                return 0;
            }


        }
        public string GET_INF_ACC(string mer_id)
        {
            bool flag = false;
            try
            {
                if (mer_id== null || mer_id =="")
                {
                    classUpdateTerminalLogWriter.WriteLog("Error GET_INF_ACC(): mer_id null");
                    return "";
                }
                flag=OpenConnectionOracle("FCSCB");
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("FCSCB");
                OracleCommand cmd = new OracleCommand("GET_INF_ACC", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter merid = new OracleParameter("mer_id", mer_id);
                merid.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(merid);

                OracleParameter acc = new OracleParameter("acc_no", OracleType.VarChar, 16);//hoannd edit 05062015 from Nvarchar(20) -> varchar(16)
                acc.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(acc);
                
                cmd.ExecuteNonQuery();

                string acc_no = null;
           
                acc_no = cmd.Parameters[1].Value.ToString();
                if(flag==true)
                    CloseConnectionOracle();
                return acc_no;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error GET_INF_ACC() ," + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();
                return null;
            }


        }


        public bool Insert_Terminal(string ter_id, string ter_gl,string type, string br)
        {
            try
            {
                bool temp = OpenConnectionSQL();
                SqlCommand cmd = new SqlCommand("insert_terminal", connectionSQL);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@TER_ID", ter_id));
                cmd.Parameters.Add(new SqlParameter("@TER_GL", ter_gl.Trim()));
                cmd.Parameters.Add(new SqlParameter("@Type", type));
                cmd.Parameters.Add(new SqlParameter("@Branch", br.Trim()));
                cmd.ExecuteNonQuery();
                if(temp==true)
                    CloseConnectionSQL();
                return true;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Err " + ex.ToString());               
                return false;
            }

        }
        
       public bool Insert_Terminal_FromFCC(string ter_id, string ter_gl, string type, string branch, string makerid, string checkerid, string up_time )
       {
           bool flag = false;
           try
           {
               flag=OpenConnectionSQL();               
               SqlCommand cmd = new SqlCommand("insert_terminal_from_fcc", connectionSQL);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Clear();
               cmd.Parameters.Add(new SqlParameter("@TER_ID", ter_id));
               cmd.Parameters.Add(new SqlParameter("@TER_GL", ter_gl.Trim()));
               cmd.Parameters.Add(new SqlParameter("@Type", type));
               cmd.Parameters.Add(new SqlParameter("@Branch", branch.Trim()));
               cmd.Parameters.Add(new SqlParameter("@MakerID", makerid));
               cmd.Parameters.Add(new SqlParameter("@CheckerID", checkerid));
               cmd.Parameters.Add(new SqlParameter("@Up_time", up_time));
               cmd.ExecuteNonQuery();
               if(flag==true)
                    CloseConnectionSQL();
               return true;
           }
           catch (Exception ex)
           {
               classUpdateTerminalLogWriter.WriteLog("Err at Insert_Terminal_FromFCC" + ex.ToString());
               if (flag == true)
                   CloseConnectionSQL();
               return false;
           }
       }
        public bool insert_merchant_from_fcc(string mer_id, string acc_no, string branch, string makerid, string checkerid, string up_time)
        {
            bool flag = false;
            try
            {
                flag=OpenConnectionSQL();
                SqlCommand cmd = new SqlCommand("insert_merchant_from_fcc", connectionSQL);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@MER_ID", mer_id));
                cmd.Parameters.Add(new SqlParameter("@ACC_NO", acc_no.Trim()));
                cmd.Parameters.Add(new SqlParameter("@Branch", branch.Trim()));
                cmd.Parameters.Add(new SqlParameter("@MakerID", makerid));
                cmd.Parameters.Add(new SqlParameter("@CheckerID", checkerid));
                cmd.Parameters.Add(new SqlParameter("@Up_time", up_time));
                cmd.ExecuteNonQuery();
                if(flag==true)
                    CloseConnectionSQL();
                return true;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error insert_merchant_from_fcc() ," + ex.Message);
                if (flag == true)
                    CloseConnectionSQL();
                return false;
            }
        }
        public bool Update_POS(string ter_id, string br)
        {
            
            try
            {
                bool temp = OpenConnectionSQL();
                SqlCommand cmd = new SqlCommand("update_POS", connectionSQL);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@TER_ID", ter_id));
                cmd.Parameters.Add(new SqlParameter("@Branch", br.Trim()));
                cmd.ExecuteNonQuery();
                if(temp==true)
                    CloseConnectionSQL();
                return temp;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Update_POS() ," + ex.Message);                
                return false;
            }

        }
        public bool Update_ATM(string ter_id, string ter_gl, string br)
        {
            
            try
            {
                bool temp = OpenConnectionSQL();
                SqlCommand cmd = new SqlCommand("update_ATM", connectionSQL);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@TER_ID", ter_id));
                cmd.Parameters.Add(new SqlParameter("@TER_GL", ter_gl.Trim()));
                cmd.Parameters.Add(new SqlParameter("@Branch", br.Trim()));
                cmd.ExecuteNonQuery();
                if(temp==true)
                    CloseConnectionSQL();
                return temp;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Update_ATM ," + ex.Message);
                return false;
            }

        }
        public bool Update_Terminal(string ter_id, string ter_gl,string destid, string br)
        {

            try
            {
            
                bool temp = OpenConnectionSQL();
                SqlCommand cmd = new SqlCommand("update_terminal", connectionSQL);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@TER_ID", ter_id));
                cmd.Parameters.Add(new SqlParameter("@TER_GL",ter_gl.Trim()));
                cmd.Parameters.Add(new SqlParameter("@DESTID", destid));
                cmd.Parameters.Add(new SqlParameter("@Branch", br.Trim()));
                cmd.ExecuteNonQuery();
                if(temp==true)
                    CloseConnectionSQL();
                return temp;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Update_Terminal() ," + ex.Message);
                return false;
            }

        }
        //public bool OpenConnection(string DBName)
        //{
        //    string connectionString = System.Configuration.ConfigurationManager.AppSettings[DBName];
        //    try
        //    {
        //        connection = new OracleConnection(connectionString);
        //        connection.Open();
        //        //hoand rem 25082016
        //        //classDataAccessLogWriter.WriteLog("Open Connection Successful for " + connection.ConnectionString);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        classDataAccessLogWriter.WriteLog("Error OpenConnection(), " + ex.Message);
        //        return false;
        //    }
        //}
       

        public int InsertSMSMessateToEBankGW(string idAlert, string mobile, string message,
                                           char msgstat, string smsType)
        {
            try
            {
                //OpenConnection("EBANK_GW");// mo ket noi CSDL
                OracleCommand cmd = new OracleCommand("sms_scb.PROC_INS_MASTERCARD_SMS", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter idAlert_p = new OracleParameter("id_alert", idAlert);
                idAlert_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(idAlert_p);

                OracleParameter mobile_p = new OracleParameter("mobile", mobile);
                mobile_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(mobile_p);

                OracleParameter message_p = new OracleParameter("message", message);
                message_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(message_p);

                OracleParameter msgstat_p = new OracleParameter("msgstat", msgstat);
                msgstat_p.Direction = ParameterDirection.Input;
                msgstat_p.OracleType = OracleType.Char;
                cmd.Parameters.Add(msgstat_p);

                OracleParameter mc_sms_type_p = new OracleParameter("mc_sms_type", smsType);
                mc_sms_type_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(mc_sms_type_p);

                OracleString rowID;
                int insertedRow = 0;

                insertedRow = cmd.ExecuteOracleNonQuery(out rowID);
                return insertedRow;
            }
            catch (Exception ex)
            {
                classCheckErrLogWriter.WriteLog("Error InsertSMSMessateToEBankGW(), " + ex.Message);
                return 0;
            }


        }
        public int Insert_Send_Email(string sms_type, string sms_detail, string dest_mobile, DateTime get_trans_datetime, string pan, string update_datetime)
        {
            bool flag = false;
            try
            {
                flag = OpenConnectionOracle("CW_DW");
                //OpenConnectionOracle("CW_DW_UAT");// hhhh
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_DW");
                OracleCommand cmd = new OracleCommand("fpt.Insert_Send_Email", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter sms_type_p = new OracleParameter("sms_type_p", sms_type);
                sms_type_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(sms_type_p);

                OracleParameter sms_detail_p = new OracleParameter("sms_detail_p", sms_detail);
                sms_detail_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(sms_detail_p);

                OracleParameter dest_mobile_p = new OracleParameter("dest_mobile_p", dest_mobile);
                dest_mobile_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(dest_mobile_p);

                OracleParameter get_trans_datetime_p = new OracleParameter("get_trans_datetime_p", get_trans_datetime);
                get_trans_datetime_p.Direction = ParameterDirection.Input;
                //get_trans_datetime_p.OracleType = OracleType.DateTime;
                cmd.Parameters.Add(get_trans_datetime_p);

                OracleParameter pan_p = new OracleParameter("pan_p", pan);
                pan_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(pan_p);

                OracleParameter update_datetime_p = new OracleParameter("update_datetime_p", update_datetime);
                update_datetime_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(update_datetime_p);

                OracleString rowID;
                int insertedRow = 0;
                
                insertedRow = cmd.ExecuteOracleNonQuery(out rowID);

                if (flag == true)
                    CloseConnection();// dong ket noi CSDL   
                return insertedRow;

            }
            catch (Exception ex)
            {
                classSendEmailLogWriter.WriteLog("Error Insert_Send_Email()at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL  
                return 0;
            }

        }
       
        public int Insert_Disable_Case(string sms_type, string sms_detail, string dest_mobile, DateTime get_trans_datetime, string pan, string update_datetime)
        {
            bool flag = false;
            try
            {
                flag=OpenConnectionOracle("CW_DW");
                //OpenConnectionOracle("CW_DW_UAT");// hhhh
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnectionOracle("CW_DW");
                OracleCommand cmd = new OracleCommand("fpt.Insert_Disable_Case", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter sms_type_p = new OracleParameter("sms_type_p", sms_type);
                sms_type_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(sms_type_p);

                OracleParameter sms_detail_p = new OracleParameter("sms_detail_p", sms_detail);
                sms_detail_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(sms_detail_p);

                OracleParameter dest_mobile_p = new OracleParameter("dest_mobile_p", dest_mobile);
                dest_mobile_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(dest_mobile_p);

                OracleParameter get_trans_datetime_p = new OracleParameter("get_trans_datetime_p", get_trans_datetime);
                get_trans_datetime_p.Direction = ParameterDirection.Input;
                //get_trans_datetime_p.OracleType = OracleType.DateTime;
                cmd.Parameters.Add(get_trans_datetime_p);

                OracleParameter pan_p = new OracleParameter("pan_p", pan);
                pan_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(pan_p);

                OracleParameter update_datetime_p = new OracleParameter("update_datetime_p", update_datetime);
                update_datetime_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(update_datetime_p);

                OracleString rowID;
                int insertedRow = 0;
               
                insertedRow = cmd.ExecuteOracleNonQuery(out rowID);
                
                if(flag==true)
                    CloseConnection();// dong ket noi CSDL   
                return insertedRow;

            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Insert_Disable_Case()at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL  
                return 0;
            }
           
        }
        public string Get_Max_UpdateTime_SendEmail(string smsType)
        {
            bool flag = false;
            try
            {
                string temp = null;
                //OpenConnectionOracle("CW_DW");// mo ket noi CSDL
                flag = OpenConnectionOracle("CW_DW_STAN");// hhhh
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection("CW_DW_STAN");
                OracleCommand cmd = new OracleCommand("Get_Max_UpdateTime_Send_E", connectionOracle);

                OracleParameter sms_type_p = new OracleParameter("sms_type_p", smsType);
                sms_type_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(sms_type_p);

                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter update_time = new OracleParameter("update_time_p", OracleType.NVarChar, 17);
                update_time.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(update_time);
                
                cmd.ExecuteNonQuery();
                temp = cmd.Parameters["update_time_p"].Value.ToString();
                if (flag == true)
                    CloseConnectionOracle();//hhhh dong ket noi CSDL  
                return temp;

            }
            catch (Exception ex)
            {
                classSendEmailLogWriter.WriteLog("Error Get_Max_UpdateTime_SendEmail() at data access, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();//hhhh dong ket noi CSDL  
                return null;
            }

        }
        public string Get_Max_UpdateTime_Dis(string smsType)
        {
            bool flag = false;
            try
            {
                string temp = null;
                //OpenConnectionOracle("CW_DW");// mo ket noi CSDL
                flag=OpenConnectionOracle("CW_DW_STAN");// hhhh
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection("CW_DW_STAN");
                OracleCommand cmd = new OracleCommand("Get_Max_UpdateTime_Dis", connectionOracle);

                OracleParameter sms_type_p = new OracleParameter("sms_type_p", smsType);
                sms_type_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(sms_type_p);

                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter update_time = new OracleParameter("update_time_p", OracleType.NVarChar, 17);
                update_time.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(update_time);
                
                cmd.ExecuteNonQuery();               
                temp = cmd.Parameters["update_time_p"].Value.ToString();
                if(flag==true)
                    CloseConnectionOracle();//hhhh dong ket noi CSDL  
                return temp;
                
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Get_Max_UpdateTime_DIS() at data access, " + ex.Message);
                if (flag == true)
                    CloseConnectionOracle();//hhhh dong ket noi CSDL  
                return null;
            }

        }
        public bool OpenConnection_New(string DBName, OracleConnection connectionOracle_P)
        {
            string connectionString = System.Configuration.ConfigurationManager.AppSettings[DBName];
            try
            {
                connectionOracle_P = new OracleConnection(connectionString);
                connectionOracle_P.Open();
                //hoand rem 25082016
                //classDataAccessLogWriter.WriteLog("Open Connection Successful for " + connection.ConnectionString);
                return true;
            }
            catch (Exception ex)
            {
                classDataAccessLogWriter.WriteLog("Error OpenConnection_New(), " + ex.Message);
                return false;
            }
        }
        public bool OpenConnection(string DBName)
        {
            string connectionString = System.Configuration.ConfigurationManager.AppSettings[DBName];
            try
            {
                connectionOracle = new OracleConnection(connectionString);
                connectionOracle.Open();
                //hoand rem 25082016
                //classDataAccessLogWriter.WriteLog("Open Connection Successful for " + connection.ConnectionString);
                return true;
            }
            catch (Exception ex)
            {
                classDataAccessLogWriter.WriteLog("Error OpenConnection(), " + ex.Message);
                return false;
            }
        }
        public bool CloseConnection_New(OracleConnection connectionOracle_p)
        {
            try
            {
                connectionOracle_p.Close();
                //hoand rem 25082016
                //classDataAccessLogWriter.WriteLog("Close Connection Successful for " + connection.ConnectionString);
                return true;
            }
            catch (Exception ex)
            {
                classDataAccessLogWriter.WriteLog("Error CloseConnection_New(), " + ex.Message);
                return false;
            }
        }
        public bool CloseConnection()
        {
            try
            {
                connectionOracle.Close();
                //hoand rem 25082016
                //classDataAccessLogWriter.WriteLog("Close Connection Successful for " + connection.ConnectionString);
                return true;
            }
            catch (Exception ex)
            {
                classDataAccessLogWriter.WriteLog("Error CloseConnection(), " + ex.Message);
                return false;
            }
        }
        public DataTable GetSendEmail_MaiCard(string updateDateTime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                flag = OpenConnection("CW_IM_STAN");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection("CW_IM_STAN");
                //OpenConnection("CW_IM_UAT");            
                OracleCommand cmd = new OracleCommand("Get_Send_Email_MainCard", connectionOracle);
                //OracleCommand cmd = new OracleCommand("Get_Send_Email_MainCard_T", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updateTime = new OracleParameter("update_time", updateDateTime);
                updateTime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updateTime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classSendEmailLogWriter.WriteLog("Error GetSendEmail_MaiCard()at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL  
                data.Clear();
                return data;
            }

        }
        public DataTable GetSendEmail(string updateDateTime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                flag = OpenConnection("CCPS_DW");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection("CCPS_DW");
                //flag = OpenConnection("CCPS_DW_STAN");// mo ket noi CSDL
                //if (connectionOracle.State == ConnectionState.Closed)
                //    flag = OpenConnection("CCPS_DW_STAN");
                //OpenConnection("CW_IM_UAT");            
                OracleCommand cmd = new OracleCommand("Get_Send_Email", connectionOracle);
                //OracleCommand cmd = new OracleCommand("Get_Send_Email_T", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updateTime = new OracleParameter("update_time", updateDateTime);
                updateTime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updateTime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
               
                da.Fill(data);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL   
                return data;
            }
            catch (Exception ex)
            {
                classSendEmailLogWriter.WriteLog("Error GetSendEmail()at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL  
                data.Clear();
                return data;
            }

        }
        public DataTable GetDisableCase_SCare_Change(string updateDateTime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {

                flag = OpenConnection("CW_IM_STAN");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection_New("CW_IM_STAN", connectionOracle);
                //OpenConnection("CW_IM_UAT");            
                OracleCommand cmd = new OracleCommand("Get_DisableCase_SCare_Change", connectionOracle);
                //OracleCommand cmd = new OracleCommand("Get_DisableCase_SCare_Change_T", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updateTime = new OracleParameter("update_time", updateDateTime);
                updateTime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updateTime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);
                if (flag == true)
                    //CloseConnection_New(connectionOracle2);// dong ket noi CSDL   
                    connectionOracle.Close();
                return data;
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error GetDisableCase_SCare_Change()at DataAccess, " + ex.Message);
                if (flag == true)
                    //CloseConnection_New(connectionOracle2);// dong ket noi CSDL 
                    connectionOracle.Close();
                data.Clear();
                return data;
            }

        }
        public DataTable GetDisableCase_Premier_New(string updateDateTime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {

                flag = OpenConnection("CW_IM_STAN");// mo ket noi CSDL
                //if (connectionOracle1.State == ConnectionState.Closed)
                //if(flag==false)
                //    flag = OpenConnection_New("CW_IM_STAN", connectionOracle);
                //OpenConnection("CW_IM_UAT");            
                OracleCommand cmd = new OracleCommand("Get_DisableCase_Premier_new", connectionOracle);
                //OracleCommand cmd = new OracleCommand("Get_DisableCase_Premier_new_T", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updateTime = new OracleParameter("update_time", updateDateTime);
                updateTime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updateTime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);
                if (flag == true)
                    //CloseConnection_New(connectionOracle1);// dong ket noi CSDL  
                    connectionOracle.Close();
                return data;
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error GetDisableCase_Premier_New()at DataAccess, " + ex.Message);
                if (flag == true)
                    //CloseConnection_New(connectionOracle1);// dong ket noi CSDL  
                    connectionOracle.Close();
                data.Clear();
                return data;
            }

        }
        public DataTable GetDisableCase_SCare_New(string updateDateTime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {

                flag = OpenConnection("CW_IM_STAN");// mo ket noi CSDL
                    if (connectionOracle.State == ConnectionState.Closed)
                        flag = OpenConnection_New("CW_IM_STAN", connectionOracle);
                //OpenConnection("CW_IM_UAT");            
                OracleCommand cmd = new OracleCommand("Get_DisableCase_SCare_new", connectionOracle);
                //OracleCommand cmd = new OracleCommand("Get_DisableCase_SCare_new_T", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updateTime = new OracleParameter("update_time", updateDateTime);
                updateTime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updateTime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);
                if (flag == true)
                    //CloseConnection_New(connectionOracle3);// dong ket noi CSDL   
                    connectionOracle.Close();
                return data;
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error GetDisableCase_SCare_New()at DataAccess, " + ex.Message);
                if (flag == true)
                    //CloseConnection_New(connectionOracle3);// dong ket noi CSDL  
                    connectionOracle.Close();
                data.Clear();
                return data;
            }

        }
        public DataTable GetDisableCase(string updateDateTime)
        {
            DataTable data = new DataTable();
            bool flag = false;
            try
            {
                //OpenConnection("CW_IM");// mo ket noi CSDL
                flag = OpenConnection("CW_IM_STAN");// mo ket noi CSDL
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection_New("CW_IM_STAN", connectionOracle);
                //OpenConnection("CW_IM_UAT");            
                OracleCommand cmd = new OracleCommand("Get_DisableCase", connectionOracle);
                //OracleCommand cmd = new OracleCommand("Get_DisableCase_T", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter updateTime = new OracleParameter("update_time", updateDateTime);
                updateTime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(updateTime);

                OracleParameter results = new OracleParameter("sys_cursor", OracleType.Cursor);
                results.Direction = ParameterDirection.Output;
                
                cmd.Parameters.Add(results);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                
                da.Fill(data);
                if (flag == true)
                    //CloseConnection_New(connectionOracle4);// dong ket noi CSDL   
                    connectionOracle.Close();
                return data;
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error GetDisableCase()at DataAccess, " + ex.Message);
                if (flag == true)
                    //CloseConnection_New(connectionOracle4);// dong ket noi CSDL  
                    connectionOracle.Close();
                data.Clear();
                return data;
            }

        }
        public int Update_Status_CASE_CARD(string key_id)
        {
            bool flag = false;
            try
            {
                flag = OpenConnection("CW_IM");//                 
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection("CW_IM");//
                //OpenConnection("CW_IM_UAT");
                OracleCommand cmd = new OracleCommand("Update_DISABLE_CASE_CARD", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter keyID = new OracleParameter("key_p", key_id);
                keyID.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(keyID);

                OracleString rowID;
                int insertedRow = 0;
                
                insertedRow = cmd.ExecuteOracleNonQuery(out rowID);
               
                if(flag==true)
                    CloseConnection();// dong ket noi CSDL   
                return insertedRow;
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Update_DISABLE_CASE_CARD() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL   
                return 0;
            }

        }
        public int Update_DIS_Rep_Renew_up_dow(string pan, string user, string date_time)
        {
            bool flag = false;
            try
            {
                flag = OpenConnection("CW_IM");// 
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection("CW_IM");// 
                //OpenConnection("CW_IM_UAT");// 
                OracleCommand cmd = new OracleCommand("Update_DIS_Rep_Renew_up_dow", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter panP = new OracleParameter("pan_p", pan);
                panP.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(panP);

                OracleParameter user_p = new OracleParameter("user_p", user);
                user_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(user_p);

                OracleParameter date_p = new OracleParameter("date_p", date_time);
                date_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(date_p);

                OracleString rowID;
                int insertedRow = 0;
                
                insertedRow = cmd.ExecuteOracleNonQuery(out rowID);
                
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL 
                return insertedRow;
                
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Update_DISABLE_CardReplacement() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL   
                return 0;
            }

        }
        public int Update_DISABLE_Premier(string pro_no, string create_date, string user_lv1)
        {
            bool flag = false;
            try
            {
                flag = OpenConnection("CW_IM");// 
                //OpenConnection("CW_IM_UAT");//
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection("CW_IM");// 
                OracleCommand cmd = new OracleCommand("Update_DISABLE_Premier", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter Pro_no_p = new OracleParameter("Pro_no_p", pro_no);
                Pro_no_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(Pro_no_p);

                OracleParameter date_time_p = new OracleParameter("date_time_p", create_date);
                date_time_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(date_time_p);

                OracleParameter user_id_p = new OracleParameter("user_id_p", user_lv1);
                user_id_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(user_id_p);


                OracleString rowID;
                int insertedRow = 0;

                insertedRow = cmd.ExecuteOracleNonQuery(out rowID);

                if (flag == true)
                    CloseConnection();// dong ket noi CSDL   
                return insertedRow;
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Update_DISABLE_Premier() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL   
                return 0;
            }

        }
        public int Update_DISABLE_SCare(string pro_no, string create_date, string user_lv1)
        {
            bool flag = false;
            try
            {
                flag = OpenConnection("CW_IM");// 
                //OpenConnection("CW_IM_UAT");//
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection("CW_IM");// 
                OracleCommand cmd = new OracleCommand("Update_DISABLE_SCare", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter Pro_no_p = new OracleParameter("Pro_no_p", pro_no);
                Pro_no_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(Pro_no_p);

                OracleParameter date_time_p = new OracleParameter("date_time_p", create_date);
                date_time_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(date_time_p);

                OracleParameter user_id_p = new OracleParameter("user_id_p", user_lv1);
                user_id_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(user_id_p);


                OracleString rowID;
                int insertedRow = 0;

                insertedRow = cmd.ExecuteOracleNonQuery(out rowID);

                if (flag == true)
                    CloseConnection();// dong ket noi CSDL   
                return insertedRow;
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Update_DISABLE_SCare() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL   
                return 0;
            }

        }
        public int Update_DISABLE_SpecialLimit(string pan, string create_date, string user_lv1)
        {
            bool flag = false;
            try
            {
                flag=OpenConnection("CW_IM");// 
                //OpenConnection("CW_IM_UAT");//
                if (connectionOracle.State == ConnectionState.Closed)
                    flag = OpenConnection("CW_IM");// 
                OracleCommand cmd = new OracleCommand("Update_DISABLE_SpecialLimit", connectionOracle);
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter pan_p = new OracleParameter("pan_p", pan);
                pan_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(pan_p);

                OracleParameter date_time_p = new OracleParameter("date_time_p", create_date);
                date_time_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(date_time_p);

                OracleParameter user_id_p = new OracleParameter("user_id_p", user_lv1);
                user_id_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(user_id_p);


                OracleString rowID;
                int insertedRow = 0;
               
                insertedRow = cmd.ExecuteOracleNonQuery(out rowID);
                
                if(flag==true)
                    CloseConnection();// dong ket noi CSDL   
                return insertedRow;
            }
            catch (Exception ex)
            {
                classDisableCaseLogWriter.WriteLog("Error Update_DISABLE_SpecialLimit() at DataAccess, " + ex.Message);
                if (flag == true)
                    CloseConnection();// dong ket noi CSDL   
                return 0;
            }

        }
    }

   
}
