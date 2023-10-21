using TransactionService.Data.Interfaces;
using TransactionService.Models;
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

        public async Task<ResponseResult<Transaction>> AddTransaction(int accountId, decimal amount)
        {
            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount
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
    }

}
