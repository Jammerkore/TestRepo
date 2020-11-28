using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Creates a 2-dimensional dictionary containing 2 integer keys.
	/// </summary>
	public class MID2DimIntDictionary
	{
		private Dictionary<int, Dictionary<int, object>> _dic = new Dictionary<int, Dictionary<int, object>>();

		public MID2DimIntDictionary()
		{
		}

		public object this[int key1, int key2]
		{
			get
			{
				return _dic[key1][key2];
			}
			set
			{
				if (!_dic.ContainsKey(key1))
				{
					_dic[key1] = new Dictionary<int, object>();
				}

				if (_dic[key1].ContainsKey(key2))
				{
					_dic[key1][key2] = value;
				}
				else
				{
					_dic[key1][key2] = new Dictionary<int, object>();
					_dic[key1][key2] = value;
				}
			}
		}

		public bool ContainsKey(int key1, int key2)
		{
			bool containsKey = false;

			try
			{
				object x = this[key1, key2];
				containsKey = true;
			}
			catch (KeyNotFoundException ex)
			{

			}
			catch
			{
				throw;
			}

			return containsKey;
		}

	}

	/// <summary>
	/// Creates a 3-dimensional dictionary containing 3 integer keys.
	/// </summary>
	public class MID3DimIntDictionary
	{
		private Dictionary<int, Dictionary<int, Dictionary<int, object>>> _dic = new Dictionary<int, Dictionary<int, Dictionary<int, object>>>();

		public MID3DimIntDictionary()
		{
		}

		public object this[int key1, int key2, int key3]
		{
			get
			{
				return _dic[key1][key2][key3];
			}
			set
			{
				if (!_dic.ContainsKey(key1))
				{
					_dic[key1] = new Dictionary<int, Dictionary<int, object>>();
				}

				if (!_dic[key1].ContainsKey(key2))
				{
					_dic[key1][key2] = new Dictionary<int, object>();
				}

				if (_dic[key1][key2].ContainsKey(key3))
				{
					_dic[key1][key2][key3] = value;
				}
				else
				{
					_dic[key1][key2][key3] = new Dictionary<int, object>();
					_dic[key1][key2][key3] = value;
				}
			}
		}

		public bool ContainsKey(int key1, int key2, int key3)
		{
			bool containsKey = false;

			try
			{
				object x = this[key1, key2, key3];
				containsKey = true;
			}
			catch (KeyNotFoundException ex)
			{

			}
			catch
			{
				throw;
			}

			return containsKey;
		}

	}

}
