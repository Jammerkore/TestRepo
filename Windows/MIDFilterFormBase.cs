using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public partial class MIDFilterFormBase : MIDRetail.Windows.MIDFormBase
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private Rectangle _dragBoxFromMouseDown;
		private int _multiSelectStartIdx;
		private int _multiSelectEndIdx;
		private int _indexOfItemUnderMouseToDrag;
		private FilterDefinition _filterDef;
		private Panel _currentPanel;

		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		// Base constructor required for Windows Form Designer support
		public MIDFilterFormBase()
		{
			InitializeComponent();
		}

		public MIDFilterFormBase(SessionAddressBlock aSAB) : base (aSAB)
		{
			InitializeComponent();
		}

		#endregion Constructors

		#region Properties
		//=============
		// PROPERTIES
		//=============
		public Panel CurrentPanel
		{
			get
			{
				return _currentPanel;
			}
			set
			{
				_currentPanel = value;
			}
		}

		public TextBox LiteralEditTextBox
		{
			get
			{
				return txtLiteralEdit;
			}
		}

		public TextBox GradeEditTextBox
		{
			get
			{
				return txtGradeEdit;
			}
		}

		public FilterDefinition FilterDef
		{
			get
			{
				return _filterDef;
			}
			set
			{
				_filterDef = value;
			}
		}

		public int MultiSelectStartIdx
		{
			get
			{
				return _multiSelectStartIdx;
			}
			set
			{
				_multiSelectStartIdx = value;
			}
		}

		public int MultiSelectEndIdx
		{
			get
			{
				return _multiSelectEndIdx;
			}
			set
			{
				_multiSelectEndIdx = value;
			}
		}

		//public ContextMenuStrip LabelMenu
		//{
		//    get
		//    {
		//        return cmsLabelMenu;
		//    }
		//}

		#endregion Properties


		#region Methods

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}

				this.txtLiteralEdit.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtLiteralEdit_KeyPress);
				this.txtLiteralEdit.Leave -= new System.EventHandler(this.txtLiteralEdit_Leave);
				this.txtGradeEdit.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtGradeEdit_KeyPress);
				this.txtGradeEdit.Leave -= new System.EventHandler(this.txtGradeEdit_Leave);
				this.cmsLabelMenu.Opening -= new System.ComponentModel.CancelEventHandler(this.cmsLabelMenu_Opening);
			}
			base.Dispose(disposing);
		}

		#region toolButton Events
		protected void BaseToolButton_Click(object sender, EventArgs e)
		{
            QueryOperand operand;

            try
            {
				operand = (QueryOperand)Activator.CreateInstance((Type)((Button)sender).Tag, new object[] { _filterDef });
                if (operand.isMainOperand)
                {
                    CurrentPanelAddOperand(operand);
                    CurrentPanelRedrawOperands();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		protected void BaseToolButton_MouseDown(object sender, MouseEventArgs e)
		{
            Size dragSize;

            try
            {
                dragSize = SystemInformation.DragSize;
                _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		protected void BaseToolButton_MouseMove(object sender, MouseEventArgs e)
		{
            try
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {
						DoDragDrop(new FilterDragObject((QueryOperand)Activator.CreateInstance((Type)((Button)sender).Tag, new object[] { _filterDef }), ((Button)sender).Text), DragDropEffects.Move);
						Image_EndDrag();
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		protected void BaseToolButton_MouseUp(object sender, MouseEventArgs e)
		{
            _dragBoxFromMouseDown = Rectangle.Empty;
		}

		protected void BaseToolButton_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:

                        if (((PanelTag)CurrentPanel.Tag).SelectedOperandList.Count > 0)
                        {
                            CurrentPanelDeleteSelectedOperands();
                            CurrentPanelRedrawOperands();
                        }

                        break;
                }

                e.Handled = true;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		protected void BaseToolButton_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
            try
            {
                if (((PanelTag)CurrentPanel.Tag).SelectedOperandList.Count == 1)
                {
                    ((BasicQueryLabel)((QueryOperand)((PanelTag)CurrentPanel.Tag).SelectedOperandList[0]).Label).StartEdit(e.KeyChar);
                }

                e.Handled = true;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}
		#endregion toolButton Events

		#region txtLiteralEdit Events
		private void txtLiteralEdit_Leave(object sender, System.EventArgs e)
		{
			try
			{
				txtLiteralEdit.Enabled = false;
				txtLiteralEdit.Visible = false;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtLiteralEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				if (e.KeyChar == 13 || e.KeyChar == 9)
				{
					((DataQueryLiteralOperand)txtLiteralEdit.Tag).LiteralValue = Convert.ToDouble(txtLiteralEdit.Text, CultureInfo.CurrentUICulture);
					CurrentPanelClearSelectedOperands();
					CurrentPanelRedrawOperands();
					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
					e.Handled = true;
				}
				else if (e.KeyChar == 27)
				{
					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
					e.Handled = true;
				}
				else if (Char.IsNumber(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
				{
				}
				else
				{
					e.Handled = true;
				}
			}
			catch (FormatException)
			{
			}
			catch (InvalidCastException)
			{
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		#endregion

		#region txtGradeEdit Events
		private void txtGradeEdit_Leave(object sender, System.EventArgs e)
		{
			try
			{
				txtGradeEdit.Enabled = false;
				txtGradeEdit.Visible = false;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtGradeEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				if (e.KeyChar == 13 || e.KeyChar == 9)
				{
					((DataQueryGradeOperand)txtGradeEdit.Tag).GradeValue = txtGradeEdit.Text;
					CurrentPanelClearSelectedOperands();
					CurrentPanelRedrawOperands();
					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
					e.Handled = true;
				}
				else if (e.KeyChar == 27)
				{
					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
					e.Handled = true;
				}
				else if (Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
				{
				}
				else
				{
					e.Handled = true;
				}
			}
			catch (FormatException)
			{
			}
			catch (InvalidCastException)
			{
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		#endregion

		#region toolList Events
		protected void BaseToolList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            Size dragSize;

            try
            {
                _indexOfItemUnderMouseToDrag = ((ListBox)sender).IndexFromPoint(e.X, e.Y);

                if (_indexOfItemUnderMouseToDrag != ListBox.NoMatches)
                {
                    dragSize = SystemInformation.DragSize;
                    _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                }
                else
                {
                    _dragBoxFromMouseDown = Rectangle.Empty;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		protected void BaseToolList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            ListBox listBox;
            Label label;
            LabelTag labelTag;
            object listItem;
            object profile;

            try
            {
                listBox = (ListBox)sender;
                label = (Label)listBox.Tag;
                labelTag = (LabelTag)label.Tag;

                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {
                        listBox.Visible = false;
                        listItem = listBox.Items[_indexOfItemUnderMouseToDrag];

                        if (listItem.GetType() == typeof(DataRowView))
                        {
                            profile = ((DataRowView)listItem).Row["Profile"];
                            if (profile.GetType() == typeof(VariableProfile))
                            {
                                labelTag.CurrentObject = profile;
                                label.Text = ((VariableProfile)profile).VariableName;
								DoDragDrop(new FilterDragObject(new DataQueryPlanVariableOperand(_filterDef, (VariableProfile)profile)), DragDropEffects.Move);
								Image_EndDrag();
							}
							else if (profile.GetType() == typeof(TimeTotalVariableReference))
							{
								labelTag.CurrentObject = profile;
								label.Text = ((TimeTotalVariableReference)profile).TimeTotalVariableProfile.VariableName;
								DoDragDrop(new FilterDragObject(new DataQueryTimeTotalVariableOperand(_filterDef, (TimeTotalVariableReference)profile)), DragDropEffects.Move);
								Image_EndDrag();
							}
							else if (profile.GetType() == typeof(VersionProfile))
							{
								labelTag.CurrentObject = profile;
								label.Text = ((VersionProfile)profile).Description;
								DoDragDrop(new FilterDragObject(profile), DragDropEffects.Move);
								Image_EndDrag();
							}
							else if (profile.GetType() == typeof(StatusProfile))
							{
								labelTag.CurrentObject = profile;
								label.Text = ((StatusProfile)profile).Description;
								DoDragDrop(new FilterDragObject(new DataQueryStatusOperand(_filterDef, (eStoreStatus)((StatusProfile)profile).Key)), DragDropEffects.Move);
								Image_EndDrag();
							}
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		protected void BaseToolList_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            ListBox listBox;
            Label label;
            LabelTag labelTag;

            try
            {
                listBox = (ListBox)sender;
                label = (Label)listBox.Tag;
                labelTag = (LabelTag)label.Tag;

                _dragBoxFromMouseDown = Rectangle.Empty;

                if (listBox.SelectedValue != null)
                {
                    if (listBox.SelectedItem.GetType() == typeof(DataRowView))
                    {
                        if (listBox.SelectedValue.GetType() == typeof(VariableProfile))
                        {
                            labelTag.CurrentObject = listBox.SelectedValue;
                            label.Text = ((VariableProfile)listBox.SelectedValue).VariableName;
                        }
                        else if (((ListBox)sender).SelectedValue.GetType() == typeof(TimeTotalVariableReference))
                        {
                            labelTag.CurrentObject = listBox.SelectedValue;
                            label.Text = ((TimeTotalVariableReference)listBox.SelectedValue).TimeTotalVariableProfile.VariableName;
                        }
                        else if (((ListBox)sender).SelectedValue.GetType() == typeof(VersionProfile))
                        {
                            labelTag.CurrentObject = listBox.SelectedValue;
                            label.Text = ((VersionProfile)listBox.SelectedValue).Description;
                        }
                        else if (((ListBox)sender).SelectedValue.GetType() == typeof(StatusProfile))
                        {
                            labelTag.CurrentObject = listBox.SelectedValue;
                            label.Text = ((StatusProfile)listBox.SelectedValue).Description;
                        }
                    }
                }

                listBox.Visible = false;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}
		//Begin TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes

		protected void BaseToolList_MouseLeave(object sender, EventArgs e)
		{
			ListBox listBox;

			try
			{
				listBox = (ListBox)sender;
				listBox.Visible = false;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		//End TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes
		#endregion

		#region toolLabel Events
		protected void BaseToolLabel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            Size dragSize;

            try
            {
                dragSize = SystemInformation.DragSize;
                _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		protected void BaseToolLabel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            Label label;
            LabelTag labelTag;
            object currObject;

            try
            {
                label = (Label)sender;
                labelTag = (LabelTag)label.Tag;

                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {
                        labelTag.ListBox.Visible = false;
                        currObject = labelTag.CurrentObject;

                        if (labelTag.CurrentObject != null)
                        {
                            if (currObject.GetType() == typeof(VariableProfile))
                            {
								DoDragDrop(new FilterDragObject(new DataQueryPlanVariableOperand(_filterDef, (VariableProfile)currObject)), DragDropEffects.Move);
								Image_EndDrag();
							}
							else if (currObject.GetType() == typeof(TimeTotalVariableReference))
							{
								DoDragDrop(new FilterDragObject(new DataQueryTimeTotalVariableOperand(_filterDef, (TimeTotalVariableReference)currObject)), DragDropEffects.Move);
								Image_EndDrag();
							}
							else if (currObject.GetType() == typeof(VersionProfile))
							{
								DoDragDrop(new FilterDragObject(currObject), DragDropEffects.Move);
								Image_EndDrag();
							}
							else if (currObject.GetType() == typeof(StatusProfile))
							{
								DoDragDrop(new FilterDragObject(new DataQueryStatusOperand(_filterDef, (eStoreStatus)((StatusProfile)currObject).Key)), DragDropEffects.Move);
								Image_EndDrag();
							}
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		protected void BaseToolLabel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            _dragBoxFromMouseDown = Rectangle.Empty;
		}

		protected void BaseToolLabel_Click(object sender, System.EventArgs e)
		{
            Label label;
            LabelTag labelTag;

            try
            {
                label = (Label)sender;
                labelTag = (LabelTag)label.Tag;

                if (labelTag.ListBox.Visible)
                {
                    labelTag.ListBox.Visible = false;
                }
                else
                {
					//Begin TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes
					//labelTag.ListBox.Location = ((Control)sender).PointToClient(label.PointToScreen(new Point(0, label.Height)));
					labelTag.ListBox.Location = labelTag.ListBox.Parent.PointToClient(label.PointToScreen(new Point(-1, -1)));
					//End TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes
					labelTag.ListBox.Width = label.Width;
                    labelTag.ListBox.BringToFront();
                    labelTag.ListBox.Visible = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}
		#endregion

		#region panel Events
		protected void BasePanel_Click(object sender, EventArgs e)
		{
            try
            {
                ((PanelTag)((Panel)sender).Tag).LastOperandClicked = null;
                ClearSelectedOperands((Panel)sender);
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		public void BasePanel_DragEnter(object sender, DragEventArgs e)
		{
            FilterDragObject filterDragObject;
            TreeNodeClipboardList cbList;
            // Begin TT#13 - JSmith - Product Characteristics cannot drag/drop in Search Window when using Product Characteristics
            ProductCharacteristicClipboardList pccbList;
            // End TT#13

            try
            {
				Image_DragEnter(sender, e);
				if (e.Data.GetDataPresent(typeof(FilterDragObject)))
                {
                    filterDragObject = (FilterDragObject)e.Data.GetData(typeof(FilterDragObject));
                    foreach (Type allowType in ((PanelTag)((Panel)sender).Tag).AllowedDropTypes)
                    {
                        if (filterDragObject.DragObject.GetType().IsSubclassOf(allowType) || filterDragObject.DragObject.GetType() == allowType)
                        {
                            if (!filterDragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)) ||
                                (filterDragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)) && ((QueryOperand)filterDragObject.DragObject).isMainOperand))
                            {
                                ClearSelectedOperands((Panel)sender);
                                e.Effect = e.AllowedEffect;
                                return;
                            }
                        }
                    }
                }
                else
                {
					foreach (Type allowType in ((PanelTag)((Panel)sender).Tag).AllowedDropTypes)
					{
						if (e.Data.GetDataPresent(allowType))
						{
                            if (allowType == typeof(TreeNodeClipboardList))
							{
                                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                                //foreach (eClipboardDataType clipboardType in ((PanelTag)((Panel)sender).Tag).AllowedClipboardProfileTypes)
                                foreach (eProfileType clipboardType in ((PanelTag)((Panel)sender).Tag).AllowedClipboardProfileTypes)
								{
                                    if (cbList.ClipboardDataType == clipboardType)
									{
										ClearSelectedOperands((Panel)sender);
										e.Effect = e.AllowedEffect;
										return;
									}
								}
							}
                            // Begin TT#13 - JSmith - Product Characteristics cannot drag/drop in Search Window when using Product Characteristics
                            else if (allowType == typeof(ProductCharacteristicClipboardList))
                            {
                                pccbList = (ProductCharacteristicClipboardList)e.Data.GetData(typeof(ProductCharacteristicClipboardList));
                                foreach (eProfileType clipboardType in ((PanelTag)((Panel)sender).Tag).AllowedClipboardProfileTypes)
                                {
                                    if (pccbList.ClipboardDataType == clipboardType)
                                    {
                                        ClearSelectedOperands((Panel)sender);
                                        e.Effect = e.AllowedEffect;
                                        return;
                                    }
                                }
                            }
                            // End TT#13
                            else
							{
								ClearSelectedOperands((Panel)sender);
								e.Effect = e.AllowedEffect;
								return;
							}
						}
					}
                }

                e.Effect = DragDropEffects.None;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		protected void BasePanel_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		public void BasePanel_DragDrop(object sender, DragEventArgs e)
		{
            QueryOperand operand;
            MIDStoreNode storeNode;
            AttrQueryAttributeMainOperand attrMainOperand;
            AttrQueryStoreMainOperand AttrQueryStoreMainOperand;
			ProdCharQueryCharacteristicMainOperand prodCharQueryCharacteristicMainOperand;
            FilterDragObject dragObject;
            TreeNodeClipboardList cbList = null;
            // Begin TT#13 - JSmith - Product Characteristics cannot drag/drop in Search Window when using Product Characteristics
            ProductCharacteristicClipboardList pccbList;
            // End TT#13

            try
            {
                HighlightLineLastOperand((Panel)sender, e);

                if (e.Data.GetDataPresent(typeof(FilterDragObject)))
                {
                    dragObject = (FilterDragObject)e.Data.GetData(typeof(FilterDragObject));
                    if (dragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)))
                    {
                        operand = (QueryOperand)dragObject.DragObject;
                        AddOperand((Panel)sender, operand);
						ClearSelectedOperands((Panel)sender);
						RedrawOperands((Panel)sender);
						Image_EndDrag();
						return;
					}
					Image_EndDrag();
				}
                else if (e.Data.GetDataPresent(typeof(MIDStoreNode)))
                {
                    storeNode = (MIDStoreNode)e.Data.GetData(typeof(MIDStoreNode));
					//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
					//switch (storeNode.StoreNodeType)
					switch (storeNode.NodeProfileType)
					//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
					{
						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//case eStoreNodeType.group:
						case eProfileType.StoreGroup:
						//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
							attrMainOperand = new AttrQueryAttributeMainOperand(FilterDef, storeNode.GroupRID);
                            attrMainOperand.AddAllSets();
                            AddOperand((Panel)sender, attrMainOperand);
                            RedrawOperands((Panel)sender);
                            break;

						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//case eStoreNodeType.groupLevel:
						case eProfileType.StoreGroupLevel:
						//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                            attrMainOperand = new AttrQueryAttributeMainOperand(FilterDef, storeNode.GroupRID);
                            attrMainOperand.AddSet(storeNode.Profile.Key);
                            AddOperand((Panel)sender, attrMainOperand);
                            RedrawOperands((Panel)sender);
                            break;

						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//case eStoreNodeType.store:
						case eProfileType.Store:
						//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                            AttrQueryStoreMainOperand = new AttrQueryStoreMainOperand(FilterDef);
							AttrQueryStoreMainOperand.AddStore(storeNode.Profile.Key);
                            AddOperand((Panel)sender, AttrQueryStoreMainOperand);
                            RedrawOperands((Panel)sender);
                            break;
                    }
                }
                else if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    switch (cbList.ClipboardDataType)
					{
                        // Begin TT#13 - JSmith - Product Characteristics cannot drag/drop in Search Window when using Product Characteristics
                        //case eProfileType.ProductCharacteristic:
                        //    prodCharQueryCharacteristicMainOperand = new ProdCharQueryCharacteristicMainOperand(FilterDef, cbList.ClipboardProfile.Key);
                        //    prodCharQueryCharacteristicMainOperand.AddValue(cbList.ClipboardProfile.Key);
                        //    AddOperand((Panel)sender, prodCharQueryCharacteristicMainOperand);
                        //    RedrawOperands((Panel)sender);
                        //    break;
                        // End TT#13
						case eProfileType.StoreGroupListView:
						case eProfileType.StoreGroupLevelListView:
						//Begin Track #6242 - JScott - When creating a filter cannot drag/drop a store attribute on the attribute tab
						case eProfileType.StoreGroup:
						case eProfileType.StoreGroupLevel:
						//End Track #6242 - JScott - When creating a filter cannot drag/drop a store attribute on the attribute tab
						case eProfileType.Store:
                            storeNode = (MIDStoreNode)cbList.ClipboardProfile.Node;
							//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
							//switch (storeNode.StoreNodeType)
							switch (storeNode.NodeProfileType)
							//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
							{
								//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
								//case eStoreNodeType.group:
								case eProfileType.StoreGroupListView:
								//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
								//Begin Track #6242 - JScott - When creating a filter cannot drag/drop a store attribute on the attribute tab
								case eProfileType.StoreGroup:
								//End Track #6242 - JScott - When creating a filter cannot drag/drop a store attribute on the attribute tab
									attrMainOperand = new AttrQueryAttributeMainOperand(FilterDef, storeNode.GroupRID);
									attrMainOperand.AddAllSets();
									AddOperand((Panel)sender, attrMainOperand);
									RedrawOperands((Panel)sender);
									break;

								//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
								//case eStoreNodeType.groupLevel:
                                case eProfileType.StoreGroupLevelListView:
								//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
								//Begin Track #6242 - JScott - When creating a filter cannot drag/drop a store attribute on the attribute tab
								case eProfileType.StoreGroupLevel:
								//End Track #6242 - JScott - When creating a filter cannot drag/drop a store attribute on the attribute tab
									attrMainOperand = new AttrQueryAttributeMainOperand(FilterDef, storeNode.GroupRID);
									attrMainOperand.AddSet(storeNode.Profile.Key);
									AddOperand((Panel)sender, attrMainOperand);
									RedrawOperands((Panel)sender);
									break;

								//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
								//case eStoreNodeType.store:
								case eProfileType.Store:
								//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
									AttrQueryStoreMainOperand = new AttrQueryStoreMainOperand(FilterDef);
									AttrQueryStoreMainOperand.AddStore(storeNode.Profile.Key);
									AddOperand((Panel)sender, AttrQueryStoreMainOperand);
									RedrawOperands((Panel)sender);
									break;
							}
							break;
						
					}
				}
                // Begin TT#13 - JSmith - Product Characteristics cannot drag/drop in Search Window when using Product Characteristics
                else if (e.Data.GetDataPresent(typeof(ProductCharacteristicClipboardList)))
                {
                    pccbList = (ProductCharacteristicClipboardList)e.Data.GetData(typeof(ProductCharacteristicClipboardList));
                    switch (pccbList.ClipboardDataType)
                    {
                        case eProfileType.ProductCharacteristicValue:
                            foreach (ProductCharacteristicClipboardProfile pccp in pccbList.ClipboardItems)
                            {
                                prodCharQueryCharacteristicMainOperand = new ProdCharQueryCharacteristicMainOperand(FilterDef, pccp.ProductCharGroupKey);
                                prodCharQueryCharacteristicMainOperand.AddValue(pccp.Key);
                                AddOperand((Panel)sender, prodCharQueryCharacteristicMainOperand);
                                RedrawOperands((Panel)sender);
                            }
                            break;
                    }
                }
                // End TT#13
			}
			catch (Exception exc)
            {
                HandleException(exc);
            }
		}

		public void BasePanel_DragLeave(object sender, EventArgs e)
		{
            try
            {
                ClearSelectedOperands((Panel)sender);
				Image_DragLeave(sender, e);
			}
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}
		#endregion

		#region LiteralEditTextBox Events
		protected void BaseTxtLiteralEdit_Leave(object sender, System.EventArgs e)
		{
			try
			{
				LiteralEditTextBox.Enabled = false;
				LiteralEditTextBox.Visible = false;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		protected void BaseTxtLiteralEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				if (e.KeyChar == 13 || e.KeyChar == 9)
				{
					((DataQueryLiteralOperand)LiteralEditTextBox.Tag).LiteralValue = Convert.ToDouble(LiteralEditTextBox.Text, CultureInfo.CurrentUICulture);
					CurrentPanelClearSelectedOperands();
					CurrentPanelRedrawOperands();
					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
					e.Handled = true;
				}
				else if (e.KeyChar == 27)
				{
					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
					e.Handled = true;
				}
				else if (Char.IsNumber(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
				{
				}
				else
				{
					e.Handled = true;
				}
			}
			catch (FormatException)
			{
			}
			catch (InvalidCastException)
			{
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		#endregion

		#region GradeEditTextBox Events
		protected void BaseTxtGradeEdit_Leave(object sender, System.EventArgs e)
		{
			try
			{
				GradeEditTextBox.Enabled = false;
				GradeEditTextBox.Visible = false;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		protected void BaseTxtGradeEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				if (e.KeyChar == 13 || e.KeyChar == 9)
				{
					((DataQueryGradeOperand)GradeEditTextBox.Tag).GradeValue = GradeEditTextBox.Text;
					CurrentPanelClearSelectedOperands();
					CurrentPanelRedrawOperands();
					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
					e.Handled = true;
				}
				else if (e.KeyChar == 27)
				{
					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
					e.Handled = true;
				}
				else if (Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
				{
				}
				else
				{
					e.Handled = true;
				}
			}
			catch (FormatException)
			{
			}
			catch (InvalidCastException)
			{
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		#endregion

		#region Public Methods


        public void AddOperand(Panel aPanel, QueryOperand aOperand)
        {
            int startIdx;

            try
            {
                if (((PanelTag)aPanel.Tag).SelectedOperandList.Count == 0)
                {
                    aOperand.Label.Parent = aPanel;
                    ((PanelTag)aPanel.Tag).OperandArray.Add(aOperand);
                }
                else
                {
                    startIdx = (int)((QueryOperand)((PanelTag)aPanel.Tag).SelectedOperandList[0]).Label.Tag;

                    DeleteSelectedOperands(aPanel);

                    aOperand.Label.Parent = aPanel;

                    ((PanelTag)aPanel.Tag).OperandArray.Insert(startIdx, aOperand);
                }

                ChangePending = true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void DeleteSelectedOperands(Panel aPanel)
        {
            try
            {
                if (((PanelTag)aPanel.Tag).SelectedOperandList.Count > 0)
                {
                    foreach (QueryOperand operand in ((PanelTag)aPanel.Tag).SelectedOperandList)
                    {
                        operand.OnDelete();
                        operand.Label.Parent = null;
                    }

                    ((PanelTag)aPanel.Tag).OperandArray.RemoveRange((int)((QueryOperand)((PanelTag)aPanel.Tag).SelectedOperandList[0]).Label.Tag, ((PanelTag)aPanel.Tag).SelectedOperandList.Count);

                    ClearSelectedOperands(aPanel);

                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ClearSelectedOperands(Panel aPanel)
        {
            try
            {
                foreach (QueryOperand operand in ((PanelTag)aPanel.Tag).SelectedOperandList)
                {
                    ((BasicQueryLabel)operand.Label).Unhighlight();
                }

                ((PanelTag)aPanel.Tag).SelectedOperandList.Clear();

                ChangePending = true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void RedrawOperands(Panel aPanel)
        {
            ArrayList operandRedrawList;
            ArrayList newOperandArray;
            QueryOperand spacer;
            int line;
            int index;
            int left;
            int top;
            int spacerHeight;
            int spacerWidth;

            try
            {
                operandRedrawList = new ArrayList();
                newOperandArray = new ArrayList();

                line = 0;
                index = 0;
                top = 0;
                left = 0;
                ((PanelTag)aPanel.Tag).LineOperandList.Clear();

				spacer = (QueryOperand)System.Activator.CreateInstance(((PanelTag)aPanel.Tag).SpacerOperandType, new object[] { _filterDef });
				//Begin Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
				//spacerHeight = spacer.Label.Height;
				spacerHeight = spacer.Label.Height + 2;
				//End Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
				spacerWidth = spacer.Label.Width;

                ((PanelTag)aPanel.Tag).LineOperandList.Add(new LineInfo(0, spacerHeight - 1));

                foreach (QueryOperand operand in ((PanelTag)aPanel.Tag).OperandArray)
                {
                    operand.Label.Parent = null;
                }

                foreach (QueryOperand operand in ((PanelTag)aPanel.Tag).OperandArray)
                {
                    if (operand.GetType() != ((PanelTag)aPanel.Tag).SpacerOperandType)
                    {
                        operandRedrawList.Clear();
                        operand.OnRedraw(operandRedrawList);

                        foreach (QueryOperand newOperand in operandRedrawList)
                        {
                            if (((LineInfo)((PanelTag)aPanel.Tag).LineOperandList[line]).LineOperands.Count > 1 && newOperand.Label.Width + spacerWidth + left > aPanel.Width)
                            {
                                line++;
								//Begin Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
								//top += newOperand.Label.Height;
								top += spacerHeight;
								//End Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
								left = 0;

                                ((PanelTag)aPanel.Tag).LineOperandList.Add(new LineInfo(top, top + spacerHeight - 1));
                            }

                            if (newOperand.isMainOperand)
                            {
								spacer = (QueryOperand)System.Activator.CreateInstance(((PanelTag)aPanel.Tag).SpacerOperandType, new object[] { _filterDef });
                                spacer.Label.Parent = aPanel;
                                spacer.Label.Tag = index;
                                spacer.Label.Left = left;
                                spacer.Label.Top = top;
                                left += spacer.Label.Width;
                                index++;

                                newOperandArray.Add(spacer);
                                ((LineInfo)((PanelTag)aPanel.Tag).LineOperandList[line]).LineOperands.Add(spacer);
                            }

                            newOperand.Label.Parent = aPanel;
                            newOperand.Label.Tag = index;
                            newOperand.Label.Left = left;
                            newOperand.Label.Top = top;
                            left += newOperand.Label.Width;
                            index++;

                            newOperandArray.Add(newOperand);
                            ((LineInfo)((PanelTag)aPanel.Tag).LineOperandList[line]).LineOperands.Add(newOperand);
                        }
                    }
                }

                ((PanelTag)aPanel.Tag).OperandArray.Clear();
                ((PanelTag)aPanel.Tag).OperandArray.AddRange(newOperandArray);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void CurrentPanelAddOperand(QueryOperand aOperand)
        {
            try
            {
                AddOperand(CurrentPanel, aOperand);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		protected void CurrentPanelDeleteSelectedOperands()
        {
            try
            {
                DeleteSelectedOperands(CurrentPanel);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void CurrentPanelClearSelectedOperands()
        {
            try
            {
                ClearSelectedOperands(CurrentPanel);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void CurrentPanelRedrawOperands()
        {
            try
            {
                RedrawOperands(CurrentPanel);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void CurrentPanelSelectOperandRange(int aStartIdx, int aEndIdx)
        {
            int i;
            int startIdx;
            int endIdx;

            try
            {
                if (aStartIdx <= aEndIdx)
                {
                    startIdx = aStartIdx;
                    endIdx = aEndIdx;
                }
                else
                {
                    startIdx = aEndIdx;
                    endIdx = aStartIdx;
                }

                ((PanelTag)CurrentPanel.Tag).SelectedOperandList.Clear();

                for (i = 0; i < endIdx; i++)
                {
                    ((BasicQueryLabel)((QueryOperand)((PanelTag)CurrentPanel.Tag).OperandArray[i]).Label).Unhighlight();
                }

                for (i = startIdx; i <= endIdx; i++)
                {
                    ((BasicQueryLabel)((QueryOperand)((PanelTag)CurrentPanel.Tag).OperandArray[i]).Label).Highlight();
                    ((PanelTag)CurrentPanel.Tag).SelectedOperandList.Add(((PanelTag)CurrentPanel.Tag).OperandArray[i]);
                }

                for (i = endIdx + 1; i < ((PanelTag)CurrentPanel.Tag).OperandArray.Count; i++)
                {
                    ((BasicQueryLabel)((QueryOperand)((PanelTag)CurrentPanel.Tag).OperandArray[i]).Label).Unhighlight();
                }

                ((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		protected void HighlightLineLastOperand(Panel aPanel, DragEventArgs e)
        {
            Point mousePoint;
            int i;
            int line;
            QueryOperand insertOperand;

            try
            {
                mousePoint = aPanel.PointToClient(new Point(e.X, e.Y));
                line = ((PanelTag)aPanel.Tag).LineOperandList.Count + 1;

                for (i = 0; i < ((PanelTag)aPanel.Tag).LineOperandList.Count; i++)
                {
                    if (mousePoint.Y <= ((LineInfo)((PanelTag)aPanel.Tag).LineOperandList[i]).LineYEnd)
                    {
                        line = i + 1;
                        break;
                    }
                }

                if (line < ((PanelTag)aPanel.Tag).LineOperandList.Count)
                {
                    insertOperand = null;
                    while (insertOperand == null && line < ((PanelTag)aPanel.Tag).LineOperandList.Count)
                    {
                        foreach (QueryOperand operand in ((LineInfo)((PanelTag)aPanel.Tag).LineOperandList[line]).LineOperands)
                        {
                            if (operand.GetType() == ((PanelTag)aPanel.Tag).SpacerOperandType)
                            {
                                insertOperand = operand;
                                break;
                            }
                        }
                        line++;
                    }
                    if (insertOperand != null)
                    {
                        ((BasicQueryLabel)insertOperand.Label).HighlightClick();
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public Label LabelCreator(FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
        {
            Type type;

            type = aQueryOperand.GetType();

            if (type == typeof(GenericQueryAndOperand))
            {
                return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(GenericQueryOrOperand))
            {
                return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(GenericQueryLeftParenOperand))
            {
                return new GenericQueryLeftParenLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(GenericQueryRightParenOperand))
            {
                return new GenericQueryRightParenLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(AttrQueryAttributeMainOperand))
            {
                return new AttrQueryAttributeMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(AttrQueryAttributeDetailOperand))
            {
                return new AttrQueryAttributeDetailLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(AttrQueryAttributeSeparatorOperand))
            {
                return new AttrQueryAttributeMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(AttrQueryAttributeEndOperand))
            {
                return new AttrQueryAttributeMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(AttrQueryStoreMainOperand))
            {
                return new AttrQueryStoreMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(AttrQueryStoreDetailOperand))
            {
                return new AttrQueryStoreDetailLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(AttrQueryStoreSeparatorOperand))
            {
                return new AttrQueryStoreMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(AttrQueryStoreEndOperand))
            {
                return new AttrQueryStoreMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(AttrQuerySpacerOperand))
            {
                return new AttrQuerySpacerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryPlanVariableOperand))
            {
                return new DataQueryVariableSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryTimeTotalVariableOperand))
            {
                return new DataQueryVariableSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryPlanBeginOperand))
            {
                return new DataQueryVariableSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryPlanSeparatorOperand))
            {
                return new DataQueryVariableSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryPlanEndOperand))
            {
                return new DataQueryVariableSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryNodeOperand))
            {
                return new DataQueryNodeModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryVersionOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryDateRangeOperand))
            {
                return new DataQueryDateRangeLabel(this, aFilterDef, aQueryOperand, aForeColor, aText, (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB }));
            }
            else if (type == typeof(DataQueryCubeModifyerOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryStoreDetailOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryStoreTotalOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryStoreAverageOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryChainDetailOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryTimeModifyerOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryAllOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryAnyOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            //Begin Track #5111 - JScott - Add additional filter functionality
            else if (type == typeof(DataQueryJoinOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            //End Track #5111 - JScott - Add additional filter functionality
            else if (type == typeof(DataQueryAverageOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryTotalOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryCorrespondingOperand))
            {
                return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryEqualOperand))
            {
                return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryLessOperand))
            {
                return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryGreaterOperand))
            {
                return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryLessEqualOperand))
            {
                return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryGreaterEqualOperand))
            {
                return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryNotOperand))
            {
                return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryPctChangeOperand))
            {
                return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryPctOfOperand))
            {
                return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryLiteralOperand))
            {
                return new DataQueryLiteralLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryGradeOperand))
            {
                return new DataQueryGradeLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQueryStatusOperand))
            {
                return new DataQueryStatusLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
            }
            else if (type == typeof(DataQuerySpacerOperand))
            {
                return new DataQuerySpacerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
			}
			else if (type == typeof(ProdCharQueryCharacteristicMainOperand))
			{
				return new ProdCharQueryCharacteristicMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
			}
			else if (type == typeof(ProdCharQueryCharacteristicDetailOperand))
			{
				return new ProdCharQueryCharacteristicDetailLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
			}
			else if (type == typeof(ProdCharQueryCharacteristicSeparatorOperand))
			{
				return new ProdCharQueryCharacteristicMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
			}
			else if (type == typeof(ProdCharQueryCharacteristicEndOperand))
			{
				return new ProdCharQueryCharacteristicMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
			}
			else if (type == typeof(ProdCharQuerySpacerOperand))
			{
				return new ProdCharQuerySpacerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
			}
            else
            {
                throw new Exception("Invalid Operand Type in LabelCreator");
            }
        }

		#endregion Public Methods

		private void cmsLabelMenu_Opening(object sender, CancelEventArgs e)
		{

		}


		#endregion Methods

	}

	#region FilterDragObject
	/// <summary>
	/// Class that defines the FilterDragObject, which is a generic object used during drag events.
	/// </summary>

	public class FilterDragObject
	{
		//=======
		// FIELDS
		//=======

		public object DragObject;
		public string Text;

		//=============
		// CONSTRUCTORS
		//=============

		public FilterDragObject(object aDragObject)
		{
			DragObject = aDragObject;
			Text = null;
		}

		public FilterDragObject(object aDragObject, string aText)
		{
			DragObject = aDragObject;
			Text = aText;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}
	#endregion

	#region StatusProfile
	/// <summary>
	/// Class that defines the StatusProfile, that is used during the drag operation of a Status.
	/// </summary>

	public class StatusProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private string _description;

		//=============
		// CONSTRUCTORS
		//=============

		public StatusProfile(eStoreStatus aStatus, string aDescription)
			: base((int)aStatus)
		{
			_description = aDescription;
		}

		//===========
		// PROPERTIES
		//===========

		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.StoreStatus;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
		}

		//========
		// METHODS
		//========
	}
	#endregion

	#region FilterNameCombo Class
	/// <summary>
	/// Class that defines the contents of the FilterName combo box.
	/// </summary>

	public class FilterNameCombo
	{
		//=======
		// FIELDS
		//=======

		private int _filterRID;
		private int _userRID;
		private string _filterName;
		private string _displayName;
        // Begin TT#1125 - JSmith - Global/User should be consistent
        //SecurityAdmin secAdmin; //TT#827-MD -jsobek -Allocation Reviews Performance
        // End TT#1125

		//=============
		// CONSTRUCTORS
		//=============

		public FilterNameCombo(int aFilterRID)
		{
			_filterRID = aFilterRID;
		}

        // Begin TT#1125 - JSmith - Global/User should be consistent
        //public FilterNameCombo(int aFilterRID, int aUserRID, string aFilterName)
        //{
        //    _filterRID = aFilterRID;
        //    _userRID = aUserRID;
        //    _filterName = aFilterName;
        //    if (aUserRID == Include.GlobalUserRID) // Issue 3806
        //    {
        //        _displayName = _filterName;
        //    }
        //    else
        //    {
        //        _displayName = _filterName + " (User)";
        //    }
        //}
        public FilterNameCombo(int aFilterRID, int aUserRID, string aFilterName)
        {
            _filterRID = aFilterRID;
            _userRID = aUserRID;
            _filterName = aFilterName;
            if (aUserRID == Include.GlobalUserRID) // Issue 3806
            {
                _displayName = _filterName;
            }
            else
            {
                //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                //secAdmin = new SecurityAdmin();
                //_displayName = _filterName + " (" + secAdmin.GetUserName(aUserRID) + ")";
                _displayName = _filterName + " (" + UserNameStorage.GetUserName(aUserRID) + ")";
                //End TT#827-MD -jsobek -Allocation Reviews Performance
            }
        }
        // End TT#1125

		//===========
		// PROPERTIES
		//===========

		public int FilterRID
		{
			get
			{
				return _filterRID;
			}
		}

		public int UserRID
		{
			get
			{
				return _userRID;
			}
		}

		public string FilterName
		{
			get
			{
				return _filterName;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _displayName;
		}

		override public bool Equals(object obj)
		{
			if (((FilterNameCombo)obj).FilterRID == _filterRID)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		override public int GetHashCode()
		{
			return _filterRID;
		}
	}
	#endregion

	#region LineInfo Class
	/// <summary>
	/// Class that defines area for holding information about each line in the query panel.
	/// </summary>

	public class LineInfo
	{
		//=======
		// FIELDS
		//=======

		private ArrayList _lineOperands;
		private int _lineYStart;
		private int _lineYEnd;

		//=============
		// CONSTRUCTORS
		//=============

		public LineInfo()
		{
			_lineOperands = new ArrayList();
			_lineYStart = 0;
			_lineYEnd = 0;
		}

		public LineInfo(int aLineYStart)
		{
			_lineOperands = new ArrayList();
			_lineYStart = aLineYStart;
			_lineYEnd = 0;
		}

		public LineInfo(int aLineYStart, int aLineYEnd)
		{
			_lineOperands = new ArrayList();
			_lineYStart = aLineYStart;
			_lineYEnd = aLineYEnd;
		}

		//===========
		// PROPERTIES
		//===========

		public ArrayList LineOperands
		{
			get
			{
				return _lineOperands;
			}
		}

		public int LineYStart
		{
			get
			{
				return _lineYStart;
			}
			set
			{
				_lineYStart = value;
			}
		}

		public int LineYEnd
		{
			get
			{
				return _lineYEnd;
			}
			set
			{
				_lineYEnd = value;
			}
		}

		//========
		// METHODS
		//========
	}
	#endregion

	#region PanelTag Class
	/// <summary>
	/// Class that defines the tag information stored in the filter query panels.
	/// </summary>

	public class PanelTag
	{
		//=======
		// FIELDS
		//=======

		private Type _spacerOperandType;
		private Control _defaultControl;
		private ArrayList _operandArray;
		private ArrayList _selectedOperandList;
		private ArrayList _lineOperandList;
		private ArrayList _allowedDropTypes;
		private QueryOperand _lastOperandClicked;
		private ArrayList _allowedClipboardProfileTypes;

		//=============
		// CONSTRUCTORS
		//=============

		public PanelTag(Type aSpacerOperandType, Control aDefaultControl, ArrayList aOperandArray)
		{
			try
			{
				_spacerOperandType = aSpacerOperandType;
				_defaultControl = aDefaultControl;
				_operandArray = aOperandArray;
				_selectedOperandList = new ArrayList();
				_lineOperandList = new ArrayList();
				_allowedDropTypes = new ArrayList();
				_allowedClipboardProfileTypes = new ArrayList();
				_lastOperandClicked = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public Type SpacerOperandType
		{
			get
			{
				return _spacerOperandType;
			}
		}

		public Control DefaultControl
		{
			get
			{
				return _defaultControl;
			}
		}

		public ArrayList OperandArray
		{
			get
			{
				return _operandArray;
			}
			set
			{
				_operandArray = value;
			}
		}

		public ArrayList SelectedOperandList
		{
			get
			{
				return _selectedOperandList;
			}
		}

		public ArrayList LineOperandList
		{
			get
			{
				return _lineOperandList;
			}
		}

		public ArrayList AllowedDropTypes
		{
			get
			{
				return _allowedDropTypes;
			}
		}

		public ArrayList AllowedClipboardProfileTypes
		{
			get
			{
				return _allowedClipboardProfileTypes;
			}
		}

		public QueryOperand LastOperandClicked
		{
			get
			{
				return _lastOperandClicked;
			}
			set
			{
				_lastOperandClicked = value;
			}
		}

		//========
		// METHODS
		//========

		public void Clear()
		{
			try
			{
				_selectedOperandList.Clear();
				_lineOperandList.Clear();
				_lastOperandClicked = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	#endregion

	#region LabelTag Class
	/// <summary>
	/// Class that defines the tag information stored in the filter query panels.
	/// </summary>

	public class LabelTag
	{
		//=======
		// FIELDS
		//=======

		private ListBox _listBox;
		private object _currObject;

		//=============
		// CONSTRUCTORS
		//=============

		public LabelTag(ListBox aListBox)
		{
			_listBox = aListBox;
			_currObject = null;
		}

		//===========
		// PROPERTIES
		//===========

		public ListBox ListBox
		{
			get
			{
				return _listBox;
			}
		}

		public Object CurrentObject
		{
			get
			{
				return _currObject;
			}
			set
			{
				_currObject = value;
			}
		}

		//========
		// METHODS
		//========
	}
	#endregion

	#region MultiSelect Class
	/// <summary>
	/// Class that identifies a multi-select operation in a drag-drop function.
	/// </summary>

	public class MultiSelect
	{
	}
	#endregion

	#region QueryOperand Class
	/// <summary>
	/// Class that defines the base QueryOperand Class.
	/// </summary>

	public class BasicQueryLabel : Label
	{
		//=======
		// FIELDS
		//=======

		protected MIDFilterFormBase _form;
		protected FilterDefinition _filterDef;
		protected QueryOperand _queryOperand;
		private Color _myDefaultBackColor;
		private Color _myDefaultForeColor;
		private Rectangle _dragBoxFromMouseDown;

		//=============
		// CONSTRUCTORS
		//=============

		public BasicQueryLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base()
		{
			_form = aForm;
			_filterDef = aFilterDef;
			_queryOperand = aQueryOperand;

			MyDefaultBackColor = Color.White;
			MyDefaultForeColor = aForeColor;

			Text = aText;

			Initialize();
		}

		public BasicQueryLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor)
			: base()
		{
			_form = aForm;
			_filterDef = aFilterDef;
			_queryOperand = aQueryOperand;

			MyDefaultBackColor = Color.White;
			MyDefaultForeColor = aForeColor;

			Initialize();
		}

		//===========
		// PROPERTIES
		//===========

		private void Initialize()
		{
			AutoSize = true;
			BorderStyle = BorderStyle.None;
			AllowDrop = true;
			TextAlign = ContentAlignment.MiddleRight;
			Font = new Font("Arial", 9);
			DragEnter += new DragEventHandler(DragEnterHandler);
			DragDrop += new DragEventHandler(DragDropHandler);
			DragOver += new DragEventHandler(DragOverHandler);
			DragLeave += new EventHandler(DragLeaveHandler);
			MouseDown += new MouseEventHandler(MouseDownHandler);
			MouseMove += new MouseEventHandler(MouseMoveHandler);
			MouseUp += new MouseEventHandler(MouseUpHandler);
			DoubleClick += new EventHandler(DoubleClickHandler);
			GiveFeedback += new GiveFeedbackEventHandler(GiveFeedbackHandler);
		}

		public Color MyDefaultBackColor
		{
			get
			{
				return _myDefaultBackColor;
			}
			set
			{
				_myDefaultBackColor = value;
				BackColor = _myDefaultBackColor;
			}
		}

		public Color MyDefaultForeColor
		{
			get
			{
				return _myDefaultForeColor;
			}
			set
			{
				_myDefaultForeColor = value;
				ForeColor = _myDefaultForeColor;
			}
		}

		//========
		// METHODS
		//========

		public void Highlight()
		{
			try
			{
				BackColor = Color.Blue;
				ForeColor = Color.White;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void Unhighlight()
		{
			try
			{
				BackColor = _myDefaultBackColor;
				ForeColor = _myDefaultForeColor;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void HighlightClick()
		{
			try
			{
				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void HighlightDragEnter(out int aStartIdx, out int aEndIdx)
		{
			try
			{
				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				aStartIdx = (int)Tag;
				aEndIdx = aStartIdx;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void StartEdit(char aKeyPressed)
		{
		}

		public void MouseDownHandler(object sender, MouseEventArgs e)
		{
			Size dragSize;

			try
			{
				if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				{
					dragSize = SystemInformation.DragSize;
					_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
				}
			}
			catch (Exception exc)
			{
				_form.HandleException(exc);
			}
		}

		public void MouseMoveHandler(object sender, MouseEventArgs e)
		{
			int startIdx;
			int endIdx;

			try
			{
				if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				{
					if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
					{
						HighlightDragEnter(out startIdx, out endIdx);
						_form.MultiSelectStartIdx = startIdx;
						_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
						DoDragDrop(new MultiSelect(), DragDropEffects.Move);
						_form.Image_EndDrag();
					}
				}
			}
			catch (Exception exc)
			{
				_form.HandleException(exc);
			}
		}

		virtual public void MouseUpHandler(object sender, MouseEventArgs e)
		{
			try
			{
				_dragBoxFromMouseDown = Rectangle.Empty;
				if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				{
					HighlightClick();
				}
			}
			catch (Exception exc)
			{
				_form.HandleException(exc);
			}
		}

		virtual public void DoubleClickHandler(object sender, EventArgs e)
		{
		}

		public void GiveFeedbackHandler(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
		{
			e.UseDefaultCursors = false;
		}

		virtual public void DragEnterHandler(object sender, DragEventArgs e)
		{
			int startIdx;
			int endIdx;
			FilterDragObject filterDragObject;
            //MIDHierarchyNode treeNode;
            TreeNodeClipboardList cbList = null;

			try
			{
				if (e.Data.GetDataPresent(typeof(MultiSelect)))
				{
					HighlightDragEnter(out startIdx, out endIdx);
					_form.CurrentPanelSelectOperandRange(_form.MultiSelectStartIdx, endIdx);
					e.Effect = e.AllowedEffect;
					return;
				}
				else
				{
					if (e.Data.GetDataPresent(typeof(FilterDragObject)))
					{
						//Begin Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
						//_form.Image_DragEnter(sender, e);
						//End Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
						filterDragObject = (FilterDragObject)e.Data.GetData(typeof(FilterDragObject));

						if (filterDragObject.DragObject.GetType() == typeof(DataQueryDateRangeOperand) ||
							filterDragObject.DragObject.GetType().IsSubclassOf(typeof(DataQueryCubeModifyerOperand)) ||
							filterDragObject.DragObject.GetType().IsSubclassOf(typeof(DataQueryTimeModifyerOperand)) ||
							filterDragObject.DragObject.GetType() == typeof(VersionProfile))
						{
							if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
							{
								HighlightDragEnter(out startIdx, out endIdx);
								_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
								e.Effect = e.AllowedEffect;
								return;
							}
						}
						else
						{
							foreach (Type allowType in ((PanelTag)_form.CurrentPanel.Tag).AllowedDropTypes)
							{
								if (filterDragObject.DragObject.GetType().IsSubclassOf(allowType) || filterDragObject.DragObject.GetType() == allowType)
								{
									if (!filterDragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)) ||
										(filterDragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)) && ((QueryOperand)filterDragObject.DragObject).isMainOperand))
									{
										HighlightDragEnter(out startIdx, out endIdx);
										_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
										e.Effect = e.AllowedEffect;
										return;
									}
								}
							}
						}
					}
                    else if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
					{
						//Begin Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
						//_form.Image_DragEnter(sender, e);
						//End Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
						{
                            cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                            //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                            if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
							{
                                //HierarchyClipboardData hcd = (HierarchyClipboardData)cbp.ClipboardData;
                                ////treeNode = (MIDHierarchyNode)e.Data.GetData(typeof(MIDHierarchyNode));
                                ////if (treeNode.NodeType == eHierarchyNodeType.TreeNode)
                                //if (hcd.NodeType == eProfileType.HierarchyNode)
                                //{
									HighlightDragEnter(out startIdx, out endIdx);
									_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
									e.Effect = e.AllowedEffect;
									return;
                                //}
							}
						}
					}
					else
					{
						foreach (Type allowType in ((PanelTag)_form.CurrentPanel.Tag).AllowedDropTypes)
						{
							if (e.Data.GetDataPresent(allowType))
							{
								HighlightDragEnter(out startIdx, out endIdx);
								_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
								e.Effect = e.AllowedEffect;
								return;
							}
						}
					}
				}

				e.Effect = DragDropEffects.None;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void DragDropHandler(object sender, DragEventArgs e)
		{
			int startIdx;
			int endIdx;
			QueryOperand operand;
            //MIDHierarchyNode treeNode;
			DataQueryVariableOperand variableOperand;
			MIDStoreNode storeNode;
			AttrQueryAttributeMainOperand attrMainOperand;
			AttrQueryStoreMainOperand AttrQueryStoreMainOperand;
			FilterDragObject dragObject;
            TreeNodeClipboardList cbList = null;

			try
			{
				_form.Image_EndDrag();
				if (e.Data.GetDataPresent(typeof(MultiSelect)))
				{
					HighlightDragEnter(out startIdx, out endIdx);
					_form.MultiSelectEndIdx = endIdx;
					_form.CurrentPanelSelectOperandRange(_form.MultiSelectStartIdx, _form.MultiSelectEndIdx);
				}
				else if (e.Data.GetDataPresent(typeof(FilterDragObject)))
				{
					dragObject = (FilterDragObject)e.Data.GetData(typeof(FilterDragObject));
					if (dragObject.DragObject.GetType() == typeof(DataQueryDateRangeOperand))
					{
						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
						{
							variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
							variableOperand.DateRangeProfile = new DateRangeProfile(Include.UndefinedCalendarDateRange);
							variableOperand.DateRangeProfile.DisplayDate = "<DateRange>";
							_form.CurrentPanelClearSelectedOperands();
							_form.CurrentPanelRedrawOperands();
						}
					}
					else if (dragObject.DragObject.GetType().IsSubclassOf(typeof(DataQueryCubeModifyerOperand)))
					{
						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
						{
							variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
							variableOperand.CubeModifyer = ((DataQueryCubeModifyerOperand)dragObject.DragObject).CubeModifyer;
							_form.CurrentPanelClearSelectedOperands();
							_form.CurrentPanelRedrawOperands();
						}
					}
					else if (dragObject.DragObject.GetType().IsSubclassOf(typeof(DataQueryTimeModifyerOperand)))
					{
						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
						{
							variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
							variableOperand.TimeModifyer = ((DataQueryTimeModifyerOperand)dragObject.DragObject).TimeModifyer;
							_form.CurrentPanelClearSelectedOperands();
							_form.CurrentPanelRedrawOperands();
						}
					}
					else if (dragObject.DragObject.GetType() == typeof(VersionProfile))
					{
						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
						{
							variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
							variableOperand.VersionProfile = (VersionProfile)dragObject.DragObject;
							_form.CurrentPanelClearSelectedOperands();
							_form.CurrentPanelRedrawOperands();
						}
					}
					else if (dragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)))
					{
						operand = (QueryOperand)dragObject.DragObject;
						_form.CurrentPanelAddOperand(operand);
						_form.CurrentPanelClearSelectedOperands();
						_form.CurrentPanelRedrawOperands();
						return;
					}
				}
                else if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
					{
                        //HierarchyClipboardData hcd = (HierarchyClipboardData)cbp.ClipboardData;
                        ////treeNode = (MIDHierarchyNode)e.Data.GetData(typeof(MIDHierarchyNode));
                        ////if (treeNode.NodeType == eHierarchyNodeType.TreeNode)
                        //if (hcd.NodeType == eProfileType.HierarchyNode)
                        //{
							if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
							{
								variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
								//variableOperand.NodeProfile = _form.SAB.HierarchyServerSession.GetNodeData(treeNode.NodeRID);
                                variableOperand.NodeProfile = _form.SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, false, true);  //TT#1916 - MD - Correction for color dragdrop  - rbeck 
								_form.CurrentPanelClearSelectedOperands();
								_form.CurrentPanelRedrawOperands();
							}
                        //}
					}
				}
				else if (e.Data.GetDataPresent(typeof(MIDStoreNode)))
				{
					storeNode = (MIDStoreNode)e.Data.GetData(typeof(MIDStoreNode));
					//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
					//switch (storeNode.StoreNodeType)
					switch (storeNode.NodeProfileType)
					//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
					{
						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//case eStoreNodeType.group:
						case eProfileType.StoreGroup:
						//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
							attrMainOperand = new AttrQueryAttributeMainOperand(_filterDef, storeNode.GroupRID);
							attrMainOperand.AddAllSets();
							_form.CurrentPanelAddOperand(attrMainOperand);
							_form.CurrentPanelRedrawOperands();
							break;

						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//case eStoreNodeType.groupLevel:
						case eProfileType.StoreGroupLevel:
						//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
							if (_queryOperand.GetType().IsSubclassOf(typeof(AttrQueryAttributeOperand)) && storeNode.GroupRID == ((AttrQueryAttributeOperand)_queryOperand).AttributeRID)
							{
								attrMainOperand = ((AttrQueryAttributeOperand)_queryOperand).AttrMainOperand;
								attrMainOperand.AddSet(storeNode.Profile.Key);
								_form.CurrentPanelClearSelectedOperands();
								_form.CurrentPanelRedrawOperands();
							}
							else
							{
								attrMainOperand = new AttrQueryAttributeMainOperand(_filterDef, storeNode.GroupRID);
								attrMainOperand.AddSet(storeNode.Profile.Key);
								_form.CurrentPanelAddOperand(attrMainOperand);
								_form.CurrentPanelRedrawOperands();
							}
							break;

						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//case eStoreNodeType.store:
						case eProfileType.Store:
						//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
							if (_queryOperand.GetType().IsSubclassOf(typeof(AttrQueryStoreOperand)))
							{
								AttrQueryStoreMainOperand = ((AttrQueryStoreOperand)_queryOperand).StoreMainOperand;
								AttrQueryStoreMainOperand.AddStore(storeNode.Profile.Key);
								_form.CurrentPanelClearSelectedOperands();
								_form.CurrentPanelRedrawOperands();
							}
							else
							{
								AttrQueryStoreMainOperand = new AttrQueryStoreMainOperand(_filterDef);
								AttrQueryStoreMainOperand.AddStore(storeNode.Profile.Key);
								_form.CurrentPanelAddOperand(AttrQueryStoreMainOperand);
								_form.CurrentPanelRedrawOperands();
							}
							break;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void DragOverHandler(object sender, DragEventArgs e)
		{
			try
			{
				//Begin Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
				//_form.Image_DragOver(sender, e);
				//End Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void DragLeaveHandler(object sender, EventArgs e)
		{
			try
			{
				_form.CurrentPanelClearSelectedOperands();
				//Begin Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
				//_form.Image_DragLeave(sender, e);
				//End Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
				this.Refresh();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	#endregion

	#region GenericQueryOperand Classes
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	// Generic Query labels
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================

	/// <summary>
	/// Class that defines a Left Parenthesis label.
	/// </summary>

	public class GenericQueryLeftParenLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public GenericQueryLeftParenLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void HighlightClick()
		{
			int i;
			int currLevel;
			int startIdx;
			int endIdx;

			try
			{
				if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked == null || (int)Tag != (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
				{
					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
					_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
				}
				else
				{
					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
					startIdx = (int)Tag;
					endIdx = startIdx;
					currLevel = 0;

					for (i = (int)Tag + 1; i < ((PanelTag)_form.CurrentPanel.Tag).OperandArray.Count; i++)
					{
						if (((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() == typeof(GenericQueryRightParenOperand))
						{
							if (currLevel == 0)
							{
								endIdx = i;
								break;
							}
							else
							{
								currLevel--;
							}
						}
						else
						{
							if (((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() == typeof(GenericQueryLeftParenOperand))
							{
								currLevel++;
							}
						}
					}

					_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines a Right Parenthesis label.
	/// </summary>

	public class GenericQueryRightParenLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public GenericQueryRightParenLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void HighlightClick()
		{
			int i;
			int currLevel;
			int startIdx;
			int endIdx;

			try
			{
				_form.CurrentPanelClearSelectedOperands();

				if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked == null || (int)Tag != (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
				{
					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
					_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
				}
				else
				{
					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
					startIdx = (int)Tag;
					endIdx = startIdx;
					currLevel = 0;

					for (i = (int)Tag - 1; i >= 0; i--)
					{
						if (((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() == typeof(GenericQueryLeftParenOperand))
						{
							if (currLevel == 0)
							{
								endIdx = i;
								break;
							}
							else
							{
								currLevel--;
							}
						}
						else
						{
							if (((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() == typeof(GenericQueryRightParenOperand))
							{
								currLevel++;
							}
						}
					}

					_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	#endregion

	#region AttrQueryOperand Classes
	/// <summary>
	/// Abstract class that defines an attribute.
	/// </summary>

	abstract public class AttrQueryAttributeLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryAttributeLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//========
		// METHODS
		//========

		override public void HighlightDragEnter(out int aStartIdx, out int aEndIdx)
		{
			try
			{
				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				HighlightAttribute(out aStartIdx, out aEndIdx);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void HighlightAttribute(out int aStartIdx, out int aEndIdx)
		{
			int i;

			try
			{
				for (i = (int)Tag; i >= 0 && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() != typeof(AttrQueryAttributeMainOperand); i--)
				{
				}

				aStartIdx = i;

				for (i = aStartIdx; i < ((PanelTag)_form.CurrentPanel.Tag).OperandArray.Count && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(AttrQueryAttributeOperand)); i++)
				{
				}

				aEndIdx = i - 1;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Abstract class that defines a store.
	/// </summary>

	abstract public class AttrQueryStoreLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryStoreLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void HighlightDragEnter(out int aStartIdx, out int aEndIdx)
		{
			try
			{
				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				HighlightStore(out aStartIdx, out aEndIdx);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void HighlightStore(out int aStartIdx, out int aEndIdx)
		{
			int i;

			try
			{
				for (i = (int)Tag; i >= 0 && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(AttrQueryStoreOperand)); i--)
				{
				}

				aStartIdx = i + 1;

				for (i = aStartIdx; i < ((PanelTag)_form.CurrentPanel.Tag).OperandArray.Count && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(AttrQueryStoreOperand)); i++)
				{
				}

				aEndIdx = i - 1;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the attribute portion of an attribute definition label.
	/// </summary>

	public class AttrQueryAttributeMainSeparatorEndLabel : AttrQueryAttributeLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryAttributeMainSeparatorEndLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void HighlightClick()
		{
			int startIdx;
			int endIdx;

			try
			{
				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				HighlightAttribute(out startIdx, out endIdx);
				_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the attribute portion of an attribute definition label.
	/// </summary>

	public class AttrQueryAttributeDetailLabel : AttrQueryAttributeLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryAttributeDetailLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines the store label portion of a store definition label.
	/// </summary>

	public class AttrQueryStoreMainSeparatorEndLabel : AttrQueryStoreLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryStoreMainSeparatorEndLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void HighlightClick()
		{
			int startIdx;
			int endIdx;

			try
			{
				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				HighlightStore(out startIdx, out endIdx);
				_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the store label portion of a store definition label.
	/// </summary>

	public class AttrQueryStoreDetailLabel : AttrQueryStoreLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryStoreDetailLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Abstract class that defines a spacer.
	/// </summary>

	public class AttrQuerySpacerLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQuerySpacerLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor)
		{
			//Begin Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
			//int height;

			//Text = " ";
			//height = Height;
			//AutoSize = false;
			//Width = 3;
			//Height = height;
			Size defaultSize;

			Text = " ";
			defaultSize = DefaultSize;
			AutoSize = false;
			Width = 3;
			Height = defaultSize.Height;
			//End Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void MouseUpHandler(object sender, MouseEventArgs e)
		{
		}
	}
	#endregion

	#region DataQueryOperand Classes
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	// Data Query labels
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================

	/// <summary>
	/// Abstract class that defines a plan
	/// </summary>

	abstract public class DataQueryPlanLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryPlanLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void HighlightDragEnter(out int aStartIdx, out int aEndIdx)
		{
			try
			{
				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				HighlightPlan(out aStartIdx, out aEndIdx);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void HighlightPlan(out int aStartIdx, out int aEndIdx)
		{
			int i;

			try
			{
				for (i = (int)Tag; i >= 0 && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(DataQueryPlanOperand)) && !((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(DataQueryVariableOperand)); i--)
				{
				}

				aStartIdx = i;

				for (i = aStartIdx; i < ((PanelTag)_form.CurrentPanel.Tag).OperandArray.Count && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(DataQueryPlanOperand)); i++)
				{
				}

				aEndIdx = i - 1;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the main portion of a plan label.
	/// </summary>

	public class DataQueryVariableSeparatorEndLabel : DataQueryPlanLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryVariableSeparatorEndLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void HighlightClick()
		{
			int startIdx;
			int endIdx;

			try
			{
				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				HighlightPlan(out startIdx, out endIdx);
				_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Abstract class that defines the base DataQueryOperand class.  This class defines the base level functionality for a label that is displayed
	/// in the data query panel.
	/// </summary>

	public class DataQueryNodeVersionModifyerLabel : DataQueryPlanLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryNodeVersionModifyerLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

    /// <summary>
    /// Abstract class that defines the base DataQueryOperand class.  This class defines the base level functionality for a label that is displayed
    /// in the data query panel.
    /// </summary>

    public class DataQueryNodeModifyerLabel : DataQueryPlanLabel
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public DataQueryNodeModifyerLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
            : base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
        {
        }

        //===========
        // PROPERTIES
        //===========

        //========
        // METHODS
        //========

        override public void StartEdit(char aKeyPressed)
        {
            try
            {
                //_form.MerchandiseEditTextBox.Tag = _queryOperand;
                //_form.MerchandiseEditTextBox.Width = Math.Max(70, Width);
                //_form.MerchandiseEditTextBox.Left = _form.CurrentPanel.Left + Left;
                //_form.MerchandiseEditTextBox.Top = _form.CurrentPanel.Top + Top;
                //_form.MerchandiseEditTextBox.BringToFront();
                //_form.MerchandiseEditTextBox.Visible = true;
                //_form.MerchandiseEditTextBox.Enabled = true;
                //_form.MerchandiseEditTextBox.Focus();

                //if (aKeyPressed != Char.MinValue)
                //{
                //    _form.MerchandiseEditTextBox.Text = Convert.ToString(aKeyPressed, CultureInfo.CurrentUICulture);
                //    _form.MerchandiseEditTextBox.Select(_form.MerchandiseEditTextBox.TextLength + 1, 0);
                //}
                //else
                //{
                //    _form.MerchandiseEditTextBox.Text = ((DataQueryNodeOperand)_queryOperand).VariableOperand.NodeProfile.NodeID;
                //    _form.MerchandiseEditTextBox.SelectAll();
                //}

                ((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override public void HighlightClick()
        {
            try
            {
                _form.CurrentPanelClearSelectedOperands();

                if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked != null && (int)Tag == (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
                {

                    StartEdit(Char.MinValue);

                    ((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
                }
                else
                {
                    ((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
                    _form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override public void DoubleClickHandler(object sender, EventArgs e)
        {
            try
            {
                StartEdit(Char.MinValue);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

	/// <summary>
	/// Abstract class that defines the base DataQueryOperand class.  This class defines the base level functionality for a label that is displayed
	/// in the data query panel.
	/// </summary>

	public class DataQueryDateRangeLabel : DataQueryPlanLabel
	{
		//=======
		// FIELDS
		//=======

		private CalendarDateSelector _calDateSel;

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryDateRangeLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText, CalendarDateSelector aCalDateSel)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
			DataQueryVariableOperand variableOperand;

			try
			{
				_calDateSel = aCalDateSel;
				_calDateSel = new CalendarDateSelector(_form.SAB);
				_calDateSel.AllowDynamicToStoreOpen = true;
				_calDateSel.AnchorDate = _form.SAB.ClientServerSession.Calendar.CurrentDate;

				variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;

				if (variableOperand.DateRangeProfile != null)
				{
					_calDateSel.DateRangeRID = variableOperand.DateRangeProfile.Key;
					_calDateSel.AnchorDateRelativeTo = variableOperand.DateRangeProfile.RelativeTo;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void HighlightClick()
		{
			DialogResult dateSelResult;
			DataQueryVariableOperand variableOperand;

			try
			{
				_form.CurrentPanelClearSelectedOperands();

				if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked != null && (int)Tag == (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
				{
					variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;

					//					if (_calDateSel == null)
					//					{
					//						_calDateSel = new CalendarDateSelector(_form.SAB);
					//						_calDateSel.AllowDynamicToStoreOpen = true;
					//						_calDateSel.AnchorDate = _form.SAB.ClientServerSession.Calendar.CurrentDate;
					//						if (variableOperand.DateRangeProfile != null)
					//						{
					//							_calDateSel.DateRangeRID = variableOperand.DateRangeProfile.Key;
					//							_calDateSel.AnchorDateRelativeTo = variableOperand.DateRangeProfile.RelativeTo;
					//						}
					//					}

					_calDateSel.StartPosition = FormStartPosition.CenterScreen;
					dateSelResult = _calDateSel.ShowDialog();

					if (dateSelResult == DialogResult.OK)
					{
						//						variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
						variableOperand.DateRangeProfile = (DateRangeProfile)_calDateSel.Tag;
						_form.CurrentPanelClearSelectedOperands();
						_form.CurrentPanelRedrawOperands();
					}

					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				}
				else
				{
					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
					_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an Not label.
	/// </summary>

	public class DataQueryLiteralLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryLiteralLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void StartEdit(char aKeyPressed)
		{
			try
			{
				_form.LiteralEditTextBox.Tag = _queryOperand;
				_form.LiteralEditTextBox.Width = Math.Max(70, Width);
				_form.LiteralEditTextBox.Left = _form.CurrentPanel.Left + Left;
				_form.LiteralEditTextBox.Top = _form.CurrentPanel.Top + Top;
				_form.LiteralEditTextBox.BringToFront();
				_form.LiteralEditTextBox.Visible = true;
				_form.LiteralEditTextBox.Enabled = true;
				_form.LiteralEditTextBox.Focus();

				if (aKeyPressed != Char.MinValue && (Char.IsNumber(aKeyPressed) || aKeyPressed == '-' || aKeyPressed == '.'))
				{
					_form.LiteralEditTextBox.Text = Convert.ToString(aKeyPressed, CultureInfo.CurrentUICulture);
					_form.LiteralEditTextBox.Select(_form.LiteralEditTextBox.TextLength + 1, 0);
				}
				else
				{
					_form.LiteralEditTextBox.Text = ((DataQueryLiteralOperand)_queryOperand).LiteralValue.ToString(CultureInfo.CurrentUICulture);
					_form.LiteralEditTextBox.SelectAll();
				}

				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void HighlightClick()
		{
			try
			{
				_form.CurrentPanelClearSelectedOperands();

				if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked != null && (int)Tag == (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
				{

					StartEdit(Char.MinValue);

					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				}
				else
				{
					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
					_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void DoubleClickHandler(object sender, EventArgs e)
		{
			try
			{
				StartEdit(Char.MinValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an Not label.
	/// </summary>

	public class DataQueryGradeLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryGradeLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void StartEdit(char aKeyPressed)
		{
			try
			{
				_form.GradeEditTextBox.Tag = _queryOperand;
				_form.GradeEditTextBox.Width = Math.Max(70, Width);
				_form.GradeEditTextBox.Left = _form.CurrentPanel.Left + Left;
				_form.GradeEditTextBox.Top = _form.CurrentPanel.Top + Top;
				_form.GradeEditTextBox.BringToFront();
				_form.GradeEditTextBox.Visible = true;
				_form.GradeEditTextBox.Enabled = true;
				_form.GradeEditTextBox.Focus();

				if (aKeyPressed != Char.MinValue && (Char.IsLetterOrDigit(aKeyPressed) || aKeyPressed == '-' || aKeyPressed == '.'))
				{
					_form.GradeEditTextBox.Text = Convert.ToString(aKeyPressed, CultureInfo.CurrentUICulture);
					_form.GradeEditTextBox.Select(_form.GradeEditTextBox.TextLength + 1, 0);
				}
				else
				{
					_form.GradeEditTextBox.Text = ((DataQueryGradeOperand)_queryOperand).GradeValue.ToString(CultureInfo.CurrentUICulture);
					_form.GradeEditTextBox.SelectAll();
				}

				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void HighlightClick()
		{
			try
			{
				_form.CurrentPanelClearSelectedOperands();

				if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked != null && (int)Tag == (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
				{

					StartEdit(Char.MinValue);

					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				}
				else
				{
					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
					_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void DoubleClickHandler(object sender, EventArgs e)
		{
			try
			{
				StartEdit(Char.MinValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an Not label.
	/// </summary>

	public class DataQueryStatusLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryStatusLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Abstract class that defines a spacer.
	/// </summary>

	public class DataQuerySpacerLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQuerySpacerLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor)
		{
			//Begin Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
			//int height;

			//Text = " ";
			//height = Height;
			//AutoSize = false;
			//Width = 3;
			//Height = height;
			Size defaultSize;

			Text = " ";
			defaultSize = DefaultSize;
			AutoSize = false;
			Width = 3;
			Height = defaultSize.Height;
			//End Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void MouseUpHandler(object sender, MouseEventArgs e)
		{
		}
	}
	#endregion

	#region ProdCharQueryOperand Classes
	/// <summary>
	/// Abstract class that defines a product characteristic.
	/// </summary>

	abstract public class ProdCharQueryCharacteristicLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQueryCharacteristicLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//========
		// METHODS
		//========

		override public void HighlightDragEnter(out int aStartIdx, out int aEndIdx)
		{
			try
			{
				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				HighlightCharacteristic(out aStartIdx, out aEndIdx);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void HighlightCharacteristic(out int aStartIdx, out int aEndIdx)
		{
			int i;

			try
			{
				for (i = (int)Tag; i >= 0 && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() != typeof(ProdCharQueryCharacteristicMainOperand); i--)
				{
				}

				aStartIdx = i;

				for (i = aStartIdx; i < ((PanelTag)_form.CurrentPanel.Tag).OperandArray.Count && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(ProdCharQueryCharacteristicOperand)); i++)
				{
				}

				aEndIdx = i - 1;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the characteristic portion of a characteristic definition label.
	/// </summary>

	public class ProdCharQueryCharacteristicMainSeparatorEndLabel : ProdCharQueryCharacteristicLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQueryCharacteristicMainSeparatorEndLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void HighlightClick()
		{
			int startIdx;
			int endIdx;

			try
			{
				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
				HighlightCharacteristic(out startIdx, out endIdx);
				_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the characteristic portion of a characteristic definition label.
	/// </summary>

	public class ProdCharQueryCharacteristicDetailLabel : ProdCharQueryCharacteristicLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQueryCharacteristicDetailLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Abstract class that defines a spacer.
	/// </summary>

	public class ProdCharQuerySpacerLabel : BasicQueryLabel
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQuerySpacerLabel(MIDFilterFormBase aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
			: base(aForm, aFilterDef, aQueryOperand, aForeColor)
		{
			//Begin Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
			//int height;

			//Text = " ";
			//height = Height;
			//AutoSize = false;
			//Width = 3;
			//Height = height;
			Size defaultSize;

			Text = " ";
			defaultSize = DefaultSize;
			AutoSize = false;
			Width = 3;
			Height = defaultSize.Height;
			//End Track #5944 - JScott - When creating a filter with 4 conditions joining with OR not meeting expectations.
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void MouseUpHandler(object sender, MouseEventArgs e)
		{
		}
	}
	#endregion
}

