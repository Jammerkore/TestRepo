using System;
using System.Data;
using System.Collections;
using System.Globalization;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Summary description for ComputationModel.
	/// </summary>
	public class ComputationModel
	{
		// Fields
		private bool _computationModelFound = false;
		private int _computationModelRID = Include.NoRID;
		private string _computationModelID;
		private string _calcMode;
		private ArrayList _modelEntries;
		private Hashtable _computationVersionsHash;
		private Hashtable _changeVariableHash;
		private Hashtable _computationTypeHash;
		private int _priorVersionRID = Include.NoRID;
		private int _priorChangeVariable = Include.Undefined;
        //private eComputationType _priorComputationType = eComputationType.None;

		// Constructors

		public ComputationModel()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public ComputationModel(string aComputationModelID)
		{
			LoadModel(aComputationModelID);
		}

		public ComputationModel(int aComputationModelRID)
		{
			LoadModel(aComputationModelRID);
		}

		// Properties
		/// <summary>
		/// Gets a flag identifying if the computation mode was found
		/// </summary>
		public bool ComputationModelFound 
		{
			get { return _computationModelFound ; }
		}

		/// <summary>
		/// Gets or sets the record ID of the computation mode
		/// </summary>
		public int ComputationModelRID 
		{
			get { return _computationModelRID ; }
			set { _computationModelRID = value; }
		}

		/// <summary>
		/// Gets or sets the ID of the computation mode
		/// </summary>
		public string ComputationModelID 
		{
			get { return _computationModelID ; }
			set { _computationModelID = value; }
		}

		/// <summary>
		/// Gets or sets the calc mode of the computation mode
		/// </summary>
		public string CalcMode 
		{
			get { return _calcMode ; }
			set { _calcMode = value; }
		}

		/// <summary>
		/// Gets or sets the array of computation model entries
		/// </summary>
		public ArrayList ModelEntries 
		{
			get { return _modelEntries ; }
			set { _modelEntries = value; }
		}


		// Methods
		/// <summary>
		/// Loads computation model and all entries.
		/// </summary>
		private void LoadModel(string aComputationModelID)
		{
			ComputationData cd = new ComputationData();
			DataTable dt = cd.ComputationModel_Read(aComputationModelID);
			if (dt.Rows.Count == 0)
			{
				_computationModelFound = false;
			}
			else
			{
				LoadModelValues(dt);
			}
		}

		private void LoadModel(int aComputationModelRID)
		{
			ComputationData cd = new ComputationData();
			DataTable dt = cd.ComputationModel_Read(aComputationModelRID);
			if (dt.Rows.Count == 0)
			{
				_computationModelFound = false;
			}
			else
			{
				LoadModelValues(dt);
			}
		}

		/// <summary>
		/// Loads computation model and all entries.
		/// </summary>
		private void LoadModelValues(DataTable aDataTable)
		{
			ComputationData cd = new ComputationData();
			_computationModelFound = true;
			_computationVersionsHash = new Hashtable();
			Hashtable changeVariableHash;
			Hashtable computationTypeHash;

			DataRow modelRow = aDataTable.Rows[0];
			_computationModelRID = Convert.ToInt32(modelRow["COMP_MODEL_RID"], CultureInfo.CurrentUICulture);
			_computationModelID = Convert.ToString(modelRow["COMP_MODEL_ID"], CultureInfo.CurrentUICulture);
			_calcMode = Convert.ToString(modelRow["CALC_MODE"], CultureInfo.CurrentUICulture);
			ModelEntries = new ArrayList();

			DataTable dt = cd.ComputationModelEntry_Read(_computationModelRID);
			foreach(DataRow dr in dt.Rows)
			{
			
				ComputationModelEntry cme = new ComputationModelEntry();

				cme.EntrySequence = Convert.ToInt32(dr["COMP_MODEL_SEQUENCE"], CultureInfo.CurrentUICulture);
				cme.ComputationType = (eComputationType)Convert.ToInt32(dr["COMP_TYPE"], CultureInfo.CurrentUICulture);
				cme.VersionRID = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
				cme.ChangeVariable = Convert.ToInt32(dr["CHANGE_VARIABLE"], CultureInfo.CurrentUICulture);
				if (dr["PRODUCT_LEVEL"] == DBNull.Value)
				{
					cme.ProductLevel = Include.Undefined;
				}
				else
				{
					cme.ProductLevel = Convert.ToInt32(dr["PRODUCT_LEVEL"], CultureInfo.CurrentUICulture);
				}
				
				ModelEntries.Add( cme );	// add model entry to model info


				changeVariableHash = (Hashtable)_computationVersionsHash[cme.VersionRID];
				if (changeVariableHash == null)
				{
					changeVariableHash = new Hashtable();
					_computationVersionsHash.Add(cme.VersionRID, changeVariableHash);
				}

				computationTypeHash = (Hashtable)changeVariableHash[cme.ChangeVariable];
				if (computationTypeHash == null)
				{
					computationTypeHash = new Hashtable();
					changeVariableHash.Add(cme.ChangeVariable, computationTypeHash);
				}

				if (!computationTypeHash.Contains(cme.ComputationType))
				{
					computationTypeHash.Add(cme.ComputationType, null);
				}
			}
		}

		/// <summary>
		/// Loads computation model and all entries.
		/// </summary>
		public bool NeedsComputationItem(int aVersionRID, int aChangeVariable, eComputationType aComputationType)
		{
			if (aVersionRID != _priorVersionRID)
			{
				_changeVariableHash = (Hashtable)_computationVersionsHash[aVersionRID];
				_priorVersionRID = aVersionRID;
				_priorChangeVariable = Include.Undefined;
			}
			if (_changeVariableHash == null)
			{
				return false;
			}

			if (aChangeVariable != _priorChangeVariable)
			{
				_computationTypeHash = (Hashtable)_changeVariableHash[aChangeVariable];
			}
			if (_computationTypeHash == null)
			{
				return false;
			}

			if (_computationTypeHash.ContainsKey(aComputationType) ||
				_computationTypeHash.ContainsKey(eComputationType.All))
			{
				return true;
			}

			return false;
		}
	}

	/// <summary>
	/// Summary description for ComputationModelEntry.
	/// </summary>
	public class ComputationModelEntry
	{
		// Fields
		private int _entrySequence;
		private eComputationType _computationType;
		private int _versionRID;
		private int _changeVariable;
		private int _productLevel;

		// Constructors

		public ComputationModelEntry()
		{
			
		}

		public ComputationModelEntry(int aEntrySequence, eComputationType aComputationType, int aVersionRID,
			int aChangeVariable, int aProductLevel)
		{
			_entrySequence = aEntrySequence;
			_computationType = aComputationType;
			_versionRID = aVersionRID;
			_changeVariable = aChangeVariable;
			_productLevel = aProductLevel;
		}

		// Properties

		/// <summary>
		/// Gets or sets the sequence of the entry
		/// </summary>
		public int EntrySequence 
		{
			get { return _entrySequence ; }
			set { _entrySequence = value; }
		}

		/// <summary>
		/// Gets or sets the type of the entry
		/// </summary>
		public eComputationType ComputationType 
		{
			get { return _computationType ; }
			set { _computationType = value; }
		}

		/// <summary>
		/// Gets or sets the version of the entry
		/// </summary>
		public int VersionRID 
		{
			get { return _versionRID ; }
			set { _versionRID = value; }
		}

		/// <summary>
		/// Gets or sets the variable that is changed in the entry
		/// </summary>
		public int ChangeVariable 
		{
			get { return _changeVariable ; }
			set { _changeVariable = value; }
		}

		/// <summary>
		/// Gets or sets the level of a product that must be change for the entry
		/// </summary>
		public int ProductLevel 
		{
			get { return _productLevel ; }
			set { _productLevel = value; }
		}

		// Methods
	}

	/// <summary>
	/// Summary description for ComputationModels.
	/// </summary>
	public class ComputationModels
	{
		// Fields
		private ArrayList _models;

		// Constructors

		public ComputationModels()
		{
			LoadModels();
		}

		// Properties

		/// <summary>
		/// Gets or sets the array of calendar models defined in the system
		/// </summary>
		public ArrayList Models 
		{
			get { return _models ; }
			set { _models = value; }
		}

		// Methods

		/// <summary>
		/// Loads all computation models.
		/// </summary>
		private void LoadModels()
		{
			ComputationData cd = new ComputationData();
			DataTable dt = cd.ComputationModel_Read();
			foreach(DataRow dr in dt.Rows)
			{
				ComputationModel cm = new ComputationModel(Convert.ToInt32(dr["COMP_MODEL_RID"]));

				this.Models.Add( cm );	// add model to model info
			}
		}
	}
}
