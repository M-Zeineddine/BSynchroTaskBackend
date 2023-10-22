namespace AccountService.Models.OutputModels
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
