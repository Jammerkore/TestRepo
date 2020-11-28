using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Logility.ROWeb
{
    public partial class WorkUltraGrid : Form
    {
        UltraGrid workUltraGrid;

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
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 255);
            this.Name = "Form1";
            this.Text = "Form1";

            this.ResumeLayout(false);

        }

        #endregion

        public WorkUltraGrid()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Tan;
            this.Text = "WinGrid from Code";
        }

        public UltraGrid Grid
        {
            get
            {
                return workUltraGrid;
            }
        }

        public void BuildGrid(BindingSource bs)
        {
            workUltraGrid = new UltraGrid();
            workUltraGrid.Size = new System.Drawing.Size(550, 192);
            workUltraGrid.Location = new System.Drawing.Point(13, 22);
            // grid must be added to controls list before rows will be available
            this.Controls.Add(workUltraGrid);

            workUltraGrid.DataSource = bs;

        }
    }

}
