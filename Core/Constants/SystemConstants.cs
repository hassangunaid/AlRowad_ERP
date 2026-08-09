using AlRowad_ERP.Core.Entities;
using AlRowad_ERP.UI.Base;


namespace AlRowad_ERP.Core.Constants
{
    public static class SystemConstants
    {
        // الاحتفاظ ببيانات المستخدم المسجل حالياً (Session)
        public static UserEntity CurrentUser { get; set; }
        // رسائل النظام المركزية
        public const string Msg_LoginFailed = "اسم المستخدم أو كلمة المرور غير صحيحة، أو الحساب غير نشط.";
        public const string Msg_RequiredFields = "يرجى إدخال اسم المستخدم (رقم المستخدم) وكلمة المرور.";
        public const string Table_FinancialPeriods = "Financial_Periods";
        public const string Col_Is_Closed = "Is_Closed";
        public const string FinancialPeriods = "Financial_Periods";
        public const string Is_Closed = "Is_Closed";
        public const string CreatedBy = "Created_By";
        public const string UpdatedBy = "Updated_By";
        public const string CreatedAt = "Created_At";
        public const string UpdatedAt = "Updated_At";

        public const string UnexpectedSystemError = "عذراً، حدث خطأ غير متوقع في النظام. تم تسجيل المشكلة برقم تسلسلي وسيتم مراجعتها من قبل الدعم الفني.\n\nالرجاء إعادة المحاولة لاحقاً.";
        public const string SystemProtectionTitle = "حماية نظام الرواد ERP";

        // حقول الحذف المنطقي (Soft Delete)
        public const string IsDeleted = "Is_Deleted";
        public const string DeletedBy = "Deleted_By";
        public const string DeletedAt = "Deleted_At";
        public static class Statuses
        {
            public const string PeriodOpen = "Open";
            public const string PeriodClosed = "Closed";
        }

        public static class Messages
        {
            public const string InsufficientStock = "عفواً، الرصيد المخزني غير كافٍ لإتمام العملية.";
            public const string UnbalancedJournal = "لا يمكن حفظ القيد: إجمالي المدين لا يساوي إجمالي الدائن.";
            public const string ZeroAmountJournal = "لا يمكن حفظ قيد بقيمة صفرية.";
            public const string ClosedFinancialPeriod = "لا يمكن إتمام العملية: الفترة المالية مغلقة.";
            public const string SaveSuccess = "تم حفظ البيانات بنجاح.";
            public const string SaveFailed = "حدث خطأ أثناء الحفظ:";
            public const string CannotDeletePosted = "لا يمكن حذف مستند مُرحل إلى الحسابات. يرجى إلغاء الترحيل أولاً.";
            public const string ConfirmDelete = "هل أنت متأكد من حذف هذا السجل؟";
            public const string DeleteSuccess = "تم الحذف بنجاح.";
            public const string DeleteFailed = "فشل الحذف! السبب:";
            // ثوابت الحارس الأخير المطلوبة
            public const string UnexpectedSystemError = "عذراً، حدث خطأ غير متوقع في النظام. تم تسجيل المشكلة برقم تسلسلي وسيتم مراجعتها من قبل الدعم الفني.\n\nالرجاء إعادة المحاولة لاحقاً.";
            public const string SystemProtectionTitle = "حماية نظام الرواد ERP";
        }
                public static class AuditFields
        {
            public const string CreatedBy = "Created_By";
            public const string CreatedAt = "Created_At";
            public const string UpdatedBy = "Updated_By";
            public const string UpdatedAt = "Updated_At";
            public const string IsDeleted = "Is_Deleted";
            public const string DeletedBy = "Deleted_By";
            public const string DeletedAt = "Deleted_At";
        }

        public static class Descriptions
        {
            public const string SalesInvoice = "فاتورة مبيعات";
        }

        public static class Accounts
        {
            // سيتم ربط هذه الحسابات لاحقاً بقاعدة البيانات، حالياً نضع قيماً افتراضية
            public const int SalesRevenue = 4101;
        }
    }

}
