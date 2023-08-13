namespace API.Models
{
    public class FoodDiary
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FoodId { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public double ServingSize { get; set; }
    }
}
