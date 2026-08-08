using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AlRowad_ERP.Data; // مسار المستودعات

namespace AlRowad_ERP.Services.Accounting
{
    public class FinancialPeriodService
    {
        private readonly FinancialPeriodRepository _periodRepo;

        public FinancialPeriodService()
        {
            _periodRepo = new FinancialPeriodRepository();
        }

        // الدستور: السلاسة والأداء العالي (async/await)
        public async Task<bool> IsPeriodOpenAsync(DateTime transactionDate, SqlTransaction transaction = null)
        {
            // استدعاء الدالة الموجودة فعلياً في المستودع لديك والتي تعيد bool
            return await _periodRepo.IsPeriodOpenAsync(transactionDate, transaction);
        }
    }
}