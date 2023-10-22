using AccountService.Data.Interfaces;
using AccountService.Models;
using AccountService.Models.InputModels;
using AccountService.Models.OutputModels;
using AccountService.Models.ResponseResults;
using Microsoft.EntityFrameworkCore;


namespace AccountService.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountServiceDbContext _context;
        private readonly HttpClient _httpClient;
        private static readonly string TransactionServiceUrl = "https://localhost:7203/api/Transactions/add";
        private static readonly string TransactionServiceBaseUrl = "https://localhost:7203/api/Transactions";


        public AccountRepository(AccountServiceDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<ResponseResult<AccountDetailsModel>> CreateAccount(AccountCreationModel model)
        {
            var customer = await _context.Customers.FindAsync(model.CustomerId);
            if (customer == null)
            {
                return new ResponseResult<AccountDetailsModel>
                {
                    IsSuccess = false,
                    Message = "Customer not found.",
                    Result = null
                };
            }

            var account = new Account { 
                CustomerId = model.CustomerId, 
                Balance = model.InitialCredit 
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            if (model.InitialCredit != 0)
            {
                var transactionRequest = new
                {
                    AccountId = account.AccountId,
                    Amount = model.InitialCredit
                };

                var response = await _httpClient.PostAsJsonAsync(TransactionServiceUrl, transactionRequest);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle the error. Maybe log the error for further investigation.
                }
            }

            return new ResponseResult<AccountDetailsModel>
            {
                IsSuccess = true,
                Message = "Account created successfully.",
                Result = new AccountDetailsModel
                {
                    Id = account.AccountId,
                    CustomerId = account.CustomerId,
                    Balance = account.Balance
                }
            };
        }


        public async Task<ResponseResult<CustomerDetailsWithAccountsModel>> GetCustomerDetailsWithAccountsAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return new ResponseResult<CustomerDetailsWithAccountsModel>
                {
                    IsSuccess = false,
                    Message = "Customer not found.",
                    Result = null
                };
            }

            var result = new CustomerDetailsWithAccountsModel
            {
                Id = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Accounts = new List<AccountDetailsWithTransactionsModel>()
            };

            decimal totalBalance = 0;
            var accounts = await _context.Accounts.Where(a => a.CustomerId == customerId).ToListAsync();

            foreach (var account in accounts)
            {
                totalBalance += account.Balance;

                var transactionsResponse = await _httpClient.GetAsync($"{TransactionServiceBaseUrl}/account/{account.AccountId}");
                if (!transactionsResponse.IsSuccessStatusCode)
                {
                    // You can choose to handle the error more robustly here, maybe logging the error.
                    continue;  // Skip this account if there's an issue fetching transactions.
                }


                var transactionWrapper = await transactionsResponse.Content.ReadAsAsync<ResponseResult<List<TransactionModel>>>();
                var transactions = transactionWrapper.Result;


                var accountDetail = new AccountDetailsWithTransactionsModel
                {
                    Id = account.AccountId,
                    CustomerId = account.CustomerId,
                    Balance = account.Balance,
                    Transactions = transactions
                };

                result.Accounts.Add(accountDetail);
            }

            result.TotalBalance = totalBalance;

            return new ResponseResult<CustomerDetailsWithAccountsModel>
            {
                IsSuccess = true,
                Message = "Customer details retrieved successfully.",
                Result = result
            };
        }


    }

}
