using AccountService.Data.Interfaces;
using AccountService.Models;
using AccountService.Models.InputModels;
using AccountService.Models.OutputModels;
using AccountService.Models.ResponseResults;


namespace AccountService.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountServiceDbContext _context;
        private readonly HttpClient _httpClient;
        private static readonly string TransactionServiceUrl = "https://localhost:7203/api/Transactions/add";

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

            var account = new Account { CustomerId = model.CustomerId, Balance = model.InitialCredit };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            if (model.InitialCredit != 0)
            {
                var transactionRequest = new
                {
                    AccountId = account.CustomerId,
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
                    Id = account.CustomerId,
                    CustomerId = account.CustomerId,
                    Balance = account.Balance
                }
            };
        }
    }

}
