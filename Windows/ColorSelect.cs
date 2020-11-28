using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Configuration;

using MIDRetail.Business; 
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmSelectFolderColor : MIDFormBase
	{
		private System.ComponentModel.IContainer components;
		private ImageList colorList;
		int buttonWidth = 50, buttonHeight = 50;
		private PictureBox picboxClicked;
		private int x, y;
		private string _imageDir;
		private System.Windows.Forms.Panel colorPanel;
		public FileSystemWatcher myWatcher;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button btnHelp;
		protected string _colorSelected;
		
		public string ColorSelected
		{
			get
			{
				return _colorSelected;
			}
		}

		protected string _colorSelectedFile;
		public string ColorSelectedFile
		{
			get
			{
				return _colorSelectedFile;
			}
		}

		protected int _imageIndexSelected;

		public int ImageIndexSelected
		{
			get
			{
				return _imageIndexSelected;
			}
		}

        public frmSelectFolderColor(SessionAddressBlock aSAB) : base (aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
//			_imageDir = MIDConfigurationManager.AppSettings["ApplicationRoot"] + "\\..\\Graphics";
            // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
            //_imageDir = MIDConfigurationManager.AppSettings[Include.MIDApplicationRoot] + MIDGraphics.GraphicsDir;
            _imageDir = MIDGraphics.MIDGraphicsDir;
            // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				this.closeButton.Click -= new System.EventHandler(this.closeButton_Click);
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
			this.colorPanel = new System.Windows.Forms.Panel();
			this.closeButton = new System.Windows.Forms.Button();
			this.btnHelp = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// colorPanel
			// 
			this.colorPanel.AutoScroll = true;
			this.colorPanel.Location = new System.Drawing.Point(24, 30);
			this.colorPanel.Name = "colorPanel";
			this.colorPanel.Size = new System.Drawing.Size(344, 255);
			this.colorPanel.TabIndex = 3;
			// 
			// closeButton
			// 
			this.closeButton.Location = new System.Drawing.Point(328, 304);
			this.closeButton.Name = "closeButton";
			this.closeButton.TabIndex = 7;
			this.closeButton.Text = "Close";
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// btnHelp
			// 
			this.btnHelp.Location = new System.Drawing.Point(8, 304);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(24, 23);
			this.btnHelp.TabIndex = 8;
			this.btnHelp.Text = "?";
			// 
			// frmSelectFolderColor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 341);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.colorPanel);
			this.Name = "frmSelectFolderColor";
			this.Text = "Select Color";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
//		[STAThread]
//		static void Main() 
//		{
//			Application.Run(new selectLevelFolderColorForm());
//		}
//
//		private void selectLevelFolderColorForm_Load(object sender, System.EventArgs e)
		public void initializeForm()
		{
			int imageIndex = 0;
			colorList = new ImageList();
			DirectoryInfo dir = new DirectoryInfo(_imageDir);
//			FileInfo[] bmpfiles = dir.GetFiles("*.closedfolder.gif");
			FileInfo[] bmpfiles = dir.GetFiles("*" + MIDGraphics.ClosedFolder);
			foreach( FileInfo f in bmpfiles)
			{
				string[] parsedFileName;
				parsedFileName = f.Name.Split('.');
				addImage(f.DirectoryName + @"\" + f.Name, imageIndex, parsedFileName[0]);
				imageIndex++;
			}
			SetReadOnly(true);
		}

		private void addImage(string imageName, int imageIndex, string imageColor)
		{
			Image image;
			if (imageName != "")
			{
				image = Image.FromFile(imageName);
				int imageWidth = image.Width;
				int imageHeight = image.Height;

				SizeF sizef = new SizeF(imageHeight / image.HorizontalResolution,
					imageWidth / image.VerticalResolution);
				float fScale = Math.Min(buttonWidth / sizef.Width,
					buttonHeight / sizef.Height);
				sizef.Width *= fScale;
				sizef.Height *= fScale;
				Size size = Size.Ceiling(sizef);
				Bitmap bitmap = new Bitmap(image, size);

				PictureBox picbox = new PictureBox();
				picbox.Image = bitmap;
				picbox.Location = new Point(x,y) + (Size) AutoScrollPosition;
				picbox.Size = new Size(buttonWidth, buttonHeight);
				picbox.Tag = imageName + "@" + imageIndex + "@" + imageColor;
//				picbox.Click += new EventHandler(buttonOnClick);
				picbox.DoubleClick += new EventHandler(buttonOnDoubleClick);
				this.colorPanel.Controls.Add(picbox);

				AdjustXY(ref x, ref y);
			}
		}


//		void buttonOnClick(object obj, EventArgs ea)
//		{
//			picboxClicked = (PictureBox) obj;
//			this.selectedTextBox.Text = (string) picboxClicked.Tag;;
//
//			if (ImageClicked != null)
//			{
//				ImageClicked(this, EventArgs.Empty);
//			}
//		}

		void buttonOnDoubleClick(object obj, EventArgs ea)
		{
			picboxClicked = (PictureBox) obj;

			string nodeTag;
			string[] parsedNodeTag;

			//Get the Tag of the node
			nodeTag = (string) picboxClicked.Tag;

			//Parse it out by path separator
			parsedNodeTag = nodeTag.Split('@');

			_colorSelectedFile = parsedNodeTag[0];
			_imageIndexSelected = Int32.Parse(parsedNodeTag[1], CultureInfo.CurrentUICulture);
			_colorSelected = parsedNodeTag[2];
//			_colorSelected = (string) picboxClicked.Tag;;
			this.Close();
		}

		protected override void OnResize(EventArgs ea)
		{
			base.OnResize(ea);
			AutoScrollPosition = Point.Empty;
			int x = 0, y = 0;
			if (this.colorPanel != null)
			{
				foreach (Control cntl in this.colorPanel.Controls)
				{
					cntl.Location = new Point(x, y) + (Size) AutoScrollPosition;
					AdjustXY(ref x, ref y);
				}
			}
		}

		void AdjustXY(ref int x, ref int y)
		{
			x += buttonWidth;

			if (x + buttonWidth > this.colorPanel.Width - 
				SystemInformation.HorizontalScrollBarArrowWidth)
			{
				x = 0;
				y += buttonHeight + 5;
			}
		}

		private void closeButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	}
}
