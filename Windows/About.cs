using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
//using System.IO;
//using System.Runtime.Remoting;
using System.Windows.Forms;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Security;
//using System.Text.RegularExpressions;

using MIDRetail.Business;
//using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;


namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class frmAbout : MIDFormBase
	{
        //[StructLayout(LayoutKind.Sequential)]
        //    public struct OSVERSIONINFOEX
        //{
        //    public int dwOSVersionInfoSize;
        //    public int dwMajorVersion;
        //    public int dwMinorVersion;
        //    public int dwBuildNumber;
        //    public int dwPlatformId;
        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
        //    public string szCSDVersion;
        //    public short wServicePackMajor;
        //    public short wServicePackMinor;
        //    public short wSuiteMask;
        //    public byte wProductType;
        //    public byte wReserved;
        //}
        //[DllImport("kernel32.Dll")]
        //public static extern short GetVersionEx(ref OSVERSIONINFOEX o);
        //[DllImport("user32.dll")]
        //public static extern int GetSystemMetrics(int nIndex);
        //public const int SM_TABLETPC = 86;

        //public enum OSVersion
        //{
        //    TabletPC,
        //    TabletPCSP2Over,
        //    NormalXP,
        //    Normal2000,
        //    None
        //};

		private SessionAddressBlock _SAB;
		private bool _detailsShown = false;
		private int cHideHeight = 0;
		private int cShowHeight = 0;
		
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ListBox listBoxAssembly;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ListBox listBoxConfiguration;
        private System.Windows.Forms.Button btnDetails;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.ListBox listBoxAddOn;
        private PictureBox certifedbox;
        private LinkLabel linkLblSupportWebPage;
        private Label lblSupportWebPage;
        private Label lblWebPage;
        private LinkLabel linkLblWebPage;
        private ListBox lbDetails;
        private Button btnok;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmAbout(SessionAddressBlock aSAB) : base (aSAB)
		{
			_SAB = aSAB;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.Icon = MIDGraphics.GetIcon(MIDGraphics.ApplicationIcon);
			this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_About);
			displayApplicationImage();
            // Begin TT#1183 - JSmith - Windows 7 Logo
            displayWindows7Image();
            // End TT#1183

            // Begin TT#698 - JSmith - Enhance environment information
            //string assemblyName = "MIDAllocation.exe";
            //string productName = "MID Advanced Allocation"; 
            //string productVersion = "unavailable";
            //string legalCopyright = "Copyright © Logility, Inc. 2018";
            //string companyName = "Management Information Disciplines, Inc.";
            //string lastUpdate = "unavailable";
            //string systemVersion = "unavailable";
          
            // End TT#698
//			try
//			{
//				System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
//				assemblyName = p.ProcessName + ".exe";
//			}
//			catch
//			{
//			}

		

            // Begin TT#292-MD - JSmith - Enhanced Help - About
            // Begin TT#698 - JSmith - Enhance environment information
            //label2.Text = productName + "  Version:" + productVersion ;
            //label2.Text += Environment.NewLine + "Last updated on: " + lastUpdate;
            //label2.Text += Environment.NewLine + "Operating system: " + DetermineOperatingSystem();

//            label2.Text = productName + "  " + MIDText.GetTextOnly(eMIDTextCode.lbl_EnvVersion) + ": " + productVersion;
//            label2.Text += Environment.NewLine + lastUpdate;
//            label2.Text += Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.lbl_EnvEnvironment) + ": " + SAB.ControlServerSession.GetMIDEnvironment();
//            label2.Text += Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.lbl_EnvOperatingSystem) + ": " + OSInfo.Name;
//            // End TT#698
//            if (OSInfo.Bits == 32)
//            {
//                label2.Text += " (32 bit)";
//            }
//            else
//            {
//                label2.Text += " (64 bit)";
//            }
//            label2.Text += Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.lbl_EnvVersion) + ": " + OSInfo.VersionString;
//            label2.Text += Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.lbl_EnvEdition) + ": " + OSInfo.Edition;
//            label2.Text += Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.lbl_EnvServicePack) + ": " + OSInfo.ServicePack;
//            label2.Text += Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.lbl_EnvFramework) + ": " + systemVersion;
////			label2.Text += Environment.NewLine + "Copyright " + legalCopyright + " " + companyName;
//            label2.Text += Environment.NewLine + legalCopyright;
//            label2.Text += Environment.NewLine + "Email: support@midretail.com";
//            label2.Text += Environment.NewLine + "After Hours Support Line - 317-222-3127"; // TT#258-MD  AGallagher - Add After Hours Support Line to the Help About 
            
  


    
            lblWebPage.Text = EnvironmentInfo.MIDInfo.webPageLabelText;
            lblSupportWebPage.Text = EnvironmentInfo.MIDInfo.supportWebPageLabelText;

            lbDetails.Items.Add(EnvironmentInfo.MIDInfo.productName + "  " + EnvironmentInfo.MIDInfo.environmentVersionLabelText + EnvironmentInfo.MIDInfo.productVersion);
            lbDetails.Items.Add(EnvironmentInfo.MIDInfo.emailSupportLabelText + EnvironmentInfo.MIDInfo.emailSupport);
            lbDetails.Items.Add(EnvironmentInfo.MIDInfo.afterHoursPhoneLabelText + EnvironmentInfo.MIDInfo.afterHoursPhone);
            lbDetails.Items.Add(EnvironmentInfo.MIDInfo.lastUpdate);
            lbDetails.Items.Add(EnvironmentInfo.MIDInfo.environmentVersionLabelText + EnvironmentBusinessInfo.GetSessionEnviroment(_SAB));

            lbDetails.Items.Add(EnvironmentInfo.MIDInfo.opertatingSystemLabelText + EnvironmentInfo.MIDInfo.opertatingSystem);
            lbDetails.Items.Add(EnvironmentInfo.MIDInfo.environmentVersionLabelText  + EnvironmentInfo.OSInfo.VersionString);
            lbDetails.Items.Add(EnvironmentInfo.MIDInfo.environmentEditionLabelText + EnvironmentInfo.OSInfo.Edition);
            lbDetails.Items.Add(EnvironmentInfo.MIDInfo.environmentServicePackLabelText + EnvironmentInfo.OSInfo.ServicePack);
            //lbDetails.Items.Add(EnvironmentInfo.MIDInfo.environmentFrameworkLabelText + EnvironmentInfo.MIDInfo.systemVersion);
            lbDetails.Items.Add(EnvironmentInfo.MIDInfo.legalCopyright);

            // End TT#292-MD - JSmith - Enhanced Help - About

			AddConfigurationInformation();
			AddAssemblyInformation();
			AddAddOnInformation();


			cShowHeight = this.Height;
			cHideHeight = this.Height - this.tabControl1.Height + 5;
			HideDetails();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.listBoxAssembly = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listBoxConfiguration = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listBoxAddOn = new System.Windows.Forms.ListBox();
            this.btnDetails = new System.Windows.Forms.Button();
            this.certifedbox = new System.Windows.Forms.PictureBox();
            this.linkLblSupportWebPage = new System.Windows.Forms.LinkLabel();
            this.lblSupportWebPage = new System.Windows.Forms.Label();
            this.lblWebPage = new System.Windows.Forms.Label();
            this.linkLblWebPage = new System.Windows.Forms.LinkLabel();
            this.lbDetails = new System.Windows.Forms.ListBox();
            this.btnok = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.certifedbox)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(47, 34);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(87, 79);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // listBoxAssembly
            // 
            this.listBoxAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxAssembly.Location = new System.Drawing.Point(16, 8);
            this.listBoxAssembly.Name = "listBoxAssembly";
            this.listBoxAssembly.Size = new System.Drawing.Size(388, 251);
            this.listBoxAssembly.TabIndex = 35;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(8, 197);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(428, 296);
            this.tabControl1.TabIndex = 37;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listBoxConfiguration);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(420, 270);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Configuration";
            // 
            // listBoxConfiguration
            // 
            this.listBoxConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxConfiguration.Location = new System.Drawing.Point(16, 8);
            this.listBoxConfiguration.Name = "listBoxConfiguration";
            this.listBoxConfiguration.Size = new System.Drawing.Size(388, 238);
            this.listBoxConfiguration.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listBoxAssembly);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(420, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Assemblies";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.listBoxAddOn);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(420, 270);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Add-Ons";
            // 
            // listBoxAddOn
            // 
            this.listBoxAddOn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxAddOn.Location = new System.Drawing.Point(16, 8);
            this.listBoxAddOn.Name = "listBoxAddOn";
            this.listBoxAddOn.Size = new System.Drawing.Size(391, 251);
            this.listBoxAddOn.TabIndex = 0;
            // 
            // btnDetails
            // 
            this.btnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDetails.Location = new System.Drawing.Point(364, 501);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(72, 23);
            this.btnDetails.TabIndex = 39;
            this.btnDetails.Text = "Details >>";
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // certifedbox
            // 
            this.certifedbox.Location = new System.Drawing.Point(58, 129);
            this.certifedbox.Name = "certifedbox";
            this.certifedbox.Size = new System.Drawing.Size(56, 59);
            this.certifedbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.certifedbox.TabIndex = 41;
            this.certifedbox.TabStop = false;
            this.certifedbox.Visible = false;
            // 
            // linkLblSupportWebPage
            // 
            this.linkLblSupportWebPage.AutoSize = true;
            this.linkLblSupportWebPage.Location = new System.Drawing.Point(245, 34);
            this.linkLblSupportWebPage.Name = "linkLblSupportWebPage";
            this.linkLblSupportWebPage.Size = new System.Drawing.Size(141, 13);
            this.linkLblSupportWebPage.TabIndex = 42;
            this.linkLblSupportWebPage.TabStop = true;
            this.linkLblSupportWebPage.Text = "https://connect.logility.com/";
            this.linkLblSupportWebPage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblSupportWebPage_LinkClicked);
            // 
            // lblSupportWebPage
            // 
            this.lblSupportWebPage.AutoSize = true;
            this.lblSupportWebPage.Location = new System.Drawing.Point(171, 34);
            this.lblSupportWebPage.Name = "lblSupportWebPage";
            this.lblSupportWebPage.Size = new System.Drawing.Size(75, 13);
            this.lblSupportWebPage.TabIndex = 43;
            this.lblSupportWebPage.Text = "Support Page:";
            this.lblSupportWebPage.Click += new System.EventHandler(this.lblSupportWebPage_Click);
            // 
            // lblWebPage
            // 
            this.lblWebPage.AutoSize = true;
            this.lblWebPage.Location = new System.Drawing.Point(185, 16);
            this.lblWebPage.Name = "lblWebPage";
            this.lblWebPage.Size = new System.Drawing.Size(61, 13);
            this.lblWebPage.TabIndex = 44;
            this.lblWebPage.Text = "Web Page:";
            // 
            // linkLblWebPage
            // 
            this.linkLblWebPage.AutoSize = true;
            this.linkLblWebPage.Location = new System.Drawing.Point(245, 16);
            this.linkLblWebPage.Name = "linkLblWebPage";
            this.linkLblWebPage.Size = new System.Drawing.Size(125, 13);
            this.linkLblWebPage.TabIndex = 45;
            this.linkLblWebPage.TabStop = true;
            this.linkLblWebPage.Text = "http://www.Logility.com/";
            this.linkLblWebPage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblWebPage_LinkClicked_1);
            // 
            // lbDetails
            // 
            this.lbDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDetails.FormattingEnabled = true;
            this.lbDetails.Location = new System.Drawing.Point(174, 57);
            this.lbDetails.Name = "lbDetails";
            this.lbDetails.Size = new System.Drawing.Size(258, 134);
            this.lbDetails.TabIndex = 46;
            // 
            // btnok
            // 
            this.btnok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnok.Location = new System.Drawing.Point(176, 501);
            this.btnok.Name = "btnok";
            this.btnok.Size = new System.Drawing.Size(75, 23);
            this.btnok.TabIndex = 47;
            this.btnok.Text = "OK";
            this.btnok.UseVisualStyleBackColor = true;
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // frmAbout
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(452, 531);
            this.Controls.Add(this.btnok);
            this.Controls.Add(this.lbDetails);
            this.Controls.Add(this.linkLblWebPage);
            this.Controls.Add(this.lblWebPage);
            this.Controls.Add(this.lblSupportWebPage);
            this.Controls.Add(this.linkLblSupportWebPage);
            this.Controls.Add(this.certifedbox);
            this.Controls.Add(this.btnDetails);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About MID Advanced Allocation";
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.btnDetails, 0);
            this.Controls.SetChildIndex(this.certifedbox, 0);
            this.Controls.SetChildIndex(this.linkLblSupportWebPage, 0);
            this.Controls.SetChildIndex(this.lblSupportWebPage, 0);
            this.Controls.SetChildIndex(this.lblWebPage, 0);
            this.Controls.SetChildIndex(this.linkLblWebPage, 0);
            this.Controls.SetChildIndex(this.lbDetails, 0);
            this.Controls.SetChildIndex(this.btnok, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.certifedbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void displayApplicationImage()
		{
			Image image;
			try
			{
				//image = Image.FromFile(MIDGraphics.ImageDir + "\\" + MIDGraphics.ApplicationImage);
                image = Image.FromFile(MIDGraphics.ImageDir + "\\" + MIDGraphics.Logility);
				SizeF sizef = new SizeF(pictureBox1.Width, pictureBox1.Height);
				Size size = Size.Ceiling(sizef);
				Bitmap bitmap = new Bitmap(image, size);
				pictureBox1.Image = bitmap;
			}

			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
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

        // Begin TT#698 - JSmith - Enhance environment information
//        private string DetermineOperatingSystem()
//        {
//            try
//            {
//                string operatingSystem = string.Empty;
//                System.OperatingSystem osInfo = System.Environment.OSVersion;

//                string name = OSInfo.Name;

//                switch(osInfo.Platform)
//                {
//                    case System.PlatformID.Win32Windows:
//                    {
//                        // Code to determine specific version of Windows 95,
//                        // Windows 98, Windows 98 Second Edition, or Windows Me.
//                        switch (osInfo.Version.Minor)
//                        {
//                            case 0:
//                                operatingSystem = "Windows 95";
//                                break;
//                            case 10:
//                                if(osInfo.Version.Revision.ToString()=="2222A")
//                                {
//                                    operatingSystem = "Windows 98 Second Edition";
//                                }
//                                else
//                                {
//                                    operatingSystem = "Windows 98";
//                                }
//                                break;
//                            case  90:
//                                operatingSystem = "Windows Me";
//                                break;
//                            default:
//                                operatingSystem = "Unknown Windows Version";
//                                break;
//                        }
//                        break;
//                    }

//                    case System.PlatformID.Win32NT:
//                    {
//                        // Code to determine specific version of Windows NT 3.51,
//                        // Windows NT 4.0, Windows 2000, or Windows XP.
//                        switch(osInfo.Version.Major)
//                        {
//                            case 3:
//                                operatingSystem = "Windows NT 3.51";
//                                break;
//                            case 4:
//                                operatingSystem = "Windows NT 4.0";
//                                break;
//                            case 5:
//                                if (osInfo.Version.Minor==0)
//                                {
//                                    operatingSystem = "Windows 2000";
//                                }
//                                else if (osInfo.Version.Minor==1)
//                                {
//                                    operatingSystem = "Windows XP";
//                                }
//                                else if (osInfo.Version.Minor==2)
//                                {
//                                    operatingSystem = "Windows Server 2003";
//                                }
//                                else
//                                {
//                                    operatingSystem = "Unknown Windows Version";
//                                }
//                                break;
//                            case 6:
//                                operatingSystem = "Longhorn";         
//                                break;
//                            default:
//                                operatingSystem = "Unknown Windows Version";
//                                break;
//                        }
//                        break;
//                    }
//                }

//                operatingSystem += " " + GetServicePack() + "  Build:" + osInfo.Version.ToString();

//                return operatingSystem;
//            }
//            catch (Exception)
//            {
//                return "unavailable";
////				MessageBox.Show(ex.Message);
////				throw;
//            }
//        }

//        private string GetServicePack()
//        {
//            OSVERSIONINFOEX os = new OSVERSIONINFOEX();
//            os.dwOSVersionInfoSize=Marshal.SizeOf(typeof(OSVERSIONINFOEX));
//            GetVersionEx(ref os);
//            return os.szCSDVersion;
//        }
        // End TT#698

		private void AddConfigurationInformation()
		{
			try
			{
                tabPage1.Text = EnvironmentBusinessInfo.ConfigurationLabelText;
				listBoxConfiguration.Items.Clear();
				
                //string version = string.Empty;
                //GlobalOptions go = new GlobalOptions();
                //DataTable dt = go.GetApplicationInfo();
                //if (dt.Rows.Count > 0)
                //{
                //    DataRow dr = dt.Rows[dt.Rows.Count - 1];
                //    version = Convert.ToString(dr["MAJOR_VERSION"], CultureInfo.CurrentCulture)
                //        + "." + Convert.ToString(dr["MINOR_VERSION"], CultureInfo.CurrentCulture)
                //        + "." + Convert.ToString(dr["REVISION"], CultureInfo.CurrentCulture)
                //        + "." + Convert.ToString(dr["MODIFICATION"], CultureInfo.CurrentCulture);
                //}

                listBoxConfiguration.Items.Add("Database=" + EnvironmentBusinessInfo.GetClientDatbaseName(_SAB) + "; Application Version=" + EnvironmentInfo.MIDInfo.applicationVersion );

                listBoxConfiguration.Items.Add(EnvironmentBusinessInfo.GetControlServerSessionInfo(_SAB));
                listBoxConfiguration.Items.Add(EnvironmentBusinessInfo.GetApplicationServerSessionInfo(_SAB));
                listBoxConfiguration.Items.Add(EnvironmentBusinessInfo.GetHierarchyServerSessionInfo(_SAB));
                listBoxConfiguration.Items.Add(EnvironmentBusinessInfo.GetSchedulerServerSessionInfo(_SAB));
                listBoxConfiguration.Items.Add(EnvironmentBusinessInfo.GetStoreServerSessionInfo(_SAB));
            

                //AddServerInformation(_SAB.ControlServerSession, "Control", _SAB.ControlServer);
                //AddServerInformation(_SAB.ApplicationServerSession, "Application", _SAB.ServerGroup.ApplicationServer);
                //AddServerInformation(_SAB.HierarchyServerSession, "Hierarchy", _SAB.ServerGroup.HierarchyServer);
                //AddServerInformation(_SAB.SchedulerServerSession, "Scheduler", _SAB.ServerGroup.SchedulerServer);
                //AddServerInformation(_SAB.StoreServerSession, "Store", _SAB.ServerGroup.StoreServer);
		
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
		}

        //private void AddServerInformation(Session aSession, string aSessionName, string aControlServerServerGroup)
        //{
        //    try
        //    {
        //        if (aSession == null)
        //        {
        //            listBox2.Items.Add(aSessionName + " Server is not defined");
        //        }
        //        else
        //            //Begin TT#708 - JScott - Services need a Retry availalbe.
        //            //if (!RemotingServices.IsTransparentProxy(aSession))
        //            if (aSession.isSessionRunningLocal)
        //            //End TT#708 - JScott - Services need a Retry availalbe.
        //        {
        //            listBox2.Items.Add(aSessionName + " Server is running local");
        //        }
        //        else
        //        {
        //            int lastIndex = aControlServerServerGroup.LastIndexOf(":");
        //            string	portNumber = aControlServerServerGroup.Substring(lastIndex, aControlServerServerGroup.Length - lastIndex);
        //            listBox2.Items.Add(aSessionName + " Server(" + aSession.GetProductVersion() + ") is running remote on " + aSession.GetMachineName() + "(" + aSession.GetIPAddress() + ")" + portNumber);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        throw;
        //    }
        //}


		private void AddAssemblyInformation()
		{
			try
			{
                tabPage2.Text = EnvironmentInfo.MIDInfo.assemblyLabelText;
                listBoxAssembly.Items.Clear();
                foreach(EnvironmentInfo.MIDInfoSupport.assemblyInfo a in EnvironmentInfo.MIDInfo.assemblyInfoList)
                {
                    listBoxAssembly.Items.Add(a.fileName + "     " + a.fileProductVersion);
                }

              
                //string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //int lastIndex = path.LastIndexOf("\\");
                //path = path.Substring(0, lastIndex + 1);
                //DirectoryInfo directoryInfo = new DirectoryInfo(path);
				
                //FileInfo[] dlls = directoryInfo.GetFiles("*.dll");
                //foreach( FileInfo f in dlls)
                //{
                //    //System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(f.Name);
                //    System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(path + @"\" +f.Name);
                //    listBox1.Items.Add(f.Name + "     Version:" + fvi.ProductVersion);
                //}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
		}

		private void AddAddOnInformation()
		{
			try
			{
                tabPage3.Text = EnvironmentBusinessInfo.AddOnLabelText;
				listBoxAddOn.Items.Clear();

                listBoxAddOn.Items.Add(EnvironmentBusinessInfo.GetAllocationInstalledInfo(_SAB));
                listBoxAddOn.Items.Add(EnvironmentBusinessInfo.GetSizeInstalledInfo(_SAB));
                listBoxAddOn.Items.Add(EnvironmentBusinessInfo.GetPlanningInstalledInfo(_SAB));
                listBoxAddOn.Items.Add(EnvironmentBusinessInfo.GetAssortmentInstalledInfo(_SAB)); // TT#862 - MD - stodd - Assortment Upgrade Issues
                listBoxAddOn.Items.Add(EnvironmentBusinessInfo.GetGroupAllocationInstalledInfo(_SAB));   // TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                listBoxAddOn.Items.Add(EnvironmentBusinessInfo.GetAnalyticsInstalledInfo(_SAB));  // TT#2131-MD - JSmith - Halo Integration
                //AddAddOn(eMIDTextCode.lbl_Allocation, _SAB.ClientServerSession.GlobalOptions.AppConfig.AllocationInstalled, 
                //    _SAB.ClientServerSession.GlobalOptions.AppConfig.AllocationTempLicense, _SAB.ClientServerSession.GlobalOptions.AppConfig.AllocationExpirationDays);
                //AddAddOn(eMIDTextCode.lbl_Size, _SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled, 
                //    _SAB.ClientServerSession.GlobalOptions.AppConfig.SizeTempLicense, _SAB.ClientServerSession.GlobalOptions.AppConfig.SizeExpirationDays);
                //AddAddOn(eMIDTextCode.lbl_Planning, _SAB.ClientServerSession.GlobalOptions.AppConfig.PlanningInstalled, 
                //    _SAB.ClientServerSession.GlobalOptions.AppConfig.PlanningTempLicense, _SAB.ClientServerSession.GlobalOptions.AppConfig.PlanningExpirationDays);
//				AddAddOn(eMIDTextCode.lbl_Assortment, _SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled, 
//					_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentTempLicense, _SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentExpirationDays);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
		}

        //private void AddAddOn(eMIDTextCode aApplication, bool aApplicationInstalled, bool aTempLicense, int aExpireDays)
        //{
        //    try
        //    {
        //        StringBuilder text;
        //        string installed = MIDText.GetTextOnly(eMIDTextCode.msg_IsInstalled);
        //        string notInstalled = MIDText.GetTextOnly(eMIDTextCode.msg_IsNotInstalled);
        //        string tempInstalled = MIDText.GetTextOnly(eMIDTextCode.msg_IsTempInstalled);
        //        if (aTempLicense)
        //        {
        //            text = new StringBuilder(tempInstalled);
        //            text.Replace("{1}", aExpireDays.ToString());
        //        }
        //        else if (aApplicationInstalled)
        //        {
        //            text = new StringBuilder(installed);
        //        }
        //        else
        //        {
        //            text = new StringBuilder(notInstalled);
        //        }
        //        text.Replace("{0}", MIDText.GetTextOnly(aApplication));
        //        listBox3.Items.Add(text.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        throw;
        //    }
        //}

		private void btnDetails_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (_detailsShown)
				{
					HideDetails();
				}
				else
				{
					ShowDetails();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
		}

		private void ShowDetails()
		{
			try
			{
				tabControl1.Visible = true;
				Height = cShowHeight;
				btnDetails.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hide);
				_detailsShown = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void HideDetails()
		{
			try
			{
				this.tabControl1.Visible = false;
				this.Height = cHideHeight;
				btnDetails.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Details);
				_detailsShown = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#292-MD - JSmith - Enhanced Help - About
        private void linkLblSupportWebPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLblSupportWebPage.LinkVisited = true;

            System.Diagnostics.Process.Start(EnvironmentInfo.MIDInfo.supportWebPageURL);
        }

        private void linkLblWebPage_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLblWebPage.LinkVisited = true;

            System.Diagnostics.Process.Start(EnvironmentInfo.MIDInfo.webPageURL);
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lblSupportWebPage_Click(object sender, EventArgs e)
        {

        }

        private void lblStoreDelete_Click(object sender, EventArgs e)
        {
                    }
        // End TT#292-MD - JSmith - Enhanced Help - About
	}


}
