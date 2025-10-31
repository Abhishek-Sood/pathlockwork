namespace backend.DTOs
{
    public class ScheduleTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public int EstimatedHours { get; set; } = 1;
        public DateTime? DueDate { get; set; }   // ✅ optional now
        public List<string> Dependencies { get; set; } = new();
    }

    public class ScheduleResponseDto
    {
        public List<string> RecommendedOrder { get; set; } = new();
    }
}
