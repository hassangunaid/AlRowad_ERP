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
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("ادارة النظام", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("دليل الحسابات");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("بيانات العملاء");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("ادارة الحسابات", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("بيانات العملاء");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("ادارة المبيعات", new System.Windows.Forms.TreeNode[] {
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("بيانات الموردين");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("ادارة المشتريات", new System.Windows.Forms.TreeNode[] {
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("بيانات الاصناف");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("ادارة المخازن", new System.Windows.Forms.TreeNode[] {
            treeNode11});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.treeView = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
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
            treeNode3.Name = "sec_SysAdmin";
            treeNode3.Text = "ادارة النظام";
            treeNode4.Name = "AccountsForm";
            treeNode4.Tag = "AccountsForm";
            treeNode4.Text = "دليل الحسابات";
            treeNode5.Name = "Customers";
            treeNode5.Tag = "Customers";
            treeNode5.Text = "بيانات العملاء";
            treeNode6.Name = "sec_AccManagement";
            treeNode6.Text = "ادارة الحسابات";
            treeNode7.Name = "Customers";
            treeNode7.Tag = "Customers";
            treeNode7.Text = "بيانات العملاء";
            treeNode8.Name = "sec_SalesManagement";
            treeNode8.Text = "ادارة المبيعات";
            treeNode9.Name = "Node4";
            treeNode9.Tag = "Suppliers";
            treeNode9.Text = "بيانات الموردين";
            treeNode10.Name = "Node0";
            treeNode10.Text = "ادارة المشتريات";
            treeNode11.Name = "Items";
            treeNode11.Tag = "Items";
            treeNode11.Text = "بيانات الاصناف";
            treeNode12.Name = "Node0";
            treeNode12.Text = "ادارة المخازن";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode6,
            treeNode8,
            treeNode10,
            treeNode12});
            this.treeView.Scrollable = false;
            this.treeView.Size = new System.Drawing.Size(391, 954);
            this.treeView.TabIndex = 0;
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_الشاشات_NodeMouseDoubleClick);
            this.treeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.butend);
            this.panel1.Controls.Add(this.data_day);
            this.panel1.Controls.Add(this.treeView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1898, 1024);
            this.panel1.TabIndex = 2;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // butend
            // 
            this.butend.BackColor = System.Drawing.SystemColors.Info;
            this.butend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.butend.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.butend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.butend.ForeColor = System.Drawing.Color.Black;
            this.butend.Location = new System.Drawing.Point(501, 170);
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

        public DragEventHandler x { get; private set; }
    }
}