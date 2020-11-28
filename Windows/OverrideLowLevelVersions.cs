using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

using C1.Win.C1FlexGrid;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for OverrideLowLevelVersions.
	/// </summary>
	public class frmOverrideLowLevelVersions : MIDFormBase
	{
		#region Fields

		private SessionAddressBlock _SAB;
//		private OTSPlanSelectionData _selection;
//		private PlanOpenParms _openParms;
		private ProfileList _overrideList;
//		private HierarchyNodeList _hnl;
//		private ePlanType _planType;
		private ProfileList _versionProfList;
		private VersionProfile _LowLevelVersionDefault;
		// Begin Issue 4858
		private bool _changeMade = false;
		// End Issue 4858
		private bool _allowUpdateChain = true;
		private bool _allowUpdateStore = true;

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private C1.Win.C1FlexGrid.C1FlexGrid grdLowLevelVersions;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Begin Issue 4858
		/// <summary>
		/// Gets the flag identifying if a change was made.
		/// </summary>
		public bool ChangeMade
		{
			get { return _changeMade ; }
		}
		// End Issue 4858

		public frmOverrideLowLevelVersions(SessionAddressBlock aSAB, ProfileList aOverrideList, ProfileList aVersionProfList, VersionProfile aLowLevelVersionDefault, 
			bool aAllowUpdateChain, bool aAllowUpdateStore) : base(aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_SAB = aSAB;
			_overrideList = aOverrideList;
			_versionProfList = aVersionProfList;
			_LowLevelVersionDefault = aLowLevelVersionDefault;
			_allowUpdateChain = aAllowUpdateChain;
			_allowUpdateStore = aAllowUpdateStore;
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

				this.grdLowLevelVersions.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.grdLowLevelVersions_AfterEdit);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.Load -= new System.EventHandler(this.frmOverrideLowLevelVersions_Load);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOverrideLowLevelVersions));
            this.grdLowLevelVersions = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLowLevelVersions)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // grdLowLevelVersions
            // 
            this.grdLowLevelVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdLowLevelVersions.ColumnInfo = "10,1,0,0,0,85,Columns:";
            this.grdLowLevelVersions.Location = new System.Drawing.Point(16, 28);
            this.grdLowLevelVersions.Name = "grdLowLevelVersions";
            this.grdLowLevelVersions.Rows.DefaultSize = 17;
            this.grdLowLevelVersions.ShowErrors = true;
            this.grdLowLevelVersions.Size = new System.Drawing.Size(320, 236);
            this.grdLowLevelVersions.StyleInfo = resources.GetString("grdLowLevelVersions.StyleInfo");
            this.grdLowLevelVersions.TabIndex = 0;
            this.grdLowLevelVersions.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdLowLevelVersions_AfterEdit);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(200, 304);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(280, 304);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmOverrideLowLevelVersions
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(360, 334);
            this.Controls.Add(this.grdLowLevelVersions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "frmOverrideLowLevelVersions";
            this.Text = "OverrideLowLevelVersions";
            this.Load += new System.EventHandler(this.frmOverrideLowLevelVersions_Load);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.grdLowLevelVersions, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLowLevelVersions)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		private void frmOverrideLowLevelVersions_Load(object sender, System.EventArgs e)
		{
			try
			{
				SetText();
				
				grdLowLevelVersions.Cols.Fixed = 1;
				grdLowLevelVersions.Rows.Fixed = 1;
				grdLowLevelVersions.Cols.Count = 3;
				grdLowLevelVersions.Rows.Count = _overrideList.Count + 1;
				grdLowLevelVersions.SetData(0, 0, MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise));
				grdLowLevelVersions.SetData(0, 1, MIDText.GetTextOnly(eMIDTextCode.lbl_Version));
				grdLowLevelVersions.SetData(0, 2, MIDText.GetTextOnly(eMIDTextCode.lbl_Exclude));

				//==================================================
				// create styles with data types, formats, etc
				//==================================================
				CellStyle cs = grdLowLevelVersions.Styles.Add("label");
				cs.DataType = typeof(string);
				cs.Font = new Font(Font, FontStyle.Bold);
				cs.TextAlign = TextAlignEnum.CenterCenter;

				string versions = string.Empty;
				foreach (VersionProfile versionProfile in _versionProfList)
				{
					versions += "|" + versionProfile.Description;
				}

				cs = grdLowLevelVersions.Styles.Add("versions");
				cs.DataType = typeof(string);
				cs.ComboList = versions;
				//				cs.ForeColor = Color.Navy;
				//				cs.Font = new Font(Font, FontStyle.Bold);

				cs = grdLowLevelVersions.Styles.Add("bool");
				cs.DataType = typeof(bool);
				cs.ImageAlign = ImageAlignEnum.CenterCenter;

				// Begin Issue 4858
				grdLowLevelVersions.Styles.Add("cell error");
				cs = grdLowLevelVersions.Styles["cell error"];
				cs.BackColor = Color.Red;
				cs.ForeColor = Color.White;

				cs = grdLowLevelVersions.Styles.Add("versions error");
				cs.DataType = typeof(string);
				cs.ComboList = versions;
				cs.BackColor = Color.Red;
				cs.ForeColor = Color.White;

				grdLowLevelVersions.Styles.Add("node");
				// END Issue 4858

				//===================================
				// assign styles to editable cells
				//===================================
				CellRange rg;
				rg = grdLowLevelVersions.GetCellRange(0,0,0,2);
				rg.Style = grdLowLevelVersions.Styles["label"];

				if (grdLowLevelVersions.Rows.Count > 1)
				{
					rg = grdLowLevelVersions.GetCellRange(1,1,grdLowLevelVersions.Rows.Count-1,1);
					rg.Style = grdLowLevelVersions.Styles["versions"];

					rg = grdLowLevelVersions.GetCellRange(1, 2, grdLowLevelVersions.Rows.Count-1, 2);
					rg.Style = grdLowLevelVersions.Styles["bool"];
				}

				string versionName = string.Empty;
				bool exclude = false;
				int row = 1;
				foreach (LowLevelVersionOverrideProfile lvop in _overrideList)
				{
					if (!lvop.VersionIsOverridden)
					{
						versionName = _LowLevelVersionDefault.Description;
						exclude = lvop.Exclude; 
					}
					else
					{
						versionName = lvop.VersionProfile.Description;
						exclude = lvop.Exclude;
					}
					grdLowLevelVersions.Rows[row].UserData = lvop.Key;
					grdLowLevelVersions.SetData(row, 0, lvop.NodeProfile.Text);
					grdLowLevelVersions.SetData(row, 1, versionName);
					grdLowLevelVersions.SetData(row, 2, exclude);
					++row;
				}

				grdLowLevelVersions.AutoSizeCols();

				grdLowLevelVersions.Cols[1].WidthDisplay = 125;

				int gridWidth =  grdLowLevelVersions.Cols[0].WidthDisplay 
					+ grdLowLevelVersions.Cols[1].WidthDisplay
					+ grdLowLevelVersions.Cols[2].WidthDisplay;
				this.Width = Convert.ToInt32(Math.Round(gridWidth * 1.2, 0));

				SetReadOnly(true);

				ValidateSpecificFields();  // Issue 4858
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SetText()
		{
			btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
			btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);

			Format_Title(eDataState.Updatable, eMIDTextCode.frm_OverrideLowLevelVersions, null);
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (!ValidateSpecificFields())
					SaveChanges();
				if (!ErrorFound)
				{
					ChangePending = false;
					this.Close();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cancel_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		override protected bool SaveChanges()
		{
			try
			{
				bool versionIsOverridden = false;
				for (int i=1; i<grdLowLevelVersions.Rows.Count; i++)
				{
					versionIsOverridden = false;
					int nodeRID = Convert.ToInt32(grdLowLevelVersions.Rows[i].UserData, CultureInfo.CurrentCulture);
					string versionDescription = Convert.ToString(grdLowLevelVersions[i, 1], CultureInfo.CurrentCulture);
					int versionRID = GetVersionRID(versionDescription);
					bool exclude = Convert.ToBoolean(grdLowLevelVersions[i, 2]);

					LowLevelVersionOverrideProfile lvop = (LowLevelVersionOverrideProfile)_overrideList.FindKey(nodeRID);
					if (versionRID != _LowLevelVersionDefault.Key)
					{
						versionIsOverridden = true;
					}
					else
					{
						versionIsOverridden = false;
					}

					lvop.VersionIsOverridden = versionIsOverridden;
					lvop.VersionProfile = (VersionProfile)_versionProfList.FindKey(versionRID);
					lvop.Exclude = exclude;
				}

				// Begin Issue 4858
				if (ChangePending)
				{
					_changeMade = true;
				}
				// End Issue 4858

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		protected bool ValidateSpecificFields()
		{
			try
			{
				ErrorFound = false;

				for (int i=1; i<grdLowLevelVersions.Rows.Count; i++)
				{
					// initialize to no error
					grdLowLevelVersions.SetCellStyle(i,0,grdLowLevelVersions.Styles["node"]);
					grdLowLevelVersions.SetCellStyle(i,1,grdLowLevelVersions.Styles["versions"]);

					int nodeRID = Convert.ToInt32(grdLowLevelVersions.Rows[i].UserData, CultureInfo.CurrentCulture);
					string versionDescription = Convert.ToString(grdLowLevelVersions[i, 1], CultureInfo.CurrentCulture);
					int versionRID = GetVersionRID(versionDescription);
					bool exclude = Convert.ToBoolean(grdLowLevelVersions[i, 2]);

					if (!exclude)
					{
//						grdLowLevelVersions.SetCellStyle(i,0,grdLowLevelVersions.Styles["versions"]);
//						grdLowLevelVersions.SetCellStyle(i,1,grdLowLevelVersions.Styles["node"]);

						LowLevelVersionOverrideProfile lvop = (LowLevelVersionOverrideProfile)_overrideList.FindKey(nodeRID);
						if (lvop.NodeProfile != null)
						{
							if (lvop.NodeProfile.StoreSecurityProfile != null)
							{
								if (_allowUpdateChain &&
									!lvop.NodeProfile.StoreSecurityProfile.AllowUpdate)
								{
									//grdLowLevelVersions.SetCellImage(i,0,ErrorImage);
									grdLowLevelVersions.SetCellStyle(i,0,grdLowLevelVersions.Styles["cell error"]);
//									ErrorProvider.SetError(grdLowLevelVersions, MIDText.GetText(eMIDTextCode.msg_pl_NotAuthorizedToPlan));
									ErrorFound = true;
								}
							}
						}
						if (lvop.VersionProfile != null)
						{
							if (lvop.VersionProfile.StoreSecurity != null)
							{
								if (_allowUpdateStore &&
									!lvop.VersionProfile.StoreSecurity.AllowUpdate)
								{
									//grdLowLevelVersions.SetCellImage(i,1,ErrorImage);
									grdLowLevelVersions.SetCellStyle(i,1,grdLowLevelVersions.Styles["versions error"]);
//									ErrorProvider.SetError(grdLowLevelVersions, MIDText.GetText(eMIDTextCode.msg_pl_NotAuthorizedToPlan));
									ErrorFound = true;
								}
							}
						}
					}
				}

				if (ErrorFound)
				{
					ErrorProvider.SetError(grdLowLevelVersions, MIDText.GetText(eMIDTextCode.msg_pl_NotAuthorizedToPlan));
				}
				else
				{
					ErrorProvider.SetError(grdLowLevelVersions, null);
				}

				return ErrorFound;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private int GetVersionRID(string aVersionDescription)
		{
			try
			{
				foreach (VersionProfile versionProfile in _versionProfList)
				{
					if (versionProfile.Description == aVersionDescription)
					{
						return versionProfile.Key;
					}
				}
				return Include.NoRID;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void grdLowLevelVersions_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;

					int nodeRID = Convert.ToInt32(grdLowLevelVersions.Rows[e.Row].UserData, CultureInfo.CurrentCulture);
					string versionDescription = Convert.ToString(grdLowLevelVersions[e.Row, 1], CultureInfo.CurrentCulture);
					int versionRID = GetVersionRID(versionDescription);

					LowLevelVersionOverrideProfile lvop = (LowLevelVersionOverrideProfile)_overrideList.FindKey(nodeRID);
					lvop.VersionProfile = (VersionProfile)_versionProfList.FindKey(versionRID);

					ValidateSpecificFields();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
