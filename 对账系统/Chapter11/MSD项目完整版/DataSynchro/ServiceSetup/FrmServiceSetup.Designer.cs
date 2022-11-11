namespace ServiceSetup
{
    partial class FrmServiceSetup
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGetStatus = new System.Windows.Forms.Button();
            this.btnStartOrEnd = new System.Windows.Forms.Button();
            this.btnInstallOrUninstall = new System.Windows.Forms.Button();
            this.lblServiceName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.gbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.lblMsg);
            this.gbMain.Controls.Add(this.label3);
            this.gbMain.Controls.Add(this.btnGetStatus);
            this.gbMain.Controls.Add(this.btnStartOrEnd);
            this.gbMain.Controls.Add(this.btnInstallOrUninstall);
            this.gbMain.Controls.Add(this.lblServiceName);
            this.gbMain.Controls.Add(this.label1);
            this.gbMain.Location = new System.Drawing.Point(26, 25);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(746, 387);
            this.gbMain.TabIndex = 0;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "数据同步程序管理";
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(267, 306);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 12);
            this.lblMsg.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 307);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "服务状态";
            // 
            // btnGetStatus
            // 
            this.btnGetStatus.Location = new System.Drawing.Point(430, 166);
            this.btnGetStatus.Name = "btnGetStatus";
            this.btnGetStatus.Size = new System.Drawing.Size(75, 23);
            this.btnGetStatus.TabIndex = 4;
            this.btnGetStatus.Text = "获取状态";
            this.btnGetStatus.UseVisualStyleBackColor = true;
            this.btnGetStatus.Click += new System.EventHandler(this.btnGetStatus_Click);
            // 
            // btnStartOrEnd
            // 
            this.btnStartOrEnd.Location = new System.Drawing.Point(234, 166);
            this.btnStartOrEnd.Name = "btnStartOrEnd";
            this.btnStartOrEnd.Size = new System.Drawing.Size(75, 23);
            this.btnStartOrEnd.TabIndex = 3;
            this.btnStartOrEnd.Text = "启动服务";
            this.btnStartOrEnd.UseVisualStyleBackColor = true;
            this.btnStartOrEnd.Click += new System.EventHandler(this.btnStartOrEnd_Click);
            // 
            // btnInstallOrUninstall
            // 
            this.btnInstallOrUninstall.Location = new System.Drawing.Point(73, 167);
            this.btnInstallOrUninstall.Name = "btnInstallOrUninstall";
            this.btnInstallOrUninstall.Size = new System.Drawing.Size(75, 23);
            this.btnInstallOrUninstall.TabIndex = 2;
            this.btnInstallOrUninstall.Text = "安装服务";
            this.btnInstallOrUninstall.UseVisualStyleBackColor = true;
            this.btnInstallOrUninstall.Click += new System.EventHandler(this.btnInstallOrUninstall_Click);
            // 
            // lblServiceName
            // 
            this.lblServiceName.AutoSize = true;
            this.lblServiceName.Location = new System.Drawing.Point(146, 79);
            this.lblServiceName.Name = "lblServiceName";
            this.lblServiceName.Size = new System.Drawing.Size(65, 12);
            this.lblServiceName.TabIndex = 1;
            this.lblServiceName.Text = "B2CSynchro";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务名称";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // FrmServiceSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 452);
            this.Controls.Add(this.gbMain);
            this.Name = "FrmServiceSetup";
            this.Text = "FrmServiceSetup";
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnStartOrEnd;
        private System.Windows.Forms.Button btnInstallOrUninstall;
        private System.Windows.Forms.Label lblServiceName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnGetStatus;
    }
}

