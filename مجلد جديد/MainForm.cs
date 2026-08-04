using AlRowad_ERP.Core;
using System;
using System.Windows.Forms;
using System.Globalization;

namespace AlRowad_ERP.Forms
{
    // الوراثة من كلاس النخاع الشوكي (BaseForm) لضمان استقرار الواجهة الحاوية
    public partial class MainForm : BaseForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 1. تثبيت الشاشة الرئيسية في وضع ملء الشاشة الصارم (مرة واحدة فقط عند الإقلاع وهي كافية جداً)
            this.WindowState = FormWindowState.Maximized;

            // 2. تشغيل التوقيت فوراً عند تحميل الشاشة
            تحديث_التاريخ_الحالي();
        }

        // الحدث العالمي المعتمد لفتح الشاشات بالماوس (النقر المزدوج فقط)
        private void tree_الشاشات_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null) return;

            // تجاهل المجموعات (القسم الأب)
            if (e.Node.Name.StartsWith("sec_"))
            {
                return;
            }

            // فتح الشاشة عند النقر المزدوج على عنصر لديه Tag
            if (e.Node.Tag != null && !string.IsNullOrEmpty(e.Node.Tag.ToString()))
            {
                string forms = e.Node.Tag.ToString();

                // 🛠️ الحل: تأخير الفتح إلى ما بعد انتهاء نقرات الماوس تماماً
                this.BeginInvoke(new Action(() => {
                    فتح_الشاشة_ديناميقيا(forms);
                }));
            }
        }
        // نظام فتح الشاشات عبر لوحة المفاتيح (زر Enter بدون صوت تنبيه)
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // منع الصوت بشكل آني فقط لحدث Enter
                e.SuppressKeyPress = true;

                TreeView tree = sender as TreeView;
                if (tree != null && tree.SelectedNode != null)
                {
                    TreeNode selectedNode = tree.SelectedNode;

                    // عند ضغط Enter على مجموعة: نقوم بتبديل حالتها بأمان
                    if (selectedNode.Name.StartsWith("sec_"))
                    {
                        if (selectedNode.IsExpanded)
                            selectedNode.Collapse();
                        else
                            selectedNode.Expand();

                        return;
                    }

                    // عند ضغط Enter على شاشة فرعية: نفتح الواجهة فوراً
                    if (selectedNode.Tag != null && !string.IsNullOrEmpty(selectedNode.Tag.ToString()))
                    {
                        string forms = selectedNode.Tag.ToString();
                        فتح_الشاشة_ديناميقيا(forms);
                    }
                }
            }
        }

        // محرك الـ Reflection الديناميكي المحمي لتوليد شاشات الـ ERP في الذاكرة الحرة
        private void فتح_الشاشة_ديناميقيا(string forms)
        {
            try
            {
                // 1. بناء الاسم الكامل للكلاس متضمناً الـ Namespace
                string fullTypeName = $"AlRowad_ERP.Forms.{forms}";

                // 2. جلب نوع (Type) الشاشة ديناميكياً باستخدام الـ Reflection
                Type formType = Type.GetType(fullTypeName);

                if (formType == null)
                {
                    MessageBox.Show($"خطأ معماري: الشاشة المطلوبة ({forms}) غير معرفة في مجلد Forms.",
                                    "تنبيه النظام", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3. إنشاء نسخة مستقلة تماماً (New Instance) في كل مرة يتم فيها الاستدعاء
                Form frm = (Form)Activator.CreateInstance(formType);

                // =========================================================================
                // 🚀 تحرير الواجهات واستقرار الأبعاد الثابتة
                // =========================================================================

                // أولاً: ضبط موضع الانطلاق في مركز الشاشة لتظهر بشكل مريح للمستخدم
                frm.StartPosition = FormStartPosition.CenterScreen;

                // ثانياً: التأكد من أن النافذة تظهر كشاشة مستقلة تماماً في نظام التشغيل
                frm.ShowInTaskbar = true;

                // دستور الرواد: إدخال الشاشة الجديدة في نظام الرصد العائم قبل ظهورها
                AlRowad_ERP.Core.FormMonitor.راقب_الشاشة(frm);

                // ثالثاً: استدعاء العرض والتركيز (مرة واحدة فقط وبشكل حاسم ونظيف) ✅
                frm.Show();
                Application.DoEvents();
                frm.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل في توليد واجهة المستخدم المتعددة. التفاصيل: {ex.Message}",
                                "خطأ نظام حرج", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // زر الخروج - يغلق الشاشة الرئيسية فقط
        private void butend_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // دالة مخصصة لجلب التاريخ واسم اليوم بالتنسيق العربي القياسي للرواد
        private void تحديث_التاريخ_الحالي()
        {
            try
            {
                var arabicCulture = new CultureInfo("ar-YE");
                string dateText = DateTime.Now.ToString("dddd، dd MMMM yyyy", arabicCulture);

                if (data_day != null)
                {
                    data_day.Text = dateText;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ في تحديث الوقت: {ex.Message}");
            }
        }

        // الحدث النبضي للـ Timer
        private void time_day_Tick(object sender, EventArgs e)
        {
            تحديث_التاريخ_الحالي();
        }

        // 🛑 تم حذف حدث MainForm_Activated بالكامل من هنا لأنه المسبب الأساسي لارتجاج الواجهات وانكماش حجمها!
    }
}