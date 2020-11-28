using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
    public partial class filterBuilderListNodeOverlay : UserControl
    {
        public filterBuilderListNodeOverlay()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // do nothing
        }

        private void aaControl_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("click");
        }

        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //    if (m.Msg == 0x000F)
        //    {
        //        //DrawText();
        //    }
        //}

        //Events
        //public event NodeClickedEventHandler NodeClickedEvent;
        //public void RaiseNodeClickedEvent()
        //{
        //    if (NodeClickedEvent != null)
        //        NodeClickedEvent(new object(), new NodeClickedEventArgs());
        //}
        //public class NodeClickedEventArgs
        //{
        //    public NodeClickedEventArgs() { }
        //}
        //public delegate void NodeClickedEventHandler(object sender, NodeClickedEventArgs e);

        //public event MouseEnterEventHandler MouseEnterEvent;
        //public void RaiseMouseEnterEvent()
        //{
        //    if (MouseEnterEvent != null)
        //        MouseEnterEvent(new object(), new MouseEnterEventArgs());
        //}
        //public class MouseEnterEventArgs
        //{
        //    public MouseEnterEventArgs() { }
        //}
        //public delegate void MouseEnterEventHandler(object sender, MouseEnterEventArgs e);

        //public event MouseLeaveEventHandler MouseLeaveEvent;
        //public void RaiseMouseLeaveEvent()
        //{
        //    if (MouseLeaveEvent != null)
        //        MouseLeaveEvent(new object(), new MouseLeaveEventArgs());
        //}
        //public class MouseLeaveEventArgs
        //{
        //    public MouseLeaveEventArgs() { }
        //}
        //public delegate void MouseLeaveEventHandler(object sender, MouseLeaveEventArgs e);
    }
}
