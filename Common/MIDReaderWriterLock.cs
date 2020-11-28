using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	/// <summary>
	/// The MIDReaderWriterLock class extends the ReaderWriterLock class with additional functionality to reduce the complexity of the application
	/// code.  This class eliminates the need for the calling program to determine whether to aquire a write lock or upgrade a read lock, thereby
	/// decreasing the complexity for obtaining write locks.  This class also contains logic that assures that the order of locks/unlocks is being
	/// performed in the correct order by the application code.
	/// </summary>

	public class MIDReaderWriterLock 
	{
		//=======
		// FIELDS
		//=======

		private string _name;
		private bool _trace;
		private int _traceSize;
		private Queue _traceQueue;
		private int _traceDumpCount;
		private int _locksDumpCount;
		private ReaderWriterLock _rwl;
		private Hashtable _threadLockInfo;
		
		//=============
		// CONSTRUCTORS
		//=============

		public MIDReaderWriterLock()
		{
			try
			{
				_name = "";
				_trace = false;
				_traceSize = 0;
				_traceQueue = null;
				_traceDumpCount = 0;
				_locksDumpCount = 0;

				_rwl = new ReaderWriterLock();
				_threadLockInfo = new Hashtable(); 
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public MIDReaderWriterLock(string aName)
		{
			try
			{
				_name = aName;
				_trace = false;
				_traceSize = 0;
				_traceQueue = null;
				_traceDumpCount = 0;
				_locksDumpCount = 0;

				_rwl = new ReaderWriterLock();
				_threadLockInfo = new Hashtable(); 
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public MIDReaderWriterLock(string aName, int aTraceSize)
		{
			try
			{
				_name = aName;
				_trace = true;
				_traceSize = aTraceSize;
				_traceQueue = new Queue();
				_traceDumpCount = 0;
				_locksDumpCount = 0;

				_rwl = new ReaderWriterLock();
				_threadLockInfo = new Hashtable(); 
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public bool Trace
		{
			get
			{
				return _trace;
			}
			set
			{
				_trace = value;
			}
		}

		//========
		// METHODS
		//========

		public void AcquireReaderLock(int aReaderLockTimeOut)
		{
			int threadID;
			ThreadLockInfo lockInfo;
			bool retry;
			int retryCount;

			try
			{
				threadID = System.Threading.Thread.CurrentThread.GetHashCode();

				lock (_threadLockInfo.SyncRoot)
				{
					lockInfo = (ThreadLockInfo)_threadLockInfo[threadID];

					if (lockInfo == null)
					{
						lockInfo = new ThreadLockInfo();
						_threadLockInfo.Add(threadID, lockInfo);
					}
				}

				if (_trace)
				{
					WriteTrace("Begin Acquire READER Lock", threadID);
					lockInfo.LockType = "Reader";
					lockInfo.OriginatingMethod = new System.Diagnostics.StackFrame(1, true).GetMethod().ToString();
					lockInfo.LockTime = DateTime.Now;
				}

				retry = true;
				retryCount = 0;

				while (retry)
				{
					try
					{
						++retryCount;
						_rwl.AcquireReaderLock(aReaderLockTimeOut);
						retry = false;
					}
					catch (ApplicationException)
					{
						if (retryCount > 3)
						{
							throw;
						}
                        // Begin TT#3313 - JSmith - PROD ANF hung on 10/25
                        Thread.Sleep(2000);
                        // ENd TT#3313 - JSmith - PROD ANF hung on 10/25
					}
				}

				lockInfo.AddAction(eLockAction.AquireReadLock);
				WriteTrace("End Acquire READER Lock", threadID);
			}
			catch (Exception exc)
			{
				DumpActiveLocksToFile();
				DumpTraceQueueToFile();
				string message = exc.ToString();
				throw;
			}
		}

		public void ReleaseReaderLock()
		{
			int threadID;
			ThreadLockInfo lockInfo;
			eLockAction lockAction;

			try
			{
				threadID = System.Threading.Thread.CurrentThread.GetHashCode();

				lock (_threadLockInfo.SyncRoot)
				{
					lockInfo = (ThreadLockInfo)_threadLockInfo[threadID];
				}

				if (lockInfo == null)
				{
					throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_NoActionsPending, MIDText.GetText(eMIDTextCode.msg_NoActionsPending));
				}

				lockAction = lockInfo.GetLastAction();

				if (lockAction != eLockAction.AquireReadLock)
				{
					throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_InvalidReaderUnlock, MIDText.GetText(eMIDTextCode.msg_InvalidReaderUnlock));
				}

				WriteTrace("Begin Release READER Lock", threadID);

				_rwl.ReleaseReaderLock();

				WriteTrace("End Release READER Lock", threadID);
					
				if (lockInfo.isEmpty)
				{
					lock (_threadLockInfo.SyncRoot)
					{
						_threadLockInfo.Remove(threadID);
					}
				}
			}
			catch (Exception exc)
			{
				DumpTraceQueueToFile();
				string message = exc.ToString();
				throw;
			}
		}

		public void AcquireWriterLock(int aWriterLockTimeOut)
		{
			int threadID;
			ThreadLockInfo lockInfo;
			bool retry;
			int retryCount;

			try
			{
				threadID = System.Threading.Thread.CurrentThread.GetHashCode();

				lock (_threadLockInfo.SyncRoot)
				{
					lockInfo = (ThreadLockInfo)_threadLockInfo[threadID];

					if (lockInfo == null)
					{
						lockInfo = new ThreadLockInfo();
						_threadLockInfo.Add(threadID, lockInfo);
					}
				}

				if (lockInfo.IsReaderLockHeld)
				{
					WriteTrace("Begin Upgrade to WRITER Lock", threadID);
					lockInfo.LockType = "Upgrade to Writer";
					lockInfo.UpgradingMethod = new System.Diagnostics.StackFrame(1, true).GetMethod().ToString();
					lockInfo.UpgradeTime = DateTime.Now;

					lockInfo.LockCookie = _rwl.UpgradeToWriterLock(aWriterLockTimeOut);
					lockInfo.AddAction(eLockAction.UpgradeWriteLock);

					WriteTrace("End Upgrade to WRITER Lock", threadID);
				}
				else
				{
					if (_trace)
					{
						WriteTrace("Begin Acquire WRITER Lock", threadID);
						lockInfo.LockType = "Writer";
						lockInfo.OriginatingMethod = new System.Diagnostics.StackFrame(1, true).GetMethod().ToString();
						lockInfo.LockTime = DateTime.Now;
					}

					retry = true;
					retryCount = 0;

					while (retry)
					{
						try
						{
							++retryCount;
							_rwl.AcquireWriterLock(aWriterLockTimeOut);
							retry = false;
						}
						catch (ApplicationException)
						{
							if (retryCount > 3)
							{
								throw;
							}
                            // Begin TT#3313 - JSmith - PROD ANF hung on 10/25
                            Thread.Sleep(2000);
                            // ENd TT#3313 - JSmith - PROD ANF hung on 10/25
						}
					}

					lockInfo.AddAction(eLockAction.AquireWriteLock);
					WriteTrace("End Acquire WRITER Lock", threadID);
				}
			}
			catch (Exception exc)
			{
				DumpActiveLocksToFile();
				DumpTraceQueueToFile();
				string message = exc.ToString();
				throw;
			}
		}

		public void ReleaseWriterLock()
		{
			int threadID;
			ThreadLockInfo lockInfo;
			LockCookie lockCookie;
			eLockAction lockAction;

			try
			{
				threadID = System.Threading.Thread.CurrentThread.GetHashCode();

				lock (_threadLockInfo.SyncRoot)
				{
					lockInfo = (ThreadLockInfo)_threadLockInfo[threadID];
				}

				if (lockInfo == null)
				{
					throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_NoActionsPending, MIDText.GetText(eMIDTextCode.msg_NoActionsPending));
				}

				lockAction = lockInfo.GetLastAction();

				if (lockAction != eLockAction.AquireWriteLock && lockAction != eLockAction.UpgradeWriteLock)
				{
					throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_InvalidWriterUnlock, MIDText.GetText(eMIDTextCode.msg_InvalidWriterUnlock));
				}

				if (lockAction == eLockAction.UpgradeWriteLock)
				{
					lockCookie = (LockCookie)lockInfo.LockCookie;

					WriteTrace("Begin Downgrade from WRITER Lock", threadID);

					_rwl.DowngradeFromWriterLock(ref lockCookie);
					lockInfo.LockCookie = null;

					WriteTrace("End Downgrade from WRITER Lock", threadID);
				}
				else
				{
					WriteTrace("Begin Release WRITER Lock", threadID);

					_rwl.ReleaseWriterLock();

					WriteTrace("End Release WRITER Lock", threadID);
				}

				if (lockInfo.isEmpty)
				{
					lock (_threadLockInfo.SyncRoot)
					{
						_threadLockInfo.Remove(threadID);
					}
				}
			}
			catch (Exception exc)
			{
				DumpTraceQueueToFile();
				string message = exc.ToString();
				throw;
			}
		}

		//================
		// Tracing
		//================

		private void WriteTrace(string msg, int threadId)
		{
			string stackFrameMethod;

			try
			{
				if (_trace)
				{
					stackFrameMethod = new System.Diagnostics.StackFrame(2, true).GetMethod().ToString();

					lock (_traceQueue.SyncRoot)
					{
						if (_traceQueue.Count == _traceSize)
						{
							_traceQueue.Dequeue();
						}

						_traceQueue.Enqueue("ThreadID: " + threadId.ToString(CultureInfo.CurrentUICulture) + 
							" - " + DateTime.Now.ToString("HH:mm:ss:fffffff", CultureInfo.CurrentUICulture) +
							" - " + msg +  " Method: " + stackFrameMethod);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void DumpTraceQueueToFile()
		{
			System.IO.StreamWriter traceFile;
			string traceLine;

			try
			{
				if (_trace)
				{
					lock (_traceQueue.SyncRoot)
					{
						if (_traceQueue.Count > 0)
						{
							_traceDumpCount++;

							traceFile = new System.IO.StreamWriter(
								System.Environment.CurrentDirectory + "\\MIDReaderWriterLock." +
								DateTime.Now.ToString("HHmmssfffffff", CultureInfo.CurrentUICulture) + "." +
								_name + "." + System.Threading.Thread.CurrentThread.GetHashCode() + "." + _traceDumpCount, false);

							while (_traceQueue.Count > 0)
							{
								traceLine = (string)_traceQueue.Dequeue();
								traceFile.WriteLine(traceLine);
							}

							traceFile.Close();
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void DumpActiveLocksToFile()
		{
			System.IO.StreamWriter locksFile;
			string locksLine;

			try
			{
				lock (_threadLockInfo.SyncRoot)
				{
					if (_threadLockInfo.Count > 0)
					{
						_locksDumpCount++;

						locksFile = new System.IO.StreamWriter(
							System.Environment.CurrentDirectory + "\\MIDReaderWriterLock.Locks." +
							DateTime.Now.ToString("HHmmssfffffff", CultureInfo.CurrentUICulture) + "." +
							_name + "." + System.Threading.Thread.CurrentThread.GetHashCode() + "." + _locksDumpCount + ".txt", false);

						foreach (ThreadLockInfo tli in _threadLockInfo.Values)
						{
							locksLine = tli.LockType + " lock was originated by " + tli.OriginatingMethod;
							locksLine += " at " + tli.LockTime.TimeOfDay.ToString();
							locksFile.WriteLine(locksLine);
							if (tli.UpgradeTime != DateTime.MinValue)
							{
								locksLine = "Lock was upgraded by " + tli.UpgradingMethod + " at " + tli.UpgradeTime.TimeOfDay.ToString();
							}
							locksFile.WriteLine("-------------------------------------");
						}

						locksFile.Close();
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The ThreadLockInfo class contains information about the ReaderWriterLock object for each thread.
	/// </summary>

	public class ThreadLockInfo
	{
		//=======
		// FIELDS
		//=======

		private Stack _actionStack;
		private object _lockCookie;
		private string _originatingMethod;
		private DateTime _lockTime;
		private string _upgradingMethod;
		private DateTime _upgradeTime;
		private string _lockType;

		//=============
		// CONSTRUCTORS
		//=============

		public ThreadLockInfo()
		{
			Initialize();
		}

		//===========
		// PROPERTIES
		//===========

		public object LockCookie
		{
			get
			{
				return _lockCookie;
			}
			set
			{
				_lockCookie = value;
			}
		}

		public string OriginatingMethod
		{
			get
			{
				return _originatingMethod;
			}
			set
			{
				_originatingMethod = value;
			}
		}

		public DateTime LockTime
		{
			get
			{
				return _lockTime;
			}
			set
			{
				_lockTime = value;
			}
		}

		public string UpgradingMethod
		{
			get
			{
				return _upgradingMethod;
			}
			set
			{
				_upgradingMethod = value;
			}
		}

		public DateTime UpgradeTime
		{
			get
			{
				return _upgradeTime;
			}
			set
			{
				_upgradeTime = value;
			}
		}

		public string LockType
		{
			get
			{
				return _lockType;
			}
			set
			{
				_lockType = value;
			}
		}

		public bool isEmpty
		{
			get
			{
				return _actionStack.Count == 0;
			}
		}

		public bool IsReaderLockHeld
		{
			get
			{
				try
				{
					if (_actionStack.Count == 0)
					{
						return false;
					}
					else
					{
						eLockAction lastAction = (eLockAction)Convert.ToInt32(_actionStack.Peek());
						if (lastAction == eLockAction.AquireReadLock)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

		public void Initialize()
		{
			try
			{
				_actionStack = new Stack();
				_lockCookie = null;
				_originatingMethod = string.Empty;
				_upgradingMethod = string.Empty;
				_lockType = string.Empty;
				_lockTime = DateTime.MinValue;
				_upgradeTime = DateTime.MinValue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void AddAction(eLockAction aLockAction)
		{
			try
			{
				_actionStack.Push(aLockAction);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public eLockAction GetLastAction()
		{
			try
			{
				if (_actionStack.Count == 0)
				{
					throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_NoActionsPending, MIDText.GetText(eMIDTextCode.msg_NoActionsPending));
				}

				return (eLockAction)Convert.ToInt32(_actionStack.Pop());
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
