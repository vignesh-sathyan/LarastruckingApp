using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class ShipmentDAL : IShipmentDAL
    {
        #region private member
        readonly IShipmentRepository shipmentRepository = null;
        #endregion

        #region constructor
        public ShipmentDAL(IShipmentRepository iShipmentRepository)
        {
            shipmentRepository = iShipmentRepository;
        }


        #endregion

        #region shipment status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            return shipmentRepository.GetStatusList();
        }
        #endregion

        #region shipment sub status
        /// <summary>
        /// get sub status list
        /// </summary>
        /// <param name="statusid"></param>
        /// <returns></returns>
        public List<ShipmentSubStatusDTO> GetSubStatusList(int statusid)
        {
            return shipmentRepository.GetSubStatusList(statusid);
        }
        #endregion

        #region get equipmentlist
        /// <summary>
        /// get equipment list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentEquipmentDTO> EquipmnetList(ValidateDriverNEquipmentDTO model)
        {
            return shipmentRepository.EquipmnetList(model);
        }
        #endregion

        #region get driver list
        /// <summary>
        /// get driver list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentDriverDTO> DriverList(ValidateDriverNEquipmentDTO entity)
        {
            return shipmentRepository.DriverList(entity);
        }
        #endregion

        #region get all driver list
        /// <summary>
        /// et all driver list 
        /// </summary>
        /// <returns></returns>
        public List<ShipmentDriverDTO> DriverList()
        {
            return shipmentRepository.DriverList();
        }
        #endregion

        #region get accessorial fee type
        /// <summary>
        /// get accessorail fee type
        /// </summary>
        /// <returns></returns>
        public List<ShipmentAccessorialFeeTypeDTO> GetShipmentAccessorialFeeType()
        {
            return shipmentRepository.GetShipmentAccessorialFeeType();
        }


        #endregion

        #region create shipment
        /// <summary>
        ///  Create Shipment
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ShipmentDTO Add(ShipmentDTO entity)
        {
            return shipmentRepository.Add(entity);
        }
        #endregion

        #region get freight type
        /// <summary>
        /// Get Freight type
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IList<GetFreightTypeDTO> getFreightType(CustomernNRouteInfoDTO entity)
        {
            return shipmentRepository.getFreightType(entity);
        }
        #endregion

        #region get pricing method
        /// <summary>
        /// Get pricing method
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IList<GetPricingMethodDTO> getPricingMethod(CustomernNRouteInfoDTO entity)
        {
            return shipmentRepository.getPricingMethod(entity);
        }
        #endregion

        #region validate route
        /// <summary>
        /// #validate route stops
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ValidateRouteStop(ValidateRouteStopDTO enity)
        {
            return shipmentRepository.ValidateRouteStop(enity);
        }
        #endregion

        #region view shipment list
        /// <summary>
        /// View shipment list
        /// </summary>
        /// <returns></returns>
        public async Task<List<AllShipmentList>> ViewShipmentList(AllShipmentDTO entity)
        {
            return await shipmentRepository.ViewShipmentList(entity);
        }


        #endregion

        #region view all shipment list
        /// <summary>
        /// View all shipment list
        /// </summary>
        /// <returns></returns>
        public List<ViewShipmentListDTO> ViewAllShipmentList(ViewShipmentDTO entity, out int recordsTotal)
        {
            return shipmentRepository.ViewAllShipmentList(entity, out recordsTotal);
        }


        public IList<AllShipmentList> AllShipmentList(AllShipmentDTO entity)
        {
            return shipmentRepository.AllShipmentList(entity);
        }
        #endregion

        #region get shipment 
        /// <summary>
        ///  get shipment by id for edit shipment
        /// </summary>
        /// <returns></returns>
        public GetShipmentDTO GetShipmentById(int shipmentId)
        {
            return shipmentRepository.GetShipmentById(shipmentId);
        }


        #endregion

        #region Upload Damage Document
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        public int UploadDamageDocument(List<GetDamageImages> damageImageList)
        {
            return shipmentRepository.UploadDamageDocument(damageImageList);
        }
        #endregion

        #region Upload proof of temp
        /// <summary>
        ///Upload proof of temp
        /// </summary>
        /// <returns></returns>
        public int UploadProofofTempDocument(List<GetProofOfTemprature> proofofTempraturesList)
        {
            return shipmentRepository.UploadProofofTempDocument(proofofTempraturesList);
        }

        #endregion

        #region edit shipment
        /// <summary>
        /// Edit shipment
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public GetShipmentDTO EditShipment(GetShipmentDTO entity)
        {
            return shipmentRepository.EditShipment(entity);
        }


        #endregion

        #region Add Route Stops
        /// <summary>
        /// add route stops
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public GetShipmentRouteStopDTO AddRouteStops(GetShipmentRouteStopDTO model)
        {
            return shipmentRepository.AddRouteStops(model);
        }
        #endregion

        #region Add Freight Detail
        /// <summary>
        /// Add freight detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public GetShipmentFreightDetailDTO AddFreightDetail(GetShipmentFreightDetailDTO model)
        {
            return shipmentRepository.AddFreightDetail(model);
        }
        #endregion

        #region Delete Proof of Temprature
        /// <summary>
        /// Delete proof of temprature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteProofOfTemprature(GetProofOfTemprature model)
        {
            return shipmentRepository.DeleteProofOfTemprature(model);
        }
        #endregion

        #region Delete Damage File
        /// <summary>
        /// Delete damage file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteDamageFile(GetDamageImages model)
        {
            return shipmentRepository.DeleteDamageFile(model);
        }
        #endregion

        #region Delete shipment
        /// <summary>
        ///Delete shipment By Id
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public bool DeleteShipment(ShipmentDTO entity)
        {
            return shipmentRepository.DeleteShipment(entity);

        }
        #endregion

        #region Approved shipment
        /// <summary>
        /// Approved shipment if shipment on hold
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ShipmentDTO ApprovedShipment(ShipmentDTO entity)
        {
            return shipmentRepository.ApprovedShipment(entity);
        }


        #endregion

        #region bind  freight type
        /// <summary>
        /// bind Freight type
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IList<GetFreightTypeDTO> bindFreightType()
        {
            return shipmentRepository.bindFreightType();
        }


        #region bind  ShipmenetIsReady
        /// <summary>
        /// bind ShipmenetIsReady
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ShipmentIsReady(int shipmentId, bool ready)
        {
            return shipmentRepository.ShipmentIsReady(shipmentId, ready);
        }


        #endregion

        #region get customerDetail
        /// <summary>
        /// Get Customer Detail
        /// </summary>
        /// <param name="customerShipmentDTO"></param>
        /// <returns></returns>
        public ShipmentEmailDTO GetCustomerDetail(ShipmentEmailDTO customerShipmentDTO)
        {
            return shipmentRepository.GetCustomerDetail(customerShipmentDTO);
        }
        #endregion

        #region shipment fumigation proof of delivery
        /// <summary>
        /// Get proof Of delivery
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public List<GetShipmentRouteStopDTO> ShipmentProofOfDelivery(string shipmentId)
        {
            return shipmentRepository.ShipmentProofOfDelivery(shipmentId);
        }


        #endregion

        #region Approve Damage Image
        /// <summary>
        /// Approve Damage Image
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ApprovedDamageImage(GetDamageImages entity)
        {
            return shipmentRepository.ApprovedDamageImage(entity);
        }
        #endregion

        #region Approve Proof Of Temprature
        /// <summary>
        /// Approve Proof Of Temprature
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ApprovedProofOFTemp(GetProofOfTemprature entity)
        {
            return shipmentRepository.ApprovedProofOFTemp(entity);
        }
        #endregion

        #region validate equipment and driver
        /// <summary>
        ///validate equipment and driver
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MatchEquipmentNDriverDTO> ValidateEquipmentNDriver(ValidateDriverNEquipmentDTO model)
        {
            return shipmentRepository.ValidateEquipmentNDriver(model);
        }


        #endregion
        #region validate  driver
        /// <summary>
        ///validate driver
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MatchEquipmentNDriverDTO> ValidateDriver(ValidateDriverNEquipmentDTO model)
        {
            return shipmentRepository.ValidateDriver(model);
        }
        #endregion

        #region validate equipment 
        /// <summary>
        ///validate equipment 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MatchEquipmentNDriverDTO> ValidateEquipment(ValidateDriverNEquipmentDTO model)
        {
            return shipmentRepository.ValidateEquipment(model);
        }
        #endregion

        #region Copy Shipment
        /// <summary>
        /// Copy shipment detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveCopyShipmentDetail(CopyShipmentDTO entity)
        {
            return shipmentRepository.SaveCopyShipmentDetail(entity);
        }
        #endregion


        #region Get Shipment Detail By Id
        /// <summary>
        ///  Get Shipment Detail By Id
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public CopyShipmentDTO GetCopyShipmentDetailById(int shipmentId)
        {
            return shipmentRepository.GetCopyShipmentDetailById(shipmentId);
        }
        #endregion


        #region Get max route no.
        /// <summary>
        /// Get max route no.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>

        public int? GetMaxRouteNo(int shipmentId)
        {
            return shipmentRepository.GetMaxRouteNo(shipmentId);

        }

        #endregion


        #region Get Driver Check In time
        /// <summary>
        /// Get Driver Check In time
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public DateTime? GetCheckInTime(int driverId)
        {
            return shipmentRepository.GetCheckInTime(driverId);
        }
        #endregion
        public string DriverPhone(int driverid)
        {
            return shipmentRepository.DriverPhone(driverid);
        }

        public string CustomerName(int CustomerID)
        {
            return shipmentRepository.CustomerName(CustomerID);
        }

        public TemperatureEmailSipmentDTO GetTemperatureEmailDetail(int shipmentId)
        {
            return shipmentRepository.GetTemperatureEmailDetail(shipmentId);
        }
    }
}
#endregion