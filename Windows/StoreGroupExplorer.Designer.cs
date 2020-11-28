using System;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	partial class StoreGroupExplorer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.imageListStoreGroup = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imageListStoreGroup
			// 
			this.imageListStoreGroup.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageListStoreGroup.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListStoreGroup.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// StoreGroupExplorer
			// 
			this.Name = "StoreGroupExplorer";
			this.Size = new System.Drawing.Size(216, 352);
			this.ResumeLayout(false);

		}

		private System.Windows.Forms.ImageList imageListStoreGroup;

		#endregion
	}
}