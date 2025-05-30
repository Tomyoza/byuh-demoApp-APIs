using byuhAPI.Models;
using byuhAPI.Service;
using MySqlConnector;


namespace byuhAPI.Service
{
    public class StudentService : MysqlService
    {
        public List<Student> GetStudentsAll()
        {
            List<Student> students = new List<Student>();
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string studentSql = "SELECT student_id, name, grade FROM student;";
                MySqlCommand studentCmd = new MySqlCommand(studentSql, connection);
                using (MySqlDataReader reader = studentCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Student student = new Student()
                        {
                            StudentId = reader.GetInt32("student_id"),
                            Name = reader.GetString("name"),
                            Grade = reader.GetString("grade")
                        };
                        students.Add(student);
                    }
                }
            }
            return students;
        }

        public Student GetById(int id)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string studentSql = "SELECT student_id, name, grade FROM student WHERE student_id = @id;";

                using (MySqlCommand command = new MySqlCommand(studentSql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Student
                            {
                                StudentId = reader.GetInt32("student_id"),
                                Name = reader.GetString("name"),
                                Grade = reader.GetString("grade")
                            };
                        }
                    }
                }
            }
            return new Student(); // Return an empty Student object if not found
        }

        public void AddStudent(Student student)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string addStudentSql = "INSERT INTO student (student_id, name, grade) VALUES (@student_id, @name, @grade);";

                MySqlCommand studentCmd = new MySqlCommand(addStudentSql, connection);
                studentCmd.Parameters.AddWithValue("@student_id", student.StudentId);
                studentCmd.Parameters.AddWithValue("@name", student.Name);
                studentCmd.Parameters.AddWithValue("@grade", student.Grade);

                try
                {
                    // Execute the command to insert the student into the database
                    int rowsAffected = studentCmd.ExecuteNonQuery();

                    // Check if any rows were affected (this means the insert was successful)
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Student added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("No student was added.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the query execution
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public bool UpdateStudent(int id, string name, string grade)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string studentSql = "UPDATE student SET name = @name, grade = @grade WHERE student_id = @id;";
                using (MySqlCommand studentCmd = new MySqlCommand(studentSql, connection))
                {
                    studentCmd.Parameters.AddWithValue(@"id", id);
                    studentCmd.Parameters.AddWithValue(@"name", name);
                    studentCmd.Parameters.AddWithValue(@"grade", grade);

                    int rowsAffected = studentCmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Returns true if the update was successful
                }
            }
        }

        public bool DeleteStudent(int id)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string studentSql = "DELETE FROM student WHERE student_id = @id;";
                using (MySqlCommand studentCmd = new MySqlCommand(studentSql, connection))
                {
                    studentCmd.Parameters.AddWithValue(@"id", id);
                    int rowsAffected = studentCmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Returns true if the delete was successful
                }
            }
        }
    }
}
