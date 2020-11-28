using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DisplayMessage
{
    public partial class Form1 : Form
    {
        public string MessageToDisplay;


        public Form1()
        {
            InitializeComponent();
        }

        private void frmLoad(object sender, EventArgs e)
        {
            this.textBox1.Text = MessageToDisplay;
        }
    }
}
