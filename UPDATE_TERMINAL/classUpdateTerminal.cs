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
    class classUpdateTerminal
    {
        public static bool exitThread = false;
        public static string updatedatatimeTer = null;
        public static string updatedatatimeMer = null;
        public static classDataAccess dataAccess = new classDataAccess();

        
        public static void RunService()
        {
            int minute = 0;
            int value = 0;
            int result_ter = 0;
            int result_mer = 0;
            DataTable table_ter = new DataTable();
            DataTable table_mer = new DataTable();
            value = classUtilities.GetIntValueFromConfig("UpdateTerminal_Minute");
            while (exitThread == false)
            {
               
                minute = DateTime.Now.Minute;
                if (minute % value == 0)
                //if(1==1)
                {
                    try
                    {
                        classUpdateTerminalLogWriter.WriteLog("-------------begin  process-------------");                   
                      
                        dataAccess = new classDataAccess();
                        table_ter.Rows.Clear();

                        table_ter = Get_Inf_Terminal(); //phai update  ter truoc,fix bug thieu ter 06052016
                        if (table_ter.Rows.Count > 0)
                        {
                            result_ter = Update_Terminal(table_ter, "(Ter)");

                        }

                        table_mer = Get_Inf_Merchant();
                        if (table_mer.Rows.Count > 0)
                            result_mer = Update_Merchant(table_mer);                 
                        
                       
                        
                        classUpdateTerminalLogWriter.WriteLog("----------------End Process-----------------");
                        if (value > 2)
                            Thread.Sleep(1000 * (value - 2) * 55);// sleep (value -1) phut de troi qua thoi gian lap lai
                
                    }
                    catch(Exception ex)
                    {
                        classUpdateTerminalLogWriter.WriteLog("Error RunService(), " + ex.Message);
                    }
                }
                Thread.Sleep(1000 * 55);
            }

        }
        public static int Update_Merchant(DataTable table)
        {
            int count_ter = 0;
            int count_mer = 0;
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    if (dataAccess.CHECK_USED_MER(row.ItemArray[0].ToString()) <= 0) // merchant ko duoc gan voi terminal
                    {
                        dataAccess.insert_merchant_from_fcc(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[2].ToString(),
                        row.ItemArray[4].ToString(), row.ItemArray[5].ToString(), row.ItemArray[3].ToString());
                        count_mer++;                        
                    }
                    else // merchant dang su dung
                    {
                        DataTable ter_update = dataAccess.GET_INF_TERMINAL_FROM_MER(row.ItemArray[0].ToString());
                        Update_Terminal(ter_update,"(Mer)");

                        dataAccess.insert_merchant_from_fcc(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[2].ToString(),
                        row.ItemArray[4].ToString(), row.ItemArray[5].ToString(), row.ItemArray[3].ToString());
                        count_ter++;
                        classUpdateTerminalLogWriter.WriteLog("Update Merchant: " + row.ItemArray[0].ToString()+" " + DateTime.Now.ToString());

                    }

                }
                //if(count_mer > 0)
                    //classUpdateTerminalLogWriter.WriteLog("Insert " + count_mer + " Merchant :" + DateTime.Now.ToString());
                //if (count_ter > 0)
                
                    //classUpdateTerminalLogWriter.WriteLog("Update " + count_ter + " Terminal from Merchant :" + DateTime.Now.ToString());
                 
                
                return count_ter;
                
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Update_Merchant(), " + ex.Message);
                return 0;
            }
            
        }
        public static int Update_Terminal(DataTable table,string dest)
        {
            int count_ATM = 0;
            int count_POS = 0;
            try
            {
                
                foreach (DataRow row in table.Rows)
                {
                    if (dataAccess.Check_Exist_Ter(row.ItemArray[0].ToString()) == 1)//terminal da ton tai
                    {
                        if (row.ItemArray[4].ToString() == "ATM")
                        {
                            bool temp = dataAccess.Update_Terminal(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(),"FCC", row.ItemArray[5].ToString());
                            if (temp == true)
                            {
                                count_ATM++;
                                dataAccess.Insert_Terminal_FromFCC(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[4].ToString()
                                    , row.ItemArray[5].ToString(), row.ItemArray[7].ToString(), row.ItemArray[8].ToString(), row.ItemArray[6].ToString());
                                classUpdateTerminalLogWriter.WriteLog("Update ATM: " +dest +" " + row.ItemArray[0].ToString() + " " + DateTime.Now.ToString());
                            }
                        }

                        if (row.ItemArray[4].ToString() == "POS")
                        {
                            bool temp =false;
                            string acc_no = dataAccess.GET_INF_ACC(row.ItemArray[9].ToString());
                            if (row.ItemArray[10].ToString() == "C")
                            {
                                string acc_temp="''" + acc_no + "''";
                                temp = dataAccess.Update_Terminal(row.ItemArray[0].ToString(), acc_temp, "CLOSED", row.ItemArray[5].ToString());
                                if (temp == true)
                                {
                                    count_POS++;
                                    dataAccess.Insert_Terminal_FromFCC(row.ItemArray[0].ToString(), acc_no, row.ItemArray[4].ToString()
                                        , row.ItemArray[5].ToString(), row.ItemArray[7].ToString(), row.ItemArray[8].ToString(), row.ItemArray[6].ToString());
                                    classUpdateTerminalLogWriter.WriteLog("Close POS: " + dest + " " + row.ItemArray[0].ToString() + " " + DateTime.Now.ToString());
                                }
                            }
                            else
                            {
                                temp = dataAccess.Update_Terminal(row.ItemArray[0].ToString(), acc_no,"FCC", row.ItemArray[5].ToString());
                                if (temp == true)
                                {
                                    count_POS++;
                                    dataAccess.Insert_Terminal_FromFCC(row.ItemArray[0].ToString(), acc_no, row.ItemArray[4].ToString()
                                        , row.ItemArray[5].ToString(), row.ItemArray[7].ToString(), row.ItemArray[8].ToString(), row.ItemArray[6].ToString());
                                    classUpdateTerminalLogWriter.WriteLog("Update POS: " + dest + " " + row.ItemArray[0].ToString() + " " + DateTime.Now.ToString());
                                }
                            }
                            
                        }
                    }
                    else//teminal chua ton tai
                    {
                        if (row.ItemArray[4].ToString() == "ATM")
                        {
                            bool temp2 = dataAccess.Insert_Terminal(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[4].ToString(), row.ItemArray[5].ToString());
                            if (temp2 == true)
                            {
                                count_ATM++;
                                dataAccess.Insert_Terminal_FromFCC(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[4].ToString()
                                        , row.ItemArray[5].ToString(), row.ItemArray[7].ToString(), row.ItemArray[8].ToString(), row.ItemArray[6].ToString());
                                classUpdateTerminalLogWriter.WriteLog("Insert ATM: " + dest + " " + row.ItemArray[0].ToString() + " " + DateTime.Now.ToString());
                            }
                        }
                        if (row.ItemArray[4].ToString() == "POS")
                        {
                            string acc_no = dataAccess.GET_INF_ACC(row.ItemArray[9].ToString());
                            string type_P = "CMS";
                            bool temp2 = dataAccess.Insert_Terminal(row.ItemArray[0].ToString(), acc_no, type_P, row.ItemArray[5].ToString());
                            if (temp2 == true)
                            {
                                count_POS++;
                                dataAccess.Insert_Terminal_FromFCC(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[4].ToString()
                                    , row.ItemArray[5].ToString(), row.ItemArray[7].ToString(), row.ItemArray[8].ToString(), row.ItemArray[6].ToString());
                                classUpdateTerminalLogWriter.WriteLog("Insert POS: " +dest +" "+ row.ItemArray[0].ToString() + " " + DateTime.Now.ToString());
                            }
                        }
                    }
                }

                //if(count_ATM > 0)
                    //classUpdateTerminalLogWriter.WriteLog("Update " + count_ATM + " ATM :" + DateTime.Now.ToString() );
                //if (count_POS > 0)
                    //classUpdateTerminalLogWriter.WriteLog("Update " + count_POS + " POS :" + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Update_Terminal(), " + ex.Message);
                return 0;
            }
            return (count_POS + count_ATM);
        }
        public static DataTable Get_Inf_Terminal()
        {
            DataTable table = new DataTable();
            try
            {
                string maxUpdatetime = null;                
                if (string.IsNullOrEmpty(updatedatatimeTer) == false)
                {
                    maxUpdatetime = updatedatatimeTer;
                    updatedatatimeTer = null;
                }
                else
                {
                    string max_time = null;
                    max_time = dataAccess.Get_Max_UpdateTime_Ter();
                    if (max_time != null)
                        maxUpdatetime = max_time.ToString();
                }
                if (string.IsNullOrEmpty(maxUpdatetime) == false)
                {
                    
                    long MaxUpdateTime_T = long.Parse(maxUpdatetime);
                    table = dataAccess.Get_inf_ter(maxUpdatetime);
                }
                 return table;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Get_Inf_Terminal(), " + ex.Message);
                table.Clear();
                return table;
            }
                
           
           
        }
        public static DataTable Get_Inf_Merchant()
        {
            DataTable table = new DataTable();
            try
            {
                string maxUpdatetime = null;
                if (string.IsNullOrEmpty(updatedatatimeMer) == false)
                {
                    maxUpdatetime = updatedatatimeMer;
                    updatedatatimeMer = null;
                }
                else
                {
                    string max_time =null;
                    max_time = dataAccess.Get_Max_UpdateTime_Mer();
                    if (max_time != null)
                        maxUpdatetime = max_time.ToString();
                   
                }
                if (string.IsNullOrEmpty(maxUpdatetime) == false)
                {
                   
                        long MaxUpdateTime_T = long.Parse(maxUpdatetime);
                        table = dataAccess.Get_inf_mer(maxUpdatetime);
                        //table = dataAccess.Get_inf_mer_train(maxUpdatetime);          
                        
                }
                return table;
            }
            catch (Exception ex)
            {
                classUpdateTerminalLogWriter.WriteLog("Error Get_Inf_Merchant(), " + ex.Message);
                table.Clear();
                return table;
            }
        }
    }
}
