namespace UnitTesting
{
    partial class PlanOptions
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
            this.chkValidateStoredProcedures = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkCreateNewDatabase = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkUpgradeStandardDatabase = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkRunUnitTests = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtPlan = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.txtFolderPath = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.chkRemoveUpgradeDBOnFailure = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkUseUpgradedDatabaseForUnitTests = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkRemoveUpgradeDBOnSuccess = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkRemoveNewDBOnSuccess = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkRemoveNewDBOnFailure = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkStopOnFirstUnitTestFailure = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkDeletePriorResultFolders = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.chkValidateStoredProcedures)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCreateNewDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUpgradeStandardDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRunUnitTests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFolderPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemoveUpgradeDBOnFailure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseUpgradedDatabaseForUnitTests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemoveUpgradeDBOnSuccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemoveNewDBOnSuccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemoveNewDBOnFailure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStopOnFirstUnitTestFailure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDeletePriorResultFolders)).BeginInit();
            this.SuspendLayout();
            // 
            // chkValidateStoredProcedures
            // 
            this.chkValidateStoredProcedures.Location = new System.Drawing.Point(3, 87);
            this.chkValidateStoredProcedures.Name = "chkValidateStoredProcedures";
            this.chkValidateStoredProcedures.Size = new System.Drawing.Size(191, 20);
            this.chkValidateStoredProcedures.TabIndex = 0;
            this.chkValidateStoredProcedures.Text = "Validate Stored Procedures";
            // 
            // chkCreateNewDatabase
            // 
            this.chkCreateNewDatabase.Location = new System.Drawing.Point(3, 108);
            this.chkCreateNewDatabase.Name = "chkCreateNewDatabase";
            this.chkCreateNewDatabase.Size = new System.Drawing.Size(191, 20);
            this.chkCreateNewDatabase.TabIndex = 1;
            this.chkCreateNewDatabase.Text = "Create New Database";
            this.chkCreateNewDatabase.CheckedChanged += new System.EventHandler(this.chkNewDatabase_CheckedChanged);
            // 
            // chkUpgradeStandardDatabase
            // 
            this.chkUpgradeStandardDatabase.Location = new System.Drawing.Point(3, 171);
            this.chkUpgradeStandardDatabase.Name = "chkUpgradeStandardDatabase";
            this.chkUpgradeStandardDatabase.Size = new System.Drawing.Size(191, 20);
            this.chkUpgradeStandardDatabase.TabIndex = 2;
            this.chkUpgradeStandardDatabase.Text = "Upgrade Standard Database";
            this.chkUpgradeStandardDatabase.CheckedChanged += new System.EventHandler(this.chkUpgradeStandardDatabase_CheckedChanged);
            // 
            // chkRunUnitTests
            // 
            this.chkRunUnitTests.Location = new System.Drawing.Point(3, 255);
            this.chkRunUnitTests.Name = "chkRunUnitTests";
            this.chkRunUnitTests.Size = new System.Drawing.Size(191, 20);
            this.chkRunUnitTests.TabIndex = 3;
            this.chkRunUnitTests.Text = "Run Unit Tests";
            this.chkRunUnitTests.CheckedChanged += new System.EventHandler(this.chkRunUnitTests_CheckedChanged);
            // 
            // txtPlan
            // 
            this.txtPlan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPlan.Location = new System.Drawing.Point(3, 17);
            this.txtPlan.Name = "txtPlan";
            this.txtPlan.Size = new System.Drawing.Size(301, 21);
            this.txtPlan.TabIndex = 31;
            // 
            // ultraLabel7
            // 
            this.ultraLabel7.Location = new System.Drawing.Point(3, 1);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel7.TabIndex = 30;
            this.ultraLabel7.Text = "Test Plan:";
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderPath.Location = new System.Drawing.Point(3, 60);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(301, 21);
            this.txtFolderPath.TabIndex = 29;
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.Location = new System.Drawing.Point(3, 44);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel6.TabIndex = 28;
            this.ultraLabel6.Text = "Test Plan Folder:";
            // 
            // chkRemoveUpgradeDBOnFailure
            // 
            this.chkRemoveUpgradeDBOnFailure.Enabled = false;
            this.chkRemoveUpgradeDBOnFailure.Location = new System.Drawing.Point(23, 213);
            this.chkRemoveUpgradeDBOnFailure.Name = "chkRemoveUpgradeDBOnFailure";
            this.chkRemoveUpgradeDBOnFailure.Size = new System.Drawing.Size(261, 20);
            this.chkRemoveUpgradeDBOnFailure.TabIndex = 33;
            this.chkRemoveUpgradeDBOnFailure.Text = "Remove Database On Failure";
            // 
            // chkUseUpgradedDatabaseForUnitTests
            // 
            this.chkUseUpgradedDatabaseForUnitTests.Enabled = false;
            this.chkUseUpgradedDatabaseForUnitTests.Location = new System.Drawing.Point(23, 192);
            this.chkUseUpgradedDatabaseForUnitTests.Name = "chkUseUpgradedDatabaseForUnitTests";
            this.chkUseUpgradedDatabaseForUnitTests.Size = new System.Drawing.Size(234, 20);
            this.chkUseUpgradedDatabaseForUnitTests.TabIndex = 32;
            this.chkUseUpgradedDatabaseForUnitTests.Text = "Use Upgraded Database For Unit Tests";
            // 
            // chkRemoveUpgradeDBOnSuccess
            // 
            this.chkRemoveUpgradeDBOnSuccess.Enabled = false;
            this.chkRemoveUpgradeDBOnSuccess.Location = new System.Drawing.Point(23, 234);
            this.chkRemoveUpgradeDBOnSuccess.Name = "chkRemoveUpgradeDBOnSuccess";
            this.chkRemoveUpgradeDBOnSuccess.Size = new System.Drawing.Size(261, 20);
            this.chkRemoveUpgradeDBOnSuccess.TabIndex = 34;
            this.chkRemoveUpgradeDBOnSuccess.Text = "Remove Database On Success";
            // 
            // chkRemoveNewDBOnSuccess
            // 
            this.chkRemoveNewDBOnSuccess.Enabled = false;
            this.chkRemoveNewDBOnSuccess.Location = new System.Drawing.Point(23, 150);
            this.chkRemoveNewDBOnSuccess.Name = "chkRemoveNewDBOnSuccess";
            this.chkRemoveNewDBOnSuccess.Size = new System.Drawing.Size(261, 20);
            this.chkRemoveNewDBOnSuccess.TabIndex = 36;
            this.chkRemoveNewDBOnSuccess.Text = "Remove Database On Success";
            // 
            // chkRemoveNewDBOnFailure
            // 
            this.chkRemoveNewDBOnFailure.Enabled = false;
            this.chkRemoveNewDBOnFailure.Location = new System.Drawing.Point(23, 129);
            this.chkRemoveNewDBOnFailure.Name = "chkRemoveNewDBOnFailure";
            this.chkRemoveNewDBOnFailure.Size = new System.Drawing.Size(261, 20);
            this.chkRemoveNewDBOnFailure.TabIndex = 35;
            this.chkRemoveNewDBOnFailure.Text = "Remove Database On Failure";
            // 
            // chkStopOnFirstUnitTestFailure
            // 
            this.chkStopOnFirstUnitTestFailure.Enabled = false;
            this.chkStopOnFirstUnitTestFailure.Location = new System.Drawing.Point(23, 275);
            this.chkStopOnFirstUnitTestFailure.Name = "chkStopOnFirstUnitTestFailure";
            this.chkStopOnFirstUnitTestFailure.Size = new System.Drawing.Size(234, 20);
            this.chkStopOnFirstUnitTestFailure.TabIndex = 37;
            this.chkStopOnFirstUnitTestFailure.Text = "Stop On First Failure";
            // 
            // chkDeletePriorResultFolders
            // 
            this.chkDeletePriorResultFolders.Enabled = false;
            this.chkDeletePriorResultFolders.Location = new System.Drawing.Point(23, 296);
            this.chkDeletePriorResultFolders.Name = "chkDeletePriorResultFolders";
            this.chkDeletePriorResultFolders.Size = new System.Drawing.Size(234, 20);
            this.chkDeletePriorResultFolders.TabIndex = 38;
            this.chkDeletePriorResultFolders.Text = "Delete Prior Result Folders";
            // 
            // PlanOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chkDeletePriorResultFolders);
            this.Controls.Add(this.chkStopOnFirstUnitTestFailure);
            this.Controls.Add(this.chkRemoveNewDBOnSuccess);
            this.Controls.Add(this.chkRemoveNewDBOnFailure);
            this.Controls.Add(this.chkRemoveUpgradeDBOnSuccess);
            this.Controls.Add(this.chkRemoveUpgradeDBOnFailure);
            this.Controls.Add(this.chkUseUpgradedDatabaseForUnitTests);
            this.Controls.Add(this.txtPlan);
            this.Controls.Add(this.ultraLabel7);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.ultraLabel6);
            this.Controls.Add(this.chkRunUnitTests);
            this.Controls.Add(this.chkUpgradeStandardDatabase);
            this.Controls.Add(this.chkCreateNewDatabase);
            this.Controls.Add(this.chkValidateStoredProcedures);
            this.Name = "PlanOptions";
            this.Size = new System.Drawing.Size(322, 377);
            ((System.ComponentModel.ISupportInitialize)(this.chkValidateStoredProcedures)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCreateNewDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUpgradeStandardDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRunUnitTests)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFolderPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemoveUpgradeDBOnFailure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseUpgradedDatabaseForUnitTests)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemoveUpgradeDBOnSuccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemoveNewDBOnSuccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemoveNewDBOnFailure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkStopOnFirstUnitTestFailure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDeletePriorResultFolders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkValidateStoredProcedures;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkCreateNewDatabase;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkUpgradeStandardDatabase;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkRunUnitTests;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtPlan;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtFolderPath;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkRemoveUpgradeDBOnFailure;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkUseUpgradedDatabaseForUnitTests;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkRemoveUpgradeDBOnSuccess;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkRemoveNewDBOnSuccess;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkRemoveNewDBOnFailure;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkStopOnFirstUnitTestFailure;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkDeletePriorResultFolders;

    }
}
