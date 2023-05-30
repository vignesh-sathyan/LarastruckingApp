using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.Driver_Fumigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel.PreTrip
{
    public class PreTripViewModel
    {
        public PreTripViewModel()
        {
           
            ShipmentsDetail = new List<ShipmentRoutesDto>();
            ShipmentFreightList = new List<ShipmentFreightDetailsDto>();
            FumigationDetail = new List<FumigationRoutesDTO>();
            FumimgationFreightList = new List<FumigationFreightDetailsDto>();
        }
      
        public int DriverLanguage { get; set; }
        public List<ShipmentRoutesDto> ShipmentsDetail { get; set; }
        public IList<ShipmentFreightDetailsDto> ShipmentFreightList { get; set; }

        public List<FumigationRoutesDTO> FumigationDetail { get; set; }

        public IList<FumigationFreightDetailsDto> FumimgationFreightList { get; set; }


    }
}