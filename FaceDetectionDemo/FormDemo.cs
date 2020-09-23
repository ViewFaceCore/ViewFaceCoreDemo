using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View.Core.Extensions;
using ViewFaceCore.Sharp;

namespace FaceDetectionDemo
{
    public partial class FormDemo : Form
    {
        public FormDemo()
        {
            InitializeComponent();

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
        /// 人脸 Pid 集合
        /// </summary>
        List<int> Pids = new List<int>();
        /// <summary>
        /// 人脸对应的年龄集合
        /// </summary>
        List<int> Ages = new List<int>();
        /// <summary>
        /// 性别集合
        /// </summary>
        List<string> Gender = new List<string>();

        /// <summary>
        /// 人脸识别库
        /// </summary>
        ViewFace ViewFace = new ViewFace();

        /// <summary>
        /// 取消令牌
        /// </summary>
        CancellationTokenSource Token { get; set; }

        /// <summary>
        /// 指示是否应关闭窗体
        /// </summary>
        bool IsClose = false;

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
        /// 窗体关闭时，关闭摄像头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Token?.Cancel();
            if (!IsClose && VideoPlayer.IsRunning)
            { // 若摄像头开启时，点击关闭是暂不关闭，并设置关闭窗口的标识，待摄像头等设备关闭后，再关闭窗体。
                e.Cancel = true;
                IsClose = true;
            }
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
                Token?.Cancel();
                ButtonStart.Text = "打开摄像头并识别人脸";
            }
            else
            {
                if (comboBox1.SelectedIndex == -1) return;
                FilterInfo info = VideoDevices[comboBox1.SelectedIndex];
                VideoCaptureDevice videoCapture = new VideoCaptureDevice(info.MonikerString);
                VideoPlayer.VideoSource = videoCapture;
                VideoPlayer.Start();
                ButtonStart.Text = "关闭摄像头";
                Token = new CancellationTokenSource();
                StartDetector(Token.Token);
            }
        }

        /// <summary>
        /// 持续检测一次人脸，直到停止。
        /// </summary>
        /// <param name="token">取消标记</param>
        private async void StartDetector(CancellationToken token)
        {
            Stopwatch stopwatch = new Stopwatch();
            while (VideoPlayer.IsRunning && !token.IsCancellationRequested)
            {
                if (CheckBoxFPS.Checked)
                { stopwatch.Restart(); }
                Bitmap bitmap = VideoPlayer.GetCurrentVideoFrame(); // 获取摄像头画面 
                if (bitmap != null)
                {
                    FaceRectangles.Clear(); Ages.Clear(); Pids.Clear();
                    if (CheckBoxDetect.Checked)
                    {
                        var infos = await ViewFace.FaceTrackAsync(bitmap); // 识别画面中的人脸
                        foreach (var info in infos)
                        {
                            FaceRectangles.Add(info.Location);
                            Pids.Add(info.Pid);
                            if (CheckBoxFaceProperty.Checked)
                            {
                                Ages.Add(await ViewFace.FaceAgePredictorAsync(bitmap, await ViewFace.FaceMarkAsync(bitmap, new ViewFaceCore.Sharp.Model.FaceInfo() { Location = info.Location, Score = info.Score })));
                                Gender.Add((await ViewFace.FaceGenderPredictorAsync(bitmap, await ViewFace.FaceMarkAsync(bitmap, new ViewFaceCore.Sharp.Model.FaceInfo() { Location = info.Location, Score = info.Score }))).ToDescription());
                            }
                        }
                    }
                    else
                    {
                        await Task.Delay(1000 / 60);
                    }
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        if (FaceRectangles.Any()) // 如果有人脸，在 bitmap 上绘制出人脸的位置信息
                        {
                            g.DrawRectangles(new Pen(Color.Red, 4), FaceRectangles.ToArray());
                            if (CheckBoxDetect.Checked && CheckBoxFaceProperty.Checked)
                            {
                                string pid = "";
                                for (int i = 0; i < FaceRectangles.Count; i++)
                                {
                                    if (Pids.Any())
                                    { pid = $"| Pid: {Pids[i]}"; }
                                    g.DrawString($"{Ages[i]} 岁 | {Gender[i]} {pid}", new Font("微软雅黑", 24), Brushes.Green, new PointF(FaceRectangles[i].X + FaceRectangles[i].Width + 24, FaceRectangles[i].Y));
                                }
                            }
                        }

                        if (CheckBoxFPS.Checked)
                        {
                            stopwatch.Stop();
                            g.DrawString($"{1000f / stopwatch.ElapsedMilliseconds:#.#} FPS", new Font("微软雅黑", 24), Brushes.Green, new Point(10, 10));
                        }
                    }
                }
                else
                { await Task.Delay(10); }
                FacePictureBox.Image?.Dispose();
                FacePictureBox.Image = bitmap;
            }

            VideoPlayer?.SignalToStop();
            VideoPlayer?.WaitForStop();
            if (IsClose)
            {
                Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxDetect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBoxFaceProperty.Enabled = CheckBoxDetect.Checked;
        }
    }
}
