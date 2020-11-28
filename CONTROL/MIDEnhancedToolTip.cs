using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// BEGIN TT#1712 - stodd - add tool tip to disabled controls
namespace MIDRetail.Windows.Controls
{
	[ProvideProperty("ToolTipWhenDisabled", typeof(Control)),
	ProvideProperty("SizeOfToolTipWhenDisabled", typeof(Control))]
	/// <summary>
	/// EnhancedToolTip supports the ToolTipWhenDisabled and SizeOfToolTipWhenDisabled
	/// extender properties that can be used to show tooltip messages when the associated
	/// control is disabled.
	/// </summary>
	public partial class MIDEnhancedToolTip : System.Windows.Forms.ToolTip
	{
		#region ========== Required constructor ==========
		// This constructor is required for the Windows Forms Designer to instantiate
		// an object of this class with New(Me.components).
		// To verify this, just remove this constructor. Build it and then put the
		// component on a form. Take a look at the Designer.vb file for InitializeComponents(),
		// and search for the line where it instantiates this class.
		public MIDEnhancedToolTip(IContainer p_container)
			: base()
		{
			// Required for Windows.Forms Class Composition Designer support
			if (p_container != null)
			{
				p_container.Add(this);
			}

			// Suscribes to Popup event listener
			// Comment out following lines if you are okay with the same Title/Icon for disabled controls.
			this.m_EvtPopup = new PopupEventHandler(EnhancedToolTip_Popup);
			this.Popup += this.m_EvtPopup;
		}
		#endregion

		#region ========== ToolTipWhenDisabled extender property support ==========
		private Dictionary<Control, string> m_ToolTipWhenDisabled = new Dictionary<Control, string>();
		private Dictionary<Control, TransparentSheet> m_TransparentSheet = new Dictionary<Control, TransparentSheet>();
		private Dictionary<Control, PaintEventHandler> m_EvtControlPaint = new Dictionary<Control, PaintEventHandler>();
		private Dictionary<Control, EventHandler> m_EvtControlEnabledChanged = new Dictionary<Control, EventHandler>();

		public void SetToolTipWhenDisabled(Control p_control, string value)
		{
			if (p_control == null)
			{
				throw new ArgumentNullException("control");
			}

			if (!String.IsNullOrEmpty(value))  
			{
				// Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
                if (this.m_ToolTipWhenDisabled.ContainsKey(p_control))
                {
                    m_ToolTipWhenDisabled.Remove(p_control);
                }
				// End TT#1019 - MD - stodd - prohibit allocation actions against GA - 
				this.m_ToolTipWhenDisabled[p_control] = value;
				if (!p_control.Enabled)
				{
					// When the control is disabled at design time, the EnabledChanged
					// event won't fire. So, on the first Paint event, we should call
					// PutOnTransparentSheet().
                    // Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
                    if (this.m_EvtControlPaint.ContainsKey(p_control))
                    {
                        m_EvtControlPaint.Remove(p_control);
                    }
                    // End TT#1019 - MD - stodd - prohibit allocation actions against GA - 
					m_EvtControlPaint.Add(p_control, new PaintEventHandler(control_Paint));
					p_control.Paint += m_EvtControlPaint[p_control];
				}
				// Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
                if (this.m_EvtControlEnabledChanged.ContainsKey(p_control))
                {
                    m_EvtControlEnabledChanged.Remove(p_control);
                }
				// End TT#1019 - MD - stodd - prohibit allocation actions against GA - 
				m_EvtControlEnabledChanged.Add(p_control, new EventHandler(control_EnabledChanged));
				p_control.EnabledChanged += m_EvtControlEnabledChanged[p_control];
			}
			else
			{
				m_ToolTipWhenDisabled.Remove(p_control);
				if (m_EvtControlEnabledChanged != null)
				{
					p_control.EnabledChanged -= m_EvtControlEnabledChanged[p_control];
				}
			}
		}

		private void control_Paint(object sender, PaintEventArgs e)
		{
			Control l_control;

			l_control = (Control)sender;
			this.PutOnTransparentSheet(l_control);
			// Immediately remove the handler because we don't need it any longer.
			if (m_EvtControlPaint != null)
			{
				l_control.Paint -= m_EvtControlPaint[l_control];
			}
		}

		private void control_EnabledChanged(object sender, EventArgs e)
		{
			Control l_control;

			l_control = (Control)sender;
			if (l_control.Enabled)
			{
				this.TakeOffTransparentSheet(l_control);
			}
			else
			{
				this.PutOnTransparentSheet(l_control);
			}
		}

		[Category("Misc"),
		Description("Determines the ToolTip shown when the mouse hovers over the disabled control."),
		Localizable(true),
		Editor(typeof(MultilineStringEditor), typeof(UITypeEditor)),
		DefaultValue("")]

		public string GetToolTipWhenDisabled(Control p_control)
		{
			if (p_control == null)
			{
				throw new ArgumentNullException("control");
			}

			if (m_ToolTipWhenDisabled.ContainsKey(p_control))
			{
				return m_ToolTipWhenDisabled[p_control];
			}
			else
			{
				return String.Empty;
			}
		}

		private void PutOnTransparentSheet(Control p_control)
		{
			TransparentSheet l_ts;

			l_ts = new TransparentSheet();
			l_ts.Location = p_control.Location;
			if (m_SizeOfToolTipWhenDisabled.ContainsKey(p_control))
			{
				l_ts.Size = this.m_SizeOfToolTipWhenDisabled[p_control];
			}
			else
			{
				l_ts.Size = p_control.Size;
			}
			p_control.Parent.Controls.Add(l_ts);
			l_ts.BringToFront();
			this.m_TransparentSheet[p_control] = l_ts;
			this.SetToolTip(l_ts, m_ToolTipWhenDisabled[p_control]);
		}

		private void TakeOffTransparentSheet(Control p_control)
		{
			TransparentSheet l_ts;

			if (m_TransparentSheet.ContainsKey(p_control))
			{
				l_ts = m_TransparentSheet[p_control];
				p_control.Parent.Controls.Remove(l_ts);
				this.SetToolTip(l_ts, String.Empty);
				l_ts.Dispose();
				m_TransparentSheet.Remove(p_control);
			}
		}
		#endregion

		#region ========== Support for the oversized transparent sheet to cover multiple visual controls. ==========
		private Dictionary<Control, Size> m_SizeOfToolTipWhenDisabled = new Dictionary<Control, Size>();

		public void SetSizeOfToolTipWhenDisabled(Control p_control, Size value)
		{
			if (p_control == null)
			{
				throw new ArgumentNullException("control");
			}

			if (!value.IsEmpty)
			{
				m_SizeOfToolTipWhenDisabled[p_control] = value;
			}
			else
			{
				m_SizeOfToolTipWhenDisabled.Remove(p_control);
			}
		}

		[Category("Misc"),
		Description("Determines the size of the ToolTip when the control is disabled." +
					 " Leave it to 0,0, unless you want the ToolTip to pop up over wider" +
					 " rectangular area than this control."),
		DefaultValue(typeof(Size), "0,0")]
		public Size GetSizeOfToolTipWhenDisabled(Control p_control)
		{
			if (p_control == null)
			{
				throw new ArgumentNullException("control");
			}

			if (m_SizeOfToolTipWhenDisabled.ContainsKey(p_control))
			{
				return m_SizeOfToolTipWhenDisabled[p_control];
			}
			else
			{
				return Size.Empty;
			}
		}
		#endregion

		#region ========== Comment out this region if you are okay with the same Title/Icon for disabled controls. ==========
		private PopupEventHandler m_EvtPopup;

		private string m_strToolTipTitle;
		/// <summary>
		/// ToolTip title when control is disabled
		/// </summary>
		public new string ToolTipTitle
		{
			get
			{
				return base.ToolTipTitle;
			}
			set
			{
				base.ToolTipTitle = value;
				this.m_strToolTipTitle = value;
			}
		}

		private ToolTipIcon m_ToolTipIcon;
		/// <summary>
		/// ToolTip icon when control is disabled
		/// </summary>
		public new ToolTipIcon ToolTipIcon
		{
			get
			{
				return base.ToolTipIcon;
			}
			set
			{
				base.ToolTipIcon = value;
				this.m_ToolTipIcon = value;
			}
		}

		private void EnhancedToolTip_Popup(object sender, PopupEventArgs e)
		{
			if (e.AssociatedControl is TransparentSheet)
			{
				base.ToolTipTitle = String.Empty;
				base.ToolTipIcon = ToolTipIcon.None;
			}
			else
			{
				base.ToolTipTitle = this.m_strToolTipTitle;
				base.ToolTipIcon = this.m_ToolTipIcon;
			}
		}

		protected override void Dispose(bool disposing)
		{
			// Removing Popup Handler.
			if (this.m_EvtPopup != null)
			{
				this.Popup -= this.m_EvtPopup;
			}
			base.Dispose(disposing);
		}
		#endregion
	}

	public class TransparentSheet : ContainerControl
	{
		public TransparentSheet()
		{
			// Disable painting the background.
			this.SetStyle(ControlStyles.Opaque, true);
			this.UpdateStyles();

			// Make sure to set the AutoScaleMode property to None 
			// so that the location and size property don't automatically change 
			// when placed in a form that has different font than this.
			this.AutoScaleMode = AutoScaleMode.None;

			// Tab stop on a transparent sheet makes no sense.
			this.TabStop = false;
		}

		private const short WS_EX_TRANSPARENT = 0x20;

		protected override CreateParams CreateParams
		{
			[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, UnmanagedCode = true)]
			get
			{
				CreateParams l_cp;
				l_cp = base.CreateParams;
				l_cp.ExStyle = (l_cp.ExStyle | WS_EX_TRANSPARENT);
				return l_cp;
			}
		}
	}
	// END TT#1712 - stodd - add tool tip to disabled controls
}
