using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class SearchResultsForm : Form
    {
        private SessionAddressBlock SAB;
        private ExplorerAddressBlock EAB;
        private filterTypes filterType;
        public SearchResultsForm(SessionAddressBlock SAB, ExplorerAddressBlock EAB, filterTypes filterType)
        {
            this.SAB = SAB;
            this.EAB = EAB;
            this.filterType = filterType;
            InitializeComponent();
            this.Icon = SharedRoutines.GetApplicationIcon();
        }

        private void SearchResultsForm_Load(object sender, EventArgs e)
        {
            this.searchResults1.LoadData(SAB, this.filterType);
            if (this.filterType == filterTypes.AuditFilter)
            {
                this.Text = "Audit Viewer";
            }
            else if (this.filterType == filterTypes.StoreFilter)
            {
                this.Text = "Store Profiles";
            }
            this.searchResults1.NewFilterEvent += new SearchResults.NewFilterEventHandler(Handle_NewFilter);
            this.searchResults1.EditFilterEvent += new SearchResults.EditFilterEventHandler(Handle_EditFilter);
            this.searchResults1.DeleteFilterEvent += new SearchResults.DeleteFilterEventHandler(Handle_DeleteFilter);
            this.searchResults1.LocateEvent += new SearchResults.LocateEventHandler(Handle_Locate);
            this.searchResults1.ClearSelectedNodeEvent += new SearchResults.ClearSelectedNodeEventHandler(Handle_ClearSelectedNode);
            this.searchResults1.CopyActionEvent += new SearchResults.CopyActionEventHandler(Handle_CopyAction);	
        }

        private void Handle_NewFilter(object sender, SearchResults.NewFilterEventArgs e)
        {
            
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                frmFilterBuilder frmFilter = SharedRoutines.GetFilterFormForNewFilters(this.filterType, SAB, EAB, SAB.ClientServerSession.UserRID);
                frmFilter.OnFilterPropertiesCloseHandler += new frmFilterBuilder.FilterPropertiesCloseEventHandler(Handle_NewFilterClosed);
                frmFilter.MdiParent = this.ParentForm;
                frmFilter.Show();
                frmFilter.BringToFront();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void Handle_NewFilterClosed(object source, frmFilterBuilder.FilterPropertiesCloseEventArgs e)
        {
            if (e.newFilterRID != Include.NoRID)
            {
                searchResults1.currentFilterRID = e.newFilterRID;
            }
            searchResults1.BindFilterComboBox();
        }

        private void Handle_EditFilter(object sender, SearchResults.EditFilterEventArgs e)
        {
            if (e.filterRID != Include.NoRID)
            {
                GenericEnqueue objEnqueue = EnqueueObject(e.filterRID, e.filterName, true);

                if (objEnqueue != null)
                {
                    bool isReadOnly = false;
                    if (objEnqueue.IsInConflict)
                    {
                        isReadOnly = true;
                    }

                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        OnFilterPropertiesCloseClass closeHandler = new OnFilterPropertiesCloseClass(objEnqueue);
                        frmFilterBuilder frmFilter = SharedRoutines.GetFilterFormForExistingFilter(e.filterRID, SAB, EAB, isReadOnly, false);
                        frmFilter.OnFilterPropertiesCloseHandler += new frmFilterBuilder.FilterPropertiesCloseEventHandler(closeHandler.OnClose);
                        frmFilter.OnFilterPropertiesCloseHandler += new frmFilterBuilder.FilterPropertiesCloseEventHandler(Handle_EditFilterClosed);
                        frmFilter.MdiParent = this.ParentForm;
                        frmFilter.Show();
                        frmFilter.BringToFront();
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }
        private void Handle_EditFilterClosed(object source, frmFilterBuilder.FilterPropertiesCloseEventArgs e)
        {
            if (e.newFilterRID != Include.NoRID)
            {
                searchResults1.currentFilterRID = e.newFilterRID;
            }
            searchResults1.BindFilterComboBox();
        }

        private void Handle_DeleteFilter(object sender, SearchResults.DeleteFilterEventArgs e)
        {
            GenericEnqueue objEnqueue = null;
            try
            {
                objEnqueue = EnqueueObject(e.filterRID, e.filterName, false);

                if (objEnqueue == null)
                {
                    return;
                }

                FilterData fd = new FilterData();
                fd.OpenUpdateConnection();
                try
                {
                    fd.FilterDelete(e.filterRID);
                    fd.CommitData();
                }
                catch (DatabaseForeignKeyViolation)
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    fd.CloseUpdateConnection();
                }

                searchResults1.currentFilterRID = Include.NoRID;
                searchResults1.BindFilterComboBox();

            }
            catch (Exception error)
            {
                string message = error.ToString();
                throw;
            }
            finally
            {
                if (objEnqueue != null)
                {
                    objEnqueue.DequeueGeneric();
                }
            }

    
        }

        private GenericEnqueue EnqueueObject(int rid, string name,  bool aAllowReadOnly)
        {
            GenericEnqueue objEnqueue;
            string errMsg;

            try
            {
                objEnqueue = new GenericEnqueue(eLockType.Filter, rid, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

                try
                {
                    objEnqueue.EnqueueGeneric();
                }
                catch (GenericConflictException)
                {
                    string[] errParms = new string[3];
                    errParms.SetValue("MID Filter Node", 0);
                    errParms.SetValue(name.Trim(), 1);
                    errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
                    errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

                    if (aAllowReadOnly)
                    {
                        errMsg += System.Environment.NewLine + System.Environment.NewLine;
                        errMsg += "Do you wish to continue with the Filter as read-only?";

                        if (MessageBox.Show(errMsg, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            objEnqueue = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show(errMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        objEnqueue = null;
                    }
                }

                return objEnqueue;
            }
            catch
            {
                throw;
            }
        }
   

        private void Handle_Locate(object sender, SearchResults.LocateEventArgs e)
        {
            if (EAB.MerchandiseExplorer != null)
            {
                try
                {
                    int hnRID = (int)e.drSelected["HN_RID"];
                    int hierarchyRID = (int)e.drSelected["PH_RID"];
                    EAB.MerchandiseExplorer.DisplayNode(hnRID, hierarchyRID);
                } 
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        private void Handle_ClearSelectedNode(object sender, SearchResults.ClearSelectedNodeEventArgs e)
        {
            if (EAB.MerchandiseExplorer != null)
            {
                try
                {
                    EAB.MerchandiseExplorer.ClearSelectedNode();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        private void Handle_CopyAction(object sender, SearchResults.CopyActionEventArgs e)
        {
            if (EAB.MerchandiseExplorer != null)
            {
                try
                {
                    EAB.MerchandiseExplorer.SetCutCopyFromClipboard(eCutCopyOperation.Copy);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }
    }
}
