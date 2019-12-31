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

namespace OCRTest
{
    public partial class RecognizeWordsOnPic : Form
    {
        private bool m_recongnizeFinished = false;
        private string m_recongnizeText = string.Empty;

        public RecognizeWordsOnPic()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            m_originalPictureBox.AllowDrop = true;
            ResetProgressBar(false);
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
    }
}
