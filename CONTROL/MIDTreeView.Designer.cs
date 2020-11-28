using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
	partial class MIDTreeView
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imageListDrag
			// 
			this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
			this.ResumeLayout(false);

		}

		private ImageList imageListDrag;

		#endregion
	}
}
