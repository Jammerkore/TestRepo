using System;
using MIDRetail.Common;

namespace MIDRetail.Business
{

	/// <summary>
	/// Base class for all algorithms
	/// </summary>
	public abstract class MIDAlgorithm
	{

		protected static Audit _warnings;

        // Begin TT#1243 - JSmith - Audit Performance
        //public MIDAlgorithm()
        //{
        //    _warnings = new Audit();
        //}
        public MIDAlgorithm(Audit aAudit)
        {
            _warnings = aAudit;
        }
        // End TT#1243


		/// <summary>
		/// Used to accumulate any warnings that occured during processing
		/// </summary>
		public Audit Warnings
		{
			get { return _warnings;	}
		}

		/// <summary>
		/// Each algorithm will have a Calculate method
		/// </summary>
		public abstract void Calculate();
	}
}
