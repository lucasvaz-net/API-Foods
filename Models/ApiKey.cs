namespace API.Models
{
    public class ApiKey
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string ApiKeyValue { get; set; }  
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; } 
    }
}
