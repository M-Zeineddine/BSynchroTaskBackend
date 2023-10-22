using TransactionService.Models;
using TransactionService.Models.InputModels;
using TransactionService.Models.ResponseResults;

namespace TransactionService.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Task<ResponseResult<Transaction>> AddTransaction(TransactionCreationModel model);
        Task<List<Transaction>> GetTransactionsByAccountId(int accountId);

    }

}
