using Microsoft.AspNetCore.Mvc;
using TransactionService.Data.Interfaces;
using TransactionService.Models.InputModels;

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
        public async Task<IActionResult> AddTransaction(TransactionCreationModel model)
        {
            var result = await _transactionRepository.AddTransaction(model);
            if (result.IsSuccess == true)
                return Ok(result);

            return BadRequest(result);
        }
    }

}
