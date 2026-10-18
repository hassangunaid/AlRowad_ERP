using AlRowad_ERP.Core;
using AlRowad_ERP.Data;
using AlRowad_ERP.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AlRowad_ERP.Repositories
{
    public class CompanyRepository
    {
        /// <summary>
        /// جلب بيانات الشركة الأساسية (غالباً سجل واحد فقط)
        /// </summary>
        public async Task<CompanyInfoDto> GetCompanyInfoAsync()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                string query = "SELECT TOP 1 * FROM Company_Info";
                return await connection.QueryFirstOrDefaultAsync<CompanyInfoDto>(query);
            }
        }

        /// <summary>
        /// حفظ أو تحديث بيانات الشركة (إدراج أو تعديل) مع معاملات آمنة
        /// </summary>
        public async Task<bool> SaveCompanyInfoAsync(CompanyInfoDto company)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // التحقق مما إذا كانت السجلات موجودة مسبقاً
                        string checkQuery = "SELECT COUNT(1) FROM Company_Info";
                        int count = await connection.ExecuteScalarAsync<int>(checkQuery, transaction: transaction);

                        string query = "";
                        if (count > 0)
                        {
                            // تحديث السجل الموجود
                            query = @"UPDATE Company_Info SET 
                                        Company_Name_Ar = @Company_Name_Ar, 
                                        Company_Name_En = @Company_Name_En, 
                                        Tax_Number = @Tax_Number, 
                                        Commercial_Record = @Commercial_Record, 
                                        Address = @Address, 
                                        Phone = @Phone, 
                                        Email = @Email, 
                                        Logo = @Logo";
                        }
                        else
                        {
                            // إدراج سجل جديد
                            query = @"INSERT INTO Company_Info 
                                        (Company_Name_Ar, Company_Name_En, Tax_Number, Commercial_Record, Address, Phone, Email, Logo) 
                                      VALUES 
                                        (@Company_Name_Ar, @Company_Name_En, @Tax_Number, @Commercial_Record, @Address, @Phone, @Email, @Logo)";
                        }

                        await connection.ExecuteAsync(query, company, transaction: transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}