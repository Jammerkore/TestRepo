using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

using MIDRetail.DataCommon;

namespace MIDRetail.Windows.Controls
{
	/// <summary>
	/// Used to save and retrieve information from the Windows clipboard.
	/// </summary>
	[Serializable()]
	abstract public class ClipboardProfileBase : Profile
	{
		private DragDropEffects _action;
		private Image _dragImage;
		private int _dragImageHeight;
		private int _dragImageWidth;
        private string _text;
        private SecurityProfile _securityProfile;
        private int _ownerUserRID;

        public ClipboardProfileBase(
            int aKey, 
            string aText, 
            SecurityProfile aSecurityProfile, 
            int aOwnerUserRID
            )
			: base(aKey)
		{
            _text = aText;
            _securityProfile = aSecurityProfile;
            _ownerUserRID = aOwnerUserRID;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Clipboard;
			}
		}

        /// <summary>
        /// Gets or sets the text associated with clipboard data.
        /// </summary>
        public string Text
        {
            get { return _text; }
        }

		/// <summary>
		/// Gets the security profile of data being put to the clipboard.
		/// </summary>
        public SecurityProfile FunctionSecurityProfile
		{
            get { return _securityProfile; }
		}

		/// <summary>
		/// Gets or sets the action being performed when the data is being put to the clipboard.
		/// </summary>
		public DragDropEffects Action
		{
			get { return _action; }
			set { _action = value; }
		}

		/// <summary>
		/// Gets the key of the owner of the Method or Workflow.
		/// </summary>
		public int OwnerUserRID
		{
            get { return _ownerUserRID; }
		}

		/// <summary>
		/// Gets or sets the image to use during a drag operation.
		/// </summary>
		public Image DragImage
		{
			get { return _dragImage; }
			set { _dragImage = value; }
		}

		/// <summary>
		/// Gets or sets the image height to use during a drag operation.
		/// </summary>
		public int DragImageHeight
		{
			get { return _dragImageHeight; }
			set { _dragImageHeight = value; }
		}

		/// <summary>
		/// Gets or sets the image width to use during a drag operation.
		/// </summary>
		public int DragImageWidth
		{
			get { return _dragImageWidth; }
			set { _dragImageWidth = value; }
		}
	}

    [Serializable()]
    abstract public class ClipboardListBase
    {
        private eProfileType _clipboardDataType;
        private ArrayList _alClipboardItems = null;

        public ClipboardListBase(eProfileType aClipboardDataType)
        {
            _clipboardDataType = aClipboardDataType;
        }

        public eProfileType ClipboardDataType
        {
            get { return _clipboardDataType; }
        }

        public ArrayList ClipboardItems
        {
            get
            {
                if (_alClipboardItems == null)
                {
                    _alClipboardItems = new ArrayList();
                }
                return _alClipboardItems;
            }
        }
    }

    /// <summary>
    /// Used to save and retrieve information from the Windows clipboard.
    /// </summary>
    [Serializable()]
    public class TreeNodeClipboardProfile : ClipboardProfileBase
    {
        private MIDTreeNode _node;

        public TreeNodeClipboardProfile(MIDTreeNode aNode)
            : base(aNode.Profile.Key, aNode.Text, aNode.FunctionSecurityProfile, aNode.OwnerUserRID)
        {
            _node = aNode;
        }

        /// <summary>
        /// Gets or sets the MIDTreeNode associates with this ClipboardProfile
        /// </summary>
        public MIDTreeNode Node
        {
            get { return _node; }
        }
    }

    [Serializable()]
    public class TreeNodeClipboardList : ClipboardListBase
    {
        public TreeNodeClipboardList(eProfileType aClipboardDataType)
            : base(aClipboardDataType)
		{
            
		}

        public TreeNodeClipboardProfile ClipboardProfile
        {
            get
            {
                return (TreeNodeClipboardProfile)ClipboardItems[0];
            }
        }
    }

    /// <summary>
    /// Used to save and retrieve information from the Windows clipboard.
    /// </summary>
    [Serializable()]
    public class HeaderClipboardProfile : ClipboardProfileBase
    {
        string _headerName;

        public HeaderClipboardProfile(
            int aKey, 
            string aText, 
            FunctionSecurityProfile aFunctionSecurityProfile)
            : base(aKey, aText, aFunctionSecurityProfile, Include.NoRID)
        {

        }

        /// <summary>
        /// Gets the Header name associated with this ClipboardProfile
        /// </summary>
        public string HeaderName
        {
            get { return base.Text; }
        }
        
    }

    [Serializable()]
    public class HeaderClipboardList : ClipboardListBase
    {
        public HeaderClipboardList()
            : base(eProfileType.HeaderType)
        {

        }

        public HeaderClipboardProfile ClipboardProfile
        {
            get
            {
                return (HeaderClipboardProfile)ClipboardItems[0];
            }
        }
    }

    /// <summary>
    /// Used to save and retrieve information from the Windows clipboard.
    /// </summary>
    [Serializable()]
    public class ProductCharacteristicClipboardProfile : ClipboardProfileBase
    {
        private eProfileType _nodeType;
        private int _productCharGroupKey;

        public ProductCharacteristicClipboardProfile(
            int aKey,
            string aText,
            FunctionSecurityProfile aFunctionSecurityProfile)
            : base(aKey, aText, aFunctionSecurityProfile, Include.NoRID)
        {

        }

        /// <summary>
        /// Gets or sets the type of node being put to the clipboard.
        /// </summary>
        public eProfileType NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

        /// <summary>
        /// Gets or sets the key of the product group for the value being put to the clipboard.
        /// </summary>
        public int ProductCharGroupKey
        {
            get { return _productCharGroupKey; }
            set { _productCharGroupKey = value; }
        }

    }

    [Serializable()]
    public class ProductCharacteristicClipboardList : ClipboardListBase
    {
        public ProductCharacteristicClipboardList()
            : base(eProfileType.ProductCharacteristicValue)
        {

        }

        public ProductCharacteristicClipboardProfile ClipboardProfile
        {
            get
            {
                return (ProductCharacteristicClipboardProfile)ClipboardItems[0];
            }
        }
    }

    /// <summary>
    /// Used to save and retrieve information from the Windows clipboard.
    /// </summary>
    [Serializable()]
    public class HierarchyNodeClipboardProfile : ClipboardProfileBase
    {
        private int _hierarchyRID;
        private eHierarchyType _hierarchyType;
        private int _homeHierarchyRID;
        private eHierarchyType _homeHierarchyType;
        private int _parentRID;
        private eProfileType _nodeType;

        public HierarchyNodeClipboardProfile(
            int aKey,
            string aText,
            FunctionSecurityProfile aFunctionSecurityProfile)
            : base(aKey, aText, aFunctionSecurityProfile, Include.NoRID)
        {
            
        }

        /// <summary>
        /// Gets or sets the hierarchy record ID for the node being put to the clipboard.
        /// </summary>
        public int HierarchyRID
        {
            get { return _hierarchyRID; }
            set { _hierarchyRID = value; }
        }
        /// <summary>
        /// Gets or sets the hierarchy type for the node being put to the clipboard.
        /// </summary>
        public eHierarchyType HierarchyType
        {
            get { return _hierarchyType; }
            set { _hierarchyType = value; }
        }
        /// <summary>
        /// Gets or sets the home hierarchy record ID for the node being put to the clipboard.
        /// </summary>
        public int HomeHierarchyRID
        {
            get { return _homeHierarchyRID; }
            set { _homeHierarchyRID = value; }
        }

        /// <summary>
        /// Gets or sets the homehierarchy type for the node being put to the clipboard.
        /// </summary>
        public eHierarchyType HomeHierarchyType
        {
            get { return _homeHierarchyType; }
            set { _homeHierarchyType = value; }
        }

        /// <summary>
        /// Gets or sets the record ID of the parent for the node being put to the clipboard.
        /// </summary>
        public int ParentRID
        {
            get { return _parentRID; }
            set { _parentRID = value; }
        }

        /// <summary>
        /// Gets or sets the type of node being put to the clipboard.
        /// </summary>
        public eProfileType NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }
    }

    [Serializable()]
    public class HierarchyNodeClipboardList : ClipboardListBase
    {
        public HierarchyNodeClipboardList()
            : base(eProfileType.HierarchyNode)
        {

        }

        public HierarchyNodeClipboardProfile ClipboardProfile
        {
            get
            {
                return (HierarchyNodeClipboardProfile)ClipboardItems[0];
            }
        }
    }

    /// <summary>
    /// Used to save and retrieve information from the Windows clipboard.
    /// </summary>
    [Serializable()]
    public class ColorCodeClipboardProfile : ClipboardProfileBase
    {
        private string _colorName;
        private eProfileType _nodeType;

        public ColorCodeClipboardProfile(
            int aKey,
            string aText,
            FunctionSecurityProfile aFunctionSecurityProfile)
            : base(aKey, aText, aFunctionSecurityProfile, Include.NoRID)
        {

        }

        /// <summary>
        /// Gets or sets the name for the color being put to the clipboard.
        /// </summary>
        public string ColorName
        {
            get { return _colorName; }
            set { _colorName = value; }
        }

        /// <summary>
        /// Gets or sets the type of node being put to the clipboard.
        /// </summary>
        public eProfileType NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

    }

    [Serializable()]
    public class ColorCodeClipboardList : ClipboardListBase
    {
        public ColorCodeClipboardList()
            : base(eProfileType.ColorCode)
        {

        }

        public ColorCodeClipboardProfile ClipboardProfile
        {
            get
            {
                return (ColorCodeClipboardProfile)ClipboardItems[0];
            }
        }
    }
}
