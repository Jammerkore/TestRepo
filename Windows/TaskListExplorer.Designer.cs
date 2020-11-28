using System;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	partial class TaskListExplorer
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
			this.imageListTaskList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imageListTaskList
			// 
			this.imageListTaskList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageListTaskList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListTaskList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// TaskListExplorer
			// 
			this.Name = "TaskListExplorer";
			this.Size = new System.Drawing.Size(216, 352);
			this.ResumeLayout(false);

		}

		private System.Windows.Forms.ImageList imageListTaskList;

		#endregion
	}
}

