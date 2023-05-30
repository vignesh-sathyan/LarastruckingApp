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

namespace LarastruckingApp.DAL.DriverModule
{
    public interface IDriverModuleDAL
    {
        #region Section 1 --> Driver Shipment
        #region Interface: Get All Pre-Trip Shipment Details
        IList<PreTripShipmentDto> GetPreTripShipmentDetails(DataTableFilterDto dto, int userId);
        #endregion
        
        #region Get Pre-Trip Check List
        /// <summary>
        /// Get Pre-Trip Check List
        /// </summary>
        /// <returns></returns>

        DriverPreTripDto GetPreTripCheckList(int shipmentId);
        #endregion

        #region Save Pre-Trip Check List
        /// <summary>
        /// Save Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        bool SavePreTripCheckList(DriverPreTripDto dto);
        #endregion

        #region Get Routes of Pre-Trip Shipment
        /// <summary>
        /// Get all the routes of Pre-Trip Shipment by shipment id
        /// </summary>
        /// <returns></returns>

        List<ShipmentRoutesDto> GetShipmentRoutes(int shipmentId, long userID);
        #endregion

        #region Get Shipment Location Details
        /// <summary>
        ///  Get Shipment Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        ShipmentLocationDetailsDto GetShipmentRoutesDetails(int ShippingRoutesId);

        #endregion

        #region  Save Pre-Trip Shipment Detail
        /// <summary>
        /// Save Pre-Trip Shipment Detail
        /// </summary>
        /// <returns></returns>
        bool SavePreTripShipmentDetail(PreTripAddShipmentDetailDto dto, out bool isEmailNeedToSend);
        #endregion

        #region Save Proof of Temperature
        /// <summary>
        /// Save Proof of Temperature
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool SaveProofOfTemperature(PreTripAddShipmentDetailDto dto);
        #endregion

        #region Save Damaged Files 
        /// <summary>
        /// Save Damaged Files
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool SaveDamagedFiles(PreTripAddShipmentDetailDto dto);
        #endregion

        #region Get Shipment Freight Details
        /// <summary>
        /// Get Shipment Freight Details By ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        List<ShipmentFreightDetailsDto> GetShipmentFreightDetails(int ShippingRoutesId);
        #endregion

        #region Get Pre-Trip Check Timings for Arrival & departure
        /// <summary>
        /// Get Pre-Trip Check Timings for Arrival & departure
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        PreTripTimmingDetailsDto GetPreTripCheckTimings(int ShippingRoutesId);

        #endregion

        #region Select Shipment Routes Stops
        /// <summary>
        /// Select Shipment Routes Stops
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        List<GetShipmentRouteStopDTO> GetShipmentRoutesStopDetail(int ShippingRoutesId);

        #endregion

        #region  Get Shipment Damaged Files
        /// <summary>
        /// Get Shipment Damaged Files By ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>

        List<ShipmentDamagedEditBindDto> GetShipmentDamagedFiles(int ShippingRoutesId);
        #endregion
        #region Get shipment Proof of Temp
        /// <summary>
        /// Get shipment Proof of Temp
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <param name="ShipmentFreightDetailId"></param>
        /// <returns></returns>
        List<ShipmentProofOfTempEditBind> GetShipmentProofOfTempFiles(int ShippingRoutesId, int ShipmentFreightDetailId);
        #endregion
        #region Delete proof of temprature
        /// <summary>
        ///  Delete proof of temprature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool DeleteProofOfTemprature(ShipmentProofOfTempEditBind model);

        #endregion
        #region Delete Damage Files
        /// <summary>
        ///  Delete Damage Files
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool DeleteDamageFiles(ShipmentDamagedEditBindDto model);
        #endregion

        #region Driver status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        List<ShipmentStatusDTO> GetStatusList();

        #endregion

        #region  Driver sub status 
        /// <summary>
        /// get sub status list
        /// </summary>
        /// <param name="statusid"></param>
        /// <returns></returns>
        List<ShipmentSubStatusDTO> GetSubStatusList(int statusid);
        #endregion

        #region Update Signature and Name
        /// <summary>
        /// Update Signature and Name
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool UpdateSignaturePadDetail(PreTripAddShipmentDetailDto dto);
        #endregion

        #region Select Signature and Name
        /// <summary>
        /// Select Signature and Name
        /// </summary>
        /// <param name="shipmentRouteId"></param>
        /// <returns></returns>
        bool SelectSignaturePadDetail(int shipmentRouteId);
        #endregion



        #region GPS Tracker 
        /// <summary>
        /// GPS Tracker 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool SaveGPSTracker(SaveGpsTrackingHistoryDto dto);
        #endregion

        #region Save Shipment web-Camera   
        /// <summary>
        /// Save web-Camera  
        /// </summary> 
        /// <param name="dto"></param>
        /// <returns></returns>
        bool saveShipmentWebCamera(SaveShipmentWebCameraDTO dto);
        #endregion

        #region Save Shipment Damage web-Camera    
        /// <summary>
        /// Save Damage web-Camera  
        /// </summary> 
        /// <param name="dto"></param>
        /// <returns></returns>
        bool saveShipmentDamageWebCamera(SaveShipmentDamageWebCamDTO dto);
        #endregion

        #region Save Shipment Waiting Time
        /// <summary>
        /// Save Shipment Waiting Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool SaveWaitingTime(SaveShipmentWaitingNotifiDto dto);
        #endregion


        #region Get shipment commment deail
        /// <summary>
        /// Get shipment commment deail
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        List<ShipmentCommentDTO> GetShipmentComments(int shipmentId);
        #endregion

        int GetDriverLanguage(int userId);
        #endregion

        #region Section 2 --> Driver Fumigation 

        #region Get Fumigation Pre-Trip Check List
        /// <summary>
        /// Get Fumigation Pre-Trip Check List
        /// </summary>
        /// <returns></returns>

        FumigationPreTripCheckUpDTO GetPreTripCheckFumigationList(int FumigationId);
        #endregion


        #region Save Fumigation Pre-Trip Check List
        /// <summary>
        /// Save Fumigation Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        bool SaveFumigationPreTripCheckList(FumigationPreTripCheckUpDTO dto);
        #endregion

        #region Get Fumigation Routes of Pre-Trip Shipment
        /// <summary>
        /// Get all the Fumigation routes of Pre-Trip Shipment by shipment id
        /// </summary>
        /// <returns></returns>

        List<FumigationRoutesDTO> GetFumigationRoutes(int FumigationId , long userID);
        #endregion

        #region Get Fumigation Location Details
        /// <summary>
        ///  Get Fumigation Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        FumigationLocationDetailsDTO GetFumigationRoutesDetails(int FumigationRoutsId);

        #endregion

        #region Get Fumigation Freight Details
        /// <summary>
        ///  Get Fumigation Freight Details By ShippingRoutes Id
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        List<FumigationFreightDetailsDto> GetFumigationFreightDetails(int FumigationRoutsId);
        #endregion

        #region Save Fumigation Detail
        /// <summary>
        /// Save Fumigation Detail
        /// </summary>
        /// <returns></returns>
        bool SaveFumigationtDetail(SaveFumigationDetailsDTO dto, out bool isEmailNeedToSend);
        #endregion

        #region Save Fumigation Proof of Temperature
        /// <summary>
        /// Save Fumigation Proof of Temperature
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool SaveFumigationProofOfTemperature(SaveFumigationDetailsDTO dto);
        #endregion

        #region Save Fumigation Damaged Files 
        /// <summary>
        /// Save Fumigation Damaged Files
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool SaveFumigationDamagedFiles(SaveFumigationDetailsDTO dto);
        #endregion


        #region Get Fumigation Damaged Files
        /// <summary>
        /// Get Fumigation Damaged Files By ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        List<FumigationDamagedEditBindDto> GetFumigationDamagedFiles(int FumigationRoutsId);
        #endregion

        #region  Get Fumigation Proof of Temp 
        /// <summary>
        ///  Get Fumigation Proof of Temp
        /// </summary>
        /// <param name="FumigationRoutsId"></param>

        /// <returns></returns>
        List<FumigationProofOfTempEditBind> GetFumigationProofOfTempFiles(int FumigationRoutsId);
        #endregion

        #region Get Driver Actual Timings for Arrival & departure
        /// <summary>
        ///   Get Driver Actual Timings for Arrival & departure
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        DriverActualTimmingsDTO GetDriverActualTimings(int FumigationRoutsId);
        #endregion

        #region Update Fumigation Signature and Name 
        /// <summary>
        /// Update Fumigation Signature And Name
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool UpdateFumigationSignaturePadDetail(SaveFumigationDetailsDTO dto);
        #endregion

        #region Select Fumigation Signature and Name 
        /// <summary>
        /// Select Fumigation Signature And Name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool SelectFumigationSignaturePadDetail(int id);
        #endregion

        #region  Delete Fumigation proof of temprature
        /// <summary>
        ///  Delete Fumigation proof of temprature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool DeleteFumigationProofOfTemprature(FumigationProofOfTempEditBind model);
        #endregion

        #region Delete Damage Files
        /// <summary>
        /// Delete Fumigation Damage Files
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        bool DeleteFumigationDamageFiles(FumigationDamagedEditBindDto model);
        #endregion

        #region GPS Tracker 
        /// <summary>
        /// GPS Tracker 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool SaveFumigationGPSTracker(SaveFumigationGpsTrackingHistoryDTO dto);
        #endregion

        #region Get Fumigation Status 
        List<ShipmentStatusDTO> GetFumigationStatusList();
        #endregion

        #region Save web-Camera  
         /// <summary>
         /// Save web-Camera  
         /// </summary> 
         /// <param name="dto"></param>
         /// <returns></returns>
         bool saveWebCamera(SaveWebCameraDTO dto);
        #endregion

        #region Save Damage web-Camera   
        /// <summary>
        /// Save Damage web-Camera  
        /// </summary> 
        /// <param name="dto"></param>
        /// <returns></returns>
        bool SaveDamageWebCamera(SaveDamageWebCamDTO dto);
        #endregion

        #region Save Fumigation Waiting Time
        /// <summary>
        /// Save Fumigation Waiting Time
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool SaveFumigationWaitingTime(SaveFumigationWaitingNotifiDto dto);
        #endregion

        #region check Status
        /// <summary>
        /// Check Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>

        bool IsStatusExist(int statusId, int fumigationId);
        #endregion
        int GetLastStatus(int statusId, int fumigationId);

        #region Get Fumigation Last Status
        /// <summary>
        ///Get fumigation Last Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        int GetShipmentLastStatus(int statusId, int shipmentId);
        #endregion
        List<FumigationRoutesDTO> ValidateReuiredField(int statusId, int fumigationId);
        List<FumigationCommentDTO> GetFumigatonComments(int fumigationId);
        #endregion
    }
}
