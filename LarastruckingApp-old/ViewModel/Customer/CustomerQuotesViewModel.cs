using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.CustomerFumigation;
using LarastruckingApp.Entities.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel.Customer
{
    public class CustomerQuotesViewModel
    {
        public CustomerQuotesViewModel()
        {
            CustomerQuotes = new List<CustomerQuotesInfoDto>();
            CutomerShipmentRoutesDetail = new List<CustomerShipmentRoutesDto>();
            CustomerShipmentTrack = new CustomerShipmentTrackDto();
            CustomerStatusTrack = new List<CustomerStatusTrackDto>();
            CustomerFumigationRoutesDetail = new List<CustomerFumigationRoutesDto>();
            CustomerFumigationTrack = new CustomerFumigationTrackDto();
            ShipmentFreightList = new List<ShipmentFreightDetailsDto>();
        }
        public List<CustomerQuotesInfoDto> CustomerQuotes { get; set; }

        public List<CustomerShipmentRoutesDto> CutomerShipmentRoutesDetail { get; set; }

        public CustomerShipmentTrackDto CustomerShipmentTrack { get; set; }
        public List<CustomerStatusTrackDto> CustomerStatusTrack { get; set; }
        public List<CustomerFumigationRoutesDto> CustomerFumigationRoutesDetail { get; set; }

        public CustomerFumigationTrackDto CustomerFumigationTrack { get; set; }
        public IList<ShipmentFreightDetailsDto> ShipmentFreightList { get; set; }
        // public List<CustomerStatusTrackDto> CustomerFumigationStatusTrack { get; set; }
    }
}