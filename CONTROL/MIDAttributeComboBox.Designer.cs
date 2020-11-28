using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
    partial class MIDAttributeComboBox
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

                
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.SelectedIndexChanged -= new System.EventHandler(this.MIDAttributeComboBox_SelectedIndexChanged);
                this.SelectionChangeCommitted -= new System.EventHandler(this.MIDAttributeComboBox_SelectionChangeCommitted);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                this.DragDrop -= new System.Windows.Forms.DragEventHandler(this.MIDAttributeComboBox_DragDrop);
                this.DragEnter -= new System.Windows.Forms.DragEventHandler(this.MIDAttributeComboBox_DragEnter);
                this.SelectedValueChanged -= new System.EventHandler(this.MIDAttributeComboBox_SelectedValueChanged);
                this.KeyDown -= new KeyEventHandler(this.MIDAttributeComboBox_KeyDown);
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
            this.SuspendLayout();
            // 
            // MIDAttributeComboBox
            // 
            this.AllowDrop = true;
            this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.SelectedIndexChanged += new System.EventHandler(this.MIDAttributeComboBox_SelectedIndexChanged);
            this.SelectionChangeCommitted += new System.EventHandler(this.MIDAttributeComboBox_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MIDAttributeComboBox_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MIDAttributeComboBox_DragEnter);
            this.SelectedValueChanged += new System.EventHandler(this.MIDAttributeComboBox_SelectedValueChanged);
            this.KeyDown += new KeyEventHandler(this.MIDAttributeComboBox_KeyDown); //TT#306-MD-VStuart-Version 5.0-Size Review not working correctly.
            this.ResumeLayout(false);

        }

        #endregion
    }
}
