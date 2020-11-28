using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetailInstaller
{
    public partial class InstallerControl : UserControl
    {
        ToolTip tt = new ToolTip();
        InstallerFrame frame = null;
        public string Parent;
        public string LookupType;

        public InstallerControl()
        {
            InitializeComponent();
        }

        public InstallerControl(InstallerFrame p_frame)
        {
            frame = p_frame;
            InitializeComponent();
        }

        public void displayFromDefault()
        {
            Image image;
            try
            {
                image = Image.FromFile(Application.StartupPath + @"\FromDefault.bmp");
                SizeF sizef = new SizeF(pictureBox1.Width, pictureBox1.Height);
                Size size = Size.Ceiling(sizef);
                Bitmap bitmap = new Bitmap(image, size);
                pictureBox1.Image = bitmap;
                tt.SetToolTip(pictureBox1, frame.GetToolTipText("config_FromDefaultSetting")); 
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void displayFromConfig()
        {
            try
            {
                pictureBox1.Image = null;
                tt.SetToolTip(pictureBox1, frame.GetToolTipText("config_FromProcessConfig")); 
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void displayFromMIDSettings()
        {
            Image image;
            try
            {
                //this.Enabled = false;
                image = Image.FromFile(Application.StartupPath + @"\FromMIDSettings.bmp");
                SizeF sizef = new SizeF(pictureBox1.Width, pictureBox1.Height);
                Size size = Size.Ceiling(sizef);
                Bitmap bitmap = new Bitmap(image, size);
                pictureBox1.Image = bitmap;
                tt.SetToolTip(pictureBox1, frame.GetToolTipText("config_FromMIDSetting"));
                foreach (Control control in this.Controls)
                {
                    if (control is PictureBox)
                    {
                    }
                    else
                    {
                        control.Enabled = false;
                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        virtual public void SetTooltipDescription(string aDescription)
        {
            throw new Exception("Cannot call this method");
        }

        public void SetTooltipControl(Control aControl, string aDescription)
        {
            tt.SetToolTip(aControl, aDescription);
        }
    }
}
