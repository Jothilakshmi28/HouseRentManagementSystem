using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HouseRentManagementSystem.ViewModel
{
    public class RegisterRenter
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [StringLength(50, MinimumLength = 1)]
        //Required attribute implements validation on Model item that this fields is mandatory for user
        [Required]
        //We are also implementing Regular expression to check if email is valid like a1@test.com
        //[RegularExpression(@"^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid E-mail ID")]
        //[RegularExpression(@"^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid E-mail ID")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail id is not valid")]

        public string Email { get; set; }

        [Required]
        //We are also implementing Regular expression to check if email is valid like a1@test.com
        [RegularExpression(@"^([0]|\+91)?[6789]\d{9}$", ErrorMessage = "Invalid Phone number")]
        public long PhoneNumber { get; set; }


        [Required]

        //We are also implementing Regular expression to check if email is valid like a1@test.com
        [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Aadhar number id is not valid")]
        public long AadharNumber { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "=>Password must be between 8 and 15 characters \n  =>Password must contain one uppercase letter \n  =>Password must contain  one lowercase letter \n  =>Password must contain one digit")]

        public string Password { get; set; }
       
       
        public byte[] ProfileImage
        {
            get; set;
        }

    }
}