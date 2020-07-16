namespace OCRTest
{
    partial class RecognizeWordsOnPic
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.m_screenShot = new System.Windows.Forms.Button();
            this.m_selectPictureButton = new System.Windows.Forms.Button();
            this.m_imagePathText = new System.Windows.Forms.TextBox();
            this.m_tipLabel = new System.Windows.Forms.Label();
            this.m_imagePathLabel = new System.Windows.Forms.Label();
            this.m_originalPictureBox = new System.Windows.Forms.PictureBox();
            this.m_cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_pasteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.picDig = new System.Windows.Forms.FolderBrowserDialog();
            this.m_resultTextBox = new System.Windows.Forms.RichTextBox();
            this.m_progressBar = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_originalPictureBox)).BeginInit();
            this.m_cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.m_screenShot);
            this.panel1.Controls.Add(this.m_selectPictureButton);
            this.panel1.Controls.Add(this.m_imagePathText);
            this.panel1.Controls.Add(this.m_tipLabel);
            this.panel1.Controls.Add(this.m_imagePathLabel);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1135, 50);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(966, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "截图";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnScreenShotTwoButtonClick);
            // 
            // m_screenShot
            // 
            this.m_screenShot.Location = new System.Drawing.Point(873, 12);
            this.m_screenShot.Name = "m_screenShot";
            this.m_screenShot.Size = new System.Drawing.Size(75, 23);
            this.m_screenShot.TabIndex = 3;
            this.m_screenShot.Text = "伪录屏";
            this.m_screenShot.UseVisualStyleBackColor = true;
            this.m_screenShot.Click += new System.EventHandler(this.OnScreenShotButtonClick);
            // 
            // m_selectPictureButton
            // 
            this.m_selectPictureButton.Location = new System.Drawing.Point(573, 14);
            this.m_selectPictureButton.Name = "m_selectPictureButton";
            this.m_selectPictureButton.Size = new System.Drawing.Size(75, 23);
            this.m_selectPictureButton.TabIndex = 2;
            this.m_selectPictureButton.Text = "选择图片";
            this.m_selectPictureButton.UseVisualStyleBackColor = true;
            this.m_selectPictureButton.Click += new System.EventHandler(this.OnSelectPictureButtonClick);
            // 
            // m_imagePathText
            // 
            this.m_imagePathText.Location = new System.Drawing.Point(75, 15);
            this.m_imagePathText.Name = "m_imagePathText";
            this.m_imagePathText.Size = new System.Drawing.Size(500, 21);
            this.m_imagePathText.TabIndex = 1;
            // 
            // m_tipLabel
            // 
            this.m_tipLabel.AutoSize = true;
            this.m_tipLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.m_tipLabel.Location = new System.Drawing.Point(670, 17);
            this.m_tipLabel.Name = "m_tipLabel";
            this.m_tipLabel.Size = new System.Drawing.Size(197, 12);
            this.m_tipLabel.TabIndex = 0;
            this.m_tipLabel.Text = "(选择图片或将图片拖入左侧图片框)";
            // 
            // m_imagePathLabel
            // 
            this.m_imagePathLabel.AutoSize = true;
            this.m_imagePathLabel.Location = new System.Drawing.Point(12, 18);
            this.m_imagePathLabel.Name = "m_imagePathLabel";
            this.m_imagePathLabel.Size = new System.Drawing.Size(59, 12);
            this.m_imagePathLabel.TabIndex = 0;
            this.m_imagePathLabel.Text = "图片路径:";
            // 
            // m_originalPictureBox
            // 
            this.m_originalPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.m_originalPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.m_originalPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.m_originalPictureBox.ContextMenuStrip = this.m_cms;
            this.m_originalPictureBox.Location = new System.Drawing.Point(9, 61);
            this.m_originalPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.m_originalPictureBox.Name = "m_originalPictureBox";
            this.m_originalPictureBox.Size = new System.Drawing.Size(566, 550);
            this.m_originalPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.m_originalPictureBox.TabIndex = 1;
            this.m_originalPictureBox.TabStop = false;
            this.m_originalPictureBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnOriginalPictureBoxDragDrop);
            this.m_originalPictureBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnOriginalPictureBoxDragEnter);
            this.m_originalPictureBox.DoubleClick += new System.EventHandler(this.pictureBox_DoubleClick);
            // 
            // m_cms
            // 
            this.m_cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_pasteMenu});
            this.m_cms.Name = "m_cms";
            this.m_cms.Size = new System.Drawing.Size(142, 26);
            // 
            // m_pasteMenu
            // 
            this.m_pasteMenu.Name = "m_pasteMenu";
            this.m_pasteMenu.Size = new System.Drawing.Size(141, 22);
            this.m_pasteMenu.Text = "粘贴(ctrl+v)";
            this.m_pasteMenu.Click += new System.EventHandler(this.OnPasteMenuClick);
            // 
            // m_resultTextBox
            // 
            this.m_resultTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_resultTextBox.Location = new System.Drawing.Point(590, 61);
            this.m_resultTextBox.Name = "m_resultTextBox";
            this.m_resultTextBox.Size = new System.Drawing.Size(536, 550);
            this.m_resultTextBox.TabIndex = 2;
            this.m_resultTextBox.Text = "";
            // 
            // m_progressBar
            // 
            this.m_progressBar.Location = new System.Drawing.Point(715, 324);
            this.m_progressBar.Name = "m_progressBar";
            this.m_progressBar.Size = new System.Drawing.Size(294, 23);
            this.m_progressBar.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1047, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "WPF截图";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // RecognizeWordsOnPic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 623);
            this.Controls.Add(this.m_progressBar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.m_resultTextBox);
            this.Controls.Add(this.m_originalPictureBox);
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(1145, 662);
            this.MinimumSize = new System.Drawing.Size(1145, 662);
            this.Name = "RecognizeWordsOnPic";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图片文字识别";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RecognizeWordsOnPic_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_originalPictureBox)).EndInit();
            this.m_cms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button m_selectPictureButton;
        private System.Windows.Forms.TextBox m_imagePathText;
        private System.Windows.Forms.Label m_imagePathLabel;
        private System.Windows.Forms.PictureBox m_originalPictureBox;
        private System.Windows.Forms.FolderBrowserDialog picDig;
        private System.Windows.Forms.RichTextBox m_resultTextBox;
        private System.Windows.Forms.Label m_tipLabel;
        private System.Windows.Forms.ProgressBar m_progressBar;
        private System.Windows.Forms.ContextMenuStrip m_cms;
        private System.Windows.Forms.ToolStripMenuItem m_pasteMenu;
        private System.Windows.Forms.Button m_screenShot;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

