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
    public partial class ScreenForm : Form
    {
        private Point start = new Point(0, 0);
        public Point Start
        {
            get { return start; }

            set { start = value; }
        }

        private Point end = new Point(0, 0);

        public event EventHandler ScreenShotOk;

        public Point End
        {
            get { return end; }

            set { end = value; }
        }


        private Button button = new Button();

        public ScreenForm()
        {
            InitializeComponent();
            //以下采用双缓冲方式，减少闪烁
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            //panel1.BackColor = Color.FromArgb(150, 0, 0, 0);
            //panel1.MouseDown += OnScreenFormMouseDown;
            //panel1.MouseUp += OnScreenFormMouseUp;
            //panel1.MouseMove += OnScreenFormMouseMove;


            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.Red;
            button.FlatAppearance.BorderSize =2;
            button.BackColor = Color.Transparent;
            button.Visible = false;
            this.Controls.Add(button);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Top = 0;
            this.Left = 0;
            //当前窗口最大化，且不显示标题栏
            Rectangle ScreenArea = Screen.GetWorkingArea(this);
            this.MaximumSize = new Size(ScreenArea.Width, ScreenArea.Height);

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            //鼠标，十字键
            this.Cursor = Cursors.Arrow;
            this.BackgroundImageLayout = ImageLayout.Center;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //using (Graphics g = this.CreateGraphics())
            //{
            //    g.DrawRectangle(new Pen(Color.Red), new Rectangle(Start, new Size(End.X - Start.X, End.Y - Start.Y)));
            //}
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScreenFormMouseDown(object sender, MouseEventArgs e)
        {
            //如果就左键
            if (e.Button == MouseButtons.Left)
            {
                Start = e.Location;
                button.Size = new Size(1,1);
                button.Location = e.Location;
                button.Visible = true;
            }
        }


        /// <summary>
        /// 鼠标起来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScreenFormMouseUp(object sender, MouseEventArgs e)
        {
            //如果就左键
            if (e.Button == MouseButtons.Left)
            {
                button.Visible = false;
                //如果起始位置和结束位置一致，则返回
                if (e.Location == Start)
                {
                    return;
                }
                End = e.Location;
                if (ScreenShotOk != null)
                {
                    ScreenShotOk(this, null);
                }
                this.Close();
            }
        }

        

        private void OnScreenFormMouseMove(object sender, MouseEventArgs e)
        {
            //如果就左键
            if (e.Button == MouseButtons.Left)
            {
                End = e.Location;
                SetButton();
                //this.Invalidate();
            }
        }

        private void SetButton()
        {
            if (End.X > Start.X && End.Y > Start.Y)
            {
                button.Size = new Size(End.X - Start.X, End.Y - Start.Y);
            }
            else if (End.X > Start.X && End.Y <= Start.Y)
            {
                button.Location = new Point(Start.X, End.Y);
                button.Size = new Size(End.X - Start.X, Start.Y - End.Y);
            }
            else if (End.X <= Start.X && End.Y > Start.Y)
            {
                button.Location = new Point(End.X,Start.Y);
                button.Size = new Size(Start.X - End.X, End.Y - Start.Y);
            }
            else
            {
                button.Location = End;
                button.Size = new Size(Start.X - End.X, Start.Y - End.Y);
            }
        }
    }
}
