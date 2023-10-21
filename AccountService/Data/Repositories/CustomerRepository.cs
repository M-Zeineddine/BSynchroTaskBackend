using AccountService.Data.Interfaces;
using AccountService.Models;
using AccountService.Models.ResponseResults;
using AccountService.Models.OutputModels;

namespace AccountService.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AccountServiceDbContext _context;

        public CustomerRepository(AccountServiceDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseResult<CustomerDetailsModel>> GetCustomerDetails(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return new ResponseResult<CustomerDetailsModel>
                {
                    IsSuccess = false,
                    Message = "Customer not found.",
                    Result = null
                };
            }

            var customerDetails = new CustomerDetailsModel
            {
                Id = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };

            return new ResponseResult<CustomerDetailsModel>
            {
                IsSuccess = true,
                Message = "Customer retrieved successfully.",
                Result = customerDetails
            };
        }

    }
}
