using byuhAPI.Models;
using MySqlConnector;

namespace byuhAPI.Service
{
    public class EnrollmentService : MysqlService
    {
        public List<Enrollment> GetEnrollmentAll()
        {
            List<Enrollment> enrollments = new List<Enrollment>();
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string enrollmentSql = "SELECT student_id, course_id FROM enrollment;";
                MySqlCommand enrollmentCmd = new MySqlCommand(enrollmentSql, connection);
                using (MySqlDataReader reader = enrollmentCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Enrollment enrollment = new Enrollment()
                        {
                            StudentId = reader.GetInt32("student_id"),
                            CourseId = reader.GetInt32("course_id")
                        };
                        enrollments.Add(enrollment);
                    }
                }
            }
            return enrollments;
        }

        public Enrollment GetEnrollmentById(int student)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string enrollmentSql = "SELECT student_id, course_id FROM enrollment WHERE student_id = @student;";
                using (MySqlCommand command = new MySqlCommand(enrollmentSql, connection))
                {
                    command.Parameters.AddWithValue("@student", student);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Enrollment
                            {
                                StudentId = reader.GetInt32("student_id"),
                                CourseId = reader.GetInt32("course_id")
                            };
                        }
                    }
                }
                return new Enrollment(); // Return an empty Enrollment object if not found
            }
        }

        public void AddEnrollment(Enrollment enrollment)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string enrollmentSql = "INSERT INTO enrollment (student_id, course_id) VALUES (@studentId, @courseId);";
                using (MySqlCommand command = new MySqlCommand(enrollmentSql, connection))
                {
                    command.Parameters.AddWithValue("@studentId", enrollment.StudentId);
                    command.Parameters.AddWithValue("@courseId", enrollment.CourseId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool UpdateEnrollment(int studentId, int courseId)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string updateSql = "UPDATE enrollment SET course_id = @courseId WHERE student_id = @studentId;";
                using (MySqlCommand command = new MySqlCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if the update was successful
                }
            }
        }

        public bool DeleteEnrollment(int studentId, int courseId)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string deleteSql = "DELETE FROM enrollment WHERE student_id = @studentId AND course_id = @courseId;";
                using (MySqlCommand command = new MySqlCommand(deleteSql, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if the deletion was successful
                }
            }

        }
    }
}
