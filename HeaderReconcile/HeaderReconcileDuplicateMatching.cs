using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace HeaderReconcile
{
    //==========================================================================
    // HeaderReconcileDuplicateMatching
    // Houses all of the code that determines if the transaction w/o a header ID
    // matching another transaction in the processing set of files.
    //==========================================================================


    public partial class HeaderReconcileProcess
    {

        private const string _color = "COLOR";
        private const string _pack = "PACK NAME";	// TT#1605-MD - stodd - Adding 'sequence' to other header ID keys causes sequence to replace all other keys.

        private const string _sequence = "SEQUENCE";


        /// <summary>
        /// Generates the HeaderDuplicateKey used to compare transactions for
        /// duplicates.
        /// </summary>
        /// <param name="hdrTran"></param>
        /// <param name="aHeaderId"></param>
        /// <param name="transNo"></param>
        /// <returns></returns>
        private eReturnCode GetHeaderDuplicateKey(HeadersHeader hdrTran, ref HeaderDuplicateKey headerDupKey)
        {
            eReturnCode returnCode = eReturnCode.successful;
            string aHeaderKeyUpper;
            List<object> keyVaues = new List<object>();
            List<object> characteristicVaues = new List<object>();
            List<string> bulkColors = new List<string>();
            List<String> packs = new List<string>();
            List<String> packColors = new List<string>();

            foreach (string headerKey in _headerKeysToMatchList)
            {
                if (string.IsNullOrEmpty(headerKey))
                {
                    continue;
                }
                aHeaderKeyUpper = headerKey.ToUpper();
                eHeaderMatchingKeyType aHeaderKeyType = GetHeaderMatchKeyType(headerKey);
                switch (aHeaderKeyType)
                {
                    case eHeaderMatchingKeyType.HeaderField:
                        {
                            GetHeaderValue(hdrTran, headerKey, keyVaues);
                            break;
                        }

                    case eHeaderMatchingKeyType.Color:
                        {
                            GetColorValues(hdrTran, headerKey, bulkColors, packColors);
                            break;
                        }

                    case eHeaderMatchingKeyType.Pack:
                        {
                            GetPackValues(hdrTran, headerKey, packs, packColors);
                            break;
                        }

                    case eHeaderMatchingKeyType.Characteristic:
                        {
                            if (hdrTran.Characteristic == null)
                            {
                                characteristicVaues.Add(string.Empty);
                            }
                            else
                            {
                                HeadersHeaderCharacteristic aChar = Array.Find(hdrTran.Characteristic, c => c.Name.ToUpper() == headerKey);
                                // Begin TT#1604-MD - stodd - Object Reference Error when running Header Reconcile API
                                if (aChar == null)
                                {
                                    characteristicVaues.Add(string.Empty);
                                }
                                else
                                {
                                    characteristicVaues.Add(aChar.Value);
                                }
                                // End TT#1604-MD - stodd - Object Reference Error when running Header Reconcile API
                            }
                            break;
                        }

                    default:
                        break;
                }
            }

            headerDupKey = new HeaderDuplicateKey(keyVaues, characteristicVaues, bulkColors, packs, packColors);

            return returnCode;
        }

        /// <summary>
        ///  Determines which type of header key has been entered.
        /// </summary>
        /// <param name="aHeaderkey"></param>
        /// <returns></returns>
        private eHeaderMatchingKeyType GetHeaderMatchKeyType(string aHeaderkey)
        {
            if (aHeaderkey.ToUpper() == _sequence)
            {
                return eHeaderMatchingKeyType.Sequence;
            }
            else if (aHeaderkey.ToUpper() == _color)
            {
                return eHeaderMatchingKeyType.Color;
            }
            else if (aHeaderkey.ToUpper() == _pack)
            {
                return eHeaderMatchingKeyType.Pack;
            }
            else if (Include.HeaderKeyList.Contains(aHeaderkey.ToUpper()))
            {
                return eHeaderMatchingKeyType.HeaderField;
            }
            else
            {
                return eHeaderMatchingKeyType.Characteristic;
            }
        }

        private eReturnCode GetHeaderValue(HeadersHeader hdrTran, string headerKey, List<object> keyValues)
        {
            eReturnCode returnCode = eReturnCode.successful;

            switch (headerKey)
            {
                case "HEADERDESCRIPTION":
                    keyValues.Add(hdrTran.HeaderDescription);
                    break;

                case "HEADERDATE":
                    if (hdrTran.HeaderDateSpecified)
                    {
                        keyValues.Add(hdrTran.HeaderDate);
                    }
                    else
                    {
                        keyValues.Add(string.Empty);
                    }
                    break;

                case "UNITRETAIL":
                    if (hdrTran.UnitRetailSpecified)
                    {
                        keyValues.Add(hdrTran.UnitRetail);
                    }
                    else
                    {
                        keyValues.Add(string.Empty);
                    }
                    break;

                case "UNITCOST":
                    if (hdrTran.UnitCostSpecified)
                    {
                        keyValues.Add(hdrTran.UnitCost);
                    }
                    else
                    {
                        keyValues.Add(string.Empty);
                    }
                    break;

                case "STYLEID":
                    keyValues.Add(hdrTran.StyleId);
                    break;

                case "PARENTOFSTYLEID":
                    keyValues.Add(hdrTran.ParentOfStyleId);
                    break;

                case "SIZEGROUPNAME":
                    keyValues.Add(hdrTran.SizeGroupName);
                    break;

                case "HEADERTYPE":
                    keyValues.Add(hdrTran.HeaderType);
                    break;

                case "DISTCENTER":
                    keyValues.Add(hdrTran.DistCenter);
                    break;

                case "PURCHASEORDER":
                    keyValues.Add(hdrTran.PurchaseOrder);
                    break;

                case "VENDOR":
                    keyValues.Add(hdrTran.Vendor);
                    break;

                case "WORKFLOW":
                    keyValues.Add(hdrTran.Workflow);
                    break;

                case "VSWID":
                    keyValues.Add(hdrTran.VSWID);
                    break;
            }

            return returnCode;
        }

        private void GetColorValues(HeadersHeader hdrTran, string headerKey, List<string> bulkColors, List<string> packColors)
        {
            if (hdrTran.BulkColor != null)
            {
                foreach (HeadersHeaderBulkColor aTransColor in hdrTran.BulkColor)
                {
                    bulkColors.Add(aTransColor.ColorCodeID);
                }
            }

            if (hdrTran.Pack != null)
            {
                foreach (HeadersHeaderPack aTransPack in hdrTran.Pack)
                {
                    if (aTransPack.PackColor != null)
                    {
                        foreach (HeadersHeaderPackPackColor aTransPackColor in aTransPack.PackColor)
                        {
                            packColors.Add(aTransPackColor.ColorCodeID);
                        }
                    }
                }
            }
        }

        private void GetPackValues(HeadersHeader hdrTran, string headerKey, List<string> packs, List<string> packColors)
        {
            if (hdrTran.Pack != null)
            {
                foreach (HeadersHeaderPack aTransPack in hdrTran.Pack)
                {
                    packs.Add(aTransPack.Name);

                    if (aTransPack.PackColor != null)
                    {
                        foreach (HeadersHeaderPackPackColor aTransPackColor in aTransPack.PackColor)
                        {
                            packColors.Add(aTransPackColor.ColorCodeID);
                        }
                    }
                }
            }
        }

    }

    public class HeaderDuplicateKey
    {
        // Auto-Implemented Properties
        public List<object> KeyValues { get; set; }
        public List<object> CharacteristicVaues { get; set; }
        public List<string> BulkColors { get; set; }
        public List<string> Packs { get; set; }
        public List<string> PackColors { get; set; }

        // Constructor
        public HeaderDuplicateKey(List<object> keyVaues, List<object>  characteristicVaues, List<string> bulkColors, List<string> packs, List<string> packColors)
        {
            KeyValues = keyVaues;
            CharacteristicVaues = characteristicVaues;
            BulkColors = bulkColors;
            Packs = packs;
            PackColors = packColors;
        }


        public override int GetHashCode()
        {
            int hash = 13;
            // Begin TT#1604-MD - stodd - Object Reference Error when running Header Reconcile API
            foreach (object o in KeyValues)
            {
                //hash += o.GetHashCode();
                hash += o == null ? 0 : o.GetHashCode();
            }
            foreach (object o in CharacteristicVaues)
            {
                //hash += o.GetHashCode();
                hash += o == null ? 0 : o.GetHashCode();
            }
            foreach (object o in BulkColors)
            {
                //hash += o.GetHashCode();
                hash += o == null ? 0 : o.GetHashCode();
            }
            foreach (object o in Packs)
            {
                //hash += o.GetHashCode();
                hash += o == null ? 0 : o.GetHashCode();
            }
            foreach (object o in PackColors)
            {
                //hash += o.GetHashCode();
                hash += o == null ? 0 : o.GetHashCode();
            }
            // End TT#1604-MD - stodd - Object Reference Error when running Header Reconcile API


            return hash;
        }

        public override bool Equals(object obj)
        {
            HeaderDuplicateKey headerDupKey = (HeaderDuplicateKey)obj;
            return KeyValues.SequenceEqual(headerDupKey.KeyValues) 
                && CharacteristicVaues.SequenceEqual(headerDupKey.CharacteristicVaues)
                && BulkColors.SequenceEqual(headerDupKey.BulkColors)
                && Packs.SequenceEqual(headerDupKey.Packs)
                && PackColors.SequenceEqual(headerDupKey.PackColors);
        }


        public int CompareTo(object obj)
        {
            int retCode;
            HeaderDuplicateKey headerDupKey = null;

            try
            {
                headerDupKey = (HeaderDuplicateKey)obj;
                retCode = 0;

                if (!KeyValues.SequenceEqual(headerDupKey.KeyValues))
                {
                    retCode = 1;
                }
                else if (!CharacteristicVaues.SequenceEqual(headerDupKey.CharacteristicVaues))
                {
                    retCode = 1;
                }
                else if (!BulkColors.SequenceEqual(headerDupKey.BulkColors))
                {
                    retCode = 1;
                }
                else if (!Packs.SequenceEqual(headerDupKey.Packs))
                {
                    retCode = 1;
                }
                else if (!PackColors.SequenceEqual(headerDupKey.PackColors))
                {
                    retCode = 1;
                }

                return retCode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

    }
}