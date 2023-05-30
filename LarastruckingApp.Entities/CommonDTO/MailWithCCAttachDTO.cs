using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Common
{
    public class MailWithCCAttachDTO
    {
        // out string sentMessage, Dictionary<byte[], string> fileByte = null)
        public string Status { get; set; }
        public string MailPurpose { get; set; }
        public string ToMail { get; set; }
        public string ToMailCC { get; set; }
        public string ToMailBCC { get; set; }
        public string MailBody { get; set; }
        public string MailSubject { get; set; }
        public string strMailtype { get; set; }
        public byte[] FileByte { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
