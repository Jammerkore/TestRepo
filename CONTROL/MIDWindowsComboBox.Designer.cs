namespace MIDRetail.Windows.Controls
{
    partial class MIDWindowsComboBox
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
     		// Begin TT#856 - JSmith - Out of memory
			//if (disposing && (components != null))
            //{
            //    components.Dispose();
            //}
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                this.SelectedIndexChanged -= new System.EventHandler(this.MIDWindowsComboBox_SelectedIndexChanged);
            }
			// End TT#856
			
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
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MIDWindowsComboBox
            // 
            this.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectedIndexChanged += new System.EventHandler(this.MIDWindowsComboBox_SelectedIndexChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListDrag;
    }
}
