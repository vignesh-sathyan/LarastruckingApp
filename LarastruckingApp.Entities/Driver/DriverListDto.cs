using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class DriverListDto
    {
        public int DriverId { get; set; }
        public int UserId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? TakenFrom { get; set; }
        public DateTime? TakenTo { get; set; }
        public string LeaveStatus { get; set; }
        public bool IsActive { get; set; }
        public string IsTWIC { get; set; }
        
    }
}
