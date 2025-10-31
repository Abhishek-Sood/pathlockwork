using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private static List<TaskItem> _tasks = new List<TaskItem>
        {
            new TaskItem { Id = Guid.NewGuid(), Description = "Sample task", IsCompleted = false }
        };

        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(_tasks);
        }

        [HttpPost]
        public IActionResult AddTask([FromBody] TaskItem task)
        {
            task.Id = Guid.NewGuid();
            _tasks.Add(task);
            return Ok(task);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid id, [FromBody] TaskItem updatedTask)
        {
            var existing = _tasks.FirstOrDefault(t => t.Id == id);
            if (existing == null)
                return NotFound();

            existing.Description = updatedTask.Description;
            existing.IsCompleted = updatedTask.IsCompleted;

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return NotFound();

            _tasks.Remove(task);
            return NoContent();
        }
    }

    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
