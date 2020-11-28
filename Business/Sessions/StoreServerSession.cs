using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
//Begin TT#708 - JScott - Services need a Retry availalbe.
using System.Threading;
//End TT#708 - JScott - Services need a Retry availalbe.

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// StoreServerGlobal is a static class that contains fields that are global to all StoreServerSession objects.
	/// </summary>
	/// <remarks>
	/// The StoreServerGlobal class is used to store information that is global to all StoreServerSession objects
	/// within the process.  A common use for this class would be to cache static information from the database in order to
	/// reduce accesses to the database.
	/// </remarks>



	class StoreTextSort:IComparer
	{
		public StoreTextSort()
		{
		}

		public int Compare(object x,object y)
		{	
			return ((StoreProfile)x).Text.CompareTo(((StoreProfile)y).Text);
				
		}
	}
	//Begin TT#708 - JScott - Services need a Retry availalbe.

	[Serializable]
	public class StoreServerSession : Session, IBatchLoadData
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public StoreServerSession(StoreServerSessionRemote aSessionRemote, int aServiceRetryCount, int aServiceRetryInterval)
			: base(aSessionRemote, eProcesses.storeService, aServiceRetryCount, aServiceRetryInterval)
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




        public MRSCalendar CalendarGlobal
        {
            get
            {
                        try
                        {
                            for (int i = 0; i < ServiceRetryCount; i++)
                            {
                                try
                                {
                                    return StoreServerSessionRemote.CalendarGlobal;
                                }
                                catch (Exception exc)
                                {
                                    if (isServiceRetryException(exc))
                                    {
                                        Thread.Sleep(ServiceRetryInterval);
                                    }
                                    else
                                    {
                                        throw;
                                    }
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

		public void Initialize()
		{
			try
			{
                //for (int i = 0; i < ServiceRetryCount; i++)
                //{
                //    try
                //    {
                //        StoreServerSessionRemote.Initialize();
                //        return;
                //    }
                //    catch (Exception exc)
                //    {
                //        if (isServiceRetryException(exc))
                //        {
                //            Thread.Sleep(ServiceRetryInterval);
                //        }
                //        else
                //        {
                //            throw;
                //        }
                //    }
                //}

                //throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));


                //================================
                // Getting current Global options
                //================================
                //GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
                //gop.LoadOptions();
                //_globalStoreDisplayOption = gop.StoreDisplay;

                StoreServerSessionRemote.Initialize();

               // StoreGroupRead storeGroupRead = new StoreGroupRead();

                //_dtActiveStoreGroupLevels = storeGroupRead.StoreGroup_Read();
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
						StoreServerSessionRemote.Refresh();
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

		public string ShowMemory()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return StoreServerSessionRemote.ShowMemory();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
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
						StoreServerSessionRemote.RefreshCalendar(refreshDate);
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

       
     
		
		public void PostingDateUpdate(DateTime postingDate)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						StoreServerSessionRemote.PostingDateUpdate(postingDate);
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



        // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
        public ProfileList GetActiveStoresList()
        {
            try
            {
                ProfileList activeList = new ProfileList(eProfileType.Store);
                foreach (StoreProfile sp in GetAllStoresList().ArrayList)
                {
                    if (sp.ActiveInd)
                    {
                        activeList.Add(sp);
                    }
                }

                return activeList;
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// returns a hashtable keyed by Store ID.  The value is the store's KEY/RID
        /// </summary>
        /// <returns></returns>
        public Hashtable GetStoreIDHash()
        {
            try
            {
                Hashtable storeHash = new Hashtable();

                foreach (StoreProfile sp in GetAllStoresList().ArrayList)
                {
                    storeHash.Add(sp.StoreId, sp.Key);
                }

                return storeHash;
            }
            catch 
            {
                throw;
            }
        }
        // End TT#1902-MD - JSmith - Store Services - VSW API Error

        // Begin TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
        public int GetAllStoresListCount()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return StoreServerSessionRemote.GetAllStoresListCount();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail

        //Begin TT#1517-MD -jsobek -Store Service Optimization
        public ProfileList GetAllStoresList()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return StoreServerSessionRemote.GetAllStoresList();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        public int GetReaderLockTimeOut()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return StoreServerSessionRemote.GetReaderLockTimeOut();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        public int GetWriterLockTimeOut()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return StoreServerSessionRemote.GetWriterLockTimeOut();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        

        public eStoreStatus GetStoreStatusForCurrentWeek(DateTime openDt, DateTime closeDt)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return StoreServerSessionRemote.GetStoreStatusForCurrentWeek(openDt, closeDt);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        public void AddStoreProfileToList(StoreProfile sp)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        StoreServerSessionRemote.AddStoreProfileToList(sp);
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
        public void UpdateStoreProfileInList(StoreProfile sp)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        StoreServerSessionRemote.UpdateStoreProfileInList(sp);
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
        //End TT#1517-MD -jsobek -Store Service Optimization

       

 
      

   
        public eStoreStatus GetStoreStatus(WeekProfile baseWeek, DateTime openDt, DateTime closeDt)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return StoreServerSessionRemote.GetStoreStatus(baseWeek, openDt, closeDt);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#2090-MD - JSmith - Performance
        public Dictionary<int, eStoreStatus> GetAllStoresSalesStatus(WeekProfile baseWeek, ArrayList stores)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return StoreServerSessionRemote.GetAllStoresSalesStatus(baseWeek, stores);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public Dictionary<int, eStoreStatus> GetAllStoresStockStatus(WeekProfile baseWeek, ArrayList stores)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return StoreServerSessionRemote.GetAllStoresStockStatus(baseWeek, stores);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2090-MD - JSmith - Performance
       
       

		public bool LoadXMLTransFile(SessionAddressBlock SAB, string fileLocation, int commitLimit, string exceptionFile, bool autoAddCharacteristics)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return StoreServerSessionRemote.LoadXMLTransFile(SAB, fileLocation, commitLimit, exceptionFile, autoAddCharacteristics);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		public bool LoadDelimitedTransFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, int commitLimit, string exceptionFile,
            bool autoAddCharacteristics, char[] characteristicDelimiter, bool useCharacteristicTransaction)
		// END TT#1401 - stodd - add resevation stores (IMO)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						// BEGIN TT#1401 - stodd - add resevation stores (IMO)
                        return StoreServerSessionRemote.LoadDelimitedTransFile(SAB, fileLocation, delimiter, commitLimit, exceptionFile, autoAddCharacteristics, characteristicDelimiter, useCharacteristicTransaction);
						// END TT#1401 - stodd - add resevation stores (IMO)
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		// BEGIN TT#739-MD - STodd - delete stores
		public bool DeleteStoreBatchProcess(SessionAddressBlock SAB)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return StoreServerSessionRemote.DeleteStoreBatchProcess(SAB);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}
		// END TT#739-MD - STodd - delete stores

       



	}

}
