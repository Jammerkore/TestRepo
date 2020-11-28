// Begin TT#632-MD - JSmith - Add Concurrent Processes to Determine Hierarchy Activity
// Resturctured process to allow for concurrent processes.  Too many changes to mark.
// End TT#632-MD - JSmith - Add Concurrent Processes to Determine Hierarchy Activity

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceProcess;
using System.ServiceProcess.Design;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.DetermineHierarchyActivity
{
    class DetermineHierarchyActivityProcess
    {
        SessionAddressBlock _SAB;
        

        public DetermineHierarchyActivityProcess(SessionAddressBlock SAB, ref bool errorFound)
        {
            _SAB = SAB;
        }

        public eReturnCode ProcessHierarchyActivity(int aCommitLimit, int aConcurrentProcesses, int aFromOffset, int aToOffset, int aForecastOffset, int aIntransitOffset)
        {
            int totalNodes = 0;
            int activeNodes = 0;
            int inactiveNodes = 0;
            int totalErrors = 0;
            Stack processStack;
            ArrayList processArrayList = new ArrayList();
            MerchandiseHierarchyData mhd;

            try
            {
                mhd = new MerchandiseHierarchyData();
                try
                {
                    mhd.OpenUpdateConnection();
                    //mhd.DetermineHierarchyActivity(aFromOffset, aToOffset, aForecastOffset, aIntransitOffset);
                    mhd.SetHierarchyActivityFalse();
                    mhd.CommitData();
                    //_SAB.HierarchyServerSession.ClearNodeCache();
                }
                catch (Exception ex)
                {
                    _SAB.ClientServerSession.Audit.Log_Exception(ex);
                    return eReturnCode.severe;
                }
                finally
                {
                    if (mhd != null &&
                        mhd.ConnectionIsOpen)
                    {
                        mhd.CloseUpdateConnection();
                    }
                }
                try
                {
                    processStack = BuildStack(_SAB.ClientServerSession, processArrayList, aCommitLimit, aFromOffset, aToOffset, aForecastOffset, aIntransitOffset);

                    if (aConcurrentProcesses > 1)
                    {
                        ConcurrentProcessManager cpm = new ConcurrentProcessManager(_SAB.ClientServerSession.Audit, processStack, aConcurrentProcesses, 5000);
                        cpm.ProcessCommands();
                    }
                    else
                    {
                        while (processStack.Count > 0)
                        {
                            ConcurrentProcess cp = (ConcurrentProcess)processStack.Pop();
                            cp.ExecuteProcess();
                            totalErrors += cp.NumberOfErrors;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _SAB.ClientServerSession.Audit.Log_Exception(ex);
                    return eReturnCode.severe;
                }
                totalNodes = mhd.GetTotalNodeCount();
                activeNodes = mhd.GetActiveNodeCount();
                inactiveNodes = mhd.GetInactiveNodeCount();
                _SAB.HierarchyServerSession.ClearNodeCache();
            }
            catch (Exception ex)
            {
                _SAB.ClientServerSession.Audit.Log_Exception(ex);
                return eReturnCode.severe;
            }
            _SAB.ClientServerSession.Audit.DetermineHierarchyActivityAuditInfo_Add(totalNodes, activeNodes, inactiveNodes, totalErrors);

            return eReturnCode.successful;
        }

        private Stack BuildStack(Session aSession, ArrayList aProcessArrayList, int aCommitLimit, int aFromOffset, int aToOffset, int aForecastOffset, int aIntransitOffset)
        {
            try
            {
                ConcurrentProcess process = null;
                Stack processStack = new Stack();

                for (int i = 0; i <= aSession.GlobalOptions.NumberOfStoreDataTables - 1; i++)
                {
                    process = new DetermineNodeActivityProcess(_SAB, aSession.Audit, i, aCommitLimit, aFromOffset, aToOffset, aForecastOffset, aIntransitOffset);
                    processStack.Push(process);
                    aProcessArrayList.Add(process);
                }

                return processStack;
            }
            catch (Exception exc)
            {
                aSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in BuildStack", "TaskListProcessor");
                aSession.Audit.Log_Exception(exc, GetType().Name);

                throw;
            }
        }
    }

    public class DetermineNodeActivityProcess : ConcurrentProcess
    {
        //=======
        // FIELDS
        //=======

        private SessionAddressBlock _SAB;
        private int _table;
        private MerchandiseHierarchyData _mhd;
        private int _fromOffset;
        private int _toOffset;
        private int _forecastOffset;
        private int _intransitOffset;
        private eReturnCode _returnCode;

        //=============
        // CONSTRUCTORS
        //=============

        public DetermineNodeActivityProcess(SessionAddressBlock aSAB, Audit aAudit, int aTable, int aCommitLimit, int aFromOffset, int aToOffset, int aForecastOffset, int aIntransitOffset)
            : base(aAudit)
        {
            try
            {
                _SAB = aSAB;
                _table = aTable;
                _fromOffset = aFromOffset;
                _toOffset = aToOffset;
                _forecastOffset = aForecastOffset;
                _intransitOffset = aIntransitOffset;
                _returnCode = eReturnCode.successful;
            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========
        public eReturnCode ReturnCode
        {
            get
            {
                return _returnCode;
            }
        }

        //========
        // METHODS
        //========

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;

            string message = "Executing Determine Hierarchy Activity for table=" + _table.ToString();
            try
            {
                _mhd = new MerchandiseHierarchyData();
                try
                {
                    _mhd.OpenUpdateConnection();
                    _mhd.DetermineHierarchyActivity(_fromOffset, _toOffset, _forecastOffset, _intransitOffset, _table);
                    _mhd.CommitData();
                }
                catch (Exception ex)
                {
                    _SAB.ClientServerSession.Audit.Log_Exception(ex);
                    _returnCode = eReturnCode.severe;
                }
                finally
                {
                    if (_mhd != null &&
                        _mhd.ConnectionIsOpen)
                    {
                        _mhd.CloseUpdateConnection();
                    }
                }
            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                    ++NumberOfErrors;

                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                    ++NumberOfErrors;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
                ++NumberOfErrors;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

    }
}
