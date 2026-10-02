using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Entities;
using AlRowad_ERP.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    public partial class LongUesr : Form
    {
        // منع تنفيذ حدث التغيير أثناء البرمجة لتجنب الحلقات اللانهائية (Infinite Loops)
        private bool _isSyncing = false;

        public LongUesr()
        {
            InitializeComponent();

            textBox4.UseSystemPasswordChar = true;

            // ربط الأحداث المعمارية
            this.Load += LongUesr_Load;
            button1.Click += BtnLogin_Click;
            button2.Click += BtnCancel_Click;

            // أحداث التزامن بين رقم المستخدم والقائمة المنسدلة
            textBox3.TextChanged += TextBox3_TextChanged;
            Name_User.SelectionChangeCommitted += Name_User_SelectionChangeCommitted;
        }
        // السلاسة والأداء: تحميل البيانات والذاكرة المحلية عند فتح الشاشة
        private async void LongUesr_Load(object sender, EventArgs e)
        {
            try
            {
                var users = await AuthRepository.GetAllActiveUsersAsync();

                Name_User.DataSource = users;
                Name_User.DisplayMember = "FullName"; // عرض الاسم بالعربي للمستخدم
                Name_User.ValueMember = "Username";   // القيمة المخفية هي رقم المستخدم (أو UserID حسب تصميمك)
                Name_User.SelectedIndex = -1;

                LoadRememberedSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات النظام: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #region محرك المفتاح الذكي (Smart Enter Navigation) - الإصدار المرحلي

        // تجاوز الدالة السيادية لالتقاط المفاتيح على مستوى الشاشة بالكامل
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // التحقق مما إذا كان المفتاح المضغوط هو مفتاح الإدخال (Enter)
            if (keyData == Keys.Enter)
            {
                // 1. مرحلة الفحص والاعتماد (الضغطة الأولى): 
                if (this.ActiveControl == textBox4)
                {
                    // التحقق المبدئي: هل قام المستخدم بكتابة شيء؟
                    if (string.IsNullOrWhiteSpace(textBox4.Text))
                    {
                        MessageBox.Show(SystemConstants.Messages.RequiredFields, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return true; // إيقاف العملية وإبقاء المؤشر في مكانه
                    }

                    // الاعتماد: الكلمة مكتوبة ومفحوصة مبدئياً. 
                    // نقل التركيز لزر "دخول" ليكون جاهزاً للضغطة الثانية
                    button1.Focus();
                    return true; // إخبار النظام أنه تم التعامل مع المفتاح بنجاح (الضغطة الأولى انتهت)
                }

                // 2. مرحلة التنفيذ (الضغطة الثانية):
                // إذا كان التركيز أصبح على زر "دخول" أو "إلغاء"، دع النظام ينفذ النقر الطبيعي
                if (this.ActiveControl == button1 || this.ActiveControl == button2)
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }

                // 3. مرحلة التنقل (Navigation/Tab):
                // لأي أداة أخرى (رقم المستخدم، القائمة المنسدلة، تذكرني)، حوّل الـ Enter إلى Tab
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
                return true;
            }

            // السماح بباقي المفاتيح بالعمل بشكلها الافتراضي
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
        #region محرك التزامن (Sync Engine)

        // 1. عند كتابة رقم المستخدم في الحقل، يتم اختيار الاسم العربي تلقائياً
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            // التأكد من عدم وجود تكرار للتنفيذ (Infinite Loop) وعدم كون الحقل فارغاً
            if (_isSyncing || Name_User.DataSource == null || string.IsNullOrWhiteSpace(textBox3.Text))
                return;

            try
            {
                _isSyncing = true;
                // مطابقة الرقم المكتوب بالقيمة المخفية (ValueMember) للقائمة المنسدلة
                Name_User.SelectedValue = textBox3.Text.Trim();
            }
            finally
            {
                _isSyncing = false;
            }
        }

        // 2. عند اختيار الاسم العربي من القائمة، يتم جلب رقم المستخدم ووضعه في textBox3
        private void Name_User_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_isSyncing || Name_User.SelectedValue == null)
                return;

            try
            {
                _isSyncing = true;
                // أخذ رقم المستخدم المرتبط بالاسم المختار ووضعه في حقل النص
                textBox3.Text = Name_User.SelectedValue.ToString();
            }
            finally
            {
                _isSyncing = false;
            }
        }

        #endregion
        
        #region إدارة الذاكرة المحلية (Remember Me)

        private void LoadRememberedSettings()
        {
            // استدعاء القيم من إعدادات الويندوز المحفوظة
            if (Properties.Settings.Default.RememberMe)
            {
                checkBox1.Checked = true; // مربع "تذكرني"
                textBox3.Text = Properties.Settings.Default.LastUsername;
                textBox4.Focus(); // توجيه المؤشر لكلمة السر مباشرة لسهولة الاستخدام
            }
        }

        private void SaveRememberedSettings()
        {
            if (checkBox1.Checked)
            {
                Properties.Settings.Default.RememberMe = true;
                Properties.Settings.Default.LastUsername = textBox3.Text.Trim();
            }
            else
            {
                Properties.Settings.Default.RememberMe = false;
                Properties.Settings.Default.LastUsername = string.Empty;
            }
            Properties.Settings.Default.Save(); // تنفيذ الحفظ محلياً
        }

        #endregion

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            // أخذ رقم المستخدم من الحقل المخصص له
            string userNumber = textBox3.Text.Trim();
            string password = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(userNumber) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(SystemConstants.Messages.RequiredFields, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                button1.Enabled = false;
                this.UseWaitCursor = true;

                // إرسال (رقم المستخدم) بدلاً من الاسم إلى طبقة التحقق
                UserEntity loggedInUser = await AuthRepository.AuthenticateUserAsync(userNumber, password);

                if (loggedInUser != null)
                {
                    SaveRememberedSettings();
                    SystemConstants.CurrentUser = loggedInUser;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(SystemConstants.Messages.LoginFailed, "فشل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox4.Clear();
                    textBox4.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء الاتصال بقاعدة البيانات: \n" + ex.Message, "خطأ نظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
                this.UseWaitCursor = false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}