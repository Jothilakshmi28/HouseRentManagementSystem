using System.ComponentModel.DataAnnotations;


namespace HouseRentManagementSystem.ViewModel
{
    public class LoginTenant
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail id is not valid")]

        public string Email { get; set; }
        [Required]

        public string Password { get; set; }

    }
}