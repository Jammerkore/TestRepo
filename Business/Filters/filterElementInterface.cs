using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    /// <summary>
    /// Standard interface for UI elements
    /// </summary>
    public interface IFilterElement
    {
        void LoadFromCondition(filter f, filterCondition condition);
        bool IsValid(filter f, filterCondition condition);
        void SaveToCondition(ref filter f, ref filterCondition condition);
        void SetElementBase(elementBase eb, filterEntrySettings groupSettings);
        void ClearControls();
    }
}
