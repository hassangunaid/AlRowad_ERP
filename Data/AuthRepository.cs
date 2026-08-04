using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
// التأكد من استدعاء مساحة الاسم التي تحتوي على DatabaseHelper

namespace AlRowad_ERP.Data
{
    public static class AuthRepository
    {
        public static async Task<UserEntity> AuthenticateUserAsync(string username, string password)
        {
            // ملاحظة أمنية: في أنظمة الـ Enterprise يجب تشفير المتغير password هنا
            // لمطابقته مع حقل Password_Hash في قاعدة البيانات. 
            // مثال: string hashedPassword = SecurityHelper.ComputeHash(password);

            string query = @"SELECT [User_ID], [Username], [Full_Name], [Is_Active], [Is_SuperAdmin] 
                             FROM [Users] 
                             WHERE [Username] = @Username AND [Password_Hash] = @Password AND [Is_Active] = 1";

            using (var connection = DatabaseHelper.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    // قم بتغيير @Password إلى hashedPassword إذا كنت تستخدم التشفير
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new UserEntity
                            {
                                UserID = Convert.ToInt32(reader["User_ID"]),
                                Username = reader["Username"].ToString(),
                                FullName = reader["Full_Name"].ToString(),
                                IsActive = Convert.ToBoolean(reader["Is_Active"]),
                                IsSuperAdmin = reader["Is_SuperAdmin"] != DBNull.Value && Convert.ToBoolean(reader["Is_SuperAdmin"])
                            };
                        }
                    }
                }
            }
            return null; // إذا لم يجد بيانات مطابقة
        }
        // أضف هذه الدالة في كلاس AuthRepository
        public static async Task<List<UserEntity>> GetAllActiveUsersAsync()
        {
            var users = new List<UserEntity>();
            string query = "SELECT [User_ID], [Username], [Full_Name] FROM [Users] WHERE [Is_Active] = 1";

            using (var connection = DatabaseHelper.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new UserEntity
                            {
                                UserID = Convert.ToInt32(reader["User_ID"]),
                                Username = reader["Username"].ToString(),
                                FullName = reader["Full_Name"].ToString()
                            });
                        }
                    }
                }
            }
            return users;
        }
    }
}