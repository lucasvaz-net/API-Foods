namespace API.Models
{
    public class Units
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string UnitValue { get; set; }  // Renomeei para UnitValue para evitar confusão com o nome da classe
        public string LabelPt { get; set; }
        public string InfoodsTagname { get; set; }
        public string SystematicName { get; set; }
        public string CommonName { get; set; }
    }

}
