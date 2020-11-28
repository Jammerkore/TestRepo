using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class Intransit : DataLayer
    {
        protected static class StoredProcedures
        {
            public static SP_MID_UPDATE_INTRANSIT_def SP_MID_UPDATE_INTRANSIT = new SP_MID_UPDATE_INTRANSIT_def();
            public class SP_MID_UPDATE_INTRANSIT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_UPDATE_INTRANSIT.SQL"

                private textParameter xml;
                private intParameter debug;
                private intParameter ReturnCode; //Declare Output Parameter

                public SP_MID_UPDATE_INTRANSIT_def()
                {
                    base.procedureName = "SP_MID_UPDATE_INTRANSIT";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("STORE_INTRANSIT");
                    xml = new textParameter("@xml", base.inputParameterList);
                    debug = new intParameter("@debug", base.inputParameterList);
                    ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
                }

                public int Update(DatabaseAccess _dba,
                                  string xml,
                                  int? debug
                                  )
                {
                    lock (typeof(SP_MID_UPDATE_INTRANSIT_def))
                    {
                        this.xml.SetValue(xml);
                        this.debug.SetValue(debug);

                        this.ReturnCode.SetValue(0); //Initialize Output Parameter
                        ExecuteStoredProcedureForUpdate(_dba);

                        return (int)this.ReturnCode.Value;
                    }
                }
            }
            public static SP_MID_UPDATE_EXT_INTRANSIT_def SP_MID_UPDATE_EXT_INTRANSIT = new SP_MID_UPDATE_EXT_INTRANSIT_def();
            public class SP_MID_UPDATE_EXT_INTRANSIT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_UPDATE_EXT_INTRANSIT.SQL"

			    private intParameter HN_RID;
			    private intParameter TIME_ID;
			    private intParameter ST_RID;
			    private intParameter VALUE;
			    private charParameter INCREMENT;
			
			    public SP_MID_UPDATE_EXT_INTRANSIT_def()
			    {
                    base.procedureName = "SP_MID_UPDATE_EXT_INTRANSIT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("EXT_INTRANSIT");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        TIME_ID = new intParameter("@TIME_ID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        VALUE = new intParameter("@VALUE", base.inputParameterList);
			        INCREMENT = new charParameter("@INCREMENT", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HN_RID,
			                      int? TIME_ID,
			                      int? ST_RID,
			                      int? VALUE,
			                      char? INCREMENT
			                      )
			    {
                    lock (typeof(SP_MID_UPDATE_EXT_INTRANSIT_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        this.TIME_ID.SetValue(TIME_ID);
                        this.ST_RID.SetValue(ST_RID);
                        this.VALUE.SetValue(VALUE);
                        this.INCREMENT.SetValue(INCREMENT);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_STORE_INTRANSIT_READ_TIME_IDS_def MID_STORE_INTRANSIT_READ_TIME_IDS = new MID_STORE_INTRANSIT_READ_TIME_IDS_def();
			public class MID_STORE_INTRANSIT_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_INTRANSIT_READ_TIME_IDS.SQL"

			    private intParameter HN_RID;
			
			    public MID_STORE_INTRANSIT_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_INTRANSIT_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_INTRANSIT");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_INTRANSIT_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_EXTERNAL_INTRANSIT_READ_TIME_IDS_def MID_STORE_EXTERNAL_INTRANSIT_READ_TIME_IDS = new MID_STORE_EXTERNAL_INTRANSIT_READ_TIME_IDS_def();
			public class MID_STORE_EXTERNAL_INTRANSIT_READ_TIME_IDS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_EXTERNAL_INTRANSIT_READ_TIME_IDS.SQL"

			    private intParameter HN_RID;
			
			    public MID_STORE_EXTERNAL_INTRANSIT_READ_TIME_IDS_def()
			    {
			        base.procedureName = "MID_STORE_EXTERNAL_INTRANSIT_READ_TIME_IDS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("STORE_EXTERNAL_INTRANSIT");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_STORE_EXTERNAL_INTRANSIT_READ_TIME_IDS_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_STORE_EXTERNAL_INTRANSIT_DELETE_def MID_STORE_EXTERNAL_INTRANSIT_DELETE = new MID_STORE_EXTERNAL_INTRANSIT_DELETE_def();
			public class MID_STORE_EXTERNAL_INTRANSIT_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_EXTERNAL_INTRANSIT_DELETE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_EXTERNAL_INTRANSIT_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_EXTERNAL_INTRANSIT_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_EXTERNAL_INTRANSIT");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_EXTERNAL_INTRANSIT_DELETE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_INTRANSIT_REV_DELETE_def MID_STORE_INTRANSIT_REV_DELETE = new MID_STORE_INTRANSIT_REV_DELETE_def();
			public class MID_STORE_INTRANSIT_REV_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_INTRANSIT_REV_DELETE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_INTRANSIT_REV_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_INTRANSIT_REV_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_INTRANSIT_REV");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_INTRANSIT_REV_DELETE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_STORE_INTRANSIT_DELETE_FROM_NODE_LIST_def MID_STORE_INTRANSIT_DELETE_FROM_NODE_LIST = new MID_STORE_INTRANSIT_DELETE_FROM_NODE_LIST_def();
			public class MID_STORE_INTRANSIT_DELETE_FROM_NODE_LIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_INTRANSIT_DELETE_FROM_NODE_LIST.SQL"

			    private tableParameter HN_RID_LIST;
			
			    public MID_STORE_INTRANSIT_DELETE_FROM_NODE_LIST_def()
			    {
			        base.procedureName = "MID_STORE_INTRANSIT_DELETE_FROM_NODE_LIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_INTRANSIT");
                    HN_RID_LIST = new tableParameter("@HN_RID_LIST", "HN_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, DataTable HN_RID_LIST)
			    {
                    lock (typeof(MID_STORE_INTRANSIT_DELETE_FROM_NODE_LIST_def))
                    {
                        this.HN_RID_LIST.SetValue(HN_RID_LIST);
                        base.SetCommandTimeout(0); //0=Unlimited time out
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_GET_INTRANSIT_def MID_GET_INTRANSIT = new MID_GET_INTRANSIT_def();
			public class MID_GET_INTRANSIT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_GET_INTRANSIT.SQL"

			    private tableParameter HN_List;
			    private tableParameter ST_Day_List;
			    private datetimeParameter flashBackIn;
			    private intParameter debug;
			    private datetimeParameter FlashBack; //Declare Output Parameter
			    private intParameter Return_Code; //Declare Output Parameter
			
			    public MID_GET_INTRANSIT_def()
			    {
			        base.procedureName = "MID_GET_INTRANSIT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("VW_STORE_INTRANSIT");
                    HN_List = new tableParameter("@HN_List", "GET_INTRANSIT_HN_TYPE", base.inputParameterList);
			        ST_Day_List = new tableParameter("@ST_Day_List", "GET_INTRANSIT_ST_TYPE", base.inputParameterList);
			        flashBackIn = new datetimeParameter("@flashBackIn", base.inputParameterList);
			        debug = new intParameter("@debug", base.inputParameterList);
			        FlashBack = new datetimeParameter("@FlashBack", base.outputParameterList); //Add Output Parameter
			        Return_Code = new intParameter("@Return_Code", base.outputParameterList); //Add Output Parameter
			    }
			
			    public DataTable ReadValues(DatabaseAccess _dba, 
                                      ref int returnCode,
                                      ref DateTime flashBack,
			                          DataTable HN_List,
			                          DataTable ST_Day_List,
			                          DateTime? flashBackIn
			                          )
			    {
                    lock (typeof(MID_GET_INTRANSIT_def))
                    {
                        this.HN_List.SetValue(HN_List);
                        this.ST_Day_List.SetValue(ST_Day_List);
                        this.flashBackIn.SetValue(flashBackIn);
                        this.debug.SetValue(0);
                        this.FlashBack.SetValue(null); //Initialize Output Parameter
                        this.Return_Code.SetValue(null); //Initialize Output Parameter
                        
                        DataTable dt =  ExecuteStoredProcedureForRead(_dba);
                        returnCode = (int)this.Return_Code.Value;
                        flashBack = (DateTime)this.FlashBack.Value;
                        return dt;
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
