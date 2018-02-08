using System.ComponentModel.DataAnnotations;

namespace Elective
{
    public class MailViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "Login", ResourceType = typeof(StringResource))]
        public string Login { get; set; }

        [Display(Name = "Theme", ResourceType = typeof(StringResource))]
        public string Theme { get; set; }

        [Required]
        [Display(Name = "Message", ResourceType = typeof(StringResource))]
        public string Message { get; set; }
    }
}