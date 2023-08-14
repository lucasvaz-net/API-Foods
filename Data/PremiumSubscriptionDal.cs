namespace API.Data
{
    using API.Models;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class PremiumSubscriptionDal
    {
        private readonly string _connectionString;

        public PremiumSubscriptionDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PremiumSubscription GetUserSubscriptionStatus(int userId)
        {
            PremiumSubscription subscription = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dbo.sp_GetUserSubscriptionStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            subscription = new PremiumSubscription
                            {
                                UserId = userId,
                                StartDate = reader.GetDateTime(reader.GetOrdinal("startDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("endDate")),
                                PaymentDetails = reader.GetString(reader.GetOrdinal("paymentDetails"))
                            };
                            string status = reader.GetString(reader.GetOrdinal("SubscriptionStatus"));
                            if (status == "Expired")
                            {
                                subscription.EndDate = null;
                            }
                        }
                    }
                }
            }

            return subscription;
        }

        public void PurchasePremiumSubscription(PremiumSubscription subscription)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_PurchasePremiumSubscription", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", subscription.UserId);
                    command.Parameters.AddWithValue("@StartDate", subscription.StartDate);
                    command.Parameters.AddWithValue("@EndDate", subscription.EndDate);
                    command.Parameters.AddWithValue("@PaymentDetails", subscription.PaymentDetails);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
