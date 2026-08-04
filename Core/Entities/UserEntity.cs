namespace AlRowad_ERP.Core.Entities
{
    // كيان المستخدم (UserEntity) المتوافق مع الجدول المرفق
    public class UserEntity
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}