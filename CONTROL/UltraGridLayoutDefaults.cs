using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;
using Infragistics.Win;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows.Controls
{
    public class UltraGridLayoutDefaults
    {
        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        Image _errorImage = null;

        public UltraGridLayoutDefaults()
        {
        }

        public UltraGridLayoutDefaults(Image aErrorImage)
        {
            _errorImage = aErrorImage;
        }

		// Begin stodd - merge issue between 4.0 and 5.0
		public void ApplyDefaults(Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			ApplyDefaults(e, false);
		}
		// End stodd - merge issue between 4.0 and 5.0

        public void ApplyDefaults(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e, bool aApplyColumnFormat, int aDecimalPositions, bool aAutoResizeColumns)
        {
            ApplyDefaults(e, aAutoResizeColumns);

            aUltraGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            if (aApplyColumnFormat)
            {
                FormatColumns(aUltraGrid, aDecimalPositions);
            }

            aUltraGrid.BeforeExitEditMode +=new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(aUltraGrid_BeforeExitEditMode);
            aUltraGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(aUltraGrid_KeyDown);
        }

        //End TT#169

        public void ApplyDefaults(Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e, bool aAutoResizeColumns)
        {
            e.Layout.Override.RowAlternateAppearance.BackColor = Color.Snow;
            // BEGIN MID Track #5467 - Default Column Chooser should not be available 
            e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.None;
            // END MID Track #5467 

            e.Layout.Override.FixedHeaderIndicator = FixedHeaderIndicator.None;
            e.Layout.Override.FixedCellAppearance.BackColor = Color.LightYellow;
            e.Layout.Override.HeaderAppearance.TextHAlign = HAlign.Center;
            e.Layout.Override.HeaderAppearance.TextVAlign = VAlign.Middle;

            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            if (aAutoResizeColumns)
            {
                e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            }
        }

        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        //public void ApplyDefaults(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid)
        public void ApplyDefaults(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid, bool aApplyColumnFormat, int aDecimalPositions, bool aAutoResizeColumns)
        //End TT#169
        {
            aUltraGrid.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Snow;
            aUltraGrid.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.None;

            aUltraGrid.DisplayLayout.Override.FixedHeaderIndicator = FixedHeaderIndicator.None;
            aUltraGrid.DisplayLayout.Override.FixedCellAppearance.BackColor = Color.LightYellow;
            aUltraGrid.DisplayLayout.Override.HeaderAppearance.TextHAlign = HAlign.Center;
            aUltraGrid.DisplayLayout.Override.HeaderAppearance.TextVAlign = VAlign.Middle;

            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            aUltraGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            if (aApplyColumnFormat)
            {
                FormatColumns(aUltraGrid, aDecimalPositions);
            }

            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            if (aAutoResizeColumns)
            {
                aUltraGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            }

            aUltraGrid.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(aUltraGrid_BeforeExitEditMode);
            aUltraGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(aUltraGrid_KeyDown);
            //End TT#169
        }

        public void ApplyAppearance(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid)
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();

            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            aUltraGrid.DisplayLayout.Appearance = appearance1;
            aUltraGrid.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            aUltraGrid.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            aUltraGrid.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            aUltraGrid.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            aUltraGrid.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            aUltraGrid.DisplayLayout.Override.RowSelectorWidth = 12;
            aUltraGrid.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            aUltraGrid.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            aUltraGrid.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            aUltraGrid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
        }

        // Begin TT#3139 - JSmith - Store Profiles – read-only access
        public void ApplyAppearanceCenterText(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid)
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();

            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            aUltraGrid.DisplayLayout.Appearance = appearance1;
            aUltraGrid.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            aUltraGrid.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Center";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            aUltraGrid.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            aUltraGrid.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            aUltraGrid.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            aUltraGrid.DisplayLayout.Override.RowSelectorWidth = 12;
            aUltraGrid.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            aUltraGrid.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            aUltraGrid.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            aUltraGrid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
        }
        // End TT#3139 - JSmith - Store Profiles – read-only access

        public void ApplyAppearance(Infragistics.Win.UltraWinGrid.UltraDropDown aUltraDropDown)
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();

            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            aUltraDropDown.DisplayLayout.Appearance = appearance1;
            aUltraDropDown.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            aUltraDropDown.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            aUltraDropDown.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            aUltraDropDown.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            aUltraDropDown.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            aUltraDropDown.DisplayLayout.Override.RowSelectorWidth = 12;
            aUltraDropDown.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            aUltraDropDown.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            aUltraDropDown.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            aUltraDropDown.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
        }

        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        private void FormatColumns(Infragistics.Win.UltraWinGrid.UltraGrid ultragid, int aDecimalPositions)
        {
            try
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridBand oBand in ultragid.DisplayLayout.Bands)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn oColumn in oBand.Columns)
                    {
                        switch (oColumn.DataType.ToString())
                        {
                            case "System.Int32":
                                oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                                oColumn.Format = "#,###,##0";
                                //oColumn.MaskInput = "99999";
                                //oColumn.Style = ColumnStyle.Integer;
                                oColumn.PromptChar = ' ';
                                break;
                            case "System.Decimal":
                            case "System.Double":
                                oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                                oColumn.Format = "#,###,###.0";
                                //oColumn.MaskInput = "#######.0##";
                                for (int i = 0; i < aDecimalPositions - 1; i++)
                                {
                                    oColumn.Format += "0";
                                    //oColumn.MaskInput += "0";
                                };
                                break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        void aUltraGrid_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinGrid.UltraGrid grid;

                grid = (Infragistics.Win.UltraWinGrid.UltraGrid)sender;
                if (grid.ActiveCell == null)
                {
                    return;
                }
                // if the cell has a GridCellTag, reset the validation to false
                if (grid.ActiveCell.Tag != null &&
                    grid.ActiveCell.Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
                {
                    ((MIDRetail.DataCommon.GridCellTag)grid.ActiveCell.Tag).GridCellTagData = false;
                }
            }
            catch
            {
                throw;
            }
        }

        void aUltraGrid_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinGrid.UltraGrid grid;
                string textValue;
                GridCellTag gridCellTag;

                grid = (Infragistics.Win.UltraWinGrid.UltraGrid)sender;
                if (grid.ActiveCell == null)
                {
                    return;
                }
                textValue = grid.ActiveCell.Text.Replace(",", null).Trim();

                // if the cell has a GridCellTag and the data has a bool of true,
                // then the cell has already been validated so don't do again
                if (grid.ActiveCell.Tag != null &&
                    grid.ActiveCell.Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
                {
                    gridCellTag = (MIDRetail.DataCommon.GridCellTag)grid.ActiveCell.Tag;
                    if (gridCellTag.GridCellTagData != null &&
                        gridCellTag.GridCellTagData.GetType() == typeof(System.Boolean))
                    {
                        if ((bool)gridCellTag.GridCellTagData)
                        {
                            return;
                        }
                    }
                }

                // allow values with a coma.  do not check dropdown list which are also type int with the RID
                // Begin TT#1750 - JSmith - Setup of 53 week year - receive error
                //if (grid.ActiveCell.Column.DataType == System.Type.GetType("System.Int32") &&
                //    grid.ActiveCell.StyleResolved != ColumnStyle.DropDownList)
                if (grid.ActiveCell.Column.DataType == System.Type.GetType("System.Int32") &&
                    grid.ActiveCell.StyleResolved != ColumnStyle.DropDownList &&
                    grid.ActiveCell.StyleResolved != ColumnStyle.DropDownValidate)
                // End TT#1750
                {
                    try
                    {
                        if (grid.ActiveCell.Text.Trim().Length > 0)
                        {
                            //textValue = grid.ActiveCell.Text.Replace(",", null).Trim();
                            grid.ActiveCell.Value = textValue;
                            // can't find way to check for data error when value is set
                            // so test value before and after set to see if value is the same
                            // if value is not the same, assume an error occurred.
                            if (textValue != grid.ActiveCell.Value.ToString())
                            {
                                gridCellTag = new GridCellTag();
                                grid.ActiveCell.Appearance.Image = _errorImage;
                                gridCellTag.Message = MIDText.GetText(eMIDTextCode.msg_MustBeInteger);
                                gridCellTag.GridCellTagData = true;
                                grid.ActiveCell.Tag = gridCellTag;
                                return;
                            }
                        }
                        grid.ActiveCell.Appearance.Image = null;
                        grid.ActiveCell.Tag = null;
                    }
                    catch
                    {
                        gridCellTag = new GridCellTag();
                        grid.ActiveCell.Appearance.Image = _errorImage;
                        gridCellTag.Message = MIDText.GetText(eMIDTextCode.msg_MustBeInteger);
                        gridCellTag.GridCellTagData = true;
                        grid.ActiveCell.Tag = gridCellTag;
                    }
                }
                else if (grid.ActiveCell.Column.DataType == System.Type.GetType("System.Double"))
                {
                    try
                    {
                        if (grid.ActiveCell.Text.Trim().Length > 0)
                        {
                            grid.ActiveCell.Value = textValue;
                            // Begin TT#1155 - JSmith - Must be numberic
                            //// can't find way to check for data error when value is set
                            //// so test value before and after set to see if value is the same
                            //// if value is not the same, assume an error occurred.
                            //if (textValue != grid.ActiveCell.Value.ToString())
                            //{
                            //    gridCellTag = new GridCellTag();
                            //    grid.ActiveCell.Appearance.Image = _errorImage;
                            //    gridCellTag.Message = MIDText.GetText(eMIDTextCode.msg_MustBeNumeric);
                            //    gridCellTag.GridCellTagData = true;
                            //    grid.ActiveCell.Tag = gridCellTag;
                            //    return;
                            //}
                            double x = Convert.ToDouble(textValue);
                            // End TT#1155
                        }
                        grid.ActiveCell.Appearance.Image = null;
                        grid.ActiveCell.Tag = null;
                    }
                    catch
                    {
                        gridCellTag = new GridCellTag();
                        grid.ActiveCell.Appearance.Image = _errorImage;
                        gridCellTag.Message = MIDText.GetText(eMIDTextCode.msg_MustBeNumeric);
                        gridCellTag.GridCellTagData = true;
                        grid.ActiveCell.Tag = gridCellTag;
                    }
                }
                else if (grid.ActiveCell.Column.DataType == System.Type.GetType("System.Decimal"))
                {
                    try
                    {
                        if (grid.ActiveCell.Text.Trim().Length > 0)
                        {
                            grid.ActiveCell.Value = textValue;
                            //// Begin TT#1155 - JSmith - Must be numberic
                            //// can't find way to check for data error when value is set
                            //// so test value before and after set to see if value is the same
                            //// if value is not the same, assume an error occurred.
                            //if (textValue != grid.ActiveCell.Value.ToString())
                            //{
                            //    gridCellTag = new GridCellTag();
                            //    grid.ActiveCell.Appearance.Image = _errorImage;
                            //    gridCellTag.Message = MIDText.GetText(eMIDTextCode.msg_MustBeNumeric);
                            //    gridCellTag.GridCellTagData = true;
                            //    grid.ActiveCell.Tag = gridCellTag;
                            //    return;
                            //}
                            decimal x = Convert.ToDecimal(textValue);
                            // End TT#1155
                        }
                        grid.ActiveCell.Appearance.Image = null;
                        grid.ActiveCell.Tag = null;
                    }
                    catch
                    {
                        gridCellTag = new GridCellTag();
                        grid.ActiveCell.Appearance.Image = _errorImage;
                        gridCellTag.Message = MIDText.GetText(eMIDTextCode.msg_MustBeNumeric);
                        gridCellTag.GridCellTagData = true;
                        grid.ActiveCell.Tag = gridCellTag;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void DetachGridEventHandlers(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid)
        {
            aUltraGrid.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(aUltraGrid_BeforeExitEditMode);
            aUltraGrid.KeyDown -= new System.Windows.Forms.KeyEventHandler(aUltraGrid_KeyDown);
        }
        //End TT#169
    }
}
