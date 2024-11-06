using System.ComponentModel.DataAnnotations;

namespace CodebridgeTestApp.Dtos
{
    public class DogCreateDto
    {
        [Required]
        [MinLength(1)]
        public string name { get; set; }

        [Required]
        [MinLength(1)]
        public string color { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int tail_length { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int weight { get; set; }
    }
}