using System;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	/// <summary>
	/// The Filter class defines a filter that can be applied to a ProfileList.
	/// </summary>
	/// <remarks>
	/// A filter can be used to reduce the number of visible profiles.
	/// </remarks>

	[Serializable]
	abstract public class Filter : MIDObject
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of Filter.
		/// </summary>

		protected Filter()
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Abstract.  This method returns the eProfileType that identifies what ProfileList this Filter can be applied to.
		/// </summary>

		abstract public eProfileType ProfileType
		{
			get;
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Applies the given Filter to a given ProfileList.
		/// </summary>
		/// <remarks>
		/// Applying a Filter to a ProfileList adds Profiles that have passed the Filter to a new ProfileList.
		/// </remarks>
		/// <param name="aProfileList">
		/// The ArrayList to apply the Filter to.
		/// </param>
		/// <returns>
		/// An ArrayList containing the selected Profiles.
		/// </returns>

		virtual public ProfileList ApplyFilter(ProfileList aProfileList)
		{
			return aProfileList;
		}
	}
}
