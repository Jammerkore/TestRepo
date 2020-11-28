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
    abstract public class HierarchyBase
    {
        //=======
        // FIELDS
        //=======
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private eProfileType _profileType;
        protected FunctionSecurityProfile _functionSecurity = null;
        private HierarchyMaintenance _hm = null;

        //=============
        // CONSTRUCTORS
        //=============
        public HierarchyBase(SessionAddressBlock SAB, ROWebTools ROWebTools, eProfileType profileType)
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _profileType = profileType;
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

        public eProfileType ProfileType
        {
            get
            {
                return _profileType;
            }
        }

        public FunctionSecurityProfile FunctionSecurity
        {
            get
            {
                if (_functionSecurity == null)
                {
                    _functionSecurity = new FunctionSecurityProfile(aKey: Include.NoRID);
                }
                return _functionSecurity;
            }
        }

        public HierarchyMaintenance HierarchyMaintenance
        {
            get
            {
                if (_hm == null)
                {
                    _hm = new HierarchyMaintenance(SAB, SAB.ClientServerSession);
                }
                return _hm;
            }
        }

        //========
        // METHODS
        //========

        /// <summary>
        /// Deletes all shortcuts for a key
        /// </summary>
        /// <param name="key">The key of the item being deleted</param>
        /// <param name="folderType">The folder type of the item being deleted</param>

        protected bool Folder_DeleteAll_Shortcut(int key, eProfileType folderType, ref string message)
        {
            try
            {
                FolderDataLayer DlFolder = new FolderDataLayer();

                DlFolder.OpenUpdateConnection();

                try
                {
                    DlFolder.Folder_Shortcut_DeleteAll(key, folderType);

                    DlFolder.CommitData();
                }
                catch (DatabaseForeignKeyViolation)
                {
                    message += SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                    return false;
                }
                catch (Exception exc)
                {
                    message += exc.ToString();
                    return false;
                }
                finally
                {
                    DlFolder.CloseUpdateConnection();
                }
            }
            catch (Exception exc)
            {
                message += exc.ToString();
                return false;
            }

            return true;
        }

        protected void BuildMessage(EditMsgs em, ref string message)
        {
            for (int i = 0; i < em.EditMessages.Count; i++)
            {
                EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[i];
                AddErrorMessage(emm, ref message);
            }
        }

        private string AddErrorMessage(EditMsgs.Message emm, ref string message)
        {
            string error = null;
            try
            {
                if (emm.code != 0)
                {
                    error = SAB.ClientServerSession.Audit.GetText(emm.code);
                }
                else
                {
                    error = emm.msg;
                }
                message += Environment.NewLine + "     " + error;
                return error;
            }
            catch (Exception exception)
            {
                message += Environment.NewLine + "     " + exception.Message;
                return error;
            }
        }

    }
}
