using System;
using System.Collections;
using System.Threading;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// The ConcurrentProcessManager manages concurrent process in threads.
	/// </summary>
	public class ConcurrentProcessManager
	{
		//=======
		// FIELDS
		//=======

		private Audit _audit;
		private Stack _commandStack;
		private int _concurrentProcesses;
		private int _intervalWaitTime;
 
		//=============
		// CONSTRUCTORS
		//=============

		public ConcurrentProcessManager(Audit aAudit, Stack aCommandStack, int aConcurrentProcesses, int aIntervalWaitTime)
		{
			_audit = aAudit;
			_commandStack = aCommandStack;
			_concurrentProcesses = aConcurrentProcesses;
			_intervalWaitTime = aIntervalWaitTime;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

        //Begin TT#172 - JSmith - Deadlock
        //public eMIDMessageLevel ProcessCommands()
        //{
        //    ConcurrentProcess[] commandProcArray = null;
        //    eMIDMessageLevel maxMessageLevel = eMIDMessageLevel.None;
        //    int i;
			
        //    try
        //    {
        //        commandProcArray = new ConcurrentProcess[_concurrentProcesses];
				
        //        if (_commandStack != null &&
        //            _commandStack.Count > 0)
        //        {
        //            while (_commandStack.Count > 0)
        //            {
        //                for (i = 0; i < _concurrentProcesses && _commandStack.Count > 0; i++)
        //                {
        //                    if (commandProcArray[i] == null || !commandProcArray[i].IsRunning)
        //                    {
        //                        commandProcArray[i] = (ConcurrentProcess)_commandStack.Pop();
									
        //                        commandProcArray[i].ExecuteProcessInThread();
        //                    }
        //                }

        //                if (_commandStack.Count > 0)
        //                {
        //                    System.Threading.Thread.Sleep(_intervalWaitTime);
        //                }
        //            }

        //            if (commandProcArray != null)
        //            {
        //                for (i = 0; i < _concurrentProcesses; i++)
        //                {
        //                    if (commandProcArray[i] != null)
        //                    {
        //                        commandProcArray[i].WaitForThreadExit();
        //                        commandProcArray[i] = null;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            maxMessageLevel = eMIDMessageLevel.NothingToDo;
        //        }

        //        return maxMessageLevel;
        //    }
        //    catch (ThreadAbortException)
        //    {
        //        if (commandProcArray != null)
        //        {
        //            foreach (ConcurrentProcess commandProc in commandProcArray)
        //            {
        //                if (commandProc != null)
        //                {
        //                    commandProc.AbortThread();
        //                    commandProc.WaitForThreadExit();
        //                }
        //            }
        //        }

        //        //				_audit.Add_Msg(eMIDMessageLevel.Warning, "TaskList was cancelled by User", "ConcurrentProcess");

        //        return eMIDMessageLevel.Severe;
        //    }
        //    catch (Exception exc)
        //    {
        //        _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessCommands", "ProcessRollupBatches");
        //        _audit.Log_Exception(exc, GetType().Name);
	
        //        return eMIDMessageLevel.Severe;
        //    }
        //}

		// Begin TT#522 - STodd - memory issues
		public eMIDMessageLevel ProcessCommands()
		{
			return ProcessCommands(true);
		}
		// End TT#522 - STodd - memory issues

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keepProcesses">Boolean to say whether the processes should be kept in process stack or not after completing. True keeps them. False removes them.</param>
		/// <returns></returns>
        public eMIDMessageLevel ProcessCommands(bool keepProcesses)
        {
            ConcurrentProcess[] commandProcArray = null;
            eMIDMessageLevel maxMessageLevel = eMIDMessageLevel.None;
            int i;
            int item;
            int processCount;
            ConcurrentProcess[] commandStackArray = null;
            Hashtable processIDHash;
            ConcurrentProcess concurrentProcess;
            bool executeProcess;

            try
            {
                commandProcArray = new ConcurrentProcess[_concurrentProcesses];

                if (_commandStack != null &&
                    _commandStack.Count > 0)
                {
                    processCount = _commandStack.Count;
                    processIDHash = new Hashtable();
                    // convert stack to array so processes can remain in list 
                    commandStackArray = new ConcurrentProcess[_commandStack.Count];
                    _commandStack.CopyTo(commandStackArray, 0);

					// Begin TT#522 - STodd - memory issues
					if (!keepProcesses)
					{
						_commandStack.Clear();
						_commandStack = null;
					}
					// End TT#522 - STodd - memory issues

                    item = GetItemToProcess(commandStackArray, 0);
                    while (processCount > 0)
                    {
                        for (i = 0; i < _concurrentProcesses && processCount > 0; i++)
                        {
                            if (commandProcArray[i] == null || !commandProcArray[i].IsRunning)
                            {
                                // if process finished running, remove process ID from hash so 
                                // next process can run
                                if (commandProcArray[i] != null && !commandProcArray[i].IsRunning)
                                {
                                    concurrentProcess = (ConcurrentProcess)commandProcArray[i];
                                    // Begin TT#243 - JSmith - API - purge failed
                                    //processIDHash.Remove(concurrentProcess.ProcessID);
                                    if (concurrentProcess.ProcessID != null)
                                    {
                                        processIDHash.Remove(concurrentProcess.ProcessID);
                                    }
                                    // End TT#243

                                    commandProcArray[i] = null;
                                }
                                concurrentProcess = (ConcurrentProcess)commandStackArray[item];
                                executeProcess = true;
                                // if process contains ID, verify a process with the same ID
                                // is not already running
                                if (concurrentProcess.ProcessID != null)
                                {
                                    if (processIDHash.ContainsKey(concurrentProcess.ProcessID))
                                    {
                                        executeProcess = false;
                                    }
                                    else
                                    {
                                        processIDHash.Add(concurrentProcess.ProcessID, null);
                                    }
                                }

                                if (executeProcess)
                                {
									commandStackArray[item] = null;

                                    commandProcArray[i] = concurrentProcess;
                                    commandProcArray[i].ExecuteProcessInThread();
                                    --processCount;
                                }

                                item = GetItemToProcess(commandStackArray, ++item);
                            }
                        }

                        if (processCount > 0)
                        {
                            System.Threading.Thread.Sleep(_intervalWaitTime);
                        }
                    }

                    if (commandProcArray != null)
                    {
                        for (i = 0; i < _concurrentProcesses; i++)
                        {
                            if (commandProcArray[i] != null)
                            {
                                commandProcArray[i].WaitForThreadExit();
                                commandProcArray[i] = null;
                            }
                        }
                    }
                }
                else
                {
                    maxMessageLevel = eMIDMessageLevel.NothingToDo;
                }

                return maxMessageLevel;
            }
            catch (ThreadAbortException)
            {
                if (commandProcArray != null)
                {
                    foreach (ConcurrentProcess commandProc in commandProcArray)
                    {
                        if (commandProc != null)
                        {
                            commandProc.AbortThread();
                            commandProc.WaitForThreadExit();
                        }
                    }
                }

                return eMIDMessageLevel.Severe;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessCommands", "ProcessRollupBatches");
                _audit.Log_Exception(exc, GetType().Name);

                return eMIDMessageLevel.Severe;
            }
        }

        private int GetItemToProcess(ConcurrentProcess[] commandStackArray, int item)
        {
            bool searching = true;
            int initialItem = item;
            int itemsChecked = 0;

            while (searching)
            {
                // at the end so restart at beginning
                if (item > commandStackArray.Length - 1)
                {
                    item = 0;
                }

                // found item
                if (commandStackArray[item] != null)
                {
                    searching = false;
                }
                // checked all items
                else if (itemsChecked == commandStackArray.Length)
                {
                    searching = false;
                    item = -1;
                }
                else
                {
                    ++item;
                }
                ++itemsChecked;
            }
            return item;
        }
        //End TT#172

		// Begin TT#522 - stodd - memory issues
		virtual public void SetProcessInfo(ConcurrentProcess sProcess)
		{

		}
		// End TT#522 - stodd - memory issues

	}

	/// <summary>
	/// ConcurrentProcess is a base class that contains common functionality to process a threaded process.
	/// </summary>

	abstract public class ConcurrentProcess
	{
		//=======
		// FIELDS
		//=======

		private Audit _audit;
		private bool _isRunning;
		private eMIDMessageLevel _exitMessageLevel;
		private DateTime _completionDateTime;
		private Thread _thread;
		private int _numberOfErrors;
        // Begin TT#172 - JSmith - Deadlock
        private string _processID;
        // End TT#172


		//=============
		// CONSTRUCTORS
		//=============

		public ConcurrentProcess(Audit aAudit)
		{
			try
			{
				_audit = aAudit;
				_numberOfErrors = 0;
                // Begin TT#172 - JSmith - Deadlock
                _processID = null;
                // End TT#172
			}
			catch (ThreadAbortException exc)
			{
				string message = exc.ToString();
				throw;
			}
			catch (Exception exc)
			{
				_audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "ConcurrentProcess");
				_audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

        // Begin TT#172 - JSmith - Deadlock
        public ConcurrentProcess(Audit aAudit, string aProcessID)
        {
            try
            {
                _audit = aAudit;
                _numberOfErrors = 0;
                _processID = aProcessID;
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "ConcurrentProcess");
                _audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
        // End TT#172

		//===========
		// PROPERTIES
		//===========

		public Audit Audit 
		{
			get { return _audit ; }
		}

        // Begin TT#172 - JSmith - Deadlock
        public string ProcessID
        {
            get { return _processID; }
        }
        // End TT#172

		public bool IsRunning
		{
			get
			{
				try
				{
					return _isRunning;
				}
				catch (ThreadAbortException exc)
				{
					string message = exc.ToString();
					throw;
				}
				catch (Exception exc)
				{
					_audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in isRunning:Get", "ConcurrentProcess");
					_audit.Log_Exception(exc, GetType().Name);
					throw;
				}
			}
			set { _isRunning = value ; }
		}

		public DateTime CompletionDateTime
		{
			get
			{
				try
				{
					return _completionDateTime;
				}
				catch (ThreadAbortException exc)
				{
					string message = exc.ToString();
					throw;
				}
				catch (Exception exc)
				{
					_audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CompletionTime:Get", "ConcurrentProcess");
					_audit.Log_Exception(exc, GetType().Name);
					throw;
				}
			}
			set { _completionDateTime = value ; }
		}

		public eMIDMessageLevel ExitMessageLevel
		{
			get
			{
				try
				{
					return _exitMessageLevel;
				}
				catch (ThreadAbortException exc)
				{
					string message = exc.ToString();
					throw;
				}
				catch (Exception exc)
				{
					_audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ReturnCode:Get", "ConcurrentProcess");
					_audit.Log_Exception(exc, GetType().Name);
					throw;
				}
			}
			set { _exitMessageLevel = value ; } 
		}

		public int NumberOfErrors 
		{
			get { return _numberOfErrors ; }
			set { _numberOfErrors = value ; }
		}

		//========
		// METHODS
		//========

		public void AbortThread()
		{
			try
			{
				_thread.Abort();
                // wait for thread to exit
                _thread.Join();
			}
			catch (ThreadAbortException exc)
			{
				_exitMessageLevel = eMIDMessageLevel.Cancelled;
				string message = exc.ToString();
				throw;
			}
			catch (Exception exc)
			{
				_exitMessageLevel = eMIDMessageLevel.Severe;
				_audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in AbortThread", "ConcurrentProcess");
				_audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		public void WaitForThreadExit()
		{
			try
			{
				_thread.Join();
			}
			catch (ThreadAbortException exc)
			{
				_exitMessageLevel = eMIDMessageLevel.Cancelled;
				string message = exc.ToString();
				throw;
			}
			catch (Exception exc)
			{
				_exitMessageLevel = eMIDMessageLevel.Severe;
				_audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in WaitForThreadExit", "ConcurrentProcess");
				_audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		public void ExecuteProcessInThread()
		{
			try
			{
				_thread = new Thread(new ThreadStart(ExecuteProcess));
				_thread.Start();
			}
			catch (ThreadAbortException exc)
			{
				string message = exc.ToString();
				_exitMessageLevel = eMIDMessageLevel.Cancelled;
				throw;
			}
			catch (Exception exc)
			{
				_exitMessageLevel = eMIDMessageLevel.Severe;
				_audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcessInThread", "ConcurrentProcess");
				_audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		/// <summary>
		/// This base call is to be overriden in each inheriting Process class to specify the Process
		/// functionality for the type of Process.
		/// </summary>
		public abstract void ExecuteProcess();
	}

}
