using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	/// <summary>
	/// Data layer for rule allocations.
	/// </summary>
	public partial class RuleAllocation : DataLayer
	{
        // Begin TT#827-MD - JSmith - Allocation Reviews Performance
        //private StringBuilder _documentXML;
        private DataTable _ruleTable = null;
        // End TT#827-MD - JSmith - Allocation Reviews Performance
		private int _recordsWritten = 0;

		public RuleAllocation() : base()
		{
		}

		public void Rule_XMLInit()
		{
			try
			{
                // Begin TT#827-MD - JSmith - Allocation Reviews Performance
                //_documentXML = new StringBuilder();
                //// add root element
                //_documentXML.Append("<root> ");

                _ruleTable = new DataTable();
                _ruleTable.Columns.Add("ST_RID", typeof(int));
                _ruleTable.Columns.Add("RULE_TYPE", typeof(int));
                _ruleTable.Columns.Add("UNITS", typeof(int));
                // End TT#827-MD - JSmith - Allocation Reviews Performance

				_recordsWritten = 0;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines the existence of a layer of store rules for a header and pack.
		/// </summary>
		/// <param name="aHeaderRID">RID that identifies the header</param>
		/// <param name="aLayerID">Layer ID of the store rules for this header.</param>
		/// <param name="aPackName">Pack Name</param>
		/// <returns></returns>
        //public bool RuleAllocationExists(int aHeaderRID, string aPackName, int aLayerID)
        //{
        //    //return RuleAllocationExists(this.GetRuleTableName(eComponentType.SpecificPack), aHeaderRID, aLayerID, GetPackRID(aHeaderRID, aPackName));
        //    return RuleAllocationExists(eComponentType.SpecificPack, aHeaderRID, aLayerID, GetPackRID(aHeaderRID, aPackName));
        //}

		/// <summary>
		/// Determines the existence of a layer of store rules for a header and component
		/// </summary>
		/// <param name="aHeaderRID">RID that identifies the header</param>
		/// <param name="aComponentType">Identifies the component type.</param>
		/// <param name="aComponentRID">RID of the component (Note: "-1" indicates a component without an RID such as TOTAL, DETAIL or BULK</param>
		/// <param name="aLayerID">Layer ID of the store rules for this header.</param>
		/// <returns>True: indicates layer of store rules exists; False: indicates layer of store rules does not exist.</returns>
        //public bool RuleAllocationExists(int aHeaderRID, eComponentType aComponentType, int aComponentRID, int aLayerRID)
        //{
        //    //return RuleAllocationExists(this.GetRuleTableName(aComponentType), aHeaderRID, aComponentRID, aLayerRID);
        //    return RuleAllocationExists(aComponentType, aHeaderRID, aComponentRID, aLayerRID);
        //}

		/// <summary>
		/// Determines the existence of a layer of store rules for a header and component
		/// </summary>
		/// <param name="aRuleTable">Rule table name</param>
		/// <param name="aHeaderRID">RID that identifies the header</param>
		/// <param name="aComponentRID">RID of the component (Note: "-1" indicates a component without an RID such as TOTAL, DETAIL or BULK</param>
		/// <param name="aLayerID">Layer ID of the store rules for this header.</param>
		/// <returns>True: indicates layer of store rules exists; False: indicates layer of store rules does not exist.</returns>
		//public bool RuleAllocationExists(string aRuleTable, int aHeaderRID, int aComponentRID, int aLayerID)
        //public bool RuleAllocationExists(eComponentType aComponentType, int aHeaderRID, int aComponentRID, int aLayerID)
        //{



        //    //string SQLCommand;
        //    int rowCount;
        //    if (aComponentRID == Include.NoRID)
        //    {
        //        // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //        //SQLCommand = "select count(*) MyCount from " + aRuleTable + " where HDR_RID ='" + aHeaderRID +  "' and LAYER_ID ='"  + aLayerID + "'";
        //        // end MID Track # 2354
        //        switch (aComponentType)
        //        {
        //            case (eComponentType.Bulk):
        //                {
        //                    //return "BULK_RULE";
        //                    rowCount = StoredProcedures.MID_BULK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba,
        //                                                                                        HDR_RID: aHeaderRID,
        //                                                                                        LAYER_ID: aLayerID
        //                                                                                        );
        //                }
        //            case (eComponentType.DetailType):
        //                {
        //                    //return "DETAIL_RULE";
        //                    rowCount = StoredProcedures.MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba,
        //                                                                                          HDR_RID: aHeaderRID,
        //                                                                                          LAYER_ID: aLayerID
        //                                                                                          );
        //                }
        //            case (eComponentType.SpecificColor):
        //                {
        //                    ////return "BULK_COLOR_RULE";
        //                    ////if (aComponentType == eComponentType.Bulk)
        //                    ////{
        //                    //    // Assortment BEGIN
        //                    //    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //                    //    //SQLCommand = "select count (*) MyCount from " + aRuleTable + " where HDR_RID ='" + aHeaderRID + "' and LAYER_ID ='" + aLayerID + "' and COLOR_CODE_RID ='" + aComponentRID + "'";
        //                    //    // end MID Track # 2354

        //                    //int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);
        //                    //    SQLCommand = "select count (*) MyCount from " + aRuleTable + " where HDR_BC_RID ='" + hdrBCRID + "' and LAYER_ID ='" + aLayerID + "'";
        //                    //    // Assortment END
        //                    ////}
        //                    rowCount = StoredProcedures.MID_BULK_COLOR_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba,
        //                                                                                                  HDR_RID: aHeaderRID,
        //                                                                                                  LAYER_ID: aLayerID
        //                                                                                                  );
        //                }
        //            case (eComponentType.SpecificPack):
        //                {
        //                    //return "PACK_RULE";
        //                    throw new Exception("Rule Allocation Component Type specific Pack, must provide a component rid");
        //                }
        //            case (eComponentType.Total):
        //                {
        //                    //return "TOTAL_RULE";
        //                    rowCount = StoredProcedures.MID_TOTAL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba,
        //                                                                                         HDR_RID: aHeaderRID,
        //                                                                                         LAYER_ID: aLayerID
        //                                                                                         );
        //                }
        //            case (eComponentType.AllColors):
        //            case (eComponentType.AllGenericPacks):
        //            case (eComponentType.AllPacks):
        //            case (eComponentType.AllSizes):
        //            case (eComponentType.ColorAndSize):
        //            case (eComponentType.GenericType):
        //            case (eComponentType.SpecificSize):
        //                {
        //                    throw new Exception("Rule Allocation Component Type must be: TOTAL, specific Pack, DETAIL, BULK or specific Color");
        //                }
        //            default:
        //                {
        //                    throw new Exception("Unknown Component Type while processing Rule Allocation");
        //                }
        //        }
        //    }
        //    //else if (aRuleTable == this.GetRuleTableName(eComponentType.Bulk))
           
        //    else
        //    {
        //        // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //        SQLCommand = "select count (*) MyCount from " + aRuleTable + " where HDR_RID ='" + aHeaderRID + "' and LAYER_ID ='" + aLayerID + "' and HDR_PACK_RID = '" + aComponentRID + "'";
        //        // end MID Track # 2354
        //        switch (aComponentType)
        //        {
        //            case (eComponentType.Bulk):
        //                {
        //                    return "BULK_RULE";
        //                }
        //            case (eComponentType.DetailType):
        //                {
        //                    return "DETAIL_RULE";
        //                }
        //            case (eComponentType.SpecificColor):
        //                {
        //                    return "BULK_COLOR_RULE";
        //                    //int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);
        //                    //    SQLCommand = "select count (*) MyCount from " + aRuleTable + " where HDR_BC_RID ='" + hdrBCRID + "' and LAYER_ID ='" + aLayerID + "'";
                         
        //                }
        //            case (eComponentType.SpecificPack):
        //                {
        //                    return "PACK_RULE";
        //                }
        //            case (eComponentType.Total):
        //                {
        //                    return "TOTAL_RULE";
        //                }
        //            case (eComponentType.AllColors):
        //            case (eComponentType.AllGenericPacks):
        //            case (eComponentType.AllPacks):
        //            case (eComponentType.AllSizes):
        //            case (eComponentType.ColorAndSize):
        //            case (eComponentType.GenericType):
        //            case (eComponentType.SpecificSize):
        //                {
        //                    throw new Exception("Rule Allocation Component Type must be: TOTAL, specific Pack, DETAIL, BULK or specific Color");
        //                }
        //            default:
        //                {
        //                    throw new Exception("Unknown Component Type while processing Rule Allocation");
        //                }
        //        }
        //    }
        //    //return (_dba.ExecuteRecordCount( SQLCommand ) > 0);
        //    return (rowCount > 0);
        //}
      
        // Assortment BEGIN 
        public int GetBulkColorRID(int headerRID, int colorCode)
        {
            //string SQLCommand = "select HDR_BC_RID from HEADER_BULK_COLOR where HDR_RID = " + headerRID.ToString(CultureInfo.CurrentUICulture) +
            //    " and COLOR_CODE_RID = " + colorCode.ToString(CultureInfo.CurrentUICulture);
            //return Convert.ToInt32(_dba.ExecuteScalar(SQLCommand), CultureInfo.CurrentUICulture);
            return (int)StoredProcedures.MID_HEADER_BULK_COLOR_READ_BULK_COLOR_RID.ReadValues(_dba,
                                                                                      HDR_RID: headerRID,
                                                                                      COLOR_CODE_RID: colorCode
                                                                                      );
        }
        // Assortment END

		/// <summary>
		/// Gets the Pack RID for a given pack on a given header.
		/// </summary>
		/// <param name="aHeaderRID">RID that identifies the header.</param>
		/// <param name="aPackName">Pack name of the pack on this header</param>
		/// <returns>RID of the pack for this header</returns>
		public int GetPackRID (int aHeaderRID, string aPackName)
		{
            //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
            ////Begin TT#1663 - DOConnell - Change Pack Name with quote issue
            ////string SQLCommand = "select HDR_PACK_RID from HEADER_PACK where " +
            ////    "HDR_RID = " + aHeaderRID.ToString(CultureInfo.CurrentUICulture) + " and HDR_PACK_NAME ='" + aPackName + "'";
            ////// end MID Track # 2354
            ////return Convert.ToInt32(_dba.ExecuteScalar( SQLCommand), CultureInfo.CurrentUICulture);
            ////Begin TT#1901 - DOConnell - Error Processing of Rule Method
            ////string SQLCommand = "select HDR_PACK_RID from HEADER_PACK where HDR_ID = @HDR_RID" +
            //string SQLCommand = "select HDR_PACK_RID from HEADER_PACK where HDR_RID = @HDR_RID" +
            ////End TT#1901 - DOConnell - Error Processing of Rule Method
            //                    " and HDR_PACK_NAME = @HDR_PACK_NAME";

            //MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
            //                              new MIDDbParameter("@HDR_PACK_NAME", aPackName, eDbType.VarChar, eParameterDirection.Input) };

            //int hdrRID = Convert.ToInt32(_dba.ExecuteScalar(SQLCommand, InParams), CultureInfo.CurrentUICulture);

            //return hdrRID;
            return (int)StoredProcedures.MID_HEADER_PACK_READ_RID_FROM_PACK_NAME.ReadValue(_dba,
                                                                                    HDR_RID: aHeaderRID,
                                                                                    HDR_PACK_NAME: aPackName
                                                                                    );
            //End TT#1663 - DOConnell - Change Pack Name with quote issue
		}

		/// <summary>
		/// Gets the Rule Table Name associated with the component.
		/// </summary>
		/// <param name="aComponentType"></param>
		/// <returns></returns>
		public string GetRuleTableName (eComponentType aComponentType)
		{
			switch (aComponentType)
			{
				case (eComponentType.Bulk):
				{
					return "BULK_RULE";
				}
				case (eComponentType.DetailType):
				{
					return "DETAIL_RULE";
				}
				case (eComponentType.SpecificColor):
				{
					return "BULK_COLOR_RULE";
				}
				case (eComponentType.SpecificPack):
				{
					return "PACK_RULE";
				}
				case (eComponentType.Total):
				{
					return "TOTAL_RULE";
				}
				case (eComponentType.AllColors):
				case (eComponentType.AllGenericPacks):
				case (eComponentType.AllPacks):
				case (eComponentType.AllSizes):
				case (eComponentType.ColorAndSize):
				case (eComponentType.GenericType):
				case (eComponentType.SpecificSize):
				{
					throw new Exception ("Rule Allocation Component Type must be: TOTAL, specific Pack, DETAIL, BULK or specific Color");
				}
				default:
				{
					throw new Exception ("Unknown Component Type while processing Rule Allocation");
				}
			}
		}

        ///// <summary>
        ///// Gets the Rule Layer Table Name associated with the component.
        ///// </summary>
        ///// <param name="aComponentType"></param>
        ///// <returns></returns>
        //public string GetLayerTableName (eComponentType aComponentType)
        //{
        //    switch (aComponentType)
        //    {
        //        case (eComponentType.Bulk):
        //        {
        //            return "BULK_RULE_LAYER";
        //        }
        //        case (eComponentType.DetailType):
        //        {
        //            return "DETAIL_RULE_LAYER";
        //        }
        //        case (eComponentType.SpecificColor):
        //        {
        //            return "BULK_COLOR_RULE_LAYER";
        //        }
        //        case (eComponentType.SpecificPack):
        //        {
        //            return "PACK_RULE_LAYER";
        //        }
        //        case (eComponentType.Total):
        //        {
        //            return "TOTAL_RULE_LAYER";
        //        }
        //        case (eComponentType.AllColors):
        //        case (eComponentType.AllGenericPacks):
        //        case (eComponentType.AllPacks):
        //        case (eComponentType.AllSizes):
        //        case (eComponentType.ColorAndSize):
        //        case (eComponentType.GenericType):
        //        case (eComponentType.SpecificSize):
        //        {
        //            throw new Exception ("Rule Allocation Component Type must be: TOTAL, specific Pack, DETAIL, BULK or specific Color");
        //        }
        //        default:
        //        {
        //            throw new Exception ("Unknown Component Type while processing Rule Layer");
        //        }
        //    }
        //}

		/// <summary>
		/// Gets DataTable of Store Rules associated with the given header, component and layer ID.
		/// </summary>
		/// <param name="aHeaderRID">RID that identifies the Header</param>
		/// <param name="aComponentType">Component Type</param>
		/// <param name="aComponentRID">RID that identifies the component on the header (NOTE: "-1" indicates no RID for components such as TOTAL, DETAIL and BULK that have no RID</param>
		/// <param name="aLayerID">Layer ID</param>
		/// <returns>DataTable of store RULES</returns>
		public DataTable GetStoreComponentRules(int aHeaderRID, eComponentType aComponentType, int aComponentRID, int aLayerID)
		{
			//string tableName = this.GetRuleTableName(aComponentType);
			//string SQLCommand;
			switch (aComponentType)
			{
                case (eComponentType.Total):
                {
                    return StoredProcedures.MID_TOTAL_RULE_READ.Read(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                }
                case (eComponentType.Bulk):
                {
                    return StoredProcedures.MID_BULK_RULE_READ.Read(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                }
				case (eComponentType.DetailType):
				{
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    //SQLCommand = "select * from " + tableName + 
                    //    " where HDR_RID =" + aHeaderRID.ToString(CultureInfo.CurrentUICulture) + 
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    //// end MID Track # 2354
                    //break;
                    return StoredProcedures.MID_DETAIL_RULE_READ.Read(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
				}
				case (eComponentType.SpecificColor):
				{
                    //// Assortment BEGIN 
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    ////SQLCommand = "select * from " + tableName + 
                    ////	" where HDR_RID =" + aHeaderRID.ToString(CultureInfo.CurrentUICulture) + 
                    ////	" and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture) + 
                    ////	" and COLOR_CODE_RID =" + aComponentRID.ToString(CultureInfo.CurrentUICulture);
                    //// end MID Track # 2354
                    int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);
                    //SQLCommand = "select * from " + tableName +
                    //    " where HDR_BC_RID =" + hdrBCRID.ToString(CultureInfo.CurrentUICulture) +
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    //// Assortment END 
                    //break;
                    return StoredProcedures.MID_BULK_COLOR_RULE_READ.Read(_dba, HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID);
				}
				case (eComponentType.SpecificPack):
				{
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    //SQLCommand = "select * from " + tableName + 
                    //    " where HDR_PACK_RID =" + aComponentRID.ToString(CultureInfo.CurrentUICulture) + 
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    //// end MID Track # 2354
                    //break;
                    return StoredProcedures.MID_PACK_RULE_READ.Read(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID);
				}
				default:
				{
					throw new Exception ("Unknown Component Type while processing Rule Layer");
				}
			}
			//return _dba.ExecuteSQLQuery( SQLCommand, "GET_STORE_RULES" );
		}

		/// <summary>
		/// Gets a data table of layer information defined for a given header and component.
		/// </summary>
		/// <param name="aHeaderRID">RID of the header</param>
		/// <param name="aComponentType">Identifies the component type</param>
		/// <param name="aComponentRID">RID of the color or pack (ignored when not a color or pack)</param>
		/// <returns></returns>
		public DataTable GetLayerInfo(int aHeaderRID, eComponentType aComponentType, int aComponentRID, int aLayerID)
		{
			//string layerTableName = this.GetLayerTableName(aComponentType);
			//string SQLCommand;
			switch (aComponentType)
			{
                case (eComponentType.Total):
                {
                    return StoredProcedures.MID_TOTAL_RULE_LAYER_READ_INFO.Read(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                }
                case (eComponentType.Bulk):
                {
                    return StoredProcedures.MID_BULK_RULE_LAYER_READ_INFO.Read(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                }
				case (eComponentType.DetailType):
				{
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    //SQLCommand = "select * from " + layerTableName + 
                    //    " where HDR_RID =" + aHeaderRID.ToString(CultureInfo.CurrentUICulture) +
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    //// end MID Track # 2354
                    //break;
                    return StoredProcedures.MID_DETAIL_RULE_LAYER_READ_INFO.Read(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
				}
				case (eComponentType.SpecificColor):
				{
                    //// Assortment BEGIN
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    ////SQLCommand = "select * from " + layerTableName + 
                    ////	" where HDR_RID =" + aHeaderRID.ToString(CultureInfo.CurrentUICulture) + 
                    ////	" and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture) +
                    ////	" and COLOR_CODE_RID =" + aComponentRID.ToString(CultureInfo.CurrentUICulture);
                    //// end MID Track # 2354
                    int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);
                    //SQLCommand = "select * from " + layerTableName +
                    //    " where HDR_BC_RID =" + hdrBCRID.ToString(CultureInfo.CurrentUICulture) +
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    //// Assortment END 
                    //break;
                    return StoredProcedures.MID_BULK_COLOR_RULE_LAYER_READ_INFO.Read(_dba, HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID);
				}
				case (eComponentType.SpecificPack):
				{
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    //SQLCommand = "select * from " + layerTableName + 
                    //    " where HDR_PACK_RID =" + aComponentRID.ToString(CultureInfo.CurrentUICulture) +
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    //// end MID Track # 2354
                    //break;
                    return StoredProcedures.MID_PACK_RULE_LAYER_READ_INFO.Read(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID);
				}
				default:
				{
					throw new Exception ("Unknown Component Type while processing Rule Layer");
				}
			}
			//return _dba.ExecuteSQLQuery( SQLCommand, "GET_LAYER_INFO" );
		}


        /// <summary>
        /// Gets a data table of all layer information defined for a given header and component.
        /// </summary>
        /// <param name="aHeaderRID">RID of the header</param>
        /// <param name="aComponentType">Identifies the component type</param>
        /// <param name="aComponentRID">RID of the color or pack (ignored when not a color or pack)</param>
        /// <returns></returns>
		public DataTable GetLayers(int aHeaderRID, eComponentType aComponentType, int aComponentRID)
		{
			//string layerTableName = this.GetLayerTableName(aComponentType);
			//string SQLCommand;
			switch (aComponentType)
			{
                case (eComponentType.Total):
                {
                    return StoredProcedures.MID_TOTAL_RULE_LAYER_READ.Read(_dba, HDR_RID: aHeaderRID);
                }
				case (eComponentType.Bulk):
                {
                    return StoredProcedures.MID_BULK_RULE_LAYER_READ.Read(_dba, HDR_RID: aHeaderRID);
                }
				case (eComponentType.DetailType):
				{
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    //SQLCommand = "select * from " + layerTableName + 
                    //    " where HDR_RID =" + aHeaderRID.ToString(CultureInfo.CurrentUICulture);
                    //// end MID Track # 2354
                    //break;
                    return StoredProcedures.MID_DETAIL_RULE_LAYER_READ.Read(_dba, HDR_RID: aHeaderRID);
				}
				case (eComponentType.SpecificColor):
				{
                    //// Assortment BEGIN 
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    ////SQLCommand = "select * from " + layerTableName + 
                    ////	" where HDR_RID =" + aHeaderRID.ToString(CultureInfo.CurrentUICulture) + 
                    ////	" and COLOR_CODE_RID =" + aComponentRID.ToString(CultureInfo.CurrentUICulture);
                    //// end MID Track # 2354
                    int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);
                    //SQLCommand = "select * from " + layerTableName +
                    //    " where HDR_BC_RID =" + hdrBCRID.ToString(CultureInfo.CurrentUICulture);
                    //// Assortment END 
                    //break;
                    return StoredProcedures.MID_BULK_COLOR_RULE_LAYER_READ.Read(_dba, HDR_BC_RID: hdrBCRID);
				}
				case (eComponentType.SpecificPack):
				{
                    //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
                    //SQLCommand = "select * from " + layerTableName + 
                    //    " where HDR_PACK_RID =" + aComponentRID.ToString(CultureInfo.CurrentUICulture);
                    //// end MID Track # 2354
                    //break;
                    return StoredProcedures.MID_PACK_RULE_LAYER_READ.Read(_dba, HDR_PACK_RID: aComponentRID);
				}
				default:
				{
					throw new Exception ("Unknown Component Type while processing Rule Layer");
				}
			}
			//return _dba.ExecuteSQLQuery( SQLCommand, "GET_LAYER_IDS" );
		}


		/// <summary>
		/// Gets a layer ID for the given header and component.
		/// </summary>
		/// <param name="aHeaderRID">RID of the header</param>
		/// <param name="aComponentType">Component type</param>
		/// <param name="aComponentRID">RID of the color or pack component (ignored when not a color or pack)</param>
		/// <returns></returns>
		public int GetLayerID(int aHeaderRID, string aPackName)
		{
			return GetLayerID(aHeaderRID, eComponentType.SpecificPack, this.GetPackRID(aHeaderRID, aPackName));
		}
		/// <summary>
		/// Gets a layer ID for the given header and component.
		/// </summary>
		/// <param name="aHeaderRID">RID of the header</param>
		/// <param name="aComponentType">Component type</param>
		/// <param name="aComponentRID">RID of the color or pack component (ignored when not a color or pack)</param>
		/// <returns></returns>
		public int GetLayerID(int aHeaderRID, eComponentType aComponentType, int aComponentRID)
		{
			int layerID = 0;
			DataTable dt = this.GetLayers(aHeaderRID, aComponentType, aComponentRID);
			if (dt.Rows.Count > 0)
			{
				foreach(System.Data.DataRow dr in dt.Rows)
				{
					int layer = Convert.ToInt32(dr["LAYER_ID"], CultureInfo.CurrentUICulture);
					if (layerID < layer)
					{
						layerID = layer;
					}
				}
			}
			layerID++;
			return layerID; 
		}

        ///// <summary>
        ///// Determines if the layer ID has been defined 
        ///// </summary>
        ///// <param name="SQLCommand">An SQL Command that checks the existence of a layer.</param>
        ///// <returns></returns>
        //public bool LayerExists(string SQLCommand)
        //{
        //    return (_dba.ExecuteRecordCount( SQLCommand ) > 0);
        //}

        ///// <summary>
        ///// Determines if a row exists on database.
        ///// </summary>
        ///// <param name="SQLCommand"></param>
        ///// <returns></returns>
        //public bool RowExists(string SQLCommand)
        //{
        //    return (_dba.ExecuteRecordCount( SQLCommand ) > 0);
        //}

//        /// <summary>
//        /// Writes Rule Layer ID to database
//        /// </summary>
//        /// <param name="aMethodRID">RID of the Method that is creating this rule layer.</param>
//        /// <param name="aHeaderRID">RID of the header where the rule is being attached.</param>
//        /// <param name="aComponentType">Identifies the component where the rule is being attached.</param>
//        /// <param name="aComponentRID">For color and packs, the RID of the color or pack.</param>
//        /// <param name="aLayerID">Layer ID</param>
//        /// <returns>True: if update is successful; False: if update fails</returns>
//        public bool WriteRuleLayerID
//            (
//            int aMethodRID,
//            int aHeaderRID,
//            eComponentType aComponentType,
//            int aComponentRID,
//            int aLayerID
//            )
//        {
//            string layerTableName = this.GetLayerTableName(aComponentType);
//            switch (aComponentType)
//            {
//                case (eComponentType.Total):
//                case (eComponentType.Bulk):
//                case (eComponentType.DetailType):
//                {
//                    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
//                    if (this.LayerExists("select count(*) MyCount from " + layerTableName + " where HDR_RID = " + aHeaderRID.ToString(CultureInfo.CurrentUICulture) +
//                        " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture)))
//                    // end MID Track # 2354
//                    {
//                        string SQLCommand = "update " + layerTableName + " set " +
//// (CSMITH) Beg MID Track #2548: Rules always saved
//                            "	METHOD_RID = @METHOD_RID" + 
//// (CSMITH) END MID Track #2548
//                            "   where HDR_RID = @HDR_RID and LAYER_ID = @LAYER_ID";

//                        MIDDbParameter[] InParams = {   new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input)
//                                                 };
//                        return (_dba.ExecuteNonQuery( SQLCommand, InParams ) > 0);
//                    }
//                    else
//                    {
//                        string SQLCommand = "insert into " + layerTableName + "( " +
//                            "	HDR_RID, LAYER_ID, METHOD_RID )" +
//                            "   values( " +
//                            "	@HDR_RID, @LAYER_ID, @METHOD_RID )";

//                        MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input)
//                                                 };
//                        return (_dba.ExecuteNonQuery( SQLCommand, InParams ) > 0);
//                    }
//                }
//                case (eComponentType.SpecificColor):
//                {
//                    // Assortment BEGIN
//                    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
////                    if (this.LayerExists("select count(*) MyCount from " + layerTableName + " where HDR_RID = " + aHeaderRID.ToString(CultureInfo.CurrentUICulture) +
////                        " and COLOR_CODE_RID = " + aComponentRID.ToString(CultureInfo.CurrentUICulture) +						
////                        " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture)))
////                    // end MID Track # 2354
////                    {
////                        string SQLCommand = "update " + layerTableName + " set " +
////// (CSMITH) Beg MID Track #2548: Rules always saved
////                            "	METHOD_RID = @METHOD_RID" + 
////// (CSMITH) END MID Track #2548
////                            "   where HDR_RID = @HDR_RID and LAYER_ID = @LAYER_ID and COLOR_CODE_RID = @COLOR_CODE_RID ";

////                        MIDDbParameter[] InParams = {   new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input),
////                                                     new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
////                                                     new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
////                                                     new MIDDbParameter("@COLOR_CODE_RID", aComponentRID, eDbType.Int, eParameterDirection.Input)
////                                                 };
////                        return (_dba.ExecuteNonQuery( SQLCommand, InParams ) > 0);
////                    }
////                    else
////                    {
////                        string SQLCommand = "insert into " + layerTableName + "( " +
////                            "	HDR_RID, LAYER_ID, COLOR_CODE_RID, METHOD_RID )" +
////                            "   values( " +
////                            "	@HDR_RID, @LAYER_ID, @COLOR_CODE_RID, @METHOD_RID )";

////                        MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
////                                                     new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
////                                                     new MIDDbParameter("@COLOR_CODE_RID", aComponentRID, eDbType.Int, eParameterDirection.Input),
////                                                     new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input)
////                                                 };
////                        return (_dba.ExecuteNonQuery( SQLCommand, InParams ) > 0);
////                    }
//                    int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);
//                    if (this.LayerExists("select count(*) MyCount from " + layerTableName + " where HDR_BC_RID = " + hdrBCRID.ToString(CultureInfo.CurrentUICulture) +
//                        " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture)))
//                    {
//                        string SQLCommand = "update " + layerTableName + " set " +
//                            "	METHOD_RID = @METHOD_RID" +
//                            "   where HDR_BC_RID = @HDR_BC_RID and LAYER_ID = @LAYER_ID ";

//                        MIDDbParameter[] InParams = {   new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@HDR_BC_RID", hdrBCRID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input) 
//                                                 };
//                        return (_dba.ExecuteNonQuery(SQLCommand, InParams) > 0);
//                    }
//                    else
//                    {
//                        string SQLCommand = "insert into " + layerTableName + "( " +
//                            "	HDR_RID, HDR_BC_RID, LAYER_ID, METHOD_RID )" +          // MID Track 6166 Null Reference when using Color Minimum Rule
//                            "   values( " +
//                            "	@HDR_RID, @HDR_BC_RID, @LAYER_ID, @METHOD_RID )";       // MID Track 6166 Null Reference when using Color Minimum Rule

//                         MIDDbParameter[] InParams = { new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),   // MID Track 6166 Null Reference when using Color Minimum Rule
//                                                     new MIDDbParameter("@HDR_BC_RID", hdrBCRID, eDbType.Int, eParameterDirection.Input),    // MID Track 6166 Null Reference when using color Minimum Rule 
//                                                     new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input)
//                                                 };
//                        return (_dba.ExecuteNonQuery(SQLCommand, InParams) > 0);
//                    }
//                    // Assortment END
//                }
//                case (eComponentType.SpecificPack):
//                {
//                    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
//                    if (this.LayerExists("select count(*) MyCount from " + layerTableName + " where HDR_PACK_RID = " + aComponentRID.ToString(CultureInfo.CurrentUICulture) +
//                        " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture)))
//                    // end MID Track # 2354
//                    {
//                        string SQLCommand = "update " + layerTableName + " set " +
//// (CSMITH) Beg MID Track #2548: Rules always saved
//                            "	METHOD_RID = @METHOD_RID" + 
//// (CSMITH) END MID Track #2548
//                            "   where HDR_PACK_RID = @HDR_PACK_RID and LAYER_ID = @LAYER_ID ";

//                        MIDDbParameter[] InParams = {   new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@HDR_PACK_RID", aComponentRID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input)
//                                                 };
//                        return (_dba.ExecuteNonQuery( SQLCommand, InParams ) > 0);
//                    }
//                    else
//                    {
//                        string SQLCommand = "insert into " + layerTableName + "( " +
//                            "	HDR_PACK_RID, LAYER_ID, METHOD_RID )" +
//                            "   values( " +
//                            "	@HDR_PACK_RID, @LAYER_ID, @METHOD_RID )";

//                        MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_PACK_RID", aComponentRID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
//                                                     new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input)
//                                                 };
//                        return (_dba.ExecuteNonQuery( SQLCommand, InParams ) > 0);
//                    }
//                }
//                default:
//                {
//                    throw new Exception ("Unknown Component Type while processing Rule Layer");
//                }
//            }

//        }

        /// <summary>
        /// Writes Rule Layer ID to database
        /// </summary>
        /// <param name="aMethodRID">RID of the Method that is creating this rule layer.</param>
        /// <param name="aHeaderRID">RID of the header where the rule is being attached.</param>
        /// <param name="aComponentType">Identifies the component where the rule is being attached.</param>
        /// <param name="aComponentRID">For color and packs, the RID of the color or pack.</param>
        /// <param name="aLayerID">Layer ID</param>
        /// <returns>True: if update is successful; False: if update fails</returns>
        public bool WriteRuleLayerID(int aMethodRID, int aHeaderRID, eComponentType aComponentType, int aComponentRID, int aLayerID)
        {
            //string layerTableName = this.GetLayerTableName(aComponentType);
            switch (aComponentType)
            {
                case (eComponentType.Bulk):
                {
                    int layerCount = StoredProcedures.MID_BULK_RULE_LAYER_READ_COUNT.ReadRecordCount(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                    bool layerExists = (layerCount > 0);
                    if (layerExists) //Update layer
                    {
                        int rowsUpdated = StoredProcedures.MID_BULK_RULE_LAYER_UPDATE.Update(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, METHOD_RID: aMethodRID);
                        return (rowsUpdated > 0);
                    }
                    else //Insert layer
                    {
                        int rowsInserted = StoredProcedures.MID_BULK_RULE_LAYER_INSERT.Insert(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, METHOD_RID: aMethodRID);
                        return (rowsInserted > 0);
                    }
                }
                case (eComponentType.Total):
                {
                    int layerCount = StoredProcedures.MID_TOTAL_RULE_LAYER_READ_COUNT.ReadRecordCount(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                    bool layerExists = (layerCount > 0);
                    if (layerExists) //Update layer
                    {
                        int rowsUpdated = StoredProcedures.MID_TOTAL_RULE_LAYER_UPDATE.Update(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, METHOD_RID: aMethodRID);
                        return (rowsUpdated > 0);
                    }
                    else //Insert layer
                    {
                        int rowsInserted = StoredProcedures.MID_TOTAL_RULE_LAYER_INSERT.Insert(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, METHOD_RID: aMethodRID);
                        return (rowsInserted > 0);
                    }
                }
                case (eComponentType.DetailType):
                {
                    int layerCount = StoredProcedures.MID_DETAIL_RULE_LAYER_READ_COUNT.ReadRecordCount(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                    bool layerExists = (layerCount > 0);
                    if (layerExists) //Update layer
                    {
                        int rowsUpdated = StoredProcedures.MID_DETAIL_RULE_LAYER_UPDATE.Update(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, METHOD_RID: aMethodRID);
                        return (rowsUpdated > 0);
                    }
                    else //Insert layer
                    {
                        int rowsInserted = StoredProcedures.MID_DETAIL_RULE_LAYER_INSERT.Insert(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, METHOD_RID: aMethodRID);
                        return (rowsInserted > 0);
                    }
                }
                case (eComponentType.SpecificColor):
                {
                    int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);

                    int layerCount = StoredProcedures.MID_BULK_COLOR_RULE_LAYER_READ_COUNT.ReadRecordCount(_dba, HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID);
                    bool layerExists = (layerCount > 0);
                    if (layerExists) //Update layer
                    {
                        int rowsUpdated = StoredProcedures.MID_BULK_COLOR_RULE_LAYER_UPDATE.Update(_dba, HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID, METHOD_RID: aMethodRID);
                        return (rowsUpdated > 0);
                    }
                    else //Insert layer
                    {
                        int rowsInserted = StoredProcedures.MID_BULK_COLOR_RULE_LAYER_INSERT.Insert(_dba, aHeaderRID, HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID, METHOD_RID: aMethodRID);
                        return (rowsInserted > 0);
                    }
                }
                case (eComponentType.SpecificPack):
                {
                    int layerCount = StoredProcedures.MID_PACK_RULE_LAYER_READ_COUNT.ReadRecordCount(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID);
                    bool layerExists = (layerCount > 0);
                    if (layerExists) //Update layer
                    {
                        int rowsUpdated = StoredProcedures.MID_PACK_RULE_LAYER_UPDATE.Update(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID, METHOD_RID: aMethodRID);
                        return (rowsUpdated > 0);
                    }
                    else //Insert layer
                    {
                        int rowsInserted = StoredProcedures.MID_PACK_RULE_LAYER_INSERT.Insert(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID, METHOD_RID: aMethodRID);
                        return (rowsInserted > 0);
                    }
                }
                default:
                {
                    throw new Exception("Unknown Component Type while processing Rule Layer");
                }
            }

        }

        ///// <summary>
        ///// Writes the Rule Allocation By Store (quantity allocated by a rule and the rule type).
        ///// </summary>
        ///// <param name="aHeaderRID">RID that identifies the header being allocated.</param>
        ///// <param name="aComponentType">Identifies the component of the header being allocated</param>
        ///// <param name="aComponentRID">RID of the color or pack being allocated (ignored when not a pack or color component)</param>
        ///// <param name="aLayerID">Layer ID</param>
        ///// <param name="aStoreRID">RID of the store being allocated</param>
        ///// <param name="aRuleType">Rule Type</param>
        ///// <param name="aRuleQtyAllocated">Quantity allocated.</param>
        ///// <returns>True: indicates write was successful; False: indicates write was unsuccessful.</returns>
        //public bool WriteRuleAllocationByStore
        //    (
        //    int aHeaderRID,
        //    eComponentType aComponentType,
        //    int aComponentRID,
        //    int aLayerID,
        //    int aStoreRID,
        //    eRuleType aRuleType,
        //    int aRuleQtyAllocated
        //    )
        //{
        //    string ruleTableName = this.GetRuleTableName(aComponentType);
        //    string layerTableName = this.GetLayerTableName(aComponentType);
        //    switch (aComponentType)
        //    {
        //        case (eComponentType.Bulk):
        //        case (eComponentType.Total):
        //        case (eComponentType.DetailType):
        //            {
        //                // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //                if (!this.LayerExists("select count(*) MyCount from " + layerTableName + " where HDR_RID = " + aHeaderRID.ToString(CultureInfo.CurrentUICulture) +
        //                    " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture)))
        //                // end MID Track # 2354
        //                {
        //                    throw new Exception("Layer = " + aLayerID.ToString(CultureInfo.CurrentUICulture) + " must exist before adding Rule Allocations by Store to " + ruleTableName);
        //                }
        //                // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //                if (this.RowExists("select count(*) MyCount from " + ruleTableName + " where HDR_RID = " + aHeaderRID.ToString(CultureInfo.CurrentUICulture) +
        //                    " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture) +
        //                    " and ST_RID = " + aStoreRID.ToString(CultureInfo.CurrentUICulture)))
        //                // end MID Track # 2354
        //                {
        //                    string SQLCommand = "update " + ruleTableName + " set " +
        //                        "	RULE_TYPE_ID = @RULE_TYPE_ID, UNITS = @UNITS" +
        //                        "   where HDR_RID = @HDR_RID and LAYER_ID = @LAYER_ID and ST_RID = @ST_RID";
        //                    MIDDbParameter[] InParams = {   new MIDDbParameter("@RULE_TYPE_ID", (int)aRuleType, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@UNITS", aRuleQtyAllocated, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@ST_RID", aStoreRID, eDbType.Int, eParameterDirection.Input)
        //                                         };
        //                    return (_dba.ExecuteNonQuery(SQLCommand, InParams) > 0);
        //                }
        //                else
        //                {
        //                    string SQLCommand = "insert into " + ruleTableName + "( " +
        //                        "	HDR_RID, LAYER_ID, ST_RID, RULE_TYPE_ID, UNITS )" +
        //                        "   values( " +
        //                        "	@HDR_RID, @LAYER_ID, @ST_RID, @RULE_TYPE_ID, @UNITS )";

        //                    MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@ST_RID", aStoreRID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@RULE_TYPE_ID", (int)aRuleType, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@UNITS", aRuleQtyAllocated, eDbType.Int, eParameterDirection.Input)
        //                                         };
        //                    return (_dba.ExecuteNonQuery(SQLCommand, InParams) > 0);
        //                }
        //            }
        //        case (eComponentType.SpecificColor):
        //            {
        //                // Assortment BEGIN
        //                //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //                //if (!this.LayerExists("select count(*) MyCount from " + layerTableName + " where HDR_RID = " + aHeaderRID.ToString(CultureInfo.CurrentUICulture) +
        //                //    " and COLOR_CODE_RID = " + aComponentRID.ToString(CultureInfo.CurrentUICulture) +
        //                //    " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture)))
        //                //// end MID Track # 2354
        //                //{
        //                //    throw new Exception("Layer = " + aLayerID.ToString(CultureInfo.CurrentUICulture) + " must exist before adding Rule Allocations by Store to " + ruleTableName);
        //                //}
        //                //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //                //if (this.RowExists("select count(*) MyCount from " + ruleTableName + " where HDR_RID = " + aHeaderRID.ToString(CultureInfo.CurrentUICulture) + 
        //                //    " and COLOR_CODE_RID = " + aComponentRID.ToString(CultureInfo.CurrentUICulture) +
        //                //    " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture) +				
        //                //    " and ST_RID = " + aStoreRID.ToString(CultureInfo.CurrentUICulture)))
        //                //// end MID Track # 2354
        //                //{
        //                //    string SQLCommand = "update " + ruleTableName + " set " +
        //                //        "	RULE_TYPE_ID = @RULE_TYPE_ID, UNITS = @UNITS" + 
        //                //        "   where HDR_RID = @HDR_RID and COLOR_CODE_RID = @COLOR_CODE_RID and LAYER_ID = @LAYER_ID and ST_RID = @ST_RID";

        //                //    MIDDbParameter[] InParams = {   new MIDDbParameter("@RULE_TYPE_ID", (int)aRuleType, eDbType.Int, eParameterDirection.Input),
        //                //                                 new MIDDbParameter("@UNITS", aRuleQtyAllocated, eDbType.Int, eParameterDirection.Input),
        //                //                                 new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
        //                //                                 new MIDDbParameter("@COLOR_CODE_RID", aComponentRID, eDbType.Int, eParameterDirection.Input),
        //                //                                 new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
        //                //                                 new MIDDbParameter("@ST_RID", aStoreRID, eDbType.Int, eParameterDirection.Input)
        //                //                             };
        //                //    return (_dba.ExecuteNonQuery( SQLCommand, InParams ) > 0);
        //                //}
        //                //else
        //                //{
        //                //    string SQLCommand = "insert into " + ruleTableName + "( " +
        //                //        "	HDR_RID, COLOR_CODE_RID, LAYER_ID, ST_RID, RULE_TYPE_ID, UNITS )" +
        //                //        "   values( " +
        //                //        "	@HDR_RID, @COLOR_CODE_RID, @LAYER_ID, @ST_RID, @RULE_TYPE_ID, @UNITS )";

        //                //    MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
        //                //                                 new MIDDbParameter("@COLOR_CODE_RID", aComponentRID, eDbType.Int, eParameterDirection.Input),
        //                //                                 new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
        //                //                                 new MIDDbParameter("@ST_RID", aStoreRID, eDbType.Int, eParameterDirection.Input),
        //                //                                 new MIDDbParameter("@RULE_TYPE_ID", (int)aRuleType, eDbType.Int, eParameterDirection.Input),
        //                //                                 new MIDDbParameter("@UNITS", aRuleQtyAllocated, eDbType.Int, eParameterDirection.Input)
        //                //                             };
        //                //    return (_dba.ExecuteNonQuery( SQLCommand, InParams ) > 0);
        //                //}	
        //                int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);
        //                if (!this.LayerExists("select count(*) MyCount from " + layerTableName + " where HDR_BC_RID = " + hdrBCRID.ToString(CultureInfo.CurrentUICulture) +
        //                    " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture)))
        //                {
        //                    throw new Exception("Layer = " + aLayerID.ToString(CultureInfo.CurrentUICulture) + " must exist before adding Rule Allocations by Store to " + ruleTableName);
        //                }

        //                if (this.RowExists("select count(*) MyCount from " + ruleTableName + " where HDR_BC_RID = " + hdrBCRID.ToString(CultureInfo.CurrentUICulture) +
        //                    " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture) +
        //                    " and ST_RID = " + aStoreRID.ToString(CultureInfo.CurrentUICulture)))
        //                {
        //                    string SQLCommand = "update " + ruleTableName + " set " +
        //                        "	RULE_TYPE_ID = @RULE_TYPE_ID, UNITS = @UNITS" +
        //                        "   where HDR_BC_RID = @HDR_BC_RID and LAYER_ID = @LAYER_ID and ST_RID = @ST_RID";

        //                    MIDDbParameter[] InParams = {   new MIDDbParameter("@RULE_TYPE_ID", (int)aRuleType, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@UNITS", aRuleQtyAllocated, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@HDR_BC_RID", hdrBCRID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@ST_RID", aStoreRID, eDbType.Int, eParameterDirection.Input)
        //                                         };
        //                    return (_dba.ExecuteNonQuery(SQLCommand, InParams) > 0);
        //                }
        //                else
        //                {
        //                    string SQLCommand = "insert into " + ruleTableName + "( " +
        //                        "	HDR_BC_RID, LAYER_ID, ST_RID, RULE_TYPE_ID, UNITS )" +
        //                        "   values( " +
        //                        "	@HDR_BC_RID, @LAYER_ID, @ST_RID, @RULE_TYPE_ID, @UNITS )";

        //                    MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_BC_RID", hdrBCRID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@ST_RID", aStoreRID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@RULE_TYPE_ID", (int)aRuleType, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@UNITS", aRuleQtyAllocated, eDbType.Int, eParameterDirection.Input)
        //                                         };
        //                    return (_dba.ExecuteNonQuery(SQLCommand, InParams) > 0);
        //                }
        //                // Assortment END 
        //            }
        //        case (eComponentType.SpecificPack):
        //            {
        //                // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //                if (!this.LayerExists("select count(*) MyCount from " + layerTableName + " where HDR_PACK_RID = " + aComponentRID.ToString(CultureInfo.CurrentUICulture) +
        //                    " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture)))
        //                // end MID Track # 2354
        //                {
        //                    throw new Exception("Layer = " + aLayerID.ToString(CultureInfo.CurrentUICulture) + " must exist before adding Rule Allocations by Store to " + ruleTableName);
        //                }
        //                // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //                if (this.RowExists("select count(*) MyCount from " + ruleTableName + " where HDR_PACK_RID = " + aComponentRID.ToString(CultureInfo.CurrentUICulture) +
        //                    " and LAYER_ID = " + aLayerID.ToString(CultureInfo.CurrentUICulture) +
        //                    " and ST_RID = " + aStoreRID.ToString(CultureInfo.CurrentUICulture)))
        //                // end MID Track # 2354
        //                {
        //                    string SQLCommand = "update " + ruleTableName + " set " +
        //                        "	RULE_TYPE_ID = @RULE_TYPE_ID, PACKS = @PACKS" +
        //                        "   where HDR_PACK_RID = @HDR_PACK_RID and LAYER_ID = @LAYER_ID and ST_RID = @ST_RID";

        //                    MIDDbParameter[] InParams = {   new MIDDbParameter("@RULE_TYPE_ID", (int)aRuleType, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@PACKS", aRuleQtyAllocated, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@HDR_PACK_RID", aComponentRID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@ST_RID", aStoreRID, eDbType.Int, eParameterDirection.Input)
        //                                         };
        //                    return (_dba.ExecuteNonQuery(SQLCommand, InParams) > 0);
        //                }
        //                else
        //                {
        //                    string SQLCommand = "insert into " + ruleTableName + "( " +
        //                        "	HDR_PACK_RID, LAYER_ID, ST_RID, RULE_TYPE_ID, PACKS )" +
        //                        "   values( " +
        //                        "	@HDR_PACK_RID, @LAYER_ID, @ST_RID, @RULE_TYPE_ID, @PACKS )";

        //                    MIDDbParameter[] InParams = { 	 new MIDDbParameter("@HDR_PACK_RID", aComponentRID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@ST_RID", aStoreRID, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@RULE_TYPE_ID", (int)aRuleType, eDbType.Int, eParameterDirection.Input),
        //                                             new MIDDbParameter("@PACKS", aRuleQtyAllocated, eDbType.Int, eParameterDirection.Input)
        //                                         };
        //                    return (_dba.ExecuteNonQuery(SQLCommand, InParams) > 0);
        //                }
        //            }
        //        default:
        //            {
        //                throw new Exception("Unknown Component Type while processing Rule Layer");
        //            }
        //    }
        //}



        private string BuildLayerNotExistMessage(int aLayerID, string ruleTableName)
        {
            return "Layer = " + aLayerID.ToString(CultureInfo.CurrentUICulture) + " must exist before adding Rule Allocations by Store to " + ruleTableName;
        }

		/// <summary>
		/// Writes the Rule Allocation By Store (quantity allocated by a rule and the rule type).
		/// </summary>
		/// <param name="aHeaderRID">RID that identifies the header being allocated.</param>
		/// <param name="aComponentType">Identifies the component of the header being allocated</param>
		/// <param name="aComponentRID">RID of the color or pack being allocated (ignored when not a pack or color component)</param>
		/// <param name="aLayerID">Layer ID</param>
		/// <param name="aStoreRID">RID of the store being allocated</param>
		/// <param name="aRuleType">Rule Type</param>
		/// <param name="aRuleQtyAllocated">Quantity allocated.</param>
		/// <returns>True: indicates write was successful; False: indicates write was unsuccessful.</returns>
		public bool WriteRuleAllocationByStore(int aHeaderRID, eComponentType aComponentType, int aComponentRID, int aLayerID, int aStoreRID, eRuleType aRuleType, int aRuleQtyAllocated)
		{
			string ruleTableName = this.GetRuleTableName(aComponentType);

			switch (aComponentType)
			{
                case (eComponentType.Bulk):
                {
                    int layerCount = StoredProcedures.MID_BULK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                    bool layerExists = (layerCount > 0);
                    if (layerExists == false) throw new Exception(BuildLayerNotExistMessage(aLayerID, ruleTableName));               

                    int ruleCount = StoredProcedures.MID_BULK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, ST_RID: aStoreRID);
                    bool ruleExists = (ruleCount > 0);
                    if (ruleExists) //Update rule
                    {
                        int rowsUpdated = StoredProcedures.MID_BULK_RULE_UPDATE.Update(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, ST_RID: aStoreRID, RULE_TYPE_ID: (int)aRuleType, UNITS: aRuleQtyAllocated);
                        return (rowsUpdated > 0);
                    }
                    else //Insert rule
                    {
                        int rowsInserted = StoredProcedures.MID_BULK_RULE_INSERT.Insert(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, ST_RID: aStoreRID, RULE_TYPE_ID: (int)aRuleType, UNITS: aRuleQtyAllocated);
                        return (rowsInserted > 0);
                    }
                }
				case (eComponentType.Total):
                {
                    int layerCount = StoredProcedures.MID_TOTAL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                    bool layerExists = (layerCount > 0);
                    if (layerExists == false) throw new Exception(BuildLayerNotExistMessage(aLayerID, ruleTableName));     

                    int ruleCount = StoredProcedures.MID_TOTAL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, ST_RID: aStoreRID);
                    bool ruleExists = (ruleCount > 0);
                    if (ruleExists) //Update rule
                    {
                        int rowsUpdated = StoredProcedures.MID_TOTAL_RULE_UPDATE.Update(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, ST_RID: aStoreRID, RULE_TYPE_ID: (int)aRuleType, UNITS: aRuleQtyAllocated);
                        return (rowsUpdated > 0);
                    }
                    else //Insert rule
                    {
                        int rowsInserted = StoredProcedures.MID_TOTAL_RULE_INSERT.Insert(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, ST_RID: aStoreRID, RULE_TYPE_ID: (int)aRuleType, UNITS: aRuleQtyAllocated);
                        return (rowsInserted > 0);
                    }
                }
				case (eComponentType.DetailType):
				{
                    int layerCount = StoredProcedures.MID_DETAIL_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                    bool layerExists = (layerCount > 0);
                    if (layerExists == false) throw new Exception(BuildLayerNotExistMessage(aLayerID, ruleTableName));     

                    int ruleCount = StoredProcedures.MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, ST_RID: aStoreRID);
                    bool ruleExists = (ruleCount > 0);
                    if (ruleExists) //Update rule
                    {
                        int rowsUpdated = StoredProcedures.MID_DETAIL_RULE_UPDATE.Update(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, ST_RID: aStoreRID, RULE_TYPE_ID: (int)aRuleType, UNITS: aRuleQtyAllocated);
                        return (rowsUpdated > 0);
                    }
                    else //Insert rule
                    {
                        int rowsInserted = StoredProcedures.MID_DETAIL_RULE_INSERT.Insert(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID, ST_RID: aStoreRID, RULE_TYPE_ID: (int)aRuleType, UNITS: aRuleQtyAllocated);
                        return (rowsInserted > 0);
                    }
				}
				case (eComponentType.SpecificColor):
				{
                    int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);

                    int layerCount = StoredProcedures.MID_BULK_COLOR_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba,HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID);
                    bool layerExists = (layerCount > 0);
                    if (layerExists == false) throw new Exception(BuildLayerNotExistMessage(aLayerID, ruleTableName));     

                    int ruleCount = StoredProcedures.MID_BULK_COLOR_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba, HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID, ST_RID: aStoreRID);
                    bool ruleExists = (ruleCount > 0);
                    if (ruleExists) //Update rule
                    {
                        int rowsUpdated = StoredProcedures.MID_BULK_COLOR_RULE_UPDATE.Update(_dba, HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID, ST_RID: aStoreRID, RULE_TYPE_ID: (int)aRuleType, UNITS: aRuleQtyAllocated);
                        return (rowsUpdated > 0);
                    }
                    else //Insert rule
                    {
                        int rowsInserted = StoredProcedures.MID_BULK_COLOR_RULE_INSERT.Insert(_dba, aHeaderRID, HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID, ST_RID: aStoreRID, RULE_TYPE_ID: (int)aRuleType, UNITS: aRuleQtyAllocated);
                        return (rowsInserted > 0);
                    }
				}
				case (eComponentType.SpecificPack):
				{
                    int layerCount = StoredProcedures.MID_PACK_RULE_LAYER_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID);
                    bool layerExists = (layerCount > 0);
                    if (layerExists == false) throw new Exception(BuildLayerNotExistMessage(aLayerID, ruleTableName));  

                    int ruleCount = StoredProcedures.MID_PACK_RULE_READ_COUNT_FROM_HEADER_AND_LAYER.ReadRecordCount(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID, ST_RID: aStoreRID);
                    bool ruleExists = (ruleCount > 0);
                    if (ruleExists) //Update rule
                    {
                        int rowsUpdated = StoredProcedures.MID_PACK_RULE_UPDATE.Update(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID, ST_RID: aStoreRID, RULE_TYPE_ID: (int)aRuleType, PACKS: aRuleQtyAllocated);
                        return (rowsUpdated > 0);
                    }
                    else //Insert rule
                    {
                        int rowsInserted = StoredProcedures.MID_PACK_RULE_INSERT.Insert(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID, ST_RID: aStoreRID, RULE_TYPE_ID: (int)aRuleType, PACKS: aRuleQtyAllocated);
                        return (rowsInserted > 0);
                    }
				}
				default:
				{
					throw new Exception ("Unknown Component Type while processing Rule Layer");
				}
			}
		}

		/// <summary>
		/// Writes the Rule Allocation By Store (quantity allocated by a rule and the rule type).
		/// </summary>
		/// <param name="aHeaderRID">RID that identifies the header being allocated.</param>
		/// <param name="aComponentType">Identifies the component of the header being allocated</param>
		/// <param name="aComponentRID">RID of the color or pack being allocated (ignored when not a pack or color component)</param>
		/// <param name="aLayerID">Layer ID</param>
		/// <param name="aStoreRID">RID of the store being allocated</param>
		/// <param name="aRuleType">Rule Type</param>
		/// <param name="aRuleQtyAllocated">Quantity allocated.</param>
		/// <returns>True: indicates write was successful; False: indicates write was unsuccessful.</returns>
		public bool XMLWriteRuleAllocationByStore
			(
			int aHeaderRID, 
			eComponentType aComponentType, 
			int aComponentRID, 
			int aLayerID,
			int aStoreRID, 
			eRuleType aRuleType, 
			int aRuleQtyAllocated
			)
		{
			//string ruleTableName = this.GetRuleTableName(aComponentType);
			//string layerTableName = this.GetLayerTableName(aComponentType);
			switch (aComponentType)
			{
				case (eComponentType.Bulk):
				case (eComponentType.Total):
				case (eComponentType.DetailType):
				{
					++_recordsWritten;
                    // Begin TT#827-MD - JSmith - Allocation Reviews Performance
                    //// add store element with attributes
                    //_documentXML.Append(" <store STR_RID=\"");
                    //_documentXML.Append(aStoreRID.ToString(CultureInfo.CurrentCulture));
                    //_documentXML.Append("\" RULE_TYPE=\"");
                    //_documentXML.Append((Convert.ToInt32(aRuleType)).ToString(CultureInfo.CurrentCulture));
                    //_documentXML.Append("\" UNITS=\"");
                    //_documentXML.Append(aRuleQtyAllocated.ToString(CultureInfo.CurrentCulture));
                    //_documentXML.Append("\"> ");

                    //// terminate store element
                    //_documentXML.Append(" </store>");
                    //// return true for now until can change all

                    DataRow dr = _ruleTable.NewRow();
                    dr["ST_RID"] = aStoreRID;
                    dr["RULE_TYPE"] = Convert.ToInt32(aRuleType);
                    dr["UNITS"] = aRuleQtyAllocated;
                    _ruleTable.Rows.Add(dr);
                    // End TT#827-MD - JSmith - Allocation Reviews Performance

					return true;
				}
				case (eComponentType.SpecificColor):
				{
					++_recordsWritten;
                    // Begin TT#827-MD - JSmith - Allocation Reviews Performance
                    //// add store element with attributes
                    //_documentXML.Append(" <store STR_RID=\"");
                    //_documentXML.Append(aStoreRID.ToString(CultureInfo.CurrentCulture));
                    //_documentXML.Append("\" RULE_TYPE=\"");
                    //_documentXML.Append((Convert.ToInt32(aRuleType)).ToString(CultureInfo.CurrentCulture));
                    //_documentXML.Append("\" UNITS=\"");
                    //_documentXML.Append(aRuleQtyAllocated.ToString(CultureInfo.CurrentCulture));
                    //_documentXML.Append("\"> ");

                    //// terminate store element
                    //_documentXML.Append(" </store>");
                    //// return true for now until can change all

                    DataRow dr = _ruleTable.NewRow();
                    dr["ST_RID"] = aStoreRID;
                    dr["RULE_TYPE"] = Convert.ToInt32(aRuleType);
                    dr["UNITS"] = aRuleQtyAllocated;
                    _ruleTable.Rows.Add(dr);
                    // End TT#827-MD - JSmith - Allocation Reviews Performance
					return true;					
				}
				case (eComponentType.SpecificPack):
				{
					++_recordsWritten;
                    // Begin TT#827-MD - JSmith - Allocation Reviews Performance
                    //// add store element with attributes
                    //_documentXML.Append(" <store STR_RID=\"");
                    //_documentXML.Append(aStoreRID.ToString(CultureInfo.CurrentCulture));
                    //_documentXML.Append("\" RULE_TYPE=\"");
                    //_documentXML.Append((Convert.ToInt32(aRuleType)).ToString(CultureInfo.CurrentCulture));
                    //_documentXML.Append("\" PACKS=\"");
                    //_documentXML.Append(aRuleQtyAllocated.ToString(CultureInfo.CurrentCulture));
                    //_documentXML.Append("\"> ");

                    //// terminate store element
                    //_documentXML.Append(" </store>");
                    //// return true for now until can change all

                    DataRow dr = _ruleTable.NewRow();
                    dr["ST_RID"] = aStoreRID;
                    dr["RULE_TYPE"] = Convert.ToInt32(aRuleType);
                    dr["UNITS"] = aRuleQtyAllocated;
                    _ruleTable.Rows.Add(dr);
                    // End TT#827-MD - JSmith - Allocation Reviews Performance
					return true;
				}
				default:
				{
					throw new Exception ("Unknown Component Type while processing Rule Layer");
				}
			}
		}

		public void Rule_XMLWrite(
			int aHeaderRID, 
			eComponentType aComponentType, 
			int aComponentRID, 
			int aLayerID
			)
		{
			try
			{
				// only send document if values or flags were sent
				if (_recordsWritten > 0)
				{
					// terminate root element
                    // Begin TT#827-MD - JSmith - Allocation Reviews Performance
                    //_documentXML.Append(" </root>");
                    // ENd TT#827-MD - JSmith - Allocation Reviews Performance
					OpenUpdateConnection();
					switch (aComponentType)
					{
						case (eComponentType.Bulk):
						{
                            //// Begin TT#827-MD - JSmith - Allocation Reviews Performance
                            ////MIDDbParameter[] InParams  = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
                            ////                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            ////                              new MIDDbParameter("@xmlDoc", _documentXML.ToString(), eDbType.Text, eParameterDirection.Input)
                            ////                          };
                            //MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@RULE_TABLE", _ruleTable, eDbType.Structured, eParameterDirection.Input)
                            //                          };
                            //// End TT#827-MD - JSmith - Allocation Reviews Performance
					
                            //_dba.ExecuteStoredProcedure("dbo.SP_MID_XML_BULK_RULE_WRITE", InParams);
                            StoredProcedures.SP_MID_XML_BULK_RULE_WRITE.Insert(_dba,
                                                                               HDR_RID: aHeaderRID,
                                                                               LAYER_ID: aLayerID,
                                                                               RULE_TABLE: _ruleTable
                                                                               );
							break;
						}
						case (eComponentType.Total):
						{
                            //// Begin TT#827-MD - JSmith - Allocation Reviews Performance
                            ////MIDDbParameter[] InParams  = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
                            ////                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            ////                              new MIDDbParameter("@xmlDoc", _documentXML.ToString(), eDbType.Text, eParameterDirection.Input)
                            ////                          };
                            //MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@RULE_TABLE", _ruleTable, eDbType.Structured, eParameterDirection.Input)
                            //                          };
                            //// End TT#827-MD - JSmith - Allocation Reviews Performance
					
                            //_dba.ExecuteStoredProcedure("dbo.SP_MID_XML_TOTAL_RULE_WRITE", InParams);
                            StoredProcedures.SP_MID_XML_TOTAL_RULE_WRITE.Insert(_dba,
                                                                                HDR_RID: aHeaderRID,
                                                                                LAYER_ID: aLayerID,
                                                                                RULE_TABLE: _ruleTable
                                                                                );
							break;
						}
						case (eComponentType.DetailType):
						{
                            //// Begin TT#827-MD - JSmith - Allocation Reviews Performance
                            ////MIDDbParameter[] InParams  = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
                            ////                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            ////                              new MIDDbParameter("@xmlDoc", _documentXML.ToString(), eDbType.Text, eParameterDirection.Input)
                            ////                          };
                            //MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@RULE_TABLE", _ruleTable, eDbType.Structured, eParameterDirection.Input)
                            //                          };
                            //// End TT#827-MD - JSmith - Allocation Reviews Performance
					
                            //_dba.ExecuteStoredProcedure("dbo.SP_MID_XML_DETAIL_RULE_WRITE", InParams);
                            StoredProcedures.SP_MID_XML_DETAIL_RULE_WRITE.Insert(_dba,
                                                                                 HDR_RID: aHeaderRID,
                                                                                 LAYER_ID: aLayerID,
                                                                                 RULE_TABLE: _ruleTable
                                                                                 );
							break;
						}
						case (eComponentType.SpecificColor):
						{
                            // Assortment BEGIN
                            //MIDDbParameter[] InParams  = {   new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@COLOR_CODE_RID", aComponentRID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@xmlDoc", _documentXML.ToString(), eDbType.Text, eParameterDirection.Input)
                            //                          };
                            int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);
                            //// Begin TT#827-MD - JSmith - Allocation Reviews Performance
                            ////MIDDbParameter[] InParams = { new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),    // MID Track 6166 Null Reference on Color Min
                            ////                              new MIDDbParameter("@HDR_BC_RID", hdrBCRID, eDbType.Int, eParameterDirection.Input),   // MID Track 6166 Null Reference on Color Min
                            ////                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            ////                              new MIDDbParameter("@xmlDoc", _documentXML.ToString(), eDbType.Text, eParameterDirection.Input)
                            ////                          };
                            //MIDDbParameter[] InParams = { new MIDDbParameter("@HDR_RID", aHeaderRID, eDbType.Int, eParameterDirection.Input),    // MID Track 6166 Null Reference on Color Min
                            //                              new MIDDbParameter("@HDR_BC_RID", hdrBCRID, eDbType.Int, eParameterDirection.Input),   // MID Track 6166 Null Reference on Color Min
                            //                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@RULE_TABLE", _ruleTable, eDbType.Structured, eParameterDirection.Input)
                            //                          };
                            //// End TT#827-MD - JSmith - Allocation Reviews Performance
                            //// Assortment END
                            //_dba.ExecuteStoredProcedure("dbo.SP_MID_XML_COLOR_RULE_WRITE", InParams);
                            StoredProcedures.SP_MID_XML_COLOR_RULE_WRITE.Insert(_dba,
                                                                                HDR_RID: aHeaderRID,
                                                                                HDR_BC_RID: hdrBCRID,
                                                                                LAYER_ID: aLayerID,
                                                                                RULE_TABLE: _ruleTable
                                                                                );
							break;
						}
						case (eComponentType.SpecificPack):
						{
                            //// Begin TT#827-MD - JSmith - Allocation Reviews Performance
                            ////MIDDbParameter[] InParams  = {   new MIDDbParameter("@HDR_PACK_RID", aComponentRID, eDbType.Int, eParameterDirection.Input),
                            ////                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            ////                              new MIDDbParameter("@xmlDoc", _documentXML.ToString(), eDbType.Text, eParameterDirection.Input)
                            ////                          };
                            //MIDDbParameter[] InParams = {   new MIDDbParameter("@HDR_PACK_RID", aComponentRID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@LAYER_ID", aLayerID, eDbType.Int, eParameterDirection.Input),
                            //                              new MIDDbParameter("@RULE_TABLE", _ruleTable, eDbType.Structured, eParameterDirection.Input)
                            //                          };
                            //// End TT#827-MD - JSmith - Allocation Reviews Performance
					
                            //_dba.ExecuteStoredProcedure("dbo.SP_MID_XML_PACK_RULE_WRITE", InParams);
                            StoredProcedures.SP_MID_XML_PACK_RULE_WRITE.Insert(_dba,
                                                                               HDR_PACK_RID: aComponentRID,
                                                                               LAYER_ID: aLayerID,
                                                                               RULE_TABLE: _ruleTable
                                                                               );
							break;
						}
						default:
						{
							throw new Exception ("Unknown Component Type while processing Rule Layer");
						}
					}

					CommitData();
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				CloseUpdateConnection();
			}
		}

		/// <summary>
		/// Deletes a layer of store rules for the given header and component.
		/// </summary>
		/// <param name="aHeaderRID">RID that identifies the header</param>
		/// <param name="aComponentType">Identifies the component</param>
		/// <param name="aComponentRID">RID for the color or pack component when appropriate ("-1" is used for TOTAL, BULK, or DETAIL).</param>
		/// <param name="aLayerID">Layer ID to remove.</param>
		/// <returns>True: delete was successful; False: delete failed.</returns>
		public bool DeleteLayer(int aHeaderRID, eComponentType aComponentType, int aComponentRID, int aLayerID)
		{
			if (!this.DeleteRuleAllocation(aHeaderRID, aComponentType, aComponentRID, aLayerID))
			{
				return false;
			}
			//string layerTableName = this.GetLayerTableName(aComponentType);
			//string layerDeleteCommand;

            int rowsDeleted;
			switch (aComponentType)
			{
                case (eComponentType.Total):
                {
                    rowsDeleted = StoredProcedures.MID_TOTAL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER.Delete(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                    break;
                }
				case (eComponentType.Bulk):
                {
                    rowsDeleted = StoredProcedures.MID_BULK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER.Delete(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                    break;
                }
				case (eComponentType.DetailType):
				{
                    //layerDeleteCommand = "delete from " + layerTableName +
                    //    " where HDR_RID =" + aHeaderRID.ToString(CultureInfo.CurrentUICulture) +
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    rowsDeleted = StoredProcedures.MID_DETAIL_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER.Delete(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
					break;
				}
				case (eComponentType.SpecificColor):
				{
                    //// Assortment BEGIN 
                    ////layerDeleteCommand = "delete from " + layerTableName +
                    ////	" where HDR_RID =" + aHeaderRID.ToString(CultureInfo.CurrentUICulture) +
                    ////	" and COLOR_CODE_RID =" + aComponentRID.ToString(CultureInfo.CurrentUICulture) +
                    ////	" and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);
                    //layerDeleteCommand = "delete from " + layerTableName +
                    //    " where HDR_BC_RID =" + hdrBCRID.ToString(CultureInfo.CurrentUICulture) +
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    //// Assortment END 
                    rowsDeleted = StoredProcedures.MID_BULK_COLOR_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER.Delete(_dba, HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID);
					break;
				}
				case (eComponentType.SpecificPack):
				{
                    //layerDeleteCommand = "delete from " + layerTableName +
                    //    " where HDR_PACK_RID =" + aComponentRID.ToString(CultureInfo.CurrentUICulture) +
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    rowsDeleted = StoredProcedures.MID_PACK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER.Delete(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID);
					break;
				}
				default:
				{
					throw new Exception ("Rule Allocation Component must be TOTAL, DETAIL, specific Pack, BULK, or specific color");
				}
			}
			//return (_dba.ExecuteNonQuery( layerDeleteCommand ) > 0);
            return (rowsDeleted > 0);
		}
		
		/// <summary>
		/// Deletes a layer of store rules for the given header and component.
		/// </summary>
		/// <param name="aHeaderRID">RID that identifies the header</param>
		/// <param name="aComponentType">Identifies the component</param>
		/// <param name="aComponentRID">RID for the color or pack component when appropriate ("-1" is used for TOTAL, BULK, or DETAIL).</param>
		/// <param name="aLayerID">Layer ID to remove.</param>
		/// <returns>True: delete was successful; False: delete failed.</returns>
		public bool DeleteRuleAllocation(int aHeaderRID, eComponentType aComponentType, int aComponentRID, int aLayerID)
		{
			//string ruleTableName = this.GetRuleTableName(aComponentType);
			//string ruleDeleteCommand;

            int rowsDeleted;
			switch (aComponentType)
			{
                case (eComponentType.Total):
                {
                    rowsDeleted = StoredProcedures.MID_TOTAL_RULE_DELETE_FROM_HEADER_AND_LAYER.Delete(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                    break;
                }
                case (eComponentType.Bulk):
                {
                    rowsDeleted = StoredProcedures.MID_BULK_RULE_DELETE_FROM_HEADER_AND_LAYER.Delete(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
                    break;
                }
				case (eComponentType.DetailType):
				{
                    //ruleDeleteCommand = "delete from " + ruleTableName + 
                    //    " where HDR_RID =" + aHeaderRID.ToString(CultureInfo.CurrentUICulture) +
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    rowsDeleted = StoredProcedures.MID_DETAIL_RULE_DELETE_FROM_HEADER_AND_LAYER.Delete(_dba, HDR_RID: aHeaderRID, LAYER_ID: aLayerID);
					break;
				}
				case (eComponentType.SpecificColor):
				{
                    // Assortment BEGIN
                    //ruleDeleteCommand = "delete from " + ruleTableName +
                    //	" where HDR_RID =" + aHeaderRID.ToString(CultureInfo.CurrentUICulture) +
                    //	" and COLOR_CODE_RID =" + aComponentRID.ToString(CultureInfo.CurrentUICulture) +
                    //	" and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    int hdrBCRID = GetBulkColorRID(aHeaderRID, aComponentRID);
                    //ruleDeleteCommand = "delete from " + ruleTableName +
                    //    " where HDR_BC_RID =" + hdrBCRID.ToString(CultureInfo.CurrentUICulture) +
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    // Assortment END 
                    rowsDeleted = StoredProcedures.MID_BULK_COLOR_RULE_DELETE_FROM_HEADER_AND_LAYER.Delete(_dba, HDR_BC_RID: hdrBCRID, LAYER_ID: aLayerID);
					break;
				}
				case (eComponentType.SpecificPack):
				{
                    //ruleDeleteCommand = "delete from " + ruleTableName +
                    //    " where HDR_PACK_RID =" + aComponentRID.ToString(CultureInfo.CurrentUICulture) +
                    //    " and LAYER_ID =" + aLayerID.ToString(CultureInfo.CurrentUICulture);
                    rowsDeleted = StoredProcedures.MID_PACK_RULE_DELETE_FROM_HEADER_AND_LAYER.Delete(_dba, HDR_PACK_RID: aComponentRID, LAYER_ID: aLayerID);
					break;
				}
				default:
				{
					throw new Exception ("Rule Allocation Component must be TOTAL, DETAIL, specific Pack, BULK, or specific color");
				}
			}
			//return (_dba.ExecuteNonQuery( ruleDeleteCommand ) > 0);
            return (rowsDeleted > 0);
		}
	}
}
