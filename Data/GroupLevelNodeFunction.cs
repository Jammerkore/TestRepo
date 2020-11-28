using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public partial class GroupLevelNodeFunction
    {
        private int _methodRID;
        private int _sglRID;
        private int _hnRID;
        private bool _applyMinMaxesInd;
        private eMinMaxInheritType _minMaxInheritType;
        private bool _isHighLevel;
        private ProfileList _stock_MinMax;

        public int MethodRID
        {
            get { return _methodRID; }
            set { _methodRID = value; }
        }

        public int SglRID
        {
            get { return _sglRID; }
            set { _sglRID = value; }
        }

        public int HN_RID
        {
            get { return _hnRID; }
            set { _hnRID = value; }
        }

        public bool ApplyMinMaxesInd
        {
            get { return _applyMinMaxesInd; }
            set { _applyMinMaxesInd = value; }
        }

        public eMinMaxInheritType MinMaxInheritType
        {
            get { return _minMaxInheritType; }
            set { _minMaxInheritType = value; }
        }

        public bool isHighLevel
        {
            get { return _isHighLevel; }
            set { _isHighLevel = value; }
        }

        /// <summary>
        /// Gets or sets the Stock MinMax ProfileList for the Method.
        /// </summary>
        public ProfileList Stock_MinMax
        {
            get { return _stock_MinMax; }
            set { _stock_MinMax = value; }
        }

        public GroupLevelNodeFunction()
        {
            _stock_MinMax = new ProfileList(eProfileType.StockMinMax);
        }

        public static DataTable GetAllGroupLevelNodeFunctions(int methodRID, int sqlRID, TransactionData td)
        {
            try
            {
                DataTable dtGLNF = MIDEnvironment.CreateDataTable();
                dtGLNF = StoredProcedures.MID_GROUP_LEVEL_NODE_FUNCTION_READ.Read(td.DBA,
                                                                                METHOD_RID: methodRID,
                                                                                SGL_RID: sqlRID
                                                                                );
                return dtGLNF;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public bool InsertGroupLevelNodeFunction(TransactionData td)
        {
            bool InsertSuccessful = false;
            StoredProcedures.MID_GROUP_LEVEL_NODE_FUNCTION_INSERT.Insert(td.DBA,
                                                                         METHOD_RID: _methodRID,
                                                                         SGL_RID: _sglRID,
                                                                         HN_RID: _hnRID,
                                                                         APPLY_MIN_MAXES_IND: Include.ConvertBoolToChar(_applyMinMaxesInd),
                                                                         MIN_MAXES_INHERIT_TYPE: (int)_minMaxInheritType
                                                                         );



            InsertSuccessful = true;

            return InsertSuccessful;
        }

        public static bool DeleteGroupLevelNodeFunction(int methodRID, int sglRID, TransactionData td)
        {
            bool DeleteSuccessful = false;

            StoredProcedures.MID_GROUP_LEVEL_NODE_FUNCTION_DELETE.Delete(td.DBA,
                                                                             METHOD_RID: methodRID,
                                                                             SGL_RID: sglRID
                                                                             );
            DeleteSuccessful = true;

            return DeleteSuccessful;
        }

        // Begin TT#2647 - JSmith - Delays in OTS Method
        /// <summary>
        /// Returns a copy of this object.
        /// </summary>
        /// <returns>
        /// A copy of the object.
        /// </returns>
        public GroupLevelNodeFunction Copy()
        {
            try
            {
                GroupLevelNodeFunction glnf = (GroupLevelNodeFunction)this.MemberwiseClone();
                glnf.MethodRID = MethodRID;
                glnf.SglRID = SglRID;
                glnf.HN_RID = HN_RID;
                glnf.ApplyMinMaxesInd = ApplyMinMaxesInd;
                glnf.MinMaxInheritType = MinMaxInheritType;
                glnf.isHighLevel = isHighLevel;
                glnf.Stock_MinMax = Stock_MinMax;


                return glnf;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#2647 - JSmith - Delays in OTS Method
    }
}
