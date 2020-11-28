using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Configuration;
using System.Timers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;
using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.Windows
{

	public class UserActivityExplorer : ExplorerBase
	{
 
		private System.ComponentModel.IContainer components;
        private MIDRetail.Business.SessionAddressBlock _SAB;
        public UserActivityExplorer(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, Form aMainMDIForm)
			: base(aSAB, aEAB, aMainMDIForm)
		{
			aEAB.UserActivityExplorer = this;
            _SAB = aSAB;
		}

		protected override void Dispose(bool disposing)
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        private Windows.UserActivityControl ucUserActivityControl;

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.ucUserActivityControl = new Windows.UserActivityControl(); 
            
            this.SuspendLayout();
            this.ucUserActivityControl.Dock = DockStyle.Fill;
            // 
            // UserActivityExplorer
            // 
            this.AllowDrop = false;
            this.Controls.Add(this.ucUserActivityControl);
        
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.Name = "UserActivityExplorer";
            this.Size = new System.Drawing.Size(1155, 344);
            this.Load += new System.EventHandler(this.UserActivityExplorer_Load);
            this.Enter += new System.EventHandler(this.UserActivityExplorer_Enter);
            this.Leave += new System.EventHandler(this.UserActivityExplorer_Leave);
            this.Controls.SetChildIndex(this.ucUserActivityControl, 0);
            this.ResumeLayout(false);
            this.PerformLayout();
		}

      

		#endregion

        protected override void InitializeExplorer()
        {
            try
            {
                base.InitializeExplorer();

                InitializeComponent();

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddMessageHandler(object source, LogUserDashboardEventArgs e)
        {
            Image msgImage;
            switch (e.MessageLevel)
            {
                case eMIDMessageLevel.Severe:
                    msgImage = MIDGraphics.GetImage(MIDGraphics.msgSevereImage);

                    break;
                case eMIDMessageLevel.Error:
                    msgImage = MIDGraphics.GetImage(MIDGraphics.msgErrorImage);
           
                    break;
                case eMIDMessageLevel.Warning:
       
                    msgImage = MIDGraphics.GetImage(MIDGraphics.msgWarningImage);
                 
                    break;
                case eMIDMessageLevel.Edit:
                      
                    msgImage = MIDGraphics.GetImage(MIDGraphics.msgEditImage);
                    
                    break;
                case eMIDMessageLevel.Debug: //TT#595-MD -jsobek -Add Debug to My Activity level 

                    msgImage = MIDGraphics.GetImage(MIDGraphics.msgDebugImage);

                    break;
                default:
                    msgImage = MIDGraphics.GetImage(MIDGraphics.msgOKImage);
                       
                    break;
            }
            if (e.MessageCode != eMIDTextCode.Unassigned &&
                e.Message == null)
            {
                e.Message = MIDText.GetText(e.MessageCode);
            }

            this.ucUserActivityControl.AddMessage(e.Time, e.Module, MIDText.GetTextOnly(Convert.ToInt32(e.MessageLevel)), e.Message, e.MessageDetails, msgImage, Convert.ToInt32(e.MessageLevel));
        }

        override protected void InitializeTreeView()
        {
        }
        override protected void ExplorerLoad()
        {
        }
        override protected void BuildTreeView()
        {
        }
		override protected void RefreshTreeView()
		{
		}
        override protected void CustomizeActionMenu(MIDTreeNode aNode)
        {
        }
        override public void IRefresh()
        {
        }


        private void DisplayMessages(EditMsgs em)
        {
            MIDRetail.Windows.DisplayMessages.Show(em, SAB, MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Activity_Monitor));
        }


        private void UserActivityExplorer_Enter(object sender, EventArgs e)
        { 
           //HideMenuItem(this, eMIDMenuItem.FileSave);
           //HideMenuItem(this, eMIDMenuItem.FileSaveAs);
        }

        private void UserActivityExplorer_Leave(object sender, EventArgs e)
        {
            //ShowMenuItem(this, eMIDMenuItem.EditClear);
            //ShowMenuItem(this, eMIDMenuItem.FileSave);
            //ShowMenuItem(this, eMIDMenuItem.FileSaveAs);
            //EnableMenuItem(this, eMIDMenuItem.EditDelete);
        }

        


        private void UserActivityExplorer_Load(object sender, EventArgs e)
        {
            //The Activity Explorer load event fires AFTER the the User Activity Control.  This is important to know when saving/loading settings.
            SAB.ClientServerSession.Audit.LogUserDashboardEvent.OnLogUserDashboardHandler += new LogUserDashboardEvent.LogUserDashboardEventHandler(AddMessageHandler);
            SAB.ApplicationServerSession.Audit.LogUserDashboardEvent.OnLogUserDashboardHandler += new LogUserDashboardEvent.LogUserDashboardEventHandler(AddMessageHandler);


       



            // check for saved grid layout
            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
           

            // check for saved toolbar manager layout
            InfragisticsLayout toolbarManagerLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.userActivityToolbars);
            if (toolbarManagerLayout.LayoutLength > 0)
            {
                string currentVersion = (string)this.ucUserActivityControl.myActivityToolbarManager.ToolbarSettings.Tag; 

                //This would be faster if we could save the version in the database so we would not have to load the layout to get the version
                Infragistics.Win.UltraWinToolbars.UltraToolbarsManager tempToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager();
                tempToolbarManager.LoadFromBinary(toolbarManagerLayout.LayoutStream);
                string savedVersion = (string)tempToolbarManager.ToolbarSettings.Tag;

                if (savedVersion == currentVersion)
                {
                   toolbarManagerLayout.LayoutStream.Position = 0;
                   this.ucUserActivityControl.myActivityToolbarManager.LoadFromBinary(toolbarManagerLayout.LayoutStream);
                   this.ucUserActivityControl.LoadChartSettings();


                   InfragisticsLayout gridLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.userActivityGrid);
                   if (gridLayout.LayoutLength > 0)
                   {
                       //string currentVersion = (string)this.ucUserActivityControl.myActivityGrid.Tag;

                       ////This would be faster if we could save the version in the database so we would not have to load the layout to get the version
                       //Infragistics.Win.UltraWinGrid.UltraGrid tempGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
                       //tempGrid.DisplayLayout.Load(gridLayout.LayoutStream);
                       //string savedVersion = (string)tempGrid.Tag;

                       //if (savedVersion == currentVersion)
                       //{

                           //using the toolbar to determine whether or not the load the grid 
                           this.ucUserActivityControl.myActivityGrid.DisplayLayout.Load(gridLayout.LayoutStream);
                           this.ucUserActivityControl.LoadGridDataSettings(); 



                       //}

                   }


                }

            }


          




            //get the upper limit allowed for the messages from the global system options
            int messageUpperLimit = 100000;
            GlobalOptions globalOptions = new GlobalOptions();
            System.Data.DataTable dtGlobalOptions = globalOptions.GetGlobalOptions();
            if (dtGlobalOptions.Rows[0]["ACTIVITY_MESSAGE_UPPER_LIMIT"] != DBNull.Value)
            {
                messageUpperLimit = (int)dtGlobalOptions.Rows[0]["ACTIVITY_MESSAGE_UPPER_LIMIT"];
            }
            this.ucUserActivityControl.messageUpperLimit = messageUpperLimit;


            this.ucUserActivityControl.messageValidationClearWarningPrompt = ((int)eMIDTextCode.msg_MyActivity_ClearWarningPrompt).ToString() + ": " + MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_ClearWarningPrompt);
            this.ucUserActivityControl.messageValidationClearWarningPromptTitle =  MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_ClearWarningPromptTitle);
            this.ucUserActivityControl.messageValidationMaxMessageLimitHighestValue = ((int)eMIDTextCode.msg_MyActivity_MessageLimitHigh).ToString() + ": " + MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitHigh).Replace("{0}", messageUpperLimit.ToString("###,###,##0"));
            this.ucUserActivityControl.messageValidationMaxMessageLimitInvalidNumber = ((int)eMIDTextCode.msg_MyActivity_MessageLimitInvalid).ToString() + ": " + MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitInvalid);
            this.ucUserActivityControl.messageValidationMaxMessageLimitInvalidNumberTitle =  MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitInvalidTitle);
            this.ucUserActivityControl.messageValidationMaxMessageLimitLowestValue = ((int)eMIDTextCode.msg_MyActivity_MessageLimitLow).ToString() + ": " + MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitLow);
            this.ucUserActivityControl.messageValidationMaxMessageLimitRestoringOriginalMessage =  ((int)eMIDTextCode.msg_MyActivity_MessageLimitRestoringOriginal).ToString() + ": " + MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitRestoringOriginal);
          

            this.ucUserActivityControl.SaveSettingsEvent += new Windows.UserActivityControl.SaveSettingsEventHandler(doSaveSettings);
            this.ucUserActivityControl.ResetSettingsEvent += new Windows.UserActivityControl.ResetSettingsEventHandler(doResetSettings);

            this.ucUserActivityControl.SAB = _SAB;
        }
        
        private void doSaveSettings(object sender, Windows.UserActivityControl.SaveSettingsEventArgs e)
        {
            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();

            System.IO.MemoryStream gridMemoryStream = new System.IO.MemoryStream();
            
            e.myActivityGrid.DisplayLayout.Save(gridMemoryStream);
            layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, MIDRetail.DataCommon.eLayoutID.userActivityGrid, gridMemoryStream);

            System.IO.MemoryStream toolbarManagerMemoryStream = new System.IO.MemoryStream();
            e.myActivityToolbarManager.SaveAsBinary(toolbarManagerMemoryStream, true);
            layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, MIDRetail.DataCommon.eLayoutID.userActivityToolbars, toolbarManagerMemoryStream);
        }
        private void doResetSettings(object sender, Windows.UserActivityControl.ResetSettingsEventArgs e)
        {
            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
            layoutData.InfragisticsLayout_Delete(SAB.ClientServerSession.UserRID, MIDRetail.DataCommon.eLayoutID.userActivityGrid);
            layoutData.InfragisticsLayout_Delete(SAB.ClientServerSession.UserRID, MIDRetail.DataCommon.eLayoutID.userActivityToolbars);
        }


 
	}


}
