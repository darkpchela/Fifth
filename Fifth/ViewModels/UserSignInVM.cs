using System.ComponentModel.DataAnnotations;

namespace Fifth.ViewModels
{
    public class UserSignInVM
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}