using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AlRowad_ERP.Data;
using AlRowad_ERP.Models;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Services.Accounting
{
    public class JournalService
    {
        private readonly JournalRepository _journalRepo;

        public JournalService()
        {
            _journalRepo = new JournalRepository();
        }

        public async Task PostJournalEntryAsync(JournalHeaderModel header, List<JournalDetailModel> details, SqlTransaction transaction)
        {
            // الدستور: سلامة البيانات المحاسبية (القيد المزدوج)
            decimal totalDebit = details.Sum(d => d.DebitAmount);
            decimal totalCredit = details.Sum(d => d.CreditAmount);

            if (totalDebit != totalCredit)
            {
                throw new InvalidOperationException(SystemConstants.Messages.UnbalancedJournal);
            }

            if (totalDebit == 0)
            {
                throw new InvalidOperationException(SystemConstants.Messages.ZeroAmountJournal);
            }

            // إدراج رأس القيد
            int journalId = await _journalRepo.InsertHeaderAsync(header, transaction);

            // إدراج التفاصيل
            foreach (var detail in details)
            {
                detail.JournalId = journalId;
                await _journalRepo.InsertDetailAsync(detail, transaction);
            }
        }
    }
}