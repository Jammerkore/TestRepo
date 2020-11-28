using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class frmAddToFavorites : MIDFormBase
    {
        FolderDataLayer _dlFolder;

        public frmAddToFavorites()
        {
            InitializeComponent();
        }

        public frmAddToFavorites(SessionAddressBlock aSAB)
            : base(aSAB)
        {
            InitializeComponent();
        }

        private void frmAddToFavorites_Load(object sender, EventArgs e)
        {
            SetText();
            Icon = MIDGraphics.GetIcon(MIDGraphics.AddToFavoritesImage);
            displayApplicationImage();
            cboCreateIn.ImageList = MIDGraphics.ImageList;
        }

        private void SetText()
        {
            btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
            btnAdd.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Add);
        }

        private void displayApplicationImage()
        {
            Image image;
            try
            {
                image = Image.FromFile(MIDGraphics.ImageDir + "\\" + MIDGraphics.FavoritesImage);
                SizeF sizef = new SizeF(pbFavoriteImage.Width, pbFavoriteImage.Height);
                Size size = Size.Ceiling(sizef);
                Bitmap bitmap = new Bitmap(image, size);
                pbFavoriteImage.Image = bitmap;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void Initialize(ArrayList aItems, int aCreateInFolder)
        {
            DataTable dtFolder;
            string folderName;
            try
            {
                _dlFolder = new FolderDataLayer();
                string items = null;
                foreach (favoriteItem fi in aItems)
                {
                    if (items != null)
                    {
                        items += "; ";
                    }
                    items += fi.Text;
                }
                txtName.Text = items;

                dtFolder = _dlFolder.Folder_Read(aCreateInFolder);
                if (dtFolder.Rows.Count == 1)
                {
                    folderName = Convert.ToString(dtFolder.Rows[0]["FOLDER_ID"], CultureInfo.CurrentUICulture);
                    cboCreateIn.Items.Add(new ComboBoxWithImagesItem(folderName, MIDGraphics.ImageIndex(MIDGraphics.FavoritesImage), 2));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cancel_Click();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        override protected bool SaveChanges()
        {
            return true;
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            AddToFavoritesCreateFolder form;
            ArrayList al;
            try
            {
                form = new AddToFavoritesCreateFolder(SAB);

                ////  Allow for a floating explorer.   ParentForm is not Client.Explorer if floating
                //if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
                //{
                //    form.MdiParent = this.ParentForm;
                //}
                //else
                //{
                //    form.MdiParent = this.ParentForm.Owner;
                //}

                //al = new ArrayList();
                //foreach (MIDHierarchyNode selectedNode in tvMerchandise.SelectedNodes)
                //{
                //    if (selectedNode.NodeType == eHierarchyNodeType.AlternateHierarchyFolder ||
                //        selectedNode.NodeType == eHierarchyNodeType.OrganizationalHierarchyFolder)
                //    {
                //        al.Add(new favoriteItem(selectedNode.NodeRID, selectedNode.Text, eFolderChildType.Hierarchy));
                //    }
                //    else
                //    {
                //        al.Add(new favoriteItem(selectedNode.NodeRID, selectedNode.Text, eFolderChildType.HierarchyNode));
                //    }
                //}
                //form.Initialize(al, _myFavoritesNode.NodeRID);
                form.ShowDialog();
            }
            catch 
            {
                throw;
            }
        }
    }

    public class favoriteItem
    {
        private int _key;
        private string _text;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//private eFolderChildType _folderChildType;
		private eProfileType _folderChildType;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public eFolderChildType FolderChildType
		public eProfileType FolderChildType
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		{
            get { return _folderChildType; }
            set { _folderChildType = value; }
        }

        public favoriteItem()
        {
        }

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public favoriteItem(int aKey, string aText, eFolderChildType aFolderChildType)
		public favoriteItem(int aKey, string aText, eProfileType aFolderChildType)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		{
            _key = aKey;
            _text = aText;
            _folderChildType = aFolderChildType;
        }
    }
}
