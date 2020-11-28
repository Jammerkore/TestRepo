using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows.Controls
{
    public partial class SelectHierarchyLowLevelControl : UserControl
    {
        public SelectHierarchyLowLevelControl()
        {
            InitializeComponent();
        }

        private int _currentLowLevelNode = Include.NoRID;
        private int _longestHighestGuest = Include.NoRID;
        private int _longestBranch = Include.NoRID;
        public void LoadData(SessionAddressBlock SAB, int NodeRID)
        {
            HierarchyProfile hierProf;
            cboLowLevels.Items.Clear();

            if (NodeRID == Include.NoRID) return;
            HierarchyNodeProfile aHierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(NodeRID, false);

            if (aHierarchyNodeProfile != null)
            {
                cboLowLevels.Enabled = true;

                hierProf = SAB.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HierarchyRID);
                if (hierProf.HierarchyType == eHierarchyType.organizational)
                {
                    for (int i = aHierarchyNodeProfile.HomeHierarchyLevel + 1; i <= hierProf.HierarchyLevels.Count; i++)
                    {
                        HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
                        cboLowLevels.Items.Add(
                            new LowLevelCombo(eLowLevelsType.HierarchyLevel,
                            //Begin Track #5866 - JScott - Matrix Balance does not work
                            //0,
                            i - aHierarchyNodeProfile.HomeHierarchyLevel,
                            //End Track #5866 - JScott - Matrix Balance does not work
                            hlp.Key,
                            hlp.LevelID));
                    }
                }
                else
                {
                    HierarchyProfile mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();

                    if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
                    {
                        _longestHighestGuest = SAB.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);
                    }
                    int highestGuestLevel = _longestHighestGuest;

                    // add guest levels to comboBox
                    if ((highestGuestLevel != int.MaxValue) && (aHierarchyNodeProfile.HomeHierarchyType != eHierarchyType.alternate)) // TT#55 - KJohnson - Override Level option needs to reflect Low level already selected(in all review screens and methods with override level option)
                    {
                        for (int i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
                        {
                            if (i == 0)
                            {
                                cboLowLevels.Items.Add(
                                    new LowLevelCombo(eLowLevelsType.HierarchyLevel,
                                    0,
                                    0,
                                    "Root"));
                            }
                            else
                            {
                                HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];
                                cboLowLevels.Items.Add(
                                    new LowLevelCombo(eLowLevelsType.HierarchyLevel,
                                    //Begin Track #5866 - JScott - Matrix Balance does not work
                                    //0,
                                    i,
                                    //End Track #5866 - JScott - Matrix Balance does not work
                                    hlp.Key,
                                    hlp.LevelID));
                            }
                        }
                    }

                    // add offsets to comboBox
                    if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
                    {
                        _longestBranch = SAB.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                    }
                    int longestBranchCount = _longestBranch;
                    int offset = 0;
                    for (int i = 0; i < longestBranchCount; i++)
                    {
                        ++offset;
                        // Begin TT#1040 - JSmith - parameter not compatible error
                        //cboLowLevels.Items.Add(
                        //    new LowLevelCombo(eLowLevelsType.LevelOffset,
                        //    offset,
                        //    0,
                        //    null));
                        cboLowLevels.Items.Add(
                            new LowLevelCombo(eLowLevelsType.LevelOffset,
                            offset,
                            0,
                            "+" + offset));
                        // End TT#1040
                    }
                }
                if (cboLowLevels.Items.Count > 0)
                {
                    cboLowLevels.SelectedIndex = 0;
                }

                _currentLowLevelNode = aHierarchyNodeProfile.Key;
            }
        }


        public int GetLowLevelSequence()
        {
            int lowLevelNo = -1;
       
            if (cboLowLevels.SelectedIndex >= 0)
            {
                LowLevelCombo lowLevelComb = (LowLevelCombo)cboLowLevels.SelectedItem;
                lowLevelNo = lowLevelComb.LowLevelSequence;
               
            }
            return lowLevelNo;
        }

    }
}
