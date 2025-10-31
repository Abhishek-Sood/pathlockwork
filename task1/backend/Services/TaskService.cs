using backend.Models;

namespace backend.Services
{
    public class TaskService
    {
        private readonly List<TaskItem> _tasks = new();

        public IEnumerable<TaskItem> GetAll() => _tasks;
        public TaskItem Add(TaskItem task)
        {
            _tasks.Add(task);
            return task;
        }

        public TaskItem? Update(Guid id, TaskItem updated)
        {
            var existing = _tasks.FirstOrDefault(t => t.Id == id);
            if (existing == null) return null;

            existing.Description = updated.Description;
            existing.IsCompleted = updated.IsCompleted;
            return existing;
        }

        public bool Delete(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;
            _tasks.Remove(task);
            return true;
        }
    }
}
