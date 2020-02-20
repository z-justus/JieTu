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
    public partial class Cutter : Form
    {
        public Cutter()
        {
            InitializeComponent();
        }

        #region 定义程序变量
        // 定义变量
        public Image ImageScreenShot { get; set; }
        // 用来记录鼠标按下的坐标，用来确定绘图起点
        private Point m_downPoint;
        // 用来表示是否截图完成
        private bool m_catchFinished = false;
        // 用来表示截图开始
        private bool m_catchStart = false;
        // 用来保存原始图像
        private Bitmap m_originBmp;
        // 用来保存截图的矩形
        private Rectangle m_catchRectangle;
        //取消按钮
        private Button m_closeButton;
        //确定按钮
        private Button m_comfirmButton;

        #endregion

        private void OnCutterLoad(object sender, EventArgs e)
        {
            // 设置控件样式为双缓冲，这样可以有效减少图片闪烁的问题
            // 第二个参数为true表示把第一个参数指定的样式应用于控件；false 表示不应用。
            // '|'表示位逻辑或运算
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
            // 改变鼠标样式
            this.Cursor = Cursors.Cross;
            // 保存全屏图片
            m_originBmp = new Bitmap(this.BackgroundImage);

            InitialButtonControl();

        }
        
        /// <summary>
        /// 初始化取消和确定按钮
        /// </summary>
        private void InitialButtonControl()
        {
            m_closeButton = new Button()
            {
                Text = "取消",
                Size = new Size(50, 30),
                Visible=false
                
            };
            m_comfirmButton = new Button()
            {
                Text = "完成",
                Size = new Size(50, 30),
                Visible = false
            };

            m_closeButton.Click += OnCloseButtonClick;
            m_comfirmButton.Click += OnComfirmButtonClick;
            this.Controls.Add(m_closeButton);
            this.Controls.Add(m_comfirmButton);
        }

        /// <summary>
        /// 左键按下开始截屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutterMouseDown(object sender, MouseEventArgs e)
        {
            // 鼠标左键按下是开始画图，也就是截图
            if (e.Button == MouseButtons.Left)
            {
                m_closeButton.Visible = true;
                m_comfirmButton.Visible = true;
                // 如果捕捉没有开始
                if (!m_catchStart)
                {
                    m_catchStart = true;
                    // 保存此时鼠标按下坐标
                    m_downPoint = new Point(e.X, e.Y);
                }
            }
        }

        /// <summary>
        /// 右键点击退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutterMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// 左键双击保存到剪贴板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutterMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && m_catchFinished)
            {
                // 新建一个与矩形一样大小的空白图片
                Bitmap CatchedBmp = new Bitmap(m_catchRectangle.Width, m_catchRectangle.Height);
                Graphics g = Graphics.FromImage(CatchedBmp);

                // 把originBmp中指定部分按照指定大小画到空白图片上  // CatchRectangle指定originBmp中指定部分
                // 第二个参数指定绘制到空白图片的位置和大小
                // 画完后CatchedBmp不再是空白图片了，而是具有与截取的图片一样的内容
                g.DrawImage(m_originBmp, new Rectangle(0, 0, m_catchRectangle.Width, m_catchRectangle.Height), m_catchRectangle, GraphicsUnit.Pixel);

                // 将图片保存到剪切板中
                Clipboard.SetImage(CatchedBmp);
                ImageScreenShot = Clipboard.GetImage();
                g.Dispose();
                m_catchFinished = false;
                this.BackgroundImage = m_originBmp;                
                CatchedBmp.Dispose();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void OnComfirmButtonClick(object sender, EventArgs e)
        {
            // 新建一个与矩形一样大小的空白图片
            Bitmap CatchedBmp = new Bitmap(m_catchRectangle.Width, m_catchRectangle.Height);
            Graphics graphics = Graphics.FromImage(CatchedBmp);

            // 把originBmp中指定部分按照指定大小画到空白图片上  // CatchRectangle指定originBmp中指定部分
            // 第二个参数指定绘制到空白图片的位置和大小
            // 画完后CatchedBmp不再是空白图片了，而是具有与截取的图片一样的内容
            graphics.DrawImage(m_originBmp, new Rectangle(0, 0, m_catchRectangle.Width, m_catchRectangle.Height), m_catchRectangle, GraphicsUnit.Pixel);

            // 将图片保存到剪切板中
            Clipboard.SetImage(CatchedBmp);
            ImageScreenShot = Clipboard.GetImage();
            graphics.Dispose();
            m_catchFinished = false;
            this.BackgroundImage = m_originBmp;
            CatchedBmp.Dispose();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        /// <summary>
        /// 鼠标移动事件处理程序，即用户改变截图大小的处理
        ///  这个方法是截图功能的核心方法，也就是绘制截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutterMouseMove(object sender, MouseEventArgs e)
        {
            // 确保截图开始
            if (m_catchStart)
            {
                // 新建一个图片对象，让它与屏幕图片相同
                Bitmap copyBmp = (Bitmap)m_originBmp.Clone();
                // 获取鼠标按下的坐标
                Point newPoint = new Point(m_downPoint.X, m_downPoint.Y);
                // 新建画板和画笔
                Graphics g = Graphics.FromImage(copyBmp);
                Pen p = new Pen(Color.Red, 1);
                // 获取矩形的长宽
                int width = Math.Abs(e.X - m_downPoint.X);
                int height = Math.Abs(e.Y - m_downPoint.Y);
                if (e.X < m_downPoint.X)
                {
                    newPoint.X = e.X;
                }
                if (e.Y < m_downPoint.Y)
                {
                    newPoint.Y = e.Y;
                }
                m_catchRectangle = new Rectangle(newPoint, new Size(width, height));
                // 将矩形画在画板上
                g.DrawRectangle(p, m_catchRectangle);
                // 释放目前的画板
                g.Dispose();
                p.Dispose();
                // 从当前窗体创建新的画板
                Graphics g1 = this.CreateGraphics();
                // 将刚才所画的图片画到截图窗体上
                // 为什么不直接在当前窗体画图呢？
                // 如果自己解决将矩形画在窗体上，会造成图片抖动并且有无数个矩形
                // 这样实现也属于二次缓冲技术
                g1.DrawImage(copyBmp, new Point(0, 0));

                g1.Dispose();
                // 释放拷贝图片，防止内存被大量消耗
                copyBmp.Dispose();                
            }
        }

        /// <summary>
        /// 左键弹起结束截屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutterMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 如果截图已经开始，鼠标左键弹起设置截图完成
                if (m_catchStart)
                {
                    m_catchStart = false;
                    m_catchFinished = true;
                    ShowButton();
                }
            }
        }

        private void ShowButton()
        {
            m_closeButton.Visible = true;
            m_comfirmButton.Visible = true;
            var location = m_catchRectangle.Location;

            m_closeButton.Location = new Point(
                m_catchRectangle.Width + location.X - m_comfirmButton.Width - m_closeButton.Width,
                m_catchRectangle.Height + location.Y + 2);
            m_comfirmButton.Location = new Point(
                m_catchRectangle.Width + location.X - m_comfirmButton.Width,
                m_catchRectangle.Height + location.Y + 2);
        }

    }
}
