using System;
using System.Collections.Generic;
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
    // BEGIN TT#1156-MD CTeegarden - make cube functionality generic
    abstract public class ROWebFunction 
    // END TT#1156-MD CTeegarden - make cube functionality generic
    {
        //=======
		// FIELDS
		//=======

        private long _ROInstanceID;
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private HierarchyMaintenance _hierMaint = null;
        private HierarchyProfile _mainHp = null;
        private FunctionSecurityProfile _functionSecurity;
        private eROClass _roClass;
        private eROReturnCode _returnCode = eROReturnCode.Successful;
        private string _returnMessage = null;

        //=============
        // CONSTRUCTORS
        //=============

        /// <summary>
        /// Creates a new instace of ROWebFunction.
        /// </summary>

        public ROWebFunction(SessionAddressBlock SAB, ROWebTools RoWebTools)
		{
            _ROInstanceID = DateTime.Now.Ticks;
            _SAB = SAB;
            _ROWebTools = RoWebTools;
		}

		//===========
		// PROPERTIES
		//===========

        /// <summary>
        /// Gets the enumeration of the class
        /// </summary>
        public eROClass ROClass
        {
            get { return _roClass; }
            set { _roClass = value; }
        }

        /// <summary>
        /// Gets the unique function ID
        /// </summary>
        public long ROInstanceID
        {
            get { return _ROInstanceID; }
        }

        // BEGIN  TT#1156-MD - CTeegarden - clean up global defaults web function
        public static long InvalidInstanceID
        {
            get { return -1; }
        }
        // END  TT#1156-MD - CTeegarden - clean up global defaults web function
        /// <summary>
        /// Gets the SessionAddressBlock
        /// </summary>
        public SessionAddressBlock SAB
        {
            get { return _SAB; }
        }

        /// <summary>
        /// Gets the ROWebTools
        /// </summary>
        public ROWebTools ROWebTools
        {
            get { return _ROWebTools; }
        }

        /// <summary>
        /// Gets or sets the function security profile
        /// </summary>
        public FunctionSecurityProfile FunctionSecurity
        {
            get { return _functionSecurity; }
            set { _functionSecurity = value; }
        }

        /// <summary>
        /// Gets or sets the function security profile
        /// </summary>
        public HierarchyMaintenance HierMaint
        {
            get
            {
                if (_hierMaint == null)
                {
                    _hierMaint = new HierarchyMaintenance(SAB);
                }
                return _hierMaint;
            }
        }
		
		public HierarchyProfile MainHp
        {
            get
            {
                if (_mainHp == null)
                {
                    _mainHp = SAB.HierarchyServerSession.GetMainHierarchyData();
                }
                return _mainHp;
            }
        }

        ///// <summary>
        ///// Gets or sets the return code of the request
        ///// </summary>
        //public eROReturnCode ReturnCode
        //{
        //    get { return _returnCode; }
        //    set { _returnCode = value; }
        //}

        ///// <summary>
        ///// Gets or sets the return message of the request
        ///// </summary>
        //public string ReturnMessage
        //{
        //    get { return _returnMessage; }
        //    set { _returnMessage = value; }
        //}

        //========
        // METHODS
        //========

        override public bool Equals(object obj)
		{
            if (((ROWebFunction)obj).ROInstanceID == _ROInstanceID)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        override public int GetHashCode()
        {
            return (int)this.ROInstanceID;
        }

        abstract public void CleanUp();

        virtual public ROOut ProcessRequest(ROParms Parms)
        {
            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        protected string FormatMessage (EditMsgs em)
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
            string module = string.Empty;
            string errors = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FollowingErrors, false);
            for (int i = 0; i < em.EditMessages.Count; i++)
            {
                EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[i];
                errors += Environment.NewLine + "     ";
                if (emm.messageByCode &&
                    emm.code != eMIDTextCode.Unassigned)
                {
                    errors += SAB.ClientServerSession.Audit.GetText(emm.code);
                }
                else
                {
                    errors += emm.msg;
                }

                if (emm.messageLevel > messageLevel)
                {
                    messageLevel = emm.messageLevel;
                }
                if (module == string.Empty)
                {
                    module = emm.module;
                }
            }
            SAB.ClientServerSession.Audit.Add_Msg(messageLevel, errors, module);

            return errors;
        }

        protected GenericEnqueue EnqueueObject(string action, string name, eLockType lockType, int key, ref string errMsg)
        {
            GenericEnqueue objEnqueue;
            errMsg = null;

            try
            {
                objEnqueue = new GenericEnqueue(lockType, key, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

                try
                {
                    objEnqueue.EnqueueGeneric();
                }
                catch (GenericConflictException)
                {
                    string[] errParms = new string[3];
                    errParms.SetValue(action, 0);
                    errParms.SetValue(name.Trim(), 1);
                    errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
                    errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

                    objEnqueue = null;
                }

                return objEnqueue;
            }
            catch
            {
                throw;
            }
        }

    }

    class KeyValueList
    {
        public string TKey { get; set; }
        public List<ValueListItem> TValues { get; set; }

    }

    class ValueListItem
    {
        public int id { get; set; }
        public string value { get; set; }
    }
}
