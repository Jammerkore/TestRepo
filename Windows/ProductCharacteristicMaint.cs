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
	public partial class ProductCharacteristicMaint : MIDFormBase
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private bool _allowMaintenance;
		private ProductCharProfileList _productCharProfileList;
		private string _menuAddCharacteristic;
		private string _menuAddCharacteristicValue;
		private string _lblNewCharacteristic;
		private string _lblNewCharacteristicValue;
        //private bool _performingCopy = false;
        //private bool _performingCut = false;
		private MIDProductCharNode _pasteNode = null;
		private ArrayList _lockControl = new ArrayList();
		HierarchyMaintenance _hierarchyMaintenance = null;

		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
        public ProductCharacteristicMaint()
            : base()
        {
            InitializeComponent();

        }

		public ProductCharacteristicMaint(SessionAddressBlock aSAB, bool aAllowMaintenance)
			: base(aSAB)
		{
			_allowMaintenance = aAllowMaintenance;
			AllowDragDrop = true;
			_hierarchyMaintenance = new HierarchyMaintenance(aSAB);
			InitializeComponent();

            //tvProdChars.InitializeTreeView(aSAB, true, ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer" ? ParentForm : ParentForm.Owner);
		}
		#endregion Constructors

		#region Properties
		//============
		// PROPERTIES
		//============

		#endregion Properties


		private void ProductCharacteristicMaint_Load(object sender, EventArgs e)
		{
			try
			{
                tvProdChars.InitializeTreeView(SAB, true, ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer" ? ParentForm : ParentForm.Owner);

				tvProdChars.ImageList = MIDGraphics.ImageList;
				FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesCharacteristics);
				if (FunctionSecurity.AllowUpdate)
				{
					Format_Title(eDataState.Updatable, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_ProductCharacteristicMaint));
				}
				else
				{
					Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_ProductCharacteristicMaint));
				}
				SetText();
				BuildContextMenu();
				LoadCharacteristics();
				SetReadOnly(FunctionSecurity.AllowUpdate);
				btnApply.Enabled = false;

                tvProdChars.OnNodeChanged += new ProductCharTreeView.NodeChangeEventHandler(tvProdChars_OnNodeChanged);
                tvProdChars.OnMIDDoubleClick += new MIDTreeView.MIDTreeViewDoubleClickHandler(tvProdChars_OnMIDDoubleClick);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        void tvProdChars_OnMIDDoubleClick(object source, MIDTreeNode node)
        {
            tvProdChars.MouseDownOnNode.BeginEdit();
        }

        void tvProdChars_OnNodeChanged(object source, NodeChangeEventArgs e)
        {
            ChangePending = true;
            btnApply.Enabled = true;
        }

		private void SetText()
		{
			btnApply.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Apply);
			btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
			btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
			_menuAddCharacteristic = MIDText.GetTextOnly(eMIDTextCode.menu_Add_Characteristic);
			_menuAddCharacteristicValue = MIDText.GetTextOnly(eMIDTextCode.menu_Add_Characteristic_Value);
			_lblNewCharacteristic = MIDText.GetTextOnly(eMIDTextCode.lbl_NewCharacteristic);
			_lblNewCharacteristicValue = MIDText.GetTextOnly(eMIDTextCode.lbl_NewCharacteristicValue);
		}

		private void BuildContextMenu()
		{
			try
			{
				tvProdChars.ContextMenuStrip = cmsProductCharTreeView;
				cmiCut.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Cut);
				cmiCut.Image = MIDGraphics.GetImage(MIDGraphics.CutImage);
				cmiCopy.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Copy);
				cmiCopy.Image = MIDGraphics.GetImage(MIDGraphics.CopyImage);
				cmiPaste.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Paste);
				cmiPaste.Image = MIDGraphics.GetImage(MIDGraphics.PasteImage);
				cmiDelete.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Delete);
				cmiDelete.Image = MIDGraphics.GetImage(MIDGraphics.DeleteImage);
				cmiSearch.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search) + "...";
				cmiSearch.Image = MIDGraphics.GetImage(MIDGraphics.FindImage);
				cmiRename.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Rename);
				cmiProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
                cmiInUse.Text = "In Use";
			}
			catch
			{
				throw;
			}
		}

		private void LoadCharacteristics()
		{
			try
			{
				MIDProductCharNode node = null;
				MIDProductCharNode productCharFolder = null;

				productCharFolder = tvProdChars.BuildProductCharFolder(MIDText.GetTextOnly(eMIDTextCode.frm_ProductCharacteristicMaint));
                //productCharFolder.NodeType = eProductCharNodeType.ProductCharFolder;
				productCharFolder.AllowDrop = false;
				tvProdChars.Nodes.Add(productCharFolder);

				_productCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics();
				foreach (ProductCharProfile pcp in _productCharProfileList)
				{
					node = tvProdChars.BuildProductCharNode(pcp.ProductCharID, pcp);
                    //node.NodeType = eProductCharNodeType.ProductChar;
                    //node.NodeType = eProfileType.ProductCharacteristic;
					if (pcp.ProductCharValues.Count > 0)
					{
                        //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                        //node.Nodes.Add(new MIDProductCharNode());
                        node.Nodes.Add(tvProdChars.BuildProductCharValueNode(string.Empty, new ProductCharValueProfile(-1), -1));
                        //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                        node.HasChildren = true;
						node.ChildrenLoaded = false;
					}
					else
					{
						node.HasChildren = false;
						node.ChildrenLoaded = true;
					}
					InsertNode(node, productCharFolder);
				}

				productCharFolder.ChildrenLoaded = true;
				if (_productCharProfileList.Count == 0)
				{
					productCharFolder.HasChildren = false;
					productCharFolder.Nodes.Clear();
				}
				productCharFolder.Expand();
			}
			catch
			{
				throw;
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

		private void btnClose_Click(object sender, EventArgs e)
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

		private void cmiAdd_Click(object sender, EventArgs e)
		{
			try
			{
				NewNode();
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		private void cmiCut_Click(object sender, EventArgs e)
		{
			try
			{
				ICut();
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		private void cmiCopy_Click(object sender, EventArgs e)
		{
			try
			{
				ICopy();
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		private void cmiPaste_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;

			try
			{
				_pasteNode = (MIDProductCharNode)tvProdChars.MouseDownOnNode;
				PasteCharacteristic();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void cmiDelete_Click(object sender, EventArgs e)
		{
			try
			{
				IDelete();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cmiProperties_Click(object sender, EventArgs e)
		{
			try
			{   // BEGIN MID Track #5460 - error adding node to new characteristic
                if (ChangePending)
                {
                    SaveChanges();
                }
                // END MID Track #5460
				foreach (MIDProductCharNode charNode in tvProdChars.GetSelectedNodes())
				{
                    //if (charNode.NodeType == eProductCharNodeType.ProductCharValue)
                    if (charNode.NodeType == eProfileType.ProductCharacteristicValue)
					{
						ProductCharProperties searchForm = new ProductCharProperties(SAB, charNode.ProductCharGroupKey, charNode.Profile.Key, charNode.Text, charNode.Parent.Text);
						//  Allow for a floating explorer.   ParentForm is not Client.Explorer if floating
						if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
						{
							searchForm.MdiParent = this.ParentForm;
						}
						else
						{
							searchForm.MdiParent = this.ParentForm.Owner;
						}
						searchForm.Show();
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void cmiSearch_Click(object sender, EventArgs e)
		{
			try
			{
				ProductCharSearch searchForm = new ProductCharSearch(SAB);
				searchForm.ProductCharLocateEvent += new ProductCharSearch.ProductCharLocateEventHandler(searchForm_ProductCharLocateEvent);
				searchForm.ProductCharRenameEvent += new ProductCharSearch.ProductCharRenameEventHandler(searchForm_ProductCharRenameEvent);
				searchForm.ProductCharDeleteEvent += new ProductCharSearch.ProductCharDeleteEventHandler(searchForm_ProductCharDeleteEvent);
				//  Allow for a floating explorer.   ParentForm is not Client.Explorer if floating
				if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
				{
					searchForm.MdiParent = this.ParentForm;
				}
				else
				{
					searchForm.MdiParent = this.ParentForm.Owner;
				}
				searchForm.Show();
			}
			catch
			{
				throw;
			}
		}

		void searchForm_ProductCharDeleteEvent(object source, ProductCharDeleteEventArgs e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		void searchForm_ProductCharRenameEvent(object source, ProductCharRenameEventArgs e)
		{
			try
			{
				lock (_lockControl.SyncRoot)
				{
					Cursor.Current = Cursors.WaitCursor;
					MIDProductCharNode node;

					// start with main characteristic folder
					node = (MIDProductCharNode)tvProdChars.Nodes[0];
					// locate characteristic
					node = LocateNode(e.CharacteristicKey, node);

					// locate value
					// if characteristic is not expanded, expand and make sure values are loaded
					if (!node.IsExpanded)
					{
						node.Expand();

						bool loop = true;
						while (loop)
						{
							if (node.ChildrenLoaded &&
								node.IsExpanded)
							{
								loop = false;
							}
							else
							{
								System.Threading.Thread.Sleep(10);
							}
						}
					}
					node = LocateNode(e.ValueKey, node);

					if (node != null)
					{
						ChangePending = true;
						btnApply.Enabled = true;
						node.NodeID = e.Text;
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//node.Text = e.Text;
						node.InternalText = e.Text;
						//End Track #6201 - JScott - Store Count removed from attr sets
						// set node as being updated
						if (node.NodeChangeType == eChangeType.none)
						{
							node.NodeChangeType = eChangeType.update;
						}
						// if value, set parent characteristic node as being updated
                        //if (node.NodeType == eProductCharNodeType.ProductCharValue)
                        if (node.NodeType == eProfileType.ProductCharacteristicValue)
						{
							if (((MIDProductCharNode)node.Parent).NodeChangeType == eChangeType.none)
							{
								((MIDProductCharNode)node.Parent).NodeChangeType = eChangeType.update;
							}
						}
					}

				}
			}
			catch
			{
				throw;
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		void searchForm_ProductCharLocateEvent(object source, ProductCharLocateEventArgs e)
		{
			try
			{
				lock (_lockControl.SyncRoot)
				{
					Cursor.Current = Cursors.WaitCursor;
					tvProdChars.SelectedNode = null;
					tvProdChars.SimulateMultiSelect = true;
					MIDProductCharNode treeNode;

					// start with main characteristic folder
					treeNode = (MIDProductCharNode)tvProdChars.Nodes[0];
					// locate characteristic
					treeNode = LocateNode(e.CharacteristicKey, treeNode);

					// locate value
					// if characteristic is not expanded, expand and make sure values are loaded
					if (!treeNode.IsExpanded)
					{
						treeNode.Expand();

						bool loop = true;
						while (loop)
						{
							if (treeNode.ChildrenLoaded &&
								treeNode.IsExpanded)
							{
								loop = false;
							}
							else
							{
								System.Threading.Thread.Sleep(10);
							}
						}
					}
					treeNode = LocateNode(e.ValueKey, treeNode);

					if (treeNode != null)
					{
						tvProdChars.SelectedNode = treeNode;
					}

				}
			}
			catch
			{
				throw;
			}
			finally
			{
				tvProdChars.SimulateMultiSelect = false;
				Cursor.Current = Cursors.Default;
			}
		}

		private MIDProductCharNode LocateNode(int aNodeRID, MIDProductCharNode aParentFolder)
		{
			try
			{
				foreach (MIDProductCharNode node in aParentFolder.Nodes)
				{
					if (node.Profile.Key == aNodeRID)
					{
						return node;
					}
				}
				MessageBox.Show("Node not found");
				return null;
			}
			catch
			{
				throw;
			}
		}

		private void tvProdChars_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				MIDProductCharNode node = (MIDProductCharNode)e.Node;
				if (node.HasChildren &&
					!node.ChildrenLoaded)
				{
					AddValues(node);
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void tvProdChars_AfterExpand(object sender, TreeViewEventArgs e)
		{
			MIDProductCharNode node = (MIDProductCharNode)e.Node;
            //Begin Track #5664 - KJohnson - Product Characteristic Issues
            if (node.NodeChangeType != eChangeType.delete)
            {
                node.ImageIndex = tvProdChars.GetFolderImageIndex(MIDGraphics.OpenFolder);
                node.SelectedImageIndex = node.ImageIndex;
            }
            //End Track #5664
        }

		private void tvProdChars_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			MIDProductCharNode node = (MIDProductCharNode)e.Node;
            //Begin Track #5664 - KJohnson - Product Characteristic Issues
            if (node.NodeChangeType != eChangeType.delete)
            {
                node.ImageIndex = tvProdChars.GetFolderImageIndex(MIDGraphics.ClosedFolder);
                node.SelectedImageIndex = node.ImageIndex;
            }
            //End Track #5664
        }

		private void AddValues(MIDProductCharNode aProductCharNode)
		{
			try
			{
				ProductCharProfile pcp = (ProductCharProfile)_productCharProfileList.FindKey(aProductCharNode.Profile.Key);
				aProductCharNode.Nodes.Clear();
				MIDProductCharNode node = null;

				foreach (ProductCharValueProfile pcvp in pcp.ProductCharValues)
				{
					node = tvProdChars.BuildProductCharValueNode(pcvp.ProductCharValue, pcvp, aProductCharNode.Profile.Key);
					//aProductCharNode.Nodes.Add(node);
					InsertNode(node, aProductCharNode);
				}
				aProductCharNode.ChildrenLoaded = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cmsProductCharTreeView_Opening(object sender, CancelEventArgs e)
		{
            //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
            //TreeNodeClipboardList cbList = null;
            object cbList = null;
            //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
            try
			{
				cmiAdd.Visible = false;
				cmiCopy.Visible = false;
				cmiCut.Visible = false;
				cmiDelete.Visible = false;
				cmiPaste.Visible = false;
				cmiRename.Visible = false;
				cmiProperties.Visible = false;
                cmiInUse.Visible = false;
				cmiSeparator1.Visible = false;
				cmiSeparator2.Visible = false;
				cmiSeparator3.Visible = false;

				IDataObject data = Clipboard.GetDataObject();
                //ClipboardProfile cbp = null;

                if (data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                    //cbList = (TreeNodeClipboardList)data.GetData(typeof(TreeNodeClipboardList));
                    cbList = data.GetData(typeof(TreeNodeClipboardList));
                    //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                }

				if (!tvProdChars.NoNodesSelected)
				{
					foreach (MIDProductCharNode node in tvProdChars.GetSelectedNodes())
					{
                        //if (node.NodeType == eProductCharNodeType.ProductCharFolder)
                        if (node.NodeType == eProfileType.MerchandiseMainProductCharFolder)
						{
							if (FunctionSecurity.AllowUpdate)
							{
								cmiAdd.Visible = true;
								cmiAdd.Text = _menuAddCharacteristic;
							}
						}
                        //else if (node.NodeType == eProductCharNodeType.ProductChar)
                        else if (node.NodeType == eProfileType.ProductCharacteristic)
						{
                            cmiInUse.Visible = true;
                            cmiSeparator3.Visible = true;
							if (FunctionSecurity.AllowUpdate)
							{
								cmiAdd.Visible = true;
								cmiAdd.Text = _menuAddCharacteristicValue;
								cmiSeparator1.Visible = true;
								cmiRename.Visible = true;
                                //if (cbp != null &&
                                //    (cbp.ClipboardDataType == eClipboardDataType.ProductChar ||
                                //    cbp.ClipboardDataType == eClipboardDataType.ProductCharList))
                                //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                                if (cbList != null)
                                {
                                    if ((cbList is TreeNodeClipboardList && (((TreeNodeClipboardList)cbList).ClipboardDataType == eProfileType.ProductCharacteristic)) ||
                                        (cbList is ProductCharacteristicClipboardList && (((ProductCharacteristicClipboardList)cbList).ClipboardDataType == eProfileType.ProductCharacteristicValue)))
                                    {
                                        //cmiPaste.Visible = true;  //TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                    }
                                }
                                //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                            }
							if (FunctionSecurity.AllowDelete)
							{
								cmiDelete.Visible = true;
							}
                        }
                        //else if (node.NodeType == eProductCharNodeType.ProductCharValue)
                        else if (node.NodeType == eProfileType.ProductCharacteristicValue)
						{
							cmiProperties.Visible = true;
                            cmiInUse.Visible = true;
							cmiSeparator3.Visible = true;
							if (FunctionSecurity.AllowUpdate)
							{
								cmiSeparator2.Visible = true;
								cmiRename.Visible = true;
                                //cmiCopy.Visible = true; //TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                //if (cbp != null &&
                                //    (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode ||
                                //    cbp.ClipboardDataType == eClipboardDataType.HierarchyNodeList))
                                //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                                if (cbList != null)
                                {
                                    if ((cbList is TreeNodeClipboardList && (((TreeNodeClipboardList)cbList).ClipboardDataType == eProfileType.HierarchyNode)) ||
                                        (cbList is ProductCharacteristicClipboardList && (((ProductCharacteristicClipboardList)cbList).ClipboardDataType == eProfileType.HierarchyNode)))
                                    {
                                        //cmiPaste.Visible = true;    //TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                    }
                                }
                                //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                            }
							if (FunctionSecurity.AllowDelete)
							{
								cmiSeparator2.Visible = true;
                                //cmiCut.Visible = true;  //TT#4428-VStuart-Cut Option should be removed from Admin-Product Characteristics
								cmiDelete.Visible = true;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cmiRename_Click(object sender, EventArgs e)
		{
			try
			{
				Rename();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void tvProdChars_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			try
			{
				MIDProductCharNode node = (MIDProductCharNode)e.Node;
                //if (node.NodeType == eProductCharNodeType.ProductCharFolder)
                if (node.NodeType == eProfileType.MerchandiseMainProductCharFolder)
				{
					e.CancelEdit = true;
					return;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        //private void tvProdChars_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        //{
        //    try
        //    {
        //        //Begin Track #5664 - KJohnson - Product Characteristic Issues
        //        string labelText = e.Label;

        //        if (labelText == null)
        //        {
        //            labelText = e.Node.Text;
        //        }

        //        if (labelText == "")
        //        {
        //            e.CancelEdit = true;
        //        }
        //        else 
        //        {
        //            MIDProductCharNode node = (MIDProductCharNode)e.Node;
        //            if (tvProdChars.ValidName(node, labelText))
        //            {
        //                node.NodeID = labelText;
        //                // set node as being updated
        //                if (node.NodeChangeType == eChangeType.none)
        //                {
        //                    node.NodeChangeType = eChangeType.update;
        //                }
        //                // if value, set parent characteristic node as being updated
        //                //if (node.NodeType == eProductCharNodeType.ProductCharValue)
        //                if (node.NodeType == eProfileType.ProductCharacteristicValue)
        //                {
        //                    if (((MIDProductCharNode)node.Parent).NodeChangeType == eChangeType.none)
        //                    {
        //                        ((MIDProductCharNode)node.Parent).NodeChangeType = eChangeType.update;
        //                    }
        //                }
        //                ChangePending = true;
        //                btnApply.Enabled = true;
        //            }
        //            else
        //            {
        //                //if (node.NodeType == eProductCharNodeType.ProductChar)
        //                if (node.NodeType == eProfileType.ProductCharacteristic)
        //                {
        //                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharGroupName), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //                else
        //                {
        //                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }

        //                e.CancelEdit = true;
        //            }
        //        }
        //        //End Track #5664
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private bool ValidName(MIDProductCharNode aNode, string aNewName)
        //{
        //    try
        //    {
        //        HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
        //        //if (aNode.NodeType == eProductCharNodeType.ProductChar)
        //        if (aNode.NodeType == eProfileType.ProductCharacteristic)
        //        {
        //            return hm.IsProductCharNameValid(aNode.Profile.Key, aNewName);
        //        }
        //        else
        //        {
        //            return hm.IsProductCharValueValid(aNode.Profile.Key, ((MIDProductCharNode)aNode.Parent).Profile.Key, aNewName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //    return false;
        //}

        //private void tvProdChars_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    tvProdChars.MouseDownOnNode.BeginEdit();
        //}

		private void tvProdChars_KeyDown(object sender, KeyEventArgs e)
		{
			if (tvProdChars.CurrentState != eDragStates.Idle)
			{
				return; // precondition, can't change effect while moving
			}

			if (e.KeyCode == Keys.X && e.Modifiers == Keys.Control) 
			{
				ICut();
				e.Handled = true;
			}
			else
				if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control) 
			{
				ICopy();
				e.Handled = true;
			}
			else
				if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control) 
			{
				_pasteNode = (MIDProductCharNode)tvProdChars.MouseDownOnNode;
				PasteCharacteristic();
				e.Handled = true;
			}
			else
				if (e.KeyCode == Keys.R && e.Modifiers == Keys.Control)  
			{
				Rename();
				e.Handled = true;
			}
			else
				if (e.KeyCode == Keys.N && e.Modifiers == Keys.Control)  
			{
				NewNode();
				e.Handled = true;
			}
			else
				if (e.KeyCode == Keys.D && e.Modifiers == Keys.Control)
				{
					IDelete();
					e.Handled = true;
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
				// main node
				MIDProductCharNode charFolder = (MIDProductCharNode)tvProdChars.Nodes[0];
				if (charFolder.Nodes != null)
				{
					foreach (MIDProductCharNode charGroup in charFolder.Nodes)
					{
						if (charGroup.NodeChangeType != eChangeType.none)
						{
							ProductCharProfile pcp = (ProductCharProfile)_productCharProfileList.FindKey(charGroup.Profile.Key);
							if (pcp == null)
							{
								pcp = new ProductCharProfile(charGroup.Profile.Key);
							}
							pcp.ProductCharChangeType = charGroup.NodeChangeType;
							pcp.ProductCharID = charGroup.NodeID;
							if (charGroup.IsExpanded ||
								pcp.Key == Include.NoRID)
							{
								pcp.ProductCharValues.Clear();
								foreach (MIDProductCharNode valueNode in charGroup.Nodes)
								{
									ProductCharValueProfile pcvp = new ProductCharValueProfile(valueNode.Profile.Key);
									pcvp.ProductCharValueChangeType = valueNode.NodeChangeType;
									pcvp.ProductCharValue = valueNode.NodeID;
                                    // Begin TT#715-MD - JSmith - Product Characteristics does not delete 
                                    pcvp.ProductCharRID = ((ProductCharValueProfile)valueNode.Profile).ProductCharRID;
                                    // End TT#715-MD - JSmith - Product Characteristics does not delete 
                                    //BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                    pcvp.HasBeenMoved = valueNode.HasBeenMoved;
                                    //END TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                    pcp.ProductCharValues.Add(pcvp);
								}
							}

                            //BEGIN TT#1179 - System.NullReferenceException when create a new Product Characteristic and Delete
                            // Begin TT#1743 - JSmith - Add Product Characteristics Fails
                            //if (pcp.Key != -1)
                            if (pcp.Key != -1 ||
                                pcp.ProductCharChangeType == eChangeType.add)
                            // End TT#1743
                            {
                                pcp = SAB.HierarchyServerSession.ProductCharUpdate(pcp, true); // TT#3558 - JSmith - Perf of Hierarchy Load
                            }
                            //END TT#1179

                            charGroup.Profile.Key = pcp.Key;
                            
							// update tree nodes
                            // Begin TT#1743 - JSmith - Add Product Characteristics Fails
                            //tvProdChars.RemoveDeletedNodes();
                            //charGroup.NodeChangeType = eChangeType.none;
                            // End TT#1743
							foreach (MIDProductCharNode valueNode in charGroup.Nodes)
							{
								if (valueNode.NodeChangeType == eChangeType.add)
								{
									ProductCharValueProfile pcvp = null;
									foreach (ProductCharValueProfile value in pcp.ProductCharValues)
									{
										if (valueNode.NodeID == value.ProductCharValue)
										{
											pcvp = value;
											break;
										}
									}
									if (pcvp != null)
									{
										valueNode.Profile.Key = pcvp.Key;
									}
								}
								valueNode.NodeChangeType = eChangeType.none;
							}
						}
					}
                    // Begin TT#1743 - JSmith - Add Product Characteristics Fails
                    tvProdChars.RemoveDeletedNodes();
                    foreach (MIDProductCharNode charGroup in charFolder.Nodes)
                    {
                        charGroup.NodeChangeType = eChangeType.none;
                    }
                    // End TT#1743
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

		public void Rename()
		{
			try
			{
				if (tvProdChars.NoNodesSelected)
				{
					return;
				}

				MIDProductCharNode node = (MIDProductCharNode)tvProdChars.GetSelectedNode(0);
				node.BeginEdit();
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		private void NewNode()
		{
			try
			{
				MIDProductCharNode node = null;
				MIDProductCharNode newNode = null;
				if (tvProdChars.NoNodesSelected)
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnableToAddRow), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

				}
				else
				{
					node = (MIDProductCharNode)tvProdChars.GetSelectedNode(0);
                    //if (node.NodeType == eProductCharNodeType.ProductCharFolder)
                    if (node.NodeType == eProfileType.MerchandiseMainProductCharFolder)
					{
						ChangePending = true;
						btnApply.Enabled = true;
						newNode = tvProdChars.BuildProductCharNode(Convert.ToString(_lblNewCharacteristic.Clone(), CultureInfo.CurrentCulture), new ProductCharProfile(Include.NoRID));
						newNode.NodeChangeType = eChangeType.add;
						node.Nodes.Add(newNode);
						node.Expand();
						newNode.BeginEdit();
					}
					else
					{
						ChangePending = true;
						btnApply.Enabled = true;
						newNode = tvProdChars.BuildProductCharValueNode(Convert.ToString(_lblNewCharacteristicValue.Clone(), CultureInfo.CurrentCulture), new ProductCharValueProfile(Include.NoRID), node.Profile.Key);
						newNode.NodeChangeType = eChangeType.add;
						if (node.NodeChangeType == eChangeType.none)
						{
							node.NodeChangeType = eChangeType.update;
						}

                        //Begin Track #5664 - KJohnson - Product Characteristic Issues
                        //newNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.NotesImage);
                        //newNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.NotesImage);
                        //End Track #5664

                        //Begin Track #5664 - KJohnson - Product Characteristic Issues
                        // Begin TT#223 - JSmith - Adding first product characteristic value leaves tree collapsed
                        //node.Expand();
                        node.Nodes.Add(newNode);
                        node.Expand();
                        // End TT#223
						newNode.BeginEdit();
                        //End Track #5664
                    }
				}
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		private void tvProdChars_DragDrop(object sender, DragEventArgs e)
		{
            //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
            //TreeNodeClipboardList cbList = null;
            object cbList = null;
            //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
            try
			{
				Point pt = tvProdChars.PointToClient(new Point(e.X, e.Y));
				_pasteNode = (MIDProductCharNode)tvProdChars.GetNodeAt(pt.X, pt.Y);
				//BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
                if (e.Data.GetDataPresent(typeof(ProductCharacteristicClipboardList)))
				{
                    cbList = (ProductCharacteristicClipboardList)e.Data.GetData(typeof(ProductCharacteristicClipboardList));
                    //if (cbp.ClipboardDataType == eClipboardDataType.ProductChar ||
                    //    cbp.ClipboardDataType == eClipboardDataType.ProductCharList)
                    //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
            		if (((ProductCharacteristicClipboardList)cbList).ClipboardDataType == eProfileType.ProductCharacteristicValue)
					{
                        PasteCharacteristics((ProductCharacteristicClipboardList)cbList);
                    }
				}
                else if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    if (((TreeNodeClipboardList)cbList).ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        PasteProducts((TreeNodeClipboardList)cbList);
                    }
                    //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
					//END TT#3962-VStuart-Dragged Values never allowed to drop-MID
                }
				tvProdChars.RemovePaintFromNodes();
				tvProdChars.PaintSelectedNodes();

                //Begin TT#1037 - JSmith - Drag/Drop product characteristic issue
                e.Effect = DragDropEffects.None;
                //End TT#1037
			}
			catch
			{
				throw;
			}
		}

		private bool ValidToDrop(DragDropEffects aDropAction, MIDProductCharNode aDropNode, ArrayList aSelectedNodes)
		{
			if (tvProdChars.isDropAllowed(aDropAction, aDropNode, aSelectedNodes))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private void PasteCharacteristic()
		{
			this.Cursor = Cursors.WaitCursor;

			try
			{
				_pasteNode = (MIDProductCharNode)tvProdChars.MouseDownOnNode;
				IPaste();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

        //Begin TT#1531-MD -jsobek -Add In Use for Product Characteristics
        private void cmiInUse_Click(object sender, EventArgs e)
        {
            try
            {  
                if (ChangePending)
                {
                    SaveChanges();
                }

                eProfileType etype = eProfileType.ProductCharacteristic;
                ArrayList ridList = new ArrayList();
                foreach (MIDProductCharNode charNode in tvProdChars.GetSelectedNodes())
                {
                    if (charNode.NodeType == eProfileType.ProductCharacteristic)
                    {
                        int cRid = charNode.NodeRID;
                        ridList.Add(cRid);
                    }
                    else if (charNode.NodeType == eProfileType.ProductCharacteristicValue)
                    {
                        etype = eProfileType.ProductCharacteristicValue;
                        int cRid = charNode.NodeRID;
                        ridList.Add(cRid);
                    }
                }
                if (ridList.Count > 0)
                {
                    string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype);
                    bool display = false;
                    DisplayInUseForm(ridList, etype, inUseTitle, ref display, true);
                }
            }
            catch
            {
                throw;
            }
        }
        private bool IsInUse(eProfileType etype, int aRID)
        {
            bool isInUse = false;
            ArrayList ridList = new ArrayList();
            ridList.Add(aRID);
            //If no RID is selected do nothing.
            if (ridList.Count > 0)
            {
                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype);
                DisplayInUseForm(ridList, etype, inUseTitle, false, out isInUse);
            }
            return isInUse;
        }
        //End TT#1531-MD -jsobek -Add In Use for Product Characteristics

		#region IFormBase Members
		public override void ICut()
		{
			try
			{
                //_performingCut = true;
                //_performingCopy = false;
				ArrayList copyNodes = new ArrayList();
				foreach (MIDProductCharNode selectedNode in tvProdChars.GetSelectedNodes())
				{
					copyNodes.Add(selectedNode);
				}
				if (copyNodes.Count > 0)
				{
                    //tvProdChars.CopyToClipboard(copyNodes, eDropAction.Move);
					tvProdChars.CopyToClipboard(tvProdChars.BuildClipboardList(copyNodes, DragDropEffects.Move));
				}
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
                //_performingCut = false;
                //_performingCopy = true;
				ArrayList copyNodes = new ArrayList();

				foreach (MIDProductCharNode selectedNode in tvProdChars.GetSelectedNodes())
				{
					copyNodes.Add(selectedNode);
				}

				if (copyNodes.Count > 0)
				{
                    //tvProdChars.CopyToClipboard(copyNodes, eDropAction.Copy);
					tvProdChars.CopyToClipboard(tvProdChars.BuildClipboardList(copyNodes, DragDropEffects.Copy));
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		public override void IPaste()
		{
			// Create a new instance of the DataObject interface.
			IDataObject data = Clipboard.GetDataObject();

			//If the data is ClipboardProfile, then retrieve the data
            //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
            //TreeNodeClipboardList cbList = null;
            object cbList = null;
            //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID

            if (data.GetDataPresent(typeof(TreeNodeClipboardList)))
			{
                //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                cbList = data.GetData(typeof(TreeNodeClipboardList));
                //if (cbp.ClipboardDataType == eClipboardDataType.ProductChar ||
                //    cbp.ClipboardDataType == eClipboardDataType.ProductCharList)
                if (cbList is ProductCharacteristicClipboardList &&
                    ((ProductCharacteristicClipboardList)cbList).ClipboardDataType == eProfileType.ProductCharacteristicValue)
				{
                    PasteCharacteristics((ProductCharacteristicClipboardList)cbList);
				}
                //else if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode ||
                //    cbp.ClipboardDataType == eClipboardDataType.HierarchyNodeList)
                else if (cbList is TreeNodeClipboardList && 
                    ((TreeNodeClipboardList)cbList).ClipboardDataType == eProfileType.HierarchyNode)
				{
                    PasteProducts((TreeNodeClipboardList)cbList);
				}
                //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
            }
		}

        //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
        //private void PasteCharacteristics(TreeNodeClipboardList aClipboardList)
        private void PasteCharacteristics(ProductCharacteristicClipboardList aClipboardList)
        //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
        {
			this.Cursor = Cursors.WaitCursor;

			try
			{

				ArrayList nodes = new ArrayList();

                if (aClipboardList != null)
				{
                    //if (aClipboardProfile.ClipboardDataType == eClipboardDataType.ProductChar)
                    //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                    if (aClipboardList.ClipboardDataType == eProfileType.ProductCharacteristicValue)
					{
                        foreach (ProductCharacteristicClipboardProfile item in aClipboardList.ClipboardItems)
                        //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
                        {
                            nodes.Add(GetNode(item));
                        }

                        //if (aClipboardProfile.isList)
                        //{
                        //    ArrayList items = (ArrayList)aClipboardProfile.ClipboardData;
                        //    foreach (ClipboardProfile item in items)
                        //    {
                        //        nodes.Add(GetNode(item));
                        //    }
                        //}
                        //else if (aClipboardProfile.ClipboardData.GetType() == typeof(ProductCharClipboardData))
                        //{
                        //    nodes.Add(GetNode(aClipboardProfile));
                        //}
					}
                    //else if (aClipboardProfile.ClipboardDataType == eClipboardDataType.ProductCharList)
                    //{
                    //    if (aClipboardProfile.ClipboardData.GetType() == typeof(ArrayList))
                    //    {
                    //        ArrayList items = (ArrayList)aClipboardProfile.ClipboardData;
                    //        foreach (ClipboardProfile item in items)
                    //        {
                    //            nodes.Add(GetNode(item));
                    //        }
                    //    }
                    //}

                    //Begin Track #5664 - KJohnson - Product Characteristic Issues
                    //if (_pasteNode.NodeType == eProductCharNodeType.ProductCharValue) 
                    if (_pasteNode.NodeType == eProfileType.ProductCharacteristicValue)
                    {
                        _pasteNode = (MIDProductCharNode)_pasteNode.Parent;
                    }
                    //End Track #5664

                    PasteCharacteristic(aClipboardList.ClipboardProfile.Action, _pasteNode, nodes);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}

		}

		private void PasteCharacteristic(DragDropEffects aDropAction, MIDProductCharNode aToNode, ArrayList aNodes)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;

				tvProdChars.CurrentState = eDragStates.Idle;
				tvProdChars.CurrentEffect = DragDropEffects.Move;
				if (aNodes.Count == 0)
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NothingToPaste));
				}
				else
					if (ValidToDrop(aDropAction, aToNode, aNodes))
					{
						bool movingNode = false;
						bool copyingNode = false;
						bool makingShortCut = false;
						int actions = 0;
						// determine message
						string message = null;
						string title = null;
						foreach (MIDProductCharNode treeNode in aNodes)
						{
							// it's a folder, drop the file
							switch (treeNode.DropAction)
							{
								case DragDropEffects.Move:
									if (!movingNode)
									{
										++actions;
									}
									movingNode = true;
									break;
								case DragDropEffects.Copy:
									if (!copyingNode)
									{
										++actions;
									}
									copyingNode = true;
									break;
								case DragDropEffects.Link:
									if (!makingShortCut)
									{
										++actions;
									}
									makingShortCut = true;
									break;
							}
						}

						if (actions > 1)
						{
						}
						else if (movingNode)
						{
							title = "Move";
							if (aNodes.Count == 1)
							{
								MIDProductCharNode moveNode = (MIDProductCharNode)aNodes[0];
								message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmMoveNode, false);
								message = message.Replace("{0}", moveNode.Text);
								message = message.Replace("{1}", aToNode.Text);
							}
							else
							{
								message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmMoveNodes, false);
								message = message.Replace("{0}", aToNode.Text);
							}
						}
						else if (copyingNode)
						{
							title = "Copy";
							if (aNodes.Count == 1)
							{
								MIDProductCharNode copyNode = (MIDProductCharNode)aNodes[0];
								message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyNode, false);
								message = message.Replace("{0}", copyNode.Text);
								message = message.Replace("{1}", aToNode.Text);
							}
							else
							{
								message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyNodes, false);
								message = message.Replace("{0}", aToNode.Text);
							}
						}
						else if (makingShortCut)
						{
							title = "Reference";
							if (aNodes.Count == 1)
							{
								MIDProductCharNode copyNode = (MIDProductCharNode)aNodes[0];
								message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmShortCut, false);
								message = message.Replace("{0}", copyNode.Text);
								message = message.Replace("{1}", aToNode.Text);
							}
							else
							{
								message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmShortcuts, false);
								message = message.Replace("{0}", aToNode.Text);
							}
						}

						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, this.GetType().Name);
						if (MessageBox.Show(message, title,
								MessageBoxButtons.YesNo, MessageBoxIcon.Question)
								== DialogResult.No)
						{
							return;
						}

						foreach (MIDProductCharNode treeNode in aNodes)
						{
							// it's a folder, drop the file
							switch (treeNode.DropAction)
							{
								case DragDropEffects.Move:
									MoveNode(treeNode, aToNode);
									break;
								case DragDropEffects.Copy:
									CopyNode(treeNode, aToNode);
									break;
								case DragDropEffects.Link:
									MakeShortCut(treeNode, aToNode);
									break;
							}
						}
						if (!aToNode.IsExpanded)
						{
							aToNode.Expand();
						}
                        //Begin Track #5664 - KJohnson - Product Characteristic Issues
                        ChangePending = true;
                        btnApply.Enabled = true;
                        //End Track #5664
                    }
			}
			catch
			{
				throw;
			}
			finally
			{
				this.Invalidate(new Region(this.ClientRectangle));
				tvProdChars.RemovePaintFromNodes();   //  reset any highlighted drop nodes
				tvProdChars.CurrentEffect = DragDropEffects.None;
				this.Cursor = Cursors.Default;
			}
		}

        private void PasteProducts(TreeNodeClipboardList aClipboardList)
		{
			this.Cursor = Cursors.WaitCursor;

			try
			{
                if (aClipboardList != null)
				{
                    //if (aClipboardProfile.ClipboardDataType == eClipboardDataType.HierarchyNode)
                    if (aClipboardList.ClipboardDataType == eProfileType.HierarchyNode)
					{
                        foreach (TreeNodeClipboardProfile item in aClipboardList.ClipboardItems)
                        {
                            AddProduct(item.Key, item.Node.Text);
                        }

                        //if (aClipboardProfile.isList)
                        //{
                        //    ArrayList items = (ArrayList)aClipboardProfile.ClipboardData;
                        //    foreach (ClipboardProfile item in items)
                        //    {
                        //        AddProduct(item.Key, item.Text);
                        //    }
                        //}
                        //else
                        //{
                        //    AddProduct(aClipboardProfile.Key, aClipboardProfile.Text);
                        //}
					}
                    //else if (aClipboardProfile.ClipboardDataType == eClipboardDataType.HierarchyNodeList)
                    //{
                    //    if (aClipboardProfile.ClipboardData.GetType() == typeof(ArrayList))
                    //    {
                    //        ArrayList items = (ArrayList)aClipboardProfile.ClipboardData;
                    //        foreach (ClipboardProfile item in items)
                    //        {
                    //            AddProduct(item.Key, item.DragImageText);
                    //        }
                    //    }
                    //}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}

		}

		private void AddProduct(int aNodeRID, string aNodeText)
		{
			try
			{
				// check to see if the node is assigned a different group value
				string currentValue;
				int currentValueRID;
				if (_hierarchyMaintenance.IsProductCharAlreadyAssigned(aNodeRID, _pasteNode.ProductCharGroupKey, _pasteNode.Profile.Key, out currentValueRID, out currentValue))
				{
					string text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ProductCharAlreadyAssigned, false);
					text = text.Replace("{0}", aNodeText);
					text = text.Replace("{1}", currentValue);
					text = text.Replace("{2}", _pasteNode.Text);
					if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
					== DialogResult.No)
					{
						return;
					}
					_hierarchyMaintenance.UpdateProductCharValue(aNodeRID, _pasteNode.ProductCharGroupKey, currentValueRID, eChangeType.delete);
				}
				_hierarchyMaintenance.UpdateProductCharValue(aNodeRID, _pasteNode.ProductCharGroupKey, _pasteNode.Profile.Key, eChangeType.add);
                //Begin Track #5664 - KJohnson - Product Characteristic Issues
                ChangePending = true;
                btnApply.Enabled = true;
                //End Track #5664
			}
			catch
			{
				throw;
			}
		}

        //BEGIN TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID
        private MIDProductCharNode GetNode(ProductCharacteristicClipboardProfile aClipboardProfile)
		{
			try
			{
                MIDProductCharNode treeNode = (MIDProductCharNode)tvProdChars.FindTreeNode(tvProdChars.Nodes, eProfileType.ProductCharacteristicValue, aClipboardProfile.Key);
				if (treeNode == null)
				{
					return tvProdChars.BuildProductCharValueNode(string.Empty, new ProductCharValueProfile(aClipboardProfile.Key), Include.NoRID);
				}
				else
				{
					return treeNode;
				}
			}
			catch
			{
				throw;
			}
		}
        //END TT#3961-MD-VStuart-Receive cast error when attempt to cut/paste values from characteristic to another characteristic-MID

        private MIDProductCharNode GetNode(TreeNodeClipboardProfile aClipboardProfile)
        {
            try
            {
                //ProductCharClipboardData clipboardData = (ProductCharClipboardData)aClipboardProfile.ClipboardData;
                MIDProductCharNode treeNode = (MIDProductCharNode)tvProdChars.FindTreeNode(tvProdChars.Nodes, aClipboardProfile.Node.Profile.ProfileType, aClipboardProfile.Key);
                if (treeNode == null)
                {
                    return tvProdChars.BuildProductCharValueNode(string.Empty, new ProductCharValueProfile(aClipboardProfile.Key), Include.NoRID);
                }
                else
                {
                    return treeNode;
                }
            }
            catch
            {
                throw;
            }
        }

		private void MoveNode(MIDProductCharNode moveNode, MIDProductCharNode toNode)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				tvProdChars.BeginUpdate();
				MIDProductCharNode currentParent = (MIDProductCharNode)moveNode.Parent;
				if (currentParent.NodeChangeType == eChangeType.none)
				{
					currentParent.NodeChangeType = eChangeType.update;
				}
				if (toNode.NodeChangeType == eChangeType.none)
				{
					toNode.NodeChangeType = eChangeType.update;
				}
				MIDProductCharNode newNode = moveNode.CloneNode();
				moveNode.NodeChangeType = eChangeType.delete;

                //BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
                moveNode.HasBeenMoved = true;
                newNode.HasBeenMoved = true;
                newNode.NodeChangeType = eChangeType.update;
                //END TT#3962-VStuart-Dragged Values never allowed to drop-MID

                //Begin Track #5664 - KJohnson - Product Characteristic Issues
                moveNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteIcon);
                moveNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteIcon);
                //End Track #5664

				InsertNode(newNode, toNode);
				ChangePending = true;
				btnApply.Enabled = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			finally
			{
				
				this.tvProdChars.EndUpdate();
				Cursor.Current = Cursors.Default;
			}
		}

		private void RepositionNode(MIDProductCharNode node)
		{
			try
			{
				MIDProductCharNode parent = (MIDProductCharNode)node.Parent;
				parent.Nodes.Remove(node);
				InsertNode(node, parent);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void InsertNode(MIDProductCharNode insertNode, MIDProductCharNode inNode)
		{
			try
			{
				int index = 0;
				foreach (MIDProductCharNode searchNode in inNode.Nodes)  // Auto sorting causes problems,  so I will do it myself
				{
					if (String.Compare(insertNode.Text, searchNode.Text) < 0)
					{
						break;
					}
					index++;
				}

                //Begin Track #5664 - KJohnson - Product Characteristic Issues
                //if (insertNode.NodeType == eProductCharNodeType.ProductCharValue)
                //if (insertNode.NodeType == eProfileType.ProductCharacteristicValue)
                //{
                //    insertNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.NotesImage);
                //    insertNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.NotesImage);
                //}
                //End Track #5664

				inNode.Nodes.Insert(index, insertNode);
				inNode.HasChildren = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void CopyNode(MIDProductCharNode copyNode, MIDProductCharNode toNode)
		{
			try
			{
				MIDProductCharNode newNode = copyNode.CloneNode();
				newNode.NodeChangeType = eChangeType.add;
				if (toNode.NodeChangeType == eChangeType.none)
				{
					toNode.NodeChangeType = eChangeType.update;
				}
				InsertNode(newNode, toNode);
				ChangePending = true;
				btnApply.Enabled = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void MakeShortCut(MIDProductCharNode copyNode, MIDProductCharNode toNode)
		{
			try
			{
				//InsertNode(mtn, toNode);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			finally
			{
				this.tvProdChars.EndUpdate();
				Cursor.Current = Cursors.Default;
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
				if (tvProdChars.OnlyOneNodesSelected)
				{
					text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteItem, false);
					text = text.Replace("{0}", tvProdChars.SelectedNode.Text);
				}
				else
				{
					text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteItems, false);
					text = text.Replace("{0}", tvProdChars.SelectedNodeCount.ToString());
				}

				if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
					== DialogResult.No)
				{
					return;
				}

				foreach (MIDProductCharNode node in tvProdChars.GetSelectedNodes())
				{
					// if value, set parent characteristic node as being updated
     
                    if (node.NodeType == eProfileType.ProductCharacteristic)
					{

                        bool isCharInUse = IsInUse(eProfileType.ProductCharacteristic, node.NodeRID);   //TT#1531-MD -jsobek -Add In Use for Product Characteristics
                        if (!isCharInUse)
                        {
                            node.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteIcon);
                            node.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteIcon);
                                
                            node.NodeChangeType = eChangeType.delete;
                            node.ForeColor = SystemColors.InactiveCaption;

                            foreach (MIDProductCharNode childNode in node.Nodes)
                            {
                                //Begin Track #5664 - KJohnson - Product Characteristic Issues
                                childNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteIcon);
                                childNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteIcon);
                                //End Track #5664
                            }
                            ChangePending = true;
                            btnApply.Enabled = true;
                        }
                    }
                    //else if (node.NodeType == eProductCharNodeType.ProductCharValue)
                    else if (node.NodeType == eProfileType.ProductCharacteristicValue)
                    {
                        bool isCharInUse = IsInUse(eProfileType.ProductCharacteristicValue, node.NodeRID);   //TT#1531-MD -jsobek -Add In Use for Product Characteristics
                        if (!isCharInUse)
                        {
                            node.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteIcon);
                            node.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteIcon);

                            node.NodeChangeType = eChangeType.delete;
                            node.ForeColor = SystemColors.InactiveCaption;
                            if (((MIDProductCharNode)node.Parent).NodeChangeType == eChangeType.none)
                            {
                                ((MIDProductCharNode)node.Parent).NodeChangeType = eChangeType.update;
                            }
                            ChangePending = true;
                            btnApply.Enabled = true;
                        }
                    }
				}
		
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		#endregion
		
	}
}