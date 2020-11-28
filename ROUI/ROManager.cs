using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace Logility.ROUI
{
    abstract public class ROManager
    {
        private SessionAddressBlock _sab;

        public ROManager(SessionAddressBlock SAB)
        {
            _sab = SAB;
        }

        public SessionAddressBlock SAB { get { return _sab; } }

        abstract public int GetViewRID();

    }


}
