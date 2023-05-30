namespace LarastruckingApp.Entities.quotesDto
{
    public class CustomerQuotesDto
    {
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsWaitingTimeRequired { get; set; }
        public int ContactInfoCount { get; set; }
    }
}
