using System;
using System.Data;
using System.Drawing;
using C1.Win.C1FlexGrid;
using System.Globalization;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// This class contains information that defines a Theme.
	/// </summary>

	public class Theme
	{		
		//=======
		// FIELDS
		//=======

		private Color _rowGroupRowHeaderDividerBrushColor;
		private Color _rowGroupDividerBrushColor;
		private Color _columnGroupDividerBrushColor;
		private SolidBrush _rowGroupRowHeaderDividerBrush;
		private SolidBrush _rowGroupDividerBrush;
		private SolidBrush _columnGroupDividerBrush;
		private SolidBrush _chiselLowerBrush;
		private SolidBrush _chiselUpperBrush;

		public string ThemeName;
		public StyleEnum ViewStyle;
		public bool DisplayRowGroupDivider;
		public bool DisplayColumnGroupDivider;
		public BorderStyleEnum CellBorderStyle;
		public Color CellBorderColor;
		public int DividerWidth;

		public Font CornerFont = new Font(System.Drawing.FontFamily.GenericSansSerif, 8, FontStyle.Bold);
		public Font NodeDescriptionFont;
		public Font ColumnGroupHeaderFont;
		public Font ColumnHeaderFont;
		public Font RowHeaderFont;
		public Font DisplayOnlyFont;
		public Font EditableFont;
		public Font IneligibleStoreFont;
		public Font LockedFont;

		public TextEffectEnum NodeDescriptionTextEffects;
		public TextEffectEnum ColumnGroupHeaderTextEffects;
		public TextEffectEnum ColumnHeaderTextEffects;

		public Color RowGroupRowHeaderDividerColor;
		public Color RowGroupDividerColor;
		public Color ColumnGroupDividerColor;

		public Color CornerForeColor = Color.White;
		public Color NodeDescriptionForeColor;
		public Color ColumnGroupHeaderForeColor;
		public Color ColumnHeaderForeColor;
		public Color NegativeForeColor;
		public Color StoreDetailRowHeaderForeColor;
		public Color StoreDetailForeColor;
		public Color StoreSetRowHeaderForeColor;
		public Color StoreSetForeColor;
		public Color StoreTotalRowHeaderForeColor;
		public Color StoreTotalForeColor;

		public Color CornerBackColor = Color.Black;
		public Color NodeDescriptionBackColor;
		public Color ColumnGroupHeaderBackColor;
		public Color ColumnHeaderBackColor;
		public Color StoreDetailRowHeaderBackColor;
		public Color StoreDetailRowHeaderAlternateBackColor;
		public Color StoreDetailBackColor;
		public Color StoreDetailAlternateBackColor;
		public Color StoreSetRowHeaderBackColor;
		public Color StoreSetRowHeaderAlternateBackColor;
		public Color StoreSetBackColor;
		public Color StoreSetAlternateBackColor;
		public Color StoreTotalRowHeaderBackColor;
		public Color StoreTotalRowHeaderAlternateBackColor;
		public Color StoreTotalBackColor;
		public Color StoreTotalAlternateBackColor;

		//=============
		// CONSTRUCTORS
		//=============

		public Theme()
		{
		}

		public Theme(DataTable aDataTable, int aRow)
		{
			DataRow dataRow;
			string nodeDescriptionFontFamily = "";
			string columnGroupHeaderFontFamily = "";
			string columnHeaderFontFamily = "";
			string rowHeaderFontFamily = "";
			string displayOnlyFontFamily = "";
			string editableFontFamily = "";
			string ineligibleStoreFontFamily = "";
			string lockedFontFamily = "";
			float nodeDescriptionFontSize = 0;
			float columnGroupHeaderFontSize = 0;
			float columnHeaderFontSize = 0;
			float rowHeaderFontSize = 0;
			float displayOnlyFontSize = 0;
			float editableFontSize = 0;
			float ineligibleStoreFontSize = 0;
			float lockedFontSize = 0;
			FontStyle nodeDescriptionFontStyle = FontStyle.Regular;
			FontStyle columnGroupHeaderFontStyle = FontStyle.Regular;
			FontStyle columnHeaderFontStyle = FontStyle.Regular;
			FontStyle rowHeaderFontStyle = FontStyle.Regular;
			FontStyle displayOnlyFontStyle = FontStyle.Regular;
			FontStyle editableFontStyle = FontStyle.Regular;
			FontStyle ineligibleStoreFontStyle = FontStyle.Regular;
			FontStyle lockedFontStyle = FontStyle.Regular;

			dataRow = aDataTable.Rows[aRow];

			foreach (DataColumn dataCol in aDataTable.Columns)
			{
				switch (dataCol.ColumnName)
				{
					case "THEMENAME":
						ThemeName = Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "VIEWSTYLE":
						ViewStyle = (StyleEnum)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "CELLBORDERSTYLE":
						CellBorderStyle = (BorderStyleEnum)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "CELLBORDERCOLOR":
						CellBorderColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "DIVWIDTH":
						DividerWidth = Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "DISPLAYROWGROUPDIV":
						if (Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture) == "Y")
						{
							DisplayRowGroupDivider = true;
						}
						else
						{
							DisplayRowGroupDivider = false;
						}
						break;

					case "ROWGROUPROWHEADERDIVCOLOR":
						RowGroupRowHeaderDividerColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "ROWGROUPROWHEADERDIVBRUSHCOLOR":
						RowGroupRowHeaderDividerBrushColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "ROWGROUPDIVCOLOR":
						RowGroupDividerColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "ROWGROUPDIVBRUSHCOLOR":
						RowGroupDividerBrushColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "DISPLAYCOLUMNGROUPDIV":
						if (Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture) == "Y")
						{
							DisplayColumnGroupDivider = true;
						}
						else
						{
							DisplayColumnGroupDivider = false;
						}
						break;

					case "COLUMNGROUPDIVCOLOR":
						ColumnGroupDividerColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "COLUMNGROUPDIVBRUSHCOLOR":
						ColumnGroupDividerBrushColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "NODEDESCRIPTIONFONTFAMILY":
						nodeDescriptionFontFamily = Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "NODEDESCRIPTIONFONTSIZE":
						nodeDescriptionFontSize = Convert.ToSingle(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "NODEDESCRIPTIONFONTSTYLE":
						nodeDescriptionFontStyle = (FontStyle)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "NODEDESCRIPTIONTEXTEFFECTS":
						NodeDescriptionTextEffects = (TextEffectEnum)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "COLUMNGROUPHEADERFONTFAMILY":
						columnGroupHeaderFontFamily = Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "COLUMNGROUPHEADERFONTSIZE":
						columnGroupHeaderFontSize = Convert.ToSingle(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "COLUMNGROUPHEADERFONTSTYLE":
						columnGroupHeaderFontStyle = (FontStyle)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "COLUMNGROUPHEADERTEXTEFFECTS":
						ColumnGroupHeaderTextEffects = (TextEffectEnum)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "COLUMNHEADERFONTFAMILY":
						columnHeaderFontFamily = Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "COLUMNHEADERFONTSIZE":
						columnHeaderFontSize = Convert.ToSingle(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "COLUMNHEADERFONTSTYLE":
						columnHeaderFontStyle = (FontStyle)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "COLUMNHEADERTEXTEFFECTS":
						ColumnHeaderTextEffects = (TextEffectEnum)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "ROWHEADERFONTFAMILY":
						rowHeaderFontFamily = Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "ROWHEADERFONTSIZE":
						rowHeaderFontSize = Convert.ToSingle(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "ROWHEADERFONTSTYLE":
						rowHeaderFontStyle = (FontStyle)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "DISPLAYONLYFONTFAMILY":
						displayOnlyFontFamily = Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "DISPLAYONLYFONTSIZE":
						displayOnlyFontSize = Convert.ToSingle(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "DISPLAYONLYFONTSTYLE":
						displayOnlyFontStyle = (FontStyle)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "EDITABLEFONTFAMILY":
						editableFontFamily = Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "EDITABLEFONTSIZE":
						editableFontSize = Convert.ToSingle(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "EDITABLEFONTSTYLE":
						editableFontStyle = (FontStyle)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "INELIGIBLESTOREFONTFAMILY":
						ineligibleStoreFontFamily = Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "INELIGIBLESTOREFONTSIZE":
						ineligibleStoreFontSize = Convert.ToSingle(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "INELIGIBLESTOREFONTSTYLE":
						ineligibleStoreFontStyle = (FontStyle)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "LOCKEDFONTFAMILY":
						lockedFontFamily = Convert.ToString(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "LOCKEDFONTSIZE":
						lockedFontSize = Convert.ToSingle(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "LOCKEDFONTSTYLE":
						lockedFontStyle = (FontStyle)Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture);
						break;

					case "NODEDESCRIPTIONFORECOLOR":
						NodeDescriptionForeColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "COLUMNGROUPHEADERFORECOLOR":
						ColumnGroupHeaderForeColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "COLUMNHEADERFORECOLOR":
						ColumnHeaderForeColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "NEGATIVEFORECOLOR":
						NegativeForeColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "NODEDESCRIPTIONBACKCOLOR":
						NodeDescriptionBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "COLUMNGROUPHEADERBACKCOLOR":
						ColumnGroupHeaderBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "COLUMNHEADERBACKCOLOR":
						ColumnHeaderBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STOREDETAILROWHEADERFORECOLOR":
						StoreDetailRowHeaderForeColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STOREDETAILFORECOLOR":
						StoreDetailForeColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORESETROWHEADERFORECOLOR":
						StoreSetRowHeaderForeColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORESETFORECOLOR":
						StoreSetForeColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORETOTALROWHEADERFORECOLOR":
						StoreTotalRowHeaderForeColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORETOTALFORECOLOR":
						StoreTotalForeColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STOREDETAILROWHEADERBACKCOLOR":
						StoreDetailRowHeaderBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STOREDETAILROWHEADERALTCOLOR":
						StoreDetailRowHeaderAlternateBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STOREDETAILBACKCOLOR":
						StoreDetailBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STOREDETAILALTBACKCOLOR":
						StoreDetailAlternateBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORESETROWHEADERBACKCOLOR":
						StoreSetRowHeaderBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORESETROWHEADERALTCOLOR":
						StoreSetRowHeaderAlternateBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORESETBACKCOLOR":
						StoreSetBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORESETALTBACKCOLOR":
						StoreSetAlternateBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORETOTALROWHEADERBACKCOLOR":
						StoreTotalRowHeaderBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORETOTALROWHEADERALTCOLOR":
						StoreTotalRowHeaderAlternateBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORETOTALBACKCOLOR":
						StoreTotalBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

					case "STORETOTALALTCOLOR":
						StoreTotalAlternateBackColor = Color.FromArgb(Convert.ToInt32(dataRow[dataCol], CultureInfo.CurrentUICulture));
						break;

				}
			}

			NodeDescriptionFont = new Font(nodeDescriptionFontFamily, nodeDescriptionFontSize, nodeDescriptionFontStyle);
			ColumnGroupHeaderFont = new Font(columnGroupHeaderFontFamily, columnGroupHeaderFontSize, columnGroupHeaderFontStyle);
			ColumnHeaderFont = new Font(columnHeaderFontFamily, columnHeaderFontSize, columnHeaderFontStyle);
			RowHeaderFont = new Font(rowHeaderFontFamily, rowHeaderFontSize, rowHeaderFontStyle);
			DisplayOnlyFont = new Font(displayOnlyFontFamily, displayOnlyFontSize, displayOnlyFontStyle);
			EditableFont = new Font(editableFontFamily, editableFontSize, editableFontStyle);
			IneligibleStoreFont = new Font(ineligibleStoreFontFamily, ineligibleStoreFontSize, ineligibleStoreFontStyle);
			LockedFont = new Font(lockedFontFamily, lockedFontSize, lockedFontStyle);
		}
		
		public Theme(Theme aTheme)
		{
			CopyFrom(aTheme);
		}
	
		//===========
		// PROPERTIES
		//===========

		public SolidBrush RowGroupRowHeaderDividerBrush
		{
			get
			{
				if (_rowGroupRowHeaderDividerBrush == null)
				{
					_rowGroupRowHeaderDividerBrush = new SolidBrush(_rowGroupRowHeaderDividerBrushColor);
				}
				
				return _rowGroupRowHeaderDividerBrush;
			}
			set
			{
				_rowGroupRowHeaderDividerBrush = value;
			}
		}

		public SolidBrush RowGroupDividerBrush
		{
			get
			{
				if (_rowGroupDividerBrush == null)
				{
					_rowGroupDividerBrush = new SolidBrush(_rowGroupDividerBrushColor);
				}
				
				return _rowGroupDividerBrush;
			}
			set
			{
				_rowGroupDividerBrush = value;
			}
		}

		public SolidBrush ColumnGroupDividerBrush
		{
			get
			{
				if (_columnGroupDividerBrush == null)
				{
					_columnGroupDividerBrush = new SolidBrush(_columnGroupDividerBrushColor);
				}

				return _columnGroupDividerBrush;
			}
			set
			{
				_columnGroupDividerBrush = value;
			}
		}

		public SolidBrush ChiselUpperBrush
		{
			get
			{
				if (_chiselUpperBrush == null)
				{
					_chiselUpperBrush = new SolidBrush(Color.White);
				}

				return _chiselUpperBrush;
			}
			set
			{
				_chiselUpperBrush = value;
			}
		}

		public SolidBrush ChiselLowerBrush
		{
			get
			{
				if (_chiselLowerBrush == null)
				{
					_chiselLowerBrush = new SolidBrush(Color.Gray);
				}

				return _chiselLowerBrush;
			}
			set
			{
				_chiselLowerBrush = value;
			}
		}

		public Color RowGroupRowHeaderDividerBrushColor
		{
			get
			{
				return _rowGroupRowHeaderDividerBrushColor;
			}
			set
			{
				_rowGroupRowHeaderDividerBrushColor = value;
				_rowGroupRowHeaderDividerBrush = null;
			}
		}

		public Color RowGroupDividerBrushColor
		{
			get
			{
				return _rowGroupDividerBrushColor;
			}
			set
			{
				_rowGroupDividerBrushColor = value;
				_rowGroupDividerBrush = null;
			}
		}

		public Color ColumnGroupDividerBrushColor
		{
			get
			{
				return _columnGroupDividerBrushColor;
			}
			set
			{
				_columnGroupDividerBrushColor = value;
				_columnGroupDividerBrush = null;
			}
		}

		// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
		public Font StoreAllocationOutOfBalance
		{
			get
			{
				return this.LockedFont;
			}
		}
		// END MID Track #1511
		//========
		// METHODS
		//========

		public void CopyFrom(Theme aTheme)
		{
			ViewStyle = aTheme.ViewStyle;
			DisplayRowGroupDivider = aTheme.DisplayRowGroupDivider;
			DisplayColumnGroupDivider = aTheme.DisplayColumnGroupDivider;
			CellBorderStyle = aTheme.CellBorderStyle;
			CellBorderColor = aTheme.CellBorderColor;
			DividerWidth = aTheme.DividerWidth;
			NodeDescriptionFont = aTheme.NodeDescriptionFont;
			ColumnGroupHeaderFont = aTheme.ColumnGroupHeaderFont;
			ColumnHeaderFont = aTheme.ColumnHeaderFont;
			RowHeaderFont = aTheme.RowHeaderFont;
			DisplayOnlyFont = aTheme.DisplayOnlyFont;
			EditableFont = aTheme.EditableFont;
			IneligibleStoreFont = aTheme.IneligibleStoreFont;
			LockedFont = aTheme.LockedFont;
			NodeDescriptionTextEffects = aTheme.NodeDescriptionTextEffects;
			ColumnGroupHeaderTextEffects = aTheme.ColumnGroupHeaderTextEffects;
			ColumnHeaderTextEffects = aTheme.ColumnHeaderTextEffects;
			RowGroupRowHeaderDividerColor = aTheme.RowGroupRowHeaderDividerColor;
			RowGroupDividerColor = aTheme.RowGroupDividerColor;
			ColumnGroupDividerColor = aTheme.ColumnGroupDividerColor;
			RowGroupRowHeaderDividerBrushColor = aTheme.RowGroupRowHeaderDividerBrushColor;
			RowGroupDividerBrushColor = aTheme.RowGroupDividerBrushColor;
			ColumnGroupDividerBrushColor = aTheme.ColumnGroupDividerBrushColor;
			NodeDescriptionForeColor = aTheme.NodeDescriptionForeColor;
			ColumnGroupHeaderForeColor = aTheme.ColumnGroupHeaderForeColor;
			ColumnHeaderForeColor = aTheme.ColumnHeaderForeColor;
			NegativeForeColor = aTheme.NegativeForeColor;
			StoreDetailRowHeaderForeColor = aTheme.StoreDetailRowHeaderForeColor;
			StoreDetailForeColor = aTheme.StoreDetailForeColor;
			StoreSetRowHeaderForeColor = aTheme.StoreSetRowHeaderForeColor;
			StoreSetForeColor = aTheme.StoreSetForeColor;
			StoreTotalRowHeaderForeColor = aTheme.StoreTotalRowHeaderForeColor;
			StoreTotalForeColor = aTheme.StoreTotalForeColor;
			NodeDescriptionBackColor = aTheme.NodeDescriptionBackColor;
			ColumnGroupHeaderBackColor = aTheme.ColumnGroupHeaderBackColor;
			ColumnHeaderBackColor = aTheme.ColumnHeaderBackColor;
			StoreDetailRowHeaderBackColor = aTheme.StoreDetailRowHeaderBackColor;
			StoreDetailRowHeaderAlternateBackColor = aTheme.StoreDetailRowHeaderAlternateBackColor;
			StoreDetailBackColor = aTheme.StoreDetailBackColor;
			StoreDetailAlternateBackColor = aTheme.StoreDetailAlternateBackColor;
			StoreSetRowHeaderBackColor = aTheme.StoreSetRowHeaderBackColor;
			StoreSetRowHeaderAlternateBackColor = aTheme.StoreSetRowHeaderAlternateBackColor;
			StoreSetBackColor = aTheme.StoreSetBackColor;
			StoreSetAlternateBackColor = aTheme.StoreSetAlternateBackColor;
			StoreTotalRowHeaderBackColor = aTheme.StoreTotalRowHeaderBackColor;
			StoreTotalRowHeaderAlternateBackColor = aTheme.StoreTotalRowHeaderAlternateBackColor;
			StoreTotalBackColor = aTheme.StoreTotalBackColor;
			StoreTotalAlternateBackColor = aTheme.StoreTotalAlternateBackColor;
		}
	}
}
