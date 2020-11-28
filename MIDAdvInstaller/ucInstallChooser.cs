using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetailInstaller
{
    public partial class ucInstallChooser : UserControl
    {
        //public events
        public event EventHandler ConfigureNext;
        public event EventHandler NotConfigureNext;

        InstallerFrame frame = null;
        ToolTip tt = new ToolTip();

        public ucInstallChooser(InstallerFrame p_frm)
        {
            InitializeComponent();

            //pass the frame form
            frame = p_frm;
			// Begin TT#74 MD - JSmith - One-button Upgrade
            //frame.SetStatusMessage(frame.GetText("SelectClient"));
			// End TT#74 M

            // Begin TT#74 MD - JSmith - One-button Upgrade
            tt.SetToolTip(rdoUpgradeAll, frame.GetToolTipText("frame_SelectUpgradeAll"));
            // End TT#74 M
            tt.SetToolTip(rdoClient, frame.GetToolTipText("frame_SelectClient"));
            tt.SetToolTip(rdoServer, frame.GetToolTipText("frame_SelectServer"));
            tt.SetToolTip(rdoConfigure, frame.GetToolTipText("frame_SelectConfigure"));
            tt.SetToolTip(rdoUtilities, frame.GetToolTipText("frame_SelectUtilities"));
            // Begin TT#74 MD - JSmith - One-button Upgrade
            rdoUpgradeAll.Text = frame.GetText("rdoUpgradeAll");
            // End TT#74 MD
            rdoClient.Text = frame.GetText("rdoClient");
            rdoServer.Text = frame.GetText("rdoServer");
            rdoConfigure.Text = frame.GetText("rdoConfigure");
            rdoUtilities.Text = frame.GetText("rdoUtilities");
        }

        // Begin TT#74 MD - JSmith - One-button Upgrade
        //client installation task
        public bool UpgradeAll
        {
            get
            {
                return rdoUpgradeAll.Checked;
            }
        }
        // End TT#74 MD

        //client installation task
        public bool Client
        {
            get
            {
                return rdoClient.Checked;
            }
        }

        //server installation task
        public bool Server
        {
            get
            {
                return rdoServer.Checked;
            }
        }

        //database installation task
        public bool Configure
        {
            get
            {
                return rdoConfigure.Checked;
            }
        }

        //database installation task
        public bool Utilities
        {
            get
            {
                return rdoUtilities.Checked;
            }
        }

        // Begin TT#74 MD - JSmith - One-button Upgrade
        private void rdoUpgradeAll_CheckedChanged(object sender, EventArgs e)
        {
            if (UpgradeAll)
            {
                frame.SetStatusMessage(frame.GetText("SelectUpgradeAll"));
            }
            NotConfigureNext(this, new EventArgs());
        }
        // End TT#74 MD

        private void rdoClient_CheckedChanged(object sender, EventArgs e)
        {
            if (Client)
            {
                frame.SetStatusMessage(frame.GetText("SelectClient"));
            }
            // Begin TT#74 MD - JSmith - One-button Upgrade
            ConfigureNext(this, new EventArgs());
            // End TT#74 MD
        }

        private void rdoServer_CheckedChanged(object sender, EventArgs e)
        {
            if (Server)
            {
                frame.SetStatusMessage(frame.GetText("SelectServer"));
            }
            // Begin TT#74 MD - JSmith - One-button Upgrade
            ConfigureNext(this, new EventArgs());
            // End TT#74 MD
        }

        private void rdoUtilities_CheckedChanged(object sender, EventArgs e)
        {
            if (Utilities)
            {
                frame.SetStatusMessage(frame.GetText("SelectUtilities"));
            }
            // Begin TT#74 MD - JSmith - One-button Upgrade
            ConfigureNext(this, new EventArgs());
            // End TT#74 MD
        }

        private void rdoConfigure_CheckedChanged(object sender, EventArgs e)
        {
            if (Configure)
            {
                frame.SetStatusMessage(frame.GetText("SelectConfigure"));
            }
            // Begin TT#74 MD - JSmith - One-button Upgrade
            ConfigureNext(this, new EventArgs());
            // End TT#74 MD
        }

        private void ucInstallChooser_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                frame.Back_Enabled = false;
                frame.Next_Enabled = true;
                ConfigureNext(this, new EventArgs());

                if (frame.htMIDRegistered.Count == 0)
                {
                    rdoConfigure.Enabled = false;
                    // Begin TT#74 MD - JSmith - One-button Upgrade
                    rdoUpgradeAll.Enabled = false;
                    // End TT#74 MD
                }
                else
                {
                    rdoConfigure.Enabled = true;
                    // Begin TT#74 MD - JSmith - One-button Upgrade
                    rdoUpgradeAll.Enabled = true;
                    rdoUpgradeAll.Checked = true;
                    // Begin TT#195 MD - JSmith - Add environment authentication
                    if (frame.isValidClient())
                    {
                        rdoClient.Enabled = true;
                    }
                    else
                    {
                        rdoClient.Enabled = false;
                    }
                    if (frame.isValidServer())
                    {
                        rdoServer.Enabled = true;
                    }
                    else
                    {
                        rdoServer.Enabled = false;
                    }
                    // End TT#195 MD
                    NotConfigureNext(this, new EventArgs());
                    // End TT#74 MD
                }
            }
        }
    }
}
