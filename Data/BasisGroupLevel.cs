using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{

	public partial class GroupLevelBasis: DataLayer
	{
		private int _methodRid;
		private int _sglRid;
		private int _seq;
		private int _hierNodeRid;
		private int _versionRid;
		private int _dateRangeRid;
		private double _weight;
		private bool _excludeInd;
		private DataCommon.eTyLyType _tyLyType;
		private eMerchandiseType _merchType = eMerchandiseType.Node;
		private int _merchPhRid = 0;
		private int _merchPhlSequence = 0;
		private int _merchOffset = 0;

		public int MethodRid 
		{
			get { return _methodRid ; }
			set { _methodRid = value; }
		}
		public int SglRid 
		{
			get { return _sglRid ; }
			set { _sglRid = value; }
		}
		public int Sequence 
		{
			get { return _seq ; }
			set { _seq = value; }
		}
		public int HierNodeRid 
		{
			get { return _hierNodeRid ; }
			set { _hierNodeRid = value; }
		}
		public int VersionRid 
		{
			get { return _versionRid ; }
			set { _versionRid = value; }
		}
		public int DateRangeRid 
		{
			get { return _dateRangeRid ; }
			set { _dateRangeRid = value; }
		}
		public double Weight 
		{
			get { return _weight ; }
			set { _weight = value; }
		}
		public bool ExcludeInd 
		{
			get { return _excludeInd ; }
			set { _excludeInd = value; }
		}
		public eTyLyType TyLyType 
		{
			get { return _tyLyType ; }
			set { _tyLyType = value; }
		}
		public eMerchandiseType MerchType 
		{
			get { return _merchType ; }
			set { _merchType = value; }
		}
		public int MerchPhRid 
		{
			get { return _merchPhRid ; }
			set { _merchPhRid = value; }
		}
		public int MerchPhlSequence 
		{
			get { return _merchPhlSequence ; }
			set { _merchPhlSequence = value; }
		}
		public int MerchOffset 
		{
			get { return _merchOffset ; }
			set { _merchOffset = value; }
		}

		public GroupLevelBasis() : base()
		{
            
		}

		/// <summary>
		/// Delete GROUP_LEVEL_BASIS based on METHOD_RID and SGL_RID
		/// </summary>
		/// <param name="method_RID">method_RID - PK of Group Level Function</param>
		/// <param name="sgl_RID">sgl_RID - PK of Group Level Function</param>
		/// <param name="dba">DatabaseAccess connection</param>
		/// <returns>boolean; true if successful, false if failed</returns>
		public bool DeleteGroupLevelBasis(int method_RID, int sgl_RID, TransactionData td)
		{
			bool DeleteSuccessfull = true;
			try
			{
                StoredProcedures.MID_GROUP_LEVEL_BASIS_DELETE.Delete(td.DBA, 
                                                                     SGL_RID: sgl_RID,
                                                                     METHOD_RID: method_RID
                                                                     );

				DeleteSuccessfull = true;
			}
			catch
			{
				DeleteSuccessfull = false;
				throw;
			}
			return DeleteSuccessfull;
		}

		/// <summary>
		/// Save GROUP_LEVEL_BASIS based on METHOD_RID and SGL_RID
		/// </summary>
		/// <param name="dba">DatabaseAccess connection</param>
		/// <returns>boolean; true if successful, false if failed</returns>
		public bool InsertGroupLevelBasis(TransactionData td)
		{
			bool InsertSuccessfull = true;
			try
			{
                int? MERCH_PH_RID_Nullable = null;
                if (_merchType == eMerchandiseType.HierarchyLevel || _merchType == eMerchandiseType.LevelOffset) MERCH_PH_RID_Nullable = _merchPhRid;

                int? MERCH_PHL_SEQUENCE_Nullable = null;
                if (_merchType == eMerchandiseType.HierarchyLevel) MERCH_PHL_SEQUENCE_Nullable = _merchPhlSequence;

                int? MERCH_OFFSET_Nullable = null;
                if (_merchType == eMerchandiseType.LevelOffset) MERCH_OFFSET_Nullable = _merchOffset;

                StoredProcedures.MID_GROUP_LEVEL_BASIS_INSERT.Insert(td.DBA, METHOD_RID: _methodRid,
                                                                     SGL_RID: _sglRid,
                                                                     BASIS_SEQ: _seq,
                                                                     HN_RID: _hierNodeRid,
                                                                     FV_RID: _versionRid,
                                                                     CDR_RID: _dateRangeRid,
                                                                     WEIGHT: _weight,
                                                                     INC_EXC_IND: Include.ConvertBoolToChar(_excludeInd),
                                                                     TYLY_TYPE_ID: Convert.ToInt32(_tyLyType, CultureInfo.CurrentUICulture),
                                                                     MERCH_TYPE: Convert.ToInt32(_merchType, CultureInfo.CurrentUICulture),
                                                                     MERCH_PH_RID: MERCH_PH_RID_Nullable,
                                                                     MERCH_PHL_SEQUENCE: MERCH_PHL_SEQUENCE_Nullable,
                                                                     MERCH_OFFSET: MERCH_OFFSET_Nullable
                                                                     );

				InsertSuccessfull = true;
			}
			catch
			{
				InsertSuccessfull = false;
				throw;
			}
			return InsertSuccessfull;
		}
		
		/// <summary>
		/// Get GROUP_LEVEL_BASIS DataTable based on METHOD_RID - Should have GLF.SGL_RID??
		/// </summary>
		/// <param name="method_RID">method_RID</param>
		/// <returns>DataTable</returns>
		public DataTable GetGroupLevelBasis(int method_RID)
		{
			try
			{	
                return StoredProcedures.MID_GROUP_LEVEL_BASIS_READ.Read(_dba, METHOD_RID: method_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#1044 - JSmith - Apply Trend to TAb is taking an unusual amount of time to apply a basis- this iwill be a problem for ANF
        /// <summary>
		/// Get GROUP_LEVEL_BASIS DataTable based on METHOD_RID - Should have GLF.SGL_RID??
		/// </summary>
		/// <param name="method_RID">method_RID</param>
		/// <returns>DataTable</returns>
		public DataTable GetGroupLevelBasis(int method_RID, eTyLyType tyLyType)
		{
            try
            {
                return StoredProcedures.MID_GROUP_LEVEL_BASIS_READ_FROM_TYLY_TYPE.Read(_dba, METHOD_RID: method_RID,
                                                                                       TYLY_TYPE_ID: Convert.ToInt32(tyLyType)
                                                                                       );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
		}
        // End TT#1044

		/// <summary>
		/// Get GROUP_LEVEL_BASIS DataTable based on METHOD_RID and SGL_RID
		/// </summary>
		/// <param name="method_RID"></param>
		/// <param name="sgl_RID"></param>
		/// <returns>DataTable</returns>
		public DataTable GetGroupLevelBasis(int method_RID, int sgl_RID)
		{
			try
			{	
                return StoredProcedures.MID_GROUP_LEVEL_BASIS_READ_FROM_STORE_GROUP_LEVEL.Read(_dba, 
                                                                                               METHOD_RID: method_RID,
                                                                                               SGL_RID: sgl_RID
                                                                                               );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetBasisByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_GROUP_LEVEL_BASIS_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
	}


}