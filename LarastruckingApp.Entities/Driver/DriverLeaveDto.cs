using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class DriverLeaveDto 
    {
        public int LeaveId { get; set; }
        public int DriverId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [DisplayName("Taken From")]
        public DateTime TakenFrom { get; set; }
        [DisplayName("Taken To")]
        public DateTime TakenTo { get; set; }
        public DateTime TodayDate { get; set; }
        public string Reason { get; set; }
        [DisplayName("Leave Status")]
        public int LeaveStatusId { get; set; }
        public string LeaveStatus { get; set; }
        public int AppliedBy { get; set; }
        public DateTime AppliedOn { get; set; }
        public bool IsSuccess { get; set; }
        public string Response { get; set; }

    }
}
