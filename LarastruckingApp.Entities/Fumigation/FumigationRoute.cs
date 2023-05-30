using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Fumigation
{
    public class FumigationRoute
    {
        public int FumigationRoutsId { get; set; }
        public Nullable<int> RouteNo { get; set; }
        public Nullable<int> FumigationId { get; set; }
        public Nullable<int> FumigationTypeId { get; set; }
        public string AirWayBill { get; set; }
        public string CustomerPO { get; set; }
        public string ContainerNo { get; set; }
        public Nullable<int> PickUpLocation { get; set; }
        public Nullable<System.DateTime> PickUpArrival { get; set; }
        public Nullable<int> FumigationSite { get; set; }
        public Nullable<System.DateTime> FumigationArrival { get; set; }
        public Nullable<int> DeliveryLocation { get; set; }
        public Nullable<System.DateTime> DeliveryArrival { get; set; }
        public Nullable<decimal> PalletCount { get; set; }
        public Nullable<decimal> BoxCount { get; set; }
        public Nullable<int> BoxType { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public string TemperatureType { get; set; }
         public Nullable<decimal> MinFee { get; set; }
        public Nullable<decimal> AddFee { get; set; }
        public Nullable<decimal> UpTo { get; set; }
        public string TrailerPosition { get; set; }
        public Nullable<decimal> TotalFee { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? ReleaseDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string Commodity { get; set; }
        public int? PricingMethod { get; set; }
        public decimal? TrailerDays { get; set; }
        public string VendorNConsignee { get; set; }
       
      public DateTime? DriverLoadingStartTime { get; set; }
        public DateTime? DriverLoadingFinishTime { get; set; }

        public DateTime? DriverFumigationIn { get; set; }
      public DateTime? DriverFumigationRelease { get; set; }
        public Nullable<System.DateTime> DriverPickupArrival { get; set; }
        public Nullable<System.DateTime> DriverPickupDeparture  { get; set; }
        public Nullable<System.DateTime> DriverDeliveryArrival { get; set; }
        public Nullable<System.DateTime> DriverDeliveryDeparture { get; set; }
        // public DateTime? DriverDeliveryDeparture { get; set; }
        //public Nullable<System.DateTime> DeletedOn { get; set; }
        //public Nullable<int> DeletedBy { get; set; }/

    }
}


