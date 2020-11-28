using System;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for MaintainSizeConstraints.
	/// </summary>
	public class MaintainSizeConstraints
	{
		private SizeModelData SizeModelData = null;

		public SizeConstraintModelProfile GetConstrainModel(string modelName)
		{
			SizeConstraintModelProfile aModel = null;
			DataTable dt = this.SizeModelData.SizeConstraintModel_Read(modelName);
			if (dt.Rows.Count == 0)
			{
				aModel = new SizeConstraintModelProfile(Include.NoRID);
			}
			else
			{	
				DataRow aRow = dt.Rows[0];
				int aModelKey = Convert.ToInt32(aRow["SIZE_CONSTRAINT_RID"], CultureInfo.CurrentUICulture);
				aModel = new SizeConstraintModelProfile(aModelKey);
			}
			return aModel;
		}

		public MaintainSizeConstraints(SizeModelData _sizeModelData)
		{
			SizeModelData = _sizeModelData;
		}

		public bool deleteSizeConstraintChildren(int _constraintModelRid,TransactionData td)
		{
			bool Successful;
			Successful = true;

			try
			{
				Successful = SizeModelData.DeleteSizeConstraintChildren(_constraintModelRid, td);

			}
			catch(Exception)
			{
				throw;
			}

			return Successful;

		}

		public bool insertUpdateCollection(int _constraintModelRid,TransactionData td,CollectionSets _setsCollection)
		{ 
			bool Successfull = true;
			
			bool IsConnectionOpen = td.ConnectionIsOpen;

			try
			{	
				if (!IsConnectionOpen)
				{
					td.OpenUpdateConnection();
				}

				deleteSizeConstraintChildren(_constraintModelRid,td);

				//PROCESS SETS AND ALL DESCENDANTS
				foreach (ItemSet oItemSet in _setsCollection)
				{
					ProcessSet(td, oItemSet);
									
					//PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
					foreach (ItemAllColor oItemAllColor in oItemSet.collectionAllColors)
					{
						// BEGIN TT#739-MD - STodd - Performance
						if (oItemAllColor.Min != Include.UndefinedMinimum || oItemAllColor.Max != Include.UndefinedMaximum || oItemAllColor.Mult != Include.UndefinedMultiple)
						{
						ProcessMinMax(td, oItemAllColor);
						}

						foreach (ItemSizeDimension oItemSizeDimension in oItemAllColor.collectionSizeDimensions)
						{
							if (oItemSizeDimension.Min != Include.UndefinedMinimum || oItemSizeDimension.Max != Include.UndefinedMaximum || oItemSizeDimension.Mult != Include.UndefinedMultiple)
							{
							ProcessMinMax(td, oItemSizeDimension);
							}

							foreach (ItemSize oItemSize in oItemSizeDimension.collectionSizes)
							{
								if (oItemSize.Min != Include.UndefinedMinimum || oItemSize.Max != Include.UndefinedMaximum || oItemSize.Mult != Include.UndefinedMultiple)
								{
								ProcessMinMax(td, oItemSize);
								}
							}
						}
						// END TT#739-MD - STodd - Performance
					}

					//PROCESS COLOR LEVEL AND ALL DESCENDANTS
					foreach (ItemColor oItemColor in oItemSet.collectionColors)
					{
						// BEGIN TT#739-MD - STodd - Performance
						if (oItemColor.Min != Include.UndefinedMinimum || oItemColor.Max != Include.UndefinedMaximum || oItemColor.Mult != Include.UndefinedMultiple)
						{
						ProcessMinMax(td, oItemColor);
						}

						foreach (ItemSizeDimension oItemSizeDimension in oItemColor.collectionSizeDimensions)
						{
							if (oItemSizeDimension.Min != Include.UndefinedMinimum || oItemSizeDimension.Max != Include.UndefinedMaximum || oItemSizeDimension.Mult != Include.UndefinedMultiple)
							{
							ProcessMinMax(td, oItemSizeDimension);
							}

							foreach (ItemSize oItemSize in oItemSizeDimension.collectionSizes)
							{
								if (oItemSize.Min != Include.UndefinedMinimum || oItemSize.Max != Include.UndefinedMaximum || oItemSize.Mult != Include.UndefinedMultiple)
								{
								ProcessMinMax(td, oItemSize);
								}
							}

						}
					}
					// END TT#739-MD - STodd - Performance
				}
					
				if (!IsConnectionOpen)
				{
					td.CommitData();
					td.CloseUpdateConnection();
				}

			}
			catch(Exception e)
			{
				if (!IsConnectionOpen)
				{
					td.Rollback();
					td.CloseUpdateConnection();
				}
				string exceptionMessage = e.Message;
				Successfull = false;
				throw;
			}

			return Successfull;
		}


		/// <summary>
		/// Processes ItemSet object from CollectionSets
		/// </summary>
		/// <param name="td">TransactionData object</param>
		/// <param name="pItem">Object of type ItemSet</param>
		/// <returns></returns>
		public bool ProcessSet(TransactionData td, ItemSet pItem)
		{
			bool Successfull = true;

			try
			{
				//MIDDbParameter[] inParams  = InitSetInParams(pItem);
                //Begin TT#1303-MD -jsobek -Size Constraints are ZERO on Save As
                //int MIN = 0;
                int? MIN = null;
                if (pItem.Min != Include.UndefinedMinimum) MIN = pItem.Min;

                //int MAX = 0;
                int? MAX = null;
                if (pItem.Max != Include.UndefinedMaximum) MAX = pItem.Max;

                //int MULT = 0;
                int? MULT = null;
                if (pItem.Mult != Include.UndefinedMultiple) MULT = pItem.Mult;
                //End TT#1303-MD -jsobek -Size Constraints are ZERO on Save As

                SizeModelData.ProcessGroupLevel(td,
                                                SIZECONSTRAINTRID: pItem.MethodRid,
                                                SGLRID: pItem.SglRid,
                                                MIN: MIN,
                                                MAX: MAX,
                                                MULT: MULT,
                                                ROWTYPEID: (int)pItem.RowTypeID
                                                );
						
				Successfull = true;
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				Successfull = false;
				throw;
			}
			return Successfull;
		}

        ///// <summary>
        ///// Initializes parameters for stored procedure.
        ///// </summary>
        ///// <returns></returns>
        //public MIDDbParameter[] InitSetInParams()
        //{
        //    MIDDbParameter[] inParams  = { new MIDDbParameter("@SIZECONSTRAINTRID", eDbType.Int, 0, "", eParameterDirection.Input),
        //                                  new MIDDbParameter("@SGLRID", eDbType.Int,0,"", eParameterDirection.Input),
        //                                  //										  new MIDDbParameter("@FILLZEROSIND", eDbType.Char, 1, "", eParameterDirection.Input),
        //                                  //										  new MIDDbParameter("@FILLZEROSQTY", eDbType.Int, 0, "", eParameterDirection.Input),
        //                                  //										  new MIDDbParameter("@FILLSIZEHOLESIND",eDbType.Char,1,"", eParameterDirection.Input),
        //                                  //										  new MIDDbParameter("@FILLSEQUENCE",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@MIN",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@MAX",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@MULT",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  //										  new MIDDbParameter("@RULE",eDbType.Int,0,"", eParameterDirection.Input)
        //                                  //										  new MIDDbParameter("@QTY",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@ROWTYPEID",eDbType.Int,0,"", eParameterDirection.Input)
        //                              } ;

        //    return inParams;
        //}


        ///// <summary>
        ///// Initializes parameters for stored procedure.
        ///// </summary>
        ///// <returns></returns>
        //public MIDDbParameter[] InitSetInParams(ItemSet pItem)
        //{
        //    MIDDbParameter[] inParams  = InitSetInParams();

        //    #region SET PARAMETER VALUES
        //    inParams[0].Value = pItem.MethodRid;

        //    inParams[1].Value = pItem.SglRid;

        //    //			inParams[2].Value = Include.ConvertBoolToChar(pItem.FillZerosInd);
        //    //
        //    //			if (pItem.FillZerosQty != Include.UndefinedQuantity)
        //    //			{
        //    //				inParams[3].Value = pItem.FillZerosQty;
        //    //			}
        //    //
        //    //			inParams[4].Value = Include.ConvertBoolToChar(pItem.FillSizeHolesInd);
        //    //
        //    //			if (pItem.FillSequence != Include.Undefined)
        //    //			{
        //    //				inParams[5].Value = pItem.FillSequence;
        //    //			}
									
        //    if (pItem.Min != Include.UndefinedMinimum)
        //    {
        //        inParams[2].Value = pItem.Min;
        //    }

        //    if (pItem.Max != Include.UndefinedMaximum)
        //    {
        //        inParams[3].Value = pItem.Max;
        //    }

        //    if (pItem.Mult != Include.UndefinedMultiple)
        //    {
        //        inParams[4].Value = pItem.Mult;
        //    }

        //    //			if (pItem.Rule != Include.UndefinedRule)
        //    //			{
        //    //				inParams[9].Value = pItem.Rule;
        //    //			}
        //    //
        //    //			if (pItem.Qty != Include.UndefinedQuantity)
        //    //			{
        //    //				inParams[10].Value = pItem.Qty;
        //    //			}

        //    inParams[5].Value = pItem.RowTypeID;
        //    #endregion

        //    return inParams;
        //}


		/// <summary>
		/// Processes ItemColor, ItemAllColor, ItemSize, or ItemSizeDimension objects from CollectionSets
		/// </summary>
		/// <param name="td">TransactionData object</param>
		/// <param name="pItem">Object of type IMinMaxItem</param>
		/// <returns></returns>
		public bool ProcessMinMax(TransactionData td, MinMaxItemBase pItem)
		{
			bool Successfull = true;

			try
			{
				//MIDDbParameter[] inParams  = InitMinMaxInParams(pItem);

				//SizeModelData.ProcessMinMax(td, inParams);

                //Begin TT#1303-MD -jsobek -Size Constraints are ZERO on Save As
                //int MIN = 0;
                int? MIN = null;
                if (pItem.Min != Include.UndefinedMinimum) MIN = pItem.Min;

                //int MAX = 0;
                int? MAX = null;
                if (pItem.Max != Include.UndefinedMaximum) MAX = pItem.Max;

                //int MULT = 0;
                int? MULT = null;
                if (pItem.Mult != Include.UndefinedMultiple) MULT = pItem.Mult;
                //End TT#1303-MD -jsobek -Size Constraints are ZERO on Save As

                SizeModelData.ProcessMinMax(td,
                                            SIZECONSTRAINTRID: pItem.MethodRid,
                                            SGLRID: pItem.SglRid,
                                            COLORCODERID: pItem.ColorCodeRid,
                                            SIZESRID: pItem.SizesRid,
                                            SIZECODERID: pItem.SizeCodeRid,
                                            MIN: MIN,
                                            MAX: MAX,
                                            MULT: MULT,
                                            ROWTYPEID: (int)pItem.RowTypeID,
                                            DIMENSIONS_RID: pItem.DimensionsRid
                                            );
                                
						
				Successfull = true;
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				Successfull = false;
				throw;
			}
			return Successfull;
		}

        ///// <summary>
        ///// Creates parameters for stored procedure.
        ///// </summary>
        ///// <returns></returns>
        //public MIDDbParameter[] InitMinMaxInParams()
        //{
        //    MIDDbParameter[] inParams  = { new MIDDbParameter("@SIZECONSTRAINTRID", eDbType.Int, 0, "", eParameterDirection.Input),
        //                                  new MIDDbParameter("@SGLRID", eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@COLORCODERID", eDbType.Int, 0, "", eParameterDirection.Input),
        //                                  new MIDDbParameter("@SIZESRID",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@SIZECODERID",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@MIN",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@MAX",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@MULT",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  //										  new MIDDbParameter("@RULE",eDbType.Int,1,"", eParameterDirection.Input),
        //                                  //										  new MIDDbParameter("@QTY",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@ROWTYPEID",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@DIMENSIONS_RID",eDbType.Int,0,"", eParameterDirection.Input)
        //                              } ;

        //    return inParams;
        //}


        ///// <summary>
        ///// Initializes parameters for stored procedure.
        ///// </summary>
        ///// <returns></returns>
        //public MIDDbParameter[] InitMinMaxInParams(MinMaxItemBase pItem)
        //{
        //    MIDDbParameter[] inParams  = InitMinMaxInParams();

        //    #region SET PARAMETER VALUES
        //    inParams[0].Value = pItem.MethodRid;

        //    inParams[1].Value = pItem.SglRid;

        //    inParams[2].Value = pItem.ColorCodeRid;

        //    inParams[3].Value = pItem.SizesRid;

        //    inParams[4].Value = pItem.SizeCodeRid;

        //    if (pItem.Min != Include.UndefinedMinimum)
        //    {
        //        inParams[5].Value = pItem.Min;
        //    }
				
        //    if (pItem.Max != Include.UndefinedMaximum)
        //    {
        //        inParams[6].Value = pItem.Max;
        //    }

        //    if (pItem.Mult != Include.UndefinedMultiple)
        //    {
        //        inParams[7].Value = pItem.Mult;
        //    }

        //    //			if (pItem.Rule != Include.UndefinedRule)
        //    //			{
        //    //				inParams[8].Value = pItem.Rule;
        //    //			}
        //    //
        //    //			if (pItem.Qty != Include.UndefinedQuantity)
        //    //			{
        //    //				inParams[9].Value = pItem.Qty;
        //    //			}

        //    inParams[8].Value = pItem.RowTypeID;

        //    inParams[9].Value = pItem.DimensionsRid;
        //    #endregion

        //    return inParams;
        //}


		/// <summary>
		/// Fills a UltraGrid ValueList with size dimensions.
		/// </summary>
		public DataTable FillSizeDimensionList(int RID, eGetDimensions getDimensions)
		{
			// Begin Issue # 3685 - stodd 2/7/2006
			// Needed sizes to be in correct sequence
			DataTable _dtDimensions = AddColumnsToDimensionDataTable();

			if (getDimensions == eGetDimensions.SizeGroupRID)
			{
				SizeGroupProfile sgp = new SizeGroupProfile(RID);
				foreach (SizeCodeProfile scp in sgp.SizeCodeList.ArrayList)
				{
					DataRow [] hits = _dtDimensions.Select("DIMENSIONS_RID = " + scp.SizeCodeSecondaryRID.ToString(CultureInfo.CurrentUICulture));
					if (hits.Length == 0)
					{
						DataRow newRow = _dtDimensions.NewRow();
						newRow["DIMENSIONS_RID"] = scp.SizeCodeSecondaryRID;
						newRow["SIZE_CODE_SECONDARY"] = scp.SizeCodeSecondary;
						_dtDimensions.Rows.Add(newRow);
					}
				}

			}
			else
			{
				SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(RID);
				foreach (SizeCodeProfile scp in scgp.SizeCodeList.ArrayList)
				{
					DataRow [] hits = _dtDimensions.Select("DIMENSIONS_RID = " + scp.SizeCodeSecondaryRID.ToString(CultureInfo.CurrentUICulture));
					if (hits.Length == 0)
					{
						DataRow newRow = _dtDimensions.NewRow();
						newRow["DIMENSIONS_RID"] = scp.SizeCodeSecondaryRID;
						newRow["SIZE_CODE_SECONDARY"] = scp.SizeCodeSecondary;
						_dtDimensions.Rows.Add(newRow);
					}
				}
			}

			return _dtDimensions;
		}

		public DataTable AddColumnsToDimensionDataTable()
		{
			DataTable _dtDimensions = MIDEnvironment.CreateDataTable();

			DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "DIMENSIONS_RID";
			_dtDimensions.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SIZE_CODE_SECONDARY";
			_dtDimensions.Columns.Add(dataColumn);

			return _dtDimensions;
		}

		/// <summary>
		/// Fills a UltraGrid ValueList with sizes based on a selected Size Group
		/// </summary>
		public DataTable FillSizesList(int RID, eGetSizes getSizes)
		{
			// Begin Issue # 3685 - stodd 2/7/2006
			// Needed sizes to be in correct sequence
			DataTable _dtSizes = AddColumnsToSizesDataTable();

			if (getSizes == eGetSizes.SizeGroupRID)
			{
				SizeGroupProfile sgp = new SizeGroupProfile(RID);
				foreach (SizeCodeProfile scp in sgp.SizeCodeList.ArrayList)
				{
					DataRow newRow = _dtSizes.NewRow();
					newRow["SIZES_RID"] = scp.SizeCodePrimaryRID;
					newRow["SIZE_CODE"] = scp.SizeCodeID;
					newRow["SIZE_CODE_PRIMARY"] = scp.SizeCodePrimary;
					newRow["SIZE_CODE_SECONDARY"] = scp.SizeCodeSecondary;
					newRow["SIZE_CODE_RID"] = scp.Key;
					newRow["DIMENSIONS_RID"] = scp.SizeCodeSecondaryRID;
					_dtSizes.Rows.Add(newRow);
				}

			}
			else  // (getSizes == eGetSizes.SizeGroupRID)
			{
				SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(RID);
				foreach (SizeCodeProfile scp in scgp.SizeCodeList.ArrayList)
				{
					DataRow newRow = _dtSizes.NewRow();
					newRow["SIZES_RID"] = scp.SizeCodePrimaryRID;
					newRow["SIZE_CODE"] = scp.SizeCodeID;
					newRow["SIZE_CODE_PRIMARY"] = scp.SizeCodePrimary;
					newRow["SIZE_CODE_SECONDARY"] = scp.SizeCodeSecondary;
					newRow["SIZE_CODE_RID"] = scp.Key;
					newRow["DIMENSIONS_RID"] = scp.SizeCodeSecondaryRID;
					_dtSizes.Rows.Add(newRow);
				}
			}

			return _dtSizes;
		}

		private DataTable AddColumnsToSizesDataTable()
		{
			DataTable _dtSizes = MIDEnvironment.CreateDataTable();

			DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "SIZES_RID";
			_dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "SIZE_CODE_RID";
			_dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "DIMENSIONS_RID";
			_dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SIZE_CODE";
			_dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SIZE_CODE_PRIMARY";
			_dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SIZE_CODE_SECONDARY";
			_dtSizes.Columns.Add(dataColumn);

			return _dtSizes;
		}

	}
}
