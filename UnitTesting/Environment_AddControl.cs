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
    public partial class Environment_AddControl : UserControl
    {
        public Environment_AddControl()
        {
            InitializeComponent();
        }
      
        public enum EditModes
        {
            Add,
            Edit
        }
        private EditModes editMode;
        public void SetEditMode(EditModes editMode)
        {
            this.editMode = editMode;
        }
        private string oldEnvironmentName;
        public void LoadEnvironment(string environmentName)
        {
            oldEnvironmentName = environmentName;
            DataRow dr = UnitTests.GetEnvironmentFromName(environmentName);
            if (dr != null)
            {
                this.txtEnvironmentName.Text = environmentName;
                this.txtConnectionString.Text = Shared_UtilityFunctions.DataRowReadField(dr, "connectionString");
                this.txtServer.Text = Shared_UtilityFunctions.DataRowReadField(dr, "server");
                this.txtDatabaseName.Text = Shared_UtilityFunctions.DataRowReadField(dr, "databaseName");
                this.txtBAKFile.Text = Shared_UtilityFunctions.DataRowReadField(dr, "bakFilePath");
            }
        }


        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnOK":
                    if (IsValid())
                    {
                        SaveEnvironment();
                        RaiseFireOK_Event();
                    }
                    break;
                case "btnCancel":
                    RaiseFireCancel_Event();
                    break;

            }
        }

        private bool IsValid()
        {
            bool valid = true;
            string newEnvironmentName = this.txtEnvironmentName.Text.Trim();
            if (newEnvironmentName == string.Empty)
            {
                valid = false;
                MessageBox.Show("Please provide an environment name.");
            }
            if (editMode == EditModes.Add)
            {
                if (UnitTests.DoesEnvironmentExist(newEnvironmentName))
                {
                    valid = false;
                    MessageBox.Show("Environment already exists.  Please provide a different name.");
                }
            }
            else if (editMode == EditModes.Edit)
            {
                if (newEnvironmentName != oldEnvironmentName)
                {
                    if (UnitTests.DoesEnvironmentExist(newEnvironmentName))
                    {
                        valid = false;
                        MessageBox.Show("Environment already exists.  Please provide a different name.");
                    }
                }
            }
            string connectionString = this.txtConnectionString.Text.Trim();
            if (connectionString == string.Empty)
            {
                valid = false;
                MessageBox.Show("Please provide a connection string.");
            }
            return valid;
        }
        private void SaveEnvironment()
        {
            string newEnvironmentName = this.txtEnvironmentName.Text.Trim();
            string connectionString = this.txtConnectionString.Text.Trim();
            string server = this.txtServer.Text.Trim();
            string databaseName = this.txtDatabaseName.Text.Trim();
            string bakFilePath = this.txtBAKFile.Text.Trim();
            if (editMode == EditModes.Add)
            {
                UnitTests.AddEnvironment(newEnvironmentName, connectionString, server, databaseName, bakFilePath);
            }
            else if (editMode == EditModes.Edit)
            {
                UnitTests.EditEnvironment(oldEnvironmentName, newEnvironmentName, connectionString, server, databaseName, bakFilePath);
            }
        }


        public event FireOK_EventHandler FireOK_Event;
        public virtual void RaiseFireOK_Event()
        {

            if (FireOK_Event != null)
                FireOK_Event(this, new FireOK_EventArgs());
        }
        public class FireOK_EventArgs
        {
            public FireOK_EventArgs() {}
        }
        public delegate void FireOK_EventHandler(object sender, FireOK_EventArgs e);
        public event FireCancel_EventHandler FireCancel_Event;
        public virtual void RaiseFireCancel_Event()
        {

            if (FireCancel_Event != null)
                FireCancel_Event(this, new FireCancel_EventArgs());
        }
        public class FireCancel_EventArgs
        {
            public FireCancel_EventArgs() { }
        }
        public delegate void FireCancel_EventHandler(object sender, FireCancel_EventArgs e);
    }
}
