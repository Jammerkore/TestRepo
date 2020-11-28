using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class FolderDataLayer : DataLayer
    {
        protected static class StoredProcedures
        {

			public static MID_FOLDER_READ_OWNED_def MID_FOLDER_READ_OWNED = new MID_FOLDER_READ_OWNED_def();
			public class MID_FOLDER_READ_OWNED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_OWNED.SQL"

			    private intParameter FOLDER_TYPE;
			    private intParameter ITEM_TYPE;
			
			    public MID_FOLDER_READ_OWNED_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_OWNED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE = new intParameter("@FOLDER_TYPE", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FOLDER_TYPE,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_OWNED_def))
                    {
                        this.FOLDER_TYPE.SetValue(FOLDER_TYPE);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_OWNED_FOR_USERS_def MID_FOLDER_READ_OWNED_FOR_USERS = new MID_FOLDER_READ_OWNED_FOR_USERS_def();
			public class MID_FOLDER_READ_OWNED_FOR_USERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_OWNED_FOR_USERS.SQL"

			    private intParameter FOLDER_TYPE;
			    private intParameter ITEM_TYPE;
			    private tableParameter USER_RID_LIST;
			
			    public MID_FOLDER_READ_OWNED_FOR_USERS_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_OWNED_FOR_USERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE = new intParameter("@FOLDER_TYPE", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FOLDER_TYPE,
			                          int? ITEM_TYPE,
			                          DataTable USER_RID_LIST
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_OWNED_FOR_USERS_def))
                    {
                        this.FOLDER_TYPE.SetValue(FOLDER_TYPE);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_ASSIGNED_def MID_FOLDER_READ_ASSIGNED = new MID_FOLDER_READ_ASSIGNED_def();
			public class MID_FOLDER_READ_ASSIGNED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_ASSIGNED.SQL"

			    private intParameter FOLDER_TYPE;
			    private intParameter ITEM_TYPE;
			
			    public MID_FOLDER_READ_ASSIGNED_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_ASSIGNED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE = new intParameter("@FOLDER_TYPE", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FOLDER_TYPE,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_ASSIGNED_def))
                    {
                        this.FOLDER_TYPE.SetValue(FOLDER_TYPE);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_ASSIGNED_FOR_USERS_def MID_FOLDER_READ_ASSIGNED_FOR_USERS = new MID_FOLDER_READ_ASSIGNED_FOR_USERS_def();
			public class MID_FOLDER_READ_ASSIGNED_FOR_USERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_ASSIGNED_FOR_USERS.SQL"

			    private intParameter FOLDER_TYPE;
			    private intParameter ITEM_TYPE;
			    private tableParameter USER_RID_LIST;
			
			    public MID_FOLDER_READ_ASSIGNED_FOR_USERS_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_ASSIGNED_FOR_USERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE = new intParameter("@FOLDER_TYPE", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FOLDER_TYPE,
			                          int? ITEM_TYPE,
			                          DataTable USER_RID_LIST
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_ASSIGNED_FOR_USERS_def))
                    {
                        this.FOLDER_TYPE.SetValue(FOLDER_TYPE);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_ASSIGNED_AND_OWNED_def MID_FOLDER_READ_ASSIGNED_AND_OWNED = new MID_FOLDER_READ_ASSIGNED_AND_OWNED_def();
			public class MID_FOLDER_READ_ASSIGNED_AND_OWNED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_ASSIGNED_AND_OWNED.SQL"

			    private intParameter FOLDER_TYPE;
			    private intParameter ITEM_TYPE;
			
			    public MID_FOLDER_READ_ASSIGNED_AND_OWNED_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_ASSIGNED_AND_OWNED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE = new intParameter("@FOLDER_TYPE", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FOLDER_TYPE,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_ASSIGNED_AND_OWNED_def))
                    {
                        this.FOLDER_TYPE.SetValue(FOLDER_TYPE);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_ASSIGNED_AND_OWNED_FOR_USERS_def MID_FOLDER_READ_ASSIGNED_AND_OWNED_FOR_USERS = new MID_FOLDER_READ_ASSIGNED_AND_OWNED_FOR_USERS_def();
			public class MID_FOLDER_READ_ASSIGNED_AND_OWNED_FOR_USERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_ASSIGNED_AND_OWNED_FOR_USERS.SQL"

			    private intParameter FOLDER_TYPE;
			    private intParameter ITEM_TYPE;
			    private tableParameter USER_RID_LIST;
			
			    public MID_FOLDER_READ_ASSIGNED_AND_OWNED_FOR_USERS_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_ASSIGNED_AND_OWNED_FOR_USERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE = new intParameter("@FOLDER_TYPE", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FOLDER_TYPE,
			                          int? ITEM_TYPE,
			                          DataTable USER_RID_LIST
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_ASSIGNED_AND_OWNED_FOR_USERS_def))
                    {
                        this.FOLDER_TYPE.SetValue(FOLDER_TYPE);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_def MID_FOLDER_READ = new MID_FOLDER_READ_def();
			public class MID_FOLDER_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ.SQL"

			    private intParameter FOLDER_TYPE;
			    private intParameter ITEM_TYPE;
			
			    public MID_FOLDER_READ_def()
			    {
			        base.procedureName = "MID_FOLDER_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE = new intParameter("@FOLDER_TYPE", base.inputParameterList);
			        ITEM_TYPE = new intParameter("@ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FOLDER_TYPE,
			                          int? ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_def))
                    {
                        this.FOLDER_TYPE.SetValue(FOLDER_TYPE);
                        this.ITEM_TYPE.SetValue(ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_FROM_RID_def MID_FOLDER_READ_FROM_RID = new MID_FOLDER_READ_FROM_RID_def();
			public class MID_FOLDER_READ_FROM_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_FROM_RID.SQL"

			    private intParameter FOLDER_RID;
			
			    public MID_FOLDER_READ_FROM_RID_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_FROM_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_RID = new intParameter("@FOLDER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FOLDER_RID)
			    {
                    lock (typeof(MID_FOLDER_READ_FROM_RID_def))
                    {
                        this.FOLDER_RID.SetValue(FOLDER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_KEY_def MID_FOLDER_READ_KEY = new MID_FOLDER_READ_KEY_def();
			public class MID_FOLDER_READ_KEY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_KEY.SQL"

			    private intParameter FOLDER_TYPE;
			    private intParameter USER_RID;
			    private stringParameter FOLDER_ID;
			    private intParameter PARENT_FOLDER_RID;
			
			    public MID_FOLDER_READ_KEY_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_KEY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE = new intParameter("@FOLDER_TYPE", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        FOLDER_ID = new stringParameter("@FOLDER_ID", base.inputParameterList);
			        PARENT_FOLDER_RID = new intParameter("@PARENT_FOLDER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? FOLDER_TYPE,
			                          int? USER_RID,
			                          string FOLDER_ID,
			                          int? PARENT_FOLDER_RID
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_KEY_def))
                    {
                        this.FOLDER_TYPE.SetValue(FOLDER_TYPE);
                        this.USER_RID.SetValue(USER_RID);
                        this.FOLDER_ID.SetValue(FOLDER_ID);
                        this.PARENT_FOLDER_RID.SetValue(PARENT_FOLDER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_FOLDER_INSERT_def SP_MID_FOLDER_INSERT = new SP_MID_FOLDER_INSERT_def();
            public class SP_MID_FOLDER_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_FOLDER_INSERT.SQL"

                private intParameter USER_ID;
                private stringParameter FOLDER_ID;
                private intParameter FOLDER_TYPE;
                private intParameter FOLDER_RID; //Declare Output Parameter

                public SP_MID_FOLDER_INSERT_def()
                {
                    base.procedureName = "SP_MID_FOLDER_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("FOLDER");
                    USER_ID = new intParameter("@USER_ID", base.inputParameterList);
                    FOLDER_ID = new stringParameter("@FOLDER_ID", base.inputParameterList);
                    FOLDER_TYPE = new intParameter("@FOLDER_TYPE", base.inputParameterList);
                    FOLDER_RID = new intParameter("@FOLDER_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              int? USER_ID,
                                              string FOLDER_ID,
                                              int? FOLDER_TYPE
                                              )
                {
                    lock (typeof(SP_MID_FOLDER_INSERT_def))
                    {
                        this.USER_ID.SetValue(USER_ID);
                        this.FOLDER_ID.SetValue(FOLDER_ID);
                        this.FOLDER_TYPE.SetValue(FOLDER_TYPE);
                        this.FOLDER_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

			public static MID_FOLDER_UPDATE_def MID_FOLDER_UPDATE = new MID_FOLDER_UPDATE_def();
			public class MID_FOLDER_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_UPDATE.SQL"

			    private intParameter FOLDER_RID;
			    private intParameter USER_RID;
			    private stringParameter FOLDER_ID;
			    private intParameter FOLDER_TYPE;
			
			    public MID_FOLDER_UPDATE_def()
			    {
			        base.procedureName = "MID_FOLDER_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("FOLDER");
			        FOLDER_RID = new intParameter("@FOLDER_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        FOLDER_ID = new stringParameter("@FOLDER_ID", base.inputParameterList);
			        FOLDER_TYPE = new intParameter("@FOLDER_TYPE", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? FOLDER_RID,
			                      int? USER_RID,
			                      string FOLDER_ID,
			                      int? FOLDER_TYPE
			                      )
			    {
                    lock (typeof(MID_FOLDER_UPDATE_def))
                    {
                        this.FOLDER_RID.SetValue(FOLDER_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.FOLDER_ID.SetValue(FOLDER_ID);
                        this.FOLDER_TYPE.SetValue(FOLDER_TYPE);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_FOLDER_DELETE_def MID_FOLDER_DELETE = new MID_FOLDER_DELETE_def();
			public class MID_FOLDER_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_DELETE.SQL"

			    private intParameter FOLDER_RID;
			
			    public MID_FOLDER_DELETE_def()
			    {
			        base.procedureName = "MID_FOLDER_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FOLDER");
			        FOLDER_RID = new intParameter("@FOLDER_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? FOLDER_RID)
			    {
                    lock (typeof(MID_FOLDER_DELETE_def))
                    {
                        this.FOLDER_RID.SetValue(FOLDER_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_FOLDER_UPDATE_ID_def MID_FOLDER_UPDATE_ID = new MID_FOLDER_UPDATE_ID_def();
			public class MID_FOLDER_UPDATE_ID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_UPDATE_ID.SQL"

			    private intParameter FOLDER_RID;
			    private stringParameter FOLDER_ID;
			
			    public MID_FOLDER_UPDATE_ID_def()
			    {
			        base.procedureName = "MID_FOLDER_UPDATE_ID";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("FOLDER");
			        FOLDER_RID = new intParameter("@FOLDER_RID", base.inputParameterList);
			        FOLDER_ID = new stringParameter("@FOLDER_ID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? FOLDER_RID,
			                      string FOLDER_ID
			                      )
			    {
                    lock (typeof(MID_FOLDER_UPDATE_ID_def))
                    {
                        this.FOLDER_RID.SetValue(FOLDER_RID);
                        this.FOLDER_ID.SetValue(FOLDER_ID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static MID_FOLDER_READ_CHILDREN_def MID_FOLDER_READ_CHILDREN = new MID_FOLDER_READ_CHILDREN_def();
            public class MID_FOLDER_READ_CHILDREN_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_CHILDREN.SQL"

                private tableParameter FOLDER_TYPE_LIST;
                private intParameter CHILD_ITEM_TYPE;

                public MID_FOLDER_READ_CHILDREN_def()
                {
                    base.procedureName = "MID_FOLDER_READ_CHILDREN";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("FOLDER");
                    FOLDER_TYPE_LIST = new tableParameter("@FOLDER_TYPE_LIST", "FOLDER_TYPE", base.inputParameterList);
                    CHILD_ITEM_TYPE = new intParameter("@CHILD_ITEM_TYPE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      DataTable FOLDER_TYPE_LIST,
                                      int? CHILD_ITEM_TYPE
                                      )
                {
                    lock (typeof(MID_FOLDER_READ_CHILDREN_def))
                    {
                        this.FOLDER_TYPE_LIST.SetValue(FOLDER_TYPE_LIST);
                        this.CHILD_ITEM_TYPE.SetValue(CHILD_ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }


			public static MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_AND_OWNED_def MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_AND_OWNED = new MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_AND_OWNED_def();
			public class MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_AND_OWNED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_AND_OWNED.SQL"

			    private tableParameter FOLDER_TYPE_LIST;
			    private intParameter CHILD_ITEM_TYPE;
			    private tableParameter USER_RID_LIST;
			
			    public MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_AND_OWNED_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_AND_OWNED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE_LIST = new tableParameter("@FOLDER_TYPE_LIST", "FOLDER_TYPE", base.inputParameterList);
			        CHILD_ITEM_TYPE = new intParameter("@CHILD_ITEM_TYPE", base.inputParameterList);
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          DataTable FOLDER_TYPE_LIST,
			                          int? CHILD_ITEM_TYPE,
			                          DataTable USER_RID_LIST
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_AND_OWNED_def))
                    {
                        this.FOLDER_TYPE_LIST.SetValue(FOLDER_TYPE_LIST);
                        this.CHILD_ITEM_TYPE.SetValue(CHILD_ITEM_TYPE);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_def MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED = new MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_def();
			public class MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED.SQL"

			    private tableParameter FOLDER_TYPE_LIST;
			    private intParameter CHILD_ITEM_TYPE;
			    private tableParameter USER_RID_LIST;
			
			    public MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE_LIST = new tableParameter("@FOLDER_TYPE_LIST", "FOLDER_TYPE", base.inputParameterList);
			        CHILD_ITEM_TYPE = new intParameter("@CHILD_ITEM_TYPE", base.inputParameterList);
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          DataTable FOLDER_TYPE_LIST,
			                          int? CHILD_ITEM_TYPE,
			                          DataTable USER_RID_LIST
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_def))
                    {
                        this.FOLDER_TYPE_LIST.SetValue(FOLDER_TYPE_LIST);
                        this.CHILD_ITEM_TYPE.SetValue(CHILD_ITEM_TYPE);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_CHILDREN_FOR_OWNED_def MID_FOLDER_READ_CHILDREN_FOR_OWNED = new MID_FOLDER_READ_CHILDREN_FOR_OWNED_def();
			public class MID_FOLDER_READ_CHILDREN_FOR_OWNED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_CHILDREN_FOR_OWNED.SQL"

			    private tableParameter FOLDER_TYPE_LIST;
			    private intParameter CHILD_ITEM_TYPE;
			    private tableParameter USER_RID_LIST;
			
			    public MID_FOLDER_READ_CHILDREN_FOR_OWNED_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_CHILDREN_FOR_OWNED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE_LIST = new tableParameter("@FOLDER_TYPE_LIST", "FOLDER_TYPE", base.inputParameterList);
			        CHILD_ITEM_TYPE = new intParameter("@CHILD_ITEM_TYPE", base.inputParameterList);
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          DataTable FOLDER_TYPE_LIST,
			                          int? CHILD_ITEM_TYPE,
			                          DataTable USER_RID_LIST
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_CHILDREN_FOR_OWNED_def))
                    {
                        this.FOLDER_TYPE_LIST.SetValue(FOLDER_TYPE_LIST);
                        this.CHILD_ITEM_TYPE.SetValue(CHILD_ITEM_TYPE);
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_NO_USERS_def MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_NO_USERS = new MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_NO_USERS_def();
			public class MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_NO_USERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_NO_USERS.SQL"

			    private tableParameter FOLDER_TYPE_LIST;
			    private intParameter CHILD_ITEM_TYPE;
			
			    public MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_NO_USERS_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_NO_USERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_TYPE_LIST = new tableParameter("@FOLDER_TYPE_LIST", "FOLDER_TYPE", base.inputParameterList);
			        CHILD_ITEM_TYPE = new intParameter("@CHILD_ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          DataTable FOLDER_TYPE_LIST,
			                          int? CHILD_ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_CHILDREN_FOR_ASSIGNED_NO_USERS_def))
                    {
                        this.FOLDER_TYPE_LIST.SetValue(FOLDER_TYPE_LIST);
                        this.CHILD_ITEM_TYPE.SetValue(CHILD_ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_CHILDREN_WITH_SHORTCUTS_def MID_FOLDER_READ_CHILDREN_WITH_SHORTCUTS = new MID_FOLDER_READ_CHILDREN_WITH_SHORTCUTS_def();
			public class MID_FOLDER_READ_CHILDREN_WITH_SHORTCUTS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_CHILDREN_WITH_SHORTCUTS.SQL"

			    private intParameter PARENT_FOLDER_RID;
			    private intParameter USER_RID;
			
			    public MID_FOLDER_READ_CHILDREN_WITH_SHORTCUTS_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_CHILDREN_WITH_SHORTCUTS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        PARENT_FOLDER_RID = new intParameter("@PARENT_FOLDER_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? PARENT_FOLDER_RID,
			                          int? USER_RID
			                          )
			    {
                    lock (typeof(MID_FOLDER_READ_CHILDREN_WITH_SHORTCUTS_def))
                    {
                        this.PARENT_FOLDER_RID.SetValue(PARENT_FOLDER_RID);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_METHOD_READ_USER_def MID_METHOD_READ_USER = new MID_METHOD_READ_USER_def();
			public class MID_METHOD_READ_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_METHOD_READ_USER.SQL"

			    private intParameter METHOD_RID;
			
			    public MID_METHOD_READ_USER_def()
			    {
			        base.procedureName = "MID_METHOD_READ_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("METHOD");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_METHOD_READ_USER_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_WORKFLOW_READ_USER_def MID_WORKFLOW_READ_USER = new MID_WORKFLOW_READ_USER_def();
			public class MID_WORKFLOW_READ_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_WORKFLOW_READ_USER.SQL"

			    private intParameter WORKFLOW_RID;
			
			    public MID_WORKFLOW_READ_USER_def()
			    {
			        base.procedureName = "MID_WORKFLOW_READ_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("WORKFLOW");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? WORKFLOW_RID)
			    {
                    lock (typeof(MID_WORKFLOW_READ_USER_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_READ_USER_def MID_FOLDER_READ_USER = new MID_FOLDER_READ_USER_def();
			public class MID_FOLDER_READ_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_READ_USER.SQL"

			    private intParameter FOLDER_RID;
			
			    public MID_FOLDER_READ_USER_def()
			    {
			        base.procedureName = "MID_FOLDER_READ_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER");
			        FOLDER_RID = new intParameter("@FOLDER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FOLDER_RID)
			    {
                    lock (typeof(MID_FOLDER_READ_USER_def))
                    {
                        this.FOLDER_RID.SetValue(FOLDER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_JOIN_INSERT_def MID_FOLDER_JOIN_INSERT = new MID_FOLDER_JOIN_INSERT_def();
			public class MID_FOLDER_JOIN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_JOIN_INSERT.SQL"

			    private intParameter PARENT_FOLDER_RID;
			    private intParameter CHILD_ITEM_RID;
			    private intParameter CHILD_ITEM_TYPE;
			
			    public MID_FOLDER_JOIN_INSERT_def()
			    {
			        base.procedureName = "MID_FOLDER_JOIN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("FOLDER_JOIN");
			        PARENT_FOLDER_RID = new intParameter("@PARENT_FOLDER_RID", base.inputParameterList);
			        CHILD_ITEM_RID = new intParameter("@CHILD_ITEM_RID", base.inputParameterList);
			        CHILD_ITEM_TYPE = new intParameter("@CHILD_ITEM_TYPE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? PARENT_FOLDER_RID,
			                      int? CHILD_ITEM_RID,
			                      int? CHILD_ITEM_TYPE
			                      )
			    {
                    lock (typeof(MID_FOLDER_JOIN_INSERT_def))
                    {
                        this.PARENT_FOLDER_RID.SetValue(PARENT_FOLDER_RID);
                        this.CHILD_ITEM_RID.SetValue(CHILD_ITEM_RID);
                        this.CHILD_ITEM_TYPE.SetValue(CHILD_ITEM_TYPE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_FOLDER_JOIN_DELETE_def MID_FOLDER_JOIN_DELETE = new MID_FOLDER_JOIN_DELETE_def();
			public class MID_FOLDER_JOIN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_JOIN_DELETE.SQL"

			    private intParameter CHILD_ITEM_TYPE;
			    private intParameter CHILD_ITEM_RID;
			
			    public MID_FOLDER_JOIN_DELETE_def()
			    {
			        base.procedureName = "MID_FOLDER_JOIN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FOLDER_JOIN");
			        CHILD_ITEM_TYPE = new intParameter("@CHILD_ITEM_TYPE", base.inputParameterList);
			        CHILD_ITEM_RID = new intParameter("@CHILD_ITEM_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? CHILD_ITEM_TYPE,
			                      int? CHILD_ITEM_RID
			                      )
			    {
                    lock (typeof(MID_FOLDER_JOIN_DELETE_def))
                    {
                        this.CHILD_ITEM_TYPE.SetValue(CHILD_ITEM_TYPE);
                        this.CHILD_ITEM_RID.SetValue(CHILD_ITEM_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_FOLDER_JOIN_READ_def MID_FOLDER_JOIN_READ = new MID_FOLDER_JOIN_READ_def();
			public class MID_FOLDER_JOIN_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_JOIN_READ.SQL"

			    private intParameter PARENT_FOLDER_RID;
			    private intParameter CHILD_ITEM_RID;
			    private intParameter CHILD_ITEM_TYPE;
			
			    public MID_FOLDER_JOIN_READ_def()
			    {
			        base.procedureName = "MID_FOLDER_JOIN_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER_JOIN");
			        PARENT_FOLDER_RID = new intParameter("@PARENT_FOLDER_RID", base.inputParameterList);
			        CHILD_ITEM_RID = new intParameter("@CHILD_ITEM_RID", base.inputParameterList);
			        CHILD_ITEM_TYPE = new intParameter("@CHILD_ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? PARENT_FOLDER_RID,
			                          int? CHILD_ITEM_RID,
			                          int? CHILD_ITEM_TYPE
			                          )
			    {
                    lock (typeof(MID_FOLDER_JOIN_READ_def))
                    {
                        this.PARENT_FOLDER_RID.SetValue(PARENT_FOLDER_RID);
                        this.CHILD_ITEM_RID.SetValue(CHILD_ITEM_RID);
                        this.CHILD_ITEM_TYPE.SetValue(CHILD_ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_SHORTCUT_READ_def MID_FOLDER_SHORTCUT_READ = new MID_FOLDER_SHORTCUT_READ_def();
			public class MID_FOLDER_SHORTCUT_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_SHORTCUT_READ.SQL"

			    private intParameter CHILD_SHORTCUT_TYPE;
			    private intParameter CHILD_SHORTCUT_RID;
			    private intParameter PARENT_FOLDER_RID;
			
			    public MID_FOLDER_SHORTCUT_READ_def()
			    {
			        base.procedureName = "MID_FOLDER_SHORTCUT_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER_SHORTCUT");
			        CHILD_SHORTCUT_TYPE = new intParameter("@CHILD_SHORTCUT_TYPE", base.inputParameterList);
			        CHILD_SHORTCUT_RID = new intParameter("@CHILD_SHORTCUT_RID", base.inputParameterList);
			        PARENT_FOLDER_RID = new intParameter("@PARENT_FOLDER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? CHILD_SHORTCUT_TYPE,
			                          int? CHILD_SHORTCUT_RID,
			                          int? PARENT_FOLDER_RID
			                          )
			    {
                    lock (typeof(MID_FOLDER_SHORTCUT_READ_def))
                    {
                        this.CHILD_SHORTCUT_TYPE.SetValue(CHILD_SHORTCUT_TYPE);
                        this.CHILD_SHORTCUT_RID.SetValue(CHILD_SHORTCUT_RID);
                        this.PARENT_FOLDER_RID.SetValue(PARENT_FOLDER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_SHORTCUT_READ_FOR_USERS_def MID_FOLDER_SHORTCUT_READ_FOR_USERS = new MID_FOLDER_SHORTCUT_READ_FOR_USERS_def();
			public class MID_FOLDER_SHORTCUT_READ_FOR_USERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_SHORTCUT_READ_FOR_USERS.SQL"

			    private tableParameter USER_RID_LIST;
			    private tableParameter FOLDER_TYPE_LIST;
			
			    public MID_FOLDER_SHORTCUT_READ_FOR_USERS_def()
			    {
			        base.procedureName = "MID_FOLDER_SHORTCUT_READ_FOR_USERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER_SHORTCUT");
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
                    FOLDER_TYPE_LIST = new tableParameter("@FOLDER_TYPE_LIST", "FOLDER_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          DataTable USER_RID_LIST,
			                          DataTable FOLDER_TYPE_LIST
			                          )
			    {
                    lock (typeof(MID_FOLDER_SHORTCUT_READ_FOR_USERS_def))
                    {
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        this.FOLDER_TYPE_LIST.SetValue(FOLDER_TYPE_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_SHORTCUT_READ_FOR_TYPES_def MID_FOLDER_SHORTCUT_READ_FOR_TYPES = new MID_FOLDER_SHORTCUT_READ_FOR_TYPES_def();
			public class MID_FOLDER_SHORTCUT_READ_FOR_TYPES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_SHORTCUT_READ_FOR_TYPES.SQL"

			    private tableParameter FOLDER_TYPE_LIST;
			
			    public MID_FOLDER_SHORTCUT_READ_FOR_TYPES_def()
			    {
			        base.procedureName = "MID_FOLDER_SHORTCUT_READ_FOR_TYPES";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER_SHORTCUT");
			        FOLDER_TYPE_LIST = new tableParameter("@FOLDER_TYPE_LIST", "FOLDER_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, DataTable FOLDER_TYPE_LIST)
			    {
                    lock (typeof(MID_FOLDER_SHORTCUT_READ_FOR_TYPES_def))
                    {
                        this.FOLDER_TYPE_LIST.SetValue(FOLDER_TYPE_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_SHORTCUT_READ_FOLDER_FOR_USERS_def MID_FOLDER_SHORTCUT_READ_FOLDER_FOR_USERS = new MID_FOLDER_SHORTCUT_READ_FOLDER_FOR_USERS_def();
			public class MID_FOLDER_SHORTCUT_READ_FOLDER_FOR_USERS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_SHORTCUT_READ_FOLDER_FOR_USERS.SQL"

			    private tableParameter USER_RID_LIST;
			    private intParameter CHILD_SHORTCUT_TYPE;
			
			    public MID_FOLDER_SHORTCUT_READ_FOLDER_FOR_USERS_def()
			    {
			        base.procedureName = "MID_FOLDER_SHORTCUT_READ_FOLDER_FOR_USERS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER_SHORTCUT");
			        USER_RID_LIST = new tableParameter("@USER_RID_LIST", "USER_RID_TYPE", base.inputParameterList);
			        CHILD_SHORTCUT_TYPE = new intParameter("@CHILD_SHORTCUT_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          DataTable USER_RID_LIST,
			                          int? CHILD_SHORTCUT_TYPE
			                          )
			    {
                    lock (typeof(MID_FOLDER_SHORTCUT_READ_FOLDER_FOR_USERS_def))
                    {
                        this.USER_RID_LIST.SetValue(USER_RID_LIST);
                        this.CHILD_SHORTCUT_TYPE.SetValue(CHILD_SHORTCUT_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_SHORTCUT_READ_FOR_SHORTCUT_TYPE_def MID_FOLDER_SHORTCUT_READ_FOR_SHORTCUT_TYPE = new MID_FOLDER_SHORTCUT_READ_FOR_SHORTCUT_TYPE_def();
			public class MID_FOLDER_SHORTCUT_READ_FOR_SHORTCUT_TYPE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_SHORTCUT_READ_FOR_SHORTCUT_TYPE.SQL"

			    private intParameter CHILD_SHORTCUT_TYPE;
			
			    public MID_FOLDER_SHORTCUT_READ_FOR_SHORTCUT_TYPE_def()
			    {
			        base.procedureName = "MID_FOLDER_SHORTCUT_READ_FOR_SHORTCUT_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FOLDER_SHORTCUT");
			        CHILD_SHORTCUT_TYPE = new intParameter("@CHILD_SHORTCUT_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? CHILD_SHORTCUT_TYPE)
			    {
                    lock (typeof(MID_FOLDER_SHORTCUT_READ_FOR_SHORTCUT_TYPE_def))
                    {
                        this.CHILD_SHORTCUT_TYPE.SetValue(CHILD_SHORTCUT_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FOLDER_SHORTCUT_INSERT_def MID_FOLDER_SHORTCUT_INSERT = new MID_FOLDER_SHORTCUT_INSERT_def();
			public class MID_FOLDER_SHORTCUT_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_SHORTCUT_INSERT.SQL"

			    private intParameter PARENT_FOLDER_RID;
			    private intParameter CHILD_SHORTCUT_RID;
			    private intParameter CHILD_SHORTCUT_TYPE;
			
			    public MID_FOLDER_SHORTCUT_INSERT_def()
			    {
			        base.procedureName = "MID_FOLDER_SHORTCUT_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("FOLDER_SHORTCUT");
			        PARENT_FOLDER_RID = new intParameter("@PARENT_FOLDER_RID", base.inputParameterList);
			        CHILD_SHORTCUT_RID = new intParameter("@CHILD_SHORTCUT_RID", base.inputParameterList);
			        CHILD_SHORTCUT_TYPE = new intParameter("@CHILD_SHORTCUT_TYPE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? PARENT_FOLDER_RID,
			                      int? CHILD_SHORTCUT_RID,
			                      int? CHILD_SHORTCUT_TYPE
			                      )
			    {
                    lock (typeof(MID_FOLDER_SHORTCUT_INSERT_def))
                    {
                        this.PARENT_FOLDER_RID.SetValue(PARENT_FOLDER_RID);
                        this.CHILD_SHORTCUT_RID.SetValue(CHILD_SHORTCUT_RID);
                        this.CHILD_SHORTCUT_TYPE.SetValue(CHILD_SHORTCUT_TYPE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_FOLDER_SHORTCUT_DELETE_def MID_FOLDER_SHORTCUT_DELETE = new MID_FOLDER_SHORTCUT_DELETE_def();
			public class MID_FOLDER_SHORTCUT_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_SHORTCUT_DELETE.SQL"

			    private intParameter PARENT_FOLDER_RID;
			    private intParameter CHILD_SHORTCUT_RID;
			    private intParameter CHILD_SHORTCUT_TYPE;
			
			    public MID_FOLDER_SHORTCUT_DELETE_def()
			    {
			        base.procedureName = "MID_FOLDER_SHORTCUT_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FOLDER_SHORTCUT");
			        PARENT_FOLDER_RID = new intParameter("@PARENT_FOLDER_RID", base.inputParameterList);
			        CHILD_SHORTCUT_RID = new intParameter("@CHILD_SHORTCUT_RID", base.inputParameterList);
			        CHILD_SHORTCUT_TYPE = new intParameter("@CHILD_SHORTCUT_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? PARENT_FOLDER_RID,
			                      int? CHILD_SHORTCUT_RID,
			                      int? CHILD_SHORTCUT_TYPE
			                      )
			    {
                    lock (typeof(MID_FOLDER_SHORTCUT_DELETE_def))
                    {
                        this.PARENT_FOLDER_RID.SetValue(PARENT_FOLDER_RID);
                        this.CHILD_SHORTCUT_RID.SetValue(CHILD_SHORTCUT_RID);
                        this.CHILD_SHORTCUT_TYPE.SetValue(CHILD_SHORTCUT_TYPE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_FOLDER_SHORTCUT_DELETE_ALL_def MID_FOLDER_SHORTCUT_DELETE_ALL = new MID_FOLDER_SHORTCUT_DELETE_ALL_def();
			public class MID_FOLDER_SHORTCUT_DELETE_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FOLDER_SHORTCUT_DELETE_ALL.SQL"

			    private intParameter CHILD_SHORTCUT_RID;
			    private intParameter CHILD_SHORTCUT_TYPE;
			
			    public MID_FOLDER_SHORTCUT_DELETE_ALL_def()
			    {
			        base.procedureName = "MID_FOLDER_SHORTCUT_DELETE_ALL";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FOLDER_SHORTCUT");
			        CHILD_SHORTCUT_RID = new intParameter("@CHILD_SHORTCUT_RID", base.inputParameterList);
			        CHILD_SHORTCUT_TYPE = new intParameter("@CHILD_SHORTCUT_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? CHILD_SHORTCUT_RID,
			                      int? CHILD_SHORTCUT_TYPE
			                      )
			    {
                    lock (typeof(MID_FOLDER_SHORTCUT_DELETE_ALL_def))
                    {
                        this.CHILD_SHORTCUT_RID.SetValue(CHILD_SHORTCUT_RID);
                        this.CHILD_SHORTCUT_TYPE.SetValue(CHILD_SHORTCUT_TYPE);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
