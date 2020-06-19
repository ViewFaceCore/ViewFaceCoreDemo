namespace FaceDetectionDemo
{
    partial class FormDemo
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
            this.ButtonStart = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.FacePictureBox = new System.Windows.Forms.PictureBox();
            this.TimerDetector = new System.Windows.Forms.Timer(this.components);
            this.VideoPlayer = new AForge.Controls.VideoSourcePlayer();
            ((System.ComponentModel.ISupportInitialize)(this.FacePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonStart
            // 
            this.ButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonStart.Location = new System.Drawing.Point(752, 12);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(127, 61);
            this.ButtonStart.TabIndex = 1;
            this.ButtonStart.Text = "打开摄像头并识别人脸";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(752, 79);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(127, 23);
            this.comboBox1.TabIndex = 2;
            // 
            // FacePictureBox
            // 
            this.FacePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FacePictureBox.Location = new System.Drawing.Point(12, 12);
            this.FacePictureBox.Name = "FacePictureBox";
            this.FacePictureBox.Size = new System.Drawing.Size(642, 418);
            this.FacePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FacePictureBox.TabIndex = 4;
            this.FacePictureBox.TabStop = false;
            // 
            // VideoPlayer
            // 
            this.VideoPlayer.Location = new System.Drawing.Point(26, 23);
            this.VideoPlayer.Name = "VideoPlayer";
            this.VideoPlayer.Size = new System.Drawing.Size(241, 168);
            this.VideoPlayer.TabIndex = 5;
            this.VideoPlayer.Text = "videoSourcePlayer1";
            this.VideoPlayer.VideoSource = null;
            // 
            // FormDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 442);
            this.Controls.Add(this.VideoPlayer);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.FacePictureBox);
            this.Name = "FormDemo";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FacePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox FacePictureBox;
        private System.Windows.Forms.Timer TimerDetector;
        private AForge.Controls.VideoSourcePlayer VideoPlayer;
    }
}

