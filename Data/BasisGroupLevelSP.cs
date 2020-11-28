using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class GroupLevelBasis : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_GROUP_LEVEL_BASIS_DELETE_def MID_GROUP_LEVEL_BASIS_DELETE = new MID_GROUP_LEVEL_BASIS_DELETE_def();
			public class MID_GROUP_LEVEL_BASIS_DELETE_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_BASIS_DELETE.SQL"

			    private intParameter SGL_RID;
			    private intParameter METHOD_RID;
			
			    public MID_GROUP_LEVEL_BASIS_DELETE_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_BASIS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("GROUP_LEVEL_BASIS");
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SGL_RID,
			                      int? METHOD_RID
			                      )
			    {
                    lock (typeof(MID_GROUP_LEVEL_BASIS_DELETE_def))
                    {
                        this.SGL_RID.SetValue(SGL_RID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_BASIS_INSERT_def MID_GROUP_LEVEL_BASIS_INSERT = new MID_GROUP_LEVEL_BASIS_INSERT_def();
			public class MID_GROUP_LEVEL_BASIS_INSERT_def : baseStoredProcedure
			{
			        //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_BASIS_INSERT.SQL"

			    public intParameter METHOD_RID;
			    public intParameter SGL_RID;
			    public intParameter BASIS_SEQ;
			    public intParameter HN_RID;
			    public intParameter FV_RID;
			    public intParameter CDR_RID;
			    public floatParameter WEIGHT;
			    public charParameter INC_EXC_IND;
			    public intParameter TYLY_TYPE_ID;
			    public intParameter MERCH_TYPE;
			    public intParameter MERCH_PH_RID;
			    public intParameter MERCH_PHL_SEQUENCE;
			    public intParameter MERCH_OFFSET;
			
			    public MID_GROUP_LEVEL_BASIS_INSERT_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_BASIS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("GROUP_LEVEL_BASIS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        BASIS_SEQ = new intParameter("@BASIS_SEQ", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        WEIGHT = new floatParameter("@WEIGHT", base.inputParameterList);
			        INC_EXC_IND = new charParameter("@INC_EXC_IND", base.inputParameterList);
			        TYLY_TYPE_ID = new intParameter("@TYLY_TYPE_ID", base.inputParameterList);
			        MERCH_TYPE = new intParameter("@MERCH_TYPE", base.inputParameterList);
			        MERCH_PH_RID = new intParameter("@MERCH_PH_RID", base.inputParameterList);
			        MERCH_PHL_SEQUENCE = new intParameter("@MERCH_PHL_SEQUENCE", base.inputParameterList);
			        MERCH_OFFSET = new intParameter("@MERCH_OFFSET", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, int? METHOD_RID,
			                      int? SGL_RID,
			                      int? BASIS_SEQ,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? CDR_RID,
			                      double? WEIGHT,
			                      char? INC_EXC_IND,
			                      int? TYLY_TYPE_ID,
			                      int? MERCH_TYPE,
			                      int? MERCH_PH_RID,
			                      int? MERCH_PHL_SEQUENCE,
			                      int? MERCH_OFFSET
			                      )
			    {
                    lock (typeof(MID_GROUP_LEVEL_BASIS_INSERT_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.BASIS_SEQ.SetValue(BASIS_SEQ);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.WEIGHT.SetValue(WEIGHT);
                        this.INC_EXC_IND.SetValue(INC_EXC_IND);
                        this.TYLY_TYPE_ID.SetValue(TYLY_TYPE_ID);
                        this.MERCH_TYPE.SetValue(MERCH_TYPE);
                        this.MERCH_PH_RID.SetValue(MERCH_PH_RID);
                        this.MERCH_PHL_SEQUENCE.SetValue(MERCH_PHL_SEQUENCE);
                        this.MERCH_OFFSET.SetValue(MERCH_OFFSET);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_BASIS_READ_def MID_GROUP_LEVEL_BASIS_READ = new MID_GROUP_LEVEL_BASIS_READ_def();
			public class MID_GROUP_LEVEL_BASIS_READ_def : baseStoredProcedure
			{
			        //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_BASIS_READ.SQL"

			    private intParameter METHOD_RID;
			
			    public MID_GROUP_LEVEL_BASIS_READ_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_BASIS_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GROUP_LEVEL_BASIS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_GROUP_LEVEL_BASIS_READ_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_BASIS_READ_FROM_TYLY_TYPE_def MID_GROUP_LEVEL_BASIS_READ_FROM_TYLY_TYPE = new MID_GROUP_LEVEL_BASIS_READ_FROM_TYLY_TYPE_def();
			public class MID_GROUP_LEVEL_BASIS_READ_FROM_TYLY_TYPE_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_BASIS_READ_FROM_TYLY_TYPE.SQL"

			    private intParameter METHOD_RID;
			    private intParameter TYLY_TYPE_ID;
			
			    public MID_GROUP_LEVEL_BASIS_READ_FROM_TYLY_TYPE_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_BASIS_READ_FROM_TYLY_TYPE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GROUP_LEVEL_BASIS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        TYLY_TYPE_ID = new intParameter("@TYLY_TYPE_ID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID,
			                          int? TYLY_TYPE_ID
			                          )
			    {
                    lock (typeof(MID_GROUP_LEVEL_BASIS_READ_FROM_TYLY_TYPE_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.TYLY_TYPE_ID.SetValue(TYLY_TYPE_ID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_BASIS_READ_FROM_STORE_GROUP_LEVEL_def MID_GROUP_LEVEL_BASIS_READ_FROM_STORE_GROUP_LEVEL = new MID_GROUP_LEVEL_BASIS_READ_FROM_STORE_GROUP_LEVEL_def();
			public class MID_GROUP_LEVEL_BASIS_READ_FROM_STORE_GROUP_LEVEL_def : baseStoredProcedure
			{
			        //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_BASIS_READ_FROM_STORE_GROUP_LEVEL.SQL"

			    private intParameter METHOD_RID;
			    private intParameter SGL_RID;
			
			    public MID_GROUP_LEVEL_BASIS_READ_FROM_STORE_GROUP_LEVEL_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_BASIS_READ_FROM_STORE_GROUP_LEVEL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GROUP_LEVEL_BASIS");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID,
			                          int? SGL_RID
			                          )
			    {
                    lock (typeof(MID_GROUP_LEVEL_BASIS_READ_FROM_STORE_GROUP_LEVEL_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_GROUP_LEVEL_BASIS_READ_FROM_NODE_def MID_GROUP_LEVEL_BASIS_READ_FROM_NODE = new MID_GROUP_LEVEL_BASIS_READ_FROM_NODE_def();
			public class MID_GROUP_LEVEL_BASIS_READ_FROM_NODE_def : baseStoredProcedure
			{
			    //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GROUP_LEVEL_BASIS_READ_FROM_NODE.SQL"

			    private intParameter HN_RID;
			
			    public MID_GROUP_LEVEL_BASIS_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_GROUP_LEVEL_BASIS_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("GROUP_LEVEL_BASIS");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_GROUP_LEVEL_BASIS_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE

        }
    }  
}
