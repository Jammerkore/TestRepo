using System;
using System.Collections.Generic;
using System.Text;

namespace MIDRetail.DataCommon
{
    /// <summary>
    /// Identifies the size units associated with a given size RID
    /// </summary>
    [Serializable]
    public struct SizeUnits
    {
        private int _sizeRID;
        private int _sizeUnits;
        /// <summary>
        /// Instantiates an instance of this structure
        /// </summary>
        /// <param name="aSizeRID">Size RID</param>
        /// <param name="aSizeUnits">Size Units associated with the given size RID</param>
        public SizeUnits(int aSizeRID, int aSizeUnits)
        {
            _sizeRID = aSizeRID;
            _sizeUnits = aSizeUnits;
        }
        /// <summary>
        /// Gets the Size RID
        /// </summary>
        public int RID
        {
            get
            {
                return _sizeRID;
            }
        }
        /// <summary>
        /// Gets the Size Units
        /// </summary>
        public int Units
        {
            get
            {
                return _sizeUnits;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is SizeUnits)
            {
                SizeUnits su = (SizeUnits)obj;
                if (_sizeRID == su.RID
                    && _sizeUnits == su.Units)
                {
                    return true;
                }
            }
            return false;

        }
        public override int GetHashCode()
        {
            return _sizeRID.GetHashCode() ^ _sizeUnits.GetHashCode();
        }
    }

    /// <summary>
    /// Dictionary whose key-value pair is SizeRID and SizeUnits, respectively.
    /// </summary>
    [Serializable]
    public class SizeUnitRun : Dictionary<int, SizeUnits>
    {
        /// <summary>
        /// Gets the array of SizeRIDs in this run
        /// </summary>
        public int[] SizeRIDs
        {
            get
            {
                int[] sizeRIDArray = new int[base.Count];
                base.Keys.CopyTo(sizeRIDArray, 0);
                return sizeRIDArray;
            }
        }
        /// <summary>
        /// Gets the Size Units in the size run for the given size RID
        /// </summary>
        /// <param name="aSizeRID">Size RID for which the Size Units are to be retrieved</param>
        /// <returns>Size Units in the size run for the given size RID</returns>
        public int GetSizeUnits(int aSizeRID)
        {
            SizeUnits sizeUnits;
            if (base.TryGetValue(aSizeRID, out sizeUnits))
            {
                return sizeUnits.Units;
            }
            return 0;
        }
        public override bool Equals(object obj)
        {
            SizeUnitRun sur = obj as SizeUnitRun;
            if (sur == null)
            {
                return false;
            }
            if (this.Count != sur.Count)
            {
                return false;
            }
            foreach (SizeUnits su in this.Values)
            {
                if (su.Units != sur.GetSizeUnits(su.RID))
                {
                    return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            int hash = 0;
            foreach (SizeUnits su in this.Values)
            {
                hash = hash ^ su.GetHashCode();
            }
            return hash;
        }
    }
}
