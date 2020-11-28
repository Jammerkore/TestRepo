using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Text;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
//BEGIN TT#3 -MD- DOConnell - Export does not produce output
using Infragistics.Win.UltraWinGrid.ExcelExport;
using System.IO;
//End TT#3 -MD- DOConnell - Export does not produce output
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public partial class GroupAllocationView : MIDFormBase, IFormBase
	{
		#region Variable Declarations

		public event System.Windows.Forms.MouseEventHandler g2MouseUpRefireHandler;
		public event System.Windows.Forms.MouseEventHandler g3MouseUpRefireHandler;

		private const bool THREADED_GRID_LOAD = false;
		private const int BIGCHANGE = 5;
		private const int SMALLCHANGE = 1;
		private const int ROWPAGESIZE = 90;
		private const int COLPAGESIZE = 30;
		private const int MINCOLSIZE = 6;
		private const int FIXEDCOLHEADERS = 3;
		private const int FIXEDROWHEADERS = 1;
		private const string NULL_DATA_STRING = " ";
		private const int HIGHNODECOMBOKEY = -2;
		private const int LOWLEVELTOTALCOMBOKEY = -1;
		private const int MAXTOTALCOLUMNS = 3;

		//private const int Grid1 = 0;
		//private const int Grid2 = 1;
		//private const int Grid3 = 2;
		//private const int Grid4 = 3;
		//private const int Grid5 = 4;
		//private const int Grid6 = 5;
		//private const int Grid7 = 6;
		//private const int Grid8 = 7;
		//private const int Grid9 = 8;
		//private const int Grid10 = 9;
		//private const int Grid11 = 10;
		//private const int Grid12 = 11;

		private FunctionSecurityProfile _allocationReviewSummarySecurity;
		private FunctionSecurityProfile _allocationReviewStyleSecurity;
		private FunctionSecurityProfile _allocationReviewSizeSecurity;
		//private FunctionSecurityProfile _assortReviewAssortmentSecurity;
		//private FunctionSecurityProfile _assortmentSecurity;
		//private FunctionSecurityProfile _assortReviewCharacteristicSecurity;

		private SessionAddressBlock _sab;
		private ExplorerAddressBlock _eab;
		private ApplicationSessionTransaction _transaction;
		private eAssortmentWindowType _windowType;

		//private AssortmentComponentVariables _componentVariables;
		//private AssortmentVariables _summaryVariables;
		//private AssortmentVariables _detailVariables;
		//private AssortmentVariables _totalVariables;
		//private AssortmentQuantityVariables _quantityVariables;

		//private UserAssortment _dlUserAssrt;
		private DataTable _dtHeaders;
		private ProfileList _storeProfileList;
		private ProfileList _storeGroupListViewProfileList;
		private ProfileList _storeGroupLevelProfileList;
		private ProfileList _storeGradeProfileList;
		private ProfileList _componentColumnProfileList;
		private ProfileList _summaryRowProfileList;
		private ProfileList _totalColumnProfileList;
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		private ProfileList _planRowProfileList;
		//End TT#2 - JScott - Assortment Planning - Phase 2
		private ProfileList _detailColumnProfileList;
		private ProfileList _detailRowProfileList;

		//private ArrayList _selectableComponentColumnHeaders;
		//private ArrayList _selectableSummaryRowHeaders;
		//private ArrayList _selectableTotalColumnHeaders;
		//private ArrayList _selectableTotalRowHeaders;	// TT#1224 - stodd - committed
		//private ArrayList _selectableDetailColumnHeaders;
		//private ArrayList _selectableDetailRowHeaders;
		//private ArrayList _selectableStoreGradeHeaders;

		//private SortedList _sortedComponentColumnHeaders;
		//private SortedList _sortedSummaryRowHeaders;
		//private SortedList _sortedTotalColumnHeaders;
		//private SortedList _sortedDetailColumnHeaders;
		//private SortedList _sortedDetailRowHeaders;
		//private SortedList _sortedStoreGradeHeaders;

		private eAllocationAssortmentViewGroupBy _columnGroupedBy;

		//private RowColProfileHeader _currStoreGroupLevelHeader;
		//private StoreGroupLevelProfile _currStoreGroupLevelProfile;

		private StoreFilterData _storeFilterDL;
		private string _windowName;

		private SortedList _row2LineList;
		private SortedList _row3LineList;

		private int _headerCol;
		private int _placeholderCol;
		private int _packCol;
		// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
		private int _highestPlaceholderHeaderCol;
		private bool _containsBothPlaceholderAndHeader;
		// END TT#2150 - stodd - totals not showing in main matrix grid
		//private bool _isHeaderSummarized;

		//private AssortmentOpenParms _openParms;
		private AllocationHeaderProfileList _headerList;
		//private DataTable _assrtViewDetail;

		private AssortmentViewSave _saveForm;

		//private int _colsSummarized;
		//==================

		//private FunctionSecurityProfile _forecastBalanceSecurity;

		//private PlanCubeGroup _planCubeGroup;
		//private AssortmentCubeGroup _asrtCubeGroup;
		private ProfileList _workingDetailProfileList;
		//private ProfileList _variableProfileList;
		//private ProfileList _quantityVariableProfileList;
		//private ProfileList _versionProfileList;
		//private IPlanComputationQuantityVariables _quantityVariables;
		//private PlanOpenParms _openParms;
		private ArrayList _userRIDList;
		//private CubeWaferCoordinateList _commonWaferCoordinateList;
		private AssortmentViewData _assortmentViewData;
		//private Hashtable _basisToolTipList;
		//private bool _storeReadOnly;
		//private bool _chainReadOnly;
		//private bool _lowLevelStoreReadOnly;
		//private bool _lowLevelChainReadOnly;

		private MIDReaderWriterLock _pageLoadLock;
		private Hashtable _loadHash;
		private bool _currentRedrawState;
		private bool _formLoading;
		private bool _bindingView;
		private bool _bindingStoreGroup;
		private bool _bindingStoreGroupLevel;
		private bool _bindingFilter;
		private bool _stopPageLoadThread;
		private bool _hSplitMove;
		private bool _isScrolling;
		private bool _isSorting;
		private bool _holdingScroll;
		private bool _placeholderSelected;
		private int _currColSplitPosition2;
		private int _currColSplitPosition3;
		private int _currRowSplitPosition4;
		private int _currRowSplitPosition12;
		private int _lastStoreGroupValue;
		private int _lastStoreGroupLevelValue;
		private int _lastViewValue;
		private int _lastFilterValue;
		private SplitterTag _currSplitterTag;

		private int _dragStartColumn; //indicates which column is the column that started the drag/drop action.
		private DragState _dragState;
		private C1FlexGrid _rightClickedFrom;
		private Rectangle _dragBoxFromMouseDown;
		private bool _mouseDown = false;

		private Bitmap _picLock; //this picture will be put in a cell that's locked.
		private Bitmap _downArrow;
		private Bitmap _upArrow;

		private int _leftMostColBeforeFreeze;
		//private ArrayList _selectableStoreGroupHeaders;
		private ArrayList _selectableVariableHeaders = new ArrayList();
		//private ArrayList _selectableTimeHeaders;
		private ArrayList _selectableBasisHeaders = new ArrayList();
		//private ArrayList _cmiBasisList;
		private SortedList _sortedVariableHeaders = new SortedList();
		private SortedList _sortedTimeHeaders = new SortedList();
		//private RowColProfileHeader _adjustmentRowHeader;
		//private RowColProfileHeader _originalRowHeader;
		//private RowColProfileHeader _currentRowHeader;
		private CellTag[][,] _gridData;
		private object _holdValue;
		private structSort _currSortParms;

		private Theme _theme;
		private ThemeProperties _frmThemeProperties; //for the properties dialog box.
		private TabPage _currentTabPage = null;
		private bool _refreshAssortmentWorkspace = false;	// TT#1261 - stodd
        //private bool _refreshAllocationWorkspace = false;	// TT#529-MD - stodd -  Status not updated on workspace After running Balance Proportional  // TT#918- MD - Jellis - Group Allocation Error prevent actions
		private bool _ignoreDisplayOnly = false;	//TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically
		private bool _enableCreatePlaceholdersAction = true;	// BEGIN TT#1930 - stodd - argument exception
		private bool _enableBalanceAssortmentAction = true;	// BEGIN TT#2148 - stodd - 

		// BEGIN - TT#2153 - stodd - Assortment Export 
		private bool _exporting = false;
		private bool _checkForExportSelected = false;
		private bool _exportDetails = false;
		private UltraGridExcelExporter _ultraGridExcelExporter1 = null;
		// END - TT#2153 - stodd - Assortment Export 
		//END TT#3 -MD- DOConnell - Export does not produce output
		// BEGIN TT#376-MD - stodd - Update Enqueue logic
		private bool _enqueueHeaderError = false;
		// END TT#376-MD - stodd - Update Enqueue logic
		private int _assortmentRid;		// TT#696-MD - Stodd - add "active process"

		private bool _changingState = false;

		//BEGIN TT#600-MD - stodd - Dragging a header with  pack(s) does not properly adjust the placeholder
		//private List<UltraGridRow> _newPHColorRows;
		//private List<UltraGridRow> _matchingHdrColorRows;
		//END TT#600-MD - stodd - Dragging a header with  pack(s) does not properly adjust the placeholder

		#endregion

		#region Error Handler & FormatForXP

		/// <summary>
		/// This procedure is temporary and should be put away later. But it's 
		/// very useful for debugging purposes because it tells you the true source
		/// of the problem to the line number.
		/// </summary>
		/// <param name="exc"></param>

		private void HandleExceptions(System.Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		protected void FormatForXP(Control ctl)
		{
			try
			{
				foreach (Control c in ctl.Controls)
				{
					FormatForXP(c);
				}

				if (ctl.GetType().BaseType == typeof(ButtonBase))
				{
					((ButtonBase)ctl).FlatStyle = FlatStyle.System;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		#endregion

		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		#region Properties
		public ApplicationSessionTransaction Transaction
		{
			get
			{
				return _transaction;
			}
		}

		// BEGIN TT#696-MD - Stodd - add "active process"
		public int AssortmentRid
		{
			get
			{
				return _assortmentRid;
			}
		}
		// END TT#696-MD - Stodd - add "active process"

        private string _groupName;
        public string GroupName 
        { 
            get 
            { 
                return _groupName; 
            } 
            set 
            { 
                _groupName = value;
                ((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Tools["TextBoxTool1"]).Text = _groupName;
            } 
        }

        // Begin TT#1087 - MD - stodd - size review showing wrong headers 
        /// <summary>
        /// Checks Group Allocation's "Process As" radiobutton for "Group" .
        /// </summary>
        public bool IsProcessAsGroup
        {
            get
            {
                bool answer = false;

                if (midToolbarRadioButton1.Button1.Checked)
                {
                    answer = true;
                }
                return answer;
            }
        }

        /// <summary>
        /// Checks Group Allocation's "Process As" radiobutton for "Headers".
        /// </summary>
        public bool IsProcessAsHeaders
        {
            get
            {
                bool answer = false;

                if (midToolbarRadioButton1.Button2.Checked)
                {
                    answer = true;
                }
                return answer;
            }
        }
        // End TT#1087 - MD - stodd - size review showing wrong headers 

		#endregion Properties
		// END TT#371-MD - stodd -  Velocity Interactive on Assortment

		#region Constructor

		public GroupAllocationView(ExplorerAddressBlock eab, ApplicationSessionTransaction trans)
			: base(trans.SAB)
		{
			try
			{
				_formLoading = true;
				_transaction = trans;

				// TT#708 - THIS NEEDs TO BE CHANGED FOR GROUP ALLOCATION 
				//_windowType = windowType;
				_windowType = eAssortmentWindowType.Assortment;



				_sab = _transaction.SAB;
				_eab = eab;
				_storeFilterDL = new StoreFilterData();
				//_openParms = aOpenParms;
				//_sab = sab;
				_theme = _sab.ClientServerSession.Theme;
				_pageLoadLock = new MIDReaderWriterLock();
				_loadHash = new Hashtable();

				//g2MouseUpRefireHandler += new System.Windows.Forms.MouseEventHandler(this.g2_MouseUp);
				//g3MouseUpRefireHandler += new System.Windows.Forms.MouseEventHandler(this.g3_MouseUp);

				InitializeComponent();

				this.Name = "Group Allocation View";
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		#endregion

		#region Methods to satisfy IFormBase

		override public void ICut()
		{
		}

		override public void ICopy()
		{
		}

		override public void IPaste()
		{
		}

		override public void IDelete()
		{
		}

		override public void IFind()
		{
			//Find();
		}

		//BEGIN TT#3 -MD- DOConnell - Export does not produce output
		public override void IExport()
		{
			try
			{
				// BEGIN TT#2153 - stodd - Assortment Export - TEMP
				// NOTE:
				// in V10.2 of Infragistics, the UltraGridExcelExporter has a new property called BandSpacing. 
				// We can use that to remove the blank lines between bands on a Detail Export.
				//===============================================================================================
				// To enable export, un-comment out next line
				//Export();
				// BEGIN TT#2153 - stodd - Assortment Export - TEMP

			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		//END TT#3 -MD- DOConnell - Export does not produce output

		override public void IUndo()
		{
			Undo();
		}

		override public void ISave()
		{
			try
			{
				//Cursor.Current = Cursors.WaitCursor;
				//StopPageLoadThreads();
				//RecomputePlanCubes();

				//this.WindowState = FormWindowState.Normal;
				//this.Enabled = false;

				//_saveForm = new AssortmentViewSave(
				//    _sab,
				//    _openParms,
				//    _asrtCubeGroup,
				//    _selectableComponentColumnHeaders,
				//    _selectableSummaryRowHeaders,
				//    _selectableTotalColumnHeaders,
				//    _selectableDetailColumnHeaders,
				//    // BEGIN TT#359-MD - stodd - add GroupBy to Asst View
				//    _selectableDetailRowHeaders,
				//    _columnGroupedBy);
				//// END TT#359-MD - stodd - add GroupBy to Asst View

				//_saveForm.OnAssortmentSaveClosingEventHandler += new AssortmentViewSave.AssortmentSaveClosingEventHandler(OnAssortmentSaveClosing);
				//// Begin TT#2 Ron Matelic Assortment Plannig
				//_saveForm.OnAssortmentSaveHeaderDataEventHandler += new AssortmentViewSave.AssortmentSaveHeaderDataEventHandler(OnAssortmentSaveHeaderData);
				//// End TT#2
				//_saveForm.MdiParent = this.MdiParent;
				//_saveForm.Show();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				//Cursor.Current = Cursors.Default;
			}
		}

		override public void ISaveAs()
		{
			try
			{
				ISave();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		override public void IRefresh()
		{
		}

		public override void ITheme()
		{
			try
			{
				Theme();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		public void UpdateData()
		{
			UpdateData(false);
		}

		public void UpdateData(bool reload)
		{
			try
			{
				//_asrtCubeGroup.Refresh();
				// Begin TT#2 - Ron Matelic - Assortment Planning
				//_asrtCubeGroup.NullHeaderObjects();
				//_asrtCubeGroup.ReadData(reload);
				//_dtHeaders = _asrtCubeGroup.GetAssortmentComponents();
				//ComponentsChanged();	//TT#926 MD - Stodd - group allocation not refreshing with data from style review - 
				// End TT#2
				//ReformatRowsChanged(true);

				//Begin TT#926 MD - Stodd - group allocation not refreshing with data from style review - 
                AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
                ArrayList hdrKeyArray = new ArrayList();
                foreach (AllocationProfile ap in alp.ArrayList)
                {
                    if (ap.HeaderType != eHeaderType.Assortment && ap.HeaderType != eHeaderType.Placeholder)
                    {
                        hdrKeyArray.Add(ap.Key);
                    }
                }
                int[] hdrRidList = new int[hdrKeyArray.Count];
                hdrRidList = (int[])hdrKeyArray.ToArray(typeof(int));
                ReloadUpdatedHeaders(hdrRidList);
				//End TT#926 MD - Stodd - group allocation not refreshing with data from style review -
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		// Begin TT#1278 - RMatelic - Assortment When selecting Save no hour glass appears and the user cannot tell if somethingis saved or not.  Selecting the X in the right had corner does not give a message asking if the user wants to save or not.
		override protected bool SaveChanges()
		{
			//bool headerChanged;
			//bool placeholderChanged;

			try
			{
				//headerChanged = ((AssortmentCubeGroup)_asrtCubeGroup).hasHeaderCubeChanged();
				//placeholderChanged = ((AssortmentCubeGroup)_asrtCubeGroup).hasPlaceholderCubeChanged();

				//if (headerChanged || placeholderChanged)
				//{
				//    _asrtCubeGroup.SaveCubeGroup();
				//}

				OnAssortmentSaveHeaderData(null);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			return true;
		}
		// End TT#1278

		public void SaveUserAssortmentData()
		{
			try
			{
				// BEGIN TT#376-MD - stodd - Update Enqueue logic
				if (!_enqueueHeaderError)
				{
					switch (_transaction.AssortmentViewLoadedFrom)
					{
						//case eAssortmentBasisLoadedFrom.UserSelectionCriteria:

						//    _transaction.AssortmentGroupBy = Convert.ToInt32(_columnGroupedBy, CultureInfo.CurrentUICulture);
						//    _transaction.AssortmentStoreAttributeRid = Convert.ToInt32(cboStoreGroup.SelectedValue, CultureInfo.CurrentUICulture);
						//    _transaction.AssortmentViewRid = Convert.ToInt32(cboView.SelectedValue, CultureInfo.CurrentUICulture);
						//    _transaction.SaveAssortmentDefaults();
						//    break;

						case eAssortmentBasisLoadedFrom.AssortmentProperties:
							// Begin TT#1171 - stodd
							//_transaction.AssortmentViewRid = Convert.ToInt32(cboView.SelectedValue, CultureInfo.CurrentUICulture);
							_transaction.SaveAssortmentDefaults();
							// End TT#1171 - stodd
							break;
					}
				}
				// END TT#376-MD - stodd - Update Enqueue logic
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		public void UpdateOtherViews()
		{
			MIDRetail.Windows.StyleView frmStyleView;
			MIDRetail.Windows.SizeView frmSizeView;
			MIDRetail.Windows.SummaryView frmSummaryView;
			try
			{

				// Begin TT#1282 - RMatelic - Unrelated to issue; if no headers, views are invalid
				bool headerFound = false;
				if (_headerList.Count > 0)
				{
					foreach (AllocationHeaderProfile ahp in _headerList)
					{
						if (ahp.HeaderType != eHeaderType.Assortment)
						{
							headerFound = true;
							break;
						}
					}
				}
				if (!headerFound)
				{
					if (_transaction.StyleView != null)
					{
						_transaction.StyleView.Close();
					}
					if (_transaction.SizeView != null)
					{
						_transaction.SizeView.Close();
					}
					if (_transaction.SummaryView != null)
					{
						_transaction.SummaryView.Close();
					}
					return;
				}
				// End TT#1282

                // Begin TT#964 - MD - stodd - style review end up with additional header and bulk components
                if (midToolbarRadioButton1.Button1.Checked) // If action was processed as a GROUP
                {
                    // Don't bother determining headers if no other review screens are active
                    if (_transaction.StyleView != null || _transaction.SizeView != null || _transaction.SummaryView != null)
                    {
                        DetermineMasterProfileList(true);
                    }
                }
                // End TT#964 - MD - stodd - style review end up with additional header and bulk components

				//SaveCurrentSettings();
				if (_transaction.StyleView != null)
				{
				//BEGIN TT#689 - MD - DOConnell - When open Size Reveiw receive a system argument exception index out of range
					//frmStyleView = (MIDRetail.Windows.StyleView)_transaction.StyleView;
					//frmStyleView.UpdateData();
                    ((MIDRetail.Windows.StyleView)_transaction.StyleView).UpdateDataReload();
					//frmStyleView.UpdateDataReload();
				//END TT#689 - MD - DOConnell - When open Size Reveiw receive a system argument exception index out of range
				}
				if (_transaction.SizeView != null)
				{
					frmSizeView = (MIDRetail.Windows.SizeView)_transaction.SizeView;
                    // Begin TT#607-MD - RMatelic - Size Review not displaying new sizes added when header is dropped on an assortment placeholder
                    //frmSizeView.UpdateData();
                    frmSizeView.UpdateDataReload();
                    // End TT#607-MD 
				}
				if (_transaction.SummaryView != null)
				{
					frmSummaryView = (MIDRetail.Windows.SummaryView)_transaction.SummaryView;
					frmSummaryView.UpdateData();
				}
				if ((_transaction.StyleView != null) || (_transaction.SizeView != null) || (_transaction.SummaryView != null))
				{
					//GetCurrentSettings();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		//BEGIN TT#3 -MD- DOConnell - Export does not produce output 
		private void Export()
		{
			MIDExportFile MIDExportFile = null;
			try
			{
				_exporting = true;
				Cursor.Current = Cursors.WaitCursor;

				if (_ultraGridExcelExporter1 == null)
				{
					_ultraGridExcelExporter1 = new UltraGridExcelExporter();
					_ultraGridExcelExporter1.RowExporting += new Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventHandler(this.ultraGridExcelExporter1_RowExporting);
					_ultraGridExcelExporter1.InitializeRow += new Infragistics.Win.UltraWinGrid.ExcelExport.InitializeRowEventHandler(this.ultraGridExcelExporter1_InitializeRow);
				}
				//========================================================
				// Tab index = 0 is Matrix tab. Index = 1 is COntent tab.
				//========================================================
				//if (tabControl.SelectedIndex == 0)
				//{
				//    XLExport();
				//}
				//else if (tabControl.SelectedIndex == 1)
				//{
					ExportContent();
				//}
			}
			catch (IOException IOex)
			{
				MessageBox.Show(IOex.Message);
				return;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			finally
			{
				//if (tabControl.SelectedIndex == 0)
				//{
				//    if (MIDExportFile != null)
				//    {
				//        MIDExportFile.WriteFile();
				//    }
				//    SetGridRedraws(true);
				//}
				Cursor.Current = Cursors.Default;
				_exporting = false;
			}
		}

		private void ExportContent()
		{
			string caption = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_Assortment_Caption);
			string messageBoxText = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_Text);
			string button1text = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_But1);
			string button2text = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_But2);
			string button3text = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_But3);
			string NoSelectError = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_No_Sel);
			int answer = 0;

			MIDMessageBox mmb = new MIDMessageBox(caption, messageBoxText, new Color[3] { Color.Empty, Color.Empty, Color.Empty }, //button colors            
					new string[3] { button1text, button2text, button3text }, 0, null, MIDGraphics.GetIcon(MIDGraphics.ExcelImage), true);

			answer = mmb.Show();

			string errorMsg = string.Empty;

			if (answer == 0)
			{ CreateExcelAllHeader(FindSavePath()); }

			if (answer == 1)
			{
				if (ugDetails.Selected.Rows.Count > 0)
				{
					CreateExcelHeaderSelected(FindSavePath());
				}
				else
				{
					errorMsg = NoSelectError;
                    MessageBox.Show(errorMsg, MIDText.GetTextOnly((int)eMIDTextCode.frm_GroupAllocationReview), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			if (answer == 2)
			{
				if (ugDetails.Selected.Rows.Count > 0)
				{
					CreateExcelHeaderSelectedDetail(FindSavePath());
				}
				else
				{
					errorMsg = NoSelectError;
                    MessageBox.Show(errorMsg, MIDText.GetTextOnly((int)eMIDTextCode.frm_GroupAllocationReview), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void CreateExcelAllHeader(String myFilepath)
		{
			string MessBoxAlltext1 = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_All);
			string MessBoxAlltext2 = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_Exp_Lit);
			try
			{
				if (myFilepath != null)
				{
					_checkForExportSelected = false;
					_exportDetails = false;
					_ultraGridExcelExporter1.Export(this.ugDetails, myFilepath);
					MessageBox.Show(MessBoxAlltext1 + myFilepath + MessBoxAlltext2 + ugDetails.Rows.Count);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void CreateExcelHeaderSelected(String myFilepath)
		{
			string MessBoxSHtext1 = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_HS);
			string MessBoxSHtext2 = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_Exp_Lit);
			try
			{
				if (myFilepath != null)
				{
					_checkForExportSelected = true;
					_exportDetails = false;
					_ultraGridExcelExporter1.Export(this.ugDetails, myFilepath);
					MessageBox.Show(MessBoxSHtext1 + myFilepath + MessBoxSHtext2 + ugDetails.Selected.Rows.Count);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void CreateExcelHeaderSelectedDetail(String myFilepath)
		{
			string MessBoxSHDtext1 = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_HSD);
			string MessBoxSHDtext2 = MIDText.GetTextOnly(eMIDTextCode.lbl_MB_Excel_Exp_Lit);
			try
			{
				if (myFilepath != null)
				{
					_checkForExportSelected = true;
					_exportDetails = true;
					_ultraGridExcelExporter1.Export(this.ugDetails, myFilepath);
					MessageBox.Show(MessBoxSHDtext1 + myFilepath + MessBoxSHDtext2 + ugDetails.Selected.Rows.Count);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void ultraGridExcelExporter1_RowExporting(object sender, RowExportingEventArgs e)
		{
			// The GridRow property on the event args is a clone of the on-screen row, and it will not pick up the Selected State 
			Infragistics.Win.UltraWinGrid.UltraGridRow exportRow = e.GridRow;


			//  Get the grid
			Infragistics.Win.UltraWinGrid.UltraGrid grid = (Infragistics.Win.UltraWinGrid.UltraGrid)e.GridRow.Band.Layout.Grid;

			// Get the real, on-screen row, from the export row. 
			Infragistics.Win.UltraWinGrid.UltraGridRow onScreenRow = grid.GetRowFromPrintRow(exportRow);

			// If the on-screen row is not selected, do not export it. 
			//if (onScreenRow.Selected == false && _checkForExportSelected == true)
			if (!IsRowSelected(onScreenRow) && _checkForExportSelected == true)
				e.Cancel = true;
			// If SUMMARY only and the on-screen row is not in the first band, do not export it. 
			if (!_exportDetails && onScreenRow.Band.Index > 0)
				e.Cancel = true;
			// attempt to remove empty rows
			if (onScreenRow.IsEmptyRow)
				e.Cancel = true;
		}

		//End TT#2153 - DOConnell 

		/// <summary>
		/// recursive search to see if row or any parent rows are selected
		/// </summary>
		/// <param name="aRow"></param>
		/// <returns></returns>
		private bool IsRowSelected(Infragistics.Win.UltraWinGrid.UltraGridRow aRow)
		{
			bool isSelected = false;

			if (aRow.Selected)
			{
				isSelected = true;
			}
			else
			{
				if (aRow.ParentRow != null)
				{
					isSelected = IsRowSelected(aRow.ParentRow);
				}
			}

			return isSelected;
		}

		private void ultraGridExcelExporter1_InitializeRow(object sender, ExcelExportInitializeRowEventArgs e)
		{

			// If SUMMARY only and the on-screen row is not in the first band, do not export it. 
			if (!_exportDetails && e.CurrentOutlineLevel == 0)
			{
				//e.SkipRow = true;
				//e.SkipSiblings = true;
				e.SkipDescendants = true;
			}
		}

		private String FindSavePath()
		{
			Stream myStream;
			string myFilepath = null;
			try
			{
				SaveFileDialog saveFileDialog1 = new SaveFileDialog();
				saveFileDialog1.Filter = "excel files (*.xls)|*.xls";
				saveFileDialog1.FilterIndex = 2;
				saveFileDialog1.RestoreDirectory = true;
				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					if ((myStream = saveFileDialog1.OpenFile()) != null)
					{
						myFilepath = saveFileDialog1.FileName;
						myStream.Close();
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return myFilepath;
		}
		//END TT#3 -MD- DOConnell - Export does not produce output
        public void Navigate(string navTo)
        {
            // BEGIN TT#553-MD - stodd - Size View null reference from attr set
            // BEGIN TT#597 - stodd - style review changes
            //if (_rebuildWafers)
            //{
            //    _transaction.ResetFirstBuild(true);
            //    if (navTo == "Size")
            //    {
            //        _transaction.ResetFirstBuildSize(true);
            //    }
            //    //BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
            //    _transaction.RebuildWafers();
            //    //END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
            //    _rebuildWafers = false;
            //}
            // END TT#597 - stodd - style review changes
            // END TT#553-MD - stodd - Size View null reference from attr set

            System.Windows.Forms.Form frm = null;
            // Load Filters if they haven't already been loaded
            if (_transaction.AllocationFilterTable == null)
            {
                StoreFilterData storeFilterDL = new StoreFilterData();
                ArrayList userRIDList = new ArrayList();
                userRIDList.Add(Include.GlobalUserRID);
                userRIDList.Add(_sab.ClientServerSession.UserRID);
                DataTable dtFilter = storeFilterDL.StoreFilter_Read(userRIDList);
                _transaction.AllocationFilterTable = dtFilter.Copy();
            }

            // Begin TT#2 - RMatelic - Assortment Planning; if no headers, views are invalid
            bool headerFound = false;
            bool createTransProfileList = false;
            if (_headerList.Count > 0)
            {
                foreach (AllocationHeaderProfile ahp in _headerList)
                {
                    if (ahp.HeaderType != eHeaderType.Assortment)
                    {
                        headerFound = true;
                        break;
                    }
                }
            }
            if (!headerFound)
            {
                return;
            }
            // End TT#2

            MIDRetail.Windows.SummaryView frmSummaryView;
            MIDRetail.Windows.StyleView frmStyleView;
            MIDRetail.Windows.SizeView frmSizeView;

            bool okToContinue = true;

            try
            {
                switch (navTo)
                {
                    case "Summary":

                        if (_transaction.SummaryView != null)
                        {
                            frmSummaryView = (MIDRetail.Windows.SummaryView)_transaction.SummaryView;
                            frmSummaryView.Activate();
                            return;
                        }
                        else
                        {
                            if (_allocationReviewSummarySecurity.AccessDenied)
                            {
                                okToContinue = false;
                            }
                            else
                            {
                                _transaction.AllocationViewType = eAllocationSelectionViewType.Summary;
                                _transaction.AllocationGroupBy = Convert.ToInt32(eAllocationSummaryViewGroupBy.Attribute, CultureInfo.CurrentUICulture);
                                frm = new MIDRetail.Windows.SummaryView(_eab, _transaction);
                            }
                        }
                        break;

                    case "Style":

                        if (_transaction.StyleView != null)
                        {
                            frmStyleView = (MIDRetail.Windows.StyleView)_transaction.StyleView;
                            frmStyleView.Activate();
                            return;
                        }
                        else
                        {
                            //Begin TT#761 - MD - DOConnell - Display only the selected headers in Style or Size View
                            okToContinue = DetermineMasterProfileList(true);	// TT#889 - MD - stodd - need not working
                            if (!okToContinue)
                            {
                                return;
                            }
                            //END TT#761 - MD - DOConnell - Display only the selected headers in Style or Size View

                            //BEGIN TT#689 - MD - DOConnell - When open Size Reveiw receive a system argument exception index out of range
                            if (_rebuildWafers)
                            {
                                _transaction.ResetFirstBuild(true);
                                _rebuildWafers = false;
                            }
                            //END TT#689 - MD - DOConnell - When open Size Reveiw receive a system argument exception index out of range

                            if (_allocationReviewStyleSecurity.AccessDenied)
                            {
                                okToContinue = false;
                            }
                            else
                            {
                                _transaction.AllocationViewType = eAllocationSelectionViewType.Style;
                                _transaction.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Header);
                                // BEGIN TT#488-MD - Stodd - Group Allocation
                                // Begin TT#875 - MD - stodd - group allocation sytle/size review showing placeholders
                                //Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["cbxAttribute"];	// TT#727-MD - Stodd - toolbar security
                                //_transaction.AllocationStoreAttributeID = int.Parse(((ValueListItem)cbo.SelectedItem).DataValue.ToString());
                                // End TT#875 - MD - stodd - group allocation sytle/size review showing placeholders
                                //_transaction.AllocationStoreAttributeID = Convert.ToInt32(cboStoreGroup.SelectedValue, CultureInfo.CurrentUICulture);
                                // END TT#488-MD - Stodd - Group Allocation
                                frm = new MIDRetail.Windows.StyleView(_eab, _transaction);
                            }
                        }
                        break;

                    case "Size":

                        if (_transaction.SizeView != null)
                        {
                            frmSizeView = (MIDRetail.Windows.SizeView)_transaction.SizeView;
                            frmSizeView.Activate();
                            return;
                        }
                        else
                        {
                            //Begin TT#761 - MD - DOConnell - Display only the selected headers in Style or Size View
                            okToContinue = DetermineMasterProfileList(true);	// TT#889 - MD - stodd - need not working
                            if (!okToContinue)
                            {
                                return;
                            }
                            //END TT#761 - MD - DOConnell - Display only the selected headers in Style or Size View

                            //BEGIN TT#689 - MD - DOConnell - When open Size Reveiw receive a system argument exception index out of range
                            if (_rebuildWafers)
                            {
                                _transaction.ResetFirstBuildSize(true);
                                _rebuildWafers = false;
                            }
                            //END TT#689 - MD - DOConnell - When open Size Reveiw receive a system argument exception index out of range
                            if (_allocationReviewSizeSecurity.AccessDenied)
                            {
                                okToContinue = false;
                            }
                            else
                            {
                                if (!SizeViewIsValid())
                                    return;
                                _transaction.AllocationViewType = eAllocationSelectionViewType.Size;
                                _transaction.AllocationGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Header, CultureInfo.CurrentUICulture);
                                // Begin TT#875 - MD - stodd - group allocation sytle/size review showing placeholders
                                // BEGIN TT#488-MD - Stodd - Group Allocation
                                //Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["cbxAttribute"];	// TT#727-MD - Stodd - toolbar security
                                //_transaction.AllocationStoreAttributeID = int.Parse(((ValueListItem)cbo.SelectedItem).DataValue.ToString());
                                // End TT#875 - MD - stodd - group allocation sytle/size review showing placeholders
                                //_transaction.AllocationStoreAttributeID = Convert.ToInt32(cboStoreGroup.SelectedValue, CultureInfo.CurrentUICulture);
                                // END TT#488-MD - Stodd - Group Allocation
                                frm = new MIDRetail.Windows.SizeView(_eab, _transaction);
                            }
                        }
                        break;
                }

                if (!okToContinue)
                {
                    MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
                    return;
                }

				// Begin TT#889 - MD - stodd - need not working
                //BEGIN TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception
                //if (_transaction.AssortmentSelectedHdrList.Count != 0)
                //{
                //    createTransProfileList = true;
                //}
                //SelectedHeaderList selectedHdrList = GetSelectableHeaderList(Convert.ToInt32(eAssortmentSelectableActionType.OpenReview), _transaction, createTransProfileList);
                //_transaction.AssortmentSelectedHdrList = selectedHdrList;
                //END TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception
				// End TT#889 - MD - stodd - need not working
                Cursor.Current = Cursors.WaitCursor;
                frm.MdiParent = this.MdiParent;
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();

                //Begin TT#792 - MD - DOConnell - Focus on placeholder after a header is attached with sizes.  select size review receive warning and screen goes blank. select x screen comes back.
                if (((MIDRetail.Windows.MIDFormBase)(frm)).ExceptionCaught)
                {
                    frm.Dispose();
                }
                //End TT#792 - MD - DOConnell - Focus on placeholder after a header is attached with sizes.  select size review receive warning and screen goes blank. select x screen comes back.

                if (navTo == "Style")
                {
                    ((MIDRetail.Windows.StyleView)frm).ResetSplitters();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
        }

		// Begin TT#889 - MD - stodd - need not working
        private void DetermineMasterProfileList()
        {
            DetermineMasterProfileList(false);
        }
		// End TT#889 - MD - stodd - need not working

        /// <summary>
        /// Determines what headers should be in the master list to be processed.
        /// AddHeaders: When navigating to Style/Size review, the real headers may need to be added
        /// to the list if only the assortment profile is in the selected header list.
        /// </summary>
        /// <param name="addHeaders"></param>
		// Begin TT#1090 - MD - stodd - problems with enqueue message -
        private bool DetermineMasterProfileList(bool addHeaders)	// TT#889 - MD - stodd - need not working
        {
            bool okToContinue = true;
            _rebuildWafers = true;
            if (tabControl.SelectedIndex == 0)
            {
                if (_selectedHeaderList.Count > 0 && IsProcessAsHeaders)               
                {
                    _transaction.CreateAllocationProfileListFromAssortmentMaster(false, _selectedHeaderList, addHeaders);	// TT#889 - MD - stodd - need not working  // TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
					//_transaction.CreateAllocationProfileListFromAssortmentMaster(true, _selectedHeaderList, addHeaders);	// TT#889 - MD - stodd - need not working // TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
                }
                else
                {
                    if (IsProcessAsHeaders)
                    {
                        string msg = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_NoHeadersSelectedInGroupAllocation);
                        DialogResult diagResult = MessageBox.Show(msg, "No Headers Selected", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (diagResult == System.Windows.Forms.DialogResult.Cancel)
                        {
                            okToContinue = false;
                        }
                    }
                    if (okToContinue)
                    {
                        // Do not include Placeholder headers
                        _transaction.CreateAllocationProfileListFromAssortmentMaster(false, false);  // TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
                        //_transaction.CreateAllocationProfileListFromAssortmentMaster(true, false);  // TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
                    }
                }
            }
            else
            {
                // Do not include Placeholder headers
                _transaction.CreateAllocationProfileListFromAssortmentMaster(false, false);
            }
            return okToContinue;
        }
		// End TT#1090 - MD - stodd - problems with enqueue message -
        // End TT#875 - MD - stodd - group allocation sytle/size review showing placeholders

		private bool SizeViewIsValid()
		{
			bool sizesExist = false;
			Header header;
			// BEGIN MID Track #2547 - Error asking for Analysis Only and Size Review
			// use different error message for Analysis only
			string errorMessage = string.Empty;
			if (_transaction.AnalysisOnly)
			{
				// BEGIN MID Track #2959 - - Remove #2547 edit disallowing Size Review for Need Analysis
				//errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeReviewInvalidForAnalysis);
				if (_transaction.SizeCurveRID != Include.NoRID)
					sizesExist = true;
				else
					errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeCurveRequiredForAnalysisOnly);
				// END MID Track #2959
			}
			else
			{
				header = new Header();
				foreach (AllocationHeaderProfile ahp in _headerList)
				{
					if (ahp.AllocationTypeFlags.WorkUpBulkSizeBuy)
						sizesExist = true;
					else if (header.BulkColorSizesExist(ahp.Key))
						sizesExist = true;
					// RonM - temporarily take out allowing pack sizes 
					//else if (header.PackSizesExist(ahp.Key)) 
					//	sizesExist = true;

					if (sizesExist)
						break;
				}
				if (!sizesExist)
					errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeReviewInvalid);
			}
			if (!sizesExist)
				MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			// END MID Track #2547
			return sizesExist;
		}

		void OnAssortmentSaveClosing(object source, bool aViewSaved, int aViewRID)
		{
			try
			{
				if (aViewSaved)
				{
					//BindViewComboBox();
					//SetCurrentView(aViewRID);
					_lastViewValue = aViewRID;
					//cboView.SelectedValue = aViewRID;
				}

				this.Enabled = true;
				//this.WindowState = FormWindowState.Maximized; // TT#1278 - unrelated to issue - moved this line down; 2 windows were temporarily displaying 
				//LoadCurrentPages();
				//LoadSurroundingPages();
				this.WindowState = FormWindowState.Maximized;
				Cursor.Current = Cursors.Default;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		#endregion

		#region GroupAllocationView Initialize and Load

		public void Initialize()
		{
			int i;
			AllocationHeaderProfileList asrtList;

			try
			{
                _windowName = MIDText.GetTextOnly(eMIDTextCode.frm_GroupAllocationReview);
				tabContent.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Content);

				_gridData = new CellTag[12][,];

				_assortmentViewData = new AssortmentViewData();

				BuildMenu();
				BuildActionMenu();
                LoadActionsOnToolbar();
				_lastFilterValue = _transaction.AllocationFilterID;

				// Set up Security
                FunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.GroupAllocationReview); // TT#1007 - md - stodd - change group allocation security - 

				_allocationReviewSummarySecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSummary);
				_allocationReviewStyleSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewStyle);
				_allocationReviewSizeSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSize);

				_userRIDList = new ArrayList();
				_userRIDList.Add(-1);

				if (_transaction.DataState != eDataState.ReadOnly)
				{
					if (!FunctionSecurity.AllowUpdate)
					{
						_transaction.DataState = eDataState.ReadOnly;
					}
				}

				if (_transaction.AllocationCriteriaExists)
				{
                    _transaction.AssortmentView = this;     // TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation
                    _transaction.UpdateAllocationViewSelectionHeaders();
                    //_transaction.AssortmentView = this;     // TT#927 - MD - Jellis - Allocations disappear from Style Review when coming from Group Allocation

					bool dummy = _transaction.AllocationStyleViewIncludesNeedAnalysis;
					_headerList = (AllocationHeaderProfileList)_transaction.GetMasterProfileList(eProfileType.AllocationHeader);

					foreach (AllocationHeaderProfile ahp in _headerList)
					{
						if (ahp.HeaderType == eHeaderType.Assortment)
						{
                            //_assortmentProfile = (AssortmentProfile)_transaction.GetAllocationProfile(ahp.Key);  // TT#488 - MD - Jellis - Group Allocation
                            _assortmentProfile = (AssortmentProfile)_transaction.GetAssortmentMemberProfile(ahp.Key);    // TT#488 - MD - Jellis - Group Allocation
							_sab.ApplicationServerSession.AddOpenAsrtView(ahp.Key, this);
						}
					}
				}

				_planRowProfileList = _transaction.PlanComputations.PlanVariables.GetAssortmentPlanningVariableList();

				ComponentsChanged();

				AssortmentProfile asp1 = _assortmentProfile;
				_assortmentRid = asp1.Key;
                Debug.WriteLine("Plan Level RID: " + asp1.PlanHnRID);
				GroupName = asp1.HeaderID;
				((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Tools["TextBoxTool1"]).Locked = true;
				_storeProfileList = _transaction.GetMasterProfileList(eProfileType.Store);

				if (_storeProfileList.Count == 0)
				{
					MessageBox.Show("Applied filter(s) have resulted in no displayable Stores.", "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}

				_workingDetailProfileList = _storeProfileList;

				this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_GroupAllocationReview); 
				this.Text += " - " + asp1.HeaderID;

				if (_transaction.DataState == eDataState.ReadOnly)
				{
					this.Text += " [Read Only]";
				}

				SetScreenControlsEnabled();

				// Format for XP, if applicable
				if (Environment.OSVersion.Version.Major > 4 && Environment.OSVersion.Version.Minor > 0 && System.IO.File.Exists(Application.ExecutablePath + ".manifest"))
				{
					FormatForXP(this);
				}

				InitializeContentTabData();

				SetCurrentTabPage();
				SetReadOnly(FunctionSecurity.AllowUpdate);

                // Begin TT#1007 - md - stodd - GA security
                if (!FunctionSecurity.AllowUpdate || _transaction.DataState == eDataState.ReadOnly) // TT#1090 - MD - stodd - problems with enqueue message - 
                {
                    Infragistics.Win.UltraWinToolbars.ComboBoxTool cboActions = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["actionComboBox"];
                    cboActions.SharedProps.Enabled = false;
                    Infragistics.Win.UltraWinToolbars.ButtonTool bProcess = (Infragistics.Win.UltraWinToolbars.ButtonTool)this.ultraToolbarsManager1.Tools["btnProcess"];
                    bProcess.SharedProps.Enabled = false;
                    midToolbarRadioButton1.Button1.Enabled = false;
                    midToolbarRadioButton1.Button2.Enabled = false;
                }
                // End TT#1007 - md - stodd - GA security

				midToolbarRadioButton1.Button1.Click += new EventHandler(Button1_Click);
				midToolbarRadioButton1.Button2.Click += new EventHandler(Button2_Click);

				_formLoading = false;
                ugDetails.Selected.Rows.Clear();    // TT#875 - MD - stodd - group allocation sytle/size review showing placeholders
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN TT#1930 - stodd - argument exception
		private void EnableCreatePlaceholderAction(bool enable)
		{
			foreach (ToolStripMenuItem menuItem in cmiAssortmentActions.DropDownItems)
			{
				if (menuItem.Text == MIDText.GetTextOnly((int)eAssortmentActionType.CreatePlaceholders))
				{
					if (enable)
					{
						menuItem.Enabled = true;
						_enableCreatePlaceholdersAction = enable;
					}
					else
					{
						menuItem.Enabled = false;
						_enableCreatePlaceholdersAction = enable;
					}
				}
			}
		}
		// END TT#1930 - stodd - argument exception


		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		private void EnableBalanceAssortmentAction(bool enable)
		{
			foreach (ToolStripMenuItem menuItem in cmiAssortmentActions.DropDownItems)
			{
				if (menuItem.Text == MIDText.GetTextOnly((int)eAssortmentActionType.BalanceAssortment))
				{
					if (enable)
					{
						menuItem.Enabled = true;
						_enableBalanceAssortmentAction = enable;
					}
					else
					{
						menuItem.Enabled = false;
						_enableBalanceAssortmentAction = enable;
					}
				}
			}
		}
		// END TT#2148 - stodd - Assortment totals do not include header values

		private void SetScreenControlsEnabled()
		{
			try
			{
				DisableMenuItem(this, eMIDMenuItem.FileSaveAs);
				// stodd - 4.0 to 4.1 manual merge

				List<int> hdrRidList = new List<int>();
				foreach (AllocationHeaderProfile ahp in _headerList)
				{
					hdrRidList.Add(ahp.Key);
				}
				string enqMessage = string.Empty;
				//if (!_transaction.HeadersEnqueued)
				//BEGIN TT#9-MD - DOConnell - Cannot drag and drop header into assortment
				if (!_transaction.AreHeadersEnqueued(_headerList))
				//if (_transaction.EnqueueHeaders(_transaction.GetHeadersToEnqueue(hdrRidList), out enqMessage))
				//END TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
				{
					DisableMenuItem(this, eMIDMenuItem.FileSave);
					//lblAction.Enabled = false;
					//btnApply.Enabled = false;
					//btnProcess.Enabled = false;
				}
			}
			catch
			{
				throw;
			}
		}

		private void SetCurrentTabPage()
		{
			try
			{
				//if (_assortmentSecurity.AllowView)
				//{
				//    _currentTabPage = this.tabAssortment;
				//}
				//else if (_assortmentSecurity.AllowView)
				//{
					_currentTabPage = this.tabContent;
					LoadContentGrid();
				//}
				//else
				//{
				//    _currentTabPage = this.tabProductChar;
				//    LoadProductCharGrid();
				//}
				this.tabControl.SelectedTab = _currentTabPage;
			}
			catch
			{
				throw;
			}
		}

		private AssortmentOpenParms LoadParmsFromTransaction()
		{
			AssortmentOpenParms openParms;
			try
			{
				openParms = new AssortmentOpenParms();
				openParms.StoreGroupRID = _transaction.AssortmentStoreAttributeRid;
				openParms.FilterRID = Include.NoRID;
				openParms.GroupBy = (eAllocationAssortmentViewGroupBy)_transaction.AssortmentGroupBy;
				openParms.ViewRID = _transaction.AssortmentViewRid;
				// BEGIN TT#2 - stodd - assortment
				if (openParms.ViewRID == Include.NoRID)
				{
					openParms.ViewRID = Include.DefaultAssortmentViewRID;
				}
				// END TT#2
				openParms.VariableType = (eAssortmentVariableType)_transaction.AssortmentVariableType;
				openParms.VariableNumber = _transaction.AssortmentVariableNumber;
				openParms.InclOnhand = _transaction.AssortmentIncludeOnhand;
				openParms.InclIntransit = _transaction.AssortmentIncludeIntransit;
				openParms.InclSimStores = _transaction.AssortmentIncludeSimStore;
				openParms.InclCommitted = _transaction.AssortmentIncludeCommitted;
				openParms.AverageBy = (eStoreAverageBy)_transaction.AssortmentAverageBy;
				openParms.GradeBoundary = (eGradeBoundary)_transaction.AssortmentGradeBoundary;
				return openParms;
			}
			catch
			{
				throw;
			}
		}

		private void AssortmentView_Load(object sender, System.EventArgs e)
		{
			try
			{
				_formLoading = true;

				//g1.Visible = false;
				//g2.Visible = false;
				//g3.Visible = false;
				//g4.Visible = false;
				//g5.Visible = false;
				//g6.Visible = false;
				//g7.Visible = false;
				//g8.Visible = false;
				//g9.Visible = false;

				//g1.Visible = ((PagingGridTag)g1.Tag).Visible;
				//g2.Visible = ((PagingGridTag)g2.Tag).Visible;
				//g3.Visible = ((PagingGridTag)g3.Tag).Visible;
				//g4.Visible = ((PagingGridTag)g4.Tag).Visible;
				//g5.Visible = ((PagingGridTag)g5.Tag).Visible;
				//g6.Visible = ((PagingGridTag)g6.Tag).Visible;
				//g7.Visible = ((PagingGridTag)g7.Tag).Visible;
				//g8.Visible = ((PagingGridTag)g8.Tag).Visible;
				//g9.Visible = ((PagingGridTag)g9.Tag).Visible;

				//spcVLevel1.Width = this.Width;
				//spcVLevel1.Height = this.Height;

				//CalcColSplitPosition2(false);
				//CalcColSplitPosition3(false);
				//SetColSplitPositions();
				//CalcRowSplitPosition4(false);
				//CalcRowSplitPosition12(false);
				//SetRowSplitPositions();
				//SetHScrollBar2Parameters();
				//SetHScrollBar3Parameters();
				//SetVScrollBar2Parameters();
				//SetVScrollBar3Parameters();

				// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
				//((WorkflowMethodTreeView)_eab.WorkflowMethodExplorer.TreeView).OnProcessMethodOnAssortmentEvent += new WorkflowMethodTreeView.ProcessMethodOnAssortmentEventHandler(OnProcessMethodOnAssortmentEvent);
				_sab.ProcessMethodOnAssortmentEvent.OnProcessMethodOnAssortmentEventHandler += new ProcessMethodOnAssortmentEvent.ProcessMethodOnAssortmentEventHandler(OnProcessMethodOnAssortmentEvent);
				// END TT#217-MD - stodd - unable to run workflow methods against assortment
				// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
				_sab.AssortmentSelectedHeaderEvent.OnAssortmentSelectedHeaderEventHandler += new AssortmentSelectedHeaderEvent.AssortmentSelectedHeaderEventHandler(OnAssortmentSelectedHeaderEvent);
				// END TT#371-MD - stodd -  Velocity Interactive on Assortment
				// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
				_sab.AssortmentTransactionEvent.OnAssortmentTransactionEventHandler += new AssortmentTransactionEvent.AssortmentTransactionEventHandler(OnAssortmentTransactionEvent);
				// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.
				// BEGIN TT#696-MD - Stodd - add "active process"
				// Begin TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
				AssortmentActiveProcessToolbarHelper.AddAssortmentReviewScreen(this.Text, _assortmentRid, "Group Allocation", this); // TT#998 - MD - stodd - Enqueue error when opening style review with GA open, but Size Need Method open as the active window.
				// End TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
				// END TT#696-MD - Stodd - add "active process"

				//optGroupByStoreGrade.Visible = false;
				//optGroupByStoreGroup.Visible = false;
				//panel5.Visible = false;
				//pnlGroupBy.Visible = false;

				//cboStoreGroup.Visible = false;
				//cboStoreGroupLevel.Visible = false;
				//cboView.Visible = false;
				//cboFilter.Visible = false;
				//btnApply.Visible = false;

				//tabControl.TabPages.Remove(tabAssortment);
				//tabControl.TabPages.Remove(tabProductChar);
				//lblAction.Visible = false;
				//btnProcess.Visible = false;
				//btnExpandCollapse.Visible = false;
				//label1.Visible = false;
				//textBox1.Visible = false;

				_formLoading = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				HandleExceptions(exc);
			}
		}

		//private void AssortmentView_Activated(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        g6.Focus();

		//        if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
		//        {
		//            g6.Select(0, 0);
		//        }
		//        else
		//        {
		//            g6.Select(-1, -1);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        HandleExceptions(exc);
		//    }
		//}

		#endregion

		#region Miscellaneous Initialization

		private void ComponentsChanged()
		{
			AssortmentComponentVariableProfile varProf;

			try
			{
				//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
				//_asrtCubeGroup.DefineTotalCubes(_sortedComponentColumnHeaders);
				//_asrtCubeGroup.ComponentsChanged(_sortedComponentColumnHeaders);
				//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.

				if (_windowType == eAssortmentWindowType.Assortment)
				{
					_placeholderSelected = false;

					//foreach (RowColProfileHeader rowColHdr in _selectableComponentColumnHeaders)
					//{
					//    varProf = (AssortmentComponentVariableProfile)rowColHdr.Profile;

					//    if (rowColHdr.IsDisplayed && varProf.Key == ((AssortmentViewComponentVariables)_componentVariables).Placeholder.Key)
					//    {
					//        _placeholderSelected = true;
					//    }
					//}
				}
				else
				{
					_placeholderSelected = false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildMenu()
		{
			try
			{
				//PopupMenuTool editMenuTool;
				//PopupMenuTool fileMenuTool;
				//ButtonTool btFind;
				//ButtonTool btExport;

				//utmMain.ImageListSmall = MIDGraphics.ImageList;
				//utmMain.ImageListLarge = MIDGraphics.ImageList;

				//fileMenuTool = new PopupMenuTool(Include.menuFile);
				//fileMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File);
				//fileMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.True;
				//utmMain.Tools.Add(fileMenuTool);

				//btExport = new ButtonTool(Include.btExport);
				//btExport.SharedProps.Caption = "&Export to Excel";
				//btExport.SharedProps.Shortcut = Shortcut.CtrlE;
				//btExport.SharedProps.MergeOrder = 10;
				//utmMain.Tools.Add(btExport);

				//fileMenuTool.Tools.Add(btExport);

				//editMenuTool = new PopupMenuTool(Include.menuEdit);
				//editMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit);
				//editMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.False;
				//utmMain.Tools.Add(editMenuTool);

				//btFind = new ButtonTool(Include.btFind);
				//btFind.SharedProps.Caption = "&Find";
				//btFind.SharedProps.Shortcut = Shortcut.CtrlF;
				//btFind.SharedProps.MergeOrder = 20;
				//btFind.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.FindImage);
				//utmMain.Tools.Add(btFind);
				//editMenuTool.Tools.Add(btFind);

				//editMenuTool.Tools["btFind"].InstanceProps.IsFirstInGroup = true;

				AddMenuItem(eMIDMenuItem.FileExport);
				AddMenuItem(eMIDMenuItem.EditFind);
				AddMenuItem(eMIDMenuItem.ToolsTheme);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildActionMenu()
		{
			ToolStripMenuItem menuItem;
			string actionText;

			try
			{
				//bool isAssortmentAction = true;
				//foreach (int action in Enum.GetValues(typeof(eAssortmentActionType)))
				//{
				//    // Begin TT#1725 - RMatelic - "Hide" Committed >>> add 'if...' condition to skip committed actions
				//    if (action == (int)eAssortmentActionType.ChargeCommitted || action == (int)eAssortmentActionType.CancelCommitted)
				//    {
				//        continue;
				//    }
				//    else
				//    {
				//        if (AllowAction(action, isAssortmentAction))
				//        {
				//            actionText = MIDText.GetTextOnly(action);

				//            menuItem = new ToolStripMenuItem(actionText);
				//            menuItem.Size = new System.Drawing.Size(152, 22);
				//            menuItem.Click += new System.EventHandler(this.AssortmentActionClick);
				//            menuItem.Tag = new ActionTag(action, actionText);
				//            cmiAssortmentActions.DropDownItems.Add(menuItem);
				//        }
				//    }
				//    // End TT#1725
				//}
				bool isAssortmentAction = false;
				foreach (int action in Enum.GetValues(typeof(eAssortmentAllocationActionType)))
				{
					if (AllowAction(action, isAssortmentAction))
					{
						actionText = MIDText.GetTextOnly(action);

						menuItem = new ToolStripMenuItem(actionText);
						menuItem.Size = new System.Drawing.Size(152, 22);
						menuItem.Click += new System.EventHandler(this.AllocationActionClick);
						menuItem.Tag = new ActionTag(action, actionText);
						cmiAllocationActions.DropDownItems.Add(menuItem);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool AllowAction(int aAction, bool bAssortmentAction)
		{
			bool allowAction = true;
			try
			{
				FunctionSecurityProfile actionSecurity;
				if (bAssortmentAction)
				{
					actionSecurity = _sab.ClientServerSession.GetMyUserActionSecurityAssignment((eAssortmentActionType)aAction);
					if (actionSecurity.AccessDenied)
					{
						allowAction = false;
					}
				}
				else
				{
					actionSecurity = _sab.ClientServerSession.GetMyUserActionSecurityAssignment((eAllocationActionType)aAction, true);
					if (actionSecurity.AccessDenied)
					{
						allowAction = false;
					}
					else
					{
						actionSecurity = _sab.ClientServerSession.GetMyUserActionSecurityAssignment((eAllocationActionType)aAction, false);
						if (actionSecurity.AccessDenied)
						{
							allowAction = false;
						}
					}
				}
			}
			catch
			{
				allowAction = false;
				throw;
			}
			return allowAction;
		}

		//private void SetCurrentView(int aViewID)
		//{
		//    DataRow drCurrView;

		//    try
		//    {
		//        _openParms.ViewRID = aViewID;
		//        drCurrView = _assortmentViewData.AssortmentView_Read(_openParms.ViewRID);
		//        _openParms.ViewName = Convert.ToString(drCurrView["VIEW_ID"], CultureInfo.CurrentUICulture);
		//        _openParms.ViewUserID = Convert.ToInt32(drCurrView["USER_RID"], CultureInfo.CurrentUICulture);
		//        // BEGIN TT#359-MD - stodd - add GroupBy to Asst View
		//        if (FormLoaded)
		//        {
		//            _openParms.GroupBy = (eAllocationAssortmentViewGroupBy)Convert.ToInt32(drCurrView["GROUP_BY_ID"]);
		//            _columnGroupedBy = _openParms.GroupBy;
		//        }
		//        // END TT#359-MD - stodd - add GroupBy to Asst View

		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void LoadView(int aViewID)
		//{
		//    int i;
		//    AssortmentComponentVariableProfile compVarProf;
		//    AssortmentTotalVariableProfile totVarProf;
		//    AssortmentSummaryVariableProfile summVarProf;
		//    //Begin TT#2 - JScott - Assortment Planning - Phase 2
		//    VariableProfile planVarProf;
		//    //End TT#2 - JScott - Assortment Planning - Phase 2
		//    AssortmentDetailVariableProfile detVarProf;
		//    QuantityVariableProfile quantVarProf;

		//    //QuantityVariableProfile viewQVarProf;
		//    DataRow viewRow;
		//    Hashtable varKeyHash;
		//    //Hashtable qVarKeyHash;
		//    //bool cont;

		//    try
		//    {
		//        // BEGIN TT#359-MD - stodd - add GroupBy to Asst View
		//        if (FormLoaded)
		//        {
		//            viewRow = _assortmentViewData.AssortmentView_Read(aViewID);
		//            eAllocationAssortmentViewGroupBy ViewGroupBy = (eAllocationAssortmentViewGroupBy)int.Parse(viewRow["GROUP_BY_ID"].ToString());
		//            if (ViewGroupBy == eAllocationAssortmentViewGroupBy.Attribute && optGroupByStoreGroup.Checked == false)
		//            {
		//                optGroupByStoreGroup.Checked = true;
		//            }
		//            if (ViewGroupBy == eAllocationAssortmentViewGroupBy.StoreGrade && optGroupByStoreGrade.Checked == false)
		//            {
		//                optGroupByStoreGrade.Checked = true;
		//            }
		//        }
		//        // BEGIN TT#359-MD - stodd - add GroupBy to Asst View

		//        _assrtViewDetail = _assortmentViewData.AssortmentViewDetail_Read(aViewID);

		//        //Load Component columns

		//        varKeyHash = new Hashtable();
		//        _selectableComponentColumnHeaders.Clear();

		//        foreach (DataRow row in _assrtViewDetail.Rows)
		//        {
		//            if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eAssortmentViewAxis.Component)
		//            {
		//                compVarProf = (AssortmentComponentVariableProfile)_componentColumnProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
		//                if (compVarProf != null)
		//                {
		//                    varKeyHash.Add(compVarProf.Key, row);
		//                }
		//            }
		//        }

		//        _placeholderSelected = false;

		//        foreach (AssortmentComponentVariableProfile varProf in _componentColumnProfileList)
		//        {
		//            viewRow = (DataRow)varKeyHash[varProf.Key];
		//            if (viewRow != null)
		//            {
		//                _selectableComponentColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, Include.ConvertCharToBool(Convert.ToChar(viewRow["SUMMARIZED_IND"], CultureInfo.CurrentUICulture)), Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
		//            }
		//            else
		//            {
		//                _selectableComponentColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
		//            }
		//        }

		//        _sortedComponentColumnHeaders.Clear();

		//        CreateSortedList(_selectableComponentColumnHeaders, _sortedComponentColumnHeaders);

		//        //Load Summary columns

		//        varKeyHash = new Hashtable();
		//        _selectableSummaryRowHeaders.Clear();

		//        foreach (DataRow row in _assrtViewDetail.Rows)
		//        {
		//            if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eAssortmentViewAxis.SummaryColumn)
		//            {
		//                summVarProf = (AssortmentSummaryVariableProfile)_summaryRowProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
		//                if (summVarProf != null)
		//                {
		//                    varKeyHash.Add(summVarProf.Key, row);
		//                }
		//                //Begin TT#2 - JScott - Assortment Planning - Phase 2
		//                else
		//                {
		//                    planVarProf = (VariableProfile)_planRowProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
		//                    if (planVarProf != null)
		//                    {
		//                        varKeyHash.Add(planVarProf.Key, row);
		//                    }
		//                }
		//                //End TT#2 - JScott - Assortment Planning - Phase 2
		//            }
		//        }

		//        foreach (AssortmentSummaryVariableProfile varProf in _summaryRowProfileList)
		//        {
		//            viewRow = (DataRow)varKeyHash[varProf.Key];
		//            if (viewRow != null)
		//            {
		//                _selectableSummaryRowHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, false, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
		//            }
		//            else
		//            {
		//                _selectableSummaryRowHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
		//            }
		//        }

		//        //Begin TT#2 - JScott - Assortment Planning - Phase 2
		//        foreach (VariableProfile varProf in _planRowProfileList)
		//        {
		//            viewRow = (DataRow)varKeyHash[varProf.Key];
		//            if (viewRow != null)
		//            {
		//                _selectableSummaryRowHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, false, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
		//            }
		//            else
		//            {
		//                _selectableSummaryRowHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
		//            }
		//        }

		//        //End TT#2 - JScott - Assortment Planning - Phase 2
		//        _sortedSummaryRowHeaders.Clear();

		//        CreateSortedList(_selectableSummaryRowHeaders, _sortedSummaryRowHeaders);

		//        //Load Total columns

		//        varKeyHash = new Hashtable();
		//        _selectableTotalColumnHeaders.Clear();

		//        foreach (DataRow row in _assrtViewDetail.Rows)
		//        {
		//            if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eAssortmentViewAxis.TotalColumn)
		//            {
		//                totVarProf = (AssortmentTotalVariableProfile)_totalColumnProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
		//                if (totVarProf != null)
		//                {
		//                    varKeyHash.Add(totVarProf.Key, row);
		//                }
		//            }
		//        }

		//        foreach (AssortmentTotalVariableProfile varProf in _totalColumnProfileList)
		//        {
		//            viewRow = (DataRow)varKeyHash[varProf.Key];
		//            if (viewRow != null)
		//            {
		//                _selectableTotalColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, false, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
		//            }
		//            else
		//            {
		//                _selectableTotalColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
		//            }
		//        }

		//        _sortedTotalColumnHeaders.Clear();

		//        CreateSortedList(_selectableTotalColumnHeaders, _sortedTotalColumnHeaders);

		//        //Load Detail columns

		//        varKeyHash = new Hashtable();
		//        _selectableDetailColumnHeaders.Clear();

		//        foreach (DataRow row in _assrtViewDetail.Rows)
		//        {
		//            if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eAssortmentViewAxis.DetailColumn)
		//            {
		//                detVarProf = (AssortmentDetailVariableProfile)_detailColumnProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
		//                if (detVarProf != null)
		//                {
		//                    varKeyHash.Add(detVarProf.Key, row);
		//                }
		//            }
		//        }

		//        foreach (AssortmentDetailVariableProfile varProf in _detailColumnProfileList)
		//        {
		//            viewRow = (DataRow)varKeyHash[varProf.Key];
		//            if (viewRow != null)
		//            {
		//                _selectableDetailColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, false, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
		//            }
		//            else
		//            {
		//                _selectableDetailColumnHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
		//            }
		//        }

		//        _sortedDetailColumnHeaders.Clear();

		//        CreateSortedList(_selectableDetailColumnHeaders, _sortedDetailColumnHeaders);

		//        //Load Detail rows

		//        varKeyHash = new Hashtable();
		//        _selectableDetailRowHeaders.Clear();

		//        foreach (DataRow row in _assrtViewDetail.Rows)
		//        {
		//            if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eAssortmentViewAxis.DetailRow)
		//            {
		//                quantVarProf = (QuantityVariableProfile)_detailRowProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
		//                if (quantVarProf != null)
		//                {
		//                    varKeyHash.Add(quantVarProf.Key, row);
		//                }
		//            }
		//        }

		//        foreach (QuantityVariableProfile varProf in _detailRowProfileList)
		//        {
		//            viewRow = (DataRow)varKeyHash[varProf.Key];
		//            if (viewRow != null)
		//            {
		//                _selectableDetailRowHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, false, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
		//            }
		//            else
		//            {
		//                _selectableDetailRowHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, false, -1, varProf));
		//            }
		//        }

		//        _sortedDetailRowHeaders.Clear();

		//        CreateSortedList(_selectableDetailRowHeaders, _sortedDetailRowHeaders);

		//        // Load Total Row
		//        //_selectableTotalRowHeaders.Clear();
		//        //_selectableTotalRowHeaders.Add(new RowColProfileHeader("Total", true, false, 0, new VariableProfile(-1)));
		//        //_selectableTotalRowHeaders.Add(new RowColProfileHeader("Balance", true, false, 0, new VariableProfile(-1)));
		//        //_selectableTotalRowHeaders.Add(new RowColProfileHeader("Commited", true, false, 0, new VariableProfile(-1)));


		//        // Build Grades

		//        _selectableStoreGradeHeaders = new ArrayList();
		//        i = 0;

		//        foreach (StoreGradeProfile strGrdProf in _storeGradeProfileList)
		//        {
		//            _selectableStoreGradeHeaders.Add(new RowColProfileHeader(strGrdProf.StoreGrade, true, i, strGrdProf));
		//            i++;
		//        }

		//        _sortedStoreGradeHeaders = new SortedList();

		//        CreateSortedList(_selectableStoreGradeHeaders, _sortedStoreGradeHeaders);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void SelectRadioBtn()
		//{
		//    try
		//    {
		//        if (_openParms.GroupBy == eAllocationAssortmentViewGroupBy.StoreGrade)
		//        {
		//            this.optGroupByStoreGrade.Checked = true;
		//        }
		//        else
		//        {
		//            this.optGroupByStoreGroup.Checked = true;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		#endregion

		#region Format Grids (From Form_Load)

		//private void SetGridRedraws(bool aValue)
		//{
		//    try
		//    {
		//        _currentRedrawState = aValue;

		//        g2.Redraw = aValue;
		//        g3.Redraw = aValue;
		//        g4.Redraw = aValue;
		//        g5.Redraw = aValue;
		//        g6.Redraw = aValue;
		//        g7.Redraw = aValue;
		//        g8.Redraw = aValue;
		//        g9.Redraw = aValue;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void FormatCol1Grids(bool aClearGrid)
		//{
		//    try
		//    {
		//        Formatg4Grid(aClearGrid);
		//        Formatg1Grid(aClearGrid);
		//        Formatg7Grid(aClearGrid);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void FormatCol2Grids(bool aClearGrid, int aVariableSortKey, SortEnum aSortDirection)
		//{
		//    try
		//    {
		//        Formatg2Grid(aClearGrid, aVariableSortKey, aSortDirection);
		//        Formatg5Grid(aClearGrid);
		//        Formatg8Grid(aClearGrid);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void FormatCol3Grids(bool aClearGrid, int aVariableSortKey, SortEnum aSortDirection)
		//{
		//    try
		//    {
		//        Formatg3Grid(aClearGrid, aVariableSortKey, aSortDirection);
		//        Formatg6Grid(aClearGrid);
		//        Formatg9Grid(aClearGrid);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Formatg1Grid(bool aClearGrid)
		//{
		//    int i;
		//    int j;
		//    CubeWaferCoordinateList asrtWaferCoordinateList;
		//    RowColProfileHeader varHeader;
		//    //Begin TT#2 - JScott - Assortment Planning - Phase 2
		//    //AssortmentSummaryVariableProfile varProf;
		//    ComputationVariableProfile varProf;
		//    //End TT#2 - JScott - Assortment Planning - Phase 2
		//    string varName;

		//    try
		//    {
		//        if (aClearGrid)
		//        {
		//            g1.Clear();
		//        }

		//        if (g1.Tag == null)
		//        {
		//            g1.Tag = new PagingGridTag(Grid1, g1, g4, g1, null, 0, 0, null, _selectableSummaryRowHeaders, null, _sortedSummaryRowHeaders, false, false);
		//        }

		//        g1.Rows.Count = _sortedSummaryRowHeaders.Count + 1;
		//        g1.Cols.Count = g4.Cols.Count;
		//        g1.Rows.Fixed = 1;
		//        g1.Cols.Fixed = g4.Cols.Count;

		//        g1.AllowDragging = AllowDraggingEnum.None;
		//        g1.AllowMerging = AllowMergingEnum.Free;

		//        for (j = 0; j < g1.Cols.Count; j++)
		//        {
		//            g1.SetData(0, j, " ");
		//        }

		//        g1.Rows[0].UserData = new RowHeaderTag(new CubeWaferCoordinateList(), null, null, 0, string.Empty, string.Empty, false);

		//        i = 0;

		//        foreach (DictionaryEntry varEntry in _sortedSummaryRowHeaders)
		//        {
		//            varHeader = (RowColProfileHeader)varEntry.Value;
		//            //Begin TT#2 - JScott - Assortment Planning - Phase 2
		//            //varProf = (AssortmentSummaryVariableProfile)varHeader.Profile;
		//            varProf = (ComputationVariableProfile)varHeader.Profile;
		//            //End TT#2 - JScott - Assortment Planning - Phase 2

		//            if (_windowType == eAssortmentWindowType.Assortment)
		//            {
		//                if (varProf.Key == ((AssortmentViewSummaryVariables)_summaryVariables).Units.Key)
		//                {
		//                    switch (_asrtCubeGroup.AssortmentVariableType)
		//                    {
		//                        case eAssortmentVariableType.Receipts:
		//                            varName = MIDText.GetTextOnly(eMIDTextCode.lbl_Receipts);
		//                            break;

		//                        case eAssortmentVariableType.Sales:
		//                            varName = MIDText.GetTextOnly(eMIDTextCode.lbl_Sales);
		//                            break;

		//                        case eAssortmentVariableType.Stock:
		//                            varName = MIDText.GetTextOnly(eMIDTextCode.lbl_Stock);
		//                            break;

		//                        default:
		//                            varName = varHeader.Name;
		//                            break;
		//                    }
		//                }
		//                else
		//                {
		//                    varName = varHeader.Name;
		//                }
		//            }
		//            else
		//            {
		//                varName = varHeader.Name;
		//            }

		//            i++;
		//            asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//            //Begin TT#2 - JScott - Assortment Planning - Phase 2
		//            //asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSummaryVariable, ((AssortmentSummaryVariableProfile)varHeader.Profile).Key));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(varHeader.Profile.ProfileType, varHeader.Profile.Key));

		//            if (varHeader.Profile.ProfileType == eProfileType.Variable)
		//            {
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, ((AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile).AssortmentBasisList[0].VersionProfile.Key));
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, ((AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile).AssortmentBasisList[0].HierarchyNodeProfile.Key));
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, 1));
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.TimeTotalVariable, ((VariableProfile)varHeader.Profile).TotalTimeTotalVariableProfile.Key));
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key));
		//            }
		//            //End TT#2 - JScott - Assortment Planning - Phase 2

		//            g1.Rows[i].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, varHeader, i, varName, varName);

		//            for (j = 0; j < g1.Cols.Count; j++)
		//            {
		//                g1.SetData(i, j, varName);
		//            }
		//        }

		//        for (i = 0; i < g4.Cols.Count; i++)
		//        {
		//            g1.Cols[i].Visible = g4.Cols[i].Visible;
		//        }

		//        ((PagingGridTag)g1.Tag).Visible = true;
		//        ((PagingGridTag)g1.Tag).RowsPerRowGroup = 1;
		//        ((PagingGridTag)g1.Tag).RowGroupsPerGrid = g1.Rows.Count - g1.Rows.Fixed;
		//        ((PagingGridTag)g1.Tag).RowsPerScroll = 1;
		//        ((PagingGridTag)g1.Tag).ColsPerColGroup = 1;
		//        ((PagingGridTag)g1.Tag).ColGroupsPerGrid = g1.Cols.Count;
		//        ((PagingGridTag)g1.Tag).ColsPerScroll = 1;

		//        foreach (Row row in g1.Rows)
		//        {
		//            row.AllowMerging = true;
		//        }

		//        _gridData[Grid1] = new CellTag[g1.Rows.Count, g1.Cols.Count];
		//        g1.Visible = ((PagingGridTag)g1.Tag).Visible;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Formatg2Grid(bool aClearGrid, int aVariableSortKey, SortEnum aSortDirection)
		//{
		//    int i;
		//    int j;
		//    CubeWaferCoordinateList asrtWaferCoordinateList;
		//    RowColProfileHeader varHeader;
		//    RowColProfileHeader totalVarHeader;
		//    AssortmentTotalVariableProfile totVarProf;

		//    try
		//    {
		//        if (aClearGrid)
		//        {
		//            g2.Clear();
		//        }

		//        if (g2.Tag == null)
		//        {
		//            g2.Tag = new PagingGridTag(Grid2, g2, g1, g2, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE, _selectableTotalColumnHeaders, null, _sortedTotalColumnHeaders, null, false, false);
		//        }

		//        totVarProf = (AssortmentTotalVariableProfile)_totalColumnProfileList.FindKey((int)eAssortmentTotalVariables.TotalUnits);
		//        totalVarHeader = new RowColProfileHeader(totVarProf.VariableName, true, 0, totVarProf);

		//        g2.Rows.Count = g1.Rows.Count;
		//        g2.Cols.Count = _sortedTotalColumnHeaders.Count;
		//        g2.Rows.Fixed = g1.Rows.Fixed;
		//        g2.Cols.Fixed = 0;

		//        g2.AllowDragging = AllowDraggingEnum.None;
		//        g2.AllowMerging = AllowMergingEnum.RestrictCols;

		//        for (j = 0; j < g2.Cols.Count; j++)
		//        {
		//            g2.SetData(0, j, " ");
		//        }

		//        i = -1;

		//        foreach (DictionaryEntry varEntry in _sortedTotalColumnHeaders)
		//        {
		//            varHeader = (RowColProfileHeader)varEntry.Value;

		//            i++;
		//            asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentTotalVariable, totalVarHeader.Profile.Key));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

		//            g2.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, new RowColProfileHeader(_asrtCubeGroup.CurrentStoreGroupProfile.Name, true, 0, _asrtCubeGroup.CurrentStoreGroupProfile), totalVarHeader, i, _asrtCubeGroup.CurrentStoreGroupProfile.Name + "|" + varHeader.Name);
		//            g2.SetData(0, i, "Total");
		//        }

		//        ((PagingGridTag)g2.Tag).Visible = true;
		//        ((PagingGridTag)g2.Tag).RowsPerRowGroup = 1;
		//        ((PagingGridTag)g2.Tag).RowGroupsPerGrid = g2.Rows.Count - g2.Rows.Fixed;
		//        ((PagingGridTag)g2.Tag).RowsPerScroll = 1;
		//        ((PagingGridTag)g2.Tag).ColsPerColGroup = 1;
		//        ((PagingGridTag)g2.Tag).ColGroupsPerGrid = g2.Cols.Count;
		//        ((PagingGridTag)g2.Tag).ColsPerScroll = 1;

		//        for (i = 0; i < g2.Rows.Fixed; i++)
		//        {
		//            g2.Rows[i].AllowMerging = true;
		//        }

		//        _gridData[Grid2] = new CellTag[g2.Rows.Count, g2.Cols.Count];
		//        g2.Visible = ((PagingGridTag)g2.Tag).Visible;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Formatg3Grid(bool aClearGrid, int aVariableSortKey, SortEnum aSortDirection)
		//{
		//    int i;
		//    CubeWaferCoordinateList asrtWaferCoordinateList;
		//    RowColProfileHeader gradeHeader;
		//    RowColProfileHeader groupLevelHeader;
		//    RowColProfileHeader varHeader;
		//    RowColProfileHeader totalVarHeader;
		//    AssortmentDetailVariableProfile totVarProf;

		//    try
		//    {
		//        if (aClearGrid)
		//        {
		//            g3.Clear();
		//        }

		//        if (g3.Tag == null)
		//        {
		//            g3.Tag = new PagingGridTag(Grid3, g3, g1, g3, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE, _selectableStoreGradeHeaders, null, _sortedStoreGradeHeaders, null, false, true);
		//        }

		//        totVarProf = (AssortmentDetailVariableProfile)_detailColumnProfileList.FindKey((int)eAssortmentDetailVariables.TotalUnits);
		//        totalVarHeader = new RowColProfileHeader(totVarProf.VariableName, true, 0, totVarProf);

		//        if (_columnGroupedBy == eAllocationAssortmentViewGroupBy.StoreGrade)
		//        {
		//            g3.Rows.Count = g1.Rows.Count;
		//            g3.Cols.Count = (_sortedStoreGradeHeaders.Count + 1) * _sortedDetailColumnHeaders.Count;
		//            g3.Rows.Fixed = g1.Rows.Fixed;
		//            g3.Cols.Fixed = 0;

		//            g3.AllowDragging = AllowDraggingEnum.None;
		//            g2.AllowMerging = AllowMergingEnum.RestrictRows;

		//            i = -1;

		//            foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
		//            {
		//                varHeader = (RowColProfileHeader)varEntry.Value;

		//                i++;
		//                asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, _currStoreGroupLevelProfile.Key));
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, totalVarHeader.Profile.Key));
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

		//                g3.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, new RowColProfileHeader(_currStoreGroupLevelProfile.Name, true, 0, _currStoreGroupLevelProfile), totalVarHeader, i, _currStoreGroupLevelProfile.Name + "|" + varHeader.Name);
		//                g3.SetData(0, i, _currStoreGroupLevelProfile.Name);
		//            }

		//            foreach (DictionaryEntry gradeEntry in _sortedStoreGradeHeaders)
		//            {
		//                gradeHeader = (RowColProfileHeader)gradeEntry.Value;

		//                foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
		//                {
		//                    varHeader = (RowColProfileHeader)varEntry.Value;

		//                    i++;
		//                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, _currStoreGroupLevelProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGrade, gradeHeader.Profile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, totalVarHeader.Profile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

		//                    g3.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, gradeHeader, totalVarHeader, i, gradeHeader.Name + "|" + varHeader.Name);
		//                    g3.SetData(0, i, gradeHeader.Name);
		//                }
		//            }
		//        }
		//        else
		//        {
		//            g3.Rows.Count = g1.Rows.Count;
		//            g3.Cols.Count = _storeGroupLevelProfileList.Count * _sortedDetailColumnHeaders.Count;
		//            g3.Rows.Fixed = g1.Rows.Fixed;
		//            g3.Cols.Fixed = 0;

		//            g3.AllowDragging = AllowDraggingEnum.None;
		//            g2.AllowMerging = AllowMergingEnum.RestrictRows;

		//            i = -1;

		//            foreach (StoreGroupLevelProfile groupLevelProf in _storeGroupLevelProfileList)
		//            {
		//                groupLevelHeader = new RowColProfileHeader(groupLevelProf.Name, true, 0, groupLevelProf);

		//                foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
		//                {
		//                    varHeader = (RowColProfileHeader)varEntry.Value;

		//                    i++;
		//                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupLevelHeader.Profile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, totalVarHeader.Profile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

		//                    g3.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, groupLevelHeader, totalVarHeader, i, groupLevelHeader.Name + "|" + varHeader.Name);
		//                    g3.SetData(0, i, groupLevelHeader.Name);
		//                }
		//            }
		//        }

		//        ((PagingGridTag)g3.Tag).Visible = true;
		//        ((PagingGridTag)g3.Tag).RowsPerRowGroup = 1;
		//        ((PagingGridTag)g3.Tag).RowGroupsPerGrid = g3.Rows.Count - g3.Rows.Fixed;
		//        ((PagingGridTag)g3.Tag).RowsPerScroll = 1;
		//        ((PagingGridTag)g3.Tag).ColsPerColGroup = _sortedDetailColumnHeaders.Count;
		//        ((PagingGridTag)g3.Tag).ColGroupsPerGrid = _sortedStoreGradeHeaders.Count + 1;
		//        ((PagingGridTag)g3.Tag).ColsPerScroll = 1;

		//        for (i = 0; i < g3.Rows.Fixed; i++)
		//        {
		//            g3.Rows[i].AllowMerging = true;
		//        }

		//        _gridData[Grid3] = new CellTag[g3.Rows.Count, g3.Cols.Count];
		//        g3.Visible = ((PagingGridTag)g3.Tag).Visible;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Formatg4Grid(bool aClearGrid)
		//{
		//    int i;
		//    int j;
		//    int treeCols;
		//    int summCols;
		//    CubeWaferCoordinateList asrtWaferCoordinateList;
		//    RowColProfileHeader varHeader;
		//    AssortmentComponentVariableProfile varProf;
		//    string firstSumCol = string.Empty;
		//    int RIDValue;
		//    int headerRID;
		//    string varStr;
		//    string summTitle;
		//    string tabStr;
		//    string[] colArray;
		//    string[] textColArray;
		//    string[] keyColArray;
		//    DataTable dtGrid;
		//    DataView dvHeaders;
		//    SortedList gridRowList;
		//    GridRow gridRow;

		//    try
		//    {
		//        int seqColumns = 4;
		//        if (aClearGrid)
		//        {
		//            g4.Clear();
		//        }

		//        g4.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Clear);

		//        if (g4.Tag == null)
		//        {
		//            g4.Tag = new PagingGridTag(Grid4, g4, g4, g4, null, 0, 0, _selectableComponentColumnHeaders, null, _sortedComponentColumnHeaders, null, false, false);
		//        }

		//        i = 0;
		//        _headerCol = int.MaxValue;
		//        _placeholderCol = int.MaxValue;
		//        _packCol = int.MaxValue;
		//        // BEGIN TT#2150 - stodd - totals not showing in main matrix grid
		//        _highestPlaceholderHeaderCol = int.MaxValue;
		//        _containsBothPlaceholderAndHeader = false;
		//        // END TT#2150 - stodd - totals not showing in main matrix grid
		//        //_isHeaderSummarized = false;

		//        foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
		//        {
		//            varHeader = (RowColProfileHeader)varEntry.Value;
		//            varProf = (AssortmentComponentVariableProfile)varHeader.Profile;

		//            switch (varProf.Key)
		//            {
		//                case (int)eAssortmentComponentVariables.HeaderID:
		//                    _headerCol = i;

		//                    // BEGIN TT#2150 - stodd - totals not showing in main matrix grid
		//                    // Note: Highest col in the tree has the lowest index
		//                    if (_headerCol > -1 && _headerCol != int.MaxValue)
		//                    {
		//                        if (_headerCol < _highestPlaceholderHeaderCol)
		//                        {
		//                            _highestPlaceholderHeaderCol = _headerCol;
		//                        }
		//                    }
		//                    // END TT#2150 - stodd - totals not showing in main matrix grid

		//                    if (varHeader.IsSummarized)
		//                    {
		//                        //_isHeaderSummarized = true;
		//                    }

		//                    break;

		//                case (int)eAssortmentComponentVariables.Placeholder:
		//                    _placeholderCol = i;
		//                    // BEGIN TT#2150 - stodd - totals not showing in main matrix grid
		//                    if (_placeholderCol > -1 && _placeholderCol != int.MaxValue)
		//                    {
		//                        if (_placeholderCol < _highestPlaceholderHeaderCol)
		//                        {
		//                            _highestPlaceholderHeaderCol = _placeholderCol;
		//                        }
		//                    }
		//                    // END TT#2150 - stodd - totals not showing in main matrix grid
		//                    break;

		//                case (int)eAssortmentComponentVariables.Pack:
		//                    _packCol = i;
		//                    break;
		//            }

		//            i++;
		//        }

		//        // BEGIN TT#2150 - stodd - totals not showing in main matrix grid
		//        if ((_headerCol > -1 && _headerCol != int.MaxValue) &&
		//            (_placeholderCol > -1 && _placeholderCol != int.MaxValue))
		//        {
		//            _containsBothPlaceholderAndHeader = true;
		//        }
		//        // END TT#2150 - stodd - totals not showing in main matrix grid

		//        ((AssortmentComponentVariableProfile)_componentVariables.VariableProfileList.FindKey((int)eAssortmentComponentVariables.Pack)).UseAlternateTextColumnName =
		//            (_packCol < _headerCol && _packCol < _placeholderCol);

		//        i = 0;
		//        varStr = "";
		//        summTitle = "";

		//        // Begin TT#1322 - stodd - sorting
		//        //colArray = new string[_sortedComponentColumnHeaders.Count * 2];
		//        colArray = new string[(_sortedComponentColumnHeaders.Count * 2) + seqColumns];
		//        // End TT#1322 - stodd - sorting
		//        textColArray = new string[_sortedComponentColumnHeaders.Count];
		//        keyColArray = new string[_sortedComponentColumnHeaders.Count];
		//        treeCols = 0;
		//        summCols = 0;
		//        bool hasPlaceholder = false;
		//        bool hasHeader = false;

		//        foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
		//        {
		//            varHeader = (RowColProfileHeader)varEntry.Value;
		//            varProf = (AssortmentComponentVariableProfile)varHeader.Profile;

		//            if (varHeader.IsSummarized)
		//            {
		//                {
		//                    treeCols = 1;
		//                    summCols++;

		//                    tabStr = "";

		//                    for (j = 0; j < summCols; j++)
		//                    {
		//                        if (j == 0)
		//                        {
		//                            tabStr += "     ";
		//                        }
		//                        else
		//                        {
		//                            tabStr += "    ";
		//                        }
		//                    }

		//                    if (summTitle.Length != 0)
		//                    {
		//                        summTitle += System.Environment.NewLine;
		//                    }

		//                    summTitle += tabStr + varHeader.Name;
		//                }
		//            }

		//            if (varStr != "")
		//            {
		//                varStr += ", ";
		//            }

		//            varStr += varProf.DisplayTextColumnName;

		//            textColArray[i / 2] = varProf.DisplayTextColumnName;
		//            keyColArray[i / 2] = varProf.RIDColumnName;

		//            colArray[i] = varProf.DisplayTextColumnName;
		//            i++;
		//            colArray[i] = varProf.RIDColumnName;
		//            i++;
		//            if (varProf.ProfileListType == eProfileType.PlaceholderHeader)
		//            {
		//                hasPlaceholder = true;
		//            }
		//            else if (varProf.ProfileListType == eProfileType.AllocationHeader)
		//            {
		//                hasHeader = true;
		//            }
		//        }

		//        // Begin TT#1322 - stodd - sorting
		//        // Add these additianl fields to the GridRow.DataRow
		//        // Begin TT#1438 - stodd - sorting issue. Commented out "if" to always include.
		//        //if (hasPlaceholder)					
		//        {
		//            colArray[i] = "PLACEHOLDERSEQ";
		//            i++;
		//            colArray[i] = "PLACEHOLDERSEQ_RID";
		//            i++;
		//        }
		//        //if (hasHeader)
		//        {
		//            colArray[i] = "HEADERSEQ";
		//            i++;
		//            colArray[i] = "HEADERSEQ_RID";
		//            i++;
		//        }
		//        // End TT#1322 - stodd - sorting
		//        // End TT#1438 - stodd - sorting issue. Commented out to always include.

		//        dvHeaders = _dtHeaders.DefaultView;

		//        if (_headerCol == int.MaxValue)
		//        {
		//            dvHeaders.RowFilter = "ASSORTMENT_IND = " + bool.TrueString;
		//        }
		//        else if (_placeholderCol == int.MaxValue)
		//        {
		//            dvHeaders.RowFilter = "ASSORTMENT_IND = " + bool.FalseString;
		//        }
		//        else
		//        {
		//            dvHeaders.RowFilter = "";
		//        }

		//        dtGrid = dvHeaders.ToTable("Grid Table", true, colArray);

		//        g4.Rows.Count = dtGrid.Rows.Count + 1;
		//        // Begin TT#1476 - stodd - invalid ooperation exception opening assortment
		//        g4.Cols.Count = dtGrid.Columns.Count - 4 + treeCols;
		//        // End TT#1476
		//        g4.Rows.Fixed = 1;
		//        g4.Cols.Fixed = treeCols;

		//        g4.GetMergeData = new GetMergeData(g4GetMergeData);

		//        if (treeCols == 1)
		//        {
		//            g4.Cols[0].Name = " ";
		//        }

		//        gridRowList = new SortedList();

		//        foreach (DataRow dtRow in dtGrid.Rows)
		//        {
		//            gridRowList.Add(new GridRow(dtRow, textColArray, keyColArray), null);
		//        }

		//        for (i = 0; i < textColArray.Length; i++)
		//        {
		//            g4.Cols[(i * 2) + treeCols].Name = textColArray[i];
		//            g4.Cols[(i * 2) + treeCols + 1].Name = keyColArray[i];
		//        }

		//        i = g4.Rows.Fixed;

		//        IDictionaryEnumerator dictEnum = gridRowList.GetEnumerator();

		//        while (dictEnum.MoveNext())
		//        {
		//            gridRow = (GridRow)dictEnum.Key;

		//            for (j = 0; j < gridRow.TextArray.Length; j++)
		//            {
		//                g4.SetData(i, (j * 2) + treeCols, gridRow.TextArray[j]);
		//                g4.SetData(i, (j * 2) + treeCols + 1, gridRow.KeyArray[j]);
		//            }

		//            i++;
		//        }

		//        if (treeCols == 1)
		//        {
		//            g4.SetData(0, 0, summTitle);
		//            g4.Tree.Style = C1.Win.C1FlexGrid.TreeStyleFlags.Simple;
		//            g4.Tree.Column = 0;
		//        }

		//        g4.AllowDragging = AllowDraggingEnum.None;
		//        g4.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictCols;

		//        foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
		//        {
		//            varHeader = (RowColProfileHeader)varEntry.Value;
		//            varProf = (AssortmentComponentVariableProfile)varHeader.Profile;

		//            g4.Cols[varProf.RIDColumnName].Visible = false;

		//            if (varHeader.IsSummarized)
		//            {
		//                // Begin TT#1227 - stodd *REMOVED for TT#1322*
		//                // Begin TT#1227 - stodd *REMOVED for TT#1322*
		//                //if (varHeader.Profile.Key != (int)eAssortmentComponentVariables.PlaceholderSeq
		//                //    && varHeader.Profile.Key != (int)eAssortmentComponentVariables.HeaderSeq)
		//                // End TT#1227 - stodd
		//                {
		//                    g4.SetData(0, varProf.DisplayTextColumnName, " ");
		//                    g4.Cols[varProf.DisplayTextColumnName].Visible = false;
		//                }
		//                // End TT#1227 - stodd
		//            }
		//            else
		//            {
		//                g4.SetData(0, varProf.DisplayTextColumnName, varHeader.Name);
		//            }
		//        }

		//        if (treeCols == 1)
		//        {
		//            g4.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Clear);
		//            i = 1;
		//            foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
		//            {
		//                // Begin TT#1227 - stodd *REMOVED for TT#1322*
		//                // Begin TT#1227 - stodd *REMOVED for TT#1322*
		//                //if (((RowColProfileHeader)varEntry.Value).Profile.Key != (int)eAssortmentComponentVariables.PlaceholderSeq
		//                //	&& ((RowColProfileHeader)varEntry.Value).Profile.Key != (int)eAssortmentComponentVariables.HeaderSeq)
		//                // End TT#1227 - stodd
		//                {
		//                    // End TT#1227 - stodd
		//                    varHeader = (RowColProfileHeader)varEntry.Value;
		//                    varProf = (AssortmentComponentVariableProfile)varHeader.Profile;

		//                    if (((RowColProfileHeader)varEntry.Value).IsSummarized)
		//                    {
		//                        // Begin TT#1227 - stodd *REMOVED for TT#1322*
		//                        //if (((RowColProfileHeader)varEntry.Value).Profile.Key != (int)eAssortmentComponentVariables.PlaceholderSeq
		//                        //    && ((RowColProfileHeader)varEntry.Value).Profile.Key != (int)eAssortmentComponentVariables.HeaderSeq)
		//                        {
		//                            if (i == 1)
		//                            {
		//                                firstSumCol = varProf.DisplayTextColumnName;
		//                            }

		//                            g4.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.None, i, firstSumCol, varProf.DisplayTextColumnName, varProf.DisplayTextColumnName, "{0}");
		//                            i++;
		//                        }
		//                        // End TT#1227 - stodd
		//                    }
		//                    else
		//                    {
		//                        varHeader = (RowColProfileHeader)varEntry.Value;

		//                        asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentComponentVariable, varHeader.Profile.Key));

		//                        g4.Cols[varProf.DisplayTextColumnName].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, null, varHeader, g4.Cols[varProf.DisplayTextColumnName].Index, varHeader.Name);
		//                    }
		//                }
		//            }
		//        }

		//        foreach (C1.Win.C1FlexGrid.Row row in g4.Rows)
		//        {
		//            if (row.Index < g4.Rows.Fixed)
		//            {
		//                row.UserData = new RowHeaderTag(new CubeWaferCoordinateList(), null, null, row.Index, string.Empty, string.Empty, false);
		//            }
		//            else if (row.IsNode)
		//            {
		//                //varHeader = (RowColProfileHeader)_sortedComponentColumnHeaders.GetByIndex(row.Node.Level - 1);
		//                //varProf = (AssortmentComponentVariableProfile)varHeader.Profile;
		//                //row.Node.Data = GetFirstDetailRow(g4, row.Node)[varProf.DisplayTextColumnName];

		//                asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, row.Node.Level));

		//                headerRID = -1;

		//                for (i = 0; i <= row.Node.Level; i++)
		//                {
		//                    varHeader = (RowColProfileHeader)_sortedComponentColumnHeaders.GetByIndex(i);
		//                    varProf = (AssortmentComponentVariableProfile)varHeader.Profile;
		//                    RIDValue = Convert.ToInt32(GetFirstDetailRow(g4, row.Node)[varProf.RIDColumnName]);
		//                    // BEGIN TT#212-MD - stodd - Matrix and content tabs out of sync - 
		//                    if (i == row.Node.Level)
		//                    {
		//                        RIDValue = int.MaxValue;
		//                    }
		//                    // END TT#212-MD - stodd - Matrix and content tabs out of sync - 
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(varProf.ProfileListType, RIDValue));

		//                    if (varProf.HeaderComponent)
		//                    {
		//                        headerRID = RIDValue;
		//                    }
		//                }

		//                if (_containsBothPlaceholderAndHeader && row.Node.Level < _highestPlaceholderHeaderCol + 1)
		//                {

		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.TotalVariableProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));

		//                }
		//                // placeholder is just above header
		//                //else if (_containsBothPlaceholderAndHeader && (_placeholderCol + 1) == _headerCol && row.Node.Level < _headerCol)
		//                else if (_containsBothPlaceholderAndHeader && (_placeholderCol + 1) == _headerCol)
		//                {

		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.TotalVariableProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));

		//                }
		//                // Level is above header 
		//                else if (row.Node.Level < _headerCol + 1 && !_containsBothPlaceholderAndHeader)
		//                {
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlaceholderHeader, 0));
		//                }
		//                else if (row.Node.Level < _placeholderCol + 1 && !_containsBothPlaceholderAndHeader)
		//                {
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
		//                }
		//                else
		//                {
		//                    if (headerRID == int.MaxValue)
		//                    {
		//                        // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
		//                        //asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.DifferenceVariableProfile.Key));
		//                        // BEGIN TT#2150 - stodd - totals not showing in main matrix grid
		//                        //if (row.Caption == "Difference")
		//                        if (row.Caption == MIDText.GetTextOnly(eMIDTextCode.msg_as_PlaceholderBalance))
		//                        // END TT#2150 - stodd - totals not showing in main matrix grid
		//                        {
		//                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.DifferenceVariableProfile.Key));
		//                        }
		//                        else
		//                        {
		//                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
		//                        }
		//                        // End TT#2
		//                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
		//                    }
		//                    else
		//                    {
		//                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

		//                        if (headerRID == -1)
		//                        {
		//                            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
		//                        }
		//                    }
		//                }

		//                row.UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, row.Index, string.Empty, string.Empty);
		//            }
		//            else
		//            {
		//                asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//                headerRID = -1;

		//                foreach (DictionaryEntry varEntry in _sortedComponentColumnHeaders)
		//                {
		//                    varHeader = (RowColProfileHeader)varEntry.Value;
		//                    varProf = (AssortmentComponentVariableProfile)varHeader.Profile;
		//                    RIDValue = Convert.ToInt32(row[varProf.RIDColumnName]);
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(varProf.ProfileListType, RIDValue));

		//                    if (varProf.HeaderComponent)
		//                    {
		//                        headerRID = RIDValue;
		//                    }
		//                }

		//                if (headerRID == int.MaxValue)
		//                {
		//                    // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
		//                    //asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.DifferenceVariableProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
		//                    // End TT#2
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
		//                }
		//                else
		//                {
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));

		//                    if (headerRID == -1)
		//                    {
		//                        asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
		//                    }
		//                }

		//                row.UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, row.Index, string.Empty, string.Empty);
		//            }
		//        }

		//        g4.Rows[0].AllowMerging = true;

		//        for (i = treeCols; i < g4.Cols.Count; i++)
		//        {
		//            g4.Cols[i].AllowMerging = true;
		//        }

		//        g4.AutoSizeCols();

		//        ((PagingGridTag)g4.Tag).Visible = true;
		//        ((PagingGridTag)g4.Tag).RowsPerRowGroup = 1;
		//        ((PagingGridTag)g4.Tag).RowGroupsPerGrid = g4.Rows.Count - g4.Rows.Fixed;
		//        ((PagingGridTag)g4.Tag).RowsPerScroll = 1;
		//        ((PagingGridTag)g4.Tag).ColsPerColGroup = 1;
		//        ((PagingGridTag)g4.Tag).ColGroupsPerGrid = g4.Cols.Count;
		//        ((PagingGridTag)g4.Tag).ColsPerScroll = 1;

		//        _gridData[Grid4] = new CellTag[g4.Rows.Count, g4.Cols.Count];
		//        g4.Visible = true;
		//        // Begin TT#1227 - stodd - sequencing *REMOVED for TT#1322*
		//        //if (g4.Cols["PLACEHOLDERSEQ"] != null)
		//        //{
		//        //    g4.Cols["PLACEHOLDERSEQ"].Visible = false;
		//        //    g4.Cols["PLACEHOLDERSEQ_RID"].Visible = false;
		//        //}
		//        //if (g4.Cols["HEADERSEQ"] != null)
		//        //{
		//        //    g4.Cols["HEADERSEQ"].Visible = false;
		//        //    g4.Cols["HEADERSEQ_RID"].Visible = false;
		//        //}
		//        // End TT#1227 - stodd - sequencing
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private Row GetFirstDetailRow(C1FlexGrid aGrid, Node aNode)
		//{
		//    try
		//    {
		//        if (aNode.Children == 0)
		//        {
		//            return aGrid.Rows[aNode.GetCellRange().TopRow + 1];
		//        }
		//        else
		//        {
		//            return GetFirstDetailRow(aGrid, aNode.GetNode(NodeTypeEnum.FirstChild));
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Formatg5Grid(bool aClearGrid)
		//{
		//    int i;
		//    //int j;
		//    CubeWaferCoordinateList asrtWaferCoordinateList;
		//    RowColProfileHeader varHeader;

		//    try
		//    {
		//        if (aClearGrid)
		//        {
		//            g5.Clear();
		//        }

		//        if (g5.Tag == null)
		//        {
		//            g5.Tag = new PagingGridTag(Grid5, g5, g4, g5, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE, _selectableTotalColumnHeaders, null, _sortedTotalColumnHeaders, null, false, false);
		//        }


		//        g5.Rows.Count = g4.Rows.Count;
		//        g5.Cols.Count = g2.Cols.Count;
		//        g5.Rows.Fixed = g4.Rows.Fixed;
		//        g5.Cols.Fixed = g2.Cols.Fixed;

		//        g5.AllowDragging = AllowDraggingEnum.None;

		//        i = -1;

		//        foreach (DictionaryEntry varEntry in _sortedTotalColumnHeaders)
		//        {
		//            varHeader = (RowColProfileHeader)varEntry.Value;

		//            i++;
		//            asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentTotalVariable, varHeader.Profile.Key));
		//            g5.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, null, varHeader, i, ((AssortmentTotalVariableProfile)varHeader.Profile).VariableName);
		//            g5.SetData(0, i, ((AssortmentTotalVariableProfile)varHeader.Profile).VariableName);
		//        }

		//        ((PagingGridTag)g5.Tag).Visible = true;
		//        ((PagingGridTag)g5.Tag).RowsPerRowGroup = 1;
		//        ((PagingGridTag)g5.Tag).RowGroupsPerGrid = g5.Rows.Count - g5.Rows.Fixed;
		//        ((PagingGridTag)g5.Tag).RowsPerScroll = 1;
		//        ((PagingGridTag)g5.Tag).ColsPerColGroup = 1;
		//        ((PagingGridTag)g5.Tag).ColGroupsPerGrid = g5.Cols.Count;
		//        ((PagingGridTag)g5.Tag).ColsPerScroll = 1;

		//        _gridData[Grid5] = new CellTag[g5.Rows.Count, g5.Cols.Count];
		//        g5.Visible = ((PagingGridTag)g5.Tag).Visible;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Formatg6Grid(bool aClearGrid)
		//{
		//    int i;
		//    CubeWaferCoordinateList asrtWaferCoordinateList;
		//    RowColProfileHeader gradeHeader;
		//    RowColProfileHeader groupLevelHeader;
		//    RowColProfileHeader varHeader;

		//    try
		//    {
		//        if (aClearGrid)
		//        {
		//            g6.Clear();
		//        }

		//        if (g6.Tag == null)
		//        {
		//            g6.Tag = new PagingGridTag(Grid6, g6, g4, g6, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE, _selectableDetailColumnHeaders, null, _sortedDetailColumnHeaders, null, false, true);
		//        }

		//        if (_columnGroupedBy == eAllocationAssortmentViewGroupBy.StoreGrade)
		//        {
		//            g6.Rows.Count = g4.Rows.Count;
		//            g6.Cols.Count = g3.Cols.Count;
		//            g6.Rows.Fixed = g4.Rows.Fixed;
		//            g6.Cols.Fixed = g3.Cols.Fixed;

		//            g6.AllowDragging = AllowDraggingEnum.None;

		//            i = -1;

		//            foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
		//            {
		//                varHeader = (RowColProfileHeader)varEntry.Value;

		//                i++;
		//                asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, _currStoreGroupLevelProfile.Key));
		//                asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, varHeader.Profile.Key));

		//                g6.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, new RowColProfileHeader(_currStoreGroupLevelProfile.Name, true, 0, _currStoreGroupLevelProfile), varHeader, i, varHeader.Name);
		//                g6.SetData(0, i, varHeader.Name);
		//            }

		//            foreach (DictionaryEntry gradeEntry in _sortedStoreGradeHeaders)
		//            {
		//                gradeHeader = (RowColProfileHeader)gradeEntry.Value;

		//                foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
		//                {
		//                    varHeader = (RowColProfileHeader)varEntry.Value;

		//                    i++;
		//                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, _currStoreGroupLevelProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGrade, gradeHeader.Profile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, varHeader.Profile.Key));

		//                    g6.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, gradeHeader, varHeader, i, varHeader.Name);
		//                    g6.SetData(0, i, varHeader.Name);
		//                }
		//            }
		//        }
		//        else
		//        {
		//            g6.Rows.Count = g4.Rows.Count;
		//            g6.Cols.Count = g3.Cols.Count;
		//            g6.Rows.Fixed = g4.Rows.Fixed;
		//            g6.Cols.Fixed = g3.Cols.Fixed;

		//            g6.AllowDragging = AllowDraggingEnum.None;

		//            i = -1;

		//            foreach (StoreGroupLevelProfile groupLevelProf in _storeGroupLevelProfileList)
		//            {
		//                groupLevelHeader = new RowColProfileHeader(groupLevelProf.Name, true, 0, groupLevelProf);

		//                foreach (DictionaryEntry varEntry in _sortedDetailColumnHeaders)
		//                {
		//                    varHeader = (RowColProfileHeader)varEntry.Value;

		//                    i++;
		//                    asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroup, _asrtCubeGroup.CurrentStoreGroupProfile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupLevelHeader.Profile.Key));
		//                    asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentDetailVariable, varHeader.Profile.Key));

		//                    g6.Cols[i].UserData = new ColumnHeaderTag(asrtWaferCoordinateList, groupLevelHeader, varHeader, i, varHeader.Name);
		//                    g6.SetData(0, i, varHeader.Name);
		//                }
		//            }
		//        }

		//        ((PagingGridTag)g6.Tag).Visible = true;
		//        ((PagingGridTag)g6.Tag).RowsPerRowGroup = 1;
		//        ((PagingGridTag)g6.Tag).RowGroupsPerGrid = g6.Rows.Count - g6.Rows.Fixed;
		//        ((PagingGridTag)g6.Tag).RowsPerScroll = 1;
		//        ((PagingGridTag)g6.Tag).ColsPerColGroup = _sortedDetailColumnHeaders.Count;
		//        ((PagingGridTag)g6.Tag).ColGroupsPerGrid = _sortedStoreGradeHeaders.Count + 1;
		//        ((PagingGridTag)g6.Tag).ColsPerScroll = 1;

		//        _gridData[Grid6] = new CellTag[g6.Rows.Count, g6.Cols.Count];
		//        g6.Visible = ((PagingGridTag)g6.Tag).Visible;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Formatg7Grid(bool aClearGrid)
		//{
		//    CubeWaferCoordinateList asrtWaferCoordinateList;
		//    int i;

		//    try
		//    {
		//        if (aClearGrid)
		//        {
		//            g7.Clear();
		//        }

		//        if (g7.Tag == null)
		//        {
		//            g7.Tag = new PagingGridTag(Grid7, g7, g4, null, null, 0, 0);
		//        }
		//        // BEGIN TT#2148 - stodd - Assortment totals do not include header values
		//        //if (_windowType == eAssortmentWindowType.Assortment)
		//        if (IsPostReceiptAssortment())
		//        {
		//            g7.Rows.Count = 2;
		//        }
		//        else
		//        {
		//            g7.Rows.Count = 4;
		//        }
		//        // END TT#2148 - stodd - Assortment totals do not include header values

		//        g7.Cols.Count = g1.Cols.Count;
		//        g7.Rows.Fixed = 0;
		//        g7.Cols.Fixed = g1.Cols.Fixed;

		//        g7.AllowDragging = AllowDraggingEnum.None;
		//        g7.AllowMerging = AllowMergingEnum.Free;

		//        // BEGIN TT#2148 - stodd - Assortment totals do not include header values
		//        //if (_windowType == eAssortmentWindowType.Assortment)
		//        if (IsPostReceiptAssortment())
		//        {
		//            // Add Header Total Row
		//            asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AllocationHeader, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
		//            g7.Rows[0].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 0, string.Empty, string.Empty);

		//            // Add Balance Row
		//            asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, ((AssortmentViewQuantityVariables)_quantityVariables).Balance.Key));

		//            g7.Rows[1].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 1, string.Empty, string.Empty);
		//        }
		//        else
		//        {

		//            // Add Placeholder Total Row
		//            asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
		//            g7.Rows[0].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 0, string.Empty, string.Empty);

		//            // Add Header Total Row
		//            asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AllocationHeader, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, _quantityVariables.ValueVariableProfile.Key));
		//            g7.Rows[1].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 1, string.Empty, string.Empty);

		//            // Add Total Row
		//            asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, ((AssortmentViewQuantityVariables)_quantityVariables).Total.Key));

		//            g7.Rows[2].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 2, string.Empty, string.Empty);

		//            // Add Balance Row
		//            asrtWaferCoordinateList = new CubeWaferCoordinateList();
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Placeholder, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentSubTotal, 0));
		//            asrtWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.AssortmentQuantityVariable, ((AssortmentViewQuantityVariables)_quantityVariables).Balance.Key));

		//            g7.Rows[3].UserData = new RowHeaderTag(asrtWaferCoordinateList, null, null, 3, string.Empty, string.Empty);
		//        }


		//        for (i = 0; i < g7.Cols.Count; i++)
		//        {
		//            //if (_windowType == eAssortmentWindowType.Assortment)
		//            if (IsPostReceiptAssortment())
		//            {
		//                g7.SetData(0, i, "Header Total");
		//                g7.SetData(1, i, "Balance");
		//            }
		//            else
		//            {
		//                g7.SetData(0, i, "Placeholder Total");
		//                g7.SetData(1, i, "Header Total");
		//                g7.SetData(2, i, "Total");
		//                g7.SetData(3, i, "Balance");
		//            }

		//        }
		//        // END TT#2148 - stodd - Assortment totals do not include header values

		//        for (i = 0; i < g4.Cols.Count; i++)
		//        {
		//            g7.Cols[i].Visible = g4.Cols[i].Visible;
		//        }

		//        ((PagingGridTag)g7.Tag).Visible = true;
		//        ((PagingGridTag)g7.Tag).RowsPerRowGroup = 1;
		//        ((PagingGridTag)g7.Tag).RowGroupsPerGrid = 1;
		//        ((PagingGridTag)g7.Tag).RowsPerScroll = 1;
		//        ((PagingGridTag)g7.Tag).ColsPerColGroup = 1;
		//        ((PagingGridTag)g7.Tag).ColGroupsPerGrid = g7.Cols.Count;
		//        ((PagingGridTag)g7.Tag).ColsPerScroll = 1;

		//        foreach (Row row in g7.Rows)
		//        {
		//            row.AllowMerging = true;
		//        }

		//        _gridData[Grid7] = new CellTag[g7.Rows.Count, g7.Cols.Count];
		//        g7.Visible = ((PagingGridTag)g7.Tag).Visible;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Formatg8Grid(bool aClearGrid)
		//{
		//    try
		//    {
		//        if (aClearGrid)
		//        {
		//            g8.Clear();
		//        }

		//        if (g8.Tag == null)
		//        {
		//            g8.Tag = new PagingGridTag(Grid8, g8, g7, g5, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
		//        }

		//        g8.Rows.Count = g7.Rows.Count;
		//        g8.Cols.Count = g2.Cols.Count;
		//        g8.Rows.Fixed = g7.Rows.Fixed;
		//        g8.Cols.Fixed = g2.Cols.Fixed;

		//        ((PagingGridTag)g8.Tag).Visible = true;
		//        ((PagingGridTag)g8.Tag).RowsPerRowGroup = 1;
		//        ((PagingGridTag)g8.Tag).RowGroupsPerGrid = g8.Rows.Count - g8.Rows.Fixed;
		//        ((PagingGridTag)g8.Tag).RowsPerScroll = 1;
		//        ((PagingGridTag)g8.Tag).ColsPerColGroup = 1;
		//        ((PagingGridTag)g8.Tag).ColGroupsPerGrid = g8.Cols.Count;
		//        ((PagingGridTag)g8.Tag).ColsPerScroll = 1;

		//        _gridData[Grid8] = new CellTag[g8.Rows.Count, g8.Cols.Count];
		//        g8.Visible = true;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Formatg9Grid(bool aClearGrid)
		//{
		//    try
		//    {
		//        if (aClearGrid)
		//        {
		//            g9.Clear();
		//        }

		//        if (g9.Tag == null)
		//        {
		//            g9.Tag = new PagingGridTag(Grid9, g9, g7, g6, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE, null, null, null, null, false, true);
		//        }

		//        g9.Rows.Count = g7.Rows.Count;
		//        g9.Cols.Count = g3.Cols.Count;
		//        g9.Rows.Fixed = g7.Rows.Fixed;
		//        g9.Cols.Fixed = g3.Cols.Fixed;

		//        ((PagingGridTag)g9.Tag).Visible = true;
		//        ((PagingGridTag)g9.Tag).RowsPerRowGroup = 1;
		//        ((PagingGridTag)g9.Tag).RowGroupsPerGrid = g9.Rows.Count - g9.Rows.Fixed;
		//        ((PagingGridTag)g9.Tag).RowsPerScroll = 1;
		//        ((PagingGridTag)g9.Tag).ColsPerColGroup = _sortedDetailColumnHeaders.Count;
		//        ((PagingGridTag)g9.Tag).ColGroupsPerGrid = _sortedStoreGradeHeaders.Count + 1;
		//        ((PagingGridTag)g9.Tag).ColsPerScroll = 1;

		//        _gridData[Grid9] = new CellTag[g9.Rows.Count, g9.Cols.Count];
		//        g9.Visible = true;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//#endregion

		//#region Preferences, Positioning, Resizing, Freezing (Cols & Rows)

		//private int CalculateGroupFromDetail(C1FlexGrid aGrid, int aDetail, int aUnitsPerScroll)
		//{
		//    try
		//    {
		//        if (aUnitsPerScroll > 0)
		//        {
		//            int result;
		//            if (((PagingGridTag)aGrid.Tag).ScrollType == eScrollType.Group)
		//            {
		//                //return (aDetail / aUnitsPerScroll) - aGrid.Rows.Fixed;
		//                result = (aDetail / aUnitsPerScroll) - aGrid.Rows.Fixed;
		//            }
		//            else
		//            {
		//                //return aDetail - aGrid.Rows.Fixed;
		//                result = aDetail - aGrid.Rows.Fixed;
		//            }
		//            if (result < 0)
		//            {
		//                result = 0;
		//            }
		//            return result;
		//        }
		//        else
		//        {
		//            return 0;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ResizeCol1()
		//{
		//    C1FlexGrid grid = null;
		//    int i;
		//    int MinColWidth = 0;

		//    try
		//    {
		//        if (g1.Cols.Fixed > 0)
		//        {
		//            g1.AutoSizeCols(0, 0, g1.Rows.Count - 1, g1.Cols.Fixed - 1, 0, AutoSizeFlags.None);
		//            g4.AutoSizeCols(0, 0, g4.Rows.Count - 1, g4.Cols.Fixed - 1, 0, AutoSizeFlags.None);
		//            g7.AutoSizeCols(0, 0, g7.Rows.Count - 1, g7.Cols.Fixed - 1, 0, AutoSizeFlags.None);
		//        }

		//        g1.AutoSizeCols(g1.Rows.Fixed, g1.Cols.Fixed, g1.Rows.Count - 1, g1.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g4.AutoSizeCols(g4.Rows.Fixed, g1.Cols.Fixed, g4.Rows.Count - 1, g4.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g7.AutoSizeCols(g7.Rows.Fixed, g1.Cols.Fixed, g7.Rows.Count - 1, g7.Cols.Count - 1, 0, AutoSizeFlags.None);

		//        for (i = 0; i < g4.Cols.Count; i++)
		//        {
		//            MinColWidth = FindMinimumColWidth(ref grid, i, g1, g4, g7);

		//            if (((PagingGridTag)g1.Tag).Visible)
		//            {
		//                MinColWidth = Math.Max(MinColWidth, g1.Cols[i].WidthDisplay);
		//            }
		//            if (((PagingGridTag)g4.Tag).Visible)
		//            {
		//                MinColWidth = Math.Max(MinColWidth, g4.Cols[i].WidthDisplay);
		//            }
		//            if (((PagingGridTag)g7.Tag).Visible)
		//            {
		//                MinColWidth = Math.Max(MinColWidth, g7.Cols[i].WidthDisplay);
		//            }

		//            if (((PagingGridTag)g1.Tag).Visible && g1.Cols[i].Visible)
		//            {
		//                g1.Cols[i].WidthDisplay = MinColWidth;
		//            }
		//            if (((PagingGridTag)g4.Tag).Visible && g4.Cols[i].Visible)
		//            {
		//                g4.Cols[i].WidthDisplay = MinColWidth;
		//            }
		//            if (((PagingGridTag)g7.Tag).Visible && g7.Cols[i].Visible)
		//            {
		//                g7.Cols[i].WidthDisplay = MinColWidth;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ResizeCol2()
		//{
		//    C1FlexGrid grid = null;
		//    int i;
		//    int MinColWidth = 0;

		//    try
		//    {
		//        g2.AutoSizeCols(g2.Rows.Fixed, 0, g2.Rows.Count - 1, g2.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g5.AutoSizeCols(g5.Rows.Fixed, 0, g5.Rows.Count - 1, g5.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g8.AutoSizeCols(g8.Rows.Fixed, 0, g8.Rows.Count - 1, g8.Cols.Count - 1, 0, AutoSizeFlags.None);

		//        for (i = 0; i < g2.Cols.Count; i++)
		//        {
		//            MinColWidth = FindMinimumColWidth(ref grid, i, g2, g5, g8);

		//            if (((PagingGridTag)g2.Tag).Visible)
		//            {
		//                MinColWidth = Math.Max(MinColWidth, g2.Cols[i].WidthDisplay);
		//            }
		//            if (((PagingGridTag)g5.Tag).Visible)
		//            {
		//                MinColWidth = Math.Max(MinColWidth, g5.Cols[i].WidthDisplay);
		//            }
		//            if (((PagingGridTag)g8.Tag).Visible)
		//            {
		//                MinColWidth = Math.Max(MinColWidth, g8.Cols[i].WidthDisplay);
		//            }

		//            if (((PagingGridTag)g2.Tag).Visible && g2.Cols[i].Visible)
		//            {
		//                g2.Cols[i].WidthDisplay = MinColWidth;
		//            }
		//            if (((PagingGridTag)g5.Tag).Visible && g5.Cols[i].Visible)
		//            {
		//                g5.Cols[i].WidthDisplay = MinColWidth;
		//            }
		//            if (((PagingGridTag)g8.Tag).Visible && g8.Cols[i].Visible)
		//            {
		//                g8.Cols[i].WidthDisplay = MinColWidth;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ResizeCol3()
		//{
		//    C1FlexGrid grid = null;
		//    int i;
		//    int MinColWidth = 0;

		//    try
		//    {
		//        g3.AutoSizeCols(g3.Rows.Fixed, 0, g3.Rows.Count - 1, g3.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g6.AutoSizeCols(g6.Rows.Fixed, 0, g6.Rows.Count - 1, g6.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g9.AutoSizeCols(g9.Rows.Fixed, 0, g9.Rows.Count - 1, g9.Cols.Count - 1, 0, AutoSizeFlags.None);

		//        for (i = 0; i < g3.Cols.Count; i++)
		//        {
		//            MinColWidth = FindMinimumColWidth(ref grid, i, g3, g6, g9);

		//            if (((PagingGridTag)g3.Tag).Visible)
		//            {
		//                MinColWidth = Math.Max(MinColWidth, g3.Cols[i].WidthDisplay);
		//            }
		//            if (((PagingGridTag)g6.Tag).Visible)
		//            {
		//                MinColWidth = Math.Max(MinColWidth, g6.Cols[i].WidthDisplay);
		//            }
		//            if (((PagingGridTag)g9.Tag).Visible)
		//            {
		//                MinColWidth = Math.Max(MinColWidth, g9.Cols[i].WidthDisplay);
		//            }

		//            if (((PagingGridTag)g3.Tag).Visible && g3.Cols[i].Visible)
		//            {
		//                g3.Cols[i].WidthDisplay = MinColWidth;
		//            }
		//            if (((PagingGridTag)g6.Tag).Visible && g6.Cols[i].Visible)
		//            {
		//                g6.Cols[i].WidthDisplay = MinColWidth;
		//            }
		//            if (((PagingGridTag)g9.Tag).Visible && g9.Cols[i].Visible)
		//            {
		//                g9.Cols[i].WidthDisplay = MinColWidth;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ResizeRow1(bool aFixedOnly)
		//{
		//    int i;
		//    int MinRowHeight = 0;

		//    try
		//    {
		//        g1.AutoSizeRows(0, 0, g1.Rows.Fixed - 1, g1.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g2.AutoSizeRows(0, 0, g2.Rows.Fixed - 1, g2.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g3.AutoSizeRows(0, 0, g3.Rows.Fixed - 1, g3.Cols.Count - 1, 0, AutoSizeFlags.None);

		//        for (i = 0; i < g1.Rows.Fixed; i++)
		//        {
		//            if (((PagingGridTag)g1.Tag).Visible)
		//            {
		//                MinRowHeight = g1.Rows[i].HeightDisplay;
		//            }
		//            if (((PagingGridTag)g2.Tag).Visible)
		//            {
		//                MinRowHeight = Math.Max(MinRowHeight, g2.Rows[i].HeightDisplay);
		//            }
		//            if (((PagingGridTag)g3.Tag).Visible)
		//            {
		//                MinRowHeight = Math.Max(MinRowHeight, g3.Rows[i].HeightDisplay);
		//            }

		//            if (((PagingGridTag)g1.Tag).Visible)
		//            {
		//                g1.Rows[i].HeightDisplay = MinRowHeight;
		//            }
		//            if (((PagingGridTag)g2.Tag).Visible)
		//            {
		//                g2.Rows[i].HeightDisplay = MinRowHeight;
		//            }
		//            if (((PagingGridTag)g3.Tag).Visible)
		//            {
		//                g3.Rows[i].HeightDisplay = MinRowHeight;
		//            }
		//        }

		//        if (!aFixedOnly)
		//        {
		//            g1.AutoSizeRows(g1.Rows.Fixed, 0, g1.Rows.Count - 1, g1.Cols.Count - 1, 0, AutoSizeFlags.None);
		//            g2.AutoSizeRows(g2.Rows.Fixed, 0, g2.Rows.Count - 1, g2.Cols.Count - 1, 0, AutoSizeFlags.None);
		//            g3.AutoSizeRows(g3.Rows.Fixed, 0, g3.Rows.Count - 1, g3.Cols.Count - 1, 0, AutoSizeFlags.None);

		//            MinRowHeight = FindMinimumRowHeight(g1, g2, g3);

		//            for (i = g1.Rows.Fixed; i < g1.Rows.Count; i++)
		//            {
		//                if (((PagingGridTag)g1.Tag).Visible)
		//                {
		//                    g1.Rows[i].HeightDisplay = MinRowHeight;
		//                }
		//                if (((PagingGridTag)g2.Tag).Visible)
		//                {
		//                    g2.Rows[i].HeightDisplay = MinRowHeight;
		//                }
		//                if (((PagingGridTag)g3.Tag).Visible)
		//                {
		//                    g3.Rows[i].HeightDisplay = MinRowHeight;
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ResizeRow4(bool aFixedOnly)
		//{
		//    int i;
		//    int MinRowHeight = 0;

		//    try
		//    {
		//        g4.AutoSizeRows(0, 0, g4.Rows.Fixed - 1, g4.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g5.AutoSizeRows(0, 0, g5.Rows.Fixed - 1, g5.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g6.AutoSizeRows(0, 0, g6.Rows.Fixed - 1, g6.Cols.Count - 1, 0, AutoSizeFlags.None);

		//        for (i = 0; i < g4.Rows.Fixed; i++)
		//        {
		//            if (((PagingGridTag)g4.Tag).Visible)
		//            {
		//                MinRowHeight = g4.Rows[i].HeightDisplay;
		//            }
		//            if (((PagingGridTag)g5.Tag).Visible)
		//            {
		//                MinRowHeight = Math.Max(MinRowHeight, g5.Rows[i].HeightDisplay);
		//            }
		//            if (((PagingGridTag)g6.Tag).Visible)
		//            {
		//                MinRowHeight = Math.Max(MinRowHeight, g6.Rows[i].HeightDisplay);
		//            }

		//            if (((PagingGridTag)g4.Tag).Visible)
		//            {
		//                g4.Rows[i].HeightDisplay = MinRowHeight;
		//            }
		//            if (((PagingGridTag)g5.Tag).Visible)
		//            {
		//                g5.Rows[i].HeightDisplay = MinRowHeight;
		//            }
		//            if (((PagingGridTag)g6.Tag).Visible)
		//            {
		//                g6.Rows[i].HeightDisplay = MinRowHeight;
		//            }
		//        }

		//        if (!aFixedOnly)
		//        {
		//            g4.AutoSizeRows(g4.Rows.Fixed, 0, g4.Rows.Count - 1, g4.Cols.Count - 1, 0, AutoSizeFlags.None);
		//            g5.AutoSizeRows(g5.Rows.Fixed, 0, g5.Rows.Count - 1, g5.Cols.Count - 1, 0, AutoSizeFlags.None);
		//            g6.AutoSizeRows(g6.Rows.Fixed, 0, g6.Rows.Count - 1, g6.Cols.Count - 1, 0, AutoSizeFlags.None);

		//            MinRowHeight = FindMinimumRowHeight(g4, g5, g6);

		//            for (i = g4.Rows.Fixed; i < g4.Rows.Count; i++)
		//            {
		//                if (((PagingGridTag)g4.Tag).Visible)
		//                {
		//                    g4.Rows[i].HeightDisplay = MinRowHeight;
		//                }
		//                if (((PagingGridTag)g5.Tag).Visible)
		//                {
		//                    g5.Rows[i].HeightDisplay = MinRowHeight;
		//                }
		//                if (((PagingGridTag)g6.Tag).Visible)
		//                {
		//                    g6.Rows[i].HeightDisplay = MinRowHeight;
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ResizeRow7(bool aFixedOnly)
		//{
		//    int i;
		//    int MinRowHeight = 0;

		//    try
		//    {
		//        g7.AutoSizeRows(0, 0, g7.Rows.Fixed - 1, g7.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g8.AutoSizeRows(0, 0, g8.Rows.Fixed - 1, g8.Cols.Count - 1, 0, AutoSizeFlags.None);
		//        g9.AutoSizeRows(0, 0, g9.Rows.Fixed - 1, g9.Cols.Count - 1, 0, AutoSizeFlags.None);

		//        for (i = 0; i < g7.Rows.Fixed; i++)
		//        {
		//            if (((PagingGridTag)g7.Tag).Visible)
		//            {
		//                MinRowHeight = g7.Rows[i].HeightDisplay;
		//            }
		//            if (((PagingGridTag)g8.Tag).Visible)
		//            {
		//                MinRowHeight = Math.Max(MinRowHeight, g8.Rows[i].HeightDisplay);
		//            }
		//            if (((PagingGridTag)g9.Tag).Visible)
		//            {
		//                MinRowHeight = Math.Max(MinRowHeight, g9.Rows[i].HeightDisplay);
		//            }

		//            if (((PagingGridTag)g7.Tag).Visible)
		//            {
		//                g7.Rows[i].HeightDisplay = MinRowHeight;
		//            }
		//            if (((PagingGridTag)g8.Tag).Visible)
		//            {
		//                g8.Rows[i].HeightDisplay = MinRowHeight;
		//            }
		//            if (((PagingGridTag)g9.Tag).Visible)
		//            {
		//                g9.Rows[i].HeightDisplay = MinRowHeight;
		//            }
		//        }

		//        if (!aFixedOnly)
		//        {
		//            g7.AutoSizeRows(g7.Rows.Fixed, 0, g7.Rows.Count - 1, g7.Cols.Count - 1, 0, AutoSizeFlags.None);
		//            g8.AutoSizeRows(g8.Rows.Fixed, 0, g8.Rows.Count - 1, g8.Cols.Count - 1, 0, AutoSizeFlags.None);
		//            g9.AutoSizeRows(g9.Rows.Fixed, 0, g9.Rows.Count - 1, g9.Cols.Count - 1, 0, AutoSizeFlags.None);

		//            MinRowHeight = FindMinimumRowHeight(g7, g8, g9);

		//            for (i = g7.Rows.Fixed; i < g7.Rows.Count; i++)
		//            {
		//                if (((PagingGridTag)g7.Tag).Visible)
		//                {
		//                    g7.Rows[i].HeightDisplay = MinRowHeight;
		//                }
		//                if (((PagingGridTag)g8.Tag).Visible)
		//                {
		//                    g8.Rows[i].HeightDisplay = MinRowHeight;
		//                }
		//                if (((PagingGridTag)g9.Tag).Visible)
		//                {
		//                    g9.Rows[i].HeightDisplay = MinRowHeight;
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		////private int FindMinimumColWidth(ref C1FlexGrid aGrid, int aCol, C1FlexGrid aColHeaderGrid, C1FlexGrid aCol2Grid, C1FlexGrid aCol3Grid, C1FlexGrid aCol4Grid)
		//private int FindMinimumColWidth(ref C1FlexGrid aGrid, int aCol, C1FlexGrid aColHeaderGrid, C1FlexGrid aCol2Grid, C1FlexGrid aCol3Grid)
		//{
		//    int i;
		//    int j;
		//    int maxCellValLength;
		//    string formatString;
		//    ArrayList colHeaderStrs;
		//    ArrayList col2Strs;
		//    ArrayList col3Strs;

		//    try
		//    {
		//        if (aGrid == null)
		//        {
		//            aGrid = new C1FlexGrid();
		//            aGrid.Cols.Count = 1;
		//            aGrid.Rows.Count = aColHeaderGrid.Styles.Count + aCol2Grid.Styles.Count + aCol3Grid.Styles.Count;

		//            i = 0;

		//            g3.Rows[0].Style = g3.Styles["ColumnHeading"];

		//            for (j = 0; j < aColHeaderGrid.Styles.Count; i++, j++)
		//            {
		//                aGrid.Rows[i].Style = aGrid.Styles.Add(aColHeaderGrid.Styles[j].Name, aColHeaderGrid.Styles[j]);
		//            }

		//            for (j = 0; j < aCol2Grid.Styles.Count; i++, j++)
		//            {
		//                aGrid.Rows[i].Style = aGrid.Styles.Add(aCol2Grid.Styles[j].Name, aCol2Grid.Styles[j]);
		//            }

		//            for (j = 0; j < aCol3Grid.Styles.Count; i++, j++)
		//            {
		//                aGrid.Rows[i].Style = aGrid.Styles.Add(aCol3Grid.Styles[j].Name, aCol3Grid.Styles[j]);
		//            }
		//        }

		//        colHeaderStrs = FindMaxCellValueStrings(aColHeaderGrid, aCol);
		//        col2Strs = FindMaxCellValueStrings(aCol2Grid, aCol);
		//        col3Strs = FindMaxCellValueStrings(aCol3Grid, aCol);

		//        formatString = "";
		//        formatString = formatString.PadLeft(MINCOLSIZE, '0');

		//        aGrid.Cols.Count = colHeaderStrs.Count + col2Strs.Count + col3Strs.Count + 1;

		//        for (i = 0; i < aGrid.Rows.Count; i++)
		//        {
		//            aGrid[i, 0] = formatString;
		//            j = 1;

		//            foreach (string str in colHeaderStrs)
		//            {
		//                aGrid[i, j] = str;
		//                j++;
		//            }

		//            foreach (string str in col2Strs)
		//            {
		//                aGrid[i, j] = str;
		//                j++;
		//            }

		//            foreach (string str in col3Strs)
		//            {
		//                aGrid[i, j] = str;
		//                j++;
		//            }
		//        }

		//        aGrid.AutoSizeCols();
		//        maxCellValLength = 0;

		//        for (i = 0; i < colHeaderStrs.Count + col2Strs.Count + col3Strs.Count + 1; i++)
		//        {
		//            maxCellValLength = Math.Max(maxCellValLength, aGrid.Cols[i].WidthDisplay);
		//        }

		//        return maxCellValLength;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private ArrayList FindMaxCellValueStrings(C1FlexGrid aGrid, int aCol)
		//{
		//    ArrayList valueStrs;
		//    char[] delimeters;
		//    int i;
		//    string cellVal;
		//    string[] splitCellVal;

		//    try
		//    {
		//        valueStrs = new ArrayList();
		//        delimeters = new char[1];
		//        delimeters[0] = ' ';

		//        for (i = 0; i < aGrid.Rows.Fixed; i++)
		//        {
		//            cellVal = (string)aGrid[i, aCol];

		//            if (cellVal != null)
		//            {
		//                splitCellVal = cellVal.Split(delimeters);
		//                foreach (string str in splitCellVal)
		//                {
		//                    if (str.Length > 0)
		//                    {
		//                        valueStrs.Add(str);
		//                    }
		//                }
		//            }
		//        }

		//        return valueStrs;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private int FindMinimumRowHeight(C1FlexGrid aRowHeaderGrid, C1FlexGrid aRow2Grid, C1FlexGrid aRow3Grid)
		//{
		//    int i;
		//    int j;
		//    C1FlexGrid grid;

		//    try
		//    {
		//        grid = new C1FlexGrid();
		//        grid.Cols.Count = aRowHeaderGrid.Styles.Count + aRow2Grid.Styles.Count + aRow3Grid.Styles.Count;
		//        grid.Rows.Count = 1;

		//        i = 0;

		//        for (j = 0; j < aRowHeaderGrid.Styles.Count; i++, j++)
		//        {
		//            grid.SetCellStyle(0, i, aRowHeaderGrid.Styles[j]);
		//        }

		//        for (j = 0; j < aRow2Grid.Styles.Count; i++, j++)
		//        {
		//            grid.SetCellStyle(0, i, aRow2Grid.Styles[j]);
		//        }

		//        for (j = 0; j < aRow3Grid.Styles.Count; i++, j++)
		//        {
		//            grid.SetCellStyle(0, i, aRow3Grid.Styles[j]);
		//        }

		//        for (i = 0; i < grid.Cols.Count; i++)
		//        {
		//            grid[0, i] = "X";
		//        }

		//        grid.AutoSizeRows();

		//        return grid.Rows[0].HeightDisplay;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CalcColSplitPosition2(bool aOverrideLock)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)g1.Tag).Visible && g1.Cols.Count > 0)
		//        {
		//            if (!((SplitterTag)spcVLevel1.Tag).Locked || aOverrideLock)
		//            {
		//                _currColSplitPosition2 = g1.Cols[g1.Cols.Count - 1].Right + 2;
		//            }
		//        }
		//        else
		//        {
		//            _currColSplitPosition2 = 0;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CalcColSplitPosition3(bool aOverrideLock)
		//{
		//    int i;

		//    try
		//    {
		//        if (((PagingGridTag)g2.Tag).Visible && g2.Cols.Count > 0)
		//        {
		//            if (!((SplitterTag)spcVLevel2.Tag).Locked || aOverrideLock)
		//            {
		//                i = Math.Min(g2.LeftCol + MAXTOTALCOLUMNS, g2.Cols.Count);
		//                _currColSplitPosition3 = (g2.Cols[i - 1].Right - g2.Cols[g2.LeftCol].Left) + 2;
		//            }
		//        }
		//        else
		//        {
		//            _currColSplitPosition3 = 0;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void SetColSplitPositions()
		//{
		//    int spcHDetailLevel2SplitPos;

		//    try
		//    {
		//        spcHDetailLevel2SplitPos = _currColSplitPosition3;

		//        _hSplitMove = true;

		//        spcVLevel2.SplitterDistance = 0;

		//        _hSplitMove = false;

		//        spcVLevel1.SplitterDistance = _currColSplitPosition2;
		//        spcVLevel2.SplitterDistance = spcHDetailLevel2SplitPos;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CalcRowSplitPosition4(bool aOverrideLock)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)g1.Tag).Visible && g1.Cols.Count > 0)
		//        {
		//            if (!((SplitterTag)spcHScrollLevel2.Tag).Locked || aOverrideLock)
		//            {
		//                _currRowSplitPosition4 = g1.Rows[g1.Rows.Count - 1].Bottom + 2;
		//            }
		//        }
		//        else
		//        {
		//            _currRowSplitPosition4 = 0;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CalcRowSplitPosition12(bool aOverrideLock)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)g7.Tag).Visible && g7.Cols.Count > 0)
		//        {
		//            if (!((SplitterTag)spcHScrollLevel1.Tag).Locked || aOverrideLock)
		//            {
		//                _currRowSplitPosition12 = g7.Rows[g7.Rows.Count - 1].Bottom + pnlSpacer.Height + spcHScrollLevel1.SplitterWidth + 2;
		//            }
		//        }
		//        else
		//        {
		//            _currRowSplitPosition12 = 0;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void SetRowSplitPositions()
		//{
		//    int spcHScrollLevel1SplitPos;
		//    int spcHScrollLevel2SplitPos;

		//    try
		//    {
		//        spcHScrollLevel1SplitPos = _currRowSplitPosition12;
		//        spcHScrollLevel2SplitPos = _currRowSplitPosition4;

		//        spcHScrollLevel2.SplitterDistance = 0;
		//        spcHScrollLevel1.SplitterDistance = 0;

		//        spcHScrollLevel1.SplitterDistance = spcHScrollLevel1.Height - spcHScrollLevel1SplitPos;
		//        spcHScrollLevel2.SplitterDistance = spcHScrollLevel2SplitPos;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void cmiFreezeColumn_Click(object sender, System.EventArgs e)
		//{
		//    int i;
		//    PagingGridTag gridTag;

		//    try
		//    {
		//        gridTag = (PagingGridTag)_rightClickedFrom.Tag;

		//        if (cmiFreezeColumn.Checked == true) //Unfreeze the columns.
		//        {
		//            //We're in this IF block because the column is already frozen. 
		//            //The user must want to un-freeze the columns.

		//            if (gridTag.GridId == Grid2)
		//            {
		//                for (i = 0; i < _rightClickedFrom.LeftCol; i++)
		//                {
		//                    g2.Cols[i].Visible = true;
		//                    if (((PagingGridTag)g5.Tag).Visible)
		//                    {
		//                        g5.Cols[i].Visible = true;
		//                    }
		//                    if (((PagingGridTag)g8.Tag).Visible)
		//                    {
		//                        g8.Cols[i].Visible = true;
		//                    }
		//                }

		//                g2.Cols.Frozen = 0;
		//                g5.Cols.Frozen = 0;
		//                g8.Cols.Frozen = 0;

		//                SetHScrollBar2Parameters();

		//                g2.LeftCol = _leftMostColBeforeFreeze;
		//                g5.LeftCol = _leftMostColBeforeFreeze;
		//                g8.LeftCol = _leftMostColBeforeFreeze;

		//                gridTag.HasColsFrozen = false;
		//            }
		//            else if (gridTag.GridId == Grid3)
		//            {
		//                for (i = 0; i < g3.LeftCol; i++)
		//                {
		//                    g3.Cols[i].Visible = true;
		//                    if (((PagingGridTag)g6.Tag).Visible)
		//                    {
		//                        g6.Cols[i].Visible = true;
		//                    }
		//                    if (((PagingGridTag)g9.Tag).Visible)
		//                    {
		//                        g9.Cols[i].Visible = true;
		//                    }
		//                }

		//                g3.Cols.Frozen = 0;
		//                g6.Cols.Frozen = 0;
		//                g9.Cols.Frozen = 0;

		//                SetHScrollBar3Parameters();

		//                g3.LeftCol = _leftMostColBeforeFreeze;
		//                g6.LeftCol = _leftMostColBeforeFreeze;
		//                g9.LeftCol = _leftMostColBeforeFreeze;

		//                gridTag.HasColsFrozen = false;
		//            }
		//        }
		//        else //Freez the columns.
		//        {
		//            //We're in this IF block because the column is not frozen.
		//            //The user must want to freeze this column.
		//            //We must freeze this column and all columns to the left of it.
		//            if (gridTag.GridId == Grid2)
		//            {
		//                _leftMostColBeforeFreeze = g2.LeftCol; //this var will be used later when un-freezing.

		//                for (i = 0; i < g2.LeftCol; i++)
		//                {
		//                    g2.Cols[i].Visible = false;
		//                    if (((PagingGridTag)g5.Tag).Visible)
		//                    {
		//                        g5.Cols[i].Visible = false;
		//                    }
		//                    if (((PagingGridTag)g8.Tag).Visible)
		//                    {
		//                        g8.Cols[i].Visible = false;
		//                    }
		//                }

		//                g2.Cols.Frozen = gridTag.MouseDownCol;
		//                g5.Cols.Frozen = gridTag.MouseDownCol;
		//                g8.Cols.Frozen = gridTag.MouseDownCol;

		//                g2.LeftCol = gridTag.MouseDownCol;
		//                g5.LeftCol = gridTag.MouseDownCol;
		//                g8.LeftCol = gridTag.MouseDownCol;

		//                gridTag.HasColsFrozen = true;

		//                SetHScrollBar2Parameters();
		//            }
		//            else if (gridTag.GridId == Grid3)
		//            {
		//                _leftMostColBeforeFreeze = g3.LeftCol; //this var will be used later when un-freezing.

		//                for (i = 0; i < g3.LeftCol; i++)
		//                {
		//                    g3.Cols[i].Visible = false;
		//                    if (((PagingGridTag)g6.Tag).Visible)
		//                    {
		//                        g6.Cols[i].Visible = false;
		//                    }
		//                    if (((PagingGridTag)g9.Tag).Visible)
		//                    {
		//                        g9.Cols[i].Visible = false;
		//                    }
		//                }

		//                g3.Cols.Frozen = gridTag.MouseDownCol;
		//                g6.Cols.Frozen = gridTag.MouseDownCol;
		//                g9.Cols.Frozen = gridTag.MouseDownCol;

		//                g3.LeftCol = gridTag.MouseDownCol;
		//                g6.LeftCol = gridTag.MouseDownCol;
		//                g9.LeftCol = gridTag.MouseDownCol;

		//                gridTag.HasColsFrozen = true;

		//                SetHScrollBar3Parameters();
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		#endregion

		#region Style-changing codes (colors, fonts, etc)

		//private void DefineStyles(bool aDefineStylesToGrid)
		//{
		//    try
		//    {
		//        Setg1Styles(aDefineStylesToGrid);
		//        Setg2Styles(aDefineStylesToGrid);
		//        Setg3Styles(aDefineStylesToGrid);
		//        Setg4Styles(aDefineStylesToGrid);
		//        Setg5Styles(aDefineStylesToGrid);
		//        Setg6Styles(aDefineStylesToGrid);
		//        Setg7Styles(aDefineStylesToGrid);
		//        Setg8Styles(aDefineStylesToGrid);
		//        Setg9Styles(aDefineStylesToGrid);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Setg1Styles(bool aDefineStylesToGrid)
		//{
		//    CellStyle cellStyle;
		//    int i;

		//    try
		//    {
		//        if (((PagingGridTag)g1.Tag).Visible)
		//        {
		//            if (aDefineStylesToGrid)
		//            {
		//                cellStyle = g1.Styles.Add("ColumnHeading");
		//                cellStyle.BackColor = _theme.ColumnHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
		//                cellStyle.TextEffect = _theme.ColumnHeaderTextEffects;
		//                cellStyle.WordWrap = true;

		//                cellStyle = g1.Styles.Add("RowHeading");
		//                cellStyle.BackColor = _theme.StoreSetRowHeaderBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
		//                cellStyle.Font = _theme.RowHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
		//                cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
		//            }

		//            for (i = 0; i < g1.Rows.Fixed; i++)
		//            {
		//                g1.Rows[i].StyleFixed = g1.Styles["ColumnHeading"];
		//                g1.Rows[i].Style = g1.Styles["ColumnHeading"];
		//            }

		//            for (i = g1.Rows.Fixed; i < g1.Rows.Count; i++)
		//            {
		//                g1.Rows[i].StyleFixed = g1.Styles["RowHeading"];
		//                g1.Rows[i].Style = g1.Styles["RowHeading"];
		//            }

		//            ClearGridFlags(g1);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Setg2Styles(bool aDefineStylesToGrid)
		//{
		//    CellStyle cellStyle;
		//    int i;

		//    try
		//    {
		//        if (((PagingGridTag)g2.Tag).Visible)
		//        {
		//            if (aDefineStylesToGrid)
		//            {
		//                cellStyle = g2.Styles.Add("ColumnHeading");
		//                cellStyle.BackColor = _theme.ColumnHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
		//                cellStyle.TextEffect = _theme.ColumnHeaderTextEffects;
		//                cellStyle.WordWrap = true;

		//                cellStyle = g2.Styles.Add("Blocked1");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g2.Styles.Add("Editable1");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g2.Styles.Add("Locked1");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g2.Styles.Add("Negative1");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g2.Styles.Add("NegativeEditable1");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g2.Styles.Add("Style1");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(g2.Styles["Blocked1"], g2.Styles["Locked1"], g2.Styles["Negative1"], g2.Styles["Editable1"], g2.Styles["NegativeEditable1"]);

		//                cellStyle = g2.Styles.Add("Blocked2");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g2.Styles.Add("Editable2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g2.Styles.Add("Locked2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g2.Styles.Add("Negative2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g2.Styles.Add("NegativeEditable2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g2.Styles.Add("Style2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(g2.Styles["Blocked2"], g2.Styles["Locked2"], g2.Styles["Negative2"], g2.Styles["Editable2"], g2.Styles["NegativeEditable2"]);
		//            }

		//            g2.Rows[0].Style = g2.Styles["ColumnHeading"];

		//            for (i = 1; i < g2.Rows.Count; i++)
		//            {
		//                g2.Rows[i].Style = g2.Styles["Style1"];
		//            }

		//            ClearGridFlags(g2);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Setg3Styles(bool aDefineStylesToGrid)
		//{
		//    CellStyle cellStyle;
		//    int i;

		//    try
		//    {
		//        if (((PagingGridTag)g3.Tag).Visible)
		//        {
		//            if (aDefineStylesToGrid)
		//            {
		//                cellStyle = g3.Styles.Add("ColumnHeading");
		//                cellStyle.BackColor = _theme.ColumnHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
		//                cellStyle.TextEffect = _theme.ColumnHeaderTextEffects;
		//                cellStyle.WordWrap = true;

		//                cellStyle = g3.Styles.Add("Blocked1");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g3.Styles.Add("Editable1");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g3.Styles.Add("Locked1");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g3.Styles.Add("Negative1");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g3.Styles.Add("NegativeEditable1");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g3.Styles.Add("Style1");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(g3.Styles["Blocked1"], g3.Styles["Locked1"], g3.Styles["Negative1"], g3.Styles["Editable1"], g3.Styles["NegativeEditable1"]);

		//                cellStyle = g3.Styles.Add("Blocked2");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g3.Styles.Add("Editable2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g3.Styles.Add("Locked2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g3.Styles.Add("Negative2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g3.Styles.Add("NegativeEditable2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g3.Styles.Add("Style2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(g3.Styles["Blocked2"], g3.Styles["Locked2"], g3.Styles["Negative2"], g3.Styles["Editable2"], g3.Styles["NegativeEditable2"]);
		//            }

		//            g3.Rows[0].Style = g3.Styles["ColumnHeading"];

		//            for (i = 1; i < g3.Rows.Count; i++)
		//            {
		//                g3.Rows[i].Style = g3.Styles["Style1"];
		//            }

		//            ClearGridFlags(g3);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Setg4Styles(bool aDefineStylesToGrid)
		//{
		//    CellStyle cellStyle;
		//    int i;

		//    try
		//    {
		//        if (((PagingGridTag)g4.Tag).Visible)
		//        {
		//            if (aDefineStylesToGrid)
		//            {
		//                cellStyle = g4.Styles.Add("ColumnTree");
		//                cellStyle.BackColor = System.Drawing.SystemColors.Window;
		//                cellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
		//                cellStyle.Font = System.Drawing.SystemFonts.DefaultFont;
		//                cellStyle.Border.Style = BorderStyleEnum.None;
		//                cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
		//                cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;

		//                cellStyle = g4.Styles.Add("ColumnHeading");
		//                cellStyle.BackColor = _theme.ColumnHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
		//                cellStyle.TextEffect = _theme.ColumnHeaderTextEffects;
		//                cellStyle.WordWrap = true;

		//                cellStyle = g4.Styles.Add("RowHeading");
		//                cellStyle.BackColor = _theme.StoreDetailRowHeaderBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
		//                cellStyle.Font = _theme.RowHeaderFont;
		//                cellStyle.Border.Style = BorderStyleEnum.None;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
		//                cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftTop;
		//            }

		//            for (i = 0; i < g4.Rows.Fixed; i++)
		//            {
		//                g4.Rows[i].StyleFixed = g4.Styles["ColumnHeading"];
		//                g4.Rows[i].Style = g4.Styles["ColumnHeading"];
		//            }

		//            for (i = g4.Rows.Fixed; i < g4.Rows.Count; i++)
		//            {
		//                g4.Rows[i].StyleFixed = g4.Styles["ColumnTree"];
		//                g4.Rows[i].Style = g4.Styles["RowHeading"];
		//            }

		//            ClearGridFlags(g4);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Setg5Styles(bool aDefineStylesToGrid)
		//{
		//    CellStyle cellStyle;
		//    int i;
		//    CubeWaferCoordinateList coorList;
		//    CubeWaferCoordinate coor;

		//    try
		//    {
		//        if (((PagingGridTag)g5.Tag).Visible)
		//        {
		//            if (aDefineStylesToGrid)
		//            {
		//                cellStyle = g5.Styles.Add("ColumnHeading");
		//                cellStyle.BackColor = _theme.ColumnHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
		//                cellStyle.TextEffect = _theme.ColumnHeaderTextEffects;
		//                cellStyle.WordWrap = true;

		//                cellStyle = g5.Styles.Add("HeaderDetailBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("HeaderDetailEditable");
		//                cellStyle.BackColor = _theme.StoreDetailBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("HeaderDetailLocked");
		//                cellStyle.BackColor = _theme.StoreDetailBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("HeaderDetailNegative");
		//                cellStyle.BackColor = _theme.StoreDetailBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("HeaderDetailNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreDetailBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("HeaderDetail");
		//                cellStyle.BackColor = _theme.StoreDetailBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g5.Styles["HeaderDetailBlocked"],
		//                    g5.Styles["HeaderDetailLocked"],
		//                    g5.Styles["HeaderDetailNegative"],
		//                    g5.Styles["HeaderDetailEditable"],
		//                    g5.Styles["HeaderDetailNegativeEditable"]);

		//                cellStyle = g5.Styles.Add("HeaderSubTotalBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("HeaderSubTotalEditable2");
		//                cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("HeaderSubTotalLocked");
		//                cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("HeaderSubTotalNegative");
		//                cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("HeaderSubTotalNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("HeaderSubTotal");
		//                cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g5.Styles["HeaderSubTotalBlocked"],
		//                    g5.Styles["HeaderSubTotalLocked"],
		//                    g5.Styles["HeaderSubTotalNegative"],
		//                    g5.Styles["HeaderSubTotalEditable2"],
		//                    g5.Styles["HeaderSubTotalNegativeEditable"]);

		//                cellStyle = g5.Styles.Add("PlaceholderDetailBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("PlaceholderDetailEditable");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("PlaceholderDetailLocked");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("PlaceholderDetailNegative");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("PlaceholderDetailNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("PlaceholderDetail");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g5.Styles["PlaceholderDetailBlocked"],
		//                    g5.Styles["PlaceholderDetailLocked"],
		//                    g5.Styles["PlaceholderDetailNegative"],
		//                    g5.Styles["PlaceholderDetailEditable"],
		//                    g5.Styles["PlaceholderDetailNegativeEditable"]);

		//                cellStyle = g5.Styles.Add("PlaceholderSubTotalBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("PlaceholderSubTotalEditable2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("PlaceholderSubTotalLocked");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("PlaceholderSubTotalNegative");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("PlaceholderSubTotalNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("PlaceholderSubTotal");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g5.Styles["PlaceholderSubTotalBlocked"],
		//                    g5.Styles["PlaceholderSubTotalLocked"],
		//                    g5.Styles["PlaceholderSubTotalNegative"],
		//                    g5.Styles["PlaceholderSubTotalEditable2"],
		//                    g5.Styles["PlaceholderSubTotalNegativeEditable"]);

		//                cellStyle = g5.Styles.Add("DifferenceDetailBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("DifferenceDetailEditable");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("DifferenceDetailLocked");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("DifferenceDetailNegative");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("DifferenceDetailNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("DifferenceDetail");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g5.Styles["DifferenceDetailBlocked"],
		//                    g5.Styles["DifferenceDetailLocked"],
		//                    g5.Styles["DifferenceDetailNegative"],
		//                    g5.Styles["DifferenceDetailEditable"],
		//                    g5.Styles["DifferenceDetailNegativeEditable"]);

		//                cellStyle = g5.Styles.Add("DifferenceSubTotalBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("DifferenceSubTotalEditable2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("DifferenceSubTotalLocked");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("DifferenceSubTotalNegative");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("DifferenceSubTotalNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g5.Styles.Add("DifferenceSubTotal");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g5.Styles["DifferenceSubTotalBlocked"],
		//                    g5.Styles["DifferenceSubTotalLocked"],
		//                    g5.Styles["DifferenceSubTotalNegative"],
		//                    g5.Styles["DifferenceSubTotalEditable2"],
		//                    g5.Styles["DifferenceSubTotalNegativeEditable"]);
		//            }

		//            g5.Rows[0].Style = g5.Styles["ColumnHeading"];

		//            for (i = 1; i < g5.Rows.Count; i++)
		//            {
		//                coorList = (CubeWaferCoordinateList)((RowHeaderTag)g4.Rows[i].UserData).CubeWaferCoorList;

		//                if (coorList.FindCoordinateType(eProfileType.Placeholder) != null)
		//                {
		//                    coor = (CubeWaferCoordinate)coorList.FindCoordinateType(eProfileType.AssortmentQuantityVariable);

		//                    if (coor.Key == _quantityVariables.ValueVariableProfile.Key)
		//                    {
		//                        if (coorList.FindCoordinateType(eProfileType.AssortmentSubTotal) != null)
		//                        {
		//                            g5.Rows[i].Style = g5.Styles["PlaceholderSubTotal"];
		//                        }
		//                        else
		//                        {
		//                            g5.Rows[i].Style = g5.Styles["PlaceholderDetail"];
		//                        }
		//                    }
		//                    else
		//                    {
		//                        if (coorList.FindCoordinateType(eProfileType.AssortmentSubTotal) != null)
		//                        {
		//                            g5.Rows[i].Style = g5.Styles["DifferenceSubTotal"];
		//                        }
		//                        else
		//                        {
		//                            g5.Rows[i].Style = g5.Styles["DifferenceDetail"];
		//                        }
		//                    }
		//                }
		//                else
		//                {
		//                    if (coorList.FindCoordinateType(eProfileType.AssortmentSubTotal) != null)
		//                    {
		//                        g5.Rows[i].Style = g5.Styles["HeaderSubTotal"];
		//                    }
		//                    else
		//                    {
		//                        g5.Rows[i].Style = g5.Styles["HeaderDetail"];
		//                    }
		//                }
		//            }

		//            ClearGridFlags(g5);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Setg6Styles(bool aDefineStylesToGrid)
		//{
		//    CellStyle cellStyle;
		//    int i;
		//    CubeWaferCoordinateList coorList;
		//    CubeWaferCoordinate coor;

		//    try
		//    {
		//        if (((PagingGridTag)g6.Tag).Visible)
		//        {
		//            if (aDefineStylesToGrid)
		//            {
		//                cellStyle = g6.Styles.Add("ColumnHeading");
		//                cellStyle.BackColor = _theme.ColumnHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
		//                cellStyle.TextEffect = _theme.ColumnHeaderTextEffects;
		//                cellStyle.WordWrap = true;

		//                cellStyle = g6.Styles.Add("HeaderDetailBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("HeaderDetailEditable");
		//                cellStyle.BackColor = _theme.StoreDetailBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("HeaderDetailLocked");
		//                cellStyle.BackColor = _theme.StoreDetailBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("HeaderDetailNegative");
		//                cellStyle.BackColor = _theme.StoreDetailBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("HeaderDetailNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreDetailBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("HeaderDetail");
		//                cellStyle.BackColor = _theme.StoreDetailBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g6.Styles["HeaderDetailBlocked"],
		//                    g6.Styles["HeaderDetailLocked"],
		//                    g6.Styles["HeaderDetailNegative"],
		//                    g6.Styles["HeaderDetailEditable"],
		//                    g6.Styles["HeaderDetailNegativeEditable"]);

		//                cellStyle = g6.Styles.Add("HeaderSubTotalBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("HeaderSubTotalEditable2");
		//                cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("HeaderSubTotalLocked");
		//                cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("HeaderSubTotalNegative");
		//                cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("HeaderSubTotalNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("HeaderSubTotal");
		//                cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreDetailForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g6.Styles["HeaderSubTotalBlocked"],
		//                    g6.Styles["HeaderSubTotalLocked"],
		//                    g6.Styles["HeaderSubTotalNegative"],
		//                    g6.Styles["HeaderSubTotalEditable2"],
		//                    g6.Styles["HeaderSubTotalNegativeEditable"]);

		//                cellStyle = g6.Styles.Add("PlaceholderDetailBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("PlaceholderDetailEditable");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("PlaceholderDetailLocked");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("PlaceholderDetailNegative");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("PlaceholderDetailNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("PlaceholderDetail");
		//                cellStyle.BackColor = _theme.StoreSetBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g6.Styles["PlaceholderDetailBlocked"],
		//                    g6.Styles["PlaceholderDetailLocked"],
		//                    g6.Styles["PlaceholderDetailNegative"],
		//                    g6.Styles["PlaceholderDetailEditable"],
		//                    g6.Styles["PlaceholderDetailNegativeEditable"]);

		//                cellStyle = g6.Styles.Add("PlaceholderSubTotalBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("PlaceholderSubTotalEditable2");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("PlaceholderSubTotalLocked");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("PlaceholderSubTotalNegative");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("PlaceholderSubTotalNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("PlaceholderSubTotal");
		//                cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreSetForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g6.Styles["PlaceholderSubTotalBlocked"],
		//                    g6.Styles["PlaceholderSubTotalLocked"],
		//                    g6.Styles["PlaceholderSubTotalNegative"],
		//                    g6.Styles["PlaceholderSubTotalEditable2"],
		//                    g6.Styles["PlaceholderSubTotalNegativeEditable"]);

		//                cellStyle = g6.Styles.Add("DifferenceDetailBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("DifferenceDetailEditable");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("DifferenceDetailLocked");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("DifferenceDetailNegative");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("DifferenceDetailNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("DifferenceDetail");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g6.Styles["DifferenceDetailBlocked"],
		//                    g6.Styles["DifferenceDetailLocked"],
		//                    g6.Styles["DifferenceDetailNegative"],
		//                    g6.Styles["DifferenceDetailEditable"],
		//                    g6.Styles["DifferenceDetailNegativeEditable"]);

		//                cellStyle = g6.Styles.Add("DifferenceSubTotalBlocked");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("DifferenceSubTotalEditable2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("DifferenceSubTotalLocked");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("DifferenceSubTotalNegative");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("DifferenceSubTotalNegativeEditable");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g6.Styles.Add("DifferenceSubTotal");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(
		//                    g6.Styles["DifferenceSubTotalBlocked"],
		//                    g6.Styles["DifferenceSubTotalLocked"],
		//                    g6.Styles["DifferenceSubTotalNegative"],
		//                    g6.Styles["DifferenceSubTotalEditable2"],
		//                    g6.Styles["DifferenceSubTotalNegativeEditable"]);
		//            }

		//            g6.Rows[0].Style = g6.Styles["ColumnHeading"];

		//            for (i = 1; i < g6.Rows.Count; i++)
		//            {
		//                coorList = (CubeWaferCoordinateList)((RowHeaderTag)g4.Rows[i].UserData).CubeWaferCoorList;

		//                if (coorList.FindCoordinateType(eProfileType.Placeholder) != null)
		//                {
		//                    coor = (CubeWaferCoordinate)coorList.FindCoordinateType(eProfileType.AssortmentQuantityVariable);

		//                    if (coor.Key == _quantityVariables.ValueVariableProfile.Key)
		//                    {
		//                        if (coorList.FindCoordinateType(eProfileType.AssortmentSubTotal) != null)
		//                        {
		//                            g6.Rows[i].Style = g6.Styles["PlaceholderSubTotal"];
		//                        }
		//                        else
		//                        {
		//                            g6.Rows[i].Style = g6.Styles["PlaceholderDetail"];
		//                        }
		//                    }
		//                    else
		//                    {
		//                        if (coorList.FindCoordinateType(eProfileType.AssortmentSubTotal) != null)
		//                        {
		//                            g6.Rows[i].Style = g6.Styles["DifferenceSubTotal"];
		//                        }
		//                        else
		//                        {
		//                            g6.Rows[i].Style = g6.Styles["DifferenceDetail"];
		//                        }
		//                    }
		//                }
		//                else
		//                {
		//                    if (coorList.FindCoordinateType(eProfileType.AssortmentSubTotal) != null)
		//                    {
		//                        g6.Rows[i].Style = g6.Styles["HeaderSubTotal"];
		//                    }
		//                    else
		//                    {
		//                        g6.Rows[i].Style = g6.Styles["HeaderDetail"];
		//                    }
		//                }
		//            }

		//            ClearGridFlags(g6);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Setg7Styles(bool aDefineStylesToGrid)
		//{
		//    CellStyle cellStyle;
		//    int i;

		//    try
		//    {
		//        if (((PagingGridTag)g7.Tag).Visible)
		//        {
		//            if (aDefineStylesToGrid)
		//            {
		//                cellStyle = g7.Styles.Add("RowHeading");
		//                cellStyle.BackColor = _theme.StoreTotalRowHeaderBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalRowHeaderForeColor;
		//                cellStyle.Font = _theme.RowHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
		//                cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
		//            }

		//            for (i = g7.Rows.Fixed; i < g7.Rows.Count; i++)
		//            {
		//                g7.Rows[i].StyleFixed = g7.Styles["RowHeading"];
		//                g7.Rows[i].Style = g7.Styles["RowHeading"];
		//            }

		//            ClearGridFlags(g7);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Setg8Styles(bool aDefineStylesToGrid)
		//{
		//    CellStyle cellStyle;
		//    int i;

		//    try
		//    {
		//        if (((PagingGridTag)g8.Tag).Visible)
		//        {
		//            if (aDefineStylesToGrid)
		//            {
		//                cellStyle = g8.Styles.Add("Blocked1");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g8.Styles.Add("Editable1");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g8.Styles.Add("Locked1");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g8.Styles.Add("Negative1");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g8.Styles.Add("NegativeEditable1");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g8.Styles.Add("Style1");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(g8.Styles["Blocked1"], g8.Styles["Locked1"], g8.Styles["Negative1"], g8.Styles["Editable1"], g8.Styles["NegativeEditable1"]);

		//                cellStyle = g8.Styles.Add("Blocked2");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g8.Styles.Add("Editable2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g8.Styles.Add("Locked2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g8.Styles.Add("Negative2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g8.Styles.Add("NegativeEditable2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g8.Styles.Add("Style2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(g8.Styles["Blocked2"], g8.Styles["Locked2"], g8.Styles["Negative2"], g8.Styles["Editable2"], g8.Styles["NegativeEditable2"]);
		//            }

		//            for (i = 0; i < g8.Rows.Count; i++)
		//            {
		//                g8.Rows[i].Style = g8.Styles["Style1"];
		//            }

		//            ClearGridFlags(g8);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void Setg9Styles(bool aDefineStylesToGrid)
		//{
		//    CellStyle cellStyle;
		//    int i;

		//    try
		//    {
		//        if (((PagingGridTag)g9.Tag).Visible)
		//        {
		//            if (aDefineStylesToGrid)
		//            {
		//                cellStyle = g9.Styles.Add("Blocked1");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g9.Styles.Add("Editable1");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g9.Styles.Add("Locked1");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g9.Styles.Add("Negative1");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g9.Styles.Add("NegativeEditable1");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g9.Styles.Add("Style1");
		//                cellStyle.BackColor = _theme.StoreTotalBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(g9.Styles["Blocked1"], g9.Styles["Locked1"], g9.Styles["Negative1"], g9.Styles["Editable1"], g9.Styles["NegativeEditable1"]);

		//                cellStyle = g9.Styles.Add("Blocked2");
		//                cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
		//                cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
		//                cellStyle.Font = _theme.ColumnGroupHeaderFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g9.Styles.Add("Editable2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g9.Styles.Add("Locked2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.LockedFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g9.Styles.Add("Negative2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g9.Styles.Add("NegativeEditable2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.NegativeForeColor;
		//                cellStyle.Font = _theme.EditableFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;

		//                cellStyle = g9.Styles.Add("Style2");
		//                cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
		//                cellStyle.ForeColor = _theme.StoreTotalForeColor;
		//                cellStyle.Font = _theme.DisplayOnlyFont;
		//                cellStyle.Border.Style = _theme.CellBorderStyle;
		//                cellStyle.Border.Color = _theme.CellBorderColor;
		//                cellStyle.UserData = new CellStyleUserData(g9.Styles["Blocked2"], g9.Styles["Locked2"], g9.Styles["Negative2"], g9.Styles["Editable2"], g9.Styles["NegativeEditable2"]);
		//            }

		//            for (i = 0; i < g9.Rows.Count; i++)
		//            {
		//                g9.Rows[i].Style = g9.Styles["Style1"];
		//            }

		//            ClearGridFlags(g9);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		private void ClearGridFlags(C1FlexGrid aGrid)
		{
			PagingGridTag gridTag;
			int i;
			int j;

			try
			{
				gridTag = (PagingGridTag)aGrid.Tag;

				for (i = 0; i < _gridData[gridTag.GridId].GetLength(0); i++)
				{
					for (j = 0; j < _gridData[gridTag.GridId].GetLength(1); j++)
					{
						_gridData[gridTag.GridId][i, j].CellFlagsInited = false;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void Theme()
		{
			try
			{
				_frmThemeProperties = new ThemeProperties(_theme);
				_frmThemeProperties.ApplyButtonClicked += new EventHandler(StylePropertiesOnChanged);
				_frmThemeProperties.StartPosition = FormStartPosition.CenterParent;

				if (_frmThemeProperties.ShowDialog() == DialogResult.OK)
				{
					//StylePropertiesChanged();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//private void btnApply_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;
		//        StopPageLoadThreads();
		//        RecomputePlanCubes();
		//        SaveDetailCubeGroup();
		//        LoadCurrentPages();
		//        LoadSurroundingPages();
		//    }
		//    //BEGIN TT#654-MD - stodd - type in % to total in the total section, hit enter twice receive a Nothing to Spread Exception   
		//    catch (NothingToSpreadException ex)
		//    {
		//        string message = ex.Message;
		//        MessageBox.Show(message, _windowName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		//        //string message = MIDText.GetText(eMIDTextCode.msg_ChainWeekLowLvlSpreadFailed);
		//        SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, ex.ToString(), this.ToString());
		//    }
		//    //END TT#654-MD - stodd - type in % to total in the total section, hit enter twice receive a Nothing to Spread Exception   
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        UpdateOtherViews();		// TT#2 - stodd - assortment
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		private void btnProcess_Click(object sender, EventArgs e)
		{
            //ProcessAction(((ActionTag)lblAction.Tag).Key);
        }
        private void ProcessAction(int action)
        {
			string message = string.Empty;
			//int action = 0;
			// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
			//ApplicationSessionTransaction actionTransaction = null;
			// END TT#217-MD - stodd - unable to run workflow methods against assortment
			GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
			_refreshAssortmentWorkspace = false;	// TT#1261 - stodd

			try
			{
				//action = ((ActionTag)lblAction.Tag).Key;

				if (action == Include.NoRID)
				{
					MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionIsRequired), this.Text);
					return;
				}

				Cursor.Current = Cursors.WaitCursor;
				ApplicationBaseAction aMethod = _transaction.CreateNewMethodAction((eMethodType)action);
				aComponent = new GeneralComponent(eGeneralComponentType.Total);

				bool aReviewFlag = false;
				bool aUseSystemTolerancePercent = true;
				double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
				int aStoreFilter = Include.AllStoreFilterRID;
				int aWorkFlowStepKey = -1;
				int selectedHdrCount = 0;	// TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
				_transaction.ActionOrigin = eActionOrigin.GroupAllocation;	// TT#488-MD - Stodd - Group Allocation
				//if (Enum.IsDefined(typeof(eAssortmentActionType), (eAssortmentActionType)action))
				//{
				//    //==============================
				//    // ASSORTMENT ACTION PROCESSING
				//    //==============================
				//    //BEGIN TT#9-MD - DOConnell - Cannot drag and drop header into assortment
				//    //BEGIN TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
				//    selectedHdrCount = ProcessAssortmentMethod(ref _transaction, action, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
				//    //ProcessAssortmentMethod(ref actionTransaction, action, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
				//    //END TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
				//}
				//else
				{
					//==============================
					// ALLOCATION ACTION PROCESSING
					//==============================
					//BEGIN TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
					selectedHdrCount = ProcessAllocationMethod(ref _transaction, action, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
					//ProcessAllocationMethod(ref actionTransaction, action, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
					//END TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
					//END TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
				}

				// Begin TT#1225 - stodd
				// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
				//BEGIN TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
				string actionText = string.Empty; 	// TT#895-MD - Stodd - Group Allocation
				if (selectedHdrCount == 0)
				{
					eAllocationActionStatus actionStatus = eAllocationActionStatus.NoActionPerformed;
					message = MIDText.GetTextOnly((int)actionStatus);
					// BEGIN TT#895-MD - Stodd - Group Allocation
                    actionText = ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["actionComboBox"]).Text;
					MessageBox.Show(message, actionText, MessageBoxButtons.OK, MessageBoxIcon.Information);
					// END TT#895-MD - Stodd - Group Allocation
				}
				else if (_transaction != null)
				//END TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
				{
					eAllocationActionStatus actionStatus = _transaction.AllocationActionAllHeaderStatus;
					message = MIDText.GetTextOnly((int)actionStatus);
					// BEGIN TT#895-MD - Stodd - Group Allocation
                    actionText = ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["actionComboBox"]).Text;
					MessageBox.Show(message, actionText, MessageBoxButtons.OK, MessageBoxIcon.Information);
					// END TT#895-MD - Stodd - Group Allocation
				}
				// END TT#217-MD - stodd - unable to run workflow methods against assortment
				// End TT#1225 - stodd
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}
			finally
			{
				// Begin TT#1228 - stodd 
				//BEGIN TT#9-MD - DOConnell - Cannot drag and drop header into assortment
				//if (actionTransaction != null)
				//{
				//eAllocationActionStatus actionStatus = actionTransaction.AllocationActionAllHeaderStatus;
				eAllocationActionStatus actionStatus = _transaction.AllocationActionAllHeaderStatus;
				//actionTransaction.Dispose();
				//================================================
				// Updates the Allocation Workspace
				//   with updated info
				//================================================
				if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
				{
					// No reason to do the UpdateData for CreatePlaceholders
					if ((eAssortmentActionType)action != eAssortmentActionType.CreatePlaceholders && (eAssortmentActionType)action != eAssortmentActionType.BalanceAssortment)
					{
						// Begin TT#1465 - RMatelic - Methods/workflows need to update Assortment Review
						//UpdateData(true);
						// End TT#1465
						// Begin TT#2 - RMatelic - Assortment Planning-allocate headers attached to placeholders

						// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
						//List<int> allocHdrKeyList = GetSelectableHeaderList(action);
						// Begin TT#1087 - MD - stodd - size review showing extra headers - 
						SelectedHeaderList allocHdrList = GetSelectableHeaderList(action, _transaction, false);		// TT#889 - MD - stodd - need not working
						// END TT#371-MD - stodd -  Velocity Interactive on Assortment
                        //int[] hdrList = new int[allocHdrKeyList.Count];
                        //int i = 0;
                        //// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                        //foreach (SelectedHeaderProfile shp in allocHdrKeyList)
                        //{
                        //    hdrList[i] = shp.Key;
                        //    i++;
                        //}
                        // Begin TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces
                        //CalculateTotalsForWkSpc(); //TT#658 - MD - DOConnell - Assortment Workspace quantity is not updaed on a Post-Receipt Assortment.
                        // End TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces
						// END TT#371-MD - stodd -  Velocity Interactive on Assortment
						// Begin TT#1465 - RMatelic - Methods/workflows need to update Assortment Review
						//ReloadUpdatedHeaders(hdrList);
						// End TT#2
                        CheckHeaderListForUpdate(allocHdrList);
						// End TT#1087 - MD - stodd - size review showing extra headers - 
                        // begin TT#918 - MD - Jellis - Group Allocation Errors prevent action
                        //      NOTE:  when group allocation actions occur, only the assortment is in the Allocation Header lists; so workspace is not being updated; ie.  for group allocations always do this refresh...
                        //// BEGIN TT#529-MD - stodd -  Status not updated on workspace After running Balance Proportional 
                        //_refreshAllocationWorkspace = CheckForAllocationHeaders(hdrList);
                        //// END TT#529-MD - stodd -  Status not updated on workspace After running Balance Proportional 
                        //// END TT#529-MD - stodd -  Status not updated on workspace After running Balance Proportional

						// End TT#1465
						ChangePending = false;	//TT#211-MD - stodd - size information not refreshing after size need is run on placeholder
					}
					// Begin TT#1261 - stodd - refreshing assortment workspace
                    // Begin TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces
                    //if (_refreshAssortmentWorkspace)
                    //{
                    //    //_eab.AssortmentWorkspaceExplorer.IRefresh();
                    //    UpdateAssortmentWorkspace();
                    //}
                    // End TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces

                    // begin TT#918 - MD - Jellis - Group Allocation Errors prevent action
                    //      NOTE:  when group allocation actions occur, only the assortment is in the Allocation Header lists; so workspace is not being updated; ie.  for group allocations always do this refresh...
                    //// BEGIN TT#529-MD - stodd -  Status not updated on workspace After running Balance Proportional
                    //if (_refreshAllocationWorkspace)
                    //{
                    //    _eab.AllocationWorkspaceExplorer.IRefresh();
                    //}
                    //_eab.AllocationWorkspaceExplorer.IRefresh();  // TT#918 - MD - Jellis - Group Allocation Errors prevent action (only need to releoad headers affected)
					// Begin TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces
                    ReloadUpdatedHeadersInAllocationWorkspace(); // TT#918 - MD - Jellis - Group Allocation Errors prevent action (only need to reload headers affected)
					// End TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces
                    //// END TT#529-MD - stodd -  Status not updated on workspace After running Balance Proportional
                    // end TT#918 - MD - Jellis - Group Allocation Errors prevent action

					// End TT#1261 - stodd - refreshing assortment workspace
					//SetGridRedraws(true);
				}
				//}
				// End TT#1228 - stodd 
				//END TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
				//BEGIN TT#579-MD - stodd -  After adding a header to a placeholder, placeholder has a balance value even though its units are all in balance  
				CalculatePlaceholderBalances();
				//END TT#579-MD - stodd -  After adding a header to a placeholder, placeholder has a balance value even though its units are all in balance  
				Cursor.Current = Cursors.Default;
			}
		}
		#region Processing Methods

		//BEGIN TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
		private int ProcessAllocationMethod(ref ApplicationSessionTransaction actionTransaction, int action, GeneralComponent aComponent, ApplicationBaseAction aMethod, bool aReviewFlag, bool aUseSystemTolerancePercent, double aTolerancePercent, int aStoreFilter, int aWorkFlowStepKey)
		{
			SelectedHeaderList selectedHdrList = GetSelectableHeaderList(action, actionTransaction, true);	// TT#889 - MD - stodd - need not working
		//END TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
			// BEGIN TT#498-MD - stodd -  handle Alloc prof exceptions properly
			try
			{
				// END TT#498-MD - stodd -  handle Alloc prof exceptions properly
				//=============================================================================
				// Get a header key list of just the allocation headers in the Header List
				//=============================================================================
				// Begin TT#1228 - stodd - header select

				// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
				//List<int> allocHdrKeyList = GetSelectableHeaderList(action);
				//SelectedHeaderList selectedHdrList = GetSelectableHeaderList(action);	// TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
				// END TT#371-MD - stodd -  Velocity Interactive on Assortment

				// BEGIN TT#1936 - stodd - cancel allocation/assortment
				if (selectedHdrList.Count == 0)
				{
					string errorMessage = MIDText.GetText(eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected);
					SAB.ClientServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Warning,
						eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected,
						errorMessage,
						"Assortment View");
					// BEGIN TT#529-MD - stodd -  Status not updated on workspace After running Balance Proportional 
					return selectedHdrList.Count;	// TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
					// END TT#529-MD - stodd -  Status not updated on workspace After running Balance Proportional 
				}
				// END TT#1936 - stodd - cancel allocation/assortment

				if (!OKToProcessAllocation((eAllocationActionType)action, selectedHdrList))
				{
					return selectedHdrList.Count;	// TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
				}
				//Begin TT#362 - MD - DOConnell - Running Balance to Proportional against a header attached to a placeholder does not reduce the Placeholder by the new allocated units
				else
				{
					// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
					//actionTransaction.AllocationSelHdrList = selectedHdrList;
					actionTransaction.AssortmentSelectedHdrList = selectedHdrList;
					// END TT#371-MD - stodd -  Velocity Interactive on Assortment
				}

				//BEGIN TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
				SetActiveAssortmentOnHeaders(selectedHdrList, true);
				// BEGIN TT#362 -MD - stodd - actions on headers not updating placeholders
				//AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
				// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
				//foreach (SelectedHeaderProfile shp in selectedHdrList.ArrayList)
				//{
				//    AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
				//    // END TT#371-MD - stodd -  Velocity Interactive on Assortment
				//    if (ap != null)
				//    {
				//        if (ap.HeaderType != eHeaderType.Placeholder && ap.HeaderType != eHeaderType.Assortment)
				//        {
				//            ap.ActivateAssortment = true;
				//        }
				//    }
				//}
				//END TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
				// END TT#362 -MD - stodd - actions on headers not updating placeholders
				//End TT#362 - MD - DOConnell - Running Balance to Proportional against a header attached to a placeholder does not reduce the Placeholder by the new allocated units
				// End TT#1228 - stodd - header select
				//BEGIN TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
				//actionTransaction = NewTransForProcessingHeaders(allocHdrKeyList);
				//END TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
				Cursor.Current = Cursors.WaitCursor;

				if (_transaction.AllocationFilterID != Include.NoRID)
					aStoreFilter = _transaction.AllocationFilterID;
				else
					aStoreFilter = Include.AllStoreFilterRID;
				AllocationWorkFlowStep aAllocationWorkFlowStep
					= new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
				actionTransaction.DoAllocationAction(aAllocationWorkFlowStep);

				// BEGIN TT#1441 - stodd - Need won't process on Placeholders
				//if (action == (int)eAssortmentAllocationActionType.StyleNeed)
				//{
				//    OutStoresInBlockedStyles(selectedHdrList);
				//}
				// END TT#1441 - stodd - Need won't process on Placeholders

				//BEGIN TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
				SetActiveAssortmentOnHeaders(selectedHdrList, false);
				// BEGIN TT#362 -MD - stodd - actions on headers not updating placeholders
				// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
				//foreach (int hdrRid in allocHdrKeyList)
				//foreach (SelectedHeaderProfile shp in selectedHdrList.ArrayList)
				//{
				//    AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
				//    // END TT#371-MD - stodd -  Velocity Interactive on Assortment
				//    if (ap != null)
				//    {
				//        if (ap.HeaderType != eHeaderType.Placeholder && ap.HeaderType != eHeaderType.Assortment)
				//        {
				//            ap.ActivateAssortment = false;
				//        }
				//    }
				//}
				//END TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
				// END TT#362 -MD - stodd - actions on headers not updating placeholders

				//Begin TT#362 - MD - DOConnell - Running Balance to Proportional against a header attached to a placeholder does not reduce the Placeholder by the new allocated units
				// ReRead headers in assortment cube to get latest data
				//BEGIN TT#501 - MD - DOConnell - On matrix tab, grade quantities revert to 0 when store attribute is changed
				//BEGIN TT#545-MD - stodd -  Processing cancel allocation action on header attached to placeholder does not adjust placeholder value  
				//_asrtCubeGroup.ReReadHeaders();
				//END TT#545-MD - stodd -  Processing cancel allocation action on header attached to placeholder does not adjust placeholder value  
				//// Reload summary data into cube totals 
				//_asrtCubeGroup.ClearTotalCubes(true);

				//_asrtCubeGroup.SaveCubeGroup();
				//CloseAndReOpenCubeGroup();
				//END TT#501 - MD - DOConnell - On matrix tab, grade quantities revert to 0 when store attribute is changed

				_refreshAssortmentWorkspace = true;	// TT#1261 - stodd
				//End TT#362 - MD - DOConnell - Running Balance to Proportional against a header attached to a placeholder does not reduce the Placeholder by the new allocated units
				// BEGIN TT#498-MD - stodd -  handle Alloc prof exceptions properly
			}
			catch (MIDException MIDexc)
			{
				HandleMIDException(MIDexc);
			}
			catch (SpreadFailed err)
			{
				MessageBox.Show(err.Message, this.Text);
			}
			catch (HeaderInUseException err)
			{
				string headerListMsg = string.Empty;
				foreach (string headerId in err.HeaderList)
				{
					if (headerListMsg.Length > 0)
						headerListMsg += ", " + headerId;
					else
						headerListMsg = " " + headerId;
				}
				MessageBox.Show(err.Message + headerListMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception err)
			{
				HandleException(err);
			}
			finally
			{

			}
			// END TT#498-MD - stodd -  handle Alloc prof exceptions properly
			return selectedHdrList.Count;	// TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
		}

		// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
		private bool VerifySecurity(SelectedHeaderList selectedHeaderList)
		{
			HierarchyNodeSecurityProfile hierNodeSecProfile; // TT#2 - RMatelic - Assortment Planning
			try
			{
				bool allowUpdate = true;
				foreach (SelectedHeaderProfile shp in selectedHeaderList)
				{
					// BEGIN TT#488-MD - STodd - Group Allocation 
					//AllocationProfile ap = _transaction.GetAllocationProfile(shp.Key);
					AllocationProfile ap = _transaction.GetAssortmentMemberProfile(shp.Key);
					// END TT#488-MD - STodd - Group Allocation 
					if (ap != null && ap.StyleHnRID > 0)
					{
						hierNodeSecProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(ap.StyleHnRID, (int)eSecurityTypes.Allocation);
						if (!hierNodeSecProfile.AllowUpdate)
						{
							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(shp.StyleHnRID, false, false);
							allowUpdate = false;
							string errorMessage = MIDText.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
							errorMessage = errorMessage + " Node: " + hnp.Text;
							SAB.ClientServerSession.Audit.Add_Msg(
								eMIDMessageLevel.Warning,
								eMIDTextCode.msg_NotAuthorizedForNode,
								errorMessage,
								"Assortment View");
							break;
						}
					}
					// End TT#2
				}
				return allowUpdate;
			}
			catch
			{
				throw;
			}
		}
		// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.

		//BEGIN TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
		/// <summary>
		/// This methods sets the ActiveAssortment switch on headers.
		/// When the ActiveAssortment switch is true, the header automatically adjusts the placeholder it is attached to.
		/// </summary>
		/// <param name="selectedHdrList"></param>
		/// <param name="isActive"></param>
		private void SetActiveAssortmentOnHeaders(ArrayList selectedHdrList, bool isActive)
		{
			// BEGIN TT#488-MD - STodd - Group Allocation 
			//AllocationProfileList apl = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
			AllocationProfileList apl = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.AssortmentMember);
			// END TT#488-MD - STodd - Group Allocation 
			foreach (int hdrRid in selectedHdrList)
			{
				AllocationProfile ap = (AllocationProfile)apl.FindKey(hdrRid);
				if (ap != null)
				{
					if (ap.HeaderType != eHeaderType.Placeholder && ap.HeaderType != eHeaderType.Assortment)
					{
						ap.ActivateAssortment = isActive;
					}
				}
			}
		}

		/// <summary>
		/// This methods sets the ActiveAssortment switch on headers.
		/// When the ActiveAssortment switch is true, the header automatically adjusts the placeholder it is attached to.
		/// </summary>
		/// <param name="selectedHdrList"></param>
		/// <param name="isActive"></param>
		private void SetActiveAssortmentOnHeaders(ProfileList selectedHdrList, bool isActive)
		{
			ArrayList hdrList = new ArrayList();
			foreach (SelectedHeaderProfile shp in selectedHdrList.ArrayList)
			{
				hdrList.Add(shp.Key);
			}
			SetActiveAssortmentOnHeaders(hdrList, isActive);
		}
		//END TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 

		// BEGIN TT#1441 - stodd - Need won't process on Placeholders
		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//private void OutStoresInBlockedStyles(List<int> allocHdrKeyList)
		//private void OutStoresInBlockedStyles(SelectedHeaderList selectedHdrList)
		//// END TT#371-MD - stodd -  Velocity Interactive on Assortment
		//{
		//    AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
		//    ArrayList blockedKeyList = new ArrayList();
		//    IDictionaryEnumerator iEnum = null;
		//    BlockedListHashKey blockedKey = null;
		//    bool didUpdateHdr = false;
		//    // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//    //foreach (int hdrRid in allocHdrKeyList)
		//    foreach (SelectedHeaderProfile shp in selectedHdrList.ArrayList)
		//    {
		//        AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
		//        // END TT#371-MD - stodd -  Velocity Interactive on Assortment
		//        if (ap != null)
		//        {
		//            iEnum = _asrtCubeGroup.BlockedList.GetEnumerator();

		//            while (iEnum.MoveNext())
		//            {
		//                blockedKey = (BlockedListHashKey)iEnum.Key;
		//                if (blockedKey.PlaceholderRID == ap.Key)
		//                {
		//                    ProfileList gradeStoreList = _asrtCubeGroup.GetStoresInSetGrade(blockedKey.StrGrpLvlRID, blockedKey.GradeRID);
		//                    ProfileList storeList = new ProfileList(eProfileType.Store);
		//                    foreach (StoreProfile sp in gradeStoreList)
		//                    {
		//                        if (sp.Key != _transaction.ReserveStore.RID)
		//                        {
		//                            storeList.Add(sp);
		//                        }
		//                    }
		//                    ap.SetAllocatedUnits(storeList, 0);
		//                    didUpdateHdr = true;
		//                }
		//            }
		//        }
		//        if (didUpdateHdr)
		//        {
		//            //BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
		//            //ap.TotalUnitsToAllocate = ap.TotalUnitsAllocated;
		//            //END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
		//            // BEGIN TT#376-MD - stodd - Update Enqueue logic
		//            if (!ap.WriteHeader())
		//            {
		//                EnqueueError(ap);
		//                return;
		//            }
		//            // END TT#376-MD - stodd - Update Enqueue logic
		//        }
		//    }
		//}
		// END TT#1441 - stodd


		//BEGIN TT#501 - MD - DOConnell - commented out old version can be removed after 2/1/2014
		//private void ProcessAssortmentMethod(ref ApplicationSessionTransaction actionTransaction, int action, GeneralComponent aComponent, ApplicationBaseAction aMethod, bool aReviewFlag, bool aUseSystemTolerancePercent, double aTolerancePercent, int aStoreFilter, int aWorkFlowStepKey)
		//{
		//    // BEGIN TT#498-MD - stodd -  handle exceptions properly
		//    try
		//    {
		//        // END TT#498-MD - stodd -  handle Alloc prof exceptions properly
		//        // Begin TT#1228 - stodd - header select
		//        // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//        //List<int> allocHdrKeyList = GetSelectableHeaderList(action);
		//        SelectedHeaderList selectedHdrList = GetSelectableHeaderList(action);
		//        // END TT#371-MD - stodd -  Velocity Interactive on Assortment

		//        // BEGIN TT#1936 - stodd - cancel allocation/assortment
		//        // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//        //if (allocHdrKeyList.Count == 0 && action != Convert.ToInt32(eAssortmentActionType.CreatePlaceholders))
		//        if (selectedHdrList.Count == 0 && action != Convert.ToInt32(eAssortmentActionType.CreatePlaceholders))
		//        // END TT#371-MD - stodd -  Velocity Interactive on Assortment
		//        {
		//            string errorMessage = MIDText.GetText(eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected);
		//            SAB.ClientServerSession.Audit.Add_Msg(
		//                eMIDMessageLevel.Warning,
		//                eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected,
		//                errorMessage,
		//                "Assortment View");
		//        }
		//        // END TT#1936 - stodd - cancel allocation/assortment

		//        // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//        if (!OKToProcessAssortment((eAssortmentActionType)action, selectedHdrList))
		//        {
		//            return;
		//        }
		//        // Begin TT#219 - MD - DOConnell - Spread Average not getting expected results
		//        else
		//        {
		//            // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//            //actionTransaction.AllocationSelHdrList = selectedHdrList;
		//            actionTransaction.AssortmentSelectedHdrList = selectedHdrList;
		//            // END TT#371-MD - stodd -  Velocity Interactive on Assortment
		//        }
		//        // END TT#371-MD - stodd -  Velocity Interactive on Assortment
		//        // End TT#219 - MD - DOConnell - Spread Average not getting expected results
		//        // End TT#1228 - stodd - header select
		//        //BEGIN TT#9-MD - DOConnell - Cannot drag and drop header into assortment
		//        //actionTransaction = NewTransForProcessingHeaders(allocHdrKeyList);
		//        //END TT#9-MD - DOConnell - Cannot drag and drop header into assortment 
		//        AssortmentWorkFlowStep aAssortmentWorkFlowStep = null;
		//        // Begin TT#2 - stodd - asortment
		//        AssortmentProfile asp = (AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile;
		//        ProfileList gradeProfList = asp.GetStoreGrades();
		//        ProfileList sgll = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);
		//        // End TT#2 - stodd - asortment
		//        eAllocationActionStatus actionStatus;
		//        switch ((eAssortmentActionType)action)
		//        {
		//            case eAssortmentActionType.CreatePlaceholders:
		//                AverageUnitDialog averageUnitFrm = new AverageUnitDialog(_storeGradeProfileList, eAllocationAssortmentViewGroupBy.StoreGrade);
		//                if (averageUnitFrm.ShowDialog() == DialogResult.OK)
		//                {
		//                    Cursor.Current = Cursors.WaitCursor;
		//                    ((AssortmentAction)aMethod).AverageUnitList = averageUnitFrm.ResultList;
		//                    ((AssortmentAction)aMethod).ViewGroupBy = eAllocationAssortmentViewGroupBy.StoreGrade;
		//                    ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
		//                    aAssortmentWorkFlowStep
		//                        = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                    actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

		//                    actionStatus = actionTransaction.AllocationActionAllHeaderStatus;
		//                    if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
		//                    {
		//                        _sab.HeaderServerSession.ReloadHeaders(false);
		//                        AllocationProfileList apl = (AllocationProfileList)actionTransaction.GetMasterProfileList(eProfileType.Allocation);
		//                        AllocationHeaderProfileList newHeaderList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
		//                        if (apl != null)
		//                        {
		//                            foreach (object asrtObj in apl)
		//                            {
		//                                AllocationProfile ap = (AllocationProfile)asrtObj;
		//                                AllocationHeaderProfile ahp = _sab.HeaderServerSession.GetHeaderData(ap.Key, true, true, true);
		//                                if (!newHeaderList.Contains(ap.Key))
		//                                {
		//                                    newHeaderList.Add(ahp);
		//                                }
		//                            }
		//                        }
		//                        _transaction.RefreshProfileLists(eProfileType.AllocationHeader);
		//                        _transaction.SetCriteriaHeaderList(newHeaderList);
		//                        _transaction.NewAssortmentCriteriaHeaderList();
		//                        _headerList = (AllocationHeaderProfileList)_transaction.GetMasterProfileList(eProfileType.AllocationHeader);

		//                        _buildDetailsGrid = true;	// TT#1262 - stodd
		//                        // Begin TT#1227 - stodd
		//                        //_asrtCubeGroup.SaveCubeGroup();
		//                        //CloseAndReOpenCubeGroup();
		//                        //UpdateData(true);
		//                        //LoadSurroundingPages();
		//                        //_buildDetailsGrid = true;	// TT#1262 - stodd
		//                        _buildProductCharsGrid = true;
		//                        LoadProductCharGrid();
		//                        _refreshAssortmentWorkspace = true; // TT#1322 - stodd
		//                        UpdateOtherViews();
		//                        // Begin TT#1262 - stodd - header units
		//                        //SaveChanges();
		//                        // End TT#1262 - stodd - header units
		//                        // End TT#1227 - stodd
		//                    }
		//                }
		//                // BEGIN TT#1930 - stodd - argument exception
		//                if (ugDetails.Rows.Count > 1 || g4.Rows.Count > 1)	// g4 will have one row for the headings
		//                {
		//                    EnableCreatePlaceholderAction(false);
		//                }
		//                else
		//                {
		//                    EnableCreatePlaceholderAction(true);
		//                }
		//                // END TT#1930 - stodd - argument exception
		//                break;

		//            case eAssortmentActionType.Redo:
		//                ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
		//                aAssortmentWorkFlowStep
		//                    = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

		//                // ReRead headers in assortment cube to get latest data
		//                _asrtCubeGroup.ReReadHeaders();
		//                // Rebuilds assortment summary to get latest data
		//                AssortmentProfile assortProf = (AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile;
		//                // BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
		//                RebuildAssortmentSummary(assortProf);
		//                // End TT#1876 - stodd - summary incorrect when coming from selection window
		//                break;

		//            case eAssortmentActionType.SpreadAverage:
		//                List<string> gradeList = new List<string>();
		//                // Begin TT#2 - stodd - asortment
		//                //AssortmentProfile asp = (AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile;
		//                //ProfileList gradeProfList = asp.GetStoreGrades();
		//                // Begin TT#2 - stodd - asortment
		//                foreach (StoreGradeProfile sgp in gradeProfList.ArrayList)
		//                {
		//                    gradeList.Add(sgp.StoreGrade);
		//                }
		//                IndexToAverageDialog idxToAvg = new IndexToAverageDialog(SAB, gradeList);
		//                DialogResult _result = idxToAvg.ShowDialog();

		//                //========================================================================================
		//                // How this works...
		//                // If the user chose "Total" or "Set Total", only a single value will be returned.
		//                // If the user chose "grade" a datatable is returned with their selections.
		//                //========================================================================================
		//                if (_result == DialogResult.Yes)
		//                {
		//                    Cursor.Current = Cursors.WaitCursor;
		//                    eIndexToAverageReturnType returnType = idxToAvg.IndexToAverageReturnType;
		//                    ((AssortmentAction)aMethod).IndexToAverageReturnType = returnType;
		//                    ((AssortmentAction)aMethod).CurrSglRid = _lastStoreGroupLevelValue;
		//                    ((AssortmentAction)aMethod).SpreadAverageOption = idxToAvg.SpreadOption;
		//                    if (returnType == eIndexToAverageReturnType.Total ||
		//                        returnType == eIndexToAverageReturnType.SetTotal)
		//                    {
		//                        double dbReturn = Convert.ToDouble(idxToAvg.IndexToAverageReturnValue);
		//                        ((AssortmentAction)aMethod).AverageUnits = dbReturn;
		//                    }
		//                    else if (returnType == eIndexToAverageReturnType.Grades)
		//                    {
		//                        DataTable dtReturn = (DataTable)idxToAvg.IndexToAverageReturnValue;
		//                        Hashtable gradeAverageUnitsHash = new Hashtable();
		//                        foreach (DataRow arow in dtReturn.Rows)
		//                        {
		//                            try
		//                            {
		//                                double gradeValue = double.Parse(arow["Value"].ToString());
		//                                gradeAverageUnitsHash.Add(arow["Grade"].ToString(), gradeValue);
		//                            }
		//                            catch
		//                            {
		//                                // The value cannot be parsed into a double,
		//                                // skip the grade.
		//                            }
		//                        }
		//                        ((AssortmentAction)aMethod).GradeAverageUnitsHash = gradeAverageUnitsHash;
		//                    }

		//                    //==================================
		//                    // Process Index to Average action
		//                    //==================================
		//                    aAssortmentWorkFlowStep
		//                    = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                    actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);
		//                    //BEGIN TT#501 - MD - DOConnell - On matrix tab, grade quantities revert to 0 when store attribute is changed
		//                    // ReRead headers in assortment cube to get latest data
		//                    //_asrtCubeGroup.ReReadHeaders();
		//                    //Rebuilds assortment summary to get latest data
		//                    //AssortmentProfile assortProf = (AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile;
		//                    //asp.AssortmentSummaryProfile.RereadStoreSummaryData();
		//                    //asp.AssortmentSummaryProfile.BuildSummary(_lastStoreGroupValue);
		//                    // Reload summary data into cube totals 
		//                    //_asrtCubeGroup.ClearTotalCubes(true);
		//                    // reload assortment grid
		//                    // Moved to BtnProcess_click
		//                    // UpdateData(true);
		//                    _refreshAssortmentWorkspace = true;	// TT#1261 - stodd
		//                    //END TT#501 - MD - DOConnell - On matrix tab, grade quantities revert to 0 when store attribute is changed
		//                }
		//                break;

		//            case eAssortmentActionType.CancelAssortment:
		//                ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
		//                aAssortmentWorkFlowStep
		//                    = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

		//                // ReRead headers in assortment cube to get latest data
		//                //_asrtCubeGroup.ReReadHeaders();
		//                // Rebuilds assortment summary to get latest data
		//                //AssortmentProfile assortProf1 = (AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile;
		//                //assortProf1.AssortmentSummaryProfile.RereadStoreSummaryData();
		//                //assortProf1.AssortmentSummaryProfile.BuildSummary(_lastStoreGroupValue);
		//                // Reload summary data into cube totals 
		//                //_asrtCubeGroup.ClearTotalCubes(true);
		//                _refreshAssortmentWorkspace = true;	// TT#1261 - stodd
		//                break;

		//            case eAssortmentActionType.BalanceAssortment:
		//                //double summaryCellValue = 0.0d;
		//                //double totalCellValue = 0.0d;

		//                //RecomputePlanCubes();

		//                // Balance Set / Grade
		//                foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
		//                {
		//                    foreach (StoreGradeProfile sgp in gradeProfList.ArrayList)
		//                    {
		//                        //Cube aCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryGrade);
		//                        //AssortmentCellReference asrtCellRef = new AssortmentCellReference((AssortmentSummaryGrade)aCube);
		//                        //asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
		//                        //asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
		//                        //asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.Units).Key;
		//                        //asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
		//                        //// cellValue is total basis units for set / grade
		//                        //summaryCellValue = asrtCellRef.CurrentCellValue;

		//                        //asrtCellRef = (AssortmentCellReference)_asrtCubeGroup.GetCube(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0)).CreateCellReference();
		//                        //asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
		//                        //asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
		//                        //asrtCellRef[eProfileType.AssortmentDetailVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList.FindKey((int)eAssortmentDetailVariables.TotalUnits).Key;
		//                        //asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
		//                        //// cellValue is total basis units for set / grade
		//                        //totalCellValue = asrtCellRef.CurrentCellValue;
		//                        //asrtCellRef.SetEntryCellValue(summaryCellValue);

		//                        AssortmentCellReference asrtCellRef9 = (AssortmentCellReference)_asrtCubeGroup.GetCube(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0)).CreateCellReference();
		//                        asrtCellRef9[eProfileType.StoreGroupLevel] = sglp.Key;
		//                        asrtCellRef9[eProfileType.StoreGrade] = sgp.Key;
		//                        asrtCellRef9[eProfileType.AssortmentDetailVariable] = ((AssortmentViewDetailVariables)_asrtCubeGroup.AssortmentComputations.AssortmentDetailVariables).TotalUnits.Key;
		//                        asrtCellRef9[eProfileType.AssortmentQuantityVariable] = ((AssortmentViewQuantityVariables)_asrtCubeGroup.AssortmentComputations.AssortmentQuantityVariables).Balance.Key;
		//                        double balCellValue = asrtCellRef9.CurrentCellValue;
		//                        asrtCellRef9.SetEntryCellValue(0);
		//                    }
		//                    RecomputePlanCubes();
		//                }
		//                StopPageLoadThreads();
		//                //RecomputePlanCubes();
		//                SaveDetailCubeGroup();
		//                // BEGIN TT#2098 - stodd - After Assortment Balance Quantity on Content tab does not change
		//                // Updates Quantity to allocate
		//                AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
		//                // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//                //foreach (int hdrRid in allocHdrKeyList)
		//                foreach (SelectedHeaderProfile shp in selectedHdrList)
		//                {
		//                    AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
		//                    // END TT#371-MD - stodd -  Velocity Interactive on Assortment
		//                    if (ap != null)
		//                    {
		//                        ap.TotalUnitsToAllocate = ap.TotalUnitsAllocated;
		//                        // BEGIN TT#376-MD - stodd - Update Enqueue logic
		//                        if (!ap.WriteHeader())
		//                        {
		//                            EnqueueError(ap);
		//                            return;
		//                        }
		//                        // END TT#376-MD - stodd - Update Enqueue logic
		//                    }
		//                }
		//                // END TT#2098 - stodd - After Assortment Balance Quantity on Content tab does not change
		//                // Content grid
		//                SaveGridChanges();
		//                //BEGIN TT#501 - MD - DOConnell - On matrix tab, grade quantities revert to 0 when store attribute is changed
		//                foreach (SelectedHeaderProfile shp in selectedHdrList)
		//                {
		//                    AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
		//                    ReloadProfileToGrid(ap.Key);
		//                }
		//                //END TT#501 - MD - DOConnell - On matrix tab, grade quantities revert to 0 when store attribute is changed

		//                //CloseAndReOpenCubeGroup();
		//                // Begin TT#1227 - stodd
		//                // ReRead headers in assortment cube to get latest data
		//                //_asrtCubeGroup.ReReadHeaders();
		//                //UpdateData(true);

		//                //LoadCurrentPages();
		//                //LoadSurroundingPages();

		//                //_buildDetailsGrid = true;
		//                //_buildProductCharsGrid = true;
		//                //LoadProductCharGrid();

		//                //UpdateOtherViews();
		//                // End TT#1227 - stodd
		//                _refreshAssortmentWorkspace = true; // TT#1322 - stodd

		//                //=============================================================
		//                // Currently all this does is set the status to successful
		//                //=============================================================
		//                aAssortmentWorkFlowStep
		//                    = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);
		//                _refreshAssortmentWorkspace = true;	// TT#1261 - stodd
		//                break;

		//            // BEGIN TT#1225 - stodd - charge intransit
		//            //case eAssortmentActionType.ChargeIntransit:
		//            //    //===================================================//
		//            //    // Remove any header type we don't want to process.
		//            //    //===================================================//
		//            //    //AllocationProfileList nonProcessApl1 = new AllocationProfileList(eProfileType.AllocationHeader);
		//            //    //AllocationProfileList apl1 = actionTransaction.GetAllocationProfileList();
		//            //    //foreach (AllocationProfile ap in apl1.ArrayList)
		//            //    //{
		//            //    //    if (ap.HeaderType == eHeaderType.ASN || ap.HeaderType == eHeaderType.Receipt || ap.HeaderType == eHeaderType.Reserve)
		//            //    //    {
		//            //    //    }
		//            //    //    else
		//            //    //    {
		//            //    //        nonProcessApl1.Add(ap);
		//            //    //    }
		//            //    //}
		//            //    //actionTransaction.RemoveAllocationProfile(nonProcessApl1);

		//            //    ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
		//            //    aAssortmentWorkFlowStep
		//            //        = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//            //    actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

		//            //    break;

		//            //case eAssortmentActionType.CancelIntransit:
		//            //    //===================================================//
		//            //    // Remove any header type we don't want to process.
		//            //    //===================================================//
		//            //    //AllocationProfileList nonProcessApl2 = new AllocationProfileList(eProfileType.AllocationHeader);
		//            //    //AllocationProfileList apl2 = actionTransaction.GetAllocationProfileList();
		//            //    //foreach (AllocationProfile ap in apl2.ArrayList)
		//            //    //{
		//            //    //    if (ap.HeaderType == eHeaderType.ASN || ap.HeaderType == eHeaderType.Receipt || ap.HeaderType == eHeaderType.Reserve)
		//            //    //    {
		//            //    //    }
		//            //    //    else
		//            //    //    {
		//            //    //        nonProcessApl2.Add(ap);
		//            //    //    }
		//            //    //}
		//            //    //actionTransaction.RemoveAllocationProfile(nonProcessApl2);

		//            //    ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
		//            //    aAssortmentWorkFlowStep
		//            //        = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//            //    actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

		//            //    break;
		//            //// End TT#1225 - stodd - charge intransit

		//            default:
		//                ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
		//                aAssortmentWorkFlowStep
		//                    = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);
		//                // ReRead headers in assortment cube to get latest data
		//                //_asrtCubeGroup.ReReadHeaders();
		//                _refreshAssortmentWorkspace = true; // TT#1322 - stodd
		//                break;
		//        }
		//        // BEGIN TT#498-MD - stodd -  handle Alloc prof exceptions properly
		//    }
		//    catch (MIDException MIDexc)
		//    {
		//        HandleMIDException(MIDexc);
		//    }
		//    catch (HeaderInUseException err)
		//    {
		//        string headerListMsg = string.Empty;
		//        foreach (string headerId in err.HeaderList)
		//        {
		//            if (headerListMsg.Length > 0)
		//                headerListMsg += ", " + headerId;
		//            else
		//                headerListMsg = " " + headerId;
		//        }
		//        MessageBox.Show(err.Message + headerListMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		//    }
		//    catch (Exception err)
		//    {
		//        HandleException(err);
		//    }
		//    finally
		//    {

		//    }
		//    // END TT#498-MD - stodd -  handle Alloc prof exceptions properly
		//}
		//END TT#501 - MD - DOConnell - commented out old version can be removed after 2/1/2014

		//BEGIN TT#501 - MD - DOConnell - On matrix tab, grade quantities revert to 0 when store attribute is changed
		//private int ProcessAssortmentMethod(ref ApplicationSessionTransaction actionTransaction, int action, GeneralComponent aComponent, ApplicationBaseAction aMethod, bool aReviewFlag, bool aUseSystemTolerancePercent, double aTolerancePercent, int aStoreFilter, int aWorkFlowStepKey)
		//{
		//    SelectedHeaderList selectedHdrList = GetSelectableHeaderList(action);
		//    try
		//    {
		//        if (selectedHdrList.Count == 0 && action != Convert.ToInt32(eAssortmentActionType.CreatePlaceholders))
		//        {
		//            string errorMessage = MIDText.GetText(eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected);
		//            SAB.ClientServerSession.Audit.Add_Msg(
		//                eMIDMessageLevel.Warning,
		//                eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected,
		//                errorMessage,
		//                "Assortment View");
		//            // BEGIN TT#529-MD - stodd -  Status not updated on workspace After running Balance Proportional 
		//            return selectedHdrList.Count;
		//            // END TT#529-MD - stodd -  Status not updated on workspace After running Balance Proportional 
		//        }

		//        if (!OKToProcessAssortment((eAssortmentActionType)action, selectedHdrList))
		//        {
		//            return selectedHdrList.Count;
		//        }
		//        else
		//        {
		//            actionTransaction.AssortmentSelectedHdrList = selectedHdrList;
		//        }

		//        AssortmentWorkFlowStep aAssortmentWorkFlowStep = null;
		//        AssortmentProfile asp = (AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile;
		//        ProfileList gradeProfList = asp.GetStoreGrades();
		//        ProfileList sgll = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);
		//        eAllocationActionStatus actionStatus;
		//        switch ((eAssortmentActionType)action)
		//        {
		//            case eAssortmentActionType.CreatePlaceholders:
		//                AverageUnitDialog averageUnitFrm = new AverageUnitDialog(_storeGradeProfileList, eAllocationAssortmentViewGroupBy.StoreGrade);
		//                if (averageUnitFrm.ShowDialog() == DialogResult.OK)
		//                {
		//                    Cursor.Current = Cursors.WaitCursor;
		//                    ((AssortmentAction)aMethod).AverageUnitList = averageUnitFrm.ResultList;
		//                    ((AssortmentAction)aMethod).ViewGroupBy = eAllocationAssortmentViewGroupBy.StoreGrade;
		//                    ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
		//                    aAssortmentWorkFlowStep
		//                        = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                    actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

		//                    actionStatus = actionTransaction.AllocationActionAllHeaderStatus;
		//                    if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
		//                    {
		//                        _sab.HeaderServerSession.ReloadHeaders(false);
		//                        AllocationProfileList apl = (AllocationProfileList)actionTransaction.GetMasterProfileList(eProfileType.Allocation);
		//                        AllocationHeaderProfileList newHeaderList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
		//                        if (apl != null)
		//                        {
		//                            foreach (object asrtObj in apl)
		//                            {
		//                                AllocationProfile ap = (AllocationProfile)asrtObj;
		//                                AllocationHeaderProfile ahp = _sab.HeaderServerSession.GetHeaderData(ap.Key, true, true, true);
		//                                if (!newHeaderList.Contains(ap.Key))
		//                                {
		//                                    newHeaderList.Add(ahp);
		//                                }
		//                            }
		//                        }
		//                        _transaction.RefreshProfileLists(eProfileType.AllocationHeader);
		//                        _transaction.SetCriteriaHeaderList(newHeaderList);
		//                        _transaction.NewAssortmentCriteriaHeaderList();
		//                        _headerList = (AllocationHeaderProfileList)_transaction.GetMasterProfileList(eProfileType.AllocationHeader);

		//                        _buildDetailsGrid = true;
		//                        _asrtCubeGroup.SaveCubeGroup();
		//                        CloseAndReOpenCubeGroup();
		//                        UpdateData(true);
		//                        LoadSurroundingPages();
		//                        _buildProductCharsGrid = true;
		//                        LoadProductCharGrid();
		//                        _refreshAssortmentWorkspace = true;
		//                        UpdateOtherViews();
		//                        //BEGIN TT#545-MD - stodd -  Processing cancel allocation action on header attached to placeholder does not adjust placeholder value  
		//                        //SaveChanges(); //TT#472 - MD - DOConnell - Getting and error saying that there are no placeholders selected when trying to run the Create Placeholder method
		//                        //END TT#545-MD - stodd -  Processing cancel allocation action on header attached to placeholder does not adjust placeholder value
		//                        //BEGIN TT#674 - MD - DOConnell - Argument Exception error received when adding characteristics to an Assortment
		//                        //UpdateSelectableComponentList(true); //TT#416 - MD - DOConnell - Characteristics do not appear in the Column Chooser unless you save the assortment, close the application, and go back in.  Should update automatically.
		//                        //END TT#674 - MD - DOConnell - Argument Exception error received when adding characteristics to an Assortment
		//                        ComponentsChanged();	// TT#656-MD - stodd - "Close Style" not available  
		//                    }
		//                }
		//                if (ugDetails.Rows.Count > 1 || g4.Rows.Count > 1)	// g4 will have one row for the headings
		//                {
		//                    EnableCreatePlaceholderAction(false);
		//                }
		//                else
		//                {
		//                    EnableCreatePlaceholderAction(true);
		//                }
		//                break;

		//            case eAssortmentActionType.Redo:
		//                ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
		//                aAssortmentWorkFlowStep
		//                    = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

		//                // ReRead headers in assortment cube to get latest data
		//                _asrtCubeGroup.ReReadHeaders();
		//                // Rebuilds assortment summary to get latest data
		//                AssortmentProfile assortProf = (AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile;
		//                RebuildAssortmentSummary(assortProf);
		//                break;

		//            case eAssortmentActionType.SpreadAverage:
		//                List<string> gradeList = new List<string>();
		//                foreach (StoreGradeProfile sgp in gradeProfList.ArrayList)
		//                {
		//                    gradeList.Add(sgp.StoreGrade);
		//                }
		//                IndexToAverageDialog idxToAvg = new IndexToAverageDialog(SAB, gradeList);
		//                DialogResult _result = idxToAvg.ShowDialog();

		//                //========================================================================================
		//                // How this works...
		//                // If the user chose "Total" or "Set Total", only a single value will be returned.
		//                // If the user chose "grade" a datatable is returned with their selections.
		//                //========================================================================================
		//                if (_result == DialogResult.Yes)
		//                {
		//                    Cursor.Current = Cursors.WaitCursor;
		//                    eIndexToAverageReturnType returnType = idxToAvg.IndexToAverageReturnType;
		//                    ((AssortmentAction)aMethod).IndexToAverageReturnType = returnType;
		//                    ((AssortmentAction)aMethod).CurrSglRid = _lastStoreGroupLevelValue;
		//                    ((AssortmentAction)aMethod).SpreadAverageOption = idxToAvg.SpreadOption;
		//                    if (returnType == eIndexToAverageReturnType.Total ||
		//                        returnType == eIndexToAverageReturnType.SetTotal)
		//                    {
		//                        double dbReturn = Convert.ToDouble(idxToAvg.IndexToAverageReturnValue);
		//                        ((AssortmentAction)aMethod).AverageUnits = dbReturn;
		//                    }
		//                    else if (returnType == eIndexToAverageReturnType.Grades)
		//                    {
		//                        DataTable dtReturn = (DataTable)idxToAvg.IndexToAverageReturnValue;
		//                        Hashtable gradeAverageUnitsHash = new Hashtable();
		//                        foreach (DataRow arow in dtReturn.Rows)
		//                        {
		//                            try
		//                            {
		//                                double gradeValue = double.Parse(arow["Value"].ToString());
		//                                gradeAverageUnitsHash.Add(arow["Grade"].ToString(), gradeValue);
		//                            }
		//                            catch
		//                            {
		//                                // The value cannot be parsed into a double,
		//                                // skip the grade.
		//                            }
		//                        }
		//                        ((AssortmentAction)aMethod).GradeAverageUnitsHash = gradeAverageUnitsHash;
		//                    }

		//                    //BEGIN TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
		//                    SetActiveAssortmentOnHeaders(selectedHdrList, true);
		//                    //BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
		//                    // Sets the switch on the real header to automatically adjust the placeholder it's attached to
		//                    //AllocationProfileList apl = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
		//                    //foreach (SelectedHeaderProfile shp in selectedHdrList.ArrayList)
		//                    //{
		//                    //    AllocationProfile ap = (AllocationProfile)apl.FindKey(shp.Key);
		//                    //    // END TT#371-MD - stodd -  Velocity Interactive on Assortment
		//                    //    if (ap != null)
		//                    //    {
		//                    //        if (ap.HeaderType != eHeaderType.Placeholder && ap.HeaderType != eHeaderType.Assortment)
		//                    //        {
		//                    //            ap.ActivateAssortment = true;
		//                    //        }
		//                    //    }
		//                    //}
		//                    //END TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
		//                    //END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 

		//                    //==================================
		//                    // Process Index to Average action
		//                    //==================================
		//                    aAssortmentWorkFlowStep
		//                    = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                    actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

		//                    //BEGIN TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
		//                    SetActiveAssortmentOnHeaders(selectedHdrList, false);
		//                    //BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
		//                    //foreach (SelectedHeaderProfile shp in selectedHdrList.ArrayList)
		//                    //{
		//                    //    AllocationProfile ap = (AllocationProfile)apl.FindKey(shp.Key);
		//                    //    // END TT#371-MD - stodd -  Velocity Interactive on Assortment
		//                    //    if (ap != null)
		//                    //    {
		//                    //        if (ap.HeaderType != eHeaderType.Placeholder && ap.HeaderType != eHeaderType.Assortment)
		//                    //        {
		//                    //            ap.ActivateAssortment = false;
		//                    //        }
		//                    //    }
		//                    //}
		//                    //END TT#562-MD - stodd -  Bulk is not getting properly updated upon an assortment save of placeholders 
		//                    //END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 

		//                    _buildDetailsGrid = true;
		//                    _buildProductCharsGrid = true;
		//                    _refreshAssortmentWorkspace = true;
		//                }
		//                break;

		//            case eAssortmentActionType.CancelAssortment:
		//                ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
		//                aAssortmentWorkFlowStep
		//                    = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);

		//                _refreshAssortmentWorkspace = true;
		//                break;

		//            case eAssortmentActionType.BalanceAssortment:
		//                // Balance Set / Grade
		//                foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
		//                {
		//                    foreach (StoreGradeProfile sgp in gradeProfList.ArrayList)
		//                    {
		//                        AssortmentCellReference asrtCellRef9 = (AssortmentCellReference)_asrtCubeGroup.GetCube(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0)).CreateCellReference();
		//                        asrtCellRef9[eProfileType.StoreGroupLevel] = sglp.Key;
		//                        asrtCellRef9[eProfileType.StoreGrade] = sgp.Key;
		//                        asrtCellRef9[eProfileType.AssortmentDetailVariable] = ((AssortmentViewDetailVariables)_asrtCubeGroup.AssortmentComputations.AssortmentDetailVariables).TotalUnits.Key;
		//                        asrtCellRef9[eProfileType.AssortmentQuantityVariable] = ((AssortmentViewQuantityVariables)_asrtCubeGroup.AssortmentComputations.AssortmentQuantityVariables).Balance.Key;
		//                        double balCellValue = asrtCellRef9.CurrentCellValue;
		//                        asrtCellRef9.SetEntryCellValue(0);
		//                    }
		//                    RecomputePlanCubes();
		//                }
		//                StopPageLoadThreads();
		//                SaveDetailCubeGroup();

		//                // Updates Quantity to allocate
		//                AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
		//                foreach (SelectedHeaderProfile shp in selectedHdrList)
		//                {
		//                    AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
		//                    if (ap != null)
		//                    {
		//                        //BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
		//                        //ap.TotalUnitsToAllocate = ap.TotalUnitsAllocated;
		//                        //END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
		//                        if (!ap.WriteHeader())
		//                        {
		//                            EnqueueError(ap);
		//                            return selectedHdrList.Count;
		//                        }
		//                    }
		//                }
		//                SaveGridChanges();

		//                foreach (SelectedHeaderProfile shp in selectedHdrList)
		//                {
		//                    AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
		//                    ReloadProfileToGrid(ap.Key);
		//                }

		//                CloseAndReOpenCubeGroup();
		//                // ReRead headers in assortment cube to get latest data
		//                UpdateData(true);

		//                LoadSurroundingPages();

		//                _buildDetailsGrid = true;
		//                _buildProductCharsGrid = true;
		//                LoadProductCharGrid();

		//                UpdateOtherViews();

		//                //=============================================================
		//                // Currently all this does is set the status to successful
		//                //=============================================================
		//                aAssortmentWorkFlowStep
		//                    = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);
		//                _refreshAssortmentWorkspace = true;	// TT#1261 - stodd
		//                break;

		//            default:
		//                ((AssortmentAction)aMethod).StoreGroupRid = _lastStoreGroupValue;
		//                aAssortmentWorkFlowStep
		//                    = new AssortmentWorkFlowStep(aMethod, aComponent, aReviewFlag, _asrtCubeGroup, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
		//                actionTransaction.DoAssortmentAction(aAssortmentWorkFlowStep);
		//                break;
		//        }
		//    }
		//    catch (MIDException MIDexc)
		//    {
		//        HandleMIDException(MIDexc);
		//    }
		//    catch (HeaderInUseException err)
		//    {
		//        string headerListMsg = string.Empty;
		//        foreach (string headerId in err.HeaderList)
		//        {
		//            if (headerListMsg.Length > 0)
		//                headerListMsg += ", " + headerId;
		//            else
		//                headerListMsg = " " + headerId;
		//        }
		//        MessageBox.Show(err.Message + headerListMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		//    }
		//    catch (Exception err)
		//    {
		//        HandleException(err);
		//    }
		//    finally
		//    {

		//    }
		//    return selectedHdrList.Count;
		//}
		//END TT#501 - MD - DOConnell - On matrix tab, grade quantities revert to 0 when store attribute is changed

		// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
		/// <summary>
		/// Rebuilds assortment summary with the latest data
		/// </summary>
		/// <param name="Assp"></param>
		//private void RebuildAssortmentSummary(AssortmentProfile Assp)
		//{
		//    try
		//    {
		//        Assp.AssortmentSummaryProfile.RereadStoreSummaryData();
		//        Assp.AssortmentSummaryProfile.BuildSummary(_lastStoreGroupValue);
		//        // Reload summary data into cube totals 
		//        _asrtCubeGroup.ClearTotalCubes(true);
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}
		// END TT#1876 - stodd - summary incorrect when coming from selection window

		// Begin TT#1228 - stodd - selectable headers
		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//private List<int> GetSelectableHeaderList(int actionRid)
		// Begin TT#889 - MD - stodd - need not working
		private SelectedHeaderList GetSelectableHeaderList(int actionRid, ApplicationSessionTransaction aTrans, bool createTransProfileList)
		// End TT#889 - MD - stodd - need not working
		// END TT#371-MD - stodd -  Velocity Interactive on Assortment
		{
			bool actionSelectable = IsActionSelectable(actionRid);
			bool actionForHeadersOnly = IsActionForHeadersOnly(actionRid);
			bool AssortmentAction = IsAssortmentAction(actionRid);
			//eAssortmentActionType action = (eAssortmentActionType)actionRid;

			// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
			//List<int> selectedHdrKeyList = new List<int>();
			SelectedHeaderList selectedHdrList = new SelectedHeaderList(eProfileType.SelectedHeader);
			// END TT#371-MD - stodd -  Velocity Interactive on Assortment



			// 	if (actionSelectable && _selectedHeaderKeyList.Count > 0)		// TT#1490 - stodd

			// Nothing is selected, so we'll send the Assortment profile for group allocation

			//==========================================================================================================================
			// This checks to see if any headers are selected. If not, then load the selected header list with the assortment header.
			//==========================================================================================================================
			// BEGIN TT#488-MD - Stodd - Group Allocation
			//int selCnt = 0;
			//AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
			//foreach (SelectedHeaderProfile shp in _selectedHeaderList)
			//{
			//    AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
			//    if (ap != null)
			//    {
			//        if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
			//        {
			//            // doesn't count
			//        }
			//        else
			//        {
			//            selCnt++;
			//        }
			//    }
			//}

			if (midToolbarRadioButton1.Button1.Checked)
			// END TT#488-MD - Stodd - Group Allocation
			{
				SelectedHeaderProfile assortmentHeader = new SelectedHeaderProfile(_assortmentProfile.Key);
				assortmentHeader.HeaderType = eHeaderType.Assortment;
                selectedHdrList.Clear();  // TT#964 - MD - stodd - style review end up with additional header and bulk components
				selectedHdrList.Add(assortmentHeader);
			} 
            else	if (actionSelectable)
			{
				// BEGIN TT#488-MD - STodd - Group Allocation 
				//AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
				AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.AssortmentMember);
				// END TT#488-MD - STodd - Group Allocation 
				// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
				//foreach (int hdrRID in _selectedHeaderKeyList)
				foreach (SelectedHeaderProfile shp in _selectedHeaderList)
				// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
				{
					// BEGIN TT#1936 - stodd - cancel allocation/assortment
					// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
					AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
					// END TT#371-MD - stodd -  Velocity Interactive on Assortment
					if (ap != null)
					{
						// Begin TT#889 - MD - stodd - need not working
                        //if (actionForHeadersOnly)
                        //{
							if (ap.HeaderType == eHeaderType.Placeholder || ap.HeaderType == eHeaderType.Assortment)
							{
								// Skip these types of headers
                                // Note: If needed, the assortment header was already added.
							}
							else
							{
								// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
								//selectedHdrKeyList.Add(hdrRID);
								SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ap.Key);
								selectedHeader.HeaderType = ap.HeaderType;
								selectedHdrList.Add(selectedHeader);
								// END TT#371-MD - stodd -  Velocity Interactive on Assortment
							}
                        //}
                        //else
                        //{
                        //    // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                        //    //selectedHdrKeyList.Add(hdrRID);
                        //    SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ap.Key);
                        //    selectedHeader.HeaderType = ap.HeaderType;
                        //    selectedHdrList.Add(selectedHeader);
                        //    // END TT#371-MD - stodd -  Velocity Interactive on Assortment
                        //}
						// END TT#1936 - stodd - cancel allocation/assortment
                        ////==============================================================================================================
                        //// When processing selected placeholders/headers, the assortment header that includes the selected headers
                        //// also needs to be included. This looks up and adds the Assortment header.
                        ////==============================================================================================================
                        //if (AssortmentAction)
                        //{
                        //    // BEGIN TT#1936 - stodd - cancel allocation/assortment
                        //    //AllocationProfile ap = (AllocationProfile)alp.FindKey(hdrRID);
                        //    //if (ap != null)
                        //    //{
                        //    // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                        //    if (!selectedHdrList.Contains(ap.AsrtRID))
                        //    {
                        //        //selectedHdrKeyList.Add(ho.AsrtRID);
                        //        SelectedHeaderProfile assortmentHeader = new SelectedHeaderProfile(ap.AsrtRID);
                        //        assortmentHeader.HeaderType = eHeaderType.Assortment;
                        //        selectedHdrList.Add(assortmentHeader);
                        //    }
                        //    // END TT#371-MD - stodd -  Velocity Interactive on Assortment
                        //    //}
                        //    // END TT#1936 - stodd - cancel allocation/assortment
                        //}
						// End TT#889 - MD - stodd - need not working
					}
				}
			}
			else
			{
				//foreach (AllocationHeaderProfile ho in _asrtCubeGroup.GetHeaderList().ArrayList)
				foreach (AllocationHeaderProfile ho in _headerList.ArrayList)
				{
					// Begin TT#889 - MD - stodd - need not working
                    //if (actionForHeadersOnly)
                    //{
                        if (ho.HeaderType == eHeaderType.Placeholder || ho.HeaderType == eHeaderType.Assortment)
                        {
                            // Skip these types of headers
                            // Note: If needed, the assortment header was already added.
                        }
                        else
						{
							// Add header
							// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
							//selectedHdrKeyList.Add(ho.Key);
							SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
							selectedHeader.HeaderType = ho.HeaderType;
							selectedHdrList.Add(selectedHeader);
							// END TT#371-MD - stodd -  Velocity Interactive on Assortment
							// Add Assortment header that goes with header, if applicable
                            //if (AssortmentAction)
                            //{
                            //    if (ho.AsrtRID != Include.NoRID)
                            //    {
                            //        // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                            //        if (!selectedHdrList.Contains(ho.AsrtRID))
                            //        {
                            //            //selectedHdrKeyList.Add(ho.AsrtRID);
                            //            SelectedHeaderProfile assortmentHeader = new SelectedHeaderProfile(ho.AsrtRID);
                            //            assortmentHeader.HeaderType = eHeaderType.Assortment;
                            //            selectedHdrList.Add(assortmentHeader);
                            //        }
                            //        // END TT#371-MD - stodd -  Velocity Interactive on Assortment
                            //    }
                            //}
						}
                    //}
                    //else
                    //{
                    //    // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                    //    //foreach (int hdrRID in _selectedHeaderKeyList)
                    //    foreach (SelectedHeaderProfile shp in _selectedHeaderList)
                    //    {
                    //        //if (ho.Key == hdrRID)
                    //        if (ho.Key == shp.Key)
                    //        {
                    //            // END TT#371-MD - stodd -  Velocity Interactive on Assortment
                    //            // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                    //            //selectedHdrKeyList.Add(ho.Key);
                    //            SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                    //            selectedHeader.HeaderType = ho.HeaderType;
                    //            selectedHdrList.Add(selectedHeader);

                    //            if (ho.AsrtRID != Include.NoRID)
                    //            {
                    //                if (!selectedHdrList.Contains(ho.AsrtRID))
                    //                {
                    //                    SelectedHeaderProfile assortmentHeader = new SelectedHeaderProfile(ho.AsrtRID);
                    //                    assortmentHeader.HeaderType = eHeaderType.Assortment;
                    //                    selectedHdrList.Add(assortmentHeader);
                    //                }
                    //            }
                    //            // END TT#371-MD - stodd -  Velocity Interactive on Assortment
                    //        }
                    //    }
                    //}
				}
			}

			// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
			// Only crreate a new Transaction profile list, if we need it.
			if (createTransProfileList)
			{
                //DetermineMasterProfileList();
				TransactionProfileList_Load(selectedHdrList, aTrans, AssortmentAction);
			}
			// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

			return selectedHdrList;
			// End TT#889 - MD - stodd - need not working
		}
		// Begin TT#889 - MD - stodd - need not working
        /// <summary>
        /// Loads the selected hdr list into the transactions allocation master profile list (and sub total profile)
        /// </summary>
        /// <param name="aSelectedHeaderList"></param>
        /// <param name="aTrans"></param>
        /// <param name="isAssortmentAction"></param>
        public void TransactionProfileList_Load(SelectedHeaderList aSelectedHeaderList, ApplicationSessionTransaction aTrans, bool isAssortmentAction)
        {
            TransactionProfileList_RemoveAll(aTrans);
            AllocationProfileList apl = (AllocationProfileList)aTrans.GetMasterProfileList(eProfileType.AssortmentMember);
            foreach (AllocationProfile ap in apl.ArrayList)
            {
                // For GA, any header but a placeholder is fine
                if (ap.HeaderType != eHeaderType.Placeholder)
                {
                    if (aSelectedHeaderList.Contains(ap.Key))
                    {
                        aTrans.AddAllocationProfile(ap);
                        aTrans.AddAllocationProfileToGrandTotal(ap);
                    }
                }
            }

            AllocationProfileList headerList = (AllocationProfileList)aTrans.GetMasterProfileList(eProfileType.Allocation);
            AllocationSubtotalProfile grandTotal = aTrans.GetAllocationGrandTotalProfile();

        }

        /// <summary>
        /// Removes all headers from the transactions allocation master profile list (and sub total profile)
        /// </summary>
        /// <param name="aTrans"></param>
        public void TransactionProfileList_RemoveAll(ApplicationSessionTransaction aTrans)
        {
            // begin TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
            //AllocationProfileList apl = (AllocationProfileList)aTrans.GetMasterProfileList(eProfileType.Allocation);
            //foreach (AllocationProfile ap in apl.ArrayList)
            //{
            //    aTrans.RemoveAllocationProfileFromSubtotal(ap, aTrans.GrandTotalName);
            //}
            aTrans.GetAllocationGrandTotalProfile().RemoveAllSubtotalMembers();            
            // end TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
            aTrans.NewAllocationMasterProfileList();
        }
		// End TT#889 - MD - stodd - need not working

		/// <summary>
		/// Determines if the action allows specific headers to be selected and processed.
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		private bool IsActionSelectable(int action)
		{
			bool isSelectable = false;
			if (Enum.IsDefined(typeof(eAssortmentSelectableActionType), action))
			{
				isSelectable = true;
			}
            // Begin TT#1026 - md - stodd - GA with Grp min-max & alloc min-max & Inv Basis = color.  On header clr 005 the Inv Basis min-max is not honored. On header clr 270 allocated below the min of 12.
            // Is a method
            else if (Enum.IsDefined(typeof(eMethodTypeUI), action))
            {
                isSelectable = true;
            }
            // End TT#1026 - md - stodd - GA with Grp min-max & alloc min-max & Inv Basis = color.  On header clr 005 the Inv Basis min-max is not honored. On header clr 270 allocated below the min of 12.

			return isSelectable;
		}

		/// <summary>
		/// Determines if the action can only be processed on allocation headers.
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		private bool IsActionForHeadersOnly(int action)
		{
			bool isHeaderOnly = false;
			if (Enum.IsDefined(typeof(eAssortmentAllocHeaderOnlyActionType), action))
			{
				isHeaderOnly = true;
			}
			return isHeaderOnly;
		}

		private bool IsAssortmentAction(int action)
		{
			bool isAssortmentAction = false;
			if (Enum.IsDefined(typeof(eAssortmentActionType), action))
			{
				isAssortmentAction = true;
			}
			return isAssortmentAction;
		}
		// End TT#1228 - stodd - selectable headers

        // begin TT#488 - MD - Jellis - Group Allocation (appears to be obsolete code)
        ///// <summary>
        ///// Return a new transaction which is created on the application server and load the selected headers
        ///// into its allocation profile list
        ///// </summary>
        ///// <returns></returns>
        //private ApplicationSessionTransaction NewTransForProcessingHeaders(List<int> allocHdrKeyList)
        //{
        //    ApplicationSessionTransaction newTrans = _sab.ApplicationServerSession.CreateTransaction();
        //    //newTrans.AllocationWorkspaceExplorer = this;
        //    if (allocHdrKeyList.Count > 0)
        //    {
        //        newTrans.NewAllocationMasterProfileList();

        //        int[] selectedHeaderArray = allocHdrKeyList.ToArray();

        //        // load the selected headers in the Application session transaction
        //        newTrans.LoadHeaders(selectedHeaderArray);
        //    }
        //    return newTrans;
        //}
        // end TT#488 - MD - Jellis - Group Allocation (appears to be obsolete code)

		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//private bool OKToProcessAllocation(eAllocationActionType aAction, List<int> allocHdrKeyList)
		private bool OKToProcessAllocation(eAllocationActionType aAction, SelectedHeaderList allocHdrList)
		// END TT#371-MD - stodd -  Velocity Interactive on Assortment
		{
			string errorMessage = string.Empty;
			string errorParm = string.Empty;
			bool okToProcess = true;
			//UltraGridRow selRow = null;
			// BEGIN TT#488-MD - STodd - Group Allocation 
			//AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
			AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.AssortmentMember);
			// END TT#488-MD - STodd - Group Allocation 
			// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
			//foreach (int key in allocHdrKeyList)
			foreach (SelectedHeaderProfile shp in allocHdrList)
			// END TT#371-MD - stodd -  Velocity Interactive on Assortment
			{
				// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
				//AllocationProfile ap = (AllocationProfile)alp.FindKey(key);
				AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
				// END TT#371-MD - stodd -  Velocity Interactive on Assortment

				// Begin #1228 - stodd
				//if (ap.HeaderType == eHeaderType.Assortment)
				//    continue;
				// End #1228 

				switch (ap.HeaderAllocationStatus)
				{
					case eHeaderAllocationStatus.ReceivedOutOfBalance:
						if (aAction != eAllocationActionType.BackoutAllocation)  // allow backout allocation when recv'd out of balance
						{
							okToProcess = false;
						}
						break;
					case eHeaderAllocationStatus.ReleaseApproved:
						if (aAction != eAllocationActionType.Reset &&
							aAction != eAllocationActionType.Release)
						{
							okToProcess = false;
						}
						break;
					case eHeaderAllocationStatus.Released:
						if (aAction != eAllocationActionType.Reset)
						{
							okToProcess = false;
						}
						break;
					default:
						if (aAction == eAllocationActionType.Reset)
						{
							okToProcess = false;
						}
						else if (aAction == eAllocationActionType.ChargeIntransit)
						{
							if (ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllInBalance
								&& ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllocatedInBalance)
								okToProcess = false;
						}
						break;
				}
				if (!okToProcess)
				{
					errorMessage = string.Format
						(MIDText.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction),
						MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));

					MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					okToProcess = false;
					break;
				}
				if (okToProcess)
				{
					if (!_sab.ClientServerSession.GlobalOptions.IsReleaseable(ap.HeaderType)
						&& aAction == eAllocationActionType.Release)
					{
						if (ap.IsDummy)
						{
							errorParm = MIDText.GetTextOnly((int)eHeaderType.Dummy) + " "
								+ MIDText.GetTextOnly(eMIDTextCode.lbl_Header);
						}
						else
						{
							errorParm = MIDText.GetTextOnly((int)ap.HeaderType);
						}
						errorMessage = string.Format
							(MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed),
							errorParm);
						MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						okToProcess = false;
						break;
					}
				}
			}
			if (okToProcess)
			{
				if (aAction == eAllocationActionType.BackoutAllocation
					|| aAction == eAllocationActionType.BackoutSizeAllocation
					|| aAction == eAllocationActionType.BackoutSizeIntransit
					|| aAction == eAllocationActionType.BackoutStyleIntransit
					|| aAction == eAllocationActionType.Reset)
				{
					errorMessage = string.Format(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionWarning),
						MIDText.GetTextOnly((int)aAction));

					DialogResult diagResult = MessageBox.Show(errorMessage, this.Text,
						MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					if (diagResult == System.Windows.Forms.DialogResult.No)
						okToProcess = false;
				}
			}

			// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
			if (okToProcess)
			{
				okToProcess = VerifySecurity(allocHdrList);
			}
			// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.

			return okToProcess;
		}

		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//private bool OKToProcessAssortment(eAssortmentActionType aAction, List<int> allocHdrKeyList)
		private bool OKToProcessAssortment(eAssortmentActionType aAction, SelectedHeaderList selectedHdrList)
		// END TT#371-MD - stodd -  Velocity Interactive on Assortment
		{
			string errorMessage = string.Empty;
			string errorParm = string.Empty;
			bool okToProcess = true;

			// Begin TT#219 - MD - DOConnell - Spread Average not getting expected results
			// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
			if (selectedHdrList.Count == 0 && aAction != eAssortmentActionType.CreatePlaceholders)
			// END TT#371-MD - stodd -  Velocity Interactive on Assortment
			{

				errorMessage = MIDText.GetText(eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected);
				SAB.ClientServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Warning,
					eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected,
					errorMessage,
					"Assortment View");
				MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				okToProcess = false;
			}
			// End TT#219 - MD - DOConnell - Spread Average not getting expected results
			return okToProcess;
		}

		#endregion

		// Begin TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces
        private void ReloadUpdatedHeadersInAllocationWorkspace()
		{
			int[] hdrIdList;
			try
			{
				hdrIdList = new int[_headerList.Count];
				int i = 0;
				foreach (AllocationHeaderProfile ahp in _headerList)
				{
					hdrIdList[i] = Convert.ToInt32(ahp.Key, CultureInfo.CurrentUICulture);
					i++;
				}
                ReloadUpdatedHeadersInAllocationWorkspace(hdrIdList);
                //if (_eab.AllocationWorkspaceExplorer != null)
                //{
                //    _eab.AllocationWorkspaceExplorer.ReloadUpdatedHeaders(hdrIdList);
                //}

			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

		}

        private void ReloadUpdatedHeadersInAllocationWorkspace(int[] hdrIdList)
        {
            try
            {
                if (_eab.AllocationWorkspaceExplorer != null)
                {
                    _eab.AllocationWorkspaceExplorer.ReloadUpdatedHeaders(hdrIdList);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        //private void ReloadUpdatedHeadersInAssortmentWorkspace()
        //{
        //    AllocationProfileList apl = _transaction.GetAssortmentMemberProfileList();
        //    //return (AllocationProfile)apl.FindKey(aHeaderRID);
        //    ArrayList asrtArray = new ArrayList();
        //    foreach (AllocationProfile ap in apl.ArrayList)
        //    {
        //        if (ap.HeaderType == eHeaderType.Assortment)
        //        {
        //            asrtArray.Add(ap.Key);
        //        }
        //        int[] asrtList = new int[asrtArray.Count];
        //        asrtList = (int[])asrtArray.ToArray(typeof(int));
        //        _eab.AssortmentWorkspaceExplorer.ReloadUpdatedAssortments(asrtList);
        //    }
        //}
		// End TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces

		/// <summary>
		/// This procedure handles the "Apply" button click event from the StyleProperties form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void StylePropertiesOnChanged(object sender, System.EventArgs e)
		{
			try
			{
				//StylePropertiesChanged();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//private void StylePropertiesChanged()
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);

		//        _sab.ClientServerSession.Theme = _frmThemeProperties.CurrentTheme;

		//        StopPageLoadThreads();
		//        DefineStyles(true);
		//        ResizeRow1(false);
		//        ResizeRow4(false);
		//        ResizeRow7(false);
		//        ResizeCol1();
		//        ResizeCol2();
		//        ResizeCol3();
		//        ResizeRow1(true);
		//        ResizeRow4(true);
		//        ResizeRow7(true);
		//        CalcColSplitPosition2(false);
		//        CalcColSplitPosition3(false);
		//        SetColSplitPositions();
		//        CalcRowSplitPosition4(false);
		//        CalcRowSplitPosition12(false);
		//        SetRowSplitPositions();
		//        SetHScrollBar2Parameters();
		//        SetHScrollBar3Parameters();
		//        SetVScrollBar2Parameters();
		//        SetVScrollBar3Parameters();
		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//        g6.Focus();
		//    }
		//}

		//private void g2_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
		//{
		//    PagingGridTag gridTag;
		//    int col;

		//    try
		//    {
		//        // added to get around Component One problem
		//        if (!((C1FlexGrid)sender).Redraw)
		//        {
		//            return;
		//        }

		//        gridTag = (PagingGridTag)g2.Tag;

		//        if (e.Row < g2.Rows.Fixed)
		//        {
		//            e.DrawCell();
		//        }
		//        else if (gridTag.ShowColBorders && (e.Col == g2.LeftCol || e.Col % gridTag.ColsPerColGroup == 0))
		//        {
		//            col = (int)(e.Col / gridTag.ColsPerColGroup) * gridTag.ColsPerColGroup;
		//            e.Text = (string)g2.GetData(e.Row, col);
		//            e.DrawCell();
		//        }
		//        else if (!gridTag.ShowColBorders && e.Col == g2.LeftCol)
		//        {
		//            e.Text = (string)g2.GetData(e.Row, 0);
		//            e.DrawCell();
		//        }
		//        else
		//        {
		//            e.DrawCell(DrawCellFlags.Background);
		//            e.DrawCell(DrawCellFlags.Border);
		//        }

		//        DrawColBorders((C1FlexGrid)sender, e);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g3_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
		//{
		//    PagingGridTag gridTag;
		//    int col;

		//    try
		//    {
		//        // added to get around Component One problem
		//        if (!((C1FlexGrid)sender).Redraw)
		//        {
		//            return;
		//        }

		//        gridTag = (PagingGridTag)g3.Tag;

		//        if (e.Row < g3.Rows.Fixed)
		//        {
		//            e.DrawCell();
		//        }
		//        else if (gridTag.ShowColBorders && (e.Col == g3.LeftCol || e.Col % gridTag.ColsPerColGroup == 0))
		//        {
		//            col = (int)(e.Col / gridTag.ColsPerColGroup) * gridTag.ColsPerColGroup;
		//            e.Text = (string)g3.GetData(e.Row, col);
		//            e.DrawCell();
		//        }
		//        else if (!gridTag.ShowColBorders && e.Col == g3.LeftCol)
		//        {
		//            e.Text = (string)g3.GetData(e.Row, 0);
		//            e.DrawCell();
		//        }
		//        else
		//        {
		//            e.DrawCell(DrawCellFlags.Background);
		//            e.DrawCell(DrawCellFlags.Border);
		//        }

		//        DrawColBorders((C1FlexGrid)sender, e);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g4_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
		//{
		//    try
		//    {
		//        // added to get around Component One problem
		//        if (!((C1FlexGrid)sender).Redraw)
		//        {
		//            return;
		//        }

		//        e.DrawCell();
		//        DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupRowHeaderDividerBrush, e);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g5_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
		//{
		//    try
		//    {
		//        // added to get around Component One problem
		//        if (!((C1FlexGrid)sender).Redraw)
		//        {
		//            return;
		//        }

		//        e.DrawCell();
		//        DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupDividerBrush, e);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g6_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
		//{
		//    try
		//    {
		//        // added to get around Component One problem
		//        if (!((C1FlexGrid)sender).Redraw)
		//        {
		//            return;
		//        }

		//        e.DrawCell();
		//        DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupDividerBrush, e);
		//        DrawColBorders((C1FlexGrid)sender, e);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g7_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
		//{
		//    try
		//    {
		//        // added to get around Component One problem
		//        if (!((C1FlexGrid)sender).Redraw)
		//        {
		//            return;
		//        }

		//        e.DrawCell();
		//        DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupRowHeaderDividerBrush, e);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g8_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
		//{
		//    try
		//    {
		//        // added to get around Component One problem
		//        if (!((C1FlexGrid)sender).Redraw)
		//        {
		//            return;
		//        }

		//        e.DrawCell();
		//        DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupDividerBrush, e);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g9_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
		//{
		//    try
		//    {
		//        // added to get around Component One problem
		//        if (!((C1FlexGrid)sender).Redraw)
		//        {
		//            return;
		//        }

		//        e.DrawCell();
		//        DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupDividerBrush, e);
		//        DrawColBorders((C1FlexGrid)sender, e);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		private void DrawRowBorders(
			C1FlexGrid aGrid,
			SolidBrush aRowBrush,
			OwnerDrawCellEventArgs e)
		{
			PagingGridTag gridTag;
			C1FlexGrid rowHeaderGrid;
			RowHeaderTag rowHeaderTag;
			Rectangle rectangle;
			Graphics graphics;
			System.Drawing.Printing.Margins margins;
			//Begin Track #4026 - JScott - Chisled view wrong after sorting
			int rowsPerGroup;
			//End Track #4026 - JScott - Chisled view wrong after sorting


			try
			{
				gridTag = (PagingGridTag)aGrid.Tag;
				rowHeaderGrid = gridTag.RowHeaderGrid;

				if (gridTag.ShowRowBorders && e.Row != aGrid.Rows.Count - 1 && rowHeaderGrid != null)
				{
					rowHeaderTag = (RowHeaderTag)rowHeaderGrid.Rows[e.Row].UserData;

					if ((_theme.ViewStyle == StyleEnum.Plain ||
						_theme.ViewStyle == StyleEnum.AlterColors ||
						_theme.ViewStyle == StyleEnum.HighlightName) &&
						_theme.DisplayRowGroupDivider == true)
					{
						graphics = e.Graphics;
						margins = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

						rectangle = e.Bounds;
						rectangle.Y = rectangle.Bottom - margins.Bottom;
						rectangle.Height = margins.Bottom;

						graphics.FillRectangle(aRowBrush, rectangle);
					}
					else if (_theme.ViewStyle == StyleEnum.Chiseled)
					{
						graphics = e.Graphics;
						margins = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

						rectangle = e.Bounds;
						rectangle.Y = rectangle.Bottom - margins.Bottom;
						rectangle.Height = margins.Bottom;

						rowsPerGroup = gridTag.RowsPerRowGroup;

						if (e.Row % (rowsPerGroup * 2) < rowsPerGroup)
						{
							graphics.FillRectangle(_theme.ChiselLowerBrush, rectangle);
						}
						else
						{
							graphics.FillRectangle(_theme.ChiselUpperBrush, rectangle);
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void DrawColBorders(C1FlexGrid aGrid, OwnerDrawCellEventArgs e)
		{
			PagingGridTag gridTag;
			Rectangle rectangle;
			Graphics graphics;
			System.Drawing.Printing.Margins margins;

			try
			{
				gridTag = (PagingGridTag)aGrid.Tag;

				if (gridTag.ShowColBorders && e.Col < aGrid.Cols.Count - 1 && (e.Col + 1) % gridTag.ColsPerColGroup == 0 && _theme.DisplayColumnGroupDivider)
				{
					graphics = e.Graphics;
					margins = new System.Drawing.Printing.Margins(1, _theme.DividerWidth, 1, 1);

					rectangle = e.Bounds;
					rectangle.X = rectangle.Right - margins.Right;
					rectangle.Width = margins.Right;

					graphics.FillRectangle(_theme.ColumnGroupDividerBrush, rectangle);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#endregion

		#region various repetitive events

		#region ToolClick Event

		//override public void utmMain_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		//{
		//    try
		//    {
		//        switch (e.Tool.Key)
		//        {
		//            case "btUndo":
		//                Undo();
		//                break;

		//            case Include.btFind:
		//                Find();
		//                break;

		//            case Include.btExport:
		//                Export();
		//                break;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		private void Undo()
		{
			//try
			//{
			//    Cursor.Current = Cursors.WaitCursor;
			//    StopPageLoadThreads();
			//    _planCubeGroup.UndoLastRecompute();
			//    LoadCurrentPages();
			//    LoadSurroundingPages();
			//    Cursor.Current = Cursors.Default;
			//}
			//catch (Exception exc)
			//{
			//    string message = exc.ToString();
			//    throw;
			//}
			//finally
			//{
			//    Cursor.Current = Cursors.Default;
			//}
		}

		//private void Find()
		//{
		//    QuickFilter quickFilter;
		//    DialogResult diagResult;
		//    StoreProfile storeProf;
		//    ProfileXRef storeSetXRef;
		//    ArrayList totalList;
		//    int storeIdx;
		//    int selectRow;
		//    int selectCol;

		//    try
		//    {
		//        quickFilter = new QuickFilter(eQuickFilterType.Find, 3, "Store:", "Time:", "Variable:");

		//        quickFilter.EnableComboBox(0);
		//        quickFilter.EnableComboBox(1);
		//        quickFilter.EnableComboBox(2);

		//        quickFilter.LoadComboBox(0, _storeProfileList.ArrayList);
		//        quickFilter.LoadComboBox(1, _sortedTimeHeaders);
		//        quickFilter.LoadComboBox(2, _sortedVariableHeaders);

		//        //BEGIN TT#6-MD-VStuart - Single Store Select
		//        quickFilter.LoadComboBoxAutoFill(0, _storeProfileList.ArrayList);
		//        //END TT#6-MD-VStuart - Single Store Select

		//        diagResult = quickFilter.ShowDialog(this);

		//        if (diagResult == DialogResult.OK)
		//        {
		//            selectRow = 0;
		//            selectCol = 0;

		//            if (quickFilter.GetSelectedIndex(0) >= 0)
		//            {
		//                storeProf = (StoreProfile)quickFilter.GetSelectedItem(0);
		//                storeSetXRef = (ProfileXRef)_asrtCubeGroup.GetProfileXRef(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
		//                totalList = storeSetXRef.GetTotalList(storeProf.Key);
		//                cboStoreGroupLevel.SelectedValue = totalList[0];
		//                storeIdx = _workingDetailProfileList.ArrayList.IndexOf(storeProf);
		//                vScrollBar2.Value = System.Math.Min(storeIdx, vScrollBar2.Maximum - vScrollBar2.LargeChange + 1);
		//                ChangeVScrollBar2Value(vScrollBar2.Value, true);

		//                selectRow = ((PagingGridTag)g4.Tag).RowsPerRowGroup * storeIdx;
		//            }

		//            if (quickFilter.GetSelectedIndex(1) >= 0)
		//            {
		//                selectCol += ((PagingGridTag)g3.Tag).ColsPerColGroup * ((RowColProfileHeader)quickFilter.GetSelectedItem(1)).Sequence;
		//            }

		//            if (quickFilter.GetSelectedIndex(2) >= 0)
		//            {
		//                selectCol += ((RowColProfileHeader)quickFilter.GetSelectedItem(2)).Sequence;
		//            }

		//            g6.Select(selectRow, selectCol);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void GetGridRow(ref object[,] aValueArray, ref ExcelGridInfo[,] aFormatArray, int aSheetRow, int aGridRow, int aGrid1Cols, int aGrid2Cols, int aGrid3Cols, C1FlexGrid aGrid1, C1FlexGrid aGrid2, C1FlexGrid aGrid3)
		//{
		//    int i;
		//    int j;

		//    try
		//    {
		//        aValueArray.Initialize();

		//        for (i = 0, j = 0; i < aGrid1Cols; i++, j++)
		//        {
		//            if (aGrid1 != null && aGridRow < aGrid1.Rows.Count)
		//            {
		//                aValueArray[aSheetRow, j] = "'" + aGrid1[aGridRow, i];
		//                CheckFormats(ref aFormatArray, aGrid1[aGridRow, i], aSheetRow, j);
		//            }
		//        }

		//        for (i = 0; i < aGrid2Cols; i++, j++)
		//        {
		//            if (aGrid2 != null && aGridRow < aGrid2.Rows.Count)
		//            {
		//                aValueArray[aSheetRow, j] = "'" + aGrid2[aGridRow, i];
		//                CheckFormats(ref aFormatArray, aGrid2[aGridRow, i], aSheetRow, j);
		//            }
		//        }

		//        for (i = 0; i < aGrid3Cols; i++, j++)
		//        {
		//            if (aGrid3 != null && aGridRow < aGrid3.Rows.Count)
		//            {
		//                aValueArray[aSheetRow, j] = "'" + aGrid3[aGridRow, i];
		//                CheckFormats(ref aFormatArray, aGrid3[aGridRow, i], aSheetRow, j);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		private void CheckFormats(ref ExcelGridInfo[,] aFormatArray, object aGridCell, int aRow, int aCol)
		{
			try
			{
				if (Convert.ToDouble(aGridCell, CultureInfo.CurrentUICulture) < 0)
				{
					if (aFormatArray[aRow, aCol] == null)
					{
						aFormatArray[aRow, aCol] = new ExcelGridInfo();
					}
					aFormatArray[aRow, aCol].Negative = true;
				}
			}
			catch (FormatException)
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private int ConvertToExcelRGB(int aColor)
		{
			int result;

			try
			{
				result = (((aColor & 0x00FF0000) >> 16) | (aColor & 0x0000FF00) | ((aColor & 0x000000FF) << 16));
				return result;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#endregion

		#region Splitters Move Events

		//private void spcHScrollLevel1_SplitterMoved(object sender, SplitterEventArgs e)
		//{
		//    SplitContainer splitter;

		//    try
		//    {
		//        if (!_hSplitMove)
		//        {
		//            _hSplitMove = true;

		//            try
		//            {
		//                splitter = (SplitContainer)sender;

		//                _currRowSplitPosition12 = splitter.Height - splitter.SplitterDistance;

		//                //BEGIN TT#422 - MD - DOConnell - Had Assortment Matrix open and the opened Allocation Workspace explorer, selected the pin and get System.Argument out of range exception
		//                if (spcHHeaderLevel1.Visible && spcHHeaderLevel1.Height > 0)
		//                //if (spcHHeaderLevel1.Visible)
		//                {
		//                    if (spcHHeaderLevel1.SplitterDistance < g1.Rows[g1.Rows.Count - 1].Bottom + panel1.Height)
		//                    {
		//                        spcHHeaderLevel1.SplitterDistance = g1.Rows[g1.Rows.Count - 1].Bottom + spcHHeaderLevel1.Height;
		//                    }

		//                    spcHHeaderLevel1.SplitterDistance = spcHHeaderLevel1.SplitterDistance + panel1.Height;
		//                    spcHHeaderLevel1.SplitterDistance = spcHHeaderLevel1.Height - _currRowSplitPosition12;

		//                    //END TT#422 - MD - DOConnell - Had Assortment Matrix open and the opened Allocation Workspace explorer, selected the pin and get System.Argument out of range exception
		//                }

		//                //BEGIN TT#422 - MD - DOConnell - Had Assortment Matrix open and the opened Allocation Workspace explorer, selected the pin and get System.Argument out of range exception
		//                if (spcHTotalLevel1.Visible && spcHTotalLevel1.Height > 0)
		//                //if (spcHTotalLevel1.Visible)
		//                {
		//                    if (spcHTotalLevel1.SplitterDistance < g1.Rows[g1.Rows.Count - 1].Bottom + panel1.Height)
		//                    {
		//                        spcHTotalLevel1.SplitterDistance = g1.Rows[g1.Rows.Count - 1].Bottom + spcHTotalLevel1.Height;
		//                    }

		//                    spcHTotalLevel1.SplitterDistance = spcHTotalLevel1.SplitterDistance + panel1.Height;
		//                    spcHTotalLevel1.SplitterDistance = spcHTotalLevel1.Height - _currRowSplitPosition12;

		//                    //END TT#422 - MD - DOConnell - Had Assortment Matrix open and the opened Allocation Workspace explorer, selected the pin and get System.Argument out of range exception
		//                }

		//                //BEGIN TT#422 - MD - DOConnell - Had Assortment Matrix open and the opened Allocation Workspace explorer, selected the pin and get System.Argument out of range exception
		//                if (spcHDetailLevel1.Visible && spcHDetailLevel1.Height > 0)
		//                //if (spcHDetailLevel1.Visible)
		//                {
		//                    if (spcHDetailLevel1.SplitterDistance < g1.Rows[g1.Rows.Count - 1].Bottom + panel1.Height)
		//                    {
		//                        spcHDetailLevel1.SplitterDistance = g1.Rows[g1.Rows.Count - 1].Bottom + spcHDetailLevel1.Height;
		//                    }

		//                    spcHDetailLevel1.SplitterDistance = spcHDetailLevel1.SplitterDistance + panel1.Height;
		//                    spcHDetailLevel1.SplitterDistance = spcHDetailLevel1.Height - _currRowSplitPosition12;

		//                    //END TT#422 - MD - DOConnell - Had Assortment Matrix open and the opened Allocation Workspace explorer, selected the pin and get System.Argument out of range exception
		//                }

		//                //BEGIN TT#422 - MD - DOConnell - Had Assortment Matrix open and the opened Allocation Workspace explorer, selected the pin and get System.Argument out of range exception
		//                if (spcHScrollLevel1.Visible && spcHScrollLevel1.Height > 0)
		//                //if (spcHScrollLevel1.Visible)
		//                {

		//                    if (spcHScrollLevel1.SplitterDistance < g1.Rows[g1.Rows.Count - 1].Bottom + panel1.Height)
		//                    {
		//                        spcHScrollLevel1.SplitterDistance = g1.Rows[g1.Rows.Count - 1].Bottom + spcHScrollLevel1.Height;
		//                    }

		//                    spcHScrollLevel1.SplitterDistance = spcHScrollLevel1.SplitterDistance + panel1.Height;
		//                    spcHScrollLevel1.SplitterDistance = spcHScrollLevel1.Height - _currRowSplitPosition12;

		//                    //END TT#422 - MD - DOConnell - Had Assortment Matrix open and the opened Allocation Workspace explorer, selected the pin and get System.Argument out of range exception
		//                }
		//            }
		//            catch (Exception exc)
		//            {
		//                string message = exc.ToString();
		//                throw;
		//            }
		//            finally
		//            {
		//                _hSplitMove = false;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void spcHScrollLevel1_DoubleClick(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        CalcRowSplitPosition12(true);

		//        spcHScrollLevel1.SplitterDistance = spcHScrollLevel1.Height - _currRowSplitPosition12;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void spcHScrollLevel2_SplitterMoved(object sender, SplitterEventArgs e)
		//{
		//    SplitContainer splitter;

		//    try
		//    {
		//        if (!_hSplitMove)
		//        {
		//            _hSplitMove = true;

		//            try
		//            {
		//                splitter = (SplitContainer)sender;

		//                _currRowSplitPosition4 = splitter.SplitterDistance;

		//                if (spcHHeaderLevel2.Visible)
		//                {
		//                    spcHHeaderLevel2.SplitterDistance = _currRowSplitPosition4;
		//                }

		//                if (spcHTotalLevel2.Visible)
		//                {
		//                    spcHTotalLevel2.SplitterDistance = _currRowSplitPosition4;
		//                }

		//                if (spcHDetailLevel2.Visible)
		//                {
		//                    spcHDetailLevel2.SplitterDistance = _currRowSplitPosition4;
		//                }

		//                if (spcHScrollLevel2.Visible)
		//                {
		//                    spcHScrollLevel2.SplitterDistance = _currRowSplitPosition4;
		//                }
		//            }
		//            catch (Exception exc)
		//            {
		//                string message = exc.ToString();
		//                throw;
		//            }
		//            finally
		//            {
		//                _hSplitMove = false;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void spcHScrollLevel2_DoubleClick(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        CalcRowSplitPosition4(true);

		//        spcHScrollLevel2.SplitterDistance = _currRowSplitPosition4;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void spcVLevel1_SplitterMoved(object sender, SplitterEventArgs e)
		//{
		//    SplitContainer splitter;

		//    try
		//    {
		//        splitter = (SplitContainer)sender;

		//        _currColSplitPosition2 = splitter.SplitterDistance;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void spcVLevel1_DoubleClick(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        CalcColSplitPosition2(true);

		//        spcVLevel1.SplitterDistance = _currColSplitPosition2;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void spcVLevel2_SplitterMoved(object sender, SplitterEventArgs e)
		//{
		//    SplitContainer splitter;

		//    try
		//    {
		//        splitter = (SplitContainer)sender;

		//        _currColSplitPosition3 = splitter.SplitterDistance;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void spcVLevel2_DoubleClick(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        CalcColSplitPosition3(true);

		//        spcVLevel2.SplitterDistance = _currColSplitPosition3;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void SplitterMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		//{
		//    try
		//    {
		//        _currSplitterTag = (SplitterTag)((SplitContainer)sender).Tag;

		//        if (_currSplitterTag.Locked)
		//        {
		//            cmiLockSplitter.Checked = true;
		//        }
		//        else
		//        {
		//            cmiLockSplitter.Checked = false;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		#endregion

		#region ScrollBars Scroll Events/Methods

		//private void SetScrollBarPosition(ScrollBar aScrollBar, int aPosition)
		//{
		//    try
		//    {
		//        if (aPosition != aScrollBar.Value)
		//        {
		//            aScrollBar.Value = aPosition;
		//        }

		//        ((ScrollBarValueChanged)aScrollBar.Tag)(aScrollBar.Value, false);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void SetVScrollBar2Parameters()
		//{
		//    try
		//    {
		//        if (((PagingGridTag)g4.Tag).Visible)
		//        {
		//            CalculateRowMaximumScroll(vScrollBar2, g4, out _row2LineList, ((PagingGridTag)g4.Tag).RowsPerScroll);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void SetVScrollBar3Parameters()
		//{
		//    try
		//    {
		//        if (((PagingGridTag)g7.Tag).Visible)
		//        {
		//            CalculateRowMaximumScroll(vScrollBar3, g7, out _row3LineList, ((PagingGridTag)g7.Tag).RowsPerScroll);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void SetHScrollBar2Parameters()
		//{
		//    try
		//    {
		//        if (((PagingGridTag)g2.Tag).Visible)
		//        {
		//            CalculateColMaximumScroll(hScrollBar2, g2, 1);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void SetHScrollBar3Parameters()
		//{
		//    try
		//    {
		//        if (((PagingGridTag)g3.Tag).Visible)
		//        {
		//            CalculateColMaximumScroll(hScrollBar3, g3, 1);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CalculateRowMaximumScroll(ScrollBar aScrollBar, C1FlexGrid aGrid, out SortedList aLineList, int aScrollSize)
		//{
		//    PagingGridTag gridTag;
		//    int fixedRowHeight;
		//    int totalRowSize;
		//    int i, j;
		//    int totalScrollSize;
		//    int newValue;

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;

		//        aLineList = new SortedList();

		//        for (i = 0; i < aGrid.Rows.Count; i++)
		//        {
		//            if (aGrid.Rows[i].Visible)
		//            {
		//                aLineList.Add(i, i);
		//            }
		//        }

		//        fixedRowHeight = 0;

		//        for (i = 0; i < aGrid.Rows.Fixed; i++)
		//        {
		//            fixedRowHeight += aGrid.Rows[i].HeightDisplay;
		//        }

		//        totalRowSize = 0;

		//        for (i = aLineList.Count - 1; totalRowSize < (aGrid.Height - fixedRowHeight) && i >= aGrid.Rows.Fixed; i -= aScrollSize)
		//        {
		//            for (j = 0; j < aScrollSize; j++)
		//            {
		//                if (aGrid.Rows[(int)aLineList.GetByIndex(i - j)].Visible)
		//                {
		//                    totalRowSize += aGrid.Rows[(int)aLineList.GetByIndex(i - j)].HeightDisplay;
		//                }
		//            }
		//        }

		//        i = i - aGrid.Rows.Fixed + 1;

		//        if (totalRowSize > (aGrid.Height - fixedRowHeight))
		//        {
		//            i += aScrollSize;
		//        }

		//        if (i > 0)
		//        {
		//            totalScrollSize = 0;

		//            for (j = 0; j < aScrollSize; j++)
		//            {
		//                if (aGrid.Rows[(int)aLineList.GetByIndex(j)].Visible)
		//                {
		//                    totalScrollSize += aGrid.Rows[(int)aLineList.GetByIndex(j)].HeightDisplay;
		//                }
		//            }

		//            if (totalScrollSize > (aGrid.Height - fixedRowHeight))
		//            {
		//                newValue = -1;

		//                if (gridTag.ScrollType == eScrollType.Group)
		//                {
		//                    newValue = aScrollBar.Value * aScrollSize;
		//                }

		//                gridTag.ScrollType = eScrollType.Line;

		//                aScrollBar.Minimum = 0;
		//                aScrollBar.Maximum = aLineList.Count - (aGrid.BottomRow - aGrid.TopRow) + aScrollSize - 1;
		//                aScrollBar.SmallChange = 1;
		//                aScrollBar.LargeChange = aScrollSize;

		//                if (newValue != -1)
		//                {
		//                    aScrollBar.Value = newValue;
		//                    ((ScrollBarValueChanged)aScrollBar.Tag)(newValue, false);
		//                }
		//            }
		//            else
		//            {
		//                newValue = -1;

		//                if (gridTag.ScrollType == eScrollType.Line)
		//                {
		//                    newValue = aScrollBar.Value / aScrollSize;
		//                }

		//                gridTag.ScrollType = eScrollType.Group;

		//                aScrollBar.Minimum = 0;
		//                aScrollBar.Maximum = (i / aScrollSize) + BIGCHANGE - 1;
		//                aScrollBar.SmallChange = SMALLCHANGE;
		//                aScrollBar.LargeChange = BIGCHANGE;

		//                if (newValue != -1)
		//                {
		//                    aScrollBar.Value = newValue;
		//                    ((ScrollBarValueChanged)aScrollBar.Tag)(newValue, false);
		//                }
		//            }
		//        }
		//        else
		//        {
		//            gridTag.ScrollType = eScrollType.None;

		//            aScrollBar.Minimum = 0;
		//            aScrollBar.Maximum = BIGCHANGE - 1;
		//            aScrollBar.SmallChange = SMALLCHANGE;
		//            aScrollBar.LargeChange = BIGCHANGE;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CalculateColMaximumScroll(ScrollBar aScrollBar, C1FlexGrid aGrid, int aScrollSize)
		//{
		//    int fixedColWidth;
		//    int totalColSize;
		//    int i, j;

		//    try
		//    {
		//        fixedColWidth = 0;

		//        for (i = 0; i < aGrid.Cols.Fixed; i++)
		//        {
		//            fixedColWidth += aGrid.Cols[i].WidthDisplay;
		//        }

		//        totalColSize = 0;

		//        for (i = aGrid.Cols.Count - 1; totalColSize < (aGrid.Width - fixedColWidth) && i >= aGrid.Cols.Fixed; i -= aScrollSize)
		//        {
		//            for (j = 0; j < aScrollSize; j++)
		//            {
		//                totalColSize += aGrid.Cols[i - j].WidthDisplay;
		//            }
		//        }

		//        i = i - aGrid.Cols.Fixed + 1;

		//        if (totalColSize > (aGrid.Width - fixedColWidth))
		//        {
		//            i += aScrollSize;
		//        }

		//        if (i > 0)
		//        {
		//            //return (i + 1) / aScrollSize;
		//            aScrollBar.Minimum = 0;
		//            aScrollBar.Maximum = (i / aScrollSize) + BIGCHANGE - 1;
		//            aScrollBar.SmallChange = SMALLCHANGE;
		//            aScrollBar.LargeChange = BIGCHANGE;
		//        }
		//        else
		//        {
		//            aScrollBar.Minimum = 0;
		//            aScrollBar.Maximum = BIGCHANGE - 1;
		//            aScrollBar.SmallChange = SMALLCHANGE;
		//            aScrollBar.LargeChange = BIGCHANGE;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void vScrollBar2_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		//{
		//    Point mousePos;
		//    PagingGridTag gridTag;

		//    try
		//    {
		//        switch (e.Type)
		//        {
		//            case ScrollEventType.SmallIncrement:
		//            case ScrollEventType.SmallDecrement:
		//            case ScrollEventType.LargeIncrement:
		//            case ScrollEventType.LargeDecrement:
		//            case ScrollEventType.ThumbTrack:

		//                gridTag = (PagingGridTag)g4.Tag;

		//                if (_holdingScroll || e.Type == ScrollEventType.ThumbTrack)
		//                {
		//                    mousePos = this.PointToClient(Control.MousePosition);

		//                    if (gridTag.ScrollType == eScrollType.Group)
		//                    {
		//                        //InitVerticalScrollTextBox(vScrollBar2, mousePos, ((RowHeaderTag)g4.Rows[Math.Min((gridTag.GroupsPerGrid - 1), e.NewValue) * gridTag.UnitsPerScroll].UserData).ScrollDisplay);
		//                        InitVerticalScrollTextBox(vScrollBar2, mousePos, ((RowHeaderTag)g4.Rows[Math.Min((gridTag.RowGroupsPerGrid - 1), (int)_row2LineList.GetByIndex(e.NewValue)) * gridTag.RowsPerScroll].UserData).ScrollDisplay);
		//                    }
		//                    else
		//                    {
		//                        //InitVerticalScrollTextBox(vScrollBar2, mousePos, ((RowHeaderTag)g4.Rows[e.NewValue].UserData).ScrollDisplay);
		//                        InitVerticalScrollTextBox(vScrollBar2, mousePos, ((RowHeaderTag)g4.Rows[(int)_row2LineList.GetByIndex(e.NewValue)].UserData).ScrollDisplay);
		//                    }
		//                }

		//                _holdingScroll = true;

		//                break;

		//            case ScrollEventType.EndScroll:

		//                rtbScrollText.Visible = false;
		//                _holdingScroll = false;
		//                //((ScrollBarValueChanged)vScrollBar2.Tag)(e.NewValue, true);
		//                ((ScrollBarValueChanged)vScrollBar2.Tag)((int)_row2LineList.GetByIndex(e.NewValue), true);
		//                break;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void vScrollBar3_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		//{
		//    Point mousePos;
		//    PagingGridTag gridTag;

		//    try
		//    {
		//        switch (e.Type)
		//        {
		//            case ScrollEventType.SmallIncrement:
		//            case ScrollEventType.SmallDecrement:
		//            case ScrollEventType.LargeIncrement:
		//            case ScrollEventType.LargeDecrement:
		//            case ScrollEventType.ThumbTrack:

		//                gridTag = (PagingGridTag)g7.Tag;

		//                if (_holdingScroll || e.Type == ScrollEventType.ThumbTrack)
		//                {
		//                    mousePos = this.PointToClient(Control.MousePosition);

		//                    if (gridTag.ScrollType == eScrollType.Group)
		//                    {
		//                        //InitVerticalScrollTextBox(vScrollBar3, mousePos, ((RowHeaderTag)g7.Rows[Math.Min((((PagingGridTag)g7.Tag).GroupsPerGrid - 1), e.NewValue) * ((PagingGridTag)g7.Tag).UnitsPerScroll].UserData).ScrollDisplay);
		//                        InitVerticalScrollTextBox(vScrollBar3, mousePos, ((RowHeaderTag)g7.Rows[Math.Min((((PagingGridTag)g7.Tag).RowGroupsPerGrid - 1), (int)_row3LineList.GetByIndex(e.NewValue)) * ((PagingGridTag)g7.Tag).RowsPerScroll].UserData).ScrollDisplay);
		//                    }
		//                    else
		//                    {
		//                        //InitVerticalScrollTextBox(vScrollBar3, mousePos, ((RowHeaderTag)g7.Rows[e.NewValue].UserData).ScrollDisplay);
		//                        InitVerticalScrollTextBox(vScrollBar3, mousePos, ((RowHeaderTag)g7.Rows[(int)_row3LineList.GetByIndex(e.NewValue)].UserData).ScrollDisplay);
		//                    }
		//                }

		//                _holdingScroll = true;

		//                break;

		//            case ScrollEventType.EndScroll:

		//                rtbScrollText.Visible = false;
		//                _holdingScroll = false;
		//                //((ScrollBarValueChanged)vScrollBar3.Tag)(e.NewValue, true);
		//                ((ScrollBarValueChanged)vScrollBar3.Tag)((int)_row3LineList.GetByIndex(e.NewValue), true);
		//                break;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void hScrollBar2_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		//{
		//    Point mousePos;

		//    try
		//    {
		//        switch (e.Type)
		//        {
		//            case ScrollEventType.SmallIncrement:
		//            case ScrollEventType.SmallDecrement:
		//            case ScrollEventType.LargeIncrement:
		//            case ScrollEventType.LargeDecrement:
		//            case ScrollEventType.ThumbTrack:

		//                if (_holdingScroll || e.Type == ScrollEventType.ThumbTrack)
		//                {
		//                    mousePos = this.PointToClient(Control.MousePosition);
		//                    InitHorizontalScrollTextBox(hScrollBar2, mousePos, ((ColumnHeaderTag)g2.Cols[Math.Min(g2.Cols.Count - 1, e.NewValue)].UserData).ScrollDisplay);
		//                }

		//                _holdingScroll = true;

		//                break;

		//            case ScrollEventType.EndScroll:

		//                rtbScrollText.Visible = false;
		//                _holdingScroll = false;
		//                ((ScrollBarValueChanged)hScrollBar2.Tag)(e.NewValue, true);
		//                break;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void hScrollBar3_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		//{
		//    Point mousePos;

		//    try
		//    {
		//        switch (e.Type)
		//        {
		//            case ScrollEventType.SmallIncrement:
		//            case ScrollEventType.SmallDecrement:
		//            case ScrollEventType.LargeIncrement:
		//            case ScrollEventType.LargeDecrement:
		//            case ScrollEventType.ThumbTrack:

		//                if (_holdingScroll || e.Type == ScrollEventType.ThumbTrack)
		//                {
		//                    mousePos = this.PointToClient(Control.MousePosition);
		//                    InitHorizontalScrollTextBox(hScrollBar3, mousePos, ((ColumnHeaderTag)g3.Cols[Math.Min(g3.Cols.Count - 1, e.NewValue)].UserData).ScrollDisplay);
		//                }

		//                _holdingScroll = true;

		//                break;

		//            case ScrollEventType.EndScroll:

		//                rtbScrollText.Visible = false;
		//                _holdingScroll = false;
		//                ((ScrollBarValueChanged)hScrollBar3.Tag)(e.NewValue, true);
		//                break;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void InitVerticalScrollTextBox(ScrollBar aScrollBar, Point aMousePos, string[] aPromptList)
		//{
		//    Size textBoxSize;
		//    Point clientPoint;

		//    try
		//    {
		//        rtbScrollText.Visible = false;

		//        textBoxSize = new Size(0, 0);

		//        foreach (string prompt in aPromptList)
		//        {
		//            lblFindSize.Text = prompt;
		//            textBoxSize.Height += lblFindSize.Height - 3;
		//            textBoxSize.Width = System.Math.Max(lblFindSize.Width - 2, textBoxSize.Width);
		//        }

		//        rtbScrollText.Clear();
		//        rtbScrollText.Size = textBoxSize;

		//        clientPoint = new Point(this.Right - aScrollBar.Width - rtbScrollText.Width - 20, aMousePos.Y - (rtbScrollText.Height / 2));
		//        clientPoint.Y = Math.Min(clientPoint.Y, this.PointToClient(pnlScrollBars.PointToScreen(new Point(aScrollBar.Right, aScrollBar.Bottom))).Y - rtbScrollText.Height - 20);
		//        clientPoint.Y = Math.Max(clientPoint.Y, this.PointToClient(pnlScrollBars.PointToScreen(new Point(aScrollBar.Left, aScrollBar.Top))).Y + 20);

		//        rtbScrollText.Location = clientPoint;
		//        rtbScrollText.Lines = aPromptList;
		//        rtbScrollText.BringToFront();
		//        rtbScrollText.Visible = true;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void InitHorizontalScrollTextBox(ScrollBar aScrollBar, Point aMousePos, string[] aPromptList)
		//{
		//    Size textBoxSize;
		//    Point clientPoint;

		//    try
		//    {
		//        rtbScrollText.Visible = false;

		//        textBoxSize = new Size(0, 0);

		//        foreach (string prompt in aPromptList)
		//        {
		//            lblFindSize.Text = prompt;
		//            textBoxSize.Height += lblFindSize.Height - 3;
		//            textBoxSize.Width = System.Math.Max(lblFindSize.Width - 2, textBoxSize.Width);
		//        }

		//        rtbScrollText.Clear();
		//        rtbScrollText.Size = textBoxSize;

		//        clientPoint = new Point(aMousePos.X - (rtbScrollText.Width / 2), this.Bottom - aScrollBar.Height - rtbScrollText.Height - 20);
		//        clientPoint.X = Math.Min(clientPoint.X, this.PointToClient(aScrollBar.PointToScreen(new Point(aScrollBar.Right, aScrollBar.Bottom))).X - rtbScrollText.Width - 20);
		//        clientPoint.X = Math.Max(clientPoint.X, this.PointToClient(aScrollBar.PointToScreen(new Point(aScrollBar.Left, aScrollBar.Top))).X + 20);

		//        rtbScrollText.Location = clientPoint;
		//        rtbScrollText.Lines = aPromptList;
		//        rtbScrollText.BringToFront();
		//        rtbScrollText.Visible = true;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ChangeVScrollBar2Value(int aNewValue, bool aLoadValues)
		//{
		//    PagingGridTag gridTag;
		//    int newValue;

		//    try
		//    {
		//        gridTag = (PagingGridTag)g4.Tag;
		//        _isScrolling = true;

		//        switch (gridTag.ScrollType)
		//        {
		//            case eScrollType.Group:
		//                newValue = aNewValue * gridTag.RowsPerScroll;
		//                break;

		//            default:
		//                newValue = aNewValue;
		//                break;
		//        }

		//        newValue += g4.Rows.Fixed;

		//        if (((PagingGridTag)g4.Tag).Visible)
		//        {
		//            g4.TopRow = newValue;
		//        }

		//        if (((PagingGridTag)g5.Tag).Visible)
		//        {
		//            g5.TopRow = newValue;
		//        }

		//        if (((PagingGridTag)g6.Tag).Visible)
		//        {
		//            g6.TopRow = newValue;
		//        }

		//        if (aLoadValues)
		//        {
		//            if (((PagingGridTag)g5.Tag).Visible)
		//            {
		//                LoadCurrentGridPage(g5);
		//            }

		//            if (((PagingGridTag)g6.Tag).Visible)
		//            {
		//                LoadCurrentGridPage(g6);
		//            }
		//        }

		//        gridTag.CurrentScrollPosition = aNewValue;
		//        _isScrolling = false;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ChangeVScrollBar3Value(int aNewValue, bool aLoadValues)
		//{
		//    PagingGridTag gridTag;
		//    int newValue;

		//    try
		//    {
		//        gridTag = (PagingGridTag)g7.Tag;
		//        _isScrolling = true;

		//        switch (gridTag.ScrollType)
		//        {
		//            case eScrollType.Group:
		//                newValue = aNewValue * gridTag.RowsPerScroll;
		//                break;

		//            default:
		//                newValue = aNewValue;
		//                break;
		//        }

		//        newValue += g7.Rows.Fixed;

		//        if (((PagingGridTag)g7.Tag).Visible)
		//        {
		//            g7.TopRow = newValue;
		//        }

		//        if (((PagingGridTag)g8.Tag).Visible)
		//        {
		//            g8.TopRow = newValue;
		//        }

		//        if (((PagingGridTag)g9.Tag).Visible)
		//        {
		//            g9.TopRow = newValue;
		//        }

		//        if (aLoadValues)
		//        {
		//            if (((PagingGridTag)g8.Tag).Visible)
		//            {
		//                LoadCurrentGridPage(g8);
		//            }

		//            if (((PagingGridTag)g9.Tag).Visible)
		//            {
		//                LoadCurrentGridPage(g9);
		//            }
		//        }

		//        gridTag.CurrentScrollPosition = aNewValue;
		//        _isScrolling = false;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ChangeHScrollBar2Value(int aNewValue, bool aLoadValues)
		//{
		//    try
		//    {
		//        _isScrolling = true;

		//        if (((PagingGridTag)g2.Tag).Visible)
		//        {
		//            g2.LeftCol = aNewValue;
		//        }

		//        if (((PagingGridTag)g5.Tag).Visible)
		//        {
		//            g5.LeftCol = aNewValue;
		//        }

		//        if (((PagingGridTag)g8.Tag).Visible)
		//        {
		//            g8.LeftCol = aNewValue;
		//        }

		//        if (aLoadValues)
		//        {
		//            if (((PagingGridTag)g2.Tag).Visible)
		//            {
		//                LoadCurrentGridPage(g2);
		//            }

		//            if (((PagingGridTag)g5.Tag).Visible)
		//            {
		//                LoadCurrentGridPage(g5);
		//            }

		//            if (((PagingGridTag)g8.Tag).Visible)
		//            {
		//                LoadCurrentGridPage(g8);
		//            }
		//        }

		//        ((PagingGridTag)g2.Tag).CurrentScrollPosition = aNewValue;
		//        _isScrolling = false;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ChangeHScrollBar3Value(int aNewValue, bool aLoadValues)
		//{
		//    //int i;

		//    try
		//    {
		//        _isScrolling = true;
		//        if (((PagingGridTag)g3.Tag).Visible)
		//        {
		//            g3.LeftCol = aNewValue;
		//        }

		//        if (((PagingGridTag)g6.Tag).Visible)
		//        {
		//            g6.LeftCol = aNewValue;
		//        }

		//        if (((PagingGridTag)g9.Tag).Visible)
		//        {
		//            g9.LeftCol = aNewValue;
		//        }

		//        if (aLoadValues)
		//        {
		//            if (((PagingGridTag)g3.Tag).Visible)
		//            {
		//                LoadCurrentGridPage(g3);
		//            }

		//            if (((PagingGridTag)g6.Tag).Visible)
		//            {
		//                LoadCurrentGridPage(g6);
		//            }

		//            if (((PagingGridTag)g9.Tag).Visible)
		//            {
		//                LoadCurrentGridPage(g9);
		//            }
		//        }

		//        ((PagingGridTag)g3.Tag).CurrentScrollPosition = aNewValue;
		//        _isScrolling = false;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		#endregion

		#region Grid BeforeScroll Events

		//private void g1_BeforeScroll(object sender, RangeEventArgs e)
		//{
		//    try
		//    {
		//        BeforeScroll(g1, null, g1, null, g1, null, hScrollBar1, e.NewRange);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g2_BeforeScroll(object sender, RangeEventArgs e)
		//{
		//    try
		//    {
		//        if (g2.Editor != null)
		//        {
		//            g2.FinishEditing();
		//        }

		//        g2.Invalidate(e.OldRange.r1, e.OldRange.c1, e.OldRange.r2, e.OldRange.c1);
		//        g2.Invalidate(e.NewRange.r1, e.NewRange.c1, e.NewRange.r2, e.NewRange.c1);

		//        BeforeScroll(g2, null, g5, null, g1, null, hScrollBar2, e.NewRange);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g3_BeforeScroll(object sender, RangeEventArgs e)
		//{
		//    try
		//    {
		//        if (g3.Editor != null)
		//        {
		//            g3.FinishEditing();
		//        }

		//        g3.Invalidate(e.OldRange.r1, e.OldRange.c1, e.OldRange.r2, e.OldRange.c1);
		//        g3.Invalidate(e.NewRange.r1, e.NewRange.c1, e.NewRange.r2, e.NewRange.c1);

		//        BeforeScroll(g3, null, g6, null, g1, null, hScrollBar3, e.NewRange);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g4_BeforeScroll(object sender, RangeEventArgs e)
		//{
		//    try
		//    {
		//        BeforeScroll(g4, g6, null, g4, null, vScrollBar2, null, e.NewRange);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g5_BeforeScroll(object sender, RangeEventArgs e)
		//{
		//    try
		//    {
		//        BeforeScroll(g5, g4, g2, g4, g2, vScrollBar2, hScrollBar2, e.NewRange);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g6_BeforeScroll(object sender, RangeEventArgs e)
		//{
		//    try
		//    {
		//        BeforeScroll(g6, g4, g3, g4, g3, vScrollBar2, hScrollBar3, e.NewRange);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g7_BeforeScroll(object sender, RangeEventArgs e)
		//{
		//    try
		//    {
		//        BeforeScroll(g7, g9, null, g7, null, vScrollBar3, null, e.NewRange);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g8_BeforeScroll(object sender, RangeEventArgs e)
		//{
		//    try
		//    {
		//        BeforeScroll(g8, g7, g2, g7, g2, vScrollBar3, hScrollBar2, e.NewRange);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g9_BeforeScroll(object sender, RangeEventArgs e)
		//{
		//    try
		//    {
		//        BeforeScroll(g9, g7, g3, g7, g3, vScrollBar3, hScrollBar3, e.NewRange);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void BeforeScroll(
		//    C1FlexGrid aGrid,
		//    C1FlexGrid aRowCompGrid,
		//    C1FlexGrid aColCompGrid,
		//    C1FlexGrid aRowHeaderGrid,
		//    C1FlexGrid aColHeaderGrid,
		//    VScrollBar aVScrollBar,
		//    HScrollBar aHScrollBar,
		//    CellRange aNewCellRange)
		//{
		//    try
		//    {
		//        if (!_isScrolling)
		//        {
		//            if (aRowCompGrid != null && aVScrollBar != null)
		//            {
		//                if (aGrid.ScrollPosition.Y < aRowCompGrid.ScrollPosition.Y)
		//                {
		//                    aVScrollBar.Value = Math.Min(CalculateGroupFromDetail(aRowHeaderGrid, aNewCellRange.r1, ((PagingGridTag)aGrid.Tag).RowsPerScroll) + 1, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);
		//                }
		//                else if (aGrid.ScrollPosition.Y > aRowCompGrid.ScrollPosition.Y)
		//                {
		//                    aVScrollBar.Value = CalculateGroupFromDetail(aRowHeaderGrid, aNewCellRange.r1, ((PagingGridTag)aGrid.Tag).RowsPerScroll);
		//                }

		//                ((ScrollBarValueChanged)aVScrollBar.Tag)(aVScrollBar.Value, true);
		//            }
		//            if (aColCompGrid != null && aHScrollBar != null)
		//            {
		//                if (aGrid.ScrollPosition.X < aColCompGrid.ScrollPosition.X)
		//                {
		//                    aHScrollBar.Value = Math.Min(CalculateGroupFromDetail(aColHeaderGrid, aNewCellRange.c1, ((PagingGridTag)aGrid.Tag).ColsPerScroll) + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1);
		//                }
		//                else if (aGrid.ScrollPosition.X > aColCompGrid.ScrollPosition.X)
		//                {
		//                    aHScrollBar.Value = CalculateGroupFromDetail(aColHeaderGrid, aNewCellRange.c1, ((PagingGridTag)aGrid.Tag).ColsPerScroll);
		//                }

		//                ((ScrollBarValueChanged)aHScrollBar.Tag)(aHScrollBar.Value, true);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		#endregion

		#region MouseDown Events

		private void GridMouseDown(object sender, MouseEventArgs e)
		{
			//bool chooserVisible;
			//bool lockVisible;
			//bool freezeVisible;
			C1FlexGrid grid;
			PagingGridTag gridTag;
			Size dragSize;
			//CellTag cellTag;

			//If the user right-clicked in grid, the context menu will pop up.
			//By setting the form variable "_rightClickedFrom" to grid, we know which
			//sets of headings (TOTALS group or individual week groups) to use
			//when cmiColChooser_Click fires.

			try
			{
				grid = (C1FlexGrid)sender;
				gridTag = (PagingGridTag)grid.Tag;

				_dragBoxFromMouseDown = Rectangle.Empty;

				gridTag.MouseDownRow = grid.MouseRow;
				gridTag.MouseDownCol = grid.MouseCol;

				if (e.Button == MouseButtons.Right)
				{
					_rightClickedFrom = grid;
					_isSorting = false;
				}
				else //left mouse button clicked.
				{
					if (gridTag.MouseDownRow < grid.Rows.Fixed && gridTag.MouseDownCol >= grid.Cols.Fixed)
					{
						//If left button is pressed, set _dragState to "dragReady" and store the 
						//initial row and column range. The "ready" state indicates a drag
						//operation is to begin if the mouse moves while the mouse is down.

						//we want to enable drag-drop only if the user click on the actual headings (which is on the 3rd row).

						if (gridTag.MouseDownRow == 0 &&
							(_dragState == DragState.dragNone || _dragState == DragState.dragReady))
						{
							_isSorting = true;
							_mouseDown = true;
							_dragState = DragState.dragReady;
							_dragStartColumn = gridTag.MouseDownCol;
							dragSize = SystemInformation.DragSize;
							_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
						}
						else if (_dragState == DragState.dragResize || gridTag.MouseDownRow != 0)
						{
							_isSorting = false;
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//private void RowHeaderMouseDown(object sender, MouseEventArgs e)
		//{
		//    C1FlexGrid grid;

		//    try
		//    {
		//        grid = (C1FlexGrid)sender;

		//        if (grid.MouseRow >= 0 && grid.MouseRow < grid.Rows.Count &&
		//            grid.MouseCol >= 0 && grid.MouseCol < grid.Cols.Count)
		//        {
		//            ((PagingGridTag)grid.Tag).MouseDownRow = grid.MouseRow;
		//            ((PagingGridTag)grid.Tag).MouseDownCol = grid.MouseCol;

		//            if (e.Button == MouseButtons.Right)
		//            {
		//                _rightClickedFrom = grid;

		//                if (((PagingGridTag)grid.Tag).SelectableColHeaders != null && ((PagingGridTag)grid.Tag).SortedColHeaders != null)
		//                {
		//                    cmiColChooser.Visible = true;
		//                    cmig2g3Seperator1.Visible = true;
		//                }
		//                else
		//                {
		//                    cmiColChooser.Visible = false;
		//                    cmig2g3Seperator1.Visible = false;
		//                }

		//                cmiLockRow.Visible = true;
		//                cmiUnlockRow.Visible = true;
		//                cmig4g7g10Seperator1.Visible = true;
		//            }
		//        }
		//        else
		//        {
		//            cmiLockRow.Visible = false;
		//            cmiUnlockRow.Visible = false;
		//            cmig4g7g10Seperator1.Visible = false;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void DetailMouseDown(object sender, MouseEventArgs e)
		//{
		//    C1FlexGrid grid;
		//    PagingGridTag gridTag;
		//    CellTag cellTag;

		//    try
		//    {
		//        grid = (C1FlexGrid)sender;
		//        gridTag = (PagingGridTag)grid.Tag;

		//        if (grid.MouseRow >= 0 && grid.MouseRow < grid.Rows.Count &&
		//            grid.MouseCol >= 0 && grid.MouseCol < grid.Cols.Count)
		//        {
		//            gridTag.MouseDownRow = grid.MouseRow;
		//            gridTag.MouseDownCol = grid.MouseCol;

		//            if (e.Button == MouseButtons.Right)
		//            {
		//                _rightClickedFrom = grid;
		//                cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[gridTag.MouseDownRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[gridTag.MouseDownCol].UserData).Order];

		//                if (cellTag.ComputationCellFlags.isClosed ||
		//                    ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) ||
		//                    cellTag.ComputationCellFlags.isIneligible ||
		//                    ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) ||
		//                    cellTag.ComputationCellFlags.isProtected ||
		//                    ComputationCellFlags.isHidden(cellTag.ComputationCellFlags) ||
		//                    ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags))
		//                {
		//                    cmiLockCell.Visible = false;
		//                }
		//                else
		//                {
		//                    cmiLockCell.Visible = true;
		//                    cmiLockCell.Checked = ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags);
		//                }
		//                if (_openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel &&
		//                    !_lowLevelStoreReadOnly)
		//                {
		//                    if (_forecastBalanceSecurity.AllowExecute)
		//                    {
		//                        cmiBalance.Visible = true;
		//                    }
		//                    else
		//                    {
		//                        cmiBalance.Visible = false;
		//                    }
		//                    cmiCopyLowToHigh.Visible = true;
		//                }
		//                else
		//                {
		//                    cmiBalance.Visible = false;
		//                    cmiCopyLowToHigh.Visible = false;
		//                }
		//            }
		//        }
		//        else
		//        {
		//            cmiLockCell.Visible = false;
		//            cmiBalance.Visible = false;
		//            cmiCopyLowToHigh.Visible = false;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		#endregion

		#region MouseMove Event

		private void GridMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//C1FlexGrid grid;
			//RowHeaderTag rowHdrTag;
			//int activeKey;
			//int mouseRow;
			//CubeWaferCoordinate basisWaferCoor;
			//CubeWaferCoordinate nodeWaferCoor;
			//System.Windows.Forms.ToolTip toolTip;
			//IDictionaryEnumerator dictEnum;

			//try
			//{
			//    grid = (C1FlexGrid)sender;
			//    mouseRow = grid.MouseRow;
			//    activeKey = -1;

			//    if (mouseRow >= 0 && mouseRow < grid.Rows.Count)
			//    {
			//        rowHdrTag = (RowHeaderTag)grid.Rows[mouseRow].UserData;
			//        basisWaferCoor = rowHdrTag.AssortmentWaferCoorList.FindCoordinateType(eProfileType.Basis);

			//        if (basisWaferCoor != null)
			//        {
			//            nodeWaferCoor = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.HierarchyNode);

			//            if (nodeWaferCoor != null)
			//            {
			//                activeKey = Include.CreateHashKey(nodeWaferCoor.Key, basisWaferCoor.Key);
			//                toolTip = (System.Windows.Forms.ToolTip)_basisToolTipList[activeKey];

			//                if (toolTip != null)
			//                {
			//                    toolTip.Active = true;
			//                }
			//            }
			//        }
			//    }

			//    dictEnum = _basisToolTipList.GetEnumerator();

			//    while (dictEnum.MoveNext())
			//    {
			//        if ((int)dictEnum.Key != activeKey)
			//        {
			//            ((System.Windows.Forms.ToolTip)dictEnum.Value).Active = false;
			//        }
			//    }
			//}
			//catch (Exception exc)
			//{
			//    HandleExceptions(exc);
			//}
		}

		#endregion

		#region Editing Cell Data

		private void GridBeforeEdit(object sender, RowColEventArgs e)
		{
			C1FlexGrid grid;
			PagingGridTag gridTag;
			CellTag cellTag;

			try
			{
				grid = (C1FlexGrid)sender;
				gridTag = (PagingGridTag)grid.Tag;
				cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[e.Row].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[e.Col].UserData).Order];

				//Debug.WriteLine("Grid ID: " + gridTag.GridId + " Row: " + e.Row + " Col: " + e.Col
				//    + " Display Only " + ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags)
				//    + " Is Null " + ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags)
				//    + " Is Blocked " + AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags)
				//    + " Is Fixed " + AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags)
				//    + " Is Hidden " + ComputationCellFlagValues.isHidden(cellTag.ComputationCellFlags)
				//    + " Is Ready Only " + ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags));


				if (ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) ||
					AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags) ||
					AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isHidden(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags))
				{
					e.Cancel = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void g2g3_AfterEdit(object sender, RowColEventArgs e)
		{
			PagingGridTag gridTag;
			C1FlexGrid grid = null;
			C1FlexGrid rowGrid;
			C1FlexGrid colGrid;

			try
			{
				grid = (C1FlexGrid)sender;
				gridTag = (PagingGridTag)grid.Tag;
				rowGrid = gridTag.RowHeaderGrid;
				colGrid = gridTag.ColHeaderGrid;

				if (gridTag.ShowColBorders)
				{
					grid.SetData(e.Row, (int)(e.Col / gridTag.ColsPerColGroup) * gridTag.ColsPerColGroup, grid[e.Row, e.Col]);
				}
				else if (!gridTag.ShowColBorders)
				{
					grid.SetData(e.Row, 0, grid[e.Row, e.Col]);
				}
			}
			catch (FormatException)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_InvalidInput), _windowName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				grid[e.Row, e.Col] = _holdValue;
			}
			catch (CellUnavailableException)
			{
				grid[e.Row, e.Col] = _holdValue;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
				grid[e.Row, e.Col] = _holdValue;
			}
		}


		//private void GridAfterEdit(object sender, RowColEventArgs e)
		//{
		//    PagingGridTag gridTag;
		//    C1FlexGrid grid = null;
		//    C1FlexGrid rowGrid;
		//    C1FlexGrid colGrid;

		//    try
		//    {
		//        grid = (C1FlexGrid)sender;
		//        gridTag = (PagingGridTag)grid.Tag;
		//        rowGrid = gridTag.RowHeaderGrid;
		//        colGrid = gridTag.ColHeaderGrid;

		//        //BEGIN TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically
		//        _asrtCubeGroup.SetCellValue(
		//            _commonWaferCoordinateList,
		//            ((RowHeaderTag)rowGrid.Rows[e.Row].UserData).CubeWaferCoorList,
		//            ((ColumnHeaderTag)colGrid.Cols[e.Col].UserData).CubeWaferCoorList,
		//            System.Convert.ToDouble(grid[e.Row, e.Col], CultureInfo.CurrentUICulture),
		//            //Begin Modification - JScott - Add Scaling Decimals
		//            //1,
		//            //1);
		//            "1",
		//            "1",
		//            _ignoreDisplayOnly);
		//        //END TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically
		//        //End Modification - JScott - Add Scaling Decimals

		//        //RowHeaderTag rowHeaderTag = (RowHeaderTag)rowGrid.Rows[e.Row].UserData;
		//        //ColumnHeaderTag colHeaderTag = (ColumnHeaderTag)colGrid.Cols[e.Col].UserData;
		//        //double quantity = System.Convert.ToDouble(grid[e.Row, e.Col], CultureInfo.CurrentUICulture);
		//        //UpdateContentGridQuantity(e.Row, e.Col, rowHeaderTag, colHeaderTag, quantity);

		//        // Begin TT#1278 - RMatelic - Assortment When selecting Save no hour glass appears and the user cannot tell if somethingis saved or not.  Selecting the X in the right had corner does not give a message asking if the user wants to save or not.
		//        if (_assortReviewAssortmentSecurity.AllowUpdate)
		//        {
		//            ChangePending = true;
		//        }
		//        // End TT#1278 
		//    }
		//    catch (FormatException)
		//    {
		//        MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_InvalidInput), _windowName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		//        grid[e.Row, e.Col] = _holdValue;
		//    }
		//    catch (CellUnavailableException)
		//    {
		//        grid[e.Row, e.Col] = _holdValue;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//        grid[e.Row, e.Col] = _holdValue;
		//    }
		//}

		//BEGIN TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically
		//private void SetCellForReinit(C1FlexGrid grid, int row, int col)
		//{
		//    PagingGridTag gridTag;
		//    C1FlexGrid rowGrid;
		//    C1FlexGrid colGrid;

		//    try
		//    {
		//        gridTag = (PagingGridTag)grid.Tag;
		//        rowGrid = gridTag.RowHeaderGrid;
		//        colGrid = gridTag.ColHeaderGrid;

		//        _asrtCubeGroup.SetIsCellInitialized(
		//            _commonWaferCoordinateList,
		//            ((RowHeaderTag)rowGrid.Rows[row].UserData).CubeWaferCoorList,
		//            ((ColumnHeaderTag)colGrid.Cols[col].UserData).CubeWaferCoorList);

		//        //_asrtCubeGroup.GetCell(
		//        //    _commonWaferCoordinateList,
		//        //    ((RowHeaderTag)rowGrid.Rows[row].UserData).CubeWaferCoorList,
		//        //    ((ColumnHeaderTag)colGrid.Cols[col].UserData).CubeWaferCoorList);

		//        if (_assortReviewAssortmentSecurity.AllowUpdate)
		//        {
		//            ChangePending = true;
		//        }
		//    }
		//    catch (FormatException)
		//    {
		//        MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_InvalidInput), _windowName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		//        grid[row, col] = _holdValue;
		//    }
		//    catch (CellUnavailableException)
		//    {
		//        grid[row, col] = _holdValue;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//        grid[row, col] = _holdValue;
		//    }
		//}
		//END TT#1996 - stodd - Rtl and Cost do not update on Assortment tab dynamically

		//BEGIN TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
		// no longer used
		//private void UpdateContentGridQuantity(int aRow, int aCol, RowHeaderTag aRowHeaderTag, ColumnHeaderTag aColHeaderTag, double aQuantity)
		//{
		//    try
		//    {
		//        CubeWaferCoordinate rowCubeWaferCoord = null;
		//        if (aColHeaderTag.CubeWaferCoorList != null && aColHeaderTag.CubeWaferCoorList.Count >= aCol)
		//        {
		//            CubeWaferCoordinate colCubeWaferCoord = (CubeWaferCoordinate)aColHeaderTag.CubeWaferCoorList[aCol];
		//            if (colCubeWaferCoord.Key == (int)eAssortmentTotalVariables.TotalUnits)
		//            {
		//                if (aRowHeaderTag.CubeWaferCoorList != null)
		//                {
		//                    if (aRowHeaderTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Placeholder) != null)
		//                    {
		//                        rowCubeWaferCoord = (CubeWaferCoordinate)aRowHeaderTag.CubeWaferCoorList.FindCoordinateType(eProfileType.AssortmentQuantityVariable);

		//                        if (rowCubeWaferCoord.Key == _quantityVariables.ValueVariableProfile.Key)
		//                        {
		//                            rowCubeWaferCoord = (CubeWaferCoordinate)aRowHeaderTag.CubeWaferCoorList.FindCoordinateType(eProfileType.PlaceholderHeader);
		//                            UpdateQuantityFromAssortmentGrid(rowCubeWaferCoord.Key, int.MaxValue, int.MaxValue, aQuantity);
		//                        }
		//                    }
		//                    else if (aRowHeaderTag.CubeWaferCoorList.FindCoordinateType(eProfileType.AssortmentSubTotal) != null)
		//                    {
		//                        rowCubeWaferCoord = (CubeWaferCoordinate)aRowHeaderTag.CubeWaferCoorList.FindCoordinateType(eProfileType.AllocationHeader);
		//                        //if (
		//                    }
		//                    else
		//                    {
		//                        rowCubeWaferCoord = (CubeWaferCoordinate)aRowHeaderTag.CubeWaferCoorList.FindCoordinateType(eProfileType.AllocationHeader);
		//                        CubeWaferCoordinate packCubeWaferCoord = (CubeWaferCoordinate)aRowHeaderTag.CubeWaferCoorList.FindCoordinateType(eProfileType.HeaderPack);
		//                        CubeWaferCoordinate colorCubeWaferCoord = (CubeWaferCoordinate)aRowHeaderTag.CubeWaferCoorList.FindCoordinateType(eProfileType.HeaderPackColor);
		//                        UpdateQuantityFromAssortmentGrid(rowCubeWaferCoord.Key, packCubeWaferCoord.Key, colorCubeWaferCoord.Key, aQuantity);
		//                    }
		//                }
		//            }
		//        }
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}
		//END TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder

		//private void GridKeyPress(object sender, KeyPressEventArgs e)
		//{
		//    C1FlexGrid grid;

		//    try
		//    {
		//        grid = (C1FlexGrid)sender;

		//        if (e.KeyChar == 13)
		//        {
		//            try
		//            {
		//                Cursor.Current = Cursors.WaitCursor;
		//                StopPageLoadThreads();
		//                RecomputePlanCubes();
		//                SaveDetailCubeGroup();
		//                LoadCurrentPages();
		//                LoadSurroundingPages();
		//            }
		//            //BEGIN TT#654-MD - stodd - type in % to total in the total section, hit enter twice receive a Nothing to Spread Exception   
		//            catch (NothingToSpreadException ex)
		//            {
		//                string message = ex.Message;
		//                MessageBox.Show(message, _windowName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		//                //string message = MIDText.GetText(eMIDTextCode.msg_ChainWeekLowLvlSpreadFailed);
		//                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, ex.ToString(), this.ToString());
		//            }
		//            //END TT#654-MD - stodd - type in % to total in the total section, hit enter twice receive a Nothing to Spread Exception   
		//            catch (Exception exc)
		//            {
		//                HandleExceptions(exc);
		//            }
		//            finally
		//            {
		//                UpdateOtherViews();	// TT#2 - stodd _assortment
		//                Cursor.Current = Cursors.Default;
		//            }

		//            e.Handled = true;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void GridStartEdit(object sender, RowColEventArgs e)
		//{
		//    C1FlexGrid grid;

		//    try
		//    {
		//        grid = (C1FlexGrid)sender;

		//        _holdValue = grid[e.Row, e.Col];
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void RecomputePlanCubes()
		//{
		//    try
		//    {
		//        if (g5.Editor != null)
		//        {
		//            g5.FinishEditing();
		//        }
		//        if (g6.Editor != null)
		//        {
		//            g6.FinishEditing();
		//        }
		//        if (g8.Editor != null)
		//        {
		//            g8.FinishEditing();
		//        }
		//        if (g9.Editor != null)
		//        {
		//            g9.FinishEditing();
		//        }

		//        _asrtCubeGroup.RecomputeCubes(true);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		#endregion

		#region Miscellaneous

		private void lblAction_Click(object sender, EventArgs e)
		{
			Point mousePos;

			try
			{
				mousePos = System.Windows.Forms.Cursor.Position;
				//cmsActions.Show(lblAction, lblAction.PointToClient(mousePos));
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void AssortmentActionClick(object sender, EventArgs e)
		{
			try
			{
				//lblAction.Text = ((ActionTag)((ToolStripMenuItem)sender).Tag).Text;
				//lblAction.Tag = ((ToolStripMenuItem)sender).Tag;
				//btnProcess.Enabled = true;

				// Begin TT#2 - stodd - bypass need to press process button 
				//int action = ((ActionTag)lblAction.Tag).Key;
                //switch ((eAssortmentActionType)action)
                //{
                //    case eAssortmentActionType.CreatePlaceholders:
                //        btnProcess.PerformClick();
                //        break;
                //    case eAssortmentActionType.SpreadAverage:
                //        btnProcess.PerformClick();
                //        break;
                //}
				// End TT#2 - stodd - bypass need to press process button 

			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void AllocationActionClick(object sender, EventArgs e)
		{
			try
			{
				//lblAction.Text = ((ActionTag)((ToolStripMenuItem)sender).Tag).Text;
				//lblAction.Tag = ((ToolStripMenuItem)sender).Tag;
				//btnProcess.Enabled = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//private void g1_VisibleChanged(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        if (!_formLoading)	// Added TT#2 - stodd - assortment
		//        {
		//            if (((PagingGridTag)g1.Tag).Visible)
		//            {
		//                spcVLevel2.Visible = true;
		//                hScrollBar2.Visible = true;
		//                spcVLevel2.Panel1Collapsed = false;
		//            }
		//            else
		//            {
		//                spcVLevel2.Visible = false;
		//                hScrollBar2.Visible = false;
		//                spcVLevel2.Panel1Collapsed = true;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g7_VisibleChanged(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        if (!_formLoading)	// Added TT#2 - stodd - assortment
		//        {
		//            if (((PagingGridTag)g7.Tag).Visible)
		//            {
		//                spcHHeaderLevel1.Visible = true;
		//                spcHTotalLevel1.Visible = true;
		//                spcHDetailLevel1.Visible = true;
		//                spcHScrollLevel1.Visible = true;
		//                vScrollBar3.Visible = true;
		//            }
		//            else
		//            {
		//                spcHHeaderLevel1.Visible = false;
		//                spcHTotalLevel1.Visible = false;
		//                spcHDetailLevel1.Visible = false;
		//                spcHScrollLevel1.Visible = false;
		//                vScrollBar3.Visible = false;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void cmiLockSplitter_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        _currSplitterTag.Locked = !_currSplitterTag.Locked;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void cmiBasis_Click(object sender, System.EventArgs e)
		//{
		//    ToolStripMenuItem basisCmiItem;

		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        basisCmiItem = (ToolStripMenuItem)sender;

		//        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
		//        {
		//            if (basisHeader.Name == basisCmiItem.Text)
		//            {
		//                basisHeader.IsDisplayed = !basisCmiItem.Checked;
		//                break;
		//            }
		//        }

		//        basisCmiItem.Checked = !basisCmiItem.Checked;

		//        ReformatRowsChanged(true);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//        g6.Focus();
		//    }
		//}
		//private void cmiBalance_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        //frmForecastBalance forecastBalance = new frmForecastBalance(_sab, _openParms, _planCubeGroup);
		//        //forecastBalance.ShowDialog();
		//        //if (forecastBalance.BalanceSuccessful)
		//        //{
		//        //    try
		//        //    {
		//        //        Cursor.Current = Cursors.WaitCursor;
		//        //        StopPageLoadThreads();
		//        //        RecomputePlanCubes();
		//        //        LoadCurrentPages();
		//        //        LoadSurroundingPages();
		//        //    }
		//        //    catch
		//        //    {
		//        //        throw;
		//        //    }
		//        //    finally
		//        //    {
		//        //        Cursor.Current = Cursors.Default;
		//        //    }
		//        //}
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
		{
			bool errorsFound = false;
			if (this.tabControl.SelectedTab.Name != _currentTabPage.Name)
			{
				switch (this.tabControl.SelectedTab.Name)
				{
					//case "tabAssortment":
					//    if (_assortReviewAssortmentSecurity.AccessDenied)
					//    {
					//        errorsFound = true;
					//    }
					//    break;

					//case "tabContent":
					//    if (_assortmentSecurity.AccessDenied)
					//    {
					//        errorsFound = true;
					//    }
					//    break;

					//case "tabProductChar":
					//    if (_assortReviewCharacteristicSecurity.AccessDenied)
					//    {
					//        errorsFound = true;
					//    }
					//    break;
				}
				if (errorsFound)
				{
					string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnauthorizedFunctionAccess);
					MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.tabControl.SelectedTab.Name != _currentTabPage.Name)
				{
					_currentTabPage = this.tabControl.SelectedTab;
					switch (_currentTabPage.Name)
					{
						//case "tabAssortment":
						//    // BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
						//    // BEGIN TT#530-MD - stodd -  Columns not correct on matrix tab
						//    if (GetAssortmentType() == eAssortmentType.PostReceipt && _asrtTypeChanged)
						//    {
						//        int selectedViewValue = (int)cboView.SelectedValue;
						//        BindViewComboBox();
						//        SetSelectedView(selectedViewValue);
						//        _asrtTypeChanged = false;
						//    }
						//    // END TT#530-MD - stodd -  Columns not correct on matrix tab
						//    // END TT#490-MD - stodd -  post-receipts should not show placeholders

						//    this.tabControl.SelectedTab = this.tabAssortment;
						//    // BEGIN TT#1538 - stodd - expand/collapse grid
						//    //this.btnExpandCollapse.Enabled = true;
						//    if (_expandAllAssortment)
						//    {
						//    //	this.btnExpandCollapse.Text = _lblCollapseAll;
						//    }
						//    else
						//    {
						//        //this.btnExpandCollapse.Text = _lblExpandAll;
						//    }
						//    // END TT#1538 - stodd - expand/collapse grid


						//    break;

						case "tabContent":

							this.tabControl.SelectedTab = this.tabContent;
							//BEGIN TT#665-MD - stodd - asst with placeholder and placeholder colors when input avg units in the matrix units not spread evenly to placeholder colors.
							bool builtGrid = LoadContentGrid();
							if (!builtGrid)
							{
								//UpdateContentGrid();
							}
							//END TT#665-MD - stodd - asst with placeholder and placeholder colors when input avg units in the matrix units not spread evenly to placeholder colors.
							//this.btnExpandCollapse.Enabled = true;
							// BEGIN TT#1538 - stodd - expand/collapse grid
							//if (_expandAllContent)
							//{
							//    //this.btnExpandCollapse.Text = _lblCollapseAll;
							//}
							//else
							//{
							//    //this.btnExpandCollapse.Text = _lblExpandAll;
							//}
							// END TT#1538 - stodd - expand/collapse grid
							break;

						//case "tabProductChar":

						//    this.tabControl.SelectedTab = this.tabProductChar;
						//    LoadProductCharGrid();
						//    //this.btnExpandCollapse.Enabled = true;
						//    // BEGIN TT#1538 - stodd - expand/collapse grid
						//    if (_expandAllProductChar)
						//    {
						//        //this.btnExpandCollapse.Text = _lblCollapseAll;
						//    }
						//    else
						//    {
						//        //this.btnExpandCollapse.Text = _lblExpandAll;
						//    }
						//    // END TT#1538 - stodd - expand/collapse grid
						//    break;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		#endregion
		#endregion

		#region G2 and G3 AfterResizeColumn Events

		//private void g2_BeforeAutosizeColumn(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
		//{
		//    try
		//    {
		//        ResizeCol2();
		//        g2_AfterResizeColumn(sender, e);
		//        e.Cancel = true;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g2_AfterResizeColumn(object sender, RowColEventArgs e)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)g5.Tag).Visible)
		//        {
		//            g5.Cols[e.Col].WidthDisplay = g2.Cols[e.Col].WidthDisplay;
		//        }
		//        if (((PagingGridTag)g8.Tag).Visible)
		//        {
		//            g8.Cols[e.Col].WidthDisplay = g2.Cols[e.Col].WidthDisplay;
		//        }

		//        ResizeRow1(false);
		//        CalcRowSplitPosition4(false);
		//        SetRowSplitPositions();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g3_BeforeAutosizeColumn(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
		//{
		//    try
		//    {
		//        ResizeCol3();
		//        g3_AfterResizeColumn(sender, e);
		//        e.Cancel = true;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g3_AfterResizeColumn(object sender, RowColEventArgs e)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)g6.Tag).Visible)
		//        {
		//            g6.Cols[e.Col].WidthDisplay = g3.Cols[e.Col].WidthDisplay;
		//        }
		//        if (((PagingGridTag)g9.Tag).Visible)
		//        {
		//            g9.Cols[e.Col].WidthDisplay = g3.Cols[e.Col].WidthDisplay;
		//        }

		//        ResizeRow1(false);
		//        CalcRowSplitPosition4(false);
		//        SetRowSplitPositions();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		#endregion

		#region Grid Resize Events

		//private void g2_Resize(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        if (!_formLoading)
		//        {
		//            SetHScrollBar2Parameters();
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g3_Resize(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        if (!_formLoading)
		//        {
		//            SetHScrollBar3Parameters();
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g4_Resize(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        if (!_formLoading)
		//        {
		//            SetVScrollBar2Parameters();
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g7_Resize(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        if (!_formLoading)
		//        {
		//            SetVScrollBar3Parameters();
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g4_AfterCollapse(object sender, RowColEventArgs e)
		//{
		//    int i;
		//    CellRange children;

		//    try
		//    {
		//        children = g4.Rows[e.Row].Node.GetCellRange();

		//        for (i = children.r1; i <= children.r2; i++)
		//        {
		//            //Begin TT#2561 - DOConnell - Assortment Grid does not collapse
		//            //g5.Rows[i].Visible = g4.Rows[i].Visible;
		//            //g6.Rows[i].Visible = g4.Rows[i].Visible;
		//            g5.Rows[i].Visible = g4.Rows[i].IsVisible;
		//            g6.Rows[i].Visible = g4.Rows[i].IsVisible;
		//            //End TT#2561 - DOConnell - Assortment Grid does not collapse
		//        }

		//        SetVScrollBar2Parameters();
		//        SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		#endregion

		#region Code related to drag and drop of g2 and g3

		private void BeforeResizeColumn(object sender, RowColEventArgs e)
		{
			//Since we are resizing, not dragging, we need to set the _dragState
			//to "dragResize" so that g2_MouseMove event doesn't process the
			//dragging actions.
			try
			{
				_dragState = DragState.dragResize;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//private void g2_MouseMove(object sender, MouseEventArgs e)
		//{
		//    //if the _dragState is "dragReady", set the _dragState to "started" 
		//    //and begin the drag.

		//    try
		//    {
		//        if (_dragState == DragState.dragReady)
		//        {
		//            if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
		//            {
		//                _isSorting = false;
		//                _mouseDown = false;
		//                _dragState = DragState.dragStarted;
		//                g2.DoDragDrop(sender, DragDropEffects.All);
		//                _dragState = DragState.dragNone;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}
		//private void g3_MouseMove(object sender, MouseEventArgs e)
		//{
		//    //if the _dragState is "dragReady", set the _dragState to "started" 
		//    //and begin the drag.

		//    try
		//    {
		//        if (_dragState == DragState.dragReady)
		//        {
		//            if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
		//            {
		//                _isSorting = false;
		//                _mouseDown = false;
		//                _dragState = DragState.dragStarted;
		//                g3.DoDragDrop(sender, DragDropEffects.All);
		//                _dragState = DragState.dragNone;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g2_MouseUp(object sender, MouseEventArgs e)
		//{
		//    //If the button is released, set the _dragState to "dragNone"
		//    ColumnHeaderTag colHeaderTag;
		//    RowColProfileHeader colHeader;
		//    SortCriteria sc;
		//    SortValue sv;

		//    //sorting a single column

		//    try
		//    {
		//        if (_mouseDown)
		//        {
		//            try
		//            {
		//                _mouseDown = false;
		//                _dragState = DragState.dragNone;
		//                _dragBoxFromMouseDown = Rectangle.Empty;

		//                Cursor.Current = Cursors.WaitCursor;
		//                SetGridRedraws(false);

		//                if (_isSorting)
		//                {
		//                    StopPageLoadThreads();
		//                    GetCellRange(g5, 0, ((PagingGridTag)g2.Tag).MouseDownCol, g5.Rows.Count - 1, ((PagingGridTag)g2.Tag).MouseDownCol, 1);

		//                    colHeaderTag = (ColumnHeaderTag)g2.Cols[((PagingGridTag)g2.Tag).MouseDownCol].UserData;
		//                    colHeader = colHeaderTag.DetailRowColHeader;

		//                    sc = new SortCriteria();
		//                    sc.Column1 = "Time Total";
		//                    sc.Column2 = colHeaderTag.ScrollDisplay[0];
		//                    sc.Column2GridPtr = g5;
		//                    sc.Column2Num = ((PagingGridTag)g2.Tag).MouseDownCol;
		//                    sc.Column2Format = ((ComputationVariableProfile)colHeader.Profile).FormatType;

		//                    if (colHeaderTag.Sort == SortEnum.none || colHeaderTag.Sort == SortEnum.asc)
		//                    {
		//                        sc.SortDirection = SortEnum.desc;
		//                    }
		//                    else if (colHeaderTag.Sort == SortEnum.desc)
		//                    {
		//                        sc.SortDirection = SortEnum.asc;
		//                    }

		//                    sv = new SortValue();
		//                    sv.Row1 = ((RowHeaderTag)g4.Rows[0].UserData).GroupRowColHeader.Name;
		//                    sv.Row2 = ((RowHeaderTag)g4.Rows[0].UserData).DetailRowColHeader.Name;
		//                    sv.Row2Num = 0;
		//                    sv.Row2Format = _quantityVariables.ValueVariableProfile.FormatType;

		//                    _currSortParms = new structSort(sv, sc);
		//                    SortColumns(g4, ref _currSortParms);

		//                    if (_theme.ViewStyle == StyleEnum.AlterColors)
		//                    {
		//                        Cursor.Current = Cursors.WaitCursor;
		//                        if (((PagingGridTag)g4.Tag).Visible)
		//                        {
		//                            Formatg4Grid(false);
		//                        }
		//                        if (((PagingGridTag)g5.Tag).Visible)
		//                        {
		//                            Formatg5Grid(false);
		//                        }
		//                        if (((PagingGridTag)g6.Tag).Visible)
		//                        {
		//                            Formatg6Grid(false);
		//                        }
		//                        Cursor.Current = Cursors.Default;
		//                    }

		//                    LoadCurrentPages();
		//                }
		//            }
		//            catch (Exception exception)
		//            {
		//                HandleExceptions(exception);
		//            }
		//            finally
		//            {
		//                SetGridRedraws(true);
		//                LoadSurroundingPages();
		//                Cursor.Current = Cursors.Default;
		//                g6.Focus();
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g3_MouseUp(object sender, MouseEventArgs e)
		//{
		//    //If the button is released, set the _dragState to "dragNone"+
		//    //ColumnHeaderTag colHeaderTag;
		//    //RowColProfileHeader colHeader;
		//    //SortCriteria sc;
		//    //SortValue sv;

		//    //sorting a single column

		//    try
		//    {
		//        //if (_mouseDown)
		//        //{
		//        //    try
		//        //    {
		//        //        _mouseDown = false;
		//        //        _dragState = DragState.dragNone;
		//        //        _dragBoxFromMouseDown = Rectangle.Empty;

		//        //        Cursor.Current = Cursors.WaitCursor;
		//        //        SetGridRedraws(false);

		//        //        if (_isSorting)
		//        //        {
		//        //            StopPageLoadThreads();
		//        //            GetCellRange(g6, 0, ((PagingGridTag)g3.Tag).MouseDownCol, g6.Rows.Count - 1, ((PagingGridTag)g3.Tag).MouseDownCol, 1);

		//        //            colHeaderTag = (ColumnHeaderTag)g3.Cols[((PagingGridTag)g3.Tag).MouseDownCol].UserData;
		//        //            colHeader = colHeaderTag.DetailRowColHeader;

		//        //            sc = new SortCriteria();
		//        //            sc.Column1 = colHeaderTag.ScrollDisplay[0];
		//        //            sc.Column2 = colHeaderTag.ScrollDisplay[1];
		//        //            sc.Column2GridPtr = g6;
		//        //            sc.Column2Num = ((PagingGridTag)g3.Tag).MouseDownCol;
		//        //            sc.Column2Format = ((ComputationVariableProfile)colHeader.Profile).FormatType;

		//        //            if (colHeaderTag.Sort == SortEnum.none || colHeaderTag.Sort == SortEnum.asc)
		//        //            {
		//        //                sc.SortDirection = SortEnum.desc;
		//        //            }
		//        //            else if (colHeaderTag.Sort == SortEnum.desc)
		//        //            {
		//        //                sc.SortDirection = SortEnum.asc;
		//        //            }

		//        //            sv = new SortValue();
		//        //            sv.Row1 = ((RowHeaderTag)g4.Rows[0].UserData).GroupRowColHeader.Name;
		//        //            sv.Row2 = ((RowHeaderTag)g4.Rows[0].UserData).DetailRowColHeader.Name;
		//        //            sv.Row2Num = 0;
		//        //            sv.Row2Format = _quantityVariables.ValueVariableProfile.FormatType;

		//        //            _currSortParms = new structSort(sv, sc);
		//        //            SortColumns(g4, ref _currSortParms);

		//        //            if (_theme.ViewStyle == StyleEnum.AlterColors)
		//        //            {
		//        //                Cursor.Current = Cursors.WaitCursor;
		//        //                if (((PagingGridTag)g4.Tag).Visible)
		//        //                {
		//        //                    ChangeRowStyles(g4);
		//        //                }
		//        //                if (((PagingGridTag)g5.Tag).Visible)
		//        //                {
		//        //                    ChangeRowStyles(g5);
		//        //                }
		//        //                if (((PagingGridTag)g6.Tag).Visible)
		//        //                {
		//        //                    ChangeRowStyles(g6);
		//        //                }
		//        //                Cursor.Current = Cursors.Default;
		//        //            }

		//        //            LoadCurrentPages();
		//        //        }
		//        //    }
		//        //    catch (Exception exception)
		//        //    {
		//        //        HandleExceptions(exception);
		//        //    }
		//        //    finally
		//        //    {
		//        //        SetGridRedraws(true);
		//        //        LoadSurroundingPages();
		//        //        Cursor.Current = Cursors.Default;
		//        //        g6.Focus();
		//        //    }
		//        //}
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g2_DragEnter(object sender, DragEventArgs e)
		//{
		//    //Check the keystate(state of mouse buttons, among other things). 
		//    //If the left mouse is down, we're okay. Else, set _dragState
		//    //to "none", which will cause the drag operation to be cancelled at the 
		//    //next QueryContinueDrag call. This situation occurs if the user
		//    //releases the mouse button outside of the grid.
		//    try
		//    {
		//        if ((e.KeyState & 0x01) == 1)
		//        {
		//            e.Effect = DragDropEffects.All;
		//        }
		//        else
		//        {
		//            _dragState = DragState.dragNone;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g3_DragEnter(object sender, DragEventArgs e)
		//{
		//    //Check the keystate(state of mouse buttons, among other things). 
		//    //If the left mouse is down, we're okay. Else, set _dragState
		//    //to "none", which will cause the drag operation to be cancelled at the 
		//    //next QueryContinueDrag call. This situation occurs if the user
		//    //releases the mouse button outside of the grid.
		//    try
		//    {
		//        if ((e.KeyState & 0x01) == 1)
		//        {
		//            e.Effect = DragDropEffects.All;
		//        }
		//        else
		//        {
		//            _dragState = DragState.dragNone;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g4_DragEnter(object sender, DragEventArgs e)
		//{
		//    //Check the keystate(state of mouse buttons, among other things). 
		//    //If the left mouse is down, we're okay. Else, set _dragState
		//    //to "none", which will cause the drag operation to be cancelled at the 
		//    //next QueryContinueDrag call. This situation occurs if the user
		//    //releases the mouse button outside of the grid.
		//    try
		//    {
		//        if ((e.KeyState & 0x01) == 1)
		//        {
		//            e.Effect = DragDropEffects.All;
		//        }
		//        else
		//        {
		//            _dragState = DragState.dragNone;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g2_DragOver(object sender, DragEventArgs e)
		//{
		//    //During drag over, if the mouse is dragged to the left- or right-most
		//    //part of the grid, scroll grid to show the columns.

		//    C1FlexGrid grid;
		//    PagingGridTag gridTag;
		//    System.Drawing.Point mousePoint;
		//    System.Drawing.Point clientPoint;

		//    try
		//    {
		//        if (e.Data.GetDataPresent(typeof(C1FlexGrid)))
		//        {
		//            grid = (C1FlexGrid)e.Data.GetData(typeof(C1FlexGrid));
		//            gridTag = (PagingGridTag)grid.Tag;

		//            if (gridTag.GridId == Grid2)
		//            {
		//                mousePoint = new Point(e.X, e.Y);
		//                clientPoint = spcVLevel2.Panel1.PointToClient(mousePoint);

		//                if (clientPoint.X > spcVLevel2.Panel1.Width - 20 && clientPoint.X < spcVLevel2.Panel1.Right)
		//                {
		//                    g2.LeftCol++;
		//                }
		//                else if (clientPoint.X > 0 && clientPoint.X < 20)
		//                {
		//                    g2.LeftCol--;
		//                }

		//                e.Effect = e.AllowedEffect;
		//            }
		//            else
		//            {
		//                e.Effect = DragDropEffects.None;
		//            }
		//        }
		//        else
		//        {
		//            e.Effect = DragDropEffects.None;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g3_DragOver(object sender, DragEventArgs e)
		//{
		//    //During drag over, if the mouse is dragged to the left- or right-most
		//    //part of the grid, scroll grid to show the columns.

		//    C1FlexGrid grid;
		//    PagingGridTag gridTag;
		//    System.Drawing.Point mousePoint;
		//    System.Drawing.Point clientPoint;

		//    try
		//    {
		//        if (e.Data.GetDataPresent(typeof(C1FlexGrid)))
		//        {
		//            grid = (C1FlexGrid)e.Data.GetData(typeof(C1FlexGrid));
		//            gridTag = (PagingGridTag)grid.Tag;

		//            if (gridTag.GridId == Grid3)
		//            {
		//                mousePoint = new Point(e.X, e.Y);
		//                clientPoint = spcVLevel2.Panel2.PointToClient(mousePoint);

		//                if (clientPoint.X > spcVLevel2.Panel2.Width - 20 && clientPoint.X < spcVLevel2.Panel2.Right)
		//                {
		//                    g3.LeftCol++;
		//                }
		//                else if (clientPoint.X > 0 && clientPoint.X < 20)
		//                {
		//                    g3.LeftCol--;
		//                }
		//            }
		//            else
		//            {
		//                e.Effect = DragDropEffects.None;
		//            }
		//        }
		//        else
		//        {
		//            e.Effect = DragDropEffects.None;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g4_DragOver(object sender, DragEventArgs e)
		//{
		//    //During drag over, if the mouse is dragged to the left- or right-most
		//    //part of the grid, scroll grid to show the columns.

		//    SelectedHeaderProfile headerProf;
		//    //System.Drawing.Point mousePoint;
		//    //System.Drawing.Point clientPoint;

		//    try
		//    {
		//        if (e.Data.GetDataPresent(typeof(SelectedHeaderProfile)))
		//        {
		//            headerProf = (SelectedHeaderProfile)e.Data.GetData(typeof(SelectedHeaderProfile));

		//            //if (gridTag.GridId == Grid2)
		//            //{
		//            //    mousePoint = new Point(e.X, e.Y);
		//            //    clientPoint = spcHHeaderLevel2.Panel2.PointToClient(mousePoint);

		//            //    if (clientPoint.X > spcHHeaderLevel2.Panel2.Width - 20 && clientPoint.X < spcHHeaderLevel2.Panel2.Right)
		//            //    {
		//            //        g4.LeftCol++;
		//            //    }
		//            //    else if (clientPoint.X > 0 && clientPoint.X < 20)
		//            //    {
		//            //        g4.LeftCol--;
		//            //    }
		//            //    if (clientPoint.Y > spcHHeaderLevel2.Panel2.Height - 20 && clientPoint.X < spcHHeaderLevel2.Panel2.Bottom)
		//            //    {
		//            //        g4.LeftCol++;
		//            //    }
		//            //    else if (clientPoint.X > 0 && clientPoint.X < 20)
		//            //    {
		//            //        g4.LeftCol--;
		//            //    }
		//            //}
		//            //else
		//            //{
		//            //    e.Effect = DragDropEffects.None;
		//            //}
		//        }
		//        else
		//        {
		//            e.Effect = DragDropEffects.None;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g2_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		//{
		//    //Check to see if the drag should continue. 
		//    //Cancel if:
		//    //(1) the escape key is pressed
		//    //(2) the DragEnter event handler cancels the drag by setting the 
		//    //		_dragState to "none".
		//    //Otherwise, if the mouse is up, perform a drop, or continue if the 
		//    //mouse if down.

		//    try
		//    {
		//        if (e.EscapePressed)
		//        {
		//            e.Action = DragAction.Cancel;
		//        }
		//        else if ((e.KeyState & 0x01) == 0)
		//        {
		//            if (_dragState == DragState.dragNone)
		//            {
		//                e.Action = DragAction.Cancel;
		//            }
		//            else
		//            {
		//                e.Action = DragAction.Drop;
		//            }
		//        }
		//        else
		//        {
		//            e.Action = DragAction.Continue;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g3_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		//{
		//    //Check to see if the drag should continue. 
		//    //Cancel if:
		//    //(1) the escape key is pressed
		//    //(2) the DragEnter event handler cancels the drag by setting the 
		//    //		_dragState to "none".
		//    //Otherwise, if the mouse is up, perform a drop, or continue if the 
		//    //mouse if down.

		//    try
		//    {
		//        if (e.EscapePressed)
		//        {
		//            e.Action = DragAction.Cancel;
		//        }
		//        else if ((e.KeyState & 0x01) == 0)
		//        {
		//            if (_dragState == DragState.dragNone)
		//            {
		//                e.Action = DragAction.Cancel;
		//            }
		//            else
		//            {
		//                e.Action = DragAction.Drop;
		//            }
		//        }
		//        else
		//        {
		//            e.Action = DragAction.Continue;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g4_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		//{
		//    //Check to see if the drag should continue. 
		//    //Cancel if:
		//    //(1) the escape key is pressed
		//    //(2) the DragEnter event handler cancels the drag by setting the 
		//    //		_dragState to "none".
		//    //Otherwise, if the mouse is up, perform a drop, or continue if the 
		//    //mouse if down.

		//    try
		//    {
		//        if (e.EscapePressed)
		//        {
		//            e.Action = DragAction.Cancel;
		//        }
		//        else if ((e.KeyState & 0x01) == 0)
		//        {
		//            if (_dragState == DragState.dragNone)
		//            {
		//                e.Action = DragAction.Cancel;
		//            }
		//            else
		//            {
		//                e.Action = DragAction.Drop;
		//            }
		//        }
		//        else
		//        {
		//            e.Action = DragAction.Continue;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void g2_DragDrop(object sender, DragEventArgs e)
		//{
		//    //This event gets fired once the user releases the mouse button during
		//    //a drag-drop action.
		//    //In this procedure, move the columns in both the headings grid(g2)
		//    //and data grids(g5, g8).

		//    int DragStopColumn = g2.MouseCol; //which column did the user halt.

		//    try
		//    {
		//        if (_dragStartColumn != DragStopColumn)
		//        {
		//            if (g2.Visible)
		//            {
		//                g2.Cols.MoveRange(_dragStartColumn, 1, DragStopColumn);
		//            }
		//            if (g5.Visible)
		//            {
		//                g5.Cols.MoveRange(_dragStartColumn, 1, DragStopColumn);
		//            }
		//            if (g8.Visible)
		//            {
		//                g8.Cols.MoveRange(_dragStartColumn, 1, DragStopColumn);
		//            }
		//        }

		//        //Finally, we want to clear the _dragState. 
		//        //This is an important clean-up step.

		//        _dragState = DragState.dragNone;
		//        _dragBoxFromMouseDown = Rectangle.Empty;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        g6.Focus();
		//    }
		//}

		//private void g3_DragDrop(object sender, DragEventArgs e)
		//{
		//    //This event gets fired once the user releases the mouse button during
		//    //a drag-drop action.
		//    //In this procedure, move the columns in both the headings grid(g3)
		//    //and data grids(g6, g9).

		//    int DragStopColumn; //which column did the user halt.
		//    int NumColsInOneGroup; //the number of columns in one group of data.
		//    int ColNum1stGroupEquiv_Start; //the column that the user began the drag.
		//    int ColNum1stGroupEquiv_Stop; //the column that the user stop the drag (or made the drop).
		//    int i;
		//    int j;
		//    int StartCol;
		//    int StopCol;
		//    int newColNum;

		//    try
		//    {
		//        DragStopColumn = g3.MouseCol;
		//        NumColsInOneGroup = ((PagingGridTag)g3.Tag).ColsPerColGroup;
		//        ColNum1stGroupEquiv_Start = _dragStartColumn % ((PagingGridTag)g3.Tag).ColsPerColGroup;
		//        ColNum1stGroupEquiv_Stop = DragStopColumn % ((PagingGridTag)g3.Tag).ColsPerColGroup;

		//        if (_dragStartColumn != DragStopColumn)
		//        {
		//            for (i = 0; i < ((PagingGridTag)g3.Tag).ColGroupsPerGrid; i++)
		//            {
		//                StartCol = i * NumColsInOneGroup + ColNum1stGroupEquiv_Start;
		//                StopCol = i * NumColsInOneGroup + ColNum1stGroupEquiv_Stop;

		//                if (g3.Visible)
		//                {
		//                    g3.Cols.MoveRange(StartCol, 1, StopCol);
		//                }
		//                if (g6.Visible)
		//                {
		//                    g6.Cols.MoveRange(StartCol, 1, StopCol);
		//                }
		//                if (g9.Visible)
		//                {
		//                    g9.Cols.MoveRange(StartCol, 1, StopCol);
		//                }
		//            }

		//            // Reorder Time Total Cube

		//            if (((PagingGridTag)g2.Tag).Visible)
		//            {
		//                newColNum = 0;

		//                for (i = 0; i < NumColsInOneGroup; i++)
		//                {
		//                    for (j = 0; j < g2.Cols.Count; j++)
		//                    {
		//                        if (((ColumnHeaderTag)g2.Cols[j].UserData).DetailRowColHeader.Profile.Key ==
		//                            ((ColumnHeaderTag)g3.Cols[i].UserData).DetailRowColHeader.Profile.Key)
		//                        {
		//                            if (g2.Visible)
		//                            {
		//                                g2.Cols.MoveRange(j, 1, newColNum);
		//                            }
		//                            if (g5.Visible)
		//                            {
		//                                g5.Cols.MoveRange(j, 1, newColNum);
		//                            }
		//                            if (g8.Visible)
		//                            {
		//                                g8.Cols.MoveRange(j, 1, newColNum);
		//                            }

		//                            newColNum++;
		//                        }
		//                    }
		//                }
		//            }

		//            // Resequence Variable Headers

		//            if (ColNum1stGroupEquiv_Start < ColNum1stGroupEquiv_Stop)
		//            {
		//                newColNum = ColNum1stGroupEquiv_Stop;
		//                for (i = ColNum1stGroupEquiv_Start; i <= ColNum1stGroupEquiv_Stop; i++)
		//                {
		//                    ((RowColProfileHeader)_sortedVariableHeaders.GetByIndex(i)).Sequence = newColNum;
		//                    newColNum = i;
		//                }
		//            }
		//            else
		//            {
		//                newColNum = ColNum1stGroupEquiv_Stop;
		//                for (i = ColNum1stGroupEquiv_Start; i >= ColNum1stGroupEquiv_Stop; i--)
		//                {
		//                    ((RowColProfileHeader)_sortedVariableHeaders.GetByIndex(i)).Sequence = newColNum;
		//                    newColNum = i;
		//                }
		//            }

		//            CreateSortedList(_selectableVariableHeaders, _sortedVariableHeaders);
		//        }

		//        //Finally, we want to clear the _dragState. 
		//        //This is an important clean-up step.

		//        _dragState = DragState.dragNone;
		//        _dragBoxFromMouseDown = Rectangle.Empty;
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        g6.Focus();
		//    }
		//}

		//private void g4_DragDrop(object sender, DragEventArgs e)
		//{
		//    ////This event gets fired once the user releases the mouse button during
		//    ////a drag-drop action.
		//    ////In this procedure, move the columns in both the headings grid(g3)
		//    ////and data grids(g6, g9, g12).

		//    //int DragStopColumn; //which column did the user halt.
		//    //int NumColsInOneGroup; //the number of columns in one group of data.
		//    //int ColNum1stGroupEquiv_Start; //the column that the user began the drag.
		//    //int ColNum1stGroupEquiv_Stop; //the column that the user stop the drag (or made the drop).
		//    //int i;
		//    //int j;
		//    //int StartCol;
		//    //int StopCol;
		//    //int newColNum;

		//    //try
		//    //{
		//    //    DragStopColumn = g3.MouseCol;
		//    //    NumColsInOneGroup = ((PagingGridTag)g3.Tag).ColsPerColGroup;
		//    //    ColNum1stGroupEquiv_Start = _dragStartColumn % ((PagingGridTag)g3.Tag).ColsPerColGroup;
		//    //    ColNum1stGroupEquiv_Stop = DragStopColumn % ((PagingGridTag)g3.Tag).ColsPerColGroup;

		//    //    if (_dragStartColumn != DragStopColumn)
		//    //    {
		//    //        for (i = 0; i < ((PagingGridTag)g3.Tag).ColGroupsPerGrid; i++)
		//    //        {
		//    //            StartCol = i * NumColsInOneGroup + ColNum1stGroupEquiv_Start;
		//    //            StopCol = i * NumColsInOneGroup + ColNum1stGroupEquiv_Stop;

		//    //            if (g3.Visible)
		//    //            {
		//    //                g3.Cols.MoveRange(StartCol, 1, StopCol);
		//    //            }
		//    //            if (g6.Visible)
		//    //            {
		//    //                g6.Cols.MoveRange(StartCol, 1, StopCol);
		//    //            }
		//    //            if (g9.Visible)
		//    //            {
		//    //                g9.Cols.MoveRange(StartCol, 1, StopCol);
		//    //            }
		//    //            //if (g12.Visible)
		//    //            //{
		//    //            //    g12.Cols.MoveRange(StartCol, 1, StopCol);
		//    //            //}
		//    //        }

		//    //        // Reorder Time Total Cube

		//    //        if (((PagingGridTag)g2.Tag).Visible)
		//    //        {
		//    //            newColNum = 0;

		//    //            for (i = 0; i < NumColsInOneGroup; i++)
		//    //            {
		//    //                for (j = 0; j < g2.Cols.Count; j++)
		//    //                {
		//    //                    if (((ColumnHeaderTag)g2.Cols[j].UserData).DetailRowColHeader.Profile.Key ==
		//    //                        ((ColumnHeaderTag)g3.Cols[i].UserData).DetailRowColHeader.Profile.Key)
		//    //                    {
		//    //                        if (g2.Visible)
		//    //                        {
		//    //                            g2.Cols.MoveRange(j, 1, newColNum);
		//    //                        }
		//    //                        if (g5.Visible)
		//    //                        {
		//    //                            g5.Cols.MoveRange(j, 1, newColNum);
		//    //                        }
		//    //                        if (g8.Visible)
		//    //                        {
		//    //                            g8.Cols.MoveRange(j, 1, newColNum);
		//    //                        }
		//    //                        //if (g11.Visible)
		//    //                        //{
		//    //                        //    g11.Cols.MoveRange(j, 1, newColNum);
		//    //                        //}
		//    //                        newColNum++;
		//    //                    }
		//    //                }
		//    //            }
		//    //        }

		//    //        // Resequence Variable Headers

		//    //        if (ColNum1stGroupEquiv_Start < ColNum1stGroupEquiv_Stop)
		//    //        {
		//    //            newColNum = ColNum1stGroupEquiv_Stop;
		//    //            for (i = ColNum1stGroupEquiv_Start; i <= ColNum1stGroupEquiv_Stop; i++)
		//    //            {
		//    //                ((RowColProfileHeader)_sortedVariableHeaders.GetByIndex(i)).Sequence = newColNum;
		//    //                newColNum = i;
		//    //            }
		//    //        }
		//    //        else
		//    //        {
		//    //            newColNum = ColNum1stGroupEquiv_Stop;
		//    //            for (i = ColNum1stGroupEquiv_Start; i >= ColNum1stGroupEquiv_Stop; i--)
		//    //            {
		//    //                ((RowColProfileHeader)_sortedVariableHeaders.GetByIndex(i)).Sequence = newColNum;
		//    //                newColNum = i;
		//    //            }
		//    //        }

		//    //        CreateSortedList(_selectableVariableHeaders, _sortedVariableHeaders);
		//    //    }

		//    //    //Finally, we want to clear the _dragState. 
		//    //    //This is an important clean-up step.

		//    //    _dragState = DragState.dragNone;
		//    //    _dragBoxFromMouseDown = Rectangle.Empty;
		//    //}
		//    //catch (Exception exc)
		//    //{
		//    //    HandleExceptions(exc);
		//    //}
		//    //finally
		//    //{
		//    //    g6.Focus();
		//    //}
		//}

		#endregion

		#region Code related to the "Row/Column Chooser" context menu

		private void cmsGrid_Opening(object sender, CancelEventArgs e)
		{
			PagingGridTag gridTag;
			CellTag cellTag;
			bool chooserVisible;
			bool lockVisible;
			bool freezeVisible;
			bool insertVisible;

			try
			{
				gridTag = (PagingGridTag)_rightClickedFrom.Tag;

				foreach (ToolStripItem menuItem in cmsGrid.Items)
				{
					menuItem.Visible = false;
				}

				chooserVisible = false;
				lockVisible = false;
				freezeVisible = false;
				insertVisible = false;

				if (gridTag.MouseDownRow < _rightClickedFrom.Rows.Fixed)
				{
					if (gridTag.SelectableColHeaders != null && gridTag.SortedColHeaders != null)
					{
						chooserVisible = true;
						cmiColChooser.Visible = true;
					}

					lockVisible = true;
					cmiLockColumn.Visible = true;
					cmiUnlockColumn.Visible = true;
					cmiCascadeLockColumn.Visible = true;
					cmiCascadeUnlockColumn.Visible = true;

					if (_placeholderSelected && _columnGroupedBy == eAllocationAssortmentViewGroupBy.StoreGrade)
					{
						cmiCloseColumn.Visible = true;
						cmiOpenColumn.Visible = true;
					}

					freezeVisible = true;
					cmiFreezeColumn.Visible = true;
					cmiFreezeColumn.Checked = gridTag.HasColsFrozen;
				}
				else if (gridTag.MouseDownCol < _rightClickedFrom.Cols.Fixed)
				{
					if (gridTag.SelectableRowHeaders != null && gridTag.SortedRowHeaders != null)
					{
						chooserVisible = true;
						cmiRowChooser.Visible = true;
					}

					lockVisible = true;
					cmiLockRow.Visible = true;
					cmiUnlockRow.Visible = true;
					cmiCascadeLockRow.Visible = true;
					cmiCascadeUnlockRow.Visible = true;
					cmiCascadeLockSection.Visible = true;
					cmiCascadeUnlockSection.Visible = true;

					if (_placeholderSelected && _columnGroupedBy == eAllocationAssortmentViewGroupBy.StoreGrade)
					{
						cmiCloseRow.Visible = true;
						cmiOpenRow.Visible = true;
					}
				}
				else if (gridTag.MouseDownRow >= _rightClickedFrom.Rows.Fixed && gridTag.MouseDownRow < _rightClickedFrom.Rows.Count &&
					gridTag.MouseDownCol >= _rightClickedFrom.Cols.Fixed && gridTag.MouseDownCol < _rightClickedFrom.Cols.Count)
				{
					cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[gridTag.MouseDownRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[gridTag.MouseDownCol].UserData).Order];

					if (ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) ||
						ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) ||
						AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags) ||
						AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags) ||
						ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags))
					{
						cmiLockCell.Visible = false;
					}
					else
					{
						lockVisible = true;
						cmiLockCell.Visible = true;
						cmiLockCell.Checked = ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags);
						cmiCascadeLockCell.Visible = true;
						cmiCascadeUnlockCell.Visible = true;
					}

					if (_placeholderSelected &&
						_columnGroupedBy == eAllocationAssortmentViewGroupBy.StoreGrade &&
						//gridTag.GridId == Grid6 &&
						!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags))
					{
						// BEGIN TT#1186 - stodd - assortment - don't show "Style Lock" on set only columns
						bool hasGrade = false;
						foreach (CubeWaferCoordinate wafCoor in ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[gridTag.MouseDownCol].UserData).CubeWaferCoorList)
						{
							if (wafCoor.WaferCoordinateType == eProfileType.StoreGrade)
							{
								hasGrade = true;
								break;
							}
						}

						if (hasGrade)
						{
							cmiCloseStyle.Visible = true;
							cmiCloseStyle.Checked = AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags);
						}
						lockVisible = true;
						// END TT#1186 - stodd - assortment
					}
				}

				if (chooserVisible || lockVisible || freezeVisible || insertVisible)
				{
					if (lockVisible && chooserVisible)
					{
						cmiLockSeparator.Visible = true;
					}
					if (freezeVisible && (chooserVisible || lockVisible))
					{
						cmiFreezeSeparator.Visible = true;
					}
					if (insertVisible && (chooserVisible || lockVisible || freezeVisible))
					{
						cmiFreezeSeparator.Visible = true;
					}
				}
				else
				{
					e.Cancel = true;
					return;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmsg4g7g10_Opening(object sender, CancelEventArgs e)
		{
			try
			{
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmsCell_Opening(object sender, CancelEventArgs e)
		{
			try
			{
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmsg1_Opening(object sender, CancelEventArgs e)
		{
			try
			{
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//private void cmiColChooser_Click(object sender, System.EventArgs e)
		//{
		//    PagingGridTag gridTag;
		//    RowColChooser frm;
		//    ChooserPageDefinition[] pageDefs;
		//    int activePage;

		//    try
		//    {
		//        gridTag = (PagingGridTag)_rightClickedFrom.Tag;

		//        if (gridTag.SelectableColHeaders != null && gridTag.SortedColHeaders != null)
		//        {
		//            ArrayList selectableColumnHeaders = new ArrayList();
		//            foreach (RowColProfileHeader rcEntry in _selectableComponentColumnHeaders)
		//            {
		//                // BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
		//                if (rcEntry.Profile.Key == (int)eAssortmentComponentVariables.Placeholder)
		//                {
		//                    if (GetAssortmentType() == eAssortmentType.PostReceipt)
		//                    {
		//                        ((AssortmentComponentVariableProfile)rcEntry.Profile).HideComponent = true;
		//                    }
		//                }
		//                // END TT#490-MD - stodd -  post-receipts should not show placeholders
		//                selectableColumnHeaders.Add(rcEntry);
		//            }

		//            pageDefs = new ChooserPageDefinition[3];
		//            pageDefs[0] = new ChooserPageDefinition("Heading Columns", new RowColChooserComponentPanel(selectableColumnHeaders));
		//            // Begin Track #4868 - JSmith - Variable Groupings
		//            //pageDefs[1] = new ChooserPageDefinition("Total Columns", new RowColChooserOrderPanel(_selectableTotalColumnHeaders, true));
		//            //pageDefs[2] = new ChooserPageDefinition("Detail Columns", new RowColChooserOrderPanel(_selectableDetailColumnHeaders, true));
		//            pageDefs[1] = new ChooserPageDefinition("Total Columns", new RowColChooserOrderPanel(_selectableTotalColumnHeaders, true, null));
		//            pageDefs[2] = new ChooserPageDefinition("Detail Columns", new RowColChooserOrderPanel(_selectableDetailColumnHeaders, true, null));
		//            // End Track #4868

		//            switch (gridTag.GridId)
		//            {
		//                case Grid1:
		//                case Grid4:
		//                case Grid7:

		//                    activePage = 0;
		//                    break;

		//                case Grid2:
		//                case Grid5:
		//                case Grid8:

		//                    activePage = 1;
		//                    break;

		//                default:

		//                    activePage = 2;
		//                    break;
		//            }

		//            // Begin Track #4868 - JSmith - Variable Groupings
		//            //frm = new RowColChooser(pageDefs, "Column Chooser", activePage);
		//            frm = new RowColChooser(pageDefs, "Column Chooser", activePage, null);
		//            // BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
		//            //frm.AssortmentType = GetAssortmentType();
		//            // END TT#490-MD - stodd -  post-receipts should not show placeholders
		//            // End Track #4868

		//            if (frm.ShowDialog() == DialogResult.OK && frm.isAnyPageChanged)
		//            {
		//                try
		//                {
		//                    Cursor.Current = Cursors.WaitCursor;

		//                    CreateSortedList(_selectableComponentColumnHeaders, _sortedComponentColumnHeaders);
		//                    CreateSortedList(_selectableTotalColumnHeaders, _sortedTotalColumnHeaders);
		//                    CreateSortedList(_selectableDetailColumnHeaders, _sortedDetailColumnHeaders);

		//                    SetGridRedraws(false);
		//                    StopPageLoadThreads();

		//                    if (frm.isPageChanged(0))
		//                    {
		//                        ComponentsChanged();
		//                    }

		//                    ReformatColsChanged(true);

		//                    // BEGIN TT#1538 - stodd - expand/collaspe button
		//                    _expandAllAssortment = true;
		//                    //this.btnExpandCollapse.Text = _lblCollapseAll;
		//                    // BEGIN TT#1538 - stodd - expand/collaspe button
		//                }
		//                catch (Exception exc)
		//                {
		//                    HandleExceptions(exc);
		//                }
		//                finally
		//                {
		//                    SetGridRedraws(true);
		//                    LoadSurroundingPages();
		//                    Cursor.Current = Cursors.Default;
		//                    g6.Focus();
		//                }
		//            }

		//            frm.Dispose();
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleExceptions(ex);
		//    }
		//}

		//private void cmiRowChooser_Click(object sender, System.EventArgs e)
		//{
		//    PagingGridTag gridTag;
		//    RowColChooser frm;

		//    try
		//    {
		//        gridTag = (PagingGridTag)_rightClickedFrom.Tag;

		//        if (gridTag.SelectableRowHeaders != null && gridTag.SortedRowHeaders != null)
		//        {
		//            // Begin Track #4868 - JSmith - Variable Groupings
		//            //frm = new RowColChooser(gridTag.SelectableRowHeaders, true, "Quantity Chooser", false);
		//            frm = new RowColChooser(gridTag.SelectableRowHeaders, true, "Quantity Chooser", false, null);
		//            // BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
		//            //frm.AssortmentType = GetAssortmentType();
		//            // END TT#490-MD - stodd -  post-receipts should not show placeholders
		//            // End Track #4868

		//            if (frm.ShowDialog() == DialogResult.OK && frm.isAnyPageChanged)
		//            {
		//                try
		//                {
		//                    Cursor.Current = Cursors.WaitCursor;

		//                    CreateSortedList(gridTag.SelectableRowHeaders, gridTag.SortedRowHeaders);

		//                    SetGridRedraws(false);
		//                    StopPageLoadThreads();
		//                    ReformatRowsChanged(true);
		//                }
		//                catch (Exception exc)
		//                {
		//                    HandleExceptions(exc);
		//                }
		//                finally
		//                {
		//                    SetGridRedraws(true);
		//                    LoadSurroundingPages();
		//                    Cursor.Current = Cursors.Default;
		//                    g6.Focus();
		//                }
		//            }

		//            frm.Dispose();
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleExceptions(ex);
		//    }
		//}

		private void CreateSortedList(ArrayList aSelectableList, SortedList aSortedList)
		{
			//SortedList sortList;
			//IDictionaryEnumerator enumerator;
			//int i, j;
			//int newCols;

			//try
			//{
			//    sortList = new SortedList();
			//    newCols = 0;

			//    for (i = 0; i < aSelectableList.Count; i++)
			//    {
			//        if (((RowColProfileHeader)aSelectableList[i]).IsDisplayed)
			//        {
			//            if (((RowColProfileHeader)aSelectableList[i]).Sequence == -1)
			//            {
			//                newCols++;
			//                ((RowColProfileHeader)aSelectableList[i]).Sequence = newCols * -1;
			//            }
			//            sortList.Add(((RowColProfileHeader)aSelectableList[i]).Sequence, i);
			//        }
			//        else
			//        {
			//            ((RowColProfileHeader)aSelectableList[i]).Sequence = -1;
			//        }
			//    }

			//    enumerator = sortList.GetEnumerator();
			//    j = 0;

			//    while (enumerator.MoveNext())
			//    {
			//        if (Convert.ToInt32(enumerator.Key, CultureInfo.CurrentUICulture) < 0)
			//        {
			//            ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = sortList.Count - newCols + (Convert.ToInt32(enumerator.Key, CultureInfo.CurrentUICulture) * -1) - 1;
			//        }
			//        else
			//        {
			//            ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = j;
			//            j++;
			//        }
			//    }

			//    aSortedList.Clear();

			//    foreach (RowColProfileHeader rowColHeader in aSelectableList)
			//    {
			//        if (rowColHeader.IsDisplayed)
			//        {
			//            aSortedList.Add(rowColHeader.Sequence, rowColHeader);
			//        }
			//    }
			//}
			//catch (Exception exc)
			//{
			//    string message = exc.ToString();
			//    throw;
			//}
			SortedList summaryList;
			SortedList selectedList;
			IDictionaryEnumerator enumerator;
			int i, j;
			int newCols;

			try
			{
				summaryList = new SortedList();
				selectedList = new SortedList();
				newCols = 0;

				// Begin TT#1227 - stodd - sequencing
				//bool placeholderIsSummarized = false;
				//int placeholderSortSeq = -1;
				//bool headerIsSummarized = false;
				//int headerSortSeq = -1;
				//RowColProfileHeader placeholderSeqHeader = null;
				//RowColProfileHeader headerSeqHeader = null;

				//Debug.WriteLine("BEFORE");
				//// Loops through selectable list caching information needed for the next loop
				//for (i = 0; i < aSelectableList.Count; i++)
				//{
				//    Debug.WriteLine(((RowColProfileHeader)aSelectableList[i]).Name + " " + ((RowColProfileHeader)aSelectableList[i]).Sequence
				//        + " " + ((RowColProfileHeader)aSelectableList[i]).IsDisplayed + " " + ((RowColProfileHeader)aSelectableList[i]).IsSummarized);
				//    if (((RowColProfileHeader)aSelectableList[i]).Profile.Key == (int)eAssortmentComponentVariables.Placeholder)
				//    {
				//        placeholderSortSeq = ((RowColProfileHeader)aSelectableList[i]).Sequence;
				//        placeholderIsSummarized = ((RowColProfileHeader)aSelectableList[i]).IsSummarized;
				//    }
				//    if (((RowColProfileHeader)aSelectableList[i]).Profile.Key == (int)eAssortmentComponentVariables.HeaderID)
				//    {
				//        headerSortSeq = ((RowColProfileHeader)aSelectableList[i]).Sequence;
				//        headerIsSummarized = ((RowColProfileHeader)aSelectableList[i]).IsSummarized;

				//    }
				//    if (((RowColProfileHeader)aSelectableList[i]).Profile.Key == (int)eAssortmentComponentVariables.PlaceholderSeq)
				//    {
				//        placeholderSeqHeader = ((RowColProfileHeader)aSelectableList[i]);
				//    }
				//    if (((RowColProfileHeader)aSelectableList[i]).Profile.Key == (int)eAssortmentComponentVariables.HeaderSeq)
				//    {
				//        headerSeqHeader = ((RowColProfileHeader)aSelectableList[i]);
				//    }
				//}

				//Debug.WriteLine("AFTER");
				////==========================================================================================================================
				//// If placeholder is selected as one of the sort criteria, the placeholder seq is the column that needs to be sorted on.
				//// The same is true with headerID. We really want to sort on header seq.
				//// This code sets all the proper switches and resequences the sequence to include the "seq' version of the columns.
				////==========================================================================================================================
				//for (i = 0; i < aSelectableList.Count; i++)
				//{
				//    if (((RowColProfileHeader)aSelectableList[i]).Sequence >= placeholderSortSeq && placeholderSortSeq > -1)
				//    {
				//        ((RowColProfileHeader)aSelectableList[i]).Sequence++;
				//    }
				//    if (((RowColProfileHeader)aSelectableList[i]).Sequence >= headerSortSeq && headerSortSeq > -1)
				//    {
				//        ((RowColProfileHeader)aSelectableList[i]).Sequence++;
				//    }
				//    if (((RowColProfileHeader)aSelectableList[i]).Profile.Key == (int)eAssortmentComponentVariables.PlaceholderSeq)
				//    {
				//        if (placeholderSortSeq > -1)
				//        {
				//            ((RowColProfileHeader)aSelectableList[i]).Sequence = placeholderSortSeq;
				//            ((RowColProfileHeader)aSelectableList[i]).IsDisplayed = true;
				//            ((RowColProfileHeader)aSelectableList[i]).IsSummarized = placeholderIsSummarized;
				//        }
				//    }
				//    if (((RowColProfileHeader)aSelectableList[i]).Profile.Key == (int)eAssortmentComponentVariables.HeaderSeq)
				//    {
				//        if (headerSortSeq > -1)
				//        {
				//            ((RowColProfileHeader)aSelectableList[i]).Sequence = headerSortSeq;
				//            ((RowColProfileHeader)aSelectableList[i]).IsDisplayed = true;
				//            ((RowColProfileHeader)aSelectableList[i]).IsSummarized = headerIsSummarized;
				//        }
				//    }
				//    Debug.WriteLine(((RowColProfileHeader)aSelectableList[i]).Name + " " + ((RowColProfileHeader)aSelectableList[i]).Sequence
				//        + " " + ((RowColProfileHeader)aSelectableList[i]).IsDisplayed + " " + ((RowColProfileHeader)aSelectableList[i]).IsSummarized);
				//}
				//// End TT#1227 - stodd - sequencing

				// BEGIN TT#490-MD - stodd -  post-receipts should show placeholders
				//RowColProfileHeader ph = null;
				//RowColProfileHeader style = null;
				//RowColProfileHeader hdr = null;
				//bool placeholderSelected = false;
				//bool othersSelected = false;
				for (i = 0; i < aSelectableList.Count; i++)
				{
					//if (((RowColProfileHeader)aSelectableList[i]).Profile.Key == (int)eAssortmentComponentVariables.Placeholder)
					//{
					//	ph = (RowColProfileHeader)aSelectableList[i];
					//}
					//if (((RowColProfileHeader)aSelectableList[i]).Profile.Key == (int)eAssortmentComponentVariables.HeaderID)
					//{
					//	hdr = (RowColProfileHeader)aSelectableList[i];
					//}
					//if (((RowColProfileHeader)aSelectableList[i]).Name == "Style/SKU")
					//{
					//	style = (RowColProfileHeader)aSelectableList[i];
					//}

					if (((RowColProfileHeader)aSelectableList[i]).IsDisplayed)
					{
						//if (((RowColProfileHeader)aSelectableList[i]).Profile.Key == (int)eAssortmentComponentVariables.Placeholder)
						//{
						//	placeholderSelected = true;
						//}
						//else
						//{
						//	othersSelected = true;
						//}
						//}
						//}
						// END TT#490-MD - stodd -  post-receipts should show placeholders


						//for (i = 0; i < aSelectableList.Count; i++)
						//{
						//if (((RowColProfileHeader)aSelectableList[i]).IsDisplayed)
						//{
						// BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
						//if (GetAssortmentType() == eAssortmentType.PostReceipt)
						//{
						//if (((RowColProfileHeader)aSelectableList[i]).Profile.Key == (int)eAssortmentComponentVariables.Placeholder)
						//{
						//((RowColProfileHeader)aSelectableList[i]).IsDisplayed = false;
						//((RowColProfileHeader)aSelectableList[i]).Sequence = -1;

						//if (othersSelected)
						//{
						//style.IsDisplayed = true;
						//style.IsSummarized = ((RowColProfileHeader)aSelectableList[i]).IsSummarized;
						//}
						//else
						//{
						//hdr.IsDisplayed = true;
						//hdr.IsSummarized = ((RowColProfileHeader)aSelectableList[i]).IsSummarized;
						//}

						//continue;
						//}
						//}
						// END TT#490-MD - stodd -  post-receipts should not show placeholders

						if (((RowColProfileHeader)aSelectableList[i]).Sequence == -1)
						{
							((RowColProfileHeader)aSelectableList[i]).Sequence = aSelectableList.Count + newCols;
							newCols++;
						}

						if (((RowColProfileHeader)aSelectableList[i]).IsSummarized)
						{
							summaryList.Add(((RowColProfileHeader)aSelectableList[i]).Sequence, i);
						}
						else
						{
							selectedList.Add(((RowColProfileHeader)aSelectableList[i]).Sequence, i);
						}
					}
					else
					{
						((RowColProfileHeader)aSelectableList[i]).Sequence = -1;
					}
				}

				j = 0;
				enumerator = summaryList.GetEnumerator();

				while (enumerator.MoveNext())
				{
					((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = j;
					j++;
				}

				enumerator = selectedList.GetEnumerator();

				while (enumerator.MoveNext())
				{
					((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = j;
					j++;
				}

				aSortedList.Clear();

				foreach (RowColProfileHeader rowColHeader in aSelectableList)
				{
					//if (rowColHeader.IsDisplayed || rowColHeader.Profile.Key == (int)eAssortmentComponentVariables.PlaceholderSeq
					//    || rowColHeader.Profile.Key == (int)eAssortmentComponentVariables.HeaderSeq)
					if (rowColHeader.IsDisplayed)
					{
						// Begin TT#1227 - stodd - sequencing *REMOVED for TT#1322*
						//if (rowColHeader.Profile.Key == (int)eAssortmentComponentVariables.PlaceholderSeq
						//    || rowColHeader.Profile.Key == (int)eAssortmentComponentVariables.HeaderSeq)
						//{
						//    //rowColHeader.IsDisplayed = false;
						//    //rowColHeader.IsSelectable = false;
						//}
						aSortedList.Add(rowColHeader.Sequence, rowColHeader);
						// End TT#1227 - stodd - sequencing
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}


		#endregion

		#region Locking/Unlocking Cell, Column, Row, and Sheet

		//private void cmiLockCell_Click(object sender, System.EventArgs e)
		//{
		//    PagingGridTag gridTag;

		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        gridTag = (PagingGridTag)_rightClickedFrom.Tag;
		//        LockUnlockGridCell(_rightClickedFrom, !cmiLockCell.Checked);
		//        // BEGIN TT#2 - stodd - assortment multiselection				
		//        GetCellRange(_rightClickedFrom, _rightClickedFrom.Selection.TopRow, _rightClickedFrom.Selection.LeftCol, _rightClickedFrom.Selection.BottomRow, _rightClickedFrom.Selection.RightCol, 1);
		//        //GetCellRange(_rightClickedFrom, gridTag.MouseDownRow, gridTag.MouseDownCol, gridTag.MouseDownRow, gridTag.MouseDownCol, 1);
		//        // END TT#2 - stodd - assortment multiselection

		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiLockColumn_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        LockUnlockColumn(_rightClickedFrom, true);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiUnlockColumn_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        LockUnlockColumn(_rightClickedFrom, false);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiLockRow_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        LockUnlockRow(_rightClickedFrom, true);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiUnlockRow_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        LockUnlockRow(_rightClickedFrom, false);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiLockSheet_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        LockUnlockSheet(true);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiUnlockSheet_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        LockUnlockSheet(false);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void LockUnlockColumn(C1FlexGrid aGrid, bool LockThisColumn)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)aGrid.Tag).GridId == Grid2)
		//        {
		//            //lock and unlock the column only if the column is lockable.
		//            LockUnlockGridColumn(g5, ((PagingGridTag)g2.Tag).MouseDownCol, LockThisColumn);
		//            LockUnlockGridColumn(g8, ((PagingGridTag)g2.Tag).MouseDownCol, LockThisColumn);
		//        }
		//        else if (((PagingGridTag)aGrid.Tag).GridId == Grid3)
		//        {
		//            LockUnlockGridColumn(g6, ((PagingGridTag)g3.Tag).MouseDownCol, LockThisColumn);
		//            LockUnlockGridColumn(g9, ((PagingGridTag)g3.Tag).MouseDownCol, LockThisColumn);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void LockUnlockRow(C1FlexGrid aGrid, bool LockThisRow)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)aGrid.Tag).GridId == Grid4)
		//        {
		//            if (((PagingGridTag)g5.Tag).Visible)
		//            {
		//                LockUnlockGridRow(g5, ((PagingGridTag)g4.Tag).MouseDownRow, LockThisRow);
		//            }
		//            if (((PagingGridTag)g6.Tag).Visible)
		//            {
		//                LockUnlockGridRow(g6, ((PagingGridTag)g4.Tag).MouseDownRow, LockThisRow);
		//            }
		//        }
		//        else if (((PagingGridTag)aGrid.Tag).GridId == Grid7)
		//        {
		//            if (((PagingGridTag)g8.Tag).Visible)
		//            {
		//                LockUnlockGridRow(g8, ((PagingGridTag)g7.Tag).MouseDownRow, LockThisRow);
		//            }
		//            if (((PagingGridTag)g9.Tag).Visible)
		//            {
		//                LockUnlockGridRow(g9, ((PagingGridTag)g7.Tag).MouseDownRow, LockThisRow);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void LockUnlockSheet(bool LockThisSheet)
		//{
		//    //There are several ways to lock/unlock the entire sheet.
		//    //One way to achieve this is to loop through each column and 
		//    //perform a column lock on the ones that are lockable.
		//    try
		//    {
		//        int col;

		//        for (col = 0; col < g2.Cols.Count; col++)
		//        {
		//            if (((PagingGridTag)g5.Tag).Visible)
		//            {
		//                LockUnlockGridColumn(g5, col, LockThisSheet);
		//            }
		//            if (((PagingGridTag)g8.Tag).Visible)
		//            {
		//                LockUnlockGridColumn(g8, col, LockThisSheet);
		//            }
		//        }

		//        for (col = 0; col < g3.Cols.Count; col++)
		//        {
		//            if (((PagingGridTag)g6.Tag).Visible)
		//            {
		//                LockUnlockGridColumn(g6, col, LockThisSheet);
		//            }
		//            if (((PagingGridTag)g9.Tag).Visible)
		//            {
		//                LockUnlockGridColumn(g9, col, LockThisSheet);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void LockUnlockGridCell(C1FlexGrid aGrid, bool LockThisCell)
		//{
		//    PagingGridTag gridTag;
		//    CellTag cellTag;

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[gridTag.MouseDownRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[gridTag.MouseDownCol].UserData).Order];

		//        if (ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) ||
		//            ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) ||
		//            AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags) ||
		//            AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags) ||
		//            ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags))
		//        {
		//            MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_NotLockable), _windowName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		//        }
		//        else
		//        {
		//            // BEGIN TT#2 - stodd - assortment multiselection
		//            for (int row = aGrid.Selection.TopRow; row <= aGrid.Selection.BottomRow; row++)
		//            {
		//                for (int col = aGrid.Selection.LeftCol; col <= aGrid.Selection.RightCol; col++)
		//                {
		//                    LockUnlockCell(aGrid, row, col, LockThisCell);
		//                }
		//            }
		//            //LockUnlockCell(aGrid, gridTag.MouseDownRow, gridTag.MouseDownCol, LockThisCell);
		//            // END TT#2 - stodd - assortment multiselection
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void LockUnlockGridColumn(C1FlexGrid aGrid, int aColumn, bool aLock)
		//{
		//    PagingGridTag gridTag;
		//    RowHeaderTag rowTag;
		//    CellTag cellTag;

		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);

		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        GetCellRange(aGrid, 0, aColumn, aGrid.Rows.Count - 1, aColumn, 1);

		//        for (int i = 0; i < aGrid.Rows.Count; i++)
		//        {
		//            rowTag = (RowHeaderTag)gridTag.RowHeaderGrid.Rows[i].UserData;

		//            if (rowTag != null)
		//            {
		//                cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[i].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[aColumn].UserData).Order];

		//                if (!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
		//                    !ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
		//                    !AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags) &&
		//                    !AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags) &&
		//                    !ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) &&
		//                    ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags) != aLock)
		//                {
		//                    LockUnlockCell(aGrid, i, aColumn, aLock);
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void LockUnlockGridRow(C1FlexGrid aGrid, int aRow, bool aLock)
		//{
		//    PagingGridTag gridTag;
		//    CellTag cellTag;

		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);

		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        GetCellRange(aGrid, aRow, 0, aRow, aGrid.Cols.Count - 1, 1);

		//        for (int i = 0; i < aGrid.Cols.Count; i++)
		//        {
		//            cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[aRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[i].UserData).Order];

		//            if (!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
		//                !ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
		//                !AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags) &&
		//                !AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags) &&
		//                !ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) &&
		//                ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags) != aLock)
		//            {
		//                LockUnlockCell(aGrid, aRow, i, aLock);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void LockUnlockCell(C1FlexGrid aGrid, int aRow, int aCol, bool aLockCell)
		//{
		//    PagingGridTag gridTag;

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;

		//        _asrtCubeGroup.SetCellLockStatus(
		//            _commonWaferCoordinateList,
		//            ((RowHeaderTag)gridTag.RowHeaderGrid.Rows[aRow].UserData).CubeWaferCoorList,
		//            ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[aCol].UserData).CubeWaferCoorList,
		//            aLockCell);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void cmiCascadeLockCell_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CascadeLockUnlockGridCell(_rightClickedFrom, true);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiCascadeUnlockCell_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CascadeLockUnlockGridCell(_rightClickedFrom, false);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiCascadeLockColumn_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CascadeLockUnlockColumn(_rightClickedFrom, true);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiCascadeUnlockColumn_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CascadeLockUnlockColumn(_rightClickedFrom, false);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiCascadeLockRow_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CascadeLockUnlockRow(_rightClickedFrom, true);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiCascadeUnlockRow_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CascadeLockUnlockRow(_rightClickedFrom, false);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiCascadeLockSection_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CascadeLockUnlockSheet(true);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiCascadeUnlockSection_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CascadeLockUnlockSheet(false);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void CascadeLockUnlockColumn(C1FlexGrid aGrid, bool LockThisColumn)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)aGrid.Tag).GridId == Grid2)
		//        {
		//            //lock and unlock the column only if the column is lockable.
		//            CascadeLockUnlockGridColumn(g5, ((PagingGridTag)g2.Tag).MouseDownCol, LockThisColumn);
		//            CascadeLockUnlockGridColumn(g8, ((PagingGridTag)g2.Tag).MouseDownCol, LockThisColumn);
		//        }
		//        else if (((PagingGridTag)aGrid.Tag).GridId == Grid3)
		//        {
		//            CascadeLockUnlockGridColumn(g6, ((PagingGridTag)g3.Tag).MouseDownCol, LockThisColumn);
		//            CascadeLockUnlockGridColumn(g9, ((PagingGridTag)g3.Tag).MouseDownCol, LockThisColumn);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CascadeLockUnlockRow(C1FlexGrid aGrid, bool LockThisRow)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)aGrid.Tag).GridId == Grid4)
		//        {
		//            if (((PagingGridTag)g5.Tag).Visible)
		//            {
		//                CascadeLockUnlockGridRow(g5, ((PagingGridTag)g4.Tag).MouseDownRow, LockThisRow);
		//            }
		//            if (((PagingGridTag)g6.Tag).Visible)
		//            {
		//                CascadeLockUnlockGridRow(g6, ((PagingGridTag)g4.Tag).MouseDownRow, LockThisRow);
		//            }
		//        }
		//        else if (((PagingGridTag)aGrid.Tag).GridId == Grid7)
		//        {
		//            if (((PagingGridTag)g8.Tag).Visible)
		//            {
		//                CascadeLockUnlockGridRow(g8, ((PagingGridTag)g7.Tag).MouseDownRow, LockThisRow);
		//            }
		//            if (((PagingGridTag)g9.Tag).Visible)
		//            {
		//                CascadeLockUnlockGridRow(g9, ((PagingGridTag)g7.Tag).MouseDownRow, LockThisRow);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CascadeLockUnlockSheet(bool LockThisSheet)
		//{
		//    //There are several ways to lock/unlock the entire sheet.
		//    //One way to achieve this is to loop through each column and 
		//    //perform a column lock on the ones that are lockable.
		//    try
		//    {
		//        int col;

		//        for (col = 0; col < g2.Cols.Count; col++)
		//        {
		//            if (((PagingGridTag)g5.Tag).Visible)
		//            {
		//                CascadeLockUnlockGridColumn(g5, col, LockThisSheet);
		//            }
		//            if (((PagingGridTag)g8.Tag).Visible)
		//            {
		//                CascadeLockUnlockGridColumn(g8, col, LockThisSheet);
		//            }
		//        }

		//        for (col = 0; col < g3.Cols.Count; col++)
		//        {
		//            if (((PagingGridTag)g6.Tag).Visible)
		//            {
		//                CascadeLockUnlockGridColumn(g6, col, LockThisSheet);
		//            }
		//            if (((PagingGridTag)g9.Tag).Visible)
		//            {
		//                CascadeLockUnlockGridColumn(g9, col, LockThisSheet);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CascadeLockUnlockGridCell(C1FlexGrid aGrid, bool LockThisCell)
		//{
		//    PagingGridTag gridTag;
		//    CellTag cellTag;

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[gridTag.MouseDownRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[gridTag.MouseDownCol].UserData).Order];

		//        if (ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) ||
		//            ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) ||
		//            AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags) ||
		//            AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags) ||
		//            ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags))
		//        {
		//            MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_NotLockable), _windowName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		//        }
		//        else
		//        {
		//            CascadeLockUnlockCell(aGrid, gridTag.MouseDownRow, gridTag.MouseDownCol, LockThisCell);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CascadeLockUnlockGridColumn(C1FlexGrid aGrid, int aColumn, bool aLock)
		//{
		//    PagingGridTag gridTag;
		//    RowHeaderTag rowTag;
		//    CellTag cellTag;

		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);

		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        GetCellRange(aGrid, 0, aColumn, aGrid.Rows.Count - 1, aColumn, 1);

		//        for (int i = 0; i < aGrid.Rows.Count; i++)
		//        {
		//            rowTag = (RowHeaderTag)gridTag.RowHeaderGrid.Rows[i].UserData;

		//            if (rowTag != null)
		//            {
		//                cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[i].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[aColumn].UserData).Order];

		//                if (!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
		//                    !ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
		//                    !AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags) &&
		//                    !AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags) &&
		//                    !ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) &&
		//                    ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags) != aLock)
		//                {
		//                    CascadeLockUnlockCell(aGrid, i, aColumn, aLock);
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void CascadeLockUnlockGridRow(C1FlexGrid aGrid, int aRow, bool aLock)
		//{
		//    PagingGridTag gridTag;
		//    CellTag cellTag;

		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);

		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        GetCellRange(aGrid, aRow, 0, aRow, aGrid.Cols.Count - 1, 1);

		//        for (int i = 0; i < aGrid.Cols.Count; i++)
		//        {
		//            cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[aRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[i].UserData).Order];

		//            if (!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
		//                !ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
		//                !AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags) &&
		//                !AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags) &&
		//                !ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) &&
		//                ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags) != aLock)
		//            {
		//                CascadeLockUnlockCell(aGrid, aRow, i, aLock);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void CascadeLockUnlockCell(C1FlexGrid aGrid, int aRow, int aCol, bool aLockCell)
		//{
		//    PagingGridTag gridTag;

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;

		//        _asrtCubeGroup.SetCellRecursiveLockStatus(
		//            _commonWaferCoordinateList,
		//            ((RowHeaderTag)gridTag.RowHeaderGrid.Rows[aRow].UserData).CubeWaferCoorList,
		//            ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[aCol].UserData).CubeWaferCoorList,
		//            aLockCell);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//#endregion

		//#region Closing/Opening Style for Cell, Column, Row, and Sheet

		//private void cmiCloseStyle_Click(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CloseOpenGridCell(_rightClickedFrom, !cmiCloseStyle.Checked);

		//        //BEGIN TT#430 - MD - DOConnell - When closing styles in the matrix grid the Average Units is not automatically recalculating.
		//        ComponentsChanged();
		//        ReformatRowsChanged(true);
		//        //END TT#430 - MD - DOConnell - When closing styles in the matrix grid the Average Units is not automatically recalculating.

		//        RecomputePlanCubes();
		//        LoadCurrentPages();
		//        LoadSurroundingPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiCloseColumn_Click(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CloseOpenColumn(_rightClickedFrom, true);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiOpenColumn_Click(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CloseOpenColumn(_rightClickedFrom, false);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiCloseRow_Click(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CloseOpenRow(_rightClickedFrom, true);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void cmiOpenRow_Click(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);
		//        StopPageLoadThreads();

		//        CloseOpenRow(_rightClickedFrom, false);

		//        LoadCurrentPages();
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void CloseOpenColumn(C1FlexGrid aGrid, bool CloseThisColumn)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)aGrid.Tag).GridId == Grid3)
		//        {
		//            CloseOpenGridColumn(g6, ((PagingGridTag)g3.Tag).MouseDownCol, CloseThisColumn);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CloseOpenRow(C1FlexGrid aGrid, bool CloseThisRow)
		//{
		//    try
		//    {
		//        if (((PagingGridTag)aGrid.Tag).GridId == Grid4)
		//        {
		//            if (((PagingGridTag)g6.Tag).Visible)
		//            {
		//                CloseOpenGridRow(g6, ((PagingGridTag)g4.Tag).MouseDownRow, CloseThisRow);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CloseOpenGridCell(C1FlexGrid aGrid, bool CloseThisCell)
		//{
		//    PagingGridTag gridTag;
		//    CellTag cellTag;

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[gridTag.MouseDownRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[gridTag.MouseDownCol].UserData).Order];
		//        // BEGIN TT#2 - stodd - assortment multiselection
		//        // BEGIN TT#1186 - stodd - assortment - illogical coordinate when selectiong set only cells
		//        if (_rightClickedFrom == aGrid)
		//        {
		//            if (gridTag.MouseDownRow >= aGrid.Selection.TopRow && gridTag.MouseDownRow <= aGrid.Selection.BottomRow
		//                && gridTag.MouseDownCol >= aGrid.Selection.LeftCol && gridTag.MouseDownCol <= aGrid.Selection.RightCol)
		//            {
		//                for (int row = aGrid.Selection.TopRow; row <= aGrid.Selection.BottomRow; row++)
		//                {
		//                    for (int col = aGrid.Selection.LeftCol; col <= aGrid.Selection.RightCol; col++)
		//                    {
		//                        bool hasGrade = false;
		//                        foreach (CubeWaferCoordinate wafCoor in ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[col].UserData).CubeWaferCoorList)
		//                        {
		//                            if (wafCoor.WaferCoordinateType == eProfileType.StoreGrade)
		//                            {
		//                                hasGrade = true;
		//                                break;
		//                            }
		//                        }
		//                        if (hasGrade)
		//                        {
		//                            cellTag = _gridData[gridTag.GridId][row, col];
		//                            CloseOpenCell(aGrid, row, col, CloseThisCell);
		//                        }
		//                    }
		//                }
		//            }
		//            else
		//            {
		//                cellTag = _gridData[gridTag.GridId][gridTag.MouseDownRow, gridTag.MouseDownCol];
		//                CloseOpenCell(aGrid, gridTag.MouseDownRow, gridTag.MouseDownCol, CloseThisCell);
		//            }
		//        }
		//        // END TT#1186 - stodd - assortment - illogical coordinate when selectiong set only cells
		//        //CloseOpenCell(aGrid, gridTag.MouseDownRow, gridTag.MouseDownCol, CloseThisCell);
		//        // END TT#2 - stodd - assortment multiselection
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void CloseOpenGridColumn(C1FlexGrid aGrid, int aColumn, bool aClose)
		//{
		//    PagingGridTag gridTag;
		//    RowHeaderTag rowTag;
		//    CellTag cellTag;

		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);

		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        GetCellRange(aGrid, 0, aColumn, aGrid.Rows.Count - 1, aColumn, 1);

		//        for (int i = 0; i < aGrid.Rows.Count; i++)
		//        {
		//            rowTag = (RowHeaderTag)gridTag.RowHeaderGrid.Rows[i].UserData;

		//            if (rowTag != null)
		//            {
		//                cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[i].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[aColumn].UserData).Order];

		//                if (!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
		//                    !ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
		//                    !AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags) &&
		//                    !ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) &&
		//                    AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags) != aClose)
		//                {
		//                    CloseOpenCell(aGrid, i, aColumn, aClose);
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void CloseOpenGridRow(C1FlexGrid aGrid, int aRow, bool aClose)
		//{
		//    PagingGridTag gridTag;
		//    CellTag cellTag;

		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        SetGridRedraws(false);

		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        GetCellRange(aGrid, aRow, 0, aRow, aGrid.Cols.Count - 1, 1);

		//        for (int i = 0; i < aGrid.Cols.Count; i++)
		//        {
		//            cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[aRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[i].UserData).Order];

		//            if (!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
		//                !ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
		//                !AssortmentCellFlagValues.isFixed(cellTag.ComputationCellFlags) &&
		//                !ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) &&
		//                AssortmentCellFlagValues.isBlocked(cellTag.ComputationCellFlags) != aClose)
		//            {
		//                CloseOpenCell(aGrid, aRow, i, aClose);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		//private void CloseOpenCell(C1FlexGrid aGrid, int aRow, int aCol, bool aCloseCell)
		//{
		//    PagingGridTag gridTag;

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;

		//        _asrtCubeGroup.SetCellBlockStatus(
		//            _commonWaferCoordinateList,
		//            ((RowHeaderTag)gridTag.RowHeaderGrid.Rows[aRow].UserData).CubeWaferCoorList,
		//            ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[aCol].UserData).CubeWaferCoorList,
		//            aCloseCell);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//#endregion

		//#region Group By Week or Group By Variables

		//private void optGroupByStoreGroup_CheckedChanged(object sender, EventArgs e)
		//{
		//    int i;
		//    ColumnHeaderTag ColumnTag;
		//    int g2VariableSortKey;
		//    SortEnum g2SortDirection;
		//    int g3VariableSortKey;
		//    SortEnum g3SortDirection;

		//    try
		//    {
		//        if (_columnGroupedBy != eAllocationAssortmentViewGroupBy.Attribute)
		//        {
		//            _columnGroupedBy = eAllocationAssortmentViewGroupBy.Attribute;

		//            if (!_formLoading)
		//            {
		//                Cursor.Current = Cursors.WaitCursor;
		//                SetGridRedraws(false);

		//                try
		//                {
		//                    StopPageLoadThreads();

		//                    g2VariableSortKey = -1;
		//                    g2SortDirection = SortEnum.none;

		//                    if (((PagingGridTag)g2.Tag).Visible)
		//                    {
		//                        g2.Clear(ClearFlags.Content);

		//                        for (i = 0; i < g2.Cols.Count; i++)
		//                        {
		//                            ColumnTag = (ColumnHeaderTag)g2.Cols[i].UserData;
		//                            if (ColumnTag.Sort == SortEnum.asc || ColumnTag.Sort == SortEnum.desc)
		//                            {
		//                                g2VariableSortKey = ColumnTag.DetailRowColHeader.Profile.Key;
		//                                g2SortDirection = ColumnTag.Sort;
		//                                break;
		//                            }
		//                        }
		//                    }

		//                    g3VariableSortKey = -1;
		//                    g3SortDirection = SortEnum.none;

		//                    if (((PagingGridTag)g3.Tag).Visible)
		//                    {
		//                        g3.Clear(ClearFlags.Content);

		//                        for (i = 0; i < g3.Cols.Count; i++)
		//                        {
		//                            ColumnTag = (ColumnHeaderTag)g3.Cols[i].UserData;
		//                            if (ColumnTag.Sort == SortEnum.asc || ColumnTag.Sort == SortEnum.desc)
		//                            {
		//                                g3VariableSortKey = ColumnTag.DetailRowColHeader.Profile.Key;
		//                                g3SortDirection = ColumnTag.Sort;
		//                                break;
		//                            }
		//                        }
		//                    }

		//                    ReformatGroupingChanged(true, g2VariableSortKey, g2SortDirection, g3VariableSortKey, g3SortDirection);
		//                    cboStoreGroupLevel.Enabled = false;
		//                    // Begin TT#1278 - RMatelic - Assortment When selecting Save no hour glass appears and the user cannot tell if somethingis saved or not.  Selecting the X in the right had corner does not give a message asking if the user wants to save or not.
		//                    if (_assortReviewAssortmentSecurity.AllowUpdate)
		//                    {
		//                        ChangePending = true;
		//                    }
		//                    // End TT#1278 
		//                }
		//                catch (Exception exc)
		//                {
		//                    string message = exc.ToString();
		//                    throw;
		//                }
		//                finally
		//                {
		//                    SetGridRedraws(true);
		//                    LoadSurroundingPages();
		//                    Cursor.Current = Cursors.Default;
		//                    g6.Focus();
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//private void optGroupByStoreGrade_CheckedChanged(object sender, EventArgs e)
		//{
		//    int i;
		//    ColumnHeaderTag ColumnTag;
		//    int g2VariableSortKey;
		//    SortEnum g2SortDirection;
		//    int g3VariableSortKey;
		//    SortEnum g3SortDirection;

		//    try
		//    {
		//        if (_columnGroupedBy != eAllocationAssortmentViewGroupBy.StoreGrade)
		//        {
		//            _columnGroupedBy = eAllocationAssortmentViewGroupBy.StoreGrade;

		//            if (!_formLoading)
		//            {
		//                Cursor.Current = Cursors.WaitCursor;
		//                SetGridRedraws(false);

		//                try
		//                {
		//                    StopPageLoadThreads();

		//                    g2VariableSortKey = -1;
		//                    g2SortDirection = SortEnum.none;

		//                    if (((PagingGridTag)g2.Tag).Visible)
		//                    {
		//                        g2.Clear(ClearFlags.Content);

		//                        for (i = 0; i < g3.Cols.Count; i++)
		//                        {
		//                            ColumnTag = (ColumnHeaderTag)g3.Cols[i].UserData;
		//                            if (ColumnTag.Sort == SortEnum.asc || ColumnTag.Sort == SortEnum.desc)
		//                            {
		//                                g2VariableSortKey = ColumnTag.GroupRowColHeader.Profile.Key;
		//                                g2SortDirection = ColumnTag.Sort;
		//                                break;
		//                            }
		//                        }
		//                    }

		//                    g3VariableSortKey = -1;
		//                    g3SortDirection = SortEnum.none;

		//                    if (((PagingGridTag)g3.Tag).Visible)
		//                    {
		//                        g3.Clear(ClearFlags.Content);

		//                        for (i = 0; i < g3.Cols.Count; i++)
		//                        {
		//                            ColumnTag = (ColumnHeaderTag)g3.Cols[i].UserData;
		//                            if (ColumnTag.Sort == SortEnum.asc || ColumnTag.Sort == SortEnum.desc)
		//                            {
		//                                g3VariableSortKey = ColumnTag.GroupRowColHeader.Profile.Key;
		//                                g3SortDirection = ColumnTag.Sort;
		//                                break;
		//                            }
		//                        }
		//                    }

		//                    ReformatGroupingChanged(true, g2VariableSortKey, g2SortDirection, g3VariableSortKey, g3SortDirection);
		//                    cboStoreGroupLevel.Enabled = true;
		//                    // Begin TT#1278 - RMatelic - Assortment When selecting Save no hour glass appears and the user cannot tell if somethingis saved or not.  Selecting the X in the right had corner does not give a message asking if the user wants to save or not.
		//                    if (_assortReviewAssortmentSecurity.AllowUpdate)
		//                    {
		//                        ChangePending = true;
		//                    }
		//                    // End TT#1278 
		//                }
		//                catch (Exception exc)
		//                {
		//                    string message = exc.ToString();
		//                    throw;
		//                }
		//                finally
		//                {
		//                    SetGridRedraws(true);
		//                    LoadSurroundingPages();
		//                    Cursor.Current = Cursors.Default;
		//                    g6.Focus();
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		#endregion

		#region Sorting columns
		//Summary and note about this region:
		//To sort a single column, the user clicks on a column heading. If the
		//column hasn't been sorted, we sort it in ascending order. Otherwise,
		//we toggle between ascending/descending orders.

		//To sort single column, we want to put the code in g2.Click or g3.Click events.
		//The biggest obstacle is that "g2(or g3).Click" may fire in many 
		//(irrelevant) situations, for example, when user right clicks the heading
		//and selects a context menu item, or clicks the header border to resize the column.
		//Therefore, we need to use the _isSorting flag to determine whether the
		//user really means to sort this column or did some other events trigger
		//it. This "_isSorting" flag is set and updated in many places:
		//g2( or g3).MouseDown|BeforeResizeColumn|AfterResizeColumn|DragDrop...

		//public void DoSorts()
		//{
		//    SortGridViews frmSortGridViews;
		//    ArrayList sortList;
		//    ArrayList valueList;

		//    try
		//    {
		//        sortList = BuildColumnList();
		//        valueList = BuildRowList();

		//        frmSortGridViews = new SortGridViews(_currSortParms, sortList, valueList);
		//        frmSortGridViews.StartPosition = FormStartPosition.CenterParent;

		//        if (frmSortGridViews.ShowDialog() == DialogResult.OK)
		//        {
		//            _currSortParms = frmSortGridViews.SortInfo;

		//            Cursor.Current = Cursors.WaitCursor;

		//            SetGridRedraws(false);
		//            StopPageLoadThreads();

		//            SortColumns(g4, ref _currSortParms);

		//            if (_theme.ViewStyle == StyleEnum.AlterColors)
		//            {
		//                Cursor.Current = Cursors.WaitCursor;
		//                if (((PagingGridTag)g4.Tag).Visible)
		//                {
		//                    Formatg4Grid(false);
		//                }
		//                if (((PagingGridTag)g5.Tag).Visible)
		//                {
		//                    Formatg5Grid(false);
		//                }
		//                if (((PagingGridTag)g6.Tag).Visible)
		//                {
		//                    Formatg6Grid(false);
		//                }
		//                if (((PagingGridTag)g7.Tag).Visible)
		//                {
		//                    Formatg7Grid(false);
		//                }
		//                if (((PagingGridTag)g8.Tag).Visible)
		//                {
		//                    Formatg8Grid(false);
		//                }
		//                if (((PagingGridTag)g9.Tag).Visible)
		//                {
		//                    Formatg9Grid(false);
		//                }
		//                Cursor.Current = Cursors.Default;
		//            }

		//            LoadCurrentPages();
		//        }

		//        frmSortGridViews.Dispose();
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//    finally
		//    {
		//        SetGridRedraws(true);
		//        LoadSurroundingPages();
		//        Cursor.Current = Cursors.Default;
		//        g6.Focus();
		//    }
		//}

		//private ArrayList BuildColumnList()
		//{
		//    SortCriteria sortData;
		//    ColumnHeaderTag colHdrTag;
		//    int i;
		//    ArrayList outList;

		//    try
		//    {
		//        outList = new ArrayList();

		//        if (g2.Visible)
		//        {
		//            for (i = 0; i < g2.Cols.Count; i++)
		//            {
		//                colHdrTag = (ColumnHeaderTag)g2.Cols[i].UserData;
		//                sortData = new SortCriteria();
		//                sortData.Column1 = "Time Total";
		//                sortData.Column2 = colHdrTag.ScrollDisplay[0];
		//                sortData.Column2Num = i;
		//                sortData.Column2GridPtr = g5;
		//                sortData.Column2Format = ((ComputationVariableProfile)colHdrTag.DetailRowColHeader.Profile).FormatType;
		//                sortData.SortDirection = SortEnum.none;
		//                outList.Add(sortData);
		//            }
		//        }

		//        for (i = 0; i < g3.Cols.Count; i++)
		//        {
		//            colHdrTag = (ColumnHeaderTag)g3.Cols[i].UserData;
		//            sortData = new SortCriteria();
		//            sortData.Column1 = colHdrTag.ScrollDisplay[0];
		//            sortData.Column2 = colHdrTag.ScrollDisplay[1];
		//            sortData.Column2Num = i;
		//            sortData.Column2GridPtr = g6;
		//            sortData.Column2Format = ((ComputationVariableProfile)colHdrTag.DetailRowColHeader.Profile).FormatType;
		//            sortData.SortDirection = SortEnum.none;
		//            outList.Add(sortData);
		//        }

		//        return outList;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private ArrayList BuildRowList()
		//{
		//    ArrayList outList;
		//    int detailsPerGroup;
		//    SortValue sortData;
		//    RowHeaderTag rowHdrTag;
		//    int i;

		//    try
		//    {
		//        outList = new ArrayList();
		//        detailsPerGroup = ((PagingGridTag)g4.Tag).RowsPerRowGroup;

		//        for (i = 0; i < detailsPerGroup; i++)
		//        {
		//            rowHdrTag = (RowHeaderTag)g4.Rows[i].UserData;
		//            sortData = new SortValue();
		//            sortData.Row1 = rowHdrTag.GroupRowColHeader.Name;
		//            sortData.Row2 = rowHdrTag.DetailRowColHeader.Name;
		//            sortData.Row2Num = i;
		//            sortData.Row2Format = ((ComputationVariableProfile)rowHdrTag.DetailRowColHeader.Profile).FormatType;
		//            outList.Add(sortData);
		//        }

		//        return outList;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void SortColumns(C1FlexGrid aGrid, ref structSort aSortParms)
		//{
		//    PagingGridTag gridTag;
		//    SortValue valueData;
		//    ArrayList sortDirList;
		//    int i;
		//    int j;
		//    SortCriteria sortData;
		//    string cellValue;
		//    SortedList sortedList;
		//    int valueRow;
		//    ArrayList keyList;
		//    GridSortEntry sortEnt;
		//    ColumnHeaderTag colTag;
		//    C1FlexGrid colHdrGrid;
		//    ColumnHeaderTag colHdrTag;

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;

		//        sortDirList = new ArrayList();

		//        for (i = 0; !aSortParms.IsSortingByDefault && i < aSortParms.SortInfo.Count; i++)
		//        {
		//            if (aSortParms.SortInfo[i] != null)
		//            {
		//                sortData = (SortCriteria)aSortParms.SortInfo[i];

		//                if (sortData.Column1 != String.Empty)
		//                {
		//                    GetCellRange(sortData.Column2GridPtr, 0, sortData.Column2Num, sortData.Column2GridPtr.Rows.Count - 1, sortData.Column2Num, 1);
		//                    sortDirList.Add(sortData.SortDirection);
		//                }
		//            }
		//        }

		//        sortedList = new SortedList(new SortComparer(sortDirList));

		//        for (i = 0; i < gridTag.RowGroupsPerGrid; i++)
		//        {
		//            keyList = new ArrayList();

		//            if (aSortParms.ValueInfo != null)
		//            {
		//                valueData = (SortValue)aSortParms.ValueInfo;
		//                valueRow = (i * gridTag.RowsPerRowGroup) + valueData.Row2Num;

		//                for (j = 0; !aSortParms.IsSortingByDefault && j < aSortParms.SortInfo.Count; j++)
		//                {
		//                    if (aSortParms.SortInfo[j] != null)
		//                    {
		//                        sortData = (SortCriteria)aSortParms.SortInfo[j];

		//                        if (sortData.Column1 != String.Empty)
		//                        {
		//                            cellValue = Convert.ToString(sortData.Column2GridPtr[valueRow, sortData.Column2Num]).Trim();

		//                            switch (valueData.Row2Format)
		//                            {
		//                                case eValueFormatType.None:

		//                                    switch (sortData.Column2Format)
		//                                    {
		//                                        case eValueFormatType.GenericNumeric:

		//                                            if (cellValue.Length > 0)
		//                                            {
		//                                                keyList.Add(Convert.ToDouble(sortData.Column2GridPtr[valueRow, sortData.Column2Num]));
		//                                            }
		//                                            else
		//                                            {
		//                                                keyList.Add((double)0);
		//                                            }
		//                                            break;

		//                                        default:

		//                                            keyList.Add(cellValue);
		//                                            break;
		//                                    }

		//                                    break;

		//                                case eValueFormatType.GenericNumeric:

		//                                    if (cellValue.Length > 0)
		//                                    {
		//                                        keyList.Add(Convert.ToDouble(sortData.Column2GridPtr[valueRow, sortData.Column2Num]));
		//                                    }
		//                                    else
		//                                    {
		//                                        keyList.Add((double)0);
		//                                    }
		//                                    break;

		//                                default:

		//                                    keyList.Add(cellValue);
		//                                    break;
		//                            }
		//                        }
		//                    }
		//                }
		//            }

		//            sortedList.Add(new GridSortEntry(aGrid.Rows[i * gridTag.RowsPerRowGroup], keyList, Convert.ToString(aGrid[i * gridTag.RowsPerRowGroup, 0]), i), null);
		//        }

		//        for (i = 0; i < sortedList.Count; i++)
		//        {
		//            sortEnt = (GridSortEntry)sortedList.GetKey(i);
		//            MoveRows(aGrid, gridTag.RowsPerRowGroup, sortEnt.RowIndex, i * gridTag.RowsPerRowGroup);
		//        }

		//        for (i = 0; i < g2.Cols.Count; i++)
		//        {
		//            colTag = (ColumnHeaderTag)g2.Cols[i].UserData;
		//            colTag.Sort = SortEnum.none;
		//            g2.Cols[i].UserData = colTag;
		//            g2.SetCellImage(g2.Rows.Count - 1, i, null);
		//        }

		//        for (i = 0; i < g3.Cols.Count; i++)
		//        {
		//            colTag = (ColumnHeaderTag)g3.Cols[i].UserData;
		//            colTag.Sort = SortEnum.none;
		//            g3.Cols[i].UserData = colTag;
		//            g3.SetCellImage(g3.Rows.Count - 1, i, null);
		//        }

		//        for (i = 0; !aSortParms.IsSortingByDefault && i < aSortParms.SortInfo.Count; i++)
		//        {
		//            if (aSortParms.SortInfo[i] != null)
		//            {
		//                sortData = (SortCriteria)aSortParms.SortInfo[i];

		//                if (sortData.Column1 != String.Empty)
		//                {
		//                    colHdrGrid = ((PagingGridTag)sortData.Column2GridPtr.Tag).ColHeaderGrid;
		//                    colHdrTag = (ColumnHeaderTag)colHdrGrid.Cols[sortData.Column2Num].UserData;

		//                    if (sortData.SortDirection == SortEnum.asc)
		//                    {
		//                        colHdrGrid.SetCellImage(colHdrGrid.Rows.Count - 1, sortData.Column2Num, _upArrow);
		//                        colHdrTag.Sort = SortEnum.asc;
		//                    }
		//                    else
		//                    {
		//                        colHdrGrid.SetCellImage(colHdrGrid.Rows.Count - 1, sortData.Column2Num, _downArrow);
		//                        colHdrTag.Sort = SortEnum.desc;
		//                    }

		//                    colHdrGrid.Cols[sortData.Column2Num].UserData = colHdrTag;
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void SortToDefault()
		//{
		//    try
		//    {
		//        _currSortParms = new structSort();
		//        _currSortParms.IsSortingByDefault = true;

		//        SortColumns(g4, ref _currSortParms);
		//        SortColumns(g7, ref _currSortParms);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void MoveRows(C1FlexGrid aGrid, int aRowsPerGroup, int OldIndex, int NewIndex)
		//{
		//    Object detailProf;

		//    try
		//    {
		//        switch (((PagingGridTag)aGrid.Tag).GridId)
		//        {
		//            case Grid4:

		//                if (((PagingGridTag)g4.Tag).Visible)
		//                {
		//                    g4.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
		//                }
		//                if (((PagingGridTag)g5.Tag).Visible)
		//                {
		//                    g5.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
		//                }
		//                if (((PagingGridTag)g6.Tag).Visible)
		//                {
		//                    g6.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
		//                }

		//                detailProf = _workingDetailProfileList.ArrayList[OldIndex / aRowsPerGroup];
		//                _workingDetailProfileList.ArrayList.RemoveAt(OldIndex / aRowsPerGroup);
		//                _workingDetailProfileList.ArrayList.Insert(NewIndex / aRowsPerGroup, detailProf);

		//                break;

		//            case Grid7:

		//                if (((PagingGridTag)g7.Tag).Visible)
		//                {
		//                    g7.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
		//                }
		//                if (((PagingGridTag)g8.Tag).Visible)
		//                {
		//                    g8.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
		//                }
		//                if (((PagingGridTag)g9.Tag).Visible)
		//                {
		//                    g9.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
		//                }

		//                break;
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void g7_Click(object sender, System.EventArgs e)
		//{
		//    //Add attribute-changing code here.
		//}

		#endregion

		#region Grid Reformat methods

		//private void ReformatRowsChanged(bool aClearGrid)
		//{
		//    try
		//    {
		//        FormatCol1Grids(aClearGrid);
		//        FormatCol2Grids(aClearGrid, -1, SortEnum.none);
		//        FormatCol3Grids(aClearGrid, -1, SortEnum.none);
		//        DefineStyles(false);
		//        ResizeRow1(false);
		//        ResizeRow4(false);
		//        ResizeRow7(false);
		//        SetHScrollBar2Parameters();
		//        SetHScrollBar3Parameters();
		//        SetVScrollBar2Parameters();
		//        SetVScrollBar3Parameters();
		//        SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
		//        SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
		//        SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
		//        SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
		//        LoadCurrentPages();
		//        ResizeCol1();
		//        ResizeCol2();
		//        ResizeCol3();
		//        ResizeRow1(true);
		//        ResizeRow4(true);
		//        ResizeRow7(true);
		//        CalcColSplitPosition2(false);
		//        CalcColSplitPosition3(false);
		//        SetColSplitPositions();
		//        CalcRowSplitPosition4(false);
		//        CalcRowSplitPosition12(false);
		//        SetRowSplitPositions();

		//        // Reset Scroll bars due to change in split positions
		//        SetHScrollBar2Parameters();
		//        SetHScrollBar3Parameters();
		//        SetVScrollBar2Parameters();
		//        SetVScrollBar3Parameters();
		//        SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
		//        SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
		//        SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
		//        SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void ReformatColsChanged(bool aClearGrid)
		//{
		//    try
		//    {
		//        FormatCol1Grids(aClearGrid);
		//        FormatCol2Grids(aClearGrid, -1, SortEnum.none);
		//        FormatCol3Grids(aClearGrid, -1, SortEnum.none);
		//        DefineStyles(false);
		//        ResizeRow1(false);
		//        ResizeRow4(false);
		//        ResizeRow7(false);
		//        SetHScrollBar2Parameters();
		//        SetHScrollBar3Parameters();
		//        SetVScrollBar2Parameters();
		//        SetVScrollBar3Parameters();
		//        SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
		//        SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
		//        SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
		//        SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
		//        LoadCurrentPages();
		//        ResizeCol1();
		//        ResizeCol2();
		//        ResizeCol3();
		//        ResizeRow1(true);
		//        ResizeRow4(true);
		//        ResizeRow7(true);
		//        CalcColSplitPosition2(false);
		//        CalcColSplitPosition3(false);
		//        SetColSplitPositions();
		//        CalcRowSplitPosition4(false);
		//        SetRowSplitPositions();

		//        // Reset Scroll bars due to change in split positions
		//        SetHScrollBar2Parameters();
		//        SetHScrollBar3Parameters();
		//        SetVScrollBar2Parameters();
		//        SetVScrollBar3Parameters();
		//        SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
		//        SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
		//        SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
		//        SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//public void ReformatStoreGroupChanged(bool aClearGrid)
		//{
		//    try
		//    {
		//        FormatCol1Grids(aClearGrid);
		//        FormatCol2Grids(aClearGrid, -1, SortEnum.none);
		//        FormatCol3Grids(aClearGrid, -1, SortEnum.none);
		//        DefineStyles(false);
		//        ResizeRow1(false);
		//        ResizeRow4(false);
		//        ResizeRow7(false);
		//        SetVScrollBar2Parameters();
		//        SetVScrollBar3Parameters();
		//        SetScrollBarPosition(hScrollBar2, 0);
		//        SetScrollBarPosition(hScrollBar3, 0);
		//        SetScrollBarPosition(vScrollBar2, 0);
		//        SetScrollBarPosition(vScrollBar3, 0);
		//        LoadCurrentPages();
		//        ResizeCol1();
		//        ResizeCol3();
		//        ResizeRow1(true);
		//        ResizeRow4(true);
		//        ResizeRow7(true);
		//        CalcColSplitPosition2(false);
		//        SetColSplitPositions();
		//        CalcRowSplitPosition4(false);
		//        CalcRowSplitPosition12(false);
		//        SetRowSplitPositions();

		//        // Reset Scroll bars due to change in split positions
		//        SetHScrollBar2Parameters();
		//        SetHScrollBar3Parameters();
		//        SetVScrollBar2Parameters();
		//        SetVScrollBar3Parameters();
		//        SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
		//        SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
		//        SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
		//        SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
				
		//        // BEGIN TT#677-MD - stodd - expand/collaspe issue
		//        //=================================================================
		//        // After reloading the assortment grid it automatically expands.
		//        // If it was previously collasped, collaspe it again.
		//        //=================================================================
		//        if (_currentTabPage.Name == "tabAssortment")
		//        {
		//            if (_expandAllAssortment == false && _currentTabPage.Name == "tabAssortment")
		//            {
		//                ExpandCollapseAssortmentGrid(false);
		//            }
		//        }
		//        else
		//        {
		//            _expandAllAssortment = true;
		//        }
		//        // END TT#677-MD - stodd - expand/collaspe issue
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//public void ReformatGroupingChanged(bool aClearGrid, int ag2VarSortKey, SortEnum ag2SortDir, int ag3VarSortKey, SortEnum ag3SortDir)
		//{
		//    try
		//    {
		//        FormatCol1Grids(aClearGrid);
		//        FormatCol2Grids(aClearGrid, ag2VarSortKey, ag2SortDir);
		//        FormatCol3Grids(aClearGrid, ag3VarSortKey, ag3SortDir);
		//        DefineStyles(false);
		//        ResizeRow1(false);
		//        ResizeRow4(false);
		//        ResizeRow7(false);
		//        SetHScrollBar2Parameters();
		//        SetHScrollBar3Parameters();
		//        SetScrollBarPosition(hScrollBar2, 0);
		//        SetScrollBarPosition(hScrollBar3, 0);
		//        LoadCurrentPages();
		//        ResizeCol1();
		//        ResizeCol2();
		//        ResizeCol3();
		//        ResizeRow1(true);
		//        ResizeRow4(true);
		//        ResizeRow7(true);
		//        CalcColSplitPosition3(false);
		//        SetColSplitPositions();
		//        CalcRowSplitPosition4(false);
		//        SetRowSplitPositions();

		//        // Reset Scroll bars due to change in split positions
		//        SetHScrollBar2Parameters();
		//        SetHScrollBar3Parameters();
		//        SetVScrollBar2Parameters();
		//        SetVScrollBar3Parameters();
		//        SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
		//        SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
		//        SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
		//        SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		#endregion

		#region Loading actual data into grids, including threading-populate

		/// <summary>
		/// This method is called by each process/event to load the current visible grid pages.
		/// </summary>

		//private void LoadCurrentPages()
		//{
		//    try
		//    {
		//        StopPageLoadThreads();

		//        ((PagingGridTag)g2.Tag).AllocatePageArray();
		//        ((PagingGridTag)g3.Tag).AllocatePageArray();
		//        ((PagingGridTag)g5.Tag).AllocatePageArray();
		//        ((PagingGridTag)g6.Tag).AllocatePageArray();
		//        ((PagingGridTag)g8.Tag).AllocatePageArray();
		//        ((PagingGridTag)g9.Tag).AllocatePageArray();

		//        if (g2.Rows.Count > 0 && g2.Cols.Count > 0)
		//        {
		//            LoadCurrentGridPages(g2);
		//        }
		//        if (g3.Rows.Count > 0 && g3.Cols.Count > 0)
		//        {
		//            LoadCurrentGridPages(g3);
		//        }
		//        if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
		//        {
		//            LoadCurrentGridPages(g5);
		//        }
		//        if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
		//        {
		//            LoadCurrentGridPages(g6);
		//        }
		//        if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
		//        {
		//            LoadCurrentGridPages(g8);
		//        }
		//        if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
		//        {
		//            LoadCurrentGridPages(g9);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		/// <summary>
		/// This method is called by each process/event to load the surrounding grid pages.
		/// </summary>

		//private void LoadSurroundingPages()
		//{
		//    try
		//    {
		//        if (g2.Rows.Count > 0 && g2.Cols.Count > 0)
		//        {
		//            LoadSurroundingGridPages(g2);
		//        }
		//        if (g3.Rows.Count > 0 && g3.Cols.Count > 0)
		//        {
		//            LoadSurroundingGridPages(g3);
		//        }
		//        if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
		//        {
		//            LoadSurroundingGridPages(g5);
		//        }
		//        if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
		//        {
		//            LoadSurroundingGridPages(g6);
		//        }
		//        if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
		//        {
		//            LoadSurroundingGridPages(g8);
		//        }
		//        if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
		//        {
		//            LoadSurroundingGridPages(g9);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		/// <summary>
		/// This method is called by each the "export" process to load all grid pages.
		/// </summary>

		//private void LoadAllPages()
		//{
		//    try
		//    {
		//        StopPageLoadThreads();

		//        if (g2.Rows.Count > 0 && g2.Cols.Count > 0)
		//        {
		//            LoadAllGridPages(g2);
		//        }
		//        if (g3.Rows.Count > 0 && g3.Cols.Count > 0)
		//        {
		//            LoadAllGridPages(g3);
		//        }
		//        if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
		//        {
		//            LoadAllGridPages(g5);
		//        }
		//        if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
		//        {
		//            LoadAllGridPages(g6);
		//        }
		//        if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
		//        {
		//            LoadAllGridPages(g8);
		//        }
		//        if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
		//        {
		//            LoadAllGridPages(g9);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		/// <summary>
		/// This method is called by each the scroll bar "scroll" events to force the reload the current visible page.
		/// </summary>

		//public void LoadCurrentGridPage(C1FlexGrid aGrid)
		//{
		//    try
		//    {
		//        if (aGrid.Rows.Count > 0 && aGrid.Cols.Count > 0)
		//        {
		//            LoadCurrentGridPages(aGrid);
		//            LoadSurroundingGridPages(aGrid);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		/// <summary>
		/// This method is called by the page load routines to do the actual work of loading a current page.
		/// </summary>

		//public void LoadCurrentGridPages(C1FlexGrid aGrid)
		//{
		//    PagingGridTag gridTag;
		//    ArrayList pages;
		//    Cursor holdCursor;

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        pages = gridTag.GetPagesToLoad(aGrid.TopRow, aGrid.LeftCol, Math.Min(aGrid.TopRow + ROWPAGESIZE - 1, aGrid.Rows.Count), Math.Min(aGrid.LeftCol + COLPAGESIZE - 1, aGrid.Cols.Count));

		//        if (pages.Count > 0)
		//        {
		//            holdCursor = Cursor.Current;
		//            Cursor.Current = Cursors.WaitCursor;

		//            try
		//            {
		//                foreach (Point page in pages)
		//                {
		//                    gridTag.LoadPage(page);
		//                }
		//            }
		//            catch (Exception exc)
		//            {
		//                string message = exc.ToString();
		//                throw;
		//            }
		//            finally
		//            {
		//                Cursor.Current = holdCursor;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// This method is called by the page load routines to do the actual work of loading surrounding pages.
		///// </summary>

		//public void LoadSurroundingGridPages(C1FlexGrid aGrid)
		//{
		//    PagingGridTag gridTag;
		//    ArrayList pages;

		//    try
		//    {
		//        if (THREADED_GRID_LOAD)
		//        {
		//            gridTag = (PagingGridTag)aGrid.Tag;
		//            pages = gridTag.GetSurroundingPagesToLoad(aGrid.TopRow, aGrid.LeftCol, Math.Min(aGrid.TopRow + ROWPAGESIZE - 1, aGrid.Rows.Count), Math.Min(aGrid.LeftCol + COLPAGESIZE - 1, aGrid.Cols.Count));

		//            foreach (Point page in pages)
		//            {
		//                gridTag.LoadPageInBackground(page);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// This method is called by the page load routines to do the actual work of loading all pages.
		///// </summary>

		//public void LoadAllGridPages(C1FlexGrid aGrid)
		//{
		//    PagingGridTag gridTag;
		//    ArrayList pages;
		//    Cursor holdCursor;

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;
		//        pages = gridTag.GetPagesToLoad(0, 0, aGrid.Rows.Count - 1, aGrid.Cols.Count - 1);

		//        if (pages.Count > 0)
		//        {
		//            holdCursor = Cursor.Current;
		//            Cursor.Current = Cursors.WaitCursor;

		//            try
		//            {
		//                foreach (Point page in pages)
		//                {
		//                    gridTag.LoadPage(page);
		//                }
		//            }
		//            catch (Exception exc)
		//            {
		//                string message = exc.ToString();
		//                throw;
		//            }
		//            finally
		//            {
		//                Cursor.Current = holdCursor;
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		/// <summary>
		/// This method is the delegate that is passed to the page loader to load the data into the grid.
		/// </summary>

		//public void GetCellRange(
		//    C1FlexGrid aGrid,
		//    int aStartRow,
		//    int aStartCol,
		//    int aEndRow,
		//    int aEndCol,
		//    int aPriority)
		//{
		//    bool wait;
		//    PagingGridTag gridTag;
		//    C1FlexGrid rowHdrGrid;
		//    C1FlexGrid colHdrGrid;
		//    int i, j, x, y;
		//    int row, col;
		//    //Begin TT#2 - JScott - Assortment Planning - Phase 2
		//    //AssortmentWaferCell[,] waferCellTable;
		//    WaferCell[,] waferCellTable;
		//    //End TT#2 - JScott - Assortment Planning - Phase 2
		//    CubeWafer asrtWafer;
		//    ColumnHeaderTag ColTag;
		//    RowHeaderTag RowTag;
		//    ComputationCellFlags cellFlags = new ComputationCellFlags();

		//    try
		//    {
		//        gridTag = (PagingGridTag)aGrid.Tag;

		//        lock (_loadHash.SyncRoot)
		//        {
		//            _loadHash.Add(System.Threading.Thread.CurrentThread.GetHashCode(), aPriority);
		//        }

		//        wait = true;

		//        while (wait)
		//        {
		//            wait = false;

		//            _pageLoadLock.AcquireWriterLock(-1);

		//            try
		//            {
		//                if (_stopPageLoadThread)
		//                {
		//                    throw new EndPageLoadThreadException();
		//                }

		//                lock (_loadHash.SyncRoot)
		//                {
		//                    foreach (int priority in _loadHash.Values)
		//                    {
		//                        if (priority < aPriority)
		//                        {
		//                            throw new WaitPageLoadException();
		//                        }
		//                    }

		//                    _loadHash.Remove(System.Threading.Thread.CurrentThread.GetHashCode());
		//                }

		//                if (aStartRow <= aEndRow && aStartCol <= aEndCol)
		//                {
		//                    rowHdrGrid = gridTag.RowHeaderGrid;
		//                    colHdrGrid = gridTag.ColHeaderGrid;

		//                    //Create the AssortmentWafer to request data
		//                    asrtWafer = new CubeWafer();

		//                    //Fill CommonWaferCoordinateListGroup
		//                    asrtWafer.CommonWaferCoordinateList = _commonWaferCoordinateList;

		//                    //Fill ColWaferCoordinateListGroup
		//                    asrtWafer.ColWaferCoordinateListGroup.Clear();

		//                    for (i = aStartCol; i <= aEndCol; i++)
		//                    {
		//                        ColTag = (ColumnHeaderTag)colHdrGrid.Cols[i].UserData;
		//                        if (ColTag != null)
		//                        {
		//                            asrtWafer.ColWaferCoordinateListGroup.Add(ColTag.CubeWaferCoorList);
		//                        }
		//                    }

		//                    //Fill RowWaferCoordinateListGroup

		//                    asrtWafer.RowWaferCoordinateListGroup.Clear();
		//                    for (i = aStartRow; i <= aEndRow; i++)
		//                    {
		//                        RowTag = (RowHeaderTag)rowHdrGrid.Rows[i].UserData;
		//                        if (RowTag != null)
		//                        {
		//                            asrtWafer.RowWaferCoordinateListGroup.Add(RowTag.CubeWaferCoorList);
		//                        }
		//                    }

		//                    if (asrtWafer.ColWaferCoordinateListGroup.Count > 0 && asrtWafer.RowWaferCoordinateListGroup.Count > 0)
		//                    {
		//                        // Retreive array of values

		//                        waferCellTable = _asrtCubeGroup.GetAssortmentWaferCellValues(asrtWafer);

		//                        //// Load Grid with values

		//                        //aGrid.Redraw = false;

		//                        try
		//                        {
		//                            x = 0;

		//                            for (i = aStartRow; i <= aEndRow; i++)
		//                            {
		//                                RowTag = (RowHeaderTag)rowHdrGrid.Rows[i].UserData;

		//                                if (RowTag != null)
		//                                {
		//                                    y = 0;

		//                                    for (j = aStartCol; j <= aEndCol; j++)
		//                                    {
		//                                        ColTag = (ColumnHeaderTag)colHdrGrid.Cols[j].UserData;

		//                                        if (ColTag != null)
		//                                        {
		//                                            if (_stopPageLoadThread)
		//                                            {
		//                                                throw new EndPageLoadThreadException();
		//                                            }

		//                                            row = ((RowHeaderTag)rowHdrGrid.Rows[i].UserData).Order;
		//                                            col = ((ColumnHeaderTag)colHdrGrid.Cols[j].UserData).Order;

		//                                            if (RowTag.LoadData)
		//                                            {
		//                                                if (waferCellTable[x, y] != null)
		//                                                {
		//                                                    cellFlags = waferCellTable[x, y].Flags;
		//                                                    // BEGIN TT#2327 - stodd - make Total % display only
		//                                                    ApplyCellFlagOverrides(gridTag, x, y, ref cellFlags);
		//                                                    // BEGIN TT#2327 - stodd - make Total % display only

		//                                                    if (!AssortmentCellFlagValues.isBlocked(cellFlags))
		//                                                    {
		//                                                        aGrid[i, j] = waferCellTable[x, y].ValueAsString;
		//                                                        SetLockPicture(aGrid, cellFlags, i, j);
		//                                                    }
		//                                                    else
		//                                                    {
		//                                                        aGrid[i, j] = NULL_DATA_STRING;
		//                                                        aGrid.SetCellImage(i, j, null);
		//                                                    }

		//                                                    if (!_gridData[gridTag.GridId][row, col].CellFlagsInited ||
		//                                                        cellFlags.Flags != _gridData[gridTag.GridId][row, col].ComputationCellFlags.Flags ||
		//                                                        (waferCellTable[x, y] != null &&
		//                                                        waferCellTable[x, y].isValueNegative != _gridData[gridTag.GridId][row, col].isCellNegative))
		//                                                    {
		//                                                        _gridData[gridTag.GridId][row, col].ComputationCellFlags = cellFlags;
		//                                                        _gridData[gridTag.GridId][row, col].isCellNegative = waferCellTable[x, y].isValueNegative;
		//                                                        ChangeCellStyles(aGrid, _gridData[gridTag.GridId][row, col], i, j);
		//                                                    }
		//                                                }
		//                                                else
		//                                                {
		//                                                    aGrid[i, j] = NULL_DATA_STRING;
		//                                                    cellFlags.Clear();
		//                                                    ComputationCellFlagValues.isNull(ref cellFlags, true);

		//                                                    if (!_gridData[gridTag.GridId][row, col].CellFlagsInited ||
		//                                                        cellFlags.Flags != _gridData[gridTag.GridId][row, col].ComputationCellFlags.Flags)
		//                                                    {
		//                                                        // BEGIN TT#2327 - stodd - make Total % display only
		//                                                        ApplyCellFlagOverrides(gridTag, x, y, ref cellFlags);
		//                                                        // END TT#2327 - stodd - make Total % display only

		//                                                        _gridData[gridTag.GridId][row, col].ComputationCellFlags = cellFlags;
		//                                                        ChangeCellStyles(aGrid, _gridData[gridTag.GridId][row, col], i, j);
		//                                                    }
		//                                                }
		//                                            }
		//                                            else
		//                                            {
		//                                                cellFlags.Clear();
		//                                                ComputationCellFlagValues.isNull(ref cellFlags, true);

		//                                                // BEGIN TT#2327 - stodd - make Total % display only
		//                                                ApplyCellFlagOverrides(gridTag, x, y, ref cellFlags);
		//                                                // END TT#2327 - stodd - make Total % display only

		//                                                _gridData[gridTag.GridId][row, col].ComputationCellFlags = cellFlags;
		//                                            }

		//                                            y++;
		//                                        }
		//                                    }

		//                                    x++;
		//                                }
		//                            }
		//                        }
		//                        catch (Exception exc)
		//                        {
		//                            string message = exc.ToString();
		//                            throw;
		//                        }
		//                        finally
		//                        {
		//                            aGrid.Redraw = _currentRedrawState;
		//                        }
		//                    }
		//                }
		//            }
		//            catch (WaitPageLoadException)
		//            {
		//                wait = true;
		//            }
		//            catch (Exception exc)
		//            {
		//                string message = exc.ToString();
		//                throw;
		//            }
		//            finally
		//            {
		//                _pageLoadLock.ReleaseWriterLock();
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		// BEGIN TT#2327 - stodd - make Total % display only
		//void ApplyCellFlagOverrides(PagingGridTag gridTag, int row, int col, ref ComputationCellFlags cellFlags)
		//{
		//    C1FlexGrid rowGrid;
		//    C1FlexGrid colGrid;
		//    CubeWaferCoordinateList rowCoorList;
		//    CubeWaferCoordinateList colCoorList;
		//    eAssortmentDetailVariables detVar = 0;
		//    eAssortmentTotalVariables totVar = 0;
		//    eAssortmentQuantityVariables qtyVar = 0;

		//    try
		//    {
		//        // Total Grids
		//        if (gridTag.GridId == 7 || gridTag.GridId == 8 || gridTag.GridId == 9)
		//        {
		//            rowGrid = gridTag.RowHeaderGrid;
		//            colGrid = gridTag.ColHeaderGrid;
		//            rowCoorList = ((RowHeaderTag)rowGrid.Rows[row].UserData).CubeWaferCoorList;
		//            colCoorList = ((ColumnHeaderTag)colGrid.Cols[col].UserData).CubeWaferCoorList;

		//            foreach (CubeWaferCoordinate coor in rowCoorList)
		//            {
		//                if (coor.WaferCoordinateType == eProfileType.AssortmentQuantityVariable)
		//                {
		//                    qtyVar = (eAssortmentQuantityVariables)coor.Key;
		//                }
		//                //Debug.WriteLine("ROW Grid " + gridTag.GridId + " " + coor.WaferCoordinateType + " " + coor.Key);
		//            }
		//            foreach (CubeWaferCoordinate coor in colCoorList)
		//            {
		//                if (coor.WaferCoordinateType == eProfileType.AssortmentDetailVariable)
		//                {
		//                    detVar = (eAssortmentDetailVariables)coor.Key;
		//                }
		//                else if (coor.WaferCoordinateType == eProfileType.AssortmentTotalVariable)
		//                {
		//                    totVar = (eAssortmentTotalVariables)coor.Key;
		//                }
		//                //Debug.WriteLine("COL Grid " + gridTag.GridId + " " + coor.WaferCoordinateType + " " + coor.Key);
		//            }

		//            if (detVar == eAssortmentDetailVariables.TotalPct || totVar == eAssortmentTotalVariables.TotalPct)
		//            {
		//                ComputationCellFlagValues.isDisplayOnly(ref cellFlags, true);
		//            }
		//        }
		//    }
		//    catch
		//    {
		//        throw;
		//    }

		//}
		// END TT#2327 - stodd - make Total % display only


		//void ChangeCellStyles(C1FlexGrid aGrid, CellTag aCellTag, int aRow, int aCol)
		//{
		//    CellStyleUserData userData;

		//    try
		//    {
		//        userData = (CellStyleUserData)aGrid.Rows[aRow].Style.UserData;

		//        if (userData == null)
		//        {
		//            throw new Exception("Invalid row style");
		//        }

		//        if (AssortmentCellFlagValues.isBlocked(aCellTag.ComputationCellFlags))
		//        {
		//            aGrid.SetCellStyle(aRow, aCol, userData.BlockedStyle);
		//        }
		//        else if (ComputationCellFlagValues.isLocked(aCellTag.ComputationCellFlags))
		//        {
		//            aGrid.SetCellStyle(aRow, aCol, userData.LockedStyle);
		//        }
		//        else if (aCellTag.isCellNegative)
		//        {
		//            if (ComputationCellFlagValues.isDisplayOnly(aCellTag.ComputationCellFlags) ||
		//                ComputationCellFlagValues.isNull(aCellTag.ComputationCellFlags) ||
		//                AssortmentCellFlagValues.isBlocked(aCellTag.ComputationCellFlags) ||
		//                AssortmentCellFlagValues.isFixed(aCellTag.ComputationCellFlags) ||
		//                ComputationCellFlagValues.isReadOnly(aCellTag.ComputationCellFlags))
		//            {
		//                aGrid.SetCellStyle(aRow, aCol, userData.NegativeStyle);
		//            }
		//            else
		//            {
		//                aGrid.SetCellStyle(aRow, aCol, userData.NegativeEditableStyle);
		//            }
		//        }
		//        else if (!ComputationCellFlagValues.isDisplayOnly(aCellTag.ComputationCellFlags) &&
		//               !ComputationCellFlagValues.isNull(aCellTag.ComputationCellFlags) &&
		//               !AssortmentCellFlagValues.isBlocked(aCellTag.ComputationCellFlags) &&
		//               !AssortmentCellFlagValues.isFixed(aCellTag.ComputationCellFlags) &&
		//               !ComputationCellFlagValues.isReadOnly(aCellTag.ComputationCellFlags))
		//        {
		//            aGrid.SetCellStyle(aRow, aCol, userData.EditableStyle);
		//        }
		//        else
		//        {
		//            aGrid.SetCellStyle(aRow, aCol, (CellStyle)null);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		// BEGIN TT#2504 - stodd - Assortment is blank after header is assigned

		/// <summary>
		/// For debugging.
		/// </summary>
		/// <param name="aGrid"></param>
		//void ReadGridPages(C1FlexGrid aGrid)
		//{
		//    PagingGridTag gridTag;
		//    ArrayList pages;
		//    Rectangle pageBound;

		//    gridTag = (PagingGridTag)aGrid.Tag;
		//    pages = gridTag.GetPagesToLoad(aGrid.TopRow, aGrid.LeftCol, Math.Min(aGrid.TopRow + ROWPAGESIZE - 1, aGrid.Rows.Count), Math.Min(aGrid.LeftCol + COLPAGESIZE - 1, aGrid.Cols.Count));

		//    if (pages.Count > 0)
		//    {
		//        foreach (Point page in pages)
		//        {
		//            pageBound = gridTag.GetPageBoundary(page);
		//            GetCellRange(aGrid, pageBound.X, pageBound.Y, pageBound.Width, pageBound.Height, 1);
		//        }
		//    }
		//}

		/// <summary>
		/// fore debugging.
		/// </summary>
		//private void ReadAllGridPages()
		//{
		//    try
		//    {

		//        ((PagingGridTag)g2.Tag).AllocatePageArray();
		//        ((PagingGridTag)g3.Tag).AllocatePageArray();
		//        ((PagingGridTag)g5.Tag).AllocatePageArray();
		//        ((PagingGridTag)g6.Tag).AllocatePageArray();
		//        ((PagingGridTag)g8.Tag).AllocatePageArray();
		//        ((PagingGridTag)g9.Tag).AllocatePageArray();

		//        if (g2.Rows.Count > 0 && g2.Cols.Count > 0)
		//        {
		//            Debug.WriteLine("Read Grid Pages for g2");
		//            ReadGridPages(g2);
		//            Debug.WriteLine("");
		//        }
		//        if (g3.Rows.Count > 0 && g3.Cols.Count > 0)
		//        {
		//            Debug.WriteLine("Read Grid Pages for g3");
		//            ReadGridPages(g3);
		//            Debug.WriteLine("");
		//        }
		//        if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
		//        {
		//            Debug.WriteLine("Read Grid Pages for g5");
		//            ReadGridPages(g5);
		//            Debug.WriteLine("");
		//        }
		//        if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
		//        {
		//            Debug.WriteLine("Read Grid Pages for g6");
		//            ReadGridPages(g6);
		//            Debug.WriteLine("");
		//        }
		//        if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
		//        {
		//            Debug.WriteLine("Read Grid Pages for g8");
		//            ReadGridPages(g8);
		//            Debug.WriteLine("");
		//        }
		//        if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
		//        {
		//            Debug.WriteLine("Read Grid Pages for g9");
		//            ReadGridPages(g9);
		//            Debug.WriteLine("");
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		private void readAllocationProfilesFromTrans()
		{
			Debug.WriteLine("READ FROM APs in TRANSACTION");
			// BEGIN TT#488-MD - STodd - Group Allocation 
			//AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
			AllocationProfileList apl = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.AssortmentMember);
			// END TT#488-MD - STodd - Group Allocation 
			foreach (AllocationProfile ahp in apl)
			{
				// BEGIN TT#488-MD - STodd - Group Allocation 
				//AllocationProfile allocProf = _transaction.GetAllocationProfile(ahp.Key);
				AllocationProfile allocProf = _transaction.GetAssortmentMemberProfile(ahp.Key);
				// END TT#488-MD - STodd - Group Allocation 
				if (allocProf != null)
				{
					//allocProf.GetAllocatedUnits(_storeProfileList);
					foreach (StoreProfile sp in _storeProfileList.ArrayList)
					{
						double val = allocProf.GetStoreItemQtyAllocated(new GeneralComponent(eGeneralComponentType.Total), sp.Key);
						if (val != 0)
						{
							Debug.WriteLine("FROM AP: " + allocProf.Key + " IN TRANS. Store: " + sp.Key + " value: " + val);
						}
					}
				}
			}
		}

		// END TT#2504 - stodd - Assortment is blank after header is assigned


		void SetLockPicture(C1FlexGrid aGrid, ComputationCellFlags aFlags, int aRow, int aCol)
		{
			try
			{
				if (ComputationCellFlagValues.isLocked(aFlags))
				{
					aGrid.SetCellImage(aRow, aCol, _picLock);
				}
				else
				{
					aGrid.SetCellImage(aRow, aCol, null);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//private void StopPageLoadThreads()
		//{
		//    try
		//    {
		//        _stopPageLoadThread = true;
		//        ((PagingGridTag)g5.Tag).WaitForPageLoads();
		//        ((PagingGridTag)g6.Tag).WaitForPageLoads();
		//        ((PagingGridTag)g8.Tag).WaitForPageLoads();
		//        ((PagingGridTag)g9.Tag).WaitForPageLoads();
		//        _stopPageLoadThread = false;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		#endregion

		#region Closing/Closed event handlers

		private void AssortmentView_Closing(object sender, CancelEventArgs e)
		{
			bool headerChanged;
			bool placeholderChanged;

			try
			{
				//BEGIN TT#442 - MD - DOConnell - After canceling a close event on an Assortment the right click menu does not work on the Content Tab
				if (!e.Cancel)
				{
					//StopPageLoadThreads();

                    CloseOtherViews(); //TT#598 - MD - DOConnell - When an assortment is closed, any style or size review windows should also be closed.

					//headerChanged = ((AssortmentCubeGroup)_asrtCubeGroup).hasHeaderCubeChanged();
					//placeholderChanged = ((AssortmentCubeGroup)_asrtCubeGroup).hasPlaceholderCubeChanged();

					//if (headerChanged || placeholderChanged)
					//{
					//    _asrtCubeGroup.SaveCubeGroup();
					//}

					// RMatelic UserAssortment data is saved via the save dialog 
					//_dlUserAssrt.UserAssortment_Update(_sab.ClientServerSession.UserRID, _openParms);

					// BEGIN TT#696-MD - Stodd - add "active process"
					AssortmentActiveProcessToolbarHelper.RemoveAssortmentReviewScreen(this.Text, _assortmentRid);
					// END TT#696-MD - Stodd - add "active process"

					//_asrtCubeGroup.Dispose();

					_transaction.AssortmentView = null;
					_transaction.CheckForHeaderDequeue();
				}
				//END TT#442 - MD - DOConnell - After canceling a close event on an Assortment the right click menu does not work on the Content Tab
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//public object g4GetMergeData(int row, int col)
		//{
		//    string data;
		//    int i;

		//    try
		//    {
		//        data = "";

		//        for (i = 0; i < col; i++)
		//        {
		//            data += g4.GetDataDisplay(row, i);
		//        }

		//        data += g4.GetDataDisplay(row, col);

		//        return data;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		#endregion

		#region Class/Structure Declarations

		//private class CellStyleUserData
		//{
		//    private CellStyle _blockedStyle;
		//    private CellStyle _lockedStyle;
		//    private CellStyle _negativeStyle;
		//    private CellStyle _editableStyle;
		//    private CellStyle _negativeEditableStyle;

		//    public CellStyleUserData(CellStyle aBlockedStyle, CellStyle aLockedStyle, CellStyle aNegativeStyle, CellStyle aEditableStyle, CellStyle aNegativeEditableStyle)
		//    {
		//        _blockedStyle = aBlockedStyle;
		//        _lockedStyle = aLockedStyle;
		//        _negativeStyle = aNegativeStyle;
		//        _editableStyle = aEditableStyle;
		//        _negativeEditableStyle = aNegativeEditableStyle;
		//    }

		//    public CellStyle BlockedStyle
		//    {
		//        get
		//        {
		//            return _blockedStyle;
		//        }
		//    }

		//    public CellStyle LockedStyle
		//    {
		//        get
		//        {
		//            return _lockedStyle;
		//        }
		//    }

		//    public CellStyle NegativeStyle
		//    {
		//        get
		//        {
		//            return _negativeStyle;
		//        }
		//    }

		//    public CellStyle EditableStyle
		//    {
		//        get
		//        {
		//            return _editableStyle;
		//        }
		//    }

		//    public CellStyle NegativeEditableStyle
		//    {
		//        get
		//        {
		//            return _negativeEditableStyle;
		//        }
		//    }
		//}

		private class ActionTag
		{
			int _key;
			string _text;

			public ActionTag(int aKey, string aText)
			{
				_key = aKey;
				_text = aText;
			}

			public int Key
			{
				get
				{
					return _key;
				}
			}

			public string Text
			{
				get
				{
					return _text;
				}
			}
		}

		//private class GridRow : IComparable
		//{
		//    string[] _textCols;
		//    string[] _keyCols;
		//    string[] _textArray;
		//    int[] _keyArray;
		//    int[] _seqArray;	// tt#1322 - STODD
		//    DataRow _dataRow;	// TT#1322 - stodd
		//    // Begin TT#1438 - stodd - sorting issue
		//    // Placeholder Seq concatenated with Header Seq
		//    string _phSeqHdrSeq;
		//    // end TT#1438 - stodd - sorting issue
		//    int _compSeqCount;	// TT#2502 - DOConnell - Nested Characteristics in Assortment not correct
		//    int i;

		//    public GridRow(DataRow aDataRow, string[] aTextCols, string[] aKeyCols)
		//    {
		//        int i;

		//        try
		//        {
		//            _textCols = aTextCols;
		//            _keyCols = aKeyCols;
		//            _dataRow = aDataRow;	// TT#1322 - stodd
		//            _compSeqCount = 0;      // TT#2502 - DOConnell - Nested Characteristics in Assortment not correct

		//            _textArray = new string[aTextCols.Length];
		//            _keyArray = new int[aKeyCols.Length];
		//            _seqArray = new int[aKeyCols.Length];	// tt#1322 - STODD

		//            _phSeqHdrSeq = Convert.ToString(aDataRow["HEADERSEQ"]);

		//            for (i = 0; i < aTextCols.Length; i++)
		//            {
		//                if (aDataRow[aTextCols[i]] == DBNull.Value)
		//                {
		//                    _textArray[i] = " ";
		//                }
		//                else
		//                {
		//                    _textArray[i] = Convert.ToString(aDataRow[aTextCols[i]]);
		//                }

		//                _keyArray[i] = Convert.ToInt32(aDataRow[aKeyCols[i]]);
		//            }
		//            //BEGIN TT#2502 - DOConnell - Nested Characteristics in Assortment not correct
		//            for (i = 0; i < _keyArray.Length; i++)
		//            {
		//                // BEGIN TT#2503 - stodd - Cannot key in Avg Str Units in Assortment Tab
		//                // This change here was actually to fix a sorting issue when no characteristics were present
		//                if (_textCols[i] != "PLACEHOLDER" && _textCols[i] != "HEADER" && _textCols[i] != "PACK"
		//                    && _textCols[i] != "COLOR" && !_textCols[i].StartsWith("HIERARCHYLEVEL")
		//                    && _textCols[i] != "ASSORTMENT" && _textCols[i] != "PLANLEVEL")
		//                {
		//                    _compSeqCount = _compSeqCount + 1;
		//                }
		//                // END TT#2503 - stodd - Cannot key in Avg Str Units in Assortment Tab
		//            }
		//            //END TT#2502 - DOConnell - Nested Characteristics in Assortment not correct
		//        }
		//        catch (Exception exc)
		//        {
		//            string message = exc.ToString();
		//            throw;
		//        }
		//    }

		//    public string[] TextCols
		//    {
		//        get
		//        {
		//            return _textCols;
		//        }
		//    }

		//    public string[] KeyCols
		//    {
		//        get
		//        {
		//            return _keyCols;
		//        }
		//    }

		//    public string[] TextArray
		//    {
		//        get
		//        {
		//            return _textArray;
		//        }
		//    }

		//    public int[] KeyArray
		//    {
		//        get
		//        {
		//            return _keyArray;
		//        }
		//    }

		//    // Begin TT#1438 - stodd - sorting issue
		//    /// <summary>
		//    /// Placeholder Seq concatenated with Header Seq.
		//    /// </summary>
		//    public string PlaceholderSeqHeaderSeq
		//    {
		//        get
		//        {
		//            return _phSeqHdrSeq;
		//        }
		//    }
		//    // End TT#1438 - stodd - sorting issue


		//    public DataRow DataRow
		//    {
		//        get
		//        {
		//            return _dataRow;
		//        }
		//    }

		//    public int CompareTo(object obj)
		//    {
		//        GridRow gridRow;
		//        int retCode;

		//        try
		//        {
		//            gridRow = (GridRow)obj;
		//            retCode = 0;

		//            for (i = 0; i < _textArray.Length && retCode == 0; i++)
		//            {
		//                if (_keyArray[i] == int.MaxValue && gridRow._keyArray[i] != int.MaxValue)
		//                {
		//                    retCode = -1;
		//                }
		//                else if (_keyArray[i] != int.MaxValue && gridRow._keyArray[i] == int.MaxValue)
		//                {
		//                    retCode = 1;
		//                }
		//                else if (_keyArray[i] == int.MaxValue && gridRow._keyArray[i] == int.MaxValue)
		//                {
		//                    // Begin TT#1461 - stodd - sorting issue
		//                    //retCode = 0;
		//                    string data = _textArray[i] + _phSeqHdrSeq;
		//                    string gridRowData = gridRow._textArray[i] + gridRow.PlaceholderSeqHeaderSeq;
		//                    retCode = data.CompareTo(gridRowData);
		//                    // End TT#1461 - stodd - sorting issue
		//                }
		//                else
		//                {
		//                    // Begin TT#1322 - stodd - sorting
		//                    if (_textCols[i] == "PLACEHOLDER")
		//                    {
		//                        // Begin TT#1461 - stodd - sorting issue
		//                        // Begin TT#1335 - stodd more sorting issues
		//                        int iSeq = int.Parse(_dataRow["PLACEHOLDERSEQ_RID"].ToString());
		//                        string seq = iSeq.ToString("0000") + _phSeqHdrSeq;
		//                        int iGridSeq = int.Parse(gridRow.DataRow["PLACEHOLDERSEQ_RID"].ToString());
		//                        string gridRowSeq = iGridSeq.ToString("0000") + gridRow.PlaceholderSeqHeaderSeq;
		//                        // End TT#1335 - stodd more sorting issues
		//                        //int seq = Convert.ToInt32(_dataRow["PLACEHOLDERSEQ_RID"]);
		//                        //int gridRowSeq = Convert.ToInt32(gridRow.DataRow["PLACEHOLDERSEQ_RID"]);
		//                        // End TT#1461 - stodd - sorting issue
		//                        retCode = seq.CompareTo(gridRowSeq);
		//                    }
		//                    else if (_textCols[i] == "HEADER")
		//                    {
		//                        // Begin TT#1461 - stodd - sorting issue
		//                        // Begin TT#1335 - stodd more sorting issues
		//                        int iSeq = int.Parse(_dataRow["HEADERSEQ_RID"].ToString());
		//                        string seq = iSeq.ToString("0000") + _phSeqHdrSeq;
		//                        int iGridSeq = int.Parse(gridRow.DataRow["HEADERSEQ_RID"].ToString());
		//                        string gridRowSeq = iGridSeq.ToString("0000") + gridRow.PlaceholderSeqHeaderSeq;
		//                        // End TT#1335 - stodd more sorting issues
		//                        //int seq = Convert.ToInt32(_dataRow["HEADERSEQ_RID"]);
		//                        //int gridRowSeq = Convert.ToInt32(gridRow.DataRow["HEADERSEQ_RID"]);
		//                        // End TT#1461 - stodd - sorting issue
		//                        retCode = seq.CompareTo(gridRowSeq);
		//                    }
		//                    else
		//                    {
		//                        //BEGIN TT#2502 - DOConnell - Nested Characteristics in Assortment not correct
		//                        string gridRowData = null;
		//                        string data = null;
		//                        // Begin TT#1438 - stodd - sorting issue
		//                        // To keep like colors/packs distinct, append phSeqHdrSeq.
		//                        // BEGIN TT#2503 - stodd - Cannot key in Avg Str Units in Assortment Tab
		//                        // This change here was actually to fix a sorting issue when no characteristics were present
		//                        if (_compSeqCount > 0)
		//                        {
		//                            for (int a = 0; a < _compSeqCount; a++)
		//                            {
		//                                data = data + _textArray[a];
		//                                gridRowData = gridRowData + gridRow._textArray[a];
		//                            }
		//                            data = data + _phSeqHdrSeq;
		//                            gridRowData = gridRowData + gridRow.PlaceholderSeqHeaderSeq;
		//                        }
		//                        else
		//                        {
		//                            data = _textArray[i] + _phSeqHdrSeq;
		//                            gridRowData = gridRow._textArray[i] + gridRow.PlaceholderSeqHeaderSeq;
		//                        }
		//                        //END TT#2502 - DOConnell - Nested Characteristics in Assortment not correct
		//                        // BEGIN TT#2503 - stodd - Cannot key in Avg Str Units in Assortment Tab

		//                        retCode = data.CompareTo(gridRowData);
		//                        //retCode = _textArray[i].CompareTo(gridRow._textArray[i]);
		//                        // End TT#1438 - stodd - sorting issue
		//                    }
		//                    // End TT#1322 - stodd - sorting
		//                }
		//            }

		//            return retCode;
		//        }
		//        catch (Exception exc)
		//        {
		//            string message = exc.ToString();
		//            throw;
		//        }
		//    }
		//}

		#endregion

		// Begin TT#1262 - stodd - link total units between matrix grid and content grid
		//private void g5_CellChanged(object sender, RowColEventArgs e)
		//{
		//    if (FormLoaded)
		//    {
		//        int hdrRid = int.MaxValue;
		//        int packRid = int.MaxValue;
		//        int colorRid = int.MaxValue;
		//        double qty = 0;

		//        C1.Win.C1FlexGrid.Column col = g5.Cols[e.Col];
		//        C1.Win.C1FlexGrid.Row row = g5.Rows[e.Row];
		//        PagingGridTag gridTag = (PagingGridTag)g5.Tag;

		//        ColumnHeaderTag colHeaderTag = (ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[e.Col].UserData;
		//        if (colHeaderTag != null)
		//        {
		//            if (colHeaderTag.CubeWaferCoorList != null && colHeaderTag.CubeWaferCoorList.Count > e.Col)
		//            {
		//                CubeWaferCoordinate colCubeWaferCoord = (CubeWaferCoordinate)colHeaderTag.CubeWaferCoorList[e.Col];
		//                if (colCubeWaferCoord.Key == (int)eAssortmentTotalVariables.TotalUnits)
		//                {
		//                    object cellObj = g5[e.Row, e.Col];
		//                    // Only bother with the below if the cell has a valid value
		//                    if (cellObj != null)
		//                    {
		//                        try
		//                        {
		//                            qty = double.Parse(cellObj.ToString());

		//                        }
		//                        catch
		//                        {
		//                            return;
		//                        }

		//                        CellTag cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[e.Row].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[e.Col].UserData).Order];
		//                        bool isHeader = false;
		//                        // BEGIN TT#2324 - stodd - qty less than zero error
		//                        bool isDifference = false;
		//                        // END TT#2324 - stodd - qty less than zero error


		//                        foreach (CubeWaferCoordinate wafCoor in ((RowHeaderTag)gridTag.RowHeaderGrid.Rows[e.Row].UserData).CubeWaferCoorList)
		//                        {
		//                            // Begin TT#2 - RMatelic - Assortment Planning - Matrix header quantities not updating values on content grid
		//                            //if (wafCoor.WaferCoordinateType == eProfileType.PlaceholderHeader)
		//                            //{
		//                            //    hdrRid = wafCoor.Key;
		//                            //}
		//                            //if (wafCoor.WaferCoordinateType == eProfileType.AllocationHeader
		//                            //    && wafCoor.Key != int.MaxValue)
		//                            //{
		//                            //    isHeader = true;
		//                            //}
		//                            //if (wafCoor.WaferCoordinateType == eProfileType.HeaderPack)
		//                            //{
		//                            //    packRid = wafCoor.Key;
		//                            //}
		//                            //if (wafCoor.WaferCoordinateType == eProfileType.HeaderPackColor)
		//                            //{
		//                            //    colorRid = wafCoor.Key;
		//                            //}
		//                            switch (wafCoor.WaferCoordinateType)
		//                            {
		//                                case eProfileType.PlaceholderHeader:
		//                                    hdrRid = wafCoor.Key;
		//                                    break;

		//                                case eProfileType.AllocationHeader:
		//                                    if (wafCoor.Key != int.MaxValue)
		//                                    {
		//                                        hdrRid = wafCoor.Key;
		//                                    }
		//                                    break;

		//                                case eProfileType.HeaderPack:
		//                                    packRid = wafCoor.Key;
		//                                    break;

		//                                case eProfileType.HeaderPackColor:
		//                                    colorRid = wafCoor.Key;
		//                                    break;

		//                                // BEGIN TT#2324 - stodd - qty less than zero error 
		//                                case eProfileType.AssortmentQuantityVariable:
		//                                    if (wafCoor.Key == (int)eAssortmentQuantityVariables.Difference)
		//                                    {
		//                                        isDifference = true;
		//                                    }
		//                                    break;
		//                                // END TT#2324 - stodd - qty less than zero error

		//                            }
		//                            // End TT#2
		//                        }
		//                        // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values 
		//                        // We only want to update Placeholder values.
		//                        //if (isHeader)
		//                        //{
		//                        //    hdrRid = int.MaxValue;
		//                        //    packRid = int.MaxValue;
		//                        //    colorRid = int.MaxValue;
		//                        //}
		//                        // End TT#2

		//                        // BEGIN TT#2324 - stodd - qty less than zero error
		//                        if (hdrRid != int.MaxValue && !isDifference)
		//                        {
		//                            UpdateQuantityFromAssortmentGrid(hdrRid, packRid, colorRid, qty);
		//                        }
		//                        // END TT#2324 - stodd - qty less than zero error
		//                    }
		//                }
		//            }
		//        }
		//    }
		//    // End TT#1262 - stodd - link total units between matrix grid and content grid
		//}

		//BEGIN TT#3 -MD- DOConnell - Export does not produce output
		#region ExcelExport
		//private void XLExport()
		//{
		//    MIDExportFile MIDExportFile = null;
		//    try
		//    {
		//        MIDExport MIDExport = null;
		//        //Begin Track #5006 - JScott - Display Low-levels one at a time
		//        //if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel ||
		//        //    _openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
		//        //if (_currentPlanSessionType == ePlanSessionType.StoreSingleLevel ||
		//        //    _currentPlanSessionType == ePlanSessionType.StoreMultiLevel)
		//        ////End Track #5006 - JScott - Display Low-levels one at a time
		//        //{
		//        //    MIDExport = new MIDExport(SAB, _includeCurrentSetLabel, _includeAllSetsLabel, true);
		//        //}
		//        //else
		//        //{
		//        //    MIDExport = new MIDExport(SAB, null, null, false);
		//        //}

		//        MIDExport = new MIDExport(SAB, null, null, false);

		//        MIDExport.AddFileFilter(eExportFilterType.Excel);
		//        MIDExport.AddFileFilter(eExportFilterType.CSV);
		//        MIDExport.AddFileFilter(eExportFilterType.XML);
		//        MIDExport.AddFileFilter(eExportFilterType.All);

		//        MIDExport.ShowDialog();
		//        if (!MIDExport.OKClicked)
		//        {
		//            return;
		//        }
		//        Cursor.Current = Cursors.WaitCursor;
		//        SetGridRedraws(false);
		//        StopPageLoadThreads();
		//        string fileName = null;
		//        if (MIDExport.OpenExcel)
		//        {
		//            // generate unique name
		//            fileName = Application.LocalUserAppDataPath + @"\mid" + DateTime.Now.Ticks.ToString();
		//            if (MIDExport.IncludeFormatting)
		//            {
		//                fileName += ".xls";
		//            }
		//            else
		//            {
		//                fileName += ".xml";
		//            }
		//        }
		//        else
		//        {
		//            fileName = MIDExport.FileName;
		//        }

		//        switch (MIDExport.ExportType)
		//        {
		//            case eExportType.Excel:
		//                if (MIDExport.IncludeFormatting)
		//                {
		//                    MIDExportFile = new MIDExportFlexGridToExcel(fileName, MIDExport.IncludeFormatting);
		//                }
		//                else
		//                {
		//                    MIDExportFile = new MIDExportFlexGridToXML(fileName, MIDExport.IncludeFormatting);
		//                }
		//                break;
		//            case eExportType.XML:
		//                MIDExportFile = new MIDExportFlexGridToXML(fileName, MIDExport.IncludeFormatting);
		//                break;
		//            default:
		//                MIDExportFile = new MIDExportFlexGridToCSV(fileName, MIDExport.IncludeFormatting);
		//                break;
		//        }

		//        if (MIDExportFile != null)
		//        {
		//            // delete file if it's already there
		//            if (File.Exists(MIDExportFile.FileName))
		//            {
		//                File.Delete(MIDExportFile.FileName);
		//            }
		//            MIDExportFile.OpenFile();
		//            // add styles to XML style sheet
		//            if (MIDExportFile.ExportType == eExportType.XML)
		//            {
		//                ExportSpecificStyle(MIDExportFile, "g2", g2, "ColumnHeading");
		//                ExportSpecificStyle(MIDExportFile, "g3", g3, "ColumnHeading");
		//                //ExportSpecificStyle(MIDExportFile, "g4", g4, "Style1");
		//                //ExportSpecificStyle(MIDExportFile, "g7", g7, "Reverse1");
		//                ExportSpecificStyle(MIDExportFile, "g4", g4, "Normal");
		//                ExportSpecificStyle(MIDExportFile, "g7", g7, "Alternate");
		//                //ExportSpecificStyle(MIDExportFile, "g10", g10, "Style1");
		//                MIDExportFile.NoMoreStyles();
		//            }
		//            if (MIDExport.ExportData == eExportData.Current)
		//            {
		//                ExportCurrentFile(MIDExportFile);
		//            }
		//            else
		//            {
		//                ExportAllFile(MIDExportFile);
		//            }
		//            if (MIDExport.OpenExcel)
		//            {
		//                Process process = new Process();
		//                process.StartInfo.FileName = "Excel.exe";
		//                process.StartInfo.Arguments = @"""" + MIDExportFile.FileName + @"""";
		//                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
		//                process.Start();
		//            }
		//        }
		//    }
		//    catch (IOException IOex)
		//    {
		//        MessageBox.Show(IOex.Message);
		//        return;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//    finally
		//    {
		//        if (MIDExportFile != null)
		//        {
		//            MIDExportFile.WriteFile();
		//        }
		//        //SetGridRedraws(true);
		//        Cursor.Current = Cursors.Default;
		//    }
		//}

		private void ExportCurrentFile(MIDExportFile aMIDExportFile)
		{
			try
			{
				int detailsPerGroup = 0;
				//SetGridRedraws(false);

				//LoadAllPages();

				StoreGroupLevelProfile sglProf = null;
				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel ||
				//    _openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
				//TT#2195
				//if (_currentPlanSessionType == ePlanSessionType.StoreSingleLevel ||
				//    _currentPlanSessionType == ePlanSessionType.StoreMultiLevel)
				////End Track #5006 - JScott - Display Low-levels one at a time
				//{
				//sglProf = (StoreGroupLevelProfile)_storeGroupLevelProfileList.FindKey(_transaction.AllocationStoreAttributeID);
				//detailsPerGroup = ((PagingGridTag)g7.Tag).DetailsPerGroup;
				//}
				//TT#2195
				//Begin Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
				//ExportData(aMIDExportFile, sglProf, detailsPerGroup, true, true, false, _workingDetailProfileList);
				//ExportData(aMIDExportFile, _workingDetailProfileList, sglProf, detailsPerGroup, true, true, false);
				//End Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
			}
			catch
			{
				throw;
			}
		}

		private void ExportAllFile(MIDExportFile aMIDExportFile)
		{
			try
			{
				bool addHeader = true;
				bool addSummary = true;
				int setCount = 0;
				//SetGridRedraws(false);

				//LoadAllPages();

				foreach (StoreGroupLevelProfile sglProf in _storeGroupLevelProfileList)
				{
					++setCount;
					if (aMIDExportFile.ExportType == eExportType.CSV)
					{
						// only add summary at end of CSV file
						if (setCount == _storeGroupLevelProfileList.Count)
						{
							addSummary = true;
						}
						else
						{
							addSummary = false;
						}
					}

					//Begin Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
					//ExportData(aMIDExportFile, sglProf, ((PagingGridTag)g7.Tag).DetailsPerGroup,
					ProfileList workList = new ProfileList(eProfileType.Store);
					//BuildWorkingStoreList(sglProf.Key, workList);

					// sort the stores
					SortedList stores = new SortedList();
					ArrayList sortedStores = new ArrayList();
					foreach (StoreProfile storeProfile in workList)
					{
						stores.Add(storeProfile.Text, storeProfile);
					}

					ProfileList exportProfileList = new ProfileList(eProfileType.Store);
					foreach (StoreProfile storeProfile in stores.Values)
					{
						exportProfileList.Add(storeProfile);
					}

					//ExportData(aMIDExportFile, exportProfileList, sglProf, ((PagingGridTag)g7.Tag).DetailsPerGroup,
					//    //End Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
					//    addHeader, addSummary, true);
					// only add heading at beginning of CSV file
					if (aMIDExportFile.ExportType == eExportType.CSV)
					{
						addHeader = false;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		//Begin Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
		//private void ExportData(MIDExportFile aMIDExportFile, StoreGroupLevelProfile sglProf, int aRowsPerGroup,
		//private void ExportData(MIDExportFile aMIDExportFile, ProfileList aDetailProfileList, StoreGroupLevelProfile sglProf, int aRowsPerGroup,
		//    //End Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
		//    bool aAddHeader, bool aAddSummary, bool aExportingAll)
		//{
		//    try
		//    {
		//        string negativeStyle = null;
		//        string textStyle = null;
		//        int i;
		//        int j;
		//        int k;
		//        int totCols;
		//        string groupingName = "sheet1";
		//        //PlanWaferCell[,] totalPlanWaferCellTable = null;
		//        WaferCell[,] totalPlanWaferCellTable;
		//        CubeWafer totalCubeWafer;
		//        //PlanWaferCell[,] detailPlanWaferCellTable = null;
		//        WaferCell[,] detailPlanWaferCellTable;
		//        CubeWafer detailCubeWafer;
		//        object[,] setvalueArray;
		//        ExcelGridInfo[,] setformatArray;
		//        C1FlexGrid exportG4 = new C1FlexGrid();
		//        VariableProfile varProf;
		//        int currVarProfKey = Include.Undefined;

		//        //Begin Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
		//        //ProfileList exportProfileList = null;
		//        //End Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
		//        if (sglProf != null) // store
		//        {
		//            //Begin Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
		//            //ProfileList workList = new ProfileList(eProfileType.Store);
		//            //BuildWorkingStoreList(sglProf.Key, workList);
		//            //// sort the stores
		//            //SortedList stores = new SortedList();
		//            //ArrayList sortedStores = new ArrayList();
		//            //foreach (StoreProfile storeProfile in workList)
		//            //{
		//            //    stores.Add(storeProfile.Text, storeProfile);
		//            //}
		//            //exportProfileList = new ProfileList(eProfileType.Store);
		//            //foreach (StoreProfile storeProfile in stores.Values)
		//            //{
		//            //    exportProfileList.Add(storeProfile);
		//            //}
		//            //Formatg4Grid(true, exportG4, exportProfileList, false);
		//            //Formatg4Grid(true, exportG4, aDetailProfileList, false);
		//            //Formatg4Grid(false);
		//            //End Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
		//            groupingName = sglProf.Name;
		//        }
		//        else // chain
		//        {
		//            exportG4 = g4;
		//        }
		//        //TT#2195
		//        //int rowsCount = g3.Rows.Count + exportG4.Rows.Count + ((PagingGridTag)g7.Tag).DetailsPerGroup + g10.Rows.Count;
		//        //int rowsCount = g3.Rows.Count + exportG4.Rows.Count + ((PagingGridTag)g7.Tag).DetailsPerGroup;
		//        //int rowsCount = g3.Rows.Count + exportG4.Rows.Count + g7.Rows.Count;
		//        //int columnsCount = exportG4.Cols.Count + g2.Cols.Count + g3.Cols.Count;

		//        totCols = exportG4.Cols.Count + g2.Cols.Count + g3.Cols.Count;

		//        setvalueArray = new object[aRowsPerGroup, totCols];
		//        setformatArray = new ExcelGridInfo[aRowsPerGroup, totCols];

		//        aMIDExportFile.AddGrouping(groupingName, totCols);

		//        aMIDExportFile.SetNumberRowsColumns(rowsCount, columnsCount);

		//        if (aMIDExportFile.IncludeFormatting &&
		//            aMIDExportFile.ExportType == eExportType.Excel)
		//        {
		//            ExportApplyFormatting(aMIDExportFile, rowsCount, columnsCount, exportG4);
		//        }

		//        if (aAddHeader)
		//        {
		//            // add headings
		//            for (i = 0; i < g3.Rows.Count; i++)
		//            {
		//                aMIDExportFile.AddRow();
		//                // insert blank columns to line up values
		//                negativeStyle = null;
		//                textStyle = "g2ColumnHeading";

		//                //for (j = 0; j < g1.Cols.Count; j++)
		//                //{
		//                //    if (g1[i, j] != null && j == 0)
		//                //    {
		//                //        aMIDExportFile.AddValue(g1[i, j].ToString(), eExportDataType.ColumnHeading, textStyle, null);
		//                //    }
		//                //    else
		//                //    {
		//                //        aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
		//                //    }
		//                //}
		//                for (j = 0; j < g1.Cols.Count; j++)
		//                {
		//                    if (g1.Cols[j].Visible)
		//                    {
		//                        if (i < g3.Rows.Count - 1 ||
		//                            g1[0, j] == null)
		//                        {
		//                            aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
		//                        }
		//                        else
		//                        {
		//                            aMIDExportFile.AddValue(g1[0, j].ToString(), eExportDataType.ColumnHeading, textStyle, null);
		//                        }
		//                    }
		//                }

		//                //for (j = 0; j < g2.Cols.Count; j++)
		//                //{
		//                //    if (g2[i, j] != null)
		//                //    {
		//                //        aMIDExportFile.AddValue(g2[i, j].ToString(), eExportDataType.ColumnHeading, textStyle, null);
		//                //    }
		//                //    else
		//                //    {
		//                //        aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
		//                //    }
		//                //}
		//                // add total columns
		//                //negativeStyle = null;

		//                textStyle = "g2ColumnHeading";

		//                //if (pnlTotals.Width > 0)
		//                //{
		//                for (j = 0; j < g2.Cols.Count; j++)
		//                {
		//                    if (g2.Cols[j].Visible)
		//                    {
		//                        if (g2[i, j] != null)
		//                        {
		//                            aMIDExportFile.AddValue(g2[i, j].ToString(), eExportDataType.ColumnHeading, textStyle, null);
		//                        }
		//                        else
		//                        {
		//                            aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
		//                        }
		//                    }
		//                }
		//                //}

		//                //negativeStyle = null;
		//                //textStyle = "g3ColumnHeading";
		//                //int sgpCount = _storeGradeProfileList.Count;
		//                //for (j = 0; j < g3.Cols.Count; j++)
		//                //{
		//                //    if (g3[i, j] != null)
		//                //    {
		//                //        aMIDExportFile.AddValue(g3[i, j].ToString(), eExportDataType.ColumnHeading, textStyle, negativeStyle);
		//                //    }
		//                //    else
		//                //    {
		//                //        aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, negativeStyle);
		//                //    }
		//                //}

		//                textStyle = "g3ColumnHeading";

		//                for (j = 0; j < g3.Cols.Count; j++)
		//                {
		//                    if (g3.Cols[j].Visible)
		//                    {
		//                        if (g3[i, j] != null)
		//                        {
		//                            aMIDExportFile.AddValue(g3[i, j].ToString(), eExportDataType.ColumnHeading, textStyle, null);
		//                        }
		//                        else
		//                        {
		//                            aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
		//                        }
		//                    }
		//                }


		//                aMIDExportFile.WriteRow();
		//            }
		//        }//end Headings

		//        for (j = 0; j < g3.Cols.Count; j++)
		//        {
		//            if (g3.Cols[j].Visible)
		//            {
		//                totCols += 1;
		//            }
		//        }
		//        aMIDExportFile.AddRow();
		//        negativeStyle = null;
		//        textStyle = "g3ColumnHeading";
		//        for (j = 0; j < g4.Cols.Count; j++)
		//        {
		//            if (g4.Cols[j].Visible)
		//            {
		//                if (g4[0, j] != null)
		//                {
		//                    aMIDExportFile.AddValue(g4[0, j].ToString(), eExportDataType.ColumnHeading, textStyle, negativeStyle);
		//                }
		//                else
		//                {
		//                    aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, negativeStyle);
		//                }
		//            }
		//        }
		//        negativeStyle = null;
		//        textStyle = "g3ColumnHeading";
		//        for (j = 0; j < g5.Cols.Count; j++)
		//        {
		//            if (g5.Cols[j].Visible)
		//            {
		//                aMIDExportFile.AddValue(g5[0, j].ToString(), eExportDataType.ColumnHeading, textStyle, negativeStyle);
		//            }
		//        }
		//        negativeStyle = null;
		//        textStyle = "g3ColumnHeading";
		//        for (j = 0; j < g6.Cols.Count; j++)
		//        {
		//            if (g6.Cols[j].Visible)
		//            {
		//                aMIDExportFile.AddValue(g6[0, j].ToString(), eExportDataType.ColumnHeading, textStyle, negativeStyle);
		//            }
		//        }
		//        aMIDExportFile.WriteRow();


		//        // add stores for set or chain values
		//        if (exportG4.Rows.Count > 0)
		//        {
		//            //Create the CubeWafers to request data
		//            totalCubeWafer = new CubeWafer();
		//            totalPlanWaferCellTable = ExportGetValues(totalCubeWafer, g5, exportG4);

		//            //Create the CubeWafers to request data
		//            detailCubeWafer = new CubeWafer();
		//            detailPlanWaferCellTable = ExportGetValues(detailCubeWafer, g6, exportG4);


		//            for (i = 1; i < exportG4.Rows.Count; i++)
		//            {
		//                aMIDExportFile.AddRow();
		//                // add row headings
		//                negativeStyle = null;
		//                //textStyle = "g4Style1";
		//                textStyle = "g4Normal";
		//                for (j = 0; j < exportG4.Cols.Count; j++)
		//                {
		//                    if (exportG4.Cols[j].Visible)
		//                    {
		//                        if (exportG4[i, j] != null)
		//                        {
		//                            aMIDExportFile.AddValue(exportG4[i, j].ToString(), eExportDataType.RowHeading, textStyle, negativeStyle);
		//                        }
		//                        else
		//                        {
		//                            aMIDExportFile.AddValue(" ", eExportDataType.RowHeading, textStyle, negativeStyle);
		//                        }
		//                    }
		//                }

		//                if (totalPlanWaferCellTable != null)
		//                {
		//                    negativeStyle = "g5Negative1";
		//                    textStyle = "g5Style1";
		//                    for (j = 0; j < totalPlanWaferCellTable.GetLength(1); j++)
		//                    {
		//                        if (totalPlanWaferCellTable[i, j] != null)
		//                        {
		//                            aMIDExportFile.AddValue(totalPlanWaferCellTable[i, j].ValueAsString, totalPlanWaferCellTable[i, j].isValueNumeric,
		//                                totalPlanWaferCellTable[i, j].isValueNegative, totalPlanWaferCellTable[i, j].NumberOfDecimals,
		//                                eExportDataType.Value, textStyle, negativeStyle);
		//                        }
		//                        else
		//                        {
		//                            aMIDExportFile.AddValue(" ", false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
		//                        }
		//                    }
		//                }
		//                // add detail columns
		//                negativeStyle = "g6Negative1";
		//                textStyle = "g6Style1";
		//                for (j = 0; j < detailPlanWaferCellTable.GetLength(1); j++)
		//                {
		//                    if (detailPlanWaferCellTable[i, j] != null)
		//                    {
		//                        aMIDExportFile.AddValue(detailPlanWaferCellTable[i, j].ValueAsString, detailPlanWaferCellTable[i, j].isValueNumeric,
		//                            detailPlanWaferCellTable[i, j].isValueNegative, detailPlanWaferCellTable[i, j].NumberOfDecimals,
		//                            eExportDataType.Value, textStyle, negativeStyle);
		//                    }
		//                    else
		//                    {
		//                        aMIDExportFile.AddValue(" ", false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
		//                    }
		//                }
		//                aMIDExportFile.WriteRow();
		//            }
		//        }

		//        // add set values
		//        //BEGIN TT#624 - MD - DOConnell - Export the Assortment to Excel and the Total and Balance row are cut off
		//        int numRows = g7.Rows.Count - 1;
		//        for (i = 0; i <= numRows; i++)
		//        //for (i = 0; i < g7.Rows.Count; i++)
		//        //END TT#624 - MD - DOConnell - Export the Assortment to Excel and the Total and Balance row are cut off
		//        {
		//            //if (((RowHeaderTag)g7.Rows[i].UserData).GroupRowColHeader.Profile.Key == sglProf.Key)
		//            //{
		//            int row = i;
		//            //for (k = 0; k <= ((PagingGridTag)g7.Tag).DetailsPerGroup; k++) //TT#624 - MD - DOConnell - Export the Assortment to Excel and the Total and Balance row are cut off
		//            //{
		//                aMIDExportFile.AddRow();
		//                // add row headings
		//                negativeStyle = null;
		//                //textStyle = "g7Reverse1";
		//                textStyle = "g7Alternate";
		//                for (j = 0; j < g7.Cols.Count; j++)
		//                {
		//                    if (g7.Cols[j].Visible)
		//                    {
		//                        if (g7[row, j] != null || Convert.ToString(g7[row, j], CultureInfo.CurrentUICulture) == " ")
		//                        //if (g7[row, j] != null) TT#624 - MD - DOConnell - Export the Assortment to Excel and the Total and Balance row are cut off
		//                        {
		//                            aMIDExportFile.AddValue(g7[row, j].ToString(), eExportDataType.RowHeading, textStyle, negativeStyle);
		//                        }
		//                        else
		//                        {
		//                            aMIDExportFile.AddValue(" ", eExportDataType.RowHeading, textStyle, negativeStyle);
		//                        }
		//                    }
		//                }
		//                // add total columns
		//                negativeStyle = "g8Negative1";
		//                textStyle = "g8Editable1";
		//                for (j = 0; j < g8.Cols.Count; j++)
		//                {
		//                    if (g8.Cols[j].Visible)
		//                    {
		//                        if (g8[row, j] != null || Convert.ToString(g8[row, j], CultureInfo.CurrentUICulture) == " ")
		//                        //if (g8[row, j] != null) TT#624 - MD - DOConnell - Export the Assortment to Excel and the Total and Balance row are cut off
		//                        {
		//                            aMIDExportFile.AddValue(g8[row, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
		//                        }
		//                        else
		//                        {
		//                            aMIDExportFile.AddValue(" ", false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
		//                        }
		//                    }
		//                }
		//                // add detail columns
		//                negativeStyle = "g9Negative1";
		//                textStyle = "g9Editable1";
		//                for (j = 0; j < g9.Cols.Count; j++)
		//                {
		//                    if (g9.Cols[j].Visible)
		//                    {
		//                        if (g9[row, j] != null || Convert.ToString(g9[row, j], CultureInfo.CurrentUICulture) == " ")
		//                        //if (g9[row, j] != null) TT#624 - MD - DOConnell - Export the Assortment to Excel and the Total and Balance row are cut off
		//                        {
		//                            aMIDExportFile.AddValue(g9[row, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
		//                        }
		//                        else
		//                        {
		//                            aMIDExportFile.AddValue(" ", false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
		//                        }
		//                    }
		//                }
		//                aMIDExportFile.WriteRow();
		//                ++row;
		//            //} 
		//            //break; //TT#624 - MD - DOConnell - Export the Assortment to Excel and the Total and Balance row are cut off
		//            //}
		//        }
		//        //TT#2195
		//        //if (aAddSummary)
		//        //{
		//        //    // add blank line before summary
		//        //    if (aMIDExportFile.ExportType == eExportType.CSV &&
		//        //        aExportingAll)
		//        //    {
		//        //        aMIDExportFile.AddRow();
		//        //        aMIDExportFile.WriteRow();
		//        //    }

		//        //    // add all stores or chain summary
		//        //    for (i = 0; i < g10.Rows.Count; i++)
		//        //    {
		//        //        aMIDExportFile.AddRow();
		//        //        // add row headings
		//        //        negativeStyle = null;
		//        //        textStyle = "g10Style1";
		//        //        for (j = 0; j < g10.Cols.Count; j++)
		//        //        {
		//        //            if (g10[i, j] != null)
		//        //            {
		//        //                aMIDExportFile.AddValue(g10[i, j].ToString(), eExportDataType.RowHeading, textStyle, negativeStyle);
		//        //            }
		//        //            else
		//        //            {
		//        //                aMIDExportFile.AddValue(" ", eExportDataType.RowHeading, textStyle, negativeStyle);
		//        //            }
		//        //        }
		//        //        // add total columns
		//        //        negativeStyle = "g11Negative1";
		//        //        textStyle = "g11Editable1";
		//        //        for (j = 0; j < g11.Cols.Count; j++)
		//        //        {
		//        //            if (g11[i, j] != null)
		//        //            {
		//        //                aMIDExportFile.AddValue(g11[i, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
		//        //            }
		//        //            else
		//        //            {
		//        //                aMIDExportFile.AddValue(" ", eExportDataType.Value, textStyle, negativeStyle);
		//        //            }
		//        //        }
		//        //        // add detail columns
		//        //        negativeStyle = "g12Negative1";
		//        //        textStyle = "g12Editable1";
		//        //        for (j = 0; j < g12.Cols.Count; j++)
		//        //        {
		//        //            if (g12[i, j] != null)
		//        //            {
		//        //                aMIDExportFile.AddValue(g12[i, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
		//        //            }
		//        //            else
		//        //            {
		//        //                aMIDExportFile.AddValue(" ", eExportDataType.Value, textStyle, negativeStyle);
		//        //            }
		//        //        }
		//        //        aMIDExportFile.WriteRow();
		//        //    }
		//        //}

		//        aMIDExportFile.WriteGrouping();
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		//private PlanWaferCell[,] ExportGetValues(CubeWafer aCubeWafer, C1FlexGrid aGrid, C1FlexGrid aRowHdrGrid)
		//private WaferCell[,] ExportGetValues(CubeWafer aCubeWafer, C1FlexGrid aGrid, C1FlexGrid aRowHdrGrid)
		//{
		//    PagingGridTag gridTag = null;
		//    C1FlexGrid rowHdrGrid;
		//    C1FlexGrid colHdrGrid;
		//    int i;
		//    //PlanWaferCell[,] planWaferCellTable = null;
		//    ColumnHeaderTag ColTag;
		//    RowHeaderTag RowTag;
		//    WaferCell[,] waferCellTable = null;
		//    CubeWafer asrtWafer;

		//    try
		//    {
		//        if (aRowHdrGrid.Rows.Count > 0)
		//        {
		//            //    //Fill CommonWaferCoordinateListGroup
		//            //    aCubeWafer.CommonWaferCoordinateList = _commonWaferCoordinateList;
		//            //    // get grid tags
		//            gridTag = (PagingGridTag)aGrid.Tag;
		//            //    // get row and column grid tags
		//            //    rowHdrGrid = aRowHdrGrid;
		//            //    colHdrGrid = gridTag.ColHeaderGrid;
		//            //    //Fill ColWaferCoordinateListGroup
		//            //    aCubeWafer.ColWaferCoordinateListGroup.Clear();
		//            //    if (colHdrGrid != null &&
		//            //        aGrid.Cols.Count > 0)
		//            //    {
		//            //        for (i = 0; i < aGrid.Cols.Count; i++)
		//            //        {
		//            //            ColTag = (ColumnHeaderTag)colHdrGrid.Cols[i].UserData;
		//            //            if (ColTag != null)
		//            //            {
		//            //                aCubeWafer.ColWaferCoordinateListGroup.Add(ColTag.CubeWaferCoorList);
		//            //            }
		//            //        }
		//            //    }

		//            //    //Fill RowWaferCoordinateListGroup
		//            //    aCubeWafer.RowWaferCoordinateListGroup.Clear();
		//            //    if (rowHdrGrid != null &&
		//            //        aRowHdrGrid.Rows.Count > 0)
		//            //    {
		//            //        for (i = 0; i < aRowHdrGrid.Rows.Count; i++)
		//            //        {
		//            //            RowTag = (RowHeaderTag)rowHdrGrid.Rows[i].UserData;
		//            //            if (RowTag != null)
		//            //            {
		//            //                aCubeWafer.RowWaferCoordinateListGroup.Add(RowTag.CubeWaferCoorList);
		//            //            }
		//            //        }
		//            //    }

		//            //    if (aCubeWafer.ColWaferCoordinateListGroup.Count > 0 && aCubeWafer.RowWaferCoordinateListGroup.Count > 0)
		//            //    {
		//            //        // Retreive array of values

		//            //        //Begin Modification - JScott - Add Scaling Decimals
		//            //        //planWaferCellTable = _planCubeGroup.GetPlanWaferCellValues(aCubeWafer, ((NumericComboObject)cboUnitScaling.SelectedItem).Value, ((NumericComboObject)cboDollarScaling.SelectedItem).Value);
		//            //        //planWaferCellTable = _planCubeGroup.GetPlanWaferCellValues(aCubeWafer, ((ComboObject)cboUnitScaling.SelectedItem).Value, ((ComboObject)cboDollarScaling.SelectedItem).Value);
		//            //        AssrtWaferCellTable = _asrtCubeGroup.GetAssortmentWaferCellValues(aCubeWafer);
		//            //        //planWaferCellTable = _asrtCubeGroup.GetAssortmentWaferCellValues(aCubeWafer);
		//            //        //End Modification - JScott - Add Scaling Decimals
		//            //    }
		//            //}
		//            rowHdrGrid = gridTag.RowHeaderGrid;
		//            colHdrGrid = gridTag.ColHeaderGrid;

		//            //Create the AssortmentWafer to request data
		//            asrtWafer = new CubeWafer();

		//            //Fill CommonWaferCoordinateListGroup
		//            asrtWafer.CommonWaferCoordinateList = _commonWaferCoordinateList;

		//            //Fill ColWaferCoordinateListGroup
		//            asrtWafer.ColWaferCoordinateListGroup.Clear();

		//            if (colHdrGrid != null && aGrid.Cols.Count > 0)
		//            {
		//                for (i = 0; i < aGrid.Cols.Count; i++)
		//                {
		//                    ColTag = (ColumnHeaderTag)colHdrGrid.Cols[i].UserData;
		//                    if (ColTag != null)
		//                    {
		//                        asrtWafer.ColWaferCoordinateListGroup.Add(ColTag.CubeWaferCoorList);
		//                    }
		//                }
		//            }

		//            //Fill RowWaferCoordinateListGroup

		//            asrtWafer.RowWaferCoordinateListGroup.Clear();
		//            if (rowHdrGrid != null && aRowHdrGrid.Rows.Count > 0)
		//            {
		//                for (i = 0; i < aRowHdrGrid.Rows.Count; i++)
		//                {
		//                    RowTag = (RowHeaderTag)rowHdrGrid.Rows[i].UserData;
		//                    if (RowTag != null)
		//                    {
		//                        asrtWafer.RowWaferCoordinateListGroup.Add(RowTag.CubeWaferCoorList);
		//                    }
		//                }
		//            }

		//            if (asrtWafer.ColWaferCoordinateListGroup.Count > 0 && asrtWafer.RowWaferCoordinateListGroup.Count > 0)
		//            {
		//                // Retreive array of values

		//                waferCellTable = _asrtCubeGroup.GetAssortmentWaferCellValues(asrtWafer);
		//            }
		//        }

		//        //return planWaferCellTable;
		//        return waferCellTable;
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		private string GetCellStyle(C1FlexGrid aGrid, int aTopRow, int aLeftCol,
			int aBottomRow, int aRightCol)
		{
			try
			{
				CellRange cellRange;
				cellRange = aGrid.GetCellRange(aTopRow, aLeftCol, aBottomRow, aRightCol);
				return cellRange.Style.Name;
			}
			catch
			{
				throw;
			}
		}

		//private void ExportApplyFormatting(MIDExportFile aMIDExportFile, int aRowsCount, int aColumnsCount,
		//    C1FlexGrid aDetailHdgGrid)
		//{
		//    try
		//    {
		//        int topRow;
		//        int leftCol;
		//        int bottomRow;
		//        int rightCol;

		//        ExportSetStyles(aMIDExportFile);

		//        // set all cells to detail
		//        aMIDExportFile.ApplyCellFormatting(g3.Rows.Count, aDetailHdgGrid.Cols.Count,
		//            aRowsCount - 1, aColumnsCount - 1, "g6Editable1");

		//        // then override certain cells with specific styles
		//        // set column headings
		//        aMIDExportFile.ApplyCellFormatting(0, 0,
		//            g3.Rows.Count - 1, aColumnsCount - 1, "g3ColumnHeading");
		//        aMIDExportFile.ApplyCellFormatting(1, 1,
		//            1, aColumnsCount - 1, "g3GroupHeader");

		//        // set row headings
		//        aMIDExportFile.ApplyCellFormatting(0, 0,
		//            aRowsCount - 1, aDetailHdgGrid.Cols.Count - 1, "g4Style1");

		//        //Begin Track #5006 - JScott - Display Low-levels one at a time
		//        //switch (_openParms.PlanSessionType)
		//        //switch (_currentPlanSessionType)
		//        ////End Track #5006 - JScott - Display Low-levels one at a time
		//        //{
		//        //    case ePlanSessionType.StoreSingleLevel:
		//        //        // set group level formatting
		//        //        // row header
		//        //        topRow = g3.Rows.Count + aDetailHdgGrid.Rows.Count;
		//        //        leftCol = 0;
		//        //        bottomRow = topRow + ((PagingGridTag)g7.Tag).DetailsPerGroup - 1;
		//        //        rightCol = g7.Cols.Count - 1;
		//        //        aMIDExportFile.ApplyCellFormatting(topRow, leftCol, bottomRow, rightCol, "g7Reverse1");
		//        //        // values
		//        //        leftCol = g7.Cols.Count;
		//        //        rightCol = aColumnsCount - 1;
		//        //        aMIDExportFile.ApplyCellFormatting(topRow, leftCol, bottomRow, rightCol, "g9Editable1");
		//        //        // set all stores formatting
		//        //        // row header
		//        //        topRow = bottomRow + 1;
		//        //        leftCol = 0;
		//        //        bottomRow = aRowsCount - 1;
		//        //        rightCol = g10.Cols.Count - 1;
		//        //        aMIDExportFile.ApplyCellFormatting(topRow, leftCol, bottomRow, rightCol, "g10Style1");
		//        //        // values
		//        //        leftCol = g10.Cols.Count;
		//        //        rightCol = aColumnsCount - 1;
		//        //        aMIDExportFile.ApplyCellFormatting(topRow, leftCol, bottomRow, rightCol, "g12Editable1");
		//        //        break;

		//        //    case ePlanSessionType.StoreMultiLevel:
		//        //        break;

		//        //    case ePlanSessionType.ChainSingleLevel:
		//        //        break;

		//        //    case ePlanSessionType.ChainMultiLevel:
		//        //        break;

		//        //    default:
		//        //        throw new Exception("Function not currently supported.");
		//        //}
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		//private void ExportSetStyles(MIDExportFile aMIDExportFile)
		//{
		//    try
		//    {
		//        ExportAddStyles(aMIDExportFile, "g2", g2);
		//        ExportAddStyles(aMIDExportFile, "g3", g3);
		//        ExportAddStyles(aMIDExportFile, "g4", g4);
		//        ExportAddStyles(aMIDExportFile, "g5", g5);
		//        ExportAddStyles(aMIDExportFile, "g6", g6);
		//        ExportAddStyles(aMIDExportFile, "g7", g7);
		//        ExportAddStyles(aMIDExportFile, "g8", g8);
		//        ExportAddStyles(aMIDExportFile, "g9", g9);
		//        //ExportAddStyles(aMIDExportFile, "g10", g10);
		//        //ExportAddStyles(aMIDExportFile, "g11", g11);
		//        //ExportAddStyles(aMIDExportFile, "g12", g12);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		private void ExportAddStyles(MIDExportFile aMIDExportFile, string aGridName, C1FlexGrid aGrid)
		{
			try
			{
				foreach (CellStyle cellStyle in aGrid.Styles)
				{
					try
					{
						aMIDExportFile.AddStyle(aGridName + cellStyle.Name, cellStyle);
					}
					catch
					{
						throw;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void ExportSpecificStyle(MIDExportFile aMIDExportFile, string aGridName, C1FlexGrid aGrid,
			string aStyleName)
		{
			try
			{
				foreach (CellStyle cellStyle in aGrid.Styles)
				{
					try
					{
						if (cellStyle.Name == aStyleName)
						{
							aMIDExportFile.AddStyle(aGridName + cellStyle.Name, cellStyle);
							break;
						}
					}
					catch
					{
						throw;
					}
				}
			}
			catch
			{
				throw;
			}
		}


		//private void BuildWorkingStoreList(int aAttributeSetKey, ProfileList aWorkingDetailProfileList)
		//{
		//    ProfileXRef storeSetXRef;
		//    ArrayList detailList;
		//    StoreProfile storeProf;

		//    try
		//    {
		//        //storeSetXRef = (ProfileXRef)_planCubeGroup.GetProfileXRef(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
		//        storeSetXRef = (ProfileXRef)_asrtCubeGroup.GetProfileXRef(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
		//        detailList = storeSetXRef.GetDetailList(aAttributeSetKey);
		//        if (detailList != null)
		//        {
		//            foreach (int storeId in detailList)
		//            {
		//                storeProf = (StoreProfile)_storeProfileList.FindKey(storeId);
		//                if (storeProf != null)
		//                {
		//                    aWorkingDetailProfileList.Add(storeProf);
		//                }
		//            }
		//        }
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		//    private void Formatg4Grid(bool aClearGrid, C1FlexGrid aGrid, ProfileList aWorkingDetailProfileList,
		//bool aViewableGrid)
		//    {
		//        CubeWaferCoordinateList cubeWaferCoordinateList;
		//        ArrayList compList;
		//        GridRowList gridRowList;
		//        RowColProfileHeader groupHeader;
		//        RowColProfileHeader varHeader;
		//        VariableProfile varProf;

		//        try
		//        {
		//            if (aClearGrid)
		//            {
		//                aGrid.Clear();
		//            }

		//            if (aGrid.Tag == null)
		//            {
		//                aGrid.Tag = new PagingGridTag(Grid4, aGrid, aGrid, null, null, 0, 0);
		//            }

		//            compList = new ArrayList();
		//            gridRowList = new GridRowList();

		//            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//            //switch (_openParms.PlanSessionType)
		//            switch (_currentPlanSessionType)
		//            //End Track #5006 - JScott - Display Low-levels one at a time
		//            {
		//                case ePlanSessionType.ChainSingleLevel:

		//                    foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
		//                    {
		//                        if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
		//                        {
		//                            if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
		//                            {
		//                                if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube && ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
		//                                {
		//                                    compList.Add(detailHeader);
		//                                }
		//                            }
		//                        }
		//                    }

		//                    ((PagingGridTag)aGrid.Tag).GroupsPerGrid = _sortedVariableHeaders.Count;

		//                    foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
		//                    {
		//                        varHeader = (RowColProfileHeader)varEntry.Value;
		//                        varProf = (VariableProfile)varHeader.Profile;

		//                        groupHeader = new RowColProfileHeader(varProf.VariableName, false, 0, varProf);

		//                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, new RowColProfileHeader("Original Plan", false, 2, null), gridRowList.Count, " ", varProf.VariableName, false), true);

		//                        if (_adjustmentRowHeader.IsDisplayed)
		//                        {
		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
		//                            //End Track #5006 - JScott - Display Low-levels one at a time
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, varProf.VariableName, varProf.VariableName));

		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
		//                            //End Track #5006 - JScott - Display Low-levels one at a time
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", varProf.VariableName + "|ADJ"));
		//                        }
		//                        else
		//                        {
		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
		//                            //End Track #5006 - JScott - Display Low-levels one at a time
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, varProf.VariableName, varProf.VariableName));
		//                        }

		//                        foreach (RowColProfileHeader detailHeader in compList)
		//                        {
		//                            if (detailHeader.IsDisplayed)
		//                            {
		//                                //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
		//                                {
		//                                    //End Track #6010 - JScott - Bad % Change on Basis2
		//                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                                    //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
		//                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
		//                                    //End Track #5006 - JScott - Display Low-levels one at a time
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + detailHeader.Name));
		//                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                }
		//                                //End Track #6010 - JScott - Bad % Change on Basis2
		//                            }
		//                        }

		//                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
		//                        {
		//                            if (basisHeader.IsDisplayed)
		//                            {
		//                                //Begin Track #5648 - JScott - Export Option from OTS Forecast Review Scrren
		//                                //groupHeader = new RowColProfileHeader("Chain " + basisHeader.Name, true, 0, null);
		//                                groupHeader = new RowColProfileHeader("Chain " + basisHeader.Name, true, 0, varProf);
		//                                //End Track #5648 - JScott - Export Option from OTS Forecast Review Scrren

		//                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                                //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
		//                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
		//                                //End Track #5006 - JScott - Display Low-levels one at a time
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                                //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                                ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
		//                                ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, varProf.VariableName + "|" + ((BasisProfile)basisHeader.Profile).Name));
		//                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key), varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key)));
		//                                ////End Track #5782
		//                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key), varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key)));
		//                                //End Track #5006 - JScott - Display Low-levels one at a time

		//                                foreach (RowColProfileHeader detailHeader in compList)
		//                                {
		//                                    if (detailHeader.IsDisplayed)
		//                                    {
		//                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
		//                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
		//                                        {
		//                                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                                            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
		//                                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
		//                                            //End Track #5006 - JScott - Display Low-levels one at a time
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                                            ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
		//                                            ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
		//                                            //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.VersionProfile.Key) + "|" + detailHeader.Name));
		//                                            ////End Track #5782
		//                                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.VersionProfile.Key) + "|" + detailHeader.Name));
		//                                            //End Track #5006 - JScott - Display Low-levels one at a time
		//                                        }
		//                                    }
		//                                }
		//                            }
		//                        }
		//                    }

		//                    break;

		//                case ePlanSessionType.StoreSingleLevel:

		//                    foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
		//                    {
		//                        if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
		//                        {
		//                            if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
		//                            {
		//                                if (((QuantityVariableProfile)detailHeader.Profile).isStoreDetailCube && ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
		//                                {
		//                                    compList.Add(detailHeader);
		//                                }
		//                            }
		//                        }
		//                    }

		//                    ((PagingGridTag)aGrid.Tag).GroupsPerGrid = aWorkingDetailProfileList.Count;

		//                    foreach (StoreProfile storeProfile in aWorkingDetailProfileList)
		//                    {
		//                        groupHeader = new RowColProfileHeader("Store", true, 0, null);

		//                        if (_adjustmentRowHeader.IsDisplayed)
		//                        {
		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
		//                            //End Track #5006 - JScott - Display Low-levels one at a time
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);

		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
		//                            //End Track #5006 - JScott - Display Low-levels one at a time
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", storeProfile.Text + "|ADJ"));
		//                        }
		//                        else
		//                        {
		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
		//                            //End Track #5006 - JScott - Display Low-levels one at a time
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);
		//                        }

		//                        foreach (RowColProfileHeader detailHeader in compList)
		//                        {
		//                            if (detailHeader.IsDisplayed)
		//                            {
		//                                //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
		//                                {
		//                                    //End Track #6010 - JScott - Bad % Change on Basis2
		//                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                    //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
		//                                    //End Track #5006 - JScott - Display Low-levels one at a time
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + detailHeader.Name));
		//                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                }
		//                                //End Track #6010 - JScott - Bad % Change on Basis2
		//                            }
		//                        }

		//                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
		//                        {
		//                            if (basisHeader.IsDisplayed)
		//                            {
		//                                groupHeader = new RowColProfileHeader("Store " + basisHeader.Name, true, 0, null);

		//                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
		//                                //End Track #5006 - JScott - Display Low-levels one at a time
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                                //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                                ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
		//                                ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
		//                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key), storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key)));
		//                                ////End Track #5782
		//                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key), storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key)));
		//                                //End Track #5006 - JScott - Display Low-levels one at a time

		//                                foreach (RowColProfileHeader detailHeader in compList)
		//                                {
		//                                    if (detailHeader.IsDisplayed)
		//                                    {
		//                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
		//                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
		//                                        {
		//                                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
		//                                            //End Track #5006 - JScott - Display Low-levels one at a time
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                            //Begin Track #5006 - JScott - Display Low-levels one at a time
		//                                            ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
		//                                            ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
		//                                            //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
		//                                            ////End Track #5782
		//                                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
		//                                            //End Track #5006 - JScott - Display Low-levels one at a time
		//                                        }
		//                                    }
		//                                }
		//                            }
		//                        }
		//                    }

		//                    break;

		//                case ePlanSessionType.ChainMultiLevel:

		//                    foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
		//                    {
		//                        if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
		//                        {
		//                            if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
		//                            {
		//                                if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
		//                                    ((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
		//                                {
		//                                    compList.Add(detailHeader);
		//                                }
		//                            }
		//                        }
		//                    }

		//                    ((PagingGridTag)aGrid.Tag).GroupsPerGrid = aWorkingDetailProfileList.Count;

		//                    foreach (PlanProfile planProf in aWorkingDetailProfileList)
		//                    {
		//                        groupHeader = new RowColProfileHeader("Low Level", true, 0, null);

		//                        if (_adjustmentRowHeader.IsDisplayed)
		//                        {
		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, planProf.NodeProfile.Text, planProf.NodeProfile.Text), true);

		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", planProf.NodeProfile.Text + "|ADJ"));
		//                        }
		//                        else
		//                        {
		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, planProf.NodeProfile.Text), true);
		//                        }

		//                        foreach (RowColProfileHeader detailHeader in compList)
		//                        {
		//                            if (detailHeader.IsDisplayed)
		//                            {
		//                                //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
		//                                {
		//                                    //End Track #6010 - JScott - Bad % Change on Basis2
		//                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, planProf.NodeProfile.Text + "|" + detailHeader.Name));
		//                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                }
		//                                //End Track #6010 - JScott - Bad % Change on Basis2
		//                            }
		//                        }

		//                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
		//                        {
		//                            if (basisHeader.IsDisplayed)
		//                            {
		//                                groupHeader = new RowColProfileHeader("Low Level " + basisHeader.Name, true, 0, null);

		//                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
		//                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
		//                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key)));
		//                                //End Track #5782

		//                                foreach (RowColProfileHeader detailHeader in compList)
		//                                {
		//                                    if (detailHeader.IsDisplayed)
		//                                    {
		//                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
		//                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
		//                                        {
		//                                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                            //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
		//                                            //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
		//                                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + detailHeader.Name));
		//                                            //End Track #5782
		//                                        }
		//                                    }
		//                                }
		//                            }
		//                        }
		//                    }

		//                    break;

		//                case ePlanSessionType.StoreMultiLevel:

		//                    foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
		//                    {
		//                        if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
		//                        {
		//                            if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
		//                            {
		//                                if (((QuantityVariableProfile)detailHeader.Profile).isStoreDetailCube)
		//                                {
		//                                    compList.Add(detailHeader);
		//                                }
		//                            }
		//                        }
		//                    }

		//                    ((PagingGridTag)aGrid.Tag).GroupsPerGrid = aWorkingDetailProfileList.Count;

		//                    foreach (StoreProfile storeProfile in aWorkingDetailProfileList)
		//                    {
		//                        //=================
		//                        // Store High Level
		//                        //=================

		//                        groupHeader = new RowColProfileHeader("Store", true, 0, null);

		//                        if (_adjustmentRowHeader.IsDisplayed)
		//                        {
		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);

		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", storeProfile.Text + "|ADJ"));
		//                        }
		//                        else
		//                        {
		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);
		//                        }

		//                        foreach (RowColProfileHeader detailHeader in compList)
		//                        {
		//                            if (detailHeader.IsDisplayed)
		//                            {
		//                                //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
		//                                {
		//                                    //End Track #6010 - JScott - Bad % Change on Basis2
		//                                    if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
		//                                    {
		//                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + detailHeader.Name));
		//                                    }
		//                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                }
		//                                //End Track #6010 - JScott - Bad % Change on Basis2
		//                            }
		//                        }

		//                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
		//                        {
		//                            if (basisHeader.IsDisplayed)
		//                            {
		//                                groupHeader = new RowColProfileHeader("Store " + basisHeader.Name, true, 0, null);

		//                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
		//                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
		//                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key), storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key)));
		//                                //End Track #5782

		//                                foreach (RowColProfileHeader detailHeader in compList)
		//                                {
		//                                    if (detailHeader.IsDisplayed)
		//                                    {
		//                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
		//                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
		//                                        {
		//                                            if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
		//                                            {
		//                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
		//                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
		//                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
		//                                                //End Track #5782
		//                                            }
		//                                        }
		//                                    }
		//                                }
		//                            }
		//                        }

		//                        //======================
		//                        // Store Low Level Total
		//                        //======================

		//                        groupHeader = new RowColProfileHeader("Low Level Total", true, 0, null);

		//                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, "Low Level Total", storeProfile.Text + "|Low Level Total"));

		//                        foreach (RowColProfileHeader detailHeader in compList)
		//                        {
		//                            if (detailHeader.IsDisplayed)
		//                            {
		//                                //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
		//                                {
		//                                    //End Track #6010 - JScott - Bad % Change on Basis2
		//                                    if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
		//                                    {
		//                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|Low Level Total|" + detailHeader.Name));
		//                                    }
		//                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                }
		//                                //End Track #6010 - JScott - Bad % Change on Basis2
		//                            }
		//                        }

		//                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
		//                        {
		//                            if (basisHeader.IsDisplayed)
		//                            {
		//                                groupHeader = new RowColProfileHeader("Low Level Total " + basisHeader.Name, true, 0, null);

		//                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

		//                                foreach (RowColProfileHeader detailHeader in compList)
		//                                {
		//                                    if (detailHeader.IsDisplayed)
		//                                    {
		//                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
		//                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
		//                                        {
		//                                            if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
		//                                            {
		//                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
		//                                            }
		//                                        }
		//                                    }
		//                                }
		//                            }
		//                        }

		//                        //==============
		//                        // Store Balance
		//                        //==============

		//                        groupHeader = new RowColProfileHeader("Store Balance", true, 0, null);

		//                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
		//                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, "Balance", storeProfile.Text + "|Balance"));

		//                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
		//                        {
		//                            if (basisHeader.IsDisplayed)
		//                            {
		//                                groupHeader = new RowColProfileHeader("Store Balance " + basisHeader.Name, true, 0, null);

		//                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
		//                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|Balance|" + ((BasisProfile)basisHeader.Profile).Name));
		//                            }
		//                        }

		//                        foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
		//                        {
		//                            //================
		//                            // Store Low Level
		//                            //================

		//                            groupHeader = new RowColProfileHeader(planProf.NodeProfile.Text, true, 0, null);

		//                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, storeProfile.Text + "|" + planProf.NodeProfile.Text));

		//                            foreach (RowColProfileHeader detailHeader in compList)
		//                            {
		//                                if (detailHeader.IsDisplayed)
		//                                {
		//                                    //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                    if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
		//                                    {
		//                                        //End Track #6010 - JScott - Bad % Change on Basis2
		//                                        if (((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
		//                                        {
		//                                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
		//                                        }
		//                                        //Begin Track #6010 - JScott - Bad % Change on Basis2
		//                                    }
		//                                    //End Track #6010 - JScott - Bad % Change on Basis2
		//                                }
		//                            }

		//                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
		//                            {
		//                                if (basisHeader.IsDisplayed)
		//                                {
		//                                    groupHeader = new RowColProfileHeader(planProf.NodeProfile.Text + basisHeader.Name, true, 0, null);

		//                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
		//                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
		//                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
		//                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key)));
		//                                    //End Track #5782

		//                                    foreach (RowColProfileHeader detailHeader in compList)
		//                                    {
		//                                        if (detailHeader.IsDisplayed)
		//                                        {
		//                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
		//                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
		//                                            {
		//                                                if (((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
		//                                                {
		//                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
		//                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
		//                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
		//                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
		//                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
		//                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
		//                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
		//                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
		//                                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
		//                                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
		//                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + detailHeader.Name));
		//                                                    //End Track #5782
		//                                                }
		//                                            }
		//                                        }
		//                                    }
		//                                }
		//                            }
		//                        }
		//                    }

		//                    break;
		//            }

		//            aGrid.Cols.Count = FIXEDROWHEADERS;
		//            aGrid.Cols.Fixed = FIXEDROWHEADERS;

		//            gridRowList.BuildGridRows(aGrid, 0);

		//            if (aViewableGrid)
		//            {
		//                ((PagingGridTag)aGrid.Tag).Visible = true;
		//            }
		//            ((PagingGridTag)aGrid.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
		//            ((PagingGridTag)aGrid.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;

		//            if (aViewableGrid)
		//            {
		//                _gridData[Grid4] = new CellTag[aGrid.Rows.Count, aGrid.Cols.Count];
		//            }
		//        }
		//        catch (Exception exc)
		//        {
		//            string message = exc.ToString();
		//            throw;
		//        }
		//    }
		#endregion
		//END TT#3 -MD- DOConnell - Export does not produce output

		// BEGIN TT#376-MD - stodd - Update Enqueue logic

		private void EnqueueError(AllocationProfile ap)
		{
			_enqueueHeaderError = true;
			// Message to audit
			string msgText = string.Format(MIDText.GetText(eMIDTextCode.msg_EnqueueFailedForHeader), ap.HeaderID);
			SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
			// Message to user
			msgText = string.Format(MIDText.GetText((int)eAllocationActionStatus.NoHeaderResourceLocks));
			MessageBox.Show(msgText, _windowName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			// Close Review windows
			CloseView();
		}

		private void CloseView()
		{
			ChangePending = false;
			CloseOtherViews();
			if (_transaction.AssortmentView != null)
			{
				Close();
			}
		}


		private void CloseOtherViews()
		{
			MIDRetail.Windows.SummaryView frmSummaryView;
			MIDRetail.Windows.SizeView frmSizeView;
            //BEGIN TT#598 - MD - DOConnell - When an assortment is closed, any style or size review windows should also be closed.
            //MIDRetail.Windows.AssortmentView frmStyleView;
            MIDRetail.Windows.StyleView frmStyleView;
            //END TT#598 - MD - DOConnell - When an assortment is closed, any style or size review windows should also be closed.
			MIDRetail.Windows.frmVelocityMethod frmVelocityMethod;
			try
			{
				if (_transaction.SummaryView != null)
				{
					frmSummaryView = (MIDRetail.Windows.SummaryView)_transaction.SummaryView;
					if (ErrorFound)
					{
						frmSummaryView.ErrorFound = true;
					}
					frmSummaryView.Close();
				}
				if (_transaction.SizeView != null)
				{
					frmSizeView = (MIDRetail.Windows.SizeView)_transaction.SizeView;
					if (ErrorFound)
					{
						frmSizeView.ErrorFound = true;
					}
					frmSizeView.Close();
				}
				if (_transaction.StyleView != null)
				{
                    //BEGIN TT#598 - MD - DOConnell - When an assortment is closed, any style or size review windows should also be closed.
                    //frmStyleView = (MIDRetail.Windows.AssortmentView)_transaction.AssortmentView;
                    frmStyleView = (MIDRetail.Windows.StyleView)_transaction.StyleView;
                    //END TT#598 - MD - DOConnell - When an assortment is closed, any style or size review windows should also be closed.
					if (ErrorFound)
					{
						frmStyleView.ErrorFound = true;
					}
					frmStyleView.Close();
				}
				if (_transaction.VelocityWindow != null)
				{
					frmVelocityMethod = (MIDRetail.Windows.frmVelocityMethod)_transaction.VelocityWindow;
					if (ErrorFound)
					{
						frmVelocityMethod.ErrorFound = true;
					}
					frmVelocityMethod.Close();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		// END TT#376-MD - stodd - Update Enqueue logic

		// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
		private void OnProcessMethodOnAssortmentEvent(object source, ProcessMethodOnAssortmentEventArgs e)
		{
            bool errorFound = false;
			try
			{
				// BEGIN TT#696-MD - Stodd - add "active process"
				if (e.AsrtRid == this.AssortmentRid)	// This is done so only the correct assortment process the method
				{
                    errorFound = IsValidToProcessMethod(e);

                    if (!errorFound)
                    {
                        // END TT#696-MD - Stodd - add "active process"
                        //ApplicationBaseAction aMethod = _transaction.CreateNewMethodAction(e.MethodType);
                        ApplicationBaseAction aMethod = _sab.ApplicationServerSession.GetMethods.GetMethod(e.MethodRid, e.MethodType);
                        GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);

                        bool aReviewFlag = false;
                        bool aUseSystemTolerancePercent = true;
                        double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
                        int aStoreFilter = Include.AllStoreFilterRID;
                        int aWorkFlowStepKey = -1;
                        SelectedHeaderList allocHdrList = null;  // TT#1075 - MD - stodd - no headers selected yet action completeld successfully
                        if (Enum.IsDefined(typeof(eAssortmentActionType), (eAssortmentActionType)e.MethodType))
                        {
                            //==============================
                            // ASSORTMENT ACTION PROCESSING
                            //==============================
                            //ProcessAssortmentMethod(ref _transaction, action, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                        }
                        else
                        {
                            //==============================
                            // ALLOCATION ACTION PROCESSING
                            //==============================
                            // Begin TT#1026 - md - stodd - GA with Grp min-max & alloc min-max & Inv Basis = color.  On header clr 005 the Inv Basis min-max is not honored. On header clr 270 allocated below the min of 12.
                            //ProcessAllocationMethod(ref _transaction, e.MethodRid, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                            ProcessAllocationMethod(ref _transaction, (int)e.MethodType, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                            // End TT#1026 - md - stodd - GA with Grp min-max & alloc min-max & Inv Basis = color.  On header clr 005 the Inv Basis min-max is not honored. On header clr 270 allocated below the min of 12.

                            //BEGIN TT#579-MD - stodd -  After adding a header to a placeholder, placeholder has a balance value even though its units are all in balance  
                            eAllocationActionStatus actionStatus = _transaction.AllocationActionAllHeaderStatus;
                            if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                            {
                                // Begin TT#1026 - md - stodd - GA with Grp min-max & alloc min-max & Inv Basis = color.  On header clr 005 the Inv Basis min-max is not honored. On header clr 270 allocated below the min of 12.
                                //SelectedHeaderList allocHdrKeyList = GetSelectableHeaderList(e.MethodRid, _transaction, true);	// TT#889 - MD - stodd - need not working
								// Begin TT#1087 - MD - stodd - size review showing extra headers - 
                                allocHdrList = GetSelectableHeaderList((int)e.MethodType, _transaction, true);	// TT#889 - MD - stodd - need not working   // TT#1075 - MD - stodd - no headers selected yet action completeld successfully
                                // End TT#1026 - md - stodd - GA with Grp min-max & alloc min-max & Inv Basis = color.  On header clr 005 the Inv Basis min-max is not honored. On header clr 270 allocated below the min of 12.

                                //int[] hdrList = new int[allocHdrKeyList.Count];
                                //int i = 0;
                                //foreach (SelectedHeaderProfile shp in allocHdrKeyList)
                                //{
                                //    hdrList[i] = shp.Key;
                                //    i++;
                                //}

                                CheckHeaderListForUpdate(allocHdrList);
								// End TT#1087 - MD - stodd - size review showing extra headers - 
                                CalculatePlaceholderBalances();

                                // begin TT#918 - MD - Jellis - Group Allocation Errors prevent action
                                //      NOTE:  when group allocation actions occur, only the assortment is in the Allocation Header lists; so workspace is not being updated; ie.  for group allocations always do this refresh...
                                //_refreshAllocationWorkspace = CheckForAllocationHeaders(hdrList);
                                // end TT#918 - MD - Jellis - Group Allocation Errors prevent action
                                // Begin TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces
                                //if (_refreshAssortmentWorkspace)
                                //{
                                //    //_eab.AssortmentWorkspaceExplorer.IRefresh();
                                //    UpdateAssortmentWorkspace();
                                //}
                                // End TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces

                                // begin TT#918 - MD - Jellis - Group Allocation Errors prevent action
                                //      NOTE:  when group allocation actions occur, only the assortment is in the Allocation Header lists; so workspace is not being updated; ie.  for group allocations always do this refresh...
                                //if (_refreshAllocationWorkspace)
                                //{
                                //    _eab.AllocationWorkspaceExplorer.IRefresh();
                                //}
                                //_eab.AllocationWorkspaceExplorer.IRefresh();  // TT#918 - MD - Jellis - Group Allocation Errors prevent action (only need to releoad headers affected)
                                // Begin TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces
                                ReloadUpdatedHeadersInAllocationWorkspace(); // TT#918 - MD - Jellis - Group Allocation Errors prevent action (only need to reload headers affected)
                                // End TT#921 - MD - replace IRefresh with ReloadUpdatedHeaders for workspaces
                                // end TT#918 - MD - Jellis - Group Allocation Errors prevent action

                                //SetGridRedraws(true);
                                ChangePending = false;	//TT#211-MD - stodd - size information not refreshing after size need is run on placeholder
                            }
                            //END TT#579-MD - stodd -  After adding a header to a placeholder, placeholder has a balance value even though its units are all in balance  
                        }
                        if (_transaction != null)
                        {
                            // Begin TT#1075 - MD - stodd - no headers selected yet action completeld successfully
                            eAllocationActionStatus actionStatus = eAllocationActionStatus.NoActionPerformed;
                            if (allocHdrList != null && allocHdrList.Count > 0)   // TT#1082 - MD - stodd - GA method with same style color gets warning message and then null ref
                            {
                                actionStatus = _transaction.AllocationActionAllHeaderStatus;
                            }
                            // End TT#1075 - MD - stodd - no headers selected yet action completeld successfully
                            string message = MIDText.GetTextOnly((int)actionStatus);
                            //MessageBox.Show(message, lblAction.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MessageBox.Show(message, e.MethodType.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                        CalculatePlaceholderBalances();	// TT#650-MD - stodd - Sizes on header not getting allocated when dropped onto a placeholder
                    }
				}	//  TT#696-MD - Stodd - add "active process"
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}
		}
        // END TT#217-MD - stodd - unable to run workflow methods against assortment

        // Begin TT#1033 - md - stodd - Able to process Locked Group Allocations - 
        private bool IsValidToProcessMethod(ProcessMethodOnAssortmentEventArgs e)
        {
            bool errorFound = false;
            // Begin TT#1030 - MD - stodd - GA method error message
            //=====================================================================================
            // Checks to be sure that the Group Allocation is set to "Process As" = "Group".
            // If not, then it's an error.
            // Button2 represents "process as" = headers.
            //=====================================================================================
            if (midToolbarRadioButton1.Button2.Checked && e.MethodType == eMethodType.GroupAllocation)
            {
                string errorMessage = MIDText.GetText(eMIDTextCode.msg_as_GroupAllocationMethodNotAllowed);
                SAB.ApplicationServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Error,
                    eMIDTextCode.msg_as_GroupAllocationMethodNotAllowed,
                    errorMessage,
                    "Group Allocation Method");

                string message = MIDText.GetTextOnly((int)eAllocationActionStatus.NoActionPerformed);
                MessageBox.Show(message, e.MethodType.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorFound = true;
            }
            // End TT#1030 - MD - stodd - GA method error message

            //=============================================================
            // Checks to be sure the Group Allocation is not Read Only.
            //=============================================================
            if (!FunctionSecurity.AllowUpdate)
            {
                string errorMessage = MIDText.GetText(eMIDTextCode.msg_as_NoProcessingMethodReadOnly);
                errorMessage = errorMessage.Replace("{0}", "Group Allocation");
                errorMessage = errorMessage.Replace("{1}", GroupName);
                SAB.ApplicationServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Error,
                    eMIDTextCode.msg_as_NoProcessingMethodReadOnly,
                    errorMessage,
                    e.MethodType.ToString());

                string message = MIDText.GetTextOnly((int)eAllocationActionStatus.NoActionPerformed);
                MessageBox.Show(message, e.MethodType.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorFound = true;
            }

            // Begin TT#1057 - MD - stodd - prevent allocation override against a GA
            //========================================================================
            // Prevent an allocation Override from being run on group allocation. 
            // Can only be run against selected headers.
            //========================================================================
			// Begin TT#1060 - md - stodd - allow Alloc Override to process against GA headers - 
            if (e.MethodType == eMethodType.AllocationOverride && midToolbarRadioButton1.Button1.Checked)
            {
                string msgText = MIDText.GetTextOnly((int)eMIDTextCode.msg_as_AllocationOverrideMethodNotAllowed);
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);

                string message = MIDText.GetTextOnly((int)eAllocationActionStatus.NoActionPerformed);
                MessageBox.Show(message, e.MethodType.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorFound = true;
            }
			// End TT#1060 - md - stodd - allow Alloc Override to process against GA headers - 
            // End TT#1057 - MD - stodd - prevent allocation override against a GA
             
            return errorFound;
        }
        // End TT#1033 - md - stodd - Able to process Locked Group Allocations -
		

		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		private void OnAssortmentSelectedHeaderEvent(object source, AssortmentSelectedHeaderEventArgs e)
		{
			try
			{
				// BEGIN TT#696-MD - Stodd - add "active process"
				if (e.AsrtRid == this.AssortmentRid)	// This is done so only the correct assortment process the method
				{
                    e.SelectedHdrList = GetSelectableHeaderList((int)e.MethodType, _transaction, true);	// TT#889 - MD - stodd - need not working
				}
				// END TT#696-MD - Stodd - add "active process"
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}
		}

		// END TT#371-MD - stodd -  Velocity Interactive on Assortment

		// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
		public void OnAssortmentTransactionEvent(object source, AssortmentTransactionEventArgs e)	// TT#698-MD - Stodd - add ability for workflows to be run against assortments.
		{
			try
			{
				// BEGIN TT#696-MD - Stodd - add "active process"
				if (e.AsrtRid == this.AssortmentRid)	// This is done so only the correct assortment process the method
				{
					e.Transaction = _transaction;
				}
				// END TT#696-MD - Stodd - add "active process"
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}
		}
		// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.

		//BEGIN TT#575-MD - stodd - After a header is dropped onto a placeholder, both the placeholder and the header are in balance but it appears the Placeholder has too few units removed
		private void DebugHeaders()
		{
			//BEGIN TT#572-MD - stodd - Assortment matrix changes are not being reflected on style review
			// BEGIN TT#488-MD - STodd - Group Allocation 
			//AllocationProfileList apl = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
			AllocationProfileList apl = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.AssortmentMember);
			// END TT#488-MD - STodd - Group Allocation 
			Debug.WriteLine(" ");
			Debug.WriteLine("Debug Assortment Headers");
			foreach (AllocationProfile ap in apl.ArrayList)
			//END TT#572-MD - stodd - Assortment matrix changes are not being reflected on style review
			{
				Debug.WriteLine(ap.HeaderID + " HDR TYPE " + ap.HeaderType + " QTY " + ap.TotalUnitsToAllocate + " ALLOC QTY " + ap.TotalUnitsAllocated);
				if (ap.Packs != null && ap.Packs.Count > 0)
				{
					int packType;
					foreach (PackHdr aPack in ap.Packs.Values)
					{
						Debug.WriteLine("  PACK " + aPack.PackName + " PACK MULT " + aPack.PackMultiple + " PACK QTY " + aPack.PacksToAllocate + " PACK ALLOC QTY " + aPack.PacksAllocated);

						if (aPack.PackColors != null && aPack.PackColors.Count > 0)
						{
							foreach (PackColorSize packColor in aPack.PackColors.Values)
							{
								Debug.WriteLine("    PACKCOLOR " + packColor.ColorName + " TY " + packColor.ColorUnitsInPack);
								if (_sab.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
								{
									if (packColor.ColorSizes != null && packColor.ColorSizes.Count > 0)
									{


									}
								}
							}
						}
					}
				}

				if (ap.BulkColors != null && ap.BulkColors.Count > 0)
				{
					Debug.WriteLine("  BULK QTY " + ap.BulkUnitsToAllocate + " BULK ALLOC QTY " + ap.BulkUnitsAllocated);
					foreach (HdrColorBin aColor in ap.BulkColors.Values)
					{
						ColorCodeProfile ccp = _sab.HierarchyServerSession.GetColorCodeProfile(aColor.ColorCodeRID);
						Debug.WriteLine("  COLOR " + ccp.ColorCodeID + " QTY " + aColor.ColorUnitsToAllocate + " ALLOC QTY " + aColor.ColorUnitsAllocated);

						if (_sab.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
						{
							if (aColor.ColorSizes != null && aColor.ColorSizes.Count > 0)
							{

							}
						}
					}
				}
			}
		}

		private void ShowEmailForm(System.Net.Mail.Attachment a)
		{

			EmailMessageForm frm = new EmailMessageForm();
			frm.AddAttachment(a);
			string subject = "Group Allocation Review";
			MIDRetail.Data.SecurityAdmin secAdmin = new MIDRetail.Data.SecurityAdmin();
			DataTable dt = secAdmin.GetUser(SAB.ClientServerSession.UserRID);
			if (dt.Rows.Count > 0)
			{
				string userName = String.Empty;
				string userFullName = String.Empty;
				if (dt.Rows[0].IsNull("USER_NAME") == false)
				{
					userName = (string)dt.Rows[0]["USER_NAME"];
				}
				if (dt.Rows[0].IsNull("USER_FULLNAME") == false)
				{
					userFullName = (string)dt.Rows[0]["USER_FULLNAME"];
				}
				subject += " - " + userName + " (" + userFullName + ")";
			}

			frm.SetDefaults("", "", "", subject, "Please see attached file.");
			frm.ShowDialog();

		}

		// Begin TT#936 - MD - stodd - Prevent the saving of empty Group Allocations - 
        override protected void BeforeClosing()
        {
            try
            {
                CancelFormClosing = false;
                int deleteEmptyGroupAllocationRid = Include.NoRID;
                bool headersFound = false;
                AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.AssortmentMember);
                foreach (AllocationProfile ap in alp.ArrayList)
                {
                    if (ap.HeaderType == eHeaderType.Assortment)
                    {
                        deleteEmptyGroupAllocationRid = ap.Key;
                    }
                    // If a "real" header is found, then GA can close normally w/o any message.
                    if (ap.HeaderType != eHeaderType.Assortment && ap.HeaderType != eHeaderType.Placeholder)
                    {
                        headersFound = true;
                        break;
                    }
                }

                // If we're here, we didn't find a any "real" headers.
                if (!headersFound)
                {
                    string errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_ConfirmEmptyGroupAllocationDelete);
                    DialogResult diagResult = MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (diagResult == System.Windows.Forms.DialogResult.OK)
                    {
                        SAB.HeaderServerSession.DeleteEmptyGroupAllocationtHeader(deleteEmptyGroupAllocationRid);

                    }
                    else if (diagResult == System.Windows.Forms.DialogResult.Cancel)
                    {
                        CancelFormClosing = true;
                    }
                }

				// Begin TT#984 - MD - stodd - save content grid format - 
                if (FormLoaded && !ugDetails.IsDisposed)
                {
                    InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                    layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, eLayoutID.groupAllocationReviewContent, ugDetails);
                }
				// End TT#984 - MD - stodd - save content grid format - 
            }
            catch (Exception ex)
            {
                HandleException(ex, "GroupAllocationView.BeforeClosing");
            }
        }
		// End TT#936 - MD - stodd - Prevent the saving of empty Group Allocations - 
		
		#region Toolbars
		private void ultraToolbarsManager1_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnProcess":
                    Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["actionComboBox"];
                    ProcessAction((int)cbo.Value);
                    break;
				case "Expand All":
					ExpandCollapseGrid(true);
                    break;
				case "Collapse All":
					ExpandCollapseGrid(false);
                    break;
				case "btnExport":
					Export();
					break;
				case "btnEmail":
					UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ugDetails);
					ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());
					break;
            }
        }

		private void ultraToolbarsManager1_ToolValueChanged(object sender, ToolEventArgs e)
		{
		}

        private void LoadActionsOnToolbar()
        {
            Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["actionComboBox"];
            cbo.ValueList.ValueListItems.Clear();
            cbo.ValueList.ValueListItems.Add(Include.NoRID, "Select action...");
            foreach (int action in Enum.GetValues(typeof(eAssortmentAllocationActionType)))
            {
                if (AllowAction(action, false))
                {
                    string actionText = MIDText.GetTextOnly(action);
                    cbo.ValueList.ValueListItems.Add(action, actionText);
                }
            }
            //set the default
            if (cbo.ValueList.ValueListItems.Count > 0)
            {
                cbo.SelectedIndex = 0;
            }
        }

		// Begin TT#941 - MD - stodd - When creating a new Group Allocation from headers selected on the allocation workspace, the selected headers are not getting placed into the Group Allocation.
        public UltraGridRow GetSelectedFirstRow()
        {
            UltraGridRow aRow = null;
            foreach (UltraGridRow row in ugDetails.Rows)
            {
                if (row.Band.Key == "Header")
                {
                    row.Selected = true;
                    break;
                }
            }

            if (ugDetails.Selected.Rows.Count == 1)
            {
                aRow = ugDetails.Selected.Rows[0];          
            }

            return aRow;
        }
		// End TT#941 - MD - stodd - When creating a new Group Allocation from headers selected on the allocation workspace, the selected headers are not getting placed into the Group Allocation.

		/// <summary>
		/// Process as Group
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		private void Button1_Click(object source, EventArgs args)
		{
			foreach (UltraGridRow row in ugDetails.Rows)
			{
				if (row.Band.Key == "Header")
				{
                    // Begin TT#1116 - md -stodd - null ref right clicking to remove row 
                    int key = Convert.ToInt32(row.Cells["KeyH"].Value);
                    AllocationProfile ap = GetAllocationProfile(key);
                    if (!ap.Placeholder)
                    {
                        row.Selected = true;
                    }
                    // End TT#1116 - md -stodd - null ref right clicking to remove row 
				}
			}
		}

		/// <summary>
		/// Process as Individual Headers
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		private void Button2_Click(object source, EventArgs args)
		{
			foreach (UltraGridRow row in ugDetails.Rows)
			{
				if (row.Band.Key == "Header")
				{
					row.Selected = false;
				}
			}
		}

		#endregion Toolbars

		#region UltraGridExcelExportWrapper
		/// <summary>
		/// Used to provide Excel Export Functionality for the Infragistics UltraWinGrid
		/// Just create an instance of this class, pass in your grid and call Export function.
		/// Saves in xls format - does not support xlsx format
		/// </summary>
		private class UltraGridExcelExportWrapper
		{
			private Infragistics.Win.UltraWinGrid.UltraGrid _ug;
			private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
			private bool _checkForExportSelected = false;
			private string _objectDescriptor;
			public UltraGridExcelExportWrapper(Infragistics.Win.UltraWinGrid.UltraGrid ug, string objectDescriptor = "rows")
			{
				_ug = ug;
				_objectDescriptor = objectDescriptor;

				this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter();
				this.ultraGridExcelExporter1.RowExporting += new Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventHandler(ultraGridExcelExporter1_RowExporting);
			}

			public void ExportAllRowsToExcel()
			{
				string myFilepath = FindSavePath();
				string MessBoxText1 = "All " + _objectDescriptor + " sucessfully exported to \r\n";
				string MessBoxText2 = "Number of " + _objectDescriptor + " exported: ";
				try
				{
					if (myFilepath != null)
					{
						_checkForExportSelected = false;
						this.ultraGridExcelExporter1.Export(_ug, myFilepath);
						MessageBox.Show(MessBoxText1 + myFilepath + "\r\n" + MessBoxText2 + _ug.Rows.Count);
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			public System.Net.Mail.Attachment ExportAllRowsToExcelAsAttachment()
			{
				try
				{
					_checkForExportSelected = false;
					return GetEmailAttachment();
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			public System.Net.Mail.Attachment ExportSelectedRowsToExcelAsAttachment()
			{
				try
				{

					_checkForExportSelected = true;
					return GetEmailAttachment();

				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			private System.Net.Mail.Attachment GetEmailAttachment()
			{
				Infragistics.Documents.Excel.Workbook wb = new Infragistics.Documents.Excel.Workbook();
				this.ultraGridExcelExporter1.Export(_ug, wb);

				//Infragistics does not save nicely directly to a memory stream, so saving as a file and reading it back into memory stream
				string fileName = System.IO.Path.GetTempPath() + "\\tempGroupAllocation_" + Data.EnvironmentInfo.MIDInfo.userName + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".tmp";
				wb.Save(fileName);
				byte[] b = System.IO.File.ReadAllBytes(fileName);
				System.IO.File.Delete(fileName);
				System.IO.MemoryStream streamAttachment = new System.IO.MemoryStream(b);

				System.Net.Mail.Attachment attachmentWorkbook;
				attachmentWorkbook = new System.Net.Mail.Attachment(streamAttachment, "GroupAllocation.xls");
				return attachmentWorkbook;
			}

			public void ExportSelectedRowsToExcel()
			{
				string myFilepath = FindSavePath();
				string MessBoxText1 = "Selected " + _objectDescriptor + " sucessfully exported to \r\n";
				string MessBoxText2 = "Number of " + _objectDescriptor + " exported: ";
				try
				{
					if (myFilepath != null)
					{
						_checkForExportSelected = true;
						this.ultraGridExcelExporter1.Export(_ug, myFilepath);
						MessageBox.Show(MessBoxText1 + myFilepath + "\r\n" + MessBoxText2 + _ug.Selected.Rows.Count);
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			private void ultraGridExcelExporter1_RowExporting(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventArgs e)
			{
				// The GridRow property on the event args is a clone of the on-screen row, and it will not pick up the Selected State 
				//Infragistics.Win.UltraWinGrid.UltraGridRow exportRow = e.GridRow;

				//  Get the grid
				//Infragistics.Win.UltraWinGrid.UltraGrid grid = (Infragistics.Win.UltraWinGrid.UltraGrid)e.GridRow.Band.Layout.Grid;

				// Get the real, on-screen row, from the export row. 
				Infragistics.Win.UltraWinGrid.UltraGridRow onScreenRow = _ug.GetRowFromPrintRow(e.GridRow);

				// If the on-screen row is not selected, do not export it. 
				if (onScreenRow.Selected == false && _checkForExportSelected == true)
					e.Cancel = true;
			}
			private String FindSavePath()
			{
				System.IO.Stream myStream;
				string myFilepath = null;
				try
				{
					SaveFileDialog saveFileDialog1 = new SaveFileDialog();
					saveFileDialog1.Filter = "excel files (*.xls)|*.xls";
					saveFileDialog1.FilterIndex = 2;
					saveFileDialog1.RestoreDirectory = true;
					if (saveFileDialog1.ShowDialog() == DialogResult.OK)
					{
						if ((myStream = saveFileDialog1.OpenFile()) != null)
						{
							myFilepath = saveFileDialog1.FileName;
							myStream.Close();
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return myFilepath;
			}
		}
		#endregion UltraGridExcelExportWrapper


	
		//END TT#575-MD - stodd - After a header is dropped onto a placeholder, both the placeholder and the header are in balance but it appears the Placeholder has too few units removed
	}
}