using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
    public partial class SearchLoadingUI : UserControl
    {
        public SearchLoadingUI()
        {
            InitializeComponent();
        }



        public void SetText(string sText)
        {

            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.ultraLabel1.Text = sText;
                    });
                }
                else
                {
                    this.ultraLabel1.Text = sText;
                }
            }
            catch
            {
            }

        }

        private int imageIndex = 0;
        public void SetNextImage()
        {
            imageIndex++;
            if (imageIndex == this.imageList1.Images.Count)
            {
                imageIndex = 0;
            }
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.pictureBox1.Image = this.imageList1.Images[imageIndex];
                    });
                }
                else
                {
                    this.pictureBox1.Image = this.imageList1.Images[imageIndex];
                }
            }
            catch
            {
            }

        }
        public void UpdateSpinner()
        {
            try
            {
                DateTime _startTotalTime = DateTime.Now;
                DateTime lastUpdated = _startTotalTime;
                bool keepGoing = true;

                while (keepGoing)
                {
                    DateTime timeNow = System.DateTime.Now;


                    //if (timeNow.Subtract(lastUpdated).TotalMilliseconds > 3 * ((imageIndex) + 1))
                    if (timeNow.Subtract(lastUpdated).TotalMilliseconds > 50)
                    {
                        lastUpdated = timeNow;
                        //    TimeSpan ts = timeNow.Subtract(_startTotalTime);


                        //    //this.Invoke((MethodInvoker)delegate
                        //    //{
                        //    //    this.ultraStatusBar1.Panels["pnlTime"].Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
                        //    //});
                        //    if (spinnerCount == 5)
                        //    {
                        //        spinnerCount = 0;
                        //        this.searchLoadingUI1.SetText(spinnerInitialText);
                        //    }
                        //    else
                        //    {
                        //        spinnerCount++;
                        //        string s = this.searchLoadingUI1.GetText();
                        //        s += ".";
                        //        this.searchLoadingUI1.SetText(s);
                        //    }
                        //    Application.DoEvents();
                        //this.searchLoadingUI1.SetNextImage();
                        imageIndex++;
                        if (imageIndex == this.imageList1.Images.Count)
                        {
                            imageIndex = 0;
                        }
                        try
                        {
                            if (this.InvokeRequired)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    this.pictureBox1.Image = this.imageList1.Images[imageIndex];
                                });
                            }
                            else
                            {
                                this.pictureBox1.Image = this.imageList1.Images[imageIndex];
                            }
                        }
                        catch
                        {
                        }
                    }

                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();  //prevent warning
            }
        }

        public string GetText()
        {
            try
            {
                return this.ultraLabel1.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            this.ultraButton1.Enabled = false;
            this.pictureBox1.Visible = false;
            SetText("Request has been canceled.");
            RaiseCancelProcessEvent();
        }

        public delegate void CancelProcessEventHandler(object sender, CancelProcessEventArgs e);
        public event CancelProcessEventHandler CancelProcessEvent;
        public void RaiseCancelProcessEvent()
        {
            if (CancelProcessEvent != null)
                CancelProcessEvent(new object(), new CancelProcessEventArgs());
        }
        public class CancelProcessEventArgs
        {
            public CancelProcessEventArgs() { }
        }


    }
}
