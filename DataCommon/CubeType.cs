using System;
using System.Collections.Generic;
using System.Text;

namespace MIDRetail.DataCommon
{
	public class eCubeType
	{
		//=======
		// FIELDS
		//=======

		private bool _offset;
		private int _baseId;
		private int _realId;
		private int _level;

		//---------------------
		// Constant definitions
		//---------------------

		public const int cNone = 1;

		// Planning Cubes

		public const int cBasis = 2;
		public const int cDateTotal = 3;
		public const int cStoreTotal = 4;
		public const int cGroupTotal = 5;
		public const int cLowLevelTotal = 6;
		public const int cChainDetail = 7;
		public const int cStoreDetail = 8;
		public const int cLowLevelDetail = 9;
		public const int cWeekDetail = 10;
		public const int cChainBasisDetail = 11;
		public const int cStoreBasisDetail = 12;
		public const int cChainBasisWeekDetail = 13;
		public const int cChainBasisPeriodDetail = 14;
		public const int cChainBasisDateTotal = 15;
		public const int cChainPlanWeekDetail = 16;
		public const int cChainPlanPeriodDetail = 17;
		public const int cChainPlanDateTotal = 18;
		public const int cChainBasisLowLevelTotalDateTotal = 19;
		public const int cChainBasisLowLevelTotalPeriodDetail = 20;
		public const int cChainBasisLowLevelTotalWeekDetail = 21;
		public const int cChainPlanLowLevelTotalDateTotal = 22;
		public const int cChainPlanLowLevelTotalPeriodDetail = 23;
		public const int cChainPlanLowLevelTotalWeekDetail = 24;
		public const int cStoreBasisGroupTotalWeekDetail = 25;
		public const int cStoreBasisGroupTotalPeriodDetail = 26;
		public const int cStoreBasisGroupTotalDateTotal = 27;
		public const int cStoreBasisStoreTotalWeekDetail = 28;
		public const int cStoreBasisStoreTotalPeriodDetail = 29;
		public const int cStoreBasisStoreTotalDateTotal = 30;
		public const int cStoreBasisWeekDetail = 31;
		public const int cStoreBasisPeriodDetail = 32;
		public const int cStoreBasisDateTotal = 33;
		public const int cStorePlanGroupTotalWeekDetail = 34;
		public const int cStorePlanGroupTotalPeriodDetail = 35;
		public const int cStorePlanGroupTotalDateTotal = 36;
		public const int cStorePlanStoreTotalWeekDetail = 37;
		public const int cStorePlanStoreTotalPeriodDetail = 38;
		public const int cStorePlanStoreTotalDateTotal = 39;
		public const int cStorePlanWeekDetail = 40;
		public const int cStorePlanPeriodDetail = 41;
		public const int cStorePlanDateTotal = 42;
		public const int cStoreBasisLowLevelTotalGroupTotalWeekDetail = 43;
		public const int cStoreBasisLowLevelTotalGroupTotalPeriodDetail = 44;
		public const int cStoreBasisLowLevelTotalGroupTotalDateTotal = 45;
		public const int cStoreBasisLowLevelTotalStoreTotalWeekDetail = 46;
		public const int cStoreBasisLowLevelTotalStoreTotalPeriodDetail = 47;
		public const int cStoreBasisLowLevelTotalStoreTotalDateTotal = 48;
		public const int cStoreBasisLowLevelTotalWeekDetail = 49;
		public const int cStoreBasisLowLevelTotalPeriodDetail = 50;
		public const int cStoreBasisLowLevelTotalDateTotal = 51;
		public const int cStorePlanLowLevelTotalGroupTotalWeekDetail = 52;
		public const int cStorePlanLowLevelTotalGroupTotalPeriodDetail = 53;
		public const int cStorePlanLowLevelTotalGroupTotalDateTotal = 54;
		public const int cStorePlanLowLevelTotalStoreTotalWeekDetail = 55;
		public const int cStorePlanLowLevelTotalStoreTotalPeriodDetail = 56;
		public const int cStorePlanLowLevelTotalStoreTotalDateTotal = 57;
		public const int cStorePlanLowLevelTotalWeekDetail = 58;
		public const int cStorePlanLowLevelTotalPeriodDetail = 59;
		public const int cStorePlanLowLevelTotalDateTotal = 60;
		public const int cStoreForecastBasisWeekDetail = 61;
		//Begin Track #6010 - JScott - Bad % Change on Basis2
		public const int cPlan = 62;
		//End Track #6010 - JScott - Bad % Change on Basis2
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		public const int cStoreBasisGradeTotalDateTotal = 63;
		//End TT#2 - JScott - Assortment Planning - Phase 2

		// Assortment Cubes

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//public const int cAssortmentHeaderColorDetail = 62;
		////Begin TT#2 - JScott - Assortment Planning - Phase 2
		////public const int cAssortmentHeaderPackDetail = 63;
		////End TT#2 - JScott - Assortment Planning - Phase 2
		//public const int cAssortmentHeaderTotalDetail = 64;
		//public const int cAssortmentPlaceholderColorDetail = 65;
		////Begin TT#2 - JScott - Assortment Planning - Phase 2
		////public const int cAssortmentPlaceholderPackDetail = 66;
		////End TT#2 - JScott - Assortment Planning - Phase 2
		public const int cAssortmentHeaderColorDetail = 64;
		public const int cAssortmentHeaderTotalDetail = 65;
		public const int cAssortmentPlaceholderColorDetail = 66;
		//End TT#2 - JScott - Assortment Planning - Phase 2
		public const int cAssortmentPlaceholderTotalDetail = 67;
		public const int cAssortmentSummaryGrade = 68;
		public const int cAssortmentSummaryGroupLevel = 69;
		public const int cAssortmentSummaryTotal = 70;
		public const int cAssortmentComponentHeaderGrade = 71;
		public const int cAssortmentComponentHeaderGroupLevel = 72;
		public const int cAssortmentComponentHeaderTotal = 73;
		public const int cAssortmentComponentPlaceholderGrade = 74;
		public const int cAssortmentComponentPlaceholderGroupLevel = 75;
		public const int cAssortmentComponentPlaceholderTotal = 76;
		public const int cAssortmentComponentGroupLevel = 77;
		public const int cAssortmentComponentTotal = 78;
		public const int cAssortmentSubTotal = 79;
		public const int cAssortmentTotal = 80;
		//Begin TT#2 - JScott - Add AssortmentPlaceholderGradeTotal Cube
		public const int cAssortmentPlaceholderGradeTotal = 81;
		//End TT#2 - JScott - Add AssortmentPlaceholderGradeTotal Cube

		// Gaps of 100 should be left after the following constants, as these are used as base points for cube types.
		public const int cAssortmentComponentHeaderGradeSubTotal = 1000;
		public const int cAssortmentComponentHeaderGroupLevelSubTotal = 1100;
		public const int cAssortmentComponentHeaderTotalSubTotal = 1200;
		public const int cAssortmentComponentPlaceholderGradeSubTotal = 1300;
		public const int cAssortmentComponentPlaceholderGroupLevelSubTotal = 1400;
		public const int cAssortmentComponentPlaceholderTotalSubTotal = 1500;

		//-------------------
		// Static definitions
		//-------------------

		static public eCubeType None = new eCubeType(cNone);
		//Begin Track #6010 - JScott - Bad % Change on Basis2
		static public eCubeType Plan = new eCubeType(cPlan);
		//End Track #6010 - JScott - Bad % Change on Basis2
		static public eCubeType Basis = new eCubeType(cBasis);
		static public eCubeType DateTotal = new eCubeType(cDateTotal);
		static public eCubeType StoreTotal = new eCubeType(cStoreTotal);
		static public eCubeType GroupTotal = new eCubeType(cGroupTotal);
		static public eCubeType LowLevelTotal = new eCubeType(cLowLevelTotal);
		static public eCubeType ChainDetail = new eCubeType(cChainDetail);
		static public eCubeType StoreDetail = new eCubeType(cStoreDetail);
		static public eCubeType LowLevelDetail = new eCubeType(cLowLevelDetail);
		static public eCubeType WeekDetail = new eCubeType(cWeekDetail);
		static public eCubeType ChainBasisDetail = new eCubeType(cChainBasisDetail);
		static public eCubeType StoreBasisDetail = new eCubeType(cStoreBasisDetail);
		static public eCubeType ChainBasisWeekDetail = new eCubeType(cChainBasisWeekDetail);
		static public eCubeType ChainBasisPeriodDetail = new eCubeType(cChainBasisPeriodDetail);
		static public eCubeType ChainBasisDateTotal = new eCubeType(cChainBasisDateTotal);
		static public eCubeType ChainPlanWeekDetail = new eCubeType(cChainPlanWeekDetail);
		static public eCubeType ChainPlanPeriodDetail = new eCubeType(cChainPlanPeriodDetail);
		static public eCubeType ChainPlanDateTotal = new eCubeType(cChainPlanDateTotal);
		static public eCubeType ChainBasisLowLevelTotalDateTotal = new eCubeType(cChainBasisLowLevelTotalDateTotal);
		static public eCubeType ChainBasisLowLevelTotalPeriodDetail = new eCubeType(cChainBasisLowLevelTotalPeriodDetail);
		static public eCubeType ChainBasisLowLevelTotalWeekDetail = new eCubeType(cChainBasisLowLevelTotalWeekDetail);
		static public eCubeType ChainPlanLowLevelTotalDateTotal = new eCubeType(cChainPlanLowLevelTotalDateTotal);
		static public eCubeType ChainPlanLowLevelTotalPeriodDetail = new eCubeType(cChainPlanLowLevelTotalPeriodDetail);
		static public eCubeType ChainPlanLowLevelTotalWeekDetail = new eCubeType(cChainPlanLowLevelTotalWeekDetail);
		static public eCubeType StoreBasisGroupTotalWeekDetail = new eCubeType(cStoreBasisGroupTotalWeekDetail);
		static public eCubeType StoreBasisGroupTotalPeriodDetail = new eCubeType(cStoreBasisGroupTotalPeriodDetail);
		static public eCubeType StoreBasisGroupTotalDateTotal = new eCubeType(cStoreBasisGroupTotalDateTotal);
		static public eCubeType StoreBasisStoreTotalWeekDetail = new eCubeType(cStoreBasisStoreTotalWeekDetail);
		static public eCubeType StoreBasisStoreTotalPeriodDetail = new eCubeType(cStoreBasisStoreTotalPeriodDetail);
		static public eCubeType StoreBasisStoreTotalDateTotal = new eCubeType(cStoreBasisStoreTotalDateTotal);
		static public eCubeType StoreBasisWeekDetail = new eCubeType(cStoreBasisWeekDetail);
		static public eCubeType StoreBasisPeriodDetail = new eCubeType(cStoreBasisPeriodDetail);
		static public eCubeType StoreBasisDateTotal = new eCubeType(cStoreBasisDateTotal);
		static public eCubeType StorePlanGroupTotalWeekDetail = new eCubeType(cStorePlanGroupTotalWeekDetail);
		static public eCubeType StorePlanGroupTotalPeriodDetail = new eCubeType(cStorePlanGroupTotalPeriodDetail);
		static public eCubeType StorePlanGroupTotalDateTotal = new eCubeType(cStorePlanGroupTotalDateTotal);
		static public eCubeType StorePlanStoreTotalWeekDetail = new eCubeType(cStorePlanStoreTotalWeekDetail);
		static public eCubeType StorePlanStoreTotalPeriodDetail = new eCubeType(cStorePlanStoreTotalPeriodDetail);
		static public eCubeType StorePlanStoreTotalDateTotal = new eCubeType(cStorePlanStoreTotalDateTotal);
		static public eCubeType StorePlanWeekDetail = new eCubeType(cStorePlanWeekDetail);
		static public eCubeType StorePlanPeriodDetail = new eCubeType(cStorePlanPeriodDetail);
		static public eCubeType StorePlanDateTotal = new eCubeType(cStorePlanDateTotal);
		static public eCubeType StoreBasisLowLevelTotalGroupTotalWeekDetail = new eCubeType(cStoreBasisLowLevelTotalGroupTotalWeekDetail);
		static public eCubeType StoreBasisLowLevelTotalGroupTotalPeriodDetail = new eCubeType(cStoreBasisLowLevelTotalGroupTotalPeriodDetail);
		static public eCubeType StoreBasisLowLevelTotalGroupTotalDateTotal = new eCubeType(cStoreBasisLowLevelTotalGroupTotalDateTotal);
		static public eCubeType StoreBasisLowLevelTotalStoreTotalWeekDetail = new eCubeType(cStoreBasisLowLevelTotalStoreTotalWeekDetail);
		static public eCubeType StoreBasisLowLevelTotalStoreTotalPeriodDetail = new eCubeType(cStoreBasisLowLevelTotalStoreTotalPeriodDetail);
		static public eCubeType StoreBasisLowLevelTotalStoreTotalDateTotal = new eCubeType(cStoreBasisLowLevelTotalStoreTotalDateTotal);
		static public eCubeType StoreBasisLowLevelTotalWeekDetail = new eCubeType(cStoreBasisLowLevelTotalWeekDetail);
		static public eCubeType StoreBasisLowLevelTotalPeriodDetail = new eCubeType(cStoreBasisLowLevelTotalPeriodDetail);
		static public eCubeType StoreBasisLowLevelTotalDateTotal = new eCubeType(cStoreBasisLowLevelTotalDateTotal);
		static public eCubeType StorePlanLowLevelTotalGroupTotalWeekDetail = new eCubeType(cStorePlanLowLevelTotalGroupTotalWeekDetail);
		static public eCubeType StorePlanLowLevelTotalGroupTotalPeriodDetail = new eCubeType(cStorePlanLowLevelTotalGroupTotalPeriodDetail);
		static public eCubeType StorePlanLowLevelTotalGroupTotalDateTotal = new eCubeType(cStorePlanLowLevelTotalGroupTotalDateTotal);
		static public eCubeType StorePlanLowLevelTotalStoreTotalWeekDetail = new eCubeType(cStorePlanLowLevelTotalStoreTotalWeekDetail);
		static public eCubeType StorePlanLowLevelTotalStoreTotalPeriodDetail = new eCubeType(cStorePlanLowLevelTotalStoreTotalPeriodDetail);
		static public eCubeType StorePlanLowLevelTotalStoreTotalDateTotal = new eCubeType(cStorePlanLowLevelTotalStoreTotalDateTotal);
		static public eCubeType StorePlanLowLevelTotalWeekDetail = new eCubeType(cStorePlanLowLevelTotalWeekDetail);
		static public eCubeType StorePlanLowLevelTotalPeriodDetail = new eCubeType(cStorePlanLowLevelTotalPeriodDetail);
		static public eCubeType StorePlanLowLevelTotalDateTotal = new eCubeType(cStorePlanLowLevelTotalDateTotal);
		static public eCubeType StoreForecastBasisWeekDetail = new eCubeType(cStoreForecastBasisWeekDetail);
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		static public eCubeType StoreBasisGradeTotalDateTotal = new eCubeType(cStoreBasisGradeTotalDateTotal);
		//End TT#2 - JScott - Assortment Planning - Phase 2
		static public eCubeType AssortmentHeaderColorDetail = new eCubeType(cAssortmentHeaderColorDetail);
        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        //static public eCubeType AssortmentHeaderPackDetail = new eCubeType(cAssortmentHeaderPackDetail);
        //End TT#2 - JScott - Assortment Planning - Phase 2
        static public eCubeType AssortmentHeaderTotalDetail = new eCubeType(cAssortmentHeaderTotalDetail);
		static public eCubeType AssortmentPlaceholderColorDetail = new eCubeType(cAssortmentPlaceholderColorDetail);
        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        //static public eCubeType AssortmentPlaceholderPackDetail = new eCubeType(cAssortmentPlaceholderPackDetail);
        //End TT#2 - JScott - Assortment Planning - Phase 2
        static public eCubeType AssortmentPlaceholderTotalDetail = new eCubeType(cAssortmentPlaceholderTotalDetail);
		static public eCubeType AssortmentSummaryGrade = new eCubeType(cAssortmentSummaryGrade);
		static public eCubeType AssortmentSummaryGroupLevel = new eCubeType(cAssortmentSummaryGroupLevel);
		static public eCubeType AssortmentSummaryTotal = new eCubeType(cAssortmentSummaryTotal);
		static public eCubeType AssortmentComponentHeaderGrade = new eCubeType(cAssortmentComponentHeaderGrade);
		static public eCubeType AssortmentComponentHeaderGroupLevel = new eCubeType(cAssortmentComponentHeaderGroupLevel);
		static public eCubeType AssortmentComponentHeaderTotal = new eCubeType(cAssortmentComponentHeaderTotal);
		static public eCubeType AssortmentComponentPlaceholderGrade = new eCubeType(cAssortmentComponentPlaceholderGrade);
		static public eCubeType AssortmentComponentPlaceholderGroupLevel = new eCubeType(cAssortmentComponentPlaceholderGroupLevel);
		static public eCubeType AssortmentComponentPlaceholderTotal = new eCubeType(cAssortmentComponentPlaceholderTotal);
		static public eCubeType AssortmentComponentGroupLevel = new eCubeType(cAssortmentComponentGroupLevel);
		static public eCubeType AssortmentComponentTotal = new eCubeType(cAssortmentComponentTotal);
		static public eCubeType AssortmentSubTotal = new eCubeType(cAssortmentSubTotal);
		static public eCubeType AssortmentTotal = new eCubeType(cAssortmentTotal);
		static public eCubeType AssortmentComponentHeaderGradeSubTotal = new eCubeType(cAssortmentComponentHeaderGradeSubTotal, 0);
		static public eCubeType AssortmentComponentHeaderGroupLevelSubTotal = new eCubeType(cAssortmentComponentHeaderGroupLevelSubTotal, 0);
		static public eCubeType AssortmentComponentHeaderTotalSubTotal = new eCubeType(cAssortmentComponentHeaderTotalSubTotal, 0);
		static public eCubeType AssortmentComponentPlaceholderGradeSubTotal = new eCubeType(cAssortmentComponentPlaceholderGradeSubTotal, 0);
		static public eCubeType AssortmentComponentPlaceholderGroupLevelSubTotal = new eCubeType(cAssortmentComponentPlaceholderGroupLevelSubTotal, 0);
		static public eCubeType AssortmentComponentPlaceholderTotalSubTotal = new eCubeType(cAssortmentComponentPlaceholderTotalSubTotal, 0);
		//Begin TT#2 - JScott - Add AssortmentPlaceholderGradeTotal Cube
		static public eCubeType AssortmentPlaceholderGradeTotal = new eCubeType(cAssortmentPlaceholderGradeTotal, 0);
		//End TT#2 - JScott - Add AssortmentPlaceholderGradeTotal Cube

		//=============
		// CONSTRUCTORS
		//=============

		public eCubeType(int aId)
		{
			_offset = false;
			_baseId = aId;
			_realId = aId;
			_level = 0;
		}

		public eCubeType(int aId, int aLevel)
		{
			_offset = true;
			_baseId = aId;
			_realId = aId + aLevel;
			_level = aLevel;
		}

		//===========
		// PROPERTIES
		//===========

		public int Id
		{
			get
			{
				return _realId;
			}
		}

		public int Level
		{
			get
			{
				return _level;
			}
		}

		//========
		// METHODS
		//========

		override public bool Equals(object obj)
		{
			try
			{
				return (this == (eCubeType)obj);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public int GetHashCode()
		{
			return _realId;
		}

		public static bool operator ==(eCubeType left, eCubeType right)
		{
			try
			{
				if ((object)left == null && (object)right == null)
				{
					return true;
				}
				else if ((object)left != null && (object)right != null)
				{
					if (left._offset == right._offset)
					{
						if (!left._offset)
						{
							if (left._realId == right._realId)
							{
								return true;
							}
							else
							{
								return false;
							}
						}
						else
						{
							if (left._baseId == right._baseId)
							{
								return true;
							}
							else
							{
								return false;
							}
						}
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public static bool operator !=(eCubeType left, eCubeType right)
		{
			try
			{
				return !(left == right);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
