using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class UploadDriverDocumentViewModel
    {
        public List<HttpPostedFileBase> DriverDocument { get; set; }
        public int DriverId { get; set; }
        public int DocumentType { get; set; }
        public string DocumentName { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}