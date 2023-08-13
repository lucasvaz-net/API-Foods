using System.Collections.Generic;
using System.Data.SqlClient;
using API.Models;

namespace API.Data
{
    public class FoodDal
    {
        private readonly DatabaseConnection _databaseConnection;

        public FoodDal(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public List<Foods> GetAllFoods()
        {
            var foods = new List<Foods>();

            using (SqlConnection connection = _databaseConnection.CreateConnection())
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM vw_AllFoods", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var food = new Foods
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("FoodId")),
                                Name = reader.GetString(reader.GetOrdinal("FoodName")),
                                Category = new Category
                                {
                                    Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                                },
                                Nutrients = new Nutrients
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")), 
                                    FoodId = reader.GetInt32(reader.GetOrdinal("FoodId")),
                                    Moisture = reader.IsDBNull(reader.GetOrdinal("Moisture")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Moisture")),
                                    Kcal = reader.IsDBNull(reader.GetOrdinal("Kcal")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Kcal")),
                                    KJ = reader.IsDBNull(reader.GetOrdinal("KJ")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("KJ")),
                                    Protein = reader.IsDBNull(reader.GetOrdinal("Protein")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Protein")),
                                    Lipids = reader.IsDBNull(reader.GetOrdinal("Lipids")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Lipids")),
                                    Cholesterol = reader.IsDBNull(reader.GetOrdinal("Cholesterol")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Cholesterol")),
                                    Carbohydrates = reader.IsDBNull(reader.GetOrdinal("Carbohydrates")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Carbohydrates")),
                                    DietaryFiber = reader.IsDBNull(reader.GetOrdinal("DietaryFiber")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("DietaryFiber")),
                                    Ash = reader.IsDBNull(reader.GetOrdinal("Ash")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Ash")),
                                    Calcium = reader.IsDBNull(reader.GetOrdinal("Calcium")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Calcium")),
                                    Magnesium = reader.IsDBNull(reader.GetOrdinal("Magnesium")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Magnesium")),
                                    Manganese = reader.IsDBNull(reader.GetOrdinal("Manganese")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Manganese")),
                                    Phosphorus = reader.IsDBNull(reader.GetOrdinal("Phosphorus")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Phosphorus")),
                                    Iron = reader.IsDBNull(reader.GetOrdinal("Iron")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Iron")),
                                    Sodium = reader.IsDBNull(reader.GetOrdinal("Sodium")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Sodium")),
                                    Potassium = reader.IsDBNull(reader.GetOrdinal("Potassium")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Potassium")),
                                    Copper = reader.IsDBNull(reader.GetOrdinal("Copper")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Copper")),
                                    Zinc = reader.IsDBNull(reader.GetOrdinal("Zinc")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Zinc")),
                                    Retinol = reader.IsDBNull(reader.GetOrdinal("Retinol")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Retinol")),
                                    Re = reader.IsDBNull(reader.GetOrdinal("Re")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Re")),
                                    Rae = reader.IsDBNull(reader.GetOrdinal("Rae")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Rae")),
                                    Thiamin = reader.IsDBNull(reader.GetOrdinal("Thiamin")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Thiamin")),
                                    Riboflavin = reader.IsDBNull(reader.GetOrdinal("Riboflavin")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Riboflavin")),
                                    Pyridoxine = reader.IsDBNull(reader.GetOrdinal("Pyridoxine")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Pyridoxine")),
                                    Niacin = reader.IsDBNull(reader.GetOrdinal("Niacin")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Niacin")),
                                    VitaminC = reader.IsDBNull(reader.GetOrdinal("VitaminC")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("VitaminC"))
                                }
                            };
                            foods.Add(food);
                        }
                    }
                }
            }
            return foods;
        }

        public Foods GetFoodById(int foodId)
        {
            Foods food = null;

            using (SqlConnection connection = _databaseConnection.CreateConnection())
            {
             
                using (SqlCommand command = new SqlCommand("SELECT * FROM vw_AllFoods WHERE FoodId = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", foodId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            food = new Foods
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("FoodId")),
                                Name = reader.GetString(reader.GetOrdinal("FoodName")),
                                Category = new Category
                                {
                                    Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                                },
                                Nutrients = new Nutrients
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")), 
                                    FoodId = reader.GetInt32(reader.GetOrdinal("FoodId")),
                                    Moisture = reader.IsDBNull(reader.GetOrdinal("Moisture")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Moisture")),
                                    Kcal = reader.IsDBNull(reader.GetOrdinal("Kcal")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Kcal")),
                                    KJ = reader.IsDBNull(reader.GetOrdinal("KJ")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("KJ")),
                                    Protein = reader.IsDBNull(reader.GetOrdinal("Protein")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Protein")),
                                    Lipids = reader.IsDBNull(reader.GetOrdinal("Lipids")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Lipids")),
                                    Cholesterol = reader.IsDBNull(reader.GetOrdinal("Cholesterol")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Cholesterol")),
                                    Carbohydrates = reader.IsDBNull(reader.GetOrdinal("Carbohydrates")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Carbohydrates")),
                                    DietaryFiber = reader.IsDBNull(reader.GetOrdinal("DietaryFiber")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("DietaryFiber")),
                                    Ash = reader.IsDBNull(reader.GetOrdinal("Ash")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Ash")),
                                    Calcium = reader.IsDBNull(reader.GetOrdinal("Calcium")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Calcium")),
                                    Magnesium = reader.IsDBNull(reader.GetOrdinal("Magnesium")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Magnesium")),
                                    Manganese = reader.IsDBNull(reader.GetOrdinal("Manganese")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Manganese")),
                                    Phosphorus = reader.IsDBNull(reader.GetOrdinal("Phosphorus")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Phosphorus")),
                                    Iron = reader.IsDBNull(reader.GetOrdinal("Iron")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Iron")),
                                    Sodium = reader.IsDBNull(reader.GetOrdinal("Sodium")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Sodium")),
                                    Potassium = reader.IsDBNull(reader.GetOrdinal("Potassium")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Potassium")),
                                    Copper = reader.IsDBNull(reader.GetOrdinal("Copper")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Copper")),
                                    Zinc = reader.IsDBNull(reader.GetOrdinal("Zinc")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Zinc")),
                                    Retinol = reader.IsDBNull(reader.GetOrdinal("Retinol")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Retinol")),
                                    Re = reader.IsDBNull(reader.GetOrdinal("Re")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Re")),
                                    Rae = reader.IsDBNull(reader.GetOrdinal("Rae")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Rae")),
                                    Thiamin = reader.IsDBNull(reader.GetOrdinal("Thiamin")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Thiamin")),
                                    Riboflavin = reader.IsDBNull(reader.GetOrdinal("Riboflavin")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Riboflavin")),
                                    Pyridoxine = reader.IsDBNull(reader.GetOrdinal("Pyridoxine")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Pyridoxine")),
                                    Niacin = reader.IsDBNull(reader.GetOrdinal("Niacin")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Niacin")),
                                    VitaminC = reader.IsDBNull(reader.GetOrdinal("VitaminC")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("VitaminC"))
                                }
                            };
                        }
                    }
                }
            }

            return food;
        }
    }
}
