using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using System.Data;

using MIDRetail.DataCommon;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;

namespace MIDRetail.Business
{
	//#region
	//public class AssortmentSummary
	//{
	//    //=======
	//    // FIELDS
	//    //=======

	//    //private int _headerRid;
	//    private StoreGroupProfile _storeGroupProfile;
	//    private ProfileList _variableItemList;
	//    private ProfileList _storeGradeList;
	//    private DataTable _dtSummary;

	//    //public int HeaderRid
	//    //{
	//    //    get { return _headerRid; }
	//    //    set { _headerRid = value; }
	//    //}

	//    public StoreGroupProfile StoreGroupProfile
	//    {
	//        get { return _storeGroupProfile; }
	//        set { _storeGroupProfile = value; }
	//    }

	//    public ProfileList StoreGradeList
	//    {
	//        get { return _storeGradeList; }
	//        set { _storeGradeList = value; }
	//    }


	//    //=============
	//    // CONSTRUCTORS
	//    //=============

	//    public AssortmentSummary(StoreGroupProfile storeGroupProfile, ProfileList storeGradeList)
	//    {
	//        //_headerRid = headerRid;
	//        _storeGroupProfile = storeGroupProfile;
	//        _storeGradeList = storeGradeList;
	//        _variableItemList = new ProfileList(eProfileType.AssortmentSummaryItem);
	//        CreateSummaryTable();
	//    }

	//    //============
	//    // Methods
	//    //============

	//    /// <summary>
	//    /// Expects valid varNum, setRid, and storeIndex.
	//    /// Atuomatically creates and adds to the variable record and the variable/set record. 
	//    /// </summary>
	//    /// <param name="varNum"></param>
	//    /// <param name="setRid"></param>
	//    /// <param name="storeIndex"></param>
	//    /// <param name="units"></param>
	//    /// <param name="avgStore"></param>
	//    /// <param name="basis"></param>
	//    /// <param name="intransit"></param>
	//    /// <param name="need"></param>
	//    /// <param name="pctNeed"></param>
	//    public void Add(int varNum, int setRid, int storeIndex, int units, int avgStore, int basis,
	//                        int intransit, int need, decimal pctNeed)
	//    {
	//        try
	//        {
	//            int grade = GetStoreGradeSequence(storeIndex);

	//            bool newRow = false;
	//            DataRow aRow = GetSummaryRow(varNum, setRid, grade, out newRow);

	//            //============================
	//            // Variable/Set/Grade record
	//            //============================
	//            int oldUnits = Convert.ToInt32(aRow["UNITS"], CultureInfo.CurrentUICulture);
	//            aRow["UNITS"] = oldUnits + units;
	//            int oldNumStores = Convert.ToInt32(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
	//            aRow["NUM_STORES"] = ++oldNumStores;
	//            //int oldAvgStore = Convert.ToInt32(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
	//            aRow["AVG_STORE"] = avgStore;
	//            //int oldAvgUnits = Convert.ToInt32(aRow["AVG_UNITS"], CultureInfo.CurrentUICulture);
	//            //aRow["AVG_UNITS"] = 0;
	//            //int oldIndex = Convert.ToInt32(aRow["INDEX"], CultureInfo.CurrentUICulture);
	//            aRow["INDEX"] = storeIndex;
	//            int oldBasis = Convert.ToInt32(aRow["BASIS"], CultureInfo.CurrentUICulture);
	//            aRow["BASIS"] = oldBasis + units;
	//            int oldNeed = Convert.ToInt32(aRow["NEED"], CultureInfo.CurrentUICulture);
	//            aRow["NEED"] = oldNeed + need;
	//            //int oldPctNeed = Convert.ToInt32(aRow["PCT_NEED"], CultureInfo.CurrentUICulture);
	//            aRow["PCT_NEED"] = pctNeed;
	//            int oldIntransit = Convert.ToInt32(aRow["INTRANSIT"], CultureInfo.CurrentUICulture);
	//            aRow["INTRANSIT"] = oldIntransit + intransit;
	//            if (newRow)
	//                _dtSummary.Rows.Add(aRow);

	//            //===================
	//            // Variable Record
	//            //===================
	//            aRow = GetSummaryRow(varNum, Include.Undefined, Include.Undefined, out newRow);
	//            oldUnits = Convert.ToInt32(aRow["UNITS"], CultureInfo.CurrentUICulture);
	//            aRow["UNITS"] = oldUnits + units;
	//            oldNumStores = Convert.ToInt32(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
	//            aRow["NUM_STORES"] = ++oldNumStores;
	//            //int oldAvgStore = Convert.ToInt32(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
	//            aRow["AVG_STORE"] = avgStore;
	//            //int oldAvgUnits = Convert.ToInt32(aRow["AVG_UNITS"], CultureInfo.CurrentUICulture);
	//            //aRow["AVG_UNITS"] = 0;
	//            //int oldIndex = Convert.ToInt32(aRow["INDEX"], CultureInfo.CurrentUICulture);
	//            aRow["INDEX"] = storeIndex;
	//            oldBasis = Convert.ToInt32(aRow["BASIS"], CultureInfo.CurrentUICulture);
	//            aRow["BASIS"] = oldBasis + units;
	//            oldNeed = Convert.ToInt32(aRow["NEED"], CultureInfo.CurrentUICulture);
	//            aRow["NEED"] = oldNeed + need;
	//            //int oldPctNeed = Convert.ToInt32(aRow["PCT_NEED"], CultureInfo.CurrentUICulture);
	//            aRow["PCT_NEED"] = pctNeed;
	//            oldIntransit = Convert.ToInt32(aRow["INTRANSIT"], CultureInfo.CurrentUICulture);
	//            aRow["INTRANSIT"] = oldIntransit + intransit;
	//            if (newRow)
	//                _dtSummary.Rows.Add(aRow);

	//            //======================
	//            // Variable/Set Record
	//            //======================
	//            aRow = GetSummaryRow(varNum, setRid, Include.Undefined, out newRow);
	//            oldUnits = Convert.ToInt32(aRow["UNITS"], CultureInfo.CurrentUICulture);
	//            aRow["UNITS"] = oldUnits + units;
	//            oldNumStores = Convert.ToInt32(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
	//            aRow["NUM_STORES"] = ++oldNumStores;
	//            //int oldAvgStore = Convert.ToInt32(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
	//            aRow["AVG_STORE"] = avgStore;
	//            //int oldAvgUnits = Convert.ToInt32(aRow["AVG_UNITS"], CultureInfo.CurrentUICulture);
	//            //aRow["AVG_UNITS"] = 0;
	//            //int oldIndex = Convert.ToInt32(aRow["INDEX"], CultureInfo.CurrentUICulture);
	//            aRow["INDEX"] = storeIndex;
	//            oldBasis = Convert.ToInt32(aRow["BASIS"], CultureInfo.CurrentUICulture);
	//            aRow["BASIS"] = oldBasis + units;
	//            oldNeed = Convert.ToInt32(aRow["NEED"], CultureInfo.CurrentUICulture);
	//            aRow["NEED"] = oldNeed + need;
	//            //int oldPctNeed = Convert.ToInt32(aRow["PCT_NEED"], CultureInfo.CurrentUICulture);
	//            aRow["PCT_NEED"] = pctNeed;
	//            oldIntransit = Convert.ToInt32(aRow["INTRANSIT"], CultureInfo.CurrentUICulture);
	//            aRow["INTRANSIT"] = oldIntransit + intransit;
	//            if (newRow)
	//                _dtSummary.Rows.Add(aRow);

	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }

	//    private DataRow GetSummaryRow(int varNum, int setRid, int grade, out bool newRow)
	//    {
	//        DataRow aRow = null;
	//        try
	//        {
	//            DataRow[] rows = _dtSummary.Select("VARIABLE_NUMBER = " + varNum.ToString() + " and " +
	//                "GROUP_LEVEL_RID = " + setRid.ToString() + " and " +
	//                "GRADE = " + grade.ToString());

	//            newRow = false;
	//            if (rows.Length == 0)
	//            {
	//                aRow = _dtSummary.NewRow();
	//                aRow["VARIABLE_NUMBER"] = varNum;
	//                aRow["GROUP_LEVEL_RID"] = setRid;
	//                aRow["GRADE"] = grade;
	//                aRow["UNITS"] = 0;
	//                aRow["NUM_STORES"] = 0;
	//                aRow["AVG_STORE"] = 0;
	//                aRow["AVG_UNITS"] = 0;
	//                aRow["INDEX"] = 0;
	//                aRow["BASIS"] = 0;
	//                aRow["NEED"] = 0;
	//                aRow["PCT_NEED"] = 0;
	//                aRow["INTRANSIT"] = 0;
	//                newRow = true;
	//            }
	//            else
	//            {
	//                aRow = rows[0];
	//            }
	//            return aRow;
	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }

	//    /// <summary>
	//    ///  Clears the Assortment Summary class profile list and member profile lists
	//    /// </summary>
	//    public void Clear()
	//    {
	//        try
	//        {
	//            _dtSummary.Clear();
	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }

	//    private void CreateSummaryTable()
	//    {
	//        _dtSummary = MIDEnvironment.CreateDataTable("Summary Table");

	//        _dtSummary.Columns.Add(new DataColumn("VARIABLE_NUMBER", typeof(int)));
	//        _dtSummary.Columns.Add(new DataColumn("GROUP_LEVEL_RID", typeof(int)));
	//        _dtSummary.Columns.Add(new DataColumn("GRADE", typeof(int)));
	//        _dtSummary.Columns.Add(new DataColumn("UNITS", typeof(int)));
	//        _dtSummary.Columns.Add(new DataColumn("NUM_STORES", typeof(int)));
	//        _dtSummary.Columns.Add(new DataColumn("AVG_STORE", typeof(decimal)));
	//        _dtSummary.Columns.Add(new DataColumn("AVG_UNITS", typeof(decimal)));
	//        _dtSummary.Columns.Add(new DataColumn("INDEX", typeof(decimal)));
	//        _dtSummary.Columns.Add(new DataColumn("BASIS", typeof(int)));
	//        _dtSummary.Columns.Add(new DataColumn("NEED", typeof(int)));
	//        _dtSummary.Columns.Add(new DataColumn("PCT_NEED", typeof(decimal)));
	//        _dtSummary.Columns.Add(new DataColumn("INTRANSIT", typeof(int)));

	//        _dtSummary.PrimaryKey = new DataColumn[] {_dtSummary.Columns["VARIABLE_NUMBER"], 
	//                                             _dtSummary.Columns["GROUP_LEVEL_RID"],
	//                                             _dtSummary.Columns["GRADE"]};

	//    }

	//    public AssortmentSummaryItemProfile GetSummary(int variableNumber, int setRid, int storeGrade)
	//    {
	//        AssortmentSummaryItemProfile summaryProfile = new AssortmentSummaryItemProfile(variableNumber);
	//        summaryProfile.VariableNumber = variableNumber;
	//        summaryProfile.Set = setRid;
	//        summaryProfile.Grade = storeGrade;
	//        try
	//        {
	//            bool newRow = false;
	//            DataRow aRow = GetSummaryRow(variableNumber, setRid, storeGrade, out newRow);

	//            if (aRow != null)
	//            {
	//                summaryProfile.NumberOfStores = Convert.ToInt32(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
	//                summaryProfile.TotalUnits = Convert.ToInt32(aRow["UNITS"], CultureInfo.CurrentUICulture);
	//                summaryProfile.AverageStore = Convert.ToInt32(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
	//                summaryProfile.Index = Convert.ToInt32(aRow["INDEX"], CultureInfo.CurrentUICulture);
	//                summaryProfile.Basis = Convert.ToInt32(aRow["BASIS"], CultureInfo.CurrentUICulture);
	//                summaryProfile.Intransit = Convert.ToInt32(aRow["INTRANSIT"], CultureInfo.CurrentUICulture);
	//                summaryProfile.Need = Convert.ToInt32(aRow["NEED"], CultureInfo.CurrentUICulture);
	//                summaryProfile.PctNeed = Convert.ToInt32(aRow["PCT_NEED"], CultureInfo.CurrentUICulture);
	//            }
	
	//            return summaryProfile;
	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }

	//    public void FinishSummary()
	//    {
	//        //=====================
	//        // variable only rows
	//        //=====================
	//        DataRow[] rows = _dtSummary.Select("GROUP_LEVEL_RID = -1 and GRADE = -1");
	//        foreach (DataRow aRow in rows)
	//        {
	//            // Calc Average Store for variable
	//            double units = Convert.ToDouble(aRow["UNITS"], CultureInfo.CurrentUICulture);
	//            double noOfStores = Convert.ToDouble(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
	//            int averageStore = 0;
	//            if (noOfStores > 0)
	//            {
	//                averageStore = (int)((units / noOfStores) + .5);
	//            }
	//            aRow["AVG_STORE"] = averageStore;
	//            aRow["INDEX"] = 100;
	//        }

	//        //=====================
	//        // variable/set rows
	//        //=====================
	//        bool newRow = false;
	//        rows = _dtSummary.Select("GROUP_LEVEL_RID <> -1 and GRADE = -1");
	//        foreach (DataRow aRow in rows)
	//        {
	//            int varNum = Convert.ToInt32(aRow["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture);
	//            // calc index for set
	//            DataRow varRow = GetSummaryRow(varNum, Include.Undefined, Include.Undefined, out newRow);
	//            double varAvgStore = Convert.ToDouble(varRow["AVG_STORE"], CultureInfo.CurrentUICulture);
	//            double setAvgStore = Convert.ToDouble(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
	//            int index = 0;
	//            if (varAvgStore > 0)
	//                index = (int)(((setAvgStore * 100) / varAvgStore) + .5);
	//            aRow["INDEX"] = index;
	//        }

	//        //=====================
	//        // variable/set rows
	//        //=====================
	//        newRow = false;
	//        rows = _dtSummary.Select("GROUP_LEVEL_RID <> -1 and GRADE <> -1");
	//        foreach (DataRow aRow in rows)
	//        {
	//            int varNum = Convert.ToInt32(aRow["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture);
	//            int setRid = Convert.ToInt32(aRow["GROUP_LEVEL_RID"], CultureInfo.CurrentUICulture);
	//            // Calc Average Store for grade
	//            double units = Convert.ToDouble(aRow["UNITS"], CultureInfo.CurrentUICulture);
	//            double noOfStores = Convert.ToDouble(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
	//            double gradeAvgStore = 0;
	//            int index = 0;
	//            if (noOfStores > 0)
	//            {
	//                gradeAvgStore = (int)((units / noOfStores) + .5);
	//                // calc index for grade
	//                DataRow setRow = GetSummaryRow(varNum, setRid, Include.Undefined, out newRow);
	//                double setAvgStore = Convert.ToDouble(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
	//                index = (int)(((gradeAvgStore * 100) / setAvgStore) + .5);
	//            }
	//            aRow["INDEX"] = index;
	//            aRow["AVG_STORE"] = (int)gradeAvgStore;
	//        }

	//        _dtSummary.AcceptChanges();

	//    }

	//    private int GetStoreGradeSequence(int index)
	//    {
	//        int grade = 0;
	//        for (int i=0;i<_storeGradeList.Count;i++)
	//        {
	//            StoreGradeProfile sgp = (StoreGradeProfile)_storeGradeList[i];
	//            int j = i + 1;
	//            int lowBoundary = 0;
	//            if (j < _storeGradeList.Count)
	//            {
	//                StoreGradeProfile nextSgp = (StoreGradeProfile)_storeGradeList[j];
	//                lowBoundary = nextSgp.Boundary;
	//            }
	//            if (index <= sgp.Boundary && index > lowBoundary)
	//            {
	//                grade = lowBoundary;
	//                break;
	//            }
	//        }
	//        return grade;
	//    }

	//    public void DebugSummary()
	//    {
	//        string line = string.Empty;
	//        for (int i = 0; i < _dtSummary.Columns.Count; i++)
	//        {
	//            string caption = _dtSummary.Columns[i].Caption;
	//            caption = caption.PadLeft(10);
	//            line += caption + " ";
	//        }
	//        Debug.WriteLine(line);
	//        foreach (DataRow aRow in _dtSummary.Rows)
	//        {
	//            line = string.Empty;
	//            for (int i = 0; i < _dtSummary.Columns.Count; i++)
	//            {
	//                string colValue = aRow[i].ToString();
	//                int length = _dtSummary.Columns[i].Caption.Length;
	//                colValue = colValue.PadLeft(length<10 ? 10 : length);
	//                line += colValue + " ";
	//            }
	//            Debug.WriteLine(line);
	//        }
	//    }
	//}


	//#endregion


	//#region AssortmentSummaryItemProfile
	//public class AssortmentSummaryItemProfile : Profile
	//{
	//    //=======
	//    // FIELDS
	//    //=======
	//    private int _varNum;
	//    private int _set;
	//    private int _grade;

	//    private int _total;
	//    private int _noOfStores;
	//    private int _avgStore;
	//    private int _index;
	//    private int _basis;
	//    private int _intransit;
	//    private int _need;
	//    private decimal _pctNeed;
	//    private ProfileList _list;

	//    public int VariableNumber
	//    {
	//        get { return _varNum; }
	//        set { _varNum = value; }
	//    }
	//    public int Set
	//    {
	//        get { return _set; }
	//        set { _set = value; }
	//    }
	//    public int Grade
	//    {
	//        get { return _grade; }
	//        set { _grade = value; }
	//    }
	//    public int TotalUnits
	//    {
	//        get { return _total; }
	//        set { _total = value; }
	//    }
	//    public int NumberOfStores
	//    {
	//        get { return _noOfStores; }
	//        set { _noOfStores = value; }
	//    }
	//    /// <summary>
	//    /// The Units for the Average store as calculated by the General Assortment Method.
	//    /// </summary>
	//    public int AverageStore
	//    {
	//        get { return _avgStore; }
	//        set { _avgStore = value; }
	//    }
	//    public int Index
	//    {
	//        get { return _index; }
	//        set { _index = value; }
	//    }
	//    public int Basis
	//    {
	//        get { return _basis; }
	//        set { _basis = value; }
	//    }
	//    public int Intransit
	//    {
	//        get { return _intransit; }
	//        set { _intransit = value; }
	//    }
	//    public int Need
	//    {
	//        get { return _need; }
	//        set { _need = value; }
	//    }
	//    public decimal PctNeed
	//    {
	//        get { return _pctNeed; }
	//        set { _pctNeed = value; }
	//    }
	//    /// <summary>
	//    /// List of subordinate items. 
	//    /// a Variable will have a list of Attribute Set AssortmentSummaryItemProfiles.
	//    /// an Attribute Set will have a list of Store Grade AssortmentSummaryItemProfiles.
	//    /// a Store Grade's list is null--It has no subordinate items.
	//    /// </summary>
	//    public ProfileList MemberList
	//    {
	//        get { return _list; }
	//        set { _list = value; }
	//    }

	//    //=============
	//    // CONSTRUCTORS
	//    //=============

	//    public AssortmentSummaryItemProfile(int key)
	//        : base(key)
	//    {
	//        _varNum = Include.Undefined;
	//        _set = Include.Undefined;
	//        _grade = Include.Undefined;
	//        _total = 0;
	//        _noOfStores = 0;
	//        _avgStore = 0;
	//        _index = 0;
	//        _intransit = 0;
	//        _need = 0;
	//        _basis = 0;
	//        _pctNeed = 0.0m;

	//        _list = new ProfileList(eProfileType.AssortmentSummaryItem);
	//    }

	//    /// <summary>
	//    /// method assumes you are adding one store's total at a time.
	//    /// </summary>
	//    /// <param name="total"></param>
	//    public void Add(int total, int avgStore)
	//    {
	//        _total += total;
	//        _noOfStores++;
	//        if (_avgStore == 0)
	//            _avgStore = avgStore;
	//    }

	//    public void AddToList(AssortmentSummaryItemProfile asip)
	//    {
	//        _list.Add(asip);
	//    }

	//    /// <summary>
	//    /// Returns the eProfileType of this profile.
	//    /// </summary>
	//    override public eProfileType ProfileType
	//    {
	//        get
	//        {
	//            return eProfileType.AssortmentSummaryItem;
	//        }
	//    }
	//}
	//#endregion
}
