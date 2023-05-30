using LarastruckingApp.Entities.Driver_Fumigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.GpsTracker
{
    public class GpsFumigationDetailsDTO
    {
        public Nullable<int> FumigationId { get; set; }
        public string ShipmentRefNo { get; set; }
        public string AirWayBill { get; set; }
        public Nullable<System.DateTime> PickDateTime { get; set; }
        public Nullable<System.DateTime> DeliveryDateTime { get; set; }
        public List<GpsTrackerHistoryDTO> GpsDriverTrackFumigationDetails { get; set; }
        public List<GetFumigationEquipmentNDriversDTO> FumigationEquipmentNdriver { get; set; }

        public List<SaveFumigationGpsTrackingHistoryDTO> SaveFumigationGpsTrackingHistory { get; set; }
    }
}
