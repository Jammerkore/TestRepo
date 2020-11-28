using System;
using System.Collections;
using System.Text;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Class that defines the Variables for Assortment.
	/// </summary>

	[Serializable]
	abstract public class AssortmentVariables
	{
		//=======
		// FIELDS
		//=======

		protected ProfileList _profileList;
		protected Hashtable _nameHash;

		//=============
		// CONSTRUCTORS
		//=============
		
		/// <summary>
		/// Creates a new instance of AssortmentVariables.
		/// </summary>

		public AssortmentVariables(eProfileType aProfType) 
		{
			try
			{
				_profileList = new ProfileList(aProfType);
				_nameHash = new Hashtable();
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
		/// Gets the number of Variables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public int NumVariables
		{
			get
			{
				try
				{
					return _profileList.Count;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the ProfileList of Variables.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>

		public ProfileList VariableProfileList
		{
			get
			{
				return _profileList;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes this instance of AssortmentVariables.
		/// </summary>

		abstract public void Initialize();

		/// <summary>
		/// This method returns VariableProfile for a given variable name.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
		/// <param name="aName">
		/// The name of the variable to lookup.
		/// </param>
		/// <returns>
		/// The Variable profile with the given name.
		/// </returns>

		public VariableProfile GetVariableProfileByName(string aName)
		{
			try
			{
				return (VariableProfile)_nameHash[aName];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that copies this object to another AssortmentVariables object.
		/// </summary>
		/// <param name="aBaseObj">
		/// The object to copy to.
		/// </param>
		/// <returns>
		/// The object copied to.
		/// </returns>

		virtual public AssortmentVariables CopyTo(AssortmentVariables aBaseObj)
		{
			try
			{
				aBaseObj._profileList = (ProfileList)_profileList.Clone();
				aBaseObj._nameHash = (Hashtable)_nameHash.Clone();

				return aBaseObj;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the Variables for Assortment.
	/// </summary>

	[Serializable]
	abstract public class AssortmentQuantityVariables : AssortmentVariables
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentVariables.
		/// </summary>

		public AssortmentQuantityVariables(eProfileType aProfType)
			: base(aProfType)
		{
			try
			{
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

		abstract public QuantityVariableProfile ValueVariableProfile { get; }
		abstract public QuantityVariableProfile DifferenceVariableProfile { get; }
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		abstract public QuantityVariableProfile TotalVariableProfile { get; }
		// End TT#2148 - stodd - Assortment totals do not include header values


		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines the Variables for Assortment.
	/// </summary>

	[Serializable]
	abstract public class AssortmentComponentVariables : AssortmentVariables
	{
		//=======
		// FIELDS
		//=======

		protected Hashtable _profTypeHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentVariables.
		/// </summary>

		public AssortmentComponentVariables(eProfileType aProfType)
			: base(aProfType)
		{
			try
			{
				_profTypeHash = new Hashtable();
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

		//========
		// METHODS
		//========

		/// <summary>
		/// Method that adds a AssortmentComponentVariableProfile to the internal varible ProfileList.
		/// </summary>
		/// <param name="aVarProf">
		/// The AssortmentComponentVariableProfile to add.
		/// </param>

		public void AddVariableProfile(AssortmentComponentVariableProfile aVarProf)
		{
			try
			{
				_profileList.Add(aVarProf);
				_nameHash.Add(aVarProf.VariableName, aVarProf);
				_profTypeHash.Add(aVarProf.ProfileListType, aVarProf);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin TT#1939 - DOConnell - Add colors to the 1st Placeholder no issue. (Added Blank row and typed in the color code) Go to the Next header and try to add a color and receive a Null Reference Exception.
        /// <summary>
        /// Method that adds a AssortmentComponentVariableProfile to the internal varible ProfileList.
        /// </summary>
        /// <param name="aVarProf">
        /// The AssortmentComponentVariableProfile to add.
        /// </param>

        public void RemoveVariableProfile(AssortmentComponentVariableProfile aVarProf)
        {
            try
            {
                _profileList.Remove(aVarProf);
                _nameHash.Remove(aVarProf.VariableName);
                _profTypeHash.Remove(aVarProf.ProfileListType);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		//End TT#1939 - DOConnell - Add colors to the 1st Placeholder no issue. (Added Blank row and typed in the color code) Go to the Next header and try to add a color and receive a Null Reference Exception.

		/// <summary>
		/// This method returns VariableProfile for a given variable name.  IComputations requirement.  DO NOT REMOVE.
		/// </summary>
        /// <param name="aProfType">
		/// The type of the profile.
		/// </param>
		/// <returns>
		/// The Variable profile with the given name.
		/// </returns>

		public AssortmentComponentVariableProfile GetVariableProfileByProfileType(eProfileType aProfType)
		{
			try
			{
				return (AssortmentComponentVariableProfile)_profTypeHash[aProfType];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that copies this object to another AssortmentVariables object.
		/// </summary>
		/// <param name="aBaseObj">
		/// The object to copy to.
		/// </param>
		/// <returns>
		/// The object copied to.
		/// </returns>

		override public AssortmentVariables CopyTo(AssortmentVariables aBaseObj)
		{
			try
			{
				base.CopyTo(aBaseObj);

				((AssortmentComponentVariables)aBaseObj)._profTypeHash = (Hashtable)_profTypeHash.Clone();

				return aBaseObj;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that creates a copy of this object.
		/// </summary>
		/// <returns>
		/// The copy of this object.
		/// </returns>
		
		abstract public AssortmentComponentVariables Copy();
	}
}