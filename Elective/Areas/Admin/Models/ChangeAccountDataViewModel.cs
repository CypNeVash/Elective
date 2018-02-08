using System;
using System.ComponentModel.DataAnnotations;

namespace Elective
{

    public class ChangeAccountDataViewModel
    {
        [Range(5, 80, ErrorMessageResourceName = "RangeValid", ErrorMessageResourceType = typeof(StringResource))]
        [Display(Name = "UserAge", ResourceType = typeof(StringResource))]
        public int Age { get; set; }

        [DataType(DataType.DateTime, ErrorMessageResourceName = "DateValid", ErrorMessageResourceType = typeof(StringResource))]
        [Display(Name = "Birthday", ResourceType = typeof(StringResource))]
        public DateTime BirthDate { get; set; }

        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "UserName", ResourceType = typeof(StringResource))]
        public string FirstName { get; set; }

        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "UserSecondName", ResourceType = typeof(StringResource))]
        public string SecondName { get; set; }

        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "UserLogin", ResourceType = typeof(StringResource))]
        public string Login { get; set; }

        [EmailAddress]
        [Display(Name = "UserEmail", ResourceType = typeof(StringResource))]
        public string Email { get; set; }

        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword", ResourceType = typeof(StringResource))]
        public string OldPassword { get; set; }

        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(StringResource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(StringResource))]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}