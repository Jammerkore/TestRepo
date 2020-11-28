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
	public class frmExcludeLowLevels : MIDFormBase
	{
		#region Fields

		private SessionAddressBlock _SAB;
		private ProfileList _excludeList;

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private C1.Win.C1FlexGrid.C1FlexGrid grdLowLevels;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmExcludeLowLevels(SessionAddressBlock aSAB, ProfileList aExcludeList) : base(aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_SAB = aSAB;
			_excludeList = aExcludeList;
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

				this.grdLowLevels.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.grdLowLevels_AfterEdit);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.Load -= new System.EventHandler(this.frmExcludeLowLevels_Load);
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
            this.grdLowLevels = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLowLevels)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // grdLowLevels
            // 
            this.grdLowLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdLowLevels.ColumnInfo = "10,1,0,0,0,85,Columns:";
            this.grdLowLevels.Location = new System.Drawing.Point(16, 30);
            this.grdLowLevels.Name = "grdLowLevels";
            this.grdLowLevels.Rows.DefaultSize = 17;
            this.grdLowLevels.Size = new System.Drawing.Size(320, 234);
            this.grdLowLevels.StyleInfo = resources.GetString("grdLowLevels.StyleInfo");
            this.grdLowLevels.TabIndex = 0;
            this.grdLowLevels.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdLowLevels_AfterEdit);
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
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grdLowLevels);
            this.Name = "frmOverrideLowLevelVersions";
            this.Text = "OverrideLowLevelVersions";
            this.Load += new System.EventHandler(this.frmExcludeLowLevels_Load);
            this.Controls.SetChildIndex(this.grdLowLevels, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLowLevels)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		private void frmExcludeLowLevels_Load(object sender, System.EventArgs e)
		{
			try
			{
				SetText();
				
				grdLowLevels.Cols.Fixed = 1;
				grdLowLevels.Rows.Fixed = 1;
				grdLowLevels.Cols.Count = 2;
				grdLowLevels.Rows.Count = _excludeList.Count + 1;
				grdLowLevels.SetData(0, 0, MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise));
				grdLowLevels.SetData(0, 1, MIDText.GetTextOnly(eMIDTextCode.lbl_Exclude));

				// create styles with data types, formats, etc
				CellStyle cs = grdLowLevels.Styles.Add("label");
				cs.DataType = typeof(string);
				cs.Font = new Font(Font, FontStyle.Bold);
				cs.TextAlign = TextAlignEnum.CenterCenter;

				cs = grdLowLevels.Styles.Add("bool");
				cs.DataType = typeof(bool);
				cs.ImageAlign = ImageAlignEnum.CenterCenter;

				// assign styles to editable cells
				CellRange rg;
				rg = grdLowLevels.GetCellRange(0,0,0,1);
				rg.Style = grdLowLevels.Styles["label"];

				if (grdLowLevels.Rows.Count > 1)
				{
					rg = grdLowLevels.GetCellRange(1, 1, grdLowLevels.Rows.Count-1, 1);
					rg.Style = grdLowLevels.Styles["bool"];
				}

				int row = 1;
				foreach (LowLevelExcludeProfile llep in _excludeList)
				{
					grdLowLevels.Rows[row].UserData = llep.Key;
					grdLowLevels.SetData(row, 0, llep.NodeProfile.Text);
					grdLowLevels.SetData(row, 1, llep.Exclude);
					++row;
				}

				grdLowLevels.AutoSizeCols();

				int gridWidth =  grdLowLevels.Cols[0].WidthDisplay 
					+ grdLowLevels.Cols[1].WidthDisplay;
				this.Width = Convert.ToInt32(Math.Round(gridWidth * 1.3, 0));

				SetReadOnly(true);
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

			Format_Title(eDataState.Updatable, eMIDTextCode.frm_ExcludeLowLevels, null);
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
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
				for (int i=1; i<grdLowLevels.Rows.Count; i++)
				{
					int nodeRID = Convert.ToInt32(grdLowLevels.Rows[i].UserData, CultureInfo.CurrentCulture);
					bool exclude = Convert.ToBoolean(grdLowLevels[i, 1]);

					LowLevelExcludeProfile llep = (LowLevelExcludeProfile)_excludeList.FindKey(nodeRID);
					llep.Exclude = exclude;
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void grdLowLevels_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
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
