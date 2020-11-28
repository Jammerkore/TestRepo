using System.ComponentModel;   // TT#5790 - JSmith - Creating attribute can cause application to crash
using System.Windows.Forms;   // TT#5790 - JSmith - Creating attribute can cause application to crash

namespace MIDRetail.Windows.Controls
{
    partial class filterBuilderListNodeContainer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(filterBuilderListNodeContainer));
            this.pLastChild = new System.Windows.Forms.PictureBox();
            this.pChild = new System.Windows.Forms.PictureBox();
            this.pStraight = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pLastChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pStraight)).BeginInit();
            this.SuspendLayout();
            // 
            // pLastChild
            // 
            this.pLastChild.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pLastChild.Image = ((System.Drawing.Image)(resources.GetObject("pLastChild.Image")));
            this.pLastChild.Location = new System.Drawing.Point(20, 0);
            this.pLastChild.Margin = new System.Windows.Forms.Padding(0);
            this.pLastChild.Name = "pLastChild";
            this.pLastChild.Size = new System.Drawing.Size(16, 16);
            this.pLastChild.TabIndex = 4;
            this.pLastChild.TabStop = false;
            this.pLastChild.Visible = false;
            // 
            // pChild
            // 
            this.pChild.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pChild.Image = ((System.Drawing.Image)(resources.GetObject("pChild.Image")));
            this.pChild.Location = new System.Drawing.Point(20, 0);
            this.pChild.Margin = new System.Windows.Forms.Padding(0);
            this.pChild.Name = "pChild";
            this.pChild.Size = new System.Drawing.Size(16, 16);
            this.pChild.TabIndex = 5;
            this.pChild.TabStop = false;
            this.pChild.Visible = false;
            // 
            // pStraight
            // 
            this.pStraight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pStraight.Image = ((System.Drawing.Image)(resources.GetObject("pStraight.Image")));
            this.pStraight.Location = new System.Drawing.Point(20, 0);
            this.pStraight.Margin = new System.Windows.Forms.Padding(0);
            this.pStraight.Name = "pStraight";
            this.pStraight.Size = new System.Drawing.Size(16, 16);
            this.pStraight.TabIndex = 6;
            this.pStraight.TabStop = false;
            this.pStraight.Visible = false;
            // 
            // filterBuilderListNodeContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pStraight);
            this.Controls.Add(this.pChild);
            this.Controls.Add(this.pLastChild);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "filterBuilderListNodeContainer";
            this.Size = new System.Drawing.Size(36, 16);
            ((System.ComponentModel.ISupportInitialize)(this.pLastChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pStraight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pLastChild;
        private System.Windows.Forms.PictureBox pChild;
        private System.Windows.Forms.PictureBox pStraight;
    }
}
