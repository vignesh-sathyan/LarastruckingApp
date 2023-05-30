using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class DriverDocumentDto
    {
        public int DocumentId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ImageName { get; set; }
        public string ImageURL { get; set; }
    }
}
