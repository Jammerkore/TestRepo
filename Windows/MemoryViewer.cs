using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for MemoryViewer.
	/// </summary>
	public class frmMemoryViewer : MIDFormBase
	{
		private SessionAddressBlock _SAB;
		private char[] _separators;

        // memory tracking
        static private Process _firstProcess = null;
        static private long _firstTotalMemory;
        static private Process _lastProcess = null;
        static private long _lastTotalMemory;

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnHierarchyRefresh;
		private System.Windows.Forms.Button btnStoreRefresh;
		private System.Windows.Forms.ListBox lbHierarchyMemory;
		private System.Windows.Forms.ListBox lbStoreMemory;
		private System.Windows.Forms.Button btnWrite;
        private ListBox lbMyComputerMemory;
        private Button btnMyComputerRefresh;
        private Label label3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmMemoryViewer(SessionAddressBlock aSAB) : base (aSAB)
		{
			_SAB = aSAB;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnHierarchyRefresh = new System.Windows.Forms.Button();
            this.btnStoreRefresh = new System.Windows.Forms.Button();
            this.lbHierarchyMemory = new System.Windows.Forms.ListBox();
            this.lbStoreMemory = new System.Windows.Forms.ListBox();
            this.btnWrite = new System.Windows.Forms.Button();
            this.lbMyComputerMemory = new System.Windows.Forms.ListBox();
            this.btnMyComputerRefresh = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(839, 371);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(48, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Hierarchy Service Memory";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(368, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Store Service Memory";
            // 
            // btnHierarchyRefresh
            // 
            this.btnHierarchyRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHierarchyRefresh.Location = new System.Drawing.Point(120, 328);
            this.btnHierarchyRefresh.Name = "btnHierarchyRefresh";
            this.btnHierarchyRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnHierarchyRefresh.TabIndex = 5;
            this.btnHierarchyRefresh.Text = "Refresh";
            this.btnHierarchyRefresh.Click += new System.EventHandler(this.btnHierarchyRefresh_Click);
            // 
            // btnStoreRefresh
            // 
            this.btnStoreRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStoreRefresh.Location = new System.Drawing.Point(432, 328);
            this.btnStoreRefresh.Name = "btnStoreRefresh";
            this.btnStoreRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnStoreRefresh.TabIndex = 6;
            this.btnStoreRefresh.Text = "Refresh";
            this.btnStoreRefresh.Click += new System.EventHandler(this.btnStoreRefresh_Click);
            // 
            // lbHierarchyMemory
            // 
            this.lbHierarchyMemory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lbHierarchyMemory.Location = new System.Drawing.Point(16, 56);
            this.lbHierarchyMemory.Name = "lbHierarchyMemory";
            this.lbHierarchyMemory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbHierarchyMemory.Size = new System.Drawing.Size(296, 264);
            this.lbHierarchyMemory.TabIndex = 7;
            this.lbHierarchyMemory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbHierarchyMemory_KeyDown);
            // 
            // lbStoreMemory
            // 
            this.lbStoreMemory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lbStoreMemory.Location = new System.Drawing.Point(320, 56);
            this.lbStoreMemory.Name = "lbStoreMemory";
            this.lbStoreMemory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbStoreMemory.Size = new System.Drawing.Size(296, 264);
            this.lbStoreMemory.TabIndex = 8;
            // 
            // btnWrite
            // 
            this.btnWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnWrite.Location = new System.Drawing.Point(759, 371);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 9;
            this.btnWrite.Text = "Write";
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // lbMyComputerMemory
            // 
            this.lbMyComputerMemory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lbMyComputerMemory.Location = new System.Drawing.Point(622, 56);
            this.lbMyComputerMemory.Name = "lbMyComputerMemory";
            this.lbMyComputerMemory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbMyComputerMemory.Size = new System.Drawing.Size(296, 264);
            this.lbMyComputerMemory.TabIndex = 12;
            // 
            // btnMyComputerRefresh
            // 
            this.btnMyComputerRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMyComputerRefresh.Location = new System.Drawing.Point(734, 328);
            this.btnMyComputerRefresh.Name = "btnMyComputerRefresh";
            this.btnMyComputerRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnMyComputerRefresh.TabIndex = 11;
            this.btnMyComputerRefresh.Text = "Refresh";
            this.btnMyComputerRefresh.Click += new System.EventHandler(this.btnMyComputerRefresh_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(670, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "My Computer Memory";
            // 
            // frmMemoryViewer
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(930, 406);
            this.Controls.Add(this.lbMyComputerMemory);
            this.Controls.Add(this.btnMyComputerRefresh);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.lbStoreMemory);
            this.Controls.Add(this.lbHierarchyMemory);
            this.Controls.Add(this.btnStoreRefresh);
            this.Controls.Add(this.btnHierarchyRefresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Name = "frmMemoryViewer";
            this.Text = "Memory Viewer";
            this.Load += new System.EventHandler(this.MemoryViewer_Load);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.btnHierarchyRefresh, 0);
            this.Controls.SetChildIndex(this.btnStoreRefresh, 0);
            this.Controls.SetChildIndex(this.lbHierarchyMemory, 0);
            this.Controls.SetChildIndex(this.lbStoreMemory, 0);
            this.Controls.SetChildIndex(this.btnWrite, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.btnMyComputerRefresh, 0);
            this.Controls.SetChildIndex(this.lbMyComputerMemory, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		private void MemoryViewer_Load(object sender, System.EventArgs e)
		{
			SetText();
			_separators = new char[System.Environment.NewLine.Length];
			for (int i=0; i<System.Environment.NewLine.Length; i++)
			{
				_separators[i] = System.Environment.NewLine[i];
			}
            SetInitialMemoryCounts();
			AddToListBox(lbHierarchyMemory, _SAB.HierarchyServerSession.ShowMemory(), false);
			AddToListBox(lbStoreMemory, _SAB.StoreServerSession.ShowMemory(), false);
            AddToListBox(lbMyComputerMemory, ShowMemory(), false);
		}

        private void SetInitialMemoryCounts()
        {
            try
            {
                System.GC.Collect();
                _firstProcess = System.Diagnostics.Process.GetCurrentProcess();
                _firstTotalMemory = System.GC.GetTotalMemory(true);
                _lastProcess = System.Diagnostics.Process.GetCurrentProcess();
                _lastTotalMemory = System.GC.GetTotalMemory(true);
            }
            catch
            {
                throw;
            }
        }

		private void SetText()
		{
			try
			{
				this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void btnHierarchyRefresh_Click(object sender, System.EventArgs e)
		{
			AddToListBox(lbHierarchyMemory, _SAB.HierarchyServerSession.ShowMemory(), true);
		}

		private void btnStoreRefresh_Click(object sender, System.EventArgs e)
		{
			AddToListBox(lbStoreMemory, _SAB.StoreServerSession.ShowMemory(), true);
		}

        private void btnMyComputerRefresh_Click(object sender, System.EventArgs e)
        {
            AddToListBox(lbMyComputerMemory, ShowMemory(), true);
        }

		private void AddToListBox(ListBox aListBox, string aMemoryInfo, bool aAddSeparator)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				if (aAddSeparator)
				{
					aListBox.Items.Add("----------------------------");
				}
				aListBox.Items.Add(DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToLongTimeString());
				string[] lines = aMemoryInfo.Split(_separators);
				if (lines != null)
				{
					for (int i=0; i<lines.Length; i++)
					{
						if (lines[i].Trim().Length > 0)
						{
							aListBox.Items.Add(lines[i]);
						}
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

        public string ShowMemory()
        {
            try
            {
                try
                {
                    System.GC.Collect();
                    bool monitorMemory = false;
                    string strMonitor = MIDConfigurationManager.AppSettings["MonitorMemory"];
                    if (strMonitor != null)
                    {
                        try
                        {
                            monitorMemory = Convert.ToBoolean(strMonitor);
                        }
                        catch
                        {
                        }

                    }
                    string memory = string.Empty;
                    long totalMemory = System.GC.GetTotalMemory(true);
                    Process process = System.Diagnostics.Process.GetCurrentProcess();
                    if (_lastProcess == null)
                    {
                        memory = "TotalMemorySize=" + totalMemory.ToString() + System.Environment.NewLine
                            + "ProcessName=" + process.ProcessName.ToString() + System.Environment.NewLine
                            + "VirtualMemorySize=" + process.VirtualMemorySize64.ToString() + System.Environment.NewLine
                            + "WorkingSet=" + process.WorkingSet64.ToString() + System.Environment.NewLine
                            + "PagedMemorySize=" + process.PagedMemorySize64.ToString() + System.Environment.NewLine
                            + "PagedSystemMemorySize=" + process.PagedSystemMemorySize64.ToString() + System.Environment.NewLine
                            + "PrivateMemorySize=" + process.PrivateMemorySize64.ToString() + System.Environment.NewLine
                            + "PeakWorkingSet=" + process.PeakWorkingSet64.ToString() + System.Environment.NewLine
                            + "PeakPagedMemorySize=" + process.PeakPagedMemorySize64.ToString() + System.Environment.NewLine
                            + "PeakVirtualMemorySize=" + process.PeakVirtualMemorySize64.ToString();
                    }
                    else
                    {
                        memory = "TotalMemorySize=" + totalMemory.ToString() + "(" + ((long)(totalMemory - _lastTotalMemory)).ToString() + ")" + "(" + ((long)(totalMemory - _firstTotalMemory)).ToString() + ")" + System.Environment.NewLine
                            + "ProcessName=" + process.ProcessName.ToString() + System.Environment.NewLine
                            + "VirtualMemorySize=" + process.VirtualMemorySize64.ToString() + "(" + ((int)(process.VirtualMemorySize64 - _lastProcess.VirtualMemorySize64)).ToString() + ")" + "(" + ((int)(process.VirtualMemorySize64 - _firstProcess.VirtualMemorySize64)).ToString() + ")" + System.Environment.NewLine
                            + "WorkingSet=" + process.WorkingSet64.ToString() + "(" + ((int)(process.WorkingSet64 - _lastProcess.WorkingSet64)).ToString() + ")" + "(" + ((int)(process.WorkingSet64 - _firstProcess.WorkingSet64)).ToString() + ")" + System.Environment.NewLine
                            + "PagedMemorySize=" + process.PagedMemorySize64.ToString() + "(" + ((int)(process.PagedMemorySize64 - _lastProcess.PagedMemorySize64)).ToString() + ")" + "(" + ((int)(process.PagedMemorySize64 - _firstProcess.PagedMemorySize64)).ToString() + ")" + System.Environment.NewLine
                            + "PagedSystemMemorySize=" + process.PagedSystemMemorySize64.ToString() + "(" + ((int)(process.PagedSystemMemorySize64 - _lastProcess.PagedSystemMemorySize64)).ToString() + ")" + "(" + ((int)(process.PagedSystemMemorySize64 - _firstProcess.PagedSystemMemorySize64)).ToString() + ")" + System.Environment.NewLine
                            + "PrivateMemorySize=" + process.PrivateMemorySize64.ToString() + "(" + ((int)(process.PrivateMemorySize64 - _lastProcess.PrivateMemorySize64)).ToString() + ")" + "(" + ((int)(process.PrivateMemorySize64 - _firstProcess.PrivateMemorySize64)).ToString() + ")" + System.Environment.NewLine
                            + "PeakWorkingSet=" + process.PeakWorkingSet64.ToString() + "(" + ((int)(process.PeakWorkingSet64 - _lastProcess.PeakWorkingSet64)).ToString() + ")" + "(" + ((int)(process.PeakWorkingSet64 - _firstProcess.PeakWorkingSet64)).ToString() + ")" + System.Environment.NewLine
                            + "PeakPagedMemorySize=" + process.PeakPagedMemorySize64.ToString() + "(" + ((int)(process.PeakPagedMemorySize64 - _lastProcess.PeakPagedMemorySize64)).ToString() + ")" + "(" + ((int)(process.PeakPagedMemorySize64 - _firstProcess.PeakPagedMemorySize64)).ToString() + ")" + System.Environment.NewLine
                            + "PeakVirtualMemorySize=" + process.PeakVirtualMemorySize64.ToString() + "(" + ((int)(process.PeakVirtualMemorySize64 - _lastProcess.PeakVirtualMemorySize64)).ToString() + ")" + "(" + ((int)(process.PeakVirtualMemorySize64 - _firstProcess.PeakVirtualMemorySize64)).ToString() + ")";
                    }
                    if (monitorMemory)
                    {
                        EventLog.WriteEntry("MIDService", memory, EventLogEntryType.Information);
                    }
                    _lastProcess = process;
                    _lastTotalMemory = totalMemory;
                    return memory;
                }
                catch
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

		private void lbHierarchyMemory_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A && e.Control) 
			{
				SelectAll(lbHierarchyMemory);
			}
		}

		private void SelectAll(ListBox aListBox)
		{
//			for (int i=0; i<aListBox.Items.Count; i++)
//			{
//				aListBox.Items[i].
//			}
		}

		private void btnWrite_Click(object sender, System.EventArgs e)
		{
			System.IO.StreamWriter memoryFile = null;
			bool isOpen = false;
			string fileName;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				fileName = System.Environment.CurrentDirectory + "/MIDMemory.txt";
				memoryFile = new System.IO.StreamWriter(fileName, false);
				isOpen = true;
				memoryFile.WriteLine("-- Hierarchy Memory --");
				WriteAll(lbHierarchyMemory, memoryFile);
				memoryFile.WriteLine(" ");
				memoryFile.WriteLine("-- Store Memory --");
				WriteAll(lbStoreMemory, memoryFile);
                memoryFile.WriteLine(" ");
                memoryFile.WriteLine("-- My Computer Memory --");
                WriteAll(lbMyComputerMemory, memoryFile);
				MessageBox.Show("Memory statistics have been written to " + fileName);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (memoryFile != null &&
					isOpen)
				{
					memoryFile.Close();
				}
				Cursor.Current = Cursors.Default;
			}
		}

		private void WriteAll(ListBox aListBox, System.IO.StreamWriter aFileName)
		{
			for (int i=0; i<aListBox.Items.Count; i++)
			{
				aFileName.WriteLine(aListBox.Items[i].ToString());
			}
		}
	}
}
