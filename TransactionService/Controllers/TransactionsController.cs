using Microsoft.AspNetCore.Mvc;
using TransactionService.Data.Interfaces;
using TransactionService.Models;
using TransactionService.Models.InputModels;
using TransactionService.Models.ResponseResults;

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
        public async Task<ResponseResult<Transaction>> AddTransaction(TransactionCreationModel model)
        {
            return await _transactionRepository.AddTransaction(model);
        }



        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetTransactionsByAccountId(int accountId)
        {
            var transactions = await _transactionRepository.GetTransactionsByAccountId(accountId);

            if(transactions.Count == 0)
            {
                return NotFound(new
                {
                    IsSuccess = false,
                    Message = "No transactions found for the provided account ID.",
                    Result = transactions
                });
            }

            return Ok(new
            {
                IsSuccess = true,
                Message = "Transactions retrieved successfully.",
                Result = transactions
            });
        }
    }

}
