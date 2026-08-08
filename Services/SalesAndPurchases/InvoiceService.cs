using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Helpers;
using AlRowad_ERP.Data;
using AlRowad_ERP.Models;
using AlRowad_ERP.Services.Accounting;
using AlRowad_ERP.Services.Inventory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AlRowad_ERP.Services.SalesAndPurchases
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _invoiceRepo;
        private readonly FinancialPeriodService _periodService;
        private readonly StockMovementService _stockService;
        private readonly JournalService _journalService;

        public InvoiceService()
        {
            _invoiceRepo = new InvoiceRepository();
            _periodService = new FinancialPeriodService();
            _stockService = new StockMovementService();
            _journalService = new JournalService();
        }

        public async Task<bool> SaveInvoiceAsync(InvoiceModel invoice, List<InvoiceDetailModel> details, string currentUserId)
        {
            // 1. الدستور: فحص السياسات قبل فتح الاتصال بقاعدة البيانات
            bool isPeriodOpen = await _periodService.IsPeriodOpenAsync(invoice.InvoiceDate);
            if (!isPeriodOpen)
            {
                throw new InvalidOperationException(SystemConstants.Messages.ClosedFinancialPeriod);
            }

            // 2. الدستور: بناء كتلة (ACID Transaction)
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                await connection.OpenAsync();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // أ. حفظ رأس الفاتورة وتفاصيلها عبر الـ Repository
                        int invoiceId = await _invoiceRepo.InsertInvoiceAsync(invoice, currentUserId, transaction);

                        foreach (var item in details)
                        {
                            item.InvoiceId = invoiceId;
                            await _invoiceRepo.InsertInvoiceDetailAsync(item, transaction);

                            // ب. التأثير المخزني اللحظي (خصم الكميات المباعة)
                            // نمرر الكمية بالسالب لأنها عملية صرف/بيع
                            await _stockService.UpdateStockQuantityAsync(item.ItemId, invoice.StoreId, -item.Quantity, currentUserId, transaction);
                        }

                        // ج. إنشاء القيد المحاسبي للفاتورة تلقائياً
                        var journalHeader = new JournalHeaderModel
                        {
                            JournalDate = invoice.InvoiceDate,
                            Description = $"{SystemConstants.Descriptions.SalesInvoice} رقم {invoiceId}",
                            CreatedBy = currentUserId
                        };

                        var journalDetails = new List<JournalDetailModel>
                        {
                            new JournalDetailModel { AccountId = invoice.CustomerId, DebitAmount = invoice.NetTotal, CreditAmount = 0 }, // مدين: العميل
                            new JournalDetailModel { AccountId = SystemConstants.Accounts.SalesRevenue, DebitAmount = 0, CreditAmount = invoice.NetTotal } // دائن: المبيعات
                        };

                        await _journalService.PostJournalEntryAsync(journalHeader, journalDetails, transaction);

                        // د. تأكيد الكتلة بالكامل (Commit)
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        // هـ. التراجع التام في حال حدوث أي خطأ في أي خطوة (Rollback)
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}