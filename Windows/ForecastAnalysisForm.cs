using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Business;

namespace MIDRetail.Windows
{
    public partial class ForecastAnalysisForm : Form
    {
        private SessionAddressBlock _SAB;
        public ForecastAnalysisForm(SessionAddressBlock sab)
        {
            _SAB = sab;
            InitializeComponent();
        }

        private void ForecastAnalysisForm_Load(object sender, EventArgs e)
        {
            this.forecastAnalysisControl1.LoadData(_SAB);
        }
    }
}
