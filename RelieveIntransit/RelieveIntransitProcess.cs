using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic; // TT#1185 Verify ENQ before Update
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Xml;
using System.Xml.Serialization;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.RelieveIntransit
{
	/// <summary>
	/// Summary description for RelieveIntransitProcess.
	/// </summary>
	public class RelieveIntransitProcess
	{

		private string sourceModule = "RelieveIntransitProcess.cs";

		SessionAddressBlock _SAB;

		private int _transIn = 0;
		private int _transErr = 0;
		private int _transOut = 0;

		private Audit _audit = null;

		private Hashtable _nodeIDs = null;
		private Hashtable _nodeRIDs = null;
		private Hashtable _storeIDs = null;

		private char[] _levelDelimiter = null;

		private bool _headerIDsPresent = false;
		private bool _relieveIntransitForAllStores; // MID Track 5694 Relieve Intransit by Header ID

        //private StoreServerSession _sss = null; //TT#1517-MD -jsobek -Store Service Optimization

		private HierarchyMaintenance _hm = null;

		private AllocationProfileList _apl = null;

		private DataRow _drRelieveIntransit = null;
		private DataSet _dsRelieveIntransit = null;
		private DataTable _dtRelieveIntransit = null;
		private DataColumn _dcRelieveIntransit = null;

		private ApplicationSessionTransaction _aTrans = null;
		private Header _headerData;
		private DataTable _headersAllocatedToReserve;    // MID Track 6250 Relieve Intransit Errors
        private int _currAssortmentHeaderRid = Include.NoRID;	// TT#1133-MD - stodd - Relieve IT not working with group allocation - 


		private Header HeaderData
		{
			get
			{
				if (_headerData == null) _headerData = new Header();
				return _headerData;
			}
		}

		public RelieveIntransitProcess(SessionAddressBlock SAB, ref bool errorFound, bool headerIDsPresent, char[] levelDelimiter, bool relieveIntransitForAllStores)  // MID Track 5694 Relieve Intransit By Header ID
		{

			try
			{
				_SAB = SAB;

				_nodeIDs = new Hashtable();
				_nodeIDs.Clear();

				_nodeRIDs = new Hashtable();
				_nodeRIDs.Clear();

				_storeIDs = new Hashtable();
				_storeIDs.Clear();

                //_sss = _SAB.StoreServerSession; //TT#1517-MD -jsobek -Store Service Optimization

				_levelDelimiter = levelDelimiter;

				_headerIDsPresent = headerIDsPresent;

				_hm = new HierarchyMaintenance(_SAB);

				_audit = _SAB.ClientServerSession.Audit;

                // begin TT#1185 - JEllis - Verify ENQ before update (part 2)
                //_aTrans = new ApplicationSessionTransaction(_SAB);
                _aTrans = _SAB.ApplicationServerSession.CreateTransaction();
                // end TT#1185 - JEllis - Verify ENQ before Update (part 2)

				_aTrans.NewAllocationMasterProfileList();
                _aTrans.NewAssortmentMemberMasterProfileList();	// TT#1133-MD - stodd - Relieve IT not working with group allocation - 

				_relieveIntransitForAllStores = relieveIntransitForAllStores; // MID Track 5694 Relieve Intransit By Header ID

				CreateDataTable();
			}

			catch (Exception Ex)
			{
				errorFound = true;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.Message, sourceModule);
			}
		}

		private void CreateDataTable()
		{

			try
			{
				// =======================================
				// Create the _dtRelieveIntransit DataTable
				// =======================================
				_dtRelieveIntransit = MIDEnvironment.CreateDataTable("RelieveIntransit");

				// ============================
				// Create the ROW_ID DataColumn
				// ============================
				_dcRelieveIntransit = new DataColumn();
				_dcRelieveIntransit.DataType = System.Type.GetType("System.Int32");
				_dcRelieveIntransit.ColumnName = "ROW_ID";
				_dcRelieveIntransit.AutoIncrement = true;
				_dcRelieveIntransit.AutoIncrementSeed = 1;
				_dcRelieveIntransit.AutoIncrementStep = 1;
				_dcRelieveIntransit.ReadOnly = true;
				_dcRelieveIntransit.Unique = true;
				_dtRelieveIntransit.Columns.Add(_dcRelieveIntransit);

				// ===============================
				// Create the HEADER_ID DataColumn
				// ===============================
				_dcRelieveIntransit = new DataColumn();
				_dcRelieveIntransit.DataType = System.Type.GetType("System.String");
				_dcRelieveIntransit.ColumnName = "HEADER_ID";
				_dcRelieveIntransit.AutoIncrement = false;
				_dcRelieveIntransit.ReadOnly = false;
				_dcRelieveIntransit.Unique = false;
				_dtRelieveIntransit.Columns.Add(_dcRelieveIntransit);

				// =================================
				// Create the NODE_RID DataColumn
				// =================================
				_dcRelieveIntransit = new DataColumn();
				_dcRelieveIntransit.DataType = System.Type.GetType("System.Int32");
				_dcRelieveIntransit.ColumnName = "NODE_RID";
				_dcRelieveIntransit.AutoIncrement = false;
				_dcRelieveIntransit.ReadOnly = false;
				_dcRelieveIntransit.Unique = false;
				_dtRelieveIntransit.Columns.Add(_dcRelieveIntransit);

				// ===============================
				// Create the STORE_RID DataColumn
				// ===============================
				_dcRelieveIntransit = new DataColumn();
				_dcRelieveIntransit.DataType = System.Type.GetType("System.Int32");
				_dcRelieveIntransit.ColumnName = "STORE_RID";
				_dcRelieveIntransit.AutoIncrement = false;
				_dcRelieveIntransit.ReadOnly = false;
				_dcRelieveIntransit.Unique = false;
				_dtRelieveIntransit.Columns.Add(_dcRelieveIntransit);

				// ===============================
				// Create the STORE_QTY DataColumn
				// ===============================
				_dcRelieveIntransit = new DataColumn();
				_dcRelieveIntransit.DataType = System.Type.GetType("System.Int32");
				_dcRelieveIntransit.ColumnName = "STORE_QTY";
				_dcRelieveIntransit.AutoIncrement = false;
				_dcRelieveIntransit.ReadOnly = false;
				_dcRelieveIntransit.Unique = false;
				_dtRelieveIntransit.Columns.Add(_dcRelieveIntransit);

				// Begin TT#1133-MD - stodd - Relieve IT not working with group allocation - 
                // =================================
                // Create the ASRT_RID DataColumn
                // =================================
                _dcRelieveIntransit = new DataColumn();
                _dcRelieveIntransit.DataType = System.Type.GetType("System.Int32");
                _dcRelieveIntransit.ColumnName = "ASRT_RID";
                _dcRelieveIntransit.AutoIncrement = false;
                _dcRelieveIntransit.ReadOnly = false;
                _dcRelieveIntransit.Unique = false;
                _dtRelieveIntransit.Columns.Add(_dcRelieveIntransit);
				// End TT#1133-MD - stodd - Relieve IT not working with group allocation - 

				// =============================================
				// Make the ROW_ID column the primary key column
				// =============================================
				DataColumn[] PrimaryKeyColumns = new DataColumn[1];
				PrimaryKeyColumns[0] = _dtRelieveIntransit.Columns["ROW_ID"];
				_dtRelieveIntransit.PrimaryKey = PrimaryKeyColumns;

				// ======================================
				// Create the _dsRelieveIntransit DataSet
				// ======================================
				_dsRelieveIntransit = MIDEnvironment.CreateDataSet();

				// ========================================================================
				// Add the _dtRelieveIntransit DataTable to the _dsRelieveIntransit DataSet
				// ========================================================================
				_dsRelieveIntransit.Tables.Add(_dtRelieveIntransit);
			}

			catch
			{
				throw;
			}
		}

		// ============================================
		// Process the delimited input transaction file
		// ============================================
		public eReturnCode ProcessVariableFile(string fileLocation, char[] delimiter, ref bool errorFound)
		{

			string line = null;
			string message = null;

			StreamReader txtReader = null;

			eReturnCode returnCode = eReturnCode.successful;

			try
			{
				txtReader = new StreamReader(fileLocation);

				while ((line = txtReader.ReadLine()) != null)
				{
					string[] fields = line.Split(delimiter);
					if (fields.Length == 1 && fields[0] == "")
					{
						continue;
					}
					++_transIn;
					returnCode = ParseDelimitedData(fields);
					if (returnCode != eReturnCode.successful)
					{
						++_transErr;
					}
				}

				if (_dtRelieveIntransit.Rows.Count > 0)
				{
					returnCode = ProcessDataTable();
				}
				else
				{
					message = "No transactions found to process" + System.Environment.NewLine;
					_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
				}
			}

			catch (Exception Ex)
			{
				errorFound = true;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.Message, sourceModule);

				returnCode = eReturnCode.severe;
			}

			finally
			{
                //Begin Track #5100 - JSmith - Add counts to audit
                //message = "Transactions Read In: " + _transIn.ToString() + System.Environment.NewLine;
                //message += "Transactions Accepted: " + _transOut.ToString() + System.Environment.NewLine;
                //message += "Transactions In Error: " + _transErr.ToString() + System.Environment.NewLine;
                //_audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                _audit.RelieveIntransitAuditInfo_Add(_transIn, _transOut, _transErr);
                // End Track #5100

				if (txtReader != null)
				{
					txtReader.Close();
				}
			}

			return returnCode;
		}

		// ======================================================
		// Parse a line from the delimited input transaction file
		// ======================================================
		private eReturnCode ParseDelimitedData(string[] fields)
		{

			string store = null;
			string header = null;
			string product = null;
			string quantity = null;
			string relieveIntransitForAllStores = null;  // MID Track 5694 MA Enhancement Relieve IT by Header ID

			eReturnCode returnCode = eReturnCode.successful;

			if (fields.Length < 1 || fields[0].Length == 0)
			{
				header = null;
			}
			else
			{
				header = fields[0];
			}

			if (fields.Length < 2 || fields[1].Length == 0)
			{
				product = null;
			}
			else
			{
				product = fields[1];
			}

			if (fields.Length < 3 || fields[2].Length == 0)
			{
				store = null;
			}
			else
			{
				store = fields[2];
			}

			if (fields.Length < 4 || fields[3].Length == 0)
			{
				quantity = "0";
			}
			else
			{
				quantity = fields[3];
			}

			// begin MID Track 5694 MA Enhancement Relieve IT by Header ID
			if (fields.Length < 5 || fields[4].Length == 0)
			{
				relieveIntransitForAllStores = null;
			}
			else
			{
				relieveIntransitForAllStores = fields[4];
			}
			// end MID Track 5694 MA Enhancement Relieve IT by Header ID

			returnCode = EditOneTrans(header, product, relieveIntransitForAllStores, store, quantity);  // MID Track 5694 MA Enhancement Relieve IT by Header ID

			return returnCode;
		}

		// ======================================
		// Process the xml input transaction file
		// ======================================
		public eReturnCode ProcessVariableFile(string fileLocation, ref bool errorFound)
		{

			string store = null;
			string header = null;
			string message = null;
			string product = null;
			string quantity = null;
			string relieveIntransitForAllStores = null;  // MID Track 5694 MA Enhancement Relieve IT by Header ID
            bool   relieveStorePresent = false;          // MID Track 5694 MA Enhancement Relieve IT by Header ID
			XmlTextReader xmlReader = null;

			eReturnCode returnCode = eReturnCode.successful;

			try
			{
				xmlReader = new XmlTextReader(fileLocation);

				while(xmlReader.Read())
				{
					switch (xmlReader.NodeType)
					{
						case XmlNodeType.Element:
							switch (xmlReader.Name.ToUpper(CultureInfo.CurrentUICulture))
							{
								case "RELIEVEINTRANSITS":
									break;
								case "RELIEVEINTRANSIT":
									if (xmlReader.HasAttributes)
									{
										relieveIntransitForAllStores = null;  // MID Track 5694 MA Enhancement Relieve IT by Header ID
										relieveStorePresent = false; // MID Track 5694 MA Enhancement Relieve IT by Header ID
										while (xmlReader.MoveToNextAttribute())
										{
											switch (xmlReader.Name.ToUpper(CultureInfo.CurrentUICulture))
											{
												case "HEADERID":
												{
													header = xmlReader.Value;
													break;
												}
												case "PRODUCTID":
												{
													product = xmlReader.Value;
													break;
												}
												default:
												{
													message = _audit.GetText(eMIDTextCode.msg_InvalidAttributeForElement, false);
													message = message.Replace("{0}", "RELIEVEINTRANSIT");
													message = message.Replace("{1}", xmlReader.Name);
													_audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
													break;
												}
											}
										}
										xmlReader.MoveToElement();
									}
									else
									{
										message = _audit.GetText(eMIDTextCode.msg_VariableRequired, false);
										message = message.Replace("{0}", "RELIEVEINTRANSIT");
										message = message.Replace("{1}", xmlReader.Name);
										_audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
									}
									break;
								case "RELIEVESTORE":
									if (xmlReader.HasAttributes)
									{
										relieveStorePresent = true; // MID TRack 5694 MA Enhancement Relieve by Header ID
										while (xmlReader.MoveToNextAttribute())
										{
											switch (xmlReader.Name.ToUpper(CultureInfo.CurrentUICulture))
											{
												case "STOREID":
												{
													store = xmlReader.Value;
													break;
												}
												case "UNITS":
												{
													quantity = xmlReader.Value;
													break;
												}
													// begin MID Track 5694 MA Enhancement Relieve IT by Header ID
												case "RELIEVEALLSTORES":
												{
													relieveIntransitForAllStores = xmlReader.Value;
													break;
												}
													// end MID Track 5694 MA Enhancement Relieve IT by Header ID
												default:
												{
													message = _audit.GetText(eMIDTextCode.msg_InvalidAttributeForElement, false);
													message = message.Replace("{0}", "RELIEVESTORE");
													message = message.Replace("{1}", xmlReader.Name);
													_audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
													break;
												}
											}
										}
										++_transIn;
										returnCode = EditOneTrans(header, product, relieveIntransitForAllStores, store, quantity);  // MID Track 5694 MA Enhancement Relieve IT by Header ID
										if (returnCode != eReturnCode.successful)
										{
											++_transErr;
										}
										store = null;
										quantity = null;
										xmlReader.MoveToElement();
									}
									else
									{
										message = _audit.GetText(eMIDTextCode.msg_VariableRequired, false);
										message = message.Replace("{0}", "RELIEVESTORE");
										message = message.Replace("{1}", xmlReader.Name);
										_audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
									}
									break;
							}
							break;
						case XmlNodeType.EndElement:
							switch (xmlReader.Name.ToUpper(CultureInfo.CurrentUICulture))
							{
								case "RELIEVESTORE":
								{
									break;
								}
								case "RELIEVEINTRANSIT":
								{
									// begin MID Track 5694 MA Enhancement Relieve IT by Header ID
									if (!relieveStorePresent)
									{
										relieveStorePresent = true;
										++_transIn;
										returnCode = EditOneTrans(header, product, null, null, "0");  // MID Track 5694 MA Enhancement Relieve IT by Header ID
										if (returnCode != eReturnCode.successful)
										{
											++_transErr;
										}
									}
									// end MID Track 5694 MA Enhancement Relive IT by Header ID
									store = null;
									header = null;
									product = null;
									quantity = null;
									relieveIntransitForAllStores = null;
									break;
								}
								case "RELIEVEINTRANSITS":
								{
									// begin MID Track 5694 MA Enhancement Relieve IT by Header ID
									if (!relieveStorePresent)
									{
										++_transIn;
										returnCode = EditOneTrans(header, product, null, null, "0");  // MID Track 5694 MA Enhancement Relieve IT by Header ID
										if (returnCode != eReturnCode.successful)
										{
											++_transErr;
										}
									}
									// end MID Track 5694 MA Enhancement Relive IT by Header ID
									break;
								}
							}
							break;
					}
				}

				if (_dtRelieveIntransit.Rows.Count > 0)
				{
					returnCode = ProcessDataTable();
				}
				else
				{
					message = "No transactions found to process" + System.Environment.NewLine;
					_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
				}
			}

			catch (Exception Ex)
			{
				errorFound = true;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.Message, sourceModule);

				returnCode = eReturnCode.severe;
			}

			finally
			{
                //Begin Track #5100 - JSmith - Add counts to audit
                //message = "Transactions Read In: " + _transIn.ToString() + System.Environment.NewLine;
                //message += "Transactions Accepted: " + _transOut.ToString() + System.Environment.NewLine;
                //message += "Transactions In Error: " + _transErr.ToString() + System.Environment.NewLine;
                //_audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                _audit.RelieveIntransitAuditInfo_Add(_transIn, _transOut, _transErr);
                // End Track #5100

				if (xmlReader != null)
				{
					xmlReader.Close();
				}
			}

			return returnCode;
		}

		// ====================================================
		// Serialize and process the xml input transaction file
		// ====================================================
		public eReturnCode SerializeVariableFile(string fileLocation, ref bool errorFound)
		{

			string message = null;

			RelieveIntransits relieveIntransits = null;

			eReturnCode returnCode = eReturnCode.successful;

            // Begin Track #4229 - JSmith - API locks .XML input file
            TextReader txtReader = null;
            // End Track #4229

			try
			{
				XmlSerializer xmlSerial = new XmlSerializer(typeof(RelieveIntransits));

                // Begin Track #4229 - JSmith - API locks .XML input file
                //TextReader txtReader = new StreamReader(fileLocation);
                txtReader = new StreamReader(fileLocation);
                // End Track #4229

				relieveIntransits = (RelieveIntransits)xmlSerial.Deserialize(txtReader);

                // Begin Track #4229 - JSmith - API locks .XML input file
                //txtReader.Close();
                // End Track #4229
			}

			catch (Exception Ex)
			{
				errorFound = true;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.Message, sourceModule);

				returnCode = eReturnCode.severe;

				return returnCode;
			}
            // Begin Track #4229 - JSmith - API locks .XML input file
            finally
            {
                if (txtReader != null)
                    txtReader.Close();
            }
            // End Track #4229

			try
			{
				foreach(RelieveIntransitsRelieveIntransit ri in relieveIntransits.RelieveIntransit)
				{
					if (ri.RelieveStore != null)
					{
						// begin MID Track 5694 MA Enhancement Relieve IT by Header ID
						if (ri.RelieveStore.Length > 0)
						{
							// end MID Track 5694 MA Enhancement Relieve IT by Header ID
							foreach(RelieveIntransitsRelieveIntransitRelieveStore rirs in ri.RelieveStore)
							{
								++_transIn;
								returnCode = EditOneTrans(ri.HeaderID,
									ri.ProductID,
									rirs.RelieveAllStores.ToString(CultureInfo.CurrentUICulture), // MID Track 5694 MA Enhancement Relieve IT by Header ID
									rirs.StoreID,
									rirs.Units.ToString(CultureInfo.CurrentUICulture));
								if (returnCode != eReturnCode.successful)
								{
									++_transErr;
								}
							}
							// begin MID Track 5694 MA Enhancement Relieve IT by Header ID
						}
						else
						{
							++_transIn;
							returnCode = EditOneTrans (ri.HeaderID,
								ri.ProductID,
								null,
								null,
								"0");
							if (returnCode != eReturnCode.successful)
							{
								++_transErr;
							}
						}
						// end MID Track 5694 MA Enhancement Relieve IT by Header ID
					}

					if (_dtRelieveIntransit.Rows.Count > 0)
					{
						returnCode = ProcessDataTable();
						if (returnCode != eReturnCode.successful)
						{
							errorFound = true;
						}
					}
					else
					{
						message = "No transactions found to process" + System.Environment.NewLine;
						_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
					}
				}
			}

			catch (Exception Ex)
			{
				errorFound = true;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.Message, sourceModule);

				returnCode = eReturnCode.severe;
			}

			finally
			{
                //Begin Track #5100 - JSmith - Add counts to audit
                //message = "Transactions Read In: " + _transIn.ToString() + System.Environment.NewLine;
                //message += "Transactions Accepted: " + _transOut.ToString() + System.Environment.NewLine;
                //message += "Transactions In Error: " + _transErr.ToString() + System.Environment.NewLine;
                //_audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                _audit.RelieveIntransitAuditInfo_Add(_transIn, _transOut, _transErr);
                // End Track #5100
			}

			return returnCode;
		}

		// =======================================
		// Edit the input data for one transaction
		// =======================================
		private Hashtable headerErrorHash = new Hashtable(); // MID Track 5694 MA Enhancement Relieve IT by Header ID
		private Hashtable headerRelieveHash = new Hashtable(); // MID Track 5694 MA Enhancement Relieve IT by Header ID
		private eReturnCode EditOneTrans(string aHeaderID, string aProductID, string aRelieveIntransitForAllStores, string aStoreID, string aQuantity) // MID Track 5694 MA Enhancement Relieve IT by Header ID
		{

			bool editError = false;

			string editMsg = null;
			string message = null;
			string storeID = null;
			string headerID = null;
			string productID = null;
			bool relieveIntransitForAllStores = _relieveIntransitForAllStores; // MID Track 5694 MA Enhancement Relieve IT by Header ID

			HierarchyNodeProfile hnp = null;

			int quantity = 0;
			int nodeRID = Include.NoRID;
			int storeRID = Include.UndefinedStoreRID;

			EditMsgs em = new EditMsgs();

			eReturnCode returnCode = eReturnCode.successful;

			editMsg =  "Header: [" + aHeaderID + "], Product: [" + aProductID + "], Store: [" + aStoreID + "], Units: [" + aQuantity + "], RelieveAllStores: [" + aRelieveIntransitForAllStores + "]"; // MID Track 5694 MA Enhancement Relieve IT by Header ID

			// ==============
			// Edit header id
			// ==============
			if (aHeaderID == "" || aHeaderID == null)
			{
				headerID = null;
				if (_headerIDsPresent)
				{
					editError = true;
					message = _audit.GetText(eMIDTextCode.msg_HeaderIDRequired, false) + " - " + editMsg;
					em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);

				}
			}
			else
			{
				if (!_headerIDsPresent)
				{
					editError = true;
					message = _audit.GetText(eMIDTextCode.msg_HeaderIDNotAllowed, false) + " - " + editMsg;
					em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
				}
				else
				{
					headerID = aHeaderID;
				}
			}

			// ===============
			// Edit product id
			// ===============
			if (aProductID == "" | aProductID == null)
			{
				editError = true;
				nodeRID = Include.NoRID;
				message = _audit.GetText(eMIDTextCode.msg_ProductRequired, false) + " - " + editMsg;
				em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
			}
			else
			{
				if (_nodeIDs.ContainsKey(aProductID))
				{
					nodeRID = (int)_nodeIDs[aProductID];
				}
				else
				{
					productID = aProductID;
					nodeRID = Include.NoRID;
					hnp = _hm.NodeLookup(ref em, productID, false);
					if (hnp.Key != Include.NoRID)
					{
						nodeRID = hnp.Key;
						_nodeIDs.Add(aProductID, nodeRID);
						_nodeRIDs.Add(nodeRID, aProductID);
					}
					else
					{
						editError = true;
						nodeRID = Include.NoRID;
						message = _audit.GetText(eMIDTextCode.msg_ProductNotFound, false) + " - " + editMsg;
						em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
					}
				}
			}

			// begin MID TRack 5694 MA Relieve Intransit Enhancement: ability to relieve by header id
			if (aRelieveIntransitForAllStores == "" || aRelieveIntransitForAllStores == null)
			{
				try 
				{
					if (headerID != null && headerID != "")
					{
						relieveIntransitForAllStores = (bool)headerRelieveHash[headerID];
					}
					else
					{
						relieveIntransitForAllStores = _relieveIntransitForAllStores;
					}
				}
				catch (NullReferenceException)
				{
					relieveIntransitForAllStores = _relieveIntransitForAllStores;
				}
			}
			else
			{
				if (headerID == null || headerID == "")
				{
					editError = true;
					message = _audit.GetText(eMIDTextCode.msg_RelieveAllStoresInvalidWhenNoHeaders, false) + " - " + editMsg;
					em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
				}
				else
				{
					try
					{

						relieveIntransitForAllStores = Convert.ToBoolean(aRelieveIntransitForAllStores,CultureInfo.CurrentUICulture);
						try
						{
							headerRelieveHash.Add(headerID, relieveIntransitForAllStores);
						}
						catch (ArgumentException)
						{
							if ((bool)headerRelieveHash[headerID] != relieveIntransitForAllStores)
							{
								editError = true;
								message = _audit.GetText(eMIDTextCode.msg_RelieveAllStoresDuplicatesNotAllowed, false) + " - " + editMsg;
								em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
								try
								{
									headerErrorHash.Add(headerID, headerID);
								}
								catch (ArgumentException)
								{
                                    // drop duplicate errors
								}
							}
						}
					}
					catch (FormatException e)
					{
						editError = true;
						message = _audit.GetText(eMIDTextCode.msg_RelieveAllStoresMustBeBoolean, false) + " - " + editMsg;
						em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
					}
				}
			}
			
			if (!relieveIntransitForAllStores)
			{
				// end MID Track 5694 MA Relieve Intransit Enhancement: ability to relieve by header id
				// =============
				// Edit store id
				// =============
				if (aStoreID == "" | aStoreID == null)
				{
					// begin MID Track 6250 Relieve Intransit errors
					if (this._headersAllocatedToReserve == null)
					{
						_headersAllocatedToReserve = this.HeaderData.GetHeadersAllocatedToReserve(); 
						DataColumn[] PrimaryKeyColumns = new DataColumn[1];
						PrimaryKeyColumns[0] = _headersAllocatedToReserve.Columns["HDR_ID"];
						_headersAllocatedToReserve.PrimaryKey = PrimaryKeyColumns;
					}
					if (this._headersAllocatedToReserve.Rows.Find(headerID) != null)
					{
						relieveIntransitForAllStores = true;
						try
						{
							headerRelieveHash.Add(headerID, relieveIntransitForAllStores);
						}
						catch (ArgumentException)
						{
                            // drop duplicates; this header HAS all units allocated to reserve and should be relieved 
							//      because a relieve transaction has been received for it (there are no other stores
							//      with an allocation.
						}
					}
					else
					{
						editError = true;
						message = _audit.GetText(eMIDTextCode.msg_StoreRequired, false) + " - " + editMsg;
						em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
					}
					// end MID Track 6250 Relieve Intransit errors
				}
				else
				{
					if (_storeIDs.ContainsKey(aStoreID))
					{
						storeRID = (int)_storeIDs[aStoreID];
					}
					else
					{
						storeID = aStoreID;
						storeRID = Include.UndefinedStoreRID;
                        storeRID = StoreMgmt.StoreProfile_GetStoreRidFromId(storeID); //_sss.GetStoreRID(storeID);
						if (storeRID != Include.UndefinedStoreRID)
						{
							// MID Track 4214 Identify stores in messages
							//_storeIDs.Add(aStoreID, storeRID);
                            if (StoreMgmt.StoreProfile_Get(storeRID).ActiveInd == true) //if (_sss.GetStoreProfile(storeRID).ActiveInd == true)
							{
								_storeIDs.Add(aStoreID, storeRID);
							}
							else
							{
								editError = true;
								message = string.Format(_audit.GetText(eMIDTextCode.msg_StoreNotActive, false), storeID) + " - " + editMsg;
								em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
							}
							// MID Track 4214 Identify stores in messages
						}
						else
						{
							editError = true;
							message = _audit.GetText(eMIDTextCode.msg_StoreNotFound, false) + " - " + editMsg;
							em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
						}
					}
				}
				// =============
				// Edit quantity
				// =============
				if (aQuantity == "" || aQuantity == null)
				{
					quantity = 0;
				}
				else
				{
					try
					{
						quantity = Convert.ToInt32(aQuantity);
					}
					catch
					{
						quantity = 0;
					}
					if (quantity < 0)
					{
						editError = true;
						message = _audit.GetText(eMIDTextCode.msg_InTransitCannotBeNeg, false) + " - " + editMsg;
						em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
					}
				}
			} // MID Track 5694 MA Enhancement Relieve IT by Header ID

			if (editError)
			{
				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];
					_audit.Add_Msg(emm.messageLevel, emm.msg, emm.module);
				}
				returnCode = eReturnCode.editErrors;
			}
			else
			{
				// begin MID Track 5694 MA Enhancement Relieve IT by Header ID
				if (relieveIntransitForAllStores)
				{
					if (headerID != null)
					{
                        AllocationHeaderProfile ahp = _SAB.HeaderServerSession.GetHeaderData(headerID, false, false, true);	// TT#1133-MD - stodd - Relieve IT not working with group allocation - 
						++_transOut;
						// ====================================================================================
						// Create _drRelieveIntransit DataRow object and add to the _dtRelieveIntransit DataTable
						// ====================================================================================
						_drRelieveIntransit = _dtRelieveIntransit.NewRow();
						_drRelieveIntransit["HEADER_ID"] = headerID;
						_drRelieveIntransit["NODE_RID"] = nodeRID;
						_drRelieveIntransit["STORE_RID"] = storeRID;
						_drRelieveIntransit["STORE_QTY"] = quantity;
                        _drRelieveIntransit["ASRT_RID"] = ahp.AsrtRID;	// TT#1133-MD - stodd - Relieve IT not working with group allocation - 
						_dtRelieveIntransit.Rows.Add(_drRelieveIntransit);
					}
				}
				else
				{
					// end MID Track 5694 MA Enhancement Relieve IT by Header ID
					if (nodeRID != Include.NoRID && storeRID != Include.UndefinedStoreRID && quantity != 0)
					{
						// Begin TT#1133-MD - stodd - Relieve IT not working with group allocation - 
                        int asrtRid = Include.NoRID;
                        if (headerID != null)
                        {
                            AllocationHeaderProfile ahp = _SAB.HeaderServerSession.GetHeaderData(headerID, false, false, true);
                            asrtRid = ahp.AsrtRID;
                        }
						// End TT#1133-MD - stodd - Relieve IT not working with group allocation - 
						++_transOut;
						// ====================================================================================
						// Create _drRelieveIntransit DataRow object and add to the _dtRelieveIntransit DataTable
						// ====================================================================================
						_drRelieveIntransit = _dtRelieveIntransit.NewRow();
						_drRelieveIntransit["HEADER_ID"] = headerID;
						_drRelieveIntransit["NODE_RID"] = nodeRID;
						_drRelieveIntransit["STORE_RID"] = storeRID;
						_drRelieveIntransit["STORE_QTY"] = quantity;
                        _drRelieveIntransit["ASRT_RID"] = asrtRid;	// TT#1133-MD - stodd - Relieve IT not working with group allocation - 
						_dtRelieveIntransit.Rows.Add(_drRelieveIntransit);
					}
				}  // MID Track 5694 MA Enhancement Relieve IT by Header ID
			}
			return returnCode;
		}

		// ============================
		// Prepare to relieve intransit
		// ============================
		private eReturnCode ProcessDataTable()
		{

			Hashtable strsQtys = null;

			string rowHeaderID = null;
			string currHeaderID = null;

			int rowQuantity = 0;
			int totalQuantity = 0;
			int rowNodeRID = Include.NoRID;
			int currNodeRID = Include.NoRID;
			int rowStoreRID = Include.UndefinedStoreRID;

			eReturnCode returnCode = eReturnCode.successful;

			try
			{
				strsQtys = new Hashtable();
				strsQtys.Clear();

				if (!_headerIDsPresent)
				{
					// =====================================
					// Retrieve all rows ordered by NODE_RID
					// =====================================
					DataRow[] _drRelieveIntransitRows = _dtRelieveIntransit.Select(null, "NODE_RID ASC", DataViewRowState.CurrentRows);
					if (_drRelieveIntransitRows.Length > 0 )
					{
						foreach (DataRow _drRelieveIntransitRow in _drRelieveIntransitRows)
						{
							rowNodeRID = Convert.ToInt32(_drRelieveIntransitRow["NODE_RID"], CultureInfo.CurrentUICulture);
							if (currNodeRID == Include.NoRID)
							{
								currNodeRID = rowNodeRID;
							}
							else
							{
								if (rowNodeRID != currNodeRID)
								{
									returnCode = RelieveNodeRID(currNodeRID, totalQuantity, strsQtys);
									strsQtys.Clear();
									totalQuantity = 0;
									currNodeRID = rowNodeRID;
								}
							}
							rowStoreRID = Convert.ToInt32(_drRelieveIntransitRow["STORE_RID"], CultureInfo.CurrentUICulture);
							rowQuantity = Convert.ToInt32(_drRelieveIntransitRow["STORE_QTY"], CultureInfo.CurrentUICulture);
							totalQuantity += rowQuantity;
							if (!strsQtys.ContainsKey(rowStoreRID))
							{
								strsQtys[rowStoreRID] = rowQuantity;
							}
							else
							{
								strsQtys[rowStoreRID] = (int)strsQtys[rowStoreRID] + rowQuantity;
							}
						}
						returnCode = RelieveNodeRID(currNodeRID, totalQuantity, strsQtys);
					}
				}
				else
				{
					bool relieveIntransitForAllStores = _relieveIntransitForAllStores;
					// ======================================
					// Retrieve all rows ordered by HEADER_ID
					// ======================================
					DataRow[] _drRelieveIntransitRows = _dtRelieveIntransit.Select(null, "ASRT_RID ASC, HEADER_ID ASC", DataViewRowState.CurrentRows);	// TT#1133-MD - stodd - Relieve IT not working with group allocation - 
					if (_drRelieveIntransitRows.Length > 0 )
					{
						foreach (DataRow _drRelieveIntransitRow in _drRelieveIntransitRows)
						{
							rowHeaderID = _drRelieveIntransitRow["HEADER_ID"].ToString();
							
							if (currHeaderID == null)
							{
								currHeaderID = rowHeaderID;
								// begin MID Track 5694 MA Enhancement Relieve IT by Header ID
								try
								{
									relieveIntransitForAllStores = (bool)headerRelieveHash[rowHeaderID];
									string message = string.Format(MIDText.GetText(eMIDTextCode.msg_RelieveAllStoresOverrideSetting), currHeaderID,relieveIntransitForAllStores.ToString());
									_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule,true);
								}
								catch (NullReferenceException)
								{
									relieveIntransitForAllStores = _relieveIntransitForAllStores;
								}
								// end MID Track 5694 MA Enhancement Relieve IT by Header ID
							}
							else
							{
								if (rowHeaderID != currHeaderID)
								{
									// Be sure Header isn't a multi -- stodd
									if (!HeaderData.IsMultiHeader(currHeaderID))
									{
										// begin MID Track 5694 MA Relieve IT Enhancement: Relieve by Header ID
										try
										{
											if (headerErrorHash.Contains(currHeaderID))
											{
												returnCode = eReturnCode.warning;
												string message = "Allocation Header " + currHeaderID + " intransit NOT relieved" + System.Environment.NewLine;
												message += "due to conflicting settings for [RelieveAllStores]";
												_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
											}
											else
											{
												// end MID Track 5694 MA Relieve IT Enhancement: Relieve by Header ID
												if (relieveIntransitForAllStores)
												{
													returnCode = RelieveHeaderID(currHeaderID);
												}
												else
												{
													// end MID Track 5694 MA Relieve IT Enhancement: Relieve by Header ID
													returnCode = RelieveHeaderID(currHeaderID, totalQuantity, strsQtys);
												}   // Track 5694 Relieve IT by Header ID
											}
											// begin MID Track 5694 MA Relieve IT Enhancement: Relieve by Header ID
										}
										catch
										{
											throw;
										}
										// end MID Track 5694 MA Relieve IT Enhancement: Relieve by Header ID
									}
									else
									{
										returnCode = eReturnCode.warning;
										string message = "Allocation Header " + currHeaderID + " intransit NOT relieved" + System.Environment.NewLine;
										message += "Header is a multi-header. Intransit should be relieved from individual headers.";
										_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
									}
									strsQtys.Clear();
									currHeaderID = rowHeaderID;
									// begin MID Track 5694 MA Enhancement Relieve IT by Header ID
									try
									{
										relieveIntransitForAllStores = (bool)headerRelieveHash[rowHeaderID];
										string message = string.Format(MIDText.GetText(eMIDTextCode.msg_RelieveAllStoresOverrideSetting), currHeaderID,relieveIntransitForAllStores.ToString());
										_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule,true);
									}
									catch (NullReferenceException)
									{
										relieveIntransitForAllStores = _relieveIntransitForAllStores;
									}
									// end MID Track 5694 MA Enhancement Relieve IT by Header ID
								}
							}
							// begin MID Track 5694 MA Enhancement Relieve IT by Header ID
							if (!relieveIntransitForAllStores)
							{
								// end MID Track 5694 MA Enhancement Relieve IT by Header ID
								rowStoreRID = Convert.ToInt32(_drRelieveIntransitRow["STORE_RID"], CultureInfo.CurrentUICulture);
								rowQuantity = Convert.ToInt32(_drRelieveIntransitRow["STORE_QTY"], CultureInfo.CurrentUICulture);
								totalQuantity += rowQuantity;
								if (!strsQtys.ContainsKey(rowStoreRID))
								{
									strsQtys[rowStoreRID] = rowQuantity;
								}
								else
								{
									strsQtys[rowStoreRID] = (int)strsQtys[rowStoreRID] + rowQuantity;
								}
							} // MID Track 5694 MA Enhancement Relieve IT by Header ID
						}
						// Check to be sure final record isn't a multi header 
						if (!HeaderData.IsMultiHeader(currHeaderID))
						{
							// begin MID Track 5694 MA Relieve IT Enhancement: Relieve by Header ID
							try
							{
								if (headerErrorHash.Contains(currHeaderID))
								{
									returnCode = eReturnCode.warning;
									string message = "Allocation Header " + currHeaderID + " intransit NOT relieved" + System.Environment.NewLine;
									message += "due to conflicting settings for [RelieveAllStores]";
									_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
								}
								else
								{
									// end MID Track 5694 MA Relieve IT Enhancement: Relieve by Header ID
									if (relieveIntransitForAllStores)
									{
										returnCode = RelieveHeaderID(currHeaderID);
									}
									else
									{
										// end MID Track 5694 MA Relieve IT Enhancement: Relieve by Header ID
										returnCode = RelieveHeaderID(currHeaderID, totalQuantity, strsQtys);
									}   // Track 5694 Relieve IT by Header ID
								}
								// begin MID Track 5694 MA Relieve IT Enhancement: Relieve by Header ID
							}
							catch
							{
								throw;
							}
							// end MID Track 5694 MA Relieve IT Enhancement: Relieve by Header ID
						}
						else
						{
							returnCode = eReturnCode.warning;
							string message = "Allocation Header " + currHeaderID + " intransit NOT relieved" + System.Environment.NewLine;
							message += "Header is a multi-header. Intransit should be relieved from individual headers.";
							_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
						}
					}
				}
			}

			catch (Exception Ex)
			{
				_audit.Add_Msg(eMIDMessageLevel.Warning, Ex.Message, sourceModule);

				returnCode = eReturnCode.warning;
			}

			return returnCode;
		}

		// ===========================================
		// Relieve intransit for the specified Product
		// ===========================================
		private eReturnCode RelieveNodeRID(int nodeRID, int totalQty, Hashtable strsQtys)
		{

			uint alocStatus = 0;
			uint shipStatus = 0;
			string message = null;
			string headerID = null;

			DataTable dtHeaderIDs = null;

			eReturnCode returnCode = eReturnCode.successful;

			try
			{
				dtHeaderIDs = HeaderData.GetHeaders(nodeRID);

				if (dtHeaderIDs.Rows.Count > 0)
				{
					foreach (DataRow drHeaderIDs in dtHeaderIDs.Rows)
					{
						alocStatus = Convert.ToUInt32(drHeaderIDs["ALLOCATION_STATUS_FLAGS"], CultureInfo.CurrentUICulture);
						shipStatus = Convert.ToUInt32(drHeaderIDs["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture);
						if (MIDFlag.GetFlagValue(alocStatus, (int)eAllocationStatusFlag.Released) &&
					      !(MIDFlag.GetFlagValue(shipStatus, (int)eShippingStatusFlag.ShippingComplete) ||
							MIDFlag.GetFlagValue(shipStatus, (int)eShippingStatusFlag.ShippingOnHold)))
						{
							headerID = drHeaderIDs["HDR_ID"].ToString();
							returnCode = RelieveHeaderID(headerID, totalQty, strsQtys);
						}
					}
				}
				else
				{
					message = "No Allocation Headers found to relieve for Product ";
					message += (string)_nodeRIDs[nodeRID] + System.Environment.NewLine;
					_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
				}
			}

			catch (Exception Ex)
			{
				_audit.Add_Msg(eMIDMessageLevel.Warning, Ex.Message, sourceModule);

				returnCode = eReturnCode.warning;
			}

			return returnCode;
		}

		// ==========================================
		// Relieve intransit for the specified Header
		// ==========================================
		// begin MID TRack 5694 Relieve Intransit Enhancement: relieve by header id
		
        private eReturnCode RelieveHeaderID(string headerID)
		{
			string message;
            string enqMessage; // TT#1185 - Verify ENQ before Update
			eReturnCode returnCode = eReturnCode.successful;
			try
			{
				if (HeaderData.HeaderExists(headerID))
				{
                    //HeaderEnqueue hdrENQ; // TT#1185 - Verify ENQ before Update 
                    _apl = (AllocationProfileList)_aTrans.GetMasterProfileList(eProfileType.Allocation);
                    _apl.Clear();
                    AllocationProfileList ampl = (AllocationProfileList)_aTrans.GetMasterProfileList(eProfileType.AssortmentMember);	// TT#1133-MD - stodd - Relieve IT not working with group allocation - 
                    int[] hdrRID = new int[1];
                    hdrRID[0] = HeaderData.GetHeaderRID(headerID);
                    // begin TT#1185 Verify ENQ before Update
                    List<int> hdrRidList = new List<int>();
                    hdrRidList.Add(hdrRID[0]);                     
                    try
                    {
                        if (_aTrans.EnqueueHeaders(_aTrans.GetHeadersToEnqueue(hdrRidList), out enqMessage))
                        {
							// Begin TT#1133-MD - stodd - Relieve IT not working with group allocation - 
                            // Load header into transaction
                            returnCode = loadHeader(hdrRID, headerID, ampl);

                            if (returnCode == eReturnCode.successful)
                            {
                                if (((AllocationProfile)_apl[0]).RelieveHeaderInTransit())
                                {
                                    message = "Allocation Header " + headerID + " intransit relieved" + System.Environment.NewLine;
                                    _audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                                }
                                else
                                {
                                    returnCode = eReturnCode.warning;
                                    message = "Allocation Header " + headerID + " intransit NOT relieved" + System.Environment.NewLine;
                                    _audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                                }
                            }
							// End TT#1133-MD - stodd - Relieve IT not working with group allocation - 
                            // begin TT#1185 Verify ENQ Before Update
                        }
                        else
                        {
                            returnCode = eReturnCode.warning;
                            message = "Allocation Header " + headerID + " intransit NOT relieved" + System.Environment.NewLine;
                            _audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                            _audit.Add_Msg(
                                    eMIDMessageLevel.Information,
                                    eMIDTextCode.msg_al_HeaderEnqFailed,
                                    MIDText.GetTextOnly(eMIDTextCode.msg_al_HeaderEnqFailed),
                                    sourceModule);
                            _audit.Add_Msg(eMIDMessageLevel.Warning, enqMessage, sourceModule);
                        }
                    }
                    //catch (HeaderConflictException)
                    //{
                    //    returnCode = eReturnCode.warning;
                    //    SecurityAdmin sa = new SecurityAdmin();
                    //    message = "Allocation Header " + headerID + " intransit NOT relieved" + System.Environment.NewLine;
                    //    _audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                    //    foreach (HeaderConflict hc in hdrENQ.HeaderConflictList)
                    //    {
                    //        message = "Allocation Header [" + HeaderData.GetHeaderID(hc.HeaderRID) + "] is locked by user [" + sa.GetUser(hc.UserRID) + "] on thread [" + hc.ThreadID + "]";
                    //        _audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                    //    }
                    //}
                    finally
                    {
                        //hdrENQ.DequeueHeaders();
                        _aTrans.DequeueHeaders();
                    }
                    // end TT#1185 Verify ENQ Before Update
				}
				else
				{
					returnCode = eReturnCode.warning;
					message = "Allocation Header " + headerID + " does NOT exist" + System.Environment.NewLine;
					_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
				}
			}
			catch (Exception Ex)
			{
				_audit.Add_Msg(eMIDMessageLevel.Warning, Ex.Message, sourceModule);

				returnCode = eReturnCode.warning;
			}
			return returnCode;
		}
		//end MID TRack 5694 Relieve Intransit Enhancement: relieve by header id

		private eReturnCode RelieveHeaderID(string headerID, int totalQty, Hashtable strsQtys)
		{
			// Begin MID Multi header stodd
			int origTotalQty = totalQty;
			// copy hash table
			Hashtable origStrsQtys = new Hashtable();
			IDictionaryEnumerator myEnumerator = strsQtys.GetEnumerator();
			while ( myEnumerator.MoveNext() )
			{
				origStrsQtys.Add((int)myEnumerator.Key, (int)myEnumerator.Value);
			}
			// Begin MID Multi header stodd

			int i = 0;
			int toShip = 0;
			int alocQty = 0;
			int shipQty = 0;
			int tmpStore = 0;


			string message = null;
            string enqMessage;	// TT#1133-MD - stodd - Relieve IT not working with group allocation - 
			Hashtable qtysToShip = null;

			ShipStore[] shipStore = null;

			GeneralComponent genComp = null;

			eReturnCode returnCode = eReturnCode.successful;

			try
			{
				qtysToShip = new Hashtable();
				qtysToShip.Clear();

				genComp = new GeneralComponent(eGeneralComponentType.Total);

				if (HeaderData.HeaderExists(headerID))
				{
					_apl = (AllocationProfileList)_aTrans.GetMasterProfileList(eProfileType.Allocation);
					// release memory for previous AllocationProfile objects and transaction cache 
                    for (int j = 0; j < _apl.Count; j++)
                    {
                        AllocationProfile ap = (AllocationProfile)_apl[j];
                        ap.InitializeDataStorage();
                        ap.Dispose();
                        ap = null;
                    }
					_aTrans.InitializeDataStorage();
					_apl.Clear();
					// force gargage collection
                	System.GC.Collect();
					// Begin TT#1133-MD - stodd - Relieve IT not working with group allocation - 
                    ProfileList ampl = (ProfileList)_aTrans.GetMasterProfileList(eProfileType.AssortmentMember);
					int[] hdrRID = new int[1];
					hdrRID[0] = HeaderData.GetHeaderRID(headerID);
                     List<int> hdrRidList = new List<int>();
                    hdrRidList.Add(hdrRID[0]);
                    try
                    {
                        if (_aTrans.EnqueueHeaders(_aTrans.GetHeadersToEnqueue(hdrRidList), out enqMessage))
                        {
                            // Load header into transaction
                            returnCode = loadHeader(hdrRID, headerID, ampl);

                            //_apl.LoadHeaders(_aTrans, null, hdrRID, _SAB.ApplicationServerSession);  // TT#488 - MD - Jellis - Group Allocation
					// End TT#1133-MD - stodd - Relieve IT not working with group allocation - 
                            // NOTE:  Assumption is that header is not an Assortment; Load will fail if it is. (Expectation is that a member of an assortment is 'ok')

                            foreach (DictionaryEntry strQty in strsQtys)
                            {
                                alocQty = ((AllocationProfile)_apl[0]).GetStoreQtyAllocated(genComp, (int)strQty.Key);
                                shipQty = ((AllocationProfile)_apl[0]).GetStoreQtyShipped(genComp, (int)strQty.Key);
                                if (alocQty <= shipQty)
                                {
                                    continue;
                                }
                                else
                                {
                                    toShip = alocQty - shipQty;
                                    if (toShip <= (int)strQty.Value)
                                    {
                                        qtysToShip.Add((int)strQty.Key, toShip);
                                    }
                                    else
                                    {
                                        qtysToShip.Add((int)strQty.Key, (int)strQty.Value);
                                    }
                                }
                            }
                            if (qtysToShip.Count > 0)
                            {
                                shipStore = new ShipStore[qtysToShip.Count];
                                foreach (DictionaryEntry qtyToShip in qtysToShip)
                                {
                                    shipStore[i++] = new ShipStore((int)qtyToShip.Key, (int)qtyToShip.Value, false);
                                }
                                if (((AllocationProfile)_apl[0]).RelieveInTransit(genComp, shipStore))
                                {
                                    message = "Allocation Header " + headerID + " intransit relieved" + System.Environment.NewLine;
                                    _audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                                    foreach (DictionaryEntry qtyToShip in qtysToShip)
                                    {
                                        tmpStore = (int)qtyToShip.Key;
                                        totalQty -= (int)qtyToShip.Value;
                                        strsQtys[tmpStore] = (int)strsQtys[tmpStore] - (int)qtyToShip.Value;
                                    }
                                }
                                else
                                {
                                    returnCode = eReturnCode.warning;
                                    message = "Allocation Header " + headerID + " intransit NOT relieved" + System.Environment.NewLine;
                                    _audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                                }
                            }

                            // begin MID Track 6250 Multi Intransit error
                            //       Relieve of Multi Intransit is wrong; should just be setting Shipping status (should be included as child is relieved)
                            //       following commented code relieves "whole" store even though only one header of multi was relieved
                            // // Begin MID Multi-header stodd
                            // // We are recursivly calling this same routine to handle the Multi header for the child header that was just processed.
                            //if (((AllocationProfile)_apl[0]).GetHeaderAllocationStatus(false) == eHeaderAllocationStatus.InUseByMultiHeader)
                            //{
                            //	int multiHeaderRid = ((AllocationProfile)_apl[0]).HeaderGroupRID;
                            //	string multiHeaderId = HeaderData.GetHeaderID(multiHeaderRid);
                            //	returnCode = RelieveHeaderID(multiHeaderId, origTotalQty, origStrsQtys);
                            //}
                            // // End MID Multi-Header
                            // end MID Track 6250 Multi Intransit error
                        }
						// Begin TT#1133-MD - stodd - Relieve IT not working with group allocation - 
                        else
                        {
                            returnCode = eReturnCode.warning;
                            message = "Allocation Header " + headerID + " intransit NOT relieved" + System.Environment.NewLine;
                            _audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                            _audit.Add_Msg(
                                    eMIDMessageLevel.Information,
                                    eMIDTextCode.msg_al_HeaderEnqFailed,
                                    MIDText.GetTextOnly(eMIDTextCode.msg_al_HeaderEnqFailed),
                                    sourceModule);
                            _audit.Add_Msg(eMIDMessageLevel.Warning, enqMessage, sourceModule);
                        }
                    }
                    finally
                    {
                        _aTrans.DequeueHeaders();
                    }
					// End TT#1133-MD - stodd - Relieve IT not working with group allocation - 
				}
				else
				{
					returnCode = eReturnCode.warning;
					message = "Allocation Header " + headerID + " does NOT exist" + System.Environment.NewLine;
					_audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
				}
			}
			catch (Exception Ex)
			{
				_audit.Add_Msg(eMIDMessageLevel.Warning, Ex.Message, sourceModule);

				returnCode = eReturnCode.warning;
			}

			return returnCode;
		}

		// Begin TT#1133-MD - stodd - Relieve IT not working with group allocation - 
        /// <summary>
        /// Loads the header into the transactions. If header belongs to a Group Allocation,
        /// Also loads the Assortment Member list in the transaction.
        /// </summary>
        /// <param name="hdrRID"></param>
        /// <param name="headerID"></param>
        /// <param name="asrtMemberProfileList"></param>
        /// <returns></returns>
        private eReturnCode loadHeader(int[] hdrRID, string headerID, ProfileList asrtMemberProfileList)
        {
            eReturnCode returnCode = eReturnCode.successful;
            string message = string.Empty;
            try
            {
                AllocationHeaderProfile ahp = _SAB.HeaderServerSession.GetHeaderData(hdrRID[0], false, false, true);
                AllocationHeaderProfile asrthp = null;

                if (ahp.AsrtRID != Include.NoRID)
                {
                    asrthp = _SAB.HeaderServerSession.GetHeaderData(ahp.AsrtRID, false, false, true);
                }

                // If this is a header that belongs to a group allocation, load the header information this way
                if (asrthp != null && asrthp.AsrtType == (int)eAssortmentType.GroupAllocation)
                {
                    if (ahp.AsrtRID == _currAssortmentHeaderRid)
                    {
                        int ownedByUserRID, ownedByThreadID;
                        long ownedByTranID;
                        // Each header processed is enqueued. If the header belongs to a group allocation that
                        // we already have the assortment members for, then the enqueue stamp needs to be updated in the
                        // assortment member list to match the header's enqueue.
                        foreach (AllocationProfile aap in asrtMemberProfileList.ArrayList)
                        {

                            eEnqueueStatus headerEnqueueStatus = aap.GetHeaderEnqueueStatus(aap.Key, out ownedByUserRID, out ownedByThreadID, out ownedByTranID);
                        }

                        AllocationProfile ap = (AllocationProfile)asrtMemberProfileList.FindKey(ahp.Key);
                        if (ap == null)
                        {
                            returnCode = eReturnCode.severe;
                            message = "Allocation Header " + headerID + " belongs to group allocation, but could not find the header within the group allocation members." + System.Environment.NewLine;
                            _audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
                        }
                        else
                        {
                            _apl.Clear();
                            _apl.Add(ap);
                        }

                        _aTrans.SetMasterProfileList(_apl);
                    }
                    else
                    {
                        // Begin TT#1316-MD - stodd - Severe Relieve in Transit errors for Group Allocation
                        //asrtMemberProfileList.Clear();
                        _aTrans.RemoveProfileListGroup(eProfileType.AssortmentMember);
                        _aTrans.LoadHeadersInTransaction(ahp.Key);
                        _apl = (AllocationProfileList)_aTrans.GetMasterProfileList(eProfileType.Allocation);
                        _currAssortmentHeaderRid = ahp.AsrtRID;
                        // End TT#1316-MD - stodd - Severe Relieve in Transit errors for Group Allocation
                    }
                }
                else
                {
                    //ap = new AllocationProfile(aTrans, hdrTran.HeaderId, Include.NoRID, _SAB.ApplicationServerSession);
                    //apl.Add(ap);
                    //aTrans.SetMasterProfileList(apl);
                    // end TT#1185 Verify ENQ before Update
                    _apl.LoadHeaders(_aTrans, null, hdrRID, _SAB.ApplicationServerSession);  // TT#488 - MD - Jellis - Group Allocation
                    // NOTE:  Assumption is that header is not an Assortment; Load will fail if it is. (Expectation is that a member of an assortment is 'ok')
                }

                return returnCode;
            }
            catch
            {
                throw;
            }
        }
		// End TT#1133-MD - stodd - Relieve IT not working with group allocation - 

	}
}
