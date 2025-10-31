namespace backend.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
    }
}
