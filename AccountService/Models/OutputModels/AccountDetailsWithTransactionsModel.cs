namespace AccountService.Models.OutputModels
{
    public class AccountDetailsWithTransactionsModel : AccountDetailsModel
    {
        public List<TransactionModel> Transactions { get; set; }
    }
}
