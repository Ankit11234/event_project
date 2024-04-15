using System.ComponentModel.DataAnnotations;

namespace event_project_1.Models
{
    public class Room
    {
        [Key]
        public int room_id { get; set; }

        [Required]
        [StringLength(50)]
        public string room_name { get; set; }
        public int user_id { get; set; }  // Assuming UserId is an int

        // You can add additional properties if needed
    }

}
