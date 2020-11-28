using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class HeaderCharacteristicsData : DataLayer
    {
        protected static class StoredProcedures
        {
            public static SP_MID_HEADER_CHAR_LOAD_def SP_MID_HEADER_CHAR_LOAD = new SP_MID_HEADER_CHAR_LOAD_def();
            public class SP_MID_HEADER_CHAR_LOAD_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADER_CHAR_LOAD.SQL"

                private stringParameter CHARNAME;
                private stringParameter VALUE_TEXT;
                private floatParameter VALUE_MONEY;
                private floatParameter VALUE_NUMBER;
                private datetimeParameter VALUE_DTM;
                private stringParameter PROCESSACTION;
                private intParameter HDR_RID;
                private intParameter RETCODE; //Declare Output Parameter

                public SP_MID_HEADER_CHAR_LOAD_def()
                {
                    base.procedureName = "SP_MID_HEADER_CHAR_LOAD";
                    base.procedureType = storedProcedureTypes.UpdateWithReturnCode;
                    base.tableNames.Add("HEADER_CHAR");
                    CHARNAME = new stringParameter("@CHARNAME", base.inputParameterList);
                    VALUE_TEXT = new stringParameter("@VALUE_TEXT", base.inputParameterList);
                    VALUE_MONEY = new floatParameter("@VALUE_MONEY", base.inputParameterList);
                    VALUE_NUMBER = new floatParameter("@VALUE_NUMBER", base.inputParameterList);
                    VALUE_DTM = new datetimeParameter("@VALUE_DTM", base.inputParameterList);
                    PROCESSACTION = new stringParameter("@PROCESSACTION", base.inputParameterList);
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    RETCODE = new intParameter("@RETCODE", base.outputParameterList); //Add Output Parameter
                }

                public int UpdateWithReturnCode(DatabaseAccess _dba,
                                                string CHARNAME,
                                                string VALUE_TEXT,
                                                double? VALUE_MONEY,
                                                double? VALUE_NUMBER,
                                                DateTime? VALUE_DTM,
                                                string PROCESSACTION,
                                                int? HDR_RID
                                                )
                {
                    lock (typeof(SP_MID_HEADER_CHAR_LOAD_def))
                    {
                        this.CHARNAME.SetValue(CHARNAME);
                        this.VALUE_TEXT.SetValue(VALUE_TEXT);
                        this.VALUE_MONEY.SetValue(VALUE_MONEY);
                        this.VALUE_NUMBER.SetValue(VALUE_NUMBER);
                        this.VALUE_DTM.SetValue(VALUE_DTM);
                        this.PROCESSACTION.SetValue(PROCESSACTION);
                        this.HDR_RID.SetValue(HDR_RID);
                        this.RETCODE.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForUpdate(_dba);
                        return (int)RETCODE.Value;
                    }
                }
            }

            public static MID_HEADER_CHAR_GROUP_READ_FROM_ID_def MID_HEADER_CHAR_GROUP_READ_FROM_ID = new MID_HEADER_CHAR_GROUP_READ_FROM_ID_def();
            public class MID_HEADER_CHAR_GROUP_READ_FROM_ID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_GROUP_READ_FROM_ID.SQL"

                private stringParameter HCG_ID;

                public MID_HEADER_CHAR_GROUP_READ_FROM_ID_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_GROUP_READ_FROM_ID";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_CHAR_GROUP");
                    HCG_ID = new stringParameter("@HCG_ID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string HCG_ID)
                {
                    lock (typeof(MID_HEADER_CHAR_GROUP_READ_FROM_ID_def))
                    {
                        this.HCG_ID.SetValue(HCG_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			public static MID_HEADER_CHAR_READ_def MID_HEADER_CHAR_READ = new MID_HEADER_CHAR_READ_def();
			public class MID_HEADER_CHAR_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ.SQL"

			    private intParameter HDR_RID;
			
			    public MID_HEADER_CHAR_READ_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER_CHAR");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_HEADER_CHAR_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


			public static MID_HEADER_CHAR_JOIN_DELETE_def MID_HEADER_CHAR_JOIN_DELETE = new MID_HEADER_CHAR_JOIN_DELETE_def();
			public class MID_HEADER_CHAR_JOIN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_JOIN_DELETE.SQL"

			    private intParameter HDR_RID;
			
			    public MID_HEADER_CHAR_JOIN_DELETE_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_JOIN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("HEADER_CHAR_JOIN");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_HEADER_CHAR_JOIN_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_JOIN_DELETE_FROM_CHAR_def MID_HEADER_CHAR_JOIN_DELETE_FROM_CHAR = new MID_HEADER_CHAR_JOIN_DELETE_FROM_CHAR_def();
			public class MID_HEADER_CHAR_JOIN_DELETE_FROM_CHAR_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_JOIN_DELETE_FROM_CHAR.SQL"

			    private intParameter HC_RID;
			
			    public MID_HEADER_CHAR_JOIN_DELETE_FROM_CHAR_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_JOIN_DELETE_FROM_CHAR";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("HEADER_CHAR_JOIN");
			        HC_RID = new intParameter("@HC_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? HC_RID)
			    {
                    lock (typeof(MID_HEADER_CHAR_JOIN_DELETE_FROM_CHAR_def))
                    {
                        this.HC_RID.SetValue(HC_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_JOIN_DELETE_FROM_GROUP_def MID_HEADER_CHAR_JOIN_DELETE_FROM_GROUP = new MID_HEADER_CHAR_JOIN_DELETE_FROM_GROUP_def();
			public class MID_HEADER_CHAR_JOIN_DELETE_FROM_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_JOIN_DELETE_FROM_GROUP.SQL"

			    private intParameter HCG_RID;
			
			    public MID_HEADER_CHAR_JOIN_DELETE_FROM_GROUP_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_JOIN_DELETE_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("HEADER_CHAR_JOIN");
			        HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? HCG_RID)
			    {
                    lock (typeof(MID_HEADER_CHAR_JOIN_DELETE_FROM_GROUP_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_HEADERCHARJOIN_INSERT_def SP_MID_HEADERCHARJOIN_INSERT = new SP_MID_HEADERCHARJOIN_INSERT_def();
            public class SP_MID_HEADERCHARJOIN_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADERCHARJOIN_INSERT.SQL"

			    private intParameter HDR_RID;
			    private intParameter HC_RID;
			
			    public SP_MID_HEADERCHARJOIN_INSERT_def()
			    {
			        base.procedureName = "SP_MID_HEADERCHARJOIN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("HEADER_CHAR_JOIN");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        HC_RID = new intParameter("@HC_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? HC_RID
			                      )
			    {
                    lock (typeof(SP_MID_HEADERCHARJOIN_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HC_RID.SetValue(HC_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_READ_FROM_TEXT_VALUE_def MID_HEADER_CHAR_READ_FROM_TEXT_VALUE = new MID_HEADER_CHAR_READ_FROM_TEXT_VALUE_def();
			public class MID_HEADER_CHAR_READ_FROM_TEXT_VALUE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_FROM_TEXT_VALUE.SQL"

			    private intParameter HCG_RID;
			    private stringParameter TEXT_VALUE;
			
			    public MID_HEADER_CHAR_READ_FROM_TEXT_VALUE_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_READ_FROM_TEXT_VALUE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER_CHAR");
			        HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
			        TEXT_VALUE = new stringParameter("@TEXT_VALUE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HCG_RID,
			                          string TEXT_VALUE
			                          )
			    {
                    lock (typeof(MID_HEADER_CHAR_READ_FROM_TEXT_VALUE_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        this.TEXT_VALUE.SetValue(TEXT_VALUE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_READ_FROM_DATE_VALUE_def MID_HEADER_CHAR_READ_FROM_DATE_VALUE = new MID_HEADER_CHAR_READ_FROM_DATE_VALUE_def();
			public class MID_HEADER_CHAR_READ_FROM_DATE_VALUE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_FROM_DATE_VALUE.SQL"

			    private intParameter HCG_RID;
			    private datetimeParameter DATE_VALUE;
			
			    public MID_HEADER_CHAR_READ_FROM_DATE_VALUE_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_READ_FROM_DATE_VALUE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER_CHAR");
			        HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
			        DATE_VALUE = new datetimeParameter("@DATE_VALUE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HCG_RID,
			                          DateTime? DATE_VALUE
			                          )
			    {
                    lock (typeof(MID_HEADER_CHAR_READ_FROM_DATE_VALUE_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        this.DATE_VALUE.SetValue(DATE_VALUE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_READ_FROM_NUMBER_VALUE_def MID_HEADER_CHAR_READ_FROM_NUMBER_VALUE = new MID_HEADER_CHAR_READ_FROM_NUMBER_VALUE_def();
			public class MID_HEADER_CHAR_READ_FROM_NUMBER_VALUE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_FROM_NUMBER_VALUE.SQL"

			    private intParameter HCG_RID;
			    private floatParameter NUMBER_VALUE;
			
			    public MID_HEADER_CHAR_READ_FROM_NUMBER_VALUE_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_READ_FROM_NUMBER_VALUE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER_CHAR");
			        HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
			        NUMBER_VALUE = new floatParameter("@NUMBER_VALUE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HCG_RID,
			                          double? NUMBER_VALUE
			                          )
			    {
                    lock (typeof(MID_HEADER_CHAR_READ_FROM_NUMBER_VALUE_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        this.NUMBER_VALUE.SetValue(NUMBER_VALUE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_READ_FROM_DOLLAR_VALUE_def MID_HEADER_CHAR_READ_FROM_DOLLAR_VALUE = new MID_HEADER_CHAR_READ_FROM_DOLLAR_VALUE_def();
			public class MID_HEADER_CHAR_READ_FROM_DOLLAR_VALUE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_FROM_DOLLAR_VALUE.SQL"

			    private intParameter HCG_RID;
			    private floatParameter DOLLAR_VALUE;
			
			    public MID_HEADER_CHAR_READ_FROM_DOLLAR_VALUE_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_READ_FROM_DOLLAR_VALUE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER_CHAR");
			        HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
			        DOLLAR_VALUE = new floatParameter("@DOLLAR_VALUE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HCG_RID,
			                          double? DOLLAR_VALUE
			                          )
			    {
                    lock (typeof(MID_HEADER_CHAR_READ_FROM_DOLLAR_VALUE_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        this.DOLLAR_VALUE.SetValue(DOLLAR_VALUE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_READ_FROM_KEY_def MID_HEADER_CHAR_READ_FROM_KEY = new MID_HEADER_CHAR_READ_FROM_KEY_def();
			public class MID_HEADER_CHAR_READ_FROM_KEY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_FROM_KEY.SQL"

			    private intParameter HCG_RID;
			
			    public MID_HEADER_CHAR_READ_FROM_KEY_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_READ_FROM_KEY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER_CHAR");
			        HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HCG_RID)
			    {
                    lock (typeof(MID_HEADER_CHAR_READ_FROM_KEY_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_HEADERCHAR_INSERT_def SP_MID_HEADERCHAR_INSERT = new SP_MID_HEADERCHAR_INSERT_def();
            public class SP_MID_HEADERCHAR_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADERCHAR_INSERT.SQL"

			    private intParameter HCG_RID;
			    private stringParameter TEXT_VALUE;
			    private datetimeParameter DATE_VALUE;
			    private floatParameter NUMBER_VALUE;
			    private floatParameter DOLLAR_VALUE;
			    private intParameter HC_RID; //Declare Output Parameter
			
			    public SP_MID_HEADERCHAR_INSERT_def()
			    {
                    base.procedureName = "SP_MID_HEADERCHAR_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("HEADERCHAR");
			        HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
			        TEXT_VALUE = new stringParameter("@TEXT_VALUE", base.inputParameterList);
			        DATE_VALUE = new datetimeParameter("@DATE_VALUE", base.inputParameterList);
			        NUMBER_VALUE = new floatParameter("@NUMBER_VALUE", base.inputParameterList);
			        DOLLAR_VALUE = new floatParameter("@DOLLAR_VALUE", base.inputParameterList);
			        HC_RID = new intParameter("@HC_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  int? HCG_RID,
			                                  string TEXT_VALUE,
			                                  DateTime? DATE_VALUE,
			                                  double? NUMBER_VALUE,
			                                  double? DOLLAR_VALUE
			                                  )
			    {
                    lock (typeof(SP_MID_HEADERCHAR_INSERT_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        this.TEXT_VALUE.SetValue(TEXT_VALUE);
                        this.DATE_VALUE.SetValue(DATE_VALUE);
                        this.NUMBER_VALUE.SetValue(NUMBER_VALUE);
                        this.DOLLAR_VALUE.SetValue(DOLLAR_VALUE);
                        this.HC_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_READ_LIST_def MID_HEADER_CHAR_READ_LIST = new MID_HEADER_CHAR_READ_LIST_def();
			public class MID_HEADER_CHAR_READ_LIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_LIST.SQL"

			
			    public MID_HEADER_CHAR_READ_LIST_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_READ_LIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER_CHAR");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_HEADER_CHAR_READ_LIST_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_READ_ALL_def MID_HEADER_CHAR_READ_ALL = new MID_HEADER_CHAR_READ_ALL_def();
			public class MID_HEADER_CHAR_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_ALL.SQL"

			
			    public MID_HEADER_CHAR_READ_ALL_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER_CHAR");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_HEADER_CHAR_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


            public static SP_MID_HEADERCHARGROUP_INSERT_def SP_MID_HEADERCHARGROUP_INSERT = new SP_MID_HEADERCHARGROUP_INSERT_def();
			public class SP_MID_HEADERCHARGROUP_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADERCHARGROUP_INSERT.SQL"

			    private stringParameter HCG_ID;
			    private intParameter HCG_TYPE;
			    private charParameter HCG_LIST_IND;
			    private charParameter HCG_PROTECT_IND;
			    private intParameter HCG_RID; //Declare Output Parameter

                public SP_MID_HEADERCHARGROUP_INSERT_def()
			    {
                    base.procedureName = "SP_MID_HEADERCHARGROUP_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("HEADERCHARGROUP");
			        HCG_ID = new stringParameter("@HCG_ID", base.inputParameterList);
			        HCG_TYPE = new intParameter("@HCG_TYPE", base.inputParameterList);
			        HCG_LIST_IND = new charParameter("@HCG_LIST_IND", base.inputParameterList);
			        HCG_PROTECT_IND = new charParameter("@HCG_PROTECT_IND", base.inputParameterList);
			        HCG_RID = new intParameter("@HCG_RID", base.outputParameterList); //Add Output Parameter
			    }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
			                      string HCG_ID,
			                      int? HCG_TYPE,
			                      char? HCG_LIST_IND,
			                      char? HCG_PROTECT_IND
			                      )
			    {
                    lock (typeof(SP_MID_HEADERCHARGROUP_INSERT_def))
                    {
                        this.HCG_ID.SetValue(HCG_ID);
                        this.HCG_TYPE.SetValue(HCG_TYPE);
                        this.HCG_LIST_IND.SetValue(HCG_LIST_IND);
                        this.HCG_PROTECT_IND.SetValue(HCG_PROTECT_IND);
                        this.HCG_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

            public static MID_HEADER_CHAR_GROUP_UPDATE_def MID_HEADER_CHAR_GROUP_UPDATE = new MID_HEADER_CHAR_GROUP_UPDATE_def();
            public class MID_HEADER_CHAR_GROUP_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_GROUP_UPDATE.SQL"

                private intParameter HCG_RID;
                private stringParameter HCG_ID;
                private intParameter HCG_TYPE;
                private charParameter HCG_LIST_IND;
                private charParameter HCG_PROTECT_IND;

                public MID_HEADER_CHAR_GROUP_UPDATE_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_GROUP_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER_CHAR_GROUP");
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                    HCG_ID = new stringParameter("@HCG_ID", base.inputParameterList);
                    HCG_TYPE = new intParameter("@HCG_TYPE", base.inputParameterList);
                    HCG_LIST_IND = new charParameter("@HCG_LIST_IND", base.inputParameterList);
                    HCG_PROTECT_IND = new charParameter("@HCG_PROTECT_IND", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                              int? HCG_RID,
                                              string HCG_ID,
                                              int? HCG_TYPE,
                                              char? HCG_LIST_IND,
                                              char? HCG_PROTECT_IND
                                              )
                {
                    lock (typeof(MID_HEADER_CHAR_GROUP_UPDATE_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        this.HCG_ID.SetValue(HCG_ID);
                        this.HCG_TYPE.SetValue(HCG_TYPE);
                        this.HCG_LIST_IND.SetValue(HCG_LIST_IND);
                        this.HCG_PROTECT_IND.SetValue(HCG_PROTECT_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }


            public static MID_HEADER_CHAR_UPDATE_def MID_HEADER_CHAR_UPDATE = new MID_HEADER_CHAR_UPDATE_def();
            public class MID_HEADER_CHAR_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_UPDATE.SQL"

                public intParameter HC_RID;
                public stringParameter TEXT_VALUE;
                public datetimeParameter DATE_VALUE;
                public floatParameter NUMBER_VALUE;
                public floatParameter DOLLAR_VALUE;

                public MID_HEADER_CHAR_UPDATE_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("HEADER_CHAR");
                    HC_RID = new intParameter("@HC_RID", base.inputParameterList);
                    TEXT_VALUE = new stringParameter("@TEXT_VALUE", base.inputParameterList);
                    DATE_VALUE = new datetimeParameter("@DATE_VALUE", base.inputParameterList);
                    NUMBER_VALUE = new floatParameter("@NUMBER_VALUE", base.inputParameterList);
                    DOLLAR_VALUE = new floatParameter("@DOLLAR_VALUE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                    int? HC_RID,
                                    string TEXT_VALUE,
                                    DateTime? DATE_VALUE,
                                    float? NUMBER_VALUE,
                                    float? DOLLAR_VALUE
                                    )
                {
                    lock (typeof(MID_HEADER_CHAR_UPDATE_def))
                    {
                        this.HC_RID.SetValue(HC_RID);
                        this.TEXT_VALUE.SetValue(TEXT_VALUE);
                        this.DATE_VALUE.SetValue(DATE_VALUE);
                        this.NUMBER_VALUE.SetValue(NUMBER_VALUE);
                        this.DOLLAR_VALUE.SetValue(DOLLAR_VALUE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

			public static MID_HEADER_CHAR_GROUP_READ_COUNT_FROM_ID_def MID_HEADER_CHAR_GROUP_READ_COUNT_FROM_ID = new MID_HEADER_CHAR_GROUP_READ_COUNT_FROM_ID_def();
			public class MID_HEADER_CHAR_GROUP_READ_COUNT_FROM_ID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_GROUP_READ_COUNT_FROM_ID.SQL"

			    private stringParameter HCG_ID;
			
			    public MID_HEADER_CHAR_GROUP_READ_COUNT_FROM_ID_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_GROUP_READ_COUNT_FROM_ID";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("HEADER_CHAR_GROUP");
			        HCG_ID = new stringParameter("@HCG_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, string HCG_ID)
			    {
                    lock (typeof(MID_HEADER_CHAR_GROUP_READ_COUNT_FROM_ID_def))
                    {
                        this.HCG_ID.SetValue(HCG_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_GROUP_READ_def MID_HEADER_CHAR_GROUP_READ = new MID_HEADER_CHAR_GROUP_READ_def();
			public class MID_HEADER_CHAR_GROUP_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_GROUP_READ.SQL"

			    private intParameter HCG_RID;
			
			    public MID_HEADER_CHAR_GROUP_READ_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_GROUP_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER_CHAR_GROUP");
			        HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HCG_RID)
			    {
                    lock (typeof(MID_HEADER_CHAR_GROUP_READ_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_HEADER_CHAR_GROUP_READ_ALL_def MID_HEADER_CHAR_GROUP_READ_ALL = new MID_HEADER_CHAR_GROUP_READ_ALL_def();
			public class MID_HEADER_CHAR_GROUP_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_GROUP_READ_ALL.SQL"

			
			    public MID_HEADER_CHAR_GROUP_READ_ALL_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_GROUP_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("HEADER_CHAR_GROUP");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_HEADER_CHAR_GROUP_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_HEADER_CHAR_READ_FOR_GROUP_MAINT_def MID_HEADER_CHAR_READ_FOR_GROUP_MAINT = new MID_HEADER_CHAR_READ_FOR_GROUP_MAINT_def();
            public class MID_HEADER_CHAR_READ_FOR_GROUP_MAINT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_FOR_GROUP_MAINT.SQL"

                private intParameter HCG_RID;

                public MID_HEADER_CHAR_READ_FOR_GROUP_MAINT_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_READ_FOR_GROUP_MAINT";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_CHAR");
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? HCG_RID
                                      )
                {
                    lock (typeof(MID_HEADER_CHAR_READ_FOR_GROUP_MAINT_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_CHAR_GROUP_READ_NAME_FOR_DUPLICATE_def MID_HEADER_CHAR_GROUP_READ_NAME_FOR_DUPLICATE = new MID_HEADER_CHAR_GROUP_READ_NAME_FOR_DUPLICATE_def();
            public class MID_HEADER_CHAR_GROUP_READ_NAME_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_GROUP_READ_NAME_FOR_DUPLICATE.SQL"

                private stringParameter PROPOSED_GROUP_NAME;
                private intParameter HCG_EDIT_RID;

                public MID_HEADER_CHAR_GROUP_READ_NAME_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_GROUP_READ_NAME_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_CHAR_GROUP");
                    PROPOSED_GROUP_NAME = new stringParameter("@PROPOSED_GROUP_NAME", base.inputParameterList);
                    HCG_EDIT_RID = new intParameter("@HCG_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      string PROPOSED_GROUP_NAME,
                                      int? HCG_EDIT_RID
                                      )
                {
                    lock (typeof(MID_HEADER_CHAR_GROUP_READ_NAME_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_GROUP_NAME.SetValue(PROPOSED_GROUP_NAME);
                        this.HCG_EDIT_RID.SetValue(HCG_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE_def MID_HEADER_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE = new MID_HEADER_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE_def();
            public class MID_HEADER_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE.SQL"

                private stringParameter PROPOSED_VALUE;
                private intParameter HCG_RID;
                private intParameter HC_EDIT_RID;

                public MID_HEADER_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_CHAR");
                    PROPOSED_VALUE = new stringParameter("@PROPOSED_VALUE", base.inputParameterList);
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                    HC_EDIT_RID = new intParameter("@HC_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      string PROPOSED_VALUE,
                                      int? HCG_RID,
                                      int? HC_EDIT_RID
                                      )
                {
                    lock (typeof(MID_HEADER_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_VALUE.SetValue(PROPOSED_VALUE);
                        this.HCG_RID.SetValue(HCG_RID);
                        this.HC_EDIT_RID.SetValue(HC_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_CHAR_READ_DATE_VALUE_FOR_DUPLICATE_def MID_HEADER_CHAR_READ_DATE_VALUE_FOR_DUPLICATE = new MID_HEADER_CHAR_READ_DATE_VALUE_FOR_DUPLICATE_def();
            public class MID_HEADER_CHAR_READ_DATE_VALUE_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_DATE_VALUE_FOR_DUPLICATE.SQL"

                private datetimeParameter PROPOSED_VALUE;
                private intParameter HCG_RID;
                private intParameter HC_EDIT_RID;

                public MID_HEADER_CHAR_READ_DATE_VALUE_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_READ_DATE_VALUE_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_CHAR");
                    PROPOSED_VALUE = new datetimeParameter("@PROPOSED_VALUE", base.inputParameterList);
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                    HC_EDIT_RID = new intParameter("@HC_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      DateTime? PROPOSED_VALUE,
                                      int? HCG_RID,
                                      int? HC_EDIT_RID
                                      )
                {
                    lock (typeof(MID_HEADER_CHAR_READ_DATE_VALUE_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_VALUE.SetValue(PROPOSED_VALUE);
                        this.HCG_RID.SetValue(HCG_RID);
                        this.HC_EDIT_RID.SetValue(HC_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE_def MID_HEADER_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE = new MID_HEADER_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE_def();
            public class MID_HEADER_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE.SQL"

                private floatParameter PROPOSED_VALUE;
                private intParameter HCG_RID;
                private intParameter HC_EDIT_RID;

                public MID_HEADER_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_CHAR");
                    PROPOSED_VALUE = new floatParameter("@PROPOSED_VALUE", base.inputParameterList);
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                    HC_EDIT_RID = new intParameter("@HC_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      float? PROPOSED_VALUE,
                                      int? HCG_RID,
                                      int? HC_EDIT_RID
                                      )
                {
                    lock (typeof(MID_HEADER_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_VALUE.SetValue(PROPOSED_VALUE);
                        this.HCG_RID.SetValue(HCG_RID);
                        this.HC_EDIT_RID.SetValue(HC_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_HEADER_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE_def MID_HEADER_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE = new MID_HEADER_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE_def();
            public class MID_HEADER_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE.SQL"

                private floatParameter PROPOSED_VALUE;
                private intParameter HCG_RID;
                private intParameter HC_EDIT_RID;

                public MID_HEADER_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_CHAR");
                    PROPOSED_VALUE = new floatParameter("@PROPOSED_VALUE", base.inputParameterList);
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                    HC_EDIT_RID = new intParameter("@HC_EDIT_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      float? PROPOSED_VALUE,
                                      int? HCG_RID,
                                      int? HC_EDIT_RID
                                      )
                {
                    lock (typeof(MID_HEADER_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE_def))
                    {
                        this.PROPOSED_VALUE.SetValue(PROPOSED_VALUE);
                        this.HCG_RID.SetValue(HCG_RID);
                        this.HC_EDIT_RID.SetValue(HC_EDIT_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }



            public static MID_HEADER_CHAR_GROUP_DELETE_def MID_HEADER_CHAR_GROUP_DELETE = new MID_HEADER_CHAR_GROUP_DELETE_def();
            public class MID_HEADER_CHAR_GROUP_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_GROUP_DELETE.SQL"

                private intParameter HCG_RID;

                public MID_HEADER_CHAR_GROUP_DELETE_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_GROUP_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_CHAR_GROUP");
                    HCG_RID = new intParameter("@HCG_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HCG_RID)
                {
                    lock (typeof(MID_HEADER_CHAR_GROUP_DELETE_def))
                    {
                        this.HCG_RID.SetValue(HCG_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }


            public static MID_HEADER_CHAR_DELETE_def MID_HEADER_CHAR_DELETE = new MID_HEADER_CHAR_DELETE_def();
            public class MID_HEADER_CHAR_DELETE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_DELETE.SQL"

                private intParameter HC_RID;

                public MID_HEADER_CHAR_DELETE_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_DELETE";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("HEADER_CHAR");
                    HC_RID = new intParameter("@HC_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? HC_RID)
                {
                    lock (typeof(MID_HEADER_CHAR_DELETE_def))
                    {
                        this.HC_RID.SetValue(HC_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }


            public static SP_MID_HEADER_CHAR_DATA_TYPE_def SP_MID_HEADER_CHAR_DATA_TYPE = new SP_MID_HEADER_CHAR_DATA_TYPE_def();
            public class SP_MID_HEADER_CHAR_DATA_TYPE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HEADER_CHAR_DATA_TYPE.SQL"

                private stringParameter CHARNAME;
                private intParameter HCG_TYPE; //Declare Output Parameter

                public SP_MID_HEADER_CHAR_DATA_TYPE_def()
                {
                    base.procedureName = "SP_MID_HEADER_CHAR_DATA_TYPE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_CHAR_DATA_TYPE");
                    CHARNAME = new stringParameter("@CHARNAME", base.inputParameterList);
                    HCG_TYPE = new intParameter("@HCG_TYPE", base.outputParameterList); //Add Output Parameter
                }

                public int ReadType(DatabaseAccess _dba, string CHARNAME)
                {
                    lock (typeof(SP_MID_HEADER_CHAR_DATA_TYPE_def))
                    {
                        this.CHARNAME.SetValue(CHARNAME);
                        this.HCG_TYPE.SetValue(null); //Initialize Output Parameter
                        DataTable dt = ExecuteStoredProcedureForRead(_dba);
                        return (int)HCG_TYPE.Value;
                    }
                }
            }


            public static MID_HEADER_CHAR_GROUP_READ_ALL_FOR_MAINT_def MID_HEADER_CHAR_GROUP_READ_ALL_FOR_MAINT = new MID_HEADER_CHAR_GROUP_READ_ALL_FOR_MAINT_def();
            public class MID_HEADER_CHAR_GROUP_READ_ALL_FOR_MAINT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_GROUP_READ_ALL_FOR_MAINT.SQL"


                public MID_HEADER_CHAR_GROUP_READ_ALL_FOR_MAINT_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_GROUP_READ_ALL_FOR_MAINT";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("HEADER_CHAR_GROUP");
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba)
                {
                    lock (typeof(MID_HEADER_CHAR_GROUP_READ_ALL_FOR_MAINT_def))
                    {
                        return ExecuteStoredProcedureForReadAsDataSet(_dba);
                    }
                }
            }




			public static MID_HEADER_CHAR_JOIN_READ_COUNT_def MID_HEADER_CHAR_JOIN_READ_COUNT = new MID_HEADER_CHAR_JOIN_READ_COUNT_def();
			public class MID_HEADER_CHAR_JOIN_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_JOIN_READ_COUNT.SQL"

			    private intParameter HC_RID;
			
			    public MID_HEADER_CHAR_JOIN_READ_COUNT_def()
			    {
			        base.procedureName = "MID_HEADER_CHAR_JOIN_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("HEADER_CHAR_JOIN");
			        HC_RID = new intParameter("@HC_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, int? HC_RID)
			    {
                    lock (typeof(MID_HEADER_CHAR_JOIN_READ_COUNT_def))
                    {
                        this.HC_RID.SetValue(HC_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

            //Begin TT#1313-MD -jsobek -Header Filters
            public static MID_HEADER_CHAR_READ_FOR_HEADER_LIST_def MID_HEADER_CHAR_READ_FOR_HEADER_LIST = new MID_HEADER_CHAR_READ_FOR_HEADER_LIST_def();
            public class MID_HEADER_CHAR_READ_FOR_HEADER_LIST_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_CHAR_READ_FOR_HEADER_LIST.SQL"

                private tableParameter HDR_RID_LIST;

                public MID_HEADER_CHAR_READ_FOR_HEADER_LIST_def()
                {
                    base.procedureName = "MID_HEADER_CHAR_READ_FOR_HEADER_LIST";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("HEADER_CHAR");
                    HDR_RID_LIST = new tableParameter("@HDR_RID_LIST", "HDR_RID_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, DataTable HDR_RID_LIST)
                {
                    lock (typeof(MID_HEADER_CHAR_READ_FOR_HEADER_LIST_def))
                    {
                        this.HDR_RID_LIST.SetValue(HDR_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#1313-MD -jsobek -Header Filters

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
