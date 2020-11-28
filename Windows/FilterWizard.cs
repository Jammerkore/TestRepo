//using System;
//using System.Collections;
//using System.ComponentModel;
//using System.Configuration;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.Globalization;
//using System.IO;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Windows.Forms;
//using System.Runtime.InteropServices; // TT#3034 - RMatelic -Display of arrows vs '+' with Window 7 

//using MIDRetail.DataCommon;
//using MIDRetail.Common;
//using MIDRetail.Data;
//using MIDRetail.Business;
//using MIDRetail.Windows;
//using MIDRetail.Windows.Controls;

//namespace MIDRetail.Windows
//{
//    using System;
//    using System.Windows.Forms;

    	
//    /// <summary>
//    /// Summary description for FilterWizard.
//    /// </summary>
//    public class frmFilterWizard : MIDFormBase
//    {
//        #region Windows Form Designer generated code

//        private System.Windows.Forms.Button btnCancel;
//        private System.Windows.Forms.Label label1;
//        private System.Windows.Forms.Label label2;
//        private MIDRetail.Windows.FilterWizardIntroPanel pnlIntro;
//        private System.Windows.Forms.RichTextBox richTextBox1;
//        private System.Windows.Forms.Button btnBack;
//        private System.Windows.Forms.Button btnNext;
//        private System.Windows.Forms.Panel pnlAttributeTitle;
//        private System.Windows.Forms.Label label3;
//        private System.Windows.Forms.Label label4;
//        private MIDRetail.Windows.FilterWizardAttributePanel pnlAttributeMain;
//        private System.Windows.Forms.ImageList imgAttributes;
//        private System.Windows.Forms.TreeView trvAttributes;
//        private System.Windows.Forms.TreeView trvStores;
//        private MIDRetail.Windows.FilterWizardDataPanel pnlDataMain;
//        private System.Windows.Forms.Panel pnlDataTitle;
//        private System.Windows.Forms.Label label7;
//        private System.Windows.Forms.Label label8;
//        private System.Windows.Forms.CheckBox chkSets;
//        private System.Windows.Forms.CheckBox chkData;
//        private MIDRetail.Windows.FilterWizardFinishPanel pnlFinish;
//        private System.Windows.Forms.RichTextBox richTextBox2;
//        private System.Windows.Forms.Label label10;
//        private MIDRetail.Windows.FilterWizardNamePanel pnlNameMain;
//        private System.Windows.Forms.Panel pnlNameTitle;
//        private System.Windows.Forms.Label label9;
//        private System.Windows.Forms.Label label11;
//        private System.Windows.Forms.RichTextBox richTextBox4;
//        private System.Windows.Forms.RichTextBox richTextBox5;
//        private System.Windows.Forms.TextBox txtName;
//        private System.Windows.Forms.Label label12;
//        private System.Windows.Forms.Label label13;
//        private System.Windows.Forms.TreeView trvConditions;
//        private System.Windows.Forms.Button btnAddCondition;
//        private System.Windows.Forms.RadioButton rdoGlobal;
//        private System.Windows.Forms.RadioButton rdoUser;
//        private System.Windows.Forms.RadioButton rdoAttributeOr;
//        private System.Windows.Forms.RadioButton rdoAttributeAnd;
//        private System.Windows.Forms.Button btnEditCondition;
//        private System.Windows.Forms.ContextMenu ctmConditions;
//        private System.Windows.Forms.ContextMenu ctmAttributes;
//        private System.Windows.Forms.ContextMenu ctmStores;
//        private System.Windows.Forms.MenuItem mniAttrDelete;
//        private System.Windows.Forms.MenuItem mniCondDelete;
//        private System.Windows.Forms.MenuItem mniStoreDelete;
//        private System.Windows.Forms.Label lblAttributeConjunction;
//        private System.Windows.Forms.Panel pnlAttributeConjunction;
//        private System.Windows.Forms.Panel pnlDataConjunction;
//        private System.Windows.Forms.Panel pnlDataInfo;
//        private System.Windows.Forms.Label lblConditionConjunction;
//        private System.Windows.Forms.RadioButton rdoConditionOr;
//        private System.Windows.Forms.RadioButton rdoConditionAnd;
//        private System.Windows.Forms.RichTextBox rtbConditionInfo;
//        private System.Windows.Forms.PictureBox pictureBox3;
//        private System.Windows.Forms.PictureBox pictureBox1;
//        private System.Windows.Forms.RichTextBox richTextBox3;
//        private System.ComponentModel.IContainer components;

//        public frmFilterWizard(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB)
//            : base(aSAB)
//        {
//            //
//            // Required for Windows Form Designer support
//            //
//            InitializeComponent();

//            //
//            // TODO: Add any constructor code after InitializeComponent call
//            //

//            _SAB = aSAB;
//            _EAB = aEAB;
//        }

//        // Begin TT#3034 - RMatelic -Display of arrows vs '+' with Window 7 
//        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
//        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

//        protected override void CreateHandle()
//        {
//            base.CreateHandle();
//            SetWindowTheme(trvAttributes.Handle, "explorer", null);
//            SetWindowTheme(trvStores.Handle, "explorer", null);
//            SetWindowTheme(trvConditions.Handle, "explorer", null);
//        }
//        // End TT#3034

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        protected override void Dispose( bool disposing )
//        {
//            if( disposing )
//            {
//                if(components != null)
//                {
//                    components.Dispose();
//                }

//                this.btnBack.Click -= new System.EventHandler(this.btnBack_Click);
//                this.btnNext.Click -= new System.EventHandler(this.btnNext_Click);
//                this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
//                this.chkData.CheckedChanged -= new System.EventHandler(this.chkSetsData_CheckedChanged);
//                this.chkSets.CheckedChanged -= new System.EventHandler(this.chkSetsData_CheckedChanged);
//                this.rdoAttributeOr.CheckedChanged -= new System.EventHandler(this.rdoAttributeOr_CheckedChanged);
//                this.rdoAttributeAnd.CheckedChanged -= new System.EventHandler(this.rdoAttributeAnd_CheckedChanged);
//                this.trvStores.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.trvStores_KeyUp);
//                this.trvStores.DragEnter -= new System.Windows.Forms.DragEventHandler(this.trvStores_DragEnter);
//                this.trvStores.DragDrop -= new System.Windows.Forms.DragEventHandler(this.trvStores_DragDrop);
//                this.trvStores.Enter -= new System.EventHandler(this.trvStores_Enter);
//                this.mniStoreDelete.Click -= new System.EventHandler(this.mniStoreDelete_Click);
//                this.trvAttributes.AfterExpand -= new System.Windows.Forms.TreeViewEventHandler(this.trvAttributes_AfterExpand);
//                this.trvAttributes.AfterCollapse -= new System.Windows.Forms.TreeViewEventHandler(this.trvAttributes_AfterCollapse);
//                this.trvAttributes.DragOver -= new System.Windows.Forms.DragEventHandler(this.trvAttributes_DragOver);
//                this.trvAttributes.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.trvAttributes_KeyUp);
//                this.trvAttributes.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.trvAttributes_AfterSelect);
//                this.trvAttributes.DragEnter -= new System.Windows.Forms.DragEventHandler(this.trvAttributes_DragEnter);
//                this.trvAttributes.DragDrop -= new System.Windows.Forms.DragEventHandler(this.trvAttributes_DragDrop);
//                this.trvAttributes.Enter -= new System.EventHandler(this.trvAttributes_Enter);
//                this.mniAttrDelete.Click -= new System.EventHandler(this.mniAttrDelete_Click);
//                this.rdoConditionOr.CheckedChanged -= new System.EventHandler(this.rdoConditionOr_CheckedChanged);
//                this.rdoConditionAnd.CheckedChanged -= new System.EventHandler(this.rdoConditionAnd_CheckedChanged);
//                this.btnEditCondition.Click -= new System.EventHandler(this.btnEditCondition_Click);
//                this.btnAddCondition.Click -= new System.EventHandler(this.btnAddCondition_Click);
//                this.trvConditions.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.trvConditions_KeyUp);
//                this.trvConditions.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.trvConditions_AfterSelect);
//                this.trvConditions.Enter -= new System.EventHandler(this.trvConditions_Enter);
//                this.mniCondDelete.Click -= new System.EventHandler(this.mniCondDelete_Click);
//                this.txtName.TextChanged -= new System.EventHandler(this.txtName_TextChanged);
//                this.Closing -= new System.ComponentModel.CancelEventHandler(this.frmFilterWizard_Closing);
//                this.Load -= new System.EventHandler(this.FilterWizard_Load);
//            }
//            base.Dispose( disposing );
//        }

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFilterWizard));
//            this.btnBack = new System.Windows.Forms.Button();
//            this.btnNext = new System.Windows.Forms.Button();
//            this.btnCancel = new System.Windows.Forms.Button();
//            this.pnlIntro = new MIDRetail.Windows.FilterWizardIntroPanel();
//            this.pictureBox3 = new System.Windows.Forms.PictureBox();
//            this.chkData = new System.Windows.Forms.CheckBox();
//            this.chkSets = new System.Windows.Forms.CheckBox();
//            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
//            this.label2 = new System.Windows.Forms.Label();
//            this.label1 = new System.Windows.Forms.Label();
//            this.pnlAttributeTitle = new System.Windows.Forms.Panel();
//            this.label4 = new System.Windows.Forms.Label();
//            this.label3 = new System.Windows.Forms.Label();
//            this.pnlAttributeMain = new MIDRetail.Windows.FilterWizardAttributePanel();
//            this.pnlAttributeConjunction = new System.Windows.Forms.Panel();
//            this.lblAttributeConjunction = new System.Windows.Forms.Label();
//            this.rdoAttributeOr = new System.Windows.Forms.RadioButton();
//            this.rdoAttributeAnd = new System.Windows.Forms.RadioButton();
//            this.label13 = new System.Windows.Forms.Label();
//            this.label12 = new System.Windows.Forms.Label();
//            this.trvStores = new System.Windows.Forms.TreeView();
//            this.ctmStores = new System.Windows.Forms.ContextMenu();
//            this.mniStoreDelete = new System.Windows.Forms.MenuItem();
//            this.trvAttributes = new System.Windows.Forms.TreeView();
//            this.ctmAttributes = new System.Windows.Forms.ContextMenu();
//            this.mniAttrDelete = new System.Windows.Forms.MenuItem();
//            this.imgAttributes = new System.Windows.Forms.ImageList(this.components);
//            this.pnlDataMain = new MIDRetail.Windows.FilterWizardDataPanel();
//            this.pnlDataConjunction = new System.Windows.Forms.Panel();
//            this.lblConditionConjunction = new System.Windows.Forms.Label();
//            this.rdoConditionOr = new System.Windows.Forms.RadioButton();
//            this.rdoConditionAnd = new System.Windows.Forms.RadioButton();
//            this.pnlDataInfo = new System.Windows.Forms.Panel();
//            this.rtbConditionInfo = new System.Windows.Forms.RichTextBox();
//            this.btnEditCondition = new System.Windows.Forms.Button();
//            this.btnAddCondition = new System.Windows.Forms.Button();
//            this.trvConditions = new System.Windows.Forms.TreeView();
//            this.ctmConditions = new System.Windows.Forms.ContextMenu();
//            this.mniCondDelete = new System.Windows.Forms.MenuItem();
//            this.pnlDataTitle = new System.Windows.Forms.Panel();
//            this.label7 = new System.Windows.Forms.Label();
//            this.label8 = new System.Windows.Forms.Label();
//            this.pnlFinish = new MIDRetail.Windows.FilterWizardFinishPanel();
//            this.pictureBox1 = new System.Windows.Forms.PictureBox();
//            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
//            this.label10 = new System.Windows.Forms.Label();
//            this.pnlNameMain = new MIDRetail.Windows.FilterWizardNamePanel();
//            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
//            this.rdoGlobal = new System.Windows.Forms.RadioButton();
//            this.rdoUser = new System.Windows.Forms.RadioButton();
//            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
//            this.txtName = new System.Windows.Forms.TextBox();
//            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
//            this.pnlNameTitle = new System.Windows.Forms.Panel();
//            this.label9 = new System.Windows.Forms.Label();
//            this.label11 = new System.Windows.Forms.Label();
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
//            this.pnlIntro.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
//            this.pnlAttributeTitle.SuspendLayout();
//            this.pnlAttributeMain.SuspendLayout();
//            this.pnlAttributeConjunction.SuspendLayout();
//            this.pnlDataMain.SuspendLayout();
//            this.pnlDataConjunction.SuspendLayout();
//            this.pnlDataInfo.SuspendLayout();
//            this.pnlDataTitle.SuspendLayout();
//            this.pnlFinish.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
//            this.pnlNameMain.SuspendLayout();
//            this.pnlNameTitle.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // utmMain
//            // 
//            this.utmMain.MenuSettings.ForceSerialization = true;
//            this.utmMain.ToolbarSettings.ForceSerialization = true;
//            // 
//            // btnBack
//            // 
//            this.btnBack.Location = new System.Drawing.Point(360, 344);
//            this.btnBack.Name = "btnBack";
//            this.btnBack.Size = new System.Drawing.Size(72, 24);
//            this.btnBack.TabIndex = 0;
//            this.btnBack.Text = "< Back";
//            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
//            // 
//            // btnNext
//            // 
//            this.btnNext.Location = new System.Drawing.Point(440, 344);
//            this.btnNext.Name = "btnNext";
//            this.btnNext.Size = new System.Drawing.Size(72, 24);
//            this.btnNext.TabIndex = 1;
//            this.btnNext.Text = "Next >";
//            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
//            // 
//            // btnCancel
//            // 
//            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
//            this.btnCancel.Location = new System.Drawing.Point(520, 344);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(72, 24);
//            this.btnCancel.TabIndex = 2;
//            this.btnCancel.Text = "Cancel";
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
//            // 
//            // pnlIntro
//            // 
//            this.pnlIntro.BackColor = System.Drawing.SystemColors.Window;
//            this.pnlIntro.BackPanel = null;
//            this.pnlIntro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.pnlIntro.Controls.Add(this.pictureBox3);
//            this.pnlIntro.Controls.Add(this.chkData);
//            this.pnlIntro.Controls.Add(this.chkSets);
//            this.pnlIntro.Controls.Add(this.richTextBox1);
//            this.pnlIntro.Controls.Add(this.label2);
//            this.pnlIntro.Controls.Add(this.label1);
//            this.pnlIntro.DefaultControl = null;
//            this.pnlIntro.Index = 0;
//            this.pnlIntro.IsBackEnabled = false;
//            this.pnlIntro.IsNextEnabled = false;
//            this.pnlIntro.Location = new System.Drawing.Point(0, 0);
//            this.pnlIntro.Name = "pnlIntro";
//            this.pnlIntro.NextPanel = null;
//            this.pnlIntro.NextText = "Next >";
//            this.pnlIntro.Size = new System.Drawing.Size(600, 336);
//            this.pnlIntro.TabIndex = 3;
//            this.pnlIntro.TitlePanel = null;
//            // 
//            // pictureBox3
//            // 
//            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
//            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
//            this.pictureBox3.Name = "pictureBox3";
//            this.pictureBox3.Size = new System.Drawing.Size(160, 336);
//            this.pictureBox3.TabIndex = 7;
//            this.pictureBox3.TabStop = false;
//            // 
//            // chkData
//            // 
//            this.chkData.Location = new System.Drawing.Point(360, 272);
//            this.chkData.Name = "chkData";
//            this.chkData.Size = new System.Drawing.Size(56, 16);
//            this.chkData.TabIndex = 6;
//            this.chkData.Text = "Data";
//            this.chkData.CheckedChanged += new System.EventHandler(this.chkSetsData_CheckedChanged);
//            // 
//            // chkSets
//            // 
//            this.chkSets.Location = new System.Drawing.Point(248, 272);
//            this.chkSets.Name = "chkSets";
//            this.chkSets.Size = new System.Drawing.Size(96, 16);
//            this.chkSets.TabIndex = 5;
//            this.chkSets.Text = "Attribute Sets";
//            this.chkSets.CheckedChanged += new System.EventHandler(this.chkSetsData_CheckedChanged);
//            // 
//            // richTextBox1
//            // 
//            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
//            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
//            this.richTextBox1.Location = new System.Drawing.Point(176, 144);
//            this.richTextBox1.Name = "richTextBox1";
//            this.richTextBox1.ReadOnly = true;
//            this.richTextBox1.Size = new System.Drawing.Size(408, 120);
//            this.richTextBox1.TabIndex = 3;
//            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
//            // 
//            // label2
//            // 
//            this.label2.Location = new System.Drawing.Point(176, 112);
//            this.label2.Name = "label2";
//            this.label2.Size = new System.Drawing.Size(408, 40);
//            this.label2.TabIndex = 1;
//            this.label2.Text = "This wizard will assist you in defining a Store Filter than can be used in the MI" +
//    "DRetail product.";
//            // 
//            // label1
//            // 
//            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label1.Location = new System.Drawing.Point(176, 24);
//            this.label1.Name = "label1";
//            this.label1.Size = new System.Drawing.Size(256, 56);
//            this.label1.TabIndex = 0;
//            this.label1.Text = "Welcome to the Filter Definition Wizard";
//            // 
//            // pnlAttributeTitle
//            // 
//            this.pnlAttributeTitle.BackColor = System.Drawing.SystemColors.Window;
//            this.pnlAttributeTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.pnlAttributeTitle.Controls.Add(this.label4);
//            this.pnlAttributeTitle.Controls.Add(this.label3);
//            this.pnlAttributeTitle.Location = new System.Drawing.Point(0, 0);
//            this.pnlAttributeTitle.Name = "pnlAttributeTitle";
//            this.pnlAttributeTitle.Size = new System.Drawing.Size(600, 56);
//            this.pnlAttributeTitle.TabIndex = 4;
//            // 
//            // label4
//            // 
//            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label4.Location = new System.Drawing.Point(48, 32);
//            this.label4.Name = "label4";
//            this.label4.Size = new System.Drawing.Size(440, 16);
//            this.label4.TabIndex = 1;
//            this.label4.Text = "Specify Attribute Sets and/or individual Stores to be included in the filter.";
//            // 
//            // label3
//            // 
//            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label3.Location = new System.Drawing.Point(24, 8);
//            this.label3.Name = "label3";
//            this.label3.Size = new System.Drawing.Size(176, 16);
//            this.label3.TabIndex = 0;
//            this.label3.Text = "Attribute Sets and Stores";
//            // 
//            // pnlAttributeMain
//            // 
//            this.pnlAttributeMain.BackColor = System.Drawing.SystemColors.Control;
//            this.pnlAttributeMain.BackPanel = null;
//            this.pnlAttributeMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.pnlAttributeMain.Controls.Add(this.pnlAttributeConjunction);
//            this.pnlAttributeMain.Controls.Add(this.label13);
//            this.pnlAttributeMain.Controls.Add(this.label12);
//            this.pnlAttributeMain.Controls.Add(this.trvStores);
//            this.pnlAttributeMain.Controls.Add(this.trvAttributes);
//            this.pnlAttributeMain.DefaultControl = null;
//            this.pnlAttributeMain.Index = 0;
//            this.pnlAttributeMain.IsBackEnabled = true;
//            this.pnlAttributeMain.IsNextEnabled = true;
//            this.pnlAttributeMain.Location = new System.Drawing.Point(0, 56);
//            this.pnlAttributeMain.Name = "pnlAttributeMain";
//            this.pnlAttributeMain.NextPanel = null;
//            this.pnlAttributeMain.NextText = "Next >";
//            this.pnlAttributeMain.Size = new System.Drawing.Size(600, 280);
//            this.pnlAttributeMain.TabIndex = 5;
//            this.pnlAttributeMain.TitlePanel = this.pnlAttributeTitle;
//            // 
//            // pnlAttributeConjunction
//            // 
//            this.pnlAttributeConjunction.Controls.Add(this.lblAttributeConjunction);
//            this.pnlAttributeConjunction.Controls.Add(this.rdoAttributeOr);
//            this.pnlAttributeConjunction.Controls.Add(this.rdoAttributeAnd);
//            this.pnlAttributeConjunction.Location = new System.Drawing.Point(392, 48);
//            this.pnlAttributeConjunction.Name = "pnlAttributeConjunction";
//            this.pnlAttributeConjunction.Size = new System.Drawing.Size(200, 72);
//            this.pnlAttributeConjunction.TabIndex = 6;
//            // 
//            // lblAttributeConjunction
//            // 
//            this.lblAttributeConjunction.Location = new System.Drawing.Point(8, 8);
//            this.lblAttributeConjunction.Name = "lblAttributeConjunction";
//            this.lblAttributeConjunction.Size = new System.Drawing.Size(184, 32);
//            this.lblAttributeConjunction.TabIndex = 6;
//            // 
//            // rdoAttributeOr
//            // 
//            this.rdoAttributeOr.Checked = true;
//            this.rdoAttributeOr.Location = new System.Drawing.Point(56, 40);
//            this.rdoAttributeOr.Name = "rdoAttributeOr";
//            this.rdoAttributeOr.Size = new System.Drawing.Size(48, 24);
//            this.rdoAttributeOr.TabIndex = 4;
//            this.rdoAttributeOr.TabStop = true;
//            this.rdoAttributeOr.Text = "OR";
//            this.rdoAttributeOr.CheckedChanged += new System.EventHandler(this.rdoAttributeOr_CheckedChanged);
//            // 
//            // rdoAttributeAnd
//            // 
//            this.rdoAttributeAnd.Location = new System.Drawing.Point(104, 40);
//            this.rdoAttributeAnd.Name = "rdoAttributeAnd";
//            this.rdoAttributeAnd.Size = new System.Drawing.Size(48, 24);
//            this.rdoAttributeAnd.TabIndex = 3;
//            this.rdoAttributeAnd.Text = "AND";
//            this.rdoAttributeAnd.CheckedChanged += new System.EventHandler(this.rdoAttributeAnd_CheckedChanged);
//            // 
//            // label13
//            // 
//            this.label13.Location = new System.Drawing.Point(200, 8);
//            this.label13.Name = "label13";
//            this.label13.Size = new System.Drawing.Size(184, 40);
//            this.label13.TabIndex = 8;
//            this.label13.Text = "Drop Stores into this box.  These stores will be included in the filter, regardle" +
//    "ss of what Set they are in.";
//            // 
//            // label12
//            // 
//            this.label12.Location = new System.Drawing.Point(8, 8);
//            this.label12.Name = "label12";
//            this.label12.Size = new System.Drawing.Size(184, 40);
//            this.label12.TabIndex = 7;
//            this.label12.Text = "Drop Attributes and/or Sets into this box.  The stores contained in them will be " +
//    "included in the filter.";
//            // 
//            // trvStores
//            // 
//            this.trvStores.AllowDrop = true;
//            this.trvStores.ContextMenu = this.ctmStores;
//            this.trvStores.Location = new System.Drawing.Point(200, 48);
//            this.trvStores.Name = "trvStores";
//            this.trvStores.ShowLines = false;
//            this.trvStores.ShowPlusMinus = false;
//            this.trvStores.ShowRootLines = false;
//            this.trvStores.Size = new System.Drawing.Size(184, 224);
//            this.trvStores.Sorted = true;
//            this.trvStores.TabIndex = 3;
//            this.trvStores.DragDrop += new System.Windows.Forms.DragEventHandler(this.trvStores_DragDrop);
//            this.trvStores.DragEnter += new System.Windows.Forms.DragEventHandler(this.trvStores_DragEnter);
//            this.trvStores.DragOver += new System.Windows.Forms.DragEventHandler(this.trvStores_DragOver);
//            this.trvStores.DragLeave += new System.EventHandler(this.trvStores_DragLeave);
//            this.trvStores.Enter += new System.EventHandler(this.trvStores_Enter);
//            this.trvStores.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trvStores_KeyUp);
//            // 
//            // ctmStores
//            // 
//            this.ctmStores.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
//            this.mniStoreDelete});
//            // 
//            // mniStoreDelete
//            // 
//            this.mniStoreDelete.Index = 0;
//            this.mniStoreDelete.Text = "Delete";
//            this.mniStoreDelete.Click += new System.EventHandler(this.mniStoreDelete_Click);
//            // 
//            // trvAttributes
//            // 
//            this.trvAttributes.AllowDrop = true;
//            this.trvAttributes.ContextMenu = this.ctmAttributes;
//            this.trvAttributes.HideSelection = false;
//            this.trvAttributes.Location = new System.Drawing.Point(8, 48);
//            this.trvAttributes.Name = "trvAttributes";
//            this.trvAttributes.Size = new System.Drawing.Size(184, 224);
//            this.trvAttributes.TabIndex = 0;
//            this.trvAttributes.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.trvAttributes_AfterCollapse);
//            this.trvAttributes.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.trvAttributes_AfterExpand);
//            this.trvAttributes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvAttributes_AfterSelect);
//            this.trvAttributes.DragDrop += new System.Windows.Forms.DragEventHandler(this.trvAttributes_DragDrop);
//            this.trvAttributes.DragEnter += new System.Windows.Forms.DragEventHandler(this.trvAttributes_DragEnter);
//            this.trvAttributes.DragOver += new System.Windows.Forms.DragEventHandler(this.trvAttributes_DragOver);
//            this.trvAttributes.DragLeave += new System.EventHandler(this.trvAttributes_DragLeave);
//            this.trvAttributes.Enter += new System.EventHandler(this.trvAttributes_Enter);
//            this.trvAttributes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trvAttributes_KeyUp);
//            // 
//            // ctmAttributes
//            // 
//            this.ctmAttributes.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
//            this.mniAttrDelete});
//            // 
//            // mniAttrDelete
//            // 
//            this.mniAttrDelete.Index = 0;
//            this.mniAttrDelete.Text = "Delete";
//            this.mniAttrDelete.Click += new System.EventHandler(this.mniAttrDelete_Click);
//            // 
//            // imgAttributes
//            // 
//            this.imgAttributes.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
//            this.imgAttributes.ImageSize = new System.Drawing.Size(16, 16);
//            this.imgAttributes.TransparentColor = System.Drawing.Color.Transparent;
//            // 
//            // pnlDataMain
//            // 
//            this.pnlDataMain.BackColor = System.Drawing.SystemColors.Control;
//            this.pnlDataMain.BackPanel = null;
//            this.pnlDataMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.pnlDataMain.Controls.Add(this.pnlDataConjunction);
//            this.pnlDataMain.Controls.Add(this.pnlDataInfo);
//            this.pnlDataMain.Controls.Add(this.btnEditCondition);
//            this.pnlDataMain.Controls.Add(this.btnAddCondition);
//            this.pnlDataMain.Controls.Add(this.trvConditions);
//            this.pnlDataMain.DefaultControl = null;
//            this.pnlDataMain.Index = 0;
//            this.pnlDataMain.IsBackEnabled = true;
//            this.pnlDataMain.IsNextEnabled = true;
//            this.pnlDataMain.Location = new System.Drawing.Point(0, 56);
//            this.pnlDataMain.Name = "pnlDataMain";
//            this.pnlDataMain.NextPanel = null;
//            this.pnlDataMain.NextText = "Next >";
//            this.pnlDataMain.Size = new System.Drawing.Size(600, 280);
//            this.pnlDataMain.TabIndex = 6;
//            this.pnlDataMain.TitlePanel = this.pnlDataTitle;
//            // 
//            // pnlDataConjunction
//            // 
//            this.pnlDataConjunction.Controls.Add(this.lblConditionConjunction);
//            this.pnlDataConjunction.Controls.Add(this.rdoConditionOr);
//            this.pnlDataConjunction.Controls.Add(this.rdoConditionAnd);
//            this.pnlDataConjunction.Location = new System.Drawing.Point(216, 200);
//            this.pnlDataConjunction.Name = "pnlDataConjunction";
//            this.pnlDataConjunction.Size = new System.Drawing.Size(376, 72);
//            this.pnlDataConjunction.TabIndex = 10;
//            // 
//            // lblConditionConjunction
//            // 
//            this.lblConditionConjunction.Location = new System.Drawing.Point(8, 8);
//            this.lblConditionConjunction.Name = "lblConditionConjunction";
//            this.lblConditionConjunction.Size = new System.Drawing.Size(176, 32);
//            this.lblConditionConjunction.TabIndex = 6;
//            // 
//            // rdoConditionOr
//            // 
//            this.rdoConditionOr.Checked = true;
//            this.rdoConditionOr.Location = new System.Drawing.Point(48, 40);
//            this.rdoConditionOr.Name = "rdoConditionOr";
//            this.rdoConditionOr.Size = new System.Drawing.Size(48, 24);
//            this.rdoConditionOr.TabIndex = 4;
//            this.rdoConditionOr.TabStop = true;
//            this.rdoConditionOr.Text = "OR";
//            this.rdoConditionOr.CheckedChanged += new System.EventHandler(this.rdoConditionOr_CheckedChanged);
//            // 
//            // rdoConditionAnd
//            // 
//            this.rdoConditionAnd.Location = new System.Drawing.Point(96, 40);
//            this.rdoConditionAnd.Name = "rdoConditionAnd";
//            this.rdoConditionAnd.Size = new System.Drawing.Size(48, 24);
//            this.rdoConditionAnd.TabIndex = 3;
//            this.rdoConditionAnd.Text = "AND";
//            this.rdoConditionAnd.CheckedChanged += new System.EventHandler(this.rdoConditionAnd_CheckedChanged);
//            // 
//            // pnlDataInfo
//            // 
//            this.pnlDataInfo.BackColor = System.Drawing.SystemColors.Control;
//            this.pnlDataInfo.Controls.Add(this.rtbConditionInfo);
//            this.pnlDataInfo.Location = new System.Drawing.Point(216, 8);
//            this.pnlDataInfo.Name = "pnlDataInfo";
//            this.pnlDataInfo.Size = new System.Drawing.Size(376, 184);
//            this.pnlDataInfo.TabIndex = 9;
//            // 
//            // rtbConditionInfo
//            // 
//            this.rtbConditionInfo.BackColor = System.Drawing.SystemColors.Control;
//            this.rtbConditionInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
//            this.rtbConditionInfo.Location = new System.Drawing.Point(8, 8);
//            this.rtbConditionInfo.Name = "rtbConditionInfo";
//            this.rtbConditionInfo.Size = new System.Drawing.Size(360, 168);
//            this.rtbConditionInfo.TabIndex = 8;
//            this.rtbConditionInfo.Text = "";
//            // 
//            // btnEditCondition
//            // 
//            this.btnEditCondition.Location = new System.Drawing.Point(112, 248);
//            this.btnEditCondition.Name = "btnEditCondition";
//            this.btnEditCondition.Size = new System.Drawing.Size(96, 24);
//            this.btnEditCondition.TabIndex = 8;
//            this.btnEditCondition.Text = "Edit Condition";
//            this.btnEditCondition.Click += new System.EventHandler(this.btnEditCondition_Click);
//            // 
//            // btnAddCondition
//            // 
//            this.btnAddCondition.Location = new System.Drawing.Point(8, 248);
//            this.btnAddCondition.Name = "btnAddCondition";
//            this.btnAddCondition.Size = new System.Drawing.Size(96, 24);
//            this.btnAddCondition.TabIndex = 1;
//            this.btnAddCondition.Text = "Add Condition";
//            this.btnAddCondition.Click += new System.EventHandler(this.btnAddCondition_Click);
//            // 
//            // trvConditions
//            // 
//            this.trvConditions.ContextMenu = this.ctmConditions;
//            this.trvConditions.Location = new System.Drawing.Point(8, 8);
//            this.trvConditions.Name = "trvConditions";
//            this.trvConditions.ShowLines = false;
//            this.trvConditions.ShowPlusMinus = false;
//            this.trvConditions.ShowRootLines = false;
//            this.trvConditions.Size = new System.Drawing.Size(200, 232);
//            this.trvConditions.TabIndex = 0;
//            this.trvConditions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvConditions_AfterSelect);
//            this.trvConditions.Enter += new System.EventHandler(this.trvConditions_Enter);
//            this.trvConditions.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trvConditions_KeyUp);
//            // 
//            // ctmConditions
//            // 
//            this.ctmConditions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
//            this.mniCondDelete});
//            // 
//            // mniCondDelete
//            // 
//            this.mniCondDelete.Index = 0;
//            this.mniCondDelete.Text = "Delete";
//            this.mniCondDelete.Click += new System.EventHandler(this.mniCondDelete_Click);
//            // 
//            // pnlDataTitle
//            // 
//            this.pnlDataTitle.BackColor = System.Drawing.SystemColors.Window;
//            this.pnlDataTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.pnlDataTitle.Controls.Add(this.label7);
//            this.pnlDataTitle.Controls.Add(this.label8);
//            this.pnlDataTitle.Location = new System.Drawing.Point(0, 0);
//            this.pnlDataTitle.Name = "pnlDataTitle";
//            this.pnlDataTitle.Size = new System.Drawing.Size(600, 56);
//            this.pnlDataTitle.TabIndex = 7;
//            // 
//            // label7
//            // 
//            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label7.Location = new System.Drawing.Point(48, 32);
//            this.label7.Name = "label7";
//            this.label7.Size = new System.Drawing.Size(416, 16);
//            this.label7.TabIndex = 1;
//            this.label7.Text = "Specify data-level requirements for a store to be included in the filter.";
//            // 
//            // label8
//            // 
//            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label8.Location = new System.Drawing.Point(24, 8);
//            this.label8.Name = "label8";
//            this.label8.Size = new System.Drawing.Size(256, 16);
//            this.label8.TabIndex = 0;
//            this.label8.Text = "Data-Level Requirements";
//            // 
//            // pnlFinish
//            // 
//            this.pnlFinish.BackColor = System.Drawing.SystemColors.Window;
//            this.pnlFinish.BackPanel = null;
//            this.pnlFinish.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.pnlFinish.Controls.Add(this.pictureBox1);
//            this.pnlFinish.Controls.Add(this.richTextBox2);
//            this.pnlFinish.Controls.Add(this.label10);
//            this.pnlFinish.DefaultControl = null;
//            this.pnlFinish.Index = 0;
//            this.pnlFinish.IsBackEnabled = true;
//            this.pnlFinish.IsNextEnabled = true;
//            this.pnlFinish.Location = new System.Drawing.Point(0, 0);
//            this.pnlFinish.Name = "pnlFinish";
//            this.pnlFinish.NextPanel = null;
//            this.pnlFinish.NextText = "Finish";
//            this.pnlFinish.Size = new System.Drawing.Size(600, 336);
//            this.pnlFinish.TabIndex = 8;
//            this.pnlFinish.TitlePanel = null;
//            // 
//            // pictureBox1
//            // 
//            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
//            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
//            this.pictureBox1.Name = "pictureBox1";
//            this.pictureBox1.Size = new System.Drawing.Size(160, 336);
//            this.pictureBox1.TabIndex = 8;
//            this.pictureBox1.TabStop = false;
//            // 
//            // richTextBox2
//            // 
//            this.richTextBox2.BackColor = System.Drawing.SystemColors.Window;
//            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
//            this.richTextBox2.Location = new System.Drawing.Point(176, 88);
//            this.richTextBox2.Name = "richTextBox2";
//            this.richTextBox2.ReadOnly = true;
//            this.richTextBox2.Size = new System.Drawing.Size(408, 56);
//            this.richTextBox2.TabIndex = 3;
//            this.richTextBox2.Text = "You have successfully completed the definition of your Store Filter.\n\n\nYour new f" +
//    "ilter will be ready for use after clicking \"Finish\" to complete.";
//            // 
//            // label10
//            // 
//            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label10.Location = new System.Drawing.Point(176, 24);
//            this.label10.Name = "label10";
//            this.label10.Size = new System.Drawing.Size(256, 32);
//            this.label10.TabIndex = 0;
//            this.label10.Text = "Congratulations!";
//            // 
//            // pnlNameMain
//            // 
//            this.pnlNameMain.BackColor = System.Drawing.SystemColors.Control;
//            this.pnlNameMain.BackPanel = null;
//            this.pnlNameMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.pnlNameMain.Controls.Add(this.richTextBox3);
//            this.pnlNameMain.Controls.Add(this.rdoGlobal);
//            this.pnlNameMain.Controls.Add(this.rdoUser);
//            this.pnlNameMain.Controls.Add(this.richTextBox4);
//            this.pnlNameMain.Controls.Add(this.txtName);
//            this.pnlNameMain.Controls.Add(this.richTextBox5);
//            this.pnlNameMain.DefaultControl = this.txtName;
//            this.pnlNameMain.Index = 0;
//            this.pnlNameMain.IsBackEnabled = true;
//            this.pnlNameMain.IsNextEnabled = false;
//            this.pnlNameMain.Location = new System.Drawing.Point(0, 56);
//            this.pnlNameMain.Name = "pnlNameMain";
//            this.pnlNameMain.NextPanel = null;
//            this.pnlNameMain.NextText = "Next >";
//            this.pnlNameMain.Size = new System.Drawing.Size(600, 280);
//            this.pnlNameMain.TabIndex = 9;
//            this.pnlNameMain.TitlePanel = this.pnlNameTitle;
//            // 
//            // richTextBox3
//            // 
//            this.richTextBox3.BackColor = System.Drawing.SystemColors.Control;
//            this.richTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
//            this.richTextBox3.Location = new System.Drawing.Point(24, 200);
//            this.richTextBox3.Name = "richTextBox3";
//            this.richTextBox3.ReadOnly = true;
//            this.richTextBox3.Size = new System.Drawing.Size(504, 24);
//            this.richTextBox3.TabIndex = 16;
//            this.richTextBox3.Text = "(If either of these selections is not active, you do not have the proper security" +
//    " to define that type of filter).";
//            // 
//            // rdoGlobal
//            // 
//            this.rdoGlobal.Location = new System.Drawing.Point(24, 168);
//            this.rdoGlobal.Name = "rdoGlobal";
//            this.rdoGlobal.Size = new System.Drawing.Size(80, 16);
//            this.rdoGlobal.TabIndex = 15;
//            this.rdoGlobal.Text = "Global";
//            // 
//            // rdoUser
//            // 
//            this.rdoUser.Checked = true;
//            this.rdoUser.Location = new System.Drawing.Point(24, 144);
//            this.rdoUser.Name = "rdoUser";
//            this.rdoUser.Size = new System.Drawing.Size(80, 16);
//            this.rdoUser.TabIndex = 14;
//            this.rdoUser.TabStop = true;
//            this.rdoUser.Text = "User";
//            // 
//            // richTextBox4
//            // 
//            this.richTextBox4.BackColor = System.Drawing.SystemColors.Control;
//            this.richTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
//            this.richTextBox4.Location = new System.Drawing.Point(24, 96);
//            this.richTextBox4.Name = "richTextBox4";
//            this.richTextBox4.ReadOnly = true;
//            this.richTextBox4.Size = new System.Drawing.Size(560, 40);
//            this.richTextBox4.TabIndex = 13;
//            this.richTextBox4.Text = "You can choose who can access your new filter.  If you select \"User\", only you wi" +
//    "ll be allowed to access it.  By seleting \"Global\", all users will have access to" +
//    " it.";
//            // 
//            // txtName
//            // 
//            this.txtName.Location = new System.Drawing.Point(24, 56);
//            this.txtName.Name = "txtName";
//            this.txtName.Size = new System.Drawing.Size(232, 20);
//            this.txtName.TabIndex = 0;
//            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
//            // 
//            // richTextBox5
//            // 
//            this.richTextBox5.BackColor = System.Drawing.SystemColors.Control;
//            this.richTextBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
//            this.richTextBox5.Location = new System.Drawing.Point(24, 24);
//            this.richTextBox5.Name = "richTextBox5";
//            this.richTextBox5.ReadOnly = true;
//            this.richTextBox5.Size = new System.Drawing.Size(408, 21);
//            this.richTextBox5.TabIndex = 11;
//            this.richTextBox5.Text = "Please choose a name for your filter:";
//            // 
//            // pnlNameTitle
//            // 
//            this.pnlNameTitle.BackColor = System.Drawing.SystemColors.Window;
//            this.pnlNameTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.pnlNameTitle.Controls.Add(this.label9);
//            this.pnlNameTitle.Controls.Add(this.label11);
//            this.pnlNameTitle.Location = new System.Drawing.Point(0, 0);
//            this.pnlNameTitle.Name = "pnlNameTitle";
//            this.pnlNameTitle.Size = new System.Drawing.Size(600, 56);
//            this.pnlNameTitle.TabIndex = 10;
//            // 
//            // label9
//            // 
//            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label9.Location = new System.Drawing.Point(48, 32);
//            this.label9.Name = "label9";
//            this.label9.Size = new System.Drawing.Size(416, 16);
//            this.label9.TabIndex = 1;
//            this.label9.Text = "Choose the name and access-level of your filter.";
//            // 
//            // label11
//            // 
//            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label11.Location = new System.Drawing.Point(24, 8);
//            this.label11.Name = "label11";
//            this.label11.Size = new System.Drawing.Size(256, 16);
//            this.label11.TabIndex = 0;
//            this.label11.Text = "Filter Name and Access Level";
//            // 
//            // frmFilterWizard
//            // 
//            this.AcceptButton = this.btnNext;
//            this.AllowDragDrop = true;
//            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//            this.BypassBaseClosingLogic = true;
//            this.CancelButton = this.btnCancel;
//            this.ClientSize = new System.Drawing.Size(600, 374);
//            this.Controls.Add(this.pnlAttributeMain);
//            this.Controls.Add(this.pnlDataMain);
//            this.Controls.Add(this.pnlDataTitle);
//            this.Controls.Add(this.pnlNameTitle);
//            this.Controls.Add(this.pnlAttributeTitle);
//            this.Controls.Add(this.btnCancel);
//            this.Controls.Add(this.btnNext);
//            this.Controls.Add(this.btnBack);
//            this.Controls.Add(this.pnlNameMain);
//            this.Controls.Add(this.pnlIntro);
//            this.Controls.Add(this.pnlFinish);
//            this.Name = "frmFilterWizard";
//            this.Text = "Filter Wizard";
//            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmFilterWizard_Closing);
//            this.Load += new System.EventHandler(this.FilterWizard_Load);
//            this.Controls.SetChildIndex(this.pnlFinish, 0);
//            this.Controls.SetChildIndex(this.pnlIntro, 0);
//            this.Controls.SetChildIndex(this.pnlNameMain, 0);
//            this.Controls.SetChildIndex(this.btnBack, 0);
//            this.Controls.SetChildIndex(this.btnNext, 0);
//            this.Controls.SetChildIndex(this.btnCancel, 0);
//            this.Controls.SetChildIndex(this.pnlAttributeTitle, 0);
//            this.Controls.SetChildIndex(this.pnlNameTitle, 0);
//            this.Controls.SetChildIndex(this.pnlDataTitle, 0);
//            this.Controls.SetChildIndex(this.pnlDataMain, 0);
//            this.Controls.SetChildIndex(this.pnlAttributeMain, 0);
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
//            this.pnlIntro.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
//            this.pnlAttributeTitle.ResumeLayout(false);
//            this.pnlAttributeMain.ResumeLayout(false);
//            this.pnlAttributeConjunction.ResumeLayout(false);
//            this.pnlDataMain.ResumeLayout(false);
//            this.pnlDataConjunction.ResumeLayout(false);
//            this.pnlDataInfo.ResumeLayout(false);
//            this.pnlDataTitle.ResumeLayout(false);
//            this.pnlFinish.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
//            this.pnlNameMain.ResumeLayout(false);
//            this.pnlNameMain.PerformLayout();
//            this.pnlNameTitle.ResumeLayout(false);
//            this.ResumeLayout(false);

//        }
//        #endregion

//        private const int cClosedFolderImage = 0;
//        private const int cOpenFolderImage = 1;
//        private const int cDynamicStoreImage = 2;
//        private const int cSelectedStoreImage = 3;
//        private const int cStaticStoreImage = 4;
//        private const int cTopClosedFolderImage = 5;
//        private const int cTopOpenFolderImage = 6;
//        private const int cClosedDynamicFolderImage = 7;
//        private const int cOpenDynamicFolderImage = 8;

//        private SessionAddressBlock _SAB;
//        private ExplorerAddressBlock _EAB;
//        private FunctionSecurityProfile _filterUserSecurity;
//        private FunctionSecurityProfile _filterGlobalSecurity;
////Begin Track #4052 - JScott - Filters not being enqueued
//        private GenericEnqueue _filterEnqueue;
////End Track #4052 - JScott - Filters not being enqueued
//        private bool _nextClicked;
//        private bool _filterSaved;
////		private bool _bindingCombos;
//        //private StoreFilterData _dlFilter;
//        private FilterData _dlFilter;
//        private FolderDataLayer _dlFolder;
//        private ArrayList _compareToList;
//        private ArrayList _timeModVar1List;
//        private ArrayList _timeModVar2List;
//        private ArrayList _cubeModList;
//        private ArrayList _variableList;
//        private ArrayList _versionList;
//        private ArrayList _comparisonList;
//        private ArrayList _statusList;
//        private FilterWizardPanel _currPanel;
//        private FilterWizardPanel _firstPanel;
//        private FilterWizardPanel _var1PanelToAdd;
//        private FilterWizardVar1 _currVar1Control;
//        private int _filterKey;
//        private TreeNode _currNode;
////		private Color _holdNodeBackColor;
        
//        private void FilterWizard_Load(object sender, System.EventArgs e)
//        {
//            int i;
//            ProfileList variableProfList;
//            ProfileList timeTotalVariableProfList;
//            ProfileList versionProfList;
//            DataTable dtStatus;

//            try
//            {
//                FunctionSecurity = new FunctionSecurityProfile(Convert.ToInt32(eSecurityFunctions.ToolsFilters));
//                FunctionSecurity.SetFullControl();
////				AllowUpdate = true;   Security changes 1/24/05 vg
//                SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg

//                imgAttributes.Images.Add(Image.FromFile(GraphicsDirectory + "\\yellow.closedfolder.gif"));
//                imgAttributes.Images.Add(Image.FromFile(GraphicsDirectory + "\\yellow.openfolder.gif"));
//                imgAttributes.Images.Add(Image.FromFile(GraphicsDirectory + "\\store_dynamic.gif"));
//                imgAttributes.Images.Add(Image.FromFile(GraphicsDirectory + "\\store_selected.gif"));
//                imgAttributes.Images.Add(Image.FromFile(GraphicsDirectory + "\\store_static.gif"));
//                imgAttributes.Images.Add(Image.FromFile(GraphicsDirectory + "\\default.closedfolder.gif"));
//                imgAttributes.Images.Add(Image.FromFile(GraphicsDirectory + "\\default.openfolder.gif"));
//                imgAttributes.Images.Add(Image.FromFile(GraphicsDirectory + "\\yellow.closedfolderdynamic.gif"));
//                imgAttributes.Images.Add(Image.FromFile(GraphicsDirectory + "\\yellow.openfolderdynamic.gif"));

//                _dlFilter = new StoreFilterData();
//                _dlFolder = new FolderDataLayer();
//                _versionList = new ArrayList();

//                _filterUserSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersUser);
//                _filterGlobalSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersGlobal);

//                if (_filterUserSecurity.AllowUpdate)
//                {
//                    this.rdoUser.Enabled = true;
//                    this.rdoUser.Checked = true;
//                }
//                else
//                {
//                    this.rdoUser.Enabled = false;
//                    this.rdoGlobal.Checked = true;
//                }

//                if (_filterGlobalSecurity.AllowUpdate)
//                {
//                    this.rdoGlobal.Enabled = true;
//                }
//                else
//                {
//                    this.rdoGlobal.Enabled = false;
//                }
				
//                versionProfList = _SAB.ClientServerSession.GetUserForecastVersions();

//                foreach (VersionProfile versionProf in versionProfList)
//                {
//                    _versionList.Add(new ProfileComboObject(versionProf.Key, versionProf.Description, versionProf));
//                }

//                _variableList = new ArrayList();

//                variableProfList = _SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;
//                timeTotalVariableProfList = _SAB.ApplicationServerSession.DefaultPlanComputations.PlanTimeTotalVariables.TimeTotalVariableProfileList;

//                foreach (VariableProfile varProf in variableProfList)
//                {
//                    _variableList.Add(new ProfileComboObject(varProf.Key, varProf.VariableName, varProf));
//                }

//                foreach (VariableProfile varProf in variableProfList)
//                {
//                    for (i = 0; i < varProf.TimeTotalVariables.Count; i++)
//                    {
//                        _variableList.Add(new TimeTotalComboObject(((TimeTotalVariableProfile)varProf.TimeTotalVariables[i]).Key, "[Date Total] " + ((TimeTotalVariableProfile)varProf.TimeTotalVariables[i]).VariableName, (Profile)varProf.TimeTotalVariables[i], varProf, i + 1));
//                    }
//                }

//                _compareToList = new ArrayList();
//                _compareToList.Add(new ComboObject((int)eFilterCompareTo.Constant, "A Constant"));
//                _compareToList.Add(new ComboObject((int)eFilterCompareTo.Variable, "A Variable"));
//                _compareToList.Add(new ComboObject((int)eFilterCompareTo.PctOf, "% Of a Variable"));
//                _compareToList.Add(new ComboObject((int)eFilterCompareTo.PctChange, "% Change of a Variable"));

//                _timeModVar1List = new ArrayList();
//                _timeModVar1List.Add(new ComboObject((int)eFilterTimeModifyer.Any, "Any"));
//                _timeModVar1List.Add(new ComboObject((int)eFilterTimeModifyer.All, "All"));
////Begin Track #5111 - JScott - Add additional filter functionality
//                _timeModVar1List.Add(new ComboObject((int)eFilterTimeModifyer.Join, "Join"));
////End Track #5111 - JScott - Add additional filter functionality
//                _timeModVar1List.Add(new ComboObject((int)eFilterTimeModifyer.Average, "Average"));
//                _timeModVar1List.Add(new ComboObject((int)eFilterTimeModifyer.Total, "Total"));

//                _timeModVar2List = new ArrayList();
//                _timeModVar2List.Add(new ComboObject((int)eFilterTimeModifyer.Any, "Any"));
//                _timeModVar2List.Add(new ComboObject((int)eFilterTimeModifyer.All, "All"));
////Begin Track #5111 - JScott - Add additional filter functionality
//                _timeModVar2List.Add(new ComboObject((int)eFilterTimeModifyer.Join, "Join"));
////End Track #5111 - JScott - Add additional filter functionality
//                _timeModVar2List.Add(new ComboObject((int)eFilterTimeModifyer.Average, "Average"));
//                _timeModVar2List.Add(new ComboObject((int)eFilterTimeModifyer.Total, "Total"));
//                _timeModVar2List.Add(new ComboObject((int)eFilterTimeModifyer.Corresponding, "Corresponding"));

//                _cubeModList = new ArrayList();
//                _cubeModList.Add(new ComboObject((int)eFilterCubeModifyer.StoreDetail, "Store Detail"));
//                _cubeModList.Add(new ComboObject((int)eFilterCubeModifyer.StoreTotal, "Store Total"));
//                _cubeModList.Add(new ComboObject((int)eFilterCubeModifyer.StoreAverage, "Store Average"));
//                _cubeModList.Add(new ComboObject((int)eFilterCubeModifyer.ChainDetail, "Chain Detail"));

//                _comparisonList = new ArrayList();
//                _comparisonList.Add(new ComparisonComboObject((int)eFilterComparisonType.Equal, "=", typeof(DataQueryEqualOperand), false));
//                _comparisonList.Add(new ComparisonComboObject((int)eFilterComparisonType.Greater, ">", typeof(DataQueryGreaterOperand), false));
//                _comparisonList.Add(new ComparisonComboObject((int)eFilterComparisonType.Less, "<", typeof(DataQueryLessOperand), false));
//                _comparisonList.Add(new ComparisonComboObject((int)eFilterComparisonType.GreaterEqual, ">=", typeof(DataQueryGreaterEqualOperand), false));
//                _comparisonList.Add(new ComparisonComboObject((int)eFilterComparisonType.LessEqual, "<=", typeof(DataQueryLessEqualOperand), false));
//                _comparisonList.Add(new ComparisonComboObject((int)eFilterComparisonType.NotEqual, "NOT =", typeof(DataQueryEqualOperand), true));
//                _comparisonList.Add(new ComparisonComboObject((int)eFilterComparisonType.NotGreater, "NOT >", typeof(DataQueryGreaterOperand), true));
//                _comparisonList.Add(new ComparisonComboObject((int)eFilterComparisonType.NotLess, "NOT <", typeof(DataQueryLessOperand), true));
//                _comparisonList.Add(new ComparisonComboObject((int)eFilterComparisonType.NotGreaterEqual, "NOT >=", typeof(DataQueryGreaterEqualOperand), true));
//                _comparisonList.Add(new ComparisonComboObject((int)eFilterComparisonType.NotLessEqual, "NOT <=", typeof(DataQueryLessEqualOperand), true));

//                dtStatus = MIDText.GetTextType(eMIDTextType.eStoreStatus, eMIDTextOrderBy.TextCode);

//                _statusList = new ArrayList();
//                foreach (DataRow row in dtStatus.Rows)
//                {
//                    _statusList.Add(new ComboObject(Convert.ToInt32(row["TEXT_CODE"]), Convert.ToString(row["TEXT_VALUE"])));
//                }

//                trvAttributes.ImageList = imgAttributes;
//                trvStores.ImageList = imgAttributes;
//                pnlAttributeConjunction.Visible = false;
//                pnlDataInfo.Visible = false;
//                pnlDataConjunction.Visible = false;

//                _firstPanel = pnlIntro;
//                _currPanel = _firstPanel;

//                _nextClicked = false;
//                _filterSaved = false;

//                ShowCurrentPanel();

//                FormLoaded = true;
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        public Button NextButton
//        {
//            get
//            {
//                return btnNext;
//            }
//        }

//        public void HandleExceptions(Exception exc)
//        {
//            Debug.WriteLine(exc.ToString());
//            MessageBox.Show(exc.ToString());
//        }

//        private void ShowCurrentPanel()
//        {
//            try
//            {
//                // allow image drag on certain panels
//                if (typeof(MIDRetail.Windows.FilterWizardConditionVar1Panel) == _currPanel.GetType() ||
//                    typeof(MIDRetail.Windows.FilterWizardConditionVar2Panel) == _currPanel.GetType() ||
//                    typeof(MIDRetail.Windows.FilterWizardConditionPercentPanel) == _currPanel.GetType())
//                {
//                    AllowDragDrop = true;
//                }
//                else
//                {
//                    AllowDragDrop = false;
//                }

//                btnNext.Text = _currPanel.NextText;
//                btnNext.Enabled = _currPanel.IsNextEnabled;
//                btnBack.Enabled = _currPanel.IsBackEnabled;
                
//                if (_currPanel.TitlePanel != null)
//                {
//                    _currPanel.TitlePanel.BringToFront();
//                }

//                if (_currPanel.DefaultControl != null)
//                {
//                    _currPanel.DefaultControl.Focus();
//                }

//                _currPanel.BringToFront();

//                // BEGIN MID Track #4347 - change condition verbage	
//                if (_currPanel.GetType() == typeof(FilterWizardDataPanel)  
//                 && trvConditions.SelectedNode != null)
//                {
//                    DisplayCurrentConditionNodeInfo();
//                }
//                // END MID Track #4347
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void frmFilterWizard_Closing(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            try
//            {
//                if (_nextClicked && !_filterSaved)
//                {
//                    if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FilterNotSaved), Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
//                    {
//                        e.Cancel = true;
//                        return;
//                    }
//                }
////Begin Track #4052 - JScott - Filters not being enqueued

//                if (_filterEnqueue != null)
//                {
//                    if (!_filterEnqueue.IsInConflict)
//                    {
//                        _filterEnqueue.DequeueGeneric();
//                    }
//                }
////End Track #4052 - JScott - Filters not being enqueued
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void btnCancel_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                this.Close();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void btnNext_Click(object sender, System.EventArgs e)
//        {
//            FilterWizardPanel panel;
//            FilterWizardQuantity quantityControl;
//            FilterWizardGrade gradeControl;
//            FilterWizardStatus statusControl;
//            FilterWizardPercent percentControl;
//            FilterWizardVar2 var2Control;
//            TreeNode treeNode;
//            ComputationVariableProfile varProf;
////Begin Track #4052 - JScott - Filters not being enqueued
//            string errMsg;
////End Track #4052 - JScott - Filters not being enqueued

//            try
//            {
//                _nextClicked = true;

//                if (_currPanel.GetType() == typeof(FilterWizardIntroPanel))
//                {
//                    pnlIntro.NextPanel = pnlNameMain;
//                    pnlNameMain.BackPanel = pnlIntro;
//                    panel = pnlNameMain;

//                    if (chkSets.Checked)
//                    {
//                        panel.NextPanel = pnlAttributeMain;
//                        pnlAttributeMain.BackPanel = panel;
//                        panel = pnlAttributeMain;
//                    }

//                    if (chkData.Checked)
//                    {
//                        panel.NextPanel = pnlDataMain;
//                        pnlDataMain.BackPanel = panel;
//                        panel = pnlDataMain;
//                    }

//                    panel.NextPanel = pnlFinish;
//                    pnlFinish.BackPanel = panel;
//                }
//                else if (_currPanel.GetType() == typeof(FilterWizardNamePanel))
//                {
//                    _filterKey = _dlFilter.FilterGetKey(filterTypes.StoreFilter, _SAB.ClientServerSession.UserRID, txtName.Text);

//                    if (_filterKey != -1)
//                    {
////Begin Track #4052 - JScott - Filters not being enqueued
//                        _filterEnqueue = new GenericEnqueue(eLockType.Filter, _filterKey, _SAB.ClientServerSession.UserRID, _SAB.ClientServerSession.ThreadID);

//                        try
//                        {
//                            _filterEnqueue.EnqueueGeneric();
//                        }
//                        catch (GenericConflictException)
//                        {
//                            /* Begin TT#1159 - Improve Messaging */
//                            string[] errParms = new string[3];
//                            errParms.SetValue("Filter", 0);
//                            errParms.SetValue(txtName.Text.Trim(), 1);
//                            errParms.SetValue(((GenericConflict)_filterEnqueue.ConflictList[0]).UserName, 2);
//                            errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);
                           
//                            //errMsg = "The Filter " + txtName.Text + " is in use by User " + ((GenericConflict)_filterEnqueue.ConflictList[0]).UserName + ".";
//                            /* End TT#1159 - Improve Messaging */

//                            MessageBox.Show(errMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
//                            _filterEnqueue = null;
//                            return;
//                        }
////End Track #4052 - JScott - Filters not being enqueued

//                        if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                        {
//                            return;
//                        }
//                    }
//                }
//                else if (_currPanel.GetType() == typeof(FilterWizardConditionVar1Panel))
//                {
//                    switch ((eFilterCompareTo)((ComboObject)((FilterWizardConditionVar1Panel)_currPanel).ParentControl.CompareToCombo.SelectedItem).Key)
//                    {
//                        case eFilterCompareTo.Constant:

//                            varProf = (ComputationVariableProfile)((ProfileComboObject)_currVar1Control.VariableCombo.SelectedItem).Profile;

//                            switch (varProf.FormatType)
//                            {
//                                case eValueFormatType.StoreGrade :
//                                    if (_currPanel.NextPanel == null || _currPanel.NextPanel.GetType() != typeof(FilterWizardConditionGradePanel))
//                                    {
//                                        gradeControl = (FilterWizardGrade)CreateControl(typeof(FilterWizardGrade), new object[] { this, trvConditions.Nodes.Count + 1 } );
//                                        gradeControl.NextButtonStatusChanged += new FilterWizardGrade.NextButtonStatusEventHandler(OnNextButtonStatusChange);
//                                        gradeControl.MainPanel.BackPanel = _currPanel;
//                                        gradeControl.MainPanel.NextPanel = pnlDataMain;
//                                        _currPanel.NextPanel = gradeControl.MainPanel;
//                                        SetGradeComboBoxes(gradeControl);
//                                    }
//                                    break;
//                                case eValueFormatType.StoreStatus :
//                                    if (_currPanel.NextPanel == null || _currPanel.NextPanel.GetType() != typeof(FilterWizardConditionStatusPanel))
//                                    {
//                                        statusControl = (FilterWizardStatus)CreateControl(typeof(FilterWizardStatus), new object[] { this, trvConditions.Nodes.Count + 1 } );
//                                        statusControl.NextButtonStatusChanged += new FilterWizardStatus.NextButtonStatusEventHandler(OnNextButtonStatusChange);
//                                        statusControl.MainPanel.BackPanel = _currPanel;
//                                        statusControl.MainPanel.NextPanel = pnlDataMain;
//                                        _currPanel.NextPanel = statusControl.MainPanel;
//                                        SetStatusComboBoxes(statusControl);
//                                    }
//                                    break;
//                                case eValueFormatType.GenericNumeric :
//                                    if (_currPanel.NextPanel == null || _currPanel.NextPanel.GetType() != typeof(FilterWizardConditionQuantityPanel))
//                                    {
//                                        quantityControl = (FilterWizardQuantity)CreateControl(typeof(FilterWizardQuantity), new object[] { this, trvConditions.Nodes.Count + 1 } );
//                                        quantityControl.NextButtonStatusChanged += new FilterWizardQuantity.NextButtonStatusEventHandler(OnNextButtonStatusChange);
//                                        quantityControl.MainPanel.BackPanel = _currPanel;
//                                        quantityControl.MainPanel.NextPanel = pnlDataMain;
//                                        _currPanel.NextPanel = quantityControl.MainPanel;
//                                        SetQuantityComboBoxes(quantityControl);
//                                    }
//                                    break;
//                                default:
//                                    break;
//                            }

//                            break;

//                        case eFilterCompareTo.PctOf:
//                        case eFilterCompareTo.PctChange:

//                            if (_currPanel.NextPanel == null || _currPanel.NextPanel.GetType() != typeof(FilterWizardConditionPercentPanel))
//                            {
//                                percentControl = (FilterWizardPercent)CreateControl(typeof(FilterWizardPercent), new object[] { this, _SAB, trvConditions.Nodes.Count + 1 } );
//                                percentControl.NextButtonStatusChanged += new FilterWizardPercent.NextButtonStatusEventHandler(OnNextButtonStatusChange);
//                                percentControl.MainPanel.BackPanel = _currPanel;
//                                percentControl.MainPanel.NextPanel = pnlDataMain;
//                                _currPanel.NextPanel = percentControl.MainPanel;
//                                SetPercentComboBoxes(percentControl);
//                            }

//                            break;

//                        case eFilterCompareTo.Variable:

//                            if (_currPanel.NextPanel == null || _currPanel.NextPanel.GetType() != typeof(FilterWizardConditionVar2Panel))
//                            {
//                                var2Control = (FilterWizardVar2)CreateControl(typeof(FilterWizardVar2), new object[] { this, _SAB, trvConditions.Nodes.Count + 1 } );
//                                var2Control.NextButtonStatusChanged += new FilterWizardVar2.NextButtonStatusEventHandler(OnNextButtonStatusChange);
//                                var2Control.MainPanel.BackPanel = _currPanel;
//                                var2Control.MainPanel.NextPanel = pnlDataMain;
//                                _currPanel.NextPanel = var2Control.MainPanel;
//                                SetVar2ComboBoxes(var2Control);
//                            }

//                            break;

//                    }
//                }
//                else if (_currPanel.GetType() == typeof(FilterWizardConditionQuantityPanel) ||
//                    _currPanel.GetType() == typeof(FilterWizardConditionGradePanel) || 
//                    _currPanel.GetType() == typeof(FilterWizardConditionStatusPanel) || 
//                    _currPanel.GetType() == typeof(FilterWizardConditionPercentPanel) || 
//                    _currPanel.GetType() == typeof(FilterWizardConditionVar2Panel))
//                {
//                    if (_var1PanelToAdd != null)
//                    {
//                        treeNode = new TreeNode();
//                        treeNode.Text = "Condition " + (trvConditions.Nodes.Count + 1);
//                        treeNode.Tag = new ConditionNodeTag(_var1PanelToAdd);

//                        trvConditions.Nodes.Add(treeNode);
//                    }
//                }

//                if (_currPanel.NextPanel == null)
//                {
//                    SaveFilter();

//                    //BEGIN TT#406-MD -jsobek -The User's Filter Explorer is not refreshed when a new Filter is added through the Filter Wizard
//                    if (_EAB != null && _EAB.FilterExplorer != null)
//                    {
//                        _EAB.FilterExplorer.RefreshData(); 
//                    }
//                    //END TT#406-MD -jsobek -The User's Filter Explorer is not refreshed when a new Filter is added through the Filter Wizard
//                    this.Close();
//                }
//                else
//                {
//                    _currPanel = _currPanel.NextPanel;
//                    ShowCurrentPanel();
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void btnBack_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                _currPanel = _currPanel.BackPanel;
//                ShowCurrentPanel();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void btnAddCondition_Click(object sender, System.EventArgs e)
//        {
//            FilterWizardVar1 var1Control;

//            try
//            {
//                var1Control = (FilterWizardVar1)CreateControl(typeof(FilterWizardVar1), new object[] { this, _SAB, trvConditions.Nodes.Count + 1 } );
//                var1Control.NextButtonStatusChanged += new FilterWizardVar1.NextButtonStatusEventHandler(OnNextButtonStatusChange);
//                var1Control.MainPanel.BackPanel = pnlDataMain;
//                SetVar1ComboBoxes(var1Control);

//                _var1PanelToAdd = var1Control.MainPanel;
//                _currPanel = var1Control.MainPanel;
//                _currVar1Control = var1Control;
//                ShowCurrentPanel();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }
		
//        private void btnEditCondition_Click(object sender, System.EventArgs e)
//        {
//            ConditionNodeTag nodeTag;

//            try
//            {
//                if (trvConditions.SelectedNode != null)
//                {
//                    nodeTag = (ConditionNodeTag)trvConditions.SelectedNode.Tag;

//                    _var1PanelToAdd = null;
//                    _currPanel = nodeTag.ConditionPanel;
//                    _currVar1Control = ((FilterWizardConditionVar1Panel)nodeTag.ConditionPanel).ParentControl;
//                    ShowCurrentPanel();
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void chkSetsData_CheckedChanged(object sender, System.EventArgs e)
//        {
//            try
//            {
//                SetIntroNextButton();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void txtName_TextChanged(object sender, System.EventArgs e)
//        {
//            try
//            {
//                SetNameNextButton();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void rdoAttributeOr_CheckedChanged(object sender, System.EventArgs e)
//        {
//            MIDStoreNode storeNode;
//            AttributeNodeTag attrNodeTag;

//            try
//            {
//                storeNode = GetParentNode((MIDStoreNode)trvAttributes.SelectedNode);
//                attrNodeTag = (AttributeNodeTag)storeNode.Tag;
//                attrNodeTag.ConjunctionType = typeof(GenericQueryOrOperand);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void rdoAttributeAnd_CheckedChanged(object sender, System.EventArgs e)
//        {
//            MIDStoreNode storeNode;
//            AttributeNodeTag attrNodeTag;

//            try
//            {
//                storeNode = GetParentNode((MIDStoreNode)trvAttributes.SelectedNode);
//                attrNodeTag = (AttributeNodeTag)storeNode.Tag;
//                attrNodeTag.ConjunctionType = typeof(GenericQueryAndOperand);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void rdoConditionOr_CheckedChanged(object sender, System.EventArgs e)
//        {
//            ConditionNodeTag condNodeTag;

//            try
//            {
//                condNodeTag = (ConditionNodeTag)trvConditions.SelectedNode.Tag;
//                condNodeTag.ConjunctionType = typeof(GenericQueryOrOperand);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void rdoConditionAnd_CheckedChanged(object sender, System.EventArgs e)
//        {
//            ConditionNodeTag condNodeTag;

//            try
//            {
//                condNodeTag = (ConditionNodeTag)trvConditions.SelectedNode.Tag;
//                condNodeTag.ConjunctionType = typeof(GenericQueryAndOperand);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvAttributes_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
//        {
//            try
//            {
//                DisplayCurrentAttributesNodeInfo();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvAttributes_Enter(object sender, System.EventArgs e)
//        {
//            try
//            {
//                DisplayCurrentAttributesNodeInfo();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvAttributes_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
//        {
//            try
//            {
//                SetOpenImage((MIDStoreNode)e.Node);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvAttributes_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
//        {
//            try
//            {
//                SetClosedImage((MIDStoreNode)e.Node);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvAttributes_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            if (e.KeyCode == Keys.Delete)
//            {
//                DeleteCurrentNode(trvAttributes);
//            }
//        }

//        private void trvAttributes_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
//        {
//            //MIDStoreNode storeNode;
//            TreeNodeClipboardList cbList;

//            try
//            {
//                Image_DragEnter(sender, e);
//                //if (e.Data.GetDataPresent(typeof(MIDStoreNode)))
//                //{
//                //    storeNode = (MIDStoreNode)e.Data.GetData(typeof(MIDStoreNode));

//                //    switch(storeNode.StoreNodeType)
//                //    {
//                //        case eStoreNodeType.group:
//                //        case eStoreNodeType.groupLevel:

//                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
//                {
//                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
//                    switch (cbList.ClipboardDataType)
//                    {
//                        //case eClipboardDataType.Attribute:
//                        //case eClipboardDataType.AttributeSet:
//                        case eProfileType.StoreGroup:
//                        case eProfileType.StoreGroupLevel:
//                            e.Effect = e.AllowedEffect;
//                            break;

//                        default:

//                            e.Effect = DragDropEffects.None;
//                            break;
//                    }
//                }
//                else
//                {
//                    e.Effect = DragDropEffects.None;
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvAttributes_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
//        {
//            Point currPoint;

//            try
//            {
//                Image_DragOver(sender, e);
//                currPoint = trvAttributes.PointToClient(new Point(e.X, e.Y));
//                _currNode = GetParentNode((MIDStoreNode)trvAttributes.GetNodeAt(currPoint.X, currPoint.Y));
//                trvAttributes.SelectedNode = _currNode;
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvAttributes_DragLeave(object sender, EventArgs e)
//        {
//            try
//            {
//                Image_DragLeave(sender, e);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvStores_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
//        {
//            TreeNodeClipboardList cbList = null;

//            try
//            {
//                Image_DragEnter(sender, e);
//                //if (e.Data.GetDataPresent(typeof(MIDStoreNode)))
//                //{
//                //    storeNode = (MIDStoreNode)e.Data.GetData(typeof(MIDStoreNode));
//                        //switch(storeNode.StoreNodeType)
//                        //    {
//                        //        case eStoreNodeType.store:
//                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
//                {
//                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
//                    switch (cbList.ClipboardDataType)
//                    {
//                        //case eClipboardDataType.Store:
//                        case eProfileType.Store:

//                            e.Effect = e.AllowedEffect;
//                            break;

//                        default:

//                            e.Effect = DragDropEffects.None;
//                            break;
//                    }
//                }
//                else
//                {
//                    e.Effect = DragDropEffects.None;
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvStores_DragOver(object sender, DragEventArgs e)
//        {
//            try
//            {
//                Image_DragOver(sender, e);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvStores_DragLeave(object sender, EventArgs e)
//        {
//            try
//            {
//                Image_DragLeave(sender, e);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvAttributes_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
//        {
//            MIDStoreNode storeNode;
//            StoreGroupProfile storeGroupProf;
//            StoreGroupLevelProfile storeGroupLevelProf;
//            MIDStoreNode parent;
//            int parentIdx;
//            TreeNodeClipboardList cbList = null;

//            try
//            {
//                //if (e.Data.GetDataPresent(typeof(MIDStoreNode)))
//                //{
//                //    storeNode = (MIDStoreNode)e.Data.GetData(typeof(MIDStoreNode));
//                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
//                {
//                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
//                    //if (cbp.ClipboardDataType == eClipboardDataType.Attribute ||
//                    //    cbp.ClipboardDataType == eClipboardDataType.AttributeSet)
//                    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                    //if (cbList.ClipboardDataType == eProfileType.StoreGroupListView ||
//                    //    cbList.ClipboardDataType == eProfileType.StoreGroupLevelListView)
//                    if (cbList.ClipboardDataType == eProfileType.StoreGroup ||
//                        cbList.ClipboardDataType == eProfileType.StoreGroupListView ||
//                        cbList.ClipboardDataType == eProfileType.StoreGroupLevel ||
//                        cbList.ClipboardDataType == eProfileType.StoreGroupLevelListView)
//                    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                    {
//                        //MIDStoreNodeClipboardData clipboardData = (MIDStoreNodeClipboardData)cbp.ClipboardData;
//                        //storeNode = clipboardData.Node;
//                        storeNode = (MIDStoreNode)cbList.ClipboardProfile.Node;

//                        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                        //switch (storeNode.StoreNodeType)
//                        switch (storeNode.NodeProfileType)
//                        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                        {
//                            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                            //case eStoreNodeType.group:

//                            //	storeGroupProf = _SAB.StoreServerSession.GetStoreGroup(storeNode.GroupRID);
//                            case eProfileType.StoreGroup:

//                                storeGroupProf = _SAB.StoreServerSession.GetStoreGroup(storeNode.Profile.Key);
//                            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                parentIdx = GetTreeNodeIdx(trvAttributes.Nodes, storeGroupProf.Name);

//                                if (parentIdx >= 0)
//                                {
//                                    parent = (MIDStoreNode)trvAttributes.Nodes[parentIdx];
//                                }
//                                else
//                                {
//                                    parent = CreateAttributesNode(storeGroupProf);

//                                    if (_currNode != null)
//                                    {
//                                        trvAttributes.Nodes.Insert(_currNode.Index, parent);
//                                    }
//                                    else
//                                    {
//                                        trvAttributes.Nodes.Add(parent);
//                                    }
//                                }

//                                foreach (StoreGroupLevelProfile strGrpLvlProf in storeGroupProf.GroupLevels)
//                                {
//                                    if (GetTreeNodeIdx(parent.Nodes, strGrpLvlProf.Name) < 0)
//                                    {
//                                        parent.Nodes.Add(CreateAttributesLevelNode(parent, strGrpLvlProf));
//                                    }
//                                }

//                                SetClosedImage(parent);

//                                break;

//                            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                            //case eStoreNodeType.groupLevel:

//                            //	storeGroupProf = _SAB.StoreServerSession.GetStoreGroup(storeNode.GroupRID);
//                            case eProfileType.StoreGroupLevel:

//                                storeGroupProf = _SAB.StoreServerSession.GetStoreGroup(storeNode.GetStoreGroupNode().Profile.Key);
//                            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                parentIdx = GetTreeNodeIdx(trvAttributes.Nodes, storeGroupProf.Name);

//                                if (parentIdx >= 0)
//                                {
//                                    parent = (MIDStoreNode)trvAttributes.Nodes[parentIdx];
//                                }
//                                else
//                                {
//                                    parent = CreateAttributesNode(storeGroupProf);

//                                    if (_currNode != null)
//                                    {
//                                        trvAttributes.Nodes.Insert(_currNode.Index, parent);
//                                    }
//                                    else
//                                    {
//                                        trvAttributes.Nodes.Add(parent);
//                                    }
//                                }

//                                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                //storeGroupLevelProf = _SAB.StoreServerSession.GetStoreGroupLevel(storeNode.GroupLevelRID);
//                                storeGroupLevelProf = _SAB.StoreServerSession.GetStoreGroupLevel(storeNode.Profile.Key);
//                                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                                if (GetTreeNodeIdx(parent.Nodes, storeGroupLevelProf.Name) < 0)
//                                {
//                                    parent.Nodes.Add(CreateAttributesLevelNode(parent, storeGroupLevelProf));
//                                }

//                                SetClosedImage(parent);

//                                break;

//                            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                            //case eStoreNodeType.store:
//                            case eProfileType.Store:
//                            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                break;
//                        }
//                    }
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvStores_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
//        {
//            MIDStoreNode storeNode;
//            StoreProfile storeProf;
//            TreeNodeClipboardList cbList = null;

//            try
//            {
//                //if (e.Data.GetDataPresent(typeof(MIDStoreNode)))
//                //{
//                //    storeNode = (MIDStoreNode)e.Data.GetData(typeof(MIDStoreNode));
//                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
//                {
//                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
//                    //if (cbp.ClipboardDataType == eClipboardDataType.Store)
//                    if (cbList.ClipboardDataType == eProfileType.Store)
//                    {
//                        //MIDStoreNodeClipboardData clipboardData = (MIDStoreNodeClipboardData)cbp.ClipboardData;
//                        //storeNode = clipboardData.Node;
//                        storeNode = (MIDStoreNode)cbList.ClipboardProfile.Node;

//                        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                        //if (storeNode.StoreNodeType == eStoreNodeType.store)
//                        if (storeNode.NodeProfileType == eProfileType.Store)
//                        {
//                            storeProf = _SAB.StoreServerSession.GetStoreProfile(storeNode.Profile.Key);
//                        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                            if (GetTreeNodeIdx(trvStores.Nodes, storeProf.Text) < 0)
//                            {
//                                trvStores.Nodes.Add(CreateStoreNode(storeProf));
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvStores_Enter(object sender, System.EventArgs e)
//        {
//            try
//            {
//                pnlAttributeConjunction.Visible = false;
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvStores_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            if (e.KeyCode == Keys.Delete)
//            {
//                DeleteCurrentNode(trvStores);
//            }
//        }

//        private void trvConditions_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
//        {
//            try
//            {
//                DisplayCurrentConditionNodeInfo();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }
		
//        private void trvConditions_Enter(object sender, System.EventArgs e)
//        {
//            try
//            {
//                DisplayCurrentConditionNodeInfo();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void trvConditions_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            if (e.KeyCode == Keys.Delete)
//            {
//                DeleteCurrentNode(trvConditions);
//            }
//        }

//        private void mniAttrDelete_Click(object sender, System.EventArgs e)
//        {
//            DeleteCurrentNode(trvAttributes);
//        }

//        private void mniCondDelete_Click(object sender, System.EventArgs e)
//        {
//            DeleteCurrentNode(trvConditions);
//        }

//        private void mniStoreDelete_Click(object sender, System.EventArgs e)
//        {
//            DeleteCurrentNode(trvStores);
//        }

//        private void OnNextButtonStatusChange(object source, NextButtonStatusEventArgs e)
//        {
//            btnNext.Enabled = e.Enabled;
//        }

//        private bool SaveFilter()
//        {
//            StoreFilterDefinition filterDef;
//            FilterWizardPanel currPanel;
//            FilterWizardPanel currCondPanel;
//            AttrQueryAttributeMainOperand attrQueryOperand;
//            AttrQueryStoreMainOperand storeQueryOperand;
//            DataQueryVariableOperand varOperand;
//            DataQueryLiteralOperand literalOperand;
//            DataQueryGradeOperand gradeOperand;
//            DataQueryStatusOperand statusOperand;
//            QueryOperand queryOperand;
//            AttributeNodeTag attrNodeTag;
//            ConditionNodeTag condNodeTag;
//            TimeTotalComboObject timeTotComboObj;
//            ProfileComboObject profComboObj;

//            eFilterCompareTo compareTo = eFilterCompareTo.None;
//            bool firstPass;

//            FilterWizardVar1 var1Control;
//            FilterWizardQuantity quantityControl;
//            FilterWizardGrade gradeControl;
//            FilterWizardStatus statusControl;
//            FilterWizardVar2 var2Control;
//            FilterWizardPercent pctControl;

//            int userRID;
//            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            DataTable dtMainFolder;
//            FolderProfile folderProf;
//            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//            try
//            {
//                filterDef = new StoreFilterDefinition(_SAB, _SAB.ClientServerSession, _dlFilter, null);
//                currPanel = _firstPanel;

//                while (currPanel != null)
//                {
//                    if (currPanel.GetType() == typeof(FilterWizardAttributePanel))
//                    {
//                        if (trvAttributes.Nodes.Count > 0)
//                        {
//                            attrQueryOperand = null;
//                            firstPass = true;

//                            foreach (MIDStoreNode storeNode in trvAttributes.Nodes)
//                            {
//                                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                //if (storeNode.StoreNodeType == eStoreNodeType.group)
//                                if (storeNode.NodeProfileType == eProfileType.StoreGroup)
//                                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                {
//                                    if (!firstPass)
//                                    {
//                                        attrNodeTag = (AttributeNodeTag)storeNode.Tag;
//                                        filterDef.AttrOperandList.Add(Activator.CreateInstance(attrNodeTag.ConjunctionType, new object[] { filterDef }));
//                                    }

//                                    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                    //attrQueryOperand = new AttrQueryAttributeMainOperand(filterDef, storeNode.GroupRID);
//                                    attrQueryOperand = new AttrQueryAttributeMainOperand(filterDef, storeNode.Profile.Key);
//                                    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                                    foreach (MIDStoreNode storeGroupNode in storeNode.Nodes)
//                                    {
//                                        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                        //if (storeGroupNode.StoreNodeType == eStoreNodeType.groupLevel)
//                                        //{
//                                        //    ((AttrQueryAttributeMainOperand)attrQueryOperand).AddSet(storeGroupNode.GroupLevelRID);
//                                        if (storeGroupNode.NodeProfileType == eProfileType.StoreGroupLevel)
//                                        {
//                                            ((AttrQueryAttributeMainOperand)attrQueryOperand).AddSet(storeGroupNode.Profile.Key);
//                                        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                        }
//                                    }

//                                    filterDef.AttrOperandList.Add(attrQueryOperand);
//                                }

//                                firstPass = false;
//                            }
//                        }

//                        if (trvAttributes.Nodes.Count > 0 && trvStores.Nodes.Count > 0)
//                        {
//                            filterDef.AttrOperandList.Add(new GenericQueryOrOperand(filterDef));
//                        }
				
//                        if (trvStores.Nodes.Count > 0)
//                        {
//                            storeQueryOperand = new AttrQueryStoreMainOperand(filterDef);

//                            foreach (MIDStoreNode storeNode in trvStores.Nodes)
//                            {
//                                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                //if (storeNode.StoreNodeType == eStoreNodeType.store)
//                                if (storeNode.NodeProfileType == eProfileType.Store)
//                                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                {
//                                    ((AttrQueryStoreMainOperand)storeQueryOperand).AddStore(storeNode.Profile.Key);
//                                }
//                            }

//                            filterDef.AttrOperandList.Add(storeQueryOperand);
//                        }
//                    }
//                    else if (currPanel.GetType() == typeof(FilterWizardDataPanel))
//                    {
//                        firstPass = true;

//                        foreach (TreeNode conditionNode in trvConditions.Nodes)
//                        {
//                            condNodeTag = (ConditionNodeTag)conditionNode.Tag;
//                            currCondPanel = condNodeTag.ConditionPanel;

//                            if (!firstPass)
//                            {
//                                filterDef.DataOperandList.Add(Activator.CreateInstance(condNodeTag.ConjunctionType, new object[] { filterDef }));
//                            }

//                            while (currCondPanel.GetType() != typeof(FilterWizardDataPanel))
//                            {
//                                if (currCondPanel.GetType() == typeof(FilterWizardConditionVar1Panel))
//                                {
//                                    var1Control = ((FilterWizardConditionVar1Panel)currCondPanel).ParentControl;

//                                    if (var1Control.VariableCombo.SelectedItem.GetType() == typeof(TimeTotalComboObject))
//                                    {
//                                        timeTotComboObj = (TimeTotalComboObject)var1Control.VariableCombo.SelectedItem;
//                                        varOperand = new DataQueryTimeTotalVariableOperand(filterDef, (TimeTotalVariableProfile)timeTotComboObj.Profile, (VariableProfile)timeTotComboObj.VarProfile, timeTotComboObj.TimeTotalIdx);
//                                    }
//                                    else
//                                    {
//                                        profComboObj = (ProfileComboObject)var1Control.VariableCombo.SelectedItem;
//                                        varOperand = new DataQueryPlanVariableOperand(filterDef, (VariableProfile)profComboObj.Profile);
//                                    }

//                                    if (var1Control.MerchandiseTextBox.Tag != null)
//                                    {
//                                        varOperand.NodeProfile = (HierarchyNodeProfile)var1Control.MerchandiseTextBox.Tag;
//                                    }

//                                    if (var1Control.VersionCombo.SelectedIndex != -1)
//                                    {
//                                        varOperand.VersionProfile = (VersionProfile)((ProfileComboObject)var1Control.VersionCombo.SelectedItem).Profile;
//                                    }

//                                    if (var1Control.DateRangeSelector.DateRangeRID != Include.UndefinedCalendarDateRange)
//                                    {
//                                        varOperand.DateRangeProfile = _SAB.ClientServerSession.Calendar.GetDateRange(var1Control.DateRangeSelector.DateRangeRID, _SAB.ClientServerSession.Calendar.CurrentDate);
//                                    }

//                                    if (var1Control.CubeModifyerCombo.SelectedIndex != -1)
//                                    {
//                                        varOperand.CubeModifyer = (eFilterCubeModifyer)((ComboObject)var1Control.CubeModifyerCombo.SelectedItem).Key;
//                                    }

//                                    if (var1Control.TimeModifyerCombo.SelectedIndex != -1)
//                                    {
//                                        varOperand.TimeModifyer = (eFilterTimeModifyer)((ComboObject)var1Control.TimeModifyerCombo.SelectedItem).Key;
//                                    }

//                                    filterDef.DataOperandList.Add(varOperand);

//                                    compareTo = (eFilterCompareTo)((ComboObject)var1Control.CompareToCombo.SelectedItem).Key;
//                                }
//                                else if (currCondPanel.GetType() == typeof(FilterWizardConditionQuantityPanel))
//                                {
//                                    quantityControl = ((FilterWizardConditionQuantityPanel)currCondPanel).ParentControl;

//                                    if (((ComparisonComboObject)quantityControl.ComparisonCombo.SelectedItem).Negate)
//                                    {
//                                        queryOperand = new DataQueryNotOperand(filterDef);
//                                        filterDef.DataOperandList.Add(queryOperand);
//                                    }

//                                    queryOperand = (QueryOperand)Activator.CreateInstance(((ComparisonComboObject)quantityControl.ComparisonCombo.SelectedItem).OperandType, new object[] { filterDef });
//                                    filterDef.DataOperandList.Add(queryOperand);

//                                    literalOperand = new DataQueryLiteralOperand(filterDef);
//                                    literalOperand.LiteralValue = System.Convert.ToDouble(quantityControl.QuantityTextBox.Text, CultureInfo.CurrentUICulture);
//                                    filterDef.DataOperandList.Add(literalOperand);
//                                }
//                                else if (currCondPanel.GetType() == typeof(FilterWizardConditionGradePanel))
//                                {
//                                    gradeControl = ((FilterWizardConditionGradePanel)currCondPanel).ParentControl;

//                                    if (((ComparisonComboObject)gradeControl.ComparisonCombo.SelectedItem).Negate)
//                                    {
//                                        queryOperand = new DataQueryNotOperand(filterDef);
//                                        filterDef.DataOperandList.Add(queryOperand);
//                                    }

//                                    queryOperand = (QueryOperand)Activator.CreateInstance(((ComparisonComboObject)gradeControl.ComparisonCombo.SelectedItem).OperandType, new object[] { filterDef });
//                                    filterDef.DataOperandList.Add(queryOperand);

//                                    gradeOperand = new DataQueryGradeOperand(filterDef);
//                                    gradeOperand.GradeValue = gradeControl.GradeTextBox.Text;
//                                    filterDef.DataOperandList.Add(gradeOperand);
//                                }
//                                else if (currCondPanel.GetType() == typeof(FilterWizardConditionStatusPanel))
//                                {
//                                    statusControl = ((FilterWizardConditionStatusPanel)currCondPanel).ParentControl;

//                                    if (((ComparisonComboObject)statusControl.ComparisonCombo.SelectedItem).Negate)
//                                    {
//                                        queryOperand = new DataQueryNotOperand(filterDef);
//                                        filterDef.DataOperandList.Add(queryOperand);
//                                    }

//                                    queryOperand = (QueryOperand)Activator.CreateInstance(((ComparisonComboObject)statusControl.ComparisonCombo.SelectedItem).OperandType, new object[] { filterDef });
//                                    filterDef.DataOperandList.Add(queryOperand);

//                                    statusOperand = new DataQueryStatusOperand(filterDef, (eStoreStatus)System.Convert.ToInt32(((ComboObject)statusControl.StatusCombo.SelectedItem).Key));
//                                    filterDef.DataOperandList.Add(statusOperand);
//                                }
//                                else if (currCondPanel.GetType() == typeof(FilterWizardConditionVar2Panel))
//                                {
//                                    var2Control = ((FilterWizardConditionVar2Panel)currCondPanel).ParentControl;

//                                    if (((ComparisonComboObject)var2Control.ComparisonCombo.SelectedItem).Negate)
//                                    {
//                                        queryOperand = new DataQueryNotOperand(filterDef);
//                                        filterDef.DataOperandList.Add(queryOperand);
//                                    }

//                                    queryOperand = (QueryOperand)Activator.CreateInstance(((ComparisonComboObject)var2Control.ComparisonCombo.SelectedItem).OperandType, new object[] { filterDef });
//                                    filterDef.DataOperandList.Add(queryOperand);

//                                    if (var2Control.VariableCombo.SelectedItem.GetType() == typeof(TimeTotalComboObject))
//                                    {
//                                        timeTotComboObj = (TimeTotalComboObject)var2Control.VariableCombo.SelectedItem;
//                                        varOperand = new DataQueryTimeTotalVariableOperand(filterDef, (TimeTotalVariableProfile)timeTotComboObj.Profile, (VariableProfile)timeTotComboObj.VarProfile, timeTotComboObj.TimeTotalIdx);
//                                    }
//                                    else
//                                    {
//                                        profComboObj = (ProfileComboObject)var2Control.VariableCombo.SelectedItem;
//                                        varOperand = new DataQueryPlanVariableOperand(filterDef, (VariableProfile)profComboObj.Profile);
//                                    }

//                                    if (var2Control.MerchandiseTextBox.Tag != null)
//                                    {
//                                        varOperand.NodeProfile = (HierarchyNodeProfile)var2Control.MerchandiseTextBox.Tag;
//                                    }

//                                    if (var2Control.VersionCombo.SelectedIndex != -1)
//                                    {
//                                        varOperand.VersionProfile = (VersionProfile)((ProfileComboObject)var2Control.VersionCombo.SelectedItem).Profile;
//                                    }

//                                    if (var2Control.DateRangeSelector.DateRangeRID != Include.UndefinedCalendarDateRange)
//                                    {
//// BEGIN MID Track #2495 - FilterWizard not saving secondary variable dates
////										varOperand.DateRangeProfile = (DateRangeProfile)var2Control.DateRangeSelector.Tag;
//                                        varOperand.DateRangeProfile = _SAB.ClientServerSession.Calendar.GetDateRange(var2Control.DateRangeSelector.DateRangeRID, _SAB.ClientServerSession.Calendar.CurrentDate);
//// END MID Track #2495 - FilterWizard not saving secondary variable dates
//                                    }

//                                    if (var2Control.CubeModifyerCombo.SelectedIndex != -1)
//                                    {
//                                        varOperand.CubeModifyer = (eFilterCubeModifyer)((ComboObject)var2Control.CubeModifyerCombo.SelectedItem).Key;
//                                    }

//                                    if (var2Control.TimeModifyerCombo.SelectedIndex != -1)
//                                    {
//                                        varOperand.TimeModifyer = (eFilterTimeModifyer)((ComboObject)var2Control.TimeModifyerCombo.SelectedItem).Key;
//                                    }

//                                    filterDef.DataOperandList.Add(varOperand);
//                                }

//                                else if (currCondPanel.GetType() == typeof(FilterWizardConditionPercentPanel))
//                                {
//                                    pctControl = ((FilterWizardConditionPercentPanel)currCondPanel).ParentControl;

//                                    if (compareTo == eFilterCompareTo.PctChange)
//                                    {
//                                        queryOperand = new DataQueryPctChangeOperand(filterDef);
//                                    }
//                                    else
//                                    {
//                                        queryOperand = new DataQueryPctOfOperand(filterDef);
//                                    }

//                                    filterDef.DataOperandList.Add(queryOperand);

//                                    if (pctControl.VariableCombo.SelectedItem.GetType() == typeof(TimeTotalComboObject))
//                                    {
//                                        timeTotComboObj = (TimeTotalComboObject)pctControl.VariableCombo.SelectedItem;
//                                        varOperand = new DataQueryTimeTotalVariableOperand(filterDef, (TimeTotalVariableProfile)timeTotComboObj.Profile, (VariableProfile)timeTotComboObj.VarProfile, timeTotComboObj.TimeTotalIdx);
//                                    }
//                                    else
//                                    {
//                                        profComboObj = (ProfileComboObject)pctControl.VariableCombo.SelectedItem;
//                                        varOperand = new DataQueryPlanVariableOperand(filterDef, (VariableProfile)profComboObj.Profile);
//                                    }

//                                    if (pctControl.MerchandiseTextBox.Tag != null)
//                                    {
//                                        varOperand.NodeProfile = (HierarchyNodeProfile)pctControl.MerchandiseTextBox.Tag;
//                                    }

//                                    if (pctControl.VersionCombo.SelectedIndex != -1)
//                                    {
//                                        varOperand.VersionProfile = (VersionProfile)((ProfileComboObject)pctControl.VersionCombo.SelectedItem).Profile;
//                                    }

//                                    if (pctControl.DateRangeSelector.DateRangeRID != Include.UndefinedCalendarDateRange)
//                                    {
//// BEGIN MID Track #2495 - FilterWizard not saving secondary variable dates
////										varOperand.DateRangeProfile = (DateRangeProfile)pctControl.DateRangeSelector.Tag;
//                                        varOperand.DateRangeProfile = _SAB.ClientServerSession.Calendar.GetDateRange(pctControl.DateRangeSelector.DateRangeRID, _SAB.ClientServerSession.Calendar.CurrentDate);
//// END MID Track #2495 - FilterWizard not saving secondary variable dates
//                                    }

//                                    if (pctControl.CubeModifyerCombo.SelectedIndex != -1)
//                                    {
//                                        varOperand.CubeModifyer = (eFilterCubeModifyer)((ComboObject)pctControl.CubeModifyerCombo.SelectedItem).Key;
//                                    }

//                                    if (pctControl.TimeModifyerCombo.SelectedIndex != -1)
//                                    {
//                                        varOperand.TimeModifyer = (eFilterTimeModifyer)((ComboObject)pctControl.TimeModifyerCombo.SelectedItem).Key;
//                                    }

//                                    filterDef.DataOperandList.Add(varOperand);

//                                    if (((ComparisonComboObject)pctControl.ComparisonCombo.SelectedItem).Negate)
//                                    {
//                                        queryOperand = new DataQueryNotOperand(filterDef);
//                                        filterDef.DataOperandList.Add(queryOperand);
//                                    }

//                                    queryOperand = (QueryOperand)Activator.CreateInstance(((ComparisonComboObject)pctControl.ComparisonCombo.SelectedItem).OperandType, new object[] { filterDef });
//                                    filterDef.DataOperandList.Add(queryOperand);

//                                    literalOperand = new DataQueryLiteralOperand(filterDef);
//                                    literalOperand.LiteralValue = System.Convert.ToDouble(pctControl.ValueTextBox.Text, CultureInfo.CurrentUICulture);
//                                    filterDef.DataOperandList.Add(literalOperand);
//                                }

//                                currCondPanel = currCondPanel.NextPanel;
//                            }

//                            firstPass = false;
//                        }
//                    }

//                    currPanel = currPanel.NextPanel;
//                }

//                if (rdoUser.Checked)
//                {
//                    userRID = _SAB.ClientServerSession.UserRID;
//                }
//                else
//                {
//                    userRID = Include.GlobalUserRID;	// Issue 3806
//                }

//                if (_filterKey == Include.NoRID)
//                {
//                    _filterKey = filterDef.SaveFilter(_filterKey, userRID, txtName.Text);

//                    _dlFolder.OpenUpdateConnection();

//                    try
//                    {
//                        // Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                        //_dlFolder.Folder_Item_Insert(Include.NoRID, _filterKey, eFolderChildType.Filter);
//                        if (userRID == Include.GlobalUserRID)
//                        {
//                            dtMainFolder = _dlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.FilterMainGlobalFolder);
//                        }
//                        else
//                        {
//                            dtMainFolder = _dlFolder.Folder_Read(userRID, eProfileType.FilterMainUserFolder);
//                        }

//                        folderProf = new FolderProfile(dtMainFolder.Rows[0]);

//                        _dlFolder.Folder_Item_Insert(folderProf.Key, _filterKey, eProfileType.StoreFilter);
//                        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                        _dlFolder.CommitData();
//                    }
//                    catch (Exception exc)
//                    {
//                        string message = exc.ToString();
//                        throw;
//                    }
//                    finally
//                    {
//                        _dlFolder.CloseUpdateConnection();
//                    }
//                }
//                else
//                {
//                    filterDef.SaveFilter(_filterKey, userRID, txtName.Text);
//                }

//                _filterSaved = true;

//                return true;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void SetVar1ComboBoxes(FilterWizardVar1 aVar1Control)
//        {
//            try
//            {
////				_bindingCombos = true;

//                foreach (ComboObject comboObj in _compareToList)
//                {
//                    aVar1Control.CompareToCombo.Items.Add(comboObj);
//                }

//                aVar1Control.CompareToCombo.SelectedIndex = 0;

//                foreach (ComboObject comboObj in _timeModVar1List)
//                {
//                    aVar1Control.TimeModifyerCombo.Items.Add(comboObj);
//                }

//                aVar1Control.TimeModifyerCombo.SelectedIndex = 0;

//                foreach (ComboObject comboObj in _cubeModList)
//                {
//                    aVar1Control.CubeModifyerCombo.Items.Add(comboObj);
//                }

//                aVar1Control.CubeModifyerCombo.SelectedIndex = 0;

//                foreach (ComboObject comboObj in _versionList)
//                {
//                    aVar1Control.VersionCombo.Items.Add(comboObj);
//                }

//                aVar1Control.VersionCombo.SelectedIndex = -1;

//                foreach (ComboObject comboObj in _variableList)
//                {
//                    aVar1Control.VariableCombo.Items.Add(comboObj);
//                }

//                aVar1Control.VariableCombo.SelectedIndex = -1;

////				_bindingCombos = false;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void SetQuantityComboBoxes(FilterWizardQuantity aQuantityControl)
//        {
//            try
//            {
////				_bindingCombos = true;

//                foreach (ComboObject comboObj in _comparisonList)
//                {
//                    aQuantityControl.ComparisonCombo.Items.Add(comboObj);
//                }

//                aQuantityControl.ComparisonCombo.SelectedIndex = 0;

////				_bindingCombos = false;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void SetGradeComboBoxes(FilterWizardGrade aGradeControl)
//        {
//            try
//            {
////				_bindingCombos = true;

//                foreach (ComboObject comboObj in _comparisonList)
//                {
//                    aGradeControl.ComparisonCombo.Items.Add(comboObj);
//                }

//                aGradeControl.ComparisonCombo.SelectedIndex = 0;

////				_bindingCombos = false;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void SetStatusComboBoxes(FilterWizardStatus aStatusControl)
//        {
//            try
//            {
////				_bindingCombos = true;

//                foreach (ComboObject comboObj in _comparisonList)
//                {
//                    aStatusControl.ComparisonCombo.Items.Add(comboObj);
//                }

//                aStatusControl.ComparisonCombo.SelectedIndex = 0;

//                foreach (ComboObject comboObj in _statusList)
//                {
//                    aStatusControl.StatusCombo.Items.Add(comboObj);
//                }

//                aStatusControl.StatusCombo.SelectedIndex = 0;

////				_bindingCombos = false;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void SetPercentComboBoxes(FilterWizardPercent aPctControl)
//        {
//            try
//            {
////				_bindingCombos = true;

//                foreach (ComboObject comboObj in _timeModVar2List)
//                {
//                    aPctControl.TimeModifyerCombo.Items.Add(comboObj);
//                }

//                aPctControl.TimeModifyerCombo.SelectedIndex = 0;

//                foreach (ComboObject comboObj in _cubeModList)
//                {
//                    aPctControl.CubeModifyerCombo.Items.Add(comboObj);
//                }

//                aPctControl.CubeModifyerCombo.SelectedIndex = 0;

//                foreach (ComboObject comboObj in _comparisonList)
//                {
//                    aPctControl.ComparisonCombo.Items.Add(comboObj);
//                }

//                aPctControl.ComparisonCombo.SelectedIndex = 0;

//                foreach (ComboObject comboObj in _versionList)
//                {
//                    aPctControl.VersionCombo.Items.Add(comboObj);
//                }

//                aPctControl.VersionCombo.SelectedIndex = -1;

//                foreach (ComboObject comboObj in _variableList)
//                {
//                    aPctControl.VariableCombo.Items.Add(comboObj);
//                }

//                aPctControl.VariableCombo.SelectedIndex = -1;

////				_bindingCombos = false;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void SetVar2ComboBoxes(FilterWizardVar2 aVar2Control)
//        {
//            try
//            {
////				_bindingCombos = true;

//                foreach (ComboObject comboObj in _timeModVar2List)
//                {
//                    aVar2Control.TimeModifyerCombo.Items.Add(comboObj);
//                }

//                aVar2Control.TimeModifyerCombo.SelectedIndex = 0;

//                foreach (ComboObject comboObj in _cubeModList)
//                {
//                    aVar2Control.CubeModifyerCombo.Items.Add(comboObj);
//                }

//                aVar2Control.CubeModifyerCombo.SelectedIndex = 0;

//                foreach (ComboObject comboObj in _comparisonList)
//                {
//                    aVar2Control.ComparisonCombo.Items.Add(comboObj);
//                }

//                aVar2Control.ComparisonCombo.SelectedIndex = 0;

//                foreach (ComboObject comboObj in _versionList)
//                {
//                    aVar2Control.VersionCombo.Items.Add(comboObj);
//                }

//                aVar2Control.VersionCombo.SelectedIndex = -1;

//                foreach (ComboObject comboObj in _variableList)
//                {
//                    aVar2Control.VariableCombo.Items.Add(comboObj);
//                }

//                aVar2Control.VariableCombo.SelectedIndex = -1;

////				_bindingCombos = false;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private int GetTreeNodeIdx(TreeNodeCollection aNodes, string aName)
//        {
//            int i;

//            try
//            {
//                for (i = 0; i < aNodes.Count; i++)
//                {
//                    if (aNodes[i].Text == aName)
//                    {
//                        return i;
//                    }
//                }

//                return -1;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void DisplayCurrentAttributesNodeInfo()
//        {
//            MIDStoreNode storeNode;
//            AttributeNodeTag attrNodeTag;
//            int currNodeIdx;

//            try
//            {
//                if (trvAttributes.SelectedNode != null)
//                {
//                    storeNode = GetParentNode((MIDStoreNode)trvAttributes.SelectedNode);
//                    if (trvAttributes.Nodes[0].Text != storeNode.Text)
//                    {
//                        currNodeIdx = trvAttributes.Nodes.IndexOf(storeNode);
//                        lblAttributeConjunction.Text = "Join with \"" + trvAttributes.Nodes[currNodeIdx - 1].Text + "\" using:";
//                        attrNodeTag = (AttributeNodeTag)storeNode.Tag;
//                        if (attrNodeTag.ConjunctionType == typeof(GenericQueryOrOperand))
//                        {
//                            rdoAttributeOr.Checked = true;
//                        }
//                        else
//                        {
//                            rdoAttributeAnd.Checked = true;
//                        }

//                        pnlAttributeConjunction.Visible = true;
//                    }
//                    else
//                    {
//                        pnlAttributeConjunction.Visible = false;
//                    }
//                }
//                else
//                {
//                    pnlAttributeConjunction.Visible = false;
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void DisplayCurrentConditionNodeInfo()
//        {
//            ConditionNodeTag condNodeTag;
//            int currNodeIdx;
//            FilterWizardConditionVar1Panel var1Panel;
//            FilterWizardConditionQuantityPanel quantityPanel;
//            FilterWizardConditionGradePanel gradePanel;
//            FilterWizardConditionStatusPanel statusPanel;
//            FilterWizardConditionPercentPanel percentPanel;
//            FilterWizardConditionVar2Panel var2Panel;

//            try
//            {
//                if (trvConditions.SelectedNode != null)
//                {
//                    pnlDataInfo.Visible = true;

//                    condNodeTag = (ConditionNodeTag)trvConditions.SelectedNode.Tag;

//                    rtbConditionInfo.Clear();

//                    var1Panel = (FilterWizardConditionVar1Panel)condNodeTag.ConditionPanel;
//                    // BEGIN MID Track #4347 - change condition verbage	
//                    //rtbConditionInfo.AppendText("Compare: " + 
//                    rtbConditionInfo.AppendText("" + 
//                    // END MID Track #4347  
//                        FormatVariableText(
//                            var1Panel.ParentControl.VariableCombo,
//                            var1Panel.ParentControl.MerchandiseTextBox,
//                            var1Panel.ParentControl.VersionCombo,
//                            var1Panel.ParentControl.DateRangeSelector,
//                            var1Panel.ParentControl.TimeModifyerCombo,
//                            var1Panel.ParentControl.CubeModifyerCombo) + System.Environment.NewLine);

//                    switch ((eFilterCompareTo)((ComboObject)var1Panel.ParentControl.CompareToCombo.SelectedItem).Key)
//                    {
//                        case eFilterCompareTo.Constant :
//                            // BEGIN MID Track #4347 - change condition verbage
//                            //if (var1Panel.NextPanel.GetType() == typeof(FilterWizardConditionQuantityPanel))
//                            //{
//                            //	quantityPanel = (FilterWizardConditionQuantityPanel)var1Panel.NextPanel;
//                            //	rtbConditionInfo.AppendText("To: " + quantityPanel.ParentControl.QuantityTextBox.Text + System.Environment.NewLine);
//                            //	rtbConditionInfo.AppendText("With: " + FormatComparisonTypeText(quantityPanel.ParentControl.ComparisonCombo) + System.Environment.NewLine);
//                            //}
//                            //else if (var1Panel.NextPanel.GetType() == typeof(FilterWizardConditionGradePanel))
//                            //{
//                            //	gradePanel = (FilterWizardConditionGradePanel)var1Panel.NextPanel;
//                            //	rtbConditionInfo.AppendText("To: " + gradePanel.ParentControl.GradeTextBox.Text + System.Environment.NewLine);
//                            //	rtbConditionInfo.AppendText("With: " + FormatComparisonTypeText(gradePanel.ParentControl.ComparisonCombo) + System.Environment.NewLine);
//                            //}
//                            //else if (var1Panel.NextPanel.GetType() == typeof(FilterWizardConditionStatusPanel))
//                            //{
//                            //	statusPanel = (FilterWizardConditionStatusPanel)var1Panel.NextPanel;
//                            //	rtbConditionInfo.AppendText("To: " + ((ComboObject)statusPanel.ParentControl.StatusCombo.SelectedItem).Value + System.Environment.NewLine);
//                            //	rtbConditionInfo.AppendText("With: " + FormatComparisonTypeText(statusPanel.ParentControl.ComparisonCombo) + System.Environment.NewLine);
//                            //}
							
//                            if (var1Panel.NextPanel.GetType() == typeof(FilterWizardConditionQuantityPanel))
//                            {
//                                quantityPanel = (FilterWizardConditionQuantityPanel)var1Panel.NextPanel;
//                                // Begin TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
//                                //rtbConditionInfo.AppendText(FormatComparisonTypeText(quantityPanel.ParentControl.ComparisonCombo.ComboBox)); 
//                                rtbConditionInfo.AppendText(FormatComparisonTypeText(quantityPanel.ParentControl.ComparisonCombo));
//                                // End TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
//                                rtbConditionInfo.AppendText(quantityPanel.ParentControl.QuantityTextBox.Text + System.Environment.NewLine);
//                            }
//                            else if (var1Panel.NextPanel.GetType() == typeof(FilterWizardConditionGradePanel))
//                            {
//                                gradePanel = (FilterWizardConditionGradePanel)var1Panel.NextPanel;
//                                // Begin TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
//                                //rtbConditionInfo.AppendText(FormatComparisonTypeText(gradePanel.ParentControl.ComparisonCombo.ComboBox));
//                                rtbConditionInfo.AppendText(FormatComparisonTypeText(gradePanel.ParentControl.ComparisonCombo));
//                                // End TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
//                                rtbConditionInfo.AppendText(gradePanel.ParentControl.GradeTextBox.Text + System.Environment.NewLine);
//                            }
//                            else if (var1Panel.NextPanel.GetType() == typeof(FilterWizardConditionStatusPanel))
//                            {
//                                statusPanel = (FilterWizardConditionStatusPanel)var1Panel.NextPanel;
//                                // Begin TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
//                                //rtbConditionInfo.AppendText(FormatComparisonTypeText(statusPanel.ParentControl.ComparisonCombo.ComboBox));
//                                rtbConditionInfo.AppendText(FormatComparisonTypeText(statusPanel.ParentControl.ComparisonCombo));
//                                // End TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
//                                rtbConditionInfo.AppendText(((ComboObject)statusPanel.ParentControl.StatusCombo.SelectedItem).Value + System.Environment.NewLine);
//                            }
//                            // END MID Track #4347 	
	
//                            break;

//                        case eFilterCompareTo.Variable :

//                            var2Panel = (FilterWizardConditionVar2Panel)var1Panel.NextPanel;
//                            // BEGIN MID Track #4347 - change condition verbage
//                            rtbConditionInfo.AppendText(FormatComparisonTypeText(var2Panel.ParentControl.ComparisonCombo));
//                            // END MID Track #4347 	
//                            rtbConditionInfo.AppendText(
//                                FormatVariableText(
//                                    var2Panel.ParentControl.VariableCombo,
//                                    var2Panel.ParentControl.MerchandiseTextBox,
//                                    var2Panel.ParentControl.VersionCombo,
//                                    var2Panel.ParentControl.DateRangeSelector,
//                                    var2Panel.ParentControl.TimeModifyerCombo,
//                                    var2Panel.ParentControl.CubeModifyerCombo) + System.Environment.NewLine);
//                            // BEGIN MID Track #4347 - change condition verbage
//                            //rtbConditionInfo.AppendText("With: " + FormatComparisonTypeText(var2Panel.ParentControl.ComparisonCombo) + System.Environment.NewLine);
//                            // END MID Track #4347 	
//                            break;

//                        case eFilterCompareTo.PctOf :

//                            percentPanel = (FilterWizardConditionPercentPanel)var1Panel.NextPanel;
//                            // BEGIN MID Track #4347 - change condition verbage
//                            //rtbConditionInfo.AppendText("To: % Of " + 
//                            //	FormatVariableText(
//                            //		var1Panel.ParentControl.VariableCombo,
//                            //		var1Panel.ParentControl.MerchandiseTextBox,
//                            //		var1Panel.ParentControl.VersionCombo,
//                            //		var1Panel.ParentControl.DateRangeSelector,
//                            //		var1Panel.ParentControl.TimeModifyerCombo,
//                            //		var1Panel.ParentControl.CubeModifyerCombo) + System.Environment.NewLine);
//                            rtbConditionInfo.AppendText("% Of " + 
//                                FormatVariableText(
//                                percentPanel.ParentControl.VariableCombo,
//                                percentPanel.ParentControl.MerchandiseTextBox,
//                                percentPanel.ParentControl.VersionCombo,
//                                percentPanel.ParentControl.DateRangeSelector,
//                                percentPanel.ParentControl.TimeModifyerCombo,
//                                percentPanel.ParentControl.CubeModifyerCombo) + System.Environment.NewLine);
							
//                            //rtbConditionInfo.AppendText("With: " + FormatComparisonTypeText(percentPanel.ParentControl.ComparisonCombo) + percentPanel.ParentControl.ValueTextBox.Text + System.Environment.NewLine);
//                            rtbConditionInfo.AppendText(FormatComparisonTypeText(percentPanel.ParentControl.ComparisonCombo) + percentPanel.ParentControl.ValueTextBox.Text + System.Environment.NewLine);
//                            // END MID Track #4347 
//                            break;

//                        case eFilterCompareTo.PctChange :

//                            percentPanel = (FilterWizardConditionPercentPanel)var1Panel.NextPanel;
//                            // BEGIN MID Track #4347 - change condition verbage
//                            //rtbConditionInfo.AppendText("To: % Change " + 
//                            //	FormatVariableText(
//                            //		var1Panel.ParentControl.VariableCombo,
//                            //		var1Panel.ParentControl.MerchandiseTextBox,
//                            //		var1Panel.ParentControl.VersionCombo,
//                            //		var1Panel.ParentControl.DateRangeSelector,
//                            //		var1Panel.ParentControl.TimeModifyerCombo,
//                            //		var1Panel.ParentControl.CubeModifyerCombo) + System.Environment.NewLine);
//                            rtbConditionInfo.AppendText("% Chg " + 
//                                FormatVariableText(
//                                    percentPanel.ParentControl.VariableCombo,
//                                    percentPanel.ParentControl.MerchandiseTextBox,
//                                    percentPanel.ParentControl.VersionCombo,
//                                    percentPanel.ParentControl.DateRangeSelector,
//                                    percentPanel.ParentControl.TimeModifyerCombo,
//                                    percentPanel.ParentControl.CubeModifyerCombo) + System.Environment.NewLine);
							 
//                            //rtbConditionInfo.AppendText("With: " + FormatComparisonTypeText(percentPanel.ParentControl.ComparisonCombo) + percentPanel.ParentControl.ValueTextBox.Text + System.Environment.NewLine);
//                            rtbConditionInfo.AppendText(FormatComparisonTypeText(percentPanel.ParentControl.ComparisonCombo) + percentPanel.ParentControl.ValueTextBox.Text + System.Environment.NewLine);
//                            // END MID Track #4347 
//                            break;

//                    }

//                    if (trvConditions.Nodes[0].Text != trvConditions.SelectedNode.Text)
//                    {
//                        currNodeIdx = trvConditions.Nodes.IndexOf(trvConditions.SelectedNode);
//                        lblConditionConjunction.Text = "Join with \"" + trvConditions.Nodes[currNodeIdx - 1].Text + "\" using:";

//                        if (condNodeTag.ConjunctionType == typeof(GenericQueryOrOperand))
//                        {
//                            rdoConditionOr.Checked = true;
//                        }
//                        else
//                        {
//                            rdoConditionAnd.Checked = true;
//                        }

//                        pnlDataConjunction.Visible = true;
//                    }
//                    else
//                    {
//                        pnlDataConjunction.Visible = false;
//                    }
//                }
//                else
//                {
//                    pnlDataInfo.Visible = false;
//                    pnlDataConjunction.Visible = false;
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        // Begin TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
//        //private string FormatVariableText(ComboBox aVariable, TextBox aMerchandise, ComboBox aVersion, MIDDateRangeSelector aDateRange, ComboBox aTimeMod, ComboBox aCubeMod)
//        private string FormatVariableText(MIDComboBoxEnh aVariable, TextBox aMerchandise, MIDComboBoxEnh aVersion, MIDDateRangeSelector aDateRange, MIDComboBoxEnh aTimeMod, MIDComboBoxEnh aCubeMod)
//        // End TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
//        {
//            string outString;

//            try
//            {
//                outString = ((ComboObject)aVariable.SelectedItem).Value + " ( ";

//                if (aMerchandise.Text.Length > 0)
//                {
//                    outString += aMerchandise.Text + ", ";
//                }
//                // BEGIN MID Track #4347 - change condition verbage		
//                //else
//                //{
//                //	outString += "Current, ";
//                //}
//                // END MID Track #4347
//                if (aVersion.SelectedIndex != -1)
//                {
//                    outString += ((ComboObject)aVersion.SelectedItem).Value + ", ";
//                }
//                // BEGIN MID Track #4347 - change condition verbage		
//                //else
//                //{
//                //	outString += "Current, ";
//                //}
//                // END MID Track #4347

//                if (aDateRange.DateRangeRID != Include.UndefinedCalendarDateRange)
//                {
//                    outString += aDateRange.Text + ", ";
//                }
//                // BEGIN MID Track #4347 - change condition verbage		
//                //else
//                //{
//                //	outString += "Current, ";
//                //}
				
//                //switch ((eFilterTimeModifyer)((ComboObject)aTimeMod.SelectedItem).Key)
//                //{
//                //	case eFilterTimeModifyer.Any :
//                //		outString += "Any, ";
//                //		break;
//                //	case eFilterTimeModifyer.All :
//                //		outString += "All, ";
//                //		break;
//                //	case eFilterTimeModifyer.Average :
//                //		outString += "Average, ";
//                //		break;
//                //	case eFilterTimeModifyer.Total :
//                //		outString += "Total, ";
//                //		break;
//                //	case eFilterTimeModifyer.Corresponding :
//                //		outString += "Corresponding, ";
//                //		break;
//                //	default:
//                //		outString += "Any, ";
//                //		break;
//                //}
				
//                //switch ((eFilterCubeModifyer)((ComboObject)aCubeMod.SelectedItem).Key)
//                //{
//                //	case eFilterCubeModifyer.StoreDetail :
//                //		outString += "Store Detail)";
//                //		break;
//                //	case eFilterCubeModifyer.StoreAverage :
//                //		outString += "Store Average)";
//                //		break;
//                //	case eFilterCubeModifyer.StoreTotal :
//                //		outString += "Store Total)";
//                //		break;
//                //	case eFilterCubeModifyer.ChainDetail :
//                //		outString += "Chain Detail)";
//                //		break;
//                //	default:
//                //		outString += "Store Detail)";
//                //		break;
//                //}
//                switch ((eFilterCubeModifyer)((ComboObject)aCubeMod.SelectedItem).Key)
//                {
//                    case eFilterCubeModifyer.StoreDetail :
//                        outString += "Store Detail ,";
//                        break;
//                    case eFilterCubeModifyer.StoreAverage :
//                        outString += "Store Average ,";
//                        break;
//                    case eFilterCubeModifyer.StoreTotal :
//                        outString += "Store Total,";
//                        break;
//                    case eFilterCubeModifyer.ChainDetail :
//                        outString += "Chain Detail ,";
//                        break;
//                    default:
//                        outString += "Store Detail ,";
//                        break;
//                }
//                switch ((eFilterTimeModifyer)((ComboObject)aTimeMod.SelectedItem).Key)
//                {
//                    case eFilterTimeModifyer.Any :
//                        outString += " Any )";
//                        break;
//                    case eFilterTimeModifyer.All :
//                        outString += " All )";
//                        break;
////Begin Track #5111 - JScott - Add additional filter functionality
//                    case eFilterTimeModifyer.Join :
//                        outString += " Join )";
//                        break;
////End Track #5111 - JScott - Add additional filter functionality
//                    case eFilterTimeModifyer.Average :
//                        outString += " Average )";
//                        break;
//                    case eFilterTimeModifyer.Total :
//                        outString += " Total )";
//                        break;
//                    case eFilterTimeModifyer.Corresponding :
//                        outString += " Corresponding )";
//                        break;
//                    default:
//                        outString += " Any )";
//                        break;
//                }

//                // END MID Track #4347
//                return outString;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        // Begin TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
//        //private string FormatComparisonTypeText(ComboBox aCondition)
//        private string FormatComparisonTypeText(MIDComboBoxEnh aCondition)
//        // End TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
//        {
//            string outString = "";

//            try
//            {
//                switch ((eFilterComparisonType)((ComboObject)aCondition.SelectedItem).Key)
//                {
//                    // BEGIN MID Track #4347 - change condition verbage	
//                    case eFilterComparisonType.Equal :
//                        //outString = "Equal To ";
//                        outString = "= ";
//                        break;
//                    case eFilterComparisonType.Greater :
//                        //outString = "Greater Than ";
//                        outString = "> ";
//                        break;
//                    case eFilterComparisonType.GreaterEqual :
//                        //outString = "Greater Than or Equal To ";
//                        outString = ">= ";
//                        break;
//                    case eFilterComparisonType.Less :
//                        //outString = "Less Than ";
//                        outString = "< ";
//                        break;
//                    case eFilterComparisonType.LessEqual :
//                        //outString = "Less Than or Equal To ";
//                        outString = "<= ";
//                        break;
//                    case eFilterComparisonType.NotEqual :
//                        //outString = "Not Equal To ";
//                        outString = "NOT = ";
//                        break;
//                    case eFilterComparisonType.NotGreater :
//                        //outString = "Not Greater Than ";
//                        outString = "NOT > ";
//                        break;
//                    case eFilterComparisonType.NotGreaterEqual :
//                        //outString = "Not Greater Than or Equal To ";
//                        outString = "NOT >= ";
//                        break;
//                    case eFilterComparisonType.NotLess :
//                        //outString = "Not Less Than ";
//                        outString = "NOT < ";
//                        break;
//                    case eFilterComparisonType.NotLessEqual :
//                        //outString = "Not Less Than or Equal To ";
//                        outString = "NOT <= ";
//                        break;
//                    // END MID Track #4347  
//                }

//                return outString;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void SetOpenImage(MIDStoreNode aStoreNode)
//        {
//            try
//            {
//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //if (aStoreNode.StoreNodeType == eStoreNodeType.group)
//                if (aStoreNode.NodeProfileType == eProfileType.StoreGroup)
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                {
//                    if (aStoreNode.isDynamic)
//                    {
//                        aStoreNode.ImageIndex = cOpenDynamicFolderImage;
//                        aStoreNode.SelectedImageIndex = cOpenDynamicFolderImage;
//                    }
//                    else
//                    {
//                        aStoreNode.ImageIndex = cOpenFolderImage;
//                        aStoreNode.SelectedImageIndex = cOpenFolderImage;
//                    }
//                }
//                else
//                {
//                    aStoreNode.ImageIndex = cOpenFolderImage;
//                    aStoreNode.SelectedImageIndex = cOpenFolderImage;
//                }

//                aStoreNode.Expanded = true;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void SetClosedImage(MIDStoreNode aStoreNode)
//        {
//            try
//            {
//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //if (aStoreNode.StoreNodeType == eStoreNodeType.group)
//                if (aStoreNode.NodeProfileType == eProfileType.StoreGroup)
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                {
//                    if (aStoreNode.isDynamic)
//                    {
//                        aStoreNode.ImageIndex = cClosedDynamicFolderImage;
//                        aStoreNode.SelectedImageIndex = cClosedDynamicFolderImage;
//                    }
//                    else
//                    {
//                        aStoreNode.ImageIndex = cClosedFolderImage;
//                        aStoreNode.SelectedImageIndex = cClosedFolderImage;
//                    }
//                }
//                else
//                {
//                    aStoreNode.ImageIndex = cClosedFolderImage;
//                    aStoreNode.SelectedImageIndex = cClosedFolderImage;
//                }

//                aStoreNode.Expanded = false;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        public MIDStoreNode CreateAttributesNode(StoreGroupProfile aStoreGroupProf)
//        {
//            MIDStoreNode storeNode;

//            try
//            {
//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //storeNode = new MIDStoreNode(SAB, aStoreGroupProf.Key);
//                if (aStoreGroupProf.IsDynamicGroup)
//                {
//                    storeNode = new MIDStoreGroupNode(
//                        SAB,
//                        eTreeNodeType.ObjectNode,
//                        aStoreGroupProf,
//                        aStoreGroupProf.Name,
//                        Include.NoRID,
//                        aStoreGroupProf.OwnerUserRID,
//                        null,
//                        cClosedDynamicFolderImage,
//                        cClosedDynamicFolderImage,
//                        aStoreGroupProf.OwnerUserRID,
//                        aStoreGroupProf.IsDynamicGroup);
//                }
//                else
//                {
//                    storeNode = new MIDStoreGroupNode(
//                        SAB,
//                        eTreeNodeType.ObjectNode,
//                        aStoreGroupProf,
//                        aStoreGroupProf.Name,
//                        Include.NoRID,
//                        aStoreGroupProf.OwnerUserRID,
//                        null,
//                        cClosedFolderImage,
//                        cClosedFolderImage,
//                        aStoreGroupProf.OwnerUserRID,
//                        aStoreGroupProf.IsDynamicGroup);
//                }
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //storeNode.GroupRID = aStoreGroupProf.Key;
//                //storeNode.Text = aStoreGroupProf.Name;
//                //storeNode.StoreNodeType = eStoreNodeType.group;
//                //storeNode.NodeDescription = aStoreGroupProf.Name;
//                //storeNode.NodeChangeType = eChangeType.none;
//                //storeNode.NodeLevel = 1;
//                //storeNode.NodeColor = "";
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //if (aStoreGroupProf.IsDynamicGroup)
//                //{
//                //    storeNode.ImageIndex = cClosedDynamicFolderImage;
//                //    storeNode.SelectedImageIndex = cClosedDynamicFolderImage;
//                //    storeNode.IsDynamic = true;
//                //}
//                //else
//                //{
//                //    storeNode.ImageIndex = cClosedFolderImage;
//                //    storeNode.SelectedImageIndex = cClosedFolderImage;
//                //    storeNode.IsDynamic = false;
//                //}

//                //storeNode.HasChildren = true;
//                //storeNode.ChildrenLoaded = true;
//                //storeNode.DynamicChildrenSql = "";
//                //storeNode.GroupLevelSequenceNo = 0;
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                storeNode.Tag = new AttributeNodeTag();

//                return storeNode;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private MIDStoreNode CreateAttributesLevelNode(MIDStoreNode aGroupNode, StoreGroupLevelProfile aStoreGroupLevelProf)
//        {
//            MIDStoreNode storeNode;

//            try
//            {
//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //storeNode = new MIDStoreNode(SAB, aGroupNode.GroupRID);
//                storeNode = new MIDStoreGroupLevelNode(
//                    SAB,
//                    eTreeNodeType.ObjectNode,
//                    aStoreGroupLevelProf,
//                    aStoreGroupLevelProf.Name,
//                    aGroupNode.Profile.Key,
//                    aGroupNode.OwnerUserRID,
//                    aStoreGroupLevelProf.Sequence,
//                    null,
//                    cClosedFolderImage,
//                    cClosedFolderImage,
//                    aGroupNode.OwnerUserRID,
//                    aGroupNode.isDynamic);
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //storeNode.GroupRID = aGroupNode.GroupRID;
//                //storeNode.GroupLevelRID = aStoreGroupLevelProf.Key;
//                //storeNode.Text = aStoreGroupLevelProf.Name;
//                //storeNode.StoreNodeType = eStoreNodeType.groupLevel;
//                //storeNode.NodeDescription = aStoreGroupLevelProf.Name;
//                //storeNode.NodeChangeType = eChangeType.none;
//                //storeNode.NodeLevel = 2;
//                //storeNode.NodeColor = "";
//                //storeNode.ImageIndex = cClosedFolderImage;
//                //storeNode.SelectedImageIndex = cClosedFolderImage;
//                //storeNode.HasChildren = true;
//                //storeNode.ChildrenLoaded = false;
//                //storeNode.IsDynamic = false;
//                //storeNode.GroupLevelSequenceNo = aStoreGroupLevelProf.Sequence;
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                return storeNode;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private MIDStoreNode CreateStoreNode(StoreProfile aStoreProf)
//        {	
//            MIDStoreNode storeNode;

//            try
//            {
//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //storeNode = new MIDStoreNode(SAB, aStoreProf.Key);
//                if (aStoreProf.DynamicStore)
//                {
//                    storeNode = new MIDStoreNode(
//                        SAB,
//                        eTreeNodeType.ObjectNode,
//                        aStoreProf,
//                        aStoreProf.Text,
//                        Include.NoRID,
//                        Include.NoRID,
//                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
//                        //null,
//                        new MIDTreeNodeSecurityGroup(null, null),
//                        //End Track #6321 - JScott - User has ability to to create folders when security is view
//                        cDynamicStoreImage,
//                        cSelectedStoreImage,
//                        Include.NoRID,
//                        aStoreProf.DynamicStore);
//                }
//                else
//                {
//                    storeNode = new MIDStoreNode(
//                        SAB,
//                        eTreeNodeType.ObjectNode,
//                        aStoreProf,
//                        aStoreProf.Text,
//                        Include.NoRID,
//                        Include.NoRID,
//                        //Begin Track #6321 - JScott - User has ability to to create folders when security is view
//                        //null,
//                        new MIDTreeNodeSecurityGroup(null, null),
//                        //End Track #6321 - JScott - User has ability to to create folders when security is view
//                        cStaticStoreImage,
//                        cSelectedStoreImage,
//                        Include.NoRID,
//                        aStoreProf.DynamicStore);
//                }
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //storeNode.GroupRID = 0;
//                //storeNode.GroupLevelRID = 0;
//                //storeNode.StoreRID = aStoreProf.Key;
//                //storeNode.Text = aStoreProf.Text;
//                //storeNode.StoreNodeType = eStoreNodeType.store;
//                //storeNode.NodeDescription = aStoreProf.Text;
//                //storeNode.NodeChangeType = eChangeType.none;
//                //storeNode.NodeLevel = 1;
//                //storeNode.NodeColor = "";
//                //storeNode.SelectedImageIndex = cSelectedStoreImage;
//                //storeNode.HasChildren = false;
//                //storeNode.ChildrenLoaded = true;
//                //storeNode.DynamicChildrenSql = "";
//                //storeNode.GroupLevelSequenceNo = 0;
//                //storeNode.IsDynamic = aStoreProf.DynamicStore;
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //if (storeNode.IsDynamic)
//                //{
//                //    storeNode.ImageIndex = cDynamicStoreImage;
//                //}
//                //else
//                //{
//                //    storeNode.ImageIndex = cStaticStoreImage;
//                //}

//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                return storeNode;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void SetIntroNextButton()
//        {
//            try
//            {
//                if (chkSets.Checked || chkData.Checked)
//                {
//                    btnNext.Enabled = true;
//                }
//                else
//                {
//                    btnNext.Enabled = false;
//                }

//                pnlIntro.IsNextEnabled = btnNext.Enabled;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void SetNameNextButton()
//        {
//            try
//            {
//                if (txtName.Text.Length > 0)
//                {
//                    btnNext.Enabled = true;
//                }
//                else
//                {
//                    btnNext.Enabled = false;
//                }

//                pnlNameMain.IsNextEnabled = btnNext.Enabled;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private MIDStoreNode GetParentNode(MIDStoreNode aNode)
//        {
//            try
//            {
//                if (aNode != null)
//                {
//                    while (aNode.Parent != null)
//                    {
//                        aNode = (MIDStoreNode)aNode.Parent;
//                    }
//                }

//                return aNode;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void cboVar1Variable_SelectionChangeCommitted(object sender, System.EventArgs e)
//        {
//            try
//            {
//                _currPanel.IsNextEnabled = true;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void DeleteCurrentNode(TreeView aTreeView)
//        {
//            TreeNode currNode;

//            try
//            {
//                if (aTreeView.SelectedNode != null)
//                {
//                    aTreeView.Nodes.Remove(aTreeView.SelectedNode);
//                    currNode = aTreeView.SelectedNode;
//                    aTreeView.SelectedNode = null;
//                    aTreeView.SelectedNode = currNode;
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//    }

//    /// <summary>
//    /// Class that defines the tag class for an Attribute TreeNode
//    /// </summary>

//    public class AttributeNodeTag
//    {
//        private Type _conjunctionType;

//        public AttributeNodeTag()
//        {
//            _conjunctionType = typeof(GenericQueryOrOperand);
//        }

//        public Type ConjunctionType
//        {
//            get
//            {
//                return _conjunctionType;
//            }
//            set
//            {
//                _conjunctionType = value;
//            }
//        }
//    }

//    /// <summary>
//    /// Class that defines the tag class for an Condition TreeNode
//    /// </summary>

//    public class ConditionNodeTag
//    {
//        private FilterWizardPanel _conditionPanel;
//        private Type _conjunctionType;

//        public ConditionNodeTag(FilterWizardPanel aConditionPanel)
//        {
//            _conditionPanel = aConditionPanel;
//            _conjunctionType = typeof(GenericQueryOrOperand);
//        }

//        public FilterWizardPanel ConditionPanel
//        {
//            get
//            {
//                return _conditionPanel;
//            }
//        }

//        public Type ConjunctionType
//        {
//            get
//            {
//                return _conjunctionType;
//            }
//            set
//            {
//                _conjunctionType = value;
//            }
//        }
//    }

//    /// <summary>
//    /// Class that defines the type object stored in a ComboBox list.
//    /// </summary>

//    public class TypeComboObject : ComboObject
//    {
//        private Type _operandType;

//        public TypeComboObject(int aKey, string aValue, Type aOperandType)
//            : base(aKey, aValue)
//        {
//            _operandType = aOperandType;
//        }

//        public Type OperandType
//        {
//            get
//            {
//                return _operandType;
//            }
//        }
//    }

//    /// <summary>
//    /// Class that defines the comparison object stored in a ComboBox list.
//    /// </summary>

//    public class ComparisonComboObject : TypeComboObject
//    {
//        private bool _negate;

//        public ComparisonComboObject(int aKey, string aValue, Type aOperandType, bool aNegate)
//            : base(aKey, aValue, aOperandType)
//        {
//            _negate = aNegate;
//        }

//        public bool Negate
//        {
//            get
//            {
//                return _negate;
//            }
//        }
//    }

//    /// <summary>
//    /// Class that defines the Profile object stored in a ComboBox list.
//    /// </summary>

//    public class ProfileComboObject : ComboObject
//    {
//        private Profile _profile;

//        public ProfileComboObject(int aKey, string aValue, Profile aProfile)
//            : base(aKey, aValue)
//        {
//            _profile = aProfile;
//        }

//        public Profile Profile
//        {
//            get
//            {
//                return _profile;
//            }
//        }
//    }

//    /// <summary>
//    /// Class that defines the TimeTotal object stored in a ComboBox list.
//    /// </summary>

//    public class TimeTotalComboObject : ProfileComboObject
//    {
//        private Profile _varProfile;
//        private int _timeTotalIdx;

//        public TimeTotalComboObject(int aKey, string aValue, Profile aTimeTotVarProfile, Profile aVarProfile, int aTimeTotalIdx)
//            : base(aKey, aValue, aTimeTotVarProfile)
//        {
//            _varProfile = aVarProfile;
//            _timeTotalIdx = aTimeTotalIdx;
//        }

//        public Profile VarProfile
//        {
//            get
//            {
//                return _varProfile;
//            }
//        }

//        public int TimeTotalIdx
//        {
//            get
//            {
//                return _timeTotalIdx;
//            }
//        }
//    }
//}
