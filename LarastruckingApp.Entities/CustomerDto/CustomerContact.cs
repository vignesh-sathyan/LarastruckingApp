using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.CustomerDto
{
    public class CustomerContact
    {
        public int ContactId { get; set; }
        public long CustomerId { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactExtension { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTitle { get; set; }
    }
}
