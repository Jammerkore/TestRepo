using MIDRetail.DataCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Logility.ROWebSharedTypes
{

    [DataContract(Name = "RONoDataOut", Namespace = "http://Logility.ROWeb/")]
    public class RONoDataOut : ROOut
    {

        public RONoDataOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {

        }

    };

    [DataContract(Name = "RODataTableOut", Namespace = "http://Logility.ROWeb/")]
    public class RODataTableOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private DataTable _dtOutput;                    // the node id

        public RODataTableOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, DataTable dtOutput) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _dtOutput = dtOutput;
        }

        public DataTable dtOutput { get { return _dtOutput; } }

    };


    [DataContract(Name = "RODataSetOut", Namespace = "http://Logility.ROWeb/")]
    public class RODataSetOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private DataSet _dsOutput;                    // the node id

        public RODataSetOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, DataSet dsOutput) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _dsOutput = dsOutput;
        }

        public DataSet dsOutput { get { return _dsOutput; } }

    };

    [DataContract(Name = "ROIntOut", Namespace = "http://Logility.ROWeb/")]
    public class ROIntOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private int _iOutput;                    // the node id

        public ROIntOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, int iOutput) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _iOutput = iOutput;
        }

        public int iOutput { get { return _iOutput; } }

    };

    [DataContract(Name = "ROLongOut", Namespace = "http://Logility.ROWeb/")]
    public class ROLongOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private long _lOutput;                    // the node id

        public ROLongOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, long lOutput) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _lOutput = lOutput;
        }

        public long iOutput { get { return _lOutput; } }

    };

    [DataContract(Name = "ROListOut", Namespace = "http://Logility.ROWeb/")]
    public class ROListOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ArrayList _ROListOutput;                    // the node id

        public ROListOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ArrayList ROListOutput) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROListOutput = ROListOutput;
        }

        public ArrayList ROListOutput { get { return _ROListOutput; } }

    };

    [DataContract(Name = "ROIntStringPairListOut", Namespace = "http://Logility.ROWeb/")]
    public class ROIntStringPairListOut : ROOut
    {
        [DataMemberAttribute(IsRequired = true)]
        private List<KeyValuePair<int, string>> _list = null;

        public ROIntStringPairListOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, List<KeyValuePair<int, string>> list) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _list = list;
        }

        public List<KeyValuePair<int, string>> ROList
        {
            get { return _list; }
        }
    };

    [DataContract(Name = "ROIListOut", Namespace = "http://Logility.ROWeb/")]
    public class ROIListOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private IList _ROIListOutput;                    // the node id

        public ROIListOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, IList ROIListOutput) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROIListOutput = ROIListOutput;
        }

        public IList ROIListOutput { get { return _ROIListOutput; } }

    };

    [DataContract(Name = "ROListObjectOut", Namespace = "http://Logility.ROWeb/")]
    public class ROListCustomOut<T> : ROOut, IEnumerable<T>
    {
        [DataMember(IsRequired = true)]
        private List<T> _ROListObjectOutput;                    // the node id

        public ROListCustomOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, List<T> ROListObjectOutput) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROListObjectOutput = ROListObjectOutput;
        }

        public List<T> ROListObjectOutput { get { return _ROListObjectOutput; } }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _ROListObjectOutput.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _ROListObjectOutput.GetEnumerator();
        }

    };

    [DataContract(Name = "ROActiveClass", Namespace = "http://Logility.ROWeb/")]
    public class ROActiveClass
    {
        [DataMember(IsRequired = true)]
        private eROClass _ROActiveClass;
        [DataMember(IsRequired = true)]
        private List<long> _instanceIDs;
        public eROClass ActiveClass { get { return _ROActiveClass; } }
        public List<long> InstanceIDs { get { return _instanceIDs; } }

        public ROActiveClass(eROClass ROActiveClass)
        {
            _ROActiveClass = ROActiveClass;
            _instanceIDs = new List<long>();
        }
    }


    [DataContract]
    [KnownType(typeof(ROOTSWorkflowStepOut))]
    public class ROOut<TElement> : ROOut
    {

        [DataMember]
        public List<TElement> ROGenericListOutput { get; set; }

        public ROOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, List<TElement> ROListOutput) :
        base(ROReturnCode, sROMessage, ROInstanceID)

        {
            ROGenericListOutput = ROListOutput;
        }

    }

    [DataContract(Name = "ROALLocationReviewPropertiesOut", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationReviewSelectionPropertiesOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROAllocationReviewSelectionProperties rOAllocationProperties;

        public ROAllocationReviewSelectionPropertiesOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROAllocationReviewSelectionProperties ROAllocationProperties) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            rOAllocationProperties = ROAllocationProperties;
        }

        public ROAllocationReviewSelectionProperties ROAllocationProperties { get { return rOAllocationProperties; } }

    };

    [DataContract(Name = "ROWorkflowOut", Namespace = "http://Logility.ROWeb/")]
    public class ROWorkflowOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROWorkflow rOWorkflow;

        public ROWorkflowOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROWorkflow roWorkflow) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            rOWorkflow = roWorkflow;
        }

        public ROWorkflow ROWorkflow { get { return rOWorkflow; } }

    };

    [DataContract(Name = "ROBoolOut", Namespace = "http://Logility.ROWeb/")]
    public class ROBoolOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private Boolean _ROBoolOutput;

        public ROBoolOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, bool ROBoolOutput) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROBoolOutput = ROBoolOutput;
        }

        public Boolean ROBoolOutput { get { return _ROBoolOutput; } }

    };

    #region "ROTreeview Structure Classes"

    public class ROExplorerView
    {
        public int FolderID { get; set; }

        public int Key { get; set; }

        public string FolderName { get; set; }

        public List<ROSubFolder> SubNode { get; set; }

        public List<ROFilterNode> FilterNodes { get; set; }

    }

    public class ROSubFolder
    {
        public int SubFolderID { get; set; }
        public string SubFolderName { get; set; }

        public int ParentFolder { get; set; }

        public List<ROFilterNode> FilterNodes { get; set; }
    }

    public class ROFilterNode
    {
        public int FilterID { get; set; }
        public int ParentID { get; set; }
        public string FilterName { get; set; }
    }

    #endregion

}

