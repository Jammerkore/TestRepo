using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
    // The delegate for message notifications.
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);

    /// <summary>
    /// Class to provide message handling.
    /// </summary>
    /// <remarks>
    /// To utilize the MessageProcessor class, 
    ///    1. Create an instance of the class
    ///    2. Add the event handler and method
    ///    3. Start the message listener.
    ///    4. Perform tasks
    ///    5. Stop the message listener
    ///    
    /// Messages can be added using SendMessage.
    /// Messages should be removed once handled. Use RemoveMessage(e.MessageRID) with the MessageRID in the EventArgs.
    /// </remarks>
    /// <example>
    /// messageProcessor = new MessageProcessor(aRecepient, aMessagingInterval)
    /// messageProcessor.Messaging.OnMessageSentHandler += new MessageEvent.MessageEventHandler(Messaging_OnMessageSentHandler);
    /// messageProcessor.StartMessageListener();
    /// ... your code here
    /// messageProcessor.StopMessageListener();
    /// </example>
    public class MessageProcessor
    {
        private System.Threading.Thread _messageThread = null;
        private bool _listenForMessages = true;
        private int _messagingInterval = 15000;
        private eMIDMessageSenderRecepient _recepient;
        private MessageEvent _messaging;
        private bool _listeningForMessages;

        /// <summary>
        /// Creates an instance of the MessageProcessor class
        /// </summary>
        /// <param name="aRecepient">The eMIDMessageSenderRecepient values associated with the process listening for messages</param>
        /// <param name="aMessagingInterval">The delay interval between checking for messages</param>
        public MessageProcessor(eMIDMessageSenderRecepient aRecepient, int aMessagingInterval)
        {
            _recepient = aRecepient;
            _messaging = new MessageEvent();
            if (aMessagingInterval != Include.Undefined)
            {
                _messagingInterval = aMessagingInterval;
            }
        }

        /// <summary>
        /// Messaging event class
        /// </summary>
        /// <remarks>
        /// This class contains the event handler to catch the messages
        /// </remarks>
        public MessageEvent Messaging
        {
            get { return _messaging; }
        }

        /// <summary>
        /// Return the recepient for whom the listener is getting messages
        /// </summary>
        public eMIDMessageSenderRecepient Recepient
        {
            get { return _recepient; }
        }

        /// <summary>
        /// Return flag identifying if message process is listening for messages
        /// </summary>
        public bool isListeningForMessages
        {
            get { return _listeningForMessages | (_messageThread != null && _messageThread.IsAlive); }
        }

        /// <summary>
        /// Starts the message processor listening for messages
        /// </summary>
        public void StartMessageListener()
        {
            _messageThread = new System.Threading.Thread(new System.Threading.ThreadStart(ListenForMessages));
            _messageThread.Start();
        }

        /// <summary>
        /// Stops the message processor listening for messages
        /// </summary>
        public void StopMessageListener()
        {
            // set flag to stop loop
            _listenForMessages = false;
            // stop thread if still alive
            if (_messageThread != null &&
                _messageThread.IsAlive)
            {
                _messageThread.Abort();
                // wait for thread to exit
                _messageThread.Join();
            }
        }

        /// <summary>
        /// Method processed within thread that checks for messages for this recepient.
        /// It will fire an event for each message found.
        /// </summary>
        private void ListenForMessages()
        {
            DataTable dt;
            MessagingData messageData;
            try
            {
                _listeningForMessages = true;
                messageData = new MessagingData();
                while (_listenForMessages)
                {
                    // Begin TT#585-MD - JSmith - Hierarchy Service fails if message thread cannot connect to database
                    //dt = messageData.Messages_Read(_recepient);
                    //foreach (DataRow message in dt.Rows)
                    //{
                    //    _messaging.SendMessage(this, 
                    //        Convert.ToInt32(message["MESSAGE_RID"]),
                    //        Convert.ToDateTime(message["MESSAGE_DATETIME"]),
                    //        (eMIDMessageSenderRecepient)Convert.ToInt32(message["MESSAGE_TO"]),
                    //        (eMIDMessageSenderRecepient)Convert.ToInt32(message["MESSAGE_FROM"]),
                    //        (eMIDMessageCode)Convert.ToInt32(message["MESSAGE_CODE"]),
                    //        Convert.ToInt32(message["MESSAGE_PROCESSING_PRIORITY"]),
                    //        Convert.ToString(message["MESSAGE_DETAILS"])
                    //        );
                    //}
                    try
                    {
                        dt = messageData.Messages_Read(_recepient);
                        foreach (DataRow message in dt.Rows)
                        {
                            _messaging.SendMessage(this,
                                Convert.ToInt32(message["MESSAGE_RID"]),
                                Convert.ToDateTime(message["MESSAGE_DATETIME"]),
                                (eMIDMessageSenderRecepient)Convert.ToInt32(message["MESSAGE_TO"]),
                                (eMIDMessageSenderRecepient)Convert.ToInt32(message["MESSAGE_FROM"]),
                                (eMIDMessageCode)Convert.ToInt32(message["MESSAGE_CODE"]),
                                Convert.ToInt32(message["MESSAGE_PROCESSING_PRIORITY"]),
                                Convert.ToString(message["MESSAGE_DETAILS"])
                                );
                        }
                    }
                    catch
                    {
                        // swallow error and try again later
                    }
                    // End TT#585-MD - JSmith - Hierarchy Service fails if message thread cannot connect to database

                    if (_listenForMessages)
                    {
                        System.Threading.Thread.Sleep(_messagingInterval);
                    }
                }

                _listeningForMessages = false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add message to be sent.
        /// </summary>
        /// <param name="aMessageTo">The eMIDMessageSenderRecepient associated with the process the message is sent to.</param>
        /// <param name="aMessageFrom">The eMIDMessageSenderRecepient associated with the process the message is sent from.</param>
        /// <param name="aMessageCode">The eMIDMessageCode associated with the message to be sent.</param>
        /// <param name="aMessageProcessingPriority">The priority of the message to be sent.</param>
        /// <param name="aMessageDetails">Any details needed for the message (i.e. a list of stores).</param>
        /// <returns></returns>
        public bool SendMessage(eMIDMessageSenderRecepient aMessageTo,
            eMIDMessageSenderRecepient aMessageFrom,
            eMIDMessageCode aMessageCode,
            int aMessageProcessingPriority,
            string aMessageDetails)
        {
            bool addSuccessful = false;
            MessagingData messageData = null;
            try
            {
                messageData = new MessagingData();
                messageData.OpenUpdateConnection();
                messageData.Message_Add(aMessageTo, aMessageFrom, aMessageCode, aMessageProcessingPriority, aMessageDetails);
                messageData.CommitData();
                addSuccessful = true;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (messageData != null &&
                    messageData.ConnectionIsOpen)
                {
                    messageData.CloseUpdateConnection();
                }
            }
            return addSuccessful;
        }

        /// <summary>
        /// Remove a message from the message queue
        /// </summary>
        /// <param name="aMessageRID">The record ID of the message to be removed.</param>
        /// <returns></returns>
        public bool RemoveMessage(int aMessageRID)
        {
            bool removeSuccessful = false;
            MessagingData messageData = null;
            try
            {
                messageData = new MessagingData();
                messageData.OpenUpdateConnection();
                messageData.Message_Delete(aMessageRID);
                messageData.CommitData();
                removeSuccessful = true;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (messageData != null &&
                    messageData.ConnectionIsOpen)
                {
                    messageData.CloseUpdateConnection();
                }
            }
            return removeSuccessful;
        }
    }

    public class MessageEvent
    {
        // add event to update menu
        public delegate void MessageEventHandler(object source, MessageEventArgs e);
        public event MessageEventHandler OnMessageSentHandler;

        public void SendMessage(
            object source, 
            int aMessageRID,
            DateTime aMessageDateTime,
            eMIDMessageSenderRecepient aMessageTo,
            eMIDMessageSenderRecepient aMessageFrom,
            eMIDMessageCode aMessageCode,
            int aMessageProcessingPriority,
            string aMessageDetails)
        {
            MessageEventArgs ea;
            // fire the event if handler is defined
            if (OnMessageSentHandler != null)
            {
                ea = new MessageEventArgs(
                    aMessageRID, 
                    aMessageDateTime, 
                    aMessageTo,
                    aMessageFrom,
                    aMessageCode,
                    aMessageProcessingPriority,
                    aMessageDetails);
                OnMessageSentHandler(source, ea);
            }
            return;
        }
    }

    public class MessageEventArgs : EventArgs
    {
        private int _messageRID;
	    private DateTime _messageDateTime;
	    private eMIDMessageSenderRecepient _messageTo;
	    private eMIDMessageSenderRecepient _messageFrom;
	    private eMIDMessageCode _messageCode;
	    private int _messageProcessingPriority;
	    private string _messageDetails;

        public MessageEventArgs(
            int aMessageRID, 
            DateTime aMessageDateTime,
            eMIDMessageSenderRecepient aMessageTo,
            eMIDMessageSenderRecepient aMessageFrom,
            eMIDMessageCode aMessageCode,
            int aMessageProcessingPriority,
            string aMessageDetails
            )
        {
            _messageRID = aMessageRID;
            _messageDateTime = aMessageDateTime;
            _messageTo = aMessageTo;
            _messageFrom = aMessageFrom;
            _messageCode = aMessageCode;
            _messageProcessingPriority = aMessageProcessingPriority;
            _messageDetails = aMessageDetails;
        }

        public int MessageRID
        {
            get { return _messageRID; }
            //set { _messageRID = value; }
        }

        public DateTime MessageDateTime
        {
            get { return _messageDateTime; }
            //set { _messageDateTime = value; }
        }

        public eMIDMessageSenderRecepient MessageTo
        {
            get { return _messageTo; }
            //set { _messageTo = value; }
        }

        public eMIDMessageSenderRecepient MessageFrom
        {
            get { return _messageFrom; }
            //set { _messageFrom = value; }
        }

        public eMIDMessageCode MessageCode
        {
            get { return _messageCode; }
            //set { _messageCode = value; }
        }

        public int MessageProcessingPriority
        {
            get { return _messageProcessingPriority; }
            //set { _messageProcessingPriority = value; }
        }

        public string MessageDetails
        {
            get { return _messageDetails; }
            //set { _messageDetails = value; }
        }
    }
}
