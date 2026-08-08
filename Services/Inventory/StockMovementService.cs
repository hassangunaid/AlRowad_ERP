using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AlRowad_ERP.Data;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Services.Inventory
{
    public class StockMovementService
    {
        private readonly ItemRepository _itemRepo;

        public StockMovementService()
        {
            _itemRepo = new ItemRepository();
        }

        public async Task UpdateStockQuantityAsync(int itemId, int storeId, decimal quantity, string userId, SqlTransaction transaction)
        {
            // الدستور: حماية البيانات وسلامة الرصيد
            if (quantity < 0) // حالة الصرف أو البيع
            {
                decimal currentBalance = await _itemRepo.GetItemBalanceAsync(itemId, storeId, transaction);
                if (currentBalance < Math.Abs(quantity))
                {
                    throw new InvalidOperationException(SystemConstants.Messages.InsufficientStock);
                }
            }

            // الدستور: استخدام الحقول الرقابية من الثوابت
            await _itemRepo.UpdateQuantityAsync(itemId, storeId, quantity, userId, SystemConstants.AuditFields.CreatedBy, transaction);
        }
    }
}