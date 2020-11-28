using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public class WindowsUtilities
    {
        #region Fields
        //=======
        // FIELDS
        //=======
        protected SessionAddressBlock _SAB; 

        #endregion Fields
        public WindowsUtilities(SessionAddressBlock SAB)
        {
            _SAB = SAB;
        }

        #region Methods
        //=======
        // METHODS
        //=======

        #endregion Methods
    }

    
}
