// Begin TT#1239 - JSmith - Overall Performance
// Converted all Hashtable to Dictionary
// Too many lines to mark
// End TT#1239
using System; 
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;


namespace MIDRetail.Business
{

	[Serializable]
	public class HierarchyServerSession : Session
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public HierarchyServerSession(HierarchyServerSessionRemote aSessionRemote, int aServiceRetryCount, int aServiceRetryInterval)
			: base(aSessionRemote, eProcesses.hierarchyService, aServiceRetryCount, aServiceRetryInterval)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public int UserID
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return HierarchyServerSessionRemote.UserID;
						}
						catch (Exception exc)
						{
							if (isServiceRetryException(exc))
							{
								Thread.Sleep(ServiceRetryInterval);
							}
							else
							{
								throw;
							}
						}
					}

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
				}
				catch
				{
					throw;
				}
			}
		}

		public bool IsHierarchyAdministrator
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return HierarchyServerSessionRemote.IsHierarchyAdministrator;
						}
						catch (Exception exc)
						{
							if (isServiceRetryException(exc))
							{
								Thread.Sleep(ServiceRetryInterval);
							}
							else
							{
								throw;
							}
						}
					}

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
				}
				catch
				{
					throw;
				}
			}
		}

		public MerchandiseHierarchyData MHD
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return HierarchyServerSessionRemote.MHD;
						}
						catch (Exception exc)
						{
							if (isServiceRetryException(exc))
							{
								Thread.Sleep(ServiceRetryInterval);
							}
							else
							{
								throw;
							}
						}
					}

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
				}
				catch
				{
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

        // Begin TT#4988 - JSmith - Performance
        public Dictionary<int, eStoreStatus> GetStoreSalesStatusHash(int yearWeek)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetStoreSalesStatusHash(yearWeek);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public Dictionary<int, eStoreStatus> GetStoreStockStatusHash(int yearWeek)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetStoreStockStatusHash(yearWeek);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#4988 - JSmith - Performance

		public string GetQualifiedNodeID(int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetQualifiedNodeID(aNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public string GetQualifiedNodeID(int aNodeRID, string aOverrideDelimeter)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetQualifiedNodeID(aNodeRID, aOverrideDelimeter);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void Initialize()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.Initialize();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void CleanUpGlobal()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.CleanUpGlobal();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#1243 - JSmith - Audit Performance
        public void CloseSession()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HierarchyServerSessionRemote.CloseSession();
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public void CloseAudit()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HierarchyServerSessionRemote.CloseAudit();
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243
		
        //  BEGIN TT#2015 - gtaylor - apply changes to lower levels
        public void ClearCache(Dictionary<int, Dictionary<long, object>> nodeChanges)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HierarchyServerSessionRemote.ClearCache(nodeChanges);
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        //  END TT#2015 - gtaylor - apply changes to lower levels

		//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		public void ClearNodeCache()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.ClearNodeCache();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model

		public void ReBuildUserHierarchies()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.ReBuildUserHierarchies();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void BuildColorSizeIDs(eHierarchyLevelType aHierarchyLevelType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.BuildColorSizeIDs(aHierarchyLevelType);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public bool ColorSizesCacheUsed()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.ColorSizesCacheUsed();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public string ShowMemory()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.ShowMemory();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void Refresh()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.Refresh();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public bool UpdateConnectionIsOpen()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.UpdateConnectionIsOpen();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void OpenUpdateConnection()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.OpenUpdateConnection();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void OpenUpdateConnection(eLockType lockType, string lockID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.OpenUpdateConnection(lockType, lockID);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void CommitData()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.CommitData();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void CloseUpdateConnection()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.CloseUpdateConnection();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void Rollback()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.Rollback();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchySessionTransaction CreateTransaction()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.CreateTransaction();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void BuildAvailableNodeList()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.BuildAvailableNodeList();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void AddToSecurityNodeList(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.AddToSecurityNodeList(nodeRID);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ArrayList GetMyAllocationStyles(ArrayList aStyles)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetMyAllocationStyles(aStyles);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public string GetSizeCurveCriteriaProfileCurveName(HierarchyNodeProfile aNodeProf, SizeCurveCriteriaProfile aSccp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCurveCriteriaProfileCurveName(aNodeProf, aSccp);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetDefaultSizeCurveCriteriaNode(HierarchyNodeProfile aNodeProf, SizeCurveCriteriaProfile aSccp, int aHdrColorCount)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDefaultSizeCurveCriteriaNode(aNodeProf, aSccp, aHdrColorCount);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}
		
		public ProfileList GetProfileList(eProfileType aProfileType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetProfileList(aProfileType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public char GetProductLevelDelimiter()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetProductLevelDelimiter();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public bool AlternateAPIRollupExists()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.AlternateAPIRollupExists();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#5384 - JSmith - Size folder disappearing from Hierarchy properties after completion of step 13 to 15 of set up guide and restarting the MID services
        //public HierarchyProfile HierarchyUpdate(HierarchyProfile hp)
        // Begin TT#1911-MD - JSmith - Database Error on Update
        //public HierarchyProfile HierarchyUpdate(HierarchyProfile hp, bool updateLevels = true)
        public HierarchyProfile HierarchyUpdate(HierarchyProfile hp)
        // End TT#1911-MD - JSmith - Database Error on Update
        // End TT#5384 - JSmith - Size folder disappearing from Hierarchy properties after completion of step 13 to 15 of set up guide and restarting the MID services
        {
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
                        // Begin TT#5384 - JSmith - Size folder disappearing from Hierarchy properties after completion of step 13 to 15 of set up guide and restarting the MID services
                        //return HierarchyServerSessionRemote.HierarchyUpdate(hp);
                        // Begin TT#1911-MD - JSmith - Database Error on Update
                        //return HierarchyServerSessionRemote.HierarchyUpdate(hp, updateLevels);
                        return HierarchyServerSessionRemote.HierarchyUpdate(hp);
                        // End TT#1911-MD - JSmith - Database Error on Update
                        // End TT#5384 - JSmith - Size folder disappearing from Hierarchy properties after completion of step 13 to 15 of set up guide and restarting the MID services
					}
					catch (Exception exc)
					{
                        if (exc.Message.Contains("50000:"))
                        {
                            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
                            if (t.ToString().Length > 32700)
                            {
                                EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString().Substring(0, 32700), EventLogEntryType.Error);
                            }
                            else
                            {
                                EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString(), EventLogEntryType.Error);
                            }
                        }
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void RefreshCalendar(DateTime refreshDate)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.RefreshCalendar(refreshDate);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public DateTime GetPostingDate()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetPostingDate();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void PostingDateUpdate(DateTime postingDate)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.PostingDateUpdate(postingDate);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public DayProfile GetCurrentDate()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetCurrentDate();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyProfile GetMainHierarchyData()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetMainHierarchyData();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyProfile GetHierarchyData(int hierarchyRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetHierarchyData(hierarchyRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyProfile GetHierarchyDataForUpdate(int aHierarchyRID, bool aAllowReadOnly)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetHierarchyDataForUpdate(aHierarchyRID, aAllowReadOnly);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void DequeueHierarchy(int hierarchyRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.DequeueHierarchy(hierarchyRID);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyProfile GetHierarchyData(string hierarchyID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetHierarchyData(hierarchyID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public eHierarchyType GetHierarchyType(int hierarchyRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetHierarchyType(hierarchyRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int GetHierarchyOwner(int hierarchyRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetHierarchyOwner(hierarchyRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2064 - JSmith - Add message to Rollup when hierarchy dependency build fails
        //public HierarchyProfileList GetHierarchiesByDependency()
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HierarchyServerSessionRemote.GetHierarchiesByDependency();
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        // Begin TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
        //public HierarchyProfileList GetHierarchiesByDependency(ref string aReturnMessage)
        public HierarchyProfileList GetHierarchiesByDependency(ref string aReturnMessage, ref eMIDMessageLevel aMessageLevel)
        // End TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        // Begin TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
                        //return HierarchyServerSessionRemote.GetHierarchiesByDependency(ref aReturnMessage);
                        return HierarchyServerSessionRemote.GetHierarchiesByDependency(ref aReturnMessage, ref aMessageLevel);
                        // End TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2064

		public HierarchyProfileList GetHierarchiesForUser(int aUserRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetHierarchiesForUser(aUserRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int GetLongestBranch(int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetLongestBranch(aNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int GetLongestBranch(int aNodeRID, bool aHomeHierarchyOnly)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetLongestBranch(aNodeRID, aHomeHierarchyOnly);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int GetHighestGuestLevel(int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetHighestGuestLevel(aNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ArrayList GetAllGuestLevels(int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAllGuestLevels(aNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        public DataTable GetHierarchyDescendantLevels(int aNodeRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetHierarchyDescendantLevels(aNodeRID);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

		public int GetNodeOwner(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeOwner(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile NodeUpdateProfileInfo(HierarchyNodeProfile hnp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.NodeUpdateProfileInfo(hnp);
					}
					catch (Exception exc)
					{
                        if (exc.Message.Contains("50000:"))
                        {
                            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
                            if (t.ToString().Length > 32700)
                            {
                                EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString().Substring(0, 32700), EventLogEntryType.Error);
                            }
                            else
                            {
                                EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString(), EventLogEntryType.Error);
                            }
                        }
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
        //public void DeleteNodeData(int nodeRID, bool deleteNode, bool deleteEligibility, bool deleteStoreGrades,
        //    bool deleteVelocityGrades, bool deleteStoreCapacity, bool deletePurgeCriteria,
        //    bool deleteDailyPercentages, bool deleteSellThruPcts, bool deleteStockMinMaxes,
        //    bool deleteSizeCurveCriteria, bool deleteSizeCurveTolerance, bool deleteSizeCurveSimilarStore, bool aDeleteCharacteristics)
        public void DeleteNodeData(int nodeRID, bool deleteNode, bool deleteEligibility, bool deleteStoreGrades,
            bool deleteVelocityGrades, bool deleteStoreCapacity, bool deletePurgeCriteria,
            bool deleteDailyPercentages, bool deleteSellThruPcts, bool deleteStockMinMaxes,
            bool deleteSizeCurveCriteria, bool deleteSizeCurveTolerance, bool deleteSizeCurveSimilarStore, 
            bool deleteSizeOutOfStock, bool deleteSizeSellThru, bool aDeleteCharacteristics, bool deleteChainSetPercent, bool deleteIMO)
        //End TT#483 - JScott - Add Size Lost Sales criteria and processing
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
                        //HierarchyServerSessionRemote.DeleteNodeData(nodeRID, deleteNode, deleteEligibility, deleteStoreGrades,
                        //    deleteVelocityGrades, deleteStoreCapacity, deletePurgeCriteria,
                        //    deleteSizeCurveCriteria, deleteSizeCurveTolerance, deleteSizeCurveSimilarStore, aDeleteCharacteristics);
                        HierarchyServerSessionRemote.DeleteNodeData(nodeRID, deleteNode, deleteEligibility, deleteStoreGrades,
                            deleteVelocityGrades, deleteStoreCapacity, deletePurgeCriteria,
                            deleteDailyPercentages, deleteSellThruPcts, deleteStockMinMaxes,
                            deleteSizeCurveCriteria, deleteSizeCurveTolerance, deleteSizeCurveSimilarStore,
                            deleteSizeOutOfStock, deleteSizeSellThru, aDeleteCharacteristics, deleteChainSetPercent, deleteIMO //TT#1401 - Reservation Stores - gtaylor
                            );
                        //End TT#483 - JScott - Add Size Lost Sales criteria and processing
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void ReplaceNode(int aNodeRID, int aReplaceWithNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.ReplaceNode(aNodeRID, aReplaceWithNodeRID);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetNodeData(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeData(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetNodeData(int nodeRID, bool chaseHierarchy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeData(nodeRID, chaseHierarchy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetNodeData(int aNodeRID, bool aChaseHierarchy, bool aBuildQualifiedID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeData(aNodeRID, aChaseHierarchy, aBuildQualifiedID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#1313-MD -jsobek -Header Filters
        public string GetHierarchyIdByRID(int hierarchyRID)
        {
            try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetHierarchyIdByRID(hierarchyRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
        }
        //End TT#1313-MD -jsobek -Header Filters

		public HierarchyNodeProfile GetNodeDataForUpdate(int aNodeRID, bool aAllowReadOnly)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeDataForUpdate(aNodeRID, aAllowReadOnly);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void DequeueNode(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.DequeueNode(nodeRID);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public bool LockHierarchyBranchForUpdate(int aHierarchyRID, int aNodeRID, bool aAllowReadOnly)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.LockHierarchyBranchForUpdate(aHierarchyRID, aNodeRID, aAllowReadOnly);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public bool LockHierarchyBranchForDelete(int aHierarchyRID, int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.LockHierarchyBranchForDelete(aHierarchyRID, aNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeLockConflictList LockHierarchyBranchForDelete(NodeLockRequestList aNodeList)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.LockHierarchyBranchForDelete(aNodeList);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void DequeueBranch(int aHierarchyRID, int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.DequeueBranch(aHierarchyRID, aNodeRID);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetNodeData(int hierarchyRID, int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeData(hierarchyRID, nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetNodeData(int aHierarchyRID, int aNodeRID, bool aBuildQualifiedID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeData(aHierarchyRID, aNodeRID, aBuildQualifiedID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        public HierarchyNodeProfile GetNodeData(int aHierarchyRID, int aNodeRID, bool aChaseHierarchy, bool aBuildQualifiedID)
        {
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeData(aHierarchyRID, aNodeRID, aChaseHierarchy, aBuildQualifiedID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetNodeData(string nodeID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeData(nodeID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetNodeData(string nodeID, bool chaseHierarchy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeData(nodeID, chaseHierarchy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}
        //Begin TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        public HierarchyNodeProfile GetNodeDataWithQualifiedID(string nodeID, bool chaseHierarchy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
                        return HierarchyServerSessionRemote.GetNodeDataWithQualifiedID(nodeID, chaseHierarchy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}
        //End TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        

        //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        public HierarchyNodeProfile GetNodeDataFromBaseSearchString(string searchString)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetNodeDataFromBaseSearchString(searchString);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        
        //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis



		public HierarchyNodeProfile GetNodeData(string aNodeID, bool aChaseHierarchy, bool aBuildQualifiedID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeData(aNodeID, aChaseHierarchy, aBuildQualifiedID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAssortmentNode()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAssortmentNode();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int GetNodeRID(string nodeID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeRID(nodeID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int GetNodeRID(HierarchyNodeProfile aHnp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeRID(aHnp);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public string GetNodeID(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeID(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public string GetNodeText(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeText(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeAndParentIdsProfile GetNodeIDAndParentID(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeIDAndParentID(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int GetNodeRID(string aNodeID, char aLevelDelimiter, string aParentID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeRID(aNodeID, aLevelDelimiter, aParentID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public Hashtable GetNodeIDHash()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeIDHash();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public bool NodeExists(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.NodeExists(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile NodeLookup(string aNodeID, char aNodeDelimiter, bool aProcessingAutoAdd, bool aLookupParent, bool aChaseHierarchy, out string oParentID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.NodeLookup(aNodeID, aNodeDelimiter, aProcessingAutoAdd, aLookupParent, aChaseHierarchy, out oParentID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public Hashtable LookupNodes(SessionAddressBlock aSAB, Hashtable aNodeHash, bool aAllowAutoAdds, int aCommitLimit)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.LookupNodes(aSAB, aNodeHash, aAllowAutoAdds, aCommitLimit);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public Hashtable LookupAncestors(ArrayList aNodeList)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.LookupAncestors(aNodeList);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int GetDescendantCount(int HierarchyRID, int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantCount(HierarchyRID, nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int GetDescendantCount(int HierarchyRID, int nodeRID, bool aHomeHierarchyOnly)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantCount(HierarchyRID, nodeRID, aHomeHierarchyOnly);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAncestorDataByLevel(int hierarchyRID, int nodeRID, int levelSequence)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAncestorDataByLevel(hierarchyRID, nodeRID, levelSequence);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAncestorData(int hierarchyRID, int nodeRID, int levelOffset)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAncestorData(hierarchyRID, nodeRID, levelOffset);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAncestorData(int nodeRID, int levelOffset)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAncestorData(nodeRID, levelOffset);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAncestorData(string nodeID, int levelOffset)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAncestorData(nodeID, levelOffset);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAncestorData(int hierarchyRID, int nodeRID, eHierarchyLevelType levelType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAncestorData(hierarchyRID, nodeRID, levelType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAncestorData(int nodeRID, eHierarchyLevelType levelType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAncestorData(nodeRID, levelType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAncestorData(string nodeID, eHierarchyLevelType levelType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAncestorData(nodeID, levelType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAncestorData(int hierarchyRID, int nodeRID, string idPrefix)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAncestorData(hierarchyRID, nodeRID, idPrefix);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAncestorData(int nodeRID, string idPrefix)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAncestorData(nodeRID, idPrefix);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantDataByLevel(int aNodeRID, int aLevelSequence, bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantDataByLevel(aNodeRID, aLevelSequence, aChaseHierarchy, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantDataByLevel(int aNodeRID, int aLevelSequence, bool aChaseHierarchy,
			eNodeSelectType aNodeSelectType, bool aBuildQualifiedID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantDataByLevel(aNodeRID, aLevelSequence, aChaseHierarchy, aNodeSelectType, aBuildQualifiedID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantData(int hierarchyRID, int nodeRID, int levelOffset, bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantData(hierarchyRID, nodeRID, levelOffset, aChaseHierarchy, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantData(int nodeRID, int levelOffset, bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantData(nodeRID, levelOffset, aChaseHierarchy, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantData(int nodeRID, int levelOffset, bool aChaseHierarchy,
			eNodeSelectType aNodeSelectType, bool aBuildQualifiedID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantData(nodeRID, levelOffset, aChaseHierarchy, aNodeSelectType, aBuildQualifiedID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantData(string nodeID, int levelOffset, bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantData(nodeID, levelOffset, aChaseHierarchy, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantData(int hierarchyRID, int nodeRID, eHierarchyLevelType levelType,
			bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantData(hierarchyRID, nodeRID, levelType, aChaseHierarchy, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantData(int nodeRID, eHierarchyLevelType levelType,
			bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantData(nodeRID, levelType, aChaseHierarchy, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantData(int nodeRID, eHierarchyLevelMasterType aLevelType,
			bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantData(nodeRID, aLevelType, aChaseHierarchy, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantData(string nodeID, eHierarchyLevelType levelType, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantData(nodeID, levelType, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetDescendantData(string nodeID, eHierarchyLevelMasterType aLevelType,
			bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDescendantData(nodeID, aLevelType, aChaseHierarchy, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeDescendantList GetNodeDescendantList(int hierarchyRID, int aNodeRID,
			eHierarchyDescendantType aGetByType, int level, eHierarchyLevelMasterType aLevelType,
			eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeDescendantList(hierarchyRID, aNodeRID, aGetByType, level, aLevelType, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ArrayList GetIntransitReadNodes(int aNodeRID, eHierarchyLevelMasterType aLevelType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetIntransitReadNodes(aNodeRID, aLevelType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // BEGIN TT#1401 - gtaylor - Reservation Stores
        public IMOProfileList GetIMOList(ProfileList storeList, int nodeRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetMethodOverrideIMOList(storeList, false);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public IMOProfileList GetMethodOverrideIMOList(ProfileList storeList, bool forCopy)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetMethodOverrideIMOList(storeList, forCopy);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public IMOProfileList GetNodeIMOList(ProfileList storeList, int aNodeRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetNodeIMOList(storeList, aNodeRID);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateNodeIMOStore(int storeRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.UpdateNodeIMOStore(storeRID);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // END TT#1401 - gtaylor - Reservation Stores

		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		public ArrayList GetIMOReadNodes(int aNodeRID, eHierarchyLevelMasterType aLevelType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetIMOReadNodes(aNodeRID, aLevelType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}
		// END TT#1401 - stodd - add resevation stores (IMO)

		public bool IsParentChild(int aHierarchyRID, int aParentRID, int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.IsParentChild(aHierarchyRID, aParentRID, aNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeAncestorList GetPlanLevelPath(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetPlanLevelPath(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetPlanLevelData(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetPlanLevelData(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile CopyNode(int nodeRID, int parentRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.CopyNode(nodeRID, parentRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetRootNodes()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetRootNodes();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetRootNodes(eHierarchySelectType aHierarchyNodeType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetRootNodes(aHierarchyNodeType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetHierarchyChildren(int parentNodeLevel, int currentHierarchyRID,
			int homeHierarchyRID, int nodeRID, bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetHierarchyChildren(parentNodeLevel, currentHierarchyRID, homeHierarchyRID, nodeRID, aChaseHierarchy, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetHierarchyChildren(int parentNodeLevel, int currentHierarchyRID,
			int homeHierarchyRID, int nodeRID, bool aChaseHierarchy, eNodeSelectType aNodeSelectType, bool aAccessDenied)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetHierarchyChildren(parentNodeLevel, currentHierarchyRID, homeHierarchyRID, nodeRID, aChaseHierarchy, aNodeSelectType, aAccessDenied);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void JoinUpdate(HierarchyJoinProfile hjp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.JoinUpdate(hjp);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public bool JoinExists(HierarchyJoinProfile hjp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.JoinExists(hjp);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int EligModelUpdate(EligModelProfile emp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.EligModelUpdate(emp);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetEligModels()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetEligModels();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ModelProfile GetModelDataForUpdate(eModelType aModelType, int aModelRID, bool aAllowReadOnly)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetModelDataForUpdate(aModelType, aModelRID, aAllowReadOnly);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void DequeueModel(eModelType aModelType, int aModelRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.DequeueModel(aModelType, aModelRID);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public EligModelProfile GetEligModelData(string ModelID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetEligModelData(ModelID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public EligModelProfile GetEligModelData(int ModelRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetEligModelData(ModelRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        public EligModelProfile GetEligModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetEligModelData(ModelRID, aStoreList);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

		public int StkModModelUpdate(StkModModelProfile smmp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.StkModModelUpdate(smmp);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetStkModModels()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStkModModels();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public StkModModelProfile GetStkModModelData(string ModelID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStkModModelData(ModelID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public StkModModelProfile GetStkModModelData(int ModelRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStkModModelData(ModelRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        public StkModModelProfile GetStkModModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetStkModModelData(ModelRID, aStoreList);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

		public int SlsModModelUpdate(SlsModModelProfile smmp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.SlsModModelUpdate(smmp);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetSlsModModels()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSlsModModels();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SlsModModelProfile GetSlsModModelData(string ModelID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSlsModModelData(ModelID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SlsModModelProfile GetSlsModModelData(int ModelRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSlsModModelData(ModelRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        public SlsModModelProfile GetSlsModModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetSlsModModelData(ModelRID, aStoreList);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

		public int FWOSModModelUpdate(FWOSModModelProfile mmp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.FWOSModModelUpdate(mmp);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetFWOSModModels()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetFWOSModModels();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public FWOSModModelProfile GetFWOSModModelData(string ModelID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetFWOSModModelData(ModelID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public FWOSModModelProfile GetFWOSModModelData(int ModelRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetFWOSModModelData(ModelRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        public FWOSModModelProfile GetFWOSModModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetFWOSModModelData(ModelRID, aStoreList);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

        //BEGIN TT#108 - MD - DOConnell - FWOS Max Model Enhancement
        ///// <summary>
        ///// Contains the information about the entries in a FWOS modifier model
        ///// </summary>
        //[Serializable()]
        //public class FWOSMaxModelInfo : ModelInfo
        //{
        //    private double _FWOSMaxModelDefault;

        //    /// <summary>
        //    /// Used to construct an instance of the class.
        //    /// </summary>
        //    public FWOSMaxModelInfo()
        //    {
        //    }

        //    /// <summary>
        //    /// Gets or sets the default value of the FWOS modifier model.
        //    /// </summary>
        //    public double FWOSMaxModelDefault
        //    {
        //        get { return _FWOSMaxModelDefault; }
        //        set { _FWOSMaxModelDefault = value; }
        //    }
        //}

        ///// <summary>
        ///// Contains the information about the entries in a FWOS modifier model entry
        ///// </summary>
        //[Serializable()]
        //public class FWOSMaxModelEntryInfo : ModelEntryInfo
        //{
        //    private double _FWOSMaxModelEntryValue;
        //    /// <summary>
        //    /// Used to construct an instance of the class.
        //    /// </summary>
        //    public FWOSMaxModelEntryInfo()
        //    {
        //    }
        //    /// <summary>
        //    /// Gets or sets the sales modifier entry value of the FWOS modifier model.
        //    /// </summary>
        //    public double FWOSMaxModelEntryValue
        //    {
        //        get { return _FWOSMaxModelEntryValue; }
        //        set { _FWOSMaxModelEntryValue = value; }
        //    }

        //}

        public int FWOSMaxModelUpdate(FWOSMaxModelProfile mmp)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.FWOSMaxModelUpdate(mmp);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public ProfileList GetFWOSMaxModels()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetFWOSMaxModels();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public FWOSMaxModelProfile GetFWOSMaxModelData(string ModelID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetFWOSMaxModelData(ModelID);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public FWOSMaxModelProfile GetFWOSMaxModelData(int ModelRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetFWOSMaxModelData(ModelRID);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        public FWOSMaxModelProfile GetFWOSMaxModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetFWOSMaxModelData(ModelRID, aStoreList);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        //END TT#108 - MD - DOConnell - FWOS Max Model Enhancement

        //BEGIN TT#1401 - gtaylor - Reservation Stores
        public void IMOUpdate(int nodeRID, IMOProfileList imopl, bool cacheCleared)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HierarchyServerSessionRemote.IMOUpdate(nodeRID, imopl, cacheCleared); // TT#2015 - gtaylor - apply changes to lower levels
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        //END TT#1401 - gtaylor - Reservation Stores

		public void StoreEligibilityUpdate(int nodeRID, StoreEligibilityList sel, bool cacheCleared)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.StoreEligibilityUpdate(nodeRID, sel, cacheCleared); // TT#2015 - gtaylor - apply changes to lower levels
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public StoreEligibilityList GetStoreEligibilityList(ProfileList storeList, int nodeRID, bool chaseHierarchy, bool forCopy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStoreEligibilityList(storeList, nodeRID, chaseHierarchy, forCopy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SimilarStoreList GetSimilarStoreList(ProfileList storeList, int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSimilarStoreList(storeList, nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeAltModelProfile GetSizeAltModelData(int ModelRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeAltModelData(ModelRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeConstraintModelProfile GetSizeConstraintModelData(int ModelRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeConstraintModelData(ModelRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCurveGroupProfile GetSizeCurveGrouplData(int ModelRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCurveGrouplData(ModelRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeGroupProfile GetSizeGroupData(int ModelRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeGroupData(ModelRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void StoreGradesUpdate(int nodeRID, StoreGradeList sgl)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.StoreGradesUpdate(nodeRID, sgl);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public StoreGradeList GetStoreGradeList(int nodeRID, bool forCopy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStoreGradeList(nodeRID, forCopy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void StockMinMaxUpdate(int aNodeRID, NodeStockMinMaxesProfile aMinMaxesProfile)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.StockMinMaxUpdate(aNodeRID, aMinMaxesProfile);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeStockMinMaxesProfile GetStockMinMaxes(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStockMinMaxes(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public StoreGradeList GetStoreGradeList(int nodeRID, bool forCopy, bool forAdmin)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStoreGradeList(nodeRID, forCopy, forAdmin);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void StoreCapacityUpdate(int nodeRID, StoreCapacityList scl, bool cacheCleared)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.StoreCapacityUpdate(nodeRID, scl, cacheCleared); // TT#2015 - gtaylor - apply changes to lower levels
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public StoreCapacityList GetStoreCapacityList(ProfileList storeList, int nodeRID, bool stopOnFind, bool forCopy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStoreCapacityList(storeList, nodeRID, stopOnFind, forCopy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void SizeCurveCriteriaUpdate(int nodeRID, SizeCurveCriteriaList sccl)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.SizeCurveCriteriaUpdate(nodeRID, sccl);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCurveCriteriaList GetSizeCurveCriteriaList(int nodeRID, bool forCopy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
                        //  TT#1572 - GRT - Urban Alternate Hierarchy - added bool to call here - removed
						return HierarchyServerSessionRemote.GetSizeCurveCriteriaList(nodeRID, forCopy, false);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCurveCriteriaProfile GetDefaultSizeCurveCriteriaProfile(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetDefaultSizeCurveCriteriaProfile(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void SizeCurveDefaultCriteriaUpdate(int nodeRID, SizeCurveDefaultCriteriaProfile scdcp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.SizeCurveDefaultCriteriaUpdate(nodeRID, scdcp);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCurveDefaultCriteriaProfile GetSizeCurveDefaultCriteriaProfile(int nodeRID, SizeCurveCriteriaList sccl, bool forCopy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCurveDefaultCriteriaProfile(nodeRID, sccl, forCopy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void SizeCurveToleranceUpdate(int nodeRID, SizeCurveToleranceProfile sctp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.SizeCurveToleranceUpdate(nodeRID, sctp);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCurveToleranceProfile GetSizeCurveToleranceProfile(int nodeRID, bool forCopy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCurveToleranceProfile(nodeRID, forCopy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void SizeCurveSimilarStoreUpdate(int nodeRID, SizeCurveSimilarStoreList scssl)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.SizeCurveSimilarStoreUpdate(nodeRID, scssl);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCurveSimilarStoreList GetSizeCurveSimilarStoreList(ProfileList storeList, int nodeRID, bool chaseHierarchy, bool forCopy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCurveSimilarStoreList(storeList, nodeRID, chaseHierarchy, forCopy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        //End TT#155 - JScott - Add Size Curve info to Node Properties
        //Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
        /// <summary>
        /// Requests the session add or update size out of stock information 
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="soosp">A instance of the SizeOutOfStockProfile class which contains SizeOutOfStockProfile objects</param>
        public void SizeOutOfStockUpdate(int nodeRID, SizeOutOfStockProfile soosp)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HierarchyServerSessionRemote.SizeOutOfStockUpdate(nodeRID, soosp);
                        // Begin TT#234 MD - JSmith - Node Properties Out of Stock Tab appearance not as expected, save and hieracey service not available, tab says read only but security is full control, able to make changes when tab is read only.
                        return;
                        // End TT#234 MD
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Requests the session get the size out of stock list for the node.
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
        /// <returns>An instance of the SizeCurveCriteriaList class containing a SizeCurveCriteriaProfile for each boundary</returns>
        public SizeOutOfStockHeaderProfile GetSizeOutOfStockHeaderProfile(int nodeRID, int strGrpRID, int szGrpRID, bool forCopy)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetSizeOutOfStockHeaderProfile(nodeRID, strGrpRID, szGrpRID, forCopy);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Requests the session get the size out of stock list for the node.
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
        /// <returns>An instance of the SizeCurveCriteriaList class containing a SizeCurveCriteriaProfile for each boundary</returns>
        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        //public SizeOutOfStockProfile GetSizeOutOfStockProfile(int nodeRID, int strGrpRID, int szGrpRID, bool forCopy)
        public SizeOutOfStockProfile GetSizeOutOfStockProfile(int nodeRID, int strGrpRID, int szGrpRID, bool forCopy, int sg_Version)
        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                        //return HierarchyServerSessionRemote.GetSizeOutOfStockProfile(nodeRID, strGrpRID, szGrpRID, forCopy);
                        return HierarchyServerSessionRemote.GetSizeOutOfStockProfile(nodeRID, strGrpRID, szGrpRID, forCopy, sg_Version);
                        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Requests the session add or update size sell thrue information 
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="soosp">A instance of the SizeSellThruProfile class which contains SizeSellThruProfile objects</param>
        public void SizeSellThruUpdate(int nodeRID, SizeSellThruProfile soosp)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HierarchyServerSessionRemote.SizeSellThruUpdate(nodeRID, soosp);
                        // Begin TT#234 MD - JSmith - Node Properties Out of Stock Tab appearance not as expected, save and hieracey service not available, tab says read only but security is full control, able to make changes when tab is read only.
                        return;
                        // End TT#234 MD
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Requests the session get the size sell thru list for the node.
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
        /// <returns>An instance of the SizeCurveCriteriaList class containing a SizeCurveCriteriaProfile for each boundary</returns>
        public SizeSellThruProfile GetSizeSellThruProfile(int nodeRID, bool forCopy)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetSizeSellThruProfile(nodeRID, forCopy);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        //End TT#483 - JScott - Add Size Lost Sales criteria and processing

        //Begin TT#483 - STodd - Add Size Lost Sales criteria and processing
        //Begin TT#739-MD -jsobek -Delete Stores
        //public DataTable GetSizeOutOfStockColorNodes()
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HierarchyServerSessionRemote.GetSizeOutOfStockColorNodes();
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetSizeSellThruLimitColorNodes()
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HierarchyServerSessionRemote.GetSizeSellThruLimitColorNodes();
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public DataTable ExecuteSizeDayToWeekSummary(int aNodeRID, int startSQLTimeID, int endSQLTimeID, int storeRID = -1)
        {
            for (int i = 0; i < ServiceRetryCount; i++)
            {
                try
                {
                    return HierarchyServerSessionRemote.ExecuteSizeDayToWeekSummary(aNodeRID, startSQLTimeID, endSQLTimeID, storeRID);
                }
                catch (Exception exc)
                {
                    if (isServiceRetryException(exc))
                    {
                        Thread.Sleep(ServiceRetryInterval);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        }
        //End TT#739-MD -jsobek -Delete Stores
        //End TT#483 - JScott - Add Size Lost Sales criteria and processing

	
		public void VelocityGradesUpdate(int nodeRID, VelocityGradeList vgl)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.VelocityGradesUpdate(nodeRID, vgl);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public VelocityGradeList GetVelocityGradeList(int nodeRID, bool forCopy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetVelocityGradeList(nodeRID, forCopy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public VelocityGradeList GetVelocityGradeList(int nodeRID, bool forCopy, bool forAdmin)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetVelocityGradeList(nodeRID, forCopy, forAdmin);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void SellThruPctsUpdate(int nodeRID, SellThruPctList stpl)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.SellThruPctsUpdate(nodeRID, stpl);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SellThruPctList GetSellThruPctList(int nodeRID, bool forCopy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSellThruPctList(nodeRID, forCopy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void StoreDailyPercentagesUpdate(int nodeRID, StoreDailyPercentagesList sdpl, bool cacheCleared)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.StoreDailyPercentagesUpdate(nodeRID, sdpl, cacheCleared); // TT#2015 - gtaylor - apply changes to lower levels
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public StoreDailyPercentagesList GetStoreDailyPercentagesList(ProfileList storeList, int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStoreDailyPercentagesList(storeList, nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        public StoreDailyPercentagesList GetStoreDailyPercentagesList(StoreProfile storeProf, int nodeRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetStoreDailyPercentagesList(storeProf, nodeRID);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public eReturnCode StoreDailyPercentagesUpdate(ProfileList datelist, int nodeRID, StoreDailyPercentagesList sdpl, DateRangeProfile drp, bool cacheCleared)
        {
            
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HierarchyServerSessionRemote.StoreDailyPercentagesUpdate(datelist, nodeRID, sdpl, drp, cacheCleared); // TT#2015 - gtaylor - apply changes to lower levels
                        return eReturnCode.successful;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        //END TT#43 - MD - DOConnell - Projected Sales Enhancement

		public StoreWeekDailyPercentagesList GetStoreDailyPercentages(int nodeRID, int yearWeek)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStoreDailyPercentages(nodeRID, yearWeek);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetStylesForColor(int colorCodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetStylesForColor(colorCodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ColorCodeProfile GetColorCodeProfile(int colorCodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetColorCodeProfile(colorCodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ColorCodeProfile GetColorCodeProfile(string colorCodeID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetColorCodeProfile(colorCodeID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ColorCodeProfile ColorCodeUpdate(ColorCodeProfile ccp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.ColorCodeUpdate(ccp);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ColorCodeList GetColorCodeList()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetColorCodeList();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        public Dictionary<string, int> GetColorCodeListByID()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetColorCodeListByID();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ColorCodeList GetPlaceholderColors(int aNumberOfPlaceholderColors, ArrayList aCurrentPlaceholderColors)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetPlaceholderColors(aNumberOfPlaceholderColors, aCurrentPlaceholderColors);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeList GetPlaceholderStyles(int aAnchorNode, int aNumberOfPlaceholderStyles,
			int aCurrentNumberOfPlaceholderStyles, int aAssortmentRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetPlaceholderStyles(aAnchorNode, aNumberOfPlaceholderStyles, aCurrentNumberOfPlaceholderStyles, aAssortmentRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetLowestConnectorNode(int aAnchorNode)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetLowestConnectorNode(aAnchorNode);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeProfile GetAnchorNode(int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAnchorNode(aNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCodeProfile GetSizeCodeProfile(int sizeCodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCodeProfile(sizeCodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCodeProfile GetSizeCodeProfile(string sizeCodeID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCodeProfile(sizeCodeID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public int GetSizeCodeRID(string aProductCategory, string aSizeCodePrimary, string aSizeCodeSecondary)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCodeRID(aProductCategory, aSizeCodePrimary, aSizeCodeSecondary);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCodeProfile SizeCodeUpdate(SizeCodeProfile scp)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.SizeCodeUpdate(scp);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ArrayList GetSizeProductCategoryList()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeProductCategoryList();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCodeList GetSizeCodeList()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCodeList();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCodeList GetSizeCodeList(string productCategory)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCodeList(productCategory);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCodeList GetSizeCodeList(string productCategory, string primary, string secondary)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCodeList(productCategory, primary, secondary);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCodeList GetExactSizeCodeList(string productCategory, string primary, string secondary)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetExactSizeCodeList(productCategory, primary, secondary);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SizeCodeList GetSizeCodeList(string productCategory, eSearchContent productCategorySearchContent,
			string primary, eSearchContent primarySearchContent,
			string secondary, eSearchContent secondarySearchContent)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCodeList(productCategory, productCategorySearchContent, primary, primarySearchContent, secondary, secondarySearchContent);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        public Dictionary<string, int> GetSizeCodeListByID()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCodeListByID();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        public Dictionary<string, int> GetSizeCodeListByPriSec()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetSizeCodeListByPriSec();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#1952 - JSmith - Size Curve Failures - Override Low Level
        //public ArrayList GetAggregateSizeNodeList(HierarchyNodeProfile aNodeProf, int aVersionRID, int aLowLvlOverRID)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HierarchyServerSessionRemote.GetAggregateSizeNodeList(aNodeProf, aVersionRID, aLowLvlOverRID);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public ArrayList GetAggregateSizeNodeList(HierarchyNodeProfile aNodeProf, int aVersionRID, int aLowLvlOverRID, ArrayList aSizeCodeFilter)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetAggregateSizeNodeList(aNodeProf, aVersionRID, aLowLvlOverRID, aSizeCodeFilter);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#1952

		public bool ColorExistsForStyle(int hierarchyRID, int nodeRID, string colorCode, ref int colorNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.ColorExistsForStyle(hierarchyRID, nodeRID, colorCode, ref colorNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public bool ColorExistsForStyle(int hierarchyRID, int nodeRID, string colorCode, string aQualifiedNodeID, ref int colorNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.ColorExistsForStyle(hierarchyRID, nodeRID, colorCode, aQualifiedNodeID, ref colorNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2763 - JSmith - Hierarchy Color descriptions not updating
        public bool ColorExistsForStyle(int hierarchyRID, int nodeRID, string colorCode, string aQualifiedNodeID, ref int colorNodeRID, out string aColorDescription)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.ColorExistsForStyle(hierarchyRID, nodeRID, colorCode, aQualifiedNodeID, ref colorNodeRID, out aColorDescription);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2763 - JSmith - Hierarchy Color descriptions not updating

		public bool SizeExistsForColor(int hierarchyRID, int nodeRID, string sizeCode, ref int sizeNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.SizeExistsForColor(hierarchyRID, nodeRID, sizeCode, ref sizeNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public bool SizeExistsForColor(int hierarchyRID, int nodeRID, string sizeCode, string aQualifiedNodeID, ref int sizeNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.SizeExistsForColor(hierarchyRID, nodeRID, sizeCode, aQualifiedNodeID, ref sizeNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeAncestorList GetNodeAncestorList(int nodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeAncestorList(nodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeAncestorList GetNodeAncestorList(int nodeRID, int hierarchyRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeAncestorList(nodeRID, hierarchyRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeAncestorList GetNodeAncestorList(int nodeRID, eHierarchySearchType aHierarchySearchType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeAncestorList(nodeRID, aHierarchySearchType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeAncestorList GetNodeAncestorList(int nodeRID, int hierarchyRID, eHierarchySearchType aHierarchySearchType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeAncestorList(nodeRID, hierarchyRID, aHierarchySearchType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		//BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
        // Begin TT#4271 - JSmith - User Release Errors Logged 01202015
        /// <summary>
        /// Requests the session get the a list of NodeAncestorProfiles which contain information about
        /// the ancestors of the node in the requested hierarchy. 
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="hierarchyRID">The record id of the hierarchy</param>
        /// <param name="aHierarchySearchType">The type of hierarchies to search for ancestors</param>
        /// <param name="UseApplyFrom">Identifies if Node Properties Apply From is to be used</param>
        /// <returns>NodeAncestorList containing ancestors for the requested node</returns>
        public NodeAncestorList GetNodeAncestorList(int nodeRID, int hierarchyRID, eHierarchySearchType aHierarchySearchType, bool UseApplyFrom)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetNodeAncestorList(nodeRID, hierarchyRID, aHierarchySearchType, UseApplyFrom);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#4271 - JSmith - User Release Errors Logged 01202015
		//END TT#3962-VStuart-Dragged Values never allowed to drop-MID

		public ArrayList GetAllNodeAncestorLists(int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAllNodeAncestorLists(aNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public SortedList GetAllNodeAncestors(int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAllNodeAncestors(aNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetAncestorPath(int aNodeRID, int aToHierarchyRID, eLowLevelsType aToLowLevelType, int aToLevelRID, int aToOffset)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetAncestorPath(aNodeRID, aToHierarchyRID, aToLowLevelType, aToLevelRID, aToOffset);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeDescendantList GetNodeDescendantList(int nodeRID, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeDescendantList(nodeRID, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeDescendantList GetNodeDescendantList(int aNodeRID, eNodeSelectType aNodeSelectType, int aLevelOffset)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeDescendantList(aNodeRID, aNodeSelectType, aLevelOffset);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeDescendantList GetNodeDescendantList(int aNodeRID, eNodeSelectType aNodeSelectType, int aFromLevelOffset, int aToLevelOffset)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeDescendantList(aNodeRID, aNodeSelectType, aFromLevelOffset, aToLevelOffset);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void UpdateHomeLevel(int aNodeRID, int aHomeHierarchyRID, int aHomeLevel)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.UpdateHomeLevel(aNodeRID, aHomeHierarchyRID, aHomeLevel);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void UpdateHomeHierarchy(HierarchyNodeProfile aHierarchyNodeProfile, HierarchyProfile aOldHierarchyProfile, HierarchyProfile aNewHierarchyProfile, int aHomeLevel)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.UpdateHomeHierarchy(aHierarchyNodeProfile, aOldHierarchyProfile, aNewHierarchyProfile, aHomeLevel);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeDescendantList GetNodeDescendantList(int nodeRID, eHierarchyLevelType aHierarchyLevelType, eNodeSelectType aNodeSelectType)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetNodeDescendantList(nodeRID, aHierarchyLevelType, aNodeSelectType);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
        //public bool LoadDelimitedTransFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, int commitLimit, string exceptionFile,
        //    bool addCharacteristicGroups, bool addCharacteristicValues, string characteristicDelimiter)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HierarchyServerSessionRemote.LoadDelimitedTransFile(SAB, fileLocation, delimiter, commitLimit, exceptionFile,
        //                    addCharacteristicGroups, addCharacteristicValues, characteristicDelimiter);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public bool LoadDelimitedTransFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, int commitLimit, string exceptionFile,
            bool addCharacteristicGroups, bool addCharacteristicValues, string characteristicDelimiter, bool useCharacteristicTransaction)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.LoadDelimitedTransFile(SAB, fileLocation, delimiter, commitLimit, exceptionFile,
                            addCharacteristicGroups, addCharacteristicValues, characteristicDelimiter, useCharacteristicTransaction);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2010

		public bool LoadXMLTransFile(SessionAddressBlock SAB, string fileLocation, int commitLimit, string exceptionFile,
			bool addCharacteristicGroups, bool addCharacteristicValues)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.LoadXMLTransFile(SAB, fileLocation, commitLimit, exceptionFile,
							addCharacteristicGroups, addCharacteristicValues);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}


        //BEGIN TT#3625 - DOConnell - performance issue with Store Eligibility Load

        public bool LoadDelimitedEligibilityFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, ref bool errorFound)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.LoadDelimitedEligibilityFile(SAB, fileLocation, delimiter, ref errorFound);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2010

        public bool LoadXMLEligibilityFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, ref bool errorFound)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.LoadXMLEligibilityFile(SAB, fileLocation, delimiter, ref errorFound);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public bool LoadDelimitedIMOFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, ref bool errorFound)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.LoadDelimitedIMOFile(SAB, fileLocation, delimiter, ref errorFound);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2010

        public bool LoadXML_IMOFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, ref bool errorFound)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.LoadXML_IMOFile(SAB, fileLocation, delimiter, ref errorFound);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        //END TT#3625 - DOConnell - performance issue with Store Eligibility Load

		public bool BuildPurgeDates(int aWeeksToKeepDailySizeOnhand)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.BuildPurgeDates(aWeeksToKeepDailySizeOnhand);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ProductCharProfile GetProductCharProfile(int aProductCharRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetProductCharProfile(aProductCharRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ProductCharProfile GetProductCharProfile(string aProductCharID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetProductCharProfile(aProductCharID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        public ProductCharProfile ProductCharUpdate(ProductCharProfile aProductCharProfile, bool isReloadAllValues)  // TT#3558 - JSmith - Perf of Hierarchy Load
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
                        return HierarchyServerSessionRemote.ProductCharUpdate(aProductCharProfile, isReloadAllValues);  // TT#3558 - JSmith - Perf of Hierarchy Load
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ProductCharProfileList GetProductCharacteristics()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetProductCharacteristics();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeCharProfileList GetProductCharacteristics(int aNodeRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetProductCharacteristics(aNodeRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public NodeCharProfileList GetProductCharacteristics(int aNodeRID, bool aChaseHierarchy)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetProductCharacteristics(aNodeRID, aChaseHierarchy);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void UpdateProductCharacteristics(int aNodeRID, NodeCharProfileList aNodeCharProfileList)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HierarchyServerSessionRemote.UpdateProductCharacteristics(aNodeRID, aNodeCharProfileList);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ProductCharValueProfile GetProductCharValueProfile(int aProductCharValueRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetProductCharValueProfile(aProductCharValueRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
			eHierarchyLevelType aLevelType, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
			bool aMaintainingModels)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetOverrideList(aModelRID, aNodeRID, aVersionRID,
							aLevelType, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor,
							aMaintainingModels);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
            eHierarchyLevelType aLevelType, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
            bool aMaintainingModels, bool aIgnoreDuplicates)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetOverrideList(aModelRID, aNodeRID, aVersionRID,
                            aLevelType, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor,
                            aMaintainingModels, aIgnoreDuplicates);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method

		public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
			eHierarchyDescendantType aGetByType, int aLevel, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
			bool aMaintainingModels)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HierarchyServerSessionRemote.GetOverrideList(aModelRID, aNodeRID, aVersionRID,
							aGetByType, aLevel, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor,
							aMaintainingModels);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
		
        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
            eHierarchyDescendantType aGetByType, int aLevel, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
            bool aMaintainingModels, bool aIgnoreDuplicates)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetOverrideList(aModelRID, aNodeRID, aVersionRID,
                            aGetByType, aLevel, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor,
                            aMaintainingModels, aIgnoreDuplicates);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3


        public int ChainSetPercentUpdate(int nodeRID, ChainSetPercentList scl, int CDR_RID, bool cacheCleared)
        {
            int returnCode = -1;
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HierarchyServerSessionRemote.ChainSetPercentUpdate(nodeRID, scl, CDR_RID, cacheCleared); // TT#2015 - gtaylor - apply changes to lower levels
                        return returnCode;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public ChainSetPercentList GetChainSetPercentList(ProfileList storeList, int nodeRID, bool stopOnFind, bool forCopy, bool forAdmin, ProfileList WeekList)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetChainSetPercentList(storeList, nodeRID, stopOnFind, forCopy, forAdmin, WeekList);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public void DeleteChainSetPercentData(int nodeRID, int TimeID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HierarchyServerSessionRemote.DeleteChainSetPercentData(nodeRID, TimeID);
                        // Begin TT#234 MD - JSmith - Node Properties Out of Stock Tab appearance not as expected, save and hieracey service not available, tab says read only but security is full control, able to make changes when tab is read only.
                        return;
                        // End TT#234 MD
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        public ProfileList GetWeeks(int CDR_RID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetWeeks(CDR_RID);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        #region Apply Changes To Lower Levels 
        // BEGIN TT#2015 - gtaylor
        //  return the total number of descendants for this nodeRID
        public int TotalDescendants(int nodeRID)
        {
            try
            {
                return HierarchyServerSessionRemote.TotalDescendants(nodeRID);
            }
            catch
            {
                throw;
            }
        }

        public int TotalAffectedDescendants(int nodeRID, int IMO, int SE, int CHR, int SC, int DP, int PC, int CSP)
        {
            try
            {
                return HierarchyServerSessionRemote.TotalAffectedDescendants(nodeRID, IMO, SE, CHR, SC, DP, PC, CSP);
            }
            catch
            {
                throw;
            }
        }

        public int LockedDescendants(int nodeRID)
        {
            try
            {
                return HierarchyServerSessionRemote.LockedDescendants(nodeRID);
            }
            catch
            {
                throw;
            }
        }

        public int LockedAncestors(int nodeRID)
        {
            try
            {
                return HierarchyServerSessionRemote.LockedAncestors(nodeRID);
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateLowerLevelNodes(NodeChangeProfile _nodeChangeProfile)
        {
            try
            {
                return HierarchyServerSessionRemote.UpdateLowerLevelNodes(_nodeChangeProfile);
            }
            catch
            {
                throw;
            }
        }
        // END TT#2015 - gtaylor
		#endregion  
		
        // Begin TT#2231 - JSmith - Size curve build failing
        public LowLevelVersionOverrideProfileList GetOverrideListOfSizes(int aModelRID, int aNodeRID, bool aGetSizeCodeRIDOnly)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetOverrideListOfSizes(aModelRID, aNodeRID, aGetSizeCodeRIDOnly);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2231

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HierarchyServerSessionRemote.GetServiceProfile();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HierarchyServerSessionRemote.VerifyEnvironment(aClientProfile);
                        // Begin TT#234 MD - JSmith - Node Properties Out of Stock Tab appearance not as expected, save and hieracey service not available, tab says read only but security is full control, able to make changes when tab is read only.
                        return;
                        // End TT#234 MD
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD
	}
	//End TT#708 - JScott - Services need a Retry availalbe.
}
