using System;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for HierarchyDetails.
	/// </summary>
	/// 

	public class frmHierarchyDetails : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView lvHierarchyDetails;
		private int sortColumn = -1;
		private ListViewItem li;
		private int X=0;
		private int Y=0;
		private string subItemText ;
		private int subItemSelected = 0 ;
        //private System.Windows.Forms.TextBox  editBox = new System.Windows.Forms.TextBox();   //TT228 - RBeck - Hierarchy Reclass message incorrectly displayed
        private TextBox editBox ;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmHierarchyDetails()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
        //TT228 - RBeck - Hierarchy Reclass message incorrectly displayed
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lvHierarchyDetails = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// lvHierarchyDetails
			// 
			this.lvHierarchyDetails.AllowColumnReorder = true;
			this.lvHierarchyDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvHierarchyDetails.HoverSelection = true;
			this.lvHierarchyDetails.Name = "lvHierarchyDetails";
			this.lvHierarchyDetails.Size = new System.Drawing.Size(672, 589);
			this.lvHierarchyDetails.TabIndex = 0;
			this.lvHierarchyDetails.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvHierarchyDetails_ColumnClick);
			this.lvHierarchyDetails.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvHierarchyDetails_MouseDown);
			this.lvHierarchyDetails.DoubleClick += new System.EventHandler(this.lvHierarchyDetails_DoubleClick);
			// 
            // editBox
			// 
            this.editBox = new System.Windows.Forms.TextBox();
            this.editBox.Size = new System.Drawing.Size(0, 0);
            this.editBox.Location = new System.Drawing.Point(0, 0);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {this.editBox});
            this.editBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditOver);
            this.editBox.LostFocus += new System.EventHandler(this.FocusOver);
            this.editBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.editBox.BackColor = Color.LightYellow;
            this.editBox.BorderStyle = BorderStyle.Fixed3D;
            this.editBox.Hide();
            this.editBox.Text = "";
            // 
            // frmHierarchyDetails
            // 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(672, 589);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {this.lvHierarchyDetails} );
            this.Controls.Add(this.editBox);
			this.Name = "frmHierarchyDetails";
			this.Text = "Hierarchy Details";
			this.ResumeLayout(false);

		}
		#endregion
	
		public void InitializeForm(TreeNode node)
		{
			lvHierarchyDetails.View = View.Details;
			lvHierarchyDetails.SmallImageList = MIDGraphics.ImageList;
			lvHierarchyDetails.Columns.Add("Name", 125, HorizontalAlignment.Left);
			lvHierarchyDetails.Columns.Add("Desc", 150, HorizontalAlignment.Left);
			lvHierarchyDetails.Columns.Add("Level", 100, HorizontalAlignment.Left);
			lvHierarchyDetails.Columns.Add("Modified", 100, HorizontalAlignment.Left);
			lvHierarchyDetails.Columns.Add("Modified By", 100, HorizontalAlignment.Left);
			Load_nodes(node);
			InitialSort();
		}

		public void RefreshForm(TreeNode node)
		{
			lvHierarchyDetails.Items.Clear();
			Load_nodes(node);
			InitialSort();
		}

		private void InitialSort()
		{
			sortColumn = 0;
			// Set the sort order to ascending by default.
			lvHierarchyDetails.Sorting = SortOrder.Ascending;
			// Call the sort method to manually sort.
			lvHierarchyDetails.Sort();
			// Set the ListViewItemSorter property to a new ListViewItemComparer
			// object.
			this.lvHierarchyDetails.ListViewItemSorter = new ListViewItemComparer(sortColumn,	lvHierarchyDetails.Sorting);
		}

		private void Load_nodes(TreeNode node)
		{
			string closedFolder;
			int imageIndex;
			
			// populate it
			switch (node.Text)
			{
				case "Mens' Wear Resort":
					closedFolder = "blue.closedfolder.gif";
					imageIndex = MIDGraphics.ImageIndex(closedFolder);
					AddHierarchyDataItem("Name", "Description", "Level", "Today" , "Joe Forecaster" ,imageIndex);
					break;
				case "Mens' Denim Tops":
					closedFolder = "blue.closedfolder.gif";
					imageIndex = MIDGraphics.ImageIndex(closedFolder);
					AddHierarchyDataItem("Name", "Description", "Level", "Today" , "Joe Forecaster" ,imageIndex);
					break;
				case "Total Earrings":
					closedFolder = "blue.closedfolder.gif";
					imageIndex = MIDGraphics.ImageIndex(closedFolder);
					AddHierarchyDataItem("Name", "Description", "Level", "Today" , "Joe Forecaster" ,imageIndex);
					break;
				case "MMS Apparel":
					closedFolder = "blue.closedfolder.gif";
					imageIndex = MIDGraphics.ImageIndex(closedFolder);
					AddHierarchyDataItem("Bath & Body Division", "Description", "Division", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Jewelry Division", "Description", "Division", "Today" , "Jane Forecaster" ,imageIndex);
					AddHierarchyDataItem("Housewares Division", "Description", "Division", "Yesterday" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Mens Division", "Description", "Division", "Today" , "Jill Forecaster" ,imageIndex);
					AddHierarchyDataItem("Womens Division", "Description", "Division", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Childrens Division", "Description", "Division", "Yesterday" , "Jane Forecaster" ,imageIndex);
					AddHierarchyDataItem("Outerwear Division", "Description", "Division", "Yesterday" , "Jill Forecaster" ,imageIndex);
					break;
				case "Mens Division":
					closedFolder = "red.closedfolder.gif";
					imageIndex = MIDGraphics.ImageIndex(closedFolder);
					AddHierarchyDataItem("Young Mens'", "Description", "Department", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Mens' Casualwear", "Description", "Department", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Suits", "Description", "Department", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Mens' Furnishings", "Description", "Department", "Today" , "Joe Forecaster" ,imageIndex);
					break;
				case "Young Mens'":
					closedFolder = "green.closedfolder.gif";
					imageIndex = MIDGraphics.ImageIndex(closedFolder);
					AddHierarchyDataItem("Slacks & Separates", "Description", "Class", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Denim", "Description", "Class", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Sweaters", "Description", "Class", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Separate Tops", "Description", "Class", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Separate Bottoms", "Description", "Class", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Collection Sportwear", "Description", "Class", "Today" , "Joe Forecaster" ,imageIndex);
					break;
				case "Denim":
					closedFolder = "purple.closedfolder.gif";
					imageIndex = MIDGraphics.ImageIndex(closedFolder);
					AddHierarchyDataItem("Basic Denim 5Pkt 12Oz Jeans Stonewash", "Description", "Style", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Basic Denim 5Pkt 12Oz Jeans Colors", "Description", "Style", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Stonewash Double Pleat 12Oz", "Description", "Style", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Stonewash 5Pkt 12Oz Short", "Description", "Style", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("Stonewash  12Oz Overall", "Description", "Style", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("L/S Button Down 10Oz Shirt", "Description", "Style", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("L/S Button Down Chambray Shirt", "Description", "Style", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("16Oz Dark Wash Jacket", "Description", "Style", "Today" , "Joe Forecaster" ,imageIndex);
					AddHierarchyDataItem("16Oz stonewash Barn Jacket", "Description", "Style", "Today" , "Joe Forecaster" ,imageIndex);
					break;
			}
		}

		private void AddHierarchyDataItem(string name, string description,  string level, string modified, string modifiedBy,int imageIndex)
		{
			string[] str;
			str = new String[5];
			str[0] = name;
			str[1] = description;
			str[2] = level;
			str[3] = modified;
			str[4] = modifiedBy;

			if (name != null)
			{
				ListViewItem listViewItem = new ListViewItem(str, imageIndex);
				lvHierarchyDetails.Items.Add(listViewItem);
			}
		}

		private void lvHierarchyDetails_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			// Determine whether the column is the same as the last column clicked.
			if (e.Column != sortColumn)
			{
				// Set the sort column to the new column.
				sortColumn = e.Column;
				// Set the sort order to ascending by default.
				lvHierarchyDetails.Sorting = SortOrder.Ascending;
			}
			else
			{
				// Determine what the last sort order was and change it.
				if (lvHierarchyDetails.Sorting == SortOrder.Ascending)
					lvHierarchyDetails.Sorting = SortOrder.Descending;
				else
					lvHierarchyDetails.Sorting = SortOrder.Ascending;
			}

			// Call the sort method to manually sort.
			lvHierarchyDetails.Sort();
			// Set the ListViewItemSorter property to a new ListViewItemComparer
			// object.
			this.lvHierarchyDetails.ListViewItemSorter = new ListViewItemComparer(e.Column,
				lvHierarchyDetails.Sorting);
		}

		private void EditOver(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == 13 ) 
			{
				li.SubItems[subItemSelected].Text = editBox.Text;
				editBox.Hide();
			}

			if ( e.KeyChar == 27 ) 
				editBox.Hide();
		}

		private void FocusOver(object sender, System.EventArgs e)
		{
			li.SubItems[subItemSelected].Text = editBox.Text;
			editBox.Hide();
		}

		public  void lvHierarchyDetails_DoubleClick(object sender, System.EventArgs e)
		{
			// Check the subitem clicked .
			int nStart = X ;
			int spos = 20 ; 
			int epos = lvHierarchyDetails.Columns[0].Width ;
			for ( int i=0; i < lvHierarchyDetails.Columns.Count ; i++)
			{
				if ( nStart > spos && nStart < epos ) 
				{
					subItemSelected = i ;
					break; 
				}
				
				spos = epos ; 
				epos += lvHierarchyDetails.Columns[i].Width;
			}

//			Console.WriteLine("SUB ITEM SELECTED = " + li.SubItems[subItemSelected].Text);
			subItemText = li.SubItems[subItemSelected].Text ;

			string colName = lvHierarchyDetails.Columns[subItemSelected].Text ;
			if ( colName == "Name" ) 
			{
				Rectangle r = new Rectangle(spos , li.Bounds.Y , epos , li.Bounds.Bottom);
				editBox.Size  = new System.Drawing.Size(epos - spos , li.Bounds.Bottom-li.Bounds.Top);
				editBox.Location = new System.Drawing.Point(spos , li.Bounds.Y);
				editBox.Show() ;
				editBox.Text = subItemText;
				editBox.SelectAll() ;
				editBox.Focus();
			}
		}

		public void lvHierarchyDetails_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			li = lvHierarchyDetails.GetItemAt(e.X , e.Y);
			X = e.X ;
			Y = e.Y ;
		}


	}
}
