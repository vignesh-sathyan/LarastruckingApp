using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.CustomerFumigation;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.Driver_Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL.CustomerModule
{
    public interface ICustomerModuleDAL
    {
        #region Get All Quotes Assigned to Customer
        /// <summary>
        /// Get All Quotes Assigned to Customer
        /// </summary>
        /// <returns></returns>
        List<CustomerQuotesInfoDto> GetAllQuotes(DataTableFilterDto dto, int userId);
        #endregion

        #region Get Multiple Routes by Shipement and Costomer
        /// <summary>
        /// Get Multiple Routes by Shipement and Costomer
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        List<CustomerShipmentRoutesDto> GetCustomerShipmentRoutes(int shipmentId);
        #endregion

        #region Get Customer Shipment Location Details
        /// <summary>
        ///  Get Customer Shipment Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        CustomerShipLocationDetailsDto GetCustomerShipmentRoutesDetails(int ShippingRoutesId);

        #endregion

        #region Shipment Track Records 
        /// <summary>
        /// Shipment Track Records 
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        CustomerShipmentTrackDto GetCustomerShipmentTrack(int shipmentId);
        #endregion

        #region Driver status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        List<ShipmentStatusDTO> GetStatusList();

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

        #region Get Shipment Freight Details
        /// <summary>
        ///  Get Shipment Freight Details By ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        List<ShipmentFreightDetailsDto> GetShipmentFreightDetails(int ShippingRoutesId);
        #endregion

        #region Get Freight Details By ID
        /// <summary>
        /// Get Freight Details By ID
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        ShipmentFreightDetailsDto GetFreightDetailsById(int ShippingRoutesId);
        #endregion

        #region Update Freight Details 
        /// <summary>
        /// Update Freight Details 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool UpdateFreightDetails(UpdateFreightDetailsDTO dto);
        #endregion


        #region Get Customer Fumigation Location Details
        /// <summary>
        /// Get Customer Fumigation Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        List<CustomerFumigationRoutesDto> GetCustomerFumigationRoutes(int FumigationId);
        #endregion

        #region Get Fumigation Location Details
        /// <summary>
        /// Get Customer Fumigation Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        CustomerFumigationLocationDetailsDto GetCustomerFumigationRoutesDetails(int FumigationRoutsId);
        #endregion

        #region Fumigation Track Records 
        /// <summary>
        /// Fumigation Track Records 
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        CustomerFumigationTrackDto GetCustomerFumigationTrack(int fumigationRouteId, int FumigationId);
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

        #region Get Fumigation Status 
        List<ShipmentStatusDTO> GetFumigationStatusList();
        #endregion

        #region Get All Old shipment to Customer
        /// <summary>
        /// Get All Old shipment to Customer
        /// </summary>
        /// <returns></returns>
        List<CustomerQuotesInfoDto> GetOldShipmentDetails(DataTableFilterDto dto, int userId);
        #endregion

        #region  Get Customer Accessorial Charge
        /// <summary>
        /// Get Customer Accessorial Charge
        /// </summary>
        /// <param name="hipmentId"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        List<CustomerAccessorialCharges> GetCustomerAccessorialCharge(int shipmentId, int routeId);
        #endregion

        #region Fumigation Get Customer Accessorial Charge
        /// <summary>
        /// Get Customer Accessorial Charge
        /// </summary>
        /// <param name="hipmentId"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        List<CustomerAccessorialCharges> GetCustFumAccessorialCharge(int fumigationId, int routeId);
        #endregion
    }
}
