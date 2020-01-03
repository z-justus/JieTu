namespace OCRTest
{
    partial class Cutter
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Cutter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Cutter";
            this.Text = "Cutter";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.OnCutterLoad);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnCutterMouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Cutter_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnCutterMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Cutter_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Cutter_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}