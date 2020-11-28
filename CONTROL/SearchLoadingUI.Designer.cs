namespace MIDRetail.Windows.Controls
{
    partial class SearchLoadingUI
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchLoadingUI));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ultraButton1, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(520, 326);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(213, 66);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(94, 94);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.TextHAlignAsString = "Center";
            this.ultraLabel1.Appearance = appearance3;
            this.tableLayoutPanel1.SetColumnSpan(this.ultraLabel1, 3);
            this.ultraLabel1.Location = new System.Drawing.Point(3, 166);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(514, 16);
            this.ultraLabel1.TabIndex = 5;
            this.ultraLabel1.Text = "Please wait.  Loading results.....";
            this.ultraLabel1.UseAppStyling = false;
            // 
            // ultraButton1
            // 
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.ultraButton1.Appearance = appearance4;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Windows8Button;
            this.ultraButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraButton1.Location = new System.Drawing.Point(213, 188);
            this.ultraButton1.MaximumSize = new System.Drawing.Size(92, 28);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(92, 28);
            this.ultraButton1.TabIndex = 6;
            this.ultraButton1.Text = "Cancel";
            this.ultraButton1.UseAppStyling = false;
            this.ultraButton1.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "loading17.png");
            this.imageList1.Images.SetKeyName(1, "loading18.png");
            this.imageList1.Images.SetKeyName(2, "loading19.png");
            this.imageList1.Images.SetKeyName(3, "loading20.png");
            this.imageList1.Images.SetKeyName(4, "loading21.png");
            this.imageList1.Images.SetKeyName(5, "loading22.png");
            this.imageList1.Images.SetKeyName(6, "loading23.png");
            this.imageList1.Images.SetKeyName(7, "loading24.png");
            this.imageList1.Images.SetKeyName(8, "loading1.png");
            this.imageList1.Images.SetKeyName(9, "loading2.png");
            this.imageList1.Images.SetKeyName(10, "loading3.png");
            this.imageList1.Images.SetKeyName(11, "loading4.png");
            this.imageList1.Images.SetKeyName(12, "loading5.png");
            this.imageList1.Images.SetKeyName(13, "loading6.png");
            this.imageList1.Images.SetKeyName(14, "loading7.png");
            this.imageList1.Images.SetKeyName(15, "loading8.png");
            this.imageList1.Images.SetKeyName(16, "loading9.png");
            this.imageList1.Images.SetKeyName(17, "loading10.png");
            this.imageList1.Images.SetKeyName(18, "loading11.png");
            this.imageList1.Images.SetKeyName(19, "loading12.png");
            this.imageList1.Images.SetKeyName(20, "loading13.png");
            this.imageList1.Images.SetKeyName(21, "loading14.png");
            this.imageList1.Images.SetKeyName(22, "loading15.png");
            this.imageList1.Images.SetKeyName(23, "loading16.png");
            // 
            // SearchLoadingUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SearchLoadingUI";
            this.Size = new System.Drawing.Size(520, 326);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.ImageList imageList1;
        public System.Windows.Forms.PictureBox pictureBox1;

    }
}
