//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
// Significantly restructured -- compare to previous version.
//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
using System;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Windows.Controls
{
	public class MIDGraphics
	{
		private static ImageList _imageList;
		private static string _imageDir;
		private static ArrayList _imageInfo;
		private static DirectoryInfo _directoryInfo;
		private static Image _shortcutLargeOverlay;
		private static Image _shortcutSmallOverlay;
		private static Image _sharedLargeOverlay;
		private static Image _sharedSmallOverlay;

		private const string ShortcutLarge = "ShortcutLarge.bmp";
		private const string ShortcutSmall = "ShortcutSmall.bmp";
		private const string SharedLarge = "SharedLarge.bmp";
		private const string SharedSmall = "SharedSmall.bmp";

        // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
        //public const string GraphicsDir = "\\..\\Graphics";
        private const string GraphicsDir = "\\..\\Graphics";
        // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration

		public const string Blank = "Blank";
		public const string AllocationSummaryImage = "AllocationSummary.gif";	// TT#707 - separate summary image from assortment
		public const string ApplicationIcon = "MIDRetail.ico";
		public const string ApplicationImage = "MIDRetail.bmp";
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
		public const string ThemeImage = "Palette.ico";
		public const string NavigateImage = "compass.ico";
		public const string FirstImage = "first.ico";
		public const string PreviousImage = "left.ico";
		public const string NextImage = "right.ico";
		public const string LastImage = "last.ico";
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
		public const string FilterSelected = "Filter_selected.ico";
		public const string FilterUnselected = "Filter_unselected.ico";
		public const string BalanceImage = "balance.ico";
		public const string LockImage = "lock.gif";
		public const string DynamicSwitchImage = "DynamicSwitch.gif";
		public const string TaskListSelected = "TaskList_selected.gif";
		public const string TaskListUnselected = "TaskList_unselected.gif";
		public const string JobSelected = "Job_selected.gif";
		public const string JobUnselected = "Job_unselected.gif";
		public const string SpecialRequestSelected = "SpecReq_selected.gif";
		public const string SpecialRequestUnselected = "SpecReq_unselected.gif";
		public const string NotesImage = "Notes.ico";
		public const string FavoritesImage = "Favorites.ico";
		public const string AddToFavoritesImage = "AddToFavorites.ico";
		public const string UserImage = "User.gif";
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
        public const string ExcelImage = "Excel.ico";   // TT#1135 - AGallagher - Export headers from allocation workspace
        public const string Windows7CertImage = "windows7_certified.png";   // TT#1183 - JSmith - Windows 7 Logo
        public const string Logility = "Logility2.bmp";   // TT#???? - AGallagher - Logility - Rebranding #1
        // Begin TT#46 MD - JSmith - User Dashboard 
        //BEGIN TT#46-MD -jsobek -Develop My Activity Log
        public const string msgOKImage = "msg_ok.png";
        public const string msgWarningImage = "msg_warning.png";
        public const string msgEditImage = "msg_edit.png";
        public const string msgErrorImage = "msg_error.png";
        public const string msgSevereImage = "msg_severe.png";
        public const string ChartImage = "chart_pie.png";
        public const string UserActivityImage = "app_view_list.png";
        //END TT#46-MD -jsobek -Develop My Activity Log
        public const string msgDebugImage = "msg_debug.png"; //TT#595-MD -jsobek -Add Debug to My Activity level 
        // End TT#46 MD
        public const string targetImage = "target.png"; //TT#696-MD -Add "Active Process" selection to application to specify where methods should look for selected headers -jsobek
		// BEGIN TT#488-MD - Stodd - Group Allocation
		public const string ToolBarApplyImage = "ToolBarApply.png";
		public const string ToolBarEmailImage = "ToolBarEmail.png";
		public const string ToolBarExportImage = "ToolBarExport.png";
		public const string ToolBarGridImage = "ToolBarGrid.png";
		public const string ToolBarGroupByImage = "ToolBarGroupBy.png";
		public const string ToolBarProcessImage = "ToolBarProcess.png";
		public const string ToolBarSaveViewImage = "ToolBarSaveView.png";
		public const string ToolBarExpandImage = "ToolBarExpand.png";
		public const string ToolBarCollapseImage = "ToolBarCollapse.png";
		// END TT#488-MD - Stodd - Group Allocation

		public static string ImageDir
		{
			get
			{
				return _imageDir;
			}
		}

		public static ImageList ImageList
		{
			get
			{
				return _imageList;
			}
		}

        // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
        public static string MIDGraphicsDir
        {
            get
            {
                string applicationRoot;
#if (DEBUG)
                applicationRoot = MIDConfigurationManager.AppSettings[Include.MIDApplicationRoot];
                if (string.IsNullOrEmpty(applicationRoot))
                {
                    applicationRoot = @"..\..";
                }
#else
                applicationRoot = Path.GetDirectoryName(Application.ExecutablePath);
#endif
                return applicationRoot + GraphicsDir;
            }
        }
        // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration

		static MIDGraphics()
		{
			FileInfo[] shortcutFiles;

			try
			{
                // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
                //_imageDir = MIDConfigurationManager.AppSettings["ApplicationRoot"] + GraphicsDir;
                _imageDir = MIDGraphicsDir;
                // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration
				_imageList = new ImageList();
				_imageList.ImageSize = new System.Drawing.Size(16, 16);
				_imageInfo = new ArrayList();

				_directoryInfo = new DirectoryInfo(_imageDir);

				shortcutFiles = _directoryInfo.GetFiles(ShortcutLarge);

				foreach (FileInfo file in shortcutFiles)
				{
					_shortcutLargeOverlay = Image.FromFile(file.DirectoryName + @"\" + file.Name);
				}

				shortcutFiles = _directoryInfo.GetFiles(ShortcutSmall);

				foreach (FileInfo file in shortcutFiles)
				{
					_shortcutSmallOverlay = Image.FromFile(file.DirectoryName + @"\" + file.Name);
				}

				shortcutFiles = _directoryInfo.GetFiles(SharedLarge);

				foreach (FileInfo file in shortcutFiles)
				{
					_sharedLargeOverlay = Image.FromFile(file.DirectoryName + @"\" + file.Name);
				}

				shortcutFiles = _directoryInfo.GetFiles(SharedSmall);
				foreach (FileInfo file in shortcutFiles)
				{
					_sharedSmallOverlay = Image.FromFile(file.DirectoryName + @"\" + file.Name);
				}

				LoadBlankImage();
				LoadImages(Folder);
				LoadImages(AllocationSummaryImage);	// TT#707 - separate summary image from assortment
				LoadImages(ApplicationImage);
				LoadImages(SecUserImage);
				LoadImages(GlobalImage);
				LoadImages(SecClosedFolderImage);
				LoadImages(SecOpenFolderImage);
				LoadImages(AssortmentImage);
				LoadImages(StyleViewImage);
				LoadImages(ForecastImage);
				LoadImages(CalendarImage);
				LoadImages(SecurityImage);
				LoadImages(HierarchyImage);
				LoadImages(HierarchyExplorerImage);
				LoadImages(StoreImage);
				LoadImages(StoreProfileImage);
				LoadImages(TaskListImage);
				LoadImages(TaskListSelected);
				LoadImages(TaskListUnselected);
				LoadImages(JobSelected);
				LoadImages(JobUnselected);
				LoadImages(SpecialRequestSelected);
				LoadImages(SpecialRequestUnselected);
				LoadImages(WorkspaceImage);
				LoadImages(MethodsImage);
				LoadImages(CutImage);
				LoadImages(CopyImage);
				LoadImages(CopyIcon);
				LoadImages(PasteImage);
				LoadImages(DeleteImage);
				LoadImages(DeleteIcon);
				LoadImages(FindImage);
				LoadImages(UndoImage);
				LoadImages(ThemeImage);
				LoadImages(NavigateImage);
				LoadImages(FirstImage);
				LoadImages(PreviousImage);
				LoadImages(NextImage);
				LoadImages(LastImage);
				LoadImages(OpenImage);
				LoadImages(SaveImage);
				LoadImages(ReplaceImage);
				LoadImages(SchedulerImage);
				LoadImages(DynamicToPlanImage);
				LoadImages(DynamicToCurrentImage);
				LoadImages(SizeImage);
				LoadImages(FilterExplorerImage);
				LoadImages(ClosedTreeFolder);
				LoadImages(OpenTreeFolder);
				LoadImages(FilterSelected);
				LoadImages(FilterUnselected);
				LoadImages(SecGroupImage);
				LoadImages(SecNoAccessImage);
				LoadImages(SecReadOnlyAccessImage);
				LoadImages(SecFullAccessImage);
				LoadImages(BalanceImage);
				LoadImages(LockImage);
				LoadImages(MagnifyingGlassImage);
				LoadImages(DynamicSwitchImage);
				LoadImages(NotesImage);
				LoadImages(CollapseImage);
				LoadImages(ExpandImage);
				LoadImages(RightSelectArrow);
				LoadImages(StoreDynamic);
				LoadImages(StoreStatic);
				LoadImages(StoreSelected);
				LoadImages(ClosedFolderDynamic);
				LoadImages(OpenedFolderDynamic);
				LoadImages(FavoritesImage);
				LoadImages(AddToFavoritesImage);
                LoadImages(ExcelImage); // TT#1135 - AGallagher - Export headers from allocation workspace
                LoadImages(Windows7CertImage);   // TT#1183 - JSmith - Windows 7 Logo
                LoadImages(Logility); //TT#???? - AGallagher - Rebranding #1
                // Begin TT#46 MD - JSmith - User Dashboard 
                //BEGIN TT#46-MD -jsobek -Develop My Activity Log
                LoadImages(msgOKImage);
                LoadImages(msgWarningImage);
                LoadImages(msgEditImage);
                LoadImages(msgErrorImage);
                LoadImages(msgSevereImage);
                LoadImages(ChartImage);
                LoadImages(UserActivityImage);
                //END TT#46-MD -jsobek -Develop My Activity Log
                LoadImages(msgDebugImage); //TT#595-MD -jsobek -Add Debug to My Activity level 
                // End TT#46 MD
                LoadImages(targetImage);//TT#696-MD -Add "Active Process" selection to application to specify where methods should look for selected headers -jsobek
				// BEGIN TT#488-MD - Stodd - Group Allocation
				LoadImages(ToolBarApplyImage);
				LoadImages(ToolBarEmailImage);
				LoadImages(ToolBarExportImage);
				LoadImages(ToolBarGridImage);
				LoadImages(ToolBarGroupByImage);
				LoadImages(ToolBarProcessImage);
				LoadImages(ToolBarSaveViewImage);
				LoadImages(ToolBarExpandImage);
				LoadImages(ToolBarCollapseImage);
				// END TT#488-MD - Stodd - Group Allocation
			}
			catch
			{
				throw;
			}
		}

		private static void LoadImages(string aFileName)
		{
			FileInfo[] bmpfiles;
			Image myImage;

			try
			{
				bmpfiles = _directoryInfo.GetFiles(aFileName);

				foreach (FileInfo file in bmpfiles)
				{
					myImage = Image.FromFile(file.DirectoryName + @"\" + file.Name);
					_imageList.Images.Add(myImage);
					_imageInfo.Add(new ImageInfo(file.Name.ToLower(CultureInfo.CurrentUICulture), _imageList.Images.Count - 1));
				}
			}
			catch
			{
				throw;
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
				_imageInfo.Add(new ImageInfo(Blank.ToLower(CultureInfo.CurrentUICulture), _imageList.Images.Count - 1));
			}
			catch
			{
				throw;
			}
		}

		public static string ImageFileName(int i)
		{
			try
			{
				return ((ImageInfo)_imageInfo[i]).Name;
			}
			catch
			{
				throw;
			}
		}

		public static int ImageIndex(string imageFileName)
		{
			try
			{
				return ImageIndex(imageFileName, false, false);
			}
			catch
			{
				throw;
			}
		}

		public static int ImageShortcutIndex(string imageFileName)
		{
			try
			{
				return ImageIndex(imageFileName, true, false);
			}
			catch
			{
				throw;
			}
		}

		public static int ImageSharedIndex(string imageFileName)
		{
			try
			{
				return ImageIndex(imageFileName, false, true);
			}
			catch
			{
				throw;
			}
		}

		public static int ImageIndexWithDefault(string imageFileName, string extension)
		{
			try
			{
				return ImageIndexWithDefault(imageFileName, extension, false, false);
			}
			catch
			{
				throw;
			}
		}

		public static int ImageShortcutIndexWithDefault(string imageFileName, string extension)
		{
			try
			{
				return ImageIndexWithDefault(imageFileName, extension, true, false);
			}
			catch
			{
				throw;
			}
		}

		public static int ImageSharedIndexWithDefault(string imageFileName, string extension)
		{
			try
			{
				return ImageIndexWithDefault(imageFileName, extension, false, true);
			}
			catch
			{
				throw;
			}
		}

		public static Image GetImage(string imageFileName)
		{
			int imageIdx;

			try
			{
				imageIdx = ImageIndex(imageFileName);

				if (imageIdx >= 0)
				{
					return _imageList.Images[imageIdx];
				}

				return null;
			}
			catch
			{
				throw;
			}
		}

		public static Icon GetIcon(string imageFileName)
		{
			try
			{
				return new Icon(_imageDir + @"\" + imageFileName);
			}
			catch
			{
				throw;
			}
		}

		public static void BuildDragImage(
			string aText,
			ImageList aImageList,
			int aIndent,
			int aSpacing,
			Font aFont,
			Color aForeColor,
			out int aImageHeight,
			out int aImageWidth)
		{
			Bitmap bmp;
			Graphics gfx;

			try
			{
				aImageHeight = 0;
				aImageWidth = 0;

				if (aText != null)
				{
					// measure the text
					bmp = new Bitmap(1, 1);
					gfx = System.Drawing.Graphics.FromImage(bmp);
					aImageHeight = Math.Min((int)(gfx.MeasureString(aText, aFont).Height), 256);
					aImageWidth = Math.Min((int)(gfx.MeasureString(aText, aFont).Width), 256);

					// Reset image list used for drag image
					aImageList.Images.Clear();
					aImageList.ImageSize = new Size(aImageWidth, aImageHeight);

					// Create new bitmap and add to image list
					bmp = new Bitmap(aImageWidth, aImageHeight);
					gfx = Graphics.FromImage(bmp);
					gfx.DrawString(aText, aFont, new SolidBrush(aForeColor), (float)aIndent, 0 + 1.0f);
					aImageList.Images.Add(bmp);
				}
			}
			catch
			{
				throw;
			}
		}

        public static void BuildDragImage(ClipboardListBase aClipboardList, ImageList aImageList, int aIndent,
			int aSpacing, Font aFont, Color aForeColor,
			out int aImageHeight, out int aImageWidth)
		{
			Bitmap bmp;
			Graphics gfx;
			int y;

            try
            {
                aImageHeight = 0;
                aImageWidth = 0;

				if (aClipboardList != null)
				{
					// measure the text
					aImageHeight = Math.Min(GetImageHeight(aClipboardList, aSpacing), 256);
					aImageWidth = Math.Min(GetImageWidth(aClipboardList) + aIndent, 256);

					// Reset image list used for drag image
					aImageList.Images.Clear();
					aImageList.ImageSize = new Size(aImageWidth, aImageHeight);

					// Create new bitmap and add to image list
					bmp = new Bitmap(aImageWidth, aImageHeight);
					gfx = Graphics.FromImage(bmp);

					y = 0;

					foreach (ClipboardProfileBase cb in aClipboardList.ClipboardItems)
					{
						gfx.DrawImage(cb.DragImage, 0, y);
						gfx.DrawString(cb.Text, aFont, new SolidBrush(aForeColor), (float)aIndent, y + 1.0f);
						y += cb.DragImageHeight + aSpacing;
					}

					aImageList.Images.Add(bmp);
				}
            }
            catch
            {
                throw;
            }
		}

		private static int ImageIndex(string imageFileName, bool shortcut, bool shared)
		{
			int i;

			try
			{
				imageFileName = imageFileName.ToLower(CultureInfo.CurrentUICulture);

				for (i = 0; i < _imageInfo.Count; i++)
				{
					if (imageFileName == ((ImageInfo)_imageInfo[i]).Name)
					{
						if (shortcut)
						{
							return ((ImageInfo)_imageInfo[i]).ShortcutIndex;
						}
						else if (shared)
						{
							return ((ImageInfo)_imageInfo[i]).SharedIndex;
						}
						else
						{
							return i;
						}
					}
				}

				return -1;
			}
			catch
			{
				throw;
			}
		}

		private static int ImageIndexWithDefault(string imageFileName, string extension, bool shortcut, bool shared)
		{
			int idx;

			try
			{
				idx = ImageIndex(imageFileName + extension, shortcut, shared);

				if (idx == -1)
				{
					idx = ImageIndex(Include.MIDDefaultColor + extension, shortcut, shared);
				}

				return idx;
			}
			catch
			{
				throw;
			}
		}

		private static int GetImageHeight(ClipboardListBase aClipboardList, int aSpacing)
		{
			int height;
            int count;

			try
			{
				height = 0;

				foreach (ClipboardProfileBase cb in aClipboardList.ClipboardItems)
				{
					count = 0;

					{
						height += cb.DragImageHeight;

						if (count > 0)
						{
							height += aSpacing;
						}

						++count;
					}
				}

				return height;
			}
			catch
			{
				throw;
			}
		}

		private static int GetImageWidth(ClipboardListBase aClipboardList)
		{
			int width;

			try
			{
				width = 0;

				foreach (ClipboardProfileBase cb in aClipboardList.ClipboardItems)
				{
					if (cb.DragImageWidth > width)
					{
						width = cb.DragImageWidth;
					}
				}

				return width;
			}
			catch
			{
				throw;
			}
		}

		private class ImageInfo
		{
			private string _name;
			private int _imageIdx;
			private int _shortcutIndex;
			private int _sharedIndex;

			public ImageInfo(string aName, int aImageIdx)
			{
				_name = aName;
				_imageIdx = aImageIdx;
				_shortcutIndex = -1;
				_sharedIndex = -1;
			}

			public string Name
			{
				get
				{
					return _name;
				}
			}

			public int ShortcutIndex
			{
				get
				{
					Image baseImage;
					Bitmap shortcutBmp;
					Graphics graph;
					Image shortcutOverlay;

					try
					{
						if (_shortcutIndex == -1)
						{
							baseImage = _imageList.Images[_imageIdx];

							shortcutBmp = new Bitmap(baseImage.Width, baseImage.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
							shortcutBmp.MakeTransparent(shortcutBmp.GetPixel(0, 0));
							graph = Graphics.FromImage(shortcutBmp);

							graph.DrawImage(
								baseImage,
								new Rectangle(0, 0, baseImage.Width, baseImage.Height),
								0,
								0,
								baseImage.Width,
								baseImage.Height,
								GraphicsUnit.Pixel);

							if (baseImage.Width > 20 && baseImage.Height > 20)
							{
								shortcutOverlay = _shortcutLargeOverlay;
							}
							else
							{
								shortcutOverlay = _shortcutSmallOverlay;
							}

							graph.DrawImage(
								shortcutOverlay,
								new Rectangle(0, baseImage.Height - shortcutOverlay.Height, shortcutOverlay.Width, shortcutOverlay.Height),
								0,
								0,
								shortcutOverlay.Width,
								shortcutOverlay.Height,
								GraphicsUnit.Pixel);

							_imageList.Images.Add(shortcutBmp);
							_shortcutIndex = _imageList.Images.Count - 1;
						}

						return _shortcutIndex;
					}
					catch
					{
						throw;
					}
				}
			}

			public int SharedIndex
			{
				get
				{
					int imageIndex;
					Image baseImage;
					Bitmap sharedBmp;
					Graphics graph;
					Image sharedOverlay;

					try
					{
						if (_sharedIndex == -1)
						{
							imageIndex = ImageIndex(_name);

							if (imageIndex >= 0)
							{
								baseImage = _imageList.Images[imageIndex];

								sharedBmp = new Bitmap(baseImage.Width, baseImage.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
								sharedBmp.MakeTransparent(sharedBmp.GetPixel(0, 0));
								graph = Graphics.FromImage(sharedBmp);

								graph.DrawImage(
									baseImage,
									new Rectangle(0, 0, baseImage.Width, baseImage.Height),
									0,
									0,
									baseImage.Width,
									baseImage.Height,
									GraphicsUnit.Pixel);

								if (baseImage.Width > 20 && baseImage.Height > 20)
								{
									sharedOverlay = _sharedLargeOverlay;
								}
								else
								{
									sharedOverlay = _sharedSmallOverlay;
								}

								graph.DrawImage(
									sharedOverlay,
									new Rectangle(0, baseImage.Height - sharedOverlay.Height, sharedOverlay.Width, sharedOverlay.Height),
									0,
									0,
									sharedOverlay.Width,
									sharedOverlay.Height,
									GraphicsUnit.Pixel);

								_imageList.Images.Add(sharedBmp);
								_sharedIndex = _imageList.Images.Count - 1;
							}
							else
							{
								throw new Exception("Invalid MIDGraphics Image requested");
							}
						}

						return _sharedIndex;
					}
					catch
					{
						throw;
					}
				}
			}
		}
	}
}
