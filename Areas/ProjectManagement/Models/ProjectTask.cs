using System.ComponentModel.DataAnnotations;

namespace COMP2139_Lab02.Areas.ProjectManagement.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Required (ErrorMessage = "Project Title is required")]
    [Display(Name = "Task Name")]  // the order of the annotation does not matter
    [StringLength(100,ErrorMessage = "The Project Title cannot be longer than 100 characters.")]
    public required string Title { get; set; }
    
    [Required(ErrorMessage = "Project Description is required")]
    [Display(Name = "Project Description")]
    [DataType(DataType.MultilineText)]
    [StringLength(500, ErrorMessage = "The Project Description cannot be longer than 500 characters.")]
    public required string Description { get; set; }
    
    
    //foreign key from project
    [Display(Name = "Parent Project ID")]
    public int ProjectId { get; set; }
    
    //Navigation property
    // this property allows for easy access to the related Project entity from a ProjectTask entity
    [Display(Name = "Parent Project")]
    public Project? Project { get; set; }
    
}