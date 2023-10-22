using AccountService.Data.Interfaces;
using AccountService.Data.Repositories;
using AccountService.Models;
using AccountService.Models.InputModels;
using AccountService.Models.OutputModels;
using AccountService.Models.ResponseResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AccountServiceDbContext _context;

        public AccountsController(IAccountRepository accountRepository, AccountServiceDbContext context)
        {
            _accountRepository = accountRepository;
            _context = context;
        }

        [HttpPost("create")]
        public async Task<ResponseResult<AccountDetailsModel>> CreateAccount(AccountCreationModel model)
        {
            return await _accountRepository.CreateAccount(model);
        }


        [HttpGet("{customerId}/details")]
        public async Task<ResponseResult<CustomerDetailsWithAccountsModel>> GetCustomerDetailsWithAccounts(int customerId)
        {
            return await _accountRepository.GetCustomerDetailsWithAccounts(customerId);

        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(int accountId)
        {
            var account = await _accountRepository.GetAccountById(accountId);
            if (account == null)
            {
                return NotFound(new { Message = "Account not found" });
            }

            var result = new AccountDetailsModel
            {
                Id = account.AccountId,
                CustomerId = account.CustomerId,
                Balance = account.Balance
            };

            return Ok(result);
        }


        [HttpPut("{accountId}/balance")]
        public async Task<IActionResult> UpdateAccountBalance(int accountId, [FromBody] decimal updatedBalance)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);

            if (account == null)
            {
                return NotFound();
            }

            account.Balance = updatedBalance;

            await _context.SaveChangesAsync();

            return Ok();
        }

    }

}
