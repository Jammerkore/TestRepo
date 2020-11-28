using System;

namespace MIDRetail.DataCommon
{
    ///// <summary>
    ///// Used to retrieve and update specific information about 
    ///// a node from the Windows clipboard.
    ///// </summary>
    //[Serializable()]
    //public class HierarchyClipboardData
    //{
    //    private int					_methodName;
    //    private eHierarchyType		_hierarchyType;
    //    private int					_homeHierarchyRID;
    //    private eHierarchyType		_homeHierarchyType;
    //    private int					_parentRID;
    //    // Begin Track #5005 - JSmith - Explorer Organization
    //    //private eHierarchyNodeType	_nodeType;
    //    private eProfileType _nodeType;
    //    private object _treeNode;

    //    // End Track #5005
    //    //		private eSecurityLevel		_securityLevel;
		
    //    /// <summary>
    //    /// Used to construct an instance of the class.
    //    /// </summary>
    //    public HierarchyClipboardData() 
    //    {
    //        _treeNode = null;
    //    }

    //    /// <summary>
    //    /// Gets or sets the hierarchy record ID for the node being put to the clipboard.
    //    /// </summary>
    //    public int HierarchyRID 
    //    {
    //        get { return _methodName ; }
    //        set { _methodName = value; }
    //    }
    //    /// <summary>
    //    /// Gets or sets the hierarchy type for the node being put to the clipboard.
    //    /// </summary>
    //    public eHierarchyType HierarchyType 
    //    {
    //        get { return _hierarchyType ; }
    //        set { _hierarchyType = value; }
    //    }
    //    /// <summary>
    //    /// Gets or sets the home hierarchy record ID for the node being put to the clipboard.
    //    /// </summary>
    //    public int HomeHierarchyRID 
    //    {
    //        get { return _homeHierarchyRID ; }
    //        set { _homeHierarchyRID = value; }
    //    }
    //    /// <summary>
    //    /// Gets or sets the homehierarchy type for the node being put to the clipboard.
    //    /// </summary>
    //    public eHierarchyType HomeHierarchyType 
    //    {
    //        get { return _homeHierarchyType ; }
    //        set { _homeHierarchyType = value; }
    //    }
    //    /// <summary>
    //    /// Gets or sets the record ID of the parent for the node being put to the clipboard.
    //    /// </summary>
    //    public int ParentRID 
    //    {
    //        get { return _parentRID ; }
    //        set { _parentRID = value; }
    //    }
    //    // Begin Track #5005 - JSmith - Explorer Organization
    //    /// <summary>
    //    /// Gets or sets the type of node being put to the clipboard.
    //    /// </summary>
    //    //public eHierarchyNodeType NodeType 
    //    public eProfileType NodeType 
    //    {
    //        get { return _nodeType ; }
    //        set { _nodeType = value; }
    //    }

    //    /// <summary>
    //    /// Gets or sets the MIDHierarchyNode being put to the clipboard.
    //    /// </summary>
    //    /// <remarks>
    //    /// The object must be cast to retrieve the data
    //    /// </remarks>
    //    public object TreeNode
    //    {
    //        get { return _treeNode; }
    //        set { _treeNode = value; }
    //    }
    //    // End Track #5005
    //}

    ///// <summary>
    ///// Used to retrieve and update specific information about 
    ///// a method from the Windows clipboard.
    ///// </summary>
    //[Serializable()]
    //public class MethodClipboardData
    //{
    //    private string				_methodName;
    //    private int					_parentRID;
    //    private eMethodType			_methodType;
    //    private int					_userRID;
    //    private eWorkflowMethodIND  _workflowMethodIND;
    //    private eWorkflowType _workflowType;
		
    //    /// <summary>
    //    /// Used to construct an instance of the class.
    //    /// </summary>
    //    public MethodClipboardData() 
    //    {

    //    }

    //    /// <summary>
    //    /// Gets or sets the name for the method being put to the clipboard.
    //    /// </summary>
    //    public string MethodName 
    //    {
    //        get { return _methodName ; }
    //        set { _methodName = value; }
    //    }
    //    /// <summary>
    //    /// Gets or sets the record ID of the parent for the node being put to the clipboard.
    //    /// </summary>
    //    public int ParentRID 
    //    {
    //        get { return _parentRID ; }
    //        set { _parentRID = value; }
    //    }

    //    /// <summary>
    //    /// Gets or sets the Method Type of the node.
    //    /// </summary>
    //    public eMethodType MethodType 
    //    {
    //        get { return _methodType ; }
    //        set { _methodType = value; }
    //    }

    //    /// <summary>
    //    /// Gets or sets the record ID of the user for the node being put to the clipboard.
    //    /// </summary>
    //    public int UserRID 
    //    {
    //        get { return _userRID ; }
    //        set { _userRID = value; }
    //    }

    //    public eWorkflowMethodIND  WorkflowMethodIND
    //    {
    //        get { return _workflowMethodIND ; }
    //        set { _workflowMethodIND = value; }
    //    }

    //    public eWorkflowType WorkflowType
    //    {
    //        get { return _workflowType; }
    //        set { _workflowType = value; }
    //    }
    //}

    ///// <summary>
    ///// Used to retrieve and update specific information about 
    ///// a header from the Windows clipboard.
    ///// </summary>
    //[Serializable()]
    //public class HeaderClipboardData
    //{
    //    private string				_headerName;
		
    //    /// <summary>
    //    /// Used to construct an instance of the class.
    //    /// </summary>
    //    public HeaderClipboardData() 
    //    {

    //    }

    //    /// <summary>
    //    /// Gets or sets the name for the header being put to the clipboard.
    //    /// </summary>
    //    public string HeaderName 
    //    {
    //        get { return _headerName ; }
    //        set { _headerName = value; }
    //    }
		
    //}

    ///// <summary>
    ///// Used to retrieve and update specific information about 
    ///// a color from the Windows clipboard.
    ///// </summary>
    //[Serializable()]
    //public class ColorClipboardData
    //{
    //    private string _colorName;
    //    private eProfileType _nodeType;

    //    /// <summary>
    //    /// Used to construct an instance of the class.
    //    /// </summary>
    //    public ColorClipboardData()
    //    {

    //    }

    //    /// <summary>
    //    /// Gets or sets the name for the color being put to the clipboard.
    //    /// </summary>
    //    public string ColorName
    //    {
    //        get { return _colorName; }
    //        set { _colorName = value; }
    //    }

    //    /// <summary>
    //    /// Gets or sets the type of node being put to the clipboard.
    //    /// </summary>
    //    public eProfileType NodeType
    //    {
    //        get { return _nodeType; }
    //        set { _nodeType = value; }
    //    }

    //}

    ///// <summary>
    ///// Used to retrieve and update specific information about 
    ///// a product characteristic from the Windows clipboard.
    ///// </summary>
    //[Serializable()]
    //public class ProductCharClipboardData
    //{
    //    private string _productCharValue;
    //    //private eProductCharNodeType _nodeType;
    //    private eProfileType _nodeType;
    //    private int _productCharGroupKey;

    //    /// <summary>
    //    /// Used to construct an instance of the class.
    //    /// </summary>
    //    public ProductCharClipboardData()
    //    {

    //    }

    //    /// <summary>
    //    /// Gets or sets the value of the product characteristic being put to the clipboard.
    //    /// </summary>
    //    public string ProductCharValue
    //    {
    //        get { return _productCharValue; }
    //        set { _productCharValue = value; }
    //    }

    //    /// <summary>
    //    /// Gets or sets the type of node being put to the clipboard.
    //    /// </summary>
    //    public eProfileType NodeType
    //    {
    //        get { return _nodeType; }
    //        set { _nodeType = value; }
    //    }

    //    /// <summary>
    //    /// Gets or sets the key of the product group for the value being put to the clipboard.
    //    /// </summary>
    //    public int ProductCharGroupKey
    //    {
    //        get { return _productCharGroupKey; }
    //        set { _productCharGroupKey = value; }
    //    }
    //}

    ///// <summary>
    ///// Used to retrieve and update specific information about 
    ///// a filter from the Windows clipboard.
    ///// </summary>
    //[Serializable()]
    //public class FilterClipboardData
    //{
    //    private string _filterName;
    //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //    //private eFilterNodeType _nodeType;
    //    private eProfileType _nodeType;
    //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //    private object _filterNode;

    //    /// <summary>
    //    /// Used to construct an instance of the class.
    //    /// </summary>
    //    public FilterClipboardData()
    //    {

    //    }

    //    /// <summary>
    //    /// Gets or sets the name for the filter being put to the clipboard.
    //    /// </summary>
    //    public string FilterName
    //    {
    //        get { return _filterName; }
    //        set { _filterName = value; }
    //    }

    //    /// <summary>
    //    /// Gets or sets the type of node being put to the clipboard.
    //    /// </summary>
    //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //    //public eFilterNodeType NodeType
    //    public eProfileType NodeType
    //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //    {
    //        get { return _nodeType; }
    //        set { _nodeType = value; }
    //    }

    //    /// <summary>
    //    /// Gets or sets the type of node being put to the clipboard.
    //    /// </summary>
    //    public object FilterNode
    //    {
    //        get { return _filterNode; }
    //        set { _filterNode = value; }
    //    }
    //}

    ///// <summary>
    ///// Used to retrieve and update specific information about 
    ///// a TaskList from the Windows clipboard.
    ///// </summary>
    //[Serializable()]
    //public class TaskListClipboardData
    //{
    //    private string _taskListName;
    //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //    //private eTaskListNodeType _nodeType;
    //    //private TaskListProfile _tasklistProfile;
    //    private eProfileType _nodeType;
    //    private object _taskListNode;
    //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

    //    /// <summary>
    //    /// Used to construct an instance of the class.
    //    /// </summary>
    //    public TaskListClipboardData()
    //    {

    //    }

    //    /// <summary>
    //    /// Gets or sets the name for the taskList being put to the clipboard.
    //    /// </summary>
    //    public string TaskListName
    //    {
    //        get { return _taskListName; }
    //        set { _taskListName = value; }
    //    }

    //    /// <summary>
    //    /// Gets or sets the type of node being put to the clipboard.
    //    /// </summary>
    //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //    //public eTaskListNodeType NodeType
    //    //{
    //    //    get { return _nodeType; }
    //    //    set { _nodeType = value; }
    //    //}

    //    ///// <summary>
    //    ///// Gets or sets the tasklist profile of node being put to the clipboard.
    //    ///// </summary>
    //    //public TaskListProfile TasklistProfile
    //    //{
    //    //    get { return _tasklistProfile; }
    //    //    set { _tasklistProfile = value; }
    //    //}
    //    public eProfileType NodeType
    //    {
    //        get { return _nodeType; }
    //        set { _nodeType = value; }
    //    }

    //    /// <summary>
    //    /// Gets or sets the type of node being put to the clipboard.
    //    /// </summary>
    //    public object TaskListNode
    //    {
    //        get { return _taskListNode; }
    //        set { _taskListNode = value; }
    //    }
    //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //}

    ///// <summary>
    ///// Used to retrieve and update specific information about 
    ///// a TaskList from the Windows clipboard.
    ///// </summary>
    //[Serializable()]
    //public class JobClipboardData
    //{
    //    private string _jobName;
    //    //private eTaskListNodeType _nodeType;
    //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //    //private JobProfile _jobProfile;
    //    private object _jobNode;
    //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

    //    /// <summary>
    //    /// Used to construct an instance of the class.
    //    /// </summary>
    //    public JobClipboardData()
    //    {

    //    }

    //    /// <summary>
    //    /// Gets or sets the name for the job being put to the clipboard.
    //    /// </summary>
    //    public string JobName
    //    {
    //        get { return _jobName; }
    //        set { _jobName = value; }
    //    }

    //    ///// <summary>
    //    ///// Gets or sets the type of node being put to the clipboard.
    //    ///// </summary>
    //    //public eTaskListNodeType NodeType
    //    //{
    //    //    get { return _nodeType; }
    //    //    set { _nodeType = value; }
    //    //}

    //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //    ///// <summary>
    //    ///// Gets or sets the job profile of node being put to the clipboard.
    //    ///// </summary>
    //    //public JobProfile JobProfile
    //    //{
    //    //    get { return _jobProfile; }
    //    //    set { _jobProfile = value; }
    //    //}
    //    /// <summary>
    //    /// Gets or sets the type of node being put to the clipboard.
    //    /// </summary>
    //    public object JobNode
    //    {
    //        get { return _jobNode; }
    //        set { _jobNode = value; }
    //    }
    //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //}
}
