namespace GDS.WMS.WFApplication
{
    partial class frmMain
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
            this.btnSyncWorkItem = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txt = new System.Windows.Forms.TextBox();
            this.PartNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartDesc1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartDesc2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartUm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSyncWorkItem
            // 
            this.btnSyncWorkItem.Location = new System.Drawing.Point(372, 6);
            this.btnSyncWorkItem.Name = "btnSyncWorkItem";
            this.btnSyncWorkItem.Size = new System.Drawing.Size(136, 23);
            this.btnSyncWorkItem.TabIndex = 0;
            this.btnSyncWorkItem.Text = "同步物料基本数据";
            this.btnSyncWorkItem.UseVisualStyleBackColor = true;
            this.btnSyncWorkItem.Click += new System.EventHandler(this.btnSyncWorkItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(618, 422);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvData);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txt);
            this.tabPage1.Controls.Add(this.btnSyncWorkItem);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(610, 396);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "物料基本信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PartNo,
            this.PartDesc1,
            this.PartDesc2,
            this.PartUm});
            this.dgvData.Enabled = false;
            this.dgvData.Location = new System.Drawing.Point(3, 35);
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(604, 358);
            this.dgvData.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(15, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "同步{0}天数据:";
            // 
            // txt
            // 
            this.txt.Location = new System.Drawing.Point(128, 8);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(218, 21);
            this.txt.TabIndex = 1;
            this.txt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // PartNo
            // 
            this.PartNo.DataPropertyName = "PartNo";
            this.PartNo.Frozen = true;
            this.PartNo.HeaderText = "物料号";
            this.PartNo.Name = "PartNo";
            this.PartNo.ReadOnly = true;
            this.PartNo.Width = 120;
            // 
            // PartDesc1
            // 
            this.PartDesc1.DataPropertyName = "PartDesc1";
            this.PartDesc1.HeaderText = "品名";
            this.PartDesc1.Name = "PartDesc1";
            this.PartDesc1.ReadOnly = true;
            this.PartDesc1.Width = 180;
            // 
            // PartDesc2
            // 
            this.PartDesc2.DataPropertyName = "PartDesc2";
            this.PartDesc2.HeaderText = "规格";
            this.PartDesc2.Name = "PartDesc2";
            this.PartDesc2.ReadOnly = true;
            this.PartDesc2.Width = 180;
            // 
            // PartUm
            // 
            this.PartUm.DataPropertyName = "PartUm";
            this.PartUm.HeaderText = "单位";
            this.PartUm.Name = "PartUm";
            this.PartUm.ReadOnly = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 446);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据同步中心";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSyncWorkItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartDesc1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartDesc2;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartUm;
    }
}

