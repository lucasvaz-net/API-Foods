namespace API.Data
{
    using System.Data;
    using System.Data.SqlClient;

    namespace API.Data
    {
        public class ApiKeyDal
        {
            private readonly string _connectionString;

            public ApiKeyDal(string connectionString)
            {
                _connectionString = connectionString;
            }

            public bool IsApiKeyValid(string apiKey)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM vw_ValidApiKeys WHERE ApiKey = @ApiKey";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApiKey", apiKey);

                        int count = (int)command.ExecuteScalar();

                        return count > 0;  // Retorna true se a chave API existir, caso contrário, retorna false.
                    }
                }
            }

        }
    }

}
