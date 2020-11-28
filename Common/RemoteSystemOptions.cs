using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
    public static class RemoteSystemOptions
    {
          //Soft Text
        public struct Messages
        {
            public static string ShutdownDisplayMessagePrefix = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_ShutdownDisplayMessagePrefix)); //"Shutdown command issued from control service: "
            public static string MessageForClientTitle = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_MessageForClientTitle)); //"Message From Control Service:"
            public static string BatchOnlyModeOnLastChangedByPrefix = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeOnLastChangedByPrefix)); //"Batch Only Mode turned on: "
            public static string BatchOnlyModeOffLastChangedByPrefix = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeOffLastChangedByPrefix)); //"Batch Only Mode turned off: "
            //public static string NotAvailableRunningLocal = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_NotAvailableRunningLocal)); //"Running Local - No system functions are currently available.";
            //public static string NotAvailableNotConnected = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_NotAvailableNotConnected)); //"Not connected to control service - No system functions are currently available."
            //public static string LogOutCommandSent = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_LogOutCommandSent)); //"Log Out command has been sent to selected users."
            //public static string BatchOnlyModeTurnedOn = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeTurnedOn)); //"Batch Only Mode is ON"
            //public static string BatchOnlyModeTurnedOff = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeTurnedOff)); //"Batch Only Mode is OFF";
            //public static string BatchOnlyModeAlreadyOnPrefix = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeAlreadyOnPrefix)); //"Batch Only Mode has already been turned ON by: "
            //public static string BatchOnlyModeAlreadyOffPrefix = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeAlreadyOffPrefix)); //"Batch Only Mode has already been turned OFF by: ";
            //public static string BatchOnlyModeWillTurnOn = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeWillTurnOn)); //"Batch Mode will be turned ON.  Please wait while client applications shut down."
            //public static string BatchOnlyModeWillTurnOff = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeWillTurnOff)); //"Batch Mode will be turned OFF."
            //public static string MessageSent = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_MessageSent)); //"Message has been sent to clients."
            //public static string UsersLoggedInSuffix = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_UsersLoggedInSuffix)); //" user(s) logged in"
        }
        //public struct Labels
        //{
            //public static string ShowCurrentUserGrid_UserColumn = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ShowCurrentUserGrid_UserColumn)); //"User"
            //public static string ShowCurrentUserGrid_MachineColumn = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ShowCurrentUserGrid_MachineColumn)); //"Machine"
            //public static string ShowCurrentUserGrid_IPAddressColumn = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ShowCurrentUserGrid_IPAddressColumn)); //"IP Address"
            //public static string gbLoginOptions = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_gbLoginOptions)); //"Login Options"
            //public static string ckbUseWindowsLogin = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ckbUseWindowsLogin)); //"Use Windows Authentication"
            //public static string cbxForceSingleClientInstance = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_cbxForceSingleClientInstance)); //"Enforce Single Instance"
            //public static string cbxForceSingleUserInstance = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_cbxForceSingleUserInstance)); //"Enforce Single User Login"
            //public static string cbxEnableRemoteSystemOptions = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_cbxEnableRemoteSystemOptions)); //"Enable Remote System Options"
            //public static string cbxControlServiceDefaultBatchOnlyModeOn = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_cbxControlServiceDefaultBatchOnlyModeOn)); //"Start with Batch Only Mode on"
            //public static string btnBatchModeTurnOn = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_btnBatchModeTurnOn)); //"Turn On"
            //public static string btnBatchModeTurnOff = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_btnBatchModeTurnOff)); //"Turn Off"
            //public static string btnSendMsg = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_btnSendMsg)); //"Send Msg"
        //public static string lblMessageForClients = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_lblMessageForClients)); //"Message for clients:"
            //public static string lblLastChangedBy = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_lblLastChangedBy)); //"Last Changed:"
            //public static string gbCurrentUsers = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_gbCurrentUsers)); //"Current Users"
            //public static string btnShowCurrentUsers = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_btnShowCurrentUsers)); //"Show Current Users"
           // public static string btnLogOutSelected = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_btnLogOutSelected)); //"Log Out Selected Users"
        //}
        
        //public struct ValidationMessages
        //{
            //public static string SelectOneOrMoreUsers = ((int)eMIDTextCode.msg_RemoteSystemOptions_SelectOneOrMoreUsers).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_SelectOneOrMoreUsers)); //"Please select one or more users to log out."
           // public static string ProvideMessageToSend = ((int)eMIDTextCode.msg_RemoteSystemOptions_ProvideMessageToSend).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_ProvideMessageToSend)); //"Please type a message to send."
         
        //}
    }
}
