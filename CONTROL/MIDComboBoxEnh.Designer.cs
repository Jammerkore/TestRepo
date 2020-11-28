using System;
using System.Windows.Forms;
using System.Drawing;

using MIDRetail.DataCommon;

namespace MIDRetail.Windows.Controls
{
    partial class MIDComboBoxEnh
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            // Begin TT#3513 - JSmith - Clean Up Memory Leaks
            //if (disposing && (components != null))
            //{
            //    components.Dispose();
            //}
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                _currentValue = null;
                _toolTip.RemoveAll();
                _toolTip.Dispose();
                _toolTip = null;
                if (comboBox1.DataSource == null)
                {
                    this.comboBox1.Items.Clear();
                }
                else
                {
                    this.comboBox1.DataSource = null;
                }
                this.comboBox1.DropDown -= new System.EventHandler(this.comboBox1_DropDown);
                this.comboBox1.SelectedIndexChanged -= new System.EventHandler(this.comboBox1_SelectedIndexChanged);
                this.comboBox1.SelectionChangeCommitted -= new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
                this.comboBox1.DropDownClosed -= new System.EventHandler(this.comboBox1_DropDownClosed);
                this.comboBox1.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyDown);
                this.comboBox1.LostFocus -= new System.EventHandler(this.comboBox1_LostFocus);
                this.comboBox1.Dispose();
                this.comboBox1 = null;
                this.Load -= new System.EventHandler(this.MIDComboBoxEnh_Load);
            }
            // End TT#3513 - JSmith - Clean Up Memory Leaks
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            if (MIDEnvironment.isWindows)
            {
                this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            }
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.IntegralHeight = false;
            this.comboBox1.Location = new System.Drawing.Point(0, 0);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(240, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            this.comboBox1.DropDownClosed += new System.EventHandler(this.comboBox1_DropDownClosed);
            this.comboBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyDown);
            this.comboBox1.LostFocus += new System.EventHandler(this.comboBox1_LostFocus);
            this.comboBox1.MaxDropDownItems = 25;  // TT#2702 - AGallagher - Header Action Drop Down on 5.0
            // 
            // MIDComboBoxEnh
            // 
            this.Controls.Add(this.comboBox1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MIDComboBoxEnh";
            this.Size = new System.Drawing.Size(240, 23);
            this.Load += new System.EventHandler(this.MIDComboBoxEnh_Load);
            this.ResumeLayout(false);

        }

        #endregion

        //BEGIN TT#62-MD-VStuart-4.2/5.0 Infrastructure Enhancements
        public class MyComboBox : ComboBox
        {
            // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
            public MyComboBox()
            {
                Graphics g = CreateGraphics();
            }
            // End TT#2669 - JSmith - Unable to remove a filter from a wokflow

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

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // Here we check for the Tab key and when true set the text
            // property and send the Enter Key or the Tab Key if NOT dropped down.
            if ((e.KeyCode == Keys.Tab) && (comboBox1.DroppedDown == true))
            {
                try
                {
                    if (comboBox1.SelectedItem.ToString() != null)
                    {
                        comboBox1.Text = comboBox1.SelectedItem.ToString();
                        SendKeys.Send("{Enter}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0} Caught exception, a comboBox is NULL.", ex);
                }
            }
            // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
            else if (e.KeyCode == Keys.Delete)
            {
                comboBox1.SelectedIndex = -1;
            }
            // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
        }

        private MIDComboBoxEnh.MyComboBox comboBox1;
        //END TT#62-MD-VStuart-4.2/5.0 Infrastructure Enhancements
    }
}
