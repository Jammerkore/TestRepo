using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using System.IO;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections.Generic;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.ReBuildIntransit{
	/// <summary>
	/// Rebuild Intransit Process Utility
	/// </summary>
	public class ReBuildIntransitProcess : System.Windows.Forms.Form
	{
		#region Fields
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Button btnProcess;
		private System.Windows.Forms.Button btnClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private string _auditMessage;
		private Queue _messageQueue;
		private Queue _summaryQueue;
		private char[] _separators;

		private SessionAddressBlock _SAB;
		private Audit _audit;
        //private StoreServerSession _sss; //TT#1517-MD -jsobek -Store Service Optimization
		private int _headersChargedToIntransitCount;
		private System.Windows.Forms.TabPage tabHierarchy;
		private System.Windows.Forms.CheckBox cbxAutoReleaseHeaderResources;
		private System.Windows.Forms.TabPage tabSummary;
		private System.Windows.Forms.ListBox lstSummary;
		private System.Windows.Forms.TabPage tabMessages;
		private System.Windows.Forms.ListBox lstMessages;
		private System.Windows.Forms.ListBox lstHierarchy;
        private MIDRetail.Windows.Controls.MIDComboBoxEnh cbxHierarchyLevel;
		private int _headersCompletelyShippedCount;
		private Hashtable _hierarchyLevelHash;
		private HierarchyNodeList _levelHierarchyNodeList;
        private int[] _sortedHierarchyNodePosition;
		#endregion Fields

		#region Constructor
		public ReBuildIntransitProcess(SessionAddressBlock SAB, ref bool errorFound)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			_separators = new char[System.Environment.NewLine.Length];
			for (int i=0; i<System.Environment.NewLine.Length; i++)
			{
				_separators[i] = System.Environment.NewLine[i];
			}

			try
			{
				_SAB = SAB;
                //_sss = _SAB.StoreServerSession; //TT#1517-MD -jsobek -Store Service Optimization
				_audit = _SAB.ClientServerSession.Audit;
				_messageQueue = new Queue();
				_summaryQueue = new Queue();
				_hierarchyLevelHash = new Hashtable();
				_levelHierarchyNodeList = null;
			}
			catch (Exception Ex)
			{
				errorFound = true;
				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.Message, this.GetType().Name);
			}
		}
		#endregion Constructor

		#region Dispose
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion Dispose

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabHierarchy = new System.Windows.Forms.TabPage();
            this.cbxHierarchyLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lstHierarchy = new System.Windows.Forms.ListBox();
            this.cbxAutoReleaseHeaderResources = new System.Windows.Forms.CheckBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.tabSummary = new System.Windows.Forms.TabPage();
            this.lstSummary = new System.Windows.Forms.ListBox();
            this.tabMessages = new System.Windows.Forms.TabPage();
            this.lstMessages = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabHierarchy.SuspendLayout();
            this.tabSummary.SuspendLayout();
            this.tabMessages.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(208, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(256, 24);
            this.label5.TabIndex = 16;
            this.label5.Text = "MID Retail";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(208, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(256, 24);
            this.label6.TabIndex = 17;
            this.label6.Text = "Rebuild Intransit Utility";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabHierarchy);
            this.tabControl1.Controls.Add(this.tabSummary);
            this.tabControl1.Controls.Add(this.tabMessages);
            this.tabControl1.Location = new System.Drawing.Point(32, 87);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(656, 321);
            this.tabControl1.TabIndex = 18;
            // 
            // tabHierarchy
            // 
            this.tabHierarchy.Controls.Add(this.cbxHierarchyLevel);
            this.tabHierarchy.Controls.Add(this.lstHierarchy);
            this.tabHierarchy.Controls.Add(this.cbxAutoReleaseHeaderResources);
            this.tabHierarchy.Controls.Add(this.btnProcess);
            this.tabHierarchy.Location = new System.Drawing.Point(4, 22);
            this.tabHierarchy.Name = "tabHierarchy";
            this.tabHierarchy.Size = new System.Drawing.Size(648, 295);
            this.tabHierarchy.TabIndex = 0;
            this.tabHierarchy.Text = "Hierarchy";
            // 
            // cbxHierarchyLevel
            // 
            this.cbxHierarchyLevel.Location = new System.Drawing.Point(16, 24);
            this.cbxHierarchyLevel.Name = "cbxHierarchyLevel";
            this.cbxHierarchyLevel.Size = new System.Drawing.Size(144, 21);
            this.cbxHierarchyLevel.TabIndex = 5;
            this.cbxHierarchyLevel.SelectedIndexChanged += new System.EventHandler(this.cbxHierarchyLevel_SelectedIndexChanged);
            // 
            // lstHierarchy
            // 
            this.lstHierarchy.Location = new System.Drawing.Point(168, 24);
            this.lstHierarchy.Name = "lstHierarchy";
            this.lstHierarchy.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstHierarchy.Size = new System.Drawing.Size(464, 199);
            this.lstHierarchy.TabIndex = 4;
            // 
            // cbxAutoReleaseHeaderResources
            // 
            this.cbxAutoReleaseHeaderResources.Location = new System.Drawing.Point(168, 240);
            this.cbxAutoReleaseHeaderResources.Name = "cbxAutoReleaseHeaderResources";
            this.cbxAutoReleaseHeaderResources.Size = new System.Drawing.Size(192, 24);
            this.cbxAutoReleaseHeaderResources.TabIndex = 3;
            this.cbxAutoReleaseHeaderResources.Text = "Auto Release Header Resources";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(568, 264);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 2;
            this.btnProcess.Text = "Process";
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // tabSummary
            // 
            this.tabSummary.Controls.Add(this.lstSummary);
            this.tabSummary.Location = new System.Drawing.Point(4, 22);
            this.tabSummary.Name = "tabSummary";
            this.tabSummary.Size = new System.Drawing.Size(648, 295);
            this.tabSummary.TabIndex = 1;
            this.tabSummary.Text = "Summary";
            this.tabSummary.Visible = false;
            // 
            // lstSummary
            // 
            this.lstSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSummary.HorizontalScrollbar = true;
            this.lstSummary.Location = new System.Drawing.Point(8, 8);
            this.lstSummary.Name = "lstSummary";
            this.lstSummary.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSummary.Size = new System.Drawing.Size(632, 277);
            this.lstSummary.TabIndex = 0;
            // 
            // tabMessages
            // 
            this.tabMessages.Controls.Add(this.lstMessages);
            this.tabMessages.Location = new System.Drawing.Point(4, 22);
            this.tabMessages.Name = "tabMessages";
            this.tabMessages.Size = new System.Drawing.Size(648, 295);
            this.tabMessages.TabIndex = 2;
            this.tabMessages.Text = "Messages";
            this.tabMessages.Visible = false;
            // 
            // lstMessages
            // 
            this.lstMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMessages.HorizontalScrollbar = true;
            this.lstMessages.Location = new System.Drawing.Point(8, 8);
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstMessages.Size = new System.Drawing.Size(632, 277);
            this.lstMessages.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(608, 416);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ReBuildIntransitProcess
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 454);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnClose);
            this.Name = "ReBuildIntransitProcess";
            this.Text = "ReBuild Intransit";
            this.Load += new System.EventHandler(this.ReBuildIntransitProcess_Load);
            this.Activated += new System.EventHandler(this.ReBuildIntransitProcess_Activated);
            this.tabControl1.ResumeLayout(false);
            this.tabHierarchy.ResumeLayout(false);
            this.tabSummary.ResumeLayout(false);
            this.tabMessages.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Handle Exception
		protected void HandleException(Exception exc)
		{
			string message = "";

			while (_messageQueue.Count > 0)
			{
				message += (string)_messageQueue.Dequeue() + System.Environment.NewLine;
			}

			message += exc.ToString();

			MessageBox.Show(message, "DatabaseUpdate", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
		}
		#endregion Handle Exception
		
		private void btnProcess_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				if (this.lstHierarchy.SelectedIndices.Count == 0)
				{
					MessageBox.Show("At least one Hierarchy Node selection is required.", this.Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
					return;
				}
				_messageQueue.Clear();
				_summaryQueue.Clear();
				HierarchyNodeList selectedNodeList = new HierarchyNodeList(eProfileType.HierarchyNode);
				for (int i=0; i<this.lstHierarchy.SelectedIndices.Count; i++)
				{
					selectedNodeList.Add((HierarchyNodeProfile)this._levelHierarchyNodeList.ArrayList[this._sortedHierarchyNodePosition[this.lstHierarchy.SelectedIndices[i]]]);
				}
				Process(selectedNodeList);
    
				if (_messageQueue.Count > 0)
				{
					int count = 0;
					string message;
					while (_messageQueue.Count > 0)
					{
						if (count > 0)
						{
							lstMessages.Items.Add("----");
						}

						message = (string)_messageQueue.Dequeue();
						string[] lines = message.Split(_separators);
						foreach (string line in lines)
						{
							if (line.Trim().Length > 0)
							{
								lstMessages.Items.Add(line);
							}
						}
						++count;
					}
				}
				if (_summaryQueue.Count > 0)
				{
					int count = 0;
					string message;
					while (_summaryQueue.Count > 0)
					{
						if (count > 0)
						{
							lstSummary.Items.Add("----");
						}

						message = (string)_summaryQueue.Dequeue();
						string[] lines = message.Split(_separators);
						foreach (string line in lines)
						{
							if (line.Trim().Length > 0)
							{
								lstSummary.Items.Add(line);
							}
						}
						++count;
					}
				}

				// switch to report tab
				this.tabControl1.SelectedTab = this.tabSummary;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
			this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
			this.Close();
		}

		// ========
		// Process 
		// ========
		/// <summary>
		/// Rebuilds Intransit for the parents of style associated with the indicated Hierarchy Node ID
		/// </summary>
		/// <param name="aHierarchyNodeID">Hierarchy Node ID for which Intransit is to be rebuilt</param>
		/// <returns>Status of the intransit rebuild</returns>
		public eReturnCode Process(string aHierarchyNodeID)
		{
			HierarchyNodeProfile hnProfile = this._SAB.HierarchyServerSession.GetNodeData(aHierarchyNodeID);
			return Process (hnProfile);
		}

		/// <summary>
		/// Rebuilds Intransit for the parents of style associated with the hierarchy nodes in the specified list
		/// </summary>
		/// <param name="aHierarchyNodeList">List of Hierarchy Nodes for which Intransit is to be rebuilt</param>
		/// <returns>Status of the intransit rebuild</returns>
		public eReturnCode Process (HierarchyNodeList aHierarchyNodeList)
		{
			eReturnCode maxReturnCode = eReturnCode.successful;
			foreach (HierarchyNodeProfile hnp in aHierarchyNodeList)
			{
				eReturnCode thisReturnCode = Process(hnp);
				if ((int)thisReturnCode > (int)maxReturnCode)
				{
					maxReturnCode = thisReturnCode;
				}
			}
			return maxReturnCode;
		}

		/// <summary>
		/// Rebuilds Intransit for the parents of style associated with the specified Hierarchy Node Profile
		/// </summary>
		/// <param name="aHierarchyNodeProfile">Hierarchy Node Profile for which Intransit is to be rebuilt</param>
		/// <returns>Status of the Intransit rebuild</returns>
		public eReturnCode Process(HierarchyNodeProfile aHierarchyNodeProfile)
		{
			eReturnCode returnCode = eReturnCode.successful;
			try
			{
				AuditRebuildIntransit(eMIDMessageLevel.Information, eMIDTextCode.msg_IT_BeginRebuildIntransit, aHierarchyNodeProfile.NodeID);
				ApplicationSessionTransaction appSessionTransaction = this._SAB.ApplicationServerSession.CreateTransaction();
				if (aHierarchyNodeProfile != null
					&& aHierarchyNodeProfile.Key > Include.NoRID)
				{
					HierarchyNodeList parentOfStyleList;
					if (aHierarchyNodeProfile.IsParentOfStyle)
					{
						parentOfStyleList = new HierarchyNodeList(eProfileType.HierarchyNode);
						parentOfStyleList.Add(aHierarchyNodeProfile);
					}
					else if (aHierarchyNodeProfile.LevelType == eHierarchyLevelType.Style
						|| aHierarchyNodeProfile.LevelType == eHierarchyLevelType.Color
						|| aHierarchyNodeProfile.LevelType == eHierarchyLevelType.Size)
					{
						parentOfStyleList = new HierarchyNodeList(eProfileType.HierarchyNode);
						HierarchyNodeProfile nodeProfile;
						foreach (int hierarchyNodeRID in aHierarchyNodeProfile.Parents)
						{
						    nodeProfile = this._SAB.HierarchyServerSession.GetNodeData(hierarchyNodeRID);
							if (nodeProfile.IsParentOfStyle)
							{
								parentOfStyleList.Add(nodeProfile);
								break;
							}
						}
					}
					else
					{
						parentOfStyleList = appSessionTransaction.GetDescendantData(aHierarchyNodeProfile.Key, eHierarchyLevelMasterType.ParentOfStyle, eNodeSelectType.All);
					}
					if (parentOfStyleList == null
						|| parentOfStyleList.Count == 0)
					{
						AuditRebuildIntransit(
							eMIDMessageLevel.Warning, 
							eMIDTextCode.msg_IT_NoParentOfStyleAssociatedWithHierarchyNode, 
							aHierarchyNodeProfile.NodeID);
						returnCode = eReturnCode.warning;
					}
					else
					{
						HierarchyNodeList stylesWithinParentList;
						HierarchyNodeList colorsWithinStyleList;
						HierarchyNodeList sizesWithinColorList;
						Intransit intransit = new Intransit();
						IntransitDeleteRequest itDeleteRequest = new IntransitDeleteRequest();
						bool rebuildSuccessfulForParentOfStyle;
						foreach (HierarchyNodeProfile parentOfStyleProfile in parentOfStyleList)
						{
							try
							{
								AuditRebuildIntransit(
									eMIDMessageLevel.Information,
									eMIDTextCode.msg_IT_BeginParentOfStyleRebuildIntransit,
									parentOfStyleProfile.NodeID);
								rebuildSuccessfulForParentOfStyle = true;
								itDeleteRequest.Clear();
								itDeleteRequest.AddHnRIDToDeleteIntransitList(parentOfStyleProfile);
								stylesWithinParentList = appSessionTransaction.GetDescendantData(parentOfStyleProfile.Key, eHierarchyLevelMasterType.Style, eNodeSelectType.All);
								if (stylesWithinParentList != null)
								{
									foreach (HierarchyNodeProfile styleProfile in stylesWithinParentList)
									{
										itDeleteRequest.AddHnRIDToDeleteIntransitList(styleProfile);
										colorsWithinStyleList = appSessionTransaction.GetDescendantData(styleProfile.Key, eHierarchyLevelMasterType.Color, eNodeSelectType.All);
										if (colorsWithinStyleList != null)
										{
											foreach (HierarchyNodeProfile colorProfile in colorsWithinStyleList)
											{
												itDeleteRequest.AddHnRIDToDeleteIntransitList(colorProfile);
												sizesWithinColorList = appSessionTransaction.GetDescendantData(colorProfile.Key, eHierarchyLevelMasterType.Size, eNodeSelectType.All);
												if (sizesWithinColorList != null)
												{
													itDeleteRequest.AddHnRIDToDeleteIntransitList(sizesWithinColorList);
												}
											}
										}
									}
									int[] headerRIDList = GetHeadersChargedToIntransit(stylesWithinParentList);
									// BEGIN TT#1185 - Verify ENQ before Update 
									// stodd - These changes were added as part of a manual merge of rebuildIntransit down into the enqueue branch. 
                                    if (headerRIDList != null)
                                    {
                                        AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
                                        apl.LoadHeaders(appSessionTransaction, null, headerRIDList, _SAB.ClientServerSession, false); // TT#488 - MD - Jellis - Group Allocation
                                        //HeaderEnqueue headerEnqueue = null;
                                        try
                                        {
                                            //headerEnqueue = new HeaderEnqueue(appSessionTransaction, headerRIDList);
											List<int> hdrList = new List<int>(headerRIDList);
											bool continueEnqueue = true;
											string conflictMessage = string.Empty;
											while (continueEnqueue)
											{
												continueEnqueue = false;
												bool enqueueStatus = EnqueueHeader(appSessionTransaction, hdrList, out conflictMessage);
												if (!enqueueStatus) // Header conflicts
												{
													SecurityAdmin secAdmin = new SecurityAdmin();
													//ArrayList headerConflicts = headerEnqueue.HeaderConflictList;
													HeaderConflict[] conflictList = appSessionTransaction.HeaderConflicts();
													Header header = new Header();
													//StringBuilder sbHeaderConflicts = new StringBuilder();
													bool autoReleaseHeaderResources = this.cbxAutoReleaseHeaderResources.Checked;
													List<int> conflictKeyList = new List<int>();
													foreach (HeaderConflict headerConflict in conflictList)
													{
														//sbHeaderConflicts.Append(
														//    "header=" + headerConflict.HeaderID
														//    + ", user=" + secAdmin.GetUserName(headerConflict.UserRID)
														//    + ";");
														conflictKeyList.Add(headerConflict.HeaderRID);
													}
													if (autoReleaseHeaderResources)
													{
														autoReleaseHeaderResources = false;
														continueEnqueue = true;
														appSessionTransaction.DequeueHeaders(conflictKeyList);
														//headerEnqueue.HeaderEnqueue_DequeueInConflict();
														this.AuditRebuildIntransit(
															eMIDMessageLevel.Information,
															eMIDTextCode.msg_IT_RebuildIntransitParentOfStyleAutoDeqHeaders,
															parentOfStyleProfile.NodeID,
															conflictMessage);
													}
													else
													{
														this.AuditRebuildIntransit(
															eMIDMessageLevel.Severe,
															eMIDTextCode.msg_IT_RebuildIntransitParentOfStyleHeadersInUse,
															parentOfStyleProfile.NodeID,
															conflictMessage);
														rebuildSuccessfulForParentOfStyle = false;
													}
												}
											}
											//bool continueEnqueue = true;
											//bool autoReleaseHeaderResources = this.cbxAutoReleaseHeaderResources.Checked;
											//while (continueEnqueue)
											//{
											//    try
											//    {
											//        continueEnqueue = false;
											//        headerEnqueue.EnqueueHeaderRIDList();
											//    }
											//    catch (HeaderConflictException)
											//    {

											//        SecurityAdmin secAdmin = new SecurityAdmin();
											//        ArrayList headerConflicts = headerEnqueue.HeaderConflictList;
											//        Header header = new Header();
											//        StringBuilder sbHeaderConflicts = new StringBuilder();
											//        foreach (HeaderConflict headerConflict in headerConflicts)
											//        {
											//            sbHeaderConflicts.Append(
											//                "header=" + header.GetHeaderID(headerConflict.HeaderRID)
											//                + ", user=" + secAdmin.GetUserName(headerConflict.UserRID)
											//                + ";");

											//        }
											//        if (autoReleaseHeaderResources)
											//        {
											//            autoReleaseHeaderResources = false;
											//            continueEnqueue = true;
											//            headerEnqueue.HeaderEnqueue_DequeueInConflict();
											//            this.AuditRebuildIntransit(
											//                eMIDMessageLevel.Information,
											//                eMIDTextCode.msg_IT_RebuildIntransitParentOfStyleAutoDeqHeaders,
											//                parentOfStyleProfile.NodeID,
											//                sbHeaderConflicts.ToString());
											//        }
											//        else
											//        {
											//            this.AuditRebuildIntransit(
											//                eMIDMessageLevel.Severe,
											//                eMIDTextCode.msg_IT_RebuildIntransitParentOfStyleHeadersInUse,
											//                parentOfStyleProfile.NodeID,
											//                sbHeaderConflicts.ToString());
											//            rebuildSuccessfulForParentOfStyle = false;
											//        }
											//    }
											//}
											// END TT#1185 - Verify ENQ before Update 
                                            if (rebuildSuccessfulForParentOfStyle)
                                            {
                                                AllocationProfileList removeList = new AllocationProfileList(eProfileType.Allocation);
                                                foreach (AllocationProfile ap in apl)
                                                {
                                                    if (ap.MultiHeader)
                                                    {
                                                        // Multi intransit is maintained in its member headers; so remove multi's from list
                                                        removeList.Add(ap);
                                                    }
                                                    else
                                                    {
                                                        // re-read headers AFTER enqueue is successful
                                                        ap.ReReadHeader();
                                                    }
                                                }
                                                foreach (AllocationProfile ap in removeList)
                                                {
                                                    apl.Remove(ap);
                                                }
                                                appSessionTransaction.ClearRebuildIntransitUpdateRequest();
                                                appSessionTransaction.SetMasterProfileList(apl);
                                                if (apl.Count > 0)
                                                {
                                                    AllocationWorkFlowStep awfs = new AllocationWorkFlowStep(
                                                        new AllocationAction(eMethodType.RebuildIntransit),
                                                        new GeneralComponent(eGeneralComponentType.Total),
                                                        false,
                                                        false,
                                                        0.0d,
                                                        Include.AllStoreFilterRID,
                                                        0);
                                                    appSessionTransaction.DoAllocationAction(awfs, true); // Temp
                                                    // begin TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
                                                    if (appSessionTransaction.AllocationActionAllHeaderStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                                                    {
                                                        rebuildSuccessfulForParentOfStyle = false;
                                                    }
                                                    // end TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
                                                }
                                                // begin TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
                                                if (rebuildSuccessfulForParentOfStyle)
                                                {
                                                    // end TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
                                                    ArrayList intransitUpdateRequests = appSessionTransaction.GetRebuildIntransitUpdateRequest();
                                                    try
                                                    {
                                                        intransit.OpenUpdateConnection(eLockType.Intransit, parentOfStyleProfile.Key);
                                                        // Begin TT#1875 - JSmith - Database error when sql command is too long
                                                        //if (itDeleteRequest.HnRIDListAsCommaDelimitedText.Length > 0)
                                                        if (itDeleteRequest.GetCountOfRequests() > 0)
                                                        // End TT#1875
                                                        {
                                                            // deletes any existing intransit
                                                            intransit.DeleteIntransit(itDeleteRequest);
                                                        }
                                                        foreach (IntransitUpdateRequest iur in intransitUpdateRequests)
                                                        {
                                                            // rebuilds intransit from header input
                                                            intransit.UpdateIt(iur);
                                                        }
                                                        intransit.CommitData();
                                                    }
                                                    catch (MIDException ex)
                                                    {
                                                        rebuildSuccessfulForParentOfStyle = false;
                                                        _messageQueue.Enqueue(ex.Message);
                                                        _audit.Log_MIDException(ex, this.GetType().Name, eExceptionLogging.logAllInnerExceptions);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        rebuildSuccessfulForParentOfStyle = false;
                                                        _messageQueue.Enqueue(ex.Message);
                                                        _audit.Log_Exception(ex, this.GetType().Name, eExceptionLogging.logAllInnerExceptions);
                                                    }
                                                    finally
                                                    {
                                                        intransit.CloseUpdateConnection();
                                                    }
                                                } // TT#2727 - Jellis - Rebuild Intransit Giving wrong message and gets Null Reference
                                            }
                                        }
                                        catch (MIDException ex)
                                        {
                                            rebuildSuccessfulForParentOfStyle = false;
                                            _messageQueue.Enqueue(ex.Message);
                                            _audit.Log_MIDException(ex, this.GetType().Name, eExceptionLogging.logAllInnerExceptions);
                                        }
                                        catch (Exception ex)
                                        {
                                            rebuildSuccessfulForParentOfStyle = false;
                                            _messageQueue.Enqueue(ex.Message);
                                            _audit.Log_Exception(ex, this.GetType().Name, eExceptionLogging.logAllInnerExceptions);
                                        }
                                        finally
                                        {
											// BEGIN TT#1185 - Verify ENQ before Update 
											// stodd - These changes were added as part of a manual merge of rebuildIntransit down into the enqueue branch. 
                                            if (!appSessionTransaction.AreHeadersInConflict)
                                            {
                                                try
                                                {
													appSessionTransaction.DequeueHeaders();
                                                }
                                                catch (Exception ex)
                                                {
                                                    _audit.Log_Exception(ex, this.GetType().Name, eExceptionLogging.logAllInnerExceptions);
                                                    rebuildSuccessfulForParentOfStyle = false;
                                                }
                                            }
											// End TT#1185 - Verify ENQ before Update 
                                            if (rebuildSuccessfulForParentOfStyle)
                                            {
                                                AuditRebuildIntransit(
                                                    eMIDMessageLevel.Information,
                                                    eMIDTextCode.msg_IT_RebuildIntransitSuccessfulForParentOfStyle,
                                                    parentOfStyleProfile.NodeID);
                                            }
                                            else
                                            {
                                                AuditRebuildIntransit(
                                                    eMIDMessageLevel.Severe,
                                                    eMIDTextCode.msg_IT_RebuildIntransitFailedForParentOfStyle,
                                                    parentOfStyleProfile.NodeID);
                                                returnCode = eReturnCode.severe;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        AuditRebuildIntransit(
                                            eMIDMessageLevel.Information,
                                            eMIDTextCode.msg_IT_RebuildIntransitSuccessfulForParentOfStyle,
                                            parentOfStyleProfile.NodeID);
                                    }
								}
							}
							catch (MIDException ex)
							{
								AuditRebuildIntransit(ex);
								returnCode = eReturnCode.severe;
							}
							catch (Exception ex)
							{
								AuditRebuildIntransit(ex);
								returnCode = eReturnCode.severe;
							}
							finally
							{
								AuditRebuildIntransit(
									eMIDMessageLevel.Information, 
									eMIDTextCode.msg_IT_EndParentOfStyleRebuildIntransit,
									parentOfStyleProfile.NodeID);
							}
						}
					}
				}
				else
				{
					AuditRebuildIntransit(
						eMIDMessageLevel.Error,
						eMIDTextCode.msg_IT_HierarchyNodeNotFound,
						aHierarchyNodeProfile.NodeID);
					returnCode = eReturnCode.editErrors;
				}
			}
			catch (MIDException ex)
			{
				AuditRebuildIntransit(ex);
				returnCode = eReturnCode.severe;
			}
			catch (Exception ex)
			{
				AuditRebuildIntransit( ex);
				returnCode = eReturnCode.severe;
			}
			finally
			{
				if (returnCode == eReturnCode.successful)
				{
					AuditRebuildIntransit(
						eMIDMessageLevel.Information,
						eMIDTextCode.msg_IT_RebuildIntransitSuccessfulForHierarchyNode,
						aHierarchyNodeProfile.Text, true);
				}
				else
				{
				    AuditRebuildIntransit(
						eMIDMessageLevel.Warning,
						eMIDTextCode.msg_IT_RebuildIntransitFailedForHierarchyNode,
                        aHierarchyNodeProfile.Text, true);
				}
				AuditRebuildIntransit(
					eMIDMessageLevel.Information,
					eMIDTextCode.msg_IT_EndRebuildIntransit,
                    aHierarchyNodeProfile.Text);
			}
			return returnCode;
		}

		/// <summary>
		/// Gets a list of Header RIDs that are charged to Intransit and not completely shipped.
		/// </summary>
		/// <param name="aHierarchyNodeList">List of style Hierarchy Node Profiles for which a list of headers is desired</param>
		/// <returns>Integer Array of Header RIDs</returns>
		private int[] GetHeadersChargedToIntransit(HierarchyNodeList aHierarchyNodeList)
		{
			Header dbHeader = new Header();
			int[] styleRIDs = new int[aHierarchyNodeList.Count];
			int i = 0;
			foreach (HierarchyNodeProfile hnp in aHierarchyNodeList)
			{
				styleRIDs[i] = hnp.Key;
				i++;
			}
            if (styleRIDs.Length > 0)
            {
                DataTable headersChargedToIT = dbHeader.GetHeadersChargedToIT(styleRIDs);
                _headersChargedToIntransitCount = headersChargedToIT.Rows.Count;
                ArrayList headerRIDs = new ArrayList();
                AllocationTypeFlags atf = new AllocationTypeFlags(); // TT#488 - MD - Jellis - Group Allocation
                foreach (DataRow dr in headersChargedToIT.Rows)
                {
                    // begin TT#488 - MD - Jellis - Group Allocation
                    atf.AllFlags = Convert.ToUInt32(dr["ALLOCATION_TYPE_FLAGS"], CultureInfo.CurrentUICulture);
                    if (atf.Assortment)
                    {
                        continue;
                    }
                    // end TT#488 - MD - Jellis - Group Allocation
                    ShippingStatusFlags shippingStatusFlags = new ShippingStatusFlags((byte)Convert.ToInt32(dr["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture));
                    if (!shippingStatusFlags.ShippingComplete)
                    {
                        headerRIDs.Add(Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture));
                    }
                    else
                    {
                        _headersCompletelyShippedCount++;
                    }
                }
                int[] headerRIDsArray = new int[headerRIDs.Count];
                headerRIDs.CopyTo(headerRIDsArray);
                return headerRIDsArray;
            }
            _headersChargedToIntransitCount = 0;
            return null;
		}

		// BEGIN TT#1185 - Verify ENQ before Update 
		// stodd - These changes were added as part of a manual merge of rebuildIntransit down into the enqueue branch. 
		private bool EnqueueHeader(ApplicationSessionTransaction aTrans, List<int> aHdrRidList, out string aErrorMsg)
		{
			aErrorMsg = string.Empty;
			bool enqueueStatus;
			List<int> hdrRidList = new List<int>();
			foreach (int hdrRID in aHdrRidList)
			{
				if (hdrRID > 0)       // remove any negative valued RIDs
				{
					hdrRidList.Add(hdrRID);
				}
			}
			if (hdrRidList.Count == 0)
			{
				enqueueStatus = true;  // new headers are not enqueued
			}
			else
			{
				enqueueStatus = aTrans.EnqueueHeaders(aTrans.GetHeadersToEnqueue(aHdrRidList), out aErrorMsg);
				//_headersEnqueued = enqueueStatus;
			}
			return enqueueStatus;
			
		}
		// END TT#1185 - Verify ENQ before Update

		#region EventProcessing
		private void ReBuildIntransitProcess_Activated(object sender, System.EventArgs e)
		{
			this.Activated -= new System.EventHandler(this.ReBuildIntransitProcess_Activated);
			this.cbxHierarchyLevel.SelectedIndex = 0;
		}

		private void AuditRebuildIntransit (eMIDMessageLevel aMIDMessageLevel, eMIDTextCode aMIDTextCode, string aHierarchyNodeID)
		{
			AuditRebuildIntransit (aMIDMessageLevel, aMIDTextCode, aHierarchyNodeID, false);
		}

		private void cbxHierarchyLevel_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            int level = this.cbxHierarchyLevel.SelectedIndex + 1;
			_levelHierarchyNodeList = (HierarchyNodeList)this._hierarchyLevelHash[level];
			if (_levelHierarchyNodeList == null)
			{
				_levelHierarchyNodeList = this._SAB.HierarchyServerSession.GetDescendantDataByLevel(this._SAB.HierarchyServerSession.GetMainHierarchyData().HierarchyRootNodeRID,level, true, eNodeSelectType.All);
				_hierarchyLevelHash.Add(level, _levelHierarchyNodeList);
			}
			this.lstHierarchy.Items.Clear();
			if (_levelHierarchyNodeList.Count > 0)
			{
				string[] sortedHierarchyNodeIDs = new string[_levelHierarchyNodeList.Count];
				_sortedHierarchyNodePosition = new int[_levelHierarchyNodeList.Count];
				for (int i=0; i< _levelHierarchyNodeList.Count; i++)
				{
					HierarchyNodeProfile hnp = (HierarchyNodeProfile)_levelHierarchyNodeList.ArrayList[i];
					sortedHierarchyNodeIDs[i] = hnp.LevelText;
					_sortedHierarchyNodePosition[i] = i;
				}
				Array.Sort(sortedHierarchyNodeIDs, _sortedHierarchyNodePosition);
				this.lstHierarchy.Items.AddRange(sortedHierarchyNodeIDs);
			}
		}


		private void ReBuildIntransitProcess_Load(object sender, System.EventArgs e)
		{
			HierarchyProfile hp = this._SAB.HierarchyServerSession.GetMainHierarchyData();
			ArrayList levelIDs = new ArrayList();
			foreach (HierarchyLevelProfile hlp in hp.HierarchyLevels.Values)
			{
				if (hlp.LevelType != eHierarchyLevelType.Style
					&& hlp.LevelType != eHierarchyLevelType.Color
					&& hlp.LevelType != eHierarchyLevelType.Size)
				{
					levelIDs.Add(hlp);
				}
			}
			string[] sortedLevelIDs = new string[levelIDs.Count];
			foreach (HierarchyLevelProfile hlp in levelIDs)
			{
				sortedLevelIDs[hlp.Level - 1] = hlp.LevelID;
			}
			this.cbxHierarchyLevel.MaxDropDownItems = sortedLevelIDs.Length;
			this.cbxHierarchyLevel.Items.AddRange(sortedLevelIDs);
		}

		#endregion EventProcessing


		#region Audit Rebuild Intransit
		/// <summary>
		/// Adds rebuild intransit audit message to message queue and system audit trail
		/// </summary>
		/// <param name="aMIDMessageLevel">Message Level</param>
		/// <param name="aMIDTextCode">Message Text Code</param>
		/// <param name="aHierarchyNodeID">Hierarchy Node ID to which the message applies</param>
		/// <param name="aSummaryMessage">True:  message will be put on the summary message queue; False: message will not be put on the summary message queue</param>
		private void AuditRebuildIntransit(eMIDMessageLevel aMIDMessageLevel, eMIDTextCode aMIDTextCode, string aHierarchyNodeID, bool aSummaryMessage)
		{
			_auditMessage = 
				string.Format(MIDText.GetTextOnly(aMIDTextCode),aHierarchyNodeID);
			string messageQueueMsg =
				aMIDMessageLevel.ToString() + " " 
				+ ((int)aMIDTextCode).ToString() + " "
				+ _auditMessage;
			_messageQueue.Enqueue(messageQueueMsg);
			if (aSummaryMessage)
			{
				_summaryQueue.Enqueue(messageQueueMsg);
			}
			_audit.Add_Msg(
				aMIDMessageLevel,
				aMIDTextCode,
				_auditMessage,
				this.GetType().Name,
				false);
		}
		/// <summary>
		/// Adds rebuild intransit audit message to message queue and system audit trail
		/// </summary>
		/// <param name="aMIDMessageLevel">Message Level</param>
		/// <param name="aMIDTextCode">Message Text Code</param>
		/// <param name="aHierarchyNodeID">Hierarchy Node ID to which the message applies</param>
		/// <param name="aAdditionalInfo">Additional information to be appended at the end of the message</param>
		private void AuditRebuildIntransit(eMIDMessageLevel aMIDMessageLevel, eMIDTextCode aMIDTextCode, string aHierarchyNodeID, string aAdditionalInfo)
		{
			_auditMessage = 
				string.Format(MIDText.GetTextOnly(aMIDTextCode),aHierarchyNodeID) + " " + aAdditionalInfo;
			_messageQueue.Enqueue(
				aMIDMessageLevel.ToString() + " " 
				+ ((int)aMIDTextCode).ToString() + " "
				+ _auditMessage);
			_audit.Add_Msg(
				aMIDMessageLevel,
				aMIDTextCode,
				_auditMessage,
				this.GetType().Name,
				false);
		}
		/// <summary>
		/// Adds an MID Exception to the message queue and system audit trail
		/// </summary>
		/// <param name="aMIDException">MID exception</param>
		private void AuditRebuildIntransit(MIDException aMIDException)
		{
			_messageQueue.Enqueue(aMIDException.ToString());
			_audit.Log_MIDException(aMIDException, GetType().Name, eExceptionLogging.logAllInnerExceptions);
		}
		/// <summary>
		/// Adds an exception to the message queue and the system audit trail
		/// </summary>
		/// <param name="aException">Exception</param>
		private void AuditRebuildIntransit(Exception aException)
		{
			_messageQueue.Enqueue(aException.ToString());
			_audit.Log_Exception(aException, GetType().Name, eExceptionLogging.logAllInnerExceptions);
		}
		#endregion Audit Rebuild Intransit
	}
}
