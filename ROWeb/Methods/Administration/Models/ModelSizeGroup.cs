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
    public class ModelSizeGroup : ModelBase
    {
        //=======
        // FIELDS
        //=======
        private SizeGroupList _sizeGroupList = null;
        private SizeGroupProfile _sizeGroupProfile;
        private ROModelSizeGroupProperties _modelProperties = null;
        private ArrayList _productCategories = null;
        private string _noSizeDimensionLbl;
        private Dictionary<string, SizeCodeProfile> sizeCodes;
        private bool _buildModelData = true;

        //=============
        // CONSTRUCTORS
        //=============
        public ModelSizeGroup(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.SizeGroup)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeGroups);
            _noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
            sizeCodes = new Dictionary<string, SizeCodeProfile>();
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
            List<KeyValuePair<int, string>> keyValueList = new List<KeyValuePair<int, string>>();

            GetSizeModels();
            //keyValueList.Add(new KeyValuePair<int, string>(-1, MIDText.GetTextOnly((int)eMIDTextCode.msg_NewSizeGroup)));
            foreach (SizeGroupProfile sgp in _sizeGroupList.ArrayList)
            {
                keyValueList.Add(new KeyValuePair<int, string>(sgp.Key, sgp.SizeGroupName.Trim()));
            }

            return keyValueList;
        }

        private void GetSizeModels()
        {
            _sizeGroupList = new SizeGroupList(eProfileType.SizeGroup);
            _sizeGroupList.LoadAll(false);
        }

        private void GetProductCategories()
        {
            _productCategories = SAB.HierarchyServerSession.GetSizeProductCategoryList();
            _productCategories.Sort();
        }

        private string GetProductCategory()
        {
            if (_productCategories == null)
            {
                GetProductCategories();
            }

            SizeCodeList scl = _sizeGroupProfile.SizeCodeList;

            foreach (SizeCodeProfile scp in scl.ArrayList)
            {
                if (!string.IsNullOrEmpty(scp.SizeCodeProductCategory))
                {
                    if (_productCategories.Contains(scp.SizeCodeProductCategory))
                    {
                        return scp.SizeCodeProductCategory;
                    }
                }
            }

            return string.Empty;
        }

        private SizeCodeProfile GetSizeCodeProfile(string ID)
        {
            SizeCodeProfile sizeCodeProfile = null;

            if (!sizeCodes.TryGetValue(ID, out sizeCodeProfile))
            {
                sizeCodeProfile = SAB.HierarchyServerSession.GetSizeCodeProfile(ID);
                sizeCodes.Add(ID, sizeCodeProfile);
            }

            return sizeCodeProfile;
        }

        override public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false)
        {
            if (_buildModelData
                || _modelProperties == null)
            {
                // populate modelProperties using Windows\SizeGroup.cs as a reference
                _sizeGroupProfile = (SizeGroupProfile)modelProfile;

                KeyValuePair<int, string> model = new KeyValuePair<int, string>(key: _sizeGroupProfile.Key, value: _sizeGroupProfile.SizeGroupName);
                _modelProperties = new ROModelSizeGroupProperties(model: model,
                    productCategory: GetProductCategory(),
                    description: _sizeGroupProfile.SizeGroupDescription
                    );

                AddSizeMatrix(ref message);

                // add product category list for dropdown
                _modelProperties.ProductCategories.Clear();
                foreach (string productCategory in _productCategories)
                {
                    _modelProperties.ProductCategories.Add(productCategory);
                }
            }

            return _modelProperties;
        }

        private void AddSizeMatrix(ref string message)
        {
            // load existing group
            SizeCodeList scl = _sizeGroupProfile.SizeCodeList;

            System.Collections.SortedList sizeAL = new SortedList();
            System.Collections.SortedList widthAL = new SortedList();
            System.Collections.Hashtable bothHash = new Hashtable();
            foreach (SizeCodeProfile scp in scl.ArrayList)
            {
                if (scp.Key == -1)
                {
                    throw new MIDException(eErrorLevel.severe,
                                           (int)eMIDTextCode.msg_CantRetrieveSizeCode,
                                           MIDText.GetText(eMIDTextCode.msg_CantRetrieveSizeCode));
                }
 
                if (!sizeAL.Contains(scp.PrimarySequence))
                    sizeAL.Add(scp.PrimarySequence, scp.SizeCodePrimary);
                if (!widthAL.Contains(scp.SecondarySequence))
                    widthAL.Add(scp.SecondarySequence, scp.SizeCodeSecondary);
                if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondary))
                    bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondary, scp);
                else
                    // this should not happen? 
                    throw new MIDException(eErrorLevel.severe,
                                           (int)eMIDTextCode.msg_LabelsNotUnique,
                                           MIDText.GetText(eMIDTextCode.msg_LabelsNotUnique));
            }

            foreach (DictionaryEntry size in sizeAL)
            {
                _modelProperties.Size.Add(new KeyValuePair<int, string>(Convert.ToInt32(size.Key), Convert.ToString(size.Value)));
            }

            _modelProperties.DefineSizeMatrix(numberOfRows: widthAL.Count, numberOfColumns: sizeAL.Count);
            int row = 0;
            int col = 0;
            foreach (DictionaryEntry width in widthAL)
            {
                _modelProperties.Width.Add(new KeyValuePair<int, string>(Convert.ToInt32(width.Key), Convert.ToString(width.Value)));
                col = 0;
                foreach (DictionaryEntry size in sizeAL)
                {
                    if (bothHash.Contains(size.Value + "~" + width.Value))
                    {
                        SizeCodeProfile scp = (SizeCodeProfile)bothHash[size.Value + "~" + width.Value];
                        _modelProperties.SizeMatrix[row][col] = new KeyValuePair<int, string>(scp.Key, scp.SizeCodeID);
                    }
                    else
                    {
                        _modelProperties.SizeMatrix[row][col] = default(KeyValuePair<int, string>);
                    }
                    col++;
                }
                row++;
            }
        }

        override public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;

            if (SetSizeGroupProfile(modelsProperties: modelsProperties, message: ref message))
            {
                if (!applyOnly)
                {
                    _sizeGroupProfile.WriteSizeGroup();
                }
            }
            else
            {
                successful = false;
            }

            return _sizeGroupProfile;
        }

        private bool SetSizeGroupProfile(ROModelProperties modelsProperties, ref string message)
        {
            ROModelSizeGroupProperties sizeGroupProperties = (ROModelSizeGroupProperties)modelsProperties;

            if (!VerifySizeGroup(sizeGroupProperties: sizeGroupProperties, message: ref message))
            {
                _buildModelData = false;
                _modelProperties = sizeGroupProperties;
                return false;
            }

            _buildModelData = true;
            _sizeGroupProfile.Key = sizeGroupProperties.Model.Key;
            _sizeGroupProfile.SizeGroupName = sizeGroupProperties.Model.Value;
            _sizeGroupProfile.SizeGroupDescription = sizeGroupProperties.Description;
            _sizeGroupProfile.SizeCodeList.Clear();

            for (int row = 0; row < sizeGroupProperties.SizeMatrix.GetLength(0); row++)
            {
                for (int col = 0; col < sizeGroupProperties.SizeMatrix[row].GetLength(0); col++)
                {
                    if (sizeGroupProperties.SizeMatrix[row][col].Key != Include.Undefined)
                    {
                        string sizeCodeStr = sizeGroupProperties.SizeMatrix[row][col].Value;

                        if (sizeCodeStr != "")
                        {
                            SizeCodeProfile scp = GetSizeCodeProfile(ID: sizeCodeStr);
                            scp.PrimarySequence = col + 1;
                            scp.SecondarySequence = row + 1;
                            _sizeGroupProfile.SizeCodeList.Add(scp);
                        }
                    }
                }
            }

            if (_sizeGroupProfile.SizeCodeList.Count == 0)
            {
                message = MIDText.GetText(eMIDTextCode.msg_MustHaveAtLeaseOneSizeCode);
                return false;
            }

            return true;
        }

        private bool VerifySizeGroup(ROModelSizeGroupProperties sizeGroupProperties, ref string message)
        {
            bool verified = true;
            sizeGroupProperties.SizeSelection.Clear();

            if (string.IsNullOrEmpty(sizeGroupProperties.ProductCategory))
            {
                message = MIDText.GetText(eMIDTextCode.msg_SelectProductCategory);
                return false;
            }

            sizeGroupProperties.MessageRow = Include.Undefined;
            sizeGroupProperties.MessageColumn = Include.Undefined;
            int sizeEntries = sizeGroupProperties.Size.Count;
            for (int row = 0; row < sizeGroupProperties.SizeMatrix.GetLength(0); row++)
            {
                if (sizeGroupProperties.SizeMatrix[row].GetLength(0) > sizeEntries)
                {
                    sizeEntries = sizeGroupProperties.SizeMatrix[row].GetLength(0);
                }
            }
            bool[] colLocks = new bool[sizeEntries];
            int widthEntries = sizeGroupProperties.Width.Count;
            if (sizeGroupProperties.SizeMatrix.GetLength(0) > widthEntries)
            {
                widthEntries = sizeGroupProperties.SizeMatrix.GetLength(0);
            }
            bool[] rowLocks = new bool[widthEntries];

            for (int col = 0; col < sizeEntries; col++)
            {
                string sizeStr = string.Empty;
                if (sizeGroupProperties.Size.Count > col)
                {
                    sizeStr = sizeGroupProperties.Size[col].Value;
                }

                for (int row = 0; row < widthEntries; row++)
                {
                    string widthStr = string.Empty;
                    if (sizeGroupProperties.Width.Count > row)
                    {
                        widthStr = sizeGroupProperties.Width[row].Value;
                    }

                    if (sizeGroupProperties.SizeMatrix[row][col].Key != Include.Undefined)
                    {
                        string sizeCodeStr = sizeGroupProperties.SizeMatrix[row][col].Value;
                        if (sizeCodeStr == null)
                        {
                            sizeCodeStr = string.Empty;
                        }

                        if (sizeCodeStr != "")
                        {
                            SizeCodeProfile scp = GetSizeCodeProfile(ID: sizeCodeStr);
                            if (scp.Key == -1)
                            {
                                if (message == null)
                                {
                                    message = MIDText.GetText(eMIDTextCode.msg_NoSizeCodeFound);
                                    message = message.Replace("{0}", sizeStr);
                                    message = message.Replace("{1}", widthStr);
                                    message = message.Replace("{2}", sizeGroupProperties.ProductCategory);
                                    sizeGroupProperties.MessageRow = row;
                                    sizeGroupProperties.MessageColumn = col;
                                }
                                verified = false;
                            }
                            else
                            {
                                // get the size code based on the product category, size and width
                                eSearchContent sizeSearchType;
                                if (colLocks[col] || sizeGroupProperties.VerifyCriteria == eSearchContent.WholeField)
                                    sizeSearchType = eSearchContent.WholeField;
                                else
                                    sizeSearchType = sizeGroupProperties.VerifyCriteria;

                                eSearchContent widthSearchType;
                                if (rowLocks[row] || sizeGroupProperties.VerifyCriteria == eSearchContent.WholeField)
                                    widthSearchType = eSearchContent.WholeField;
                                else
                                    widthSearchType = sizeGroupProperties.VerifyCriteria;

                                if (widthStr == string.Empty)
                                {
                                    if (scp.SizeCodeSecondary != null)
                                    {
                                        widthStr = scp.SizeCodeSecondary;
                                    }
                                    else
                                    {
                                        widthStr = _noSizeDimensionLbl;
                                    }
                                }

                                SizeCodeList scl = SAB.HierarchyServerSession.GetSizeCodeList(
                                    sizeGroupProperties.ProductCategory, eSearchContent.WholeField,
                                    sizeStr, sizeSearchType,
                                    widthStr, widthSearchType);

                                if (scl.FindKey(scp.Key) == null
                                    || scp.SizeCodeProductCategory != sizeGroupProperties.ProductCategory)
                                {
                                    if (message == null
                                        && !sizeGroupProperties.SizeSelectionIsSet)
                                    {
                                        message = MIDText.GetText(eMIDTextCode.msg_CorrectThisCode);
                                        message = message.Replace("{0}", scp.SizeCodeID);
                                        message = message.Replace("{1}", scp.SizeCodePrimary);
                                        message = message.Replace("{2}", scp.SizeCodeSecondary);
                                        message = message.Replace("{3}", scp.SizeCodeProductCategory);
                                        foreach (SizeCodeProfile sizeCode in scl)
                                        {
                                            string listStr = sizeCode.SizeCodeID;
                                            if (sizeCode.SizeCodePrimary != null && sizeCode.SizeCodePrimary.Trim().Length != 0)
                                            {
                                                listStr += "   -   " + sizeCode.SizeCodePrimary;
                                            }
                                            if (sizeCode.SizeCodeSecondary != null && sizeCode.SizeCodeSecondary.Trim().Length != 0)
                                            {
                                                listStr += "   -   " + sizeCode.SizeCodeSecondary;
                                            }
                                            sizeGroupProperties.SizeSelection.Add(new KeyValuePair<int, string>(sizeCode.Key, listStr));
                                        }
                                        sizeGroupProperties.MessageRow = row;
                                        sizeGroupProperties.MessageColumn = col;
                                    }
                                    verified = false;
                                }

                                if (col > sizeGroupProperties.Size.Count - 1)
                                {
                                    sizeGroupProperties.Size.Add(new KeyValuePair<int, string>(col, scp.SizeCodePrimary));
                                }
                                sizeStr = scp.SizeCodePrimary;
                                colLocks[col] = true;

                                if (row > sizeGroupProperties.Width.Count - 1)
                                {
                                    sizeGroupProperties.Width.Add(new KeyValuePair<int, string>(row, scp.SizeCodeSecondary));
                                }
                                widthStr = scp.SizeCodeSecondary;
                                rowLocks[row] = true;
                            }
                        }
                        else if ((sizeStr != "" && (widthStr != "" || row == 0)) ||
                                     // we have a blank cell, lets try to fill it in
                                     (sizeStr != "" && widthStr.Trim().Length == 0 && row == 0))
                        // if no secondary size, only allow one row	
                        {

                            // get the size code based on the product category, size and width
                            eSearchContent sizeSearchType;
                            if (colLocks[col] || sizeGroupProperties.VerifyCriteria == eSearchContent.WholeField)
                                sizeSearchType = eSearchContent.WholeField;
                            else
                                sizeSearchType = sizeGroupProperties.VerifyCriteria;

                            eSearchContent widthSearchType;
                            if (rowLocks[row] || sizeGroupProperties.VerifyCriteria == eSearchContent.WholeField)
                                widthSearchType = eSearchContent.WholeField;
                            else
                                widthSearchType = sizeGroupProperties.VerifyCriteria;

                            if (widthStr == string.Empty)
                                widthStr = _noSizeDimensionLbl;

                            SizeCodeList scl = SAB.HierarchyServerSession.GetSizeCodeList(
                                sizeGroupProperties.ProductCategory, eSearchContent.WholeField,
                                sizeStr, sizeSearchType,
                                widthStr, widthSearchType);

                            if (scl.Count > 1) // Have the user choose the code
                            {
                                if (message == null
                                    && !sizeGroupProperties.SizeSelectionIsSet)
                                {
                                    SizeCodeProfile scp = null;
                                    foreach (SizeCodeProfile sizeCode in scl)
                                    {
                                        string listStr = sizeCode.SizeCodeID;
                                        if (sizeCode.SizeCodePrimary != null && sizeCode.SizeCodePrimary.Trim().Length != 0)
                                        {
                                            listStr += "   -   " + sizeCode.SizeCodePrimary;
                                        }
                                        if (sizeCode.SizeCodeSecondary != null && sizeCode.SizeCodeSecondary.Trim().Length != 0)
                                        {
                                            listStr += "   -   " + sizeCode.SizeCodeSecondary;
                                        }
                                        sizeGroupProperties.SizeSelection.Add(new KeyValuePair<int, string>(sizeCode.Key, listStr));

                                        if (sizeCode.SizeCodePrimary == sizeStr
                                            && sizeCode.SizeCodeSecondary == widthStr)
                                        {
                                            scp = sizeCode;
                                        }
                                    }

                                    message = "Select correct size code for " + sizeStr + ", " + widthStr;

                                    if (scp == null)
                                    {
                                        scp = (SizeCodeProfile)scl.ArrayList[0];
                                    }
                                    sizeCodeStr = scp.SizeCodeID;
                                }
                                verified = false;
                            }
                            else if (scl.Count == 1) // No choice, only one code
                            {
                                SizeCodeProfile scp = (SizeCodeProfile)scl.ArrayList[0];
                                sizeCodeStr = scp.SizeCodeID;
                            }
                            else
                            {
                                if (message == null)
                                {
                                    message = MIDText.GetText(eMIDTextCode.msg_NoSizeCodeFound);
                                    message = message.Replace("{0}", sizeStr);
                                    message = message.Replace("{1}", widthStr);
                                    message = message.Replace("{2}", sizeGroupProperties.ProductCategory);
                                    sizeGroupProperties.MessageRow = row;
                                    sizeGroupProperties.MessageColumn = col;
                                }
                                verified = false;
                            }

                            if (sizeCodeStr != "")
                            {
                                SizeCodeProfile scp = GetSizeCodeProfile(ID: sizeCodeStr);
                                if (col > sizeGroupProperties.Size.Count - 1)
                                {
                                    sizeGroupProperties.Size.Add(new KeyValuePair<int, string>(col, scp.SizeCodePrimary));
                                }
                                sizeStr = scp.SizeCodePrimary;
                                colLocks[col] = true;

                                if (row > sizeGroupProperties.Width.Count - 1)
                                {
                                    sizeGroupProperties.Width.Add(new KeyValuePair<int, string>(row, scp.SizeCodeSecondary));
                                }
                                widthStr = scp.SizeCodeSecondary;
                                rowLocks[row] = true;

                                if (sizeGroupProperties.SizeMatrix[row][col].Equals(default(KeyValuePair<int, string>)))
                                {
                                    sizeGroupProperties.SizeMatrix[row][col] = new KeyValuePair<int, string>(scp.Key, scp.SizeCodeID);
                                }
                            }
                        }
                    }
                }
            }

            if (verified)
            {
                // check for duplicate columns or rows
                Hashtable dupHash = new Hashtable();
                foreach (KeyValuePair<int, string> size in sizeGroupProperties.Size)
                {
                    string sizeStr = size.Value;
                    if (sizeStr != "")
                    {
                        if (dupHash.ContainsKey(sizeStr))
                        {
                            if (message == null)
                            {
                                message = MIDText.GetText(eMIDTextCode.msg_DuplicateSize);
                                message = message.Replace("{0}", sizeStr);
                            }
                            verified = false;
                        }
                        dupHash.Add(sizeStr, null);
                    }
                }
                dupHash.Clear();
                foreach (KeyValuePair<int, string> width in sizeGroupProperties.Width)
                {
                    string widthStr = width.Value;
                    if (widthStr != "")
                    {
                        if (dupHash.ContainsKey(widthStr))
                        {
                            if (message == null)
                            {
                                message = MIDText.GetText(eMIDTextCode.msg_DuplicateWidth);
                                message = message.Replace("{0}", widthStr);
                            }
                            verified = false;
                        }
                        dupHash.Add(widthStr, null);
                    }
                }
            }

            return verified;
        }

        override public bool ModelDelete(int key, ref string message)
        {
            if (_sizeGroupList == null)
            {
                GetSizeModels();
            }

            SizeGroupProfile sgp = (SizeGroupProfile)_sizeGroupList.FindKey(key);
            if (sgp == null)
            {
                message = SAB.ClientServerSession.Audit.GetText(
                    messageCode: eMIDTextCode.msg_ValueWasNotFound,
                    addToAuditReport: true,
                    args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_Size_Group) }
                    );
                return false;
            }

            sgp.DeleteSizeGroup();

            _sizeGroupList.Remove(sgp);

            return true;
        }

        override public bool ModelNameExists(string name)
        {
            if (_sizeGroupList == null)
            {
                GetSizeModels();
            }

            SizeGroupProfile sgp = _sizeGroupList.FindGroupName(name);
            if (sgp != null )
            {
                return true;
            }

            return false; 
        }
    }
}
