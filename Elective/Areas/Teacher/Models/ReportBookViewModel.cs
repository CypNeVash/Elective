using BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elective
{
    public class ReportBookViewModel
    {
        public IList<ReportViewModel> Reports { get; set; } = new List<ReportViewModel>();
        public Facultative Elective { get; set; }
    }
}