using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon; 

namespace MIDRetail.Windows.Controls
{
    public partial class MIDWindowsComboBox : ComboBox
    {
        private SessionAddressBlock _SAB;
        private FunctionSecurityProfile _functionSecurity;
        internal bool _continueReadOnly;
        internal bool _initializing;
        private int _spacing = 2;
        private int _imageHeight;
        private int _imageWidth;

        /// <summary>
        /// Gets or sets the function security profile
        /// </summary>
        public FunctionSecurityProfile FunctionSecurity
        {
            get { return _functionSecurity; }
        }

        public bool ContinueReadOnly
        {
            get { return _continueReadOnly; }
        }

        protected SessionAddressBlock SAB
        {
            get { return _SAB; }
        }

        public MIDWindowsComboBox()
        {
            InitializeComponent();
        }

        public void Initialize(SessionAddressBlock aSAB, FunctionSecurityProfile aFunctionSecurity)
        {
            _initializing = true;
            _SAB = aSAB;
            _functionSecurity = aFunctionSecurity;
            _initializing = false;
        }

        private void MIDWindowsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Determines whether the specified key is a regular input key or a special key that requires preprocessing.
        /// </summary>
        /// <returns>
        /// true if the specified key is a regular input key; otherwise, false.
        /// </returns>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"/> values.
        ///                 </param>
        protected override bool IsInputKey(Keys keyData)
        {
            {
                if (this.DroppedDown == true)
                {
                    switch (keyData)
                    {
                        case Keys.Tab:
                            return true;
                        default:
                            return base.IsInputKey(keyData);
                    }
                }
                else
                {
                    {
                        return base.IsInputKey(keyData);
                    }
                }
            }
        }
    }
}
