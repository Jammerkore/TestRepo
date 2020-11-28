using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;


namespace MIDRetail.Windows
{
	public partial class ProductCharProperties : MIDFormBase
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private int _productCharRID;
		private int _valueRID;
		private string _value;
		private string _productChar;
		private eDragStates _currentState = eDragStates.Idle;
		HierarchyProfile _mainHierarchy = null;
		HierarchyMaintenance _hierarchyMaintenance = null;

		#endregion Fields

		#region Constructors
		public ProductCharProperties(SessionAddressBlock aSAB, int aProductCharRID, int aValueRID, string aValue,
			string aProductChar)
			: base(aSAB)
		{
			_productCharRID = aProductCharRID;
			_valueRID = aValueRID;
			_value = aValue;
			_productChar = aProductChar;
			_hierarchyMaintenance = new HierarchyMaintenance(aSAB);
			InitializeComponent();
		}
		#endregion Constructors

		#region Properties
		//============
		// PROPERTIES
		//============

		#endregion Properties

		private void ProductCharProperties_Load(object sender, EventArgs e)
		{
			lvAssigned.SmallImageList = MIDGraphics.ImageList;
			FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesCharacteristics);
			
			if (FunctionSecurity.AllowUpdate)
			{
				Format_Title(eDataState.Updatable, eMIDTextCode.frm_Properties, _value);
			}
			else
			{
				Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_Properties, _value);
			}
			SetText();
			BuildContextMenu();
			_mainHierarchy = SAB.HierarchyServerSession.GetMainHierarchyData();
			LoadProducts();
			SetReadOnly(FunctionSecurity.AllowUpdate);
			btnApply.Enabled = false;
		}

		private void SetText()
		{
			btnApply.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Apply);
			btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
			btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
			lblCharacteristic.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Characteristic) + ":" + _productChar;
			gbxAssigned.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Assigned);
			colProduct.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Product);
			colHierarchy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hierarchy);
			colLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Node_Level);
		}

		private void BuildContextMenu()
		{
			try
			{
				lvAssigned.ContextMenuStrip = cmsAssigned;
				cmiCut.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Cut);
				cmiCut.Image = MIDGraphics.GetImage(MIDGraphics.CutImage);
				cmiCopy.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Copy);
				cmiCopy.Image = MIDGraphics.GetImage(MIDGraphics.CopyImage);
				cmiPaste.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Paste);
				cmiPaste.Image = MIDGraphics.GetImage(MIDGraphics.PasteImage);
				cmiDelete.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Delete);
				cmiDelete.Image = MIDGraphics.GetImage(MIDGraphics.DeleteImage);
			}
			catch
			{
				throw;
			}
		}

		private void LoadProducts()
		{
			try
			{
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.Hier_Char_Join_ReadByValue(_valueRID);
				foreach (DataRow dr in dt.Rows)
				{
					AddProduct(Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentCulture), eChangeType.none);
				}
			}
			catch
			{
				throw;
			}
		}

		private void AddProduct(int aNodeRID, eChangeType aChangeType)
		{
			try
			{
				if (aChangeType == eChangeType.add)
				{
					if (NodeAlreadyAssigned(aNodeRID))
					{
						return;
					}
				}
				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(aNodeRID, false);
				// check to see if the node is assigned a different group value
				string currentValue;
				int currentValueRID;
				if (aChangeType == eChangeType.add &&
					_hierarchyMaintenance.IsProductCharAlreadyAssigned(aNodeRID, _productCharRID, _valueRID, out currentValueRID, out currentValue))
				{
					string text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ProductCharAlreadyAssigned, false);
					text = text.Replace("{0}", hnp.Text);
					text = text.Replace("{1}", currentValue);
					text = text.Replace("{2}", _value);
					if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
					== DialogResult.No)
					{
						return;
					}
					_hierarchyMaintenance.UpdateProductCharValue(aNodeRID, _productCharRID, currentValueRID, eChangeType.delete);
				}
				HierarchyNodeSecurityProfile nodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(aNodeRID, (int)eSecurityTypes.All);
				if (!nodeSecurity.AccessDenied)
				{
					string folderColor = Include.MIDDefaultColor;
					string hierarchy = string.Empty;
					string level = string.Empty;
					if (hnp.HomeHierarchyRID == _mainHierarchy.Key)
					{
						if (hnp.HomeHierarchyLevel > 0)
						{
							folderColor = ((HierarchyLevelProfile)_mainHierarchy.HierarchyLevels[hnp.HomeHierarchyLevel]).LevelColor;
							level = ((HierarchyLevelProfile)_mainHierarchy.HierarchyLevels[hnp.HomeHierarchyLevel]).LevelID;
							hierarchy = _mainHierarchy.HierarchyID;
						}
						else
						{
							folderColor = _mainHierarchy.HierarchyColor;
							level = _mainHierarchy.HierarchyID;
							hierarchy = _mainHierarchy.HierarchyID;
						}
					}
					else
					{
						hierarchy = ((HierarchyProfile)SAB.HierarchyServerSession.GetHierarchyData(hnp.HomeHierarchyRID)).HierarchyID;
					}
					int imageIndex;
					imageIndex = MIDGraphics.ImageIndexWithDefault(folderColor, MIDGraphics.ClosedFolder);

                    //BEGIN tt#1068 - Properties of Characteristic Values - Color level merchandise must be qualified with the Style - apicchetti - 01/13/2011
                    string[] items = null;
                    if (hnp.NodeLevel >= 6)
                    {
                        string path = "";
                        string _delim = SAB.ClientServerSession.GlobalOptions.ProductLevelDelimiter.ToString();

                        NodeAncestorList ancestors = SAB.HierarchyServerSession.GetNodeAncestorList(aNodeRID);

                        ArrayList alAncestors = ancestors.ArrayList;

                        int ctr_nodeLevel = hnp.NodeLevel;

                        foreach (NodeAncestorProfile hnpAncestor in alAncestors)
                        {
                            if (ctr_nodeLevel >= 5)
                            {
                                HierarchyNodeProfile parentHP = SAB.HierarchyServerSession.GetNodeData(hnpAncestor.Key, false);

                                if (path == "")
                                {
                                    path = parentHP.Text;
                                }
                                else
                                {
                                    path = parentHP.Text  + _delim + path;
                                }

                            }

                            ctr_nodeLevel--;
                            
                        }

                        items = new string[] { path, hierarchy, level };
                    }
                    else
                    {
                        items = new string[] { hnp.Text, hierarchy, level };
                    }
                    //END tt#1068 - Properties of Characteristic Values - Color level merchandise must be qualified with the Style - apicchetti - 01/13/2011


					System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(items, imageIndex);
					item.Tag = new MIDProductCharPropertyItemTag(aNodeRID, hnp.HomeHierarchyRID, hnp.HomeHierarchyLevel, nodeSecurity, aChangeType);
					lvAssigned.Items.Add(item);
					if (aChangeType != eChangeType.none)
					{
						ChangePending = true;
						btnApply.Enabled = true;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private bool NodeAlreadyAssigned(int aNodeRID)
		{
			try
			{
				foreach (ListViewItem item in lvAssigned.Items)
				{
					MIDProductCharPropertyItemTag itemTag = (MIDProductCharPropertyItemTag)item.Tag;
					if (itemTag.NodeRID == aNodeRID)
					{
						return true;
					}
				}

				return false;
			}
			catch
			{
				throw;
			}
		}

		private void cmsAssigned_Opening(object sender, CancelEventArgs e)
		{
            TreeNodeClipboardList cbList;
            TreeNodeClipboardProfile cbp;

			try
			{
				bool showMenu = false;
				cmiCopy.Visible = false;
				cmiCut.Visible = false;
				cmiDelete.Visible = false;
				cmiPaste.Visible = false;
				cmiSeparator1.Visible = false;

				IDataObject data = Clipboard.GetDataObject();

                if (data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    cbList = (TreeNodeClipboardList)data.GetData(typeof(TreeNodeClipboardList));
                    //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode ||
                    //    cbp.ClipboardDataType == eClipboardDataType.HierarchyNodeList)
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
					{
						if (FunctionSecurity.AllowUpdate)
						{
							cmiPaste.Visible = true;
							showMenu = true;
						}
					}
				}
				
				if (lvAssigned.SelectedItems.Count > 0)
				{
					foreach (ListViewItem item in lvAssigned.SelectedItems)
					{
						MIDProductCharPropertyItemTag itemTag = (MIDProductCharPropertyItemTag)item.Tag;
						if (FunctionSecurity.AllowUpdate &&
							itemTag.NodeSecurity.AllowUpdate)
						{
							cmiCopy.Visible = true;
							showMenu = true;
						}
						if (FunctionSecurity.AllowDelete &&
							itemTag.NodeSecurity.AllowDelete)
						{
							cmiSeparator1.Visible = true;
							cmiCut.Visible = true;
							cmiDelete.Visible = true;
							showMenu = true;
						}
					}
				}

				// no items to select, do not show menu
				if (!showMenu)
				{
					e.Cancel = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cmiCut_Click(object sender, EventArgs e)
		{
			try
			{
				ICut();
			}
			catch
			{
				throw;
			}
		}

		private void cmiCopy_Click(object sender, EventArgs e)
		{
			try
			{
				ICopy();
			}
			catch
			{
				throw;
			}
		}

		private void cmiPaste_Click(object sender, EventArgs e)
		{
			try
			{
				IPaste();
			}
			catch
			{
				throw;
			}
		}

		private void cmiDelete_Click(object sender, EventArgs e)
		{
			try
			{
				IDelete();
			}
			catch
			{
				throw;
			}
		}

		private void gbxAssigned_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void gbxAssigned_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		private void gbxAssigned_DragLeave(object sender, EventArgs e)
		{
			Image_DragLeave(sender, e);
		}
		
		private void lvAssigned_DragEnter(object sender, DragEventArgs e)
		{
            TreeNodeClipboardList cbList;

			try
			{
				Image_DragEnter(sender, e);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //if ((cbp.ClipboardDataType == eClipboardDataType.HierarchyNode ||
                    //    cbp.ClipboardDataType == eClipboardDataType.HierarchyNodeList) &&
                    //    FunctionSecurity.AllowUpdate)
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode &&
                        FunctionSecurity.AllowUpdate)
					{
						e.Effect = DragDropEffects.All;
					}
					else
					{
						e.Effect = DragDropEffects.None;
					}
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void lvAssigned_DragOver(object sender, DragEventArgs e)
		{
			try
			{
				Image_DragOver(sender, e);
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void lvAssigned_DragLeave(object sender, EventArgs e)
		{
			Image_DragLeave(sender, e);
		}

		private void lvAssigned_DragDrop(object sender, DragEventArgs e)
		{
            TreeNodeClipboardList cbList;
			try
			{
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    AddProducts(cbList);
                }
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        private void AddProducts(TreeNodeClipboardList aClipboardList)
		{
			try
			{
                foreach (TreeNodeClipboardProfile item in aClipboardList.ClipboardItems)
                {
                    AddProduct(item.Key, eChangeType.add);
                }

                //if (aClipboardProfile.ClipboardDataType == eClipboardDataType.HierarchyNode)
                //if (aClipboardProfile.ClipboardDataType == eProfileType.HierarchyNode)
                //{
                //    if (aClipboardProfile.isList)
                //    {
                //        ArrayList items = (ArrayList)aClipboardProfile.ClipboardData;
                //        foreach (ClipboardProfile item in items)
                //        {
                //            AddProduct(item.Key, eChangeType.add);
                //        }
                //    }
                //    else
                //    {
                //        AddProduct(aClipboardProfile.Key, eChangeType.add);
                //    }
                //}
                //else if (aClipboardProfile.ClipboardDataType == eClipboardDataType.HierarchyNodeList)
                //{
                //    if (aClipboardProfile.ClipboardData.GetType() == typeof(ArrayList))
                //    {
                //        ArrayList items = (ArrayList)aClipboardProfile.ClipboardData;
                //        foreach (ClipboardProfile item in items)
                //        {
                //            AddProduct(item.Key, eChangeType.add);
                //        }
                //    }
                //}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void lvAssigned_KeyDown(object sender, KeyEventArgs e)
		{
			if (_currentState != eDragStates.Idle)
			{
				return; // precondition, can't change effect while moving
			}

			if (e.KeyCode == Keys.X && e.Modifiers == Keys.Control)
			{
				ICut();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
			{
				ICopy();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
			{
				IPaste();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.D && e.Modifiers == Keys.Control)
			{
				IDelete();
				e.Handled = true;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				SaveChanges();
				Close();
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			try
			{
				Cancel_Click();
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			try
			{
				SaveChanges();
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		override protected bool SaveChanges()
		{
			try
			{
				if (!ChangePending)
				{
					return true;
				}
				ErrorFound = false;

				foreach (ListViewItem item in lvAssigned.Items)
				{
					MIDProductCharPropertyItemTag itemTag = (MIDProductCharPropertyItemTag)item.Tag;
					switch (itemTag.ChangeType)
					{
						case eChangeType.add:
							_hierarchyMaintenance.UpdateProductCharValue(itemTag.NodeRID, _productCharRID, _valueRID, itemTag.ChangeType);
							break;
						case eChangeType.delete:
							_hierarchyMaintenance.UpdateProductCharValue(itemTag.NodeRID, _productCharRID, _valueRID, itemTag.ChangeType);
							lvAssigned.Items.Remove(item);
							break;
					}
				}
				
				ChangePending = false;
				btnApply.Enabled = false;
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}

			return true;
		}

		override protected ClipboardProfileBase BuildClipboardItem(System.Windows.Forms.ListViewItem aItem, DragDropEffects aAction)
		{

            try
			{
				MIDProductCharPropertyItemTag itemTag = (MIDProductCharPropertyItemTag)aItem.Tag;
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(itemTag.NodeRID, false);
                
				HierarchyTreeView tempTreeView = new HierarchyTreeView();
                tempTreeView.InitializeTreeView(SAB, false, ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer" ? ParentForm : ParentForm.Owner);
                MIDHierarchyNode tempNode = new MIDHierarchyNode(SAB, eTreeNodeType.ObjectNode, hnp, hnp.Text, Include.NoRID, Include.NoRID, null, Include.NoRID, Include.NoRID, Include.NoRID);
                TreeNodeClipboardProfile cbp = new TreeNodeClipboardProfile(tempNode);
                cbp.Action = aAction;
				tempTreeView.Nodes.Add(tempNode);
				cbp.DragImage = lvAssigned.SmallImageList.Images[aItem.ImageIndex];
                cbp.DragImageHeight = tempNode.Bounds.Height;
				cbp.DragImageWidth = tempNode.Bounds.Width;

                return cbp;
			}
			catch
			{
				throw;
			}
		}

		#region IFormBase Members
		public override void ICut()
		{
			try
			{
				CopyProductCharacteristicsToClipboard(lvAssigned, eProfileType.ProductCharacteristic, DragDropEffects.Move);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		public override void ICopy()
		{
			try
			{
				CopyProductCharacteristicsToClipboard(lvAssigned, eProfileType.ProductCharacteristic, DragDropEffects.Copy);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		public override void IPaste()
		{
            TreeNodeClipboardList cbList;

			// Create a new instance of the DataObject interface.
			IDataObject data = Clipboard.GetDataObject();

			//If the data is ClipboardProfile, then retrieve the data
            //ClipboardProfile cbp = null;

            if (data.GetDataPresent(typeof(TreeNodeClipboardList)))
			{
                cbList = (TreeNodeClipboardList)data.GetData(typeof(TreeNodeClipboardList));
                AddProducts(cbList);
			}
		}

		public override void ISave()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				SaveChanges();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		public override void ISaveAs()
		{

		}

		public override void IRefresh()
		{
			try
			{
				if (FormLoaded)
				{

				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		public override void IDelete()
		{
			try
			{
				string text = string.Empty;
				if (lvAssigned.SelectedItems.Count == 1)
				{
					text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteItem, false);
					text = text.Replace("{0}", lvAssigned.SelectedItems[0].Text);
				}
				else
				{
					text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteItems, false);
					text = text.Replace("{0}", lvAssigned.SelectedItems.Count.ToString());
				}

				if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
					== DialogResult.No)
				{
					return;
				}

				foreach (ListViewItem item in lvAssigned.SelectedItems)
				{
					((MIDProductCharPropertyItemTag)item.Tag).ChangeType = eChangeType.delete;
					item.ForeColor = SystemColors.InactiveCaption;
				}
				ChangePending = true;
				btnApply.Enabled = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		#endregion

	}

	public class MIDProductCharPropertyItemTag
	{
		private int _nodeRID;
		private int _homeHierarchyRID;
		private int _hierarchyLevel;
		private HierarchyNodeSecurityProfile _nodeSecurity;
		private eChangeType _changeType;

		public MIDProductCharPropertyItemTag(int aNodeRID, int aHomeHierarchyRID,
			int aHierarchyLevel, HierarchyNodeSecurityProfile aNodeSecurity, eChangeType aChangeType)
		{
			_nodeRID = aNodeRID;
			_homeHierarchyRID = aHomeHierarchyRID;
			_hierarchyLevel = aHierarchyLevel;
			_nodeSecurity = aNodeSecurity;
			_changeType = aChangeType;
		}

		public int NodeRID
		{
			get { return _nodeRID; }
		}

		public int HomeHierarchyRID
		{
			get { return _homeHierarchyRID; }
		}

		public int HierarchyLevel
		{
			get { return _hierarchyLevel; }
		}

		/// <summary>
		/// Gets or sets the security profile of the product for the user.
		/// </summary>
		public HierarchyNodeSecurityProfile NodeSecurity
		{
			get { return _nodeSecurity; }
		}

		/// <summary>
		/// Gets or sets the change type of the item.
		/// </summary>
		public eChangeType ChangeType
		{
			get { return _changeType; }
			set { _changeType = value; }

		}
	}

}