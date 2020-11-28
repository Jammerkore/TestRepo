using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDRetail.Common
{
    public class AssortmentActiveProcessToolbarHelper
    {
        public class AssortmentScreen
        {
            public string screenTitle;
            public int screenID;
			// Begin TT#998 - MD - stodd - Enqueue error when opening style review with GA open, but Size Need Method open as the active window.
            public System.Windows.Forms.Form form;
			// Begin TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
            public string screenType;

            public AssortmentScreen(string screenTitle, int screenID, string screenType, System.Windows.Forms.Form form)
            {
                this.screenTitle = screenTitle;
                this.screenID = screenID;
                this.screenType = screenType;
                this.form = form;
            }
			// End TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
			// End TT#998 - MD - stodd - Enqueue error when opening style review with GA open, but Size Need Method open as the active window.
        }


        public static List<AssortmentScreen> assortmentScreenList = IntializeAssortmentScreenList();

        public static List<AssortmentScreen> IntializeAssortmentScreenList()
        {
            List<AssortmentScreen>  tempList = new List<AssortmentScreen>();
			// Begin TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
            AssortmentScreen allocationProcess = new AssortmentScreen("Allocation Workspace", -1, "Allocation Workspace", null); 	// TT#998 - MD - stodd - Enqueue error when opening style review with GA open, but Size Need Method open as the active window.
			// End TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
            tempList.Add(allocationProcess);
            ActiveProcess = allocationProcess;

            return tempList;
        }

		// Begin TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
        public static void AddAssortmentReviewScreen(string screenTitle, int screenID, string screenType, System.Windows.Forms.Form form)	// TT#998 - MD - stodd - Enqueue error when opening style review with GA open, but Size Need Method open as the active window.
        {
            assortmentScreenList.Add(new AssortmentScreen(screenTitle, screenID, screenType, form));	// TT#998 - MD - stodd - Enqueue error when opening style review with GA open, but Size Need Method open as the active window.
            RaiseListChangedEvent();
        }
		// End TT#1019 - MD - stodd - interactive velocity vs read-only group allocation - 
		
        public static void RemoveAssortmentReviewScreen(string screenTitle, int screenID)
        {
            AssortmentScreen result = assortmentScreenList.Find(
              delegate(AssortmentScreen r)
              {
                  return r.screenTitle == screenTitle;
              }
              );
            if (result != null)
            {
                assortmentScreenList.Remove(result);
                RaiseListChangedEvent();
            }
        }

        public static AssortmentScreen ActiveProcess;

   
        private static bool _isAssortmentInstalled = false;
        public static bool IsAssortmentInstalled 
        {
            get { return _isAssortmentInstalled; }
            set { _isAssortmentInstalled = value; } //Set one time by Explorer.cs (InitializeComponent) when the client app starts
        }

        private static bool _isAssortmentActiveProcessAccessAllowed = false;
        public static bool IsAssortmentActiveProcessAccessAllowed
        {
            get { return _isAssortmentActiveProcessAccessAllowed; }
            set { _isAssortmentActiveProcessAccessAllowed = value; } //Set one time by Explorer.cs (InitializeComponent) when the client app starts
        }

        //Active Process Changed Event
        //public class ActiveProcessChangedEventArgs
        //{
        //  public ActiveProcessChangedEventArgs(string selectedProcessTitle, int selectedProcessID) { this.selectedProcessTitle = selectedProcessTitle; this.selectedProcessID = selectedProcessID; }
        //  public string selectedProcessTitle { get; private set; } // readonly
        //  public int selectedProcessID { get; private set; } // readonly
        //}
        //public delegate void ActiveProcessChangedEventHandler(object sender, ActiveProcessChangedEventArgs e);
        //public static event ActiveProcessChangedEventHandler ActiveProcessChangedEvent;
        //public static void RaiseActiveProcessChangedEvent(object obj, string selectedProcessTitle, int selectedProcessID)
        //{
           
        //    if (ActiveProcessChangedEvent != null)
        //        ActiveProcessChangedEvent(obj, new ActiveProcessChangedEventArgs(selectedProcessTitle, selectedProcessID));
        //}

        //Update Active Process
        public static void ActiveProcessChanged(string selectedProcessTitle, int selectedProcessID)
        {
            ActiveProcess = assortmentScreenList.Find(
             delegate(AssortmentScreen r)
             {
                 return r.screenTitle == selectedProcessTitle;
             }
             ); 
        }

        //List Changed Event
        public class ListChangedEventArgs
        {
            public ListChangedEventArgs() {  }
        }
        public delegate void ListChangedEventHandler(ListChangedEventArgs e);
        public static event ListChangedEventHandler ListChangedEvent;
        public static void RaiseListChangedEvent()
        {
            if (ListChangedEvent != null)
                ListChangedEvent(new ListChangedEventArgs());
        }


    }
}
