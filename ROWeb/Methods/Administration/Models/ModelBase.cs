using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
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
    abstract public class ModelBase
    {
        //=======
        // FIELDS
        //=======
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private eModelType _modelType;
        protected FunctionSecurityProfile _functionSecurity;
        protected FunctionSecurityProfile _userSecurity = null;
        protected FunctionSecurityProfile _globalSecurity = null;
        private ModelProfile _currentModelProfile = null;

        //=============
        // CONSTRUCTORS
        //=============
        public ModelBase(SessionAddressBlock SAB, ROWebTools ROWebTools, eModelType modelType)
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _modelType = modelType;
        }

        //===========
        // PROPERTIES
        //===========
        /// <summary>
        /// Gets SessionAddressBlock.
        /// </summary>
        public SessionAddressBlock SAB
        {
            get
            {
                return _SAB;
            }
        }
        /// <summary>
        /// Gets the ROWebTools
        /// </summary>
        public ROWebTools ROWebTools
        {
            get { return _ROWebTools; }
        }

        public eModelType ModelType
        {
            get
            {
                return _modelType;
            }
        }

        public FunctionSecurityProfile FunctionSecurity
        {
            get
            {
                return _functionSecurity;
            }
        }

        public FunctionSecurityProfile UserSecurity
        {
            get
            {
                return _userSecurity;
            }
        }

        public FunctionSecurityProfile GlobalSecurity
        {
            get
            {
                return _globalSecurity;
            }
        }

        public ModelProfile CurrentModelProfile
        {
            get
            {
                return _currentModelProfile;
            }
            set
            {
                _currentModelProfile = value;
            }
        }

        //========
        // METHODS
        //========


        abstract public List<KeyValuePair<int, string>> ModelGetList();

        abstract public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false);

        abstract public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false);

        abstract public bool ModelDelete(int key, ref string message);

        abstract public bool ModelNameExists(string name);

        abstract public bool OnClosing();

        virtual public ModelProfile GetModelProfile(ROModelParms parms)
        {
            ModelProfile modelProfile;

            if (parms.ReadOnly
                || !FunctionSecurity.AllowUpdate
                || parms.Key < 0)
            {
                modelProfile = _SAB.HierarchyServerSession.GetModelData(aModelType: parms.ModelType, aModelRID: parms.Key);
            }
            else
            {
                modelProfile = (ModelProfile)_SAB.HierarchyServerSession.GetModelDataForUpdate(aModelType: parms.ModelType, aModelRID: parms.Key, aAllowReadOnly: true, isWindows: MIDEnvironment.isWindows);
                if (modelProfile.ModelLockStatus == eLockStatus.ReadOnly)
                {
                    MIDEnvironment.isChangedToReadOnly = true;
                }
            }

            return modelProfile;
        }

        protected List<KeyValuePair<int, string>> ConvertProfileListToList(ProfileList modelProfileList)
        {
            List<KeyValuePair<int, string>> keyValueList = new List<KeyValuePair<int, string>>();
            SortedList modelSortedList = new SortedList();

            foreach (ModelName modelName in modelProfileList.ArrayList)
            {
                modelSortedList.Add(modelName.ModelID, modelName.Key);
            }

            foreach (DictionaryEntry de in modelSortedList)
            {
                keyValueList.Add(new KeyValuePair<int, string>(Convert.ToInt32(de.Value), Convert.ToString(de.Key)));
            }

            return keyValueList;
        }

        virtual public ROModelParms GetModelParms(ROModelPropertiesParms parms, eModelType modelType, int key, bool readOnly = false)
        {
            ROModelParms modelParms = new ROModelParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.GetModel,
                ROInstanceID: parms.ROInstanceID,
                modelType: modelType,
                key: key,
                readOnly: readOnly
                );

            return modelParms;
        }

        virtual public ROModelParms GetModelParms(ROModelParms parms, eModelType modelType, int key, bool readOnly = false)
        {
            ROModelParms modelParms = new ROModelParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.GetModel,
                ROInstanceID: parms.ROInstanceID,
                modelType: modelType,
                key: key,
                readOnly: readOnly
                );

            return modelParms;
        }

        public void UnlockModel(eModelType modelType, int key)
        {
            _SAB.HierarchyServerSession.DequeueModel(aModelType: modelType, aModelRID: key);
        }

        public eLockStatus LockModel(eModelType modelType, int key, string name, bool allowReadOnly, out string message)
        {
            message = null;
            eLockStatus lockStatus = eLockStatus.Undefined;

            if (key == Include.NoRID)
            {
                return lockStatus;
            }

            ModelEnqueue modelEnqueue = new ModelEnqueue(
                    modelType,
                    key,
                    SAB.ClientServerSession.UserRID,
                    SAB.ClientServerSession.ThreadID);

            try
            {
                modelEnqueue.EnqueueModel();
                lockStatus = eLockStatus.Locked;
            }
            catch (ModelConflictException)
            {
                message = "The following model(s) requested:" + System.Environment.NewLine;
                foreach (ModelConflict MCon in modelEnqueue.ConflictList)
                {
                    message += System.Environment.NewLine + "Model: " + name + ", User: " + MCon.UserName;
                }
                message += System.Environment.NewLine + System.Environment.NewLine;
                if (allowReadOnly)
                {
                    lockStatus = eLockStatus.ReadOnly;
                    message += "The model is not available to be updated at this time.";
                }
                else
                {
                    message += MIDText.GetText(eMIDTextCode.msg_NodeNotAffected);
                    lockStatus = eLockStatus.Cancel;
                }
            }

            return lockStatus;
        }

        /// <summary>
		/// Fills a KeyValuePair List with colors.
		/// </summary>
		/// <param name="colorList">KeyValuePair object to fill</param>
		protected void FillColorList(List<KeyValuePair<int, string>> colorList)
        {
            ColorData colorData = new ColorData();
            DataTable dataTableColors = colorData.Colors_Read();

            foreach (DataRow dataRow in dataTableColors.Rows)
            {
                colorList.Add(new KeyValuePair<int, string>(
                    Convert.ToInt32(dataRow["COLOR_CODE_RID"],  CultureInfo.CurrentUICulture),
                    dataRow["COLOR_CODE_ID"].ToString() + " - " + dataRow["COLOR_CODE_NAME"].ToString())
                    );
            }

        }

        /// <summary>
        /// Fills class with size dimensions.
        /// </summary>
        /// <remarks>Method must be overridden</remarks>
        protected void FillDimensionSizeList(List<ROSizeDimension> sizeDimensionSizes, int Key, eGetDimensions getDimensions, eGetSizes getSizes)
        {
            ROSizeDimension dimensionSizes;
            int dimensionKey;
            string dimension;
            SizeModelData sizeModelData = new SizeModelData();
            MaintainSizeConstraints maint = new MaintainSizeConstraints(sizeModelData);
            DataTable dtDimensions = maint.FillSizeDimensionList(Key, getDimensions);
            DataTable dtSizes = maint.FillSizesList(Key, getSizes);

            foreach (DataRow dr in dtDimensions.Rows)
            {
                dimensionKey = Convert.ToInt32(dr["DIMENSIONS_RID"]);
                dimension = dr["SIZE_CODE_SECONDARY"].ToString();
                dimensionSizes = new ROSizeDimension(dimension: new KeyValuePair<int, string>(
                    dimensionKey,
                    dimension)
                    );
                FillSizesList(dimensionSizes: dimensionSizes, dtSizes: dtSizes, dimensionKey: dimensionKey);
                sizeDimensionSizes.Add(dimensionSizes);

            }
        }

        /// <summary>
		/// Fills class with sizes based on a selected Size Group or Size Curve
		/// </summary>
		protected void FillSizesList(ROSizeDimension dimensionSizes, DataTable dtSizes, int dimensionKey)
        {
            int sizeKey;
            string size;

            DataRow[] SelectRows = dtSizes.Select("DIMENSIONS_RID = '" + dimensionKey.ToString() + "'");

            foreach (DataRow dr in SelectRows)
            {
                sizeKey = Convert.ToInt32(dr["SIZES_RID"]);
                size = dr["SIZE_CODE_PRIMARY"].ToString();
                dimensionSizes.Sizes.Add(new KeyValuePair<int, string>(
                    sizeKey,
                    size)
                    );
            }
        }

        /// <summary>
		/// Fills a KeyValuePair List with size groups.
		/// </summary>
		/// <param name="sizeGroups">KeyValuePair object to fill</param>
		protected void FillSizeGroupList(List<KeyValuePair<int, string>> sizeGroups)
        {
            SizeGroup dataLayersizeGroupData = new SizeGroup();
            DataTable dataTableSizeGroups = dataLayersizeGroupData.GetSizeGroups(false);
            sizeGroups.AddRange(ApplicationUtilities.DataTableToKeyValues(dataTableSizeGroups, "SIZE_GROUP_RID", "SIZE_GROUP_NAME"));
        }

        /// <summary>
		/// Fills a KeyValuePair List with size curve groups.
		/// </summary>
		/// <param name="sizeCurveGroups">KeyValuePair object to fill</param>
		protected void FillSizeCurveGroupList(List<KeyValuePair<int, string>> sizeCurveGroups)
        {
            SizeCurve dataLayerSizeCurve = new SizeCurve();
            DataTable dataTableSizeCurveGroups = dataLayerSizeCurve.GetSizeCurveGroups();
            sizeCurveGroups.AddRange(ApplicationUtilities.DataTableToKeyValues(dataTableSizeCurveGroups, "SIZE_CURVE_GROUP_RID", "SIZE_CURVE_GROUP_NAME"));

        }

    }
}
