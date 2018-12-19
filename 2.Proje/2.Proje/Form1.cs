using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision;
using AForge.Vision.Motion;
using System.IO;
using System.Collections;
using System.IO.Ports;

namespace _2.Proje
{
    public partial class Form1 : Form
    {
        int LED;
        int SERIPORT;

        private FilterInfoCollection KAYNAKLAR;
        private VideoCaptureDevice KAMERA;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KAYNAKLAR = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo KAYNAK in KAYNAKLAR)
            {
                comboBox1.Items.Add(KAYNAK.Name);
            }

            KAMERA = new VideoCaptureDevice();
            button4.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (KAMERA.IsRunning)
            {
                KAMERA.Stop();
                pictureBox1.Image = null;
                pictureBox1.Invalidate();
                button1.Text = "BAŞLAT";
            }
            else
            {
                KAMERA = new VideoCaptureDevice(KAYNAKLAR[comboBox1.SelectedIndex].MonikerString);
                KAMERA.NewFrame += kamera_NewFrame;
                KAMERA.Start();
                button1.Text = "DURDUR";
            }
        }

        private void kamera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap goruntu = (Bitmap)eventArgs.Frame.Clone();
            Bitmap gercekgoruntu = (Bitmap)eventArgs.Frame.Clone();
            goruntu.RotateFlip(RotateFlipType.Rotate180FlipY);
            gercekgoruntu.RotateFlip(RotateFlipType.Rotate180FlipY);

            EuclideanColorFiltering FILTRELEME = new EuclideanColorFiltering();
            FILTRELEME.CenterColor = new RGB(Color.FromArgb(20, 20, 220));
            FILTRELEME.Radius = 100;
            FILTRELEME.ApplyInPlace(goruntu);

            KAREOLUSTUR(goruntu);

            pictureBox2.Image = gercekgoruntu;
            pictureBox1.Image = goruntu;

        }

        private void KAREOLUSTUR(Bitmap goruntu)
        {
            BlobCounter COUNTERBLOB = new BlobCounter();
            COUNTERBLOB.MinWidth = 30;
            COUNTERBLOB.MinHeight = 30;
            COUNTERBLOB.FilterBlobs = true;
            COUNTERBLOB.ObjectsOrder = ObjectsOrder.Size;

            Grayscale GRIFILTRELEME = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap GRIGORUNTU = GRIFILTRELEME.Apply(goruntu);

            COUNTERBLOB.ProcessImage(GRIGORUNTU);
            Rectangle[] RECS = COUNTERBLOB.GetObjectsRectangles();

            foreach (Rectangle REC in RECS)
            {
                if (RECS.Length > 0)
                {
                    Rectangle nesneRect = RECS[0];
                    Graphics f = Graphics.FromImage(goruntu);
                    using (Pen p = new Pen(Color.FromArgb(255, 255, 255), 2))
                    {
                        f.DrawRectangle(p, nesneRect);
                    }

                    int nesneXEKSENI = nesneRect.X + (nesneRect.Width / 2);
                    int nesneYEKSENI = nesneRect.Y + (nesneRect.Height / 2);
                    f.DrawString(nesneXEKSENI.ToString() + "X" + nesneYEKSENI.ToString(), new Font("Times New Roman", 40), Brushes.Yellow, new System.Drawing.Point(0, 0));
                    f.Dispose();

                    if (nesneXEKSENI > 0 && nesneXEKSENI <= 213)
                    {
                        LED = 1;
                    }
                    else if (nesneXEKSENI > 213 && nesneXEKSENI <= 426)
                    {
                        LED = 2;
                    }
                    else if (nesneXEKSENI > 426 && nesneXEKSENI <= 640)
                    {
                        LED = 3;
                    }

                    if (SERIPORT != LED)
                    {
                        SERIPORT = LED;
                        if (serialPort1.IsOpen)
                        {
                            serialPort1.Write(LED.ToString());
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] PORTLAR = SerialPort.GetPortNames();
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(PORTLAR);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                serialPort1.PortName = comboBox2.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Open();
                button3.Enabled = false;
                button4.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            button3.Enabled = true;
            button4.Enabled = false;
        }
    }
}
