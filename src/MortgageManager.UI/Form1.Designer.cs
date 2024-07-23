namespace MortgageManager.UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnImport = new Button();
            dataGridImported = new DataGridView();
            label1 = new Label();
            lblProductCount = new Label();
            btnUpload = new Button();
            dialogImport = new OpenFileDialog();
            label2 = new Label();
            lblFilePath = new Label();
            panel1 = new Panel();
            pnlInfo = new Panel();
            lblOverallStatus = new Label();
            lblOutputTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridImported).BeginInit();
            panel1.SuspendLayout();
            pnlInfo.SuspendLayout();
            SuspendLayout();
            // 
            // btnImport
            // 
            btnImport.Location = new Point(82, 2);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(75, 37);
            btnImport.TabIndex = 0;
            btnImport.Text = "Open...";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += ImportButtonClick;
            // 
            // dataGridImported
            // 
            dataGridImported.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridImported.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridImported.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllHeaders;
            dataGridImported.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridImported.Location = new Point(5, 56);
            dataGridImported.Name = "dataGridImported";
            dataGridImported.Size = new Size(603, 456);
            dataGridImported.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(-1, 20);
            label1.Name = "label1";
            label1.Size = new Size(109, 15);
            label1.TabIndex = 2;
            label1.Text = "Products Imported:";
            // 
            // lblProductCount
            // 
            lblProductCount.AutoSize = true;
            lblProductCount.ForeColor = Color.FromArgb(0, 192, 0);
            lblProductCount.Location = new Point(106, 20);
            lblProductCount.Name = "lblProductCount";
            lblProductCount.Size = new Size(82, 15);
            lblProductCount.TabIndex = 3;
            lblProductCount.Text = "productCount";
            // 
            // btnUpload
            // 
            btnUpload.Enabled = false;
            btnUpload.Location = new Point(1, 2);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(75, 37);
            btnUpload.TabIndex = 0;
            btnUpload.Text = "Upload";
            btnUpload.UseVisualStyleBackColor = true;
            btnUpload.Click += UploadButtonClick;
            // 
            // dialogImport
            // 
            dialogImport.FileName = "openFileDialog1";
            dialogImport.FileOk += DialogImportFileOk;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(0, 3);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 2;
            label2.Text = "File: ";
            // 
            // lblFilePath
            // 
            lblFilePath.AutoSize = true;
            lblFilePath.ForeColor = Color.FromArgb(0, 192, 0);
            lblFilePath.Location = new Point(26, 3);
            lblFilePath.Name = "lblFilePath";
            lblFilePath.Size = new Size(47, 15);
            lblFilePath.TabIndex = 3;
            lblFilePath.Text = "filePath";
            // 
            // panel1
            // 
            panel1.Controls.Add(pnlInfo);
            panel1.Controls.Add(btnUpload);
            panel1.Controls.Add(btnImport);
            panel1.Location = new Point(5, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(968, 41);
            panel1.TabIndex = 4;
            // 
            // pnlInfo
            // 
            pnlInfo.Controls.Add(lblFilePath);
            pnlInfo.Controls.Add(lblProductCount);
            pnlInfo.Controls.Add(label2);
            pnlInfo.Controls.Add(label1);
            pnlInfo.Location = new Point(162, 2);
            pnlInfo.Name = "pnlInfo";
            pnlInfo.Size = new Size(799, 37);
            pnlInfo.TabIndex = 4;
            pnlInfo.Visible = false;
            // 
            // lblOverallStatus
            // 
            lblOverallStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lblOverallStatus.BackColor = Color.FromArgb(64, 64, 64);
            lblOverallStatus.ForeColor = Color.White;
            lblOverallStatus.Location = new Point(614, 76);
            lblOverallStatus.Name = "lblOverallStatus";
            lblOverallStatus.Size = new Size(359, 436);
            lblOverallStatus.TabIndex = 5;
            lblOverallStatus.Text = "label3";
            // 
            // lblOutputTitle
            // 
            lblOutputTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOutputTitle.AutoSize = true;
            lblOutputTitle.Location = new Point(614, 56);
            lblOutputTitle.Name = "lblOutputTitle";
            lblOutputTitle.Size = new Size(48, 15);
            lblOutputTitle.TabIndex = 6;
            lblOutputTitle.Text = "Output:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(978, 517);
            Controls.Add(lblOutputTitle);
            Controls.Add(lblOverallStatus);
            Controls.Add(panel1);
            Controls.Add(dataGridImported);
            Name = "Form1";
            Text = "Mortgage Product Manager";
            ((System.ComponentModel.ISupportInitialize)dataGridImported).EndInit();
            panel1.ResumeLayout(false);
            pnlInfo.ResumeLayout(false);
            pnlInfo.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnImport;
        private DataGridView dataGridImported;
        private Label label1;
        private Label lblProductCount;
        private Button btnUpload;
        private OpenFileDialog dialogImport;
        private Label label2;
        private Label lblFilePath;
        private Panel panel1;
        private Panel pnlInfo;
        private Label lblOverallStatus;
        private Label lblOutputTitle;
    }
}
