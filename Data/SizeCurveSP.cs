using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class SizeCurve : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_SIZE_CURVE_GROUP_READ_FROM_NAME_def MID_SIZE_CURVE_GROUP_READ_FROM_NAME = new MID_SIZE_CURVE_GROUP_READ_FROM_NAME_def();
			public class MID_SIZE_CURVE_GROUP_READ_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GROUP_READ_FROM_NAME.SQL"

			    private stringParameter SIZE_CURVE_GROUP_NAME;
			
			    public MID_SIZE_CURVE_GROUP_READ_FROM_NAME_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_GROUP_READ_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CURVE_GROUP");
			        SIZE_CURVE_GROUP_NAME = new stringParameter("@SIZE_CURVE_GROUP_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_CURVE_GROUP_NAME)
			    {
                    lock (typeof(MID_SIZE_CURVE_GROUP_READ_FROM_NAME_def))
                    {
                        this.SIZE_CURVE_GROUP_NAME.SetValue(SIZE_CURVE_GROUP_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_GROUP_READ_FROM_NAME_UPPER_def MID_SIZE_CURVE_GROUP_READ_FROM_NAME_UPPER = new MID_SIZE_CURVE_GROUP_READ_FROM_NAME_UPPER_def();
			public class MID_SIZE_CURVE_GROUP_READ_FROM_NAME_UPPER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GROUP_READ_FROM_NAME_UPPER.SQL"

                private stringParameter SIZE_CURVE_GROUP_NAME;
			
			    public MID_SIZE_CURVE_GROUP_READ_FROM_NAME_UPPER_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_GROUP_READ_FROM_NAME_UPPER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CURVE_GROUP");
			        SIZE_CURVE_GROUP_NAME = new stringParameter("@SIZE_CURVE_GROUP_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_CURVE_GROUP_NAME)
			    {
                    lock (typeof(MID_SIZE_CURVE_GROUP_READ_FROM_NAME_UPPER_def))
                    {
                        this.SIZE_CURVE_GROUP_NAME.SetValue(SIZE_CURVE_GROUP_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_GROUP_READ_ALL_def MID_SIZE_CURVE_GROUP_READ_ALL = new MID_SIZE_CURVE_GROUP_READ_ALL_def();
			public class MID_SIZE_CURVE_GROUP_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GROUP_READ_ALL.SQL"

			
			    public MID_SIZE_CURVE_GROUP_READ_ALL_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_GROUP_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CURVE_GROUP");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SIZE_CURVE_GROUP_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_GROUP_READ_def MID_SIZE_CURVE_GROUP_READ = new MID_SIZE_CURVE_GROUP_READ_def();
			public class MID_SIZE_CURVE_GROUP_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GROUP_READ.SQL"

                private intParameter SIZE_CURVE_GROUP_RID;
			
			    public MID_SIZE_CURVE_GROUP_READ_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_GROUP_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CURVE_GROUP");
			        SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SIZE_CURVE_GROUP_RID)
			    {
                    lock (typeof(MID_SIZE_CURVE_GROUP_READ_def))
                    {
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_GROUP_READ_FROM_NAME_EXACT_def MID_SIZE_CURVE_GROUP_READ_FROM_NAME_EXACT = new MID_SIZE_CURVE_GROUP_READ_FROM_NAME_EXACT_def();
			public class MID_SIZE_CURVE_GROUP_READ_FROM_NAME_EXACT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GROUP_READ_FROM_NAME_EXACT.SQL"

                private stringParameter SIZE_CURVE_GROUP_NAME;
			
			    public MID_SIZE_CURVE_GROUP_READ_FROM_NAME_EXACT_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_GROUP_READ_FROM_NAME_EXACT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CURVE_GROUP");
			        SIZE_CURVE_GROUP_NAME = new stringParameter("@SIZE_CURVE_GROUP_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_CURVE_GROUP_NAME)
			    {
                    lock (typeof(MID_SIZE_CURVE_GROUP_READ_FROM_NAME_EXACT_def))
                    {
                        this.SIZE_CURVE_GROUP_NAME.SetValue(SIZE_CURVE_GROUP_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_SIZE_CURVE_GROUP_INSERT_def SP_MID_SIZE_CURVE_GROUP_INSERT = new SP_MID_SIZE_CURVE_GROUP_INSERT_def();
			public class SP_MID_SIZE_CURVE_GROUP_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_CURVE_GROUP_INSERT.SQL"

                private stringParameter SIZE_CURVE_GROUP_NAME;
                private intParameter DEFAULT_SIZE_CURVE_RID;
                private intParameter DEFINED_SIZE_GROUP_RID;
                private intParameter SIZE_CURVE_GROUP_RID; //Declare Output Parameter

                public SP_MID_SIZE_CURVE_GROUP_INSERT_def()
			    {
                    base.procedureName = "SP_MID_SIZE_CURVE_GROUP_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("SIZE_CURVE_GROUP");
			        SIZE_CURVE_GROUP_NAME = new stringParameter("@SIZE_CURVE_GROUP_NAME", base.inputParameterList);
			        DEFAULT_SIZE_CURVE_RID = new intParameter("@DEFAULT_SIZE_CURVE_RID", base.inputParameterList);
			        DEFINED_SIZE_GROUP_RID = new intParameter("@DEFINED_SIZE_GROUP_RID", base.inputParameterList);
			        SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string SIZE_CURVE_GROUP_NAME,
			                                  int? DEFAULT_SIZE_CURVE_RID,
			                                  int? DEFINED_SIZE_GROUP_RID
			                                  )
			    {
                    lock (typeof(SP_MID_SIZE_CURVE_GROUP_INSERT_def))
                    {
                        this.SIZE_CURVE_GROUP_NAME.SetValue(SIZE_CURVE_GROUP_NAME);
                        this.DEFAULT_SIZE_CURVE_RID.SetValue(DEFAULT_SIZE_CURVE_RID);
                        this.DEFINED_SIZE_GROUP_RID.SetValue(DEFINED_SIZE_GROUP_RID);
                        this.SIZE_CURVE_GROUP_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_GROUP_UPDATE_def MID_SIZE_CURVE_GROUP_UPDATE = new MID_SIZE_CURVE_GROUP_UPDATE_def();
			public class MID_SIZE_CURVE_GROUP_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GROUP_UPDATE.SQL"

                private intParameter SIZE_CURVE_GROUP_RID;
                private stringParameter SIZE_CURVE_GROUP_NAME;
                private intParameter DEFAULT_SIZE_CURVE_RID;
                private intParameter DEFINED_SIZE_GROUP_RID;
			
			    public MID_SIZE_CURVE_GROUP_UPDATE_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_GROUP_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("SIZE_CURVE_GROUP");
			        SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
			        SIZE_CURVE_GROUP_NAME = new stringParameter("@SIZE_CURVE_GROUP_NAME", base.inputParameterList);
			        DEFAULT_SIZE_CURVE_RID = new intParameter("@DEFAULT_SIZE_CURVE_RID", base.inputParameterList);
			        DEFINED_SIZE_GROUP_RID = new intParameter("@DEFINED_SIZE_GROUP_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? SIZE_CURVE_GROUP_RID,
			                      string SIZE_CURVE_GROUP_NAME,
			                      int? DEFAULT_SIZE_CURVE_RID,
			                      int? DEFINED_SIZE_GROUP_RID
			                      )
			    {
                    lock (typeof(MID_SIZE_CURVE_GROUP_UPDATE_def))
                    {
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        this.SIZE_CURVE_GROUP_NAME.SetValue(SIZE_CURVE_GROUP_NAME);
                        this.DEFAULT_SIZE_CURVE_RID.SetValue(DEFAULT_SIZE_CURVE_RID);
                        this.DEFINED_SIZE_GROUP_RID.SetValue(DEFINED_SIZE_GROUP_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_REMOVE_ALL_SIZE_CURVES_FROM_GROUP_def SP_MID_REMOVE_ALL_SIZE_CURVES_FROM_GROUP = new SP_MID_REMOVE_ALL_SIZE_CURVES_FROM_GROUP_def();
			public class SP_MID_REMOVE_ALL_SIZE_CURVES_FROM_GROUP_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_REMOVE_ALL_SIZE_CURVES_FROM_GROUP.SQL"

                private stringParameter inSizeCurveGroup;

                public SP_MID_REMOVE_ALL_SIZE_CURVES_FROM_GROUP_def()
			    {
                    base.procedureName = "SP_MID_REMOVE_ALL_SIZE_CURVES_FROM_GROUP";
			        base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("SIZE_CURVE_GROUP");
                    inSizeCurveGroup = new stringParameter("@inSizeCurveGroup", base.inputParameterList);
			    }

                public int Delete(DatabaseAccess _dba, string inSizeCurveGroup)
			    {
                    lock (typeof(SP_MID_REMOVE_ALL_SIZE_CURVES_FROM_GROUP_def))
                    {
                        this.inSizeCurveGroup.SetValue(inSizeCurveGroup);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static MID_GET_SIZE_CURVES_FOR_GROUP_def MID_GET_SIZE_CURVES_FOR_GROUP_READ = new MID_GET_SIZE_CURVES_FOR_GROUP_def();
			public class MID_GET_SIZE_CURVES_FOR_GROUP_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_SIZE_CURVES_FOR_GROUP.SQL"

                private intParameter SizeCurveGroupRid;

                public MID_GET_SIZE_CURVES_FOR_GROUP_def()
			    {
                    base.procedureName = "MID_GET_SIZE_CURVES_FOR_GROUP";
			        base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("SIZE_CURVE_GROUP");
                    SizeCurveGroupRid = new intParameter("@SizeCurveGroupRid", base.inputParameterList);
			    }

                public DataSet ReadAsDataSet(DatabaseAccess _dba, int? SizeCurveGroupRid)
			    {
                    lock (typeof(MID_GET_SIZE_CURVES_FOR_GROUP_def))
                    {
                        this.SizeCurveGroupRid.SetValue(SizeCurveGroupRid);
                        DataSet ds = ExecuteStoredProcedureForReadAsDataSet(_dba);

                        ds.Tables[0].TableName = "GroupNameAndDefaultCurveRID";
                        ds.Tables[1].TableName = "SizeCurveData";
                        return ds;
                    }
			    }
			}


            public static SP_MID_IS_SIZE_CURVE_GROUP_IN_USE_def SP_MID_IS_SIZE_CURVE_GROUP_IN_USE = new SP_MID_IS_SIZE_CURVE_GROUP_IN_USE_def();
            public class SP_MID_IS_SIZE_CURVE_GROUP_IN_USE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_IS_SIZE_CURVE_GROUP_IN_USE.SQL"

                private intParameter SCG_RID;
                private intParameter ReturnCode; //Declare Output Parameter
			
			    public SP_MID_IS_SIZE_CURVE_GROUP_IN_USE_def()
			    {
                    base.procedureName = "SP_MID_IS_SIZE_CURVE_GROUP_IN_USE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CONSTRAINT_MODEL");
			        SCG_RID = new intParameter("@SCG_RID", base.inputParameterList);
			        ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int ReadValue(DatabaseAccess _dba, int? SCG_RID)
			    {
                    lock (typeof(SP_MID_IS_SIZE_CURVE_GROUP_IN_USE_def))
                    {
                        this.SCG_RID.SetValue(SCG_RID);
                        this.ReturnCode.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForRead(_dba);
                        return (int)this.ReturnCode.Value;
                    }
			    }
			}

            public static SP_MID_SIZE_CURVE_GROUP_DELETE_def SP_MID_SIZE_CURVE_GROUP_DELETE = new SP_MID_SIZE_CURVE_GROUP_DELETE_def();
			public class SP_MID_SIZE_CURVE_GROUP_DELETE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_CURVE_GROUP_DELETE.SQL"

                private intParameter SIZE_CURVE_GROUP_RID;

                public SP_MID_SIZE_CURVE_GROUP_DELETE_def()
			    {
                    base.procedureName = "SP_MID_SIZE_CURVE_GROUP_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_CURVE_GROUP");
			        SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_CURVE_GROUP_RID)
			    {
                    lock (typeof(SP_MID_SIZE_CURVE_GROUP_DELETE_def))
                    {
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_GROUP_JOIN_INSERT_def MID_SIZE_CURVE_GROUP_JOIN_INSERT = new MID_SIZE_CURVE_GROUP_JOIN_INSERT_def();
			public class MID_SIZE_CURVE_GROUP_JOIN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GROUP_JOIN_INSERT.SQL"

                private intParameter SIZE_CURVE_GROUP_RID;
                private intParameter SIZE_CURVE_RID;
			
			    public MID_SIZE_CURVE_GROUP_JOIN_INSERT_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_GROUP_JOIN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SIZE_CURVE_GROUP_JOIN");
			        SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
			        SIZE_CURVE_RID = new intParameter("@SIZE_CURVE_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? SIZE_CURVE_GROUP_RID,
			                      int? SIZE_CURVE_RID
			                      )
			    {
                    lock (typeof(MID_SIZE_CURVE_GROUP_JOIN_INSERT_def))
                    {
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        this.SIZE_CURVE_RID.SetValue(SIZE_CURVE_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_GROUP_JOIN_DELETE_def MID_SIZE_CURVE_GROUP_JOIN_DELETE = new MID_SIZE_CURVE_GROUP_JOIN_DELETE_def();
			public class MID_SIZE_CURVE_GROUP_JOIN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GROUP_JOIN_DELETE.SQL"

                private intParameter SIZE_CURVE_GROUP_RID;
			
			    public MID_SIZE_CURVE_GROUP_JOIN_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_GROUP_JOIN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_CURVE_GROUP_JOIN");
			        SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_CURVE_GROUP_RID)
			    {
                    lock (typeof(MID_SIZE_CURVE_GROUP_JOIN_DELETE_def))
                    {
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_READ_def MID_SIZE_CURVE_READ = new MID_SIZE_CURVE_READ_def();
			public class MID_SIZE_CURVE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_READ.SQL"

                private intParameter SIZE_CURVE_RID;
                private stringParameter SIZE_CODE_SECONDARY;
			
			    public MID_SIZE_CURVE_READ_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CURVE");
			        SIZE_CURVE_RID = new intParameter("@SIZE_CURVE_RID", base.inputParameterList);
			        SIZE_CODE_SECONDARY = new stringParameter("@SIZE_CODE_SECONDARY", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SIZE_CURVE_RID,
			                          string SIZE_CODE_SECONDARY
			                          )
			    {
                    lock (typeof(MID_SIZE_CURVE_READ_def))
                    {
                        this.SIZE_CURVE_RID.SetValue(SIZE_CURVE_RID);
                        this.SIZE_CODE_SECONDARY.SetValue(SIZE_CODE_SECONDARY);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_GROUP_READ_FOR_SIZE_CURVE_GROUP_def MID_SIZE_CURVE_GROUP_READ_FOR_SIZE_CURVE_GROUP = new MID_SIZE_CURVE_GROUP_READ_FOR_SIZE_CURVE_GROUP_def();
			public class MID_SIZE_CURVE_GROUP_READ_FOR_SIZE_CURVE_GROUP_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GROUP_READ_FOR_SIZE_CURVE_GROUP.SQL"

                private intParameter SIZE_CURVE_RID;
			
			    public MID_SIZE_CURVE_GROUP_READ_FOR_SIZE_CURVE_GROUP_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_GROUP_READ_FOR_SIZE_CURVE_GROUP";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CURVE_GROUP");
			        SIZE_CURVE_RID = new intParameter("@SIZE_CURVE_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SIZE_CURVE_RID)
			    {
                    lock (typeof(MID_SIZE_CURVE_GROUP_READ_FOR_SIZE_CURVE_GROUP_def))
                    {
                        this.SIZE_CURVE_RID.SetValue(SIZE_CURVE_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_READ_KEY_def MID_SIZE_CURVE_READ_KEY = new MID_SIZE_CURVE_READ_KEY_def();
			public class MID_SIZE_CURVE_READ_KEY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_READ_KEY.SQL"

                private stringParameter SIZE_CURVE_NAME;
			
			    public MID_SIZE_CURVE_READ_KEY_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_READ_KEY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SIZE_CURVE");
			        SIZE_CURVE_NAME = new stringParameter("@SIZE_CURVE_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SIZE_CURVE_NAME)
			    {
                    lock (typeof(MID_SIZE_CURVE_READ_KEY_def))
                    {
                        this.SIZE_CURVE_NAME.SetValue(SIZE_CURVE_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}


            public static SP_MID_SIZE_CURVE_INSERT_def SP_MID_SIZE_CURVE_INSERT = new SP_MID_SIZE_CURVE_INSERT_def();
            public class SP_MID_SIZE_CURVE_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_CURVE_INSERT.SQL"

                private stringParameter SIZE_CURVE_NAME;
                private intParameter SIZE_CURVE_RID; //Declare Output Parameter
			
			    public SP_MID_SIZE_CURVE_INSERT_def()
			    {
                    base.procedureName = "SP_MID_SIZE_CURVE_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("SIZE_CURVE");
			        SIZE_CURVE_NAME = new stringParameter("@SIZE_CURVE_NAME", base.inputParameterList);
			        SIZE_CURVE_RID = new intParameter("@SIZE_CURVE_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, string SIZE_CURVE_NAME)
			    {
                    lock (typeof(SP_MID_SIZE_CURVE_INSERT_def))
                    {
                        this.SIZE_CURVE_NAME.SetValue(SIZE_CURVE_NAME);
                        this.SIZE_CURVE_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_UPDATE_def MID_SIZE_CURVE_UPDATE = new MID_SIZE_CURVE_UPDATE_def();
			public class MID_SIZE_CURVE_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_UPDATE.SQL"

                private intParameter SIZE_CURVE_RID;
                private stringParameter SIZE_CURVE_NAME;
			
			    public MID_SIZE_CURVE_UPDATE_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("SIZE_CURVE");
			        SIZE_CURVE_RID = new intParameter("@SIZE_CURVE_RID", base.inputParameterList);
			        SIZE_CURVE_NAME = new stringParameter("@SIZE_CURVE_NAME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? SIZE_CURVE_RID,
			                      string SIZE_CURVE_NAME
			                      )
			    {
                    lock (typeof(MID_SIZE_CURVE_UPDATE_def))
                    {
                        this.SIZE_CURVE_RID.SetValue(SIZE_CURVE_RID);
                        this.SIZE_CURVE_NAME.SetValue(SIZE_CURVE_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_SIZE_CURVE_DELETE_def SP_MID_SIZE_CURVE_DELETE = new SP_MID_SIZE_CURVE_DELETE_def();
            public class SP_MID_SIZE_CURVE_DELETE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SIZE_CURVE_DELETE.SQL"

                private intParameter SIZE_CURVE_RID;
			
			    public SP_MID_SIZE_CURVE_DELETE_def()
			    {
                    base.procedureName = "SP_MID_SIZE_CURVE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_CURVE");
			        SIZE_CURVE_RID = new intParameter("@SIZE_CURVE_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_CURVE_RID)
			    {
                    lock (typeof(SP_MID_SIZE_CURVE_DELETE_def))
                    {
                        this.SIZE_CURVE_RID.SetValue(SIZE_CURVE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}


			public static MID_SIZE_CURVE_JOIN_INSERT_def MID_SIZE_CURVE_JOIN_INSERT = new MID_SIZE_CURVE_JOIN_INSERT_def();
			public class MID_SIZE_CURVE_JOIN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_JOIN_INSERT.SQL"

                private intParameter SIZE_CURVE_RID;
                private intParameter SIZE_CODE_RID;
                private floatParameter PERCENT;
			
			    public MID_SIZE_CURVE_JOIN_INSERT_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_JOIN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SIZE_CURVE_JOIN");
			        SIZE_CURVE_RID = new intParameter("@SIZE_CURVE_RID", base.inputParameterList);
			        SIZE_CODE_RID = new intParameter("@SIZE_CODE_RID", base.inputParameterList);
			        PERCENT = new floatParameter("@PERCENT", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? SIZE_CURVE_RID,
			                      int? SIZE_CODE_RID,
			                      double? PERCENT
			                      )
			    {
                    lock (typeof(MID_SIZE_CURVE_JOIN_INSERT_def))
                    {
                        this.SIZE_CURVE_RID.SetValue(SIZE_CURVE_RID);
                        this.SIZE_CODE_RID.SetValue(SIZE_CODE_RID);
                        this.PERCENT.SetValue(PERCENT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_JOIN_DELETE_def MID_SIZE_CURVE_JOIN_DELETE = new MID_SIZE_CURVE_JOIN_DELETE_def();
			public class MID_SIZE_CURVE_JOIN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_JOIN_DELETE.SQL"

                private intParameter SIZE_CURVE_RID;
			
			    public MID_SIZE_CURVE_JOIN_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_JOIN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_CURVE_JOIN");
			        SIZE_CURVE_RID = new intParameter("@SIZE_CURVE_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_CURVE_RID)
			    {
                    lock (typeof(MID_SIZE_CURVE_JOIN_DELETE_def))
                    {
                        this.SIZE_CURVE_RID.SetValue(SIZE_CURVE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_SIZE_CURVE_BY_GROUP_INSERT_def MID_STORE_SIZE_CURVE_BY_GROUP_INSERT = new MID_STORE_SIZE_CURVE_BY_GROUP_INSERT_def();
			public class MID_STORE_SIZE_CURVE_BY_GROUP_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_SIZE_CURVE_BY_GROUP_INSERT.SQL"

                private intParameter ST_RID;
                private intParameter SIZE_CURVE_GROUP_RID;
                private intParameter SIZE_CURVE_RID;
			    
			
			    public MID_STORE_SIZE_CURVE_BY_GROUP_INSERT_def()
			    {
			        base.procedureName = "MID_STORE_SIZE_CURVE_BY_GROUP_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("STORE_SIZE_CURVE_BY_GROUP");
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
                    SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
                    SIZE_CURVE_RID = new intParameter("@SIZE_CURVE_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? ST_RID,
			                      int? SIZE_CURVE_GROUP_RID,
			                      int? SIZE_CURVE_RID
			                      )
			    {
                    lock (typeof(MID_STORE_SIZE_CURVE_BY_GROUP_INSERT_def))
                    {
                        this.ST_RID.SetValue(ST_RID);
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        this.SIZE_CURVE_RID.SetValue(SIZE_CURVE_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SIZE_CURVE_GROUP_RID_DELETE_def MID_SIZE_CURVE_GROUP_RID_DELETE = new MID_SIZE_CURVE_GROUP_RID_DELETE_def();
			public class MID_SIZE_CURVE_GROUP_RID_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SIZE_CURVE_GROUP_RID_DELETE.SQL"

                private intParameter SIZE_CURVE_GROUP_RID;
			
			    public MID_SIZE_CURVE_GROUP_RID_DELETE_def()
			    {
			        base.procedureName = "MID_SIZE_CURVE_GROUP_RID_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SIZE_CURVE_GROUP_RID");
			        SIZE_CURVE_GROUP_RID = new intParameter("@SIZE_CURVE_GROUP_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SIZE_CURVE_GROUP_RID)
			    {
                    lock (typeof(MID_SIZE_CURVE_GROUP_RID_DELETE_def))
                    {
                        this.SIZE_CURVE_GROUP_RID.SetValue(SIZE_CURVE_GROUP_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
