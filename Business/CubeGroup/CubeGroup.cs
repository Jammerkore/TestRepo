using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The CubeGroup class contains a collection of Cubes.
	/// </summary>
	/// <remarks>
	/// Groups of Cubes can be related through the used of a CubeGroup class.  Group level functions can be added to inherited classes from this abstract
	/// base class.
	/// </remarks>

	abstract public class CubeGroup : WorkArea
	{
		//=======
		// FIELDS
		//=======

		protected System.Collections.Hashtable _cubeTable;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of CubeGroup using the given SessionAddressBlock and Transaction.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to the SessionAddressBlock for this user session.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to the Transaction for this functional transaction.
		/// </param>

		public CubeGroup(SessionAddressBlock aSAB, Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
			try
			{
				_cubeTable = new System.Collections.Hashtable();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override protected void Dispose(bool disposing)
		{
			try
			{
				base.Dispose(disposing);
			}
			catch (Exception)
			{
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the HashTable that contains the collection of Cubes.
		/// </summary>

		public System.Collections.Hashtable CubeTable
		{
			get
			{
				return _cubeTable;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Method that returns the PlanCube reference to the cube specified by the eCubeType parameter.
		/// </summary>
		/// <param name="aCubeType">
		/// The eCubeType parameter of the Cube reference to be retrieved.
		/// </param>
		/// <returns>
		/// The reference to the PlanCube for the given eCubeType.
		/// </returns>

		virtual public Cube GetCube(eCubeType aCubeType)
		{
			try
			{
				return (Cube)_cubeTable[aCubeType];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that adds a PlanCube reference identified by the eCubeType parameter to the list.
		/// </summary>
		/// <param name="aCubeType">
		/// The eCubeType parameter of the Cube reference to be added.
		/// </param>
		/// <param name="aCube">
		/// The reference to the PlanCube for the given eCubeType.
		/// </param>

		virtual public void SetCube(eCubeType aCubeType, Cube aCube)
		{
			try
			{
				_cubeTable[aCubeType] = aCube;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//Begin Enhancement - JScott - Add Balance Low Levels functionality

		/// <summary>
		/// Closes this PlanCubeGroup.
		/// </summary>

		virtual public void CloseCubeGroup()
		{
			try
			{
				_cubeTable.Clear();
				_cubeTable = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Enhancement - JScott - Add Balance Low Levels functionality
	}
}
