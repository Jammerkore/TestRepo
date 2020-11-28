using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public partial class RowColChooserComponentPanel : RowColChooserPanel
	{
		const int cScrollDelayTime = 200;
		const string cOrderText = "Order";
		const string cComponentsText = "<Components>";

		ArrayList _componentHeaders;
		ArrayList _otherHeaders;
		private bool _formLoading;
		private bool _changed;
		private MIDTimer _scrollUpTimer;
		private MIDTimer _scrollDownTimer;
		private int _dragItemIdx;
		private int _dropItemIdx;
		private Rectangle _dragBox;
		private Point _screenOffset;
		private int _visibleItems;

		public RowColChooserComponentPanel()
		{
			_formLoading = true;
			InitializeComponent();
			_changed = false;
			Initialize();
		}

		public RowColChooserComponentPanel(ArrayList aHeaders)
			: base(aHeaders, false)
		{
			try
			{
				_formLoading = true;
				InitializeComponent();

				_changed = false;
				Initialize();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public bool isChanged
		{
			get
			{
				return _changed;
			}
		}

		private void Initialize()
		{
			bool componentAdded;

			try
			{
				_componentHeaders = new ArrayList();
				_otherHeaders = new ArrayList();
				componentAdded = false;

				foreach (RowColProfileHeader header in _headers)
				{
					// BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
					if (((AssortmentComponentVariableProfile)header.Profile).MainComponent)
					{
						if (!componentAdded && header.Sequence != -1)
						{
							if (!((AssortmentComponentVariableProfile)header.Profile).HideComponent)
							{
								_otherHeaders.Add(new RowColProfileHeader(cComponentsText, true, header.Sequence, null));
								componentAdded = true;
							}
						}

						if (!((AssortmentComponentVariableProfile)header.Profile).HideComponent)
						{
							_componentHeaders.Add(header);
						}
					}
					else
					{
						if (!((AssortmentComponentVariableProfile)header.Profile).HideComponent)
						{
							_otherHeaders.Add(header);
						}
					}
					// END TT#490-MD - stodd -  post-receipts should not show placeholders
				}

				lstComponentHeaders.Tag = new ListBoxInfo(_componentHeaders, lstComponentOrder, null);
				lstOtherHeaders.Tag = new ListBoxInfo(_otherHeaders, lstOtherOrder, lstComponentHeaders);

				_scrollUpTimer = new MIDTimer(cScrollDelayTime);
				_scrollUpTimer.Elapsed += new ElapsedEventHandler(ScrollUpTimer_Elapsed);
				_scrollDownTimer = new MIDTimer(cScrollDelayTime);
				_scrollDownTimer.Elapsed += new ElapsedEventHandler(ScrollDownTimer_Elapsed);

				spcChooser.Panel2Collapsed = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void HandleExceptions(System.Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		private void RowColChooserComponentPanel_Load(object sender, EventArgs e)
		{
			try
			{
				lstComponentOrder.Visible = true;
				lblComponentOrder.Text = cOrderText;
				lstOtherOrder.Visible = true;
				lblOtherOrder.Text = cOrderText;

				_formLoading = false;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiComponentHeadersClearAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				ClearAll(lstComponentHeaders);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiComponentHeadersSelectAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				SelectAll(lstComponentHeaders);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiComponentHeadersRestoreDefaults_Click(object sender, System.EventArgs e)
		{
			try
			{
				RestoreDefaults(lstComponentHeaders);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiOtherHeadersClearAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				ClearAll(lstOtherHeaders);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiOtherHeadersSelectAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				SelectAll(lstOtherHeaders);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiOtherHeadersRestoreDefaults_Click(object sender, System.EventArgs e)
		{
			try
			{
				RestoreDefaults(lstOtherHeaders);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void CheckListBox_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			CheckedListBox listBox;
			ListBoxInfo listBoxInfo;

			try
			{
				if (!_formLoading)
				{
					listBox = (CheckedListBox)sender;
					listBoxInfo = (ListBoxInfo)listBox.Tag;

					if (e.NewValue == CheckState.Checked)
					{
						listBoxInfo.OrderListBox.Items.Add(listBox.Items[e.Index]);
					}
					else
					{
						listBoxInfo.OrderListBox.Items.Remove(listBox.Items[e.Index]);
					}

					_changed = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ListBox_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			ListBox listBox;

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

				listBox = (ListBox)sender;
				_dropItemIdx = listBox.IndexFromPoint(listBox.PointToClient(new Point(e.X, e.Y)));
				listBox.SelectedIndex = _dropItemIdx;

				if (_dropItemIdx == listBox.TopIndex)
				{
					_scrollUpTimer.Start(listBox);
				}
				else
				{
					_scrollUpTimer.Stop();
				}

				if (_dropItemIdx == listBox.TopIndex + _visibleItems - 1)
				{
					_scrollDownTimer.Start(listBox);
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

		private void ListBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ListBox listBox;
			Size dragSize;

			try
			{
				listBox = (ListBox)sender;
				_dragItemIdx = listBox.IndexFromPoint(e.X, e.Y);

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

		private void ListBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ListBox listBox;
			DragDropEffects dropEffect;
			Object item;

			try
			{
				listBox = (ListBox)sender;

				if (listBox.Items.Count > 1)
				{
					if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
					{
						if (_dragBox != Rectangle.Empty && !_dragBox.Contains(e.X, e.Y))
						{
							_screenOffset = SystemInformation.WorkingArea.Location;
							_visibleItems = (listBox.Height - 4) / listBox.ItemHeight;
							dropEffect = listBox.DoDragDrop(listBox.Items[_dragItemIdx], DragDropEffects.All | DragDropEffects.Link);

							_scrollUpTimer.Stop();
							_scrollDownTimer.Stop();

							if (dropEffect == DragDropEffects.Move)
							{
								item = listBox.Items[_dragItemIdx];

								listBox.Items.RemoveAt(_dragItemIdx);

								if (_dropItemIdx != ListBox.NoMatches)
								{
									listBox.Items.Insert(_dropItemIdx, item);
								}
								else
								{
									_dropItemIdx = listBox.Items.Add(item);
								}

								listBox.SelectedIndex = _dropItemIdx;

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

		private void ListBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_dragBox = Rectangle.Empty;
		}

		private void ListBox_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			ListBox listBox;
			System.Drawing.Brush brush;

			try
			{
				listBox = (ListBox)sender;

				e.DrawBackground();
				brush = new System.Drawing.SolidBrush(e.ForeColor);
				e.Graphics.DrawString(listBox.Items[e.Index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
				e.DrawFocusRectangle();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ScrollUpTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			MIDTimer timer;

			try
			{
				timer = (MIDTimer)sender;

				if (timer.ActiveListBox.InvokeRequired)
				{
					_scrollUpTimer.Stop();
					timer.ActiveListBox.Invoke(new ElapsedEventHandler(this.ScrollUpTimer_Elapsed), sender, e);
					_scrollUpTimer.Start();
				}
				else
				{
					if (_dropItemIdx == timer.ActiveListBox.TopIndex && _dropItemIdx > 0)
					{
						_dropItemIdx--;
						timer.ActiveListBox.SelectedIndex = _dropItemIdx;
						timer.ActiveListBox.TopIndex = _dropItemIdx;
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
			MIDTimer timer;

			try
			{
				timer = (MIDTimer)sender;

				if (timer.ActiveListBox.InvokeRequired)
				{
					_scrollDownTimer.Stop();
					timer.ActiveListBox.Invoke(new ElapsedEventHandler(this.ScrollDownTimer_Elapsed), sender, e);
					_scrollDownTimer.Start();
				}
				else
				{
					if (_dropItemIdx == timer.ActiveListBox.TopIndex + _visibleItems - 1 && _dropItemIdx < timer.ActiveListBox.Items.Count - 1)
					{
						_dropItemIdx++;
						timer.ActiveListBox.SelectedIndex = _dropItemIdx;
						timer.ActiveListBox.TopIndex = _dropItemIdx - _visibleItems + 1;
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
			try
			{
				FillListBox(lstComponentHeaders);
				FillListBox(lstOtherHeaders);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		public void FillListBox(CheckedListBox aListBox)
		{
			ListBoxInfo listBoxInfo;
			ListBoxItem componentListItem;
			SortedList sortList;
			IDictionaryEnumerator dictEnum;
			RowColProfileHeader profHeader;

			try
			{
				_formLoading = true;

				listBoxInfo = (ListBoxInfo)aListBox.Tag;
				componentListItem = null;
				aListBox.BeginUpdate();

				try
				{
					aListBox.Items.Clear();

					foreach (RowColProfileHeader header in listBoxInfo.List)
					{
						if (header.Profile == null)
						{
							componentListItem = new ListBoxItem(header, header.Name);
							aListBox.Items.Add(componentListItem, header.IsDisplayed);
						}
						else
						{
							aListBox.Items.Add(new ListBoxItem(header, header.Name), header.IsDisplayed);
						}
					}
				}
				finally
				{
					aListBox.EndUpdate();
				}

				listBoxInfo.OrderListBox.BeginUpdate();

				try
				{
					listBoxInfo.OrderListBox.Items.Clear();

					sortList = new SortedList();

					foreach (ListBoxItem item in aListBox.Items)
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
						listBoxInfo.OrderListBox.Items.Add(dictEnum.Value);
					}

					if (componentListItem != null)
					{
						aListBox.Items.Remove(componentListItem);
					}
				}
				finally
				{
					listBoxInfo.OrderListBox.EndUpdate();
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

		private void ClearAll(CheckedListBox aListBox)
		{
			int i;

			try
			{
				for (i = 0; i < aListBox.Items.Count; i++)
				{
					aListBox.SetItemChecked(i, false);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void SelectAll(CheckedListBox aListBox)
		{
			int i;

			try
			{
				for (i = 0; i < aListBox.Items.Count; i++)
				{
					aListBox.SetItemChecked(i, true);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void RestoreDefaults(CheckedListBox aListBox)
		{
			try
			{
				FillListBox(aListBox);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		override public bool ValidateData()
		{
			try
			{
				if (lstComponentOrder.Items.Count == 0)
				{
					MessageBox.Show("You must choose at least one Component to display.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
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
			RowColProfileHeader profComponent;
			RowColProfileHeader lastProf;
			IEnumerator iHeaderEnum;
			IEnumerator iComponentEnum;

			try
			{
				foreach (RowColProfileHeader header in _headers)
				{
					header.Sequence = -1;
					header.IsDisplayed = false;
				}

				iHeaderEnum = lstOtherOrder.Items.GetEnumerator();
				lastProf = null;
				i = 0;

				while (iHeaderEnum.MoveNext())
				{
					profHeader = (RowColProfileHeader)((ListBoxItem)iHeaderEnum.Current).Value;

					if (profHeader.Profile != null)
					{
						profHeader.Sequence = i;
						profHeader.IsDisplayed = true;
						profHeader.IsSummarized = true;

						lastProf = profHeader;
						i++;
					}
					else
					{
						iComponentEnum = lstComponentOrder.Items.GetEnumerator();

						while (iComponentEnum.MoveNext())
						{
							profComponent = (RowColProfileHeader)((ListBoxItem)iComponentEnum.Current).Value;
							profComponent.Sequence = i;
							profComponent.IsDisplayed = true;
							profComponent.IsSummarized = true;

							lastProf = profComponent;
							i++;
						}
					}
				}

				if (lastProf != null)
				{
					lastProf.IsSummarized = false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private class MIDTimer : System.Timers.Timer
		{
			private ListBox _activeListBox;

			public MIDTimer(double aInterval)
				: base(aInterval)
			{
			}

			public ListBox ActiveListBox
			{
				get
				{
					return _activeListBox;
				}
			}

			public void Start(ListBox aActiveListBox)
			{
				_activeListBox = aActiveListBox;
				base.Start();
			}
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

		private class ListBoxInfo
		{
			private ArrayList _list;
			private ListBox _orderListBox;
			private CheckedListBox _componentListBox;

			public ListBoxInfo(ArrayList aList, ListBox aOrderListBox, CheckedListBox aComponentListBox)
			{
				_list = aList;
				_orderListBox = aOrderListBox;
				_componentListBox = aComponentListBox;
			}

			public ArrayList List
			{
				get
				{
					return _list;
				}
			}

			public ListBox OrderListBox
			{
				get
				{
					return _orderListBox;
				}
			}

			public CheckedListBox ComponentListBox
			{
				get
				{
					return _componentListBox;
				}
			}
		}
	}
}
