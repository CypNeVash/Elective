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
        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "FacultativeName", ResourceType = typeof(StringResource))]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "Theme", ResourceType = typeof(StringResource))]
        public string Theme { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "StringLengthValid", ErrorMessageResourceType = typeof(StringResource), MinimumLength = 6)]
        [Display(Name = "Description", ResourceType = typeof(StringResource))]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Duration", ResourceType = typeof(StringResource))]
        [Range(0,100500, ErrorMessageResourceName = "RangeMarkValid", ErrorMessageResourceType = typeof(StringResource))]
        public int Duration { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "DateValid", ErrorMessageResourceType = typeof(StringResource))]
        [Display(Name = "StartFacultative", ResourceType = typeof(StringResource))]
        public DateTime StartFacultative { get; set; }

        [Required]
        [Display(Name = "Status", ResourceType = typeof(StringResource))]
        public FacultativeStatus Status { get; set; }
    }
}