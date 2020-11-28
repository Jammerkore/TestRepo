using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using C1.Win.C1FlexGrid;

using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows.Controls
//namespace MIDFlexGroup
{
	/// <summary>
	/// MIDFlexGroup -- Implements Outlook-style grouping using the C1FlexGrid control
	/// </summary>
	
	public class MIDFlexGroup : PictureBox, ISupportInitialize
	{
		//=======
		// FIELDS
		//=======

		// Default constants

		private const int MARGIN = 8;			// spacing between groups, edges
		private const int SCROLLSTEP = 15;		// scroll step (while dragging mouse)
		private const int DRAGTHRESHOLD = 8;	// pixels before starting column drag
		private const int GROUPHEIGHT = 17;
		private const string GROUP_MSG = "Drag column headers here to create groups";

		// Bitmaps

		private static StringFormat _sf;
		private static string _imageDir;
		private static Bitmap _bmpInsert;		// insert icon
		private static Bitmap _bmpSortUp;		// sort icon
		private static Bitmap _bmpSortDn;		// sort icon

		private int _groupHeight;
		private bool _firstPaint;
		private C1FlexGrid _grid;				// grid control
		private ArrayList _groups;				// list of fields (columns) in the group area
		private DragLabel _dragger;				// aux control to drag fields
		private CellStyle _styleGroup;			// used to paint groups
		private CellStyle _styleEmpty;			// used to paint empty area
		private bool _insGroup;					// whether column being inserted is a group/column 
		private int _insIndex;					// index where group/column should be inserted
		private string _groupMessage;			// message displayed in the empty group area
		private Rectangle _insRect;				// rectangle where insert indicator is drawn
		private Rectangle _insRectLast;			// rectangle where last insert indicator was drawn
		private SolidBrush _backBrush;			// gdi object used for painting group area
		private SolidBrush _foreBrush;			// gdi object used for painting group area
		private SolidBrush _brGrp;				// gdi object used for painting grid
		private SolidBrush _brBdr;				// gdi object used for painting grid

		public event GridChangedEventHandler GridChanged;
		public event RangeEventHandler BeforeRowColChange;
		public event RangeEventHandler AfterRowColChange;
		public event RangeEventHandler BeforeSelChange;
		public event RangeEventHandler AfterSelChange;

		//=============
		// CONSTRUCTORS
		//=============

		public MIDFlexGroup()
		{
			// initialize static members
			if (_sf == null)
			{
				_sf = new StringFormat(StringFormat.GenericDefault);
				_sf.Alignment = _sf.LineAlignment = StringAlignment.Center;
                // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
                //_imageDir = MIDConfigurationManager.AppSettings[Include.MIDApplicationRoot] + MIDGraphics.GraphicsDir;
                _imageDir = MIDGraphics.MIDGraphicsDir;
                // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration
				_bmpInsert = LoadBitmap("InsertPoint", Color.White);
				_bmpSortUp = LoadBitmap("SortUp", Color.Red);
				_bmpSortDn = LoadBitmap("SortDn", Color.Red);
			}

			// initialize contained Flex control
			_firstPaint = true;
			_groupHeight = GROUPHEIGHT;
			_grid = new C1FlexGrid();
			_grid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
			_grid.Dock = DockStyle.Bottom;
			_grid.Size = new Size(10,10);
			_grid.AllowSorting = AllowSortingEnum.None;
			_grid.AllowMerging = AllowMergingEnum.Nodes;
			_grid.Cols[0].Width = _grid.Rows.DefaultSize;
			_grid.ShowCursor = true;
			_grid.Tree.Style = TreeStyleFlags.Symbols;
			_grid.DrawMode = DrawModeEnum.OwnerDraw;
			_grid.Rows.Fixed = 1;
			_grid.BeforeMouseDown += new BeforeMouseDownEventHandler(grid_BeforeMouseDown);
			_grid.KeyPress += new KeyPressEventHandler(grid_KeyPress);
			_grid.AfterResizeColumn += new RowColEventHandler(grid_AfterResizeColumn);
			_grid.OwnerDrawCell += new OwnerDrawCellEventHandler(grid_OwnerDrawCell);
			_grid.AfterDataRefresh += new ListChangedEventHandler(grid_DataChanged);
			_grid.GridChanged += new GridChangedEventHandler(grid_GridChanged);

			// hook up additional event handlers
			_grid.BeforeRowColChange += new RangeEventHandler(grid_BeforeRowColChange);
			_grid.AfterRowColChange  += new RangeEventHandler(grid_AfterRowColChange );
			_grid.BeforeSelChange += new RangeEventHandler(grid_BeforeSelChange);
			_grid.AfterSelChange += new RangeEventHandler(grid_AfterSelChange);

			// Setup grid styles
			_grid.Styles["Normal"].Border.Color = Color.FromKnownColor(KnownColor.Control);
			_grid.Styles["Normal"].Border.Style = BorderStyleEnum.Flat;
			_grid.Styles["Normal"].Border.Width = 1;
			_grid.Styles["Normal"].Border.Direction = BorderDirEnum.Horizontal;

			_grid.Styles["Fixed"].Font = new Font(_grid.Styles["Fixed"].Font, FontStyle.Bold);
			_grid.Styles["Fixed"].BackColor = Color.FromKnownColor(KnownColor.Control);
			_grid.Styles["Fixed"].ForeColor = Color.FromKnownColor(KnownColor.ControlText);
			_grid.Styles["Fixed"].Border.Color = Color.FromKnownColor(KnownColor.ControlDark);
			_grid.Styles["Fixed"].Border.Style = BorderStyleEnum.Flat;
			_grid.Styles["Fixed"].Border.Width = 1;
			_grid.Styles["Fixed"].Border.Direction = BorderDirEnum.Both;
			_grid.Styles["Fixed"].WordWrap = true;

			_grid.Styles["Highlight"].BackColor = Color.FromKnownColor(KnownColor.Highlight);
			_grid.Styles["Highlight"].ForeColor = Color.FromKnownColor(KnownColor.HighlightText);

			_grid.Styles["Search"].BackColor = Color.FromKnownColor(KnownColor.Highlight);
			_grid.Styles["Search"].ForeColor = Color.FromKnownColor(KnownColor.HighlightText);

			_grid.Styles["Frozen"].BackColor = Color.FromKnownColor(KnownColor.Beige);

			_grid.Styles["EmptyArea"].BackColor = Color.FromKnownColor(KnownColor.AppWorkspace);
			_grid.Styles["EmptyArea"].Border.Color = Color.FromKnownColor(KnownColor.ControlDarkDark);
			_grid.Styles["EmptyArea"].Border.Style = BorderStyleEnum.Flat;
			_grid.Styles["EmptyArea"].Border.Width = 1;
			_grid.Styles["EmptyArea"].Border.Direction = BorderDirEnum.Both;

			_grid.Styles["GrandTotal"].BackColor = Color.FromKnownColor(KnownColor.Black);
			_grid.Styles["GrandTotal"].ForeColor = Color.FromKnownColor(KnownColor.White);

			_grid.Styles["Subtotal0"].BackColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
			_grid.Styles["Subtotal0"].ForeColor = Color.FromKnownColor(KnownColor.White);

			_grid.Styles["Subtotal1"].BackColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
			_grid.Styles["Subtotal1"].ForeColor = Color.FromKnownColor(KnownColor.White);

			_grid.Styles["Subtotal2"].BackColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
			_grid.Styles["Subtotal2"].ForeColor = Color.FromKnownColor(KnownColor.White);

			_grid.Styles["Subtotal3"].BackColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
			_grid.Styles["Subtotal3"].ForeColor = Color.FromKnownColor(KnownColor.White);

			_grid.Styles["Subtotal4"].BackColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
			_grid.Styles["Subtotal4"].ForeColor = Color.FromKnownColor(KnownColor.White);

			_grid.Styles["Subtotal5"].BackColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
			_grid.Styles["Subtotal5"].ForeColor = Color.FromKnownColor(KnownColor.White);

			_grid.Styles.Add("Group");
			_grid.Styles["Group"].Font = new Font(_grid.Styles["Group"].Font, FontStyle.Bold);
			_grid.Styles["Group"].BackColor = Color.FromKnownColor(KnownColor.Control);
			_grid.Styles["Group"].ForeColor = Color.FromKnownColor(KnownColor.ControlText);
			_grid.Styles["Group"].Border.Color = Color.FromKnownColor(KnownColor.ControlDark);
			_grid.Styles["Group"].Border.Style = BorderStyleEnum.Flat;
			_grid.Styles["Group"].Border.Width = 1;
			_grid.Styles["Group"].Border.Direction = BorderDirEnum.Both;

			_grid.Styles.Add("Empty");
			_grid.Styles["Empty"].BackColor = Color.FromKnownColor(KnownColor.ControlDark);
			_grid.Styles["Empty"].ForeColor = Color.FromKnownColor(KnownColor.ControlLightLight);
			_grid.Styles["Empty"].Border.Color = Color.FromKnownColor(KnownColor.ControlDarkDark);
			_grid.Styles["Empty"].Border.Style = BorderStyleEnum.Flat;
			_grid.Styles["Empty"].Border.Width = 1;
			_grid.Styles["Empty"].Border.Direction = BorderDirEnum.Both;

			// initialize styles
			_grid.Styles.Normal.Border.Direction = BorderDirEnum.Horizontal;
			_styleGroup = _grid.Styles.Add("Group", _grid.Styles.Fixed);
			_styleEmpty = _grid.Styles.Add("Empty", _grid.Styles.EmptyArea);
			_styleEmpty.BackColor = SystemColors.ControlDarkDark;
			_styleEmpty.ForeColor = SystemColors.ControlLightLight;

			// initialize internal members
			_groupMessage = GROUP_MSG;
			_groups = new ArrayList();
			_insIndex = -1;

			// initialize field dragger control
			_dragger = new DragLabel(this);

			// initialize parent control
			SuspendLayout();
			BorderStyle = BorderStyle.Fixed3D;
			BackColor = SystemColors.ControlDark;
			ForeColor = SystemColors.ControlLightLight;
			Controls.AddRange(new Control[] { _dragger, _grid });

			ResumeLayout(false);
		}

		//===========
		// PROPERTIES
		//===========

		[
		Description("Gets or sets the height of the Group headers."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content) 
		]
		public int GroupHeight
		{
			get
			{
				return _groupHeight;
			}
			set
			{
				_groupHeight = value;
			}
		}

		[
		Description("Gets or sets the properties of the internal FlexGrid."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content) 
		]
		public C1FlexGrid Grid
		{
			get
			{
				return _grid;
			}
		}

		public CellStyle StyleGroupRows
		{
			get
			{
				return _styleGroup;
			}
		}

		public CellStyle StyleGroupArea
		{
			get
			{
				return _styleEmpty;
			}
		}

		new public Image Image
		{
			get
			{
				return base.Image;
			}
			set
			{
				base.Image = value;
			}
		}

		public string GroupMessage
		{
			get
			{
				return _groupMessage;
			}
			set
			{
				_groupMessage = value; 
				Invalidate();
			}
		}

		public string Groups
		{
			get
			{
				StringBuilder sb;
				int i;

				try
				{
					// build string with column names
					sb = new StringBuilder();

					for (i = 0; i < _groups.Count; i++)
					{
						if (i > 0)
						{
							sb.Append(", ");
						}

						sb.Append(((ColumnData)_groups[i]).Column.Name);
					}

					return sb.ToString();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set 
			{
				string[] colNames;
				Column col;

				try
				{
					// make current group columns visible
					_grid.Redraw = false;

					foreach (ColumnData colData in _groups)
					{
						colData.Column.Visible = true;
					}

					// rebuild _groups collection
					_groups.Clear();

					if (value.Length > 0)
					{
						colNames = value.Split(',');

						foreach (string colName in colNames)
						{
							col = _grid.Cols[colName.Trim()];

							if (col != null)
							{
								_groups.Add(col.UserData);
							}
						}
					}

					// apply new collection
					UpdateGroups();
					UpdateLayout();

					// done
					_grid.Redraw = true;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

		void ISupportInitialize.BeginInit()
		{
			_grid.BeginInit();

			this.Controls.Add(_grid);
			_grid.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Nodes;
			_grid.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
			_grid.BackColor = System.Drawing.SystemColors.Window;
			_grid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
			_grid.Cursor = System.Windows.Forms.Cursors.Default;
			_grid.Dock = System.Windows.Forms.DockStyle.Bottom;
			_grid.DrawMode = C1.Win.C1FlexGrid.DrawModeEnum.OwnerDraw;
			_grid.ForeColor = System.Drawing.SystemColors.WindowText;
			_grid.Location = new System.Drawing.Point(0, 38);
			_grid.ShowCursor = true;
			
			_grid.TabIndex = 1;
			_grid.Tree.Style = C1.Win.C1FlexGrid.TreeStyleFlags.Symbols;
		}

		void ISupportInitialize.EndInit()
		{
			// don't call EndInit without BeginInit first <<B4>>
			_grid.BeginInit();
			_grid.EndInit();

			// flex has re-created the styles, 
			// so get a fresh reference to the custom ones we'll use
			_styleGroup = _grid.Styles["Group"];
			_styleEmpty = _grid.Styles["Empty"];

			// make sure grid is visible <<B4>>
			_grid.Visible = true;
		}

		override protected void OnSizeChanged(EventArgs e)
		{
			try
			{
				UpdateLayout();
				base.OnSizeChanged(e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		override protected void OnMouseDown(MouseEventArgs e)
		{
			Rectangle rect;

			try
			{
				if ((e.Button & MouseButtons.Left) != 0)
				{
					foreach (ColumnData colData in _groups)
					{
						rect = GetGroupRectangle(colData);

						if (rect.Contains(e.X, e.Y))
						{
							_dragger.StartDragging(colData);
							return;
						}
					}
				}

				base.OnMouseDown(e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		override protected void OnPaint(PaintEventArgs e)
		{
			Color color;
			Rectangle rect;

			try
			{
				if (_firstPaint)
				{
					InitializeControl();
					_firstPaint = false;
				}

				// update brushes
				color = _styleEmpty.BackColor;
				if (_backBrush == null || _backBrush.Color != color)
				{
					_backBrush = new SolidBrush(color);
				}

				color = _styleEmpty.ForeColor;
				if (_foreBrush == null || _foreBrush.Color != color)
				{
					_foreBrush = new SolidBrush(color);
				}

				color = _styleGroup.BackColor;
				if (_brGrp == null || _brGrp.Color != color)
				{
					_brGrp = new SolidBrush(color);
				}
			
				color = _styleGroup.Border.Color;
				if (_brBdr == null || _brBdr.Color != color)
				{
					_brBdr = new SolidBrush(color);
				}

				// get group area
				rect = ClientRectangle;
				rect.Height = _grid.Top;

				// draw background
				e.Graphics.FillRectangle(_backBrush, rect);

				if (_groups.Count == 0)
				{
					e.Graphics.DrawString(_groupMessage, _styleEmpty.Font, _foreBrush, rect, _sf);
				}
				else
				{
					foreach (ColumnData colData in _groups)
					{
						rect = GetGroupRectangle(colData);
						PaintGroup(e.Graphics, rect, colData);
					}
				}

				// show insert position while dragging
				if (_dragger.Visible)
				{
					DrawImageCentered(e.Graphics, _bmpInsert, _insRect);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void HandleExceptions(System.Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		private void InitializeControl()
		{
			try
			{
				foreach (Column col in _grid.Cols)
				{
					col.UserData = new ColumnData(this, col, _groupHeight, _dragger.Font);
				}

				UpdateGroups();
				UpdateLayout();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void grid_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				if (_dragger.Visible)
				{
					_dragger.Visible = false;
					Invalidate();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_BeforeMouseDown(object sender, BeforeMouseDownEventArgs e)
		{
			HitTestInfo hti;
			ColumnCollection cols;

			try
			{
				if ((e.Button & MouseButtons.Left) == 0)
				{
					return;
				}
			
				hti = _grid.HitTest(e.X, e.Y);
				if (hti.Type != HitTestTypeEnum.ColumnHeader)
				{
					return;
				}

				// check that the click was on the first row
				// (in case there's additional fixed rows)
				if (hti.Row > 0)
				{
					return;
				}

				// check that the click was on a scrollable column
				cols = _grid.Cols;
				if (hti.Column < cols.Fixed)
				{
					return;
				}

				e.Cancel = true;

				// check that we have at least one non-grouped column
				if (_groups.Count >= cols.Count - cols.Fixed - 1)
				{
					return;
				}

				_dragger.StartDragging(cols[hti.Column]);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_AfterResizeColumn(object sender, RowColEventArgs e)
		{
			try
			{
				UpdateLayout();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			Row row;
			int idt;
			int x;
			int lvl;
			Rectangle rect;
			int i;

			try
			{
				if (_groups.Count == 0)
				{
					return;
				}

				if (e.Col != _grid.Tree.Column)
				{
					return;
				}

				if (e.Row < _grid.Rows.Fixed)
				{
					return;
				}

				row = _grid.Rows[e.Row];

				if (row.Node == null)
				{
					return;
				}

				idt = _grid.Tree.Indent;
				x = _grid.ScrollPosition.X;
				lvl = row.Node.Level;

				// custom draw nodes
				if (row.IsNode)
				{
					// draw background and content
					e.Style = _styleGroup;
					e.DrawCell(DrawCellFlags.Background | DrawCellFlags.Content);

					// draw line above
					if (lvl == 0 || !_grid.Rows[e.Row-1].IsNode)
					{
						rect = e.Bounds;
						OffsetLeft(ref rect, lvl * idt + x);
						rect.Height = 1;
						e.Graphics.FillRectangle(_brBdr, rect);
					}

					// if the node is expanded, draw line below
					if (row.Node.Expanded)
					{
						rect = e.Bounds;
						OffsetLeft(ref rect, (lvl+1) * idt + x);
						rect.Y = rect.Bottom-1;
						rect.Height = 1;
						e.Graphics.FillRectangle(_brBdr, rect);
					}

					// draw vertical lines to the left of the symbol
					rect = e.Bounds;
					rect.X += x;
					rect.Width = 1;

					for (i = 0; i <= lvl; i++)
					{
						e.Graphics.FillRectangle(_brBdr, rect);
						rect.Offset(idt, 0);
					}
				}
				else
				{
					e.DrawCell();

					// fill area on the left
					rect = e.Bounds;
					rect.Width = (lvl+1) * idt;
					e.Graphics.FillRectangle(_brGrp, rect);

					// draw vertical lines over filled area
					rect = e.Bounds;
					rect.Width = 1;

					for (i = 0; i <= lvl+1; i++)
					{
						e.Graphics.FillRectangle(_brBdr, rect);
						rect.Offset(idt, 0);
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_GridChanged(object sender, GridChangedEventArgs e)
		{
			try
			{
				if (e.GridChangedType == GridChangedTypeEnum.StyleChanged ||
					e.GridChangedType == GridChangedTypeEnum.RepaintGrid)
				{
					Invalidate();
				}

				if (GridChanged != null)
				{
					GridChanged(this, e);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}		

		private void grid_BeforeRowColChange(object sender, RangeEventArgs e)
		{
			try
			{
				if (BeforeRowColChange != null)
				{
					BeforeRowColChange(this, e);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_AfterRowColChange(object sender, RangeEventArgs e)
		{
			try
			{
				if (AfterRowColChange != null)
				{
					AfterRowColChange(this, e);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_BeforeSelChange(object sender, RangeEventArgs e)
		{
			try
			{
				if (BeforeSelChange != null)
				{
					BeforeSelChange(this, e);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_AfterSelChange(object sender, RangeEventArgs e)
		{
			try
			{
				if (AfterSelChange != null)
				{
					AfterSelChange(this, e);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_DataChanged(object sender, ListChangedEventArgs e)
		{
			try
			{
				if (e.ListChangedType == ListChangedType.Reset)
				{
					UpdateGroups();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}
		
		private void OffsetLeft(ref Rectangle aRect, int aXOffset)
		{
			aRect.X += aXOffset;
			aRect.Width -= aXOffset;
		}

		private void UpdateLayout()
		{
			int height = 0;
			int i;

			try
			{
				if (_groups.Count > 0)
				{
					height = GetGroupRectangle((ColumnData)_groups[_groups.Count - 1]).Bottom + MARGIN;
				}
				else
				{
					height = 2 * _grid.Rows.DefaultSize;
				}

				_grid.Height = ClientRectangle.Height - height;

				for (i = 0; i < _grid.Rows.Fixed; i++)
				{
					_grid.AutoSizeRow(i);
				}

				Invalidate();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void UpdateGroups()
		{
			int i;
			int colIdx;
			string fmt;

			try
			{
				_grid.Redraw = false;
				_grid.Subtotal(AggregateEnum.Clear);

				_grid.Tree.Column = _grid.Cols.Fixed + _groups.Count;
				_grid.Cols[_grid.Tree.Column].TextAlign = TextAlignEnum.LeftCenter;

				for (i = 0; i < _groups.Count; i++)
				{
					colIdx = i + _grid.Cols.Fixed;
					//					fmt = _grid.Cols[colIdx].Caption + ": {0}";
					//					_grid.Subtotal(AggregateEnum.None, i, colIdx, 0, fmt);
					fmt = "{0}";
					_grid.Subtotal(AggregateEnum.None, i, colIdx, 0, fmt);
				}

				_grid.Redraw = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void UpdateInsertLocation()
		{
			Point mousePos;
			Rectangle rect;
			int i;
			int insIdx;

			try
			{
				mousePos = PointToClient(Control.MousePosition);

				_insRect = Rectangle.Empty;
				_insIndex = -1;
			
				// insert into group list
				if (GetMouseArea(mousePos) == MouseArea.Group)
				{
					// find position where new group should be inserted
					insIdx = _groups.Count;

					for (i = 0; i < _groups.Count; i++)
					{
						rect = GetGroupRectangle((ColumnData)_groups[i]);

						if (rect.X + rect.Width/2 > mousePos.X)
						{
							insIdx = i;
							break;
						}
					}

					_insGroup = true;
					_insIndex = insIdx;

					if (insIdx < _groups.Count)
					{
						_insRect = GetGroupRectangle((ColumnData)_groups[insIdx]);
						_insRect.X -= MARGIN;
					}
					else if (insIdx > 0)
					{
						_insRect = GetGroupRectangle((ColumnData)_groups[_groups.Count - 1]);
						_insRect.X = _insRect.Right;
					}
					else
					{
						_insRect = new Rectangle(MARGIN, MARGIN, 0, _groupHeight);
					}

					if (insIdx > 0 && insIdx < _groups.Count)
					{
						_insRect.Y -= _insRect.Height/2;
						_insRect.Height += _insRect.Height/2;
					}

					_insRect.Width = MARGIN;
				}
				else // remove from group list (insert into grid)
				{
					// find position where grid column should be inserted
					insIdx = _grid.Cols.Count;

					for (i = _grid.Cols.Fixed; i < _grid.Cols.Count; i++)
					{
						rect = _grid.GetCellRect(0, i, false);

						if (rect.X + rect.Width/2 > mousePos.X)
						{
							insIdx = i;
							break;
						}
					}

					_insGroup = false;
					_insIndex = insIdx;

					if (insIdx < _grid.Cols.Count)
					{
						_insRect = _grid.GetCellRect(0, insIdx, false);
						_insRect.Width = 0;
					}
					else
					{
						_insRect = _grid.GetCellRect(0, insIdx - 1, false);
						_insRect.X = _insRect.Right;
						_insRect.Width = 0;
					}

					_insRect.Inflate(MARGIN / 2, 5);
					_insRect.Offset(0, _grid.Top);
				}

				if (_insRect != _insRectLast)
				{
					Invalidate();
					_insRectLast = _insRect;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void FinishDragging(Column aCol, bool aDragged)
		{
			int oldIndex;
			int newIndex;
			int i;

			try
			{
				if (!aDragged)
				{
					if (_groups.Contains(new ColumnData(this, aCol)))
					{
						if ((aCol.Sort & SortFlags.Ascending) != 0)
						{
							aCol.Sort = SortFlags.Descending;
						}
						else
						{
							aCol.Sort = SortFlags.Ascending;
						}

						_grid.Sort(SortFlags.UseColSort, ((ColumnData)_groups[0]).Column.Index, ((ColumnData)_groups[_groups.Count - 1]).Column.Index);
					}
					else if (_groups.Count == 0)
					{
						ClearRemainingColumnSortFlags(aCol);

						if ((aCol.Sort & SortFlags.Ascending) != 0)
						{
							aCol.Sort = SortFlags.Descending;
						}
						else if ((aCol.Sort & SortFlags.Descending) != 0)
						{
							aCol.Sort = SortFlags.None;
						}
						else
						{
							aCol.Sort = SortFlags.Ascending;
						}

						_grid.Sort(aCol.Sort, aCol.Index);
					}
				}
				else if (_insGroup)		// insert column into group collection
				{
					if (_groups.Count == 0)
					{
						ClearRemainingColumnSortFlags(aCol);
					}

					// add group to list at the proper position (col->grp, grp->grp)
					_groups.Insert(_insIndex, aCol.UserData);

					for (i = 0; i < _groups.Count; i++)
					{
						if (i != _insIndex && ((ColumnData)_groups[i]).Column == aCol)
						{
							_groups.RemoveAt(i);
							break;
						}
					}

					_grid.Cols.Move(aCol.Index, _groups.IndexOf(new ColumnData(this, aCol)) + _grid.Cols.Fixed);
					aCol.Visible = false;

					if ((aCol.Sort & (SortFlags.Ascending | SortFlags.Descending)) == 0)
					{
						aCol.Sort = SortFlags.Ascending;
					}

					_grid.Sort(SortFlags.UseColSort, ((ColumnData)_groups[0]).Column.Index, ((ColumnData)_groups[_groups.Count - 1]).Column.Index);
				}
				else		// insert column into grid
				{
					// move column to a new position (col->col, grp->col)
					oldIndex = aCol.Index;
					newIndex = _insIndex;

					if (newIndex > oldIndex)
					{
						newIndex--;
					}

					_grid.Cols.Move(oldIndex, newIndex);
					aCol.Visible = true;

					oldIndex = _groups.IndexOf(new ColumnData(this, aCol));
					if (oldIndex >= 0)
					{
						_groups.RemoveAt(oldIndex);
						aCol.Sort = SortFlags.None;
					}
				}

				UpdateGroups();
				UpdateLayout();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private MouseArea GetMouseArea(Point aMousePos)
		{
			try
			{
				if (aMousePos.Y < _grid.Top)
				{
					return MouseArea.Group;
				}
				else
				{
					return MouseArea.Grid;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void ClearRemainingColumnSortFlags(Column aCol)
		{
			try
			{
				foreach (Column col in Grid.Cols)
				{
					if (col != aCol)
					{
						col.Sort = SortFlags.None;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private Rectangle GetGroupRectangle(ColumnData aColData)
		{
			Rectangle rect;
			
			try
			{
				rect = new Rectangle(MARGIN, MARGIN, 0, 0);

				rect.Width = aColData.GroupWidth;
				rect.Height = aColData.GroupHeight;

				foreach (ColumnData colData in _groups)
				{
					if (colData != aColData)
					{
						rect.X += colData.GroupWidth + MARGIN;
						rect.Y += rect.Height / 2;
					}
					else
					{
						break;
					}
				}

				return rect;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void DrawImageCentered(Graphics aGrapics, Image aImage, Rectangle aRect)
		{
			Size size;

			try
			{
				size = aImage.Size;
				aRect.Offset((aRect.Width - size.Width)/2, (aRect.Height - size.Height)/2);
				aRect.Size = size;
				aGrapics.DrawImageUnscaled(aImage, aRect);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private Bitmap LoadBitmap(string aName, Color aTransparent)
		{
			string file;
			Bitmap bmp;

			try
			{
				file = _imageDir + "\\" + aName + ".bmp";
				if (System.IO.File.Exists(file))
				{
					bmp = new Bitmap(_imageDir + "\\" + aName + ".bmp");
					bmp.MakeTransparent(aTransparent);
				}
				else
				{
					bmp = null;
				}

				return bmp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
//		private Bitmap LoadBitmap(string aName, Color aTransparent)
//		{
//			Assembly assmbly;
//			string rsrcName;
//			Bitmap bmp;
//
//			try
//			{
//				assmbly = Assembly.GetExecutingAssembly();
//
//				// find resource by name
//				aName += ".bmp";
//				rsrcName = null;
//
//				foreach (string rsrc in assmbly.GetManifestResourceNames())
//				{
//					if (rsrc.EndsWith(aName))
//					{
//						rsrcName = rsrc;
//						break;
//					}
//				}
//
//				bmp = new Bitmap(assmbly.GetManifestResourceStream(rsrcName));
//				bmp.MakeTransparent(aTransparent);
//
//				return bmp;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}

		private void PaintGroup(Graphics aGraphics, Rectangle aRect, ColumnData aColData)
		{
			Image img;

			try
			{
				_dragger.PaintControl(aGraphics, aRect, aColData.GroupText, false);

				if (_grid.ShowSort)
				{
					if ((aColData.Column.Sort & SortFlags.Ascending) != 0)
					{
						img = _bmpSortUp;
					}
					else if ((aColData.Column.Sort & SortFlags.Descending) != 0)
					{
						img = _bmpSortDn;
					}
					else
					{
						img = null;
					}

					if (img != null)
					{
						aRect.X = aRect.Right - aRect.Height;
						aRect.Width = aRect.Height;
						DrawImageCentered(aGraphics, img, aRect);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private enum MouseArea
		{
			Group,
			Grid
		}

		private class ColumnData
		{
			MIDFlexGroup _owner;
			private Column _col;
			private int _groupWidth;
			private int _groupHeight;
			private string _groupText;
			private int _visColWidth;

			internal ColumnData(MIDFlexGroup aOwner, Column aCol)
			{
				Label label;

				try
				{
					_owner = aOwner;
					_col = aCol;

					_groupHeight = _owner.GroupHeight;

					_groupText = _col.Caption.Replace('\n', ' ');

					label = new Label();
					label.AutoSize = true;
					label.Font = _owner._dragger.Font;
					label.Text = _groupText;

					_groupWidth = label.Width + _groupHeight;

					_visColWidth = aCol.WidthDisplay;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			internal ColumnData(MIDFlexGroup aOwner, Column aCol, int aHeight, Font aFont)
			{
				Label label;

				try
				{
					_owner = aOwner;
					_col = aCol;

					_groupHeight = aHeight;
					_groupText = _col.Caption.Replace('\n', ' ');

					label = new Label();
					label.AutoSize = true;
					label.Font = aFont;
					label.Text = _groupText;

					_groupWidth = label.Width + _groupHeight;
					_visColWidth = aCol.WidthDisplay;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			public override bool Equals(object obj)
			{
				return _col.Index == ((ColumnData)obj).Column.Index;
			}

			public override int GetHashCode()
			{
				return _col.Index;
			}

			internal Column Column
			{
				get
				{
					return _col;
				}
			}

			internal int GroupWidth
			{
				get
				{
					return _groupWidth;
				}
			}

			internal int GroupHeight
			{
				get
				{
					return _groupHeight;
				}
			}

			internal string GroupText
			{
				get
				{
					return _groupText;
				}
			}

			internal int VisibleColumnWidth
			{
				get
				{
					return _visColWidth;
				}
			}
		}

		/// <summary>
		/// DragLabel -- Private class used to implement column dragging
		/// </summary>
		
		private class DragLabel : Label
		{
			//=======
			// FIELDS
			//=======

			private static StringFormat _sf;
			private static Pen _darkPen;
			private static Pen _litePen;

			private MIDFlexGroup _owner;
			private Column _column;
			private Point _offset;
			private Point _startMousePos;
			private bool _dragging;
			private Rectangle _rectClip;
			private SolidBrush _backBrush;
			private SolidBrush _foreBrush;
			private SolidBrush _dragBrush;

			private string _groupText;
			private string _colText;
			private Size _groupSize;
			private Size _colSize;

			//=============
			// CONSTRUCTORS
			//=============

			internal DragLabel(MIDFlexGroup aOwner)
			{
				try
				{
					if (_sf == null)
					{
						_sf = new StringFormat(StringFormat.GenericDefault);
						_sf.Alignment = StringAlignment.Near;
						_sf.LineAlignment = StringAlignment.Center;
						_sf.FormatFlags |= StringFormatFlags.NoWrap;
						_darkPen = SystemPens.ControlDark;
						_litePen = SystemPens.ControlLightLight;
					}

					_owner  = aOwner;
					Visible = false;
					BackColor = Color.Transparent;
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

			override protected void OnPaint(PaintEventArgs e)
			{
				try
				{
					if (_owner.GetMouseArea(_owner.PointToClient(Control.MousePosition)) == MouseArea.Group)
					{
						Size = _groupSize;
						PaintControl(e.Graphics, ClientRectangle, _groupText, true);
					}
					else
					{
						Size = _colSize;
						PaintControl(e.Graphics, ClientRectangle, _colText, true);
					}
				}
				catch (Exception exc)
				{
					_owner.HandleExceptions(exc);
				}
			}

			override protected void OnMouseMove(MouseEventArgs e)
			{
				Point currMousePos;
				Point currPos;
				Point newPos;
				Point currScrollPos;
				Point newScrollPos;
				Rectangle rect;
				C1FlexGrid grid;
				bool scroll;

				try
				{
					// drag while the left button is down
					if ((e.Button & MouseButtons.Left) == 0)
					{
						return;
					}

					// make sure the mouse moved at least a little
					if (!_dragging)
					{
						currMousePos = Control.MousePosition;

						if (Math.Abs(currMousePos.X - _startMousePos.X) < DRAGTHRESHOLD &&
							Math.Abs(currMousePos.Y - _startMousePos.Y) < DRAGTHRESHOLD)
						{
							return;
						}

						_dragging = true;
					}

					// calculate new position for the control
					currPos = _owner.PointToClient(Control.MousePosition);
					newPos = currPos;
					newPos.Offset(-_offset.X, -_offset.Y);

					// clip to grouping area, scroll
					rect = _rectClip;
					grid = _owner.Grid;
					newScrollPos = grid.ScrollPosition;

					if (newPos.X + Width > rect.Right)
					{
						newPos.X = rect.Right - Width;
						newScrollPos.X -= SCROLLSTEP;
					}

					if (newPos.X < 0)
					{
						newPos.X = 0;
						newScrollPos.X += SCROLLSTEP;
					}

					if (newPos.Y + Height > rect.Bottom)
					{
						newPos.Y = rect.Bottom - Height;
					}

					if (newPos.Y < 0)
					{
						newPos.Y = 0;
					}

					// move dragger control
					if (Location != newPos)
					{
						Location = newPos;
					}

					// scroll grid
					scroll = false;

					if (currPos.Y >= _owner.Grid.Top && grid.ScrollPosition != newScrollPos)
					{
						currScrollPos = grid.ScrollPosition;
						grid.ScrollPosition = newScrollPos;
						scroll = (grid.ScrollPosition != currScrollPos);
					}

					// update insert location (after scrolling grid)
					_owner.UpdateInsertLocation();

					// keep scrolling
					if (scroll) 
					{
						_owner.Update();
						OnMouseMove(e);
					}
				}
				catch (Exception exc)
				{
					_owner.HandleExceptions(exc);
				}
			}

			override protected void OnMouseUp(MouseEventArgs e)
			{
				try
				{
					if ((Control.MouseButtons & MouseButtons.Left) != 0)
					{
						return;
					}

					if (!Visible)
					{
						return;
					}

					Visible = false;
					_owner.FinishDragging(_column, _dragging);
				}
				catch (Exception exc)
				{
					_owner.HandleExceptions(exc);
				}
			}

			override protected void OnLeave(EventArgs e)
			{
				Visible = false;
			}

			internal void StartDragging(Column aCol)
			{
				Rectangle rect;

				try
				{
					rect = _owner.Grid.GetCellRect(0, aCol.Index, false);
					rect.Width = aCol.WidthDisplay;
					rect = _owner.Grid.RectangleToScreen(rect);
					rect = _owner.RectangleToClient(rect);

					_colSize = new Size(rect.Width, rect.Height);
					_groupSize = new Size(System.Math.Max(rect.Width, ((ColumnData)aCol.UserData).GroupWidth), ((ColumnData)aCol.UserData).GroupHeight);
					_colText = aCol.Caption;
					_groupText = ((ColumnData)aCol.UserData).GroupText;

					StartDragging(aCol, rect);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			internal void StartDragging(ColumnData aColData)
			{
				Rectangle rect;

				try
				{
					rect = _owner.Grid.GetCellRect(0, aColData.Column.Index, false);
					rect = _owner.Grid.RectangleToScreen(rect);
					rect = _owner.RectangleToClient(rect);

					_colSize = new Size(aColData.GroupWidth, rect.Height);
					_groupSize = new Size(aColData.GroupWidth, aColData.GroupHeight);
					_colText = aColData.GroupText;
					_groupText = aColData.GroupText;


					StartDragging(aColData.Column, _owner.GetGroupRectangle(aColData));
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			private void StartDragging(Column aCol, Rectangle aRect)
			{
				C1FlexGrid grid;

				try
				{
					grid = _owner.Grid;
					_column = aCol;

					// initialize position/visibility
					Location = new Point(aRect.X, aRect.Y);
					Visible = true;

					// calculate clip rectangle
					_rectClip = _owner.ClientRectangle;
					_rectClip.Height = grid.Top + grid.Rows[0].HeightDisplay;

					// keep track of the mouse position
					_startMousePos = Control.MousePosition;
					_offset = PointToClient(_startMousePos);
					_dragging = false;

					// capture mouse to track MouseMove event
					Capture = true;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			internal void PaintControl(Graphics aGraphics, Rectangle aRect, string aText, bool aDragging)
			{
				CellStyle cs;
				Color color;

				try
				{
					// update brushes
					cs = _owner.Grid.Styles["Group"];
					color = cs.BackColor;

					if (_backBrush == null || _backBrush.Color != color)
					{
						_backBrush = new SolidBrush(color);
						_dragBrush = new SolidBrush(Color.FromArgb(100, color));
					}

					color = cs.ForeColor;

					if (_foreBrush == null || _foreBrush.Color != color)
					{
						_foreBrush = new SolidBrush(color);
					}

					Font = cs.Font;

					if (aDragging)
					{
						aGraphics.FillRectangle(_dragBrush, aRect);
					}
					else
					{
						aGraphics.FillRectangle(_backBrush, aRect);
					}

					// paint control
					aGraphics.DrawString(aText, Font, _foreBrush, aRect, _sf);

					// paint border 
					// note: ControlPaint.DrawBorder3D is not good with transparent stuff
					aRect.Width--;
					aRect.Height--;

					aGraphics.DrawLine(_darkPen, aRect.Left + 1, aRect.Bottom, aRect.Right, aRect.Bottom);
					aGraphics.DrawLine(_darkPen, aRect.Right, aRect.Bottom, aRect.Right, aRect.Top + 1);
					aGraphics.DrawLine(_litePen, aRect.Left, aRect.Bottom - 1, aRect.Left, aRect.Top);
					aGraphics.DrawLine(_litePen, aRect.Left, aRect.Top, aRect.Right - 1, aRect.Top);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	}
}