using AccountService.Data.Interfaces;
using AccountService.Models;
using AccountService.Models.OutputModels;
using AccountService.Models.ResponseResults;


namespace AccountService.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountServiceDbContext _context;
        

        public AccountRepository(AccountServiceDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseResult<AccountDetailsModel>> CreateAccount(int customerId, decimal initialCredit)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return new ResponseResult<AccountDetailsModel>
                {
                    IsSuccess = false,
                    Message = "Customer not found.",
                    Result = null
                };
            }

            var account = new Account { CustomerId = customerId, Balance = initialCredit };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            /*if (initialCredit != 0)
            {
                
            }*/

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
