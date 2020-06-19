using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewFaceCore.Sharp;

namespace FaceDetectionDemo
{
    public partial class FormDemo : Form
    {
        public FormDemo()
        {
            InitializeComponent();

            TimerDetector.Interval = 1000 / 15; // 15 FPS
            TimerDetector.Tick += TimerDetector_Tick;

            VideoPlayer.Visible = false; // 隐藏摄像头画面控件
        }

        /// <summary>
        /// 摄像头设备信息集合
        /// </summary>
        FilterInfoCollection VideoDevices;
        /// <summary>
        /// 人脸位置信息集合
        /// </summary>
        List<Rectangle> FaceRectangles = new List<Rectangle>();

        /// <summary>
        /// 人脸识别库
        /// </summary>
        ViewFace ViewFace = new ViewFace();

        /// <summary>
        /// 窗体加载时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Load(object sender, EventArgs e)
        {
            VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            comboBox1.Items.Clear();
            foreach (FilterInfo info in VideoDevices)
            {
                comboBox1.Items.Add(info.Name);
            }
            if (comboBox1.Items.Count > 0)
            { comboBox1.SelectedIndex = 0; }
        }

        /// <summary>
        /// 点击开始按钮时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (VideoPlayer.IsRunning)
            {
                VideoPlayer.SignalToStop();
                VideoPlayer.WaitForStop();
                TimerDetector.Start();
                ButtonStart.Text = "打开摄像头并识别人脸";
            }
            else
            {
                if (comboBox1.SelectedIndex == -1) return;
                FilterInfo info = VideoDevices[comboBox1.SelectedIndex];
                VideoCaptureDevice videoCapture = new VideoCaptureDevice(info.MonikerString);
                VideoPlayer.VideoSource = videoCapture;
                VideoPlayer.Start();
                TimerDetector.Start();
                ButtonStart.Text = "关闭摄像头";
            }
        }

        /// <summary>
        /// 每 100 ms 检测一次人脸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerDetector_Tick(object sender, EventArgs e)
        {
            if (VideoPlayer.IsRunning)
            {
                Bitmap bitmap = VideoPlayer.GetCurrentVideoFrame(); // 获取摄像头画面
                if (bitmap != null)
                {
                    var infos = ViewFace.FaceDetector(bitmap); // 识别画面中的人脸
                    FaceRectangles.Clear();
                    foreach (var info in infos)
                    {
                        FaceRectangles.Add(new Rectangle(info.Location.X, info.Location.Y, info.Location.Width, info.Location.Height));
                    }
                    if (FaceRectangles.Count > 0) // 如果有人脸，在 bitmap 上绘制出人脸的位置信息
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.DrawRectangles(new Pen(Color.Red, 4), FaceRectangles.ToArray());
                        }
                    }
                }
                FacePictureBox.Image = bitmap;
            }
        }

        /// <summary>
        /// 窗体关闭时，关闭摄像头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            VideoPlayer.SignalToStop();
            VideoPlayer.WaitForStop();
        }
    }
}
