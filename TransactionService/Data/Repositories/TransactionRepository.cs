using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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


            if (model.Amount == 0)
            {
                return new ResponseResult<Transaction>
                {
                    IsSuccess = false,
                    Message = "Can't make transaction of 0.",
                    Result = null
                };
            }


            // Fetch current account details
            dynamic accountDetails = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            if (accountDetails == null || accountDetails.balance == null)
            {
                return new ResponseResult<Transaction>
                {
                    IsSuccess = false,
                    Message = "Invalid account details received.",
                    Result = null
                };
            }

            decimal currentBalance = accountDetails.balance;
            decimal updatedBalance = currentBalance + model.Amount;

            if (updatedBalance < 0)
            {
                return new ResponseResult<Transaction>
                {
                    IsSuccess = false,
                    Message = "Insufficient funds.",
                    Result = null
                };
            }



            // Update the balance in the Accounts service
            var updateResponse = await _httpClient.PutAsJsonAsync($"https://localhost:7157/api/Accounts/{model.AccountId}/balance", updatedBalance);

            if (!updateResponse.IsSuccessStatusCode)
            {
                return new ResponseResult<Transaction>
                {
                    IsSuccess = false,
                    Message = "Failed to update the account balance.",
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
