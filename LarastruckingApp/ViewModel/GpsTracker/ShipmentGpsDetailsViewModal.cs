using LarastruckingApp.Entities;
using LarastruckingApp.Entities.GpsTracker;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel.GpsTracker
{
    public class ShipmentGpsDetailsViewModal
    {
        public ShipmentGpsDetailsViewModal()
        {
            GpsShipmentDetails = new GpsShipmentDetailsDTO();
            GetShipmentEquipAndDriver = new List<GetShipmentEquipmentNDriverDTO>();
            GetGpsTrackerDetails = new List<GpsTrackerHistoryDTO>();
        }

        public GpsShipmentDetailsDTO GpsShipmentDetails { get; set; }
        public List<GetShipmentEquipmentNDriverDTO> GetShipmentEquipAndDriver { get; set;}
        public List<GpsTrackerHistoryDTO> GetGpsTrackerDetails { get; set; }
       
    }
}