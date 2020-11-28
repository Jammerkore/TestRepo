using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public partial class MessagingData : DataLayer
    {

        public MessagingData()
            : base()
		{
		}

        public DataTable Messages_Read(eMIDMessageSenderRecepient aRecepient)
        {
            try
            {
                return StoredProcedures.MID_MESSAGE_QUEUE_READ.Read(_dba, MESSAGE_TO: (int)aRecepient);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int Message_Add(eMIDMessageSenderRecepient aMessageTo, eMIDMessageSenderRecepient aMessageFrom, eMIDMessageCode aMessageCode, int aMessageProcessingPriority, string aMessageDetails)
        {
            int messageRID = Include.NoRID;
            try
            {
                messageRID = StoredProcedures.SP_MID_MESSAGE_QUEUE_INSERT.InsertAndReturnRID(_dba,
                                                                                             MESSAGE_TO: (int)aMessageTo,
                                                                                             MESSAGE_FROM: (int)aMessageFrom,
                                                                                             MESSAGE_CODE: (int)aMessageCode,
                                                                                             MESSAGE_PROCESSING_PRIORITY: aMessageProcessingPriority,
                                                                                             MESSAGE_DETAILS: aMessageDetails
                                                                                             );

            }
            catch
            {
                throw;
            }
            return messageRID;
        }

        public bool Message_Delete(int aMessageRID)
        {
            bool deleteSuccessful = false;
            try
            {
                StoredProcedures.MID_MESSAGE_QUEUE_DELETE.Delete(_dba, MESSAGE_RID: aMessageRID);
                deleteSuccessful = true;
            }
            catch
            {
                deleteSuccessful = false;
            }
            return deleteSuccessful;
        }
    }
}
