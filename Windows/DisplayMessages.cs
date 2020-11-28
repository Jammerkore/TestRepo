using System;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Business;

namespace MIDRetail.Windows
{
    /// <summary>
    /// Summary description for DisplayMessages.
    /// </summary>
    public class DisplayMessages
    {
        public DisplayMessages()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void Show(EditMsgs em, SessionAddressBlock sab, string label)
        {
            try
            {
                // Begin TT#3435 - JSmith - Errors messages not complete with encounter database errors.
                //string errors = sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FollowingErrors);
                //for (int i = 0; i < em.EditMessages.Count; i++)
                //{
                //    EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[i];
                //    errors += Environment.NewLine + "     ";
                //    if (emm.messageByCode &&
                //        emm.code != eMIDTextCode.Unassigned)
                //    {
                //        errors += sab.ClientServerSession.Audit.GetText(emm.code);
                //    }
                //    else
                //    {
                //        errors += emm.msg;
                //    }
                //}
                //MessageBox.Show(errors, label, MessageBoxButtons.OK, MessageBoxIcon.Error);

                eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
                string module = string.Empty;
                string errors = sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FollowingErrors, false);
                for (int i = 0; i < em.EditMessages.Count; i++)
                {
                    EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[i];
                    errors += Environment.NewLine + "     ";
                    if (emm.messageByCode &&
                        emm.code != eMIDTextCode.Unassigned)
                    {
                        errors += sab.ClientServerSession.Audit.GetText(emm.code);
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
                sab.ClientServerSession.Audit.Add_Msg(messageLevel, errors, module);

                MessageBox.Show(errors, label, MessageBoxButtons.OK, MessageBoxIcon.Error);
                // End TT#3435 - JSmith - Errors messages not complete with encounter database errors.
            }
            catch (Exception)
            {
                string errors = Include.ErrorDatabase;
                for (int i = 0; i < em.EditMessages.Count; i++)
                {
                    EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[i];
                    errors += Environment.NewLine + "     ";
                    if (!emm.messageByCode)
                    {
                        errors += emm.msg;
                    }
                }
                MessageBox.Show(errors, label, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Show(string text, bool requestFailed = true)
        {
            if (MIDEnvironment.isWindows)
            {
                MessageBox.Show(text);
            }
            else
            {
                MIDEnvironment.Message = text;
                if (requestFailed)
                {
                    MIDEnvironment.requestFailed = requestFailed;
                }
            }
        }

        public static void Show(string text, string caption, bool requestFailed = true)
        {
            if (MIDEnvironment.isWindows)
            {
                MessageBox.Show(text, caption);
            }
            else
            {
                MIDEnvironment.Message = text;
                if (requestFailed)
                {
                    MIDEnvironment.requestFailed = requestFailed;
                }
            }
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, bool requestFailed = true, DialogResult defaultResult = DialogResult.OK)
        {
            if (MIDEnvironment.isWindows)
            {
                return MessageBox.Show(text, caption, buttons, icon);
            }
            else
            {
                MIDEnvironment.Message = text;
                if (requestFailed)
                {
                    MIDEnvironment.requestFailed = requestFailed;
                }
                return defaultResult;
            }
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, bool requestFailed = true, DialogResult defaultResult = DialogResult.OK)
        {
            if (MIDEnvironment.isWindows)
            {
                return MessageBox.Show(owner, text, caption, buttons, icon);
            }
            else
            {
                MIDEnvironment.Message = text;
                if (requestFailed)
                {
                    MIDEnvironment.requestFailed = requestFailed;
                }
                return defaultResult;
            }
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, bool requestFailed = true, DialogResult defaultResult = DialogResult.OK)
        {
            if (MIDEnvironment.isWindows)
            {
                return MessageBox.Show(text, caption, buttons, icon, defaultButton);
            }
            else
            {
                MIDEnvironment.Message = text;
                if (requestFailed)
                {
                    MIDEnvironment.requestFailed = requestFailed;
                }
                return defaultResult;
            }
        }
    }
}
