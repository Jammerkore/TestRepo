using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class FilterData : DataLayer
    {
        protected static class StoredProcedures
        {

           

			public static MID_FILTER_INSERT_def MID_FILTER_INSERT = new MID_FILTER_INSERT_def();
			public class MID_FILTER_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_INSERT.SQL"

			    private intParameter USER_RID;
                private intParameter OWNER_USER_RID;
                private intParameter FILTER_TYPE;
			    private stringParameter FILTER_NAME;
			    private intParameter IS_LIMITED;
			    private intParameter RESULT_LIMIT;
			    private intParameter FILTER_RID; //Declare Output Parameter
			
			    public MID_FILTER_INSERT_def()
			    {
			        base.procedureName = "MID_FILTER_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("FILTER");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    OWNER_USER_RID = new intParameter("@OWNER_USER_RID", base.inputParameterList);
                    FILTER_TYPE = new intParameter("@FILTER_TYPE", base.inputParameterList);
			        FILTER_NAME = new stringParameter("@FILTER_NAME", base.inputParameterList);
			        IS_LIMITED = new intParameter("@IS_LIMITED", base.inputParameterList);
			        RESULT_LIMIT = new intParameter("@RESULT_LIMIT", base.inputParameterList);
			        FILTER_RID = new intParameter("@FILTER_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  int? USER_RID,
                                              int? OWNER_USER_RID,
                                              int? FILTER_TYPE,
			                                  string FILTER_NAME,
			                                  int? IS_LIMITED,
			                                  int? RESULT_LIMIT
			                                  )
			    {
                    lock (typeof(MID_FILTER_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNER_USER_RID.SetValue(OWNER_USER_RID);
                        this.FILTER_TYPE.SetValue(FILTER_TYPE);
                        this.FILTER_NAME.SetValue(FILTER_NAME);
                        this.IS_LIMITED.SetValue(IS_LIMITED);
                        this.RESULT_LIMIT.SetValue(RESULT_LIMIT);
                        this.FILTER_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

            public static MID_FILTER_INSERT_AUDIT_DEFAULT_def MID_FILTER_INSERT_AUDIT_DEFAULT = new MID_FILTER_INSERT_AUDIT_DEFAULT_def();
            public class MID_FILTER_INSERT_AUDIT_DEFAULT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_INSERT_AUDIT_DEFAULT.SQL"

                private intParameter USER_RID;
                private intParameter FILTER_RID; //Declare Output Parameter

                public MID_FILTER_INSERT_AUDIT_DEFAULT_def()
                {
                    base.procedureName = "MID_FILTER_INSERT_AUDIT_DEFAULT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("FILTER");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    FILTER_RID = new intParameter("@FILTER_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              int? USER_RID
                                              )
                {
                    lock (typeof(MID_FILTER_INSERT_AUDIT_DEFAULT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.FILTER_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

			public static MID_FILTER_CONDITION_INSERT_def MID_FILTER_CONDITION_INSERT = new MID_FILTER_CONDITION_INSERT_def();
			public class MID_FILTER_CONDITION_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_CONDITION_INSERT.SQL"

			    private intParameter FILTER_RID;
			    private intParameter SEQ;
			    private intParameter PARENT_SEQ;
			    private intParameter SIBLING_SEQ;
			    private intParameter ELEMENT_GROUP_TYPE_INDEX;
			    private intParameter LOGIC_INDEX;
			    private intParameter FIELD_INDEX;
			    private intParameter OPERATOR_INDEX;
			    private intParameter VALUE_TYPE_INDEX;
			    private intParameter DATE_TYPE_INDEX;
			    private intParameter NUMERIC_TYPE_INDEX;
			    private stringParameter VALUE_TO_COMPARE;
			    private floatParameter VALUE_TO_COMPARE_DOUBLE;
			    private floatParameter VALUE_TO_COMPARE_DOUBLE2;
			    private intParameter VALUE_TO_COMPARE_INT;
			    private intParameter VALUE_TO_COMPARE_INT2;
			    private intParameter VALUE_TO_COMPARE_BOOL;
			    private datetimeParameter VALUE_TO_COMPARE_DATE_FROM;
			    private datetimeParameter VALUE_TO_COMPARE_DATE_TO;
			    private intParameter VALUE_TO_COMPARE_DATE_DAYS_FROM;
			    private intParameter VALUE_TO_COMPARE_DATE_DAYS_TO;
			    private intParameter VAR1_VARIABLE_INDEX;
			    private intParameter VAR1_VERSION_INDEX;
			    private intParameter VAR1_HN_RID;
			    private intParameter VAR1_CDR_RID;
			    private intParameter VAR1_VALUE_TYPE_INDEX;
			    private intParameter VAR1_TIME_INDEX;
                private intParameter VAR1_IS_TIME_TOTAL;
			    private intParameter VAR_PERCENTAGE_OPERATOR_INDEX;
			    private intParameter VAR2_VARIABLE_INDEX;
			    private intParameter VAR2_VERSION_INDEX;
			    private intParameter VAR2_HN_RID;
			    private intParameter VAR2_CDR_RID;
			    private intParameter VAR2_VALUE_TYPE_INDEX;
			    private intParameter VAR2_TIME_INDEX;
                private intParameter VAR2_IS_TIME_TOTAL;
			    private intParameter HEADER_HN_RID;
                //private intParameter HEADER_PH_RID;
			    private intParameter SORT_BY_TYPE_INDEX;
			    private intParameter SORT_BY_FIELD_INDEX;
			    private intParameter LIST_VALUE_CONSTANT_INDEX;
			    private intParameter CONDITION_RID; //Declare Output Parameter
			
			    public MID_FILTER_CONDITION_INSERT_def()
			    {
			        base.procedureName = "MID_FILTER_CONDITION_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("FILTER_CONDITION");
			        FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
			        SEQ = new intParameter("@SEQ", base.inputParameterList);
			        PARENT_SEQ = new intParameter("@PARENT_SEQ", base.inputParameterList);
			        SIBLING_SEQ = new intParameter("@SIBLING_SEQ", base.inputParameterList);
			        ELEMENT_GROUP_TYPE_INDEX = new intParameter("@ELEMENT_GROUP_TYPE_INDEX", base.inputParameterList);
			        LOGIC_INDEX = new intParameter("@LOGIC_INDEX", base.inputParameterList);
			        FIELD_INDEX = new intParameter("@FIELD_INDEX", base.inputParameterList);
			        OPERATOR_INDEX = new intParameter("@OPERATOR_INDEX", base.inputParameterList);
			        VALUE_TYPE_INDEX = new intParameter("@VALUE_TYPE_INDEX", base.inputParameterList);
			        DATE_TYPE_INDEX = new intParameter("@DATE_TYPE_INDEX", base.inputParameterList);
			        NUMERIC_TYPE_INDEX = new intParameter("@NUMERIC_TYPE_INDEX", base.inputParameterList);
			        VALUE_TO_COMPARE = new stringParameter("@VALUE_TO_COMPARE", base.inputParameterList);
			        VALUE_TO_COMPARE_DOUBLE = new floatParameter("@VALUE_TO_COMPARE_DOUBLE", base.inputParameterList);
			        VALUE_TO_COMPARE_DOUBLE2 = new floatParameter("@VALUE_TO_COMPARE_DOUBLE2", base.inputParameterList);
			        VALUE_TO_COMPARE_INT = new intParameter("@VALUE_TO_COMPARE_INT", base.inputParameterList);
			        VALUE_TO_COMPARE_INT2 = new intParameter("@VALUE_TO_COMPARE_INT2", base.inputParameterList);
			        VALUE_TO_COMPARE_BOOL = new intParameter("@VALUE_TO_COMPARE_BOOL", base.inputParameterList);
			        VALUE_TO_COMPARE_DATE_FROM = new datetimeParameter("@VALUE_TO_COMPARE_DATE_FROM", base.inputParameterList);
			        VALUE_TO_COMPARE_DATE_TO = new datetimeParameter("@VALUE_TO_COMPARE_DATE_TO", base.inputParameterList);
			        VALUE_TO_COMPARE_DATE_DAYS_FROM = new intParameter("@VALUE_TO_COMPARE_DATE_DAYS_FROM", base.inputParameterList);
			        VALUE_TO_COMPARE_DATE_DAYS_TO = new intParameter("@VALUE_TO_COMPARE_DATE_DAYS_TO", base.inputParameterList);
			        VAR1_VARIABLE_INDEX = new intParameter("@VAR1_VARIABLE_INDEX", base.inputParameterList);
			        VAR1_VERSION_INDEX = new intParameter("@VAR1_VERSION_INDEX", base.inputParameterList);
			        VAR1_HN_RID = new intParameter("@VAR1_HN_RID", base.inputParameterList);
			        VAR1_CDR_RID = new intParameter("@VAR1_CDR_RID", base.inputParameterList);
			        VAR1_VALUE_TYPE_INDEX = new intParameter("@VAR1_VALUE_TYPE_INDEX", base.inputParameterList);
			        VAR1_TIME_INDEX = new intParameter("@VAR1_TIME_INDEX", base.inputParameterList);
                    VAR1_IS_TIME_TOTAL = new intParameter("@VAR1_IS_TIME_TOTAL", base.inputParameterList);
			        VAR_PERCENTAGE_OPERATOR_INDEX = new intParameter("@VAR_PERCENTAGE_OPERATOR_INDEX", base.inputParameterList);
			        VAR2_VARIABLE_INDEX = new intParameter("@VAR2_VARIABLE_INDEX", base.inputParameterList);
			        VAR2_VERSION_INDEX = new intParameter("@VAR2_VERSION_INDEX", base.inputParameterList);
			        VAR2_HN_RID = new intParameter("@VAR2_HN_RID", base.inputParameterList);
			        VAR2_CDR_RID = new intParameter("@VAR2_CDR_RID", base.inputParameterList);
			        VAR2_VALUE_TYPE_INDEX = new intParameter("@VAR2_VALUE_TYPE_INDEX", base.inputParameterList);
			        VAR2_TIME_INDEX = new intParameter("@VAR2_TIME_INDEX", base.inputParameterList);
                    VAR2_IS_TIME_TOTAL = new intParameter("@VAR2_IS_TIME_TOTAL", base.inputParameterList);
			        HEADER_HN_RID = new intParameter("@HEADER_HN_RID", base.inputParameterList);
                    //HEADER_PH_RID = new intParameter("@HEADER_PH_RID", base.inputParameterList);
			        SORT_BY_TYPE_INDEX = new intParameter("@SORT_BY_TYPE_INDEX", base.inputParameterList);
			        SORT_BY_FIELD_INDEX = new intParameter("@SORT_BY_FIELD_INDEX", base.inputParameterList);
			        LIST_VALUE_CONSTANT_INDEX = new intParameter("@LIST_VALUE_CONSTANT_INDEX", base.inputParameterList);
			        CONDITION_RID = new intParameter("@CONDITION_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  int? FILTER_RID,
			                                  int? SEQ,
			                                  int? PARENT_SEQ,
			                                  int? SIBLING_SEQ,
			                                  int? ELEMENT_GROUP_TYPE_INDEX,
			                                  int? LOGIC_INDEX,
			                                  int? FIELD_INDEX,
			                                  int? OPERATOR_INDEX,
			                                  int? VALUE_TYPE_INDEX,
			                                  int? DATE_TYPE_INDEX,
			                                  int? NUMERIC_TYPE_INDEX,
			                                  string VALUE_TO_COMPARE,
			                                  double? VALUE_TO_COMPARE_DOUBLE,
			                                  double? VALUE_TO_COMPARE_DOUBLE2,
			                                  int? VALUE_TO_COMPARE_INT,
			                                  int? VALUE_TO_COMPARE_INT2,
			                                  int? VALUE_TO_COMPARE_BOOL,
			                                  DateTime? VALUE_TO_COMPARE_DATE_FROM,
			                                  DateTime? VALUE_TO_COMPARE_DATE_TO,
			                                  int? VALUE_TO_COMPARE_DATE_DAYS_FROM,
			                                  int? VALUE_TO_COMPARE_DATE_DAYS_TO,
			                                  int? VAR1_VARIABLE_INDEX,
			                                  int? VAR1_VERSION_INDEX,
			                                  int? VAR1_HN_RID,
			                                  int? VAR1_CDR_RID,
			                                  int? VAR1_VALUE_TYPE_INDEX,
			                                  int? VAR1_TIME_INDEX,
			                                  int? VAR_PERCENTAGE_OPERATOR_INDEX,
			                                  int? VAR2_VARIABLE_INDEX,
			                                  int? VAR2_VERSION_INDEX,
			                                  int? VAR2_HN_RID,
			                                  int? VAR2_CDR_RID,
			                                  int? VAR2_VALUE_TYPE_INDEX,
			                                  int? VAR2_TIME_INDEX,
			                                  int? HEADER_HN_RID,
                                              //int? HEADER_PH_RID,
			                                  int? SORT_BY_TYPE_INDEX,
			                                  int? SORT_BY_FIELD_INDEX,
			                                  int? LIST_VALUE_CONSTANT_INDEX
			                                  )
			    {
                    lock (typeof(MID_FILTER_CONDITION_INSERT_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        this.SEQ.SetValue(SEQ);
                        this.PARENT_SEQ.SetValue(PARENT_SEQ);
                        this.SIBLING_SEQ.SetValue(SIBLING_SEQ);
                        this.ELEMENT_GROUP_TYPE_INDEX.SetValue(ELEMENT_GROUP_TYPE_INDEX);
                        this.LOGIC_INDEX.SetValue(LOGIC_INDEX);
                        this.FIELD_INDEX.SetValue(FIELD_INDEX);
                        this.OPERATOR_INDEX.SetValue(OPERATOR_INDEX);
                        this.VALUE_TYPE_INDEX.SetValue(VALUE_TYPE_INDEX);
                        this.DATE_TYPE_INDEX.SetValue(DATE_TYPE_INDEX);
                        this.NUMERIC_TYPE_INDEX.SetValue(NUMERIC_TYPE_INDEX);
                        this.VALUE_TO_COMPARE.SetValue(VALUE_TO_COMPARE);
                        this.VALUE_TO_COMPARE_DOUBLE.SetValue(VALUE_TO_COMPARE_DOUBLE);
                        this.VALUE_TO_COMPARE_DOUBLE2.SetValue(VALUE_TO_COMPARE_DOUBLE2);
                        this.VALUE_TO_COMPARE_INT.SetValue(VALUE_TO_COMPARE_INT);
                        this.VALUE_TO_COMPARE_INT2.SetValue(VALUE_TO_COMPARE_INT2);
                        this.VALUE_TO_COMPARE_BOOL.SetValue(VALUE_TO_COMPARE_BOOL);
                        this.VALUE_TO_COMPARE_DATE_FROM.SetValue(VALUE_TO_COMPARE_DATE_FROM);
                        this.VALUE_TO_COMPARE_DATE_TO.SetValue(VALUE_TO_COMPARE_DATE_TO);
                        this.VALUE_TO_COMPARE_DATE_DAYS_FROM.SetValue(VALUE_TO_COMPARE_DATE_DAYS_FROM);
                        this.VALUE_TO_COMPARE_DATE_DAYS_TO.SetValue(VALUE_TO_COMPARE_DATE_DAYS_TO);
                        this.VAR1_VARIABLE_INDEX.SetValue(VAR1_VARIABLE_INDEX);
                        this.VAR1_VERSION_INDEX.SetValue(VAR1_VERSION_INDEX);
                        this.VAR1_HN_RID.SetValue(VAR1_HN_RID);
                        this.VAR1_CDR_RID.SetValue(VAR1_CDR_RID);
                        this.VAR1_VALUE_TYPE_INDEX.SetValue(VAR1_VALUE_TYPE_INDEX);
                        this.VAR1_TIME_INDEX.SetValue(VAR1_TIME_INDEX);
                        this.VAR1_IS_TIME_TOTAL.SetValue(0);
                        this.VAR_PERCENTAGE_OPERATOR_INDEX.SetValue(VAR_PERCENTAGE_OPERATOR_INDEX);
                        this.VAR2_VARIABLE_INDEX.SetValue(VAR2_VARIABLE_INDEX);
                        this.VAR2_VERSION_INDEX.SetValue(VAR2_VERSION_INDEX);
                        this.VAR2_HN_RID.SetValue(VAR2_HN_RID);
                        this.VAR2_CDR_RID.SetValue(VAR2_CDR_RID);
                        this.VAR2_VALUE_TYPE_INDEX.SetValue(VAR2_VALUE_TYPE_INDEX);
                        this.VAR2_TIME_INDEX.SetValue(VAR2_TIME_INDEX);
                        this.VAR2_IS_TIME_TOTAL.SetValue(0);
                        this.HEADER_HN_RID.SetValue(HEADER_HN_RID);
                        //this.HEADER_PH_RID.SetValue(HEADER_PH_RID);
                        this.SORT_BY_TYPE_INDEX.SetValue(SORT_BY_TYPE_INDEX);
                        this.SORT_BY_FIELD_INDEX.SetValue(SORT_BY_FIELD_INDEX);
                        this.LIST_VALUE_CONSTANT_INDEX.SetValue(LIST_VALUE_CONSTANT_INDEX);
                        this.CONDITION_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

            public static MID_FILTER_CONDITION_INSERT_AUDIT_DEFAULT_def MID_FILTER_CONDITION_INSERT_AUDIT_DEFAULT = new MID_FILTER_CONDITION_INSERT_AUDIT_DEFAULT_def();
            public class MID_FILTER_CONDITION_INSERT_AUDIT_DEFAULT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_CONDITION_INSERT_AUDIT_DEFAULT.SQL"

                private intParameter FILTER_RID;

                public MID_FILTER_CONDITION_INSERT_AUDIT_DEFAULT_def()
                {
                    base.procedureName = "MID_FILTER_CONDITION_INSERT_AUDIT_DEFAULT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("FILTER_CONDITION");
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                              int? FILTER_RID
                                              )
                {
                    lock (typeof(MID_FILTER_CONDITION_INSERT_AUDIT_DEFAULT_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

			public static MID_FILTER_CONDITION_LIST_VALUES_INSERT_def MID_FILTER_CONDITION_LIST_VALUES_INSERT = new MID_FILTER_CONDITION_LIST_VALUES_INSERT_def();
			public class MID_FILTER_CONDITION_LIST_VALUES_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_CONDITION_LIST_VALUES_INSERT.SQL"

			    private tableParameter LIST_TABLE;
			
			    public MID_FILTER_CONDITION_LIST_VALUES_INSERT_def()
			    {
			        base.procedureName = "MID_FILTER_CONDITION_LIST_VALUES_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("FILTER_CONDITION_LIST_VALUES");
			        LIST_TABLE = new tableParameter("@LIST_TABLE", "FILTER_LIST_VALUE_TYPE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, DataTable LIST_TABLE)
			    {
                    lock (typeof(MID_FILTER_CONDITION_LIST_VALUES_INSERT_def))
                    {
                        this.LIST_TABLE.SetValue(LIST_TABLE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_FILTER_CONDITION_DELETE_def MID_FILTER_CONDITION_DELETE = new MID_FILTER_CONDITION_DELETE_def();
			public class MID_FILTER_CONDITION_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_CONDITION_DELETE.SQL"

			    private intParameter FILTER_RID;
			
			    public MID_FILTER_CONDITION_DELETE_def()
			    {
			        base.procedureName = "MID_FILTER_CONDITION_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FILTER_CONDITION");
			        FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? FILTER_RID)
			    {
                    lock (typeof(MID_FILTER_CONDITION_DELETE_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_FILTER_DELETE_def MID_FILTER_DELETE = new MID_FILTER_DELETE_def();
			public class MID_FILTER_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_DELETE.SQL"

			    private intParameter FILTER_RID;
			
			    public MID_FILTER_DELETE_def()
			    {
			        base.procedureName = "MID_FILTER_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FILTER");
			        FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? FILTER_RID)
			    {
                    lock (typeof(MID_FILTER_DELETE_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}


            public static MID_HEADER_CHAR_READ_FOR_GROUP_def MID_HEADER_CHAR_READ_FOR_GROUP = new MID_HEADER_CHAR_READ_FOR_GROUP_def();
            public class MID_HEADER_CHAR_READ_FOR_GROUP_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_FOR_GROUP.SQL"

                private intParameter HCG_RID;
			
			    public MID_HEADER_CHAR_READ_FOR_GROUP_def()
			    {
                    base.procedureName = "MID_HEADER_CHAR_READ_FOR_GROUP";
			        base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HCG_RID");
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
			    }

                public DataTable Read(DatabaseAccess _dba, int? HCG_RID)
			    {
                    lock (typeof(MID_HEADER_CHAR_READ_FOR_GROUP_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_STORE_CHAR_READ_FOR_GROUP_def MID_STORE_CHAR_READ_FOR_GROUP = new MID_STORE_CHAR_READ_FOR_GROUP_def();
            public class MID_STORE_CHAR_READ_FOR_GROUP_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_READ_FOR_GROUP.SQL"

                private intParameter SCG_RID;
                private stringParameter SORT_STRING;  // TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.

                public MID_STORE_CHAR_READ_FOR_GROUP_def()
                {
                    base.procedureName = "MID_STORE_CHAR_READ_FOR_GROUP";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SCG_RID");
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                    SORT_STRING = new stringParameter("@SORT_STRING", base.inputParameterList);  // TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
                }

                // Begin TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
				//public DataTable Read(DatabaseAccess _dba, int? SCG_RID)
                public DataTable Read(DatabaseAccess _dba, int? SCG_RID, string SORT_STRING)
				// End TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
                {
                    lock (typeof(MID_STORE_CHAR_READ_FOR_GROUP_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        this.SORT_STRING.SetValue(SORT_STRING);  // TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


			public static MID_FILTER_READ_def MID_FILTER_READ = new MID_FILTER_READ_def();
			public class MID_FILTER_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ.SQL"

			    private intParameter FILTER_RID;
			
			    public MID_FILTER_READ_def()
			    {
			        base.procedureName = "MID_FILTER_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FILTER");
			        FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FILTER_RID)
			    {
                    lock (typeof(MID_FILTER_READ_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FILTER_CONDITION_READ_def MID_FILTER_CONDITION_READ = new MID_FILTER_CONDITION_READ_def();
			public class MID_FILTER_CONDITION_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_CONDITION_READ.SQL"

			    private intParameter FILTER_RID;
			
			    public MID_FILTER_CONDITION_READ_def()
			    {
			        base.procedureName = "MID_FILTER_CONDITION_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FILTER_CONDITION");
			        FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FILTER_RID)
			    {
                    lock (typeof(MID_FILTER_CONDITION_READ_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FILTER_CONDITION_LIST_VALUES_READ_def MID_FILTER_CONDITION_LIST_VALUES_READ = new MID_FILTER_CONDITION_LIST_VALUES_READ_def();
			public class MID_FILTER_CONDITION_LIST_VALUES_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_CONDITION_LIST_VALUES_READ.SQL"

			    private intParameter FILTER_RID;
			
			    public MID_FILTER_CONDITION_LIST_VALUES_READ_def()
			    {
			        base.procedureName = "MID_FILTER_CONDITION_LIST_VALUES_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FILTER_CONDITION_LIST_VALUES");
			        FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FILTER_RID)
			    {
                    lock (typeof(MID_FILTER_CONDITION_LIST_VALUES_READ_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FILTER_READ_NAME_FOR_DUPLICATE_def MID_FILTER_READ_NAME_FOR_DUPLICATE = new MID_FILTER_READ_NAME_FOR_DUPLICATE_def();
			public class MID_FILTER_READ_NAME_FOR_DUPLICATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ_NAME_FOR_DUPLICATE.SQL"

			    private intParameter FILTER_TYPE;
			    private stringParameter PROPOSED_FILTER_NAME;
                private intParameter FILTER_EDIT_RID;
			
			    public MID_FILTER_READ_NAME_FOR_DUPLICATE_def()
			    {
			        base.procedureName = "MID_FILTER_READ_NAME_FOR_DUPLICATE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FILTER");
			        FILTER_TYPE = new intParameter("@FILTER_TYPE", base.inputParameterList);
			        PROPOSED_FILTER_NAME = new stringParameter("@PROPOSED_FILTER_NAME", base.inputParameterList);
                    FILTER_EDIT_RID = new intParameter("@FILTER_EDIT_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FILTER_TYPE,
			                          string PROPOSED_FILTER_NAME,
                                      int? FILTER_EDIT_RID
			                          )
			    {
                    lock (typeof(MID_FILTER_READ_NAME_FOR_DUPLICATE_def))
                    {
                        this.FILTER_TYPE.SetValue(FILTER_TYPE);
                        this.PROPOSED_FILTER_NAME.SetValue(PROPOSED_FILTER_NAME);
                        this.FILTER_EDIT_RID.SetValue(FILTER_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_FILTER_READ_FROM_USER_AND_NAME_def MID_FILTER_READ_FROM_USER_AND_NAME = new MID_FILTER_READ_FROM_USER_AND_NAME_def();
            public class MID_FILTER_READ_FROM_USER_AND_NAME_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ_FROM_USER_AND_NAME.SQL"

                private intParameter FILTER_TYPE;
                private intParameter USER_RID;
                private stringParameter FILTER_NAME;
              

                public MID_FILTER_READ_FROM_USER_AND_NAME_def()
                {
                    base.procedureName = "MID_FILTER_READ_FROM_USER_AND_NAME";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FILTER");
                    FILTER_TYPE = new intParameter("@FILTER_TYPE", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    FILTER_NAME = new stringParameter("@FILTER_NAME", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? FILTER_TYPE,
                                      int? USER_RID,
                                      string FILTER_NAME
                                      )
                {
                    lock (typeof(MID_FILTER_READ_FROM_USER_AND_NAME_def))
                    {
                        this.FILTER_TYPE.SetValue(FILTER_TYPE);
                        this.USER_RID.SetValue(USER_RID);
                        this.FILTER_NAME.SetValue(FILTER_NAME);

                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_FILTER_READ_FROM_USER_def MID_FILTER_READ_FROM_USER = new MID_FILTER_READ_FROM_USER_def();
            public class MID_FILTER_READ_FROM_USER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ_FROM_USER.SQL"

                private intParameter FILTER_TYPE;
                private intParameter USER_RID;

                public MID_FILTER_READ_FROM_USER_def()
                {
                    base.procedureName = "MID_FILTER_READ_FROM_USER";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FILTER");
                    FILTER_TYPE = new intParameter("@FILTER_TYPE", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? FILTER_TYPE,
                                      int? USER_RID
                                      )
                {
                    lock (typeof(MID_FILTER_READ_FROM_USER_def))
                    {
                        this.FILTER_TYPE.SetValue(FILTER_TYPE);
                        this.USER_RID.SetValue(USER_RID);

                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_FILTER_READ_DEFAULT_def MID_FILTER_READ_DEFAULT = new MID_FILTER_READ_DEFAULT_def();
            public class MID_FILTER_READ_DEFAULT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ_DEFAULT.SQL"

                public MID_FILTER_READ_DEFAULT_def()
                {
                    base.procedureName = "MID_FILTER_READ_DEFAULT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FILTER");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(MID_FILTER_READ_DEFAULT_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			public static MID_FILTER_UPDATE_def MID_FILTER_UPDATE = new MID_FILTER_UPDATE_def();
			public class MID_FILTER_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_UPDATE.SQL"

			    private intParameter FILTER_RID;
			    private intParameter USER_RID;
                private intParameter OWNER_USER_RID;  // TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
			    private stringParameter FILTER_NAME;
			    private intParameter IS_LIMITED;
			    private intParameter RESULT_LIMIT;
			
			    public MID_FILTER_UPDATE_def()
			    {
			        base.procedureName = "MID_FILTER_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("FILTER");
			        FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    OWNER_USER_RID = new intParameter("@OWNER_USER_RID", base.inputParameterList);  // TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
			        FILTER_NAME = new stringParameter("@FILTER_NAME", base.inputParameterList);
			        IS_LIMITED = new intParameter("@IS_LIMITED", base.inputParameterList);
			        RESULT_LIMIT = new intParameter("@RESULT_LIMIT", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? FILTER_RID,
			                      int? USER_RID,
                                  int? OWNER_USER_RID,  // TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
			                      string FILTER_NAME,
			                      int? IS_LIMITED,
			                      int? RESULT_LIMIT
			                      )
			    {
                    lock (typeof(MID_FILTER_UPDATE_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.OWNER_USER_RID.SetValue(OWNER_USER_RID);  // TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
                        this.FILTER_NAME.SetValue(FILTER_NAME);
                        this.IS_LIMITED.SetValue(IS_LIMITED);
                        this.RESULT_LIMIT.SetValue(RESULT_LIMIT);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static MID_FILTER_UPDATE_NAME_def MID_FILTER_UPDATE_NAME = new MID_FILTER_UPDATE_NAME_def();
            public class MID_FILTER_UPDATE_NAME_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_UPDATE_NAME.SQL"

                private intParameter FILTER_RID;
                private intParameter USER_RID;
                private stringParameter FILTER_NAME;

                public MID_FILTER_UPDATE_NAME_def()
                {
                    base.procedureName = "MID_FILTER_UPDATE_NAME";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("FILTER");
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    FILTER_NAME = new stringParameter("@FILTER_NAME", base.inputParameterList);                
                }

                public int Update(DatabaseAccess _dba,
                                  int? FILTER_RID,
                                  int? USER_RID,
                                  string FILTER_NAME
                                  )
                {
                    lock (typeof(MID_FILTER_UPDATE_NAME_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.FILTER_NAME.SetValue(FILTER_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

			public static MID_FILTER_READ_FROM_USERS_def MID_FILTER_READ_FROM_USERS = new MID_FILTER_READ_FROM_USERS_def();
			public class MID_FILTER_READ_FROM_USERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ_FROM_USERS.SQL"

			    private intParameter FILTER_TYPE;
                private intParameter FILTER_PROFILE_TYPE;
                private tableParameter USER_RID_LIST;
			
			    public MID_FILTER_READ_FROM_USERS_def()
			    {
			        base.procedureName = "MID_FILTER_READ_FROM_USERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FILTER");
			        FILTER_TYPE = new intParameter("@FILTER_TYPE", base.inputParameterList);
                    FILTER_PROFILE_TYPE = new intParameter("@FILTER_PROFILE_TYPE", base.inputParameterList);
                    USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FILTER_TYPE,
                                      int? FILTER_PROFILE_TYPE,
			                          DataTable USER_RID_LIST
			                          )
			    {
                    lock (typeof(MID_FILTER_READ_FROM_USERS_def))
                    {
                        this.FILTER_TYPE.SetValue(FILTER_TYPE);
                        this.FILTER_PROFILE_TYPE.SetValue(FILTER_PROFILE_TYPE);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            //Begin TT#1388-MD -jsobek -Product Filter
            public static MID_FILTER_READ_FOR_USER_def MID_FILTER_READ_FOR_USER = new MID_FILTER_READ_FOR_USER_def();
            public class MID_FILTER_READ_FOR_USER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ_FOR_USER.SQL"

                private intParameter FILTER_TYPE;
                private tableParameter USER_RID_LIST;

                public MID_FILTER_READ_FOR_USER_def()
                {
                    base.procedureName = "MID_FILTER_READ_FOR_USER";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FILTER");
                    FILTER_TYPE = new intParameter("@FILTER_TYPE", base.inputParameterList);
                    USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? FILTER_TYPE,
                                      DataTable USER_RID_LIST
                                      )
                {
                    lock (typeof(MID_FILTER_READ_FOR_USER_def))
                    {
                        this.FILTER_TYPE.SetValue(FILTER_TYPE);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#1388-MD -jsobek -Product Filter

			public static MID_FILTER_READ_PARENT_FROM_USERS_def MID_FILTER_READ_PARENT_FROM_USERS = new MID_FILTER_READ_PARENT_FROM_USERS_def();
			public class MID_FILTER_READ_PARENT_FROM_USERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ_PARENT_FROM_USERS.SQL"

			    private intParameter FILTER_TYPE;
                private intParameter FILTER_PROFILE_TYPE;
			    private tableParameter USER_RID_LIST;
			
			    public MID_FILTER_READ_PARENT_FROM_USERS_def()
			    {
			        base.procedureName = "MID_FILTER_READ_PARENT_FROM_USERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FILTER");
			        FILTER_TYPE = new intParameter("@FILTER_TYPE", base.inputParameterList);
                    FILTER_PROFILE_TYPE = new intParameter("@FILTER_PROFILE_TYPE", base.inputParameterList);
                    USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FILTER_TYPE,
                                      int? FILTER_PROFILE_TYPE,
			                          DataTable USER_RID_LIST
			                          )
			    {
                    lock (typeof(MID_FILTER_READ_PARENT_FROM_USERS_def))
                    {
                        this.FILTER_TYPE.SetValue(FILTER_TYPE);
                        this.FILTER_PROFILE_TYPE.SetValue(FILTER_PROFILE_TYPE);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_CHAR_READ_FOR_FILTER_def MID_STORE_CHAR_READ_FOR_FILTER = new MID_STORE_CHAR_READ_FOR_FILTER_def();
			public class MID_STORE_CHAR_READ_FOR_FILTER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_CHAR_READ_FOR_FILTER.SQL"

			    private intParameter SCG_RID;
			
			    public MID_STORE_CHAR_READ_FOR_FILTER_def()
			    {
			        base.procedureName = "MID_STORE_CHAR_READ_FOR_FILTER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_CHAR");
			        SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SCG_RID)
			    {
                    lock (typeof(MID_STORE_CHAR_READ_FOR_FILTER_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            //Begin TT#1313-MD -jsobek -Header Filters
            public static MID_DOES_WF_PROCEDURE_EXIST_def MID_DOES_WF_PROCEDURE_EXIST = new MID_DOES_WF_PROCEDURE_EXIST_def();
            public class MID_DOES_WF_PROCEDURE_EXIST_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DOES_WF_PROCEDURE_EXIST.SQL"

                private stringParameter WF_NAME;

                public MID_DOES_WF_PROCEDURE_EXIST_def()
                {
                    base.procedureName = "MID_DOES_WF_PROCEDURE_EXIST";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("sysobjects");
                    WF_NAME = new stringParameter("@WF_NAME", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string WF_NAME)
                {
                    lock (typeof(MID_DOES_WF_PROCEDURE_EXIST_def))
                    {
                        this.WF_NAME.SetValue(WF_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


            public static MID_USER_CURRENT_WORKSPACE_FILTER_READ_def MID_USER_CURRENT_WORKSPACE_FILTER_READ = new MID_USER_CURRENT_WORKSPACE_FILTER_READ_def();
            public class MID_USER_CURRENT_WORKSPACE_FILTER_READ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_CURRENT_WORKSPACE_FILTER_READ.SQL"

                private intParameter USER_RID;
                private intParameter WORKSPACE_TYPE;

                public MID_USER_CURRENT_WORKSPACE_FILTER_READ_def()
                {
                    base.procedureName = "MID_USER_CURRENT_WORKSPACE_FILTER_READ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("USER_CURRENT_WORKSPACE_FILTER");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    WORKSPACE_TYPE = new intParameter("@WORKSPACE_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? USER_RID,
                                      int? WORKSPACE_TYPE
                                      )
                {
                    lock (typeof(MID_USER_CURRENT_WORKSPACE_FILTER_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.WORKSPACE_TYPE.SetValue(WORKSPACE_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_USER_CURRENT_WORKSPACE_FILTER_INSERT_def MID_USER_CURRENT_WORKSPACE_FILTER_INSERT = new MID_USER_CURRENT_WORKSPACE_FILTER_INSERT_def();
            public class MID_USER_CURRENT_WORKSPACE_FILTER_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_CURRENT_WORKSPACE_FILTER_INSERT.SQL"

                private intParameter USER_RID;
                private intParameter WORKSPACE_TYPE;
                private intParameter FILTER_RID;

                public MID_USER_CURRENT_WORKSPACE_FILTER_INSERT_def()
                {
                    base.procedureName = "MID_USER_CURRENT_WORKSPACE_FILTER_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("USER_CURRENT_WORKSPACE_FILTER");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    WORKSPACE_TYPE = new intParameter("@WORKSPACE_TYPE", base.inputParameterList);
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? USER_RID,
                                  int? WORKSPACE_TYPE,
                                  int? FILTER_RID
                                  )
                {
                    lock (typeof(MID_USER_CURRENT_WORKSPACE_FILTER_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.WORKSPACE_TYPE.SetValue(WORKSPACE_TYPE);
                        this.FILTER_RID.SetValue(FILTER_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_def MID_USER_CURRENT_WORKSPACE_FILTER_DELETE = new MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_def();
            public class MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_CURRENT_WORKSPACE_FILTER_DELETE.SQL"

                private intParameter USER_RID;
                private intParameter WORKSPACE_TYPE;

                public MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_def()
                {
                    base.procedureName = "MID_USER_CURRENT_WORKSPACE_FILTER_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("USER_CURRENT_WORKSPACE_FILTER");
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    WORKSPACE_TYPE = new intParameter("@WORKSPACE_TYPE", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba,
                                  int? USER_RID,
                                  int? WORKSPACE_TYPE
                                  )
                {
                    lock (typeof(MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.WORKSPACE_TYPE.SetValue(WORKSPACE_TYPE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

			// Begin TT#1362-MD - stodd - Header filter InUse is not returning any filters being InUse even though they are
            public static MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_ALL_def MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_ALL = new MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_ALL_def();
            public class MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_ALL_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_ALL.SQL"

                private intParameter FILTER_RID;

                public MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_ALL_def()
                {
                    base.procedureName = "MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_ALL";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("USER_CURRENT_WORKSPACE_FILTER");
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba,
                                  int? FILTER_RID
                                  )
                {
                    lock (typeof(MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_ALL_def))
                    {
                        this.FILTER_RID.SetValue(FILTER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }
			// End TT#1362-MD - stodd - Header filter InUse is not returning any filters being InUse even though they are


            public static MID_GRID_VIEW_READ_WORKSPACE_FILTER_FROM_NODE_def MID_GRID_VIEW_READ_WORKSPACE_FILTER_FROM_NODE = new MID_GRID_VIEW_READ_WORKSPACE_FILTER_FROM_NODE_def();
            public class MID_GRID_VIEW_READ_WORKSPACE_FILTER_FROM_NODE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GRID_VIEW_READ_WORKSPACE_FILTER_FROM_NODE.SQL"

                private intParameter HEADER_HN_RID;
                private intParameter FILTER_TYPE;

                public MID_GRID_VIEW_READ_WORKSPACE_FILTER_FROM_NODE_def()
                {
                    base.procedureName = "MID_GRID_VIEW_READ_WORKSPACE_FILTER_FROM_NODE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("GRID_VIEW");
                    HEADER_HN_RID = new intParameter("@HEADER_HN_RID", base.inputParameterList);
                    FILTER_TYPE = new intParameter("@FILTER_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? HEADER_HN_RID,
                                      int? FILTER_TYPE
                                      )
                {
                    lock (typeof(MID_GRID_VIEW_READ_WORKSPACE_FILTER_FROM_NODE_def))
                    {
                        this.HEADER_HN_RID.SetValue(HEADER_HN_RID);
                        this.FILTER_TYPE.SetValue(FILTER_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#1313-MD -jsobek -Header Filters
            //Begin TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions
            public static MID_FILTER_READ_FROM_TYPE_def MID_FILTER_READ_FROM_TYPE = new MID_FILTER_READ_FROM_TYPE_def();
            public class MID_FILTER_READ_FROM_TYPE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ_FROM_TYPE.SQL"

                private intParameter FILTER_TYPE;

                public MID_FILTER_READ_FROM_TYPE_def()
                {
                    base.procedureName = "MID_FILTER_READ_FROM_TYPE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FILTER");
                    FILTER_TYPE = new intParameter("@FILTER_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? FILTER_TYPE
                                      )
                {
                    lock (typeof(MID_FILTER_READ_FROM_TYPE_def))
                    {
                        this.FILTER_TYPE.SetValue(FILTER_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions

            // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
            public static MID_FILTER_READ_FROM_TYPE_AND_FIELD_NAME_def MID_FILTER_READ_FROM_TYPE_AND_FIELD_NAME = new MID_FILTER_READ_FROM_TYPE_AND_FIELD_NAME_def();
            public class  MID_FILTER_READ_FROM_TYPE_AND_FIELD_NAME_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ_FROM_TYPE_AND_FIELD_NAME.SQL"

                private intParameter FILTER_TYPE;
                private stringParameter FIELD_NAME;

                public MID_FILTER_READ_FROM_TYPE_AND_FIELD_NAME_def()
                {
                    base.procedureName = "MID_FILTER_READ_FROM_TYPE_AND_FIELD_NAME";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FILTER");
                    FILTER_TYPE = new intParameter("@FILTER_TYPE", base.inputParameterList);
                    FIELD_NAME = new stringParameter("@FIELD_NAME", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? FILTER_TYPE,
                                      string FIELD_NAME
                                      )
                {
                    lock (typeof(MID_FILTER_READ_FROM_TYPE_AND_FIELD_NAME_def))
                    {
                        this.FILTER_TYPE.SetValue(FILTER_TYPE);
                        this.FIELD_NAME.SetValue(FIELD_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields


            public static MID_FILTER_READ_FOR_REFRESH_def MID_FILTER_READ_FOR_REFRESH = new MID_FILTER_READ_FOR_REFRESH_def();
            public class MID_FILTER_READ_FOR_REFRESH_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_READ_FOR_REFRESH.SQL"
                
                private tableParameter FIELD_CHANGED_LIST;
                private tableParameter CHAR_CHANGED_LIST;

                public MID_FILTER_READ_FOR_REFRESH_def()
                {
                    base.procedureName = "MID_FILTER_READ_FOR_REFRESH";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FILTER");
                    FIELD_CHANGED_LIST = new tableParameter("@FIELD_CHANGED_LIST", "FIELD_CHANGED_TYPE", base.inputParameterList);
                    CHAR_CHANGED_LIST = new tableParameter("@CHAR_CHANGED_LIST", "CHAR_CHANGED_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                  DataTable FIELD_CHANGED_LIST,
                                  DataTable CHAR_CHANGED_LIST
                                  )
                {
                    lock (typeof(MID_FILTER_READ_FOR_REFRESH_def))
                    {
                        this.FIELD_CHANGED_LIST.SetValue(FIELD_CHANGED_LIST);
                        this.CHAR_CHANGED_LIST.SetValue(CHAR_CHANGED_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


            public static MID_FILTER_INSERT_DYNAMIC_ATTRIBUTE_def MID_FILTER_INSERT_DYNAMIC_ATTRIBUTE = new MID_FILTER_INSERT_DYNAMIC_ATTRIBUTE_def();
            public class MID_FILTER_INSERT_DYNAMIC_ATTRIBUTE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_INSERT_DYNAMIC_ATTRIBUTE.SQL"

                private intParameter SCG_RID;
                private stringParameter SCG_ID;
                private intParameter USER_RID;
                private intParameter FILTER_RID; //Declare Output Parameter

                public MID_FILTER_INSERT_DYNAMIC_ATTRIBUTE_def()
                {
                    base.procedureName = "MID_FILTER_INSERT_DYNAMIC_ATTRIBUTE";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("FILTER");
                    SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
                    SCG_ID = new stringParameter("@SCG_ID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    FILTER_RID = new intParameter("@FILTER_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                  int? SCG_RID,
                                  string SCG_ID,
                                  int? USER_RID
                                  )
                {
                    lock (typeof(MID_FILTER_INSERT_DYNAMIC_ATTRIBUTE_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        this.SCG_ID.SetValue(SCG_ID);
                        this.USER_RID.SetValue(USER_RID);
                        this.FILTER_RID.SetValue(-1); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static MID_FILTER_INSERT_ALL_STORES_SET_def MID_FILTER_INSERT_ALL_STORES_SET = new MID_FILTER_INSERT_ALL_STORES_SET_def();
            public class MID_FILTER_INSERT_ALL_STORES_SET_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FILTER_INSERT_ALL_STORES_SET.SQL"

                private intParameter FILTER_RID; //Declare Output Parameter

                public MID_FILTER_INSERT_ALL_STORES_SET_def()
                {
                    base.procedureName = "MID_FILTER_INSERT_ALL_STORES_SET";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("FILTER");
                    FILTER_RID = new intParameter("@FILTER_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba )
                {
                    lock (typeof(MID_FILTER_INSERT_ALL_STORES_SET_def))
                    {
                        this.FILTER_RID.SetValue(-1); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
