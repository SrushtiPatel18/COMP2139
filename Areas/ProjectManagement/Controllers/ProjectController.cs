using COMP2139_Lab02.Areas.ProjectManagement.Models;
using COMP2139_Lab02.Data;
using COMP2139_Lab02.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Labs.Areas.ProjectManagement.Controllers;
[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
public class ProjectController : Controller
{
     private readonly ApplicationDbContext _context;

     public ProjectController(ApplicationDbContext context)
     {
          _context = context;
     }

     [HttpGet("")]
     public async Task<IActionResult> Index()
     {
          var projects = await _context.Projects.ToListAsync();

          return View(projects);

     }

     [HttpGet("Create")]
     public IActionResult Create()
     {
          return View();
     }

     [HttpPost("Create")]
     [ValidateAntiForgeryToken] // protect for c s r and f
     public async Task <IActionResult> Create(Project project)
     {
          if (ModelState.IsValid)
          {
               _context.Projects.Add(project); //add project to database in memory
               await _context.SaveChangesAsync(); // save changes(persist/commit) to database
               return RedirectToAction("Index"); // redirects  the (ProjectController) to the  Index action
          }

          return View(project);
     }

     [HttpGet("Details/{id}")]
     public async Task<IActionResult> Details(int id)
     {
          //Retrieves the project with the ProjectId specified of returns null if not found
          var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
          if (project == null)
          {
               return NotFound(); // Returns a  404 error if the project is not found
          }

          return View(project);
     }

     [HttpGet("Edit/{id}")]
     public async Task<IActionResult> Edit(int id)
     {
          var project = await _context.Projects.FindAsync(id);
          if (project == null)
          {
               return NotFound();
          }

          return View(project);

     }

     [HttpPost("Edit/{id}")]
     [ValidateAntiForgeryToken]
     public async Task<IActionResult> Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
     {
          if (id != project.ProjectId)
          {
               return NotFound();
          }

          if (ModelState.IsValid)
          {
               try
               {
                    _context.Update(project); // update the project as do not save it so need line 89
                    await _context.SaveChangesAsync(); // commit the changes to database
               }
               catch (DbUpdateConcurrencyException)
               {
                    if (!await ProjectExists(project.ProjectId))
                    {
                         return NotFound();
                    }
                    else
                    {
                         throw; // throws error if the exception can not be identified( unknown)
                    }

               }

               return RedirectToAction("Index");
          }

          return View(project);

     }


     /// <summary>
     /// Checks if project exists in the database
     /// </summary>
     /// <param name="id"></param>
     /// <returns></returns>
     private async Task<bool> ProjectExists(int id)
     {
          return await _context.Projects.AnyAsync(e => e.ProjectId == id);
     }

     [HttpGet("Delete/{id}")]
     public async Task<IActionResult> Delete(int id)
     {
          var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
          if (project == null)
          {
               return NotFound();
          }

          return View(project);
     }

     [HttpPost("Delete"), ActionName("Delete")]
     [ValidateAntiForgeryToken]
     public async Task<IActionResult> DeleteConfirmed(int ProjectId)
     {
          var project = await _context.Projects.FindAsync(ProjectId);
          if (project != null)
          {
               _context.Projects.Remove(project);
               _context.SaveChanges();
               return RedirectToAction("Index");
          }
          return NotFound(); // returns 404 error
     }
     [HttpGet("Search/{searchString}")]

     public  async Task<IActionResult> Search(string searchString)
     {
          //fetch all projects from the database as Queryable collection
          // this allows us to apply filters before executing the database query
          var projectQuery = _context.Projects.AsQueryable();
          
          bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);
          if (!searchPerformed)
          {    
               searchString = searchString.ToLower();
                projectQuery =projectQuery.Where(p =>
                    p.Name.ToLower().Contains(searchString)
               || (p.Description != null && p.Description .ToLower().Contains(searchString)));
               
          }
          //Asynchronous exucution - this  call  does not block the thread while waiting for the database
          // Instead of blocking , ASP.NET can process incoming request while waiting for the result
          
          
          //await releases the current thread while waiting for the query executing
          
          
           var projects= await projectQuery.ToListAsync();
           //store metadata for the view
           ViewData["SearchPerformed"] = searchPerformed;
           ViewData["SearchString"] = searchString;
           return View("Index", projects);
     }
} 