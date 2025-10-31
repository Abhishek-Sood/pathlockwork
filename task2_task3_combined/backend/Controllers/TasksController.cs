using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TasksController(AppDbContext context) => _context = context;

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpPost("projects/{projectId}/tasks")]
public async Task<IActionResult> AddTask(int projectId, TaskDto dto)
{
    var userId = GetUserId();
    var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
    if (project == null) return NotFound("Project not found.");

    var task = new TaskItem
    {
        Title = dto.Title,
        Description = dto.Description,
        DueDate = dto.DueDate,
        IsCompleted = dto.IsCompleted,
        ProjectId = projectId
    };

    _context.Tasks.Add(task);
    await _context.SaveChangesAsync();

    return Ok(task);
}


        [HttpPut("tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, TaskDto dto)
        {
            var task = await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null || task.Project.UserId != GetUserId())
                return NotFound("Task not found.");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;
            task.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();
            return Ok(task);
        }

        [HttpDelete("tasks/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var task = await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null || task.Project.UserId != GetUserId())
                return NotFound("Task not found.");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Task deleted." });
        }

        [HttpGet("projects/{projectId}/tasks")]
public async Task<IActionResult> GetTasks(int projectId)
{
    var userId = GetUserId();

    var project = await _context.Projects
        .Include(p => p.Tasks)
        .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

    if (project == null)
        return NotFound("Project not found.");

    return Ok(project.Tasks);
}

    }
}
