namespace MIDRetail.Windows.Controls
{
    partial class SelectSingleHierarchyNodeControl
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
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton("ShowHierarchy");
            this.txtHierarchyNode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtHierarchyNode)).BeginInit();
            this.SuspendLayout();
            // 
            // txtHierarchyNode
            // 
            this.txtHierarchyNode.AllowDrop = true;
            editorButton1.Key = "ShowHierarchy";
            editorButton1.Text = "...";
            editorButton1.Visible = false;
            this.txtHierarchyNode.ButtonsRight.Add(editorButton1);
            this.txtHierarchyNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHierarchyNode.Location = new System.Drawing.Point(0, 0);
            this.txtHierarchyNode.Name = "txtHierarchyNode";
            this.txtHierarchyNode.Size = new System.Drawing.Size(150, 21);
            this.txtHierarchyNode.TabIndex = 22;
            this.txtHierarchyNode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtHierarchyNode_EditorButtonClick);
            this.txtHierarchyNode.TextChanged += new System.EventHandler(this.txtHierarchyNode_TextChanged);
            this.txtHierarchyNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtHierarchyNode_DragDrop);
            this.txtHierarchyNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtHierarchyNode_DragEnter);
            this.txtHierarchyNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtHierarchyNode_DragOver);
            this.txtHierarchyNode.DragLeave += new System.EventHandler(this.txtHierarchyNode_DragLeave);
            this.txtHierarchyNode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHierarchyNode_KeyDown);
            this.txtHierarchyNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtHierarchyNode_Validating);
            this.txtHierarchyNode.Validated += new System.EventHandler(this.txtHierarchyNode_Validated);
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // SelectSingleHierarchyNodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtHierarchyNode);
            this.Name = "SelectSingleHierarchyNodeControl";
            this.Size = new System.Drawing.Size(150, 21);
            ((System.ComponentModel.ISupportInitialize)(this.txtHierarchyNode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtHierarchyNode;
        private System.Windows.Forms.ImageList imageListDrag;
    }
}
