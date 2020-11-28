using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	public delegate Label LabelCreatorDelegate(FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText);

	#region TimeTotalVariableReference Class

	/// <summary>
	/// Class that defines a TimeTotalVariableReference, which is a connection between a time total VariableProfile and the variable VariableProfile.
	/// </summary>

	public class TimeTotalVariableReference
	{
		//=======
		// FIELDS
		//=======

		private TimeTotalVariableProfile _timeTotVarProf;
		private VariableProfile _varProf;
		private int _timeTotIdx;

		//=============
		// CONSTRUCTORS
		//=============

		public TimeTotalVariableReference(TimeTotalVariableProfile aTimeTotVarProf, VariableProfile aVarProf, int aTimeTotIdx)
		{
			_timeTotVarProf = aTimeTotVarProf;
			_varProf = aVarProf;
			_timeTotIdx = aTimeTotIdx;
		}

		//===========
		// PROPERTIES
		//===========

		public TimeTotalVariableProfile TimeTotalVariableProfile
		{
			get
			{
				return _timeTotVarProf;
			}
		}

		public VariableProfile VariableProfile
		{
			get
			{
				return _varProf;
			}
		}

		public int TimeTotalIndex
		{
			get
			{
				return _timeTotIdx;
			}
		}

		//========
		// METHODS
		//========
	}
	#endregion

	#region FilterDefinition Class
	/// <summary>
	/// Class that defines the base FilterDefinition Class.
	/// </summary>

	abstract public class FilterDefinition
	{
		//=======
		// FIELDS
		//=======

		private SessionAddressBlock _SAB;
		private Session _currentSession;
		private LabelCreatorDelegate _labelCreator;
		private int _filterRID;
        //Begin Track #5727 - KJohnson - Stop proccessing if outdated information
        private bool _filterOutdatedInformation;
        //End Track #5727 - KJohnson

		//=============
		// CONSTRUCTORS
		//=============

		public FilterDefinition(
			SessionAddressBlock aSAB,
			Session aCurrentSession,
			LabelCreatorDelegate aLabelCreator)
		{
			_SAB = aSAB;
			_currentSession = aCurrentSession;
			_labelCreator = aLabelCreator;
			_filterRID = -1;
            //Begin Track #5727 - KJohnson - Stop proccessing if outdated information
            _filterOutdatedInformation = false;
            //End Track #5727 - KJohnson
        }

		public FilterDefinition(
			SessionAddressBlock aSAB,
			Session aCurrentSession,
			LabelCreatorDelegate aLabelCreator,
			int aFilterRID)
		{
			_SAB = aSAB;
			_currentSession = aCurrentSession;
			_labelCreator = aLabelCreator;
			_filterRID = aFilterRID;
            //Begin Track #5727 - KJohnson - Stop proccessing if outdated information
			_filterOutdatedInformation = false;
            //End Track #5727 - KJohnson
        }

		//===========
		// PROPERTIES
		//===========

		public SessionAddressBlock SAB
		{
			get
			{
				return _SAB;
			}
		}

		public Session CurrentSession
		{
			get
			{
				return _currentSession;
			}
		}

		public LabelCreatorDelegate LabelCreator
		{
			get
			{
				return _labelCreator;
			}
		}

		public int FilterRID
		{
			get
			{
				return _filterRID;
			}
		}

        //Begin Track #5727 - KJohnson - Stop proccessing if outdated information
        public bool FilterOutdatedInformation
        {
            get
            {
				return _filterOutdatedInformation;
            }
            set
            {
				_filterOutdatedInformation = value;
            }
        }
        //End Track #5727 - KJohnson

		//========
		// METHODS
		//========
		abstract public int SaveFilter(int aFilterKey, int aUserRID, string aSaveName);
	}
	#endregion

	#region StoreFilterDefinition Class
	/// <summary>
	/// Class that defines the StoreFilterDefinition.
	/// </summary>

	public class StoreFilterDefinition : FilterDefinition
	{
		//=======
		// FIELDS
		//=======

		private StoreFilterData _storeFilterDL;
		private ProfileList _versionProfList;
		private ProfileList _variableProfList;
		private ProfileList _timeTotalVariableProfList;
		private ArrayList _attrOperandList;
		private ArrayList _dataOperandList;

		//=============
		// CONSTRUCTORS
		//=============

		public StoreFilterDefinition(
			SessionAddressBlock aSAB,
			Session aCurrentSession,
			StoreFilterData aStoreFilterDL,
			LabelCreatorDelegate aLabelCreator)
			: base(aSAB, aCurrentSession, aLabelCreator)
		{
			_storeFilterDL = aStoreFilterDL;
			_versionProfList = null;
			_variableProfList = null;
			_timeTotalVariableProfList = null;

			_attrOperandList = new ArrayList();
			_dataOperandList = new ArrayList();
		}

		public StoreFilterDefinition(
			SessionAddressBlock aSAB,
			Session aCurrentSession,
			StoreFilterData aStoreFilterDL,
			LabelCreatorDelegate aLabelCreator,
			ProfileList aVersionProfileList,
			ProfileList aVariableProfileList,
			ProfileList aTimeTotalVariableProfileList,
			int aFilterRID)
			: base(aSAB, aCurrentSession, aLabelCreator, aFilterRID)
		{
			_storeFilterDL = aStoreFilterDL;
			_versionProfList = aVersionProfileList;
			_variableProfList = aVariableProfileList;
			_timeTotalVariableProfList = aTimeTotalVariableProfileList;

			ReadAndLoadFilter();

		}

		//===========
		// PROPERTIES
		//===========

		public ProfileList VersionProfileList
		{
			get
			{
				return _versionProfList;
			}
		}

		public ProfileList VariableProfileList
		{
			get
			{
				return _variableProfList;
			}
		}

		public ProfileList TimeTotalVariableProfileList
		{
			get
			{
				return _timeTotalVariableProfList;
			}
		}

		public ArrayList AttrOperandList
		{
			get
			{
				return _attrOperandList;
			}
		}

		public ArrayList DataOperandList
		{
			get
			{
				return _dataOperandList;
			}
		}

		//========
		// METHODS
		//========

		private void ReadAndLoadFilter()
		{
			BinaryFormatter binaryFmtr;
			DataTable table;
			ArrayList operandList;
			MemoryStream memStream;
			ArrayList queueList;
			Type type;
			QueryOperand operand;

			try
			{
				binaryFmtr = new BinaryFormatter();
				_attrOperandList = new ArrayList();
				_dataOperandList = new ArrayList();

				table = _storeFilterDL.StoreFilterObject_Read(FilterRID);

				foreach (DataRow row in table.Rows)
				{
					if ((eFilterObjectType)Convert.ToInt32(row["STORE_FILTER_OBJECT_TYPE"], CultureInfo.CurrentUICulture) == eFilterObjectType.Attribute)
					{
						operandList = _attrOperandList;
					}
					else
					{
						operandList = _dataOperandList;
					}

					memStream = new MemoryStream((byte[])row["STORE_FILTER_OBJECT"]);
                    //Begin TT#1192 - JSmith - Batch Blocked
                    binaryFmtr.Binder = new MIDDeserializationBinder();
                    AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
                    {
                        AssemblyName requestedName = new AssemblyName(e.Name);
                        //Begin TT#1293-MD -jsobek -Resource Error when reopening a previously opened and closed window
                        string assemblyFullFilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\" + requestedName.Name + ".dll";
                        if (System.IO.File.Exists(assemblyFullFilePath) == true)
                        {
                            return Assembly.LoadFrom(assemblyFullFilePath);
                        }
                        else
                        {
                            return null;
                        }
                        //End TT#1293-MD -jsobek -Resource Error when reopening a previously opened and closed window
                    };
                    // End TT#1192
					queueList = (ArrayList)binaryFmtr.Deserialize(memStream);
					operandList.Clear();

					foreach (Queue queue in queueList)
					{
						type = (Type)queue.Dequeue();

						operand = (QueryOperand)Activator.CreateInstance(type, new object[] {this, queue});

						if (operand.ValidCreate)
						{
							operandList.Add(operand);
						}

						if (operand.OutdatedInformation)
						{
                            FilterOutdatedInformation = true; //Track #5727
						}
					}
				}

				if (FilterOutdatedInformation) //Track #5727
				{
					SAB.MessageCallback.HandleMessage(CurrentSession.Audit.GetText(eMIDTextCode.msg_OutdatedFilterInfo), "Filter Definition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
        public void RebuildOperandsWithNoUI()
        {
            //filters have to be "redrawn" after they are loaded for the operand list to match what is really saved
            ArrayList attributeOperandRedrawList = new ArrayList();
            foreach (QueryOperand operand in _attrOperandList)
            {
                operand.OnRedraw(attributeOperandRedrawList);
            }
            _attrOperandList = attributeOperandRedrawList;

            ArrayList dataOperandRedrawList = new ArrayList();
            foreach (QueryOperand operand in _dataOperandList)
            {
                operand.OnRedraw(dataOperandRedrawList);
            }
            _dataOperandList = dataOperandRedrawList;
        }
        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

		override public int SaveFilter(int aFilterKey, int aUserRID, string aSaveName)
		{
			ArrayList attrQueueList;
			ArrayList dataQueueList;
			Queue queue;
			MemoryStream memStream;
			BinaryFormatter binaryFmtr;
			byte[] attrArray;
			byte[] dataArray;

			try
			{
				CheckSyntax();

				binaryFmtr = new BinaryFormatter();

				attrQueueList = new ArrayList();
				foreach (QueryOperand operand in _attrOperandList)
				{
					queue = operand.GetDBDataQueue();
					if (queue != null)
					{
						attrQueueList.Add(queue);
					}
				}

				memStream = new MemoryStream();
				binaryFmtr.Serialize(memStream, attrQueueList);
				attrArray = memStream.ToArray();
			
				dataQueueList = new ArrayList();
				foreach (QueryOperand operand in _dataOperandList)
				{
					queue = operand.GetDBDataQueue();
					if (queue != null)
					{
						dataQueueList.Add(queue);
					}
				}

				memStream = new MemoryStream();
				binaryFmtr.Serialize(memStream, dataQueueList);
				dataArray = memStream.ToArray();

				_storeFilterDL.OpenUpdateConnection();

				try
				{
					if (aFilterKey == -1)
					{
						aFilterKey = _storeFilterDL.StoreFilter_Insert(aUserRID, aSaveName);
					}
					else
					{
						_storeFilterDL.StoreFilter_Update(aFilterKey, aUserRID, aSaveName);
						_storeFilterDL.StoreFilterObject_Delete(aFilterKey);
					}

					_storeFilterDL.StoreFilterObject_Insert(aFilterKey, attrArray, dataArray);

                    SaveFilterXRef(aFilterKey); //TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

					_storeFilterDL.CommitData();

					return aFilterKey;
				}
				catch (Exception exc)
				{
					_storeFilterDL.Rollback();
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_storeFilterDL.CloseUpdateConnection();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}





        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
        private void SaveFilterXRef(int aFilterKey)
        {
            

            ArrayList aFilterXRefList = new ArrayList(); // create an array of cross reference data to save

            //For attribute operands, we only care about the attribute (aka store group) and the attribute set (aka store group level)
            foreach (QueryOperand operand in _attrOperandList)
            {
                //Stores cannot be deleted so we do not need to include them in the In Use cross reference.  They fall in the operand type of AttrQueryStoreDetailOperand.
                if (operand.GetType() == typeof(AttrQueryAttributeDetailOperand))
                {
                    AttrQueryAttributeDetailOperand a = (AttrQueryAttributeDetailOperand)operand;

                    //save two XRef entries - one for the store group, and one for the store group level
                    FilterXRef xrefGroup = new FilterXRef(eFilterXRefType.StoreFilterXRef, aFilterKey, eProfileType.StoreGroup, a.GetGroupLevelProfile().GroupRid);
                    aFilterXRefList.Add(xrefGroup);

                    FilterXRef xrefGroupLevel = new FilterXRef(eFilterXRefType.StoreFilterXRef, aFilterKey, eProfileType.StoreGroupLevel, a.GetGroupLevelProfile().Key);
                    aFilterXRefList.Add(xrefGroupLevel);
                }

                //also check the main operand
                //Begin TT#3196-MD -jsobek -Filter XREF is not being updated with new Filter Creation or Updates
                if (operand.GetType() == typeof(AttrQueryAttributeMainOperand))
                {
                    AttrQueryAttributeMainOperand a = (AttrQueryAttributeMainOperand)operand;

                    //save two XRef entries - one for the store group, and one for the store group level
                    if (a.StoreGroupProfile != null)
                    {
                        FilterXRef xrefGroup = new FilterXRef(eFilterXRefType.StoreFilterXRef, aFilterKey, eProfileType.StoreGroup, a.StoreGroupProfile.Key);
                        aFilterXRefList.Add(xrefGroup);

                        foreach (StoreGroupLevelProfile strGrpLvlProf in a.AttributeSetProfList)
                        {
                            FilterXRef xrefGroupLevel = new FilterXRef(eFilterXRefType.StoreFilterXRef, aFilterKey, eProfileType.StoreGroupLevel, strGrpLvlProf.Key);
                            aFilterXRefList.Add(xrefGroupLevel);
                        }
                    }
                }
                //End TT#3196-MD -jsobek -Filter XREF is not being updated with new Filter Creation or Updates

            }

         

            foreach (QueryOperand operand in _dataOperandList)
            {
                eProfileType profileType = new eProfileType();
                int profileTypeRID = 0;
                bool saveXRef = false;

                //determine the eProfileType and the corresponding RID


                //DataQueryCubeModifyerOperand - No need to store in cross reference - it is just enum values: None,StoreDetail,StoreAverage,StoreTotal,ChainDetail
                //DataQueryTimeModifyerOperand - No need to store in cross reference - it is also just an enum: None,Any,All,Join,Average,Total,Corresponding
                //DataQueryGradeOperand - Ignoring this for now as it seems like the grades are used as a literal in the filters
                //DataQueryStatusOperand - Ignoring this type as well - the application grabs a set of text rows from APPLICATION_TEXT
                if (operand.GetType() == typeof(DataQueryTimeTotalVariableOperand))
                {
                    DataQueryTimeTotalVariableOperand a = (DataQueryTimeTotalVariableOperand)operand;

                    profileType = eProfileType.TimeTotalVariable;
                    profileTypeRID = a.TimeTotalVariableProfile.Key;

                    saveXRef = true;
                }   
                else if (operand.GetType() == typeof(DataQueryVersionOperand))
                {
                    DataQueryVersionOperand a = (DataQueryVersionOperand)operand;

                    profileType = a.GetVersionProfile().ProfileType;  //eProfileType.Version;
                    profileTypeRID = a.GetVersionProfile().Key;

                    saveXRef = true;
                }
                else if (operand.GetType() == typeof(DataQueryDateRangeOperand))
                {
                    DataQueryDateRangeOperand a = (DataQueryDateRangeOperand)operand;

                    profileType = a.GetDateRangeProfile().ProfileType;  //eProfileType.DateRange;
                    profileTypeRID = a.GetDateRangeProfile().Key;

                    saveXRef = true;
                }
                //Begin TT#3196-MD -jsobek -Filter XREF is not being updated with new Filter Creation or Updates
                else if (operand.GetType() == typeof(DataQueryNodeOperand))
                {
                    DataQueryNodeOperand a = (DataQueryNodeOperand)operand;
                    profileType = a.GetNodeProfile().ProfileType;  //eProfileType.HierarchyNode;
                    profileTypeRID = a.GetNodeProfile().Key;

                    saveXRef = true;
                }
                else if (operand.GetType().IsSubclassOf(typeof(DataQueryVariableOperand)))
                {
                    DataQueryVariableOperand a = (DataQueryVariableOperand)operand;

                    if (a.NodeProfile != null)
                    {
                        profileType = a.NodeProfile.ProfileType; //eProfileType.HierarchyNode;
                        profileTypeRID = a.NodeProfile.Key;

                        saveXRef = true;

                        if (a.VersionProfile != null)
                        {
                            //also save a version with this
                            FilterXRef xrefVersion = new FilterXRef(eFilterXRefType.StoreFilterXRef, aFilterKey, eProfileType.Version, a.VersionProfile.Key);
                            aFilterXRefList.Add(xrefVersion);
                        }
                    }
                }
                else if (operand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
                {
                    DataQueryPlanOperand a = (DataQueryPlanOperand)operand;
                    if (a.VariableOperand != null && a.VariableOperand.NodeProfile != null)
                    {
                        profileType = a.VariableOperand.NodeProfile.ProfileType; //eProfileType.HierarchyNode;
                        profileTypeRID = a.VariableOperand.NodeProfile.Key;

                        saveXRef = true;

                        if (a.VariableOperand.VersionProfile != null)
                        {
                            //also save a version with this
                            FilterXRef xrefVersion = new FilterXRef(eFilterXRefType.StoreFilterXRef, aFilterKey, eProfileType.Version, a.VariableOperand.VersionProfile.Key);
                            aFilterXRefList.Add(xrefVersion);
                        }
                    }
                }
                
                //End TT#3196-MD -jsobek -Filter XREF is not being updated with new Filter Creation or Updates



                //if we cannot find a good eProfileType and a good corresponding RID - do not include in the IN USE filter cross reference table
                if (saveXRef)
                {
                    FilterXRef xref = new FilterXRef(eFilterXRefType.StoreFilterXRef, aFilterKey, profileType, profileTypeRID);
                    aFilterXRefList.Add(xref);
                }

            }

            _storeFilterDL.StoreFilterObject_XRef_Delete(aFilterKey);
            _storeFilterDL.StoreFilterObject_XRef_Insert(aFilterXRefList);

            
        }

        public void SaveFilterXRefOnly(int aFilterKey)
        {
            try
            {
                CheckSyntax();

                _storeFilterDL.OpenUpdateConnection();

                try
                {
                    SaveFilterXRef(aFilterKey); 
                    _storeFilterDL.CommitData();
                }
                catch (Exception exc)
                {
                    _storeFilterDL.Rollback();
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    _storeFilterDL.CloseUpdateConnection();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }



        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)


		public bool CheckSyntax()
		{
			IEnumerator enumerator;
			QueryOperand operand;
			int parenLevel;

			try
			{
				enumerator = _attrOperandList.GetEnumerator();
				operand = GetNextOperand(eFilterListType.Attribute, enumerator, false);
                // Begin TT#189 MD - JSmith - Filter Performance
                parenLevel = 0;
                if (operand != null)
                {
                    //parenLevel = 0;
                    // End TT#189 MD

                    try
                    {
                        CheckAttrSyntax(enumerator, ref operand, ref parenLevel);
                    }
                    // Begin TT#189 MD - JSmith - Filter Performance
                    //catch (EndOfOperandsException)
                    //{
                    //}
                    // End TT#189 MD
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                // Begin TT#189 MD - JSmith - Filter Performance
                }
                // End TT#189 MD

				if (parenLevel != 0)
				{
					throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnbalancedParenthesis), eFilterListType.Attribute, null);
				}
			}
            // Begin TT#189 MD - JSmith - Filter Performance
            //catch (EndOfOperandsException)
            //{
            //}
            // End TT#189 MD
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}

			try
			{
				enumerator = _dataOperandList.GetEnumerator();
				operand = GetNextOperand(eFilterListType.Data, enumerator, false);
                // Begin TT#189 MD - JSmith - Filter Performance
                if (operand == null)
                {
                    return true;
                }
                // End TT#189 MD
				parenLevel = 0;

				try
				{
					CheckDataSyntax(enumerator, ref operand, ref parenLevel);
				}
                // Begin TT#189 MD - JSmith - Filter Performance
                //catch (EndOfOperandsException)
                //{
                //}
                // End TT#189 MD
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}

				if (parenLevel != 0)
				{
					throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnbalancedParenthesis), eFilterListType.Data, null);
				}
			}
            // Begin TT#189 MD - JSmith - Filter Performance
            //catch (EndOfOperandsException)
            //{
            //}
            // End TT#189 MD
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}

			return true;
		}

		private void CheckAttrSyntax(IEnumerator aEnumerator, ref QueryOperand aOperand, ref int aParenLevel)
		{
			try
			{
				while (true)
				{
					if (aOperand.GetType() == typeof(GenericQueryLeftParenOperand))
					{
						aParenLevel++;
						aOperand = GetNextOperand(eFilterListType.Attribute, aEnumerator, true);
                        // Begin TT#189 MD - JSmith - Filter Performance
                        if (aOperand == null)
                        {
                            return;
                        }
                        // End TT#189 MD
						CheckAttrSyntax(aEnumerator, ref aOperand, ref aParenLevel);
					}
					else
					{
						if (aOperand.GetType() == typeof(AttrQueryAttributeMainOperand) || aOperand.GetType() == typeof(AttrQueryStoreMainOperand))
						{
							aOperand = GetNextOperand(eFilterListType.Attribute, aEnumerator, false);
                            // Begin TT#189 MD - JSmith - Filter Performance
                            if (aOperand == null)
                            {
                                return;
                            }
                            // End TT#189 MD
						}
						else
						{
							throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SetOrStoreExpected), eFilterListType.Attribute, aOperand);
						}
					}

					if (aOperand.GetType() == typeof(GenericQueryRightParenOperand))
					{
						if (aParenLevel == 0)
						{
							throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnmatchedParenthesis), eFilterListType.Attribute, aOperand);
						}
						aParenLevel--;
						aOperand = GetNextOperand(eFilterListType.Attribute, aEnumerator, false);
                        // Begin TT#189 MD - JSmith - Filter Performance
                        if (aOperand == null)
                        {
                            return;
                        }
                        // End TT#189 MD
						return;
					}
					else if (aOperand.GetType() == typeof(GenericQueryAndOperand) || aOperand.GetType() == typeof(GenericQueryOrOperand))
					{
						aOperand = GetNextOperand(eFilterListType.Attribute, aEnumerator, true);
                        // Begin TT#189 MD - JSmith - Filter Performance
                        if (aOperand == null)
                        {
                            return;
                        }
                        // End TT#189 MD
					}
					else
					{
						throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AndOrOrExpected), eFilterListType.Attribute, aOperand);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void CheckDataSyntax(IEnumerator aEnumerator, ref QueryOperand aOperand, ref int aParenLevel)
		{
			eValueFormatType var1FormatType;
			bool pctValue;

			try
			{
				while (true)
				{
					if (aOperand.GetType() == typeof(GenericQueryLeftParenOperand))
					{
						aParenLevel++;
						aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, true);
                        // Begin TT#189 MD - JSmith - Filter Performance
                        if (aOperand == null)
                        {
                            return;
                        }
                        // End TT#189 MD
						CheckDataSyntax(aEnumerator, ref aOperand, ref aParenLevel);
					}
					else
					{
						var1FormatType = eValueFormatType.None;
						pctValue = false;

						if (aOperand.GetType() == typeof(DataQueryPlanVariableOperand) || aOperand.GetType() == typeof(DataQueryTimeTotalVariableOperand))
						{
							if (((DataQueryVariableOperand)aOperand).TimeModifyer == eFilterTimeModifyer.Corresponding)
							{
								throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CorrespondingNotValid), eFilterListType.Data, aOperand);
							}

//Begin Track #5111 - JScott - Add additional filter functionality
							if (((DataQueryVariableOperand)aOperand).TimeModifyer == eFilterTimeModifyer.Join && ((DataQueryVariableOperand)aOperand).DateRangeProfile != null)
							{
								throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateRangeNotValidWithJoin), eFilterListType.Data, aOperand);
							}

//End Track #5111 - JScott - Add additional filter functionality
							if (aOperand.GetType() == typeof(DataQueryPlanVariableOperand))
							{
								var1FormatType = ((DataQueryPlanVariableOperand)aOperand).VariableProfile.FormatType;
							}
							else
							{
								var1FormatType = ((DataQueryTimeTotalVariableOperand)aOperand).TimeTotalVariableProfile.FormatType;
							}

							aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, true);
                            // Begin TT#189 MD - JSmith - Filter Performance
                            if (aOperand == null)
                            {
                                return;
                            }
                            // End TT#189 MD
						}
						else
						{
							throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VExpected), eFilterListType.Data, aOperand);
						}

						if (aOperand.GetType() == typeof(DataQueryPctChangeOperand) || aOperand.GetType() == typeof(DataQueryPctOfOperand))
						{
							aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, true);
                            // Begin TT#189 MD - JSmith - Filter Performance
                            if (aOperand == null)
                            {
                                return;
                            }
                            // End TT#189 MD

							if (aOperand.GetType() == typeof(DataQueryPlanVariableOperand) || aOperand.GetType() == typeof(DataQueryTimeTotalVariableOperand))
							{
//Begin Track #5111 - JScott - Add additional filter functionality
								if (((DataQueryVariableOperand)aOperand).TimeModifyer == eFilterTimeModifyer.Join && ((DataQueryVariableOperand)aOperand).DateRangeProfile != null)
								{
									throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateRangeNotValidWithJoin), eFilterListType.Data, aOperand);
								}

//End Track #5111 - JScott - Add additional filter functionality
								aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, true);
                                // Begin TT#189 MD - JSmith - Filter Performance
                                if (aOperand == null)
                                {
                                    return;
                                }
                                // End TT#189 MD
							}
							else
							{
								throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VExpected), eFilterListType.Data, aOperand);
							}

							pctValue = true;
						}

						if (aOperand.GetType() == typeof(DataQueryNotOperand))
						{
							aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, true);
                            // Begin TT#189 MD - JSmith - Filter Performance
                            if (aOperand == null)
                            {
                                return;
                            }
                            // End TT#189 MD
						}

						if (aOperand.GetType() == typeof(DataQueryEqualOperand) ||
							aOperand.GetType() == typeof(DataQueryLessOperand) ||
							aOperand.GetType() == typeof(DataQueryLessEqualOperand) ||
							aOperand.GetType() == typeof(DataQueryGreaterOperand) ||
							aOperand.GetType() == typeof(DataQueryGreaterEqualOperand))
						{
							aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, true);
                            // Begin TT#189 MD - JSmith - Filter Performance
                            if (aOperand == null)
                            {
                                return;
                            }
                            // End TT#189 MD
						}
						else
						{
							throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_OperatorExpected), eFilterListType.Data, aOperand);
						}

						if (pctValue)
						{
							if (aOperand.GetType() == typeof(DataQueryLiteralOperand))
							{
								aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
                                // Begin TT#189 MD - JSmith - Filter Performance
                                if (aOperand == null)
                                {
                                    return;
                                }
                                // End TT#189 MD
							}
							else
							{
								throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_QExpected), eFilterListType.Data, aOperand);
							}
						}
						else
						{
							if (var1FormatType == eValueFormatType.GenericNumeric)
							{
//Begin Track #5111 - JScott - Add additional filter functionality
//								if (aOperand.GetType() == typeof(DataQueryPlanVariableOperand) ||
//									aOperand.GetType() == typeof(DataQueryTimeTotalVariableOperand) ||
//									aOperand.GetType() == typeof(DataQueryLiteralOperand))
								if (aOperand.GetType() == typeof(DataQueryPlanVariableOperand) ||
									aOperand.GetType() == typeof(DataQueryTimeTotalVariableOperand))
								{
									if (((DataQueryVariableOperand)aOperand).TimeModifyer == eFilterTimeModifyer.Join && ((DataQueryVariableOperand)aOperand).DateRangeProfile != null)
									{
										throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateRangeNotValidWithJoin), eFilterListType.Data, aOperand);
									}

									aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
                                    // Begin TT#189 MD - JSmith - Filter Performance
                                    if (aOperand == null)
                                    {
                                        return;
                                    }
                                    // End TT#189 MD
								}
								else if (aOperand.GetType() == typeof(DataQueryLiteralOperand))
//End Track #5111 - JScott - Add additional filter functionality
								{
									aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
                                    // Begin TT#189 MD - JSmith - Filter Performance
                                    if (aOperand == null)
                                    {
                                        return;
                                    }
                                    // End TT#189 MD
								}
								else
								{
									throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VQExpected), eFilterListType.Data, aOperand);
								}
							}
							else if (var1FormatType == eValueFormatType.StoreGrade)
							{
//Begin Track #5111 - JScott - Add additional filter functionality
//								if (aOperand.GetType() == typeof(DataQueryPlanVariableOperand) ||
//									aOperand.GetType() == typeof(DataQueryTimeTotalVariableOperand) ||
//									aOperand.GetType() == typeof(DataQueryLiteralOperand))
								if (aOperand.GetType() == typeof(DataQueryPlanVariableOperand) ||
									aOperand.GetType() == typeof(DataQueryTimeTotalVariableOperand))
								{
									if (((DataQueryVariableOperand)aOperand).TimeModifyer == eFilterTimeModifyer.Join && ((DataQueryVariableOperand)aOperand).DateRangeProfile != null)
									{
										throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateRangeNotValidWithJoin), eFilterListType.Data, aOperand);
									}

									aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
                                    // Begin TT#189 MD - JSmith - Filter Performance
                                    if (aOperand == null)
                                    {
                                        return;
                                    }
                                    // End TT#189 MD
								}
								//Begin TT#202 - JScott - Filter Wizard upon finishing a Filter receive syntax error
								else if (aOperand.GetType() == typeof(DataQueryGradeOperand))
								{
									aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
                                    // Begin TT#189 MD - JSmith - Filter Performance
                                    if (aOperand == null)
                                    {
                                        return;
                                    }
                                    // End TT#189 MD
								}
								//End TT#202 - JScott - Filter Wizard upon finishing a Filter receive syntax error
								else if (aOperand.GetType() == typeof(DataQueryLiteralOperand))
//End Track #5111 - JScott - Add additional filter functionality
								{
									aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
                                    // Begin TT#189 MD - JSmith - Filter Performance
                                    if (aOperand == null)
                                    {
                                        return;
                                    }
                                    // End TT#189 MD
								}
								else
								{
									throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VGExpected), eFilterListType.Data, aOperand);
								}
							}
							else if (var1FormatType == eValueFormatType.StoreStatus)
							{
//Begin Track #5111 - JScott - Add additional filter functionality
//								if (aOperand.GetType() == typeof(DataQueryPlanVariableOperand) ||
//									aOperand.GetType() == typeof(DataQueryTimeTotalVariableOperand) ||
//									aOperand.GetType() == typeof(DataQueryLiteralOperand))
								if (aOperand.GetType() == typeof(DataQueryPlanVariableOperand) ||
									aOperand.GetType() == typeof(DataQueryTimeTotalVariableOperand))
								{
									if (((DataQueryVariableOperand)aOperand).TimeModifyer == eFilterTimeModifyer.Join && ((DataQueryVariableOperand)aOperand).DateRangeProfile != null)
									{
										throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateRangeNotValidWithJoin), eFilterListType.Data, aOperand);
									}

									aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
                                    // Begin TT#189 MD - JSmith - Filter Performance
                                    if (aOperand == null)
                                    {
                                        return;
                                    }
                                    // End TT#189 MD
								}
								//Begin TT#202 - JScott - Filter Wizard upon finishing a Filter receive syntax error
								else if (aOperand.GetType() == typeof(DataQueryStatusOperand))
								{
									aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
                                    // Begin TT#189 MD - JSmith - Filter Performance
                                    if (aOperand == null)
                                    {
                                        return;
                                    }
                                    // End TT#189 MD
								}
								//End TT#202 - JScott - Filter Wizard upon finishing a Filter receive syntax error
								else if (aOperand.GetType() == typeof(DataQueryLiteralOperand))
//End Track #5111 - JScott - Add additional filter functionality
								{
									aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
                                    // Begin TT#189 MD - JSmith - Filter Performance
                                    if (aOperand == null)
                                    {
                                        return;
                                    }
                                    // End TT#189 MD
								}
								else
								{
									throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VSExpected), eFilterListType.Data, aOperand);
								}
							}
							else
							{
								throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VQExpected), eFilterListType.Data, aOperand);
							}
						}
//Begin Track #5111 - JScott - Add additional filter functionality
//
//						if (aOperand.GetType() == typeof(GenericQueryRightParenOperand))
//						{
//							if (aParenLevel == 0)
//							{
//								throw new FilterSyntaxErrorException(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnmatchedParenthesis), eFilterListType.Data, aOperand);
//							}
//							aParenLevel--;
//							aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
//							return;
//						}
//						else if (aOperand.GetType() == typeof(GenericQueryAndOperand) || aOperand.GetType() == typeof(GenericQueryOrOperand))
//						{
//							aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, true);
//						}
//						else
//						{
//							throw new FilterSyntaxErrorException(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AndOrOrExpected), eFilterListType.Data, aOperand);
//						}
//					}
					}

					if (aOperand.GetType() == typeof(GenericQueryRightParenOperand))
					{
						if (aParenLevel == 0)
						{
							throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnmatchedParenthesis), eFilterListType.Data, aOperand);
						}
						aParenLevel--;
						aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, false);
						return;
					}
					else if (aOperand.GetType() == typeof(GenericQueryAndOperand) || aOperand.GetType() == typeof(GenericQueryOrOperand))
					{
						aOperand = GetNextOperand(eFilterListType.Data, aEnumerator, true);
                        // Begin TT#189 MD - JSmith - Filter Performance
                        if (aOperand == null)
                        {
                            return;
                        }
                        // End TT#189 MD
					}
					else
					{
						throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AndOrOrExpected), eFilterListType.Data, aOperand);
					}
//End Track #5111 - JScott - Add additional filter functionality
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private QueryOperand GetNextOperand(eFilterListType aFilterListType, IEnumerator aEnumerator, bool aThrowEarlyEnd)
		{
			QueryOperand queryOperand;

			try
			{
				bool rc;

				while ((rc = aEnumerator.MoveNext()) && (aEnumerator.Current.GetType() == typeof(AttrQuerySpacerOperand) || aEnumerator.Current.GetType() == typeof(DataQuerySpacerOperand) || !((QueryOperand)aEnumerator.Current).isMainOperand))
				{
				}

				if (rc)
				{
					queryOperand = (QueryOperand)aEnumerator.Current;

					try
					{
						queryOperand.CheckOperand();
					}
					catch (FilterOperandErrorException exc)
					{
						throw new FilterSyntaxErrorException(exc.Message, aFilterListType, queryOperand);
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}

					return (QueryOperand)aEnumerator.Current;
				}
				else
				{
					if (aThrowEarlyEnd)
					{
						throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnexpectedEOL), aFilterListType, null);
					}
					else
					{
                        // Begin TT#189 MD - JSmith - Filter Performance
                        //throw new EndOfOperandsException();
                        return null;
                        // End TT#189 MD
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
	#endregion

	#region ProductFilterDefinition Class
	/// <summary>
	/// Class that defines the StoreFilterDefinition.
	/// </summary>

	public class ProductSearchFilterDefinition : FilterDefinition
	{
		//=======
		// FIELDS
		//=======
		private ProductFilterData _productFilterDL;
		private ArrayList _characteristicOperandList;

		//=============
		// CONSTRUCTORS
		//=============

		public ProductSearchFilterDefinition(
			SessionAddressBlock aSAB,
			Session aCurrentSession,
			ProductFilterData aProductFilterDL,
			LabelCreatorDelegate aLabelCreator)
			: base(aSAB, aCurrentSession, aLabelCreator)
		{
			_productFilterDL = aProductFilterDL;
			_characteristicOperandList = new ArrayList();
		}

		public ProductSearchFilterDefinition(
			SessionAddressBlock aSAB,
			Session aCurrentSession,
			ProductFilterData aProductFilterDL,
			LabelCreatorDelegate aLabelCreator,
			int aFilterRID)
			: base(aSAB, aCurrentSession, aLabelCreator, aFilterRID)
		{
			_productFilterDL = aProductFilterDL;

			ReadAndLoadFilter();
		}

		//===========
		// PROPERTIES
		//===========

		public ArrayList CharacteristicOperandList
		{
			get
			{
				return _characteristicOperandList;
			}
		}

		//========
		// METHODS
		//========

		private void ReadAndLoadFilter()
		{
			BinaryFormatter binaryFmtr;
			DataTable table;
			ArrayList operandList;
			MemoryStream memStream;
			ArrayList queueList;
			Type type;
			QueryOperand operand;

			try
			{
				binaryFmtr = new BinaryFormatter();
				_characteristicOperandList = new ArrayList();

				table = _productFilterDL.ProductSearchObject_Read(SAB.ClientServerSession.UserRID);

				foreach (DataRow row in table.Rows)
				{
					//if ((eFilterObjectType)Convert.ToInt32(row["PRODUCT_SEARCH_OBJECT_TYPE"], CultureInfo.CurrentUICulture) == eFilterObjectType.Characteristic)
					//{
				        operandList = _characteristicOperandList;
					//}

				    memStream = new MemoryStream((byte[])row["PRODUCT_SEARCH_OBJECT"]);
                    //Begin TT#1192 - JSmith - Batch Blocked
                    binaryFmtr.Binder = new MIDDeserializationBinder();
                    AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
                    {
                        AssemblyName requestedName = new AssemblyName(e.Name);
                        //Begin TT#1293-MD -jsobek -Resource Error when reopening a previously opened and closed window
                        string assemblyFullFilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\" + requestedName.Name + ".dll";
                        if (System.IO.File.Exists(assemblyFullFilePath) == true)
                        { 
                            return Assembly.LoadFrom(assemblyFullFilePath);
                        }
                        else
                        {
                            return null;
                        }
                        //End TT#1293-MD -jsobek -Resource Error when reopening a previously opened and closed window
                    };
                    // End TT#1192
				    queueList = (ArrayList)binaryFmtr.Deserialize(memStream);
				    operandList.Clear();

					foreach (Queue queue in queueList)
					{
						type = (Type)queue.Dequeue();

						operand = (QueryOperand)Activator.CreateInstance(type, new object[] { this, queue });

						if (operand.ValidCreate)
						{
							operandList.Add(operand);
						}

						if (operand.OutdatedInformation)
						{
                            FilterOutdatedInformation = true; //Track #5727
						}
					}
				}

				if (FilterOutdatedInformation) //Track #5727
				{
					SAB.MessageCallback.HandleMessage(CurrentSession.Audit.GetText(eMIDTextCode.msg_OutdatedFilterInfo), "Filter Definition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
        public void RebuildOperandsWithNoUI()
        {
            //filters have to be "redrawn" after they are loaded for the operand list to match what is really saved
            ArrayList characteristicOperandRedrawList = new ArrayList();
            foreach (QueryOperand operand in _characteristicOperandList)
            {
                operand.OnRedraw(characteristicOperandRedrawList);
            }
            _characteristicOperandList = characteristicOperandRedrawList;
        }
        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

		override public int SaveFilter(int aFilterKey, int aUserRID, string aSaveName)
		{
			ArrayList characteristicQueueList;
			Queue queue;
			MemoryStream memStream;
			BinaryFormatter binaryFmtr;
			byte[] characteristicArray;

			try
			{
				CheckSyntax();

				binaryFmtr = new BinaryFormatter();

				characteristicQueueList = new ArrayList();
				foreach (QueryOperand operand in _characteristicOperandList)
				{
					queue = operand.GetDBDataQueue();
					if (queue != null)
					{
						characteristicQueueList.Add(queue);
					}
				}

				memStream = new MemoryStream();
				binaryFmtr.Serialize(memStream, characteristicQueueList);
				characteristicArray = memStream.ToArray();


				_productFilterDL.OpenUpdateConnection();

				try
				{
					_productFilterDL.ProductSearchObject_Delete(aUserRID);
					_productFilterDL.ProductSearchObject_Insert(aUserRID, characteristicArray);

                    SaveFilterXRef(aUserRID); //TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

                    _productFilterDL.CommitData();

					return aFilterKey;
				}
				catch (Exception exc)
				{
					_productFilterDL.Rollback();
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_productFilterDL.CloseUpdateConnection();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
        private void SaveFilterXRef(int aUserRID)
        {
            ArrayList aFilterXRefList = new ArrayList(); // create an array of cross reference data to save

            foreach (QueryOperand operand in _characteristicOperandList)
            {
    
                if (operand.GetType() == typeof(ProdCharQueryCharacteristicDetailOperand))
                {
                    ProdCharQueryCharacteristicDetailOperand a = (ProdCharQueryCharacteristicDetailOperand)operand;


                    FilterXRef xrefCharacteristicValue = new FilterXRef(eFilterXRefType.ProductSearchXRef, aUserRID, eProfileType.ProductCharacteristicValue, a.GetProductCharValueProfile().Key);
                    aFilterXRefList.Add(xrefCharacteristicValue);


                    FilterXRef xrefCharacteristic = new FilterXRef(eFilterXRefType.ProductSearchXRef, aUserRID, eProfileType.ProductCharacteristic, a.GetProductCharValueProfile().ProductCharRID);
                    aFilterXRefList.Add(xrefCharacteristic);
                }

                //Begin TT#3196-MD -jsobek -Filter XREF is not being updated with new Filter Creation or Updates
                if (operand.GetType() == typeof(ProdCharQueryCharacteristicMainOperand))
                {
                    ProdCharQueryCharacteristicMainOperand a = (ProdCharQueryCharacteristicMainOperand)operand;

                    foreach (ProductCharValueProfile o in a.ProductCharValueProfList)
                    {
                        FilterXRef xrefCharacteristicValue = new FilterXRef(eFilterXRefType.ProductSearchXRef, aUserRID, eProfileType.ProductCharacteristicValue, o.Key);
                        aFilterXRefList.Add(xrefCharacteristicValue);

                        FilterXRef xrefCharacteristic = new FilterXRef(eFilterXRefType.ProductSearchXRef, aUserRID, eProfileType.ProductCharacteristic, o.ProductCharRID);
                        aFilterXRefList.Add(xrefCharacteristic);
                    }

                    if (a.ProductCharProfileStored != null)
                    {
                        FilterXRef xrefCharacteristicStored = new FilterXRef(eFilterXRefType.ProductSearchXRef, aUserRID, eProfileType.ProductCharacteristic, a.ProductCharProfileStored.Key);
                        aFilterXRefList.Add(xrefCharacteristicStored);
                    }
                }
                //End TT#3196-MD -jsobek -Filter XREF is not being updated with new Filter Creation or Updates
            }

            _productFilterDL.ProductSearchObject_XRef_Delete(aUserRID);
            _productFilterDL.ProductSearchObject_XRef_Insert(aFilterXRefList);
        }
        public void SaveFilterXRefOnly(int aFilterKey)
        {
            try
            {
                CheckSyntax();

                _productFilterDL.OpenUpdateConnection();

                try
                {
                    SaveFilterXRef(aFilterKey);
                    _productFilterDL.CommitData();
                }
                catch (Exception exc)
                {
                    _productFilterDL.Rollback();
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    _productFilterDL.CloseUpdateConnection();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

		public bool CheckSyntax()
		{
			IEnumerator enumerator;
			QueryOperand operand;
			int parenLevel;

			try
			{
				enumerator = _characteristicOperandList.GetEnumerator();
				operand = GetNextOperand(eFilterListType.Characteristic, enumerator, false);
				parenLevel = 0;

                // Begin TT#189 MD - JSmith - Filter Performance
                if (operand != null)
                {
                    // End TT#189 MD
                    try
                    {
                        CheckCharacteristicSyntax(enumerator, ref operand, ref parenLevel);
                    }
                    // Begin TT#189 MD - JSmith - Filter Performance
                    //catch (EndOfOperandsException)
                    //{
                    //}
                    // End TT#189 MD
                    catch (Exception exc)
                    {
                        string message = exc.ToString();
                        throw;
                    }
                // Begin TT#189 MD - JSmith - Filter Performance
                }
                // End TT#189 MD

				if (parenLevel != 0)
				{
					throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnbalancedParenthesis), eFilterListType.Attribute, null);
				}
			}
            // Begin TT#189 MD - JSmith - Filter Performance
            //catch (EndOfOperandsException)
            //{
            //}
            // End TT#189 MD
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}

			return true;
		}

		private void CheckCharacteristicSyntax(IEnumerator aEnumerator, ref QueryOperand aOperand, ref int aParenLevel)
		{
			try
			{
				while (true)
				{
					if (aOperand.GetType() == typeof(GenericQueryLeftParenOperand))
					{
						aParenLevel++;
						aOperand = GetNextOperand(eFilterListType.Characteristic, aEnumerator, true);
                        // Begin TT#189 MD - JSmith - Filter Performance
                        if (aOperand == null)
                        {
                            return;
                        }
                        // End TT#189 MD
						CheckCharacteristicSyntax(aEnumerator, ref aOperand, ref aParenLevel);
					}
					else if (aOperand.GetType() == typeof(DataQueryNotOperand))
					{
						aOperand = GetNextOperand(eFilterListType.Characteristic, aEnumerator, true);
                        // Begin TT#189 MD - JSmith - Filter Performance
                        if (aOperand == null)
                        {
                            return;
                        }
                        // End TT#189 MD
						continue;
					}
					else
					{
						if (aOperand.GetType() == typeof(ProdCharQueryCharacteristicMainOperand))
						{
							aOperand = GetNextOperand(eFilterListType.Characteristic, aEnumerator, false);
                            // Begin TT#189 MD - JSmith - Filter Performance
                            if (aOperand == null)
                            {
                                return;
                            }
                            // End TT#189 MD
						}
						else
						{
							throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CharacteristicValueExpected), eFilterListType.Attribute, aOperand);
						}
					}

					if (aOperand.GetType() == typeof(GenericQueryRightParenOperand))
					{
						if (aParenLevel == 0)
						{
							throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnmatchedParenthesis), eFilterListType.Characteristic, aOperand);
						}
						aParenLevel--;
						aOperand = GetNextOperand(eFilterListType.Characteristic, aEnumerator, false);
						return;
					}
					else if (aOperand.GetType() == typeof(GenericQueryAndOperand) || aOperand.GetType() == typeof(GenericQueryOrOperand))
					{
						aOperand = GetNextOperand(eFilterListType.Characteristic, aEnumerator, true);
                        // Begin TT#189 MD - JSmith - Filter Performance
                        if (aOperand == null)
                        {
                            return;
                        }
                        // End TT#189 MD
					}
					else
					{
						throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AndOrOrExpected), eFilterListType.Attribute, aOperand);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private QueryOperand GetNextOperand(eFilterListType aFilterListType, IEnumerator aEnumerator, bool aThrowEarlyEnd)
		{
			QueryOperand queryOperand;

			try
			{
				bool rc;

				while ((rc = aEnumerator.MoveNext()) && (aEnumerator.Current.GetType() == typeof(ProdCharQuerySpacerOperand) || !((QueryOperand)aEnumerator.Current).isMainOperand))
				{
				}

				if (rc)
				{
					queryOperand = (QueryOperand)aEnumerator.Current;

					try
					{
						queryOperand.CheckOperand();
					}
					catch (FilterOperandErrorException exc)
					{
						throw new FilterSyntaxErrorException(exc.Message, aFilterListType, queryOperand);
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}

					return (QueryOperand)aEnumerator.Current;
				}
				else
				{
					if (aThrowEarlyEnd)
					{
						throw new FilterSyntaxErrorException(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnexpectedEOL), aFilterListType, null);
					}
					else
					{
                        // Begin TT#189 MD - JSmith - Filter Performance
                        //throw new EndOfOperandsException();
                        return null;
                        // End TT#189 MD
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
	#endregion

	#region QueryOperand Class
	/// <summary>
	/// Class that defines the base QueryOperand Class.
	/// </summary>

	abstract public class QueryOperand
	{
		//=======
		// FIELDS
		//=======

		private Label _label;
		protected FilterDefinition _filterDef;
		protected bool _validCreate;
		protected bool _outdatedInformation;

		//=============
		// CONSTRUCTORS
		//=============

		public QueryOperand(FilterDefinition aFilterDef)
		{
			try
			{
				_filterDef = aFilterDef;
				Initialize();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public QueryOperand(FilterDefinition aFilterDef, Queue aDataQueue)
		{
			try
			{
				_filterDef = aFilterDef;
				Initialize();
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

		public Label Label
		{
			get
			{
				if (_label == null && _validCreate && _filterDef.LabelCreator != null)
				{
					_label = CreateLabel();
				}
				return _label;
			}
		}

		virtual public bool isMainOperand
		{
			get
			{
				return true;
			}
		}

		public bool ValidCreate
		{
			get
			{
				return _validCreate;
			}
		}

		public bool OutdatedInformation
		{
			get
			{
				return _outdatedInformation;
			}
		}

		//========
		// METHODS
		//========

		abstract protected Label CreateLabel();

		private void Initialize()
		{
			try
			{
				_validCreate = true;
				_outdatedInformation = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void CheckOperand()
		{
			return;
		}

		virtual public void OnDelete()
		{
		}

		virtual public void OnRedraw(ArrayList aOutList)
		{
			aOutList.Add(this);
		}

		virtual public Queue GetDBDataQueue()
		{
			try
			{
				Queue dataQueue;

				dataQueue = new Queue();
				dataQueue.Enqueue(this.GetType());
				return dataQueue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	#endregion

	#region GenericQueryOperand Classes
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	// Generic Query labels
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================

	/// <summary>
	/// Abstract class that defines the base GenericQueryOperand class.  This class defines the base level functionality for a label that is displayed
	/// in the attribute query panel.
	/// </summary>

	abstract public class GenericQueryOperand : QueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public GenericQueryOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public GenericQueryOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines an Or label.
	/// </summary>

	public class GenericQueryOrOperand : GenericQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public GenericQueryOrOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public GenericQueryOrOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Blue, "OR");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an And label.
	/// </summary>

	public class GenericQueryAndOperand : GenericQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public GenericQueryAndOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public GenericQueryAndOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Blue, "AND");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines a Left Parenthesis label.
	/// </summary>

	public class GenericQueryLeftParenOperand : GenericQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public GenericQueryLeftParenOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public GenericQueryLeftParenOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}
	
		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Black, "(");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines a Right Parenthesis label.
	/// </summary>

	public class GenericQueryRightParenOperand : GenericQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public GenericQueryRightParenOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public GenericQueryRightParenOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}
	
		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Black, ")");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	#endregion

	#region AttrQueryOperand Classes
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	// Attribute Query labels
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	
	/// <summary>
	/// Abstract class that defines the base AttrQueryOperand class.  This class defines the base level functionality for a label that is displayed
	/// in the attribute query panel.
	/// </summary>

	abstract public class AttrQueryOperand : QueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public AttrQueryOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Abstract class that defines an attribute.
	/// </summary>

	abstract public class AttrQueryAttributeOperand : AttrQueryOperand
	{
		//=======
		// FIELDS
		//=======

		private int _attributeRID;
		protected AttrQueryAttributeMainOperand _attrMainOperand;

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryAttributeOperand(FilterDefinition aFilterDef, int aAttributeRID)
			: base(aFilterDef)
		{
			try
			{
				_attributeRID = aAttributeRID;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public AttrQueryAttributeOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
			try
			{
				_attributeRID = (int)aDataQueue.Dequeue();
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

		public AttrQueryAttributeMainOperand AttrMainOperand
		{
			get
			{
				return _attrMainOperand;
			}
		}

		public int AttributeRID
		{
			get
			{
				return _attributeRID;
			}
		}

		//========
		// METHODS
		//========

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			dataQueue = base.GetDBDataQueue();
			dataQueue.Enqueue(_attributeRID);

			return dataQueue;
		}
	}

	/// <summary>
	/// Abstract class that defines a store.
	/// </summary>

	abstract public class AttrQueryStoreOperand : AttrQueryOperand
	{
		//=======
		// FIELDS
		//=======

		protected AttrQueryStoreMainOperand _storeMainOperand;

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryStoreOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public AttrQueryStoreOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		public AttrQueryStoreMainOperand StoreMainOperand
		{
			get
			{
				return _storeMainOperand;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines the attribute portion of an attribute definition label.
	/// </summary>

	public class AttrQueryAttributeMainOperand : AttrQueryAttributeOperand
	{
		//=======
		// FIELDS
		//=======

		private StoreGroupProfile _storeGroupProfile;
		private ProfileList _attributeSetProfList;
		private ArrayList _attributeOperandList;
		private bool _rebuildOperands;
			
		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryAttributeMainOperand(FilterDefinition aFilterDef, int aAttributeRID)
			: base(aFilterDef, aAttributeRID)
		{
			try
			{
				_attributeSetProfList = new ProfileList(eProfileType.StoreGroupLevel);

				Initialize();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public AttrQueryAttributeMainOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
			int key;
			StoreGroupLevelProfile storeGrpLvlProf;

			try
			{
				_attributeSetProfList = new ProfileList(eProfileType.StoreGroupLevel);

				while (aDataQueue.Count > 0)
				{
					key = (int)aDataQueue.Dequeue();
					storeGrpLvlProf = _filterDef.SAB.StoreServerSession.GetStoreGroupLevel(key);

					if (storeGrpLvlProf == null)
					{
						_outdatedInformation = true;
					}
					else
					{
						_attributeSetProfList.Add(storeGrpLvlProf);
					}
				}

				Initialize();
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

		public ProfileList AttributeSetProfList
		{
			get
			{
				return _attributeSetProfList;
			}
		}
        //Begin TT#3196-MD -jsobek -Filter XREF is not being updated with new Filter Creation or Updates
        public StoreGroupProfile StoreGroupProfile
        {
            get
            {
                return _storeGroupProfile;
            }
        }
        //End TT#3196-MD -jsobek -Filter XREF is not being updated with new Filter Creation or Updates

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, _storeGroupProfile.Name + " = ");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void CheckOperand()
		{
			base.CheckOperand();

			if (_attributeSetProfList.Count == 0)
			{
				throw new FilterOperandErrorException(_filterDef.CurrentSession.Audit.GetText(eMIDTextCode.msg_NoSetsAssigned));
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
			foreach (object obj in GetAttributeOperands())
			{
				aOutList.Add(obj);
			}
		}

		private void Initialize()
		{
			try
			{
				_attrMainOperand = this;
				_storeGroupProfile = _filterDef.SAB.StoreServerSession.GetStoreGroup(AttributeRID);

				if (_storeGroupProfile == null)
				{
					_validCreate = false;
					_outdatedInformation = true;
				}
				else
				{
					BuildAttributeOperands();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			try
			{
				dataQueue = base.GetDBDataQueue();

				foreach (StoreGroupLevelProfile strGrpLvlProf in _attributeSetProfList)
				{
					dataQueue.Enqueue(strGrpLvlProf.Key);
				}

				return dataQueue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void AddAllSets()
		{
			try
			{
				_attributeSetProfList.Clear();

				foreach (StoreGroupLevelProfile strGrpLvlProf in _storeGroupProfile.GroupLevels)
				{
					_attributeSetProfList.Add(strGrpLvlProf);
				}

				_rebuildOperands = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void AddSet(int aSetRID)
		{
			StoreGroupLevelProfile storeGrpLvlProf;

			try
			{
				storeGrpLvlProf = _filterDef.SAB.StoreServerSession.GetStoreGroupLevel(aSetRID);

				if (!_attributeSetProfList.Contains(storeGrpLvlProf))
				{
					_attributeSetProfList.Add(storeGrpLvlProf);
					_rebuildOperands = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void RemoveSet(StoreGroupLevelProfile aStoreGrpLvlProf)
		{
			try
			{
				if (_attributeSetProfList.Contains(aStoreGrpLvlProf))
				{
					_attributeSetProfList.Remove(aStoreGrpLvlProf);
					_rebuildOperands = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ArrayList GetAttributeOperands()
		{
			try
			{
				if (_rebuildOperands)
				{
					BuildAttributeOperands();
					_rebuildOperands = false;
				}

				return _attributeOperandList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildAttributeOperands()
		{
			bool firstPass;
			QueryOperand QueryOperand;

			try
			{
				_attributeOperandList = new ArrayList();
				_attributeOperandList.Add(this);
			
				firstPass = true;
				foreach (StoreGroupLevelProfile strGrpLvlProf in _attributeSetProfList)
				{
					if (firstPass)
					{
						firstPass = false;
					}
					else
					{
						_attributeOperandList.Add(new AttrQueryAttributeSeparatorOperand(_filterDef, this, AttributeRID));
					}

					QueryOperand = new AttrQueryAttributeDetailOperand(_filterDef, this, AttributeRID, strGrpLvlProf);

					if (QueryOperand.ValidCreate)
					{
						_attributeOperandList.Add(QueryOperand);
					}

					if (QueryOperand.OutdatedInformation)
					{
						_outdatedInformation = true;
					}
				}

				_attributeOperandList.Add(new AttrQueryAttributeEndOperand(_filterDef, this, AttributeRID));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the attribute set portion of an attribute definition label.
	/// </summary>

	public class AttrQueryAttributeDetailOperand : AttrQueryAttributeOperand
	{
		//=======
		// FIELDS
		//=======

		StoreGroupLevelProfile _storeGrpLvlProf;

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryAttributeDetailOperand(FilterDefinition aFilterDef, AttrQueryAttributeMainOperand aAttrMainOperand, int aAttributeRID, StoreGroupLevelProfile aStoreGrpLvlProf)
			: base(aFilterDef, aAttributeRID)
		{
			try
			{
				_attrMainOperand = aAttrMainOperand;
				_storeGrpLvlProf = aStoreGrpLvlProf;

				if (_storeGrpLvlProf == null)
				{
					_outdatedInformation = true;
					_validCreate = false;
				}
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

		public override bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, "'" + _storeGrpLvlProf.Name + "'");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnDelete()
		{
			_attrMainOperand.RemoveSet(_storeGrpLvlProf);
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
        public StoreGroupLevelProfile GetGroupLevelProfile()
        {
            return _storeGrpLvlProf; 
        }
        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

	}

	/// <summary>
	/// Class that defines the separator portion of an attribute definition label.
	/// </summary>

	public class AttrQueryAttributeSeparatorOperand : AttrQueryAttributeOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryAttributeSeparatorOperand(FilterDefinition aFilterDef, AttrQueryAttributeMainOperand aAttrMainOperand, int aAttributeRID)
			: base(aFilterDef, aAttributeRID)
		{
			_attrMainOperand = aAttrMainOperand;
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, " OR ");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Class that defines the end portion of an attribute definition label.
	/// </summary>

	public class AttrQueryAttributeEndOperand : AttrQueryAttributeOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryAttributeEndOperand(FilterDefinition aFilterDef, AttrQueryAttributeMainOperand aAttrMainOperand, int aAttributeRID)
			: base(aFilterDef, aAttributeRID)
		{
			_attrMainOperand = aAttrMainOperand;
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, "");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Class that defines the store label portion of a store definition label.
	/// </summary>

	public class AttrQueryStoreMainOperand : AttrQueryStoreOperand
	{
		//=======
		// FIELDS
		//=======

		private ProfileList _storeProfList;
		private ArrayList _AttrQueryStoreOperandList;
		private bool _rebuildOperands;
			
		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryStoreMainOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
			try
			{
				_storeProfList = new ProfileList(eProfileType.Store);

				Initialize();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public AttrQueryStoreMainOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
			int key;
			StoreProfile storeProf;

			try
			{
				_storeProfList = new ProfileList(eProfileType.Store);

				while (aDataQueue.Count > 0)
				{
					key = (int)aDataQueue.Dequeue();
					storeProf = _filterDef.SAB.StoreServerSession.GetStoreProfile(key);

					if (storeProf == null)
					{
						_outdatedInformation = true;
					}
					else
					{
						_storeProfList.Add(storeProf);
					}
				}

				Initialize();
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

		public ProfileList StoreProfileList
		{
			get
			{
				return _storeProfList;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Red, "Store = ");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void CheckOperand()
		{
			base.CheckOperand();

			if (_storeProfList.Count == 0)
			{
				throw new FilterOperandErrorException(_filterDef.CurrentSession.Audit.GetText(eMIDTextCode.msg_NoStoresAssigned));
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
			foreach (object obj in GetAttrQueryStoreOperands())
			{
				aOutList.Add(obj);
			}
		}

		private void Initialize()
		{
			try
			{
				_storeMainOperand = this;
				BuildStoreOperands();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			try
			{
				dataQueue = base.GetDBDataQueue();

				foreach (StoreProfile storeProf in _storeProfList)
				{
					dataQueue.Enqueue(storeProf.Key);
				}

				return dataQueue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void AddStore(int aStoreId)
		{
			StoreProfile storeProf;

			try
			{
				storeProf = _filterDef.SAB.StoreServerSession.GetStoreProfile(aStoreId);

				if (!_storeProfList.Contains(storeProf))
				{
					_storeProfList.Add(storeProf);
					_rebuildOperands = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void RemoveStore(StoreProfile aStoreProf)
		{
			try
			{
				if (_storeProfList.Contains(aStoreProf))
				{
					_storeProfList.Remove(aStoreProf);
					_rebuildOperands = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ArrayList GetAttrQueryStoreOperands()
		{
			try
			{
				if (_rebuildOperands)
				{
					BuildStoreOperands();
					_rebuildOperands = false;
				}

				return _AttrQueryStoreOperandList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildStoreOperands()
		{
			bool firstPass;
			QueryOperand QueryOperand;

			try
			{
				_AttrQueryStoreOperandList = new ArrayList();
				_AttrQueryStoreOperandList.Add(this);
			
				firstPass = true;
				foreach (StoreProfile storeProf in _storeProfList)
				{
					if (firstPass)
					{
						firstPass = false;
					}
					else
					{
						_AttrQueryStoreOperandList.Add(new AttrQueryStoreSeparatorOperand(_filterDef, this));
					}

					QueryOperand = new AttrQueryStoreDetailOperand(_filterDef, this, storeProf);

					if (QueryOperand.ValidCreate)
					{
						_AttrQueryStoreOperandList.Add(QueryOperand);
					}

					if (QueryOperand.OutdatedInformation)
					{
						_outdatedInformation = true;
					}
				}

				_AttrQueryStoreOperandList.Add(new AttrQueryStoreEndOperand(_filterDef, this));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the store portion of a store definition label.
	/// </summary>

	public class AttrQueryStoreDetailOperand : AttrQueryStoreOperand
	{
		//=======
		// FIELDS
		//=======

		StoreProfile _storeProf;

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryStoreDetailOperand(FilterDefinition aFilterDef, AttrQueryStoreMainOperand aStoreMainOperand, StoreProfile aStoreProfile)
			: base(aFilterDef)
		{
			try
			{
				_storeMainOperand = aStoreMainOperand;
				_storeProf = aStoreProfile;

				if (_storeProf == null)
				{
					_outdatedInformation = true;
					_validCreate = false;
				}
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

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
                // Begin TT#35 - JSmith - includes store name but not number
                //return _filterDef.LabelCreator(_filterDef, this, Color.Red, "'" + _storeProf.StoreName + "'");
                return _filterDef.LabelCreator(_filterDef, this, Color.Red, "'" + _storeProf.Text + "'");
                // End TT#35
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnDelete()
		{
			_storeMainOperand.RemoveStore(_storeProf);
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}

        public StoreProfile GetStoreProfile()
        {
            return _storeProf;
        }
	}

	/// <summary>
	/// Class that defines the separator portion of a store definition label.
	/// </summary>

	public class AttrQueryStoreSeparatorOperand : AttrQueryStoreOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryStoreSeparatorOperand(FilterDefinition aFilterDef, AttrQueryStoreMainOperand aStoreMainOperand)
			: base(aFilterDef)
		{
			_storeMainOperand = aStoreMainOperand;
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Red, " OR ");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Class that defines the end portion of a store definition label.
	/// </summary>

	public class AttrQueryStoreEndOperand : AttrQueryStoreOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQueryStoreEndOperand(FilterDefinition aFilterDef, AttrQueryStoreMainOperand aStoreMainOperand)
			: base(aFilterDef)
		{
			_storeMainOperand = aStoreMainOperand;
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Red, "");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Abstract class that defines a spacer.
	/// </summary>

	public class AttrQuerySpacerOperand : AttrQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AttrQuerySpacerOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public AttrQuerySpacerOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Red, "");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}
	#endregion

	#region DataQueryOperand Classes
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	// Data Query labels
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================

	/// <summary>
	/// Abstract class that defines the base DataQueryOperand class.  This class defines the base level functionality for a label that is displayed
	/// in the data query panel.
	/// </summary>

	abstract public class DataQueryOperand : QueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public DataQueryOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Abstract class that defines a plan
	/// </summary>

	abstract public class DataQueryPlanOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		protected DataQueryVariableOperand _variableOperand;

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryPlanOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public DataQueryPlanOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef)
		{
			_variableOperand = aVariableOperand;
		}

		public DataQueryPlanOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		public DataQueryVariableOperand VariableOperand
		{
			get
			{
				return _variableOperand;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines the main portion of a plan label.
	/// </summary>

	abstract public class DataQueryVariableOperand : DataQueryPlanOperand
	{
		//=======
		// FIELDS
		//=======

		protected HierarchyNodeProfile _nodeProf;
		protected VersionProfile _versionProf;
		protected DateRangeProfile _dateRangeProf;
		protected eFilterCubeModifyer _cubeModifyer;
		protected eFilterTimeModifyer _timeModifyer;

		protected ArrayList _planOperandList;
		protected PlanCubeGroup _planCubeGroup;

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryVariableOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
			try
			{
				_nodeProf = null;
				_versionProf = null;
				_dateRangeProf = null;
				_cubeModifyer = eFilterCubeModifyer.None;
				_timeModifyer = eFilterTimeModifyer.None;

				Initialize();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataQueryVariableOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef)
		{
			int key;

			try
			{
				key = (int)aDataQueue.Dequeue();
				if (key != -1)
				{

                //Begin TT#1916 - MD - Color display incomplete -RBeck
                    //_nodeProf = _filterDef.SAB.HierarchyServerSession.GetNodeData(key, true, false);
                    _nodeProf  =  _filterDef.SAB.HierarchyServerSession.GetNodeData(key,true,true);
                //End   TT#1916 - MD - Color display incomplete -RBeck

					if (_nodeProf == null)
					{
						_outdatedInformation = true;
					}
				}
				else
				{
					_nodeProf = null;
				}

				key = (int)aDataQueue.Dequeue();
				if (key != -1)
				{
					_versionProf = (VersionProfile)((StoreFilterDefinition)_filterDef).VersionProfileList.FindKey(key);

					if (_versionProf == null)
					{
						_outdatedInformation = true;
					}
				}
				else
				{
					_versionProf = null;
				}

				key = (int)aDataQueue.Dequeue();
				if (key != -1)
				{
					_dateRangeProf = _filterDef.CurrentSession.Calendar.GetDateRange(key, _filterDef.CurrentSession.Calendar.CurrentDate);

                    //Begin TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed
                    //if (_dateRangeProf == null)
					if (_dateRangeProf == null || _dateRangeProf.Key == 1)
                    //End TT#1818 - DOConnell - Delete of Date Range referenced in a filter is allowed
					{
						_outdatedInformation = true;
					}
				}
				else
				{
					_dateRangeProf = null;
				}

				_cubeModifyer = (eFilterCubeModifyer)aDataQueue.Dequeue();
				_timeModifyer = (eFilterTimeModifyer)aDataQueue.Dequeue();

				Initialize();
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

		public HierarchyNodeProfile NodeProfile
		{
			get
			{
				return _nodeProf;
			}
			set
			{
				_nodeProf = value;
				_planOperandList = null;
			}
		}

		public VersionProfile VersionProfile
		{
			get
			{
				return _versionProf;
			}
			set
			{
				_versionProf = value;
				_planOperandList = null;
			}
		}

		public DateRangeProfile DateRangeProfile
		{
			get
			{
				return _dateRangeProf;
			}
			set
			{
				_dateRangeProf = value;
				_planOperandList = null;
			}
		}

		public eFilterCubeModifyer CubeModifyer
		{
			get
			{
				return _cubeModifyer;
			}
			set
			{
				_cubeModifyer = value;
				_planOperandList = null;
			}
		}
		
		public eFilterTimeModifyer TimeModifyer
		{
			get
			{
				return _timeModifyer;
			}
			set
			{
				_timeModifyer = value;
				_planOperandList = null;
			}
		}

		public PlanCubeGroup PlanCubeGroup
		{
			get
			{
				return _planCubeGroup;
			}
			set
			{
				_planCubeGroup = value;
			}
		}

		public bool isVariableChanged
		{
			get
			{
				return _nodeProf != null || _versionProf != null || _dateRangeProf != null || _timeModifyer != eFilterTimeModifyer.None || _cubeModifyer != eFilterCubeModifyer.None;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckOperand()
		{
			base.CheckOperand();

			if (_dateRangeProf != null)
			{
				if (_dateRangeProf.Key == Include.UndefinedCalendarDateRange)
				{
					throw new FilterOperandErrorException(_filterDef.CurrentSession.Audit.GetText(eMIDTextCode.msg_DateRangeNotDefined));
				}
			}
			
			if (_nodeProf != null)
			{
				if (_nodeProf.Key == Include.NoRID)
				{
					throw new FilterOperandErrorException(_filterDef.CurrentSession.Audit.GetText(eMIDTextCode.msg_MerchandiseRequired));
				}
			}
		}

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			dataQueue = base.GetDBDataQueue();
			if (_nodeProf != null)
			{
				dataQueue.Enqueue(_nodeProf.Key);
			}
			else
			{
				dataQueue.Enqueue(-1);
			}

			if (_versionProf != null)
			{
				dataQueue.Enqueue(_versionProf.Key);
			}
			else
			{
				dataQueue.Enqueue(-1);
			}

			if (_dateRangeProf != null)
			{
				dataQueue.Enqueue(_dateRangeProf.Key);
			}
			else
			{
				dataQueue.Enqueue(-1);
			}

			dataQueue.Enqueue(_cubeModifyer);
			dataQueue.Enqueue(_timeModifyer);

			return dataQueue;
		}

		override public void OnRedraw(ArrayList aOutList)
		{
			foreach (object obj in GetPlanOperands())
			{
				aOutList.Add(obj);
			}
		}

		public ArrayList GetPlanOperands()
		{
			try
			{
				if (_planOperandList == null)
				{
					BuildOperandList();
				}
				
				return _planOperandList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void Initialize()
		{
			try
			{
				_variableOperand = this;
				_planCubeGroup = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildOperandList()
		{
			bool needSeparator;

			try
			{
				needSeparator = false;

				_planOperandList = new ArrayList();
				_planOperandList.Add(this);

				_planOperandList.Add(new DataQueryPlanBeginOperand(_filterDef, this));

				if (_nodeProf != null)
				{
					_planOperandList.Add(new DataQueryNodeOperand(_filterDef, this));
					needSeparator = true;
				}

				if (_versionProf != null)
				{
					if (needSeparator)
					{
						_planOperandList.Add(new DataQueryPlanSeparatorOperand(_filterDef, this));
					}
					_planOperandList.Add(new DataQueryVersionOperand(_filterDef, this));
					needSeparator = true;
				}

				if (_dateRangeProf != null)
				{
					if (needSeparator)
					{
						_planOperandList.Add(new DataQueryPlanSeparatorOperand(_filterDef, this));
					}
					_planOperandList.Add(new DataQueryDateRangeOperand(_filterDef, this));
					needSeparator = true;
				}

				if (_cubeModifyer != eFilterCubeModifyer.None)
				{
					if (needSeparator)
					{
						_planOperandList.Add(new DataQueryPlanSeparatorOperand(_filterDef, this));
					}
					_planOperandList.Add(new DataQueryCubeModifyerOperand(_filterDef, this, _cubeModifyer));
					needSeparator = true;
				}

				if (_timeModifyer != eFilterTimeModifyer.None)
				{
					if (needSeparator)
					{
						_planOperandList.Add(new DataQueryPlanSeparatorOperand(_filterDef, this));
					}
					_planOperandList.Add(new DataQueryTimeModifyerOperand(_filterDef, this, _timeModifyer));
				}

				_planOperandList.Add(new DataQueryPlanEndOperand(_filterDef, this));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines a Variable label.
	/// </summary>

	public class DataQueryPlanVariableOperand : DataQueryVariableOperand
	{
		//=======
		// FIELDS
		//=======

		private VariableProfile _varProf;

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryPlanVariableOperand(FilterDefinition aFilterDef, VariableProfile aVarProf)
			: base(aFilterDef)
		{
			try
			{
				_varProf = aVarProf;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataQueryPlanVariableOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
			try
			{
				_varProf = (VariableProfile)((StoreFilterDefinition)_filterDef).VariableProfileList.FindKey((int)aDataQueue.Dequeue());

				if (_varProf == null)
				{
					_outdatedInformation = true;
					_validCreate = false;
				}
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

		public VariableProfile VariableProfile
		{
			get
			{
				return _varProf;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, _varProf.VariableName);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			try
			{
				dataQueue = base.GetDBDataQueue();
				dataQueue.Enqueue(_varProf.Key);

				return dataQueue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines a Variable label.
	/// </summary>

	public class DataQueryTimeTotalVariableOperand : DataQueryVariableOperand
	{
		//=======
		// FIELDS
		//=======

		private TimeTotalVariableProfile _timeTotVarProf;
		private VariableProfile _varProf;
		private int _timeTotIdx;

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryTimeTotalVariableOperand(FilterDefinition aFilterDef, TimeTotalVariableProfile aTimeTotVarProf, VariableProfile aVarProf, int aTimeTotIdx)
			: base(aFilterDef)
		{
			try
			{
				_timeTotVarProf = aTimeTotVarProf;
				_varProf = aVarProf;
				_timeTotIdx = aTimeTotIdx;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataQueryTimeTotalVariableOperand(FilterDefinition aFilterDef, TimeTotalVariableReference aTimeTotVarRef)
			: base(aFilterDef)
		{
			try
			{
				_timeTotVarProf = aTimeTotVarRef.TimeTotalVariableProfile;
				_varProf = aTimeTotVarRef.VariableProfile;
				_timeTotIdx = aTimeTotVarRef.TimeTotalIndex;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataQueryTimeTotalVariableOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
			try
			{
				_timeTotVarProf = (TimeTotalVariableProfile)((StoreFilterDefinition)_filterDef).TimeTotalVariableProfileList.FindKey((int)aDataQueue.Dequeue());

				if (_timeTotVarProf == null)
				{
					_outdatedInformation = true;
					_validCreate = false;
				}
				
				_varProf = (VariableProfile)((StoreFilterDefinition)_filterDef).VariableProfileList.FindKey((int)aDataQueue.Dequeue());
				
				if (_varProf == null)
				{
					_outdatedInformation = true;
					_validCreate = false;
				}
				
				_timeTotIdx = (int)aDataQueue.Dequeue();
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

		public TimeTotalVariableProfile TimeTotalVariableProfile
		{
			get
			{
				return _timeTotVarProf;
			}
		}

		public VariableProfile VariableProfile
		{
			get
			{
				return _varProf;
			}
		}
		
		public int TimeTotalIndex
		{
			get
			{
				return _timeTotIdx;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, _timeTotVarProf.VariableName);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			try
			{
				dataQueue = base.GetDBDataQueue();
				dataQueue.Enqueue(_timeTotVarProf.Key);
				dataQueue.Enqueue(_varProf.Key);
				dataQueue.Enqueue(_timeTotIdx);

				return dataQueue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the end portion of a store definition label.
	/// </summary>

	public class DataQueryPlanBeginOperand : DataQueryPlanOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryPlanBeginOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand)
		{
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				if (VariableOperand.isVariableChanged)
				{
					return _filterDef.LabelCreator(_filterDef, this, Color.Green, "(");
				}
				else
				{
					return _filterDef.LabelCreator(_filterDef, this, Color.Green, "");
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Class that defines the separator portion of a definition label.
	/// </summary>

	public class DataQueryPlanSeparatorOperand : DataQueryPlanOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryPlanSeparatorOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand)
		{
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, ",");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Class that defines the end portion of a store definition label.
	/// </summary>

	public class DataQueryPlanEndOperand : DataQueryPlanOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryPlanEndOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand)
		{
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				if (VariableOperand.isVariableChanged)
				{
					return _filterDef.LabelCreator(_filterDef, this, Color.Green, ")");
				}
				else
				{
					return _filterDef.LabelCreator(_filterDef, this, Color.Green, "");
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Abstract class that defines the base DataQueryOperand class.  This class defines the base level functionality for a label that is displayed
	/// in the data query panel.
	/// </summary>

	public class DataQueryNodeOperand : DataQueryPlanOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryNodeOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public DataQueryNodeOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand)
		{
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
			//Begin TT#1916 - MD - Color display incomplete -RBeck
                return _filterDef.LabelCreator(_filterDef, this, Color.Green, VariableOperand.NodeProfile.Text);
                //return _filterDef.LabelCreator(_filterDef, this, Color.Green, VariableOperand.NodeProfile.NodeName);
            //End   TT#1916 - MD - Color display incomplete -RBeck
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnDelete()
		{
			VariableOperand.NodeProfile = null;
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
        public HierarchyNodeProfile GetNodeProfile()
        {
            return base.VariableOperand.NodeProfile;  
        }
        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

	}

	/// <summary>
	/// Abstract class that defines the base DataQueryOperand class.  This class defines the base level functionality for a label that is displayed
	/// in the data query panel.
	/// </summary>

	public class DataQueryVersionOperand : DataQueryPlanOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryVersionOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand)
		{
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}
		
		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, VariableOperand.VersionProfile.Description);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnDelete()
		{
			VariableOperand.VersionProfile = null;
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
        public VersionProfile GetVersionProfile()
        {
            return base.VariableOperand.VersionProfile;
        }
        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

	}

	/// <summary>
	/// Abstract class that defines the base DataQueryOperand class.  This class defines the base level functionality for a label that is displayed
	/// in the data query panel.
	/// </summary>

	public class DataQueryDateRangeOperand : DataQueryPlanOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryDateRangeOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public DataQueryDateRangeOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand)
		{
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, VariableOperand.DateRangeProfile.DisplayDate);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnDelete()
		{
			VariableOperand.DateRangeProfile = null;
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
        public DateRangeProfile GetDateRangeProfile()
        {
            return base.VariableOperand.DateRangeProfile;
        }
        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
	}

	/// <summary>
	/// Class that defines an Store Detail label.
	/// </summary>

	public class DataQueryCubeModifyerOperand : DataQueryPlanOperand
	{
		//=======
		// FIELDS
		//=======

		private eFilterCubeModifyer _cubeModifyer;

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryCubeModifyerOperand(FilterDefinition aFilterDef, eFilterCubeModifyer aCubeModifyer)
			: base(aFilterDef)
		{
			_cubeModifyer = aCubeModifyer;
		}

		public DataQueryCubeModifyerOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand, eFilterCubeModifyer aCubeModifyer)
			: base(aFilterDef, aVariableOperand)
		{
			_cubeModifyer = aCubeModifyer;
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		public eFilterCubeModifyer CubeModifyer
		{
			get
			{
				return _cubeModifyer;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			string labelText;

			try
			{
				switch (_cubeModifyer)
				{
					case eFilterCubeModifyer.StoreDetail:
						labelText = "Store Detail";
						break;

					case eFilterCubeModifyer.StoreTotal:
						labelText = "Store Total";
						break;

					case eFilterCubeModifyer.StoreAverage:
						labelText = "Store Average";
						break;
					
					case eFilterCubeModifyer.ChainDetail:
						labelText = "Chain Detail";
						break;
					
					default:
						labelText = "Undefined";
						break;
				}

				return _filterDef.LabelCreator(_filterDef, this, Color.Green, labelText);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnDelete()
		{
			VariableOperand.CubeModifyer = eFilterCubeModifyer.None;
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Class that defines an Store Detail label.
	/// </summary>

	public class DataQueryStoreDetailOperand : DataQueryCubeModifyerOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryStoreDetailOperand(FilterDefinition aFilterDef)
			: base(aFilterDef, eFilterCubeModifyer.StoreDetail)
		{
		}

		public DataQueryStoreDetailOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand, eFilterCubeModifyer.StoreDetail)
		{
		}
	
		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines an Store Total label.
	/// </summary>

	public class DataQueryStoreTotalOperand : DataQueryCubeModifyerOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryStoreTotalOperand(FilterDefinition aFilterDef)
			: base(aFilterDef, eFilterCubeModifyer.StoreTotal)
		{
		}

		public DataQueryStoreTotalOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand, eFilterCubeModifyer.StoreTotal)
		{
		}
	
		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines an Store Average label.
	/// </summary>

	public class DataQueryStoreAverageOperand : DataQueryCubeModifyerOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryStoreAverageOperand(FilterDefinition aFilterDef)
			: base(aFilterDef, eFilterCubeModifyer.StoreAverage)
		{
		}

		public DataQueryStoreAverageOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand, eFilterCubeModifyer.StoreAverage)
		{
		}
	
		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines an Chain Detail label.
	/// </summary>

	public class DataQueryChainDetailOperand : DataQueryCubeModifyerOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryChainDetailOperand(FilterDefinition aFilterDef)
			: base(aFilterDef, eFilterCubeModifyer.ChainDetail)
		{
		}

		public DataQueryChainDetailOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand, eFilterCubeModifyer.ChainDetail)
		{
		}
	
		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines an Store Detail label.
	/// </summary>

	public class DataQueryTimeModifyerOperand : DataQueryPlanOperand
	{
		//=======
		// FIELDS
		//=======

		private eFilterTimeModifyer _timeModifyer;

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryTimeModifyerOperand(FilterDefinition aFilterDef, eFilterTimeModifyer aTimeModifyer)
			: base(aFilterDef)
		{
			_timeModifyer = aTimeModifyer;
		}

		public DataQueryTimeModifyerOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand, eFilterTimeModifyer aTimeModifyer)
			: base(aFilterDef, aVariableOperand)
		{
			_timeModifyer = aTimeModifyer;
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		public eFilterTimeModifyer TimeModifyer
		{
			get
			{
				return _timeModifyer;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			string labelText;

			try
			{
				switch (_timeModifyer)
				{
					case eFilterTimeModifyer.Any:
						labelText = "Any";
						break;

					case eFilterTimeModifyer.All:
						labelText = "All";
						break;

//Begin Track #5111 - JScott - Add additional filter functionality
					case eFilterTimeModifyer.Join:
						labelText = "Join";
						break;

//End Track #5111 - JScott - Add additional filter functionality
					case eFilterTimeModifyer.Average:
						labelText = "Average";
						break;
					
					case eFilterTimeModifyer.Total:
						labelText = "Total";
						break;
					
					case eFilterTimeModifyer.Corresponding:
						labelText = "Corresponding";
						break;
					
					default:
						labelText = "Undefined";
						break;
				}

				return _filterDef.LabelCreator(_filterDef, this, Color.Green, labelText);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnDelete()
		{
			VariableOperand.TimeModifyer = eFilterTimeModifyer.None;
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Class that defines an Any label.
	/// </summary>

	public class DataQueryAnyOperand : DataQueryTimeModifyerOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryAnyOperand(FilterDefinition aFilterDef)
			: base(aFilterDef, eFilterTimeModifyer.Any)
		{
		}

		public DataQueryAnyOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand, eFilterTimeModifyer.Any)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines an All label.
	/// </summary>

	public class DataQueryAllOperand : DataQueryTimeModifyerOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryAllOperand(FilterDefinition aFilterDef)
			: base(aFilterDef, eFilterTimeModifyer.All)
		{
		}

		public DataQueryAllOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand, eFilterTimeModifyer.All)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

//Begin Track #5111 - JScott - Add additional filter functionality
	/// <summary>
	/// Class that defines an AnyJoined label.
	/// </summary>

	public class DataQueryJoinOperand : DataQueryTimeModifyerOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryJoinOperand(FilterDefinition aFilterDef)
			: base(aFilterDef, eFilterTimeModifyer.Join)
		{
		}

		public DataQueryJoinOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand, eFilterTimeModifyer.Join)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

//End Track #5111 - JScott - Add additional filter functionality
	/// <summary>
	/// Class that defines an Average label.
	/// </summary>

	public class DataQueryAverageOperand : DataQueryTimeModifyerOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryAverageOperand(FilterDefinition aFilterDef)
			: base(aFilterDef, eFilterTimeModifyer.Average)
		{
		}

		public DataQueryAverageOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand, eFilterTimeModifyer.Average)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines an Average label.
	/// </summary>

	public class DataQueryTotalOperand : DataQueryTimeModifyerOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryTotalOperand(FilterDefinition aFilterDef)
			: base(aFilterDef, eFilterTimeModifyer.Total)
		{
		}

		public DataQueryTotalOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand, eFilterTimeModifyer.Total)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines an Corresponding label.
	/// </summary>

	public class DataQueryCorrespondingOperand : DataQueryTimeModifyerOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryCorrespondingOperand(FilterDefinition aFilterDef)
			: base(aFilterDef, eFilterTimeModifyer.Corresponding)
		{
		}

		public DataQueryCorrespondingOperand(FilterDefinition aFilterDef, DataQueryVariableOperand aVariableOperand)
			: base(aFilterDef, aVariableOperand, eFilterTimeModifyer.Corresponding)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines an Equal label.
	/// </summary>

	public class DataQueryEqualOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryEqualOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public DataQueryEqualOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Blue, "=");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an LessThan label.
	/// </summary>

	public class DataQueryLessOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryLessOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}
	
		public DataQueryLessOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Blue, "<");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an GreaterThan label.
	/// </summary>

	public class DataQueryGreaterOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryGreaterOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}
	
		public DataQueryGreaterOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Blue, ">");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	
	/// <summary>
	/// Class that defines an LessThanOrEqual label.
	/// </summary>

	public class DataQueryLessEqualOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryLessEqualOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public DataQueryLessEqualOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Blue, "<=");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	
	/// <summary>
	/// Class that defines an GreaterThanOrEqual label.
	/// </summary>

	public class DataQueryGreaterEqualOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryGreaterEqualOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}
	
		public DataQueryGreaterEqualOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Blue, ">=");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	
	/// <summary>
	/// Class that defines an Not label.
	/// </summary>

	public class DataQueryNotOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryNotOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public DataQueryNotOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}
	
		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Blue, "NOT");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an Not label.
	/// </summary>

	public class DataQueryPctChangeOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryPctChangeOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public DataQueryPctChangeOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}
	
		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Blue, "% Chg");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an Not label.
	/// </summary>

	public class DataQueryPctOfOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryPctOfOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public DataQueryPctOfOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}
	
		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Blue, "% Of");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an Not label.
	/// </summary>

	public class DataQueryLiteralOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		private bool _defined;
		private double _literalValue;

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryLiteralOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
			try
			{
				_defined = false;
				_literalValue = 0;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataQueryLiteralOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
			try
			{
				_defined = (bool)aDataQueue.Dequeue();
				_literalValue = (double)aDataQueue.Dequeue();
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

		public bool Defined
		{
			get
			{
				return _defined;
			}
		}

		public double LiteralValue
		{
			get
			{
				return _literalValue;
			}
			set
			{
				_defined = true;
				_literalValue = value;
				if (Label != null)
				{
					Label.Text = GetText();
				}
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Red, GetText());
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private string GetText()
		{
			if (_defined)
			{
				return Convert.ToString(_literalValue, CultureInfo.CurrentUICulture);
			}
			else
			{
				return "<Quantity>";
			}
		}

		override public void CheckOperand()
		{
			base.CheckOperand();

			if (!_defined)
			{
				throw new FilterOperandErrorException(_filterDef.CurrentSession.Audit.GetText(eMIDTextCode.msg_QuantityNotDefined));
			}
		}

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			try
			{
				dataQueue = base.GetDBDataQueue();
				dataQueue.Enqueue(_defined);
				dataQueue.Enqueue(_literalValue);

				return dataQueue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an Not label.
	/// </summary>

	public class DataQueryGradeOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		private bool _defined;
		private string _gradeValue;

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryGradeOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
			try
			{
				_defined = false;
				_gradeValue = "";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataQueryGradeOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
			try
			{
				_defined = (bool)aDataQueue.Dequeue();
				_gradeValue = (string)aDataQueue.Dequeue();
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

		public bool Defined
		{
			get
			{
				return _defined;
			}
		}

		public string GradeValue
		{
			get
			{
				return _gradeValue;
			}
			set
			{
				_defined = true;
				_gradeValue = value;
				if (Label != null)
				{
					Label.Text = GetText();
				}
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Red, GetText());
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private string GetText()
		{
			if (_defined)
			{
				return Convert.ToString(_gradeValue, CultureInfo.CurrentUICulture);
			}
			else
			{
				return "<Grade>";
			}
		}

		override public void CheckOperand()
		{
			base.CheckOperand();

			if (!_defined)
			{
				throw new FilterOperandErrorException(_filterDef.CurrentSession.Audit.GetText(eMIDTextCode.msg_GradeNotDefined));
			}
		}

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			try
			{
				dataQueue = base.GetDBDataQueue();
				dataQueue.Enqueue(_defined);
				dataQueue.Enqueue(_gradeValue);

				return dataQueue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines an Not label.
	/// </summary>

	public class DataQueryStatusOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		private eStoreStatus _statusValue;
		private string _description;

		//=============
		// CONSTRUCTORS
		//=============

		public DataQueryStatusOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
			try
			{
				_statusValue = eStoreStatus.None;
				_description = string.Empty;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataQueryStatusOperand(FilterDefinition aFilterDef, eStoreStatus aStoreStatus)
			: base(aFilterDef)
		{
			try
			{
				_statusValue = aStoreStatus;
				_description = string.Empty;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataQueryStatusOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
			try
			{
				_statusValue = (eStoreStatus)aDataQueue.Dequeue();
				_description = string.Empty;
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

		public eStoreStatus StatusValue
		{
			get
			{
				return _statusValue;
			}
		}

		public string Description
		{
			get
			{
				if (_description == string.Empty)
				{
					_description = MIDText.GetTextOnly((int)_statusValue);
				}

				return _description;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Red, Description);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			try
			{
				dataQueue = base.GetDBDataQueue();
				dataQueue.Enqueue(_statusValue);

				return dataQueue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Abstract class that defines a spacer.
	/// </summary>

	public class DataQuerySpacerOperand : DataQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DataQuerySpacerOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public DataQuerySpacerOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Red, "");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}
	#endregion

	#region ProdCharQueryOperand Classes
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	// Product characteristic Query labels
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================
	//==================================================================================

	/// <summary>
	/// Abstract class that defines the base ProdCharQueryOperand class.  This class defines 
	/// the base level functionality for a label that is displayed
	/// in the product characteristic query panel.
	/// </summary>

	abstract public class ProdCharQueryOperand : QueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQueryOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public ProdCharQueryOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Abstract class that defines an attribute.
	/// </summary>

	abstract public class ProdCharQueryCharacteristicOperand : ProdCharQueryOperand
	{
		//=======
		// FIELDS
		//=======

		private int _characteristicRID;
		protected ProdCharQueryCharacteristicMainOperand _mainOperand;

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQueryCharacteristicOperand(FilterDefinition aFilterDef, int aCharacteristicRID)
			: base(aFilterDef)
		{
			try
			{
				_characteristicRID = aCharacteristicRID;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ProdCharQueryCharacteristicOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
			try
			{
				_characteristicRID = (int)aDataQueue.Dequeue();
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

		public ProdCharQueryCharacteristicMainOperand MainOperand
		{
			get
			{
				return _mainOperand;
			}
		}

		public int CharacteristicRID
		{
			get
			{
				return _characteristicRID;
			}
		}

		//========
		// METHODS
		//========

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			dataQueue = base.GetDBDataQueue();
			dataQueue.Enqueue(_characteristicRID);

			return dataQueue;
		}
	}

	/// <summary>
	/// Class that defines the attribute portion of an attribute definition label.
	/// </summary>

	public class ProdCharQueryCharacteristicMainOperand : ProdCharQueryCharacteristicOperand
	{
		//=======
		// FIELDS
		//=======

		private ProductCharProfile _productCharProfile;
		private ProfileList _productCharValueProfList;
		private ArrayList _operandList;
		private bool _rebuildOperands;

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQueryCharacteristicMainOperand(FilterDefinition aFilterDef, int aCharacteristicRID)
			: base(aFilterDef, aCharacteristicRID)
		{
			try
			{
				_productCharValueProfList = new ProfileList(eProfileType.ProductCharacteristicValue);

				Initialize();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ProdCharQueryCharacteristicMainOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
			int key;
			ProductCharValueProfile productCharValueProf;

			try
			{
				_productCharValueProfList = new ProfileList(eProfileType.ProductCharacteristicValue);

				while (aDataQueue.Count > 0)
				{
					key = (int)aDataQueue.Dequeue();
					productCharValueProf = _filterDef.SAB.HierarchyServerSession.GetProductCharValueProfile(key);

					if (productCharValueProf == null)
					{
						_outdatedInformation = true;
					}
					else
					{
						_productCharValueProfList.Add(productCharValueProf);
					}
				}

				Initialize();
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

		public ProfileList ProductCharValueProfList
		{
			get
			{
				return _productCharValueProfList;
			}
		}
        //Begin TT#3196-MD -jsobek -Filter XREF is not being updated with new Filter Creation or Updates
        public ProductCharProfile ProductCharProfileStored
        {
            get
			{
                return _productCharProfile;
			}
        }
        //End TT#3196-MD -jsobek -Filter XREF is not being updated with new Filter Creation or Updates
            

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, _productCharProfile.Text + " = ");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void CheckOperand()
		{
			base.CheckOperand();

			if (_productCharValueProfList.Count == 0)
			{
				throw new FilterOperandErrorException(_filterDef.CurrentSession.Audit.GetText(eMIDTextCode.msg_NoSetsAssigned));
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
			foreach (object obj in GetCharacteristicOperands())
			{
				aOutList.Add(obj);
			}
		}

		private void Initialize()
		{
			try
			{
				_mainOperand = this;
				_productCharProfile = _filterDef.SAB.HierarchyServerSession.GetProductCharProfile(CharacteristicRID);

				if (_productCharProfile == null)
				{
					_validCreate = false;
					_outdatedInformation = true;
				}
				else
				{
					BuildCharacteristicOperands();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public Queue GetDBDataQueue()
		{
			Queue dataQueue;

			try
			{
				dataQueue = base.GetDBDataQueue();

				foreach (ProductCharValueProfile productCharValueProf in _productCharValueProfList)
				{
					dataQueue.Enqueue(productCharValueProf.Key);
				}

				return dataQueue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void AddValue(int aValueRID)
		{
			ProductCharValueProfile productCharValueProfile;

			try
			{
				productCharValueProfile = _filterDef.SAB.HierarchyServerSession.GetProductCharValueProfile(aValueRID);

				if (!_productCharValueProfList.Contains(productCharValueProfile))
				{
					_productCharValueProfList.Add(productCharValueProfile);
					_rebuildOperands = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void RemoveValue(ProductCharValueProfile aProductCharValueProfile)
		{
			try
			{
				if (_productCharValueProfList.Contains(aProductCharValueProfile))
				{
					_productCharValueProfList.Remove(aProductCharValueProfile);
					_rebuildOperands = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ArrayList GetCharacteristicOperands()
		{
			try
			{
				if (_rebuildOperands)
				{
					BuildCharacteristicOperands();
					_rebuildOperands = false;
				}

				return _operandList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildCharacteristicOperands()
		{
			bool firstPass;
			QueryOperand QueryOperand;

			try
			{
				_operandList = new ArrayList();
				_operandList.Add(this);

				firstPass = true;
				foreach (ProductCharValueProfile productCharValueProf in _productCharValueProfList)
				{
					if (firstPass)
					{
						firstPass = false;
					}
					else
					{
						_operandList.Add(new ProdCharQueryCharacteristicSeparatorOperand(_filterDef, this, CharacteristicRID));
					}

					QueryOperand = new ProdCharQueryCharacteristicDetailOperand(_filterDef, this, CharacteristicRID, productCharValueProf);

					if (QueryOperand.ValidCreate)
					{
						_operandList.Add(QueryOperand);
					}

					if (QueryOperand.OutdatedInformation)
					{
						_outdatedInformation = true;
					}
				}

				_operandList.Add(new ProdCharQueryCharacteristicEndOperand(_filterDef, this, CharacteristicRID));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Class that defines the characteristic value portion of a characteristic definition label.
	/// </summary>

	public class ProdCharQueryCharacteristicDetailOperand : ProdCharQueryCharacteristicOperand
	{
		//=======
		// FIELDS
		//=======

		ProductCharValueProfile _productCharValueProfile;

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQueryCharacteristicDetailOperand(FilterDefinition aFilterDef, ProdCharQueryCharacteristicMainOperand aMainOperand, int aCharacteristicRID, ProductCharValueProfile aProductCharValueProf)
			: base(aFilterDef, aCharacteristicRID)
		{
			try
			{
				_mainOperand = aMainOperand;
				_productCharValueProfile = aProductCharValueProf;

				if (_productCharValueProfile == null)
				{
					_outdatedInformation = true;
					_validCreate = false;
				}
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

		public override bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========


		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, "'" + _productCharValueProfile.Text + "'");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnDelete()
		{
			//_mainOperand.RemoveSet(_storeGrpLvlProf);
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}

        //Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
        public ProductCharValueProfile GetProductCharValueProfile() 
        {
            return _productCharValueProfile;
        }
        //End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)

	}

	/// <summary>
	/// Class that defines the separator portion of a characteristic definition label.
	/// </summary>

	public class ProdCharQueryCharacteristicSeparatorOperand : ProdCharQueryCharacteristicOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQueryCharacteristicSeparatorOperand(FilterDefinition aFilterDef, ProdCharQueryCharacteristicMainOperand aMainOperand, int aCharacteristicRID)
			: base(aFilterDef, aCharacteristicRID)
		{
			_mainOperand = aMainOperand;
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, " OR ");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Class that defines the end portion of a characteristic definition label.
	/// </summary>

	public class ProdCharQueryCharacteristicEndOperand : ProdCharQueryCharacteristicOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQueryCharacteristicEndOperand(FilterDefinition aFilterDef, ProdCharQueryCharacteristicMainOperand aMainOperand, int aCharacteristicRID)
			: base(aFilterDef, aCharacteristicRID)
		{
			_mainOperand = aMainOperand;
		}

		//===========
		// PROPERTIES
		//===========

		override public bool isMainOperand
		{
			get
			{
				return false;
			}
		}

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Green, "");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void OnRedraw(ArrayList aOutList)
		{
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}

	/// <summary>
	/// Abstract class that defines a spacer.
	/// </summary>

	public class ProdCharQuerySpacerOperand : ProdCharQueryOperand
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ProdCharQuerySpacerOperand(FilterDefinition aFilterDef)
			: base(aFilterDef)
		{
		}

		public ProdCharQuerySpacerOperand(FilterDefinition aFilterDef, Queue aDataQueue)
			: base(aFilterDef, aDataQueue)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected Label CreateLabel()
		{
			try
			{
				return _filterDef.LabelCreator(_filterDef, this, Color.Red, "");
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public Queue GetDBDataQueue()
		{
			return null;
		}
	}
	#endregion
}
