using AlRowad_ERP.Core.Entities;
using AlRowad_ERP.UI.Base;

namespace AlRowad_ERP.Core.Constants
{
    public static class SystemConstants
    {
        // ---------------------------------------------------
        // 1. بيانات الجلسة (Session State)
        // ---------------------------------------------------
        public static UserEntity CurrentUser { get; set; }

        // ---------------------------------------------------
        // 2. الفروع (Nested Classes) لتنظيم الثوابت
        // ---------------------------------------------------

        public static class Messages
        {
            public const string LoginFailed = "اسم المستخدم أو كلمة المرور غير صحيحة، أو الحساب غير نشط.";
            public const string RequiredFields = "يرجى إدخال اسم المستخدم (رقم المستخدم) وكلمة المرور.";
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
            public const string UnexpectedSystemError = "عذراً، حدث خطأ غير متوقع في النظام. تم تسجيل المشكلة برقم تسلسلي وسيتم مراجعتها من قبل الدعم الفني.\n\nالرجاء إعادة المحاولة لاحقاً.";
            public const string SystemProtectionTitle = "حماية نظام الرواد ERP";
        }

        public static class Tables
        {
            public const string FinancialPeriods = "Financial_Periods";
            public const string Units = "Units";
            public const string Items = "Items";
            public const string Item_Balances = "Item_Balances";
        }

        public static class Columns
        {
            // الحقول العامة والرقابية
            public const string Is_Closed = "Is_Closed";
            public const string Is_Deleted = "Is_Deleted";
            public const string Created_By = "Created_By";
            public const string Created_At = "Created_At";
            public const string Updated_By = "Updated_By";
            public const string Updated_At = "Updated_At";
            public const string Deleted_By = "Deleted_By";
            public const string Deleted_At = "Deleted_At";

            // حقول المخزون والأصناف والوحدات
            public const string Store_ID = "Store_ID";
            public const string Quantity = "Quantity";
            public const string Unit_ID = "Unit_ID";
            public const string Unit_Name = "Unit_Name";
            public const string Conversion_Factor = "Conversion_Factor";
            public const string Item_ID = "Item_ID";
            public const string Item_Name = "Item_Name";
            public const string Base_Unit_ID = "Base_Unit_ID";
            public const string Default_Price = "Default_Price";
            public const string Min_Sale_Price = "Min_Sale_Price";
            public const string Reorder_Level = "Reorder_Level";
        }

        public static class Statuses
        {
            public const string PeriodOpen = "Open";
            public const string PeriodClosed = "Closed";
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
            public const int SalesRevenue = 4101;
        }
    }
}