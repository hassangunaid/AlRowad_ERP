using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
namespace AlRowad_ERP.Forms
{
    // الوراثة من كلاس النخاع الشوكي (BaseForm) لضمان استقرار الواجهة الحاوية
    public partial class MainForm : BaseForm
    {
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 1. تثبيت الشاشة الرئيسية في وضع ملء الشاشة الصارم (مرة واحدة فقط عند الإقلاع وهي كافية جداً)
            this.WindowState = FormWindowState.Maximized;
            BindCurrentUserData();

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
                // 1. البحث الذكي المعماري: نبحث عن الكلاس بالاسم في كامل مساحات الأسماء (Namespaces)
                Type formType = System.Reflection.Assembly.GetExecutingAssembly()
                                .GetTypes()
                                .FirstOrDefault(t => t.Name == forms || t.FullName == forms);

                if (formType == null)
                {
                    MessageBox.Show($"خطأ معماري: الشاشة المطلوبة ({forms}) غير معرفة في أي مجلد داخل النظام.",
                                    "تنبيه النظام", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. إنشاء نسخة مستقلة تماماً (New Instance)
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

                // ثالثاً: استدعاء العرض والتركيز
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
        #region إدارة حالة المستخدم (User State Management)

        private void BindCurrentUserData()
        {
            // 1. إجراء أمني (Security Check): التأكد من أن هناك مستخدم مسجل دخول فعلياً
            if (SystemConstants.CurrentUser != null)
            {
                // 2. تطبيق الدستور: قراءة البيانات من الكيان المركزي 
                // نستخدم FullName لعرض الاسم الصريح، أو Username لعرض رقم/معرف المستخدم
                Uesr_Name.Text = SystemConstants.CurrentUser.FullName;

                // (اختياري) تقييد صلاحيات أخرى في الشاشة الرئيسية بناءً على الكيان
                // if (!SystemConstants.CurrentUser.IsSuperAdmin) { ... }
            }
            else
            {
                // 3. مسار الطوارئ (Fallback): في حال تم تجاوز شاشة الدخول بطريقة ما
                Uesr_Name.Text = "مستخدم غير مصرح";

                // معمارياً: يُفضل إغلاق الشاشة فوراً لحماية النظام المحاسبي
                MessageBox.Show("تم اكتشاف وصول غير مصرح به. سيتم إغلاق النظام.", "خرق أمني", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
        }

        #endregion
    

        // الحدث النبضي للـ Timer
        private void time_day_Tick(object sender, EventArgs e)
        {
            تحديث_التاريخ_الحالي();
        }

        private void BtnSwitchUser_Click(object sender, EventArgs e)
        {
            // فتح شاشة الدخول للحساب الجديد
            using (var loginForm = new LongUesr())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // تم تغيير SystemConstants.CurrentUser بنجاح في شاشة الدخول

                    // إنشاء شاشة رئيسية جديدة للمستخدم الجديد وتسجيلها في الرادار
                    MainForm newMain = new MainForm();
                    AlRowad_ERP.Core.FormMonitor.راقب_الشاشة(newMain);
                    newMain.Show();

                    // إغلاق الشاشة الرئيسية الحالية. 
                    // الشاشات الفرعية القديمة ستظل مفتوحة، والرادار لن يقتل النظام لأنها مسجلة فيه.
                    this.Close();
                }
            }
        }
    }

        // 🛑 تم حذف حدث MainForm_Activated بالكامل من هنا لأنه المسبب الأساسي لارتجاج الواجهات وانكماش حجمها!
    }
