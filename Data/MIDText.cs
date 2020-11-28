// Begin TT#1159 - JSmith - Improve Messaging
// Restructured to use Dictionary instead of Hashtable for performance
// Too many changes to mark
// Removed old tags
// End TT#1159

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{

	public partial class MIDText
	{
		// Fields

		private static DatabaseAccess _dba;
        private static Dictionary<int, string> _dicText;
        private static Dictionary<int, DataTable> _dicTextDataTable;
        private static ReaderWriterLockSlim dictionaryLock;
        private static Dictionary<int, eMIDMessageLevel> _dicMsgLevel;

		static MIDText()
		{
			_dba = new DatabaseAccess();
            _dicText = new Dictionary<int, string>();
            _dicTextDataTable = new Dictionary<int, DataTable>();
            dictionaryLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;
            LoadLabels();
            _dicMsgLevel = new Dictionary<int, eMIDMessageLevel>();     
		}
		// Properties

		// Methods

		public static void OpenUpdateConnection()
		{
			_dba.OpenUpdateConnection();
		}

		public static void CommitData()
		{
			_dba.CommitData();
		}

		public static void CloseUpdateConnection()
		{
			_dba.CloseUpdateConnection();
		}

        public static void LoadLabels()
        {
            DataTable dt;
            string text;
            int textCode;
            try
            {
                using (new WriteLock(dictionaryLock)) 
                {
                    dt = StoredProcedures.MID_APPLICATION_TEXT_READ_ALL_LABELS.Read(_dba);

                    foreach (DataRow dr in dt.Rows)
                    {

                        textCode = Convert.ToInt32(dr["TEXT_CODE"]);
                        text = Convert.ToString(dr["TEXT_VALUE"]);
                        text = ReplaceControlChars(text);

                        if (text != null &&
                            text.Trim().Length > 0)
                        {
                            _dicText[textCode] = text;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static string GetText(eMIDTextCode textCode, params object[] args)
		{
            try
            {
                return GetText((int)textCode, args);
            }
            catch
            {
                throw;
            }
		}

        public static string GetText(int enumCode, params object[] args)
		{
            try
            {
                bool found = false;
                string text = null;
                using (new WriteLock(dictionaryLock))
                {
                    found = _dicText.TryGetValue(enumCode, out text);
                    if (!found || text == string.Empty)
                    {
                        text = (string)StoredProcedures.MID_APPLICATION_TEXT_READ_FROM_CODE.ReadValue(_dba, TEXT_CODE: enumCode);
                        text = ReplaceControlChars(text);

                        if (text == null ||
                            text.Trim().Length == 0)
                        {
                            return Include.ErrorBadTextCode + enumCode.ToString(CultureInfo.CurrentUICulture);
                        }
                        else
                        {
                            _dicText[enumCode] = text;
                        }
                    }
                }

                if (args != null && args.Length > 0)
                {
                    text = string.Format(text, args);
                }

                return enumCode.ToString(CultureInfo.CurrentUICulture) + ":" + text;
            }
            catch (MIDDatabaseUnavailableException err)
            {
                return err.Message;
            }
            catch
            {
                throw;
            }
		}

        public static string GetTextFromCode(int textCode)
        {
            //SqlParameter InParam = new SqlParameter();
            //InParam.Direction = ParameterDirection.Input;
            //InParam.ParameterName = "@TEXT_CODE";
            //InParam.SqlDbType = SqlDbType.Int;
            //InParam.Value = textCode;
            //List<SqlParameter> pList = new List<SqlParameter>();
            //pList.Add(InParam);
            //DataTable dt = ExecuteStoredProcedure("MID_APPLICATION_TEXT_READ_FROM_CODE", pList);
            //return (string)StoredProcedures.MID_APPLICATION_TEXT_READ_FROM_CODE.ReadValue(_dba, TEXT_CODE: textCode);
            try
            {
                string text = null;
                bool found = _dicText.TryGetValue(textCode, out text);
                if (!found || text == string.Empty)
                {
                    text = (string)StoredProcedures.MID_APPLICATION_TEXT_READ_FROM_CODE.ReadValue(_dba, TEXT_CODE: textCode);
                    text = ReplaceControlChars(text);

                    if (text == null ||
                        text.Trim().Length == 0)
                    {
                        return Include.ErrorBadTextCode + textCode.ToString(CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        using (new WriteLock(dictionaryLock))
                        {
                            _dicText[textCode] = text;
                        }
                        return text;
                    }
                }
                else
                {
                    return text;
                }
            }
            catch (MIDDatabaseUnavailableException err)
            {
                return err.Message;
            }
            catch
            {
                throw;
            }

        }

        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
        public static void PreLoadText()
        {

                using (new WriteLock(dictionaryLock))
                {

                    DataSet dsValues;
                    dsValues = StoredProcedures.MID_PRELOAD_APPLICATION_TEXT.ReadAsDataSet(_dba);


                    foreach (DataRow dr in dsValues.Tables[0].Rows)
                    {
                        int textCode = (int)dr["TEXT_CODE"];
                        string text = (string)dr["TEXT_VALUE"];
                        text = ReplaceControlChars(text);
                        if (text == null || text.Trim().Length == 0)
                        {
                            //return Include.ErrorBadTextCode + textCode.ToString(CultureInfo.CurrentUICulture);
                        }
                        else
                        {
                            _dicText[textCode] = text;
                        }
                    }

                    
                }
           
        }
        //End TT#827-MD -jsobek -Allocation Reviews Performance

        public static string GetTextOnly(eMIDTextCode textCode, params object[] args)
		{
            try
            {
                return GetTextOnly((int)textCode, args);
            }
            catch
            {
                throw;
            }
		}

        public static string GetTextOnly(int enumCode, params object[] args)
		{
            try
			{
                bool found = false;
                using (new WriteLock(dictionaryLock)) 
				{
					string text = null;
					found = _dicText.TryGetValue(enumCode, out text);
                    if (!found || text == string.Empty)
					{

                        text = (string)StoredProcedures.MID_APPLICATION_TEXT_READ_FROM_CODE.ReadValue(_dba, TEXT_CODE: enumCode);
						text = ReplaceControlChars(text);
						if (text == null ||
							text.Trim().Length == 0)
						{
							return Include.ErrorBadTextCode + enumCode.ToString(CultureInfo.CurrentUICulture);
						}
						else
						{
                            _dicText[enumCode] = text;
						}
					}

                    if (args != null && args.Length > 0)
                    {
                        text = string.Format(text, args);
                    }

					return text;
				}
			}
			catch(MIDDatabaseUnavailableException err)
			{
				return err.Message;
			}
			catch 
			{
				throw;
			}
		}

		public static DataTable GetMsg(eMIDTextCode textCode)
		{
            DataTable dt;
            try
            {
                 bool found = false;
                 using (new WriteLock(dictionaryLock))
                 {
                     found = _dicTextDataTable.TryGetValue((int)textCode, out dt);
                     if (!found || dt == null)
                     {
                         dt = StoredProcedures.MID_APPLICATION_TEXT_READ.Read(_dba, TEXT_CODE: (int)textCode);
                         if (dt.Rows.Count == 1)
                         {
                             _dicTextDataTable[Convert.ToInt32(textCode)] = dt;
                             _dicText[Convert.ToInt32(textCode)] = Convert.ToString(dt.Rows[0]["TEXT_VALUE"]);
                             _dicMsgLevel[Convert.ToInt32(textCode)] = (eMIDMessageLevel)dt.Rows[0]["TEXT_LEVEL"];
                         }
                     }
                     return dt;
                 }
            }
            catch
            {
                throw;
            }
		}

        public static void GetMsg(eMIDTextCode textCode, out string aTextValue, out eMIDMessageLevel aMessageLevel)
        {
            DataTable dt;
            try
            {
                bool found = false;
                using (new WriteLock(dictionaryLock))
                {
                    found = _dicText.TryGetValue((int)textCode, out aTextValue);
                    if (!found || aTextValue == string.Empty)
                    {
                        dt = StoredProcedures.MID_APPLICATION_TEXT_READ.Read(_dba, TEXT_CODE: (int)textCode);
                        if (dt.Rows.Count == 1)
                        {
                            aTextValue = Convert.ToString(dt.Rows[0]["TEXT_VALUE"]);
                            _dicText[Convert.ToInt32(textCode)] = aTextValue;
                            _dicMsgLevel[Convert.ToInt32(textCode)] = (eMIDMessageLevel)dt.Rows[0]["TEXT_LEVEL"];
                        }
                    }
                    found = _dicMsgLevel.TryGetValue((int)textCode, out aMessageLevel);
                }
            }
            catch
            {
                throw;
            }
        }

        public static DataTable GetTextType(eMIDTextType textType, eMIDTextOrderBy orderBy, params int[] exclude)
        {
            try
            {
                if (exclude != null && exclude.Length > 0)
                {
                    DataTable dtExcludeList = new DataTable();
                    dtExcludeList.Columns.Add("TEXT_CODE", typeof(int));
                    foreach (object value in exclude)
                    {
                        //ensure userRIDs are distinct, and only added to the datatable one time
                        if (dtExcludeList.Select("TEXT_CODE=" + value.ToString()).Length == 0)
                        {
                            DataRow dr = dtExcludeList.NewRow();
                            dr["TEXT_CODE"] = value.ToString();
                            dtExcludeList.Rows.Add(dr);
                        }
                    }

                    if (orderBy == eMIDTextOrderBy.TextCode)
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE.Read(_dba,
                                                                                                                       TEXT_TYPE: (int)textType,
                                                                                                                       TEXT_CODE_EXCLUDE_LIST: dtExcludeList
                                                                                                                       );
                    }
                    else
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE.Read(_dba,
                                                                                                                    TEXT_TYPE: (int)textType,
                                                                                                                    TEXT_CODE_EXCLUDE_LIST: dtExcludeList
                                                                                                                    );
                    }
                }
                else
                {
                    if (orderBy == eMIDTextOrderBy.TextCode)
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_CODE.Read(_dba, TEXT_TYPE: (int)textType);
                    }
                    else
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_INFO_FOR_TEXT_TYPE_SORTED_BY_VALUE.Read(_dba, TEXT_TYPE: (int)textType);
                    }
                }

            }
            catch
            {
                throw;
            }
        }

        //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        public static DataTable GetTextTypeValueFirst(eMIDTextType textType, eMIDTextOrderBy orderBy, params int[] exclude)
        {
            try
            {

                if (exclude != null && exclude.Length > 0)
                {
                    DataTable dtExcludeList = new DataTable();
                    dtExcludeList.Columns.Add("TEXT_CODE", typeof(int));
                    foreach (object value in exclude)
                    {
                        //ensure userRIDs are distinct, and only added to the datatable one time
                        if (dtExcludeList.Select("TEXT_CODE=" + value.ToString()).Length == 0)
                        {
                            DataRow dr = dtExcludeList.NewRow();
                            dr["TEXT_CODE"] = value.ToString();
                            dtExcludeList.Rows.Add(dr);
                        }
                    }

                    if (orderBy == eMIDTextOrderBy.TextCode)
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_CODE.Read(_dba,
                                                                                                                          TEXT_TYPE: (int)textType,
                                                                                                                          TEXT_CODE_EXCLUDE_LIST: dtExcludeList
                                                                                                                          );
                    }
                    else
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_VALUE_FIRST_SORTED_BY_VALUE.Read(_dba,
                                                                                                                           TEXT_TYPE: (int)textType,
                                                                                                                           TEXT_CODE_EXCLUDE_LIST: dtExcludeList
                                                                                                                           );
                    }
                }
                else
                {
                    if (orderBy == eMIDTextOrderBy.TextCode)
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_CODE.Read(_dba, TEXT_TYPE: (int)textType);
                    }
                    else
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_VALUE_FIRST_SORTED_BY_VALUE.Read(_dba, TEXT_TYPE: (int)textType);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis



        public static DataTable GetTextType(eMIDTextType textType, eMIDTextOrderBy orderBy, eMIDMessageLevel messageLevel, params int[] exclude)
        {
            try
            {

                if (exclude != null && exclude.Length > 0)
                {
                    DataTable dtExcludeList = new DataTable();
                    dtExcludeList.Columns.Add("TEXT_CODE", typeof(int));
                    foreach (object value in exclude)
                    {
                        //ensure userRIDs are distinct, and only added to the datatable one time
                        if (dtExcludeList.Select("TEXT_CODE=" + value.ToString()).Length == 0)
                        {
                            DataRow dr = dtExcludeList.NewRow();
                            dr["TEXT_CODE"] = value.ToString();
                            dtExcludeList.Rows.Add(dr);
                        }
                    }

                    if (orderBy == eMIDTextOrderBy.TextCode)
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_CODE.Read(_dba,
                                                                                                                  TEXT_TYPE: (int)textType,
                                                                                                                  TEXT_LEVEL: (int)messageLevel,
                                                                                                                  TEXT_CODE_EXCLUDE_LIST: dtExcludeList
                                                                                                                  );
                    }
                    else
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_EXCLUSION_SORTED_BY_VALUE.Read(_dba,
                                                                                                               TEXT_TYPE: (int)textType,
                                                                                                               TEXT_LEVEL: (int)messageLevel,
                                                                                                               TEXT_CODE_EXCLUDE_LIST: dtExcludeList
                                                                                                               );
                    }
                }
                else
                {
                    if (orderBy == eMIDTextOrderBy.TextCode)
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_CODE.Read(_dba,
                                                                                                        TEXT_TYPE: (int)textType,
                                                                                                        TEXT_LEVEL: (int)messageLevel
                                                                                                        );
                    }
                    else
                    {
                        return StoredProcedures.MID_APPLICATION_TEXT_READ_FOR_TEXT_TYPE_SORTED_BY_VALUE.Read(_dba,
                                                                                                     TEXT_TYPE: (int)textType,
                                                                                                     TEXT_LEVEL: (int)messageLevel
                                                                                                     );
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static eMIDMessageLevel GetMessageLevel(int enumCode)
        {
            try
            {
                bool found = false;
                DataTable dt;
                eMIDMessageLevel msgLevel;
                using (new WriteLock(dictionaryLock)) 
                {
                    found = _dicMsgLevel.TryGetValue(enumCode, out msgLevel);
                    if (!found)
                    {
                        dt = StoredProcedures.MID_APPLICATION_TEXT_READ_LEVEL.Read(_dba, TEXT_CODE: enumCode);
                        if (dt.Rows.Count == 1)
                        {
							// Begin TT#1455 - invalid cast exception
							if (dt.Rows[0]["TEXT_LEVEL"] == DBNull.Value)
							{
								msgLevel = eMIDMessageLevel.None;
							}
							else
							{
								msgLevel = (eMIDMessageLevel)dt.Rows[0]["TEXT_LEVEL"];
								_dicMsgLevel[enumCode] = msgLevel;
							}
							// End TT#1455 - invalid cast exception
                        }
                        else
                        {
                            msgLevel = eMIDMessageLevel.None;
                        }
                    }

                    return (eMIDMessageLevel)msgLevel;
                }
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Determine the message level to use for message
        /// </summary>
        /// <param name="enumCode">The text code</param>
        /// <param name="aDefaultMessageLevel">The message level if the text is not on the database</param>
        /// <returns></returns>
        public static eMIDMessageLevel GetMessageLevel(int enumCode, eMIDMessageLevel aDefaultMessageLevel)
        {
            try
            {
                bool found = false;
                DataTable dt;
                eMIDMessageLevel msgLevel;
                using (new WriteLock(dictionaryLock)) 
                {
                    found = _dicMsgLevel.TryGetValue(enumCode, out msgLevel);
                    if (!found)
                    {
                        dt = StoredProcedures.MID_APPLICATION_TEXT_READ_LEVEL.Read(_dba, TEXT_CODE: enumCode);
                        if (dt.Rows.Count == 1)
                        {
							// Begin TT#1455 - invalid cast exception
							if (dt.Rows[0]["TEXT_LEVEL"] == DBNull.Value)
							{
								msgLevel = eMIDMessageLevel.None;
							}
							else
							{
								msgLevel = (eMIDMessageLevel)dt.Rows[0]["TEXT_LEVEL"];
								_dicMsgLevel[enumCode] = msgLevel;
							}
							// End TT#1455 - invalid cast exception
                        }
                        else
                        {
                            msgLevel = eMIDMessageLevel.None;
                        }
                    }

                    if ((eMIDMessageLevel)msgLevel != eMIDMessageLevel.None)
                    {
                        return (eMIDMessageLevel)msgLevel;
                    }

                    return aDefaultMessageLevel;
                }
            }
            catch
            {
                throw;
            }
        }
        
		public static DataTable GetLabels(int beginValue, int endValue)
		{
			try
			{	
                return StoredProcedures.MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE_SORTED_BY_CODE.Read(_dba,
                                                                                                      TEXT_CODE_BEGIN_VALUE: beginValue,
                                                                                                      TEXT_CODE_END_VALUE: endValue
                                                                                                      );
			}
			catch  
			{
				throw;
			}
		}

        //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        public static DataTable GetLabelValuesInRange(int beginValue, int endValue)
        {
            try
            {
                return StoredProcedures.MID_APPLICATION_TEXT_READ_LABELS_IN_RANGE.Read(_dba,
                                                                                       TEXT_CODE_BEGIN_VALUE: beginValue,
                                                                                       TEXT_CODE_END_VALUE: endValue
                                                                                       );
            }
            catch
            {
                throw;
            }
        }
        //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis

		public static DataTable Read(eMIDTextOrderBy orderBy)
		{
			try
			{
                if (orderBy == eMIDTextOrderBy.TextCode)
                {
                    return StoredProcedures.MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_CODE.Read(_dba);
                }
                else
                {
                    return StoredProcedures.MID_APPLICATION_TEXT_READ_ALL_SORTED_BY_VALUE.Read(_dba);
                }
			}
			catch  
			{
				throw;
			}
		}

		public static bool Update(int aTextCode, string aTextValue, int aMessageLevel)
		{
			bool rc;

			try
			{
                int msgCount = StoredProcedures.MID_APPLICATION_TEXT_READ_COUNT_FROM_CODE.ReadRecordCount(_dba, TEXT_CODE: aTextCode);

                if (msgCount > 0)
				{
                    int rowsUpdated = StoredProcedures.MID_APPLICATION_TEXT_UPDATE.Update(_dba,
                                                                                          TEXT_CODE: aTextCode,
                                                                                          TEXT_VALUE: aTextValue,
                                                                                          TEXT_LEVEL: aMessageLevel
                                                                                          );
                    rc = (rowsUpdated > 0);
				}
				else
				{
                    StoredProcedures.SP_MID_SET_TEXT.Insert(_dba,
                                                            TEXT_CODE: aTextCode,
                                                            TEXT_VALUE: aTextValue,
                                                            TEXT_MAX_LENGTH: 250,
                                                            TEXT_TYPE: 0,
                                                            TEXT_LEVEL: aMessageLevel,
                                                            TEXT_VALUE_TYPE: null,
                                                            TEXT_ORDER: null
                                                            );
					rc = true;
				}

                using (new WriteLock(dictionaryLock))
                {
                    _dicText[aTextCode] = aTextValue;
                    _dicMsgLevel[aTextCode] = (eMIDMessageLevel)aMessageLevel;
                }

				return rc;
			}
			catch  
			{
				throw;
			}
            //finally		
            //{
            //    _dicText.Clear();
            //}			
		}

		public static string ReplaceControlChars(string aTextValue)
		{
			if (aTextValue != null)
			{
				aTextValue = aTextValue.Replace(@"\n", Environment.NewLine);
                aTextValue = aTextValue.Replace(@"{newline}", Environment.NewLine);
			}
			return aTextValue;
		}
	}
}
