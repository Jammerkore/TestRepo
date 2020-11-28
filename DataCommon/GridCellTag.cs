using System;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Used to save and retrieve information from the cell tag.
	/// </summary>
	public class GridCellTag
	{
		private string					_message;
		private string					_helpText;
		private object					_GridCellTagData;
		
		public GridCellTag()
		{
			_message = null;
			_helpText = null;
			_GridCellTagData = null;
		}
		
		/// <summary>
		/// Gets or sets the message associated with the cell.
		/// </summary>
		public string Message 
		{
			get { return _message ; }
			set { _message = value; }
		}
		
		/// <summary>
		/// Gets or sets the help text associated with the cell.
		/// </summary>
		public string HelpText 
		{
			get { return _helpText ; }
			set { _helpText = value; }
		}

		/// <summary>
		/// Gets or sets the object containing the data for the cell.
		/// </summary>
		public object GridCellTagData 
		{
			get { return _GridCellTagData ; }
			set { _GridCellTagData = value; }
		}
	}
}
