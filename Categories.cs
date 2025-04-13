using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models;

public class Categories
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<Products> Products { get; set; }
}