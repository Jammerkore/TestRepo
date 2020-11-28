namespace MIDRetail.Windows.Controls
{
    partial class filterElementCalendar
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(filterElementCalendar));
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.mdsDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // ultraLabel3
            // 
            appearance1.TextHAlignAsString = "Right";
            this.ultraLabel3.Appearance = appearance1;
            this.ultraLabel3.Location = new System.Drawing.Point(2, 4);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(72, 14);
            this.ultraLabel3.TabIndex = 10;
            this.ultraLabel3.Text = "Date Range:";
            this.ultraLabel3.UseAppStyling = false;
            // 
            // mdsDateRange
            // 
            this.mdsDateRange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mdsDateRange.DateRangeForm = null;
            this.mdsDateRange.DateRangeRID = 0;
            this.mdsDateRange.Location = new System.Drawing.Point(74, 0);
            this.mdsDateRange.Name = "mdsDateRange";
            this.mdsDateRange.Size = new System.Drawing.Size(187, 24);
            this.mdsDateRange.TabIndex = 34;
            this.mdsDateRange.Click += new System.EventHandler(this.mdsDateRange_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "DynamicToPlan.gif");
            this.imageList1.Images.SetKeyName(1, "DynamicToCurrent.gif");
            // 
            // filterElementCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.mdsDateRange);
            this.Controls.Add(this.ultraLabel3);
            this.Name = "filterElementCalendar";
            this.Size = new System.Drawing.Size(262, 24);
            this.Load += new System.EventHandler(this.filterElementCalendar_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private MIDDateRangeSelector mdsDateRange;
        private System.Windows.Forms.ImageList imageList1;



    }
}
