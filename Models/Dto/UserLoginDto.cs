using thu6_pvo_dictionary.Models.Entity;

namespace thu6_pvo_dictionary.Models.Dto
{
    public class UserLoginDto
    {
        public string token { get; set; }
        public User user { get; set; }
    }
}
