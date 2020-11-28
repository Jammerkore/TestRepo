using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public partial class AssortmentViewData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static SP_MID_GET_POST_RECEIPT_ASRT_VIEWS_def SP_MID_GET_POST_RECEIPT_ASRT_VIEWS = new SP_MID_GET_POST_RECEIPT_ASRT_VIEWS_def();
			public class SP_MID_GET_POST_RECEIPT_ASRT_VIEWS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_POST_RECEIPT_ASRT_VIEWS.SQL"

			    private stringParameter USER_RID_LIST;

                public SP_MID_GET_POST_RECEIPT_ASRT_VIEWS_def()
			    {
                    base.procedureName = "SP_MID_GET_POST_RECEIPT_ASRT_VIEWS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GET_POST_RECEIPT_ASRT_VIEWS");
			        USER_RID_LIST = new stringParameter("@USER_RID_LIST", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string USER_RID_LIST)
			    {
                    lock (typeof(SP_MID_GET_POST_RECEIPT_ASRT_VIEWS_def))
                    {
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_ASSORTMENT_READ_def MID_USER_ASSORTMENT_READ = new MID_USER_ASSORTMENT_READ_def();
			public class MID_USER_ASSORTMENT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_READ.SQL"

			    private intParameter USER_RID;
			
			    public MID_USER_ASSORTMENT_READ_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("USER_ASSORTMENT");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_USER_ASSORTMENT_READ_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_VIEW_DETAIL_DELETE_def MID_ASSORTMENT_VIEW_DETAIL_DELETE = new MID_ASSORTMENT_VIEW_DETAIL_DELETE_def();
			public class MID_ASSORTMENT_VIEW_DETAIL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_VIEW_DETAIL_DELETE.SQL"

			    private intParameter VIEW_RID;
			
			    public MID_ASSORTMENT_VIEW_DETAIL_DELETE_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_VIEW_DETAIL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ASSORTMENT_VIEW_DETAIL");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_VIEW_DETAIL_DELETE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_VIEW_DELETE_def MID_ASSORTMENT_VIEW_DELETE = new MID_ASSORTMENT_VIEW_DELETE_def();
			public class MID_ASSORTMENT_VIEW_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_VIEW_DELETE.SQL"

			    private intParameter VIEW_RID;
			
			    public MID_ASSORTMENT_VIEW_DELETE_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_VIEW_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ASSORTMENT_VIEW");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_VIEW_DELETE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_USER_ASSORTMENT_UPDATE_FOR_VIEW_DELETE_def MID_USER_ASSORTMENT_UPDATE_FOR_VIEW_DELETE = new MID_USER_ASSORTMENT_UPDATE_FOR_VIEW_DELETE_def();
			public class MID_USER_ASSORTMENT_UPDATE_FOR_VIEW_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_UPDATE_FOR_VIEW_DELETE.SQL"

			    private intParameter VIEW_RID;
			    private intParameter DEFAULT_VIEW_RID;
			
			    public MID_USER_ASSORTMENT_UPDATE_FOR_VIEW_DELETE_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_UPDATE_FOR_VIEW_DELETE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_ASSORTMENT");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			        DEFAULT_VIEW_RID = new intParameter("@DEFAULT_VIEW_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? VIEW_RID,
			                      int? DEFAULT_VIEW_RID
			                      )
			    {
                    lock (typeof(MID_USER_ASSORTMENT_UPDATE_FOR_VIEW_DELETE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.DEFAULT_VIEW_RID.SetValue(DEFAULT_VIEW_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}


			public static MID_ASSORTMENT_VIEW_DETAIL_INSERT_def MID_ASSORTMENT_VIEW_DETAIL_INSERT = new MID_ASSORTMENT_VIEW_DETAIL_INSERT_def();
			public class MID_ASSORTMENT_VIEW_DETAIL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_VIEW_DETAIL_INSERT.SQL"

			    private intParameter VIEW_RID;
			    private intParameter AXIS;
			    private intParameter AXIS_SEQUENCE;
			    private intParameter PROFILE_TYPE;
			    private intParameter PROFILE_KEY;
			    private charParameter SUMMARIZED_IND;
			
			    public MID_ASSORTMENT_VIEW_DETAIL_INSERT_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_VIEW_DETAIL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ASSORTMENT_VIEW_DETAIL");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			        AXIS = new intParameter("@AXIS", base.inputParameterList);
			        AXIS_SEQUENCE = new intParameter("@AXIS_SEQUENCE", base.inputParameterList);
			        PROFILE_TYPE = new intParameter("@PROFILE_TYPE", base.inputParameterList);
			        PROFILE_KEY = new intParameter("@PROFILE_KEY", base.inputParameterList);
			        SUMMARIZED_IND = new charParameter("@SUMMARIZED_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? VIEW_RID,
			                      int? AXIS,
			                      int? AXIS_SEQUENCE,
			                      int? PROFILE_TYPE,
			                      int? PROFILE_KEY,
			                      char? SUMMARIZED_IND
			                      )
			    {
                    lock (typeof(MID_ASSORTMENT_VIEW_DETAIL_INSERT_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.AXIS.SetValue(AXIS);
                        this.AXIS_SEQUENCE.SetValue(AXIS_SEQUENCE);
                        this.PROFILE_TYPE.SetValue(PROFILE_TYPE);
                        this.PROFILE_KEY.SetValue(PROFILE_KEY);
                        this.SUMMARIZED_IND.SetValue(SUMMARIZED_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_VIEW_DETAIL_READ_def MID_ASSORTMENT_VIEW_DETAIL_READ = new MID_ASSORTMENT_VIEW_DETAIL_READ_def();
			public class MID_ASSORTMENT_VIEW_DETAIL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_VIEW_DETAIL_READ.SQL"

			    private intParameter VIEW_RID;
			
			    public MID_ASSORTMENT_VIEW_DETAIL_READ_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_VIEW_DETAIL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_VIEW_DETAIL");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_VIEW_DETAIL_READ_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_VIEW_READ_GET_KEY_def MID_ASSORTMENT_VIEW_READ_GET_KEY = new MID_ASSORTMENT_VIEW_READ_GET_KEY_def();
			public class MID_ASSORTMENT_VIEW_READ_GET_KEY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_VIEW_READ_GET_KEY.SQL"

			    private intParameter USER_RID;
			    private stringParameter VIEW_ID;
                private intParameter ASSORTMENT_VIEW_TYPE;	// TT#1456-MD - stodd - When saving views in Assortment with same name as a Group Allocation View, the Group Allocation view is overlaid.
			
			    public MID_ASSORTMENT_VIEW_READ_GET_KEY_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_VIEW_READ_GET_KEY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_VIEW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        VIEW_ID = new stringParameter("@VIEW_ID", base.inputParameterList);
                    ASSORTMENT_VIEW_TYPE = new intParameter("@ASSORTMENT_VIEW_TYPE", base.inputParameterList);	// TT#1456-MD - stodd - When saving views in Assortment with same name as a Group Allocation View, the Group Allocation view is overlaid.
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? USER_RID,
			                          string VIEW_ID,
                                      eAssortmentViewType ASSORTMENT_VIEW_TYPE	// TT#1456-MD - stodd - When saving views in Assortment with same name as a Group Allocation View, the Group Allocation view is overlaid.
			                          )
			    {
                    lock (typeof(MID_ASSORTMENT_VIEW_READ_GET_KEY_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.VIEW_ID.SetValue(VIEW_ID);
                        this.ASSORTMENT_VIEW_TYPE.SetValue((int)ASSORTMENT_VIEW_TYPE);	// TT#1456-MD - stodd - When saving views in Assortment with same name as a Group Allocation View, the Group Allocation view is overlaid.
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_USER_ASSORTMENT_UPDATE_def MID_USER_ASSORTMENT_UPDATE = new MID_USER_ASSORTMENT_UPDATE_def();
			public class MID_USER_ASSORTMENT_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_USER_ASSORTMENT_UPDATE.SQL"

			    private intParameter VIEW_RID;
			    private intParameter USER_RID;
			
			    public MID_USER_ASSORTMENT_UPDATE_def()
			    {
			        base.procedureName = "MID_USER_ASSORTMENT_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("USER_ASSORTMENT");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? VIEW_RID,
			                      int? USER_RID
			                      )
			    {
                    lock (typeof(MID_USER_ASSORTMENT_UPDATE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_ASSORTMENT_VIEW_INSERT_def SP_MID_ASSORTMENT_VIEW_INSERT = new SP_MID_ASSORTMENT_VIEW_INSERT_def();
            public class SP_MID_ASSORTMENT_VIEW_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_ASSORTMENT_VIEW_INSERT.SQL"

			    private intParameter USER_RID;
			    private stringParameter VIEW_ID;
			    private intParameter GROUP_BY_ID;
                private intParameter SG_RID;		// TT#4247 - stodd - Store attribute not being saved in matrix view
                private intParameter ASSORTMENT_VIEW_TYPE; //TT#1268-MD -jsobek -5.4 Merge
			    private intParameter VIEW_RID; //Declare Output Parameter
			
			    public SP_MID_ASSORTMENT_VIEW_INSERT_def()
			    {
                    base.procedureName = "SP_MID_ASSORTMENT_VIEW_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("ASSORTMENT_VIEW");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        VIEW_ID = new stringParameter("@VIEW_ID", base.inputParameterList);
			        GROUP_BY_ID = new intParameter("@GROUP_BY_ID", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);		// TT#4247 - stodd - Store attribute not being saved in matrix view
                    ASSORTMENT_VIEW_TYPE = new intParameter("@ASSORTMENT_VIEW_TYPE", base.inputParameterList); //TT#1268-MD -jsobek -5.4 Merge
			        VIEW_RID = new intParameter("@VIEW_RID", base.outputParameterList); //Add Output Parameter
			    }

                public int InsertAndReturnRID(DatabaseAccess _dba, 
			                      int? USER_RID,
			                      string VIEW_ID,
			                      int? GROUP_BY_ID,
                                  int? SG_RID,		// TT#4247 - stodd - Store attribute not being saved in matrix view
                                  int? ASSORTMENT_VIEW_TYPE //TT#1268-MD -jsobek -5.4 Merge
			                      )
			    {
                    lock (typeof(SP_MID_ASSORTMENT_VIEW_INSERT_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        this.VIEW_ID.SetValue(VIEW_ID);
                        this.GROUP_BY_ID.SetValue(GROUP_BY_ID);
                        this.SG_RID.SetValue(SG_RID);		// TT#4247 - stodd - Store attribute not being saved in matrix view
                        this.ASSORTMENT_VIEW_TYPE.SetValue(ASSORTMENT_VIEW_TYPE); //TT#1268-MD -jsobek -5.4 Merge
                        this.VIEW_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

            //Begin TT#1268-MD -jsobek -5.4 Merge
            public static MID_ASSORTMENT_VIEW_UPDATE_def MID_ASSORTMENT_VIEW_UPDATE = new MID_ASSORTMENT_VIEW_UPDATE_def();
            public class MID_ASSORTMENT_VIEW_UPDATE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_VIEW_UPDATE.SQL"

                private intParameter VIEW_RID;
                private intParameter USER_RID;
                private stringParameter VIEW_ID;
                private intParameter GROUP_BY_ID;
                private intParameter SG_RID;		// TT#4247 - stodd - Store attribute not being saved in matrix view
                private intParameter ASSORTMENT_VIEW_TYPE;

                public MID_ASSORTMENT_VIEW_UPDATE_def()
                {
                    base.procedureName = "MID_ASSORTMENT_VIEW_UPDATE";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("ASSORTMENT_VIEW");
                    VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                    VIEW_ID = new stringParameter("@VIEW_ID", base.inputParameterList);
                    GROUP_BY_ID = new intParameter("@GROUP_BY_ID", base.inputParameterList);
                    SG_RID = new intParameter("@SG_RID", base.inputParameterList);		// TT#4247 - stodd - Store attribute not being saved in matrix view
                    ASSORTMENT_VIEW_TYPE = new intParameter("@ASSORTMENT_VIEW_TYPE", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba,
                                  int? VIEW_RID,
                                  int? USER_RID,
                                  string VIEW_ID,
                                  int? GROUP_BY_ID,
                                  int? SG_RID,		// TT#4247 - stodd - Store attribute not being saved in matrix view
                                  int? ASSORTMENT_VIEW_TYPE
                                  )
                {
                    lock (typeof(MID_ASSORTMENT_VIEW_UPDATE_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.VIEW_ID.SetValue(VIEW_ID);
                        this.GROUP_BY_ID.SetValue(GROUP_BY_ID);
                        this.SG_RID.SetValue(SG_RID);		// TT#4247 - stodd - Store attribute not being saved in matrix view
                        this.ASSORTMENT_VIEW_TYPE.SetValue(ASSORTMENT_VIEW_TYPE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }
            //End TT#1268-MD -jsobek -5.4 Merge

			public static MID_ASSORTMENT_VIEW_READ_VIEW_RID_def MID_ASSORTMENT_VIEW_READ_VIEW_RID = new MID_ASSORTMENT_VIEW_READ_VIEW_RID_def();
			public class MID_ASSORTMENT_VIEW_READ_VIEW_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_VIEW_READ_VIEW_RID.SQL"

			    private intParameter VIEW_RID;
			
			    public MID_ASSORTMENT_VIEW_READ_VIEW_RID_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_VIEW_READ_VIEW_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_VIEW");
			        VIEW_RID = new intParameter("@VIEW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? VIEW_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_VIEW_READ_VIEW_RID_def))
                    {
                        this.VIEW_RID.SetValue(VIEW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_VIEW_READ_ALL_def MID_ASSORTMENT_VIEW_READ_ALL = new MID_ASSORTMENT_VIEW_READ_ALL_def();
			public class MID_ASSORTMENT_VIEW_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_VIEW_READ_ALL.SQL"

			    private tableParameter USER_RID_LIST;
			
			    public MID_ASSORTMENT_VIEW_READ_ALL_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_VIEW_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_VIEW");
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, DataTable USER_RID_LIST)
			    {
                    lock (typeof(MID_ASSORTMENT_VIEW_READ_ALL_def))
                    {
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            //Begin TT#1268-MD -jsobek -5.4 Merge
            public static MID_ASSORTMENT_VIEW_READ_BY_TYPE_def MID_ASSORTMENT_VIEW_READ_BY_TYPE = new MID_ASSORTMENT_VIEW_READ_BY_TYPE_def();
            public class MID_ASSORTMENT_VIEW_READ_BY_TYPE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_VIEW_READ_BY_TYPE.SQL"

                private tableParameter USER_RID_LIST;
                private intParameter ASSORTMENT_VIEW_TYPE;

                public MID_ASSORTMENT_VIEW_READ_BY_TYPE_def()
                {
                    base.procedureName = "MID_ASSORTMENT_VIEW_READ_BY_TYPE";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("ASSORTMENT_VIEW");
                    USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
                    ASSORTMENT_VIEW_TYPE = new intParameter("@ASSORTMENT_VIEW_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, DataTable USER_RID_LIST, int? ASSORTMENT_VIEW_TYPE)
                {
                    lock (typeof(MID_ASSORTMENT_VIEW_READ_BY_TYPE_def))
                    {
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        this.ASSORTMENT_VIEW_TYPE.SetValue(ASSORTMENT_VIEW_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }
            //End TT#1268-MD -jsobek -5.4 Merge

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
