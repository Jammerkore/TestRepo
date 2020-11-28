using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public class NodePropertiesStoreGrades : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======
        StoreGradeList _storeGradeList = null;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesStoreGrades(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.StoreGrade)
        {
           
        }

        //===========
        // PROPERTIES
        //===========



        //========
        // METHODS
        //========

        override public bool OnClosing()
        {
            return true;
        }


        override public RONodeProperties NodePropertiesGetData(ROProfileKeyParms parms, object nodePropertiesData, ref string message, bool applyOnly = false)
        {
            _storeGradeList = (StoreGradeList)nodePropertiesData;

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesStoreGrades nodeProperties = new RONodePropertiesStoreGrades(node: node);

            // populate modelProperties using Windows\NodeProperties.cs as a reference
            AddStoreGrades(nodeProperties: nodeProperties, storeGradeList: _storeGradeList, message: ref message);

            return nodeProperties;
        }

        private void AddStoreGrades(RONodePropertiesStoreGrades nodeProperties, StoreGradeList storeGradeList, ref string message)
        {
            HierarchyNodeProfile hnp = null;
            RONodePropertiesStoreGrade storeGrade;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            foreach (StoreGradeProfile sgp in storeGradeList)
            {
                if (sgp.StoreGradesIsInherited
                    && !nodeProperties.StoreGradesIsInherited)
                {
                      if (hnp == null || hnp.Key != sgp.StoreGradesInheritedFromNodeRID)
                    {
                        hnp = GetHierarchyNodeProfile( key: sgp.StoreGradesInheritedFromNodeRID);
                    }
                    nodeProperties.StoreGradesInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }
                if (sgp.MinMaxesIsInherited
                    && !nodeProperties.MinimumMaximumsIsInherited)
                {
                    if (hnp == null || hnp.Key != sgp.MinMaxesInheritedFromNodeRID)
                    {
                        hnp = GetHierarchyNodeProfile(key: sgp.MinMaxesInheritedFromNodeRID);
                    }
                    nodeProperties.MinimumMaximumsInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }

                storeGrade = new RONodePropertiesStoreGrade(
                    storeGrade: new KeyValuePair<int, string>(sgp.Boundary, sgp.StoreGrade),
                    weeksOfSupplyIndex: sgp.WosIndex);

                if (sgp.MinStock > Include.Undefined)
                {
                    storeGrade.Minimum = sgp.MinStock;
                }
    
                if (sgp.MaxStock > Include.Undefined)
                {
                    storeGrade.Maximum = sgp.MaxStock;
                }

                if (sgp.MinAd > Include.Undefined)
                {
                    storeGrade.AdMinimum = sgp.MinAd;
                }

                if (sgp.MinColor > Include.Undefined)
                {
                    storeGrade.ColorMinimum = sgp.MinColor;
                }

                if (sgp.MaxColor > Include.Undefined)
                {
                    storeGrade.ColorMaximum = sgp.MaxColor;
                }

                if (sgp.ShipUpTo > Include.Undefined)
                {
                    storeGrade.ShipUpTo = sgp.ShipUpTo;
                }

                nodeProperties.StoreGrades.Add(storeGrade);

            }
        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;

            RONodePropertiesStoreGrades nodePropertiesStoreGradesData = (RONodePropertiesStoreGrades)nodePropertiesData;

            if (_storeGradeList == null)
            {
                _storeGradeList = GetStoreGrades(key: nodePropertiesStoreGradesData.Node.Key);
            }

            if (SetStoreGrades(nodePropertiesStoreGradesData: nodePropertiesStoreGradesData, message: ref message))
            {
                if (!applyOnly)
                {
                    if (!UpdateStoreGrades(message: ref message))
                    {
                        successful = false;
                    }
                }
            }
            else
            {
                successful = false;
            }

            return _storeGradeList;
        }


        /// <summary>
        /// Takes values from input class and updates the store grade memory object
        /// </summary>
        /// <param name="nodePropertiesData">Input values for the store grade</param>
        /// <param name="message">The message</param>
        private bool SetStoreGrades(RONodePropertiesStoreGrades nodePropertiesStoreGradesData, ref string message)
        {
            bool StoreGradesChangesMade = false;
            bool MinMaxesChangesMade = false;

            int index = 0;
            // rebuild grades each time

            bool deleteMinMaxValues = true;

            // check for changes so no how to set change flags
            if (nodePropertiesStoreGradesData.StoreGrades.Count != _storeGradeList.Count)
            {
                StoreGradesChangesMade = true;
                MinMaxesChangesMade = true;
            }
            else
            {
                foreach (RONodePropertiesStoreGrade sg in nodePropertiesStoreGradesData.StoreGrades)
                {
                    StoreGradeProfile sgp = (StoreGradeProfile)_storeGradeList[index];
                    if (sg.StoreGrade.Value != sgp.StoreGrade
                        || sg.StoreGrade.Key != sgp.Boundary
                        || sg.WeeksOfSupplyIndex != sgp.WosIndex
                        )
                    {
                        StoreGradesChangesMade = true;
                    }

                    if ((!sg.MinimumIsSet && sgp.MinStock == Include.Undefined)
                        && (!sg.MaximumIsSet && sgp.MaxStock == Include.Undefined)
                        && (!sg.AdMinimumIsSet && sgp.MinAd == Include.Undefined)
                        && (!sg.ColorMinimumIsSet && sgp.MinColor == Include.Undefined)
                        && (!sg.ColorMaximumIsSet && sgp.MaxColor == Include.Undefined)
                        && (!sg.ShipUpToIsSet && sgp.ShipUpTo == Include.Undefined)
                        )
                    {
                        continue;  // nothing is set so skip
                    }
                    else if (sg.Minimum != sgp.MinStock
                        || sg.Maximum != sgp.MaxStock
                        || sg.AdMinimum != sgp.MinAd
                        || sg.ColorMinimum != sgp.MinColor
                        || sg.ColorMaximum != sgp.MaxColor
                        || sg.ShipUpTo != sgp.ShipUpTo
                        )
                    {
                        MinMaxesChangesMade = true;
                    }
                    ++index;
                }
            }

            index = 0;
            if (StoreGradesChangesMade
                || MinMaxesChangesMade)
            {
                _storeGradeList.Clear();
                foreach (RONodePropertiesStoreGrade sg in nodePropertiesStoreGradesData.StoreGrades)
                {
                    StoreGradeProfile sgp = new StoreGradeProfile(index);

                    if (StoreGradesChangesMade)
                    {
                        sgp.StoreGradeChangeType = eChangeType.add;
                        sgp.MinMaxChangeType = eChangeType.update;          // if change store grade, then update min/maxes
                    }
                    else
                        if (MinMaxesChangesMade)
                    {
                        if (nodePropertiesStoreGradesData.StoreGradesIsInherited)
                        {
                            sgp.StoreGradesIsInherited = true;
                            sgp.MinMaxChangeType = eChangeType.add; // if store grades don't exist, must add min/maxes
                        }
                        else
                        {
                            sgp.MinMaxChangeType = eChangeType.update;  // if store grades exist, then min/maxes are updated
                        }
                    }

                    sgp.StoreGrade = sg.StoreGrade.Value;
                    sgp.Boundary = sg.StoreGrade.Key;

                    //if (Convert.IsDBNull(gridRow.Cells["OriginalBoundary"].Value))
                    //{
                    sgp.OriginalBoundary = sgp.Boundary;
                    //}
                    //else
                    //{
                    //    sgp.OriginalBoundary = Convert.ToInt32(gridRow.Cells["OriginalBoundary"].Value, CultureInfo.CurrentUICulture);
                    //}

                    sgp.WosIndex = sg.WeeksOfSupplyIndex;

                    if (sg.MinimumIsSet)
                    {
                        sgp.MinStock = (int)sg.Minimum;
                        deleteMinMaxValues = false;
                    }
                    else
                    {
                        sgp.MinStock = Include.Undefined;
                    }

                    if (sg.MaximumIsSet)
                    {
                        sgp.MaxStock = (int)sg.Maximum;
                        deleteMinMaxValues = false;
                    }
                    else
                    {
                        sgp.MaxStock = Include.Undefined;
                    }

                    if (sg.AdMinimumIsSet)
                    {
                        sgp.MinAd = (int)sg.AdMinimum;
                        deleteMinMaxValues = false;
                    }
                    else
                    {
                        sgp.MinAd = Include.Undefined;
                    }

                    if (sg.ColorMinimumIsSet)
                    {
                        sgp.MinColor = (int)sg.ColorMinimum;
                        deleteMinMaxValues = false;
                    }
                    else
                    {
                        sgp.MinColor = Include.Undefined;
                    }

                    if (sg.ColorMaximumIsSet)
                    {
                        sgp.MaxColor = (int)sg.ColorMaximum;
                        deleteMinMaxValues = false;
                    }
                    else
                    {
                        sgp.MaxColor = Include.Undefined;
                    }

                    if (sg.ShipUpToIsSet)
                    {
                        sgp.ShipUpTo = (int)sg.ShipUpTo;
                        deleteMinMaxValues = false;
                    }
                    else
                    {
                        sgp.ShipUpTo = Include.Undefined;
                    }

                    _storeGradeList.Add(sgp);

                    ++index;
                }

                // if all settings are undefined, delete entry so inheritance will be established
                if (deleteMinMaxValues)
                {
                    foreach (StoreGradeProfile sgp in _storeGradeList)
                    {
                        sgp.MinMaxChangeType = eChangeType.delete;
                    }
                }

                
            }

            return true;
        }

        private bool UpdateStoreGrades(ref string message)
        {
            SAB.HierarchyServerSession.StoreGradesUpdate(HierarchyNodeProfile.Key, _storeGradeList);

            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            if (_storeGradeList == null)
            {
                _storeGradeList = GetStoreGrades(key: key);
            }

            foreach (StoreGradeProfile sgp in _storeGradeList)
            {
                sgp.StoreGradeChangeType = eChangeType.delete;
                sgp.MinMaxChangeType = eChangeType.delete;
            }

            SAB.HierarchyServerSession.StoreGradesUpdate(HierarchyNodeProfile.Key, _storeGradeList);

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deleteStoreGrades: true);
                }
                else
                {
                    message = MIDText.GetText(eMIDTextCode.lbl_ACLL_LockAttemptFailed);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            finally
            {
                SAB.HierarchyServerSession.DequeueBranch(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key);
            }

            return true;
        }

        override public object NodePropertiesGetValues(ROProfileKeyParms parms)
        {
            return GetStoreGrades(key: parms.Key);
        }

        private StoreGradeList GetStoreGrades(int key)
        {
            return SAB.HierarchyServerSession.GetStoreGradeList(nodeRID: key, forCopy: false, forAdmin: true);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyStoreGrades, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyStoreGrades, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
        }
    }
}
