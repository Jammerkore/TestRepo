using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class AssortmentDetailData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static SP_MID_XML_ASSRT_MATRIX_WRITE_def SP_MID_XML_ASSRT_MATRIX_WRITE = new SP_MID_XML_ASSRT_MATRIX_WRITE_def();
            public class SP_MID_XML_ASSRT_MATRIX_WRITE_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_XML_ASSRT_MATRIX_WRITE.SQL"

			    private stringParameter XMLDOC;
			
			    public SP_MID_XML_ASSRT_MATRIX_WRITE_def()
			    {
                    base.procedureName = "SP_MID_XML_ASSRT_MATRIX_WRITE";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("XML_ASSRT_MATRIX");
                    XMLDOC = new stringParameter("@xmlDoc", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, string XMLDOC)
			    {
                    lock (typeof(SP_MID_XML_ASSRT_MATRIX_WRITE_def))
                    {
                        this.XMLDOC.SetValue(XMLDOC);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_MATRIX_DETAIL_DELETE_def MID_ASSORTMENT_MATRIX_DETAIL_DELETE = new MID_ASSORTMENT_MATRIX_DETAIL_DELETE_def();
			public class MID_ASSORTMENT_MATRIX_DETAIL_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_MATRIX_DETAIL_DELETE.SQL"

			    private tableParameter HDR_RID_LIST;
			
			    public MID_ASSORTMENT_MATRIX_DETAIL_DELETE_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_MATRIX_DETAIL_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ASSORTMENT_MATRIX_DETAIL");
                    HDR_RID_LIST = new tableParameter("@HDR_RID_LIST", "HDR_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, DataTable HDR_RID_LIST)
			    {
                    lock (typeof(MID_ASSORTMENT_MATRIX_DETAIL_DELETE_def))
                    {
                        this.HDR_RID_LIST.SetValue(HDR_RID_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_STYLE_CLOSED_DELETE_def MID_ASSORTMENT_STYLE_CLOSED_DELETE = new MID_ASSORTMENT_STYLE_CLOSED_DELETE_def();
			public class MID_ASSORTMENT_STYLE_CLOSED_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_STYLE_CLOSED_DELETE.SQL"

			    private tableParameter HDR_RID_LIST;
			
			    public MID_ASSORTMENT_STYLE_CLOSED_DELETE_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_STYLE_CLOSED_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ASSORTMENT_STYLE_CLOSED");
			        HDR_RID_LIST = new tableParameter("@HDR_RID_LIST", "HDR_RID_TYPE", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, DataTable HDR_RID_LIST)
			    {
                    lock (typeof(MID_ASSORTMENT_STYLE_CLOSED_DELETE_def))
                    {
                        this.HDR_RID_LIST.SetValue(HDR_RID_LIST);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_STYLE_CLOSED_READ_HDR_LIST_def MID_ASSORTMENT_STYLE_CLOSED_READ_HDR_LIST = new MID_ASSORTMENT_STYLE_CLOSED_READ_HDR_LIST_def();
			public class MID_ASSORTMENT_STYLE_CLOSED_READ_HDR_LIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_STYLE_CLOSED_READ_HDR_LIST.SQL"

			    private tableParameter HDR_RID_LIST;
			
			    public MID_ASSORTMENT_STYLE_CLOSED_READ_HDR_LIST_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_STYLE_CLOSED_READ_HDR_LIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_STYLE_CLOSED");
			        HDR_RID_LIST = new tableParameter("@HDR_RID_LIST", "HDR_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, DataTable HDR_RID_LIST)
			    {
                    lock (typeof(MID_ASSORTMENT_STYLE_CLOSED_READ_HDR_LIST_def))
                    {
                        this.HDR_RID_LIST.SetValue(HDR_RID_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_STYLE_CLOSED_INSERT_def MID_ASSORTMENT_STYLE_CLOSED_INSERT = new MID_ASSORTMENT_STYLE_CLOSED_INSERT_def();
			public class MID_ASSORTMENT_STYLE_CLOSED_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_STYLE_CLOSED_INSERT.SQL"

			    private intParameter HDR_RID;
			    private intParameter SGL_RID;
			    private intParameter GRADE;
			    private charParameter CLOSED;
			
			    public MID_ASSORTMENT_STYLE_CLOSED_INSERT_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_STYLE_CLOSED_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ASSORTMENT_STYLE_CLOSED");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        SGL_RID = new intParameter("@SGL_RID", base.inputParameterList);
			        GRADE = new intParameter("@GRADE", base.inputParameterList);
			        CLOSED = new charParameter("@CLOSED", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? SGL_RID,
			                      int? GRADE,
			                      char? CLOSED
			                      )
			    {
                    lock (typeof(MID_ASSORTMENT_STYLE_CLOSED_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.SGL_RID.SetValue(SGL_RID);
                        this.GRADE.SetValue(GRADE);
                        this.CLOSED.SetValue(CLOSED);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_STORE_ELIGIBILITY_DELETE_def MID_ASSORTMENT_STORE_ELIGIBILITY_DELETE = new MID_ASSORTMENT_STORE_ELIGIBILITY_DELETE_def();
			public class MID_ASSORTMENT_STORE_ELIGIBILITY_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_STORE_ELIGIBILITY_DELETE.SQL"

			    private intParameter HDR_RID;
			
			    public MID_ASSORTMENT_STORE_ELIGIBILITY_DELETE_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_STORE_ELIGIBILITY_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("ASSORTMENT_STORE_ELIGIBILITY");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_STORE_ELIGIBILITY_DELETE_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_STORE_ELIGIBILITY_INSERT_def MID_ASSORTMENT_STORE_ELIGIBILITY_INSERT = new MID_ASSORTMENT_STORE_ELIGIBILITY_INSERT_def();
			public class MID_ASSORTMENT_STORE_ELIGIBILITY_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_STORE_ELIGIBILITY_INSERT.SQL"

			    private intParameter HDR_RID;
			    private intParameter ST_RID;
			    private charParameter ELIGIBLE;
			
			    public MID_ASSORTMENT_STORE_ELIGIBILITY_INSERT_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_STORE_ELIGIBILITY_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("ASSORTMENT_STORE_ELIGIBILITY");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			        ST_RID = new intParameter("@ST_RID", base.inputParameterList);
			        ELIGIBLE = new charParameter("@ELIGIBLE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? HDR_RID,
			                      int? ST_RID,
			                      char? ELIGIBLE
			                      )
			    {
                    lock (typeof(MID_ASSORTMENT_STORE_ELIGIBILITY_INSERT_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        this.ST_RID.SetValue(ST_RID);
                        this.ELIGIBLE.SetValue(ELIGIBLE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_ASSORTMENT_STORE_ELIGIBILITY_READ_def MID_ASSORTMENT_STORE_ELIGIBILITY_READ = new MID_ASSORTMENT_STORE_ELIGIBILITY_READ_def();
			public class MID_ASSORTMENT_STORE_ELIGIBILITY_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_ASSORTMENT_STORE_ELIGIBILITY_READ.SQL"

			    private intParameter HDR_RID;
			
			    public MID_ASSORTMENT_STORE_ELIGIBILITY_READ_def()
			    {
			        base.procedureName = "MID_ASSORTMENT_STORE_ELIGIBILITY_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("ASSORTMENT_STORE_ELIGIBILITY");
			        HDR_RID = new intParameter("@HDR_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HDR_RID)
			    {
                    lock (typeof(MID_ASSORTMENT_STORE_ELIGIBILITY_READ_def))
                    {
                        this.HDR_RID.SetValue(HDR_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
