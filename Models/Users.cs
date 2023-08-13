namespace API.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double? Weight { get; set; }
        public double? GoalWeight { get; set; }
        public string ActivityLevel { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime? LastLogin { get; set; }
    }

}
