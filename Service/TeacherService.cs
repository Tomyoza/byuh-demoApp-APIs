using byuhAPI.Models;
using byuhAPI.Service;
using MySqlConnector;

namespace byuhAPI.Service
{
    public class TeacherService : MysqlService
    {
        public List<Teacher> GetTeachersAll()
        {
            List<Teacher> teachers = new List<Teacher>();
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string teacherSql = "SELECT teacher_id, name, age FROM teacher;";
                MySqlCommand command = new MySqlCommand(teacherSql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Teacher teacher = new Teacher()
                        {
                            TeacherId = reader.GetInt32("teacher_id"),
                            Name = reader.GetString("name"),
                            Age = reader.GetInt32("age")
                        };
                        teachers.Add(teacher);
                    }
                }
            }
            return teachers;
        }

        public Teacher GetById(int id)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string teacherSql = "SELECT teacher_id, name, age FROM teacher WHERE teacher_id = @id;";
                using (MySqlCommand command = new MySqlCommand(teacherSql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Teacher
                            {
                                TeacherId = reader.GetInt32("teacher_id"),
                                Name = reader.GetString("name"),
                                Age = reader.GetInt32("age")
                            };
                        }
                    }
                }

            }
            return new Teacher(); // Return an empty Teacher object if not found
        }

        public bool AddTeacher(Teacher teacher)
        {
            string addTeacherSql = "INSERT INTO teacher (teacher_id, name, age) VALUES (@teacher_id, @name, @age);";
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                MySqlCommand command = new MySqlCommand(addTeacherSql, connection);
                command.Parameters.AddWithValue("@teahcer_id", teacher.TeacherId);
                command.Parameters.AddWithValue("@name", teacher.Name);
                command.Parameters.AddWithValue("@age", teacher.Age);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0; // Return true if the insert was successful
            }
        }

        public bool UpdateTeacher(int id, string name, int age)
        {
            string updateTeacherSql = "UPDATE teacher SET name = @name, age = @age WHERE teacher_id = @id;";
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                MySqlCommand command = new MySqlCommand(updateTeacherSql, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@age", age);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0; // Return true if the update was successful
            }
        }

        public bool DeleteTeacher(int id)
        {
            string deleteTeacherSql = "DELETE FROM teacher WHERE teacher_id = @id;";
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                MySqlCommand command = new MySqlCommand(deleteTeacherSql, connection);
                command.Parameters.AddWithValue("@id", id);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0; // Return true if the delete was successful
            }

        }
    }
}
