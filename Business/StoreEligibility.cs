using System;
using System.Collections;
using System.Diagnostics;
using System.Data;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for StoreEligibility.
	/// </summary>
    //public class StoreEligibility
    //{
    //    private SessionAddressBlock _SAB;

    //    public StoreEligibility(SessionAddressBlock SAB)
    //    {
    //        _SAB = SAB;
    //    }

    //    /// <summary>
    //    /// Returns a hashtable containing a list of all stores and a bool representing the
    //    /// stores eligibility.
    //    /// </summary>
    //    /// <remarks>
    //    /// THIS FUNCTION IS NOT COMPLETE!!  It currently returns ALL stores as TRUE (eligible).
    //    /// </remarks>
    //    /// <param name="node_rid"></param>
    //    /// <param name="yearWeek"></param>
    //    /// <returns></returns>
    //    public Hashtable GetStoreEligibilty(int node_rid, int yearWeek)
    //    {
    //        Hashtable hashtable = new Hashtable();

    //        ProfileList allStoreList = _SAB.StoreServerSession.GetAllStoresList();
    //        foreach(StoreProfile sp in allStoreList.ArrayList)
    //        {
    //            hashtable.Add(sp.Key,true);
    //        }

    //        return hashtable;
    //    }

		
    //}
}
