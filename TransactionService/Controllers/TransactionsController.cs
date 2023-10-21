using Microsoft.AspNetCore.Mvc;
using TransactionService.Data.Interfaces;

namespace TransactionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTransaction(int accountId, decimal amount)
        {
            var result = await _transactionRepository.AddTransaction(accountId, amount);
            if (result.IsSuccess == true)
                return Ok(result);

            return BadRequest(result);
        }
    }

}
