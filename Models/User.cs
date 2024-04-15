using System.ComponentModel.DataAnnotations;

namespace event_project_1.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public bool AdminOverride { get; set; }

        [Required]
        public string Role { get; set; }
    }

   
}
