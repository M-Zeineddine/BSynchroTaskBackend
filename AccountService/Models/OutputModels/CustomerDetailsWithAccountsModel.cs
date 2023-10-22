namespace AccountService.Models.OutputModels
{
    public class CustomerDetailsWithAccountsModel : CustomerDetailsModel
    {
        public List<AccountDetailsWithTransactionsModel> Accounts { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
