using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDRetail.DataCommon
{
    /// <summary>
    /// Used to hold options for header filters, so we can easily add/modify options without changing parameters everywhere
    /// </summary>
    [Serializable]
    public class FilterHeaderOptions
    {
        public int HN_RID_OVERRIDE = -1;
        public bool USE_WORKSPACE_FIELDS = false;
        public filterTypes filterType;
    }
}
