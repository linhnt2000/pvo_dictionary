using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Request
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

}
