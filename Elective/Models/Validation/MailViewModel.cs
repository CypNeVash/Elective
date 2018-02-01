using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Elective
{
    public class MailViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [Display(Name = "Login пользователя")]
        public string Login { get; set; }

        [Display(Name = "Theme")]
        public string Theme { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}