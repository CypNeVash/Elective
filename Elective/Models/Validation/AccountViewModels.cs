using System;
using System.ComponentModel.DataAnnotations;

namespace Elective
{

    public class LoginViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "Login", ResourceType = typeof(StringResource))]
        public string Login { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(StringResource))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(StringResource))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Range(5, 80, ErrorMessageResourceName = "RangeValid", ErrorMessageResourceType = typeof(StringResource))]
        [Display(Name = "UserAge", ResourceType = typeof(StringResource))]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "Datavalid", ErrorMessageResourceType = typeof(StringResource))]
        [Display(Name = "Birthday", ResourceType = typeof(StringResource))]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "UserName", ResourceType = typeof(StringResource))]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "UserSecondName", ResourceType = typeof(StringResource))]
        public string SecondName { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "UserLogin", ResourceType = typeof(StringResource))]
        public string Login { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "UserEmail", ResourceType = typeof(StringResource))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(StringResource))]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(StringResource))]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

}
