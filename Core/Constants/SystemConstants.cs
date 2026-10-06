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
            public const string Suppliers = "Suppliers";
            public const string Drivers = "Drivers";
            public const string FinancialPeriods = "Financial_Periods";
            public const string Units = "Units";
            public const string Items = "Items";
            public const string Item_Balances = "Item_Balances";
            public const string Agency_Settings = "Agency_Settings";
            public const string Accounts = "Accounts";
            public const string Shipment_Receipt_Headers = "Shipment_Receipt_Headers";
            public const string Shipment_Receipt_Details = "Shipment_Receipt_Details";
        }

        public static class Columns

        {
            public const string Is_Farmer = "Is_Farmer";
            // --- الثوابت العامة المشتركة ---
            public const string Notes = "Notes";
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
            // حقول ادارة نظام الوكلاء
            public const string Agency_ID = "Agency_ID";
            public const string Agency_Name = "Agency_Name";
            public const string Default_Farmer_Commission = "Default_Farmer_Commission";
            public const string Default_Buyer_Commission = "Default_Buyer_Commission";
            public const string Default_Handling_Fee = "Default_Handling_Fee";
            public const string Commission_Income_Account_ID = "Commission_Income_Account_ID";
            public const string Handling_Payable_Account_ID = "Handling_Payable_Account_ID";
            public const string Acc_ID = "Acc_ID";
            public const string Acc_Name = "Acc_Name";

            public static bool Is_Stopped { get; internal set; }
            //     حقول بيانات الموردين والمزارعين
            public const string Supp_ID = "Supp_ID";
            public const string Supp_Name = "Supp_Name";
            public const string Supp_Phone = "Supp_Phone";
            public const string Supp_Address = "Supp_Address";

            // --- ثوابت إعدادات الوكالة ---
            public const string Agency_Phone1 = "Agency_Phone1";
            public const string Agency_Phone2 = "Agency_Phone2";
            public const string Agency_Address = "Agency_Address";
            public const string Agency_Notes = "Agency_Notes";
            public const string Eng_Agency_Name = "Eng_Agency_Name";
            public const string Eng_Agency_Notes = "Eng_Agency_Notes";
            public const string Eng_Agency_Address = "Eng_Agency_Address";
            // السياسات والنسب
            public const string Farmer_Commission_Percent = "Farmer_Commission_Percent";
            public const string Buyer_Fee_Per_Package = "Buyer_Fee_Per_Package";
            public const string Office_Service_Fee = "Office_Service_Fee";
            public const string Allow_Override_In_Invoice = "Allow_Override_In_Invoice";

            // الربط المحاسبي
            public const string Acc_Farmer_Commission = "Acc_Farmer_Commission";
            public const string Acc_Buyer_Fee = "Acc_Buyer_Fee";
            public const string Acc_Additional_Discount = "Acc_Additional_Discount";
            // حقول السائقين
            public const string Driver_ID = "Driver_ID";
            public const string License_Number = "License_Number";
            public const string Vehicle_Type = "Vehicle_Type";
            public const string Phone_Number = "Phone_Number";
            // 1. حقول رأس الحمولة (Shipment Headers)
            public const string Shipment_ID = "Shipment_ID";
            public const string Shipment_Code = "Shipment_Code";
            public const string Receipt_Date = "Receipt_Date";
            public const string Driver_Name = "Driver_Name";
            public const string Vehicle_Number = "Vehicle_Number";
            public const string Driver_Phone = "Driver_Phone";
            public const string Status_Code = "Status_Code";
            public const string Total_Estimated_Value = "Total_Estimated_Value";
            public const string Total_Actual_Value = "Total_Actual_Value";

            // 2. حقول تفاصيل الحمولة (Shipment Details)
            public const string Detail_ID = "Detail_ID";
            public const string Farmer_ID = "Farmer_ID"; // مرتبط بجدول الموردين (Is_Farmer = 1)
            public const string Estimated_Price = "Estimated_Price";
            public const string Estimated_Total = "Estimated_Total";
            public const string Actual_Price = "Actual_Price";
            public const string Actual_Total = "Actual_Total";
            public const string Discount_Numeric = "Discount_Numeric";
            public const string Extra_Expense = "Extra_Expense";
            public const string Net_Amount = "Net_Amount";
            public const string Is_Invoiced_To_Farmer = "Is_Invoiced_To_Farmer";
            public const string Estimated_Discount = "Estimated_Discount";
           
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