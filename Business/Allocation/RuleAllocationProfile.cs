using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// The Rule Allocation tracks allocations resulting from special rules
	/// </summary>
	public class RuleAllocationProfile:Profile
	{
		#region Fields
		//========//
		// FIELDS //
		//========//

		ApplicationSessionTransaction _transaction;
		ProfileList _allStoreList;
		int _methodRID;
		int _headerRID;
		GeneralComponent _component;
		int _componentRID;
		string _storeQtyTag;
		bool[] _storeChanged;
		eRuleType[] _ruleType;
		int[] _qtyAllocatedByRule;
		#endregion Fields

		#region Constructors
		//==============//
		// CONSTRUCTORS //
		//==============//
		/// <summary>
		/// Constructor to build new Rule Allocation Layer
		/// </summary>
		/// <param name="aMethodRID">RID of the Allocation Method creating this layer.</param>
		/// <param name="aTransaction">Application Session Transaction associated with this layer.</param>
		/// <param name="aHeaderRID">Header RID for which this layer is being built.</param>
		/// <param name="aComponent">Component for which this layer is being built.</param>
		public RuleAllocationProfile(int aMethodRID, ApplicationSessionTransaction aTransaction, int aHeaderRID, GeneralComponent aComponent):base(aTransaction.GetRuleLayerID(aHeaderRID, aComponent))
		{
			if (aMethodRID < 1)
			{
				throw new MIDException(eErrorLevel.fatal,
					(int)eMIDTextCode.msg_al_MethodRIDCannotBeLessThan1,
					MIDText.GetText(eMIDTextCode.msg_al_MethodRIDCannotBeLessThan1));
			}
			InitializeRule (aTransaction, aHeaderRID, aComponent);
			if (MethodRID != Include.NoRID)
			{
				throw new MIDException(eErrorLevel.fatal,
					(int)eMIDTextCode.msg_al_RuleAllocationAlreadyExists,
					MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationAlreadyExists));
			}
			_methodRID = aMethodRID;
		}

		/// <summary>
		/// Constructor to build existing Rule Allocation Layer
		/// </summary>
		/// <param name="aTransaction">Application Session Transaction associated with this layer.</param>
		/// <param name="aHeaderRID">Header RID for which this layer was built.</param>
		/// <param name="aComponent">Component for which this layer was built.</param>
		/// <param name="aLayerID">Layer ID to retrieve.</param>
		//BEGIN TT#575-MD - stodd - After a header is dropped onto a placeholder, both the placeholder and the header are in balance but it appears the Placeholder has too few units removed
		public RuleAllocationProfile(ApplicationSessionTransaction aTransaction, int aHeaderRID, GeneralComponent aComponent, int aLayerID, bool ignoreMethodRid):base(aLayerID)
		//END TT#575-MD - stodd - After a header is dropped onto a placeholder, both the placeholder and the header are in balance but it appears the Placeholder has too few units removed
		{
			if (aLayerID < 1)
			{
				throw new MIDException(eErrorLevel.fatal,
					(int)eMIDTextCode.msg_al_RuleLayerIDCannotBeLessThan1,
					MIDText.GetText(eMIDTextCode.msg_al_RuleLayerIDCannotBeLessThan1));
			}
			InitializeRule (aTransaction, aHeaderRID, aComponent);

			//BEGIN TT#575-MD - stodd - After a header is dropped onto a placeholder, both the placeholder and the header are in balance but it appears the Placeholder has too few units removed
			if (!ignoreMethodRid)
			{
				if (MethodRID == Include.NoRID)
				{
					throw new MIDException(eErrorLevel.fatal,
						(int)eMIDTextCode.msg_al_RuleAllocationDoesNotExist,
						MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationDoesNotExist));
				}
			}
			//END TT#575-MD - stodd - After a header is dropped onto a placeholder, both the placeholder and the header are in balance but it appears the Placeholder has too few units removed
		}

		/// <summary>
		/// Private method to initialize the rule values.
		/// </summary>
		/// <param name="aTransaction">Transaction associated with the rule</param>
		/// <param name="aHeaderRID">Header RID associated with the rule</param>
		/// <param name="aComponent">Component associated with the rule</param>
		private void InitializeRule(ApplicationSessionTransaction aTransaction, int aHeaderRID, GeneralComponent aComponent)
		{
			_transaction = aTransaction;
			_headerRID = aHeaderRID;
			_component = aComponent;
			_allStoreList =  (ProfileList)_transaction.GetMasterProfileList(eProfileType.Store);
			this._qtyAllocatedByRule = new int[_allStoreList.Count];
			this._qtyAllocatedByRule.Initialize();
			this._storeChanged = new bool[_allStoreList.Count];
			this._storeChanged.Initialize();
			this._ruleType = new eRuleType[_allStoreList.Count];
			for (int i=0; i < _allStoreList.Count; i++)
			{
				_ruleType[i] = eRuleType.None;
			}
			_storeQtyTag = "UNITS";
			LoadRuleAllocation();
		}
		#endregion Constructors

		#region Properties
		//============//
		// PROPERTIES //
		//============//
		/// <summary>
		/// Gets the Profile Type of this profile (RuleAllocation).
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.RuleAllocation;
			}
		}

		/// <summary>
		/// Gets the transaction associated with this Rule Allocation.
		/// </summary>
		public ApplicationSessionTransaction Transaction
		{
			get
			{
				return _transaction;
			}
		}

		/// <summary>
		/// Gets the header RID associated with this Rule Allocation.
		/// </summary>
		public int HeaderRID
		{
			get 
			{
				return _headerRID;
			}
		}

		/// <summary>
		/// Gets the component associated with this Rule Allocation.
		/// </summary>
		public GeneralComponent Component
		{
			get
			{
				return _component;
			}
		}

		/// <summary>
		/// Gets the component RID associated with this Rule Allocation.
		/// </summary>
		public int ComponentRID
		{
			get
			{
				return _componentRID;
			}
		}

		/// <summary>
		/// Gets or sets the layer ID associated with this Rule Allocation.
		/// </summary>
		public int LayerID
		{
			get
			{
				return base.Key;
			}
			set
			{
				base.Key = value;
			}
		}

		/// <summary>
		/// Gets or sets the method RID which determined the store rules.
		/// </summary>
		public int MethodRID
		{
			get
			{
				return _methodRID;
			}
			set
			{
				_methodRID = value;
			}
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#region StoreDimension
		/// <summary>
		/// Sets the store dimension
		/// </summary>
		/// <param name="aStoreCount">Number of stores.</param>
		internal void SetStoreDimension(int aStoreCount)
		{
			if (aStoreCount < 1)
			{
				//throw new MIDException (eErrorLevel.warning,  // MID track 5374 Workflow Errors do not stop Process
				throw new MIDException (eErrorLevel.severe,     // MID Track 5374 Workflow Errors do not stop process
					(int)eMIDTextCode.msg_NumberOfStoresCannotBeLessThan1,
					MIDText.GetText(eMIDTextCode.msg_NumberOfStoresCannotBeLessThan1));
			}
			else
			{
				_storeChanged = new bool[aStoreCount];
				_ruleType = new eRuleType[aStoreCount];
				_qtyAllocatedByRule = new int[aStoreCount];
				_qtyAllocatedByRule.Initialize();
				for (int i = 0; i < aStoreCount; ++i)
				{
					_storeChanged[i] = false;
					_ruleType[i] = eRuleType.None;
				}
			}
		}
		#endregion StoreDimension

		#region StoreIndexRID
		/// <summary>
		/// Gets store index value for store RID.
		/// </summary>
		/// <param name="aStoreRID">RID identifier for the store.</param>
		/// <returns>Store Index associated with the storeRID</returns>
		public Index_RID StoreIndex(int aStoreRID)
		{
			Index_RID sIndexRID = (_transaction).StoreIndexRID(aStoreRID);
			if (sIndexRID.RID == Include.UndefinedStoreRID)
			{
				// begin MID Track 4214 Identify stores in error messages
				throw new MIDException (eErrorLevel.severe,
					(int)(eMIDTextCode.msg_StoreRIDNotFound),
					string.Format(MIDText.GetText(eMIDTextCode.msg_StoreRIDNotFound),aStoreRID.ToString()));
				//throw new MIDException(eErrorLevel.severe,
				//	(int)(eMIDTextCode.msg_StoreRIDNotFound),
				//	MIDText.GetText(eMIDTextCode.msg_StoreRIDNotFound));
				// end MID Track 4214 Identify stores in error messages
			}
			return sIndexRID;
		}
		#endregion StoreIndexRID

		#region SetStoreRuleAllocation
		/// <summary>
		/// Sets Rule Allocation for the given store
		/// </summary>
		/// <param name="aStoreRID">Store RID</param>
		/// <param name="aRuleType">Rule Type</param>
		/// <param name="aQtyAllocated">Quantity Allocated</param>
		public void SetStoreRuleAllocation (int aStoreRID, eRuleType aRuleType, int aQtyAllocated)
		{
			SetStoreRuleAllocation (StoreIndex(aStoreRID), aRuleType, aQtyAllocated);
		}

		/// <summary>
		/// Sets Rule Allocation for the given store
		/// </summary>
		/// <param name="aStore">Store Index_RID that describes the store</param>
		/// <param name="aRuleType">Rule Type</param>
		/// <param name="aQtyAllocated">Quantity Allocated</param>
		internal void SetStoreRuleAllocation (Index_RID aStore, eRuleType aRuleType, int aQtyAllocated)
		{
			this.SetStoreQtyAllocatedByRule(aStore, aQtyAllocated);
			this.SetStoreRuleType (aStore, aRuleType);
		}
		#endregion SetStoreRuleAllocation

		#region StoreRuleType
		#region GetStoreRuleType
		/// <summary>
		/// Gets rule type that determined rule allocation for the store.
		/// </summary>
		/// <param name="aStoreRID">Store RID that identifies the store</param>
		/// <returns>eRuleType that determined the store allocation</returns>
		public eRuleType GetStoreRuleType (int aStoreRID)
		{
			return GetStoreRuleType(StoreIndex(aStoreRID));
		}

		/// <summary>
		/// Gets rule type that determined rule allocation for the store.
		/// </summary>
		/// <param name="aStore">Store Index_RID that identifies the store</param>
		/// <returns>eRuleType that determined the store allocation</returns>
		internal eRuleType GetStoreRuleType (Index_RID aStore)
		{
			return _ruleType[aStore.Index];
		}
		#endregion GetStoreRuleType

		#region SetStoreRuleType
		/// <summary>
		/// Sets Store Rule Type that determines rule allocation for the store.
		/// </summary>
		/// <param name="aStoreRID">Store RID that identifies the store</param>
		/// <param name="aRuleType">Rule Type</param>
		private void SetStoreRuleType (int aStoreRID, eRuleType aRuleType)
		{
			SetStoreRuleType (StoreIndex(aStoreRID), aRuleType);
		}

		/// <summary>
		/// Sets Store Rule Type that determines rule allocation for the store.
		/// </summary>
		/// <param name="aStore">Store Index_RID that identifies the store</param>
		/// <param name="aRuleType">Rule Type</param>
		private void SetStoreRuleType (Index_RID aStore, eRuleType aRuleType)
		{
			if (_ruleType[aStore.Index] != aRuleType)
			{
				_ruleType[aStore.Index] = aRuleType;
				_storeChanged[aStore.Index] = true;
			}
		}
		#endregion SetStoreRuleType
		#endregion StoreRuleType

		#region StoreQtyAllocatedByRule
		#region GetStoreQtyAllocatedByRule
		/// <summary>
		/// Gets Store Quantity Allocated By Rule
		/// </summary>
		/// <param name="aStoreRID">Store RID that identifies the store.</param>
		/// <returns>Quantity allocated by rule.</returns>
		public int GetStoreQtyAllocatedByRule (int aStoreRID)
		{
			return GetStoreQtyAllocatedByRule (StoreIndex(aStoreRID));
		}

		/// <summary>
		/// Gets Store Quantity Allocated By Rule
		/// </summary>
		/// <param name="aStore">Store Index_RID that identifies the store.</param>
		/// <returns>Quantity allocated by rule.</returns>
		internal int GetStoreQtyAllocatedByRule (Index_RID aStore)
		{
			return _qtyAllocatedByRule[aStore.Index];
		}
		#endregion GetStoreQtyAllocatedByRule

		#region SetStoreQtyAllocatedByRule
        //Begin TT#855-MD -jsobek -Velocity Enhancements -unused function
        ///// <summary>
        ///// Sets Store Quantity Allocated By Rule
        ///// </summary>
        ///// <param name="aStoreRID">Store RID that identifies the store.</param>
        ///// <param name="aQtyAllocated">Quantity Allocated</param>
        //private void SetStoreQtyAllocatedByRule (int aStoreRID, int aQtyAllocated)
        //{
        //    SetStoreQtyAllocatedByRule (StoreIndex(aStoreRID), aQtyAllocated);
        //}
        //End TT#855-MD -jsobek -Velocity Enhancements -unused function

		/// <summary>
		/// Sets Store Quantity Allocated By Rule
		/// </summary>
		/// <param name="aStore">Store Index_RID that identifies the store.</param>
		/// <param name="aQtyAllocated">Quantity Allocated</param>
		private void SetStoreQtyAllocatedByRule (Index_RID aStore, int aQtyAllocated)
		{
			if (aQtyAllocated < 0)
			{
				throw new MIDException(eErrorLevel.warning,
					(int)eMIDTextCode.msg_QtyAllocatedCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg));
			}
			if (aQtyAllocated != _qtyAllocatedByRule[aStore.Index])
			{
				_qtyAllocatedByRule[aStore.Index] = aQtyAllocated;
				_storeChanged[aStore.Index] = true;
			}
		}
		#endregion SetStoreQtyAllocatedByRule
		#endregion StoreQtyAllocatedByRule

		#region DatabaseAccess
		#region Load
		/// <summary>
		/// Loads the rule allocation from the database.
		/// </summary>
		private void LoadRuleAllocation()
		{
			RuleAllocation ra = new RuleAllocation();
			System.Data.DataTable dt;
			Index_RID storeIdxRID;
			switch (this.Component.ComponentType)
			{
				case (eComponentType.Total):
				case (eComponentType.Bulk):
				case (eComponentType.DetailType):
				{
					_componentRID = Include.NoRID;
					break;
				}
				case (eComponentType.SpecificPack):
				{
					AllocationPackComponent pack = (AllocationPackComponent)this.Component;
					_componentRID = ra.GetPackRID(this.HeaderRID, pack.PackName);
					// Begin TT#4485 - JSmith - Rule Method error with specific pack and pack not on header
                    if (_componentRID == Include.NoRID)
                    {
                        throw new MIDException(eErrorLevel.warning,
                        (int)eMIDTextCode.msg_PackNotDefinedOnHeader,
                        MIDText.GetText(eMIDTextCode.msg_PackNotDefinedOnHeader, false));  // MID Track 5374 Workflow not stopping on error
                    }
					// End TT#4485 - JSmith - Rule Method error with specific pack and pack not on header
					_storeQtyTag = "PACKS";
					break;
				}
				case (eComponentType.SpecificColor):
				{
					AllocationColorOrSizeComponent color = (AllocationColorOrSizeComponent)this.Component;
					_componentRID = color.ColorRID;
                    // Begin TT#4490 - JSmith - Rule Method error with specific color and color is not on header
                    int hdrBCRID = ra.GetBulkColorRID(this.HeaderRID, _componentRID);
                    if (hdrBCRID == Include.NoRID)
                    {
                        throw new MIDException(eErrorLevel.warning,
                        (int)eMIDTextCode.msg_al_NoMatchingColors,
                        MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors, false));  
                    }
                    // End TT#4490 - JSmith - Rule Method error with specific color and color is not on header
					break;
				}
				case (eComponentType.AllColors):
				case (eComponentType.AllGenericPacks):
				case (eComponentType.AllNonGenericPacks):
				case (eComponentType.AllPacks):
				case (eComponentType.AllSizes):
				case (eComponentType.ColorAndSize):
				case (eComponentType.GenericType):
				case (eComponentType.SpecificSize):
				{
					throw new MIDException (eErrorLevel.fatal,
						(int)eMIDTextCode.msg_al_ComponentMustBeSpecific,
						MIDText.GetText(eMIDTextCode.msg_al_ComponentMustBeSpecific));
				}
				default:
				{
					throw new MIDException(eErrorLevel.fatal,
						(int)eMIDTextCode.msg_al_UnknownComponentType,
						MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
				}
			}
			dt = ra.GetLayerInfo(this.HeaderRID, this.Component.ComponentType, this.ComponentRID, this.LayerID);
			if (dt.Rows.Count != 1)
			{
				this.MethodRID = Include.NoRID;
			}
			else
			{
				System.Data.DataRow dr = dt.Rows[0];
				this.MethodRID = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);
			}

			dt = ra.GetStoreComponentRules(this.HeaderRID, this.Component.ComponentType, this.ComponentRID, this.LayerID);
			foreach (System.Data.DataRow dr in dt.Rows)
			{
				int storeRID = Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture);
				StoreProfile sp = (StoreProfile)_allStoreList.FindKey(storeRID);
				if (sp == null)
				{
					// store was added to the database
				}
				else
				{
					storeIdxRID = _transaction.StoreIndexRID(storeRID);
					this._qtyAllocatedByRule[storeIdxRID.Index] = Convert.ToInt32(dr[_storeQtyTag], CultureInfo.CurrentUICulture);
					this._ruleType[storeIdxRID.Index] = (eRuleType)Convert.ToInt32(dr["RULE_TYPE_ID"], CultureInfo.CurrentUICulture);
				}
			}
		}
		#endregion Load

		#region Write
		/// <summary>
		/// Write this Rule Allocation to the database.
		/// </summary>
		public bool WriteRuleAllocation()
		{
			RuleAllocation ra = new RuleAllocation();
			bool success = true;
			bool connectionIsOpened = false;

			try
			{
				ra.OpenUpdateConnection();
				if (ra.WriteRuleLayerID(this.MethodRID,	this.HeaderRID,	this.Component.ComponentType, this.ComponentRID, this.LayerID))
				{
					// This has to be commited now. During the WriteRuleAllocationByStore() it tries to read the rule layer
					// is locked out by the above insert.  (In Oracle only I suspect.  stodd)
					ra.CommitData();
//					ra.CloseUpdateConnection();
//					ra.OpenUpdateConnection();

					Index_RID storeIdxRID;
					if (this.Component.ComponentType == eComponentType.Bulk ||
						this.Component.ComponentType == eComponentType.Total ||
						this.Component.ComponentType == eComponentType.DetailType ||
						this.Component.ComponentType == eComponentType.SpecificColor ||
						this.Component.ComponentType == eComponentType.SpecificPack )
					{
						ra.Rule_XMLInit();
						foreach (StoreProfile sp in this._allStoreList)
						{
							storeIdxRID = StoreIndex(sp.Key);
							if (this._storeChanged[storeIdxRID.Index])
							{
								if (!ra.XMLWriteRuleAllocationByStore
									(
									this.HeaderRID,
									this.Component.ComponentType,
									this.ComponentRID,
									this.LayerID,
									storeIdxRID.RID,
									this.GetStoreRuleType(storeIdxRID),
									this.GetStoreQtyAllocatedByRule(storeIdxRID)
									)
									)
								{
									success = false;
								}
							}
						}
						if (success)
						{
							//						ra.CommitData();
							ra.Rule_XMLWrite(this.HeaderRID,
								this.Component.ComponentType,
								this.ComponentRID,
								this.LayerID);
						}
					}
					else
					{
						ra.OpenUpdateConnection();
						connectionIsOpened = true;
						foreach (StoreProfile sp in this._allStoreList)
						{
							storeIdxRID = StoreIndex(sp.Key);
							if (this._storeChanged[storeIdxRID.Index])
							{
								if (!ra.WriteRuleAllocationByStore
									(
									this.HeaderRID,
									this.Component.ComponentType,
									this.ComponentRID,
									this.LayerID,
									storeIdxRID.RID,
									this.GetStoreRuleType(storeIdxRID),
									this.GetStoreQtyAllocatedByRule(storeIdxRID)
									)
									)
								{
									success = false;
								}
							}
						}
						if (success)
						{
							ra.CommitData();
						}
					}
					this._storeChanged.Initialize();
				}
				else
				{
					success = false;
				}
				// BEGIN MID Change j.ellis Allow "soft" rule in interactive veloclty
				if (success)
				{
					this._transaction.UpdateLayerID(this._headerRID, this.Component, this.LayerID);
				}
				// BEGIN MID Change j.ellis Allow "soft" rule in interactive veloclty
			}
			catch ( Exception )  // MID Change j.ellis removed unused variable.
			{
				success = false;
				throw;
			}
			finally
			{
				if (connectionIsOpened)
				{
					ra.CloseUpdateConnection();
				}
			}

			//			_session.Audit.Add_Msg(
			//				eMIDMessageLevel.Information,
			//				eMIDTextCode.msg_RuleAllocationUpdateSuccess,
			//				_headerRID,
			//              _layerID
			//				this.GetType().Name);
			return success;
		}

		#endregion Write
		#endregion DatabaseAccess
		#endregion Methods
	}
}

