using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Auth.ViewModels
{
    public class RegisterVm
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}