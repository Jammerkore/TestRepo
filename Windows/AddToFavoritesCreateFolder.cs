using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class AddToFavoritesCreateFolder : MIDFormBase
    {
        public AddToFavoritesCreateFolder()
        {
            InitializeComponent();
        }

        public AddToFavoritesCreateFolder(SessionAddressBlock aSAB)
            : base(aSAB)
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            
        }
    }
}
