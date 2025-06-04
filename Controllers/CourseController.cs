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
    }
}
