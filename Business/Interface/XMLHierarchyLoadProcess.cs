using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Configuration;
using System.Data;
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
	/// Summary description for XMLHierarchyLoadProcess.
	/// </summary>
	public class XMLHierarchyLoadProcess : HierarchyLoadProcessBase
	{
		private string _sourceModule = "XMLHierarchyLoadProcess.cs";
		private Hierarchies _hierarchies = null;

		public XMLHierarchyLoadProcess(SessionAddressBlock SAB, int commitLimit, ref bool errorFound, bool addCharacteristicGroups, bool addCharacteristicValues)
			: base(SAB, commitLimit, addCharacteristicGroups, addCharacteristicValues)
		{
			string message = null;

			try
			{
			}
			catch ( Exception err )
			{
				errorFound = true;
				SAB.HierarchyServerSession.Audit.Log_Exception(err, _sourceModule);
				SAB.HierarchyServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
				if (SAB.HierarchyServerSession.UpdateConnectionIsOpen())
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

		public int ProcessHierarchyFile(string fileLocation, ref bool errorFound, string exceptionFileName)
		{
			int returnCode = 0;
						
			if(!File.Exists(fileLocation))	// Make sure our file exists before attempting to deserialize
			{
				errorFound = true;
				throw new XMLHierarchyLoadProcessException(String.Format("Hierarchy Service can not find the file located at '{0}'", fileLocation));
			}
            // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
            // Begin Track #4229 - JSmith - API locks .XML input file
            //TextReader r = null;
            // End Track #4229
            // End TT#3523 - JSmith - Performance of Anthro morning processing jobs
			try 
			{
				/* I created MIDRetail.HierarchyLoad.HierarchyLoadSchema.xsd to define and validate
						what a HierarchyLoad XML file should look like. From the Visual Studio command prompt I
						run xsd /c HierarchyLoadSchema.xsd to generate a class file that is a strongly typed
						represenation of that schema. The end result is I don't have to parse a loaded XML
						document node by node which can result in errors if the Xml document is not formed
						perfectly prior to reciept by this function.
					*/
                // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
                //XmlSerializer s = new XmlSerializer(typeof(Hierarchies));		// Create a Serializer
                //// Begin Track #4229 - JSmith - API locks .XML input file
                ////TextReader r = new StreamReader(fileLocation);					// Load the Xml File
                //r = new StreamReader(fileLocation);					// Load the Xml File
                //// End Track #4229
                //_hierarchies = (Hierarchies)s.Deserialize(r);					// Deserialize the Xml File to a strongly typed object
                //// Begin Track #4229 - JSmith - API locks .XML input file
                ////r.Close();														// Close the input file.
                //// End Track #4229

                _hierarchies = (Hierarchies)MIDFileReader.ReadXMLFile(fileLocation, typeof(Hierarchies));
                // End TT#3523 - JSmith - Performance of Anthro morning processing jobs
			}
			catch(Exception ex)
			{
				errorFound = true;
				SAB.HierarchyServerSession.Audit.Log_Exception(ex, _sourceModule, eExceptionLogging.logAllInnerExceptions);
				throw new XMLHierarchyLoadProcessException(String.Format("Error encountered during deserialization of the file '{0}':message='{1}'", fileLocation, ex.Message), ex);
			}
            // Begin Track #4229 - JSmith - API locks .XML input file
            // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
            //finally
            //{
            //    if (r != null)
            //        r.Close();
            //}
            //// End Track #4229
            // End TT#3523 - JSmith - Performance of Anthro morning processing jobs
			 
			try
			{
				foreach(HierarchiesHierarchy h in _hierarchies.Hierarchy)
				{
					if (Convert.ToBoolean(h.UpdateDefinition, CultureInfo.CurrentUICulture))
					{
						// commit any pending product transactions from the prior hierarchy
						if (SAB.HierarchyServerSession.UpdateConnectionIsOpen())
						{
							if (RecordsNotCommitted > 0)
							{
								SAB.HierarchyServerSession.CommitData();
								RecordsNotCommitted = 0;
							}
							SAB.HierarchyServerSession.CloseUpdateConnection();
						}
                        //Begin TT#1754 - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy
                        if (h.Type == HierarchiesHierarchyType.Organizational)
                        {
                            returnCode = AddHierarchyRecord(h.ID, Convert.ToString(h.Type, CultureInfo.CurrentUICulture), h.Color);
                            if (returnCode != 0)
                                break;
                        }
                        else
                        {
                            returnCode = AddHierarchyRecord(h.ID, Convert.ToString(h.Type, CultureInfo.CurrentUICulture), h.Color);
                        }
                        //End TT#1754 - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy
						if (h.Level != null)
						{
							int levelSequence = 0;
							foreach(HierarchiesHierarchyLevel l in h.Level) // Now loop through and add each level
							{
								++levelSequence;
								returnCode = AddLevelRecord(h.ID, levelSequence, l.Name, Convert.ToString(l.Type, CultureInfo.CurrentUICulture), 
									Convert.ToString(l.PlanLevelType, CultureInfo.CurrentUICulture), 
									Convert.ToString(l.LengthType, CultureInfo.CurrentUICulture),
									l.RequiredSize, l.SizeRangeFrom, l.SizeRangeTo, l.Color, l.IsOTSForecastLevel);
							}
						}
					}
					if (h.Product != null)
					{
						if (_hierarchies.Options != null)
						{
							ReclassPreview = _hierarchies.Options.ReclassPreview;
							// Begin Track #5259 - JSmith - Add new reclass roll options
							RollExternalIntransit = _hierarchies.Options.RollExternalIntransit;
							RollAlternateHierarchies = _hierarchies.Options.RollAlternateHierarchies;
							// End Track #5259
                            // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
                            if (_hierarchies.Options.AutoAddCharacteristicsSpecified)
                            {
                                AutoAddCharacteristics = _hierarchies.Options.AutoAddCharacteristics;
                                // Begin TT#285 - JSmith - Characteristic value not auto-adding
                                AutoAddCharacteristicValues = AutoAddCharacteristics;
                                // End TT#285
                            }
                            // End TT#167
							if (_hierarchies.Options.ReclassForecastVersion != null)
							{
								foreach(HierarchiesOptionsReclassForecastVersion fv in _hierarchies.Options.ReclassForecastVersion)
								{
									string version = fv.Name.ToLower();
									if (ForecastVersions.Contains(version))
									{
										if (fv.Rollup)
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
										string message = "Version:" + fv.Name;
										SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VersionNotFound, message, _sourceModule);
									}
								}
							}
						}
						
                        // Begin TT#3546 - JSmith - Alternate Hierarchy Load Error
                        // process records first time to stage all records in cache area of hierarchy service
                        bool aProcessRecord;
                        ArrayList alChanges = new ArrayList();
						foreach (HierarchiesHierarchyProduct p in h.Product) // Now loop through and affect product
                        {
                            returnCode = AddMerchandiseRecord(h.ID, p.Parent, p.ID, p.Name, p.Description, Convert.ToString(p.Type, CultureInfo.CurrentUICulture),
                                        p.SizeCodeProductCategory, p.SizeCodePrimary, p.SizeCodeSecondary, p.OTSForecastLevel,
                                        p.OTSForecastLevelHierarchy,
                                        p.OTSForecastLevelSelectSpecified ? Convert.ToString(p.OTSForecastLevelSelect, CultureInfo.CurrentUICulture) : null,
                                        p.OTSForecastNodeSearchSpecified ? Convert.ToString(p.OTSForecastNodeSearch, CultureInfo.CurrentUICulture) : null,
                                        p.OTSForecastStartsWith, p.ApplyNodeProperties, out aProcessRecord, true);
                            // Begin TT#5561 - JSmith - Product characteristic update not applied - xml 
                            if (returnCode == 0)
                            {
                                if (p.Characteristic != null)
                                {
                                    foreach (HierarchiesHierarchyProductCharacteristic c in p.Characteristic) // Now loop through and check each Characteristic
                                    {
                                        AddProductCharacteristic(h.ID, p.Parent, p.ID, c.Name, c.Value, "Text");
                                    }
                                    if (HierarchyMaintenance.IsCharacteristicsUpdated)
                                    {
                                        aProcessRecord = true;
                                    }
                                }
                            }
                            // End TT#5561 - JSmith - Product characteristic update not applied - xml 
                            // Begin TT#3631 - JSmith - Hierarchy Reclass Not Processing Actions
                            //if (aProcessRecord)
                            if (aProcessRecord ||
                                p.Action != HierarchiesHierarchyProductAction.Update)
                            // End TT#3631 - JSmith - Hierarchy Reclass Not Processing Actions
                            {
                                alChanges.Add(p);
                            }
                        }
                        // End TT#3546 - JSmith - Alternate Hierarchy Load Error
						
						if (!SAB.HierarchyServerSession.UpdateConnectionIsOpen())
						{
							SAB.HierarchyServerSession.OpenUpdateConnection();
						}
									
                        //foreach(HierarchiesHierarchyProduct p in h.Product) // Now loop through and affect product
                        foreach (HierarchiesHierarchyProduct p in alChanges) // Now loop through and affect product  // TT#3546 - JSmith - Alternate Hierarchy Load Error
						{
							switch (p.Action)
							{
								case HierarchiesHierarchyProductAction.Update:
                                    // Begin TT#685 - JSmith - KI Hierarchy Load Failure to Resume
                                    if (!SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                                    {
                                        SAB.HierarchyServerSession.OpenUpdateConnection();
                                    }
                                    // End TT#685
									returnCode = AddMerchandiseRecord(h.ID, p.Parent, p.ID, p.Name, p.Description, Convert.ToString(p.Type, CultureInfo.CurrentUICulture),
										p.SizeCodeProductCategory, p.SizeCodePrimary, p.SizeCodeSecondary, p.OTSForecastLevel,
//										p.OTSForecastLevelHierarchy, Convert.ToString(p.OTSForecastLevelSelect, CultureInfo.CurrentUICulture),
//										Convert.ToString(p.OTSForecastNodeSearch, CultureInfo.CurrentUICulture),
										p.OTSForecastLevelHierarchy, 
										p.OTSForecastLevelSelectSpecified ? Convert.ToString(p.OTSForecastLevelSelect, CultureInfo.CurrentUICulture) : null,
										p.OTSForecastNodeSearchSpecified ? Convert.ToString(p.OTSForecastNodeSearch, CultureInfo.CurrentUICulture) : null,
										p.OTSForecastStartsWith, p.ApplyNodeProperties, out aProcessRecord,  false);  // TT#3546 - JSmith - Alternate Hierarchy Load Error
									if (returnCode == 0)
									{
										if (p.Characteristic != null)
										{
											foreach (HierarchiesHierarchyProductCharacteristic c in p.Characteristic) // Now loop through and add each Characteristic
											{
                                                // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
                                                //AddProductCharacteristic(h.ID, p.ID, c.Name, c.Value);
                                                // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
                                                //AddProductCharacteristic(h.ID, p.ID, c.Name, c.Value, "Text");
                                                AddProductCharacteristic(h.ID, p.Parent, p.ID, c.Name, c.Value, "Text");
                                                // End TT#298
                                                // End TT#167
											}
											WriteProductCharacteristics(h.ID, p.ID);
										}
									}
									break;
								case HierarchiesHierarchyProductAction.Move:
									if (SAB.HierarchyServerSession.UpdateConnectionIsOpen())
									{
										if (RecordsNotCommitted > 0)
										{
											SAB.HierarchyServerSession.CommitData();
											RecordsNotCommitted = 0;
										}
									}
                                    // Begin TT#685 - JSmith - KI Hierarchy Load Failure to Resume
                                    else
                                    {
                                        SAB.HierarchyServerSession.OpenUpdateConnection();
                                    }
                                    // End TT#685
									// Begin Track #5259 - JSmith - Add new reclass roll options
//									returnCode = MoveMerchandiseRecord(h.ID, p.Parent, p.ID, p.ToParent, ReclassPreview, RollupForecastVersions);
									returnCode = MoveMerchandiseRecord(h.ID, p.Parent, p.ID, p.ToParent, ReclassPreview, RollupForecastVersions, 
										RollExternalIntransit, RollAlternateHierarchies);
									// End Track #5259
									// rename after move if requested
                                    //Begin TT#266 - JSmith - Hierarchy Reclass fails on rename during a move action
                                    //if (p.NewID != null ||
                                    //    p.Name != null ||
                                    //    p.Description != null)
                                    if (returnCode == 0 &&
                                        (p.NewID != null ||
                                        p.Name != null ||
                                        p.Description != null))
                                    //End TT#266
									{
										// use old ID as new ID if new ID is not provided
										if (p.NewID == null)
										{
											p.NewID = p.ID;
										}
                                        //Begin TT#266 - JSmith - Hierarchy Reclass fails on rename during a move action
                                        //returnCode = RenameMerchandiseRecord(h.ID, p.Parent, p.ID, p.NewID, p.Name, p.Description, ReclassPreview, true);
                                        returnCode = RenameMerchandiseRecord(h.ID, p.ToParent, p.ID, p.NewID, p.Name, p.Description, ReclassPreview, true);
                                        //End TT#266
									}
									break;
								case HierarchiesHierarchyProductAction.Delete:
									if (SAB.HierarchyServerSession.UpdateConnectionIsOpen())
									{
										if (RecordsNotCommitted > 0)
										{
											SAB.HierarchyServerSession.CommitData();
											RecordsNotCommitted = 0;
										}
									}
                                    // Begin TT#685 - JSmith - KI Hierarchy Load Failure to Resume
                                    else
                                    {
                                        SAB.HierarchyServerSession.OpenUpdateConnection();
                                    }
                                    // End TT#685
									// Begin Track #5259 - JSmith - Add new reclass roll options
//									returnCode = DeleteMerchandiseRecord(h.ID, p.Parent, p.ID, p.ReplaceWith, p.ForceDelete, ReclassPreview, RollupForecastVersions);
									returnCode = DeleteMerchandiseRecord(h.ID, p.Parent, p.ID, p.ReplaceWith, p.ForceDelete, ReclassPreview, RollupForecastVersions,
										RollExternalIntransit, RollAlternateHierarchies);
									// End Track #5259
									break;
								case HierarchiesHierarchyProductAction.Rename:
									if (SAB.HierarchyServerSession.UpdateConnectionIsOpen())
									{
										if (RecordsNotCommitted > 0)
										{
											SAB.HierarchyServerSession.CommitData();
											RecordsNotCommitted = 0;
										}
									}
                                    // Begin TT#685 - JSmith - KI Hierarchy Load Failure to Resume
                                    else
                                    {
                                        SAB.HierarchyServerSession.OpenUpdateConnection();
                                    }
                                    // End TT#685
									returnCode = RenameMerchandiseRecord(h.ID, p.Parent, p.ID, p.NewID, p.Name, p.Description, ReclassPreview, false);
									break;
							}
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
					}
				}
				// commit any pending records
				if (SAB.HierarchyServerSession.UpdateConnectionIsOpen())
				{
					if (RecordsNotCommitted > 0)
					{
						SAB.HierarchyServerSession.CommitData();
						RecordsNotCommitted = 0;
					}
					SAB.HierarchyServerSession.CloseUpdateConnection();
				}
			}
			catch(Exception ex)
			{
				errorFound = true;
				SAB.HierarchyServerSession.Audit.Log_Exception(ex, _sourceModule);
				SAB.HierarchyServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
				if (SAB.HierarchyServerSession.UpdateConnectionIsOpen())
				{
					if (RecordsNotCommitted > 0)
					{
						SAB.HierarchyServerSession.CommitData();
						RecordsNotCommitted = 0;
					}
				}
				throw new XMLHierarchyLoadProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
			}
			finally
			{
                //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
                //SAB.HierarchyServerSession.Audit.HierarchyLoadAuditInfo_Add(HierarchyRecs, LevelRecs, ProductRecs, RecordsWithErrors,
                //    MoveRecs, RenameRecs, DeleteRecs);
                SAB.HierarchyServerSession.Audit.HierarchyLoadAuditInfo_Add(HierarchyRecs, LevelRecs, ProductRecs, RecordsWithErrors,
                    MoveRecs, RenameRecs, DeleteRecs, ProductRecsAdded, ProductRecsUpdated);
                //End TT#106 MD
			}

			return returnCode;
		}
	}

	/// <summary>
	/// Local class's exception type.
	/// </summary>
	[Serializable]
	public class XMLHierarchyLoadProcessException : Exception
	{
		/// <summary>
		/// Used when throwing exceptions in the XML Hierarchy Load Class
		/// </summary>
		/// <param name="message">The error message to display</param>
		public XMLHierarchyLoadProcessException(string message): base(message)
		{
		}
		public XMLHierarchyLoadProcessException(string message, Exception innerException): base(message, innerException)
		{
		}

		public XMLHierarchyLoadProcessException(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}
		override public void GetObjectData(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}
}

