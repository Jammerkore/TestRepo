using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class StatusBarWindow : Form
    {
        private delegate void UpdateStatusTextDelegate(string s);
        private void UpdateStatusText(string s)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateStatusTextDelegate(UpdateStatusText), new object[] { s });
            }
            else
            {
                this.lblStatusText.Text = s;
            }
        }
        public string StatusText
        {
            set { UpdateStatusText(value); }
        }

        private delegate void UpdateStatusBarDelegate(int v);
        private void UpdateStatusBar(int v)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateStatusBarDelegate(UpdateStatusBar), new object[] { v });
            }
            else
            {
                this.pbProgressBar.Value = v;
            }
        }
        public int StatusBarValue
        {
            set { UpdateStatusBar(value); }
        }

        public StatusBarWindow()
        {
            InitializeComponent();
            // Begin TT#1183 - JSmith - Windows 7 Logo
            // displayWindows7Image();   // TT#4658 - AGallagher - Remove Windows 7 logo from application
            // End TT#1183
        }

        // Begin TT#1183 - JSmith - Windows 7 Logo
        private void displayWindows7Image()
        {
            Image image;
            try
            {
                image = Image.FromFile(MIDGraphics.ImageDir + "\\" + MIDGraphics.Windows7CertImage);
                SizeF sizef = new SizeF(certifedbox.Width, certifedbox.Height);
                Size size = Size.Ceiling(sizef);
                Bitmap bitmap = new Bitmap(image, size);
                certifedbox.Image = bitmap;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        // End TT#1183
    }
}
