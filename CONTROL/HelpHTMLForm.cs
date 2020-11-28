using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
    public partial class HelpHTMLForm : Form
    {
        public HelpHTMLForm()
        {
            InitializeComponent();

        }

        private void helpHTMLForm_Load(object sender, EventArgs e)
        {

        }
        public void LoadHelpFile(string filePath)
        {
            this.helpHTMLControl1.LoadHelp(filePath);
        }
    }
}
