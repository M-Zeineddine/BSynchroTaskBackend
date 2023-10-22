using Microsoft.EntityFrameworkCore;
using TransactionService.Data.Interfaces;
using TransactionService.Models;
using TransactionService.Models.InputModels;
using TransactionService.Models.ResponseResults;

namespace TransactionService.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionServiceDbContext _context;

        public TransactionRepository(TransactionServiceDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseResult<Transaction>> AddTransaction(TransactionCreationModel model)
        {
            var transaction = new Transaction
            {
                AccountId = model.AccountId,
                Amount = model.Amount
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return new ResponseResult<Transaction>
            {
                IsSuccess = true,
                Message = "Transaction added successfully.",
                Result = transaction
            };
        }

        public async Task<List<Transaction>> GetTransactionsByAccountId(int accountId)
        {
            return await _context.Transactions.Where(t => t.AccountId == accountId).ToListAsync();
        }

    }

}
