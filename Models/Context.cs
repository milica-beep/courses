using Microsoft.EntityFrameworkCore;

namespace Models;

public class Context : DbContext
{
    public required DbSet<Student> Students { get; set; }
    public required DbSet<Course> Courses { get; set; }
    public Context(DbContextOptions options) : base(options)
    {

    }
}
