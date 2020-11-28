using System;
using System.Collections.Generic;
using System.Text;

namespace MIDRetail.DataCommon
{
    public enum ePackPatternType
    {
        Vendor = 0,
        BuildPacksMethod = 1,
        OptionPack = 2
    }

    /// <summary>
    /// Defines criteria used to generate packs in a Work Up Buy
    /// </summary>
    [Serializable]
    public abstract class PackPattern : ICloneable
    {
        #region Fields
        private int _packPatternRID;
        private string _packPatternName;
        private int _packMultiple;
        private SizeUnitRun _sizeRun;
        private int _maxPatternPacks;
        #endregion Fields

        #region Constructor
        /// <summary>
        /// Creates an instance of the PackPattern Class containing a Size Run
        /// </summary>
        /// <param name="aPackPatternRID">RID that identifies the Pack Pattern</param>
        /// <param name="aPackPatternName">Unique name of the pattern within a given combination.</param>
        /// <param name="aSizeUnitList">List of the sizes and units in the pack size run</param>
        public PackPattern(int aPackPatternRID, string aPackPatternName, List<SizeUnits> aSizeUnitList)
        {
            _packPatternRID = aPackPatternRID;
            if (aPackPatternName == null
                || aPackPatternName == String.Empty
                || aPackPatternName.Length > 50)
            {

                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_al_PackPatternNameRequired);
            }
            _packPatternName = aPackPatternName;
            SizeUnitRun sizeRun = new SizeUnitRun();
            foreach (SizeUnits su in aSizeUnitList)
            {
                sizeRun.Add(su.RID, su);
            }
            BuildSizeRun(sizeRun);
        }
        /// <summary>
        /// Creates an instance of the PackPattern Class containing a Size Run
        /// </summary>
        /// <param name="aPackPatternRID">RID that identifies the Pack Pattern</param>
        /// <param name="aPackPatternName">Unique name of the pattern within a given combination.</param>
        /// <param name="aSizeUnitArray">SizeUnits array of sizes in the pack size run (no duplicate size codes)</param>
        public PackPattern(int aPackPatternRID, string aPackPatternName, SizeUnits[] aSizeUnitArray)
        {
            _packPatternRID = aPackPatternRID;
            if (aPackPatternName == null
                || aPackPatternName == String.Empty
                || aPackPatternName.Length > 50)
            {

                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_al_PackPatternNameRequired);
            }
            _packPatternName = aPackPatternName;
            SizeUnitRun sizeRun = new SizeUnitRun();
            foreach (SizeUnits su in aSizeUnitArray)
            {
                sizeRun.Add(su.RID, su);
            }
            BuildSizeRun(sizeRun);
        }
        /// <summary>
        /// Creates an instance of the PackPattern Class containing a Size Run
        /// </summary>
        /// <param name="aPackPatternRID">RID that identifies the Pack Pattern</param>
        /// <param name="aPackPatternName">Unique name of the pattern within a given combination.</param>
        /// <param name="aSizeUnitRun">SizeUnitRun containing the sizes and units for this pack's size run</param>
        public PackPattern(int aPackPatternRID, string aPackPatternName, SizeUnitRun aSizeUnitRun)
        {
            _packPatternRID = aPackPatternRID;
            if (aPackPatternName == null
                || aPackPatternName == String.Empty
                || aPackPatternName.Length > 50)
            {
                throw new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackPatternNameRequired);
            }
            _packPatternName = aPackPatternName;
            BuildSizeRun(aSizeUnitRun);
        }
        /// <summary>
        /// Creates an instance of the PackPattern Class containing no size run
        /// </summary>
        /// <param name="aPackPatternRID">RID that identifies the Pack Pattern</param>
        /// <param name="aPackPatternName">Unique name of the pattern within a given combination.</param>
        /// <param name="aPackMultiple">Pack Multiple for the Pack Pattern</param>
        /// <param name="aMaxPatternPacks">Maximum packs of this multiple that a generated header may contain.</param>
        public PackPattern(int aPackPatternRID, string aPackPatternName, int aPackMultiple, int aMaxPatternPacks)
        {
            _packPatternRID = aPackPatternRID;
            if (aPackPatternName == null
                || aPackPatternName == String.Empty
                || aPackPatternName.Length > 50)
            {
                throw new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackPatternNameRequired);
            }
            _packPatternName = aPackPatternName;
            if (aPackMultiple < 1)
            {
                throw new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackMultipleMustBeGT_0);
            }
            if (aMaxPatternPacks < 1)
            {
                throw new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_MaxPacksMustBeGT_0);
            }
            _maxPatternPacks = (int)aMaxPatternPacks;
            _packMultiple = (int)aPackMultiple;
            _sizeRun = null;
        }
        /// <summary>
        /// Builds the PackPattern class using a given Size Run
        /// </summary>
        /// <param name="aSizeUnitRun">SizeUnitRun describing the Pack's size run</param>
        private void BuildSizeRun(SizeUnitRun aSizeUnitRun)
        {
            if (PatternType == ePackPatternType.Vendor)
            {
                throw new MIDException(eErrorLevel.warning,(int)eMIDTextCode.msg_al_VendorPatternCannotContainSzRun);
            }
            int packMultiple = 0;
            SizeUnitRun sizeRun = new SizeUnitRun();
            foreach (SizeUnits su in aSizeUnitRun.Values)
            {
                packMultiple += su.Units;
                sizeRun.Add(su.RID, new SizeUnits(su.RID, su.Units));
            }
            if (packMultiple < 1)
            {
                throw new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackMultipleMustBeGT_0);
            }
            _packMultiple = packMultiple;
            _sizeRun = sizeRun;
            _maxPatternPacks = 1;
        }
        #endregion Constructor

        #region Properties
        /// <summary>
        /// Identifies the PackPattern Type
        /// </summary>
        public abstract ePackPatternType PatternType
        {
            get;
        }
        /// <summary>
        /// Indicates whether Pack Pattern is read only
        /// </summary>
        public abstract bool PatternIsReadOnly
        {
            get;
        }
        public int PackPatternRID
        {
            get { return _packPatternRID; }
            set { _packPatternRID = value; }
        }
        public string PatternName
        {
            get { return _packPatternName; }
        }
        /// <summary>
        /// Indicates if this PackPattern contains a Size Run
        /// </summary>
        public bool PatternIncludesSizeRun
        {
            get { return (_sizeRun != null); }
        }
        /// <summary>
        /// Gets Pack Multiple
        /// </summary>
        public int PackMultiple
        {
            get { return _packMultiple; }
        }
        /// <summary>
        /// Gets Maximum number of packs of this Pattern that may exist on a header
        /// </summary>
        public int MaxPatternPacks
        {
            get { return _maxPatternPacks; }
        }

        /// <summary>
        /// Gets a list of the Size RIDs associated with this pattern.
        /// </summary>
        public int[] SizeRIDs
        {
            get 
            {
                if (_sizeRun == null)
                {
                    return null;
                }
                return (int[])_sizeRun.SizeRIDs.Clone();
            }
        }
        /// <summary>
        /// Gets a copy of the SizeUnitRun associated with this pattern
        /// </summary>
        public SizeUnitRun SizeRun
        {
            get
            {
                if (_sizeRun == null)
                {
                    return null;
                }
                SizeUnitRun sizeRun = new SizeUnitRun();
                foreach (SizeUnits su in _sizeRun.Values)
                {
                    sizeRun.Add(su.RID, new SizeUnits(su.RID, su.Units));
                }
                return sizeRun;
            }
        }
        /// <summary>
        /// Gets a copy of the SizeUnitsList associated with this pattern
        /// </summary>
        public List<SizeUnits> SizeUnitsList
        {
            get
            {
                List<SizeUnits> sizeList = new List<SizeUnits>();
                if (_sizeRun == null)
                {
                    return sizeList;
                }
                foreach (SizeUnits su in _sizeRun.Values)
                {
                    sizeList.Add(new SizeUnits(su.RID, su.Units));
                }
                return sizeList;
            }
        }
        /// <summary>
        /// Gets the count of sizes having units in this pattern.
        /// </summary>
        public int CountOfSizesWithUnits
        {
            get
            {
                if (_sizeRun == null)
                {
                    return 0;
                }
                int count = 0;
                foreach (SizeUnits su in _sizeRun.Values)
                {
                    if (su.Units > 0)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Sets the pack pattern name.
        /// </summary>
        /// <param name="aPackPatternName">Desired pack pattern name</param>
        /// <param name="aStatusReason">The reason the set action failed; if the set action is successful, this message is null.</param>
        /// <returns>True: set was successful, aStatusReason is null in this case; False: set failed, aStatusReason contains a reason for the failure.</returns>
        public bool SetPackPatternName(string aPackPatternName, out MIDException aStatusReason)
        {
            if (PatternIsReadOnly)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackPatternReadOnly);
                return false;
            }
            if (aPackPatternName == null
                || aPackPatternName == String.Empty
                || aPackPatternName.Length > 50)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackPatternNameRequired);
                return false;
            }
            _packPatternName = aPackPatternName;
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the maximum pattern packs which is the number of packs with this pattern that may co-exist on the same header.
        /// </summary>
        /// <param name="aMaxPatternPacks">Desired maximum pattern packs</param>
        /// <param name="aStatusReason">The reason the set action failed; if the set action is successful, this message is null.</param>
        /// <returns></returns>
        public bool SetMaxPatternPacks(int aMaxPatternPacks, out MIDException aStatusReason)
        {
            if (PatternIsReadOnly)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackPatternReadOnly);
                return false;
            }
            if (PatternIncludesSizeRun)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_CannotModifyMaxPackWhenSzRun);
                return false;
            }
            if (aMaxPatternPacks < 1)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_MaxPacksMustBeGT_0);
                return false;
            }
            if (PatternType == ePackPatternType.Vendor)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_VendorPatternCannotBeModified);
                return false;
            }
            aStatusReason = null;
            _maxPatternPacks = aMaxPatternPacks;
            return true;
        }
        /// <summary>
        /// Sets the pack multiple for this Pack Pattern
        /// </summary>
        /// <param name="aPackMultiple">The desired pack multiple</param>
        /// <param name="aStatusReason">The reason the set action failed; if the set action is successful, this message is null.</param>
        /// <returns>True:  Set was successful; False: Set failed in which case, aStatusReason will give the reason for the failure.</returns>
        /// <remarks>See also the method "RemoveSizeRunFromPattern".</remarks>
        public bool SetPackMultiple(int aPackMultiple, out MIDException aStatusReason)
        {
            if (PatternIsReadOnly)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackPatternReadOnly);
                return false;
            }
            if (PatternIncludesSizeRun)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode. msg_al_PackMultCannotBeModifiedWhenSzRun);
                return false;
            }
            if (aPackMultiple < 1)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackMultipleMustBeGT_0);
                return false;
            }
            if (PatternType == ePackPatternType.Vendor)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_VendorPatternCannotBeModified);
                return false;
            }
            _packMultiple = aPackMultiple;
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Gets the units associated with the identified size in this PackPattern's size run
        /// </summary>
        /// <param name="aSizeRID">Size Code RID that identifies the size</param>
        /// <returns>Number of units in this size for this PackPattern</returns>
        public int GetSizeUnits(int aSizeRID)
        {
            if (PatternIncludesSizeRun)
            {
                SizeUnits sizeUnits;
                if (_sizeRun.TryGetValue(aSizeRID, out sizeUnits))
                {
                    return sizeUnits.Units;
                }
            }
            return 0;
        }
        /// <summary>
        /// Replaces the size units for a given size with the PackPattern's size run
        /// </summary>
        /// <param name="aSizeUnits">SizeUnits that identifies the Size Code RID and size units to replace</param>
        /// <param name="aStatusReason">The reason the set action failed; if the set action is successful, this message is null.</param>
        /// <remark>Changing size units for a size WILL change the PackPattern's Pack Multiple</remark>"
        public bool SetSizeInRun(SizeUnits aSizeUnits, out MIDException aStatusReason)
        {
            if (PatternIncludesSizeRun)
            {
                if (aSizeUnits.Units < 1)
                {
                    aStatusReason = new MIDException(eErrorLevel.warning,(int)eMIDTextCode.msg_al_SzRunUnitsMustBeGT_0);
                    return false;
                }
                int newMultiple =
                    _packMultiple
                    - GetSizeUnits(aSizeUnits.RID)
                    + aSizeUnits.Units;
                if (newMultiple == 0)
                {
                    aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ReplacingSzRunCausesInvalidPackMult);
                    return false;
                }
                _sizeRun.Remove(aSizeUnits.RID);
                _sizeRun.Add(aSizeUnits.RID, new SizeUnits(aSizeUnits.RID, aSizeUnits.Units));
                _packMultiple = newMultiple;
                aStatusReason = null;
                return true;
            }
            aStatusReason = new MIDException (eErrorLevel.warning, (int)eMIDTextCode.msg_al_CannotSetSizeValueWhenNoSzRun);
            return false;
        }
        /// <summary>
        /// Removes a size from the PackPattern's size run provided the Pack Multiple is not reduced to zero
        /// </summary>
        /// <param name="aSizeRID">Size Code RID that is to be removed</param>
        /// <param name="aStatusReason">The reason the set action failed; if the set action is successful, this message is null.</param>
        /// <remarks>Size can only be removed from size run when other sizes with positive units remain in the run</remarks>
        public bool RemoveSizeFromRun(int aSizeRID, out MIDException aStatusReason)
        {
            if (PatternIncludesSizeRun)
            {
                int sizeUnits = GetSizeUnits(aSizeRID);
                if (_packMultiple == sizeUnits)
                {
                    aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ReplacingSzRunCausesInvalidPackMult);
                    return false;
                }
                _packMultiple -= GetSizeUnits(aSizeRID);
                _sizeRun.Remove(aSizeRID);
                
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Replaces or adds a size run in the PackPattern
        /// </summary>
        /// <param name="aSizeRunList">List of SizeUnits in the size run</param>
        public void ReplaceSizeRunInPattern(List<SizeUnits> aSizeRunList)
        {
            SizeUnitRun sizeUnitRun = new SizeUnitRun();
            foreach (SizeUnits su in aSizeRunList)
            {
                sizeUnitRun.Add(su.RID, su);
            }
            ReplaceSizeRunInPattern(sizeUnitRun);
        }
        /// <summary>
        /// Replaces or adds a size run in the PackPattern
        /// </summary>
        /// <param name="aSizeUnitRun">Size run to add to the pattern</param>
        public void ReplaceSizeRunInPattern(SizeUnitRun aSizeUnitRun)
        {
            BuildSizeRun(aSizeUnitRun);
        }
        /// <summary>
        /// Removes a size run from the pack pattern
        /// </summary>
        /// <param name="aStatusReason">The reason the set action failed; if the set action is successful, this message is null.</param>
        /// <returns>True: when removal is successful; False: when removal fails</returns>
        public bool RemoveSizeRunFromPattern(out MIDException aStatusReason)
        {
            return RemoveSizeRunFromPattern(_packMultiple, out aStatusReason);
        }
        /// <summary>
        /// Removes a size run from the pack pattern
        /// </summary>
        /// <param name="aNewPackMultiple">Pack Multiple of the pattern after removal is complete</param>
        /// <param name="aStatusReason">The reason the remove action failed; if the remove action is successful, this message is null.</param>
        /// <returns>True: when removal is successful; False: when removal fails</returns>
        public bool RemoveSizeRunFromPattern(int aNewPackMultiple, out MIDException aStatusReason)
        {
            if (PatternIsReadOnly)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackPatternReadOnly);
                return false;
            }
            if (PatternType == ePackPatternType.Vendor)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_VendorPatternCannotBeModified);
                return false;
            }
            if (aNewPackMultiple < 1)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackMultipleMustBeGT_0);
                return false;
            }
            _packMultiple = (int)aNewPackMultiple;
            _sizeRun = null;
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Determines if a given object has the same content as this object.
        /// </summary>
        /// <param name="obj">The object to compare to this object</param>
        /// <returns>True: both objects contain the same content; False: objects do not contain same content</returns>
        public override bool Equals(object obj)
        {
            PackPattern pp = obj as PackPattern;
            if (pp == null)
            {
                return false;
            }
            if (_packMultiple != pp.PackMultiple)
            {
                return false;
            }
            if (_maxPatternPacks != pp.MaxPatternPacks)
            {
                return false;
            }
            if (_sizeRun != null)
            {
                if (pp._sizeRun == null)
                {
                    return false;
                }
                if (_sizeRun.Count != pp._sizeRun.Count)
                {
                    return false;
                }
                foreach (SizeUnits su in _sizeRun.Values)
                {
                    if (su.Units != pp.GetSizeUnits(su.RID))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            if (_sizeRun == null)
            {
                return _packMultiple.GetHashCode();
            }
            return _packMultiple.GetHashCode() ^ _sizeRun.GetHashCode();
        }
        /// <summary>
        /// Returns a clone of this object.
        /// </summary>
        /// <returns>A copy of the content of this object</returns>
        public abstract object Clone();
        #endregion Methods
    }

    /// <summary>
    /// Describes a Pack Pattern associated with a specific BuildPacksMethod
    /// </summary>
    [Serializable]
    public class BuildPacksMethod_PackPattern : PackPattern
    {
        private int _methodRID;
        /// <summary>
        /// Creates an instance of a BuildPacksMethod Pack Pattern containing a Size Run
        /// </summary>
        /// <param name="aMethodRID">RID of the BuildPacksMethod</param>
        /// <param name="aPackPatternRID">RID that identifies the PackPattern within the Method</param>
        /// <param name="aPackPatternName">Unique name of the pattern within the method and combination</param>
        /// <param name="aSizeUnitList">List of Size Units within the size run</param>
        public BuildPacksMethod_PackPattern(int aMethodRID, int aPackPatternRID, string aPackPatternName, List<SizeUnits> aSizeUnitList)
            : base(aPackPatternRID, aPackPatternName, aSizeUnitList)
        {
            _methodRID = aMethodRID;
        }
        /// <summary>
        /// Creates an instance of a BuildPacksMethod Pack Pattern containing a Size Run
        /// </summary>
        /// <param name="aMethodRID">RID of the BuildPacksMethod</param>
        /// <param name="aPackPatternRID">RID that identifies the PackPattern within the Method</param>
        /// <param name="aPackPatternName">Unique name of the pattern within the method and combination</param>
        /// <param name="aSizeUnitRun">SizeRun associated with this Pack Pattern</param>
        public BuildPacksMethod_PackPattern(int aMethodRID, int aPackPatternRID, string aPackPatternName, SizeUnitRun aSizeUnitRun)
            : base(aPackPatternRID, aPackPatternName, aSizeUnitRun)
        {
            _methodRID = aMethodRID;
        }
        /// <summary>
        /// Creates an instance of a BuildPacksMethod Pack Pattern with no particular size run
        /// </summary>
        /// <param name="aMethodRID">RID of the BuildPacksMethod</param>
        /// <param name="aPackPatternRID">RID that identifies the PackPattern within the Method</param>
        /// <param name="aPackPatternName">Unique name of the pattern within the method and combination</param>
        /// <param name="aPackMultiple">Pack Multiple associated with this Pack Pattern</param>
        public BuildPacksMethod_PackPattern(int aMethodRID, int aPackPatternRID, string aPackPatternName, int aPackMultiple, int aMaxPatternPacks)
            : base(aPackPatternRID, aPackPatternName, aPackMultiple, aMaxPatternPacks)
        {
            _methodRID = aMethodRID;

        }
        /// <summary>
        /// Indicates if the pattern is reas only or may be updated.
        /// </summary>
        public override bool  PatternIsReadOnly
        {
        	get { return false; }
        }
        /// <summary>
        /// Gets the PackPatternType
        /// </summary>
        public override ePackPatternType PatternType
        {
            get { return ePackPatternType.BuildPacksMethod; }
        }
        /// <summary>
        /// Creates a clone of this object
        /// </summary>
        /// <returns>A clone of this object</returns>
        public override object Clone()
        {
            if (PatternIncludesSizeRun)
            {
                return new BuildPacksMethod_PackPattern(
                     _methodRID,
                     PackPatternRID,
                     PatternName,
                     SizeRun);
            }
            return new BuildPacksMethod_PackPattern(
                _methodRID,
                PackPatternRID,
                PatternName,
                PackMultiple,
                MaxPatternPacks);
        }
    }

    /// <summary>
    /// Describes a Pack Pattern associated with Vendor contraints 
    /// </summary>
    [Serializable]
    public class Vendor_PackPattern : PackPattern, ICloneable
    {
        private int _bPC_RID;
        /// <summary>
        /// Creates an instance of a Pack Pattern (with no particular size run)
        /// </summary>
        /// <param name="aBPC_RID">RID that identifies the Build Pack Criteria</param>
        /// <param name="aPackPatternRID">RID that identifies the PackPattern within the Build Pack Criteria</param>
        /// <param name="aPackPatternName">Unique name of the pattern within the method and combination</param>
        /// <param name="aPackMultiple">Pack Multiple associated with the Pack Pattern</param>
        public Vendor_PackPattern(int aBPC_RID, int aPackPatternRID, string aPackPatternName, int aPackMultiple, int aMaxPatternPacks)
            : base(aPackPatternRID, aPackPatternName, aPackMultiple, aMaxPatternPacks)
        {
            _bPC_RID = aBPC_RID;

        }
        /// <summary>
        /// Gets the PackPatternType
        /// </summary>
        public override ePackPatternType PatternType
        {
            get { return ePackPatternType.Vendor; }
        }
        public override bool PatternIsReadOnly
        {
            get { return true; }
        }
        public override object Clone()
        {
            return new Vendor_PackPattern(
               _bPC_RID,
               PackPatternRID,
               PatternName,
               PackMultiple,
               MaxPatternPacks);
        }
    }
    /// <summary>
    /// Describes a pack option witin a build packs method
    /// </summary>
    [Serializable]
    public class OptionPack_PackPattern : PackPattern, ICloneable
    {
        private string _packName;
        public OptionPack_PackPattern(string aPackName, int aPackPatternRID, string aPackPatternName, SizeUnitRun aSizeUnitRun)
            : base(aPackPatternRID, aPackPatternName, aSizeUnitRun)
        {
            _packName = aPackName;
        }
        public OptionPack_PackPattern(string aPackName, int aPackPatternRID, string aPackPatternName, List<SizeUnits> aSizeUnitRun)
            : base(aPackPatternRID, aPackPatternName, aSizeUnitRun)
        {
            _packName = aPackName;
        }
        public OptionPack_PackPattern(string aPackName, int aPackPatternRID, string aPackPatternName, SizeUnits[] aSizeUnitRun)
            : base(aPackPatternRID, aPackPatternName, aSizeUnitRun)
        {
            _packName = aPackName;
        }
        public override ePackPatternType PatternType
        {
            get { return ePackPatternType.OptionPack; }
        }
        public override bool PatternIsReadOnly
        {
            get { return true; }
        }
        // begin TT#565 Tab4
        public string PackName
        {
            get { return _packName; }
        }
        // end TT#565 Tab4
        public override object Clone()
        {
            return new OptionPack_PackPattern(
                _packName,
                PackPatternRID,
                PatternName,
                SizeRun);
        }
    }

    /// <summary>
    /// Describes a pack pattern combination (valid packs that may be generated on a header).
    /// </summary>
    [Serializable]
    public abstract class PackPatternCombo : ICloneable
    {
        private int _patternComboRID;
        private string _patternComboName;
        private bool _patternComboSelected;
        private List<PackPattern> _patternList;
        /// <summary>
        /// Creates an instance of the PackPatternCombo.
        /// </summary>
        /// <param name="aPatternComboRID">RID of the Combination</param>
        /// <param name="aPatternComboName">Combination name </param>
        /// <param name="aPatternComboSelected">True: Combination is selected; False;</param>
        /// <param name="aPackPatternList">List of PackPatterns belonging to the Combination</param>
        public PackPatternCombo(
            int aPatternComboRID, 
            string aPatternComboName, 
            bool aPatternComboSelected, 
            List<PackPattern> aPackPatternList)
        {
            _patternComboRID = aPatternComboRID;
            _patternComboName = aPatternComboName;
            _patternComboSelected = aPatternComboSelected;
            if (aPackPatternList.Count < 1)
            {
                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_al_ComboMustContainAtLeast1Pattern);
            }
            List<PackPattern> packPatternList = new List<PackPattern>();
            foreach (PackPattern pp in aPackPatternList)
            {
                if (pp.PatternType != PackPatternType)
                {
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_al_ComboAndMemberPatternTypesMustMatch);
                }
                packPatternList.Add((PackPattern)pp.Clone());
            }
            _patternList = packPatternList;
        }
        /// <summary>
        /// Gets the Pack Pattern Type
        /// </summary>
        public abstract ePackPatternType PackPatternType
        {
            get;
        }
        /// <summary>
        /// Gets the Combination RID
        /// </summary>
        public int ComboRID
        {
            get { return _patternComboRID; }
            set { _patternComboRID = value; }
        }
        /// <summary>
        /// Gets the Combination Name
        /// </summary>
        public string ComboName
        {
            get { return _patternComboName; }
        }
        /// <summary>
        /// Indicates whether the Combination is selected for consideration.
        /// </summary>
        public bool ComboSelected
        {
            get { return _patternComboSelected; }
            set { _patternComboSelected = value; }
        }
        ///// <summary>
        ///// Gets a copy of the list of pack patterns belonging to this combination.
        ///// </summary>
        //public List<PackPattern> PackPatternList
        //{
        //    get 
        //    {
        //        List<PackPattern> packPattern = new List<PackPattern>();
        //        foreach (PackPattern pp in _patternList)
        //        {
        //            packPattern.Add((PackPattern)pp.Clone());
        //        }
        //        return packPattern;
        //    }
        //}
        /// <summary>
        /// Gets a copy of the list of pack patterns belonging to this combination.
        /// </summary>
        public PackPatternList PackPatternList
        {
            get
            {
                PackPatternList packPatternList = new PackPatternList();
                foreach (PackPattern pp in _patternList)
                {
                    packPatternList.Add((PackPattern)pp.Clone());
                }
                return packPatternList;
            }
        }
        /// <summary>
        /// Sets the PackPatternList associated with this combination
        /// </summary>
        /// <param name="aPackPatternList">The list of patterns belonging to this combination</param>
        /// <param name="aStatusReason">The reason the set fails; null if the set succeeds</param>
        /// <returns>True: Set was successful in which case the aStatusReason is null; False: Set failed and the reason for failure is in aStatusReason.</returns>
        public bool SetPackPatternList(List<PackPattern> aPackPatternList, out MIDException aStatusReason)
        {
            if (aPackPatternList == null
                || aPackPatternList.Count == 0)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ComboMustContainAtLeast1Pattern);
                return false;
            }
            List<PackPattern> packPatternList = new List<PackPattern>();
            foreach (PackPattern pp in aPackPatternList)
            {
                if (pp.PatternType != PackPatternType)
                {
                    aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ComboAndMemberPatternTypesMustMatch);
                    return false;
                }
                packPatternList.Add((PackPattern)pp.Clone());
            }
            _patternList = packPatternList;
            aStatusReason = null;
            return true;
        }
        public abstract object Clone();
    }
    /// <summary>
    /// BuildPacksMethod Pack Pattern Combination.  Combinations of this type reside on the database with the method.  
    /// </summary>
    [Serializable]
    public class BuildPacksMethod_Combo : PackPatternCombo
    {
        private int _method_RID;
        public BuildPacksMethod_Combo(
            int aBuildPacksMethodRID, 
            int aPatternComboRID,
            string aPatternComboName,
            bool aPatternComboSelected,
            List<PackPattern> aBuildPacksMethod_PatternList)
            : base(aPatternComboRID, aPatternComboName, aPatternComboSelected, aBuildPacksMethod_PatternList)
        {
            _method_RID = aBuildPacksMethodRID;
        }
        /// <summary>
        /// Gets the Pattern type of the Combination
        /// </summary>
        public override ePackPatternType PackPatternType
        {
            get { return ePackPatternType.BuildPacksMethod; }
        }
        /// <summary>
        /// Gets a clone of this pack pattern combination
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new BuildPacksMethod_Combo(
                _method_RID,
                this.ComboRID,
                this.ComboName,
                this.ComboSelected,
                PackPatternList);
        }
    }

    /// <summary>
    /// Vendor Combination.  Combinations of this type reside in the BuildPackCriteria table independent of the BuildPackMethod; a BuildPackMethod may reference a Vendor Combination but may mot modify it. 
    /// </summary>
    [Serializable]
    public class Vendor_Combo : PackPatternCombo, ICloneable
    {
        private int _bPC_RID;
        /// <summary>
        /// Creates an instance of the Vendor Combination.
        /// </summary>
        /// <param name="aBPC_RID">The RID in the BuildPackCriteria table that identifies the Vendor (BuildPacksCriteria) pattern combinations.</param>
        /// <param name="aPatternComboRID">The RID in the BuildPackCriteria that identifies a combination defined for the vendor.</param>
        /// <param name="aPatternComboName">The name of the vendor combination.</param>
        /// <param name="aPatternComboSelected">Indicates whether this vendor combination has been selected for consideration in the BuildPackMethod</param>
        /// <param name="aVendor_PatternList">List of the Vendor Patterns belonging to this vendor combination.</param>
         public Vendor_Combo(
            int aBPC_RID, 
            int aPatternComboRID,
            string aPatternComboName,
            bool aPatternComboSelected,
            List<PackPattern> aVendor_PatternList)
            : base(aPatternComboRID, aPatternComboName, aPatternComboSelected, aVendor_PatternList)
        {
            _bPC_RID = aBPC_RID;
        }
        /// <summary>
        /// Gets the Pack Pattern Type of this Combination.
        /// </summary>
        public override ePackPatternType PackPatternType
        {
            get { return ePackPatternType.Vendor; }
        }
        /// <summary>
        /// Gets a clone of this vendor pattern combination.
        /// </summary>
        /// <returns>Clone of this vendor combaination</returns>
        public override object Clone()
        {
            return new Vendor_Combo(
                _bPC_RID,
                this.ComboRID,
                this.ComboName,
                this.ComboSelected,
                PackPatternList);
        }
    }

    public class OptionPack_Combo : PackPatternCombo, ICloneable
    {
        private int _optionPackID;
        public OptionPack_Combo(
            int aOptionPackID,
            int aPatternComboRID,
            string aPatternComboName,
            bool aPatternComboSelected,
            List<PackPattern> aOptionPack_PatternList)
            : base(aPatternComboRID, aPatternComboName, aPatternComboSelected, aOptionPack_PatternList)
        {
            _optionPackID = aOptionPackID;
        }
        public override ePackPatternType PackPatternType
        {
            get { return ePackPatternType.OptionPack; }
        }
        public int OptionPackID
        {
            get { return _optionPackID; }
            set { _optionPackID = value; }
        }
        public override object Clone()
        {
            return new OptionPack_Combo(
                _optionPackID,
                ComboRID,
                ComboName,
                ComboSelected,
                PackPatternList);
        }
    }

    [Serializable]
    public class PackPatternList : List<PackPattern>
    {
        // begin TT#744 - Use orig Pack fitting logic; remove bulk
        public bool AllPacksIncludeSizeRun
        {
            get
            {
                foreach (PackPattern pp in this)
                {
                    if (!pp.PatternIncludesSizeRun)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        // end TT#744 - Use orig Pack fitting logic; remove bulk
    }
    [Serializable]
    public class OptionPackList : List<PackPatternList>
    {
    }
}
