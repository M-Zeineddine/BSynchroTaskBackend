using AccountService.Data.Interfaces;
using AccountService.Models;
using AccountService.Models.OutputModels;
using AccountService.Models.ResponseResults;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }



        [HttpGet("{customerId}")]
        public async Task<ResponseResult<CustomerDetailsModel>> GetCustomer(int customerId)
        {
            return await _customerRepository.GetCustomerDetails(customerId);
        }



    }
}
