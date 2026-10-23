namespace AlRowad_ERP.Reports.Form
{
    partial class Frm_ReportViewer
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControlReports = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabControlReports
            // 
            this.tabControlReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlReports.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.tabControlReports.Location = new System.Drawing.Point(0, 0);
            this.tabControlReports.Name = "tabControlReports";
            this.tabControlReports.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabControlReports.RightToLeftLayout = true;
            this.tabControlReports.SelectedIndex = 0;
            this.tabControlReports.Size = new System.Drawing.Size(1000, 700);
            this.tabControlReports.TabIndex = 0;
            // 
            // Frm_ReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.tabControlReports);
            this.Name = "Frm_ReportViewer";
            this.Text = "عارض التقارير المركزي - نظام الرواد ERP";
            this.ResumeLayout(false);
        }

        public System.Windows.Forms.TabControl tabControlReports;
    }
}