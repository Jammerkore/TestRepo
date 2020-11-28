using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class IMO_Data : DataLayer
    {
        protected static class StoredProcedures
        {

            public static SP_MID_UPDATE_IMO_def SP_MID_UPDATE_IMO = new SP_MID_UPDATE_IMO_def();
			public class SP_MID_UPDATE_IMO_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_UPDATE_IMO.SQL"

			    private textParameter xml;
                private intParameter debug;
                private intParameter ReturnCode; //Declare Output Parameter

                public SP_MID_UPDATE_IMO_def()
			    {
                    base.procedureName = "SP_MID_UPDATE_IMO";
			        base.procedureType = storedProcedureTypes.UpdateWithReturnCode;
                    base.tableNames.Add("STORE_IMO");
                    xml = new textParameter("@xml", base.inputParameterList);
                    debug = new intParameter("@debug", base.inputParameterList);
                    ReturnCode = new intParameter("@ReturnCode", base.outputParameterList); //Add Output Parameter
			    }

                public int UpdateWithReturnCode(DatabaseAccess _dba,
                                                string xml,
                                                int? debug
			                                    )
			    {
                    lock (typeof(SP_MID_UPDATE_IMO_def))
                    {
                        this.xml.SetValue(xml);
                        this.debug.SetValue(debug);
                        this.ReturnCode.SetValue(-100); //Initialize Output Parameter //TT#1309-MD -jsobek -Cancel  Allocation results in Action failed
                        ExecuteStoredProcedureForUpdate(_dba);
                        return Convert.ToInt32(ReturnCode.Value); //TT#1309-MD -jsobek -Cancel  Allocation results in Action failed
                    }
			    }
			}

            public static SP_MID_GET_IMO_def SP_MID_GET_IMO = new SP_MID_GET_IMO_def();
			public class SP_MID_GET_IMO_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_GET_IMO.SQL"

                private tableParameter HN_List;
                private intParameter debug;
                private datetimeParameter FlashBack; //Declare Output Parameter
                private intParameter Return_Code; //Declare Output Parameter

                public SP_MID_GET_IMO_def()
			    {
                    base.procedureName = "SP_MID_GET_IMO";
			        base.procedureType = storedProcedureTypes.OutputOnly;
			        base.tableNames.Add("GET_IMO");
                    HN_List = new tableParameter("@HN_List", "GET_IMO_HN_TYPE", base.inputParameterList);
                    debug = new intParameter("@debug", base.inputParameterList);
                    FlashBack = new datetimeParameter("@FlashBack", base.outputParameterList); //Add Output Parameter
                    Return_Code = new intParameter("@Return_Code", base.outputParameterList); //Add Output Parameter
			    }
			
			    public DataTable ReadValues(DatabaseAccess _dba,
                                      ref int returnCode,
                                      ref DateTime flashBack,
                                      DataTable HN_List,
                                      DateTime? flashBackDefaultValue
			                          )
			    {
                    lock (typeof(SP_MID_GET_IMO_def))
                    {
                        this.HN_List.SetValue(HN_List);
                        this.debug.SetValue(0);
                        this.FlashBack.SetValue(flashBackDefaultValue); //Initialize Output Parameter
                        this.Return_Code.SetValue(null); //Initialize Output Parameter



                        DataTable dt = ExecuteStoredProcedureForRead(_dba);
                        returnCode = (int)Return_Code.Value;
                        flashBack = (DateTime)FlashBack.Value;
                        return dt;
                       
                    }
			    }
			}

			public static MID_STORE_IMO_REV_DELETE_def MID_STORE_IMO_REV_DELETE = new MID_STORE_IMO_REV_DELETE_def();
			public class MID_STORE_IMO_REV_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_STORE_IMO_REV_DELETE.SQL"

			    private intParameter COMMIT_LIMIT;
			
			    public MID_STORE_IMO_REV_DELETE_def()
			    {
			        base.procedureName = "MID_STORE_IMO_REV_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("STORE_IMO_REV");
			        COMMIT_LIMIT = new intParameter("@COMMIT_LIMIT", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? COMMIT_LIMIT)
			    {
                    lock (typeof(MID_STORE_IMO_REV_DELETE_def))
                    {
                        this.COMMIT_LIMIT.SetValue(COMMIT_LIMIT);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
