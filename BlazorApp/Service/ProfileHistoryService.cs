using BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace BlazorApp.Service
{
    

    public class ProfileHistoryService
    {
        private readonly string connectionString = "Server=WIN22DS-09694AE\\SQLEXPRESS;Database=BrazorPage; Integrated Security=True; TrustServerCertificate=True";

        public async Task<List<ProfileHistoryModel>> GetAllProfileHistoryAsync()
        {
            List<ProfileHistoryModel> profiles = new List<ProfileHistoryModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("GetAllProfileHistory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ProfileHistoryModel profile = new ProfileHistoryModel
                            {
                                Id = reader.GetInt32(0),
                                ProfileName = reader.GetString(1),
                                Timestamp = reader.GetDateTime(2),
                                UserId = reader.GetInt32(3),
                                Action = reader.GetString(4)
                            };

                            profiles.Add(profile);
                        }
                    }
                }
            }

            ProfileHistoryModel mymodel = new ProfileHistoryModel();
            //mymodel.Id = 3;
            mymodel.ProfileName = "Zakria";
            mymodel.UserId = 3;
            mymodel.Action = "Fooling Arround";
            mymodel.Timestamp = System.DateTime.Now;
            InsertProfileHistoryAsync(mymodel);



            return profiles;
        }

        public async Task<int> InsertProfileHistoryAsync(ProfileHistoryModel profile)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("InsertProfileHistory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ProfileName", profile.ProfileName);
                        command.Parameters.AddWithValue("@Timestamp", profile.Timestamp);
                        command.Parameters.AddWithValue("@UserId", profile.UserId);
                        command.Parameters.AddWithValue("@Action", profile.Action);

                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error in InsertProfileHistoryAsync: {ex.Message}");
                throw; // Re-throw the exception to propagate it further if necessary
            }
        }

        public async Task<int> UpdateProfileHistoryAsync(ProfileHistoryModel profile)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("UpdateProfileHistory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", profile.Id);
                        command.Parameters.AddWithValue("@ProfileName", profile.ProfileName);
                        command.Parameters.AddWithValue("@Timestamp", profile.Timestamp);
                        command.Parameters.AddWithValue("@UserId", profile.UserId);
                        command.Parameters.AddWithValue("@Action", profile.Action);

                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateProfileHistoryAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<ProfileHistoryModel> GetProfileByIdAsync(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("GetProfileById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new ProfileHistoryModel
                                {
                                    Id = reader.GetInt32(0),
                                    ProfileName = reader.GetString(1),
                                    Timestamp = reader.GetDateTime(2),
                                    UserId = reader.GetInt32(3),
                                    Action = reader.GetString(4)
                                };
                            }
                        }
                    }
                }

                return null; // Return null if no profile is found with the given Id
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProfileByIdAsync: {ex.Message}");
                throw;
            }
        }


    }


}
