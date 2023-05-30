using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.Driver_Fumigation;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL.DriverModule
{
    public class DriverModuleDAL : IDriverModuleDAL
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>

        private readonly IDriverModuleRepository _IDriverModuleRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="IDriverModuleDAL"></param>

        #region Section 1--> Driver Shipment
        public DriverModuleDAL(IDriverModuleRepository IDriverModuleRepository)
        {
            _IDriverModuleRepository = IDriverModuleRepository;
        }
        #endregion

        #region Get All Pre-Trip Shipment Details
        /// <summary>
        /// Get All Pre-Trip Shipment Details
        /// </summary>
        /// <returns></returns>

        public IList<PreTripShipmentDto> GetPreTripShipmentDetails(DataTableFilterDto dto, int userId)
        {
            return _IDriverModuleRepository.GetPreTripShipmentDetails(dto, userId);
        }
        #endregion

        #region Get Pre-Trip Check List
        /// <summary>
        /// Get Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        public DriverPreTripDto GetPreTripCheckList(int shipmentId)
        {
            return _IDriverModuleRepository.GetPreTripCheckList(shipmentId);
        }
        #endregion

        #region Save Pre-Trip Check List
        /// <summary>
        /// Save Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        public bool SavePreTripCheckList(DriverPreTripDto dto)
        {

            return _IDriverModuleRepository.SavePreTripCheckList(dto);
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

            return _IDriverModuleRepository.SaveProofOfTemperature(dto);
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

            return _IDriverModuleRepository.SaveDamagedFiles(dto);
        }
        #endregion

        #region Get Routes of Pre-Trip Shipment
        /// <summary>
        /// Get all the routes of Pre-Trip Shipment by shipment id
        /// </summary>
        /// <returns></returns>

        public List<ShipmentRoutesDto> GetShipmentRoutes(int shipmentId, long userID)
        {
            return _IDriverModuleRepository.GetShipmentRoutes(shipmentId, userID);
        }
        #endregion
        #region Get Shipment Location Details
        /// <summary>
        ///  Get Shipment Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public ShipmentLocationDetailsDto GetShipmentRoutesDetails(int ShippingRoutesId)
        {
            return _IDriverModuleRepository.GetShipmentRoutesDetails(ShippingRoutesId);
        }
        #endregion

        #region Save Pre-Trip Shipment Detail
        /// <summary>
        /// Save Pre-Trip Shipment Detail
        /// </summary>
        /// <returns></returns>
        public bool SavePreTripShipmentDetail(PreTripAddShipmentDetailDto dto, out bool isEmailNeedToSend)
        {

            return _IDriverModuleRepository.SavePreTripShipmentDetail(dto, out isEmailNeedToSend);
        }
        #endregion

        #region Get Shipment Freight Details
        /// <summary>
        ///  Get Shipment Freight Details By ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public List<ShipmentFreightDetailsDto> GetShipmentFreightDetails(int ShippingRoutesId)
        {
            return _IDriverModuleRepository.GetShipmentFreightDetails(ShippingRoutesId);
        }
        #endregion

        #region Get Pre-Trip Check Timings for Arrival & departure
        /// <summary>
        ///  Get Pre-Trip Check Timings for Arrival & departure
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public PreTripTimmingDetailsDto GetPreTripCheckTimings(int ShippingRoutesId)
        {
            return _IDriverModuleRepository.GetPreTripCheckTimings(ShippingRoutesId);
        }
        #endregion

        #region Select Shipment Routes Stops
        /// <summary>
        ///  Select Shipment Routes Stops
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public List<GetShipmentRouteStopDTO> GetShipmentRoutesStopDetail(int ShippingRoutesId)
        {
            return _IDriverModuleRepository.GetShipmentRoutesStopDetail(ShippingRoutesId);
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
            return _IDriverModuleRepository.GetShipmentComments(shipmentId);
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
            return _IDriverModuleRepository.GetShipmentDamagedFiles(ShippingRoutesId);
        }
        #endregion
        #region  Get shipment Proof of Temp 
        /// <summary>
        /// Get shipment Proof of Temp
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <param name="ShipmentFreightDetailId"></param>
        /// <returns></returns>

        public List<ShipmentProofOfTempEditBind> GetShipmentProofOfTempFiles(int ShippingRoutesId, int ShipmentFreightDetailId)
        {
            return _IDriverModuleRepository.GetShipmentProofOfTempFiles(ShippingRoutesId, ShipmentFreightDetailId);
        }
        #endregion
        #region  Delete proof of temprature
        /// <summary>
        ///  Delete proof of temprature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public bool DeleteProofOfTemprature(ShipmentProofOfTempEditBind model)
        {
            return _IDriverModuleRepository.DeleteProofOfTemprature(model);
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
            return _IDriverModuleRepository.DeleteDamageFiles(model);
        }

        #endregion

        #region Driver status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            return _IDriverModuleRepository.GetStatusList();
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
            return _IDriverModuleRepository.GetSubStatusList(statusid);
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
            return _IDriverModuleRepository.UpdateSignaturePadDetail(dto);
        }
        #endregion

        #region Select Signature and Name
        /// <summary>
        /// Select Signature and Name
        /// </summary>
        /// <param name="shipmentRouteId"></param>
        /// <returns></returns>
        public bool SelectSignaturePadDetail(int shipmentRouteId)
        {
            return _IDriverModuleRepository.SelectSignaturePadDetail(shipmentRouteId);
        }
        #endregion

        #region GPS Tracker 
        /// <summary>
        /// GPS Tracker 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public bool SaveGPSTracker(SaveGpsTrackingHistoryDto dto)
        {
            return _IDriverModuleRepository.SaveGPSTracker(dto);
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
            return _IDriverModuleRepository.saveShipmentWebCamera(dto);
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
            return _IDriverModuleRepository.saveShipmentDamageWebCamera(dto);
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
            return _IDriverModuleRepository.SaveWaitingTime(dto);
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
            return _IDriverModuleRepository.GetPreTripCheckFumigationList(FumigationId);
        }
        #endregion

        #region Save Fumigation Pre-Trip Check List
        /// <summary>
        /// Save Fumigation Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        public bool SaveFumigationPreTripCheckList(FumigationPreTripCheckUpDTO dto)
        {

            return _IDriverModuleRepository.SaveFumigationPreTripCheckList(dto);
        }
        #endregion

        #region Get Fumigation Routes of Pre-Trip Shipment
        /// <summary>
        /// Get all the Fumigation routes of Pre-Trip Shipment by shipment id
        /// </summary>
        /// <returns></returns>

        public List<FumigationRoutesDTO> GetFumigationRoutes(int FumigationId, long userID)
        {
            return _IDriverModuleRepository.GetFumigationRoutes(FumigationId, userID);
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
            return _IDriverModuleRepository.GetFumigationRoutesDetails(FumigationRoutsId);
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
            return _IDriverModuleRepository.GetFumigationFreightDetails(FumigationRoutsId);
        }
        #endregion

        #region Save Fumigation Detail
        /// <summary>
        ///  Save Fumigation Detail
        /// </summary>
        /// <returns></returns>
        public bool SaveFumigationtDetail(SaveFumigationDetailsDTO dto, out bool isEmailNeedToSend)
        {

            return _IDriverModuleRepository.SaveFumigationtDetail(dto, out isEmailNeedToSend);
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

            return _IDriverModuleRepository.SaveFumigationProofOfTemperature(dto);
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

            return _IDriverModuleRepository.SaveFumigationDamagedFiles(dto);
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
            return _IDriverModuleRepository.GetFumigationDamagedFiles(FumigationRoutsId);
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
            return _IDriverModuleRepository.GetFumigationProofOfTempFiles(FumigationRoutsId);
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
            return _IDriverModuleRepository.GetDriverActualTimings(FumigationRoutsId);
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
            return _IDriverModuleRepository.UpdateFumigationSignaturePadDetail(dto);
        }
        #endregion

        #region Select Fumigation Signature and Name
        /// <summary>
        /// Select Signature and Name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SelectFumigationSignaturePadDetail(int fumigationRoutId)
        {
            return _IDriverModuleRepository.SelectFumigationSignaturePadDetail(fumigationRoutId);
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
            return _IDriverModuleRepository.DeleteFumigationProofOfTemprature(model);
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
            return _IDriverModuleRepository.DeleteFumigationDamageFiles(model);
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
            return _IDriverModuleRepository.SaveFumigationGPSTracker(dto);
        }
        #endregion

        #region fumigation status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetFumigationStatusList()
        {
            return _IDriverModuleRepository.GetFumigationStatusList();
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
            return _IDriverModuleRepository.saveWebCamera(dto);
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
            return _IDriverModuleRepository.SaveDamageWebCamera(dto);
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
            return _IDriverModuleRepository.SaveFumigationWaitingTime(dto);
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
            return _IDriverModuleRepository.IsStatusExist(statusId, fumigationId);
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
            return _IDriverModuleRepository.GetLastStatus(statusId, fumigationId);
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
            return _IDriverModuleRepository.GetShipmentLastStatus(statusId, shipmentId);
        }

        #endregion
        public List<FumigationRoutesDTO> ValidateReuiredField(int statusId, int fumigationId)
        {
            return _IDriverModuleRepository.ValidateReuiredField(statusId, fumigationId);
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
            return _IDriverModuleRepository.GetDriverLanguage(userId);
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
            return _IDriverModuleRepository.GetFumigatonComments(fumigationId);
        }
        #endregion
    }
}
