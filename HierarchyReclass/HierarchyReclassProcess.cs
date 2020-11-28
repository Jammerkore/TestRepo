using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.HierarchyReclass
{
	class HierarchyReclassProcess
	{
		const int cCommitLimit = 1000;
		const string cHierarchyOutFile = "HierarchyTrans";
        // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
        //const string cReclassOutFile = "ReclassTrans";
        const string cReclassMoveOutFile = "ReclassMoveTrans";
        const string cReclassDeleteOutFile = "ReclassDeleteTrans";
        const string cReclassRenameOutFile = "ReclassRenameTrans";
        // End TT#2186

        //BEGIN TT#3896-VStuart-Provide file name(s) in audit-MID
        const string sourceModule = "HierarchyReclassProcess.cs";
        //END TT#3896-VStuart-Provide file name(s) in audit-MID

		private SessionAddressBlock _SAB;
		private int _processId;
		private string _delimiter;
		private string _triggerSuffix;
        // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
        private string _reclassMoveTriggerSuffix;
        private string _reclassDeleteTriggerSuffix;
        private string _reclassRenameTriggerSuffix;
        // End TT#2186
		private string _outSuffix;
		private string _outputDirectory;
		private int _maxRecsPerFile;
		private bool _allowDeletes;
		private int _numRejectedRecs;
		private int _numHierarchyRecs;
		private int _numAddUpdateRecs;
		private int _numDeleteRecs;
		private int _numEditRecs;
		private Hashtable _hierarchyIdHash;
        // Begin TT#223 MD - JSmith - Add file order processsing (FIFO, LIFO)
        private int lineCount = 0;
        // End TT#223 MD
        // Begin TT#226 MD - JSmith - Options records not duplicated to each file
        private ArrayList _alOptionsRecords; 
        // End TT#226 MD

		private int _hierRecsWritten;
		private int _hierOutSeq;
		private string _hierOutName;
		private StreamWriter _hierOutFile;
	
		private int _reclassRecsWritten;
		private int _reclassOutSeq;
		private string _reclassOutName;
        private StreamWriter _reclassOutFile;
        // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
        private int _reclassDeleteRecsWritten;
        private int _reclassDeleteOutSeq;
        private string _reclassDeleteOutName;
        private StreamWriter _reclassDeleteOutFile;
        private int _reclassRenameRecsWritten;
        private int _reclassRenameOutSeq;
        private string _reclassRenameOutName;
        private StreamWriter _reclassRenameOutFile;
        // End TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files

		private MIDEnqueue _dlEnqueue;
		private HierarchyReclassData _dlHierReclass;

        // Begin TT#1921 - JSmith - Error during Hierarchy Reclass 
        private int _TRANS_ORIGINAL_Column_Size;
        private int _TRANS_HIERARCHY_ID_Column_Size;
        private int _TRANS_PARENT_ID_Column_Size;
        private int _TRANS_PRODUCT_ID_Column_Size;
        private int _TRANS_PRODUCT_NAME_Column_Size;
        private int _TRANS_PRODUCT_DESC_Column_Size;
        private string msg_DatabaseColumnSizeExceeded;
        // End TT#1921

		public int NumRejectedRecs
		{
			get
			{
				return _numRejectedRecs;
			}
		}

		public int NumHierarchyRecs
		{
			get
			{
				return _numHierarchyRecs;
			}
		}

		public int NumAddUpdateRecs
		{
			get
			{
				return _numAddUpdateRecs;
			}
		}

		public int NumDeleteRecs
		{
			get
			{
				return _numDeleteRecs;
			}
		}

		public int NumEditRecs
		{
			get
			{
				return _numEditRecs;
			}
		}

		public HierarchyReclassProcess(
			SessionAddressBlock aSAB,
			int aProcessId,
			string aDelimiter,
			string aTriggerSuffix,
            // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
            string aReclassMoveTriggerSuffix,
            string aReclassDeleteTriggerSuffix,
            string aReclassRenameTriggerSuffix,
            // End TT#2186
			string aOutSuffix,
			string aOutputDirectory,
			int aMaxRecsPerFile,
			bool aAllowDeletes)
		{
			_SAB = aSAB;
			_processId = aProcessId;
			_delimiter = aDelimiter;
			_triggerSuffix = aTriggerSuffix;
            // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
            _reclassMoveTriggerSuffix = aReclassMoveTriggerSuffix;
            _reclassDeleteTriggerSuffix = aReclassDeleteTriggerSuffix;
            _reclassRenameTriggerSuffix = aReclassRenameTriggerSuffix;
            // End TT#2186
			_outSuffix = aOutSuffix;
			_outputDirectory = aOutputDirectory;
			_maxRecsPerFile = aMaxRecsPerFile;
			_allowDeletes = aAllowDeletes;

			_hierRecsWritten = 0;
			_hierOutSeq = 0;
			_reclassRecsWritten = 0;
			_reclassOutSeq = 0;
			_numRejectedRecs = 0;
			_numHierarchyRecs = 0;
			_numAddUpdateRecs = 0;
			_numDeleteRecs = 0;
			_numEditRecs = 0;
			_hierarchyIdHash = new Hashtable();
		}

		public void Initialize()
		{
			try
			{
				_dlEnqueue = new MIDEnqueue();
				_dlHierReclass = new HierarchyReclassData();

				_dlEnqueue.OpenUpdateConnection();

				try
				{
                    // Begin TT#1921 - JSmith - Error during Hierarchy Reclass 
                    _TRANS_ORIGINAL_Column_Size = _dlEnqueue.GetColumnSize("HIERARCHY_RECLASS_TRANS", "TRANS_ORIGINAL");
                    _TRANS_HIERARCHY_ID_Column_Size = _dlEnqueue.GetColumnSize("HIERARCHY_RECLASS_TRANS", "TRANS_HIERARCHY_ID");
                    _TRANS_PARENT_ID_Column_Size = _dlEnqueue.GetColumnSize("HIERARCHY_RECLASS_TRANS", "TRANS_PARENT_ID");
                    _TRANS_PRODUCT_ID_Column_Size = _dlEnqueue.GetColumnSize("HIERARCHY_RECLASS_TRANS", "TRANS_PRODUCT_ID");
                    _TRANS_PRODUCT_NAME_Column_Size = _dlEnqueue.GetColumnSize("HIERARCHY_RECLASS_TRANS", "TRANS_PRODUCT_NAME");
                    _TRANS_PRODUCT_DESC_Column_Size = _dlEnqueue.GetColumnSize("HIERARCHY_RECLASS_TRANS", "TRANS_PRODUCT_DESC");
                    msg_DatabaseColumnSizeExceeded = MIDText.GetText(eMIDTextCode.msg_DatabaseColumnSizeExceeded);
                    // End TT#1921

					_dlEnqueue.Enqueue_Insert(eLockType.HierarchyReclass, _processId, Include.SystemUserRID, _processId);
					_dlEnqueue.CommitData();
				}
				catch (Exception exc)
				{
					_dlEnqueue.Rollback();
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in Initialize(): " + exc.Message, "HierarchyReclassProcess");
					throw;
				}
				finally
				{
					_dlEnqueue.CloseUpdateConnection();
				}
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in Initialize(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		public void Cleanup()
		{
			try
			{
				CloseHierarchyTransaction();
				CloseReclassTransaction();
                // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
                CloseReclassDeleteTransaction();
                CloseReclassRenameTransaction();
                // End TT#2186

				_dlEnqueue.OpenUpdateConnection();

				try
				{
					_dlEnqueue.Enqueue_Delete(eLockType.HierarchyReclass, _processId, Include.SystemUserRID);
					_dlEnqueue.CommitData();
				}
				catch (Exception exc)
				{
					_dlEnqueue.Rollback();
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in Cleanup(): " + exc.Message, "HierarchyReclassProcess");
					throw;
				}
				finally
				{
					_dlEnqueue.CloseUpdateConnection();
				}
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in Cleanup(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		public void ClearTransactionTable()
		{
			try
			{
				_dlHierReclass.OpenUpdateConnection();

				try
				{
					_dlHierReclass.HierReclassTrans_ClearTrans(_processId);
					_dlHierReclass.CommitData();
				}
				catch (Exception exc)
				{
					_dlHierReclass.Rollback();
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in ClearTransactionTable(): " + exc.Message, "HierarchyReclassProcess");
					throw;
				}
				finally
				{
					_dlHierReclass.CloseUpdateConnection();
				}
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in ClearTransactionTable(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		public void LoadXMLTransactionFile(string aInputFile)
		{
			XmlSerializer xmlSer;
			Hierarchies hierarchies;
			StreamReader reader;
            // Begin TT#223 MD - JSmith - Add file order processsing (FIFO, LIFO)
            //int lineCount;
            // End TT#223 MD
			int commitLimit;
			string inLine;
			string prodName = string.Empty;
			string prodDesc = string.Empty;

            //BEGIN TT#3896-VStuart-Provide file name(s) in audit-MID
            string message;
            //END TT#3896-VStuart-Provide file name(s) in audit-MID


			try
			{
                // Begin TT#226 MD - JSmith - Options records not duplicated to each file
                _alOptionsRecords = new ArrayList(); 
                // End TT#226 MD
				reader = new StreamReader(aInputFile);

                //BEGIN TT#3896-VStuart-Provide file name(s) in audit-MID
                message = aInputFile;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                //END TT#3896-VStuart-Provide file name(s) in audit-MID

				try
				{
					xmlSer = new XmlSerializer(typeof(Hierarchies));
					hierarchies = (Hierarchies)xmlSer.Deserialize(reader);
                }
				catch (Exception exc)
				{
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in LoadXMLTransactionFile(): " + exc.Message, "HierarchyReclassProcess");
					throw;
				}
				finally
				{
					reader.Close();
				}

				if (hierarchies != null)
				{
					_dlHierReclass.OpenUpdateConnection();

					try
					{
                        // Begin TT#223 MD - JSmith - Add file order processsing (FIFO, LIFO)
                        //lineCount = 0;
                        // End TT#223 MD
						commitLimit = 0;

						if (hierarchies.Options != null)
						{
							if (hierarchies.Options.ReclassPreview ||
								hierarchies.Options.RollExternalIntransit ||
								hierarchies.Options.RollAlternateHierarchies ||
                                hierarchies.Options.AllowDeletesSpecified || // TT#3342 - JSmith - Add AllowDeletes to Option record
								(hierarchies.Options.AutoAddCharacteristicsSpecified && hierarchies.Options.AutoAddCharacteristics))
							{
                                // Begin TT#226 MD - JSmith - Options records not duplicated to each file
                               
                                //WriteHierarchyTransaction("O" + _delimiter +
                                //                        "B" + _delimiter +
                                //                        FormatTransString(hierarchies.Options.ReclassPreview) + _delimiter +
                                //                        FormatTransString(hierarchies.Options.RollExternalIntransit) + _delimiter +
                                //                        FormatTransString(hierarchies.Options.RollAlternateHierarchies) + _delimiter +
                                //                        FormatTransString(hierarchies.Options.AutoAddCharacteristics));
                                _alOptionsRecords.Add("O" + _delimiter +
                                                       "B" + _delimiter +
                                                       FormatTransString(hierarchies.Options.ReclassPreview) + _delimiter +
                                                       FormatTransString(hierarchies.Options.RollExternalIntransit) + _delimiter +
                                                       FormatTransString(hierarchies.Options.RollAlternateHierarchies) + _delimiter +
                                                       FormatTransString(hierarchies.Options.AutoAddCharacteristics));
                                // End TT#226 MD
                                // Begin TT#3342 - JSmith - Add AllowDeletes to Option record
                                if (hierarchies.Options.AllowDeletesSpecified)
                                {
                                    _allowDeletes = hierarchies.Options.AllowDeletes;
                                }

                                // End TT#3342 - JSmith - Add AllowDeletes to Option record
							}

							if (hierarchies.Options.ReclassForecastVersion != null)
							{
								foreach (HierarchiesOptionsReclassForecastVersion rollupOption in hierarchies.Options.ReclassForecastVersion)
								{
                                    // Begin TT#226 MD - JSmith - Options records not duplicated to each file
                                    //WriteHierarchyTransaction("O" + _delimiter +
                                    //                        "R" + _delimiter +
                                    //                        FormatTransString(rollupOption.Name) + _delimiter +
                                    //                        FormatTransString(rollupOption.Rollup));
                                    _alOptionsRecords.Add("O" + _delimiter +
                                                            "R" + _delimiter +
                                                            FormatTransString(rollupOption.Name) + _delimiter +
                                                            FormatTransString(rollupOption.Rollup));
                                    // End TT#226 MD
								}
							}
						}

						if (hierarchies.Hierarchy != null)
						{
							foreach (HierarchiesHierarchy hierTrans in hierarchies.Hierarchy)
							{
								_hierarchyIdHash[hierTrans.ID] = null;

								if (hierTrans.UpdateDefinition)
								{
									WriteHierarchyTransaction("H" + _delimiter +
															FormatTransString(hierTrans.ID) + _delimiter +
															FormatTransString(hierTrans.Type) + _delimiter +
															FormatTransString(hierTrans.Color) + _delimiter);
									_numHierarchyRecs++;
								}

								if (hierTrans.Level != null)
								{
									foreach (HierarchiesHierarchyLevel levelTrans in hierTrans.Level)
									{
										WriteHierarchyTransaction("L" + _delimiter +
																FormatTransString(hierTrans.ID) + _delimiter +
																FormatTransString(levelTrans.Name) + _delimiter +
																FormatTransString(levelTrans.Type) + _delimiter +
																FormatTransString(levelTrans.LengthType) + _delimiter +
																FormatTransString(levelTrans.Color) + _delimiter +
																levelTrans.RequiredSize + _delimiter +
																levelTrans.SizeRangeFrom + _delimiter +
																levelTrans.SizeRangeTo + _delimiter +
																FormatTransString(levelTrans.PlanLevelType) + _delimiter +
																FormatTransString(levelTrans.IsOTSForecastLevel));
										_numHierarchyRecs++;
									}
								}

								if (hierTrans.Product != null)
								{
									foreach (HierarchiesHierarchyProduct productTrans in hierTrans.Product)
									{
										switch (productTrans.Action)
										{
											case HierarchiesHierarchyProductAction.Update:

												inLine = "P" + "##MID##" +
														FormatTransString(hierTrans.ID) + "##MID##" +
														FormatTransString(productTrans.Parent) + "##MID##" +
														FormatTransString(productTrans.ID) + "##MID##" +
														FormatTransString(productTrans.Name) + "##MID##" +
														FormatTransString(productTrans.Description) + "##MID##" +
														FormatTransString(productTrans.Type) + "##MID##" +
														FormatTransString(productTrans.SizeCodeProductCategory) + "##MID##" +
														FormatTransString(productTrans.SizeCodePrimary) + "##MID##" +
														FormatTransString(productTrans.SizeCodeSecondary) + "##MID##" +
														FormatTransString(productTrans.OTSForecastLevel) + "##MID##" +
														FormatTransString(productTrans.OTSForecastLevelHierarchy) + "##MID##" +
														(productTrans.OTSForecastLevelSelectSpecified ? FormatTransString(productTrans.OTSForecastLevelSelect) : string.Empty) + "##MID##" +
														(productTrans.OTSForecastNodeSearchSpecified ? FormatTransString(productTrans.OTSForecastNodeSearch) : string.Empty) + "##MID##" +
                                                // Begin TT#1960 - JSmith - Apply Node Properties Error during Hierarchy ReClass Load
                                                        //FormatTransString(productTrans.OTSForecastStartsWith);
                                                        FormatTransString(productTrans.OTSForecastStartsWith) + "##MID##" +
                                                        FormatTransString(productTrans.ApplyNodeProperties);
                                                // Begin TT#1960

												if (productTrans.Characteristic != null)
												{
													foreach (HierarchiesHierarchyProductCharacteristic charTrans in productTrans.Characteristic)
													{
														inLine += "##MID##" + FormatTransString(charTrans.Name) +
																"##MID##" + FormatTransString(charTrans.Value);
													}
												}

                                                // Begin TT#1921 - JSmith - Error during Hierarchy Reclass
                                                if (DataIsValidToWrite(aInputFile,
                                                    inLine,
                                                    FormatTransString(hierTrans.ID),
                                                    FormatTransString(productTrans.Parent),
                                                    FormatTransString(productTrans.ID),
                                                    FormatTransString(productTrans.Name),
                                                    FormatTransString(productTrans.Description)))
                                                {
                                                // End TT#1921
                                                    _dlHierReclass.HierReclassTrans_Insert(
                                                        _processId,
                                                        lineCount++,
                                                        inLine,
                                                        FormatTransString(hierTrans.ID),
                                                        FormatTransString(productTrans.Parent),
                                                        FormatTransString(productTrans.ID),
                                                        FormatTransString(productTrans.Name),
                                                        FormatTransString(productTrans.Description));

                                                    commitLimit++;

                                                    if (commitLimit > cCommitLimit)
                                                    {
                                                        _dlHierReclass.CommitData();
                                                        commitLimit = 0;
                                                    }
                                                // Begin TT#1921 - JSmith - Error during Hierarchy Reclass
                                                }
                                                // End TT#1921

												break;

											case HierarchiesHierarchyProductAction.Move:

												WriteHierarchyTransaction("R" + _delimiter +
																		"M" + _delimiter +
																		FormatTransString(hierTrans.ID) + _delimiter +
																		FormatTransString(productTrans.Parent) + _delimiter +
																		FormatTransString(productTrans.ID) + _delimiter +
																		FormatTransString(productTrans.ToParent) + _delimiter +
																		FormatTransString(productTrans.NewID) + _delimiter +
																		FormatTransString(productTrans.Name) + _delimiter +
																		FormatTransString(productTrans.Description) + _delimiter);
												_numHierarchyRecs++;

												break;

											case HierarchiesHierarchyProductAction.Delete:

												WriteHierarchyTransaction("R" + _delimiter +
																		"D" + _delimiter +
																		FormatTransString(hierTrans.ID) + _delimiter +
																		FormatTransString(productTrans.Parent) + _delimiter +
																		FormatTransString(productTrans.ID) + _delimiter +
																		FormatTransString(productTrans.ReplaceWith) + _delimiter +
																		FormatTransString(productTrans.ForceDelete));
												_numHierarchyRecs++;

												break;

											case HierarchiesHierarchyProductAction.Rename:

												WriteHierarchyTransaction("R" + _delimiter +
																		"R" + _delimiter +
																		FormatTransString(hierTrans.ID) + _delimiter +
																		FormatTransString(productTrans.Parent) + _delimiter +
																		FormatTransString(productTrans.ID) + _delimiter +
																		FormatTransString(productTrans.NewID) + _delimiter +
																		FormatTransString(productTrans.Name) + _delimiter +
																		FormatTransString(productTrans.Description));
												_numHierarchyRecs++;

												break;
										}
									}
								}
							}
						}

						_dlHierReclass.CommitData();
					}
					catch (Exception exc)
					{
						_dlHierReclass.Rollback();
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in LoadXMLTransactionFile(): " + exc.Message, "HierarchyReclassProcess");
						throw;
					}
				}
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in LoadXMLTransactionFile(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		public void LoadDelimitedTransactionFile(string aInputFile)
		{
			StreamReader reader;
            // Begin TT#223 MD - JSmith - Add file order processsing (FIFO, LIFO)
            //int lineCount;
            // End TT#223 MD
			int commitLimit;
			string inLine;
			string[] split;
			string prodName = string.Empty;
			string prodDesc = string.Empty;

            //BEGIN TT#3896-VStuart-Provide file name(s) in audit-MID
            string message;
            //END TT#3896-VStuart-Provide file name(s) in audit-MID

			try
			{
                // Begin TT#226 MD - JSmith - Options records not duplicated to each file
                _alOptionsRecords = new ArrayList();
                // End TT#226 MD
				reader = new StreamReader(aInputFile);

                //BEGIN TT#3896-VStuart-Provide file name(s) in audit-MID
                message = aInputFile;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                //END TT#3896-VStuart-Provide file name(s) in audit-MID

				try
				{
					_dlHierReclass.OpenUpdateConnection();

					try
					{
                        // Begin TT#223 MD - JSmith - Add file order processsing (FIFO, LIFO)
                        //lineCount = 0;
                        // End TT#223 MD
						commitLimit = 0;

						while (!reader.EndOfStream)
						{
							lineCount++;
							inLine = reader.ReadLine().Trim();

							if (inLine != string.Empty)
							{
								split = inLine.Split(_delimiter.ToCharArray());

								if (EditTransaction(split))
								{
									_hierarchyIdHash[split[1].Trim()] = null;

									if (split[0].Trim() == "P")
									{
										if (split.Length > 4)
										{
											prodName = split[4].Trim();
										}

										if (split.Length > 5)
										{
											prodDesc = split[5].Trim();
										}

                                        // Begin TT#1921 - JSmith - Error during Hierarchy Reclass
                                        if (DataIsValidToWrite(aInputFile, inLine.Replace(_delimiter, "##MID##"), split[1].Trim(), split[2].Trim(), split[3].Trim(), prodName, prodDesc))
                                        {
                                        // End TT#1921
                                            _dlHierReclass.HierReclassTrans_Insert(_processId, lineCount, inLine.Replace(_delimiter, "##MID##"), split[1].Trim(), split[2].Trim(), split[3].Trim(), prodName, prodDesc);


                                            commitLimit++;

                                            if (commitLimit > cCommitLimit)
                                            {
                                                _dlHierReclass.CommitData();
                                                commitLimit = 0;
                                            }
                                        // Begin TT#1921 - JSmith - Error during Hierarchy Reclass
                                        }
                                        // End TT#1921
									}
                                    // Begin TT#226 MD - JSmith - Options records not duplicated to each file
                                    else if (split[0].Trim() == "O" &&
                                        split[1].Trim() == "R")
                                    {
                                        _alOptionsRecords.Add(inLine);
                                    }
                                    // End TT#226 MD
                                    // Begin TT#3342 - JSmith - Add AllowDeletes to Option record
                                    else if (split[0].Trim() == "O" &&
                                    split[1].Trim() == "B")
                                    {
                                        if (split.Length > 6 &&
                                            split[6].Trim().Length > 0)
                                        {
                                            try
                                            {
                                                _allowDeletes = Convert.ToBoolean(split[6].Trim());
                                            }
                                            catch
                                            {
                                            }
                                        }
                                        _alOptionsRecords.Add(inLine);
                                    }
                                    // End TT#3342 - JSmith - Add AllowDeletes to Option record
                                    else
                                    {
                                        WriteHierarchyTransaction(inLine);
                                        _numHierarchyRecs++;
                                    }
								}
								else
								{
									_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, "Invalid Input Transaction rejected (Line #" + lineCount + "): " + inLine, "HierarchyReclassProcess");
									_numRejectedRecs++;
								}
							}
						}

						_dlHierReclass.CommitData();
					}
					catch (Exception exc)
					{
						_dlHierReclass.Rollback();
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in LoadDelimitedTransactionFile(): " + exc.Message, "HierarchyReclassProcess");
						throw;
					}
				}
				catch (Exception exc)
				{
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in LoadDelimitedTransactionFile(): " + exc.Message, "HierarchyReclassProcess");
					throw;
				}
				finally
				{
					reader.Close();
				}
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in LoadDelimitedTransactionFile(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		public void ProcessTransactions()
		{
			IDictionaryEnumerator iEnum;
			DataSet dsTrans;

			try
			{
				iEnum = _hierarchyIdHash.GetEnumerator();

				while (iEnum.MoveNext())
				{
					dsTrans = _dlHierReclass.HierReclassTrans_Process(_processId, Convert.ToString(iEnum.Key));

					//Process Add/Update entries

					foreach (DataRow row in dsTrans.Tables[0].Rows)
					{
						WriteHierarchyTransaction(Convert.ToString(row["TRANS_ORIGINAL"]).Replace("##MID##", _delimiter));
						_numAddUpdateRecs++;
					}

					//Process Delete entries

					if (_allowDeletes)
					{
						foreach (DataRow row in dsTrans.Tables[1].Rows)
						{
                            // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
                            //WriteReclassTransaction("R" + _delimiter + "D" + _delimiter + Convert.ToString(row["HIERARCHY_ID"]) + _delimiter + Convert.ToString(row["PARENT_ID"]) + _delimiter + Convert.ToString(row["PRODUCT_ID"]));
                            WriteReclassDeleteTransaction("R" + _delimiter + "D" + _delimiter + Convert.ToString(row["HIERARCHY_ID"]) + _delimiter + Convert.ToString(row["PARENT_ID"]) + _delimiter + Convert.ToString(row["PRODUCT_ID"]));
                            // End TT#2186
							_numDeleteRecs++;
						}
					}

					//Process Move entries

					foreach (DataRow row in dsTrans.Tables[2].Rows)
					{
    					WriteReclassTransaction("R" + _delimiter + "M" + _delimiter + Convert.ToString(row["HIERARCHY_ID"]) + _delimiter + Convert.ToString(row["PARENT_ID"]) + _delimiter + Convert.ToString(row["PRODUCT_ID"]) + _delimiter + Convert.ToString(row["TRANS_PARENT_ID"]));
						_numEditRecs++;
					}
				}
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in ProcessTransactions(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		private bool EditTransaction(string[] aSplitTransaction)
		{
			int i;

			try
			{
				if (aSplitTransaction.Length < 4)
				{
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, "Invalid Input Transaction -- 4 fields are required.", "HierarchyReclassProcess");
					return false;
				}

				for (i = 0; i < 4; i++)
				{
					if (aSplitTransaction[i].Trim() == string.Empty)
					{
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, "Invalid Input Transaction -- Field " + (i + 1) + " is required", "HierarchyReclassProcess");
						return false;
					}
				}

				return true;
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in EditTransaction(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

        // Begin TT#1921 - JSmith - Error during Hierarchy Reclass
        private bool DataIsValidToWrite(string aInputFile, string aOriginalLine, string aHierId, string aParentId, string aProductId, string aProductName, string aProductDesc)
        {
            string message;
            try
            {
                if (aOriginalLine.Length > _TRANS_ORIGINAL_Column_Size)
                {
                    message = (string)msg_DatabaseColumnSizeExceeded.Clone();
                    message = message.Replace("{0}", "Transaction for Hierarchy=" + aHierId + ";Parent=" + aParentId + ";Product ID=" + aProductId + " from file:" + aInputFile);
                    message = message.Replace("{1}", _TRANS_ORIGINAL_Column_Size.ToString());
                    message = message.Replace("{2}", "TRANS_ORIGINAL");
                    message = message.Replace("{3}", "HIERARCHY_RECLASS_TRANS");
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, "HierarchyReclassProcess");
                    return false;
                }

                if (aHierId.Length > _TRANS_HIERARCHY_ID_Column_Size)
                {
                    message = (string)msg_DatabaseColumnSizeExceeded.Clone();
                    message = message.Replace("{0}", "Hierarchy=" + aHierId + " from file:" + aInputFile);
                    message = message.Replace("{1}", _TRANS_HIERARCHY_ID_Column_Size.ToString());
                    message = message.Replace("{2}", "TRANS_HIERARCHY_ID");
                    message = message.Replace("{3}", "HIERARCHY_RECLASS_TRANS");
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, "HierarchyReclassProcess");
                    return false;
                }

                if (aParentId.Length > _TRANS_PARENT_ID_Column_Size)
                {
                    message = (string)msg_DatabaseColumnSizeExceeded.Clone();
                    message = message.Replace("{0}", "Parent=" + aParentId + " from file:" + aInputFile);
                    message = message.Replace("{1}", _TRANS_PARENT_ID_Column_Size.ToString());
                    message = message.Replace("{2}", "TRANS_PARENT_ID");
                    message = message.Replace("{3}", "HIERARCHY_RECLASS_TRANS");
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, "HierarchyReclassProcess");
                    return false;
                }

                if (aProductId.Length > _TRANS_PRODUCT_ID_Column_Size)
                {
                    message = (string)msg_DatabaseColumnSizeExceeded.Clone();
                    message = message.Replace("{0}", "Product ID=" + aProductId + " from file:" + aInputFile);
                    message = message.Replace("{1}", _TRANS_PRODUCT_ID_Column_Size.ToString());
                    message = message.Replace("{2}", "TRANS_PRODUCT_ID");
                    message = message.Replace("{3}", "HIERARCHY_RECLASS_TRANS");
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, "HierarchyReclassProcess");
                    return false;
                }

                if (aProductName.Length > _TRANS_PRODUCT_NAME_Column_Size)
                {
                    message = (string)msg_DatabaseColumnSizeExceeded.Clone();
                    message = message.Replace("{0}", "Name=" + aProductName + " from file:" + aInputFile);
                    message = message.Replace("{1}", _TRANS_PRODUCT_NAME_Column_Size.ToString());
                    message = message.Replace("{2}", "TRANS_PRODUCT_NAME");
                    message = message.Replace("{3}", "HIERARCHY_RECLASS_TRANS");
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, "HierarchyReclassProcess");
                    return false;
                }

                if (aProductDesc.Length > _TRANS_PRODUCT_DESC_Column_Size)
                {
                    message = (string)msg_DatabaseColumnSizeExceeded.Clone();
                    message = message.Replace("{0}", "Description=" + aProductDesc + " from file:" + aInputFile);
                    message = message.Replace("{1}", _TRANS_PRODUCT_DESC_Column_Size.ToString());
                    message = message.Replace("{2}", "TRANS_PRODUCT_DESC");
                    message = message.Replace("{3}", "HIERARCHY_RECLASS_TRANS");
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, "HierarchyReclassProcess");
                    return false;
                }

                return true;
            }
            catch (Exception exc)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in EditTransaction(): " + exc.Message, "HierarchyReclassProcess");
                throw;
            }
        }
        // End TT#1921

		private void OpenHierarchyTransaction()
		{
			try
			{
				if (_maxRecsPerFile != 0)
				{
					_hierOutName = _outputDirectory + "\\" + cHierarchyOutFile + "." + _hierOutSeq.ToString("D3", CultureInfo.CurrentUICulture) + _outSuffix;
				}
				else
				{
					_hierOutName = _outputDirectory + "\\" + cHierarchyOutFile + _outSuffix;
				}

				_hierOutFile = new StreamWriter(_hierOutName);
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in OpenHierarchyTransaction(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		private void CloseHierarchyTransaction()
		{
			StreamWriter trgFile;

			try
			{
				if (_hierOutFile != null)
				{
					_hierOutFile.Close();

					if (_triggerSuffix != string.Empty)
					{
						trgFile = new StreamWriter(_hierOutName + _triggerSuffix);
						trgFile.Close();
					}

					_hierOutFile = null;
					_hierOutSeq++;
				}
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in CloseHierarchyTransaction(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		private void WriteHierarchyTransaction(string aInLine)
		{

			try
			{
				if (_hierOutFile != null)
				{
					if (_maxRecsPerFile != 0 && _hierRecsWritten == _maxRecsPerFile)
					{
						CloseHierarchyTransaction();
						_hierRecsWritten = 0;
					}
				}

				if (_hierOutFile == null)
				{
					OpenHierarchyTransaction();
                    foreach (string optionsRecord in _alOptionsRecords)
                    {
                        _hierOutFile.WriteLine(optionsRecord);
                        _hierRecsWritten++;
                    }
				}

				_hierOutFile.WriteLine(aInLine);
				_hierRecsWritten++;
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in WriteHierarchyTransaction(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		private void OpenReclassTransaction()
		{
			try
			{
                // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
                //if (_maxRecsPerFile != 0)
                //{
                //    _reclassOutName = _outputDirectory + "\\" + cReclassOutFile + "." + _reclassOutSeq.ToString("D3", CultureInfo.CurrentUICulture) + _outSuffix;
                //}
                //else
                //{
                //    _reclassOutName = _outputDirectory + "\\" + cReclassOutFile + _outSuffix;
                //}
                if (_maxRecsPerFile != 0)
                {
                    _reclassOutName = _outputDirectory + "\\" + cReclassMoveOutFile + "." + _reclassOutSeq.ToString("D3", CultureInfo.CurrentUICulture) + _outSuffix;
                }
                else
                {
                    _reclassOutName = _outputDirectory + "\\" + cReclassMoveOutFile + _outSuffix;
                }
                // End TT#2186

				_reclassOutFile = new StreamWriter(_reclassOutName);
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in OpenReclassTransaction(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		private void CloseReclassTransaction()
		{
			StreamWriter trgFile;

			try
			{
				if (_reclassOutFile != null)
				{
					_reclassOutFile.Close();
                    // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
                    //if (_triggerSuffix != string.Empty)
                    //{
                    //    trgFile = new StreamWriter(_reclassOutName + _triggerSuffix);
                    //    trgFile.Close();
                    //}
                    if (_reclassMoveTriggerSuffix != string.Empty)
                    {
                        trgFile = new StreamWriter(_reclassOutName + _reclassMoveTriggerSuffix);
                        trgFile.Close();
                    }
                    // End TT#2186

					_reclassOutFile = null;
					_reclassOutSeq++;
				}
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in CloseReclassTransaction(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

		private void WriteReclassTransaction(string aInLine)
		{
			try
			{
				if (_reclassOutFile != null)
				{
					if (_maxRecsPerFile != 0 && _reclassRecsWritten == _maxRecsPerFile)
					{
						CloseReclassTransaction();
						_reclassRecsWritten = 0;
					}
				}

				if (_reclassOutFile == null)
				{
					OpenReclassTransaction();
                    // Begin TT#226 MD - JSmith - Options records not duplicated to each file
                    foreach (string optionsRecord in _alOptionsRecords)
                    {
                        _reclassOutFile.WriteLine(optionsRecord);
                        _reclassRecsWritten++;
                    }
                    // End TT#226 MD
				}

				_reclassOutFile.WriteLine(aInLine);
				_reclassRecsWritten++;
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in WriteReclassTransaction(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}

        // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
        private void OpenReclassDeleteTransaction()
        {
            try
            {
                if (_maxRecsPerFile != 0)
                {
                    _reclassDeleteOutName = _outputDirectory + "\\" + cReclassDeleteOutFile + "." + _reclassDeleteOutSeq.ToString("D3", CultureInfo.CurrentUICulture) + _outSuffix;
                }
                else
                {
                    _reclassDeleteOutName = _outputDirectory + "\\" + cReclassDeleteOutFile + _outSuffix;
                }

                _reclassDeleteOutFile = new StreamWriter(_reclassDeleteOutName);
            }
            catch (Exception exc)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in OpenReclassDeleteTransaction(): " + exc.Message, "HierarchyReclassProcess");
                throw;
            }
        }

        private void CloseReclassDeleteTransaction()
        {
            StreamWriter trgFile;

            try
            {
                if (_reclassDeleteOutFile != null)
                {
                    _reclassDeleteOutFile.Close();
                    if (_reclassDeleteTriggerSuffix != string.Empty)
                    {
                        trgFile = new StreamWriter(_reclassDeleteOutName + _reclassDeleteTriggerSuffix);
                        trgFile.Close();
                    }

                    _reclassDeleteOutFile = null;
                    _reclassDeleteOutSeq++;
                }
            }
            catch (Exception exc)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in CloseReclassDeleteTransaction(): " + exc.Message, "HierarchyReclassProcess");
                throw;
            }
        }

        private void WriteReclassDeleteTransaction(string aInLine)
        {
            try
            {
                if (_reclassDeleteOutFile != null)
                {
                    if (_maxRecsPerFile != 0 && _reclassDeleteRecsWritten == _maxRecsPerFile)
                    {
                        CloseReclassDeleteTransaction();
                        _reclassDeleteRecsWritten = 0;
                    }
                }

                if (_reclassDeleteOutFile == null)
                {
                    OpenReclassDeleteTransaction();
                    // Begin TT#226 MD - JSmith - Options records not duplicated to each file
                    foreach (string optionsRecord in _alOptionsRecords)
                    {
                        _reclassDeleteOutFile.WriteLine(optionsRecord);
                        _reclassDeleteRecsWritten++;
                    }
                    // End TT#226 MD
                }

                _reclassDeleteOutFile.WriteLine(aInLine);
                _reclassDeleteRecsWritten++;
            }
            catch (Exception exc)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in WriteReclassDeleteTransaction(): " + exc.Message, "HierarchyReclassProcess");
                throw;
            }
        }

        private void OpenReclassRenameTransaction()
        {
            try
            {
                if (_maxRecsPerFile != 0)
                {
                    _reclassRenameOutName = _outputDirectory + "\\" + cReclassRenameOutFile + "." + _reclassRenameOutSeq.ToString("D3", CultureInfo.CurrentUICulture) + _outSuffix;
                }
                else
                {
                    _reclassRenameOutName = _outputDirectory + "\\" + cReclassRenameOutFile + _outSuffix;
                }

                _reclassRenameOutFile = new StreamWriter(_reclassRenameOutName);
            }
            catch (Exception exc)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in OpenReclassRenameTransaction(): " + exc.Message, "HierarchyReclassProcess");
                throw;
            }
        }

        private void CloseReclassRenameTransaction()
        {
            StreamWriter trgFile;

            try
            {
                if (_reclassRenameOutFile != null)
                {
                    _reclassRenameOutFile.Close();
                    if (_reclassRenameTriggerSuffix != string.Empty)
                    {
                        trgFile = new StreamWriter(_reclassRenameOutName + _reclassRenameTriggerSuffix);
                        trgFile.Close();
                    }
                    // End TT#2186

                    _reclassRenameOutFile = null;
                    _reclassRenameOutSeq++;
                }
            }
            catch (Exception exc)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in CloseReclassRenameTransaction(): " + exc.Message, "HierarchyReclassProcess");
                throw;
            }
        }

        private void WriteReclassRenameTransaction(string aInLine)
        {
            try
            {
                if (_reclassRenameOutFile != null)
                {
                    if (_maxRecsPerFile != 0 && _reclassRenameRecsWritten == _maxRecsPerFile)
                    {
                        CloseReclassRenameTransaction();
                        _reclassRenameRecsWritten = 0;
                    }
                }

                if (_reclassRenameOutFile == null)
                {
                    OpenReclassRenameTransaction();
                    // Begin TT#226 MD - JSmith - Options records not duplicated to each file
                    foreach (string optionsRecord in _alOptionsRecords)
                    {
                        _reclassRenameOutFile.WriteLine(optionsRecord);
                        _reclassRenameRecsWritten++;
                    }
                    // End TT#226 MD
                }

                _reclassRenameOutFile.WriteLine(aInLine);
                _reclassRenameRecsWritten++;
            }
            catch (Exception exc)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in WriteReclassRenameTransaction(): " + exc.Message, "HierarchyReclassProcess");
                throw;
            }
        }
        // End TT#2186

		private string FormatTransString(object aObject)
		{
			try
			{
				if (aObject != null)
				{
					return Convert.ToString(aObject, CultureInfo.CurrentUICulture);
				}
				else
				{
					return string.Empty;
				}
			}
			catch (Exception exc)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Error encountered in FormatTransString(): " + exc.Message, "HierarchyReclassProcess");
				throw;
			}
		}
	}
}
