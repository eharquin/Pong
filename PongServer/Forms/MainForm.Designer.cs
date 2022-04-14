
using PongServer.Util;

namespace PongServer
{
    partial class MainForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grpServeurStatusLog = new System.Windows.Forms.GroupBox();
            this.dgvServeurStatusLog = new System.Windows.Forms.DataGridView();
            this.clmLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.grpServeurOperations = new System.Windows.Forms.GroupBox();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.grpServerControl = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpServeurStatusLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServeurStatusLog)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.grpServeurOperations.SuspendLayout();
            this.grpServerControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grpServeurStatusLog, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1457, 788);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // grpServeurStatusLog
            // 
            this.grpServeurStatusLog.Controls.Add(this.dgvServeurStatusLog);
            this.grpServeurStatusLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpServeurStatusLog.Location = new System.Drawing.Point(203, 3);
            this.grpServeurStatusLog.Name = "grpServeurStatusLog";
            this.grpServeurStatusLog.Size = new System.Drawing.Size(1251, 782);
            this.grpServeurStatusLog.TabIndex = 2;
            this.grpServeurStatusLog.TabStop = false;
            this.grpServeurStatusLog.Text = "Serveur status log";
            // 
            // dgvServeurStatusLog
            // 
            this.dgvServeurStatusLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServeurStatusLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmLevel,
            this.clmId,
            this.clmMessage});
            this.dgvServeurStatusLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServeurStatusLog.Location = new System.Drawing.Point(3, 27);
            this.dgvServeurStatusLog.Name = "dgvServeurStatusLog";
            this.dgvServeurStatusLog.RowHeadersWidth = 62;
            this.dgvServeurStatusLog.RowTemplate.Height = 33;
            this.dgvServeurStatusLog.Size = new System.Drawing.Size(1245, 752);
            this.dgvServeurStatusLog.TabIndex = 2;
            // 
            // clmLevel
            // 
            this.clmLevel.HeaderText = "Level";
            this.clmLevel.MinimumWidth = 8;
            this.clmLevel.Name = "clmLevel";
            this.clmLevel.Width = 150;
            // 
            // clmId
            // 
            this.clmId.HeaderText = "Id";
            this.clmId.MinimumWidth = 8;
            this.clmId.Name = "clmId";
            this.clmId.Width = 150;
            // 
            // clmMessage
            // 
            this.clmMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmMessage.HeaderText = "Message";
            this.clmMessage.MinimumWidth = 8;
            this.clmMessage.Name = "clmMessage";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.grpServeurOperations);
            this.flowLayoutPanel1.Controls.Add(this.grpServerControl);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(194, 782);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // grpServeurOperations
            // 
            this.grpServeurOperations.Controls.Add(this.btnStopServer);
            this.grpServeurOperations.Controls.Add(this.btnStartServer);
            this.grpServeurOperations.Location = new System.Drawing.Point(3, 3);
            this.grpServeurOperations.Name = "grpServeurOperations";
            this.grpServeurOperations.Size = new System.Drawing.Size(192, 118);
            this.grpServeurOperations.TabIndex = 1;
            this.grpServeurOperations.TabStop = false;
            this.grpServeurOperations.Text = "Server Operations";
            // 
            // btnStopServer
            // 
            this.btnStopServer.Enabled = false;
            this.btnStopServer.Location = new System.Drawing.Point(6, 78);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(176, 34);
            this.btnStopServer.TabIndex = 2;
            this.btnStopServer.Text = "Stop Server";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click_1);
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(6, 30);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(176, 34);
            this.btnStartServer.TabIndex = 1;
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click_1);
            // 
            // grpServerControl
            // 
            this.grpServerControl.Controls.Add(this.comboBox1);
            this.grpServerControl.Location = new System.Drawing.Point(3, 127);
            this.grpServerControl.Name = "grpServerControl";
            this.grpServerControl.Size = new System.Drawing.Size(192, 155);
            this.grpServerControl.TabIndex = 3;
            this.grpServerControl.TabStop = false;
            this.grpServerControl.Text = "Server Control";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Debug"});
            this.comboBox1.Location = new System.Drawing.Point(6, 30);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(179, 33);
            this.comboBox1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1457, 788);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grpServeurStatusLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServeurStatusLog)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.grpServeurOperations.ResumeLayout(false);
            this.grpServerControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox grpServeurStatusLog;
        private System.Windows.Forms.DataGridView dgvServeurStatusLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmId;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMessage;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox grpServeurOperations;
        private System.Windows.Forms.Button btnStopServer;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.GroupBox grpServerControl;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}