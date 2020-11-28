using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;
using System.Text.RegularExpressions;

namespace MIDRetail.Business
{
    public static class ExceptionHandler
    {
        // Begin TT#1808-MD - JSmith - Store Load Error
        //Begin TT#1313-MD -jsobek -Header Filters -Allow for common exception handling across application
        //public static SessionAddressBlock SABforExceptions;
        //public static void HandleException(Exception ex, string caption = "Error:")
        //{
        //    //Add to audit log.
        //    if (SABforExceptions != null && SABforExceptions.ClientServerSession != null && SABforExceptions.ClientServerSession.Audit != null)
        //    {
        //        SABforExceptions.ClientServerSession.Audit.Log_Exception(ex);
        //    }

        //    //Show message on screen.
        //    System.Windows.Forms.MessageBox.Show("Error: " + ex.ToString(), caption);
        //}
        //public static void HandleException(string err, string caption = "Error:")
        //{
        //    //Add to audit log.
        //    if (SABforExceptions != null && SABforExceptions.ClientServerSession != null && SABforExceptions.ClientServerSession.Audit != null)
        //    {
        //        SABforExceptions.ClientServerSession.Audit.Log_Exception(err);
        //    }

        //    //Show message on screen.
        //    System.Windows.Forms.MessageBox.Show("Error: " + err, caption);
        //}

        private static Session _session;
        private static bool _showMessageBox;   

        public static void Initialize(Session session, bool showMessageBox = false)
        {
            _session = session;
            _showMessageBox = showMessageBox;
        }

        public static void HandleException(Exception ex, string caption = "Error:")
        {
            //Add to audit log.
            if (_session != null && _session.Audit != null)
            {
                _session.Audit.Log_Exception(ex);
            } 

            //Show message on screen.
            if (_showMessageBox)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + ex.ToString(), caption);
            }
        }
        public static void HandleException(string err, string caption = "Error:")
        {
            //Add to audit log.
            if (_session != null && _session.Audit != null)
            {
                _session.Audit.Log_Exception(err);
            }

            //Show message on screen.
            if (_showMessageBox)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + err, caption);
            }
        }
        // End TT#1808-MD - JSmith - Store Load Error

        //End TT#1313-MD -jsobek -Header Filters -Allow for common exception handling across application
        public static bool InDebugMode; //TT#1372-MD -jsobek -Filters - Move Condition Display Menu
    }

    //Begin TT#1532-MD -jsobek -Add In Use for Header Characteristics
    public static class InUseUtility
    {
        
        public static string GetInUseTitleFromProfileType(eProfileType etype)
        {
            string inUseTitle = Regex.Replace(etype.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
            return inUseTitle;
        }
    }
    //End TT#1532-MD -jsobek -Add In Use for Header Characteristics

    public class HeaderCharList
    {
        #region Fields
        //=======
        // FIELDS
        //=======

        #endregion Fields
        public HeaderCharList()
        {
        }

        /// <summary>
        /// Returns an ArrayList or sorted header and characteristic entries.
        /// </summary>
        /// <remarks>Header fields have negative keys to not duplicate characteristic RIDs</remarks>
        /// <returns></returns>
        public ArrayList BuildHeaderCharList()
        {
            try
            {
                string fieldName;
                HeaderCharacteristicsData hdcData = new HeaderCharacteristicsData();
                DataTable dt = hdcData.HeaderCharGroup_Read();

                SortedList entryList = new SortedList();
                HeaderFieldCharEntry oe;

                foreach (DataRow row in dt.Rows)
                {

                    oe = new HeaderFieldCharEntry('C', Convert.ToInt32(row["HCG_RID"]), Convert.ToString(row["HCG_ID"]), (eHeaderCharType)Convert.ToInt32(row["HCG_TYPE"]));
                    entryList.Add(oe.Text, oe);
                }

                // Add Header fields with negative keys as to not duplicate entries
                eHeaderCharType dataType;
                foreach (filterHeaderFieldTypes fieldType in filterHeaderFieldTypes.fieldTypeList)
                {
                    if (fieldType.dataType.valueType == filterValueTypes.Boolean)
                    {
                        dataType = eHeaderCharType.boolean;
                    }
                    else if (fieldType.dataType.valueType == filterValueTypes.Date)
                    {
                        dataType = eHeaderCharType.date;
                    }
                    else if (fieldType.dataType.valueType == filterValueTypes.Dollar)
                    {
                        dataType = eHeaderCharType.dollar;
                    }
                    else if (fieldType.dataType.valueType == filterValueTypes.Numeric)
                    {
                        dataType = eHeaderCharType.number;
                    }
                    else
                    {
                        dataType = eHeaderCharType.text;
                    }
                    fieldName = fieldType.Name;
                    if (fieldType.dbIndex == 2
                        || fieldType.dbIndex == 3)
                    {
                        fieldName += " [Level]";
                    }
                    oe = new HeaderFieldCharEntry('H', fieldType.dbIndex * -1, fieldName, dataType);
                    entryList.Add(oe.Text, oe);
                }

                ArrayList alEntries = new ArrayList();
                IDictionaryEnumerator dictEnum = entryList.GetEnumerator();

                while (dictEnum.MoveNext())
                {
                    oe = (HeaderFieldCharEntry)dictEnum.Value;
                    alEntries.Add(oe);
                }

                return alEntries;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object GetHeaderField(SessionAddressBlock SAB, filterHeaderFieldTypes fieldType, AllocationProfile ap )
        {

            if (fieldType == filterHeaderFieldTypes.HeaderID)
            {
                return ap.HeaderID;
            }
            else if (fieldType == filterHeaderFieldTypes.PO) //purchase order
            {
                return ap.PurchaseOrder;
            }
            else if (fieldType == filterHeaderFieldTypes.Vendor)
            {
                return ap.Vendor;
            }
            else if (fieldType == filterHeaderFieldTypes.DC) //distribution center
            {
                return ap.DistributionCenter;
            }
            else if (fieldType == filterHeaderFieldTypes.VswID) //aka IMO ID
            {
                return ap.ImoID;
            }
            else if (fieldType == filterHeaderFieldTypes.ShipStatus)
            {
                return MIDText.GetTextOnly(Convert.ToInt32(ap.HeaderShipStatus));
            }
            else if (fieldType == filterHeaderFieldTypes.IntransitStatus)
            {
                return MIDText.GetTextOnly(Convert.ToInt32(ap.HeaderIntransitStatus));
            }
            else if (fieldType == filterHeaderFieldTypes.QuantityToAllocate)
            {
                return ap.TotalUnitsToAllocate;
            }
            else if (fieldType == filterHeaderFieldTypes.SubClass) //Parent of style base node
            {
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID);
                return SAB.HierarchyServerSession.GetNodeData(hnp.HomeHierarchyParentRID).Text;
            }
            else if (fieldType == filterHeaderFieldTypes.Style)
            {
                return SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID).Text;
            }
            else if (fieldType == filterHeaderFieldTypes.NumPacks)
            {
                return ap.PackCount;
            }
            else if (fieldType == filterHeaderFieldTypes.NumBulkColors)
            {
                return ap.BulkColorCount;
            }
            else if (fieldType == filterHeaderFieldTypes.NumBulkSizes)
            {
                return ap.BulkColorSizeCount;
            }
            else if (fieldType == filterHeaderFieldTypes.SizeGroup)
            {
                SizeGroupProfile sgp = new SizeGroupProfile(ap.SizeGroupRID, false);
                return sgp.SizeGroupName;
            }
            else if (fieldType == filterHeaderFieldTypes.VswProcess)
            {
                if (ap.AdjustVSW_OnHand)
                {
                    return "Adjust";
                }
                else
                {
                    return "Replace";
                }
            }

            return null;
        }

        public object GetHeaderCharacteristic(SessionAddressBlock SAB, string HCG_ID, AllocationProfile ap)
        {
            return ((CharacteristicsBin)ap.Characteristics[HCG_ID]).Value;
        }
    }

    public class HeaderFieldCharEntry
    {
        private char _type;
        private int _key;
        private string _text;
        private eHeaderCharType _fieldDataType;

        public HeaderFieldCharEntry(char cType, int iKey, string sText, eHeaderCharType fieldDataType)
        {
            _type = cType;
            _key = iKey;
            _text = sText;
            _fieldDataType = fieldDataType;
        }

        /// <summary>
        /// Gets the type of override entry.
        /// </summary>
        public char Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the key of override entry.
        /// </summary>
        public int Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Gets the text of override entry.
        /// </summary>
        public string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// Gets the data type of the entry.
        /// </summary>
        public eHeaderCharType FieldDataType
        {
            get { return _fieldDataType; }
        }

    }
}
