using byuhAPI.Service;
using byuhAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace byuhAPI.Controllers
{
    [ApiController]
    [Route("enrollment/Controller")]
    public class EnrollmentController : ControllerBase
    {
        private readonly MysqlService _mysqlService;
        private readonly EnrollmentService _enrollmentService;

        public EnrollmentController(MysqlService mysqlService, EnrollmentService enrollmentService)
        {
            _mysqlService = mysqlService;
            _enrollmentService = enrollmentService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var enrollments = _enrollmentService.GetEnrollmentAll();
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public IActionResult GetEnrollmentById(int id)
        {
            var enrollment = _enrollmentService.GetEnrollmentById(id);
            if (enrollment == null)
                return Ok();
            return Ok(enrollment);
        }

        [HttpPost]
        [Route("AddEnrollment")]
        public IActionResult AddEnrollment([FromBody] Enrollment enrollment)
        {
            try
            {
                enrollment.StudentId = 0; // Assuming StudentId is auto-generated
                _enrollmentService.AddEnrollment(enrollment);
                return CreatedAtAction(nameof(AddEnrollment), new { id = enrollment.StudentId }, enrollment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateEnrollmentById")]
        public IActionResult UpdateEnrollment(int id, [FromBody] Enrollment enrollment)
        {
            bool isUpdated = _enrollmentService.UpdateEnrollment(id, enrollment.CourseId);
            if (isUpdated)
            {
                return Ok(enrollment);
            }
            return NotFound($"Enrollment with ID {id} not found.");
        }

        [HttpDelete]
        [Route("DeleteEnrollment")]
        public IActionResult DeleteEnrollment(int id, int courseId)
        {
            bool isDeleted = _enrollmentService.DeleteEnrollment(id, courseId);
            if (isDeleted)
            {
                return Ok($"Enrollment with ID {id} deleted successfully.");
            }
            return NotFound($"Enrollment with ID {id} not found.");
        }

    }
}
