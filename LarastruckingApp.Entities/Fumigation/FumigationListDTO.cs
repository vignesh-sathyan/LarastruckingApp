using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Fumigation
{

    public class FumigationOtherList
    {
        public int FumigationId { get; set; }
        public string VendorNConsignee { get; set; }
        public string CustomerName { get; set; }
        public string StatusName { get; set; }
        public string FumigationTypes { get; set; }
        public string TrailerPosition { get; set; }
        public string PickupLocation { get; set; }
        public string FumigationSite { get; set; }
        public string DeliveryLocation { get; set; }
        public string ActLoadingStart { get; set; }
        public string ActLoadingFinish { get; set; }
        public string ActFumigationIn { get; set; }
        public string ActFumigationRelease { get; set; }
        public string ActDepartureDate { get; set; }
        public string ActDeliveryArrival { get; set; }
        public string ActDeliveryDeparture { get; set; }
        public string Temperature { get; set; }
        public string AWB { get; set; }
        public string CustomerPO { get; set; }
        public string ContainerNo { get; set; }
        public string BoxCount { get; set; }
        public string PalletCount { get; set; }
        public string PickUpDriver { get; set; }
        public string PickUpEquipment { get; set; }
        public string DeliveryDriver { get; set; }
        public string DeliveryEquipment { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public string RequestedAt { get; set; }
        public int ApproveStatus { get; set; }
        public string Comments { get; set; }
        public bool WTReady { get; set; }
        public bool STReady { get; set; }
    }

    public class FumigationListDTO
    {
        public int FumigationId { get; set; }
        public string ShipmentRefNo { get; set; }
        public string CustomerName { get; set; }
        public string StatusName { get; set; }
        public string VendorNconsignee { get; set; }
        public string FumigationTypes { get; set; }
        public string TrailerPosition { get; set; }
        public string PickUpLocation { get; set; }
        public string FumigationSite { get; set; }
        public string DeliveryLocation { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public string PickUpArrival { get; set; }
        public Nullable<System.DateTime> FumigationArrival { get; set; }
        public Nullable<System.DateTime> DeliveryArrival { get; set; }
        public string Temperature { get; set; }
        public string AWB_CP_CN { get; set; }
        public string BoxCount { get; set; }
        public string PalletCount { get; set; }
        public string PickUpDriver { get; set; }
        public string PickUpEquipment { get; set; }
        public string DeliveryDriver { get; set; }
        public string DeliveryEquipment { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string AWB { get; set; }
        public string CustomerPO { get; set; }
        public string ContainerNo { get; set; }
        public int ApproveStatus { get; set; }
        public string Comments { get; set; }

    }

    public class FumigationViewAllListDTO
    {
        public int FumigationId { get; set; }
        public string ShipmentRefNo { get; set; }
        public string CustomerName { get; set; }
        public string StatusName { get; set; }
        public string VendorNconsignee { get; set; }
        public string FumigationTypes { get; set; }
        public string TrailerPosition { get; set; }
        public string PickUpLocation { get; set; }
        public string FumigationSite { get; set; }
        public string DeliveryLocation { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public Nullable<System.DateTime> PickUpArrival { get; set; }
        public Nullable<System.DateTime> FumigationArrival { get; set; }
        public Nullable<System.DateTime> DeliveryArrival { get; set; }
        public string Temperature { get; set; }
        public string AWB_CP_CN { get; set; }
        public string BoxCount { get; set; }
        public string PalletCount { get; set; }
        public string PickUpDriver { get; set; }
        public string PickUpEquipment { get; set; }
        public string DeliveryDriver { get; set; }
        public string DeliveryEquipment { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public string AWB { get; set; }
        public string CustomerPO { get; set; }
        public string ContainerNo { get; set; }
        public int ApproveStatus { get; set; }
        public bool WTReady { get; set; }
        public bool STReady { get; set; }

    }

    public class FumigationArchiveListDTO
    {
        public int FumigationId { get; set; }
        public string ShipmentRefNo { get; set; }
        public string CustomerName { get; set; }
        public string StatusName { get; set; }
        public string VendorNconsignee { get; set; }
        public string FumigationTypes { get; set; }
        public string TrailerPosition { get; set; }
        public string PickUpLocation { get; set; }
        public string FumigationSite { get; set; }
        public string DeliveryLocation { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public Nullable<System.DateTime> PickUpArrival { get; set; }
        public Nullable<System.DateTime> FumigationArrival { get; set; }
        public Nullable<System.DateTime> DeliveryArrival { get; set; }
        public string Temperature { get; set; }
        public string AWB_CP_CN { get; set; }
        public string BoxCount { get; set; }
        public string PalletCount { get; set; }
        public string PickUpDriver { get; set; }
        public string PickUpEquipment { get; set; }
        public string DeliveryDriver { get; set; }
        public string DeliveryEquipment { get; set; }
        public string ReleaseDate { get; set; }
        public string AWB { get; set; }
        public string CustomerPO { get; set; }
        public string ContainerNo { get; set; }
        public int ApproveStatus { get; set; }

    }
}
