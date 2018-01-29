using BusinessModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Elective
{

    public class FacultativeViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [Display(Name = "Theme")]
        public string Theme { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Duration")]
        [Range(0,100500, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.")]
        public int Duration { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start facultative time")]
        public DateTime StartFacultative { get; set; }

        [Required]
        public FacultativeStatus Status { get; set; }
    }
}