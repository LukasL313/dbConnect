using System;
using Npgsql;
using System.Threading.Tasks;

class DBconnect
{
    static async Task Main(string[] args)
    {
        string connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");

        try
        {
            await using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                Console.WriteLine($"The PostgreSQL version: {connection.PostgreSqlVersion}");

                Console.Write("Enter Username: ");
                string? username = Console.ReadLine();
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Username cannot be null");
                    return;
                }

                Console.Write("Enter email: ");
                string? email = Console.ReadLine();
                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Email cannot be null");
                    return;
                }

                Console.Write("Password: ");
                string? password = Console.ReadLine();
                if (string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Password cannot be null");
                    return;
                }

                using (var cmd = new NpgsqlCommand("INSERT INTO users (\"username\", \"email\", \"password\") VALUES (@username, @email, @password)", connection))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.Parameters.AddWithValue("password", password);

                    int rows = await cmd.ExecuteNonQueryAsync();
                    Console.WriteLine($"Rows inserted: {rows}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error has occurred: {ex.Message}");
        }
    }
}
