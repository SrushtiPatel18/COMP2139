using COMP2139_Lab02.Areas.ProjectManagement.Models;
using COMP2139_Lab02.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Lab02.Areas.ProjectManagement.Controllers;
[Area("ProjectManagement")]
[Route("ProjectTask")]

public class ProjectTaskController : Controller

{
    private readonly ApplicationDbContext _context;

    public ProjectTaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("Index/{projectId}")]
    public async Task<IActionResult> Index(int projectId)
    {
        var tasks = await _context
            .Tasks.
            Where(t => t.ProjectId == projectId)
            .ToListAsync();
        
        ViewBag.ProjectId = projectId;
        return View(tasks);
    }

    [HttpGet("Details/{id}")]
    public IActionResult Details(int id)
    {
        var task = _context
            .Tasks
            .Include(p => p.Project)// include the related project for the task
            .FirstOrDefault(t =>t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpGet("Create/{projectId}")]
    public IActionResult Create(int projectId)
    {
        var project = _context.Projects.Find(projectId);
        if (project == null)
        {
            return NotFound();
        }

        //create empty project for view
        var task = new ProjectTask
        {
            ProjectId = projectId,
            Title = "",
            Description = "",
        };
        return View(task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title,Description,ProjectId")] ProjectTask task)
    {
        if (ModelState.IsValid)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();

            return RedirectToAction("Index", new { projectId = task.ProjectId });

        }

        return View(task);
    }

    [HttpGet("Edit/{id}")]
    public IActionResult Edit(int id)
    {
        var task = _context
            .Tasks
            .Include(p => p.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
    {
        if (id != task.ProjectTaskId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }
        return View(task);
    }

    [HttpGet("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var task = _context
            .Tasks
            .Include(p => p.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }
        return View(task);
        
    }

    [HttpPost("Delete/{id}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int ProjectTaskId)
    {
        var task = _context.Tasks.Find(ProjectTaskId);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
            
        }
        return NotFound();
    }

    [HttpGet("Search/{searchTerm},{projectId}")]
    public async Task<IActionResult> Search(int? projectId, string searchString)
    {
        // start a
        var taskQuery = _context.Tasks.AsQueryable();
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        if (projectId.HasValue)
        {
            taskQuery = taskQuery.Where(t => t.ProjectTaskId == projectId);
        }

        if (searchPerformed)
        {
            taskQuery = taskQuery.Where(t => t.Title.ToLower().Contains(searchString)
            ||(t.Description != null && t.Description.ToLower().Contains(searchString.ToLower())));
            
        }
        //as
        var tasks = await taskQuery.ToListAsync();
        ViewBag.ProjectId = projectId;
        ViewData["SearchPerformed"] = searchPerformed;
        ViewData["SearchString"] = searchString;
        
        return View("Index",tasks);
    }
}

