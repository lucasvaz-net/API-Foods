namespace API.Models
{
    public class Nutrients
    {
        public int Id { get; set; }
        public int FoodId { get; set; }  // Foreign Key
        public float? Moisture { get; set; }
        public float? Kcal { get; set; }
        public float? KJ { get; set; }
        public float? Protein { get; set; }
        public float? Lipids { get; set; }
        public float? Cholesterol { get; set; }
        public float? Carbohydrates { get; set; }
        public float? DietaryFiber { get; set; }
        public float? Ash { get; set; }
        public float? Calcium { get; set; }
        public float? Magnesium { get; set; }
        public float? Manganese { get; set; }
        public float? Phosphorus { get; set; }
        public float? Iron { get; set; }
        public float? Sodium { get; set; }
        public float? Potassium { get; set; }
        public float? Copper { get; set; }
        public float? Zinc { get; set; }
        public float? Retinol { get; set; }
        public float? Re { get; set; }
        public float? Rae { get; set; }
        public float? Thiamin { get; set; }
        public float? Riboflavin { get; set; }
        public float? Pyridoxine { get; set; }
        public float? Niacin { get; set; }
        public float? VitaminC { get; set; }
    }

}
