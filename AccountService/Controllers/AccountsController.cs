using AccountService.Data.Interfaces;
using AccountService.Data.Repositories;
using AccountService.Models.InputModels;
using AccountService.Models.OutputModels;
using AccountService.Models.ResponseResults;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("create")]
        public async Task<ResponseResult<AccountDetailsModel>> CreateAccount(AccountCreationModel model)
        {
            return await _accountRepository.CreateAccount(model);
        }

    }

}
