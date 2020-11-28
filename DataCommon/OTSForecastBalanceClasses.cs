using System;
using System.Globalization;

namespace MIDRetail.DataCommon
{
    /// <summary>
    /// Class that defines the contents of the FromLevel combo box.
    /// </summary>

    public class FromLevelCombo
    {
        //=======
        // FIELDS
        //=======

        private eFromLevelsType _fromLevelType;
        private int _fromLevelOffset;
        private int _fromLevelSequence;
        private string _fromLevelName;
        private string _displayName;

        //=============
        // CONSTRUCTORS
        //=============

        public FromLevelCombo(eFromLevelsType aFromLevelType, int aFromLevelOffset, int aFromLevelSequence, string aFromLevelName)
        {
            _fromLevelType = aFromLevelType;
            _fromLevelOffset = aFromLevelOffset;
            _fromLevelSequence = aFromLevelSequence;
            _fromLevelName = aFromLevelName;
            if (_fromLevelType == eFromLevelsType.HierarchyLevel)
            {
                _displayName = _fromLevelName;
            }
            else
            {
                _displayName = "+" + _fromLevelOffset.ToString();
            }
        }

        //===========
        // PROPERTIES
        //===========

        public eFromLevelsType FromLevelType
        {
            get
            {
                return _fromLevelType;
            }
        }

        public int FromLevelOffset
        {
            get
            {
                return _fromLevelOffset;
            }
        }

        public int FromLevelSequence
        {
            get
            {
                return _fromLevelSequence;
            }
        }

        public string FromLevelName
        {
            get
            {
                return _fromLevelName;
            }
        }

        //========
        // METHODS
        //========

        override public string ToString()
        {
            return _displayName;
        }

        override public bool Equals(object obj)
        {
            if ((obj.GetType().Name != "DBNull") && ((FromLevelCombo)obj).FromLevelType == _fromLevelType)
            {
                if (((FromLevelCombo)obj).FromLevelType == eFromLevelsType.LevelOffset)
                {
                    if (((FromLevelCombo)obj).FromLevelOffset == _fromLevelOffset)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (((FromLevelCombo)obj).FromLevelType == eFromLevelsType.HierarchyLevel)
                {
                    if (((FromLevelCombo)obj).FromLevelSequence == _fromLevelSequence)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        override public int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


    /// <summary>
    /// Class that defines the contents of the ToLevel combo box.
    /// </summary>

    public class ToLevelCombo
    {
        //=======
        // FIELDS
        //=======

        private eToLevelsType _toLevelType;
        private int _toLevelOffset;
        private int _toLevelSequence;
        private string _toLevelName;
        private string _displayName;

        //=============
        // CONSTRUCTORS
        //=============

        public ToLevelCombo(eToLevelsType aToLevelType, int aToLevelOffset, int aToLevelSequence, string aToLevelName)
        {
            _toLevelType = aToLevelType;
            _toLevelOffset = aToLevelOffset;
            _toLevelSequence = aToLevelSequence;
            _toLevelName = aToLevelName;
            if (_toLevelType == eToLevelsType.HierarchyLevel)
            {
                _displayName = _toLevelName;
            }
            else
            {
                _displayName = "+" + _toLevelOffset.ToString();
            }
        }

        //===========
        // PROPERTIES
        //===========

        public eToLevelsType ToLevelType
        {
            get
            {
                return _toLevelType;
            }
        }

        public int ToLevelOffset
        {
            get
            {
                return _toLevelOffset;
            }
        }

        public int ToLevelSequence
        {
            get
            {
                return _toLevelSequence;
            }
        }

        public string ToLevelName
        {
            get
            {
                return _toLevelName;
            }
        }

        //========
        // METHODS
        //========

        override public string ToString()
        {
            return _displayName;
        }

        override public bool Equals(object obj)
        {
            if ((obj.GetType().Name != "DBNull") && ((ToLevelCombo)obj).ToLevelType == _toLevelType)
            {
                if (((ToLevelCombo)obj).ToLevelType == eToLevelsType.LevelOffset)
                {
                    if (((ToLevelCombo)obj).ToLevelOffset == _toLevelOffset)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (((ToLevelCombo)obj).ToLevelType == eToLevelsType.HierarchyLevel)
                {
                    if (((ToLevelCombo)obj).ToLevelSequence == _toLevelSequence)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        override public int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


	/// <summary>
	/// Class that defines the contents of the HighLevel combo box.
	/// </summary>

	public class HighLevelCombo
	{
		//=======
		// FIELDS
		//=======

		private eHighLevelsType _highLevelType;
		private int _highLevelOffset;
		private int _highLevelSequence;
		private string _highLevelName;
		private string _displayName;

		//=============
		// CONSTRUCTORS
		//=============

        public HighLevelCombo(eHighLevelsType aHighLevelType, int aHighLevelOffset, int aHighLevelSequence, string aHighLevelName)
		{
            _highLevelType = aHighLevelType;
            _highLevelOffset = aHighLevelOffset;
            _highLevelSequence = aHighLevelSequence;
            _highLevelName = aHighLevelName;
            if (_highLevelType == eHighLevelsType.HierarchyLevel)
			{
				_displayName = _highLevelName;
			}
			else
			{
				_displayName = "+" + _highLevelOffset.ToString();
			}
		}

		//===========
		// PROPERTIES
		//===========

        public eHighLevelsType HighLevelType
		{
			get
			{
				return _highLevelType;
			}
		}

        public int HighLevelOffset
		{
			get
			{
				return _highLevelOffset;
			}
		}

        public int HighLevelSequence
		{
			get
			{
				return _highLevelSequence;
			}
		}

        public string HighLevelName
		{
			get
			{
				return _highLevelName;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _displayName;
		}

		override public bool Equals(object obj)
		{
            if ((obj.GetType().Name != "DBNull") && ((HighLevelCombo)obj).HighLevelType == _highLevelType)
			{
                if (((HighLevelCombo)obj).HighLevelType == eHighLevelsType.LevelOffset)
				{
                    if (((HighLevelCombo)obj).HighLevelOffset == _highLevelOffset)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
                else if (((HighLevelCombo)obj).HighLevelType == eHighLevelsType.HierarchyLevel)
				{
                    if (((HighLevelCombo)obj).HighLevelSequence == _highLevelSequence)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		override public int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
    [Serializable()]
    public class LowLevelCombo
	{
		//=======
		// FIELDS
		//=======

		private eLowLevelsType _lowLevelType;
		private int _lowLevelOffset;
		private int _lowLevelSequence;
		private string _lowLevelName;
		private string _displayName;

		//=============
		// CONSTRUCTORS
		//=============

		public LowLevelCombo(eLowLevelsType aLowLevelType, int aLowLevelOffset, int aLowLevelSequence, string aLowLevelName)
		{
			_lowLevelType = aLowLevelType;
			_lowLevelOffset = aLowLevelOffset;
			_lowLevelSequence = aLowLevelSequence;
			_lowLevelName = aLowLevelName;
			if (_lowLevelType == eLowLevelsType.HierarchyLevel)
			{
				_displayName = _lowLevelName;
			}
			else
			{
				_displayName = "+" + _lowLevelOffset.ToString();
			}
		}

		//===========
		// PROPERTIES
		//===========

		public eLowLevelsType LowLevelType
		{
			get
			{
				return _lowLevelType;
			}
		}

		public int LowLevelOffset
		{
			get
			{
				return _lowLevelOffset;
			}
		}

		public int LowLevelSequence
		{
			get
			{
				return _lowLevelSequence;
			}
		}

		public string LowLevelName
		{
			get
			{
				return _lowLevelName;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _displayName;
		}

		override public bool Equals(object obj)
		{
            if ((obj.GetType().Name != "DBNull") && ((LowLevelCombo)obj).LowLevelType == _lowLevelType)
			{
				if (((LowLevelCombo)obj).LowLevelType == eLowLevelsType.LevelOffset)
				{
					if (((LowLevelCombo)obj).LowLevelOffset == _lowLevelOffset)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else if (((LowLevelCombo)obj).LowLevelType == eLowLevelsType.HierarchyLevel)
				{
					if (((LowLevelCombo)obj).LowLevelSequence == _lowLevelSequence)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		override public int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	/// <summary>
	/// Class that defines the contents of the iterations combo box.
	/// </summary>

	public class IterationsCombo
	{
		//=======
		// FIELDS
		//=======

		private eIterationType	_iterationType;
		private int				_iterationCount;
		private string			_displayName;

		//=============
		// CONSTRUCTORS
		//=============

		public IterationsCombo(eIterationType aIterationType, int aIterationCount)
		{
			_iterationType = aIterationType;
			_iterationCount = aIterationCount;
			if (_iterationType == eIterationType.UseBase)
			{
				_displayName = "Use default";
			}
			else
			{
				_displayName = _iterationCount.ToString();
			}
		}

		//===========
		// PROPERTIES
		//===========

		public eIterationType IterationType
		{
			get
			{
				return _iterationType;
			}
		}

		public int IterationCount
		{
			get
			{
				return _iterationCount;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _displayName;
		}

		override public bool Equals(object obj)
		{
			if (((IterationsCombo)obj).IterationType == _iterationType &&
				((IterationsCombo)obj).IterationCount == _iterationCount)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		override public int GetHashCode()
		{
			return base.GetHashCode();
		}

	}


	/// <summary>
	/// Class that defines the contents of the variable combo box.
	/// </summary>

	public class VariableCombo
	{
		//=======
		// FIELDS
		//=======

		private int				_variableNumber;
		private string			_variableName;
		private string			_displayName;

		//=============
		// CONSTRUCTORS
		//=============

		public VariableCombo(int aVariableNumber, string aVariableName)
		{
			_variableNumber = aVariableNumber;
			_variableName = aVariableName;
			_displayName = aVariableName;
		}

		//===========
		// PROPERTIES
		//===========

		public int VariableNumber
		{
			get
			{
				return _variableNumber;
			}
		}

		public string VariableName
		{
			get
			{
				return _variableName;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _displayName;
		}

		override public bool Equals(object obj)
		{
			if (((VariableCombo)obj).VariableNumber == _variableNumber)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		override public int GetHashCode()
		{
			return _variableNumber;
		}
	}

	/// <summary>
	/// Class that defines the contents of the computation mode combo box.
	/// </summary>

	public class ComputationModeCombo
	{
		//=======
		// FIELDS
		//=======

		private string			_computationModeName;
		private string			_displayName;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationModeCombo(string aComputationModeName)
		{
			_computationModeName = aComputationModeName;
			_displayName = aComputationModeName;
		}

		//===========
		// PROPERTIES
		//===========

		public string ComputationModeName
		{
			get
			{
				return _computationModeName;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _displayName;
		}

		override public bool Equals(object obj)
		{
            // Begin Issue # 5595 kjohnson
            if (obj.GetType() == typeof(ComputationModeCombo))
            {
                if (((ComputationModeCombo)obj).ComputationModeName == _computationModeName)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // End Issue # 5595
            else 
            {
                return false;
            }
		}

		override public int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	/// <summary>
	/// Class that defines the contents of the using combo box.
	/// </summary>

	public class UsingCombo
	{
		//=======
		// FIELDS
		//=======

		private ePlanBasisType	_planBasisType;
		private int				_basisIndex;
		private string			_displayName;

		//=============
		// CONSTRUCTORS
		//=============

		public UsingCombo(ePlanBasisType aPlanBasisType, int aBasisIndex)
		{
			_planBasisType = aPlanBasisType;
			_basisIndex = aBasisIndex;
			if (aPlanBasisType == ePlanBasisType.Plan)
			{
				_displayName = "Plan";
			}
			else
			{
				int basisID = aBasisIndex + 1;
				_displayName = "Basis " + basisID.ToString(CultureInfo.CurrentCulture);
			}
		}

		//===========
		// PROPERTIES
		//===========

		public ePlanBasisType PlanBasisType
		{
			get
			{
				return _planBasisType;
			}
		}

		public int BasisIndex
		{
			get
			{
				return _basisIndex;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _displayName;
		}

		override public bool Equals(object obj)
		{
			if (((UsingCombo)obj).PlanBasisType == _planBasisType &&
				((UsingCombo)obj).BasisIndex == _basisIndex)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		override public int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	/// <summary>
	/// Class that defines the contents of the variable combo box.
	/// </summary>

	public class VariableCheckedListBox
	{
		//=======
		// FIELDS
		//=======

		private int				_variableNumber;
		private string			_variableName;
		private string			_displayName;

		//=============
		// CONSTRUCTORS
		//=============

		public VariableCheckedListBox(int aVariableNumber, string aVariableName)
		{
			_variableNumber = aVariableNumber;
			_variableName = aVariableName;
			_displayName = aVariableName;
		}

		//===========
		// PROPERTIES
		//===========

		public int VariableNumber
		{
			get
			{
				return _variableNumber;
			}
		}

		public string VariableName
		{
			get
			{
				return _variableName;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _displayName;
		}

		override public bool Equals(object obj)
		{
			if (((VariableCombo)obj).VariableNumber == _variableNumber)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		override public int GetHashCode()
		{
			return _variableNumber;
		}
	}
}
