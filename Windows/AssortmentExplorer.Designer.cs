namespace MIDRetail.Windows
{
	partial class AssortmentExplorer
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
			this.imageListAssortment = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imageListAssortment
			// 
			this.imageListAssortment.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageListAssortment.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListAssortment.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// AssortmentExplorer
			// 
			this.Name = "AssortmentExplorer";
			this.Size = new System.Drawing.Size(243, 370);
			this.ResumeLayout(false);

		}

		#endregion

		private MIDRetail.Windows.Controls.MIDTreeView midTreeView1;
		private System.Windows.Forms.ImageList imageListAssortment;
	}
}