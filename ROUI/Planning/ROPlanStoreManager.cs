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
//using Infragistics.UltraChart.Shared.Styles; // TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options

namespace Logility.ROUI
{
  
    abstract public class ROPlanStoreManager : ROPlanManager
    {
        

        public ROPlanStoreManager(SessionAddressBlock SAB, PlanOpenParms aOpenParms)
            : base(SAB, aOpenParms)
        {

        }

        
       
    }
}
