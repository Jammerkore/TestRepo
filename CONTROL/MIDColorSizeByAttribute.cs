using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace MIDRetail.Windows.Controls
{
	public partial class MIDColorSizeByAttribute : Control
	{
		// Constants

		private const string C_SET = "SETLEVEL",
								C_SET_ALL_CLR = "SETALLCOLOR",
								C_SET_CLR = "SETCOLOR",
								C_ALL_CLR_SZ = "ALLCOLORSIZE",
								C_CLR_SZ = "COLORSIZE",
								C_CLR_SZ_DIM = "COLORSIZEDIMENSION",
								C_ALL_CLR_SZ_DIM = "ALLCOLORSIZEDIMENSION";

		private const int C_CONTEXT_SET_APPLY = 0,
							C_CONTEXT_SET_CLEAR = 1,
							C_CONTEXT_SET_SEP1 = 2,
							C_CONTEXT_SET_ADD_CLR = 3;

		private const int C_CONTEXT_CLR_APPLY = 0,
							C_CONTEXT_CLR_CLEAR = 1,
							C_CONTEXT_CLR_SEP1 = 2,
							C_CONTEXT_CLR_ADD_CLR = 3,
							C_CONTEXT_CLR_ADD_SZ_DIM = 4,
							C_CONTEXT_CLR_SEP2 = 5,
							C_CONTEXT_CLR_DELETE = 6;

		private const int C_CONTEXT_SZ_ADD_SZ = 1,
							C_CONTEXT_SZ_SEP1 = 2,
							C_CONTEXT_SZ_DELETE = 3;

		private const int C_CONTEXT_DIM_APPLY = 0,
							C_CONTEXT_DIM_CLEAR = 1,
							C_CONTEXT_DIM_SEP1 = 2,
							C_CONTEXT_DIM_ADD_DIM = 3,
							C_CONTEXT_DIM_ADD_SZ = 4,
							C_CONTEXT_DIM_SEP2 = 5,
							C_CONTEXT_DIM_DELETE = 6;

		private const string C_INHERITED_FROM = "Inherited from ";

		// Events

		public delegate void ValueChangedHandler(object source);
		public event ValueChangedHandler ValueChanged;

		// Fields

		private SessionAddressBlock _SAB;
		private SizeOutOfStockProfile _sizeOOSProfile;
		
		private Image _errorImage = null;
		private Image _inheritanceImage = null;
		private FunctionSecurityProfile _functionSecurity;

		private ContextMenu _setContext;
		private ContextMenu _colorContext;
		private ContextMenu _sizeContext;
		private ContextMenu _dimensionContext;

		private DataSet _dsQuantities;
		private DataTable _dtColors = null;
		private DataTable _dtColorSizes = null;
		private DataTable _dtSizes = null;

		private ArrayList _errMsgs = new ArrayList();
		private bool _editByCell = false;
		private bool _rollback = false;
		private bool _showContext = false;

		private KeyEventArgs _gridKeyEvent = null;

		private bool _OOSLocked = false;

		private Hashtable _hnpHash = new Hashtable();

		// Constructors

		public MIDColorSizeByAttribute()
		{
			InitializeComponent();

			_hnpHash = new Hashtable();
		}

		// Properties

		private bool EditByCell
		{
			get { return _editByCell; }
			set { _editByCell = value; }
		}

		private ArrayList ErrorMessages
		{
			get { return _errMsgs; }
		}

		// Methods

		private void ugAllSize_InitializeLayout(object sender, InitializeLayoutEventArgs e)
		{
			InfragisticsLayoutData layoutData;
			InfragisticsLayout layout;
			UltraGridLayoutDefaults ugld;

			try
			{
				layoutData = new InfragisticsLayoutData();
				layout = layoutData.InfragisticsLayout_Read(_SAB.ClientServerSession.UserRID, eLayoutID.sizeOutOfStockGrid);

				if (layout.LayoutLength > 0)
				{
					ugAllSize.DisplayLayout.Load(layout.LayoutStream);
				}
				else
				{
                    ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                    //ugld.ApplyDefaults(e);
                    ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
					DefaultGridLayout();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugAllSize_InitializeLayout");
			}
		}

		private void ugAllSize_SelectionDrag(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if (ugAllSize.Selected.Cells.Count > 0)
				{
					ugAllSize.DoDragDrop(ugAllSize.Selected.Cells, DragDropEffects.Move);
					return;
				}

				if (ugAllSize.Selected.Rows.Count > 0)
				{
					ugAllSize.DoDragDrop(ugAllSize.Selected.Rows, DragDropEffects.Move);
					return;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugAllSize_SelectionDrag");
			}
		}

		private void ugAllSize_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				RaiseValueChanged();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugAllSize_AfterCellUpdate");
			}
		}

		private void ugAllSize_AfterRowsDeleted(object sender, System.EventArgs e)
		{
			try
			{
				RaiseValueChanged();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugAllSize_AfterRowsDeleted");
			}
		}

		private void ugAllSize_MouseEnterElement(object sender, UIElementEventArgs e)
		{
			try
			{

				UltraGridCell gridCell = (UltraGridCell)e.Element.GetContext(typeof(UltraGridCell));
				if (gridCell != null)
				{
					switch (gridCell.Column.Style)
					{
						case Infragistics.Win.UltraWinGrid.ColumnStyle.TriStateCheckBox:
						case Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox:
							ugAllSize.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
							break;
						default:
							switch (gridCell.Column.DataType.Name.ToUpper())
							{
								case "BOOLEAN":
									ugAllSize.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
									break;
								default:
									ugAllSize.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;
									break;
							}
							break;
					}
				}

				ShowUltraGridToolTip(ugAllSize, e);
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugAllSize_MouseEnterElement");
			}
		}

		private void ugAllSize_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			_gridKeyEvent = e;
		}

		private void ugAllSize_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			_gridKeyEvent = null;
		}

		private void ugAllSize_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{

					if (!_showContext)
					{
						return;
					}

					UltraGrid myGrid = (UltraGrid)sender;
					UltraGridRow activeRow = myGrid.ActiveRow;

					switch (activeRow.Band.Key.ToUpper())
					{
						case C_SET:
							if ((eSizeMethodRowType)(int)activeRow.Cells["ROW_TYPE_ID"].Value == eSizeMethodRowType.AllSize)
							{
								_setContext.MenuItems[C_CONTEXT_SET_ADD_CLR].Enabled = false;
							}
							else
							{
								_setContext.MenuItems[C_CONTEXT_SET_ADD_CLR].Enabled = true;
							}
							myGrid.ContextMenu = _setContext;
							break;
						case C_CLR_SZ_DIM:
						case C_ALL_CLR_SZ_DIM:
							myGrid.ContextMenu = _dimensionContext;
							break;
						case C_CLR_SZ:
						case C_ALL_CLR_SZ:
							myGrid.ContextMenu = _sizeContext;
							break;
						case C_SET_CLR:
						case C_SET_ALL_CLR:

							if ((eSizeMethodRowType)(int)activeRow.Cells["ROW_TYPE_ID"].Value == eSizeMethodRowType.Color)
							{
								_colorContext.MenuItems[C_CONTEXT_CLR_ADD_CLR].Enabled = true;
								_colorContext.MenuItems[C_CONTEXT_CLR_DELETE].Enabled = true;
							}
							else
							{
								_colorContext.MenuItems[C_CONTEXT_CLR_ADD_CLR].Enabled = false;
								_colorContext.MenuItems[C_CONTEXT_CLR_DELETE].Enabled = false;
							}

							myGrid.ContextMenu = _colorContext;
							break;
					}

				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugAllSize_MouseDown");
			}
		}

		private void ugAllSize_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			UIElement aUIElement;

			try
			{
				if (_gridKeyEvent == null || !_gridKeyEvent.Control)
				{
					aUIElement = ugAllSize.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

					if (aUIElement == null ||
						(UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow)) == null ||
						(UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell)) == null)
					{
						return;
					}

					ugAllSize.PerformAction(UltraGridAction.EnterEditMode);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugAllSize_MouseUp");
			}
		}

		private void ugAllSize_AfterRowUpdate(object sender, RowEventArgs e)
		{
			UltraGrid myGrid;
			DataRow[] rows;

			try
			{
				myGrid = (UltraGrid)sender;

				e.Row.Cells["IS_INHERITED"].Value = 0;
				e.Row.Cells["IS_INHERITED_FROM"].Value = Include.NoRID;

				switch (e.Row.Band.Key.ToUpper())
				{
					case C_SET_CLR:
					case C_ALL_CLR_SZ_DIM:
					case C_CLR_SZ_DIM:
						myGrid.Rows.Refresh(RefreshRow.ReloadData, true);
						myGrid.ActiveRow = e.Row;
						break;

					case C_ALL_CLR_SZ:
					case C_CLR_SZ:
						if (e.Row.Cells["SIZE_CODE_RID"].Value != DBNull.Value)
						{
							rows = _dtSizes.Select("SIZE_CODE_RID = " + Convert.ToInt32(e.Row.Cells["SIZE_CODE_RID"].Value));
							if (rows.Length > 0)
							{
								e.Row.Cells["SIZES_RID"].Value = rows[0]["SIZES_RID"];
							}
						}
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugAllSize_AfterRowUpdate");
			}
		}

		private void ugAllSize_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
		{
			try
			{
			}
			catch (Exception ex)
			{
				e.Cancel = true;
				HandleException(ex, "ugAllSize_BeforeExitEditMode");
			}
		}

		private void ugAllSize_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UltraGrid myGrid;
			UltraGridRow myRow;

			try
			{
				myGrid = (UltraGrid)sender;
				myRow = myGrid.ActiveRow;

				switch (myRow.Band.Key.ToUpper())
				{
					case C_CLR_SZ:
					case C_ALL_CLR_SZ:
						if (myGrid.ActiveCell.Column.Key == "SIZE_CODE_RID")
						{
							//SWAP THE VALUE LIST SO THAT THE CORRECT TEXT WILL DISPLAY
							myGrid.ActiveCell.ValueList = null;
						}
						break;
					case C_CLR_SZ_DIM:
					case C_ALL_CLR_SZ_DIM:
						if (myGrid.ActiveCell.Column.Key == "DIMENSIONS_RID")
						{
							//SWAP THE VALUE LIST SO THAT THE CORRECT TEXT WILL DISPLAY
							myGrid.ActiveCell.ValueList = null;
						}
						break;
					case C_SET_CLR:
						if (myGrid.ActiveCell.Column.Key == "COLOR_CODE_RID")
						{
							//SWAP THE VALUE LIST SO THAT THE CORRECT TEXT WILL DISPLAY
							myGrid.ActiveCell.ValueList = null;
						}
						break;
				}

			}
			catch (Exception ex)
			{
				e.Cancel = true;
				HandleException(ex, "ugAllSize_BeforeCellDeactivate");
			}
		}

		private void ugAllSize_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
		{
			try
			{
				if (!VerifyBeforeInsert(((UltraGrid)sender).ActiveRow))
				{
					e.Cancel = true;
				}
			}
			catch (Exception ex)
			{
				e.Cancel = true;
				HandleException(ex, "ugAllSize_BeforeRowUpdate");
			}
		}

		private void ugAllSize_BeforeRowInsert(object sender, BeforeRowInsertEventArgs e)
		{
			UltraGrid myGrid;
			UltraGridRow activeRow;

			try
			{
				myGrid = (UltraGrid)sender;
				activeRow = myGrid.ActiveRow;

				if (activeRow.IsAddRow)
				{
					activeRow.Update();
					ugAllSize.ActiveRow = activeRow;
				}

				//Fixes an issue with the SortIndicator being set.
				//***********************************************************
				if (activeRow.HasChild() && activeRow.Expanded == false)
				{
					activeRow.Expanded = true;
				}
				//***********************************************************

				if (!VerifyBeforeInsert(activeRow))
				{
					e.Cancel = true;
				}
			}
			catch (Exception ex)
			{
				e.Cancel = true;
				HandleException(ex, "ugAllSize_BeforeRowInsert");
			}
		}

		private void ugAllSize_AfterRowInsert(object sender, RowEventArgs e)
		{
			bool hasValues;
			UltraGrid myGrid;

			try
			{
				RaiseValueChanged();
				myGrid = (UltraGrid)sender;

				e.Row.Cells["IS_INHERITED"].Value = 0;
				e.Row.Cells["IS_INHERITED_FROM"].Value = Include.NoRID;

				switch (e.Row.Band.Key.ToUpper())
				{
				    case C_SET_CLR:
						e.Row.Cells["ROW_TYPE_ID"].Value = eSizeMethodRowType.Color;
						e.Row.Cells["SIZES_RID"].Value = -1;
				        e.Row.Cells["DIMENSIONS_RID"].Value = -1;
				        e.Row.Cells["SIZE_CODE_RID"].Value = -1;
				        break;
					case C_CLR_SZ_DIM:
						e.Row.Cells["ROW_TYPE_ID"].Value = eSizeMethodRowType.ColorSizeDimension;
						e.Row.Cells["SIZES_RID"].Value = -1;
						e.Row.Cells["SIZE_CODE_RID"].Value = -1;
						break;
					case C_ALL_CLR_SZ_DIM:
						e.Row.Cells["ROW_TYPE_ID"].Value = eSizeMethodRowType.AllColorSizeDimension;
						e.Row.Cells["SIZES_RID"].Value = -1;
						e.Row.Cells["SIZE_CODE_RID"].Value = -1;
						break;
					case C_CLR_SZ:
						e.Row.Cells["ROW_TYPE_ID"].Value = eSizeMethodRowType.ColorSize;
						break;
					case C_ALL_CLR_SZ:
						e.Row.Cells["ROW_TYPE_ID"].Value = eSizeMethodRowType.AllColorSize;
						break;
				}

				switch (e.Row.Band.Key.ToUpper())
				{
					case C_CLR_SZ:
					case C_ALL_CLR_SZ:
						hasValues = CreateSizeCellList(e.Row, myGrid.DisplayLayout.ValueLists["SizeCell"]);

						if (!hasValues)
						{
							MessageBox.Show("All available sizes are being used for this dimension.\nThis row will be removed.",
								"MID Advanced Allocation",
								MessageBoxButtons.OK);
							e.Row.Delete(false);
						}
						break;
					case C_CLR_SZ_DIM:
					case C_ALL_CLR_SZ_DIM:

						hasValues = CreateDimensionCellList(e.Row, myGrid.DisplayLayout.ValueLists["DimensionCell"]);

						if (!hasValues)
						{
							MessageBox.Show("All available dimensions are being used for this color.\nThis row will be removed.",
								"MID Advanced Allocation",
								MessageBoxButtons.OK);
							e.Row.Delete(false);
						}
						break;
					case C_SET_CLR:

						hasValues = CreateColorCellList(e.Row, myGrid.DisplayLayout.ValueLists["ColorCell"]);

						if (!hasValues)
						{
							MessageBox.Show("All available colors are being used for this set.\nThis row will be removed.",
								"MID Advanced Allocation",
								MessageBoxButtons.OK);
							e.Row.Delete(false);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				e.Row.Delete(false);
				HandleException(ex, "ugAllSize_AfterRowInsert");
			}
		}

		private void ugAllSize_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UltraGrid myGrid;
			UltraGridRow myRow;

			try
			{
				myGrid = (UltraGrid)sender;
				myRow = myGrid.ActiveRow;

				switch (myRow.Band.Key.ToUpper())
				{
					case C_CLR_SZ:
					case C_ALL_CLR_SZ:
						if (myGrid.ActiveCell.Column.Key == "SIZE_CODE_RID")
						{
							CreateSizeCellList(myRow, myGrid.DisplayLayout.ValueLists["SizeCell"]);

							myGrid.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
							myGrid.ActiveCell.ValueList = myGrid.DisplayLayout.ValueLists["SizeCell"];
						}
						break;
					case C_CLR_SZ_DIM:
					case C_ALL_CLR_SZ_DIM:
						if (myGrid.ActiveCell.Column.Key == "DIMENSIONS_RID")
						{
							CreateDimensionCellList(myRow, myGrid.DisplayLayout.ValueLists["DimensionCell"]);

							myGrid.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
							myGrid.ActiveCell.ValueList = myGrid.DisplayLayout.ValueLists["DimensionCell"];
						}
						break;
					case C_SET_CLR:
						if (myGrid.ActiveCell.Column.Key == "COLOR_CODE_RID")
						{
							CreateColorCellList(myRow, myGrid.DisplayLayout.ValueLists["ColorCell"]);

							myGrid.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
							myGrid.ActiveCell.ValueList = myGrid.DisplayLayout.ValueLists["ColorCell"];
						}
						break;
				}
			}
			catch (Exception ex)
			{
				e.Cancel = true;
				HandleException(ex, "ugAllSize_BeforeEnterEditMode");
			}
		}

		private void ugAllSize_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			try
			{
				SetQuantityActivation(e.Row);
				SetRowProperties(e.Row);
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugAllSize_InitializeRow");
			}
		}

		private void mnuApply_Click(object sender, System.EventArgs e)
		{
			UltraGridRow activeRow;

			try
			{
				activeRow = ugAllSize.ActiveRow;

				ugAllSize.EventManager.AllEventsEnabled = false;
				ugAllSize.PerformAction(UltraGridAction.ExitEditMode);
				ugAllSize.EventManager.AllEventsEnabled = true;

				switch (activeRow.Band.Key.ToUpper())
				{
					case C_SET:

						if ((eSizeMethodRowType)Convert.ToInt32(activeRow.Cells["ROW_TYPE_ID"].Value, CultureInfo.CurrentUICulture) == eSizeMethodRowType.AllSize)
						{
							CopyAllSizeData(activeRow);
						}
						else
						{
							CopySetData(activeRow);
						}
						break;

					case C_SET_ALL_CLR:

						CopyAllColorData(activeRow);
						break;

					case C_SET_CLR:

						CopyColorData(activeRow);
						break;

					case C_CLR_SZ:
						break;

					case C_ALL_CLR_SZ:
						break;

					case C_ALL_CLR_SZ_DIM:

						CopyAllColorDimensionData(activeRow);
						break;

					case C_CLR_SZ_DIM:

						CopyColorDimensionData(activeRow);
						break;
				}

				ugAllSize.UpdateData();
			}
			catch (Exception ex)
			{
				HandleException(ex, "mnuApply_Click");
			}
		}

		private void mnuClear_Click(object sender, System.EventArgs e)
		{
			UltraGridRow activeRow;

			try
			{
				activeRow = ugAllSize.ActiveRow;

				switch (activeRow.Band.Key.ToUpper())
				{
					case C_SET:

						if ((eSizeMethodRowType)Convert.ToInt32(activeRow.Cells["ROW_TYPE_ID"].Value, CultureInfo.CurrentUICulture) == eSizeMethodRowType.AllSize)
						{
							ClearAllSizeData(activeRow);
						}
						else
						{
							ClearSetData(activeRow);
						}
						break;

					case C_SET_ALL_CLR:

						ClearAllColorData(activeRow);
						break;

					case C_SET_CLR:

						ClearColorData(activeRow);
						break;

					case C_CLR_SZ:
						break;

					case C_ALL_CLR_SZ:
						break;

					case C_CLR_SZ_DIM:

						ClearColorDimensionData(activeRow);
						break;

					case C_ALL_CLR_SZ_DIM:

						ClearAllColorDimensionData(activeRow);
						break;
				}

				ugAllSize.UpdateData();
			}
			catch (Exception ex)
			{
				HandleException(ex, "mnuClear_Click");
			}
		}

		private void mnuAddChildRow_Click(object sender, System.EventArgs e)
		{
			UltraGridRow activeRow;

			try
			{
				activeRow = ugAllSize.ActiveRow;

				switch (activeRow.Band.Key.ToUpper())
				{
					case C_SET:

						//SET LEVEL HAS ALL COLOR AND COLOR BAND,
						//ADDNEW IS ONLY ALLOWED FOR COLOR.
						//******************************************
						activeRow.Update();
						ugAllSize.ActiveRow = activeRow;
						activeRow.ChildBands[C_SET_CLR].Band.AddNew();
						break;

					default:

						//ADD ROW TO ACTIVEROWS FIRST CHILD BAND.
						//********************************************
						activeRow.Update();
						ugAllSize.ActiveRow = activeRow;
						activeRow.ChildBands[0].Band.AddNew();
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "mnuAddChildRow_Click");
			}
		}

		private void mnuAddRow_Click(object sender, System.EventArgs e)
		{
			UltraGridRow activeRow;

			try
			{
				activeRow = ugAllSize.ActiveRow;

				if (activeRow.IsAddRow)
				{
					activeRow.Update();
					ugAllSize.ActiveRow = activeRow;
				}

				activeRow.Band.AddNew();
			}
			catch (Exception ex)
			{
				HandleException(ex, "mnuAddRow_Click");
			}
		}

		private void mnuDeleteRow_Click(object sender, System.EventArgs e)
		{
			UltraGridRow activeRow;

			try
			{
				activeRow = ugAllSize.ActiveRow;

				activeRow.Selected = true;
				ugAllSize.DeleteSelectedRows();
			}
			catch (Exception ex)
			{
				HandleException(ex, "mnuDeleteRow_Click");
			}
		}

		private void RaiseValueChanged()
		{
			try
			{
				if (ValueChanged != null)
				{
					ValueChanged(this);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void InitializeControl(SessionAddressBlock aSAB, DataTable dtColors, DataTable dtColorSizes, FunctionSecurityProfile aFunctionSecurity, Image aErrorImage, Image aInheritanceImage)
		{
			try
			{
				_SAB = aSAB;
				_dtColors = dtColors;
				_dtColorSizes = dtColorSizes;
				_errorImage = aErrorImage;
				_inheritanceImage = aInheritanceImage;
				_functionSecurity = aFunctionSecurity;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void BindControl(DataSet dsQuantities)
		{
			try
			{
				_dsQuantities = dsQuantities;

				this.ugAllSize.DataSource = null;
				this.ugAllSize.DataSource = _dsQuantities;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void UpdateData()
		{
			try
			{
				ugAllSize.UpdateData();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void CloseControl()
		{
			InfragisticsLayoutData layoutData;

			try
			{
				layoutData = new InfragisticsLayoutData();
				layoutData.InfragisticsLayout_Save(_SAB.ClientServerSession.UserRID, eLayoutID.sizeOutOfStockGrid, ugAllSize);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void DefaultGridLayout()
		{
			try
			{
				//Create any context menus that may be used on the grid.
				BuildContextMenus();

				//Set cancel update action
				ugAllSize.RowUpdateCancelAction = RowUpdateCancelAction.RetainDataAndActivation;

				//Create the Value List Collections
				ugAllSize.DisplayLayout.ValueLists.Clear();
				InitializeValueLists(ugAllSize);

				ugAllSize.DisplayLayout.AddNewBox.Hidden = false;
				ugAllSize.DisplayLayout.Override.SelectTypeCell = SelectType.ExtendedAutoDrag;

				PositionColumns();
				CustomizeColumns();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void BuildContextMenus()
		{
			MenuItem mnuSetApply;
			MenuItem mnuSetClear;
			MenuItem mnuSetSeparator1;
			MenuItem mnuSetAddColor;
			MenuItem mnuColorApply;
			MenuItem mnuColorClear;
			MenuItem mnuColorSeparator1;
			MenuItem mnuColorAddColor;
			MenuItem mnuColorAddSizeDim;
			MenuItem mnuColorSeparator2;
			MenuItem mnuColorDelete;
			MenuItem mnuSizeAddSize;
			MenuItem mnuSizeSeparator1;
			MenuItem mnuSizeDelete;
			MenuItem mnuDimensionApply;
			MenuItem mnuDimensionClear;
			MenuItem mnuDimensionSeparator1;
			MenuItem mnuDimensionAddSizeDim;
			MenuItem mnuDimensionAddSize;
			MenuItem mnuDimensionSeparator2;
			MenuItem mnuDimensionDelete;

			try
			{
				// SET LEVEL

				mnuSetApply = new MenuItem("Apply", new EventHandler(this.mnuApply_Click));
				mnuSetApply.Index = C_CONTEXT_SET_APPLY;
				mnuSetClear = new MenuItem("Clear", new EventHandler(this.mnuClear_Click));
				mnuSetClear.Index = C_CONTEXT_SET_CLEAR;
				mnuSetSeparator1 = new MenuItem("-");
				mnuSetSeparator1.Index = C_CONTEXT_SET_SEP1;
				mnuSetAddColor = new MenuItem("Add Color", new EventHandler(this.mnuAddChildRow_Click));
				mnuSetAddColor.Index = C_CONTEXT_SET_ADD_CLR;
				_setContext = new ContextMenu(new MenuItem[] {	
																 mnuSetApply, 
																 mnuSetClear, 
																 mnuSetSeparator1, 
																 mnuSetAddColor 
															 });

				// COLOR LEVEL

				mnuColorApply = new MenuItem("Apply", new EventHandler(this.mnuApply_Click));
				mnuColorApply.Index = C_CONTEXT_CLR_APPLY;
				mnuColorClear = new MenuItem("Clear", new EventHandler(this.mnuClear_Click));
				mnuColorClear.Index = C_CONTEXT_CLR_CLEAR;
				mnuColorSeparator1 = new MenuItem("-");
				mnuColorSeparator1.Index = C_CONTEXT_CLR_SEP1;
				mnuColorAddColor = new MenuItem("Add Color", new EventHandler(this.mnuAddRow_Click));
				mnuColorAddColor.Index = C_CONTEXT_CLR_ADD_CLR;
				mnuColorAddSizeDim = new MenuItem("Add Size Dimension", new EventHandler(this.mnuAddChildRow_Click));
				mnuColorAddSizeDim.Index = C_CONTEXT_CLR_ADD_SZ_DIM;
				mnuColorSeparator2 = new MenuItem("-");
				mnuColorSeparator2.Index = C_CONTEXT_CLR_SEP2;
				mnuColorDelete = new MenuItem("Delete", new EventHandler(this.mnuDeleteRow_Click));
				mnuColorDelete.Index = C_CONTEXT_CLR_DELETE;

				_colorContext = new ContextMenu(new MenuItem[]  {
																	mnuColorApply,
																	mnuColorClear,
																	mnuColorSeparator1,
																	mnuColorAddColor,
																	mnuColorAddSizeDim,
																	mnuColorSeparator2,
																	mnuColorDelete
																});

				// SIZE LEVEL

				mnuSizeAddSize = new MenuItem("Add Size", new EventHandler(this.mnuAddRow_Click));
				mnuSizeAddSize.Index = C_CONTEXT_SZ_ADD_SZ;
				mnuSizeSeparator1 = new MenuItem("-");
				mnuSizeSeparator1.Index = C_CONTEXT_SZ_SEP1;
				mnuSizeDelete = new MenuItem("Delete", new EventHandler(this.mnuDeleteRow_Click));
				mnuSizeDelete.Index = C_CONTEXT_SZ_DELETE;
				_sizeContext = new ContextMenu(new MenuItem[]  {   mnuSizeAddSize,
																   mnuSizeSeparator1,
																   mnuSizeDelete
															   });

				// DIMENSION LEVEL

				mnuDimensionApply = new MenuItem("Apply", new EventHandler(this.mnuApply_Click));
				mnuDimensionApply.Index = C_CONTEXT_DIM_APPLY;
				mnuDimensionClear = new MenuItem("Clear", new EventHandler(this.mnuClear_Click));
				mnuDimensionClear.Index = C_CONTEXT_DIM_CLEAR;
				mnuDimensionSeparator1 = new MenuItem("-");
				mnuDimensionSeparator1.Index = C_CONTEXT_DIM_SEP1;
				mnuDimensionAddSizeDim = new MenuItem("Add Size Dimension", new EventHandler(this.mnuAddRow_Click));
				mnuDimensionAddSizeDim.Index = C_CONTEXT_DIM_ADD_DIM;
				mnuDimensionAddSize = new MenuItem("Add Size", new EventHandler(this.mnuAddChildRow_Click));
				mnuDimensionAddSize.Index = C_CONTEXT_DIM_ADD_SZ;
				mnuDimensionSeparator2 = new MenuItem("-");
				mnuDimensionSeparator2.Index = C_CONTEXT_DIM_SEP2;
				mnuDimensionDelete = new MenuItem("Delete", new EventHandler(this.mnuDeleteRow_Click));
				mnuDimensionDelete.Index = C_CONTEXT_DIM_DELETE;
				_dimensionContext = new ContextMenu(new MenuItem[]  {
																		mnuDimensionApply,
																		mnuDimensionClear,
																		mnuDimensionSeparator1,
																		mnuDimensionAddSizeDim,
																		mnuDimensionAddSize,
																		mnuDimensionSeparator2,
																		mnuDimensionDelete
																	});
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void InitializeValueLists(UltraGrid myGrid)
		{
			try
			{
				myGrid.DisplayLayout.ValueLists.Add("Colors");
				myGrid.DisplayLayout.ValueLists.Add("Rules");
				myGrid.DisplayLayout.ValueLists.Add("SortOrder");
				myGrid.DisplayLayout.ValueLists.Add("Sizes");
				myGrid.DisplayLayout.ValueLists.Add("Dimensions");
				myGrid.DisplayLayout.ValueLists.Add("SizeCell");
				myGrid.DisplayLayout.ValueLists.Add("DimensionCell");
				myGrid.DisplayLayout.ValueLists.Add("ColorCell");
				myGrid.DisplayLayout.ValueLists["ColorCell"].SortStyle = ValueListSortStyle.Ascending;

				FillColorList(myGrid.DisplayLayout.ValueLists["Colors"]);
				FillSizesList(myGrid.DisplayLayout.ValueLists["Sizes"]);
				FillSizeDimensionList(myGrid.DisplayLayout.ValueLists["Dimensions"]);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void FillColorList(ValueList valueList)
		{
			DataColumn dataColumn;
			DataRow newRow;

			try
			{
				valueList.ValueListItems.Clear();

				foreach (DataRow row in _dtColors.Rows)
				{
					valueList.ValueListItems.Add(Convert.ToInt32(row["COLOR_CODE_RID"], CultureInfo.CurrentUICulture),
						row["COLOR_CODE_ID"].ToString() + " - " + row["COLOR_CODE_NAME"].ToString());
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void FillSizesList(ValueList valueList)
		{
			DataRow newRow;

			try
			{
				valueList.ValueListItems.Clear();
				_dtSizes = CreateSizeDataTable();

				foreach (DataRow row in _dtColorSizes.Rows)
				{
					if (_dtSizes.Select("SIZES_RID = " + Convert.ToInt32(row["SIZES_RID"])).Length == 0)
					{
						newRow = _dtSizes.NewRow();
						newRow["SIZES_RID"] = row["SIZES_RID"];
						newRow["SIZE_CODE_RID"] = row["SIZE_CODE_RID"];
						newRow["SIZE_CODE_PRIMARY"] = row["SIZE_CODE_PRIMARY"];
						_dtSizes.Rows.Add(newRow);
					}
				}

				foreach (DataRow row in _dtSizes.Rows)
				{
					valueList.ValueListItems.Add(Convert.ToInt32(row["SIZE_CODE_RID"]), row["SIZE_CODE_PRIMARY"].ToString());
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void FillSizeDimensionList(ValueList valueList)
		{
			DataTable dtDimensions;
			DataRow newRow;

			try
			{
				valueList.ValueListItems.Clear();
				dtDimensions = CreateDimensionDataTable();

				foreach (DataRow row in _dtColorSizes.Rows)
				{
					if (dtDimensions.Select("DIMENSIONS_RID = " + Convert.ToInt32(row["DIMENSIONS_RID"])).Length == 0)
					{
						newRow = dtDimensions.NewRow();
						newRow["DIMENSIONS_RID"] = row["DIMENSIONS_RID"];
						newRow["SIZE_CODE_SECONDARY"] = row["SIZE_CODE_SECONDARY"];
						dtDimensions.Rows.Add(newRow);
					}
				}

				foreach (DataRow row in dtDimensions.Rows)
				{
					valueList.ValueListItems.Add(Convert.ToInt32(row["DIMENSIONS_RID"]), row["SIZE_CODE_SECONDARY"].ToString());
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void PositionColumns()
		{
			UltraGridColumn column;

			try
			{
				// SET BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_SET))
				{
					column = ugAllSize.DisplayLayout.Bands[C_SET].Columns["BAND_DSC"];
					column.Header.VisiblePosition = 0;
					column.Header.Caption = "Store Sets";
					column.Width = 200;

					column = ugAllSize.DisplayLayout.Bands[C_SET].Columns["OOS_QUANTITY"];
					column.Header.VisiblePosition = 4;
					column.Header.Caption = "Qty";

					ugAllSize.DisplayLayout.Bands[C_SET].Columns["SGL_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET].Columns["SGL_SEQUENCE"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET].Columns["ROW_TYPE_ID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET].Columns["IS_INHERITED"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET].Columns["IS_INHERITED_FROM"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET].Override.AllowDelete = DefaultableBoolean.False;
					ugAllSize.DisplayLayout.Bands[C_SET].AddButtonCaption = "Set";
				}

				// ALL COLOR BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_SET_ALL_CLR))
				{
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["BAND_DSC"].Header.VisiblePosition = 0;

					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["OOS_QUANTITY"].Header.VisiblePosition = 1;

					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SGL_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["ROW_TYPE_ID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["COLOR_CODE_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZES_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_CODE_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_SEQUENCE"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["DIMENSIONS_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["IS_INHERITED"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["IS_INHERITED_FROM"].Hidden = true;

					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].ColHeadersVisible = false;
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].Override.AllowDelete = DefaultableBoolean.False;
					ugAllSize.DisplayLayout.Bands[C_SET_ALL_CLR].AddButtonCaption = "All Color";
				}

				// COLOR BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_SET_CLR))
				{
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["COLOR_CODE_RID"].Header.VisiblePosition = 0;
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["OOS_QUANTITY"].Header.VisiblePosition = 1;

					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["SGL_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["ROW_TYPE_ID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["SIZES_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_CODE_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_SEQUENCE"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["DIMENSIONS_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["BAND_DSC"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["IS_INHERITED"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["IS_INHERITED_FROM"].Hidden = true;

					ugAllSize.DisplayLayout.Bands[C_SET_CLR].ColHeadersVisible = false;
					ugAllSize.DisplayLayout.Bands[C_SET_CLR].AddButtonCaption = "Color";
				}

				// COLOR SIZE BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_CLR_SZ))
				{

					column = ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Header.VisiblePosition = 0;

					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["OOS_QUANTITY"].Header.VisiblePosition = 1;

					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["DIMENSIONS_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZES_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["SGL_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["ROW_TYPE_ID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_SEQUENCE"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["COLOR_CODE_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["BAND_DSC"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["IS_INHERITED"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["IS_INHERITED_FROM"].Hidden = true;

					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].ColHeadersVisible = false;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ].AddButtonCaption = "Size";
				}

				// ALL COLOR SIZE BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ))
				{
					column = ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Header.VisiblePosition = 0;

					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["OOS_QUANTITY"].Header.VisiblePosition = 1;

					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZES_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SGL_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["DIMENSIONS_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["ROW_TYPE_ID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_SEQUENCE"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["COLOR_CODE_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["BAND_DSC"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["IS_INHERITED"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["IS_INHERITED_FROM"].Hidden = true;

					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].ColHeadersVisible = false;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].AddButtonCaption = "Size";
				}

				// ALL COLOR DIMENSION BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ_DIM))
				{
					column = ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Header.VisiblePosition = 0;

					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["OOS_QUANTITY"].Header.VisiblePosition = 1;

					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZES_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_CODE_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SGL_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["ROW_TYPE_ID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_SEQUENCE"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["COLOR_CODE_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["BAND_DSC"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["IS_INHERITED"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["IS_INHERITED_FROM"].Hidden = true;

					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].ColHeadersVisible = false;
					ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].AddButtonCaption = "Size Dimension";

				}

				// COLOR DIMENSION BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_CLR_SZ_DIM))
				{
					column = ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Header.VisiblePosition = 0;

					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["OOS_QUANTITY"].Header.VisiblePosition = 1;

					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZES_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_CODE_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SGL_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["ROW_TYPE_ID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_SEQUENCE"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["COLOR_CODE_RID"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["BAND_DSC"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["IS_INHERITED"].Hidden = true;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["IS_INHERITED_FROM"].Hidden = true;

					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].ColHeadersVisible = false;
					ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].AddButtonCaption = "Size Dimension";
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void CustomizeColumns()
		{
			UltraGridColumn column;

			try
			{
				// COLOR

				if (ugAllSize.DisplayLayout.Bands.Exists(C_SET_CLR))
				{
					column = ugAllSize.DisplayLayout.Bands[C_SET_CLR].Columns["COLOR_CODE_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugAllSize.DisplayLayout.ValueLists["Colors"];
				}

				// COLOR SIZE BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_CLR_SZ))
				{
					column = ugAllSize.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugAllSize.DisplayLayout.ValueLists["Sizes"];
				}

				// ALL COLOR SIZE BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ))
				{
					column = ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugAllSize.DisplayLayout.ValueLists["Sizes"];
				}

				// COLOR DIM BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_CLR_SZ_DIM))
				{
					column = ugAllSize.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugAllSize.DisplayLayout.ValueLists["Dimensions"];
				}

				// ALL COLOR DIM BAND

				if (ugAllSize.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ_DIM))
				{
					column = ugAllSize.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugAllSize.DisplayLayout.ValueLists["Dimensions"];
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Serves two functions:
		/// 1) Returns boolean to the ugAllSize_AfterRowInsert event which determines if a new size should be allowed.
		/// 2) Creates the "SizeCell" value list to be used for the Size dropdown.
		/// </summary>
		/// <param name="myRow"></param>
		/// <param name="childBand"></param>
		/// <returns></returns>
		private bool CreateColorCellList(UltraGridRow myRow, ValueList valueList)
		{
			bool hasValues;
			bool addVal;

			try
			{
				hasValues = false;
				valueList.ValueListItems.Clear();

				foreach (DataRow row in _dtColors.Rows)
				{
					addVal = true;

					foreach (UltraGridRow ugRow in myRow.ParentRow.ChildBands[myRow.Band].Rows)
					{
						if (ugRow.Cells["COLOR_CODE_RID"].Value != System.DBNull.Value)
						{
							if (Convert.ToInt32(ugRow.Cells["COLOR_CODE_RID"].Value) == Convert.ToInt32(row["COLOR_CODE_RID"]))
							{
								if (ugRow.Index != myRow.Index)
								{
									addVal = false;
								}

								break;
							}
						}
					}

					if (addVal)
					{
						valueList.ValueListItems.Add(Convert.ToInt32(row["COLOR_CODE_RID"]), row["COLOR_CODE_ID"].ToString() + " - " + row["COLOR_CODE_NAME"].ToString());
						hasValues = true;
					}
				}

				return hasValues;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Serves two functions:
		/// 1) Returns boolean to the ugAllSize_AfterRowInsert event which determines if a new size should be allowed.
		/// 2) Creates the "SizeCell" value list to be used for the Size dropdown.
		/// </summary>
		/// <param name="myRow"></param>
		/// <param name="childBand"></param>
		/// <returns></returns>
		private bool CreateDimensionCellList(UltraGridRow myRow, ValueList valueList)
		{
			bool hasValues;
			DataTable dtDimensions;
			DataRow[] rows;
			bool addVal;
			DataRow newRow;

			try
			{
				hasValues = false;
				valueList.ValueListItems.Clear();
				dtDimensions = CreateDimensionDataTable();

				if (Convert.ToInt32(myRow.Cells["COLOR_CODE_RID"].Value) != Include.NoRID)
				{
					rows = _dtColorSizes.Select("COLOR_CODE_RID = " + Convert.ToInt32(myRow.Cells["COLOR_CODE_RID"].Value));
				}
				else
				{
					rows = _dtColorSizes.Select();
				}

				foreach (DataRow row in rows)
				{
					if (dtDimensions.Select("DIMENSIONS_RID = " + Convert.ToInt32(row["DIMENSIONS_RID"])).Length == 0)
					{
						addVal = true;

						foreach (UltraGridRow ugRow in myRow.ParentRow.ChildBands[myRow.Band].Rows)
						{
							if (ugRow.Cells["DIMENSIONS_RID"].Value != System.DBNull.Value)
							{
								if (Convert.ToInt32(ugRow.Cells["DIMENSIONS_RID"].Value) == Convert.ToInt32(row["DIMENSIONS_RID"]))
								{
									if (ugRow.Index != myRow.Index)
									{
										addVal = false;
									}

									break;
								}
							}
						}

						if (addVal)
						{
							newRow = dtDimensions.NewRow();
							newRow["DIMENSIONS_RID"] = row["DIMENSIONS_RID"];
							newRow["SIZE_CODE_SECONDARY"] = row["SIZE_CODE_SECONDARY"];
							dtDimensions.Rows.Add(newRow);

							hasValues = true;
						}
					}
				}

				foreach (DataRow row in dtDimensions.Rows)
				{
					valueList.ValueListItems.Add(Convert.ToInt32(row["DIMENSIONS_RID"]), row["SIZE_CODE_SECONDARY"].ToString());
				}

				return hasValues;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Serves two functions:
		/// 1) Returns boolean to the ugAllSize_AfterRowInsert event which determines if a new size should be allowed.
		/// 2) Creates the "SizeCell" value list to be used for the Size dropdown.
		/// </summary>
		/// <param name="myRow"></param>
		/// <param name="childBand"></param>
		/// <returns></returns>
		private bool CreateSizeCellList(UltraGridRow myRow, ValueList valueList)
		{
			bool hasValues;
			DataRow[] rows;
			bool addVal;
			DataRow newRow;

			try
			{
				hasValues = false;
				valueList.ValueListItems.Clear();
				_dtSizes = CreateSizeDataTable();

				if (Convert.ToInt32(myRow.Cells["COLOR_CODE_RID"].Value) != Include.NoRID)
				{
					rows = _dtColorSizes.Select("COLOR_CODE_RID = " + Convert.ToInt32(myRow.Cells["COLOR_CODE_RID"].Value) +
												" AND DIMENSIONS_RID = " + Convert.ToInt32(myRow.Cells["DIMENSIONS_RID"].Value));
				}
				else
				{
					rows = _dtColorSizes.Select("DIMENSIONS_RID = " + Convert.ToInt32(myRow.Cells["DIMENSIONS_RID"].Value));
				}

				foreach (DataRow row in rows)
				{
					if (_dtSizes.Select("SIZES_RID = " + Convert.ToInt32(row["SIZES_RID"])).Length == 0)
					{
						addVal = true;

						foreach (UltraGridRow ugRow in myRow.ParentRow.ChildBands[myRow.Band].Rows)
						{
							if (ugRow.Cells["SIZE_CODE_RID"].Value != System.DBNull.Value)
							{
								if (Convert.ToInt32(ugRow.Cells["SIZE_CODE_RID"].Value) == Convert.ToInt32(row["SIZE_CODE_RID"]))
								{
									if (ugRow.Index != myRow.Index)
									{
										addVal = false;
									}

									break;
								}
							}
						}

						if (addVal)
						{
							newRow = _dtSizes.NewRow();
							newRow["SIZES_RID"] = row["SIZES_RID"];
							newRow["SIZE_CODE_RID"] = row["SIZE_CODE_RID"];
							newRow["SIZE_CODE_PRIMARY"] = row["SIZE_CODE_PRIMARY"];
							_dtSizes.Rows.Add(newRow);

							hasValues = true;
						}
					}
				}

				foreach (DataRow row in _dtSizes.Rows)
				{
					valueList.ValueListItems.Add(Convert.ToInt32(row["SIZE_CODE_RID"]), row["SIZE_CODE_PRIMARY"].ToString());
				}

				return hasValues;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private DataTable CreateSizeDataTable()
		{
			DataTable dtSizes;
			DataColumn dataColumn;

			try
			{
				dtSizes = MIDEnvironment.CreateDataTable();

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "SIZES_RID";
				dtSizes.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "SIZE_CODE_RID";
				dtSizes.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "SIZE_CODE_PRIMARY";
				dtSizes.Columns.Add(dataColumn);

				return dtSizes;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private DataTable CreateDimensionDataTable()
		{
			DataTable dtDimensions;
			DataColumn dataColumn;

			try
			{
				dtDimensions = MIDEnvironment.CreateDataTable();

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "DIMENSIONS_RID";
				dtDimensions.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "SIZE_CODE_SECONDARY";
				dtDimensions.Columns.Add(dataColumn);

				return dtDimensions;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// walks through the grid and sets the quantity fields to editable or disabled
		/// depending upon the rule value.
		/// </summary>
		public void SetEditableQuantityCells()
		{
			try
			{
				//================================================================================
				//WALK THE GRID - Checking EACH ROW
				//================================================================================
				foreach (UltraGridRow setRow in ugAllSize.Rows)
				{
					SetQuantityActivation(setRow);

					if (setRow.HasChild())
					{
						//ALL COLORS ROW
						//===============
						foreach (UltraGridRow allColorRow in setRow.ChildBands[C_SET_ALL_CLR].Rows)
						{
							SetQuantityActivation(allColorRow);

							if (allColorRow.HasChild())
							{
								//ALL COLOR DIMENSION ROWS
								foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[C_ALL_CLR_SZ_DIM].Rows)
								{
									SetQuantityActivation(allColorDimRow);

									//ALL COLOR DIMENSION/SIZE ROWS
									if (allColorDimRow.HasChild())
									{
										foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[C_ALL_CLR_SZ].Rows)
										{
											SetQuantityActivation(allColorSizeRow);
										}
									}
								}
							}
						}
						//===========
						//COLOR ROWS 
						//===========
						foreach (UltraGridRow colorRow in setRow.ChildBands[C_SET_CLR].Rows)
						{
							SetQuantityActivation(colorRow);

							if (colorRow.HasChild())
							{
								//COLOR SIZE
								//=============
								foreach (UltraGridRow colorDimRow in colorRow.ChildBands[C_CLR_SZ_DIM].Rows)
								{
									SetQuantityActivation(colorDimRow);

									if (colorDimRow.HasChild())
									{
										//COLOR SIZE DIMENSION
										//======================
										foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[C_CLR_SZ].Rows)
										{
											SetQuantityActivation(colorSizeRow);
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void SetQuantityActivation(UltraGridRow aRow)
		{
			try
			{
				//if (aRow.Cells["OOS_QUANTITY"].Hidden == false)
				//{
				//    aRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;
				//    aRow.Cells["OOS_QUANTITY"].Activation = Activation.NoEdit;
				//}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void SetRowProperties(UltraGridRow aRow)
		{
			int inheritedRID;
			HierarchyNodeProfile hnp;
			string inheritMsg;

			try
			{
				if (aRow.Cells["IS_INHERITED"].Value != DBNull.Value && Convert.ToInt32(aRow.Cells["IS_INHERITED"].Value) == 1)
				{
					inheritedRID = Convert.ToInt32(aRow.Cells["IS_INHERITED_FROM"].Value, CultureInfo.CurrentUICulture);
					hnp = GetNodeProfile(inheritedRID);
					inheritMsg = C_INHERITED_FROM + hnp.Text;

					aRow.Cells["OOS_QUANTITY"].Appearance.Image = _inheritanceImage;
					aRow.Cells["OOS_QUANTITY"].Tag = inheritMsg;
				}
				else
				{
					aRow.Cells["OOS_QUANTITY"].Appearance.Image = null;
					aRow.Cells["OOS_QUANTITY"].Tag = null;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private HierarchyNodeProfile GetNodeProfile(int aKey)
		{
			HierarchyNodeProfile hnp;

			try
			{
				hnp = (HierarchyNodeProfile)_hnpHash[aKey];

				if (hnp == null)
				{
					hnp = _SAB.HierarchyServerSession.GetNodeData(aKey, false);
					_hnpHash.Add(aKey, hnp);
				}

				return hnp;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private bool VerifyBeforeInsert(UltraGridRow activeRow)
		{
			bool isValid;

			try
			{
				isValid = true;

				switch (activeRow.Band.Key.ToUpper())
				{
					case C_SET_CLR:
						isValid = IsColorCodeValid(activeRow);
						break;
					case C_ALL_CLR_SZ_DIM:
					case C_CLR_SZ_DIM:
						isValid = IsDimensionValid(activeRow);
						break;
					case C_CLR_SZ:
					case C_ALL_CLR_SZ:
						isValid = IsSizeValid(activeRow);
						break;
				}

				return isValid;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Validates the color code rid cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		private bool IsColorCodeValid(UltraGridRow thisRow)
		{
			bool isValid;
			UltraGridCell activeCell;

			try
			{
				isValid = true;

				ErrorMessages.Clear();
				activeCell = thisRow.Cells["COLOR_CODE_RID"];

				activeCell.Appearance.Image = null;
				activeCell.Tag = null;

				if (activeCell.Value == System.DBNull.Value)
				{
					isValid = false;
					ErrorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorRequired));
				}

				if (!isValid)
				{
					AttachErrors(activeCell);
				}

				return isValid;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Validates the sizes_rid cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		private bool IsSizeValid(UltraGridRow thisRow)
		{
			bool isValid;
			UltraGridCell activeCell;

			try
			{
				isValid = true;

				ErrorMessages.Clear();
				activeCell = thisRow.Cells["SIZE_CODE_RID"];

				activeCell.Appearance.Image = null;
				activeCell.Tag = null;

				if (activeCell.Value == System.DBNull.Value)
				{
					isValid = false;
					ErrorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeRequired));
				}

				if (!isValid)
				{
					AttachErrors(activeCell);
				}

				return isValid;
			}
			catch (Exception ex)
			{
				throw;
			}
		}


		/// <summary>
		/// Validates the size_code_rid cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns></returns>
		private bool IsDimensionValid(UltraGridRow thisRow)
		{
			bool isValid;
			UltraGridCell activeCell;

			try
			{
				isValid = true;

				ErrorMessages.Clear();
				activeCell = thisRow.Cells["DIMENSIONS_RID"];

				activeCell.Appearance.Image = null;
				activeCell.Tag = null;

				if (activeCell.Value == System.DBNull.Value)
				{
					isValid = false;
					ErrorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeDimensionRequired));
				}

				if (!isValid)
				{
					AttachErrors(activeCell);
				}

				return isValid;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Used to attach an error image and tag values to a UltraGridCell.  Tag may contain multiple error messages.
		/// </summary>
		/// <param name="activeCell"></param>
		private void AttachErrors(UltraGridCell activeCell)
		{
			int errIdx;

			try
			{
				activeCell.Appearance.Image = _errorImage;

				for (errIdx = 0; errIdx <= ErrorMessages.Count - 1; errIdx++)
				{
					activeCell.Tag = (errIdx == 0) ? ErrorMessages[errIdx] : activeCell.Tag + "\n" + ErrorMessages[errIdx];
				}

				ErrorMessages.Clear();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void CopyAllSizeData(UltraGridRow activeRow)
		{
			throw new Exception("Can not call base method CopyAllSizeData(UltraGridRow activeRow).");
		}

		private void ClearAllSizeData(UltraGridRow activeRow)
		{
			throw new Exception("Can not call base method ClearAllSizeData(UltraGridRow activeRow).");
		}

		private void ClearSetData(UltraGridRow activeRow)
		{
			throw new Exception("Can not call base method ClearSetData(UltraGridRow activeRow).");
		}

		private void CopySetData(UltraGridRow activeRow)
		{
			try
			{
				if (activeRow.HasChild())
				{
					//COPY TO ALL COLOR ROW
					foreach (UltraGridRow allColorRow in activeRow.ChildBands[0].Rows)
					{
						allColorRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;


						if (allColorRow.HasChild())
						{
							foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
							{
								allColorDimRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;

								if (allColorDimRow.HasChild())
								{
									foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
									{
										allColorSizeRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;
									}
								}
							}
						}
					}

					//COPY TO COLOR
					foreach (UltraGridRow colorRow in activeRow.ChildBands[1].Rows)
					{
						colorRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;


						if (colorRow.HasChild())
						{
							foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
							{
								colorDimRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;

								if (colorDimRow.HasChild())
								{
									foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
									{
										colorSizeRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void CopyColorData(UltraGridRow activeRow)
		{
			try
			{
				if (activeRow.HasChild())
				{
					foreach (UltraGridRow colorDimRow in activeRow.ChildBands[0].Rows)
					{
						colorDimRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;

						if (colorDimRow.HasChild())
						{
							foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
							{
								colorSizeRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void ClearColorData(UltraGridRow activeRow)
		{
			try
			{
				activeRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;

				if (activeRow.HasChild())
				{
					foreach (UltraGridRow colorDimRow in activeRow.ChildBands[0].Rows)
					{
						colorDimRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;

						if (colorDimRow.HasChild())
						{
							foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
							{
								colorSizeRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void CopyAllColorData(UltraGridRow activeRow)
		{
			try
			{
				if (activeRow.HasChild())
				{
					foreach (UltraGridRow allColorDimRow in activeRow.ChildBands[0].Rows)
					{
						allColorDimRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;

						if (allColorDimRow.HasChild())
						{
							foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
							{
								allColorSizeRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void ClearAllColorData(UltraGridRow activeRow)
		{
			try
			{
				activeRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;

				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach (UltraGridRow allColorDimRow in activeRow.ChildBands[0].Rows)
					{
						allColorDimRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;

						if (allColorDimRow.HasChild())
						{
							foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
							{
								allColorSizeRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void CopyAllColorDimensionData(UltraGridRow activeRow)
		{
			try
			{
				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach (UltraGridRow allColorSizeRow in activeRow.ChildBands[0].Rows)
					{
						allColorSizeRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void ClearAllColorDimensionData(UltraGridRow activeRow)
		{
			try
			{
				activeRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;

				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach (UltraGridRow allColorSizeRow in activeRow.ChildBands[0].Rows)
					{
						allColorSizeRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void CopyColorDimensionData(UltraGridRow activeRow)
		{
			try
			{
				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach (UltraGridRow colorSizeRow in activeRow.ChildBands[0].Rows)
					{
						colorSizeRow.Cells["OOS_QUANTITY"].Value = activeRow.Cells["OOS_QUANTITY"].Value;
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void ClearColorDimensionData(UltraGridRow activeRow)
		{
			try
			{
				activeRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;

				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach (UltraGridRow colorSizeRow in activeRow.ChildBands[0].Rows)
					{
						colorSizeRow.Cells["OOS_QUANTITY"].Value = DBNull.Value;
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void HandleException(Exception ex, string moduleName)
		{
			_SAB.ClientServerSession.Audit.Log_Exception(ex, moduleName);
			MessageBox.Show(ex.ToString(), "MIDFormBase.cs - " + moduleName);
		}

		/// <summary>
		/// Shows ToolTip to display error message in an UntraGrid cell 
		/// </summary>
		/// <param name="ultraGrid">The UltraGrid where the tool tip is to be displayed</param>
		/// <param name="e">The UIElementEventArgs arguments of the MouseEnterElement event</param>
		private void ShowUltraGridToolTip(Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid, Infragistics.Win.UIElementEventArgs e)
		{
			UltraGridCell gridCell;
			GridCellTag gct;

			try
			{
				if (this.toolTip1 != null && this.toolTip1.Active)
				{
					this.toolTip1.Active = false;
				}

				gridCell = (UltraGridCell)e.Element.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell));

				if (gridCell != null)
				{
					if (gridCell.Tag != null)
					{
						if (gridCell.Tag.GetType() == typeof(System.String))
						{
							toolTip1.Active = true;
							toolTip1.SetToolTip(ultraGrid, (string)gridCell.Tag);
						}
                        else if (gridCell.Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
						{
							gct = (GridCellTag)gridCell.Tag;

							if (gct.Message != null)
							{
								toolTip1.Active = true;
								toolTip1.SetToolTip(ultraGrid, gct.Message);
							}
							else if (gct.HelpText != null)
							{
								toolTip1.Active = true;
								toolTip1.SetToolTip(ultraGrid, gct.HelpText);
							}
							else
							{
								toolTip1.Active = false;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
