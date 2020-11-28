using System;
using System.Collections;
using System.Windows.Forms;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    /// <summary>
    /// This CustomStoreFilter filters out stores based upon a custom User definition.
    /// </summary>

//    [Serializable]
//    public class BasicStoreFilter : Filter, IDisposable
//    {
//        //=======
//        // FIELDS
//        //=======

//        private SessionAddressBlock _SAB;
//        private ApplicationSessionTransaction _transaction;
//        private Session _currentSession;
//        private int _filterID;
//        private StoreFilterData _filterDL;
//        private StoreFilterDefinition _filterDef;
//        private Hashtable _filterCubeGroupHash;
//        private DateTime _nullDate = new DateTime(1, 1, 1);

//        //=============
//        // CONSTRUCTORS
//        //=============

//        public BasicStoreFilter(
//            SessionAddressBlock aSAB,
//            ApplicationSessionTransaction aTransaction,
//            Session aCurrentSession,
//            int aFilterID)
//        {
//            try
//            {
//                _SAB = aSAB;
//                _transaction = aTransaction;
//                _currentSession = aCurrentSession;
//                _filterID = aFilterID;

//                _filterDL = new StoreFilterData();
//                _filterCubeGroupHash = new Hashtable();

//                _filterDef = new StoreFilterDefinition(
//                    _SAB,
//                    _currentSession,
//                    _filterDL,
//                    null,
//                    _transaction.GetProfileList(eProfileType.Version),
//                    _transaction.GetProfileList(eProfileType.Variable),
//                    _transaction.GetProfileList(eProfileType.TimeTotalVariable),
//                    _filterID);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        //===========
//        // PROPERTIES
//        //===========

//        override public eProfileType ProfileType
//        {
//            get
//            {
//                return eProfileType.Store;
//            }
//        }

//        //========
//        // METHODS
//        //========

//        /// <summary>
//        /// Applies this Filter to a given ProfileList.
//        /// </summary>
//        /// <remarks>
//        /// Applying a Filter to a ProfileList adds Profiles that have passed the Filter to a new ProfileList.
//        /// </remarks>
//        /// <param name="aProfileList">
//        /// The ArrayList to apply the Filter to.
//        /// </param>
//        /// <returns>
//        /// An ArrayList containing the selected Profiles.
//        /// </returns>

//        override public ProfileList ApplyFilter(ProfileList aProfileList)
//        {
//            IEnumerator enumerator;
//            QueryOperand operand = null;
//            ProfileList profileList;
//            ProfileList newProfileList;
//            DialogResult diagResult;

//            try
//            {
//                if (_filterDef.DataOperandList.Count > 0)
//                {
//                    diagResult = _SAB.MessageCallback.HandleMessage(
//                        MIDText.GetTextOnly(eMIDTextCode.msg_FilterContainsData),
//                        "Filter Warning",
//                        System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk);

//                    if (diagResult != System.Windows.Forms.DialogResult.OK)
//                    {
//                        throw new FilterContainsDataException();
//                    }
//                }

//                newProfileList = aProfileList;

//                if (_filterDef.AttrOperandList.Count > 0)
//                {
//                    profileList = newProfileList;
//                    newProfileList = new ProfileList(eProfileType.Store);

//                    foreach (StoreProfile storeProf in profileList)
//                    {
//                        enumerator = _filterDef.AttrOperandList.GetEnumerator();
//                        if (ProcessAttr(enumerator, ref operand, storeProf))
//                        {
//                            newProfileList.Add(storeProf);
//                        }
//                    }
//                }

//                return newProfileList;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private bool ProcessAttr(IEnumerator aEnumerator, ref QueryOperand aOperand, StoreProfile aStoreProf)
//        {
//            bool currentReturn = false;

//            try
//            {
//                aOperand = GetNextOperand(aEnumerator);
//                // Begin TT#189 MD - JSmith - Filter Performance
//                if (aOperand == null)
//                {
//                    return currentReturn;
//                }
//                // End TT#189 MD

//                if (aOperand.GetType() == typeof(GenericQueryLeftParenOperand))
//                {
//                    currentReturn = ProcessAttr(aEnumerator, ref aOperand, aStoreProf);
//                    aOperand = GetNextOperand(aEnumerator);
//                    // Begin TT#189 MD - JSmith - Filter Performance
//                    if (aOperand == null)
//                    {
//                        return currentReturn;
//                    }
//                    // End TT#189 MD
//                }
//                else
//                {
//                    if (aOperand.GetType() == typeof(AttrQueryAttributeMainOperand))
//                    {
//                        foreach (StoreGroupLevelProfile sglp in ((AttrQueryAttributeMainOperand)aOperand).AttributeSetProfList)
//                        {
//                            if (sglp.Stores.Contains(aStoreProf.Key))
//                            {
//                                currentReturn = true;
//                            }
//                        }
//                    }
//                    else
//                    {
//                        if (((AttrQueryStoreMainOperand)aOperand).StoreProfileList.Contains(aStoreProf))
//                        {
//                            currentReturn = true;
//                        }
//                    }

//                    aOperand = GetNextOperand(aEnumerator);
//                    // Begin TT#189 MD - JSmith - Filter Performance
//                    if (aOperand == null)
//                    {
//                        return currentReturn;
//                    }
//                    // End TT#189 MD
//                }

//                if (aOperand.GetType() == typeof(GenericQueryRightParenOperand))
//                {
//                    return currentReturn;
//                }
//                else if (aOperand.GetType() == typeof(GenericQueryAndOperand))
//                {
//                    return ProcessAttr(aEnumerator, ref aOperand, aStoreProf) && currentReturn;
//                }
//                else
//                {
//                    return ProcessAttr(aEnumerator, ref aOperand, aStoreProf) || currentReturn;
//                }
//            }
//            // Begin TT#189 MD - JSmith - Filter Performance
//            //catch (EndOfOperandsException)
//            //{
//            //    return currentReturn;
//            //}
//            // End TT#189 MD
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private QueryOperand GetNextOperand(IEnumerator aEnumerator)
//        {
//            bool rc;

//            try
//            {
//                while ((rc = aEnumerator.MoveNext()) &&
//                    (aEnumerator.Current.GetType() == typeof(AttrQuerySpacerOperand) ||
//                    aEnumerator.Current.GetType() == typeof(DataQuerySpacerOperand) ||
//                    !((QueryOperand)aEnumerator.Current).isMainOperand))
//                {
//                }

//                if (rc)
//                {
//                    return (QueryOperand)aEnumerator.Current;
//                }
//                else
//                {
//                    // Begin TT#189 MD - JSmith - Filter Performance
//                    //throw new EndOfOperandsException();
//                    return null;
//                    // End TT#189 MD
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//    }
}
