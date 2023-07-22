using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Request
{
    public class UpdateUserInforRequest
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Position { get; set; }
        public IFormFile? avatar { get; set; }
    }
}
