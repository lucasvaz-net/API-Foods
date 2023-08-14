using API.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace API.Data
{
    public class FoodDiaryDal
    {
        private readonly string _connectionString;

        public FoodDiaryDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<FoodDiaryEntry> GetUserFoodDiary(int userId)
        {
            var entries = new List<FoodDiaryEntry>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM vw_UserFoodDiary WHERE UserId = @UserId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entry = new FoodDiaryEntry
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                EntryDate = reader.GetDateTime(reader.GetOrdinal("EntryDate")),
                                Food = new Foods
                                {
                                    Name = reader.GetString(reader.GetOrdinal("FoodName")),
                                    Nutrients = new Nutrients
                                    {
                                        Moisture = reader.IsDBNull(reader.GetOrdinal("Moisture")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Moisture")),
                                        Kcal = reader.IsDBNull(reader.GetOrdinal("Calories")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Calories")),
                                        KJ = reader.IsDBNull(reader.GetOrdinal("Energykj")) ? null : (float?)reader.GetFloat(reader.GetOrdinal("Energykj")),
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
                                }
                            };
                            entries.Add(entry);
                        }
                    }
                }
            }
            return entries;
        }

        public bool AddFoodToDiary(int userId, int foodId, float servingSize, DateTime? entryDate = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_AddFoodToDiary", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@FoodID", foodId);
                        command.Parameters.AddWithValue("@ServingSize", servingSize);
                        if (entryDate.HasValue)
                            command.Parameters.AddWithValue("@EntryDate", entryDate.Value);
                        else
                            command.Parameters.AddWithValue("@EntryDate", DBNull.Value);

                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();


                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveFoodFromDiary(int diaryEntryId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_RemoveFoodFromDiary", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DiaryEntryID", diaryEntryId);

                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();


                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }



    }
}
