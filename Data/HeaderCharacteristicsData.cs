using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data {

	public partial class HeaderCharacteristicsData : DataLayer {
		private DBAdapter _daHeaderCharGroup;
		private DBAdapter _daHeaderChar;
		private DBAdapter _daHeaderJoin;
		private DBAdapter _daAllHeaderChar;

		public HeaderCharacteristicsData() : base() 
        {

		}

        public int HeaderCharLoad(string CHARNAME, string VALUE_TEXT, double? VALUE_MONEY, double? VALUE_NUMBER, DateTime? VALUE_DTM, string PROCESSACTION, int HDR_RID)
		{
			int RetVal = 0;

			try
            {   // Begin TT#721 - RMatelic -Duplicate header characteristics:  moved connection process to calling datalock
                //base.OpenUpdateConnection();
                // End TT#721  
                RetVal = StoredProcedures.SP_MID_HEADER_CHAR_LOAD.UpdateWithReturnCode(_dba,
                                                               CHARNAME: CHARNAME,
                                                               VALUE_TEXT: VALUE_TEXT,
                                                               VALUE_MONEY: VALUE_MONEY,
                                                               VALUE_NUMBER: VALUE_NUMBER,
                                                               VALUE_DTM: VALUE_DTM,
                                                               PROCESSACTION: PROCESSACTION,
                                                               HDR_RID: HDR_RID
                                                               );
                
                // Begin TT#721 - RMatelic -Duplicate header characteristics:  moved connection process to calling datalock
                //base.CommitData();
                // End TT#721 

                //RetVal = StoredProcedures.MID_HEADER_CHAR_LOAD.InsertAndReturnRID(_dba,
                //                                                         CHARNAME: aCHARNAME,
                //                                                         VALUE_TEXT: aVALUE_TEXT,
                //                                                         VALUE_MONEY: aVALUE_MONEY,
                //                                                         VALUE_NUMBER: aVALUE_NUMBER,
                //                                                         VALUE_DTM: aVALUE_DTM,
                //                                                         PROCESSACTION: aPROCESSACTION,
                //                                                         HDR_RID: aHDR_RID
                //                                                         );
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				RetVal = -1;
				throw;
			}
            // Begin TT#721 - RMatelic -Duplicate header characteristics:  moved connection process to calling datalock
            //finally
            //{
            //    base.CloseUpdateConnection();
            //}
            // End TT#721
			return RetVal;
		}


		//public int HeaderCharDataType(MIDDbParameter[] inParams, MIDDbParameter[] outParams)
        public int HeaderCharDataType(string CHARNAME)
		{
            //			int Successfull = 0;

			try
			{
				base.OpenUpdateConnection();
                return StoredProcedures.SP_MID_HEADER_CHAR_DATA_TYPE.ReadType(_dba, CHARNAME: CHARNAME);
                //return (int)StoredProcedures.SP_MID_HEADER_CHAR_DATA_TYPE.HCG_TYPE.Value;

			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
                //				Successfull = -1;
				throw;
			}
			finally
			{
				base.CloseUpdateConnection();			
			}
            //			return Successfull;
		}


		public DataTable HeaderCharGroup_Read() 
		{
			try {
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_HEADER_CHAR_GROUP_READ_ALL.Read(_dba);
			}
			catch ( Exception err ) {
				string message = err.ToString();
				throw;
			}
		}

        public DataTable HeaderCharGroup_ReadValues(int hcgRID)
        {
            try
            {
                return StoredProcedures.MID_HEADER_CHAR_READ_FOR_GROUP_MAINT.Read(_dba, HCG_RID: hcgRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public bool DoesHeaderCharGroupNameAlreadyExist(string proposedName, int charGroupEditRID)
        {
            DataTable dt = StoredProcedures.MID_HEADER_CHAR_GROUP_READ_NAME_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_GROUP_NAME: proposedName,
                                                                                    HCG_EDIT_RID: charGroupEditRID);
            int matchCount = (int)dt.Rows[0]["MYCOUNT"];
            if (matchCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DoesHeaderCharValueTextAlreadyExist(string proposedText, int charGroupRID, int valueEditRID, ref int hcRID)
        {
            DataTable dt = StoredProcedures.MID_HEADER_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_VALUE: proposedText,
                                                                                    HCG_RID: charGroupRID,
                                                                                    HC_EDIT_RID: valueEditRID);

            hcRID = Include.NoRID;

            if (dt.Rows.Count > 0)
            {
                hcRID = (int)dt.Rows[0]["HC_RID"];
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DoesHeaderCharValueDateAlreadyExist(DateTime proposedValue, int charGroupRID, int valueEditRID, ref int hcRID)
        {
            DataTable dt = StoredProcedures.MID_HEADER_CHAR_READ_DATE_VALUE_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_VALUE: proposedValue,
                                                                                    HCG_RID: charGroupRID,
                                                                                    HC_EDIT_RID: valueEditRID);
            hcRID = Include.NoRID;

            if (dt.Rows.Count > 0)
            {
                hcRID = (int)dt.Rows[0]["HC_RID"];
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DoesHeaderCharValueNumberAlreadyExist(float proposedValue, int charGroupRID, int valueEditRID, ref int hcRID)
        {
            DataTable dt = StoredProcedures.MID_HEADER_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_VALUE: proposedValue,
                                                                                    HCG_RID: charGroupRID,
                                                                                    HC_EDIT_RID: valueEditRID);
            hcRID = Include.NoRID;

            if (dt.Rows.Count > 0)
            {
                hcRID = (int)dt.Rows[0]["HC_RID"];
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DoesHeaderCharValueDollarAlreadyExist(float proposedValue, int charGroupRID, int valueEditRID, ref int hcRID)
        {
            DataTable dt = StoredProcedures.MID_HEADER_CHAR_READ_DOLLAR_VALUE_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_VALUE: proposedValue,
                                                                                    HCG_RID: charGroupRID,
                                                                                    HC_EDIT_RID: valueEditRID);
            hcRID = Include.NoRID;

            if (dt.Rows.Count > 0)
            {
                hcRID = (int)dt.Rows[0]["HC_RID"];
                return true;
            }
            else
            {
                return false;
            }
        }

        public void HeaderCharGroup_Delete(int hcgRid)
        {
            try
            {
                _dba.OpenUpdateConnection();
                StoredProcedures.MID_HEADER_CHAR_GROUP_DELETE.Delete(_dba, HCG_RID: hcgRid);
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public void HeaderCharValue_Delete(int hcRid)
        {
            try
            {
                _dba.OpenUpdateConnection();
                StoredProcedures.MID_HEADER_CHAR_DELETE.Delete(_dba, HC_RID: hcRid);
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }


		// Begin - John Smith - Linked headers
		public DataTable HeaderCharGroup_Read(int aGroupRID) 
		{
			try 
			{
                return StoredProcedures.MID_HEADER_CHAR_GROUP_READ.Read(_dba, HCG_RID: aGroupRID);
			}
			catch ( Exception err ) 
			{
				string message = err.ToString();
				throw;
			}
		}
		// End - Linked headers

		//Begin TT#1252 - JScott - Duplicate key in Workspace when opening application
		public DataTable HeaderCharGroup_Read(string aGroupID)
		{
			try
			{
				aGroupID = aGroupID.Trim();
                return StoredProcedures.MID_HEADER_CHAR_GROUP_READ_FROM_ID.Read(_dba, HCG_ID: aGroupID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		//End TT#1252 - JScott - Duplicate key in Workspace when opening application
		// Begin TT#168 - RMatelic - Header characteristics Auto add 
        public bool HeaderCharGroup_Exists(string aGroupID)
        {
            try
            {
                aGroupID = aGroupID.Trim();
                int rowCount = StoredProcedures.MID_HEADER_CHAR_GROUP_READ_COUNT_FROM_ID.ReadRecordCount(_dba, HCG_ID: aGroupID);
                return (rowCount > 0);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int HeaderCharGroupInsert(string HCG_ID, int HCG_TYPE, char HCG_LIST_IND, char HCG_PROTECT_IND)
        {
            int RetVal = 0;

            try
            {   // Begin TT#721 - RMatelic -Duplicate header characteristics:  moved connection process to calling datalock
                //base.OpenUpdateConnection();
                // End TT#721  
                //RetVal = _dba.ExecuteStoredProcedure("SP_MID_HEADERCHARGROUP_INSERT", inParams, outParams);
                RetVal = StoredProcedures.SP_MID_HEADERCHARGROUP_INSERT.InsertAndReturnRID(_dba,
                                                                                           HCG_ID: HCG_ID,
                                                                                           HCG_TYPE: HCG_TYPE,
                                                                                           HCG_LIST_IND: HCG_LIST_IND,
                                                                                           HCG_PROTECT_IND: HCG_PROTECT_IND
                                                                                           );
                // Begin TT#721 - RMatelic -Duplicate header characteristics:  moved connection process to calling datalock
                //base.CommitData();
                // End TT#721 
            }
            catch (Exception e)
            {
                string exceptionMessage = e.Message;
                RetVal = -1;
                throw;
            }
            // Begin TT#721 - RMatelic -Duplicate header characteristics:  moved connection process to calling datalock
            //finally
            //{
            //    base.CloseUpdateConnection();
            //}
            // End TT#721
            return RetVal;
        }
        // End TT#168


        public int HeaderCharGroup_Insert(string charGroupName, eHeaderCharType aCharType, bool hasList, bool isProtected)
        {
            try
            {
                int hcgRid = -1;
                char cHasList = Include.ConvertBoolToChar(hasList);
                char cIsProtected = Include.ConvertBoolToChar(isProtected);


                _dba.OpenUpdateConnection();

                hcgRid = StoredProcedures.SP_MID_HEADERCHARGROUP_INSERT.InsertAndReturnRID(_dba,
                                                                            HCG_ID: charGroupName,
                                                                            HCG_TYPE: Convert.ToInt32(aCharType),
                                                                            HCG_LIST_IND: cHasList,
                                                                            HCG_PROTECT_IND: cIsProtected
                                                                            );
                _dba.CommitData();
                _dba.CloseUpdateConnection();
                return hcgRid;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public void HeaderCharGroup_Update(int scgRID, string charGroupName, eHeaderCharType aCharType, bool hasList, bool isProtected)
        {
            try
            {

                char cHasList = Include.ConvertBoolToChar(hasList);
                char cIsProtected = Include.ConvertBoolToChar(isProtected);

                _dba.OpenUpdateConnection();
                int rowsUpdated = StoredProcedures.MID_HEADER_CHAR_GROUP_UPDATE.Update(_dba,
                                                                                          HCG_RID: scgRID,
                                                                                          HCG_ID: charGroupName,
                                                                                          HCG_TYPE: Convert.ToInt32(aCharType),
                                                                                          HCG_LIST_IND: cHasList,
                                                                                          HCG_PROTECT_IND: cIsProtected
                                                                                          );
                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int HeaderCharValue_Insert(int hcgRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue)
        {
            try
            {
                int hcRid = Include.NoRID;
                _dba.OpenUpdateConnection();
                hcRid = StoredProcedures.SP_MID_HEADERCHAR_INSERT.InsertAndReturnRID(_dba,
                                                                            HCG_RID: hcgRID,
                                                                            TEXT_VALUE: textValue,
                                                                            DATE_VALUE: dateValue,
                                                                            NUMBER_VALUE: numberValue,
                                                                            DOLLAR_VALUE: dollarValue
                                                                            );

                _dba.CommitData();
                _dba.CloseUpdateConnection();
                return hcRid;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public void HeaderCharValue_Update(int hcRID, string textValue, DateTime? dateValue, float? numberValue, float? dollarValue)
        {
            try
            {
                _dba.OpenUpdateConnection();
                int rowsUpdate = StoredProcedures.MID_HEADER_CHAR_UPDATE.Update(_dba,
                                                                                HC_RID: hcRID,
                                                                                TEXT_VALUE: textValue,
                                                                                DATE_VALUE: dateValue,
                                                                                NUMBER_VALUE: numberValue,
                                                                                DOLLAR_VALUE: dollarValue
                                                                                );

                _dba.CommitData();
                _dba.CloseUpdateConnection();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //public DataSet HeaderCharGroup_ReadUsingAdapter(DataSet ds) {
        //    _daHeaderCharGroup = _dba.NewDBAdapter("HEADER_CHAR_GROUP");
        //    // Select
        //    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //    //string selectCommand = "select HCG_RID, HCG_ID, HCG_TYPE, HCG_LIST_IND, HCG_PROTECT_IND from HEADER_CHAR_GROUP order by 2";
        //    string selectCommand = "MID_HEADER_CHAR_GROUP_READ_ALL_FOR_MAINT";
        //    // end MID Track # 2354
        //    _daHeaderCharGroup.SelectCommand(selectCommand);
        //    // Insert	
        //    MIDDbParameter[] inParams = { new MIDDbParameter("@HCG_ID", eDbType.VarChar, 50, "HCG_ID", eParameterDirection.Input),
        //                                  new MIDDbParameter("@HCG_TYPE", eDbType.Single, 0, "HCG_TYPE", eParameterDirection.Input),
        //                                  new MIDDbParameter("@HCG_LIST_IND", eDbType.Char, 1, "HCG_LIST_IND", eParameterDirection.Input),
        //                                  new MIDDbParameter("@HCG_PROTECT_IND", eDbType.Char, 1, "HCG_PROTECT_IND", eParameterDirection.Input)};

        //    MIDDbParameter[] outParams = { new MIDDbParameter("@HCG_RID", eDbType.Single, 0, "HCG_RID") };
        //    outParams[0].Direction = eParameterDirection.Output;
        //    _daHeaderCharGroup.InsertCommand("SP_MID_HEADERCHARGROUP_INSERT", inParams, outParams);
        //    // Update
        //    string updateCommand = "UPDATE HEADER_CHAR_GROUP SET HCG_ID = @HCG_ID, " +
        //        " HCG_TYPE = @HCG_TYPE, HCG_LIST_IND = @HCG_LIST_IND, HCG_PROTECT_IND = @HCG_PROTECT_IND WHERE HCG_RID = @HCG_RID";
        //    MIDDbParameter[] inParams2 = {  new MIDDbParameter("@HCG_ID", eDbType.VarChar, 50, "HCG_ID", eParameterDirection.Input),
        //                                   new MIDDbParameter("@HCG_TYPE", eDbType.Single, 10, "HCG_TYPE", eParameterDirection.Input),
        //                                   new MIDDbParameter("@HCG_LIST_IND", eDbType.Char, 1, "HCG_LIST_IND", eParameterDirection.Input),
        //                                   new MIDDbParameter("@HCG_PROTECT_IND", eDbType.Char, 1, "HCG_PROTECT_IND", eParameterDirection.Input),
        //                                   new MIDDbParameter("@HCG_RID", eDbType.Single, 0, "HCG_RID", eParameterDirection.Input) };

        //    _daHeaderCharGroup.UpdateCommand(updateCommand, inParams2);
        //    // Delete
        //    string deleteCommand = "DELETE FROM HEADER_CHAR_GROUP WHERE HCG_RID = @HCG_RID";
        //    MIDDbParameter[] inParams3 = { new MIDDbParameter("@HCG_RID", eDbType.Int, 0, "HCG_RID") };
        //    inParams3[0].Direction = eParameterDirection.Input;
        //    _daHeaderCharGroup.DeleteCommand(deleteCommand, inParams3);

        //    _daHeaderCharGroup.Fill(ds);

        //    return ds;
           
        //}
		

        //public DataTable HeaderChar_Read() {
        //    try {
        //        //string SQLCommand = "select HC_RID, HCG_RID, TEXT_VALUE, ";
        //        //SQLCommand += "DATE_VALUE, NUMBER_VALUE, DOLLAR_VALUE ";
        //        //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //        //SQLCommand += "from HEADER_CHAR order by 2, 1";
        //        //// end MID Track # 2354
				
        //        //return _dba.ExecuteSQLQuery( SQLCommand, "Header Characteristic Values" );
        //        return StoredProcedures.MID_HEADER_CHAR_READ_ALL.Read(_dba);
        //    }
        //    catch ( Exception err ) {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}

		public DataTable HeaderCharList_Read()
		{
			try
			{
                return StoredProcedures.MID_HEADER_CHAR_READ_LIST.Read(_dba);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        //public DataSet HeaderChar_ReadUsingAdapter(DataSet ds) {
        //    _daHeaderChar = _dba.NewDBAdapter("HEADER_CHAR");
        //    // Select
        //    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //    string selectCommand = "select HC.HC_RID, HC.HCG_RID, HC.TEXT_VALUE, HC.DATE_VALUE, HC.NUMBER_VALUE, HC.DOLLAR_VALUE from HEADER_CHAR HC, HEADER_CHAR_GROUP HCG where HC.HCG_RID = HCG.HCG_RID and HCG.HCG_LIST_IND = 1 order by 2, 1";
        //    // end MID Track # 2354
        //    _daHeaderChar.SelectCommand(selectCommand);
        //    // Insert	
        //    MIDDbParameter[] inParams  = {  new MIDDbParameter("@HCG_RID", eDbType.Single, 0, "HCG_RID"),
        //                                  new MIDDbParameter("@TEXT_VALUE", eDbType.VarChar, 50, "TEXT_VALUE"),
        //                                  new MIDDbParameter("@DATE_VALUE", eDbType.DateTime, 0, "DATE_VALUE"),
        //                                  new MIDDbParameter("@NUMBER_VALUE", eDbType.Single, 0, "NUMBER_VALUE"),
        //                                  new MIDDbParameter("@DOLLAR_VALUE", eDbType.Single, 0, "DOLLAR_VALUE") };	  
									
        //    for (int i=0;i<5;i++) {
        //        inParams[i].Direction = eParameterDirection.Input;
        //    }

        //    MIDDbParameter[] outParams = { new MIDDbParameter("@HC_RID", eDbType.Single, 0, "HC_RID") };
        //    outParams[0].Direction = eParameterDirection.Output;
        //    _daHeaderChar.InsertCommand("SP_MID_HEADERCHAR_INSERT", inParams, outParams);
        //    // Update
        //    string updateCommand = "UPDATE HEADER_CHAR SET TEXT_VALUE = @TEXT_VALUE, " +
        //        " DATE_VALUE = @DATE_VALUE,	NUMBER_VALUE = @NUMBER_VALUE, DOLLAR_VALUE = @DOLLAR_VALUE " +
        //        "WHERE HC_RID = @HC_RID";
        //    MIDDbParameter[] inParams2  = {  new MIDDbParameter("@TEXT_VALUE", eDbType.VarChar, 50, "TEXT_VALUE"),
        //                                   new MIDDbParameter("@DATE_VALUE", eDbType.DateTime, 0, "DATE_VALUE"),
        //                                   new MIDDbParameter("@NUMBER_VALUE", eDbType.Single, 0, "NUMBER_VALUE"),
        //                                   new MIDDbParameter("@DOLLAR_VALUE", eDbType.Single, 0, "DOLLAR_VALUE"),
        //                                   new MIDDbParameter("@HC_RID", eDbType.Int, 0, "HC_RID") }; 		
        //    for (int i=0;i<5;i++) {
        //        inParams2[i].Direction = eParameterDirection.Input;
        //    }
        //    _daHeaderChar.UpdateCommand(updateCommand, inParams2);
        //    // Delete
        //    string deleteCommand = "DELETE FROM HEADER_CHAR WHERE HCG_RID = @HCG_RID and HC_RID = @HC_RID";
        //    MIDDbParameter[] inParams3  = { new MIDDbParameter("@HCG_RID", eDbType.Int, 0, "HCG_RID"),
        //                                   new MIDDbParameter("@HC_RID", eDbType.Int, 0, "HC_RID")};
        //    inParams3[0].Direction = eParameterDirection.Input;
        //    inParams3[1].Direction = eParameterDirection.Input;

        //    _daHeaderChar.DeleteCommand(deleteCommand, inParams3);

        //    _daHeaderChar.Fill( ds );

        //    return ds;
        //}

   
		public int HeaderCharInsert(int aHeaderCharGroupRID, eHeaderCharType aHeaderCharType, object aCharValue)
		{
			try
			{


                string TEXT_VALUE = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aHeaderCharType == eHeaderCharType.text) TEXT_VALUE = Convert.ToString(aCharValue, CultureInfo.CurrentUICulture);

                DateTime? DATE_VALUE = null;
                if (aHeaderCharType == eHeaderCharType.date) DATE_VALUE = Convert.ToDateTime(aCharValue, CultureInfo.CurrentUICulture);

                double? NUMBER_VALUE = null;
                if (aHeaderCharType == eHeaderCharType.number) NUMBER_VALUE = Convert.ToDouble(aCharValue, CultureInfo.CurrentUICulture);

                double? DOLLAR_VALUE = null;
                if (aHeaderCharType == eHeaderCharType.dollar) DOLLAR_VALUE = Convert.ToDouble(aCharValue, CultureInfo.CurrentUICulture);

                return StoredProcedures.SP_MID_HEADERCHAR_INSERT.InsertAndReturnRID(_dba,
                                                                                  HCG_RID: aHeaderCharGroupRID,
                                                                                  TEXT_VALUE: TEXT_VALUE,
                                                                                  DATE_VALUE: DATE_VALUE,
                                                                                  NUMBER_VALUE: NUMBER_VALUE,
                                                                                  DOLLAR_VALUE: DOLLAR_VALUE
                                                                                  );
			}
			catch
			{
				throw;
			}
		}

		public DataTable HeaderChar_Read(int aHeaderCharGroupRID, eHeaderCharType aHeaderCharType, object aCharValue)
		{
			try
			{
                

                if (aHeaderCharType == eHeaderCharType.text)
                {
                    return StoredProcedures.MID_HEADER_CHAR_READ_FROM_TEXT_VALUE.Read(_dba,
                                                                                      HCG_RID: aHeaderCharGroupRID,
                                                                                      TEXT_VALUE: Convert.ToString(aCharValue, CultureInfo.CurrentUICulture)
                                                                                      );
                }
                if (aHeaderCharType == eHeaderCharType.date)
                {
                    return StoredProcedures.MID_HEADER_CHAR_READ_FROM_DATE_VALUE.Read(_dba,
                                                                                  HCG_RID: aHeaderCharGroupRID,
                                                                                  DATE_VALUE: Convert.ToDateTime(aCharValue, CultureInfo.CurrentUICulture)
                                                                                  );
                }
                if (aHeaderCharType == eHeaderCharType.number)
                {
                    return StoredProcedures.MID_HEADER_CHAR_READ_FROM_NUMBER_VALUE.Read(_dba,
                                                                                    HCG_RID: aHeaderCharGroupRID,
                                                                                    NUMBER_VALUE: Convert.ToDouble(Convert.ToInt32(aCharValue, CultureInfo.CurrentUICulture), CultureInfo.CurrentUICulture)
                                                                                    );
                }
                if (aHeaderCharType == eHeaderCharType.dollar)
                {
                    return StoredProcedures.MID_HEADER_CHAR_READ_FROM_DOLLAR_VALUE.Read(_dba,
                                                                                    HCG_RID: aHeaderCharGroupRID,
                                                                                    DOLLAR_VALUE: Convert.ToDouble(Convert.ToInt32(aCharValue, CultureInfo.CurrentUICulture), CultureInfo.CurrentUICulture)
                                                                                    );
                }
                return StoredProcedures.MID_HEADER_CHAR_READ_FROM_KEY.Read(_dba, HCG_RID: aHeaderCharGroupRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}


        //public DataSet AllHeaderChar_ReadUsingAdapter(DataSet ds) {
        //    _daAllHeaderChar = _dba.NewDBAdapter("HEADER_CHAR");
        //    // Select
        //    // begin MID Track # 2354 - removed nolock because it causes concurrency issues
        //    string selectCommand = "select HC.HC_RID, HC.HCG_RID, HC.TEXT_VALUE, HC.DATE_VALUE, HC.NUMBER_VALUE, HC.DOLLAR_VALUE from HEADER_CHAR HC";
        //    // end MID Track # 2354
        //    _daAllHeaderChar.SelectCommand(selectCommand);
        //    // Insert	
        //    MIDDbParameter[] inParams  = {  new MIDDbParameter("@HCG_RID", eDbType.Single, 0, "HCG_RID"),
        //                                  new MIDDbParameter("@TEXT_VALUE", eDbType.VarChar, 50, "TEXT_VALUE"),
        //                                  new MIDDbParameter("@DATE_VALUE", eDbType.DateTime, 0, "DATE_VALUE"),
        //                                  new MIDDbParameter("@NUMBER_VALUE", eDbType.Single, 0, "NUMBER_VALUE"),
        //                                  new MIDDbParameter("@DOLLAR_VALUE", eDbType.Single, 0, "DOLLAR_VALUE") };	  
									
        //    for (int i=0;i<5;i++) {
        //        inParams[i].Direction = eParameterDirection.Input;
        //    }

        //    MIDDbParameter[] outParams = { new MIDDbParameter("@HC_RID", eDbType.Single, 0, "HC_RID") };
        //    outParams[0].Direction = eParameterDirection.Output;
        //    _daAllHeaderChar.InsertCommand("SP_MID_HEADERCHAR_INSERT", inParams, outParams);
        //    // Update
        //    string updateCommand = "UPDATE HEADER_CHAR SET TEXT_VALUE = @TEXT_VALUE, " +
        //        " DATE_VALUE = @DATE_VALUE,	NUMBER_VALUE = @NUMBER_VALUE, DOLLAR_VALUE = @DOLLAR_VALUE " +
        //        "WHERE HC_RID = @HC_RID";
        //    MIDDbParameter[] inParams2  = {  new MIDDbParameter("@TEXT_VALUE", eDbType.VarChar, 50, "TEXT_VALUE"),
        //                                   new MIDDbParameter("@DATE_VALUE", eDbType.DateTime, 0, "DATE_VALUE"),
        //                                   new MIDDbParameter("@NUMBER_VALUE", eDbType.Single, 0, "NUMBER_VALUE"),
        //                                   new MIDDbParameter("@DOLLAR_VALUE", eDbType.Single, 0, "DOLLAR_VALUE"),
        //                                   new MIDDbParameter("@HC_RID", eDbType.Single, 0, "HC_RID") }; 		
        //    for (int i=0;i<5;i++) {
        //        inParams2[i].Direction = eParameterDirection.Input;
        //    }
        //    _daAllHeaderChar.UpdateCommand(updateCommand, inParams2);
        //    // Delete
        //    string deleteCommand = "DELETE FROM HEADER_CHAR WHERE HCG_RID = @HCG_RID and HC_RID = @HC_RID";
        //    MIDDbParameter[] inParams3  = { new MIDDbParameter("@HCG_RID", eDbType.Int, 0, "HCG_RID"),
        //                                   new MIDDbParameter("@HC_RID", eDbType.Int, 0, "HC_RID")};
        //    inParams3[0].Direction = eParameterDirection.Input;
        //    inParams3[1].Direction = eParameterDirection.Input;

        //    _daAllHeaderChar.DeleteCommand(deleteCommand, inParams3);

        //    _daAllHeaderChar.Fill( ds );

        //    return ds;
        //}



		public DataTable HeaderJoin_Read(int aHeaderRID)
		{
			try
			{
                return StoredProcedures.MID_HEADER_CHAR_READ.Read(_dba, HDR_RID: aHeaderRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

     
		public void HeaderJoinInsert(int aHeaderRID, int aCharacteristicRID)
		{
            StoredProcedures.SP_MID_HEADERCHARJOIN_INSERT.Insert(_dba,
                                                                    HDR_RID: aHeaderRID,
                                                                    HC_RID: aCharacteristicRID
                                                                    );
		}

		public int deleteAllJoinsByHeaderID(int headerID) 
        {
            return StoredProcedures.MID_HEADER_CHAR_JOIN_DELETE.Delete(_dba, HDR_RID: headerID);
		}

		public int deleteAllJoinsByCharID(int charID) 
        {
            return StoredProcedures.MID_HEADER_CHAR_JOIN_DELETE_FROM_CHAR.Delete(_dba, HC_RID: charID);
		}

		public int deleteAllJoinsByCharGroupID(int groupCharID) 
        {

            return StoredProcedures.MID_HEADER_CHAR_JOIN_DELETE_FROM_GROUP.Delete(_dba, HCG_RID: groupCharID);
		}


        //public void HeaderCharGroup_UpdateUsingAdapter(DataTable xDataTable) 
        //{
        //    try {
        //        _daHeaderCharGroup.UpdateTable(xDataTable);
        //    }
        //    catch ( Exception err ) {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}

        //public void HeaderChar_UpdateUsingAdapter(DataTable xDataTable) 
        //{
        //    try {
        //        _daHeaderChar.UpdateTable(xDataTable);
        //    }
        //    catch ( Exception err ) {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}

        //public void AllHeaderChar_UpdateUsingAdapter(DataTable xDataTable) 
        //{
        //    try {
        //        _daAllHeaderChar.UpdateTable(xDataTable);
        //    }
        //    catch ( Exception err ) {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}

        //public void HeaderJoin_UpdateUsingAdapter(DataTable xDataTable) 
        //{
        //    try {
        //        _daHeaderJoin.UpdateTable(xDataTable);
        //    }
        //    catch ( Exception err ) {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
		// BEGIN MID Track #3978 - error deleting characteristic
		public bool HeadersExistForChar(int charRID)
		{
            int count = StoredProcedures.MID_HEADER_CHAR_JOIN_READ_COUNT.ReadRecordCount(_dba, HC_RID: charRID);
			return count > 0 ? true : false;
		}
		// END MID Track #3978  

        //Begin TT#1313-MD -jsobek -Header Filters
        public DataTable ReadHeaderCharacteristicsForHeaderRidList(DataTable dtHeaderList)
        {
            try
            {
                return StoredProcedures.MID_HEADER_CHAR_READ_FOR_HEADER_LIST.Read(_dba, HDR_RID_LIST: dtHeaderList);
            }
            catch
            {
                throw;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters
	}
}
