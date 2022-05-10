using System.ComponentModel.DataAnnotations;

namespace Restaurant.Server.Models
{
    public class CustomerModel
    {
        [Required]
        [MinLength(6)]
        [StringLength(32)]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [MinLength(12)]
        [StringLength(32)]
        public string Address { get; set; }
    }
}
