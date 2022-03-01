using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using System.Reflection;

namespace Logility.ROWeb
{
    public class ApplicationUtilities
    {
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;

        public ApplicationUtilities(SessionAddressBlock SAB, ROWebTools RoWebTools)
        {
            _SAB = SAB;
            _ROWebTools = RoWebTools;
        }

        internal static bool AllowDeleteFromInUse(
            int key, 
            eProfileType profileType, 
            SessionAddressBlock SAB, 
            List<string> acceptableConflicts = null
            )
        {
            ROInUse ROInUse = InUse.CheckInUse(
                itemProfileType: profileType, 
                key: key, 
                inQuiry: false,
                acceptableConflicts: acceptableConflicts
                );

            if (!ROInUse.AllowDelete)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                    messageCode: eMIDTextCode.msg_DeleteInUseWarning,
                    addToAuditReport: true
                    );
                MIDEnvironment.requestFailed = true;
            }

            return ROInUse.AllowDelete;
        }

    }
}
