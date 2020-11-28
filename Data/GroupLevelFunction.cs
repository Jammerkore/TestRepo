using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for GroupLevelFunction.
	/// </summary>
	public class GroupLevelFunction: MethodBaseData
	{
		private int _method_RID;
		private int _SGL_RID;
		private char _Default_Ind;
		private char _Plan_Ind;
		private char _Use_Default_Ind;
		private char _Clear_Ind;
		private char _Season_Ind;
		private int _Season_HN_RID;
		private int _GLFT_ID;
		private int _GLSB_ID;
		private char _LY_Alt_Ind;
		private char _Trend_Alt_Ind;
		private char _TY_Equalize_Weight;
		private char _LY_Equalize_Weight;
		private char _Apply_Equalize_Weight;
        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        private char _Proj_Curr_Wk_Sales_Ind;
        //END TT#43 - MD - DOConnell - Projected Sales Enhancement
//		public int Method_RID
//		{
//			get	{return _method_RID;}
//			set	{_method_RID = value;	}
//		}

		public int SGL_RID
		{
			get	{return _SGL_RID;}
			set	{_SGL_RID = value;}
		}
		public char Default_Ind
		{
			get
			{if (_Default_Ind == 0)
				 return '0';
			else
				return _Default_Ind;}
			set
			{_Default_Ind = value;	}
		}
		public char Plan_Ind
		{
			get
			{if (_Plan_Ind == 0)
				 return '0';
			 else
				 return _Plan_Ind;}
			set
			{_Plan_Ind = value;	}
		}
		public char Use_Default_Ind
		{
			get
			{if (_Use_Default_Ind == 0)
				 return '0';
			 else
				 return _Use_Default_Ind;}
			set
			{_Use_Default_Ind = value;	}
		}

		public char Clear_Ind
		{
			get
			{if (_Clear_Ind == 0)
				 return '0';
			 else
				 return _Clear_Ind;}
			set
			{_Clear_Ind = value;	}
		}
		public char Season_Ind
		{
			get
			{if (_Season_Ind == 0)
				 return '0';
			 else
				 return _Season_Ind;}
			set
			{_Season_Ind = value;	}
		}

		public int Season_HN_RID
		{
			get	{return _Season_HN_RID;}
			set	{_Season_HN_RID = value;	}
		}

		public int GLFT_ID
		{
			get	{return _GLFT_ID;}
			set	{_GLFT_ID = value;	}
		}

		public int GLSB_ID
		{
			get	{return _GLSB_ID;}
			set	{_GLSB_ID = value;	}
		}
		public char LY_Alt_Ind
		{
			get
			{if (_LY_Alt_Ind == 0)
				 return '0';
			 else
				 return _LY_Alt_Ind;}
			set
			{_LY_Alt_Ind = value;	}
		}
		public char Trend_Alt_Ind
		{
			get
			{if (_Trend_Alt_Ind == 0)
				 return '0';
			 else
				 return _Trend_Alt_Ind;}
			set
			{_Trend_Alt_Ind = value;	}
		}
        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        public char Proj_Curr_Wk_Sales_Ind
        {
            get
            {
                if (_Proj_Curr_Wk_Sales_Ind == 0)
                    return '0';
                else
                    return _Proj_Curr_Wk_Sales_Ind;
            }
            set
            { _Proj_Curr_Wk_Sales_Ind = value; }
        }
        //END TT#43 - MD - DOConnell - Projected Sales Enhancement
		public char TY_Equalize_Weight
		{
			get
			{
				if(_TY_Equalize_Weight == 0)
					return '0';
				else
					return _TY_Equalize_Weight;
			}
			set { _TY_Equalize_Weight = value; }
		}
		public char LY_Equalize_Weight
		{
			get
			{
				if(_LY_Equalize_Weight == 0)
					return '0';
				else
					return _LY_Equalize_Weight;
			}
			set { _LY_Equalize_Weight = value; }
		}
		public char Apply_Equalize_Weight
		{
			get
			{
				if(_Apply_Equalize_Weight == 0)
					return '0';
				else
					return _Apply_Equalize_Weight;
			}
			set { _Apply_Equalize_Weight = value; }
		}
		public GroupLevelFunction()
		{
		}

		//Populate an instance of this class based on the key.
        //public bool PopulateGLF(int method_RID, int sgl_rid)
        //{
        //    try
        //    {	
        //        //StringBuilder SQLCommand = new StringBuilder();
        //        //SQLCommand.AppendFormat("SELECT DEFAULT_IND, PLAN_IND, USE_DEFAULT_IND, CLEAR_IND, "
        //        //    + "COALESCE(SEASON_IND,0) SEASON_IND, COALESCE(SEASON_HN_RID,0) SEASON_HN_RID, GLFT_ID, GLSB_ID, "
        //        //    + "LY_ALT_IND, TREND_ALT_IND, TY_EQUALIZE_WEIGHT_IND, LY_EQUALIZE_WEIGHT_IND, APPLY_EQUALIZE_WEIGHT_IND "
        //        //    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //        //    + "FROM GROUP_LEVEL_FUNCTION WHERE METHOD_RID = {0} "
        //        //    // end MID Track # 2354
        //        //    + "AND SGL_RID = {1}",
        //        //    method_RID,
        //        //    sgl_rid);

        //        DataTable dtGLF = MIDEnvironment.CreateDataTable();
        //        //dtGLF = _dba.ExecuteSQLQuery( SQLCommand.ToString(), "Group_Level_Function" );
        //        dtGLF = StoredProcedures.MID_GROUP_LEVEL_FUNCTION_READ.Read(_dba,
        //                                                                    METHOD_RID: method_RID,
        //                                                                    SGL_RID: sgl_rid
        //                                                                    );

        //        //if(dtOTSPlan != null)
        //        if(dtGLF.Rows.Count != 0)
        //        {
        //            DataRow dr = dtGLF.Rows[0];
        //            _method_RID = method_RID;
        //            _SGL_RID = sgl_rid;
        //            _Default_Ind = Convert.ToChar(dr["DEFAULT_IND"], CultureInfo.CurrentUICulture);
        //            _Plan_Ind = Convert.ToChar(dr["PLAN_IND"], CultureInfo.CurrentUICulture);
        //            _Use_Default_Ind = Convert.ToChar(dr["USE_DEFAULT_IND"], CultureInfo.CurrentUICulture);	
        //            _Clear_Ind = Convert.ToChar(dr["CLEAR_IND"], CultureInfo.CurrentUICulture);
        //            _Season_Ind = Convert.ToChar(dr["SEASON_IND"], CultureInfo.CurrentUICulture);	
        //            _Season_HN_RID = Convert.ToInt32(dr["SEASON_HN_RID"], CultureInfo.CurrentUICulture);
        //            _GLFT_ID = Convert.ToInt32(dr["GLFT_ID"], CultureInfo.CurrentUICulture);
        //            _GLSB_ID = Convert.ToInt32(dr["GLSB_ID"], CultureInfo.CurrentUICulture);
        //            _LY_Alt_Ind = Convert.ToChar(dr["YL_ALT_IND"], CultureInfo.CurrentUICulture);
        //            _Trend_Alt_Ind = Convert.ToChar(dr["TREND_ALT_IND"], CultureInfo.CurrentUICulture);
        //            _TY_Equalize_Weight = Convert.ToChar(dr["TY_EQUALIZE_WEIGHT_IND"], CultureInfo.CurrentUICulture);
        //            _LY_Equalize_Weight = Convert.ToChar(dr["LY_EQUALIZE_WEIGHT_IND"], CultureInfo.CurrentUICulture);
        //            _Apply_Equalize_Weight = Convert.ToChar(dr["APPLY_EQUALIZE_WEIGHT_IND"], CultureInfo.CurrentUICulture);
        //            //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        //            _Proj_Curr_Wk_Sales_Ind = Convert.ToChar(dr["PROJECT_CURR_WEEK_SALES_IND"], CultureInfo.CurrentUICulture);
        //            //END TT#43 - MD - DOConnell - Projected Sales Enhancement
        //            return true;
        //        }
        //        else
        //        {
        //            _SGL_RID = 0;
        //            return false;
        //        }
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}

//		public bool InsertGLF(int method_RID, TransactionData td)
//		{
//			try
//			{
//				StringBuilder SQLCommand = new StringBuilder();
//
//				SQLCommand.Append("INSERT INTO GROUP_LEVEL_FUNCTION(METHOD_RID, ");
//				SQLCommand.Append("SGL_RID, DEFAULT_IND, PLAN_IND, "); 
//				SQLCommand.Append("USE_DEFAULT_IND, CLEAR_IND, SEASON_IND, SEASON_HN_RID, GLFT_ID, GLSB_ID) "); //, GLSB_ID, GLRT_ID) ");	
//				SQLCommand.Append("VALUES (");
//				SQLCommand.Append(_method_RID);
//				SQLCommand.Append(",");
//				SQLCommand.Append(_SGL_RID.ToString());
//				SQLCommand.Append(",'");
//				SQLCommand.Append(_Default_Ind);
//				SQLCommand.Append("','");
//				SQLCommand.Append(_Plan_Ind);
//				SQLCommand.Append("','");
//				SQLCommand.Append(_Use_Default_Ind);
//				SQLCommand.Append("','");
//				SQLCommand.Append(_Clear_Ind);
//				SQLCommand.Append("','");
//				SQLCommand.Append(_Season_Ind);
//				SQLCommand.Append("',");
//				SQLCommand.Append(_Season_HN_RID.ToString());
//				SQLCommand.Append(",");
//				SQLCommand.Append(_GLFT_ID.ToString());
//				SQLCommand.Append(",");
//				SQLCommand.Append(_GLSB_ID.ToString());
//				SQLCommand.Append(")");
//
// 				_dba.OpenUpdateConnection();
//				_dba.ExecuteNonQuery(SQLCommand.ToString());
//	
//				_dba.CommitData();
//				return true;
//			}
//			catch ( Exception err )
//			{
//				return false;
//			}
//		}

		public bool InsertGLF(TransactionData td)
		{
			bool InsertSuccessfull = true;
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();

                //SQLCommand.AppendFormat("INSERT INTO GROUP_LEVEL_FUNCTION "
                //    + "(METHOD_RID, SGL_RID, DEFAULT_IND, PLAN_IND, USE_DEFAULT_IND, CLEAR_IND, SEASON_IND, "
                //    + "SEASON_HN_RID, GLFT_ID, GLSB_ID, LY_ALT_IND, TREND_ALT_IND, TY_EQUALIZE_WEIGHT_IND, "
                //    + "LY_EQUALIZE_WEIGHT_IND, APPLY_EQUALIZE_WEIGHT_IND, PROJECT_CURR_WEEK_SALES_IND) VALUES ("
                //    + "{0}, {1}, '{2}', '{3}', '{4}', '{5}', '{6}', {7}, {8}, {9}, '{10}', '{11}', '{12}', '{13}', '{14}', '{15}')",
                //    Method_RID,
                //    _SGL_RID.ToString(CultureInfo.CurrentUICulture),
                //    _Default_Ind,
                //    _Plan_Ind,
                //    _Use_Default_Ind,
                //    _Clear_Ind,
                //    _Season_Ind,
                //    _Season_HN_RID.ToString(CultureInfo.CurrentUICulture),
                //    _GLFT_ID.ToString(CultureInfo.CurrentUICulture),
                //    _GLSB_ID.ToString(CultureInfo.CurrentUICulture),
                //    _LY_Alt_Ind.ToString(CultureInfo.CurrentUICulture),
                //    _Trend_Alt_Ind.ToString(CultureInfo.CurrentUICulture),
                //    _TY_Equalize_Weight.ToString(CultureInfo.CurrentUICulture),
                //    _LY_Equalize_Weight.ToString(CultureInfo.CurrentUICulture),
                //    _Apply_Equalize_Weight.ToString(CultureInfo.CurrentUICulture),
                ////BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                //    _Proj_Curr_Wk_Sales_Ind.ToString(CultureInfo.CurrentUICulture));
                ////END TT#43 - MD - DOConnell - Projected Sales Enhancement
                //td.DBA.ExecuteNonQuery(SQLCommand.ToString());

                StoredProcedures.MID_GROUP_LEVEL_FUNCTION_INSERT.Insert(td.DBA,
                                                                        METHOD_RID: Method_RID,
                                                                        SGL_RID: _SGL_RID,
                                                                        DEFAULT_IND: _Default_Ind,
                                                                        PLAN_IND: _Plan_Ind,
                                                                        USE_DEFAULT_IND: _Use_Default_Ind,
                                                                        CLEAR_IND: _Clear_Ind,
                                                                        SEASON_IND: _Season_Ind,
                                                                        SEASON_HN_RID: _Season_HN_RID,
                                                                        GLFT_ID: _GLFT_ID,
                                                                        GLSB_ID: _GLSB_ID,
                                                                        LY_ALT_IND: _LY_Alt_Ind,
                                                                        TREND_ALT_IND: _Trend_Alt_Ind,
                                                                        TY_EQUALIZE_WEIGHT_IND: _TY_Equalize_Weight,
                                                                        LY_EQUALIZE_WEIGHT_IND: _LY_Equalize_Weight,
                                                                        APPLY_EQUALIZE_WEIGHT_IND: _Apply_Equalize_Weight,
                                                                        PROJECT_CURR_WEEK_SALES_IND: _Proj_Curr_Wk_Sales_Ind
                                                                        );

				InsertSuccessfull = true;
			}
			catch ( Exception err )
			{
				string exceptionMessage = err.Message;
				InsertSuccessfull = false;
				string message = err.ToString();
				throw;
			}
			return InsertSuccessfull;
		}

//		/// <summary>
//		/// Insert a row in the Group_Level_Function table
//		/// </summary>
//		/// <param name="values"></param>
//		/// <param name="method_RID"></param>
//		/// <returns>boolean depending if Insert completed or failed</returns>
//		public bool InsertGLF(Object [] values, int method_RID, DatabaseAccess dba)
//		{
//			bool InsertSuccessfull = true;
//			try
//			{	
//				StringBuilder SQLCommand = new StringBuilder();
//
//				SQLCommand.Append("INSERT INTO GROUP_LEVEL_FUNCTION(METHOD_RID, ");
//				SQLCommand.Append("SGL_RID, DEFAULT_IND, PLAN_IND, "); 
//				SQLCommand.Append("USE_DEFAULT_IND, CLEAR_IND, SEASON_IND, SEASON_HN_RID, GLFT_ID, GLSB_ID, ");
//				SQLCommand.Append("LY_ALT_IND, TREND_ALT_IND) ");
//				SQLCommand.Append("VALUES (");
//				SQLCommand.Append(method_RID);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[0]);
//				SQLCommand.Append(",'");
//				SQLCommand.Append(values[1]);
//				SQLCommand.Append("','");
//				SQLCommand.Append(values[2]);//
//				SQLCommand.Append("','");
//				SQLCommand.Append(values[3]);//
//				SQLCommand.Append("','");
//				SQLCommand.Append(values[4]);//
//				SQLCommand.Append("','");
//				SQLCommand.Append(values[5]);//
//				SQLCommand.Append("',");
//				SQLCommand.Append(values[6]);//
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[7]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[8]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[9]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[10]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[11]);
//				SQLCommand.Append(",");
//				SQLCommand.Append(values[12]);
//				SQLCommand.Append(")");
//
//				dba.ExecuteNonQuery(SQLCommand.ToString());
//	
//				InsertSuccessfull = true;
//			}
//			catch
//			{
//				InsertSuccessfull = false;
//				throw;
//			}
//			return InsertSuccessfull;
//		}

		//To be moved to MethodRelations
        //public bool UpdateGLF(TransactionData td)
        //{
        //    bool UpdateSuccessfull = true;
        //    try
        //    {
        //        StringBuilder SQLCommand = new StringBuilder();

        //        SQLCommand.AppendFormat("UPDATE GROUP_LEVEL_FUNCTION SET DEFAULT_IND = '{0}', "
        //            + "PLAN_IND = '{1}', USE_DEFAULT_IND = '{2}', CLEAR_IND = '{3}', "
        //            + "SEASON_IND = '{4}', SEASON_HN_RID = {5}, GLFT_ID = {6}, GLSB_ID = {7}, "
        //            + "LY_ALT_IND = '{8}', TREND_ALT_IND = '{9}', TY_EQUALIZE_WEIGHT_IND = '{10}', "
        //            + "LY_EQUALIZE_WEIGHT_IND = '{11}', APPLY_EQUALIZE_WEIGHT_IND = '{12}' "
        //            + "WHERE METHOD_RID = {13} "
        //            + "AND SGL_RID = {14} "
        //            //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        //            + "AND PROJECT_CURR_WEEK_SALES_IND = {15}",
        //            //END TT#43 - MD - DOConnell - Projected Sales Enhancement
        //            _Default_Ind.ToString(CultureInfo.CurrentUICulture),
        //            _Plan_Ind.ToString(CultureInfo.CurrentUICulture),
        //            _Use_Default_Ind.ToString(CultureInfo.CurrentUICulture),
        //            _Clear_Ind.ToString(CultureInfo.CurrentUICulture),
        //            _Season_Ind.ToString(CultureInfo.CurrentUICulture),
        //            _Season_HN_RID.ToString(CultureInfo.CurrentUICulture),
        //            _GLFT_ID.ToString(CultureInfo.CurrentUICulture),
        //            _GLSB_ID.ToString(CultureInfo.CurrentUICulture),
        //            _LY_Alt_Ind.ToString(CultureInfo.CurrentUICulture),
        //            _Trend_Alt_Ind.ToString(CultureInfo.CurrentUICulture),
        //            _TY_Equalize_Weight.ToString(CultureInfo.CurrentUICulture),
        //            _LY_Equalize_Weight.ToString(CultureInfo.CurrentUICulture),
        //            _Apply_Equalize_Weight.ToString(CultureInfo.CurrentUICulture),
        //            _method_RID,
        //            _SGL_RID,
        //        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        //            _Proj_Curr_Wk_Sales_Ind.ToString(CultureInfo.CurrentUICulture));
        //        //END TT#43 - MD - DOConnell - Projected Sales Enhancement
        //        //td.OpenUpdateConnection();
        //        td.DBA.ExecuteNonQuery(SQLCommand.ToString());
	
        //        //td.CommitData();
        //        UpdateSuccessfull = true;
        //    }
        //    catch ( Exception err )
        //    {
        //        string exceptionMessage = err.Message;
        //        UpdateSuccessfull = false;
        //        string message = err.ToString();
        //        throw;
        //    }
        //    return UpdateSuccessfull;
        //}

//		public int GetSGL_RIDForGL(int method_RID, int SG_RID)
//		{
//			try
//			{
//				StringBuilder SQLCommand = new StringBuilder();
//
//				SQLCommand.Append("SELECT NVL(MIN(SGL_RID),0) ");
//				SQLCommand.Append("FROM GROUP_LEVEL_FUNCTION WHERE METHOD_RID = ");
//				SQLCommand.Append(method_RID);
//				SQLCommand.Append(" AND ");
//				SQLCommand.Append("SGL_RID IN ");
//				SQLCommand.Append("(SELECT SGL_RID FROM STORE_GROUP_LEVEL SGL ");
//				SQLCommand.Append("INNER JOIN STORE_GROUP SG ON SGL.SG_RID = SG.SG_RID ");
//				SQLCommand.Append("WHERE SGL.SG_RID = ");
//				SQLCommand.Append(SG_RID);
//				SQLCommand.Append(")");
//
//				return Convert.ToInt32(_dba.ExecuteScalar(SQLCommand.ToString()));
//			}
//			catch (Exception err)
//			{
//				throw;
//			}
//
//		}

		//To be moved to MethodRelations
		public bool DeleteGLFCascade(int method_RID, int SG_RID, TransactionData td)
		{
			bool DeleteSuccessfull = true;
			try
			{
				// GROUP_LEVEL_BASIS
                StoredProcedures.MID_GROUP_LEVEL_BASIS_DELETE_FROM_GROUP.Delete(td.DBA, METHOD_RID: method_RID);

				// TREND_CAPS
                StoredProcedures.MID_TREND_CAPS_DELETE_FROM_GROUP.Delete(td.DBA, METHOD_RID: method_RID);           

				// STOCK_MIN_MAX
                StoredProcedures.MID_STOCK_MIN_MAX_DELETE_FROM_GROUP.Delete(td.DBA, METHOD_RID: method_RID);

				// GROUP_LEVEL_NODE_FUNCTION
                StoredProcedures.MID_GROUP_LEVEL_NODE_FUNCTION_DELETE_FROM_GROUP.Delete(td.DBA, METHOD_RID: method_RID);

				// GROUP_LEVEL_FUNCTION
                StoredProcedures.MID_GROUP_LEVEL_FUNCTION_DELETE_FROM_GROUP.Delete(td.DBA, METHOD_RID: method_RID);

				DeleteSuccessfull = true;
			}
			catch
			{
				DeleteSuccessfull = false;
				throw;
			}
			return DeleteSuccessfull;
		}

        //public int GetDefaultByStoreGroup(int method_RID, int SGL_RID)
        //{
        //    try
        //    {
        //        StringBuilder SQLCommand = new StringBuilder();

        //        // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //        SQLCommand.AppendFormat("SELECT GLF.SGL_RID AS SGL_RID "
        //            + "FROM GROUP_LEVEL_FUNCTION GLF INNER JOIN STORE_GROUP_LEVEL SGL ON GLF.SGL_RID = SGL.SGL_RID "
        //            + "WHERE SGL.SGL_RID IN "
        //                + "(SELECT SGL_RID FROM STORE_GROUP_LEVEL WHERE  "
        //                + "SG_RID = (SELECT SG_RID FROM STORE_GROUP_LEVEL WHERE SGL_RID = {1})) "
        //            + "AND METHOD_RID = {1} "
        //            + "AND DEFAULT_IND = '1'",
        //            SGL_RID,
        //            method_RID);
        //        // end MID Track # 2354

        //        return Convert.ToInt32(_dba.ExecuteScalar(SQLCommand.ToString()), CultureInfo.CurrentUICulture);
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }

        //}

		/// <summary>
		/// Returns all Group Level Functions for the method data.
		/// The Default is returned first, followed by the functions that use
		/// the default, and the remaining functions follow. 
		/// </summary>
		/// <returns></returns>
		public DataTable GetAllGroupLevelFunctions(int method_RID)
		{
			try
			{
				DataTable dtGLFunction = MIDEnvironment.CreateDataTable();
		      
                //// BEGIN Issue 5028 stodd  (merged in)
                //StringBuilder SQLCommand = new StringBuilder();		
                //SQLCommand.Append("SELECT glf.SGL_RID, SGL_SEQUENCE, COALESCE(DEFAULT_IND,'0') DEFAULT_IND, ");
                //SQLCommand.Append("COALESCE(PLAN_IND,'0') PLAN_IND, ");
                //SQLCommand.Append("COALESCE(USE_DEFAULT_IND,'0') USE_DEFAULT_IND, ");
                //SQLCommand.Append("COALESCE(CLEAR_IND,'0') CLEAR_IND, ");
                //SQLCommand.Append("COALESCE(SEASON_IND,'0') SEASON_IND, ");
                //SQLCommand.Append("COALESCE(SEASON_HN_RID,0) SEASON_HN_RID, GLFT_ID, GLSB_ID, ");
                //SQLCommand.Append("COALESCE(LY_ALT_IND,'0') LY_ALT_IND, ");
                //SQLCommand.Append("COALESCE(TREND_ALT_IND,'0') TREND_ALT_IND,  ");
                //SQLCommand.Append("COALESCE(TY_EQUALIZE_WEIGHT_IND,'0') TY_EQUALIZE_WEIGHT_IND,  ");
                //SQLCommand.Append("COALESCE(LY_EQUALIZE_WEIGHT_IND,'0') LY_EQUALIZE_WEIGHT_IND,  ");
                //SQLCommand.Append("COALESCE(APPLY_EQUALIZE_WEIGHT_IND,'0') APPLY_EQUALIZE_WEIGHT_IND,  ");
                ////BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                //SQLCommand.Append("COALESCE(PROJECT_CURR_WEEK_SALES_IND,'0') PROJECT_CURR_WEEK_SALES_IND  ");
                ////END TT#43 - MD - DOConnell - Projected Sales Enhancement
                //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                //SQLCommand.Append("FROM GROUP_LEVEL_FUNCTION glf, STORE_GROUP_LEVEL sgl ");
                //// end MID Track # 2354
                //SQLCommand.Append("WHERE ");
                //SQLCommand.Append("METHOD_RID = ");
                //SQLCommand.Append(method_RID);
                //SQLCommand.Append(" and glf.SGL_RID = sgl.SGL_RID ");
                //SQLCommand.Append(" order by SGL_SEQUENCE, DEFAULT_IND desc, USE_DEFAULT_IND desc ");
                //// End Issue 5028

                //dtGLFunction = _dba.ExecuteQuery(SQLCommand.ToString());
                dtGLFunction = StoredProcedures.MID_GROUP_LEVEL_FUNCTION_READ_ALL.Read(_dba, METHOD_RID: method_RID);

				return dtGLFunction;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetGroupLevelFunctionsByNode(int aNodeRID)
		{
			try
			{
                //StringBuilder SQLCommand = new StringBuilder();		
                //SQLCommand.AppendFormat("SELECT glf.METHOD_RID, m.METHOD_NAME, m.METHOD_TYPE_ID, "
                //    + "m.USER_RID, au.USER_NAME "
                //    + "FROM GROUP_LEVEL_FUNCTION glf, METHOD m, APPLICATION_USER au "
                //    + "WHERE glf.SEASON_HN_RID = {0} "
                //    + "AND glf.METHOD_RID = m.METHOD_RID "
                //    + "AND m.USER_RID = au.USER_RID",
                //    aNodeRID);

                //return _dba.ExecuteQuery(SQLCommand.ToString());
                return StoredProcedures.MID_GROUP_LEVEL_FUNCTION_READ_FROM_NODE.Read(_dba, SEASON_HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
	}

	
}
