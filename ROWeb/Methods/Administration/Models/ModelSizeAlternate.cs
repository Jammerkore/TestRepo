using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public class ModelSizeAlternate : ModelBase
    {
        //=======
        // FIELDS
        //=======
        private SizeAltModelProfile _sizeAltModelProfile;
        private SizeModelData _sizeModelData;
        private ROModelSizeAlternateProperties _modelProperties;
        private DataSet _dsAlt = new DataSet();
        private bool _promptSizeChange = false;
        private ArrayList _sizeCodeKeyList = new ArrayList();
        private ArrayList _sizeCodeIDList = new ArrayList();
        private ArrayList _sizeKeyList = new ArrayList();
        private ArrayList _sizeIDList = new ArrayList();
        private ArrayList _dimKeyList = new ArrayList();
        private ArrayList _dimIDList = new ArrayList();
        private ArrayList _altSizeCodeKeyList = new ArrayList();
        private ArrayList _altSizeCodeIDList = new ArrayList();
        private ArrayList _altSizeKeyList = new ArrayList();
        private ArrayList _altSizeIDList = new ArrayList();
        private ArrayList _altDimKeyList = new ArrayList();
        private ArrayList _altDimIDList = new ArrayList();
        private string _message = string.Empty;
        //=============
        // CONSTRUCTORS
        //=============
        public ModelSizeAlternate(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base(SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.SizeAlternates)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeAlternates);
        }

        //===========
        // PROPERTIES
        //===========



        //========
        // METHODS
        //========

        override public bool OnClosing()
        {
            return true;
        }

        override public List<KeyValuePair<int, string>> ModelGetList()
        {
            _sizeModelData = new SizeModelData();
            DataTable dtSizeModels = _sizeModelData.SizeAlternateModel_Read();

            dtSizeModels = ApplicationUtilities.SortDataTable(dataTable: dtSizeModels, sColName: "SIZE_ALTERNATE_NAME", bAscending: true);

            return ApplicationUtilities.DataTableToKeyValues(dtSizeModels, "SIZE_ALTERNATE_RID", "SIZE_ALTERNATE_NAME");
        }

        override public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false)
        {
            // populate modelProperties using Windows\SizeAlertnateMaint.cs as a reference
            _sizeAltModelProfile = (SizeAltModelProfile)modelProfile;

            int primarySizeCurveKey = _sizeAltModelProfile.PrimarySizeCurveRid;
            int alternateSizeCurveKey = _sizeAltModelProfile.AlternateSizeCurveRid;

            if (parms is ROSizeAlternateModelParms)
            {
                ROSizeAlternateModelParms sizeAlternateModelParms = (ROSizeAlternateModelParms)parms;
                if (sizeAlternateModelParms.PrimarySizeCurveKey != Include.NoRID)
                {
                    primarySizeCurveKey = sizeAlternateModelParms.PrimarySizeCurveKey;
                }
                if (sizeAlternateModelParms.AlternateSizeCurveKey != Include.NoRID)
                {
                    alternateSizeCurveKey = sizeAlternateModelParms.AlternateSizeCurveKey;
                }
            }

            //create model properties
            KeyValuePair<int, string> model = new KeyValuePair<int, string>(key: _sizeAltModelProfile.Key, value: _sizeAltModelProfile.SizeAlternateName);
            _modelProperties = new ROModelSizeAlternateProperties(model: model,
                primarySizeCurve: GetName.GetSizeCurveGroupName(primarySizeCurveKey),
                alternateSizeCurve: GetName.GetSizeCurveGroupName(alternateSizeCurveKey)
                );

            //file size list from model
            try
            {
                if (_sizeAltModelProfile != null
                    && _sizeAltModelProfile.AlternateSizeList != null)
                {
                    foreach (SizeAlternatePrimary sap in _sizeAltModelProfile.AlternateSizeList)
                    {
                        ROSizeAlternatePrimarySize sizeAlternatePrimarySize = new ROSizeAlternatePrimarySize(
                            seq: sap.Sequence,
                            size: GetName.GetSizeAlternateModelSize(primarySizeCurveKey, sap.SizeRid),
                            dimension: GetName.GetSizeAlternateModelDimension(primarySizeCurveKey, sap.DimensionRid)
                            );
                        int alternateSequence = 1;
                        foreach (SizeAlternate sa in sap.AlternateList)
                        {
                            ROSizeAlternateAlternateSize sizeAlternateAlternateSize = new ROSizeAlternateAlternateSize(
                                seq: alternateSequence, 
                                size: GetName.GetSizeAlternateModelSize(alternateSizeCurveKey, sa.SizeRid), 
                                dimension: GetName.GetSizeAlternateModelDimension(alternateSizeCurveKey, sa.DimensionRid)
                                );
                            sizeAlternatePrimarySize.SizeAlternateAlternateSizes.Add(sizeAlternateAlternateSize);
                            ++alternateSequence;
                        }
                        
                        _modelProperties.SizeAlternatePrimarySizes.Add(sizeAlternatePrimarySize);
                    }
                }
            }
            catch (Exception exc)
            {
                message = exc.ToString();
                throw;
            }

            eGetSizes getSizesUsing = eGetSizes.SizeCurveGroupRID;
            eGetDimensions getDimensionsUsing = eGetDimensions.SizeCurveGroupRID;

            if (primarySizeCurveKey > Include.NoRID)
            {
                FillDimensionSizeList(
                    sizeDimensionSizes: _modelProperties.PrimarySizeCurveDimensions,
                    Key: primarySizeCurveKey,
                    getDimensions: getDimensionsUsing,
                    getSizes: getSizesUsing
                    );
            }

            if (alternateSizeCurveKey > Include.NoRID)
            {
                FillDimensionSizeList(
                    sizeDimensionSizes: _modelProperties.AlternateSizeCurveDimensions,
                    Key: alternateSizeCurveKey,
                    getDimensions: getDimensionsUsing,
                    getSizes: getSizesUsing
                    );
            }

            return _modelProperties;
        }
        private SizeAltModelProfile GetAlternateModel(string modelName)
        {
            SizeAltModelProfile aModel = null;
            DataTable dt = _sizeModelData.SizeAlternateModel_Read(modelName);
            if (dt.Rows.Count == 0)
            {
                aModel = new SizeAltModelProfile(Include.NoRID);
            }
            else
            {
                DataRow aRow = dt.Rows[0];
                int aModelKey = Convert.ToInt32(aRow["SIZE_ALTERNATE_RID"]);
                aModel = new SizeAltModelProfile(aModelKey);
            }
            return aModel;
        }

        /// <summary>
        /// Updates the Model in memory and then updates the database
        /// </summary>
        /// <param name="modelsProperties">Input values for the Size Curve</param>
        /// <param name="cloneDates">A flag identifying if any dates are to be cloned</param>
        /// <param name="message">Output message</param>
        /// <returns></returns>
        override public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;

            _sizeModelData = new SizeModelData();
            ROModelSizeAlternateProperties sizeAlternateProperties = (ROModelSizeAlternateProperties)modelsProperties;
            int sizeAlternateModelRid = modelsProperties.Model.Key;
            string saveName = modelsProperties.Model.Value.ToString();

            bool continueSave = false;

            try
            {
                //if either new model or copy as model
                if (sizeAlternateModelRid == Include.NoRID)
                {
                    SizeAltModelProfile checkExists = GetAlternateModel(saveName);
                    if (checkExists.Key == Include.NoRID) //new model name does not exist
                    {
                        _sizeAltModelProfile.Key = sizeAlternateModelRid;
                        _sizeAltModelProfile.ModelID = saveName;
                        _sizeAltModelProfile.SizeAlternateName = saveName;
                        _sizeAltModelProfile.ModelChangeType = eChangeType.add;
                        continueSave = true;
                    }
                    else //new model name does exist
                    {
                        message = eMIDTextCode.msg_DuplicateName.ToString();
                        continueSave = false;
                        successful = false;
                    }
                }
                //model is update of existing 
                else
                {
                    //current model equals saveing model
                    if (_sizeAltModelProfile.Key == sizeAlternateModelRid)
                    {
                        _sizeAltModelProfile.Key = sizeAlternateModelRid;
                        _sizeAltModelProfile.ModelID = saveName;
                        _sizeAltModelProfile.SizeAlternateName = saveName;
                        _sizeAltModelProfile.ModelChangeType = eChangeType.update;
                        continueSave = true;
                    }
                    else
                    {
                        message = eMIDTextCode.msg_SaveCanceled.ToString();
                        continueSave = false;
                        successful = false;
                    }
                }


                if (continueSave)
                {
                    //move changes from properties to model profile
                    SaveToProfile(sizeAlternateProperties);

                    if (!applyOnly)
                    {
                        if (!string.IsNullOrEmpty(_sizeAltModelProfile.SizeAlternateName))
                        {
                            // Save file alternate model data
                            successful = InsertUpdateSizeAlternates(sizeAlternateModelRid);
                            if (successful)
                            {
                                message = eMIDTextCode.msg_ProcessCompletedSuccessfully.ToString();
                            }
                            else
                            {
                                successful = false;
                            }
                        }
                        else
                        {
                            message = eMIDTextCode.msg_NameRequiredToSave.ToString();
                            successful = false;
                        }
                    }
                }
                else
                {
                    message = eMIDTextCode.msg_SaveCanceled.ToString();
                    successful = false;
                }
            }
            catch (Exception exception)
            {
                throw (exception);
            }
            finally
            {

            }

            return _sizeAltModelProfile;
        }

        /// <summary>
        /// reads the data from the model properties and places it in the current SizeAlternateProfile
        /// it does this by using the DataSet DataSource. 
        /// </summary>
        private void SaveToProfile(ROModelSizeAlternateProperties sizeAlternateProperties)
        {
            try
            {
                _promptSizeChange = false;

                if (sizeAlternateProperties.PrimarySizeCurve.Key != _sizeAltModelProfile.PrimarySizeCurveRid)
                {
                    _sizeAltModelProfile.PrimarySizeCurveRid = sizeAlternateProperties.PrimarySizeCurve.Key;
                    sizeAlternateProperties.PrimarySizeCurve = GetName.GetSizeCurveGroupName(sizeCurveGroupRID: sizeAlternateProperties.PrimarySizeCurve.Key);
                    _promptSizeChange = true;

                }
                if (sizeAlternateProperties.AlternateSizeCurve.Key != _sizeAltModelProfile.AlternateSizeCurveRid)
                {
                    _sizeAltModelProfile.AlternateSizeCurveRid = sizeAlternateProperties.AlternateSizeCurve.Key;
                    sizeAlternateProperties.AlternateSizeCurve = GetName.GetSizeCurveGroupName(sizeCurveGroupRID: sizeAlternateProperties.AlternateSizeCurve.Key);
                    _promptSizeChange = true;
                }
                // Clean up old data
                _sizeAltModelProfile.ClearAltSizeList();
                int seq = 0;

                if (_promptSizeChange) // create a blank primary row
                {
                    SizeAlternatePrimary aPrimary = new SizeAlternatePrimary();
                    //load default primary size and dimension
                    bool proceed = LoadPrimarySizeArrays();
                    if (proceed)
                    {
                        // create blank size list
                        aPrimary.Sequence = 1;
                        aPrimary.SizeRid = Convert.ToInt32(_sizeKeyList[0]);
                        aPrimary.DimensionRid =  Convert.ToInt32(_dimKeyList[0]);
                        if (sizeAlternateProperties.SizeAlternatePrimarySizes.Count == 0)
                        {
                            KeyValuePair<int, string> size = GetName.GetSizeAlternateModelSize(
                                sizeCurveRID: sizeAlternateProperties.PrimarySizeCurve.Key,
                                sizeCodeRID: Convert.ToInt32(_sizeKeyList[0])
                                );
                            KeyValuePair<int, string> dimension = GetName.GetSizeAlternateModelDimension(
                                sizeCurveRID: sizeAlternateProperties.PrimarySizeCurve.Key,
                                dimensionRID: Convert.ToInt32(_dimKeyList[0])
                                );
                            sizeAlternateProperties.SizeAlternatePrimarySizes.Add(new ROSizeAlternatePrimarySize(
                                seq: 0,
                                size: size,
                                dimension: dimension)
                                );
                        }
                        else
                        {
                            sizeAlternateProperties.SizeAlternatePrimarySizes[0].Size = GetName.GetSizeAlternateModelSize(
                                sizeCurveRID: sizeAlternateProperties.PrimarySizeCurve.Key,
                                sizeCodeRID: Convert.ToInt32(_sizeKeyList[0])
                                );
                            sizeAlternateProperties.SizeAlternatePrimarySizes[0].Dimension = GetName.GetSizeAlternateModelDimension(
                                sizeCurveRID: sizeAlternateProperties.PrimarySizeCurve.Key,
                                dimensionRID: Convert.ToInt32(_dimKeyList[0])
                                );
                        }
                    }
                    else
                    {
                        aPrimary.Sequence = 1;
                        aPrimary.SizeRid = Include.NoRID;
                        aPrimary.DimensionRid = Include.NoRID;
                    }
                    _sizeAltModelProfile.AlternateSizeList.Add(aPrimary);
                }
                else 
                {
                    //build size list in model from properties
                    foreach (ROSizeAlternatePrimarySize sizeAlternatePrimarySize in sizeAlternateProperties.SizeAlternatePrimarySizes)
                    {
                        if (sizeAlternatePrimarySize.SizeAlternateAlternateSizes != null
                            && sizeAlternatePrimarySize.SizeAlternateAlternateSizes.Count > 0)
                        {
                            SizeAlternatePrimary aPrimary = new SizeAlternatePrimary();
                            aPrimary.Sequence = ++seq;//==== Convert.ToInt32(sizeAlternatePrimarySize.Seq);
                            aPrimary.SizeRid = sizeAlternatePrimarySize.Size.Key;
                            aPrimary.DimensionRid = sizeAlternatePrimarySize.Dimension.Key;
                            sizeAlternatePrimarySize.Size = GetName.GetSizeAlternateModelSize(sizeAlternateProperties.PrimarySizeCurve.Key, sizeAlternatePrimarySize.Size.Key);
                            sizeAlternatePrimarySize.Dimension = GetName.GetSizeAlternateModelDimension(sizeAlternateProperties.PrimarySizeCurve.Key, sizeAlternatePrimarySize.Dimension.Key);
                            //add alternate set
                            int alternateSequence = 1;
                            foreach (ROSizeAlternateAlternateSize sizeAlternateAlternateSize in sizeAlternatePrimarySize.SizeAlternateAlternateSizes)
                            {
                                SizeAlternate aAlternate = new SizeAlternate();
                                aAlternate.Sequence = alternateSequence;
                                aAlternate.SizeRid = sizeAlternateAlternateSize.Size.Key;
                                aAlternate.DimensionRid = sizeAlternateAlternateSize.Dimension.Key;
                                sizeAlternateAlternateSize.Size = GetName.GetSizeAlternateModelSize(sizeAlternateProperties.AlternateSizeCurve.Key, sizeAlternateAlternateSize.Size.Key);
                                sizeAlternateAlternateSize.Dimension = GetName.GetSizeAlternateModelDimension(sizeAlternateProperties.AlternateSizeCurve.Key, sizeAlternateAlternateSize.Dimension.Key);
                                aPrimary.AlternateList.Add(aAlternate);
                                ++alternateSequence;
                            }
                            _sizeAltModelProfile.AlternateSizeList.Add(aPrimary);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        private bool LoadPrimarySizeArrays()
        {
            bool success = false;
            _sizeCodeKeyList.Clear();
            _sizeCodeIDList.Clear();
            _sizeKeyList.Clear();
            _sizeIDList.Clear();
            _dimKeyList.Clear();
            _dimIDList.Clear();

            if (this._sizeAltModelProfile.PrimarySizeCurveRid != Include.NoRID)
            {
                SizeCurveGroupProfile _primSizeCurveGroupProfile = new SizeCurveGroupProfile(_sizeAltModelProfile.PrimarySizeCurveRid);
                ProfileList scl = _primSizeCurveGroupProfile.SizeCodeList;
                //productCatStr = ((SizeCodeProfile)(scl.ArrayList[0])).SizeCodeProductCategory;

                // begin MID Track 5812 Size Alternate Model Gets Error on Create
                if (scl.Count > 0)
                {
                    foreach (SizeCodeProfile scp in scl.ArrayList)
                    {
                        if (scp.Key == -1)
                        {
                            _message = MIDText.GetText(eMIDTextCode.msg_CantRetrieveSizeCode);
                            break;
                        }
                         _sizeCodeKeyList.Add(scp.Key);
                        _sizeCodeIDList.Add(scp.SizeCodeID);
                        if (!_sizeIDList.Contains(scp.SizeCodePrimary))
                        {
                            _sizeIDList.Add(scp.SizeCodePrimary);
                            _sizeKeyList.Add(scp.SizeCodePrimaryRID);
                        }
                        if (!_dimIDList.Contains(scp.SizeCodeSecondary))
                        {
                            _dimIDList.Add(scp.SizeCodeSecondary);
                            _dimKeyList.Add(scp.SizeCodeSecondaryRID);
                        }
                    }
                    success = true;
                } 
            }

            return success;
        }

        private bool LoadAlternateSizeArrays()
        {
            bool success = false;
            _altSizeCodeKeyList.Clear();
            _altSizeCodeIDList.Clear();
            _altSizeKeyList.Clear();
            _altSizeIDList.Clear();
            _altDimKeyList.Clear();
            _altDimIDList.Clear();

            if (_sizeAltModelProfile.AlternateSizeCurveRid != Include.NoRID)
            {
                SizeCurveGroupProfile _altSizeCurveGroupProfile = new SizeCurveGroupProfile(_sizeAltModelProfile.AlternateSizeCurveRid);
                ProfileList scl = _altSizeCurveGroupProfile.SizeCodeList;

                foreach (SizeCodeProfile scp in scl.ArrayList)
                {
                    if (scp.Key == -1)
                    {
                        _message = MIDText.GetText(eMIDTextCode.msg_CantRetrieveSizeCode);
                        break;
                    }
                    _altSizeCodeKeyList.Add(scp.Key);
                    _altSizeCodeIDList.Add(scp.SizeCodeID);
                    if (!_altSizeIDList.Contains(scp.SizeCodePrimary))
                    {
                        _altSizeIDList.Add(scp.SizeCodePrimary);
                        _altSizeKeyList.Add(scp.SizeCodePrimaryRID);
                    }
                    if (!_altDimIDList.Contains(scp.SizeCodeSecondary))
                    {
                        _altDimIDList.Add(scp.SizeCodeSecondary);
                        _altDimKeyList.Add(scp.SizeCodeSecondaryRID);
                    }
                }
                success = true;
            }

            return success;
        }

        private bool DeleteSizeAlternateChildren(TransactionData td)
        {
            bool successful = true;

            try
            {
                successful = _sizeModelData.DeleteSizeAlternateChildren(_sizeAltModelProfile.Key, td);

            }
            catch (Exception)
            {
                throw;
            }

            return successful;

        }
        private bool InsertSizeAlternateChildren(TransactionData td)
        {
            bool successful = true;

            try
            {
                successful = DeleteSizeAlternateChildren(td);
                if (successful)
                {
                    foreach (SizeAlternatePrimary sap in _sizeAltModelProfile.AlternateSizeList)
                    {
                        _sizeModelData.SizeAltPrimarySize_insert(_sizeAltModelProfile.Key, sap.Sequence, sap.SizeRid, sap.DimensionRid, td);
                        int altSeq = 0;
                        foreach (SizeAlternate sa in sap.AlternateList)
                        {
                            _sizeModelData.SizeAltAlternateSize_insert(_sizeAltModelProfile.Key, sap.Sequence, ++altSeq, sa.SizeRid, sa.DimensionRid, td);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return successful;

        }
        private bool InsertUpdateSizeAlternates(int sizeAlternateModelKey)
        {
            bool successfull = true;
            TransactionData td = new TransactionData();

            try
            {
                td.OpenUpdateConnection();
                //if the model exists the _insert will  update instead
                int SizeAltModelRid = _sizeModelData.SizeAlternateModel_Insert(sizeAlternateModelKey, _sizeAltModelProfile.ModelID, _sizeAltModelProfile.PrimarySizeCurveRid, _sizeAltModelProfile.AlternateSizeCurveRid, td);
                _sizeAltModelProfile.Key = SizeAltModelRid;
                //delete then add the size alternate children
                successfull = InsertSizeAlternateChildren(td);
                td.CommitData();
            }
            catch 
            {
                td.Rollback();
                throw;
            }
            finally
            {
                td.CloseUpdateConnection();
            }
            return successfull;
        }

       

        /// <summary>
        /// Deletes the Model in memory and then updates the database
        /// </summary>
        /// <param name="key">A int identifying the model ID to delete</param>
        /// <param name="message">Output message</param>
        /// <returns></returns>
        override public bool ModelDelete(int key, ref string message)
        {
            TransactionData td = new TransactionData();
            try
            {
                int currIndex = key;
                if (currIndex != Include.NoRID)
                {
                    if (_functionSecurity.AllowDelete)
                    { 
                        _sizeAltModelProfile = new SizeAltModelProfile(currIndex);
                        eProfileType type = _sizeAltModelProfile.ProfileType;
                        _sizeModelData = new SizeModelData();
                        _sizeAltModelProfile.ModelChangeType = eChangeType.delete;
                        td.OpenUpdateConnection();
                        _sizeModelData.DeleteSizeAlternateChildren(currIndex, td);
                        _sizeModelData.SizeAlternateModel_Delete(currIndex, td);
                        td.CommitData();
                    }
                }
            }
            catch (DatabaseForeignKeyViolation)
            {
                td.Rollback();
                message = eMIDTextCode.msg_DeleteFailedDataInUse.ToString();
                return false;
                
            }
            catch (Exception exception)
            {
                td.Rollback();
                message = exception.Message.ToString();
                return false;
            }
            finally
            {
                td.CloseUpdateConnection();
                message = eMIDTextCode.msg_DeleteSuccessfulWithValue.ToString();

            }
            return true;
        }
        
        override public bool ModelNameExists(string name)
        {
            ModelProfile checkExists = SAB.HierarchyServerSession.GetModelData(aModelType: eModelType.SizeAlternates, modelID: name);

            return checkExists.Key != Include.NoRID;
        }

        override public ROModelParms GetModelParms(ROModelPropertiesParms parms, eModelType modelType, int key, bool readOnly = false)
        {
            ROModelSizeAlternateProperties sizeAlternateProperties = (ROModelSizeAlternateProperties)parms.ROModelProperties;

            ROModelParms modelParms = new ROSizeAlternateModelParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.GetModel,
                ROInstanceID: parms.ROInstanceID,
                modelType: modelType,
                key: key,
                readOnly: readOnly,
                primarySizeCurveKey: sizeAlternateProperties.PrimarySizeCurve.Key,
                alternateSizeCurveKey: sizeAlternateProperties.AlternateSizeCurve.Key
                );

            return modelParms;
        }
    }
}
