using MySqlConnector;

namespace byuhAPI.Service
{
    public class MysqlService
    {
        private readonly string _connectionString;

        public MysqlService()
        {
            string[] user = Environment.GetEnvironmentVariable("BYUH").Split(':');
            _connectionString = "server=localhost; userid=" + user[0] + "; password=" + user[1] + "; database=" + "byuh";

        }

        public MySqlConnection GetOpenMySqlConnection()
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
        public static bool ExecuteUpdate(MySqlTransaction act, MySqlCommand cmd)
        {
            int rc = cmd.ExecuteNonQuery();
            if (rc != 0)
            {
                act.Commit();
                return true;
            }
            act.Rollback();
            return false;
        }
        public bool TestConnection()
        {
            try
            {
                using (var connection = GetOpenMySqlConnection())
                {
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database connection failed: " + ex.Message);
                return false;
            }
        }
    }
}
