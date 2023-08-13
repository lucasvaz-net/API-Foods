namespace API.Models
{
    public class UserGoals
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double? DailyCalories { get; set; }
        public double? ProteinGoal { get; set; }
        public double? CarbsGoal { get; set; }
        public double? FatGoal { get; set; }
    }

}
