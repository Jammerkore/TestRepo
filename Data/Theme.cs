using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class ThemeData : DataLayer
	{
        public ThemeData() : base()
		{
		}

		public DataTable Theme_ReadByUser(int aUserRID)
		{
			try
			{
				// MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_THEME_READ_FROM_USER.Read(_dba, USER_RID: aUserRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable Theme_ReadByTheme(int aThemeRID)
		{
			try
			{
				// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_THEME_READ.Read(_dba, THEME_RID: aThemeRID);
			}
			catch
			{
				throw;
			}
		}

		public int Theme_Insert(int aUserRID, Theme aTheme)
		{
			try
			{
                char displayRowDivider;
                char displayColDivider;

                if (aTheme.DisplayRowGroupDivider)
                {
                    displayRowDivider = 'Y';
                }
                else
                {
                    displayRowDivider = 'N';
                }

                if (aTheme.DisplayColumnGroupDivider)
                {
                    displayColDivider = 'Y';
                }
                else
                {
                    displayColDivider = 'N';
                }

                return StoredProcedures.SP_MID_THEME_INSERT.InsertAndReturnRID(_dba,
                                                         USERRID: aUserRID,
                                                         THEMENAME: aTheme.ThemeName,
                                                         VIEWSTYLE: (int)aTheme.ViewStyle,
                                                         CELLBORDERSTYLE: (int)aTheme.CellBorderStyle,
                                                         CELLBORDERCOLOR: aTheme.CellBorderColor.ToArgb(),
                                                         DIVWIDTH: aTheme.DividerWidth,
                                                         DISPLAYROWGROUPDIV: displayRowDivider,
                                                         ROWGRPROWHDRDIVCOLOR: aTheme.RowGroupRowHeaderDividerColor.ToArgb(),
                                                         ROWGRPROWHDRDIVBRUSHCOLOR: aTheme.RowGroupRowHeaderDividerBrushColor.ToArgb(),
                                                         ROWGRPDIVCOLOR: aTheme.RowGroupDividerColor.ToArgb(),
                                                         ROWGRPDIVBRUSHCOLOR: aTheme.RowGroupDividerBrushColor.ToArgb(),
                                                         DISPLAYCOLUMNGROUPDIV: displayColDivider,
                                                         COLGRPDIVCOLOR: aTheme.ColumnGroupDividerColor.ToArgb(),
                                                         COLGRPDIVBRUSHCOLOR: aTheme.ColumnGroupDividerBrushColor.ToArgb(),
                                                         NODEDESCFONTFAMILY: aTheme.NodeDescriptionFont.FontFamily.Name,
                                                         NODEDESCFONTSIZE: aTheme.NodeDescriptionFont.Size,
                                                         NODEDESCFONTSTYLE: (int)aTheme.NodeDescriptionFont.Style,
                                                         NODEDESCTEXTEFFECTS: (int)aTheme.NodeDescriptionTextEffects,
                                                         COLGRPHDRFONTFAMILY: aTheme.ColumnGroupHeaderFont.FontFamily.Name,
                                                         COLGRPHDRFONTSIZE: aTheme.ColumnGroupHeaderFont.Size,
                                                         COLGRPHDRFONTSTYLE: (int)aTheme.ColumnGroupHeaderFont.Style,
                                                         COLGRPHDRTEXTEFFECTS: (int)aTheme.ColumnGroupHeaderTextEffects,
                                                         COLHDRFONTFAMILY: aTheme.ColumnHeaderFont.FontFamily.Name,
                                                         COLHDRFONTSIZE: aTheme.ColumnHeaderFont.Size,
                                                         COLHDRFONTSTYLE: (int)aTheme.ColumnHeaderFont.Style,
                                                         COLHDRTEXTEFFECTS: (int)aTheme.ColumnHeaderTextEffects,
                                                         ROWHDRFONTFAMILY: aTheme.RowHeaderFont.FontFamily.Name,
                                                         ROWHDRFONTSIZE: aTheme.RowHeaderFont.Size,
                                                         ROWHDRFONTSTYLE: (int)aTheme.RowHeaderFont.Style,
                                                         DISPLAYONLYFONTFAMILY: aTheme.DisplayOnlyFont.FontFamily.Name,
                                                         DISPLAYONLYFONTSIZE: aTheme.DisplayOnlyFont.Size,
                                                         DISPLAYONLYFONTSTYLE: (int)aTheme.DisplayOnlyFont.Style,
                                                         EDITABLEFONTFAMILY: aTheme.EditableFont.FontFamily.Name,
                                                         EDITABLEFONTSIZE: aTheme.EditableFont.Size,
                                                         EDITABLEFONTSTYLE: (int)aTheme.EditableFont.Style,
                                                         INELIGSTRFONTFAMILY: aTheme.IneligibleStoreFont.FontFamily.Name,
                                                         INELIGSTRFONTSIZE: aTheme.IneligibleStoreFont.Size,
                                                         INELIGSTRFONTSTYLE: (int)aTheme.IneligibleStoreFont.Style,
                                                         LOCKEDFONTFAMILY: aTheme.LockedFont.FontFamily.Name,
                                                         LOCKEDFONTSIZE: aTheme.LockedFont.Size,
                                                         LOCKEDFONTSTYLE: (int)aTheme.LockedFont.Style,
                                                         NODEDESCFORECOLOR: aTheme.NodeDescriptionForeColor.ToArgb(),
                                                         COLGRPHDRFORECOLOR: aTheme.ColumnGroupHeaderForeColor.ToArgb(),
                                                         COLHDRFORECOLOR: aTheme.ColumnHeaderForeColor.ToArgb(),
                                                         NEGATIVEFORECOLOR: aTheme.NegativeForeColor.ToArgb(),
                                                         NODEDESCBACKCOLOR: aTheme.NodeDescriptionBackColor.ToArgb(),
                                                         COLGRPHDRBACKCOLOR: aTheme.ColumnGroupHeaderBackColor.ToArgb(),
                                                         COLHDRBACKCOLOR: aTheme.ColumnHeaderBackColor.ToArgb(),
                                                         STRDETROWHEADERFORECOLOR: aTheme.StoreDetailRowHeaderForeColor.ToArgb(),
                                                         STRDETFORECOLOR: aTheme.StoreDetailForeColor.ToArgb(),
                                                         STRSETROWHEADERFORECOLOR: aTheme.StoreSetRowHeaderForeColor.ToArgb(),
                                                         STRSETFORECOLOR: aTheme.StoreSetForeColor.ToArgb(),
                                                         STRTOTROWHDRFORECOLOR: aTheme.StoreTotalRowHeaderForeColor.ToArgb(),
                                                         STRTOTALFORECOLOR: aTheme.StoreTotalForeColor.ToArgb(),
                                                         STRDETROWHEADERBACKCOLOR: aTheme.StoreDetailRowHeaderBackColor.ToArgb(),
                                                         STRDETROWHEADERALTCOLOR: aTheme.StoreDetailRowHeaderAlternateBackColor.ToArgb(),
                                                         STRDETBACKCOLOR: aTheme.StoreDetailBackColor.ToArgb(),
                                                         STRDETALTCOLOR: aTheme.StoreDetailAlternateBackColor.ToArgb(),
                                                         STRSETROWHEADERBACKCOLOR: aTheme.StoreSetRowHeaderBackColor.ToArgb(),
                                                         STRSETROWHEADERALTCOLOR: aTheme.StoreSetRowHeaderAlternateBackColor.ToArgb(),
                                                         STRSETBACKCOLOR: aTheme.StoreSetBackColor.ToArgb(),
                                                         STRSETALTCOLOR: aTheme.StoreSetAlternateBackColor.ToArgb(),
                                                         STRTOTROWHDRBACKCOLOR: aTheme.StoreTotalRowHeaderBackColor.ToArgb(),
                                                         STRTOTROWHDRALTCOLOR: aTheme.StoreTotalRowHeaderAlternateBackColor.ToArgb(),
                                                         STRTOTALBACKCOLOR: aTheme.StoreTotalBackColor.ToArgb(),
                                                         STRTOTALALTCOLOR: aTheme.StoreTotalAlternateBackColor.ToArgb()
                                                         );
			}
			catch
			{
				throw;
			}
		}

		public void Theme_Update(int aThemeRID, Theme aTheme)
		{
			try
			{
                char displayRowDivider;
                char displayColDivider;

                if (aTheme.DisplayRowGroupDivider)
                {
                    displayRowDivider = 'Y';
                }
                else
                {
                    displayRowDivider = 'N';
                }

                if (aTheme.DisplayColumnGroupDivider)
                {
                    displayColDivider = 'Y';
                }
                else
                {
                    displayColDivider = 'N';
                }

                StoredProcedures.MID_THEME_UPDATE.Update(_dba,
                                                         THEME_RID: aThemeRID,
                                                         THEMENAME: aTheme.ThemeName,
                                                         VIEWSTYLE: (int)aTheme.ViewStyle,
                                                         CELLBORDERSTYLE: (int)aTheme.CellBorderStyle,
                                                         CELLBORDERCOLOR: aTheme.CellBorderColor.ToArgb(),
                                                         DIVWIDTH: aTheme.DividerWidth,
                                                         DISPLAYROWGROUPDIV: displayRowDivider,
                                                         ROWGRPROWHDRDIVCOLOR: aTheme.RowGroupRowHeaderDividerColor.ToArgb(),
                                                         ROWGRPROWHDRDIVBRUSHCOLOR: aTheme.RowGroupRowHeaderDividerBrushColor.ToArgb(),
                                                         ROWGRPDIVCOLOR: aTheme.RowGroupDividerColor.ToArgb(),
                                                         ROWGRPDIVBRUSHCOLOR: aTheme.RowGroupDividerBrushColor.ToArgb(),
                                                         DISPLAYCOLUMNGROUPDIV: displayColDivider,
                                                         COLGRPDIVCOLOR: aTheme.ColumnGroupDividerColor.ToArgb(),
                                                         COLGRPDIVBRUSHCOLOR: aTheme.ColumnGroupDividerBrushColor.ToArgb(),
                                                         NODEDESCFONTFAMILY: aTheme.NodeDescriptionFont.FontFamily.Name,
                                                         NODEDESCFONTSIZE: aTheme.NodeDescriptionFont.Size,
                                                         NODEDESCFONTSTYLE: (int)aTheme.NodeDescriptionFont.Style,
                                                         NODEDESCTEXTEFFECTS: (int)aTheme.NodeDescriptionTextEffects,
                                                         COLGRPHDRFONTFAMILY: aTheme.ColumnGroupHeaderFont.FontFamily.Name,
                                                         COLGRPHDRFONTSIZE: aTheme.ColumnGroupHeaderFont.Size,
                                                         COLGRPHDRFONTSTYLE: (int)aTheme.ColumnGroupHeaderFont.Style,
                                                         COLGRPHDRTEXTEFFECTS: (int)aTheme.ColumnGroupHeaderTextEffects,
                                                         COLHDRFONTFAMILY: aTheme.ColumnHeaderFont.FontFamily.Name,
                                                         COLHDRFONTSIZE: aTheme.ColumnHeaderFont.Size,
                                                         COLHDRFONTSTYLE: (int)aTheme.ColumnHeaderFont.Style,
                                                         COLHDRTEXTEFFECTS: (int)aTheme.ColumnHeaderTextEffects,
                                                         ROWHDRFONTFAMILY: aTheme.RowHeaderFont.FontFamily.Name,
                                                         ROWHDRFONTSIZE: aTheme.RowHeaderFont.Size,
                                                         ROWHDRFONTSTYLE: (int)aTheme.RowHeaderFont.Style,
                                                         DISPLAYONLYFONTFAMILY: aTheme.DisplayOnlyFont.FontFamily.Name,
                                                         DISPLAYONLYFONTSIZE: aTheme.DisplayOnlyFont.Size,
                                                         DISPLAYONLYFONTSTYLE: (int)aTheme.DisplayOnlyFont.Style,
                                                         EDITABLEFONTFAMILY: aTheme.EditableFont.FontFamily.Name,
                                                         EDITABLEFONTSIZE: aTheme.EditableFont.Size,
                                                         EDITABLEFONTSTYLE: (int)aTheme.EditableFont.Style,
                                                         INELIGSTRFONTFAMILY: aTheme.IneligibleStoreFont.FontFamily.Name,
                                                         INELIGSTRFONTSIZE: aTheme.IneligibleStoreFont.Size,
                                                         INELIGSTRFONTSTYLE: (int)aTheme.IneligibleStoreFont.Style,
                                                         LOCKEDFONTFAMILY: aTheme.LockedFont.FontFamily.Name,
                                                         LOCKEDFONTSIZE: aTheme.LockedFont.Size,
                                                         LOCKEDFONTSTYLE: (int)aTheme.LockedFont.Style,
                                                         NODEDESCFORECOLOR: aTheme.NodeDescriptionForeColor.ToArgb(),
                                                         COLGRPHDRFORECOLOR: aTheme.ColumnGroupHeaderForeColor.ToArgb(),
                                                         COLHDRFORECOLOR: aTheme.ColumnHeaderForeColor.ToArgb(),
                                                         NEGATIVEFORECOLOR: aTheme.NegativeForeColor.ToArgb(),
                                                         NODEDESCBACKCOLOR: aTheme.NodeDescriptionBackColor.ToArgb(),
                                                         COLGRPHDRBACKCOLOR: aTheme.ColumnGroupHeaderBackColor.ToArgb(),
                                                         COLHDRBACKCOLOR: aTheme.ColumnHeaderBackColor.ToArgb(),
                                                         STRDETROWHEADERFORECOLOR: aTheme.StoreDetailRowHeaderForeColor.ToArgb(),
                                                         STRDETFORECOLOR: aTheme.StoreDetailForeColor.ToArgb(),
                                                         STRSETROWHEADERFORECOLOR: aTheme.StoreSetRowHeaderForeColor.ToArgb(),
                                                         STRSETFORECOLOR: aTheme.StoreSetForeColor.ToArgb(),
                                                         STRTOTROWHDRFORECOLOR: aTheme.StoreTotalRowHeaderForeColor.ToArgb(),
                                                         STRTOTALFORECOLOR: aTheme.StoreTotalForeColor.ToArgb(),
                                                         STRDETROWHEADERBACKCOLOR: aTheme.StoreDetailRowHeaderBackColor.ToArgb(),
                                                         STRDETROWHEADERALTCOLOR: aTheme.StoreDetailRowHeaderAlternateBackColor.ToArgb(),
                                                         STRDETBACKCOLOR: aTheme.StoreDetailBackColor.ToArgb(),
                                                         STRDETALTCOLOR: aTheme.StoreDetailAlternateBackColor.ToArgb(),
                                                         STRSETROWHEADERBACKCOLOR: aTheme.StoreSetRowHeaderBackColor.ToArgb(),
                                                         STRSETROWHEADERALTCOLOR: aTheme.StoreSetRowHeaderAlternateBackColor.ToArgb(),
                                                         STRSETBACKCOLOR: aTheme.StoreSetBackColor.ToArgb(),
                                                         STRSETALTCOLOR: aTheme.StoreSetAlternateBackColor.ToArgb(),
                                                         STRTOTROWHDRBACKCOLOR: aTheme.StoreTotalRowHeaderBackColor.ToArgb(),
                                                         STRTOTROWHDRALTCOLOR: aTheme.StoreTotalRowHeaderAlternateBackColor.ToArgb(),
                                                         STRTOTALBACKCOLOR: aTheme.StoreTotalBackColor.ToArgb(),
                                                         STRTOTALALTCOLOR: aTheme.StoreTotalAlternateBackColor.ToArgb()
                                                         );
			}
			catch
			{
				throw;
			}
		}

        //BEGIN TT#3914-VStuart-Set default Theme to Modern 1 (for new users)-MID
        public int GetDefaultThemeRID()
        {
            try
            {
                return Convert.ToInt32(StoredProcedures.MID_THEME_READ_THEME_RID.ReadValue(_dba, THEME_NAME: Include.DefaultThemeName, USER_RID: Include.GlobalUserRID));
            }
            catch
            {
                throw;
            }
        }
        //END TT#3914-VStuart-Set default Theme to Modern 1 (for new users)-MID
      
	}
}
