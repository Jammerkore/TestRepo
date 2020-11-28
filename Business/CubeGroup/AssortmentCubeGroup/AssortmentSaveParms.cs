using System;

namespace MIDRetail.Business
{
	[Serializable]
	public class AssortmentSaveParms
	{
		//=======
		// FIELDS
		//=======

		private bool _saveAssortmentValues;
		private bool _saveView;
		private int _viewUserRID;
		private string _viewName;
		private int _viewRID;

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentSaveParms()
		{
			_saveAssortmentValues = false;
			_saveView = false;
		}

		//===========
		// PROPERTIES
		//===========

		public bool SaveAssortmentValues
		{
			get
			{
				return _saveAssortmentValues;
			}
			set
			{
				_saveAssortmentValues = value;
			}
		}

		public bool SaveView
		{
			get
			{
				return _saveView;
			}
			set
			{
				_saveView = value;
			}
		}

		public int ViewUserRID
		{
			get
			{
				return _viewUserRID;
			}
			set
			{
				_viewUserRID = value;
			}
		}

		public string ViewName
		{
			get
			{
				return _viewName;
			}
			set
			{
				_viewName = value;
			}
		}

		public int ViewRID
		{
			get
			{
				return _viewRID;
			}
			set
			{
				_viewRID = value;
			}
		}

		//========
		// METHODS
		//========
	}
}
