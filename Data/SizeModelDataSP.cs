using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class SizeModelData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_SIZE_CONSTRAINT_MODEL_READ_ALL_def MID_SIZE_CONSTRAINT_MODEL_READ_ALL = new MID_SIZE_CONSTRAINT_MODEL_READ_ALL_def();
			public class MID_SIZE_CONSTRAINT_MODEL_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_MODEL_READ_ALL.SQL"

			
			    public MID_SIZE_CONSTRAINT_MODEL_READ_ALL_def()
			    {
			        base.procedureName = "MID_SIZE_CONSTRAINT_MODEL_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CONSTRAINT_MODEL");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SIZE_CONSTRAINT_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_SIZE_CONSTRAINT_MODELS_GET_def MID_SIZE_CONSTRAINT_MODELS_GET = new MID_SIZE_CONSTRAINT_MODELS_GET_def();
            public class MID_SIZE_CONSTRAINT_MODELS_GET_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_MODELS_GET.SQL"

                private intParameter SIZE_CONSTRAINT_RID;

                public MID_SIZE_CONSTRAINT_MODELS_GET_def()
                {
                    base.procedureName = "MID_SIZE_CONSTRAINT_MODELS_GET";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("SIZE_CONSTRAINT_MODEL");
                    SIZE_CONSTRAINT_RID = new intParameter("@SIZE_CONSTRAINT_RID", base.inputParameterList);
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, int? SIZE_CONSTRAINT_RID)
                {
                    lock (typeof(MID_SIZE_CONSTRAINT_MODELS_GET_def))
                    {
                        this.SIZE_CONSTRAINT_RID.SetValue(SIZE_CONSTRAINT_RID);
                        return ExecuteStoredProcedureForReadAsDataSet(_dba);
                    }
                }
            }

			public static MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_def MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME = new MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_def();
			public class MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME.SQL"

                private stringParameter SIZE_CONSTRAINT_NAME;
			
			    public MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_def()
			    {
			        base.procedureName = "MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CONSTRAINT_MODELS");
			        SIZE_CONSTRAINT_NAME = new stringParameter("@SIZE_CONSTRAINT_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_CONSTRAINT_NAME)
			    {
                    lock (typeof(MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_def))
                    {
                        this.SIZE_CONSTRAINT_NAME.SetValue(SIZE_CONSTRAINT_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_UPPER_def MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_UPPER = new MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_UPPER_def();
			public class MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_UPPER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_UPPER.SQL"

                private stringParameter SIZE_CONSTRAINT_NAME;
			
			    public MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_UPPER_def()
			    {
			        base.procedureName = "MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_UPPER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CONSTRAINT_MODELS");
			        SIZE_CONSTRAINT_NAME = new stringParameter("@SIZE_CONSTRAINT_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_CONSTRAINT_NAME)
			    {
                    lock (typeof(MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_UPPER_def))
                    {
                        this.SIZE_CONSTRAINT_NAME.SetValue(SIZE_CONSTRAINT_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_EXACT_def MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_EXACT = new MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_EXACT_def();
			public class MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_EXACT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_EXACT.SQL"

                private stringParameter SIZE_CONSTRAINT_NAME;
			
			    public MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_EXACT_def()
			    {
			        base.procedureName = "MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_EXACT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CONSTRAINT_MODELS");
			        SIZE_CONSTRAINT_NAME = new stringParameter("@SIZE_CONSTRAINT_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_CONSTRAINT_NAME)
			    {
                    lock (typeof(MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_EXACT_def))
                    {
                        this.SIZE_CONSTRAINT_NAME.SetValue(SIZE_CONSTRAINT_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_def MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME = new MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_def();
			public class MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME.SQL"

                private stringParameter SIZE_ALTERNATE_NAME;
			
			    public MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_def()
			    {
			        base.procedureName = "MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_ALTERNATE_MODEL");
			        SIZE_ALTERNATE_NAME = new stringParameter("@SIZE_ALTERNATE_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_ALTERNATE_NAME)
			    {
                    lock (typeof(MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_def))
                    {
                        this.SIZE_ALTERNATE_NAME.SetValue(SIZE_ALTERNATE_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_UPPER_def MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_UPPER = new MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_UPPER_def();
			public class MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_UPPER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_UPPER.SQL"

                private stringParameter SIZE_ALTERNATE_NAME;
			
			    public MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_UPPER_def()
			    {
			        base.procedureName = "MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_UPPER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_ALTERNATE_MODEL");
			        SIZE_ALTERNATE_NAME = new stringParameter("@SIZE_ALTERNATE_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_ALTERNATE_NAME)
			    {
                    lock (typeof(MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_UPPER_def))
                    {
                        this.SIZE_ALTERNATE_NAME.SetValue(SIZE_ALTERNATE_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CONSTRAINT_GRPLVL_DELETE_def MID_SIZE_CONSTRAINT_GRPLVL_DELETE = new MID_SIZE_CONSTRAINT_GRPLVL_DELETE_def();
			public class MID_SIZE_CONSTRAINT_GRPLVL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_GRPLVL_DELETE.SQL"

                private intParameter SIZE_CONSTRAINT_RID;
			
			    public MID_SIZE_CONSTRAINT_GRPLVL_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_CONSTRAINT_GRPLVL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_CONSTRAINT_GRPLVL");
			        SIZE_CONSTRAINT_RID = new intParameter("@SIZE_CONSTRAINT_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_CONSTRAINT_RID)
			    {
                    lock (typeof(MID_SIZE_CONSTRAINT_GRPLVL_DELETE_def))
                    {
                        this.SIZE_CONSTRAINT_RID.SetValue(SIZE_CONSTRAINT_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SIZE_CONSTRAINT_MINMAX_DELETE_def MID_SIZE_CONSTRAINT_MINMAX_DELETE = new MID_SIZE_CONSTRAINT_MINMAX_DELETE_def();
			public class MID_SIZE_CONSTRAINT_MINMAX_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_MINMAX_DELETE.SQL"

                private intParameter SIZE_CONSTRAINT_RID;
			
			    public MID_SIZE_CONSTRAINT_MINMAX_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_CONSTRAINT_MINMAX_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_CONSTRAINT_MINMAX");
			        SIZE_CONSTRAINT_RID = new intParameter("@SIZE_CONSTRAINT_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_CONSTRAINT_RID)
			    {
                    lock (typeof(MID_SIZE_CONSTRAINT_MINMAX_DELETE_def))
                    {
                        this.SIZE_CONSTRAINT_RID.SetValue(SIZE_CONSTRAINT_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SIZE_CONSTRAINT_MODEL_DELETE_def MID_SIZE_CONSTRAINT_MODEL_DELETE = new MID_SIZE_CONSTRAINT_MODEL_DELETE_def();
			public class MID_SIZE_CONSTRAINT_MODEL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CONSTRAINT_MODEL_DELETE.SQL"

                private intParameter SIZE_CONSTRAINT_RID;
			
			    public MID_SIZE_CONSTRAINT_MODEL_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_CONSTRAINT_MODEL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_CONSTRAINT_MODEL");
			        SIZE_CONSTRAINT_RID = new intParameter("@SIZE_CONSTRAINT_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_CONSTRAINT_RID)
			    {
                    lock (typeof(MID_SIZE_CONSTRAINT_MODEL_DELETE_def))
                    {
                        this.SIZE_CONSTRAINT_RID.SetValue(SIZE_CONSTRAINT_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALTERNATE_MODEL_READ_ALL_def MID_SIZE_ALTERNATE_MODEL_READ_ALL = new MID_SIZE_ALTERNATE_MODEL_READ_ALL_def();
			public class MID_SIZE_ALTERNATE_MODEL_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALTERNATE_MODEL_READ_ALL.SQL"

			
			    public MID_SIZE_ALTERNATE_MODEL_READ_ALL_def()
			    {
			        base.procedureName = "MID_SIZE_ALTERNATE_MODEL_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_ALTERNATE_MODEL");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SIZE_ALTERNATE_MODEL_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALTERNATE_MODEL_READ_def MID_SIZE_ALTERNATE_MODEL_READ = new MID_SIZE_ALTERNATE_MODEL_READ_def();
			public class MID_SIZE_ALTERNATE_MODEL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALTERNATE_MODEL_READ.SQL"

                private intParameter SIZE_ALTERNATE_RID;
			
			    public MID_SIZE_ALTERNATE_MODEL_READ_def()
			    {
			        base.procedureName = "MID_SIZE_ALTERNATE_MODEL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_ALTERNATE_MODEL");
			        SIZE_ALTERNATE_RID = new intParameter("@SIZE_ALTERNATE_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SIZE_ALTERNATE_RID)
			    {
                    lock (typeof(MID_SIZE_ALTERNATE_MODEL_READ_def))
                    {
                        this.SIZE_ALTERNATE_RID.SetValue(SIZE_ALTERNATE_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALTERNATE_MODEL_READ_FROM_ALTERNATE_NAME_def MID_SIZE_ALTERNATE_MODEL_READ_FROM_ALTERNATE_NAME = new MID_SIZE_ALTERNATE_MODEL_READ_FROM_ALTERNATE_NAME_def();
			public class MID_SIZE_ALTERNATE_MODEL_READ_FROM_ALTERNATE_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALTERNATE_MODEL_READ_FROM_ALTERNATE_NAME.SQL"

                private stringParameter SIZE_ALTERNATE_NAME;
			
			    public MID_SIZE_ALTERNATE_MODEL_READ_FROM_ALTERNATE_NAME_def()
			    {
			        base.procedureName = "MID_SIZE_ALTERNATE_MODEL_READ_FROM_ALTERNATE_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_ALTERNATE_MODEL");
			        SIZE_ALTERNATE_NAME = new stringParameter("@SIZE_ALTERNATE_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_ALTERNATE_NAME)
			    {
                    lock (typeof(MID_SIZE_ALTERNATE_MODEL_READ_FROM_ALTERNATE_NAME_def))
                    {
                        this.SIZE_ALTERNATE_NAME.SetValue(SIZE_ALTERNATE_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALTERNATE_MODEL_DELETE_def MID_SIZE_ALTERNATE_MODEL_DELETE = new MID_SIZE_ALTERNATE_MODEL_DELETE_def();
			public class MID_SIZE_ALTERNATE_MODEL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALTERNATE_MODEL_DELETE.SQL"

                private intParameter SIZE_ALTERNATE_RID;
			
			    public MID_SIZE_ALTERNATE_MODEL_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_ALTERNATE_MODEL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_ALTERNATE_MODEL");
			        SIZE_ALTERNATE_RID = new intParameter("@SIZE_ALTERNATE_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_ALTERNATE_RID)
			    {
                    lock (typeof(MID_SIZE_ALTERNATE_MODEL_DELETE_def))
                    {
                        this.SIZE_ALTERNATE_RID.SetValue(SIZE_ALTERNATE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALT_PRIMARY_SIZE_READ_def MID_SIZE_ALT_PRIMARY_SIZE_READ = new MID_SIZE_ALT_PRIMARY_SIZE_READ_def();
			public class MID_SIZE_ALT_PRIMARY_SIZE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALT_PRIMARY_SIZE_READ.SQL"

                private intParameter SIZE_ALTERNATE_RID;
			
			    public MID_SIZE_ALT_PRIMARY_SIZE_READ_def()
			    {
			        base.procedureName = "MID_SIZE_ALT_PRIMARY_SIZE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_ALT_PRIMARY_SIZE");
			        SIZE_ALTERNATE_RID = new intParameter("@SIZE_ALTERNATE_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SIZE_ALTERNATE_RID)
			    {
                    lock (typeof(MID_SIZE_ALT_PRIMARY_SIZE_READ_def))
                    {
                        this.SIZE_ALTERNATE_RID.SetValue(SIZE_ALTERNATE_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALT_ALTERNATE_SIZE_READ_def MID_SIZE_ALT_ALTERNATE_SIZE_READ = new MID_SIZE_ALT_ALTERNATE_SIZE_READ_def();
			public class MID_SIZE_ALT_ALTERNATE_SIZE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALT_ALTERNATE_SIZE_READ.SQL"

                private intParameter SIZE_ALTERNATE_RID;
			
			    public MID_SIZE_ALT_ALTERNATE_SIZE_READ_def()
			    {
			        base.procedureName = "MID_SIZE_ALT_ALTERNATE_SIZE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_ALT_ALTERNATE_SIZE");
			        SIZE_ALTERNATE_RID = new intParameter("@SIZE_ALTERNATE_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SIZE_ALTERNATE_RID)
			    {
                    lock (typeof(MID_SIZE_ALT_ALTERNATE_SIZE_READ_def))
                    {
                        this.SIZE_ALTERNATE_RID.SetValue(SIZE_ALTERNATE_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALT_ALTERNATE_SIZE_DELETE_def MID_SIZE_ALT_ALTERNATE_SIZE_DELETE = new MID_SIZE_ALT_ALTERNATE_SIZE_DELETE_def();
			public class MID_SIZE_ALT_ALTERNATE_SIZE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALT_ALTERNATE_SIZE_DELETE.SQL"

                private intParameter SIZE_ALTERNATE_RID;
			
			    public MID_SIZE_ALT_ALTERNATE_SIZE_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_ALT_ALTERNATE_SIZE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_ALT_ALTERNATE_SIZE");
			        SIZE_ALTERNATE_RID = new intParameter("@SIZE_ALTERNATE_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_ALTERNATE_RID)
			    {
                    lock (typeof(MID_SIZE_ALT_ALTERNATE_SIZE_DELETE_def))
                    {
                        this.SIZE_ALTERNATE_RID.SetValue(SIZE_ALTERNATE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALT_PRIMARY_SIZE_DELETE_def MID_SIZE_ALT_PRIMARY_SIZE_DELETE = new MID_SIZE_ALT_PRIMARY_SIZE_DELETE_def();
			public class MID_SIZE_ALT_PRIMARY_SIZE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALT_PRIMARY_SIZE_DELETE.SQL"

                private intParameter SIZE_ALTERNATE_RID;
			
			    public MID_SIZE_ALT_PRIMARY_SIZE_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_ALT_PRIMARY_SIZE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_ALT_PRIMARY_SIZE");
			        SIZE_ALTERNATE_RID = new intParameter("@SIZE_ALTERNATE_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_ALTERNATE_RID)
			    {
                    lock (typeof(MID_SIZE_ALT_PRIMARY_SIZE_DELETE_def))
                    {
                        this.SIZE_ALTERNATE_RID.SetValue(SIZE_ALTERNATE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALT_PRIMARY_SIZE_INSERT_def MID_SIZE_ALT_PRIMARY_SIZE_INSERT = new MID_SIZE_ALT_PRIMARY_SIZE_INSERT_def();
			public class MID_SIZE_ALT_PRIMARY_SIZE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALT_PRIMARY_SIZE_INSERT.SQL"

                private intParameter SIZE_ALTERNATE_RID;
                private intParameter PRIMARY_SIZE_SEQ;
                private intParameter SIZES_RID;
                private intParameter DIMENSIONS_RID;
			
			    public MID_SIZE_ALT_PRIMARY_SIZE_INSERT_def()
			    {
			        base.procedureName = "MID_SIZE_ALT_PRIMARY_SIZE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SIZE_ALT_PRIMARY_SIZE");
			        SIZE_ALTERNATE_RID = new intParameter("@SIZE_ALTERNATE_RID", base.inputParameterList);
			        PRIMARY_SIZE_SEQ = new intParameter("@PRIMARY_SIZE_SEQ", base.inputParameterList);
			        SIZES_RID = new intParameter("@SIZES_RID", base.inputParameterList);
			        DIMENSIONS_RID = new intParameter("@DIMENSIONS_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? SIZE_ALTERNATE_RID,
			                      int? PRIMARY_SIZE_SEQ,
			                      int? SIZES_RID,
			                      int? DIMENSIONS_RID
			                      )
			    {
                    lock (typeof(MID_SIZE_ALT_PRIMARY_SIZE_INSERT_def))
                    {
                        this.SIZE_ALTERNATE_RID.SetValue(SIZE_ALTERNATE_RID);
                        this.PRIMARY_SIZE_SEQ.SetValue(PRIMARY_SIZE_SEQ);
                        this.SIZES_RID.SetValue(SIZES_RID);
                        this.DIMENSIONS_RID.SetValue(DIMENSIONS_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SIZE_ALT_ALTERNATE_SIZE_INSERT_def MID_SIZE_ALT_ALTERNATE_SIZE_INSERT = new MID_SIZE_ALT_ALTERNATE_SIZE_INSERT_def();
			public class MID_SIZE_ALT_ALTERNATE_SIZE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_ALT_ALTERNATE_SIZE_INSERT.SQL"

                private intParameter SIZE_ALTERNATE_RID;
                private intParameter PRIMARY_SIZE_SEQ;
                private intParameter ALTERNATE_SIZE_SEQ;
                private intParameter SIZES_RID;
                private intParameter DIMENSIONS_RID;
			
			    public MID_SIZE_ALT_ALTERNATE_SIZE_INSERT_def()
			    {
			        base.procedureName = "MID_SIZE_ALT_ALTERNATE_SIZE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SIZE_ALT_ALTERNATE_SIZE");
			        SIZE_ALTERNATE_RID = new intParameter("@SIZE_ALTERNATE_RID", base.inputParameterList);
			        PRIMARY_SIZE_SEQ = new intParameter("@PRIMARY_SIZE_SEQ", base.inputParameterList);
			        ALTERNATE_SIZE_SEQ = new intParameter("@ALTERNATE_SIZE_SEQ", base.inputParameterList);
			        SIZES_RID = new intParameter("@SIZES_RID", base.inputParameterList);
			        DIMENSIONS_RID = new intParameter("@DIMENSIONS_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? SIZE_ALTERNATE_RID,
			                      int? PRIMARY_SIZE_SEQ,
			                      int? ALTERNATE_SIZE_SEQ,
			                      int? SIZES_RID,
			                      int? DIMENSIONS_RID
			                      )
			    {
                    lock (typeof(MID_SIZE_ALT_ALTERNATE_SIZE_INSERT_def))
                    {
                        this.SIZE_ALTERNATE_RID.SetValue(SIZE_ALTERNATE_RID);
                        this.PRIMARY_SIZE_SEQ.SetValue(PRIMARY_SIZE_SEQ);
                        this.ALTERNATE_SIZE_SEQ.SetValue(ALTERNATE_SIZE_SEQ);
                        this.SIZES_RID.SetValue(SIZES_RID);
                        this.DIMENSIONS_RID.SetValue(DIMENSIONS_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_SZ_CONSTR_MODEL_UP_INS_def SP_MID_SZ_CONSTR_MODEL_UP_INS = new SP_MID_SZ_CONSTR_MODEL_UP_INS_def();
            public class SP_MID_SZ_CONSTR_MODEL_UP_INS_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SZ_CONSTR_MODEL_UP_INS.SQL"

                private intParameter RID;
                private stringParameter NAME;
                private intParameter SIZEGROUPRID;
                private intParameter SIZECURVEGROUPRID;
                private intParameter SGRID;
                private intParameter NEWRID; //Declare Output Parameter

                public SP_MID_SZ_CONSTR_MODEL_UP_INS_def()
                {
                    base.procedureName = "SP_MID_SZ_CONSTR_MODEL_UP_INS";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("SIZE_CONSTRAINT_MODEL");
                    RID = new intParameter("@RID", base.inputParameterList);
                    NAME = new stringParameter("@NAME", base.inputParameterList);
                    SIZEGROUPRID = new intParameter("@SIZEGROUPRID", base.inputParameterList);
                    SIZECURVEGROUPRID = new intParameter("@SIZECURVEGROUPRID", base.inputParameterList);
                    SGRID = new intParameter("@SGRID", base.inputParameterList);
                    NEWRID = new intParameter("@NEWRID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              int? RID,
                                              string NAME,
                                              int? SIZEGROUPRID,
                                              int? SIZECURVEGROUPRID,
                                              int? SGRID
                                              )
                {
                    lock (typeof(SP_MID_SZ_CONSTR_MODEL_UP_INS_def))
                    {
                        this.RID.SetValue(RID);
                        this.NAME.SetValue(NAME);
                        this.SIZEGROUPRID.SetValue(SIZEGROUPRID);
                        this.SIZECURVEGROUPRID.SetValue(SIZECURVEGROUPRID);
                        this.SGRID.SetValue(SGRID);
                        this.NEWRID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

            public static SP_MID_SIZE_CONST_GET_ALL_MINMAX_def SP_MID_SIZE_CONST_GET_ALL_MINMAX = new SP_MID_SIZE_CONST_GET_ALL_MINMAX_def();
            public class SP_MID_SIZE_CONST_GET_ALL_MINMAX_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_CONST_GET_ALL_MINMAX.SQL"

                private intParameter SIZECONSTRAINTRID;
                private intParameter SGRID;
                private intParameter SG_VERSION;  // TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception

                public SP_MID_SIZE_CONST_GET_ALL_MINMAX_def()
                {
                    base.procedureName = "SP_MID_SIZE_CONST_GET_ALL_MINMAX";
                    base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("SIZE_CONST_GET_ALL_MINMAX");
                    SIZECONSTRAINTRID = new intParameter("@SIZECONSTRAINTRID", base.inputParameterList);
                    SGRID = new intParameter("@SGRID", base.inputParameterList);
                    SG_VERSION = new intParameter("@SG_VERSION", base.inputParameterList);  // TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
                }

                public DataSet ReadAsDataSet(DatabaseAccess _dba,
                                             int? SIZECONSTRAINTRID,
                                             int? SGRID,
                                             int? SG_VERSION  // TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
                                             )
                {
                    lock (typeof(SP_MID_SIZE_CONST_GET_ALL_MINMAX_def))
                    {
                        this.SIZECONSTRAINTRID.SetValue(SIZECONSTRAINTRID);
                        this.SGRID.SetValue(SGRID);
                        this.SG_VERSION.SetValue(SG_VERSION);  // TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
                        DataSet dsMinMax = ExecuteStoredProcedureForReadAsDataSet(_dba);
                        dsMinMax.Tables[0].TableName = "SetLevel";
                        dsMinMax.Tables[1].TableName = "AllColor";
                        dsMinMax.Tables[2].TableName = "AllColorSize";
                        dsMinMax.Tables[3].TableName = "ColorSize";
                        dsMinMax.Tables[4].TableName = "AllColorSizeDimension";
                        dsMinMax.Tables[5].TableName = "ColorSizeDimension";
                        dsMinMax.Tables[6].TableName = "Color";
                        return dsMinMax;
                    }
                }
            }

            public static SP_MID_SZ_ALT_MODEL_UP_INS_def SP_MID_SZ_ALT_MODEL_UP_INS = new SP_MID_SZ_ALT_MODEL_UP_INS_def();
            public class SP_MID_SZ_ALT_MODEL_UP_INS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SZ_ALT_MODEL_UP_INS.SQL"

			    private intParameter RID;
			    private stringParameter NAME;
			    private intParameter PRIMSIZECURVERID;
			    private intParameter ALTSIZECURVERID;
			    private intParameter NEWRID; //Declare Output Parameter
			
			    public SP_MID_SZ_ALT_MODEL_UP_INS_def()
			    {
                    base.procedureName = "SP_MID_SZ_ALT_MODEL_UP_INS";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("SZ_ALT_MODEL_UP");
			        RID = new intParameter("@RID", base.inputParameterList);
			        NAME = new stringParameter("@NAME", base.inputParameterList);
			        PRIMSIZECURVERID = new intParameter("@PRIMSIZECURVERID", base.inputParameterList);
			        ALTSIZECURVERID = new intParameter("@ALTSIZECURVERID", base.inputParameterList);
			        NEWRID = new intParameter("@NEWRID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  int? RID,
			                                  string NAME,
			                                  int? PRIMSIZECURVERID,
			                                  int? ALTSIZECURVERID
			                                  )
			    {
                    lock (typeof(SP_MID_SZ_ALT_MODEL_UP_INS_def))
                    {
                        this.RID.SetValue(RID);
                        this.NAME.SetValue(NAME);
                        this.PRIMSIZECURVERID.SetValue(PRIMSIZECURVERID);
                        this.ALTSIZECURVERID.SetValue(ALTSIZECURVERID);
                        this.NEWRID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

            public static SP_MID_SIZE_CONST_GET_GRPLVL_def SP_MID_SIZE_CONST_GET_GRPLVL = new SP_MID_SIZE_CONST_GET_GRPLVL_def();
            public class SP_MID_SIZE_CONST_GET_GRPLVL_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_CONST_GET_GRPLVL.SQL"

                private intParameter SIZECONSTRAINTRID;
                private intParameter SGRID;
                private intParameter SG_VERSION;  // TT#5810 - JSmith - Size Constraint API failing
			
			    public SP_MID_SIZE_CONST_GET_GRPLVL_def()
			    {
                    base.procedureName = "SP_MID_SIZE_CONST_GET_GRPLVL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CONST_GET_GRPLVL");
			        SIZECONSTRAINTRID = new intParameter("@SIZECONSTRAINTRID", base.inputParameterList);
			        SGRID = new intParameter("@SGRID", base.inputParameterList);
                    SG_VERSION = new intParameter("@SG_VERSION", base.inputParameterList);   // TT#5810 - JSmith - Size Constraint API failing
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SIZECONSTRAINTRID,
                                      int? SGRID,
                                      int? SG_VERSION  // TT#5810 - JSmith - Size Constraint API failing
			                          )
			    {
                    lock (typeof(SP_MID_SIZE_CONST_GET_GRPLVL_def))
                    {
                        this.SIZECONSTRAINTRID.SetValue(SIZECONSTRAINTRID);
                        this.SGRID.SetValue(SGRID);
                        this.SG_VERSION.SetValue(SG_VERSION);  // TT#5810 - JSmith - Size Constraint API failing
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_SIZE_CONST_MM_INSERT_def SP_MID_SIZE_CONST_MM_INSERT = new SP_MID_SIZE_CONST_MM_INSERT_def();
            public class SP_MID_SIZE_CONST_MM_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_CONST_MM_INSERT.SQL"

                private intParameter SIZECONSTRAINTRID;
                private intParameter SGLRID;
                private intParameter COLORCODERID;
                private intParameter SIZESRID;
                private intParameter SIZECODERID;
                private intParameter MIN;
                private intParameter MAX;
                private intParameter MULT;
                private intParameter ROWTYPEID;
                private intParameter DIMENSIONS_RID;
			
			    public SP_MID_SIZE_CONST_MM_INSERT_def()
			    {
                    base.procedureName = "SP_MID_SIZE_CONST_MM_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SIZE_CONST_MM");
			        SIZECONSTRAINTRID = new intParameter("@SIZECONSTRAINTRID", base.inputParameterList);
			        SGLRID = new intParameter("@SGLRID", base.inputParameterList);
			        COLORCODERID = new intParameter("@COLORCODERID", base.inputParameterList);
			        SIZESRID = new intParameter("@SIZESRID", base.inputParameterList);
			        SIZECODERID = new intParameter("@SIZECODERID", base.inputParameterList);
			        MIN = new intParameter("@MIN", base.inputParameterList);
			        MAX = new intParameter("@MAX", base.inputParameterList);
			        MULT = new intParameter("@MULT", base.inputParameterList);
			        ROWTYPEID = new intParameter("@ROWTYPEID", base.inputParameterList);
			        DIMENSIONS_RID = new intParameter("@DIMENSIONS_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? SIZECONSTRAINTRID,
			                      int? SGLRID,
			                      int? COLORCODERID,
			                      int? SIZESRID,
			                      int? SIZECODERID,
			                      int? MIN,
			                      int? MAX,
			                      int? MULT,
			                      int? ROWTYPEID,
			                      int? DIMENSIONS_RID
			                      )
			    {
                    lock (typeof(SP_MID_SIZE_CONST_MM_INSERT_def))
                    {
                        this.SIZECONSTRAINTRID.SetValue(SIZECONSTRAINTRID);
                        this.SGLRID.SetValue(SGLRID);
                        this.COLORCODERID.SetValue(COLORCODERID);
                        this.SIZESRID.SetValue(SIZESRID);
                        this.SIZECODERID.SetValue(SIZECODERID);
                        this.MIN.SetValue(MIN);
                        this.MAX.SetValue(MAX);
                        this.MULT.SetValue(MULT);
                        this.ROWTYPEID.SetValue(ROWTYPEID);
                        this.DIMENSIONS_RID.SetValue(DIMENSIONS_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_SIZE_CONST_GL_INSERT_def SP_MID_SIZE_CONST_GL_INSERT = new SP_MID_SIZE_CONST_GL_INSERT_def();
            public class SP_MID_SIZE_CONST_GL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_CONST_GL_INSERT.SQL"

                private intParameter SIZECONSTRAINTRID;
                private intParameter SGLRID;
                private intParameter MIN;
                private intParameter MAX;
                private intParameter MULT;
                private intParameter ROWTYPEID;
			
			    public SP_MID_SIZE_CONST_GL_INSERT_def()
			    {
                    base.procedureName = "SP_MID_SIZE_CONST_GL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SIZE_CONST_GL");
			        SIZECONSTRAINTRID = new intParameter("@SIZECONSTRAINTRID", base.inputParameterList);
			        SGLRID = new intParameter("@SGLRID", base.inputParameterList);
			        MIN = new intParameter("@MIN", base.inputParameterList);
			        MAX = new intParameter("@MAX", base.inputParameterList);
			        MULT = new intParameter("@MULT", base.inputParameterList);
			        ROWTYPEID = new intParameter("@ROWTYPEID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? SIZECONSTRAINTRID,
			                      int? SGLRID,
			                      int? MIN,
			                      int? MAX,
			                      int? MULT,
			                      int? ROWTYPEID
			                      )
			    {
                    lock (typeof(SP_MID_SIZE_CONST_GL_INSERT_def))
                    {
                        this.SIZECONSTRAINTRID.SetValue(SIZECONSTRAINTRID);
                        this.SGLRID.SetValue(SGLRID);
                        this.MIN.SetValue(MIN);
                        this.MAX.SetValue(MAX);
                        this.MULT.SetValue(MULT);
                        this.ROWTYPEID.SetValue(ROWTYPEID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
