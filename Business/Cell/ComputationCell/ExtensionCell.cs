using System;

namespace MIDRetail.Business
{
	/// <summary>
	/// ExtensionCell is a class that defines the structure of a extension cell which contains information used by the cube undo logic and computations.
	/// </summary>
	/// <remarks>
	/// A Extension Cell inherits from the Cell class and contains an additional field to store a hashtable of previous values and computation information.
	/// </remarks>
	
	public class ExtensionCell : Cell
	{
		//=======
		// FIELDS
		//=======

		private double _preInitValue;
		private double _postInitValue;
		private ComputationInfo _compInfo;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of ExtensionCell.
		/// </summary>

		public ExtensionCell()
		{
			_preInitValue = double.NaN;
			_postInitValue = double.NaN;
			_compInfo = null;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets a boolean indicating if the ComputationInfo object has been created.
		/// </summary>

		public bool isCompInfoAllocated
		{
			get
			{
				return (_compInfo != null);
			}
		}

		/// <summary>
		/// Gets or sets the pre-init Cell value.
		/// </summary>

		public double PreInitValue
		{
			get
			{
				return _preInitValue;
			}
			set
			{
				_preInitValue = value;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the pre-init Cell value has been set.
		/// </summary>

		public bool isPreInitValueSet
		{
			get
			{
				return (!double.IsNaN(_preInitValue));
			}
		}

		/// <summary>
		/// Gets or sets the post-init Cell value.
		/// </summary>

		public double PostInitValue
		{
			get
			{
				return _postInitValue;
			}
			set
			{
				_postInitValue = value;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the post-init Cell value has been set.
		/// </summary>

		public bool isPostInitValueSet
		{
			get
			{
				return (!double.IsNaN(_postInitValue));
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Clone method to satisfy the abstract requirement from the Cell class.  Does a shallow copy.
		/// </summary>
		/// <returns>
		/// The cloned object.
		/// </returns>

		override public Cell Clone()
		{
			ExtensionCell extCell;

			try
			{
				extCell = new ExtensionCell();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			
			return extCell;
		}

		/// <summary>
		/// Copy method to satisfy the abstract requirement from the Cell class.  Does a deep copy.
		/// </summary>
		/// <returns>
		/// The copied object.
		/// </returns>

		override public Cell Copy()
		{
			ExtensionCell extCell;

			try
			{
				extCell = new ExtensionCell();
				extCell.CopyFrom(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}

			return extCell;
		}

		/// <summary>
		/// CopyFrom method to satisfy the abstract requirement from the Cell class.  Does a deep copy.
		/// </summary>
		/// <param name="aCell">
		/// The cell to copy.
		/// </param>

		override public void CopyFrom(Cell aCell)
		{
			try
			{
				_preInitValue = ((ExtensionCell)aCell)._preInitValue;
				_postInitValue = ((ExtensionCell)aCell)._postInitValue;
				_compInfo = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Defines the method to clear the contents of the ExtensionCell.
		/// </summary>

		override public void Clear()
		{
			_preInitValue = double.NaN;
			_postInitValue = double.NaN;
			_compInfo = null;
		}

		/// <summary>
		/// Gets the ComputationInfo object.
		/// </summary>

		public ComputationInfo GetCompInfo(ComputationCellReference aCompCellRef)
		{
			try
			{
				if (_compInfo == null)
				{
					_compInfo = new ComputationInfo();
					((ComputationCubeGroup)aCompCellRef.Cube.CubeGroup).CompInfoList.Enqueue(this);
				}

				return _compInfo;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#1954-MD - JSmith - Assortment Performance
        public ComputationInfo GetCompInfo()
        {
            try
            {
                return _compInfo;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#1954-MD - JSmith - Assortment Performance

		/// <summary>
		/// Sets the ComputationInfo object.
		/// </summary>

		public void SetCompInfo(ComputationInfo aCompInfo)
		{
			_compInfo = aCompInfo;
		}
	}
}