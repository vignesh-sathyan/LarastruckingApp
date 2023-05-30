using LarastruckingApp.Entities;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
    public interface IShipmentBAL
    {
        List<ShipmentStatusDTO> GetStatusList();
        List<ShipmentSubStatusDTO> GetSubStatusList(int statusid);
        List<ShipmentEquipmentDTO> EquipmnetList(ValidateDriverNEquipmentDTO model);
        List<ShipmentDriverDTO> DriverList(ValidateDriverNEquipmentDTO entity);
        List<ShipmentAccessorialFeeTypeDTO> GetShipmentAccessorialFeeType();
        ShipmentDTO Add(ShipmentDTO entity);
        IList<GetFreightTypeDTO> getFreightType(CustomernNRouteInfoDTO entity);
        IList<GetPricingMethodDTO> getPricingMethod(CustomernNRouteInfoDTO entity);
        bool ValidateRouteStop(ValidateRouteStopDTO enity);
        Task<List<AllShipmentList>> ViewShipmentList(AllShipmentDTO entity);
        List<ViewShipmentListDTO> ViewAllShipmentList(ViewShipmentDTO entity, out int recordsTotal);
        GetShipmentDTO GetShipmentById(int shipmentId);
        int UploadDamageDocument(List<GetDamageImages> damageImageList);
        int UploadProofofTempDocument(List<GetProofOfTemprature> proofofTempraturesList);
        GetShipmentDTO EditShipment(GetShipmentDTO entity);
        GetShipmentRouteStopDTO AddRouteStops(GetShipmentRouteStopDTO model);
        GetShipmentFreightDetailDTO AddFreightDetail(GetShipmentFreightDetailDTO model);
        bool DeleteProofOfTemprature(GetProofOfTemprature model);
        bool DeleteDamageFile(GetDamageImages model);
        bool DeleteShipment(ShipmentDTO entity);
        ShipmentDTO ApprovedShipment(ShipmentDTO entity);
        IList<GetFreightTypeDTO> bindFreightType();
        bool ShipmentIsReady(int shipmentId, bool ready);
        ShipmentEmailDTO GetCustomerDetail(ShipmentEmailDTO customerShipmentDTO);
        List<GetShipmentRouteStopDTO> ShipmentProofOfDelivery(string shipmentId);
        bool ApprovedDamageImage(GetDamageImages entity);
        bool ApprovedProofOFTemp(GetProofOfTemprature entity);
        List<MatchEquipmentNDriverDTO> ValidateEquipmentNDriver(ValidateDriverNEquipmentDTO model);
        List<MatchEquipmentNDriverDTO> ValidateDriver(ValidateDriverNEquipmentDTO model);
        List<MatchEquipmentNDriverDTO> ValidateEquipment(ValidateDriverNEquipmentDTO model);
        bool SaveCopyShipmentDetail(CopyShipmentDTO entity);
        CopyShipmentDTO GetCopyShipmentDetailById(int shipmentId);
        int? GetMaxRouteNo(int shipmentId);
        IList<AllShipmentList> AllShipmentList(AllShipmentDTO entity);
        List<ShipmentDriverDTO> DriverList();
        DateTime? GetCheckInTime(int driverId);
        string DriverPhone(int driverId);
        string CustomerName(int CustomerID);
    }
}
