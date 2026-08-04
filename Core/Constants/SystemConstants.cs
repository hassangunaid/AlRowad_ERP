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

        // حقول الحذف المنطقي (Soft Delete)
        public const string IsDeleted = "Is_Deleted";
        public const string DeletedBy = "Deleted_By";
        public const string DeletedAt = "Deleted_At";

    }
}