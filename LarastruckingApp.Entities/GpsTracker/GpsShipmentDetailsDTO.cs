using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.GpsTracker;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities
{
    public class GpsShipmentDetailsDTO
    {
        public Nullable<int> ShipmentId { get; set; }
        public string ShipmentRefNo { get; set; }
        public string AirWayBill { get; set; }
        public Nullable<System.DateTime> PickDateTime { get; set; }
        public Nullable<System.DateTime> DeliveryDateTime { get; set; }
        public List<GpsTrackerHistoryDTO> GpsDriverTrackShipmentDetails { get; set; }
        public List<GetShipmentEquipmentNDriverDTO> ShipmentEquipmentNdriver { get; set; }

        public List<SaveGpsTrackingHistoryDto> SaveGpsTrackingHistory { get; set; }
    }
}
