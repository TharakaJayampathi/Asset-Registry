using System.ComponentModel.DataAnnotations;

namespace test_backend.Models
{
    public class JsonCreateDto
    {
        [Required(ErrorMessage = "JsonData is required.")]
        public string JsonData { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}
