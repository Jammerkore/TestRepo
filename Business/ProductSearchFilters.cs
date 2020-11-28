using System;
using System.Collections;
using System.Windows.Forms;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    /// <summary>
	/// This ProductSearchFilter filters out products based upon a user definition.
    /// </summary>

    //[Serializable]
    //public class ProductSearchFilter
    //{
    //    //=======
    //    // FIELDS
    //    //=======

    //    private SessionAddressBlock _SAB;
    //    private ProductFilterData _filterDL;
    //    private ProductSearchFilterDefinition _filterDef;

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    public ProductSearchFilter(
    //        SessionAddressBlock aSAB)
    //    {
    //        try
    //        {
    //            _SAB = aSAB;

    //            _filterDL = new ProductFilterData();

    //            _filterDef = new ProductSearchFilterDefinition(
    //                _SAB,
    //                _SAB.ClientServerSession,
    //                _filterDL, 
    //                null, 
    //                0);
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========


    //    //========
    //    // METHODS
    //    //========

    //    /// <summary>
    //    /// Applies this Filter to a given NodeCharProfileList.
    //    /// </summary>
    //    /// <param name="aNodeCharProfileList">
    //    /// The NodeCharProfileList to apply the Filter to.
    //    /// </param>
    //    /// <returns>
    //    /// A boolean indicating if the characteristics of the product pass the filter.
    //    /// </returns>

    //    public bool ApplyFilter(ProfileList aNodeCharProfileList)
    //    {
    //        IEnumerator enumerator;
    //        QueryOperand operand = null;
    //        bool selectItem = true;

    //        try
    //        {
    //            if (_filterDef.CharacteristicOperandList.Count > 0)
    //            {
    //                enumerator = _filterDef.CharacteristicOperandList.GetEnumerator();
    //                selectItem = ProcessCharacteristic(enumerator, ref operand, false, aNodeCharProfileList);
    //            }

    //            return selectItem;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private bool ProcessCharacteristic(IEnumerator aEnumerator, ref QueryOperand aOperand, bool aNegateComparison, ProfileList aNodeCharProfileList)
    //    {
    //        bool currentReturn = false;

    //        try
    //        {
    //            aOperand = GetNextOperand(aEnumerator);
    //            // Begin TT#189 MD - JSmith - Filter Performance
    //            if (aOperand == null)
    //            {
    //                return currentReturn;
    //            }
    //            // End TT#189 MD

    //            if (aOperand.GetType() == typeof(GenericQueryLeftParenOperand))
    //            {
    //                currentReturn = ProcessCharacteristic(aEnumerator, ref aOperand, false, aNodeCharProfileList);
    //                if (aNegateComparison)
    //                {
    //                    if (currentReturn)
    //                    {
    //                        currentReturn = false;
    //                    }
    //                    else
    //                    {
    //                        currentReturn = true;
    //                    }
    //                }
    //                aOperand = GetNextOperand(aEnumerator);
    //                // Begin TT#189 MD - JSmith - Filter Performance
    //                if (aOperand == null)
    //                {
    //                    return currentReturn;
    //                }
    //                // End TT#189 MD
    //            }
    //            else if (aOperand.GetType() == typeof(DataQueryNotOperand))
    //            {
    //                return ProcessCharacteristic(aEnumerator, ref aOperand, true, aNodeCharProfileList);
    //            }
    //            else
    //            {
    //                if (aOperand.GetType() == typeof(ProdCharQueryCharacteristicMainOperand))
    //                {
    //                    bool foundValue = false;
    //                    foreach (ProductCharValueProfile pcvp in ((ProdCharQueryCharacteristicMainOperand)aOperand).ProductCharValueProfList)
    //                    {
    //                        NodeCharProfile ncp = (NodeCharProfile)aNodeCharProfileList.FindKey(pcvp.ProductCharRID);
    //                        if (ncp != null &&
    //                            ncp.ProductCharValueRID == pcvp.Key)
    //                        {
    //                            foundValue = true;
    //                            break;
    //                        }
    //                    }
    //                    if (aNegateComparison)
    //                    {
    //                        if (foundValue)
    //                        {
    //                            currentReturn = false;
    //                        }
    //                        else
    //                        {
    //                            currentReturn = true;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        currentReturn = foundValue;
    //                    }
    //                }

    //                aOperand = GetNextOperand(aEnumerator);
    //                // Begin TT#189 MD - JSmith - Filter Performance
    //                if (aOperand == null)
    //                {
    //                    return currentReturn;
    //                }
    //                // End TT#189 MD
    //            }

    //            if (aOperand.GetType() == typeof(GenericQueryRightParenOperand))
    //            {
    //                return currentReturn;
    //            }
    //            else if (aOperand.GetType() == typeof(GenericQueryAndOperand))
    //            {
    //                return ProcessCharacteristic(aEnumerator, ref aOperand, aNegateComparison, aNodeCharProfileList) && currentReturn;
    //            }
    //            else
    //            {
    //                return ProcessCharacteristic(aEnumerator, ref aOperand, aNegateComparison, aNodeCharProfileList) || currentReturn;
    //            }
    //        }
    //        // Begin TT#189 MD - JSmith - Filter Performance
    //        //catch (EndOfOperandsException)
    //        //{
    //        //    return currentReturn;
    //        //}
    //        // End TT#189 MD
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    private QueryOperand GetNextOperand(IEnumerator aEnumerator)
    //    {
    //        bool rc;

    //        try
    //        {
    //            while ((rc = aEnumerator.MoveNext()) &&
    //                (aEnumerator.Current.GetType() == typeof(AttrQuerySpacerOperand) ||
    //                aEnumerator.Current.GetType() == typeof(DataQuerySpacerOperand) ||
    //                !((QueryOperand)aEnumerator.Current).isMainOperand))
    //            {
    //            }

    //            if (rc)
    //            {
    //                return (QueryOperand)aEnumerator.Current;
    //            }
    //            else
    //            {
    //                // Begin TT#189 MD - JSmith - Filter Performance
    //                //throw new EndOfOperandsException();
    //                return null;
    //                // End TT#189 MD
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }
    //}
}
