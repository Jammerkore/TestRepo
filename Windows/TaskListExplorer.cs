using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public partial class TaskListExplorer : ExplorerBase
	{
		//=======
		// FIELDS
		//=======

		private System.Windows.Forms.ToolStripMenuItem cmiSchedule;

		//=============
		// CONSTRUCTORS
		//=============

		public TaskListExplorer(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, Form aMainMDIForm)
			: base(aSAB, aEAB, aMainMDIForm)
		{
			aEAB.TaskListExplorer = this;
		}

		//========
		// METHODS
		//========

		//------------------
		// Virtual overrides
		//------------------

		/// <summary>
		/// Virtual method that is called to initialize the ExplorerBase TreeView
		/// </summary>

		protected override void InitializeExplorer()
		{
			try
			{
				base.InitializeExplorer();

				InitializeComponent();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Virtual method that is called to initialize the ExplorerBase TreeView
		/// </summary>

		override protected void InitializeTreeView()
		{
			try
			{
				TreeView = new TaskListTreeView();
				TreeView.InitializeTreeView(SAB, false, MainMDIForm);

				TreeView.AllowDrop = true;
				TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
				TreeView.ImageList = this.imageListTaskList;
				TreeView.LabelEdit = true;
				TreeView.Location = new System.Drawing.Point(0, 0);
				TreeView.Name = "TreeView";
				TreeView.PathSeparator = ".";
				TreeView.Size = new System.Drawing.Size(216, 352);
				TreeView.TabIndex = 0;

				TreeView.OnMIDNodeSelect += new MIDTreeView.MIDTreeViewNodeSelectHandler(this.TaskListTreeView_OnMIDNodeSelect);
				TreeView.OnMIDDoubleClick += new MIDTreeView.MIDTreeViewDoubleClickHandler(this.MIDTreeView_OnMIDDoubleClick);

				Controls.Add(this.TreeView);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Virtual method that is called to perform Form Load tasks
		/// </summary>

		override protected void ExplorerLoad()
		{
			try
			{
				TreeView.ImageList = MIDGraphics.ImageList;

				_cutCopyOperation = eCutCopyOperation.None;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Virtual method that is called to build the ExplorerBase TreeView
		/// </summary>

		override protected void BuildTreeView()
		{
			try
			{
                // Begin TT#2 - JSmith - Assortment Security
                if (SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersTasklist).AccessDenied)
                {
                    return;
                }
                // End TT#2

				BuildContextmenu();
				TreeView.LoadNodes();
				((TaskListTreeView)TreeView).InitialExpand();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Virtual method that is called to refresh the ExplorerBase TreeView
		/// </summary>

		override protected void RefreshTreeView()
		{
			try
			{
				TreeView.LoadNodes();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Virtual method that gets the text for the New Item menu item.
		/// </summary>
		/// <returns>
		/// The text for the New Item menu item.
		/// </returns>

		override protected string GetNewItemText(MIDTreeNode aCurrentNode)
		{
			if (aCurrentNode.GetTopSourceNode().NodeProfileType == eProfileType.TaskListJobMainFolder)
			{
				return "Job";
			}
			else if (aCurrentNode.GetTopSourceNode().NodeProfileType == eProfileType.TaskListSpecialRequestMainFolder)
			{
				return "Special Request";
			}
			else
			{
				return "Task List";
			}
		}

		protected override void CustomizeActionMenu(MIDTreeNode aNode)
		{
			try
			{
				if (aNode.NodeProfileType == eProfileType.TaskList ||
					aNode.NodeProfileType == eProfileType.Job)
				{
					cmiSchedule.Visible = true;
				}
				else
				{
					cmiSchedule.Visible = false;
				}

				if (SAB.SchedulerServerSession == null)
				{
					cmiSchedule.Enabled = false;
				}
				else
				{
					cmiSchedule.Enabled = true;
				}

                //Begin TT#110-MD - JSmith - In Use Tool
                if (aNode.NodeProfileType == eProfileType.SpecialRequest)
                {
                    CustomizeActionMenuItem(eExplorerActionMenuItem.InUse, false);
                }
                //End TT#110-MD - JSmith - In Use Tool

                // Begin TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.
                if (aNode.TreeNodeType == eTreeNodeType.ObjectNode)
                {
                    ScheduleData dlSchedule = new ScheduleData();
                    DataTable dtTask = dlSchedule.Task_ReadByTaskList(aNode.Profile.Key);
                    foreach (DataRow dr in dtTask.Rows)
                    {
                        if (!((TaskListTreeView)this.TreeView).AvailableTasks.ContainsKey(Convert.ToInt32(dr["TASK_TYPE"])))
                        {
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Copy, false);
                        }

                    }
                }
                // End TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.

                // Begin TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer
                if (((MIDTaskListNode)aNode).isSharedNode)
                {
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewFolder, false);
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewItem, false);
                }
                // End TT#4226 - JSmith - Invalid menu option on shared user folder in Task List Explorer

			}
			catch
			{
				throw;
			}
		}

		//----------------
		// Private methods
		//----------------

		private void BuildContextmenu()
		{
			try
			{
				cmiSchedule = new System.Windows.Forms.ToolStripMenuItem();
				cmiSchedule.Name = "cmiSchedule";
				cmiSchedule.Size = new System.Drawing.Size(195, 22);
				cmiSchedule.Text = "Schedule...";
				cmiSchedule.Click += new System.EventHandler(this.cmiSchedule_Click);
				AddContextMenuItem(cmiSchedule, eExplorerActionMenuItem.None, eExplorerActionMenuItem.RefreshSeparator);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void cmiSchedule_Click(object sender, System.EventArgs e)
		{
			FunctionSecurityProfile securityProf;
			MIDTaskListNode taskListNode;
			frmScheduleProperties schedProperties;

			try
			{
				if (TreeView.SelectedNode != null)
				{
					taskListNode = (MIDTaskListNode)TreeView.SelectedNode;

					if (taskListNode.NodeProfileType == eProfileType.TaskList)
					{
						securityProf = new FunctionSecurityProfile(0);
						securityProf.SetAllowUpdate();
						schedProperties = new frmScheduleProperties(SAB, securityProf, (TaskListProfile)taskListNode.Profile);
						schedProperties.OnSchedulePropertiesSaveHandler += new frmScheduleProperties.SchedulePropertiesSaveEventHandler(OnSchedulePropertiesSave);

						if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
						{
							schedProperties.MdiParent = this.ParentForm;
						}
						else
						{
							schedProperties.MdiParent = this.ParentForm.Owner;
						}

						schedProperties.Show();
						schedProperties.BringToFront();
					}
					else if (taskListNode.NodeProfileType == eProfileType.Job)
					{
						securityProf = new FunctionSecurityProfile(0);
						securityProf.SetAllowUpdate();
						schedProperties = new frmScheduleProperties(SAB, securityProf, (JobProfile)taskListNode.Profile);
						schedProperties.OnSchedulePropertiesSaveHandler += new frmScheduleProperties.SchedulePropertiesSaveEventHandler(OnSchedulePropertiesSave);

						if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
						{
							schedProperties.MdiParent = this.ParentForm;
						}
						else
						{
							schedProperties.MdiParent = this.ParentForm.Owner;
						}

						schedProperties.Show();
						schedProperties.BringToFront();
					}
				}
			}
			catch (Exception error)
			{
				HandleException(error);
			}
		}

		protected void TaskListTreeView_OnMIDNodeSelect(object source, MIDTreeNode node)
		{
			frmScheduleBrowser schedBrowser;

			try
			{
				if (node != null)
				{
					if (node.NodeProfileType == eProfileType.Job)
					{
						schedBrowser = GetScheduleBrowserWindow();

						if (schedBrowser != null)
						{
							schedBrowser.ShowJob(node.Profile.Key);
						}
					}
				}
			}
			catch (Exception error)
			{
				HandleException(error);
			}
		}

		private void RefreshScheduleBrowserWindow()
		{
			frmScheduleBrowser schedBrowser;

			try
			{
				schedBrowser = GetScheduleBrowserWindow();

				if (schedBrowser != null)
				{
					schedBrowser.Refresh();
				}
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
		}

		private frmScheduleBrowser GetScheduleBrowserWindow()
		{
			try
			{
				foreach (Form childForm in MainMDIForm.MdiChildren)
				{
					if (childForm.GetType() == typeof(frmScheduleBrowser))
					{
						return (frmScheduleBrowser)childForm;
					}
				}

				return null;
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
		}

		private void OnSchedulePropertiesSave(object source, SchedulePropertiesSaveEventArgs e)
		{
			try
			{
				RefreshScheduleBrowserWindow();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
