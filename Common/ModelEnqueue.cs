using System;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	public class ModelConflictException : Exception
	{
	}

	/// <summary>
	/// The ModelConflict class stores information regarding a conflict during the enqueue of a Model.
	/// </summary>

	public struct ModelConflict
	{
		//=======
		// FIELDS
		//=======

		private int _userRID;
		private string _userName;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ModelConflict.
		/// </summary>
		/// <param name="aUserRID">
		/// The user RID of the conflict.
		/// </param>
		/// /// <param name="aUserRID">
		/// The user Name of the conflict.
		/// </param>

		public ModelConflict(int aUserRID, string aUserName)
		{
			_userRID = aUserRID;
			_userName = aUserName;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the user RID of the conflict.
		/// </summary>

		public int UserRID
		{
			get
			{
				return _userRID;
			}
		}

		/// <summary>
		/// Gets the user name of the conflict.
		/// </summary>

		public string UserName
		{
			get
			{
				return _userName;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The ModelEnqueue class provides functionality for enqueuing a model before using.
	/// </summary>
	/// <remarks>
	/// A model is only allowed to be in use for update once for all users.  
	/// </remarks>

	public class ModelEnqueue
	{
		//=======
		// FIELDS
		//=======

		private eModelType _modelType;
		private int _modelRID;
		private int _userRID;
		private int _clientThreadID;

		private Data.MIDEnqueue _MIDEnqueueData;
		private System.Collections.ArrayList _conflictList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ModelEnqueue using the given Model node information.
		/// </summary>
		/// <param name="aModelRID">
		/// The record ID of the Model node.
		/// </param>
		/// <param name="aUserRID">
		/// The UserRID of the user who is requesting an enqueue.
		/// </param>
		/// <param name="aClientThreadID">
		/// The thread ID of the client session.  This is used to distinguish multiple login for the same user.
		/// </param>

		public ModelEnqueue(
			eModelType aModelType,
			int aModelRID,
			int aUserRID,
			int aClientThreadID)
		{
			try
			{
				_modelType = aModelType;
				_modelRID = aModelRID;
				_userRID = aUserRID;
				_clientThreadID = aClientThreadID;
				_MIDEnqueueData = new Data.MIDEnqueue();
				_conflictList = new System.Collections.ArrayList();
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

		/// <summary>
		/// Returns a boolean indicating if the Model is in conflict.
		/// </summary>

		public bool IsInConflict
		{
			get
			{
				try
				{
					return (_conflictList.Count != 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Returns an arraylist of ModelConflict objects.
		/// </summary>

		public System.Collections.ArrayList ConflictList
		{
			get
			{
				return _conflictList;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// This method attempts to enqueue the Model.  If no existing enqueue exists for this Model, the Model is enqueued.  If there is an
		/// existing enqueue, the ConflictList is updated and a ModelConflictExcection is thrown.
		/// </summary>

		public void EnqueueModel()
		{
			System.Data.DataTable lockTable;

			try
			{
				_conflictList.Clear();

				eLockType lockType = eLockType.EligibilityModel;
				switch (_modelType)
				{
					case eModelType.Eligibility:
						lockType = eLockType.EligibilityModel;
						break;
					case eModelType.SalesModifier:
						lockType = eLockType.SalesModifierModel;
						break;
					case eModelType.StockModifier:
						lockType = eLockType.StockModifierModel;
						break;
					case eModelType.Forecasting:
						lockType = eLockType.Forecasting;
						break;
					case eModelType.ForecastBalance:
						lockType = eLockType.ForecastBalance;
						break;
					case eModelType.SizeAlternates:				// BEGIN MID Track #4970 - add size models
						lockType = eLockType.SizeAlternates;
						break;
					case eModelType.SizeConstraints:
						lockType = eLockType.SizeConstraints;
						break;										
					case eModelType.SizeCurve:
						lockType = eLockType.SizeCurve;
						break;	
					case eModelType.SizeGroup:
						lockType = eLockType.SizeGroup;
						break;										// END MID Track #4970
					// BEGIN MID Track #4370 - John Smith - FWOS Models
					case eModelType.FWOSModifier:
						lockType = eLockType.FWOSModifier;
						break;
					// END MID Track #4370
                    // BEGIN TT#108 - MD - doconnell - FWOS Max Model
                    case eModelType.FWOSMax:
                        lockType = eLockType.FWOSMaxModel;
                        break;
                    // END TT#108 - MD - doconnell - FWOS Max Model
					default:
						throw new MIDException (eErrorLevel.severe,	0, "Invalid lock type in model enqueue");
//						break;
				}

				lockTable = _MIDEnqueueData.Model_Read(lockType, _modelRID);

				foreach (System.Data.DataRow dataRow in lockTable.Rows)
				{
					_conflictList.Add(
						new ModelConflict(
						System.Convert.ToInt32(dataRow["USER_RID"], CultureInfo.CurrentUICulture),
						System.Convert.ToString(dataRow["USER_FULLNAME"], CultureInfo.CurrentUICulture)));
				}

				if (_conflictList.Count > 0)
				{
					throw new ModelConflictException();
				}
				else
				{
					_MIDEnqueueData.OpenUpdateConnection();

					try
					{
						_MIDEnqueueData.Model_Insert(lockType, _modelRID, _userRID, _clientThreadID);

						_MIDEnqueueData.CommitData();
					}
					catch (Exception exc)
					{
						_MIDEnqueueData.Rollback();
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_MIDEnqueueData.CloseUpdateConnection();
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method dequeues a Model.
		/// </summary>

		public void DequeueModel()
		{
			try
			{
				_MIDEnqueueData.OpenUpdateConnection();

				try
				{
					eLockType lockType = eLockType.EligibilityModel;
					switch (_modelType)
					{
						case eModelType.Eligibility:
							lockType = eLockType.EligibilityModel;
							break;
						case eModelType.SalesModifier:
							lockType = eLockType.SalesModifierModel;
							break;
						case eModelType.StockModifier:
							lockType = eLockType.StockModifierModel;
							break;
						case eModelType.Forecasting:
							lockType = eLockType.Forecasting;
							break;
						case eModelType.ForecastBalance:
							lockType = eLockType.ForecastBalance;
							break;
						case eModelType.SizeAlternates:					// BEGIN MID Track #4970 - add size models
							lockType = eLockType.SizeAlternates;
							break;
						case eModelType.SizeConstraints:
							lockType = eLockType.SizeConstraints;
							break;										
						case eModelType.SizeCurve:
							lockType = eLockType.SizeCurve;
							break;	
						case eModelType.SizeGroup:
							lockType = eLockType.SizeGroup;
							break;										// END MID Track #4970
							// BEGIN MID Track #4370 - John Smith - FWOS Models
						case eModelType.FWOSModifier:
							lockType = eLockType.FWOSModifier;
							break;
							// END MID Track #4370
                        // BEGIN TT#108 - MD - doconnell - FWOS Max Model
                        case eModelType.FWOSMax:
                            lockType = eLockType.FWOSMaxModel;
                            break;
                        // END TT#108 - MD - doconnell - FWOS Max Model
						default:	
							throw new MIDException (eErrorLevel.severe,	0, "Invalid lock type in model enqueue");
//							break;
					}
					_MIDEnqueueData.Model_Delete(lockType, _modelRID, _userRID);

					_MIDEnqueueData.CommitData();
				}
				catch (Exception exc)
				{
					_MIDEnqueueData.Rollback();
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_MIDEnqueueData.CloseUpdateConnection();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
