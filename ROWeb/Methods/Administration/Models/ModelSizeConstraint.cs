using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Globalization;
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
    public class ModelSizeConstraint : ModelBase
    {
        //=======
        // FIELDS
        //=======
        protected const string C_SET = "SETLEVEL",
            C_SET_ALL_CLR = "SETALLCOLOR",
            C_SET_CLR = "SETCOLOR",
            C_ALL_CLR_SZ = "ALLCOLORSIZE",
            C_CLR_SZ = "COLORSIZE",
            C_CLR_SZ_DIM = "COLORSIZEDIMENSION",
            C_ALL_CLR_SZ_DIM = "ALLCOLORSIZEDIMENSION",

            C_TABLE_SET = "SetLevel",
            C_TABLE_ALL_CLR = "AllColor",
            C_TABLE_CLR = "Color",
            C_TABLE_ALL_CLR_SZ = "AllColorSize",
            C_TABLE_CLR_SZ = "ColorSize",
            C_TABLE_CLR_SZ_DIM = "ColorSizeDimension",
            C_TABLE_ALL_CLR_SZ_DIM = "AllColorSizeDimension";

        private SizeConstraintModelProfile _sizeConstraintProfile;
        private CollectionSets _setsCollection;
        private DataSet _sizeConstraints;
        private DataSet _dsBackup = null;
        private int _originalAttributeKey = Include.NoRID;
        private int _originalSizeGroupKey = Include.NoRID;
        private int _originalSizeCurveGroupKey = Include.NoRID;
        private bool _constraintRollback = false;
        private bool _createConstraintData = true;
        private SizeModelData _sizeModelData = new SizeModelData();

        //=============
        // CONSTRUCTORS
        //=============
        public ModelSizeConstraint(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.SizeConstraints)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeConstraints);
        }

        //===========
        // PROPERTIES
        //===========



        //========
        // METHODS
        //========

        override public bool OnClosing()
        {
            return RestoreConstraintValues();
        }

        public bool RestoreConstraintValues()
        {
            TransactionData td = null;
            SizeModelData sizeModelData = new SizeModelData();

            try
            {
                //CREATE AND OPEN CONNECTION HERE SO THE POSSIBLE ROLLBACKS
                //ARE ON THE SAME TRANSACTION.  THE INSERT UPDATES WILL
                //OPEN AND COMMIT/ROLLBACK DATA UNLESS THE PROVIDED TRANSACTIONDATA
                //OBJECT IS ALREADY OPEN.
                td = new TransactionData();

                if (_constraintRollback)
                {
                    if (!td.ConnectionIsOpen)
                    {
                        td.OpenUpdateConnection();
                    }
                    _sizeConstraints = _dsBackup;
                    FillCollections(sizeConstraintModelKey: _sizeConstraintProfile.Key, attributeKey: _originalAttributeKey, sizeGroupKey: _originalSizeGroupKey, sizeCurveGroupKey: _originalSizeCurveGroupKey);
                    InsertUpdateSizeConstraints(sizeConstraintModekKey: _sizeConstraintProfile.Key, td: td, sizeModelData: sizeModelData);

                    if (td.ConnectionIsOpen)
                    {
                        td.CommitData();
                        td.CloseUpdateConnection();
                    }
                }
            }
            catch (Exception)
            {
                if (td != null
                    && td.ConnectionIsOpen)
                {
                    td.Rollback();
                    td.CloseUpdateConnection();
                }
            }

            return true;
        }

        override public List<KeyValuePair<int, string>> ModelGetList()
        {
            SizeModelData sizeModelData = new SizeModelData();

            DataTable dtSizeModel = sizeModelData.SizeConstraintModel_Read();

            dtSizeModel = ApplicationUtilities.SortDataTable(dataTable: dtSizeModel, sColName: "SIZE_CONSTRAINT_NAME", bAscending: true);

            return ApplicationUtilities.DataTableToKeyValues(dtSizeModel, "SIZE_CONSTRAINT_RID", "SIZE_CONSTRAINT_NAME");
        }

        int _definedAttribute = Include.Undefined;

        override public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false)
        {
            // populate modelProperties using Windows\SizeConstraintsMaint.cs as a reference

            if (_sizeConstraintProfile != null
                && modelProfile.Key != _sizeConstraintProfile.Key
                && _constraintRollback)
            {
                RestoreConstraintValues();
                _constraintRollback = false;
            }

            _sizeConstraintProfile = (SizeConstraintModelProfile)modelProfile;

            int attributeKey = _sizeConstraintProfile.StoreGroupRid;
            int attributeSetKey = Include.NoRID;
            int sizeGroupKey = _sizeConstraintProfile.SizeGroupRid;
            int sizeCurveGroupKey = _sizeConstraintProfile.SizeCurveGroupRid;

            if (parms is ROSizeConstraintModelParms)
            {
                ROSizeConstraintModelParms sizeConstraintModelParms = (ROSizeConstraintModelParms)parms;
                if (sizeConstraintModelParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = sizeConstraintModelParms.AttributeKey;
                }
                if (sizeConstraintModelParms.AttributeSetKey != Include.NoRID)
                {
                    attributeSetKey = sizeConstraintModelParms.AttributeSetKey;
                }
                if (sizeConstraintModelParms.SizeGroupKey != Include.NoRID)
                {
                    sizeGroupKey = sizeConstraintModelParms.SizeGroupKey;
                }
                if (sizeConstraintModelParms.SizeCurveGroupKey != Include.NoRID)
                {
                    sizeCurveGroupKey = sizeConstraintModelParms.SizeCurveGroupKey;
                }
            }

            if (_definedAttribute == Include.Undefined)
            {
                _definedAttribute = _sizeConstraintProfile.StoreGroupRid;
            }

            KeyValuePair<int, string> model = new KeyValuePair<int, string>(_sizeConstraintProfile.Key, _sizeConstraintProfile.SizeConstraintName);
            ROModelSizeConstraintProperties modelProperties = new ROModelSizeConstraintProperties(model: model,
                attribute: GetName.GetAttributeName(key: attributeKey),
                attributeSet: GetName.GetAttributeSetName(key: attributeSetKey),
                sizeCurveGroup: GetName.GetSizeCurveGroupName(sizeCurveGroupRID: sizeCurveGroupKey),
                sizeGroup: GetName.GetSizeGroup(key: sizeGroupKey),
                definedAttribute: GetName.GetAttributeName(key: _definedAttribute)
                );

            modelProperties.DefaultLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Default);

            if (_createConstraintData)
            {
                CreateConstraintData(attributeKey: attributeKey, sizeGroupKey: sizeGroupKey, sizeCurveGroupKey: sizeCurveGroupKey, fillCollections: true);
                _createConstraintData = false;
            }

            if (!applyOnly)
            {
                _dsBackup = _sizeConstraints.Copy();  // backup this Model's grid data
                _originalAttributeKey = attributeKey;
                _originalSizeGroupKey = sizeGroupKey;
                _originalSizeCurveGroupKey = sizeCurveGroupKey;
            }

            BuildConstraints(modelProperties: modelProperties, sizeGroupKey: sizeGroupKey, sizeCurveGroupKey: sizeCurveGroupKey, message: ref message);

            FillColorList(
                colorList: modelProperties.Colors,
                addDefaultColor: false,
                addAllColors: false
                );

            FillSizeGroupList(modelProperties.SizeGroups);

            FillSizeCurveGroupList(modelProperties.SizeCurveGroups);

            return modelProperties;
        }

        private bool CreateConstraintData(int attributeKey, int sizeGroupKey, int sizeCurveGroupKey, bool fillCollections)
        {
            bool Success = true;
            bool getSizes = false;

            eGetSizes getSizesUsing = eGetSizes.SizeGroupRID;
            eGetDimensions getDimensionsUsing = eGetDimensions.SizeGroupRID;

            if (sizeGroupKey != Include.NoRID)
            {
                getSizesUsing = eGetSizes.SizeGroupRID;
                getDimensionsUsing = eGetDimensions.SizeGroupRID;
            }
            else
            {
                getSizesUsing = eGetSizes.SizeCurveGroupRID;
                getDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
            }

            SizeModelData sizeModelData = new SizeModelData();

            try
            {
                _sizeConstraints = MIDEnvironment.CreateDataSet();

                switch (getSizesUsing)
                {
                    case eGetSizes.SizeGroupRID:
                        if (sizeGroupKey != Include.NoRID)
                        {
                            getSizes = true;
                        }
                        else
                        {
                            getSizes = false;
                        }
                        break;
                    case eGetSizes.SizeCurveGroupRID:
                        if (sizeCurveGroupKey != Include.NoRID)
                        {
                            getSizes = true;
                        }
                        else
                        {
                            getSizes = false;
                        }
                        break;
                }


                _sizeConstraints = sizeModelData.GetChildData(_sizeConstraintProfile.Key, _sizeConstraintProfile.StoreGroupRid, getSizes, StoreMgmt.StoreGroup_GetVersion(_sizeConstraintProfile.StoreGroupRid));

                DetermineSizeSequence(attributeKey: attributeKey, sizeGroupKey: sizeGroupKey, sizeCurveGroupKey: sizeCurveGroupKey);

                if (fillCollections)
                {
                    FillCollections(sizeConstraintModelKey: _sizeConstraintProfile.Key, attributeKey: attributeKey, sizeGroupKey: sizeGroupKey, sizeCurveGroupKey: sizeCurveGroupKey);
                }

            }
            catch
            {
                Success = false;
                throw;
            }
            return Success;

        }

        private bool BuildConstraintDataSet(ROModelSizeConstraintProperties sizeConstraintProperties, ref string message)
        {
            if (_sizeConstraints == null)
            {
                CreateConstraintData(attributeKey: sizeConstraintProperties.Attribute.Key,
                    sizeGroupKey: sizeConstraintProperties.SizeGroup.Key,
                    sizeCurveGroupKey: sizeConstraintProperties.SizeCurveGroup.Key,
                    fillCollections: false
                    );
            }
            else
            {
                // clear table bottom up because of constraints
                string selectString = "SGL_RID=" + sizeConstraintProperties.SizeConstraintAttributeSet.AttributeSet.Key;

                if (_sizeConstraints.Tables.Contains(C_TABLE_ALL_CLR_SZ))
                {
                    ClearSetRows(tableName: C_TABLE_ALL_CLR_SZ, selectString: selectString);
                }
                if (_sizeConstraints.Tables.Contains(C_TABLE_ALL_CLR_SZ_DIM))
                {
                    ClearSetRows(tableName: C_TABLE_ALL_CLR_SZ_DIM, selectString: selectString);
                }
                if (_sizeConstraints.Tables.Contains(C_TABLE_ALL_CLR))
                {
                    ClearSetRows(tableName: C_TABLE_ALL_CLR, selectString: selectString);
                }
                if (_sizeConstraints.Relations.Contains(C_TABLE_CLR_SZ))
                {
                    ClearSetRows(tableName: C_TABLE_CLR_SZ, selectString: selectString);
                }
                if (_sizeConstraints.Relations.Contains(C_TABLE_CLR_SZ_DIM))
                {
                    ClearSetRows(tableName: C_TABLE_CLR_SZ_DIM, selectString: selectString);
                }
                if (_sizeConstraints.Tables.Contains(C_TABLE_CLR))
                {
                    ClearSetRows(tableName: C_TABLE_CLR, selectString: selectString);
                }
                if (_sizeConstraints.Tables.Contains(C_TABLE_SET))
                {
                    ClearSetRows(tableName: C_TABLE_SET, selectString: selectString);
                }
            }

            BuildConstraint(sizeConstraintModelKey: sizeConstraintProperties.Model.Key,
                attributeSetKey: sizeConstraintProperties.SizeConstraintAttributeSet.AttributeSet.Key,
                attributeSetName: sizeConstraintProperties.SizeConstraintAttributeSet.AttributeSet.Value,
                sizeConstraints: sizeConstraintProperties.SizeConstraintAttributeSet.SizeConstraints,
                message: ref message);

            return true;
        }

        private void ClearSetRows(string tableName, string selectString)
        {
            DataRow[] constraintDataRows = _sizeConstraints.Tables[tableName].Select(selectString);
            foreach (var constraintDataRow in constraintDataRows)
            {
                constraintDataRow.Delete();
            }
            _sizeConstraints.Tables[tableName].AcceptChanges();
        }

        private bool BuildConstraint(int sizeConstraintModelKey,
            int attributeSetKey, 
            string attributeSetName, 
            ROSizeConstraints sizeConstraints, 
            ref string message)
        {
            int sizeDimensionSequence, sizeSequence;

            AddSetRow(sizeConstraintModelKey: sizeConstraintModelKey,
                    attributeSetKey: attributeSetKey,
                    attributeSetName: attributeSetName,
                    sizeConstraintValues: sizeConstraints.ConstraintValues,
                    message: ref message);

            foreach (ROSizeConstraintColor sizeConstraintColor in sizeConstraints.SizeConstraintColor)
            {
                if (sizeConstraintColor.Color.Key == Include.NoRID)
                {
                    AddAllColorRow(sizeConstraintModelKey: sizeConstraintModelKey,
                        attributeSetKey: attributeSetKey,
                        attributeSetName: attributeSetName,
                        colorCodeKey: sizeConstraintColor.Color.Key,
                        sizeConstraintValues: sizeConstraintColor.ConstraintValues,
                        message: ref message);
                }
                else
                {
                    AddColorRow(sizeConstraintModelKey: sizeConstraintModelKey,
                        attributeSetKey: attributeSetKey,
                        attributeSetName: attributeSetName,
                        colorCodeKey: sizeConstraintColor.Color.Key,
                        sizeConstraintValues: sizeConstraintColor.ConstraintValues,
                        message: ref message);
                }
                sizeDimensionSequence = 1;
                foreach (ROSizeConstraintDimension sizeConstraintDimension in sizeConstraintColor.SizeConstraintDimension)
                {
                    if (sizeConstraintColor.Color.Key == Include.NoRID)
                    {
                        AddAllColorSizeDimensionRow(sizeConstraintModelKey: sizeConstraintModelKey,
                            attributeSetKey: attributeSetKey,
                            attributeSetName: attributeSetName,
                            colorCodeKey: sizeConstraintColor.Color.Key,
                            dimensionKey: sizeConstraintDimension.Dimension.Key,
                            sizeSequence: sizeDimensionSequence,
                            sizeConstraintValues: sizeConstraintDimension.ConstraintValues,
                            message: ref message);
                    }
                    else
                    {
                        AddColorSizeDimensionRow(sizeConstraintModelKey: sizeConstraintModelKey,
                            attributeSetKey: attributeSetKey,
                            attributeSetName: attributeSetName,
                            colorCodeKey: sizeConstraintColor.Color.Key,
                            dimensionKey: sizeConstraintDimension.Dimension.Key,
                            sizeSequence: sizeDimensionSequence,
                            sizeConstraintValues: sizeConstraintDimension.ConstraintValues,
                            message: ref message);
                    }
                    sizeSequence = 1;
                    foreach (ROSizeConstraintSize sizeConstraintSize in sizeConstraintDimension.SizeConstraintSize)
                    {
                        if (sizeConstraintColor.Color.Key == Include.NoRID)
                        {
                            AddAllColorSizeDimensionSizeRow(sizeConstraintModelKey: sizeConstraintModelKey,
                                attributeSetKey: attributeSetKey,
                                attributeSetName: attributeSetName,
                                colorCodeKey: sizeConstraintColor.Color.Key,
                                dimensionKey: sizeConstraintDimension.Dimension.Key,
                                sizeKey: sizeConstraintSize.Size.Key,
                                sizeSequence: sizeSequence,
                                sizeConstraintValues: sizeConstraintSize.ConstraintValues,
                                message: ref message);
                        }
                        else
                        {
                            AddColorSizeDimensionSizeRow(sizeConstraintModelKey: sizeConstraintModelKey,
                                attributeSetKey: attributeSetKey,
                                attributeSetName: attributeSetName,
                                colorCodeKey: sizeConstraintColor.Color.Key,
                                dimensionKey: sizeConstraintDimension.Dimension.Key,
                                sizeKey: sizeConstraintSize.Size.Key,
                                sizeSequence: sizeSequence,
                                sizeConstraintValues: sizeConstraintSize.ConstraintValues,
                                message: ref message);
                        }
                        ++sizeSequence;
                    }
                    ++sizeDimensionSequence;
                }
            }


            return true;
        }

        private void AddSetRow(int sizeConstraintModelKey,
            int attributeSetKey,
            string attributeSetName,
            ROSizeConstraintValues sizeConstraintValues,
            ref string message)
        {
            DataRow dr;

            dr = _sizeConstraints.Tables[C_TABLE_SET].NewRow();
            dr["BAND_DSC"] = attributeSetName;
            dr["SIZE_CONSTRAINT_RID"] = sizeConstraintModelKey;
            dr["SGL_RID"] = attributeSetKey;
            if (sizeConstraintValues.MinimumSet)
            {
                dr["SIZE_MIN"] = sizeConstraintValues.Minimum;
            }
            if (sizeConstraintValues.MaximumSet)
            {
                dr["SIZE_MAX"] = sizeConstraintValues.Maximum;
            }
            if (sizeConstraintValues.MultipleSet)
            {
                dr["SIZE_MULT"] = sizeConstraintValues.Multiple;
            }
            if (attributeSetKey == Include.NoRID)
            {
                dr["ROW_TYPE_ID"] = eSizeMethodRowType.Default.GetHashCode();
            }
            else
            {
                dr["ROW_TYPE_ID"] = eSizeMethodRowType.Set.GetHashCode();
            }
            _sizeConstraints.Tables[C_TABLE_SET].Rows.Add(dr);
        }

        private void AddAllColorRow(int sizeConstraintModelKey,
            int attributeSetKey,
            string attributeSetName,
            int colorCodeKey,
            ROSizeConstraintValues sizeConstraintValues,
            ref string message)
        {
            DataRow dr;

            dr = _sizeConstraints.Tables[C_TABLE_ALL_CLR].NewRow();
            dr["BAND_DSC"] = "All Colors";
            dr["SIZE_CONSTRAINT_RID"] = sizeConstraintModelKey;
            dr["SGL_RID"] = attributeSetKey;
            dr["COLOR_CODE_RID"] = colorCodeKey;
            dr["SIZES_RID"] = Include.NoRID;
            dr["DIMENSIONS_RID"] = Include.NoRID;
            dr["SIZE_CODE_RID"] = Include.NoRID;
            if (sizeConstraintValues.MinimumSet)
            {
                dr["SIZE_MIN"] = sizeConstraintValues.Minimum;
            }
            if (sizeConstraintValues.MaximumSet)
            {
                dr["SIZE_MAX"] = sizeConstraintValues.Maximum;
            }
            if (sizeConstraintValues.MultipleSet)
            {
                dr["SIZE_MULT"] = sizeConstraintValues.Multiple;
            }
             dr["ROW_TYPE_ID"] = eSizeMethodRowType.AllColor.GetHashCode();

            _sizeConstraints.Tables[C_TABLE_ALL_CLR].Rows.Add(dr);

        }

        private void AddAllColorSizeDimensionRow(int sizeConstraintModelKey,
            int attributeSetKey,
            string attributeSetName,
            int colorCodeKey,
            int dimensionKey,
            int sizeSequence,
            ROSizeConstraintValues sizeConstraintValues,
            ref string message)
        {
            DataRow dr;

            dr = _sizeConstraints.Tables[C_TABLE_ALL_CLR_SZ_DIM].NewRow();
            dr["BAND_DSC"] = "All Colors";
            dr["SIZE_CONSTRAINT_RID"] = sizeConstraintModelKey;
            dr["SGL_RID"] = attributeSetKey;
            dr["COLOR_CODE_RID"] = colorCodeKey;
            dr["SIZES_RID"] = Include.NoRID;
            dr["DIMENSIONS_RID"] = dimensionKey;
            dr["SIZE_CODE_RID"] = Include.NoRID;
            dr["SIZE_SEQ"] = sizeSequence;
            if (sizeConstraintValues.MinimumSet)
            {
                dr["SIZE_MIN"] = sizeConstraintValues.Minimum;
            }
            if (sizeConstraintValues.MaximumSet)
            {
                dr["SIZE_MAX"] = sizeConstraintValues.Maximum;
            }
            if (sizeConstraintValues.MultipleSet)
            {
                dr["SIZE_MULT"] = sizeConstraintValues.Multiple;
            }
            dr["ROW_TYPE_ID"] = eSizeMethodRowType.AllColorSizeDimension.GetHashCode();

            _sizeConstraints.Tables[C_TABLE_ALL_CLR_SZ_DIM].Rows.Add(dr);

        }

        private void AddAllColorSizeDimensionSizeRow(int sizeConstraintModelKey,
            int attributeSetKey,
            string attributeSetName,
            int colorCodeKey,
            int dimensionKey,
            int sizeKey,
            int sizeSequence,
            ROSizeConstraintValues sizeConstraintValues,
            ref string message)
        {
            DataRow dr;

            dr = _sizeConstraints.Tables[C_TABLE_ALL_CLR_SZ].NewRow();
            dr["BAND_DSC"] = "All Colors";
            dr["SIZE_CONSTRAINT_RID"] = sizeConstraintModelKey;
            dr["SGL_RID"] = attributeSetKey;
            dr["COLOR_CODE_RID"] = colorCodeKey;
            dr["SIZES_RID"] = Include.NoRID;
            dr["DIMENSIONS_RID"] = dimensionKey;
            dr["SIZE_CODE_RID"] = sizeKey;
            dr["SIZE_SEQ"] = sizeSequence;
            if (sizeConstraintValues.MinimumSet)
            {
                dr["SIZE_MIN"] = sizeConstraintValues.Minimum;
            }
            if (sizeConstraintValues.MaximumSet)
            {
                dr["SIZE_MAX"] = sizeConstraintValues.Maximum;
            }
            if (sizeConstraintValues.MultipleSet)
            {
                dr["SIZE_MULT"] = sizeConstraintValues.Multiple;
            }
            dr["ROW_TYPE_ID"] = eSizeMethodRowType.AllColorSize.GetHashCode();

            _sizeConstraints.Tables[C_TABLE_ALL_CLR_SZ].Rows.Add(dr);

        }

        private void AddColorRow(int sizeConstraintModelKey,
            int attributeSetKey,
            string attributeSetName,
            int colorCodeKey,
            ROSizeConstraintValues sizeConstraintValues,
            ref string message)
        {
            DataRow dr;

            dr = _sizeConstraints.Tables[C_TABLE_CLR].NewRow();
            dr["BAND_DSC"] = "Colors";
            dr["SIZE_CONSTRAINT_RID"] = sizeConstraintModelKey;
            dr["SGL_RID"] = attributeSetKey;
            dr["COLOR_CODE_RID"] = colorCodeKey;
            dr["SIZES_RID"] = Include.NoRID;
            dr["DIMENSIONS_RID"] = Include.NoRID;
            dr["SIZE_CODE_RID"] = Include.NoRID;
            if (sizeConstraintValues.MinimumSet)
            {
                dr["SIZE_MIN"] = sizeConstraintValues.Minimum;
            }
            if (sizeConstraintValues.MaximumSet)
            {
                dr["SIZE_MAX"] = sizeConstraintValues.Maximum;
            }
            if (sizeConstraintValues.MultipleSet)
            {
                dr["SIZE_MULT"] = sizeConstraintValues.Multiple;
            }
            dr["ROW_TYPE_ID"] = eSizeMethodRowType.Color.GetHashCode();

            _sizeConstraints.Tables[C_TABLE_CLR].Rows.Add(dr);

        }

        private void AddColorSizeDimensionRow(int sizeConstraintModelKey,
            int attributeSetKey,
            string attributeSetName,
            int colorCodeKey,
            int dimensionKey,
            int sizeSequence,
            ROSizeConstraintValues sizeConstraintValues,
            ref string message)
        {
            DataRow dr;

            dr = _sizeConstraints.Tables[C_TABLE_CLR_SZ_DIM].NewRow();
            dr["BAND_DSC"] = "All Colors";
            dr["SIZE_CONSTRAINT_RID"] = sizeConstraintModelKey;
            dr["SGL_RID"] = attributeSetKey;
            dr["COLOR_CODE_RID"] = colorCodeKey;
            dr["SIZES_RID"] = Include.NoRID;
            dr["DIMENSIONS_RID"] = dimensionKey;
            dr["SIZE_CODE_RID"] = Include.NoRID;
            dr["SIZE_SEQ"] = sizeSequence;
            if (sizeConstraintValues.MinimumSet)
            {
                dr["SIZE_MIN"] = sizeConstraintValues.Minimum;
            }
            if (sizeConstraintValues.MaximumSet)
            {
                dr["SIZE_MAX"] = sizeConstraintValues.Maximum;
            }
            if (sizeConstraintValues.MultipleSet)
            {
                dr["SIZE_MULT"] = sizeConstraintValues.Multiple;
            }
            dr["ROW_TYPE_ID"] = eSizeMethodRowType.ColorSizeDimension.GetHashCode();

            _sizeConstraints.Tables[C_TABLE_CLR_SZ_DIM].Rows.Add(dr);

        }

        private void AddColorSizeDimensionSizeRow(int sizeConstraintModelKey,
            int attributeSetKey,
            string attributeSetName,
            int colorCodeKey,
            int dimensionKey,
            int sizeKey,
            int sizeSequence,
            ROSizeConstraintValues sizeConstraintValues,
            ref string message)
        {
            DataRow dr;

            dr = _sizeConstraints.Tables[C_TABLE_CLR_SZ].NewRow();
            dr["BAND_DSC"] = "All Colors";
            dr["SIZE_CONSTRAINT_RID"] = sizeConstraintModelKey;
            dr["SGL_RID"] = attributeSetKey;
            dr["COLOR_CODE_RID"] = colorCodeKey;
            dr["SIZES_RID"] = Include.NoRID;
            dr["DIMENSIONS_RID"] = dimensionKey;
            dr["SIZE_CODE_RID"] = sizeKey;
            dr["SIZE_SEQ"] = sizeSequence;
            if (sizeConstraintValues.MinimumSet)
            {
                dr["SIZE_MIN"] = sizeConstraintValues.Minimum;
            }
            if (sizeConstraintValues.MaximumSet)
            {
                dr["SIZE_MAX"] = sizeConstraintValues.Maximum;
            }
            if (sizeConstraintValues.MultipleSet)
            {
                dr["SIZE_MULT"] = sizeConstraintValues.Multiple;
            }
            dr["ROW_TYPE_ID"] = eSizeMethodRowType.ColorSize.GetHashCode();

            _sizeConstraints.Tables[C_TABLE_CLR_SZ].Rows.Add(dr);

        }

        private void FillCollections(int sizeConstraintModelKey, int attributeKey, int sizeGroupKey, int sizeCurveGroupKey)
        {
            int setIdx;
            int allColorIdx;
            int colorIdx;
            int allColorSizeIdx;
            int colorSizeIdx;
            int colorSizeDimIdx;
            int allColorSizeDimIdx;

            if (_setsCollection != null)
            {
                _setsCollection.Clear();
            }

            _setsCollection = new CollectionSets();

            if (_sizeConstraints.Tables["SetLevel"].Rows.Count > 0)
            {
                //FOR EACH SET LEVEL
                foreach (DataRow drSet in _sizeConstraints.Tables["SetLevel"].Rows)
                {
                    setIdx = _setsCollection.Add(new ItemSet(sizeConstraintModelKey, drSet));

                    //ALL COLOR
                    foreach (DataRow drAllColor in drSet.GetChildRows("SetAllColor"))
                    {
                        allColorIdx = _setsCollection[setIdx].collectionAllColors.Add(new ItemAllColor(sizeConstraintModelKey, drAllColor));

                        //SIZE DIMENSION
                        if (_sizeConstraints.Relations.Contains("AllColorSizeDimension"))
                        {
                            foreach (DataRow drACSizeDim in drAllColor.GetChildRows("AllColorSizeDimension"))
                            {
                                allColorSizeDimIdx = _setsCollection[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions.Add(new ItemSizeDimension(sizeConstraintModelKey, drACSizeDim));

                                //SIZE
                                foreach (DataRow drACSize in drACSizeDim.GetChildRows("AllColorSize"))
                                {
                                    allColorSizeIdx = _setsCollection[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions[allColorSizeDimIdx].collectionSizes.Add(new ItemSize(sizeConstraintModelKey, drACSize));
                                }
                            }
                        }
                    }

                    //COLOR
                    foreach (DataRow drColor in drSet.GetChildRows("SetColor"))
                    {
                        colorIdx = _setsCollection[setIdx].collectionColors.Add(new ItemColor(sizeConstraintModelKey, drColor));

                        //SIZE DIMENSION
                        if (_sizeConstraints.Relations.Contains("ColorSizeDimension"))
                        {
                            foreach (DataRow drCSizeDim in drColor.GetChildRows("ColorSizeDimension"))
                            {
                                colorSizeDimIdx = _setsCollection[setIdx].collectionColors[colorIdx].collectionSizeDimensions.Add(new ItemSizeDimension(sizeConstraintModelKey, drCSizeDim));

                                //SIZE
                                foreach (DataRow drCSize in drCSizeDim.GetChildRows("ColorSize"))
                                {
                                    colorSizeIdx = _setsCollection[setIdx].collectionColors[colorIdx].collectionSizeDimensions[colorSizeDimIdx].collectionSizes.Add(new ItemSize(sizeConstraintModelKey, drCSize));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DetermineSizeSequence(int attributeKey, int sizeGroupKey, int sizeCurveGroupKey)
        {
            try
            {
                SizeGroupProfile sgp = null;
                SizeCurveGroupProfile scgp;

                if (sizeGroupKey != Include.NoRID)
                {
                    sgp = new SizeGroupProfile(sizeGroupKey);
                }
                else if (sizeCurveGroupKey != Include.NoRID)
                {
                    scgp = new SizeCurveGroupProfile(sizeCurveGroupKey);
                    sgp = new SizeGroupProfile(scgp.DefinedSizeGroupRID);
                }

                if (sgp != null)
                {
                    if (_sizeConstraints.Tables.Contains(C_ALL_CLR_SZ_DIM))
                    {
                        SetSizeSequence(sgp, _sizeConstraints.Tables[C_ALL_CLR_SZ_DIM]);
                    }
                    if (_sizeConstraints.Tables.Contains(C_ALL_CLR_SZ))
                    {
                        SetSizeSequence(sgp, _sizeConstraints.Tables[C_ALL_CLR_SZ]);
                    }
                    if (_sizeConstraints.Tables.Contains(C_CLR_SZ_DIM))
                    {
                        SetSizeSequence(sgp, _sizeConstraints.Tables[C_CLR_SZ_DIM]);
                    }
                    if (_sizeConstraints.Tables.Contains(C_CLR_SZ))
                    {
                        SetSizeSequence(sgp, _sizeConstraints.Tables[C_CLR_SZ]);
                    }
                    _sizeConstraints.AcceptChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add sequence column and determine value; then sort
        /// </summary>
        private void SetSizeSequence(SizeGroupProfile aSizeGroupProfile, DataTable dtSizeCollection)
        {
            if (dtSizeCollection != null)
            {
                foreach (DataRow aRow in dtSizeCollection.Rows)
                {
                    int sizeCodeRid = Convert.ToInt32(aRow["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
                    int dimensionsRid = Convert.ToInt32(aRow["DIMENSIONS_RID"], CultureInfo.CurrentUICulture);
                    if (sizeCodeRid != Include.NoRID)
                    {
                        aRow["SIZE_SEQ"] = GetSizeCodeSequence(aSizeGroupProfile, sizeCodeRid);
                    }
                    else if (dimensionsRid != Include.NoRID)
                    {
                        aRow["SIZE_SEQ"] = GetDimensionSequence(aSizeGroupProfile, dimensionsRid);
                    }
                }
                dtSizeCollection.DefaultView.Sort = "SIZE_SEQ";
            }
        }

        private int GetSizeCodeSequence(SizeGroupProfile aSizeGroupProfile, int sizeCodeRid)
        {
            int seq = int.MaxValue;
            if (aSizeGroupProfile.SizeCodeList.Contains(sizeCodeRid))
            {
                SizeCodeProfile scp = (SizeCodeProfile)aSizeGroupProfile.SizeCodeList.FindKey(sizeCodeRid);
                seq = scp.PrimarySequence;
            }
            return seq;
        }

        private int GetDimensionSequence(SizeGroupProfile aSizeGroupProfile, int dimensionRid)
        {
            int seq = int.MaxValue;
            foreach (SizeCodeProfile scp in aSizeGroupProfile.SizeCodeList)
            {
                if (scp.SizeCodeSecondaryRID == dimensionRid)
                {
                    seq = scp.SecondarySequence;
                    break;
                }
            }
            return seq;
        }

        private bool BuildConstraints(ROModelSizeConstraintProperties modelProperties, int sizeGroupKey, int sizeCurveGroupKey, ref string message)
        {
            ROSizeConstraints sizeConstraints;
            ROSizeConstraintAttributeSet sizeConstraintAttributeSet;
            ROSizeConstraintColor sizeConstraintColor;
            ROSizeConstraintDimension sizeConstraintDimension;
            ROSizeConstraintSize sizeConstraintSize;
            int sizeOrderKey, attributeSetKey, colorKey, dimensionsKey, sizeKey;
            int? minimum, maximum, multiple;

            eGetSizes getSizesUsing = eGetSizes.SizeGroupRID;
            eGetDimensions getDimensionsUsing = eGetDimensions.SizeGroupRID;

            if (sizeGroupKey != Include.NoRID)
            {
                sizeOrderKey = sizeGroupKey;
                getSizesUsing = eGetSizes.SizeGroupRID;
                getDimensionsUsing = eGetDimensions.SizeGroupRID;
            }
            else
            {
                sizeOrderKey = sizeCurveGroupKey;
                getSizesUsing = eGetSizes.SizeCurveGroupRID;
                getDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
            }
            //PROCESS SETS AND ALL DESCENDANTS

            KeyValuePair<int, string> selectedAttributeSet;
            foreach (DataRow drSet in _sizeConstraints.Tables[C_TABLE_SET].Rows)
            {
                attributeSetKey = Convert.ToInt32(drSet["SGL_RID"]);

                if (modelProperties.AttributeSet.Key != attributeSetKey)
                {
                    continue;
                }

                GetConstraintValues(dr: drSet, minimum: out minimum, maximum: out maximum, multiple: out multiple);
                sizeConstraints = new ROSizeConstraints(minimum: minimum, maximum: maximum, multiple: multiple);

                DataView allColorView = new DataView(_sizeConstraints.Tables[C_TABLE_ALL_CLR]);
                allColorView.RowFilter = "SGL_RID = " + attributeSetKey.ToString();
                //PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
                foreach (DataRowView drvAllColor in allColorView)
                {
                    colorKey = Convert.ToInt32(drvAllColor["COLOR_CODE_RID"]);
                    GetConstraintValues(drv: drvAllColor, minimum: out minimum, maximum: out maximum, multiple: out multiple);
                    sizeConstraintColor = new ROSizeConstraintColor(color: GetName.GetColor(colorCodeRID: colorKey, SAB: SAB),
                        minimum: minimum,
                        maximum: maximum,
                        multiple: multiple
                        );

                    DataView allColorSizeDimensionView = new DataView(_sizeConstraints.Tables[C_TABLE_ALL_CLR_SZ_DIM],
                                                       "SGL_RID = " + attributeSetKey.ToString() + " and COLOR_CODE_RID = " + colorKey.ToString(),
                                                       "SIZE_SEQ",
                                                       DataViewRowState.CurrentRows);
                    foreach (DataRowView drvAllColorSizeDimension in allColorSizeDimensionView)
                    {
                        dimensionsKey = Convert.ToInt32(drvAllColorSizeDimension["DIMENSIONS_RID"]);
                        GetConstraintValues(drv: drvAllColorSizeDimension, minimum: out minimum, maximum: out maximum, multiple: out multiple);
                        sizeConstraintDimension = new ROSizeConstraintDimension(dimension: GetName.GetDimension(dimensionRID: dimensionsKey,
                                                                                                                sizeGroupRID: sizeGroupKey, 
                                                                                                                sizeCurveGroupRID: sizeCurveGroupKey,
                                                                                                                getSizesUsing: getSizesUsing, 
                                                                                                                getDimensionsUsing: getDimensionsUsing,
                                                                                                                SAB: SAB),
                            minimum: minimum,
                            maximum: maximum,
                            multiple: multiple
                            );

                        DataView allColorSizeView = new DataView(_sizeConstraints.Tables[C_TABLE_ALL_CLR_SZ],
                                                       "SGL_RID = " + attributeSetKey.ToString() + " and DIMENSIONS_RID = " + dimensionsKey.ToString(),
                                                       "SIZE_SEQ",
                                                       DataViewRowState.CurrentRows);
                        foreach (DataRowView drvAllColorSize in allColorSizeView)
                        {
                            sizeKey = Convert.ToInt32(drvAllColorSize["SIZE_CODE_RID"]);
                            GetConstraintValues(drv: drvAllColorSize, minimum: out minimum, maximum: out maximum, multiple: out multiple);
                            sizeConstraintSize = new ROSizeConstraintSize(size: GetName.GetSize(sizesRID: sizeKey,
                                                                                                SAB: SAB),
                                minimum: minimum,
                                maximum: maximum,
                                multiple: multiple
                                );

                            sizeConstraintDimension.SizeConstraintSize.Add(sizeConstraintSize);
                        }

                        sizeConstraintColor.SizeConstraintDimension.Add(sizeConstraintDimension);
                    }

                    sizeConstraints.SizeConstraintColor.Add(sizeConstraintColor);
                }

                //PROCESS COLOR LEVEL AND ALL DESCENDANTS
                DataView colorView = new DataView(_sizeConstraints.Tables[C_TABLE_CLR]);
                colorView.RowFilter = "SGL_RID = " + attributeSetKey.ToString();
                foreach (DataRowView drvColor in colorView)
                {
                    colorKey = Convert.ToInt32(drvColor["COLOR_CODE_RID"]);
                    GetConstraintValues(drv: drvColor, minimum: out minimum, maximum: out maximum, multiple: out multiple);
                    sizeConstraintColor = new ROSizeConstraintColor(color: GetName.GetColor(colorCodeRID: colorKey, SAB: SAB),
                        minimum: minimum,
                        maximum: maximum,
                        multiple: multiple
                        );

                    DataView colorSizeDimensionView = new DataView(_sizeConstraints.Tables[C_TABLE_CLR_SZ_DIM],
                                                       "SGL_RID = " + attributeSetKey.ToString() + " and COLOR_CODE_RID = " + colorKey.ToString(),
                                                       "SIZE_SEQ",
                                                       DataViewRowState.CurrentRows);
                    foreach (DataRowView drvColorSizeDimension in colorSizeDimensionView)
                    {
                        dimensionsKey = Convert.ToInt32(drvColorSizeDimension["DIMENSIONS_RID"]);
                        GetConstraintValues(drv: drvColorSizeDimension, minimum: out minimum, maximum: out maximum, multiple: out multiple);
                        sizeConstraintDimension = new ROSizeConstraintDimension(dimension: GetName.GetDimension(dimensionRID: dimensionsKey,
                                                                                                                sizeGroupRID: sizeGroupKey,
                                                                                                                sizeCurveGroupRID: sizeCurveGroupKey,
                                                                                                                getSizesUsing: getSizesUsing,
                                                                                                                getDimensionsUsing: getDimensionsUsing,
                                                                                                                SAB: SAB),
                            minimum: minimum,
                            maximum: maximum,
                            multiple: multiple
                            );

                        DataView colorSizeView = new DataView(_sizeConstraints.Tables[C_TABLE_CLR_SZ],
                                                       "SGL_RID = " + attributeSetKey.ToString() + " and DIMENSIONS_RID = " + dimensionsKey.ToString(),
                                                       "SIZE_SEQ",
                                                       DataViewRowState.CurrentRows);
                        foreach (DataRowView drvColorSize in colorSizeView)
                        {
                            sizeKey = Convert.ToInt32(drvColorSize["SIZE_CODE_RID"]);
                            GetConstraintValues(drv: drvColorSize, minimum: out minimum, maximum: out maximum, multiple: out multiple);
                            sizeConstraintSize = new ROSizeConstraintSize(size: GetName.GetSize(sizesRID: sizeKey,
                                                                                                SAB: SAB),
                                             minimum: minimum,
                                             maximum: maximum,
                                             multiple: multiple
                                             );

                            sizeConstraintDimension.SizeConstraintSize.Add(sizeConstraintSize);
                        }

                        sizeConstraintColor.SizeConstraintDimension.Add(sizeConstraintDimension);

                    }

                    sizeConstraints.SizeConstraintColor.Add(sizeConstraintColor);
                }

                
                if (attributeSetKey == Include.NoRID)
                {
                    selectedAttributeSet = new KeyValuePair<int, string>(attributeSetKey, modelProperties.DefaultLabel);
                }
                else
                {
                    selectedAttributeSet = GetName.GetAttributeSetName(key: attributeSetKey);
                }
                sizeConstraintAttributeSet = new ROSizeConstraintAttributeSet(attributeSet: selectedAttributeSet, sizeConstraints: sizeConstraints);
                modelProperties.SizeConstraintAttributeSet = sizeConstraintAttributeSet;
            }

            // create empty instance for set when data is not found
            if (modelProperties.SizeConstraintAttributeSet == null)
            {
                selectedAttributeSet = GetName.GetAttributeSetName(key: modelProperties.AttributeSet.Key);
                sizeConstraints = new ROSizeConstraints(minimum: null, maximum: null, multiple: null);
                sizeConstraintColor = new ROSizeConstraintColor(color: GetName.GetColor(colorCodeRID: Include.UndefinedColor, SAB: SAB),
                        minimum: null,
                        maximum: null,
                        multiple: null
                        );

                sizeConstraints.SizeConstraintColor.Add(sizeConstraintColor);
                sizeConstraintAttributeSet = new ROSizeConstraintAttributeSet(attributeSet: selectedAttributeSet, sizeConstraints: sizeConstraints);
                modelProperties.SizeConstraintAttributeSet = sizeConstraintAttributeSet;
            }

            FillDimensionSizeList(
                sizeDimensionSizes: modelProperties.SizeConstraintDimensions, 
                Key: sizeOrderKey, 
                getDimensions: getDimensionsUsing, 
                getSizes: getSizesUsing,
                includeDefaultDimension: false,
                includeDefaultSize: false,
                useSizeCodeKey: true
                );

            return true;
        }

        private void GetConstraintValues(DataRow dr, out int? minimum, out int? maximum, out int? multiple)
        {
            minimum = (dr["SIZE_MIN"].ToString().Trim() != string.Empty) ? Convert.ToInt32(dr["SIZE_MIN"], CultureInfo.CurrentUICulture) : (int?)null;
            maximum = (dr["SIZE_MAX"].ToString().Trim() != string.Empty) ? Convert.ToInt32(dr["SIZE_MAX"], CultureInfo.CurrentUICulture) : (int?)null;
            multiple = (dr["SIZE_MULT"].ToString().Trim() != string.Empty) ? Convert.ToInt32(dr["SIZE_MULT"], CultureInfo.CurrentUICulture) : (int?)null;
        }

        private void GetConstraintValues(DataRowView drv, out int? minimum, out int? maximum, out int? multiple)
        {
            minimum = (drv["SIZE_MIN"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drv["SIZE_MIN"], CultureInfo.CurrentUICulture) : (int?)null;
            maximum = (drv["SIZE_MAX"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drv["SIZE_MAX"], CultureInfo.CurrentUICulture) : (int?)null;
            multiple = (drv["SIZE_MULT"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drv["SIZE_MULT"], CultureInfo.CurrentUICulture) : (int?)null;
        }

        override public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            SizeModelData sizeModelData = new SizeModelData();
            ROModelSizeConstraintProperties sizeConstraintProperties = (ROModelSizeConstraintProperties)modelsProperties;
            int sizeConstraintModelRid = modelsProperties.Model.Key;

            if (sizeConstraintProperties.Attribute.Key != _sizeConstraintProfile.StoreGroupRid
                || sizeConstraintProperties.SizeGroup.Key != _sizeConstraintProfile.SizeGroupRid
                || sizeConstraintProperties.SizeCurveGroup.Key != _sizeConstraintProfile.SizeCurveGroupRid)
            {
                _constraintRollback = true;
                _createConstraintData = true;
                ClearConstraints(key: sizeConstraintProperties.Model.Key);
                _sizeConstraintProfile.StoreGroupRid = sizeConstraintProperties.Attribute.Key;
                if (sizeConstraintProperties.SizeGroupIsSet)
                {
                    _sizeConstraintProfile.SizeGroupRid = sizeConstraintProperties.SizeGroup.Key;
                    _sizeConstraintProfile.SizeCurveGroupRid = Include.NoRID;
                }
                else if (sizeConstraintProperties.SizeCurveGroupIsSet)
                {
                    _sizeConstraintProfile.SizeCurveGroupRid = sizeConstraintProperties.SizeCurveGroup.Key;
                    _sizeConstraintProfile.SizeGroupRid = Include.NoRID;
                }
                else
                {
                    _sizeConstraintProfile.SizeGroupRid = Include.NoRID;
                    _sizeConstraintProfile.SizeCurveGroupRid = Include.NoRID;
                }
            }
            else if (BuildConstraintDataSet(sizeConstraintProperties: sizeConstraintProperties, message: ref message))
            {
                DetermineSizeSequence(attributeKey: sizeConstraintProperties.Attribute.Key, sizeGroupKey: sizeConstraintProperties.SizeGroup.Key, sizeCurveGroupKey: sizeConstraintProperties.SizeCurveGroup.Key);
                
                if (!applyOnly)
                {
                    TransactionData td = new TransactionData();

                    try
                    {
                        td.OpenUpdateConnection();
                        sizeConstraintModelRid = sizeModelData.SizeConstraintModel_Update_Insert(sizeConstraintProperties.Model.Key,
                            sizeConstraintProperties.Model.Value, sizeConstraintProperties.SizeGroup.Key, sizeConstraintProperties.SizeCurveGroup.Key, sizeConstraintProperties.Attribute.Key, td);
                        FillCollections(sizeConstraintModelKey: sizeConstraintModelRid, attributeKey: sizeConstraintProperties.Attribute.Key, sizeGroupKey: sizeConstraintProperties.SizeGroup.Key, sizeCurveGroupKey: sizeConstraintProperties.SizeCurveGroup.Key);
                        InsertUpdateSizeConstraints(sizeConstraintModekKey: sizeConstraintModelRid, td: td, sizeModelData: sizeModelData);
                        td.CommitData();
                    }
                    finally
                    {
                        td.CloseUpdateConnection();
                    }
                    _constraintRollback = false;
                }
                else
                {
                    FillCollections(sizeConstraintModelKey: sizeConstraintProperties.Model.Key, attributeKey: sizeConstraintProperties.Attribute.Key, sizeGroupKey: sizeConstraintProperties.SizeGroup.Key, sizeCurveGroupKey: sizeConstraintProperties.SizeCurveGroup.Key);
                }
            }
            else
            {
                successful = false;
            }

            _sizeConstraintProfile.Key = sizeConstraintModelRid;
            _sizeConstraintProfile.ModelID = sizeConstraintProperties.Model.Value;
            _sizeConstraintProfile.SizeConstraintName = sizeConstraintProperties.Model.Value;

            return _sizeConstraintProfile;
        }

        private void ClearConstraints(int key)
        {
            TransactionData td = new TransactionData();
            SizeModelData sizeModelData = new SizeModelData();

            try
            {
                td.OpenUpdateConnection();
                DeleteSizeConstraintChildren(td: td, sizeModelData: sizeModelData, key: key);
                td.CommitData();
            }
            catch (DatabaseForeignKeyViolation)
            {
                td.Rollback();
                return;
            }
            catch (Exception)
            {
                td.Rollback();
            }
            finally
            {
                td.CloseUpdateConnection();
            }
        }

        private bool InsertUpdateSizeConstraints(int sizeConstraintModekKey, TransactionData td, SizeModelData sizeModelData)
        {
            bool Successfull = true;
            MaintainSizeConstraints maint = new MaintainSizeConstraints(sizeModelData);

            try
            {
                Successfull = maint.insertUpdateCollection(sizeConstraintModekKey, td, _setsCollection);
            }
            catch
            {
                throw;
            }

            return Successfull;

        }

        override public bool ModelDelete(int key, ref string message)
        {
            message = null;
            TransactionData td = new TransactionData();
            SizeModelData sizeModelData = new SizeModelData();

            try
            {
                td.OpenUpdateConnection();

                DeleteSizeConstraintChildren(td: td, sizeModelData: sizeModelData, key: key);
                sizeModelData.SizeConstraintModel_Delete(key, td);
                td.CommitData();
            }
            catch (DatabaseForeignKeyViolation)
            {
                td.Rollback();
                return false;
            }
            catch (Exception)
            {
                td.Rollback();
                return false;
            }
            finally
            {
                td.CloseUpdateConnection();
            }

            return true;
        }

        private bool DeleteSizeConstraintChildren(TransactionData td, SizeModelData sizeModelData, int key)
        {
            MaintainSizeConstraints maint = new MaintainSizeConstraints(sizeModelData);
            try
            {
                return maint.deleteSizeConstraintChildren(key, td);
            }
            catch
            {
                throw;
            }
        }

        override public bool ModelNameExists(string name)
        {
            ModelProfile checkExists = SAB.HierarchyServerSession.GetModelData(aModelType: eModelType.SizeConstraints, modelID: name);

            return checkExists.Key != Include.NoRID;
        }

        override public ROModelParms GetModelParms(ROModelPropertiesParms parms, eModelType modelType, int key, bool readOnly = false)
        {
            ROModelSizeConstraintProperties sizeConstraintProperties = (ROModelSizeConstraintProperties)parms.ROModelProperties;

            ROModelParms modelParms = new ROSizeConstraintModelParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.GetModel,
                ROInstanceID: parms.ROInstanceID,
                modelType: modelType,
                key: key,
                readOnly: readOnly,
                attributeKey: sizeConstraintProperties.Attribute.Key,
                attributeSetKey: sizeConstraintProperties.AttributeSet.Key,
                sizeGroupKey: sizeConstraintProperties.SizeGroup.Key,
                sizeCurveGroupKey: sizeConstraintProperties.SizeCurveGroup.Key
                );

            return modelParms;
        }
    }
}
