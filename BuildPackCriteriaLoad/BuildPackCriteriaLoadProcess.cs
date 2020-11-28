using System;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;



namespace MIDRetail.BuildPackCriteriaLoad
{
	/// <summary>
	/// Summary description for BuildPackCriteriaLoadProcess.
	/// </summary>
	public class BuildPackCriteriaLoadProcess
	{
		private string sourceModule = "BuildPackCriteriaLoadProcess.cs";
		SessionAddressBlock _SAB;
		protected BuildPackCriteriaData _bpcd = null;
		protected Audit _audit = null;
		protected int _recordsRead = 0;
		protected int _recordsWithErrors = 0;
		protected int _recordsAddedUpdated = 0;

		public BuildPackCriteriaLoadProcess(SessionAddressBlock SAB, ref bool errorFound)
		{
			try
			{
				_SAB = SAB;
				_audit = _SAB.ClientServerSession.Audit;
				_bpcd = new BuildPackCriteriaData();
			}
			catch (Exception ex)
			{
				errorFound = true;
				_audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
				_audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
				throw;
			}
		}

		public eReturnCode ProcessVariableFile(string fileLocation, char[] delimiter, ref bool errorFound)
		{
			StreamReader reader = null;
			string line = null;
			string message = null;
			string msgDetails = null;
			eReturnCode returnCode = eReturnCode.successful;

			try
			{
				reader = new StreamReader(fileLocation);  //opens the file

				_bpcd.OpenUpdateConnection();

				try
				{
					while ((line = reader.ReadLine()) != null)
					{
						string[] fields = MIDstringTools.Split(line, delimiter[0], true);
						if (fields.Length == 1 && (fields[0] == null || fields[0] == ""))  // skip blank line 
						{
							continue;
						}
						++_recordsRead;

						if (fields.Length == 6)
						{
							returnCode = SeparateDelimitedData(line, fields);
							_bpcd.CommitData();

							if (returnCode != eReturnCode.successful)
							{
								++_recordsWithErrors;
							}
							else
							{
								++_recordsAddedUpdated;
							}
						}
						else
						{
							++_recordsWithErrors;
							msgDetails = "Input record not defined in correct format";
							_audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_MismatchedDelimiter, msgDetails, sourceModule);
							continue;
						}
					}
				}
				catch
				{
					_bpcd.Rollback();
					throw;
				}
				finally
				{
					_bpcd.CloseUpdateConnection();
				}
			}
			catch (FileNotFoundException fileNotFound_error)
			{
				string exceptionMessage = fileNotFound_error.Message;
				errorFound = true;
				message = " : " + fileLocation;
				_audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_InputFileNotFound, message, sourceModule);
				_audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
			}
			catch (Exception ex)
			{
				errorFound = true;
				_audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
				_audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
				throw;
			}
			finally
			{
				_audit.BuildPackCriteriaLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _recordsAddedUpdated);
			}
			return returnCode;
		}

		private eReturnCode SeparateDelimitedData(string line, string[] fields)
		{
			eReturnCode returnCode = eReturnCode.successful;
			string criteriaName = string.Empty;
			int compMinQuant = 0;
			int sizeMult = 0;
			int packMult = 0;
			string comboName = string.Empty;
			int comboNumPacks = 0;
			int colSize;
			string message;

			try
			{
				if (fields.Length != 6)
				{
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_InvalidBuildPackCriteriaRecord), sourceModule);
					returnCode = eReturnCode.severe;
				}

				criteriaName = fields[0];
				colSize = _bpcd.GetColumnSize("BUILD_PACK_CRITERIA", "BPC_NAME");

				if (criteriaName.Length > colSize)
				{
					message = MIDText.GetText(eMIDTextCode.msg_InvalidBuildPackCriteriaName);
					message = message.Replace("{0}", Convert.ToString(colSize));
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					returnCode = eReturnCode.severe;
				}

				try
				{
					compMinQuant = Convert.ToInt32(fields[1]);
				}
				catch (Exception ex)
				{
					message = MIDText.GetText(eMIDTextCode.msg_InvalidBuildPackCriteriaCompMin);
					message = message.Replace("{0}", ex.Message);
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					returnCode = eReturnCode.severe;
				}

				try
				{
					sizeMult = Convert.ToInt32(fields[2]);
				}
				catch (Exception ex)
				{
					message = MIDText.GetText(eMIDTextCode.msg_InvalidBuildPackCriteriaSizeMult);
					message = message.Replace("{0}", ex.Message);
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					returnCode = eReturnCode.severe;
				}

				comboName = fields[3];
				colSize = _bpcd.GetColumnSize("BUILD_PACK_COMBO", "BPC_COMBO_NAME");

				if (comboName.Length > colSize)
				{
					message = MIDText.GetText(eMIDTextCode.msg_InvalidBuildPackCriteriaComboName);
					message = message.Replace("{0}", Convert.ToString(colSize));
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					returnCode = eReturnCode.severe;
				}

				try
				{
					packMult = Convert.ToInt32(fields[4]);
				}
				catch (Exception ex)
				{
					message = MIDText.GetText(eMIDTextCode.msg_InvalidBuildPackCriteriaPackMult);
					message = message.Replace("{0}", ex.Message);
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					returnCode = eReturnCode.severe;
				}

				try
				{
					if (fields[5].Length == 0)
					{
						comboNumPacks = 1;
					}
					else
					{
						comboNumPacks = Convert.ToInt32(fields[5]);

						if (comboNumPacks == 0)
						{
							comboNumPacks = 1;
						}
					}
				}
				catch (Exception ex)
				{
					message = MIDText.GetText(eMIDTextCode.msg_InvalidBuildPackCriteriaMaxPacks);
					message = message.Replace("{0}", ex.Message);
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					returnCode = eReturnCode.severe;
				}

				if (returnCode == eReturnCode.successful)
				{
					_bpcd.BuildPackCriteria_Insert(criteriaName, compMinQuant, sizeMult, packMult, comboName, comboNumPacks);
				}

				return returnCode;
			}
			catch
			{
				throw;
			}
		}
	}
}
