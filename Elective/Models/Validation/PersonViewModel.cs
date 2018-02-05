using System;
using System.ComponentModel.DataAnnotations;

namespace Elective
{
    public class PersonViewModel
    {
        [Required]
        [Range(5, 80, ErrorMessage = "Значение {0} должно быть больше чеме {1}."),]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Birthday")]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [Display(Name = "Имя пользователя")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [Display(Name = "Фамилия пользователя")]
        public string SecondName { get; set; }
    }
}