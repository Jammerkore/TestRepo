using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class SearchResultContainer : UserControl
    {
        public SearchResultContainer()
        {
            InitializeComponent();
        }
        private void SearchResultContainer_Load(object sender, EventArgs e)
        {
            //this.searchLoadingUI1.SetText(spinnerInitialText);
            this.searchLoadingUI1.CancelProcessEvent += new SearchLoadingUI.CancelProcessEventHandler(Handle_CancelProcess);
        }


        private List<Thread> _threadList = new List<Thread>();

        private Thread spinnerThread;

        public void Start(string sqlToExecute, SessionAddressBlock SAB)
        {
            ThreadStart spinnerStart = new ThreadStart(this.searchLoadingUI1.UpdateSpinner);
            spinnerThread = new Thread(spinnerStart);
            spinnerThread.Name = "Spinner";
            spinnerThread.Start();

            this.sql = sqlToExecute;
            this.SAB = SAB;
            ThreadStart processStart = new ThreadStart(StartProcess);
            Thread processThread = new Thread(processStart);
            processThread.Name = "Process";
            _threadList.Add(processThread);

            StartAndWaitForThreads();
        }

        private filter filterToUse;

        //public void Start(filter f, SessionAddressBlock SAB)
        //{
        //    ThreadStart spinnerStart = new ThreadStart(this.searchLoadingUI1.UpdateSpinner);
        //    spinnerThread = new Thread(spinnerStart);
        //    spinnerThread.Name = "Spinner";
        //    spinnerThread.Start();

        //    this.filterToUse = f;
        //    this.SAB = SAB;
        //    ThreadStart processStart = new ThreadStart(StartProcessForFilteredList);
        //    Thread processThread = new Thread(processStart);
        //    processThread.Name = "Process";
        //    _threadList.Add(processThread);

        //    StartAndWaitForThreads();
        //}

        private void StartAndWaitForThreads()
        {
            try
            {
                //Start all threads we added
                foreach (Thread t in this._threadList)
                {
                    if (t.ThreadState == System.Threading.ThreadState.Unstarted)
                    {
                        t.Start();
                    }

                }

                //Wait for all threads we added to finish
                bool keepWaitingForThreadsToFinish = true;
                while (keepWaitingForThreadsToFinish)
                {
                    bool allThreadsFinished = true;
                    foreach (Thread t in this._threadList)
                    {
                        //if (t.Name != "TotalTime")
                        // {
                        if (t.ThreadState != System.Threading.ThreadState.Aborted && t.ThreadState != System.Threading.ThreadState.Stopped)
                        {
                            allThreadsFinished = false;
                        }
                        //}
                    }

                    if (allThreadsFinished == true)
                    {
                        keepWaitingForThreadsToFinish = false;
                        //_threadList[0].Abort(); //abort the total time thread
                    }

                    Application.DoEvents();
                }


                if (spinnerThread != null)
                {
                    spinnerThread.Abort();
                }


            }
            catch (Exception ex)
            {
                Abort();
                HandleException(ex);
            }
        }


        private void HandleException(Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }

        private string sql;
        private SessionAddressBlock SAB;
        private DataSet dsResults;

        public delegate void AfterExecutionDelegate(ref DataSet ds);
        public AfterExecutionDelegate AfterExecution = null;

        //private ProfileList filteredList;

        //private DataTable dtResults;
        private void StartProcess()
        {
            try
            {
                FilterData fd = new FilterData();
                DataTable dt = fd.ExecuteSQLQuery(sql, "searchResults", 0); //TT#1430-MD -jsobek -Null reference after canceling a product search


                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        dsResults = new DataSet();
                        dsResults.Tables.Add(dt);
                        if (AfterExecution != null)
                        {
                            AfterExecution.Invoke(ref dsResults);
                        }
                
                    });
                }
                else
                {
                    dsResults = new DataSet();
                    dsResults.Tables.Add(dt);
                    if (AfterExecution != null)
                    {
                        AfterExecution.Invoke(ref dsResults);
                    }
           
                }
            }
            catch (ThreadAbortException tex) //TT#1430-MD -jsobek -Null reference after canceling a product search
            {
                string s = tex.ToString();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
                
  



            try
            {
                if (this.gridControl1.InvokeRequired)
                {
                    this.gridControl1.Invoke((MethodInvoker)delegate
                    {
                        this.gridControl1.BindGrid(dsResults);
                    });
                }
                else
                {
                    this.gridControl1.BindGrid(dsResults);
                }

                if (this.searchLoadingUI1.InvokeRequired)
                {
                    this.searchLoadingUI1.Invoke((MethodInvoker)delegate
                    {
                        this.searchLoadingUI1.Visible = false;
                    });
                }
                else
                {
                    this.searchLoadingUI1.Visible = false;
                }
            }
            catch
            {
            }

            

        }

    


        //private void StartProcessForFilteredList()
        //{
        //    try
        //    {
        //        //FilterData fd = new FilterData();
        //        //DataTable dt = fd.ExecuteSQLQuery(sql, "searchResults", 0); //TT#1430-MD -jsobek -Null reference after canceling a product search
        //        this.filterToUse.SetExtraInfoForCubes(SAB, SAB.ApplicationServerSession.CreateTransaction(), null);
        //        ProfileList filteredStoreList = filterEngine.RunFilter(this.filterToUse, SAB.StoreServerSession.GetAllStoresList());
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("ID");
        //        dt.Columns.Add("Name");

        //        foreach (Profile p in filteredStoreList)
        //        {
        //            StoreProfile sp = (StoreProfile)p;

        //            DataRow dr = dt.NewRow();
        //            dr["ID"] = sp.StoreId;
        //            dr["Name"] = sp.StoreName;

        //            dt.Rows.Add(dr);
        //        }

        //        if (this.InvokeRequired)
        //        {
        //            this.Invoke((MethodInvoker)delegate
        //            {
        //                dsResults = new DataSet();
        //                dsResults.Tables.Add(dt);
        //                if (AfterExecution != null)
        //                {
        //                    AfterExecution.Invoke(ref dsResults);
        //                }

        //            });
        //        }
        //        else
        //        {
        //            dsResults = new DataSet();
        //            dsResults.Tables.Add(dt);
        //            if (AfterExecution != null)
        //            {
        //                AfterExecution.Invoke(ref dsResults);
        //            }

        //        }
        //    }
        //    catch (ThreadAbortException tex) //TT#1430-MD -jsobek -Null reference after canceling a product search
        //    {
        //        string s = tex.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandler.HandleException(ex);
        //    }

        //    //foreach (DataRow dr in dt.Rows)
        //    //{
        //    //    int hnRID = (int)dr["HN_RID"];
        //    //    int hierarchyRID = (int)dr["PH_RID"];
        //    //    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(hierarchyRID, hnRID, true);
        //    //    dr["Merchandise"] = hnp.Text;
        //    //}




        //    try
        //    {
        //        if (this.gridControl1.InvokeRequired)
        //        {
        //            this.gridControl1.Invoke((MethodInvoker)delegate
        //            {
        //                this.gridControl1.BindGrid(dsResults);
        //            });
        //        }
        //        else
        //        {
        //            this.gridControl1.BindGrid(dsResults);
        //        }
        //    }
        //    catch
        //    {
        //    }

        //    try
        //    {
        //        if (this.searchLoadingUI1.InvokeRequired)
        //        {
        //            this.searchLoadingUI1.Invoke((MethodInvoker)delegate
        //            {
        //                this.searchLoadingUI1.Visible = false;
        //            });
        //        }
        //        else
        //        {
        //            this.searchLoadingUI1.Visible = false;
        //        }
        //    }
        //    catch
        //    {
        //    }


        //    //Application.DoEvents();
        //    //}
        //}

        public void Abort()
        {
            foreach (Thread t in this._threadList)
            {
                if (t != null) t.Abort();
            }
            if (spinnerThread != null)
            {
                spinnerThread.Abort();
            }
        }
        private void Handle_CancelProcess(object sender, SearchLoadingUI.CancelProcessEventArgs e)
        {
            Abort();
        }

    }
}
