using System;
using System.Collections;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// MIDHashtable is a Hashtable with the Add() method
	/// overridden so if you add something that is already on 
	/// the Hashtable, it modifies it.
	/// </summary>
	public class MIDHashtable : Hashtable
	{
		public MIDHashtable() : base()
		{
		}

		override public void Add(object aKey, object aValue)
		{
			try
			{
				if (!this.ContainsKey(aKey))
				{
					base.Add(aKey, aValue);
				}
				else
				{
					base.Remove(aKey);
					base.Add(aKey, aValue);
				}
			}
			catch
			{
				throw;
			}
		}
	}
}
