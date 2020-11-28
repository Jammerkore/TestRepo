using System;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

//using MIDRetail.Interface;
//using MIDRetail.Windows;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for HierarchyLoadProcess.
	/// </summary>
	public class HierarchyLoadProcess : HierarchyLoadProcessBase
	{
		string _sourceModule = "HierarchyLoadProcess.cs";
		private bool _variablesConnectionIsOpen = false;
        // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
        private bool _useCharacteristicTransaction = false;
        // End TT#2010

        // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
        //public HierarchyLoadProcess(SessionAddressBlock SAB, int commitLimit, ref bool errorFound, bool addCharacteristicGroups, bool addCharacteristicValues)
        //    : base(SAB, commitLimit, addCharacteristicGroups, addCharacteristicValues)
        public HierarchyLoadProcess(SessionAddressBlock SAB, int commitLimit, ref bool errorFound, bool addCharacteristicGroups, bool addCharacteristicValues, bool useCharacteristicTransaction)
            : base(SAB, commitLimit, addCharacteristicGroups, addCharacteristicValues)
        // End TT#2010
		{
			string message = null;

			try
			{
                // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
                _useCharacteristicTransaction = useCharacteristicTransaction;
                // End TT#2010
			}
			catch ( Exception err )
			{
				errorFound = true;
				SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, err.Message, _sourceModule);
				SAB.HierarchyServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
				if (_variablesConnectionIsOpen)
				{
					if (RecordsNotCommitted > 0)
					{
						SAB.HierarchyServerSession.CommitData();
					}
					SAB.HierarchyServerSession.CloseUpdateConnection();
				}
				message = err.Message;
				throw;
			}
		}


        // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
        //public int ProcessHierarchyFile(string fileLocation, char[] delimiter, ref bool errorFound, string exceptionFileName)
        public int ProcessHierarchyFile(string fileLocation, char[] delimiter, string characteristicDelimiter, ref bool errorFound, string exceptionFileName)
        // End TT#167
		{
			StreamReader reader = null;
			StreamWriter exceptionFile = null;
            //string line = null;  // TT#3546 - JSmith - Alternate Hierarchy Load Error
            string line2 = null;  // TT#3546 - JSmith - Alternate Hierarchy Load Error
			string message = null;
			int returnCode = 0;
			int levelSequence = 0;
			
			try
			{
                // Begin TT#366 - JSmith - Cannot access exceptionFile.txt because it is being used
                //exceptionFile = new StreamWriter(exceptionFileName);
                // End TT#366

                // Begin TT#3546 - JSmith - Alternate Hierarchy Load Error
                // process records first time to stage all records in cache area of hierarchy service
				reader = new StreamReader(fileLocation);  //opens the file

                bool aProcessRecord;
                ArrayList alChanges = new ArrayList();
                while ((line2 = reader.ReadLine()) != null)
                {
                    string[] fields = MIDstringTools.Split(line2, delimiter[0], true);
                    if (fields.Length == 1)
                    {
                        if (fields[0] == null)  // skip blank line 
                        {
                            continue;
                        }
                        else
                        {
                            ++RecordsRead;
                            ++RecordsWithErrors;
                            returnCode = 1;
                            SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_MismatchedDelimiter, line2, GetType().Name);
                            continue;
                        }
                    }
                    ++RecordsRead;
                    if (fields[0] == null)
                    {
                        returnCode = 1;
                        SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_TransactionTypeInvalid, line2, GetType().Name);
                    }
                    else
                    {
                        if (fields[0].ToUpper(CultureInfo.CurrentUICulture) == "P")
                        {
                            returnCode = AddProductRecord(fields, characteristicDelimiter, out aProcessRecord, true);
                            if (aProcessRecord)
                            {
                                alChanges.Add(line2);
                            }
                        }
                        else
                        {
                            alChanges.Add(line2);
                        }
                    }
                }

                if (reader != null)
                {
                    reader.Close();
                }
                

                //reader = new StreamReader(fileLocation);  //opens the file
                //while ((line = reader.ReadLine()) != null)
                foreach (string line in alChanges)
                // End TT#3546 - JSmith - Alternate Hierarchy Load Error
                {
					string[] fields = MIDstringTools.Split(line,delimiter[0],true);
                    // Begin TT#3546 - JSmith - Alternate Hierarchy Load Error
                    //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
                    //if (fields.Length == 1 && fields[0] == null)  // skip blank line 
                    //{
                    //    continue;
                    //}
                    //if (fields.Length == 1)  
                    //{
                    //    if (fields[0] == null)  // skip blank line 
                    //    {
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        ++RecordsRead;
                    //        ++RecordsWithErrors;
                    //        returnCode = 1;
                    //        // Begin TT#523 - JSmith - Serialization error referencing the Audit
                    //        //SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_MismatchedDelimiter, line, GetType().Name);
                    //        SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_MismatchedDelimiter, line, GetType().Name);
                    //        // End TT#523 - JSmith - Serialization error referencing the Audit
                    //        continue;
                    //    }
                    //}
                    ////End TT#106 MD
                    //++RecordsRead;
                    // End TT#3546 - JSmith - Alternate Hierarchy Load Error
					if (fields[0] == null)
					{
						returnCode = 1;
						SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_TransactionTypeInvalid, line, GetType().Name);
					}
					else
					{
						switch (fields[0].ToUpper(CultureInfo.CurrentUICulture))
						{
							case "O":  // options record
							{
								returnCode = ProcessOptionsRecord(line, fields);
								break;
							}

							case "H":  // hierarchy record
							{
								if (_variablesConnectionIsOpen)
								{
									if (RecordsNotCommitted > 0)
									{
										SAB.HierarchyServerSession.CommitData();
									}
									SAB.HierarchyServerSession.CloseUpdateConnection();
									_variablesConnectionIsOpen = false;
								}
								returnCode = AddHierarchyRecord(fields);
								break;
							}

							case "L":  // level record
							{
								if (_variablesConnectionIsOpen)
								{
									if (RecordsNotCommitted > 0)
									{
										SAB.HierarchyServerSession.CommitData();
									}
									SAB.HierarchyServerSession.CloseUpdateConnection();
									_variablesConnectionIsOpen = false;
								}
								++levelSequence;
								returnCode = AddLevelRecord(levelSequence, fields);
								break;
							}

							case "P":  // product record
							{
                                // Begin TT#685 - JSmith - KI Hierarchy Load Failure to Resume
                                //if (!_variablesConnectionIsOpen)
                                if (!_variablesConnectionIsOpen ||
                                    !SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                                // End TT#685
								{
									SAB.HierarchyServerSession.OpenUpdateConnection();
									_variablesConnectionIsOpen = true;
								}
                                // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
                                //returnCode = AddProductRecord(fields);
                                returnCode = AddProductRecord(fields, characteristicDelimiter, out aProcessRecord, false);  // TT#3546 - JSmith - Alternate Hierarchy Load Error
                                // End TT#167
								if (returnCode == 0)
								{
									++RecordsNotCommitted;
								}
								if (RecordsNotCommitted >= CommitLimit)
								{
									SAB.HierarchyServerSession.CommitData();
									RecordsNotCommitted = 0;
								}
								break;
							}

                            // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
                            case "C":  // characteristics record
                            {
                                if (_useCharacteristicTransaction)
                                {
                                    if (!_variablesConnectionIsOpen ||
                                        !SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                                    {
                                        SAB.HierarchyServerSession.OpenUpdateConnection();
                                        _variablesConnectionIsOpen = true;
                                    }
                                    returnCode = AddCharacteristicRecord(fields, characteristicDelimiter);
                                    if (returnCode == 0)
                                    {
                                        ++RecordsNotCommitted;
                                    }
                                    if (RecordsNotCommitted >= CommitLimit)
                                    {
                                        SAB.HierarchyServerSession.CommitData();
                                        RecordsNotCommitted = 0;
                                    }
                                }
                                else
                                {
                                    returnCode = 1;
                                    SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_TransactionTypeInvalid, line, GetType().Name);
                                }
                                break;
                            }
                            // End TT#2010

							case "R":  // reclass record
							{
                                // Begin TT#685 - JSmith - KI Hierarchy Load Failure to Resume
                                //if (!_variablesConnectionIsOpen)
                                if (!_variablesConnectionIsOpen ||
                                    !SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                                // End TT#685
								{
									SAB.HierarchyServerSession.OpenUpdateConnection();
									_variablesConnectionIsOpen = true;
								}

								if (SAB.HierarchyServerSession.UpdateConnectionIsOpen())
								{
									if (RecordsNotCommitted > 0)
									{
										SAB.HierarchyServerSession.CommitData();
										RecordsNotCommitted = 0;
									}
								}

								returnCode = ProcessReclassRecord(line, fields);
								if (returnCode == 0)
								{
									++RecordsNotCommitted;
								}
								if (RecordsNotCommitted >= CommitLimit)
								{
									SAB.HierarchyServerSession.CommitData();
									RecordsNotCommitted = 0;
								}
								break;
							}

							default:  //invalid
							{
								returnCode = 1;
								SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_TransactionTypeInvalid, line, GetType().Name);
								break;
							}
						}
					}
                    // Begin TT#366 - JSmith - Cannot access exceptionFile.txt because it is being used
                    //if (returnCode != 0)
                    if (returnCode != 0 &&
                        exceptionFile != null)
                    // End TT#366
					{
						exceptionFile.WriteLine(line);
					}
					//Begin TT#1754 - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy
                    //Begin TT#61 - MD - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy
                    if (fields[0].StartsWith("H"))
                    {
                        string hierarchyType = fields[2];
                        if (fields.Length < 3 || fields[2] == null || fields[2].Length == 0)
                        {
                            hierarchyType = "open";
                        }
                        else
                        {
                            hierarchyType = fields[2];
                        }
                        if (hierarchyType == "organizational" && returnCode != 0)
                        {
                            break;
                        }
                    }
                    //End TT#61 - MD - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy
					//End Begin TT#1754 - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy
				}
			}

			catch ( FileNotFoundException fileNotFound_error )
			{
				string exceptionMessage = fileNotFound_error.Message;
				errorFound = true;
				message = " : " + fileLocation;
				SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_InputFileNotFound, message);
				SAB.HierarchyServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
			}
			catch ( Exception err )
			{
				errorFound = true;
				SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, err.Message, _sourceModule);
				SAB.HierarchyServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
				if (_variablesConnectionIsOpen)
				{
					if (RecordsNotCommitted > 0)
					{
						SAB.HierarchyServerSession.CommitData();
						RecordsNotCommitted = 0;
					}
				}
//				if (SAB.HierarchyServerSession != null)
//				{
//					SAB.HierarchyServerSession.Terminate();
//				}
				throw;
			}
			finally
			{ 
				if (_variablesConnectionIsOpen)
				{
					if (RecordsNotCommitted > 0)
					{
						SAB.HierarchyServerSession.CommitData();
					}
					SAB.HierarchyServerSession.CloseUpdateConnection();
				}
                //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
                //SAB.HierarchyServerSession.Audit.HierarchyLoadAuditInfo_Add(HierarchyRecs, LevelRecs, ProductRecs, RecordsWithErrors,
                //    MoveRecs, RenameRecs, DeleteRecs);
                SAB.HierarchyServerSession.Audit.HierarchyLoadAuditInfo_Add(HierarchyRecs, LevelRecs, ProductRecs, RecordsWithErrors,
                    MoveRecs, RenameRecs, DeleteRecs, ProductRecsAdded, ProductRecsUpdated);
                //End TT#106 MD
                // Begin TT#3546 - JSmith - Alternate Hierarchy Load Error
                //if (reader != null)
                //{
                //    reader.Close(); 
                //}
                // End TT#3546 - JSmith - Alternate Hierarchy Load Error
				if (exceptionFile != null)
				{
					exceptionFile.Close(); 
				}
			}
			return returnCode;
		}

		private int AddHierarchyRecord(string[] fields)
		{
			string hierarchyID = null;
			string hierarchyType = null;
			string hierarchyColor = null;

			try
			{
				hierarchyID = fields[1];
				
				if (fields.Length < 3 || fields[2] == null || fields[2].Length == 0)   
				{
					hierarchyType = "open";
				}
				else
				{
					hierarchyType = fields[2];
				}

				if (fields.Length < 4 || fields[3] == null || fields[3].Length == 0)   
				{
					hierarchyColor = Include.MIDDefaultColor;
				}
				else
				{
					hierarchyColor = fields[3];
				}

                return AddHierarchyRecord(hierarchyID, hierarchyType, hierarchyColor);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			 
		}

		private int AddLevelRecord(int levelSequence, string[] fields)
		{
			string hierarchyID = null;
			string levelID = null;
			string levelLengthType = null;
			string levelType = null;
			string OTSPlanLevelType = "Undefined";
			string levelColor = null;
			int requiredSize = 0;
			int rangeFrom = 0;
			int rangeTo = 0;
			bool isOTSForecastLevel = false;

			try
			{
				hierarchyID = fields[1];
				
				if (fields.Length < 3 || fields[2] == null || fields[2].Length == 0)   
				{
					levelID = null;
				}
				else
				{
					levelID = fields[2];
				}

				if (fields.Length < 4 || fields[3] == null || fields[3].Length == 0)   
				{
					levelType = "undefined";
				}
				else
				{
					levelType = fields[3];
				}

				if (fields.Length < 5 || fields[4] == null || fields[4].Length == 0)   
				{
					levelLengthType = "unrestricted";
				}
				else
				{
					levelLengthType = fields[4];
				}

				if (fields.Length < 6 || fields[5] == null || fields[5].Length == 0)   
				{
					levelColor = Include.MIDDefaultColor;
				}
				else
				{
					levelColor = fields[5];
				}

				if (fields.Length < 7 || fields[6] == null || fields[6].Length == 0)   
				{
					requiredSize = 0;
				}
				else
				{
					requiredSize = Convert.ToInt32(fields[6], CultureInfo.CurrentUICulture);
				}

				if (fields.Length < 8 || fields[7] == null || fields[7].Length == 0)   
				{
					rangeFrom = 0;
				}
				else
				{
					rangeFrom = Convert.ToInt32(fields[7], CultureInfo.CurrentUICulture);
				}

				if (fields.Length < 9 || fields[8] == null || fields[8].Length == 0)   
				{
					rangeTo = 0;
				}
				else
				{
					rangeTo = Convert.ToInt32(fields[8], CultureInfo.CurrentUICulture);
				}

				if (fields.Length < 10 || fields[9] == null || fields[9].Length == 0)   
				{
					OTSPlanLevelType = "Undefined";
				}
				else
				{
					OTSPlanLevelType = fields[9];
				}

				//Begin Track #3948 - JSmith - add OTS Forecast Level interface
				if (fields.Length < 11 || fields[10] == null || fields[10].Length == 0)   
				{
					isOTSForecastLevel = false;
				}
				else
				{
					try
					{
						isOTSForecastLevel = Convert.ToBoolean(fields[10]);
					}
					catch
					{
					}
				}

				return AddLevelRecord(hierarchyID, levelSequence, levelID, levelType, OTSPlanLevelType,
					levelLengthType, requiredSize, rangeFrom, rangeTo, levelColor, isOTSForecastLevel);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
        //private int AddProductRecord(string[] fields)
        private int AddProductRecord(string[] fields, string characteristicDelimiter, out bool aProcessRecord, bool aBuildCacheOnly)  // TT#3546 - JSmith - Alternate Hierarchy Load Error
        // End TT#167
		{
			string hierarchyID = null;
			string parentID = null;
			string nodeID = null;
			string nodeName = null;
			string description = null;
			string productType = null;
			string sizeProductCategory = null;
			string sizeCodePrimary = null;
			string sizeCodeSecondary = null;
			string OTSForecastLevel = null;
			string OTSForecastLevelHierarchy = null;
			string aOTSForecastLevelSelect = null;
            string OTSForecastNodeSearch = null;
			string OTSForecastStartsWith = null;
            string ApplyNodeProperties = null;
			int fieldNumber = 1;
			int returnCode = 0;
            // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
            string characteristicValue;
            string characteristicType;
            // End TT#167

			try
			{
				hierarchyID = GetField(fields, fieldNumber++);

				parentID = GetField(fields, fieldNumber++);

				nodeID = GetField(fields, fieldNumber++);

				nodeName = GetField(fields, fieldNumber++);

				description = GetField(fields, fieldNumber++);

				productType = GetField(fields, fieldNumber++);

				sizeProductCategory = GetField(fields, fieldNumber++);

				sizeCodePrimary = GetField(fields, fieldNumber++);

				sizeCodeSecondary = GetField(fields, fieldNumber++);

				OTSForecastLevel = GetField(fields, fieldNumber++);

				OTSForecastLevelHierarchy = GetField(fields, fieldNumber++);
				
				aOTSForecastLevelSelect = GetField(fields, fieldNumber++);
				
				OTSForecastNodeSearch = GetField(fields, fieldNumber++);
				
				OTSForecastStartsWith = GetField(fields, fieldNumber++);

                // Begin TT#1399
                // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
                if (_useCharacteristicTransaction)
                {
                    ApplyNodeProperties = GetField(fields, fieldNumber++);
                }
                else
                {
                    ApplyNodeProperties = null;
                }
                // End TT#2010
                // End TT#1399
				
				returnCode = AddMerchandiseRecord(hierarchyID, parentID, nodeID, nodeName, description, productType,
					sizeProductCategory, sizeCodePrimary, sizeCodeSecondary, OTSForecastLevel,
                    OTSForecastLevelHierarchy, aOTSForecastLevelSelect, OTSForecastNodeSearch, OTSForecastStartsWith, ApplyNodeProperties, out aProcessRecord, aBuildCacheOnly);  // TT#3546 - JSmith - Alternate Hierarchy Load Error
				
				// process characteristics
                // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
                //// Begin TT#68 - JSmith - Invalid product characteristic message
                ////if (returnCode == 0)
                //if (returnCode == 0 &&
                //    fields.Length > fieldNumber)
                //// End TT#68
                //{
                //    int dynamicCharacteristsCount = 0;
                //    if (((fields.Length - fieldNumber) % 2) == 0)	// make sure characteristics are paired
                //    {
                //        int c = fieldNumber;
                //        dynamicCharacteristsCount = (fields.Length - fieldNumber) / 2;
                //        for (int i = 0; i < dynamicCharacteristsCount; i++)
                //        {
                //            // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
                //            //AddProductCharacteristic(hierarchyID, nodeID, fields[c], fields[c + 1]);
                //            characteristicValue = fields[c + 1];
                //            characteristicType = "Text";
                //            if (ParseCharName(ref characteristicValue, ref characteristicType, characteristicDelimiter))
                //            {
                //                // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
                //                //AddProductCharacteristic(hierarchyID, nodeID, fields[c], characteristicValue, characteristicType);
                //                AddProductCharacteristic(hierarchyID, parentID, nodeID, fields[c], characteristicValue, characteristicType);
                //                // End TT#298
                //            }
                //            // End TT#167
                //            c += 2;	// characteristics are in pairs
                //        }
                //        WriteProductCharacteristics(hierarchyID, nodeID);
                //    }
                //    else
                //    {
                //        string msgDetails = "Hierarchy: " + hierarchyID + "; ID: " + nodeID;
                //        SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidCharNotPaired, msgDetails, GetType().Name);
                //    }
                //}
                if (returnCode == 0 &&
                    fields.Length > fieldNumber &&
                    !_useCharacteristicTransaction)
                {
                    returnCode = ProcessCharacteristics(hierarchyID, parentID, nodeID, fieldNumber, fields, characteristicDelimiter);
                }
                // End TT#2010

				return returnCode;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
        private int AddCharacteristicRecord(string[] fields, string characteristicDelimiter)
        {
            string hierarchyID = null;
            string parentID = null;
            string nodeID = null;
            int fieldNumber = 1;
            

            try
            {
                hierarchyID = GetField(fields, fieldNumber++);

                parentID = GetField(fields, fieldNumber++);

                nodeID = GetField(fields, fieldNumber++);

                return ProcessCharacteristics(hierarchyID, parentID, nodeID, fieldNumber, fields,  characteristicDelimiter);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        private int ProcessCharacteristics(string aHierarchyID, string aParentID, string aNodeID, int aFieldNumber,
            string[] fields, string characteristicDelimiter)
        {
            int returnCode = 0;
            string characteristicValue;
            string characteristicType;

            try
            {
                // process characteristics
                if (returnCode == 0 &&
                    fields.Length > aFieldNumber)
                {
                    int dynamicCharacteristsCount = 0;
                    if (((fields.Length - aFieldNumber) % 2) == 0)	// make sure characteristics are paired
                    {
                        int c = aFieldNumber;
                        dynamicCharacteristsCount = (fields.Length - aFieldNumber) / 2;
                        for (int i = 0; i < dynamicCharacteristsCount; i++)
                        {
                            characteristicValue = fields[c + 1];
                            characteristicType = "Text";
                            if (ParseCharName(ref characteristicValue, ref characteristicType, characteristicDelimiter))
                            {
                                AddProductCharacteristic(aHierarchyID, aParentID, aNodeID, fields[c], characteristicValue, characteristicType);
                            }
                            c += 2;	// characteristics are in pairs
                        }
                        // Begin TT#5561 - JSmith - Product characteristic update not applied - xml 
                        //WriteProductCharacteristics(aHierarchyID, aNodeID);
                        if (HierarchyMaintenance.IsCharacteristicsUpdated)
                        {
                            WriteProductCharacteristics(aHierarchyID, aNodeID);
                        }
                        // End TT#5561 - JSmith - Product characteristic update not applied - xml 
                    }
                    else
                    {
                        string msgDetails = "Hierarchy: " + aHierarchyID + "; ID: " + aNodeID;
                        SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidCharNotPaired, msgDetails, GetType().Name);
                    }
                }

                return returnCode;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#2010

        // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
        private bool ParseCharName(ref string aChar, ref string aCharType, string aCharacteristicDelimiter)
        {
            bool isValid = true;
            // Begin TT#4529 - JSmith - Null reference error loading characteristics
            if (aCharType == null)
            {
                aCharType = "TEXT";
            }
            if (aChar == null)
            {
                return false;  // return false so null characteristic is ignored
            }
            // End TT#4529 - JSmith - Null reference error loading characteristics
            string origChar = aChar.Trim();
            aCharType = "Text";
            try
            {
                if (aChar.Contains(aCharacteristicDelimiter))
                {
                    int delimIndex = origChar.IndexOf(aCharacteristicDelimiter);
                    if (delimIndex > -1)
                    {
                        aChar = origChar.Substring(0, delimIndex);
                        string charType = (origChar.Substring(delimIndex + 1)).ToUpper();
                        switch (charType)
                        {
                            case "TEXT":
                                //storeCharType = eStoreCharType.text;
                                break;
                            case "DATE":
                                //storeCharType = eStoreCharType.date;
                                break;
                            case "NUMBER":
                                //storeCharType = eStoreCharType.number;
                                break;
                            case "DOLLAR":
                                //storeCharType = eStoreCharType.dollar;
                                break;
                            default:
                                isValid = false;
                                string msgDetails = SAB.HierarchyServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharType, false);
                                msgDetails = msgDetails.Replace("{0}", origChar);
                                msgDetails = msgDetails.Replace("{1}", charType);
                                SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreCharateristic, msgDetails, _sourceModule);
                                break;
                        }
                    }
                    else
                    {
                        aCharType = "Text";
                        aChar = aChar.Trim();
                    }
                }
            }
            catch
            {
                throw;
            }


            return isValid;
        }
        // End TT#167

		private string GetField(string[] aFields, int aFieldNumber)
		{
			try
			{
                //BEGIN TT#3869 - DOConnell - Cannot set OTS Forecast Default in Hierarchy Load API
                //if (aFields.Length < aFieldNumber + 1 || aFields[aFieldNumber] == null || aFields[aFieldNumber].Length == 0)
                if (aFields.Length < aFieldNumber + 1 || aFields[aFieldNumber] == null)
                //END TT#3869 - DOConnell - Cannot set OTS Forecast Default in Hierarchy Load API
				{
					return null;
				}
				else
				{
					return aFields[aFieldNumber];
				}
			}
			catch
			{
				throw;
			}
		}

		private int ProcessOptionsRecord(string aTransaction, string[] fields)
		{
			int returnCode = 0;

			try
			{
				string type = "B";
				if (fields.Length < 2 || fields[1] == null)
				{
					SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassOptionTypeInvalid, aTransaction, GetType().Name);
					return 1;
				}
				else
				{
					type = fields[1].ToUpper();
				}

				switch (type)
				{
					case "B":
						returnCode = ProcessBaseOptionsRecord(aTransaction, fields);
						break;
					case "R":
						returnCode = ProcessRollupOptionsRecord(aTransaction, fields);
						break;
					default:
						returnCode = 1;
						SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassOptionTypeInvalid, aTransaction, GetType().Name);
						break;
				}

				return returnCode;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private int ProcessBaseOptionsRecord(string aTransaction, string[] fields)
		{
			int returnCode = 0;
			try
			{
				try
				{
					if (fields.Length > 2 &&  fields[2] != null)   
					{
						try
						{
                            ReclassPreview = Convert.ToBoolean(fields[2].Trim());
						}
						catch
						{
							returnCode = 1;
							SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassPreviewFlagInvalid, aTransaction, GetType().Name);
							ReclassPreview = true;
						}
					}
				}
				catch
				{
					returnCode = 1;
					SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassPreviewFlagInvalid, aTransaction, GetType().Name);
					ReclassPreview = true;
				}

				// Begin Track #5259 - JSmith - Add new reclass roll options
				try
				{
					if (fields.Length > 3 &&  fields[3] != null)   
					{
						try
						{
                            RollExternalIntransit = Convert.ToBoolean(fields[3].Trim());
						}
						catch
						{
							returnCode = 1;
							SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassRollExtIntFlagInvalid, aTransaction, GetType().Name);
							RollExternalIntransit = false;
						}
					}
				}
				catch
				{
					returnCode = 1;
					SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassRollExtIntFlagInvalid, aTransaction, GetType().Name);
					RollExternalIntransit = false;
				}

				try
				{
					if (fields.Length > 4 &&  fields[4] != null)   
					{
						try
						{
                            RollAlternateHierarchies = Convert.ToBoolean(fields[4].Trim());
						}
						catch
						{
							returnCode = 1;
							SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassRollAltHierFlagInvalid, aTransaction, GetType().Name);
							RollAlternateHierarchies = false;
						}
					}
				}
				catch
				{
					returnCode = 1;
					SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassRollAltHierFlagInvalid, aTransaction, GetType().Name);
					RollAlternateHierarchies = false;
				}
				// End Track #5259

                // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
                // auto add flag
                if (fields.Length > 5 && fields[5] != null && fields[5].Length > 0)
                {
                    try
                    {
                        AutoAddCharacteristics = (Convert.ToBoolean(fields[5].Trim()));
                        // Begin TT#224 - JSmith - Options=True does not allow dynamic characteristics
                        AutoAddCharacteristicValues = AutoAddCharacteristics;
                        // End TT#224
                    }
                    catch
                    {
                        returnCode = 1;
                        string msgDetails = SAB.HierarchyServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharAutoAddValue, false);
                        msgDetails = msgDetails.Replace("{0}", fields[5].ToString(CultureInfo.CurrentUICulture));
                        SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidCharAutoAddValue, msgDetails, _sourceModule);
                    }
                }

                // Characteristic Type Delimiter
                // Begin TT#3910 - JSmith - Data load error for alternate hierarchy
                //if (fields.Length > 6 && fields[6] != null && fields[6].Length > 0)
                if (fields.Length > 7 && fields[7] != null && fields[7].Length > 0)
                // End TT#3910 - JSmith - Data load error for alternate hierarchy
                {
                    try
                    {
                        // Begin TT#3910 - JSmith - Data load error for alternate hierarchy
                        //CharacteristicDelimiter = (Convert.ToString(fields[6])).Trim();
                        CharacteristicDelimiter = (Convert.ToString(fields[7])).Trim();
                        // End TT#3910 - JSmith - Data load error for alternate hierarchy
                        if ((CharacteristicDelimiter.Trim()).Length != 1)
                        {
                            returnCode = 1;
                            string msgDetails = SAB.HierarchyServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharTypeDelimiter, false);
                            msgDetails = msgDetails.Replace("{0}", CharacteristicDelimiter);
                            SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidCharTypeDelimiter, msgDetails, _sourceModule);

                        }
                    }
                    catch
                    {
                        returnCode = 1;
                        string msgDetails = SAB.HierarchyServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharTypeDelimiter, false);
                        msgDetails = msgDetails.Replace("{0}", fields[2].ToString(CultureInfo.CurrentUICulture));
                        SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidCharTypeDelimiter, msgDetails, _sourceModule);
                    }
                }
                // End TT#167

				return returnCode;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private int ProcessRollupOptionsRecord(string aTransaction, string[] fields)
		{
			int returnCode = 0;
			try
			{
				if (fields.Length > 2 &&  fields[2] != null)   
				{
					string version = fields[2].ToLower();
					if (ForecastVersions.Contains(version))
					{
						if (fields.Length > 3 &&  fields[3] != null)   
						{
							bool rollVersion = false;
							try
							{
								rollVersion = Convert.ToBoolean(fields[3]);
							}
							catch
							{
								returnCode = 1;
								SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassRollupFlagInvalid, aTransaction, GetType().Name);
							}
							if (rollVersion)
							{
								RollupForecastVersions[(int)ForecastVersions[version]] = version;
							}
							else
							{
								RollupForecastVersions.Remove((int)ForecastVersions[version]);
							}
						}
						else
						{
							returnCode = 1;
							SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassRollupFlagInvalid, aTransaction, GetType().Name);
						}
					}
					else
					{
						returnCode = 1;
						SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VersionNotFound, aTransaction, GetType().Name);
					}
				}
				else
				{
					returnCode = 1;
					SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VersionRequired, aTransaction, GetType().Name);
				}
				return returnCode;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private int ProcessReclassRecord(string aTransaction, string[] fields)
		{
			try
			{
				int returnCode = 0;
				string hierarchyID = null;
				string parentID = null;
				string nodeID = null;
				string nodeName = null;
				string description = null;
				string newNodeID = null;

				if (fields.Length < 2 || fields[1] == null)
				{
					returnCode = 1;
					SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassActionInvalid, aTransaction, GetType().Name);
				}
				else
				{
					if (fields.Length > 2 && fields[2] != null)
					{
						hierarchyID = fields[2];
					}
					if (fields.Length > 3 && fields[3] != null)
					{
						parentID = fields[3];
					}
					if (fields.Length > 4 && fields[4] != null)
					{
						nodeID = fields[4];
					}
					switch (fields[1].ToLower())
					{
						case "m":
						case "move":
							string toParent = null;
							if (fields.Length > 5 && fields[5] != null)
							{
								toParent = fields[5];
							}
							// Begin Track #5259 - JSmith - Add new reclass roll options
//							returnCode = MoveMerchandiseRecord(hierarchyID, parentID, nodeID, toParent, ReclassPreview, RollupForecastVersions);
							returnCode = MoveMerchandiseRecord(hierarchyID, parentID, nodeID, toParent, ReclassPreview, RollupForecastVersions, 
								RollExternalIntransit, RollAlternateHierarchies);
							// End Track #5259
							// rename after move if requested
							if (returnCode == 0)
							{
								if ((fields.Length > 6 && fields[6] != null) ||
									(fields.Length > 7 && fields[7] != null) ||
									(fields.Length > 8 && fields[8] != null))
								{
									// use old ID as new ID if new ID is not provided
									if (fields.Length > 6 && fields[6] != null)
									{
										newNodeID = fields[6];
									}
									else
									{
										newNodeID = fields[4];
									}
									if (fields.Length > 7 && fields[7] != null)
									{
										nodeName = fields[7];
									}
									if (fields.Length > 8 && fields[8] != null)
									{
										description = fields[8];
									}
                                    //Begin TT#266 - JSmith - Hierarchy Reclass fails on rename during a move action
                                    //returnCode = RenameMerchandiseRecord(hierarchyID, parentID, nodeID, newNodeID, nodeName, description, ReclassPreview, true);
                                    returnCode = RenameMerchandiseRecord(hierarchyID, toParent, nodeID, newNodeID, nodeName, description, ReclassPreview, true);
                                    //End TT#266
								}
							}
							break;
						case "d":
						case "delete":
							bool forceDelete = false;
							string replaceWith = null;
							if (fields.Length > 5 && fields[5] != null)
							{
								replaceWith = fields[5];
							}
							try
							{
								if (fields.Length > 6 && fields[6] != null)
								{
									forceDelete = Convert.ToBoolean(fields[6]);
								}
							}
							catch
							{
								SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassForceDeleteInvalid, aTransaction, GetType().Name);
							}
							// Begin Track #5259 - JSmith - Add new reclass roll options
//							returnCode = DeleteMerchandiseRecord(hierarchyID, parentID, nodeID, replaceWith, forceDelete, ReclassPreview, RollupForecastVersions);
							returnCode = DeleteMerchandiseRecord(hierarchyID, parentID, nodeID, replaceWith, forceDelete, ReclassPreview, RollupForecastVersions,
								RollExternalIntransit, RollAlternateHierarchies);
							// End Track #5259
							break;
						case "r":
						case "rename":
							if (fields.Length > 5 && fields[5] != null)
							{
								newNodeID = fields[5];
							}
							if (fields.Length > 6 && fields[6] != null)
							{
								nodeName = fields[6];
							}
							if (fields.Length > 7 && fields[7] != null)
							{
								description = fields[7];
							}
							returnCode = RenameMerchandiseRecord(hierarchyID, parentID, nodeID, newNodeID, nodeName, description, ReclassPreview, false);
							break;
						default:
							returnCode = 1;
							SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ReclassActionInvalid, aTransaction, GetType().Name);
							break;
					}
				}
				return returnCode;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
	}
}
