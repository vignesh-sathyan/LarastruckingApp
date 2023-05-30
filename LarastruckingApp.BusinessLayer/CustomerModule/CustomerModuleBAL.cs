using LarastruckingApp.DAL.CustomerModule;
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

namespace LarastruckingApp.BusinessLayer.CustomerModule
{
    public class CustomerModuleBAL : ICustomerModuleBAL
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>

        private readonly ICustomerModuleDAL iCustomerDAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="IDriverModuleDAL"></param>

        public CustomerModuleBAL(ICustomerModuleDAL ICustomerDAL)
        {
            iCustomerDAL = ICustomerDAL;
        }
        #endregion

        #region Customer Dashboard => Shipment Details
        #region Get All Quotes Assigned to Customer
        /// <summary>
        /// Get All Quotes Assigned to Customer
        /// </summary>
        /// <returns></returns>
        public List<CustomerQuotesInfoDto> GetAllQuotes(DataTableFilterDto dto, int userId)
        {
            return iCustomerDAL.GetAllQuotes(dto, userId);
        }
        #endregion

        #region Get Multiple Routes by Shipement and Costomer
        /// <summary>
        /// Get Multiple Routes by Shipement and Costomer
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>

        public List<CustomerShipmentRoutesDto> GetCustomerShipmentRoutes(int shipmentId)
        {
            return iCustomerDAL.GetCustomerShipmentRoutes(shipmentId);
        }
        #endregion

        #region Get Customer Shipment Location Details
        /// <summary>
        /// Get Customer Shipment Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>


        public CustomerShipLocationDetailsDto GetCustomerShipmentRoutesDetails(int ShippingRoutesId)
        {
            return iCustomerDAL.GetCustomerShipmentRoutesDetails(ShippingRoutesId);
        }
        #endregion

        #region Shipment Track Records
        /// <summary>
        /// Shipment Track Records
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public CustomerShipmentTrackDto GetCustomerShipmentTrack(int shipmentId)
        {
            return iCustomerDAL.GetCustomerShipmentTrack(shipmentId);
        }
        #endregion

        #region Driver status

        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            return iCustomerDAL.GetStatusList();
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
            return iCustomerDAL.GetShipmentDamagedFiles(ShippingRoutesId);
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
            return iCustomerDAL.GetShipmentProofOfTempFiles(ShippingRoutesId, ShipmentFreightDetailId);
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
            return iCustomerDAL.GetShipmentFreightDetails(ShippingRoutesId);
        }

        #endregion

        #region Shipment  Get Customer Accessorial Charge
        /// <summary>
        /// Get Customer Accessorial Charge
        /// </summary>
        /// <param name="hipmentId"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public List<CustomerAccessorialCharges> GetCustomerAccessorialCharge(int shipmentId, int routeId)
        {
            return iCustomerDAL.GetCustomerAccessorialCharge(shipmentId,routeId);
        }
        #endregion

        #region Fumigation  Get Customer Accessorial Charge
        /// <summary>
        /// Get Customer Accessorial Charge
        /// </summary>
        /// <param name="hipmentId"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public List<CustomerAccessorialCharges> GetCustFumAccessorialCharge(int fumigationId, int routeId)
        {
            return iCustomerDAL.GetCustFumAccessorialCharge(fumigationId, routeId);
        }
        #endregion


        #region Get Freight Details By ID
        /// <summary>
        /// Get Freight Details By ID
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public ShipmentFreightDetailsDto GetFreightDetailsById(int ShippingRoutesId)
        {
            return iCustomerDAL.GetFreightDetailsById(ShippingRoutesId);
        }

        #endregion

        #region Update Freight Details
        /// <summary>
        /// Update Freight Details
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool UpdateFreightDetails(UpdateFreightDetailsDTO dto)
        {
            return iCustomerDAL.UpdateFreightDetails(dto);
        }
        #endregion

        #endregion
        #region Customer dashboard = > Fumigation details

        #region Get Multiple Routes by Fumigation and Costomer
        /// <summary>
        /// Get Multiple Routes by Fumigation and Costomer
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        public List<CustomerFumigationRoutesDto> GetCustomerFumigationRoutes(int FumigationId)
        {
            return iCustomerDAL.GetCustomerFumigationRoutes(FumigationId);
        }

        #endregion

        #region Get Customer Fumigation Location Details
        /// <summary>
        ///  Get Customer Fumigation Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        public CustomerFumigationLocationDetailsDto GetCustomerFumigationRoutesDetails(int FumigationRoutsId)
        {
            return iCustomerDAL.GetCustomerFumigationRoutesDetails(FumigationRoutsId);
        }
        #endregion

        #region Fumigation Track Records 
        /// <summary>
        /// Fumigation Track Records 
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        public CustomerFumigationTrackDto GetCustomerFumigationTrack(int fumigationRouteId, int FumigationId)
        {
            return iCustomerDAL.GetCustomerFumigationTrack(fumigationRouteId, FumigationId);
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
            return iCustomerDAL.GetFumigationDamagedFiles(FumigationRoutsId);
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
            return iCustomerDAL.GetFumigationProofOfTempFiles(FumigationRoutsId);
        }
        #endregion

        #region fumigation status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetFumigationStatusList()
        {
            return iCustomerDAL.GetFumigationStatusList();
        }

        #endregion

        #endregion

        #region Customer Dashboard => Old Shipment Details

        #region Get All Old shipment to Customer
        /// <summary>
        /// Get All Old shipment to Customer
        /// </summary>
        /// <returns></returns>
        public List<CustomerQuotesInfoDto> GetOldShipmentDetails(DataTableFilterDto dto, int userId)
        {
            return iCustomerDAL.GetOldShipmentDetails(dto, userId);
        }
        #endregion
        #endregion
    }
}
