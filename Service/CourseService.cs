using byuhAPI.Models;
using MySqlConnector;

namespace byuhAPI.Service
{
    public class CourseService : MysqlService
    {
        public List<Course> GetCoursesAll()
        {
            List<Course> courses = new List<Course>();
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string courseSql = "SELECT course_id, name, teacher_id FROM course;";
                MySqlCommand courseCmd = new MySqlCommand(courseSql, connection);
                using (MySqlDataReader reader = courseCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Course course = new Course()
                        {
                            CourseId = reader.GetInt32("course_id"),
                            Name = reader.GetString("name"),
                            TeacherId = reader.GetInt32("teacher_id")
                        };
                        courses.Add(course);
                    }
                }
            }
            return courses;
        }

        public Course GetCourseById(int id)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string courseSql = "SELECT course_id, name, teacher_id FROM course WHERE course_id = @id;";
                using (MySqlCommand command = new MySqlCommand(courseSql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Course
                            {
                                CourseId = reader.GetInt32("course_id"),
                                Name = reader.GetString("name"),
                                TeacherId = reader.GetInt32("teacher_id")
                            };
                        }
                    }
                }
                return new Course(); // Return an empty Course object if not found
            }
        }

        public void AddCourse(Course course)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string addCourseSql = "INSERT INTO course (name, teacher_id) VALUES (@name, @teacherId);";

                MySqlCommand command = new MySqlCommand(addCourseSql, connection);
                command.Parameters.AddWithValue("@name", course.Name);
                command.Parameters.AddWithValue("@teacherId", course.TeacherId);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Optionally, you can retrieve the last inserted ID if needed
                        Console.WriteLine("Course added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("No course was added.");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error adding course: " + ex.Message);
                    throw;
                }
            }
        }

        public bool UpdateCourse(int id, string name, int teacherId)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string updateCourseSql = "UPDATE course SET name = @name, teacher_id = @teacherId WHERE course_id = @id;";
                MySqlCommand command = new MySqlCommand(updateCourseSql, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@teacherId", teacherId);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0; // Returns true if the update was successful
            }
        }

        public bool DeleteCourse(int id)
        {
            using (MySqlConnection connection = GetOpenMySqlConnection())
            {
                string deleteCourseSql = "DELETE FROM course WHERE course_id = @id;";
                MySqlCommand command = new MySqlCommand(deleteCourseSql, connection);
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0; // Returns true if the deletion was successful
            }
        }
    }
}
