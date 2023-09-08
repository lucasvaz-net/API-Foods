using API.Models;
using System.Data;
using System.Data.SqlClient;

namespace API.Data
{
    public class UserDal
    {
        private readonly string _connectionString;

        public UserDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int RegisterUser(Users user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_RegisterUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@hashedPassword", user.HashedPassword);
                    command.Parameters.AddWithValue("@dateOfBirth", user.DateOfBirth);
                    command.Parameters.AddWithValue("@weight", user.Weight);
                    command.Parameters.AddWithValue("@goalWeight", user.GoalWeight);
                    command.Parameters.AddWithValue("@activityLevel", user.ActivityLevel);


                    SqlParameter userIdOut = new SqlParameter
                    {
                        ParameterName = "@userIdOut",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(userIdOut);

                    connection.Open();
                    command.ExecuteNonQuery();

                    return (int)userIdOut.Value;
                }
            }
        }

        public string GetHashedPasswordByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dbo.sp_GetUserByEmail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(reader.GetOrdinal("HashedPassword"));
                        }
                    }
                }
            }

            return null;  
        }



        public int? CheckUserCredentials(string email, string hashedPassword)
        {
            int? userId = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("dbo.sp_CheckUserCredentials", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@HashedPassword", hashedPassword);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(reader.GetOrdinal("id"));
                        }
                    }
                }
            }

            return userId;
        }


        public Users GetUserProfile(int userId)
        {
            Users userProfile = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM vw_UserProfile WHERE UserId = @UserId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userProfile = new Users
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Username = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                Weight = reader.IsDBNull(reader.GetOrdinal("CurrentWeight"))
            ? (float?)null
            : reader.GetFloat(reader.GetOrdinal("CurrentWeight")),
                                GoalWeight = reader.IsDBNull(reader.GetOrdinal("GoalWeight"))
                ? (float?)null
                : reader.GetFloat(reader.GetOrdinal("GoalWeight")),
                                ActivityLevel = reader.GetString(reader.GetOrdinal("ActivityLevel")),
                                DateJoined = reader.GetDateTime(reader.GetOrdinal("DateJoined")),
                                LastLogin = reader.IsDBNull(reader.GetOrdinal("LastLogin"))
                                            ? (DateTime?)null
                                            : reader.GetDateTime(reader.GetOrdinal("LastLogin"))
                            };
                        }
                    }
                }
            }

            return userProfile;
        }

        public bool UpdateUserProfile(Users user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UpdateUserProfile", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@userId", user.Id);
                    command.Parameters.AddWithValue("@username", user.Username ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@email", user.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@hashedPassword", user.HashedPassword ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@dateOfBirth", user.DateOfBirth ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@weight", user.Weight.HasValue ? (object)user.Weight.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@goalWeight", user.GoalWeight.HasValue ? (object)user.GoalWeight.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@activityLevel", user.ActivityLevel ?? (object)DBNull.Value);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

        public int? GetUserIdByToken(string token)
        {
            int? userId = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dbo.sp_GetUserByToken", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@token", token);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(reader.GetOrdinal("id"));
                        }
                    }
                }
            }

            return userId;
        }



    }
}
