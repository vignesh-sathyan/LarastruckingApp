using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Utility.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class ShipmentBAL : IShipmentBAL
    {
        #region private member
        readonly IShipmentDAL shipmentDAL = null;
        #endregion

        #region constructor
        public ShipmentBAL(IShipmentDAL iShipmentDAL)
        {
            shipmentDAL = iShipmentDAL;
        }

        #endregion

        #region shipment status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            return shipmentDAL.GetStatusList();
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
            return shipmentDAL.GetSubStatusList(statusid);
        }
        #endregion

        #region get equipmentlist
        /// <summary>
        /// get equipment list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentEquipmentDTO> EquipmnetList(ValidateDriverNEquipmentDTO model)
        {
            if (model != null)
            {
                model.FirstPickupArrivalDate = (model.FirstPickupArrivalDate == null ? model.FirstPickupArrivalDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.FirstPickupArrivalDate)));
                model.LastPickupArrivalDate = (model.LastPickupArrivalDate == null ? model.LastPickupArrivalDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.LastPickupArrivalDate)));
            }
            return shipmentDAL.EquipmnetList(model);
        }
        #endregion

        #region get driver list
        /// <summary>
        /// get driver list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentDriverDTO> DriverList(ValidateDriverNEquipmentDTO entity)
        {
            entity.FirstPickupArrivalDate = entity.FirstPickupArrivalDate == null ? entity.FirstPickupArrivalDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.FirstPickupArrivalDate));
            entity.LastPickupArrivalDate = entity.LastPickupArrivalDate == null ? entity.LastPickupArrivalDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.LastPickupArrivalDate));
            return shipmentDAL.DriverList(entity);
        }
        #endregion

        #region get all driver list
        /// <summary>
        /// et all driver list 
        /// </summary>
        /// <returns></returns>
        public List<ShipmentDriverDTO> DriverList()
        {
            return shipmentDAL.DriverList();
        }
        #endregion


        #region get accessorial fee type
        /// <summary>
        /// get accessorail fee type
        /// </summary>
        /// <returns></returns>
        public List<ShipmentAccessorialFeeTypeDTO> GetShipmentAccessorialFeeType()
        {
            return shipmentDAL.GetShipmentAccessorialFeeType();
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
            if (entity.ShipmentRoutesStop != null)
            {
                foreach (var routestop in entity.ShipmentRoutesStop)
                {
                    routestop.PickDateTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(routestop.PickDateTime));
                    routestop.PickDateTimeTo = Configurations.ConvertLocalToUTC(Convert.ToDateTime(routestop.PickDateTimeTo));
                    routestop.DeliveryDateTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(routestop.DeliveryDateTime));
                    routestop.DeliveryDateTimeTo = Configurations.ConvertLocalToUTC(Convert.ToDateTime(routestop.DeliveryDateTimeTo));
                }
            }
            if (entity.ShipmentEquipmentNdriver != null)
            {
                foreach (var equipmentNDriver in entity.ShipmentEquipmentNdriver)
                {
                    equipmentNDriver.DriverId = (equipmentNDriver.DriverId == 0 ? null : equipmentNDriver.DriverId);
                    equipmentNDriver.EquipmentId = (equipmentNDriver.EquipmentId == 0 ? null : equipmentNDriver.EquipmentId);
                }
            }
            if (entity.ShipmentFreightDetail != null)
            {
                foreach (var freight in entity.ShipmentFreightDetail)
                {
                    if(freight.IsPartialShipment==false)
                    {
                        freight.PartialBox = freight.NoOfBox;
                        freight.PartialPallet = Convert.ToInt32(freight.QutWgtVlm);
                    }
                    if (freight.TemperatureType == "C" && freight.Temperature != null)
                    {
                        freight.Temperature = ConversionFormula.NullCelsiusToFahrenheit(freight.Temperature);
                        freight.TemperatureType = "F";
                    }
                }
            }

            return shipmentDAL.Add(entity);
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
            if (entity != null && entity.PickupArrivalDate != null)
            {
                entity.PickupArrivalDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.PickupArrivalDate));
            }

            return shipmentDAL.getFreightType(entity);
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
            if (entity != null && entity.PickupArrivalDate != null)
            {
                entity.PickupArrivalDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.PickupArrivalDate));
            }
            return shipmentDAL.getPricingMethod(entity);
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
            enity.ArrivalDate = Configurations.ConvertLocalToUTC(enity.ArrivalDate);
            return shipmentDAL.ValidateRouteStop(enity);


        }

        #endregion

        #region view shipment list
        /// <summary>
        /// View shipment list
        /// </summary>
        /// <returns></returns>
        public async Task<List<AllShipmentList>> ViewShipmentList(AllShipmentDTO entity)
        {
           
            return await shipmentDAL.ViewShipmentList(entity);
        }

        #endregion

        #region all view shipment list
        /// <summary>
        /// View all shipment list
        /// </summary>
        /// <returns></returns>
        public List<ViewShipmentListDTO> ViewAllShipmentList(ViewShipmentDTO entity, out int recordsTotal)
        {          
            return shipmentDAL.ViewAllShipmentList(entity, out recordsTotal);
        }
        public IList<AllShipmentList> AllShipmentList(AllShipmentDTO entity)
        {
            return shipmentDAL.AllShipmentList(entity);
        }
        #endregion

        #region get shipment 
        /// <summary>
        ///  get shipment by id for edit shipment
        /// </summary>
        /// <returns></returns>
        public GetShipmentDTO GetShipmentById(int shipmentId)
        {
            return shipmentDAL.GetShipmentById(shipmentId);
        }


        #endregion

        #region Upload Damage Document
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        public int UploadDamageDocument(List<GetDamageImages> damageImageList)
        {
            return shipmentDAL.UploadDamageDocument(damageImageList);
        }

        #endregion

        #region Upload Damage Document
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        public int UploadProofofTempDocument(List<GetProofOfTemprature> proofofTempraturesList)
        {
            return shipmentDAL.UploadProofofTempDocument(proofofTempraturesList);
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
            if (entity.ShipmentFreightDetail != null)
            {
                foreach (var freight in entity.ShipmentFreightDetail)
                {
                    if (freight.TemperatureType == "C" && freight.Temperature != null)
                    {
                        freight.Temperature = ConversionFormula.NullCelsiusToFahrenheit(freight.Temperature);
                        freight.TemperatureType = "F";
                    }
                }
            }
            return shipmentDAL.EditShipment(entity);
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
            return shipmentDAL.AddRouteStops(model);
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
            if (model.TemperatureType == "C" && model.Temperature != null)
            {
                model.Temperature = ConversionFormula.NullCelsiusToFahrenheit(model.Temperature);
                model.TemperatureType = "F";
            }
            return shipmentDAL.AddFreightDetail(model);
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
            return shipmentDAL.DeleteProofOfTemprature(model);
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
            return shipmentDAL.DeleteDamageFile(model);
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
            return shipmentDAL.DeleteShipment(entity);
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
            return shipmentDAL.ApprovedShipment(entity);
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
            return shipmentDAL.bindFreightType();
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
            return shipmentDAL.GetCustomerDetail(customerShipmentDTO);
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
            return shipmentDAL.ShipmentProofOfDelivery(shipmentId);
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
            return shipmentDAL.ApprovedDamageImage(entity);
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
            return shipmentDAL.ApprovedProofOFTemp(entity);
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
            if (model != null)
            {
                model.FirstPickupArrivalDate = (model.FirstPickupArrivalDate == null ? model.FirstPickupArrivalDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.FirstPickupArrivalDate)));
                model.LastPickupArrivalDate = (model.LastPickupArrivalDate == null ? model.LastPickupArrivalDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.LastPickupArrivalDate)));
            }
            return shipmentDAL.ValidateEquipmentNDriver(model);
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
            if (model != null)
            {
                model.FirstPickupArrivalDate = (model.FirstPickupArrivalDate == null ? model.FirstPickupArrivalDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.FirstPickupArrivalDate)));
                model.LastPickupArrivalDate = (model.LastPickupArrivalDate == null ? model.LastPickupArrivalDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.LastPickupArrivalDate)));
            }
            return shipmentDAL.ValidateDriver(model);
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
            if (model != null)
            {
                model.FirstPickupArrivalDate = (model.FirstPickupArrivalDate == null ? model.FirstPickupArrivalDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.FirstPickupArrivalDate)));
                model.LastPickupArrivalDate = (model.LastPickupArrivalDate == null ? model.LastPickupArrivalDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.LastPickupArrivalDate)));
            }
            return shipmentDAL.ValidateEquipment(model);
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

            return shipmentDAL.SaveCopyShipmentDetail(entity);
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
            return shipmentDAL.GetCopyShipmentDetailById(shipmentId);
        }

        

        #endregion

        public bool ShipmentIsReady(int shipmentId, bool ready)
        {
            return shipmentDAL.ShipmentIsReady(shipmentId, ready);
        }

        public bool ShipmentWTReady(int shipmentId, bool ready)
        {
            return shipmentDAL.ShipmentWTReady(shipmentId, ready);
        }

        public bool ShipmentSTReady(int shipmentId, bool ready)
        {
            return shipmentDAL.ShipmentSTReady(shipmentId, ready);
        }

        #region Get max route no.
        /// <summary>
        /// Get max route no.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>

        public int? GetMaxRouteNo(int shipmentId)
        {
            return shipmentDAL.GetMaxRouteNo(shipmentId);

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
            return shipmentDAL.GetCheckInTime(driverId);
        }
        #endregion

        public string DriverPhone(int driverid)
        {
            return shipmentDAL.DriverPhone(driverid);
        }

        public string CustomerName(int CustomerID)
        {
            return shipmentDAL.CustomerName(CustomerID);
        }

        public TemperatureEmailSipmentDTO GetTemperatureEmailDetail(int shipmentId)
        {
            return shipmentDAL.GetTemperatureEmailDetail(shipmentId);
        }

        #region Delete Comments
        /// <summary>
        ///Delete shipment By Id
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public bool DeleteComments(ShipmentDTO entity)
        {
            return shipmentDAL.DeleteComments(entity);
        }


        #endregion
    }

}
