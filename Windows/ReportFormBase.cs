using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public class ReportFormBase : MIDRetail.Windows.MIDFormBase
	{
		#region Member Vars
		private int _nodeRID = Include.NoRID;
        private int _planLevelRID = Include.NoRID;  // TT#350 - RMatelic - Report does not show any allocations  
		private bool _textChanged = false;
		private bool _priorError = false;
        public Hashtable _styleColorSizeHash;  // Begin TT#397 - RMatelic - Allocation Audit Report not selecting headers when drag/drop a style/ color node
        private SessionAddressBlock _SAB;      // End TT#397  
		#endregion

		#region Properties
		public int NodeRID
		{
			get { return _nodeRID; }
			set { _nodeRID = value; }
		}
        public int PlanLevelRID                     // Begin TT#350 - RMatelic - Report does not show any allocations  
        {
            get { return _planLevelRID; }
            set { _planLevelRID = value; }
        }                                           // End TT#350
        #endregion

		#region Constructors and Initialization Code
		private System.ComponentModel.IContainer components = null;

        public ReportFormBase(SessionAddressBlock aSAB)
            : base(aSAB)
		{
			InitializeComponent();
            _SAB = aSAB;
		}

		public ReportFormBase() : base()
		{
			InitializeComponent();
		}

		protected override void Dispose( bool disposing )
		{
            if (disposing)
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
		#endregion

		#region Event Handlers
		protected void txtBaseMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList;
			try
			{
				TextBox txtMerchandise = (TextBox)sender;
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    // Begin TT#397 -  RMatelic - Unrelated  - show fully qualified node text
                    //txtMerchandise.Text = cbList.ClipboardProfile.Text;
                    //NodeRID = cbList.ClipboardProfile.Key;
                    // Begin TT#350 - RMatelic - Report does not show any allocations 
                    //HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(NodeRID);
                    //txtMerchandise.Tag = hnp;
                    // End TT#350 
                    NodeRID = cbList.ClipboardProfile.Key;
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(NodeRID, true, true);
                    txtMerchandise.Text = hnp.Text;
                    txtMerchandise.Tag = hnp;
                    // End TT#397
                    ErrorProvider.SetError(txtMerchandise, string.Empty);
                }
			}
			catch (BadDataInClipboardException)
			{
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        protected void txtBaseMerchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            Merchandise_DragEnter(sender, e);
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

        protected void txtBaseMerchandise_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string errorMessage;

			try
			{
                TextBox txtBaseMerchandise = (TextBox)sender;
                if (txtBaseMerchandise.Text.Trim() == string.Empty && txtBaseMerchandise.Tag != null)
				{
                    txtBaseMerchandise.Text = string.Empty;
                    txtBaseMerchandise.Tag = null;
					NodeRID = Include.NoRID;
				}
				else
				{
					if (_textChanged)
					{
						_textChanged = false;

                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(txtBaseMerchandise.Text, false);
						if (hnp.Key == Include.NoRID)
						{
							_priorError = true;

                            errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), txtBaseMerchandise.Text);
                            ErrorProvider.SetError(txtBaseMerchandise, errorMessage);
							MessageBox.Show(errorMessage);

							e.Cancel = true;
						}
						else 
						{
                            txtBaseMerchandise.Text = hnp.Text;
                            txtBaseMerchandise.Tag = hnp;
							NodeRID = hnp.Key;
						}	
					}
					else if (_priorError)
					{
                        if (txtBaseMerchandise.Tag == null)
						{
                            txtBaseMerchandise.Text = string.Empty;
						}
						else
						{
                            txtBaseMerchandise.Text = ((HierarchyNodeProfile)txtBaseMerchandise.Tag).Text;
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        protected void txtBaseMerchandise_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
            TextBox txtBaseMerchandise = (TextBox)sender;
			_textChanged = true;
            ErrorProvider.SetError(txtBaseMerchandise, string.Empty);
		}

        protected void txtBaseMerchandise_Validated(object sender, System.EventArgs e)
		{
			try
			{
                TextBox txtBaseMerchandise = (TextBox)sender;
				_textChanged = false;
				_priorError = false;
                ErrorProvider.SetError(txtBaseMerchandise, string.Empty);
			}
			catch (Exception)
			{
				throw;
			}
		}

        // Begin TT#350 - RMatelic - Report does not show any allocations  
        protected void txtBasePlanLevel_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNodeClipboardList cbList;
            try
            {
                TextBox txtBasePlanLevel = (TextBox)sender;
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //txtBasePlanLevel.Text = cbList.ClipboardProfile.Text; // TT#397 -  RMatelic - Unrelated  - show fully qualified node text; see below
                    PlanLevelRID = cbList.ClipboardProfile.Key;
                    // Begin TT#397 -  RMatelic - Unrelated  - show fully qualified node text
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(PlanLevelRID, true, true);
                    txtBasePlanLevel.Text = hnp.Text;
                    txtBasePlanLevel.Tag = hnp;
                    // End TT#397
                    ErrorProvider.SetError(txtBasePlanLevel, string.Empty);
                }
            }
            catch (BadDataInClipboardException)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected void txtBasePlanLevel_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Merchandise_DragEnter(sender, e);
        }

        protected void txtBasePlanLevel_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string errorMessage;

            try
            {
                TextBox txtBasePlanLevel = (TextBox)sender;
                if (txtBasePlanLevel.Text.Trim() == string.Empty && txtBasePlanLevel.Tag != null)
                {
                    txtBasePlanLevel.Text = string.Empty;
                    txtBasePlanLevel.Tag = null;
                    PlanLevelRID = Include.NoRID;
                }
                else
                {
                    if (_textChanged)
                    {
                        _textChanged = false;

                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(txtBasePlanLevel.Text, false);
                        if (hnp.Key == Include.NoRID)
                        {
                            _priorError = true;

                            errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), txtBasePlanLevel.Text);
                            ErrorProvider.SetError(txtBasePlanLevel, errorMessage);
                            MessageBox.Show(errorMessage);

                            e.Cancel = true;
                        }
                        else
                        {
                            txtBasePlanLevel.Text = hnp.Text;
                            txtBasePlanLevel.Tag = hnp;
                            PlanLevelRID = hnp.Key;
                        }
                    }
                    else if (_priorError)
                    {
                        if (txtBasePlanLevel.Tag == null)
                        {
                            txtBasePlanLevel.Text = string.Empty;
                        }
                        else
                        {
                            txtBasePlanLevel.Text = ((HierarchyNodeProfile)txtBasePlanLevel.Tag).Text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected void txtBasePlanLevel_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            TextBox txtBasePlanLevel = (TextBox)sender;
            _textChanged = true;
            ErrorProvider.SetError(txtBasePlanLevel, string.Empty);
        }

        protected void txtBasePlanLevel_Validated(object sender, System.EventArgs e)
        {
            try
            {
                TextBox txtBasePlanLevel = (TextBox)sender;
                _textChanged = false;
                _priorError = false;
                ErrorProvider.SetError(txtBasePlanLevel, string.Empty);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // End TT#350  

		#endregion

		#region Private Methods
        // Begin TT#397 - RMatelic - Allocation Audit Report not selecting headers when drag/drop a style/ color node
        public string GetHeaderRIDList()
        {
            HierarchyNodeList hnl = new HierarchyNodeList(eProfileType.HierarchyNode);
            Header headerDataRecord = new Header();
            AllocationHeaderProfile headerProfile;
            DataTable headerDataTable;
            string headerRIDTextList = string.Empty;
            try
            {
                if (this.NodeRID > 0)
                {
                    _styleColorSizeHash = new Hashtable();
                    HierarchyNodeProfile np = _SAB.HierarchyServerSession.GetNodeData(this.NodeRID);
                    if (np.HomeHierarchyType == eHierarchyType.alternate || np.LevelType == eHierarchyLevelType.Color || np.LevelType == eHierarchyLevelType.Size)
                    {
                        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions 
                        //Not sure why this is being called...
                        //_SAB.HeaderServerSession.GetHeadersForUser(Include.AdministratorUserRID);
                        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions 

                        ProcessHierarchyNode(np, ref hnl);
                        foreach (HierarchyNodeProfile hnp in hnl)
                        {
                            headerDataTable = headerDataRecord.GetHeaders(hnp.Key);

                            foreach (DataRow dr in headerDataTable.Rows)
                            {
                                headerProfile = _SAB.HeaderServerSession.GetHeaderData(Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture), aIncludeComponents: true, aIncludeCharacteristics: true, blForceGet: false);
                                if (ProcessHeader(headerProfile))
                                {
                                    headerRIDTextList += headerProfile.Key.ToString() + ",";
                                }
                            }
                        }

                        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions 
                        //Do not think this is now necessary if we do not call GetHeadersForUser above
						//Begin TT#1022 - JScott - Allocation workspace gets severe error after Allocation Audit report is processed (can repeat)
						//Reset Header List to current User's list
						//_SAB.HeaderServerSession.GetHeadersForUser(_SAB.ClientServerSession.UserRID);
						//End TT#1022 - JScott - Allocation workspace gets severe error after Allocation Audit report is processed (can repeat)
                        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
					}
                }
            }
            catch (Exception)
            {
                throw;
            }
            return headerRIDTextList;
        }
    
        private void ProcessHierarchyNode(HierarchyNodeProfile aNodeProfile, ref HierarchyNodeList aHierNodeList)
        {
            try
            {
                if (aNodeProfile.HomeHierarchyType == eHierarchyType.alternate)
                {
                    // Get next level nodes
                    HierarchyNodeList hnl = _SAB.HierarchyServerSession.GetDescendantData(aNodeProfile.Key, 1, false, eNodeSelectType.All);
                    foreach (HierarchyNodeProfile hnp in hnl)
                    {
                        ProcessHierarchyNode(hnp, ref aHierNodeList);
                    }
                }
                else
                {
                    // _styleColorSizeHash contains only styles where the color node or size node was specifically indicated 
                    NodeAncestorList nal;
                    Hashtable htColor;
                    ArrayList alSize;
                    int colorRID;
                    switch (aNodeProfile.LevelType)
                    {
                        case eHierarchyLevelType.Undefined:
                            HierarchyNodeList hnl = _SAB.HierarchyServerSession.GetDescendantData(aNodeProfile.Key, eHierarchyLevelMasterType.Style, false, eNodeSelectType.All);
                            foreach (HierarchyNodeProfile np in hnl)
                            {
                                if (!aHierNodeList.Contains(np.Key))
                                {
                                    aHierNodeList.Add(np);
                                }
                            }
                            break;

                        case eHierarchyLevelType.Style:
                            if (!aHierNodeList.Contains(aNodeProfile.Key))
                            {
                                aHierNodeList.Add(aNodeProfile);
                            }
                            break;

                        case eHierarchyLevelType.Color:
                            colorRID = aNodeProfile.ColorOrSizeCodeRID;
                            if (colorRID > Include.DummyColorRID)
                            {
                                nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aNodeProfile.Key, aNodeProfile.HomeHierarchyRID, eHierarchySearchType.HomeHierarchyOnly);
                                foreach (NodeAncestorProfile nap in nal)
                                {
                                    HierarchyNodeProfile np = _SAB.HierarchyServerSession.GetNodeData(nap.Key);
                                    if (np.LevelType == eHierarchyLevelType.Style)
                                    {
                                        if (!aHierNodeList.Contains(np.Key))
                                        {
                                            aHierNodeList.Add(np);
                                        }
                                        if (_styleColorSizeHash.ContainsKey(np.Key))
                                        {
                                            htColor = (Hashtable)_styleColorSizeHash[np.Key];
                                            if (!htColor.ContainsKey(colorRID))
                                            {
                                                alSize = new ArrayList();
                                                htColor.Add(colorRID, alSize);
                                            }
                                        }
                                        else
                                        {
                                            htColor = new Hashtable();
                                            alSize = new ArrayList();
                                            htColor.Add(colorRID, alSize);
                                            _styleColorSizeHash.Add(np.Key, htColor);
                                        }
                                        break;
                                    }
                                }
                            }
                            break;

                        case eHierarchyLevelType.Size:
                            colorRID = Include.DummyColorRID;
                            int sizeRID = aNodeProfile.ColorOrSizeCodeRID;
                            nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aNodeProfile.Key, aNodeProfile.HomeHierarchyRID, eHierarchySearchType.HomeHierarchyOnly);
                            foreach (NodeAncestorProfile nap in nal)
                            {
                                HierarchyNodeProfile np = _SAB.HierarchyServerSession.GetNodeData(nap.Key);
                                if (np.LevelType == eHierarchyLevelType.Color)
                                {
                                    colorRID = np.ColorOrSizeCodeRID;
                                }
                                else if (np.LevelType == eHierarchyLevelType.Style)
                                {
                                    if (!aHierNodeList.Contains(np.Key))
                                    {
                                        aHierNodeList.Add(np);
                                    }
                                    if (_styleColorSizeHash.ContainsKey(np.Key))
                                    {
                                        htColor = (Hashtable)_styleColorSizeHash[np.Key];
                                        if (colorRID > Include.DummyColorRID)
                                        {
                                            if (htColor.ContainsKey(colorRID))
                                            {
                                                alSize = (ArrayList)htColor[colorRID];
                                                if (!alSize.Contains(sizeRID))
                                                {
                                                    alSize.Add(sizeRID);
                                                }
                                            }
                                            else
                                            {
                                                alSize = new ArrayList();
                                                alSize.Add(sizeRID);
                                                htColor = new Hashtable();
                                                htColor.Add(colorRID, alSize);
                                            }
                                        }
                                    }
                                    else if (colorRID > Include.DummyColorRID)
                                    {
                                        alSize = new ArrayList();
                                        alSize.Add(sizeRID);
                                        htColor = new Hashtable();
                                        htColor.Add(colorRID, alSize);
                                        _styleColorSizeHash.Add(np.Key, htColor);
                                    }
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private bool ProcessHeader(AllocationHeaderProfile aHdrProfile)
        {
            // if hash table does not contain style, process header; otherwise see if header contains the colors/sizes in hash table 
            bool processHeader = true;
            try
            {
                if (_styleColorSizeHash.ContainsKey(aHdrProfile.StyleHnRID))
                {
                    if (aHdrProfile.BulkColors.Count == 0)
                    {
                        if (aHdrProfile.Packs.Count == 0)
                        {
                            processHeader = false;
                        }
                        else
                        {
                            processHeader = ProcessHeaderPackColor(aHdrProfile);
                        }
                    }
                    else
                    {
                        if (ProcessHeaderBulkColor(aHdrProfile))
                        {
                            if (aHdrProfile.Packs.Count > 0)
                            {
                                processHeader = ProcessHeaderPackColor(aHdrProfile);
                            }
                        }
                        else
                        {
                            processHeader = false;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return processHeader;
        }

        private bool ProcessHeaderBulkColor(AllocationHeaderProfile aHdrProfile)
        {
            bool processHeader = true;
            try
            {
                Hashtable htColor = (Hashtable)_styleColorSizeHash[aHdrProfile.StyleHnRID];
                foreach (HeaderBulkColorProfile hbcp in aHdrProfile.BulkColors.Values)
                {
                    if (hbcp.Key > Include.DummyColorRID)
                    {
                        if (htColor.ContainsKey(hbcp.Key))
                        {
                            ArrayList alSize = (ArrayList)htColor[hbcp.Key];
                            if (alSize.Count > 0)
                            {
                                // check that all sizes in header color are in array list
                                foreach (HeaderBulkColorSizeProfile hbcsp in hbcp.Sizes.Values)
                                {
                                    if (!alSize.Contains(hbcsp.Key))
                                    {
                                        processHeader = false;
                                        break;
                                    }
                                }
                                if (!processHeader)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            processHeader = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return processHeader;
        }

        private bool ProcessHeaderPackColor(AllocationHeaderProfile aHdrProfile)
        {
            bool processHeader = true;
            try
            {
                bool packColorFoundInHT = false;
                Hashtable htColor = (Hashtable)_styleColorSizeHash[aHdrProfile.StyleHnRID];
                foreach (HeaderPackProfile hpp in aHdrProfile.Packs.Values)
                {
                    if (hpp.Colors != null && hpp.Colors.Count > 0)
                    {
                        foreach (HeaderPackColorProfile hpcp in hpp.Colors.Values)
                        {
                            if (hpcp.Key > Include.DummyColorRID)
                            {
                                if (htColor.ContainsKey(hpcp.Key))
                                {
                                    packColorFoundInHT = true;
                                    ArrayList alSize = (ArrayList)htColor[hpcp.Key];
                                    if (alSize.Count > 0)
                                    {
                                        // check that all sizes in pack color are in array list
                                        if (hpcp.Sizes != null && hpcp.Sizes.Count > 0)
                                        {
                                            foreach (HeaderPackColorSizeProfile hpcsp in hpcp.Sizes.Values)
                                            {
                                                if (!alSize.Contains(hpcsp.Key))
                                                {
                                                    processHeader = false;
                                                    break;
                                                }
                                            }
                                            if (!processHeader)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    processHeader = false;
                                    break;
                                }
                            }
                        }
                        if (!processHeader)
                        {
                            break;
                        }
                    }
                }
                if (processHeader)		// either all pack colors/sizes were found or no pack colors/sizes were found
                {
                    if (!packColorFoundInHT && (aHdrProfile.BulkColors.Count == 0))  // if no pack colors/sizes were found and no bulk colors, do not process header	
                    {
                        processHeader = false;
                    }
                }
            }
            catch
            {
                throw;
            }
            return processHeader;
        }
        // End TT#397 
		#endregion
	}
}

