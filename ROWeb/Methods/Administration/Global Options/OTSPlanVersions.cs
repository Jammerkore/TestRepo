using System;
using System.Collections.Generic;
// BEGIN TT#1156-MD - CTeegarden - add OTS Plan Versions
using System.Data;
using System.Globalization;
// END TT#1156-MD - CTeegarden - add OTS Plan Versions
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Data;  
using MIDRetail.DataCommon; 

namespace Logility.ROWeb
{
    public partial class ROGlobalOptions
    {
        private DataTable GetForecastVersions()
        {
            MIDTextDataHandler textDataHandler = new MIDTextDataHandler("Forecast Versions", "ID", "NAME");

            return textDataHandler.GetUITextTable(eMIDTextType.eForecastBlendType, eMIDTextOrderBy.TextCode);
        }

        private DataTable GetOTSPlanList(string sTableName, bool bGetHistory)
        {
            DataTable dt = new DataTable(sTableName);
            DataRow dr;

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("NAME", typeof(string));

            FVRowHandler rowHandler = FVRowHandler.Instance;

            foreach (DataRow drIn in _dtFV.Rows)
            {
                rowHandler.ParseDBRow(drIn);

                bool bIsHistoryRow = (rowHandler.iRID == Include.FV_ActualRID) || (rowHandler.iRID == Include.FV_ModifiedRID);
                bool bSaveRow = (bGetHistory && bIsHistoryRow) || !(bGetHistory || bIsHistoryRow);

                if (bSaveRow)
                {
                    dr = dt.NewRow();

                    dr["ID"] = rowHandler.iRID;
                    dr["NAME"] = rowHandler.sDescription;

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        private DataTable GetOTSPlanVersions()
        {
            DataTable dt = BuildOTSPlanVersionsDataTable();

            if (_GlobalOptionsProfile.Key != Include.NoRID)
            {
                AddOTSPlanVersionsData(dt);
            }

            return dt;
        }

        private DataTable BuildOTSPlanVersionsDataTable()
        {
            DataTable dt = new DataTable("OTS Plan Versions");

            FVRowHandler.Instance.AddUITableColumns(dt); 

            return dt;
        }

        private void AddOTSPlanVersionsData(DataTable dt)
        {
            foreach (DataRow drFV in _dtFV.Rows)
            {
                DataRow dr = dt.NewRow();

                FVRowHandler.Instance.TranslateDBRowToUI(drFV, dr);
                dt.Rows.Add(dr);
            }
        }

        private void UpdateOTSPlanVersions(DataTable dtOTSPlanVersions)
        {
            ForecastVersion fv = new ForecastVersion();
            DataTable dtFV = fv.GetForecastVersions(true);

            UpdateOrCreateFVRows(fv, dtFV, dtOTSPlanVersions);
            DeleteOrMakeInactiveFVRows(fv, dtFV, dtOTSPlanVersions);
        }

        private void UpdateOrCreateFVRows(ForecastVersion fv, DataTable dtFV, DataTable dtOTSPlanVersions)
        {
            FVRowHandler rowHandler = FVRowHandler.Instance;
            var existingRowsSet = GetIDColumnSet(dtFV, rowHandler.sDBIDColName);

            try
            {
                fv.OpenUpdateConnection();
                foreach (DataRow dr in dtOTSPlanVersions.Rows)
                {
                    rowHandler.ParseUIRow(dr);
                    if (existingRowsSet.Contains(rowHandler.iRID)) 	
                    {
                        rowHandler.Update(fv);
                    }
                    else
                    {
                        rowHandler.Create(fv);
                    }
                }
                fv.CommitData();
            }
            catch(Exception)
            {
                fv.Rollback();
                throw;
            }
            finally 
            {
                fv.CloseUpdateConnection();
            }
        }

        private void DeleteOrMakeInactiveFVRows(ForecastVersion fv, DataTable dtFV, DataTable dtOTSPlanVersions)
        {
            FVRowHandler rowHandler = FVRowHandler.Instance;
            var updateRowsSet = GetIDColumnSet(dtOTSPlanVersions, rowHandler.sUIIDColName);

            foreach (DataRow dr in dtFV.Rows)
            {
                rowHandler.ParseDBRow(dr);
                if (!updateRowsSet.Contains(rowHandler.iRID)) 	
                {
                    rowHandler.Delete(fv);
                }
            }
        }

        private HashSet<int> GetIDColumnSet(DataTable dt, string sKeyColName)
        {
            HashSet<int> set = new HashSet<int>();

            foreach (DataRow dr in dt.Rows)
            {
                
				int iRID = ColumnHandler.ConvertColumnToInt(dr[sKeyColName], Include.NoRID);

                if (iRID != Include.NoRID)
                {
                    set.Add(iRID);
                }
				
            }

            return set;
        }

    }
    	
    public abstract class ColumnHandler
    {
         protected string _sUIColName;
        protected eMIDTextCode _eTextCode;
        protected bool _bReadOnly;

        public ColumnHandler(string sUIColName, eMIDTextCode eTextCode, bool bReadOnly)
        {
            _sUIColName = sUIColName;
            _eTextCode = eTextCode;
            _bReadOnly = bReadOnly;
        }

        public string sUIColName
        {
            get { return _sUIColName; }
        }

        public abstract DataColumn BuildUIColumn();

        public static string ConvertColumnToString(Object colVal, string sDefaultVal)
        {
            string sReturn = sDefaultVal;

            if (colVal != DBNull.Value)
            {
                sReturn = Convert.ToString(colVal);
            }

            return sReturn;
        }

        public static bool ConvertColumnToBoolean(Object colVal, bool defaultVal)
        {
            bool bReturn = defaultVal;

            if (colVal != DBNull.Value)
            {
                if ((colVal is Boolean) || (colVal is bool))
                    bReturn = Convert.ToBoolean(colVal);
                else
                    bReturn = Include.ConvertCharToBool(Convert.ToChar(colVal, CultureInfo.CurrentUICulture));
            }

            return bReturn;
        }

        public static int ConvertColumnToInt(Object colVal, int iDefaultVal)
        {
            int iReturn = iDefaultVal;

            if (colVal != DBNull.Value)
            {
                iReturn = Convert.ToInt32(colVal, CultureInfo.CurrentCulture);
            }

            return iReturn;
        }

        public static double ConvertColumnToDouble(Object colVal, double dDefaultVal)
        {
            double dReturn = dDefaultVal;

            if (colVal != DBNull.Value)
            {
                dReturn = Convert.ToDouble(colVal, CultureInfo.CurrentCulture);
            }

            return dReturn;
        }
    }

    public interface IDBColumnHandler
    {
        void TranslateDBColumnToUI(DataRow drDBRow, DataRow drUIRow);
    }

    public class TypedColumnHandler<T> : ColumnHandler
    {
        protected T _defaultVal;

        public TypedColumnHandler(string sUIColName, eMIDTextCode eTextCode, bool bReadOnly, T defaultVal)
            : base(sUIColName, eTextCode, bReadOnly)
        {
            ValidateType(defaultVal);
            _defaultVal = defaultVal;
        }

        protected void ValidateType(T val)
        {
            if ( (val is int) || (val is double) || (val is bool) || (val is string) )
            {
                return;
            }

            throw new Exception("Invalid type used for TypedColumnHandler<>");
        }

        public override DataColumn BuildUIColumn()
        {
            DataColumn dc = new DataColumn(_sUIColName, _defaultVal.GetType());
            string sCaption = (_eTextCode == eMIDTextCode.Unassigned) ? dc.Caption = _sUIColName : dc.Caption = MIDText.GetTextOnly(_eTextCode);

            dc.AllowDBNull = false;
            dc.Caption = sCaption;
            dc.DefaultValue = _defaultVal;
            dc.ReadOnly = _bReadOnly;

            return dc;
        }

        public T ParseUIColumn(DataRow dr)
        {
            return ConvertColumn(dr[_sUIColName]);
        }

        public void SetUIColumn(DataRow dr, T value)
        {
            dr[_sUIColName] = value;
        }

        protected T ConvertColumn(Object obj)
        {
            if ( (_defaultVal is int) || (_defaultVal is Int32) )
            {
                return (T) (object) ConvertColumnToInt(obj, Convert.ToInt32(_defaultVal));
            }
            else if ( (_defaultVal is double) || (_defaultVal is Double) )
            {
                return (T)(object)ConvertColumnToDouble(obj, Convert.ToDouble(_defaultVal));
            }
            else if ( (_defaultVal is bool) || (_defaultVal is Boolean) )
            {
                return (T)(object)ConvertColumnToBoolean(obj, Convert.ToBoolean(_defaultVal));
            }
            else // if (_defaultVal is string)
            {
                return (T)(object)ConvertColumnToString(obj, Convert.ToString(_defaultVal));
            }
        }
    }

    public class TypedDBColumnHandler<T> : TypedColumnHandler<T>, IDBColumnHandler
    {
        string _sDBColName;

        public string sDBColName { get { return _sDBColName; } }

        public TypedDBColumnHandler(string sDBColName, string sUIColName, eMIDTextCode eTextCode, bool bReadOnly, T defaultVal)
            : base(sUIColName, eTextCode, bReadOnly, defaultVal)
        {
            _sDBColName = sDBColName;
        }

        public void TranslateDBColumnToUI(DataRow drDBRow, DataRow drUIRow)
        {
            drUIRow[_sUIColName] = ConvertColumn(drDBRow[_sDBColName]);
        }

        public T ParseColumn(DataRow dr, bool isDBRow)
        {
            string sColName = isDBRow ? _sDBColName : _sUIColName;

            return ConvertColumn(dr[sColName]);
        }
    }

    public abstract class RowHandler
    {
        protected ColumnHandler[] _aColumnHandlers;

        protected RowHandler()
        {
            _aColumnHandlers = new ColumnHandler[] { }; 
        }


        public void AddUITableColumns(DataTable dt)
        {
            foreach (ColumnHandler columnHandler in _aColumnHandlers)
            {
                dt.Columns.Add(columnHandler.BuildUIColumn());
            }
        }

        public abstract void ParseUIRow(DataRow dr);


        public abstract void FillUIRow(DataRow dr);

    }

    public abstract class DBRowHandler : RowHandler
    {
        protected TypedDBColumnHandler<int> _RIDColumnHandler;

        public int iRID { get { return _iRID; } }  

        protected int _iRID;  

        protected DBRowHandler(string sDBIDColName, string sUIIDColName, eMIDTextCode eTextCode)
            : base()
        {
            _RIDColumnHandler = new TypedDBColumnHandler<int>(sDBIDColName, sUIIDColName, eTextCode, true, Include.NoRID);
            _aColumnHandlers = new ColumnHandler[] { _RIDColumnHandler };
        }

        public string sUIIDColName
        {
            get { return _RIDColumnHandler.sUIColName; }
        }

        public string sDBIDColName
        {
            get { return _RIDColumnHandler.sDBColName; }
        }

        public virtual void TranslateDBRowToUI(DataRow drDB, DataRow drUI)  
        {
            foreach (IDBColumnHandler columnHandler in _aColumnHandlers)
            {
                columnHandler.TranslateDBColumnToUI(drDB, drUI);
            }
        }

        public override void ParseUIRow(DataRow dr)
        {
            ParseDataRow(dr, false);
        }

        public override void FillUIRow(DataRow dr)
        {
            throw new NotImplementedException("Not used in DB Row Handler");
        }

        public virtual void ParseDBRow(DataRow dr)  
        {
            ParseDataRow(dr, true);
        }

        protected virtual void ParseDataRow(DataRow dr, bool bIsDBRow)
        {
            _iRID = _RIDColumnHandler.ParseColumn(dr, bIsDBRow);  
        }
    }

    public class FVRowHandler : DBRowHandler
	

    {

        private static FVRowHandler _Instance;

        public static FVRowHandler Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new FVRowHandler();
                }

                return _Instance;
            }
        }

        
        private string sVersionID { get; set; }

        private TypedDBColumnHandler<string> _VersionIDColHandler;
        public string sDescription { get; set; }
        private TypedDBColumnHandler<string> _DescriptionColHandler;
        private bool bProtectHistory { get; set; }
        private TypedDBColumnHandler<bool> _ProtectHistoryColHandler;
        private bool bActive { get; set; }
        private TypedDBColumnHandler<bool> _ActiveColHandler;
        private bool bSimilarStore { get; set; }
        private TypedDBColumnHandler<bool> _SimilarStoreColHandler;
        private eForecastBlendType eCombine { get; set; }
        private TypedDBColumnHandler<int> _CombineColHandler;
        private int iActualRID { get; set; }
        private TypedDBColumnHandler<int> _ActualRIDColHandler;
        private int iForecastRID { get; set; }
        private TypedDBColumnHandler<int> _ForecastRIDColHandler;
        private bool bCurrentBlend { get; set; }
        private TypedDBColumnHandler<bool> _CurrentBlendColHandler;

        protected FVRowHandler()
            : base("FV_RID", "id", eMIDTextCode.Unassigned)  
        {
            _VersionIDColHandler = new TypedDBColumnHandler<string>("FV_ID", "VERSION", eMIDTextCode.Unassigned, true, "Z");
            _DescriptionColHandler = new TypedDBColumnHandler<string>("DESCRIPTION", "DESCRIPTION", eMIDTextCode.lbl_FV_Description, false, "");
            _ProtectHistoryColHandler = new TypedDBColumnHandler<bool>("PROTECT_HISTORY_IND", "PROTECT_HISTORY", eMIDTextCode.lbl_FV_ProtectHistory, false, false);
            _ActiveColHandler = new TypedDBColumnHandler<bool>("ACTIVE_IND", "ACTIVE", eMIDTextCode.lbl_FV_Active, false, false);
            _SimilarStoreColHandler = new TypedDBColumnHandler<bool>("SIMILAR_STORE_IND", "SIMILAR_STORE", eMIDTextCode.lbl_FV_SimilarStore, false, false);
            _CombineColHandler = new TypedDBColumnHandler<int>("BLEND_TYPE", "COMBINE", eMIDTextCode.lbl_FV_Combine, false, (int)eForecastBlendType.None);
            _ActualRIDColHandler = new TypedDBColumnHandler<int>("ACTUAL_FV_RID", "ACTUAL", eMIDTextCode.lbl_FV_CombineActual, false, Include.NoRID);
            _ForecastRIDColHandler = new TypedDBColumnHandler<int>("FORECAST_FV_RID", "FORECAST", eMIDTextCode.lbl_FV_CombineForecast, false, Include.NoRID);
            _CurrentBlendColHandler = new TypedDBColumnHandler<bool>("CURRENT_BLEND_IND", "PERIOD_HISTORY", eMIDTextCode.lbl_FV_CombineCurrentMonth, false, false);

            _aColumnHandlers = new ColumnHandler[] { _RIDColumnHandler, _VersionIDColHandler, _DescriptionColHandler, _ProtectHistoryColHandler, 
                                                     _ActiveColHandler,  _SimilarStoreColHandler, _CombineColHandler, _ActualRIDColHandler,
                                                     _ForecastRIDColHandler, _CurrentBlendColHandler };
        }

        public void Update(DataLayer dl) 
        {
            ForecastVersion fv = (ForecastVersion) dl;

            fv.UpdateVersion(_iRID, sVersionID, sDescription, bProtectHistory, bActive, eCombine,  
                             iActualRID, iForecastRID, bCurrentBlend, bSimilarStore);
        }

        public void Create(DataLayer dl) 
        {
            ForecastVersion fv = (ForecastVersion) dl;

            fv.CreateVersion("Z", sDescription, bProtectHistory, bActive, eCombine, iActualRID, iForecastRID, bCurrentBlend, bSimilarStore);
        }

        public void Delete(DataLayer dl) 
        {
            ForecastVersion fv = (ForecastVersion) dl;

            try
            {
                fv.OpenUpdateConnection();
                fv.DeleteVersion(_iRID);  
                fv.CommitData();
            }
            catch (Exception)
            {
                bProtectHistory = false;
                Update(fv);
            }
            finally
            {
                fv.CloseUpdateConnection();
            }
        }

        protected override void ParseDataRow(DataRow dr, bool bIsDBRow)
        {
            base.ParseDataRow(dr, bIsDBRow);

            sVersionID = _VersionIDColHandler.ParseColumn(dr, bIsDBRow);
            sDescription = _DescriptionColHandler.ParseColumn(dr, bIsDBRow);
            bProtectHistory = _ProtectHistoryColHandler.ParseColumn(dr, bIsDBRow);
            bActive = _ActiveColHandler.ParseColumn(dr, bIsDBRow);
            bSimilarStore = _SimilarStoreColHandler.ParseColumn(dr, bIsDBRow);
            eCombine = (eForecastBlendType)_CombineColHandler.ParseColumn(dr, bIsDBRow);
            iActualRID = _ActualRIDColHandler.ParseColumn(dr, bIsDBRow);
            iForecastRID = _ForecastRIDColHandler.ParseColumn(dr, bIsDBRow);
            bCurrentBlend = _CurrentBlendColHandler.ParseColumn(dr, bIsDBRow);

            if (eCombine == eForecastBlendType.None)
            {
                iActualRID = Include.NoRID;
                iForecastRID = Include.NoRID;
            }
        }
    }
}
