using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.QuotesDto;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using LarastruckingApp.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class ShipmentRepository : IShipmentRepository
    {
        #region private member
        private readonly ExecuteSQLStoredProceduce sp_dbContext = null;
        private readonly LarastruckingDBEntities shipmentContext;
        #endregion

        #region constroctor
        public ShipmentRepository()
        {
            shipmentContext = new LarastruckingDBEntities();
            sp_dbContext = new ExecuteSQLStoredProceduce();
        }
        #endregion

        #region shipment status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            try
            {

                var statuslist = shipmentContext.tblShipmentStatus.Where(x => x.IsActive && x.IsDeleted == false && x.IsShipment == true).OrderBy(x => x.DisplayOrder);
                return AutoMapperServices<tblShipmentStatu, ShipmentStatusDTO>.ReturnObjectList(statuslist.ToList());
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                var substatuslist = shipmentContext.tblShipmentSubStatus.Where(x => x.IsActive && x.IsDeleted == false && x.StatusId == statusid).OrderBy(x => x.DisplayOrder);
                return AutoMapperServices<tblShipmentSubStatu, ShipmentSubStatusDTO>.ReturnObjectList(substatuslist.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region get driver list
        /// <summary>
        /// get driver list for bind 
        /// </summary>
        /// <returns></returns>
        public List<ShipmentDriverDTO> DriverList(ValidateDriverNEquipmentDTO entity)
        {
            try
            {
                bool isBonded = false;
                if (entity.ShipmentId > 0)
                {
                    isBonded = shipmentContext.tblShipmentFreightDetails.Where(x => x.FreightTypeId == 2 && x.ShipmentId == entity.ShipmentId).ToList().Count > 0 ? true : false;
                }



                var driverWithLeaveList = (from user in shipmentContext.tblUsers
                                           join driver in shipmentContext.tblDrivers on user.Userid equals driver.UserId
                                           join userrole in shipmentContext.tblUserRoles on user.Userid equals userrole.UserID
                                           join leave in shipmentContext.tblLeaves on user.Userid equals leave.UserId
                                           where user.IsActive && driver.IsActive && driver.IsDeleted == false && user.IsDeleted == false && userrole.RoleID == 4
                                           && ((entity.FirstPickupArrivalDate >= leave.TakenFrom || entity.LastPickupArrivalDate >= leave.TakenFrom)
                                           && (entity.FirstPickupArrivalDate <= leave.TakenTo || entity.LastPickupArrivalDate <= leave.TakenTo))
                                           select new ShipmentDriverDTO
                                           {
                                               DriverId = driver.DriverID,
                                               DriverName = user.FirstName + " " + (user.LastName ?? string.Empty)
                                           }
                    ).ToList();


                //Check driver on leave or not
                var driverWithoutLeaveList = (from driver in shipmentContext.tblDrivers
                                              where driver.IsActive && driver.IsDeleted == false
                                              select new ShipmentDriverDTO
                                              {
                                                  DriverId = driver.DriverID,
                                                  DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty),
                                                  IsTSACertificate = (shipmentContext.tblDriverDocuments.Where(x => x.DocumentTypeId == 6 && x.DriverId == driver.DriverID && x.IsActive && x.IsDeleted == false).ToList()).Count() > 0 ? true : false,

                                              }).ToList();
                if (isBonded)
                {
                    driverWithoutLeaveList = driverWithoutLeaveList.Where(x => x.IsTSACertificate).ToList();
                }

                driverWithoutLeaveList = driverWithoutLeaveList.Where(x => !driverWithLeaveList.Any(y => y.DriverId == x.DriverId)).ToList();

                var trailerRentalPickupDriver = (from TR in shipmentContext.tblTrailerRentals
                                                 join TRD in shipmentContext.tblTrailerRentalDetails on TR.TrailerRentalId equals TRD.TrailerRentalId
                                                 join driver in shipmentContext.tblDrivers on TRD.PickupDriverId equals driver.DriverID
                                                 where (entity.TrailerRentalId > 0 ? TR.TrailerRentalId != entity.TrailerRentalId : 1 == 1) && TRD.IsDeleted == false && TR.IsDeleted == false && driver.IsActive && driver.IsDeleted == false
                                                 && (entity.FirstPickupArrivalDate <= TRD.StartDate || entity.FirstPickupArrivalDate <= TRD.EndDate || entity.LastPickupArrivalDate <= TRD.EndDate)
                                                 select new ShipmentDriverDTO
                                                 {
                                                     DriverId = driver.DriverID,
                                                     DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty)
                                                 }).ToList();
                // driverWithoutLeaveList = driverWithoutLeaveList.Where(x => !trailerRentalPickupDriver.Any(y => y.DriverId == x.DriverId)).ToList();
                var trailerRentalDeliveryDriver = (from TR in shipmentContext.tblTrailerRentals
                                                   join TRD in shipmentContext.tblTrailerRentalDetails on TR.TrailerRentalId equals TRD.TrailerRentalId
                                                   join driver in shipmentContext.tblDrivers on TRD.DeliveryDriverId equals driver.DriverID
                                                   where (entity.TrailerRentalId > 0 ? TR.TrailerRentalId != entity.TrailerRentalId : 1 == 1) && TR.IsDeleted == false && TRD.IsDeleted == false && driver.IsActive && driver.IsDeleted == false
                                                   && (entity.FirstPickupArrivalDate <= TRD.StartDate || entity.FirstPickupArrivalDate <= TRD.EndDate || entity.LastPickupArrivalDate <= TRD.EndDate)
                                                   select new ShipmentDriverDTO
                                                   {
                                                       DriverId = driver.DriverID,
                                                       DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty)
                                                   }).ToList();
                //driverWithoutLeaveList = driverWithoutLeaveList.Where(x => !trailerRentalDeliveryDriver.Any(y => y.DriverId == x.DriverId)).ToList();
                return driverWithoutLeaveList.OrderBy(x => x.DriverName).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region get all driver list
        /// <summary>
        /// et all driver list 
        /// </summary>
        /// <returns></returns>
        public List<ShipmentDriverDTO> DriverList()
        {
            try
            {

                //Check driver on leave or not
                var driverList = (from driver in shipmentContext.tblDrivers
                                  where driver.IsActive && driver.IsDeleted == false
                                  select new ShipmentDriverDTO
                                  {
                                      DriverId = driver.DriverID,
                                      DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty),
                                      IsTSACertificate = (shipmentContext.tblDriverDocuments.Where(x => x.DocumentTypeId == 6 && x.DriverId == driver.DriverID && x.IsActive && x.IsDeleted == false).ToList()).Count() > 0 ? true : false,

                                  }).ToList();



                return driverList.OrderBy(x => x.DriverName).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region get equipment detail
        /// <summary>
        /// Get equipment detail
        /// </summary>
        /// <returns></returns>
        public List<ShipmentEquipmentDTO> EquipmnetList(ValidateDriverNEquipmentDTO model)
        {
            try
            {

                List<ShipmentEquipmentDTO> shipmentEquipmentList = new List<ShipmentEquipmentDTO>();

                var equipmentlist = shipmentContext.tblEquipmentDetails.Include("tblEquipmentType").Include("tblEquipmentFreights").Include("tblEquipmentFreights.tblFreightType")
                    .Include("tblEquipmentDoorTypes").Include("tblEquipmentDoorTypes.tblDoorType").Where(x => x.IsDeleted == false).Select(x => new ShipmentEquipmentDTO()
                    {
                        EDID = x.EDID,
                        VehicleType = x.tblEquipmentType.VehicleTypeName,
                        EquipmentNo = x.EquipmentNo ?? string.Empty,
                        FreightTypeList = x.tblEquipmentFreights.Select(y => y.tblFreightType.FreightTypeName).ToList(),
                        DoorTypeList = x.tblEquipmentDoorTypes.Select(y => y.tblDoorType.DoorType).ToList(),
                        Bed = x.RollerBed ?? string.Empty,
                        MaxLoad = x.MaxLoad ?? string.Empty,

                    }).ToList();
                if (equipmentlist.Count() > 0)
                {
                    var trailerRentalDetails = (from TR in shipmentContext.tblTrailerRentals
                                                join TRD in shipmentContext.tblTrailerRentalDetails on TR.TrailerRentalId equals TRD.TrailerRentalId
                                                where (model.TrailerRentalId > 0 ? TR.TrailerRentalId != model.TrailerRentalId : 1 == 1) && TRD.IsDeleted == false && TR.IsDeleted == false
                                                                && (model.FirstPickupArrivalDate <= TRD.StartDate || model.FirstPickupArrivalDate <= TRD.EndDate || model.LastPickupArrivalDate <= TRD.EndDate)
                                                select TRD.EquipmentId
                                                              ).ToList();

                    var shipments = (from shipment in shipmentContext.tblShipments
                                     join routes in shipmentContext.tblShipmentRoutesStops on shipment.ShipmentId equals routes.ShippingId
                                     join tblEquipment in shipmentContext.tblShipmentEquipmentNdrivers on routes.ShippingId equals tblEquipment.ShipmentId
                                     where (shipment.StatusId != 7 && shipment.StatusId != 8 && shipment.StatusId != 11)
                                     && (model.ShipmentId > 0 ? model.ShipmentId != shipment.ShipmentId : 1 == 1)
                                     && shipment.IsDeleted == false
                                     && routes.IsDeleted == false
                                     && (model.FirstPickupArrivalDate <= routes.PickDateTime || model.FirstPickupArrivalDate <= routes.DeliveryDateTime || model.LastPickupArrivalDate <= routes.DeliveryDateTime)
                                     select tblEquipment.EquipmentId
                                     ).ToList();

                    var fumigations = (from fumigation in shipmentContext.tblFumigations
                                       join route in shipmentContext.tblFumigationRouts on fumigation.FumigationId equals route.FumigationId
                                       join equipment in shipmentContext.tblFumigationEquipmentNDrivers on route.FumigationId equals equipment.FumigationId
                                       where (model.FumigationId > 0 ? fumigation.FumigationId != model.FumigationId : 1 == 1) && equipment.FumigationRoutsId == route.FumigationRoutsId && (fumigation.StatusId != 7 && fumigation.StatusId != 8 && fumigation.StatusId != 11)
                                       && fumigation.IsDeleted == false
                                       && route.IsDeleted == false
                                       && (model.FirstPickupArrivalDate <= route.PickUpArrival || model.FirstPickupArrivalDate <= route.FumigationArrival || model.LastPickupArrivalDate <= route.FumigationArrival)
                                        && (model.FirstPickupArrivalDate <= route.ReleaseDate || model.FirstPickupArrivalDate <= route.DeliveryArrival || model.LastPickupArrivalDate <= route.DeliveryArrival)
                                       select equipment.EquipmentId).ToList();


                    foreach (var equipment in equipmentlist)
                    {
                        equipment.FreightType = String.Join(",", equipment.FreightTypeList) ?? string.Empty;
                        equipment.DoorType = String.Join(",", equipment.DoorTypeList) ?? string.Empty;
                        int equipmentCount = 0;
                        if (model.FirstPickupArrivalDate != null && model.LastPickupArrivalDate != null)
                        {
                            equipmentCount = trailerRentalDetails.Where(x => x.Value == equipment.EDID).Count();

                            equipment.IsAssigned = shipments.Where(x => x.Value == equipment.EDID).Count() > 0;

                            if (equipment.IsAssigned == false)
                            {
                                equipment.IsAssigned = fumigations.Where(x => x.Value == equipment.EDID).Count() > 0;
                            }
                        }
                        if (equipmentCount == 0)
                        {
                            shipmentEquipmentList.Add(equipment);
                        }
                    }
                }
                return shipmentEquipmentList.OrderBy(x => x.EquipmentNo).ToList();

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region get accessorial fee type
        /// <summary>
        /// get accessorial fee type list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentAccessorialFeeTypeDTO> GetShipmentAccessorialFeeType()
        {
            try
            {


                var accessorailFeeTypeList = shipmentContext.tblAccessorialFeesTypes.Where(x => x.IsActive);
                return AutoMapperServices<tblAccessorialFeesType, ShipmentAccessorialFeeTypeDTO>.ReturnObjectList(accessorailFeeTypeList.OrderBy(x => x.Name).ToList());
            }
            catch (Exception)
            {

                throw;
            }
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

            ShipmentDTO objQuoteDTO = new ShipmentDTO();
            try
            {
                entity.ShipmentFreightDetail = entity.ShipmentFreightDetail == null ? null : GetFreightList(entity.ShipmentFreightDetail, entity.CustomerId);
                var shipmentRoutStops = entity.ShipmentRoutesStop == null ? null : SqlUtil.ToDataTable(entity.ShipmentRoutesStop);
                var accessorialPrice = entity.AccessorialPrice == null ? null : SqlUtil.ToDataTable(entity.AccessorialPrice);
                var equipmentNDriver = entity.ShipmentEquipmentNdriver == null ? null : SqlUtil.ToDataTable(entity.ShipmentEquipmentNdriver);
                var freightdetail = entity.ShipmentFreightDetail == null ? null : SqlUtil.ToDataTable(entity.ShipmentFreightDetail);
                string ShipmetnRefNo = GenrateShipmentRefNo();
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SPType", SpType.Insert),
                        new SqlParameter("@CustomerId", entity.CustomerId),
                        new SqlParameter("@StatusId", entity.StatusId),
                        new SqlParameter("@SubStatusId", entity.SubStatusId),
                        new SqlParameter("@RequestedBy", entity.RequestedBy),
                        new SqlParameter("@Reason",entity.Reason),
                        new SqlParameter("@ShipmentRefNo",ShipmetnRefNo),
                        new SqlParameter("@AirWayBill", entity.AirWayBill),
                        new SqlParameter("@CustomerPO", entity.CustomerPO),
                        new SqlParameter("@OrderNo", entity.OrderNo),
                        new SqlParameter("@CustomerRef",entity.CustomerRef),
                        new SqlParameter("@ContainerNo",entity.ContainerNo),
                         new SqlParameter("@PurchaseDoc",entity.PurchaseDoc),
                        new SqlParameter("@FinalTotalAmount",entity.FinalTotalAmount),
                        new SqlParameter("@DriverInstruction",entity.DriverInstruction),
                        new SqlParameter("@ShipmentRouteStopsDetail",shipmentRoutStops),
                        new SqlParameter("@AccessorialPrice",accessorialPrice),
                         new SqlParameter("@ShipmentDriverNEquipment",equipmentNDriver),
                         new SqlParameter("@ShipmentFreightDetail",freightdetail),
                          new SqlParameter("@VendorNconsignee",entity.VendorNconsignee),
                        new SqlParameter("@CreatedBy", entity.CreatedBy),
                     };

                var result = sp_dbContext.ExecuteStoredProcedure<SpResponseDTO>("usp_CreateShipment", sqlParameters);

                var response = result != null && result.Count > 0 ? result[0] : new SpResponseDTO();

                objQuoteDTO.IsSuccess = (response.ResponseText == Configurations.Insert);


            }
            catch (Exception)
            {
                throw;
            }
            return objQuoteDTO;

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
            try
            {


                GetShipmentDTO shipment = new GetShipmentDTO();
                if (entity != null)
                {

                    var shipmentdata = shipmentContext.tblShipments.Where(x => x.ShipmentId == entity.ShipmentId).FirstOrDefault();


                    if (shipmentdata != null)
                    {
                        if (shipmentdata.StatusId != entity.StatusId && entity.StatusId != 11)
                        {
                            shipment.IsMailNeedToSend = true;
                        }
                        shipmentdata.StatusId = entity.StatusId;
                        shipmentdata.SubStatusId = entity.SubStatusId;
                        shipmentdata.RequestedBy = entity.RequestedBy;
                        shipmentdata.Reason = entity.Reason;
                        shipmentdata.ShipmentRefNo = entity.ShipmentRefNo;
                        shipmentdata.AirWayBill = entity.AirWayBill;
                        shipmentdata.CustomerPO = entity.CustomerPO;
                        shipmentdata.OrderNo = entity.OrderNo;
                        shipmentdata.CustomerRef = entity.CustomerRef;
                        shipmentdata.ContainerNo = entity.ContainerNo;
                        shipmentdata.PurchaseDoc = entity.PurchaseDoc;
                        shipmentdata.DriverInstruction = entity.DriverInstruction;
                        shipmentdata.VendorNconsignee = entity.VendorNconsignee;
                        shipmentdata.ModifiedBy = entity.CreatedBy;
                        shipmentdata.ModifiedDate = entity.CreatedDate;
                        shipmentContext.Entry(shipmentdata).State = EntityState.Modified;
                        var statusHistory = shipmentContext.tblShipmentStatusHistories.Where(x => x.ShipmentId == entity.ShipmentId).OrderByDescending(x => x.ShipmentStatusHistoryId).FirstOrDefault();
                        if (statusHistory != null && statusHistory.StatusId != entity.StatusId)
                        {
                            tblShipmentStatusHistory shipmentStatusHistory = new tblShipmentStatusHistory();
                            shipmentStatusHistory.ShipmentId = entity.ShipmentId;
                            shipmentStatusHistory.StatusId = Convert.ToInt32(entity.StatusId);
                            shipmentStatusHistory.SubStatusId = entity.SubStatusId;
                            shipmentStatusHistory.Reason = entity.Reason;
                            shipmentStatusHistory.CreatedBy = entity.CreatedBy;
                            shipmentStatusHistory.CreatedOn = entity.CreatedDate;
                            shipmentContext.tblShipmentStatusHistories.Add(shipmentStatusHistory);

                            tblShipmentEventHistory shipmentEventHistory = new tblShipmentEventHistory();
                            shipmentEventHistory.ShipmentId = entity.ShipmentId;
                            shipmentEventHistory.StatusId = entity.StatusId;
                            shipmentEventHistory.UserId = entity.CreatedBy;
                            shipmentEventHistory.Event = "STATUS";
                            shipmentEventHistory.EventDateTime = entity.CreatedDate;
                            shipmentContext.tblShipmentEventHistories.Add(shipmentEventHistory);


                        }
                        if (entity.ShipmentRoutesStop != null)
                        {
                            foreach (var SRS in entity.ShipmentRoutesStop)
                            {
                                if (SRS.ShipmentRouteStopId > 0)
                                {
                                    if (SRS.IsDeleted)
                                    {
                                        var shpRouteStops = shipmentContext.tblShipmentRoutesStops.Where(x => x.ShippingRoutesId == SRS.ShipmentRouteStopId && x.IsDeleted == false).FirstOrDefault();
                                        if (shpRouteStops != null)
                                        {
                                            shpRouteStops.IsDeleted = true;
                                            shpRouteStops.DeletedBy = entity.CreatedBy;
                                            shpRouteStops.DeletedOn = entity.CreatedDate;
                                            shipmentContext.Entry(shpRouteStops).State = EntityState.Modified;

                                        }

                                    }
                                    else
                                    {
                                        var shpRouteStops = shipmentContext.tblShipmentRoutesStops.Where(x => x.ShippingRoutesId == SRS.ShipmentRouteStopId && x.IsDeleted == false).FirstOrDefault();
                                        if (shpRouteStops != null)
                                        {

                                            shpRouteStops.PickupLocationId = SRS.PickupLocationId;
                                            shpRouteStops.DeliveryLocationId = SRS.DeliveryLocationId;
                                            shpRouteStops.PickDateTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(SRS.PickDateTime));
                                            shpRouteStops.PickUpDateTimeTo = Configurations.ConvertLocalToUTC(Convert.ToDateTime(SRS.PickDateTimeTo));
                                            shpRouteStops.DeliveryDateTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(SRS.DeliveryDateTime));
                                            shpRouteStops.DeliveryDateTimeTo = Configurations.ConvertLocalToUTC(Convert.ToDateTime(SRS.DeliveryDateTimeTo));
                                            shpRouteStops.Comment = SRS.Comment;
                                            shpRouteStops.IsDeliveryWaitingTimeRequired = SRS.IsDeliveryWaitingTimeRequired;
                                            shpRouteStops.IsPickUpWaitingTimeRequired = SRS.IsPickUpWaitingTimeRequired;
                                            shpRouteStops.IsAppointmentRequired = SRS.IsAppointmentRequired;
                                            shipmentContext.Entry(shpRouteStops).State = EntityState.Modified;
                                        }

                                    }

                                }
                            }

                        }
                        if (entity.AccessorialPrice != null)
                        {
                            foreach (var accPrice in entity.AccessorialPrice)
                            {
                                if (accPrice.ShipmentAccessorialPriceId > 0)
                                {
                                    if (accPrice.IsDeleted)
                                    {
                                        var shipmentAccessorialPrice = shipmentContext.tblShipmentAccessorialPrices.Where(x => x.ShipmentAccessorialPriceId == accPrice.ShipmentAccessorialPriceId && x.IsDeleted == false).FirstOrDefault();
                                        if (shipmentAccessorialPrice != null)
                                        {


                                            shipmentAccessorialPrice.IsDeleted = true;
                                            shipmentAccessorialPrice.DeletedBy = entity.CreatedBy;
                                            shipmentAccessorialPrice.DeletedOn = entity.CreatedDate;
                                            shipmentContext.Entry(shipmentAccessorialPrice).State = EntityState.Modified;
                                        }
                                    }
                                    else
                                    {
                                        var shipmentAccessorialPrice = shipmentContext.tblShipmentAccessorialPrices.Where(x => x.ShipmentAccessorialPriceId == accPrice.ShipmentAccessorialPriceId && x.IsDeleted == false).FirstOrDefault();
                                        if (shipmentAccessorialPrice != null)
                                        {
                                            shipmentAccessorialPrice.Unit = accPrice.Unit;
                                            shipmentAccessorialPrice.AmtPerUnit = accPrice.AmtPerUnit;
                                            shipmentAccessorialPrice.Amount = accPrice.Amount;
                                            shipmentAccessorialPrice.Reason = accPrice.Reason;
                                            shipmentContext.Entry(shipmentAccessorialPrice).State = EntityState.Modified;
                                        }
                                    }

                                }
                                else
                                {
                                    if (accPrice.IsDeleted == false)
                                    {
                                        var shpRouteStops = shipmentContext.tblShipmentRoutesStops.Where(x => x.ShippingId == entity.ShipmentId && x.RouteNo == accPrice.RouteNo && x.IsDeleted == false).FirstOrDefault();
                                        if (shpRouteStops != null)
                                        {
                                            tblShipmentAccessorialPrice objShipmentAccessorialPrice = new tblShipmentAccessorialPrice();
                                            objShipmentAccessorialPrice.ShipmentId = entity.ShipmentId;
                                            objShipmentAccessorialPrice.ShipmentRouteStopeId = shpRouteStops.ShippingRoutesId;
                                            objShipmentAccessorialPrice.AccessorialFeeTypeId = accPrice.AccessorialFeeTypeId;
                                            objShipmentAccessorialPrice.Unit = accPrice.Unit;
                                            objShipmentAccessorialPrice.AmtPerUnit = accPrice.AmtPerUnit;
                                            objShipmentAccessorialPrice.Amount = accPrice.Amount;
                                            objShipmentAccessorialPrice.Reason = accPrice.Reason;
                                            shipmentContext.tblShipmentAccessorialPrices.Add(objShipmentAccessorialPrice);
                                        }
                                    }

                                }
                            }

                        }
                        if (entity.ShipmentFreightDetail != null)
                        {
                            foreach (var shpFreightDetail in entity.ShipmentFreightDetail)
                            {
                                if (shpFreightDetail.ShipmentFreightDetailId > 0)
                                {
                                    if (shpFreightDetail.IsDeleted)
                                    {
                                        var freightDetail = shipmentContext.tblShipmentFreightDetails.Where(x => x.ShipmentBaseFreightDetailId == shpFreightDetail.ShipmentFreightDetailId && x.IsDeleted == false).FirstOrDefault();
                                        if (freightDetail != null)
                                        {
                                            freightDetail.IsDeleted = true;
                                            freightDetail.DeletedBy = entity.CreatedBy;
                                            freightDetail.DeletedOn = entity.CreatedDate;
                                            shipmentContext.Entry(freightDetail).State = EntityState.Modified;
                                        }
                                    }
                                    else
                                    {
                                        var freightDetail = shipmentContext.tblShipmentFreightDetails.Where(x => x.ShipmentBaseFreightDetailId == shpFreightDetail.ShipmentFreightDetailId && x.IsDeleted == false).FirstOrDefault();
                                        if (freightDetail != null)
                                        {

                                            freightDetail.Commodity = shpFreightDetail.Commodity;
                                            freightDetail.FreightTypeId = shpFreightDetail.FreightTypeId;
                                            freightDetail.PricingMethodId = shpFreightDetail.PricingMethodId;
                                            freightDetail.MinFee = shpFreightDetail.MinFee;
                                            freightDetail.UpTo = shpFreightDetail.UpTo;
                                            freightDetail.UnitPrice = shpFreightDetail.UnitPrice;
                                            freightDetail.Hazardous = shpFreightDetail.Hazardous;
                                            freightDetail.Temperature = shpFreightDetail.Temperature;
                                            freightDetail.TemperatureType = shpFreightDetail.TemperatureType;
                                            freightDetail.QuantityNweight = shpFreightDetail.QutWgtVlm;
                                            freightDetail.TotalPrice = shpFreightDetail.TotalPrice;
                                            freightDetail.Weight = shpFreightDetail.Weight;
                                            freightDetail.Unit = shpFreightDetail.Unit;
                                            freightDetail.NoOfBox = shpFreightDetail.NoOfBox;
                                            freightDetail.Comments = shpFreightDetail.Comments;
                                            freightDetail.TrailerCount = shpFreightDetail.TrailerCount;
                                            freightDetail.IsPartialShipment = shpFreightDetail.IsPartialShipment;

                                            if (shpFreightDetail.IsPartialShipment)
                                            {
                                                freightDetail.PartialPallete = shpFreightDetail.PartialPallet;
                                                freightDetail.PartialBox = shpFreightDetail.PartialBox;
                                            }
                                            else
                                            {
                                                freightDetail.PartialPallete = Convert.ToInt32(shpFreightDetail.QutWgtVlm);
                                                freightDetail.PartialBox = shpFreightDetail.NoOfBox;
                                            }

                                            shipmentContext.Entry(freightDetail).State = EntityState.Modified;

                                        }
                                    }
                                }

                            }
                        }
                        if (entity.ShipmentEquipmentNdriver != null)
                        {
                            var ShipmentEquipmentNdriver = shipmentContext.tblShipmentEquipmentNdrivers.Where(x => x.ShipmentId == entity.ShipmentId).ToList();
                            foreach (var equipmentNdriver in ShipmentEquipmentNdriver)
                            {
                                shipmentContext.tblShipmentEquipmentNdrivers.Remove(equipmentNdriver);
                            }

                            foreach (var equipmentNdriver in entity.ShipmentEquipmentNdriver)
                            {
                                tblShipmentEquipmentNdriver objshipmentEquipmentNdriver = new tblShipmentEquipmentNdriver();
                                objshipmentEquipmentNdriver.ShipmentId = entity.ShipmentId;
                                objshipmentEquipmentNdriver.DriverId = (equipmentNdriver.DriverId == 0 ? null : equipmentNdriver.DriverId);
                                objshipmentEquipmentNdriver.EquipmentId = (equipmentNdriver.EquipmentId == 0 ? null : equipmentNdriver.EquipmentId);
                                objshipmentEquipmentNdriver.CreatedDate = entity.CreatedDate;
                                objshipmentEquipmentNdriver.IsActive = true;
                                
                                shipmentContext.tblShipmentEquipmentNdrivers.Add(objshipmentEquipmentNdriver);
                            }
                        }
                        if (!string.IsNullOrEmpty(entity.DriverInstruction))
                        {
                            tblShipmentCommment shipmentCommment = new tblShipmentCommment();
                            shipmentCommment.ShipmentId = entity.ShipmentId;
                            shipmentCommment.Comment = entity.DriverInstruction;
                            shipmentCommment.CommentBy = "DP";
                            shipmentCommment.CreatedOn = entity.CreatedDate;
                            shipmentCommment.CreatedBy = entity.CreatedBy;
                            shipmentContext.tblShipmentCommments.Add(shipmentCommment);
                        }
                        shipment.IsSuccess = shipmentContext.SaveChanges() > 0;
                    }
                }
                return shipment;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region method to get freight list detail
        /// <summary>
        /// Method to get freight list detail
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<ShipmentBaseFreightDetailDTO> GetFreightList(List<ShipmentBaseFreightDetailDTO> model, long? CustomerId)
        {
            try
            {

                if (model != null)
                {

                    foreach (var freightlist in model)
                    {
                        var data = (from quote in shipmentContext.tblQuotes
                                    join routedetail in shipmentContext.tblQuoteRouteStops on quote.QuoteId equals routedetail.QuoteId
                                    join freightdetail in shipmentContext.tblCustomerBaseFreightDetails on routedetail.QuoteRouteStopsId equals freightdetail.QuoteRouteStopsId
                                    where quote.CustomerId == CustomerId
                                    && routedetail.PickupLocationId == freightlist.PickupLocationId
                                    && routedetail.DeliveryLocationId == freightlist.DeliveryLocationId
                                    && freightdetail.PricingMethodId == freightlist.PricingMethodId &&
                                    freightdetail.FreightTypeId == freightlist.FreightTypeId
                                    select new
                                    {
                                        quote.QuoteId,
                                        freightdetail.MinFee,
                                        freightdetail.Upto,
                                        freightdetail.UnitPrice,
                                    }
                                  ).OrderByDescending(x => x.QuoteId).FirstOrDefault();

                        if (data != null)
                        {
                            freightlist.MinFee = data.MinFee;
                            freightlist.UpTo = data.Upto;
                            freightlist.UnitPrice = data.UnitPrice;
                            if (freightlist.QutWgtVlm > 0 && freightlist.QutWgtVlm <= data.Upto)
                            {
                                freightlist.TotalPrice = data.MinFee;
                            }
                            else
                            {
                                freightlist.TotalPrice = data.MinFee + (freightlist.QutWgtVlm - data.Upto) * data.UnitPrice;
                            }

                        }


                    }

                }
                return model;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region method to get freight detail
        /// <summary>
        /// Method to get freight detail
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public GetShipmentFreightDetailDTO GetFreightData(GetShipmentFreightDetailDTO model)
        {
            try
            {


                if (model != null)
                {
                    if (model.QutWgtVlm > 0 && model.QutWgtVlm <= model.UpTo)
                    {
                        model.TotalPrice = model.MinFee;
                    }
                    else
                    {
                        model.TotalPrice = model.MinFee + (model.QutWgtVlm - model.UpTo) * model.UnitPrice;
                    }



                }
                return model;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {

                        new SqlParameter("@CustomerId",entity.CustomerId),
                        new SqlParameter("@PickupLocationId", entity.PickupLocationId),
                        new SqlParameter("@DeliveryLocationId", entity.DeliveryLocationId),
                         new SqlParameter("@PickupArrivalDate", entity.PickupArrivalDate),
                     };
                var result = sp_dbContext.ExecuteStoredProcedure<GetFreightTypeDTO>("usp_GetFreightType", sqlParameters);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
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
            try
            {
                var result = (from freights in shipmentContext.tblFreightTypes
                              where freights.IsActive && freights.IsDeleted == false
                              select new GetFreightTypeDTO
                              {
                                  FreightTypeId = freights.FreightTypeId,
                                  FreightTypeName = freights.FreightTypeName
                              }).ToList();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
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

            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {

                        new SqlParameter("@CustomerId",entity.CustomerId),
                        new SqlParameter("@PickupLocationId",entity.PickupLocationId),
                        new SqlParameter("@DeliveryLocationId",entity.DeliveryLocationId),
                         new SqlParameter("@PickupArrivalDate", entity.PickupArrivalDate),
                     };
                var result = sp_dbContext.ExecuteStoredProcedure<GetPricingMethodDTO>("usp_GetPricingMethod", sqlParameters);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region validate route
        /// <summary>
        /// #validate route stops
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ValidateRouteStop(ValidateRouteStopDTO entity)
        {
            var data = (from quote in shipmentContext.tblQuotes
                        join route in shipmentContext.tblQuoteRouteStops on quote.QuoteId equals route.QuoteId
                        where quote.CustomerId == entity.CustomerId && route.PickupLocationId == entity.PickupLocation && route.DeliveryLocationId == entity.DeliveryLocation
                        && DbFunctions.TruncateTime(entity.ArrivalDate) >= DbFunctions.TruncateTime(quote.QuoteDate) && DbFunctions.TruncateTime(entity.ArrivalDate) <= DbFunctions.TruncateTime(quote.ValidUptoDate)
                        select quote
                      ).ToList();

            return data.Count() > 0;

        }
        #endregion

        #region view shipment list
        /// <summary>
        /// View shipment list
        /// </summary>
        /// <returns></returns>

        public List<ViewShipmentListDTO> ViewShipmentList1(ViewShipmentDTO entity)
        {
            try
            {
                List<ViewShipmentListDTO> objShipmentList = new List<ViewShipmentListDTO>();
                var shipmentList = new List<ViewShipmentListDTO>();

                //For order taken status
                if (entity.IsOrderTaken)
                {
                    var shipmentLists = (from shipment in shipmentContext.tblShipments
                                         join customer in shipmentContext.tblCustomerRegistrations on shipment.CustomerId equals customer.CustomerID
                                         join status in shipmentContext.tblShipmentStatus on shipment.StatusId equals status.StatusId
                                         join freight in shipmentContext.tblShipmentFreightDetails on shipment.ShipmentId equals freight.ShipmentId
                                         where customer.IsDeleted == false && customer.IsActive == true && (entity.FreightTypeId > 0 ? freight.FreightTypeId == entity.FreightTypeId : 1 == 1) && (entity.CustomerId > 0 ? shipment.CustomerId == entity.CustomerId : 1 == 1) && shipment.IsDeleted == false && status.StatusName == "Order Taken"
                                         select new ViewShipmentListDTO
                                         {

                                             ShipmentId = shipment.ShipmentId,
                                             ShipmentRefNo = shipment.ShipmentRefNo ?? string.Empty,
                                             Status = status.StatusName ?? string.Empty,
                                             CustomerName = customer.CustomerName ?? string.Empty,
                                             AirWayBillNo = shipment.AirWayBill ?? string.Empty,
                                             CustomerPO = shipment.CustomerPO ?? string.Empty,
                                             PickupLocationList = (from route in shipmentContext.tblShipmentRoutesStops
                                                                   join address in shipmentContext.tblAddresses on route.PickupLocationId equals address.AddressId
                                                                   join state in shipmentContext.tblStates on address.State equals state.ID
                                                                   where address.AddressId == route.PickupLocationId && route.ShippingId == shipment.ShipmentId && route.IsDeleted == false
                                                                   select new { PickupLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + " " + address.City + " " + state.Name.ToUpper() + " " + address.Zip }).Select(x => x.PickupLocation).ToList(),
                                             PickUpDateList = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select route.PickDateTime).ToList(),
                                             DeliveryDateList = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select route.DeliveryDateTime).ToList(),
                                             DeliveryLocationList = (from route in shipmentContext.tblShipmentRoutesStops
                                                                     join address in shipmentContext.tblAddresses on route.DeliveryLocationId equals address.AddressId
                                                                     join state in shipmentContext.tblStates on address.State equals state.ID
                                                                     where address.AddressId == route.DeliveryLocationId && route.ShippingId == shipment.ShipmentId && route.IsDeleted == false
                                                                     select new { DeliveryLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + " " + address.City + " " + state.Name.ToUpper() + " " + address.Zip }).Select(x => x.DeliveryLocation).ToList(),

                                             PickUpDate = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select route.PickDateTime).FirstOrDefault(),
                                             DeliveryDate = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select new { deliverydate = route.DeliveryDateTime, routeid = route.ShippingRoutesId }).OrderByDescending(x => x.routeid).Select(x => x.deliverydate).FirstOrDefault(),
                                             Driver = (from driver in shipmentContext.tblDrivers
                                                       join equipmentNdriver in shipmentContext.tblShipmentEquipmentNdrivers on driver.DriverID equals equipmentNdriver.DriverId
                                                       where equipmentNdriver.ShipmentId == shipment.ShipmentId
                                                       select new ShipmentDriverDTO
                                                       {
                                                           DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty)
                                                       }
                                                        ).ToList(),
                                             Equipment = (from equipment in shipmentContext.tblEquipmentDetails
                                                          join equipmentNdriver in shipmentContext.tblShipmentEquipmentNdrivers on equipment.EDID equals equipmentNdriver.EquipmentId
                                                          where equipmentNdriver.ShipmentId == shipment.ShipmentId
                                                          select new ShipmentEquipmentDTO
                                                          {
                                                              EquipmentNo = equipment.EquipmentNo
                                                          }
                                                         ).ToList(),
                                             ShipmentFreightDetail = (from shipmentFreight in shipmentContext.tblShipmentFreightDetails
                                                                      where shipmentFreight.ShipmentId == shipment.ShipmentId && shipmentFreight.IsDeleted == false
                                                                      group shipmentFreight by new { shipmentFreight.ShipmentRouteStopeId } into g
                                                                      select new GetShipmentFreightDetailDTO
                                                                      {
                                                                          ShipmentRouteId = g.Key.ShipmentRouteStopeId,
                                                                          QutWgtVlm = (from data in g select data.QuantityNweight).ToList().Sum() ?? 0,
                                                                          NoOfBox = (from data in g select data.NoOfBox).ToList().Sum() ?? 0,
                                                                          NonePartialPallet = (from data in g where data.IsPartialShipment == false select data.QuantityNweight).ToList().Sum() ?? 0,
                                                                          NonePartialBox = (from data in g where data.IsPartialShipment == false select data.NoOfBox).ToList().Sum() ?? 0,
                                                                          ShipmentWeightList = (from data in g
                                                                                                group data by data.Unit into w
                                                                                                select new ShipmentBaseFreightDetailDTO
                                                                                                {
                                                                                                    Weight = (from d in w select d.Weight).ToList().Sum() ?? 0,
                                                                                                    Unit = w.Key,
                                                                                                }).ToList(),
                                                                          IsPartialShipment = (from data in g where data.IsPartialShipment select data.IsPartialShipment).FirstOrDefault(),
                                                                          PartialPallet = (from data in g select data.PartialPallete).ToList().Sum() ?? 0,
                                                                          PartialBox = (from data in g select data.PartialBox).ToList().Sum() ?? 0,
                                                                          TrailerCount = (from data in g select data.TrailerCount).ToList().Sum() ?? 0,
                                                                      }).ToList(),

                                         }
                                       ).OrderBy(x => x.ShipmentId);
                    shipmentList = shipmentLists.ToList();
                }
                else
                {
                    var shipmentLists = (from shipment in shipmentContext.tblShipments
                                         join customer in shipmentContext.tblCustomerRegistrations on shipment.CustomerId equals customer.CustomerID
                                         join status in shipmentContext.tblShipmentStatus on shipment.StatusId equals status.StatusId
                                         join freight in shipmentContext.tblShipmentFreightDetails on shipment.ShipmentId equals freight.ShipmentId
                                         where customer.IsDeleted == false && customer.IsActive == true && (entity.FreightTypeId > 0 ? freight.FreightTypeId == entity.FreightTypeId : 1 == 1) && (entity.CustomerId > 0 ? shipment.CustomerId == entity.CustomerId : 1 == 1) && shipment.IsDeleted == false && status.StatusName != "Order Taken" && status.StatusName != "Cancelled" && status.StatusName.ToLower().Trim() != ("Completed").ToLower().Trim() && status.StatusId != 11
                                         select new ViewShipmentListDTO
                                         {

                                             ShipmentId = shipment.ShipmentId,
                                             ShipmentRefNo = shipment.ShipmentRefNo ?? string.Empty,
                                             Status = status.StatusName ?? string.Empty,
                                             CustomerName = customer.CustomerName ?? string.Empty,
                                             AirWayBillNo = shipment.AirWayBill ?? string.Empty,
                                             CustomerPO = shipment.CustomerPO ?? string.Empty,
                                             PickupLocationList = (from route in shipmentContext.tblShipmentRoutesStops
                                                                   join address in shipmentContext.tblAddresses on route.PickupLocationId equals address.AddressId
                                                                   join state in shipmentContext.tblStates on address.State equals state.ID
                                                                   where address.AddressId == route.PickupLocationId && route.ShippingId == shipment.ShipmentId && route.IsDeleted == false
                                                                   select new { PickupLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + " " + address.City + " " + state.Name.ToUpper() + " " + address.Zip }).Select(x => x.PickupLocation).ToList(),
                                             PickUpDateList = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select route.PickDateTime).ToList(),
                                             DeliveryDateList = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select route.DeliveryDateTime).ToList(),

                                             DeliveryLocationList = (from route in shipmentContext.tblShipmentRoutesStops
                                                                     join address in shipmentContext.tblAddresses on route.DeliveryLocationId equals address.AddressId
                                                                     join state in shipmentContext.tblStates on address.State equals state.ID
                                                                     where address.AddressId == route.DeliveryLocationId && route.ShippingId == shipment.ShipmentId && route.IsDeleted == false
                                                                     select new { DeliveryLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + " " + address.City + " " + state.Name.ToUpper() + " " + address.Zip }).Select(x => x.DeliveryLocation).ToList(),
                                             PickUpDate = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select route.PickDateTime).FirstOrDefault(),
                                             DeliveryDate = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select new { deliverydate = route.DeliveryDateTime, routeid = route.ShippingRoutesId }).OrderByDescending(x => x.routeid).Select(x => x.deliverydate).FirstOrDefault(),
                                             Driver = (from driver in shipmentContext.tblDrivers
                                                       join equipmentNdriver in shipmentContext.tblShipmentEquipmentNdrivers on driver.DriverID equals equipmentNdriver.DriverId
                                                       where equipmentNdriver.ShipmentId == shipment.ShipmentId
                                                       select new ShipmentDriverDTO
                                                       {
                                                           DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty)
                                                       }
                                                        ).ToList(),
                                             Equipment = (from equipment in shipmentContext.tblEquipmentDetails
                                                          join equipmentNdriver in shipmentContext.tblShipmentEquipmentNdrivers on equipment.EDID equals equipmentNdriver.EquipmentId
                                                          where equipmentNdriver.ShipmentId == shipment.ShipmentId
                                                          select new ShipmentEquipmentDTO
                                                          {
                                                              EquipmentNo = equipment.EquipmentNo
                                                          }
                                                         ).ToList(),
                                             ShipmentFreightDetail = (from shipmentFreight in shipmentContext.tblShipmentFreightDetails
                                                                      where shipmentFreight.ShipmentId == shipment.ShipmentId && shipmentFreight.IsDeleted == false
                                                                      group shipmentFreight by new { shipmentFreight.ShipmentRouteStopeId } into g
                                                                      select new GetShipmentFreightDetailDTO
                                                                      {
                                                                          ShipmentRouteId = g.Key.ShipmentRouteStopeId,
                                                                          QutWgtVlm = (from data in g select data.QuantityNweight).ToList().Sum() ?? 0,
                                                                          NoOfBox = (from data in g select data.NoOfBox).ToList().Sum() ?? 0,
                                                                          NonePartialPallet = (from data in g where data.IsPartialShipment == false select data.QuantityNweight).ToList().Sum() ?? 0,
                                                                          NonePartialBox = (from data in g where data.IsPartialShipment == false select data.NoOfBox).ToList().Sum() ?? 0,
                                                                          ShipmentWeightList = (from data in g
                                                                                                group data by data.Unit into w
                                                                                                select new ShipmentBaseFreightDetailDTO
                                                                                                {
                                                                                                    Weight = (from d in w select d.Weight).ToList().Sum() ?? 0,
                                                                                                    Unit = w.Key,
                                                                                                }).ToList(),
                                                                          IsPartialShipment = (from data in g where data.IsPartialShipment select data.IsPartialShipment).FirstOrDefault(),
                                                                          PartialPallet = (from data in g select data.PartialPallete).ToList().Sum() ?? 0,
                                                                          PartialBox = (from data in g select data.PartialBox).ToList().Sum() ?? 0,
                                                                          TrailerCount = (from data in g select data.TrailerCount).ToList().Sum() ?? 0,
                                                                      }).ToList(),
                                             //NoOfBox = (from shipmentFreight in shipmentContext.tblShipmentFreightDetails
                                             //           where shipmentFreight.ShipmentId == shipment.ShipmentId && shipmentFreight.IsDeleted == false
                                             //           select shipmentFreight.NoOfBox).ToList().Sum(),
                                             //PartialPallete = (from shipmentFreight in shipmentContext.tblShipmentFreightDetails
                                             //                  where shipmentFreight.ShipmentId == shipment.ShipmentId && shipmentFreight.IsDeleted == false && shipmentFreight.IsPartialShipment && shipmentFreight.PartialPallete != null
                                             //                  select shipmentFreight.PartialPallete).ToList().Sum() ?? 0,
                                             //PartilalBox = (from shipmentFreight in shipmentContext.tblShipmentFreightDetails
                                             //               where shipmentFreight.ShipmentId == shipment.ShipmentId && shipmentFreight.IsDeleted == false && shipmentFreight.IsPartialShipment && shipmentFreight.PartialBox != null
                                             //               select shipmentFreight.PartialBox).ToList().Sum() ?? 0,

                                             //Quantity = (from shipmentFreight in shipmentContext.tblShipmentFreightDetails
                                             //            where shipmentFreight.ShipmentId == shipment.ShipmentId && shipmentFreight.IsDeleted == false && shipmentFreight.QuantityNweight > 0
                                             //            select shipmentFreight.QuantityNweight
                                             //         ).ToList().Sum(),
                                             //Weight = (from shipmentFreight in shipmentContext.tblShipmentFreightDetails
                                             //          where shipmentFreight.ShipmentId == shipment.ShipmentId && shipmentFreight.IsDeleted == false && shipmentFreight.Weight > 0
                                             //          group shipmentFreight by new { shipmentFreight.Unit } into g
                                             //          select new ShipmentBaseFreightDetailDTO
                                             //          {
                                             //              Weight = g.Select(x => x.Weight).ToList().Sum() ?? 0,
                                             //              Unit = g.Key.Unit
                                             //          }
                                             //         ).ToList(),
                                             //TrailerCount = (from shipmentFreight in shipmentContext.tblShipmentFreightDetails
                                             //                where shipmentFreight.ShipmentId == shipment.ShipmentId && shipmentFreight.IsDeleted == false && shipmentFreight.TrailerCount > 0
                                             //                select shipmentFreight.TrailerCount
                                             //         ).ToList().Sum(),
                                         }
                                        ).OrderByDescending(x => x.ShipmentId);
                    shipmentList = shipmentLists.ToList();
                }
                if (entity.StartDate != null && entity.EndDate != null)
                {
                    foreach (var shipment in shipmentList)
                    {
                        //shipment.PickUpLocation = string.Join(",", (from pk in shipment.PickupLocationList
                        //                                            group pk by pk into g
                        //                                            select g.Key).ToList());
                        shipment.DriverName = string.Join(",", shipment.Driver.Select(x => x.DriverName));
                        shipment.EquipmentNo = string.Join(",", shipment.Equipment.Select(x => x.EquipmentNo));


                        //shipment.QutVolWgt = (shipment.Quantity > 0 ? shipment.Quantity + " " + PricingMethod.Pallet : string.Empty);
                        // shipment.Weights = string.Join(", ", shipment.Weight.Select(x => (x.Weight + " " + x.Unit)));
                        if (shipment.PickUpDateList.Count > 0)
                        {
                            for (int i = 0; i < shipment.PickUpDateList.Count; i++)
                            {
                                shipment.PickUpDateList[i] = Configurations.ConvertDateTime(Convert.ToDateTime(shipment.PickUpDateList[i]));
                            }

                        }
                        if (shipment.DeliveryDateList.Count > 0)
                        {
                            for (int i = 0; i < shipment.DeliveryLocationList.Count; i++)
                            {
                                shipment.DeliveryDateList[i] = Configurations.ConvertDateTime(Convert.ToDateTime(shipment.DeliveryDateList[i]));
                            }

                        }
                        shipment.PickUpLocation = string.Join("|", (from pk in shipment.PickupLocationList
                                                                    select pk).ToList());
                        shipment.DeliveryLocation = string.Join("|", (from dk in shipment.DeliveryLocationList
                                                                      select dk).ToList());
                        if (shipment.ShipmentFreightDetail.Count > 0)
                        {
                            string palletBoxWeith = string.Empty;
                            foreach (var freightDetail in shipment.ShipmentFreightDetail)
                            {
                                if (freightDetail.IsPartialShipment)
                                {
                                    palletBoxWeith += (freightDetail.PartialPallet > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + "/" + (freightDetail.PartialPallet + freightDetail.NonePartialPallet).ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : (freightDetail.QutWgtVlm > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : string.Empty));
                                    palletBoxWeith += (freightDetail.PartialBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + "/" + (freightDetail.PartialBox + freightDetail.NonePartialBox).ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : (freightDetail.NoOfBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + " " + PricingMethod.BXS) : string.Empty + ", "));

                                }
                                else
                                {
                                    palletBoxWeith += (freightDetail.QutWgtVlm > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : string.Empty);
                                    palletBoxWeith += (freightDetail.NoOfBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : string.Empty);
                                }
                                string weight = string.Empty;
                                if (freightDetail.ShipmentWeightList.Count > 0)
                                {
                                    foreach (var weights in freightDetail.ShipmentWeightList)
                                    {
                                        weight += (weights.Weight > 0 ? (weights.Weight.ToString().Replace(".00", "") + " " + weights.Unit + ", ") : string.Empty);
                                    }
                                }
                                palletBoxWeith += weight; ///(freightDetail.Weight > 0 ? (freightDetail.Weight.ToString().Replace(".00", "") + " " + freightDetail.Unit + ", ") : string.Empty);
                                palletBoxWeith += freightDetail.TrailerCount > 0 ? (freightDetail.TrailerCount.ToString().Replace(".00", "") + " " + PricingMethod.Trailer) : string.Empty;
                                palletBoxWeith = palletBoxWeith.Trim().Trim(',');
                                palletBoxWeith += "<br/>";
                            }

                            shipment.QutVolWgt = palletBoxWeith;
                        }
                        //   shipment.PickUpDate = Configurations.ConvertDateTime(Convert.ToDateTime(shipment.PickUpDate));
                        //  shipment.DeliveryDate = Configurations.ConvertDateTime(Convert.ToDateTime(shipment.DeliveryDate));
                        if (Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(shipment.PickUpDate)).Date >= entity.StartDate.Value.Date && Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(shipment.PickUpDate)).Date <= entity.EndDate.Value.Date)
                        {
                            objShipmentList.Add(shipment);
                        }


                    }

                    return objShipmentList.ToList();
                }
                else
                {

                    foreach (var shipment in shipmentList)
                    {
                        shipment.DriverName = string.Join(",", shipment.Driver.Select(x => x.DriverName));
                        shipment.EquipmentNo = string.Join(",", shipment.Equipment.Select(x => x.EquipmentNo));
                        //shipment.QutVolWgt = (shipment.Quantity > 0 ? shipment.Quantity + " " + PricingMethod.Pallet : string.Empty);
                        if (shipment.PickUpDateList.Count > 0)
                        {
                            for (int i = 0; i < shipment.PickUpDateList.Count; i++)
                            {
                                shipment.PickUpDateList[i] = Configurations.ConvertDateTime(Convert.ToDateTime(shipment.PickUpDateList[i]));
                            }

                        }
                        if (shipment.DeliveryDateList.Count > 0)
                        {
                            for (int i = 0; i < shipment.DeliveryLocationList.Count; i++)
                            {
                                shipment.DeliveryDateList[i] = Configurations.ConvertDateTime(Convert.ToDateTime(shipment.DeliveryDateList[i]));
                            }

                        }
                        if (shipment.ShipmentFreightDetail.Count > 0)
                        {
                            string palletBoxWeith = string.Empty;
                            foreach (var freightDetail in shipment.ShipmentFreightDetail)
                            {
                                if (freightDetail.IsPartialShipment)
                                {
                                    palletBoxWeith += (freightDetail.PartialPallet > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + "/" + (freightDetail.PartialPallet + freightDetail.NonePartialPallet).ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : (freightDetail.QutWgtVlm > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : string.Empty));
                                    palletBoxWeith += (freightDetail.PartialBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + "/" + (freightDetail.PartialBox + freightDetail.NonePartialBox).ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : (freightDetail.NoOfBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : string.Empty));

                                }
                                else
                                {
                                    palletBoxWeith += (freightDetail.QutWgtVlm > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : string.Empty);
                                    palletBoxWeith += (freightDetail.NoOfBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : string.Empty);
                                }
                                string weight = string.Empty;
                                if (freightDetail.ShipmentWeightList.Count > 0)
                                {
                                    foreach (var weights in freightDetail.ShipmentWeightList)
                                    {
                                        weight += (weights.Weight > 0 ? (weights.Weight.ToString().Replace(".00", "") + " " + weights.Unit + ", ") : string.Empty);
                                    }
                                }
                                palletBoxWeith += weight; ///(freightDetail.Weight > 0 ? (freightDetail.Weight.ToString().Replace(".00", "") + " " + freightDetail.Unit + ", ") : string.Empty);
                                palletBoxWeith += freightDetail.TrailerCount > 0 ? (freightDetail.TrailerCount.ToString().Replace(".00", "") + " " + PricingMethod.Trailer) : string.Empty;
                                palletBoxWeith = palletBoxWeith.Trim().Trim(',');
                                palletBoxWeith += "<br/>";
                            }

                            shipment.QutVolWgt = palletBoxWeith;
                        }
                        //shipment.Weights = string.Join(", ", shipment.Weight.Select(x => (x.Weight + " " + x.Unit)));
                        shipment.PickUpLocation = string.Join("|", (from pk in shipment.PickupLocationList
                                                                    select pk).ToList());
                        shipment.DeliveryLocation = string.Join("|", (from dk in shipment.DeliveryLocationList
                                                                      select dk).ToList());
                    }
                }
                shipmentList = shipmentList.GroupBy(x => x.ShipmentId).Select(g => g.First()).ToList();
                return shipmentList.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<List<AllShipmentList>> ViewShipmentList(AllShipmentDTO entity)
        {
            var result = new List<AllShipmentList>();
            try
            {
                var totalCount = new SqlParameter
                {
                    ParameterName = "@TotalCount",
                    Value = 0,
                    Direction = ParameterDirection.Output
                };

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SearchTerm", entity.SearchTerm),
                        new SqlParameter("@SortColumn", entity.SortColumn),
                        new SqlParameter("@SortOrder", entity.SortOrder),
                        new SqlParameter("@PageNumber", entity.PageNumber),
                        new SqlParameter("@PageSize", entity.PageSize)
                    };


                if (entity.IsOrderTaken)
                {
                    result = await Task.FromResult(sp_dbContext.ExecuteStoredProcedure<AllShipmentList>("usp_GetShipment_OrderTaken_List", sqlParameters));
                }
                else
                {
                    result = await Task.FromResult(sp_dbContext.ExecuteStoredProcedure<AllShipmentList>("usp_GetShipment_NotOrderTaken_List", sqlParameters));
                }

                entity.TotalCount = result != null && result.Count() > 0 ? result.Select(x => x.TotalCount).FirstOrDefault() : 0;
                return result != null && result.Count() > 0 ? result.ToList() : new List<AllShipmentList>();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region get shipment 
        /// <summary>
        ///  get shipment by id for edit shipment
        /// </summary>
        /// <returns></returns>a
        public GetShipmentDTO GetShipmentById(int shipmentId)
        {
            try
            {

                var shipmentdetail = (from shipment in shipmentContext.tblShipments
                                      join customer in shipmentContext.tblCustomerRegistrations on shipment.CustomerId equals customer.CustomerID
                                      join status in shipmentContext.tblShipmentStatus on shipment.StatusId equals status.StatusId
                                      where shipment.ShipmentId == shipmentId
                                      select new GetShipmentDTO
                                      {
                                          IsWaitingTimeRequired = shipmentContext.tblCustomerRegistrations.Where(x => x.CustomerID == shipment.CustomerId).Select(x => x.IsWaitingTimeRequired).FirstOrDefault(),
                                          ContactInfoCount = shipmentContext.tblCustomerContacts.Where(x => x.CustomerId == shipment.CustomerId).Count(),
                                          ShipmentId = shipment.ShipmentId,
                                          CustomerId = customer.CustomerID,
                                          CustomerName = customer.CustomerName,
                                          StatusId = shipment.StatusId,
                                          StatusName = status.StatusName ?? string.Empty,
                                          SubStatusId = shipment.SubStatusId,
                                          RequestedBy = shipment.RequestedBy,
                                          Reason = shipment.Reason ?? string.Empty,
                                          ShipmentRefNo = shipment.ShipmentRefNo,
                                          AirWayBill = shipment.AirWayBill ?? string.Empty,
                                          CustomerPO = shipment.CustomerPO ?? string.Empty,
                                          OrderNo = shipment.OrderNo ?? string.Empty,
                                          CustomerRef = shipment.CustomerRef ?? string.Empty,
                                          ContainerNo = shipment.ContainerNo ?? string.Empty,
                                          PurchaseDoc = shipment.PurchaseDoc ?? string.Empty,
                                          DriverInstruction = shipment.DriverInstruction ?? string.Empty,
                                          VendorNconsignee = shipment.VendorNconsignee,
                                          ShipmentComments = (from comment in shipmentContext.tblShipmentCommments
                                                              where comment.ShipmentId == shipmentId
                                                              select new ShipmentCommentDTO
                                                              {
                                                                  comment = (comment.CommentBy + " :- " + comment.Comment)
                                                              }
                                                            ).ToList(),
                                          ShipmentRoutesStop = (from shipmentRouteStops in shipmentContext.tblShipmentRoutesStops
                                                                where shipmentRouteStops.ShippingId == shipmentId && shipmentRouteStops.IsDeleted == false
                                                                select new GetShipmentRouteStopDTO
                                                                {
                                                                    ShipmentRouteStopId = shipmentRouteStops.ShippingRoutesId,
                                                                    RouteNo = shipmentRouteStops.RouteNo,
                                                                    PickupLocationId = shipmentRouteStops.PickupLocationId,
                                                                    PickupLocation = (from address in shipmentContext.tblAddresses
                                                                                      join state in shipmentContext.tblStates on address.State equals state.ID
                                                                                      where address.AddressId == shipmentRouteStops.PickupLocationId
                                                                                      select new { PicUpLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName.Trim() + ",")) + address.Address1 + " " + address.City + " " + state.Name.ToUpper() + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault(),
                                                                    PickDateTime = shipmentRouteStops.PickDateTime,
                                                                    PickDateTimeTo = shipmentRouteStops.PickUpDateTimeTo,
                                                                    DeliveryLocationId = shipmentRouteStops.DeliveryLocationId,
                                                                    DeliveryLocation = (from address in shipmentContext.tblAddresses
                                                                                        join state in shipmentContext.tblStates on address.State equals state.ID
                                                                                        where address.AddressId == shipmentRouteStops.DeliveryLocationId
                                                                                        select new { PicUpLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName.Trim() + ",")) + address.Address1 + " " + address.City + " " + state.Name.ToUpper() + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault(),
                                                                    DeliveryDateTime = shipmentRouteStops.DeliveryDateTime,
                                                                    DeliveryDateTimeTo = shipmentRouteStops.DeliveryDateTimeTo,
                                                                    Comment = shipmentRouteStops.Comment,
                                                                    IsDeleted = shipmentRouteStops.IsDeleted,
                                                                    ActPickupArrival = shipmentRouteStops.DriverPickupArrival,
                                                                    ActPickupDeparture = shipmentRouteStops.DriverPickupDeparture,
                                                                    ActDeliveryArrival = shipmentRouteStops.DriverDeliveryArrival,
                                                                    ActDeliveryDeparture = shipmentRouteStops.DriverDeliveryDeparture,
                                                                    ReceiverName = shipmentRouteStops.ReceiverName,
                                                                    DigitalSignature = shipmentRouteStops.DigitalSignature,
                                                                    IsAppointmentNeeded = shipmentContext.tblAddresses.Where(x => x.AddressId == shipmentRouteStops.DeliveryLocationId).Select(x => x.IsAppointmentRequired).FirstOrDefault(),
                                                                    IsAppointmentRequired = shipmentRouteStops.IsAppointmentRequired,
                                                                    IsWaitingTimeNeeded = shipmentContext.tblCustomerRegistrations.Where(x => x.CustomerID == customer.CustomerID).Select(x => x.IsWaitingTimeRequired).FirstOrDefault(),
                                                                    IsPickUpWaitingTimeRequired = shipmentRouteStops.IsPickUpWaitingTimeRequired,
                                                                    IsDeliveryWaitingTimeRequired = shipmentRouteStops.IsDeliveryWaitingTimeRequired,
                                                                    Website = shipmentContext.tblAddresses.Where(x => x.AddressId == shipmentRouteStops.DeliveryLocationId).Select(x => x.Website).FirstOrDefault(),
                                                                    Phone = shipmentContext.tblAddresses.Where(x => x.AddressId == shipmentRouteStops.DeliveryLocationId).Select(x => x.Phone).FirstOrDefault(),
                                                                }
                                                              ).ToList(),
                                          ShipmentFreightDetail = (from shipmentFreightDetail in shipmentContext.tblShipmentFreightDetails
                                                                   join shipmentRoutesStops in shipmentContext.tblShipmentRoutesStops on shipmentFreightDetail.ShipmentRouteStopeId equals shipmentRoutesStops.ShippingRoutesId
                                                                   where shipmentFreightDetail.ShipmentId == shipmentId && shipmentRoutesStops.IsDeleted == false && shipmentFreightDetail.IsDeleted == false
                                                                   select new GetShipmentFreightDetailDTO
                                                                   {
                                                                       MinFee = shipmentFreightDetail.MinFee,
                                                                       UpTo = shipmentFreightDetail.UpTo,
                                                                       UnitPrice = shipmentFreightDetail.UnitPrice,
                                                                       TotalPrice = shipmentFreightDetail.TotalPrice,
                                                                       ShipmentFreightDetailId = shipmentFreightDetail.ShipmentBaseFreightDetailId,
                                                                       ShipmentId = shipmentFreightDetail.ShipmentId,
                                                                       RouteNo = shipmentRoutesStops.RouteNo,
                                                                       Commodity = shipmentFreightDetail.Commodity ?? string.Empty,
                                                                       FreightTypeId = shipmentFreightDetail.FreightTypeId,
                                                                       FreightType = (from freight in shipmentContext.tblFreightTypes where freight.FreightTypeId == shipmentFreightDetail.FreightTypeId select freight.FreightTypeName.ToUpper()).FirstOrDefault() ?? string.Empty,
                                                                       PricingMethodId = shipmentFreightDetail.PricingMethodId,
                                                                       PricingMethod = (from pricingmethod in shipmentContext.tblPricingMethods where pricingmethod.PricingMethodId == shipmentFreightDetail.PricingMethodId select pricingmethod.PricingMethodName.ToUpper()).FirstOrDefault() ?? string.Empty,
                                                                       QutWgtVlm = shipmentFreightDetail.QuantityNweight ?? 0,
                                                                       Temperature = shipmentFreightDetail.Temperature,
                                                                       Hazardous = shipmentFreightDetail.Hazardous,
                                                                       PickupLocationId = shipmentRoutesStops.PickupLocationId,
                                                                       DeliveryLocationId = shipmentRoutesStops.DeliveryLocationId,
                                                                       PcsTypeId = shipmentFreightDetail.PcsType,
                                                                       NoOfBox = shipmentFreightDetail.NoOfBox ?? 0,
                                                                       Weight = shipmentFreightDetail.Weight ?? 0,
                                                                       Unit = shipmentFreightDetail.Unit ?? string.Empty,
                                                                       TrailerCount = shipmentFreightDetail.TrailerCount ?? 0,
                                                                       Comments = shipmentFreightDetail.Comments ?? string.Empty,
                                                                       IsPartialShipment = shipmentFreightDetail.IsPartialShipment,
                                                                       PartialBox = shipmentFreightDetail.PartialBox ?? 0,
                                                                       PartialPallet = shipmentFreightDetail.PartialPallete ?? 0
                                                                   }
                                                                 ).ToList(),
                                          AccessorialPrice = (from accessorialprice in shipmentContext.tblShipmentAccessorialPrices
                                                              join shipmentRoutesStops in shipmentContext.tblShipmentRoutesStops on accessorialprice.ShipmentRouteStopeId equals shipmentRoutesStops.ShippingRoutesId
                                                              where accessorialprice.ShipmentId == shipmentId && shipmentRoutesStops.IsDeleted == false && accessorialprice.IsDeleted == false
                                                              select new GetShipmentAccessorialPriceDTO
                                                              {
                                                                  Reason = accessorialprice.Reason,
                                                                  ShipmentAccessorialPriceId = accessorialprice.ShipmentAccessorialPriceId,
                                                                  RouteNo = shipmentRoutesStops.RouteNo,
                                                                  AccessorialFeeTypeId = accessorialprice.AccessorialFeeTypeId,
                                                                  Unit = accessorialprice.Unit,
                                                                  AmtPerUnit = accessorialprice.AmtPerUnit,
                                                                  Amount = accessorialprice.Amount,
                                                                  FeeType = (from aft in shipmentContext.tblAccessorialFeesTypes where aft.Id == accessorialprice.AccessorialFeeTypeId select aft.PricingMethod).FirstOrDefault(),
                                                              }
                                                            ).ToList(),
                                          DamageImages = (from damageImage in shipmentContext.tblDamagedImages
                                                          join shipmentRouteStops in shipmentContext.tblShipmentRoutesStops on damageImage.ShipmentRouteID equals shipmentRouteStops.ShippingRoutesId
                                                          where shipmentRouteStops.ShippingId == shipmentId && damageImage.IsDeleted == false
                                                          select new GetDamageImages
                                                          {
                                                              ShipmentRouteStopId = shipmentRouteStops.ShippingRoutesId,
                                                              DamageId = damageImage.DamagedID,
                                                              RouteNo = shipmentRouteStops.RouteNo,
                                                              ImageName = damageImage.ImageName,
                                                              ImageDescription = damageImage.ImageDescription,
                                                              ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + damageImage.ImageUrl),
                                                              CreatedOn = damageImage.CreatedOn,
                                                              IsApproved = damageImage.IsApproved,
                                                          }
                                                          ).ToList(),

                                          ProofOfTemprature = (from proofoftemp in shipmentContext.tblProofOfTemperatureImages
                                                               join freightdetial in shipmentContext.tblShipmentFreightDetails on proofoftemp.ShipmentFreightDetailId equals freightdetial.ShipmentBaseFreightDetailId
                                                               join shipmentRouteStops in shipmentContext.tblShipmentRoutesStops on freightdetial.ShipmentRouteStopeId equals shipmentRouteStops.ShippingRoutesId
                                                               where shipmentRouteStops.ShippingId == shipmentId && proofoftemp.IsDeleted == false
                                                               select new GetProofOfTemprature
                                                               {
                                                                   ProofOfTempratureId = proofoftemp.ImageId,
                                                                   FreightDetailId = freightdetial.ShipmentBaseFreightDetailId,
                                                                   ImageName = proofoftemp.ImageName,
                                                                   ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + proofoftemp.ImageUrl),
                                                                   CreatedOn = proofoftemp.CreatedOn,
                                                                   ActualTemperature = proofoftemp.ActualTemperature,
                                                                   IsApproved = proofoftemp.IsApproved,
                                                                   IsLoading = proofoftemp.IsLoading
                                                               }
                                                             ).ToList(),
                                          ShipmentEquipmentNdriver = (from eqpNdriver in shipmentContext.tblShipmentEquipmentNdrivers
                                                                          //join driver in shipmentContext.tblDrivers on eqpNdriver.DriverId equals driver.DriverID
                                                                          //join equipmnet in shipmentContext.tblEquipmentDetails on eqpNdriver.EquipmentId equals equipmnet.EDID
                                                                      where eqpNdriver.ShipmentId == shipmentId
                                                                      select new GetShipmentEquipmentNDriverDTO
                                                                      {

                                                                          ShipmentEquipmentNDriverId = eqpNdriver.ShipmentEquipmentNdriverId,
                                                                          DriverId = eqpNdriver.DriverId,
                                                                          //DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty),
                                                                          EquipmentId = eqpNdriver.EquipmentId,
                                                                          //EquipmentName = equipmnet.EquipmentNo,
                                                                          //DriverId = eqpNdriver.DriverId,
                                                                        
                                                                          DriverName = (from driver in shipmentContext.tblDrivers
                                                                                        where eqpNdriver.DriverId == driver.DriverID
                                                                                        select new
                                                                                        {
                                                                                            DriverName =
                                                                                        (driver.FirstName + " " + (driver.LastName ?? string.Empty))
                                                                                        }).Select(x => x.DriverName).FirstOrDefault() ?? string.Empty,
                                                                          //EquipmentId = eqpNdriver.EquipmentId,
                                                                          EquipmentName = (from equipment in shipmentContext.tblEquipmentDetails
                                                                                           where eqpNdriver.EquipmentId == equipment.EDID
                                                                                           select equipment.EquipmentNo).FirstOrDefault() ?? string.Empty,
                                                                      }
                                                                    ).ToList(),
                                      }

                                    ).FirstOrDefault();
                if (shipmentdetail != null)
                {
                    var shipmentStatusHistory = (shipmentContext.tblShipmentStatusHistories.Where(x => x.ShipmentId == shipmentdetail.ShipmentId).OrderByDescending(x => x.ShipmentStatusHistoryId).FirstOrDefault());

                    if (shipmentStatusHistory != null)
                    {
                        if (shipmentStatusHistory.StatusId != shipmentdetail.StatusId)
                        {
                            shipmentdetail.IsOnHold = true;
                        }
                    }
                    foreach (var routestop in shipmentdetail.ShipmentRoutesStop)
                    {
                        routestop.PickDateTime = routestop.PickDateTime == null ? routestop.PickDateTime : Configurations.ConvertDateTime(Convert.ToDateTime(routestop.PickDateTime));
                        routestop.PickDateTimeTo = routestop.PickDateTimeTo == null ? routestop.PickDateTimeTo : Configurations.ConvertDateTime(Convert.ToDateTime(routestop.PickDateTimeTo));
                        routestop.DeliveryDateTime = routestop.DeliveryDateTime == null ? routestop.DeliveryDateTime : Configurations.ConvertDateTime(Convert.ToDateTime(routestop.DeliveryDateTime));
                        routestop.DeliveryDateTimeTo = routestop.DeliveryDateTimeTo == null ? routestop.DeliveryDateTimeTo : Configurations.ConvertDateTime(Convert.ToDateTime(routestop.DeliveryDateTimeTo));
                        routestop.ActPickupArrival = routestop.ActPickupArrival == null ? routestop.ActPickupArrival : Configurations.ConvertDateTime(Convert.ToDateTime(routestop.ActPickupArrival));
                        routestop.ActPickupDeparture = routestop.ActPickupDeparture == null ? routestop.ActPickupDeparture : Configurations.ConvertDateTime(Convert.ToDateTime(routestop.ActPickupDeparture));
                        routestop.ActDeliveryArrival = routestop.ActDeliveryArrival == null ? routestop.ActDeliveryArrival : Configurations.ConvertDateTime(Convert.ToDateTime(routestop.ActDeliveryArrival));
                        routestop.ActDeliveryDeparture = routestop.ActDeliveryDeparture == null ? routestop.ActDeliveryDeparture : Configurations.ConvertDateTime(Convert.ToDateTime(routestop.ActDeliveryDeparture));


                    }
                    foreach (var profoftemp in shipmentdetail.ProofOfTemprature)
                    {
                        profoftemp.CreatedOn = profoftemp.CreatedOn == null ? profoftemp.CreatedOn : Configurations.ConvertDateTime(Convert.ToDateTime(profoftemp.CreatedOn));
                    }
                    foreach (var damage in shipmentdetail.DamageImages)
                    {
                        damage.CreatedOn = damage.CreatedOn == null ? damage.CreatedOn : Configurations.ConvertDateTime(Convert.ToDateTime(damage.CreatedOn));
                    }
                }


                return shipmentdetail;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Upload Damage Document
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        public int UploadDamageDocument(List<GetDamageImages> damageImageList)
        {
            try
            {


                var result = 0;
                if (damageImageList != null && damageImageList.Count > 0)
                {

                    foreach (var file in damageImageList)
                    {
                        tblDamagedImage objDamagedImage = new tblDamagedImage();
                        objDamagedImage.ShipmentRouteID = file.ShipmentRouteStopId;
                        objDamagedImage.ImageName = file.ImageName;
                        objDamagedImage.ImageDescription = file.ImageDescription;
                        objDamagedImage.ImageUrl = file.ImageUrl;
                        objDamagedImage.CreatedBy = file.CreatedBy;
                        objDamagedImage.CreatedOn = file.CreatedOn;
                        objDamagedImage.IsApproved = true;
                        objDamagedImage.ApprovedBy = file.CreatedBy;
                        objDamagedImage.ApprovedOn = file.CreatedOn;
                        shipmentContext.tblDamagedImages.Add(objDamagedImage);
                        result = shipmentContext.SaveChanges() > 0 ? objDamagedImage.DamagedID : 0;
                    }
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Upload Proof of Temp
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        public int UploadProofofTempDocument(List<GetProofOfTemprature> proofofTempraturesList)
        {
            try
            {


                var result = 0;
                if (proofofTempraturesList != null && proofofTempraturesList.Count > 0)
                {
                    foreach (var file in proofofTempraturesList)
                    {
                        tblProofOfTemperatureImage proofofTempImage = new tblProofOfTemperatureImage();
                        proofofTempImage.ShipmentRouteId = file.ShipmentRouteStopId;
                        proofofTempImage.ShipmentFreightDetailId = file.FreightDetailId;
                        proofofTempImage.ActualTemperature = file.ActualTemperature;
                        proofofTempImage.ImageName = file.ImageName;
                        proofofTempImage.ImageUrl = file.ImageUrl;
                        proofofTempImage.CreatedBy = file.CreatedBy;
                        proofofTempImage.CreatedOn = file.CreatedOn;
                        proofofTempImage.IsApproved = true;
                        proofofTempImage.ApprovedBy = file.CreatedBy;
                        proofofTempImage.ApprovedOn = file.CreatedOn;
                        shipmentContext.tblProofOfTemperatureImages.Add(proofofTempImage);
                        result = shipmentContext.SaveChanges() > 0 ? proofofTempImage.ImageId : 0;
                    }
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {


                GetShipmentRouteStopDTO objShipmetnRouteStops = new GetShipmentRouteStopDTO();
                if (model != null && model.ShipmentId > 0)
                {

                    tblShipmentRoutesStop objShipmentRouteStops = new tblShipmentRoutesStop();
                    objShipmentRouteStops.ShippingId = model.ShipmentId;
                    objShipmentRouteStops.RouteNo = model.RouteNo;
                    objShipmentRouteStops.PickupLocationId = model.PickupLocationId;
                    objShipmentRouteStops.PickDateTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.PickDateTime));
                    objShipmentRouteStops.PickUpDateTimeTo = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.PickDateTimeTo));
                    objShipmentRouteStops.DeliveryLocationId = model.DeliveryLocationId;
                    objShipmentRouteStops.DeliveryDateTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.DeliveryDateTime));
                    objShipmentRouteStops.DeliveryDateTimeTo = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.DeliveryDateTimeTo));
                    objShipmentRouteStops.Comment = model.Comment;
                    objShipmentRouteStops.IsAppointmentRequired = model.IsAppointmentRequired;
                    objShipmentRouteStops.IsPickUpWaitingTimeRequired = model.IsPickUpWaitingTimeRequired;
                    objShipmentRouteStops.IsDeliveryWaitingTimeRequired = model.IsDeliveryWaitingTimeRequired;


                    shipmentContext.tblShipmentRoutesStops.Add(objShipmentRouteStops);
                    objShipmetnRouteStops.IsSuccess = shipmentContext.SaveChanges() > 0;
                    objShipmetnRouteStops.ShipmentRouteStopId = objShipmentRouteStops.ShippingRoutesId;

                }
                return objShipmetnRouteStops;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {


                GetShipmentFreightDetailDTO objShipmentFreightDetail = new GetShipmentFreightDetailDTO();
                tblShipmentFreightDetail objShipmentFreight = new tblShipmentFreightDetail();
                var getfreightData = GetSingleFreightData(model, model.CustomerId);
                if (getfreightData != null)
                {
                    objShipmentFreight.ShipmentId = model.ShipmentId;
                    objShipmentFreight.ShipmentRouteStopeId = model.ShipmentRouteId;
                    objShipmentFreight.Commodity = model.Commodity;
                    objShipmentFreight.FreightTypeId = model.FreightTypeId;
                    objShipmentFreight.PricingMethodId = model.PricingMethodId;
                    objShipmentFreight.MinFee = getfreightData.MinFee;
                    objShipmentFreight.UpTo = getfreightData.UpTo;
                    objShipmentFreight.UnitPrice = getfreightData.UnitPrice;
                    objShipmentFreight.Hazardous = model.Hazardous;
                    objShipmentFreight.Temperature = model.Temperature;
                    objShipmentFreight.TemperatureType = model.TemperatureType;
                    objShipmentFreight.Weight = model.Weight;
                    objShipmentFreight.Unit = model.Unit;
                    objShipmentFreight.NoOfBox = model.NoOfBox;
                    objShipmentFreight.TrailerCount = model.TrailerCount;
                    objShipmentFreight.Comments = model.Comments;
                    objShipmentFreight.QuantityNweight = model.QutWgtVlm;
                    objShipmentFreight.TotalPrice = getfreightData.TotalPrice;
                    objShipmentFreight.IsPartialShipment = getfreightData.IsPartialShipment;
                    objShipmentFreight.PartialBox = getfreightData.PartialBox;
                    objShipmentFreight.PartialPallete = getfreightData.PartialPallet;
                    shipmentContext.tblShipmentFreightDetails.Add(objShipmentFreight);
                    objShipmentFreightDetail.IsSuccess = shipmentContext.SaveChanges() > 0;
                    objShipmentFreightDetail.ShipmentFreightDetailId = objShipmentFreight.ShipmentBaseFreightDetailId;
                }
                return objShipmentFreightDetail;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region method to get single freight detail
        /// <summary>
        /// Method to get freight detail
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public GetShipmentFreightDetailDTO GetSingleFreightData(GetShipmentFreightDetailDTO model, long? CustomerId)
        {
            try
            {


                if (model != null)
                {

                    var data = (from quote in shipmentContext.tblQuotes
                                join routedetail in shipmentContext.tblQuoteRouteStops on quote.QuoteId equals routedetail.QuoteId
                                join freightdetail in shipmentContext.tblCustomerBaseFreightDetails on routedetail.QuoteRouteStopsId equals freightdetail.QuoteRouteStopsId
                                where quote.CustomerId == CustomerId
                                && routedetail.PickupLocationId == model.PickupLocationId
                                && routedetail.DeliveryLocationId == model.DeliveryLocationId
                                && freightdetail.PricingMethodId == model.PricingMethodId &&
                                freightdetail.FreightTypeId == model.FreightTypeId
                                select new
                                {
                                    quote.QuoteId,
                                    freightdetail.MinFee,
                                    freightdetail.Upto,
                                    freightdetail.UnitPrice,
                                }
                              ).OrderByDescending(x => x.QuoteId).FirstOrDefault();

                    if (data != null)
                    {
                        model.MinFee = data.MinFee;
                        model.UpTo = data.Upto;
                        model.UnitPrice = data.UnitPrice;
                        if (model.QutWgtVlm > 0 && model.QutWgtVlm <= data.Upto)
                        {
                            model.TotalPrice = data.MinFee;
                        }
                        else
                        {
                            model.TotalPrice = data.MinFee + (model.QutWgtVlm - data.Upto) * data.UnitPrice;
                        }

                    }




                }
                return model;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                bool result = false;
                if (model != null)
                {
                    var proofofTempImage = shipmentContext.tblProofOfTemperatureImages.Where(x => x.ImageId == model.ProofOfTempratureId).FirstOrDefault();
                    if (proofofTempImage != null)
                    {
                        proofofTempImage.IsDeleted = true;
                        proofofTempImage.DeletedBy = model.CreatedBy;
                        proofofTempImage.DeletedOn = model.CreatedOn;
                        shipmentContext.Entry(proofofTempImage).State = EntityState.Modified;
                        result = shipmentContext.SaveChanges() > 0;
                    }

                }
                return result;

            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                bool result = false;
                if (model != null)
                {
                    var damageImage = shipmentContext.tblDamagedImages.Where(x => x.DamagedID == model.DamageId).FirstOrDefault();
                    if (damageImage != null)
                    {
                        damageImage.IsDeleted = true;
                        damageImage.DeletedBy = model.CreatedBy;
                        damageImage.DeletedOn = model.CreatedOn;
                        shipmentContext.Entry(damageImage).State = EntityState.Modified;
                        result = shipmentContext.SaveChanges() > 0;
                    }

                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Genrate shipment reference no 
        /// <summary>
        /// Genrate shipment reference no
        /// </summary>
        /// <returns></returns>

        public string GenrateShipmentRefNo()
        {
            try
            {


                string preLable = "SRN_";
                string shipmentRefNo = string.Empty;
                var lastShipmentId = shipmentContext.tblShipments.OrderByDescending(x => x.ShipmentId).Select(x => x.ShipmentId).FirstOrDefault();
                if (lastShipmentId > 0)
                {
                    shipmentRefNo = preLable + (1000 + lastShipmentId + 1).ToString();

                }
                else
                {
                    shipmentRefNo = preLable + (1000 + 1).ToString();
                }
                return shipmentRefNo;
            }
            catch (Exception)
            {

                throw;
            }

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
            try
            {
                bool result = false;
                var table = shipmentContext.tblShipments.Find(entity.ShipmentId);
                if (table != null)
                {
                    table.IsDeleted = true;
                    table.DeletedBy = entity.CreatedBy;
                    table.DeletedOn = entity.CreatedDate;
                    shipmentContext.Entry(table).State = System.Data.Entity.EntityState.Modified;
                    result = shipmentContext.SaveChanges() > 0;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {


                ShipmentDTO shipment = new ShipmentDTO();
                if (entity.ShipmentId > 0)
                {

                    var shipmentdata = shipmentContext.tblShipments.Where(x => x.ShipmentId == entity.ShipmentId).FirstOrDefault();
                    if (shipmentdata != null)
                    {
                        shipmentdata.StatusId = entity.StatusId;
                        shipmentdata.SubStatusId = entity.SubStatusId;
                        shipmentdata.Reason = entity.Reason;
                        shipmentdata.ModifiedBy = entity.CreatedBy;
                        shipmentdata.ModifiedDate = entity.CreatedDate;
                        shipmentContext.Entry(shipmentdata).State = EntityState.Modified;


                        tblShipmentStatusHistory shipmentStatusHistory = new tblShipmentStatusHistory();
                        shipmentStatusHistory.ShipmentId = entity.ShipmentId;
                        shipmentStatusHistory.StatusId = Convert.ToInt32(entity.StatusId);
                        shipmentStatusHistory.SubStatusId = entity.SubStatusId;
                        shipmentStatusHistory.Reason = entity.Reason;
                        shipmentStatusHistory.CreatedBy = entity.CreatedBy;
                        shipmentStatusHistory.CreatedOn = entity.CreatedDate;
                        shipmentContext.tblShipmentStatusHistories.Add(shipmentStatusHistory);

                        tblShipmentEventHistory shipmentEventHistory = new tblShipmentEventHistory();
                        shipmentEventHistory.ShipmentId = entity.ShipmentId;
                        shipmentEventHistory.StatusId = entity.StatusId;
                        shipmentEventHistory.UserId = entity.CreatedBy;
                        shipmentEventHistory.Event = "STATUS";
                        shipmentEventHistory.EventDateTime = entity.CreatedDate;
                        shipmentContext.tblShipmentEventHistories.Add(shipmentEventHistory);

                    }
                    shipment.IsSuccess = shipmentContext.SaveChanges() > 0;
                }
                return shipment;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region view all shipment list
        /// <summary>
        /// View All shipment list
        /// </summary>
        /// <returns></returns>

        public List<ViewShipmentListDTO> ViewAllShipmentList(ViewShipmentDTO entity, out int recordsTotal)
        {
            try
            {
                // var data1 = AllShipmentList(entity);
                List<ViewShipmentListDTO> objShipmentList = new List<ViewShipmentListDTO>();
                var shipmentList = new List<ViewShipmentListDTO>();

                var shipmentLists = (from shipment in shipmentContext.tblShipments
                                     join customer in shipmentContext.tblCustomerRegistrations on shipment.CustomerId equals customer.CustomerID
                                     join status in shipmentContext.tblShipmentStatus on shipment.StatusId equals status.StatusId
                                     join freight in shipmentContext.tblShipmentFreightDetails on shipment.ShipmentId equals freight.ShipmentId
                                     where customer.IsDeleted == false && customer.IsActive == true && (entity.FreightTypeId > 0 ? freight.FreightTypeId == entity.FreightTypeId : 1 == 1) && (entity.CustomerId > 0 ? shipment.CustomerId == entity.CustomerId : 1 == 1) && shipment.IsDeleted == false && (entity.StatusId > 0 ? shipment.StatusId == entity.StatusId : 1 == 1) && (shipment.StatusId == 11 || shipment.StatusId == 8)
                                     // && (entity.search != "" ? (status.StatusName.ToUpper().Contains(entity.search.ToUpper()) || customer.CustomerName.ToUpper().Contains(entity.search.ToUpper())
                                     // || shipment.AirWayBill.ToUpper().Contains(entity.search.ToUpper()) || shipment.CustomerPO.ToUpper().Contains(entity.search.ToUpper())) : 1 == 1)
                                     select new ViewShipmentListDTO
                                     {

                                         ShipmentId = shipment.ShipmentId,
                                         ShipmentRefNo = shipment.ShipmentRefNo ?? string.Empty,
                                         Status = status.StatusName ?? string.Empty,
                                         CustomerName = customer.CustomerName ?? string.Empty,
                                         AirWayBillNo = shipment.AirWayBill ?? string.Empty,
                                         CustomerPO = shipment.CustomerPO ?? string.Empty,
                                         PickupLocationList = (from route in shipmentContext.tblShipmentRoutesStops
                                                               join address in shipmentContext.tblAddresses on route.PickupLocationId equals address.AddressId
                                                               join state in shipmentContext.tblStates on address.State equals state.ID
                                                               where address.AddressId == route.PickupLocationId && route.ShippingId == shipment.ShipmentId && route.IsDeleted == false
                                                               select new { PickupLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + " " + address.City + " " + state.Name.ToUpper() + " " + address.Zip }).Select(x => x.PickupLocation).ToList(),
                                         PickUpDateList = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select route.PickDateTime).ToList(),
                                         DeliveryDateList = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select route.DeliveryDateTime).ToList(),

                                         DeliveryLocationList = (from route in shipmentContext.tblShipmentRoutesStops
                                                                 join address in shipmentContext.tblAddresses on route.DeliveryLocationId equals address.AddressId
                                                                 join state in shipmentContext.tblStates on address.State equals state.ID
                                                                 where address.AddressId == route.DeliveryLocationId && route.ShippingId == shipment.ShipmentId && route.IsDeleted == false
                                                                 select new { DeliveryLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + " " + address.City + " " + state.Name.ToUpper() + " " + address.Zip }).Select(x => x.DeliveryLocation).ToList(),
                                         PickUpDate = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select route.PickDateTime).FirstOrDefault(),
                                         DeliveryDate = (from route in shipmentContext.tblShipmentRoutesStops where route.ShippingId == shipment.ShipmentId && route.IsDeleted == false select new { deliverydate = route.DeliveryDateTime, routeid = route.ShippingRoutesId }).OrderByDescending(x => x.routeid).Select(x => x.deliverydate).FirstOrDefault(),
                                         Driver = (from driver in shipmentContext.tblDrivers
                                                   join equipmentNdriver in shipmentContext.tblShipmentEquipmentNdrivers on driver.DriverID equals equipmentNdriver.DriverId
                                                   where equipmentNdriver.ShipmentId == shipment.ShipmentId
                                                   select new ShipmentDriverDTO
                                                   {
                                                       DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty)
                                                   }
                                                        ).ToList(),
                                         Equipment = (from equipment in shipmentContext.tblEquipmentDetails
                                                      join equipmentNdriver in shipmentContext.tblShipmentEquipmentNdrivers on equipment.EDID equals equipmentNdriver.EquipmentId
                                                      where equipmentNdriver.ShipmentId == shipment.ShipmentId
                                                      select new ShipmentEquipmentDTO
                                                      {
                                                          EquipmentNo = equipment.EquipmentNo
                                                      }
                                                         ).ToList(),
                                         ShipmentFreightDetail = (from shipmentFreight in shipmentContext.tblShipmentFreightDetails
                                                                  where shipmentFreight.ShipmentId == shipment.ShipmentId && shipmentFreight.IsDeleted == false
                                                                  group shipmentFreight by new { shipmentFreight.ShipmentRouteStopeId } into g
                                                                  select new GetShipmentFreightDetailDTO
                                                                  {
                                                                      ShipmentRouteId = g.Key.ShipmentRouteStopeId,
                                                                      QutWgtVlm = (from data in g select data.QuantityNweight).ToList().Sum() ?? 0,
                                                                      NoOfBox = (from data in g select data.NoOfBox).ToList().Sum() ?? 0,
                                                                      NonePartialPallet = (from data in g where data.IsPartialShipment == false select data.QuantityNweight).ToList().Sum() ?? 0,
                                                                      NonePartialBox = (from data in g where data.IsPartialShipment == false select data.NoOfBox).ToList().Sum() ?? 0,
                                                                      ShipmentWeightList = (from data in g
                                                                                            group data by data.Unit into w
                                                                                            select new ShipmentBaseFreightDetailDTO
                                                                                            {
                                                                                                Weight = (from d in w select d.Weight).ToList().Sum() ?? 0,
                                                                                                Unit = w.Key,
                                                                                            }).ToList(),
                                                                      IsPartialShipment = (from data in g where data.IsPartialShipment select data.IsPartialShipment).FirstOrDefault(),
                                                                      PartialPallet = (from data in g select data.PartialPallete).ToList().Sum() ?? 0,
                                                                      PartialBox = (from data in g select data.PartialBox).ToList().Sum() ?? 0,
                                                                      TrailerCount = (from data in g select data.TrailerCount).ToList().Sum() ?? 0,
                                                                  }).ToList(),

                                     }
                                      ).OrderByDescending(x => x.ShipmentId);
                shipmentList = shipmentLists.ToList();


                if (entity.StartDate != null && entity.EndDate != null)
                {
                    foreach (var shipment in shipmentList)
                    {

                        shipment.DriverName = string.Join(",", shipment.Driver.Select(x => x.DriverName));
                        shipment.EquipmentNo = string.Join(",", shipment.Equipment.Select(x => x.EquipmentNo));

                        if (shipment.PickUpDateList.Count > 0)
                        {
                            for (int i = 0; i < shipment.PickUpDateList.Count; i++)
                            {
                                shipment.PickUpDateList[i] = Configurations.ConvertDateTime(Convert.ToDateTime(shipment.PickUpDateList[i]));
                            }

                        }
                        if (shipment.DeliveryDateList.Count > 0)
                        {
                            for (int i = 0; i < shipment.DeliveryLocationList.Count; i++)
                            {
                                shipment.DeliveryDateList[i] = Configurations.ConvertDateTime(Convert.ToDateTime(shipment.DeliveryDateList[i]));
                            }

                        }
                        shipment.PickUpLocation = string.Join("|", (from pk in shipment.PickupLocationList
                                                                    select pk).ToList());
                        shipment.DeliveryLocation = string.Join("|", (from dk in shipment.DeliveryLocationList
                                                                      select dk).ToList());
                        if (shipment.ShipmentFreightDetail.Count > 0)
                        {
                            string palletBoxWeith = string.Empty;
                            foreach (var freightDetail in shipment.ShipmentFreightDetail)
                            {
                                if (freightDetail.IsPartialShipment)
                                {
                                    palletBoxWeith += (freightDetail.PartialPallet > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + "/" + (freightDetail.PartialPallet + freightDetail.NonePartialPallet).ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : (freightDetail.QutWgtVlm > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : string.Empty));
                                    palletBoxWeith += (freightDetail.PartialBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + "/" + (freightDetail.PartialBox + freightDetail.NonePartialBox).ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : (freightDetail.NoOfBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : string.Empty));

                                }
                                else
                                {
                                    palletBoxWeith += (freightDetail.QutWgtVlm > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : string.Empty);
                                    palletBoxWeith += (freightDetail.NoOfBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : string.Empty);
                                }
                                string weight = string.Empty;
                                if (freightDetail.ShipmentWeightList.Count > 0)
                                {
                                    foreach (var weights in freightDetail.ShipmentWeightList)
                                    {
                                        weight += (weights.Weight > 0 ? (weights.Weight.ToString().Replace(".00", "") + " " + weights.Unit + ", ") : string.Empty);
                                    }
                                }
                                palletBoxWeith += weight;
                                palletBoxWeith += freightDetail.TrailerCount > 0 ? (freightDetail.TrailerCount.ToString().Replace(".00", "") + " " + PricingMethod.Trailer) : string.Empty;
                                palletBoxWeith = palletBoxWeith.Trim().Trim(',');
                                palletBoxWeith += "<br/>";
                            }

                            shipment.QutVolWgt = palletBoxWeith;
                        }

                        if (Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(shipment.PickUpDate)).Date >= entity.StartDate.Value.Date && Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(shipment.PickUpDate)).Date <= entity.EndDate.Value.Date)
                        {
                            objShipmentList.Add(shipment);
                        }


                    }
                    if (!string.IsNullOrEmpty(entity.search))
                    {
                        objShipmentList = objShipmentList.Where(x =>
                        x.Status.ToUpper().Contains(entity.search.ToUpper()) ||
                       x.CustomerName.ToUpper().Contains(entity.search.ToUpper()) ||
                        x.PickUpLocation.ToUpper().Contains(entity.search.ToUpper()) ||
                          x.DeliveryLocation.ToUpper().Contains(entity.search.ToUpper()) ||
                           x.AirWayBillNo.ToUpper().Contains(entity.search.ToUpper()) ||
                           x.CustomerPO.ToUpper().Contains(entity.search.ToUpper()) ||
                              x.DriverName.ToUpper().Contains(entity.search.ToUpper()) ||
                               x.EquipmentNo.ToUpper().Contains(entity.search.ToUpper()) ||
                                x.QutVolWgt.ToUpper().Contains(entity.search.ToUpper())

                       ).ToList();
                    }

                    objShipmentList = objShipmentList.GroupBy(x => x.ShipmentId).Select(g => g.First()).ToList();
                    recordsTotal = objShipmentList.Count();
                    if (!(string.IsNullOrEmpty(entity.sortColumn) && string.IsNullOrEmpty(entity.sortColumnDir)))
                    {
                        objShipmentList = entity.sortColumnDir == "asc" ? objShipmentList.OrderBy(x => x.GetType().GetProperty(entity.sortColumn).GetValue(x, null)).ToList() : objShipmentList.OrderByDescending(x => x.GetType().GetProperty(entity.sortColumn).GetValue(x, null)).ToList();
                        objShipmentList = objShipmentList.Skip(entity.skip).Take(entity.pageSize).ToList();
                    }

                    return objShipmentList.ToList();
                }
                else
                {
                    //var shipmentList1 = shipmentList.Select(x => new ViewShipmentListDTO()
                    //{
                    //    DriverName = string.Join(",", x.Driver.Select(y => y.DriverName)),
                    //    EquipmentNo = string.Join(",", x.Equipment.Select(y => y.EquipmentNo)),
                    //    //PickUpDateList= new 

                    //}).ToList();
                    foreach (var shipment in shipmentList)
                    {
                        shipment.DriverName = string.Join(",", shipment.Driver.Select(x => x.DriverName));
                        shipment.EquipmentNo = string.Join(",", shipment.Equipment.Select(x => x.EquipmentNo));
                        if (shipment.PickUpDateList.Count > 0)
                        {
                            for (int i = 0; i < shipment.PickUpDateList.Count; i++)
                            {
                                shipment.PickUpDateList[i] = Configurations.ConvertDateTime(Convert.ToDateTime(shipment.PickUpDateList[i]));
                            }

                        }
                        if (shipment.DeliveryDateList.Count > 0)
                        {
                            for (int i = 0; i < shipment.DeliveryLocationList.Count; i++)
                            {
                                shipment.DeliveryDateList[i] = Configurations.ConvertDateTime(Convert.ToDateTime(shipment.DeliveryDateList[i]));
                            }

                        }
                        if (shipment.ShipmentFreightDetail.Count > 0)
                        {
                            string palletBoxWeith = string.Empty;
                            foreach (var freightDetail in shipment.ShipmentFreightDetail)
                            {
                                if (freightDetail.IsPartialShipment)
                                {
                                    palletBoxWeith += (freightDetail.PartialPallet > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + "/" + (freightDetail.PartialPallet + freightDetail.NonePartialPallet).ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : (freightDetail.QutWgtVlm > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : string.Empty));
                                    palletBoxWeith += (freightDetail.PartialBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + "/" + (freightDetail.PartialBox + freightDetail.NonePartialBox).ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : (freightDetail.NoOfBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : string.Empty));

                                }
                                else
                                {
                                    palletBoxWeith += (freightDetail.QutWgtVlm > 0 ? (freightDetail.QutWgtVlm.ToString().Replace(".00", "") + " " + PricingMethod.PLTS + ", ") : string.Empty);
                                    palletBoxWeith += (freightDetail.NoOfBox > 0 ? (freightDetail.NoOfBox.ToString().Replace(".00", "") + " " + PricingMethod.BXS + ", ") : string.Empty);
                                }
                                string weight = string.Empty;
                                if (freightDetail.ShipmentWeightList.Count > 0)
                                {
                                    foreach (var weights in freightDetail.ShipmentWeightList)
                                    {
                                        weight += (weights.Weight > 0 ? (weights.Weight.ToString().Replace(".00", "") + " " + weights.Unit + ", ") : string.Empty);
                                    }
                                }
                                palletBoxWeith += weight;
                                palletBoxWeith += freightDetail.TrailerCount > 0 ? (freightDetail.TrailerCount.ToString().Replace(".00", "") + " " + PricingMethod.Trailer) : string.Empty;
                                palletBoxWeith = palletBoxWeith.Trim().Trim(',');
                                palletBoxWeith += "<br/>";
                            }

                            shipment.QutVolWgt = palletBoxWeith;
                        }

                        shipment.PickUpLocation = string.Join("|", (from pk in shipment.PickupLocationList
                                                                    select pk).ToList());
                        shipment.DeliveryLocation = string.Join("|", (from dk in shipment.DeliveryLocationList
                                                                      select dk).ToList());
                    }
                }
                if (!string.IsNullOrEmpty(entity.search))
                {
                    shipmentList = shipmentList.Where(x =>
                    x.Status.ToUpper().Contains(entity.search.ToUpper()) ||
                    x.CustomerName.ToUpper().Contains(entity.search.ToUpper()) ||
                    x.PickUpLocation.ToUpper().Contains(entity.search.ToUpper()) ||
                    x.DeliveryLocation.ToUpper().Contains(entity.search.ToUpper()) ||
                    x.AirWayBillNo.ToUpper().Contains(entity.search.ToUpper()) ||
                    x.CustomerPO.ToUpper().Contains(entity.search.ToUpper()) ||
                    x.DriverName.ToUpper().Contains(entity.search.ToUpper()) ||
                    x.EquipmentNo.ToUpper().Contains(entity.search.ToUpper()) ||
                    x.QutVolWgt.ToUpper().Contains(entity.search.ToUpper())

                   ).ToList();
                }

                shipmentList = shipmentList.GroupBy(x => x.ShipmentId).Select(g => g.First()).ToList();
                recordsTotal = shipmentList.Count();
                if (!(string.IsNullOrEmpty(entity.sortColumn) && string.IsNullOrEmpty(entity.sortColumnDir)))
                {
                    shipmentList = entity.sortColumnDir == "asc" ? shipmentList.OrderBy(x => x.GetType().GetProperty(entity.sortColumn).GetValue(x, null)).ToList() : shipmentList.OrderByDescending(x => x.GetType().GetProperty(entity.sortColumn).GetValue(x, null)).ToList();
                    shipmentList = shipmentList.Skip(entity.skip).Take(entity.pageSize).ToList();
                }
                return shipmentList.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public IList<AllShipmentList> AllShipmentList(AllShipmentDTO entity)
        {
            try
            {
                //var data = ViewShipmentList1(entity);
                var totalCount = new SqlParameter
                {
                    ParameterName = "@TotalCount",
                    Value = 0,
                    Direction = ParameterDirection.Output
                };

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SearchTerm", entity.SearchTerm),
                        new SqlParameter("@SortColumn", entity.SortColumn),
                        new SqlParameter("@SortOrder", entity.SortOrder),
                        new SqlParameter("@PageNumber", entity.PageNumber),
                        new SqlParameter("@PageSize", entity.PageSize),
                        new SqlParameter("@StartDate", entity.StartDate),
                        new SqlParameter("@EndDate", entity.EndDate),
                        new SqlParameter("@CustomerId", entity.CustomerId),
                        new SqlParameter("@FreightTypeId", entity.FreightTypeId),
                        new SqlParameter("@StatusId", entity.StatusId),
                            new SqlParameter("@DriverName", entity.DriverName),
                                            };

                var result = sp_dbContext.ExecuteStoredProcedure<AllShipmentList>("usp_GetShipmentList_New", sqlParameters);
                entity.TotalCount = result != null && result.Count() > 0 ? result.Select(x => x.TotalCount).FirstOrDefault() : 0;
                return result != null && result.Count() > 0 ? result.ToList() : new List<AllShipmentList>();
            }
            catch (Exception)
            {

                throw;
            }

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
            try
            {
                var customerDetail = new ShipmentEmailDTO();
                EncryptDecrypt ObjEncryptDecrypt = new EncryptDecrypt();
                customerDetail = (from shipment in shipmentContext.tblShipments
                                  join customer in shipmentContext.tblCustomerRegistrations on shipment.CustomerId equals customer.CustomerID
                                  join user in shipmentContext.tblUsers on customer.UserId equals user.Userid
                                  join status in shipmentContext.tblShipmentStatus on shipment.StatusId equals status.StatusId
                                  where (customerShipmentDTO.ShipmentId > 0 ? customerShipmentDTO.ShipmentId == shipment.ShipmentId : 1 == 1) && (customerShipmentDTO.CustomerId > 0 ? customerShipmentDTO.CustomerId == shipment.CustomerId : 1 == 1)
                                  select new ShipmentEmailDTO
                                  {

                                      ShipmentId = shipment.ShipmentId,
                                      CustomerName = customer.CustomerName.ToUpper() ?? string.Empty,

                                      ContactPersons = (from contactPerson in shipmentContext.tblCustomerContacts
                                                        where contactPerson.CustomerId == customer.CustomerID
                                                        select new CustomerContact
                                                        {
                                                            ContactEmail = contactPerson.ContactEmail,
                                                        }
                                                         ).ToList(),
                                      CustomerMail = user.UserName ?? string.Empty,
                                      Consignee = shipment.VendorNconsignee.ToUpper() ?? string.Empty,
                                      ShipmentRefNo = shipment.ShipmentRefNo,
                                      Status = status.StatusName ?? string.Empty,
                                      StatusId = shipment.StatusId,
                                      SubStatus = shipmentContext.tblShipmentSubStatus.Where(x => x.SubStatusId == shipment.SubStatusId).Select(x => x.SubStatusName).FirstOrDefault() ?? string.Empty,
                                      AirWayBill = shipment.AirWayBill ?? string.Empty,
                                      OrderNo = shipment.OrderNo ?? string.Empty,
                                      CustomerPO = shipment.CustomerPO ?? string.Empty,
                                      OrderTaken = shipmentContext.tblShipmentRoutesStops.Where(x => x.ShippingId == shipment.ShipmentId).Select(x => x.PickDateTime).FirstOrDefault(),
                                      ESTDateTime = shipmentContext.tblShipmentRoutesStops.Where(x => x.ShippingId == shipment.ShipmentId).OrderByDescending(x => x.ShippingRoutesId).Select(x => x.DeliveryDateTime).FirstOrDefault(),
                                      ShipmentStatusHistory = (from shipmenthistory in shipmentContext.tblShipmentStatusHistories
                                                               join status in shipmentContext.tblShipmentStatus on shipmenthistory.StatusId equals status.StatusId
                                                               where shipmenthistory.ShipmentId == shipment.ShipmentId && shipmenthistory.StatusId != 1
                                                               select new ShipmentStatusHistoryDTO
                                                               {
                                                                   ShipmentStatusHistoryId = shipmenthistory.ShipmentStatusHistoryId,
                                                                   StatusId = shipmenthistory.StatusId,
                                                                   DateTime = shipmenthistory.CreatedOn,
                                                                   Status = status.StatusName ?? string.Empty,
                                                                   SubStatusId = shipmenthistory.SubStatusId,
                                                                   Reason = shipmenthistory.Reason,
                                                                   SubStatus = shipmentContext.tblShipmentSubStatus.Where(x => x.SubStatusId == shipmenthistory.SubStatusId).Select(x => x.SubStatusName).FirstOrDefault() ?? string.Empty,
                                                                   Colour = status.Colour,
                                                                   ImageUrl = (LarastruckingApp.Entities.Common.Configurations.ImageURL + status.ImageURL)
                                                               }
                                                             ).OrderBy(x => x.ShipmentStatusHistoryId).ToList(),
                                      ShipmentStatusList = (from status in shipmentContext.tblShipmentStatus
                                                            where status.StatusId != 1 && status.StatusId != 3 && status.StatusId != 4 && status.StatusId != 8 && status.StatusId != 9 && status.StatusId != 10 && status.StatusId != 11 && status.StatusId != 13
                                                            select new ShipmentStatusDTO
                                                            {
                                                                DisplayOrder = status.DisplayOrder,
                                                                StatusId = status.StatusId,
                                                                GrayImageURL = (LarastruckingApp.Entities.Common.Configurations.ImageURL + status.GrayImageURL),
                                                            }
                                                         ).OrderBy(x => x.DisplayOrder).ToList(),
                                      AccessorialPrice = (from accessorialprice in shipmentContext.tblShipmentAccessorialPrices
                                                          where accessorialprice.ShipmentId == shipment.ShipmentId && accessorialprice.IsDeleted == false && accessorialprice.IsDeleted == false
                                                          select new GetShipmentAccessorialPriceDTO
                                                          {
                                                              Amount = accessorialprice.Amount,
                                                              FeeType = (from aft in shipmentContext.tblAccessorialFeesTypes where aft.Id == accessorialprice.AccessorialFeeTypeId select (accessorialprice.AccessorialFeeTypeId == 23 ? accessorialprice.Reason : aft.Name)).FirstOrDefault(),
                                                          }
                                                        ).ToList(),
                                      ShipmentFreightDetail = (from shipmentFreightDetail in shipmentContext.tblShipmentFreightDetails
                                                               where shipmentFreightDetail.ShipmentId == shipment.ShipmentId && shipmentFreightDetail.IsDeleted == false && shipmentFreightDetail.IsDeleted == false
                                                               select new GetShipmentFreightDetailDTO
                                                               {
                                                                   ShipmentRouteId = shipmentFreightDetail.ShipmentRouteStopeId,
                                                                   PickupLocation = (from shipmentRouteStops in shipmentContext.tblShipmentRoutesStops
                                                                                     join address in shipmentContext.tblAddresses on shipmentRouteStops.PickupLocationId equals address.AddressId
                                                                                     where shipmentRouteStops.ShippingRoutesId == shipmentFreightDetail.ShipmentRouteStopeId
                                                                                     select new
                                                                                     {
                                                                                         PickupLocation = (address.CompanyName + " | " + address.City),

                                                                                     }).Select(x => x.PickupLocation).FirstOrDefault(),

                                                                   DeliveryLocation = (from shipmentRouteStops in shipmentContext.tblShipmentRoutesStops
                                                                                       join address in shipmentContext.tblAddresses on shipmentRouteStops.DeliveryLocationId equals address.AddressId
                                                                                       where shipmentRouteStops.ShippingRoutesId == shipmentFreightDetail.ShipmentRouteStopeId
                                                                                       select new
                                                                                       {
                                                                                           DeliveryLocation = (address.CompanyName + " | " + address.City),

                                                                                       }).Select(x => x.DeliveryLocation).FirstOrDefault(),

                                                                   Commodity = shipmentFreightDetail.Commodity ?? string.Empty,
                                                                   FreightType = (from freight in shipmentContext.tblFreightTypes where freight.FreightTypeId == shipmentFreightDetail.FreightTypeId select freight.FreightTypeName).FirstOrDefault() ?? string.Empty,
                                                                   QutWgtVlm = shipmentFreightDetail.QuantityNweight ?? 0,
                                                                   Temperature = shipmentFreightDetail.Temperature,
                                                                   Hazardous = shipmentFreightDetail.Hazardous,
                                                                   NoOfBox = shipmentFreightDetail.NoOfBox ?? 0,
                                                                   Weight = shipmentFreightDetail.Weight ?? 0,
                                                                   Unit = shipmentFreightDetail.Unit ?? string.Empty,
                                                                   TrailerCount = shipmentFreightDetail.TrailerCount ?? 0,

                                                               }
                                                             ).ToList(),
                                      ShipmentEquipmentNdriver = (from eqpNdriver in shipmentContext.tblShipmentEquipmentNdrivers
                                                                  where eqpNdriver.ShipmentId == shipment.ShipmentId
                                                                  select new GetShipmentEquipmentNDriverDTO
                                                                  {
                                                                      ShipmentEquipmentNDriverId = eqpNdriver.ShipmentEquipmentNdriverId,
                                                                      DriverId = eqpNdriver.DriverId,
                                                                      EquipmentId = eqpNdriver.EquipmentId,
                                                                      DriverName = (from driver in shipmentContext.tblDrivers
                                                                                    where eqpNdriver.DriverId == driver.DriverID
                                                                                    select new
                                                                                    {
                                                                                        DriverName =
                                                                                    (driver.FirstName + " " + (driver.LastName ?? string.Empty))
                                                                                    }).Select(x => x.DriverName).FirstOrDefault() ?? string.Empty,
                                                                      EquipmentName = (from equipment in shipmentContext.tblEquipmentDetails
                                                                                       where eqpNdriver.EquipmentId == equipment.EDID
                                                                                       select equipment.EquipmentNo).FirstOrDefault() ?? string.Empty,
                                                                  }
                                                                    ).ToList(),

                                      ProofOfTemprature = (from proofoftemp in shipmentContext.tblProofOfTemperatureImages
                                                           join freightdetial in shipmentContext.tblShipmentFreightDetails on proofoftemp.ShipmentFreightDetailId equals freightdetial.ShipmentBaseFreightDetailId
                                                           join shipmentRouteStops in shipmentContext.tblShipmentRoutesStops on freightdetial.ShipmentRouteStopeId equals shipmentRouteStops.ShippingRoutesId
                                                           where shipmentRouteStops.ShippingId == shipment.ShipmentId && proofoftemp.IsDeleted == false && freightdetial.IsDeleted == false && shipmentRouteStops.IsDeleted == false
                                                           select new GetProofOfTemprature
                                                           {
                                                               IsApproved = proofoftemp.IsApproved,
                                                               ProofOfTempratureId = proofoftemp.ImageId,
                                                               ShipmentRouteStopId = shipmentRouteStops.ShippingRoutesId,
                                                               FreightDetailId = freightdetial.ShipmentBaseFreightDetailId,
                                                               ImageName = proofoftemp.ImageName,
                                                               ImageUrl = proofoftemp.ImageUrl,
                                                               CreatedOn = proofoftemp.CreatedOn,
                                                               IsLoading = proofoftemp.IsLoading,
                                                               ActualTemperature = proofoftemp.ActualTemperature
                                                           }
                                                         ).OrderByDescending(x => x.CreatedOn).ToList(),
                                      DamageImages = (from damageImage in shipmentContext.tblDamagedImages
                                                      join shipmentRouteStops in shipmentContext.tblShipmentRoutesStops on damageImage.ShipmentRouteID equals shipmentRouteStops.ShippingRoutesId
                                                      where shipmentRouteStops.ShippingId == shipment.ShipmentId && damageImage.IsDeleted == false && shipmentRouteStops.IsDeleted == false
                                                      select new GetDamageImages
                                                      {
                                                          IsApproved = damageImage.IsApproved,
                                                          ImageName = damageImage.ImageName,
                                                          ImageUrl = damageImage.ImageUrl
                                                      }).ToList(),
                                      ShipmentRoutesStop = (from shipmentRouteStops in shipmentContext.tblShipmentRoutesStops
                                                            where shipmentRouteStops.ShippingId == shipment.ShipmentId
                                                            && shipmentRouteStops.DigitalSignature != null && shipmentRouteStops.IsDeleted == false
                                                            select new GetShipmentRouteStopDTO
                                                            {

                                                                SignatureDateTime = shipmentRouteStops.DriverDeliveryDeparture,
                                                                ReceiverName = shipmentRouteStops.ReceiverName,
                                                                DigitalSignature = shipmentRouteStops.DigitalSignature,
                                                                DigitalSignaturePath = (LarastruckingApp.Entities.Common.Configurations.ImageURL + shipmentRouteStops.DigitalSignaturePath)
                                                            }
                                                              ).ToList(),


                                  }
                                   ).OrderByDescending(x => x.ShipmentId).FirstOrDefault();

                if (customerDetail != null)
                {
                    foreach (var proofOfTemp in customerDetail.ProofOfTemprature)
                    {
                        proofOfTemp.Ext = proofOfTemp.ImageName.Split('.')[1];
                        proofOfTemp.ImageUrl = (LarastruckingApp.Entities.Common.Configurations.ImageURL + proofOfTemp.ImageUrl);
                    }
                    foreach (var damageDoc in customerDetail.DamageImages)
                    {
                        damageDoc.Ext = damageDoc.ImageName.Split('.')[1];
                        damageDoc.ImageUrl = (LarastruckingApp.Entities.Common.Configurations.ImageURL + damageDoc.ImageUrl);
                    }
                    customerDetail.StatusGrayDotPath = LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingResource.StatusDotsGrayImage;
                    customerDetail.StatusDotPath = LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingResource.StatusDotsImage;
                    if (customerDetail.ContactPersons.Count > 0)
                    {
                        customerDetail.AllContactPerson = string.Join(",", customerDetail.ContactPersons.Select(x => x.ContactEmail).ToList());
                    }
                    else
                    {
                        customerDetail.AllContactPerson = customerDetail.CustomerMail;
                    }

                    customerDetail.LogoURL = (LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingApp.Entities.Common.Configurations.LarasLogo);
                    customerDetail.ESTDateTime = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(customerDetail.ESTDateTime));
                    customerDetail.OrderTaken = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(customerDetail.OrderTaken));

                    customerDetail.ActualTemp = string.Join(", ", (from AT in customerDetail.ProofOfTemprature
                                                                   group AT by AT.FreightDetailId into i
                                                                   select new GetProofOfTemprature
                                                                   {
                                                                       ActualTemperature = (from t in i select t.ActualTemperature).FirstOrDefault()
                                                                   }
                              ).ToList().Select(x => x.ActualTemperature));

                    customerDetail.AccessorialPrice = (from accessorialprice in customerDetail.AccessorialPrice
                                                       group accessorialprice by accessorialprice.FeeType into acc
                                                       select new GetShipmentAccessorialPriceDTO
                                                       {
                                                           FeeType = acc.Key,
                                                           Amount = (from a in acc where a.FeeType == acc.Key select a.Amount).Sum()
                                                       }
                                                     ).ToList();
                    if (customerDetail.ShipmentFreightDetail.Count > 0)
                    {

                        string temprature = string.Empty;
                        string damageDetail = string.Empty;
                        int tempNo = 1;
                        int demNo = 1;
                        List<GetShipmentFreightDetailDTO> getShipmentFreightDetailList = new List<GetShipmentFreightDetailDTO>();
                        foreach (var routs in customerDetail.ShipmentFreightDetail.GroupBy(x => x.ShipmentRouteId).ToList().OrderBy(x => x.Key).Select(x => x.Key))
                        {
                            var proofOfTempList = customerDetail.ProofOfTemprature.ToList().Where(x => x.ShipmentRouteStopId == routs && x.IsLoading == true).GroupBy(x => x.ActualTemperature).Select(x => x.Key).ToList();
                            if (proofOfTempList.Count > 0)
                            {
                                foreach (var temp in proofOfTempList)
                                {
                                    var unit = decimal.TryParse(temp, out decimal n) == true ? " F |" : " |";

                                    if (customerDetail.ProofOfTemprature.Count == 1)
                                    {
                                        temprature += " Temp : " + temp + unit;
                                    }
                                    else
                                    {
                                        temprature += " Temp " + tempNo + ": " + temp + unit;
                                    }
                                    tempNo = tempNo + 1;
                                }

                            }

                            var damageDetailList = shipmentContext.tblDamagedImages.ToList().Where(x => x.ShipmentRouteID == routs).GroupBy(x => x.ImageDescription).Select(x => x.Key).ToList();
                            if (damageDetailList.Count() > 0)
                            {
                                if (customerDetail.ShipmentRoutesStop.Count() == 1)
                                {
                                    damageDetail = damageDetailList.Select(x => x).FirstOrDefault();
                                }
                                else
                                {
                                    foreach (var damage in damageDetailList)
                                    {
                                        damageDetail += " D" + demNo + ": " + damage + " |";
                                    }
                                }
                                demNo = demNo + 1;
                            }
                            
                            GetShipmentFreightDetailDTO getShipmentFreightDetail = new GetShipmentFreightDetailDTO();
                            getShipmentFreightDetail.ShipmentRouteId = routs;
                            getShipmentFreightDetail.PickupLocation = customerDetail.ShipmentFreightDetail.Where(x => x.ShipmentRouteId == routs).Select(x => x.PickupLocation).FirstOrDefault();
                            getShipmentFreightDetail.DeliveryLocation = customerDetail.ShipmentFreightDetail.Where(x => x.ShipmentRouteId == routs).Select(x => x.DeliveryLocation).FirstOrDefault();
                            getShipmentFreightDetail.Commodity = string.Join(",", customerDetail.ShipmentFreightDetail.Where(x => x.ShipmentRouteId == routs).GroupBy(x => x.Commodity).ToList().Select(x => x.Key));
                            getShipmentFreightDetail.FreightType = string.Join(",", customerDetail.ShipmentFreightDetail.Where(x => x.ShipmentRouteId == routs).GroupBy(x => x.FreightType).ToList().Select(x => x.Key));
                            getShipmentFreightDetail.NoOfBox = customerDetail.ShipmentFreightDetail.Where(x => x.ShipmentRouteId == routs).Select(x => x.NoOfBox).Sum();

                            getShipmentFreightDetail.WeightWithUnit = string.Join(", ", ((from wgt in customerDetail.ShipmentFreightDetail
                                                                                          where wgt.ShipmentRouteId == routs
                                                                                          group wgt by wgt.Unit into w

                                                                                          select new GetShipmentFreightDetailDTO
                                                                                          {
                                                                                              Unit = w.Key,
                                                                                              Weight = (from weight in w where weight.Unit == w.Key select weight.Weight).Sum()
                                                                                          }).ToList()).Select(x => (x.Weight + " " + x.Unit)))
                                                                         ;
                            getShipmentFreightDetail.QutWgtVlm = Convert.ToInt32(customerDetail.ShipmentFreightDetail.Where(x => x.ShipmentRouteId == routs).Select(x => x.QutWgtVlm).Sum().ToString().Replace(".00", ""));
                            getShipmentFreightDetail.TrailerCount = customerDetail.ShipmentFreightDetail.Where(x => x.ShipmentRouteId == routs).Select(x => x.TrailerCount).Sum();


                            getShipmentFreightDetailList.Add(getShipmentFreightDetail);


                        }

                        customerDetail.ShipmentFreightDetail = getShipmentFreightDetailList;
                        customerDetail.LoadingTemp = temprature.TrimEnd('|');
                        customerDetail.LoadingDamageDetail = damageDetail.TrimEnd('|');
                    }

                    customerDetail.Drivers = string.Join(" | ", customerDetail.ShipmentEquipmentNdriver.ToList().GroupBy(x => x.DriverName).Select(x => x.Key));
                    customerDetail.Equipments = string.Join(" | ", customerDetail.ShipmentEquipmentNdriver.ToList().GroupBy(x => x.EquipmentName).Select(x => x.Key));

                    if (customerDetail.ShipmentStatusHistory.Count > 0)
                    {

                        foreach (var shphistory in customerDetail.ShipmentStatusHistory)
                        {
                            if (shphistory.SubStatusId == 7 || shphistory.SubStatusId == 11)
                            {
                                shphistory.SubStatus = shphistory.Reason;
                            }
                            shphistory.DateTime = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(shphistory.DateTime));
                            var shipStatusHistory = customerDetail.ShipmentStatusList.Where(x => x.StatusId == shphistory.StatusId).FirstOrDefault();
                            if (shipStatusHistory != null)
                            {
                                customerDetail.ShipmentStatusList.Remove(shipStatusHistory);
                            }

                        }

                    }
                    if (customerDetail.ShipmentRoutesStop.Count > 0)
                    {
                        string shipmentId = ObjEncryptDecrypt.EncryptString(customerShipmentDTO.ShipmentId.ToString());
                        customerDetail.ProofOfDeliveryURL = LarastruckingApp.Entities.Common.Configurations.BaseURL + "ProofOfDelivery/ShipmentProofOfDelivery?shipmentId=" + shipmentId;

                        foreach (var routeStop in customerDetail.ShipmentRoutesStop)
                        {
                            routeStop.SignatureDateTime = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(routeStop.SignatureDateTime));
                        }

                    }

                }


                return customerDetail;
            }
            catch (Exception)
            {

                throw;
            }
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

            List<GetShipmentRouteStopDTO> objGetShipmentRouteStope = new List<GetShipmentRouteStopDTO>();
            try
            {
                if (!string.IsNullOrEmpty(shipmentId))
                {

                    EncryptDecrypt ObjEncryptDecrypt = new EncryptDecrypt();
                    string strShipmentId = ObjEncryptDecrypt.DecryptString(shipmentId);
                    if (!string.IsNullOrEmpty(strShipmentId))
                    {
                        int shpID = Convert.ToInt32(strShipmentId) > 0 ? Convert.ToInt32(strShipmentId) : 0;
                        if (shpID > 0)
                        {
                            objGetShipmentRouteStope = (from shipmentRouteStops in shipmentContext.tblShipmentRoutesStops
                                                        where shipmentRouteStops.ShippingId == shpID && shipmentRouteStops.DigitalSignature != null && shipmentRouteStops.IsDeleted == false
                                                        select new GetShipmentRouteStopDTO
                                                        {
                                                            RouteNo = shipmentRouteStops.RouteNo,
                                                            ShipmentRouteStopId = shipmentRouteStops.ShippingRoutesId,
                                                            ReceiverName = shipmentRouteStops.ReceiverName,
                                                            DigitalSignature = shipmentRouteStops.DigitalSignature ?? string.Empty,
                                                            DeliveryLocation = (from address in shipmentContext.tblAddresses
                                                                                join state in shipmentContext.tblStates on address.State equals state.ID
                                                                                where address.AddressId == shipmentRouteStops.DeliveryLocationId
                                                                                select new { PicUpLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + "," + address.City + "," + state.Name + "," + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault(),
                                                        }
                                                                  ).ToList();
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return objGetShipmentRouteStope;
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
            try
            {


                if (entity.ProofOfTempratureId > 0)
                {
                    var proofOfTemp = shipmentContext.tblProofOfTemperatureImages.Where(x => x.ImageId == entity.ProofOfTempratureId).FirstOrDefault();
                    if (proofOfTemp != null)
                    {
                        proofOfTemp.IsApproved = true;
                        proofOfTemp.ApprovedBy = entity.ApprovedBy;
                        proofOfTemp.ApprovedOn = entity.ApprovedOn;
                    }
                    shipmentContext.Entry(proofOfTemp).State = EntityState.Modified;
                }
                return shipmentContext.SaveChanges() > 0 ? true : false;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {


                if (entity.DamageId > 0)
                {
                    var damageDocument = shipmentContext.tblDamagedImages.Where(x => x.DamagedID == entity.DamageId).FirstOrDefault();
                    if (damageDocument != null)
                    {
                        damageDocument.IsApproved = true;
                        damageDocument.ApprovedBy = entity.ApprovedBy;
                        damageDocument.ApprovedOn = entity.ApprovedOn;
                    }
                    shipmentContext.Entry(damageDocument).State = EntityState.Modified;
                }
                return shipmentContext.SaveChanges() > 0 ? true : false;
            }
            catch (Exception)
            {

                throw;
            }

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
            try
            {


                List<MatchEquipmentNDriverDTO> objEquipmentNDriver = new List<MatchEquipmentNDriverDTO>();
                if (model != null)
                {
                    if (model.ShipmentEquipmentNdriver.Count > 0)
                    {
                        foreach (var shipNdrv in model.ShipmentEquipmentNdriver)
                        {
                            var shipmentEquipmentList = (from shipment in shipmentContext.tblShipments
                                                         join routes in shipmentContext.tblShipmentRoutesStops on shipment.ShipmentId equals routes.ShippingId
                                                         join tblEquipment in shipmentContext.tblShipmentEquipmentNdrivers on routes.ShippingId equals tblEquipment.ShipmentId
                                                         where tblEquipment.EquipmentId == shipNdrv.EquipmentId
                                                         && (model.ShipmentId > 0 ? model.ShipmentId != shipment.ShipmentId : 1 == 1) && (shipment.StatusId != 7 && shipment.StatusId != 8 && shipment.StatusId != 11)
                                                         // && shipment.ShipmentId != model.ShipmentId
                                                         && shipment.IsDeleted == false
                                                         && routes.IsDeleted == false
                                                         && (model.FirstPickupArrivalDate <= routes.PickDateTime || model.FirstPickupArrivalDate <= routes.DeliveryDateTime || model.LastPickupArrivalDate <= routes.DeliveryDateTime)
                                                         select new MatchEquipmentNDriverDTO
                                                         {
                                                             ShipmentId = shipment.ShipmentId,
                                                             IsShipment = true,
                                                             AWB = shipment.AirWayBill,
                                                             CustomerPO = shipment.CustomerPO,
                                                             EquipmentId = tblEquipment.EquipmentId
                                                         }

                                           ).ToList();
                            objEquipmentNDriver.AddRange(shipmentEquipmentList);
                            var shipmentDriverList = (from shipment in shipmentContext.tblShipments
                                                      join routes in shipmentContext.tblShipmentRoutesStops on shipment.ShipmentId equals routes.ShippingId
                                                      join tblEquipment in shipmentContext.tblShipmentEquipmentNdrivers on routes.ShippingId equals tblEquipment.ShipmentId
                                                      where tblEquipment.DriverId == shipNdrv.DriverId
                                                      // && shipment.ShipmentId != model.ShipmentId
                                                      && (model.ShipmentId > 0 ? model.ShipmentId != shipment.ShipmentId : 1 == 1) && (shipment.StatusId != 7 && shipment.StatusId != 8 && shipment.StatusId != 11)
                                                      && shipment.IsDeleted == false
                                                      && routes.IsDeleted == false
                                                      && (model.FirstPickupArrivalDate <= routes.PickDateTime || model.FirstPickupArrivalDate <= routes.DeliveryDateTime || model.LastPickupArrivalDate <= routes.DeliveryDateTime)
                                                      select new MatchEquipmentNDriverDTO
                                                      {
                                                          ShipmentId = shipment.ShipmentId,
                                                          IsShipment = true,
                                                          AWB = shipment.AirWayBill,
                                                          CustomerPO = shipment.CustomerPO,
                                                          DriverId = tblEquipment.DriverId
                                                      }

                                        ).ToList();
                            objEquipmentNDriver.AddRange(shipmentDriverList);
                            var fumigationEquipment = (from fumigation in shipmentContext.tblFumigations
                                                       join route in shipmentContext.tblFumigationRouts on fumigation.FumigationId equals route.FumigationId
                                                       join equipment in shipmentContext.tblFumigationEquipmentNDrivers on route.FumigationId equals equipment.FumigationId
                                                       where equipment.FumigationRoutsId == route.FumigationRoutsId && equipment.EquipmentId == shipNdrv.EquipmentId && (fumigation.StatusId != 7 && fumigation.StatusId != 8 && fumigation.StatusId != 11)
                                                       && fumigation.IsDeleted == false
                                                       && route.IsDeleted == false
                                                       && (model.FirstPickupArrivalDate <= route.PickUpArrival || model.FirstPickupArrivalDate <= route.DeliveryArrival || model.LastPickupArrivalDate <= route.DeliveryArrival)
                                                       select new MatchEquipmentNDriverDTO
                                                       {
                                                           FumigationId = fumigation.FumigationId,
                                                           IsShipment = false,
                                                           AWB = route.AirWayBill,
                                                           CustomerPO = route.CustomerPO,
                                                           EquipmentId = equipment.EquipmentId
                                                       }
                                                       ).ToList();
                            objEquipmentNDriver.AddRange(fumigationEquipment);
                            var fumigationDriver = (from fumigation in shipmentContext.tblFumigations
                                                    join route in shipmentContext.tblFumigationRouts on fumigation.FumigationId equals route.FumigationId
                                                    join equipment in shipmentContext.tblFumigationEquipmentNDrivers on route.FumigationId equals equipment.FumigationId
                                                    where equipment.FumigationRoutsId == route.FumigationRoutsId && equipment.DriverId == shipNdrv.DriverId
                                                    && fumigation.IsDeleted == false
                                                    && route.IsDeleted == false
                                                    && (model.FirstPickupArrivalDate <= route.PickUpArrival || model.FirstPickupArrivalDate <= route.DeliveryArrival || model.LastPickupArrivalDate <= route.DeliveryArrival)
                                                    && (fumigation.StatusId != 7 && fumigation.StatusId != 8 && fumigation.StatusId != 11)
                                                    select new MatchEquipmentNDriverDTO
                                                    {
                                                        FumigationId = fumigation.FumigationId,
                                                        IsShipment = false,
                                                        AWB = route.AirWayBill,
                                                        CustomerPO = route.CustomerPO,
                                                        DriverId = equipment.DriverId
                                                    }
                                                      ).ToList();
                            objEquipmentNDriver.AddRange(fumigationDriver);

                        }
                    }
                }

                return objEquipmentNDriver;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {


                List<MatchEquipmentNDriverDTO> objEquipment = new List<MatchEquipmentNDriverDTO>();
                var shipmentEquipmentList = (from shipment in shipmentContext.tblShipments
                                             join routes in shipmentContext.tblShipmentRoutesStops on shipment.ShipmentId equals routes.ShippingId
                                             join tblEquipment in shipmentContext.tblShipmentEquipmentNdrivers on routes.ShippingId equals tblEquipment.ShipmentId
                                             where tblEquipment.EquipmentId == model.EquipmentId
                                             && (model.ShipmentId > 0 ? model.ShipmentId != shipment.ShipmentId : 1 == 1) && (shipment.StatusId != 7 && shipment.StatusId != 8 && shipment.StatusId != 11)
                                             && shipment.IsDeleted == false
                                             && routes.IsDeleted == false
                                             && (model.FirstPickupArrivalDate <= routes.PickDateTime || model.FirstPickupArrivalDate <= routes.DeliveryDateTime || model.LastPickupArrivalDate <= routes.DeliveryDateTime)
                                             select new MatchEquipmentNDriverDTO
                                             {
                                                 ShipmentId = shipment.ShipmentId,
                                                 IsShipment = true,
                                                 AWB = shipment.AirWayBill ?? string.Empty,
                                                 CustomerPO = shipment.CustomerPO ?? string.Empty,
                                                 EquipmentId = tblEquipment.EquipmentId
                                             }

                                          ).ToList();

                shipmentEquipmentList = (from SEL in shipmentEquipmentList
                                         group SEL by SEL.AWB into g
                                         select new MatchEquipmentNDriverDTO
                                         {
                                             ShipmentId = (from shp in g select shp.ShipmentId).FirstOrDefault(),
                                             IsShipment = (from shp in g select shp.IsShipment).FirstOrDefault(),
                                             AWB = g.Key ?? string.Empty,
                                             CustomerPO = (from shp in g select shp.CustomerPO).FirstOrDefault() ?? string.Empty,
                                             EquipmentId = (from shp in g select shp.EquipmentId).FirstOrDefault()
                                         }).ToList();



                objEquipment.AddRange(shipmentEquipmentList);

                var fumigationEquipment = (from fumigation in shipmentContext.tblFumigations
                                           join route in shipmentContext.tblFumigationRouts on fumigation.FumigationId equals route.FumigationId
                                           join equipment in shipmentContext.tblFumigationEquipmentNDrivers on route.FumigationId equals equipment.FumigationId
                                           where equipment.FumigationRoutsId == route.FumigationRoutsId && equipment.EquipmentId == model.EquipmentId
                                           && fumigation.IsDeleted == false
                                           && (model.FumigationId > 0 ? model.FumigationId != fumigation.FumigationId : 1 == 1) && (fumigation.StatusId != 7 && fumigation.StatusId != 8 && fumigation.StatusId != 11)
                                           && route.IsDeleted == false
                                           // && (model.FirstPickupArrivalDate <= route.PickUpArrival || model.FirstPickupArrivalDate <= route.DeliveryArrival || model.LastPickupArrivalDate <= route.DeliveryArrival)
                                           && (model.FirstPickupArrivalDate <= route.PickUpArrival || model.FirstPickupArrivalDate <= route.FumigationArrival || model.LastPickupArrivalDate <= route.FumigationArrival)
                                           && (model.FirstPickupArrivalDate <= route.ReleaseDate || model.FirstPickupArrivalDate <= route.DeliveryArrival || model.LastPickupArrivalDate <= route.DeliveryArrival)
                                           select new MatchEquipmentNDriverDTO
                                           {
                                               FumigationId = fumigation.FumigationId,
                                               IsShipment = false,
                                               AWB = route.AirWayBill ?? string.Empty,
                                               ContainerNo = route.ContainerNo ?? string.Empty,
                                               EquipmentId = equipment.EquipmentId
                                           }
                                           ).ToList();
                fumigationEquipment = (from SEL in fumigationEquipment
                                       group SEL by SEL.AWB into g
                                       select new MatchEquipmentNDriverDTO
                                       {
                                           ShipmentId = (from shp in g select shp.ShipmentId).FirstOrDefault(),
                                           IsShipment = (from shp in g select shp.IsShipment).FirstOrDefault(),
                                           AWB = g.Key ?? string.Empty,
                                           ContainerNo = (from shp in g select shp.ContainerNo).FirstOrDefault() ?? string.Empty,
                                           EquipmentId = (from shp in g select shp.EquipmentId).FirstOrDefault()
                                       }).ToList();
                objEquipment.AddRange(fumigationEquipment);


                return objEquipment;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {


                List<MatchEquipmentNDriverDTO> objDriver = new List<MatchEquipmentNDriverDTO>();
                var shipmentDriverList = (from shipment in shipmentContext.tblShipments
                                          join routes in shipmentContext.tblShipmentRoutesStops on shipment.ShipmentId equals routes.ShippingId
                                          join tblEquipment in shipmentContext.tblShipmentEquipmentNdrivers on routes.ShippingId equals tblEquipment.ShipmentId
                                          where tblEquipment.DriverId == model.DriverId
                                          // && shipment.ShipmentId != model.ShipmentId
                                          && (model.ShipmentId > 0 ? model.ShipmentId != shipment.ShipmentId : 1 == 1) && (shipment.StatusId != 7 && shipment.StatusId != 8 && shipment.StatusId != 11)
                                          && shipment.IsDeleted == false
                                          && routes.IsDeleted == false
                                          && (model.FirstPickupArrivalDate <= routes.PickDateTime || model.FirstPickupArrivalDate <= routes.DeliveryDateTime || model.LastPickupArrivalDate <= routes.DeliveryDateTime)
                                          select new MatchEquipmentNDriverDTO
                                          {
                                              ShipmentId = shipment.ShipmentId,
                                              IsShipment = true,
                                              AWB = shipment.AirWayBill ?? string.Empty,
                                              CustomerPO = shipment.CustomerPO ?? string.Empty,
                                              DriverId = tblEquipment.DriverId
                                          }

                                ).ToList();
                shipmentDriverList = (from SEL in shipmentDriverList
                                      group SEL by SEL.AWB into g
                                      select new MatchEquipmentNDriverDTO
                                      {
                                          ShipmentId = (from shp in g select shp.ShipmentId).FirstOrDefault(),
                                          IsShipment = (from shp in g select shp.IsShipment).FirstOrDefault(),
                                          AWB = g.Key ?? string.Empty,
                                          CustomerPO = (from shp in g select shp.CustomerPO).FirstOrDefault() ?? string.Empty,
                                          EquipmentId = (from shp in g select shp.EquipmentId).FirstOrDefault()
                                      }).ToList();
                objDriver.AddRange(shipmentDriverList);

                var fumigationDriver = (from fumigation in shipmentContext.tblFumigations
                                        join route in shipmentContext.tblFumigationRouts on fumigation.FumigationId equals route.FumigationId
                                        join equipment in shipmentContext.tblFumigationEquipmentNDrivers on route.FumigationId equals equipment.FumigationId
                                        where equipment.FumigationRoutsId == route.FumigationRoutsId && equipment.DriverId == model.DriverId
                                        && fumigation.IsDeleted == false
                                         && (model.FumigationId > 0 ? model.FumigationId != fumigation.FumigationId : 1 == 1) && (fumigation.StatusId != 7 && fumigation.StatusId != 8 && fumigation.StatusId != 11)
                                        && route.IsDeleted == false
                                        //&& (model.FirstPickupArrivalDate <= route.PickUpArrival || model.FirstPickupArrivalDate <= route.DeliveryArrival || model.LastPickupArrivalDate <= route.DeliveryArrival)
                                        && (model.FirstPickupArrivalDate <= route.PickUpArrival || model.FirstPickupArrivalDate <= route.FumigationArrival || model.LastPickupArrivalDate <= route.FumigationArrival)
                                        && (model.FirstPickupArrivalDate <= route.ReleaseDate || model.FirstPickupArrivalDate <= route.DeliveryArrival || model.LastPickupArrivalDate <= route.DeliveryArrival)
                                        select new MatchEquipmentNDriverDTO
                                        {
                                            FumigationId = fumigation.FumigationId,
                                            IsShipment = false,
                                            AWB = route.AirWayBill ?? string.Empty,
                                            ContainerNo = route.ContainerNo ?? string.Empty,
                                            DriverId = equipment.DriverId
                                        }
                                          ).ToList();
                fumigationDriver = (from SEL in fumigationDriver
                                    group SEL by SEL.AWB into g
                                    select new MatchEquipmentNDriverDTO
                                    {
                                        ShipmentId = (from shp in g select shp.ShipmentId).FirstOrDefault(),
                                        IsShipment = (from shp in g select shp.IsShipment).FirstOrDefault(),
                                        AWB = g.Key ?? string.Empty,
                                        ContainerNo = (from shp in g select shp.ContainerNo).FirstOrDefault() ?? string.Empty,
                                        EquipmentId = (from shp in g select shp.EquipmentId).FirstOrDefault()
                                    }).ToList();
                objDriver.AddRange(fumigationDriver);
                return objDriver;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {


                var shipmentDetail = new CopyShipmentDTO();
                if (shipmentId > 0)
                {
                    shipmentDetail = (from shipment in shipmentContext.tblShipments
                                      join customer in shipmentContext.tblCustomerRegistrations on shipment.CustomerId equals customer.CustomerID
                                      join routes in shipmentContext.tblShipmentRoutesStops on shipment.ShipmentId equals routes.ShippingId
                                      where shipment.ShipmentId == shipmentId && routes.IsDeleted == false
                                      select new CopyShipmentDTO
                                      {
                                          ShipmentId = shipment.ShipmentId,
                                          EstPickupArriaval = routes.PickDateTime,
                                          EstDeliveryArrival = routes.DeliveryDateTime,
                                          CustomerId = shipment.CustomerId,
                                          CustomerName = customer.CustomerName,
                                          AWB = shipment.AirWayBill,
                                      }
                                    ).FirstOrDefault();
                    if (shipmentDetail != null)
                    {
                        shipmentDetail.EstPickupArriaval = Configurations.ConvertDateTime(Convert.ToDateTime(shipmentDetail.EstPickupArriaval));
                        shipmentDetail.EstDeliveryArrival = Configurations.ConvertDateTime(Convert.ToDateTime(shipmentDetail.EstDeliveryArrival));
                    }

                }
                return shipmentDetail;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {

                bool isSuccess = false;
                if (entity != null)
                {
                    if (entity != null)
                    {

                        var shipmentdata = shipmentContext.tblShipments.Where(x => x.ShipmentId == entity.ShipmentId).FirstOrDefault();
                        if (shipmentdata != null)
                        {
                            tblShipment objShipment = new tblShipment();
                            objShipment.CustomerId = entity.CustomerId;
                            objShipment.StatusId = 1;
                            objShipment.SubStatusId = null;
                            objShipment.RequestedBy = shipmentdata.RequestedBy;
                            objShipment.Reason = shipmentdata.Reason;
                            objShipment.ShipmentRefNo = shipmentdata.ShipmentRefNo;
                            objShipment.AirWayBill = entity.AWB;

                            objShipment.FinalTotalAmount = shipmentdata.FinalTotalAmount;
                            objShipment.DriverInstruction = shipmentdata.DriverInstruction;
                            objShipment.VendorNconsignee = shipmentdata.VendorNconsignee;
                            objShipment.CreatedBy = entity.CreatedBy;
                            objShipment.CreatedDate = entity.CreatedDate;
                            shipmentContext.tblShipments.Add(objShipment);

                            tblShipmentStatusHistory shipmentStatusHistory = new tblShipmentStatusHistory();
                            shipmentStatusHistory.ShipmentId = objShipment.ShipmentId;
                            shipmentStatusHistory.StatusId = 1;
                            shipmentStatusHistory.SubStatusId = null;
                            shipmentStatusHistory.Reason = null;
                            shipmentStatusHistory.CreatedBy = entity.CreatedBy;
                            shipmentStatusHistory.CreatedOn = entity.CreatedDate;
                            shipmentContext.tblShipmentStatusHistories.Add(shipmentStatusHistory);
                            isSuccess = shipmentContext.SaveChanges() > 0;

                            tblShipmentEventHistory shipmentEventHistory = new tblShipmentEventHistory();
                            shipmentEventHistory.ShipmentId = objShipment.ShipmentId;
                            shipmentEventHistory.StatusId = 1;
                            shipmentEventHistory.UserId = entity.CreatedBy;
                            shipmentEventHistory.Event = "STATUS";
                            shipmentEventHistory.EventDateTime = entity.CreatedDate;
                            shipmentContext.tblShipmentEventHistories.Add(shipmentEventHistory);
                            shipmentContext.SaveChanges();

                            bool IsWaitingTime = shipmentContext.tblCustomerRegistrations.Where(x => x.CustomerID == entity.CustomerId).Select(x => x.IsWaitingTimeRequired).FirstOrDefault();
                            var routeDetail = shipmentContext.tblShipmentRoutesStops.Where(x => x.ShippingId == shipmentdata.ShipmentId && x.IsDeleted == false).ToList();
                            if (routeDetail.Count > 0)
                            {
                                foreach (var route in routeDetail)
                                {


                                    tblShipmentRoutesStop objShipmentRouteStops = new tblShipmentRoutesStop();
                                    objShipmentRouteStops.ShippingId = objShipment.ShipmentId;
                                    objShipmentRouteStops.RouteNo = route.RouteNo;
                                    objShipmentRouteStops.PickupLocationId = route.PickupLocationId;
                                    objShipmentRouteStops.PickDateTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.EstPickupArriaval));
                                    objShipmentRouteStops.PickUpDateTimeTo = Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.EstDeliveryArrival));
                                    objShipmentRouteStops.DeliveryLocationId = route.DeliveryLocationId;
                                    objShipmentRouteStops.DeliveryDateTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.EstPickupArriaval));
                                    objShipmentRouteStops.DeliveryDateTimeTo = Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.EstDeliveryArrival));
                                    objShipmentRouteStops.Comment = route.Comment;
                                    objShipmentRouteStops.IsAppointmentRequired = route.IsAppointmentRequired;
                                    objShipmentRouteStops.IsPickUpWaitingTimeRequired = IsWaitingTime;
                                    objShipmentRouteStops.IsDeliveryWaitingTimeRequired = IsWaitingTime;
                                    shipmentContext.tblShipmentRoutesStops.Add(objShipmentRouteStops);


                                    var freightDetail = shipmentContext.tblShipmentFreightDetails.Where(x => x.ShipmentId == shipmentdata.ShipmentId && x.ShipmentRouteStopeId == route.ShippingRoutesId && x.IsDeleted == false).ToList();
                                    if (freightDetail.Count > 0)
                                    {
                                        foreach (var freight in freightDetail)
                                        {
                                            tblShipmentFreightDetail objShipmentFreight = new tblShipmentFreightDetail();
                                            objShipmentFreight.ShipmentId = objShipment.ShipmentId;
                                            objShipmentFreight.ShipmentRouteStopeId = objShipmentRouteStops.ShippingRoutesId;
                                            objShipmentFreight.Commodity = freight.Commodity;
                                            objShipmentFreight.FreightTypeId = freight.FreightTypeId;
                                            objShipmentFreight.PricingMethodId = freight.PricingMethodId;
                                            objShipmentFreight.MinFee = freight.MinFee;
                                            objShipmentFreight.UpTo = freight.UpTo;
                                            objShipmentFreight.UnitPrice = freight.UnitPrice;
                                            objShipmentFreight.Hazardous = freight.Hazardous;
                                            objShipmentFreight.Temperature = freight.Temperature;
                                            objShipmentFreight.TemperatureType = freight.TemperatureType;
                                            objShipmentFreight.Weight = freight.Weight;
                                            objShipmentFreight.Unit = freight.Unit;
                                            objShipmentFreight.NoOfBox = freight.NoOfBox;
                                            objShipmentFreight.TrailerCount = freight.TrailerCount;
                                            objShipmentFreight.Comments = freight.Comments;
                                            objShipmentFreight.QuantityNweight = freight.QuantityNweight;
                                            objShipmentFreight.TotalPrice = freight.TotalPrice;
                                            objShipmentFreight.IsPartialShipment = freight.IsPartialShipment;
                                            objShipmentFreight.PartialBox = freight.PartialBox;
                                            objShipmentFreight.PartialPallete = freight.PartialPallete;
                                            shipmentContext.tblShipmentFreightDetails.Add(objShipmentFreight);

                                        }
                                    }

                                    isSuccess = shipmentContext.SaveChanges() > 0;
                                }
                            }



                            return isSuccess;
                        }



                    }



                }
                return false;

            }
            catch (Exception)
            {

                throw;
            }
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
            int? MaxRouteNo = 0;
            if (shipmentId > 0)
            {
                MaxRouteNo = shipmentContext.tblShipmentRoutesStops.Where(x => x.ShippingId == shipmentId).Max(x => x.RouteNo);
            }
            return MaxRouteNo;

        }


        #endregion

        #region get pricing method
        /// <summary>
        /// Get pricing method
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ShipmentIsReady(int shipmentId, bool ready)
        {

            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {

                        new SqlParameter("@ShipmentId",shipmentId),
                        new SqlParameter("@IsReady",ready),
                     };
                var result = sp_dbContext.ExecuteStoredProcedure<int>("usp_UpdateShipmenetIsReady", sqlParameters);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
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
            var timeCard = (from TC in shipmentContext.tblTimeCards
                            join DRV in shipmentContext.tblDrivers on TC.UserId equals DRV.UserId
                            where DRV.DriverID == driverId && DbFunctions.TruncateTime(Configurations.TodayDateTime) == DbFunctions.TruncateTime(TC.InDateTime)
                            select new
                            {
                                InDateTime = TC.InDateTime
                            }
                          ).Select(x=>x.InDateTime).FirstOrDefault();
            if(timeCard!=null)
            {
                timeCard = Configurations.ConvertDateTime(Convert.ToDateTime(timeCard));
            }

            return timeCard;


        }
        #endregion

        public string DriverPhone(int driverid)
        {
            try
            {

                //Check driver on leave or not
                var driverList = (from driver in shipmentContext.tblDrivers
                                  where driver.DriverID  == driverid
                                  select new
                                  {
                                      Phone = driver.Phone
                                  }
                                  ).Select(x => x.Phone).FirstOrDefault();

                if (driverList != null)
                {
                    driverList = driverList.ToString();
                }

                return driverList;
            }
            catch (Exception)
            {

                throw;
            }

        }

     // Get Customer Name by Dart
        public string CustomerName(int CustomerID)
        {
            try
            {

                //Check driver on leave or not
                var customerList = (from customer in shipmentContext.tblCustomerRegistrations
                                  where customer.CustomerID == CustomerID
                                  select new
                                  {
                                      CustomerName = customer.CustomerName
                                  }
                                  ).Select(x => x.CustomerName).FirstOrDefault();

                if (customerList != null)
                {
                    customerList = customerList.ToString();
                }

                return customerList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region GetTemperatureEmailDetail
        public TemperatureEmailSipmentDTO GetTemperatureEmailDetail(int shipmentId)
        {
            try
            {
                var temperatureDetail = new TemperatureEmailSipmentDTO();

                temperatureDetail = (from shipments in shipmentContext.tblShipments
                                     join shipmentRouteStop in shipmentContext.tblShipmentRoutesStops on shipments.ShipmentId equals shipmentRouteStop.ShippingId
                                     where shipments.ShipmentId == shipmentId
                                     select new TemperatureEmailSipmentDTO 
                                     {
                                         ShipmentRoutsId = shipmentRouteStop.ShippingRoutesId,
                                         ShipmentId = shipmentRouteStop.ShippingId,
                                         PickUpArrival = shipmentRouteStop.PickDateTime,
                                         PickUpDeparture = shipmentRouteStop.PickUpDateTimeTo,
                                         DeliveryArrival = shipmentRouteStop.DeliveryDateTime,
                                         DeliveryDeparture = shipmentRouteStop.DeliveryDateTimeTo,
                                         AirWayBill = shipments.AirWayBill,
                                         OrderNo = shipments.ContainerNo,
                                         CustomerPO = shipments.CustomerPO,
                                         LogoURL = (LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingApp.Entities.Common.Configurations.LarasLogo),
                                         ActTemp = "",
                                         ActTemperature = 0
                                     }

                                  ).FirstOrDefault();

                //var temperatureDetails = new TemperatureEmailSipmentDTO();

                //temperatureDetail = (from shipments in shipmentContext.tblShipments
                //                     where shipments.ShipmentId == shipmentId && shipments.IsDeleted == false
                //                     select new TemperatureEmailSipmentDTO
                //                     {


                //                         AirWayBill = shipments.AirWayBill,
                //                         OrderNo = shipments.ContainerNo,
                //                         CustomerPO = shipments.CustomerPO,
                //                         LogoURL = (LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingApp.Entities.Common.Configurations.LarasLogo),
                //                         ActTemp = "",
                //                         ActTemperature = 0
                //                     }

                //                  ).FirstOrDefault();

                ErrorLog("ShipmentEmail: "+ temperatureDetail);


                return temperatureDetail;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        public static void ErrorLog(string sErrMsg)
        {
            string sLogFormat;
            string sErrorTime;

            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear + sMonth + sDay;
            string path = System.Web.HttpContext.Current.Server.MapPath("../../Assets/ErrorLog");
            StreamWriter sw = new StreamWriter(path + sErrorTime, true);
            sw.WriteLine(sLogFormat + sErrMsg);
            sw.Flush();
            sw.Close();
        }
    }
}
