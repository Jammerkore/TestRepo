using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for RollupData.
	/// </summary>
	public partial class RollupData : DataLayer
	{	
		private StringBuilder _documentXML;
		int _recordsWritten = 0;
		private Hashtable _rollupItems;
		
		public RollupData() : base()
		{
			_rollupItems = new Hashtable();
		}

		public int RecordsWritten
		{
			get
			{
				return _recordsWritten;
			}
		}

        // Begin TT#5485 - JSmith - Performance
        //public void BuildRollupItems(int aProcess, int aHierarchyRID, int aNodeRID, int aVersionRID,
        //    int aTimeID,int aRollupType, int aFromLevel, int aToLevel, int aFirstDayOfWeek, int aLastDayOfWeek,
        //    int aFirstDayOfNextWeek)
        public void BuildRollupItems(int aProcess, int aHierarchyRID, int aNodeRID, int aVersionRID,
            string aTimeID, int aRollupType, int aFromLevel, int aToLevel, int aFirstDayOfWeek, int aLastDayOfWeek,
            int aFirstDayOfNextWeek)
		// End TT#5485 - JSmith - Performance
		{
			try
			{
                StoredProcedures.SP_MID_BUILD_ROLLUP_ITEMS.Insert(_dba,
                                                                      PROCESS: aProcess,
                                                                      PH_RID: aHierarchyRID,
                                                                      HN_RID: aNodeRID,
                                                                      FV_RID: aVersionRID,
                                                                      TIME_ID: aTimeID,
                                                                      ITEM_TYPE: aRollupType,
                                                                      FROM_LEVEL: aFromLevel,
                                                                      TO_LEVEL: aToLevel,
                                                                      FIRST_DAY_OF_WEEK: aFirstDayOfWeek,
                                                                      LAST_DAY_OF_WEEK: aLastDayOfWeek,
                                                                      FIRST_DAY_OF_NEXT_WEEK: aFirstDayOfNextWeek
                                                                      );

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin Track #6107 - JSmith - Alternate not rolling correctly
		// Begin TT#5485 - JSmith - Performance
        //public void BuildAlternateRollupItems(int aProcess, int aHierarchyRID, int aNodeRID, int aVersionRID,
        //    int aTimeID, int aRollupType, int aFromLevel, int aToLevel, int aFirstDayOfWeek, int aLastDayOfWeek,
        //    int aFirstDayOfNextWeek)
        public void BuildAlternateRollupItems(int aProcess, int aHierarchyRID, int aNodeRID, int aVersionRID,
            string aTimeID, int aRollupType, int aFromLevel, int aToLevel, int aFirstDayOfWeek, int aLastDayOfWeek,
            int aFirstDayOfNextWeek)
		// End TT#5485 - JSmith - Performance
        {
            try
            {
                StoredProcedures.SP_MID_BUILD_ALT_ROLLUP_ITEMS.Insert(_dba,
                                                                          PROCESS: aProcess,
                                                                          PH_RID: aHierarchyRID,
                                                                          HN_RID: aNodeRID,
                                                                          FV_RID: aVersionRID,
                                                                          TIME_ID: aTimeID,
                                                                          ITEM_TYPE: aRollupType,
                                                                          FROM_LEVEL: aFromLevel,
                                                                          TO_LEVEL: aToLevel,
                                                                          FIRST_DAY_OF_WEEK: aFirstDayOfWeek,
                                                                          LAST_DAY_OF_WEEK: aLastDayOfWeek,
                                                                          FIRST_DAY_OF_NEXT_WEEK: aFirstDayOfNextWeek
                                                                          );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End Track #6107

		public void BuildRollupAncestors(int aProcess, bool aRollAlternatesOnly)
		{
			try
			{
				char alternatesOnly = 'N';
				if (aRollAlternatesOnly)
				{
					alternatesOnly = 'Y';
				}

				OpenUpdateConnection();
           
                StoredProcedures.SP_MID_BUILD_ROLLUP_HIER_ITEMS.Insert(_dba,
                                                                       PROCESS: aProcess,
                                                                       AlternatesOnly: alternatesOnly
                                                                       );
				CommitData();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				if (ConnectionIsOpen)
				{
					CloseUpdateConnection();
				}
			}
		}



		public bool DeleteProcessedRollupItems(bool aRollAlternatesOnly)
		{
			try
			{
				char alternatesOnly = 'N';
				if (aRollAlternatesOnly)
				{
					alternatesOnly = 'Y';
				}
                
                int rowsDeleted = StoredProcedures.MID_ROLLUP_ITEM_DELETE_PROCESSED_ITEMS.Delete(_dba, ALTERNATES_ONLY: alternatesOnly);
                return (rowsDeleted > 0);
			}
			catch 
			{
				throw;
			}
		}

        // Begin TT#410-MD - JSmith - Remove all items
        public bool DeleteRollupItems(int aProcess)
        {
            try
            {
                int rowsDeleted = StoredProcedures.MID_ROLLUP_ITEM_DELETE_FROM_PROCESS.Delete(_dba, PROCESS: aProcess);
                return (rowsDeleted > 0);
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteRollupItems(int aProcess, int aHierarchyRID)
        {
            try
            {
                int rowsDeleted = StoredProcedures.MID_ROLLUP_ITEM_DELETE.Delete(_dba,
                                                                                 PROCESS: aProcess,
                                                                                 PH_RID: aHierarchyRID
                                                                                 );
                return (rowsDeleted > 0);
            }
            catch
            {
                throw;
            }
        }
        // End TT#410-MD - JSmith - Remove all items

		public void Rollup_XMLInit()
		{
			try
			{
				_documentXML = new StringBuilder();
				// add root element
				_documentXML.Append("<root> ");
				_recordsWritten = 0;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Rollup_XMLInsert(int aProcess, int aNodeRID, int aType,
			int aTimeID, int aVersionRID, int aHierarchyRID, int aHierarchyLevel,
			int aFirstDayOfWeek, int aLastDayOfWeek, int aFirstDayOfNextWeek, bool aAlternatesOnly)
		{
			try
			{
				if (aVersionRID != Include.FV_ActualRID)
				{
					aAlternatesOnly = false;
				}

				if (!DuplicateRollupItem(aNodeRID, aType, aTimeID, aVersionRID))
				{
					++_recordsWritten;
					// add node element with attributes
					_documentXML.Append(" <node PROCESS=\"");
					_documentXML.Append(aProcess.ToString());
					_documentXML.Append("\" HN_RID=\"");
					_documentXML.Append(aNodeRID.ToString());
					_documentXML.Append("\" TIME_ID=\"");
					_documentXML.Append(aTimeID.ToString());
					_documentXML.Append("\" ITEM_TYPE=\"");
					_documentXML.Append(aType.ToString());
					_documentXML.Append("\" FV_RID=\"");
					_documentXML.Append(aVersionRID.ToString());
					_documentXML.Append("\" PH_RID=\"");
					_documentXML.Append(aHierarchyRID.ToString());
					_documentXML.Append("\" HOME_LEVEL=\"");
					_documentXML.Append(aHierarchyLevel.ToString());
					_documentXML.Append("\" FIRST_DAY=\"");
					_documentXML.Append(aFirstDayOfWeek.ToString());
					_documentXML.Append("\" LAST_DAY=\"");
					_documentXML.Append(aLastDayOfWeek.ToString());
					_documentXML.Append("\" FIRST_DAY_NEXT_WEEK=\"");
					_documentXML.Append(aFirstDayOfNextWeek.ToString());
					_documentXML.Append("\" ALTERNATES_ONLY=\"");
					if (aAlternatesOnly)
					{
						_documentXML.Append("Y");
					}
					else
					{
						_documentXML.Append("N");
					}
					_documentXML.Append("\"> ");

					// terminate node element
					_documentXML.Append(" </node>");
				}

				return;
			}
			catch
			{
				throw;
			}
		}

		private bool DuplicateRollupItem(int aNodeRID, int aType,
			int aTimeID, int aVersionRID)
		{
			try
			{
				string key = aNodeRID.ToString() + "|" + aType.ToString() + "|" + aTimeID.ToString() + "|" + aVersionRID.ToString();
				if (_rollupItems.Contains(key))
				{
					return true;
				}
				else
				{
					_rollupItems.Add(key, null);
					return false;
				}
			}
			catch
			{
				throw;
			}
		}

		
		public void Rollup_XMLWrite()
		{
			try
			{
				// only send document if values or flags were sent
				if (_recordsWritten > 0)
				{
					// terminate root element
					_documentXML.Append(" </root>");
                    StoredProcedures.SP_MID_XML_ROLLUP_ITEM_WRITE.Insert(_dba, xmlDoc: _documentXML.ToString());
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		

		public bool Rollup_Insert(int aProcess, int aNodeRID, int aType,
			int aTimeID, int aVersionRID, int aHierarchyRID, int aHierarchyLevel)
		{
			try
			{
                int rowsInserted = StoredProcedures.MID_ROLLUP_ITEM_INSERT.Insert(_dba,
                                                                                   PROCESS: aProcess,
                                                                                   HN_RID: aNodeRID,
                                                                                   PH_RID: aHierarchyRID,
                                                                                   HOME_LEVEL: aHierarchyLevel,
                                                                                   TIME_ID: aTimeID,
                                                                                   ITEM_TYPE: aType,
                                                                                   FV_RID: aVersionRID
                                                                                   );
                return (rowsInserted > 0);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public ArrayList GetRollupTypes(int aProcess, int aHierarchyRID)
		{
			try
			{
				ArrayList al = new ArrayList();
                //MID Track # 2354 - removed nolock because it causes concurrency issues 
                DataTable dt = StoredProcedures.MID_ROLLUP_ITEM_READ_TYPES.Read(_dba,
                                                                        PROCESS: aProcess,
                                                                        PH_RID: aHierarchyRID
                                                                        );
				foreach(DataRow dr in dt.Rows)
				{
					al.Add(Convert.ToInt32(dr["ITEM_TYPE"], CultureInfo.CurrentUICulture));
				}
				return al;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public ArrayList GetRollupVersions(int aProcess, int aHierarchyRID, int aRollupType)
		{
			try
			{
				ArrayList al = new ArrayList();
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                DataTable dt = StoredProcedures.MID_ROLLUP_ITEM_READ_VERSIONS.Read(_dba,
                                                                           PROCESS: aProcess,
                                                                           PH_RID: aHierarchyRID,
                                                                           ITEM_TYPE: aRollupType
                                                                           );
				foreach(DataRow dr in dt.Rows)
				{
					al.Add(Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture));
				}
				return al;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int GetMaxRollupLevel(int aProcess, int aHierarchyRID, int aRollupType, int aVersion)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues

                return (int)StoredProcedures.MID_ROLLUP_ITEM_READ_MAX_HOME_LEVEL.ReadValue(_dba,
                                                                                PROCESS: aProcess,
                                                                                PH_RID: aHierarchyRID,
                                                                                ITEM_TYPE: aRollupType,
                                                                                FV_RID: aVersion
                                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int GetMinRollupLevel(int aProcess, int aHierarchyRID, int aRollupType, int aVersion)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return (int)StoredProcedures.MID_ROLLUP_ITEM_READ_MIN_HOME_LEVEL.ReadValue(_dba,
                                                                                PROCESS: aProcess,
                                                                                PH_RID: aHierarchyRID,
                                                                                ITEM_TYPE: aRollupType,
                                                                                FV_RID: aVersion
                                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public int DetermineBatches(int aProcess, int aHierarchyRID, int aRollupType, int aVersion, int aLevel, int aBatchSize, int aNumberOfTables)
		{
			int batches = 0;
			try
			{
				OpenUpdateConnection();
				// update mod numbers
				try
				{
                    StoredProcedures.MID_ROLLUP_ITEM_UPDATE_MOD_NUMBERS.Update(_dba);
					CommitData();
				}
				catch
				{
					throw;
				}
				finally
				{
					if (ConnectionIsOpen)
					{
						CloseUpdateConnection();
					}
				}

				OpenUpdateConnection();
				// clear batch numbers
				try
				{
                    StoredProcedures.MID_ROLLUP_ITEM_UPDATE_FOR_CLEAR.Update(_dba,
                                                                         PROCESS: aProcess,
                                                                         FV_RID: aVersion,
                                                                         ITEM_TYPE: aRollupType,
                                                                         PH_RID: aHierarchyRID,
                                                                         HOME_LEVEL: aLevel
                                                                         );
					CommitData();
				}
				catch
				{
					throw;
				}
				finally
				{
					if (ConnectionIsOpen)
					{
						CloseUpdateConnection();
					}
				}

				for (int i = 0; i < aNumberOfTables; i++)
				{
					batches = DetermineBatches(aProcess, aHierarchyRID, aRollupType, aVersion, aLevel, 
						aBatchSize, i, batches);
				}

				return batches;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				if (ConnectionIsOpen)
				{
					CloseUpdateConnection();
				}
			}
		}

        private int DetermineBatches(int aProcess, int aHierarchyRID, int aRollupType, int aVersion, int aLevel, 
			int aBatchSize, int aTableNumber, int aBatchNumber)
		{
			int batches = 0;
			try
			{
				OpenUpdateConnection();
                batches = StoredProcedures.SP_MID_SET_ROLLUP_BATCHES.Update(_dba,
                                                                      PROCESS: aProcess,
                                                                      PH_RID: aHierarchyRID,
                                                                      HOME_LEVEL: aLevel,
                                                                      ITEM_TYPE: aRollupType,
                                                                      FV_RID: aVersion,
                                                                      BATCH_SIZE: aBatchSize,
                                                                      TABLE_NUMBER: aTableNumber,
                                                                      BATCH_NUMBER: aBatchNumber
                                                                      );
                //batches = (int)StoredProcedures.SP_MID_SET_ROLLUP_BATCHES.BATCH_COUNT.Value;
				CommitData();
				return batches;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				if (ConnectionIsOpen)
				{
					CloseUpdateConnection();
				}
			}
		}
		// Begin Track #5987

		public DataTable GetBatchesNumbers(int aProcess, int aHierarchyRID, int aRollupType, int aVersion, int aLevel)
		{
			try
			{
                return StoredProcedures.MID_ROLLUP_ITEM_READ_BATCH_NUMBERS.Read(_dba,
                                                                                PROCESS: aProcess,
                                                                                PH_RID: aHierarchyRID,
                                                                                FV_RID: aVersion,
                                                                                ITEM_TYPE: aRollupType,
                                                                                HOME_LEVEL: aLevel
                                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		// Begin MID Track #4730 - JSmith - roll size intransit to dummy color
		public void BuildDummyColorRollupItems(int aProcess)
		{
			try
			{
				OpenUpdateConnection();

                StoredProcedures.SP_MID_BUILD_ROLLUP_DCLR_ITEMS.Insert(_dba, PROCESS: aProcess);
				
				CommitData();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				if (ConnectionIsOpen)
				{
					CloseUpdateConnection();
				}
			}
		}
		// End MID Track #4730

		public int AddRollupProcess(int aProcessRID, eRollType aRollType, eProcessCompletionStatus aStatusCode, int aBatchNumber)
		{
			try
			{
                return StoredProcedures.SP_MID_ROLLUP_PROCESS_INSERT.InsertAndReturnRID(_dba,
                                                                                     PROCESS_RID: aProcessRID,
                                                                                     ROLLUP_TYPE: (int)aRollType,
                                                                                     STATUS_CODE: (int)aStatusCode,
                                                                                     BATCH_NUMBER: aBatchNumber,
                                                                                     START_TIME: DateTime.Now
                                                                                     );

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void UpdateRollupProcess(int aRollupRID, eProcessCompletionStatus aStatusCode)
		{
			try
			{
                StoredProcedures.MID_ROLLUP_PROCESS_UPDATE.Update(_dba,
                                                                  ROLLUP_RID: aRollupRID,
                                                                  STATUS_CODE: (int)aStatusCode,
                                                                  STOP_TIME: DateTime.Now
                                                                  );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ProcessRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aVersion, int aLevel,
			int aBatchNumber)
		{
			try
			{
//				int count = GetItemCount(aProcess, aHierarchyRID, aRollupType, aVersion, aLevel);
				eRollType rollupType = (eRollType) aRollupType;
//				while (count > 0)
//				{
					switch (rollupType)
					{
						case eRollType.chainWeeklyForecast:
							ProcessChainWeeklyForecastRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aVersion, aLevel, aBatchNumber);
							break;
						case eRollType.chainWeeklyHistory:
							ProcessChainWeeklyHistoryRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aLevel, aBatchNumber);
							break;
						case eRollType.storeDailyHistory:
							ProcessStoreDailyHistoryRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aLevel, aBatchNumber);
							break;
						case eRollType.storeExternalIntransit:
							ProcessStoreIntransitRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aLevel, aBatchNumber);
							break;
						case eRollType.storeIntransit:
							ProcessStoreIntransitRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aLevel, aBatchNumber);
							break;
						case eRollType.storeWeeklyForecast:
							ProcessStoreWeeklyForecastRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aVersion, aLevel, aBatchNumber);
							break;
						case eRollType.storeWeeklyHistory:
							ProcessStoreWeeklyHistoryRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aLevel, aBatchNumber);
							break;
						case eRollType.storeToChain:
							if (aVersion == Include.FV_ActualRID)
							{
								ProcessStoreChainWeeklyHistoryRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aLevel, aBatchNumber);
							}
							else
							{
								ProcessStoreChainWeeklyForecastRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aVersion, aLevel, aBatchNumber);
							}
							break;
						// Begin MID Track #4730 - JSmith - roll size intransit to dummy color
						case eRollType.dummyColor:
							ProcessDummyColorRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aLevel, aBatchNumber);
							break;
						// End MID Track #4730
						default:
							throw new Exception("Application error: Invalid Rollup type");
//							break;
					}
//					_dba.CommitData();
//					count = GetItemCount(aProcess, aHierarchyRID, aRollupType, Include.FV_ActualRID, aLevel);
//				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void ProcessChainWeeklyHistoryRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aLevel, int aBatchNumber)
		{
			try
			{
				MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@PH_RID", aHierarchyRID, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@HOME_LEVEL", aLevel, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@ITEM_TYPE", aRollupType, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@BATCH_NUMBER", aBatchNumber, eDbType.Int, eParameterDirection.Input)} ;
				
				_dba.ExecuteStoredProcedure(aStoredProcedureName, InParams);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void ProcessChainWeeklyForecastRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aVersion, int aLevel, int aBatchNumber)
		{
			try
			{
				MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@PH_RID", aHierarchyRID, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@HOME_LEVEL", aLevel, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@ITEM_TYPE", aRollupType, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@FV_RID", aVersion, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@BATCH_NUMBER", aBatchNumber, eDbType.Int, eParameterDirection.Input)} ;
	
				_dba.ExecuteStoredProcedure(aStoredProcedureName, InParams);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void ProcessStoreDailyHistoryRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aLevel, int aBatchNumber)
		{
			try
			{
				MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@PH_RID", aHierarchyRID, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@HOME_LEVEL", aLevel, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@ITEM_TYPE", aRollupType, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@BATCH_NUMBER", aBatchNumber, eDbType.Int, eParameterDirection.Input) } ;
	
				_dba.ExecuteStoredProcedure(aStoredProcedureName, InParams);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void ProcessStoreIntransitRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aLevel, int aBatchNumber)
		{
			try
			{
				MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@PH_RID", aHierarchyRID, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@HOME_LEVEL", aLevel, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@ITEM_TYPE", aRollupType, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@BATCH_NUMBER", aBatchNumber, eDbType.Int, eParameterDirection.Input) } ;
	
				_dba.ExecuteStoredProcedure(aStoredProcedureName, InParams);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void ProcessStoreWeeklyHistoryRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aLevel, int aBatchNumber)
		{
			try
			{
				MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@PH_RID", aHierarchyRID, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@HOME_LEVEL", aLevel, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@ITEM_TYPE", aRollupType, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@BATCH_NUMBER", aBatchNumber, eDbType.Int, eParameterDirection.Input) } ;

				_dba.ExecuteStoredProcedure(aStoredProcedureName, InParams);
	
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void ProcessStoreWeeklyForecastRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aVersion, int aLevel, int aBatchNumber)
		{
			try
			{
				MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@PH_RID", aHierarchyRID, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@HOME_LEVEL", aLevel, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@ITEM_TYPE", aRollupType, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@FV_RID", aVersion, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@BATCH_NUMBER", aBatchNumber, eDbType.Int, eParameterDirection.Input) } ;
	
				_dba.ExecuteStoredProcedure(aStoredProcedureName, InParams);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void ProcessStoreChainWeeklyHistoryRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aLevel, int aBatchNumber)
		{
			try
			{
				MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@PH_RID", aHierarchyRID, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@HOME_LEVEL", aLevel, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@ITEM_TYPE", aRollupType, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@BATCH_NUMBER", aBatchNumber, eDbType.Int, eParameterDirection.Input) } ;
	
				_dba.ExecuteStoredProcedure(aStoredProcedureName, InParams);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void ProcessStoreChainWeeklyForecastRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aVersion, int aLevel, int aBatchNumber)
		{
			try
			{
				MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@PH_RID", aHierarchyRID, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@HOME_LEVEL", aLevel, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@ITEM_TYPE", aRollupType, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@FV_RID", aVersion, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@BATCH_NUMBER", aBatchNumber, eDbType.Int, eParameterDirection.Input) } ;
	
				_dba.ExecuteStoredProcedure(aStoredProcedureName, InParams);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ProcessDaysToWeeksRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aVersion, int aLevel, int aBatchNumber)
		{
			try
			{
				eRollType rollupType = (eRollType) aRollupType;
				switch (rollupType)
				{
					case eRollType.storeDailyHistoryToWeeks:
						ProcessStoreDaysToWeeksHistoryRollup(aStoredProcedureName, aProcess, aHierarchyRID, aRollupType, aLevel, aBatchNumber);
						break;
					default:
						throw new Exception("Application error: Invalid Rollup type");
//						break;
				}

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void ProcessStoreDaysToWeeksHistoryRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aLevel, int aBatchNumber)
		{
			try
			{
				MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@PH_RID", aHierarchyRID, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@HOME_LEVEL", aLevel, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@ITEM_TYPE", aRollupType, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@BATCH_NUMBER", aBatchNumber, eDbType.Int, eParameterDirection.Input)} ;
				
				_dba.ExecuteStoredProcedure(aStoredProcedureName, InParams);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		// Begin MID Track #4730 - JSmith - roll size intransit to dummy color
		private void ProcessDummyColorRollup(string aStoredProcedureName, int aProcess, int aHierarchyRID, int aRollupType, int aLevel, int aBatchNumber)
		{
			try
			{
				MIDDbParameter[] InParams  = { new MIDDbParameter("@PROCESS", Convert.ToInt32(aProcess, CultureInfo.CurrentUICulture), eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@ITEM_TYPE", aRollupType, eDbType.Int, eParameterDirection.Input),
											  new MIDDbParameter("@BATCH_NUMBER", aBatchNumber, eDbType.Int, eParameterDirection.Input)} ;
				
				_dba.ExecuteStoredProcedure(aStoredProcedureName, InParams);

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		// End MID Track #4730

		public int GetItemCount(int aProcess, int aHierarchyRID)
		{
			try
			{
                return StoredProcedures.MID_ROLLUP_ITEM_READ_COUNT_NON_PROCESSED.ReadRecordCount(_dba,
                                                                                                 PROCESS: aProcess,
                                                                                                 PH_RID: aHierarchyRID
                                                                                                 );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int GetItemCount(int aProcess, int aHierarchyRID, int aRollupType, int aVersion, int aLevel)
		{
			try
			{
				//MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_ROLLUP_ITEM_READ_COUNT.ReadRecordCount(_dba,
                                                                                    PROCESS: aProcess,
                                                                                    PH_RID: aHierarchyRID,
                                                                                    FV_RID: aVersion,
                                                                                    ITEM_TYPE: aRollupType,
                                                                                    HOME_LEVEL: aLevel
                                                                                    );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

	}
}
