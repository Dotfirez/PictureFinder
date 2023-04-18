using System.Drawing;
using System.Windows.Forms;

namespace PictureFindTest
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            panel2=new Panel();
            splitContainer1=new SplitContainer();
            label2=new Label();
            label1=new Label();
            listBox1=new ListBox();
            button4=new Button();
            pictureBox4=new PictureBox();
            button3=new Button();
            pictureBox1=new PictureBox();
            button1=new Button();
            pictureBox3=new PictureBox();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Controls.Add(splitContainer1);
            panel2.Dock=DockStyle.Fill;
            panel2.Location=new Point(0, 0);
            panel2.Margin=new Padding(4, 4, 4, 4);
            panel2.Name="panel2";
            panel2.Size=new Size(845, 707);
            panel2.TabIndex=1;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock=DockStyle.Fill;
            splitContainer1.FixedPanel=FixedPanel.Panel1;
            splitContainer1.Location=new Point(0, 0);
            splitContainer1.Margin=new Padding(4, 4, 4, 4);
            splitContainer1.Name="splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(listBox1);
            splitContainer1.Panel1.Controls.Add(button4);
            splitContainer1.Panel1.Controls.Add(pictureBox4);
            splitContainer1.Panel1.Controls.Add(button3);
            splitContainer1.Panel1.Controls.Add(pictureBox1);
            splitContainer1.Panel1.Controls.Add(button1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(pictureBox3);
            splitContainer1.Size=new Size(845, 707);
            splitContainer1.SplitterDistance=283;
            splitContainer1.SplitterWidth=5;
            splitContainer1.TabIndex=0;
            // 
            // label2
            // 
            label2.AutoSize=true;
            label2.BorderStyle=BorderStyle.FixedSingle;
            label2.Location=new Point(205, 427);
            label2.Margin=new Padding(4, 0, 4, 0);
            label2.Name="label2";
            label2.Size=new Size(17, 19);
            label2.TabIndex=4;
            label2.Text="0";
            // 
            // label1
            // 
            label1.AutoSize=true;
            label1.Location=new Point(102, 427);
            label1.Margin=new Padding(4, 0, 4, 0);
            label1.Name="label1";
            label1.Size=new Size(104, 17);
            label1.TabIndex=3;
            label1.Text="查找花费的时间：";
            // 
            // listBox1
            // 
            listBox1.Anchor=AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left;
            listBox1.FormattingEnabled=true;
            listBox1.ItemHeight=17;
            listBox1.Location=new Point(18, 453);
            listBox1.Margin=new Padding(4, 4, 4, 4);
            listBox1.Name="listBox1";
            listBox1.Size=new Size(252, 242);
            listBox1.TabIndex=2;
            // 
            // button4
            // 
            button4.Location=new Point(16, 413);
            button4.Margin=new Padding(4, 4, 4, 4);
            button4.Name="button4";
            button4.Size=new Size(78, 35);
            button4.TabIndex=1;
            button4.Text="查找小图";
            button4.UseVisualStyleBackColor=true;
            button4.Click+=button4_Click;
            // 
            // pictureBox4
            // 
            pictureBox4.BorderStyle=BorderStyle.FixedSingle;
            pictureBox4.Location=new Point(14, 250);
            pictureBox4.Margin=new Padding(4, 4, 4, 4);
            pictureBox4.Name="pictureBox4";
            pictureBox4.Size=new Size(157, 155);
            pictureBox4.TabIndex=1;
            pictureBox4.TabStop=false;
            // 
            // button3
            // 
            button3.Location=new Point(15, 210);
            button3.Margin=new Padding(4, 4, 4, 4);
            button3.Name="button3";
            button3.Size=new Size(72, 35);
            button3.TabIndex=1;
            button3.Text="加载小图";
            button3.UseVisualStyleBackColor=true;
            button3.Click+=button3_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle=BorderStyle.FixedSingle;
            pictureBox1.Location=new Point(14, 57);
            pictureBox1.Margin=new Padding(4, 4, 4, 4);
            pictureBox1.Name="pictureBox1";
            pictureBox1.Size=new Size(144, 148);
            pictureBox1.SizeMode=PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex=0;
            pictureBox1.TabStop=false;
            pictureBox1.Click+=pictureBox1_Click;
            // 
            // button1
            // 
            button1.Location=new Point(14, 18);
            button1.Margin=new Padding(4, 4, 4, 4);
            button1.Name="button1";
            button1.Size=new Size(83, 35);
            button1.TabIndex=0;
            button1.Text="加载大图";
            button1.UseVisualStyleBackColor=true;
            button1.Click+=button1_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.BorderStyle=BorderStyle.FixedSingle;
            pictureBox3.Dock=DockStyle.Fill;
            pictureBox3.Location=new Point(0, 0);
            pictureBox3.Margin=new Padding(4, 4, 4, 4);
            pictureBox3.Name="pictureBox3";
            pictureBox3.Size=new Size(557, 707);
            pictureBox3.SizeMode=PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex=0;
            pictureBox3.TabStop=false;
            // 
            // frmMain
            // 
            AutoScaleDimensions=new SizeF(7F, 17F);
            AutoScaleMode=AutoScaleMode.Font;
            ClientSize=new Size(845, 707);
            Controls.Add(panel2);
            Margin=new Padding(4, 4, 4, 4);
            Name="frmMain";
            Text="BitmapFindTest";
            panel2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}