using System;
using System.Drawing;
using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
	/// <summary>
	/// Summary description for DataGridIconTextColumn.
	/// </summary>
	public class DataGridIconTextColumn : DataGridTextBoxColumn
	{
		private ImageList _icons;
		delegateGetIconIndexForRow _getIconIndex;
		
		public DataGridIconTextColumn(ImageList Icons, delegateGetIconIndexForRow getIconIndex)
		{
			_icons = Icons;
			_getIconIndex = getIconIndex;
		}

		protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Brush backBrush, System.Drawing.Brush foreBrush, bool alignToRight)
		{
			try
			{
				Image icon1 = this._icons.Images[_getIconIndex(rowNum)];
				Rectangle rect = new Rectangle(bounds.X, bounds.Y, icon1.Size.Width, bounds.Height);
				g.FillRectangle(backBrush, rect);
				g.DrawImage(icon1, rect);

				bounds.X = bounds.X + rect.Width;
				bounds.Width = bounds.Width - rect.Width;
				base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
			}
			
			catch(Exception ex)
			{
				string exceptionMessage = ex.Message;
			}
		}
	}

	public delegate int delegateGetIconIndexForRow(int row);
}
