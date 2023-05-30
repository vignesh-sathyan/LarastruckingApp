using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TrailerRental
{
    public class TrailerRentalDetailDTO
    {
        public int TrailerRentalDetailId { get; set; }
        public Nullable<int> TrailerRentalId { get; set; }
        public Nullable<int> DeliveryLocationId { get; set; }
        public Nullable<int> PickUpLocationId { get; set; }
        public string PickUpLocationText { get; set; }
        public Nullable<int> DeliveryDriverId { get; set; }
        public string DeliveryLocationText { get; set; }
        public Nullable<int> PickupDriverId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<System.DateTime> ReturnedDate { get; set; }
        public Nullable<int> NoOfDays { get; set; }
        public Nullable<int> EquipmentId { get; set; }
        public Nullable<decimal> FeePerDay { get; set; }
        public Nullable<decimal> FixedFee { get; set; }
        public Nullable<decimal> TotalFee { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public string EquipmentNo { get; set; }
    }
}
