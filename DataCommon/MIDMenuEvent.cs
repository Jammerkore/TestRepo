using System;
using System.Collections;
using System.Text;

namespace MIDRetail.DataCommon
{
	[Serializable]
    public class MIDMenuEvent
    {
        // add event to update menu
        public delegate void MenuChangeEventHandler(object source, MIDMenuEventArgs e);
        public event MenuChangeEventHandler OnMenuChangeHandler;

        public void ChangeMenu(object source, string aMenu, eMIDMenuItem aMenuItem, eMIDMenuAction aMIDMenuAction)
        {
            MIDMenuEventArgs ea = new MIDMenuEventArgs(aMenu, aMenuItem, aMIDMenuAction);
            // fire the event
            OnMenuChangeHandler(source, ea);
            return;
        }

        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        public void ChangeMenu(object source, string aMenu, eMIDMenuItem aMenuItem, eMIDMenuAction aMIDMenuAction, string aText)
        {
            MIDMenuEventArgs ea = new MIDMenuEventArgs(aMenu, aMenuItem, aMIDMenuAction, aText);
            OnMenuChangeHandler(source, ea);
            return;
        }
        // End TT#335
    }

    public class MIDMenuEventArgs : EventArgs
    {
        string _menu;
        eMIDMenuItem _MIDMenuItem;
        eMIDMenuAction _MIDMenuAction;
        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        string _Text = null;
        // End TT#335

        public MIDMenuEventArgs(string aMenu, eMIDMenuItem aMenuItem, eMIDMenuAction aMIDMenuAction)
		{
            _menu = aMenu;
            _MIDMenuItem = aMenuItem;
            _MIDMenuAction = aMIDMenuAction;
		}

        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        public MIDMenuEventArgs(string aMenu, eMIDMenuItem aMenuItem, eMIDMenuAction aMIDMenuAction, string aText)
        {
            _menu = aMenu;
            _MIDMenuItem = aMenuItem;
            _MIDMenuAction = aMIDMenuAction;
            _Text = aText;
        }
        // End TT#335

        public string MIDMenu
        {
            get { return _menu; }
        }

        public eMIDMenuItem MIDMenuItem
        {
            get { return _MIDMenuItem; }
        }

        public eMIDMenuAction MIDMenuAction
        {
            get { return _MIDMenuAction; }
        }

        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        public string Text
        {
            get { return _Text; }
        }
        // End TT#335
    }

    //public class MIDMenuManagement
    //{
    //    private Stack _menuActions;


    //    public MIDMenuManagement()
    //    {
    //        _menuActions = new Stack();
    //    }

    //    public void AddMenuActionMIDMenuManagement()
    //    {
    //        _menuActions = new Stack();
    //    }
    //}
}
