using System.ComponentModel;   // TT#5790 - JSmith - Creating attribute can cause application to crash
using System.Windows.Forms;   // TT#5790 - JSmith - Creating attribute can cause application to crash

namespace MIDRetail.Windows.Controls
{
    partial class filterBuilderListNode
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

            RemoveHandlerList(this);  // TT#5790 - JSmith - Creating attribute can cause application to crash

            base.Dispose(disposing);
        }

        // Begin TT#5790 - JSmith - Creating attribute can cause application to crash
        public void RemoveHandlerList(Control c)
        {
            EventHandlerList list = (EventHandlerList)typeof(Control).GetProperty("Events", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(c, null);
            typeof(EventHandlerList).GetMethod("Dispose").Invoke(list, null);
        }
        // End TT#5790 - JSmith - Creating attribute can cause application to crash


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(filterBuilderListNode));
            this.e1 = new Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor();
            this.pEmpty = new System.Windows.Forms.PictureBox();
            this.pSelected = new System.Windows.Forms.PictureBox();
            this.pHotTrack = new System.Windows.Forms.PictureBox();
            this.u1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.aaControl1 = new MIDRetail.Windows.Controls.filterBuilderListNodeOverlay();
            ((System.ComponentModel.ISupportInitialize)(this.pEmpty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pHotTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.u1)).BeginInit();
            this.SuspendLayout();
            // 
            // e1
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance2.BackColorDisabled = System.Drawing.Color.White;
            appearance2.BackColorDisabled2 = System.Drawing.Color.White;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance2.BorderColor = System.Drawing.Color.White;
            appearance2.FontData.BoldAsString = "False";
            appearance2.FontData.ItalicAsString = "False";
            appearance2.FontData.Name = "Microsoft Sans Serif";
            appearance2.FontData.SizeInPoints = 8.25F;
            appearance2.FontData.StrikeoutAsString = "False";
            appearance2.FontData.UnderlineAsString = "False";
            this.e1.Appearance = appearance2;
            this.e1.AutoSize = true;
            this.e1.HideSelection = false;
            this.e1.Location = new System.Drawing.Point(19, 0);
            this.e1.Margin = new System.Windows.Forms.Padding(0);
            this.e1.Name = "e1";
            this.e1.ReadOnly = true;
            this.e1.ScrollBarDisplayStyle = Infragistics.Win.UltraWinScrollBar.ScrollBarDisplayStyle.Never;
            this.e1.Size = new System.Drawing.Size(125, 16);
            this.e1.TabIndex = 2;
            this.e1.TabStop = false;
            this.e1.UseAppStyling = false;
            this.e1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.e1.Value = "And Conditon = \'Not Set\'";
            this.e1.WrapText = false;
            this.e1.Enter += new System.EventHandler(this.e1_Enter);
            this.e1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.e1_KeyDown);
            this.e1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.e1_KeyPress);
            this.e1.Leave += new System.EventHandler(this.e1_Leave);
            // 
            // pEmpty
            // 
            this.pEmpty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pEmpty.Image = ((System.Drawing.Image)(resources.GetObject("pEmpty.Image")));
            this.pEmpty.Location = new System.Drawing.Point(0, -2);
            this.pEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.pEmpty.Name = "pEmpty";
            this.pEmpty.Size = new System.Drawing.Size(18, 18);
            this.pEmpty.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pEmpty.TabIndex = 3;
            this.pEmpty.TabStop = false;
            // 
            // pSelected
            // 
            this.pSelected.Image = ((System.Drawing.Image)(resources.GetObject("pSelected.Image")));
            this.pSelected.Location = new System.Drawing.Point(0, -2);
            this.pSelected.Margin = new System.Windows.Forms.Padding(0);
            this.pSelected.Name = "pSelected";
            this.pSelected.Size = new System.Drawing.Size(18, 18);
            this.pSelected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pSelected.TabIndex = 4;
            this.pSelected.TabStop = false;
            this.pSelected.Visible = false;
            // 
            // pHotTrack
            // 
            this.pHotTrack.Image = ((System.Drawing.Image)(resources.GetObject("pHotTrack.Image")));
            this.pHotTrack.Location = new System.Drawing.Point(0, -2);
            this.pHotTrack.Margin = new System.Windows.Forms.Padding(0);
            this.pHotTrack.Name = "pHotTrack";
            this.pHotTrack.Size = new System.Drawing.Size(18, 18);
            this.pHotTrack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pHotTrack.TabIndex = 6;
            this.pHotTrack.TabStop = false;
            this.pHotTrack.Visible = false;
            // 
            // u1
            // 
            this.u1.AutoSize = false;
            this.u1.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.u1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.u1.Location = new System.Drawing.Point(25, 0);
            this.u1.Margin = new System.Windows.Forms.Padding(0);
            this.u1.Name = "u1";
            this.u1.Size = new System.Drawing.Size(10, 14);
            this.u1.TabIndex = 1;
            this.u1.UseAppStyling = false;
            this.u1.Enter += new System.EventHandler(this.u1_Enter);
            this.u1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.u1_KeyDown);
            this.u1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.u1_KeyPress);
            this.u1.Leave += new System.EventHandler(this.u1_Leave);
            // 
            // aaControl1
            // 
            this.aaControl1.BackColor = System.Drawing.Color.Transparent;
            this.aaControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.aaControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aaControl1.Location = new System.Drawing.Point(0, 0);
            this.aaControl1.Margin = new System.Windows.Forms.Padding(0);
            this.aaControl1.Name = "aaControl1";
            this.aaControl1.Size = new System.Drawing.Size(144, 16);
            this.aaControl1.TabIndex = 5;
            this.aaControl1.TabStop = false;
            this.aaControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.e1_MouseClick);
            this.aaControl1.MouseEnter += new System.EventHandler(this.e1_MouseEnter);
            this.aaControl1.MouseLeave += new System.EventHandler(this.e1_MouseLeave);
            // 
            // filterBuilderListNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.aaControl1);
            this.Controls.Add(this.pEmpty);
            this.Controls.Add(this.pHotTrack);
            this.Controls.Add(this.pSelected);
            this.Controls.Add(this.e1);
            this.Controls.Add(this.u1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "filterBuilderListNode";
            this.Size = new System.Drawing.Size(144, 16);
            ((System.ComponentModel.ISupportInitialize)(this.pEmpty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pHotTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.u1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pEmpty;
        private System.Windows.Forms.PictureBox pSelected;
        private filterBuilderListNodeOverlay aaControl1;
        private System.Windows.Forms.PictureBox pHotTrack;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor u1;
        public Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor e1;

    }
}
