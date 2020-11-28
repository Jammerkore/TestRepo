using System;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Wraps the GeneralComponent class and creates an instance of the GeneralComponent for the 
	/// provided component type.
	/// </summary>
	public class GeneralComponentWrapper
	{
		//=======
		// FIELDS
		//=======
		private eComponentType _componentType;
		private int _colorRID;
		private int _sizeRID;
		private string _packName;
		private int _headerRID; // issue 4108
		private GeneralComponent _generalComponent;
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aComponentType">Component Type</param>
		public GeneralComponentWrapper(eComponentType aComponentType, int aColorRID, int aSizeRID, string aPackName)
		{
			_componentType = aComponentType;
			_colorRID = aColorRID;
			_sizeRID = aSizeRID;
			_packName = aPackName;
			_headerRID = Include.NoRID; // Issue 4108
			switch (_componentType)
			{
				case eComponentType.SpecificColor:
					_generalComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, _colorRID);
					break;
				case eComponentType.SpecificSize:
					_generalComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificSize, _sizeRID);
					break;
				case eComponentType.SpecificPack:
					_generalComponent = new AllocationPackComponent(_packName);
					break;
				default:
					_generalComponent = new GeneralComponent(_componentType);
					break;

			}
		}

		public GeneralComponentWrapper(GeneralComponent aGeneralComponent)
		{
			_componentType = aGeneralComponent.ComponentType;
			_colorRID = Include.NoRID;
			_sizeRID = Include.NoRID;
			_packName = string.Empty;
			_headerRID = Include.NoRID; // Issue 4108
			switch (_componentType)
			{
				case eComponentType.SpecificColor:
					AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)aGeneralComponent;
					_colorRID = colorComponent.ColorRID;
					break;
				case eComponentType.SpecificSize:
					AllocationColorOrSizeComponent sizeComponent = (AllocationColorOrSizeComponent)aGeneralComponent;
					_sizeRID = sizeComponent.SizeRID;
					break;
				case eComponentType.SpecificPack:
					AllocationPackComponent packComponent = (AllocationPackComponent)aGeneralComponent;
					_packName = packComponent.PackName;
					break;
				default:
					_generalComponent = aGeneralComponent;
					break;

			}
		}
		// Begin Issue 4108
		public GeneralComponentWrapper(int aHeaderRID, GeneralComponent aGeneralComponent)
		{
			_componentType = aGeneralComponent.ComponentType;
			_generalComponent = aGeneralComponent;
			_colorRID = Include.NoRID;
			_sizeRID = Include.NoRID;
			_packName = string.Empty;
			_headerRID = aHeaderRID; // Issue 4108
			switch (_componentType)
			{
				case eComponentType.SpecificColor:
					AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)aGeneralComponent;
					_colorRID = colorComponent.ColorRID;
					break;
				case eComponentType.SpecificSize:
					AllocationColorOrSizeComponent sizeComponent = (AllocationColorOrSizeComponent)aGeneralComponent;
					_sizeRID = sizeComponent.SizeRID;
					break;
				case eComponentType.SpecificPack:
					AllocationPackComponent packComponent = (AllocationPackComponent)aGeneralComponent;
					_packName = packComponent.PackName;
					break;
				default:
					break;

			}
		}
		// end issue 4108

		//===========
		// PROPERTIES
		//===========
		

		/// <summary>
		/// Gets the Component type of this component
		/// </summary>
		public eComponentType ComponentType
		{
			get
			{
				return _componentType;
			}
		}

		/// <summary>
		/// Gets the color record ID of this component
		/// </summary>
		public int ColorRID
		{
			get
			{
				return _colorRID;
			}
		}

		/// <summary>
		/// Gets the size record ID of this component
		/// </summary>
		public int SizeRID
		{
			get
			{
				return _sizeRID;
			}
		}

		/// <summary>
		/// Gets the pack name of this component
		/// </summary>
		public string PackName
		{
			get
			{
				return _packName;
			}
		}

		public GeneralComponent GeneralComponent
		{
			get
			{
				return _generalComponent;
			}
		}

		//Begin Issue 4108
		/// <summary>
		/// Gets the header rid of this component
		/// </summary>
		public int HeaderRID
		{
			get
			{
				return _headerRID;
			}
		}
		// End issue 4108
		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Describes a general component to select for allocation.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A general component is the base class for all components.  The components are divided into four
	/// category types: General, Pack, ColorOrSize and ColorAndSize.  
	/// </para><para>
	/// The general category selects components using a generic description of the components desired. 
	/// The general category includes components: Total, GenericType, DetailType, AllPacks, AllGenericPacks,
	/// AllNonGenericPacks, Bulk, AllColors and AllSizes.
	/// </para><para>
	/// The pack category selects components using a specific pack name.
	/// </para><para>
	/// The ColorOrSize category selects bulk components using a specific color RID or a specific size RID
	/// (never both simultaneously).
	/// </para><para>
	/// The ColorAndSize category selects bulk components based on combined color and size criteria.
	/// </para><para>
	/// The Allocation Components are:
	/// <list type="bullet">
	/// <item>Total:  General Component that selects the grand total allocation bucket.</item>
	/// <item>GenericType: General Component that selects the total "generic pack" allocation bucket.</item>
	/// <item>DetailType:  General Component that selects the total "detail" allocation bucket.</item>
	/// <item>AllPacks: General Component that selects all packs regardless of type.</item>
	/// <item>AllGenericPacks: General Component that selects all generic packs.</item>
	/// <item>AllNonGenericPacks: General Component that selects all non-generic packs.</item>
	/// <item>SpecificPack: Specific pack component selector.</item>
	/// <item>Bulk: General Component that selects the Bulk allocation bucket.</item>
	/// <item>AllColors: General Component that selects all colors.</item>
	/// <item>SpecificColor: Specific Bulk component selector that selects an instance of a color.</item>
	/// <item>AllSizes: General Component that selects all sizes (typically used in conjunction with a color component selector).</item>
	/// <item>SpecificSize: Specific Bulk component selector that selects an instance of a size (typically used in conjunction with a color component selector).</item>
	/// <item>ColorAndSize: ColorAndSize selector that identifies colors and sizes to select.</item>
	/// </list></para>
	/// </remarks>
    [Serializable]
	public class GeneralComponent
	{
		//=======
		// FIELDS
		//=======
		private eComponentType _componentType;
		//=============
		// CONSTRUCTORS
		//=============
	    /// <summary>
	    /// Creates an instance of this class.
	    /// </summary>
	    /// <param name="aComponentType">General Component Type</param>
		public GeneralComponent(eGeneralComponentType aComponentType)
		{
			_componentType = ((eComponentType)aComponentType);
		}
		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aComponentType">Component Type</param>
		internal GeneralComponent(eComponentType aComponentType)
		{
			_componentType = aComponentType;
		}
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets the component category for this component.
		/// </summary>
		virtual public eComponentCategory ComponentCategory
		{
			get
			{
				return eComponentCategory.General;
			}
		}

		/// <summary>
		/// Gets the Component type of this component
		/// </summary>
		public eComponentType ComponentType
		{
			get
			{
				return _componentType;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Describes a pack component.
	/// </summary>
	/// <remarks>
	/// Inherits from the general component.  In addition to the component type, the selected
	/// specific pack name resides in this component selector.
	/// </remarks>
	[Serializable] 
	public class AllocationPackComponent:GeneralComponent
	{
		//=======
		// FIELDS
		//=======
		private string _packName;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aPackName">Pack name associated with this selector.</param>
		public AllocationPackComponent(string aPackName):base(eComponentType.SpecificPack)
		{
			_packName = aPackName;
		}
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets the category for this component.
		/// </summary>
		override public eComponentCategory ComponentCategory
		{
			get
			{
				return eComponentCategory.PackSpecific;
			}
		}
		/// <summary>
		/// Gets the pack name associated with this component.
		/// </summary>
		public string PackName
		{
			get
			{
				return _packName;
			}
		}
	}

	/// <summary>
	/// Describes the ColorOrSizeComponent (not simultaneously).
	/// </summary>
	[Serializable] 
	public class AllocationColorOrSizeComponent:GeneralComponent
	{
		//=======
		// FIELDS
		//=======
		private int _componentRID;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of a color or size component (exclusive or)
		/// </summary>
		/// <param name="aBulkType">Bulk type: SpecificColor or SpecificSize</param>
		/// <param name="aColorOrSizeRID"></param>
		public AllocationColorOrSizeComponent(eSpecificBulkType aBulkType, int aColorOrSizeRID):base((eComponentType)aBulkType)
		{
			_componentRID = aColorOrSizeRID;
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets the component category
		/// </summary>
		override public eComponentCategory ComponentCategory
		{
			get
			{
				return eComponentCategory.ColorOrSizeSpecific;
			}
		}

		/// <summary>
		/// Gets the colorRID if the component type is color.
		/// </summary>
		public int ColorRID
		{
			get
			{
				if (base.ComponentType == eComponentType.SpecificColor)
				{
					return _componentRID;
				}
				else
				{
					//throw new MIDException (eErrorLevel.warning,  // MID track 5374 Workflow Errors do not stop Process
					throw new MIDException (eErrorLevel.severe,     // MID Track 5374 Workflow Errors do not stop process
						(int)eMIDTextCode.msg_NotSpecificColorComponent,
						MIDText.GetText(eMIDTextCode.msg_NotSpecificColorComponent));
				}
			}
		}

		/// <summary>
		/// Gets the Size RID if the component type is size.
		/// </summary>
		public int SizeRID
		{
			get
			{
				if (base.ComponentType == eComponentType.SpecificSize)
				{
					return _componentRID;
				}
				else
				{
					//throw new MIDException (eErrorLevel.warning,  // MID track 5374 Workflow Errors do not stop Process
					throw new MIDException (eErrorLevel.severe,     // MID Track 5374 Workflow Errors do not stop process
						(int)eMIDTextCode.msg_NotSpecificSizeComponent,
						MIDText.GetText(eMIDTextCode.msg_NotSpecificSizeComponent));
				}
			}
		}

		/// <summary>
		/// Gets the Primary Size Dimension RID if the component type is Primary Size.
		/// </summary>
		public int PrimarySizeDimRID
		{
			get
			{
				if (base.ComponentType == eComponentType.SpecificSizePrimaryDim)
				{
					return _componentRID;
				}
				else
				{
					//throw new MIDException (eErrorLevel.warning,  // MID track 5374 Workflow Errors do not stop Process
					throw new MIDException (eErrorLevel.severe,     // MID Track 5374 Workflow Errors do not stop process
						(int)eMIDTextCode.msg_NotSpecificPrimarySizeComponent,
                        MIDText.GetText(eMIDTextCode.msg_NotSpecificPrimarySizeComponent)); // TT#1008 Size Review error when zeroing size total
				}
			}
		}

		/// <summary>
		/// Gets the Secondary Size Dimension RID if the component type is Secondary Size.
		/// </summary>
		public int SecondarySizeDimRID
		{
			get
			{
				if (base.ComponentType == eComponentType.SpecificSizeSecondaryDim)
				{
					return _componentRID;
				}
				else
				{
					//throw new MIDException (eErrorLevel.warning,  // MID track 5374 Workflow Errors do not stop Process
					throw new MIDException (eErrorLevel.severe,     // MID Track 5374 Workflow Errors do not stop process
						(int)eMIDTextCode.msg_NotSpecificSecondarySizeComponent,
                        MIDText.GetText(eMIDTextCode.msg_NotSpecificSecondarySizeComponent));  // TT#1008 Size Review error when zeroing size total column
				}
			}
		}
	}

	/// <summary>
	/// Describes a color size component that select colors and the indicated sizes within the selected colors.
	/// </summary>
	[Serializable] 
	public class AllocationColorSizeComponent:GeneralComponent
	{
		//=======
		// FIELDS
		//=======
		private GeneralComponent _colorComponent;
		private GeneralComponent _sizeComponent;
	
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aColorComponent">The color component</param>
		/// <param name="aSizeComponent"></param>
		public AllocationColorSizeComponent(GeneralComponent aColorComponent, GeneralComponent aSizeComponent):base(eComponentType.ColorAndSize)
		{
			if (aColorComponent.ComponentType == eComponentType.SpecificColor
				|| aColorComponent.ComponentType == eComponentType.AllColors)
			{
				_colorComponent = aColorComponent;
			}
			else
			{
				//throw new MIDException (eErrorLevel.warning,  // MID track 5374 Workflow Errors do not stop Process
				throw new MIDException (eErrorLevel.severe,     // MID Track 5374 Workflow Errors do not stop process
					(int)eMIDTextCode.msg_NotColorComponent,
					MIDText.GetText(eMIDTextCode.msg_NotColorComponent));
			}
			if (aSizeComponent.ComponentType == eComponentType.SpecificSize
				|| aSizeComponent.ComponentType == eComponentType.AllSizes
				|| aSizeComponent.ComponentType == eComponentType.SpecificSizePrimaryDim
				|| aSizeComponent.ComponentType == eComponentType.SpecificSizeSecondaryDim)
			{
				_sizeComponent = aSizeComponent;
			}
			else
			{
				//throw new MIDException (eErrorLevel.warning,  // MID track 5374 Workflow Errors do not stop Process
				throw new MIDException (eErrorLevel.severe,     // MID Track 5374 Workflow Errors do not stop process
					(int)eMIDTextCode.msg_NotSizeComponent,
					MIDText.GetText(eMIDTextCode.msg_NotSizeComponent));
			}
		}

		//===========
		// PROPERTIES
        //===========
		/// <summary>
		/// Gets the category of this component.
		/// </summary>
		override public eComponentCategory ComponentCategory
		{
			get
			{
				return eComponentCategory.ColorAndSize;
			}
		}

        /// <summary>
        /// Gets the associated color component.
        /// </summary>
		public GeneralComponent ColorComponent
		{
			get
			{
				return _colorComponent;
			}
		}

		/// <summary>
		/// Gets the associated size component.
		/// </summary>
		public GeneralComponent SizeComponent
		{
			get
			{
				return _sizeComponent;
			}
		}
	}
}

