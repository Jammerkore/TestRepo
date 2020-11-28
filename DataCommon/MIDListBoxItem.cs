using System;
using System.Windows.Forms;

using MIDRetail.DataCommon;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Summary description for MIDListBoxItem.
	/// </summary>
	public class MIDListBoxItem
	{
		private int _key;
		private string _value;
		private object _tag;

		public MIDListBoxItem()
		{
			_key = Include.NoRID;
			_value = string.Empty;
			_tag = null;
		}

		public MIDListBoxItem(int aKey)
		{
			_key = aKey;
			_value = string.Empty;
			_tag = null;
		}

		public MIDListBoxItem(string aValue)
		{
			_key = Include.NoRID;
			_value = aValue;
			_tag = null;
		}

		public MIDListBoxItem(string aValue, object aTag)
		{
			_key = Include.NoRID;
			_value = aValue;
			_tag = aTag;
		}

		public MIDListBoxItem(int aKey, string aValue, object aTag)
		{
			_key = aKey;
			_value = aValue;
			_tag = aTag;
		}

		public MIDListBoxItem(int aKey, string aValue)
		{
			_key = aKey;
			_value = aValue;
			_tag = null;
		}

		/// <summary>
		/// Gets or sets the key for the listbox item.
		/// </summary>
		public int Key
		{
			get
			{
				return _key;
			}
		}

		/// <summary>
		/// Gets or sets the text for the listbox item.
		/// </summary>
		public string Value 
		{
			get { return _value ; }
			set { _value = value; }
		}

		/// <summary>
		/// Gets or sets the tag for the listbox item.
		/// </summary>
		public object Tag 
		{
			get { return _tag ; }
			set { _tag = value; }
		}

		/// <summary>
		/// override ToString
		/// </summary>
		/// <returns>string</returns>
		public override string ToString()
		{
			return this.Value;
		}

		public override bool Equals(object obj)
		{
			return (((MIDListBoxItem)obj)._key == _key && ((MIDListBoxItem)obj)._value == _value);
		}

		public override int GetHashCode() 
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
	}
}
