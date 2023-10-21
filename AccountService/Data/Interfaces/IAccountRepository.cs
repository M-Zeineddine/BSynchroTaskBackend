using AccountService.Models;
using AccountService.Models.InputModels;
using AccountService.Models.OutputModels;
using AccountService.Models.ResponseResults;

namespace AccountService.Data.Interfaces
{
    public interface IAccountRepository
    {
        Task<ResponseResult<AccountDetailsModel>> CreateAccount(AccountCreationModel model);
    }


}
