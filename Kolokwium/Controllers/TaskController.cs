using Kolokwium.Context;
using Kolokwium.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskEntity = Kolokwium.Models.Task;

namespace Kolokwium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TaskController(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetTasks([FromQuery] int? projectId)
        {
            IQueryable<TaskEntity> query = _context.Tasks.Include(t => t.Project).Include(t => t.Reporter).Include(t => t.Assignee);

            if (projectId.HasValue)
            {
                query = query.Where(t => t.IdProject == projectId.Value);
            }

            var tasks = await query.ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTask(AddTaskRequest request)
        {
            var project = await _context.Projects.FindAsync(request.IdProject);
            if (project == null)
            {
                return BadRequest("Projekt o danym ID nie istnieje");
            }
            var reporter = await _context.Projects.FindAsync(request?.IdReporter);
            if (reporter == null)
            {
                return BadRequest("Reporter o danym ID nie istnieje");
            }

            var reporterAccess = await _context.Accesses.AnyAsync(a => a.IdUser == request.IdReporter && a.IdProject == request.IdProject);
            if (!reporterAccess)
            {
                return BadRequest("Reporter nie ma dostępu do tego projektu.");
            }


            var asigneeAccess = await _context.Accesses.AnyAsync(a => a.IdUser == request.IdAssignee && a.IdProject == request.IdProject);
            if (!asigneeAccess)
            {
                return BadRequest("Assignee nie ma dostępu do tego projektu.");
            }


            var task = new TaskEntity
            {
                Name = request.Name,
                Description = request.Description,
                IdProject = request.IdProject,
                IdReporter = request.IdReporter,
                IdAssignee = request.IdAssignee,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }
    }
       
    
}
