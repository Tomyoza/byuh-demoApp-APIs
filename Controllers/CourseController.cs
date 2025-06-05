using Microsoft.AspNetCore.Mvc;
using byuhAPI.Models;
using byuhAPI.Service;

namespace byuhAPI.Controllers
{
    [ApiController]
    [Route("course/Controller")]
    public class CourseController : ControllerBase
    {
        private readonly MysqlService _mysqlService;
        private readonly CourseService _courseService;

        public CourseController(MysqlService mysqlService, CourseService courseService)
        {
            _mysqlService = mysqlService;
            _courseService = courseService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var courses = _courseService.GetCoursesAll();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null || course.CourseId == 0)
                return NotFound();
            return Ok(course);
        }

        [HttpPost]
        [Route("AddCourse")]
        public IActionResult addCourse([FromBody] Course course)
        {
            try
            {
                course.CourseId = 0; // Ensure CourseId is set to 0 for new entries
                _courseService.AddCourse(course);
                return CreatedAtAction(nameof(addCourse), new { id = course.CourseId }, course);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateCourseById")]
        public IActionResult updateCourseById(int id, [FromBody] Course course)
        {
            bool isUpdated = _courseService.UpdateCourse(id, course.Name, course.TeacherId);
            if (!isUpdated)
            {
                return NotFound();
            }
            return NoContent(); // Return 204 No Content on successful update
        }

        [HttpDelete]
        [Route("DeleteCourseById")]
        public IActionResult deleteCourse(int id)
        {
            bool isDeleted = _courseService.DeleteCourse(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent(); // Return 204 No Content on successful deletion
        }
    }
}
