using Dotfire;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PictureFindTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            Bitmap b = new Bitmap("temp/flower.png");
            pictureBox1.Image = b;
            b = new Bitmap("temp/flower2.png");
            pictureBox4.Image = b;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox3.Image = pictureBox1.Image;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(图片)|*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filename = ofd.FileName;
                Bitmap b = new Bitmap(filename);
                pictureBox1.Image = b;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(图片)|*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filename = ofd.FileName;
                Bitmap b = new Bitmap(filename);
                pictureBox4.Image = b;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Bitmap big = (Bitmap)pictureBox1.Image;
            Bitmap small = (Bitmap)pictureBox4.Image;
            if (big==null || small==null)
            {
                MessageBox.Show("请先加载大图和小图");
                return;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool befind = PictureFinder.FindSmallPic(out Dictionary<Point, float> Found, big, small, 0.8f);
            stopwatch.Stop();
            label2.Text = $"{stopwatch.ElapsedMilliseconds}毫秒";

            List<Rectangle> lrect = new List<Rectangle>();


            foreach (var v in Found)
            {
                listBox1.Items.Add($"{v.Key},相似度{string.Format("{0:f2}", v.Value*100)}%,{label2.Text}");
                lrect.Add(new Rectangle(v.Key.X, v.Key.Y, small.Width, small.Height));
            }
            Bitmap c = PictureFinder.DrawRectangle(big, lrect, Color.Yellow, 2);
            pictureBox3.Image = c;

        }
    }
}
