namespace AccountService.Models.OutputModels
{
    public class CustomerDetailsModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static implicit operator CustomerDetailsModel(Customer v)
        {
            throw new NotImplementedException();
        }
    }
}
