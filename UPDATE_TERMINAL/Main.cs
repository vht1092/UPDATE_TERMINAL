using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
//using Tamir.SharpSsh;
//using Tamir.Streams;



namespace UPDATE_TERMINAL
{
    public partial class UPDATE_TERMINAL : Form
    {

        Thread _threadDisableCase;
        Thread _threadSendEmail;
        Thread _threadUpdateTerminal;
        Thread _threadCheckErr;

        public UPDATE_TERMINAL()
        {
            InitializeComponent();
            InitializeControl();        
            InitializeThread();
            classDataAccess dataAccess = new classDataAccess();
        }
        private void InitializeThread()
        {
            _threadUpdateTerminal = new Thread(new ThreadStart(classUpdateTerminal.RunService));
            _threadCheckErr = new Thread(new ThreadStart(classCheckErr.RunService));
            _threadDisableCase = new Thread(new ThreadStart(classDisableCase.RunService));
            _threadSendEmail = new Thread(new ThreadStart(classSendEmail.RunService));
            
        }

        private void InitializeControl()
        {
            btnStopUpdateTerminal.Enabled = false;
            ckbUpdateTerminal.Checked = false;
            btnStopDisableCase.Enabled = false;
            ckbDisableCase.Checked = false;
            btnStopSendEmail.Enabled = false;
            ckbSendEmail.Checked = false;

            btnStopCheckErr.Enabled = false;
        }
       
        private void btnPauseUpdateTerminal_Click(object sender, EventArgs e)
        {

        }

        private void btnStartUpdateTerminal_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUpdateTerminalBeginDateTime.Text) == false)
            {
                try
                {
                    string updateTime = txtUpdateTerminalBeginDateTime.Text;
                    DateTime updateDT = new DateTime();
                    if (DateTime.TryParse(updateTime, out updateDT) == false || updateTime.Length < 14)
                    {
                        MessageBox.Show("Thời gian bắt đầu không đúng kiểu dữ liệu!");
                        return;
                    }
                    classUpdateTerminal.updatedatatimeMer = updateTime.Substring(0, 4) + updateTime.Substring(5, 2) + updateTime.Substring(8, 2)
                    + updateTime.Substring(11, 2) + updateTime.Substring(14, 2) + updateTime.Substring(17, 2);// +updateTime.Substring(20, 3);
                    classUpdateTerminal.updatedatatimeTer = updateTime.Substring(0, 4) + updateTime.Substring(5, 2) + updateTime.Substring(8, 2)
                    + updateTime.Substring(11, 2) + updateTime.Substring(14, 2) + updateTime.Substring(17, 2);// +updateTime.Substring(20, 3);
                }
                catch (Exception ex)
                {
                    classUpdateTerminalLogWriter.WriteLog(ex.Message);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc bắt đầu với thời gian: " +
                    txtUpdateTerminalBeginDateTime.Text, "Cảnh báo", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;
            }
            else
            {
                if (ckbUpdateTerminal.Checked == true)
                {
                    MessageBox.Show("Thời gian bắt đầu không đúng kiểu dữ liệu!");
                    return;
                }
            }
            classUpdateTerminalLogWriter.WriteLog("Start service Update Terminal");
            btnStartUpdateTerminal.Enabled = false;
            btnStopUpdateTerminal.Enabled = true;
            switch (_threadUpdateTerminal.ThreadState)
            {
                case ThreadState.Unstarted:
                    _threadUpdateTerminal.Start();
                    break;
                case ThreadState.Suspended:
                    _threadUpdateTerminal.Resume();
                    break;
            }
        }

        private void btnStartCheckErr_Click(object sender, EventArgs e)
        {
            classCheckErrLogWriter.WriteLog("Start Service Check Err");
            btnStartCheckErr.Enabled = false;
            btnStopCheckErr.Enabled = true;
            //classReminderPayment1LogWriter.OpenFileWriter();
            switch (_threadCheckErr.ThreadState)
            {
                case ThreadState.Unstarted:
                    _threadCheckErr.Start();
                    break;
                case ThreadState.Suspended:
                    _threadCheckErr.Resume();
                    break;
            }
        }

        private void btnResumeUpdateTerminal_Click(object sender, EventArgs e)
        {

        }

        private void btnStopUpdateTerminal_Click(object sender, EventArgs e)
        {

            _threadUpdateTerminal.Suspend();

            classUpdateTerminalLogWriter.WriteLog("Stop Service Update Terminal");

            btnStopUpdateTerminal.Enabled = false;
            btnStartUpdateTerminal.Enabled = true;
        }

        private void btnStopCheckErr_Click(object sender, EventArgs e)
        {
            _threadCheckErr.Suspend();
            classCheckErrLogWriter.WriteLog("Stop Service Check Err");
            btnStopCheckErr.Enabled = false;
            btnStartCheckErr.Enabled = true;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void btnStartSendEmail_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSendEmailBeginDateTime.Text) == false)
            {
                try
                {
                    classSendEmailLogWriter.WriteLog("Start Service Send Email for Approval Card");
                    string updateTime = txtSendEmailBeginDateTime.Text;
                    DateTime updateDT = new DateTime();
                    if (DateTime.TryParse(updateTime, out updateDT) == false || updateTime.Length < 23)
                    {
                        MessageBox.Show("Thời gian bắt đầu không đúng kiểu dữ liệu!");
                        return;
                    }
                    classSendEmail._updateDateTime = updateTime.Substring(0, 4) + updateTime.Substring(5, 2) + updateTime.Substring(8, 2)
                            + updateTime.Substring(11, 2) + updateTime.Substring(14, 2) + updateTime.Substring(17, 2) + updateTime.Substring(20, 3);
                }
                catch (Exception ex)
                {
                    classSendEmailLogWriter.WriteLog(ex.Message);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc bắt đầu với thời gian: " +
                        txtSendEmailBeginDateTime.Text, "Cảnh báo", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;


            }
            else
            {
                if (ckbSendEmail.Checked == true)
                {
                    MessageBox.Show("Thời gian bắt đầu không đúng kiểu dữ liệu!");
                    return;
                }
            }
            btnStartSendEmail.Enabled = false;
            btnStopSendEmail.Enabled = true;
            switch (_threadSendEmail.ThreadState)
            {
                case ThreadState.Unstarted:
                    _threadSendEmail.Start();
                    break;
                case ThreadState.Suspended:
                    _threadSendEmail.Resume();
                    break;
            }
        }

        private void btnStartDisableCase_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDisableCaseBeginDateTime.Text) == false)
            {
                try
                {
                    classDisableCaseLogWriter.WriteLog("Start Service Disable Case");
                    string updateTime = txtDisableCaseBeginDateTime.Text;
                    DateTime updateDT = new DateTime();
                    if (DateTime.TryParse(updateTime, out updateDT) == false || updateTime.Length < 23)
                    {
                        MessageBox.Show("Thời gian bắt đầu không đúng kiểu dữ liệu!");
                        return;
                    }
                    classDisableCase._updateDateTime = updateTime.Substring(0, 4) + updateTime.Substring(5, 2) + updateTime.Substring(8, 2)
                            + updateTime.Substring(11, 2) + updateTime.Substring(14, 2) + updateTime.Substring(17, 2) + updateTime.Substring(20, 3);
                }
                catch (Exception ex)
                {
                    classDisableCaseLogWriter.WriteLog(ex.Message);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc bắt đầu với thời gian: " +
                        txtDisableCaseBeginDateTime.Text, "Cảnh báo", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;


            }
            else
            {
                if (ckbDisableCase.Checked == true)
                {
                    MessageBox.Show("Thời gian bắt đầu không đúng kiểu dữ liệu!");
                    return;
                }
            }
            btnStartDisableCase.Enabled = false;
            btnStopDisableCase.Enabled = true;
            switch (_threadDisableCase.ThreadState)
            {
                case ThreadState.Unstarted:
                    _threadDisableCase.Start();
                    break;
                case ThreadState.Suspended:
                    _threadDisableCase.Resume();
                    break;
            }
        }

        private void btnStopDisableCase_Click(object sender, EventArgs e)
        {
            _threadDisableCase.Suspend();
            classDisableCaseLogWriter.WriteLog("Stop Service Disable Case");
            btnStartDisableCase.Enabled = true;
            btnStopDisableCase.Enabled = false;
        }

        private void btnPauseCheckErr_Click(object sender, EventArgs e)
        {

        }

        private void btnPauseDisableCase_Click(object sender, EventArgs e)
        {

        }

        private void btnPauseSendEmail_Click(object sender, EventArgs e)
        {

        }

        private void btnResumeDisableCase_Click(object sender, EventArgs e)
        {

        }

        private void btnResumeSendEmail_Click(object sender, EventArgs e)
        {

        }

        private void btnStopSendEmail_Click(object sender, EventArgs e)
        {
            _threadSendEmail.Suspend();
            classSendEmailLogWriter.WriteLog("Stop Service Send Email for Approval Card");
            btnStartSendEmail.Enabled = true;
            btnStopSendEmail.Enabled = false;
        }
            
    }
}