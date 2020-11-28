using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;

namespace MIDRetail.Windows.Controls
{
	public delegate object GetMergeData(int row, int col);

	public partial class MIDFlexGrid : C1.Win.C1FlexGrid.C1FlexGrid
	{
		private bool _doingMerge;
		private GetMergeData _getMergeData;

		public MIDFlexGrid()
		{
			InitializeComponent();

			_doingMerge = false;
		}

		public GetMergeData GetMergeData
		{
			set
			{
				_getMergeData = value;
			}
		}

		public override CellRange GetMergedRange(int row, int col, bool clip)
		{
			_doingMerge = true;
			CellRange cellRange = base.GetMergedRange(row, col, clip);
			_doingMerge = false;

			return cellRange;
		}

		public override Object GetData(int row, int col)
		{
			string data;
			int i;

			if (_doingMerge && _getMergeData != null)
			{
				data = "";

				for (i = 0; i < col; i++)
				{
					data += base.GetDataDisplay(row, i);
				}

				data += base.GetDataDisplay(row, col);

				return data;
			}
			else
			{
				return base.GetData(row, col);
			}
		}
	}
}

