using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;


namespace MIDRetail.SizeConstraintsLoad
{
	/// <summary>
	/// Summary description for SizeConstraintsLoadProcess.
	/// </summary>
	public class SizeConstraintsLoadProcess
	{
		SessionAddressBlock _SAB;

		private int _sizeGroupRid = -1;

		private int _sizeCurveGroupRid = -1;
		
		private int _storeGroupRid = -1;

		private SizeModelData _sizeModelData = null;	

		private Audit _audit = null;

        private ApplicationSessionTransaction _appSessTrans = null; 
		
		private Hashtable _storeGroupRIDs = null;

		private Hashtable _sizeCurveGroupRIDs = null;

		private Hashtable _sizeGroupRIDs = null;

		private DataTable _dtColors = null;

		private int _recordsRead = 0;
		private int _addRecords = 0;
		private int _updateRecords = 0;
		private int _deleteRecords = 0;
		private int _recordsWithErrors = 0;


		public SizeConstraintsLoadProcess(SessionAddressBlock SAB, ref bool scErrorFound)
		{
			_SAB = SAB;

			_audit = _SAB.ClientServerSession.Audit;

            _appSessTrans = new ApplicationSessionTransaction(_SAB); 

			ColorData cData = new ColorData();
			_dtColors = cData.Colors_Read();

			_sizeModelData = new SizeModelData();
			//_appSessTrans.GetMasterProfileList(eProfileType.ColorCode);

			//Get the Store Group Rids into the Hashtable storeGroupRIDs
			ProfileList sgpl = _appSessTrans.GetMasterProfileList(eProfileType.StoreGroup);
			if (sgpl.Count > 0)
			{
				_storeGroupRIDs = new Hashtable();
				foreach(StoreGroupProfile sgp in sgpl) 
				{
					if (!_storeGroupRIDs.ContainsKey(sgp.GroupId))
					{
						String rid = "" + sgp.Key;

						_storeGroupRIDs.Add(sgp.GroupId, rid);
					}
				}
			}
			else
			{
				scErrorFound = true;

				string msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_NoStoreGroupsAreCurrentlyDefined);//"No store groups are currently defined";
				_audit.Add_Msg(eMIDMessageLevel.Severe, msgText, GetType().Name);
			}

			//Get the Size Curve Group Rids into the Hashtable sizeCurveGroupRIDs
			DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
			SizeCurve objSizeCurve = new SizeCurve();
			dtSizeCurve = objSizeCurve.GetSizeCurveGroups();
			
			if(dtSizeCurve.Rows.Count > 0)
		    {
				_sizeCurveGroupRIDs = new Hashtable();
				foreach(DataRow dtr in dtSizeCurve.Rows)
				{
					String id = dtr["SIZE_CURVE_GROUP_NAME"].ToString();
					String rid = dtr["SIZE_CURVE_GROUP_RID"].ToString();
					if(!_sizeCurveGroupRIDs.ContainsKey(id))
					   _sizeCurveGroupRIDs.Add(id,rid);
				}
			}

			//Get the Size Group Rids into the Hashtable sizeGroupRIDs
			SizeGroupList sizgl = new SizeGroupList(eProfileType.SizeGroup);
			sizgl.LoadAll(false);
			if(sizgl.ArrayList.Count > 0)
			{
				_sizeGroupRIDs = new Hashtable();
				foreach(SizeGroupProfile sgp in sizgl.ArrayList)
				{
				   if(!_sizeGroupRIDs.ContainsKey(sgp.SizeGroupName))	
				     _sizeGroupRIDs.Add(sgp.SizeGroupName,"" + sgp.Key);
				}
			}
		}

		// ====================================================
		// Serialize and process the xml input transaction file
		// ====================================================
		public eReturnCode ProcessSizeConstraintsTrans(string sizeConstraintsTransFile, ref bool scErrorFound)
		{
			string msgText = null;

			TextReader xmlReader = null;
			XmlSerializer xmlSerial = null;

			SizeConstraintsModels sizeConTrans = null;

			eReturnCode rtnCode = eReturnCode.successful;

			// ================
			// Begin processing
			// ================
			try
			{ 
			    xmlSerial = new XmlSerializer(typeof(SizeConstraintsModels));
			    xmlReader = new StreamReader(sizeConstraintsTransFile);
			    sizeConTrans = (SizeConstraintsModels)xmlSerial.Deserialize(xmlReader);
			    xmlReader.Close();
				
			   	if(sizeConTrans.SizeConstraintsModel != null)
				{
					foreach(SizeConstraintsModelsSizeConstraintsModel conModel in sizeConTrans.SizeConstraintsModel)
					{
						++_recordsRead;
						

						if(conModel.ModelName==null || conModel.ModelName.Trim()=="")
						{
							//msgText = "Model Name not defined in input file" + System.Environment.NewLine;
							msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_ModelNameNotDefined);
							_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
							++_recordsWithErrors;
							
						}
						else if(conModel.SizeCurve == null && conModel.SizeGroup==null)
						{
							//msgText = "Both the Size curve and Size group values not defined for size constraint model " + conModel.ModelName + System.Environment.NewLine;
							msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_SizeGroupAndSizeCurveNotDefined);
							msgText = msgText.Replace("{0}",conModel.ModelName);

							_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
							++_recordsWithErrors;
						}
						else if(conModel.Set == null)
						{
							//msgText = "Atleast one ModelSet should be defined for size constraint model " + conModel.ModelName + System.Environment.NewLine;
							msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_ModelSetNotDefined);
							msgText = msgText.Replace("{0}",conModel.ModelName);

							_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
							++_recordsWithErrors;
						}
						else 
						{
							if(conModel.Attribute==null || conModel.Attribute=="")
								conModel.Attribute = "All Stores";	
							
							foreach(SizeConstraintsModelsSizeConstraintsModelSet modelSet in conModel.Set)
							{
								if(modelSet.SetName==null || modelSet.SetName.Trim() == "")
								{
									modelSet.SetName = "Default";
								}
							}

							if(conModel.Action==SizeConstraintsModelsSizeConstraintsModelAction.Create)
							{
								if(createUpdateSizeConstraint(conModel)==eReturnCode.successful)
							      ++_addRecords;
								else
								  ++_recordsWithErrors;
							}	 
							else if(conModel.Action==SizeConstraintsModelsSizeConstraintsModelAction.Modify)
							{
								if(createUpdateSizeConstraint(conModel)==eReturnCode.successful)
								  ++_updateRecords;
								else
								  ++_recordsWithErrors;

							}
							else if (conModel.Action==SizeConstraintsModelsSizeConstraintsModelAction.Remove)
							{
								if(removeSizeConstraint(conModel)==eReturnCode.successful)
								  ++_deleteRecords;
								else
								  ++_recordsWithErrors;
							}
						}
					}
				}
				else
				{
					//msgText = "No Size Constraints transaction found in input file" + System.Environment.NewLine;
					msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_SizeConstraintsNotDefined);
					_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
				}

			}
			catch (Exception Ex)
			{
				scErrorFound = true;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);

				rtnCode = eReturnCode.severe;
			}
			finally
			{
				if(xmlReader != null)
				  xmlReader.Close();
				_audit.SizeConstraintsLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _addRecords, _updateRecords, _deleteRecords);
			}

			return rtnCode;
		}


		/// <summary>
		/// 
		/// Fills the constraint collection.  Collection can be retrieved with CollectionSets property.
		/// </summary>
		private CollectionSets FillCollections(SizeConstraintsModelsSizeConstraintsModel conModel,int sizeConstraintModelRid,ref bool scError)
		{
			int setIdx = -1;
			int allColorIdx = -1;
			int colorIdx = -1;
			int allColorSizeIdx = -1;
			int colorSizeIdx = -1;
			int	colorSizeDimIdx = -1;
			int allColorSizeDimIdx = -1;

			int colorCodeRID = -1;
			int dimensionRID = -100;
			int sizeCodeRID = -1;
			int sizesRID = -1;
			int storeGroupLocRID = -1;

			scError = false;
			String msgText = "";

			int sizeMin  = 0;
			int sizeMax  = 0;
			int sizeMult = 0;
			DataTable dtDimensions = null;
			DataTable dtSizes = null;

			CollectionSets _setsCollection = new CollectionSets();

			//Get the Dimension List
			MaintainSizeConstraints maint = new MaintainSizeConstraints(this._sizeModelData);
			if(this._sizeGroupRid != -1)
			{
				dtSizes = maint.FillSizesList(_sizeGroupRid,eGetSizes.SizeGroupRID);
				dtDimensions = maint.FillSizeDimensionList(this._sizeGroupRid,eGetDimensions.SizeGroupRID);
			}
			else if(this._sizeCurveGroupRid != -1)
			{
				dtSizes = maint.FillSizesList(this._sizeCurveGroupRid,eGetSizes.SizeCurveGroupRID);
				dtDimensions = maint.FillSizeDimensionList(this._sizeCurveGroupRid,eGetDimensions.SizeCurveGroupRID);
			}

			//Get the no DIM String and the no dimension rid from dimension list
			int noDimRid = -100;
			String noDim = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
			DataRow[] drNoDim = dtDimensions.Select("SIZE_CODE_SECONDARY='" + noDim + "'");
			if(drNoDim != null && drNoDim.Length == 1)
			  noDimRid = int.Parse(drNoDim[0]["DIMENSIONS_RID"].ToString());
			if(noDimRid == -100)
			{
				noDim = MIDText.GetTextOnly((int)eMIDTextCode.str_NoSecondarySize);
				drNoDim = dtDimensions.Select("SIZE_CODE_SECONDARY='" + noDim + "'");
				if(drNoDim != null && drNoDim.Length == 1)
					noDimRid = int.Parse(drNoDim[0]["DIMENSIONS_RID"].ToString());
			}

            // Begin TT#5810 - JSmith - Size Constraint API failing
			//DataTable dtGrpLevel  = _sizeModelData.SetupGroupLevelTable(-1,_storeGroupRid);
			DataTable dtGrpLevel = _sizeModelData.SetupGroupLevelTable(-1, _storeGroupRid, StoreMgmt.StoreGroup_GetVersion(_storeGroupRid));
			// End TT#5810 - JSmith - Size Constraint API failing

			String prevSetName = "";
			String prevColorCode = "";
			int prevDimRID = -100;

			
						
			foreach(SizeConstraintsModelsSizeConstraintsModelSet mSet in conModel.Set)
			{
				storeGroupLocRID = -1;
				colorCodeRID = -1;
				dimensionRID = -100;
				sizeCodeRID  = -1;
			    sizesRID = -1;
				if(mSet.SizeMin==null || mSet.SizeMin.Trim()=="")
				  sizeMin = Include.UndefinedMinimum;
				else 
				  sizeMin = Convert.ToInt32(mSet.SizeMin,CultureInfo.CurrentUICulture);

				if(mSet.SizeMax==null || mSet.SizeMax.Trim()=="")
				  sizeMax = Include.UndefinedMaximum;
				else 
				  sizeMax = Convert.ToInt32(mSet.SizeMax,CultureInfo.CurrentUICulture);

				if(mSet.SizeMult==null || mSet.SizeMult.Trim()=="")
				  sizeMult = Include.UndefinedMultiple;
				else 
				  sizeMult = Convert.ToInt32(mSet.SizeMult,CultureInfo.CurrentUICulture);

				if(sizeMin >= sizeMax)
				{
					//msgText = " Size Min  or Size Mult cannot be greater thanSize Max. Size constraint model = " + conModel.ModelName + System.Environment.NewLine;
					msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidSizeMinAndMax);
					msgText = msgText.Replace("{0}",conModel.ModelName + " Set = " + mSet.SetName + 
                                              " Color = " + mSet.ColorCode +  " Size Min = " + sizeMin + 
											  " Size Max = "  + sizeMax + "Size Mult = " + sizeMult);

					_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
					scError = true;                 
					return null;
				}

				//Get the Store Group Lvl RID and setIdx
				DataRow[] setRows = dtGrpLevel.Select("BAND_DSC='" + mSet.SetName + "'");
				if(setRows == null || setRows.Length != 1)
				{
					//msgText = "Invalid Store group level defined in size constraint model " + conModel.ModelName + System.Environment.NewLine;
					msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidSizeGroupLevel);
					msgText = msgText.Replace("{0}",conModel.ModelName + "Store Group Level Name = " + mSet.SetName);

					_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
					scError = true;
					return null;
				}
				storeGroupLocRID = int.Parse(setRows[0]["SGL_RID"].ToString());
				
				//Get the Color Code RID
				if(mSet.ColorCode != null && mSet.ColorCode.Trim()!="" && mSet.ColorCode.Trim() != "All Colors")
				{
					DataRow[] drColor = _dtColors.Select("COLOR_CODE_ID='"+ mSet.ColorCode.Trim() +  "'");
					if(drColor.Length != 1)
					{
						//msgText = "Invalid  Color Code defined in size constraint model " + conModel.ModelName + System.Environment.NewLine;
						msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidColorCode);
						msgText = msgText.Replace("{0}",conModel.ModelName + 
												  " Set Name = " + mSet.SetName + 
												  " Color Code  = " + mSet.ColorCode);

						_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
						scError = true;
						return null;
					}

					colorCodeRID = int.Parse(drColor[0]["COLOR_CODE_RID"].ToString());
				}

				//Get Size Dimension RID
				if(mSet.SizeSecondary!=null && mSet.SizeSecondary.Trim()!="")
				{
					if(mSet.ColorCode==null || mSet.ColorCode.Trim()=="")
						mSet.ColorCode = "All Colors";

					DataRow[] drDimension = dtDimensions.Select("SIZE_CODE_SECONDARY='" + mSet.SizeSecondary.Trim() + "'");
					if(drDimension == null || drDimension.Length != 1)
					{
						//msgText = "Invalid  Dimension Code defined in size constraint model " + conModel.ModelName + System.Environment.NewLine;
						msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidDimensionCode);
						msgText = msgText.Replace("{0}",conModel.ModelName + 
												  " Set Name = " + mSet.SetName + 
												  " Color Code = " + mSet.ColorCode +
												  " Dimension Code  = " + mSet.SizeSecondary);
						_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
						scError = true;
						return null;
					}

					dimensionRID = int.Parse(drDimension[0]["DIMENSIONS_RID"].ToString());
				}

				//Get Size Code RID
				if(mSet.SizeCode!=null && mSet.SizeCode.Trim() != "")
				{
					DataRow[] drSizes = null;

					if(mSet.ColorCode==null || mSet.ColorCode.Trim()=="")
						mSet.ColorCode = "All Colors";
					
					if((mSet.SizePrimary == null || mSet.SizePrimary.Trim()=="") && 
						(mSet.SizeSecondary == null || mSet.SizeSecondary.Trim()==""))
					{
						drSizes = dtSizes.Select("SIZE_CODE = '" + mSet.SizeCode.Trim()+ "'");
					}

					else if((mSet.SizePrimary != null && mSet.SizePrimary.Trim()!="") && 
						(mSet.SizeSecondary == null || mSet.SizeSecondary.Trim()==""))
					{
						drSizes = dtSizes.Select("SIZE_CODE = '" + mSet.SizeCode.Trim()+ "'" + 
							" AND SIZE_CODE_PRIMARY = '" + mSet.SizePrimary.Trim() + "'");
					}

					else if((mSet.SizePrimary == null || mSet.SizePrimary.Trim()=="") && 
						(mSet.SizeSecondary != null && mSet.SizeSecondary.Trim()!=""))
					{
						drSizes = dtSizes.Select("SIZE_CODE = '" + mSet.SizeCode.Trim()+ "'" + 
							" AND SIZE_CODE_SECONDARY = '" + mSet.SizeSecondary.Trim() + "'");
					}

					else if((mSet.SizePrimary != null && mSet.SizePrimary.Trim()!="") && 
						(mSet.SizeSecondary != null && mSet.SizeSecondary.Trim()!=""))
					{
						drSizes = dtSizes.Select("SIZE_CODE = '" + mSet.SizeCode.Trim()+ "'" + 
							" AND SIZE_CODE_PRIMARY = '" + mSet.SizePrimary.Trim()+ "'"  +
							" AND SIZE_CODE_SECONDARY='" + mSet.SizeSecondary.Trim() + "'");
					}

					DataRow drSize = null;
					if(drSizes == null || drSizes.Length==0)
					{
						//msgText = "Invalid  Size Code defined in size constraint model " + conModel.ModelName + System.Environment.NewLine;
						msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidSizeCode);
						msgText = msgText.Replace("{0}",conModel.ModelName + 
							" Set Name = " + mSet.SetName +
							" Color Code = " + mSet.ColorCode + 
							" Size Primary = " + (mSet.SizePrimary ==null || mSet.SizePrimary.Trim()==""?"null":mSet.SizePrimary) + 
							" Size Secondary = " +(mSet.SizeSecondary==null || mSet.SizeSecondary.Trim()==""?noDim:mSet.SizeSecondary)  + 
							" Size Code = " + mSet.SizeCode);

						_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
						scError = true;
						return null;
					}
					else if(drSizes.Length > 1)
					{
						if(mSet.SizeSecondary != null && mSet.SizeSecondary.Trim()!="")
						{
							msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidSizeCode);
							msgText = msgText.Replace("{0}",conModel.ModelName + 
								" Set Name = " + mSet.SetName +
								" Color Code = " + mSet.ColorCode + 
								" Size Primary = " + (mSet.SizePrimary ==null || mSet.SizePrimary.Trim()==""?"null":mSet.SizePrimary) + 
								" Size Secondary = " +(mSet.SizeSecondary==null || mSet.SizeSecondary.Trim()==""?noDim:mSet.SizeSecondary)  + 
								" Size Code = " + mSet.SizeCode);

							_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
							scError = true;
							return null;
						}

						foreach(DataRow dr in drSizes)
						{
							if(noDimRid != -100 && Convert.ToString(dr["SIZE_CODE_SECONDARY"])==noDim)
							{
								drSize = dr;
								break;
							}
						}					
					}
					else //if(drSizes.Length==1)
					{
						drSize = drSizes[0];
					}
				
					if(drSize==null)
					{
						msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidSizeCode);
						msgText = msgText.Replace("{0}",conModel.ModelName + 
							" Set Name = " + mSet.SetName +
							" Color Code = " + mSet.ColorCode + 
							" Size Primary = " + (mSet.SizePrimary ==null || mSet.SizePrimary.Trim()==""?"null":mSet.SizePrimary) + 
							" Size Secondary = " +(mSet.SizeSecondary==null || mSet.SizeSecondary.Trim()==""?noDim:mSet.SizeSecondary)  + 
							" Size Code = " + mSet.SizeCode);

						_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
						scError = true;
						return null;
					}
					sizeCodeRID  = int.Parse(drSize["SIZE_CODE_RID"].ToString());
					
					if(dimensionRID==-100)
					  dimensionRID = int.Parse(drSize["DIMENSIONS_RID"].ToString());
					//sizesRID    = int.Parse(drSize[0]["SIZES_RID"].ToString());
				}
				else if(mSet.SizePrimary!=null && mSet.SizePrimary.Trim() != "")
				{
					if(mSet.ColorCode==null || mSet.ColorCode.Trim()=="")
						mSet.ColorCode = "All Colors";

					DataRow[] drSizes = null;
					if(mSet.SizeSecondary != null && mSet.SizeSecondary.Trim() != "")
					{
				       drSizes = dtSizes.Select("SIZE_CODE_PRIMARY = '" + mSet.SizePrimary.Trim()+ "'" + 
							         " And SIZE_CODE_SECONDARY = '" +  mSet.SizeSecondary.Trim() + "'");
					}
					else
					{
					  drSizes = dtSizes.Select("SIZE_CODE_PRIMARY = '" + mSet.SizePrimary.Trim()+ "'");
					}

					DataRow drSize = null;
					if(drSizes == null || drSizes.Length==0)
					{
						//msgText = "Invalid  Size Code defined in size constraint model " + conModel.ModelName + System.Environment.NewLine;
						msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidSizeCode);
						msgText = msgText.Replace("{0}",conModel.ModelName + 
							" Set Name = " + mSet.SetName +
							" Color Code = " + mSet.ColorCode + 
							" Size Primary = " + mSet.SizePrimary + 
							" Size Secondary = " + (mSet.SizeSecondary==null || mSet.SizeSecondary.Trim()==""?"null":mSet.SizeSecondary)  + 
							" Size Code = null" );

						_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
						scError = true;
						return null;
					}
					else if(drSizes.Length > 1)
					{
						if(mSet.SizeSecondary != null && mSet.SizeSecondary.Trim()!="")
						{
							msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidSizeCode);
							msgText = msgText.Replace("{0}",conModel.ModelName + 
								" Set Name = " + mSet.SetName +
								" Color Code = " + mSet.ColorCode + 
								" Size Primary = " + mSet.SizePrimary + 
								" Size Secondary = " + (mSet.SizeSecondary==null || mSet.SizeSecondary.Trim()==""?"null":mSet.SizeSecondary)  + 
								" Size Code = null" );

							_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
							scError = true;
							return null;
						}

						foreach(DataRow dr in drSizes)
						{
							if(noDimRid != -1 && Convert.ToString(dr["SIZE_CODE_SECONDARY"])==noDim)
							{
								drSize = dr;
								break;
							}
						}					
					}
					else
					{
						drSize = drSizes[0];
					}
					
					if(drSize==null)
					{
						msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidSizeCode);
						msgText = msgText.Replace("{0}",conModel.ModelName + 
							" Set Name = " + mSet.SetName +
							" Color Code = " + mSet.ColorCode + 
							" Size Primary = " + mSet.SizePrimary + 
							" Size Secondary = " +(mSet.SizeSecondary==null || mSet.SizeSecondary.Trim()==""?noDim:mSet.SizeSecondary)  + 
							" Size Code = null" + mSet.SizeCode);

						_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
						scError = true;
						return null;
					}
					sizeCodeRID  = int.Parse(drSize["SIZE_CODE_RID"].ToString());
					
					if(dimensionRID==-100)
						dimensionRID = int.Parse(drSize["DIMENSIONS_RID"].ToString());
					//sizesRID    = int.Parse(drSize[0]["SIZES_RID"].ToString());
				}

				
				//Reset All collection Set indexes
				if(mSet.SetName != prevSetName)
				{
					prevColorCode = "";
					prevDimRID = -100;
				}
				else if(prevColorCode != mSet.ColorCode)
				{
					prevDimRID = -100;
				}

				//Default Set Level
				if(storeGroupLocRID == -1 && colorCodeRID == -1 && 
					(mSet.ColorCode == null || mSet.ColorCode.Trim()=="") && 
					dimensionRID == -100 && sizeCodeRID == -1)
				{
					setIdx = _setsCollection.Add(
					new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
					sizeMin,sizeMax,sizeMult,-1,-1,eSizeMethodRowType.Default));
				}
				
				//Set Level
				else if(storeGroupLocRID != -1 && colorCodeRID == -1 && 
					    (mSet.ColorCode == null || mSet.ColorCode.Trim()=="") &&
					    dimensionRID == -100 && sizeCodeRID == -1)
				{
					setIdx = _setsCollection.Add(
					new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
					sizeMin,sizeMax,sizeMult,-1,-1,eSizeMethodRowType.Set));
				}

					//Set and All Color Level
				else if(mSet.ColorCode =="All Colors" && dimensionRID == -100 && sizeCodeRID == -1)
				{
					if(mSet.SetName != prevSetName)
					{
						//Create the set level if it hasnt been already created
						if(storeGroupLocRID != -1)//Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Set));

						}
						else //Default Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Default));

						}
					}

					allColorIdx = _setsCollection[setIdx].collectionAllColors.Add(new ItemAllColor(sizeConstraintModelRid,colorCodeRID,
										sizeMin,sizeMax,sizeMult,-1,-1,-1,-1,-1,
							storeGroupLocRID,eSizeMethodRowType.AllColor));
				}

				//Set And Color Level
				else if(colorCodeRID != -1 && dimensionRID == -100 && sizeCodeRID == -1)
				{
					if(mSet.SetName != prevSetName)
					{
						//Create the set level if it hasnt been already created
						if(storeGroupLocRID != -1)//Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Set));

						}
						else //Default Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Default));

						}
					}

				   colorIdx = _setsCollection[setIdx].collectionColors.Add(new ItemColor(sizeConstraintModelRid,colorCodeRID,
							sizeMin,sizeMax,sizeMult,-1,-1,-1,-1,
							-1,storeGroupLocRID,eSizeMethodRowType.Color));					
				}

				//Set, All Color and dimension level
				else if(mSet.ColorCode == "All Colors" && dimensionRID != -100 && sizeCodeRID == -1)
				{
					//Create the set if not already created
					if(mSet.SetName != prevSetName)
					{
						//Create the set level if it hasnt been already created
						if(storeGroupLocRID != -1)//Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Set));

						}
						else //Default Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Default));

						}
					}
					
					//Create the all color level if not already created
					if(mSet.ColorCode != prevColorCode && mSet.ColorCode == "All Colors")
					{
						allColorIdx = _setsCollection[setIdx].collectionAllColors.Add(
							new ItemAllColor(sizeConstraintModelRid,colorCodeRID,
							Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
							-1,-1,-1,-1,-1,storeGroupLocRID,eSizeMethodRowType.AllColor));
					}

					allColorSizeDimIdx = _setsCollection[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions.Add(
							new ItemSizeDimension(sizeConstraintModelRid,colorCodeRID,
												  sizeMin,sizeMax,sizeMult,-1,-1,-1,-1,
												  dimensionRID,storeGroupLocRID,eSizeMethodRowType.AllColorSizeDimension));
				}

				//Set, Color and dimension level
				else if(colorCodeRID != -1 && dimensionRID != -100 && sizeCodeRID == -1)
				{
					//Create the set if not already created
					if(mSet.SetName != prevSetName)
					{
						//Create the set level if it hasnt been already created
						if(storeGroupLocRID != -1)//Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Set));

						}
						else //Default Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Default));

						}
					}
					
					//Create the color if not already created
					if(mSet.ColorCode != prevColorCode && mSet.ColorCode != "All Colors")
					{
						colorIdx = _setsCollection[setIdx].collectionColors.Add(
							new ItemColor(sizeConstraintModelRid,colorCodeRID,
							Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
							-1,-1,-1,-1,-1,storeGroupLocRID,eSizeMethodRowType.Color));					
					}

					colorSizeDimIdx = _setsCollection[setIdx].collectionColors[colorIdx].collectionSizeDimensions.Add(
							new ItemSizeDimension(sizeConstraintModelRid,colorCodeRID,
							sizeMin,sizeMax,sizeMult,-1,-1,-1,-1,
							dimensionRID,storeGroupLocRID,eSizeMethodRowType.ColorSizeDimension));
				}

				//Set, All Color, dimension and Size Code  level
				else if(mSet.ColorCode == "All Colors" && dimensionRID != -100 && sizeCodeRID != -1)
				{
					
					//Create the set if not already created
					if(mSet.SetName != prevSetName)
					{
						//Create the set level if it hasnt been already created
						if(storeGroupLocRID != -1)//Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Set));

						}
						else //Default Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Default));

						}
					}
					
					//Create the all color level if not already created
					if(mSet.ColorCode != prevColorCode && mSet.ColorCode == "All Colors")
					{
							allColorIdx = _setsCollection[setIdx].collectionAllColors.Add(
								new ItemAllColor(sizeConstraintModelRid,colorCodeRID,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,-1,-1,-1,storeGroupLocRID,eSizeMethodRowType.AllColor));
					}

					//Create the dimension level if not already created
					if(dimensionRID != prevDimRID)
					{
						allColorSizeDimIdx = _setsCollection[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions.Add(
							new ItemSizeDimension(sizeConstraintModelRid,colorCodeRID,
							Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
							-1,-1,-1,-1,dimensionRID,storeGroupLocRID,eSizeMethodRowType.AllColorSizeDimension));
					}

					allColorSizeIdx = _setsCollection[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions[allColorSizeDimIdx].
							collectionSizes.Add(new ItemSize(sizeConstraintModelRid,colorCodeRID,
							sizeMin,sizeMax,sizeMult,-1,-1,sizesRID,sizeCodeRID,
							dimensionRID,storeGroupLocRID,eSizeMethodRowType.AllColorSize));
				}
				
				//Set, Color, dimension and Size Code  level
				else if(colorCodeRID != -1 && dimensionRID != -100 && sizeCodeRID != -1)
				{
					//Create the set if not already created
					if(mSet.SetName != prevSetName)
					{
						//Create the set level if it hasnt been already created
						if(storeGroupLocRID != -1)//Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Set));

						}
						else //Default Set Level
						{
							setIdx = _setsCollection.Add(
								new ItemSet(sizeConstraintModelRid,storeGroupLocRID,false,0,false,0,
								Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
								-1,-1,eSizeMethodRowType.Default));

						}
					}
					
					//Create the color if not already created
					if(mSet.ColorCode != prevColorCode && mSet.ColorCode != "All Colors")
					{
						colorIdx = _setsCollection[setIdx].collectionColors.Add(
							new ItemColor(sizeConstraintModelRid,colorCodeRID,
							Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
							-1,-1,-1,-1,-1,storeGroupLocRID,eSizeMethodRowType.Color));					
					}
					
					//Create Dimension level if not already created
					if(dimensionRID != prevDimRID)
					{
						colorSizeDimIdx = _setsCollection[setIdx].collectionColors[colorIdx].collectionSizeDimensions.Add(
							new ItemSizeDimension(sizeConstraintModelRid,colorCodeRID,
							Include.UndefinedMinimum,Include.UndefinedMaximum,Include.UndefinedMultiple,
							-1,-1,-1,-1,dimensionRID,storeGroupLocRID,eSizeMethodRowType.ColorSizeDimension));
					}

					colorSizeIdx = _setsCollection[setIdx].collectionColors[colorIdx].collectionSizeDimensions[colorSizeDimIdx].
							collectionSizes.Add(new ItemSize(sizeConstraintModelRid,colorCodeRID,
							sizeMin,sizeMax,sizeMult,-1,-1,sizesRID,sizeCodeRID,
							dimensionRID,storeGroupLocRID,eSizeMethodRowType.ColorSize));
				}


				prevSetName = mSet.SetName;
				prevColorCode = mSet.ColorCode;
				prevDimRID = dimensionRID;			
			}
		   
		    return _setsCollection;
		}

		private eReturnCode createUpdateSizeConstraint(SizeConstraintsModelsSizeConstraintsModel conModel)
		{
			eReturnCode rtnCode = eReturnCode.successful;
			String msgText = "";


			if(_storeGroupRIDs.ContainsKey(conModel.Attribute))
			{
				_storeGroupRid = int.Parse((String)_storeGroupRIDs[conModel.Attribute]);
			}
			else
			{
				//msgText = "Invalid store group set for constraint model " + conModel.ModelName + System.Environment.NewLine;
				msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidStoreGroup);
				msgText = msgText.Replace("{0}",conModel.ModelName + " Store Group Name/Attribute field in xml = " + conModel.Attribute);

				_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
				return eReturnCode.editErrors;
			}

			if(conModel.SizeGroup != null)
			{
				if(_sizeGroupRIDs != null &&
					_sizeGroupRIDs.ContainsKey(conModel.SizeGroup))
				{
					_sizeGroupRid = int.Parse((String)_sizeGroupRIDs[conModel.SizeGroup]);
				}
				else
				{
					//msgText = "Invalid size group set for constraint model " + conModel.ModelName + System.Environment.NewLine;
					msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidSizeGroup);
					msgText = msgText.Replace("{0}",conModel.ModelName + " Size Group = " + conModel.SizeGroup);

					_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
					return eReturnCode.editErrors;
				}
			}
			else if(conModel.SizeCurve != null)
			{
				if(_sizeCurveGroupRIDs.ContainsKey(conModel.SizeCurve))
				{
					_sizeCurveGroupRid = int.Parse((String)_sizeCurveGroupRIDs[conModel.SizeCurve]);
				}
				else
				{
					//msgText = "Invalid size curve group set for constraint model " + conModel.ModelName + System.Environment.NewLine;
					msgText= _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_scl_InvalidSizeCurveGroup);
					msgText = msgText.Replace("{0}",conModel.ModelName + " Size Curve Group Name = " + conModel.SizeCurve);

					_audit.Add_Msg(eMIDMessageLevel.Edit, msgText, GetType().Name);
					return eReturnCode.editErrors;
				}
			}

			TransactionData td = new TransactionData();
			try
			{
				MaintainSizeConstraints maint = new MaintainSizeConstraints(_sizeModelData);
				SizeConstraintModelProfile aModel = maint.GetConstrainModel(conModel.ModelName);
				td.OpenUpdateConnection();
				int sizeConstraintModelRid = _sizeModelData.SizeConstraintModel_Update_Insert(aModel.Key,
																							  conModel.ModelName, 
																							  _sizeGroupRid, 
																							  _sizeCurveGroupRid,
																							 _storeGroupRid,
																							 td);

				bool scError = false;
				CollectionSets _setsCollection = FillCollections(conModel,sizeConstraintModelRid,ref scError);
				if(!scError)
				{
					maint.insertUpdateCollection(sizeConstraintModelRid,td,_setsCollection);
					td.CommitData();
				}
				else
				{
					td.Rollback();
					rtnCode = eReturnCode.editErrors;
				}

				td.CloseUpdateConnection();
			}
			catch (Exception ex)
			{
				if (td.ConnectionIsOpen) 
				{
					td.Rollback();
					td.CloseUpdateConnection();
				}

				_audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), GetType().Name);				
				rtnCode = eReturnCode.severe;
			}


			return rtnCode;
		}

		private eReturnCode  removeSizeConstraint(SizeConstraintsModelsSizeConstraintsModel conModel)
		{
			TransactionData td = new TransactionData();
			eReturnCode rtnCode = eReturnCode.successful;
			try
			{
				MaintainSizeConstraints maint = new MaintainSizeConstraints(_sizeModelData);
				SizeConstraintModelProfile aModel = maint.GetConstrainModel(conModel.ModelName);
				td.OpenUpdateConnection();
				maint.deleteSizeConstraintChildren(aModel.Key,td);
				this._sizeModelData.SizeConstraintModel_Delete(aModel.Key, td);
				td.CommitData();
			}
			catch (Exception ex)
			{
				if (td.ConnectionIsOpen) 
				{
					td.Rollback();
					td.CloseUpdateConnection();
				}

				_audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), GetType().Name);
				rtnCode = eReturnCode.severe;
				
			}

			return rtnCode;
		}
	}
}
