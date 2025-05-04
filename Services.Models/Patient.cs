using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class Patient
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required, StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required, RegularExpression(@"^\d+$", ErrorMessage = "CI debe ser numérico")]
        public string CI { get; set; } = null!;

        public string? BloodGroup { get; set; }
    }
}
