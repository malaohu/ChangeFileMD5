namespace ChangeFileMD5
{
    partial class Form1
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
            this.dgvFilesInfo = new System.Windows.Forms.DataGridView();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnLoadFiles = new System.Windows.Forms.Button();
            this.lblTip = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilesInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFilesInfo
            // 
            this.dgvFilesInfo.AllowUserToAddRows = false;
            this.dgvFilesInfo.AllowUserToDeleteRows = false;
            this.dgvFilesInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvFilesInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dgvFilesInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilesInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvFilesInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvFilesInfo.Name = "dgvFilesInfo";
            this.dgvFilesInfo.ReadOnly = true;
            this.dgvFilesInfo.RowTemplate.Height = 23;
            this.dgvFilesInfo.Size = new System.Drawing.Size(787, 262);
            this.dgvFilesInfo.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(415, 275);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(110, 47);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "开始修改MD5";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLoadFiles
            // 
            this.btnLoadFiles.Location = new System.Drawing.Point(240, 275);
            this.btnLoadFiles.Name = "btnLoadFiles";
            this.btnLoadFiles.Size = new System.Drawing.Size(95, 47);
            this.btnLoadFiles.TabIndex = 2;
            this.btnLoadFiles.Text = "载入文件";
            this.btnLoadFiles.UseVisualStyleBackColor = true;
            this.btnLoadFiles.Click += new System.EventHandler(this.btnLoadFiles_Click);
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Font = new System.Drawing.Font("华文行楷", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTip.ForeColor = System.Drawing.Color.Red;
            this.lblTip.Location = new System.Drawing.Point(265, 335);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(0, 19);
            this.lblTip.TabIndex = 3;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 362);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(787, 34);
            this.progressBar.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(787, 396);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.btnLoadFiles);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.dgvFilesInfo);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MD5修改";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilesInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFilesInfo;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnLoadFiles;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

