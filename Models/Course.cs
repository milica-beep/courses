using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models;

[Table("Course")]
public class Course
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string? Name { get; set; }

    [Range(1, 5)]
    public int Year { get; set; }

    public List<Student>? Students { get; set; }
}