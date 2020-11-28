using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Text;
using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;
using System.Linq;
using System.Collections.Generic;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public partial class PlanView : MIDFormBase, IFormBase
    {
        #region Variable Declarations

        public event System.Windows.Forms.MouseEventHandler g2MouseUpRefireHandler;
        public event System.Windows.Forms.MouseEventHandler g3MouseUpRefireHandler;

//Begin Track #5244 - JScott - Add debugging for error encountered in PlanView window
//		private const bool THREADED_GRID_LOAD = true;
		private const bool THREADED_GRID_LOAD = false;
//End Track #5244 - JScott - Add debugging for error encountered in PlanView window
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

        private const int Grid1 = 0;
        private const int Grid2 = 1;
        private const int Grid3 = 2;
        private const int Grid4 = 3;
        private const int Grid5 = 4;
        private const int Grid6 = 5;
        private const int Grid7 = 6;
        private const int Grid8 = 7;
        private const int Grid9 = 8;
        private const int Grid10 = 9;
        private const int Grid11 = 10;
        private const int Grid12 = 11;

        private FunctionSecurityProfile _forecastBalanceSecurity;
        //private StoreFilterData _storeFilterDL;
        private FilterData _storeFilterDL;
        private TimeSpan _totalPrePageLoadTime;
        private TimeSpan _totalFirstPageLoadTime;
        private TimeSpan _totalPostPageLoadTime;

        private SessionAddressBlock _sab;
        private ApplicationSessionTransaction _transaction;
        private PlanCubeGroup _planCubeGroup;
        private string _windowName;
        private ProfileList _storeProfileList;
        private ProfileList _storeGroupListViewProfileList;
        private ProfileList _storeGroupLevelProfileList;
        private ProfileList _workingDetailProfileList;
        private ProfileList _variableProfileList;
        private ProfileList _quantityVariableProfileList;
        private ProfileList _versionProfileList;
		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		private ProfileList _periodProfileList;
		//End Track #5121 - JScott - Add Year/Season/Quarter totals
        private IPlanComputationQuantityVariables _quantityVariables;
        private PlanOpenParms _openParms;
        private PlanViewData _viewDL;
        private DataTable _dtView;
		private ArrayList _filterUserRIDList;
		private ArrayList _viewUserRIDList;
		private CubeWaferCoordinateList _commonWaferCoordinateList;
        private PlanViewData _planViewData;
        private DataTable _planViewDetail;
        private Hashtable _basisToolTipList;
        private Hashtable _basisLabelList;
        private bool _storeReadOnly;
        private bool _chainReadOnly;
        private bool _lowLevelStoreReadOnly;
        private bool _lowLevelChainReadOnly;
		//Begin Track #5006 - JScott - Display Low-levels one at a time
		private PlanProfile _currentStorePlanProfile;
		private PlanProfile _currentChainPlanProfile;
		private ePlanSessionType _currentPlanSessionType;
		private ArrayList _navigateItemList;
		private int _currentNavigateToolIdx;
		//End Track #5006 - JScott - Display Low-levels one at a time

        private MIDReaderWriterLock _pageLoadLock;
        private Hashtable _loadHash;
        private bool _currentRedrawState;
        private bool _formLoading;
        private bool _bindingView;
        private bool _bindingAttribute;
        private bool _bindingAttributeSet;
        private bool _bindingFilter;
        private bool _bindingUnitScaling;
        private bool _bindingDollarScaling;
        private bool _stopPageLoadThread;
        private bool _hSplitMove;
        private bool _isScrolling;
        private bool _isSorting;
        private bool _holdingScroll;
		private int _currColSplitPosition2;
        private int _currColSplitPosition3;
        private int _currRowSplitPosition4;
        private int _currRowSplitPosition8;
        private int _currRowSplitPosition12;
        private int _lastAttributeValue;
        private int _lastAttributeSetValue;
        private int _lastViewValue;
        private int _lastFilterValue;
        private int _lastUnitScalingValue;
        private int _lastDollarScalingValue;
        private SplitterTag _currSplitterTag;

        private int _dragStartColumn; //indicates which column is the column that started the drag/drop action.
        private DragState _dragState;
        private C1FlexGrid _rightClickedFrom;
        private Rectangle _dragBoxFromMouseDown;
        private bool _mouseDown = false;
        private bool _missedUp = false;

        private Bitmap _picLock; //this picture will be put in a cell that's locked.
        private Bitmap _downArrow;
        private Bitmap _upArrow;

		//Begin Track #5003 - JScott - Freeze Rows
		//private int _leftMostColBeforeFreeze;
		//End Track #5003 - JScott - Freeze Rows
		private ArrayList _selectableQuantityHeaders;
        private ArrayList _selectableStoreAttributeHeaders;
        private ArrayList _selectableVariableHeaders;
        private ArrayList _selectableTimeHeaders;
        private ArrayList _selectableBasisHeaders;
		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		private ArrayList _selectablePeriodHeaders;
		//End Track #5121 - JScott - Add Year/Season/Quarter totals
		private ArrayList _cmiBasisList;
        private SortedList _sortedVariableHeaders;
        private SortedList _sortedTimeHeaders;
		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		private Hashtable _periodHeaderHash;
		//End Track #5121 - JScott - Add Year/Season/Quarter totals
        private GroupedBy _columnGroupedBy;
        private RowColProfileHeader _adjustmentRowHeader;
        private RowColProfileHeader _originalRowHeader;
        private RowColProfileHeader _currentRowHeader;
        private CellTag[][,] _gridData;
        private object _holdValue;
        private structSort _currSortParms;
		private string _includeCurrentSetLabel = null;
		private string _includeAllSetsLabel = null;
        private string _basisLabel = null;

        private Theme _theme;
        private ThemeProperties _frmThemeProperties; //for the properties dialog box.

        private PlanViewSave _saveForm;

        private int _productDisplayCombination;
        private int _storeDisplayCombination;

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

        //Begin TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed
        private void HandleMIDException(MIDException MIDexc)
        {
            string Title, errLevel, Msg;
            MessageBoxIcon icon;
            MessageBoxButtons buttons;
            buttons = MessageBoxButtons.OK;
            switch (MIDexc.ErrorLevel)
            {
                case eErrorLevel.severe:
                    icon = MessageBoxIcon.Stop;
                    errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Severe));
                    break;

                case eErrorLevel.information:
                    icon = MessageBoxIcon.Information;
                    errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Information));
                    break;

                case eErrorLevel.warning:
                    icon = MessageBoxIcon.Warning;
                    errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Warning));
                    break;

                //Begin TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
                case eErrorLevel.error:
                    icon = MessageBoxIcon.Error;
                    errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Error));
                    break;

                //End TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
                default:
                    icon = MessageBoxIcon.Stop;
                    errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Severe));
                    break;
            }
            if (MIDexc.InnerException != null)
            {
                Title = errLevel + " - " + MIDexc.Message;
                Msg = MIDexc.InnerException.Message;
            }
            else
            {
                Title = errLevel;
                Msg = MIDexc.Message;
            }
            MessageBox.Show(this, Msg, Title,
                buttons, icon);
        }
        //End TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed

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

        #region Constructor

        public PlanView(SessionAddressBlock sab, PlanOpenParms aOpenParms)
            : base(sab)
        {
            try
            {
				_formLoading = true;
				_storeFilterDL = new FilterData();
                _openParms = aOpenParms;
                _sab = sab;
                _theme = _sab.ClientServerSession.Theme;
                _pageLoadLock = new MIDReaderWriterLock();
                _loadHash = new Hashtable();
                g2MouseUpRefireHandler += new System.Windows.Forms.MouseEventHandler(this.g2_MouseUp);
                g3MouseUpRefireHandler += new System.Windows.Forms.MouseEventHandler(this.g3_MouseUp);
				_includeAllSetsLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeAllSets);
				_includeCurrentSetLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeCurrentSet);
                // Begin TT#1580 - JSmith - Window Closes before Save Yes, No or Cancel can be selected
                BypassBaseClosingLogic = true;
                // End TT#1580

                InitializeComponent();
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

        override public void ISave()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                StopPageLoadThreads();
                RecomputePlanCubes();

                this.WindowState = FormWindowState.Normal;
                this.Enabled = false;
				//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
				////Begin Track #5690 - JScott - Can not save low to high
				////_saveForm = new PlanViewSave(_sab, _openParms, _planCubeGroup, _selectableVariableHeaders, _selectableQuantityHeaders);
				//_saveForm = new PlanViewSave(_sab, _openParms, _planCubeGroup, _selectableVariableHeaders, _selectableQuantityHeaders, false);
				////End Track #5690 - JScott - Can not save low to high
				_saveForm = new PlanViewSave(_sab, _openParms, _planCubeGroup, _selectableVariableHeaders, _selectableQuantityHeaders, false, _selectablePeriodHeaders);
				//End Track #5121 - JScott - Add Year/Season/Quarter totals
				_saveForm.OnPlanSaveClosingEventHandler += new PlanViewSave.PlanSaveClosingEventHandler(OnPlanSaveClosing);
                _saveForm.MdiParent = this.MdiParent;
                _saveForm.Show();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        override public void ISaveAs()
        {
            try
            {
				//Begin Track #5690 - JScott - Can not save low to high
				//ISave();
				Cursor.Current = Cursors.WaitCursor;
				StopPageLoadThreads();
				RecomputePlanCubes();

				this.WindowState = FormWindowState.Normal;
				this.Enabled = false;
                //Begin Track #5121 - JScott - Add Year/Season/Quarter totals
                //_saveForm = new PlanViewSave(_sab, _openParms, _planCubeGroup, _selectableVariableHeaders, _selectableQuantityHeaders, true);
                _saveForm = new PlanViewSave(_sab, _openParms, _planCubeGroup, _selectableVariableHeaders, _selectableQuantityHeaders, true, _selectablePeriodHeaders);
                //End Track #5121 - JScott - Add Year/Season/Quarter totals
				_saveForm.OnPlanSaveClosingEventHandler += new PlanViewSave.PlanSaveClosingEventHandler(OnPlanSaveAsClosing);
				_saveForm.MdiParent = this.MdiParent;
				_saveForm.Show();
				//End Track #5690 - JScott - Can not save low to high
			}
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        override public void IRefresh()
        {
            try
            {

            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

		//Begin Track #5690 - JScott - Can not save low to high
		//void OnPlanSaveClosing(object source)
		void OnPlanSaveClosing(object source, ePlanViewSaveResult closeResult)
		//End Track #5690 - JScott - Can not save low to high
		{
            try
            {
                this.Enabled = true;
                this.WindowState = FormWindowState.Maximized;
                LoadCurrentPages();
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

		//Begin Track #5690 - JScott - Can not save low to high
		void OnPlanSaveAsClosing(object source, ePlanViewSaveResult closeResult)
		{
			try
			{
				if (closeResult == ePlanViewSaveResult.Save)
				{
					this.Close();
				}
				else
				{
					this.Enabled = true;
					this.WindowState = FormWindowState.Maximized;
					LoadCurrentPages();
					LoadSurroundingPages();
					Cursor.Current = Cursors.Default;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//End Track #5690 - JScott - Can not save low to high
		#endregion

        #region PlanView Initialize and Load

        public void Initialize()
        {
            int i;
            ToolStripMenuItem basisCmiItem;
            ToolStripSeparator basisCmiSeparator;
            ProfileList basisProfList;
            // Begin TT#1125 - JSmith - Global/User should be consistent
            //SecurityAdmin secAdmin; //TT#827-MD -jsobek -Allocation Reviews Performance
            int userRID;
            // End TT#1125
#if (DEBUG)
            DateTime startTime;
            DateTime startTime2;
#endif

            try
            {
#if (DEBUG)
                _totalPrePageLoadTime = TimeSpan.Zero;
                _totalFirstPageLoadTime = TimeSpan.Zero;
                _totalPostPageLoadTime = TimeSpan.Zero;

                startTime = DateTime.Now;
                startTime2 = DateTime.Now;
#endif
                // Begin TT#1125 - JSmith - Global/User should be consistent
                //secAdmin = new SecurityAdmin(); //TT#827-MD -jsobek -Allocation Reviews Performance
                // End TT#1125

                _windowName = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanReview);
				//Begin Track #5006 - JScott - Display Low-levels one at a time
				_currentPlanSessionType = _openParms.PlanSessionType;
				//End Track #5006 - JScott - Display Low-levels one at a time

                //Begin Track #5858 - Kjohnson - Validating store security only
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                // End TT#44

              //cboStoreAttribute.Tag = "IgnoreMouseWheel";
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, eMIDControlCode.form_OTSPlanReview);
                switch (_openParms.PlanSessionType)
                {
                    case ePlanSessionType.StoreSingleLevel:
                        FunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelStore);
                        break;

                    case ePlanSessionType.StoreMultiLevel:
                        FunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelStore);
                        break;

                    case ePlanSessionType.ChainSingleLevel:
                        FunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelChain);
                        break;

                    case ePlanSessionType.ChainMultiLevel:
                        FunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelChain);
                        break;

                    default:
                        throw new Exception("Function not currently supported.");
                }
                // Begin TT#1118 - JSmith - Undesirable curser position
                //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, true);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eMIDControlCode.field_Filter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true, FunctionSecurity, true);
                cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eMIDControlCode.field_Filter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true, FunctionSecurity, true);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                // End TT#1118
                // Begin TT#1118 - JSmith - Undesirable curser position
                //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, eMIDControlCode.form_OTSPlanReview, FunctionSecurity, true);
                cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, eMIDControlCode.form_OTSPlanReview, true, FunctionSecurity, true);
                // End TT#1118
                // End TT#44
                //Begin Track #5858 - Kjohnson
                cboAttributeSet.Tag = "IgnoreMouseWheel";
                cboView.Tag = "IgnoreMouseWheel";
                cboUnitScaling.Tag = "IgnoreMouseWheel";
                cboDollarScaling.Tag = "IgnoreMouseWheel";

                spcHHeaderLevel2.Tag = new SplitterTag();
                spcHTotalLevel2.Tag = spcHHeaderLevel2.Tag;
                spcHDetailLevel2.Tag = spcHHeaderLevel2.Tag;
                spcHScrollLevel2.Tag = spcHHeaderLevel2.Tag;
                spcHHeaderLevel3.Tag = new SplitterTag();
                spcHTotalLevel3.Tag = spcHHeaderLevel3.Tag;
                spcHDetailLevel3.Tag = spcHHeaderLevel3.Tag;
                spcHScrollLevel3.Tag = spcHHeaderLevel3.Tag;
                spcHHeaderLevel1.Tag = new SplitterTag();
                spcHTotalLevel1.Tag = spcHHeaderLevel1.Tag;
                spcHDetailLevel1.Tag = spcHHeaderLevel1.Tag;
                spcHScrollLevel1.Tag = spcHHeaderLevel1.Tag;
                spcVLevel1.Tag = new SplitterTag();
                spcVLevel2.Tag = new SplitterTag();

				//Begin Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
				spcHHeaderLevel1.Panel1MinSize = spcHHeaderLevel2.SplitterWidth + spcHHeaderLevel3.SplitterWidth;
				spcHHeaderLevel1.Panel2MinSize = 0;
				spcHHeaderLevel2.Panel1MinSize = 0;
				spcHHeaderLevel2.Panel2MinSize = spcHHeaderLevel3.SplitterWidth;
				spcHHeaderLevel3.Panel1MinSize = 0;
				spcHHeaderLevel3.Panel2MinSize = 0;

				spcHTotalLevel1.Panel1MinSize = spcHTotalLevel2.SplitterWidth + spcHTotalLevel3.SplitterWidth;
				spcHTotalLevel1.Panel2MinSize = 0;
				spcHTotalLevel2.Panel1MinSize = 0;
				spcHTotalLevel2.Panel2MinSize = spcHTotalLevel3.SplitterWidth;
				spcHTotalLevel3.Panel1MinSize = 0;
				spcHTotalLevel3.Panel2MinSize = 0;

				spcHDetailLevel1.Panel1MinSize = spcHDetailLevel2.SplitterWidth + spcHDetailLevel3.SplitterWidth;
				spcHDetailLevel1.Panel2MinSize = 0;
				spcHDetailLevel2.Panel1MinSize = 0;
				spcHDetailLevel2.Panel2MinSize = spcHDetailLevel3.SplitterWidth;
				spcHDetailLevel3.Panel1MinSize = 0;
				spcHDetailLevel3.Panel2MinSize = 0;

				spcHScrollLevel1.Panel1MinSize = spcHScrollLevel2.SplitterWidth + spcHScrollLevel3.SplitterWidth;
				spcHScrollLevel1.Panel2MinSize = 0;
				spcHScrollLevel2.Panel1MinSize = 0;
				spcHScrollLevel2.Panel2MinSize = spcHScrollLevel3.SplitterWidth;
				spcHScrollLevel3.Panel1MinSize = 0;
				spcHScrollLevel3.Panel2MinSize = 0;

				//End Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
				_gridData = new CellTag[12][,];
                _commonWaferCoordinateList = new CubeWaferCoordinateList();
                _planViewData = new PlanViewData();

                _picLock = new Bitmap(GraphicsDirectory + "\\lock.gif");
                _downArrow = new Bitmap(GraphicsDirectory + "\\down.gif");
                _upArrow = new Bitmap(GraphicsDirectory + "\\up.gif");
				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//btnThemeProperties.Image = new Bitmap(GraphicsDirectory + "\\style.gif");
				//End Track #5006 - JScott - Display Low-levels one at a time

                BuildMenu();

                hScrollBar2.Tag = new ScrollBarValueChanged(ChangeHScrollBar2Value);
                hScrollBar3.Tag = new ScrollBarValueChanged(ChangeHScrollBar3Value);
                vScrollBar2.Tag = new ScrollBarValueChanged(ChangeVScrollBar2Value);
                vScrollBar3.Tag = new ScrollBarValueChanged(ChangeVScrollBar3Value);
                vScrollBar4.Tag = new ScrollBarValueChanged(ChangeVScrollBar4Value);

                _columnGroupedBy = GroupedBy.GroupedByTime;
                _lastFilterValue = _openParms.FilterRID;
                SetGridRedraws(false);

                g3.DrawMode = DrawModeEnum.OwnerDraw;
                g4.DrawMode = DrawModeEnum.OwnerDraw;
                g5.DrawMode = DrawModeEnum.OwnerDraw;
                g6.DrawMode = DrawModeEnum.OwnerDraw;
                g7.DrawMode = DrawModeEnum.OwnerDraw;
                g8.DrawMode = DrawModeEnum.OwnerDraw;
                g9.DrawMode = DrawModeEnum.OwnerDraw;
                g10.DrawMode = DrawModeEnum.OwnerDraw;
                g11.DrawMode = DrawModeEnum.OwnerDraw;
                g12.DrawMode = DrawModeEnum.OwnerDraw;

                // Create the PlanOpenParms object to open the Plan with

                _versionProfileList = _sab.ClientServerSession.GetUserForecastVersions();

                _forecastBalanceSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastForecastBalance);
                FunctionSecurityProfile filterUserSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
                FunctionSecurityProfile filterGlobalSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);
				FunctionSecurityProfile viewUserSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsUser);
				FunctionSecurityProfile viewGlobalSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsGlobal);

                _basisLabel = LoadBasisLabel();

				_filterUserRIDList = new ArrayList();

				_filterUserRIDList.Add(-1);

				if (filterUserSecurity.AllowView)
				{
					_filterUserRIDList.Add(_sab.ClientServerSession.UserRID);
				}

				if (filterGlobalSecurity.AllowView)
				{
					_filterUserRIDList.Add(Include.GlobalUserRID);
				}

				_viewUserRIDList = new ArrayList();

				_viewUserRIDList.Add(-1);

				if (viewUserSecurity.AllowView)
				{
					_viewUserRIDList.Add(_sab.ClientServerSession.UserRID);
				}

				if (viewGlobalSecurity.AllowView)
				{
					_viewUserRIDList.Add(Include.GlobalUserRID);
				}

                // Load the views

                _viewDL = new PlanViewData();
				_dtView = _viewDL.PlanView_Read(_viewUserRIDList);

				_dtView.Columns.Add(new DataColumn("DISPLAY_ID", typeof(string)));

                // Begin TT#1125 - JSmith - Global/User should be consistent
                //foreach (DataRow row in _dtView.Rows)
                //{
                //    if (Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture) != Include.GlobalUserRID)
                //    {
                //        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (User)";
                //    }
                //    else
                //    {
                //        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
                //    }
                //}
                foreach (DataRow row in _dtView.Rows)
                {
                     userRID = Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture);
                     if (userRID != Include.GlobalUserRID)
                    {
                        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                        //row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (" + secAdmin.GetUserName(userRID) + ")";
                        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (" + UserNameStorage.GetUserName(userRID) + ")";
                        //End TT#827-MD -jsobek -Allocation Reviews Performance
                    }
                    else
                    {
                        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
                    }
                }
                // End TT#1125

                //Create an App Server Transaction

                _transaction = _sab.ApplicationServerSession.CreateTransaction();

                //Create a StoreMaintCubeGroup

                switch (_openParms.PlanSessionType)
                {
                    case ePlanSessionType.StoreSingleLevel:
                        _planCubeGroup = _transaction.CreateStorePlanMaintCubeGroup();
                        break;

                    case ePlanSessionType.StoreMultiLevel:
                        _planCubeGroup = _transaction.CreateStoreMultiLevelPlanMaintCubeGroup();
                        break;

                    case ePlanSessionType.ChainSingleLevel:
                        _planCubeGroup = _transaction.CreateChainPlanMaintCubeGroup();
                        break;

                    case ePlanSessionType.ChainMultiLevel:
                        _planCubeGroup = _transaction.CreateChainMultiLevelPlanMaintCubeGroup();
                        break;

                    default:
                        throw new Exception("Function not currently supported.");
                }

                //Open the cubegroup

                _planCubeGroup.OpenCubeGroup(_openParms);

                // Create Basis Tooltips

                _basisToolTipList = new Hashtable();
                _basisLabelList = new Hashtable();

                switch (_openParms.PlanSessionType)
                {
                    case ePlanSessionType.StoreSingleLevel:

                        basisProfList = _openParms.GetBasisProfileList(_planCubeGroup, _openParms.StoreHLPlanProfile.NodeProfile.Key, _openParms.StoreHLPlanProfile.VersionProfile.Key);

                        foreach (BasisProfile basisProfile in basisProfList)
                        {
                            BuildBasisLabelsAndToolTip(_openParms.StoreHLPlanProfile.NodeProfile, basisProfile);
                        }

                        basisProfList = _openParms.GetBasisProfileList(_planCubeGroup, _openParms.ChainHLPlanProfile.NodeProfile.Key, _openParms.StoreHLPlanProfile.VersionProfile.Key);

                        foreach (BasisProfile basisProfile in basisProfList)
                        {
                            BuildBasisLabelsAndToolTip(_openParms.ChainHLPlanProfile.NodeProfile, basisProfile);
                        }
                        break;

                    case ePlanSessionType.StoreMultiLevel:

                        basisProfList = _openParms.GetBasisProfileList(_planCubeGroup, _openParms.StoreHLPlanProfile.NodeProfile.Key, _openParms.StoreHLPlanProfile.VersionProfile.Key);

                        foreach (BasisProfile basisProfile in basisProfList)
                        {
                            BuildBasisLabelsAndToolTip(_openParms.StoreHLPlanProfile.NodeProfile, basisProfile);
                        }

                        foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
                        {
                            basisProfList = _openParms.GetBasisProfileList(_planCubeGroup, planProf.NodeProfile.Key, planProf.VersionProfile.Key);

                            foreach (BasisProfile basisProfile in basisProfList)
                            {
                                BuildBasisLabelsAndToolTip(planProf.NodeProfile, basisProfile);
                            }
                        }
                        break;

                    case ePlanSessionType.ChainSingleLevel:

                        basisProfList = _openParms.GetBasisProfileList(_planCubeGroup, _openParms.ChainHLPlanProfile.NodeProfile.Key, _openParms.StoreHLPlanProfile.VersionProfile.Key);

                        foreach (BasisProfile basisProfile in basisProfList)
                        {
                            BuildBasisLabelsAndToolTip(_openParms.ChainHLPlanProfile.NodeProfile, basisProfile);
                        }
                        break;

                    case ePlanSessionType.ChainMultiLevel:

                        basisProfList = _openParms.GetBasisProfileList(_planCubeGroup, _openParms.ChainHLPlanProfile.NodeProfile.Key, _openParms.ChainHLPlanProfile.VersionProfile.Key);

                        foreach (BasisProfile basisProfile in basisProfList)
                        {
                            BuildBasisLabelsAndToolTip(_openParms.ChainHLPlanProfile.NodeProfile, basisProfile);
                        }

                        foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
                        {
                            basisProfList = _openParms.GetBasisProfileList(_planCubeGroup, planProf.NodeProfile.Key, planProf.VersionProfile.Key);

                            foreach (BasisProfile basisProfile in basisProfList)
                            {
                                BuildBasisLabelsAndToolTip(planProf.NodeProfile, basisProfile);
                            }
                        }
                        break;
                }

                switch (_openParms.PlanSessionType)
                {
                    case ePlanSessionType.StoreSingleLevel:
                    case ePlanSessionType.StoreMultiLevel:

                        if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel)
                        {
                            ((StorePlanMaintCubeGroup)_planCubeGroup).GetReadOnlyFlags(out _storeReadOnly, out _chainReadOnly);
                        }
                        else
                        {
                            ((StoreMultiLevelPlanMaintCubeGroup)_planCubeGroup).GetReadOnlyFlags(out _storeReadOnly, out _chainReadOnly, out _lowLevelStoreReadOnly, out _lowLevelChainReadOnly);
                        }

                        ((PlanCubeGroup)_planCubeGroup).SetStoreFilter(_openParms.FilterRID, _planCubeGroup);

                        //Retrieve StoreProfile list
                        _storeProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.Store);

                        //BEGIN TT#6-MD-VStuart - Single Store Select
                        ProfileList _singleStoreProfileList = new ProfileList(eProfileType.Store);
                        if (!String.IsNullOrEmpty(_openParms.StoreId) && _openParms.StoreId != "(None)")
                        {
                            //MessageBox.Show("We have single store mode.", "Single Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            foreach (StoreProfile storeProf in _storeProfileList)
                            {
                                if (storeProf.StoreId == _openParms.StoreId)
                                {
                                     _singleStoreProfileList.Add(storeProf);
                                }
                            }
                            _storeProfileList = _singleStoreProfileList;
                        }
                        //END TT#6-MD-VStuart - Single Store Select

                        Audit _audit;
                        _audit = _SAB.ApplicationServerSession.Audit;

                        string message = "Plan Session Type: " + _openParms.PlanSessionType;
                        _audit.Add_Msg(eMIDMessageLevel.Debug,  message, this.GetType().Name);
                        
                        message = "StoreProfileLIst count: " + _storeProfileList.Count.ToString();
                        _audit.Add_Msg(eMIDMessageLevel.Debug,  message, this.GetType().Name);

                        message = "Store Group RID: " + _openParms.StoreGroupRID;
                        _audit.Add_Msg(eMIDMessageLevel.Debug, message, this.GetType().Name);
                        


                        if (_storeProfileList.Count == 0)  /// SMR HERE
                        {
                            MessageBox.Show("Applied filter(s) have resulted in no displayable Stores.", "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                        _workingDetailProfileList = _storeProfileList;

                        //Retrieve StoreGroupListViewProfile list

                        _storeGroupListViewProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupListView);

                        //Retrieve StoreGroupLevelProfile list

                        _storeGroupLevelProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);
                        _selectableStoreAttributeHeaders = new ArrayList();

                        foreach (StoreGroupLevelProfile strGrpLvlProf in _storeGroupLevelProfileList)
                        {
							//Begin Track #6261 - JScott - Sets in Set summary section are out of order
							//_selectableStoreAttributeHeaders.Add(new RowColProfileHeader(strGrpLvlProf.Name, true, 0, strGrpLvlProf));
							_selectableStoreAttributeHeaders.Add(new RowColProfileHeader(strGrpLvlProf.Name, true, strGrpLvlProf.Sequence, strGrpLvlProf));
							//End Track #6261 - JScott - Sets in Set summary section are out of order
						}

                        break;

                    case ePlanSessionType.ChainSingleLevel:

                        ((ChainPlanMaintCubeGroup)_planCubeGroup).GetReadOnlyFlags(out _chainReadOnly);
                        _workingDetailProfileList = null;

                        break;

                    case ePlanSessionType.ChainMultiLevel:

                        ((ChainMultiLevelPlanMaintCubeGroup)_planCubeGroup).GetReadOnlyFlags(out _chainReadOnly, out _lowLevelChainReadOnly);
                        _workingDetailProfileList = _openParms.LowLevelPlanProfileList;

                        break;
                }

				//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
				//Retrieve Period ProfileList from Calendar

				_periodProfileList = _sab.ClientServerSession.Calendar.GetPeriodProfileList(_openParms.DateRangeProfile.Key);

				//End Track #5121 - JScott - Add Year/Season/Quarter totals
                //Retrieve VariableProfile lists

                _variableProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.Variable);
                _quantityVariableProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.QuantityVariable);

                // Load View

				LoadView();

                // Build Basis List

                _selectableBasisHeaders = new ArrayList();
                _cmiBasisList = new ArrayList();

                if (_openParms.BasisProfileList.Count > 0)
                {
                    basisCmiSeparator = new ToolStripSeparator();
                    cmsg4g7g10.Items.Add(basisCmiSeparator);
                    _cmiBasisList.Add(basisCmiSeparator);

                    for (i = 0; i < _openParms.BasisProfileList.Count; i++)
                    {
                        //Begin Track #5750 - KJohnson - Basis Labels Not Showing Correct Information
                        BasisProfile bp = (BasisProfile)_openParms.BasisProfileList[i];
                        BasisDetailProfile bdp = (BasisDetailProfile)bp.BasisDetailProfileList[0];
                        //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                        string lblName = GetBasisLabel(bp.Key, bdp.HierarchyNodeProfile.Key);
                        if (lblName != null)
                        {
                            bp.Name = lblName;
                        }
                        //End Track #5782

                        //Begin Track #5779 - JScott - Right Click Menu for Labels
                        //_selectableBasisHeaders.Add(new RowColProfileHeader(bp.Name, true, i, bp));
                        //End Track #5779 - JScott - Right Click Menu for Labels

                        basisCmiItem = new ToolStripMenuItem();
                        //Begin Track #5779 - JScott - Right Click Menu for Labels
						_selectableBasisHeaders.Add(new RowColProfileHeader(bp.Name, true, i, bp));
						//End Track #5779 - JScott - Right Click Menu for Labels
						basisCmiItem.Text = bp.Name;
                        //End Track #5750 - KJohnson
                        basisCmiItem.Click += new System.EventHandler(this.cmiBasis_Click);
                        basisCmiItem.Checked = true;

                        cmsg4g7g10.Items.Add(basisCmiItem);
                        _cmiBasisList.Add(basisCmiItem);
                    }
                }

                // Build DateProfile list

				//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
				//if (_openParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Period)
				//{
				//    cmiShowPeriods.Checked = true;
				//    cmiShowWeeks.Checked = false;
				//}
				//else
				//{
				//    cmiShowPeriods.Checked = false;
				//    cmiShowWeeks.Checked = true;
				//}

				//End Track #5121 - JScott - Add Year/Season/Quarter totals
                BuildTimeHeaders();

                // Set Title bar

                switch (_openParms.PlanSessionType)
                {
                    case ePlanSessionType.StoreSingleLevel:
                    case ePlanSessionType.StoreMultiLevel:
                        this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanReview) + " - " + _openParms.StoreHLPlanProfile.NodeProfile.Text + " / " + _openParms.StoreHLPlanProfile.VersionProfile.Description + " / " + _openParms.DateRangeProfile.DisplayDate;
                        break;

                    case ePlanSessionType.ChainSingleLevel:
                    case ePlanSessionType.ChainMultiLevel:
                        this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanReview) + " - " + _openParms.ChainHLPlanProfile.NodeProfile.Text + " / " + _openParms.ChainHLPlanProfile.VersionProfile.Description + " / " + _openParms.DateRangeProfile.DisplayDate;
                        break;
                }

                if (_storeReadOnly)
                {
                    this.Text += " [Read Only]";
                }

                // Format and Fill grids

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//switch (_openParms.PlanSessionType)
				//{
				//    case ePlanSessionType.ChainSingleLevel:
				//        cmiRowChooser.Visible = false;
				//        cmiColumnChooser.Visible = false;
				//        cmig2g3Seperator1.Visible = false;
				//        cmiQuantityChooser.Visible = true;
				//        cmiVariableChooser.Visible = true;
				//        pnlGroupBy.Visible = false;
				//        pnlSpacer2.Visible = false;
				//        cboView.Visible = true;
				//        cboFilter.Visible = false;
				//        cboStoreAttribute.Visible = false;
				//        cboAttributeSet.Visible = false;
				//        cboUnitScaling.Visible = true;
				//        cboDollarScaling.Visible = true;
				//        break;

				//    case ePlanSessionType.ChainMultiLevel:
				//        cmiRowChooser.Visible = true;
				//        cmiColumnChooser.Visible = true;
				//        cmig2g3Seperator1.Visible = true;
				//        cmiQuantityChooser.Visible = false;
				//        cmiVariableChooser.Visible = false;
				//        pnlGroupBy.Visible = true;
				//        pnlSpacer2.Visible = true;
				//        cboView.Visible = true;
				//        cboFilter.Visible = false;
				//        cboStoreAttribute.Visible = false;
				//        cboAttributeSet.Visible = false;
				//        cboUnitScaling.Visible = true;
				//        cboDollarScaling.Visible = true;
				//        break;

				//    case ePlanSessionType.StoreSingleLevel:
				//        cmiRowChooser.Visible = true;
				//        cmiColumnChooser.Visible = true;
				//        cmig2g3Seperator1.Visible = true;
				//        cmiQuantityChooser.Visible = false;
				//        cmiVariableChooser.Visible = false;
				//        pnlGroupBy.Visible = true;
				//        pnlSpacer2.Visible = true;
				//        cboView.Visible = true;
				//        cboFilter.Visible = true;
				//        cboStoreAttribute.Visible = true;
				//        cboAttributeSet.Visible = true;
				//        cboUnitScaling.Visible = true;
				//        cboDollarScaling.Visible = true;

				//        BindFilterComboBox();
				//        BindStoreAttrComboBox();
				//        break;

				//    case ePlanSessionType.StoreMultiLevel:
				//        cmiRowChooser.Visible = true;
				//        cmiColumnChooser.Visible = true;
				//        cmig2g3Seperator1.Visible = true;
				//        cmiQuantityChooser.Visible = false;
				//        cmiVariableChooser.Visible = false;
				//        pnlGroupBy.Visible = true;
				//        pnlSpacer2.Visible = true;
				//        cboView.Visible = true;
				//        cboFilter.Visible = true;
				//        cboStoreAttribute.Visible = true;
				//        cboAttributeSet.Visible = true;
				//        cboUnitScaling.Visible = true;
				//        cboDollarScaling.Visible = true;

				//        BindFilterComboBox();
				//        BindStoreAttrComboBox();
				//        break;
				//}
				//
				switch (_openParms.PlanSessionType)
				{
					case ePlanSessionType.ChainMultiLevel:
					case ePlanSessionType.StoreMultiLevel:

						pnlNavigate.Visible = true;
						pnlSpacer3.Visible = true;
						tspNavigate.ImageList = MIDGraphics.ImageList;

						tsbNavigate.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.NavigateImage);
						tsbFirst.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.FirstImage);
						tsbPrevious.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.PreviousImage);
						tsbNext.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.NextImage);
						tsbLast.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.LastImage);

						_navigateItemList = new ArrayList();

						i = _navigateItemList.Add(new ToolStripMenuItem());
						((ToolStripMenuItem)_navigateItemList[i]).Tag = null;
						((ToolStripMenuItem)_navigateItemList[i]).Text = "Multi-Level";
						((ToolStripMenuItem)_navigateItemList[i]).Checked = true;
						((ToolStripMenuItem)_navigateItemList[i]).Click += new System.EventHandler(this.cmiNavigate_Click);

						if (_openParms.PlanSessionType == ePlanSessionType.ChainMultiLevel)
						{
							i = _navigateItemList.Add(new ToolStripMenuItem());
							((ToolStripMenuItem)_navigateItemList[i]).Tag = _openParms.ChainHLPlanProfile;
							((ToolStripMenuItem)_navigateItemList[i]).Text = _openParms.ChainHLPlanProfile.NodeProfile.Text;
							((ToolStripMenuItem)_navigateItemList[i]).Checked = false;
							((ToolStripMenuItem)_navigateItemList[i]).Click += new System.EventHandler(this.cmiNavigate_Click);
						}
						else
						{
							i = _navigateItemList.Add(new ToolStripMenuItem());
							((ToolStripMenuItem)_navigateItemList[i]).Tag = _openParms.StoreHLPlanProfile;
							((ToolStripMenuItem)_navigateItemList[i]).Text = _openParms.StoreHLPlanProfile.NodeProfile.Text;
							((ToolStripMenuItem)_navigateItemList[i]).Checked = false;
							((ToolStripMenuItem)_navigateItemList[i]).Click += new System.EventHandler(this.cmiNavigate_Click);
						}

						foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
						{
							i = _navigateItemList.Add(new ToolStripMenuItem());
							((ToolStripMenuItem)_navigateItemList[i]).Tag = planProf;
							((ToolStripMenuItem)_navigateItemList[i]).Text = planProf.NodeProfile.Text;
							((ToolStripMenuItem)_navigateItemList[i]).Checked = false;
							((ToolStripMenuItem)_navigateItemList[i]).Click += new System.EventHandler(this.cmiNavigate_Click);
						}

						cmsNavigate.Items.AddRange((ToolStripMenuItem[])_navigateItemList.ToArray(typeof(ToolStripMenuItem)));

						_currentNavigateToolIdx = 0;
						_currentStorePlanProfile = null;
						_currentChainPlanProfile = null;

						break;

					case ePlanSessionType.ChainSingleLevel:

						pnlNavigate.Visible = false;
						pnlSpacer3.Visible = false;

						_currentStorePlanProfile = null;
						_currentChainPlanProfile = _openParms.ChainHLPlanProfile;

						break;

					case ePlanSessionType.StoreSingleLevel:

						pnlNavigate.Visible = false;
						pnlSpacer3.Visible = false;

						_currentStorePlanProfile = _openParms.StoreHLPlanProfile;
						_currentChainPlanProfile = _openParms.ChainHLPlanProfile;

						break;
				}

				ConfigureControls();
				//End Track #5006 - JScott - Display Low-levels one at a time
                BindViewComboBox();
                BindUnitScalingComboBox();
                BindDollarScalingComboBox();

                if (_openParms.PlanSessionType != ePlanSessionType.ChainSingleLevel)
                {
                    SelectRadioBtn();
                }

				//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				//Formatg2Grid(false, -1, SortEnum.none);
				//Formatg3Grid(false, -1, -1, SortEnum.none);
				Formatg2Grid(false, null, SortEnum.none);
				Formatg3Grid(false, null, SortEnum.none);
				//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				Formatg4Grid(false, g4, _workingDetailProfileList, true);
                Formatg5Grid(false);
                Formatg6Grid(false);
                Formatg7Grid(false);
                Formatg8Grid(false);
                Formatg9Grid(false);
                Formatg10Grid(false);
                Formatg11Grid(false);
                Formatg12Grid(false);
                SortToDefault();
                DefineStyles();
                SetScrollBarPosition(hScrollBar2, 0);
                SetScrollBarPosition(hScrollBar3, 0);
                SetScrollBarPosition(vScrollBar2, 0);
                SetScrollBarPosition(vScrollBar3, 0);
                SetScrollBarPosition(vScrollBar4, 0);

#if (DEBUG)
                _totalPrePageLoadTime = _totalPrePageLoadTime.Add(DateTime.Now.Subtract(startTime));

                startTime = DateTime.Now;
#endif

                LoadCurrentPages();

#if (DEBUG)
                _totalFirstPageLoadTime = _totalFirstPageLoadTime.Add(DateTime.Now.Subtract(startTime));

                startTime = DateTime.Now;
#endif

                ResizeCol1();
                ResizeCol2();
                ResizeCol3();
                ResizeRow1();
                ResizeRow4();
                ResizeRow7();
                ResizeRow10();

                // Format for XP, if applicable
                if (Environment.OSVersion.Version.Major > 4 && Environment.OSVersion.Version.Minor > 0 && System.IO.File.Exists(Application.ExecutablePath + ".manifest"))
                {
                    FormatForXP(this);
                }

                SetGridRedraws(true);
                LoadSurroundingPages();
                _formLoading = false;

#if (DEBUG)
                _totalPostPageLoadTime = _totalPostPageLoadTime.Add(DateTime.Now.Subtract(startTime));
#endif
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		//Begin Track #5006 - JScott - Display Low-levels one at a time
		private void ConfigureControls()
		{
			try
			{
				switch (_currentPlanSessionType)
				{
					case ePlanSessionType.ChainSingleLevel:
						cmiRowChooser.Visible = false;
						cmiColumnChooser.Visible = false;
						cmig2g3Seperator1.Visible = false;
						cmiQuantityChooser.Visible = true;
						cmiVariableChooser.Visible = true;
						pnlGroupBy.Visible = false;
						pnlSpacer2.Visible = false;
						cboView.Visible = true;
						cboFilter.Visible = false;
						cboStoreAttribute.Visible = false;
						cboAttributeSet.Visible = false;
						cboUnitScaling.Visible = true;
						cboDollarScaling.Visible = true;
						break;

					case ePlanSessionType.ChainMultiLevel:
						cmiRowChooser.Visible = true;
						cmiColumnChooser.Visible = true;
						cmig2g3Seperator1.Visible = true;
						cmiQuantityChooser.Visible = false;
						cmiVariableChooser.Visible = false;
						pnlGroupBy.Visible = true;
						pnlSpacer2.Visible = true;
						cboView.Visible = true;
						cboFilter.Visible = false;
						cboStoreAttribute.Visible = false;
						cboAttributeSet.Visible = false;
						cboUnitScaling.Visible = true;
						cboDollarScaling.Visible = true;
						break;

					case ePlanSessionType.StoreSingleLevel:
						cmiRowChooser.Visible = true;
						cmiColumnChooser.Visible = true;
						cmig2g3Seperator1.Visible = true;
						cmiQuantityChooser.Visible = false;
						cmiVariableChooser.Visible = false;
						pnlGroupBy.Visible = true;
						pnlSpacer2.Visible = true;
						cboView.Visible = true;
						cboFilter.Visible = true;
						cboStoreAttribute.Visible = true;
						cboAttributeSet.Visible = true;
						cboUnitScaling.Visible = true;
						cboDollarScaling.Visible = true;

						BindFilterComboBox();
						BindStoreAttrComboBox();
						break;

					case ePlanSessionType.StoreMultiLevel:
						cmiRowChooser.Visible = true;
						cmiColumnChooser.Visible = true;
						cmig2g3Seperator1.Visible = true;
						cmiQuantityChooser.Visible = false;
						cmiVariableChooser.Visible = false;
						pnlGroupBy.Visible = true;
						pnlSpacer2.Visible = true;
						cboView.Visible = true;
						cboFilter.Visible = true;
						cboStoreAttribute.Visible = true;
						cboAttributeSet.Visible = true;
						cboUnitScaling.Visible = true;
						cboDollarScaling.Visible = true;

						BindFilterComboBox();
						BindStoreAttrComboBox();
						break;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5006 - JScott - Display Low-levels one at a time
		private void BuildBasisLabelsAndToolTip(HierarchyNodeProfile aHierarchyNodeProfile, BasisProfile aBasisProfile)
        {
            string toolTipStr;
            string newLine;
            int i;
            System.Windows.Forms.ToolTip toolTip;

            try
            {
                //Begin Track #5782 - KJohnson - Basis Labels Not Showing Correct Information
                //foreach (BasisProfile basisProfile in _openParms.BasisProfileList)
                //{
                //End Track #5782 - KJohnson
                    //-----------Fill In ToolTip---------------------------------------------
                    toolTipStr = "";
                    newLine = "";
                    i = 0;

                    foreach (BasisDetailProfile basisDetailProfile in aBasisProfile.BasisDetailProfileList)
                    {
                        i++;
                        toolTipStr += newLine + "Detail " + Convert.ToInt32(i, CultureInfo.CurrentUICulture) + ": " + basisDetailProfile.HierarchyNodeProfile.Text + " / " + basisDetailProfile.VersionProfile.Description + " / " + basisDetailProfile.DateRangeProfile.DisplayDate + " / " + Convert.ToString(basisDetailProfile.Weight, CultureInfo.CurrentUICulture);
                        newLine = System.Environment.NewLine;
                    }

                    toolTip = new System.Windows.Forms.ToolTip(this.components);
                    toolTip.Active = false;
                    toolTip.SetToolTip(g4, toolTipStr);
                    toolTip.SetToolTip(g7, toolTipStr);
                    toolTip.SetToolTip(g10, toolTipStr);

					_basisToolTipList[new HashKeyObject(aHierarchyNodeProfile.Key, aBasisProfile.Key)] = toolTip;


                    //-----------Fill In Basis Labels---------------------------------------------
                    string tmpLabel = _basisLabel;
                    foreach (BasisDetailProfile basisDetailProfile in aBasisProfile.BasisDetailProfileList)
                    {
                        tmpLabel = tmpLabel.Replace("Merchandise", basisDetailProfile.HierarchyNodeProfile.Text);
                        tmpLabel = tmpLabel.Replace("Version", basisDetailProfile.VersionProfile.Description);
                        tmpLabel = tmpLabel.Replace("Time_Period", basisDetailProfile.DateRangeProfile.DisplayDate);
                    //Begin Track #5782 - KJohnson - Basis Labels Not Showing Correct Information
                    if (tmpLabel == "")
                    {
                        tmpLabel = aBasisProfile.Name;
                    }
                    else
                    {
                        aBasisProfile.Name = tmpLabel;
                    }
                    //End Track #5782 - KJohnson
                    break;
                }
                _basisLabelList[new HashKeyObject(aHierarchyNodeProfile.Key, aBasisProfile.Key)] = tmpLabel;
                //Begin Track #5782 - KJohnson - Basis Labels Not Showing Correct Information
                //}
                //End Track #5782 - KJohnson
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildTimeHeaders()
        {
            int i;

            try
            {
                i = 0;

                _selectableTimeHeaders = new ArrayList();

				//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
				//if (cmiShowPeriods.Checked)
				//{
				//    foreach (PeriodProfile perProf in _openParms.GetPeriodProfileList(_sab.ClientServerSession))
				//    {
				//        _selectableTimeHeaders.Add(new RowColProfileHeader(perProf.Text(), true, i, perProf));
				//        i++;

				//        if (cmiShowWeeks.Checked)
				//        {
				//            foreach (WeekProfile weekProf in perProf.Weeks)
				//            {
				//                _selectableTimeHeaders.Add(new RowColProfileHeader(weekProf.Text(), true, i, weekProf));
				//                i++;
				//            }
				//        }
				//    }
				//}
				//else
				//{
				//    foreach (WeekProfile weekProf in _openParms.GetWeekProfileList(_sab.ClientServerSession))
				//    {
				//        _selectableTimeHeaders.Add(new RowColProfileHeader(weekProf.Text(), true, i, weekProf));
				//        i++;
				//    }
				//}
				BuildPeriodHeaders(_periodProfileList, _openParms.GetWeekProfileList(_sab.ClientServerSession), ref i);
				//End Track #5121 - JScott - Add Year/Season/Quarter totals

                CreateSortedList(_selectableTimeHeaders, out _sortedTimeHeaders);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		private void BuildPeriodHeaders(ProfileList aPeriodList, ProfileList aWeekList, ref int aSeq)
		{
			try
			{
				if (aPeriodList.ProfileType == eProfileType.Period)
				{
					foreach (PeriodProfile perProf in aPeriodList)
					{
						if (_periodHeaderHash.Contains((int)perProf.PeriodProfileType))
						{
							_selectableTimeHeaders.Add(new RowColProfileHeader(perProf.Text(), true, aSeq++, perProf));
						}

						if (perProf.ChildPeriodList.Count > 0)
						{
							BuildPeriodHeaders(perProf.ChildPeriodList, aWeekList, ref aSeq);
						}
						else
						{
							BuildPeriodHeaders(perProf.Weeks, aWeekList, ref aSeq);
						}
					}
				}
				else
				{
					if (_periodHeaderHash.Contains((int)aPeriodList.ProfileType))
					{
						foreach (WeekProfile weekProf in aPeriodList)
						{
							if (aWeekList.Contains(weekProf.Key))
							{
								_selectableTimeHeaders.Add(new RowColProfileHeader(weekProf.Text(), true, aSeq++, weekProf));
							}
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

		//End Track #5121 - JScott - Add Year/Season/Quarter totals
        private void PlanView_Load(object sender, System.EventArgs e)
        {
            try
            {
                _formLoading = true;

                g2.Visible = false;
                g3.Visible = false;
                g4.Visible = false;
                g5.Visible = false;
                g6.Visible = false;
                g7.Visible = false;
                g8.Visible = false;
                g9.Visible = false;
                g10.Visible = false;
                g11.Visible = false;
                g12.Visible = false;

                g2.Visible = ((PagingGridTag)g2.Tag).Visible;
                g3.Visible = ((PagingGridTag)g3.Tag).Visible;
                g4.Visible = ((PagingGridTag)g4.Tag).Visible;
                g5.Visible = ((PagingGridTag)g5.Tag).Visible;
                g6.Visible = ((PagingGridTag)g6.Tag).Visible;
                g7.Visible = ((PagingGridTag)g7.Tag).Visible;
                g8.Visible = ((PagingGridTag)g8.Tag).Visible;
                g9.Visible = ((PagingGridTag)g9.Tag).Visible;
                g10.Visible = ((PagingGridTag)g10.Tag).Visible;
                g11.Visible = ((PagingGridTag)g11.Tag).Visible;
                g12.Visible = ((PagingGridTag)g12.Tag).Visible;

                CalcColSplitPosition2(false);
                CalcColSplitPosition3(false);
                SetColSplitPositions();
                CalcRowSplitPosition4(false);
                CalcRowSplitPosition8(false);
                CalcRowSplitPosition12(false);
                SetRowSplitPositions();
                SetHScrollBar2Parameters();
                SetHScrollBar3Parameters();
                SetVScrollBar2Parameters();
                SetVScrollBar3Parameters();
                SetVScrollBar4Parameters();
                HighlightActiveAttributeSet();

                _formLoading = false;
				//Begin Init Performance Benchmarking -- DO NOT REMOVE

				//System.IO.StreamWriter perfOut = new StreamWriter(System.Environment.CurrentDirectory + "/InitTimings.out", false);
				//_planCubeGroup.PerfInitHash.SortAndPrint(perfOut);
				//perfOut.Close();
				//End Init Performance Benchmarking -- DO NOT REMOVE
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                HandleExceptions(exc);
            }
        }

        private void PlanView_Activated(object sender, System.EventArgs e)
        {
            try
            {
                g6.Focus();

                if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
                {
                    g6.Select(0, 0);
                }
                else
                {
                    g6.Select(-1, -1);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                HandleExceptions(exc);
            }
        }

        #endregion

        #region ComboBox Binding and Selection

        private void BindFilterComboBox()
        {
            DataTable dtFilter;

            try
            {
                _bindingFilter = true;

                cboFilter.Items.Clear();
                cboFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));	// Issue 3806

				dtFilter = _storeFilterDL.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, _filterUserRIDList);

                foreach (DataRow row in dtFilter.Rows)
                {
                    cboFilter.Items.Add(
                        new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
                        Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
                        Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
                }

                //BEGIN TT#6-MD-VStuart - Single Store Select
                // If a single store has been selected, then lets add that store to the combobox list.
                if (!String.IsNullOrEmpty(_openParms.StoreId) && _openParms.StoreId != "(None)")
                {
                    cboFilter.Items.Add(
                        new FilterNameCombo(-2, StoreMgmt.UserID, _openParms.StoreIdNm)); //new FilterNameCombo(-2, _sab.StoreServerSession.UserID, _openParms.StoreIdNm));
                    cboFilter.SelectedItem = new FilterNameCombo(-2);
                }
                else
                {
                    cboFilter.SelectedItem = new FilterNameCombo(_lastFilterValue);
                }
                //END TT#6-MD-VStuart - Single Store Select

                AdjustTextWidthComboBox_DropDown(cboFilter);  // TT#1401 - Agallagher - VSW
                _bindingFilter = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BindViewComboBox()
        {
            try
            {
                _bindingView = true;

                _lastViewValue = _openParms.ViewRID;
                cboView.ValueMember = "VIEW_RID";
				cboView.DisplayMember = "DISPLAY_ID";
				cboView.DataSource = _dtView;
                cboView.SelectedValue = _openParms.ViewRID;

                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                //var z = new EventArgs();
                //this.cboView_SelectionChangeCommitted(this, z);	//This will "Execute" the event handler.
                //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

                AdjustTextWidthComboBox_DropDown(cboView);  // TT#1401 - Agallagher - VSW
                _bindingView = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#316 - JSmith - Replace all Windows Combobox controls with new enhanced control
        void cboView_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboView_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboDollarScaling_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboDollarScaling_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboUnitScaling_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboUnitScaling_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboFilter_SelectionChangeCommitted(source, new EventArgs());
        }
        void cboAttributeSet_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboAttributeSet_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        // End TT#316 - JSmith - Replace all Windows Combobox controls with new enhanced control

        
        //BEGIN TT#6-MD-VStuart - Single Store Select
        //private void cboFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
        //{
            //FilterNameCombo filterNameCbo;

            //try
            //{
            //    filterNameCbo = (FilterNameCombo)cboFilter.SelectedItem;

            //    if (cboFilter.SelectedIndex != -1)
            //    {
            //        if (!_bindingFilter && filterNameCbo.FilterRID != _lastFilterValue)
            //        {
            //            if (!_formLoading)
            //            {
            //                Cursor.Current = Cursors.WaitCursor;
            //                SetGridRedraws(false);

            //                try
            //                {
            //                    StopPageLoadThreads();

            //                    //Begin Track #5006 - JScott - Display Low-levels one at a time
            //                    //if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel || _openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
            //                    if (_currentPlanSessionType == ePlanSessionType.StoreSingleLevel || _currentPlanSessionType == ePlanSessionType.StoreMultiLevel)
            //                    //End Track #5006 - JScott - Display Low-levels one at a time
            //                    {
            //                        _planCubeGroup.SetStoreFilter(((FilterNameCombo)cboFilter.SelectedItem).FilterRID);
            //                        _storeProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.Store);

            //                        if (_storeProfileList.Count == 0)
            //                        {
            //                            MessageBox.Show("Applied filter(s) have resulted in no displayable Stores.", "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //                        }

            //                        _workingDetailProfileList = new ProfileList(eProfileType.Store);
            //                        BuildWorkingStoreList(_lastAttributeSetValue, _workingDetailProfileList);
            //                        ReformatRowsChanged(false);

            //                        _lastFilterValue = cboFilter.SelectedIndex;
            //                    }
            //                }
            //                //Begin TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed
            //                catch (MIDException ex)
            //                {
            //                    HandleMIDException(ex);
            //                }
            //                //End TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed
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
            //        }

            //        if (((FilterNameCombo)cboFilter.SelectedItem).FilterRID == -1)
            //        {
            //            cboFilter.SelectedIndex = -1;
            //        }
            //    }
            //}
            //catch (Exception exc)
            //{
            //    HandleExceptions(exc);
            //}
        //}
        //END TT#6-MD-VStuart - Single Store Select

        private void cboFilter_DropDown(object sender, System.EventArgs e)
        {
            try
            {
                BindFilterComboBox();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void cboFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                Image_DragEnter(sender, e);
                if (e.Data.GetDataPresent(typeof(MIDFilterNode)))
                {
                    e.Effect = DragDropEffects.All;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void cboFilter_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                    ((MIDComboBoxEnh)((ComboBox)sender).Parent).FirePropertyChangeEvent();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        ///// <summary>
        ///// Populate all values of the Store_Group_Levels (Attribute Sets)
        ///// (based on key from Store_Group) of the cboStoreAttribute
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void cboView_SelectionChangeCommitted(object sender, System.EventArgs e)
        //{
        //    int selectedValue;
        //    DataRow row;

        //    try
        //    {
        //        row = ((DataRowView)cboView.SelectedItem).Row;

        //        selectedValue = Convert.ToInt32(row["VIEW_RID"], CultureInfo.CurrentUICulture);

        //        if ((!_bindingView && selectedValue != _lastViewValue) ||
        //            (_bindingView && selectedValue == _lastViewValue))
        //        {
        //            if (!_formLoading)
        //            {
        //                try
        //                {
        //                    Cursor.Current = Cursors.WaitCursor;

        //                    _openParms.ViewRID = selectedValue;
        //                    _openParms.ViewName = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
        //                    _openParms.ViewUserID = Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture);

        //                    LoadView();
        //                    //Begin Track #5969 - JScott - Views don't observe "show" (wks, mos, qts,sea) properties
        //                    BuildTimeHeaders();
        //                    //End Track #5969 - JScott - Views don't observe "show" (wks, mos, qts,sea) properties

        //                    SetGridRedraws(false);
        //                    StopPageLoadThreads();
        //                    ReformatRowsChanged(true);

        //                    _lastViewValue = selectedValue;
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
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        HandleExceptions(exc);
        //    }
        //}

        /// <summary>
        /// Populate all Store_Groups (Attributes); 1st sel if new else selection made
        /// in load
        /// </summary>
        private void BindStoreAttrComboBox()
        {
            ProfileList attrList;
            // Begin Track #4872 - JSmith - Global/User Attributes
            FunctionSecurityProfile userAttrSecLvl;
            // End Track #4872

            try
            {
                _bindingAttribute = true;

               // attrList = (ProfileList)_storeGroupListViewProfileList.Clone();   // TT#1517-MD - Store Service Optimization - SRISCH 
                attrList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MyUserAndGlobal, true);   // TT#1517-MD - Store Service Optimization - SRISCH

                _lastAttributeValue = _openParms.StoreGroupRID;
                // Begin Track #4872 - JSmith - Global/User Attributes
                //cboStoreAttribute.ValueMember = "Key";
                //cboStoreAttribute.DisplayMember = "Name";
                //cboStoreAttribute.DataSource = attrList.ArrayList;
                userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
                if (FunctionSecurity == null) // set security
                {
                    FunctionSecurity = new FunctionSecurityProfile(0);
                    FunctionSecurity.SetFullControl();
                }
                cboStoreAttribute.Initialize(SAB, FunctionSecurity, attrList.ArrayList, !userAttrSecLvl.AccessDenied);
                // End Track #4872
                cboStoreAttribute.SelectedValue = _openParms.StoreGroupRID;
                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                ////var z = new EventArgs();
                ////this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());	//This will "Execute" the event handler.
                //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                AdjustTextWidthComboBox_DropDown(cboStoreAttribute);  // TT#1401 - Agallagher - VSW
                _bindingAttribute = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
        /// <summary>
        /// Populate all values of the Store_Group_Levels (Attribute Sets)
        /// (based on key from Store_Group) of the cboStoreAttribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if ((!_bindingAttribute && (int)cboStoreAttribute.SelectedValue != _lastAttributeValue) ||
        //            (_bindingAttribute && (int)cboStoreAttribute.SelectedValue == _lastAttributeValue))
        //        {
        //            if (!_formLoading)
        //            {
        //                Cursor.Current = Cursors.WaitCursor;
        //                StopPageLoadThreads();

        //                if (_planCubeGroup.GetType() == typeof(StoreMultiLevelPlanMaintCubeGroup))
        //                {
        //                    ((StoreMultiLevelPlanMaintCubeGroup)_planCubeGroup).SetStoreGroup(new StoreGroupProfile(((StoreGroupListViewProfile)_storeGroupListViewProfileList.FindKey((int)cboStoreAttribute.SelectedValue)).Key));
        //                }
        //                else
        //                {
        //                    ((StorePlanMaintCubeGroup)_planCubeGroup).SetStoreGroup(new StoreGroupProfile(((StoreGroupListViewProfile)_storeGroupListViewProfileList.FindKey((int)cboStoreAttribute.SelectedValue)).Key));
        //                }
        //                _storeGroupLevelProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);

        //                _selectableStoreAttributeHeaders = new ArrayList();
        //                foreach (StoreGroupLevelProfile strGrpLvlProf in _storeGroupLevelProfileList)
        //                {
        //                    //Begin Track #6261 - JScott - Sets in Set summary section are out of order
        //                    //_selectableStoreAttributeHeaders.Add(new RowColProfileHeader(strGrpLvlProf.Name, true, 0, strGrpLvlProf));
        //                    _selectableStoreAttributeHeaders.Add(new RowColProfileHeader(strGrpLvlProf.Name, true, strGrpLvlProf.Sequence, strGrpLvlProf));
        //                    //End Track #6261 - JScott - Sets in Set summary section are out of order
        //                }

        //                Cursor.Current = Cursors.Default;
        //            }

        //            BindStoreAttrSetComboBox();

        //            _lastAttributeValue = (int)cboStoreAttribute.SelectedValue;
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        HandleExceptions(exc);
        //    }
        //}
        //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

        private void cboStoreAttribute_DragDrop(object sender, DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }
        /// <summary>
        /// Populate all values of the Store_Group_Levels (Attribute Sets)
        /// based on the key parameter.
        /// </summary>
        private void BindStoreAttrSetComboBox()
        {
            ProfileList attrSetList;

            try
            {
                _bindingAttributeSet = true;

                attrSetList = new ProfileList(eProfileType.StoreGroupLevel);

                foreach (StoreGroupLevelProfile sglProf in _storeGroupLevelProfileList)
                {
                    attrSetList.Add(sglProf);
                }

                _lastAttributeSetValue = attrSetList[0].Key;
                cboAttributeSet.ValueMember = "Key";
                cboAttributeSet.DisplayMember = "Name";
                cboAttributeSet.DataSource = attrSetList.ArrayList;
                cboAttributeSet.SelectedValue = attrSetList[0].Key;

                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                ////var z = new EventArgs();
                //this.cboAttributeSet_SelectionChangeCommitted(this, z);	//This will "Execute" the event handler.
                //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                
                AdjustTextWidthComboBox_DropDown(cboAttributeSet);  // TT#1401 - Agallagher - VSW
                _bindingAttributeSet = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
        //private void cboAttributeSet_SelectionChangeCommitted(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if ((!_bindingAttributeSet && (int)cboAttributeSet.SelectedValue != _lastAttributeSetValue) ||
        //            (_bindingAttributeSet && (int)cboAttributeSet.SelectedValue == _lastAttributeSetValue))
        //        {
        //            try
        //            {
        //                _workingDetailProfileList = new ProfileList(eProfileType.Store);
        //                BuildWorkingStoreList((int)cboAttributeSet.SelectedValue, _workingDetailProfileList);
        //            }
        //            catch (Exception exc)
        //            {
        //                HandleExceptions(exc);
        //            }

        //            if (!_formLoading)
        //            {
        //                try
        //                {
        //                    Cursor.Current = Cursors.WaitCursor;

        //                    SetGridRedraws(false);
        //                    StopPageLoadThreads();
        //                    ReformatAttributeChanged(false);
        //                    HighlightActiveAttributeSet();

        //                    _lastAttributeSetValue = (int)cboAttributeSet.SelectedValue;
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
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        HandleExceptions(exc);
        //    }
        //}
        //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

		private void BuildWorkingStoreList(int aAttributeSetKey, ProfileList aWorkingDetailProfileList)
		{
			ProfileXRef storeSetXRef;
			ArrayList detailList;
			StoreProfile storeProf;

			try
			{
				storeSetXRef = (ProfileXRef)_planCubeGroup.GetProfileXRef(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
				detailList = storeSetXRef.GetDetailList(aAttributeSetKey);
				if (detailList != null)
				{
					foreach (int storeId in detailList)
					{
						storeProf = (StoreProfile)_storeProfileList.FindKey(storeId);
						if (storeProf != null)
						{
							aWorkingDetailProfileList.Add(storeProf);
						}
					}
				}
			}
			catch 
			{
				throw;
			}
		}

        private void BindUnitScalingComboBox()
        {
            DataTable dtText;
            ArrayList scalingList;

            try
            {
                _bindingUnitScaling = true;

                scalingList = new ArrayList();
                dtText = MIDText.GetTextType(eMIDTextType.eUnitScaling, eMIDTextOrderBy.TextValue);

				//Begin Modification - JScott - Add Scaling Decimals
				//scalingList.Add(new NumericComboObject((int)eUnitScaling.Ones, 1));
				scalingList.Add(new ComboObject((int)eUnitScaling.Ones, "1"));
				//End Modification - JScott - Add Scaling Decimals

                foreach (DataRow row in dtText.Rows)
                {
					//Begin Modification - JScott - Add Scaling Decimals
					//try
					//{
					//    scalingList.Add(new NumericComboObject(Convert.ToInt32(row["TEXT_CODE"]), Convert.ToInt32(row["TEXT_VALUE"])));
					//}
					//catch (System.FormatException)
					//{
					//}
					//catch (Exception exc)
					//{
					//    string message = exc.ToString();
					//    throw;
					//}
					if (Convert.ToString(row["TEXT_VALUE"]) != string.Empty)
					{
						scalingList.Add(new ComboObject(Convert.ToInt32(row["TEXT_CODE"]), Convert.ToString(row["TEXT_VALUE"])));
					}
					//End Modification - JScott - Add Scaling Decimals
                }

                cboUnitScaling.DataSource = scalingList;
                cboUnitScaling.ValueMember = "Key";
				//Begin Modification - JScott - Add Scaling Decimals
				//cboUnitScaling.DisplayMember = "ValueAsString";
				cboUnitScaling.DisplayMember = "Value";
				//End Modification - JScott - Add Scaling Decimals

                cboUnitScaling.SelectedValue = (int)eUnitScaling.Ones;
                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                ////var z = new EventArgs();
                //this.cboUnitScaling_SelectionChangeCommitted(this, z);	//This will "Execute" the event handler.
                //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

                _bindingUnitScaling = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
       //private void cboUnitScaling_SelectionChangeCommitted(object sender, System.EventArgs e)
       // {
       //     //Begin Modification - JScott - Add Scaling Decimals
       //     //NumericComboObject comboObj;
       //     ComboObject comboObj;
       //     //End Modification - JScott - Add Scaling Decimals

       //     try
       //     {
       //         //Begin Modification - JScott - Add Scaling Decimals
       //         //comboObj = (NumericComboObject)cboUnitScaling.SelectedItem;
       //         comboObj = (ComboObject)cboUnitScaling.SelectedItem;
       //         //End Modification - JScott - Add Scaling Decimals

       //         if (comboObj != null && !_formLoading && !_bindingUnitScaling && comboObj.Key != _lastUnitScalingValue)
       //         {
       //             try
       //             {
       //                 Cursor.Current = Cursors.WaitCursor;

       //                 SetGridRedraws(false);
       //                 StopPageLoadThreads();
       //                 LoadCurrentPages();
       //                 ResizeCol1();
       //                 ResizeCol2();
       //                 ResizeCol3();
       //                 CalcColSplitPosition2(false);
       //                 CalcColSplitPosition3(false);
       //                 SetColSplitPositions();

       //                 _lastUnitScalingValue = comboObj.Key;
       //             }
       //             catch (Exception exc)
       //             {
       //                 HandleExceptions(exc);
       //             }
       //             finally
       //             {
       //                 SetGridRedraws(true);
       //                 LoadSurroundingPages();
       //                 Cursor.Current = Cursors.Default;
       //                 g6.Focus();
       //             }
       //         }
       //     }
       //     catch (Exception exc)
       //     {
       //         HandleExceptions(exc);
       //     }
       // }
        //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
 
        private void BindDollarScalingComboBox()
        {
            DataTable dtText;
            ArrayList scalingList;

            try
            {
                _bindingDollarScaling = true;

                scalingList = new ArrayList();
                dtText = MIDText.GetTextType(eMIDTextType.eDollarScaling, eMIDTextOrderBy.TextValue);

				//Begin Modification - JScott - Add Scaling Decimals
				//scalingList.Add(new NumericComboObject((int)eDollarScaling.Ones, 1));
				scalingList.Add(new ComboObject((int)eDollarScaling.Ones, "1"));
				//End Modification - JScott - Add Scaling Decimals

                foreach (DataRow row in dtText.Rows)
                {
					//Begin Modification - JScott - Add Scaling Decimals
					//try
					//{
					//    scalingList.Add(new NumericComboObject(Convert.ToInt32(row["TEXT_CODE"]), Convert.ToInt32(row["TEXT_VALUE"])));
					//}
					//catch (System.FormatException)
					//{
					//}
					//catch (Exception exc)
					//{
					//    string message = exc.ToString();
					//    throw;
					//}
					if (Convert.ToString(row["TEXT_VALUE"]) != string.Empty)
					{
						scalingList.Add(new ComboObject(Convert.ToInt32(row["TEXT_CODE"]), Convert.ToString(row["TEXT_VALUE"])));
					}
					//End Modification - JScott - Add Scaling Decimals
				}

                cboDollarScaling.DataSource = scalingList;
                cboDollarScaling.ValueMember = "Key";
				//Begin Modification - JScott - Add Scaling Decimals
				//cboDollarScaling.DisplayMember = "ValueAsString";
				cboDollarScaling.DisplayMember = "Value";
				//End Modification - JScott - Add Scaling Decimals

                cboDollarScaling.SelectedValue = (int)eDollarScaling.Ones;
                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                ////var z = new EventArgs();
                //this.cboDollarScaling_SelectionChangeCommitted(this, z);    //This will "Execute" the event handler.
                //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

                _bindingDollarScaling = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
        //private void cboDollarScaling_SelectionChangeCommitted(object sender, System.EventArgs e)
        //{
        //    //Begin Modification - JScott - Add Scaling Decimals
        //    //NumericComboObject comboObj;
        //    ComboObject comboObj;
        //    //End Modification - JScott - Add Scaling Decimals

        //    try
        //    {
        //        //Begin Modification - JScott - Add Scaling Decimals
        //        //comboObj = (NumericComboObject)cboDollarScaling.SelectedItem;
        //        comboObj = (ComboObject)cboDollarScaling.SelectedItem;
        //        //End Modification - JScott - Add Scaling Decimals

        //        if (comboObj != null && !_formLoading && !_bindingDollarScaling && comboObj.Key != _lastDollarScalingValue)
        //        {
        //            try
        //            {
        //                Cursor.Current = Cursors.WaitCursor;

        //                SetGridRedraws(false);
        //                StopPageLoadThreads();
        //                LoadCurrentPages();
        //                ResizeCol1();
        //                ResizeCol2();
        //                ResizeCol3();
        //                CalcColSplitPosition2(false);
        //                CalcColSplitPosition3(false);
        //                SetColSplitPositions();

        //                _lastDollarScalingValue = comboObj.Key;
        //            }
        //            catch (Exception exc)
        //            {
        //                HandleExceptions(exc);
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
        //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

        #endregion

        #region Miscellaneous Initialization

        private void BuildMenu()
        {
            try
            {
                ButtonTool btUndo;
				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//ButtonTool btFind;
				ButtonTool btTheme;
				//End Track #5006 - JScott - Display Low-levels one at a time
				ButtonTool btExport;

                btExport = new ButtonTool(Include.btExport);
                btExport.SharedProps.Caption = "&Export";
                btExport.SharedProps.Shortcut = Shortcut.CtrlE;
                btExport.SharedProps.MergeOrder = 10;
                utmMain.Tools.Add(btExport);

				FileMenuTool.Tools.Add(btExport);

                btUndo = new ButtonTool("btUndo");
                btUndo.SharedProps.Caption = "&Undo";
                btUndo.SharedProps.Shortcut = Shortcut.CtrlZ;
                btUndo.SharedProps.MergeOrder = 0;
                btUndo.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.UndoImage);
                utmMain.Tools.Add(btUndo);
				EditMenuTool.Tools.Add(btUndo);

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (_openParms.PlanSessionType != ePlanSessionType.ChainSingleLevel)
				//{
				//    btFind = new ButtonTool(Include.btFind);
				//    btFind.SharedProps.Caption = "&Find";
				//    btFind.SharedProps.Shortcut = Shortcut.CtrlF;
				//    btFind.SharedProps.MergeOrder = 20;
				//    btFind.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.FindImage);
				//    utmMain.Tools.Add(btFind);
				//    EditMenuTool.Tools.Add(btFind);
				//}
				btTheme = new ButtonTool("btTheme");
				btTheme.SharedProps.Caption = "T&heme";
				btTheme.SharedProps.Shortcut = Shortcut.CtrlH;
				btTheme.SharedProps.MergeOrder = 0;
				btTheme.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.ThemeImage);
				utmMain.Tools.Add(btTheme);
				ToolsMenuTool.Tools.Add(btTheme);

				BuildFindMenu();
				//End Track #5006 - JScott - Display Low-levels one at a time

				EditMenuTool.Tools["btUndo"].InstanceProps.IsFirstInGroup = true;
			}
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		//Begin Track #5006 - JScott - Display Low-levels one at a time
		private void BuildFindMenu()
		{
            ButtonTool btFind;
			int index;

			try
			{
				if (_currentPlanSessionType != ePlanSessionType.ChainSingleLevel)
				{
					index = utmMain.Tools.IndexOf(Include.btFind);

					if (index == -1)
					{
						btFind = new ButtonTool(Include.btFind);
						btFind.SharedProps.Caption = "&Find";
						btFind.SharedProps.Shortcut = Shortcut.CtrlF;
						btFind.SharedProps.MergeOrder = 20;
						btFind.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.FindImage);

						utmMain.Tools.Add(btFind);
						EditMenuTool.Tools.Add(btFind);
					}
				}
				else
				{
					index = utmMain.Tools.IndexOf(Include.btFind);

					if (index != -1)
					{
						utmMain.Tools.RemoveAt(index);
					}

					index = EditMenuTool.Tools.IndexOf(Include.btFind);

					if (index != -1)
					{
						EditMenuTool.Tools.RemoveAt(index);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5006 - JScott - Display Low-levels one at a time
		private void LoadView()
		{
            int i;
            VariableProfile viewVarProf;
            QuantityVariableProfile viewQVarProf;
            DataRow viewRow;
            Hashtable varKeyHash;
			//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
			Hashtable perKeyHash;
			bool selectYear;
			bool selectSeason;
			bool selectQuarter;
			bool selectMonth;
			bool selectWeek;
			//End Track #5121 - JScott - Add Year/Season/Quarter totals
			Hashtable qVarKeyHash;
            bool cont;

            try
            {
                //Read PlanViewDetail table

				_planViewDetail = _planViewData.PlanViewDetail_Read(_openParms.ViewRID);

                //Load columns

                varKeyHash = new Hashtable();
                _selectableVariableHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
					//if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Column)
					//{
					//    viewVarProf = (VariableProfile)_variableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));
					//    if (viewVarProf != null)
					//    {
					//        varKeyHash.Add(viewVarProf.Key, row);
					//    }
					//}
					if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Variable)
                    {
						if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.Variable)
						{
							viewVarProf = (VariableProfile)_variableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));

							if (viewVarProf != null)
							{
								varKeyHash.Add(viewVarProf.Key, row);
							}
						}
					}
					//End Track #5121 - JScott - Add Year/Season/Quarter totals
                }

                foreach (VariableProfile varProf in _variableProfileList)
                {
                    viewRow = (DataRow)varKeyHash[varProf.Key];
                    if (viewRow != null)
                    {
                        // Begin Track #4868 - JSmith - Variable Groupings
                        //_selectableVariableHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf));
                        _selectableVariableHeaders.Add(new RowColProfileHeader(varProf.VariableName, true, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), varProf, varProf.Groupings));
                        // End Track #4868
                    }
                    else
                    {
                        // Begin Track #4868 - JSmith - Variable Groupings
                        //_selectableVariableHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, -1, varProf));
                        _selectableVariableHeaders.Add(new RowColProfileHeader(varProf.VariableName, false, -1, varProf, varProf.Groupings));
                        // End Track #4868
                    }
                }

                CreateSortedList(_selectableVariableHeaders, out _sortedVariableHeaders);

				if (_sortedVariableHeaders.Count == 0)
				{
					MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_pl_NoDisplayableVariables), "No Displayable Variables", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				
				//Load rows

                qVarKeyHash = new Hashtable();
                _selectableQuantityHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
					//if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Row)
					if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Quantity)
					//End Track #5121 - JScott - Add Year/Season/Quarter totals
					{
						if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.QuantityVariable)
                        {
                            viewQVarProf = (QuantityVariableProfile)_quantityVariableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));

                            if (viewQVarProf != null)
                            {
                                qVarKeyHash.Add(viewQVarProf.Key, row);
                            }
                        }
                    }
                }

				_quantityVariables = _transaction.PlanComputations.PlanQuantityVariables;

				_currentRowHeader = new RowColProfileHeader("Current Plan", true, 0, _quantityVariables.ValueQuantity);
				_originalRowHeader = new RowColProfileHeader("Original Plan", false, 1, _quantityVariables.ValueQuantity);

				viewRow = (DataRow)qVarKeyHash[_quantityVariables.ValueQuantity.Key];

				if (viewRow != null)
				{
					_adjustmentRowHeader = new RowColProfileHeader("Adjusted Plan", true, 0, _quantityVariables.ValueQuantity);
				}
				else
				{
					_adjustmentRowHeader = new RowColProfileHeader("Adjusted Plan", false, 0, _quantityVariables.ValueQuantity);
				}

				_selectableQuantityHeaders.Add(_adjustmentRowHeader);
                i = 2;

                foreach (QuantityVariableProfile qVarProf in _quantityVariableProfileList)
                {
                    cont = false;

					//Begin Track #5006 - JScott - Display Low-levels one at a time
					//switch (_openParms.PlanSessionType)
					switch (_currentPlanSessionType)
					//End Track #5006 - JScott - Display Low-levels one at a time
					{
                        case ePlanSessionType.ChainMultiLevel:
                            if (qVarProf.isChainMultiView &&
                                (qVarProf.isChainDetailCube))
                            {
                                cont = true;
                            }
                            break;

                        case ePlanSessionType.ChainSingleLevel:
                            if (qVarProf.isChainSingleView && qVarProf.isHighLevel &&
                                qVarProf.isChainDetailCube)
                            {
                                cont = true;
                            }
                            break;

                        case ePlanSessionType.StoreMultiLevel:
                            if (qVarProf.isStoreMultiView &&
                                (qVarProf.isChainDetailCube || qVarProf.isStoreDetailCube || qVarProf.isStoreSetCube || qVarProf.isStoreTotalCube))
                            {
                                cont = true;
                            }
                            break;

                        case ePlanSessionType.StoreSingleLevel:
                            if (qVarProf.isStoreSingleView && qVarProf.isHighLevel &&
                                (qVarProf.isChainDetailCube || qVarProf.isStoreDetailCube || qVarProf.isStoreSetCube || qVarProf.isStoreTotalCube))
                            {
                                cont = true;
                            }
                            break;
                    }

                    if (qVarProf.isSelectable && cont)
                    {
                        viewRow = (DataRow)qVarKeyHash[qVarProf.Key];
                        if (viewRow != null)
                        {
                            _selectableQuantityHeaders.Add(new RowColProfileHeader(qVarProf.VariableName, true, i, qVarProf));
                        }
                        else
                        {
                            _selectableQuantityHeaders.Add(new RowColProfileHeader(qVarProf.VariableName, false, i, qVarProf));
                        }
                        i++;
                    }
                }
				//Begin Track #5121 - JScott - Add Year/Season/Quarter totals

				// Load Periods

				perKeyHash = new Hashtable();
				_selectablePeriodHeaders = new ArrayList();

				foreach (DataRow row in _planViewDetail.Rows)
				{
					if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Period)
					{
						if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.Period)
						{
							perKeyHash.Add(Convert.ToInt32(row["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), row);
						}
					}
				}

				selectMonth = (DataRow)perKeyHash[(int)eProfileType.Month] != null;
				selectWeek = (DataRow)perKeyHash[(int)eProfileType.Week] != null;

				if (_openParms.PlanSessionType == ePlanSessionType.ChainMultiLevel ||
					_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
				{
					selectYear = (DataRow)perKeyHash[(int)eProfileType.Year] != null;
					selectSeason = (DataRow)perKeyHash[(int)eProfileType.Season] != null;
					selectQuarter = (DataRow)perKeyHash[(int)eProfileType.Quarter] != null;

					if (selectYear)
					{
						_selectablePeriodHeaders.Add(new RowColProfileHeader("Show Years", true, (int)eProfileType.Year, null));
					}
					else
					{
						_selectablePeriodHeaders.Add(new RowColProfileHeader("Show Years", false, (int)eProfileType.Year, null));
					}

					if (selectSeason)
					{
						_selectablePeriodHeaders.Add(new RowColProfileHeader("Show Seasons", true, (int)eProfileType.Season, null));
					}
					else
					{
						_selectablePeriodHeaders.Add(new RowColProfileHeader("Show Seasons", false, (int)eProfileType.Season, null));
					}

					if (selectQuarter)
					{
						_selectablePeriodHeaders.Add(new RowColProfileHeader("Show Quarters", true, (int)eProfileType.Quarter, null));
					}
					else
					{
						_selectablePeriodHeaders.Add(new RowColProfileHeader("Show Quarters", false, (int)eProfileType.Quarter, null));
					}

					if (!selectYear && !selectSeason && !selectQuarter && !selectMonth && !selectWeek)
					{
						selectMonth = true;
					}
				}
				else
				{
					if (!selectMonth && !selectWeek)
					{
						selectMonth = true;
					}
				}

				if (selectMonth)
				{
					_selectablePeriodHeaders.Add(new RowColProfileHeader("Show Months", true, (int)eProfileType.Month, null));
				}
				else
				{
					_selectablePeriodHeaders.Add(new RowColProfileHeader("Show Months", false, (int)eProfileType.Month, null));
				}

				if (selectWeek)
				{
					_selectablePeriodHeaders.Add(new RowColProfileHeader("Show Weeks", true, (int)eProfileType.Week, null));
				}
				else
				{
					_selectablePeriodHeaders.Add(new RowColProfileHeader("Show Weeks", false, (int)eProfileType.Week, null));
				}

				CreatePeriodHash();
				//End Track #5121 - JScott - Add Year/Season/Quarter totals
			}
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SelectRadioBtn()
        {
            try
            {
                if (_openParms.GroupBy == eStorePlanSelectedGroupBy.ByTimePeriod)
                {
                    this.optGroupByTime.Checked = true;
                }
                else
                {
                    this.optGroupByVariable.Checked = true;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        #endregion

        #region Format Grids (From Form_Load)

        private void SetGridRedraws(bool aValue)
        {
            try
            {
                _currentRedrawState = aValue;

                g2.Redraw = aValue;
                g3.Redraw = aValue;
                g4.Redraw = aValue;
                g5.Redraw = aValue;
                g6.Redraw = aValue;
                g7.Redraw = aValue;
                g8.Redraw = aValue;
                g9.Redraw = aValue;
                g10.Redraw = aValue;
                g11.Redraw = aValue;
                g12.Redraw = aValue;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
		//private void Formatg2Grid(bool aClearGrid, int aVariableSortKey, SortEnum aSortDirection)
		private void Formatg2Grid(bool aClearGrid, CubeWaferCoordinateList aSortCoordinates, SortEnum aSortDirection)
		//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
		{
            int colsPerGroup;
            int maxTimeTotVars;
            ArrayList timeTotVars;
            int i;
            CubeWaferCoordinateList cubeWaferCoordinateList;
            RowColProfileHeader varHeader;
            string timeName;
			VariableProfile varProf;

            try
            {
                if (aClearGrid)
                {
                    g2.Clear();
                }

                if (g2.Tag == null)
                {
                    g2.Tag = new PagingGridTag(Grid2, g2, null, g2, null, 0, 0);
                }

                if (_openParms.DateRangeProfile.Name == string.Empty)
                {
                    timeName = "Total";
                }
                else
                {
                    timeName = _openParms.DateRangeProfile.Name;
                }

                if (_openParms.GetSummaryDateProfile(_sab.ClientServerSession) != null)
                {
					//Begin Track #5006 - JScott - Display Low-levels one at a time
					//if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                    if (_currentPlanSessionType == ePlanSessionType.ChainSingleLevel)
					//End Track #5006 - JScott - Display Low-levels one at a time
                    {
						maxTimeTotVars = 0;

						foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
						{
							varHeader = (RowColProfileHeader)varEntry.Value;
							varProf = (VariableProfile)varHeader.Profile;

							maxTimeTotVars = Math.Max(maxTimeTotVars, varProf.TimeTotalChainVariables.Count);
						}

						colsPerGroup = 1;

                        ((PagingGridTag)g2.Tag).GroupsPerGrid = 1;

                        g2.Rows.Count = 2;
						g2.Cols.Count = ((PagingGridTag)g2.Tag).GroupsPerGrid * colsPerGroup * maxTimeTotVars;
						g2.Rows.Fixed = g2.Rows.Count;
                        g2.Cols.Fixed = 0;

                        g2.AllowDragging = AllowDraggingEnum.None;
                        g2.AllowMerging = AllowMergingEnum.RestrictCols;
						//Begin Track #6260 - JScott - Multi level>select cls 35018>results in Err
						((PagingGridTag)g2.Tag).SortImageRow = -1;
						//End Track #6260 - JScott - Multi level>select cls 35018>results in Err

						for (i = 0; i < maxTimeTotVars; i++)
						{
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(_openParms.GetSummaryDateProfile(_sab.ClientServerSession).ProfileType, _openParms.GetSummaryDateProfile(_sab.ClientServerSession).Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainTimeTotalIndex, i + 1));
                            g2.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, null, null, i, string.Empty);

                            g2.SetData(0, i, " ");
                            g2.SetData(1, i, timeName + " ");
                        }

                        i--;
                    }
                    else
                    {
						//Begin Track #5006 - JScott - Display Low-levels one at a time
						//switch (_openParms.PlanSessionType)
                        switch (_currentPlanSessionType)
						//End Track #5006 - JScott - Display Low-levels one at a time
                        {
                            case ePlanSessionType.ChainMultiLevel:
								maxTimeTotVars = _transaction.PlanComputations.PlanVariables.MaxChainTimeTotalVariables;
                                break;

                            default:
								maxTimeTotVars = _transaction.PlanComputations.PlanVariables.MaxStoreTimeTotalVariables;
                                break;
                        }


                        if (_columnGroupedBy == GroupedBy.GroupedByTime)
                        {
							colsPerGroup = _sortedVariableHeaders.Count * maxTimeTotVars;

                            ((PagingGridTag)g2.Tag).GroupsPerGrid = 1;

                            g2.Rows.Count = FIXEDCOLHEADERS;
							g2.Cols.Count = ((PagingGridTag)g2.Tag).GroupsPerGrid * colsPerGroup;
							g2.Rows.Fixed = g2.Rows.Count;
                            g2.Cols.Fixed = 0;

                            g2.AllowDragging = AllowDraggingEnum.None;
                            g2.AllowMerging = AllowMergingEnum.RestrictCols;
							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							((PagingGridTag)g2.Tag).SortImageRow = 2;
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

                            i = -1;

                            foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                            {
                                varHeader = (RowColProfileHeader)varEntry.Value;

								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//switch (_openParms.PlanSessionType)
								switch (_currentPlanSessionType)
								//End Track #5006 - JScott - Display Low-levels one at a time
                                {
                                    case ePlanSessionType.ChainMultiLevel:
                                        timeTotVars = ((VariableProfile)varHeader.Profile).TimeTotalChainVariables;
                                        break;

                                    default:
                                        timeTotVars = ((VariableProfile)varHeader.Profile).TimeTotalStoreVariables;
                                        break;
                                }

                                foreach (TimeTotalVariableProfile timeTotVarProf in timeTotVars)
                                {
                                    i++;
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(_openParms.GetSummaryDateProfile(_sab.ClientServerSession).ProfileType, _openParms.GetSummaryDateProfile(_sab.ClientServerSession).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.TimeTotalVariable, timeTotVarProf.Key));
                                    g2.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, null, varHeader, i, timeTotVarProf.VariableName);

									//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
									//if (varHeader.Profile.Key == aVariableSortKey)
									if (aSortCoordinates != null && aSortCoordinates.Equals(cubeWaferCoordinateList))
									//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
									{
                                        ((ColumnHeaderTag)g2.Cols[i].UserData).Sort = aSortDirection;
                                        if (aSortDirection == SortEnum.asc)
                                        {
                                            g2.SetCellImage(2, i, _upArrow);
                                        }
                                        else if (aSortDirection == SortEnum.desc)
                                        {
                                            g2.SetCellImage(2, i, _downArrow);
                                        }
                                    }

                                    g2.SetData(0, i, " ");
                                    g2.SetData(1, i, timeName + " ");
                                    g2.SetData(2, i, timeTotVarProf.VariableName);
                                }
                            }
                        }
                        else
                        {
                            colsPerGroup = 1;

                            ((PagingGridTag)g2.Tag).GroupsPerGrid = _sortedVariableHeaders.Count;

                            g2.Rows.Count = FIXEDCOLHEADERS;
                            g2.Cols.Count = ((PagingGridTag)g2.Tag).GroupsPerGrid * colsPerGroup * maxTimeTotVars;
                            g2.Rows.Fixed = g2.Rows.Count;
                            g2.Cols.Fixed = 0;

                            g2.AllowDragging = AllowDraggingEnum.None;
                            g2.AllowMerging = AllowMergingEnum.RestrictCols;
							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							((PagingGridTag)g2.Tag).SortImageRow = 1;
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

                            i = -1;
                            foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                            {
                                varHeader = (RowColProfileHeader)varEntry.Value;

								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//switch (_openParms.PlanSessionType)
								switch (_currentPlanSessionType)
								//End Track #5006 - JScott - Display Low-levels one at a time
								{
                                    case ePlanSessionType.ChainMultiLevel:
                                        timeTotVars = ((VariableProfile)varHeader.Profile).TimeTotalChainVariables;
                                        break;

                                    default:
                                        timeTotVars = ((VariableProfile)varHeader.Profile).TimeTotalStoreVariables;
                                        break;
                                }

                                foreach (TimeTotalVariableProfile timeTotVarProf in timeTotVars)
                                {
                                    i++;
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(_openParms.GetSummaryDateProfile(_sab.ClientServerSession).ProfileType, _openParms.GetSummaryDateProfile(_sab.ClientServerSession).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.TimeTotalVariable, timeTotVarProf.Key));
                                    g2.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, varHeader, null, i, timeTotVarProf.VariableName);

									//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
									//if (varHeader.Profile.Key == aVariableSortKey)
									if (aSortCoordinates != null && aSortCoordinates.Equals(cubeWaferCoordinateList))
									//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                                    {
                                        ((ColumnHeaderTag)g2.Cols[i].UserData).Sort = aSortDirection;
                                        if (aSortDirection == SortEnum.asc)
                                        {
											//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
											//g2.SetCellImage(2, i, _upArrow);
											g2.SetCellImage(1, i, _upArrow);
											//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
										}
                                        else if (aSortDirection == SortEnum.desc)
                                        {
											//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
											//g2.SetCellImage(2, i, _downArrow);
											g2.SetCellImage(1, i, _downArrow);
											//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
										}
                                    }

                                    g2.SetData(0, i, " ");
                                    g2.SetData(1, i, timeTotVarProf.VariableName);
                                    g2.SetData(2, i, timeName + " ");
                                }
                            }
                        }
                    }

                    if (i >= 0)
                    {
                        g2.Cols.Count = i + 1;
                        ((PagingGridTag)g2.Tag).Visible = true;
                        ((PagingGridTag)g2.Tag).DetailsPerGroup = colsPerGroup;
                    }
                    else
                    {
                        g2.Cols.Count = 0;
                        g2.Rows.Count = 0;
                        ((PagingGridTag)g2.Tag).Visible = false;
                        ((PagingGridTag)g2.Tag).DetailsPerGroup = 0;
                    }
                }
                else
                {
                    g2.Cols.Count = 0;
                    g2.Rows.Count = 0;
                    ((PagingGridTag)g2.Tag).Visible = false;
                    ((PagingGridTag)g2.Tag).DetailsPerGroup = 0;
                }

                ((PagingGridTag)g2.Tag).UnitsPerScroll = 1;

                foreach (Row row in g2.Rows)
                {
                    row.AllowMerging = true;
                }

                _gridData[Grid2] = new CellTag[g2.Rows.Count, g2.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
		//private void Formatg3Grid(bool aClearGrid, int aVariableSortKey, int aTimeSortKey, SortEnum aSortDirection)
		private void Formatg3Grid(bool aClearGrid, CubeWaferCoordinateList aSortCoordinates, SortEnum aSortDirection)
		//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
        {
            int colsPerGroup;
            int i;
            string headerDesc;
            CubeWaferCoordinateList cubeWaferCoordinateList;
            RowColProfileHeader varHeader;
            RowColProfileHeader timeHeader = null;

            try
            {
                if (aClearGrid)
                {
                    g3.Clear();
                }

                if (g3.Tag == null)
                {
                    g3.Tag = new PagingGridTag(Grid3, g3, null, g3, null, 0, 0);
                }

                g3.AllowDragging = AllowDraggingEnum.None;
                g3.AllowMerging = AllowMergingEnum.RestrictCols;

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//switch (_openParms.PlanSessionType)
				switch (_currentPlanSessionType)
				//End Track #5006 - JScott - Display Low-levels one at a time
				{
                    case ePlanSessionType.StoreSingleLevel:
						//Begin Track #5006 - JScott - Display Low-levels one at a time
						//headerDesc = "Store" + ((_storeReadOnly) ? " (Read Only)" : "") + ": " + _openParms.StoreHLPlanProfile.NodeProfile.Text + " / " + _openParms.StoreHLPlanProfile.VersionProfile.Description + " / " + _openParms.DateRangeProfile.DisplayDate +
							//" --- Chain" + ((_chainReadOnly) ? " (Read Only)" : "") + ": " + _openParms.ChainHLPlanProfile.NodeProfile.Text + " / " + _openParms.ChainHLPlanProfile.VersionProfile.Description;
						headerDesc = "Store" + ((_storeReadOnly) ? " (Read Only)" : "") + ": " + _currentStorePlanProfile.NodeProfile.Text + " / " + _currentStorePlanProfile.VersionProfile.Description + " / " + _openParms.DateRangeProfile.DisplayDate +
							" --- Chain" + ((_chainReadOnly) ? " (Read Only)" : "") + ": " + _currentChainPlanProfile.NodeProfile.Text + " / " + _currentChainPlanProfile.VersionProfile.Description;
						//End Track #5006 - JScott - Display Low-levels one at a time
                        break;

                    case ePlanSessionType.StoreMultiLevel:
                        if (_openParms.LowLevelsType == eLowLevelsType.HierarchyLevel)
                        {
                            headerDesc = "Store" + ((_storeReadOnly) ? " (Read Only)" : "") + ": " + _openParms.StoreHLPlanProfile.NodeProfile.Text + " / " + _openParms.StoreHLPlanProfile.VersionProfile.Description + " / " + _openParms.DateRangeProfile.DisplayDate +
                                " --- Chain" + ((_chainReadOnly) ? " (Read Only)" : "") + ": " + _openParms.ChainHLPlanProfile.NodeProfile.Text + " / " + _openParms.ChainHLPlanProfile.VersionProfile.Description +
                                " --- Low-Level" + ((_lowLevelChainReadOnly || _lowLevelStoreReadOnly) ? " (Read Only)" : "") + ": " + ((HierarchyLevelProfile)_sab.HierarchyServerSession.GetMainHierarchyData().HierarchyLevels[_openParms.LowLevelsSequence]).LevelID;
                        }
                        else
                        {
                            headerDesc = "Store" + ((_storeReadOnly) ? " (Read Only)" : "") + ": " + _openParms.StoreHLPlanProfile.NodeProfile.Text + " / " + _openParms.StoreHLPlanProfile.VersionProfile.Description + " / " + _openParms.DateRangeProfile.DisplayDate +
                                " --- Chain" + ((_chainReadOnly) ? " (Read Only)" : "") + ": " + _openParms.ChainHLPlanProfile.NodeProfile.Text + " / " + _openParms.ChainHLPlanProfile.VersionProfile.Description +
                                " --- Low-Level" + ((_lowLevelChainReadOnly || _lowLevelStoreReadOnly) ? " (Read Only)" : "") + ": +" + _openParms.LowLevelsOffset.ToString();
                        }
                        break;

                    case ePlanSessionType.ChainSingleLevel:
						//Begin Track #5006 - JScott - Display Low-levels one at a time
						//headerDesc = "Chain" + ((_chainReadOnly) ? " (Read Only)" : "") + ": " + _openParms.ChainHLPlanProfile.NodeProfile.Text + " / " + _openParms.ChainHLPlanProfile.VersionProfile.Description;
						headerDesc = "Chain" + ((_chainReadOnly) ? " (Read Only)" : "") + ": " + _currentChainPlanProfile.NodeProfile.Text + " / " + _currentChainPlanProfile.VersionProfile.Description;
						//Begin Track #5006 - JScott - Display Low-levels one at a time
						break;

                    case ePlanSessionType.ChainMultiLevel:
                        if (_openParms.LowLevelsType == eLowLevelsType.HierarchyLevel)
                        {
                            headerDesc = "Chain" + ((_chainReadOnly) ? " (Read Only)" : "") + ": " + _openParms.ChainHLPlanProfile.NodeProfile.Text + " / " + _openParms.ChainHLPlanProfile.VersionProfile.Description + " / " + _openParms.DateRangeProfile.DisplayDate +
                                " --- Low-Level" + ((_lowLevelChainReadOnly) ? " (Read Only)" : "") + ": " + ((HierarchyLevelProfile)_sab.HierarchyServerSession.GetMainHierarchyData().HierarchyLevels[_openParms.LowLevelsSequence]).LevelID;
                        }
                        else
                        {
                            headerDesc = "Chain" + ((_chainReadOnly) ? " (Read Only)" : "") + ": " + _openParms.ChainHLPlanProfile.NodeProfile.Text + " / " + _openParms.ChainHLPlanProfile.VersionProfile.Description + " / " + _openParms.DateRangeProfile.DisplayDate +
                                " --- Low-Level" + ((_lowLevelChainReadOnly) ? " (Read Only)" : "") + ": +" + _openParms.LowLevelsOffset.ToString();
                        }
                        break;

                    default:
                        headerDesc = "Unassigned";
                        break;
                }

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                if (_currentPlanSessionType == ePlanSessionType.ChainSingleLevel)
				//End Track #5006 - JScott - Display Low-levels one at a time
                {
                    colsPerGroup = 1;

                    ((PagingGridTag)g3.Tag).GroupsPerGrid = _sortedTimeHeaders.Count;

                    g3.Rows.Count = 2;
                    g3.Cols.Count = ((PagingGridTag)g3.Tag).GroupsPerGrid * colsPerGroup;
                    g3.Rows.Fixed = g3.Rows.Count;
                    g3.Cols.Fixed = 0;
					//Begin Track #6260 - JScott - Multi level>select cls 35018>results in Err
					((PagingGridTag)g3.Tag).SortImageRow = -1;
					//End Track #6260 - JScott - Multi level>select cls 35018>results in Err

                    i = -1;

                    foreach (DictionaryEntry timeEntry in _sortedTimeHeaders)
                    {
                        timeHeader = (RowColProfileHeader)timeEntry.Value;

                        i++;
                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(timeHeader.Profile.ProfileType, timeHeader.Profile.Key));

                        g3.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, timeHeader, null, i, timeHeader.Name);

						//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
						//if (timeHeader.Profile.Key == aTimeSortKey)
						if (aSortCoordinates != null && aSortCoordinates.Equals(cubeWaferCoordinateList))
						//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
						{
                            ((ColumnHeaderTag)g3.Cols[i].UserData).Sort = aSortDirection;
                            if (aSortDirection == SortEnum.asc)
                            {
                                g3.SetCellImage(2, i, _upArrow);
                            }
                            else if (aSortDirection == SortEnum.desc)
                            {
                                g3.SetCellImage(2, i, _downArrow);
                            }
                        }

                        g3.SetData(0, i, headerDesc);
                        g3.SetData(1, i, timeHeader.Name);
                    }
                }
                else if (_columnGroupedBy == GroupedBy.GroupedByTime)
                {
                    colsPerGroup = _sortedVariableHeaders.Count;

                    ((PagingGridTag)g3.Tag).GroupsPerGrid = _sortedTimeHeaders.Count;

                    g3.Rows.Count = FIXEDCOLHEADERS;
                    g3.Cols.Count = ((PagingGridTag)g3.Tag).GroupsPerGrid * colsPerGroup;
                    g3.Rows.Fixed = g3.Rows.Count;
                    g3.Cols.Fixed = 0;
					//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
					((PagingGridTag)g3.Tag).SortImageRow = 2;
					//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

                    i = -1;

                    foreach (DictionaryEntry timeEntry in _sortedTimeHeaders)
                    {
                        timeHeader = (RowColProfileHeader)timeEntry.Value;

                        foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                        {
                            varHeader = (RowColProfileHeader)varEntry.Value;

                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(timeHeader.Profile.ProfileType, timeHeader.Profile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varHeader.Profile.Key));

                            g3.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, timeHeader, varHeader, i, timeHeader.Name + "|" + varHeader.Name);

							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							//if (varHeader.Profile.Key == aVariableSortKey &&
							//    timeHeader.Profile.Key == aTimeSortKey)
							if (aSortCoordinates != null && aSortCoordinates.Equals(cubeWaferCoordinateList))
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                            {
                                ((ColumnHeaderTag)g3.Cols[i].UserData).Sort = aSortDirection;
                                if (aSortDirection == SortEnum.asc)
                                {
                                    g3.SetCellImage(2, i, _upArrow);
                                }
                                else if (aSortDirection == SortEnum.desc)
                                {
                                    g3.SetCellImage(2, i, _downArrow);
                                }
                            }

                            g3.SetData(0, i, headerDesc);
                            g3.SetData(1, i, timeHeader.Name);
                            g3.SetData(2, i, varHeader.Name);
                        }
                    }
                }
                else
                {
                    colsPerGroup = _sortedTimeHeaders.Count;

                    ((PagingGridTag)g3.Tag).GroupsPerGrid = _sortedVariableHeaders.Count;

                    g3.Rows.Count = FIXEDCOLHEADERS;
                    g3.Cols.Count = ((PagingGridTag)g3.Tag).GroupsPerGrid * colsPerGroup;
                    g3.Rows.Fixed = g3.Rows.Count;
                    g3.Cols.Fixed = 0;
					//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
					((PagingGridTag)g3.Tag).SortImageRow = 2;
					//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

                    i = -1;

                    foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                    {
                        varHeader = (RowColProfileHeader)varEntry.Value;

                        foreach (DictionaryEntry timeEntry in _sortedTimeHeaders)
                        {
                            timeHeader = (RowColProfileHeader)timeEntry.Value;

                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(timeHeader.Profile.ProfileType, timeHeader.Profile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varHeader.Profile.Key));

                            g3.Cols[i].UserData = new ColumnHeaderTag(cubeWaferCoordinateList, varHeader, timeHeader, i, varHeader.Name + "|" + timeHeader.Name);

							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							//if (timeHeader.Profile.Key == aTimeSortKey &&
							//    varHeader.Profile.Key == aVariableSortKey)
							if (aSortCoordinates != null && aSortCoordinates.Equals(cubeWaferCoordinateList))
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                            {
                                ((ColumnHeaderTag)g3.Cols[i].UserData).Sort = aSortDirection;
                                if (aSortDirection == SortEnum.asc)
                                {
                                    g3.SetCellImage(2, i, _upArrow);
                                }
                                else if (aSortDirection == SortEnum.desc)
                                {
                                    g3.SetCellImage(2, i, _downArrow);
                                }
                            }

                            g3.SetData(0, i, headerDesc);
                            g3.SetData(1, i, varHeader.Name);
                            g3.SetData(2, i, timeHeader.Name);
                        }
                    }
                }

                ((PagingGridTag)g3.Tag).Visible = true;
                ((PagingGridTag)g3.Tag).DetailsPerGroup = colsPerGroup;
                ((PagingGridTag)g3.Tag).UnitsPerScroll = 1;

                foreach (Row row in g3.Rows)
                {
                    row.AllowMerging = true;
                }

                _gridData[Grid3] = new CellTag[g3.Rows.Count, g3.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		private void Formatg4Grid(bool aClearGrid, C1FlexGrid aGrid, ProfileList aWorkingDetailProfileList,
			bool aViewableGrid)
        {
            CubeWaferCoordinateList cubeWaferCoordinateList;
            ArrayList compList;
            GridRowList gridRowList;
            RowColProfileHeader groupHeader;
            RowColProfileHeader varHeader;
            VariableProfile varProf;

            try
            {
                if (aClearGrid)
                {
                    aGrid.Clear();
                }

                if (aGrid.Tag == null)
                {
                    aGrid.Tag = new PagingGridTag(Grid4, aGrid, aGrid, null, null, 0, 0);
                }

                compList = new ArrayList();
                gridRowList = new GridRowList();

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//switch (_openParms.PlanSessionType)
                switch (_currentPlanSessionType)
				//End Track #5006 - JScott - Display Low-levels one at a time
                {
                    case ePlanSessionType.ChainSingleLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube && ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)aGrid.Tag).GroupsPerGrid = _sortedVariableHeaders.Count;

                        foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
                        {
                            varHeader = (RowColProfileHeader)varEntry.Value;
                            varProf = (VariableProfile)varHeader.Profile;

                            groupHeader = new RowColProfileHeader(varProf.VariableName, false, 0, varProf);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, new RowColProfileHeader("Original Plan", false, 2, null), gridRowList.Count, " ", varProf.VariableName, false), true);

                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, varProf.VariableName, varProf.VariableName));

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", varProf.VariableName + "|ADJ"));
                            }
                            else
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, varProf.VariableName, varProf.VariableName));
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
									{
									//End Track #6010 - JScott - Bad % Change on Basis2
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
										//Begin Track #5006 - JScott - Display Low-levels one at a time
										//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
										//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
										//End Track #5006 - JScott - Display Low-levels one at a time
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + detailHeader.Name));
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									}
									//End Track #6010 - JScott - Bad % Change on Basis2
                                }
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    //Begin Track #5648 - JScott - Export Option from OTS Forecast Review Scrren
                                    //groupHeader = new RowColProfileHeader("Chain " + basisHeader.Name, true, 0, null);
                                    groupHeader = new RowColProfileHeader("Chain " + basisHeader.Name, true, 0, varProf);
                                    //End Track #5648 - JScott - Export Option from OTS Forecast Review Scrren

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
									//Begin Track #5006 - JScott - Display Low-levels one at a time
									//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
									//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
									cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
									cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
									//End Track #5006 - JScott - Display Low-levels one at a time
									cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
									//Begin Track #5006 - JScott - Display Low-levels one at a time
									////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
									////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, varProf.VariableName + "|" + ((BasisProfile)basisHeader.Profile).Name));
									//gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key), varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key)));
									////End Track #5782
									gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key), varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key)));
									//End Track #5006 - JScott - Display Low-levels one at a time

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
												//Begin Track #5006 - JScott - Display Low-levels one at a time
												//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
												//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
												//End Track #5006 - JScott - Display Low-levels one at a time
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
												//Begin Track #5006 - JScott - Display Low-levels one at a time
												////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
												////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
												//gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.VersionProfile.Key) + "|" + detailHeader.Name));
												////End Track #5782
												gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.VersionProfile.Key) + "|" + detailHeader.Name));
												//End Track #5006 - JScott - Display Low-levels one at a time
											}
                                        }
                                    }
                                }
                            }
                        }

                        break;

                    case ePlanSessionType.StoreSingleLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreDetailCube && ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

						((PagingGridTag)aGrid.Tag).GroupsPerGrid = aWorkingDetailProfileList.Count;

						foreach (StoreProfile storeProfile in aWorkingDetailProfileList)
                        {
                            groupHeader = new RowColProfileHeader("Store", true, 0, null);

                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", storeProfile.Text + "|ADJ"));
                            }
                            else
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
									{
									//End Track #6010 - JScott - Bad % Change on Basis2
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
										//Begin Track #5006 - JScott - Display Low-levels one at a time
										//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
										//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
										//End Track #5006 - JScott - Display Low-levels one at a time
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + detailHeader.Name));
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									}
									//End Track #6010 - JScott - Bad % Change on Basis2
								}
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    groupHeader = new RowColProfileHeader("Store " + basisHeader.Name, true, 0, null);

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
									//Begin Track #5006 - JScott - Display Low-levels one at a time
									//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
									//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
									cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
									cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
									//End Track #5006 - JScott - Display Low-levels one at a time
									cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
									//Begin Track #5006 - JScott - Display Low-levels one at a time
									////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
									////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
									//gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key), storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key)));
									////End Track #5782
									gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key), storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key)));
									//End Track #5006 - JScott - Display Low-levels one at a time

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
												//Begin Track #5006 - JScott - Display Low-levels one at a time
												//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
												//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
												//End Track #5006 - JScott - Display Low-levels one at a time
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
												//Begin Track #5006 - JScott - Display Low-levels one at a time
												////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
												////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
												//gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
												////End Track #5782
												gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
												//End Track #5006 - JScott - Display Low-levels one at a time
											}
                                        }
                                    }
                                }
                            }
                        }

                        break;

                    case ePlanSessionType.ChainMultiLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                        ((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

						((PagingGridTag)aGrid.Tag).GroupsPerGrid = aWorkingDetailProfileList.Count;

						foreach (PlanProfile planProf in aWorkingDetailProfileList)
                        {
                            groupHeader = new RowColProfileHeader("Low Level", true, 0, null);

                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, planProf.NodeProfile.Text, planProf.NodeProfile.Text), true);

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", planProf.NodeProfile.Text + "|ADJ"));
                            }
                            else
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, planProf.NodeProfile.Text), true);
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
									{
									//End Track #6010 - JScott - Bad % Change on Basis2
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, planProf.NodeProfile.Text + "|" + detailHeader.Name));
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									}
									//End Track #6010 - JScott - Bad % Change on Basis2
								}
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    groupHeader = new RowColProfileHeader("Low Level " + basisHeader.Name, true, 0, null);

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key)));
                                    //End Track #5782

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5782
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;

                    case ePlanSessionType.StoreMultiLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreDetailCube)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

						((PagingGridTag)aGrid.Tag).GroupsPerGrid = aWorkingDetailProfileList.Count;

						foreach (StoreProfile storeProfile in aWorkingDetailProfileList)
                        {
                            //=================
                            // Store High Level
                            //=================

                            groupHeader = new RowColProfileHeader("Store", true, 0, null);

                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", storeProfile.Text + "|ADJ"));
                            }
                            else
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, storeProfile.Text, storeProfile.Text), true);
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
									{
									//End Track #6010 - JScott - Bad % Change on Basis2
										if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
										{
											cubeWaferCoordinateList = new CubeWaferCoordinateList();
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
											gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + detailHeader.Name));
										}
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									}
									//End Track #6010 - JScott - Bad % Change on Basis2
								}
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    groupHeader = new RowColProfileHeader("Store " + basisHeader.Name, true, 0, null);

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key), storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key)));
                                    //End Track #5782

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                                {
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                    //End Track #5782
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //======================
                            // Store Low Level Total
                            //======================

                            groupHeader = new RowColProfileHeader("Low Level Total", true, 0, null);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, "Low Level Total", storeProfile.Text + "|Low Level Total"));

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
									{
									//End Track #6010 - JScott - Bad % Change on Basis2
										if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
										{
											cubeWaferCoordinateList = new CubeWaferCoordinateList();
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
											gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|Low Level Total|" + detailHeader.Name));
										}
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									}
									//End Track #6010 - JScott - Bad % Change on Basis2
								}
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    groupHeader = new RowColProfileHeader("Low Level Total " + basisHeader.Name, true, 0, null);

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                                {
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //==============
                            // Store Balance
                            //==============

                            groupHeader = new RowColProfileHeader("Store Balance", true, 0, null);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, "Balance", storeProfile.Text + "|Balance"));

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    groupHeader = new RowColProfileHeader("Store Balance " + basisHeader.Name, true, 0, null);

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|Balance|" + ((BasisProfile)basisHeader.Profile).Name));
                                }
                            }

                            foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
                            {
                                //================
                                // Store Low Level
                                //================

                                groupHeader = new RowColProfileHeader(planProf.NodeProfile.Text, true, 0, null);

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, storeProfile.Text + "|" + planProf.NodeProfile.Text));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
										//Begin Track #6010 - JScott - Bad % Change on Basis2
										if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
										{
										//End Track #6010 - JScott - Bad % Change on Basis2
											if (((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
											{
												cubeWaferCoordinateList = new CubeWaferCoordinateList();
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
												gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
											}
										//Begin Track #6010 - JScott - Bad % Change on Basis2
										}
										//End Track #6010 - JScott - Bad % Change on Basis2
									}
                                }

                                foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                                {
                                    if (basisHeader.IsDisplayed)
                                    {
                                        groupHeader = new RowColProfileHeader(planProf.NodeProfile.Text + basisHeader.Name, true, 0, null);

                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                        //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                        //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key)));
                                        //End Track #5782

                                        foreach (RowColProfileHeader detailHeader in compList)
                                        {
                                            if (detailHeader.IsDisplayed)
                                            {
                                                if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                    detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                                {
                                                    if (((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                                    {
                                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Store, storeProfile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                        //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                        //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, storeProfile.Text + "|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + detailHeader.Name));
                                                        //End Track #5782
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                }

                aGrid.Cols.Count = FIXEDROWHEADERS;
                aGrid.Cols.Fixed = FIXEDROWHEADERS;

                gridRowList.BuildGridRows(aGrid, 0);

				if (aViewableGrid)
				{
					((PagingGridTag)aGrid.Tag).Visible = true;
				}
                ((PagingGridTag)aGrid.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                ((PagingGridTag)aGrid.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;

				if (aViewableGrid)
				{
					_gridData[Grid4] = new CellTag[aGrid.Rows.Count, aGrid.Cols.Count];
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private string LoadBasisLabel()
        {
            try
            {
                //---Load Basis Label-----------
                string tmpBasisLabel = "";
                string concat = "";

                MIDRetail.Data.GlobalOptions opts = new MIDRetail.Data.GlobalOptions();
                DataTable dt = opts.GetGlobalOptions();
                DataRow dr = dt.Rows[0];

                _productDisplayCombination = Convert.ToInt32(dr["PRODUCT_LEVEL_DISPLAY_ID"], CultureInfo.CurrentUICulture);
                _storeDisplayCombination = Convert.ToInt32(dr["STORE_DISPLAY_OPTION_ID"], CultureInfo.CurrentUICulture);

                BasisLabelTypeProfile viewVarProf;
                ProfileList varProfList = GetBasisLabelProfList(Convert.ToInt32(dr["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture));
                dt = opts.GetBasisLabelInfo(Convert.ToInt32(dr["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture));
                foreach (DataRow releaseRow in dt.Rows)
                {
                    int basisLabelType = Convert.ToInt32(releaseRow["LABEL_TYPE"], CultureInfo.CurrentUICulture);
                    BasisLabelTypeProfile bltp = new BasisLabelTypeProfile(basisLabelType);
                    viewVarProf = (BasisLabelTypeProfile)varProfList.FindKey(basisLabelType);
                    bltp.BasisLabelSystemOptionRID = Convert.ToInt32(releaseRow["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture);
                    bltp.BasisLabelName = Convert.ToString(viewVarProf.BasisLabelName);
                    bltp.BasisLabelType = basisLabelType;
                    bltp.BasisLabelSequence = Convert.ToInt32(releaseRow["LABEL_SEQ"], CultureInfo.CurrentUICulture);
                    tmpBasisLabel = tmpBasisLabel + concat + bltp.BasisLabelName;
                    concat = " / ";
                }
                return tmpBasisLabel;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }

        }


        private ProfileList GetBasisLabelProfList(int systemOptionRID)
        {
            ProfileList basisLabelList = new ProfileList(eProfileType.BasisLabelType);

            Array values;
            string[] names;

            values = System.Enum.GetValues(typeof(eBasisLabelType));
            names = System.Enum.GetNames(typeof(eBasisLabelType));

            for (int i = 0; i < names.Length; i++)
            {
                BasisLabelTypeProfile bltp = new BasisLabelTypeProfile(i);
                bltp.BasisLabelSystemOptionRID = systemOptionRID;
                bltp.BasisLabelName = names[i];
                bltp.BasisLabelType = i;
                bltp.BasisLabelSequence = -1;

                basisLabelList.Add(bltp);
            }
            return basisLabelList;
        }

        private string GetBasisLabel(int basisHeaderKey, int hierarchyNodeKey)
        {
            HashKeyObject activeKey = new HashKeyObject(hierarchyNodeKey, basisHeaderKey);
            return (string)_basisLabelList[activeKey];
        }

		private void Formatg5Grid(bool aClearGrid)
        {
            int i;
            int j;
            int rowsPerGroup;
            VariableProfile varProf;

            try
            {
                if (aClearGrid)
                {
                    g5.Clear();
                }

                if (g5.Tag == null)
                {
                    g5.Tag = new PagingGridTag(Grid5, g5, g4, g2, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                if (_openParms.GetSummaryDateProfile(_sab.ClientServerSession) != null && ((PagingGridTag)g4.Tag).Visible)
                {
                    g5.Rows.Count = g4.Rows.Count;
                    g5.Cols.Count = g2.Cols.Count;
                    g5.Rows.Fixed = 0;
                    g5.Cols.Fixed = 0;

                    foreach (Row row in g5.Rows)
                    {
                        row.UserData = new RowTag();
                    }

                    foreach (Column col in g5.Cols)
                    {
                        col.UserData = new ColumnTag();
                    }

                    ((PagingGridTag)g5.Tag).Visible = true;

					//Begin Track #5006 - JScott - Display Low-levels one at a time
					//if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                    if (_currentPlanSessionType == ePlanSessionType.ChainSingleLevel)
					//End Track #5006 - JScott - Display Low-levels one at a time
                    {
                        if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
                        {
                            rowsPerGroup = ((PagingGridTag)g4.Tag).DetailsPerGroup;

                            for (i = 0; i < g5.Rows.Count; i += rowsPerGroup)
                            {
                                varProf = (VariableProfile)((RowColProfileHeader)((RowHeaderTag)g4.Rows[i].UserData).GroupRowColHeader).Profile;

                                for (j = 0; j < varProf.TimeTotalChainVariables.Count; j++)
                                {
                                    g5.SetCellStyle(i, j, (CellStyle)null);
                                    g5.SetData(i, j, ((TimeTotalVariableProfile)varProf.GetChainTimeTotalVariable(j + 1)).VariableName);
                                }
                            }
                        }
                    }
                }
                else
                {
                    g5.Cols.Count = 0;
                    g5.Rows.Count = 0;
                    ((PagingGridTag)g5.Tag).Visible = false;
                }

                _gridData[Grid5] = new CellTag[g5.Rows.Count, g5.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg6Grid(bool aClearGrid)
        {
            try
            {
                if (aClearGrid)
                {
                    g6.Clear();
                }

                if (g6.Tag == null)
                {
                    g6.Tag = new PagingGridTag(Grid6, g6, g4, g3, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                g6.Rows.Count = g4.Rows.Count;
                g6.Cols.Count = g3.Cols.Count;
                g6.Rows.Fixed = 0;
                g6.Cols.Fixed = 0;

                foreach (Row row in g6.Rows)
                {
                    row.UserData = new RowTag();
                }

                foreach (Column col in g6.Cols)
                {
                    col.UserData = new ColumnTag();
                }

                ((PagingGridTag)g6.Tag).Visible = true;

                _gridData[Grid6] = new CellTag[g6.Rows.Count, g6.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg7Grid(bool aClearGrid)
        {
            CubeWaferCoordinateList cubeWaferCoordinateList;
            ArrayList compList;
            GridRowList gridRowList;
            int initVisibleRows = 0;

            try
            {
                if (aClearGrid)
                {
                    g7.Clear();
                }

                if (g7.Tag == null)
                {
                    g7.Tag = new PagingGridTag(Grid7, g7, g7, null, null, 0, 0);
                }

                compList = new ArrayList();
                gridRowList = new GridRowList();

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//switch (_openParms.PlanSessionType)
                switch (_currentPlanSessionType)
				//End Track #5006 - JScott - Display Low-levels one at a time
                {
                    case ePlanSessionType.ChainSingleLevel:
                    case ePlanSessionType.ChainMultiLevel:

                        g7.Cols.Count = 0;
                        g7.Rows.Count = 0;

                        ((PagingGridTag)g7.Tag).Visible = false;
                        ((PagingGridTag)g7.Tag).DetailsPerGroup = 0;
                        ((PagingGridTag)g7.Tag).UnitsPerScroll = 0;
                        break;

                    case ePlanSessionType.StoreSingleLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreSetCube)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)g7.Tag).GroupsPerGrid = _storeGroupLevelProfileList.Count;

                        foreach (RowColProfileHeader groupHeader in _selectableStoreAttributeHeaders)
                        {
                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, groupHeader.Name, groupHeader.Name), true);

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", groupHeader.Name + "|ADJ"));
                            }
                            else
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, groupHeader.Name, groupHeader.Name), true);
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
									{
									//End Track #6010 - JScott - Bad % Change on Basis2
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
										//Begin Track #5006 - JScott - Display Low-levels one at a time
										//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
										//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
										//End Track #5006 - JScott - Display Low-levels one at a time
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + detailHeader.Name));
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									}
									//End Track #6010 - JScott - Bad % Change on Basis2
								}
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
									//Begin Track #5006 - JScott - Display Low-levels one at a time
									//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
									//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
									cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
									cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
									//End Track #5006 - JScott - Display Low-levels one at a time
									cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
									//Begin Track #5006 - JScott - Display Low-levels one at a time
									////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
									////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name));
									//gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key), groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key)));
									////End Track #5782
									gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key), groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key)));
									//End Track #5006 - JScott - Display Low-levels one at a time

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
												//Begin Track #5006 - JScott - Display Low-levels one at a time
												//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
												//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
												//End Track #5006 - JScott - Display Low-levels one at a time
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
												//Begin Track #5006 - JScott - Display Low-levels one at a time
												////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
												////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
												//gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
												////End Track #5782
												gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
												//End Track #5006 - JScott - Display Low-levels one at a time
											}
                                        }
                                    }
                                }
                            }
                        }

                        g7.Cols.Count = FIXEDROWHEADERS;
                        g7.Cols.Fixed = FIXEDROWHEADERS;

                        gridRowList.BuildGridRows(g7, 0);

                        ((PagingGridTag)g7.Tag).Visible = true;
                        ((PagingGridTag)g7.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g7.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;

                        break;

                    case ePlanSessionType.StoreMultiLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreSetCube)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)g7.Tag).GroupsPerGrid = _storeGroupLevelProfileList.Count;

                        foreach (RowColProfileHeader groupHeader in _selectableStoreAttributeHeaders)
                        {
                            //=================
                            // Store High Level
                            //=================

                            initVisibleRows = 0;

                            if (_adjustmentRowHeader.IsDisplayed)
                            {
                                initVisibleRows++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, gridRowList.Count, groupHeader.Name, groupHeader.Name), true);

                                initVisibleRows++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, gridRowList.Count, "ADJ", groupHeader.Name + "|ADJ"));
                            }
                            else
                            {
                                initVisibleRows++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, groupHeader.Name, groupHeader.Name), true);
                            }

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
									{
									//End Track #6010 - JScott - Bad % Change on Basis2
										if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
										{
											initVisibleRows++;
											cubeWaferCoordinateList = new CubeWaferCoordinateList();
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
											gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + detailHeader.Name));
										}
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									}
									//End Track #6010 - JScott - Bad % Change on Basis2
								}
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    initVisibleRows++;
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key), groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key)));
                                    //End Track #5782

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                                {
                                                    initVisibleRows++;
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                    //End Track #5782
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //======================
                            // Store Low Level Total
                            //======================

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, "Low Level Total", groupHeader.Name + "|Low Level Total"));

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
									{
									//End Track #6010 - JScott - Bad % Change on Basis2
										if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
										{
											cubeWaferCoordinateList = new CubeWaferCoordinateList();
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
											gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|Low Level Total|" + detailHeader.Name));
										}
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									}
									//End Track #6010 - JScott - Bad % Change on Basis2
								}
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, groupHeader.Name + "|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                                {
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //==============
                            // Store Balance
                            //==============

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, "Balance", groupHeader.Name + "|Balance"));

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, groupHeader.Name + "|Balance|" + ((BasisProfile)basisHeader.Profile).Name));
                                }
                            }

                            foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
                            {
                                //================
                                // Store Low Level
                                //================

                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, groupHeader.Name + "|" + planProf.NodeProfile.Text));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
										//Begin Track #6010 - JScott - Bad % Change on Basis2
										if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
										{
										//End Track #6010 - JScott - Bad % Change on Basis2
											if (((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
											{
												cubeWaferCoordinateList = new CubeWaferCoordinateList();
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
												gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
											}
										//Begin Track #6010 - JScott - Bad % Change on Basis2
										}
										//End Track #6010 - JScott - Bad % Change on Basis2
									}
                                }

                                foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                                {
                                    if (basisHeader.IsDisplayed)
                                    {
                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                        //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                        //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + planProf.NodeProfile.Text));
                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + planProf.NodeProfile.Text));
                                        //End Track #5782

                                        foreach (RowColProfileHeader detailHeader in compList)
                                        {
                                            if (detailHeader.IsDisplayed)
                                            {
                                                if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                    detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                                {
                                                    if (((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                                    {
                                                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreGroupLevel, groupHeader.Profile.Key));
                                                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                        //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                        //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
                                                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, groupHeader.Name + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
                                                        //End Track #5782
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        g7.Cols.Count = FIXEDROWHEADERS;
                        g7.Cols.Fixed = FIXEDROWHEADERS;

                        gridRowList.BuildGridRows(g7, 0);

                        ((PagingGridTag)g7.Tag).Visible = true;
                        ((PagingGridTag)g7.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g7.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g7.Tag).InitialVisibleRows = initVisibleRows;

                        break;
                }

                _gridData[Grid7] = new CellTag[g7.Rows.Count, g7.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg8Grid(bool aClearGrid)
        {
            try
            {
                if (aClearGrid)
                {
                    g8.Clear();
                }

                if (g8.Tag == null)
                {
                    g8.Tag = new PagingGridTag(Grid8, g8, g7, g2, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (_openParms.GetSummaryDateProfile(_sab.ClientServerSession) != null && ((PagingGridTag)g7.Tag).Visible && (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel || _openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel))
                if (_openParms.GetSummaryDateProfile(_sab.ClientServerSession) != null && ((PagingGridTag)g7.Tag).Visible && (_currentPlanSessionType == ePlanSessionType.StoreSingleLevel || _currentPlanSessionType == ePlanSessionType.StoreMultiLevel))
				//End Track #5006 - JScott - Display Low-levels one at a time
                {
                    g8.Rows.Count = g7.Rows.Count;
                    g8.Cols.Count = g2.Cols.Count;
                    g8.Rows.Fixed = 0;
                    g8.Cols.Fixed = 0;

                    foreach (Row row in g8.Rows)
                    {
                        row.UserData = new RowTag();
                    }

                    foreach (Column col in g8.Cols)
                    {
                        col.UserData = new ColumnTag();
                    }

                    ((PagingGridTag)g8.Tag).Visible = true;
                }
                else
                {
                    g8.Cols.Count = 0;
                    g8.Rows.Count = 0;
                    ((PagingGridTag)g8.Tag).Visible = false;
                }

                _gridData[Grid8] = new CellTag[g8.Rows.Count, g8.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg9Grid(bool aClearGrid)
        {
            try
            {
                if (aClearGrid)
                {
                    g9.Clear();
                }

                if (g9.Tag == null)
                {
                    g9.Tag = new PagingGridTag(Grid9, g9, g7, g3, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel || _openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
                if (_currentPlanSessionType == ePlanSessionType.StoreSingleLevel || _currentPlanSessionType == ePlanSessionType.StoreMultiLevel)
				//End Track #5006 - JScott - Display Low-levels one at a time
                {
                    g9.Rows.Count = g7.Rows.Count;
                    g9.Cols.Count = g3.Cols.Count;
                    g9.Rows.Fixed = 0;
                    g9.Cols.Fixed = 0;

                    foreach (Row row in g9.Rows)
                    {
                        row.UserData = new RowTag();
                    }

                    foreach (Column col in g9.Cols)
                    {
                        col.UserData = new ColumnTag();
                    }

                    ((PagingGridTag)g9.Tag).Visible = true;
                }
                else
                {
                    g9.Cols.Count = 0;
                    g9.Rows.Count = 0;
                    ((PagingGridTag)g9.Tag).Visible = false;
                }

                _gridData[Grid9] = new CellTag[g9.Rows.Count, g9.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg10Grid(bool aClearGrid)
        {
            int i;
            CubeWaferCoordinateList cubeWaferCoordinateList;
            ArrayList compList;
            GridRowList gridRowList;
            int initVisibleRows = 0;

            try
            {
                if (aClearGrid)
                {
                    g10.Clear();
                }

                if (g10.Tag == null)
                {
                    g10.Tag = new PagingGridTag(Grid10, g10, g10, null, null, 0, 0);
                }

                compList = new ArrayList();
                gridRowList = new GridRowList();

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//switch (_openParms.PlanSessionType)
				switch (_currentPlanSessionType)
				//End Track #5006 - JScott - Display Low-levels one at a time
				{
                    case ePlanSessionType.ChainSingleLevel:

                        g10.Cols.Count = 0;
                        g10.Rows.Count = 0;

                        ((PagingGridTag)g10.Tag).Visible = false;
                        ((PagingGridTag)g10.Tag).DetailsPerGroup = 0;
                        ((PagingGridTag)g10.Tag).UnitsPerScroll = 0;
                        break;

                    case ePlanSessionType.StoreSingleLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel &&
                                        (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube ||
                                        ((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube))
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)g10.Tag).GroupsPerGrid = 2;

                        // All Store

                        i = 0;

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
							//Begin Track #5006 - JScott - Display Low-levels one at a time
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
							//End Track #5006 - JScott - Display Low-levels one at a time
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, "All Store", "All Store"), true);

                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
							//Begin Track #5006 - JScott - Display Low-levels one at a time
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
							//End Track #5006 - JScott - Display Low-levels one at a time
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", "All Store|ADJ"));
                        }
                        else
                        {
                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
							//Begin Track #5006 - JScott - Display Low-levels one at a time
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
							//End Track #5006 - JScott - Display Low-levels one at a time
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "All Store", "All Store"), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
								{
								//End Track #6010 - JScott - Bad % Change on Basis2
									i++;

									if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube)
									{
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
										//Begin Track #5006 - JScott - Display Low-levels one at a time
										//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
										//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
										//End Track #5006 - JScott - Display Low-levels one at a time
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + detailHeader.Name));
									}
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								}
								//End Track #6010 - JScott - Bad % Change on Basis2
							}
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                i++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
								////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "All Store|" + ((BasisProfile)basisHeader.Profile).Name));
								//gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key), "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key)));
								////End Track #5782
								gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key), "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key)));
								//End Track #5006 - JScott - Display Low-levels one at a time

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            i++;

                                            if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
												//Begin Track #5006 - JScott - Display Low-levels one at a time
												//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
												//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentStorePlanProfile.NodeProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentStorePlanProfile.VersionProfile.Key));
												//End Track #5006 - JScott - Display Low-levels one at a time
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
												//Begin Track #5006 - JScott - Display Low-levels one at a time
												////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
												////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
												//gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
												////End Track #5782
												gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentStorePlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
												//End Track #5006 - JScott - Display Low-levels one at a time
											}
                                        }
                                    }
                                }
                            }
                        }

                        gridRowList.RowsPerGroup = i;

                        // Chain

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
							//Begin Track #5006 - JScott - Display Low-levels one at a time
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
							//End Track #5006 - JScott - Display Low-levels one at a time
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, "Chain", "Chain"), true);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
							//Begin Track #5006 - JScott - Display Low-levels one at a time
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
							//End Track #5006 - JScott - Display Low-levels one at a time
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", "Chain|ADJ"));
                        }
                        else
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
							//Begin Track #5006 - JScott - Display Low-levels one at a time
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
							//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
							//End Track #5006 - JScott - Display Low-levels one at a time
							cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "Chain", "Chain"), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
								{
								//End Track #6010 - JScott - Bad % Change on Basis2
									if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube)
									{
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
										//Begin Track #5006 - JScott - Display Low-levels one at a time
										//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
										//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
										//End Track #5006 - JScott - Display Low-levels one at a time
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + detailHeader.Name));
									}
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								}
								//End Track #6010 - JScott - Bad % Change on Basis2
							}
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
								//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
								//End Track #5006 - JScott - Display Low-levels one at a time
								cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
								//Begin Track #5006 - JScott - Display Low-levels one at a time
								////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
								////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Chain|" + ((BasisProfile)basisHeader.Profile).Name));
								//gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key), "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key)));
								////End Track #5782
								gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key), "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key)));
								//End Track #5006 - JScott - Display Low-levels one at a time

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
												//Begin Track #5006 - JScott - Display Low-levels one at a time
												//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
												//cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _currentChainPlanProfile.NodeProfile.Key));
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _currentChainPlanProfile.VersionProfile.Key));
												//End Track #5006 - JScott - Display Low-levels one at a time
												cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
												//Begin Track #5006 - JScott - Display Low-levels one at a time
												////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
												////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
												//gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
												////End Track #5782
												gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _currentChainPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
												//End Track #5006 - JScott - Display Low-levels one at a time
											}
                                        }
                                    }
                                }
                            }
                        }

                        g10.Cols.Count = FIXEDROWHEADERS;
                        g10.Cols.Fixed = FIXEDROWHEADERS;

                        gridRowList.BuildGridRows(g10, 0);

                        ((PagingGridTag)g10.Tag).Visible = true;
                        ((PagingGridTag)g10.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g10.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;

                        break;

                    case ePlanSessionType.ChainMultiLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)g10.Tag).GroupsPerGrid = 3;

                        // Low Level Total

                        i = 0;

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, "Low Level Total", "Low Level Total"), true);

                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", "Low Level Total|ADJ"));
                        }
                        else
                        {
                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "Low Level Total", "Low Level Total"), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
								{
								//End Track #6010 - JScott - Bad % Change on Basis2
									i++;

									if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
									{
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Low Level Total|" + detailHeader.Name));
									}
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								}
								//End Track #6010 - JScott - Bad % Change on Basis2
							}
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                i++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            i++;

                                            if (((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        gridRowList.RowsPerGroup = i;

                        // High Level

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, _openParms.ChainHLPlanProfile.NodeProfile.Text, _openParms.ChainHLPlanProfile.NodeProfile.Text), true);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", _openParms.ChainHLPlanProfile.NodeProfile.Text + "|" + "ADJ"));
                        }
                        else
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, _openParms.ChainHLPlanProfile.NodeProfile.Text, _openParms.ChainHLPlanProfile.NodeProfile.Text), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
								{
								//End Track #6010 - JScott - Bad % Change on Basis2
									if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
									{
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, _openParms.ChainHLPlanProfile.NodeProfile.Text + "|" + detailHeader.Name));
									}
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								}
								//End Track #6010 - JScott - Bad % Change on Basis2
							}
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, _openParms.ChainHLPlanProfile.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key), _openParms.ChainHLPlanProfile.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key)));
                                //End Track #5782

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            if (((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, _openParms.ChainHLPlanProfile.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, _openParms.ChainHLPlanProfile.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5782
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Balance

                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "Balance", "Balance"), true);

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Balance|" + ((BasisProfile)basisHeader.Profile).Name));
                            }
                        }

                        g10.Cols.Count = FIXEDROWHEADERS;
                        g10.Cols.Fixed = FIXEDROWHEADERS;

                        gridRowList.BuildGridRows(g10, 0);

                        ((PagingGridTag)g10.Tag).Visible = true;
                        ((PagingGridTag)g10.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g10.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;

                        break;

                    case ePlanSessionType.StoreMultiLevel:

                        foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
                        {
                            if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                            {
                                if (_selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube ||
                                        ((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube)
                                    {
                                        compList.Add(detailHeader);
                                    }
                                }
                            }
                        }

                        ((PagingGridTag)g10.Tag).GroupsPerGrid = 3;

                        // All Store High Level

                        i = 0;
                        initVisibleRows = 0;

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            i++;
                            initVisibleRows++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, "All Store", "All Store"), true);

                            i++;
                            initVisibleRows++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", "All Store|ADJ"));
                        }
                        else
                        {
                            i++;
                            initVisibleRows++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "All Store", "All Store"), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
								{
								//End Track #6010 - JScott - Bad % Change on Basis2
									i++;
									initVisibleRows++;

									if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
										((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
									{
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + detailHeader.Name));
									}
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								}
								//End Track #6010 - JScott - Bad % Change on Basis2
							}
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                i++;
                                initVisibleRows++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "All Store|" + ((BasisProfile)basisHeader.Profile).Name));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key), "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key)));
                                //End Track #5782

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            i++;
                                            initVisibleRows++;

                                            if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
                                                ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.StoreHLPlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.StoreHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.StoreHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5782
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // All Store Low Level Total

                        i++;
                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, "Low Level Total", "All Store|Low Level Total"));

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
							if (detailHeader.IsDisplayed)
							{
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
								{
								//End Track #6010 - JScott - Bad % Change on Basis2
									i++;

									if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
										((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
									{
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|Low Level Total|" + detailHeader.Name));
									}
								}
							//Begin Track #6010 - JScott - Bad % Change on Basis2
							}
							//End Track #6010 - JScott - Bad % Change on Basis2
						}

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                i++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "All Store|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            i++;

                                            if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
                                                ((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // All Store Balance

                        i++;
                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, "Balance", "All Store|Balance"));

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                i++;
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "All Store|Balance|" + ((BasisProfile)basisHeader.Profile).Name));
                            }
                        }

                        foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
                        {
                            // All Store Low Level

                            i++;
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, "All Store|" + planProf.NodeProfile.Text));

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
									{
									//End Track #6010 - JScott - Bad % Change on Basis2
										i++;

										if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
											((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
										{
											cubeWaferCoordinateList = new CubeWaferCoordinateList();
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
											gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
										}
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									}
									//End Track #6010 - JScott - Bad % Change on Basis2
								}
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    i++;

                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "All Store|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), "All Store|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key)));
                                    //End Track #5782

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                i++;

                                                if (((QuantityVariableProfile)detailHeader.Profile).isStoreTotalCube &&
                                                    ((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                                {
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StorePlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.StoreTotal, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "All Store|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + detailHeader.Name));
                                                    //End Track #5782
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        gridRowList.RowsPerGroup = i;

                        // Chain High Level

                        if (_adjustmentRowHeader.IsDisplayed)
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _originalRowHeader, gridRowList.Count, "Chain", "Chain"), true);

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _adjustmentRowHeader, gridRowList.Count, "ADJ", "Chain|ADJ"));
                        }
                        else
                        {
                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "Chain", "Chain"), true);
                        }

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
								{
								//End Track #6010 - JScott - Bad % Change on Basis2
									if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
										((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
									{
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + detailHeader.Name));
									}
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								}
								//End Track #6010 - JScott - Bad % Change on Basis2
							}
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Chain|" + ((BasisProfile)basisHeader.Profile).Name));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key), "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key)));
                                //End Track #5782

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                                ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key) + "|" + detailHeader.Name));
                                                //End Track #5782
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Chain Low Level Total

                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, "Low Level Total", "Chain|Low Level Total"));

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
								{
								//End Track #6010 - JScott - Bad % Change on Basis2
									if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
										((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
									{
										cubeWaferCoordinateList = new CubeWaferCoordinateList();
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
										cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
										gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|Low Level Total|" + detailHeader.Name));
									}
								//Begin Track #6010 - JScott - Bad % Change on Basis2
								}
								//End Track #6010 - JScott - Bad % Change on Basis2
							}
                        }

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Chain|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name));

                                foreach (RowColProfileHeader detailHeader in compList)
                                {
                                    if (detailHeader.IsDisplayed)
                                    {
                                        if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                            detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                        {
                                            if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                                ((QuantityVariableProfile)detailHeader.Profile).isLowLevelTotal)
                                            {
                                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|Low Level Total|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Chain Balance

                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, "Balance", "Chain|Balance"));

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotalVersion, Include.FV_PlanLowLevelTotalRID));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.LowLevelTotal, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.BalanceQuantity.Key));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Chain|Balance|" + ((BasisProfile)basisHeader.Profile).Name));
                            }
                        }

                        foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
                        {
                            // Chain Low Level

                            cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                            gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, planProf.NodeProfile.Text, "Chain|" + planProf.NodeProfile.Text));

                            foreach (RowColProfileHeader detailHeader in compList)
                            {
                                if (detailHeader.IsDisplayed)
                                {
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
									{
									//End Track #6010 - JScott - Bad % Change on Basis2
										if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
											((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
										{
											cubeWaferCoordinateList = new CubeWaferCoordinateList();
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
											cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
											gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + planProf.NodeProfile.Text + "|" + detailHeader.Name));
										}
									//Begin Track #6010 - JScott - Bad % Change on Basis2
									}
									//End Track #6010 - JScott - Bad % Change on Basis2
								}
                            }

                            foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                            {
                                if (basisHeader.IsDisplayed)
                                {
                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Chain|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name));
                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key), "Chain|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key)));
                                    //End Track #5782

                                    foreach (RowColProfileHeader detailHeader in compList)
                                    {
                                        if (detailHeader.IsDisplayed)
                                        {
                                            if (basisHeader.Sequence != ((RowColProfileHeader)_selectableBasisHeaders[_selectableBasisHeaders.Count - 1]).Sequence ||
                                                detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                            {
                                                if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube &&
                                                    ((QuantityVariableProfile)detailHeader.Profile).isLowLevel)
                                                {
                                                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, planProf.NodeProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, planProf.VersionProfile.Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                                    //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + planProf.NodeProfile.Text + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                                    gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, detailHeader, gridRowList.Count, detailHeader.Name, "Chain|" + planProf.NodeProfile.Text + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, planProf.NodeProfile.Key) + "|" + detailHeader.Name));
                                                    //End Track #5782
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Difference High Level

                        cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.DifferenceQuantity.Key));
                        gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, _currentRowHeader, gridRowList.Count, "Difference", "Difference"), true);

                        foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                        {
                            if (basisHeader.IsDisplayed)
                            {
                                cubeWaferCoordinateList = new CubeWaferCoordinateList();
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.DifferenceQuantity.Key));
                                //Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, "Difference|" + ((BasisProfile)basisHeader.Profile).Name));
                                gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, null, null, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key), "Difference|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key)));
                                //End Track #5782
                            }
                        }

                        g10.Cols.Count = FIXEDROWHEADERS;
                        g10.Cols.Fixed = FIXEDROWHEADERS;

                        gridRowList.BuildGridRows(g10, 0);

                        ((PagingGridTag)g10.Tag).Visible = true;
                        ((PagingGridTag)g10.Tag).DetailsPerGroup = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g10.Tag).UnitsPerScroll = gridRowList.RowsPerGroup;
                        ((PagingGridTag)g10.Tag).InitialVisibleRows = initVisibleRows;

                        break;
                }

                _gridData[Grid10] = new CellTag[g10.Rows.Count, g10.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg11Grid(bool aClearGrid)
        {
            try
            {
                if (aClearGrid)
                {
                    g11.Clear();
                }

                if (g11.Tag == null)
                {
                    g11.Tag = new PagingGridTag(Grid11, g11, g10, g2, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                if (_openParms.GetSummaryDateProfile(_sab.ClientServerSession) != null && ((PagingGridTag)g10.Tag).Visible)
                {
                    g11.Rows.Count = g10.Rows.Count;
                    g11.Cols.Count = g2.Cols.Count;
                    g11.Rows.Fixed = 0;
                    g11.Cols.Fixed = 0;

                    foreach (Row row in g11.Rows)
                    {
                        row.UserData = new RowTag();
                    }

                    foreach (Column col in g11.Cols)
                    {
                        col.UserData = new ColumnTag();
                    }

                    ((PagingGridTag)g11.Tag).Visible = true;
                }
                else
                {
                    g11.Cols.Count = 0;
                    g11.Rows.Count = 0;
                    ((PagingGridTag)g11.Tag).Visible = false;
                }

                _gridData[Grid11] = new CellTag[g11.Rows.Count, g11.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Formatg12Grid(bool aClearGrid)
        {
            try
            {
                if (aClearGrid)
                {
                    g12.Clear();
                }

                if (g12.Tag == null)
                {
                    g12.Tag = new PagingGridTag(Grid12, g12, g10, g3, new PageLoadDelegate(GetCellRange), ROWPAGESIZE, COLPAGESIZE);
                }

                if (((PagingGridTag)g10.Tag).Visible)
                {
                    g12.Rows.Count = g10.Rows.Count;
                    g12.Cols.Count = g3.Cols.Count;
                    g12.Rows.Fixed = 0;
                    g12.Cols.Fixed = 0;

                    foreach (Row row in g12.Rows)
                    {
                        row.UserData = new RowTag();
                    }

                    foreach (Column col in g12.Cols)
                    {
                        col.UserData = new ColumnTag();
                    }

                    ((PagingGridTag)g12.Tag).Visible = true;
                }
                else
                {
                    g12.Cols.Count = 0;
                    g12.Rows.Count = 0;
                    ((PagingGridTag)g12.Tag).Visible = false;
                }

                _gridData[Grid12] = new CellTag[g12.Rows.Count, g12.Cols.Count];
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        #endregion

        #region Preferences, Positioning, Resizing, Freezing (Cols & Rows)

        private int CalculateGroupFromDetail(C1FlexGrid aGrid, int aDetail)
        {
            PagingGridTag gridTag;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;
                if (gridTag.UnitsPerScroll > 0)
                {
                    if (gridTag.ScrollType == eScrollType.Group)
                    {
                        return aDetail / ((PagingGridTag)aGrid.Tag).UnitsPerScroll;
                    }
                    else
                    {
                        return aDetail;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ResizeCol1()
        {
            int i;
            int MinColWidth = 0;

            try
            {
                g4.AutoSizeCols();
                g7.AutoSizeCols();
                g10.AutoSizeCols();

                for (i = 0; i < g4.Cols.Count; i++)
                {
                    if (((PagingGridTag)g4.Tag).Visible)
                    {
                        MinColWidth = g4.Cols[i].WidthDisplay;
                    }
                    if (((PagingGridTag)g7.Tag).Visible)
                    {
                        MinColWidth = Math.Max(MinColWidth, g7.Cols[i].WidthDisplay);
                    }
                    if (((PagingGridTag)g10.Tag).Visible)
                    {
                        MinColWidth = Math.Max(MinColWidth, g10.Cols[i].WidthDisplay);
                    }

                    if (((PagingGridTag)g4.Tag).Visible)
                    {
                        g4.Cols[i].WidthDisplay = MinColWidth;
                        g4.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                    if (((PagingGridTag)g7.Tag).Visible)
                    {
                        g7.Cols[i].WidthDisplay = MinColWidth;
                        g7.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                    if (((PagingGridTag)g10.Tag).Visible)
                    {
                        g10.Cols[i].WidthDisplay = MinColWidth;
                        g10.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ResizeCol2()
        {
            C1FlexGrid grid = null;
            int i;
            int MinColWidth = 0;

            try
            {
                for (i = 0; i < g2.Cols.Count; i++)
                {
                    if (((PagingGridTag)g5.Tag).Visible)
                    {
                        g5.Cols[i].WidthDisplay = 1;
                    }
                    if (((PagingGridTag)g8.Tag).Visible)
                    {
                        g8.Cols[i].WidthDisplay = 1;
                    }
                    if (((PagingGridTag)g11.Tag).Visible)
                    {
                        g11.Cols[i].WidthDisplay = 1;
                    }
                }

                g5.AutoSizeCols();
                g8.AutoSizeCols();
                g11.AutoSizeCols();

                for (i = 0; i < g2.Cols.Count; i++)
                {
                    MinColWidth = FindMinimumColWidth(ref grid, i, g2, g5, g8, g11);
                    if (((PagingGridTag)g5.Tag).Visible)
                    {
                        MinColWidth = Math.Max(MinColWidth, g5.Cols[i].WidthDisplay);
                    }
                    if (((PagingGridTag)g8.Tag).Visible)
                    {
                        MinColWidth = Math.Max(MinColWidth, g8.Cols[i].WidthDisplay);
                    }
                    if (((PagingGridTag)g11.Tag).Visible)
                    {
                        MinColWidth = Math.Max(MinColWidth, g11.Cols[i].WidthDisplay);
                    }

                    if (((PagingGridTag)g2.Tag).Visible)
                    {
                        g2.Cols[i].WidthDisplay = MinColWidth;
                        g2.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                    if (((PagingGridTag)g5.Tag).Visible)
                    {
                        g5.Cols[i].WidthDisplay = MinColWidth;
                        g5.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                    if (((PagingGridTag)g8.Tag).Visible)
                    {
                        g8.Cols[i].WidthDisplay = MinColWidth;
                        g8.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                    if (((PagingGridTag)g11.Tag).Visible)
                    {
                        g11.Cols[i].WidthDisplay = MinColWidth;
                        g11.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ResizeCol3()
        {
            C1FlexGrid grid = null;
            int i;
            int MinColWidth = 0;

            try
            {
                for (i = 0; i < g3.Cols.Count; i++)
                {
                    if (((PagingGridTag)g6.Tag).Visible)
                    {
                        g6.Cols[i].WidthDisplay = 1;
                    }
                    if (((PagingGridTag)g9.Tag).Visible)
                    {
                        g9.Cols[i].WidthDisplay = 1;
                    }
                    if (((PagingGridTag)g12.Tag).Visible)
                    {
                        g12.Cols[i].WidthDisplay = 1;
                    }
                }

                g6.AutoSizeCols();
                g9.AutoSizeCols();
                g12.AutoSizeCols();

            //TT#2586 - MD - Weeks disappear from OTS Forecast when sorting - RBeck  
                MinColWidth = 0;
                for (i = 0; i < g3.Cols.Count; i++)
                {   
                    MinColWidth = Math.Max(FindMinimumColWidth(ref grid, i, g3, g6, g9, g12),MinColWidth) ;                
                }                
            //TT#2586 - MD - Weeks disappear from OTS Forecast when sorting - RBeck  

                for (i = 0; i < g3.Cols.Count; i++)
                {
                    //MinColWidth = FindMinimumColWidth(ref grid, i, g3, g6, g9, g12);
                    
                    if (((PagingGridTag)g6.Tag).Visible)
                    {
                        MinColWidth = Math.Max(MinColWidth, g6.Cols[i].WidthDisplay);
                    }
                    if (((PagingGridTag)g9.Tag).Visible)
                    {
                        MinColWidth = Math.Max(MinColWidth, g9.Cols[i].WidthDisplay);
                    }
                    if (((PagingGridTag)g12.Tag).Visible)
                    {
                        MinColWidth = Math.Max(MinColWidth, g12.Cols[i].WidthDisplay);
                    }

                    if (((PagingGridTag)g3.Tag).Visible)
                    {
                        g3.Cols[i].WidthDisplay = MinColWidth;
                        g3.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                    if (((PagingGridTag)g6.Tag).Visible)
                    {
                        g6.Cols[i].WidthDisplay = MinColWidth;
                        g6.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                    if (((PagingGridTag)g9.Tag).Visible)
                    {
                        g9.Cols[i].WidthDisplay = MinColWidth;
                        g9.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                    if (((PagingGridTag)g12.Tag).Visible)
                    {
                        g12.Cols[i].WidthDisplay = MinColWidth;
                        g12.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ResizeRow1()
        {
            int i;
            int MinRowHeight = 0;

            try
            {
                g3.AutoSizeRows();
                g2.AutoSizeRows();

                for (i = 0; i < g3.Rows.Count; i++)
                {
                    if (((PagingGridTag)g3.Tag).Visible)
                    {
                        MinRowHeight = g3.Rows[i].HeightDisplay;
                    }
                    if (((PagingGridTag)g2.Tag).Visible)
                    {
                        MinRowHeight = Math.Max(MinRowHeight, g2.Rows[i].HeightDisplay);
                    }

                    if (((PagingGridTag)g3.Tag).Visible)
                    {
                        g3.Rows[i].HeightDisplay = MinRowHeight;
                    }
                    if (((PagingGridTag)g2.Tag).Visible)
                    {
                        g2.Rows[i].HeightDisplay = MinRowHeight;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ResizeRow4()
        {
            int i;
            int MinRowHeight = 0;

            try
            {
                MinRowHeight = FindMinimumRowHeight(g4, g5, g6);

                for (i = 0; i < g4.Rows.Count; i++)
                {
                    if (((PagingGridTag)g4.Tag).Visible)
                    {
                        g4.Rows[i].HeightDisplay = MinRowHeight;
                    }
                    if (((PagingGridTag)g5.Tag).Visible)
                    {
                        g5.Rows[i].HeightDisplay = MinRowHeight;
                    }
                    if (((PagingGridTag)g6.Tag).Visible)
                    {
                        g6.Rows[i].HeightDisplay = MinRowHeight;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ResizeRow7()
        {
            int i;
            int MinRowHeight = 0;

            try
            {
                MinRowHeight = FindMinimumRowHeight(g7, g8, g9);

                for (i = 0; i < g7.Rows.Count; i++)
                {
                    if (((PagingGridTag)g7.Tag).Visible)
                    {
                        g7.Rows[i].HeightDisplay = MinRowHeight;
                    }
                    if (((PagingGridTag)g8.Tag).Visible)
                    {
                        g8.Rows[i].HeightDisplay = MinRowHeight;
                    }
                    if (((PagingGridTag)g9.Tag).Visible)
                    {
                        g9.Rows[i].HeightDisplay = MinRowHeight;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ResizeRow10()
        {
            int i;
            int MinRowHeight = 0;

            try
            {
                MinRowHeight = FindMinimumRowHeight(g10, g11, g12);

                for (i = 0; i < g10.Rows.Count; i++)
                {
                    if (g10.Rows[i].Visible)
                    {
                        if (((PagingGridTag)g10.Tag).Visible)
                        {
                            g10.Rows[i].HeightDisplay = MinRowHeight;
                        }
                        if (((PagingGridTag)g11.Tag).Visible)
                        {
                            g11.Rows[i].HeightDisplay = MinRowHeight;
                        }
                        if (((PagingGridTag)g12.Tag).Visible)
                        {
                            g12.Rows[i].HeightDisplay = MinRowHeight;
                        }
                    }
                    else
                    {
                        if (((PagingGridTag)g11.Tag).Visible)
                        {
                            g11.Rows[i].Visible = false;
                        }
                        if (((PagingGridTag)g12.Tag).Visible)
                        {
                            g12.Rows[i].Visible = false;
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

        private int FindMinimumColWidth(ref C1FlexGrid aGrid, int aCol, C1FlexGrid aColHeaderGrid, C1FlexGrid aCol2Grid, C1FlexGrid aCol3Grid, C1FlexGrid aCol4Grid)
        {
            int i;
            char[] delimeters;
            int maxCellValLength;
            string cellVal;
            string[] splitCellVal;
            string formatString;
			//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
			Image cellImg;
			//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
            // Begin TT#1096 - JSmith - OTS Forecast Sorting
            bool addImageLength = false;
            // End TT#1096

            try
            {
                if (aGrid == null)
                {
                    aGrid = new C1FlexGrid();
                    aGrid.Cols.Count = 1;
                    aGrid.Rows.Count = 26;

                    aGrid.SetCellStyle(0, 0, aColHeaderGrid.Styles["Style1"]);
                    aGrid.SetCellStyle(1, 0, aColHeaderGrid.Styles["Style2"]);
                    if (((PagingGridTag)aCol2Grid.Tag).Visible)
                    {
                        aGrid.SetCellStyle(2, 0, aCol2Grid.Styles["Style1"]);
                        aGrid.SetCellStyle(3, 0, aCol2Grid.Styles["Style1Editable"]);
                        aGrid.SetCellStyle(4, 0, aCol2Grid.Styles["Style1Negative"]);
                        aGrid.SetCellStyle(5, 0, aCol2Grid.Styles["Style1NegativeEditable"]);
                        aGrid.SetCellStyle(6, 0, aCol2Grid.Styles["Style2"]);
                        aGrid.SetCellStyle(7, 0, aCol2Grid.Styles["Style2Editable"]);
                        aGrid.SetCellStyle(8, 0, aCol2Grid.Styles["Style2Negative"]);
                        aGrid.SetCellStyle(9, 0, aCol2Grid.Styles["Style2NegativeEditable"]);
                    }
                    if (((PagingGridTag)aCol3Grid.Tag).Visible)
                    {
                        aGrid.SetCellStyle(10, 0, aCol3Grid.Styles["Style1"]);
                        aGrid.SetCellStyle(11, 0, aCol3Grid.Styles["Style1Editable"]);
                        aGrid.SetCellStyle(12, 0, aCol3Grid.Styles["Style1Negative"]);
                        aGrid.SetCellStyle(13, 0, aCol3Grid.Styles["Style1NegativeEditable"]);
                        aGrid.SetCellStyle(14, 0, aCol3Grid.Styles["Style2"]);
                        aGrid.SetCellStyle(15, 0, aCol3Grid.Styles["Style2Editable"]);
                        aGrid.SetCellStyle(16, 0, aCol3Grid.Styles["Style2Negative"]);
                        aGrid.SetCellStyle(17, 0, aCol3Grid.Styles["Style2NegativeEditable"]);
                    }
                    if (((PagingGridTag)aCol4Grid.Tag).Visible)
                    {
                        aGrid.SetCellStyle(18, 0, aCol4Grid.Styles["Style1"]);
                        aGrid.SetCellStyle(19, 0, aCol4Grid.Styles["Style1Editable"]);
                        aGrid.SetCellStyle(20, 0, aCol4Grid.Styles["Style1Negative"]);
                        aGrid.SetCellStyle(21, 0, aCol4Grid.Styles["Style1NegativeEditable"]);
                        aGrid.SetCellStyle(22, 0, aCol4Grid.Styles["Style2"]);
                        aGrid.SetCellStyle(23, 0, aCol4Grid.Styles["Style2Editable"]);
                        aGrid.SetCellStyle(24, 0, aCol4Grid.Styles["Style2Negative"]);
                        aGrid.SetCellStyle(25, 0, aCol4Grid.Styles["Style2NegativeEditable"]);
                    }
                }

                delimeters = new char[1];
                delimeters[0] = ' ';

                maxCellValLength = MINCOLSIZE;

				for (i = 1; i < aColHeaderGrid.Rows.Count; i++)
				{
                    cellVal = (string)aColHeaderGrid[i, aCol];
                    splitCellVal = cellVal.Split(delimeters);
                    foreach (string str in splitCellVal)
                    {
                        maxCellValLength = Math.Max(maxCellValLength, str.Length);
                    }
                }

				//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				if (((PagingGridTag)aColHeaderGrid.Tag).SortImageRow != -1)
				{
					cellImg = aColHeaderGrid.GetCellImage(((PagingGridTag)aColHeaderGrid.Tag).SortImageRow, aCol);

                    // Begin TT#1096 - JSmith - OTS Forecast Sorting
                    //aGrid.SetCellImage(0, 0, cellImg);
                    //aGrid.SetCellImage(1, 0, cellImg);
                    if (cellImg == null)
                    {
                        addImageLength = true;
                    }
                    else
                    {
                        aGrid.SetCellImage(0, 0, cellImg);
                        aGrid.SetCellImage(1, 0, cellImg);
                    }
                    // End TT#1096
                }
                // Begin TT#1096 - JSmith - OTS Forecast Sorting
                else
                {
                    addImageLength = true;
                }
                // End TT#1096

				//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				formatString = "";
				formatString = formatString.PadLeft(maxCellValLength, 'O');

                for (i = 0; i < aGrid.Rows.Count; i++)
                {
                    aGrid[i, 0] = formatString;
                }

                aGrid.AutoSizeCols();

                // Begin TT#1096 - JSmith - OTS Forecast Sorting
                if (addImageLength)
                {
                    return aGrid.Cols[0].WidthDisplay + _downArrow.Width + 5;
                }
                else
                {
                    return aGrid.Cols[0].WidthDisplay;
                }
                // End TT#1096
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private int FindMinimumRowHeight(C1FlexGrid aRowHeaderGrid, C1FlexGrid aRow2Grid, C1FlexGrid aRow3Grid)
        {
            int i;
            C1FlexGrid grid;

            try
            {
                grid = new C1FlexGrid();
                grid.Cols.Count = 18;
                grid.Rows.Count = 1;

                for (i = 0; i < grid.Cols.Count; i++)
                {
                    grid[0, i] = "X";
                }

                grid.SetCellStyle(0, 0, aRowHeaderGrid.Styles["Style1"]);
                grid.SetCellStyle(0, 1, aRowHeaderGrid.Styles["Style2"]);
                if (((PagingGridTag)aRow2Grid.Tag).Visible)
                {
                    grid.SetCellStyle(0, 2, aRow2Grid.Styles["Style1"]);
                    grid.SetCellStyle(0, 3, aRow2Grid.Styles["Style1Editable"]);
                    grid.SetCellStyle(0, 4, aRow2Grid.Styles["Style1Negative"]);
                    grid.SetCellStyle(0, 5, aRow2Grid.Styles["Style1NegativeEditable"]);
                    grid.SetCellStyle(0, 6, aRow2Grid.Styles["Style2"]);
                    grid.SetCellStyle(0, 7, aRow2Grid.Styles["Style2Editable"]);
                    grid.SetCellStyle(0, 8, aRow2Grid.Styles["Style2Negative"]);
                    grid.SetCellStyle(0, 9, aRow2Grid.Styles["Style2NegativeEditable"]);
                }
                if (((PagingGridTag)aRow3Grid.Tag).Visible)
                {
                    grid.SetCellStyle(0, 10, aRow3Grid.Styles["Style1"]);
                    grid.SetCellStyle(0, 11, aRow3Grid.Styles["Style1Editable"]);
                    grid.SetCellStyle(0, 12, aRow3Grid.Styles["Style1Negative"]);
                    grid.SetCellStyle(0, 13, aRow3Grid.Styles["Style1NegativeEditable"]);
                    grid.SetCellStyle(0, 14, aRow3Grid.Styles["Style2"]);
                    grid.SetCellStyle(0, 15, aRow3Grid.Styles["Style2Editable"]);
                    grid.SetCellStyle(0, 16, aRow3Grid.Styles["Style2Negative"]);
                    grid.SetCellStyle(0, 17, aRow3Grid.Styles["Style2NegativeEditable"]);
                }

                grid.AutoSizeRows();

                return grid.Rows[0].HeightDisplay;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CalcColSplitPosition2(bool aOverrideLock)
        {
            try
            {
                if (((PagingGridTag)g4.Tag).Visible)
                {
                    if (!((SplitterTag)spcVLevel1.Tag).Locked || aOverrideLock)
                    {
						_currColSplitPosition2 = g4.Cols[g4.Cols.Count - 1].Right + 2;
                    }
                }
                else
                {
                    _currColSplitPosition2 = 0;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CalcColSplitPosition3(bool aOverrideLock)
        {
            int lastKey;
            int i;

            try
            {
                if (((PagingGridTag)g2.Tag).Visible)
                {
                    if (!((SplitterTag)spcVLevel2.Tag).Locked || aOverrideLock)
                    {
						//Begin Track #5006 - JScott - Display Low-levels one at a time
						//if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                        if (_currentPlanSessionType == ePlanSessionType.ChainSingleLevel)
						//End Track #5006 - JScott - Display Low-levels one at a time
                        {
                            i = g2.Cols.Count;
                        }
                        else if (_columnGroupedBy == GroupedBy.GroupedByTime)
                        {
                            lastKey = ((ColumnHeaderTag)g2.Cols[g2.LeftCol].UserData).DetailRowColHeader.Profile.Key;

                            for (i = g2.LeftCol + 1; i < g2.Cols.Count; i++)
                            {
                                if (((ColumnHeaderTag)g2.Cols[i].UserData).DetailRowColHeader.Profile.Key != lastKey)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            lastKey = ((ColumnHeaderTag)g2.Cols[g2.LeftCol].UserData).GroupRowColHeader.Profile.Key;

                            for (i = g2.LeftCol + 1; i < g2.Cols.Count; i++)
                            {
                                if (((ColumnHeaderTag)g2.Cols[i].UserData).GroupRowColHeader.Profile.Key != lastKey)
                                {
                                    break;
                                }
                            }
                        }

						_currColSplitPosition3 = (g2.Cols[i - 1].Right - g2.Cols[g2.LeftCol].Left) + 2;
                    }
                }
                else
                {
                    _currColSplitPosition3 = 0;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SetColSplitPositions()
        {
            int spcHDetailLevel2SplitPos;

            try
            {
                spcHDetailLevel2SplitPos = _currColSplitPosition3;

                _hSplitMove = true;

                spcVLevel2.SplitterDistance = 0;

                _hSplitMove = false;

				_currColSplitPosition3 = spcHDetailLevel2SplitPos;
				spcVLevel1.SplitterDistance = _currColSplitPosition2;
                spcVLevel2.SplitterDistance = spcHDetailLevel2SplitPos;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CalcRowSplitPosition4(bool aOverrideLock)
        {
            try
            {
                if (((PagingGridTag)g3.Tag).Visible)
                {
                    if (!((SplitterTag)spcHScrollLevel2.Tag).Locked || aOverrideLock)
                    {
						_currRowSplitPosition4 = g3.Rows[g3.Rows.Count - 1].Bottom + 2;
                    }
                }
                else
                {
                    _currRowSplitPosition4 = 0;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CalcRowSplitPosition8(bool aOverrideLock)
        {
            try
            {
                if (((PagingGridTag)g7.Tag).Visible)
                {
                    if (!((SplitterTag)spcHScrollLevel3.Tag).Locked || aOverrideLock)
                    {
                        if (((PagingGridTag)g10.Tag).InitialVisibleRows != -1)
                        {
							_currRowSplitPosition8 = g7.Rows[((PagingGridTag)g7.Tag).InitialVisibleRows - 1].Bottom + spcHScrollLevel3.SplitterWidth + 2;
                        }
                        else
                        {
							_currRowSplitPosition8 = g7.Rows[((PagingGridTag)g7.Tag).UnitsPerScroll - 1].Bottom + spcHScrollLevel3.SplitterWidth + 2;
                        }
                    }
                }
                else
                {
                    _currRowSplitPosition8 = 0;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CalcRowSplitPosition12(bool aOverrideLock)
        {
            try
            {
                if (((PagingGridTag)g10.Tag).Visible)
                {
                    if (!((SplitterTag)spcHScrollLevel1.Tag).Locked || aOverrideLock)
                    {
                        if (((PagingGridTag)g10.Tag).InitialVisibleRows != -1)
                        {
							//Begin Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
							//_currRowSplitPosition12 = g10.Rows[((PagingGridTag)g10.Tag).InitialVisibleRows - 1].Bottom + pnlSpacer.Height + spcHScrollLevel1.SplitterWidth + 2;
							_currRowSplitPosition12 = g10.Rows[((PagingGridTag)g10.Tag).InitialVisibleRows - 1].Bottom + spcHScrollLevel1.SplitterWidth + 2;
							//End Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
						}
                        else
                        {
							//Begin Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
							//_currRowSplitPosition12 = g10.Rows[g10.Rows.Count - 1].Bottom + pnlSpacer.Height + spcHScrollLevel1.SplitterWidth + 2;
							_currRowSplitPosition12 = g10.Rows[g10.Rows.Count - 1].Bottom + spcHScrollLevel1.SplitterWidth + 2;
							//End Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
						}
                    }
                }
                else
                {
                    _currRowSplitPosition12 = 0;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SetRowSplitPositions()
        {
            int spcHScrollLevel1SplitPos;
            int spcHScrollLevel2SplitPos;
            int spcHScrollLevel3SplitPos;

            try
            {
				spcHScrollLevel1SplitPos = _currRowSplitPosition12;
                spcHScrollLevel2SplitPos = _currRowSplitPosition4;
                spcHScrollLevel3SplitPos = _currRowSplitPosition8;

                spcHScrollLevel3.SplitterDistance = spcHScrollLevel3.Height;
				//Begin Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
				//spcHScrollLevel2.SplitterDistance = 0;
				//spcHScrollLevel1.SplitterDistance = 0;
				spcHScrollLevel2.SplitterDistance = spcHScrollLevel2.Panel1MinSize;
				spcHScrollLevel1.SplitterDistance = spcHScrollLevel1.Panel1MinSize;
				//End Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.

				//Begin Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
				//spcHScrollLevel1.SplitterDistance = spcHScrollLevel1.Height - spcHScrollLevel1SplitPos;
				spcHScrollLevel1.SplitterDistance = spcHScrollLevel1.Height - Math.Min(spcHScrollLevel1SplitPos, spcHScrollLevel1.Height);
				//End Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
				spcHScrollLevel2.SplitterDistance = spcHScrollLevel2SplitPos;
				//Begin Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
				//spcHScrollLevel3.SplitterDistance = spcHScrollLevel3.Height - spcHScrollLevel3SplitPos;
				spcHScrollLevel3.SplitterDistance = spcHScrollLevel3.Height - Math.Min(spcHScrollLevel3SplitPos, spcHScrollLevel3.Height);
				//End Track #5570 - JScott - Received an Invalid Operation error when in Store Multi-level review.
			}
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
		}

        private void cmiFreezeColumn_Click(object sender, System.EventArgs e)
        {
            int i;
            PagingGridTag gridTag;
			//Begin Track #5003 - JScott - Freeze Rows
			int leftCol;
			//End Track #5003 - JScott - Freeze Rows

            try
            {
                gridTag = (PagingGridTag)_rightClickedFrom.Tag;

                if (cmiFreezeColumn.Checked == true) //Unfreeze the columns.
                {
                    //We're in this IF block because the column is already frozen. 
                    //The user must want to un-freeze the columns.

                    if (gridTag.GridId == Grid2)
                    {
						//Begin Track #5003 - JScott - Freeze Rows
						//for (i = 0; i < _rightClickedFrom.LeftCol; i++)
						for (i = 0; i < gridTag.LeftMostColBeforeFreeze; i++)
						//End Track #5003 - JScott - Freeze Rows
                        {
                            g2.Cols[i].Visible = true;
                            if (((PagingGridTag)g5.Tag).Visible)
                            {
                                g5.Cols[i].Visible = true;
                            }
                            if (((PagingGridTag)g8.Tag).Visible)
                            {
                                g8.Cols[i].Visible = true;
                            }
                            if (((PagingGridTag)g11.Tag).Visible)
                            {
                                g11.Cols[i].Visible = true;
                            }
                        }

                        g2.Cols.Frozen = 0;
                        g5.Cols.Frozen = 0;
                        g8.Cols.Frozen = 0;
                        g11.Cols.Frozen = 0;

                        SetHScrollBar2Parameters();
						//Begin Track #5003 - JScott - Freeze Rows

						//g2.LeftCol = _leftMostColBeforeFreeze;
						//g5.LeftCol = _leftMostColBeforeFreeze;
						//g8.LeftCol = _leftMostColBeforeFreeze;
						//g11.LeftCol = _leftMostColBeforeFreeze;
						SetScrollBarPosition(hScrollBar2, gridTag.LeftMostColBeforeFreeze);
						//End Track #5003 - JScott - Freeze Rows

                        gridTag.HasColsFrozen = false;
                    }
                    else if (gridTag.GridId == Grid3)
                    {
						//Begin Track #5003 - JScott - Freeze Rows
						//for (i = 0; i < g3.LeftCol; i++)
						for (i = 0; i < gridTag.LeftMostColBeforeFreeze; i++)
						//End Track #5003 - JScott - Freeze Rows
                        {
                            g3.Cols[i].Visible = true;
                            if (((PagingGridTag)g6.Tag).Visible)
                            {
                                g6.Cols[i].Visible = true;
                            }
                            if (((PagingGridTag)g9.Tag).Visible)
                            {
                                g9.Cols[i].Visible = true;
                            }
                            if (((PagingGridTag)g12.Tag).Visible)
                            {
                                g12.Cols[i].Visible = true;
                            }
                        }

                        g3.Cols.Frozen = 0;
                        g6.Cols.Frozen = 0;
                        g9.Cols.Frozen = 0;
                        g12.Cols.Frozen = 0;

                        SetHScrollBar3Parameters();
						//Begin Track #5003 - JScott - Freeze Rows

						//g3.LeftCol = _leftMostColBeforeFreeze;
						//g6.LeftCol = _leftMostColBeforeFreeze;
						//g9.LeftCol = _leftMostColBeforeFreeze;
						//g12.LeftCol = _leftMostColBeforeFreeze;
						SetScrollBarPosition(hScrollBar3, gridTag.LeftMostColBeforeFreeze);
						//End Track #5003 - JScott - Freeze Rows

                        gridTag.HasColsFrozen = false;
                    }
                }
                else //Freez the columns.
                {
                    //We're in this IF block because the column is not frozen.
                    //The user must want to freeze this column.
                    //We must freeze this column and all columns to the left of it.
					//Begin Track #5003 - JScott - Freeze Rows

					leftCol = _rightClickedFrom.LeftCol;

					//End Track #5003 - JScott - Freeze Rows
					if (gridTag.GridId == Grid2)
                    {
						//Begin Track #5003 - JScott - Freeze Rows
						//_leftMostColBeforeFreeze = g2.LeftCol; //this var will be used later when un-freezing.
						gridTag.LeftMostColBeforeFreeze = leftCol; //this var will be used later when un-freezing.
						//End Track #5003 - JScott - Freeze Rows

						//Begin Track #5003 - JScott - Freeze Rows
						//for (i = 0; i < g2.LeftCol; i++)
                        for (i = 0; i < leftCol; i++)
						//End Track #5003 - JScott - Freeze Rows
                        {
                            g2.Cols[i].Visible = false;
                            if (((PagingGridTag)g5.Tag).Visible)
                            {
                                g5.Cols[i].Visible = false;
                            }
                            if (((PagingGridTag)g8.Tag).Visible)
                            {
                                g8.Cols[i].Visible = false;
                            }
                            if (((PagingGridTag)g11.Tag).Visible)
                            {
                                g11.Cols[i].Visible = false;
                            }
                        }

                        g2.Cols.Frozen = gridTag.MouseDownCol;
                        g5.Cols.Frozen = gridTag.MouseDownCol;
                        g8.Cols.Frozen = gridTag.MouseDownCol;
                        g11.Cols.Frozen = gridTag.MouseDownCol;

						//Begin Track #5003 - JScott - Freeze Rows
						//g2.LeftCol = gridTag.MouseDownCol;
						//g5.LeftCol = gridTag.MouseDownCol;
						//g8.LeftCol = gridTag.MouseDownCol;
						//g11.LeftCol = gridTag.MouseDownCol;
						SetHScrollBar2Parameters();
						SetScrollBarPosition(hScrollBar2, gridTag.MouseDownCol);
						//End Track #5003 - JScott - Freeze Rows

						gridTag.HasColsFrozen = true;
						//Begin Track #5003 - JScott - Freeze Rows

						//SetHScrollBar2Parameters();
						//End Track #5003 - JScott - Freeze Rows
					}
                    else if (gridTag.GridId == Grid3)
                    {
						//Begin Track #5003 - JScott - Freeze Rows
						//_leftMostColBeforeFreeze = g3.LeftCol; //this var will be used later when un-freezing.
						gridTag.LeftMostColBeforeFreeze = leftCol; //this var will be used later when un-freezing.
						//End Track #5003 - JScott - Freeze Rows

						//Begin Track #5003 - JScott - Freeze Rows
						//for (i = 0; i < g3.LeftCol; i++)
                        for (i = 0; i < leftCol; i++)
						//End Track #5003 - JScott - Freeze Rows
                        {
                            g3.Cols[i].Visible = false;
                            if (((PagingGridTag)g6.Tag).Visible)
                            {
                                g6.Cols[i].Visible = false;
                            }
                            if (((PagingGridTag)g9.Tag).Visible)
                            {
                                g9.Cols[i].Visible = false;
                            }
                            if (((PagingGridTag)g12.Tag).Visible)
                            {
                                g12.Cols[i].Visible = false;
                            }
                        }

                        g3.Cols.Frozen = gridTag.MouseDownCol;
                        g6.Cols.Frozen = gridTag.MouseDownCol;
                        g9.Cols.Frozen = gridTag.MouseDownCol;
                        g12.Cols.Frozen = gridTag.MouseDownCol;

						//Begin Track #5003 - JScott - Freeze Rows
						//g3.LeftCol = gridTag.MouseDownCol;
						//g6.LeftCol = gridTag.MouseDownCol;
						//g9.LeftCol = gridTag.MouseDownCol;
						//g12.LeftCol = gridTag.MouseDownCol;
						SetHScrollBar3Parameters();
						SetScrollBarPosition(hScrollBar3, gridTag.MouseDownCol);
						//End Track #5003 - JScott - Freeze Rows

						gridTag.HasColsFrozen = true;
						//Begin Track #5003 - JScott - Freeze Rows

						//SetHScrollBar3Parameters();
						//End Track #5003 - JScott - Freeze Rows
					}
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

		//Begin Track #5003 - JScott - Freeze Rows
		private void cmiFreezeRow_Click(object sender, EventArgs e)
		{
			int i;
			PagingGridTag gridTag;
			int topRow;

			try
			{
				gridTag = (PagingGridTag)_rightClickedFrom.Tag;

				if (cmiFreezeRow.Checked == true) //Unfreeze the rows.
				{
					//We're in this IF block because the row is already frozen. 
					//The user must want to un-freeze the rows.

					//Begin Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
					//if (gridTag.GridId == Grid4)
					//{
					//    for (i = 0; i < gridTag.TopMostRowBeforeFreeze; i++)
					//    {
					//        g4.Rows[i].Visible = true;
					//        if (((PagingGridTag)g5.Tag).Visible)
					//        {
					//            g5.Rows[i].Visible = true;
					//        }
					//        if (((PagingGridTag)g6.Tag).Visible)
					//        {
					//            g6.Rows[i].Visible = true;
					//        }
					//    }

					//    g4.Rows.Frozen = 0;
					//    g5.Rows.Frozen = 0;
					//    g6.Rows.Frozen = 0;

					//    SetVScrollBar2Parameters();
					//    SetScrollBarPosition(vScrollBar2, gridTag.TopMostRowBeforeFreeze);

					//    gridTag.HasRowsFrozen = false;
					//}
					//else if (gridTag.GridId == Grid7)
					//{
					//    for (i = 0; i < gridTag.TopMostRowBeforeFreeze; i++)
					//    {
					//        g7.Rows[i].Visible = true;
					//        if (((PagingGridTag)g8.Tag).Visible)
					//        {
					//            g8.Rows[i].Visible = true;
					//        }
					//        if (((PagingGridTag)g9.Tag).Visible)
					//        {
					//            g9.Rows[i].Visible = true;
					//        }
					//    }

					//    g7.Rows.Frozen = 0;
					//    g8.Rows.Frozen = 0;
					//    g9.Rows.Frozen = 0;

					//    SetVScrollBar3Parameters();
					//    SetScrollBarPosition(vScrollBar3, gridTag.TopMostRowBeforeFreeze);

					//    gridTag.HasRowsFrozen = false;
					//}
					//else if (gridTag.GridId == Grid10)
					//{
					//    for (i = 0; i < gridTag.TopMostRowBeforeFreeze; i++)
					//    {
					//        g10.Rows[i].Visible = true;
					//        if (((PagingGridTag)g11.Tag).Visible)
					//        {
					//            g11.Rows[i].Visible = true;
					//        }
					//        if (((PagingGridTag)g12.Tag).Visible)
					//        {
					//            g12.Rows[i].Visible = true;
					//        }
					//    }

					//    g10.Rows.Frozen = 0;
					//    g11.Rows.Frozen = 0;
					//    g12.Rows.Frozen = 0;

					//    SetVScrollBar4Parameters();
					//    SetScrollBarPosition(vScrollBar4, gridTag.TopMostRowBeforeFreeze);

					//    gridTag.HasRowsFrozen = false;
					//}
					UnfreezeRows(gridTag);

					if (gridTag.GridId == Grid4)
					{
						SetVScrollBar2Parameters();
						SetScrollBarPosition(vScrollBar2, gridTag.TopMostRowBeforeFreeze / gridTag.UnitsPerScroll);
					}
					else if (gridTag.GridId == Grid7)
					{
						SetVScrollBar3Parameters();
						SetScrollBarPosition(vScrollBar3, gridTag.TopMostRowBeforeFreeze / gridTag.UnitsPerScroll);
					}
					else if (gridTag.GridId == Grid10)
					{
						SetVScrollBar4Parameters();
						SetScrollBarPosition(vScrollBar4, gridTag.TopMostRowBeforeFreeze / gridTag.UnitsPerScroll);
					}
					//End Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
				}
				else //Freeze the rows.
				{
					//We're in this IF block because the row is not frozen.
					//The user must want to freeze this row.
					//We must freeze this row and all rows above it.

					topRow = _rightClickedFrom.TopRow;

					//Begin Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
					//if (gridTag.GridId == Grid4)
					//{
					//    gridTag.TopMostRowBeforeFreeze = topRow; //this var will be used later when un-freezing.

					//    for (i = 0; i < topRow; i++)
					//    {
					//        g4.Rows[i].Visible = false;
					//        if (((PagingGridTag)g5.Tag).Visible)
					//        {
					//            g5.Rows[i].Visible = false;
					//        }
					//        if (((PagingGridTag)g6.Tag).Visible)
					//        {
					//            g6.Rows[i].Visible = false;
					//        }
					//    }

					//    g4.Rows.Frozen = (gridTag.MouseDownRow / gridTag.UnitsPerScroll) * gridTag.UnitsPerScroll;
					//    g5.Rows.Frozen = g4.Rows.Frozen;
					//    g6.Rows.Frozen = g4.Rows.Frozen;

					//    SetVScrollBar2Parameters();
					//    SetScrollBarPosition(vScrollBar2, gridTag.MouseDownRow / gridTag.UnitsPerScroll);

					//    gridTag.HasRowsFrozen = true;
					//}
					//else if (gridTag.GridId == Grid7)
					//{
					//    gridTag.TopMostRowBeforeFreeze = topRow; //this var will be used later when un-freezing.

					//    for (i = 0; i < topRow; i++)
					//    {
					//        g7.Rows[i].Visible = false;
					//        if (((PagingGridTag)g8.Tag).Visible)
					//        {
					//            g8.Rows[i].Visible = false;
					//        }
					//        if (((PagingGridTag)g9.Tag).Visible)
					//        {
					//            g9.Rows[i].Visible = false;
					//        }
					//    }

					//    g7.Rows.Frozen = (gridTag.MouseDownRow / gridTag.UnitsPerScroll) * gridTag.UnitsPerScroll;
					//    g8.Rows.Frozen = g7.Rows.Frozen;
					//    g9.Rows.Frozen = g7.Rows.Frozen;

					//    SetVScrollBar3Parameters();
					//    SetScrollBarPosition(vScrollBar3, gridTag.MouseDownRow / gridTag.UnitsPerScroll);

					//    gridTag.HasRowsFrozen = true;
					//}
					//else if (gridTag.GridId == Grid10)
					//{
					//    gridTag.TopMostRowBeforeFreeze = topRow; //this var will be used later when un-freezing.

					//    for (i = 0; i < topRow; i++)
					//    {
					//        g10.Rows[i].Visible = false;
					//        if (((PagingGridTag)g11.Tag).Visible)
					//        {
					//            g11.Rows[i].Visible = false;
					//        }
					//        if (((PagingGridTag)g12.Tag).Visible)
					//        {
					//            g12.Rows[i].Visible = false;
					//        }
					//    }

					//    g10.Rows.Frozen = (gridTag.MouseDownRow / gridTag.UnitsPerScroll) * gridTag.UnitsPerScroll;
					//    g11.Rows.Frozen = g10.Rows.Frozen;
					//    g12.Rows.Frozen = g10.Rows.Frozen;

					//    SetVScrollBar4Parameters();
					//    SetScrollBarPosition(vScrollBar4, gridTag.MouseDownRow / gridTag.UnitsPerScroll);

					//    gridTag.HasRowsFrozen = true;
					//}
					FreezeRows(gridTag, topRow);

					if (gridTag.GridId == Grid4)
					{
						SetVScrollBar2Parameters();
						SetScrollBarPosition(vScrollBar2, gridTag.MouseDownRow / gridTag.UnitsPerScroll);
					}
					else if (gridTag.GridId == Grid7)
					{
						SetVScrollBar3Parameters();
						SetScrollBarPosition(vScrollBar3, gridTag.MouseDownRow / gridTag.UnitsPerScroll);
					}
					else if (gridTag.GridId == Grid10)
					{
						SetVScrollBar4Parameters();
						SetScrollBarPosition(vScrollBar4, gridTag.MouseDownRow / gridTag.UnitsPerScroll);
					}
					//End Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//End Track #5003 - JScott - Freeze Rows
		//Begin Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
		private void UnfreezeRows(PagingGridTag aGridTag)
		{
			int i;

			try
			{
				if (aGridTag.GridId == Grid4)
				{
					for (i = 0; i < aGridTag.TopMostRowBeforeFreeze; i++)
					{
						g4.Rows[i].Visible = true;
						if (((PagingGridTag)g5.Tag).Visible)
						{
							g5.Rows[i].Visible = true;
						}
						if (((PagingGridTag)g6.Tag).Visible)
						{
							g6.Rows[i].Visible = true;
						}
					}

					g4.Rows.Frozen = 0;
					g5.Rows.Frozen = 0;
					g6.Rows.Frozen = 0;

					aGridTag.HasRowsFrozen = false;
				}
				else if (aGridTag.GridId == Grid7)
				{
					for (i = 0; i < aGridTag.TopMostRowBeforeFreeze; i++)
					{
						g7.Rows[i].Visible = true;
						if (((PagingGridTag)g8.Tag).Visible)
						{
							g8.Rows[i].Visible = true;
						}
						if (((PagingGridTag)g9.Tag).Visible)
						{
							g9.Rows[i].Visible = true;
						}
					}

					g7.Rows.Frozen = 0;
					g8.Rows.Frozen = 0;
					g9.Rows.Frozen = 0;

					aGridTag.HasRowsFrozen = false;
				}
				else if (aGridTag.GridId == Grid10)
				{
					for (i = 0; i < aGridTag.TopMostRowBeforeFreeze; i++)
					{
						g10.Rows[i].Visible = true;
						if (((PagingGridTag)g11.Tag).Visible)
						{
							g11.Rows[i].Visible = true;
						}
						if (((PagingGridTag)g12.Tag).Visible)
						{
							g12.Rows[i].Visible = true;
						}
					}

					g10.Rows.Frozen = 0;
					g11.Rows.Frozen = 0;
					g12.Rows.Frozen = 0;

					aGridTag.HasRowsFrozen = false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void FreezeRows(PagingGridTag aGridTag, int aTopRow)
		{
			int i;

			try
			{
				if (aGridTag.GridId == Grid4)
				{
					aGridTag.TopMostRowBeforeFreeze = aTopRow; //this var will be used later when un-freezing.

					for (i = 0; i < aTopRow; i++)
					{
						g4.Rows[i].Visible = false;
						if (((PagingGridTag)g5.Tag).Visible)
						{
							g5.Rows[i].Visible = false;
						}
						if (((PagingGridTag)g6.Tag).Visible)
						{
							g6.Rows[i].Visible = false;
						}
					}

					g4.Rows.Frozen = (aGridTag.MouseDownRow / aGridTag.UnitsPerScroll) * aGridTag.UnitsPerScroll;
					g5.Rows.Frozen = g4.Rows.Frozen;
					g6.Rows.Frozen = g4.Rows.Frozen;

					aGridTag.HasRowsFrozen = true;
				}
				else if (aGridTag.GridId == Grid7)
				{
					aGridTag.TopMostRowBeforeFreeze = aTopRow; //this var will be used later when un-freezing.

					for (i = 0; i < aTopRow; i++)
					{
						g7.Rows[i].Visible = false;
						if (((PagingGridTag)g8.Tag).Visible)
						{
							g8.Rows[i].Visible = false;
						}
						if (((PagingGridTag)g9.Tag).Visible)
						{
							g9.Rows[i].Visible = false;
						}
					}

					g7.Rows.Frozen = (aGridTag.MouseDownRow / aGridTag.UnitsPerScroll) * aGridTag.UnitsPerScroll;
					g8.Rows.Frozen = g7.Rows.Frozen;
					g9.Rows.Frozen = g7.Rows.Frozen;

					aGridTag.HasRowsFrozen = true;
				}
				else if (aGridTag.GridId == Grid10)
				{
					aGridTag.TopMostRowBeforeFreeze = aTopRow; //this var will be used later when un-freezing.

					for (i = 0; i < aTopRow; i++)
					{
						g10.Rows[i].Visible = false;
						if (((PagingGridTag)g11.Tag).Visible)
						{
							g11.Rows[i].Visible = false;
						}
						if (((PagingGridTag)g12.Tag).Visible)
						{
							g12.Rows[i].Visible = false;
						}
					}

					g10.Rows.Frozen = (aGridTag.MouseDownRow / aGridTag.UnitsPerScroll) * aGridTag.UnitsPerScroll;
					g11.Rows.Frozen = g10.Rows.Frozen;
					g12.Rows.Frozen = g10.Rows.Frozen;

					aGridTag.HasRowsFrozen = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
		#endregion

        #region Style-changing codes (colors, fonts, etc)

        private void DefineStyles()
        {
            try
            {
                Setg2Styles(true);
                Setg3Styles(true);
                Setg4Styles(true);
                Setg5Styles(true);
                Setg6Styles(true);
                Setg7Styles(true);
                Setg8Styles(true);
                Setg9Styles(true);
                Setg10Styles(true);
                Setg11Styles(true);
                Setg12Styles(true);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Setg2Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g2.Styles.Add("MerDesc");
					cellStyle.BackColor = _theme.NodeDescriptionBackColor;
					cellStyle.ForeColor = _theme.NodeDescriptionForeColor;
					cellStyle.Font = _theme.NodeDescriptionFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.Margins = new System.Drawing.Printing.Margins(0, 0, 3, 3);
					cellStyle.TextEffect = _theme.NodeDescriptionTextEffects;

					cellStyle = g2.Styles.Add("GroupHeader");
					cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
					cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
					cellStyle.Font = _theme.ColumnGroupHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 3, 3);
					cellStyle.TextEffect = _theme.ColumnGroupHeaderTextEffects;
					cellStyle.WordWrap = true;

					cellStyle = g2.Styles.Add("ColumnHeading");
					cellStyle.BackColor = _theme.ColumnHeaderBackColor;
					cellStyle.ForeColor = _theme.ColumnHeaderForeColor;
					cellStyle.Font = _theme.ColumnHeaderFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
					cellStyle.TextEffect = _theme.ColumnHeaderTextEffects;
					cellStyle.WordWrap = true;
				}

				if (((PagingGridTag)g2.Tag).Visible)
				{
					g2.Rows[0].Style = g2.Styles["MerDesc"];

					//Begin Track #5006 - JScott - Display Low-levels one at a time
					//if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
					if (_currentPlanSessionType == ePlanSessionType.ChainSingleLevel)
					//End Track #5006 - JScott - Display Low-levels one at a time
					{
						g2.Styles["GroupHeader"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
						g2.Rows[1].Style = g2.Styles["GroupHeader"];
					}
					else
					{
						if (((PagingGridTag)g2.Tag).GroupsPerGrid == 1 && ((PagingGridTag)g2.Tag).DetailsPerGroup == 1)
						{
							g2.Styles["GroupHeader"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
							g2.Styles["ColumnHeading"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
						}
						else if (((PagingGridTag)g2.Tag).DetailsPerGroup == 1)
						{
							g2.Styles["GroupHeader"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
							g2.Styles["ColumnHeading"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
						}
						else
						{
							g2.Styles["GroupHeader"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
							g2.Styles["ColumnHeading"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
						}

						g2.Rows[1].Style = g2.Styles["GroupHeader"];
						g2.Rows[2].Style = g2.Styles["ColumnHeading"];
					}
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Setg3Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;
            CellRange cellRange;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g3.Styles.Add("MerDesc");
					cellStyle.BackColor = _theme.NodeDescriptionBackColor;
					cellStyle.ForeColor = _theme.NodeDescriptionForeColor;
					cellStyle.Font = _theme.NodeDescriptionFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.Margins = new System.Drawing.Printing.Margins(0, 0, 3, 3);
					cellStyle.TextEffect = _theme.NodeDescriptionTextEffects;

					cellStyle = g3.Styles.Add("GroupHeader");
					cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
					cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
					cellStyle.Font = _theme.ColumnGroupHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 3, 3);
					cellStyle.TextEffect = _theme.ColumnGroupHeaderTextEffects;
					cellStyle.WordWrap = true;

					cellStyle = g3.Styles.Add("ColumnHeading");
					cellStyle.BackColor = _theme.ColumnHeaderBackColor;
					cellStyle.ForeColor = _theme.ColumnHeaderForeColor;
					cellStyle.Font = _theme.ColumnHeaderFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
					cellStyle.TextEffect = _theme.ColumnHeaderTextEffects;
					cellStyle.WordWrap = true;
				}

				if (((PagingGridTag)g3.Tag).Visible && g3.Cols.Count > 0)
				{
					cellRange = g3.GetCellRange(0, 0, 0, g3.Cols.Count - 1);
					cellRange.Style = g3.Styles["MerDesc"];

					//Begin Track #5006 - JScott - Display Low-levels one at a time
					//if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
					if (_currentPlanSessionType == ePlanSessionType.ChainSingleLevel)
					//End Track #5006 - JScott - Display Low-levels one at a time
					{
						g3.Styles["GroupHeader"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
						cellRange = g3.GetCellRange(1, 0, 1, g3.Cols.Count - 1);
						cellRange.Style = g3.Styles["GroupHeader"];
					}
					else
					{
						if (((PagingGridTag)g3.Tag).GroupsPerGrid == 1 && ((PagingGridTag)g3.Tag).DetailsPerGroup == 1)
						{
							g3.Styles["GroupHeader"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
							g3.Styles["ColumnHeading"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
						}
						else if (((PagingGridTag)g3.Tag).DetailsPerGroup == 1)
						{
							g3.Styles["GroupHeader"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
							g3.Styles["ColumnHeading"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
						}
						else
						{
							g3.Styles["GroupHeader"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
							g3.Styles["ColumnHeading"].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
						}

						cellRange = g3.GetCellRange(1, 0, 1, g3.Cols.Count - 1);
						cellRange.Style = g3.Styles["GroupHeader"];
						cellRange = g3.GetCellRange(2, 0, 2, g3.Cols.Count - 1);
						cellRange.Style = g3.Styles["ColumnHeading"];
					}
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Setg4Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;
            int i;
            CellRange cellRange;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g4.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderBackColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
		
					cellStyle = g4.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
				}
			
				if (((PagingGridTag)g4.Tag).Visible)
				{
					ChangeRowStyles(g4);

					for (i = 0; i < g4.Rows.Count; i++)
					{
						cellRange = g4.GetCellRange(i, 0, i, g4.Cols.Count - 1);
						cellRange.Style = g4.Rows[i].Style;
					}
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Setg5Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g5.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Ineligible1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Locked1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Ineligible2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Locked2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("TimeTotalHeading1");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderBackColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
		
					cellStyle = g5.Styles.Add("TimeTotalHeading2");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
				}

				if (((PagingGridTag)g5.Tag).Visible)
				{
					ChangeRowStyles(g5);
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Setg6Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g6.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Ineligible1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Locked1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Ineligible2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Locked2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
				}

				if (((PagingGridTag)g6.Tag).Visible)
				{
					ChangeRowStyles(g6);
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Setg7Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;
            int i;
            CellRange cellRange;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g7.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreSetRowHeaderBackColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
			
					cellStyle = g7.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreSetRowHeaderAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
			
					cellStyle = g7.Styles.Add("Reverse1");
					cellStyle.BackColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
			
					cellStyle = g7.Styles.Add("Reverse2");
					cellStyle.BackColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderAlternateBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
				}

				if (((PagingGridTag)g7.Tag).Visible)
				{
					ChangeRowStyles(g7);

					for (i = 0; i < g7.Rows.Count; i++)
					{
						cellRange = g7.GetCellRange(i, 0, i, g7.Cols.Count - 1);
						if (((RowHeaderTag)g7.Rows[i].UserData).GroupRowColHeader.Profile.Key == (int)cboAttributeSet.SelectedValue)
						{
							switch (g7.Rows[i].Style.Name)
							{
								case "Style1":
									cellRange.Style = g7.Styles["Reverse1"];
									break;
								case "Style2":
									cellRange.Style = g7.Styles["Reverse2"];
									break;
								default:
									throw new Exception("Invalid row style");
							}
						}
						else
						{
							cellRange.Style = g7.Rows[i].Style;
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

        private void Setg8Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g8.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Ineligible1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Locked1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Ineligible2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Locked2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
				}

				if (((PagingGridTag)g8.Tag).Visible)
				{
					ChangeRowStyles(g8);
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Setg9Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g9.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Ineligible1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Locked1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Ineligible2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Locked2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
				}

				if (((PagingGridTag)g9.Tag).Visible)
				{
					ChangeRowStyles(g9);
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Setg10Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;
            int i;
            CellRange cellRange;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g10.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreTotalRowHeaderBackColor;
					cellStyle.ForeColor = _theme.StoreTotalRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;

					cellStyle = g10.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreTotalRowHeaderAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
				}

				if (((PagingGridTag)g10.Tag).Visible)
				{
					ChangeRowStyles(g10);

					for (i = 0; i < g10.Rows.Count; i++)
					{
						cellRange = g10.GetCellRange(i, 0, i, g10.Cols.Count - 1);
						cellRange.Style = g10.Rows[i].Style;
					}
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Setg11Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g11.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Ineligible1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Locked1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Ineligible2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Locked2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
				}

				if (((PagingGridTag)g11.Tag).Visible)
				{
					ChangeRowStyles(g11);
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void Setg12Styles(bool aDefineStylesToGrid)
        {
            CellStyle cellStyle;

            try
            {
				if (aDefineStylesToGrid)
				{
					cellStyle = g12.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Ineligible1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Locked1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Ineligible2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.IneligibleStoreFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Locked2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.LockedFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
				}

				if (((PagingGridTag)g12.Tag).Visible)
				{
					ChangeRowStyles(g12);
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ChangeRowStyles(C1FlexGrid aGrid)
        {
            PagingGridTag gridTag;
            int rowsPerGroup;
            int i;
            int j;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;
                rowsPerGroup = ((PagingGridTag)gridTag.RowHeaderGrid.Tag).DetailsPerGroup;

                if (aGrid.Rows.Count > 0 && aGrid.Cols.Count > 0)
                {
                    if (_theme.ViewStyle == StyleEnum.Plain || _theme.ViewStyle == StyleEnum.Chiseled)
                    {
                        for (i = 0; i < aGrid.Rows.Count; i++)
                        {
                            aGrid.Rows[i].Style = aGrid.Styles["Style1"];
                        }
                    }
                    else if (_theme.ViewStyle == StyleEnum.AlterColors)
                    {
                        for (i = 0; i < aGrid.Rows.Count; i++)
                        {
                            if (i % (rowsPerGroup * 2) < rowsPerGroup)
                            {
                                aGrid.Rows[i].Style = aGrid.Styles["Style1"];
                            }
                            else
                            {
                                aGrid.Rows[i].Style = aGrid.Styles["Style2"];
                            }
                        }
                    }
                    else if (_theme.ViewStyle == StyleEnum.HighlightName)
                    {
                        for (i = 0; i < aGrid.Rows.Count; i++)
                        {
                            if (i % rowsPerGroup == 0)
                            {
                                aGrid.Rows[i].Style = aGrid.Styles["Style1"];
                            }
                            else
                            {
                                aGrid.Rows[i].Style = aGrid.Styles["Style2"];
                            }
                        }
                    }
                }

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (gridTag.GridId == Grid5 && _openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                if (gridTag.GridId == Grid5 && _currentPlanSessionType == ePlanSessionType.ChainSingleLevel)
				//End Track #5006 - JScott - Display Low-levels one at a time
                {
                    if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
                    {
                        if (_theme.ViewStyle == StyleEnum.Plain || _theme.ViewStyle == StyleEnum.Chiseled)
                        {
                            for (i = 0; i < g5.Rows.Count; i += rowsPerGroup)
                            {
                                g5.Rows[i].Style = g5.Styles["TimeTotalHeading1"];
                            }
                        }
                        else if (_theme.ViewStyle == StyleEnum.AlterColors)
                        {
                            for (i = 0; i < g5.Rows.Count; i += rowsPerGroup)
                            {
                                if (i % (rowsPerGroup * 2) < rowsPerGroup)
                                {
                                    g5.Rows[i].Style = g5.Styles["TimeTotalHeading1"];
                                }
                                else
                                {
                                    g5.Rows[i].Style = g5.Styles["TimeTotalHeading2"];
                                }
                            }
                        }
                        else if (_theme.ViewStyle == StyleEnum.HighlightName)
                        {
                            for (i = 0; i < g5.Rows.Count; i += rowsPerGroup)
                            {
                                if (i % rowsPerGroup == 0)
                                {
                                    g5.Rows[i].Style = g5.Styles["TimeTotalHeading1"];
                                }
                                else
                                {
                                    g5.Rows[i].Style = g5.Styles["TimeTotalHeading2"];
                                }
                            }
                        }
                    }
                }

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

		//Begin Track #5006 - JScott - Display Low-levels one at a time
		//private void btnThemeProperties_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        _frmThemeProperties = new ThemeProperties(_theme);
		//        _frmThemeProperties.ApplyButtonClicked += new EventHandler(StylePropertiesOnChanged);
		//        _frmThemeProperties.StartPosition = FormStartPosition.CenterParent;

		//        if (_frmThemeProperties.ShowDialog() == DialogResult.OK)
		//        {
		//            StylePropertiesChanged();
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//}

		//End Track #5006 - JScott - Display Low-levels one at a time
		private void btnApply_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                StopPageLoadThreads();
                RecomputePlanCubes();
                LoadCurrentPages();
                LoadSurroundingPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// This procedure handles the "Apply" button click event from the StyleProperties form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void StylePropertiesOnChanged(object sender, System.EventArgs e)
        {
            try
            {
                StylePropertiesChanged();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void StylePropertiesChanged()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);

                _sab.ClientServerSession.Theme = _frmThemeProperties.CurrentTheme;

                StopPageLoadThreads();
                DefineStyles();
                ResizeRow4();
                ResizeRow7();
                ResizeRow10();
                ResizeCol1();
                ResizeCol2();
                ResizeCol3();
                ResizeRow1();
                CalcColSplitPosition2(false);
                CalcColSplitPosition3(false);
                SetColSplitPositions();
                CalcRowSplitPosition4(false);
                CalcRowSplitPosition8(false);
                CalcRowSplitPosition12(false);
                SetRowSplitPositions();
                SetHScrollBar2Parameters();
                SetHScrollBar3Parameters();
                SetVScrollBar2Parameters();
                SetVScrollBar3Parameters();
                SetVScrollBar4Parameters();
                HighlightActiveAttributeSet();
                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
                g6.Focus();
            }
        }

        private void g3_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            try
            {
                // added to get around Component One problem
				//Begin Issue - JScott - Merge 3.0 problem with FlexGrid
				//if (!((C1FlexGrid)sender).Redraw)
				if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
				//End Issue - JScott - Merge 3.0 problem with FlexGrid
                {
                    return;
                }

                if (e.Row == 2)
                {
                    e.DrawCell();
                    DrawColBorders((C1FlexGrid)sender, e);
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g4_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            try
            {
                // added to get around Component One problem
				//Begin Issue - JScott - Merge 3.0 problem with FlexGrid
				//if (!((C1FlexGrid)sender).Redraw)
				if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
				//End Issue - JScott - Merge 3.0 problem with FlexGrid
                {
                    return;
                }

                e.DrawCell();
                DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupRowHeaderDividerBrush, e);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g5_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            try
            {
                // added to get around Component One problem
				//Begin Issue - JScott - Merge 3.0 problem with FlexGrid
				//if (!((C1FlexGrid)sender).Redraw)
				if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
				//End Issue - JScott - Merge 3.0 problem with FlexGrid
                {
                    return;
                }

                e.DrawCell();
                DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupDividerBrush, e);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g6_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            try
            {
                // added to get around Component One problem
				//Begin Issue - JScott - Merge 3.0 problem with FlexGrid
				//if (!((C1FlexGrid)sender).Redraw)
				if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
				//End Issue - JScott - Merge 3.0 problem with FlexGrid
                {
                    return;
                }

                e.DrawCell();
                DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupDividerBrush, e);
                DrawColBorders((C1FlexGrid)sender, e);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g7_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            try
            {
                // added to get around Component One problem
				//Begin Issue - JScott - Merge 3.0 problem with FlexGrid
				//if (!((C1FlexGrid)sender).Redraw)
				if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
				//End Issue - JScott - Merge 3.0 problem with FlexGrid
                {
                    return;
                }

                e.DrawCell();
                DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupRowHeaderDividerBrush, e);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g8_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            try
            {
                // added to get around Component One problem
				//Begin Issue - JScott - Merge 3.0 problem with FlexGrid
				//if (!((C1FlexGrid)sender).Redraw)
				if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
				//End Issue - JScott - Merge 3.0 problem with FlexGrid
                {
                    return;
                }

                e.DrawCell();
                DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupDividerBrush, e);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g9_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            try
            {
                // added to get around Component One problem
				//Begin Issue - JScott - Merge 3.0 problem with FlexGrid
				//if (!((C1FlexGrid)sender).Redraw)
				if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
				//End Issue - JScott - Merge 3.0 problem with FlexGrid
                {
                    return;
                }

                e.DrawCell();
                DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupDividerBrush, e);
                DrawColBorders((C1FlexGrid)sender, e);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g10_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            try
            {
                // added to get around Component One problem
				//Begin Issue - JScott - Merge 3.0 problem with FlexGrid
				//if (!((C1FlexGrid)sender).Redraw)
				if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
				//End Issue - JScott - Merge 3.0 problem with FlexGrid
                {
                    return;
                }

                e.DrawCell();
                DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupRowHeaderDividerBrush, e);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g11_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            try
            {
                // added to get around Component One problem
				//Begin Issue - JScott - Merge 3.0 problem with FlexGrid
				//if (!((C1FlexGrid)sender).Redraw)
				if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
				//End Issue - JScott - Merge 3.0 problem with FlexGrid
                {
                    return;
                }

                e.DrawCell();
                DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupDividerBrush, e);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g12_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            try
            {
                // added to get around Component One problem
				//Begin Issue - JScott - Merge 3.0 problem with FlexGrid
				//if (!((C1FlexGrid)sender).Redraw)
				if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
				//End Issue - JScott - Merge 3.0 problem with FlexGrid
                {
                    return;
                }

                e.DrawCell();
                DrawRowBorders((C1FlexGrid)sender, _theme.RowGroupDividerBrush, e);
                DrawColBorders((C1FlexGrid)sender, e);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void DrawRowBorders(
            C1FlexGrid aGrid,
            SolidBrush aRowBrush,
            OwnerDrawCellEventArgs e)
        {
            C1FlexGrid rowHeaderGrid;
            RowHeaderTag rowHeaderTag;
            Rectangle rectangle;
            Graphics graphics;
            System.Drawing.Printing.Margins margins;
			int rowsPerGroup;

            try
            {
                rowHeaderGrid = ((PagingGridTag)aGrid.Tag).RowHeaderGrid;

                if (e.Row != aGrid.Rows.Count - 1 && rowHeaderGrid != null)
                {
                    rowHeaderTag = (RowHeaderTag)rowHeaderGrid.Rows[e.Row].UserData;

                    if (rowHeaderTag.DrawBorder)
                    {
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

							rowsPerGroup = ((PagingGridTag)rowHeaderGrid.Tag).DetailsPerGroup;

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
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void DrawColBorders(C1FlexGrid aGrid, OwnerDrawCellEventArgs e)
        {
            Rectangle rectangle;
            Graphics graphics;
            System.Drawing.Printing.Margins margins;

            try
            {
                if (e.Col < aGrid.Cols.Count - 1 && (e.Col + 1) % ((PagingGridTag)g3.Tag).DetailsPerGroup == 0 && _theme.DisplayColumnGroupDivider)
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

		override protected void utmMain_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
            try
            {
                switch (e.Tool.Key)
                {
                    case "btUndo":
                        Undo();
                        break;

					//Begin Track #5006 - JScott - Display Low-levels one at a time
					case "btTheme":
						Theme();
						break;

					//End Track #5006 - JScott - Display Low-levels one at a time
					case Include.btFind:
                        Find();
                        break;

                    case Include.btExport:
                        Export();
                        break;

					default:
						base.utmMain_ToolClick(sender, e);
						break;
				}
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void Undo()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                StopPageLoadThreads();
                _planCubeGroup.UndoLastRecompute();
                LoadCurrentPages();
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

		//Begin Track #5006 - JScott - Display Low-levels one at a time
		private void Theme()
		{
			try
			{
				_frmThemeProperties = new ThemeProperties(_theme);
				_frmThemeProperties.ApplyButtonClicked += new EventHandler(StylePropertiesOnChanged);
				_frmThemeProperties.StartPosition = FormStartPosition.CenterParent;

				if (_frmThemeProperties.ShowDialog() == DialogResult.OK)
				{
					StylePropertiesChanged();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//End Track #5006 - JScott - Display Low-levels one at a time
		private void Find()
        {
            QuickFilter quickFilter;
            DialogResult diagResult;
            StoreProfile storeProf;
            ProfileXRef storeSetXRef;
            ArrayList totalList;
            int storeIdx;
            int selectRow;
            int selectCol;

            try
            {
				// BEGIN TT#2703 - stodd - select first comboBox on QuickFilter
                quickFilter = new QuickFilter(eQuickFilterType.Find, 3, true, "Store:", "Time:", "Variable:");
				// END TT#2703 - stodd - select first comboBox on QuickFilter

                quickFilter.EnableComboBox(0);
                quickFilter.EnableComboBox(1);
                quickFilter.EnableComboBox(2);

                //BEGIN TT#3984-MD-VStuart-Object Reference error 
                if (_storeProfileList != null) //If there are no stores to find skip populating the combobox.
                {
                    quickFilter.LoadComboBox(0, _storeProfileList.ArrayList);
                    //BEGIN TT#6-MD-VStuart - Single Store Select
                    quickFilter.LoadComboBoxAutoFill(0, _storeProfileList.ArrayList);
                    //END TT#6-MD-VStuart - Single Store Select
                }
                else
                {
                    quickFilter.DisableComboBox(0);
                }
                //END TT#3984-MD-VStuart-Object Reference error 

                quickFilter.LoadComboBox(1, _sortedTimeHeaders);
                quickFilter.LoadComboBox(2, _sortedVariableHeaders);

                diagResult = quickFilter.ShowDialog(this);

                if (diagResult == DialogResult.OK)
                {
                    selectRow = 0;
                    selectCol = 0;

                    if (quickFilter.GetSelectedIndex(0) >= 0)
                    {
                        storeProf = (StoreProfile)quickFilter.GetSelectedItem(0);
                        storeSetXRef = (ProfileXRef)_planCubeGroup.GetProfileXRef(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
                        totalList = storeSetXRef.GetTotalList(storeProf.Key);
                        cboAttributeSet.SelectedValue = totalList[0];
                        storeIdx = _workingDetailProfileList.ArrayList.IndexOf(storeProf);
						//Begin TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
						//vScrollBar2.Value = System.Math.Min(storeIdx, vScrollBar2.Maximum - vScrollBar2.LargeChange + 1);
						vScrollBar2.Value = Math.Max(Math.Min(storeIdx, vScrollBar2.Maximum - vScrollBar2.LargeChange + 1), vScrollBar2.Minimum);
						//End TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
						ChangeVScrollBar2Value(vScrollBar2.Value, true);

                        selectRow = ((PagingGridTag)g4.Tag).DetailsPerGroup * storeIdx;
                    }

                    if (quickFilter.GetSelectedIndex(1) >= 0)
                    {
                        if (_columnGroupedBy == GroupedBy.GroupedByTime)
                        {
                            selectCol += ((PagingGridTag)g3.Tag).DetailsPerGroup * ((RowColProfileHeader)quickFilter.GetSelectedItem(1)).Sequence;
                        }
                        else
                        {
                            selectCol += ((RowColProfileHeader)quickFilter.GetSelectedItem(1)).Sequence;
                        }
                    }

                    if (quickFilter.GetSelectedIndex(2) >= 0)
                    {
                        if (_columnGroupedBy == GroupedBy.GroupedByTime)
                        {
                            selectCol += ((RowColProfileHeader)quickFilter.GetSelectedItem(2)).Sequence;
                        }
                        else
                        {
                            selectCol += ((PagingGridTag)g3.Tag).DetailsPerGroup * ((RowColProfileHeader)quickFilter.GetSelectedItem(2)).Sequence;
                        }
                    }

                    g6.Select(selectRow, selectCol);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		private void Export()
		{
			MIDExportFile MIDExportFile = null;
			try
			{
				MIDExport MIDExport = null;
				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel ||
				//    _openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
				if (_currentPlanSessionType == ePlanSessionType.StoreSingleLevel ||
					_currentPlanSessionType == ePlanSessionType.StoreMultiLevel)
				//End Track #5006 - JScott - Display Low-levels one at a time
				{
					MIDExport = new MIDExport(SAB, _includeCurrentSetLabel, _includeAllSetsLabel, true);
				}
				else
				{
					MIDExport = new MIDExport(SAB, null, null, false);
				}

				MIDExport.AddFileFilter(eExportFilterType.Excel);
				MIDExport.AddFileFilter(eExportFilterType.CSV);
				MIDExport.AddFileFilter(eExportFilterType.XML);
				MIDExport.AddFileFilter(eExportFilterType.All);

				MIDExport.ShowDialog();
				if (!MIDExport.OKClicked)
				{
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				SetGridRedraws(false);
				StopPageLoadThreads();
				string fileName = null;
				if (MIDExport.OpenExcel)
				{
					// generate unique name
					fileName = Application.LocalUserAppDataPath + @"\mid" + DateTime.Now.Ticks.ToString();
					if (MIDExport.IncludeFormatting)
					{
						fileName += ".xls";
					}
					else
					{
						fileName += ".xml";
					}
				}
				else
				{
					fileName = MIDExport.FileName;
				}

				switch (MIDExport.ExportType)
				{
					case eExportType.Excel:
						if (MIDExport.IncludeFormatting)
						{
							MIDExportFile = new MIDExportFlexGridToExcel(fileName, MIDExport.IncludeFormatting);
						}
						else
						{
							MIDExportFile = new MIDExportFlexGridToXML(fileName, MIDExport.IncludeFormatting);
						}
						break;
					case eExportType.XML:
						MIDExportFile = new MIDExportFlexGridToXML(fileName, MIDExport.IncludeFormatting);
						break;
					default:
						MIDExportFile = new MIDExportFlexGridToCSV(fileName, MIDExport.IncludeFormatting);
						break;
				}

				if (MIDExportFile != null)
				{
					// delete file if it's already there
					if (File.Exists(MIDExportFile.FileName))
					{
						File.Delete(MIDExportFile.FileName);
					}
					MIDExportFile.OpenFile();
					// add styles to XML style sheet
					if (MIDExportFile.ExportType == eExportType.XML)
					{
						ExportSpecificStyle(MIDExportFile, "g2", g2, "ColumnHeading");
						ExportSpecificStyle(MIDExportFile, "g3", g3, "ColumnHeading");
						ExportSpecificStyle(MIDExportFile, "g4", g4, "Style1");
						ExportSpecificStyle(MIDExportFile, "g7", g7, "Reverse1");
						ExportSpecificStyle(MIDExportFile, "g10", g10, "Style1");
						MIDExportFile.NoMoreStyles();
					}
					if (MIDExport.ExportData == eExportData.Current)
					{
						ExportCurrentFile(MIDExportFile);
					}
					else
					{
						ExportAllFile(MIDExportFile);
					}
					if (MIDExport.OpenExcel)
					{
						Process process = new Process();
						process.StartInfo.FileName = "Excel.exe";
						process.StartInfo.Arguments = @"""" + MIDExportFile.FileName + @"""";
						process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
						process.Start();
					}
				}
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
				if (MIDExportFile != null)
				{
					MIDExportFile.WriteFile();
				}
				SetGridRedraws(true);
				Cursor.Current = Cursors.Default;
			}
		}

		private void ExportCurrentFile(MIDExportFile aMIDExportFile)
		{
			try
			{
				int detailsPerGroup = 0;
				SetGridRedraws(false);

				LoadAllPages();

				StoreGroupLevelProfile sglProf = null;
				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel ||
				//    _openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
				if (_currentPlanSessionType == ePlanSessionType.StoreSingleLevel ||
					_currentPlanSessionType == ePlanSessionType.StoreMultiLevel)
				//End Track #5006 - JScott - Display Low-levels one at a time
				{
					sglProf = (StoreGroupLevelProfile)_storeGroupLevelProfileList.FindKey(_lastAttributeSetValue);
					detailsPerGroup = ((PagingGridTag)g7.Tag).DetailsPerGroup;
				}
				//Begin Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
				//ExportData(aMIDExportFile, sglProf, detailsPerGroup, true, true, false, _workingDetailProfileList);
				ExportData(aMIDExportFile, _workingDetailProfileList, sglProf, detailsPerGroup, true, true, false);
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
				SetGridRedraws(false);

				LoadAllPages();

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
					BuildWorkingStoreList(sglProf.Key, workList);

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

					ExportData(aMIDExportFile, exportProfileList, sglProf, ((PagingGridTag)g7.Tag).DetailsPerGroup,
					//End Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
						addHeader, addSummary, true);
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
		private void ExportData(MIDExportFile aMIDExportFile, ProfileList aDetailProfileList, StoreGroupLevelProfile sglProf, int aRowsPerGroup,
		//End Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
			bool aAddHeader, bool aAddSummary, bool aExportingAll)
		{
			try
			{
				string negativeStyle = null;
				string textStyle = null;
				int i;
				int j;
				int k;
				int totCols;
				string groupingName = "sheet1";
				PlanWaferCell[,] totalPlanWaferCellTable = null;
				CubeWafer totalCubeWafer;
				PlanWaferCell[,] detailPlanWaferCellTable = null;
				CubeWafer detailCubeWafer;
				object[,] setvalueArray;
				ExcelGridInfo[,] setformatArray;
				C1FlexGrid exportG4 = new C1FlexGrid();
                VariableProfile varProf;
                int currVarProfKey = Include.Undefined;

				//Begin Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
				//ProfileList exportProfileList = null;
				//End Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
				if (sglProf != null) // store
				{
					//Begin Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
					//ProfileList workList = new ProfileList(eProfileType.Store);
					//BuildWorkingStoreList(sglProf.Key, workList);
					//// sort the stores
					//SortedList stores = new SortedList();
					//ArrayList sortedStores = new ArrayList();
					//foreach (StoreProfile storeProfile in workList)
					//{
					//    stores.Add(storeProfile.Text, storeProfile);
					//}
					//exportProfileList = new ProfileList(eProfileType.Store);
					//foreach (StoreProfile storeProfile in stores.Values)
					//{
					//    exportProfileList.Add(storeProfile);
					//}
					//Formatg4Grid(true, exportG4, exportProfileList, false);
					Formatg4Grid(true, exportG4, aDetailProfileList, false);
					//End Track #5674 - JScott - Export to Excel - Rank Stores within View, but export out in numerical Order
					groupingName = sglProf.Name;
				}
				else // chain
				{
					exportG4 = g4;
				}

				int rowsCount = g3.Rows.Count + exportG4.Rows.Count + ((PagingGridTag)g7.Tag).DetailsPerGroup + g10.Rows.Count;
				int columnsCount = exportG4.Cols.Count + g2.Cols.Count + g3.Cols.Count;

				totCols = exportG4.Cols.Count + g2.Cols.Count + g3.Cols.Count;

				setvalueArray = new object[aRowsPerGroup, totCols];
				setformatArray = new ExcelGridInfo[aRowsPerGroup, totCols];

				aMIDExportFile.AddGrouping(groupingName, totCols);

				aMIDExportFile.SetNumberRowsColumns(rowsCount, columnsCount);

				if (aMIDExportFile.IncludeFormatting &&
					aMIDExportFile.ExportType == eExportType.Excel)
				{
					ExportApplyFormatting(aMIDExportFile, rowsCount, columnsCount, exportG4);
				}

				if (aAddHeader)
				{
					// add headings
					for (i = 0; i < g3.Rows.Count; i++)
					{
						aMIDExportFile.AddRow();
						// insert blank columns to line up values
						negativeStyle = null;
						textStyle = "g2ColumnHeading";
						for (j = 0; j < g4.Cols.Count; j++)
						{
							aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
						}
						for (j = 0; j < g2.Cols.Count; j++)
						{
							if (g2[i, j] != null)
							{
								aMIDExportFile.AddValue(g2[i, j].ToString(), eExportDataType.ColumnHeading, textStyle, null);
							}
							else
							{
								aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
							}
						}
						negativeStyle = null;
						textStyle = "g3ColumnHeading";
						for (j = 0; j < g3.Cols.Count; j++)
						{
							if (g3[i, j] != null)
							{
								aMIDExportFile.AddValue(g3[i, j].ToString(), eExportDataType.ColumnHeading, textStyle, negativeStyle);
							}
							else
							{
								aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, negativeStyle);
							}
						}
						aMIDExportFile.WriteRow();
					}
				}

				// add stores for set or chain values
				if (exportG4.Rows.Count > 0)
				{
					//Create the CubeWafers to request data
					totalCubeWafer = new CubeWafer();
					totalPlanWaferCellTable = ExportGetValues(totalCubeWafer, g5, exportG4);

					//Create the CubeWafers to request data
					detailCubeWafer = new CubeWafer();
					detailPlanWaferCellTable = ExportGetValues(detailCubeWafer, g6, exportG4);


					for (i = 0; i < exportG4.Rows.Count; i++)
					{
						aMIDExportFile.AddRow();
						// add row headings
						negativeStyle = null;
						textStyle = "g4Style1";
						for (j = 0; j < exportG4.Cols.Count; j++)
						{
							aMIDExportFile.AddValue(exportG4[i, j].ToString(), eExportDataType.RowHeading, textStyle, negativeStyle);
						}
						// add total variable names for chain single level
                        varProf = (VariableProfile)((RowColProfileHeader)((RowHeaderTag)exportG4.Rows[i].UserData).GroupRowColHeader).Profile;
						//Begin Track #5006 - JScott - Display Low-levels one at a time
						//if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel &&
                        if (_currentPlanSessionType == ePlanSessionType.ChainSingleLevel &&
						//End Track #5006 - JScott - Display Low-levels one at a time
							//Begin Track #5666 - JScott - Export to Excel when Formatting checked receive and error.
							g2.Rows.Count > 0 &&
							//End Track #5666 - JScott - Export to Excel when Formatting checked receive and error.
							varProf.Key != currVarProfKey)
                        {
                            for (j = 0; j < varProf.TimeTotalChainVariables.Count; j++)
                            {
                                aMIDExportFile.AddValue(((TimeTotalVariableProfile)varProf.GetChainTimeTotalVariable(j + 1)).VariableName, false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
                            }
                            currVarProfKey = varProf.Key;
                        }
                        // add total columns
                        else if (totalPlanWaferCellTable != null)
                        {
                            negativeStyle = "g5Negative1";
                            textStyle = "g5Style1";
                            for (j = 0; j < totalPlanWaferCellTable.GetLength(1); j++)
                            {
                                if (totalPlanWaferCellTable[i, j] != null)
                                {
                                    aMIDExportFile.AddValue(totalPlanWaferCellTable[i, j].ValueAsString, totalPlanWaferCellTable[i, j].isValueNumeric,
                                        totalPlanWaferCellTable[i, j].isValueNegative, totalPlanWaferCellTable[i, j].NumberOfDecimals,
                                        eExportDataType.Value, textStyle, negativeStyle);
                                }
                                else
                                {
                                    aMIDExportFile.AddValue(" ", false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
                                }
                            }
                        }
						// add detail columns
						negativeStyle = "g6Negative1";
						textStyle = "g6Style1";
						for (j = 0; j < detailPlanWaferCellTable.GetLength(1); j++)
						{
							if (detailPlanWaferCellTable[i, j] != null)
							{
								aMIDExportFile.AddValue(detailPlanWaferCellTable[i, j].ValueAsString, detailPlanWaferCellTable[i, j].isValueNumeric,
									detailPlanWaferCellTable[i, j].isValueNegative, detailPlanWaferCellTable[i, j].NumberOfDecimals, 
									eExportDataType.Value, textStyle, negativeStyle);
							}
							else
							{
								aMIDExportFile.AddValue(" ", false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
							}
						}
						aMIDExportFile.WriteRow();
					}
				}

				// add set values
				for (i = 0; i < g7.Rows.Count; i++)
				{
					if (((RowHeaderTag)g7.Rows[i].UserData).GroupRowColHeader.Profile.Key == sglProf.Key)
					{
						int row = i;
						for (k = 0; k < ((PagingGridTag)g7.Tag).DetailsPerGroup; k++)
						{
							aMIDExportFile.AddRow();
							// add row headings
							negativeStyle = null;
							textStyle = "g7Reverse1";
							for (j = 0; j < g7.Cols.Count; j++)
							{
								if (g7[row, j] != null)
								{
									aMIDExportFile.AddValue(g7[row, j].ToString(), eExportDataType.RowHeading, textStyle, negativeStyle);
								}
								else
								{
									aMIDExportFile.AddValue(" ", eExportDataType.RowHeading, textStyle, negativeStyle);
								}
							}
							// add total columns
							negativeStyle = "g8Negative1";
							textStyle = "g8Editable1";
							for (j = 0; j < g8.Cols.Count; j++)
							{
								if (g8[row, j] != null)
								{
									aMIDExportFile.AddValue(g8[row, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
								}
								else
								{
									aMIDExportFile.AddValue(" ", false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
								}
							}
							// add detail columns
							negativeStyle = "g9Negative1";
							textStyle = "g9Editable1";
							for (j = 0; j < g9.Cols.Count; j++)
							{
								if (g9[row, j] != null)
								{
									aMIDExportFile.AddValue(g9[row, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
								}
								else
								{
									aMIDExportFile.AddValue(" ", false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
								}
							}
							aMIDExportFile.WriteRow();
							++row;
						}
						break;
					}
				}

				if (aAddSummary)
				{
					// add blank line before summary
					if (aMIDExportFile.ExportType == eExportType.CSV &&
						aExportingAll)
					{
						aMIDExportFile.AddRow();
						aMIDExportFile.WriteRow();
					}

					// add all stores or chain summary
					for (i = 0; i < g10.Rows.Count; i++)
					{
						aMIDExportFile.AddRow();
						// add row headings
						negativeStyle = null;
						textStyle = "g10Style1";
						for (j = 0; j < g10.Cols.Count; j++)
						{
							if (g10[i, j] != null)
							{
								aMIDExportFile.AddValue(g10[i, j].ToString(), eExportDataType.RowHeading, textStyle, negativeStyle);
							}
							else
							{
								aMIDExportFile.AddValue(" ", eExportDataType.RowHeading, textStyle, negativeStyle);
							}
						}
						// add total columns
						negativeStyle = "g11Negative1";
						textStyle = "g11Editable1";
						for (j = 0; j < g11.Cols.Count; j++)
						{
							if (g11[i, j] != null)
							{
								aMIDExportFile.AddValue(g11[i, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
							}
							else
							{
								aMIDExportFile.AddValue(" ", eExportDataType.Value, textStyle, negativeStyle);
							}
						}
						// add detail columns
						negativeStyle = "g12Negative1";
						textStyle = "g12Editable1";
						for (j = 0; j < g12.Cols.Count; j++)
						{
							if (g12[i, j] != null)
							{
								aMIDExportFile.AddValue(g12[i, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
							}
							else
							{
								aMIDExportFile.AddValue(" ", eExportDataType.Value, textStyle, negativeStyle);
							}
						}
						aMIDExportFile.WriteRow();
					}
				}

				aMIDExportFile.WriteGrouping();
			}
			catch
			{
				throw;
			}
		}

		private PlanWaferCell[,] ExportGetValues(CubeWafer aCubeWafer, C1FlexGrid aGrid, C1FlexGrid aRowHdrGrid)
		{
			try
			{
				PagingGridTag gridTag;
				C1FlexGrid rowHdrGrid;
				C1FlexGrid colHdrGrid;
				int i;
				PlanWaferCell[,] planWaferCellTable = null;
				ColumnHeaderTag ColTag;
				RowHeaderTag RowTag;

				if (aRowHdrGrid.Rows.Count > 0)
				{
					//Fill CommonWaferCoordinateListGroup
					aCubeWafer.CommonWaferCoordinateList = _commonWaferCoordinateList;
					// get grid tags
					gridTag = (PagingGridTag)aGrid.Tag;
					// get row and column grid tags
					rowHdrGrid = aRowHdrGrid;
					colHdrGrid = gridTag.ColHeaderGrid;
					//Fill ColWaferCoordinateListGroup
					aCubeWafer.ColWaferCoordinateListGroup.Clear();
					if (colHdrGrid != null &&
						aGrid.Cols.Count > 0)
					{
						for (i = 0; i < aGrid.Cols.Count; i++)
						{
							ColTag = (ColumnHeaderTag)colHdrGrid.Cols[i].UserData;
							if (ColTag != null)
							{
								aCubeWafer.ColWaferCoordinateListGroup.Add(ColTag.CubeWaferCoorList);
							}
						}
					}

					//Fill RowWaferCoordinateListGroup
					aCubeWafer.RowWaferCoordinateListGroup.Clear();
					if (rowHdrGrid != null &&
						aRowHdrGrid.Rows.Count > 0)
					{
						for (i = 0; i < aRowHdrGrid.Rows.Count; i++)
						{
							RowTag = (RowHeaderTag)rowHdrGrid.Rows[i].UserData;
							if (RowTag != null)
							{
								aCubeWafer.RowWaferCoordinateListGroup.Add(RowTag.CubeWaferCoorList);
							}
						}
					}

					if (aCubeWafer.ColWaferCoordinateListGroup.Count > 0 && aCubeWafer.RowWaferCoordinateListGroup.Count > 0)
					{
						// Retreive array of values

						//Begin Modification - JScott - Add Scaling Decimals
						//planWaferCellTable = _planCubeGroup.GetPlanWaferCellValues(aCubeWafer, ((NumericComboObject)cboUnitScaling.SelectedItem).Value, ((NumericComboObject)cboDollarScaling.SelectedItem).Value);
						planWaferCellTable = _planCubeGroup.GetPlanWaferCellValues(aCubeWafer, ((ComboObject)cboUnitScaling.SelectedItem).Value, ((ComboObject)cboDollarScaling.SelectedItem).Value);
						//End Modification - JScott - Add Scaling Decimals
					}
				}
				return planWaferCellTable;
			}
			catch
			{
				throw;
			}
		}

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

		private void ExportApplyFormatting(MIDExportFile aMIDExportFile, int aRowsCount, int aColumnsCount,
			C1FlexGrid aDetailHdgGrid)
		{
			try
			{
				int topRow;
				int leftCol;
				int bottomRow;
				int rightCol;

				ExportSetStyles(aMIDExportFile);

				// set all cells to detail
				aMIDExportFile.ApplyCellFormatting(g3.Rows.Count, aDetailHdgGrid.Cols.Count,
					aRowsCount - 1, aColumnsCount - 1, "g6Editable1");

				// then override certain cells with specific styles
				// set column headings
				aMIDExportFile.ApplyCellFormatting(0, 0,
					g3.Rows.Count - 1, aColumnsCount - 1, "g3ColumnHeading");
				aMIDExportFile.ApplyCellFormatting(1, 1,
					1, aColumnsCount - 1, "g3GroupHeader");

				// set row headings
				aMIDExportFile.ApplyCellFormatting(0, 0,
					aRowsCount - 1, aDetailHdgGrid.Cols.Count - 1, "g4Style1");

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//switch (_openParms.PlanSessionType)
				switch (_currentPlanSessionType)
				//End Track #5006 - JScott - Display Low-levels one at a time
				{
					case ePlanSessionType.StoreSingleLevel:
						// set group level formatting
						// row header
						topRow = g3.Rows.Count + aDetailHdgGrid.Rows.Count;
						leftCol = 0;
						bottomRow = topRow + ((PagingGridTag)g7.Tag).DetailsPerGroup - 1;
						rightCol = g7.Cols.Count - 1;
						aMIDExportFile.ApplyCellFormatting(topRow, leftCol, bottomRow, rightCol, "g7Reverse1");
						// values
						leftCol = g7.Cols.Count;
						rightCol = aColumnsCount - 1;
						aMIDExportFile.ApplyCellFormatting(topRow, leftCol, bottomRow, rightCol, "g9Editable1");
						// set all stores formatting
						// row header
						topRow = bottomRow + 1;
						leftCol = 0;
						bottomRow = aRowsCount - 1;
						rightCol = g10.Cols.Count - 1;
						aMIDExportFile.ApplyCellFormatting(topRow, leftCol, bottomRow, rightCol, "g10Style1");
						// values
						leftCol = g10.Cols.Count;
						rightCol = aColumnsCount - 1;
						aMIDExportFile.ApplyCellFormatting(topRow, leftCol, bottomRow, rightCol, "g12Editable1");
						break;

					case ePlanSessionType.StoreMultiLevel:
						break;

					case ePlanSessionType.ChainSingleLevel:
						break;

					case ePlanSessionType.ChainMultiLevel:
						break;

					default:
						throw new Exception("Function not currently supported.");
				}
			}
			catch
			{
				throw;
			}
		}

		private void ExportSetStyles(MIDExportFile aMIDExportFile)
		{
			try
			{
				ExportAddStyles(aMIDExportFile, "g2", g2);
				ExportAddStyles(aMIDExportFile, "g3", g3);
				ExportAddStyles(aMIDExportFile, "g4", g4);
				ExportAddStyles(aMIDExportFile, "g5", g5);
				ExportAddStyles(aMIDExportFile, "g6", g6);
				ExportAddStyles(aMIDExportFile, "g7", g7);
				ExportAddStyles(aMIDExportFile, "g8", g8);
				ExportAddStyles(aMIDExportFile, "g9", g9);
				ExportAddStyles(aMIDExportFile, "g10", g10);
				ExportAddStyles(aMIDExportFile, "g11", g11);
				ExportAddStyles(aMIDExportFile, "g12", g12);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

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

        private void GetGridRow(ref object[,] aValueArray, ref ExcelGridInfo[,] aFormatArray, int aSheetRow, int aGridRow, int aGrid1Cols, int aGrid2Cols, int aGrid3Cols, C1FlexGrid aGrid1, C1FlexGrid aGrid2, C1FlexGrid aGrid3)
        {
            int i;
            int j;

            try
            {
                aValueArray.Initialize();

                for (i = 0, j = 0; i < aGrid1Cols; i++, j++)
                {
                    if (aGrid1 != null && aGridRow < aGrid1.Rows.Count)
                    {
                        aValueArray[aSheetRow, j] = "'" + aGrid1[aGridRow, i];
                        CheckFormats(ref aFormatArray, aGrid1[aGridRow, i], aSheetRow, j);
                    }
                }

                for (i = 0; i < aGrid2Cols; i++, j++)
                {
                    if (aGrid2 != null && aGridRow < aGrid2.Rows.Count)
                    {
                        aValueArray[aSheetRow, j] = "'" + aGrid2[aGridRow, i];
                        CheckFormats(ref aFormatArray, aGrid2[aGridRow, i], aSheetRow, j);
                    }
                }

                for (i = 0; i < aGrid3Cols; i++, j++)
                {
                    if (aGrid3 != null && aGridRow < aGrid3.Rows.Count)
                    {
                        aValueArray[aSheetRow, j] = "'" + aGrid3[aGridRow, i];
                        CheckFormats(ref aFormatArray, aGrid3[aGridRow, i], aSheetRow, j);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

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

		//Begin Track #5006 - JScott - Display Low-levels one at a time
		private void tsbNavigate_Click(object sender, EventArgs e)
		{
			Point mousePos;

			try
			{
				mousePos = System.Windows.Forms.Cursor.Position;
				cmsNavigate.Show(tspNavigate, tspNavigate.PointToClient(mousePos));
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void tsbFirst_Click(object sender, EventArgs e)
		{
			try
			{
				ReformatOnNavigate(0);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void tsbPrevious_Click(object sender, EventArgs e)
		{
			int i;

			try
			{
				i = _currentNavigateToolIdx - 1;

				if (i < 0)
				{
					i = cmsNavigate.Items.Count - 1;
				}

				ReformatOnNavigate(i);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void tsbNext_Click(object sender, EventArgs e)
		{
			int i;

			try
			{
				i = _currentNavigateToolIdx + 1;

				if (i == cmsNavigate.Items.Count)
				{
					i = 0;
				}

				ReformatOnNavigate(i);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void tsbLast_Click(object sender, EventArgs e)
		{
			try
			{
				ReformatOnNavigate(cmsNavigate.Items.Count - 1);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiNavigate_Click(object sender, System.EventArgs e)
		{
			try
			{
				ReformatOnNavigate(cmsNavigate.Items.IndexOf((ToolStripMenuItem)sender));
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ReformatOnNavigate(int aNewIndex)
		{
			ToolStripMenuItem tsmi;

			try
			{
				if (aNewIndex != _currentNavigateToolIdx)
				{
					try
					{
						Cursor.Current = Cursors.WaitCursor;

						_currentNavigateToolIdx = aNewIndex;

						tsmi = (ToolStripMenuItem)cmsNavigate.Items[_currentNavigateToolIdx];

						SetGridRedraws(false);
						StopPageLoadThreads();

						foreach (ToolStripMenuItem tsmis in cmsNavigate.Items)
						{
							tsmis.Checked = false;
						}

						tsmi.Checked = true;

						_currentStorePlanProfile = (PlanProfile)tsmi.Tag;
						_currentChainPlanProfile = (PlanProfile)tsmi.Tag;

						if (tsmi.Tag == null)
						{
							if (_openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel ||
								_openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
							{
								_currentPlanSessionType = ePlanSessionType.StoreMultiLevel;
							}
							else
							{
								_currentPlanSessionType = ePlanSessionType.ChainMultiLevel;
							}
						}
						else
						{
							if (_openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel ||
								_openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
							{
								_currentPlanSessionType = ePlanSessionType.StoreSingleLevel;
							}
							else
							{
								_currentPlanSessionType = ePlanSessionType.ChainSingleLevel;
							}
						}

						ConfigureControls();
						BuildFindMenu();
						ReformatRowsChanged(true);
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
					finally
					{
						SetGridRedraws(true);
						LoadSurroundingPages();
						Cursor.Current = Cursors.Default;
						g6.Focus();
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5006 - JScott - Display Low-levels one at a time
		#endregion

        #region Splitters Move Events

        private void spcHScrollLevel1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SplitContainer splitter;

            try
            {
                if (!_hSplitMove)
                {
                    _hSplitMove = true;
                    splitter = (SplitContainer)sender;

                    _currRowSplitPosition12 = splitter.Height - splitter.SplitterDistance;

                    if (spcHHeaderLevel1.Visible)
                    {
                        spcHHeaderLevel1.SplitterDistance = spcHHeaderLevel1.Height - _currRowSplitPosition12;
                    }

                    if (spcHTotalLevel1.Visible)
                    {
                        spcHTotalLevel1.SplitterDistance = spcHTotalLevel1.Height - _currRowSplitPosition12;
                    }

                    if (spcHDetailLevel1.Visible)
                    {
                        spcHDetailLevel1.SplitterDistance = spcHDetailLevel1.Height - _currRowSplitPosition12;
                    }

                    if (spcHScrollLevel1.Visible)
                    {
                        spcHScrollLevel1.SplitterDistance = spcHScrollLevel1.Height - _currRowSplitPosition12;
                    }

                    _hSplitMove = false;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void spcHScrollLevel1_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                CalcRowSplitPosition12(true);

                spcHScrollLevel1.SplitterDistance = spcHScrollLevel1.Height - _currRowSplitPosition12;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void spcHScrollLevel2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SplitContainer splitter;

            try
            {
                if (!_hSplitMove)
                {
                    _hSplitMove = true;
                    splitter = (SplitContainer)sender;

                    _currRowSplitPosition4 = splitter.SplitterDistance;

                    if (spcHHeaderLevel2.Visible)
                    {
                        spcHHeaderLevel2.SplitterDistance = _currRowSplitPosition4;
                    }

                    if (spcHTotalLevel2.Visible)
                    {
                        spcHTotalLevel2.SplitterDistance = _currRowSplitPosition4;
                    }

                    if (spcHDetailLevel2.Visible)
                    {
                        spcHDetailLevel2.SplitterDistance = _currRowSplitPosition4;
                    }

                    if (spcHScrollLevel2.Visible)
                    {
                        spcHScrollLevel2.SplitterDistance = _currRowSplitPosition4;
                    }

                    _hSplitMove = false;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void spcHScrollLevel2_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                CalcRowSplitPosition4(true);

                spcHScrollLevel2.SplitterDistance = _currRowSplitPosition4;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void spcHScrollLevel3_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SplitContainer splitter;
            int splitTest;  //TT#4445-VStuart-Splitter Distance Error-MID

            try
            {
				if (!_hSplitMove)
                {
                    _hSplitMove = true;
                    splitter = (SplitContainer)sender;

                    _currRowSplitPosition8 = splitter.Height - splitter.SplitterDistance;

                    if (spcHHeaderLevel3.Visible)
                    {
                        //BEGIN TT#4445-VStuart-Splitter Distance Error-MID
                        splitTest = spcHHeaderLevel3.Height - _currRowSplitPosition8;
                        if (splitTest < 0)
                        { 
                            spcHHeaderLevel3.SplitterDistance = 1; 
                        }
                        else
                        { 
                            spcHHeaderLevel3.SplitterDistance = spcHHeaderLevel3.Height - _currRowSplitPosition8; 
                        }
                        //END TT#4445-VStuart-Splitter Distance Error-MID
                    }

                    if (spcHTotalLevel3.Visible)
                    {
                        //BEGIN TT#4445-VStuart-Splitter Distance Error-MID
                        splitTest = spcHTotalLevel3.Height - _currRowSplitPosition8;
                        if (splitTest < 0)
                        {
                            spcHTotalLevel3.SplitterDistance = 1;
                        }
                        else
                        {
                            spcHTotalLevel3.SplitterDistance = spcHTotalLevel3.Height - _currRowSplitPosition8;
                        }
                        //END TT#4445-VStuart-Splitter Distance Error-MID
                    }

                    if (spcHDetailLevel3.Visible)
                    {
                        spcHDetailLevel3.SplitterDistance = spcHDetailLevel3.Height - _currRowSplitPosition8;
                    }

                    if (spcHScrollLevel3.Visible)
                    {
                        spcHScrollLevel3.SplitterDistance = spcHScrollLevel3.Height - _currRowSplitPosition8;
                    }

                    HighlightActiveAttributeSet();
                    _hSplitMove = false;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void spcHScrollLevel3_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                CalcRowSplitPosition8(true);

                spcHScrollLevel3.SplitterDistance = spcHScrollLevel3.Height - _currRowSplitPosition8;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void spcVLevel1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SplitContainer splitter;

			try
			{
				splitter = (SplitContainer)sender;

				_currColSplitPosition2 = splitter.SplitterDistance;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
        }

        private void spcVLevel1_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                CalcColSplitPosition2(true);

                spcVLevel1.SplitterDistance = _currColSplitPosition2;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void spcVLevel2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SplitContainer splitter;

			try
			{
				splitter = (SplitContainer)sender;

				_currColSplitPosition3 = splitter.SplitterDistance;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
        }

        private void spcVLevel2_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                CalcColSplitPosition3(true);

                spcVLevel2.SplitterDistance = _currColSplitPosition3;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void SplitterMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                _currSplitterTag = (SplitterTag)((SplitContainer)sender).Tag;

                if (_currSplitterTag.Locked)
                {
                    cmiLockSplitter.Checked = true;
                }
                else
                {
                    cmiLockSplitter.Checked = false;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        #endregion

        #region ScrollBars Scroll Events/Methods

        private void SetScrollBarPosition(ScrollBar aScrollBar, int aPosition)
        {
			try
            {
				if (aPosition != aScrollBar.Value)
                {
					//Begin TT#896 - JScott - Can't Change Attributes if any Column "Freezing" has been done
					//aScrollBar.Value = aPosition;
					aScrollBar.Value = Math.Max(Math.Min(aPosition, aScrollBar.Maximum), aScrollBar.Minimum);
					//End TT#896 - JScott - Can't Change Attributes if any Column "Freezing" has been done
                }

                ((ScrollBarValueChanged)aScrollBar.Tag)(aScrollBar.Value, false);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SetVScrollBar2Parameters()
        {
            try
            {
                if (((PagingGridTag)g4.Tag).Visible)
                {
                    CalculateRowMaximumScroll(vScrollBar2, g4, ((PagingGridTag)g4.Tag).UnitsPerScroll);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SetVScrollBar3Parameters()
        {
            try
            {
                if (((PagingGridTag)g7.Tag).Visible)
                {
                    CalculateRowMaximumScroll(vScrollBar3, g7, ((PagingGridTag)g7.Tag).UnitsPerScroll);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SetVScrollBar4Parameters()
        {
            try
            {
                if (((PagingGridTag)g10.Tag).Visible)
                {
                    CalculateRowMaximumScroll(vScrollBar4, g10, ((PagingGridTag)g10.Tag).UnitsPerScroll);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SetHScrollBar2Parameters()
        {
            int maxScroll;

            try
            {
                if (((PagingGridTag)g2.Tag).Visible)
                {
                    maxScroll = CalculateColMaximumScroll(g2, 1);
                    hScrollBar2.Minimum = 0;
                    hScrollBar2.Maximum = maxScroll + BIGCHANGE - 1;
                    hScrollBar2.SmallChange = SMALLCHANGE;
                    hScrollBar2.LargeChange = BIGCHANGE;
					//Begin Track #5003 - JScott - Freeze Rows

					hScrollBar2.Maximum += g2.Cols.Frozen;
					hScrollBar2.Minimum += g2.Cols.Frozen;
					//End Track #5003 - JScott - Freeze Rows
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SetHScrollBar3Parameters()
        {
            int maxScroll;

            try
            {
                if (((PagingGridTag)g3.Tag).Visible)
                {
                    maxScroll = CalculateColMaximumScroll(g3, 1);

                    hScrollBar3.Minimum = 0;
                    hScrollBar3.SmallChange = SMALLCHANGE;

					//Begin Track #5006 - JScott - Display Low-levels one at a time
					//if (_openParms.PlanSessionType != ePlanSessionType.ChainSingleLevel)
                    if (_currentPlanSessionType != ePlanSessionType.ChainSingleLevel)
					//End Track #5006 - JScott - Display Low-levels one at a time
                    {
                        hScrollBar3.Maximum = maxScroll + ((PagingGridTag)g3.Tag).DetailsPerGroup - 1;
                        hScrollBar3.LargeChange = ((PagingGridTag)g3.Tag).DetailsPerGroup;
                    }
                    else
                    {
                        hScrollBar3.Maximum = maxScroll + BIGCHANGE - 1;
                        hScrollBar3.LargeChange = BIGCHANGE;
                    }
					//Begin Track #5003 - JScott - Freeze Rows

					hScrollBar3.Maximum += g3.Cols.Frozen;
					hScrollBar3.Minimum += g3.Cols.Frozen;
					//End Track #5003 - JScott - Freeze Rows
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		private void CalculateRowMaximumScroll(ScrollBar aScrollBar, C1FlexGrid aGrid, int aScrollSize)
        {
			PagingGridTag gridTag;
			int totalRowSize;
			int i, j;
			//Begin Track #5003 - JScott - Freeze Rows
			int hiddenCount;
			//End Track #5003 - JScott - Freeze Rows
			int totalScrollSize;
			int newValue;
			int totalRows;
			int largeScroll;

			try
			{
				_isScrolling = true;

				aGrid.TopRow = aGrid.TopRow;

				gridTag = (PagingGridTag)aGrid.Tag;

				totalRowSize = 0;
				totalScrollSize = 0;
				totalRows = 0;
				//Begin Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.

				totalScrollSize = 0;

				// Begin Track #6130 stodd - null reference when set has no stores
				if (aGrid.Rows.Count > 0)
				{
					for (i = aGrid.Rows.Count - 1, j = 0; j < aScrollSize; i--, j++)
					{
						totalScrollSize += aGrid.Rows[i].HeightDisplay;
					}
				}
				// End Track #6130 stodd - null reference when set has no stores

				if (totalScrollSize > aGrid.Height && gridTag.HasRowsFrozen)
				{
					UnfreezeRows(gridTag);
				}

				//End Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
				//Begin Track #5003 - JScott - Freeze Rows

				//for (i = aGrid.Rows.Count - 1; totalRowSize <= aGrid.Height && i >= 0; i -= aScrollSize)
				hiddenCount = 0;

				for (i = 0; i < aGrid.Rows.Count; i++)
				{
					if (!aGrid.Rows[i].Visible)
					{
						hiddenCount++;
					}
					else
					{
						break;
					}
				}

				for (i = aGrid.Rows.Count - 1; totalRowSize <= aGrid.Height && i >= hiddenCount; i -= aScrollSize)
				//End Track #5003 - JScott - Freeze Rows
				{
					for (j = 0; j < aScrollSize; j++)
					{
						totalRowSize += aGrid.Rows[i - j].HeightDisplay;
						totalRows++;

						if ((i - j) < aScrollSize)
						{
							totalScrollSize += aGrid.Rows[i - j].HeightDisplay;
							totalRows++;
						}
					}
				}

				//Begin Track #5003 - JScott - Freeze Rows
				i = (i - hiddenCount) + 1;
				//End Track #5003 - JScott - Freeze Rows
				//Begin Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
				//totalScrollSize = 0;

				//for (j = 0; j < aScrollSize; j++)
				//{
				//    totalScrollSize += aGrid.Rows[j].HeightDisplay;
				//}
				//End Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.

				if (totalRowSize > aGrid.Height)
				{
					if (totalScrollSize > aGrid.Height)
					{
						newValue = -1;

						if (gridTag.ScrollType == eScrollType.Group)
						{
							newValue = gridTag.CurrentScrollGroup * gridTag.UnitsPerScroll;
						}

						gridTag.ScrollType = eScrollType.Line;

						aScrollBar.Minimum = 0;
						aScrollBar.Maximum = gridTag.VisibleRowCount + aScrollSize - (aGrid.BottomRow - aGrid.TopRow) - 1;
						aScrollBar.SmallChange = 1;
						aScrollBar.LargeChange = aScrollSize;
						// Begin track #6130 stodd
						if (newValue > aScrollBar.Maximum)
							newValue = aScrollBar.Maximum;
						// End track #6130
						if (newValue != -1)
						{

							aScrollBar.Value = newValue;
							((ScrollBarValueChanged)aScrollBar.Tag)(newValue, false);
						}
					}
					else
					{
						newValue = -1;

						if (gridTag.ScrollType == eScrollType.Line)
						{
							newValue = gridTag.CurrentScrollGroup;
						}

						gridTag.ScrollType = eScrollType.Group;

						if (i + aScrollSize < aGrid.Rows.Count)
						{
							i += aScrollSize;
						}

						largeScroll = (int)((totalRows - 1) / aScrollSize);

						aScrollBar.Minimum = 0;
						aScrollBar.Maximum = ((i + 1) / aScrollSize) + largeScroll - 1;
						aScrollBar.SmallChange = SMALLCHANGE;
						aScrollBar.LargeChange = largeScroll;

						if (newValue != -1)
						{
							aScrollBar.Value = newValue;
							((ScrollBarValueChanged)aScrollBar.Tag)(newValue, false);
						}
						//Begin Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.

						aScrollBar.Maximum += aGrid.Rows.Frozen / gridTag.UnitsPerScroll;
						aScrollBar.Minimum += aGrid.Rows.Frozen / gridTag.UnitsPerScroll;
						//End Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
					}
					//Begin Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
					////Begin Track #5003 - JScott - Freeze Rows

					//aScrollBar.Maximum += aGrid.Rows.Frozen / gridTag.UnitsPerScroll;
					//aScrollBar.Minimum += aGrid.Rows.Frozen / gridTag.UnitsPerScroll;
					////End Track #5003 - JScott - Freeze Rows
					//End Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
				}
				else
				{
					gridTag.ScrollType = eScrollType.None;

					aScrollBar.Minimum = 0;
					aScrollBar.Maximum = BIGCHANGE - 1;
					aScrollBar.SmallChange = SMALLCHANGE;
					aScrollBar.LargeChange = BIGCHANGE;
					//Begin Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.

					if (gridTag.UnitsPerScroll != 0)
					{
						aScrollBar.Maximum += aGrid.Rows.Frozen / gridTag.UnitsPerScroll;
						aScrollBar.Minimum += aGrid.Rows.Frozen / gridTag.UnitsPerScroll;
					}
					//End Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			finally
			{
				_isScrolling = false;
			}
		}

		private int CalculateColMaximumScroll(C1FlexGrid aGrid, int aScrollSize)
        {
            int totalColSize;
			int i, j;
			//Begin Track #5003 - JScott - Freeze Rows
			int hiddenCount;
			//End Track #5003 - JScott - Freeze Rows

            try
            {
                totalColSize = 0;
				//Begin Track #5003 - JScott - Freeze Rows

				//for (i = aGrid.Cols.Count - 1; totalColSize < aGrid.Width && i >= 0; i -= aScrollSize)
				hiddenCount = 0;

				for (i = 0; i < aGrid.Cols.Count; i++)
				{
					if (!aGrid.Cols[i].Visible)
					{
						hiddenCount++;
					}
					else
					{
						break;
					}
				}

				for (i = aGrid.Cols.Count - 1; totalColSize < aGrid.Width && i >= hiddenCount; i -= aScrollSize)
				//End Track #5003 - JScott - Freeze Rows
				{
                    for (j = 0; j < aScrollSize; j++)
                    {
                        totalColSize += aGrid.Cols[i - j].WidthDisplay;
                    }
                }

				//Begin Track #5003 - JScott - Freeze Rows
				//i++;
				i = (i - hiddenCount) + 1;
				//End Track #5003 - JScott - Freeze Rows

				if (totalColSize > aGrid.Width)
				{
					i += aScrollSize;
				}

                return i / aScrollSize;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void vScrollBar2_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            Point mousePos;
            PagingGridTag gridTag;

            try
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallIncrement:
                    case ScrollEventType.SmallDecrement:
                    case ScrollEventType.LargeIncrement:
                    case ScrollEventType.LargeDecrement:
                    case ScrollEventType.ThumbTrack:

                        gridTag = (PagingGridTag)g4.Tag;

                        if (_holdingScroll || e.Type == ScrollEventType.ThumbTrack)
                        {
                            mousePos = this.PointToClient(Control.MousePosition);

                            if (gridTag.ScrollType == eScrollType.Group)
                            {
                                InitVerticalScrollTextBox(vScrollBar2, mousePos, e.NewValue, ((RowHeaderTag)g4.Rows[Math.Min((gridTag.GroupsPerGrid - 1), e.NewValue) * gridTag.UnitsPerScroll].UserData).ScrollDisplay);
                            }
                            else
                            {
                                InitVerticalScrollTextBox(vScrollBar2, mousePos, e.NewValue, ((RowHeaderTag)g4.Rows[e.NewValue].UserData).ScrollDisplay);
                            }
                        }

                        _holdingScroll = true;

                        break;

                    case ScrollEventType.EndScroll:

                        rtbScrollText.Visible = false;
                        _holdingScroll = false;
                        ((ScrollBarValueChanged)vScrollBar2.Tag)(e.NewValue, true);
                        break;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void vScrollBar3_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            Point mousePos;
            PagingGridTag gridTag;

            try
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallIncrement:
                    case ScrollEventType.SmallDecrement:
                    case ScrollEventType.LargeIncrement:
                    case ScrollEventType.LargeDecrement:
                    case ScrollEventType.ThumbTrack:

                        gridTag = (PagingGridTag)g7.Tag;

                        if (_holdingScroll || e.Type == ScrollEventType.ThumbTrack)
                        {
                            mousePos = this.PointToClient(Control.MousePosition);

                            if (gridTag.ScrollType == eScrollType.Group)
                            {
                                InitVerticalScrollTextBox(vScrollBar3, mousePos, e.NewValue, ((RowHeaderTag)g7.Rows[Math.Min((((PagingGridTag)g7.Tag).GroupsPerGrid - 1), e.NewValue) * ((PagingGridTag)g7.Tag).UnitsPerScroll].UserData).ScrollDisplay);
                            }
                            else
                            {
                                InitVerticalScrollTextBox(vScrollBar3, mousePos, e.NewValue, ((RowHeaderTag)g7.Rows[e.NewValue].UserData).ScrollDisplay);
                            }
                        }

                        _holdingScroll = true;

                        break;

                    case ScrollEventType.EndScroll:

                        rtbScrollText.Visible = false;
                        _holdingScroll = false;
                        ((ScrollBarValueChanged)vScrollBar3.Tag)(e.NewValue, true);
                        break;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void vScrollBar4_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            Point mousePos;
            PagingGridTag gridTag;

            try
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallIncrement:
                    case ScrollEventType.SmallDecrement:
                    case ScrollEventType.LargeIncrement:
                    case ScrollEventType.LargeDecrement:
                    case ScrollEventType.ThumbTrack:

                        gridTag = (PagingGridTag)g10.Tag;

                        if (_holdingScroll || e.Type == ScrollEventType.ThumbTrack)
                        {
                            mousePos = this.PointToClient(Control.MousePosition);

                            if (gridTag.ScrollType == eScrollType.Group)
                            {
                                InitVerticalScrollTextBox(vScrollBar4, mousePos, e.NewValue, ((RowHeaderTag)g10.Rows[Math.Min((((PagingGridTag)g10.Tag).GroupsPerGrid - 1), e.NewValue) * ((PagingGridTag)g10.Tag).UnitsPerScroll].UserData).ScrollDisplay);
                            }
                            else
                            {
								if (g10.Rows[e.NewValue].UserData != null)
								{
									InitVerticalScrollTextBox(vScrollBar4, mousePos, e.NewValue, ((RowHeaderTag)g10.Rows[e.NewValue].UserData).ScrollDisplay);
								}
							}
                        }

                        _holdingScroll = true;

                        break;

                    case ScrollEventType.EndScroll:

                        rtbScrollText.Visible = false;
                        _holdingScroll = false;
                        ((ScrollBarValueChanged)vScrollBar4.Tag)(e.NewValue, true);
                        break;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void hScrollBar2_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            Point mousePos;

            try
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallIncrement:
                    case ScrollEventType.SmallDecrement:
                    case ScrollEventType.LargeIncrement:
                    case ScrollEventType.LargeDecrement:
                    case ScrollEventType.ThumbTrack:

                        if (_holdingScroll || e.Type == ScrollEventType.ThumbTrack)
                        {
                            mousePos = this.PointToClient(Control.MousePosition);
                            InitHorizontalScrollTextBox(hScrollBar2, mousePos, ((ColumnHeaderTag)g2.Cols[Math.Min(g2.Cols.Count - 1, e.NewValue)].UserData).ScrollDisplay);
                        }

                        _holdingScroll = true;

                        break;

                    case ScrollEventType.EndScroll:

                        rtbScrollText.Visible = false;
                        _holdingScroll = false;
                        ((ScrollBarValueChanged)hScrollBar2.Tag)(e.NewValue, true);
                        break;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void hScrollBar3_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            Point mousePos;

            try
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallIncrement:
                    case ScrollEventType.SmallDecrement:
                    case ScrollEventType.LargeIncrement:
                    case ScrollEventType.LargeDecrement:
                    case ScrollEventType.ThumbTrack:

                        if (_holdingScroll || e.Type == ScrollEventType.ThumbTrack)
                        {
                            mousePos = this.PointToClient(Control.MousePosition);
                            InitHorizontalScrollTextBox(hScrollBar3, mousePos, ((ColumnHeaderTag)g3.Cols[Math.Min(g3.Cols.Count - 1, e.NewValue)].UserData).ScrollDisplay);
                        }

                        _holdingScroll = true;

                        break;

                    case ScrollEventType.EndScroll:

                        rtbScrollText.Visible = false;
                        _holdingScroll = false;
                        ((ScrollBarValueChanged)hScrollBar3.Tag)(e.NewValue, true);
                        break;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void InitVerticalScrollTextBox(ScrollBar aScrollBar, Point aMousePos, int aNewValue, string[] aPromptList)
        {
            Size textBoxSize;
            Point clientPoint;

            try
            {
                rtbScrollText.Visible = false;

                textBoxSize = new Size(0, 0);

                foreach (string prompt in aPromptList)
                {
                    lblFindSize.Text = prompt;
                    textBoxSize.Height += lblFindSize.Height - 3;
                    textBoxSize.Width = System.Math.Max(lblFindSize.Width - 2, textBoxSize.Width);
                }

                rtbScrollText.Clear();
                rtbScrollText.Size = textBoxSize;

                clientPoint = new Point(this.Right - aScrollBar.Width - rtbScrollText.Width - 20, aMousePos.Y - (rtbScrollText.Height / 2));
                clientPoint.Y = Math.Min(clientPoint.Y, this.PointToClient(pnlScrollBars.PointToScreen(new Point(aScrollBar.Right, aScrollBar.Bottom))).Y - rtbScrollText.Height - 20);
                clientPoint.Y = Math.Max(clientPoint.Y, this.PointToClient(pnlScrollBars.PointToScreen(new Point(aScrollBar.Left, aScrollBar.Top))).Y + 20);

                rtbScrollText.Location = clientPoint;
                rtbScrollText.Lines = aPromptList;
                rtbScrollText.BringToFront();
                rtbScrollText.Visible = true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void InitHorizontalScrollTextBox(ScrollBar aScrollBar, Point aMousePos, string[] aPromptList)
        {
            Size textBoxSize;
            Point clientPoint;

            try
            {
                rtbScrollText.Visible = false;

                textBoxSize = new Size(0, 0);

                foreach (string prompt in aPromptList)
                {
                    lblFindSize.Text = prompt;
                    textBoxSize.Height += lblFindSize.Height - 3;
                    textBoxSize.Width = System.Math.Max(lblFindSize.Width - 2, textBoxSize.Width);
                }

                rtbScrollText.Clear();
                rtbScrollText.Size = textBoxSize;

                clientPoint = new Point(aMousePos.X - (rtbScrollText.Width / 2), this.Bottom - aScrollBar.Height - rtbScrollText.Height - 20);
                clientPoint.X = Math.Min(clientPoint.X, this.PointToClient(aScrollBar.PointToScreen(new Point(aScrollBar.Right, aScrollBar.Bottom))).X - rtbScrollText.Width - 20);
                clientPoint.X = Math.Max(clientPoint.X, this.PointToClient(aScrollBar.PointToScreen(new Point(aScrollBar.Left, aScrollBar.Top))).X + 20);

                rtbScrollText.Location = clientPoint;
                rtbScrollText.Lines = aPromptList;
                rtbScrollText.BringToFront();
                rtbScrollText.Visible = true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ChangeVScrollBar2Value(int aNewValue, bool aLoadValues)
        {
            PagingGridTag gridTag;
            int newValue;

            try
            {
                gridTag = (PagingGridTag)g4.Tag;
                _isScrolling = true;

                switch (gridTag.ScrollType)
                {
                    case eScrollType.Group:
                        newValue = aNewValue * gridTag.UnitsPerScroll;
                        break;

                    default:
                        newValue = aNewValue;
                        break;
                }

                if (((PagingGridTag)g4.Tag).Visible)
                {
                    g4.TopRow = newValue;
                }

                if (((PagingGridTag)g5.Tag).Visible)
                {
                    g5.TopRow = newValue;
                }

                if (((PagingGridTag)g6.Tag).Visible)
                {
                    g6.TopRow = newValue;
                }

                if (aLoadValues)
                {
                    if (((PagingGridTag)g5.Tag).Visible)
                    {
                        LoadCurrentGridPage(g5);
                    }

                    if (((PagingGridTag)g6.Tag).Visible)
                    {
                        LoadCurrentGridPage(g6);
                    }
                }

                gridTag.CurrentScrollPosition = aNewValue;
                _isScrolling = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ChangeVScrollBar3Value(int aNewValue, bool aLoadValues)
        {
            PagingGridTag gridTag;
            int newValue;

            try
            {
                gridTag = (PagingGridTag)g7.Tag;
                _isScrolling = true;

                switch (gridTag.ScrollType)
                {
                    case eScrollType.Group:
                        newValue = aNewValue * gridTag.UnitsPerScroll;
                        break;

                    default:
                        newValue = aNewValue;
                        break;
                }

                if (((PagingGridTag)g7.Tag).Visible)
                {
                    g7.TopRow = newValue;
                }

                if (((PagingGridTag)g8.Tag).Visible)
                {
                    g8.TopRow = newValue;
                }

                if (((PagingGridTag)g9.Tag).Visible)
                {
                    g9.TopRow = newValue;
                }

                if (aLoadValues)
                {
                    if (((PagingGridTag)g8.Tag).Visible)
                    {
                        LoadCurrentGridPage(g8);
                    }

                    if (((PagingGridTag)g9.Tag).Visible)
                    {
                        LoadCurrentGridPage(g9);
                    }
                }

                gridTag.CurrentScrollPosition = aNewValue;
                _isScrolling = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ChangeVScrollBar4Value(int aNewValue, bool aLoadValues)
        {
            PagingGridTag gridTag;
            int newValue;

            try
            {
                gridTag = (PagingGridTag)g10.Tag;
                _isScrolling = true;

                switch (gridTag.ScrollType)
                {
                    case eScrollType.Group:
                        newValue = aNewValue * gridTag.UnitsPerScroll;
                        break;

                    default:
                        newValue = aNewValue;
                        break;
                }

                if (((PagingGridTag)g10.Tag).Visible)
                {
                    g10.TopRow = newValue;
                }

                if (((PagingGridTag)g11.Tag).Visible)
                {
                    g11.TopRow = newValue;
                }

                if (((PagingGridTag)g12.Tag).Visible)
                {
                    g12.TopRow = newValue;
                }

                if (aLoadValues)
                {
                    if (((PagingGridTag)g11.Tag).Visible)
                    {
                        LoadCurrentGridPage(g11);
                    }

                    if (((PagingGridTag)g12.Tag).Visible)
                    {
                        LoadCurrentGridPage(g12);
                    }
                }

                gridTag.CurrentScrollPosition = aNewValue;
                _isScrolling = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ChangeHScrollBar2Value(int aNewValue, bool aLoadValues)
        {
            try
            {
                _isScrolling = true;

                if (((PagingGridTag)g2.Tag).Visible)
                {
                    g2.LeftCol = aNewValue;
                }

                if (((PagingGridTag)g5.Tag).Visible)
                {
                    g5.LeftCol = aNewValue;
                }

                if (((PagingGridTag)g8.Tag).Visible)
                {
                    g8.LeftCol = aNewValue;
                }

                if (((PagingGridTag)g11.Tag).Visible)
                {
                    g11.LeftCol = aNewValue;
                }

                if (aLoadValues)
                {
                    if (((PagingGridTag)g5.Tag).Visible)
                    {
                        LoadCurrentGridPage(g5);
                    }

                    if (((PagingGridTag)g8.Tag).Visible)
                    {
                        LoadCurrentGridPage(g8);
                    }

                    if (((PagingGridTag)g11.Tag).Visible)
                    {
                        LoadCurrentGridPage(g11);
                    }
                }

                ((PagingGridTag)g2.Tag).CurrentScrollPosition = aNewValue;
                _isScrolling = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ChangeHScrollBar3Value(int aNewValue, bool aLoadValues)
        {
            int i;

            try
            {
                _isScrolling = true;
                if (((PagingGridTag)g3.Tag).Visible)
                {
                    g3.LeftCol = aNewValue;
                }

                if (((PagingGridTag)g6.Tag).Visible)
                {
                    g6.LeftCol = aNewValue;
                }

                if (((PagingGridTag)g9.Tag).Visible)
                {
                    g9.LeftCol = aNewValue;
                }

                if (((PagingGridTag)g12.Tag).Visible)
                {
                    g12.LeftCol = aNewValue;
                }

                if (aLoadValues)
                {
                    if (((PagingGridTag)g6.Tag).Visible)
                    {
                        LoadCurrentGridPage(g6);
                    }

                    if (((PagingGridTag)g9.Tag).Visible)
                    {
                        LoadCurrentGridPage(g9);
                    }

                    if (((PagingGridTag)g12.Tag).Visible)
                    {
                        LoadCurrentGridPage(g12);
                    }
                }

                ((PagingGridTag)g3.Tag).CurrentScrollPosition = aNewValue;
                _isScrolling = false;

				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (_openParms.PlanSessionType != ePlanSessionType.ChainSingleLevel)
                if (_currentPlanSessionType != ePlanSessionType.ChainSingleLevel)
				//End Track #5006 - JScott - Display Low-levels one at a time
                {
                    if (((PagingGridTag)g2.Tag).Visible)
                    {
                        for (i = 0; i < g2.Cols.Count; i++)
                        {
                            if (_columnGroupedBy == GroupedBy.GroupedByTime)
                            {
                                if (((ColumnHeaderTag)g2.Cols[i].UserData).DetailRowColHeader.Profile.Key ==
                                    ((ColumnHeaderTag)g3.Cols[g3.LeftCol].UserData).DetailRowColHeader.Profile.Key)
                                {
									//Begin TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
									//hScrollBar2.Value = System.Math.Min(i, hScrollBar2.Maximum - hScrollBar2.LargeChange + 1);
									hScrollBar2.Value = Math.Max(Math.Min(i, hScrollBar2.Maximum - hScrollBar2.LargeChange + 1), hScrollBar2.Minimum);
									//End TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
									ChangeHScrollBar2Value(hScrollBar2.Value, aLoadValues);
                                    break;
                                }
                            }
                            else
                            {
                                if (((ColumnHeaderTag)g2.Cols[i].UserData).GroupRowColHeader.Profile.Key ==
                                    ((ColumnHeaderTag)g3.Cols[g3.LeftCol].UserData).GroupRowColHeader.Profile.Key)
                                {
									//Begin TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
									//hScrollBar2.Value = System.Math.Min(i, hScrollBar2.Maximum - hScrollBar2.LargeChange + 1);
									hScrollBar2.Value = Math.Max(Math.Min(i, hScrollBar2.Maximum - hScrollBar2.LargeChange + 1), hScrollBar2.Minimum);
									//End TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
									ChangeHScrollBar2Value(hScrollBar2.Value, aLoadValues);
                                    break;
                                }
                            }
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

        #endregion

        #region Grid BeforeScroll Events

        private void HighlightActiveAttributeSet()
        {
            try
            {
                int i;

				if (g7.Tag != null && ((PagingGridTag)g7.Tag).Visible)
                {
                    for (i = 0; i < g7.Rows.Count; i++)
                    {
                        if (((RowHeaderTag)g7.Rows[i].UserData).GroupRowColHeader.Profile.Key == (int)cboAttributeSet.SelectedValue)
                        {
							//Begin TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
							//vScrollBar3.Value = System.Math.Min(CalculateGroupFromDetail(g7, i), vScrollBar3.Maximum - vScrollBar3.LargeChange + 1);
							vScrollBar3.Value = Math.Max(Math.Min(CalculateGroupFromDetail(g7, i), vScrollBar3.Maximum - vScrollBar3.LargeChange + 1), vScrollBar3.Minimum);
							//End TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
							ChangeVScrollBar3Value(vScrollBar3.Value, true);
                            break;
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

        private void g2_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g2, null, g5, null, g2, null, hScrollBar2);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g3_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g3, null, g6, null, g2, null, hScrollBar3);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g4_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g4, g6, null, g4, null, vScrollBar2, null);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g5_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g5, g4, g2, g4, g2, vScrollBar2, hScrollBar2);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g6_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g6, g4, g3, g4, g3, vScrollBar2, hScrollBar3);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g7_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g7, g9, null, g7, null, vScrollBar3, null);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g8_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g8, g7, g2, g7, g2, vScrollBar3, hScrollBar2);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g9_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g9, g7, g3, g7, g3, vScrollBar3, hScrollBar3);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g10_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g10, g12, null, g10, null, vScrollBar4, null);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g11_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g11, g10, g2, g10, g2, vScrollBar4, hScrollBar2);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g12_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                BeforeScroll(g12, g10, g3, g10, g3, vScrollBar4, hScrollBar3);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void BeforeScroll(
            C1FlexGrid aGrid,
            C1FlexGrid aRowCompGrid,
            C1FlexGrid aColCompGrid,
            C1FlexGrid aRowHeaderGrid,
            C1FlexGrid aColHeaderGrid,
            VScrollBar aVScrollBar,
            HScrollBar aHScrollBar)
        {
            try
            {
                if (!_isScrolling)
                {
                    if (aRowCompGrid != null && aVScrollBar != null)
                    {
                        if (aGrid.ScrollPosition.Y < aRowCompGrid.ScrollPosition.Y)
                        {
							//Begin TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
							//aVScrollBar.Value = Math.Min(CalculateGroupFromDetail(aRowHeaderGrid, aGrid.TopRow) + 1, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);
							aVScrollBar.Value = Math.Max(Math.Min(CalculateGroupFromDetail(aRowHeaderGrid, aGrid.TopRow) + 1, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1), aVScrollBar.Minimum);
							//End TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
							((ScrollBarValueChanged)aVScrollBar.Tag)(aVScrollBar.Value, true);
						}
                        else if (aGrid.ScrollPosition.Y > aRowCompGrid.ScrollPosition.Y)
                        {
							//Begin TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
							//aVScrollBar.Value = CalculateGroupFromDetail(aRowHeaderGrid, aGrid.TopRow);
							aVScrollBar.Value = Math.Max(CalculateGroupFromDetail(aRowHeaderGrid, aGrid.TopRow), aVScrollBar.Minimum);
							//End TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
							((ScrollBarValueChanged)aVScrollBar.Tag)(aVScrollBar.Value, true);
						}
                    }
                    if (aColCompGrid != null && aHScrollBar != null)
                    {
                        if (aGrid.ScrollPosition.X < aColCompGrid.ScrollPosition.X)
                        {
							//Begin TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
							//aHScrollBar.Value = Math.Min(CalculateGroupFromDetail(aColHeaderGrid, aGrid.LeftCol) + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1);
							aHScrollBar.Value = Math.Max(Math.Min(CalculateGroupFromDetail(aColHeaderGrid, aGrid.LeftCol) + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1), aHScrollBar.Minimum);
							//End TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
							((ScrollBarValueChanged)aHScrollBar.Tag)(aHScrollBar.Value, true);
						}
                        else if (aGrid.ScrollPosition.X > aColCompGrid.ScrollPosition.X)
                        {
							//Begin TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
							//aHScrollBar.Value = CalculateGroupFromDetail(aColHeaderGrid, aGrid.LeftCol);
							aHScrollBar.Value = Math.Max(CalculateGroupFromDetail(aColHeaderGrid, aGrid.LeftCol), aHScrollBar.Minimum);
							//End TT#1014 - JScott - Forecast Review- move a column then freeze a column- change an attribute or sort on a column and receive a system argument out of range exception
							((ScrollBarValueChanged)aHScrollBar.Tag)(aHScrollBar.Value, true);
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

        #endregion

        #region MouseDown Events

        private void ColHeaderMouseDown(object sender, MouseEventArgs e)
        {
            C1FlexGrid grid;
            PagingGridTag gridTag;
            Size dragSize;

            //If the user right-clicked in grid, the context menu will pop up.
            //By setting the form variable "_rightClickedFrom" to grid, we know which
            //sets of headings (TOTALS group or individual week groups) to use
            //when cmiColumnChooser_Click fires.

            try
            {
                grid = (C1FlexGrid)sender;
                gridTag = (PagingGridTag)grid.Tag;
				_rightClickedFrom = grid;

                try
                {
                    _dragBoxFromMouseDown = Rectangle.Empty;

                    if (grid.MouseRow >= 0 && grid.MouseRow < grid.Rows.Count &&
                        grid.MouseCol >= 0 && grid.MouseCol < grid.Cols.Count)
                    {
                        gridTag.MouseDownRow = grid.MouseRow;
                        gridTag.MouseDownCol = grid.MouseCol;

                        if (e.Button == MouseButtons.Right)
                        {
                            _isSorting = false;

                            //if there are columns frozen,
                            //put a check mark next to cmiFreezeColumn context menu
                            //to visually indicate so to the user, and vice versa. 
                            cmiFreezeColumn.Checked = gridTag.HasColsFrozen;
                        }
                        else //left mouse button clicked.
                        {
                            //If left button is pressed, set _dragState to "dragReady" and store the 
                            //initial row and column range. The "ready" state indicates a drag
                            //operation is to begin if the mouse moves while the mouse is down.

                            //we want to enable drag-drop only if the user click on the actual headings (which is on the 3rd row).

                            if (gridTag.MouseDownRow == 2 &&
                                (_dragState == DragState.dragNone || _dragState == DragState.dragReady))
                            {
                                _isSorting = true;
                                _mouseDown = true;
                                _dragState = DragState.dragReady;
                                _dragStartColumn = gridTag.MouseDownCol;
                                dragSize = SystemInformation.DragSize;
                                _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                            }
                            else if (_dragState == DragState.dragResize || gridTag.MouseDownRow != 2)
                            {
                                _isSorting = false;
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    if (_missedUp)
                    {
                        switch (gridTag.GridId)
                        {
                            case Grid2:
                                g2MouseUpRefireHandler(sender, e);
                                break;
                            case Grid3:
                                g3MouseUpRefireHandler(sender, e);
                                break;
                        }
                        _missedUp = false;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void RowHeaderMouseDown(object sender, MouseEventArgs e)
        {
            C1FlexGrid grid;
			//Begin Track #5003 - JScott - Freeze Rows
			PagingGridTag gridTag;
			//End Track #5003 - JScott - Freeze Rows

            try
            {
                grid = (C1FlexGrid)sender;
				//Begin Track #5003 - JScott - Freeze Rows
				gridTag = (PagingGridTag)grid.Tag;
				//End Track #5003 - JScott - Freeze Rows
				_rightClickedFrom = grid;

                if (grid.MouseRow >= 0 && grid.MouseRow < grid.Rows.Count &&
                    grid.MouseCol >= 0 && grid.MouseCol < grid.Cols.Count)
                {
                    ((PagingGridTag)grid.Tag).MouseDownRow = grid.MouseRow;
                    ((PagingGridTag)grid.Tag).MouseDownCol = grid.MouseCol;

                    if (e.Button == MouseButtons.Right)
                    {
                        cmiLockRow.Visible = true;
                        cmiUnlockRow.Visible = true;
                        cmig4g7g10Seperator1.Visible = true;
						//Begin Track #5003 - JScott - Freeze Rows
						cmiFreezeRow.Checked = gridTag.HasRowsFrozen;
						//End Track #5003 - JScott - Freeze Rows
						//Begin Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
						cmiFreezeRow.Enabled = gridTag.ScrollType != eScrollType.Line;
						//End Track #5834 - JScott - Freeze a row and receive an Argument out of Range Exception.
					}
                }
                else
                {
                    cmiLockRow.Visible = false;
                    cmiUnlockRow.Visible = false;
                    cmig4g7g10Seperator1.Visible = false;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void DetailMouseDown(object sender, MouseEventArgs e)
        {
            C1FlexGrid grid;
            PagingGridTag gridTag;
            CellTag cellTag;

            try
            {
                grid = (C1FlexGrid)sender;
                gridTag = (PagingGridTag)grid.Tag;
				_rightClickedFrom = grid;

                if (grid.MouseRow >= 0 && grid.MouseRow < grid.Rows.Count &&
                    grid.MouseCol >= 0 && grid.MouseCol < grid.Cols.Count)
                {
                    gridTag.MouseDownRow = grid.MouseRow;
                    gridTag.MouseDownCol = grid.MouseCol;

                    if (e.Button == MouseButtons.Right)
                    {
                        // Begin TT#4008 - JSmith - Invalid Balance Low Levels option on right mouse for chain single level
                        cmiLockCell.Visible = true;
                        cmiBalance.Visible = true;
                        cmiCopyLowToHigh.Visible = true;
                        cmiBalanceLowLevels.Visible = true;
                        cmiCascadeLockCell.Visible = true;
                        cmiCascadeUnlockCell.Visible = true;
                        // End TT#4008 - JSmith - Invalid Balance Low Levels option on right mouse for chain single level

                        cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[gridTag.MouseDownRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[gridTag.MouseDownCol].UserData).Order];

						if (PlanCellFlagValues.isClosed(cellTag.ComputationCellFlags) ||
                            ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) ||
							PlanCellFlagValues.isIneligible(cellTag.ComputationCellFlags) ||
							ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) ||
							PlanCellFlagValues.isProtected(cellTag.ComputationCellFlags) ||
							ComputationCellFlagValues.isHidden(cellTag.ComputationCellFlags) ||
							ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) ||
                            ((RowHeaderTag)gridTag.RowHeaderGrid.Rows[gridTag.MouseDownRow].UserData).RowHeading == "Balance")
                        {
                            cmiLockCell.Visible = false;
                            // Begin TT#4008 - JSmith - Invalid Balance Low Levels option on right mouse for chain single level
                            cmiCascadeLockCell.Visible = false;
                            cmiCascadeUnlockCell.Visible = false;
                            // End TT#4008 - JSmith - Invalid Balance Low Levels option on right mouse for chain single level
                        }
                        else
                        {
                            cmiLockCell.Visible = true;
							cmiLockCell.Checked = ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags);
                        }
						if (_openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel &&
                            !_lowLevelStoreReadOnly)
                        {
                            if (_forecastBalanceSecurity.AllowExecute)
                            {
                                cmiBalance.Visible = true;
                            }
                            else
                            {
                                cmiBalance.Visible = false;
                            }

							//Begin Enhancement - JScott - Add Balance Low Levels functionality
							cmiBalanceLowLevels.Visible = false;
							//End Enhancement - JScott - Add Balance Low Levels functionality
							cmiCopyLowToHigh.Visible = true;
                        }
						//Begin Enhancement - JScott - Add Balance Low Levels functionality
						else if ((_openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel &&
						    !_lowLevelStoreReadOnly) ||
						    (_openParms.PlanSessionType == ePlanSessionType.ChainMultiLevel &&
						    !_lowLevelChainReadOnly))
						{
							cmiBalance.Visible = false;
							cmiBalanceLowLevels.Visible = true;
							cmiCopyLowToHigh.Visible = true;
						}
						//End Enhancement - JScott - Add Balance Low Levels functionality
                        else
                        {
                            cmiBalance.Visible = false;
							//Begin Enhancement - JScott - Add Balance Low Levels functionality
							cmiBalanceLowLevels.Visible = false;
							//End Enhancement - JScott - Add Balance Low Levels functionality
                            cmiCopyLowToHigh.Visible = false;
                        }
                    }
                }
                else
                {
                    cmiLockCell.Visible = false;
                    cmiBalance.Visible = false;
                    cmiCopyLowToHigh.Visible = false;
                    // Begin TT#4008 - JSmith - Invalid Balance Low Levels option on right mouse for chain single level
                    cmiBalanceLowLevels.Visible = false;
                    cmiCascadeLockCell.Visible = false;
                    cmiCascadeUnlockCell.Visible = false;
                    // End TT#4008 - JSmith - Invalid Balance Low Levels option on right mouse for chain single level
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        #endregion

        #region MouseMove Event

        private void GridMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            C1FlexGrid grid;
            RowHeaderTag rowHdrTag;
			HashKeyObject activeKey;
			int mouseRow;
            CubeWaferCoordinate basisWaferCoor;
            CubeWaferCoordinate nodeWaferCoor;
            System.Windows.Forms.ToolTip toolTip;
            IDictionaryEnumerator dictEnum;

            try
            {
                grid = (C1FlexGrid)sender;
                mouseRow = grid.MouseRow;
				activeKey = null;

                if (mouseRow >= 0 && mouseRow < grid.Rows.Count)
                {
                    rowHdrTag = (RowHeaderTag)grid.Rows[mouseRow].UserData;
					basisWaferCoor = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Basis);

                    if (basisWaferCoor != null)
                    {
						nodeWaferCoor = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.HierarchyNode);

                        if (nodeWaferCoor != null)
                        {
							activeKey = new HashKeyObject(nodeWaferCoor.Key, basisWaferCoor.Key);
							toolTip = (System.Windows.Forms.ToolTip)_basisToolTipList[activeKey];

                            if (toolTip != null)
                            {
                                toolTip.Active = true;
                            }
                        }
                    }
                }

                dictEnum = _basisToolTipList.GetEnumerator();

                while (dictEnum.MoveNext())
                {
					if (activeKey == null || !((HashKeyObject)dictEnum.Key).Equals(activeKey))
                    {
                        ((System.Windows.Forms.ToolTip)dictEnum.Value).Active = false;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
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

				if (PlanCellFlagValues.isClosed(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) ||
					PlanCellFlagValues.isIneligible(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) ||
					PlanCellFlagValues.isProtected(cellTag.ComputationCellFlags) ||
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

        private void GridAfterEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grid = null;
            C1FlexGrid rowGrid;
            C1FlexGrid colGrid;

            try
            {
                grid = (C1FlexGrid)sender;
                rowGrid = GetRowGrid(grid);
                colGrid = GetColumnGrid(grid);

                _planCubeGroup.SetCellValue(
                    _commonWaferCoordinateList,
					((RowHeaderTag)rowGrid.Rows[e.Row].UserData).CubeWaferCoorList,
					((ColumnHeaderTag)colGrid.Cols[e.Col].UserData).CubeWaferCoorList,
					System.Convert.ToDouble(grid[e.Row, e.Col], CultureInfo.CurrentUICulture),
					//Begin Modification - JScott - Add Scaling Decimals
					//((NumericComboObject)cboUnitScaling.SelectedItem).Value,
					//((NumericComboObject)cboDollarScaling.SelectedItem).Value);
					((ComboObject)cboUnitScaling.SelectedItem).Value,
                    ((ComboObject)cboDollarScaling.SelectedItem).Value);
					//End Modification - JScott - Add Scaling Decimals
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

        private void GridKeyPress(object sender, KeyPressEventArgs e)
        {
            C1FlexGrid grid;

            try
            {
                grid = (C1FlexGrid)sender;

                if (e.KeyChar == 13)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        StopPageLoadThreads();
                        RecomputePlanCubes();
                        LoadCurrentPages();
                        LoadSurroundingPages();
                    }
                    catch (Exception exc)
                    {
                        HandleExceptions(exc);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }

                    e.Handled = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

		private void g5_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				GridKeyDown(vScrollBar2, e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void g6_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				GridKeyDown(vScrollBar2, e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void g8_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				GridKeyDown(vScrollBar3, e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void g9_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				GridKeyDown(vScrollBar3, e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void g11_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				GridKeyDown(vScrollBar4, e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void g12_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				GridKeyDown(vScrollBar4, e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void GridKeyDown(VScrollBar aScrollBar, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				switch (e.KeyData)
				{
					case Keys.PageDown:
						aScrollBar.Value = Math.Min(aScrollBar.Value + aScrollBar.LargeChange, aScrollBar.Maximum);
						((ScrollBarValueChanged)aScrollBar.Tag)(aScrollBar.Value, true);
						e.Handled = true;
						break;

					case Keys.PageUp:
						aScrollBar.Value = Math.Max(aScrollBar.Value - aScrollBar.LargeChange, aScrollBar.Minimum);
						((ScrollBarValueChanged)aScrollBar.Tag)(aScrollBar.Value, true);
						e.Handled = true;
						break;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				HandleExceptions(exc);
			}
		}

		private void GridStartEdit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grid;

            try
            {
                grid = (C1FlexGrid)sender;

                _holdValue = grid[e.Row, e.Col];
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private C1FlexGrid GetRowGrid(C1FlexGrid aGrid)
        {
            try
            {
                switch (((PagingGridTag)aGrid.Tag).GridId)
                {
                    case Grid5:
                    case Grid6:
                        return g4;
                    case Grid8:
                    case Grid9:
                        return g7;
                    case Grid11:
                    case Grid12:
                        return g10;
                    default:
                        return null;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private C1FlexGrid GetColumnGrid(C1FlexGrid aGrid)
        {
            try
            {
                switch (((PagingGridTag)aGrid.Tag).GridId)
                {
                    case Grid5:
                    case Grid8:
                    case Grid11:
                        return g2;
                    case Grid6:
                    case Grid9:
                    case Grid12:
                        return g3;
                    default:
                        return null;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void RecomputePlanCubes()
        {
            try
            {
                if (g5.Editor != null)
                {
                    g5.FinishEditing();
                }
                if (g6.Editor != null)
                {
                    g6.FinishEditing();
                }
                if (g8.Editor != null)
                {
                    g8.FinishEditing();
                }
                if (g9.Editor != null)
                {
                    g9.FinishEditing();
                }
                if (g11.Editor != null)
                {
                    g11.FinishEditing();
                }
                if (g12.Editor != null)
                {
                    g12.FinishEditing();
                }

                _planCubeGroup.RecomputeCubes(true);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        #endregion

        #region Miscellaneous

        private void g2_VisibleChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (((PagingGridTag)g2.Tag).Visible)
                {
                    hScrollBar2.Visible = true;
                    spcVLevel2.Panel1Collapsed = false;
                }
                else
                {
                    hScrollBar2.Visible = false;
                    spcVLevel2.Panel1Collapsed = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g7_VisibleChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (((PagingGridTag)g7.Tag).Visible)
                {
                    vScrollBar3.Visible = true;
					spcHHeaderLevel3.Panel2Collapsed = false;
					spcHTotalLevel3.Panel2Collapsed = false;
					spcHDetailLevel3.Panel2Collapsed = false;
					spcHScrollLevel3.Panel2Collapsed = false;
				}
                else
                {
                    vScrollBar3.Visible = false;
					spcHHeaderLevel3.Panel2Collapsed = true;
					spcHTotalLevel3.Panel2Collapsed = true;
					spcHDetailLevel3.Panel2Collapsed = true;
					spcHScrollLevel3.Panel2Collapsed = true;
				}
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g10_VisibleChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (((PagingGridTag)g10.Tag).Visible)
                {
                    vScrollBar4.Visible = true;
					spcHHeaderLevel1.Panel2Collapsed = false;
					spcHTotalLevel1.Panel2Collapsed = false;
					spcHDetailLevel1.Panel2Collapsed = false;
					spcHScrollLevel1.Panel2Collapsed = false;
				}
                else
                {
                    vScrollBar4.Visible = false;
					spcHHeaderLevel1.Panel2Collapsed = true;
					spcHTotalLevel1.Panel2Collapsed = true;
					spcHDetailLevel1.Panel2Collapsed = true;
					spcHScrollLevel1.Panel2Collapsed = true;
				}
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void cmiLockSplitter_Click(object sender, System.EventArgs e)
        {
            try
            {
                _currSplitterTag.Locked = !_currSplitterTag.Locked;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void cmiBasis_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem basisCmiItem;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                basisCmiItem = (ToolStripMenuItem)sender;

                foreach (RowColProfileHeader basisHeader in _selectableBasisHeaders)
                {
                    if (basisHeader.Name == basisCmiItem.Text)
                    {
                        basisHeader.IsDisplayed = !basisCmiItem.Checked;
                        break;
                    }
                }

                basisCmiItem.Checked = !basisCmiItem.Checked;

                ReformatRowsChanged(true);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
                g6.Focus();
            }
        }
        private void cmiBalance_Click(object sender, System.EventArgs e)
        {
            try
            {
                frmForecastBalance forecastBalance = new frmForecastBalance(_sab, _openParms, _planCubeGroup);
                forecastBalance.ShowDialog();
                if (forecastBalance.BalanceSuccessful)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        StopPageLoadThreads();
                        RecomputePlanCubes();
                        LoadCurrentPages();
                        LoadSurroundingPages();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		private void cmiBalanceLowLevels_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				StopPageLoadThreads();

				_planCubeGroup.BalanceLowLevels();
				LoadCurrentPages();
				LoadSurroundingPages();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		//End Enhancement - JScott - Add Balance Low Levels functionality
        private void cmiCopyLowToHigh_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                StopPageLoadThreads();

				//Begin Enhancement - JScott - Add Balance Low Levels functionality
				//RecomputePlanCubes();
				_planCubeGroup.CopyLowToHigh();
				//End Enhancement - JScott - Add Balance Low Levels functionality
                LoadCurrentPages();
                LoadSurroundingPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion
        #endregion

        #region G2 and G3 AfterResizeColumn Events

        private void g2_BeforeAutosizeColumn(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                ResizeCol2();
                g2_AfterResizeColumn(sender, e);
                e.Cancel = true;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g2_AfterResizeColumn(object sender, RowColEventArgs e)
        {
            try
            {
				Cursor.Current = Cursors.WaitCursor;

				SetGridRedraws(false);

				StopPageLoadThreads();

				if (((PagingGridTag)g5.Tag).Visible)
                {
                    g5.Cols[e.Col].WidthDisplay = g2.Cols[e.Col].WidthDisplay;
                }
                if (((PagingGridTag)g8.Tag).Visible)
                {
                    g8.Cols[e.Col].WidthDisplay = g2.Cols[e.Col].WidthDisplay;
                }
                if (((PagingGridTag)g11.Tag).Visible)
                {
                    g11.Cols[e.Col].WidthDisplay = g2.Cols[e.Col].WidthDisplay;
                }

                ResizeRow1();
                CalcRowSplitPosition4(false);
                SetRowSplitPositions();
				SetVScrollBar2Parameters();
				SetVScrollBar3Parameters();
				SetVScrollBar4Parameters();
				LoadCurrentPages();
			}
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
			finally
			{
				SetGridRedraws(true);
				LoadSurroundingPages();
				Cursor.Current = Cursors.Default;
			}
		}

        private void g3_BeforeAutosizeColumn(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                ResizeCol3();
                g3_AfterResizeColumn(sender, e);
                e.Cancel = true;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g3_AfterResizeColumn(object sender, RowColEventArgs e)
        {
            try
            {
				Cursor.Current = Cursors.WaitCursor;

				SetGridRedraws(false);

				StopPageLoadThreads();

				if (((PagingGridTag)g6.Tag).Visible)
                {
                    g6.Cols[e.Col].WidthDisplay = g3.Cols[e.Col].WidthDisplay;
                }
                if (((PagingGridTag)g9.Tag).Visible)
                {
                    g9.Cols[e.Col].WidthDisplay = g3.Cols[e.Col].WidthDisplay;
                }
                if (((PagingGridTag)g12.Tag).Visible)
                {
                    g12.Cols[e.Col].WidthDisplay = g3.Cols[e.Col].WidthDisplay;
                }

                ResizeRow1();
                CalcRowSplitPosition4(false);
                SetRowSplitPositions();
				SetVScrollBar2Parameters();
				SetVScrollBar3Parameters();
				SetVScrollBar4Parameters();
				LoadCurrentPages();
			}
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
			finally
			{
				SetGridRedraws(true);
				LoadSurroundingPages();
				Cursor.Current = Cursors.Default;
			}
		}

        #endregion

        #region Grid Resize Events

        private void g2_Resize(object sender, EventArgs e)
        {
            try
            {
                if (!_formLoading)
                {
                    SetHScrollBar2Parameters();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void g3_Resize(object sender, EventArgs e)
        {
            try
            {
                if (!_formLoading)
                {
                    SetHScrollBar3Parameters();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void g4_Resize(object sender, EventArgs e)
        {
            try
            {
                if (!_formLoading)
                {
                    SetVScrollBar2Parameters();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void g7_Resize(object sender, EventArgs e)
        {
            try
            {
                if (!_formLoading)
                {
                    SetVScrollBar3Parameters();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void g10_Resize(object sender, EventArgs e)
        {
            try
            {
                if (!_formLoading)
                {
                    SetVScrollBar4Parameters();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

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

        private void g2_MouseMove(object sender, MouseEventArgs e)
        {
            //if the _dragState is "dragReady", set the _dragState to "started" 
            //and begin the drag.

            try
            {
                if (_dragState == DragState.dragReady)
                {
                    if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {
                        _isSorting = false;
                        _mouseDown = false;
                        _missedUp = false;
                        _dragState = DragState.dragStarted;
                        g2.DoDragDrop(sender, DragDropEffects.All);
						_dragState = DragState.dragNone;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }
        private void g3_MouseMove(object sender, MouseEventArgs e)
        {
            //if the _dragState is "dragReady", set the _dragState to "started" 
            //and begin the drag.

            try
            {
                if (_dragState == DragState.dragReady)
                {
                    if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {
                        _isSorting = false;
                        _mouseDown = false;
                        _missedUp = false;
                        _dragState = DragState.dragStarted;
                        g3.DoDragDrop(sender, DragDropEffects.All);
						_dragState = DragState.dragNone;
					}
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g2_MouseUp(object sender, MouseEventArgs e)
        {
            //If the button is released, set the _dragState to "dragNone"
            ColumnHeaderTag colHeaderTag;
            RowColProfileHeader colHeader;
            SortCriteria sc;
            SortValue sv;
			//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
			int imageRow;
			//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

            //sorting a single column

            try
            {
                if (_mouseDown)
                {
                    try
                    {
                        _mouseDown = false;
                        _dragState = DragState.dragNone;
                        _dragBoxFromMouseDown = Rectangle.Empty;

                        Cursor.Current = Cursors.WaitCursor;
                        SetGridRedraws(false);

                        if (_isSorting)
                        {
                            StopPageLoadThreads();
                            GetCellRange(g5, 0, ((PagingGridTag)g2.Tag).MouseDownCol, g5.Rows.Count - 1, ((PagingGridTag)g2.Tag).MouseDownCol, 1);

                            colHeaderTag = (ColumnHeaderTag)g2.Cols[((PagingGridTag)g2.Tag).MouseDownCol].UserData;
                            if (_columnGroupedBy == GroupedBy.GroupedByTime)
                            {
                                colHeader = colHeaderTag.DetailRowColHeader;
								//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
								imageRow = g2.Rows.Count - 1;
								//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							}
                            else
                            {
                                colHeader = colHeaderTag.GroupRowColHeader;
								//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
								imageRow = g2.Rows.Count - 2;
								//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							}

                            sc = new SortCriteria();
                            sc.Column1 = "Time Total";
                            sc.Column2 = colHeaderTag.ScrollDisplay[0];
                            sc.Column2GridPtr = g5;
                            sc.Column2Num = ((PagingGridTag)g2.Tag).MouseDownCol;
							sc.Column2Format = ((ComputationVariableProfile)colHeader.Profile).FormatType;

                            if (colHeaderTag.Sort == SortEnum.none || colHeaderTag.Sort == SortEnum.asc)
                            {
                                sc.SortDirection = SortEnum.desc;
                            }
                            else if (colHeaderTag.Sort == SortEnum.desc)
                            {
                                sc.SortDirection = SortEnum.asc;
                            }

                            sv = new SortValue();
                            sv.Row1 = ((RowHeaderTag)g4.Rows[0].UserData).GroupRowColHeader.Name;
                            sv.Row2 = ((RowHeaderTag)g4.Rows[0].UserData).DetailRowColHeader.Name;
                            sv.Row2Num = 0;
                            sv.Row2Format = _quantityVariables.ValueQuantity.FormatType;

                            _currSortParms = new structSort(sv, sc);
							SortColumns(g4, ref _currSortParms);

                            if (_theme.ViewStyle == StyleEnum.AlterColors)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                if (((PagingGridTag)g4.Tag).Visible)
                                {
                                    ChangeRowStyles(g4);
                                }
                                if (((PagingGridTag)g5.Tag).Visible)
                                {
                                    ChangeRowStyles(g5);
                                }
                                if (((PagingGridTag)g6.Tag).Visible)
                                {
                                    ChangeRowStyles(g6);
                                }
                                Cursor.Current = Cursors.Default;
                            }

							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							//LoadCurrentPages();
							ReformatSort();
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
                        }
                    }
                    catch (Exception exception)
                    {
                        HandleExceptions(exception);
                    }
                    finally
                    {
                        SetGridRedraws(true);
                        LoadSurroundingPages();
                        Cursor.Current = Cursors.Default;
                        g6.Focus();
                    }
                }
                // Begin TT#1096 - JSmith - OTS Forecast Sorting
                else if (_dragState == DragState.dragResize)
                {
                    _dragState = DragState.dragNone;
                }
                // End TT#1096
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g3_MouseUp(object sender, MouseEventArgs e)
        {
            //If the button is released, set the _dragState to "dragNone"+
            ColumnHeaderTag colHeaderTag;
            RowColProfileHeader colHeader;
            SortCriteria sc;
            SortValue sv;

            //sorting a single column

            try
            {
                if (_mouseDown)
                {
                    try
                    {
                        _mouseDown = false;
                        _dragState = DragState.dragNone;
                        _dragBoxFromMouseDown = Rectangle.Empty;

                        Cursor.Current = Cursors.WaitCursor;
                        SetGridRedraws(false);

                        if (_isSorting)
                        {
                            StopPageLoadThreads();
                            GetCellRange(g6, 0, ((PagingGridTag)g3.Tag).MouseDownCol, g6.Rows.Count - 1, ((PagingGridTag)g3.Tag).MouseDownCol, 1);

                            colHeaderTag = (ColumnHeaderTag)g3.Cols[((PagingGridTag)g3.Tag).MouseDownCol].UserData;
                            if (_columnGroupedBy == GroupedBy.GroupedByTime)
                            {
                                colHeader = colHeaderTag.DetailRowColHeader;
                            }
                            else
                            {
                                colHeader = colHeaderTag.GroupRowColHeader;
                            }

                            sc = new SortCriteria();
                            sc.Column1 = colHeaderTag.ScrollDisplay[0];
                            sc.Column2 = colHeaderTag.ScrollDisplay[1];
                            sc.Column2GridPtr = g6;
                            sc.Column2Num = ((PagingGridTag)g3.Tag).MouseDownCol;
							sc.Column2Format = ((ComputationVariableProfile)colHeader.Profile).FormatType;

                            if (colHeaderTag.Sort == SortEnum.none || colHeaderTag.Sort == SortEnum.asc)
                            {
                                sc.SortDirection = SortEnum.desc;
                            }
                            else if (colHeaderTag.Sort == SortEnum.desc)
                            {
                                sc.SortDirection = SortEnum.asc;
                            }

                            sv = new SortValue();
                            sv.Row1 = ((RowHeaderTag)g4.Rows[0].UserData).GroupRowColHeader.Name;
                            sv.Row2 = ((RowHeaderTag)g4.Rows[0].UserData).DetailRowColHeader.Name;
                            sv.Row2Num = 0;
                            sv.Row2Format = _quantityVariables.ValueQuantity.FormatType;

                            _currSortParms = new structSort(sv, sc);
							SortColumns(g4, ref _currSortParms);

                            if (_theme.ViewStyle == StyleEnum.AlterColors)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                if (((PagingGridTag)g4.Tag).Visible)
                                {
                                    ChangeRowStyles(g4);
                                }
                                if (((PagingGridTag)g5.Tag).Visible)
                                {
                                    ChangeRowStyles(g5);
                                }
                                if (((PagingGridTag)g6.Tag).Visible)
                                {
                                    ChangeRowStyles(g6);
                                }
                                Cursor.Current = Cursors.Default;
                            }

							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							//LoadCurrentPages();
							ReformatSort();
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
						}
                    }
                    catch (Exception exception)
                    {
                        HandleExceptions(exception);
                    }
                    finally
                    {
                        SetGridRedraws(true);
                        LoadSurroundingPages();
                        Cursor.Current = Cursors.Default;
                        g6.Focus();
                    }
                }
                // Begin TT#1096 - JSmith - OTS Forecast Sorting
                else if (_dragState == DragState.dragResize)
                {
                    _dragState = DragState.dragNone;
                }
                // End TT#1096
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g2_DragEnter(object sender, DragEventArgs e)
        {
            //Check the keystate(state of mouse buttons, among other things). 
            //If the left mouse is down, we're okay. Else, set _dragState
            //to "none", which will cause the drag operation to be cancelled at the 
            //next QueryContinueDrag call. This situation occurs if the user
            //releases the mouse button outside of the grid.
            try
            {
                if ((e.KeyState & 0x01) == 1)
                {
                    e.Effect = DragDropEffects.All;
                }
                else
                {
                    _dragState = DragState.dragNone;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g3_DragEnter(object sender, DragEventArgs e)
        {
            //Check the keystate(state of mouse buttons, among other things). 
            //If the left mouse is down, we're okay. Else, set _dragState
            //to "none", which will cause the drag operation to be cancelled at the 
            //next QueryContinueDrag call. This situation occurs if the user
            //releases the mouse button outside of the grid.
            try
            {
                if ((e.KeyState & 0x01) == 1)
                {
                    e.Effect = DragDropEffects.All;
                }
                else
                {
                    _dragState = DragState.dragNone;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g2_DragOver(object sender, DragEventArgs e)
        {
            //During drag over, if the mouse is dragged to the left- or right-most
            //part of the grid, scroll grid to show the columns.

			C1FlexGrid grid;
			PagingGridTag gridTag;
			System.Drawing.Point mousePoint;
            System.Drawing.Point clientPoint;

			try
			{
				if (e.Data.GetDataPresent(typeof(C1FlexGrid)))
				{
					grid = (C1FlexGrid)e.Data.GetData(typeof(C1FlexGrid));
					gridTag = (PagingGridTag)grid.Tag;

					if (gridTag.GridId == Grid2)
					{
						mousePoint = new Point(e.X, e.Y);
						clientPoint = spcVLevel2.Panel1.PointToClient(mousePoint);

						if (clientPoint.X > spcVLevel2.Panel1.Width - 20 && clientPoint.X < spcVLevel2.Panel1.Right)
						{
							g2.LeftCol++;
						}
						else if (clientPoint.X > 0 && clientPoint.X < 20)
						{
							g2.LeftCol--;
						}
					}
					else
					{
						e.Effect = DragDropEffects.None;
					}
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
        }

        private void g3_DragOver(object sender, DragEventArgs e)
        {
            //During drag over, if the mouse is dragged to the left- or right-most
            //part of the grid, scroll grid to show the columns.

			C1FlexGrid grid;
			PagingGridTag gridTag;
			System.Drawing.Point mousePoint;
            System.Drawing.Point clientPoint;

			try
			{
				if (e.Data.GetDataPresent(typeof(C1FlexGrid)))
				{
					grid = (C1FlexGrid)e.Data.GetData(typeof(C1FlexGrid));
					gridTag = (PagingGridTag)grid.Tag;

					if (gridTag.GridId == Grid3)
					{
						mousePoint = new Point(e.X, e.Y);
						clientPoint = spcVLevel2.Panel2.PointToClient(mousePoint);

						if (clientPoint.X > spcVLevel2.Panel2.Width - 20 && clientPoint.X < spcVLevel2.Panel2.Right)
						{
							g3.LeftCol++;
						}
						else if (clientPoint.X > 0 && clientPoint.X < 20)
						{
							g3.LeftCol--;
						}
					}
					else
					{
						e.Effect = DragDropEffects.None;
					}
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
        }

        private void g2_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            //Check to see if the drag should continue. 
            //Cancel if:
            //(1) the escape key is pressed
            //(2) the DragEnter event handler cancels the drag by setting the 
            //		_dragState to "none".
            //Otherwise, if the mouse is up, perform a drop, or continue if the 
            //mouse if down.

            try
            {
                if (e.EscapePressed)
                {
                    e.Action = DragAction.Cancel;
                }
                else if ((e.KeyState & 0x01) == 0)
                {
                    if (_dragState == DragState.dragNone)
                    {
                        e.Action = DragAction.Cancel;
                    }
                    else
                    {
                        e.Action = DragAction.Drop;
                    }
                }
                else
                {
                    e.Action = DragAction.Continue;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g3_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            //Check to see if the drag should continue. 
            //Cancel if:
            //(1) the escape key is pressed
            //(2) the DragEnter event handler cancels the drag by setting the 
            //		_dragState to "none".
            //Otherwise, if the mouse is up, perform a drop, or continue if the 
            //mouse if down.

            try
            {
                if (e.EscapePressed)
                {
                    e.Action = DragAction.Cancel;
                }
                else if ((e.KeyState & 0x01) == 0)
                {
                    if (_dragState == DragState.dragNone)
                    {
                        e.Action = DragAction.Cancel;
                    }
                    else
                    {
                        e.Action = DragAction.Drop;
                    }
                }
                else
                {
                    e.Action = DragAction.Continue;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void g2_DragDrop(object sender, DragEventArgs e)
        {
            //This event gets fired once the user releases the mouse button during
            //a drag-drop action.
            //In this procedure, move the columns in both the headings grid(g2)
            //and data grids(g5, g8, g11).

            int DragStopColumn = g2.MouseCol; //which column did the user halt.

            try
            {
				//Begin TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
				Cursor.Current = Cursors.WaitCursor;
				SetGridRedraws(false);

				//End TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
				if (_dragStartColumn != DragStopColumn)
                {
					if (((PagingGridTag)g2.Tag).Visible)
                    {
                        g2.Cols.MoveRange(_dragStartColumn, 1, DragStopColumn);
                    }
					if (((PagingGridTag)g5.Tag).Visible)
					{
                        g5.Cols.MoveRange(_dragStartColumn, 1, DragStopColumn);
                    }
					if (((PagingGridTag)g8.Tag).Visible)
					{
                        g8.Cols.MoveRange(_dragStartColumn, 1, DragStopColumn);
                    }
					if (((PagingGridTag)g11.Tag).Visible)
					{
                        g11.Cols.MoveRange(_dragStartColumn, 1, DragStopColumn);
                    }
                }

                //Finally, we want to clear the _dragState. 
                //This is an important clean-up step.

                _dragState = DragState.dragNone;
                _dragBoxFromMouseDown = Rectangle.Empty;
				//Begin TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 

				ReformatColumnMove();
				//End TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
			}
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
				//Begin TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
				SetGridRedraws(true);
				LoadSurroundingPages();
				Cursor.Current = Cursors.Default;
				//End TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
				g6.Focus();
            }
        }

        private void g3_DragDrop(object sender, DragEventArgs e)
        {
            //This event gets fired once the user releases the mouse button during
            //a drag-drop action.
            //In this procedure, move the columns in both the headings grid(g3)
            //and data grids(g6, g9, g12).

            int DragStopColumn; //which column did the user halt.
            int NumColsInOneGroup; //the number of columns in one group of data.
            int ColNum1stGroupEquiv_Start; //the column that the user began the drag.
            int ColNum1stGroupEquiv_Stop; //the column that the user stop the drag (or made the drop).
            int i;
            int j;
            int StartCol;
            int StopCol;
            int newColNum;

            try
            {
				//Begin TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
				Cursor.Current = Cursors.WaitCursor;
				SetGridRedraws(false);

				//End TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
				DragStopColumn = g3.MouseCol;
                NumColsInOneGroup = ((PagingGridTag)g3.Tag).DetailsPerGroup;
                ColNum1stGroupEquiv_Start = _dragStartColumn % ((PagingGridTag)g3.Tag).DetailsPerGroup;
                ColNum1stGroupEquiv_Stop = DragStopColumn % ((PagingGridTag)g3.Tag).DetailsPerGroup;

                if (_dragStartColumn != DragStopColumn)
                {
                    for (i = 0; i < ((PagingGridTag)g3.Tag).GroupsPerGrid; i++)
                    {
                        StartCol = i * NumColsInOneGroup + ColNum1stGroupEquiv_Start;
                        StopCol = i * NumColsInOneGroup + ColNum1stGroupEquiv_Stop;

						if (((PagingGridTag)g3.Tag).Visible)
						{
                            g3.Cols.MoveRange(StartCol, 1, StopCol);
                        }
						if (((PagingGridTag)g6.Tag).Visible)
						{
                            g6.Cols.MoveRange(StartCol, 1, StopCol);
                        }
						if (((PagingGridTag)g9.Tag).Visible)
						{
                            g9.Cols.MoveRange(StartCol, 1, StopCol);
                        }
						if (((PagingGridTag)g12.Tag).Visible)
						{
                            g12.Cols.MoveRange(StartCol, 1, StopCol);
                        }
                    }

                    if (_columnGroupedBy == GroupedBy.GroupedByTime)
                    {
                        // Reorder Time Total Cube

                        if (((PagingGridTag)g2.Tag).Visible)
                        {
                            newColNum = 0;

                            for (i = 0; i < NumColsInOneGroup; i++)
                            {
                                for (j = 0; j < g2.Cols.Count; j++)
                                {
                                    if (((ColumnHeaderTag)g2.Cols[j].UserData).DetailRowColHeader.Profile.Key ==
                                        ((ColumnHeaderTag)g3.Cols[i].UserData).DetailRowColHeader.Profile.Key)
                                    {
										if (((PagingGridTag)g2.Tag).Visible)
										{
                                            g2.Cols.MoveRange(j, 1, newColNum);
                                        }
										if (((PagingGridTag)g5.Tag).Visible)
										{
                                            g5.Cols.MoveRange(j, 1, newColNum);
                                        }
										if (((PagingGridTag)g8.Tag).Visible)
										{
                                            g8.Cols.MoveRange(j, 1, newColNum);
                                        }
										if (((PagingGridTag)g11.Tag).Visible)
										{
                                            g11.Cols.MoveRange(j, 1, newColNum);
                                        }
                                        newColNum++;
                                    }
                                }
                            }
                        }

                        // Resequence Variable Headers

                        if (ColNum1stGroupEquiv_Start < ColNum1stGroupEquiv_Stop)
                        {
                            newColNum = ColNum1stGroupEquiv_Stop;
                            for (i = ColNum1stGroupEquiv_Start; i <= ColNum1stGroupEquiv_Stop; i++)
                            {
                                ((RowColProfileHeader)_sortedVariableHeaders.GetByIndex(i)).Sequence = newColNum;
                                newColNum = i;
                            }
                        }
                        else
                        {
                            newColNum = ColNum1stGroupEquiv_Stop;
                            for (i = ColNum1stGroupEquiv_Start; i >= ColNum1stGroupEquiv_Stop; i--)
                            {
                                ((RowColProfileHeader)_sortedVariableHeaders.GetByIndex(i)).Sequence = newColNum;
                                newColNum = i;
                            }
                        }

                        CreateSortedList(_selectableVariableHeaders, out _sortedVariableHeaders);
                    }
                    else
                    {
                        // Resequence Variable Headers

                        if (ColNum1stGroupEquiv_Start < ColNum1stGroupEquiv_Stop)
                        {
                            newColNum = ColNum1stGroupEquiv_Stop;
                            for (i = ColNum1stGroupEquiv_Start; i <= ColNum1stGroupEquiv_Stop; i++)
                            {
                                ((RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i)).Sequence = newColNum;
                                newColNum = i;
                            }
                        }
                        else
                        {
                            newColNum = ColNum1stGroupEquiv_Stop;
                            for (i = ColNum1stGroupEquiv_Start; i >= ColNum1stGroupEquiv_Stop; i--)
                            {
                                ((RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i)).Sequence = newColNum;
                                newColNum = i;
                            }
                        }

                        CreateSortedList(_selectableTimeHeaders, out _sortedTimeHeaders);
                    }
                }

                //Finally, we want to clear the _dragState. 
                //This is an important clean-up step.

                _dragState = DragState.dragNone;
                _dragBoxFromMouseDown = Rectangle.Empty;
				//Begin TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 

				ReformatColumnMove();
				//End TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
			}
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
				//Begin TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
				SetGridRedraws(true);
				LoadSurroundingPages();
				Cursor.Current = Cursors.Default;
				//End TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
				g6.Focus();
            }
        }

        #endregion

        #region Code related to the "Row/Column Chooser" context menu

        private void cmsg2g3_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                // Begin TT#180 - JSmith - object reference error
                //if (((PagingGridTag)_rightClickedFrom.Tag).GridId == Grid3 && _openParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Period)
                if ((_rightClickedFrom != null && ((PagingGridTag)_rightClickedFrom.Tag).GridId == Grid3) &&
                    (_openParms != null && _openParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Period))
                // End TT#180
                {
                    cmig2g3Seperator3.Visible = true;
					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
					//cmiShowPeriods.Visible = true;
					//cmiShowWeeks.Visible = true;

					//if (cmiShowPeriods.Checked)
					//{
					//    if (cmiShowWeeks.Checked)
					//    {
					//        cmiShowPeriods.Enabled = true;
					//        cmiShowWeeks.Enabled = true;
					//    }
					//    else
					//    {
					//        cmiShowPeriods.Enabled = false;
					//        cmiShowWeeks.Enabled = true;
					//    }
					//}
					//else
					//{
					//    cmiShowPeriods.Enabled = true;
					//    cmiShowWeeks.Enabled = false;
					//}
					//End Track #5121 - JScott - Add Year/Season/Quarter totals
				}
                else
                {
                    cmig2g3Seperator3.Visible = false;
					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
					//cmiShowPeriods.Visible = false;
					//cmiShowWeeks.Visible = false;
					//End Track #5121 - JScott - Add Year/Season/Quarter totals
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		//private void cmiShowPeriods_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        if (cmiShowPeriods.Checked)
		//        {
		//            if (cmiShowWeeks.Checked)
		//            {
		//                cmiShowPeriods.Checked = false;
		//            }
		//            else
		//            {
		//                return;
		//            }
		//        }
		//        else
		//        {
		//            cmiShowPeriods.Checked = true;
		//        }

		//        try
		//        {
		//            SetGridRedraws(false);
		//            StopPageLoadThreads();
		//            BuildTimeHeaders();
		//            ReformatTimeChanged(false);
		//        }
		//        catch (Exception exc)
		//        {
		//            string message = exc.ToString();
		//            throw;
		//        }
		//        finally
		//        {
		//            SetGridRedraws(true);
		//            LoadSurroundingPages();
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        Cursor.Current = Cursors.Default;
		//        g6.Focus();
		//    }
		//}

		//private void cmiShowWeeks_Click(object sender, System.EventArgs e)
		//{
		//    try
		//    {
		//        Cursor.Current = Cursors.WaitCursor;

		//        if (cmiShowWeeks.Checked)
		//        {
		//            if (cmiShowPeriods.Checked)
		//            {
		//                cmiShowWeeks.Checked = false;
		//            }
		//            else
		//            {
		//                return;
		//            }
		//        }
		//        else
		//        {
		//            cmiShowWeeks.Checked = true;
		//        }

		//        try
		//        {
		//            SetGridRedraws(false);
		//            StopPageLoadThreads();
		//            BuildTimeHeaders();
		//            ReformatTimeChanged(false);
		//        }
		//        catch (Exception exc)
		//        {
		//            string message = exc.ToString();
		//            throw;
		//        }
		//        finally
		//        {
		//            SetGridRedraws(true);
		//            LoadSurroundingPages();
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        HandleExceptions(exc);
		//    }
		//    finally
		//    {
		//        Cursor.Current = Cursors.Default;
		//        g6.Focus();
		//    }
		//}
		private void cmiShow_Click(object sender, System.EventArgs e)
		{
			RowColChooser frm;

			try
			{
                // Begin Track #4868 - JSmith - Variable Groupings
                //frm = new RowColChooser(_selectablePeriodHeaders, true, "Show Periods/Weeks", false);
                frm = new RowColChooser(_selectablePeriodHeaders, true, "Show Periods/Weeks", false, null);
                // End Track #4868

				if (frm.ShowDialog() == DialogResult.OK)
				{
					try
					{
						Cursor.Current = Cursors.WaitCursor;

						try
						{
							CreatePeriodHash();

							SetGridRedraws(false);
							StopPageLoadThreads();
							BuildTimeHeaders();
							ReformatTimeChanged(false);
						}
						catch (Exception exc)
						{
							string message = exc.ToString();
							throw;
						}
						finally
						{
							SetGridRedraws(true);
							LoadSurroundingPages();
						}
					}
					catch (Exception exc)
					{
						HandleExceptions(exc);
					}
					finally
					{
						Cursor.Current = Cursors.Default;
						g6.Focus();
					}
				}

				frm.Dispose();
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}
		}
		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals

		private void cmiColumnChooser_Click(object sender, System.EventArgs e)
        {
            try
            {
                cmiVariableChooser_Click(sender, e);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void cmiRowChooser_Click(object sender, System.EventArgs e)
        {
            try
            {
                cmiQuantityChooser_Click(sender, e);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
        }

        private void cmiQuantityChooser_Click(object sender, System.EventArgs e)
        {
            RowColChooser frm;

            try
            {
// Begin Track #4868 - JSmith - Variable Groupings
                //frm = new RowColChooser(_selectableQuantityHeaders, false, "Quantity Chooser", false);
                frm = new RowColChooser(_selectableQuantityHeaders, false, "Quantity Chooser", false, null);
// End Track #4868

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        SetGridRedraws(false);
                        StopPageLoadThreads();
                        ReformatRowsChanged(true);
                    }
                    catch (Exception exc)
                    {
                        HandleExceptions(exc);
                    }
                    finally
                    {
                        SetGridRedraws(true);
                        LoadSurroundingPages();
                        Cursor.Current = Cursors.Default;
                        g6.Focus();
                    }
                }

                frm.Dispose();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
        }

        private void cmiVariableChooser_Click(object sender, System.EventArgs e)
        {
            RowColChooser frm;

            try
            {
// Begin Track #4868 - JSmith - Variable Groupings
                //frm = new RowColChooser(_selectableVariableHeaders, true, "Variable Chooser", true);
                frm = new RowColChooser(_selectableVariableHeaders, true, "Variable Chooser", true, _transaction.PlanComputations.PlanVariables.GetVariableGroupings());
// End Track #4868

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        CreateSortedList(_selectableVariableHeaders, out _sortedVariableHeaders);

                        SetGridRedraws(false);
                        StopPageLoadThreads();

						//Begin Track #5006 - JScott - Display Low-levels one at a time
						//if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                        if (_currentPlanSessionType == ePlanSessionType.ChainSingleLevel)
						//End Track #5006 - JScott - Display Low-levels one at a time
                        {
                            ReformatRowsChanged(true);
                        }
                        else
                        {
                            ReformatColsChanged(true);
                        }
                    }
                    catch (Exception exc)
                    {
                        HandleExceptions(exc);
                    }
                    finally
                    {
                        SetGridRedraws(true);
                        LoadSurroundingPages();
                        Cursor.Current = Cursors.Default;
                        g6.Focus();
                    }
                }

                frm.Dispose();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
        }

        private void CreateSortedList(ArrayList aSelectableList, out SortedList aSortedList)
        {
            SortedList sortList;
            IDictionaryEnumerator enumerator;
            int i, j;
            int newCols;

            try
            {
                sortList = new SortedList();
                newCols = 0;

                for (i = 0; i < aSelectableList.Count; i++)
                {
                    if (((RowColProfileHeader)aSelectableList[i]).IsDisplayed)
                    {
                        if (((RowColProfileHeader)aSelectableList[i]).Sequence == -1)
                        {
                            newCols++;
                            ((RowColProfileHeader)aSelectableList[i]).Sequence = newCols * -1;
                        }
                        sortList.Add(((RowColProfileHeader)aSelectableList[i]).Sequence, i);
                    }
                    else
                    {
                        ((RowColProfileHeader)aSelectableList[i]).Sequence = -1;
                    }
                }

                enumerator = sortList.GetEnumerator();
                j = 0;

                while (enumerator.MoveNext())
                {
                    if (Convert.ToInt32(enumerator.Key, CultureInfo.CurrentUICulture) < 0)
                    {
                        ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = sortList.Count - newCols + (Convert.ToInt32(enumerator.Key, CultureInfo.CurrentUICulture) * -1) - 1;
                    }
                    else
                    {
                        ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = j;
                        j++;
                    }
                }

                aSortedList = new SortedList();

                foreach (RowColProfileHeader rowColHeader in aSelectableList)
                {
                    if (rowColHeader.IsDisplayed)
                    {
                        aSortedList.Add(rowColHeader.Sequence, rowColHeader);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		private void CreatePeriodHash()
		{
			int i;

			try
			{
				_periodHeaderHash = new Hashtable();

				for (i = 0; i < _selectablePeriodHeaders.Count; i++)
				{
					if (((RowColProfileHeader)_selectablePeriodHeaders[i]).IsDisplayed)
					{
						_periodHeaderHash.Add(((RowColProfileHeader)_selectablePeriodHeaders[i]).Sequence, null);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #5121 - JScott - Add Year/Season/Quarter totals

        #endregion

        #region Locking/Unlocking Cell, Column, Row, and Sheet

        private void cmiLockCell_Click(object sender, System.EventArgs e)
        {
            PagingGridTag gridTag;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                gridTag = (PagingGridTag)_rightClickedFrom.Tag;
                LockUnlockGridCell(_rightClickedFrom, !cmiLockCell.Checked);
                GetCellRange(_rightClickedFrom, gridTag.MouseDownRow, gridTag.MouseDownCol, gridTag.MouseDownRow, gridTag.MouseDownCol, 1);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiLockColumn_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                LockUnlockColumn(_rightClickedFrom, true);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiUnlockColumn_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                LockUnlockColumn(_rightClickedFrom, false);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiLockRow_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                LockUnlockRow(_rightClickedFrom, true);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiUnlockRow_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                LockUnlockRow(_rightClickedFrom, false);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiLockSheet_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                LockUnlockSheet(true);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiUnlockSheet_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                LockUnlockSheet(false);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void LockUnlockColumn(C1FlexGrid aGrid, bool LockThisColumn)
        {
            try
            {
                if (((PagingGridTag)aGrid.Tag).GridId == Grid2)
                {
                    //lock and unlock the column only if the column is lockable.
                    LockUnlockGridColumn(g5, ((PagingGridTag)g2.Tag).MouseDownCol, LockThisColumn);
                    LockUnlockGridColumn(g8, ((PagingGridTag)g2.Tag).MouseDownCol, LockThisColumn);
                    LockUnlockGridColumn(g11, ((PagingGridTag)g2.Tag).MouseDownCol, LockThisColumn);
                }
                else if (((PagingGridTag)aGrid.Tag).GridId == Grid3)
                {
                    LockUnlockGridColumn(g6, ((PagingGridTag)g3.Tag).MouseDownCol, LockThisColumn);
                    LockUnlockGridColumn(g9, ((PagingGridTag)g3.Tag).MouseDownCol, LockThisColumn);
                    LockUnlockGridColumn(g12, ((PagingGridTag)g3.Tag).MouseDownCol, LockThisColumn);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void LockUnlockRow(C1FlexGrid aGrid, bool LockThisRow)
        {
            try
            {
                if (((PagingGridTag)aGrid.Tag).GridId == Grid4)
                {
                    if (((PagingGridTag)g5.Tag).Visible)
                    {
                        LockUnlockGridRow(g5, ((PagingGridTag)g4.Tag).MouseDownRow, LockThisRow);
                    }
                    if (((PagingGridTag)g6.Tag).Visible)
                    {
                        LockUnlockGridRow(g6, ((PagingGridTag)g4.Tag).MouseDownRow, LockThisRow);
                    }
                }
                else if (((PagingGridTag)aGrid.Tag).GridId == Grid7)
                {
                    if (((PagingGridTag)g8.Tag).Visible)
                    {
                        LockUnlockGridRow(g8, ((PagingGridTag)g7.Tag).MouseDownRow, LockThisRow);
                    }
                    if (((PagingGridTag)g9.Tag).Visible)
                    {
                        LockUnlockGridRow(g9, ((PagingGridTag)g7.Tag).MouseDownRow, LockThisRow);
                    }
                }
                else if (((PagingGridTag)aGrid.Tag).GridId == Grid10)
                {
                    if (((PagingGridTag)g11.Tag).Visible)
                    {
                        LockUnlockGridRow(g11, ((PagingGridTag)g10.Tag).MouseDownRow, LockThisRow);
                    }
                    if (((PagingGridTag)g12.Tag).Visible)
                    {
                        LockUnlockGridRow(g12, ((PagingGridTag)g10.Tag).MouseDownRow, LockThisRow);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void LockUnlockSheet(bool LockThisSheet)
        {
            //There are several ways to lock/unlock the entire sheet.
            //One way to achieve this is to loop through each column and 
            //perform a column lock on the ones that are lockable.
            try
            {
                int col;

                for (col = 0; col < g2.Cols.Count; col++)
                {
                    if (((PagingGridTag)g5.Tag).Visible)
                    {
                        LockUnlockGridColumn(g5, col, LockThisSheet);
                    }
                    if (((PagingGridTag)g8.Tag).Visible)
                    {
                        LockUnlockGridColumn(g8, col, LockThisSheet);
                    }
                    if (((PagingGridTag)g11.Tag).Visible)
                    {
                        LockUnlockGridColumn(g11, col, LockThisSheet);
                    }
                }

                for (col = 0; col < g3.Cols.Count; col++)
                {
                    if (((PagingGridTag)g6.Tag).Visible)
                    {
                        LockUnlockGridColumn(g6, col, LockThisSheet);
                    }
                    if (((PagingGridTag)g9.Tag).Visible)
                    {
                        LockUnlockGridColumn(g9, col, LockThisSheet);
                    }
                    if (((PagingGridTag)g12.Tag).Visible)
                    {
                        LockUnlockGridColumn(g12, col, LockThisSheet);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void LockUnlockGridCell(C1FlexGrid aGrid, bool LockThisCell)
        {
            PagingGridTag gridTag;
            CellTag cellTag;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;
                cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[gridTag.MouseDownRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[gridTag.MouseDownCol].UserData).Order];

				if (PlanCellFlagValues.isClosed(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) ||
					PlanCellFlagValues.isIneligible(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) ||
					PlanCellFlagValues.isProtected(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isHidden(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags))
                {
                    MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_NotLockable), _windowName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    LockUnlockCell(aGrid, gridTag.MouseDownRow, gridTag.MouseDownCol, LockThisCell);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void LockUnlockGridColumn(C1FlexGrid aGrid, int aColumn, bool aLock)
        {
            PagingGridTag gridTag;
            RowHeaderTag rowTag;
            CellTag cellTag;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);

                gridTag = (PagingGridTag)aGrid.Tag;
                GetCellRange(aGrid, 0, aColumn, aGrid.Rows.Count - 1, aColumn, 1);

                for (int i = 0; i < aGrid.Rows.Count; i++)
                {
                    rowTag = (RowHeaderTag)gridTag.RowHeaderGrid.Rows[i].UserData;

                    if (rowTag != null)
                    {
                        cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[i].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[aColumn].UserData).Order];

						if (!PlanCellFlagValues.isClosed(cellTag.ComputationCellFlags) &&
							!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
							!PlanCellFlagValues.isIneligible(cellTag.ComputationCellFlags) &&
							!ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
							!PlanCellFlagValues.isProtected(cellTag.ComputationCellFlags) &&
							!ComputationCellFlagValues.isHidden(cellTag.ComputationCellFlags) &&
							!ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) &&
							ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags) != aLock)
                        {
                            LockUnlockCell(aGrid, i, aColumn, aLock);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                SetGridRedraws(true);
                Cursor.Current = Cursors.Default;
            }
        }

        private void LockUnlockGridRow(C1FlexGrid aGrid, int aRow, bool aLock)
        {
            PagingGridTag gridTag;
            CellTag cellTag;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);

                gridTag = (PagingGridTag)aGrid.Tag;
                GetCellRange(aGrid, aRow, 0, aRow, aGrid.Cols.Count - 1, 1);

                for (int i = 0; i < aGrid.Cols.Count; i++)
                {
                    cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[aRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[i].UserData).Order];

					if (!PlanCellFlagValues.isClosed(cellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
						!PlanCellFlagValues.isIneligible(cellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
						!PlanCellFlagValues.isProtected(cellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isHidden(cellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) &&
						ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags) != aLock)
                    {
                        LockUnlockCell(aGrid, aRow, i, aLock);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                SetGridRedraws(true);
                Cursor.Current = Cursors.Default;
            }
        }

        private void LockUnlockCell(C1FlexGrid aGrid, int aRow, int aCol, bool aLockCell)
        {
            PagingGridTag gridTag;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;

                _planCubeGroup.SetCellLockStatus(
                    _commonWaferCoordinateList,
					((RowHeaderTag)gridTag.RowHeaderGrid.Rows[aRow].UserData).CubeWaferCoorList,
					((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[aCol].UserData).CubeWaferCoorList,
					aLockCell);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void cmiCascadeLockCell_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                CascadeLockUnlockGridCell(_rightClickedFrom, true);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiCascadeUnlockCell_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                CascadeLockUnlockGridCell(_rightClickedFrom, false);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiCascadeLockColumn_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                CascadeLockUnlockColumn(_rightClickedFrom, true);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiCascadeUnlockColumn_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                CascadeLockUnlockColumn(_rightClickedFrom, false);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiCascadeLockRow_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                CascadeLockUnlockRow(_rightClickedFrom, true);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiCascadeUnlockRow_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                CascadeLockUnlockRow(_rightClickedFrom, false);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiCascadeLockSheet_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                CascadeLockUnlockSheet(true);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiCascadeUnlockSheet_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                StopPageLoadThreads();

                CascadeLockUnlockSheet(false);

                LoadCurrentPages();
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
            }
        }

        private void CascadeLockUnlockColumn(C1FlexGrid aGrid, bool LockThisColumn)
        {
            try
            {
                if (((PagingGridTag)aGrid.Tag).GridId == Grid2)
                {
                    //lock and unlock the column only if the column is lockable.
                    CascadeLockUnlockGridColumn(g5, ((PagingGridTag)g2.Tag).MouseDownCol, LockThisColumn);
                    CascadeLockUnlockGridColumn(g8, ((PagingGridTag)g2.Tag).MouseDownCol, LockThisColumn);
                    CascadeLockUnlockGridColumn(g11, ((PagingGridTag)g2.Tag).MouseDownCol, LockThisColumn);
                }
                else if (((PagingGridTag)aGrid.Tag).GridId == Grid3)
                {
                    CascadeLockUnlockGridColumn(g6, ((PagingGridTag)g3.Tag).MouseDownCol, LockThisColumn);
                    CascadeLockUnlockGridColumn(g9, ((PagingGridTag)g3.Tag).MouseDownCol, LockThisColumn);
                    CascadeLockUnlockGridColumn(g12, ((PagingGridTag)g3.Tag).MouseDownCol, LockThisColumn);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CascadeLockUnlockRow(C1FlexGrid aGrid, bool LockThisRow)
        {
            try
            {
                if (((PagingGridTag)aGrid.Tag).GridId == Grid4)
                {
                    if (((PagingGridTag)g5.Tag).Visible)
                    {
                        CascadeLockUnlockGridRow(g5, ((PagingGridTag)g4.Tag).MouseDownRow, LockThisRow);
                    }
                    if (((PagingGridTag)g6.Tag).Visible)
                    {
                        CascadeLockUnlockGridRow(g6, ((PagingGridTag)g4.Tag).MouseDownRow, LockThisRow);
                    }
                }
                else if (((PagingGridTag)aGrid.Tag).GridId == Grid7)
                {
                    if (((PagingGridTag)g8.Tag).Visible)
                    {
                        CascadeLockUnlockGridRow(g8, ((PagingGridTag)g7.Tag).MouseDownRow, LockThisRow);
                    }
                    if (((PagingGridTag)g9.Tag).Visible)
                    {
                        CascadeLockUnlockGridRow(g9, ((PagingGridTag)g7.Tag).MouseDownRow, LockThisRow);
                    }
                }
                else if (((PagingGridTag)aGrid.Tag).GridId == Grid10)
                {
                    if (((PagingGridTag)g11.Tag).Visible)
                    {
                        CascadeLockUnlockGridRow(g11, ((PagingGridTag)g10.Tag).MouseDownRow, LockThisRow);
                    }
                    if (((PagingGridTag)g12.Tag).Visible)
                    {
                        CascadeLockUnlockGridRow(g12, ((PagingGridTag)g10.Tag).MouseDownRow, LockThisRow);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CascadeLockUnlockSheet(bool LockThisSheet)
        {
            //There are several ways to lock/unlock the entire sheet.
            //One way to achieve this is to loop through each column and 
            //perform a column lock on the ones that are lockable.
            try
            {
                int col;

                for (col = 0; col < g2.Cols.Count; col++)
                {
                    if (((PagingGridTag)g5.Tag).Visible)
                    {
                        CascadeLockUnlockGridColumn(g5, col, LockThisSheet);
                    }
                    if (((PagingGridTag)g8.Tag).Visible)
                    {
                        CascadeLockUnlockGridColumn(g8, col, LockThisSheet);
                    }
                    if (((PagingGridTag)g11.Tag).Visible)
                    {
                        CascadeLockUnlockGridColumn(g11, col, LockThisSheet);
                    }
                }

                for (col = 0; col < g3.Cols.Count; col++)
                {
                    if (((PagingGridTag)g6.Tag).Visible)
                    {
                        CascadeLockUnlockGridColumn(g6, col, LockThisSheet);
                    }
                    if (((PagingGridTag)g9.Tag).Visible)
                    {
                        CascadeLockUnlockGridColumn(g9, col, LockThisSheet);
                    }
                    if (((PagingGridTag)g12.Tag).Visible)
                    {
                        CascadeLockUnlockGridColumn(g12, col, LockThisSheet);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CascadeLockUnlockGridCell(C1FlexGrid aGrid, bool LockThisCell)
        {
            PagingGridTag gridTag;
            CellTag cellTag;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;
                cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[gridTag.MouseDownRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[gridTag.MouseDownCol].UserData).Order];

				if (PlanCellFlagValues.isClosed(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) ||
					PlanCellFlagValues.isIneligible(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) ||
					PlanCellFlagValues.isProtected(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isHidden(cellTag.ComputationCellFlags) ||
					ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags))
                {
                    MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_NotLockable), _windowName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    CascadeLockUnlockCell(aGrid, gridTag.MouseDownRow, gridTag.MouseDownCol, LockThisCell);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CascadeLockUnlockGridColumn(C1FlexGrid aGrid, int aColumn, bool aLock)
        {
            PagingGridTag gridTag;
            RowHeaderTag rowTag;
            CellTag cellTag;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);

                gridTag = (PagingGridTag)aGrid.Tag;
                GetCellRange(aGrid, 0, aColumn, aGrid.Rows.Count - 1, aColumn, 1);

                for (int i = 0; i < aGrid.Rows.Count; i++)
                {
                    rowTag = (RowHeaderTag)gridTag.RowHeaderGrid.Rows[i].UserData;

                    if (rowTag != null)
                    {
                        cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[i].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[aColumn].UserData).Order];

						if (!PlanCellFlagValues.isClosed(cellTag.ComputationCellFlags) &&
							!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
							!PlanCellFlagValues.isIneligible(cellTag.ComputationCellFlags) &&
							!ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
							!PlanCellFlagValues.isProtected(cellTag.ComputationCellFlags) &&
							!ComputationCellFlagValues.isHidden(cellTag.ComputationCellFlags) &&
							!ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) &&
							ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags) != aLock)
                        {
                            CascadeLockUnlockCell(aGrid, i, aColumn, aLock);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                SetGridRedraws(true);
                Cursor.Current = Cursors.Default;
            }
        }

        private void CascadeLockUnlockGridRow(C1FlexGrid aGrid, int aRow, bool aLock)
        {
            PagingGridTag gridTag;
            CellTag cellTag;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);

                gridTag = (PagingGridTag)aGrid.Tag;
                GetCellRange(aGrid, aRow, 0, aRow, aGrid.Cols.Count - 1, 1);

                for (int i = 0; i < aGrid.Cols.Count; i++)
                {
                    cellTag = _gridData[gridTag.GridId][((RowHeaderTag)gridTag.RowHeaderGrid.Rows[aRow].UserData).Order, ((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[i].UserData).Order];

					if (!PlanCellFlagValues.isClosed(cellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isDisplayOnly(cellTag.ComputationCellFlags) &&
						!PlanCellFlagValues.isIneligible(cellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isNull(cellTag.ComputationCellFlags) &&
						!PlanCellFlagValues.isProtected(cellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isHidden(cellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isReadOnly(cellTag.ComputationCellFlags) &&
						ComputationCellFlagValues.isLocked(cellTag.ComputationCellFlags) != aLock)
                    {
                        CascadeLockUnlockCell(aGrid, aRow, i, aLock);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                SetGridRedraws(true);
                Cursor.Current = Cursors.Default;
            }
        }

        private void CascadeLockUnlockCell(C1FlexGrid aGrid, int aRow, int aCol, bool aLockCell)
        {
            PagingGridTag gridTag;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;

                _planCubeGroup.SetCellRecursiveLockStatus(
                    _commonWaferCoordinateList,
					((RowHeaderTag)gridTag.RowHeaderGrid.Rows[aRow].UserData).CubeWaferCoorList,
					((ColumnHeaderTag)gridTag.ColHeaderGrid.Cols[aCol].UserData).CubeWaferCoorList,
					aLockCell);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        #endregion

        #region Group By Week or Group By Variables

        private void optGroupByVariable_CheckedChanged(object sender, System.EventArgs e)
        {
            int i;
            ColumnHeaderTag ColumnTag;
			//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
			//int g2VariableSortKey;
			CubeWaferCoordinateList g2SortCoordinates;
			//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
            SortEnum g2SortDirection;
			//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
			//int g3VariableSortKey;
			//int g3TimeSortKey;
			CubeWaferCoordinateList g3SortCoordinates;
			//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
			SortEnum g3SortDirection;

            try
            {
                if (optGroupByVariable.Checked != false)
                {
                    _columnGroupedBy = GroupedBy.GroupedByVariable;

                    if (!_formLoading)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        SetGridRedraws(false);

                        try
                        {
                            StopPageLoadThreads();

							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							//g2VariableSortKey = -1;
							g2SortCoordinates = null;
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							g2SortDirection = SortEnum.none;

                            if (((PagingGridTag)g2.Tag).Visible)
                            {
                                g2.Clear(ClearFlags.Content);

                                for (i = 0; i < g2.Cols.Count; i++)
                                {
                                    ColumnTag = (ColumnHeaderTag)g2.Cols[i].UserData;
                                    if (ColumnTag.Sort == SortEnum.asc || ColumnTag.Sort == SortEnum.desc)
                                    {
										//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
										//g2VariableSortKey = ColumnTag.DetailRowColHeader.Profile.Key;
										g2SortCoordinates = (CubeWaferCoordinateList)ColumnTag.CubeWaferCoorList.Copy();
										//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
										g2SortDirection = ColumnTag.Sort;
                                        break;
                                    }
                                }
                            }

							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							//g3VariableSortKey = -1;
							//g3TimeSortKey = -1;
							g3SortCoordinates = null;
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							g3SortDirection = SortEnum.none;

                            if (((PagingGridTag)g3.Tag).Visible)
                            {
                                g3.Clear(ClearFlags.Content);

                                for (i = 0; i < g3.Cols.Count; i++)
                                {
                                    ColumnTag = (ColumnHeaderTag)g3.Cols[i].UserData;
                                    if (ColumnTag.Sort == SortEnum.asc || ColumnTag.Sort == SortEnum.desc)
                                    {
										//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
										//g3VariableSortKey = ColumnTag.DetailRowColHeader.Profile.Key;
										//g3TimeSortKey = ColumnTag.GroupRowColHeader.Profile.Key;
										g3SortCoordinates = (CubeWaferCoordinateList)ColumnTag.CubeWaferCoorList.Copy();
										//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
										g3SortDirection = ColumnTag.Sort;
                                        break;
                                    }
                                }
                            }

							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							//ReformatGroupingChanged(true, g2VariableSortKey, g2SortDirection, g3VariableSortKey, g3TimeSortKey, g3SortDirection);
							ReformatGroupingChanged(true, g2SortCoordinates, g2SortDirection, g3SortCoordinates, g3SortDirection);
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
						}
                        catch (Exception exc)
                        {
                            string message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            SetGridRedraws(true);
                            LoadSurroundingPages();
                            Cursor.Current = Cursors.Default;
                            g6.Focus();
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void optGroupByTime_CheckedChanged(object sender, System.EventArgs e)
        {
            int i;
            ColumnHeaderTag ColumnTag;
			//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
			//int g2VariableSortKey;
			CubeWaferCoordinateList g2SortCoordinates;
			//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
			SortEnum g2SortDirection;
			//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
			//int g3VariableSortKey;
			//int g3TimeSortKey;
			CubeWaferCoordinateList g3SortCoordinates;
			//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
			SortEnum g3SortDirection;

            try
            {
                if (_columnGroupedBy != GroupedBy.GroupedByTime)
                {
                    _columnGroupedBy = GroupedBy.GroupedByTime;

                    if (!_formLoading)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        SetGridRedraws(false);

                        try
                        {
                            StopPageLoadThreads();

							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							//g2VariableSortKey = -1;
							g2SortCoordinates = null;
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							g2SortDirection = SortEnum.none;

                            if (((PagingGridTag)g2.Tag).Visible)
                            {
                                g2.Clear(ClearFlags.Content);

								//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
								//for (i = 0; i < g3.Cols.Count; i++)
								for (i = 0; i < g2.Cols.Count; i++)
								//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
								{
									//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
									//ColumnTag = (ColumnHeaderTag)g3.Cols[i].UserData;
									ColumnTag = (ColumnHeaderTag)g2.Cols[i].UserData;
									//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
									if (ColumnTag.Sort == SortEnum.asc || ColumnTag.Sort == SortEnum.desc)
                                    {
										//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
										//g2VariableSortKey = ColumnTag.GroupRowColHeader.Profile.Key;
										g2SortCoordinates = (CubeWaferCoordinateList)ColumnTag.CubeWaferCoorList.Copy();
										//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
										g2SortDirection = ColumnTag.Sort;
                                        break;
                                    }
                                }
                            }

							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							//g3VariableSortKey = -1;
							//g3TimeSortKey = -1;
							g3SortCoordinates = null;
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							g3SortDirection = SortEnum.none;

                            if (((PagingGridTag)g3.Tag).Visible)
                            {
                                g3.Clear(ClearFlags.Content);

                                for (i = 0; i < g3.Cols.Count; i++)
                                {
                                    ColumnTag = (ColumnHeaderTag)g3.Cols[i].UserData;
                                    if (ColumnTag.Sort == SortEnum.asc || ColumnTag.Sort == SortEnum.desc)
                                    {
										//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
										//g3VariableSortKey = ColumnTag.GroupRowColHeader.Profile.Key;
										//g3TimeSortKey = ColumnTag.DetailRowColHeader.Profile.Key;
										g3SortCoordinates = (CubeWaferCoordinateList)ColumnTag.CubeWaferCoorList.Copy();
										//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
										g3SortDirection = ColumnTag.Sort;
                                        break;
                                    }
                                }
                            }

							//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
							//ReformatGroupingChanged(true, g2VariableSortKey, g2SortDirection, g3VariableSortKey, g3TimeSortKey, g3SortDirection);
							ReformatGroupingChanged(true, g2SortCoordinates, g2SortDirection, g3SortCoordinates, g3SortDirection);
							//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
						}
                        catch (Exception exc)
                        {
                            string message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            SetGridRedraws(true);
                            LoadSurroundingPages();
                            Cursor.Current = Cursors.Default;
                            g6.Focus();
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

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

        public void DoSorts()
        {
            SortGridViews frmSortGridViews;
            ArrayList sortList;
            ArrayList valueList;

            try
            {
				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (_openParms.PlanSessionType != ePlanSessionType.ChainSingleLevel)
                if (_currentPlanSessionType != ePlanSessionType.ChainSingleLevel)
				//End Track #5006 - JScott - Display Low-levels one at a time
                {
                    sortList = BuildColumnList();
                    valueList = BuildRowList();

                    frmSortGridViews = new SortGridViews(_currSortParms, sortList, valueList);
                    frmSortGridViews.StartPosition = FormStartPosition.CenterParent;

                    if (frmSortGridViews.ShowDialog() == DialogResult.OK)
                    {
                        _currSortParms = frmSortGridViews.SortInfo;

                        Cursor.Current = Cursors.WaitCursor;

                        SetGridRedraws(false);
                        StopPageLoadThreads();

                        SortColumns(g4, ref _currSortParms);

                        if (_theme.ViewStyle == StyleEnum.AlterColors)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            if (((PagingGridTag)g4.Tag).Visible)
                            {
                                ChangeRowStyles(g4);
                            }
                            if (((PagingGridTag)g5.Tag).Visible)
                            {
                                ChangeRowStyles(g5);
                            }
                            if (((PagingGridTag)g6.Tag).Visible)
                            {
                                ChangeRowStyles(g6);
                            }
                            if (((PagingGridTag)g7.Tag).Visible)
                            {
                                ChangeRowStyles(g7);
                            }
                            if (((PagingGridTag)g8.Tag).Visible)
                            {
                                ChangeRowStyles(g8);
                            }
                            if (((PagingGridTag)g9.Tag).Visible)
                            {
                                ChangeRowStyles(g9);
                            }
                            Cursor.Current = Cursors.Default;
                        }

						//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
						//LoadCurrentPages();
						ReformatSort();
						//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
					}

                    frmSortGridViews.Dispose();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                SetGridRedraws(true);
                LoadSurroundingPages();
                Cursor.Current = Cursors.Default;
                g6.Focus();
            }
        }

        private ArrayList BuildColumnList()
        {
            SortCriteria sortData;
            ColumnHeaderTag colHdrTag;
            int i;
            ArrayList outList;

            try
            {
                outList = new ArrayList();

                switch (_columnGroupedBy)
                {
                    case GroupedBy.GroupedByTime:

						if (((PagingGridTag)g2.Tag).Visible)
						{
                            for (i = 0; i < g2.Cols.Count; i++)
                            {
                                colHdrTag = (ColumnHeaderTag)g2.Cols[i].UserData;
                                sortData = new SortCriteria();
                                sortData.Column1 = "Time Total";
                                sortData.Column2 = colHdrTag.ScrollDisplay[0];
                                sortData.Column2Num = i;
                                sortData.Column2GridPtr = g5;
								sortData.Column2Format = ((ComputationVariableProfile)colHdrTag.DetailRowColHeader.Profile).FormatType;
                                sortData.SortDirection = SortEnum.none;
                                outList.Add(sortData);
                            }
                        }

                        for (i = 0; i < g3.Cols.Count; i++)
                        {
                            colHdrTag = (ColumnHeaderTag)g3.Cols[i].UserData;
                            sortData = new SortCriteria();
                            sortData.Column1 = colHdrTag.ScrollDisplay[0];
                            sortData.Column2 = colHdrTag.ScrollDisplay[1];
                            sortData.Column2Num = i;
                            sortData.Column2GridPtr = g6;
							sortData.Column2Format = ((ComputationVariableProfile)colHdrTag.DetailRowColHeader.Profile).FormatType;
                            sortData.SortDirection = SortEnum.none;
                            outList.Add(sortData);
                        }

                        break;

                    default:

						if (((PagingGridTag)g2.Tag).Visible)
						{
                            for (i = 0; i < g2.Cols.Count; i++)
                            {
                                colHdrTag = (ColumnHeaderTag)g2.Cols[i].UserData;
                                sortData = new SortCriteria();
                                sortData.Column1 = colHdrTag.ScrollDisplay[0];
                                sortData.Column2 = "Time Total";
                                sortData.Column2Num = i;
                                sortData.Column2GridPtr = g5;
								sortData.Column2Format = ((ComputationVariableProfile)colHdrTag.GroupRowColHeader.Profile).FormatType;
                                sortData.SortDirection = SortEnum.none;
                                outList.Add(sortData);
                            }
                        }

                        for (i = 0; i < g3.Cols.Count; i++)
                        {
                            colHdrTag = (ColumnHeaderTag)g3.Cols[i].UserData;
                            sortData = new SortCriteria();
                            sortData.Column1 = colHdrTag.ScrollDisplay[0];
                            sortData.Column2 = colHdrTag.ScrollDisplay[1];
                            sortData.Column2Num = i;
                            sortData.Column2GridPtr = g6;
							sortData.Column2Format = ((ComputationVariableProfile)colHdrTag.GroupRowColHeader.Profile).FormatType;
                            sortData.SortDirection = SortEnum.none;
                            outList.Add(sortData);
                        }

                        break;
                }

                return outList;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private ArrayList BuildRowList()
        {
            ArrayList outList;
            int detailsPerGroup;
            SortValue sortData;
            RowHeaderTag rowHdrTag;
            int i;

            try
            {
                outList = new ArrayList();
                detailsPerGroup = ((PagingGridTag)g4.Tag).DetailsPerGroup;

                for (i = 0; i < detailsPerGroup; i++)
                {
                    rowHdrTag = (RowHeaderTag)g4.Rows[i].UserData;
                    sortData = new SortValue();
                    sortData.Row1 = rowHdrTag.GroupRowColHeader.Name;
                    sortData.Row2 = rowHdrTag.DetailRowColHeader.Name;
                    sortData.Row2Num = i;
					sortData.Row2Format = ((ComputationVariableProfile)rowHdrTag.DetailRowColHeader.Profile).FormatType;
                    outList.Add(sortData);
                }

                return outList;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		private void SortColumns(C1FlexGrid aGrid, ref structSort aSortParms)
		{
            PagingGridTag gridTag;
            SortValue valueData;
            ArrayList sortDirList;
            int i;
            int j;
            SortCriteria sortData;
            string cellValue;
            SortedList sortedList;
            int valueRow;
            ArrayList keyList;
            GridSortEntry sortEnt;
            ColumnHeaderTag colTag;
            C1FlexGrid colHdrGrid;
            ColumnHeaderTag colHdrTag;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;

                sortDirList = new ArrayList();

                for (i = 0; !aSortParms.IsSortingByDefault && i < aSortParms.SortInfo.Count; i++)
                {
                    if (aSortParms.SortInfo[i] != null)
                    {
                        sortData = (SortCriteria)aSortParms.SortInfo[i];

                        if (sortData.Column1 != String.Empty)
                        {
                            GetCellRange(sortData.Column2GridPtr, 0, sortData.Column2Num, sortData.Column2GridPtr.Rows.Count - 1, sortData.Column2Num, 1);
                            sortDirList.Add(sortData.SortDirection);
                        }
                    }
                }

                sortedList = new SortedList(new SortComparer(sortDirList));

                for (i = 0; i < gridTag.GroupsPerGrid; i++)
                {
                    keyList = new ArrayList();

                    if (aSortParms.ValueInfo != null)
                    {
                        valueData = (SortValue)aSortParms.ValueInfo;
                        valueRow = (i * gridTag.DetailsPerGroup) + valueData.Row2Num;

                        for (j = 0; !aSortParms.IsSortingByDefault && j < aSortParms.SortInfo.Count; j++)
                        {
                            if (aSortParms.SortInfo[j] != null)
                            {
                                sortData = (SortCriteria)aSortParms.SortInfo[j];

                                if (sortData.Column1 != String.Empty)
                                {
                                    cellValue = Convert.ToString(sortData.Column2GridPtr[valueRow, sortData.Column2Num]).Trim();

                                    switch (valueData.Row2Format)
                                    {
                                        case eValueFormatType.None:

                                            switch (sortData.Column2Format)
                                            {
                                                case eValueFormatType.GenericNumeric:

                                                    if (cellValue.Length > 0)
                                                    {
                                                        keyList.Add(Convert.ToDouble(sortData.Column2GridPtr[valueRow, sortData.Column2Num]));
                                                    }
                                                    else
                                                    {
                                                        keyList.Add((double)0);
                                                    }
                                                    break;

                                                default:

                                                    keyList.Add(cellValue);
                                                    break;
                                            }

                                            break;

                                        case eValueFormatType.GenericNumeric:

                                            if (cellValue.Length > 0)
                                            {
                                                keyList.Add(Convert.ToDouble(sortData.Column2GridPtr[valueRow, sortData.Column2Num]));
                                            }
                                            else
                                            {
                                                keyList.Add((double)0);
                                            }
                                            break;

                                        default:

                                            keyList.Add(cellValue);
                                            break;
                                    }
                                }
                            }
                        }
                    }

					//Begin Track #5006 - JScott - Display Low-levels one at a time
					//if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
                    if (_currentPlanSessionType == ePlanSessionType.ChainSingleLevel)
					//End Track #5006 - JScott - Display Low-levels one at a time
                    {
                        sortedList.Add(new GridSortEntry(aGrid.Rows[i * gridTag.DetailsPerGroup], keyList, ((RowColProfileHeader)((RowHeaderTag)aGrid.Rows[i * gridTag.DetailsPerGroup].UserData).GroupRowColHeader).Profile.Key, i), null);
                    }
                    else
                    {
						//Begin Track #6261 - JScott - Sets in Set summary section are out of order
						//sortedList.Add(new GridSortEntry(aGrid.Rows[i * gridTag.DetailsPerGroup], keyList, Convert.ToString(aGrid[i * gridTag.DetailsPerGroup, 0]), i), null);
						if (gridTag.GridId == Grid7)
						{
							sortedList.Add(new GridSortEntry(aGrid.Rows[i * gridTag.DetailsPerGroup], keyList, ((RowHeaderTag)aGrid.Rows[i * gridTag.DetailsPerGroup].UserData).GroupRowColHeader.Sequence, i), null);
						}
						else
						{
							sortedList.Add(new GridSortEntry(aGrid.Rows[i * gridTag.DetailsPerGroup], keyList, Convert.ToString(aGrid[i * gridTag.DetailsPerGroup, 0]), i), null);
						}
						//End Track #6261 - JScott - Sets in Set summary section are out of order
					}
                }

                for (i = 0; i < sortedList.Count; i++)
                {
                    sortEnt = (GridSortEntry)sortedList.GetKey(i);
                    MoveRows(aGrid, gridTag.DetailsPerGroup, sortEnt.RowIndex, i * gridTag.DetailsPerGroup);
                }

                for (i = 0; i < g2.Cols.Count; i++)
                {
                    colTag = (ColumnHeaderTag)g2.Cols[i].UserData;
                    colTag.Sort = SortEnum.none;
                    g2.Cols[i].UserData = colTag;
					//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
					//g2.SetCellImage(g2.Rows.Count - 1, i, null);
					g2.SetCellImage(((PagingGridTag)g2.Tag).SortImageRow, i, null);
					//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				}

                for (i = 0; i < g3.Cols.Count; i++)
                {
                    colTag = (ColumnHeaderTag)g3.Cols[i].UserData;
                    colTag.Sort = SortEnum.none;
                    g3.Cols[i].UserData = colTag;
					//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
					//g3.SetCellImage(g3.Rows.Count - 1, i, null);
					g3.SetCellImage(((PagingGridTag)g3.Tag).SortImageRow, i, null);
					//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				}

                for (i = 0; !aSortParms.IsSortingByDefault && i < aSortParms.SortInfo.Count; i++)
                {
                    if (aSortParms.SortInfo[i] != null)
                    {
                        sortData = (SortCriteria)aSortParms.SortInfo[i];

                        if (sortData.Column1 != String.Empty)
                        {
                            colHdrGrid = ((PagingGridTag)sortData.Column2GridPtr.Tag).ColHeaderGrid;
                            colHdrTag = (ColumnHeaderTag)colHdrGrid.Cols[sortData.Column2Num].UserData;

                            if (sortData.SortDirection == SortEnum.asc)
                            {
								//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
								//colHdrGrid.SetCellImage(colHdrGrid.Rows.Count - 1, sortData.Column2Num, _upArrow);
								colHdrGrid.SetCellImage(((PagingGridTag)colHdrGrid.Tag).SortImageRow, sortData.Column2Num, _upArrow);
								//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
								colHdrTag.Sort = SortEnum.asc;
                            }
                            else
                            {
								//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
								//colHdrGrid.SetCellImage(colHdrGrid.Rows.Count - 1, sortData.Column2Num, _downArrow);
								colHdrGrid.SetCellImage(((PagingGridTag)colHdrGrid.Tag).SortImageRow, sortData.Column2Num, _downArrow);
								//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
								colHdrTag.Sort = SortEnum.desc;
                            }

                            colHdrGrid.Cols[sortData.Column2Num].UserData = colHdrTag;
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

        private void SortToDefault()
        {
            try
            {
				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//if (_openParms.PlanSessionType != ePlanSessionType.ChainSingleLevel)
                if (_currentPlanSessionType != ePlanSessionType.ChainSingleLevel)
				//End Track #5006 - JScott - Display Low-levels one at a time
                {
                    _currSortParms = new structSort();
                    _currSortParms.IsSortingByDefault = true;

                    SortColumns(g4, ref _currSortParms);
                    SortColumns(g7, ref _currSortParms);
				}
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void MoveRows(C1FlexGrid aGrid, int aRowsPerGroup, int OldIndex, int NewIndex)
        {
            Object detailProf;

            try
            {
                switch (((PagingGridTag)aGrid.Tag).GridId)
                {
                    case Grid4:

                        if (((PagingGridTag)g4.Tag).Visible)
                        {
                            g4.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }
                        if (((PagingGridTag)g5.Tag).Visible)
                        {
                            g5.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }
                        if (((PagingGridTag)g6.Tag).Visible)
                        {
                            g6.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }

                        detailProf = _workingDetailProfileList.ArrayList[OldIndex / aRowsPerGroup];
                        _workingDetailProfileList.ArrayList.RemoveAt(OldIndex / aRowsPerGroup);
                        _workingDetailProfileList.ArrayList.Insert(NewIndex / aRowsPerGroup, detailProf);

                        break;

                    case Grid7:

                        if (((PagingGridTag)g7.Tag).Visible)
                        {
                            g7.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }
                        if (((PagingGridTag)g8.Tag).Visible)
                        {
                            g8.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }
                        if (((PagingGridTag)g9.Tag).Visible)
                        {
                            g9.Rows.MoveRange(OldIndex, aRowsPerGroup, NewIndex);
                        }

                        break;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void g7_Click(object sender, System.EventArgs e)
        {
            //Add attribute-changing code here.
        }

        #endregion

        #region Grid Reformat methods

        private void ReformatRowsChanged(bool aClearGrid)
        {
            try
            {
				//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				//Formatg2Grid(aClearGrid, -1, SortEnum.none);
				//Formatg3Grid(aClearGrid, -1, -1, SortEnum.none);
				Formatg2Grid(aClearGrid, null, SortEnum.none);
				Formatg3Grid(aClearGrid, null, SortEnum.none);
				//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				Formatg4Grid(aClearGrid, g4, _workingDetailProfileList, true);
                Formatg5Grid(aClearGrid);
                Formatg6Grid(aClearGrid);
                Formatg7Grid(aClearGrid);
                Formatg8Grid(aClearGrid);
                Formatg9Grid(aClearGrid);
                Formatg10Grid(aClearGrid);
                Formatg11Grid(aClearGrid);
                Formatg12Grid(aClearGrid);
				//Begin Track #6064 - JScott - After making a change to Stock and navigating to the next page receive a Argument out of Range Exception

				g2.Visible = ((PagingGridTag)g2.Tag).Visible;
				g3.Visible = ((PagingGridTag)g3.Tag).Visible;
				g4.Visible = ((PagingGridTag)g4.Tag).Visible;
				g5.Visible = ((PagingGridTag)g5.Tag).Visible;
				g6.Visible = ((PagingGridTag)g6.Tag).Visible;
				g7.Visible = ((PagingGridTag)g7.Tag).Visible;
				g8.Visible = ((PagingGridTag)g8.Tag).Visible;
				g9.Visible = ((PagingGridTag)g9.Tag).Visible;
				g10.Visible = ((PagingGridTag)g10.Tag).Visible;
				g11.Visible = ((PagingGridTag)g11.Tag).Visible;
				g12.Visible = ((PagingGridTag)g12.Tag).Visible;

				//End Track #6064 - JScott - After making a change to Stock and navigating to the next page receive a Argument out of Range Exception
				SortToDefault();
                Setg2Styles(false);
                Setg3Styles(false);
                Setg4Styles(false);
                Setg5Styles(false);
                Setg6Styles(false);
                Setg7Styles(false);
                Setg8Styles(false);
                Setg9Styles(false);
                Setg10Styles(false);
                Setg11Styles(false);
                Setg12Styles(false);
                ResizeRow4();
                ResizeRow7();
                ResizeRow10();
                SetHScrollBar2Parameters();
                SetHScrollBar3Parameters();
                SetVScrollBar2Parameters();
                SetVScrollBar3Parameters();
                SetVScrollBar4Parameters();
                SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
                SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
                SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
                SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
                SetScrollBarPosition(vScrollBar4, vScrollBar4.Value);
                LoadCurrentPages();
                ResizeCol1();
                ResizeCol2();
                ResizeCol3();
                ResizeRow1();
                CalcColSplitPosition2(false);
                CalcColSplitPosition3(false);
                SetColSplitPositions();
                CalcRowSplitPosition4(false);
                CalcRowSplitPosition8(false);
                CalcRowSplitPosition12(false);
                SetRowSplitPositions();

                // Reset Scroll bars due to change in split positions
                SetHScrollBar2Parameters();
                SetHScrollBar3Parameters();
                SetVScrollBar2Parameters();
                SetVScrollBar3Parameters();
                SetVScrollBar4Parameters();
                SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
                SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
                SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
                SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
                SetScrollBarPosition(vScrollBar4, vScrollBar4.Value);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ReformatColsChanged(bool aClearGrid)
        {
            try
            {
				//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				//Formatg2Grid(aClearGrid, -1, SortEnum.none);
				//Formatg3Grid(aClearGrid, -1, -1, SortEnum.none);
				Formatg2Grid(aClearGrid, null, SortEnum.none);
				Formatg3Grid(aClearGrid, null, SortEnum.none);
				//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				Formatg5Grid(aClearGrid);
                Formatg6Grid(aClearGrid);
                Formatg8Grid(aClearGrid);
                Formatg9Grid(aClearGrid);
                Formatg11Grid(aClearGrid);
                Formatg12Grid(aClearGrid);
				//Begin Track #6064 - JScott - After making a change to Stock and navigating to the next page receive a Argument out of Range Exception

				g2.Visible = ((PagingGridTag)g2.Tag).Visible;
				g3.Visible = ((PagingGridTag)g3.Tag).Visible;
				g5.Visible = ((PagingGridTag)g5.Tag).Visible;
				g6.Visible = ((PagingGridTag)g6.Tag).Visible;
				g8.Visible = ((PagingGridTag)g8.Tag).Visible;
				g9.Visible = ((PagingGridTag)g9.Tag).Visible;
				g11.Visible = ((PagingGridTag)g11.Tag).Visible;
				g12.Visible = ((PagingGridTag)g12.Tag).Visible;

				//End Track #6064 - JScott - After making a change to Stock and navigating to the next page receive a Argument out of Range Exception
				Setg2Styles(false);
                Setg3Styles(false);
                Setg5Styles(false);
                Setg6Styles(false);
                Setg8Styles(false);
                Setg9Styles(false);
                Setg11Styles(false);
                Setg12Styles(false);
                ResizeRow4();
                ResizeRow7();
                ResizeRow10();
                SetHScrollBar2Parameters();
                SetHScrollBar3Parameters();
                SetVScrollBar2Parameters();
                SetVScrollBar3Parameters();
                SetVScrollBar4Parameters();
                SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
                SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
                SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
                SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
                SetScrollBarPosition(vScrollBar4, vScrollBar4.Value);
                LoadCurrentPages();
                ResizeCol2();
                ResizeCol3();
                ResizeRow1();
                CalcColSplitPosition2(false);
                CalcColSplitPosition3(false);
                SetColSplitPositions();
                CalcRowSplitPosition4(false);
                SetRowSplitPositions();

                // Reset Scroll bars due to change in split positions
                SetHScrollBar2Parameters();
                SetHScrollBar3Parameters();
                SetVScrollBar2Parameters();
                SetVScrollBar3Parameters();
                SetVScrollBar4Parameters();
                SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
                SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
                SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
                SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
                SetScrollBarPosition(vScrollBar4, vScrollBar4.Value);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ReformatTimeChanged(bool aClearGrid)
        {
            try
            {
				//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				//Formatg3Grid(aClearGrid, -1, -1, SortEnum.none);
				Formatg3Grid(aClearGrid, null, SortEnum.none);
				//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				Formatg6Grid(aClearGrid);
                Formatg9Grid(aClearGrid);
                Formatg12Grid(aClearGrid);
				//Begin Track #6064 - JScott - After making a change to Stock and navigating to the next page receive a Argument out of Range Exception

				g3.Visible = ((PagingGridTag)g3.Tag).Visible;
				g6.Visible = ((PagingGridTag)g6.Tag).Visible;
				g9.Visible = ((PagingGridTag)g9.Tag).Visible;
				g12.Visible = ((PagingGridTag)g12.Tag).Visible;

				//End Track #6064 - JScott - After making a change to Stock and navigating to the next page receive a Argument out of Range Exception
				Setg3Styles(false);
                Setg6Styles(false);
                Setg9Styles(false);
                Setg12Styles(false);
                SetHScrollBar3Parameters();
                SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
                LoadCurrentPages();
                ResizeCol3();
                ResizeRow1();
                CalcColSplitPosition3(false);
                SetColSplitPositions();
                CalcRowSplitPosition4(false);
                SetRowSplitPositions();

                // Reset Scroll bars due to change in split positions
                SetHScrollBar2Parameters();
                SetHScrollBar3Parameters();
                SetVScrollBar2Parameters();
                SetVScrollBar3Parameters();
                SetVScrollBar4Parameters();
                SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
                SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
                SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
                SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
                SetScrollBarPosition(vScrollBar4, vScrollBar4.Value);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ReformatAttributeChanged(bool aClearGrid)
        {
            try
            {
				Formatg4Grid(aClearGrid, g4, _workingDetailProfileList, true);
                Formatg5Grid(aClearGrid);
                Formatg6Grid(aClearGrid);
                Formatg7Grid(aClearGrid);
                Formatg8Grid(aClearGrid);
                Formatg9Grid(aClearGrid);
				//Begin Track #6064 - JScott - After making a change to Stock and navigating to the next page receive a Argument out of Range Exception

				g4.Visible = ((PagingGridTag)g4.Tag).Visible;
				g5.Visible = ((PagingGridTag)g5.Tag).Visible;
				g6.Visible = ((PagingGridTag)g6.Tag).Visible;
				g7.Visible = ((PagingGridTag)g7.Tag).Visible;
				g8.Visible = ((PagingGridTag)g8.Tag).Visible;
				g9.Visible = ((PagingGridTag)g9.Tag).Visible;

				//End Track #6064 - JScott - After making a change to Stock and navigating to the next page receive a Argument out of Range Exception
				SortToDefault();
                Setg4Styles(false);
                Setg5Styles(false);
                Setg6Styles(false);
                Setg7Styles(false);
                Setg8Styles(false);
                Setg9Styles(false);
                ResizeRow4();
                ResizeRow7();
                SetVScrollBar2Parameters();
                SetVScrollBar3Parameters();
                SetScrollBarPosition(hScrollBar2, 0);
                SetScrollBarPosition(hScrollBar3, 0);
                SetScrollBarPosition(vScrollBar2, 0);
                SetScrollBarPosition(vScrollBar3, 0);
                SetScrollBarPosition(vScrollBar4, 0);
                LoadCurrentPages();
                ResizeCol1();
                CalcColSplitPosition2(false);
                SetColSplitPositions();
                CalcRowSplitPosition8(false);
                CalcRowSplitPosition12(false);
                SetRowSplitPositions();

                // Reset Scroll bars due to change in split positions
                SetHScrollBar2Parameters();
                SetHScrollBar3Parameters();
                SetVScrollBar2Parameters();
                SetVScrollBar3Parameters();
                SetVScrollBar4Parameters();
                SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
                SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
                SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
                SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
                SetScrollBarPosition(vScrollBar4, vScrollBar4.Value);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
		//public void ReformatGroupingChanged(bool aClearGrid, int ag2VarSortKey, SortEnum ag2SortDir, int ag3VarSortKey, int ag3TimeSortKey, SortEnum ag3SortDir)
		public void ReformatGroupingChanged(bool aClearGrid, CubeWaferCoordinateList ag2SortCoordinates, SortEnum ag2SortDir, CubeWaferCoordinateList ag3SortCoordinates, SortEnum ag3SortDir)
		//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
		{
            try
            {
				//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				//Formatg2Grid(aClearGrid, ag2VarSortKey, ag2SortDir);
				//Formatg3Grid(aClearGrid, ag3VarSortKey, ag3TimeSortKey, ag3SortDir);
				Formatg2Grid(aClearGrid, ag2SortCoordinates, ag2SortDir);
				Formatg3Grid(aClearGrid, ag3SortCoordinates, ag3SortDir);
				//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
				//Begin Track #6064 - JScott - After making a change to Stock and navigating to the next page receive a Argument out of Range Exception

				g2.Visible = ((PagingGridTag)g2.Tag).Visible;
				g3.Visible = ((PagingGridTag)g3.Tag).Visible;

				//End Track #6064 - JScott - After making a change to Stock and navigating to the next page receive a Argument out of Range Exception
				Setg2Styles(false);
                Setg3Styles(false);
                SetHScrollBar2Parameters();
                SetHScrollBar3Parameters();
                SetScrollBarPosition(hScrollBar2, 0);
                SetScrollBarPosition(hScrollBar3, 0);
                LoadCurrentPages();
                ResizeCol2();
                ResizeCol3();
                ResizeRow1();
                CalcColSplitPosition3(false);
                SetColSplitPositions();
                CalcRowSplitPosition4(false);
                SetRowSplitPositions();

                // Reset Scroll bars due to change in split positions
                SetHScrollBar2Parameters();
                SetHScrollBar3Parameters();
                SetVScrollBar2Parameters();
                SetVScrollBar3Parameters();
                SetVScrollBar4Parameters();
                SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
                SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
                SetScrollBarPosition(vScrollBar2, vScrollBar2.Value);
                SetScrollBarPosition(vScrollBar3, vScrollBar3.Value);
                SetScrollBarPosition(vScrollBar4, vScrollBar4.Value);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

		public void ReformatSort()
		{
			try
			{
				LoadCurrentPages();
				//Begin TT#772 - JScott - OTS forecast review screen doesn't hold sizing when sorted
				//ResizeCol2();
                ResizeCol3();       //TT#2586 - MD - Weeks disappear from OTS Forecast when sorting - RBeck                                                                        
				//CalcColSplitPosition3(false);
				//SetColSplitPositions();

				//// Reset Scroll bars due to change in split positions
				//SetHScrollBar2Parameters();
				//SetHScrollBar3Parameters();
				//SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
				//SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
				//End TT#772 - JScott - OTS forecast review screen doesn't hold sizing when sorted
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
		//Begin TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 

		public void ReformatColumnMove()
		{
			try
			{
				ResizeCol2();
				ResizeCol3();
				CalcColSplitPosition3(false);
				SetColSplitPositions();

				// Reset Scroll bars due to change in split positions
				SetHScrollBar2Parameters();
				SetHScrollBar3Parameters();
				SetScrollBarPosition(hScrollBar2, hScrollBar2.Value);
				SetScrollBarPosition(hScrollBar3, hScrollBar3.Value);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End TT#898 - JScott - OTS Forecast Review-after moving column, then sorting column, layout changes. 
		#endregion

        #region Loading actual data into grids, including threading-populate

        /// <summary>
        /// This method is called by each process/event to load the current visible grid pages.
        /// </summary>

        private void LoadCurrentPages()
        {
            try
            {
                StopPageLoadThreads();

                ((PagingGridTag)g5.Tag).AllocatePageArray();
                ((PagingGridTag)g6.Tag).AllocatePageArray();
                ((PagingGridTag)g8.Tag).AllocatePageArray();
                ((PagingGridTag)g9.Tag).AllocatePageArray();
                ((PagingGridTag)g11.Tag).AllocatePageArray();
                ((PagingGridTag)g12.Tag).AllocatePageArray();

                if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g5);
                }
                if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g6);
                }
                if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g8);
                }
                if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g9);
                }
                if (g11.Rows.Count > 0 && g11.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g11);
                }
                if (g12.Rows.Count > 0 && g12.Cols.Count > 0)
                {
                    LoadCurrentGridPages(g12);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// This method is called by each process/event to load the surrounding grid pages.
        /// </summary>

        private void LoadSurroundingPages()
        {
            try
            {
                if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g5);
                }
                if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g6);
                }
                if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g8);
                }
                if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g9);
                }
                if (g11.Rows.Count > 0 && g11.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g11);
                }
                if (g12.Rows.Count > 0 && g12.Cols.Count > 0)
                {
                    LoadSurroundingGridPages(g12);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// This method is called by each the "export" process to load all grid pages.
        /// </summary>

        private void LoadAllPages()
        {
            try
            {
                StopPageLoadThreads();

                if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
                {
                    LoadAllGridPages(g5);
                }
                if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
                {
                    LoadAllGridPages(g6);
                }
                if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
                {
                    LoadAllGridPages(g8);
                }
                if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
                {
                    LoadAllGridPages(g9);
                }
                if (g11.Rows.Count > 0 && g11.Cols.Count > 0)
                {
                    LoadAllGridPages(g11);
                }
                if (g12.Rows.Count > 0 && g12.Cols.Count > 0)
                {
                    LoadAllGridPages(g12);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// This method is called by each the scroll bar "scroll" events to force the reload the current visible page.
        /// </summary>

        public void LoadCurrentGridPage(C1FlexGrid aGrid)
        {
            try
            {
                if (aGrid.Rows.Count > 0 && aGrid.Cols.Count > 0)
                {
                    LoadCurrentGridPages(aGrid);
                    LoadSurroundingGridPages(aGrid);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// This method is called by the page load routines to do the actual work of loading a current page.
        /// </summary>

        public void LoadCurrentGridPages(C1FlexGrid aGrid)
        {
            PagingGridTag gridTag;
            ArrayList pages;
            Cursor holdCursor;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;
                pages = gridTag.GetPagesToLoad(aGrid.TopRow, aGrid.LeftCol, Math.Min(aGrid.TopRow + ROWPAGESIZE - 1, aGrid.Rows.Count), Math.Min(aGrid.LeftCol + COLPAGESIZE - 1, aGrid.Cols.Count));

                if (pages.Count > 0)
                {
                    holdCursor = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;

                    try
                    {
                        foreach (Point page in pages)
                        {
                            gridTag.LoadPage(page);
                        }
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        Cursor.Current = holdCursor;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// This method is called by the page load routines to do the actual work of loading surrounding pages.
        /// </summary>

        public void LoadSurroundingGridPages(C1FlexGrid aGrid)
        {
            PagingGridTag gridTag;
            ArrayList pages;

            try
            {
                if (THREADED_GRID_LOAD)
                {
                    gridTag = (PagingGridTag)aGrid.Tag;
                    pages = gridTag.GetSurroundingPagesToLoad(aGrid.TopRow, aGrid.LeftCol, Math.Min(aGrid.TopRow + ROWPAGESIZE - 1, aGrid.Rows.Count), Math.Min(aGrid.LeftCol + COLPAGESIZE - 1, aGrid.Cols.Count));

                    foreach (Point page in pages)
                    {
                        gridTag.LoadPageInBackground(page);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// This method is called by the page load routines to do the actual work of loading all pages.
        /// </summary>

        public void LoadAllGridPages(C1FlexGrid aGrid)
        {
            PagingGridTag gridTag;
            ArrayList pages;
            Cursor holdCursor;

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;
                pages = gridTag.GetPagesToLoad(0, 0, aGrid.Rows.Count - 1, aGrid.Cols.Count - 1);

                if (pages.Count > 0)
                {
                    holdCursor = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;

                    try
                    {
                        foreach (Point page in pages)
                        {
                            gridTag.LoadPage(page);
                        }
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        Cursor.Current = holdCursor;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		public void ResetSplitters()
		{
			try
			{
				SetColSplitPositions();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}
		
		/// <summary>
        /// This method is the delegate that is passed to the page loader to load the data into the grid.
        /// </summary>

        public void GetCellRange(
            C1FlexGrid aGrid,
            int aStartRow,
            int aStartCol,
            int aEndRow,
            int aEndCol,
            int aPriority)
        {
            bool wait;
            PagingGridTag gridTag;
            C1FlexGrid rowHdrGrid;
            C1FlexGrid colHdrGrid;
            int i, j, x, y;
            int row, col;
			// Begin Track #6415 - stodd
            PlanWaferFlagCell[,] planWaferCellTable;
			// End Track #6415 - stodd
            CubeWafer cubeWafer;
            ColumnHeaderTag ColTag;
            RowHeaderTag RowTag;
			ComputationCellFlags planFlags = new ComputationCellFlags();

            try
            {
                gridTag = (PagingGridTag)aGrid.Tag;

                lock (_loadHash.SyncRoot)
                {
                    _loadHash.Add(System.Threading.Thread.CurrentThread.GetHashCode(), aPriority);
                }

                wait = true;

                while (wait)
                {
                    wait = false;

                    _pageLoadLock.AcquireWriterLock(-1);

                    try
                    {
                        if (_stopPageLoadThread)
                        {
                            throw new EndPageLoadThreadException();
                        }

                        lock (_loadHash.SyncRoot)
                        {
                            foreach (int priority in _loadHash.Values)
                            {
                                if (priority < aPriority)
                                {
                                    throw new WaitPageLoadException();
                                }
                            }

                            _loadHash.Remove(System.Threading.Thread.CurrentThread.GetHashCode());
                        }

                        if (aStartRow <= aEndRow && aStartCol <= aEndCol)
                        {
                            rowHdrGrid = gridTag.RowHeaderGrid;
                            colHdrGrid = gridTag.ColHeaderGrid;

                            //Create the CubeWafer to request data
                            cubeWafer = new CubeWafer();

                            //Fill CommonWaferCoordinateListGroup
                            cubeWafer.CommonWaferCoordinateList = _commonWaferCoordinateList;

                            //Fill ColWaferCoordinateListGroup
                            cubeWafer.ColWaferCoordinateListGroup.Clear();

                            for (i = aStartCol; i <= aEndCol; i++)
                            {
                                ColTag = (ColumnHeaderTag)colHdrGrid.Cols[i].UserData;
                                if (ColTag != null)
                                {
									cubeWafer.ColWaferCoordinateListGroup.Add(ColTag.CubeWaferCoorList);
								}
                            }

                            //Fill RowWaferCoordinateListGroup

                            cubeWafer.RowWaferCoordinateListGroup.Clear();
                            for (i = aStartRow; i <= aEndRow; i++)
                            {
                                RowTag = (RowHeaderTag)rowHdrGrid.Rows[i].UserData;
                                if (RowTag != null)
                                {
									cubeWafer.RowWaferCoordinateListGroup.Add(RowTag.CubeWaferCoorList);
								}
                            }

                            if (cubeWafer.ColWaferCoordinateListGroup.Count > 0 && cubeWafer.RowWaferCoordinateListGroup.Count > 0)
                            {
                                // Retreive array of values

								//Begin Modification - JScott - Add Scaling Decimals
								//planWaferCellTable = _planCubeGroup.GetPlanWaferCellValues(cubeWafer, ((NumericComboObject)cboUnitScaling.SelectedItem).Value, ((NumericComboObject)cboDollarScaling.SelectedItem).Value);
								planWaferCellTable = _planCubeGroup.GetPlanWaferCellValues(cubeWafer, ((ComboObject)cboUnitScaling.SelectedItem).Value, ((ComboObject)cboDollarScaling.SelectedItem).Value);
								//End Modification - JScott - Add Scaling Decimals

                                // Load Grid with values

                                aGrid.Redraw = false;

                                try
                                {
                                    x = 0;

                                    for (i = aStartRow; i <= aEndRow; i++)
                                    {
                                        RowTag = (RowHeaderTag)rowHdrGrid.Rows[i].UserData;

                                        if (RowTag != null)
                                        {
                                            y = 0;

                                            for (j = aStartCol; j <= aEndCol; j++)
                                            {
                                                ColTag = (ColumnHeaderTag)colHdrGrid.Cols[j].UserData;

                                                if (ColTag != null)
                                                {
                                                    if (_stopPageLoadThread)
                                                    {
                                                        throw new EndPageLoadThreadException();
                                                    }

                                                    row = ((RowHeaderTag)rowHdrGrid.Rows[i].UserData).Order;
                                                    col = ((ColumnHeaderTag)colHdrGrid.Cols[j].UserData).Order;

                                                    if (RowTag.LoadData)
                                                    {
                                                        if (planWaferCellTable[x, y] != null)
                                                        {
                                                            aGrid[i, j] = planWaferCellTable[x, y].ValueAsString;
                                                            planFlags = planWaferCellTable[x, y].Flags;

                                                            if (!_gridData[gridTag.GridId][row, col].CellFlagsInited ||
																planFlags.Flags != _gridData[gridTag.GridId][row, col].ComputationCellFlags.Flags ||
                                                                (planWaferCellTable[x, y] != null &&
                                                                planWaferCellTable[x, y].isValueNegative != _gridData[gridTag.GridId][row, col].isCellNegative))
                                                            {
																_gridData[gridTag.GridId][row, col].ComputationCellFlags = planFlags;
                                                                _gridData[gridTag.GridId][row, col].isCellNegative = planWaferCellTable[x, y].isValueNegative;
                                                                ChangeCellStyles(aGrid, _gridData[gridTag.GridId][row, col], i, j);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            aGrid[i, j] = NULL_DATA_STRING;
                                                            planFlags.Clear();
                                                            ComputationCellFlagValues.isNull(ref planFlags, true);

                                                            if (!_gridData[gridTag.GridId][row, col].CellFlagsInited ||
																planFlags.Flags != _gridData[gridTag.GridId][row, col].ComputationCellFlags.Flags)
                                                            {
																_gridData[gridTag.GridId][row, col].ComputationCellFlags = planFlags;
                                                                ChangeCellStyles(aGrid, _gridData[gridTag.GridId][row, col], i, j);
                                                            }
                                                        }

                                                        SetLockPicture(aGrid, planFlags, i, j);
                                                    }
                                                    else
                                                    {
                                                        planFlags.Clear();
														ComputationCellFlagValues.isNull(ref planFlags, true);
														_gridData[gridTag.GridId][row, col].ComputationCellFlags = planFlags;
                                                    }

                                                    y++;
                                                }
                                            }

                                            x++;
                                        }
                                    }
                                }
                                catch (Exception exc)
                                {
                                    string message = exc.ToString();
                                    throw;
                                }
                                finally
                                {
                                    aGrid.Redraw = _currentRedrawState;
                                }
                            }
                        }
                    }
                    catch (WaitPageLoadException)
                    {
                        wait = true;
                    }
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                    finally
                    {
                        _pageLoadLock.ReleaseWriterLock();
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        void ChangeCellStyles(C1FlexGrid aGrid, CellTag aCellTag, int aRow, int aCol)
        {
            try
            {
				if (PlanCellFlagValues.isIneligible(aCellTag.ComputationCellFlags))
                {
                    switch (aGrid.Rows[aRow].Style.Name)
                    {
                        case "Style1":
                            aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Ineligible1"]);
                            break;
                        case "Style2":
                            aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Ineligible2"]);
                            break;
                        default:
                            throw new Exception("Invalid row style");
                    }
                }
				else if (ComputationCellFlagValues.isLocked(aCellTag.ComputationCellFlags))
                {
                    switch (aGrid.Rows[aRow].Style.Name)
                    {
                        case "Style1":
                            aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Locked1"]);
                            break;
                        case "Style2":
                            aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Locked2"]);
                            break;
                        default:
                            throw new Exception("Invalid row style");
                    }
                }
                else if (aCellTag.isCellNegative)
                {
					if (PlanCellFlagValues.isClosed(aCellTag.ComputationCellFlags) ||
						ComputationCellFlagValues.isDisplayOnly(aCellTag.ComputationCellFlags) ||
						ComputationCellFlagValues.isNull(aCellTag.ComputationCellFlags) ||
						PlanCellFlagValues.isProtected(aCellTag.ComputationCellFlags) ||
						ComputationCellFlagValues.isHidden(aCellTag.ComputationCellFlags) ||
						ComputationCellFlagValues.isReadOnly(aCellTag.ComputationCellFlags))
                    {
                        switch (aGrid.Rows[aRow].Style.Name)
                        {
                            case "Style1":
                                aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Negative1"]);
                                break;
                            case "Style2":
                                aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Negative2"]);
                                break;
                            default:
                                throw new Exception("Invalid row style");
                        }
                    }
                    else
                    {
                        switch (aGrid.Rows[aRow].Style.Name)
                        {
                            case "Style1":
                                aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["NegativeEditable1"]);
                                break;
                            case "Style2":
                                aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["NegativeEditable2"]);
                                break;
                            default:
                                throw new Exception("Invalid row style");
                        }
                    }
                }
				else if (!PlanCellFlagValues.isClosed(aCellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isDisplayOnly(aCellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isNull(aCellTag.ComputationCellFlags) &&
						!PlanCellFlagValues.isProtected(aCellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isHidden(aCellTag.ComputationCellFlags) &&
						!ComputationCellFlagValues.isReadOnly(aCellTag.ComputationCellFlags))
                {
                    switch (aGrid.Rows[aRow].Style.Name)
                    {
                        case "Style1":
                            aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable1"]);
                            break;
                        case "Style2":
                            aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable2"]);
                            break;
                        default:
                            throw new Exception("Invalid row style");
                    }
                }
                else
                {
                    aGrid.SetCellStyle(aRow, aCol, (CellStyle)null);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

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

        private void StopPageLoadThreads()
        {
            try
            {
                _stopPageLoadThread = true;
                ((PagingGridTag)g5.Tag).WaitForPageLoads();
                ((PagingGridTag)g6.Tag).WaitForPageLoads();
                ((PagingGridTag)g8.Tag).WaitForPageLoads();
                ((PagingGridTag)g9.Tag).WaitForPageLoads();
                ((PagingGridTag)g11.Tag).WaitForPageLoads();
                ((PagingGridTag)g12.Tag).WaitForPageLoads();
                _stopPageLoadThread = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        #endregion

        #region Closing/Closed event handlers

        private void PlanView_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                StopPageLoadThreads();

				//Begin Track #6370 -- When trying to close a store plan it can take up to 5 minutes or more
				if (_planCubeGroup.UserChanged)
				{
				//End Track #6370 -- When trying to close a store plan it can take up to 5 minutes or more
					switch (_openParms.PlanSessionType)
					{
						case ePlanSessionType.StoreSingleLevel:
							CheckStoreSingleLevel(e);
							break;
						case ePlanSessionType.ChainSingleLevel:
							CheckChainSingleLevel(e);
							break;
						case ePlanSessionType.StoreMultiLevel:
							CheckStoreMultiLevel(e);
							break;
						case ePlanSessionType.ChainMultiLevel:
							CheckChainMultiLevel(e);
							break;
						default:
							throw new Exception("Function not currently supported.");
					}
				//Begin Track #6370 -- When trying to close a store plan it can take up to 5 minutes or more
				}
				//End Track #6370 -- When trying to close a store plan it can take up to 5 minutes or more

                if (!e.Cancel)
                {
                    _planCubeGroup.CloseCubeGroup();
                    _planCubeGroup.Dispose();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CheckStoreSingleLevel(CancelEventArgs e)
        {
            bool storeChanged;
            bool chainChanged;
            PlanSaveParms planSaveParms;
            DialogResult result;

            try
            {
                storeChanged = ((StorePlanMaintCubeGroup)_planCubeGroup).hasStoreCubeChanged();
                chainChanged = ((StorePlanMaintCubeGroup)_planCubeGroup).hasChainCubeChanged();

                if (storeChanged || chainChanged)
                {
                    if (storeChanged && chainChanged)
                    {
                        result = MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_SaveStoreAndChain), _windowName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    }
                    else if (storeChanged)
                    {
                        result = MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_SaveStore), _windowName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        result = MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_SaveChain), _windowName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    }

                    switch (result)
                    {
                        case DialogResult.Yes:

                            try
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                RecomputePlanCubes();

                                planSaveParms = new PlanSaveParms();

                                if (storeChanged)
                                {
                                    planSaveParms.SaveStoreHighLevel = true;
                                    planSaveParms.StoreHighLevelNodeRID = _openParms.StoreHLPlanProfile.NodeProfile.Key;
                                    planSaveParms.StoreHighLevelVersionRID = _openParms.StoreHLPlanProfile.VersionProfile.Key;
                                    planSaveParms.StoreHighLevelDateRangeRID = _openParms.DateRangeProfile.Key;
                                }

                                if (chainChanged)
                                {
                                    planSaveParms.SaveChainHighLevel = true;
                                    planSaveParms.ChainHighLevelNodeRID = _openParms.ChainHLPlanProfile.NodeProfile.Key;
                                    planSaveParms.ChainHighLevelVersionRID = _openParms.ChainHLPlanProfile.VersionProfile.Key;
                                    planSaveParms.ChainHighLevelDateRangeRID = _openParms.DateRangeProfile.Key;
                                    planSaveParms.SaveHighLevelAllStoreAsChain = false;
                                }

                                _planCubeGroup.SaveCubeGroup(planSaveParms);
                            }
                            catch (PlanInUseException)
                            {
                                e.Cancel = true;
                            }
                            catch (Exception exc)
                            {
                                HandleExceptions(exc);
                                e.Cancel = true;
                            }
                            finally
                            {
                                Cursor.Current = Cursors.Default;
                            }

                            break;

                        case DialogResult.No:
                            break;

                        case DialogResult.Cancel:
                            e.Cancel = true;
                            return;

                        default:
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CheckChainSingleLevel(CancelEventArgs e)
        {
            bool chainChanged;
            PlanSaveParms planSaveParms;
            DialogResult result;

            try
            {
                chainChanged = ((ChainPlanMaintCubeGroup)_planCubeGroup).hasChainCubeChanged();

                if (chainChanged)
                {
                    result = MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_SaveChain), _windowName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    switch (result)
                    {
                        case DialogResult.Yes:

                            try
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                RecomputePlanCubes();

                                planSaveParms = new PlanSaveParms();

                                planSaveParms.SaveChainHighLevel = true;
                                planSaveParms.ChainHighLevelNodeRID = _openParms.ChainHLPlanProfile.NodeProfile.Key;
                                planSaveParms.ChainHighLevelVersionRID = _openParms.ChainHLPlanProfile.VersionProfile.Key;
                                planSaveParms.ChainHighLevelDateRangeRID = _openParms.DateRangeProfile.Key;
                                planSaveParms.SaveHighLevelAllStoreAsChain = false;

                                _planCubeGroup.SaveCubeGroup(planSaveParms);
                            }
                            catch (PlanInUseException)
                            {
                                e.Cancel = true;
                            }
                            catch (Exception exc)
                            {
                                HandleExceptions(exc);
                                e.Cancel = true;
                            }
                            finally
                            {
                                Cursor.Current = Cursors.Default;
                            }

                            break;

                        case DialogResult.No:
                            break;

                        case DialogResult.Cancel:
                            e.Cancel = true;
                            return;

                        default:
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CheckStoreMultiLevel(CancelEventArgs e)
        {
            bool storeHighLevelChanged;
            bool chainHighLevelChanged;
            bool storeLowLevelChanged;
            bool chainLowLevelChanged;
            PlanSaveParms planSaveParms;
            DialogResult result;

            try
            {
                storeHighLevelChanged = ((StoreMultiLevelPlanMaintCubeGroup)_planCubeGroup).hasStoreHighLevelCubeChanged();
                chainHighLevelChanged = ((StoreMultiLevelPlanMaintCubeGroup)_planCubeGroup).hasChainHighLevelCubeChanged();
                storeLowLevelChanged = ((StoreMultiLevelPlanMaintCubeGroup)_planCubeGroup).hasStoreLowLevelCubeChanged();
                chainLowLevelChanged = ((StoreMultiLevelPlanMaintCubeGroup)_planCubeGroup).hasChainLowLevelCubeChanged();

                if (storeHighLevelChanged || chainHighLevelChanged ||
                    storeLowLevelChanged || chainLowLevelChanged)
                {
                    StringBuilder text = new StringBuilder(_sab.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_pl_Save, false));
                    string changedTypes = string.Empty;
                    bool firstChange = true;
                    if (storeHighLevelChanged)
                    {
                        changedTypes = "Store high level plan";
                        firstChange = false;
                    }
                    if (chainHighLevelChanged)
                    {
                        if (!firstChange)
                        {
                            changedTypes += ", ";
                        }
                        changedTypes += "Chain high level plan";
                        firstChange = false;
                    }
                    if (storeLowLevelChanged)
                    {
                        if (!firstChange)
                        {
                            changedTypes += ", ";
                        }
                        changedTypes += "Store low level plan";
                        firstChange = false;
                    }
                    if (chainLowLevelChanged)
                    {
                        if (!firstChange)
                        {
                            changedTypes += ", ";
                        }
                        changedTypes += "Chain low level plan";
                    }

                    text.Replace("{0}", changedTypes);

                    result = MessageBox.Show(text.ToString(), _windowName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    switch (result)
                    {
                        case DialogResult.Yes:

                            try
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                RecomputePlanCubes();

                                planSaveParms = new PlanSaveParms();

                                if (storeHighLevelChanged)
                                {
                                    planSaveParms.SaveStoreHighLevel = true;
                                    planSaveParms.StoreHighLevelNodeRID = _openParms.StoreHLPlanProfile.NodeProfile.Key;
                                    planSaveParms.StoreHighLevelVersionRID = _openParms.StoreHLPlanProfile.VersionProfile.Key;
                                    planSaveParms.StoreHighLevelDateRangeRID = _openParms.DateRangeProfile.Key;
                                }

                                if (chainHighLevelChanged)
                                {
                                    planSaveParms.SaveChainHighLevel = true;
                                    planSaveParms.ChainHighLevelNodeRID = _openParms.ChainHLPlanProfile.NodeProfile.Key;
                                    planSaveParms.ChainHighLevelVersionRID = _openParms.ChainHLPlanProfile.VersionProfile.Key;
                                    planSaveParms.ChainHighLevelDateRangeRID = _openParms.DateRangeProfile.Key;
                                    planSaveParms.SaveHighLevelAllStoreAsChain = false;
                                }

                                if (storeLowLevelChanged)
                                {
                                    planSaveParms.SaveStoreLowLevel = true;
                                }

                                if (chainLowLevelChanged)
                                {
                                    planSaveParms.SaveChainLowLevel = true;
                                }

                                _planCubeGroup.SaveCubeGroup(planSaveParms);
                            }
                            catch (PlanInUseException)
                            {
                                e.Cancel = true;
                            }
                            catch (Exception exc)
                            {
                                HandleExceptions(exc);
                                e.Cancel = true;
                            }
                            finally
                            {
                                Cursor.Current = Cursors.Default;
                            }

                            break;

                        case DialogResult.No:
                            break;

                        case DialogResult.Cancel:
                            e.Cancel = true;
                            return;

                        default:
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void CheckChainMultiLevel(CancelEventArgs e)
        {
            bool chainHighLevelChanged;
            bool chainLowLevelChanged;
            PlanSaveParms planSaveParms;
            DialogResult result;

            try
            {
                chainHighLevelChanged = ((ChainMultiLevelPlanMaintCubeGroup)_planCubeGroup).hasChainHighLevelCubeChanged();
                chainLowLevelChanged = ((ChainMultiLevelPlanMaintCubeGroup)_planCubeGroup).hasChainLowLevelCubeChanged();

                if (chainHighLevelChanged || chainLowLevelChanged)
                {
                    StringBuilder text = new StringBuilder(_sab.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_pl_Save, false));
                    string changedTypes = string.Empty;
                    bool firstChange = true;
                    if (chainHighLevelChanged)
                    {
                        changedTypes += "Chain high level plan";
                        firstChange = false;
                    }
                    if (chainLowLevelChanged)
                    {
                        if (!firstChange)
                        {
                            changedTypes += ", ";
                        }
                        changedTypes += "Chain low level plan";
                    }

                    text.Replace("{0}", changedTypes);

                    result = MessageBox.Show(text.ToString(), _windowName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    switch (result)
                    {
                        case DialogResult.Yes:

                            try
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                RecomputePlanCubes();

                                planSaveParms = new PlanSaveParms();

                                if (chainHighLevelChanged)
                                {
                                    planSaveParms.SaveChainHighLevel = true;
                                    planSaveParms.ChainHighLevelNodeRID = _openParms.ChainHLPlanProfile.NodeProfile.Key;
                                    planSaveParms.ChainHighLevelVersionRID = _openParms.ChainHLPlanProfile.VersionProfile.Key;
                                    planSaveParms.ChainHighLevelDateRangeRID = _openParms.DateRangeProfile.Key;
                                    planSaveParms.SaveHighLevelAllStoreAsChain = false;
                                }

                                if (chainLowLevelChanged)
                                {
                                    planSaveParms.SaveChainLowLevel = true;
                                }

                                _planCubeGroup.SaveCubeGroup(planSaveParms);
                            }
                            catch (PlanInUseException)
                            {
                                e.Cancel = true;
                            }
                            catch (Exception exc)
                            {
                                HandleExceptions(exc);
                                e.Cancel = true;
                            }
                            finally
                            {
                                Cursor.Current = Cursors.Default;
                            }

                            break;

                        case DialogResult.No:
                            break;

                        case DialogResult.Cancel:
                            e.Cancel = true;
                            return;

                        default:
                            break;
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

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void cboStoreAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboStoreAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void cboFilter_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        //BEGIN TT#6-MD-VStuart - Single Store Select
        private void cboFilter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Testing vstuart
            FilterNameCombo filterNameCbo;

            try
            {
                filterNameCbo = (FilterNameCombo)cboFilter.SelectedItem;
                // Begin TT#5432 - JSmith - Store Filters in OTS Forecast Not Displaying
                bool filterChanged = false;
                // End TT#5432 - JSmith - Store Filters in OTS Forecast Not Displaying

                if (cboFilter.SelectedIndex != -1)
                {
                    //BEGIN TT#6-MD-VStuart - Single Store Select
                    if (filterNameCbo.FilterName == "(None)")
                    {
                        // Begin TT#5432 - JSmith - Store Filters in OTS Forecast Not Displaying
                        //_lastFilterValue = 0; //This value is changed to force 'None' to get all stores.
                        if (_lastFilterValue != 0)
                        {
                            filterChanged = true;
                            _lastFilterValue = 0; //This value is changed to force 'None' to get all stores.
                        }
                        // End TT#5432 - JSmith - Store Filters in OTS Forecast Not Displaying
                    }
                    else
                    {
                        // Begin TT#5432 - JSmith - Store Filters in OTS Forecast Not Displaying
                        //_lastFilterValue = cboFilter.SelectedIndex;
                        if (filterNameCbo.FilterRID != _lastFilterValue)
                        {
                            filterChanged = true;
                            _lastFilterValue = filterNameCbo.FilterRID;
                        }
                        // End TT#5432 - JSmith - Store Filters in OTS Forecast Not Displaying
                    }

                    // Begin TT#5432 - JSmith - Store Filters in OTS Forecast Not Displaying
                    //if (!_bindingFilter && (filterNameCbo.FilterRID != _lastFilterValue))
                    if (!_bindingFilter && filterChanged)
                    // End TT#5432 - JSmith - Store Filters in OTS Forecast Not Displaying
                    //END TT#6-MD-VStuart - Single Store Select
                    {
                        if (!_formLoading)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            SetGridRedraws(false);

                            try
                            {
                                StopPageLoadThreads();

                                //Begin Track #5006 - JScott - Display Low-levels one at a time
                                //if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel || _openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
                                if (_currentPlanSessionType == ePlanSessionType.StoreSingleLevel || _currentPlanSessionType == ePlanSessionType.StoreMultiLevel)
                                //End Track #5006 - JScott - Display Low-levels one at a time
                                {


                                    _planCubeGroup.SetStoreFilter(((FilterNameCombo)cboFilter.SelectedItem).FilterRID, _planCubeGroup);

                                    //BEGIN TT#6-MD-VStuart - Single Store Select
                                    if ((((FilterNameCombo)cboFilter.SelectedItem).FilterRID) == -2)
                                    {
                                        
                                        ProfileList _singleStoreProfileList = new ProfileList(eProfileType.Store);
                                        if (!String.IsNullOrEmpty(_openParms.StoreId) && _openParms.StoreId != "(None)")
                                        {
                                            _storeProfileList.Clear();
                                            _singleStoreProfileList.Clear();
                                            StoreProfile sp = StoreMgmt.StoreProfile_Get(_openParms.StoreId); //_sab.StoreServerSession.GetStoreProfile(_openParms.StoreId);
                                            _storeProfileList.Add(sp);
                                            _singleStoreProfileList.Add(sp);
                                        }
                                    }
                             
                                    else
                                    {
                                            _storeProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.Store);
                                    }
                                    //END TT#6-MD-VStuart - Single Store Select

                                    //_storeProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.Store);

                                    if (_storeProfileList.Count == 0)
                                    {
                                        MessageBox.Show("Applied filter(s) have resulted in no displayable Stores.", "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }

                                    _workingDetailProfileList = new ProfileList(eProfileType.Store);
                                    BuildWorkingStoreList(_lastAttributeSetValue, _workingDetailProfileList);
                                    ReformatRowsChanged(false);

                                    // Begin TT#5432 - JSmith - Store Filters in OTS Forecast Not Displaying
                                    //_lastFilterValue = cboFilter.SelectedIndex;
                                    // End TT#5432 - JSmith - Store Filters in OTS Forecast Not Displaying
                                }
                            }
                            //Begin TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed
                            catch (MIDException ex)
                            {
                                HandleMIDException(ex);
                            }
                            //End TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed
                            catch (Exception exc)
                            {
                                HandleExceptions(exc);
                            }
                            finally
                            {
                                SetGridRedraws(true);
                                LoadSurroundingPages();
                                Cursor.Current = Cursors.Default;
                                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                                //g6.Focus();
                                // End TT#301-MD - JSmith - Controls are not functioning properly
                            }
                        }
                    }

                    if (((FilterNameCombo)cboFilter.SelectedItem).FilterRID == -1)
                    {
                        cboFilter.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }

        }
        //END TT#6-MD-VStuart - Single Store Select

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        void cboFilter_DropDownClosed(object sender, System.EventArgs e)
        {
            g6.Focus();
        }
        // End TT#301-MD - JSmith - Controls are not functioning properly

        //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
        /// <summary>
        /// Populate all values of the Store_Group_Levels (Attribute Sets)
        /// (based on key from Store_Group) of the cboStoreAttribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboStoreAttribute_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if ((!_bindingAttribute && (int)cboStoreAttribute.SelectedValue != _lastAttributeValue) ||
                    (_bindingAttribute && (int)cboStoreAttribute.SelectedValue == _lastAttributeValue))
                {
                    if (!_formLoading)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        StopPageLoadThreads();

                        if (_planCubeGroup.GetType() == typeof(StoreMultiLevelPlanMaintCubeGroup))
                        {
                            ((StoreMultiLevelPlanMaintCubeGroup)_planCubeGroup).SetStoreGroup(new StoreGroupProfile(((StoreGroupListViewProfile)_storeGroupListViewProfileList.FindKey((int)cboStoreAttribute.SelectedValue)).Key));
                        }
                        else
                        {
                            ((StorePlanMaintCubeGroup)_planCubeGroup).SetStoreGroup(new StoreGroupProfile(((StoreGroupListViewProfile)_storeGroupListViewProfileList.FindKey((int)cboStoreAttribute.SelectedValue)).Key));
                        }
                        _storeGroupLevelProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);

                        _selectableStoreAttributeHeaders = new ArrayList();
                        foreach (StoreGroupLevelProfile strGrpLvlProf in _storeGroupLevelProfileList)
                        {
                            //Begin Track #6261 - JScott - Sets in Set summary section are out of order
                            //_selectableStoreAttributeHeaders.Add(new RowColProfileHeader(strGrpLvlProf.Name, true, 0, strGrpLvlProf));
                            _selectableStoreAttributeHeaders.Add(new RowColProfileHeader(strGrpLvlProf.Name, true, strGrpLvlProf.Sequence, strGrpLvlProf));
                            //End Track #6261 - JScott - Sets in Set summary section are out of order
                        }

                        Cursor.Current = Cursors.Default;
                    }

                    BindStoreAttrSetComboBox();

                    _lastAttributeValue = (int)cboStoreAttribute.SelectedValue;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }
        //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        void cboStoreAttribute_DropDownClosed(object sender, System.EventArgs e)
        {
            g6.Focus();
        }
        // End TT#301-MD - JSmith - Controls are not functioning properly

        //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
        private void cboAttributeSet_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if ((!_bindingAttributeSet && (int)cboAttributeSet.SelectedValue != _lastAttributeSetValue) ||
                    (_bindingAttributeSet && (int)cboAttributeSet.SelectedValue == _lastAttributeSetValue))
                {
                    try
                    {
                        _workingDetailProfileList = new ProfileList(eProfileType.Store);
                        BuildWorkingStoreList((int)cboAttributeSet.SelectedValue, _workingDetailProfileList);
                    }
                    catch (Exception exc)
                    {
                        HandleExceptions(exc);
                    }

                    if (!_formLoading)
                    {
                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            SetGridRedraws(false);
                            StopPageLoadThreads();
                            ReformatAttributeChanged(false);
                            HighlightActiveAttributeSet();

                            _lastAttributeSetValue = (int)cboAttributeSet.SelectedValue;
                        }
                        catch (Exception exc)
                        {
                            HandleExceptions(exc);
                        }
                        finally
                        {
                            SetGridRedraws(true);
                            LoadSurroundingPages();
                            Cursor.Current = Cursors.Default;
                            // Begin TT#301-MD - JSmith - Controls are not functioning properly
                            //g6.Focus();
                            // End TT#301-MD - JSmith - Controls are not functioning properly
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        void cboAttributeSet_DropDownClosed(object sender, System.EventArgs e)
        {
            g6.Focus();
        }
        // End TT#301-MD - JSmith - Controls are not functioning properly

        /// <summary>
        /// Populate all values of the Store_Group_Levels (Attribute Sets)
        /// (based on key from Store_Group) of the cboStoreAttribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboView_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int selectedValue;
            DataRow row;

            try
            {
                row = ((DataRowView)cboView.SelectedItem).Row;

                selectedValue = Convert.ToInt32(row["VIEW_RID"], CultureInfo.CurrentUICulture);

                if ((!_bindingView && selectedValue != _lastViewValue) ||
                    (_bindingView && selectedValue == _lastViewValue))
                {
                    if (!_formLoading)
                    {
                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            _openParms.ViewRID = selectedValue;
                            _openParms.ViewName = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
                            _openParms.ViewUserID = Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture);

                            LoadView();
                            //Begin Track #5969 - JScott - Views don't observe "show" (wks, mos, qts,sea) properties
                            BuildTimeHeaders();
                            //End Track #5969 - JScott - Views don't observe "show" (wks, mos, qts,sea) properties

                            SetGridRedraws(false);
                            StopPageLoadThreads();
                            ReformatRowsChanged(true);

                            _lastViewValue = selectedValue;
                        }
                        catch (Exception exc)
                        {
                            HandleExceptions(exc);
                        }
                        finally
                        {
                            SetGridRedraws(true);
                            LoadSurroundingPages();
                            Cursor.Current = Cursors.Default;
                            // Begin TT#301-MD - JSmith - Controls are not functioning properly
                            //g6.Focus();
                            // End TT#301-MD - JSmith - Controls are not functioning properly
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }
        //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        void cboView_DropDownClosed(object sender, System.EventArgs e)
        {
            g6.Focus();
        }
        // End TT#301-MD - JSmith - Controls are not functioning properly

        //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
        private void cboUnitScaling_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Begin Modification - JScott - Add Scaling Decimals
            //NumericComboObject comboObj;
            ComboObject comboObj;
            //End Modification - JScott - Add Scaling Decimals

            try
            {
                //Begin Modification - JScott - Add Scaling Decimals
                //comboObj = (NumericComboObject)cboUnitScaling.SelectedItem;
                comboObj = (ComboObject)cboUnitScaling.SelectedItem;
                //End Modification - JScott - Add Scaling Decimals

                if (comboObj != null && !_formLoading && !_bindingUnitScaling && comboObj.Key != _lastUnitScalingValue)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        SetGridRedraws(false);
                        StopPageLoadThreads();
                        LoadCurrentPages();
                        ResizeCol1();
                        ResizeCol2();
                        ResizeCol3();
                        CalcColSplitPosition2(false);
                        CalcColSplitPosition3(false);
                        SetColSplitPositions();

                        _lastUnitScalingValue = comboObj.Key;
                    }
                    catch (Exception exc)
                    {
                        HandleExceptions(exc);
                    }
                    finally
                    {
                        SetGridRedraws(true);
                        LoadSurroundingPages();
                        Cursor.Current = Cursors.Default;
                        // Begin TT#301-MD - JSmith - Controls are not functioning properly
                        //g6.Focus();
                        // End TT#301-MD - JSmith - Controls are not functioning properly
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }
        //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        void cboUnitScaling_DropDownClosed(object sender, System.EventArgs e)
        {
            g6.Focus();
        }
        // End TT#301-MD - JSmith - Controls are not functioning properly

        //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
        private void cboDollarScaling_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Begin Modification - JScott - Add Scaling Decimals
            //NumericComboObject comboObj;
            ComboObject comboObj;
            //End Modification - JScott - Add Scaling Decimals

            try
            {
                //Begin Modification - JScott - Add Scaling Decimals
                //comboObj = (NumericComboObject)cboDollarScaling.SelectedItem;
                comboObj = (ComboObject)cboDollarScaling.SelectedItem;
                //End Modification - JScott - Add Scaling Decimals

                if (comboObj != null && !_formLoading && !_bindingDollarScaling && comboObj.Key != _lastDollarScalingValue)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        SetGridRedraws(false);
                        StopPageLoadThreads();
                        LoadCurrentPages();
                        ResizeCol1();
                        ResizeCol2();
                        ResizeCol3();
                        CalcColSplitPosition2(false);
                        CalcColSplitPosition3(false);
                        SetColSplitPositions();

                        _lastDollarScalingValue = comboObj.Key;
                    }
                    catch (Exception exc)
                    {
                        HandleExceptions(exc);
                    }
                    finally
                    {
                        SetGridRedraws(true);
                        LoadSurroundingPages();
                        Cursor.Current = Cursors.Default;
                        // Begin TT#301-MD - JSmith - Controls are not functioning properly
                        //g6.Focus();
                        // End TT#301-MD - JSmith - Controls are not functioning properly
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        void cboDollarScaling_DropDownClosed(object sender, System.EventArgs e)
        {
            g6.Focus();
        }
        // End TT#301-MD - JSmith - Controls are not functioning properly
        //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly     
        // End Track #4872
    }

    #region Class/Structure Declarations

    public enum GroupedBy
    {
        GroupedByTime,
        GroupedByVariable
    }

    public delegate void ScrollBarValueChanged(int aNewValue, bool aLoadValues);

    public class ExcelGridInfo
    {
        public bool Negative;

        public ExcelGridInfo()
        {
            Negative = false;
        }
    }

    public class SplitterTag
    {
        public bool Locked;

        public SplitterTag()
        {
            Locked = false;
        }
    }

    /// <summary>
    /// Class that defines the contents of the FilterName combo box.
    /// </summary>

    public class LevelNameCombo
    {
        //=======
        // FIELDS
        //=======

        private PlanProfile _planProf;
        private int _key;
        private string _displayName;

        //=============
        // CONSTRUCTORS
        //=============

        public LevelNameCombo(int aKey, string aDisplayName)
        {
            _key = aKey;
            _displayName = aDisplayName;

            _planProf = null;
        }

        public LevelNameCombo(PlanProfile aPlanProfile)
        {
            _planProf = aPlanProfile;

            _key = aPlanProfile.NodeProfile.Key;
            _displayName = aPlanProfile.NodeProfile.Text;
        }

        //===========
        // PROPERTIES
        //===========

        public PlanProfile PlanProfile
        {
            get
            {
                return _planProf;
            }
        }

        public int Key
        {
            get
            {
                return _key;
            }
        }

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        //========
        // METHODS
        //========

        override public string ToString()
        {
            return _displayName;
        }

        override public bool Equals(object obj)
        {
            if (((LevelNameCombo)obj)._key == _key)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        override public int GetHashCode()
        {
            return _key;
        }
    }

    /// <summary>
    /// Class that is used to store the RowHeaderTag objects for a grid.
    /// </summary>

    public class GridRowList
    {
        //=======
        // FIELDS
        //=======

        private bool _rowsPerGroupInited;
        private int _rowsPerGroup;
        private ArrayList _gridRowList;

        //=============
        // CONSTRUCTORS
        //=============

        public GridRowList()
        {
            _rowsPerGroupInited = false;
            _rowsPerGroup = 0;
            _gridRowList = new ArrayList();
        }

        public GridRowList(int aRowsPerGroup)
        {
            _rowsPerGroupInited = true;
            _rowsPerGroup = aRowsPerGroup;
            _gridRowList = new ArrayList();
        }

        //===========
        // PROPERTIES
        //===========

        public int Count
        {
            get
            {
                return _gridRowList.Count;
            }
        }

        public int RowsPerGroup
        {
            get
            {
                return _rowsPerGroup;
            }
            set
            {
                _rowsPerGroupInited = true;
                _rowsPerGroup = value;
            }
        }
        // Begin TT#609 - RMatelic - OTS Forecast Chain Ladder View
        public ArrayList GridRowListPublic
        {
            get
            {
                return _gridRowList;
            }
        }
        // End TT#609
        //========
        // METHODS
        //========

        public void Add(RowHeaderTag aRowHeaderTag)
        {
            try
            {
                Add(aRowHeaderTag, false);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void Add(RowHeaderTag aRowHeaderTag, bool aFirstInGroup)
        {
            try
            {
                if (aFirstInGroup && _gridRowList.Count > 0)
                {
                    AddBorderToLastVisibleRow();
                    FillGridToGroupSize();
                }

                _gridRowList.Add(aRowHeaderTag);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void BuildGridRows(C1FlexGrid aGrid, int aFixedRows)
        {
            int i;
            int j;

            try
            {
                AddBorderToLastVisibleRow();
                FillGridToGroupSize();

                aGrid.Rows.Count = _gridRowList.Count;
                aGrid.Rows.Fixed = aFixedRows;

                i = 0;
                j = 0;

                foreach (RowHeaderTag rowHdrTag in _gridRowList)
                {
                    if (rowHdrTag != null)
                    {
                        aGrid.Rows[i].Visible = true;
                        aGrid.Rows[i].UserData = rowHdrTag;
                        aGrid.SetData(i, 0, rowHdrTag.RowHeading);

                        j++;
                    }
                    else
                    {
                        aGrid.Rows[i].Visible = false;
                    }

                    i++;
                }

                ((PagingGridTag)aGrid.Tag).VisibleRowCount = j;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		private void AddBorderToLastVisibleRow()
		{
			int i;

			try
			{
				for (i = _gridRowList.Count - 1; i > -1; i--)
				{
					if (_gridRowList[i] != null)
					{
						((RowHeaderTag)_gridRowList[i]).DrawBorder = true;
						break;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}


        private void FillGridToGroupSize()
        {
            try
            {
                if (_rowsPerGroupInited)
                {
                    while (_gridRowList.Count % _rowsPerGroup != 0)
                    {
                        _gridRowList.Add(null);
                    }
                }
                else
                {
                    _rowsPerGroupInited = true;
                    _rowsPerGroup = _gridRowList.Count;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

    /// <summary>
    /// Class that defines a sort object.
    /// </summary>

    public class GridSortEntry
    {
        //=======
        // FIELDS
        //=======

        private C1.Win.C1FlexGrid.Row _row;
        private IComparable[] _keys;
        private IComparable _defaultKey;
        private int _defaultSequence;

        //=============
        // CONSTRUCTORS
        //=============

        public GridSortEntry(C1.Win.C1FlexGrid.Row aRow, double[] aKeys, IComparable aDefaultKey, int aDefaultSequence)
        {
            _row = aRow;
            _keys = (IComparable[])aKeys.Clone();
            _defaultKey = aDefaultKey;
            _defaultSequence = aDefaultSequence;
        }

        public GridSortEntry(C1.Win.C1FlexGrid.Row aRow, string[] aKeys, IComparable aDefaultKey, int aDefaultSequence)
        {
            _row = aRow;
            _keys = (IComparable[])aKeys.Clone();
            _defaultKey = aDefaultKey;
            _defaultSequence = aDefaultSequence;
        }

        public GridSortEntry(C1.Win.C1FlexGrid.Row aRow, ArrayList aKeys, IComparable aDefaultKey, int aDefaultSequence)
        {
            _row = aRow;
            _keys = (IComparable[])aKeys.ToArray(typeof(IComparable));
            _defaultKey = aDefaultKey;
            _defaultSequence = aDefaultSequence;
        }

        //===========
        // PROPERTIES
        //===========

        public int RowIndex
        {
            get
            {
                return _row.Index;
            }
        }

        public IComparable[] Keys
        {
            get
            {
                return _keys;
            }
        }

        public IComparable DefaultKey
        {
            get
            {
                return _defaultKey;
            }
        }

        public int DefaultSequence
        {
            get
            {
                return _defaultSequence;
            }
        }

        //========
        // METHODS
        //========
    }

    public class SortComparer : IComparer
    {
        private int[] _sortDir;

        //=============
        // CONSTRUCTORS
        //=============

        public SortComparer(ArrayList aSortDirList)
        {
            int i;

            _sortDir = new int[aSortDirList.Count];

            for (i = 0; i < aSortDirList.Count; i++)
            {
                if ((SortEnum)aSortDirList[i] == SortEnum.desc)
                {
                    _sortDir[i] = -1;
                }
                else
                {
                    _sortDir[i] = 1;
                }
            }
        }

        //===========
        // PROPERTIES
        //===========

        //========
        // METHODS
        //========

        public int Compare(object x, object y)
        {
            GridSortEntry xObj;
            GridSortEntry yObj;
            int maxCount;
            int i;
            int compRes;

            try
            {
                xObj = (GridSortEntry)x;
                yObj = (GridSortEntry)y;
                maxCount = xObj.Keys.Length;

                for (i = 0; i < maxCount; i++)
                {
                    compRes = xObj.Keys[i].CompareTo(yObj.Keys[i]);

                    if (compRes != 0)
                    {
                        return compRes * _sortDir[i];
                    }
                }

                compRes = xObj.DefaultKey.CompareTo(yObj.DefaultKey);

                if (compRes != 0)
                {
                    return compRes;
                }
                else
                {
                    return xObj.DefaultSequence.CompareTo(yObj.DefaultSequence);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

    #endregion
}
