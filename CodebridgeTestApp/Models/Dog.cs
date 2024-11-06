using System.ComponentModel.DataAnnotations;

namespace CodebridgeTestApp.Models
{
    public class Dog
    {
        [Key]
        [Required]
        [MinLength(1)]
        public required string name { get; set; }

        [Required]
        [MinLength(1)]
        public required string color { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public required int tail_length { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public required int weight { get; set; }


        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(name) &&
                name.Length >= 1 &&
                !string.IsNullOrWhiteSpace(color) &&
                color.Length >= 1 &&
                tail_length >= 0 &&
                weight >= 1;
        }
    }
}