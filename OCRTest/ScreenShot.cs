using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCRTest
{
    public partial class ScreenShot : Form
    {
        private ScreenForm screenForm = new ScreenForm();

        private Bitmap currentBitmap;

        public ScreenShot()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            screenForm.ScreenShotOk += new EventHandler(ScreenShotOk_Click);
        }

        public Image ScreenShotImage { get; set; }

        private void ScreenShotOk_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(screenForm.End.X - screenForm.Start.X, screenForm.End.Y - screenForm.Start.Y);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                int w = screenForm.End.X - screenForm.Start.X;
                int h = screenForm.End.Y - screenForm.Start.Y;
                Rectangle destRect = new Rectangle(0, 0, w + 1, h + 1);//在画布上要显示的区域（记得像素加1）
                Rectangle srcRect = new Rectangle(screenForm.Start.X, screenForm.Start.Y - 15, w + 1, h + 1);//图像上要截取的区域
                g.DrawImage(currentBitmap, destRect, srcRect, GraphicsUnit.Pixel);//加图像绘制到画布上
            }
            //this.pictureBox1.Image = bmp;
            ScreenShotImage = bmp;
            this.Show();
            this.Close();
        }

        public Bitmap GetScreen()
        {
            //获取整个屏幕图像,不包括任务栏
            Rectangle ScreenArea = Screen.GetWorkingArea(this);
            Bitmap bmp = new Bitmap(ScreenArea.Width, ScreenArea.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(ScreenArea.Width, ScreenArea.Height));
            }
            return bmp;
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();//隐藏当前
            //this.pictureBox1=null;//清除绘制的历史线条
            this.currentBitmap = GetScreen();
            screenForm.BackgroundImage = this.currentBitmap;
            screenForm.StartPosition = FormStartPosition.Manual;//起始位置
            screenForm.ShowDialog();
        }
    }
}
