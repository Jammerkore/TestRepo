using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
    public partial class HelpHTMLControl : UserControl
    {
        public HelpHTMLControl()
        {
            InitializeComponent();


        }


        public void LoadHelp(string spath)
        {
            //string sStart = "about:blank";
            // webBrowser1.Navigate(sStart);
            //HtmlDocument doc = this.webBrowser1.Document.OpenNew(false);
            //doc.Write(GetHeaderFilterHelp());
            //webBrowser1.Refresh();
            webBrowser1.Navigate(spath);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.ultraToolbarsManager1.Tools["btnBack"].SharedProps.Enabled = webBrowser1.CanGoBack;
            this.ultraToolbarsManager1.Tools["btnForward"].SharedProps.Enabled = webBrowser1.CanGoForward;
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnBack":
                    webBrowser1.GoBack();
                    break;
                case "btnForward":
                    webBrowser1.GoForward();
                    break;

            }
        }


        //public string GetHeaderFilterHelp()
        //{
        //    string s = string.Empty;
        //    s += "<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01//EN' 'http://www.w3.org/TR/html4/strict.dtd'>";
        //    s += "<html>";
        //    s += "<head>";
        //    s += "  <meta content='text/html; charset=ISO-8859-1'";
        //    s += " http-equiv='content-type'>";
        //    s += "  <title>Store Filters</title>";
        //    s += "</head>";
        //    s += "<body>";
        //    s += "<table style='text-align: left; width: 100%;' border='0'";
        //    s += " cellpadding='2' cellspacing='2'>";
        //    s += "  <tbody>";
        //    s += "    <tr>";
        //    s += "      <td>";
        //    s += "      <h1";
        //    s += " style='font-weight: normal; font-family: Helvetica,Arial,sans-serif;'><big>Store";
        //    s += "Filters</big></h1>";
        //    s += "      </td>";
        //    s += "      <td></td>";
        //    s += "      <td style='text-align: right;'><img";
        //    s += " style='width: 368px; height: 106px;' alt='MID Retail, Inc.'";
        //    s += " src='Header%20Filter%20Help_files/image003.gif'></td>";
        //    s += "    </tr>";
        //    s += "  </tbody>";
        //    s += "</table>";
        //    s += "<hr";
        //    s += " style='width: 100%; height: 2px; font-family: Helvetica,Arial,sans-serif;'>";
        //    s += "<h2";
        //    s += " style='font-family: Helvetica,Arial,sans-serif; color: rgb(1, 0, 0);'><big";
        //    s += " style='font-weight: normal;'>Overview</big></h2>";
        //    s += "<span style='font-family: Helvetica,Arial,sans-serif;'>First,";
        //    s += "select a condtion type, then click the add button.&nbsp; The";
        //    s += "condition types are described in detail below.</span><br>";
        //    s += "<br>";
        //    s += "<h2";
        //    s += " style='font-family: Helvetica,Arial,sans-serif; color: rgb(1, 0, 0);'><big";
        //    s += " style='font-weight: normal;'>Specific Store</big></h2>";
        //    s += "<span style='font-family: Helvetica,Arial,sans-serif;'>First,";
        //    s += "select a condtion type, then click the add button.&nbsp; The";
        //    s += "condition types are described in detail below.<br>";
        //    s += "<br>";
        //    s += "</span>";
        //    s += "<h2";
        //    s += " style='font-family: Helvetica,Arial,sans-serif; color: rgb(1, 0, 0);'><big";
        //    s += " style='font-weight: normal;'>Store Attributes</big></h2>";
        //    s += "<span style='font-family: Helvetica,Arial,sans-serif;'>First,";
        //    s += "select a condtion type, then click the add button.&nbsp; The";
        //    s += "condition types are described in detail below.<br>";
        //    s += "<br>";
        //    s += "</span>";
        //    s += "<h2";
        //    s += " style='font-family: Helvetica,Arial,sans-serif; color: rgb(1, 0, 0);'><big";
        //    s += " style='font-weight: normal;'>Store Status</big></h2>";
        //    s += "<span style='font-family: Helvetica,Arial,sans-serif;'>First,";
        //    s += "select a condtion type, then click the add button.&nbsp; The";
        //    s += "condition types are described in detail below.<br>";
        //    s += "<br>";
        //    s += "</span>";
        //    s += "<h2";
        //    s += " style='font-family: Helvetica,Arial,sans-serif; color: rgb(1, 0, 0);'><big";
        //    s += " style='font-weight: normal;'>Variable to Constant</big></h2>";
        //    s += "<span style='font-family: Helvetica,Arial,sans-serif;'>First,";
        //    s += "select a condtion type, then click the add button.&nbsp; The";
        //    s += "condition types are described in detail below.<br>";
        //    s += "<br>";
        //    s += "</span>";
        //    s += "<h2";
        //    s += " style='font-family: Helvetica,Arial,sans-serif; color: rgb(1, 0, 0);'><big";
        //    s += " style='font-weight: normal;'>Variable to Variable</big></h2>";
        //    s += "<span style='font-family: Helvetica,Arial,sans-serif;'>First,";
        //    s += "select a condtion type, then click the add button.&nbsp; The";
        //    s += "condition types are described in detail below.</span><br>";
        //    s += "<br>";
        //    s += "<h2";
        //    s += " style='font-family: Helvetica,Arial,sans-serif; color: rgb(1, 0, 0);'><big";
        //    s += " style='font-weight: normal;'>Variable Percentage</big></h2>";
        //    s += "<span style='font-family: Helvetica,Arial,sans-serif;'>First,";
        //    s += "select a condtion type, then click the add button.&nbsp; The";
        //    s += "condition types are described in detail below.</span>";
        //    s += "</body>";
        //    s += "</html>";
        //    return s;

        //}


    }
}
