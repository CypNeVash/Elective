using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Elective
{
    public class ReportViewModel
    {
        [Required]
        [Range(0,100, ErrorMessage = "Значение range 0-100")]
        [Display(Name = "Name")]
        public int Mark { get; set; }

        [Required]
        public Guid Id { get; set; }
    }
}