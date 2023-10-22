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
        private readonly HttpClient _httpClient;
        public TransactionRepository(TransactionServiceDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<ResponseResult<Transaction>> AddTransaction(TransactionCreationModel model)
        {
            //Check if account entered exists
            string checkAccountUrl = $"https://localhost:7157/api/Accounts/{model.AccountId}";

            HttpResponseMessage response = await _httpClient.GetAsync(checkAccountUrl);

            if (!response.IsSuccessStatusCode)
            {
                return new ResponseResult<Transaction>
                {
                    IsSuccess = false,
                    Message = "The provided accountId does not exist.",
                    Result = null
                };
            }

            //Rest of code to add transaction
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
