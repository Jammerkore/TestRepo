using System;
using System.Windows.Forms;

namespace MIDRetail.Windows
{
	public class NextButtonStatusEventArgs : EventArgs
	{
		private bool _enabled;

		public NextButtonStatusEventArgs(bool aEnabled)
		{
			_enabled = aEnabled;
		}

		public bool Enabled
		{
			get
			{
				return _enabled;
			}
		}
	}

	abstract public class FilterWizardPanel : System.Windows.Forms.Panel
	{
		private Panel _titlePanel;
		private Control _defaultControl;
		private FilterWizardPanel _backPanel;
		private FilterWizardPanel _nextPanel;
		private string _nextText;
		private bool _isBackEnabled;
		private bool _isNextEnabled;
		private int _index;

		public FilterWizardPanel()
		{
			_titlePanel = null;
			_defaultControl = null;
			_backPanel = null;
			_nextPanel = null;
			_nextText = "";
			_isBackEnabled = false;
			_isNextEnabled = false;
			_index = 0;
		}

		public Panel TitlePanel
		{
			get
			{
				return _titlePanel;
			}
			set
			{
				_titlePanel = value;
			}
		}

		public Control DefaultControl
		{
			get
			{
				return _defaultControl;
			}
			set
			{
				_defaultControl = value;
			}
		}

		public FilterWizardPanel BackPanel
		{
			get
			{
				return _backPanel;
			}
			set
			{
				_backPanel = value;
			}
		}

		public FilterWizardPanel NextPanel
		{
			get
			{
				return _nextPanel;
			}
			set
			{
				_nextPanel = value;
			}
		}

		public string NextText
		{
			get
			{
				return _nextText;
			}
			set
			{
				_nextText = value;
			}
		}

		public bool IsBackEnabled
		{
			get
			{
				return _isBackEnabled;
			}
			set
			{
				_isBackEnabled = value;
			}
		}

		public bool IsNextEnabled
		{
			get
			{
				return _isNextEnabled;
			}
			set
			{
				_isNextEnabled = value;
			}
		}

		public int Index
		{
			get
			{
				return _index;
			}
			set
			{
				_index = value;
			}
		}
	}

	public class FilterWizardIntroPanel : FilterWizardPanel
	{
	}

	public class FilterWizardNamePanel : FilterWizardPanel
	{
	}

	public class FilterWizardAttributePanel : FilterWizardPanel
	{
	}

	public class FilterWizardDataPanel : FilterWizardPanel
	{
	}

	public class FilterWizardFinishPanel : FilterWizardPanel
	{
	}

	public class FilterWizardConditionVar1Panel : FilterWizardPanel
	{
		private FilterWizardVar1 _parentControl;

		public FilterWizardVar1 ParentControl
		{
			get
			{
				return _parentControl;
			}
			set
			{
				_parentControl = value;
			}
		}
	}

	public class FilterWizardConditionQuantityPanel : FilterWizardPanel
	{
		private FilterWizardQuantity _parentControl;

		public FilterWizardQuantity ParentControl
		{
			get
			{
				return _parentControl;
			}
			set
			{
				_parentControl = value;
			}
		}
	}

	public class FilterWizardConditionGradePanel : FilterWizardPanel
	{
		private FilterWizardGrade _parentControl;

		public FilterWizardGrade ParentControl
		{
			get
			{
				return _parentControl;
			}
			set
			{
				_parentControl = value;
			}
		}
	}

	public class FilterWizardConditionStatusPanel : FilterWizardPanel
	{
		private FilterWizardStatus _parentControl;

		public FilterWizardStatus ParentControl
		{
			get
			{
				return _parentControl;
			}
			set
			{
				_parentControl = value;
			}
		}
	}

	public class FilterWizardConditionPercentPanel : FilterWizardPanel
	{
		private FilterWizardPercent _parentControl;

		public FilterWizardPercent ParentControl
		{
			get
			{
				return _parentControl;
			}
			set
			{
				_parentControl = value;
			}
		}
	}

	public class FilterWizardConditionVar2Panel : FilterWizardPanel
	{
		private FilterWizardVar2 _parentControl;

		public FilterWizardVar2 ParentControl
		{
			get
			{
				return _parentControl;
			}
			set
			{
				_parentControl = value;
			}
		}
	}
}
