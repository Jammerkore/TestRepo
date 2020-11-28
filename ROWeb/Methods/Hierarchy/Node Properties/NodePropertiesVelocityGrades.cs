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
    public class NodePropertiesVelocityGrades : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======
        private VelocityGradeList _velocityGradeList = null;
        private SellThruPctList _sellThruPctList;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesVelocityGrades(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base(SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.VelocityGrade)
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
            _velocityGradeList = (VelocityGradeList)nodePropertiesData;

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesVelocityGrades nodeProperties = new RONodePropertiesVelocityGrades(node: node);

            // populate modelProperties using Windows\NodeProperties.cs as a reference
            AddVelocityGrades(nodeProperties: nodeProperties, message: ref message);

            AddSellThruPercents(nodeProperties: nodeProperties, message: ref message);

            return nodeProperties;
        }

        private void AddVelocityGrades(RONodePropertiesVelocityGrades nodeProperties, ref string message)
        {
            HierarchyNodeProfile hnp = null;
            RONodePropertiesVelocityGrade VelocityGrade;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            foreach (VelocityGradeProfile vgp in _velocityGradeList)
            {
                if (vgp.VelocityGradeIsInherited
                    && !nodeProperties.VelocityGradesIsInherited)
                {
                      if (hnp == null || hnp.Key != vgp.VelocityGradeInheritedFromNodeRID)
                    {
                        hnp = GetHierarchyNodeProfile(key: vgp.VelocityGradeInheritedFromNodeRID);
                    }
                    nodeProperties.VelocityGradesInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }
                if (vgp.VelocityMinMaxesIsInherited
                    && !nodeProperties.MinimumMaximumsIsInherited)
                {
                    if (hnp == null || hnp.Key != vgp.VelocityMinMaxesInheritedFromNodeRID)
                    {
                        hnp = GetHierarchyNodeProfile(key: vgp.VelocityMinMaxesInheritedFromNodeRID);
                    }
                    nodeProperties.MinimumMaximumsInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }

                VelocityGrade = new RONodePropertiesVelocityGrade(
                    velocityGrade: new KeyValuePair<int, string>(vgp.Boundary, vgp.VelocityGrade));

                if (vgp.VelocityMinStock > Include.Undefined)
                {
                    VelocityGrade.Minimum = vgp.VelocityMinStock;
                }
    
                if (vgp.VelocityMaxStock > Include.Undefined)
                {
                    VelocityGrade.Maximum = vgp.VelocityMaxStock;
                }

                if (vgp.VelocityMinAd > Include.Undefined)
                {
                    VelocityGrade.AdMinimum = vgp.VelocityMinAd;
                }

                nodeProperties.VelocityGrades.Add(VelocityGrade);

            }
        }

        private void AddSellThruPercents(RONodePropertiesVelocityGrades nodeProperties, ref string message)
        {
            HierarchyNodeProfile hnp = null;
            ROSellThruList sellThru;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            if (_sellThruPctList == null)
            {
                _sellThruPctList = GetSellThruPercents(key: nodeProperties.Node.Key);
            }

            foreach (SellThruPctProfile stpp in _sellThruPctList)
            {
                if (stpp.SellThruPctIsInherited
                    && !nodeProperties.SellThruPercentsIsInherited)
                {
                    if (hnp == null || hnp.Key != stpp.SellThruPctInheritedFromNodeRID)
                    {
                        hnp = GetHierarchyNodeProfile(key: stpp.SellThruPctInheritedFromNodeRID);
                    }
                    nodeProperties.SellThruPercentsInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }
                sellThru = new ROSellThruList(sellThru: stpp.SellThruPct);

                nodeProperties.SellThruPercents.Add(sellThru);
            }
        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;

            RONodePropertiesVelocityGrades nodePropertiesVelocityGradesData = (RONodePropertiesVelocityGrades)nodePropertiesData;

            if (_velocityGradeList == null)
            {
                _velocityGradeList = GetVelocityGrades(key: nodePropertiesVelocityGradesData.Node.Key);
            }

            if (_sellThruPctList == null)
            {
                _sellThruPctList = GetSellThruPercents(key: nodePropertiesVelocityGradesData.Node.Key);
            }

            if (SetVelocityGrades(nodePropertiesVelocityGradesData: nodePropertiesVelocityGradesData, message: ref message))
            {
                if (SetSellThruPercents(nodePropertiesVelocityGradesData: nodePropertiesVelocityGradesData, message: ref message))
                {
                    if (!applyOnly)
                    {
                        if (!UpdateVelocityGrades(message: ref message))
                        {
                            successful = false;
                        }
                        else if (!UpdateSellThruPercents(message: ref message))
                        {
                            successful = false;
                        }
                    }
                }
                else
                {
                    successful = false;
                }
            }
            else
            {
                successful = false;
            }

            return _velocityGradeList;
        }


        /// <summary>
        /// Takes values from input class and updates the velocity grade memory object
        /// </summary>
        /// <param name="nodePropertiesData">Input values for the velocity grade</param>
        /// <param name="message">The message</param>
        private bool SetVelocityGrades(RONodePropertiesVelocityGrades nodePropertiesVelocityGradesData, ref string message)
        {
            bool VelocityGradesChangesMade = false;
            bool MinMaxesChangesMade = false;

            int index = 0;
            // rebuild grades each time

            bool deleteMinMaxValues = true;

            // check for changes so no how to set change flags
            if (nodePropertiesVelocityGradesData.VelocityGrades.Count != _velocityGradeList.Count)
            {
                VelocityGradesChangesMade = true;
                MinMaxesChangesMade = true;
            }
            else
            {
                foreach (RONodePropertiesVelocityGrade vg in nodePropertiesVelocityGradesData.VelocityGrades)
                {
                    VelocityGradeProfile vgp = (VelocityGradeProfile)_velocityGradeList[index];
                    if (vg.VelocityGrade.Value != vgp.VelocityGrade
                        || vg.VelocityGrade.Key != vgp.Boundary
                        )
                    {
                        VelocityGradesChangesMade = true;
                    }

                    if ((!vg.MinimumIsSet && vgp.VelocityMinStock == Include.Undefined)
                        && (!vg.MaximumIsSet && vgp.VelocityMaxStock == Include.Undefined)
                        && (!vg.AdMinimumIsSet && vgp.VelocityMinAd == Include.Undefined)
                        )
                    {
                        continue;  // nothing is set so skip
                    }
                    else if (vg.Minimum != vgp.VelocityMinStock
                        || vg.Maximum != vgp.VelocityMaxStock
                        || vg.AdMinimum != vgp.VelocityMinAd
                        )
                    {
                        MinMaxesChangesMade = true;
                    }
                    ++index;
                }
            }

            index = 0;
            if (VelocityGradesChangesMade
                || MinMaxesChangesMade)
            {
                _velocityGradeList.Clear();
                foreach (RONodePropertiesVelocityGrade sg in nodePropertiesVelocityGradesData.VelocityGrades)
                {
                    VelocityGradeProfile vgp = new VelocityGradeProfile(index);

                    if (VelocityGradesChangesMade)
                    {
                        vgp.VelocityGradeChangeType = eChangeType.add;
                        vgp.VelocityMinMaxChangeType = eChangeType.update;          // if change velocity grade, then update min/maxes
                    }
                    else
                        if (MinMaxesChangesMade)
                    {
                        if (nodePropertiesVelocityGradesData.VelocityGradesIsInherited)
                        {
                            vgp.VelocityGradeIsInherited = true;
                            vgp.VelocityMinMaxChangeType = eChangeType.add; // if velocity grades don't exist, must add min/maxes
                        }
                        else
                        {
                            vgp.VelocityMinMaxChangeType = eChangeType.update;  // if velocity grades exist, then min/maxes are updated
                        }
                    }

                    vgp.VelocityGrade = sg.VelocityGrade.Value;
                    vgp.Boundary = sg.VelocityGrade.Key;

                    if (sg.MinimumIsSet)
                    {
                        vgp.VelocityMinStock = (int)sg.Minimum;
                        deleteMinMaxValues = false;
                    }
                    else
                    {
                        vgp.VelocityMinStock = Include.Undefined;
                    }

                    if (sg.MaximumIsSet)
                    {
                        vgp.VelocityMaxStock = (int)sg.Maximum;
                        deleteMinMaxValues = false;
                    }
                    else
                    {
                        vgp.VelocityMaxStock = Include.Undefined;
                    }

                    if (sg.AdMinimumIsSet)
                    {
                        vgp.VelocityMinAd = (int)sg.AdMinimum;
                        deleteMinMaxValues = false;
                    }
                    else
                    {
                        vgp.VelocityMinAd = Include.Undefined;
                    }

                    _velocityGradeList.Add(vgp);

                    ++index;
                }

                // if all settings are undefined, delete entry so inheritance will be established
                if (deleteMinMaxValues)
                {
                    foreach (VelocityGradeProfile vgp in _velocityGradeList)
                    {
                        vgp.VelocityMinMaxChangeType = eChangeType.delete;
                    }
                }

                
            }

            return true;
        }

        /// <summary>
        /// Takes values from input class and updates the sell thru percents memory object
        /// </summary>
        /// <param name="nodePropertiesData">Input values for the sell thru percents</param>
        /// <param name="message">The message</param>
        private bool SetSellThruPercents(RONodePropertiesVelocityGrades nodePropertiesVelocityGradesData, ref string message)
        {
            bool SellThruPctsChangesMade = false;

            int index = 0;

            // check for changes so no how to set change flags
            if (nodePropertiesVelocityGradesData.SellThruPercents.Count != _sellThruPctList.Count)
            {
                SellThruPctsChangesMade = true;
            }
            else
            {
                foreach (ROSellThruList sellThru in nodePropertiesVelocityGradesData.SellThruPercents)
                {
                    SellThruPctProfile stpp = (SellThruPctProfile)_sellThruPctList[index];
                    if (sellThru.Sell_Thru != stpp.SellThruPct
                        )
                    {
                        SellThruPctsChangesMade = true;
                    }
                    ++index;
                }
            }

            index = 0;
            if (SellThruPctsChangesMade)
            {
                _sellThruPctList.Clear();
                foreach (ROSellThruList sellThru in nodePropertiesVelocityGradesData.SellThruPercents)
                {
                    SellThruPctProfile stpp = new SellThruPctProfile(index);

                    if (SellThruPctsChangesMade)
                    {
                        stpp.SellThruPctChangeType = eChangeType.add;
                    }

                    stpp.SellThruPct = sellThru.Sell_Thru;

                    _sellThruPctList.Add(stpp);

                    ++index;
                }
            }

            return true;
        }

        private bool UpdateVelocityGrades(ref string message)
        {
            SAB.HierarchyServerSession.VelocityGradesUpdate(HierarchyNodeProfile.Key, _velocityGradeList);

            return true;
        }

        private bool UpdateSellThruPercents(ref string message)
        {
            SAB.HierarchyServerSession.SellThruPctsUpdate(HierarchyNodeProfile.Key, _sellThruPctList);

            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            if (_velocityGradeList == null)
            {
                _velocityGradeList = GetVelocityGrades(key: key);
            }

            foreach (VelocityGradeProfile vgp in _velocityGradeList)
            {
                vgp.VelocityGradeChangeType = eChangeType.delete;
                vgp.VelocityMinMaxChangeType = eChangeType.delete;
            }

            SAB.HierarchyServerSession.VelocityGradesUpdate(HierarchyNodeProfile.Key, _velocityGradeList);

            if (_sellThruPctList == null)
            {
                _sellThruPctList = GetSellThruPercents(key: key);
            }

            foreach (SellThruPctProfile stpp in _sellThruPctList)
            {
                stpp.SellThruPctChangeType = eChangeType.delete;
            }

            SAB.HierarchyServerSession.SellThruPctsUpdate(HierarchyNodeProfile.Key, _sellThruPctList);

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deleteVelocityGrades: true, deleteSellThruPcts: true);
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
            return GetVelocityGrades(key: parms.Key);
        }

        private VelocityGradeList GetVelocityGrades(int key)
        {
            return SAB.HierarchyServerSession.GetVelocityGradeList(nodeRID: key, forCopy: false, forAdmin: true);
        }

        private SellThruPctList GetSellThruPercents(int key)
        {
            return SAB.HierarchyServerSession.GetSellThruPctList(nodeRID: key, forCopy: false);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyVelocity, (int)eSecurityTypes.Allocation);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyVelocity, (int)eSecurityTypes.Allocation);
            }

        }
    }
}
