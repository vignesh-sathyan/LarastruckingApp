using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class UploadAccidentDocumentViewModel
    {
        public List<HttpPostedFileBase> AccidentImage { get; set; }
        public int? AccidentReportId { get; set; }
        public int AccidentDocumentId { get; set; }
        public string DocumentName { get; set; }

    }
}