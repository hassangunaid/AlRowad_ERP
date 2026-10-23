using AlRowad_ERP.Core;
using System;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    partial class MainForm 
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("تهيئة الوحدات");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("تهيئة العملات");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("ادارة الشاشات");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("بيانات المؤسسة");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("النسخ الاحتياطي");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("ادارة النظام", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("بيانات العملاء");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("تقرير كشف حساب");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("تقارير ادارة الحسابات", new System.Windows.Forms.TreeNode[] {
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("سند صرف");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("سند قبض");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("عمليات ادارة الحسابات", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("دليل الحسابات");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("الارصدة الافتتاحية");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("مدخلات ادارة الحسابات", new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode14});
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("ادارة الحسابات", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode9,
            treeNode12,
            treeNode15});
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("بيانات العملاء");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("ادارة المبيعات", new System.Windows.Forms.TreeNode[] {
            treeNode17});
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("بيانات الموردين");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("ادارة المشتريات", new System.Windows.Forms.TreeNode[] {
            treeNode19});
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("بيانات الاصناف");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("ادارة المخازن", new System.Windows.Forms.TreeNode[] {
            treeNode21});
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("ادارة نظام الوكلاء");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("بيانات السائقين");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("فاتورة الحراج الفوري");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("تجميع الحراجات");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("ادارةا الوكيل وسيط", new System.Windows.Forms.TreeNode[] {
            treeNode25,
            treeNode26});
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("بيانات الحمولة ");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("تقيم الحمولات");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("فاتورة شراء ");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("ادارة الوكيل مسوق", new System.Windows.Forms.TreeNode[] {
            treeNode28,
            treeNode29,
            treeNode30});
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("نـظـام ادارة الـوكـلاء", new System.Windows.Forms.TreeNode[] {
            treeNode23,
            treeNode24,
            treeNode27,
            treeNode31});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.treeView = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnSwitchUser = new System.Windows.Forms.Button();
            this.Uesr_Name = new System.Windows.Forms.TextBox();
            this.butend = new System.Windows.Forms.Button();
            this.data_day = new System.Windows.Forms.Label();
            this.time_day = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.AccessibleRole = System.Windows.Forms.AccessibleRole.SpinButton;
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.treeView.LineColor = System.Drawing.Color.Brown;
            this.treeView.Location = new System.Drawing.Point(20, 57);
            this.treeView.Margin = new System.Windows.Forms.Padding(4);
            this.treeView.Name = "treeView";
            treeNode1.Name = "Unitsform";
            treeNode1.Tag = "UnitsForm";
            treeNode1.Text = "تهيئة الوحدات";
            treeNode2.Name = "Currencies";
            treeNode2.Tag = "Currencies";
            treeNode2.Text = "تهيئة العملات";
            treeNode3.Name = "Frm_ThemeManager";
            treeNode3.Tag = "Frm_ThemeManager";
            treeNode3.Text = "ادارة الشاشات";
            treeNode4.Name = "Frm_CompanyInfo";
            treeNode4.Tag = "Frm_CompanyInfo";
            treeNode4.Text = "بيانات المؤسسة";
            treeNode5.Name = "FrmBackupSettings";
            treeNode5.Tag = "FrmBackupSettings";
            treeNode5.Text = "النسخ الاحتياطي";
            treeNode6.Name = "sec_SysAdmin";
            treeNode6.Text = "ادارة النظام";
            treeNode7.Name = "Customers";
            treeNode7.Tag = "Customers";
            treeNode7.Text = "بيانات العملاء";
            treeNode8.Name = "Frm_AccountStatement";
            treeNode8.Tag = "Frm_AccountStatement";
            treeNode8.Text = "تقرير كشف حساب";
            treeNode9.Name = "";
            treeNode9.Text = "تقارير ادارة الحسابات";
            treeNode10.Name = "PaymentVoucher";
            treeNode10.Tag = "PaymentVoucher";
            treeNode10.Text = "سند صرف";
            treeNode11.Name = "ReceiptVoucher";
            treeNode11.Tag = "ReceiptVoucher";
            treeNode11.Text = "سند قبض";
            treeNode12.Name = "";
            treeNode12.Text = "عمليات ادارة الحسابات";
            treeNode13.Name = "AccountsForm";
            treeNode13.Tag = "AccountsForm";
            treeNode13.Text = "دليل الحسابات";
            treeNode14.Name = "Frm_OpeningBalances";
            treeNode14.Tag = "Frm_OpeningBalances";
            treeNode14.Text = "الارصدة الافتتاحية";
            treeNode15.Name = "Node1";
            treeNode15.Text = "مدخلات ادارة الحسابات";
            treeNode16.Name = "sec_AccManagement";
            treeNode16.Text = "ادارة الحسابات";
            treeNode17.Name = "Customers";
            treeNode17.Tag = "Customers";
            treeNode17.Text = "بيانات العملاء";
            treeNode18.Name = "sec_SalesManagement";
            treeNode18.Text = "ادارة المبيعات";
            treeNode19.Name = "Node4";
            treeNode19.Tag = "Suppliers";
            treeNode19.Text = "بيانات الموردين";
            treeNode20.Name = "Node0";
            treeNode20.Text = "ادارة المشتريات";
            treeNode21.Name = "Items";
            treeNode21.Tag = "Items";
            treeNode21.Text = "بيانات الاصناف";
            treeNode22.Name = "Node0";
            treeNode22.Text = "ادارة المخازن";
            treeNode23.Name = "AgencySettingsForm";
            treeNode23.Tag = "AgencySettingsForm";
            treeNode23.Text = "ادارة نظام الوكلاء";
            treeNode24.Name = "Node0";
            treeNode24.Tag = "DriverForm";
            treeNode24.Text = "بيانات السائقين";
            treeNode25.Name = "AuctionReceiptForm";
            treeNode25.Tag = "AuctionReceiptForm";
            treeNode25.Text = "فاتورة الحراج الفوري";
            treeNode26.Name = "Node9";
            treeNode26.Tag = "AuctionSalesDetails";
            treeNode26.Text = "تجميع الحراجات";
            treeNode27.Name = "Node3";
            treeNode27.Text = "ادارةا الوكيل وسيط";
            treeNode28.Name = "Node6";
            treeNode28.Tag = "ShipmentReceiptForm";
            treeNode28.Text = "بيانات الحمولة ";
            treeNode29.Name = "Node7";
            treeNode29.Tag = "ShipmentEvaluationForm";
            treeNode29.Text = "تقيم الحمولات";
            treeNode30.Name = "Node8";
            treeNode30.Tag = "FarmerInvoiceForm";
            treeNode30.Text = "فاتورة شراء ";
            treeNode31.Name = "Node5";
            treeNode31.Text = "ادارة الوكيل مسوق";
            treeNode32.Name = "AgentsManagemen";
            treeNode32.Tag = "";
            treeNode32.Text = "نـظـام ادارة الـوكـلاء";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode16,
            treeNode18,
            treeNode20,
            treeNode22,
            treeNode32});
            this.treeView.Scrollable = false;
            this.treeView.Size = new System.Drawing.Size(391, 954);
            this.treeView.TabIndex = 0;
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_الشاشات_NodeMouseDoubleClick);
            this.treeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.BtnSwitchUser);
            this.panel1.Controls.Add(this.Uesr_Name);
            this.panel1.Controls.Add(this.butend);
            this.panel1.Controls.Add(this.data_day);
            this.panel1.Controls.Add(this.treeView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1898, 1024);
            this.panel1.TabIndex = 2;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // BtnSwitchUser
            // 
            this.BtnSwitchUser.BackColor = System.Drawing.SystemColors.Info;
            this.BtnSwitchUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnSwitchUser.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BtnSwitchUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSwitchUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.BtnSwitchUser.ForeColor = System.Drawing.Color.Black;
            this.BtnSwitchUser.Location = new System.Drawing.Point(815, 38);
            this.BtnSwitchUser.Name = "BtnSwitchUser";
            this.BtnSwitchUser.Size = new System.Drawing.Size(99, 48);
            this.BtnSwitchUser.TabIndex = 3;
            this.BtnSwitchUser.Text = "تبديل";
            this.BtnSwitchUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSwitchUser.UseVisualStyleBackColor = false;
            this.BtnSwitchUser.Click += new System.EventHandler(this.BtnSwitchUser_Click);
            // 
            // Uesr_Name
            // 
            this.Uesr_Name.Location = new System.Drawing.Point(1652, 110);
            this.Uesr_Name.Name = "Uesr_Name";
            this.Uesr_Name.Size = new System.Drawing.Size(234, 30);
            this.Uesr_Name.TabIndex = 2;
            // 
            // butend
            // 
            this.butend.BackColor = System.Drawing.SystemColors.Info;
            this.butend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.butend.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.butend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.butend.ForeColor = System.Drawing.Color.Black;
            this.butend.Location = new System.Drawing.Point(697, 38);
            this.butend.Name = "butend";
            this.butend.Size = new System.Drawing.Size(99, 48);
            this.butend.TabIndex = 1;
            this.butend.Text = "خــروج";
            this.butend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.butend.UseVisualStyleBackColor = false;
            this.butend.Click += new System.EventHandler(this.butend_Click);
            // 
            // data_day
            // 
            this.data_day.BackColor = System.Drawing.Color.White;
            this.data_day.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.data_day.Location = new System.Drawing.Point(1655, 40);
            this.data_day.Name = "data_day";
            this.data_day.Size = new System.Drawing.Size(231, 46);
            this.data_day.TabIndex = 0;
            this.data_day.Text = "TIM";
            this.data_day.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.data_day.Click += new System.EventHandler(this.data_day_Click);
            // 
            // time_day
            // 
            this.time_day.Enabled = true;
            this.time_day.Interval = 1000;
            this.time_day.Tick += new System.EventHandler(this.time_day_Tick);
            // 
            // MainForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1898, 1024);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void data_day_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private Panel panel1;
        private Label data_day;
        private Timer time_day;
        private Button butend;
        private TextBox Uesr_Name;
        private Button BtnSwitchUser;

        public DragEventHandler x { get; private set; }
    }
}