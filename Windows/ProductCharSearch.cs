using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;


namespace MIDRetail.Windows
{
	public partial class ProductCharSearch : MIDFormBase
	{
		// add events to update explorer when hierarchy is changed
		public delegate void ProductCharLocateEventHandler(object source, ProductCharLocateEventArgs e);
		public event ProductCharLocateEventHandler ProductCharLocateEvent;

		public delegate void ProductCharRenameEventHandler(object source, ProductCharRenameEventArgs e);
		public event ProductCharRenameEventHandler ProductCharRenameEvent;


        public delegate void ProductCharDeleteEventHandler(object source, ProductCharDeleteEventArgs e);
        public event ProductCharDeleteEventHandler ProductCharDeleteEvent;


		#region Variable Declarations

		private bool _searching;
		private bool _nodeUpdated = false;
		private bool _mouseDown = false;
		private HierarchyMaintenance _hierarchyMaintenance;
		private ArrayList _nodeList = new ArrayList();
		private Hashtable _results;
		private Thread _thread;
		private int sortColumn = -1;
		private ListViewItem _selectedItem;
		private int _XPos = 0;
		private int _YPos = 0;
		private string subItemText;
		private int subItemSelected = 0;
		private char _wildcard = '*';
		private char _separator = ';';
        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
        ProductCharSearchEngine searchEngine = null;
        // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client

		#endregion

		public ProductCharSearch(SessionAddressBlock aSAB)
			: base(aSAB)
		{
			_hierarchyMaintenance = new HierarchyMaintenance(SAB);
			_results = new Hashtable();
			InitializeComponent();
			_searching = false;
		}

		private void ProductCharSearch_Load(object sender, EventArgs e)
		{
			SetText();
			
			lvProductChars.Visible = false;
			BuildContextmenu();
			lvProductChars.ContextMenuStrip = cmsResults;
			lvProductChars.SmallImageList = MIDGraphics.ImageList;

			txtEdit.Size = new System.Drawing.Size(0, 0);
			txtEdit.Location = new System.Drawing.Point(0, 0);
			txtEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditOver);
			txtEdit.LostFocus += new System.EventHandler(this.FocusOver);
			txtEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			//txtEdit.BackColor = lvProductChars.BackColor; 
			//txtEdit.BackColor = Color.LightYellow;
			txtEdit.BorderStyle = BorderStyle.Fixed3D;
			txtEdit.Hide();
			txtEdit.Text = "";
		}

		private void SetText()
		{
			this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
			button1.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
			lblInstructions.Text = MIDText.GetTextOnly(eMIDTextCode.msg_SearchInstructions);
			lblCriteria.Text = MIDText.GetTextOnly(eMIDTextCode.msg_SearchCriteria);
			lblValue.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchValue);
			lblCharacteristic.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchCharacteristic);
			gbxOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchOptions);
			chkMatchCase.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchMatchCase);
			chkMatchWholeWord.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchMatchWholeWord);
			colCharacteristic.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Characteristic);
			colValue.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Value);
		}
		private void BuildContextmenu()
		{
			try
			{
				cmiCut.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Cut);
				cmiCopy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Copy);
				cmiDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Delete);
				cmiRename.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Rename);
				cmiLocate.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Locate);
				cmiSelectAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SelectAllEntries);
				cmiCut.Image = MIDGraphics.GetImage(MIDGraphics.CutImage);
				cmiCopy.Image = MIDGraphics.GetImage(MIDGraphics.CopyImage);
				cmiDelete.Image = MIDGraphics.GetImage(MIDGraphics.DeleteImage);
				//cmiRename.Image = MIDGraphics.GetImage(MIDGraphics.RenameImage);
				cmiLocate.Image = MIDGraphics.GetImage(MIDGraphics.FindImage);
				cmiCut.Click += new EventHandler(cmiCut_Click);
				cmiCopy.Click += new EventHandler(cmiCopy_Click);
				cmiDelete.Click += new EventHandler(cmiDelete_Click);
				cmiRename.Click += new EventHandler(cmiRename_Click);
				cmiLocate.Click += new EventHandler(cmiLocate_Click);

				cmiCut.Visible = false;
			}
			catch
			{
				throw;
			}
		}

		void cmiLocate_Click(object sender, EventArgs e)
		{
			foreach (System.Windows.Forms.ListViewItem item in lvProductChars.SelectedItems)
			{
				MIDProductCharSearchItemTag itemTag = (MIDProductCharSearchItemTag)item.Tag;
				ProductCharLocateEvent(this, new ProductCharLocateEventArgs(itemTag.ValueKey, itemTag.CharacteristicKey));
			}
		}

		void cmiRename_Click(object sender, EventArgs e)
		{
			try
			{
				EditText();
			}
			catch
			{
				throw;
			}
		}

		void cmiDelete_Click(object sender, EventArgs e)
		{
			try
			{
				frmProgress progress = new frmProgress(0, 0);
				progress.Title = MIDText.GetTextOnly(eMIDTextCode.msg_Deleting);
                //_continueProcessing = true;
				progress.labelText = MIDText.GetTextOnly(eMIDTextCode.msg_LockingForDelete);
				progress.Show();

                //string text = null;

				progress.EnableOKButton();

			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}

		void cmiCopy_Click(object sender, EventArgs e)
		{
			try
			{
				CopyProductCharacteristicsToClipboard(lvProductChars, eProfileType.ProductCharacteristicValue, DragDropEffects.Copy);
			}
			catch
			{
				throw;
			}
		}

		void cmiCut_Click(object sender, EventArgs e)
		{
			try
			{
				CopyProductCharacteristicsToClipboard(lvProductChars, eProfileType.ProductCharacteristic, DragDropEffects.Move);
			}
			catch
			{
				throw;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				if (_searching)
				{
                    // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
                    //_thread.Abort();
                    searchEngine.RequestStop();
                    // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
                    // wait for thread to exit
                    _thread.Join();
					button1.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
					_searching = false;
				}
				else
				{
					button1.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Stop);
					lblInstructions.Visible = false;
					lvProductChars.Items.Clear();
					lvProductChars.Visible = true;
					lvProductChars.Dock = DockStyle.Fill;
					lvProductChars.View = View.Details;
					_searching = true;
					_results.Clear();

					progressBar1.Visible = true;
					progressBar1.Value = 0;
                    // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
                    //ProductCharSearchEngine searchEngine = new ProductCharSearchEngine(SAB, _nodeList, _wildcard, 
                    //    _separator, chkMatchCase.Checked, chkMatchWholeWord.Checked,
                    //    txtValue.Text, txtCharacteristic.Text);
                    searchEngine = new ProductCharSearchEngine(SAB, _nodeList, _wildcard,
                        _separator, chkMatchCase.Checked, chkMatchWholeWord.Checked,
                        txtValue.Text, txtCharacteristic.Text);
                    // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
					searchEngine.MIDSearchEvent +=new MIDSearchEventHandler(searchEngine_MIDSearchEvent);
					searchEngine.MIDSearchCompletedEvent += new MIDSearchCompletedEventHandler(searchEngine_MIDSearchCompletedEvent);
					_thread = new Thread(new ThreadStart(searchEngine.GetSearchResults));
					_thread.Start();
                    // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
                    // Loop until worker thread activates.
                    while (!_thread.IsAlive) ;
                    // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
				}
			}
			catch
			{
				throw;
			}
		}

		void searchEngine_MIDSearchCompletedEvent(object sender, MIDSearchCompletedEventArgs e)
		{
			try
			{
				if (button1.InvokeRequired)
				{
					button1.Invoke(new MIDSearchCompletedEventHandler(searchEngine_MIDSearchCompletedEvent), new object[] { sender, e });
				}
				else
				{
					button1.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
					_searching = false;
					progressBar1.Visible = false;
				}

			}
			catch
			{
				throw;
			}
		}

		void searchEngine_MIDSearchEvent(object sender, MIDSearchEventArgs e)
		{
			try
			{
				bool addToList = false;
				if (lvProductChars.InvokeRequired)
				{
					lvProductChars.Invoke(new MIDSearchEventHandler(searchEngine_MIDSearchEvent), new object[] { sender, e });
				}
				else
				{
					MIDProductCharSearchEventArgs ea = (MIDProductCharSearchEventArgs)e;
					if (ea.ValueKey > 0)
					{
						lock (_results.SyncRoot)
						{
							if (progressBar1.Value >= 100)
							{
								progressBar1.Value = 0;
							}
							else
							{
								progressBar1.Value += 10;
							}

							if (!_results.Contains(ea.ValueKey))
							{
								addToList = true;
								_results.Add(ea.ValueKey, null);
							}

							if (addToList)
							{
								int imageIndex;
								imageIndex = MIDGraphics.ImageIndexWithDefault(Include.MIDDefaultColor, MIDGraphics.ClosedFolder);
								string[] items = new string[] { ea.Value, ea.Characteristic };
								System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(items, imageIndex);
								item.Tag = new MIDProductCharSearchItemTag(ea.ValueKey, ea.CharacteristicKey);
								lvProductChars.Items.Add(item);
							}
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void lvProductChars_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			try
			{
				foreach (System.Windows.Forms.ListViewItem item in lvProductChars.SelectedItems)
				{
					MIDProductCharSearchItemTag itemTag = (MIDProductCharSearchItemTag)item.Tag;
				}
			}
			catch
			{
				throw;
			}
		}

		private void lvProductChars_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (!_searching)
			{
				// Determine whether the column is the same as the last column clicked.
				if (e.Column != sortColumn)
				{
					// Set the sort column to the new column.
					sortColumn = e.Column;
					// Set the sort order to ascending by default.
					lvProductChars.Sorting = SortOrder.Ascending;
				}
				else
				{
					// Determine what the last sort order was and change it.
					if (lvProductChars.Sorting == SortOrder.Ascending)
						lvProductChars.Sorting = SortOrder.Descending;
					else
						lvProductChars.Sorting = SortOrder.Ascending;
				}

				// Call the sort method to manually sort.
				lvProductChars.Sort();
				// Set the ListViewItemSorter property to a new ListViewItemComparer
				// object.
				lvProductChars.ListViewItemSorter = new ListViewItemComparer(e.Column,
					lvProductChars.Sorting);
			}
		}

		private void EditOver(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				MIDProductCharSearchItemTag itemTag = null;
				if (e.KeyChar == 13)
				{
					itemTag = (MIDProductCharSearchItemTag)_selectedItem.Tag;

					if (_selectedItem.SubItems[subItemSelected].Text != txtEdit.Text)
					{
						UpdateNode(itemTag, txtEdit.Text);
					}
				}

				if (e.KeyChar == 27) // Esc
				{
					itemTag = (MIDProductCharSearchItemTag)_selectedItem.Tag;
					txtEdit.Hide();
				}
			}
			catch
			{
				throw;
			}
		}

		private void FocusOver(object sender, System.EventArgs e)
		{
			MIDProductCharSearchItemTag itemTag = null;
			try
			{
				itemTag = (MIDProductCharSearchItemTag)_selectedItem.Tag;
				if (_selectedItem.SubItems[subItemSelected].Text != txtEdit.Text)
				{
					UpdateNode(itemTag, txtEdit.Text);
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (itemTag != null)
				{
				}
			}
		}

		private void UpdateNode(MIDProductCharSearchItemTag aItemTag, string aText)
		{
			try
			{
				if (!_nodeUpdated)
				{
					HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
					_nodeUpdated = true;
					if (hm.IsProductCharValueValid(aItemTag.ValueKey, aItemTag.CharacteristicKey, txtEdit.Text))
					{
						txtEdit.Hide();

						_selectedItem.SubItems[subItemSelected].Text = txtEdit.Text;
						ProductCharRenameEvent(this, new ProductCharRenameEventArgs(aItemTag.ValueKey, aItemTag.CharacteristicKey, aText));
					}
					else
					{
						txtEdit.Text = _selectedItem.SubItems[subItemSelected].Text;
						MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void lvProductChars_DoubleClick(object sender, EventArgs e)
		{
			EditText();
		}

		private void EditText()
		{
			try
			{
				_nodeUpdated = false;
				MIDProductCharSearchItemTag itemTag = null;
				eLockStatus lockStatus = eLockStatus.Undefined;
				// Check the subitem clicked .
				int nStart = _XPos;
				int spos = 20;
				int epos = lvProductChars.Columns[0].Width;
				for (int i = 0; i < lvProductChars.Columns.Count; i++)
				{
					if (nStart > spos && nStart < epos)
					{
						subItemSelected = i;
						break;
					}

					spos = epos;
					epos += lvProductChars.Columns[i].Width;
				}

				if (_selectedItem == null)
				{
					return;
				}

				subItemText = _selectedItem.SubItems[subItemSelected].Text;

				string colName = lvProductChars.Columns[subItemSelected].Text;
				if (colName == "Value")
				{
					itemTag = (MIDProductCharSearchItemTag)_selectedItem.Tag;
					lockStatus = eLockStatus.Locked;

					if (lockStatus == eLockStatus.Locked)
					{
						Rectangle r = new Rectangle(spos, _selectedItem.Bounds.Y, epos, _selectedItem.Bounds.Bottom);
						txtEdit.Size = new System.Drawing.Size(epos - spos, _selectedItem.Bounds.Bottom - _selectedItem.Bounds.Top);
						txtEdit.Location = new System.Drawing.Point(spos, _selectedItem.Bounds.Y);
						txtEdit.Show();
						txtEdit.Text = subItemText;
						txtEdit.SelectAll();
						txtEdit.Focus();
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void lvProductChars_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				bool bControl = (ModifierKeys == Keys.Control);
				bool bShift = (ModifierKeys == Keys.Shift);
				bool selectItem = false;

				if (e.Button == MouseButtons.Left)
				{
					selectItem = true;
				}
				else if (e.Button == MouseButtons.Right &&
					!bControl &&
					!bShift)
				{
					selectItem = true;
				}

				if (selectItem)
				{
					_selectedItem = lvProductChars.GetItemAt(e.X, e.Y);
					_XPos = e.X;
					_YPos = e.Y;
				}
				_mouseDown = true;
			}
			catch
			{
				throw;
			}
		}

		private void lvProductChars_MouseUp(object sender, MouseEventArgs e)
		{
			_mouseDown = false;
		}

		private void lvProductChars_MouseMove(object sender, MouseEventArgs e)
		{
            ProductCharacteristicClipboardList pcList;
			try
			{
				if (_mouseDown &&
					lvProductChars.SelectedItems.Count > 0)
				{
					pcList = CopyProductCharacteristicsToClipboard(lvProductChars, eProfileType.ProductCharacteristic, DragDropEffects.Move);
					int xPos, yPos;
					int imageHeight, imageWidth;
					ProductCharTreeView tempTreeView = new ProductCharTreeView();
                    MIDGraphics.BuildDragImage(pcList, ImageListDrag, tempTreeView.Indent, Spacing,
									Font, ForeColor, out imageHeight, out imageWidth);

					xPos = imageWidth / 2;
					yPos = imageHeight / 2;

					// Begin dragging image
					if (DragHelper.ImageList_BeginDrag(ImageListDrag.Handle, 0, xPos, yPos))
					{
						// Begin dragging
                        lvProductChars.DoDragDrop(pcList, DragDropEffects.Move);
						// End dragging image
						DragHelper.ImageList_EndDrag();
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void cmiSelectAll_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lvProductChars.Items)
			{
				item.Selected = true;
			}
		}

		private void cmsResults_Opening(object sender, CancelEventArgs e)
		{
			try
			{
				cmiRename.Enabled = false;
				cmiLocate.Enabled = false;
				cmiDelete.Enabled = false;
				cmiCut.Enabled = false;
				cmiCopy.Enabled = false;
				if (lvProductChars.SelectedItems.Count > 0)
				{
					cmiLocate.Enabled = true;
					cmiRename.Enabled = true;
					foreach (System.Windows.Forms.ListViewItem item in lvProductChars.SelectedItems)
					{
						MIDProductCharSearchItemTag itemTag = (MIDProductCharSearchItemTag)item.Tag;
						cmiCopy.Enabled = true;
					}
				}

				// hide until functions are enabled
				toolStripMenuItem1.Visible = false;
			}
			catch
			{
			}
			//if (_searching)
			//{
			//    e.Cancel = true;
			//    return;
			//}
		}

		private bool AllowRename(int aNodeRID)
		{
			try
			{
				return false;
			}
			catch
			{
				throw;
			}
		}

		override protected ClipboardProfileBase BuildClipboardItem(System.Windows.Forms.ListViewItem aItem, DragDropEffects aAction)
		{
            ProductCharacteristicClipboardProfile cbp;
            FunctionSecurityProfile securityProfile;
			try
			{
				MIDProductCharSearchItemTag itemTag = (MIDProductCharSearchItemTag)aItem.Tag;
                ProductCharValueProfile pcvp = SAB.HierarchyServerSession.GetProductCharValueProfile(itemTag.ValueKey);
                securityProfile = new FunctionSecurityProfile(pcvp.Key);
                securityProfile.SetFullControl();
                cbp = new ProductCharacteristicClipboardProfile(itemTag.ValueKey, pcvp.ProductCharValue, securityProfile);
                cbp.ProductCharGroupKey = itemTag.CharacteristicKey;
                cbp.Action = aAction;

                // use temp tree node to calculate dimensions so calculations for image dragging are consistent
				ProductCharTreeView tempTreeView = new ProductCharTreeView();
                MIDProductCharNode tempNode = new MIDProductCharNode(SAB, eTreeNodeType.ObjectNode, pcvp, pcvp.Text, Include.NoRID, Include.NoRID, null, Include.NoRID, Include.NoRID, Include.NoRID);
                tempTreeView.Nodes.Add(tempNode);
				cbp.DragImage = lvProductChars.SmallImageList.Images[aItem.ImageIndex];
                cbp.DragImageHeight = tempNode.Bounds.Height;
				cbp.DragImageWidth = tempNode.Bounds.Width;
                
                return cbp;
			}
			catch
			{
				throw;
			}
		}

        private void DisplayMessages(EditMsgs em)
		{
			MIDRetail.Windows.DisplayMessages.Show(em, SAB, this.Text);
		}

		private void lvProductChars_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void lvProductChars_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		private void lvProductChars_DragLeave(object sender, EventArgs e)
		{
			Image_DragLeave(sender, e);
		}

		private void splitContainer1_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void splitContainer1_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		private void splitContainer1_DragLeave(object sender, EventArgs e)
		{
			Image_DragLeave(sender, e);
		}

		private void splitContainer1_Panel1_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void splitContainer1_Panel1_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		private void splitContainer1_Panel1_DragLeave(object sender, EventArgs e)
		{
			Image_DragLeave(sender, e);
		}
	}

	public class MIDProductCharSearchItemTag
	{
		private int _valueKey;
		private int _characteristicKey;
		
		public MIDProductCharSearchItemTag(int aValueKey, int aCharacteristicKey)
		{
			_valueKey = aValueKey;
			_characteristicKey = aCharacteristicKey;
		}

		public int ValueKey
		{
			get { return _valueKey; }
		}
		public int CharacteristicKey
		{
			get { return _characteristicKey; }
		}
	}

	public class ProductCharLocateEventArgs : EventArgs
	{
		int _valueKey;
		int _characteristicKey;

		public ProductCharLocateEventArgs(int aValueKey, int aCharacteristicKey)
		{
			_valueKey = aValueKey;
			_characteristicKey = aCharacteristicKey;
		}
		public int ValueKey
		{
			get { return _valueKey; }
			set { _valueKey = value; }
		}
		public int CharacteristicKey
		{
			get { return _characteristicKey; }
			set { _characteristicKey = value; }
		}
	}

	public class ProductCharRenameEventArgs : EventArgs
	{
		int _valueKey;
		int _characteristicKey;
		private string _text;

		public ProductCharRenameEventArgs(int aValueKey, int aCharacteristicKey, string aText)
		{
			_valueKey = aValueKey;
			_characteristicKey = aCharacteristicKey;
			_text = aText;
		}
		public int ValueKey
		{
			get { return _valueKey; }
			set { _valueKey = value; }
		}
		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}
		public int CharacteristicKey
		{
			get { return _characteristicKey; }
			set { _characteristicKey = value; }
		}
	}

	public class ProductCharDeleteEventArgs : EventArgs
	{
		private int _parentKey;
		private int _key;

		public ProductCharDeleteEventArgs(int aParentKey, int aKey)
		{
			_parentKey = aParentKey;
			_key = aKey;
		}
		public int ParentKey
		{
			get { return _parentKey; }
			set { _parentKey = value; }
		}
		public int Key
		{
			get { return _key; }
			set { _key = value; }
		}
	}
}