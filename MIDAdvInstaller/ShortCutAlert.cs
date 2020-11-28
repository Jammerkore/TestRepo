using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetailInstaller
{
    public partial class ShortCutAlert : Form
    {
        public ShortCutAlert()
        {
            InitializeComponent();
        }

        //link name property
        string strLinkName = "";
        public string LinkName
        {
            get
            {
                return strLinkName;
            }
            set
            {
                strLinkName = value;
            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            strLinkName = txtLinkName.Text.ToString().Trim();
            this.Hide();
        }

    }
}
