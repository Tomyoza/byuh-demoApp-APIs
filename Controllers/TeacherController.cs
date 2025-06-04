using byuhAPI.Service;
using byuhAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace byuhAPI.Controllers
{
    [ApiController]
    [Route("teacher/Controller")]
    public class TeacherController : ControllerBase
    {
        private readonly MysqlService _mysqlService;
        private readonly TeacherService _teacherService;

        public TeacherController(MysqlService mysqlService, TeacherService teacherService)
        {
            _mysqlService = mysqlService;
            _teacherService = teacherService;
        }


        [HttpGet("GetAll")]
        //[Route("Teacher/GetAll")]
        public IActionResult GetAll()
        {
            var teachers = _teacherService.GetTeachersAll();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        //[Route("GetTeacherById/{id}")]
        public IActionResult GetTeacherById(int id)
        {
            var teacher = _teacherService.GetById(id);
            if (teacher == null)
                return Ok();
            return Ok(teacher);
        }

        [HttpPost]
        [Route("AddTeacher")]
        public IActionResult AddTeacher([FromBody] Teacher teacher)
        {
            try
            {
                teacher.TeacherId = 0;
                _teacherService.AddTeacher(teacher);
                return CreatedAtAction(nameof(AddTeacher), new { id = teacher.TeacherId }, teacher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpPut]
        [Route("UpdateTeacherById")]
        public IActionResult UpdateTeacher(int id, [FromBody] Teacher teacher)
        {
            bool isUpdated = _teacherService.UpdateTeacher(id, teacher.Name, teacher.Age);
            if (!isUpdated)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteTeacherById")]
        public IActionResult DeleteTeacherById(int id)
        {
            bool isDeleted = _teacherService.DeleteTeacher(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
