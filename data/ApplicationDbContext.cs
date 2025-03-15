using COMP2139_Lab02.Areas.ProjectManagement.Models;
using COMP2139_Lab02.Models;
using Microsoft.EntityFrameworkCore;
namespace COMP2139_Lab02.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }    
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>() 
            .HasMany(p => p.Tasks)   
            .WithOne(t => t.Project) 
            .HasForeignKey(t => t.ProjectId) 
            .OnDelete(DeleteBehavior.Cascade); 
  
  
        modelBuilder.Entity<Project>()
            .Property(b => b.StartDate)
            .HasConversion(
                v => v. ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
  
        modelBuilder.Entity<Project>()
            .Property(b => b.EndDate)
            .HasConversion(
                v => v. ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
  
        // Seeds Project table with two projects upon bootup
        modelBuilder.Entity<Project>().HasData(
            new Project { ProjectId = 1, Name = "Assignment 1" , Description = "Comp2139 Assignment 1" },
            new Project { ProjectId = 2, Name = "Assignment 2" , Description = "Comp2139 Assignment 2" }

   
        );
    
    }
}