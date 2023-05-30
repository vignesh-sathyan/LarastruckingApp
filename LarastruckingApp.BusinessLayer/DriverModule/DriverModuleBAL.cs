using LarastruckingApp.DAL.DriverModule;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.Driver_Fumigation;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.DriverModule
{
    public class DriverModuleBAL : IDriverModuleBAL
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly IDriverModuleDAL iDriverModuleDAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="IDriverModuleDAL"></param>
        public DriverModuleBAL(IDriverModuleDAL IDriverModuleDAL)
        {
            iDriverModuleDAL = IDriverModuleDAL;
        }
        #endregion


        #region Section 1--> Driver Shipment
        #region Get All Pre-Trip Shipment Details
        /// <summary>
        /// Get All Pre-Trip Shipment Details
        /// </summary>
        /// <returns></returns>
        public IList<PreTripShipmentDto> GetPreTripShipmentDetails(DataTableFilterDto dto, int userId)
        {
            return iDriverModuleDAL.GetPreTripShipmentDetails(dto, userId);
        }
        #endregion

        #region Get Pre-Trip Check List
        /// <summary>
        /// Get Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        public DriverPreTripDto GetPreTripCheckList(int shipmentId)
        {
            return iDriverModuleDAL.GetPreTripCheckList(shipmentId);
        }
        #endregion

        #region Save Pre-Trip Check List
        /// <summary>
        /// Save Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        public bool SavePreTripCheckList(DriverPreTripDto dto)
        {

            return iDriverModuleDAL.SavePreTripCheckList(dto);
        }
        #endregion

        #region Get Routes of Pre-Trip Shipment
        /// <summary>
        /// Get all the routes of Pre-Trip Shipment by shipment id
        /// </summary>
        /// <returns></returns>

        public List<ShipmentRoutesDto> GetShipmentRoutes(int shipmentId, long userID)
        {
            return iDriverModuleDAL.GetShipmentRoutes(shipmentId, userID);
        }
        #endregion

        #region Get Shipment Location Details
        /// <summary>
        /// Get Shipment Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>


        public ShipmentLocationDetailsDto GetShipmentLocationDetails(int ShippingRoutesId)
        {
            return iDriverModuleDAL.GetShipmentRoutesDetails(ShippingRoutesId);
        }
        #endregion

        #region Save Pre-Trip Shipment Detail
        /// <summary>
        /// Save Pre-Trip Shipment Detail
        /// </summary>
        /// <returns></returns>
        public bool SavePreTripShipmentDetail(PreTripAddShipmentDetailDto dto, out bool isEmailNeedToSend)
        {
            return iDriverModuleDAL.SavePreTripShipmentDetail(dto, out isEmailNeedToSend);
        }
        #endregion

        #region Save Proof of Temperature
        /// <summary>
        /// Save Proof of Temperature
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool SaveProofOfTemperature(PreTripAddShipmentDetailDto dto)
        {

            return iDriverModuleDAL.SaveProofOfTemperature(dto);
        }
        #endregion

        #region Save Damaged Files 
        /// <summary>
        /// Save Damaged Files 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool SaveDamagedFiles(PreTripAddShipmentDetailDto dto)
        {

            return iDriverModuleDAL.SaveDamagedFiles(dto);
        }
        #endregion

        #region Get Shipment Freight Details
        /// <summary>
        /// Get Shipment Freight Details By ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public List<ShipmentFreightDetailsDto> GetShipmentFreightDetails(int ShippingRoutesId)
        {
            return iDriverModuleDAL.GetShipmentFreightDetails(ShippingRoutesId);
        }

        #endregion

        #region Get Pre-Trip Check Timings for Arrival & departure
        /// <summary>
        /// Get Pre-Trip Check Timings for Arrival & departure
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public PreTripTimmingDetailsDto GetPreTripCheckTimings(int ShippingRoutesId)
        {
            //return iDriverModuleDAL.GetPreTripCheckTimings(ShippingRoutesId);
            var PreTripCheckTimings = iDriverModuleDAL.GetPreTripCheckTimings(ShippingRoutesId);

            PreTripCheckTimings.DriverPickupArrival = PreTripCheckTimings.DriverPickupArrival == null ? PreTripCheckTimings.DriverPickupArrival : Configurations.ConvertDateTime(Convert.ToDateTime(PreTripCheckTimings.DriverPickupArrival));
            PreTripCheckTimings.DriverPickupDeparture = PreTripCheckTimings.DriverPickupDeparture == null ? PreTripCheckTimings.DriverPickupDeparture : Configurations.ConvertDateTime(Convert.ToDateTime(PreTripCheckTimings.DriverPickupDeparture));
            PreTripCheckTimings.DriverDeliveryArrival = PreTripCheckTimings.DriverDeliveryArrival == null ? PreTripCheckTimings.DriverDeliveryArrival : Configurations.ConvertDateTime(Convert.ToDateTime(PreTripCheckTimings.DriverDeliveryArrival));
            PreTripCheckTimings.DriverDeliveryDeparture = PreTripCheckTimings.DriverDeliveryDeparture == null ? PreTripCheckTimings.DriverDeliveryDeparture : Configurations.ConvertDateTime(Convert.ToDateTime(PreTripCheckTimings.DriverDeliveryDeparture));
            return PreTripCheckTimings;
        }

        #endregion


        #region Get Pre-Trip Check Timings for Arrival & departure
        /// <summary>
        /// Get Pre-Trip Check Timings for Arrival & departure
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public List<GetShipmentRouteStopDTO> GetShipmentRoutesStopDetail(int ShippingRoutesId)
        {
            return iDriverModuleDAL.GetShipmentRoutesStopDetail(ShippingRoutesId);
        }

        #endregion

        #region Get shipment commment deail
        /// <summary>
        /// Get shipment commment deail
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public List<ShipmentCommentDTO> GetShipmentComments(int shipmentId)
        {
            return iDriverModuleDAL.GetShipmentComments(shipmentId);
        }
        #endregion

        #region Get Shipment Damaged Files
        /// <summary>
        /// Get Shipment Damaged Files By ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public List<ShipmentDamagedEditBindDto> GetShipmentDamagedFiles(int ShippingRoutesId)
        {
            return iDriverModuleDAL.GetShipmentDamagedFiles(ShippingRoutesId);
        }
        #endregion

        #region  Get shipment Proof of Temp 
        /// <summary>
        ///  Get shipment Proof of Temp 
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <param name="ShipmentFreightDetailId"></param>
        /// <returns></returns>
        public List<ShipmentProofOfTempEditBind> GetShipmentProofOfTempFiles(int ShippingRoutesId, int ShipmentFreightDetailId)
        {
            return iDriverModuleDAL.GetShipmentProofOfTempFiles(ShippingRoutesId, ShipmentFreightDetailId);
        }
        #endregion
        #region  Delete proof of temprature
        /// <summary>
        /// Delete proof of temprature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteProofOfTemprature(ShipmentProofOfTempEditBind model)
        {
            return iDriverModuleDAL.DeleteProofOfTemprature(model);
        }
        #endregion

        #region Delete Damage Files
        /// <summary>
        /// Delete Damage Files
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteDamageFiles(ShipmentDamagedEditBindDto model)
        {
            return iDriverModuleDAL.DeleteDamageFiles(model);
        }

        #endregion

        #region Driver status

        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            return iDriverModuleDAL.GetStatusList();
        }
        #endregion

        #region Driver sub status
        /// <summary>
        /// get sub status list
        /// </summary>
        /// <param name="statusid"></param>
        /// <returns></returns>
        public List<ShipmentSubStatusDTO> GetSubStatusList(int statusid)
        {
            return iDriverModuleDAL.GetSubStatusList(statusid);
        }
        #endregion

        #region Update Signature and Name
        /// <summary>
        /// Update Signature and Name
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool UpdateSignaturePadDetail(PreTripAddShipmentDetailDto dto)
        {
            return iDriverModuleDAL.UpdateSignaturePadDetail(dto);
        }
        #endregion


        #region select Signature and Name
        /// <summary>
        /// select Signature and Name
        /// </summary>
        /// <param name="shipmentRouteId"></param>
        /// <returns></returns>
        public bool SelectignaturePadDetail(int shipmentRouteId)
        {
            return iDriverModuleDAL.SelectSignaturePadDetail(shipmentRouteId);
        }
        #endregion

        #region GPS Tracker 
        /// <summary>
        ///  GPS Tracker
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool SaveGPSTracker(SaveGpsTrackingHistoryDto dto)
        {
            return iDriverModuleDAL.SaveGPSTracker(dto);
        }

        #endregion

        #region Save Shipment web-Camera  
        /// <summary>
        /// Save Shipment web-Camera 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public bool saveShipmentWebCamera(SaveShipmentWebCameraDTO dto)
        {
            return iDriverModuleDAL.saveShipmentWebCamera(dto);
        }
        #endregion

        #region Save Shipment Damage web-Camera
        /// <summary>
        /// Save Shipment Damage web-Camera 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public bool saveShipmentDamageWebCamera(SaveShipmentDamageWebCamDTO dto)
        {
            return iDriverModuleDAL.saveShipmentDamageWebCamera(dto);
        }
        #endregion

        #region Save Shipment Waiting Time
        /// <summary>
        /// Save Shipment Waiting Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool SaveWaitingTime(SaveShipmentWaitingNotifiDto dto)
        {
            return iDriverModuleDAL.SaveWaitingTime(dto);
        }
        #endregion


        #endregion

        #region Section 2--> Driver Fumigation

        #region Get Fumigation Pre-Trip Check List
        /// <summary>
        /// Get Fumigation Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        public FumigationPreTripCheckUpDTO GetPreTripCheckFumigationList(int FumigationId)
        {
            return iDriverModuleDAL.GetPreTripCheckFumigationList(FumigationId);
        }
        #endregion

        #region Save Fumigation Pre-Trip Check List
        /// <summary>
        /// Save Fumigation Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        public bool SaveFumigationPreTripCheckList(FumigationPreTripCheckUpDTO dto)
        {

            return iDriverModuleDAL.SaveFumigationPreTripCheckList(dto);
        }
        #endregion

        #region Get Fumigation Routes of Pre-Trip Shipment
        /// <summary>
        /// Get all the Fumigation routes of Pre-Trip Shipment by shipment id
        /// </summary>
        /// <returns></returns>

        public List<FumigationRoutesDTO> GetFumigationRoutes(int FumigationId, long userID)
        {
            return iDriverModuleDAL.GetFumigationRoutes(FumigationId, userID);
        }
        #endregion

        #region Get Fumigation Location Details
        /// <summary>
        ///  Get Shipment Details for Multiple Routes By FumigationRouts Id 
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        public FumigationLocationDetailsDTO GetFumigationRoutesDetails(int FumigationRoutsId)
        {
            return iDriverModuleDAL.GetFumigationRoutesDetails(FumigationRoutsId);
        }
        #endregion

        #region Get Fumigation Freight Details
        /// <summary>
        ///  Get Fumigation Freight Details By ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public List<FumigationFreightDetailsDto> GetFumigationFreightDetails(int FumigationRoutsId)
        {
            return iDriverModuleDAL.GetFumigationFreightDetails(FumigationRoutsId);
        }
        #endregion

        #region Save Fumigation Detail
        /// <summary>
        ///  Save Fumigation Detail
        /// </summary>
        /// <returns></returns>
        public bool SaveFumigationtDetail(SaveFumigationDetailsDTO dto, out bool isEmailNeedToSend)
        {

            return iDriverModuleDAL.SaveFumigationtDetail(dto, out isEmailNeedToSend);
        }
        #endregion

        #region Save Fumigation Proof of Temperature
        /// <summary>
        /// Save Fumigation Proof of Temperature
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool SaveFumigationProofOfTemperature(SaveFumigationDetailsDTO dto)
        {

            return iDriverModuleDAL.SaveFumigationProofOfTemperature(dto);
        }
        #endregion
        #region Save Damaged Files 
        /// <summary>
        /// Save Damaged Files 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool SaveFumigationDamagedFiles(SaveFumigationDetailsDTO dto)
        {

            return iDriverModuleDAL.SaveFumigationDamagedFiles(dto);
        }
        #endregion





        #region Get Fumigation Damaged Files
        /// <summary>
        /// Get Fumigation Damaged Files By ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public List<FumigationDamagedEditBindDto> GetFumigationDamagedFiles(int FumigationRoutsId)
        {
            return iDriverModuleDAL.GetFumigationDamagedFiles(FumigationRoutsId);
        }
        #endregion

        #region  Get Fumigation Proof of Temp 
        /// <summary>
        /// Get Fumigation Proof of Temp
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>

        public List<FumigationProofOfTempEditBind> GetFumigationProofOfTempFiles(int FumigationRoutsId)
        {
            return iDriverModuleDAL.GetFumigationProofOfTempFiles(FumigationRoutsId);
        }
        #endregion

        #region Get Driver Actual Timings for Arrival & departure
        /// <summary>
        ///  Get Driver Actual Timings for Arrival & departure
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        public DriverActualTimmingsDTO GetDriverActualTimings(int FumigationRoutsId)
        {
            var DriverActualTimings = iDriverModuleDAL.GetDriverActualTimings(FumigationRoutsId);

            DriverActualTimings.DriverPickupArrival = DriverActualTimings.DriverPickupArrival == null ? DriverActualTimings.DriverPickupArrival : Configurations.ConvertDateTime(Convert.ToDateTime(DriverActualTimings.DriverPickupArrival));
            DriverActualTimings.DriverPickupDeparture = DriverActualTimings.DriverPickupDeparture == null ? DriverActualTimings.DriverPickupDeparture : Configurations.ConvertDateTime(Convert.ToDateTime(DriverActualTimings.DriverPickupDeparture));
            DriverActualTimings.DriverDeliveryArrival = DriverActualTimings.DriverDeliveryArrival == null ? DriverActualTimings.DriverDeliveryArrival : Configurations.ConvertDateTime(Convert.ToDateTime(DriverActualTimings.DriverDeliveryArrival));
            DriverActualTimings.DriverDeliveryDeparture = DriverActualTimings.DriverDeliveryDeparture == null ? DriverActualTimings.DriverDeliveryDeparture : Configurations.ConvertDateTime(Convert.ToDateTime(DriverActualTimings.DriverDeliveryDeparture));
            DriverActualTimings.DepartureDate = DriverActualTimings.DepartureDate == null ? DriverActualTimings.DepartureDate : Configurations.ConvertDateTime(Convert.ToDateTime(DriverActualTimings.DepartureDate));

            DriverActualTimings.DriverFumigationIn = DriverActualTimings.DriverFumigationIn == null ? DriverActualTimings.DriverFumigationIn : Configurations.ConvertDateTime(Convert.ToDateTime(DriverActualTimings.DriverFumigationIn));
            DriverActualTimings.DriverLoadingStartTime = DriverActualTimings.DriverLoadingStartTime == null ? DriverActualTimings.DriverLoadingStartTime : Configurations.ConvertDateTime(Convert.ToDateTime(DriverActualTimings.DriverLoadingStartTime));
            DriverActualTimings.DriverLoadingFinishTime = DriverActualTimings.DriverLoadingFinishTime == null ? DriverActualTimings.DriverLoadingFinishTime : Configurations.ConvertDateTime(Convert.ToDateTime(DriverActualTimings.DriverLoadingFinishTime));
            DriverActualTimings.DriverFumigationRelease = DriverActualTimings.DriverFumigationRelease == null ? DriverActualTimings.DriverFumigationRelease : Configurations.ConvertDateTime(Convert.ToDateTime(DriverActualTimings.DriverFumigationRelease));
            return DriverActualTimings;
        }
        #endregion

        #region Update Fumigation Signature and Name
        /// <summary>
        /// Update Signature and Name
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool UpdateFumigationSignaturePadDetail(SaveFumigationDetailsDTO dto)
        {
            return iDriverModuleDAL.UpdateFumigationSignaturePadDetail(dto);
        }
        #endregion

        #region Select Fumigation Signature and Name
        /// <summary>
        /// Seidlect Signature and Name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SelectFumigationSignaturePadDetail(int fumigationRoutId)
        {
            return iDriverModuleDAL.SelectFumigationSignaturePadDetail(fumigationRoutId);
        }
        #endregion


        #region  Delete fumigation proof of temprature
        /// <summary>
        ///  Delete fumigation proof of temprature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public bool DeleteFumigationProofOfTemprature(FumigationProofOfTempEditBind model)
        {
            return iDriverModuleDAL.DeleteFumigationProofOfTemprature(model);
        }
        #endregion

        #region Delete fumigation Damage Files
        /// <summary>
        /// Delete fumigation Damage Files
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteFumigationDamageFiles(FumigationDamagedEditBindDto model)
        {
            return iDriverModuleDAL.DeleteFumigationDamageFiles(model);
        }

        #endregion

        #region GPS Tracker 
        /// <summary>
        /// GPS Tracker 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public bool SaveFumigationGPSTracker(SaveFumigationGpsTrackingHistoryDTO dto)
        {
            return iDriverModuleDAL.SaveFumigationGPSTracker(dto);
        }
        #endregion

        #region fumigation status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetFumigationStatusList()
        {
            return iDriverModuleDAL.GetFumigationStatusList();
        }

        #endregion

        #region Save web-Camera 
        /// <summary>
        /// Save web-Camera
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public bool saveWebCamera(SaveWebCameraDTO dto)
        {
            return iDriverModuleDAL.saveWebCamera(dto);
        }
        #endregion

        #region Save Damage web-Camera  
        /// <summary>
        /// Save Damage web-Camera  
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public bool SaveDamageWebCamera(SaveDamageWebCamDTO dto)
        {
            return iDriverModuleDAL.SaveDamageWebCamera(dto);
        }
        #endregion

        #region Save Fumigation Waiting Time
        /// <summary>
        /// Save Fumigation Waiting Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool SaveFumigationWaitingTime(SaveFumigationWaitingNotifiDto dto)
        {
            return iDriverModuleDAL.SaveFumigationWaitingTime(dto);
        }
        #endregion
        #region check Status
        /// <summary>
        /// Check Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public bool IsStatusExist(int statusId, int fumigationId)
        {
            return iDriverModuleDAL.IsStatusExist(statusId, fumigationId);
        }

        #endregion

        #region  Get Last Status
        /// <summary>
        /// Get Last Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public int GetLastStatus(int statusId, int fumigationId)
        {
            return iDriverModuleDAL.GetLastStatus(statusId, fumigationId);
        }

        #endregion

        #region  Get Fumigation Last Status
        /// <summary>
        /// Get Fumigation Last Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public int GetShipmentLastStatus(int statusId, int shipmentId)
        {
            return iDriverModuleDAL.GetShipmentLastStatus(statusId, shipmentId);
        }

        #endregion

        public List<FumigationRoutesDTO> ValidateReuiredField(int statusId, int fumigationId)
        {
            return iDriverModuleDAL.ValidateReuiredField(statusId, fumigationId);
        }
        #endregion

        #region GetDriverLanguage
        /// <summary>
        /// Get Driver Language
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetDriverLanguage(int userId)
        {
            return iDriverModuleDAL.GetDriverLanguage(userId);
        }
        #endregion

        #region Get fumigation commment deail
        /// <summary>
        /// Get shipment commment deail
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        public List<FumigationCommentDTO> GetFumigatonComments(int fumigationId)
        {
            return iDriverModuleDAL.GetFumigatonComments(fumigationId);
        }
        #endregion
    }
}


