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
	public partial class ColorNodeSearch : MIDFormBase
	{
		// add events to update explorer when hierarchy is changed
		public delegate void ColorNodeLocateEventHandler(object source, ColorNodeLocateEventArgs e);
		public event ColorNodeLocateEventHandler ColorNodeLocateEvent;

		public delegate void ColorNodeRenameEventHandler(object source, ColorNodeRenameEventArgs e);
		public event ColorNodeRenameEventHandler ColorNodeRenameEvent;


        public delegate void ColorNodeDeleteEventHandler(object source, ColorNodeDeleteEventArgs e);
        public event ColorNodeDeleteEventHandler ColorNodeDeleteEvent;


		#region Variable Declarations

		private bool _searching;
		private bool _nodeUpdated = false;
        //private bool _continueProcessing = false;
        //private bool _progressCancelClicked = false;
		private bool _mouseDown = false;
		private SessionAddressBlock _SAB;
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
        ColorSearchEngine searchEngine = null;
        // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client

		#endregion

		public ColorNodeSearch(SessionAddressBlock aSAB)
			: base(aSAB)
		{
			_SAB = aSAB;
			_hierarchyMaintenance = new HierarchyMaintenance(_SAB);
			_results = new Hashtable();
			InitializeComponent();
			_searching = false;
		}

		private void ColorNodeSearch_Load(object sender, EventArgs e)
		{
			this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
			button1.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
			lblInstructions.Text = MIDText.GetTextOnly(eMIDTextCode.msg_SearchInstructions);
			lblCriteria.Text = MIDText.GetTextOnly(eMIDTextCode.msg_SearchCriteria);
			lblID.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchID);
			lblName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchName);
			lblGroupName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchGroupName);
			gbxOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchOptions);
			chkMatchCase.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchMatchCase);
			chkMatchWholeWord.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchMatchWholeWord);
			
			lvColors.Visible = false;
			BuildContextmenu();
			lvColors.ContextMenuStrip = cmsResults;
			lvColors.SmallImageList = MIDGraphics.ImageList;

			txtEdit.Size = new System.Drawing.Size(0, 0);
			txtEdit.Location = new System.Drawing.Point(0, 0);
			txtEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditOver);
			txtEdit.LostFocus += new System.EventHandler(this.FocusOver);
			txtEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			//txtEdit.BackColor = lvColors.BackColor; 
			//txtEdit.BackColor = Color.LightYellow;
			txtEdit.BorderStyle = BorderStyle.Fixed3D;
			txtEdit.Hide();
			txtEdit.Text = "";
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

				cmiDelete.Visible = false;
				cmiCut.Visible = false;
				cmiRename.Visible = false;
			}
			catch
			{
				throw;
			}
		}

		void cmiLocate_Click(object sender, EventArgs e)
		{
			foreach (System.Windows.Forms.ListViewItem item in lvColors.SelectedItems)
			{
				MIDColorSearchItemTag itemTag = (MIDColorSearchItemTag)item.Tag;
				ColorNodeLocateEvent(this, new ColorNodeLocateEventArgs(itemTag.Key, itemTag.GroupName));
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
				CopyColorsToClipboard(lvColors, eProfileType.ColorCode, DragDropEffects.Copy);
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
				CopyColorsToClipboard(lvColors, eProfileType.ColorCode, DragDropEffects.Move);
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
					lvColors.Items.Clear();
					lvColors.Visible = true;
					lvColors.Dock = DockStyle.Fill;
					lvColors.View = View.Details;
					_searching = true;
					_results.Clear();

					progressBar1.Visible = true;
					progressBar1.Value = 0;
                    // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
                    //ColorSearchEngine searchEngine = new ColorSearchEngine(_SAB, _nodeList, _wildcard, 
                    //    _separator, chkMatchCase.Checked, chkMatchWholeWord.Checked,
                    //    txtID.Text, txtName.Text, txtGroupName.Text);
                    searchEngine = new ColorSearchEngine(_SAB, _nodeList, _wildcard,
                        _separator, chkMatchCase.Checked, chkMatchWholeWord.Checked,
                        txtID.Text, txtName.Text, txtGroupName.Text);
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
				if (lvColors.InvokeRequired)
				{
					lvColors.Invoke(new MIDSearchEventHandler(searchEngine_MIDSearchEvent), new object[] { sender, e });
				}
				else
				{
					MIDColorSearchEventArgs ea = (MIDColorSearchEventArgs)e;
					if (ea.Key > 0)
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

							if (!_results.Contains(ea.Key))
							{
								addToList = true;
								_results.Add(ea.Key, null);
							}

							if (addToList)
							{
								int imageIndex;
								imageIndex = MIDGraphics.ImageIndexWithDefault(Include.MIDDefaultColor, MIDGraphics.ClosedFolder);
								string[] items = new string[] { ea.ID, ea.Name, ea.GroupName };
								System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(items, imageIndex);
								item.Tag = new MIDColorSearchItemTag(ea.Key, ea.GroupName);
								lvColors.Items.Add(item);
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

		private void lvColors_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			try
			{
				foreach (System.Windows.Forms.ListViewItem item in lvColors.SelectedItems)
				{
					MIDColorSearchItemTag itemTag = (MIDColorSearchItemTag)item.Tag;
				}
			}
			catch
			{
				throw;
			}
		}

		private void lvColors_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (!_searching)
			{
				// Determine whether the column is the same as the last column clicked.
				if (e.Column != sortColumn)
				{
					// Set the sort column to the new column.
					sortColumn = e.Column;
					// Set the sort order to ascending by default.
					lvColors.Sorting = SortOrder.Ascending;
				}
				else
				{
					// Determine what the last sort order was and change it.
					if (lvColors.Sorting == SortOrder.Ascending)
						lvColors.Sorting = SortOrder.Descending;
					else
						lvColors.Sorting = SortOrder.Ascending;
				}

				// Call the sort method to manually sort.
				lvColors.Sort();
				// Set the ListViewItemSorter property to a new ListViewItemComparer
				// object.
				lvColors.ListViewItemSorter = new ListViewItemComparer(e.Column,
					lvColors.Sorting);
			}
		}

		private void EditOver(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				MIDColorSearchItemTag itemTag = null;
				if (e.KeyChar == 13)
				{
					itemTag = (MIDColorSearchItemTag)_selectedItem.Tag;

					if (_selectedItem.SubItems[subItemSelected].Text != txtEdit.Text)
					{
						UpdateNode(itemTag, txtEdit.Text);
					}
				}

				if (e.KeyChar == 27) // Esc
				{
					itemTag = (MIDColorSearchItemTag)_selectedItem.Tag;
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
			MIDColorSearchItemTag itemTag = null;
			try
			{
				itemTag = (MIDColorSearchItemTag)_selectedItem.Tag;
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

		private void UpdateNode(MIDColorSearchItemTag aItemTag, string aText)
		{
			try
			{
				if (!_nodeUpdated)
				{
					txtEdit.Hide();

					_selectedItem.SubItems[subItemSelected].Text = txtEdit.Text;
					ColorNodeRenameEvent(this, new ColorNodeRenameEventArgs(aItemTag.Key, aText));
					_nodeUpdated = true;
				}
			}
			catch
			{
				throw;
			}
		}

		private void lvColors_DoubleClick(object sender, EventArgs e)
		{
			EditText();
		}

		private void EditText()
		{
			try
			{
				_nodeUpdated = false;
				MIDColorSearchItemTag itemTag = null;
				eLockStatus lockStatus = eLockStatus.Undefined;
				// Check the subitem clicked .
				int nStart = _XPos;
				int spos = 20;
				int epos = lvColors.Columns[0].Width;
				for (int i = 0; i < lvColors.Columns.Count; i++)
				{
					if (nStart > spos && nStart < epos)
					{
						subItemSelected = i;
						break;
					}

					spos = epos;
					epos += lvColors.Columns[i].Width;
				}

				subItemText = _selectedItem.SubItems[subItemSelected].Text;

				string colName = lvColors.Columns[subItemSelected].Text;
				if (colName == "ID")
				{
					itemTag = (MIDColorSearchItemTag)_selectedItem.Tag;
					lockStatus = eLockStatus.Undefined;

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

		private void lvColors_MouseDown(object sender, MouseEventArgs e)
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
					_selectedItem = lvColors.GetItemAt(e.X, e.Y);
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

		private void lvColors_MouseUp(object sender, MouseEventArgs e)
		{
			_mouseDown = false;
		}

		private void lvColors_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				if (_mouseDown &&
					lvColors.SelectedItems.Count > 0)
				{
					ColorCodeClipboardList ccList = CopyColorsToClipboard(lvColors, eProfileType.ColorCode, DragDropEffects.Move);
					int xPos, yPos;
					int imageHeight, imageWidth;
					ColorTreeView tempTreeView = new ColorTreeView();
                    MIDGraphics.BuildDragImage(ccList, ImageListDrag, tempTreeView.Indent, Spacing,
									Font, ForeColor, out imageHeight, out imageWidth);

					xPos = imageWidth / 2;
					yPos = imageHeight / 2;

					// Begin dragging image
					if (DragHelper.ImageList_BeginDrag(ImageListDrag.Handle, 0, xPos, yPos))
					{
						// Begin dragging
                        lvColors.DoDragDrop(ccList, DragDropEffects.Move);
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
			foreach (ListViewItem item in lvColors.Items)
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
				if (lvColors.SelectedItems.Count > 0)
				{
					cmiLocate.Enabled = true;
					foreach (System.Windows.Forms.ListViewItem item in lvColors.SelectedItems)
					{
						MIDColorSearchItemTag itemTag = (MIDColorSearchItemTag)item.Tag;
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
            ColorCodeClipboardProfile cbp;
            FunctionSecurityProfile securityProfile;

			try
			{
				MIDColorSearchItemTag itemTag = (MIDColorSearchItemTag)aItem.Tag;
                ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(itemTag.Key);
                securityProfile = new FunctionSecurityProfile(ccp.Key);
                securityProfile.SetFullControl();
                cbp = new ColorCodeClipboardProfile(itemTag.Key, ccp.Text, securityProfile);
                cbp.Action = aAction;

                // use temp tree node to calculate dimensions so calculations for image dragging are consistent
                ColorTreeView tempTreeView = new ColorTreeView();
                MIDColorNode tempNode = new MIDColorNode(SAB, eTreeNodeType.ObjectNode, ccp, ccp.Text, Include.NoRID, Include.NoRID, securityProfile, Include.NoRID, Include.NoRID, Include.NoRID);
                tempTreeView.Nodes.Add(tempNode);
				cbp.DragImage = lvColors.SmallImageList.Images[aItem.ImageIndex];
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
			MIDRetail.Windows.DisplayMessages.Show(em, _SAB, this.Text);
		}

		private void lvColors_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void lvColors_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		private void lvColors_DragLeave(object sender, EventArgs e)
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

	public class MIDColorSearchItemTag
	{
		private int _key;
		private string _groupName;

		public MIDColorSearchItemTag(int aKey, string aGroupName)
		{
			_key = aKey;
			_groupName = aGroupName;
		}

		public int Key
		{
			get { return _key; }
		}

		public string GroupName
		{
			get { return _groupName; }
		}
	}

	public class ColorNodeLocateEventArgs : EventArgs
	{
		int _key;
		string _groupName;

		public ColorNodeLocateEventArgs(int aKey, string aGroupName)
		{
			_key = aKey;
			_groupName = aGroupName;
		}
		public int Key
		{
			get { return _key; }
			set { _key = value; }
		}
		public string GroupName
		{
			get { return _groupName; }
			set { _groupName = value; }
		}
	}

	public class ColorNodeRenameEventArgs : EventArgs
	{
		private int _key;
		private string _text;

		public ColorNodeRenameEventArgs(int aKey, string aText)
		{
			_key = aKey;
			_text = aText;
		}
		public int Key
		{
			get { return _key; }
			set { _key = value; }
		}
		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}
	}

	public class ColorNodeDeleteEventArgs : EventArgs
	{
		private int _parentKey;
		private int _key;

		public ColorNodeDeleteEventArgs(int aParentKey, int aKey)
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