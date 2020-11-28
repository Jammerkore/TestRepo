using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading;

using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;
using MIDRetail.Business;

namespace MIDRetail.Windows
{
	public class frmLoginMessageDisplay : System.Windows.Forms.Form
	{

        private PictureBox certifedbox;
        private Panel pnlShowMessage;
        private Button btnClose;
        private Label lblMessage;
        private PictureBox pictureBox1;
        private Label RO;

		private System.ComponentModel.Container components = null;

        public frmLoginMessageDisplay(string messageToDisplay)
		{
			InitializeComponent();

            this.lblMessage.Text = messageToDisplay;
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoginMessageDisplay));
            this.certifedbox = new System.Windows.Forms.PictureBox();
            this.pnlShowMessage = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.RO = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.certifedbox)).BeginInit();
            this.pnlShowMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // certifedbox
            // 
            this.certifedbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.certifedbox.Location = new System.Drawing.Point(519, 591);
            this.certifedbox.Name = "certifedbox";
            this.certifedbox.Size = new System.Drawing.Size(60, 72);
            this.certifedbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.certifedbox.TabIndex = 15;
            this.certifedbox.TabStop = false;
            this.certifedbox.Visible = false;
            // 
            // pnlShowMessage
            // 
            this.pnlShowMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlShowMessage.Controls.Add(this.btnClose);
            this.pnlShowMessage.Controls.Add(this.lblMessage);
            this.pnlShowMessage.Location = new System.Drawing.Point(8, 470);
            this.pnlShowMessage.Name = "pnlShowMessage";
            this.pnlShowMessage.Size = new System.Drawing.Size(482, 198);
            this.pnlShowMessage.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.SystemColors.Desktop;
            this.btnClose.Location = new System.Drawing.Point(251, 140);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Window;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(133, 15);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(320, 56);
            this.lblMessage.TabIndex = 12;
            this.lblMessage.Text = "Another client instance of the application is already running.";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(584, 439);
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // RO
            // 
            this.RO.AutoSize = true;
            this.RO.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RO.Location = new System.Drawing.Point(205, 444);
            this.RO.Name = "RO";
            this.RO.Size = new System.Drawing.Size(180, 22);
            this.RO.TabIndex = 19;
            this.RO.Text = "Retail Optimization";
            this.RO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmLoginMessageDisplay
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 13);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(591, 680);
            this.Controls.Add(this.RO);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.certifedbox);
            this.Controls.Add(this.pnlShowMessage);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(520, 380);
            this.Name = "frmLoginMessageDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.certifedbox)).EndInit();
            this.pnlShowMessage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void Login_Load(object sender, System.EventArgs e)
		{
			try
			{
				//Cursor.Current = Cursors.WaitCursor;
                // this.Icon = new System.Drawing.Icon(MIDGraphics.ImageDir + "\\" + MIDGraphics.ApplicationIcon);  // TT#4658 - AGallagher - Remove Windows 7 logo from application
                // displayWindows7Image();   // TT#4658 - AGallagher - Remove Windows 7 logo from application		
			}
			catch( Exception exception )
			{
				MessageBox.Show(this, exception.Message );
			}
			finally
			{
				//Cursor.Current = Cursors.Default;
			}
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

	
   

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   
	}
}
