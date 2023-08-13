namespace API.Models
{
    public class Foods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public Nutrients Nutrients { get; set; }
    }
}
