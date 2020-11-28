namespace MIDRetail.Windows
{
    partial class ForecastAnalysisForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.forecastAnalysisControl1 = new MIDRetail.Windows.Controls.ForecastAnalysisControl();
            this.SuspendLayout();
            // 
            // forecastAnalysisControl1
            // 
            this.forecastAnalysisControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.forecastAnalysisControl1.Location = new System.Drawing.Point(0, 0);
            this.forecastAnalysisControl1.Name = "forecastAnalysisControl1";
            this.forecastAnalysisControl1.Size = new System.Drawing.Size(784, 562);
            this.forecastAnalysisControl1.TabIndex = 0;
            // 
            // ForecastAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.forecastAnalysisControl1);
            this.Name = "ForecastAnalysisForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Forecast Analysis";
            this.Load += new System.EventHandler(this.ForecastAnalysisForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ForecastAnalysisControl forecastAnalysisControl1;
    }
}