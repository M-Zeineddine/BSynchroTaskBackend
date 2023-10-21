using TransactionService.Models;
using TransactionService.Models.ResponseResults;

namespace TransactionService.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Task<ResponseResult<Transaction>> AddTransaction(int accountId, decimal amount);
    }

}
