// Begin Track #4868 - JSmith - Variable Groupings
// Too many changes to mark.  Use difference tool for comparison.
// End Track #4868

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Timers;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for RowColChooserOrderPanel.
	/// </summary>
	public class RowColChooserOrderPanel : RowColChooserPanel
	{
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel pnlHeaders;
		private System.Windows.Forms.Panel pnlOrder;
		private System.Windows.Forms.Label lblHeaders;
		private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.ListBox lstOrder;
		private System.Windows.Forms.ContextMenu cmsHeaders;
		private System.Windows.Forms.MenuItem cmiRestoreDefaults;
		private System.Windows.Forms.MenuItem cmiSelectAll;
		private System.Windows.Forms.MenuItem cmiClearAll;
        private TreeView tvHeaders;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			if (disposing)
			{
				this.lstOrder.DrawItem -= new System.Windows.Forms.DrawItemEventHandler(this.lstOrder_DrawItem);
				this.lstOrder.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.lstOrder_MouseUp);
				this.lstOrder.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.lstOrder_MouseMove);
				this.lstOrder.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.lstOrder_MouseDown);
				this.lstOrder.DragOver -= new System.Windows.Forms.DragEventHandler(this.lstOrder_DragOver);
                this.tvHeaders.AfterCheck -= new System.Windows.Forms.TreeViewEventHandler(this.tvHeaders_AfterCheck);
				this.cmiRestoreDefaults.Click -= new System.EventHandler(this.cmiRestoreDefaults_Click);
				this.cmiSelectAll.Click -= new System.EventHandler(this.cmiSelectAll_Click);
				this.cmiClearAll.Click -= new System.EventHandler(this.cmiClearAll_Click);
				this.Load -= new System.EventHandler(this.RowColChooserOrderPanel_Load);

				if (_scrollUpTimer != null)
				{
					_scrollUpTimer.Elapsed -= new ElapsedEventHandler(ScrollUpTimer_Elapsed);
				}

				if (_scrollDownTimer != null)
				{
					_scrollDownTimer.Elapsed -= new ElapsedEventHandler(ScrollDownTimer_Elapsed);
				}
			}

			base.Dispose(disposing);
		}
		
		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pnlHeaders = new System.Windows.Forms.Panel();
            this.tvHeaders = new System.Windows.Forms.TreeView();
            this.cmsHeaders = new System.Windows.Forms.ContextMenu();
            this.cmiRestoreDefaults = new System.Windows.Forms.MenuItem();
            this.cmiSelectAll = new System.Windows.Forms.MenuItem();
            this.cmiClearAll = new System.Windows.Forms.MenuItem();
            this.lblHeaders = new System.Windows.Forms.Label();
            this.pnlOrder = new System.Windows.Forms.Panel();
            this.lstOrder = new System.Windows.Forms.ListBox();
            this.lblOrder = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlHeaders.SuspendLayout();
            this.pnlOrder.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeaders
            // 
            this.pnlHeaders.Controls.Add(this.tvHeaders);
            this.pnlHeaders.Controls.Add(this.lblHeaders);
            this.pnlHeaders.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlHeaders.Location = new System.Drawing.Point(0, 0);
            this.pnlHeaders.Name = "pnlHeaders";
            this.pnlHeaders.Size = new System.Drawing.Size(208, 280);
            this.pnlHeaders.TabIndex = 0;
            // 
            // tvHeaders
            // 
            this.tvHeaders.CheckBoxes = true;
            this.tvHeaders.ContextMenu = this.cmsHeaders;
            this.tvHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvHeaders.Location = new System.Drawing.Point(0, 16);
            this.tvHeaders.Name = "tvHeaders";
            this.tvHeaders.Size = new System.Drawing.Size(208, 264);
            this.tvHeaders.TabIndex = 2;
            this.tvHeaders.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvHeaders_AfterCheck);
            this.tvHeaders.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvHeaders_AfterCollapse);
            this.tvHeaders.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvHeaders_AfterExpand);
            // 
            // cmsHeaders
            // 
            this.cmsHeaders.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmiRestoreDefaults,
            this.cmiSelectAll,
            this.cmiClearAll});
            // 
            // cmiRestoreDefaults
            // 
            this.cmiRestoreDefaults.Index = 0;
            this.cmiRestoreDefaults.Text = "Restore Defaults";
            this.cmiRestoreDefaults.Click += new System.EventHandler(this.cmiRestoreDefaults_Click);
            // 
            // cmiSelectAll
            // 
            this.cmiSelectAll.Index = 1;
            this.cmiSelectAll.Text = "Select All";
            this.cmiSelectAll.Click += new System.EventHandler(this.cmiSelectAll_Click);
            // 
            // cmiClearAll
            // 
            this.cmiClearAll.Index = 2;
            this.cmiClearAll.Text = "Clear All";
            this.cmiClearAll.Click += new System.EventHandler(this.cmiClearAll_Click);
            // 
            // lblHeaders
            // 
            this.lblHeaders.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeaders.Location = new System.Drawing.Point(0, 0);
            this.lblHeaders.Name = "lblHeaders";
            this.lblHeaders.Size = new System.Drawing.Size(208, 16);
            this.lblHeaders.TabIndex = 0;
            this.lblHeaders.Text = "Displayable Items:";
            // 
            // pnlOrder
            // 
            this.pnlOrder.Controls.Add(this.lstOrder);
            this.pnlOrder.Controls.Add(this.lblOrder);
            this.pnlOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOrder.Location = new System.Drawing.Point(208, 0);
            this.pnlOrder.Name = "pnlOrder";
            this.pnlOrder.Size = new System.Drawing.Size(240, 280);
            this.pnlOrder.TabIndex = 1;
            // 
            // lstOrder
            // 
            this.lstOrder.AllowDrop = true;
            this.lstOrder.ContextMenu = this.cmsHeaders;
            this.lstOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstOrder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstOrder.ItemHeight = 15;
            this.lstOrder.Location = new System.Drawing.Point(0, 16);
            this.lstOrder.Name = "lstOrder";
            this.lstOrder.Size = new System.Drawing.Size(240, 259);
            this.lstOrder.TabIndex = 2;
            this.lstOrder.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstOrder_MouseUp);
            this.lstOrder.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstOrder_DrawItem);
            this.lstOrder.DragOver += new System.Windows.Forms.DragEventHandler(this.lstOrder_DragOver);
            this.lstOrder.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstOrder_MouseMove);
            this.lstOrder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstOrder_MouseDown);
            // 
            // lblOrder
            // 
            this.lblOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOrder.Location = new System.Drawing.Point(0, 0);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(240, 16);
            this.lblOrder.TabIndex = 1;
            this.lblOrder.Text = "Order:";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(208, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 280);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // RowColChooserOrderPanel
            // 
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlOrder);
            this.Controls.Add(this.pnlHeaders);
            this.Name = "RowColChooserOrderPanel";
            this.Size = new System.Drawing.Size(448, 280);
            this.Load += new System.EventHandler(this.RowColChooserOrderPanel_Load);
            this.pnlHeaders.ResumeLayout(false);
            this.pnlOrder.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		const int cScrollDelayTime = 200;
		const string cOrderText = "Order";

		private bool _formLoading;
		private bool _changed;
		private ArrayList _selectedHeaders;
		private System.Timers.Timer _scrollUpTimer;
		private System.Timers.Timer _scrollDownTimer;
		private int _dragItemIdx;
		private int _dropItemIdx;
		private Rectangle _dragBox;
		private Point _screenOffset;
		private int _visibleItems;
        private ArrayList _groupings;
        private int _closedImageIndex = -1;
        private int _openImageIndex = -1;
        private int _itemImageIndex = -1;

        // add event to update form when changed
        private ListChangedEventHandler onListChanged;

		public RowColChooserOrderPanel()
		{
			_formLoading = true;
			InitializeComponent();
			_changed = false;
			_selectedHeaders = new ArrayList();
			Initialize();
		}

		public RowColChooserOrderPanel(ArrayList aHeaders, bool aOneHeaderRequired, ArrayList aGroupings)
			: base(aHeaders, aOneHeaderRequired)
		{
			_formLoading = true;
			InitializeComponent();
			_changed = false;
			_selectedHeaders = new ArrayList();
            _groupings = aGroupings;
			Initialize();
		}

        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }

		override public bool isChanged
		{
			get
			{
				return _changed;
			}
		}

        public int SelectedCount
        {
            get { return lstOrder.Items.Count; }
        }

		private void Initialize()
		{
			_scrollUpTimer = new System.Timers.Timer(cScrollDelayTime);
			_scrollUpTimer.Elapsed += new ElapsedEventHandler(ScrollUpTimer_Elapsed);
			_scrollDownTimer = new System.Timers.Timer(cScrollDelayTime);
			_scrollDownTimer.Elapsed += new ElapsedEventHandler(ScrollDownTimer_Elapsed);
            //tvHeaders.ImageList = MIDGraphics.ImageList;
            _closedImageIndex = MIDGraphics.ImageIndexWithDefault(Include.MIDDefaultColor, MIDGraphics.ClosedFolder);
            _openImageIndex = MIDGraphics.ImageIndexWithDefault(Include.MIDDefaultColor, MIDGraphics.OpenFolder);
            _itemImageIndex = MIDGraphics.ImageIndexWithDefault(MIDGraphics.Blank, null);
            //_itemImageIndex = MIDGraphics.ImageIndexWithDefault(MIDGraphics.ChooserImage, null);
		}

		private void HandleExceptions(System.Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }

        private void RowColChooserOrderPanel_Load(object sender, EventArgs e)
		{
			try
			{
				lstOrder.Visible = true;
				lblOrder.Text = cOrderText;

				_formLoading = false;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiClearAll_Click(object sender, System.EventArgs e)
		{
			try
			{
                foreach (ChooserTreeNode ctn in tvHeaders.Nodes)
                {
                    SetGroupingNodes(ctn, false);
                }

                if (onListChanged != null)
                {
                    onListChanged(this, new ListChangedEventArgs(ListChangedType.ItemDeleted, -1));
                }
            }
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiSelectAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				foreach (ChooserTreeNode ctn in tvHeaders.Nodes)
                {
                    SetGroupingNodes(ctn, true);
                }

                if (onListChanged != null)
                {
                    onListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, -1));
                }
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiRestoreDefaults_Click(object sender, System.EventArgs e)
		{
			try
			{
				FillControl();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

        private void tvHeaders_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (!_formLoading)
                {
                    lstOrder.BeginUpdate();

                    try
                    {
                        if (((ChooserTreeNode)e.Node).IsGroupingNode)
                        {
                            if (!((ChooserTreeNode)e.Node).IsCascadeChange)
                            {
                                SetGroupingNodes((ChooserTreeNode)e.Node, e.Node.Checked);
                            }
                            else
                            {
                                ((ChooserTreeNode)e.Node).IsCascadeChange = false;
                            }
                            if (e.Node.Parent != null)
                            {
                                CheckGroupForCheck((ChooserTreeNode)e.Node.Parent);
                            }
                        }
                        else if (e.Node.Checked)
                        {
                            lstOrder.Items.Add(((ChooserTreeNode)e.Node).ListBoxItem);
                            if (e.Node.Parent != null)
                            {
                                CheckGroupForCheck((ChooserTreeNode)e.Node.Parent);
                            }
                        }
                        else
                        {
                            lstOrder.Items.Remove(((ChooserTreeNode)e.Node).ListBoxItem);
                            if (e.Node.Parent != null)
                            {
                                ((ChooserTreeNode)e.Node.Parent).IsCascadeChange = true;
                                e.Node.Parent.Checked = false;
                            }
                        }
                    }
                    finally
                    {
                        lstOrder.EndUpdate();
                    }

                    if (onListChanged != null)
                    {
                        onListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, -1));
                    }

                    _changed = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void CheckGroupForCheck(ChooserTreeNode aGroupNode)
        {
            bool check = true;
            foreach (ChooserTreeNode ctn in aGroupNode.Nodes)
            {
                if (!ctn.Checked)
                {
                    check = false;
                }
            }
            if (check)
            {
                aGroupNode.IsCascadeChange = true;
                aGroupNode.Checked = true;
            }
        }

        private void SetGroupingNodes(ChooserTreeNode aChooserTreeNode, bool aChecked)
        {
            try
            {
                if (aChooserTreeNode.Checked != aChecked)
                {
                    aChooserTreeNode.Checked = aChecked;
                }
                foreach (ChooserTreeNode ctn in aChooserTreeNode.Nodes)
                {
                    SetGroupingNodes(ctn, aChecked);
                }
            }
            catch 
            {
                throw;
            }
        }

		private void lstOrder_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				if (!e.Data.GetDataPresent(typeof(ListBoxItem)))
				{
					return;
				}

				if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
				{
					e.Effect = DragDropEffects.Move;
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}

				_dropItemIdx = lstOrder.IndexFromPoint(lstOrder.PointToClient(new Point(e.X, e.Y)));
				lstOrder.SelectedIndex = _dropItemIdx;

				if (_dropItemIdx == lstOrder.TopIndex)
				{
					_scrollUpTimer.Start();
				}
				else
				{
					_scrollUpTimer.Stop();
				}

				if (_dropItemIdx == lstOrder.TopIndex + _visibleItems - 1)
				{
					_scrollDownTimer.Start();
				}
				else
				{
					_scrollDownTimer.Stop();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void lstOrder_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Size dragSize;

			try
			{
				_dragItemIdx = lstOrder.IndexFromPoint(e.X, e.Y);

				if (_dragItemIdx != ListBox.NoMatches)
				{
					dragSize = SystemInformation.DragSize;
					_dragBox = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
				}
				else
				{
					_dragBox = Rectangle.Empty;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void lstOrder_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			DragDropEffects dropEffect;
			Object item;

			try
			{
				if (lstOrder.Items.Count > 1)
				{
					if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
					{
						if (_dragBox != Rectangle.Empty && !_dragBox.Contains(e.X, e.Y))
						{
							_screenOffset = SystemInformation.WorkingArea.Location;
							_visibleItems = (lstOrder.Height - 4) / lstOrder.ItemHeight;
							dropEffect = lstOrder.DoDragDrop(lstOrder.Items[_dragItemIdx], DragDropEffects.All | DragDropEffects.Link);

							_scrollUpTimer.Stop();
							_scrollDownTimer.Stop();

							if (dropEffect == DragDropEffects.Move)
							{
								item = lstOrder.Items[_dragItemIdx];

								lstOrder.BeginUpdate();

								try
								{
									lstOrder.Items.RemoveAt(_dragItemIdx);

									if (_dropItemIdx != ListBox.NoMatches)
									{
										lstOrder.Items.Insert(_dropItemIdx, item);
									}
									else
									{
										_dropItemIdx = lstOrder.Items.Add(item);
									}
								}
								finally
								{
									lstOrder.EndUpdate();
								}

								lstOrder.SelectedIndex = _dropItemIdx;

								_changed = true;
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void lstOrder_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_dragBox = Rectangle.Empty;
		}

		private void lstOrder_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			System.Drawing.Brush brush;

			try
			{
				if (e.Index >= 0 && e.Index < lstOrder.Items.Count)
				{
					e.DrawBackground();
					brush = new System.Drawing.SolidBrush(e.ForeColor);
					e.Graphics.DrawString(lstOrder.Items[e.Index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
					e.DrawFocusRectangle();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ScrollUpTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				if (lstOrder.InvokeRequired)
				{
					_scrollUpTimer.Stop();
					lstOrder.Invoke(new ElapsedEventHandler(this.ScrollUpTimer_Elapsed), new object[] { sender, e } );
					_scrollUpTimer.Start();
				}
				else
				{
					if (_dropItemIdx == lstOrder.TopIndex && _dropItemIdx > 0)
					{
						_dropItemIdx--;
						lstOrder.SelectedIndex = _dropItemIdx;
						lstOrder.TopIndex = _dropItemIdx;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ScrollDownTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			try
			{
				if (lstOrder.InvokeRequired)
				{
					_scrollDownTimer.Stop();
					lstOrder.Invoke(new ElapsedEventHandler(this.ScrollDownTimer_Elapsed), new object [] { sender, e } );
					_scrollDownTimer.Start();
				}
				else
				{
					if (_dropItemIdx == lstOrder.TopIndex + _visibleItems - 1 && _dropItemIdx < lstOrder.Items.Count - 1)
					{
						_dropItemIdx++;
						lstOrder.SelectedIndex = _dropItemIdx;
						lstOrder.TopIndex = _dropItemIdx - _visibleItems + 1;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		override public void FillControl()
		{
			SortedList sortList;
			IDictionaryEnumerator dictEnum;
			RowColProfileHeader profHeader;
            ListBoxItem listBoxItem;
            ChooserTreeNode chooserTreeNode;
            ArrayList chooserItems;
            ChooserTreeNode groupTreeNode;
            RowColHeader rowColHeader;
            Font font;
            ArrayList nodesToDelete;

			try
			{
				_formLoading = true;
                chooserItems = new ArrayList();
                nodesToDelete = new ArrayList();

                tvHeaders.BeginUpdate();

				try
				{
                    tvHeaders.Nodes.Clear();
					_selectedHeaders.Clear();

                    // add groupings
                    if (_groupings != null)
                    {
                        // use FindGroupNode to add groupings
                        foreach (string grp in _groupings)
                        {
                            rowColHeader = new RowColHeader(grp, false, grp);
                            FindGroupNode(rowColHeader);
                        }
                    }

					foreach (RowColHeader header in _headers)
					{
						if (header.IsSelectable)
						{
                            listBoxItem = new ListBoxItem(header, header.Name);
                            chooserTreeNode = new ChooserTreeNode(header.Name, listBoxItem, false, header.IsDisplayed);
                            chooserTreeNode.ImageIndex = _itemImageIndex;
                            chooserTreeNode.SelectedImageIndex = chooserTreeNode.ImageIndex;
                            if (header.Grouping == null)
                            {
                                tvHeaders.Nodes.Add(chooserTreeNode);
                            }
                            else
                            {
                                groupTreeNode = FindGroupNode(header);
                                groupTreeNode.Nodes.Add(chooserTreeNode);
                            }
							_selectedHeaders.Add(header);
                            chooserItems.Add(listBoxItem);
						}
					}

                    // determine if group needs check because all children are checked
                    foreach (ChooserTreeNode ctn in tvHeaders.Nodes)
                    {
                        if (ctn.IsGroupingNode)
                        {
                            if (ctn.Nodes.Count == 0)
                            {
                                nodesToDelete.Add(ctn);
                            }
                            else
                            {
                                AllChildrenChecked(ctn);
                            }
                        }
                    }

                    foreach (ChooserTreeNode ctn in nodesToDelete)
                    {
                        tvHeaders.Nodes.Remove(ctn);
                    }
				}
				finally
				{
                    tvHeaders.EndUpdate();
				}

				lstOrder.BeginUpdate();

				try
				{
					lstOrder.Items.Clear();

					sortList = new SortedList();

                    foreach (ListBoxItem item in chooserItems)
                    {
                        profHeader = (RowColProfileHeader)item.Value;

                        if (profHeader.Sequence != -1)
                        {
                            sortList.Add(profHeader.Sequence, item);
                        }
                    }

					dictEnum = sortList.GetEnumerator();

					while (dictEnum.MoveNext())
					{
						lstOrder.Items.Add(dictEnum.Value);
					}
				}
				finally
				{
					lstOrder.EndUpdate();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			finally
			{
				_changed = false;
				_formLoading = false;
			}
		}

        private ChooserTreeNode FindGroupNode(RowColHeader aHeader)
        {
            string[] groupings = null;
            ChooserTreeNode groupTreeNode = null;
            ChooserTreeNode parentGroupTreeNode = null;
            Font font;

            try
            {
                groupings = aHeader.Grouping.Trim().Split('|');

                if (groupings != null)
                {
                    for (int i = 0; i < groupings.Length; i++)
                    {
                        if (i == 0)  // check root groupings
                        {
                            foreach (ChooserTreeNode ctn in tvHeaders.Nodes)
                            {
                                if (groupings[i] == ctn.Text &&
                                    ctn.IsGroupingNode)
                                {
                                    groupTreeNode = ctn;
                                }
                            }
                            if (groupTreeNode == null)
                            {
                                groupTreeNode = new ChooserTreeNode(groupings[i], null, true, false);
                                groupTreeNode.ImageIndex = _closedImageIndex;
                                groupTreeNode.SelectedImageIndex = groupTreeNode.ImageIndex;
                                font = new Font(tvHeaders.Font, FontStyle.Regular);
                                groupTreeNode.NodeFont = font;
                                tvHeaders.Nodes.Add(groupTreeNode);
                            }
                        }
                        else
                        {
                            parentGroupTreeNode = groupTreeNode;
                            groupTreeNode = null;
                            foreach (ChooserTreeNode ctn in parentGroupTreeNode.Nodes)
                            {
                                if (groupings[i] == ctn.Text &&
                                    ctn.IsGroupingNode)
                                {
                                    groupTreeNode = ctn;
                                }
                            }
                            if (groupTreeNode == null)
                            {
                                groupTreeNode = new ChooserTreeNode(groupings[i], null, true, false);
                                groupTreeNode.ImageIndex = _closedImageIndex;
                                groupTreeNode.SelectedImageIndex = groupTreeNode.ImageIndex;
                                font = new Font(tvHeaders.Font, FontStyle.Regular);
                                groupTreeNode.NodeFont = font;
                                parentGroupTreeNode.Nodes.Add(groupTreeNode);
                            }
                        }
                    }
                }
                return groupTreeNode;
            }
            catch
            {
                throw;
            }
        }

        private void AllChildrenChecked(ChooserTreeNode aChooserTreeNode)
        {
            bool check = true;
            if (aChooserTreeNode.Nodes.Count > 0)
            {
                foreach (ChooserTreeNode ctn in aChooserTreeNode.Nodes)
                {
                    if (ctn.IsGroupingNode)
                    {
                        AllChildrenChecked(ctn);
                    }
                    if (!ctn.Checked)
                    {
                        check = false;
                    }
                }
            }
            else
            {
                check = false;
                if (aChooserTreeNode.Parent != null)
                {
                    aChooserTreeNode.Parent.Nodes.Remove(aChooserTreeNode);
                }
            }

            if (check)
            {
                aChooserTreeNode.Checked = true;
            }
        }

		override public bool ValidateData()
		{
			int i;
			int itemsChecked;

			try
			{
				if (_oneHeaderRequired)
				{
					if (lstOrder.Items.Count == 0)
					{
						MessageBox.Show("You cannot hide all items.  You must choose at least one to display.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					else
					{
						return true;
					}
				}
				else
				{
					return true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void UpdateData()
		{
			int i;
			RowColProfileHeader profHeader;
			IEnumerator iEnum;

			try
			{
				foreach (RowColProfileHeader header in _headers)
				{
					header.Sequence = -1;
					header.IsDisplayed = false;
				}

				iEnum = lstOrder.Items.GetEnumerator();
				i = 0;

				while (iEnum.MoveNext())
				{
					profHeader = (RowColProfileHeader)((ListBoxItem)iEnum.Current).Value;
					profHeader.Sequence = i;
					profHeader.IsDisplayed = true;

					i++;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void ResetChangedFlag()
		{
			_changed = false;
		}

		private class ListBoxItem : ICloneable
		{
			object _value;
			string _displayValue;

			public object Value
			{
				get
				{
					return _value;
				}
			}

			override public string ToString()
			{
				return _displayValue;
			}

			public override bool Equals(object obj)
			{
				return ((ListBoxItem)obj).Value == _value;
			}

			override public int GetHashCode()
			{
				try
				{
					return base.GetHashCode();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			public ListBoxItem(object aValue, string aDisplayValue)
			{
				_value = aValue;
				_displayValue = aDisplayValue;
			}

			public object Clone()
			{
				ListBoxItem newItem;

				try
				{
					newItem = new ListBoxItem(_value, _displayValue);
					return newItem;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        private class ChooserTreeNode : TreeNode
        {
            ListBoxItem _listBoxItem;
            bool _isGroupingNode;
            bool _isDisplayed;
            bool _isCascadeChange;

            public ListBoxItem ListBoxItem
            {
                get
                {
                    return _listBoxItem;
                }
            }

            public bool IsGroupingNode
            {
                get
                {
                    return _isGroupingNode;
                }
            }

            public bool IsDisplayed
            {
                get
                {
                    return _isDisplayed;
                }
                set
                {
                    _isDisplayed = value;
                }
            }

            public bool IsCascadeChange
            {
                get
                {
                    return _isCascadeChange;
                }
                set
                {
                    _isCascadeChange = value;
                }
            }

            public ChooserTreeNode(string aText, ListBoxItem aListBoxItem, bool aIsGroupingNode, bool aIsDisplayed) : base(aText)
            {
                _listBoxItem = aListBoxItem;
                _isDisplayed = aIsDisplayed;
                _isGroupingNode = aIsGroupingNode;
                if (_isDisplayed)
                {
                    Checked = true;
                }
                _isCascadeChange = false;
            }
        }

        private void tvHeaders_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = _closedImageIndex;
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        private void tvHeaders_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                e.Node.ImageIndex = _openImageIndex;
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }
	}

    #region RowColChooserChangeEventArgs Class

    public class RowColChooserChangeEventArgs : EventArgs
    {
        bool _formClosing;

        public RowColChooserChangeEventArgs()
        {
            _formClosing = false;
        }

        public bool FormClosing
        {
            get { return _formClosing; }
            set { _formClosing = value; }
        }
    }

    #endregion RowColChooserChangeEventArgs Class
}
