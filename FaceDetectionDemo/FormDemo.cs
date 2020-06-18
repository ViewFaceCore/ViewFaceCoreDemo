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
        FilterInfoCollection videoDevices;
        ViewFace ViewFace = new ViewFace();

        List<Rectangle> Rectangles = new List<Rectangle>();

        public FormDemo()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            comboBox1.Items.Clear();
            foreach (FilterInfo info in videoDevices)
            {
                comboBox1.Items.Add(info.Name);
            }
            if (comboBox1.Items.Count > 0)
            { comboBox1.SelectedIndex = 0; }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;

            FilterInfo info = videoDevices[comboBox1.SelectedIndex];
            VideoCaptureDevice videoCapture = new VideoCaptureDevice(info.MonikerString);
            videoSourcePlayer1.VideoSource = videoCapture;
            videoSourcePlayer1.Start();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Bitmap bitmap = videoSourcePlayer1.GetCurrentVideoFrame();
            if (videoSourcePlayer1.IsRunning && bitmap != null)
            {
                var infos = ViewFace.FaceDetector(bitmap);
                Rectangles.Clear();
                foreach (var info in infos)
                {
                    Rectangles.Add(new Rectangle(info.Location.X, info.Location.Y, info.Location.Width, info.Location.Height));
                }
                if (videoSourcePlayer1.IsRunning && Rectangles.Count > 0)
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.DrawRectangles(new Pen(Color.Green, 4), Rectangles.ToArray());
                    }
                    pictureBox1.Image = bitmap;
                }
            }
        }
    }
}
