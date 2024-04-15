using System.ComponentModel.DataAnnotations;

namespace event_project_1.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [StringLength(50)]
        public string Recurrence { get; set; }

        public int? RecurrenceInterval { get; set; } = 1;

        [StringLength(50)]
        public string RecurrenceFrequency { get; set; } = "daily";

        [StringLength(50)]
        public string RecurrenceEndType { get; set; } = "occurrences";

        public DateTime? EndDate { get; set; } = null;

        public int? Occurrences { get; set; } = null;

        public int? RoomId { get; set; } = null ;

    }
}
