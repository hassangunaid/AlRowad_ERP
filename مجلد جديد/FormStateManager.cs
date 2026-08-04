// ملف: Core/FormController.cs
using System.Linq;
using System.Windows.Forms;

public static class FormController
{
    // دالة تفعيل الأزرار حسب حالة النظام
    // الوضع: true = حالة إدخال/تعديل (جديد/إضافة/تعديل)
    // الوضع: false = حالة استعراض/قراءة (Browse Mode)
    public static void SetButtonsState(Form frm, bool isEditing)
    {
        // استخدام الانعكاس (Reflection) أو الوصول المباشر للأزرار التي عرفتها
        // سنفترض أن الشاشة تحتوي على هذه الأزرار كـ public

        // الأزرار التي تعمل فقط في وضع العرض
        var browseButtons = new[] { "btn_New", "btn_Edit", "btn_Search", "btn_Print", "btn_Close" };

        // الأزرار التي تعمل فقط في وضع الإدخال/التعديل
        var editButtons = new[] { "btn_Save", "btn_Cancel", "btn_AddFrom" };

        foreach (var name in browseButtons)
        {
            var btn = frm.Controls.Find(name, true).FirstOrDefault() as Button;
            if (btn != null) btn.Enabled = !isEditing;
        }

        foreach (var name in editButtons)
        {
            var btn = frm.Controls.Find(name, true).FirstOrDefault() as Button;
            if (btn != null) btn.Enabled = isEditing;
        }
    }
}