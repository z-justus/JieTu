using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using BaiduAIAPI;
using System.IO;
using ImageMoveZoom;
using System.Web;
using Utilities;
using Module;
using System.Threading;
using System.Runtime.InteropServices;
using ScreenCapture;

namespace OCRTest
{
    public partial class RecognizeWordsOnPic : Form
    {
        private ScreenForm screenForm = new ScreenForm();
        private Bitmap currentBitmap;
        public Image ScreenShotImage { get; set; }


        private bool m_recongnizeFinished = false;
        private string m_recongnizeText = string.Empty;

        public RecognizeWordsOnPic()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            m_originalPictureBox.AllowDrop = true;
            ResetProgressBar(false);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            screenForm.ScreenShotOk += new EventHandler(OnScreenShotOkClick);
        }

        private void ResetProgressBar(bool visible, int max = 100, int value = 0)
        {
            m_progressBar.Maximum = max;
            m_progressBar.Value = value;
            m_progressBar.Visible = visible;
        }

        //选择图片
        private void OnSelectPictureButtonClick(object sender, EventArgs e)
        {
            try
            {
                m_originalPictureBox.Image = GetImage();
                m_imagePathText.Focus();
                Recognize();
            }
            catch
            {
                MessageBox.Show("选择文件出错！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private Image GetImage()
        {
            using (OpenFileDialog picDlg = new OpenFileDialog())
            {
                picDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                picDlg.Filter = "图片|*.bmp;*.jpg;*.png";
                picDlg.RestoreDirectory = true;
                picDlg.FilterIndex = 1;
                if (picDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return null;
                }
                m_imagePathText.Text = picDlg.FileName;
                FileStream fileImage = new FileStream(picDlg.FileName, FileMode.Open, FileAccess.Read);
                Image image = Image.FromStream(fileImage);
                if (image == null || !VerificationImage(image))
                {
                    return null;
                }
                fileImage.Dispose();
                return image;
            }
        }

        //双击预览图片
        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (m_originalPictureBox.Image == null)
            {
                return;
            }
            using (PicViewForm picView = new PicViewForm())
            {
                picView.InPutBuffer(m_originalPictureBox.Image);
                picView.ShowDialog();
            }
        }

        //识别文字
        private void Recognize()
        {
            m_recongnizeFinished = false;
            m_resultTextBox.Text = string.Empty;
            m_originalPictureBox.Refresh();
            m_resultTextBox.Refresh();
            if (m_originalPictureBox.Image == null)
            {
                return;
            }
            
            Task.Run(RemoteBaiduRecognize);
            RunProgressBar();
            m_resultTextBox.Text = m_recongnizeText;
        }

        private void RemoteBaiduRecognize()
        {
            try
            {
                AccessTokenView token = AccessToken.GetAccessToken();
                if (!token.IsSuccess)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show("获取Token失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                    m_recongnizeText = string.Empty;
                    return;
                }
                m_recongnizeText = RecognizeText(token.SuccessModel.access_token);
                m_recongnizeFinished = true;
            }
            catch
            {
                MessageBox.Show("识别过程出错！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                m_recongnizeFinished = true;
            }
        }

        private string RecognizeText(string tokenString)
        {
            string host = AccessToken.GetBaiduRecognizeUrl() + "?access_token=" + tokenString;//参数参考https://ai.baidu.com/docs#/OCR-API/top
            Bitmap bitmap = new Bitmap(m_originalPictureBox.Image);
            string imageByteValue = ImageConversion.ImageToByte64String(bitmap, System.Drawing.Imaging.ImageFormat.Jpeg);
            if (!VerificationImage(imageByteValue))
            {
                return string.Empty;
            }
            string requestParas = "detect_direction=" + false + "&image=" + HttpUtility.UrlEncode(imageByteValue);
            string result = HttpRequestHelper.Post(host, requestParas);
            RecognizeResultModel recognizeResult = JsonExtends.ToObject<RecognizeResultModel>(result);
            if (recognizeResult == null || recognizeResult.words_result == null)
            {
                return string.Empty;
            }
            string[] wordArray = recognizeResult.words_result.Select(a => a.words).ToArray();
            return string.Join("\r\n", wordArray);
        }

        private void RunProgressBar()
        {
            ResetProgressBar(true);
            int value = 1;
            while (true)
            {
                bool maxValueNotEnough = value == m_progressBar.Maximum;
                int newMaxValue = maxValueNotEnough ? m_progressBar.Maximum + 1 : m_progressBar.Maximum;
                if (!m_recongnizeFinished)
                {
                    ResetProgressBar(true, newMaxValue, value);
                    value++;
                    Thread.Sleep(10);
                }
                else
                {
                    ResetProgressBar(true, newMaxValue, newMaxValue);
                    break;
                }
            }
            ResetProgressBar(false);
        }

        private bool VerificationImage(Image image)
        {
            int minSidePX = Math.Min(image.Size.Width, image.Size.Height);
            int maxSidePX = Math.Max(image.Size.Width, image.Size.Height);
            if (minSidePX < 15)
            {
                MessageBox.Show("图片最小边至少15px！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (maxSidePX > 4096)
            {
                MessageBox.Show("图片最大边不能超过4096px！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private bool VerificationImage(string imageByte64)
        {
            Encoding encoding = Encoding.Default;
            string urlEncodeImage = HttpUtility.UrlEncode(imageByte64);
            byte[] urlEncodeImageBytes = encoding.GetBytes(urlEncodeImage);
            if (urlEncodeImageBytes.Length > 1024 * 1024 * 4)
            {
                MessageBox.Show("图片过大！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void OnOriginalPictureBoxDragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array files = e.Data.GetData(DataFormats.FileDrop) as Array;
                string path;
                if (!GetCorrectImagePath(files, out path))
                {
                    return;
                }
                m_imagePathText.Text = path;
                m_originalPictureBox.Image = Image.FromFile(path);
                Recognize();
            }
            catch
            {
                MessageBox.Show("拖动图片出错！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnOriginalPictureBoxDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OnPasteMenuClick(object sender, EventArgs e)
        {
            PasteAndRecognize();
        }

        private void PasteAndRecognize()
        {
            try
            {
                Image image = Clipboard.GetImage();
                if (image == null)
                {
                    MessageBox.Show("请选择一张文字图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                m_originalPictureBox.Image = image;
                Recognize();
            }
            catch
            {
                MessageBox.Show("粘贴图片出错！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool GetCorrectImagePath(Array files, out string correctImagePath)
        {
            correctImagePath = string.Empty;
            if (files == null || files.Length != 1)
            {
                MessageBox.Show("请选择一张文字图片（仅限于.bmp、.jpg、.png格式）！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string path = files.GetValue(0).ToString();
            string extensionName = Path.GetExtension(path).ToLower();
            if (!File.Exists(path) || (extensionName != ".bmp" && extensionName != ".jpg" && extensionName != ".png"))
            {
                MessageBox.Show("请选择一张文字图片（仅限于.bmp、.jpg、.png格式）！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            correctImagePath = path;
            return true;
        }

        private void RecognizeWordsOnPic_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            {
                PasteAndRecognize();
            }
        }

        private void OnScreenShotButtonClick(object sender, EventArgs e)
        {
            try
            {
                timer1.Start();
            }
            catch
            {
                MessageBox.Show("选择文件出错！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }           
        }

        #region 伪录屏 

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        private const Int32 CURSOR_SHOWING = 0x00000001;
        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINT ptScreenPos;
        }

        private void Timer1_Tick_1(object sender, EventArgs e)
        {
            Image myimage = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            Graphics g = Graphics.FromImage(myimage);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height));
            CURSORINFO pci;
            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out pci);
            System.Windows.Forms.Cursor cur = new System.Windows.Forms.Cursor(pci.hCursor);
            cur.Draw(g, new Rectangle(pci.ptScreenPos.x - 10, pci.ptScreenPos.y - 10, cur.Size.Width, cur.Size.Height));
            m_originalPictureBox.Image = myimage;
        }

        #endregion

        private void OnScreenShotTwoButtonClick(object sender, EventArgs e)
        {
            try
            {
                this.Hide();//隐藏当前
                Thread.Sleep(500);

                #region 第二种方法  
                GetImageTwo();
                #endregion

                #region 第一种方法

                //this.currentBitmap = GetScreen();
                //screenForm.BackgroundImage = this.currentBitmap;
                //screenForm.StartPosition = FormStartPosition.Manual;//起始位置
                //screenForm.ShowDialog();
                ////m_originalPictureBox.Image = ScreenShotImage;
                ////Recognize();
                #endregion

            }
            catch(Exception ex)
            {
                MessageBox.Show("选择文件出错！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Show();
            }
        }

        #region 第二种需要方法

        Cutter m_cutter = null;

        private void GetImageTwo()
        {
            // 新建一个和屏幕大小相同的图片
            Bitmap CatchBmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);

            // 创建一个画板，让我们可以在画板上画图  // 这个画板也就是和屏幕大小一样大的图片
            // 我们可以通过Graphics这个类在这个空白图片上画图
            Graphics g = Graphics.FromImage(CatchBmp);

            // 把屏幕图片拷贝到我们创建的空白图片 CatchBmp中
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));

            // 创建截图窗体
            m_cutter = new Cutter();

            // 指示窗体的背景图片为屏幕图片
            m_cutter.BackgroundImage = CatchBmp;
            // 显示窗体
            //cutter.Show();
            // 如果Cutter窗体结束，则从剪切板获得截取的图片，并显示在聊天窗体的发送框中
            if (m_cutter.ShowDialog() == DialogResult.OK)
            {
                IDataObject iData = Clipboard.GetDataObject();

                this.Show();
                if (iData.GetDataPresent(DataFormats.Bitmap))
                {
                    if (m_cutter.ImageScreenShot==null)
                    {
                        return;
                    }
                    m_originalPictureBox.Image = m_cutter.ImageScreenShot;
                    Recognize();
                }
            }
        }

        #endregion


        #region 第一种截屏所需方法
        public Bitmap GetScreen()
        {
            //获取整个屏幕图像,不包括任务栏
            Rectangle ScreenArea = Screen.GetWorkingArea(this);
            Bitmap bitmap = new Bitmap(ScreenArea.Width, ScreenArea.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(ScreenArea.Width, ScreenArea.Height));
            }
            return bitmap;
        }

        
        private void OnScreenShotOkClick(object sender, EventArgs e)
        {
            GetBackImage();           
        }

        private void GetBackImage()
        {
            Bitmap bitmap;
            if (screenForm.End.X > screenForm.Start.X && screenForm.End.Y > screenForm.Start.Y)
            {
                bitmap = ToRightAndBottom();
            }
            else if (screenForm.End.X > screenForm.Start.X && screenForm.End.Y <= screenForm.Start.Y)
            {
                bitmap = ToRightAndTop();
            }
            else if (screenForm.End.X <= screenForm.Start.X && screenForm.End.Y > screenForm.Start.Y)
            {
                bitmap = ToLeftAndBottom();
            }
            else
            {
                bitmap = ToLeftAndTop();
            }
            ScreenShotImage = bitmap;
            this.Show();
        }

        private Bitmap ToRightAndBottom()
        {
            Bitmap bitmap = new Bitmap(screenForm.End.X - screenForm.Start.X, screenForm.End.Y - screenForm.Start.Y);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int w = screenForm.End.X - screenForm.Start.X;
                int h = screenForm.End.Y - screenForm.Start.Y;
                Rectangle destRect = new Rectangle(0, 0, w + 1, h + 1);//在画布上要显示的区域（记得像素加1）
                Rectangle srcRect = new Rectangle(screenForm.Start.X, screenForm.Start.Y - 15, w + 1, h + 1);//图像上要截取的区域
                g.DrawImage(currentBitmap, destRect, srcRect, GraphicsUnit.Pixel);//加图像绘制到画布上
            }

            return bitmap;
        }

        private Bitmap ToRightAndTop()
        {
            Bitmap bitmap = new Bitmap(screenForm.End.X - screenForm.Start.X, screenForm.Start.Y - screenForm.End.Y);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int w = screenForm.End.X - screenForm.Start.X;
                int h = screenForm.Start.Y - screenForm.End.Y;
                Rectangle destRect = new Rectangle(0, 0, w + 1, h + 1);//在画布上要显示的区域（记得像素加1）
                Rectangle srcRect = new Rectangle(screenForm.Start.X, screenForm.End.Y + 15, w + 1, h + 1);//图像上要截取的区域
                g.DrawImage(currentBitmap, destRect, srcRect, GraphicsUnit.Pixel);//加图像绘制到画布上
            }

            return bitmap;
        }

        private Bitmap ToLeftAndBottom()
        {
            Bitmap bitmap = new Bitmap(screenForm.Start.X - screenForm.End.X, screenForm.End.Y - screenForm.Start.Y);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int w = screenForm.Start.X - screenForm.End.X;
                int h = screenForm.End.Y - screenForm.Start.Y;
                Rectangle destRect = new Rectangle(0, 0, w + 1, h + 1);//在画布上要显示的区域（记得像素加1）
                Rectangle srcRect = new Rectangle(screenForm.End.X, screenForm.Start.Y - 15, w + 1, h + 1);//图像上要截取的区域
                g.DrawImage(currentBitmap, destRect, srcRect, GraphicsUnit.Pixel);//加图像绘制到画布上
            }

            return bitmap;
        }

        private Bitmap ToLeftAndTop()
        {
            Bitmap bitmap = new Bitmap(screenForm.Start.X - screenForm.End.X, screenForm.Start.Y - screenForm.End.Y);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int w = screenForm.Start.X - screenForm.End.X;
                int h = screenForm.Start.Y - screenForm.End.Y;
                Rectangle destRect = new Rectangle(0, 0, w + 1, h + 1);//在画布上要显示的区域（记得像素加1）
                Rectangle srcRect = new Rectangle(screenForm.End.X, screenForm.End.Y - 15, w + 1, h + 1);//图像上要截取的区域
                g.DrawImage(currentBitmap, destRect, srcRect, GraphicsUnit.Pixel);//加图像绘制到画布上
            }

            return bitmap;
        }

        #endregion

        /// <summary>
        /// wpf截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            CaptureWindow capture = new CaptureWindow();
            capture.
        }
    }
}
