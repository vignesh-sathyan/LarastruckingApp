using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.GpsTracker;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class GpsTrackerRepository : IGpsTrackingRepository
    {
        #region Private Members
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities GpsContext;

        #endregion
        #region Contructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="IGpsTrackingDAL"></param>

        public GpsTrackerRepository()
        {

            GpsContext = new LarastruckingDBEntities();

        }
        #endregion

        #region Get Shipment Details and Gps Tracking Details 
        /// <summary>
        /// Get Shipment Details and Gps Tracking Details 
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>

        public GpsShipmentDetailsDTO GetShipmentDetails(int shipmentId)
        {
            try
            {


                var shipmentdetail = (from shipment in GpsContext.tblShipments
                                      where shipment.ShipmentId == shipmentId
                                      select new GpsShipmentDetailsDTO
                                      {
                                          ShipmentId = shipment.ShipmentId,
                                          ShipmentRefNo = shipment.ShipmentRefNo,
                                          AirWayBill = shipment.AirWayBill,
                                          PickDateTime = (from route in GpsContext.tblShipmentRoutesStops where route.ShippingId == shipmentId && route.IsDeleted == false select route.DriverPickupArrival).FirstOrDefault(),
                                          DeliveryDateTime = (from route in GpsContext.tblShipmentRoutesStops where route.ShippingId == shipmentId && route.IsDeleted == false select new { deliverydate = route.DriverDeliveryDeparture, routeid = route.ShippingRoutesId }).OrderByDescending(x => x.routeid).Select(x => x.deliverydate).FirstOrDefault(),

                                          ShipmentEquipmentNdriver = (from eqpNdriver in GpsContext.tblShipmentEquipmentNdrivers
                                                                      join driver in GpsContext.tblDrivers on eqpNdriver.DriverId equals driver.DriverID
                                                                      join equipmnet in GpsContext.tblEquipmentDetails on eqpNdriver.EquipmentId equals equipmnet.EDID
                                                                      where eqpNdriver.ShipmentId == shipmentId
                                                                      select new GetShipmentEquipmentNDriverDTO
                                                                      {
                                                                          ShipmentEquipmentNDriverId = eqpNdriver.ShipmentEquipmentNdriverId,
                                                                          UserId = driver.UserId,
                                                                          DriverId = eqpNdriver.DriverId,
                                                                          DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty),
                                                                          EquipmentId = eqpNdriver.EquipmentId,
                                                                          EquipmentName = equipmnet.EquipmentNo
                                                                      }).ToList(),
                                      }
                                      ).FirstOrDefault();
                shipmentdetail.PickDateTime = shipmentdetail.PickDateTime == null ? DateTime.Now : Configurations.ConvertDateTime(Convert.ToDateTime(shipmentdetail.PickDateTime));
                shipmentdetail.DeliveryDateTime = shipmentdetail.DeliveryDateTime == null ? DateTime.Now : Configurations.ConvertDateTime(Convert.ToDateTime(shipmentdetail.DeliveryDateTime));
                return shipmentdetail;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Gps Tracking Details 
        /// <summary>
        /// Gps Tracking Details 
        /// </summary> 
        /// <param name="shipmentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public List<GpsTrackerHistoryDTO> GetGpsTrackerDetails(GpsTrackerHistoryDTO dto)
        {
            try
            {
                dto.StartDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.StartDate));// Convert.ToDateTime("2021-01-08 13:40:00.000");

                dto.EndDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.EndDate));  //Convert.ToDateTime("2021-01-08 14:06:19.000");

                List<GpsTrackerHistoryDTO> gpsDriverTrackShipmentDetails = new List<GpsTrackerHistoryDTO>();
                if (dto.DefaultDetails == "GetDetailsWithStartEnddate")
                {
                    gpsDriverTrackShipmentDetails = (from GpsDriver in GpsContext.tblDriverGpsTrakingHistories
                                                     where GpsDriver.ShipmentId == dto.ShipmentId && GpsDriver.UserId == dto.UserId
                                                     && (GpsDriver.CreatedOn >= dto.StartDate && GpsDriver.CreatedOn <= dto.EndDate)
                                                     select new GpsTrackerHistoryDTO
                                                     {
                                                         ShipmentId = GpsDriver.ShipmentId,
                                                         UserId = GpsDriver.UserId,
                                                         Latitude = GpsDriver.Latitude,
                                                         longitude = GpsDriver.longitude,
                                                         CreatedOn = GpsDriver.CreatedOn,
                                                         Event = GpsDriver.Event ?? string.Empty,
                                                     }).ToList();
                }
                else
                {
                    gpsDriverTrackShipmentDetails = (from GpsDriver in GpsContext.tblDriverGpsTrakingHistories
                                                     where
                                                     // GpsDriver.ShipmentId == dto.ShipmentId &&
                                                     GpsDriver.UserId == dto.UserId
                                                      && (GpsDriver.CreatedOn >= dto.StartDate && GpsDriver.CreatedOn <= dto.EndDate)

                                                     select new GpsTrackerHistoryDTO
                                                     {
                                                         DriverGPSID = GpsDriver.DriverGpsID,
                                                         ShipmentId = GpsDriver.ShipmentId,
                                                         UserId = GpsDriver.UserId,
                                                         Latitude = GpsDriver.Latitude,
                                                         longitude = GpsDriver.longitude,
                                                         CreatedOn = GpsDriver.CreatedOn,
                                                         Event = GpsDriver.Event,
                                                         // PickDateTime = (from route in GpsContext.tblShipmentRoutesStops where route.ShippingId == GpsDriver.ShipmentId && route.IsDeleted == false select route.PickDateTime).FirstOrDefault(),
                                                         //  DeliveryDateTime = (from route in GpsContext.tblShipmentRoutesStops where route.ShippingId == GpsDriver.ShipmentId && route.IsDeleted == false select new { deliverydate = route.DeliveryDateTime, routeid = route.ShippingRoutesId }).OrderByDescending(x => x.routeid).Select(x => x.deliverydate).FirstOrDefault(),

                                                     }).ToList();
                }
                gpsDriverTrackShipmentDetails = gpsDriverTrackShipmentDetails.GroupBy(x => x.Latitude).Select(x => new { key = x.Key, listofes = (x.Where(y => y.Event != null).ToList().Count > 0 ? x.Where(y => y.Event != null).FirstOrDefault() : x.FirstOrDefault()) }).ToList().Select(x => x.listofes).OrderBy(x => x.DriverGPSID).ToList();
                return gpsDriverTrackShipmentDetails.ToList();
                //return gpsDriverTrackShipmentDetails.Distinct().ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion


        #region Get Fumigation Details and Gps Tracking Details 
        /// <summary>
        /// Get Shipment Details and Gps Tracking Details 
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>

        public GpsFumigationDetailsDTO GetFumigationDetails(int FumigationId)
        {
            try
            {


                var Fumigationdetail = (from Fumigation in GpsContext.tblFumigations
                                        where Fumigation.FumigationId == FumigationId
                                        select new GpsFumigationDetailsDTO
                                        {
                                            FumigationId = Fumigation.FumigationId,
                                            ShipmentRefNo = Fumigation.ShipmentRefNo,
                                            PickDateTime = (from route in GpsContext.tblFumigationRouts where route.FumigationId == FumigationId && route.IsDeleted == false select route.DriverPickupArrival).FirstOrDefault(),
                                            DeliveryDateTime = (from route in GpsContext.tblFumigationRouts where route.FumigationId == FumigationId && route.IsDeleted == false select new { deliverydate = route.DriverDeliveryDeparture, routeid = route.FumigationRoutsId }).OrderByDescending(x => x.routeid).Select(x => x.deliverydate).FirstOrDefault(),

                                            FumigationEquipmentNdriver = (from eqpNdriver in GpsContext.tblFumigationEquipmentNDrivers
                                                                          join fumigationRoute in GpsContext.tblFumigationRouts on eqpNdriver.FumigationRoutsId equals fumigationRoute.FumigationRoutsId
                                                                          join address in GpsContext.tblAddresses on fumigationRoute.PickUpLocation equals address.AddressId
                                                                          join state in GpsContext.tblStates on address.State equals state.ID
                                                                          join driver in GpsContext.tblDrivers on eqpNdriver.DriverId equals driver.DriverID
                                                                          join equipmnet in GpsContext.tblEquipmentDetails on eqpNdriver.EquipmentId equals equipmnet.EDID
                                                                          join address1 in GpsContext.tblAddresses on fumigationRoute.DeliveryLocation equals address1.AddressId
                                                                          join state1 in GpsContext.tblStates on address1.State equals state1.ID
                                                                          join address2 in GpsContext.tblAddresses on fumigationRoute.FumigationSite equals address2.AddressId
                                                                          join state2 in GpsContext.tblStates on address2.State equals state2.ID

                                                                          where eqpNdriver.FumigationId == FumigationId && eqpNdriver.IsDeleted == false
                                                                          select new GetFumigationEquipmentNDriversDTO
                                                                          {
                                                                              FumigationEquipmentNDriverId = eqpNdriver.FumigationEquipmentNDriverId,
                                                                              UserId = driver.UserId,
                                                                              DriverId = eqpNdriver.DriverId,
                                                                              DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty),
                                                                              EquipmentId = eqpNdriver.EquipmentId,
                                                                              EquipmentName = equipmnet.EquipmentNo,
                                                                              PickupLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + "," + address.City + "," + state.Name + "," + address.Zip,
                                                                              DeliveryLocation = (address1.CompanyName == null ? string.Empty : (address1.CompanyName + ",")) + address1.Address1 + "," + address1.City + "," + state1.Name + "," + address1.Zip,
                                                                              FumigationLocation = (address2.CompanyName == null ? string.Empty : (address2.CompanyName + ",")) + address2.Address1 + "," + address2.City + "," + state2.Name + "," + address2.Zip,
                                                                              IsPickUp = eqpNdriver.IsPickUp,
                                                                              IsDeleted = eqpNdriver.IsDeleted,
                                                                              AirWayBillNo = fumigationRoute.AirWayBill,
                                                                          }).ToList(),
                                        }
                                      ).FirstOrDefault();

                Fumigationdetail.PickDateTime = Fumigationdetail.PickDateTime == null ? DateTime.Now : Configurations.ConvertDateTime(Convert.ToDateTime(Fumigationdetail.PickDateTime));
                Fumigationdetail.DeliveryDateTime = Fumigationdetail.DeliveryDateTime == null ? DateTime.Now : Configurations.ConvertDateTime(Convert.ToDateTime(Fumigationdetail.DeliveryDateTime));
                return Fumigationdetail;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Gps Fumigation Tracking Details 
        /// <summary>
        /// Gps Fumigation Tracking Details 
        /// </summary> 
        /// <param name="FumigationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public List<GpsTrackerHistoryDTO> GetFumigationGpsTrackerDetails(GpsTrackerHistoryDTO dto)
        {
            try
            {
                dto.StartDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.StartDate));// Convert.ToDateTime("2021-01-08 13:40:00.000");

                dto.EndDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.EndDate));  //Convert.ToDateTime("2021-01-08 14:06:19.000");


                List<GpsTrackerHistoryDTO> gpsDriverTrackFumigationDetails = new List<GpsTrackerHistoryDTO>();
                if (dto.DefaultDetails == "GetDetailsWithStartEnddate")
                {
                    gpsDriverTrackFumigationDetails = (from GpsDriver in GpsContext.tblDriverGpsTrakingHistories
                                                       where GpsDriver.FumigationId == dto.FumigationId && GpsDriver.UserId == dto.UserId
                                                       && (GpsDriver.CreatedOn >= dto.StartDate && GpsDriver.CreatedOn <= dto.EndDate)
                                                       select new GpsTrackerHistoryDTO
                                                       {
                                                           FumigationId = GpsDriver.FumigationId,
                                                           UserId = GpsDriver.UserId,
                                                           Latitude = GpsDriver.Latitude,
                                                           longitude = GpsDriver.longitude,
                                                           CreatedOn = GpsDriver.CreatedOn,
                                                           Event = GpsDriver.Event ?? string.Empty,
                                                       }).ToList();
                }
                else
                {
                    gpsDriverTrackFumigationDetails = (from GpsDriver in GpsContext.tblDriverGpsTrakingHistories
                                                       where
                                                       //GpsDriver.FumigationId == dto.FumigationId &&
                                                       GpsDriver.UserId == dto.UserId
                                                        && (GpsDriver.CreatedOn >= dto.StartDate && GpsDriver.CreatedOn <= dto.EndDate)
                                                       select new GpsTrackerHistoryDTO
                                                       {
                                                           FumigationId = GpsDriver.FumigationId,
                                                           UserId = GpsDriver.UserId,
                                                           Latitude = GpsDriver.Latitude,
                                                           longitude = GpsDriver.longitude,
                                                           CreatedOn = GpsDriver.CreatedOn,
                                                           Event = GpsDriver.Event ?? string.Empty,

                                                       }).ToList();
                }
                gpsDriverTrackFumigationDetails = gpsDriverTrackFumigationDetails.GroupBy(x => x.Latitude).Select(x => new { key = x.Key, listofes = (x.Where(y => y.Event != null).ToList().Count > 0 ? x.Where(y => y.Event != null).FirstOrDefault() : x.FirstOrDefault()) }).ToList().Select(x => x.listofes).OrderBy(x => x.DriverGPSID).ToList();
                return gpsDriverTrackFumigationDetails.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
