using System;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	partial class FilterExplorer
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
			this.imageListFilter = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imageListFilter
			// 
			this.imageListFilter.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageListFilter.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListFilter.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// FilterExplorer
			// 
			this.Name = "FilterExplorer";
			this.Size = new System.Drawing.Size(216, 352);
			this.ResumeLayout(false);

		}

		private System.Windows.Forms.ImageList imageListFilter;

		#endregion
	}
}

