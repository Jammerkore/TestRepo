using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UnitTesting
{
    public partial class EnvironmentCurrentControl : UserControl
    {
        public EnvironmentCurrentControl()
        {
            InitializeComponent();
            DataSet dsEnvironment = UnitTests.GetEnvironmentsForSelection();
            this.ultraCombo1.SetDataBinding(dsEnvironment, "Environments");
            this.ultraCombo1.Text = UnitTests.defaultEnvironment;
            UnitTests.EnvironmentChanged_Event += new UnitTests.EnvironmentChanged_EventHandler(Handle_EnvironmentChanged);
        }
        private void Handle_EnvironmentChanged(object sender, UnitTests.EnvironmentChanged_EventArgs e)
        {
            this.ultraCombo1.Text = UnitTests.defaultEnvironment;
        }

        //public event SelectEnvironment_OK_EventHandler SelectEnvironment_OK_Event;
        //public virtual void RaiseSelectEnvironment_OK_Event(string environmentName)
        //{

        //    if (SelectEnvironment_OK_Event != null)
        //        SelectEnvironment_OK_Event(this, new SelectEnvironment_OK_EventArgs(environmentName));
        //}
        //public class SelectEnvironment_OK_EventArgs
        //{
        //    public SelectEnvironment_OK_EventArgs(string environmentName) { this.environmentName = environmentName; }
        //    public string environmentName { get; private set; }
        //}
        //public delegate void SelectEnvironment_OK_EventHandler(object sender, SelectEnvironment_OK_EventArgs e);
        //public event SelectEnvironment_Cancel_EventHandler SelectEnvironment_Cancel_Event;
        //public virtual void RaiseSelectEnvironment_Cancel_Event()
        //{

        //    if (SelectEnvironment_Cancel_Event != null)
        //        SelectEnvironment_Cancel_Event(this, new SelectEnvironment_Cancel_EventArgs());
        //}
        //public class SelectEnvironment_Cancel_EventArgs
        //{
        //    public SelectEnvironment_Cancel_EventArgs() { }
        //}
        //public delegate void SelectEnvironment_Cancel_EventHandler(object sender, SelectEnvironment_Cancel_EventArgs e);

        private void ultraCombo1_ValueChanged(object sender, EventArgs e)
        {
            string environmentName = this.ultraCombo1.Text;
            if (UnitTests.DoesEnvironmentExist(environmentName) == false)
            {
                MessageBox.Show("Please select a valid DB.");
            }
            UnitTests.defaultEnvironment = environmentName;
            UnitTests.RaiseEnvironmentChanged_Event(this, environmentName);
        }
    }
    
}
