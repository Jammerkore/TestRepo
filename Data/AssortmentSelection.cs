using System;
using System.Diagnostics;
using System.Collections;
using System.Data;
using System.Text;
using MIDRetail.DataCommon;
using System.ComponentModel;
using System.IO;
using System.Globalization;

namespace MIDRetail.Data
{
	public partial class AssortmentSelection : DataLayer
	{
		private int _userRid;
		private int _sgRid;
		private int _groupBy;
		private int _viewRid;
		private eAssortmentVariableType _variableType;
		private int _variableNumber;
		private bool _inclOnhand;
		private bool _iclIntransit;
		private bool _inclSimStores;
		private bool _inclCommitted;	
		private eStoreAverageBy _averageBy;
		private eGradeBoundary _gradeBoundary;
		private DataTable _dtBasis;
		private DataTable _dtStoreGrade;

		//===============
		// Properties
		//==============
		public int UserRid
		{
			get { return _userRid; }
			set { _userRid = value; }
		}
		public int StoreAttributeRid
		{
			get { return _sgRid; }
			set { _sgRid = value; }
		}
		public int GroupBy
		{
			get { return _groupBy; }
			set { _groupBy = value; }
		}
		public int ViewRid
		{
			get { return _viewRid; }
			set { _viewRid = value; }
		}
		public eAssortmentVariableType VariableType
		{
			get { return _variableType; }
			set { _variableType = value; }
		}
		public int VariableNumber
		{
			get { return _variableNumber; }
			set { _variableNumber = value; }
		}
		public bool IncludeOnhand
		{
			get { return _inclOnhand; }
			set { _inclOnhand = value; }
		}
		public bool IncludeIntransit
		{
			get { return _iclIntransit; }
			set { _iclIntransit = value; }
		}
		public bool IncludeSimStore
		{
			get { return _inclSimStores; }
			set { _inclSimStores = value; }
		}
		public bool IncludeCommitted
		{
			get { return _inclCommitted; }
			set { _inclCommitted = value; }
		}
		public eStoreAverageBy AverageBy
		{
			get { return _averageBy; }
			set { _averageBy = value; }
		}
		public eGradeBoundary GradeBoundary
		{
			get { return _gradeBoundary; }
			set { _gradeBoundary = value; }
		}
		public DataTable BasisDataTable
		{
			get { return _dtBasis; }
			set { _dtBasis = value; }
		}
		public DataTable StoreGradeDataTable
		{
			get { return _dtStoreGrade; }
			set { _dtStoreGrade = value; }
		}

		//===============
		// Constructors
		//==============

		public AssortmentSelection() : base()
		{
			_userRid = Include.UndefinedUserRID;
			Initialize();
		}
        public AssortmentSelection(int aUserID) : base()
		{
			_userRid = aUserID;
			ReadUserAssortmentDefaults();
		}

		//=============
		// Methods
		//=============

		public void Initialize()
		{
			StoreAttributeRid = Include.NoRID;
			GroupBy = 1;
			ViewRid = Include.NoRID;
			VariableType = eAssortmentVariableType.None;
			VariableNumber = Include.NoRID;
			IncludeCommitted = false;
			IncludeIntransit = false;
			IncludeOnhand = false;
			IncludeSimStore = false;
			AverageBy = eStoreAverageBy.None;
			GradeBoundary = eGradeBoundary.Unknown;
			BasisDataTable = MIDEnvironment.CreateDataTable();
			StoreGradeDataTable = MIDEnvironment.CreateDataTable();
		}

		public void ReadUserAssortmentDefaults()
		{
			ReadUserAssortmentDefaults(UserRid);
		}
		public void ReadUserAssortmentDefaults(int userRid)
		{
			UserRid = userRid;
			UserAssortment_Read();
			UserAssortmentBasis_Read();
			UserAssortmentStoreGrade_Read();
		}

		private void UserAssortment_Read()
		{
			DataTable dt;
			try
			{
                dt = StoredProcedures.MID_USER_ASSORTMENT_READ.Read(_dba, USER_RID: UserRid);

				if (dt.Rows.Count == 1)
				{
					UserRid = (Convert.ToInt32(dt.Rows[0]["USER_RID"]));
					StoreAttributeRid = (Convert.ToInt32(dt.Rows[0]["SG_RID"]));
					GroupBy = (Convert.ToInt32(dt.Rows[0]["GROUP_BY_ID"]));
					ViewRid = (Convert.ToInt32(dt.Rows[0]["VIEW_RID"]));
					VariableType = ((eAssortmentVariableType)(Convert.ToInt32(dt.Rows[0]["VARIABLE_TYPE"])));
					VariableNumber = (Convert.ToInt32(dt.Rows[0]["VARIABLE_NUMBER"]));
                    // RMatelic - add if... conditions 
                    if (dt.Rows[0]["INCL_COMMITTED"] != System.DBNull.Value)
                    {
                        IncludeCommitted = Include.ConvertCharToBool(Convert.ToChar(dt.Rows[0]["INCL_COMMITTED"]));
                    }
                    else
                    {
                        IncludeCommitted = false;
                    }
                    if (dt.Rows[0]["INCL_SIMILAR_STORES"] != System.DBNull.Value)
                    { 
                        IncludeSimStore = Include.ConvertCharToBool(Convert.ToChar(dt.Rows[0]["INCL_SIMILAR_STORES"]));
                    }
                    else
                    {
                        IncludeSimStore = false;
                    }
                    if (dt.Rows[0]["INCL_INTRANSIT"] != System.DBNull.Value)
                    {
                        IncludeIntransit = Include.ConvertCharToBool(Convert.ToChar(dt.Rows[0]["INCL_INTRANSIT"]));
                    }
                    else
                    {
                        IncludeIntransit = false;
                    }
                    if (dt.Rows[0]["INCL_ONHAND"] != System.DBNull.Value)
                    {
                        IncludeOnhand = Include.ConvertCharToBool(Convert.ToChar(dt.Rows[0]["INCL_ONHAND"]));
                    }
                    else
                    {
                        IncludeOnhand = false;
                    }
					//
                    AverageBy = ((eStoreAverageBy)(Convert.ToInt32(dt.Rows[0]["AVERAGE_BY"])));
					GradeBoundary = ((eGradeBoundary)(Convert.ToInt32(dt.Rows[0]["GRADE_BOUNDARY_IND"])));
				}
				else
				{
					Initialize();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}

		}

		private void UserAssortmentBasis_Read()
		{
			try
			{
                BasisDataTable = StoredProcedures.MID_USER_ASSORTMENT_BASIS_READ.Read(_dba, USER_RID: UserRid);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void UserAssortmentStoreGrade_Read()
		{
			try
			{
                StoreGradeDataTable = StoredProcedures.MID_USER_ASSORTMENT_STORE_GRADE_READ.Read(_dba, USER_RID: UserRid);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SaveUserAssortmentDefaults()
		{
			try
			{
				_dba.OpenUpdateConnection();


                StoredProcedures.MID_USER_ASSORTMENT_STORE_GRADE_DELETE.Delete(_dba, USER_RID: UserRid);

                StoredProcedures.MID_USER_ASSORTMENT_BASIS_DELETE.Delete(_dba, USER_RID: UserRid);

                StoredProcedures.MID_USER_ASSORTMENT_DELETE.Delete(_dba, USER_RID: UserRid);

				UserAssortment_Insert();
				UserAssortmentBasis_Insert();
				UserAssortmentStoreGrade_Insert();

				_dba.CommitData();
				_dba.CloseUpdateConnection();
			}
			catch
			{
				_dba.RollBack();
				_dba.CloseUpdateConnection();
				throw;
			}
			
		}

		private void UserAssortment_Insert()
		{
			try
			{
				if (VariableType != eAssortmentVariableType.None)
				{
                    StoredProcedures.MID_USER_ASSORTMENT_INSERT.Insert(_dba,
                                                                   USER_RID: UserRid,
                                                                   SG_RID: StoreAttributeRid,
                                                                   GROUP_BY_ID: GroupBy,
                                                                   VIEW_RID: ViewRid,
                                                                   VARIABLE_TYPE: (int)VariableType,
                                                                   VARIABLE_NUMBER: VariableNumber,
                                                                   INCL_ONHAND: Include.ConvertBoolToChar(IncludeOnhand),
                                                                   INCL_INTRANSIT: Include.ConvertBoolToChar(IncludeIntransit),
                                                                   INCL_SIMILAR_STORES: Include.ConvertBoolToChar(IncludeSimStore),
                                                                   INCL_COMMITTED: Include.ConvertBoolToChar(IncludeCommitted),
                                                                   AVERAGE_BY: (int)(AverageBy),
                                                                   GRADE_BOUNDARY_IND: (int)(GradeBoundary)
                                                                   );
				}
			}
			catch
			{
				throw;
			}
		}

		private void UserAssortmentBasis_Insert()
		{
			try
			{
				int seq = 1;
				foreach (DataRow aRow in BasisDataTable.Rows)
				{
                    StoredProcedures.MID_USER_ASSORTMENT_BASIS_INSERT.Insert(_dba,
                                                                         USER_RID: UserRid,
                                                                         BASIS_SEQ: seq++,
                                                                         HN_RID: Convert.ToInt32(aRow["HN_RID"]),
                                                                         FV_RID: Convert.ToInt32(aRow["FV_RID"]),
                                                                         CDR_RID: Convert.ToInt32(aRow["CDR_RID"]),
                                                                         WEIGHT: Convert.ToDecimal(aRow["WEIGHT"])
                                                                         );
				}
				
			}
			catch
			{
				throw;
			}
		}

		private void UserAssortmentStoreGrade_Insert()
		{
			try
			{
				int seq = 1;
				foreach (DataRow aRow in StoreGradeDataTable.Rows)
				{
                    StoredProcedures.MID_USER_ASSORTMENT_STORE_GRADE_INSERT.Insert(_dba,
                                                                               USER_RID: UserRid,
                                                                               STORE_GRADE_SEQ: seq++,
                                                                               BOUNDARY_UNITS: Convert.ToInt32(aRow["BOUNDARY_UNITS"]),
                                                                               BOUNDARY_INDEX: Convert.ToInt32(aRow["BOUNDARY_INDEX"]),
                                                                               GRADE_CODE: Convert.ToString(aRow["GRADE_CODE"])
                                                                               );
				}

			}
			catch
			{
				throw;
			}
		}
	}
}
