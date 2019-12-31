using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ImageMoveZoom
{
    public partial class PicViewForm : Form
    {
        public PicViewForm()
        {
            InitializeComponent();
            //双缓存
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
        }
        Bitmap bufferpic;//加快GDI读取用缓存图片
        
        Size stopScalingSize; // 图片停止缩放时图片大小（如图片大于窗体大小则为窗体大小）
        int mouse_offset_x = 0; // 鼠标x位置与位置中心的偏移量
        int mouse_offset_y = 0;// 鼠标y位置与位置中心的偏移量
        float scale_x = 1f; //图片x位置变化幅度
        float scale_y = 1f; //图片y位置变化幅度

        Point mouseOriginalLocation; //鼠标原始位置
        int mouse_move_offset_x = 0; //鼠标移动x方向上的偏移量
        int mouse_move_offset_y = 0; //鼠标移动y方向上的偏移量
        Point picLocation = new Point(); //图片当前位置

        public void InPutBuffer(Image img)
        {
            bufferpic = new Bitmap(img);
            pictureBox1.Image = bufferpic;
        }

        private void PicViewForm_Load(object sender, EventArgs e)
        {
            Init();

            this.pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);//鼠标滚轮事件
        }

        void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            mouse_offset_x = e.X - this.pictureBox1.Width / 2;
            mouse_offset_y = e.Y - this.pictureBox1.Height / 2;

            scale_x = GetLocationScale(pictureBox1.Width / 2, mouse_offset_x);
            scale_y = GetLocationScale(pictureBox1.Height / 2, mouse_offset_y);

            System.Drawing.Size t = pictureBox1.Size;
            t.Width += e.Delta;
            t.Height += e.Delta;

            Point p = picLocation;
            p.X += (int)(((float)(pictureBox1.Width - t.Width)) / 2 * scale_x);
            p.Y += (int)(((float)(pictureBox1.Height - t.Height)) / 2 * scale_y);

            if (t.Width > stopScalingSize.Width || t.Height > stopScalingSize.Height)
            {
                pictureBox1.Width = t.Width;
                pictureBox1.Height = t.Height;
                picLocation = p;
            }
            this.pictureBox1.Location = picLocation;
        }

        private void Init()
        {
            float p1 = (float)this.pictureBox1.Image.Width / (float)this.pictureBox1.Image.Height; //图片宽高比
            float p2 = (float)this.ClientRectangle.Width / (float)this.ClientRectangle.Height; //窗体宽高比
            if (p1 > p2)
            {
                if (this.pictureBox1.Image.Width > this.ClientRectangle.Width)
                {
                    this.pictureBox1.Width = this.ClientRectangle.Width;
                    this.pictureBox1.Height = (int)((float)this.pictureBox1.Width / p1);
                }
                else
                {
                    this.pictureBox1.Size = this.pictureBox1.Image.Size;
                }
            }
            else
            {
                if (this.pictureBox1.Image.Height > this.ClientRectangle.Height)
                {
                    this.pictureBox1.Height = this.ClientRectangle.Height;
                    this.pictureBox1.Width = (int)((float)this.pictureBox1.Height * p1);
                }
                else
                {
                    this.pictureBox1.Size = this.pictureBox1.Image.Size;
                }

            }
            picLocation = new Point((this.ClientRectangle.Width - this.pictureBox1.Width) / 2, (this.ClientRectangle.Height - this.pictureBox1.Height) / 2);
            this.pictureBox1.Location = picLocation;
            stopScalingSize = new Size(this.pictureBox1.Width, this.pictureBox1.Height);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBox1.Focus();
        }
        
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouse_move_offset_x = mouseOriginalLocation.X - e.Location.X;
                mouse_move_offset_y = mouseOriginalLocation.Y - e.Location.Y;

                picLocation.X = this.pictureBox1.Location.X - mouse_move_offset_x;
                picLocation.Y = this.pictureBox1.Location.Y - mouse_move_offset_y;
                this.pictureBox1.Location = picLocation;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseOriginalLocation = e.Location; //记录下鼠标原始位置
            Cursor = Cursors.SizeAll;
        }

        /// <summary>
        /// 计算图片坐标变化幅度
        /// </summary>
        /// <param name="range"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private float GetLocationScale(int range,int offset)
        {
            float s = 1f;
            if (offset < 0)
            {
                s = 1f - (float)(-offset) / (float)range;
            }
            else if (offset > 0)
            {
                s = 1f + (float)offset / (float)range;
            }
            return s;
        }

        private void PicViewForm_Resize(object sender, EventArgs e)
        {
            Init();
        }
    }
}
