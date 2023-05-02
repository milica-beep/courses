using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("Student")]
public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(10000, 20000)]
    public int StudentId { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Lastname { get; set; }

    public List<Course>? Courses { get; set; }
}