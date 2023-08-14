namespace API.Models
{
    public class FoodDiaryEntry
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime EntryDate { get; set; }
        public Foods Food { get; set; }
    }

}
