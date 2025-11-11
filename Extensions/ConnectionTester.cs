using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace inventoryApiDotnet.Extensions
{
    public class ConnectionTester
    {
        public static void TestConnection(string connectionString)
        {
          try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                Console.WriteLine("✅ Connection to PostgreSQL successful!");
                Console.WriteLine($"Database: {conn.Database}");
                Console.WriteLine($"Host: {conn.Host}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Connection failed:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}