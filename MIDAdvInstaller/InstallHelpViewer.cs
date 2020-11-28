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
    public partial class InstallHelpViewer : Form
    {
        public InstallHelpViewer()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //hide log viewer
            this.Close();
            //this.Hide();
        }

        public void SetText(string aText)
        {
            textBox1.Text = aText;
        }
    }
}
