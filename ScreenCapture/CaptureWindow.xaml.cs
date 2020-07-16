using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewModel.Base;

namespace ScreenCapture
{
    public partial class CaptureWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #region NotifyPropties
        #region NotifyProp FullScreenSource
        private BitmapSource fullScreenSource;
        public BitmapSource FullScreenSource
        {
            get => fullScreenSource;
            set
            {
                fullScreenSource = value;
                OnPropertyChanged(nameof(FullScreenSource));
            }
        }
        #endregion

        #region NotifyProp ToolVisible
        private bool toolVisible;
        public bool ToolVisible
        {
            get => toolVisible;
            set
            {
                toolVisible = value;
                OnPropertyChanged(nameof(ToolVisible));
            }
        }
        #endregion

        #region NotifyProp ToolLeft
        private double toolLeft;
        public double ToolLeft
        {
            get => toolLeft;
            set
            {
                toolLeft = value;
                OnPropertyChanged(nameof(ToolLeft));
            }
        }
        #endregion

        #region NotifyProp ToolTop
        private double toolTop;
        public double ToolTop
        {
            get => toolTop;
            set
            {
                toolTop = value;
                OnPropertyChanged(nameof(ToolTop));
            }
        }
        #endregion

        #region NotifyProp CaptureRect
        private Rect captureRect = new Rect(0, 0, 0, 0);
        public Rect CaptureRect
        {
            get { return captureRect; }
            set
            {
                captureRect = value;
                OnPropertyChanged(nameof(CaptureRect));
            }
        }
        #endregion

        #endregion
        
        public CaptureWindow()
        {
            InitializeComponent();
            this.Height = 800;// SystemInformation.VirtualScreen.Height;
            this.Width = 800;// SystemInformation.VirtualScreen.Width;
            this.Left = 0;
            this.Top = 0;
            DataContext = this;
            var bitmap = GetScreenSnapshot();
            FullScreenSource = ImageHelper.BitmapToBitmapImage(bitmap);
            bitmap.Dispose();
        }
        public Bitmap GetScreenSnapshot()
        {
            System.Drawing.Rectangle rc = SystemInformation.VirtualScreen;
            var bitmap = new Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics memoryGrahics = Graphics.FromImage(bitmap))
            {
                memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
            }
            return bitmap;
        }
        private bool isCaptured;
        private System.Windows.Point point1, point2;
        /// <summary>
        /// //0没有按下 1没有画好的时候mousedown  2已经画好了的时候 mousedown
        /// </summary>
        private int mouseDownState;
        private void OnDragMove(double x, double y)
        {
            captureRect.X = x;
            captureRect.Y = y;
            OnPropertyChanged(nameof(CaptureRect));
            RefreshToolLocation();
        }
        private void OnDragResize(Grid grid, Rect rect)
        {
            CaptureRect = rect;
            RefreshToolLocation();
        }
        #region select area


        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isCaptured = true;
                mouseDownState = 0;
                ClipBorderCanvas.IsHitTestVisible = true;
                RefreshToolLocation();
                ToolVisible = true;
            }
            (sender as UIElement).ReleaseMouseCapture();
        }
        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mouseDownState == 1)
            {
                var p = e.GetPosition(this);
                point2 = p;
                CaptureRect = new Rect(point1, point2);
            }
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (!isCaptured)
                {
                    mouseDownState = 1;
                    point1 = e.GetPosition(this);
                }
            }
            (sender as UIElement).CaptureMouse();
        }
        #endregion
        private void RefreshToolLocation()
        {
            ToolLeft = captureRect.X;
            var y = captureRect.Y;
            if (this.Height - y - captureRect.Height < 35 && y < 35)
                ToolTop = y + captureRect.Height - 30;
            else if (this.Height - y - captureRect.Height < 35)
                ToolTop = y - 35;
            else
                ToolTop = y + captureRect.Height + 5;
        }
        #region tool buttons click

        private void OnCopyClick(object sender, RoutedEventArgs e)
        {
            var rect = new Int32Rect((int)captureRect.X, (int)captureRect.Y, (int)captureRect.Width, (int)captureRect.Height);
            var stride = fullScreenSource.Format.BitsPerPixel * rect.Width / 8;
            byte[] data = new byte[rect.Height * stride];
            fullScreenSource.CopyPixels(rect, data, stride, 0);
            var res = BitmapSource.Create(rect.Width, rect.Height, 0, 0, PixelFormats.Bgr32, null, data, stride);
            System.Windows.Clipboard.SetImage(res);
        }
        
        private void OnDingClick(object sender, RoutedEventArgs e)
        {
            var rect = new Int32Rect((int)captureRect.X, (int)captureRect.Y, (int)captureRect.Width, (int)captureRect.Height);
            var stride = fullScreenSource.Format.BitsPerPixel * rect.Width / 8;
            byte[] data = new byte[rect.Height * stride];
            fullScreenSource.CopyPixels(rect, data, stride, 0);
            var res = BitmapSource.Create(rect.Width, rect.Height, 0, 0, PixelFormats.Bgr32, null, data, stride);
            DingWindow dingWindow = new DingWindow(res);
            dingWindow.Left = rect.X;
            dingWindow.Top = rect.Y;
            dingWindow.Show();
            this.Close();
        }
        #endregion
    }
}
