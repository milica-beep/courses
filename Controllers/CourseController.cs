using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace courses.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    public Context Context { get; set; }

    public CourseController(Context context)
    {
        Context = context;
    }

    [HttpPost("AddCourse")]
    public async Task<ActionResult> AddCourse([FromBody] Course course)
    {
        if (string.IsNullOrWhiteSpace(course.Name) || course.Name.Length > 50)
        {
            return BadRequest("Course name cannot be empty or above 50 characters.");
        }

        if(course.Year > 5 || course.Year < 1)
        {
            return BadRequest("Course year is invalid.");
        }

        try
        {
            Context.Courses.Add(course);
            await Context.SaveChangesAsync(); 
            return Ok($"New course with id {course.Id} is added."); 
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("AddStudentToCourse/{studentId}/{courseId}")]
    public async Task<ActionResult> AddStudentToCourse(int studentId, int courseId)
    {
        if(studentId < 0 || courseId < 0)
        {
            return BadRequest("Invalid id");
        }

        try
        {
            var student = Context.Students.Where(s => s.Id == studentId).FirstOrDefault();
            var course = Context.Courses.Where(c => c.Id == courseId).FirstOrDefault();

            if(student != null && course != null)
            {
                student.Courses = student.Courses ?? new();
                course.Students = course.Students ?? new();

                student.Courses.Add(course);
                course.Students.Add(student);

                Context.Students.Update(student);
                await Context.SaveChangesAsync();
                return Ok("Succesfuly added student to a course!");
            }
            else
            {
                return BadRequest("Invalid id");
            }
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetStudents/{courseId}")]
    public ActionResult GetStudents(int courseId)
    {
        if(courseId < 0)
        {
            return BadRequest("invalid id");
        }

        try
        {
            var course = Context.Courses.Include(c => c.Students).Where(c => c.Id == courseId).FirstOrDefault();
            if(course != null)
            {
                return Ok(course.Students);
            }
            else
            {
                return BadRequest("invlaid id");
            }
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}