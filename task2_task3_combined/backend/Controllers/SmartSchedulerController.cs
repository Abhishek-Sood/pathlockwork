using Microsoft.AspNetCore.Mvc;
using backend.DTOs;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/projects/{projectId}/schedule")]
    public class SmartSchedulerController : ControllerBase
    {
        [HttpPost]
        public IActionResult ScheduleTasks(int projectId, [FromBody] List<ScheduleTaskDto> tasks)
        {
            if (tasks == null || tasks.Count == 0)
                return BadRequest(new { message = "No tasks provided in the request body." });

            // 1️⃣ Build dependency graph
            var graph = new Dictionary<string, List<string>>();
            var indegree = new Dictionary<string, int>();
            var allTitles = new HashSet<string>(tasks.Select(t => t.Title));

            foreach (var task in tasks)
            {
                indegree[task.Title] = 0;
                graph[task.Title] = new List<string>();
            }

            // 2️⃣ Safely handle dependencies (skip invalid or null)
            foreach (var task in tasks)
            {
                var deps = task.Dependencies ?? new List<string>();
                foreach (var dep in deps)
                {
                    // skip dependencies that don't exist
                    if (!allTitles.Contains(dep))
                        continue;

                    graph[dep].Add(task.Title);
                    indegree[task.Title]++;
                }
            }

            // 3️⃣ Modified Topological Sort — also consider due dates
            var queue = new List<string>(indegree.Where(x => x.Value == 0).Select(x => x.Key));
            var result = new List<string>();

            while (queue.Count > 0)
            {
                // sort zero-indegree tasks by due date (nulls last)
                var sorted = queue
                    .OrderBy(t => tasks.First(x => x.Title == t).DueDate == default
                        ? DateTime.MaxValue
                        : tasks.First(x => x.Title == t).DueDate)
                    .ToList();

                var current = sorted.First();
                queue.Remove(current);
                result.Add(current);

                foreach (var next in graph[current])
                {
                    indegree[next]--;
                    if (indegree[next] == 0)
                        queue.Add(next);
                }
            }

            // 4️⃣ Add remaining (if any)
            var remaining = tasks
                .Where(t => !result.Contains(t.Title))
                .OrderBy(t => t.DueDate == default ? DateTime.MaxValue : t.DueDate)
                .Select(t => t.Title)
                .ToList();

            result.AddRange(remaining);

            return Ok(new ScheduleResponseDto
            {
                RecommendedOrder = result
            });
        }
    }
}
