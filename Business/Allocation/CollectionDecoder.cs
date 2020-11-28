// TT#1432 - Moved this object from "common" to "Business.Allocation"
using System;
using System.Collections;
using System.Collections.Generic; // TT#1391 - TMW New Action
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Diagnostics;

using MIDRetail.DataCommon;
using MIDRetail.Common;


namespace MIDRetail.Business.Allocation
{

    #region Collection Decoder

	public class CollectionDecoder
	{
		//=============
		// Variables
		//=============
        private ApplicationSessionTransaction _appSessionTrans;   // TT#1432 - Size Dimension Constraints not working
		private CollectionSets _collectionSets;
		//private CollectionFringeSets _collectionFringeSets;
		private CollectionRuleSets _collectionRuleSets;
		private eSizeCollectionType _collectionType;
		private bool _continue;
		private Hashtable _minMaxHash; // MID Track 3844 Constraints not working
		private Hashtable _ruleHash;   // MID Track 3844 Constraints not working
		private Hashtable _sglHash;   // MID Track 4244 Constraints not working
        private Dictionary<int, SizeCodeProfile> _sizeCodeProfileDict; // TT#1391 - TMW New Action

		//=============
		// CONstructors
		//=============

		public CollectionDecoder(ApplicationSessionTransaction aAppTran, CollectionSets aCollection, Hashtable sglHash) //Issue 4244 // TT#1432 - Size Dimension Constraints not working
		{
            _appSessionTrans = aAppTran;                                      // TT#1432 - Size Dimension Constraints not working
			_collectionSets = aCollection;
			_sglHash = sglHash;			// Issue 4244
			_collectionType = eSizeCollectionType.MinMaxCollection;
			_minMaxHash = new Hashtable(); // MID Track 3844 Constraints not working
            _sizeCodeProfileDict = new Dictionary<int, SizeCodeProfile>(); // TT#1391 - TMW New Action
		}

		// These rules are used during fringe processing
        public CollectionDecoder(ApplicationSessionTransaction aAppTran, CollectionRuleSets aCollection, Hashtable sglHash)  // Issue 4244  // TT#1432 - Size Dimension Constraints not working
        {// TT#1432 - Size Dimension Constraints not working
            _appSessionTrans = aAppTran;                                      // TT#1432 - Size Dimension Constraints not working
            _sglHash = sglHash;			// Issue 4244
			_collectionRuleSets = aCollection;
			_collectionType = eSizeCollectionType.RulesCollection;
			_ruleHash = new Hashtable(); // MID Track 3844 Constraints not working
            _sizeCodeProfileDict = new Dictionary<int, SizeCodeProfile>(); // TT#1391 - TMW New Action
		}


        //public object GetItemForStore(int stRid, int colorRid, SizeCodeProfile sizeCodeProfile)  // TT#1391 - TMW New Action
        public object GetItemForStore(int stRid, int colorRid, int sizeCodeRID)                    // TT#1391 - TMW New Action
		{
			try
			{
				int sglRid = Include.NoRID;
				if (_sglHash.ContainsKey(stRid))
					sglRid = (int)_sglHash[stRid];

                //object returnObj = GetItem(sglRid, colorRid, sizeCodeProfile); // TT#1391 - TMW New Action
                object returnObj = GetItem(sglRid, colorRid, sizeCodeRID);       // TT31391 - TMW New Action
				
				return returnObj;
			}
			catch
			{
				throw;
			}
		}

		// public object GetItem(int sglRid, int colorRid, int sizeCodeRid) // MID Track 3492 Size Need Method with Contraints not allocating correctly
        //public object GetItem(int sglRid, int colorRid, SizeCodeProfile sizeCodeProfile) // MID Track 3492 Size Need Method with Constraints not allocating correctly  // TT#1391 - TMW New Action
        public object GetItem(int sglRid, int colorRid, int sizeCodeRid)    // TT#1391 - TMW New Action
		{
            //int sizeCodeRid = sizeCodeProfile.Key;        // TT#1391 - TMW New Action
			object returnObj = null;
			MinMaxItemBase minMax = null;
			RuleItemBase rule = null;
			_continue = true;
			if (_collectionType == eSizeCollectionType.MinMaxCollection)
			{
                //minMax = GetItemMinMax(sglRid, colorRid, sizeCodeProfile); // MID Track 3492 Size Need Method with constraint not allocating correctly // TT#1391 - TMW New Action
                minMax = GetItemMinMax(sglRid, colorRid, sizeCodeRid);       // TT#1391 - TMW New Action
				returnObj = minMax;
			}
			else if (_collectionType == eSizeCollectionType.RulesCollection)
			{
                //rule = GetItemRule(sglRid, colorRid, sizeCodeProfile); // MID Track 3492 Size Need Method with Constraints not allocating correctly  // TT#1391 - TMW New Action
                rule = GetItemRule(sglRid, colorRid, sizeCodeRid);       // TT#1391 - TMW New Action
				returnObj = rule;
			}

			return returnObj;
		}

        //public void DebugSetsCollection()
        //{
        //    //PROCESS SETS AND ALL DESCENDANTS
        //    Debug.WriteLine("---------------------------------------------");
        //    if (_collectionType == eSizeCollectionType.RulesCollection)
        //    {
        //        foreach (ItemRuleSet oItemSet in _collectionRuleSets)
        //        {
        //            Debug.WriteLine("RID " + oItemSet.MethodRid.ToString() +
        //                " SGL " + oItemSet.SglRid.ToString() + " RULE " + oItemSet.Rule.ToString() +
        //                " QTY " + oItemSet.Qty.ToString());
									
        //            //PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
        //            foreach (ItemRuleAllColor oItemAllColor in oItemSet.collectionRuleAllColors)
        //            {
        //                DebugItem(1, oItemAllColor);

        //                foreach (ItemRuleSizeDimension oItemSizeDimension in oItemAllColor.collectionRuleSizeDimensions)
        //                {
        //                    DebugItem(2, oItemSizeDimension);

        //                    foreach (ItemRuleSize oItemSize in oItemSizeDimension.collectionRuleSizes)
        //                    {
        //                        DebugItem(3, oItemSize);
        //                    }
        //                }
        //            }

        //            //PROCESS COLOR LEVEL AND ALL DESCENDANTS
        //            foreach (ItemRuleColor oItemColor in oItemSet.collectionRuleColors)
        //            {
        //                DebugItem(1, oItemColor);

        //                foreach (ItemRuleSizeDimension oItemSizeDimension in oItemColor.collectionRuleSizeDimensions)
        //                {
        //                    DebugItem(2, oItemSizeDimension);

        //                    foreach (ItemRuleSize oItemSize in oItemSizeDimension.collectionRuleSizes)
        //                    {
        //                        DebugItem(3, oItemSize);
        //                    }

        //                }
        //            }
        //        }

        //    }
        //}

        //private void DebugItem(int level, RuleItemBase pItem)
        //{
        //    string sLevel = string.Empty;
        //    switch (level)
        //    {
        //        case 1:
        //            sLevel = "  ";
        //            break;
        //        case 2: 
        //            sLevel = "    ";
        //            break;
        //        case 3:
        //            sLevel = "      ";
        //            break;
        //        default:
        //            sLevel = "--";
        //            break;
        //    }
        //    Debug.WriteLine(sLevel + "SGL " + pItem.SglRid.ToString() +
        //        " COLOR " + pItem.ColorCodeRid.ToString() +
        //        " DIM " + pItem.DimensionsRid.ToString() +
        //        " SIZE/SIZES " + pItem.SizeCodeRid.ToString() + "/" + pItem.SizesRid.ToString() +
        //        " Rule " + pItem.Rule.ToString() +
        //        " QTY " + pItem.Qty.ToString());
        //}
        #region SizeCodeProfile
        private SizeCodeProfile GetSizeCodeProfile(int aSizeCodeRID)
        {
            SizeCodeProfile sizeCodeProfile;
            if (!_sizeCodeProfileDict.TryGetValue(aSizeCodeRID, out sizeCodeProfile))
            {
                sizeCodeProfile = _appSessionTrans.GetSizeCodeProfile(aSizeCodeRID);  // TT#1432 - Size Dimension Constraints not working
                _sizeCodeProfileDict.Add(aSizeCodeRID, sizeCodeProfile);
            }
            return sizeCodeProfile;
        }
        #endregion SizeCodeProfile

        #region MinMax (Constraints)
        // J.Ellis unnecessary comments removed
        //private MinMaxItemBase GetItemMinMax(int sglRid, int colorRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need with constraints not allocating correctly  // TT#1391 - TMW New Action
        private MinMaxItemBase GetItemMinMax(int sglRid, int colorRid, int sizeCodeRid)                    // TT#1391 - TMW New Action
		{
			// begin MID Track 3844 Constraints not working
			MinMaxItemBase minMax;
			long minMaxSGL_ColorKey = ((long) sglRid << 32) + colorRid;
			Hashtable sglColorHash;
			if (_minMaxHash.Contains(minMaxSGL_ColorKey))
			{
				sglColorHash = (Hashtable)_minMaxHash[minMaxSGL_ColorKey];
			}
			else
			{
				sglColorHash = new Hashtable();
				_minMaxHash.Add(minMaxSGL_ColorKey, sglColorHash);
			}
            // begin TT#1391 - TMW new Action
            //if (sglColorHash.Contains(aSizeCodeProfile.Key))
            //{
            //    minMax = (MinMaxItemBase)sglColorHash[aSizeCodeProfile.Key];
            //}
            //else
            minMax = (MinMaxItemBase)sglColorHash[sizeCodeRid];
            if (minMax == null)
                // end TT#1391 - TMW New Action
			{
				// end MID Track 3844 Constratints not working
				try
				{
                    // begin TT#1391 - TMW New Action
                    SizeCodeProfile aSizeCodeProfile = GetSizeCodeProfile(sizeCodeRid);
                    // end TT#1391 - TMW New Action
                    minMax = new MinMaxItemBase(sglRid, colorRid, aSizeCodeProfile); // MID Track 3844 Constraints not working
					_continue = true; // MID Track 3844 Constraints not working
					while (_continue)
					{
						minMax = SearchMinMaxCollection(minMax);
                        // begin MID Track 3844 Constraints not working
						//if ((minMax.Min != 0 && minMax.Max != int.MaxValue && minMax.Mult != Include.Undefined)
						//	 || minMax.DimensionsRid == Include.MaskedRID && minMax.SizeCodeRid == Include.MaskedRID && minMax.ColorCodeRid == Include.MaskedRID && minMax.SglRid == Include.MaskedRID) // MID Track 3844 Constraints not working
						//	//|| minMax.DimensionsRid ==Include.NoRID && minMax.SizeCodeRid == Include.NoRID && minMax.ColorCodeRid == Include.NoRID && minMax.SglRid == Include.NoRID) // MID Track 3844 Constraints not working // MID Track 3492 Size Need with constraints not allocating correctly
						//{
						//	_continue = false;
						//}
						if (minMax.Min != 0 && minMax.Max != int.MaxValue && minMax.Mult != Include.Undefined)
						{
							_continue = false;
						}
							// end MID Track 3844 Constraints not working
						else
						{
							// begin MID Track 3492 Size Need with constraints not allocating correctly
							if (minMax.ColorCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
							{
								if (minMax.DimensionsRid != Include.MaskedRID) // MID Track 3844 Constraints not working
								{
									if (minMax.SizeCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
									{
										minMax.SizeCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
									}
									else
									{
										minMax.DimensionsRid = Include.MaskedRID; // MID Track 3844 Constraints not working
										//minMax.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3844 Constraint not working
									}
								}
								else
								{
									minMax.DimensionsRid = aSizeCodeProfile.SizeCodeSecondaryRID;
									minMax.SizeCodeRid = aSizeCodeProfile.Key;
									minMax.ColorCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
								}
							}
							else
							{
								if (minMax.DimensionsRid != Include.MaskedRID) // MID Track 3844 Constraints not working  
								{
									if (minMax.SizeCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
									{
										minMax.SizeCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
									}
									else
									{
										minMax.DimensionsRid = Include.MaskedRID; // MID Track 3844 Constraints not working
										//minMax.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3844 Contraints not working
									}
								}
								else
								{
									// begin MID Track 3844 Constraints not working
									if (minMax.SglRid == Include.NoRID)
									{
										_continue = false;
									}
									else
									{
										minMax.SglRid = Include.NoRID; // MID Track 3844 Constraints not working
										minMax.ColorCodeRid = colorRid;
										minMax.DimensionsRid = aSizeCodeProfile.SizeCodeSecondaryRID; 
										minMax.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3492 Size Need Method with constraint not allocating correctly
									}
									// end MID Track 3844 Constraints not working
								}
							}
						}
					}
				    sglColorHash.Add(aSizeCodeProfile.Key, minMax); // MID Track 3844 Constraints not working
					//return minMax;

				}
				catch (Exception)
				{
					throw;
				}
			}
			return minMax; // MID Track 3844 Constraints not working
		}

		public MinMaxItemBase SearchMinMaxCollection(MinMaxItemBase minMax)
		{
			foreach (ItemSet oItemSet in _collectionSets)
			{
				if (oItemSet.SglRid == minMax.SglRid) 
				{
					if (minMax.ColorCodeRid == Include.MaskedRID) // MID Track 3844 Constraints not working
					{
						//PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
						foreach (ItemAllColor oItemAllColor in oItemSet.collectionAllColors)
						{
							foreach (ItemSizeDimension oItemSizeDimension in oItemAllColor.collectionSizeDimensions)
							{
								if (oItemSizeDimension.DimensionsRid == minMax.DimensionsRid) // MID Track 3492 Size Need With Constraints Not Allocating Correctly
								{                                                             // MID Track 3492 Size Need With Constraints Not Allocating Correctly
									foreach (ItemSize oItemSize in oItemSizeDimension.collectionSizes)
									{
										if (oItemSize.SizeCodeRid == minMax.SizeCodeRid) // MID Track 3492 Size Need With Constraints Not Allocating Correctly
										{                                                // MID Track 3492 Size Need With Constraints Not Allocating Correctly
											if (minMax.Min == 0)
												minMax.Min = oItemSize.Min;
											if (minMax.Max == int.MaxValue)
												minMax.Max = oItemSize.Max;
											if (minMax.Mult == Include.Undefined)
												minMax.Mult = oItemSize.Mult;
											break;                                       // MID Track 3492 Size Need With Constraints Not Allocating Correctly
										}                                                // MID Track 3492 Size Need With Constraints Not Allocating Correctly
									}
									// begin MID Track 3492 Size Need With Constraints Not Allocating Correctly
									if (minMax.Min == 0)
										minMax.Min = oItemSizeDimension.Min;
									if (minMax.Max == int.MaxValue)
										minMax.Max = oItemSizeDimension.Max;
									if (minMax.Mult == Include.Undefined)
										minMax.Mult = oItemSizeDimension.Mult;
									break;
									// end  MID Track 3492 Size Need With Constraints Not Allocating Correctly
								}   // MID Track 3492 Size Need With Constraints Not allocating Correctly
							}

							if (minMax.Min == 0)
								minMax.Min = oItemAllColor.Min;
							if (minMax.Max == int.MaxValue)
								minMax.Max = oItemAllColor.Max;
							if (minMax.Mult == Include.Undefined)
								minMax.Mult = oItemAllColor.Mult;
							break;
						}
					}
					else
					{
						//PROCESS COLOR LEVEL AND ALL DESCENDANTS
						foreach (ItemColor oItemColor in oItemSet.collectionColors)
						{
							if (oItemColor.ColorCodeRid == minMax.ColorCodeRid)
							{
								foreach (ItemSizeDimension oItemSizeDimension in oItemColor.collectionSizeDimensions)
								{
									if (oItemSizeDimension.DimensionsRid == minMax.DimensionsRid) // MID Track 3492 Size Need With Constraints Not Allocating Correctly
									{                                                            // MID Track 3492 Size Need With Constraints Not Allocating Correctly
										foreach (ItemSize oItemSize in oItemSizeDimension.collectionSizes)
										{
											if (oItemSize.SizeCodeRid == minMax.SizeCodeRid)
											{
												minMax.Min = oItemSize.Min;
												minMax.Max = oItemSize.Max;
												minMax.Mult = oItemSize.Mult;
												break;
											}
										}
										// begin MID Track 3492 Size Need With Constraints Not Allocating Correctly
										if (minMax.Min == 0)
											minMax.Min = oItemSizeDimension.Min;
										if (minMax.Max == int.MaxValue)
											minMax.Max = oItemSizeDimension.Max;
										if (minMax.Mult == Include.Undefined)
											minMax.Mult = oItemSizeDimension.Mult;
										break;
										// end  MID Track 3492 Size Need With Constraints Not Allocating Correctly
									}   // MID Track 3492 Size Need With Constraints Not Allocating Correctly
								}
								
								if (minMax.Min == 0)
									minMax.Min = oItemColor.Min;
								if (minMax.Max == int.MaxValue)
									minMax.Max = oItemColor.Max;
								if (minMax.Mult == Include.Undefined)
									minMax.Mult = oItemColor.Mult;
								break; // MID Track 3492 Size Need With Constraints Not Allocating Correctly
							}
						}
					}

					if (minMax.Min == 0)
						minMax.Min = oItemSet.Min;
					if (minMax.Max == int.MaxValue)
						minMax.Max = oItemSet.Max;
					if (minMax.Mult == Include.Undefined)
						minMax.Mult = oItemSet.Mult;
					break; // MID Track 3492 Size Need With Constraints Not Allocating Correctly
				}
			}

			return minMax;
		}
		#endregion

		#region Size Rule
		// J. Ellis Removed unnecessary comments for readablity // MID Track 3844 Contraints not working
        //private RuleItemBase GetItemRule(int sglRid, int colorRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need Method with constraints not allocating correctly // TT#1391 - TMW New Action
        private RuleItemBase GetItemRule(int sglRid, int colorRid, int sizeCodeRid)                    // TT#1391 - TMW New Action
		{
			// begin MID Track 3844 Constraints not working
			RuleItemBase rule;
			long ruleSGL_ColorKey = ((long) sglRid << 32) + colorRid;
			Hashtable sglColorHash;
			if (_ruleHash.Contains(ruleSGL_ColorKey))
			{
				sglColorHash = (Hashtable)_ruleHash[ruleSGL_ColorKey];
			}
			else
			{
				sglColorHash = new Hashtable();
				_ruleHash.Add(ruleSGL_ColorKey, sglColorHash);
			}
            // begin TT#1391 - TMW New Action
            //if (sglColorHash.Contains(aSizeCodeProfile.Key))
            //{
            //    rule = (RuleItemBase)sglColorHash[aSizeCodeProfile.Key];
            //}
            //else
            rule = (RuleItemBase)sglColorHash[sizeCodeRid];
            if (rule == null)
                // end TT#1391 - TMW New Action
			{
				// end MID Track 3844 Constraints not working
				try
				{
                    // begin TT#1391 - TMW New Action
                    SizeCodeProfile aSizeCodeProfile = GetSizeCodeProfile(sizeCodeRid);
                    // end TT#1391 - TMW New Action
                    rule = new RuleItemBase(sglRid, colorRid, aSizeCodeProfile); // MID Track 3844 Constraints not working // MID Track 3492 Size Need Method with Constraints not allocating correctly
					_continue = true; // MID Track 3844 Constraints not working
					while (_continue)
					{
						rule = SearchRuleCollection(rule);
                        // begin MID Track 3844 Constraints not working
						//if ((rule.Rule != Include.Undefined)
						//	|| rule.DimensionsRid == Include.MaskedRID && rule.SizeCodeRid == Include.MaskedRID && rule.ColorCodeRid == Include.MaskedRID && rule.SglRid == Include.NoRID) // MID Track 3844 Constraints not working
						//{
						//	_continue = false;
						//}
						if (rule.Rule != Include.Undefined)
						{
							_continue = false;
						}
						else
						{
							// end MID Track 3844 Constraints not working
							// begin MID Track 3492 Size Need with constraints not allocating correctly
							if (rule.ColorCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
							{
								if (rule.DimensionsRid != Include.MaskedRID) // MID Track 3844 Constraints not working
								{
									if (rule.SizeCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
									{
										rule.SizeCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
									}
									else
									{
										rule.DimensionsRid = Include.MaskedRID; // MID Track 3844 Constraints not working
										//	rule.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3844 Constraints not working
									}
								}
								else
								{
									rule.DimensionsRid = aSizeCodeProfile.SizeCodeSecondaryRID;
									rule.SizeCodeRid = aSizeCodeProfile.Key;
									rule.ColorCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
								}
							}
							else
							{
								if (rule.DimensionsRid != Include.MaskedRID) // MID Track 3844 Constraints not working  
								{
									if (rule.SizeCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
									{
										rule.SizeCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
									}
									else
									{
										rule.DimensionsRid = Include.MaskedRID; // MID Track 3844 Constraints not working
										// rule.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3844 Constraints not working
									}
								}
								else
								{
									// begin MID Track Constraints not working
									if (rule.SglRid == Include.NoRID) // MID Track 3844 Constraints not working
									{
										_continue = false;
									}
									else
									{
										rule.SglRid = Include.NoRID; // MID Track 3844 Constraints not working
										rule.ColorCodeRid = colorRid;
										rule.DimensionsRid = aSizeCodeProfile.SizeCodeSecondaryRID; 
										rule.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3492 Size Need Method with constraint not allocating correctly
									}
									// end MID Track Constraints not working
								}
							}
						} // MID Track 3844 Constraints not working
					} 
					sglColorHash.Add(aSizeCodeProfile.Key, rule); // MID track 3844 Constraints not working
					//return rule; // MID Track 3844 Constraints not working

				}
				catch (Exception)
				{
					throw;
				}
			} // MID Track 3844 Constraints not working
			return rule; // MID Track 3844 Constraints not working
		}

		public RuleItemBase SearchRuleCollection(RuleItemBase aRuleItem)
		{
			foreach (ItemRuleSet oItemRuleSet in _collectionRuleSets)
			{
				if (oItemRuleSet.SglRid == aRuleItem.SglRid) 
				{
					if (aRuleItem.ColorCodeRid == Include.MaskedRID) // MID Track 3844 Constraints not working
					{
						//PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
						foreach (ItemRuleAllColor oItemRuleAllColor in oItemRuleSet.collectionRuleAllColors)
						{
							foreach (ItemRuleSizeDimension oItemRuleSizeDimension in oItemRuleAllColor.collectionRuleSizeDimensions)
							{
								if (oItemRuleSizeDimension.DimensionsRid == aRuleItem.DimensionsRid)
								{
									foreach (ItemRuleSize oItemRuleSize in oItemRuleSizeDimension.collectionRuleSizes) // MID Track 3782 Rules Decoder not working
									{                                                                                  // MID Track 3782 Rules Decoder not working
										if (oItemRuleSize.SizeCodeRid == aRuleItem.SizeCodeRid)
										{
											aRuleItem.Rule = oItemRuleSize.Rule;
											aRuleItem.Qty = oItemRuleSize.Qty;
											break;
										}
									}
									if (aRuleItem.Rule == Include.Undefined)                                           // MID Track 3782 Rules Decoder not working
									{                                                                                  // MID Track 3782 Rules Decoder not working
										aRuleItem.Rule = oItemRuleSizeDimension.Rule;                                  // MID Track 3782 Rules Decoder not working
										aRuleItem.Qty = oItemRuleSizeDimension.Qty;                                    // MID Track 3782 Rules Decoder not working
									}                                                                                  // MID Track 3782 Rules Decoder not working
									break;                                                                         // MID Track 3782 Rules Decoder not working
								}                                                                                      // MID Track 3782 Rules Decoder not working
							}

							if (aRuleItem.Rule == Include.Undefined)
							{
								aRuleItem.Rule = oItemRuleAllColor.Rule;
								aRuleItem.Qty = oItemRuleAllColor.Qty;
							}
							break;
						}
					}
					else
					{
						//PROCESS COLOR LEVEL AND ALL DESCENDANTS
						foreach (ItemRuleColor oItemRuleColor in oItemRuleSet.collectionRuleColors)
						{
							if (oItemRuleColor.ColorCodeRid == aRuleItem.ColorCodeRid)
							{
								foreach (ItemRuleSizeDimension oItemRuleSizeDimension in oItemRuleColor.collectionRuleSizeDimensions)
								{
									if (oItemRuleSizeDimension.DimensionsRid == aRuleItem.DimensionsRid) // MID Track 3782 Rules Decoder not working
									{                                                                    // MID Track 3782 Rules Decoder not working
										foreach (ItemRuleSize oItemRuleSize in oItemRuleSizeDimension.collectionRuleSizes)
										{
											if (oItemRuleSize.SizeCodeRid == aRuleItem.SizeCodeRid)
											{
												aRuleItem.Rule = oItemRuleSize.Rule;
												aRuleItem.Qty = oItemRuleSize.Qty;
												break;
											}
										}
										if (aRuleItem.Rule == Include.Undefined)                                           // MID Track 3782 Rules Decoder not working
										{                                                                                  // MID Track 3782 Rules Decoder not working
											aRuleItem.Rule = oItemRuleSizeDimension.Rule;                                  // MID Track 3782 Rules Decoder not working
											aRuleItem.Qty = oItemRuleSizeDimension.Qty;                                    // MID Track 3782 Rules Decoder not working
										}                                                                                  // MID Track 3782 Rules Decoder not working
										break;                                                                         // MID Track 3782 Rules Decoder not working
									}                                                                                      // MID Track 3782 Rules Decoder not working
								}
								
								if (aRuleItem.Rule == Include.Undefined)
								{
									aRuleItem.Rule = oItemRuleColor.Rule;
									aRuleItem.Qty = oItemRuleColor.Qty;
								}
								break;
							}
						}
					}
					if (aRuleItem.Rule == Include.Undefined)
					{
						aRuleItem.Rule = oItemRuleSet.Rule;
						aRuleItem.Qty = oItemRuleSet.Qty;
					}
					break;
				}
			}

			return aRuleItem;
		}
		#endregion
	}


	#endregion 

}