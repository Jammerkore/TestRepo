using System;
using System.Drawing;
using System.Windows.Forms;

using MID.MRS.DataCommon;

namespace MID.MRS.Common
{
	/// <summary>
	/// Used to save and retrieve information from the Windows clipboard.
	/// </summary>
	[Serializable()]
	public class ClipboardProfile : Profile
	{
		private static DataFormats.Format format;

		static ClipboardProfile()
		{
			// Registers a new data format with the windows Clipboard
			format = DataFormats.GetFormat(typeof(ClipboardProfile).FullName);
		}

		public static DataFormats.Format Format
		{
			get
			{
				return format;
			}
		}

		private string					_text;
		private eClipboardDataType		_clipboardDataType;
		private object					_clipboardData;
		private SecurityProfile			_securityProfile;
		private eDropAction				_action;
		private Image					_dragImage;
		private int						_dragImageHeight;
		private int						_dragImageWidth;
		private string					_dragImageText;
        // Begin Track #4872 - JSmith - Global/User Attributes
        private int _ownerUserRID;
        // End Track #4872

		public ClipboardProfile(int aKey)
		: base(aKey)
		{
            // Begin Track #4872 - JSmith - Global/User Attributes
            _ownerUserRID = Include.NoRID;
            // End Track #4872
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
			get { return _text ; }
			set { _text = value; }
		}

		/// <summary>
		/// Gets or sets the data type in the clipboard.
		/// </summary>
		public eClipboardDataType ClipboardDataType 
		{
			get { return _clipboardDataType ; }
			set { _clipboardDataType = value; }
		}

		/// <summary>
		/// Gets or sets the data associated with the type
		/// of data in the clipboard.
		/// </summary>
		public object ClipboardData 
		{
			get { return _clipboardData ; }
			set { _clipboardData = value; }
		}

		/// <summary>
		/// Gets or sets the security profile of data being put to the clipboard.
		/// </summary>
		public SecurityProfile SecurityProfile
		{
			get { return _securityProfile ; }
			set { _securityProfile = value; }
		}

		/// <summary>
		/// Gets or sets the action being performed when the data is being put to the clipboard.
		/// </summary>
		public eDropAction Action
		{
			get { return _action; }
			set { _action = value; }
		}

        // Begin Track #4872 - JSmith - Global/User Attributes
        /// <summary>
        /// Gets or sets the key of the owner of the Method or Workflow.
        /// </summary>
        public int OwnerUserRID
        {
            get { return _ownerUserRID; }
            set { _ownerUserRID = value; }
        }
        // End Track #4872

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

		/// <summary>
		/// Gets or sets the image height to use during a drag operation.
		/// </summary>
		public string DragImageText
		{
			get { return _dragImageText; }
			set { _dragImageText = value; }
		}
	}
}
