using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TimeCardDTO
{
   public class ValidateScheduledCheckInTimeDTO
    {
        public int Result { get; set; }
        public DateTime? ScheduledCheckIn { get; set; }
    }
}
