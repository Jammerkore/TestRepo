using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.ForecastComputations
{
	/// <summary>
	/// The ComputationsCollection class creates a collection for Computations objects.
	/// </summary>

	abstract public class BasePlanComputationsCollection : IPlanComputationsCollection
	{
		//=======
		// FIELDS
		//=======

		private Hashtable _compHash;
		private BasePlanComputations _defaultComps;

		//=============
		// CONSTRUCTORS
		//=============

		public BasePlanComputationsCollection()
		{
			try
			{
				_compHash = new Hashtable();
			}
			catch (Exception)
			{
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		public IPlanComputations GetComputations(string aName)
		{
			return (IPlanComputations)_compHash[aName];
		}

		public IPlanComputations GetDefaultComputations()
		{
			return (IPlanComputations)_defaultComps;
		}

		public IPlanComputations[] GetComputationList()
		{
			IPlanComputations[] compArray;

			try
			{
				compArray = new IPlanComputations[_compHash.Count];
				int i = 0;

				foreach (object obj in _compHash.Values)
				{
					compArray[i] = (IPlanComputations)obj;
					++i;
				}

				return compArray;
			}
			catch (Exception)
			{
				throw;
			}
		}

		protected void AddComputation(BasePlanComputations aComps)
		{
			try
			{
				AddComputation(aComps, false);
			}
			catch (Exception)
			{
				throw;
			}
		}

		protected void AddComputation(BasePlanComputations aComps, bool aDefault)
		{
			try
			{
				_compHash.Add(aComps.Name, aComps);

				if (aDefault)
				{
					_defaultComps = aComps;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	/// <summary>
	/// Abstract.  The BasePlanComputations class is a base class for Computations classes.
	/// </summary>

	abstract public class BasePlanComputations : BaseComputations, IPlanComputations
	{
		//=======
		// FIELDS
		//=======

		protected BasePlanVariables _basePlanVariables;
		protected BasePlanTimeTotalVariables _basePlanTimeTotalVariables;
		protected BasePlanQuantityVariables _basePlanQuantityVariables;
		protected BasePlanFormulasAndSpreads _basePlanFormulasAndSpreads;
		protected BasePlanChangeMethods _basePlanChangeMethods;
		protected BasePlanVariableInitialization _basePlanVariableInitialization;
		protected BasePlanCubeInitialization _basePlanCubeInitialization;
		protected BasePlanToolBox _basePlanToolBox;

		//=============
		// CONSTRUCTORS
		//=============

		public BasePlanComputations()
		{
			try
			{
			}
			catch (Exception)
			{
				throw;
			}
		}

		//=======================
		// INTERFACE REQUIREMENTS
		//=======================

		/// <summary>
		/// Gets the Variables object.
		/// </summary>

		public IPlanComputationVariables PlanVariables
		{
			get
			{
				return BasePlanVariables;
			}
		}

		/// <summary>
		/// Gets the TimeTotalVariables object.
		/// </summary>

		public IPlanComputationTimeTotalVariables PlanTimeTotalVariables
		{
			get
			{
				return BasePlanTimeTotalVariables;
			}
		}

		/// <summary>
		/// Gets the QuantityVariables object.
		/// </summary>

		public IPlanComputationQuantityVariables PlanQuantityVariables
		{
			get
			{
				return BasePlanQuantityVariables;
			}
		}

		/// <summary>
		/// Gets the CubeInitialization object.
		/// </summary>

		public IPlanComputationCubeInitialization PlanCubeInitialization
		{
			get
			{
				return BasePlanCubeInitialization;
			}
		}

		/// <summary>
		/// Creates the PlanComputationWorkArea
		/// </summary>

		public object CreatePlanComputationWorkArea()
		{
			return new PlanComputationWorkArea();
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the CubeInitialization object.
		/// </summary>

		abstract public string Name { get; }

		/// <summary>
		/// Gets the ToolBox object.
		/// </summary>

		override public BaseToolBox ToolBox
		{
			get
			{
				return _basePlanToolBox;
			}
		}

		/// <summary>
		/// Gets the FormulasAndSpreads object.
		/// </summary>

		public BasePlanFormulasAndSpreads BasePlanFormulasAndSpreads
		{
			get
			{
				return _basePlanFormulasAndSpreads;
			}
		}

		/// <summary>
		/// Gets the ChangeMethods object.
		/// </summary>

		public BasePlanChangeMethods BasePlanChangeMethods
		{
			get
			{
				return _basePlanChangeMethods;
			}
		}

		/// <summary>
		/// Gets the VariableInitialization object.
		/// </summary>

		public BasePlanVariableInitialization BasePlanVariableInitialization
		{
			get
			{
				return _basePlanVariableInitialization;
			}
		}

		/// <summary>
		/// Gets the CubeInitialization object.
		/// </summary>

		public BasePlanCubeInitialization BasePlanCubeInitialization
		{
			get
			{
				return _basePlanCubeInitialization;
			}
		}

		/// <summary>
		/// Gets the ToolBox object.
		/// </summary>

		public BasePlanToolBox BasePlanToolBox
		{
			get
			{
				return _basePlanToolBox;
			}
		}

		/// <summary>
		/// Gets the Variables object.
		/// </summary>

		public BasePlanVariables BasePlanVariables
		{
			get
			{
				return _basePlanVariables;
			}
		}

		/// <summary>
		/// Gets the TimeTotalVariables object.
		/// </summary>

		public BasePlanTimeTotalVariables BasePlanTimeTotalVariables
		{
			get
			{
				return _basePlanTimeTotalVariables;
			}
		}

		/// <summary>
		/// Gets the QuantityVariables object.
		/// </summary>

		public BasePlanQuantityVariables BasePlanQuantityVariables
		{
			get
			{
				return _basePlanQuantityVariables;
			}
		}

		//========
		// METHODS
		//========
	}
}