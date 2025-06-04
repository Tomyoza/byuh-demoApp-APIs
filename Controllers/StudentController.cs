using byuhAPI.Service;
using byuhAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace byuhAPI.Controllers
{
    [ApiController]
    [Route("Controller")]
    public class StudentController : ControllerBase
    {
        private readonly MysqlService _mysqlService;
        private readonly StudentService _studentService;
        public StudentController(MysqlService mysqlService, StudentService studentService)
        {
            _mysqlService = mysqlService;
            _studentService = studentService;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var students = _studentService.GetStudentsAll();
            return Ok(students);
        }

        [HttpGet]
        [Route("GetStudentById/{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _studentService.GetById(id);
            if (student == null)
                return Ok();
            return Ok(student);
        }

        [HttpPost]
        [Route("AddStudent")]
        public IActionResult AddStudent([FromBody] Student student)
        {
            try
            {
                student.StudentId = 0;
                _studentService.AddStudent(student);
                return CreatedAtAction(nameof(AddStudent), new { id = student.StudentId }, student);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateById")]
        public IActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            bool isUpdated = _studentService.UpdateStudent(id, student.Name, student.Grade);
            if (!isUpdated)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpDelete]
        [Route("DeleteById")]
        public IActionResult DeleteStudent(int id)
        {
            bool isDelted = _studentService.DeleteStudent(id);
            if(!isDelted)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("test-db-connection")]
        public IActionResult TestDatabaseConnection()
        {
            try
            {
                using (var connection = _mysqlService.GetOpenMySqlConnection())
                {
                    return Ok("Database connected successfully.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database connection failed: {ex.Message}");
            }
        }
    }
}
