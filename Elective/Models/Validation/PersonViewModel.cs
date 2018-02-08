using System;
using System.ComponentModel.DataAnnotations;

namespace Elective
{
    public class PersonViewModel
    {
        [Required]
        [Range(5, 80, ErrorMessageResourceName = "RangeValid", ErrorMessageResourceType = typeof(StringResource))]
        [Display(Name = "UserAge", ResourceType = typeof(StringResource))]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "DateValid", ErrorMessageResourceType = typeof(StringResource))]
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
    }
}