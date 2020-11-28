using System;
using System.Collections;
using System.Collections.Generic;

namespace MIDRetail.Common
{
	/// <summary>
	/// Provides a keyable cache area for objects.
	/// </summary>

	public class MIDCache<TKey, TValue>
	{
		//=======
		// FIELDS
		//=======

		private int _numEntries;
		private int _expireGroupSize;
		private Dictionary<TKey, CacheObject<TKey, TValue>> _cacheHash;
		private MIDLinkedList<TKey> _cacheLinkedList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of MIDCache that will hold the given number of entries.
		/// </summary>
		/// <param name="numEntries">
		/// The number of entries that the new MIDCache will hold.  The oldest cached entries will roll off first.
		/// </param>

		public MIDCache(int numEntries, int expireGroupSize)
		{
			try
			{
				if (numEntries <= 0)
				{
					throw new ArgumentException("Value must be greater than 0.", "numEntries");
				}

				if (expireGroupSize <= 0 || expireGroupSize >= numEntries)
				{
					throw new ArgumentException("Value must be greater than 0 and less that numEntries.", "expireGroupSize");
				}

				_numEntries = numEntries;
				_expireGroupSize = expireGroupSize;
				_cacheHash = new Dictionary<TKey, CacheObject<TKey, TValue>>();
				_cacheLinkedList = new MIDLinkedList<TKey>();
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

		/// <summary>
		/// Gets or sets the cache entry for the given key.  During a set, if the cache entry exists, the value will be replaced.  If it does not exist, it will be added.
		/// </summary>

		public TValue this[TKey key]
		{
			get
			{
				CacheObject<TKey, TValue> cacheObj;

				try
				{
					lock (this)
					{
						cacheObj = (CacheObject<TKey, TValue>)_cacheHash[key];

						if (cacheObj != null)
						{
							_cacheLinkedList.MoveNodeToBeginning(cacheObj.LinkedListNode);

							return cacheObj.Data;
						}
						else
						{
							return default(TValue);
						}
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					Add(key, value);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the current number of cache entries that have been added.
		/// </summary>

		public int Count
		{
			get
			{
				try
				{
					lock (this)
					{
						return _cacheHash.Count;
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the number of entries that the cache allows.
		/// </summary>

		public int NumEntries
		{
			get
			{
				try
				{
					lock (this)
					{
						return _numEntries;
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the number of entries that are replaced when the cache expires entries.
		/// </summary>

		public int ExpireGroupSize
		{
			get
			{
				try
				{
					lock (this)
					{
						return _expireGroupSize;
					}
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

		/// <summary>
		/// Clears all cache entries.
		/// </summary>

		public void Clear()
		{
			try
			{
				lock (this)
				{
					_cacheHash.Clear();
					_cacheLinkedList.Clear();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if a cache entry for the given key exists.
		/// </summary>
		/// <param name="key">
		/// The key to look for.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cache entry exists.
		/// </returns>

		public bool Contains(TKey key)
		{
			try
			{
				lock (this)
				{
					return _cacheHash.ContainsKey(key);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if a cache entry for the given key exists.
		/// </summary>
		/// <param name="key">
		/// The key to look for.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cache entry exists.
		/// </returns>

		public bool ContainsKey(TKey key)
		{
			try
			{
				lock (this)
				{
					return _cacheHash.ContainsKey(key);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds a cache entry.  If the cache entry exists, the value will be replaced.  If it does not exist, it will be added.
		/// </summary>
		/// <param name="key">
		/// The key of the new cache entry.
		/// </param>
		/// <param name="value">
		/// The value of the new cache entry.
		/// </param>

		public void Add(TKey key, TValue value)
		{
			CacheObject<TKey, TValue> cacheObj;
			ArrayList oldNodes;

			try
			{
				lock (this)
				{
					if (_cacheHash.ContainsKey(key))
					{
						cacheObj = (CacheObject<TKey, TValue>)_cacheHash[key];
						cacheObj.Data = value;
						_cacheLinkedList.MoveNodeToBeginning(cacheObj.LinkedListNode);
					}
					else
					{
						if (_cacheHash.Count == _numEntries)
						{
							oldNodes = _cacheLinkedList.RemoveLastNodes(_expireGroupSize);

							foreach (MIDLinkedListNode<TKey> node in oldNodes)
							{
//								_cacheHash.Remove((int)node.Data);
								_cacheHash.Remove(node.Data);
							}
						}

						_cacheHash[key] = new CacheObject<TKey, TValue>(value, _cacheLinkedList.InsertNodeAtBeginning(new MIDLinkedListNode<TKey>(key)));
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Removes a cache entry.  
		/// </summary>
		/// <param name="key">
		/// The key of the new cache entry.
		/// </param>

		public void Remove(TKey key)
		{
			try
			{
				lock (this)
				{
					if (_cacheHash.ContainsKey(key))
					{
						_cacheHash.Remove(key);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a copy of the cache Hashtable.  
		/// </summary>
		
		public Hashtable GetHashCopy()
		{
			try
			{
				lock (this)
				{
					Hashtable ht = new Hashtable();
					foreach (KeyValuePair<TKey, CacheObject<TKey, TValue>> hashEntry in _cacheHash) 
					{
						CacheObject<TKey, TValue> cacheObj = (CacheObject<TKey, TValue>)hashEntry.Value;
						ht[hashEntry.Key] = cacheObj.Data;
					}
					return ht;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	internal class CacheObject<TKey, TValue>
	{
		private TValue _data;
		private MIDLinkedListNode<TKey> _linkedListNode;

		public CacheObject(TValue aData, MIDLinkedListNode<TKey> aLinkedListNode)
		{
			_data = aData;
			_linkedListNode = aLinkedListNode;
		}

		public TValue Data
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}

		public MIDLinkedListNode<TKey> LinkedListNode
		{
			get
			{
				return _linkedListNode;
			}
			set
			{
				_linkedListNode = value;
			}
		}
	}
}
