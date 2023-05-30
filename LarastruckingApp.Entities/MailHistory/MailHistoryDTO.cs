using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.MailHistory
{
    public class MailHistoryDTO : CommonDTO
    {
        public int MailHistoryId { get; set; }
        public string MailPurpose { get; set; }
        public string Status { get; set; }
        public string ToMail { get; set; }
        public string ToMailCC { get; set; }
        public string ToMailBCC { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public byte[] FileByte { get; set; }
        public string ErrorMessage { get; set; }
        public string Trace { get; set; }
        public string InnerException { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
                      
    }
}
