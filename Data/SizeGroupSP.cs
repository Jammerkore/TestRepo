using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class SizeGroup : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_SIZE_GROUP_READ_FROM_NAME_def MID_SIZE_GROUP_READ_FROM_NAME = new MID_SIZE_GROUP_READ_FROM_NAME_def();
			public class MID_SIZE_GROUP_READ_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_GROUP_READ_FROM_NAME.SQL"

			    private stringParameter SIZE_GROUP_NAME;
			
			    public MID_SIZE_GROUP_READ_FROM_NAME_def()
			    {
			        base.procedureName = "MID_SIZE_GROUP_READ_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_GROUP");
			        SIZE_GROUP_NAME = new stringParameter("@SIZE_GROUP_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_GROUP_NAME)
			    {
                    lock (typeof(MID_SIZE_GROUP_READ_FROM_NAME_def))
                    {
                        this.SIZE_GROUP_NAME.SetValue(SIZE_GROUP_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_GROUP_READ_FROM_NAME_UPPER_def MID_SIZE_GROUP_READ_FROM_NAME_UPPER = new MID_SIZE_GROUP_READ_FROM_NAME_UPPER_def();
			public class MID_SIZE_GROUP_READ_FROM_NAME_UPPER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_GROUP_READ_FROM_NAME_UPPER.SQL"

                private stringParameter SIZE_GROUP_NAME;
			
			    public MID_SIZE_GROUP_READ_FROM_NAME_UPPER_def()
			    {
			        base.procedureName = "MID_SIZE_GROUP_READ_FROM_NAME_UPPER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_GROUP");
			        SIZE_GROUP_NAME = new stringParameter("@SIZE_GROUP_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_GROUP_NAME)
			    {
                    lock (typeof(MID_SIZE_GROUP_READ_FROM_NAME_UPPER_def))
                    {
                        this.SIZE_GROUP_NAME.SetValue(SIZE_GROUP_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_GROUP_UPDATE_def MID_SIZE_GROUP_UPDATE = new MID_SIZE_GROUP_UPDATE_def();
			public class MID_SIZE_GROUP_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_GROUP_UPDATE.SQL"

                private intParameter SIZE_GROUP_RID;
                private stringParameter SIZE_GROUP_NAME;
                private stringParameter SIZE_GROUP_DESCRIPTION;
                private charParameter WIDTH_ACROSS_IND;
			
			    public MID_SIZE_GROUP_UPDATE_def()
			    {
			        base.procedureName = "MID_SIZE_GROUP_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("SIZE_GROUP");
			        SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
			        SIZE_GROUP_NAME = new stringParameter("@SIZE_GROUP_NAME", base.inputParameterList);
			        SIZE_GROUP_DESCRIPTION = new stringParameter("@SIZE_GROUP_DESCRIPTION", base.inputParameterList);
			        WIDTH_ACROSS_IND = new charParameter("@WIDTH_ACROSS_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? SIZE_GROUP_RID,
			                      string SIZE_GROUP_NAME,
			                      string SIZE_GROUP_DESCRIPTION,
			                      char? WIDTH_ACROSS_IND
			                      )
			    {
                    lock (typeof(MID_SIZE_GROUP_UPDATE_def))
                    {
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        this.SIZE_GROUP_NAME.SetValue(SIZE_GROUP_NAME);
                        this.SIZE_GROUP_DESCRIPTION.SetValue(SIZE_GROUP_DESCRIPTION);
                        this.WIDTH_ACROSS_IND.SetValue(WIDTH_ACROSS_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_SIZE_GROUP_READ_NAME_def MID_SIZE_GROUP_READ_NAME = new MID_SIZE_GROUP_READ_NAME_def();
			public class MID_SIZE_GROUP_READ_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_GROUP_READ_NAME.SQL"

                private intParameter SIZE_GROUP_RID;
			
			    public MID_SIZE_GROUP_READ_NAME_def()
			    {
			        base.procedureName = "MID_SIZE_GROUP_READ_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_GROUP");
			        SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SIZE_GROUP_RID)
			    {
                    lock (typeof(MID_SIZE_GROUP_READ_NAME_def))
                    {
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_GROUP_JOIN_INSERT_def MID_SIZE_GROUP_JOIN_INSERT = new MID_SIZE_GROUP_JOIN_INSERT_def();
			public class MID_SIZE_GROUP_JOIN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_GROUP_JOIN_INSERT.SQL"

                private intParameter SIZE_GROUP_RID;
                private intParameter SIZE_CODE_RID;
                private intParameter SEQ;
			
			    public MID_SIZE_GROUP_JOIN_INSERT_def()
			    {
			        base.procedureName = "MID_SIZE_GROUP_JOIN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SIZE_GROUP_JOIN");
			        SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
			        SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
			        SEQ = new intParameter("@SEQ", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? SIZE_GROUP_RID,
			                      int? SIZE_CODE_RID,
			                      int? SEQ
			                      )
			    {
                    lock (typeof(MID_SIZE_GROUP_JOIN_INSERT_def))
                    {
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.SEQ.SetValue(SEQ);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SIZE_GROUP_JOIN_DELETE_def MID_SIZE_GROUP_JOIN_DELETE = new MID_SIZE_GROUP_JOIN_DELETE_def();
			public class MID_SIZE_GROUP_JOIN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_GROUP_JOIN_DELETE.SQL"

                private intParameter SIZE_GROUP_RID;
			
			    public MID_SIZE_GROUP_JOIN_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_GROUP_JOIN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_GROUP_JOIN");
			        SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_GROUP_RID)
			    {
                    lock (typeof(MID_SIZE_GROUP_JOIN_DELETE_def))
                    {
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SIZE_GROUP_READ_FROM_GROUP_NAME_def MID_SIZE_GROUP_READ_FROM_GROUP_NAME = new MID_SIZE_GROUP_READ_FROM_GROUP_NAME_def();
			public class MID_SIZE_GROUP_READ_FROM_GROUP_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_GROUP_READ_FROM_GROUP_NAME.SQL"

                private stringParameter SIZE_GROUP_NAME;
			
			    public MID_SIZE_GROUP_READ_FROM_GROUP_NAME_def()
			    {
			        base.procedureName = "MID_SIZE_GROUP_READ_FROM_GROUP_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_GROUP");
			        SIZE_GROUP_NAME = new stringParameter("@SIZE_GROUP_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_GROUP_NAME)
			    {
                    lock (typeof(MID_SIZE_GROUP_READ_FROM_GROUP_NAME_def))
                    {
                        this.SIZE_GROUP_NAME.SetValue(SIZE_GROUP_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_GROUP_READ_def MID_SIZE_GROUP_READ = new MID_SIZE_GROUP_READ_def();
			public class MID_SIZE_GROUP_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_GROUP_READ.SQL"

                private intParameter SIZE_GROUP_RID;
			
			    public MID_SIZE_GROUP_READ_def()
			    {
			        base.procedureName = "MID_SIZE_GROUP_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_GROUP");
			        SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SIZE_GROUP_RID)
			    {
                    lock (typeof(MID_SIZE_GROUP_READ_def))
                    {
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_GROUP_READ_ALL_DEFINED_def MID_SIZE_GROUP_READ_ALL_DEFINED = new MID_SIZE_GROUP_READ_ALL_DEFINED_def();
			public class MID_SIZE_GROUP_READ_ALL_DEFINED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_GROUP_READ_ALL_DEFINED.SQL"

			
			    public MID_SIZE_GROUP_READ_ALL_DEFINED_def()
			    {
			        base.procedureName = "MID_SIZE_GROUP_READ_ALL_DEFINED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_GROUP");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SIZE_GROUP_READ_ALL_DEFINED_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_GROUP_READ_ALL_def MID_SIZE_GROUP_READ_ALL = new MID_SIZE_GROUP_READ_ALL_def();
			public class MID_SIZE_GROUP_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_GROUP_READ_ALL.SQL"

			
			    public MID_SIZE_GROUP_READ_ALL_def()
			    {
			        base.procedureName = "MID_SIZE_GROUP_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_GROUP");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SIZE_GROUP_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CODE_READ_def MID_SIZE_CODE_READ = new MID_SIZE_CODE_READ_def();
			public class MID_SIZE_CODE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_READ.SQL"

                private intParameter SIZE_CODE_RID;
			
			    public MID_SIZE_CODE_READ_def()
			    {
			        base.procedureName = "MID_SIZE_CODE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CODE");
			        SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SIZE_CODE_RID)
			    {
                    lock (typeof(MID_SIZE_CODE_READ_def))
                    {
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CODE_READ_RID_def MID_SIZE_CODE_READ_RID = new MID_SIZE_CODE_READ_RID_def();
			public class MID_SIZE_CODE_READ_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_READ_RID.SQL"

                private stringParameter SIZE_CODE_ID;
			
			    public MID_SIZE_CODE_READ_RID_def()
			    {
			        base.procedureName = "MID_SIZE_CODE_READ_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CODE");
			        SIZE_CODE_ID = new stringParameter("@SIZE_CODE_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_CODE_ID)
			    {
                    lock (typeof(MID_SIZE_CODE_READ_RID_def))
                    {
                        this.SIZE_CODE_ID.SetValue(SIZE_CODE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CODE_READ_PRIMARY_AND_SECONDARY_def MID_SIZE_CODE_READ_PRIMARY_AND_SECONDARY = new MID_SIZE_CODE_READ_PRIMARY_AND_SECONDARY_def();
			public class MID_SIZE_CODE_READ_PRIMARY_AND_SECONDARY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_READ_PRIMARY_AND_SECONDARY.SQL"

                private stringParameter SIZE_CODE_PRIMARY;
                private stringParameter SIZE_CODE_SECONDARY;
			
			    public MID_SIZE_CODE_READ_PRIMARY_AND_SECONDARY_def()
			    {
			        base.procedureName = "MID_SIZE_CODE_READ_PRIMARY_AND_SECONDARY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CODE");
			        SIZE_CODE_PRIMARY = new stringParameter("@SIZE_CODE_PRIMARY", base.inputParameterList);
			        SIZE_CODE_SECONDARY = new stringParameter("@SIZE_CODE_SECONDARY", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          string SIZE_CODE_PRIMARY,
			                          string SIZE_CODE_SECONDARY
			                          )
			    {
                    lock (typeof(MID_SIZE_CODE_READ_PRIMARY_AND_SECONDARY_def))
                    {
                        this.SIZE_CODE_PRIMARY.SetValue(SIZE_CODE_PRIMARY);
                        this.SIZE_CODE_SECONDARY.SetValue(SIZE_CODE_SECONDARY);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


			public static MID_SIZE_CODE_READ_COUNT_def MID_SIZE_CODE_READ_COUNT = new MID_SIZE_CODE_READ_COUNT_def();
			public class MID_SIZE_CODE_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_READ_COUNT.SQL"

                private stringParameter SIZE_CODE_ID;
			
			    public MID_SIZE_CODE_READ_COUNT_def()
			    {
			        base.procedureName = "MID_SIZE_CODE_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("SIZE_CODE");
			        SIZE_CODE_ID = new stringParameter("@SIZE_CODE_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, string SIZE_CODE_ID)
			    {
                    lock (typeof(MID_SIZE_CODE_READ_COUNT_def))
                    {
                        this.SIZE_CODE_ID.SetValue(SIZE_CODE_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_SIZE_CODE_DELETE_def MID_SIZE_CODE_DELETE = new MID_SIZE_CODE_DELETE_def();
			public class MID_SIZE_CODE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_DELETE.SQL"

                private intParameter SIZE_CODE_RID;
			
			    public MID_SIZE_CODE_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_CODE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_CODE");
			        SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_CODE_RID)
			    {
                    lock (typeof(MID_SIZE_CODE_DELETE_def))
                    {
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}


			public static MID_DIMENSIONS_INSERT_def MID_DIMENSIONS_INSERT = new MID_DIMENSIONS_INSERT_def();
			public class MID_DIMENSIONS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DIMENSIONS_INSERT.SQL"

                private stringParameter SIZE_CODE_SECONDARY;
			
			    public MID_DIMENSIONS_INSERT_def()
			    {
			        base.procedureName = "MID_DIMENSIONS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("DIMENSIONS");
			        SIZE_CODE_SECONDARY = new stringParameter("@SIZE_CODE_SECONDARY", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, string SIZE_CODE_SECONDARY)
			    {
                    lock (typeof(MID_DIMENSIONS_INSERT_def))
                    {
                        this.SIZE_CODE_SECONDARY.SetValue(SIZE_CODE_SECONDARY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_DIMENSIONS_READ_COUNT_def MID_DIMENSIONS_READ_COUNT = new MID_DIMENSIONS_READ_COUNT_def();
			public class MID_DIMENSIONS_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DIMENSIONS_READ_COUNT.SQL"

                private stringParameter SIZE_CODE_SECONDARY;
			
			    public MID_DIMENSIONS_READ_COUNT_def()
			    {
			        base.procedureName = "MID_DIMENSIONS_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("DIMENSIONS");
			        SIZE_CODE_SECONDARY = new stringParameter("@SIZE_CODE_SECONDARY", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, string SIZE_CODE_SECONDARY)
			    {
                    lock (typeof(MID_DIMENSIONS_READ_COUNT_def))
                    {
                        this.SIZE_CODE_SECONDARY.SetValue(SIZE_CODE_SECONDARY);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_DIMENSIONS_READ_WITH_SIZE_CODE_SECONDARY_def MID_DIMENSIONS_READ_WITH_SIZE_CODE_SECONDARY = new MID_DIMENSIONS_READ_WITH_SIZE_CODE_SECONDARY_def();
			public class MID_DIMENSIONS_READ_WITH_SIZE_CODE_SECONDARY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DIMENSIONS_READ_WITH_SIZE_CODE_SECONDARY.SQL"

                private stringParameter SIZE_CODE_SECONDARY;
			
			    public MID_DIMENSIONS_READ_WITH_SIZE_CODE_SECONDARY_def()
			    {
			        base.procedureName = "MID_DIMENSIONS_READ_WITH_SIZE_CODE_SECONDARY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("DIMENSIONS");
			        SIZE_CODE_SECONDARY = new stringParameter("@SIZE_CODE_SECONDARY", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_CODE_SECONDARY)
			    {
                    lock (typeof(MID_DIMENSIONS_READ_WITH_SIZE_CODE_SECONDARY_def))
                    {
                        this.SIZE_CODE_SECONDARY.SetValue(SIZE_CODE_SECONDARY);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


			public static MID_DIMENSIONS_READ_ALL_def MID_DIMENSIONS_READ_ALL = new MID_DIMENSIONS_READ_ALL_def();
			public class MID_DIMENSIONS_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DIMENSIONS_READ_ALL.SQL"

			
			    public MID_DIMENSIONS_READ_ALL_def()
			    {
			        base.procedureName = "MID_DIMENSIONS_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("DIMENSIONS");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_DIMENSIONS_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZES_READ_COUNT_def MID_SIZES_READ_COUNT = new MID_SIZES_READ_COUNT_def();
			public class MID_SIZES_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZES_READ_COUNT.SQL"

                private stringParameter SIZE_CODE_PRIMARY;
			
			    public MID_SIZES_READ_COUNT_def()
			    {
			        base.procedureName = "MID_SIZES_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("SIZES");
			        SIZE_CODE_PRIMARY = new stringParameter("@SIZE_CODE_PRIMARY", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, string SIZE_CODE_PRIMARY)
			    {
                    lock (typeof(MID_SIZES_READ_COUNT_def))
                    {
                        this.SIZE_CODE_PRIMARY.SetValue(SIZE_CODE_PRIMARY);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_SIZES_INSERT_def MID_SIZES_INSERT = new MID_SIZES_INSERT_def();
			public class MID_SIZES_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZES_INSERT.SQL"

                private stringParameter SIZE_CODE_PRIMARY;
			
			    public MID_SIZES_INSERT_def()
			    {
			        base.procedureName = "MID_SIZES_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SIZES");
			        SIZE_CODE_PRIMARY = new stringParameter("@SIZE_CODE_PRIMARY", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, string SIZE_CODE_PRIMARY)
			    {
                    lock (typeof(MID_SIZES_INSERT_def))
                    {
                        this.SIZE_CODE_PRIMARY.SetValue(SIZE_CODE_PRIMARY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SIZES_READ_SIZE_CODE_PRIMARY_def MID_SIZES_READ_SIZE_CODE_PRIMARY = new MID_SIZES_READ_SIZE_CODE_PRIMARY_def();
			public class MID_SIZES_READ_SIZE_CODE_PRIMARY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZES_READ_SIZE_CODE_PRIMARY.SQL"

                private stringParameter SIZE_CODE_PRIMARY;
			
			    public MID_SIZES_READ_SIZE_CODE_PRIMARY_def()
			    {
			        base.procedureName = "MID_SIZES_READ_SIZE_CODE_PRIMARY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZES");
			        SIZE_CODE_PRIMARY = new stringParameter("@SIZE_CODE_PRIMARY", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_CODE_PRIMARY)
			    {
                    lock (typeof(MID_SIZES_READ_SIZE_CODE_PRIMARY_def))
                    {
                        this.SIZE_CODE_PRIMARY.SetValue(SIZE_CODE_PRIMARY);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


			public static MID_SIZES_READ_ALL_def MID_SIZES_READ_ALL = new MID_SIZES_READ_ALL_def();
			public class MID_SIZES_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZES_READ_ALL.SQL"

			
			    public MID_SIZES_READ_ALL_def()
			    {
			        base.procedureName = "MID_SIZES_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZES");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SIZES_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}



			public static MID_SIZE_CODE_UPDATE_def MID_SIZE_CODE_UPDATE = new MID_SIZE_CODE_UPDATE_def();
			public class MID_SIZE_CODE_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_UPDATE.SQL"

                private intParameter SIZE_CODE_RID;
                private stringParameter SIZE_CODE_ID;
                private stringParameter SIZE_CODE_PRIMARY;
                private stringParameter SIZE_CODE_SECONDARY;
                private stringParameter SIZE_CODE_PRODUCT_CATEGORY;
			
			    public MID_SIZE_CODE_UPDATE_def()
			    {
			        base.procedureName = "MID_SIZE_CODE_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("SIZE_CODE");
			        SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
			        SIZE_CODE_ID = new stringParameter("@SIZE_CODE_ID", base.inputParameterList);
			        SIZE_CODE_PRIMARY = new stringParameter("@SIZE_CODE_PRIMARY", base.inputParameterList);
			        SIZE_CODE_SECONDARY = new stringParameter("@SIZE_CODE_SECONDARY", base.inputParameterList);
			        SIZE_CODE_PRODUCT_CATEGORY = new stringParameter("@SIZE_CODE_PRODUCT_CATEGORY", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? SIZE_CODE_RID,
			                      string SIZE_CODE_ID,
			                      string SIZE_CODE_PRIMARY,
			                      string SIZE_CODE_SECONDARY,
			                      string SIZE_CODE_PRODUCT_CATEGORY
			                      )
			    {
                    lock (typeof(MID_SIZE_CODE_UPDATE_def))
                    {
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.SIZE_CODE_ID.SetValue(SIZE_CODE_ID);
                        this.SIZE_CODE_PRIMARY.SetValue(SIZE_CODE_PRIMARY);
                        this.SIZE_CODE_SECONDARY.SetValue(SIZE_CODE_SECONDARY);
                        this.SIZE_CODE_PRODUCT_CATEGORY.SetValue(SIZE_CODE_PRODUCT_CATEGORY);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_SIZE_INSERT_def SP_MID_SIZE_INSERT = new SP_MID_SIZE_INSERT_def();
            public class SP_MID_SIZE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_INSERT.SQL"

                private stringParameter SIZE_CODE_ID;
                private stringParameter SIZE_CODE_PRIMARY;
                private stringParameter SIZE_CODE_SECONDARY;
                private stringParameter SIZE_CODE_PRODUCT_CATEGORY;
                private intParameter SIZE_CODE_RID; //Declare Output Parameter
			
			    public SP_MID_SIZE_INSERT_def()
			    {
                    base.procedureName = "SP_MID_SIZE_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("SIZE_CODE");
			        SIZE_CODE_ID = new stringParameter("@SIZE_CODE_ID", base.inputParameterList);
			        SIZE_CODE_PRIMARY = new stringParameter("@SIZE_CODE_PRIMARY", base.inputParameterList);
			        SIZE_CODE_SECONDARY = new stringParameter("@SIZE_CODE_SECONDARY", base.inputParameterList);
			        SIZE_CODE_PRODUCT_CATEGORY = new stringParameter("@SIZE_CODE_PRODUCT_CATEGORY", base.inputParameterList);
			        SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string SIZE_CODE_ID,
			                                  string SIZE_CODE_PRIMARY,
			                                  string SIZE_CODE_SECONDARY,
			                                  string SIZE_CODE_PRODUCT_CATEGORY
			                                  )
			    {
                    lock (typeof(SP_MID_SIZE_INSERT_def))
                    {
                        this.SIZE_CODE_ID.SetValue(SIZE_CODE_ID);
                        this.SIZE_CODE_PRIMARY.SetValue(SIZE_CODE_PRIMARY);
                        this.SIZE_CODE_SECONDARY.SetValue(SIZE_CODE_SECONDARY);
                        this.SIZE_CODE_PRODUCT_CATEGORY.SetValue(SIZE_CODE_PRODUCT_CATEGORY);
                        this.SIZE_CODE_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_SIZE_CODE_READ_ALL_def MID_SIZE_CODE_READ_ALL = new MID_SIZE_CODE_READ_ALL_def();
			public class MID_SIZE_CODE_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_READ_ALL.SQL"

			
			    public MID_SIZE_CODE_READ_ALL_def()
			    {
			        base.procedureName = "MID_SIZE_CODE_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CODE");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SIZE_CODE_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CODE_READ_ALL_PRODUCT_CATEGORY_def MID_SIZE_CODE_READ_ALL_PRODUCT_CATEGORY = new MID_SIZE_CODE_READ_ALL_PRODUCT_CATEGORY_def();
			public class MID_SIZE_CODE_READ_ALL_PRODUCT_CATEGORY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CODE_READ_ALL_PRODUCT_CATEGORY.SQL"

			
			    public MID_SIZE_CODE_READ_ALL_PRODUCT_CATEGORY_def()
			    {
			        base.procedureName = "MID_SIZE_CODE_READ_ALL_PRODUCT_CATEGORY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CODE");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SIZE_CODE_READ_ALL_PRODUCT_CATEGORY_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_SIZE_GROUP_DELETE_def SP_MID_SIZE_GROUP_DELETE = new SP_MID_SIZE_GROUP_DELETE_def();
            public class SP_MID_SIZE_GROUP_DELETE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_GROUP_DELETE.SQL"

                private intParameter SIZE_GROUP_RID;
			
			    public SP_MID_SIZE_GROUP_DELETE_def()
			    {
                    base.procedureName = "SP_MID_SIZE_GROUP_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_GROUP");
			        SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_GROUP_RID)
			    {
                    lock (typeof(SP_MID_SIZE_GROUP_DELETE_def))
                    {
                        this.SIZE_GROUP_RID.SetValue(SIZE_GROUP_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_SIZE_GROUP_INSERT_def SP_MID_SIZE_GROUP_INSERT = new SP_MID_SIZE_GROUP_INSERT_def();
            public class SP_MID_SIZE_GROUP_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_GROUP_INSERT.SQL"

                private stringParameter SIZE_GROUP_NAME;
                private stringParameter SIZE_GROUP_DESCRIPTION;
                private charParameter WIDTH_ACROSS_IND;
                private intParameter SIZE_GROUP_RID; //Declare Output Parameter
			
			    public SP_MID_SIZE_GROUP_INSERT_def()
			    {
                    base.procedureName = "SP_MID_SIZE_GROUP_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("SIZE_GROUP");
			        SIZE_GROUP_NAME = new stringParameter("@SIZE_GROUP_NAME", base.inputParameterList);
			        SIZE_GROUP_DESCRIPTION = new stringParameter("@SIZE_GROUP_DESCRIPTION", base.inputParameterList);
			        WIDTH_ACROSS_IND = new charParameter("@WIDTH_ACROSS_IND", base.inputParameterList);
			        SIZE_GROUP_RID = new intParameter("@SIZE_GROUP_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string SIZE_GROUP_NAME,
			                                  string SIZE_GROUP_DESCRIPTION,
			                                  char? WIDTH_ACROSS_IND
			                                  )
			    {
                    lock (typeof(SP_MID_SIZE_GROUP_INSERT_def))
                    {
                        this.SIZE_GROUP_NAME.SetValue(SIZE_GROUP_NAME);
                        this.SIZE_GROUP_DESCRIPTION.SetValue(SIZE_GROUP_DESCRIPTION);
                        this.WIDTH_ACROSS_IND.SetValue(WIDTH_ACROSS_IND);
                        this.SIZE_GROUP_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
