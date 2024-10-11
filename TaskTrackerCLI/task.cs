using System;

namespace TaskTracker
{
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // "todo", "in-progress", "done"
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
