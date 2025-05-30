using byuhAPI.Service;
using byuhAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace byuhAPI.Controllers
{
    [ApiController]
    [Route("controller")]
    public class StudentController : ControllerBase
    {
        private readonly MysqlService _mysqlService;
        private readonly StudentService _studentService;
        public StudentController(MysqlService mysqlService, StudentService studentService)
        {
            _mysqlService = mysqlService;
            _studentService = studentService;
        }

        [HttpGet("GetAll")]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var students = _studentService.GetStudentsAll();
            return Ok(students);
        }

        [HttpGet("{id}")]
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

    }
}
