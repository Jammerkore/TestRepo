using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
    public partial class MIDMultiColumnDropDown : UserControl
    {
        private bool _dropped = false;
        private int _originalHeight = 40;
        public event EventHandler MIDSelectionChanged;
        private bool blLoaded = false;
                
        public MIDMultiColumnDropDown()
        {
            InitializeComponent();
        }

        private void btnDrop_Click(object sender, EventArgs e)
        {
            if (_dropped == false)
            {
                _dropped = true;
                this.Height = this.pnlDropDown.Height + this.grdList.Height + 1;
            }
            else
            {
                _dropped = false;
                this.Height = _originalHeight;
            }

            this.BringToFront();
        }

        int intRowsToDisplay = 8;
        public int RowsToDisplay
        {
            get
            {
                return intRowsToDisplay;
            }
            set
            {
                intRowsToDisplay = value;
            }
        }


        DataTable dtDataSource = new DataTable();
        public DataTable MIDDataSource
        {
            get
            {
                return dtDataSource;
            }
            set
            {
                dtDataSource = value;
                grdList.DataSource = dtDataSource;
                for(int i = 0; i < grdList.Columns.Count; i++)
                {
                    DataGridViewColumn col = grdList.Columns[i];

                    col.Width = (grdList.Width / grdList.Columns.Count) -1;

                    col.SortMode = DataGridViewColumnSortMode.NotSortable;

                    TextBox txtDisplay = new TextBox();
                    txtDisplay.Name = "txtDisplay" + i.ToString().Trim();
                    txtDisplay.Width = col.Width - (btnDrop.Width / grdList.Columns.Count);
                    txtDisplay.Left = i * txtDisplay.Width;
                    txtDisplay.Top = pnlDropDown.Height - txtDisplay.Height;
                    txtDisplay.BorderStyle = BorderStyle.None;
                    pnlDropDown.Controls.Add(txtDisplay);

                    Label lblHeader = new Label();
                    lblHeader.Name = "lblHeader" + i.ToString().Trim();
                    lblHeader.Width = col.Width - (btnDrop.Width / grdList.Columns.Count);
                    lblHeader.Left = i * lblHeader.Width - 2;
                    lblHeader.Top = 2;
                    pnlHeaders.Controls.Add(lblHeader);

                }

                int gridHeight = 0;
                for (int n = 0; n < grdList.Rows.Count; n++)
                {
                    if (n < intRowsToDisplay)
                    {
                        gridHeight += grdList.Rows[n].Height;
                    }
                    else
                    {
                        break;
                    }
                }

                grdList.Height = gridHeight + grdList.Rows[0].Height;
                grdList.BackgroundColor = pnlDropDown.BackColor;

            }
        }

        private void MIDMultiColumnDropDown_Load(object sender, EventArgs e)
        {
            blLoaded = true;
        }

        private void grdList_SelectionChanged(object sender, EventArgs e)
        {
            if (blLoaded == true)
            {
                for (int i = 0; i < grdList.Columns.Count; i++)
                {
                    for (int n = 0; n < pnlDropDown.Controls.Count; n++)
                    {
                        if (pnlDropDown.Controls[n].Name == "txtDisplay" + i.ToString().Trim())
                        {
                            //pnlDropDown.Controls[n].Text = grdList.Rows[0].Cells[i].Value.ToString().Trim();
                        }
                    }

                    for (int k = 0; k < pnlHeaders.Controls.Count; k++)
                    {
                        if (pnlHeaders.Controls[k].Name == "lblHeader" + i.ToString().Trim())
                        {
                            pnlHeaders.Controls[k].Text = grdList.Columns[i].Name.ToString().Trim();
                        }
                    }
                }
            
                this.Height = _originalHeight;
                _dropped = false;

                MIDSelectionChanged(this, new EventArgs());
            }
        }

        private void MIDMultiColumnDropDown_Resize(object sender, EventArgs e)
        {
            if (_dropped == false)
            {
                pnlDropDown.Height = this.Height;
                pnlDropDown.Width = this.Width;
            }
        }

        private void pnlDropDown_Resize(object sender, EventArgs e)
        {
            grdList.Top = pnlDropDown.Height + 1;
            grdList.Width = pnlDropDown.Width;
            pnlHeaders.Width = pnlDropDown.Width - 3;
            btnDrop.Height = pnlDropDown.Height - 22;
            btnDrop.Left = grdList.Width - btnDrop.Width - 4;
            _originalHeight = this.Height;
        }

        DataGridViewRow drRow = new DataGridViewRow();
        public DataGridViewRow SelectedRow
        {
            get
            {
                drRow = grdList.SelectedRows[0];

                return drRow;
            }
        }

    }
}
