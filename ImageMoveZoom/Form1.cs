using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageMoveZoom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ImageList imageList1 = new ImageList();
        private void Form1_Load(object sender, EventArgs e)
        {
            imageList1.ImageSize = new System.Drawing.Size(120, 120);
            this.listView1.LargeImageList = imageList1;
            DirectoryInfo diImg = new DirectoryInfo(Application.StartupPath + "\\Images");
            foreach (FileInfo fi in diImg.GetFiles())
            {
                Image img = Image.FromFile(fi.FullName);
                imageList1.Images.Add(fi.Name, img);
            }
            for (int i=0; i<imageList1.Images.Count;i++)
            {
                ListViewItem lvi = new ListViewItem(new string[] { imageList1.Images.Keys[i] });
                lvi.ImageKey = imageList1.Images.Keys[i];
                this.listView1.Items.Add(lvi);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem lvi = this.listView1.SelectedItems[0];
                string key = lvi.SubItems[0].Text.ToString();
                PicViewForm pvf = new PicViewForm();
                Image img = Image.FromFile(Application.StartupPath + "\\Images\\" + key);
                pvf.InPutBuffer(img);
                pvf.Text += " - " + key;
                pvf.ShowDialog();
            }
        }
    }
}
