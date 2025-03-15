using System.ComponentModel.DataAnnotations;
using COMP2139_Lab02.Models;

namespace COMP2139_Lab02.Areas.ProjectManagement.Models;

public class Project
{
    // <summary>
    // This is the primary key for projects
    // </summary>
    
    public int ProjectId { get; set; }
    // <summary>
    // The Name of the project
    // [Required]: Ensures this property must be SET must have a project name
    // </summary>
    
    [Required]
    [Display(Name = "Project Name")]
    [StringLength(100,ErrorMessage = "The Project Name cannot be longer than 100 characters.")]
    public  required string Name { get; set; }
    
    
    [Display(Name = "Project Description")]
    [DataType(DataType.MultilineText)]
    [StringLength(500, ErrorMessage = "The Project Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }
    
    
    [Display(Name = "Project Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime StartDate { get; set; }
    
    
    [Display(Name = "Project End Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime EndDate { get; set; }
    
    
    [Display(Name = "Project Status")]
    [StringLength(20, ErrorMessage = "The Project Status cannot be longer than 20 characters.")]
    public string? Status { get; set; }
    
    //One-to-Many
    // This will allow EF core to understand that one Project has Potentially many ProjectsTasks
    public List<ProjectTask>? Tasks { get; set; }
    
}