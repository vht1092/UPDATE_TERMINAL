namespace UPDATE_TERMINAL
{
    partial class UPDATE_TERMINAL
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label11 = new System.Windows.Forms.Label();
            this.txtUpdateTerminalBeginDateTime = new System.Windows.Forms.TextBox();
            this.ckbUpdateTerminal = new System.Windows.Forms.CheckBox();
            this.btnResumeUpdateTerminal = new System.Windows.Forms.Button();
            this.btnPauseUpdateTerminal = new System.Windows.Forms.Button();
            this.btnStopUpdateTerminal = new System.Windows.Forms.Button();
            this.btnStartUpdateTerminal = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnResumeCheckErr = new System.Windows.Forms.Button();
            this.btnPauseCheckErr = new System.Windows.Forms.Button();
            this.btnStopCheckErr = new System.Windows.Forms.Button();
            this.btnStartCheckErr = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDisableCaseBeginDateTime = new System.Windows.Forms.TextBox();
            this.ckbDisableCase = new System.Windows.Forms.CheckBox();
            this.btnResumeDisableCase = new System.Windows.Forms.Button();
            this.btnPauseDisableCase = new System.Windows.Forms.Button();
            this.btnStopDisableCase = new System.Windows.Forms.Button();
            this.btnStartDisableCase = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSendEmailBeginDateTime = new System.Windows.Forms.TextBox();
            this.ckbSendEmail = new System.Windows.Forms.CheckBox();
            this.btnResumeSendEmail = new System.Windows.Forms.Button();
            this.btnPauseSendEmail = new System.Windows.Forms.Button();
            this.btnStopSendEmail = new System.Windows.Forms.Button();
            this.btnStartSendEmail = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(544, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 13);
            this.label11.TabIndex = 52;
            this.label11.Text = "(Thời Gian Bắt Đầu)";
            // 
            // txtUpdateTerminalBeginDateTime
            // 
            this.txtUpdateTerminalBeginDateTime.Location = new System.Drawing.Point(408, 45);
            this.txtUpdateTerminalBeginDateTime.Name = "txtUpdateTerminalBeginDateTime";
            this.txtUpdateTerminalBeginDateTime.Size = new System.Drawing.Size(130, 20);
            this.txtUpdateTerminalBeginDateTime.TabIndex = 51;
            // 
            // ckbUpdateTerminal
            // 
            this.ckbUpdateTerminal.AutoSize = true;
            this.ckbUpdateTerminal.Location = new System.Drawing.Point(343, 48);
            this.ckbUpdateTerminal.Name = "ckbUpdateTerminal";
            this.ckbUpdateTerminal.Size = new System.Drawing.Size(60, 17);
            this.ckbUpdateTerminal.TabIndex = 50;
            this.ckbUpdateTerminal.Text = "Cài đặt";
            this.ckbUpdateTerminal.UseVisualStyleBackColor = true;
            // 
            // btnResumeUpdateTerminal
            // 
            this.btnResumeUpdateTerminal.Enabled = false;
            this.btnResumeUpdateTerminal.Location = new System.Drawing.Point(170, 46);
            this.btnResumeUpdateTerminal.Name = "btnResumeUpdateTerminal";
            this.btnResumeUpdateTerminal.Size = new System.Drawing.Size(75, 23);
            this.btnResumeUpdateTerminal.TabIndex = 49;
            this.btnResumeUpdateTerminal.Text = "Resume";
            this.btnResumeUpdateTerminal.UseVisualStyleBackColor = true;
            this.btnResumeUpdateTerminal.Click += new System.EventHandler(this.btnResumeUpdateTerminal_Click);
            // 
            // btnPauseUpdateTerminal
            // 
            this.btnPauseUpdateTerminal.Enabled = false;
            this.btnPauseUpdateTerminal.Location = new System.Drawing.Point(91, 46);
            this.btnPauseUpdateTerminal.Name = "btnPauseUpdateTerminal";
            this.btnPauseUpdateTerminal.Size = new System.Drawing.Size(75, 23);
            this.btnPauseUpdateTerminal.TabIndex = 48;
            this.btnPauseUpdateTerminal.Text = "Pause";
            this.btnPauseUpdateTerminal.UseVisualStyleBackColor = true;
            this.btnPauseUpdateTerminal.Click += new System.EventHandler(this.btnPauseUpdateTerminal_Click);
            // 
            // btnStopUpdateTerminal
            // 
            this.btnStopUpdateTerminal.Location = new System.Drawing.Point(250, 46);
            this.btnStopUpdateTerminal.Name = "btnStopUpdateTerminal";
            this.btnStopUpdateTerminal.Size = new System.Drawing.Size(75, 23);
            this.btnStopUpdateTerminal.TabIndex = 47;
            this.btnStopUpdateTerminal.Text = "Stop";
            this.btnStopUpdateTerminal.UseVisualStyleBackColor = true;
            this.btnStopUpdateTerminal.Click += new System.EventHandler(this.btnStopUpdateTerminal_Click);
            // 
            // btnStartUpdateTerminal
            // 
            this.btnStartUpdateTerminal.Location = new System.Drawing.Point(12, 46);
            this.btnStartUpdateTerminal.Name = "btnStartUpdateTerminal";
            this.btnStartUpdateTerminal.Size = new System.Drawing.Size(75, 23);
            this.btnStartUpdateTerminal.TabIndex = 46;
            this.btnStartUpdateTerminal.Text = "Start";
            this.btnStartUpdateTerminal.UseVisualStyleBackColor = true;
            this.btnStartUpdateTerminal.Click += new System.EventHandler(this.btnStartUpdateTerminal_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(405, 71);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(142, 13);
            this.label12.TabIndex = 53;
            this.label12.Text = "(YYYY/MM/DD HH:MM:SS)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "Update Terminal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 63;
            this.label2.Text = "Check_Err_SMS";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnResumeCheckErr
            // 
            this.btnResumeCheckErr.Enabled = false;
            this.btnResumeCheckErr.Location = new System.Drawing.Point(170, 159);
            this.btnResumeCheckErr.Name = "btnResumeCheckErr";
            this.btnResumeCheckErr.Size = new System.Drawing.Size(75, 23);
            this.btnResumeCheckErr.TabIndex = 58;
            this.btnResumeCheckErr.Text = "Resume";
            this.btnResumeCheckErr.UseVisualStyleBackColor = true;
            // 
            // btnPauseCheckErr
            // 
            this.btnPauseCheckErr.Enabled = false;
            this.btnPauseCheckErr.Location = new System.Drawing.Point(91, 159);
            this.btnPauseCheckErr.Name = "btnPauseCheckErr";
            this.btnPauseCheckErr.Size = new System.Drawing.Size(75, 23);
            this.btnPauseCheckErr.TabIndex = 57;
            this.btnPauseCheckErr.Text = "Pause";
            this.btnPauseCheckErr.UseVisualStyleBackColor = true;
            this.btnPauseCheckErr.Click += new System.EventHandler(this.btnPauseCheckErr_Click);
            // 
            // btnStopCheckErr
            // 
            this.btnStopCheckErr.Location = new System.Drawing.Point(250, 159);
            this.btnStopCheckErr.Name = "btnStopCheckErr";
            this.btnStopCheckErr.Size = new System.Drawing.Size(75, 23);
            this.btnStopCheckErr.TabIndex = 56;
            this.btnStopCheckErr.Text = "Stop";
            this.btnStopCheckErr.UseVisualStyleBackColor = true;
            this.btnStopCheckErr.Click += new System.EventHandler(this.btnStopCheckErr_Click);
            // 
            // btnStartCheckErr
            // 
            this.btnStartCheckErr.Location = new System.Drawing.Point(12, 159);
            this.btnStartCheckErr.Name = "btnStartCheckErr";
            this.btnStartCheckErr.Size = new System.Drawing.Size(75, 23);
            this.btnStartCheckErr.TabIndex = 55;
            this.btnStartCheckErr.Text = "Start";
            this.btnStartCheckErr.UseVisualStyleBackColor = true;
            this.btnStartCheckErr.Click += new System.EventHandler(this.btnStartCheckErr_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 72;
            this.label3.Text = "Disable Case";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(405, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 13);
            this.label4.TabIndex = 71;
            this.label4.Text = "(YYYY/MM/DD HH:MM:SS)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(544, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 70;
            this.label5.Text = "(Thời Gian Bắt Đầu)";
            // 
            // txtDisableCaseBeginDateTime
            // 
            this.txtDisableCaseBeginDateTime.Location = new System.Drawing.Point(408, 102);
            this.txtDisableCaseBeginDateTime.Name = "txtDisableCaseBeginDateTime";
            this.txtDisableCaseBeginDateTime.Size = new System.Drawing.Size(130, 20);
            this.txtDisableCaseBeginDateTime.TabIndex = 69;
            // 
            // ckbDisableCase
            // 
            this.ckbDisableCase.AutoSize = true;
            this.ckbDisableCase.Location = new System.Drawing.Point(343, 105);
            this.ckbDisableCase.Name = "ckbDisableCase";
            this.ckbDisableCase.Size = new System.Drawing.Size(60, 17);
            this.ckbDisableCase.TabIndex = 68;
            this.ckbDisableCase.Text = "Cài đặt";
            this.ckbDisableCase.UseVisualStyleBackColor = true;
            // 
            // btnResumeDisableCase
            // 
            this.btnResumeDisableCase.Enabled = false;
            this.btnResumeDisableCase.Location = new System.Drawing.Point(170, 103);
            this.btnResumeDisableCase.Name = "btnResumeDisableCase";
            this.btnResumeDisableCase.Size = new System.Drawing.Size(75, 23);
            this.btnResumeDisableCase.TabIndex = 67;
            this.btnResumeDisableCase.Text = "Resume";
            this.btnResumeDisableCase.UseVisualStyleBackColor = true;
            this.btnResumeDisableCase.Click += new System.EventHandler(this.btnResumeDisableCase_Click);
            // 
            // btnPauseDisableCase
            // 
            this.btnPauseDisableCase.Enabled = false;
            this.btnPauseDisableCase.Location = new System.Drawing.Point(91, 103);
            this.btnPauseDisableCase.Name = "btnPauseDisableCase";
            this.btnPauseDisableCase.Size = new System.Drawing.Size(75, 23);
            this.btnPauseDisableCase.TabIndex = 66;
            this.btnPauseDisableCase.Text = "Pause";
            this.btnPauseDisableCase.UseVisualStyleBackColor = true;
            this.btnPauseDisableCase.Click += new System.EventHandler(this.btnPauseDisableCase_Click);
            // 
            // btnStopDisableCase
            // 
            this.btnStopDisableCase.Location = new System.Drawing.Point(250, 103);
            this.btnStopDisableCase.Name = "btnStopDisableCase";
            this.btnStopDisableCase.Size = new System.Drawing.Size(75, 23);
            this.btnStopDisableCase.TabIndex = 65;
            this.btnStopDisableCase.Text = "Stop";
            this.btnStopDisableCase.UseVisualStyleBackColor = true;
            this.btnStopDisableCase.Click += new System.EventHandler(this.btnStopDisableCase_Click);
            // 
            // btnStartDisableCase
            // 
            this.btnStartDisableCase.Location = new System.Drawing.Point(12, 103);
            this.btnStartDisableCase.Name = "btnStartDisableCase";
            this.btnStartDisableCase.Size = new System.Drawing.Size(75, 23);
            this.btnStartDisableCase.TabIndex = 64;
            this.btnStartDisableCase.Text = "Start";
            this.btnStartDisableCase.UseVisualStyleBackColor = true;
            this.btnStartDisableCase.Click += new System.EventHandler(this.btnStartDisableCase_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 198);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 13);
            this.label6.TabIndex = 81;
            this.label6.Text = "Send Email Approval Card & Main Card";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(405, 240);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(166, 13);
            this.label7.TabIndex = 80;
            this.label7.Text = "(YYYY/MM/DD HH:MM:SS.XXX)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(544, 218);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 13);
            this.label8.TabIndex = 79;
            this.label8.Text = "(Thời Gian Bắt Đầu)";
            // 
            // txtSendEmailBeginDateTime
            // 
            this.txtSendEmailBeginDateTime.Location = new System.Drawing.Point(408, 214);
            this.txtSendEmailBeginDateTime.Name = "txtSendEmailBeginDateTime";
            this.txtSendEmailBeginDateTime.Size = new System.Drawing.Size(130, 20);
            this.txtSendEmailBeginDateTime.TabIndex = 78;
            // 
            // ckbSendEmail
            // 
            this.ckbSendEmail.AutoSize = true;
            this.ckbSendEmail.Location = new System.Drawing.Point(343, 217);
            this.ckbSendEmail.Name = "ckbSendEmail";
            this.ckbSendEmail.Size = new System.Drawing.Size(60, 17);
            this.ckbSendEmail.TabIndex = 77;
            this.ckbSendEmail.Text = "Cài đặt";
            this.ckbSendEmail.UseVisualStyleBackColor = true;
            // 
            // btnResumeSendEmail
            // 
            this.btnResumeSendEmail.Enabled = false;
            this.btnResumeSendEmail.Location = new System.Drawing.Point(170, 215);
            this.btnResumeSendEmail.Name = "btnResumeSendEmail";
            this.btnResumeSendEmail.Size = new System.Drawing.Size(75, 23);
            this.btnResumeSendEmail.TabIndex = 76;
            this.btnResumeSendEmail.Text = "Resume";
            this.btnResumeSendEmail.UseVisualStyleBackColor = true;
            this.btnResumeSendEmail.Click += new System.EventHandler(this.btnResumeSendEmail_Click);
            // 
            // btnPauseSendEmail
            // 
            this.btnPauseSendEmail.Enabled = false;
            this.btnPauseSendEmail.Location = new System.Drawing.Point(91, 215);
            this.btnPauseSendEmail.Name = "btnPauseSendEmail";
            this.btnPauseSendEmail.Size = new System.Drawing.Size(75, 23);
            this.btnPauseSendEmail.TabIndex = 75;
            this.btnPauseSendEmail.Text = "Pause";
            this.btnPauseSendEmail.UseVisualStyleBackColor = true;
            this.btnPauseSendEmail.Click += new System.EventHandler(this.btnPauseSendEmail_Click);
            // 
            // btnStopSendEmail
            // 
            this.btnStopSendEmail.Location = new System.Drawing.Point(250, 215);
            this.btnStopSendEmail.Name = "btnStopSendEmail";
            this.btnStopSendEmail.Size = new System.Drawing.Size(75, 23);
            this.btnStopSendEmail.TabIndex = 74;
            this.btnStopSendEmail.Text = "Stop";
            this.btnStopSendEmail.UseVisualStyleBackColor = true;
            this.btnStopSendEmail.Click += new System.EventHandler(this.btnStopSendEmail_Click);
            // 
            // btnStartSendEmail
            // 
            this.btnStartSendEmail.Location = new System.Drawing.Point(12, 215);
            this.btnStartSendEmail.Name = "btnStartSendEmail";
            this.btnStartSendEmail.Size = new System.Drawing.Size(75, 23);
            this.btnStartSendEmail.TabIndex = 73;
            this.btnStartSendEmail.Text = "Start";
            this.btnStartSendEmail.UseVisualStyleBackColor = true;
            this.btnStartSendEmail.Click += new System.EventHandler(this.btnStartSendEmail_Click);
            // 
            // UPDATE_TERMINAL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 267);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSendEmailBeginDateTime);
            this.Controls.Add(this.ckbSendEmail);
            this.Controls.Add(this.btnResumeSendEmail);
            this.Controls.Add(this.btnPauseSendEmail);
            this.Controls.Add(this.btnStopSendEmail);
            this.Controls.Add(this.btnStartSendEmail);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDisableCaseBeginDateTime);
            this.Controls.Add(this.ckbDisableCase);
            this.Controls.Add(this.btnResumeDisableCase);
            this.Controls.Add(this.btnPauseDisableCase);
            this.Controls.Add(this.btnStopDisableCase);
            this.Controls.Add(this.btnStartDisableCase);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnResumeCheckErr);
            this.Controls.Add(this.btnPauseCheckErr);
            this.Controls.Add(this.btnStopCheckErr);
            this.Controls.Add(this.btnStartCheckErr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtUpdateTerminalBeginDateTime);
            this.Controls.Add(this.ckbUpdateTerminal);
            this.Controls.Add(this.btnResumeUpdateTerminal);
            this.Controls.Add(this.btnPauseUpdateTerminal);
            this.Controls.Add(this.btnStopUpdateTerminal);
            this.Controls.Add(this.btnStartUpdateTerminal);
            this.Name = "UPDATE_TERMINAL";
            this.Text = "UPDATE TERMINAL";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtUpdateTerminalBeginDateTime;
        private System.Windows.Forms.CheckBox ckbUpdateTerminal;
        private System.Windows.Forms.Button btnResumeUpdateTerminal;
        private System.Windows.Forms.Button btnPauseUpdateTerminal;
        private System.Windows.Forms.Button btnStopUpdateTerminal;
        private System.Windows.Forms.Button btnStartUpdateTerminal;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnResumeCheckErr;
        private System.Windows.Forms.Button btnPauseCheckErr;
        private System.Windows.Forms.Button btnStopCheckErr;
        private System.Windows.Forms.Button btnStartCheckErr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDisableCaseBeginDateTime;
        private System.Windows.Forms.CheckBox ckbDisableCase;
        private System.Windows.Forms.Button btnResumeDisableCase;
        private System.Windows.Forms.Button btnPauseDisableCase;
        private System.Windows.Forms.Button btnStopDisableCase;
        private System.Windows.Forms.Button btnStartDisableCase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSendEmailBeginDateTime;
        private System.Windows.Forms.CheckBox ckbSendEmail;
        private System.Windows.Forms.Button btnResumeSendEmail;
        private System.Windows.Forms.Button btnPauseSendEmail;
        private System.Windows.Forms.Button btnStopSendEmail;
        private System.Windows.Forms.Button btnStartSendEmail;
    }
}

