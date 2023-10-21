using AccountService.Models;
using AccountService.Models.OutputModels;
using AccountService.Models.ResponseResults;

namespace AccountService.Data.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<ResponseResult<CustomerDetailsModel>> GetCustomerDetails(int customerId);
    }
}
