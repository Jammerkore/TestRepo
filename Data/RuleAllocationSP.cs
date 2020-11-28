using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class RuleAllocation : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_HEADER_BULK_COLOR_READ_BULK_COLOR_RID_def MID_HEADER_BULK_COLOR_READ_BULK_COLOR_RID = new MID_HEADER_BULK_COLOR_READ_BULK_COLOR_RID_def();
            public class MID_HEADER_BULK_COLOR_READ_BULK_COLOR_RID_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_BULK_COLOR_READ_BULK_COLOR_RID.SQL"

                private intParameter HDR_RID;
                private intParameter COLOR_CODE_RID;

                public MID_HEADER_BULK_COLOR_READ_BULK_COLOR_RID_def()
                {
                    base.procedureName = "MID_HEADER_BULK_COLOR_READ_BULK_COLOR_RID";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("HEADER_BULK_COLOR");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    COLOR_CODE_RID = new intParameter("@COLOR_CODE_RID", base.inputParameterList);
                }

                public object ReadValues(DatabaseAccess _dba,
                                         int? HDR_RID,
                                         int? COLOR_CODE_RID
                                         )
                {
                    lock (typeof(MID_HEADER_BULK_COLOR_READ_BULK_COLOR_RID_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.COLOR_CODE_RID.SetValue(COLOR_CODE_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

            public static MID_HEADER_PACK_READ_RID_FROM_PACK_NAME_def MID_HEADER_PACK_READ_RID_FROM_PACK_NAME = new MID_HEADER_PACK_READ_RID_FROM_PACK_NAME_def();
            public class MID_HEADER_PACK_READ_RID_FROM_PACK_NAME_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HEADER_PACK_READ_RID_FROM_PACK_NAME.SQL"

                private intParameter HDR_RID;
                private stringParameter HDR_PACK_NAME;

                public MID_HEADER_PACK_READ_RID_FROM_PACK_NAME_def()
                {
                    base.procedureName = "MID_HEADER_PACK_READ_RID_FROM_PACK_NAME";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("HEADER_PACK");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    HDR_PACK_NAME = new stringParameter("@HDR_PACK_NAME", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba,
                                         int? HDR_RID,
                                         string HDR_PACK_NAME
                                         )
                {
                    lock (typeof(MID_HEADER_PACK_READ_RID_FROM_PACK_NAME_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_PACK_NAME.SetValue(HDR_PACK_NAME);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

			public static MID_BULK_RULE_DELETE_FROM_HEADER_AND_LAYER_def MID_BULK_RULE_DELETE_FROM_HEADER_AND_LAYER = new MID_BULK_RULE_DELETE_FROM_HEADER_AND_LAYER_def();
			public class MID_BULK_RULE_DELETE_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_DELETE_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_RULE_DELETE_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_BULK_RULE_DELETE_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("BULK_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID
			                      )
			    {
                    lock (typeof(MID_BULK_RULE_DELETE_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_DELETE_FROM_HEADER_AND_LAYER_def MID_TOTAL_RULE_DELETE_FROM_HEADER_AND_LAYER = new MID_TOTAL_RULE_DELETE_FROM_HEADER_AND_LAYER_def();
			public class MID_TOTAL_RULE_DELETE_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_DELETE_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_TOTAL_RULE_DELETE_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_DELETE_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TOTAL_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID
			                      )
			    {
                    lock (typeof(MID_TOTAL_RULE_DELETE_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_DELETE_FROM_HEADER_AND_LAYER_def MID_DETAIL_RULE_DELETE_FROM_HEADER_AND_LAYER = new MID_DETAIL_RULE_DELETE_FROM_HEADER_AND_LAYER_def();
			public class MID_DETAIL_RULE_DELETE_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_DELETE_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_DETAIL_RULE_DELETE_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_DELETE_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("DETAIL_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID
			                      )
			    {
                    lock (typeof(MID_DETAIL_RULE_DELETE_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_DELETE_FROM_HEADER_AND_LAYER_def MID_BULK_COLOR_RULE_DELETE_FROM_HEADER_AND_LAYER = new MID_BULK_COLOR_RULE_DELETE_FROM_HEADER_AND_LAYER_def();
			public class MID_BULK_COLOR_RULE_DELETE_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_DELETE_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_COLOR_RULE_DELETE_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_DELETE_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("BULK_COLOR_RULE");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HDR_BC_RID,
			                      int? LAYER_ID
			                      )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_DELETE_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_DELETE_FROM_HEADER_AND_LAYER_def MID_PACK_RULE_DELETE_FROM_HEADER_AND_LAYER = new MID_PACK_RULE_DELETE_FROM_HEADER_AND_LAYER_def();
			public class MID_PACK_RULE_DELETE_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_DELETE_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
			
			    public MID_PACK_RULE_DELETE_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_PACK_RULE_DELETE_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("PACK_RULE");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HDR_PACK_RID,
			                      int? LAYER_ID
			                      )
			    {
                    lock (typeof(MID_PACK_RULE_DELETE_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def MID_TOTAL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER = new MID_TOTAL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def();
			public class MID_TOTAL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_TOTAL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TOTAL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID
			                      )
			    {
                    lock (typeof(MID_TOTAL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def MID_BULK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER = new MID_BULK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def();
			public class MID_BULK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_BULK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("BULK_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID
			                      )
			    {
                    lock (typeof(MID_BULK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def MID_DETAIL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER = new MID_DETAIL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def();
			public class MID_DETAIL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_DETAIL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("DETAIL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID
			                      )
			    {
                    lock (typeof(MID_DETAIL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def MID_BULK_COLOR_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER = new MID_BULK_COLOR_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def();
			public class MID_BULK_COLOR_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_COLOR_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("BULK_COLOR_RULE_LAYER");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HDR_BC_RID,
			                      int? LAYER_ID
			                      )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def MID_PACK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER = new MID_PACK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def();
			public class MID_PACK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
			
			    public MID_PACK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_PACK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("PACK_RULE_LAYER");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? HDR_PACK_RID,
			                      int? LAYER_ID
			                      )
			    {
                    lock (typeof(MID_PACK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_XML_BULK_RULE_WRITE_def SP_MID_XML_BULK_RULE_WRITE = new SP_MID_XML_BULK_RULE_WRITE_def();
            public class SP_MID_XML_BULK_RULE_WRITE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_XML_BULK_RULE_WRITE.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private tableParameter RULE_TABLE;
			
			    public SP_MID_XML_BULK_RULE_WRITE_def()
			    {
                    base.procedureName = "SP_MID_XML_BULK_RULE_WRITE";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BULK_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        RULE_TABLE = new tableParameter("@RULE_TABLE", "BULK_RULE_TYPE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      DataTable RULE_TABLE
			                      )
			    {
                    lock (typeof(SP_MID_XML_BULK_RULE_WRITE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.RULE_TABLE.SetValue(RULE_TABLE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_XML_TOTAL_RULE_WRITE_def SP_MID_XML_TOTAL_RULE_WRITE = new SP_MID_XML_TOTAL_RULE_WRITE_def();
            public class SP_MID_XML_TOTAL_RULE_WRITE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_XML_TOTAL_RULE_WRITE.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private tableParameter RULE_TABLE;

                public SP_MID_XML_TOTAL_RULE_WRITE_def()
                {
                    base.procedureName = "SP_MID_XML_TOTAL_RULE_WRITE";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("TOTAL_RULE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
                    RULE_TABLE = new tableParameter("@RULE_TABLE", "TOTAL_RULE_TYPE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? HDR_RID,
                                  int? LAYER_ID,
                                  DataTable RULE_TABLE
                                  )
                {
                    lock (typeof(SP_MID_XML_TOTAL_RULE_WRITE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.RULE_TABLE.SetValue(RULE_TABLE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static SP_MID_XML_DETAIL_RULE_WRITE_def SP_MID_XML_DETAIL_RULE_WRITE = new SP_MID_XML_DETAIL_RULE_WRITE_def();
            public class SP_MID_XML_DETAIL_RULE_WRITE_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_XML_DETAIL_RULE_WRITE.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private tableParameter RULE_TABLE;

                public SP_MID_XML_DETAIL_RULE_WRITE_def()
                {
                    base.procedureName = "SP_MID_XML_DETAIL_RULE_WRITE";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("DETAIL_RULE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
                    LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
                    RULE_TABLE = new tableParameter("@RULE_TABLE", "DETAIL_RULE_TYPE", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? HDR_RID,
                                  int? LAYER_ID,
                                  DataTable RULE_TABLE
                                  )
                {
                    lock (typeof(SP_MID_XML_DETAIL_RULE_WRITE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.RULE_TABLE.SetValue(RULE_TABLE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static SP_MID_XML_COLOR_RULE_WRITE_def SP_MID_XML_COLOR_RULE_WRITE = new SP_MID_XML_COLOR_RULE_WRITE_def();
            public class SP_MID_XML_COLOR_RULE_WRITE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_XML_COLOR_RULE_WRITE.SQL"

                private intParameter HDR_RID;
                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
                private tableParameter RULE_TABLE;
			
			    public SP_MID_XML_COLOR_RULE_WRITE_def()
			    {
                    base.procedureName = "SP_MID_XML_COLOR_RULE_WRITE";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BULK_COLOR_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        RULE_TABLE = new tableParameter("@RULE_TABLE", "COLOR_RULE_TYPE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? HDR_BC_RID,
			                      int? LAYER_ID,
			                      DataTable RULE_TABLE
			                      )
			    {
                    lock (typeof(SP_MID_XML_COLOR_RULE_WRITE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.RULE_TABLE.SetValue(RULE_TABLE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_XML_PACK_RULE_WRITE_def SP_MID_XML_PACK_RULE_WRITE = new SP_MID_XML_PACK_RULE_WRITE_def();
            public class SP_MID_XML_PACK_RULE_WRITE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_XML_PACK_RULE_WRITE.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
                private tableParameter RULE_TABLE;
			
			    public SP_MID_XML_PACK_RULE_WRITE_def()
			    {
                    base.procedureName = "SP_MID_XML_PACK_RULE_WRITE";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("PACK_RULE");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        RULE_TABLE = new tableParameter("@RULE_TABLE", "PACK_RULE_TYPE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_PACK_RID,
			                      int? LAYER_ID,
			                      DataTable RULE_TABLE
			                      )
			    {
                    lock (typeof(SP_MID_XML_PACK_RULE_WRITE_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.RULE_TABLE.SetValue(RULE_TABLE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def MID_BULK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER = new MID_BULK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def();
			public class MID_BULK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_BULK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("BULK_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_RID,
			                               int? LAYER_ID
			                               )
			    {
                    lock (typeof(MID_BULK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def MID_TOTAL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER = new MID_TOTAL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def();
			public class MID_TOTAL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_TOTAL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("TOTAL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_RID,
			                               int? LAYER_ID
			                               )
			    {
                    lock (typeof(MID_TOTAL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def MID_DETAIL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER = new MID_DETAIL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def();
			public class MID_DETAIL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_DETAIL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("DETAIL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_RID,
			                               int? LAYER_ID
			                               )
			    {
                    lock (typeof(MID_DETAIL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def MID_BULK_COLOR_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER = new MID_BULK_COLOR_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def();
			public class MID_BULK_COLOR_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_COLOR_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("BULK_COLOR_RULE_LAYER");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_BC_RID,
			                               int? LAYER_ID
			                               )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def MID_PACK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER = new MID_PACK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def();
			public class MID_PACK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
			
			    public MID_PACK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_PACK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("PACK_RULE_LAYER");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_PACK_RID,
			                               int? LAYER_ID
			                               )
			    {
                    lock (typeof(MID_PACK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def MID_BULK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER = new MID_BULK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def();
			public class MID_BULK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
			
			    public MID_BULK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_BULK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("BULK_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_RID,
			                               int? LAYER_ID,
			                               int? ST_RID
			                               )
			    {
                    lock (typeof(MID_BULK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def MID_TOTAL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER = new MID_TOTAL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def();
			public class MID_TOTAL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
			
			    public MID_TOTAL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("TOTAL_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_RID,
			                               int? LAYER_ID,
			                               int? ST_RID
			                               )
			    {
                    lock (typeof(MID_TOTAL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER = new MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def();
			public class MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
			
			    public MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("DETAIL_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_RID,
			                               int? LAYER_ID,
			                               int? ST_RID
			                               )
			    {
                    lock (typeof(MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def MID_BULK_COLOR_RULE_READ_COUNT_FROM_HEADER_AND_LAYER = new MID_BULK_COLOR_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def();
			public class MID_BULK_COLOR_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
			
			    public MID_BULK_COLOR_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_READ_COUNT_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("BULK_COLOR_RULE");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_BC_RID,
			                               int? LAYER_ID,
			                               int? ST_RID
			                               )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def MID_PACK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER = new MID_PACK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def();
			public class MID_PACK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
			
			    public MID_PACK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def()
			    {
			        base.procedureName = "MID_PACK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("PACK_RULE");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_PACK_RID,
			                               int? LAYER_ID,
			                               int? ST_RID
			                               )
			    {
                    lock (typeof(MID_PACK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_UPDATE_def MID_BULK_RULE_UPDATE = new MID_BULK_RULE_UPDATE_def();
			public class MID_BULK_RULE_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_UPDATE.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
                private intParameter RULE_TYPE_ID;
                private intParameter UNITS;
			
			    public MID_BULK_RULE_UPDATE_def()
			    {
			        base.procedureName = "MID_BULK_RULE_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("BULK_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        RULE_TYPE_ID = new intParameter("@RULE_TYPE_ID", base.inputParameterList);
			        UNITS = new intParameter("@UNITS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? ST_RID,
			                      int? RULE_TYPE_ID,
			                      int? UNITS
			                      )
			    {
                    lock (typeof(MID_BULK_RULE_UPDATE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.RULE_TYPE_ID.SetValue(RULE_TYPE_ID);
                        this.UNITS.SetValue(UNITS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_UPDATE_def MID_TOTAL_RULE_UPDATE = new MID_TOTAL_RULE_UPDATE_def();
			public class MID_TOTAL_RULE_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_UPDATE.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
                private intParameter RULE_TYPE_ID;
                private intParameter UNITS;
			
			    public MID_TOTAL_RULE_UPDATE_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("TOTAL_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        RULE_TYPE_ID = new intParameter("@RULE_TYPE_ID", base.inputParameterList);
			        UNITS = new intParameter("@UNITS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? ST_RID,
			                      int? RULE_TYPE_ID,
			                      int? UNITS
			                      )
			    {
                    lock (typeof(MID_TOTAL_RULE_UPDATE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.RULE_TYPE_ID.SetValue(RULE_TYPE_ID);
                        this.UNITS.SetValue(UNITS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_UPDATE_def MID_DETAIL_RULE_UPDATE = new MID_DETAIL_RULE_UPDATE_def();
			public class MID_DETAIL_RULE_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_UPDATE.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
                private intParameter RULE_TYPE_ID;
                private intParameter UNITS;
			
			    public MID_DETAIL_RULE_UPDATE_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("DETAIL_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        RULE_TYPE_ID = new intParameter("@RULE_TYPE_ID", base.inputParameterList);
			        UNITS = new intParameter("@UNITS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? ST_RID,
			                      int? RULE_TYPE_ID,
			                      int? UNITS
			                      )
			    {
                    lock (typeof(MID_DETAIL_RULE_UPDATE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.RULE_TYPE_ID.SetValue(RULE_TYPE_ID);
                        this.UNITS.SetValue(UNITS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_UPDATE_def MID_BULK_COLOR_RULE_UPDATE = new MID_BULK_COLOR_RULE_UPDATE_def();
			public class MID_BULK_COLOR_RULE_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_UPDATE.SQL"

                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
                private intParameter RULE_TYPE_ID;
                private intParameter UNITS;
			
			    public MID_BULK_COLOR_RULE_UPDATE_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("BULK_COLOR_RULE");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        RULE_TYPE_ID = new intParameter("@RULE_TYPE_ID", base.inputParameterList);
			        UNITS = new intParameter("@UNITS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_BC_RID,
			                      int? LAYER_ID,
			                      int? ST_RID,
			                      int? RULE_TYPE_ID,
			                      int? UNITS
			                      )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_UPDATE_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.RULE_TYPE_ID.SetValue(RULE_TYPE_ID);
                        this.UNITS.SetValue(UNITS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_UPDATE_def MID_PACK_RULE_UPDATE = new MID_PACK_RULE_UPDATE_def();
			public class MID_PACK_RULE_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_UPDATE.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
                private intParameter RULE_TYPE_ID;
                private intParameter PACKS;
			
			    public MID_PACK_RULE_UPDATE_def()
			    {
			        base.procedureName = "MID_PACK_RULE_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PACK_RULE");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        RULE_TYPE_ID = new intParameter("@RULE_TYPE_ID", base.inputParameterList);
			        PACKS = new intParameter("@PACKS", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_PACK_RID,
			                      int? LAYER_ID,
			                      int? ST_RID,
			                      int? RULE_TYPE_ID,
			                      int? PACKS
			                      )
			    {
                    lock (typeof(MID_PACK_RULE_UPDATE_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.RULE_TYPE_ID.SetValue(RULE_TYPE_ID);
                        this.PACKS.SetValue(PACKS);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_INSERT_def MID_BULK_RULE_INSERT = new MID_BULK_RULE_INSERT_def();
			public class MID_BULK_RULE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
                private intParameter RULE_TYPE_ID;
                private intParameter UNITS;
			
			    public MID_BULK_RULE_INSERT_def()
			    {
			        base.procedureName = "MID_BULK_RULE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BULK_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        RULE_TYPE_ID = new intParameter("@RULE_TYPE_ID", base.inputParameterList);
			        UNITS = new intParameter("@UNITS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? ST_RID,
			                      int? RULE_TYPE_ID,
			                      int? UNITS
			                      )
			    {
                    lock (typeof(MID_BULK_RULE_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.RULE_TYPE_ID.SetValue(RULE_TYPE_ID);
                        this.UNITS.SetValue(UNITS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_INSERT_def MID_TOTAL_RULE_INSERT = new MID_TOTAL_RULE_INSERT_def();
			public class MID_TOTAL_RULE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
                private intParameter RULE_TYPE_ID;
                private intParameter UNITS;
			
			    public MID_TOTAL_RULE_INSERT_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TOTAL_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        RULE_TYPE_ID = new intParameter("@RULE_TYPE_ID", base.inputParameterList);
			        UNITS = new intParameter("@UNITS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? ST_RID,
			                      int? RULE_TYPE_ID,
			                      int? UNITS
			                      )
			    {
                    lock (typeof(MID_TOTAL_RULE_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.RULE_TYPE_ID.SetValue(RULE_TYPE_ID);
                        this.UNITS.SetValue(UNITS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_INSERT_def MID_DETAIL_RULE_INSERT = new MID_DETAIL_RULE_INSERT_def();
			public class MID_DETAIL_RULE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
                private intParameter RULE_TYPE_ID;
                private intParameter UNITS;
			
			    public MID_DETAIL_RULE_INSERT_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("DETAIL_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        RULE_TYPE_ID = new intParameter("@RULE_TYPE_ID", base.inputParameterList);
			        UNITS = new intParameter("@UNITS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? ST_RID,
			                      int? RULE_TYPE_ID,
			                      int? UNITS
			                      )
			    {
                    lock (typeof(MID_DETAIL_RULE_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.RULE_TYPE_ID.SetValue(RULE_TYPE_ID);
                        this.UNITS.SetValue(UNITS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_INSERT_def MID_BULK_COLOR_RULE_INSERT = new MID_BULK_COLOR_RULE_INSERT_def();
			public class MID_BULK_COLOR_RULE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
                private intParameter RULE_TYPE_ID;
                private intParameter UNITS;
			
			    public MID_BULK_COLOR_RULE_INSERT_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BULK_COLOR_RULE");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        RULE_TYPE_ID = new intParameter("@RULE_TYPE_ID", base.inputParameterList);
			        UNITS = new intParameter("@UNITS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba,
                                  int? HDR_RID,
			                      int? HDR_BC_RID,
			                      int? LAYER_ID,
			                      int? ST_RID,
			                      int? RULE_TYPE_ID,
			                      int? UNITS
			                      )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.RULE_TYPE_ID.SetValue(RULE_TYPE_ID);
                        this.UNITS.SetValue(UNITS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_INSERT_def MID_PACK_RULE_INSERT = new MID_PACK_RULE_INSERT_def();
			public class MID_PACK_RULE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_INSERT.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
                private intParameter ST_RID;
                private intParameter RULE_TYPE_ID;
                private intParameter PACKS;
			
			    public MID_PACK_RULE_INSERT_def()
			    {
			        base.procedureName = "MID_PACK_RULE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("PACK_RULE");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        RULE_TYPE_ID = new intParameter("@RULE_TYPE_ID", base.inputParameterList);
			        PACKS = new intParameter("@PACKS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_PACK_RID,
			                      int? LAYER_ID,
			                      int? ST_RID,
			                      int? RULE_TYPE_ID,
			                      int? PACKS
			                      )
			    {
                    lock (typeof(MID_PACK_RULE_INSERT_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.RULE_TYPE_ID.SetValue(RULE_TYPE_ID);
                        this.PACKS.SetValue(PACKS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_LAYER_READ_COUNT_def MID_BULK_RULE_LAYER_READ_COUNT = new MID_BULK_RULE_LAYER_READ_COUNT_def();
			public class MID_BULK_RULE_LAYER_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_LAYER_READ_COUNT.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_RULE_LAYER_READ_COUNT_def()
			    {
			        base.procedureName = "MID_BULK_RULE_LAYER_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("BULK_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_RID,
			                               int? LAYER_ID
			                               )
			    {
                    lock (typeof(MID_BULK_RULE_LAYER_READ_COUNT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_LAYER_READ_COUNT_def MID_TOTAL_RULE_LAYER_READ_COUNT = new MID_TOTAL_RULE_LAYER_READ_COUNT_def();
			public class MID_TOTAL_RULE_LAYER_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_LAYER_READ_COUNT.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_TOTAL_RULE_LAYER_READ_COUNT_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_LAYER_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("TOTAL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_RID,
			                               int? LAYER_ID
			                               )
			    {
                    lock (typeof(MID_TOTAL_RULE_LAYER_READ_COUNT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_LAYER_READ_COUNT_def MID_DETAIL_RULE_LAYER_READ_COUNT = new MID_DETAIL_RULE_LAYER_READ_COUNT_def();
			public class MID_DETAIL_RULE_LAYER_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_LAYER_READ_COUNT.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_DETAIL_RULE_LAYER_READ_COUNT_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_LAYER_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("DETAIL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_RID,
			                               int? LAYER_ID
			                               )
			    {
                    lock (typeof(MID_DETAIL_RULE_LAYER_READ_COUNT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_LAYER_READ_COUNT_def MID_BULK_COLOR_RULE_LAYER_READ_COUNT = new MID_BULK_COLOR_RULE_LAYER_READ_COUNT_def();
			public class MID_BULK_COLOR_RULE_LAYER_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_LAYER_READ_COUNT.SQL"

                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_COLOR_RULE_LAYER_READ_COUNT_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_LAYER_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("BULK_COLOR_RULE_LAYER");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_BC_RID,
			                               int? LAYER_ID
			                               )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_LAYER_READ_COUNT_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_LAYER_READ_COUNT_def MID_PACK_RULE_LAYER_READ_COUNT = new MID_PACK_RULE_LAYER_READ_COUNT_def();
			public class MID_PACK_RULE_LAYER_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_LAYER_READ_COUNT.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
			
			    public MID_PACK_RULE_LAYER_READ_COUNT_def()
			    {
			        base.procedureName = "MID_PACK_RULE_LAYER_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("PACK_RULE_LAYER");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, 
			                               int? HDR_PACK_RID,
			                               int? LAYER_ID
			                               )
			    {
                    lock (typeof(MID_PACK_RULE_LAYER_READ_COUNT_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_LAYER_UPDATE_def MID_BULK_RULE_LAYER_UPDATE = new MID_BULK_RULE_LAYER_UPDATE_def();
			public class MID_BULK_RULE_LAYER_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_LAYER_UPDATE.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter METHOD_RID;
			
			    public MID_BULK_RULE_LAYER_UPDATE_def()
			    {
			        base.procedureName = "MID_BULK_RULE_LAYER_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("BULK_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_BULK_RULE_LAYER_UPDATE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_LAYER_UPDATE_def MID_TOTAL_RULE_LAYER_UPDATE = new MID_TOTAL_RULE_LAYER_UPDATE_def();
			public class MID_TOTAL_RULE_LAYER_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_LAYER_UPDATE.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter METHOD_RID;
			
			    public MID_TOTAL_RULE_LAYER_UPDATE_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_LAYER_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("TOTAL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_TOTAL_RULE_LAYER_UPDATE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_LAYER_UPDATE_def MID_DETAIL_RULE_LAYER_UPDATE = new MID_DETAIL_RULE_LAYER_UPDATE_def();
			public class MID_DETAIL_RULE_LAYER_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_LAYER_UPDATE.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter METHOD_RID;
			
			    public MID_DETAIL_RULE_LAYER_UPDATE_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_LAYER_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("DETAIL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_DETAIL_RULE_LAYER_UPDATE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_LAYER_UPDATE_def MID_BULK_COLOR_RULE_LAYER_UPDATE = new MID_BULK_COLOR_RULE_LAYER_UPDATE_def();
			public class MID_BULK_COLOR_RULE_LAYER_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_LAYER_UPDATE.SQL"

                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
                private intParameter METHOD_RID;
			
			    public MID_BULK_COLOR_RULE_LAYER_UPDATE_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_LAYER_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("BULK_COLOR_RULE_LAYER");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_BC_RID,
			                      int? LAYER_ID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_LAYER_UPDATE_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_LAYER_UPDATE_def MID_PACK_RULE_LAYER_UPDATE = new MID_PACK_RULE_LAYER_UPDATE_def();
			public class MID_PACK_RULE_LAYER_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_LAYER_UPDATE.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
                private intParameter METHOD_RID;
			
			    public MID_PACK_RULE_LAYER_UPDATE_def()
			    {
			        base.procedureName = "MID_PACK_RULE_LAYER_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("PACK_RULE_LAYER");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? HDR_PACK_RID,
			                      int? LAYER_ID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_PACK_RULE_LAYER_UPDATE_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_LAYER_INSERT_def MID_PACK_RULE_LAYER_INSERT = new MID_PACK_RULE_LAYER_INSERT_def();
			public class MID_PACK_RULE_LAYER_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_LAYER_INSERT.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
                private intParameter METHOD_RID;
			
			    public MID_PACK_RULE_LAYER_INSERT_def()
			    {
			        base.procedureName = "MID_PACK_RULE_LAYER_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("PACK_RULE_LAYER");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_PACK_RID,
			                      int? LAYER_ID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_PACK_RULE_LAYER_INSERT_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_LAYER_INSERT_def MID_BULK_COLOR_RULE_LAYER_INSERT = new MID_BULK_COLOR_RULE_LAYER_INSERT_def();
			public class MID_BULK_COLOR_RULE_LAYER_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_LAYER_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
                private intParameter METHOD_RID;
			
			    public MID_BULK_COLOR_RULE_LAYER_INSERT_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_LAYER_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BULK_COLOR_RULE_LAYER");
                    HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
                                  int? HDR_RID,
			                      int? HDR_BC_RID,
			                      int? LAYER_ID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_LAYER_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_LAYER_INSERT_def MID_DETAIL_RULE_LAYER_INSERT = new MID_DETAIL_RULE_LAYER_INSERT_def();
			public class MID_DETAIL_RULE_LAYER_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_LAYER_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter METHOD_RID;
			
			    public MID_DETAIL_RULE_LAYER_INSERT_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_LAYER_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("DETAIL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_DETAIL_RULE_LAYER_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_LAYER_INSERT_def MID_TOTAL_RULE_LAYER_INSERT = new MID_TOTAL_RULE_LAYER_INSERT_def();
			public class MID_TOTAL_RULE_LAYER_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_LAYER_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter METHOD_RID;
			
			    public MID_TOTAL_RULE_LAYER_INSERT_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_LAYER_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TOTAL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_TOTAL_RULE_LAYER_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_LAYER_INSERT_def MID_BULK_RULE_LAYER_INSERT = new MID_BULK_RULE_LAYER_INSERT_def();
			public class MID_BULK_RULE_LAYER_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_LAYER_INSERT.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
                private intParameter METHOD_RID;
			
			    public MID_BULK_RULE_LAYER_INSERT_def()
			    {
			        base.procedureName = "MID_BULK_RULE_LAYER_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("BULK_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? LAYER_ID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_BULK_RULE_LAYER_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_LAYER_READ_def MID_TOTAL_RULE_LAYER_READ = new MID_TOTAL_RULE_LAYER_READ_def();
			public class MID_TOTAL_RULE_LAYER_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_LAYER_READ.SQL"

                private intParameter HDR_RID;
			
			    public MID_TOTAL_RULE_LAYER_READ_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_LAYER_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TOTAL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_TOTAL_RULE_LAYER_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_LAYER_READ_def MID_BULK_RULE_LAYER_READ = new MID_BULK_RULE_LAYER_READ_def();
			public class MID_BULK_RULE_LAYER_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_LAYER_READ.SQL"

                private intParameter HDR_RID;
			
			    public MID_BULK_RULE_LAYER_READ_def()
			    {
			        base.procedureName = "MID_BULK_RULE_LAYER_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("BULK_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_BULK_RULE_LAYER_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_LAYER_READ_def MID_DETAIL_RULE_LAYER_READ = new MID_DETAIL_RULE_LAYER_READ_def();
			public class MID_DETAIL_RULE_LAYER_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_LAYER_READ.SQL"

                private intParameter HDR_RID;
			
			    public MID_DETAIL_RULE_LAYER_READ_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_LAYER_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("DETAIL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_DETAIL_RULE_LAYER_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_LAYER_READ_def MID_BULK_COLOR_RULE_LAYER_READ = new MID_BULK_COLOR_RULE_LAYER_READ_def();
			public class MID_BULK_COLOR_RULE_LAYER_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_LAYER_READ.SQL"

                private intParameter HDR_BC_RID;
			
			    public MID_BULK_COLOR_RULE_LAYER_READ_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_LAYER_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("BULK_COLOR_RULE_LAYER");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_BC_RID)
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_LAYER_READ_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_LAYER_READ_def MID_PACK_RULE_LAYER_READ = new MID_PACK_RULE_LAYER_READ_def();
			public class MID_PACK_RULE_LAYER_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_LAYER_READ.SQL"

                private intParameter HDR_PACK_RID;
			
			    public MID_PACK_RULE_LAYER_READ_def()
			    {
			        base.procedureName = "MID_PACK_RULE_LAYER_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PACK_RULE_LAYER");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_PACK_RID)
			    {
                    lock (typeof(MID_PACK_RULE_LAYER_READ_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_LAYER_READ_INFO_def MID_BULK_RULE_LAYER_READ_INFO = new MID_BULK_RULE_LAYER_READ_INFO_def();
			public class MID_BULK_RULE_LAYER_READ_INFO_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_LAYER_READ_INFO.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_RULE_LAYER_READ_INFO_def()
			    {
			        base.procedureName = "MID_BULK_RULE_LAYER_READ_INFO";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("BULK_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HDR_RID,
			                          int? LAYER_ID
			                          )
			    {
                    lock (typeof(MID_BULK_RULE_LAYER_READ_INFO_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_LAYER_READ_INFO_def MID_TOTAL_RULE_LAYER_READ_INFO = new MID_TOTAL_RULE_LAYER_READ_INFO_def();
			public class MID_TOTAL_RULE_LAYER_READ_INFO_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_LAYER_READ_INFO.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_TOTAL_RULE_LAYER_READ_INFO_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_LAYER_READ_INFO";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TOTAL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HDR_RID,
			                          int? LAYER_ID
			                          )
			    {
                    lock (typeof(MID_TOTAL_RULE_LAYER_READ_INFO_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_LAYER_READ_INFO_def MID_DETAIL_RULE_LAYER_READ_INFO = new MID_DETAIL_RULE_LAYER_READ_INFO_def();
			public class MID_DETAIL_RULE_LAYER_READ_INFO_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_LAYER_READ_INFO.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_DETAIL_RULE_LAYER_READ_INFO_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_LAYER_READ_INFO";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("DETAIL_RULE_LAYER");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HDR_RID,
			                          int? LAYER_ID
			                          )
			    {
                    lock (typeof(MID_DETAIL_RULE_LAYER_READ_INFO_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_LAYER_READ_INFO_def MID_BULK_COLOR_RULE_LAYER_READ_INFO = new MID_BULK_COLOR_RULE_LAYER_READ_INFO_def();
			public class MID_BULK_COLOR_RULE_LAYER_READ_INFO_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_LAYER_READ_INFO.SQL"

                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_COLOR_RULE_LAYER_READ_INFO_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_LAYER_READ_INFO";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("BULK_COLOR_RULE_LAYER");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HDR_BC_RID,
			                          int? LAYER_ID
			                          )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_LAYER_READ_INFO_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_LAYER_READ_INFO_def MID_PACK_RULE_LAYER_READ_INFO = new MID_PACK_RULE_LAYER_READ_INFO_def();
			public class MID_PACK_RULE_LAYER_READ_INFO_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_LAYER_READ_INFO.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
			
			    public MID_PACK_RULE_LAYER_READ_INFO_def()
			    {
			        base.procedureName = "MID_PACK_RULE_LAYER_READ_INFO";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PACK_RULE_LAYER");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HDR_PACK_RID,
			                          int? LAYER_ID
			                          )
			    {
                    lock (typeof(MID_PACK_RULE_LAYER_READ_INFO_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_BULK_RULE_READ_def MID_BULK_RULE_READ = new MID_BULK_RULE_READ_def();
			public class MID_BULK_RULE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_RULE_READ.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_RULE_READ_def()
			    {
			        base.procedureName = "MID_BULK_RULE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("BULK_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HDR_RID,
			                          int? LAYER_ID
			                          )
			    {
                    lock (typeof(MID_BULK_RULE_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TOTAL_RULE_READ_def MID_TOTAL_RULE_READ = new MID_TOTAL_RULE_READ_def();
			public class MID_TOTAL_RULE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TOTAL_RULE_READ.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_TOTAL_RULE_READ_def()
			    {
			        base.procedureName = "MID_TOTAL_RULE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TOTAL_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HDR_RID,
			                          int? LAYER_ID
			                          )
			    {
                    lock (typeof(MID_TOTAL_RULE_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_DETAIL_RULE_READ_def MID_DETAIL_RULE_READ = new MID_DETAIL_RULE_READ_def();
			public class MID_DETAIL_RULE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_DETAIL_RULE_READ.SQL"

                private intParameter HDR_RID;
                private intParameter LAYER_ID;
			
			    public MID_DETAIL_RULE_READ_def()
			    {
			        base.procedureName = "MID_DETAIL_RULE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("DETAIL_RULE");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HDR_RID,
			                          int? LAYER_ID
			                          )
			    {
                    lock (typeof(MID_DETAIL_RULE_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_BULK_COLOR_RULE_READ_def MID_BULK_COLOR_RULE_READ = new MID_BULK_COLOR_RULE_READ_def();
			public class MID_BULK_COLOR_RULE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_BULK_COLOR_RULE_READ.SQL"

                private intParameter HDR_BC_RID;
                private intParameter LAYER_ID;
			
			    public MID_BULK_COLOR_RULE_READ_def()
			    {
			        base.procedureName = "MID_BULK_COLOR_RULE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("BULK_COLOR_RULE");
			        HDR_BC_RID = new intParameter("@HDR_BC_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HDR_BC_RID,
			                          int? LAYER_ID
			                          )
			    {
                    lock (typeof(MID_BULK_COLOR_RULE_READ_def))
                    {
                        this.HDR_BC_RID.SetValue(HDR_BC_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_PACK_RULE_READ_def MID_PACK_RULE_READ = new MID_PACK_RULE_READ_def();
			public class MID_PACK_RULE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_PACK_RULE_READ.SQL"

                private intParameter HDR_PACK_RID;
                private intParameter LAYER_ID;
			
			    public MID_PACK_RULE_READ_def()
			    {
			        base.procedureName = "MID_PACK_RULE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("PACK_RULE");
			        HDR_PACK_RID = new intParameter("@HDR_PACK_RID", base.inputParameterList);
			        LAYER_ID = new intParameter("@LAYER_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? HDR_PACK_RID,
			                          int? LAYER_ID
			                          )
			    {
                    lock (typeof(MID_PACK_RULE_READ_def))
                    {
                        this.HDR_PACK_RID.SetValue(HDR_PACK_RID);
                        this.LAYER_ID.SetValue(LAYER_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
