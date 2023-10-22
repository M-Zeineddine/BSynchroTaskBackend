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


        [HttpGet("{customerId}/details")]
        public async Task<IActionResult> GetCustomerDetailsWithAccounts(int customerId)
        {
            var customerDetailsResponse = await _accountRepository.GetCustomerDetailsWithAccountsAsync(customerId);

            if (customerDetailsResponse.Result == null)
            {
                return NotFound(new ResponseResult<CustomerDetailsWithAccountsModel>
                {
                    IsSuccess = false,
                    Message = "Customer not found.",
                    Result = null
                });
            }

            return Ok(customerDetailsResponse);
        }



    }

}
