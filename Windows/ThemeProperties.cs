using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;
using C1.Win.C1FlexGrid;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;
using System.Configuration;

namespace MIDRetail.Windows
{
	public class ThemeProperties : System.Windows.Forms.Form
	{
		#region Windows-generated declarations

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton optPlain;
		private System.Windows.Forms.RadioButton optAlter;
		private System.Windows.Forms.RadioButton optHighlightName;
		private System.Windows.Forms.RadioButton optChiseled;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.TabPage tabFonts;
		private System.Windows.Forms.TabPage tabBGColors;
		private System.Windows.Forms.TabPage tabBorders;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage tabForeColors;
		private System.Windows.Forms.PictureBox picboxBaseStyle;
		private System.Windows.Forms.TabPage tabSavedStyles;
		private System.Windows.Forms.ColumnHeader colSize;
		private System.Windows.Forms.ColumnHeader bolIsBold;
		private System.Windows.Forms.ColumnHeader colIsItalic;
		private System.Windows.Forms.ListView lvFonts;
		private System.Windows.Forms.ColumnHeader colFontFamily;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colIsUnderline;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button btnForeColorG11G12;
		private System.Windows.Forms.Button btnForeColorG10;
		private System.Windows.Forms.Button btnForeColorG8G9;
		private System.Windows.Forms.Button btnForeColorG7;
		private System.Windows.Forms.Button btnForeColorG5G6;
		private System.Windows.Forms.Button btnForeColorG4;
		private System.Windows.Forms.Button btnForeColorNegative;
		private System.Windows.Forms.Button btnForeColorColHeader;
		private System.Windows.Forms.Button btnForeColorGroupHeader;
		private System.Windows.Forms.Button btnForeColorMerDesc;
		private System.Windows.Forms.Button btnForeColorG1;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox txtForeColorG1;
		private System.Windows.Forms.TextBox txtForeColorMerDesc;
		private System.Windows.Forms.TextBox txtForeColorGroupHeader;
		private System.Windows.Forms.TextBox txtForeColorColHeader;
		private System.Windows.Forms.TextBox txtForeColorNegative;
		private System.Windows.Forms.TextBox txtForeColorG10;
		private System.Windows.Forms.TextBox txtForeColorG8G9;
		private System.Windows.Forms.TextBox txtForeColorG7;
		private System.Windows.Forms.TextBox txtForeColorG5G6;
		private System.Windows.Forms.TextBox txtForeColorG4;
		private System.Windows.Forms.TextBox txtForeColorG11G12;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.TextBox txtBGColorColHeader;
		private System.Windows.Forms.TextBox txtBGColorGroupHeader;
		private System.Windows.Forms.TextBox txtBGColorMerDesc;
		private System.Windows.Forms.TextBox txtBGColorG1;
		private System.Windows.Forms.Button btnBGColorG1;
		private System.Windows.Forms.Button btnBGColorMerDesc;
		private System.Windows.Forms.Button btnBGColorGroupHeader;
		private System.Windows.Forms.Button btnBGColorColHeader;
		private System.Windows.Forms.TextBox txtBGColorG7Color2;
		private System.Windows.Forms.TextBox txtBGColorG7Color1;
		private System.Windows.Forms.TextBox txtBGColorG4Color2;
		private System.Windows.Forms.TextBox txtBGColorG4Color1;
		private System.Windows.Forms.Button btnBGColorG4Color1;
		private System.Windows.Forms.Button btnBGColorG4Color2;
		private System.Windows.Forms.Button btnBGColorG7Color1;
		private System.Windows.Forms.Button btnBGColorG7Color2;
		private System.Windows.Forms.TextBox txtBGColorG10Color2;
		private System.Windows.Forms.TextBox txtBGColorG10Color1;
		private System.Windows.Forms.Button btnBGColorG10Color1;
		private System.Windows.Forms.Button btnBGColorG10Color2;
		private System.Windows.Forms.TextBox txtBGColorG11G12Color2;
		private System.Windows.Forms.TextBox txtBGColorG11G12Color1;
		private System.Windows.Forms.Button btnBGColorG11G12Color1;
		private System.Windows.Forms.Button btnBGColorG11G12Color2;
		private System.Windows.Forms.TextBox txtBGColorG8G9Color2;
		private System.Windows.Forms.TextBox txtBGColorG8G9Color1;
		private System.Windows.Forms.TextBox txtBGColorG5G6Color2;
		private System.Windows.Forms.TextBox txtBGColorG5G6Color1;
		private System.Windows.Forms.Button btnBGColorG5G6Color1;
		private System.Windows.Forms.Button btnBGColorG5G6Color2;
		private System.Windows.Forms.Button btnBGColorG8G9Color1;
		private System.Windows.Forms.Button btnBGColorG8G9Color2;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboCellBorderStyle;
		private System.Windows.Forms.GroupBox grpCellBorder;
		private System.Windows.Forms.TextBox txtCellBorderColor;
		private System.Windows.Forms.Button btnCellBorderColor;
		private System.Windows.Forms.GroupBox grpDividerOptions;
		private System.Windows.Forms.Label label34;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboDividerWidth;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chkDisplayVerticalDivider;
		private System.Windows.Forms.CheckBox chkDisplayHorizontalDivider;
		private System.Windows.Forms.TextBox txtDividerColorHorData;
		private System.Windows.Forms.TextBox txtDividerColorHorRow;
		private System.Windows.Forms.Button btnDividerColorHorRow;
		private System.Windows.Forms.Button btnDividerColorHorData;
		private System.Windows.Forms.TextBox txtDividerColorVertical;
		private System.Windows.Forms.Button btnDividerColorVertical;
		private System.Windows.Forms.Button btnRenameStyle;
		private System.Windows.Forms.Button btnDeleteStyle;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label lblBGColorG11G12Color2;
		private System.Windows.Forms.Label lblBGColorG8G9Color2;
		private System.Windows.Forms.Label lblBGColorG5G6Color2;
		private System.Windows.Forms.Label lblBGColorG10Color2;
		private System.Windows.Forms.Label lblBGColorG7Color2;
		private System.Windows.Forms.Label lblBGColorG4Color2;
		private System.Windows.Forms.Label lblCellBorderColor;
		private System.Windows.Forms.Label lblDividerColorVertical;
		private System.Windows.Forms.Label lblDividerColorHorData;
		private System.Windows.Forms.Label lblDividerColorHorRow;
		private System.Windows.Forms.Label lblHorizontalDivider;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.GroupBox grpTextEffects;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboTextEffectMerDesc;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboTextEffectGroupHeader;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboTextEffectColHeader;

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

				this.optChiseled.CheckedChanged -= new System.EventHandler(this.optChiseled_CheckedChanged);
				this.optHighlightName.CheckedChanged -= new System.EventHandler(this.optHighlightName_CheckedChanged);
				this.optAlter.CheckedChanged -= new System.EventHandler(this.optAlter_CheckedChanged);
				this.optPlain.CheckedChanged -= new System.EventHandler(this.optPlain_CheckedChanged);
				this.txtDividerColorVertical.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtDividerColorVertical_KeyPress);
				this.txtDividerColorVertical.Validating -= new System.ComponentModel.CancelEventHandler(this.txtDividerColorVertical_Validating);
				this.btnDividerColorVertical.Click -= new System.EventHandler(this.btnDividerColorVertical_Click);
				this.txtDividerColorHorData.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtDividerColorHorData_KeyPress);
				this.txtDividerColorHorData.Validating -= new System.ComponentModel.CancelEventHandler(this.txtDividerColorHorData_Validating);
				this.txtDividerColorHorRow.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtDividerColorHorRow_KeyPress);
				this.txtDividerColorHorRow.Validating -= new System.ComponentModel.CancelEventHandler(this.txtDividerColorHorRow_Validating);
				this.btnDividerColorHorRow.Click -= new System.EventHandler(this.btnDividerColorHorRow_Click);
				this.btnDividerColorHorData.Click -= new System.EventHandler(this.btnDividerColorHorData_Click);
				this.cboDividerWidth.SelectionChangeCommitted -= new System.EventHandler(this.cboDividerWidth_SelectionChangeCommitted);
				this.chkDisplayHorizontalDivider.CheckedChanged -= new System.EventHandler(this.chkDisplayHorizontalDivider_CheckedChanged);
				this.chkDisplayVerticalDivider.CheckedChanged -= new System.EventHandler(this.chkDisplayVerticalDivider_CheckedChanged);
				this.txtCellBorderColor.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtCellBorderColor_KeyPress);
				this.txtCellBorderColor.Validating -= new System.ComponentModel.CancelEventHandler(this.txtCellBorderColor_Validating);
				this.btnCellBorderColor.Click -= new System.EventHandler(this.btnCellBorderColor_Click);
				this.cboCellBorderStyle.SelectionChangeCommitted -= new System.EventHandler(this.cboCellBorderStyle_SelectionChangeCommitted);
				this.txtBGColorG11G12Color2.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG11G12Color2_KeyPress);
				this.txtBGColorG11G12Color2.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG11G12Color2_Validating);
				this.txtBGColorG11G12Color1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG11G12Color1_KeyPress);
				this.txtBGColorG11G12Color1.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG11G12Color1_Validating);
				this.btnBGColorG11G12Color1.Click -= new System.EventHandler(this.btnBGColorG11G12Color1_Click);
				this.btnBGColorG11G12Color2.Click -= new System.EventHandler(this.btnBGColorG11G12Color2_Click);
				this.txtBGColorG8G9Color2.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG8G9Color2_KeyPress);
				this.txtBGColorG8G9Color2.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG8G9Color2_Validating);
				this.txtBGColorG8G9Color1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG8G9Color1_KeyPress);
				this.txtBGColorG8G9Color1.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG8G9Color1_Validating);
				this.txtBGColorG5G6Color2.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG5G6Color2_KeyPress);
				this.txtBGColorG5G6Color2.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG5G6Color2_Validating);
				this.txtBGColorG5G6Color1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG5G6Color1_KeyPress);
				this.txtBGColorG5G6Color1.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG5G6Color1_Validating);
				this.btnBGColorG5G6Color1.Click -= new System.EventHandler(this.btnBGColorG5G6Color1_Click);
				this.btnBGColorG5G6Color2.Click -= new System.EventHandler(this.btnBGColorG5G6Color2_Click);
				this.btnBGColorG8G9Color1.Click -= new System.EventHandler(this.btnBGColorG8G9Color1_Click);
				this.btnBGColorG8G9Color2.Click -= new System.EventHandler(this.btnBGColorG8G9Color2_Click);
				this.txtBGColorG10Color2.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG10Color2_KeyPress);
				this.txtBGColorG10Color2.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG10Color2_Validating);
				this.txtBGColorG10Color1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG10Color1_KeyPress);
				this.txtBGColorG10Color1.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG10Color1_Validating);
				this.btnBGColorG10Color1.Click -= new System.EventHandler(this.btnBGColorG10Color1_Click);
				this.btnBGColorG10Color2.Click -= new System.EventHandler(this.btnBGColorG10Color2_Click);
				this.txtBGColorG7Color2.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG7Color2_KeyPress);
				this.txtBGColorG7Color2.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG7Color2_Validating);
				this.txtBGColorG7Color1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG7Color1_KeyPress);
				this.txtBGColorG7Color1.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG7Color1_Validating);
				this.txtBGColorG4Color2.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG4Color2_KeyPress);
				this.txtBGColorG4Color2.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG4Color2_Validating);
				this.txtBGColorG4Color1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG4Color1_KeyPress);
				this.txtBGColorG4Color1.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG4Color1_Validating);
				this.btnBGColorG4Color1.Click -= new System.EventHandler(this.btnBGColorG4Color1_Click);
				this.btnBGColorG4Color2.Click -= new System.EventHandler(this.btnBGColorG4Color2_Click);
				this.btnBGColorG7Color1.Click -= new System.EventHandler(this.btnBGColorG7Color1_Click);
				this.btnBGColorG7Color2.Click -= new System.EventHandler(this.btnBGColorG7Color2_Click);
				this.txtBGColorColHeader.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorColHeader_KeyPress);
				this.txtBGColorColHeader.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorColHeader_Validating);
				this.txtBGColorGroupHeader.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorGroupHeader_KeyPress);
				this.txtBGColorGroupHeader.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorGroupHeader_Validating);
				this.txtBGColorMerDesc.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorMerDesc_KeyPress);
				this.txtBGColorMerDesc.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorMerDesc_Validating);
				this.txtBGColorG1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG1_KeyPress);
				this.txtBGColorG1.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBGColorG1_Validating);
				this.btnBGColorG1.Click -= new System.EventHandler(this.btnBGColorG1_Click);
				this.btnBGColorMerDesc.Click -= new System.EventHandler(this.btnBGColorMerDesc_Click);
				this.btnBGColorGroupHeader.Click -= new System.EventHandler(this.btnBGColorGroupHeader_Click);
				this.btnBGColorColHeader.Click -= new System.EventHandler(this.btnBGColorColHeader_Click);
				this.txtForeColorG11G12.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG11G12_KeyPress);
				this.txtForeColorG11G12.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorG11G12_Validating);
				this.txtForeColorG10.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG10_KeyPress);
				this.txtForeColorG10.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorG10_Validating);
				this.txtForeColorG8G9.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG8G9_KeyPress);
				this.txtForeColorG8G9.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorG8G9_Validating);
				this.txtForeColorG7.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG7_KeyPress);
				this.txtForeColorG7.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorG7_Validating);
				this.txtForeColorG5G6.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG5G6_KeyPress);
				this.txtForeColorG5G6.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorG5G6_Validating);
				this.txtForeColorG4.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG4_KeyPress);
				this.txtForeColorG4.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorG4_Validating);
				this.txtForeColorNegative.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorNegative_KeyPress);
				this.txtForeColorNegative.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorNegative_Validating);
				this.txtForeColorColHeader.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorColHeader_KeyPress);
				this.txtForeColorColHeader.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorColHeader_Validating);
				this.txtForeColorGroupHeader.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorGroupHeader_KeyPress);
				this.txtForeColorGroupHeader.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorGroupHeader_Validating);
				this.txtForeColorMerDesc.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorMerDesc_KeyPress);
				this.txtForeColorMerDesc.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorMerDesc_Validating);
				this.txtForeColorG1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG1_KeyPress);
				this.txtForeColorG1.Validating -= new System.ComponentModel.CancelEventHandler(this.txtForeColorG1_Validating);
				this.btnForeColorG1.Click -= new System.EventHandler(this.btnForeColorG1_Click);
				this.btnForeColorMerDesc.Click -= new System.EventHandler(this.btnForeColorMerDesc_Click);
				this.btnForeColorGroupHeader.Click -= new System.EventHandler(this.btnForeColorGroupHeader_Click);
				this.btnForeColorColHeader.Click -= new System.EventHandler(this.btnForeColorColHeader_Click);
				this.btnForeColorNegative.Click -= new System.EventHandler(this.btnForeColorNegative_Click);
				this.btnForeColorG4.Click -= new System.EventHandler(this.btnForeColorG4_Click);
				this.btnForeColorG5G6.Click -= new System.EventHandler(this.btnForeColorG5G6_Click);
				this.btnForeColorG7.Click -= new System.EventHandler(this.btnForeColorG7_Click);
				this.btnForeColorG8G9.Click -= new System.EventHandler(this.btnForeColorG8G9_Click);
				this.btnForeColorG10.Click -= new System.EventHandler(this.btnForeColorG10_Click);
				this.btnForeColorG11G12.Click -= new System.EventHandler(this.btnForeColorG11G12_Click);
				this.cboTextEffectColHeader.SelectionChangeCommitted -= new System.EventHandler(this.cboTextEffectColHeader_SelectionChangeCommitted);
				this.cboTextEffectGroupHeader.SelectionChangeCommitted -= new System.EventHandler(this.cboTextEffectGroupHeader_SelectionChangeCommitted);
				this.cboTextEffectMerDesc.SelectionChangeCommitted -= new System.EventHandler(this.cboTextEffectMerDesc_SelectionChangeCommitted);
				this.lvFonts.ItemActivate -= new System.EventHandler(this.lvFonts_ItemActivate);
				this.lvFonts.SelectedIndexChanged -= new System.EventHandler(this.lvFonts_SelectedIndexChanged);
				this.btnDeleteStyle.Click -= new System.EventHandler(this.btnDeleteStyle_Click);
				this.btnRenameStyle.Click -= new System.EventHandler(this.btnRenameStyle_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.lstThemes.SelectedIndexChanged -= new System.EventHandler(this.lstThemes_SelectedIndexChanged);

                this.cboDividerWidth.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboDividerWidth_MIDComboBoxPropertiesChangedEvent);
                this.cboCellBorderStyle.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboCellBorderStyle_MIDComboBoxPropertiesChangedEvent);
                this.cboTextEffectColHeader.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboTextEffectColHeader_MIDComboBoxPropertiesChangedEvent);
                this.cboTextEffectGroupHeader.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboTextEffectGroupHeader_MIDComboBoxPropertiesChangedEvent);
                this.cboTextEffectMerDesc.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboTextEffectMerDesc_MIDComboBoxPropertiesChangedEvent);

				this.Load -= new System.EventHandler(this.ThemeProperties_Load);
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Main Text");
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Editable Cells");
			System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Merchandise Description");
			System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Group Header");
			System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("Column Headers");
			System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Row Headers");
			System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Ineligible Stores");
			System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("Lock");
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.picboxBaseStyle = new System.Windows.Forms.PictureBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.optChiseled = new System.Windows.Forms.RadioButton();
			this.optHighlightName = new System.Windows.Forms.RadioButton();
			this.optAlter = new System.Windows.Forms.RadioButton();
			this.optPlain = new System.Windows.Forms.RadioButton();
			this.tabBorders = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblDividerColorHorData = new System.Windows.Forms.Label();
			this.lblDividerColorHorRow = new System.Windows.Forms.Label();
			this.txtDividerColorVertical = new System.Windows.Forms.TextBox();
			this.btnDividerColorVertical = new System.Windows.Forms.Button();
			this.txtDividerColorHorData = new System.Windows.Forms.TextBox();
			this.txtDividerColorHorRow = new System.Windows.Forms.TextBox();
			this.btnDividerColorHorRow = new System.Windows.Forms.Button();
			this.btnDividerColorHorData = new System.Windows.Forms.Button();
			this.lblDividerColorVertical = new System.Windows.Forms.Label();
			this.lblHorizontalDivider = new System.Windows.Forms.Label();
			this.grpDividerOptions = new System.Windows.Forms.GroupBox();
			this.cboDividerWidth = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.label34 = new System.Windows.Forms.Label();
			this.chkDisplayHorizontalDivider = new System.Windows.Forms.CheckBox();
			this.chkDisplayVerticalDivider = new System.Windows.Forms.CheckBox();
			this.grpCellBorder = new System.Windows.Forms.GroupBox();
			this.txtCellBorderColor = new System.Windows.Forms.TextBox();
			this.btnCellBorderColor = new System.Windows.Forms.Button();
			this.lblCellBorderColor = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cboCellBorderStyle = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.tabBGColors = new System.Windows.Forms.TabPage();
			this.label33 = new System.Windows.Forms.Label();
			this.txtBGColorG11G12Color2 = new System.Windows.Forms.TextBox();
			this.txtBGColorG11G12Color1 = new System.Windows.Forms.TextBox();
			this.btnBGColorG11G12Color1 = new System.Windows.Forms.Button();
			this.btnBGColorG11G12Color2 = new System.Windows.Forms.Button();
			this.label27 = new System.Windows.Forms.Label();
			this.lblBGColorG11G12Color2 = new System.Windows.Forms.Label();
			this.txtBGColorG8G9Color2 = new System.Windows.Forms.TextBox();
			this.txtBGColorG8G9Color1 = new System.Windows.Forms.TextBox();
			this.txtBGColorG5G6Color2 = new System.Windows.Forms.TextBox();
			this.txtBGColorG5G6Color1 = new System.Windows.Forms.TextBox();
			this.btnBGColorG5G6Color1 = new System.Windows.Forms.Button();
			this.btnBGColorG5G6Color2 = new System.Windows.Forms.Button();
			this.btnBGColorG8G9Color1 = new System.Windows.Forms.Button();
			this.btnBGColorG8G9Color2 = new System.Windows.Forms.Button();
			this.label29 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.lblBGColorG8G9Color2 = new System.Windows.Forms.Label();
			this.lblBGColorG5G6Color2 = new System.Windows.Forms.Label();
			this.txtBGColorG10Color2 = new System.Windows.Forms.TextBox();
			this.txtBGColorG10Color1 = new System.Windows.Forms.TextBox();
			this.btnBGColorG10Color1 = new System.Windows.Forms.Button();
			this.btnBGColorG10Color2 = new System.Windows.Forms.Button();
			this.label25 = new System.Windows.Forms.Label();
			this.lblBGColorG10Color2 = new System.Windows.Forms.Label();
			this.txtBGColorG7Color2 = new System.Windows.Forms.TextBox();
			this.txtBGColorG7Color1 = new System.Windows.Forms.TextBox();
			this.txtBGColorG4Color2 = new System.Windows.Forms.TextBox();
			this.txtBGColorG4Color1 = new System.Windows.Forms.TextBox();
			this.btnBGColorG4Color1 = new System.Windows.Forms.Button();
			this.btnBGColorG4Color2 = new System.Windows.Forms.Button();
			this.btnBGColorG7Color1 = new System.Windows.Forms.Button();
			this.btnBGColorG7Color2 = new System.Windows.Forms.Button();
			this.label20 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.lblBGColorG7Color2 = new System.Windows.Forms.Label();
			this.lblBGColorG4Color2 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.txtBGColorColHeader = new System.Windows.Forms.TextBox();
			this.txtBGColorGroupHeader = new System.Windows.Forms.TextBox();
			this.txtBGColorMerDesc = new System.Windows.Forms.TextBox();
			this.txtBGColorG1 = new System.Windows.Forms.TextBox();
			this.btnBGColorG1 = new System.Windows.Forms.Button();
			this.btnBGColorMerDesc = new System.Windows.Forms.Button();
			this.btnBGColorGroupHeader = new System.Windows.Forms.Button();
			this.btnBGColorColHeader = new System.Windows.Forms.Button();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.tabForeColors = new System.Windows.Forms.TabPage();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.txtForeColorG11G12 = new System.Windows.Forms.TextBox();
			this.txtForeColorG10 = new System.Windows.Forms.TextBox();
			this.txtForeColorG8G9 = new System.Windows.Forms.TextBox();
			this.txtForeColorG7 = new System.Windows.Forms.TextBox();
			this.txtForeColorG5G6 = new System.Windows.Forms.TextBox();
			this.txtForeColorG4 = new System.Windows.Forms.TextBox();
			this.txtForeColorNegative = new System.Windows.Forms.TextBox();
			this.txtForeColorColHeader = new System.Windows.Forms.TextBox();
			this.txtForeColorGroupHeader = new System.Windows.Forms.TextBox();
			this.txtForeColorMerDesc = new System.Windows.Forms.TextBox();
			this.txtForeColorG1 = new System.Windows.Forms.TextBox();
			this.btnForeColorG1 = new System.Windows.Forms.Button();
			this.btnForeColorMerDesc = new System.Windows.Forms.Button();
			this.btnForeColorGroupHeader = new System.Windows.Forms.Button();
			this.btnForeColorColHeader = new System.Windows.Forms.Button();
			this.btnForeColorNegative = new System.Windows.Forms.Button();
			this.btnForeColorG4 = new System.Windows.Forms.Button();
			this.btnForeColorG5G6 = new System.Windows.Forms.Button();
			this.btnForeColorG7 = new System.Windows.Forms.Button();
			this.btnForeColorG8G9 = new System.Windows.Forms.Button();
			this.btnForeColorG10 = new System.Windows.Forms.Button();
			this.btnForeColorG11G12 = new System.Windows.Forms.Button();
			this.label13 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.tabFonts = new System.Windows.Forms.TabPage();
			this.grpTextEffects = new System.Windows.Forms.GroupBox();
			this.label24 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cboTextEffectColHeader = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cboTextEffectGroupHeader = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cboTextEffectMerDesc = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.lvFonts = new System.Windows.Forms.ListView();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.colFontFamily = new System.Windows.Forms.ColumnHeader();
			this.colSize = new System.Windows.Forms.ColumnHeader();
			this.bolIsBold = new System.Windows.Forms.ColumnHeader();
			this.colIsItalic = new System.Windows.Forms.ColumnHeader();
			this.colIsUnderline = new System.Windows.Forms.ColumnHeader();
			this.tabSavedStyles = new System.Windows.Forms.TabPage();
			this.lstThemes = new System.Windows.Forms.ListBox();
			this.btnDeleteStyle = new System.Windows.Forms.Button();
			this.btnRenameStyle = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabGeneral.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabBorders.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.grpDividerOptions.SuspendLayout();
			this.grpCellBorder.SuspendLayout();
			this.tabBGColors.SuspendLayout();
			this.tabForeColors.SuspendLayout();
			this.tabFonts.SuspendLayout();
			this.grpTextEffects.SuspendLayout();
			this.tabSavedStyles.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabGeneral);
			this.tabControl1.Controls.Add(this.tabBorders);
			this.tabControl1.Controls.Add(this.tabBGColors);
			this.tabControl1.Controls.Add(this.tabForeColors);
			this.tabControl1.Controls.Add(this.tabFonts);
			this.tabControl1.Controls.Add(this.tabSavedStyles);
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(448, 280);
			this.tabControl1.TabIndex = 0;
			// 
			// tabGeneral
			// 
			this.tabGeneral.Controls.Add(this.picboxBaseStyle);
			this.tabGeneral.Controls.Add(this.groupBox1);
			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Size = new System.Drawing.Size(440, 254);
			this.tabGeneral.TabIndex = 0;
			this.tabGeneral.Text = "Base Style";
			// 
			// picboxBaseStyle
			// 
			this.picboxBaseStyle.Location = new System.Drawing.Point(122, 16);
			this.picboxBaseStyle.Name = "picboxBaseStyle";
			this.picboxBaseStyle.Size = new System.Drawing.Size(310, 232);
			this.picboxBaseStyle.TabIndex = 2;
			this.picboxBaseStyle.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.optChiseled);
			this.groupBox1.Controls.Add(this.optHighlightName);
			this.groupBox1.Controls.Add(this.optAlter);
			this.groupBox1.Controls.Add(this.optPlain);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(108, 120);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Base Style";
			// 
			// optChiseled
			// 
			this.optChiseled.Location = new System.Drawing.Point(6, 96);
			this.optChiseled.Name = "optChiseled";
			this.optChiseled.Size = new System.Drawing.Size(100, 16);
			this.optChiseled.TabIndex = 3;
			this.optChiseled.Text = "Chiseled";
			this.optChiseled.CheckedChanged += new System.EventHandler(this.optChiseled_CheckedChanged);
			// 
			// optHighlightName
			// 
			this.optHighlightName.Location = new System.Drawing.Point(6, 72);
			this.optHighlightName.Name = "optHighlightName";
			this.optHighlightName.Size = new System.Drawing.Size(100, 16);
			this.optHighlightName.TabIndex = 2;
			this.optHighlightName.Text = "Highlight Name";
			this.optHighlightName.CheckedChanged += new System.EventHandler(this.optHighlightName_CheckedChanged);
			// 
			// optAlter
			// 
			this.optAlter.Location = new System.Drawing.Point(6, 48);
			this.optAlter.Name = "optAlter";
			this.optAlter.Size = new System.Drawing.Size(100, 16);
			this.optAlter.TabIndex = 1;
			this.optAlter.Text = "Alter Colors";
			this.optAlter.CheckedChanged += new System.EventHandler(this.optAlter_CheckedChanged);
			// 
			// optPlain
			// 
			this.optPlain.Location = new System.Drawing.Point(6, 24);
			this.optPlain.Name = "optPlain";
			this.optPlain.Size = new System.Drawing.Size(100, 16);
			this.optPlain.TabIndex = 0;
			this.optPlain.Text = "Plain";
			this.optPlain.CheckedChanged += new System.EventHandler(this.optPlain_CheckedChanged);
			// 
			// tabBorders
			// 
			this.tabBorders.Controls.Add(this.groupBox2);
			this.tabBorders.Controls.Add(this.grpDividerOptions);
			this.tabBorders.Controls.Add(this.grpCellBorder);
			this.tabBorders.Location = new System.Drawing.Point(4, 22);
			this.tabBorders.Name = "tabBorders";
			this.tabBorders.Size = new System.Drawing.Size(440, 254);
			this.tabBorders.TabIndex = 3;
			this.tabBorders.Text = "Borders";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lblDividerColorHorData);
			this.groupBox2.Controls.Add(this.lblDividerColorHorRow);
			this.groupBox2.Controls.Add(this.txtDividerColorVertical);
			this.groupBox2.Controls.Add(this.btnDividerColorVertical);
			this.groupBox2.Controls.Add(this.txtDividerColorHorData);
			this.groupBox2.Controls.Add(this.txtDividerColorHorRow);
			this.groupBox2.Controls.Add(this.btnDividerColorHorRow);
			this.groupBox2.Controls.Add(this.btnDividerColorHorData);
			this.groupBox2.Controls.Add(this.lblDividerColorVertical);
			this.groupBox2.Controls.Add(this.lblHorizontalDivider);
			this.groupBox2.Location = new System.Drawing.Point(16, 136);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(408, 112);
			this.groupBox2.TabIndex = 12;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Divider Colors";
			// 
			// lblDividerColorHorData
			// 
			this.lblDividerColorHorData.Location = new System.Drawing.Point(112, 88);
			this.lblDividerColorHorData.Name = "lblDividerColorHorData";
			this.lblDividerColorHorData.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblDividerColorHorData.Size = new System.Drawing.Size(88, 16);
			this.lblDividerColorHorData.TabIndex = 71;
			this.lblDividerColorHorData.Text = "For data";
			// 
			// lblDividerColorHorRow
			// 
			this.lblDividerColorHorRow.Location = new System.Drawing.Point(112, 64);
			this.lblDividerColorHorRow.Name = "lblDividerColorHorRow";
			this.lblDividerColorHorRow.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblDividerColorHorRow.Size = new System.Drawing.Size(88, 16);
			this.lblDividerColorHorRow.TabIndex = 70;
			this.lblDividerColorHorRow.Text = "For row headers";
			// 
			// txtDividerColorVertical
			// 
			this.txtDividerColorVertical.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtDividerColorVertical.Location = new System.Drawing.Point(240, 16);
			this.txtDividerColorVertical.Name = "txtDividerColorVertical";
			this.txtDividerColorVertical.Size = new System.Drawing.Size(64, 18);
			this.txtDividerColorVertical.TabIndex = 69;
			this.txtDividerColorVertical.Text = "100, 100, 100";
			this.txtDividerColorVertical.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDividerColorVertical_KeyPress);
			this.txtDividerColorVertical.Validating += new System.ComponentModel.CancelEventHandler(this.txtDividerColorVertical_Validating);
			// 
			// btnDividerColorVertical
			// 
			this.btnDividerColorVertical.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDividerColorVertical.Location = new System.Drawing.Point(208, 16);
			this.btnDividerColorVertical.Name = "btnDividerColorVertical";
			this.btnDividerColorVertical.Size = new System.Drawing.Size(24, 23);
			this.btnDividerColorVertical.TabIndex = 68;
			this.btnDividerColorVertical.Click += new System.EventHandler(this.btnDividerColorVertical_Click);
			// 
			// txtDividerColorHorData
			// 
			this.txtDividerColorHorData.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtDividerColorHorData.Location = new System.Drawing.Point(240, 80);
			this.txtDividerColorHorData.Name = "txtDividerColorHorData";
			this.txtDividerColorHorData.Size = new System.Drawing.Size(64, 18);
			this.txtDividerColorHorData.TabIndex = 67;
			this.txtDividerColorHorData.Text = "100, 100, 100";
			this.txtDividerColorHorData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDividerColorHorData_KeyPress);
			this.txtDividerColorHorData.Validating += new System.ComponentModel.CancelEventHandler(this.txtDividerColorHorData_Validating);
			// 
			// txtDividerColorHorRow
			// 
			this.txtDividerColorHorRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtDividerColorHorRow.Location = new System.Drawing.Point(240, 56);
			this.txtDividerColorHorRow.Name = "txtDividerColorHorRow";
			this.txtDividerColorHorRow.Size = new System.Drawing.Size(64, 18);
			this.txtDividerColorHorRow.TabIndex = 66;
			this.txtDividerColorHorRow.Text = "100, 100, 100";
			this.txtDividerColorHorRow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDividerColorHorRow_KeyPress);
			this.txtDividerColorHorRow.Validating += new System.ComponentModel.CancelEventHandler(this.txtDividerColorHorRow_Validating);
			// 
			// btnDividerColorHorRow
			// 
			this.btnDividerColorHorRow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDividerColorHorRow.Location = new System.Drawing.Point(208, 56);
			this.btnDividerColorHorRow.Name = "btnDividerColorHorRow";
			this.btnDividerColorHorRow.Size = new System.Drawing.Size(24, 23);
			this.btnDividerColorHorRow.TabIndex = 65;
			this.btnDividerColorHorRow.Click += new System.EventHandler(this.btnDividerColorHorRow_Click);
			// 
			// btnDividerColorHorData
			// 
			this.btnDividerColorHorData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDividerColorHorData.Location = new System.Drawing.Point(208, 80);
			this.btnDividerColorHorData.Name = "btnDividerColorHorData";
			this.btnDividerColorHorData.Size = new System.Drawing.Size(24, 23);
			this.btnDividerColorHorData.TabIndex = 64;
			this.btnDividerColorHorData.Click += new System.EventHandler(this.btnDividerColorHorData_Click);
			// 
			// lblDividerColorVertical
			// 
			this.lblDividerColorVertical.Location = new System.Drawing.Point(8, 24);
			this.lblDividerColorVertical.Name = "lblDividerColorVertical";
			this.lblDividerColorVertical.Size = new System.Drawing.Size(80, 16);
			this.lblDividerColorVertical.TabIndex = 63;
			this.lblDividerColorVertical.Text = "Vertical Divider";
			// 
			// lblHorizontalDivider
			// 
			this.lblHorizontalDivider.Location = new System.Drawing.Point(8, 64);
			this.lblHorizontalDivider.Name = "lblHorizontalDivider";
			this.lblHorizontalDivider.Size = new System.Drawing.Size(96, 16);
			this.lblHorizontalDivider.TabIndex = 62;
			this.lblHorizontalDivider.Text = "Horizontal Divider";
			// 
			// grpDividerOptions
			// 
			this.grpDividerOptions.Controls.Add(this.cboDividerWidth);
			this.grpDividerOptions.Controls.Add(this.label34);
			this.grpDividerOptions.Controls.Add(this.chkDisplayHorizontalDivider);
			this.grpDividerOptions.Controls.Add(this.chkDisplayVerticalDivider);
			this.grpDividerOptions.Location = new System.Drawing.Point(16, 64);
			this.grpDividerOptions.Name = "grpDividerOptions";
			this.grpDividerOptions.Size = new System.Drawing.Size(408, 64);
			this.grpDividerOptions.TabIndex = 11;
			this.grpDividerOptions.TabStop = false;
			this.grpDividerOptions.Text = "Dividers Options";
			// 
			// cboDividerWidth
			// 
			this.cboDividerWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDividerWidth.Items.AddRange(new object[] {
																 "1",
																 "2",
																 "3",
																 "4"});
			this.cboDividerWidth.Location = new System.Drawing.Point(336, 16);
			this.cboDividerWidth.Name = "cboDividerWidth";
			this.cboDividerWidth.Size = new System.Drawing.Size(56, 21);
			this.cboDividerWidth.TabIndex = 3;
			this.cboDividerWidth.SelectionChangeCommitted += new System.EventHandler(this.cboDividerWidth_SelectionChangeCommitted);
            this.cboDividerWidth.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboDividerWidth_MIDComboBoxPropertiesChangedEvent);
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(256, 24);
			this.label34.Name = "label34";
			this.label34.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label34.Size = new System.Drawing.Size(72, 16);
			this.label34.TabIndex = 2;
			this.label34.Text = "Divider width";
			// 
			// chkDisplayHorizontalDivider
			// 
			this.chkDisplayHorizontalDivider.Location = new System.Drawing.Point(16, 40);
			this.chkDisplayHorizontalDivider.Name = "chkDisplayHorizontalDivider";
			this.chkDisplayHorizontalDivider.Size = new System.Drawing.Size(160, 16);
			this.chkDisplayHorizontalDivider.TabIndex = 1;
			this.chkDisplayHorizontalDivider.Text = "Display horizontal divider";
			this.chkDisplayHorizontalDivider.CheckedChanged += new System.EventHandler(this.chkDisplayHorizontalDivider_CheckedChanged);
			// 
			// chkDisplayVerticalDivider
			// 
			this.chkDisplayVerticalDivider.Location = new System.Drawing.Point(16, 24);
			this.chkDisplayVerticalDivider.Name = "chkDisplayVerticalDivider";
			this.chkDisplayVerticalDivider.Size = new System.Drawing.Size(160, 16);
			this.chkDisplayVerticalDivider.TabIndex = 0;
			this.chkDisplayVerticalDivider.Text = "Display vertical divider";
			this.chkDisplayVerticalDivider.CheckedChanged += new System.EventHandler(this.chkDisplayVerticalDivider_CheckedChanged);
			// 
			// grpCellBorder
			// 
			this.grpCellBorder.Controls.Add(this.txtCellBorderColor);
			this.grpCellBorder.Controls.Add(this.btnCellBorderColor);
			this.grpCellBorder.Controls.Add(this.lblCellBorderColor);
			this.grpCellBorder.Controls.Add(this.label1);
			this.grpCellBorder.Controls.Add(this.cboCellBorderStyle);
			this.grpCellBorder.Location = new System.Drawing.Point(16, 8);
			this.grpCellBorder.Name = "grpCellBorder";
			this.grpCellBorder.Size = new System.Drawing.Size(408, 48);
			this.grpCellBorder.TabIndex = 10;
			this.grpCellBorder.TabStop = false;
			this.grpCellBorder.Text = "Cell Borders";
			// 
			// txtCellBorderColor
			// 
			this.txtCellBorderColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtCellBorderColor.Location = new System.Drawing.Point(328, 16);
			this.txtCellBorderColor.Name = "txtCellBorderColor";
			this.txtCellBorderColor.Size = new System.Drawing.Size(64, 18);
			this.txtCellBorderColor.TabIndex = 48;
			this.txtCellBorderColor.Text = "100, 100, 100";
			this.txtCellBorderColor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCellBorderColor_KeyPress);
			this.txtCellBorderColor.Validating += new System.ComponentModel.CancelEventHandler(this.txtCellBorderColor_Validating);
			// 
			// btnCellBorderColor
			// 
			this.btnCellBorderColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCellBorderColor.Location = new System.Drawing.Point(296, 16);
			this.btnCellBorderColor.Name = "btnCellBorderColor";
			this.btnCellBorderColor.Size = new System.Drawing.Size(24, 23);
			this.btnCellBorderColor.TabIndex = 47;
			this.btnCellBorderColor.Click += new System.EventHandler(this.btnCellBorderColor_Click);
			// 
			// lblCellBorderColor
			// 
			this.lblCellBorderColor.Location = new System.Drawing.Point(216, 24);
			this.lblCellBorderColor.Name = "lblCellBorderColor";
			this.lblCellBorderColor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblCellBorderColor.Size = new System.Drawing.Size(72, 16);
			this.lblCellBorderColor.TabIndex = 9;
			this.lblCellBorderColor.Text = "Border Color";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 8;
			this.label1.Text = "Border Style";
			// 
			// cboCellBorderStyle
			// 
			this.cboCellBorderStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCellBorderStyle.Items.AddRange(new object[] {
																	"Dotted",
																	"Double",
																	"Fillet",
																	"Flat",
																	"Groove",
																	"Inset",
																	"None",
																	"Raised"});
			this.cboCellBorderStyle.Location = new System.Drawing.Point(88, 16);
			this.cboCellBorderStyle.Name = "cboCellBorderStyle";
			this.cboCellBorderStyle.Size = new System.Drawing.Size(96, 21);
			this.cboCellBorderStyle.TabIndex = 6;
			this.cboCellBorderStyle.SelectionChangeCommitted += new System.EventHandler(this.cboCellBorderStyle_SelectionChangeCommitted);
            this.cboCellBorderStyle.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboCellBorderStyle_MIDComboBoxPropertiesChangedEvent);
			// 
			// tabBGColors
			// 
			this.tabBGColors.Controls.Add(this.label33);
			this.tabBGColors.Controls.Add(this.txtBGColorG11G12Color2);
			this.tabBGColors.Controls.Add(this.txtBGColorG11G12Color1);
			this.tabBGColors.Controls.Add(this.btnBGColorG11G12Color1);
			this.tabBGColors.Controls.Add(this.btnBGColorG11G12Color2);
			this.tabBGColors.Controls.Add(this.label27);
			this.tabBGColors.Controls.Add(this.lblBGColorG11G12Color2);
			this.tabBGColors.Controls.Add(this.txtBGColorG8G9Color2);
			this.tabBGColors.Controls.Add(this.txtBGColorG8G9Color1);
			this.tabBGColors.Controls.Add(this.txtBGColorG5G6Color2);
			this.tabBGColors.Controls.Add(this.txtBGColorG5G6Color1);
			this.tabBGColors.Controls.Add(this.btnBGColorG5G6Color1);
			this.tabBGColors.Controls.Add(this.btnBGColorG5G6Color2);
			this.tabBGColors.Controls.Add(this.btnBGColorG8G9Color1);
			this.tabBGColors.Controls.Add(this.btnBGColorG8G9Color2);
			this.tabBGColors.Controls.Add(this.label29);
			this.tabBGColors.Controls.Add(this.label30);
			this.tabBGColors.Controls.Add(this.lblBGColorG8G9Color2);
			this.tabBGColors.Controls.Add(this.lblBGColorG5G6Color2);
			this.tabBGColors.Controls.Add(this.txtBGColorG10Color2);
			this.tabBGColors.Controls.Add(this.txtBGColorG10Color1);
			this.tabBGColors.Controls.Add(this.btnBGColorG10Color1);
			this.tabBGColors.Controls.Add(this.btnBGColorG10Color2);
			this.tabBGColors.Controls.Add(this.label25);
			this.tabBGColors.Controls.Add(this.lblBGColorG10Color2);
			this.tabBGColors.Controls.Add(this.txtBGColorG7Color2);
			this.tabBGColors.Controls.Add(this.txtBGColorG7Color1);
			this.tabBGColors.Controls.Add(this.txtBGColorG4Color2);
			this.tabBGColors.Controls.Add(this.txtBGColorG4Color1);
			this.tabBGColors.Controls.Add(this.btnBGColorG4Color1);
			this.tabBGColors.Controls.Add(this.btnBGColorG4Color2);
			this.tabBGColors.Controls.Add(this.btnBGColorG7Color1);
			this.tabBGColors.Controls.Add(this.btnBGColorG7Color2);
			this.tabBGColors.Controls.Add(this.label20);
			this.tabBGColors.Controls.Add(this.label22);
			this.tabBGColors.Controls.Add(this.lblBGColorG7Color2);
			this.tabBGColors.Controls.Add(this.lblBGColorG4Color2);
			this.tabBGColors.Controls.Add(this.label16);
			this.tabBGColors.Controls.Add(this.txtBGColorColHeader);
			this.tabBGColors.Controls.Add(this.txtBGColorGroupHeader);
			this.tabBGColors.Controls.Add(this.txtBGColorMerDesc);
			this.tabBGColors.Controls.Add(this.txtBGColorG1);
			this.tabBGColors.Controls.Add(this.btnBGColorG1);
			this.tabBGColors.Controls.Add(this.btnBGColorMerDesc);
			this.tabBGColors.Controls.Add(this.btnBGColorGroupHeader);
			this.tabBGColors.Controls.Add(this.btnBGColorColHeader);
			this.tabBGColors.Controls.Add(this.label17);
			this.tabBGColors.Controls.Add(this.label18);
			this.tabBGColors.Controls.Add(this.label19);
			this.tabBGColors.Controls.Add(this.label21);
			this.tabBGColors.Location = new System.Drawing.Point(4, 22);
			this.tabBGColors.Name = "tabBGColors";
			this.tabBGColors.Size = new System.Drawing.Size(440, 254);
			this.tabBGColors.TabIndex = 2;
			this.tabBGColors.Text = "Background Colors";
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(358, 8);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(64, 16);
			this.label33.TabIndex = 88;
			this.label33.Text = "RGB Value:";
			// 
			// txtBGColorG11G12Color2
			// 
			this.txtBGColorG11G12Color2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG11G12Color2.Location = new System.Drawing.Point(358, 224);
			this.txtBGColorG11G12Color2.Name = "txtBGColorG11G12Color2";
			this.txtBGColorG11G12Color2.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG11G12Color2.TabIndex = 87;
			this.txtBGColorG11G12Color2.Text = "100, 100, 100";
			this.txtBGColorG11G12Color2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG11G12Color2_KeyPress);
			this.txtBGColorG11G12Color2.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG11G12Color2_Validating);
			// 
			// txtBGColorG11G12Color1
			// 
			this.txtBGColorG11G12Color1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG11G12Color1.Location = new System.Drawing.Point(358, 200);
			this.txtBGColorG11G12Color1.Name = "txtBGColorG11G12Color1";
			this.txtBGColorG11G12Color1.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG11G12Color1.TabIndex = 86;
			this.txtBGColorG11G12Color1.Text = "100, 100, 100";
			this.txtBGColorG11G12Color1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG11G12Color1_KeyPress);
			this.txtBGColorG11G12Color1.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG11G12Color1_Validating);
			// 
			// btnBGColorG11G12Color1
			// 
			this.btnBGColorG11G12Color1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG11G12Color1.Location = new System.Drawing.Point(326, 200);
			this.btnBGColorG11G12Color1.Name = "btnBGColorG11G12Color1";
			this.btnBGColorG11G12Color1.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG11G12Color1.TabIndex = 85;
			this.btnBGColorG11G12Color1.Click += new System.EventHandler(this.btnBGColorG11G12Color1_Click);
			// 
			// btnBGColorG11G12Color2
			// 
			this.btnBGColorG11G12Color2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG11G12Color2.Location = new System.Drawing.Point(326, 224);
			this.btnBGColorG11G12Color2.Name = "btnBGColorG11G12Color2";
			this.btnBGColorG11G12Color2.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG11G12Color2.TabIndex = 84;
			this.btnBGColorG11G12Color2.Click += new System.EventHandler(this.btnBGColorG11G12Color2_Click);
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(222, 200);
			this.label27.Name = "label27";
			this.label27.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label27.Size = new System.Drawing.Size(100, 24);
			this.label27.TabIndex = 83;
			this.label27.Text = "All Store/Chain Data";
			// 
			// lblBGColorG11G12Color2
			// 
			this.lblBGColorG11G12Color2.Location = new System.Drawing.Point(222, 232);
			this.lblBGColorG11G12Color2.Name = "lblBGColorG11G12Color2";
			this.lblBGColorG11G12Color2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblBGColorG11G12Color2.Size = new System.Drawing.Size(100, 16);
			this.lblBGColorG11G12Color2.TabIndex = 82;
			this.lblBGColorG11G12Color2.Text = "Alternate Color";
			// 
			// txtBGColorG8G9Color2
			// 
			this.txtBGColorG8G9Color2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG8G9Color2.Location = new System.Drawing.Point(358, 168);
			this.txtBGColorG8G9Color2.Name = "txtBGColorG8G9Color2";
			this.txtBGColorG8G9Color2.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG8G9Color2.TabIndex = 81;
			this.txtBGColorG8G9Color2.Text = "100, 100, 100";
			this.txtBGColorG8G9Color2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG8G9Color2_KeyPress);
			this.txtBGColorG8G9Color2.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG8G9Color2_Validating);
			// 
			// txtBGColorG8G9Color1
			// 
			this.txtBGColorG8G9Color1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG8G9Color1.Location = new System.Drawing.Point(358, 144);
			this.txtBGColorG8G9Color1.Name = "txtBGColorG8G9Color1";
			this.txtBGColorG8G9Color1.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG8G9Color1.TabIndex = 80;
			this.txtBGColorG8G9Color1.Text = "100, 100, 100";
			this.txtBGColorG8G9Color1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG8G9Color1_KeyPress);
			this.txtBGColorG8G9Color1.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG8G9Color1_Validating);
			// 
			// txtBGColorG5G6Color2
			// 
			this.txtBGColorG5G6Color2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG5G6Color2.Location = new System.Drawing.Point(358, 112);
			this.txtBGColorG5G6Color2.Name = "txtBGColorG5G6Color2";
			this.txtBGColorG5G6Color2.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG5G6Color2.TabIndex = 79;
			this.txtBGColorG5G6Color2.Text = "100, 100, 100";
			this.txtBGColorG5G6Color2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG5G6Color2_KeyPress);
			this.txtBGColorG5G6Color2.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG5G6Color2_Validating);
			// 
			// txtBGColorG5G6Color1
			// 
			this.txtBGColorG5G6Color1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG5G6Color1.Location = new System.Drawing.Point(358, 88);
			this.txtBGColorG5G6Color1.Name = "txtBGColorG5G6Color1";
			this.txtBGColorG5G6Color1.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG5G6Color1.TabIndex = 78;
			this.txtBGColorG5G6Color1.Text = "100, 100, 100";
			this.txtBGColorG5G6Color1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG5G6Color1_KeyPress);
			this.txtBGColorG5G6Color1.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG5G6Color1_Validating);
			// 
			// btnBGColorG5G6Color1
			// 
			this.btnBGColorG5G6Color1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG5G6Color1.Location = new System.Drawing.Point(326, 88);
			this.btnBGColorG5G6Color1.Name = "btnBGColorG5G6Color1";
			this.btnBGColorG5G6Color1.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG5G6Color1.TabIndex = 77;
			this.btnBGColorG5G6Color1.Click += new System.EventHandler(this.btnBGColorG5G6Color1_Click);
			// 
			// btnBGColorG5G6Color2
			// 
			this.btnBGColorG5G6Color2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG5G6Color2.Location = new System.Drawing.Point(326, 112);
			this.btnBGColorG5G6Color2.Name = "btnBGColorG5G6Color2";
			this.btnBGColorG5G6Color2.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG5G6Color2.TabIndex = 76;
			this.btnBGColorG5G6Color2.Click += new System.EventHandler(this.btnBGColorG5G6Color2_Click);
			// 
			// btnBGColorG8G9Color1
			// 
			this.btnBGColorG8G9Color1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG8G9Color1.Location = new System.Drawing.Point(326, 144);
			this.btnBGColorG8G9Color1.Name = "btnBGColorG8G9Color1";
			this.btnBGColorG8G9Color1.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG8G9Color1.TabIndex = 75;
			this.btnBGColorG8G9Color1.Click += new System.EventHandler(this.btnBGColorG8G9Color1_Click);
			// 
			// btnBGColorG8G9Color2
			// 
			this.btnBGColorG8G9Color2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG8G9Color2.Location = new System.Drawing.Point(326, 168);
			this.btnBGColorG8G9Color2.Name = "btnBGColorG8G9Color2";
			this.btnBGColorG8G9Color2.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG8G9Color2.TabIndex = 74;
			this.btnBGColorG8G9Color2.Click += new System.EventHandler(this.btnBGColorG8G9Color2_Click);
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(222, 96);
			this.label29.Name = "label29";
			this.label29.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label29.Size = new System.Drawing.Size(100, 16);
			this.label29.TabIndex = 73;
			this.label29.Text = "Store Plan Data";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(222, 152);
			this.label30.Name = "label30";
			this.label30.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label30.Size = new System.Drawing.Size(100, 16);
			this.label30.TabIndex = 71;
			this.label30.Text = "Store Set Data";
			// 
			// lblBGColorG8G9Color2
			// 
			this.lblBGColorG8G9Color2.Location = new System.Drawing.Point(222, 176);
			this.lblBGColorG8G9Color2.Name = "lblBGColorG8G9Color2";
			this.lblBGColorG8G9Color2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblBGColorG8G9Color2.Size = new System.Drawing.Size(100, 16);
			this.lblBGColorG8G9Color2.TabIndex = 70;
			this.lblBGColorG8G9Color2.Text = "Alternate Color";
			// 
			// lblBGColorG5G6Color2
			// 
			this.lblBGColorG5G6Color2.Location = new System.Drawing.Point(222, 120);
			this.lblBGColorG5G6Color2.Name = "lblBGColorG5G6Color2";
			this.lblBGColorG5G6Color2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblBGColorG5G6Color2.Size = new System.Drawing.Size(100, 16);
			this.lblBGColorG5G6Color2.TabIndex = 72;
			this.lblBGColorG5G6Color2.Text = "Alternate Color";
			// 
			// txtBGColorG10Color2
			// 
			this.txtBGColorG10Color2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG10Color2.Location = new System.Drawing.Point(150, 224);
			this.txtBGColorG10Color2.Name = "txtBGColorG10Color2";
			this.txtBGColorG10Color2.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG10Color2.TabIndex = 69;
			this.txtBGColorG10Color2.Text = "100, 100, 100";
			this.txtBGColorG10Color2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG10Color2_KeyPress);
			this.txtBGColorG10Color2.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG10Color2_Validating);
			// 
			// txtBGColorG10Color1
			// 
			this.txtBGColorG10Color1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG10Color1.Location = new System.Drawing.Point(150, 200);
			this.txtBGColorG10Color1.Name = "txtBGColorG10Color1";
			this.txtBGColorG10Color1.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG10Color1.TabIndex = 68;
			this.txtBGColorG10Color1.Text = "100, 100, 100";
			this.txtBGColorG10Color1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG10Color1_KeyPress);
			this.txtBGColorG10Color1.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG10Color1_Validating);
			// 
			// btnBGColorG10Color1
			// 
			this.btnBGColorG10Color1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG10Color1.Location = new System.Drawing.Point(118, 200);
			this.btnBGColorG10Color1.Name = "btnBGColorG10Color1";
			this.btnBGColorG10Color1.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG10Color1.TabIndex = 67;
			this.btnBGColorG10Color1.Click += new System.EventHandler(this.btnBGColorG10Color1_Click);
			// 
			// btnBGColorG10Color2
			// 
			this.btnBGColorG10Color2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG10Color2.Location = new System.Drawing.Point(118, 224);
			this.btnBGColorG10Color2.Name = "btnBGColorG10Color2";
			this.btnBGColorG10Color2.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG10Color2.TabIndex = 66;
			this.btnBGColorG10Color2.Click += new System.EventHandler(this.btnBGColorG10Color2_Click);
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(14, 200);
			this.label25.Name = "label25";
			this.label25.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label25.Size = new System.Drawing.Size(100, 24);
			this.label25.TabIndex = 65;
			this.label25.Text = "All Store/Chain Header";
			// 
			// lblBGColorG10Color2
			// 
			this.lblBGColorG10Color2.Location = new System.Drawing.Point(14, 232);
			this.lblBGColorG10Color2.Name = "lblBGColorG10Color2";
			this.lblBGColorG10Color2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblBGColorG10Color2.Size = new System.Drawing.Size(100, 16);
			this.lblBGColorG10Color2.TabIndex = 64;
			this.lblBGColorG10Color2.Text = "Alternate Color";
			// 
			// txtBGColorG7Color2
			// 
			this.txtBGColorG7Color2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG7Color2.Location = new System.Drawing.Point(150, 168);
			this.txtBGColorG7Color2.Name = "txtBGColorG7Color2";
			this.txtBGColorG7Color2.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG7Color2.TabIndex = 63;
			this.txtBGColorG7Color2.Text = "100, 100, 100";
			this.txtBGColorG7Color2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG7Color2_KeyPress);
			this.txtBGColorG7Color2.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG7Color2_Validating);
			// 
			// txtBGColorG7Color1
			// 
			this.txtBGColorG7Color1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG7Color1.Location = new System.Drawing.Point(150, 144);
			this.txtBGColorG7Color1.Name = "txtBGColorG7Color1";
			this.txtBGColorG7Color1.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG7Color1.TabIndex = 62;
			this.txtBGColorG7Color1.Text = "100, 100, 100";
			this.txtBGColorG7Color1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG7Color1_KeyPress);
			this.txtBGColorG7Color1.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG7Color1_Validating);
			// 
			// txtBGColorG4Color2
			// 
			this.txtBGColorG4Color2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG4Color2.Location = new System.Drawing.Point(150, 112);
			this.txtBGColorG4Color2.Name = "txtBGColorG4Color2";
			this.txtBGColorG4Color2.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG4Color2.TabIndex = 61;
			this.txtBGColorG4Color2.Text = "100, 100, 100";
			this.txtBGColorG4Color2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG4Color2_KeyPress);
			this.txtBGColorG4Color2.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG4Color2_Validating);
			// 
			// txtBGColorG4Color1
			// 
			this.txtBGColorG4Color1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG4Color1.Location = new System.Drawing.Point(150, 88);
			this.txtBGColorG4Color1.Name = "txtBGColorG4Color1";
			this.txtBGColorG4Color1.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG4Color1.TabIndex = 60;
			this.txtBGColorG4Color1.Text = "100, 100, 100";
			this.txtBGColorG4Color1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG4Color1_KeyPress);
			this.txtBGColorG4Color1.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG4Color1_Validating);
			// 
			// btnBGColorG4Color1
			// 
			this.btnBGColorG4Color1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG4Color1.Location = new System.Drawing.Point(118, 88);
			this.btnBGColorG4Color1.Name = "btnBGColorG4Color1";
			this.btnBGColorG4Color1.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG4Color1.TabIndex = 59;
			this.btnBGColorG4Color1.Click += new System.EventHandler(this.btnBGColorG4Color1_Click);
			// 
			// btnBGColorG4Color2
			// 
			this.btnBGColorG4Color2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG4Color2.Location = new System.Drawing.Point(118, 112);
			this.btnBGColorG4Color2.Name = "btnBGColorG4Color2";
			this.btnBGColorG4Color2.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG4Color2.TabIndex = 58;
			this.btnBGColorG4Color2.Click += new System.EventHandler(this.btnBGColorG4Color2_Click);
			// 
			// btnBGColorG7Color1
			// 
			this.btnBGColorG7Color1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG7Color1.Location = new System.Drawing.Point(118, 144);
			this.btnBGColorG7Color1.Name = "btnBGColorG7Color1";
			this.btnBGColorG7Color1.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG7Color1.TabIndex = 57;
			this.btnBGColorG7Color1.Click += new System.EventHandler(this.btnBGColorG7Color1_Click);
			// 
			// btnBGColorG7Color2
			// 
			this.btnBGColorG7Color2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG7Color2.Location = new System.Drawing.Point(118, 168);
			this.btnBGColorG7Color2.Name = "btnBGColorG7Color2";
			this.btnBGColorG7Color2.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG7Color2.TabIndex = 56;
			this.btnBGColorG7Color2.Click += new System.EventHandler(this.btnBGColorG7Color2_Click);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(14, 96);
			this.label20.Name = "label20";
			this.label20.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label20.Size = new System.Drawing.Size(100, 16);
			this.label20.TabIndex = 55;
			this.label20.Text = "Store Plan Header";
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(14, 152);
			this.label22.Name = "label22";
			this.label22.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label22.Size = new System.Drawing.Size(100, 16);
			this.label22.TabIndex = 53;
			this.label22.Text = "Store Set Header";
			// 
			// lblBGColorG7Color2
			// 
			this.lblBGColorG7Color2.Location = new System.Drawing.Point(14, 176);
			this.lblBGColorG7Color2.Name = "lblBGColorG7Color2";
			this.lblBGColorG7Color2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblBGColorG7Color2.Size = new System.Drawing.Size(100, 16);
			this.lblBGColorG7Color2.TabIndex = 52;
			this.lblBGColorG7Color2.Text = "Alternate Color";
			// 
			// lblBGColorG4Color2
			// 
			this.lblBGColorG4Color2.Location = new System.Drawing.Point(14, 120);
			this.lblBGColorG4Color2.Name = "lblBGColorG4Color2";
			this.lblBGColorG4Color2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblBGColorG4Color2.Size = new System.Drawing.Size(100, 16);
			this.lblBGColorG4Color2.TabIndex = 54;
			this.lblBGColorG4Color2.Text = "Alternate Color";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(150, 8);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(64, 16);
			this.label16.TabIndex = 51;
			this.label16.Text = "RGB Value:";
			// 
			// txtBGColorColHeader
			// 
			this.txtBGColorColHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorColHeader.Location = new System.Drawing.Point(358, 48);
			this.txtBGColorColHeader.Name = "txtBGColorColHeader";
			this.txtBGColorColHeader.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorColHeader.TabIndex = 49;
			this.txtBGColorColHeader.Text = "100, 100, 100";
			this.txtBGColorColHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorColHeader_KeyPress);
			this.txtBGColorColHeader.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorColHeader_Validating);
			// 
			// txtBGColorGroupHeader
			// 
			this.txtBGColorGroupHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorGroupHeader.Location = new System.Drawing.Point(358, 24);
			this.txtBGColorGroupHeader.Name = "txtBGColorGroupHeader";
			this.txtBGColorGroupHeader.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorGroupHeader.TabIndex = 48;
			this.txtBGColorGroupHeader.Text = "100, 100, 100";
			this.txtBGColorGroupHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorGroupHeader_KeyPress);
			this.txtBGColorGroupHeader.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorGroupHeader_Validating);
			// 
			// txtBGColorMerDesc
			// 
			this.txtBGColorMerDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorMerDesc.Location = new System.Drawing.Point(150, 24);
			this.txtBGColorMerDesc.Name = "txtBGColorMerDesc";
			this.txtBGColorMerDesc.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorMerDesc.TabIndex = 47;
			this.txtBGColorMerDesc.Text = "100, 100, 100";
			this.txtBGColorMerDesc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorMerDesc_KeyPress);
			this.txtBGColorMerDesc.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorMerDesc_Validating);
			// 
			// txtBGColorG1
			// 
			this.txtBGColorG1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtBGColorG1.Location = new System.Drawing.Point(150, 48);
			this.txtBGColorG1.Name = "txtBGColorG1";
			this.txtBGColorG1.Size = new System.Drawing.Size(64, 18);
			this.txtBGColorG1.TabIndex = 46;
			this.txtBGColorG1.Text = "100, 100, 100";
			this.txtBGColorG1.Visible = false;
			this.txtBGColorG1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBGColorG1_KeyPress);
			this.txtBGColorG1.Validating += new System.ComponentModel.CancelEventHandler(this.txtBGColorG1_Validating);
			// 
			// btnBGColorG1
			// 
			this.btnBGColorG1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorG1.Location = new System.Drawing.Point(118, 48);
			this.btnBGColorG1.Name = "btnBGColorG1";
			this.btnBGColorG1.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorG1.TabIndex = 45;
			this.btnBGColorG1.Visible = false;
			this.btnBGColorG1.Click += new System.EventHandler(this.btnBGColorG1_Click);
			// 
			// btnBGColorMerDesc
			// 
			this.btnBGColorMerDesc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorMerDesc.Location = new System.Drawing.Point(118, 24);
			this.btnBGColorMerDesc.Name = "btnBGColorMerDesc";
			this.btnBGColorMerDesc.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorMerDesc.TabIndex = 44;
			this.btnBGColorMerDesc.Click += new System.EventHandler(this.btnBGColorMerDesc_Click);
			// 
			// btnBGColorGroupHeader
			// 
			this.btnBGColorGroupHeader.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorGroupHeader.Location = new System.Drawing.Point(326, 24);
			this.btnBGColorGroupHeader.Name = "btnBGColorGroupHeader";
			this.btnBGColorGroupHeader.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorGroupHeader.TabIndex = 43;
			this.btnBGColorGroupHeader.Click += new System.EventHandler(this.btnBGColorGroupHeader_Click);
			// 
			// btnBGColorColHeader
			// 
			this.btnBGColorColHeader.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnBGColorColHeader.Location = new System.Drawing.Point(326, 48);
			this.btnBGColorColHeader.Name = "btnBGColorColHeader";
			this.btnBGColorColHeader.Size = new System.Drawing.Size(24, 23);
			this.btnBGColorColHeader.TabIndex = 42;
			this.btnBGColorColHeader.Click += new System.EventHandler(this.btnBGColorColHeader_Click);
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(14, 56);
			this.label17.Name = "label17";
			this.label17.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label17.Size = new System.Drawing.Size(100, 16);
			this.label17.TabIndex = 40;
			this.label17.Text = "Corner Grid";
			this.label17.Visible = false;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(222, 32);
			this.label18.Name = "label18";
			this.label18.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label18.Size = new System.Drawing.Size(100, 16);
			this.label18.TabIndex = 38;
			this.label18.Text = "Group Header";
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(222, 56);
			this.label19.Name = "label19";
			this.label19.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label19.Size = new System.Drawing.Size(100, 16);
			this.label19.TabIndex = 37;
			this.label19.Text = "Column Header";
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(14, 32);
			this.label21.Name = "label21";
			this.label21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label21.Size = new System.Drawing.Size(100, 16);
			this.label21.TabIndex = 39;
			this.label21.Text = "Merchandise Desc";
			// 
			// tabForeColors
			// 
			this.tabForeColors.Controls.Add(this.label15);
			this.tabForeColors.Controls.Add(this.label14);
			this.tabForeColors.Controls.Add(this.txtForeColorG11G12);
			this.tabForeColors.Controls.Add(this.txtForeColorG10);
			this.tabForeColors.Controls.Add(this.txtForeColorG8G9);
			this.tabForeColors.Controls.Add(this.txtForeColorG7);
			this.tabForeColors.Controls.Add(this.txtForeColorG5G6);
			this.tabForeColors.Controls.Add(this.txtForeColorG4);
			this.tabForeColors.Controls.Add(this.txtForeColorNegative);
			this.tabForeColors.Controls.Add(this.txtForeColorColHeader);
			this.tabForeColors.Controls.Add(this.txtForeColorGroupHeader);
			this.tabForeColors.Controls.Add(this.txtForeColorMerDesc);
			this.tabForeColors.Controls.Add(this.txtForeColorG1);
			this.tabForeColors.Controls.Add(this.btnForeColorG1);
			this.tabForeColors.Controls.Add(this.btnForeColorMerDesc);
			this.tabForeColors.Controls.Add(this.btnForeColorGroupHeader);
			this.tabForeColors.Controls.Add(this.btnForeColorColHeader);
			this.tabForeColors.Controls.Add(this.btnForeColorNegative);
			this.tabForeColors.Controls.Add(this.btnForeColorG4);
			this.tabForeColors.Controls.Add(this.btnForeColorG5G6);
			this.tabForeColors.Controls.Add(this.btnForeColorG7);
			this.tabForeColors.Controls.Add(this.btnForeColorG8G9);
			this.tabForeColors.Controls.Add(this.btnForeColorG10);
			this.tabForeColors.Controls.Add(this.btnForeColorG11G12);
			this.tabForeColors.Controls.Add(this.label13);
			this.tabForeColors.Controls.Add(this.label11);
			this.tabForeColors.Controls.Add(this.label10);
			this.tabForeColors.Controls.Add(this.label9);
			this.tabForeColors.Controls.Add(this.label8);
			this.tabForeColors.Controls.Add(this.label7);
			this.tabForeColors.Controls.Add(this.label6);
			this.tabForeColors.Controls.Add(this.label5);
			this.tabForeColors.Controls.Add(this.label4);
			this.tabForeColors.Controls.Add(this.label3);
			this.tabForeColors.Controls.Add(this.label12);
			this.tabForeColors.Location = new System.Drawing.Point(4, 22);
			this.tabForeColors.Name = "tabForeColors";
			this.tabForeColors.Size = new System.Drawing.Size(440, 254);
			this.tabForeColors.TabIndex = 4;
			this.tabForeColors.Text = "Fore Colors";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(352, 16);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(64, 16);
			this.label15.TabIndex = 36;
			this.label15.Text = "RGB Value:";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(136, 16);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(64, 16);
			this.label14.TabIndex = 35;
			this.label14.Text = "RGB Value:";
			// 
			// txtForeColorG11G12
			// 
			this.txtForeColorG11G12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorG11G12.Location = new System.Drawing.Point(352, 152);
			this.txtForeColorG11G12.Name = "txtForeColorG11G12";
			this.txtForeColorG11G12.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorG11G12.TabIndex = 34;
			this.txtForeColorG11G12.Text = "100, 100, 100";
			this.txtForeColorG11G12.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG11G12_KeyPress);
			this.txtForeColorG11G12.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorG11G12_Validating);
			// 
			// txtForeColorG10
			// 
			this.txtForeColorG10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorG10.Location = new System.Drawing.Point(352, 128);
			this.txtForeColorG10.Name = "txtForeColorG10";
			this.txtForeColorG10.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorG10.TabIndex = 33;
			this.txtForeColorG10.Text = "100, 100, 100";
			this.txtForeColorG10.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG10_KeyPress);
			this.txtForeColorG10.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorG10_Validating);
			// 
			// txtForeColorG8G9
			// 
			this.txtForeColorG8G9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorG8G9.Location = new System.Drawing.Point(352, 104);
			this.txtForeColorG8G9.Name = "txtForeColorG8G9";
			this.txtForeColorG8G9.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorG8G9.TabIndex = 32;
			this.txtForeColorG8G9.Text = "100, 100, 100";
			this.txtForeColorG8G9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG8G9_KeyPress);
			this.txtForeColorG8G9.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorG8G9_Validating);
			// 
			// txtForeColorG7
			// 
			this.txtForeColorG7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorG7.Location = new System.Drawing.Point(352, 80);
			this.txtForeColorG7.Name = "txtForeColorG7";
			this.txtForeColorG7.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorG7.TabIndex = 31;
			this.txtForeColorG7.Text = "100, 100, 100";
			this.txtForeColorG7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG7_KeyPress);
			this.txtForeColorG7.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorG7_Validating);
			// 
			// txtForeColorG5G6
			// 
			this.txtForeColorG5G6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorG5G6.Location = new System.Drawing.Point(352, 56);
			this.txtForeColorG5G6.Name = "txtForeColorG5G6";
			this.txtForeColorG5G6.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorG5G6.TabIndex = 30;
			this.txtForeColorG5G6.Text = "100, 100, 100";
			this.txtForeColorG5G6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG5G6_KeyPress);
			this.txtForeColorG5G6.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorG5G6_Validating);
			// 
			// txtForeColorG4
			// 
			this.txtForeColorG4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorG4.Location = new System.Drawing.Point(352, 32);
			this.txtForeColorG4.Name = "txtForeColorG4";
			this.txtForeColorG4.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorG4.TabIndex = 29;
			this.txtForeColorG4.Text = "100, 100, 100";
			this.txtForeColorG4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG4_KeyPress);
			this.txtForeColorG4.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorG4_Validating);
			// 
			// txtForeColorNegative
			// 
			this.txtForeColorNegative.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorNegative.Location = new System.Drawing.Point(136, 104);
			this.txtForeColorNegative.Name = "txtForeColorNegative";
			this.txtForeColorNegative.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorNegative.TabIndex = 28;
			this.txtForeColorNegative.Text = "100, 100, 100";
			this.txtForeColorNegative.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorNegative_KeyPress);
			this.txtForeColorNegative.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorNegative_Validating);
			// 
			// txtForeColorColHeader
			// 
			this.txtForeColorColHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorColHeader.Location = new System.Drawing.Point(136, 80);
			this.txtForeColorColHeader.Name = "txtForeColorColHeader";
			this.txtForeColorColHeader.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorColHeader.TabIndex = 27;
			this.txtForeColorColHeader.Text = "100, 100, 100";
			this.txtForeColorColHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorColHeader_KeyPress);
			this.txtForeColorColHeader.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorColHeader_Validating);
			// 
			// txtForeColorGroupHeader
			// 
			this.txtForeColorGroupHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorGroupHeader.Location = new System.Drawing.Point(136, 56);
			this.txtForeColorGroupHeader.Name = "txtForeColorGroupHeader";
			this.txtForeColorGroupHeader.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorGroupHeader.TabIndex = 26;
			this.txtForeColorGroupHeader.Text = "100, 100, 100";
			this.txtForeColorGroupHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorGroupHeader_KeyPress);
			this.txtForeColorGroupHeader.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorGroupHeader_Validating);
			// 
			// txtForeColorMerDesc
			// 
			this.txtForeColorMerDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorMerDesc.Location = new System.Drawing.Point(136, 32);
			this.txtForeColorMerDesc.Name = "txtForeColorMerDesc";
			this.txtForeColorMerDesc.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorMerDesc.TabIndex = 25;
			this.txtForeColorMerDesc.Text = "100, 100, 100";
			this.txtForeColorMerDesc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorMerDesc_KeyPress);
			this.txtForeColorMerDesc.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorMerDesc_Validating);
			// 
			// txtForeColorG1
			// 
			this.txtForeColorG1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtForeColorG1.Location = new System.Drawing.Point(136, 128);
			this.txtForeColorG1.Name = "txtForeColorG1";
			this.txtForeColorG1.Size = new System.Drawing.Size(64, 18);
			this.txtForeColorG1.TabIndex = 24;
			this.txtForeColorG1.Text = "100, 100, 100";
			this.txtForeColorG1.Visible = false;
			this.txtForeColorG1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtForeColorG1_KeyPress);
			this.txtForeColorG1.Validating += new System.ComponentModel.CancelEventHandler(this.txtForeColorG1_Validating);
			// 
			// btnForeColorG1
			// 
			this.btnForeColorG1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorG1.Location = new System.Drawing.Point(104, 128);
			this.btnForeColorG1.Name = "btnForeColorG1";
			this.btnForeColorG1.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorG1.TabIndex = 23;
			this.btnForeColorG1.Visible = false;
			this.btnForeColorG1.Click += new System.EventHandler(this.btnForeColorG1_Click);
			// 
			// btnForeColorMerDesc
			// 
			this.btnForeColorMerDesc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorMerDesc.Location = new System.Drawing.Point(104, 32);
			this.btnForeColorMerDesc.Name = "btnForeColorMerDesc";
			this.btnForeColorMerDesc.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorMerDesc.TabIndex = 22;
			this.btnForeColorMerDesc.Click += new System.EventHandler(this.btnForeColorMerDesc_Click);
			// 
			// btnForeColorGroupHeader
			// 
			this.btnForeColorGroupHeader.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorGroupHeader.Location = new System.Drawing.Point(104, 56);
			this.btnForeColorGroupHeader.Name = "btnForeColorGroupHeader";
			this.btnForeColorGroupHeader.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorGroupHeader.TabIndex = 21;
			this.btnForeColorGroupHeader.Click += new System.EventHandler(this.btnForeColorGroupHeader_Click);
			// 
			// btnForeColorColHeader
			// 
			this.btnForeColorColHeader.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorColHeader.Location = new System.Drawing.Point(104, 80);
			this.btnForeColorColHeader.Name = "btnForeColorColHeader";
			this.btnForeColorColHeader.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorColHeader.TabIndex = 20;
			this.btnForeColorColHeader.Click += new System.EventHandler(this.btnForeColorColHeader_Click);
			// 
			// btnForeColorNegative
			// 
			this.btnForeColorNegative.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorNegative.Location = new System.Drawing.Point(104, 104);
			this.btnForeColorNegative.Name = "btnForeColorNegative";
			this.btnForeColorNegative.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorNegative.TabIndex = 19;
			this.btnForeColorNegative.Click += new System.EventHandler(this.btnForeColorNegative_Click);
			// 
			// btnForeColorG4
			// 
			this.btnForeColorG4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorG4.Location = new System.Drawing.Point(320, 32);
			this.btnForeColorG4.Name = "btnForeColorG4";
			this.btnForeColorG4.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorG4.TabIndex = 18;
			this.btnForeColorG4.Click += new System.EventHandler(this.btnForeColorG4_Click);
			// 
			// btnForeColorG5G6
			// 
			this.btnForeColorG5G6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorG5G6.Location = new System.Drawing.Point(320, 56);
			this.btnForeColorG5G6.Name = "btnForeColorG5G6";
			this.btnForeColorG5G6.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorG5G6.TabIndex = 17;
			this.btnForeColorG5G6.Click += new System.EventHandler(this.btnForeColorG5G6_Click);
			// 
			// btnForeColorG7
			// 
			this.btnForeColorG7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorG7.Location = new System.Drawing.Point(320, 80);
			this.btnForeColorG7.Name = "btnForeColorG7";
			this.btnForeColorG7.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorG7.TabIndex = 16;
			this.btnForeColorG7.Click += new System.EventHandler(this.btnForeColorG7_Click);
			// 
			// btnForeColorG8G9
			// 
			this.btnForeColorG8G9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorG8G9.Location = new System.Drawing.Point(320, 104);
			this.btnForeColorG8G9.Name = "btnForeColorG8G9";
			this.btnForeColorG8G9.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorG8G9.TabIndex = 15;
			this.btnForeColorG8G9.Click += new System.EventHandler(this.btnForeColorG8G9_Click);
			// 
			// btnForeColorG10
			// 
			this.btnForeColorG10.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorG10.Location = new System.Drawing.Point(320, 128);
			this.btnForeColorG10.Name = "btnForeColorG10";
			this.btnForeColorG10.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorG10.TabIndex = 14;
			this.btnForeColorG10.Click += new System.EventHandler(this.btnForeColorG10_Click);
			// 
			// btnForeColorG11G12
			// 
			this.btnForeColorG11G12.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnForeColorG11G12.Location = new System.Drawing.Point(320, 152);
			this.btnForeColorG11G12.Name = "btnForeColorG11G12";
			this.btnForeColorG11G12.Size = new System.Drawing.Size(24, 23);
			this.btnForeColorG11G12.TabIndex = 13;
			this.btnForeColorG11G12.Click += new System.EventHandler(this.btnForeColorG11G12_Click);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(0, 136);
			this.label13.Name = "label13";
			this.label13.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label13.Size = new System.Drawing.Size(100, 16);
			this.label13.TabIndex = 10;
			this.label13.Text = "Corner Grid";
			this.label13.Visible = false;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(0, 64);
			this.label11.Name = "label11";
			this.label11.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label11.Size = new System.Drawing.Size(100, 16);
			this.label11.TabIndex = 8;
			this.label11.Text = "Group Header";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(0, 88);
			this.label10.Name = "label10";
			this.label10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label10.Size = new System.Drawing.Size(100, 16);
			this.label10.TabIndex = 7;
			this.label10.Text = "Column Header";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(0, 112);
			this.label9.Name = "label9";
			this.label9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label9.Size = new System.Drawing.Size(100, 16);
			this.label9.TabIndex = 6;
			this.label9.Text = "Negative Numbers";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(216, 40);
			this.label8.Name = "label8";
			this.label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label8.Size = new System.Drawing.Size(100, 16);
			this.label8.TabIndex = 5;
			this.label8.Text = "Store Plan Header";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(216, 64);
			this.label7.Name = "label7";
			this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label7.Size = new System.Drawing.Size(100, 16);
			this.label7.TabIndex = 4;
			this.label7.Text = "Store Plan Data";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(216, 88);
			this.label6.Name = "label6";
			this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 3;
			this.label6.Text = "Store Set Header";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(216, 112);
			this.label5.Name = "label5";
			this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label5.Size = new System.Drawing.Size(100, 16);
			this.label5.TabIndex = 2;
			this.label5.Text = "Store Set Data";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(200, 136);
			this.label4.Name = "label4";
			this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label4.Size = new System.Drawing.Size(120, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "All Store/Chain Header";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(200, 160);
			this.label3.Name = "label3";
			this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label3.Size = new System.Drawing.Size(120, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "All Store/Chain Data";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(-24, 40);
			this.label12.Name = "label12";
			this.label12.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label12.Size = new System.Drawing.Size(130, 16);
			this.label12.TabIndex = 9;
			this.label12.Text = ".Merchandise Desc";
			// 
			// tabFonts
			// 
			this.tabFonts.Controls.Add(this.grpTextEffects);
			this.tabFonts.Controls.Add(this.lvFonts);
			this.tabFonts.Location = new System.Drawing.Point(4, 22);
			this.tabFonts.Name = "tabFonts";
			this.tabFonts.Size = new System.Drawing.Size(440, 254);
			this.tabFonts.TabIndex = 1;
			this.tabFonts.Text = "Fonts";
			// 
			// grpTextEffects
			// 
			this.grpTextEffects.Controls.Add(this.label24);
			this.grpTextEffects.Controls.Add(this.label23);
			this.grpTextEffects.Controls.Add(this.label2);
			this.grpTextEffects.Controls.Add(this.cboTextEffectColHeader);
			this.grpTextEffects.Controls.Add(this.cboTextEffectGroupHeader);
			this.grpTextEffects.Controls.Add(this.cboTextEffectMerDesc);
			this.grpTextEffects.Location = new System.Drawing.Point(6, 152);
			this.grpTextEffects.Name = "grpTextEffects";
			this.grpTextEffects.Size = new System.Drawing.Size(426, 96);
			this.grpTextEffects.TabIndex = 1;
			this.grpTextEffects.TabStop = false;
			this.grpTextEffects.Text = "Text Effects";
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(48, 72);
			this.label24.Name = "label24";
			this.label24.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label24.Size = new System.Drawing.Size(136, 16);
			this.label24.TabIndex = 5;
			this.label24.Text = "Column Header";
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(48, 48);
			this.label23.Name = "label23";
			this.label23.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label23.Size = new System.Drawing.Size(136, 16);
			this.label23.TabIndex = 4;
			this.label23.Text = "Group Header";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(48, 24);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Merchandise Description";
			// 
			// cboTextEffectColHeader
			// 
			this.cboTextEffectColHeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTextEffectColHeader.Items.AddRange(new object[] {
																		"Flat",
																		"Inset",
																		"Raised"});
			this.cboTextEffectColHeader.Location = new System.Drawing.Point(192, 64);
			this.cboTextEffectColHeader.Name = "cboTextEffectColHeader";
			this.cboTextEffectColHeader.Size = new System.Drawing.Size(88, 21);
			this.cboTextEffectColHeader.TabIndex = 2;
			this.cboTextEffectColHeader.SelectionChangeCommitted += new System.EventHandler(this.cboTextEffectColHeader_SelectionChangeCommitted);
            this.cboTextEffectColHeader.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboTextEffectColHeader_MIDComboBoxPropertiesChangedEvent);
			// 
			// cboTextEffectGroupHeader
			// 
			this.cboTextEffectGroupHeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTextEffectGroupHeader.Items.AddRange(new object[] {
																		  "Flat",
																		  "Inset",
																		  "Raised"});
			this.cboTextEffectGroupHeader.Location = new System.Drawing.Point(192, 40);
			this.cboTextEffectGroupHeader.Name = "cboTextEffectGroupHeader";
			this.cboTextEffectGroupHeader.Size = new System.Drawing.Size(88, 21);
			this.cboTextEffectGroupHeader.TabIndex = 1;
			this.cboTextEffectGroupHeader.SelectionChangeCommitted += new System.EventHandler(this.cboTextEffectGroupHeader_SelectionChangeCommitted);
            this.cboTextEffectGroupHeader.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboTextEffectGroupHeader_MIDComboBoxPropertiesChangedEvent);
			// 
			// cboTextEffectMerDesc
			// 
			this.cboTextEffectMerDesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTextEffectMerDesc.Items.AddRange(new object[] {
																	  "Flat",
																	  "Inset",
																	  "Raised"});
			this.cboTextEffectMerDesc.Location = new System.Drawing.Point(192, 16);
			this.cboTextEffectMerDesc.Name = "cboTextEffectMerDesc";
			this.cboTextEffectMerDesc.Size = new System.Drawing.Size(88, 21);
			this.cboTextEffectMerDesc.TabIndex = 0;
			this.cboTextEffectMerDesc.SelectionChangeCommitted += new System.EventHandler(this.cboTextEffectMerDesc_SelectionChangeCommitted);
            this.cboTextEffectMerDesc.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboTextEffectMerDesc_MIDComboBoxPropertiesChangedEvent);
			// 
			// lvFonts
			// 
			this.lvFonts.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.lvFonts.AutoArrange = false;
			this.lvFonts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.colName,
																					  this.colFontFamily,
																					  this.colSize,
																					  this.bolIsBold,
																					  this.colIsItalic,
																					  this.colIsUnderline});
			this.lvFonts.FullRowSelect = true;
			this.lvFonts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvFonts.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																					listViewItem1,
																					listViewItem2,
																					listViewItem3,
																					listViewItem4,
																					listViewItem5,
																					listViewItem6,
																					listViewItem7,
																					listViewItem8});
			this.lvFonts.Location = new System.Drawing.Point(6, 8);
			this.lvFonts.MultiSelect = false;
			this.lvFonts.Name = "lvFonts";
			this.lvFonts.Size = new System.Drawing.Size(426, 136);
			this.lvFonts.TabIndex = 0;
			this.lvFonts.View = System.Windows.Forms.View.Details;
			this.lvFonts.ItemActivate += new System.EventHandler(this.lvFonts_ItemActivate);
			this.lvFonts.SelectedIndexChanged += new System.EventHandler(this.lvFonts_SelectedIndexChanged);
			// 
			// colName
			// 
			this.colName.Text = "";
			this.colName.Width = 110;
			// 
			// colFontFamily
			// 
			this.colFontFamily.Text = "Font Family";
			this.colFontFamily.Width = 100;
			// 
			// colSize
			// 
			this.colSize.Text = "Size";
			this.colSize.Width = 45;
			// 
			// bolIsBold
			// 
			this.bolIsBold.Text = "Bold";
			this.bolIsBold.Width = 45;
			// 
			// colIsItalic
			// 
			this.colIsItalic.Text = "Italic";
			this.colIsItalic.Width = 45;
			// 
			// colIsUnderline
			// 
			this.colIsUnderline.Text = "Underline";
			this.colIsUnderline.Width = 58;
			// 
			// tabSavedStyles
			// 
			this.tabSavedStyles.Controls.Add(this.lstThemes);
			this.tabSavedStyles.Controls.Add(this.btnDeleteStyle);
			this.tabSavedStyles.Controls.Add(this.btnRenameStyle);
			this.tabSavedStyles.Location = new System.Drawing.Point(4, 22);
			this.tabSavedStyles.Name = "tabSavedStyles";
			this.tabSavedStyles.Size = new System.Drawing.Size(440, 254);
			this.tabSavedStyles.TabIndex = 5;
			this.tabSavedStyles.Text = "System Themes";
			// 
			// lstThemes
			// 
			this.lstThemes.Location = new System.Drawing.Point(8, 8);
			this.lstThemes.Name = "lstThemes";
			this.lstThemes.Size = new System.Drawing.Size(192, 238);
			this.lstThemes.TabIndex = 0;
			this.lstThemes.SelectedIndexChanged += new System.EventHandler(this.lstThemes_SelectedIndexChanged);
			// 
			// btnDeleteStyle
			// 
			this.btnDeleteStyle.Location = new System.Drawing.Point(208, 40);
			this.btnDeleteStyle.Name = "btnDeleteStyle";
			this.btnDeleteStyle.Size = new System.Drawing.Size(104, 23);
			this.btnDeleteStyle.TabIndex = 2;
			this.btnDeleteStyle.Text = "Delete";
			this.btnDeleteStyle.Visible = false;
			this.btnDeleteStyle.Click += new System.EventHandler(this.btnDeleteStyle_Click);
			// 
			// btnRenameStyle
			// 
			this.btnRenameStyle.Location = new System.Drawing.Point(208, 8);
			this.btnRenameStyle.Name = "btnRenameStyle";
			this.btnRenameStyle.Size = new System.Drawing.Size(104, 23);
			this.btnRenameStyle.TabIndex = 1;
			this.btnRenameStyle.Text = "Rename";
			this.btnRenameStyle.Visible = false;
			this.btnRenameStyle.Click += new System.EventHandler(this.btnRenameStyle_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(301, 296);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(381, 296);
			this.btnApply.Name = "btnApply";
			this.btnApply.TabIndex = 2;
			this.btnApply.Text = "Apply";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(221, 296);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// ThemeProperties
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(466, 328);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ThemeProperties";
			this.Text = "Theme Properties";
			this.Load += new System.EventHandler(this.ThemeProperties_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tabBorders.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.grpDividerOptions.ResumeLayout(false);
			this.grpCellBorder.ResumeLayout(false);
			this.tabBGColors.ResumeLayout(false);
			this.tabForeColors.ResumeLayout(false);
			this.tabFonts.ResumeLayout(false);
			this.grpTextEffects.ResumeLayout(false);
			this.tabSavedStyles.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		#region Variable Declarations

		public event EventHandler ApplyButtonClicked;
		private string ImageDir;
		private Theme _currentTheme;
		private DataTable _systemThemeTable;
		private System.Windows.Forms.ListBox lstThemes;
		private bool _formLoading;

		#endregion

		#region Properties

		public Theme CurrentTheme
		{
			get
			{
				return _currentTheme;
			}
		}

		#endregion

		#region Constructor

		public ThemeProperties(Theme aTheme)
		{
			ThemeData themeData;

			InitializeComponent();

			_currentTheme = new Theme(aTheme);

			themeData = new ThemeData();
			_systemThemeTable = themeData.Theme_ReadByUser(Include.GlobalUserRID);	// Issue 3806

			BindThemeList();

            // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
            //ImageDir = MIDConfigurationManager.AppSettings[Include.MIDApplicationRoot] + MIDGraphics.GraphicsDir;
            ImageDir = MIDGraphics.MIDGraphicsDir;
            // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration
		}

		#endregion

		#region Form Events

		private void ThemeProperties_Load(object sender, System.EventArgs e)
		{
			PopulateBGColors();
			PopulateViewStyle();
			PopulateFonts();
			PopulateForeColors();
			PopulateBorderOptions();
		}
		
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void btnApply_Click(object sender, System.EventArgs e)
		{
			this.ApplyButtonClicked(this, new System.EventArgs());
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}		
		
		#endregion

		#region Base Style tab
		private void PopulateViewStyle()
		{
			switch (_currentTheme.ViewStyle)
			{
				case StyleEnum.Plain:
					optPlain.Checked = true; 
					picboxBaseStyle.Image = new Bitmap(ImageDir + "\\ViewStyle_Plain.gif");
					break;
				case StyleEnum.AlterColors:
					optAlter.Checked = true;
					picboxBaseStyle.Image = new Bitmap(ImageDir + "\\ViewStyle_AlterColor.gif");
					break;
				case StyleEnum.HighlightName:
					optHighlightName.Checked = true;
					picboxBaseStyle.Image = new Bitmap(ImageDir + "\\ViewStyle_Highlight.gif");
					break;
				case StyleEnum.Chiseled:
					optChiseled.Checked = true;
					picboxBaseStyle.Image = new Bitmap(ImageDir + "\\ViewStyle_Chiseled.gif");
					break;
			}
		}
		private void optPlain_CheckedChanged(object sender, System.EventArgs e)
		{
			if (optPlain.Checked == true)
			{
				picboxBaseStyle.Image = new Bitmap(ImageDir + "\\ViewStyle_Plain.gif");
				_currentTheme.ViewStyle = StyleEnum.Plain;

				EnableDisableAlternateColors(false);
				EnableDisableHorizontalDivider(true);
			}
		}

		private void optAlter_CheckedChanged(object sender, System.EventArgs e)
		{
			if (optAlter.Checked == true)
			{
				picboxBaseStyle.Image = new Bitmap(ImageDir + "\\ViewStyle_AlterColor.gif");
				_currentTheme.ViewStyle = StyleEnum.AlterColors;

				EnableDisableAlternateColors(true);
				EnableDisableHorizontalDivider(true);
			}
		}

		private void optHighlightName_CheckedChanged(object sender, System.EventArgs e)
		{
			if (optHighlightName.Checked == true)
			{
				picboxBaseStyle.Image = new Bitmap(ImageDir + "\\ViewStyle_Highlight.gif");
				_currentTheme.ViewStyle = StyleEnum.HighlightName;

				EnableDisableAlternateColors(true);
				EnableDisableHorizontalDivider(true);
			}
		}

		private void optChiseled_CheckedChanged(object sender, System.EventArgs e)
		{
			if (optChiseled.Checked == true)
			{
				picboxBaseStyle.Image = new Bitmap(ImageDir + "\\ViewStyle_Chiseled.gif");
				_currentTheme.ViewStyle = StyleEnum.Chiseled;

				EnableDisableAlternateColors(false);
				EnableDisableHorizontalDivider(false);
			}
		}
		private void EnableDisableAlternateColors(bool enable)
		{
			switch(enable)
			{
				case true:
					//enable alternate background colors on the Background Colors tab
					lblBGColorG4Color2.Enabled = true;
					btnBGColorG4Color2.Enabled = true;
					btnBGColorG4Color2.BackColor = _currentTheme.StoreDetailRowHeaderAlternateBackColor;
					txtBGColorG4Color2.Text = ToRGB(_currentTheme.StoreDetailRowHeaderAlternateBackColor);
					txtBGColorG4Color2.Enabled = true;
					lblBGColorG5G6Color2.Enabled = true;
					btnBGColorG5G6Color2.Enabled = true;
					btnBGColorG5G6Color2.BackColor = _currentTheme.StoreDetailAlternateBackColor;
					txtBGColorG5G6Color2.Text = ToRGB(_currentTheme.StoreDetailAlternateBackColor);
					txtBGColorG5G6Color2.Enabled = true;
					lblBGColorG7Color2.Enabled = true;
					btnBGColorG7Color2.Enabled = true;
					btnBGColorG7Color2.BackColor = _currentTheme.StoreSetRowHeaderAlternateBackColor;
					txtBGColorG7Color2.Text = ToRGB(_currentTheme.StoreSetRowHeaderAlternateBackColor);
					txtBGColorG7Color2.Enabled = true;
					lblBGColorG8G9Color2.Enabled = true;
					btnBGColorG8G9Color2.Enabled = true;
					btnBGColorG8G9Color2.BackColor = _currentTheme.StoreSetAlternateBackColor;
					txtBGColorG8G9Color2.Text = ToRGB(_currentTheme.StoreSetAlternateBackColor);
					txtBGColorG8G9Color2.Enabled = true;
					lblBGColorG10Color2.Enabled = true;
					btnBGColorG10Color2.Enabled = true;
					btnBGColorG10Color2.BackColor = _currentTheme.StoreTotalRowHeaderAlternateBackColor;
					txtBGColorG10Color2.Text = ToRGB(_currentTheme.StoreTotalRowHeaderAlternateBackColor);
					txtBGColorG10Color2.Enabled = true;
					lblBGColorG11G12Color2.Enabled = true;
					btnBGColorG11G12Color2.Enabled = true;
					btnBGColorG11G12Color2.BackColor = _currentTheme.StoreTotalAlternateBackColor;
					txtBGColorG11G12Color2.Text = ToRGB(_currentTheme.StoreTotalAlternateBackColor);
					txtBGColorG11G12Color2.Enabled = true;
					break;
				case false:
					//disable alternate background colors on the Background Colors tab
					lblBGColorG4Color2.Enabled = false;
					btnBGColorG4Color2.Enabled = false;
					btnBGColorG4Color2.BackColor = this.BackColor;
					txtBGColorG4Color2.Text = "";
					txtBGColorG4Color2.Enabled = false;
					lblBGColorG5G6Color2.Enabled = false;
					btnBGColorG5G6Color2.Enabled = false;
					btnBGColorG5G6Color2.BackColor = this.BackColor;
					txtBGColorG5G6Color2.Text = "";
					txtBGColorG5G6Color2.Enabled = false;
					lblBGColorG7Color2.Enabled = false;
					btnBGColorG7Color2.Enabled = false;
					btnBGColorG7Color2.BackColor = this.BackColor;
					txtBGColorG7Color2.Text = "";
					txtBGColorG7Color2.Enabled = false;
					lblBGColorG8G9Color2.Enabled = false;
					btnBGColorG8G9Color2.Enabled = false;
					btnBGColorG8G9Color2.BackColor = this.BackColor;
					txtBGColorG8G9Color2.Text = "";
					txtBGColorG8G9Color2.Enabled = false;
					lblBGColorG10Color2.Enabled = false;
					btnBGColorG10Color2.Enabled = false;
					btnBGColorG10Color2.BackColor = this.BackColor;
					txtBGColorG10Color2.Text = "";
					txtBGColorG10Color2.Enabled = false;
					lblBGColorG11G12Color2.Enabled = false;
					btnBGColorG11G12Color2.Enabled = false;
					btnBGColorG11G12Color2.BackColor = this.BackColor;
					txtBGColorG11G12Color2.Text = "";
					txtBGColorG11G12Color2.Enabled = false;
					break;
			}
		}

		private void EnableDisableHorizontalDivider(bool enable)
		{
			switch (enable)
			{
				case true:
					chkDisplayHorizontalDivider.Checked = _currentTheme.DisplayRowGroupDivider;
					chkDisplayHorizontalDivider.Enabled = true;
					EnableDisableHorDividerColor(true);
					break;
				case false:
					chkDisplayHorizontalDivider.Checked = false;
					chkDisplayHorizontalDivider.Enabled = false;
					EnableDisableHorDividerColor(false);
					break;
			}
		}

		#endregion

		#region Fonts tab

		private void PopulateFonts()
		{
			//Clear existing items in the list view.
			lvFonts.Items.Clear();

			//Main Text

			string[] SubItemsMain = {"Main Text", _currentTheme.DisplayOnlyFont.FontFamily.Name, 
												_currentTheme.DisplayOnlyFont.Size.ToString(CultureInfo.CurrentUICulture), 
												_currentTheme.DisplayOnlyFont.Bold.ToString(CultureInfo.CurrentUICulture),
												_currentTheme.DisplayOnlyFont.Italic.ToString(CultureInfo.CurrentUICulture),
												_currentTheme.DisplayOnlyFont.Underline.ToString(CultureInfo.CurrentUICulture)};

			ListViewItem main = new ListViewItem(SubItemsMain, -1, Color.Black, 
				lvFonts.BackColor, new Font(_currentTheme.DisplayOnlyFont.FontFamily, 8, 
				_currentTheme.DisplayOnlyFont.Style));

			//Editable Cells

			string[] SubItemsEditable = {"Editable Cells", _currentTheme.EditableFont.FontFamily.Name, 
													 _currentTheme.EditableFont.Size.ToString(CultureInfo.CurrentUICulture), 
													 _currentTheme.EditableFont.Bold.ToString(CultureInfo.CurrentUICulture),
													 _currentTheme.EditableFont.Italic.ToString(CultureInfo.CurrentUICulture),
													 _currentTheme.EditableFont.Underline.ToString(CultureInfo.CurrentUICulture)};

			ListViewItem editable = new ListViewItem(SubItemsEditable, -1, Color.Black, 
				lvFonts.BackColor, new Font(_currentTheme.EditableFont.FontFamily, 8,
				_currentTheme.EditableFont.Style));

//			//Corner Grid (g1)
//
//			string[] SubItemsCornerGrid = {"Corner Grid", _fonts.g1.FontFamily.Name,
//														_fonts.g1.Size.ToString(CultureInfo.CurrentUICulture), 
//														_fonts.g1.Bold.ToString(CultureInfo.CurrentUICulture),
//														_fonts.g1.Italic.ToString(CultureInfo.CurrentUICulture),
//														_fonts.g1.Underline.ToString(CultureInfo.CurrentUICulture)};
//
//			ListViewItem g1 = new ListViewItem(SubItemsCornerGrid, -1, Color.Black, 
//				lvFonts.BackColor, new Font(_fonts.g1.FontFamily, 8,
//				_fonts.g1.Style));

			//MerDesc

			string[] SubItemsMerDesc = {"Merchandise Description", _currentTheme.NodeDescriptionFont.FontFamily.Name,
													_currentTheme.NodeDescriptionFont.Size.ToString(CultureInfo.CurrentUICulture), 
													_currentTheme.NodeDescriptionFont.Bold.ToString(CultureInfo.CurrentUICulture),
													_currentTheme.NodeDescriptionFont.Italic.ToString(CultureInfo.CurrentUICulture),
													_currentTheme.NodeDescriptionFont.Underline.ToString(CultureInfo.CurrentUICulture)};

			ListViewItem MerDesc = new ListViewItem(SubItemsMerDesc, -1, Color.Black,
				lvFonts.BackColor, new Font(_currentTheme.NodeDescriptionFont.FontFamily, 8,
				_currentTheme.NodeDescriptionFont.Style));

			//Group Header

			string[] SubItemsGroupHeader = {"Group Header", _currentTheme.ColumnGroupHeaderFont.FontFamily.Name,
														 _currentTheme.ColumnGroupHeaderFont.Size.ToString(CultureInfo.CurrentUICulture), 
														 _currentTheme.ColumnGroupHeaderFont.Bold.ToString(CultureInfo.CurrentUICulture),
														 _currentTheme.ColumnGroupHeaderFont.Italic.ToString(CultureInfo.CurrentUICulture),
														 _currentTheme.ColumnGroupHeaderFont.Underline.ToString(CultureInfo.CurrentUICulture)};

			ListViewItem GroupHeader = new ListViewItem(SubItemsGroupHeader, -1, 
				Color.Black, lvFonts.BackColor, new Font(_currentTheme.ColumnGroupHeaderFont.FontFamily, 8,
				_currentTheme.ColumnGroupHeaderFont.Style));

			//Column Header

			string[] SubItemsColumnHeader = {"Column Headers", _currentTheme.ColumnHeaderFont.FontFamily.Name,
														  _currentTheme.ColumnHeaderFont.Size.ToString(CultureInfo.CurrentUICulture), 
														  _currentTheme.ColumnHeaderFont.Bold.ToString(CultureInfo.CurrentUICulture),
														  _currentTheme.ColumnHeaderFont.Italic.ToString(CultureInfo.CurrentUICulture),
														  _currentTheme.ColumnHeaderFont.Underline.ToString(CultureInfo.CurrentUICulture)};

			ListViewItem ColumnHeader = new ListViewItem(SubItemsColumnHeader, -1, 
				Color.Black, lvFonts.BackColor, new Font(_currentTheme.ColumnHeaderFont.FontFamily, 8,
				_currentTheme.ColumnHeaderFont.Style));

			//Row Header

			string[] SubItemsRowHeader = {"Row Header", _currentTheme.RowHeaderFont.FontFamily.Name,
													  _currentTheme.RowHeaderFont.Size.ToString(CultureInfo.CurrentUICulture), 
													  _currentTheme.RowHeaderFont.Bold.ToString(CultureInfo.CurrentUICulture),
													  _currentTheme.RowHeaderFont.Italic.ToString(CultureInfo.CurrentUICulture),
													  _currentTheme.RowHeaderFont.Underline.ToString(CultureInfo.CurrentUICulture)};

			ListViewItem RowHeader = new ListViewItem(SubItemsRowHeader, -1, 
				Color.Black, lvFonts.BackColor, new Font(_currentTheme.RowHeaderFont.FontFamily, 8,
				_currentTheme.RowHeaderFont.Style));

			//Ineligible stores

			string[] SubItemsIneligStores = {"Ineligible Stores", _currentTheme.IneligibleStoreFont.FontFamily.Name,
											 _currentTheme.IneligibleStoreFont.Size.ToString(CultureInfo.CurrentUICulture), 
											 _currentTheme.IneligibleStoreFont.Bold.ToString(CultureInfo.CurrentUICulture),
											 _currentTheme.IneligibleStoreFont.Italic.ToString(CultureInfo.CurrentUICulture),
											 _currentTheme.IneligibleStoreFont.Underline.ToString(CultureInfo.CurrentUICulture)};

			ListViewItem IneligStores = new ListViewItem(SubItemsIneligStores, -1, 
				Color.Black, lvFonts.BackColor, new Font(_currentTheme.IneligibleStoreFont.FontFamily, 8,
				_currentTheme.IneligibleStoreFont.Style));

			//Locks

			string[] SubItemsLock = {"Lock", _currentTheme.LockedFont.FontFamily.Name,
												_currentTheme.LockedFont.Size.ToString(CultureInfo.CurrentUICulture), 
												_currentTheme.LockedFont.Bold.ToString(CultureInfo.CurrentUICulture),
												_currentTheme.LockedFont.Italic.ToString(CultureInfo.CurrentUICulture),
												_currentTheme.LockedFont.Underline.ToString(CultureInfo.CurrentUICulture)};

			ListViewItem Lock = new ListViewItem(SubItemsLock, -1, 
				Color.Black, lvFonts.BackColor, new Font(_currentTheme.LockedFont.FontFamily, 8,
				_currentTheme.LockedFont.Style));

			lvFonts.Items.Add(main);
			lvFonts.Items.Add(editable);
//			lvFonts.Items.Add(g1);
			lvFonts.Items.Add(MerDesc);
			lvFonts.Items.Add(GroupHeader);
			lvFonts.Items.Add(ColumnHeader);
			lvFonts.Items.Add(RowHeader);
			lvFonts.Items.Add(IneligStores);
			lvFonts.Items.Add(Lock);

			//Text Effects:
			cboTextEffectMerDesc.SelectedIndex = TextEffectToIndex(_currentTheme.NodeDescriptionTextEffects.ToString());
			cboTextEffectGroupHeader.SelectedIndex = TextEffectToIndex(_currentTheme.ColumnGroupHeaderTextEffects.ToString());
			cboTextEffectColHeader.SelectedIndex = TextEffectToIndex(_currentTheme.ColumnHeaderTextEffects.ToString());
		}
		private void cboTextEffectMerDesc_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			_currentTheme.NodeDescriptionTextEffects = GetTextEffect(cboTextEffectMerDesc.Text);
		}

		private void cboTextEffectGroupHeader_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			_currentTheme.ColumnGroupHeaderTextEffects = GetTextEffect(cboTextEffectGroupHeader.Text);
		}

		private void cboTextEffectColHeader_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			_currentTheme.ColumnHeaderTextEffects = GetTextEffect(cboTextEffectColHeader.Text);
		}
		private int TextEffectToIndex(string EffectName)
		{
			switch (EffectName)
			{
				case "Flat":
					return 0;
				case "Inset":
					return 1;
				case "Raised":
					return 2;
				default:
					return -1;
			}
		}
		private TextEffectEnum GetTextEffect(string EffectText)
		{
			switch (EffectText)
			{
				case "Flat":
					return TextEffectEnum.Flat;
				case "Inset":
					return TextEffectEnum.Inset;
				case "Raised":
					return TextEffectEnum.Raised;
				default:
					return TextEffectEnum.Flat;
			}
		}
		private void lvFonts_ItemActivate(object sender, System.EventArgs e)
		{
			FontDialog f = new FontDialog();

			switch (lvFonts.SelectedIndices[0])
			{
				case 0:	//Main text
					f.ShowColor = false;
					f.Font = _currentTheme.DisplayOnlyFont;
					if (f.ShowDialog() == DialogResult.OK)
						_currentTheme.DisplayOnlyFont = f.Font;
					break;
				case 1:	//Editable Cells
					f.ShowColor = false;
					f.Font = _currentTheme.EditableFont;
					if (f.ShowDialog() == DialogResult.OK)
						_currentTheme.EditableFont = f.Font;
					break;
//				case 2:	//Corner Grid (g1)
//					f.ShowColor = true;
//					f.Font = _fonts.g1;
//					f.Color = _foreColors.g1;
//					if (f.ShowDialog() == DialogResult.OK)
//					{
//						_fonts.g1 = f.Font;
//						_foreColors.g1 = f.Color;
//					}
//					break;
//Begin Track #4091 - JScott - Theme fonts changing wrong value
//				case 3:	//Merchandise Description
				case 2:	//Merchandise Description
//End Track #4091 - JScott - Theme fonts changing wrong value
					f.ShowColor = true;
					f.Font = _currentTheme.NodeDescriptionFont;
					f.Color = _currentTheme.NodeDescriptionForeColor;
					if (f.ShowDialog() == DialogResult.OK)
					{
						_currentTheme.NodeDescriptionFont = f.Font;
						_currentTheme.NodeDescriptionForeColor = f.Color;
					}
					break;
//Begin Track #4091 - JScott - Theme fonts changing wrong value
//				case 4:	//Group Header
				case 3:	//Group Header
//End Track #4091 - JScott - Theme fonts changing wrong value
					f.ShowColor = true;
					f.Font = _currentTheme.ColumnGroupHeaderFont;
					f.Color = _currentTheme.ColumnGroupHeaderForeColor;
					if (f.ShowDialog() == DialogResult.OK)
					{
						_currentTheme.ColumnGroupHeaderFont = f.Font;
						_currentTheme.ColumnGroupHeaderForeColor = f.Color;
					}
					break;
//Begin Track #4091 - JScott - Theme fonts changing wrong value
//				case 5:	//Column Header
				case 4:	//Column Header
//End Track #4091 - JScott - Theme fonts changing wrong value
					f.ShowColor = true;
					f.Font = _currentTheme.ColumnHeaderFont;
					f.Color = _currentTheme.ColumnHeaderForeColor;
					if (f.ShowDialog() == DialogResult.OK)
					{
						_currentTheme.ColumnHeaderFont = f.Font;
						_currentTheme.ColumnHeaderForeColor = f.Color;
					}
					break;
//Begin Track #4091 - JScott - Theme fonts changing wrong value
//				case 6:	//Row Headers
				case 5:	//Row Headers
//End Track #4091 - JScott - Theme fonts changing wrong value
					f.ShowColor = false;
					f.Font = _currentTheme.RowHeaderFont;
					if (f.ShowDialog() == DialogResult.OK)
						_currentTheme.RowHeaderFont = f.Font;
					break;
//Begin Track #4091 - JScott - Theme fonts changing wrong value
//				case 7:	//Ineligible Store
				case 6:	//Ineligible Store
//End Track #4091 - JScott - Theme fonts changing wrong value
					f.ShowColor = false;
					f.Font = _currentTheme.IneligibleStoreFont;
					if (f.ShowDialog() == DialogResult.OK)
						_currentTheme.IneligibleStoreFont = f.Font;
					break;
//Begin Track #4091 - JScott - Theme fonts changing wrong value
//				case 8:	//Lock
				case 7:	//Lock
//End Track #4091 - JScott - Theme fonts changing wrong value
					f.ShowColor = false;
					f.Font = _currentTheme.LockedFont;
					if (f.ShowDialog() == DialogResult.OK)
						_currentTheme.LockedFont = f.Font;
					break;
			}

			//Refresh the list view.
			PopulateFonts();
		}

		private void lvFonts_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		#endregion

		#region ForeColors tab

		private void PopulateForeColors()
		{
//			btnForeColorG1.BackColor = _foreColors.g1;
			btnForeColorMerDesc.BackColor = _currentTheme.NodeDescriptionForeColor;
			btnForeColorGroupHeader.BackColor = _currentTheme.ColumnGroupHeaderForeColor;
			btnForeColorColHeader.BackColor = _currentTheme.ColumnHeaderForeColor;
			btnForeColorNegative.BackColor = _currentTheme.NegativeForeColor;
			btnForeColorG4.BackColor = _currentTheme.StoreDetailRowHeaderForeColor;
			btnForeColorG5G6.BackColor = _currentTheme.StoreDetailForeColor;
			btnForeColorG7.BackColor = _currentTheme.StoreSetRowHeaderForeColor;
			btnForeColorG8G9.BackColor = _currentTheme.StoreSetForeColor;
			btnForeColorG10.BackColor = _currentTheme.StoreTotalRowHeaderForeColor;
			btnForeColorG11G12.BackColor = _currentTheme.StoreTotalForeColor;

//			txtForeColorG1.Text = ToRGB(_foreColors.g1);
			txtForeColorMerDesc.Text = ToRGB(_currentTheme.NodeDescriptionForeColor);
			txtForeColorGroupHeader.Text = ToRGB(_currentTheme.ColumnGroupHeaderForeColor);
			txtForeColorColHeader.Text = ToRGB(_currentTheme.ColumnHeaderForeColor);
			txtForeColorNegative.Text = ToRGB(_currentTheme.NegativeForeColor);
			txtForeColorG4.Text = ToRGB(_currentTheme.StoreDetailRowHeaderForeColor);
			txtForeColorG5G6.Text = ToRGB(_currentTheme.StoreDetailForeColor);
			txtForeColorG7.Text = ToRGB(_currentTheme.StoreSetRowHeaderForeColor);
			txtForeColorG8G9.Text = ToRGB(_currentTheme.StoreSetForeColor);
			txtForeColorG10.Text = ToRGB(_currentTheme.StoreTotalRowHeaderForeColor);
			txtForeColorG11G12.Text = ToRGB(_currentTheme.StoreTotalForeColor);
		}
		
		
		private void btnForeColorG1_Click(object sender, System.EventArgs e)
		{
//			ColorDialog c = new ColorDialog();
//			c.Color = _foreColors.g1;
//			if (c.ShowDialog() == DialogResult.OK)
//			{
//				_foreColors.g1 = c.Color;
//				btnForeColorG1.BackColor = _foreColors.g1;
//				txtForeColorG1.Text = ToRGB(_foreColors.g1);
//			}
		}

		private void txtForeColorG1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
//			try
//			{
//				string[] RGBarray = txtForeColorG1.Text.Split(new char[] {','});
//				if (RGBarray.Length != 3)
//				{
//					throw new ApplicationException();
//				}
//				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
//				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
//				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);
//
//				_foreColors.g1 = Color.FromArgb(R, G, B);
//				btnForeColorG1.BackColor = _foreColors.g1;
//			}
//			catch
//			{
//				MessageBox.Show("Invalid Value!", "Error");
//				txtForeColorG1.Focus();
//				txtForeColorG1.SelectAll();
//			}
		}

		private void txtForeColorG1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
//			if(e.KeyChar == (char)13)
//			{
//				txtForeColorG1_Validating(this, new System.ComponentModel.CancelEventArgs());
//				e.Handled = true;
//			}
		}

		
		private void btnForeColorMerDesc_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.NodeDescriptionForeColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.NodeDescriptionForeColor = c.Color;
				btnForeColorMerDesc.BackColor = _currentTheme.NodeDescriptionForeColor;
				txtForeColorMerDesc.Text = ToRGB(_currentTheme.NodeDescriptionForeColor);
			}
		}

		private void txtForeColorMerDesc_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtForeColorMerDesc.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.NodeDescriptionForeColor = Color.FromArgb(R, G, B);
				btnForeColorMerDesc.BackColor = _currentTheme.NodeDescriptionForeColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtForeColorMerDesc.Focus();
				txtForeColorMerDesc.SelectAll();
			}
		}

		private void txtForeColorMerDesc_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtForeColorMerDesc_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnForeColorGroupHeader_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.ColumnGroupHeaderForeColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.ColumnGroupHeaderForeColor = c.Color;
				btnForeColorGroupHeader.BackColor = _currentTheme.ColumnGroupHeaderForeColor;
				txtForeColorGroupHeader.Text = ToRGB(_currentTheme.ColumnGroupHeaderForeColor);
			}
		}

		private void txtForeColorGroupHeader_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtForeColorGroupHeader.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.ColumnGroupHeaderForeColor = Color.FromArgb(R, G, B);
				btnForeColorGroupHeader.BackColor = _currentTheme.ColumnGroupHeaderForeColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtForeColorGroupHeader.Focus();
				txtForeColorGroupHeader.SelectAll();
			}
		}

		private void txtForeColorGroupHeader_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtForeColorGroupHeader_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnForeColorColHeader_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.ColumnHeaderForeColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.ColumnHeaderForeColor = c.Color;
				btnForeColorColHeader.BackColor = _currentTheme.ColumnHeaderForeColor;
				txtForeColorColHeader.Text = ToRGB(_currentTheme.ColumnHeaderForeColor);
			}
		}

		private void txtForeColorColHeader_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtForeColorColHeader.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.ColumnHeaderForeColor = Color.FromArgb(R, G, B);
				btnForeColorColHeader.BackColor = _currentTheme.ColumnHeaderForeColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtForeColorColHeader.Focus();
				txtForeColorColHeader.SelectAll();
			}
		}

		private void txtForeColorColHeader_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtForeColorColHeader_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnForeColorNegative_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.NegativeForeColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.NegativeForeColor = c.Color;
				btnForeColorNegative.BackColor = _currentTheme.NegativeForeColor;
				txtForeColorNegative.Text = ToRGB(_currentTheme.NegativeForeColor);
			}
		}

		private void txtForeColorNegative_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtForeColorNegative.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.NegativeForeColor = Color.FromArgb(R, G, B);
				btnForeColorNegative.BackColor = _currentTheme.NegativeForeColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtForeColorNegative.Focus();
				txtForeColorNegative.SelectAll();
			}
		}

		private void txtForeColorNegative_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtForeColorNegative_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnForeColorG4_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreDetailRowHeaderForeColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreDetailRowHeaderForeColor = c.Color;
				btnForeColorG4.BackColor = _currentTheme.StoreDetailRowHeaderForeColor;
				txtForeColorG4.Text = ToRGB(_currentTheme.StoreDetailRowHeaderForeColor);
			}
		}

		private void txtForeColorG4_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtForeColorG4.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreDetailRowHeaderForeColor = Color.FromArgb(R, G, B);
				btnForeColorG4.BackColor = _currentTheme.StoreDetailRowHeaderForeColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtForeColorG4.Focus();
				txtForeColorG4.SelectAll();
			}
		}

		private void txtForeColorG4_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtForeColorG4_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnForeColorG5G6_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreDetailForeColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreDetailForeColor = c.Color;
				btnForeColorG5G6.BackColor = _currentTheme.StoreDetailForeColor;
				txtForeColorG5G6.Text = ToRGB(_currentTheme.StoreDetailForeColor);
			}
		}

		private void txtForeColorG5G6_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtForeColorG5G6.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreDetailForeColor = Color.FromArgb(R, G, B);
				btnForeColorG5G6.BackColor = _currentTheme.StoreDetailForeColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtForeColorG5G6.Focus();
				txtForeColorG5G6.SelectAll();
			}
		}

		private void txtForeColorG5G6_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtForeColorG5G6_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnForeColorG7_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreSetRowHeaderForeColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreSetRowHeaderForeColor = c.Color;
				btnForeColorG7.BackColor = _currentTheme.StoreSetRowHeaderForeColor;
				txtForeColorG7.Text = ToRGB(_currentTheme.StoreSetRowHeaderForeColor);
			}
		}

		private void txtForeColorG7_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtForeColorG7.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreSetRowHeaderForeColor = Color.FromArgb(R, G, B);
				btnForeColorG7.BackColor = _currentTheme.StoreSetRowHeaderForeColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtForeColorG7.Focus();
				txtForeColorG7.SelectAll();
			}
		}

		private void txtForeColorG7_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtForeColorG7_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnForeColorG8G9_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreSetForeColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreSetForeColor = c.Color;
				btnForeColorG8G9.BackColor = _currentTheme.StoreSetForeColor;
				txtForeColorG8G9.Text = ToRGB(_currentTheme.StoreSetForeColor);
			}
		}

		private void txtForeColorG8G9_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtForeColorG8G9.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreSetForeColor = Color.FromArgb(R, G, B);
				btnForeColorG8G9.BackColor = _currentTheme.StoreSetForeColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtForeColorG8G9.Focus();
				txtForeColorG8G9.SelectAll();
			}
		}

		private void txtForeColorG8G9_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtForeColorG8G9_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnForeColorG10_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreTotalRowHeaderForeColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreTotalRowHeaderForeColor = c.Color;
				btnForeColorG10.BackColor = _currentTheme.StoreTotalRowHeaderForeColor;
				txtForeColorG10.Text = ToRGB(_currentTheme.StoreTotalRowHeaderForeColor);
			}
		}

		private void txtForeColorG10_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtForeColorG10.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreTotalRowHeaderForeColor = Color.FromArgb(R, G, B);
				btnForeColorG10.BackColor = _currentTheme.StoreTotalRowHeaderForeColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtForeColorG10.Focus();
				txtForeColorG10.SelectAll();
			}
		}

		private void txtForeColorG10_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtForeColorG10_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnForeColorG11G12_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreTotalForeColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreTotalForeColor = c.Color;
				btnForeColorG11G12.BackColor = _currentTheme.StoreTotalForeColor;
				txtForeColorG11G12.Text = ToRGB(_currentTheme.StoreTotalForeColor);
			}
		}

		private void txtForeColorG11G12_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtForeColorG11G12.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreTotalForeColor = Color.FromArgb(R, G, B);
				btnForeColorG11G12.BackColor = _currentTheme.StoreTotalForeColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtForeColorG11G12.Focus();
				txtForeColorG11G12.SelectAll();
			}
		}

		private void txtForeColorG11G12_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtForeColorG11G12_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		#endregion

		#region Background Colors tab

		private void PopulateBGColors()
		{
//			btnBGColorG1.BackColor = _bgColors.g1;
			btnBGColorMerDesc.BackColor = _currentTheme.NodeDescriptionBackColor;
			btnBGColorGroupHeader.BackColor = _currentTheme.ColumnGroupHeaderBackColor;
			btnBGColorColHeader.BackColor = _currentTheme.ColumnHeaderBackColor;
			btnBGColorG4Color1.BackColor = _currentTheme.StoreDetailRowHeaderBackColor;
			btnBGColorG4Color2.BackColor = _currentTheme.StoreDetailRowHeaderAlternateBackColor;
			btnBGColorG5G6Color1.BackColor = _currentTheme.StoreDetailBackColor;
			btnBGColorG5G6Color2.BackColor = _currentTheme.StoreDetailAlternateBackColor;
			btnBGColorG7Color1.BackColor = _currentTheme.StoreSetRowHeaderBackColor;
			btnBGColorG7Color2.BackColor = _currentTheme.StoreSetRowHeaderAlternateBackColor;
			btnBGColorG8G9Color1.BackColor = _currentTheme.StoreSetBackColor;
			btnBGColorG8G9Color2.BackColor = _currentTheme.StoreSetAlternateBackColor;
			btnBGColorG10Color1.BackColor = _currentTheme.StoreTotalRowHeaderBackColor;
			btnBGColorG10Color2.BackColor = _currentTheme.StoreTotalRowHeaderAlternateBackColor;
			btnBGColorG11G12Color1.BackColor = _currentTheme.StoreTotalBackColor;
			btnBGColorG11G12Color2.BackColor = _currentTheme.StoreTotalAlternateBackColor;

//			txtBGColorG1.Text = ToRGB(_bgColors.g1);
			txtBGColorMerDesc.Text = ToRGB(_currentTheme.NodeDescriptionBackColor);
			txtBGColorGroupHeader.Text = ToRGB(_currentTheme.ColumnGroupHeaderBackColor);
			txtBGColorColHeader.Text = ToRGB(_currentTheme.ColumnHeaderBackColor);
			txtBGColorG4Color1.Text = ToRGB(_currentTheme.StoreDetailRowHeaderBackColor);
			txtBGColorG4Color2.Text = ToRGB(_currentTheme.StoreDetailRowHeaderAlternateBackColor);
			txtBGColorG5G6Color1.Text = ToRGB(_currentTheme.StoreDetailBackColor);
			txtBGColorG5G6Color2.Text = ToRGB(_currentTheme.StoreDetailAlternateBackColor);
			txtBGColorG7Color1.Text = ToRGB(_currentTheme.StoreSetRowHeaderBackColor);
			txtBGColorG7Color2.Text = ToRGB(_currentTheme.StoreSetRowHeaderAlternateBackColor);
			txtBGColorG8G9Color1.Text = ToRGB(_currentTheme.StoreSetBackColor);
			txtBGColorG8G9Color2.Text = ToRGB(_currentTheme.StoreSetAlternateBackColor);
			txtBGColorG10Color1.Text = ToRGB(_currentTheme.StoreTotalRowHeaderBackColor);
			txtBGColorG10Color2.Text = ToRGB(_currentTheme.StoreTotalRowHeaderAlternateBackColor);
			txtBGColorG11G12Color1.Text = ToRGB(_currentTheme.StoreTotalBackColor);
			txtBGColorG11G12Color2.Text = ToRGB(_currentTheme.StoreTotalAlternateBackColor);
		}
		
		private void btnBGColorG1_Click(object sender, System.EventArgs e)
		{
//			ColorDialog c = new ColorDialog();
//			c.Color = _bgColors.g1;
//			if (c.ShowDialog() == DialogResult.OK)
//			{
//				_bgColors.g1 = c.Color;
//				btnBGColorG1.BackColor = _bgColors.g1;
//				txtBGColorG1.Text = ToRGB(_bgColors.g1);
//			}
		}

		private void txtBGColorG1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
//			try
//			{
//				string[] RGBarray = txtBGColorG1.Text.Split(new char[] {','});
//				if (RGBarray.Length != 3)
//				{
//					throw new ApplicationException();
//				}
//				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
//				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
//				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);
//
//				_bgColors.g1 = Color.FromArgb(R, G, B);
//				btnBGColorG1.BackColor = _bgColors.g1;
//			}
//			catch
//			{
//				MessageBox.Show("Invalid Value!", "Error");
//				txtBGColorG1.Focus();
//				txtBGColorG1.SelectAll();
//			}
		}

		private void txtBGColorG1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
//			if(e.KeyChar == (char)13)
//			{
//				txtBGColorG1_Validating(this, new System.ComponentModel.CancelEventArgs());
//				e.Handled = true;
//			}
		}

		
		private void btnBGColorMerDesc_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.NodeDescriptionBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.NodeDescriptionBackColor = c.Color;
				btnBGColorMerDesc.BackColor = _currentTheme.NodeDescriptionBackColor;
				txtBGColorMerDesc.Text = ToRGB(_currentTheme.NodeDescriptionBackColor);
			}
		}

		private void txtBGColorMerDesc_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorMerDesc.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.NodeDescriptionBackColor = Color.FromArgb(R, G, B);
				btnBGColorMerDesc.BackColor = _currentTheme.NodeDescriptionBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorMerDesc.Focus();
				txtBGColorMerDesc.SelectAll();
			}
		}

		private void txtBGColorMerDesc_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorMerDesc_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorGroupHeader_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.ColumnGroupHeaderBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.ColumnGroupHeaderBackColor = c.Color;
				btnBGColorGroupHeader.BackColor = _currentTheme.ColumnGroupHeaderBackColor;
				txtBGColorGroupHeader.Text = ToRGB(_currentTheme.ColumnGroupHeaderBackColor);
			}
		}

		private void txtBGColorGroupHeader_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorGroupHeader.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.ColumnGroupHeaderBackColor = Color.FromArgb(R, G, B);
				btnBGColorGroupHeader.BackColor = _currentTheme.ColumnGroupHeaderBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorGroupHeader.Focus();
				txtBGColorGroupHeader.SelectAll();
			}
		}

		private void txtBGColorGroupHeader_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorGroupHeader_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorColHeader_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.ColumnHeaderBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.ColumnHeaderBackColor = c.Color;
				btnBGColorColHeader.BackColor = _currentTheme.ColumnHeaderBackColor;
				txtBGColorColHeader.Text = ToRGB(_currentTheme.ColumnHeaderBackColor);
			}
		}

		private void txtBGColorColHeader_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorColHeader.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.ColumnHeaderBackColor = Color.FromArgb(R, G, B);
				btnBGColorColHeader.BackColor = _currentTheme.ColumnHeaderBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorColHeader.Focus();
				txtBGColorColHeader.SelectAll();
			}
		}

		private void txtBGColorColHeader_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorColHeader_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG4Color1_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreDetailRowHeaderBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreDetailRowHeaderBackColor = c.Color;
				btnBGColorG4Color1.BackColor = _currentTheme.StoreDetailRowHeaderBackColor;
				txtBGColorG4Color1.Text = ToRGB(_currentTheme.StoreDetailRowHeaderBackColor);
			}
		}

		private void txtBGColorG4Color1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG4Color1.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreDetailRowHeaderBackColor = Color.FromArgb(R, G, B);
				btnBGColorG4Color1.BackColor = _currentTheme.StoreDetailRowHeaderBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG4Color1.Focus();
				txtBGColorG4Color1.SelectAll();
			}
		}

		private void txtBGColorG4Color1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG4Color1_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG4Color2_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreDetailRowHeaderAlternateBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreDetailRowHeaderAlternateBackColor = c.Color;
				btnBGColorG4Color2.BackColor = _currentTheme.StoreDetailRowHeaderAlternateBackColor;
				txtBGColorG4Color2.Text = ToRGB(_currentTheme.StoreDetailRowHeaderAlternateBackColor);
			}
		}

		private void txtBGColorG4Color2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG4Color2.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreDetailRowHeaderAlternateBackColor = Color.FromArgb(R, G, B);
				btnBGColorG4Color2.BackColor = _currentTheme.StoreDetailRowHeaderAlternateBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG4Color2.Focus();
				txtBGColorG4Color2.SelectAll();
			}
		}

		private void txtBGColorG4Color2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG4Color2_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG7Color1_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreSetRowHeaderBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreSetRowHeaderBackColor = c.Color;
				btnBGColorG7Color1.BackColor = _currentTheme.StoreSetRowHeaderBackColor;
				txtBGColorG7Color1.Text = ToRGB(_currentTheme.StoreSetRowHeaderBackColor);
			}
		}

		private void txtBGColorG7Color1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG7Color1.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreSetRowHeaderBackColor = Color.FromArgb(R, G, B);
				btnBGColorG7Color1.BackColor = _currentTheme.StoreSetRowHeaderBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG7Color1.Focus();
				txtBGColorG7Color1.SelectAll();
			}
		}

		private void txtBGColorG7Color1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG7Color1_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG7Color2_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreSetRowHeaderAlternateBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreSetRowHeaderAlternateBackColor = c.Color;
				btnBGColorG7Color2.BackColor = _currentTheme.StoreSetRowHeaderAlternateBackColor;
				txtBGColorG7Color2.Text = ToRGB(_currentTheme.StoreSetRowHeaderAlternateBackColor);
			}
		}

		private void txtBGColorG7Color2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG7Color2.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreSetRowHeaderAlternateBackColor = Color.FromArgb(R, G, B);
				btnBGColorG7Color2.BackColor = _currentTheme.StoreSetRowHeaderAlternateBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG7Color2.Focus();
				txtBGColorG7Color2.SelectAll();
			}
		}

		private void txtBGColorG7Color2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG7Color2_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG10Color1_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreTotalRowHeaderBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreTotalRowHeaderBackColor = c.Color;
				btnBGColorG10Color1.BackColor = _currentTheme.StoreTotalRowHeaderBackColor;
				txtBGColorG10Color1.Text = ToRGB(_currentTheme.StoreTotalRowHeaderBackColor);
			}
		}

		private void txtBGColorG10Color1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG10Color1.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreTotalRowHeaderBackColor = Color.FromArgb(R, G, B);
				btnBGColorG10Color1.BackColor = _currentTheme.StoreTotalRowHeaderBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG10Color1.Focus();
				txtBGColorG10Color1.SelectAll();
			}
		}

		private void txtBGColorG10Color1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG10Color1_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG10Color2_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreTotalRowHeaderAlternateBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreTotalRowHeaderAlternateBackColor = c.Color;
				btnBGColorG10Color2.BackColor = _currentTheme.StoreTotalRowHeaderAlternateBackColor;
				txtBGColorG10Color2.Text = ToRGB(_currentTheme.StoreTotalRowHeaderAlternateBackColor);
			}
		}

		private void txtBGColorG10Color2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG10Color2.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreTotalRowHeaderAlternateBackColor = Color.FromArgb(R, G, B);
				btnBGColorG10Color2.BackColor = _currentTheme.StoreTotalRowHeaderAlternateBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG10Color2.Focus();
				txtBGColorG10Color2.SelectAll();
			}
		}

		private void txtBGColorG10Color2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG10Color2_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG5G6Color1_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreDetailBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreDetailBackColor = c.Color;
				btnBGColorG5G6Color1.BackColor = _currentTheme.StoreDetailBackColor;
				txtBGColorG5G6Color1.Text = ToRGB(_currentTheme.StoreDetailBackColor);
			}
		}

		private void txtBGColorG5G6Color1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG5G6Color1.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreDetailBackColor = Color.FromArgb(R, G, B);
				btnBGColorG5G6Color1.BackColor = _currentTheme.StoreDetailBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG5G6Color1.Focus();
				txtBGColorG5G6Color1.SelectAll();
			}
		}

		private void txtBGColorG5G6Color1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG5G6Color1_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG5G6Color2_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreDetailAlternateBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreDetailAlternateBackColor = c.Color;
				btnBGColorG5G6Color2.BackColor = _currentTheme.StoreDetailAlternateBackColor;
				txtBGColorG5G6Color2.Text = ToRGB(_currentTheme.StoreDetailAlternateBackColor);
			}
		}

		private void txtBGColorG5G6Color2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG5G6Color2.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreDetailAlternateBackColor = Color.FromArgb(R, G, B);
				btnBGColorG5G6Color2.BackColor = _currentTheme.StoreDetailAlternateBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG5G6Color2.Focus();
				txtBGColorG5G6Color2.SelectAll();
			}
		}

		private void txtBGColorG5G6Color2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG5G6Color2_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG8G9Color1_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreSetBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreSetBackColor = c.Color;
				btnBGColorG8G9Color1.BackColor = _currentTheme.StoreSetBackColor;
				txtBGColorG8G9Color1.Text = ToRGB(_currentTheme.StoreSetBackColor);
			}
		}

		private void txtBGColorG8G9Color1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG8G9Color1.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreSetBackColor = Color.FromArgb(R, G, B);
				btnBGColorG8G9Color1.BackColor = _currentTheme.StoreSetBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG8G9Color1.Focus();
				txtBGColorG8G9Color1.SelectAll();
			}
		}

		private void txtBGColorG8G9Color1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG8G9Color1_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG8G9Color2_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreSetAlternateBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreSetAlternateBackColor = c.Color;
				btnBGColorG8G9Color2.BackColor = _currentTheme.StoreSetAlternateBackColor;
				txtBGColorG8G9Color2.Text = ToRGB(_currentTheme.StoreSetAlternateBackColor);
			}
		}

		private void txtBGColorG8G9Color2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG8G9Color2.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreSetAlternateBackColor = Color.FromArgb(R, G, B);
				btnBGColorG8G9Color2.BackColor = _currentTheme.StoreSetAlternateBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG8G9Color2.Focus();
				txtBGColorG8G9Color2.SelectAll();
			}
		}

		private void txtBGColorG8G9Color2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG8G9Color2_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG11G12Color1_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreTotalBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreTotalBackColor = c.Color;
				btnBGColorG11G12Color1.BackColor = _currentTheme.StoreTotalBackColor;
				txtBGColorG11G12Color1.Text = ToRGB(_currentTheme.StoreTotalBackColor);
			}
		}

		private void txtBGColorG11G12Color1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG11G12Color1.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreTotalBackColor = Color.FromArgb(R, G, B);
				btnBGColorG11G12Color1.BackColor = _currentTheme.StoreTotalBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG11G12Color1.Focus();
				txtBGColorG11G12Color1.SelectAll();
			}
		}

		private void txtBGColorG11G12Color1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG11G12Color1_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		
		private void btnBGColorG11G12Color2_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.StoreTotalAlternateBackColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.StoreTotalAlternateBackColor = c.Color;
				btnBGColorG11G12Color2.BackColor = _currentTheme.StoreTotalAlternateBackColor;
				txtBGColorG11G12Color2.Text = ToRGB(_currentTheme.StoreTotalAlternateBackColor);
			}
		}

		private void txtBGColorG11G12Color2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtBGColorG11G12Color2.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.StoreTotalAlternateBackColor = Color.FromArgb(R, G, B);
				btnBGColorG11G12Color2.BackColor = _currentTheme.StoreTotalAlternateBackColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtBGColorG11G12Color2.Focus();
				txtBGColorG11G12Color2.SelectAll();
			}
		}

		private void txtBGColorG11G12Color2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtBGColorG11G12Color2_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		#endregion

		#region Border Options tab

		private void PopulateBorderOptions()
		{
			cboCellBorderStyle.Text  = _currentTheme.CellBorderStyle.ToString();
			btnCellBorderColor.BackColor = _currentTheme.CellBorderColor;
			txtCellBorderColor.Text = ToRGB(_currentTheme.CellBorderColor);

			chkDisplayVerticalDivider.Checked = _currentTheme.DisplayColumnGroupDivider;
			chkDisplayHorizontalDivider.Checked = _currentTheme.DisplayRowGroupDivider;
			cboDividerWidth.Text = Convert.ToString(_currentTheme.DividerWidth, CultureInfo.CurrentUICulture);

			btnDividerColorVertical.BackColor = _currentTheme.ColumnGroupDividerBrushColor;
			txtDividerColorVertical.Text = ToRGB(_currentTheme.ColumnGroupDividerBrushColor);

			btnDividerColorHorRow.BackColor = _currentTheme.RowGroupRowHeaderDividerBrushColor;
			txtDividerColorHorRow.Text = ToRGB(_currentTheme.RowGroupRowHeaderDividerBrushColor);

			btnDividerColorHorData.BackColor = _currentTheme.RowGroupDividerBrushColor;
			txtDividerColorHorData.Text = ToRGB(_currentTheme.RowGroupDividerBrushColor);
		}

		private void cboCellBorderStyle_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			switch (cboCellBorderStyle.Text)
			{
				case "Dotted":
					_currentTheme.CellBorderStyle = BorderStyleEnum.Dotted;
					EnableDisableCellBorderColor(true);
					break;
				case "Double":
					_currentTheme.CellBorderStyle = BorderStyleEnum.Double;
					EnableDisableCellBorderColor(true);
					break;
				case "Fillet":
					_currentTheme.CellBorderStyle = BorderStyleEnum.Fillet;
					EnableDisableCellBorderColor(false);
					break;
				case "Flat":
					_currentTheme.CellBorderStyle = BorderStyleEnum.Flat;
					EnableDisableCellBorderColor(true);
					break;
				case "Groove":
					_currentTheme.CellBorderStyle = BorderStyleEnum.Groove;
					EnableDisableCellBorderColor(false);
					break;
				case "Inset":
					_currentTheme.CellBorderStyle = BorderStyleEnum.Inset;
					EnableDisableCellBorderColor(false);
					break;
				case "None":
					_currentTheme.CellBorderStyle = BorderStyleEnum.None;
					EnableDisableCellBorderColor(false);
					break;
				case "Raised":
					_currentTheme.CellBorderStyle = BorderStyleEnum.Raised;
					EnableDisableCellBorderColor(false);
					break;
			}
		}
		private void EnableDisableCellBorderColor(bool enable)
		{
			switch (enable)
			{
				case true:
					lblCellBorderColor.Enabled = true;
					btnCellBorderColor.BackColor = _currentTheme.CellBorderColor;
					btnCellBorderColor.Enabled = true;
					txtCellBorderColor.Enabled = true;
					txtCellBorderColor.Text = ToRGB(_currentTheme.CellBorderColor);
					break;
				case false:
					lblCellBorderColor.Enabled = false;
					btnCellBorderColor.BackColor = this.BackColor;
					btnCellBorderColor.Enabled = false;
					txtCellBorderColor.Enabled = false;
					txtCellBorderColor.Text = "";
					break;
			}
		}

		private void btnCellBorderColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.CellBorderColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.CellBorderColor = c.Color;
				btnCellBorderColor.BackColor = _currentTheme.CellBorderColor;
				txtCellBorderColor.Text = ToRGB(_currentTheme.CellBorderColor);
			}
		}

		private void txtCellBorderColor_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtCellBorderColor.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.CellBorderColor = Color.FromArgb(R, G, B);
				btnCellBorderColor.BackColor = _currentTheme.CellBorderColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtCellBorderColor.Focus();
				txtCellBorderColor.SelectAll();
			}
		}

		private void txtCellBorderColor_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtCellBorderColor_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		private void chkDisplayVerticalDivider_CheckedChanged(object sender, System.EventArgs e)
		{
			_currentTheme.DisplayColumnGroupDivider = chkDisplayVerticalDivider.Checked;
			
			//if both vertical and horizontal dividers are unchecked, disable Divider Width combo.
			if (chkDisplayVerticalDivider.Checked==false && chkDisplayHorizontalDivider.Checked == false)
			{
				cboDividerWidth.Enabled = false;
			}
			else
			{
				cboDividerWidth.Enabled = true;
			}

			//based on whether vertical border is checked or not, enable/disable the divider color
			switch (chkDisplayVerticalDivider.Checked)
			{
				case true:
					EnableDisableVerDividerColor(true);
					break;
				case false:
					EnableDisableVerDividerColor(false);
					break;
			}
		}

		private void chkDisplayHorizontalDivider_CheckedChanged(object sender, System.EventArgs e)
		{
			_currentTheme.DisplayRowGroupDivider = chkDisplayHorizontalDivider.Checked;
			
			//if both vertical and horizontal dividers are unchecked, disable Divider Width combo.
			if (chkDisplayVerticalDivider.Checked==false && chkDisplayHorizontalDivider.Checked == false)
			{
				cboDividerWidth.Enabled = false;
			}
			else
			{
				cboDividerWidth.Enabled = true;
			}

			//based on whether horizontal border is checked or not, enable/disable the divider color
			switch (chkDisplayHorizontalDivider.Checked)
			{
				case true:
					EnableDisableHorDividerColor(true);
					break;
				case false:
					EnableDisableHorDividerColor(false);
					break;
			}
		}
		private void EnableDisableVerDividerColor(bool enable)
		{
			switch(enable)
			{
				case true:
					lblDividerColorVertical.Enabled = true;
					btnDividerColorVertical.BackColor = _currentTheme.ColumnGroupDividerBrushColor;
					btnDividerColorVertical.Enabled = true;
					txtDividerColorVertical.Text = ToRGB(_currentTheme.ColumnGroupDividerBrushColor);
					txtDividerColorVertical.Enabled = true;
					break;
				case false:
					lblDividerColorVertical.Enabled = false;
					btnDividerColorVertical.BackColor = this.BackColor;
					btnDividerColorVertical.Enabled = false;
					txtDividerColorVertical.Text = "";
					txtDividerColorVertical.Enabled = false;
					break;
			}
		}
		private void EnableDisableHorDividerColor(bool enable)
		{
			switch(enable)
			{
				case true:
					lblHorizontalDivider.Enabled = true;
					
					lblDividerColorHorRow.Enabled = true;
					btnDividerColorHorRow.BackColor = _currentTheme.RowGroupRowHeaderDividerBrushColor;
					btnDividerColorHorRow.Enabled = true;
					txtDividerColorHorRow.Text = ToRGB(_currentTheme.RowGroupRowHeaderDividerBrushColor);
					txtDividerColorHorRow.Enabled = true;

					lblDividerColorHorData.Enabled = true;
					btnDividerColorHorData.BackColor = _currentTheme.RowGroupDividerBrushColor;
					btnDividerColorHorData.Enabled = true;
					txtDividerColorHorData.Text = ToRGB(_currentTheme.RowGroupDividerBrushColor);
					txtDividerColorHorData.Enabled = true;
					break;
				case false:
					lblHorizontalDivider.Enabled = false;
					
					lblDividerColorHorRow.Enabled = false;
					btnDividerColorHorRow.BackColor = this.BackColor;
					btnDividerColorHorRow.Enabled = false;
					txtDividerColorHorRow.Text = "";
					txtDividerColorHorRow.Enabled = false;

					lblDividerColorHorData.Enabled = false;
					btnDividerColorHorData.BackColor = this.BackColor;
					btnDividerColorHorData.Enabled = false;
					txtDividerColorHorData.Text = "";
					txtDividerColorHorData.Enabled = false;
					break;
			}
		}

		private void cboDividerWidth_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			_currentTheme.DividerWidth = Convert.ToInt32(cboDividerWidth.Text, CultureInfo.CurrentUICulture);
		}

		private void btnDividerColorVertical_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();

            // Begin TT#609-MD - RMatelic - OTS Forecast Chain Ladder View
            //c.Color = _currentTheme.RowGroupDividerBrushColor;
            //if (c.ShowDialog() == DialogResult.OK)
            //{
            //    _currentTheme.RowGroupDividerBrushColor = c.Color;
            //    btnDividerColorVertical.BackColor = _currentTheme.RowGroupDividerBrushColor;
            //    txtDividerColorVertical.Text = ToRGB(_currentTheme.RowGroupDividerBrushColor);
            //}
            c.Color = _currentTheme.ColumnGroupDividerBrushColor;
            if (c.ShowDialog() == DialogResult.OK)
            {
                _currentTheme.ColumnGroupDividerBrushColor = c.Color;
                btnDividerColorVertical.BackColor = _currentTheme.ColumnGroupDividerBrushColor;
                txtDividerColorVertical.Text = ToRGB(_currentTheme.ColumnGroupDividerBrushColor);
            }
            //End TT#609-MD  
		}

		private void txtDividerColorVertical_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtDividerColorVertical.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

                // Begin TT#609-MD - RMatelic - OTS Forecast Chain Ladder View
                //_currentTheme.RowGroupDividerBrushColor = Color.FromArgb(R, G, B);
                //btnDividerColorVertical.BackColor = _currentTheme.RowGroupDividerBrushColor;
                _currentTheme.ColumnGroupDividerBrushColor = Color.FromArgb(R, G, B);
                btnDividerColorVertical.BackColor = _currentTheme.ColumnGroupDividerBrushColor;
                //End TT#609-MD
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtDividerColorVertical.Focus();
				txtDividerColorVertical.SelectAll();
			}
		}

		private void txtDividerColorVertical_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtDividerColorVertical_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		private void btnDividerColorHorRow_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.RowGroupRowHeaderDividerBrushColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
				_currentTheme.RowGroupRowHeaderDividerBrushColor = c.Color;
				btnDividerColorHorRow.BackColor = _currentTheme.RowGroupRowHeaderDividerBrushColor;
				txtDividerColorHorRow.Text = ToRGB(_currentTheme.RowGroupRowHeaderDividerBrushColor);
			}
		}

		private void txtDividerColorHorRow_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtDividerColorHorRow.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

				_currentTheme.RowGroupRowHeaderDividerBrushColor = Color.FromArgb(R, G, B);
				btnDividerColorHorRow.BackColor = _currentTheme.RowGroupRowHeaderDividerBrushColor;
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtDividerColorHorRow.Focus();
				txtDividerColorHorRow.SelectAll();
			}
		}

		private void txtDividerColorHorRow_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtDividerColorHorRow_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		private void btnDividerColorHorData_Click(object sender, System.EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			c.Color = _currentTheme.ColumnGroupDividerBrushColor;
			if (c.ShowDialog() == DialogResult.OK)
			{
                // Begin TT#609-MD - RMatelic - OTS Forecast Chain Ladder View
                //_currentTheme.ColumnGroupDividerBrushColor = c.Color;
                //btnDividerColorHorData.BackColor = _currentTheme.ColumnGroupDividerBrushColor;
                //txtDividerColorHorData.Text = ToRGB(_currentTheme.ColumnGroupDividerBrushColor);
                _currentTheme.RowGroupDividerBrushColor = c.Color;
                btnDividerColorHorData.BackColor = _currentTheme.RowGroupDividerBrushColor;
                txtDividerColorHorData.Text = ToRGB(_currentTheme.RowGroupDividerBrushColor);
                // End TT#609-MD
			}
		}

		private void txtDividerColorHorData_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				string[] RGBarray = txtDividerColorHorData.Text.Split(new char[] {','});
				if (RGBarray.Length != 3)
				{
					throw new ApplicationException();
				}
				int R = Convert.ToInt32(RGBarray[0], CultureInfo.CurrentUICulture);
				int G = Convert.ToInt32(RGBarray[1], CultureInfo.CurrentUICulture);
				int B = Convert.ToInt32(RGBarray[2], CultureInfo.CurrentUICulture);

                // Begin TT#609-MD - RMatelic - OTS Forecast Chain Ladder View
                //_currentTheme.ColumnGroupDividerBrushColor = Color.FromArgb(R, G, B);
                //btnDividerColorHorData.BackColor = _currentTheme.ColumnGroupDividerBrushColor;
                _currentTheme.RowGroupDividerBrushColor = Color.FromArgb(R, G, B);
                btnDividerColorHorData.BackColor = _currentTheme.RowGroupDividerBrushColor;
                //End TT#609-MD 
			}
			catch
			{
				MessageBox.Show("Invalid Value!", "Error");
				txtDividerColorHorData.Focus();
				txtDividerColorHorData.SelectAll();
			}
		}

		private void txtDividerColorHorData_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				txtDividerColorHorData_Validating(this, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		#endregion

		#region System Themes tab

		private void lstThemes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (!_formLoading)
			{
				_currentTheme = new Theme(_systemThemeTable, lstThemes.SelectedIndex);

				PopulateBGColors();
				PopulateViewStyle();
				PopulateFonts();
				PopulateForeColors();
				PopulateBorderOptions();
			}
		}

		private void btnRenameStyle_Click(object sender, System.EventArgs e)
		{
		
		}

		private void btnDeleteStyle_Click(object sender, System.EventArgs e)
		{
		
		}

		#endregion

		#region Misc Methods

		private void BindThemeList()
		{
			try
			{
				_formLoading = true;
				lstThemes.DataSource = _systemThemeTable;
				lstThemes.DisplayMember = "THEMENAME";
				lstThemes.SelectedIndex = -1;
				_formLoading = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private string ToRGB(Color color)
		{
			return color.R.ToString(CultureInfo.CurrentUICulture) + ", " + color.G.ToString(CultureInfo.CurrentUICulture) + ", " + color.B.ToString(CultureInfo.CurrentUICulture);
		}
		
		#endregion

        private void cboDividerWidth_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboDividerWidth_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboCellBorderStyle_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboCellBorderStyle_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboTextEffectColHeader_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboTextEffectColHeader_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboTextEffectGroupHeader_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboTextEffectGroupHeader_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboTextEffectMerDesc_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboTextEffectMerDesc_SelectionChangeCommitted(source, new EventArgs());
        }
	}
}
