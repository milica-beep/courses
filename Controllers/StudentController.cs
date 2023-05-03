using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace courses.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    public Context Context { get; set; }

    public StudentController(Context context)
    {
        Context = context;
    }

    [HttpPost("AddStudent")]
    public async Task<ActionResult> AddStudent([FromBody]Student student)
    {
        if(student.StudentId < 10000 || student.StudentId > 20000)
        {
            return BadRequest("Student id is out of range.");
        }

        if (string.IsNullOrWhiteSpace(student.Name) || student.Name.Length > 50)
        {
            return BadRequest("Name cannot be empty or above 50 characters.");
        }

        if (string.IsNullOrWhiteSpace(student.Lastname) || student.Lastname.Length > 50)
        {
            return BadRequest("Lastname cannot be empty or above 50 characters.");
        }

        try
        {
            var existingStudent = Context.Students.Where(s => s.StudentId == student.StudentId)
                                                  .FirstOrDefault();
            if(existingStudent != null)
            {
                return BadRequest($"Student with id {student.StudentId} already exists. Please choose another id.");
            }

            Context.Students.Add(student);
            await Context.SaveChangesAsync(); 
            return Ok($"New student with id {student.Id} is added."); 
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetStudent/{id}")]
    public ActionResult GetStudent(int id)
    {
        if(id <= 0)
        {
            return BadRequest("Invalid id.");
        }
        else 
        {
            try
            {
                var student = Context.Students.Include(s => s.Courses).Where(s => s.Id == id).FirstOrDefault();
                if(student != null)
                {
                    return Ok(student);
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
    }

    [HttpPut("UpdateStudent")]
    public async Task<ActionResult> PromeniFromBody([FromBody] Student student)
    {
        if(student.Id <= 0)
        {
            return BadRequest("Invalid id");
        }

        if(student.StudentId < 10000 || student.StudentId > 20000)
        {
            return BadRequest("Student id is out of range.");
        }

        if (string.IsNullOrWhiteSpace(student.Name) || student.Name.Length > 50)
        {
            return BadRequest("Name cannot be empty or above 50 characters.");
        }

        if (string.IsNullOrWhiteSpace(student.Lastname) || student.Lastname.Length > 50)
        {
            return BadRequest("Lastname cannot be empty or above 50 characters.");
        }

        try 
        {
            Context.Students.Update(student);
            await Context.SaveChangesAsync();
            return Ok($"Updated student with id {student.Id}");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeleteStudent/{id}")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid id.");
        }

        else 
        {
            try
            {
                var student = await Context.Students.FindAsync(id);
                if(student != null)
                {
                    Context.Students.Remove(student);
                    await Context.SaveChangesAsync();
                    return Ok($"Student is deleted.");
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
    }

    [HttpGet("GetCourses/{studentId}")]
    public ActionResult GetCourses(int studentId)
    {
        if(studentId < 0)
        {
            return BadRequest("invalid id");
        }

        try
        {
            var student = Context.Students.Include(s => s.Courses).Where(c => c.Id == studentId).FirstOrDefault();
            if(student != null)
            {
                return Ok(student.Courses);
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
