using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MIDRetail.Data;

namespace MIDRetail.Windows.Controls
{

    /// <summary>
    /// Textbox that allows more than one email adddress to be auto suggested
    /// </summary>
    public class EmailAddressTextBox : TextBox
    {
        private IList<string> _emailAddressList = null;
        private Panel _dropDownPanel = null;
        private ListBox _addressListBox = null;
        private int _mouseIndex = -1;
        private bool _settingFocusToDropDown = false;
        private bool _settingText = false;
        private bool _closingDropDown = false;

        /// <summary>
        /// validates each email address in the textbox
        /// </summary>
        /// <returns></returns>
        public bool IsEveryAddressInTextValid()
        {
            bool valid = true;
            string[] addresses = this.Text.Split(';');
            foreach (string emailAddress in addresses)
            {
                if (emailAddress != string.Empty)
                {
                    if (MIDEmail.IsAddressValid(emailAddress) == false)
                    {
                        valid = false;
                    }
                }

            }
            return valid;
        }


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (MIDEmail.UseOutlookContacts == false || MIDEmail.IsOutlookInstalled() == false)  //TT#3624 -jsobek -Unhandled Exception opening Task List when "Use Outlook Contact Information" is checked on 
            {
                return;
            }

            if (_settingText == false)
            {
                string startText = GetTextToMatch();


                if (startText != string.Empty)
                {
                    var matchingAddresses = from address in _emailAddressList where address.StartsWith(startText, StringComparison.CurrentCultureIgnoreCase) orderby address select address;
                    if (matchingAddresses.Count<string>() > 0)
                    {
                        _addressListBox.Items.Clear();

                        foreach (string str in matchingAddresses)
                        {
                            _addressListBox.Items.Add(str);
                        }

                        _dropDownPanel.Width = this.Width;
                        _dropDownPanel.Visible = true;
                        _dropDownPanel.BringToFront();
                        this.Focus();
                    }
                }
                else
                {
                    HideDropDown();
                }
            }
            else
            {
                HideDropDown();
            }
        }

        private string GetTextToMatch()
        {
            string startText; 
                
            //startText = this.Text.Trim();
            //if (startText.Contains(";") && startText.EndsWith(";") == false)
            //{
            //    startText = startText.Substring(startText.LastIndexOf(";") + 1);
            //}

            if (this.Text.Contains(";") == false) //simple case - first entry
            {
                startText = this.Text;
            }
            else
            {
                int currentPosition = this.SelectionStart;  //zero based
                int currentLength = this.Text.Length;
               

                string sBeforeCurrent = string.Empty;
                //string sAfterCurrent = string.Empty;

                if (currentPosition > 0)
                {
                    sBeforeCurrent = this.Text.Substring(0, currentPosition);
                    if (sBeforeCurrent.Contains(";"))
                    {
                        int sBeforeLastSemiPosition = sBeforeCurrent.LastIndexOf(";");
                        //get everything after the last semicolon
                        if (sBeforeLastSemiPosition < sBeforeCurrent.Length)
                        {
                            sBeforeCurrent = sBeforeCurrent.Substring(sBeforeLastSemiPosition + 1);
                        }
                    }
                   
                }

                //if (currentPosition < currentLength)
                //{
                //    sAfterCurrent = this.Text.Substring(currentPosition);
                //    if (sAfterCurrent.Contains(";"))
                //    {
                //        int sAfterFirstSemiPosition = sAfterCurrent.IndexOf(";");
                //        //get everything before the first semicolon
                //        if (sAfterFirstSemiPosition > 0)
                //        {
                //            sAfterCurrent = this.Text.Substring(0, sAfterFirstSemiPosition);
                //        }
                //    }
                   

                //}

                return sBeforeCurrent;
                
            }




            return startText;
        }
      

        private void CloseDropDownPanel()
        {
            if (_addressListBox.SelectedItems.Count > 0)
            {
                SetTextWithNewSelection();     
            }
            _addressListBox.ClearSelected();
            HideDropDown();
            this.Focus();
        }

        private void HideDropDown()
        {
            this._closingDropDown = true;
            _dropDownPanel.Visible = false;
            this._closingDropDown = false;
        }


        private void SetTextWithNewSelection()
        {
            // Identify the region (first entry where no semicolons exist, before first semicolon, in between semicolons, after last semicolon
            // Set the selected text based on the region
            // Replace selected text with new selected entry

            string newSelectedEntry = _addressListBox.SelectedItem.ToString();




            if (this.Text.Contains(";") == false) //simple case - first entry
            {
                _settingText = true;
                this.Text = newSelectedEntry;
                this.SelectAll();
                _settingText = false;
            }
            else
            {
                int currentPosition = this.SelectionStart;  //zero based
                int currentLength = this.Text.Length;
               

                string sBeforeCurrent = string.Empty;
                string sAfterCurrent = string.Empty;

                if (currentPosition > 0)
                {
                    sBeforeCurrent = this.Text.Substring(0, currentPosition);
                    if (sBeforeCurrent.Contains(";"))
                    {
                        int sBeforeLastSemiPosition = sBeforeCurrent.LastIndexOf(";");
                        //strip everything after the last semicolon
                        if (sBeforeLastSemiPosition + 1 < sBeforeCurrent.Length)
                        {
                            sBeforeCurrent = sBeforeCurrent.Substring(0, sBeforeLastSemiPosition + 1);
                        }
                    }
                    else
                    {
                        sBeforeCurrent = string.Empty;
                    }
                }

                if (currentPosition < currentLength)
                {
                    sAfterCurrent = this.Text.Substring(currentPosition);
                    if (sAfterCurrent.Contains(";"))
                    {
                        int sAfterFirstSemiPosition = sAfterCurrent.IndexOf(";");
                        //strip everything before the first semicolon
                        if (sAfterFirstSemiPosition < sAfterCurrent.Length)
                        {
                            sAfterCurrent = sAfterCurrent.Substring(sAfterFirstSemiPosition);
                        }
                        else
                        {
                            sAfterCurrent = string.Empty;
                        }
                    }
                    else
                    {
                        sAfterCurrent = string.Empty;
                    }
                    if (sAfterCurrent.StartsWith(";"))
                    {
                        sAfterCurrent = sAfterCurrent.Substring(1);
                    }
                }

                _settingText = true;
                this.Text = sBeforeCurrent + newSelectedEntry + sAfterCurrent;
                this.SelectionStart = sBeforeCurrent.Length;
                this.SelectionLength = newSelectedEntry.Length;
                _settingText = false;
            }


            //int lastsemi = this.Text.LastIndexOf(";");
            //if (lastsemi > 0)
            //{
            //    this.Text = this.Text.Substring(0, lastsemi + 1) + 
            //    this.SelectionStart = lastsemi + 1;
            //    this.SelectionLength = _addressListBox.SelectedItem.ToString().Length;
            //}
            
        }

  

        private void addressListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush textBrush = SystemBrushes.WindowText;

            if (e.Index > -1)
            {
                if (e.Index == _mouseIndex)
                {
                    e.Graphics.FillRectangle(SystemBrushes.HotTrack, e.Bounds);
                    textBrush = SystemBrushes.HighlightText;
                }
                else
                {
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                        textBrush = SystemBrushes.HighlightText;
                    }
                    else
                        e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
                }
                e.Graphics.DrawString(_addressListBox.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds.Left + 2, e.Bounds.Top);
            }
        }

        private void addressListBox_MouseMove(object sender, MouseEventArgs e)
        {
            int index = _addressListBox.IndexFromPoint(e.Location);
            if (index != _mouseIndex)
            {
                _mouseIndex = index;
                _addressListBox.Invalidate();
            }
        }

        private void addressListBox_MouseLeave(object sender, EventArgs e)
        {
            if (_mouseIndex > -1)
            {
                _mouseIndex = -1;
                _addressListBox.Invalidate();
            }
        }

        private bool UsingOutlookContacts()
        {
            try
            {
                return MIDEmail.UseOutlookContacts;
            }
            catch
            {
                return false;
            }
        }

        private bool IsOutlookInstalled()
        {
            try
            {
                return MIDEmail.IsOutlookInstalled();
            }
            catch
            {
                return false;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Designer causes problems
            //if (MIDEmail.UseOutlookContacts == false)
            //{
            //    return;
            //}
            if (UsingOutlookContacts() == false || IsOutlookInstalled() == false)
            {
                return;
            }



            _dropDownPanel = new Panel();
            _dropDownPanel.Visible = false;
            _dropDownPanel.Width = this.Width;
            _dropDownPanel.Height = 80;
            _dropDownPanel.Location = new Point(this.Left, this.Top + this.Height);

            _addressListBox = new ListBox();
            _addressListBox.Font = new Font(this.Font.FontFamily, this.Font.Size + 1);
            _addressListBox.ItemHeight = 16;
            _addressListBox.SelectionMode = SelectionMode.One;
            _addressListBox.DrawMode = DrawMode.OwnerDrawFixed;

            _addressListBox.KeyDown += (KeyEventHandler)((sender, k) =>
            {
                if ((k.KeyCode == Keys.Enter || k.KeyCode == Keys.Right) && _addressListBox.SelectedIndex >= 0)
                {
                    CloseDropDownPanel();
                }
            });
            _addressListBox.LostFocus += (EventHandler)((sender, k) =>
            {
                if (_closingDropDown == false && _addressListBox.SelectedIndex >= 0)
                {
                    CloseDropDownPanel();
                }
            });


            _addressListBox.Click += (EventHandler)((sender, arg) =>
            {
                CloseDropDownPanel();
            });

            _addressListBox.MouseMove += (MouseEventHandler)((sender, arg) =>
            {
                addressListBox_MouseMove(sender, arg);
            });
            _addressListBox.MouseLeave += (EventHandler)((sender, arg) =>
            {
                addressListBox_MouseLeave(sender, arg);
            });

            _addressListBox.DrawItem += (DrawItemEventHandler)((sender, arg) =>
            {
                addressListBox_DrawItem(sender, arg);
            });

            this.KeyDown += (KeyEventHandler)((sender, k) =>
            {
                if (k.KeyCode == Keys.Down && _dropDownPanel.Visible)
                {
                    _settingFocusToDropDown = true;
                    if (_addressListBox.Items.Count > 0)
                    {
                        _addressListBox.SelectedIndex = 0;
                    }
                    _addressListBox.Focus();
                    _settingFocusToDropDown = false;
                }
            });
            this.LostFocus += (EventHandler)((sender, arg) =>
            {
                if (_settingFocusToDropDown == false && _addressListBox.Focused == false)
                {
                    //close the dropdown
                    HideDropDown();
                }
            });
            this.Validating += (System.ComponentModel.CancelEventHandler)((sender, arg) =>
            {
                if (_settingFocusToDropDown == false && _addressListBox.Focused == false)
                {
                    //close the dropdown
                    HideDropDown();
                }
            });

            this.Resize += (EventHandler)((sender, arg) =>
            {

                //close the dropdown
                HideDropDown();

            });


            this.Parent.Controls.Add(_dropDownPanel);
            _dropDownPanel.Controls.Add(_addressListBox);
            _addressListBox.Dock = DockStyle.Fill;

            _emailAddressList = MIDEmail.GetOutlookContractList();

        }
    }
}
