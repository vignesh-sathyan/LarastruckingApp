using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public  class GetDriverExpDetailsDTO
    {
        public int DriverID { get; set; }
        public int UserId { get; set; }
        public string DriverName { get; set; }
        public string EmailId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime DocumentExpiryDate { get; set; }
        public bool ISEmailSent { get; set; }
        public bool ActiveDriver { get; set; }
        public bool ActiveDocument { get; set; }
        public DateTime? EmailSentDate { get; set; }
    }
}
