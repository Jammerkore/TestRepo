using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using System.Collections;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace Logility.ROUI
{
    /// <summary>
    /// Data that is defined at the manager level and used in the view
    /// This data is set only once per instance of the ladder screen
    /// </summary>
    abstract public class ROPlanStoreManagerData : ROPlanManagerData
    {

        public ROPlanStoreManagerData(SessionAddressBlock aSAB, PlanOpenParms aOpenParms)
            : base(aSAB, aOpenParms)
        {

        }

    }
    /// <summary>
    /// Data that is defined per view
    /// </summary>
    abstract public class ROStoreViewData : ROPlanViewData
    {
        

        public ROStoreViewData(int viewRID, ROPlanStoreManagerData managerData)
            : base(managerData)
        {
            
        }

        
    }
}
