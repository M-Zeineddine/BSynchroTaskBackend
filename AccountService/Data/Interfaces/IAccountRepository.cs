using AccountService.Models;
using AccountService.Models.OutputModels;
using AccountService.Models.ResponseResults;

namespace AccountService.Data.Interfaces
{
    public interface IAccountRepository
    {
        Task<ResponseResult<AccountDetailsModel>> CreateAccount(int customerId, decimal initialCredit);
    }


}
