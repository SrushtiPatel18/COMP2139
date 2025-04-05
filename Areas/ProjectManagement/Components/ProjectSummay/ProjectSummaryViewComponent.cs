using COMP2139_Lab02.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//Name: Srushti Maheshkumar Patel
//Student ID: 101485546

namespace COMP2139_Lab02.Areas.ProjectManagement.Components.ProjectSummay;

public class ProjectSummaryViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public ProjectSummaryViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int projectId)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);

        if (project == null)
        {
            return Content("Project not found");
        }
        return View(project);
    }
}