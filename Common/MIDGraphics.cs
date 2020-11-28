using System;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.Globalization;

using MID.MRS.DataCommon;

namespace MID.MRS.Common
{
	/// <summary>
	/// Summary description for MIDGraphics.
	/// </summary>
	public class MIDGraphics
	{
		private static ImageList _imageList;
		private static string _imageDir;
		private static ArrayList _imageFileName;
		private static DirectoryInfo _directoryInfo;
		private static Image _shortcutLargeOverlay;
		private static Image _shortcutSmallOverlay;
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private static Image _sharedLargeOverlay;
		private static Image _sharedSmallOverlay;
		//End Track #4815

		public const string GraphicsDir = "\\..\\Graphics"; 

		public const string ShortcutLarge = "ShortcutLarge.bmp";
		public const string ShortcutSmall = "ShortcutSmall.bmp";
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		public const string SharedLarge = "SharedLarge.bmp";
		public const string SharedSmall = "SharedSmall.bmp";
		//End Track #4815
		public const string ApplicationIcon = "MIDAllocation.ico";
		public const string ApplicationImage = "AdvancedAllocation.bmp";
		public const string AssortmentImage = "Assortment.gif";
		public const string CalendarImage = "Calendar.gif";
		public const string ClosedFolder = ".closedfolder.gif";
		public const string CopyImage = "Copy.GIF";
		public const string CopyIcon = "Copy.ico";
		public const string CollapseImage = "Collapse.GIF";
		public const string CutImage = "Cut.GIF";
		public const string DeleteImage = "Delete.gif";
        public const string DeleteIcon = "Delete.ico";
		public const string ExpandImage = "Expand.GIF";
		public const string FindImage = "FIND.GIF";
		public const string UndoImage = "Undo.GIF";
		//Begin Track #5006 - JScott - Display Low-levels one at a time
		public const string ThemeImage = "Palette.ico";
		public const string NavigateImage = "compass.ico";
		public const string FirstImage = "first.ico";
		public const string PreviousImage = "left.ico";
		public const string NextImage = "right.ico";
		public const string LastImage = "last.ico";
		//End Track #5006 - JScott - Display Low-levels one at a time
		public const string ReplaceImage = "Replace.GIF";
		public const string DefaultClosedFolder = "default.closedfolder.gif";
		public const string DefaultOpenFolder = "default.openfolder.gif";
		public const string DownArrow = "DownArrow.gif";
        public const string ErrorImage = "Error16.ico";
        public const string WarningImage = "Warning16.ico";
		public const string Folder = "*.*folder.gif";
		public const string ForecastImage = "Forecast.gif";
		public const string GlobalImage = "globe.ico";
		public const string HierarchyImage = "Hierarchy.gif";
		public const string HierarchyExplorerImage = "HierarchyExplorer.gif";
		public const string InheritanceImage = "Inheritance.ico";
		public const string InsertImage = "Insert.bmp";
		public const string InsertBeforeImage = "InsertBefore.bmp";
		public const string InsertAfterImage = "InsertAfter.bmp";
		public const string MagnifyingGlassImage = "MagnifyingGlass.gif";
		public const string MethodsImage = "Methods.GIF";
		public const string NewImage = "New.GIF";
		public const string OpenImage = "Open.GIF";
		public const string PasteImage = "Paste.GIF";
		public const string OpenFolder = ".openfolder.gif";
		public const string ReoccurringImage = "dynamic.gif";
		public const string DynamicToPlanImage = "DynamicToPlan.gif";
		public const string DynamicToCurrentImage = "DynamicToCurrent.gif";
		public const string SaveImage = "Save.GIF";
        public const string SaveIcon = "Save.ico";
		public const string SchedulerImage = "Scheduler.GIF";
		public const string SecurityImage = "Security.gif";
		public const string SizeImage = "Size.gif";
		public const string StoreImage = "store.gif";
		public const string StoreProfileImage = "StoreProfile.GIF";
		public const string TaskListImage = "TaskList.GIF";
		public const string UpArrow = "UpArrow.gif";
		public const string RightSelectArrow = "RightSelectArrow.GIF";
		public const string WorkspaceImage = "Workspace.GIF";
		public const string StyleViewImage = "StyleView.gif";
		public const string FilterExplorerImage = "FilterExplorer.gif";
		public const string ClosedTreeFolder = "ClosedTreeFolder.gif";
		public const string OpenTreeFolder = "OpenTreeFolder.gif";
		public const string ClosedTreeShortcutFolder = "ClosedTreeFolder.gif.shortcut";
		public const string OpenTreeShortcutFolder = "OpenTreeFolder.gif.shortcut";
		public const string FilterSelected = "Filter_selected.ico";
		public const string FilterUnselected = "Filter_unselected.ico";
		public const string FilterShortcutSelected = "Filter_selected.ico.shortcut";
		public const string FilterShortcutUnselected = "Filter_unselected.ico.shortcut";
		public const string GlobalShortcutImage = "globe.ico.shortcut";
		public const string BalanceImage = "balance.ico";
		public const string LockImage = "lock.gif";
		public const string DynamicSwitchImage = "DynamicSwitch.gif";
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		public const string TaskListSelected = "TaskList_selected.gif";
		public const string TaskListUnselected = "TaskList_unselected.gif";
		public const string JobSelected = "Job_selected.gif";
		public const string JobUnselected = "Job_unselected.gif";
		//End Track #4815
        public const string NotesImage = "Notes.ico";

		public const string StoreDynamic = "store_dynamic.gif";
		public const string StoreStatic = "store_static.gif";
		public const string StoreSelected = "store_selected.gif";
		public const string ClosedFolderDynamic = "yellow.closedfolderdynamic.GIF";
		public const string OpenedFolderDynamic = "yellow.openfolderdynamic.GIF";
				
		public const string SecUserImage = "User.gif";
		public const string SecGroupImage = "Group.bmp";
		public const string SecClosedFolderImage = "CLSDFOLD.ICO";
		public const string SecOpenFolderImage = "OPENFOLD.ICO";
		public const string SecNoAccessImage = "TRFFC10C.ico";
		public const string SecReadOnlyAccessImage = "TRFFC10B.ico";
		public const string SecFullAccessImage = "TRFFC10A.ico";

        public const string Blank = "Blank";

		public static string ImageDir
		{
			get{ return _imageDir; }
		}

		public static ImageList ImageList
		{
			get{ return _imageList; }
		}
		
		static MIDGraphics()
		{
			FileInfo[] shortcutFiles;

			_imageDir = MIDConfigurationManager.AppSettings["ApplicationRoot"] + GraphicsDir;
			_imageList = new ImageList();
			_imageList.ImageSize = new System.Drawing.Size(16, 16);
//			_imageList.TransparentColor = System.Drawing.Color.White;
			_imageFileName = new ArrayList();
			_directoryInfo = new DirectoryInfo(_imageDir);

			shortcutFiles = _directoryInfo.GetFiles(ShortcutLarge);
			foreach (FileInfo file in shortcutFiles)
			{
				string fileName = file.DirectoryName + @"\" + file.Name;
				_shortcutLargeOverlay = Image.FromFile(fileName);
			}

			shortcutFiles = _directoryInfo.GetFiles(ShortcutSmall);
			foreach (FileInfo file in shortcutFiles)
			{
				string fileName = file.DirectoryName + @"\" + file.Name;
				_shortcutSmallOverlay = Image.FromFile(fileName);
			}

			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			shortcutFiles = _directoryInfo.GetFiles(SharedLarge);
			foreach (FileInfo file in shortcutFiles)
			{
				string fileName = file.DirectoryName + @"\" + file.Name;
				_sharedLargeOverlay = Image.FromFile(fileName);
			}

			shortcutFiles = _directoryInfo.GetFiles(SharedSmall);
			foreach (FileInfo file in shortcutFiles)
			{
				string fileName = file.DirectoryName + @"\" + file.Name;
				_sharedSmallOverlay = Image.FromFile(fileName);
			}
			//End Track #4815

			// can't add all images at once because this runs out of memory.
			// add folders
			LoadImages (Folder, true, true);
			
			LoadImages (ApplicationImage, false, false);
			LoadImages (SecUserImage, false, true);
			LoadImages (GlobalImage, true, false);
			LoadImages (SecClosedFolderImage, false, false);
			LoadImages (SecOpenFolderImage, false, false);
			LoadImages (AssortmentImage, false, false);
			LoadImages (StyleViewImage, false, false);
			LoadImages (ForecastImage, false, false);
			LoadImages (CalendarImage, false, false);
			LoadImages (SecurityImage, false, false);
			LoadImages (HierarchyImage, false, false);
			LoadImages (HierarchyExplorerImage, false, false);
			LoadImages (StoreImage, false, false);
			LoadImages (StoreProfileImage, false, false);
			LoadImages (TaskListImage, false, false);
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			LoadImages (TaskListSelected, false, true);
			LoadImages (TaskListUnselected, false, true);
			LoadImages (JobSelected, false, false);
			LoadImages (JobUnselected, false, false);
			//End Track #4815
			LoadImages (WorkspaceImage, false, false);
			LoadImages (MethodsImage, false, false);
			LoadImages (CutImage, false, false);
			LoadImages (CopyImage, false, false);
			LoadImages (CopyIcon, false, false);
			LoadImages (PasteImage, false, false);
			LoadImages (DeleteImage, false, false);
            LoadImages (DeleteIcon, false, false);
			LoadImages (FindImage, false, false);
			LoadImages (UndoImage, false, false);
			//Begin Track #5006 - JScott - Display Low-levels one at a time
			LoadImages (ThemeImage, false, false);
			LoadImages (NavigateImage, false, false);
			LoadImages (FirstImage, false, false);
			LoadImages (PreviousImage, false, false);
			LoadImages (NextImage, false, false);
			LoadImages (LastImage, false, false);
			//End Track #5006 - JScott - Display Low-levels one at a time
			LoadImages (OpenImage, false, false);
			LoadImages (SaveImage, false, false);
			LoadImages (ReplaceImage, false, false);
			LoadImages (SchedulerImage, false, false);
			LoadImages (DynamicToPlanImage, false, false);
			LoadImages (DynamicToCurrentImage, false, false);
			LoadImages (SizeImage, false, false);
			LoadImages (FilterExplorerImage, false, false);
			LoadImages (ClosedTreeFolder, true, true);
			LoadImages (OpenTreeFolder, true, true);
			LoadImages (FilterSelected, true, true);
			LoadImages (FilterUnselected, true, true);
			LoadImages (SecGroupImage, false, false);
			LoadImages (SecNoAccessImage, false, false);
			LoadImages (SecReadOnlyAccessImage, false, false);
			LoadImages (SecFullAccessImage, false, false);
			LoadImages (BalanceImage, false, false);
			LoadImages (LockImage, false, false);
			LoadImages (MagnifyingGlassImage, false, false);
			LoadImages (DynamicSwitchImage, false, false);
            LoadImages(NotesImage, false, false);
            LoadImages(CollapseImage, false, false);
            LoadImages(ExpandImage, false, false);
            LoadImages(RightSelectArrow, false, false);
            LoadImages(StoreDynamic, false, false);
            LoadImages(StoreStatic, false, false);
            LoadImages(StoreSelected, false, false);
            LoadImages(ClosedFolderDynamic, false, false);
            LoadImages(OpenedFolderDynamic, false, false);
            LoadBlankImage();
		}

		private static void LoadImages (string aFileName, bool aCreateShortcutImage, bool aCreateSharedImage)
		{
			string fileName;
			Bitmap shortcutBmp;
			Graphics graph;
			Image shortcutOverlay;

			try
			{
				FileInfo[] bmpfiles = _directoryInfo.GetFiles(aFileName);
				foreach( FileInfo f in bmpfiles)
				{
					fileName = f.DirectoryName + @"\" + f.Name;
					Image myImage = Image.FromFile(fileName);
					_imageList.Images.Add(myImage);
					_imageFileName.Add(f.Name.ToLower(CultureInfo.CurrentUICulture));

					if (aCreateShortcutImage)
					{
						shortcutBmp = new Bitmap(myImage.Width, myImage.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
						shortcutBmp.MakeTransparent(shortcutBmp.GetPixel(0, 0));
						graph = Graphics.FromImage(shortcutBmp);

						graph.DrawImage(
							myImage,                               
							new Rectangle(0, 0, myImage.Width, myImage.Height), 
							0,                                      
							0,                                       
							myImage.Width,                                
							myImage.Height,                               
							GraphicsUnit.Pixel);

						if (myImage.Width > 20 && myImage.Height > 20)
						{
							shortcutOverlay = _shortcutLargeOverlay;
						}
						else
						{
							shortcutOverlay = _shortcutSmallOverlay;
						}

						graph.DrawImage(
							shortcutOverlay,
							new Rectangle(0, myImage.Height - shortcutOverlay.Height, shortcutOverlay.Width, shortcutOverlay.Height),
							0,
							0,
							shortcutOverlay.Width,
							shortcutOverlay.Height,
							GraphicsUnit.Pixel);

						_imageList.Images.Add(shortcutBmp);
//						_imageFileName.Add(aShortcutName.ToLower(CultureInfo.CurrentUICulture));
						_imageFileName.Add(f.Name.ToLower(CultureInfo.CurrentUICulture) + ".shortcut");
					}
					//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
					if (aCreateSharedImage)
					{
						shortcutBmp = new Bitmap(myImage.Width, myImage.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
						shortcutBmp.MakeTransparent(shortcutBmp.GetPixel(0, 0));
						graph = Graphics.FromImage(shortcutBmp);

						graph.DrawImage(
							myImage,                               
							new Rectangle(0, 0, myImage.Width, myImage.Height), 
							0,                                      
							0,                                       
							myImage.Width,                                
							myImage.Height,                               
							GraphicsUnit.Pixel);

						if (myImage.Width > 20 && myImage.Height > 20)
						{
							shortcutOverlay = _sharedLargeOverlay;
						}
						else
						{
							shortcutOverlay = _sharedSmallOverlay;
						}

						graph.DrawImage(
							shortcutOverlay,
							new Rectangle(0, myImage.Height - shortcutOverlay.Height, shortcutOverlay.Width, shortcutOverlay.Height),
							0,
							0,
							shortcutOverlay.Width,
							shortcutOverlay.Height,
							GraphicsUnit.Pixel);

						_imageList.Images.Add(shortcutBmp);
						_imageFileName.Add(f.Name.ToLower(CultureInfo.CurrentUICulture) + Include.SharedExtension);
					}
					//End Track #4815
				}
			}
			catch
			{
			}
		}

        private static void LoadBlankImage()
        {
            Bitmap bitmap;

            try
            {
                bitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
                _imageList.Images.Add(bitmap);
                _imageFileName.Add(Blank.ToLower(CultureInfo.CurrentUICulture));
            }
            catch
            {
            }
        }

		public static string ImageFileName (int i)
		{
			return _imageFileName[i].ToString(); 
		}

		public static int ImageIndex (string imageFileName)
		{
			imageFileName = imageFileName.ToLower(CultureInfo.CurrentUICulture);
			for (int i = 0; i <_imageFileName.Count; i++)
			{
				if (imageFileName == _imageFileName[i].ToString())
				{
					return i;
				}
			}
			return -1;
		}

		public static int ImageIndexWithShortcut (string imageFileName, string extension)
		{
			return ImageIndexWithDefault (imageFileName, extension + ".shortcut");
		}

// Begin Track #3490 - Jeff Scott - Invalid folder color causing errors
		public static int ImageIndexWithDefault (string imageFileName, string extension)
		{
			int index;

			index = ImageIndex(imageFileName + extension);
			if (index == -1)
			{
				index = ImageIndex(Include.MIDDefaultColor + extension);
			}
			return index;
		}

// Begin Track #3490 - Jeff Scott - Invalid folder color causing errors
		public static Image GetImage (string imageFileName)
		{
			int imageIndex = ImageIndex (imageFileName);
			if (imageIndex >= 0)
			{
				return _imageList.Images[imageIndex];
			}
			return null;
		}

		public static Icon GetIcon (string imageFileName)
		{
			Icon icon = new Icon(_imageDir + @"\" + imageFileName);
			
			return icon;
		}

		public static void BuildDragImage(string aText, ImageList aImageList, int aIndent,
			int aSpacing, Font aFont, Color aForeColor,  
			out int aImageHeight, out int aImageWidth)
		{
			try
			{
				aImageHeight = 0;
				aImageWidth = 0;
				if (aText == null)
				{
					return;
				}

				// measure the text
				Bitmap b = new Bitmap(1, 1);
				Graphics g = System.Drawing.Graphics.FromImage(b);

				aImageHeight = (int)(g.MeasureString(aText, aFont).Height);
				// 256 is max height
				if (aImageHeight > 256)
				{
					aImageHeight = 256;
				}
				aImageWidth = (int)(g.MeasureString(aText, aFont).Width);
				// 256 is max width
				if (aImageWidth > 256)
				{
					aImageWidth = 256;
				}

				// Reset image list used for drag image
				aImageList.Images.Clear();
				aImageList.ImageSize = new Size(aImageWidth, aImageHeight);

				// Create new bitmap
				// This bitmap will contain the image to be dragged
				Bitmap bmp = new Bitmap(aImageWidth, aImageHeight);

				// Get graphics from bitmap
				Graphics gfx = Graphics.FromImage(bmp);
				int x = 0, y = 0;

				AddItem(gfx, aText, x, y, aIndent, aFont, aForeColor);

				// Add bitmap to imagelist
				aImageList.Images.Add(bmp);
			}
			catch
			{
				throw;
			}
		}

		public static void BuildDragImage(ClipboardProfile aClipboardProfile, ImageList aImageList, int aIndent,
			int aSpacing, Font aFont, Color aForeColor,
			out int aImageHeight, out int aImageWidth)
		{
			try
			{
				aImageHeight = 0;
				aImageWidth = 0;
				if (aClipboardProfile == null)
				{
					return;
				}

				aImageHeight = GetImageHeight(aClipboardProfile, aSpacing);
				// 256 is max height
				if (aImageHeight > 256)
				{
					aImageHeight = 256;
				}
				aImageWidth = GetImageWidth(aClipboardProfile) + aIndent;
				// 256 is max width
				if (aImageWidth > 256)
				{
					aImageWidth = 256;
				}

				// Reset image list used for drag image
				aImageList.Images.Clear();
				aImageList.ImageSize = new Size(aImageWidth, aImageHeight);

				// Create new bitmap
				// This bitmap will contain the image to be dragged
				Bitmap bmp = new Bitmap(aImageWidth, aImageHeight);

				// Get graphics from bitmap
				Graphics gfx = Graphics.FromImage(bmp);
				int x = 0, y = 0;

				if (aClipboardProfile.ClipboardData.GetType() == typeof(ArrayList))
				{
					ArrayList items = (ArrayList)aClipboardProfile.ClipboardData;
					foreach (ClipboardProfile item in items)
					{
						AddItem(gfx, item, x, y, aIndent, aFont, aForeColor);
						y += item.DragImageHeight + aSpacing;
					}
				}
				else
				{
					AddItem(gfx, aClipboardProfile, x, y, aIndent, aFont, aForeColor);
				}

				// Add bitmap to imagelist
				aImageList.Images.Add(bmp);
			}
			catch
			{
				throw;
			}
		}

		private static int GetImageHeight(ClipboardProfile aClipboardProfile, int aSpacing)
		{
			int height = 0;
			if (aClipboardProfile.ClipboardData.GetType() == typeof(ArrayList))
			{
				int count = 0;
				ArrayList items = (ArrayList)aClipboardProfile.ClipboardData;
				foreach (ClipboardProfile item in items)
				{
					height += item.DragImageHeight;
					if (count > 0)
					{
						height += aSpacing;
					}
					++count;
				}
			}
			else
			{
				height = aClipboardProfile.DragImageHeight;
			}
			return height;
		}

		private static int GetImageWidth(ClipboardProfile aClipboardProfile)
		{
			int width = 0;
			if (aClipboardProfile.ClipboardData.GetType() == typeof(ArrayList))
			{
				ArrayList items = (ArrayList)aClipboardProfile.ClipboardData;
				foreach (ClipboardProfile item in items)
				{
					if (item.DragImageWidth > width)
					{
						width = item.DragImageWidth;
					}
				}
			}
			else
			{
				width = aClipboardProfile.DragImageWidth;
			}
			return width;
		}

		private static void AddItem(Graphics aGraphics, ClipboardProfile aClipboardProfile, int xPos, int yPos,
			int aIndent, Font aFont, Color aForeColor)
		{
			// Draw node icon into the bitmap
			aGraphics.DrawImage(aClipboardProfile.DragImage, xPos, yPos);

			// Draw node label into bitmap
			aGraphics.DrawString(aClipboardProfile.Text,
				aFont,
				new SolidBrush(aForeColor),
				(float)aIndent, yPos + 1.0f);
		}

		private static void AddItem(Graphics aGraphics, string aText, int xPos, int yPos,
			int aIndent, Font aFont, Color aForeColor)
		{
			// Draw node label into bitmap
			aGraphics.DrawString(aText,
				aFont,
				new SolidBrush(aForeColor),
				(float)aIndent, yPos + 1.0f);
		}
	}
}
