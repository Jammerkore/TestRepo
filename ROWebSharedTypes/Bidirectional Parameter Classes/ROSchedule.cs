using MIDRetail.DataCommon;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Logility.ROWebSharedTypes
{
    [DataContract(Name = "ROTaskProperties", Namespace = "http://Logility.ROWeb/")]
    /// <summary>
    /// Base class for all task classes.
    /// </summary>
    /// <remarks>Also used to provide list of tasks in a task list</remarks>
    public class ROTaskProperties
    {
        /// <summary>
        /// KeyValuePair containing the key and name of the task
        /// </summary>
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _task;

        /// <summary>
        /// The eTaskType of the task
        /// </summary>
        [DataMember(IsRequired = true)]
        private eTaskType _taskType;

        /// <summary>
        /// KeyValuePair containing the eMIDMessageLevel and message leve of the task
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _maximumMessageLevel;

        /// <summary>
        /// The email success from email address
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailSuccessFrom;

        /// <summary>
        /// The email success to email address
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailSuccessTo;

        /// <summary>
        /// The email success from CC address
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailSuccessCC;

        /// <summary>
        /// The email success from BCC address
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailSuccessBCC;

        /// <summary>
        /// The email success subject
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailSuccessSubject;

        /// <summary>
        /// The email success message body
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailSuccessBody;

        /// <summary>
        /// The email failure from email address
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailFailureFrom;

        /// <summary>
        /// The email failure to email address
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailFailureTo;

        /// <summary>
        /// The email failure CC email address
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailFailureCC;

        /// <summary>
        /// The email failure BCC email address
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailFailureBCC;

        /// <summary>
        /// The email failure subject
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailFailureSubject;

        /// <summary>
        /// The email failure message body
        /// </summary>
        [DataMember(IsRequired = true)]
        private string _emailFailureBody;
        

        public ROTaskProperties(
            KeyValuePair<int, string> task,
            eTaskType taskType,
            KeyValuePair<int, string> maximumMessageLevel,
            string emailSuccessFrom = null,
            string emailSuccessTo = null,
            string emailSuccessCC = null,
            string emailSuccessBCC = null,
            string emailSuccessSubject = null,
            string emailSuccessBody = null,
            string emailFailureFrom = null,
            string emailFailureTo = null,
            string emailFailureCC = null,
            string emailFailureBCC = null,
            string emailFailureSubject = null,
            string emailFailureBody = null
            )
        {
            _task = task;
            _taskType = taskType;
            _maximumMessageLevel = maximumMessageLevel;
            _emailSuccessFrom = emailSuccessFrom;
            _emailSuccessTo = emailSuccessTo;
            _emailSuccessCC = emailSuccessCC;
            _emailSuccessBCC = emailSuccessBCC;
            _emailSuccessSubject = emailSuccessSubject;
            _emailSuccessBody = emailSuccessBody;
            _emailFailureFrom = emailFailureFrom;
            _emailFailureTo = emailFailureTo;
            _emailFailureCC = emailFailureCC;
            _emailFailureBCC = emailFailureBCC;
            _emailFailureSubject = emailFailureSubject;
            _emailFailureBody = emailFailureBody;
        }

        /// <summary>
        /// KeyValuePair containing the key and name of the task
        /// </summary>
        /// <remarks>Key is the sequence of the task</remarks>
        public KeyValuePair<int, string> Task
        {
            get { return _task; }
            set { _task = value; }
        }

        public eTaskType TaskType
        {
            get { return _taskType; }
            set { _taskType = value; }
        }

        public KeyValuePair<int, string> MaximumMessageLevel
        {
            get { return _maximumMessageLevel; }
            set { _maximumMessageLevel = value; }
        }

        /// <summary>
        /// The email success from email address
        /// </summary>
        public string EmailSuccessFrom
        {
            get { return _emailSuccessFrom; }
            set { _emailSuccessFrom = value; }
        }

        /// <summary>
        /// The email success to email address
        /// </summary>
        public string EmailSuccessTo
        {
            get { return _emailSuccessTo; }
            set { _emailSuccessTo = value; }
        }

        /// <summary>
        /// The email success from CC address
        /// </summary>
        public string EmailSuccessCC
        {
            get { return _emailSuccessCC; }
            set { _emailSuccessCC = value; }
        }

        /// <summary>
        /// The email success from BCC address
        /// </summary>
        public string EmailSuccessBCC
        {
            get { return _emailSuccessBCC; }
            set { _emailSuccessBCC = value; }
        }

        /// <summary>
        /// The email success subject
        /// </summary>
        public string EmailSuccessSubject
        {
            get { return _emailSuccessSubject; }
            set { _emailSuccessSubject = value; }
        }

        /// <summary>
        /// The email success message body
        /// </summary>
        public string EmailSuccessBody
        {
            get { return _emailSuccessBody; }
            set { _emailSuccessBody = value; }
        }

        /// <summary>
        /// The email failure from email address
        /// </summary>
        public string EmailFailureFrom
        {
            get { return _emailFailureFrom; }
            set { _emailFailureFrom = value; }
        }

        /// <summary>
        /// The email failure to email address
        /// </summary>
        public string EmailFailureTo
        {
            get { return _emailFailureTo; }
            set { _emailFailureTo = value; }
        }

        /// <summary>
        /// The email failure CC email address
        /// </summary>
        public string EmailFailureCC
        {
            get { return _emailFailureCC; }
            set { _emailFailureCC = value; }
        }

        /// <summary>
        /// The email failure BCC email address
        /// </summary>
        public string EmailFailureBCC
        {
            get { return _emailFailureBCC; }
            set { _emailFailureBCC = value; }
        }

        /// <summary>
        /// The email failure subject
        /// </summary>
        public string EmailFailureSubject
        {
            get { return _emailFailureSubject; }
            set { _emailFailureSubject = value; }
        }

        /// <summary>
        /// The email failure message body
        /// </summary>
        public string EmailFailureBody
        {
            get { return _emailFailureBody; }
            set { _emailFailureBody = value; }
        }

        public void CopyValuesToDerivedClass(ROTaskProperties taskProperties)
        {
            _emailSuccessFrom = taskProperties.EmailSuccessFrom;
            _emailSuccessTo = taskProperties.EmailSuccessTo;
            _emailSuccessCC = taskProperties.EmailSuccessCC;
            _emailSuccessBCC = taskProperties.EmailSuccessBCC;
            _emailSuccessSubject = taskProperties.EmailSuccessSubject;
            _emailSuccessBody = taskProperties.EmailSuccessBody;
            _emailFailureFrom = taskProperties.EmailFailureFrom;
            _emailFailureTo = taskProperties.EmailFailureTo;
            _emailFailureCC = taskProperties.EmailFailureCC;
            _emailFailureBCC = taskProperties.EmailFailureBCC;
            _emailFailureSubject = taskProperties.EmailFailureSubject;
            _emailFailureBody = taskProperties.EmailFailureBody;
        }

    }

    [DataContract(Name = "ROTaskAllocateMerchandiseWorkflowMethod", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskAllocateMerchandiseWorkflowMethod
    {
        /// <summary>
        /// KeyValuePair containing the key and name of the workflow or method
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _workflowOrMethod;

        /// <summary>
        /// A flag identifying if the _workflowOrMethod field is a workflow
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _isWorkflow;

        /// <summary>
        /// KeyValuePair containing the key and name of the execute date
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _executeDate;

        #region Public Properties

        /// <summary>
        /// Gets the flag identifying if the workflow or method has been set.
        /// </summary>
        public bool WorkflowOrMethodIsSet
        {
            get { return !_workflowOrMethod.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with workflow or method key and name
        /// </summary>
        public KeyValuePair<int, string> WorkflowOrMethod { get { return _workflowOrMethod; } set { _workflowOrMethod = value; } }

        /// <summary>
        /// A flag identifying if the _workflowOrMethod field is a workflow
        /// </summary>
        public bool IsWorkflow
        {
            get { return _isWorkflow; }
        }

        /// <summary>
        /// Gets the flag identifying if the execute date has been set.
        /// </summary>
        public bool ExecuteDateIsSet
        {
            get { return !_executeDate.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with execute date key and name
        /// </summary>
        public KeyValuePair<int, string> ExecuteDate { get { return _executeDate; } set { _executeDate = value; } }

        #endregion

        public ROTaskAllocateMerchandiseWorkflowMethod(
            KeyValuePair<int, string> workflowOrMethod = default(KeyValuePair<int, string>),
            bool isWorkflow = true,
            KeyValuePair<int, string> executeDate = default(KeyValuePair<int, string>)
            )
        {
            _workflowOrMethod = workflowOrMethod;
            _isWorkflow = isWorkflow;
            _executeDate = executeDate;
        }
    }

    [DataContract(Name = "ROTaskAllocateMerchandise", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskAllocateMerchandise
    {
        /// <summary>
        /// KeyValuePair containing the key and name of the merchandise
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _merchandise;

        /// <summary>
        /// KeyValuePair containing the key and name of the filter
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _filter;

        /// <summary>
        /// List of workflow entries
        /// </summary>
        [DataMember(IsRequired = true)]
        private List<ROTaskAllocateMerchandiseWorkflowMethod> _workflow;

        #region Public Properties

        /// <summary>
        /// Gets the flag identifying if the merchandise has been set.
        /// </summary>
        public bool DefaultToWorkflow
        {
            get { return !_merchandise.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with merchandise key and name
        /// </summary>
        public KeyValuePair<int, string> Merchandise { get { return _merchandise; } set { _merchandise = value; } }

        /// <summary>
        /// Gets the flag identifying if the merchandise has been set.
        /// </summary>
        public bool MerchandiseIsSet
        {
            get { return !_merchandise.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets the flag identifying if the filter has been set.
        /// </summary>
        public bool FilterIsSet
        {
            get { return !_filter.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with merchandise key and name
        /// </summary>
        public KeyValuePair<int, string> Filter { get { return _filter; } set { _filter = value; } }

        /// <summary>
        /// List of workflow entries
        /// </summary>
        public List<ROTaskAllocateMerchandiseWorkflowMethod> Workflow { get { return _workflow; } }

        /// <summary>
        /// Gets the flag identifying if there are workflow values.
        /// </summary>
        public bool HasWorkflowValues
        {
            get { return _workflow.Count > 0; }
        }

        #endregion

        public ROTaskAllocateMerchandise(
            KeyValuePair<int, string> merchandise = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> filter = default(KeyValuePair<int, string>))
        {
            _merchandise = merchandise;
            _filter = filter;
            _workflow = new List<ROTaskAllocateMerchandiseWorkflowMethod>();
        }
    }

    [DataContract(Name = "ROTaskAllocate", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskAllocate : ROTaskProperties
    {
        /// <summary>
        /// List of merchandise entries
        /// </summary>
        [DataMember(IsRequired = true)]
        private List<ROTaskAllocateMerchandise> _merchandise;

        #region Public Properties
        /// <summary>
        /// KeyValuePair with merchandise key and name
        /// </summary>
        public List<ROTaskAllocateMerchandise> Merchandise { get { return _merchandise; } }

        /// <summary>
        /// Gets the flag identifying if there are merchandise values.
        /// </summary>
        public bool HasMerchandiseValues
        {
            get { return _merchandise.Count > 0; }
        }
        #endregion

        public ROTaskAllocate(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.Allocate,
                maximumMessageLevel: maximumMessageLevel
                )

        {
            _merchandise = new List<ROTaskAllocateMerchandise>();
        }
    }

    [DataContract(Name = "ROTaskHierarchyLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskHierarchyLoad : ROTaskLoad
    {

        #region Public Properties

        #endregion

        public ROTaskHierarchyLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.HierarchyLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskStoreLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskStoreLoad : ROTaskLoad
    {

        #region Public Properties

        #endregion

        public ROTaskStoreLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.StoreLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskHistoryPlanLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskHistoryPlanLoad : ROTaskLoad
    {

        #region Public Properties

        #endregion

        public ROTaskHistoryPlanLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.HistoryPlanLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskColorCodeLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskColorCodeLoad : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskColorCodeLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.ColorCodeLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskSizeCodeLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskSizeCodeLoad : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskSizeCodeLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.SizeCodeLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskLoad : ROTaskProperties
    {
        [DataMember(IsRequired = true)]
        private string _directory;

        [DataMember(IsRequired = true)]
        private string _flagFileSuffix;

        [DataMember(IsRequired = true)]
        private int _concurrentFiles;

        [DataMember(IsRequired = true)]
        private int _processingDirection;

        [DataMember(IsRequired = true)]
        private bool _enableRunSuffix;

        [DataMember(IsRequired = true)]
        private string _runSuffix;

        #region Public Properties
        public string Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }
        public string FlagFileSuffix
        {
            get { return _flagFileSuffix; }
            set { _flagFileSuffix = value; }
        }
        public int ConcurrentFiles
        {
            get { return _concurrentFiles; }
            set { _concurrentFiles = value; }
        }
        public int ProcessingDirection
        {
            get { return _processingDirection; }
            set { _processingDirection = value; }
        }
        public bool EnableRunSuffix
        {
            get { return _enableRunSuffix; }
            set { _enableRunSuffix = value; }
        }
        public string RunSuffix
        {
            get { return _runSuffix; }
            set { _runSuffix = value; }
        }
        #endregion

        public ROTaskLoad(
            KeyValuePair<int, string> task,
            eTaskType taskType,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: taskType,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskHeaderLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskHeaderLoad : ROTaskLoad
    {
        #region Public Properties

        #endregion

        public ROTaskHeaderLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.HeaderLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskForecasting", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskForecasting : ROTaskProperties
    {
        /// <summary>
        /// List of merchandise entries
        /// </summary>
        [DataMember(IsRequired = true)]
        private List<ROTaskForecastMerchandise> _merchandise;

        #region Public Properties
        public List<ROTaskForecastMerchandise> Merchandise { get { return _merchandise; } }
        #endregion

        public ROTaskForecasting(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.Forecasting,
            maximumMessageLevel: maximumMessageLevel)

        {
            _merchandise = new List<ROTaskForecastMerchandise>();
        }
    }

    [DataContract(Name = "ROTaskForecastMerchandise", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskForecastMerchandise
    {
        /// <summary>
        /// KeyValuePair containing the key and name of the merchandise
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _merchandise;

        /// <summary>
        /// KeyValuePair containing the key and name of the filter
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _version;

        /// <summary>
        /// List of workflow entries
        /// </summary>
        [DataMember(IsRequired = true)]
        private List<ROTaskForecastMerchandiseWorkflowMethod> _workflow;

        #region Public Properties

        /// <summary>
        /// Gets the flag identifying if the merchandise has been set.
        /// </summary>
        public bool DefaultToWorkflow
        {
            get { return !_merchandise.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with merchandise key and name
        /// </summary>
        public KeyValuePair<int, string> Merchandise { get { return _merchandise; } set { _merchandise = value; } }

        /// <summary>
        /// Gets the flag identifying if the merchandise has been set.
        /// </summary>
        public bool MerchandiseIsSet
        {
            get { return !_merchandise.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets the flag identifying if the filter has been set.
        /// </summary>
        public bool VersionIsSet
        {
            get { return !_version.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with merchandise key and name
        /// </summary>
        public KeyValuePair<int, string> Version { get { return _version; } set { _version = value; } }

        /// <summary>
        /// List of workflow entries
        /// </summary>
        public List<ROTaskForecastMerchandiseWorkflowMethod> Workflow { get { return _workflow; } }

        /// <summary>
        /// Gets the flag identifying if there are workflow values.
        /// </summary>
        public bool HasWorkflowValues
        {
            get { return _workflow.Count > 0; }
        }

        #endregion

        public ROTaskForecastMerchandise(
            KeyValuePair<int, string> merchandise = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> version = default(KeyValuePair<int, string>))
        {
            _merchandise = merchandise;
            _version = version;
            _workflow = new List<ROTaskForecastMerchandiseWorkflowMethod>();
        }
    }

    [DataContract(Name = "ROTaskForecastMerchandiseWorkflowMethod", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskForecastMerchandiseWorkflowMethod
    {
        /// <summary>
        /// KeyValuePair containing the key and name of the workflow or method
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _workflowOrMethod;

        /// <summary>
        /// A flag identifying if the _workflowOrMethod field is a workflow
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _isWorkflow;

        /// <summary>
        /// KeyValuePair containing the key and name of the execute date
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _executeDate;

        #region Public Properties

        /// <summary>
        /// Gets the flag identifying if the workflow or method has been set.
        /// </summary>
        public bool WorkflowOrMethodIsSet
        {
            get { return !_workflowOrMethod.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with workflow or method key and name
        /// </summary>
        public KeyValuePair<int, string> WorkflowOrMethod { get { return _workflowOrMethod; } set { _workflowOrMethod = value; } }

        /// <summary>
        /// A flag identifying if the _workflowOrMethod field is a workflow
        /// </summary>
        public bool IsWorkflow
        {
            get { return _isWorkflow; }
        }

        /// <summary>
        /// Gets the flag identifying if the execute date has been set.
        /// </summary>
        public bool ExecuteDateIsSet
        {
            get { return !_executeDate.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with execute date key and name
        /// </summary>
        public KeyValuePair<int, string> ExecuteDate { get { return _executeDate; } set { _executeDate = value; } }

        #endregion

        public ROTaskForecastMerchandiseWorkflowMethod(
            KeyValuePair<int, string> workflowOrMethod = default(KeyValuePair<int, string>),
            bool isWorkflow = true,
            KeyValuePair<int, string> executeDate = default(KeyValuePair<int, string>)
            )
        {
            _workflowOrMethod = workflowOrMethod;
            _isWorkflow = isWorkflow;
            _executeDate = executeDate;
        }
    }

    [DataContract(Name = "ROTaskRollupMerchandise", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskRollupMerchandise
    {
        /// <summary>
        /// KeyValuePair containing the key and name of the merchandise
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _merchandise;

        /// <summary>
        /// KeyValuePair containing the key and name of the version
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _version;

        /// <summary>
        /// KeyValuePair containing the key and name of the date range
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _dateRange;

        /// <summary>
        /// List of KeyValuePair containing the hierarchy and sequence for possible from and to levels
        /// </summary>
        [DataMember(IsRequired = true)]
        protected List<KeyValuePair<int, string>> _hierarchyLevels;

        /// <summary>
        /// KeyValuePair containing the hierarchy and sequence of the from level
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _fromLevel;

        /// <summary>
        /// KeyValuePair containing the hierarchy and sequence of the from level
        /// </summary>
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _toLevel;

        /// <summary>
        /// Flag containing if a Posting Rollup is to be performed
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _rollPosting;

        /// <summary>
        /// Flag containing if a Reclass Rollup is to be performed
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _rollReclass;

        /// <summary>
        /// Flag containing if a Hierarchy Levels Rollup is to be performed
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _rollHierarchyLevels;

        /// <summary>
        /// Flag containing if a Day to Week Rollup is to be performed
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _rollDayToWeek;

        /// <summary>
        /// Flag containing if Days are to be rolled
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _rollDay;

        /// <summary>
        /// Flag containing if Weeks are to be rolled
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _rollWeek;

        /// <summary>
        /// Flag containing if Stores are to be rolled
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _rollStore;

        /// <summary>
        /// Flag containing if Chain is to be rolled
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _rollChain;

        /// <summary>
        /// Flag containing if a Store to Chain Rollup is to be performed
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _rollStoreToChain;

        /// <summary>
        /// Flag containing if an Intransit Rollup is to be performed
        /// </summary>
        [DataMember(IsRequired = true)]
        protected bool _rollIntransit;

        #region Public Properties

        /// <summary>
        /// KeyValuePair with merchandise key and name
        /// </summary>
        public KeyValuePair<int, string> Merchandise { get { return _merchandise; } set { _merchandise = value; } }

        /// <summary>
        /// Gets the flag identifying if the merchandise has been set.
        /// </summary>
        public bool MerchandiseIsSet
        {
            get { return !_merchandise.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets the flag identifying if the version has been set.
        /// </summary>
        public bool VersionIsSet
        {
            get { return !_version.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with version key and name
        /// </summary>
        public KeyValuePair<int, string> Version { get { return _version; } set { _version = value; } }

        /// <summary>
        /// Gets the flag identifying if the date range has been set.
        /// </summary>
        public bool DateRangeIsSet
        {
            get { return !_dateRange.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with date range key and name
        /// </summary>
        public KeyValuePair<int, string> DateRange { get { return _dateRange; } set { _dateRange = value; } }

        /// <summary>
        /// List of KeyValuePair containing the hierarchy and sequence for possible from levels
        /// </summary>
        public List<KeyValuePair<int, string>> HierarchyLevels { get { return _hierarchyLevels; } }

        /// <summary>
        /// Gets the flag identifying if the from level has been set.
        /// </summary>
        public bool FromLevelIsSet
        {
            get { return !_fromLevel.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with from level hierarchy and level sequence
        /// </summary>
        public KeyValuePair<int, string> FromLevel { get { return _fromLevel; } set { _fromLevel = value; } }

        /// <summary>
        /// Gets the flag identifying if the to level has been set.
        /// </summary>
        public bool ToLevelIsSet
        {
            get { return !_toLevel.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// KeyValuePair with to level hierarchy and level sequence
        /// </summary>
        public KeyValuePair<int, string> ToLevel { get { return _toLevel; } set { _toLevel = value; } }

        /// <summary>
        /// Flag containing if a Posting Rollup is to be performed
        /// </summary>
        public bool RollPosting { get { return _rollPosting; } set { _rollPosting = value; } }

        /// <summary>
        /// Flag containing if a Reclass Rollup is to be performed
        /// </summary>
        public bool RollReclass { get { return _rollReclass; } set { _rollReclass = value; } }

        /// <summary>
        /// Flag containing if a Hierarchy Levels Rollup is to be performed
        /// </summary>
        public bool RollHierarchyLevels { get { return _rollHierarchyLevels; } set { _rollHierarchyLevels = value; } }

        /// <summary>
        /// Flag containing if a Day to Week Rollup is to be performed
        /// </summary>
        public bool RollDayToWeek { get { return _rollDayToWeek; } set { _rollDayToWeek = value; } }

        /// <summary>
        /// Flag containing if Days are to be rolled
        /// </summary>
        public bool RollDay { get { return _rollDay; } set { _rollDay = value; } }

        /// <summary>
        /// Flag containing if Weeks are to be rolled
        /// </summary>
        public bool RollWeek { get { return _rollWeek; } set { _rollWeek = value; } }

        /// <summary>
        /// Flag containing if Stores are to be rolled
        /// </summary>
        public bool RollStore { get { return _rollStore; } set { _rollStore = value; } }

        /// <summary>
        /// Flag containing if Chain is to be rolled
        /// </summary>
        public bool RollChain { get { return _rollChain; } set { _rollChain = value; } }

        /// <summary>
        /// Flag containing if a Store to Chain Rollup is to be performed
        /// </summary>
        public bool RollStoreToChain { get { return _rollStoreToChain; } set { _rollStoreToChain = value; } }

        /// <summary>
        /// Flag containing if an Intransit Rollup is to be performed
        /// </summary>
        public bool RollIntransit { get { return _rollIntransit; } set { _rollIntransit = value; } }

        #endregion

        public ROTaskRollupMerchandise(
            KeyValuePair<int, string> merchandise = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> version = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> dateRange = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> fromLevel = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> toLevel = default(KeyValuePair<int, string>),
            bool rollPosting = false,
            bool rollReclass = false,
            bool rollHierarchyLevels = false,
            bool rollDayToWeek = false,
            bool rollDay = false,
            bool rollWeek = false,
            bool rollStore = false,
            bool rollChain = false,
            bool rollStoreToChain = false,
            bool rollIntransit = false
            )
        {
            _merchandise = merchandise;
            _version = version;
            _dateRange = dateRange;
            _fromLevel = fromLevel;
            _toLevel = toLevel;
            _rollPosting = rollPosting;
            _rollReclass = rollReclass;
            _rollHierarchyLevels = rollHierarchyLevels;
            _rollDayToWeek = rollDayToWeek;
            _rollDay = rollDay;
            _rollWeek = rollWeek;
            _rollStore = rollStore;
            _rollChain = rollChain;
            _rollStoreToChain = rollStoreToChain;
            _rollIntransit = rollIntransit;
            _hierarchyLevels = new List<KeyValuePair<int, string>>();

        }
    }

    [DataContract(Name = "ROTaskRollup", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskRollup : ROTaskProperties
    {
        /// <summary>
        /// List of merchandise entries
        /// </summary>
        [DataMember(IsRequired = true)]
        private List<ROTaskRollupMerchandise> _merchandise;

        /// <summary>
        /// List of KeyValuePairs for version with version key and name
        /// </summary>
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _versions;

        #region Public Properties
        /// <summary>
        /// KeyValuePair with merchandise key and name
        /// </summary>
        public List<ROTaskRollupMerchandise> Merchandise { get { return _merchandise; } }

        /// <summary>
        /// Gets the flag identifying if there are merchandise values.
        /// </summary>
        public bool HasMerchandiseValues
        {
            get { return _merchandise.Count > 0; }
        }

        /// <summary>
        /// List of KeyValuePairs for version with version key and name
        /// </summary>
        public List<KeyValuePair<int, string>> Versions { get { return _versions; } }

        /// <summary>
        /// Gets the flag identifying if there are versions.
        /// </summary>
        public bool HasVersions
        {
            get { return _versions.Count > 0; }
        }

        #endregion

        public ROTaskRollup(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.Rollup,
            maximumMessageLevel: maximumMessageLevel)

        {
            _merchandise = new List<ROTaskRollupMerchandise>();
            _versions = new List<KeyValuePair<int, string>>();
        }
    }

    [DataContract(Name = "ROTaskRelieveIntransit", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskRelieveIntransit : ROTaskLoad
    {

        #region Public Properties

        #endregion

        public ROTaskRelieveIntransit(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.RelieveIntransit,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskPurge", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskPurge : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskPurge(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.Purge,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskExternalProgram", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskExternalProgram : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskExternalProgram(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.ExternalProgram,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskSizeCurveLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskSizeCurveLoad : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskSizeCurveLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.SizeCurveLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskSQLScript", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskSQLScript : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskSQLScript(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.SQLScript,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskSizeConstraintsLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskSizeConstraintsLoad : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskSizeConstraintsLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.SizeConstraintsLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskSizeCurveMethod", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskSizeCurveMethod : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskSizeCurveMethod(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.SizeCurveMethod,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskSizeCurves", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskSizeCurves : ROTaskProperties
    {
        [DataMember(IsRequired = true)]
        protected List<KeyValuePair<int, string>> _merchandise;

        #region Public Properties
        public List<KeyValuePair<int, string>> Merchandise { get { return _merchandise; } set { _merchandise = value; } }
        #endregion

        public ROTaskSizeCurves(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.SizeCurves,
            maximumMessageLevel: maximumMessageLevel)

        {
            _merchandise = new List<KeyValuePair<int, string>>();
        }
    }

    [DataContract(Name = "ROTaskSizeDayToWeekSummary", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskSizeDayToWeekSummary : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskSizeDayToWeekSummary(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.SizeDayToWeekSummary,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskBuildPackCriteriaLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskBuildPackCriteriaLoad : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskBuildPackCriteriaLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.BuildPackCriteriaLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskChainSetPercentCriteriaLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskChainSetPercentCriteriaLoad : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskChainSetPercentCriteriaLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.ChainSetPercentCriteriaLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskPushToBackStockLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskPushToBackStockLoad : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskPushToBackStockLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.PushToBackStockLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskDailyPercentagesCriteriaLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskDailyPercentagesCriteriaLoad : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskDailyPercentagesCriteriaLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.DailyPercentagesCriteriaLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskStoreEligibilityCriteriaLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskStoreEligibilityCriteriaLoad : ROTaskLoad
    {

        #region Public Properties

        #endregion

        public ROTaskStoreEligibilityCriteriaLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.StoreEligibilityCriteriaLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskVSWCriteriaLoad", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskVSWCriteriaLoad : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskVSWCriteriaLoad(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.VSWCriteriaLoad,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskHeaderReconcile", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskHeaderReconcile : ROTaskProperties
    {
        [DataMember(IsRequired = true)]
        private string _inputDirectory;

        [DataMember(IsRequired = true)]
        private string _outputDirectory;

        [DataMember(IsRequired = true)]
        private string _triggerSuffix;

        [DataMember(IsRequired = true)]
        private string _removeTransactionFileName;

        [DataMember(IsRequired = true)]
        private string _removeTransactionTriggerSuffix;

        [DataMember(IsRequired = true)]
        private string _headerTypes;

        [DataMember(IsRequired = true)]
        private string _headerFileName;

        #region Public Properties
        public string InputDirectory
        {
            get { return _inputDirectory; }
            set { _inputDirectory = value; }
        }

        public string OutputDirectory
        {
            get { return _outputDirectory; }
            set { _outputDirectory = value; }
        }

        public string TriggerSuffix
        {
            get { return _triggerSuffix; }
            set { _triggerSuffix = value; }
        }

        public string RemoveTransactionFileName
        {
            get { return _removeTransactionFileName; }
            set { _removeTransactionFileName = value; }
        }
        public string RemoveTransactionTriggerSuffix
        {
            get { return _removeTransactionTriggerSuffix; }
            set { _removeTransactionTriggerSuffix = value; }
        }
        public string HeaderTypes
        {
            get { return _headerTypes; }
            set { _headerTypes = value; }
        }
        public string HeaderFileName
        {
            get { return _headerFileName; }
            set { _headerFileName = value; }
        }
        #endregion

        public ROTaskHeaderReconcile(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.HeaderReconcile,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskBatchCompute", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskBatchCompute : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskBatchCompute(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.BatchComp,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskChainForecasting", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskChainForecasting : ROTaskProperties
    {

        #region Public Properties

        #endregion

        public ROTaskChainForecasting(
            KeyValuePair<int, string> task,
            KeyValuePair<int, string> maximumMessageLevel) :
            base(task: task,
                taskType: eTaskType.computationDriver,
            maximumMessageLevel: maximumMessageLevel)

        {

        }
    }

    [DataContract(Name = "ROTaskListProperties", Namespace = "http://Logility.ROWeb/")]
    /// <summary>
    /// Base class for all task lists
    /// </summary>
    public class ROTaskListProperties : ROBaseProperties
    {
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _taskList;

        [DataMember(IsRequired = true)]
        protected int _userKey;

        [DataMember(IsRequired = true)]
        protected List<ROTaskProperties> _tasks;

        public ROTaskListProperties(KeyValuePair<int, string> taskList,int userKey)
        {
            _taskList = taskList;
            _userKey = userKey;
            _tasks = new List<ROTaskProperties>();
        }

        public KeyValuePair<int, string> TaskList
        {
            get { return _taskList; }
            set { _taskList = value; }
        }

        public bool AddingTaskList
        {
            get { return TaskList.Key == Include.NoRID; }
        }

        public bool UpdatingTaskList
        {
            get { return TaskList.Key != Include.NoRID; }
        }

        public int UserKey
        {
            get { return _userKey; }
            set { _userKey = value; }
        }


        public eGlobalUserType GlobalUserType
        {
            get
            {
                if (UserKey == Include.GetGlobalUserRID())
                {
                    return eGlobalUserType.Global;
                }
                else
                {
                    return eGlobalUserType.User;
                }
            }
        }

        public List<ROTaskProperties> Tasks
        {
            get { return _tasks; }
        }
    }

    
}