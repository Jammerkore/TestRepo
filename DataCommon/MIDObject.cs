using System;

namespace MIDRetail.DataCommon
{
	abstract public class MIDObject :  IDisposable
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of MIDObject.
		/// </summary>

		public MIDObject()
		{

		}

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			System.GC.SuppressFinalize(this);
		}

		virtual protected void Dispose(bool disposing)
		{
		}

		~MIDObject()
		{
			Dispose(false);
		}

		#endregion

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}
}
