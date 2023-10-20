using AccountService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AccountServiceDbContext _context; // Assuming you named it AccountDbContext

        public CustomersController(AccountServiceDbContext context)
        {
            _context = context;
        }

        // GET: api/customers/{customerId}
        [HttpGet("{customerId}")]
        public ActionResult GetCustomer(int customerId)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer == null)
                return NotFound("Customer not found.");

            return Ok(customer);
        }

        // Other potential endpoints for creating, updating, or deleting customers can be added here
    }
}
