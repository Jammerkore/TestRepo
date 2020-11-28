namespace MIDRetail.Windows
{
    partial class frmAddToFavorites
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
            this.pbFavoriteImage = new System.Windows.Forms.PictureBox();
            this.lblAddAFavorite = new System.Windows.Forms.Label();
            this.lblAddItem = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCreateIn = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnNewFolder = new System.Windows.Forms.Button();
            this.cboCreateIn = new MIDRetail.Windows.Controls.ComboBoxWithImages();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFavoriteImage)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // pbFavoriteImage
            // 
            this.pbFavoriteImage.Location = new System.Drawing.Point(29, 16);
            this.pbFavoriteImage.Name = "pbFavoriteImage";
            this.pbFavoriteImage.Size = new System.Drawing.Size(34, 31);
            this.pbFavoriteImage.TabIndex = 0;
            this.pbFavoriteImage.TabStop = false;
            // 
            // lblAddAFavorite
            // 
            this.lblAddAFavorite.AutoSize = true;
            this.lblAddAFavorite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddAFavorite.Location = new System.Drawing.Point(88, 16);
            this.lblAddAFavorite.Name = "lblAddAFavorite";
            this.lblAddAFavorite.Size = new System.Drawing.Size(90, 13);
            this.lblAddAFavorite.TabIndex = 1;
            this.lblAddAFavorite.Text = "Add a Favorite";
            // 
            // lblAddItem
            // 
            this.lblAddItem.AutoSize = true;
            this.lblAddItem.Location = new System.Drawing.Point(88, 44);
            this.lblAddItem.Name = "lblAddItem";
            this.lblAddItem.Size = new System.Drawing.Size(131, 13);
            this.lblAddItem.TabIndex = 3;
            this.lblAddItem.Text = "Add this item as a favorite.";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(26, 89);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "Name:";
            // 
            // lblCreateIn
            // 
            this.lblCreateIn.AutoSize = true;
            this.lblCreateIn.Location = new System.Drawing.Point(26, 122);
            this.lblCreateIn.Name = "lblCreateIn";
            this.lblCreateIn.Size = new System.Drawing.Size(52, 13);
            this.lblCreateIn.TabIndex = 5;
            this.lblCreateIn.Text = "Create in:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(91, 86);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(279, 20);
            this.txtName.TabIndex = 6;
            // 
            // btnNewFolder
            // 
            this.btnNewFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewFolder.Location = new System.Drawing.Point(297, 119);
            this.btnNewFolder.Name = "btnNewFolder";
            this.btnNewFolder.Size = new System.Drawing.Size(73, 23);
            this.btnNewFolder.TabIndex = 8;
            this.btnNewFolder.Text = "New Folder";
            this.btnNewFolder.UseVisualStyleBackColor = true;
            this.btnNewFolder.Click += new System.EventHandler(this.btnNewFolder_Click);
            // 
            // cboCreateIn
            // 
            this.cboCreateIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCreateIn.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboCreateIn.FormattingEnabled = true;
            this.cboCreateIn.ImageList = null;
            this.cboCreateIn.Location = new System.Drawing.Point(91, 119);
            this.cboCreateIn.Name = "cboCreateIn";
            this.cboCreateIn.Size = new System.Drawing.Size(198, 21);
            this.cboCreateIn.TabIndex = 9;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(214, 158);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(295, 158);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmAddToFavorites
            // 
            this.AllowDragDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 193);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cboCreateIn);
            this.Controls.Add(this.btnNewFolder);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblCreateIn);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblAddItem);
            this.Controls.Add(this.lblAddAFavorite);
            this.Controls.Add(this.pbFavoriteImage);
            this.Name = "frmAddToFavorites";
            this.Text = "Add a Favorite";
            this.Load += new System.EventHandler(this.frmAddToFavorites_Load);
            this.Controls.SetChildIndex(this.pbFavoriteImage, 0);
            this.Controls.SetChildIndex(this.lblAddAFavorite, 0);
            this.Controls.SetChildIndex(this.lblAddItem, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.lblCreateIn, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.btnNewFolder, 0);
            this.Controls.SetChildIndex(this.cboCreateIn, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFavoriteImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbFavoriteImage;
        private System.Windows.Forms.Label lblAddAFavorite;
        private System.Windows.Forms.Label lblAddItem;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCreateIn;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnNewFolder;
        private MIDRetail.Windows.Controls.ComboBoxWithImages cboCreateIn;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
    }
}