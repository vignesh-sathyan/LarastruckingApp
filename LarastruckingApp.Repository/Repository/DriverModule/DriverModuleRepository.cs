using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.Driver_Fumigation;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;

namespace LarastruckingApp.Repository.Repository.DriverModule
{
    public class DriverModuleRepository : IDriverModuleRepository
    {

        #region Private Members
        /// <summary>
        /// defining Private members
        /// </summary>
        private ExecuteSQLStoredProceduce _dbContext = null;
        private readonly LarastruckingDBEntities driverContext;
        #endregion

        #region Contructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="IDriverModuleDAL"></param>

        public DriverModuleRepository(ExecuteSQLStoredProceduce dbContext)
        {

            _dbContext = dbContext;
            driverContext = new LarastruckingDBEntities();
        }
        #endregion

        #region  Section 1-> Driver Shipment
        #region Get All Pre-Trip Shipment Details
        /// <summary>
        /// Get All Pre-Trip Shipment Details
        /// </summary>
        /// <returns></returns>

        public IList<PreTripShipmentDto> GetPreTripShipmentDetails(DataTableFilterDto dto, int userId)
        {
            try
            {
                //var totalCount = new SqlParameter
                //{
                //    ParameterName = "@TotalCount",
                //    Value = 0,
                //    Direction = ParameterDirection.Output
                //};

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@SearchTerm", dto.SearchTerm),
                        new SqlParameter("@SortColumn", dto.SortColumn),
                        new SqlParameter("@SortOrder", dto.SortOrder),
                        new SqlParameter("@PageNumber", dto.PageNumber),
                        new SqlParameter("@PageSize", dto.PageSize),
                       // totalCount
                    };


                var result = _dbContext.ExecuteStoredProcedure<PreTripShipmentDto>("usp_DriverDashboard", sqlParameters);

                dto.TotalCount = result != null && result.Count > 0 ? Convert.ToInt32(result.Select(x => x.TotalCount).FirstOrDefault()) : 0;
                result = result != null && result.Count > 0 ? result : new List<PreTripShipmentDto>();
               
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Pre-Trip Check List
        /// <summary>
        /// Get Pre-Trip Check List
        /// </summary>
        /// <returns></returns>

        public DriverPreTripDto GetPreTripCheckList(int shipmentId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 2),
                        new SqlParameter("@ShipmentId", shipmentId)
                    };

            var result = _dbContext.ExecuteStoredProcedure<DriverPreTripDto>("usp_DriverModulePreTrip", sqlParameters);
            return result != null && result.Count > 0 ? result[0] : null;
        }
        #endregion

        #region Save Pre-Trip Check List
        /// <summary>
        /// Save Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        public bool SavePreTripCheckList(DriverPreTripDto dto)
        {

            if (dto.PreTripCheckupId == 0)
            {
                tblPreTripCheckUp preTrip = AutoMapperServices<DriverPreTripDto, tblPreTripCheckUp>.ReturnObject(dto);
                preTrip.CreatedOn = Configurations.TodayDateTime;
                preTrip.ModifiedOn = Configurations.TodayDateTime;

                _dbContext.tblPreTripCheckUps.Add(preTrip);

            }
            else
            {
                var dbExists = _dbContext.tblPreTripCheckUps.Find(dto.PreTripCheckupId);
                if (dbExists != null)
                {
                    dbExists.IsTiresGood = dto.IsTiresGood;
                    dbExists.IsBreaksGood = dto.IsBreaksGood;
                    dbExists.Fuel = dto.Fuel;
                    dbExists.LoadStraps = dto.LoadStraps;
                    dbExists.OverAllCondition = dto.OverAllCondition;
                    dbExists.ModifiedOn = Configurations.TodayDateTime;
                }
            }

            return _dbContext.SaveChanges() > 0;
        }
        #endregion

        #region Get Routes of Pre-Trip Shipment
        /// <summary>
        /// Get all the routes of Pre-Trip Shipment by shipment id
        /// </summary>
        /// <returns></returns>

        public List<ShipmentRoutesDto> GetShipmentRoutes(int shipmentId, long userID)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 3),
                        new SqlParameter("@ShipmentId", shipmentId),
                         new SqlParameter("@UserId", userID)
                    };

            var result = _dbContext.ExecuteStoredProcedure<ShipmentRoutesDto>("usp_DriverModulePreTrip", sqlParameters);
            result = result != null && result.Count > 0 ? result : new List<ShipmentRoutesDto>();
            if (result != null && result.Count > 0)
            {
                foreach (var data in result)
                {
                    data.PickupDateTime = Configurations.ConvertUTCtoLocalTime(data.PickupDateTime);
                    data.DeliveryDateTime = Configurations.ConvertUTCtoLocalTime(data.DeliveryDateTime);
                }
            }
            return result;
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
            List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 4),
                        new SqlParameter("@ShippingRoutesId", ShippingRoutesId)
                    };

            var result = _dbContext.ExecuteStoredProcedure<ShipmentLocationDetailsDto>("usp_DriverModulePreTrip", sqlParameters);
            if (result != null && result.Count > 0)
            {
                result[0].PickUpArrivalDate = Configurations.ConvertDateTime(result[0].PickUpArrivalDate);
                result[0].DeliveryArrive = Configurations.ConvertDateTime(result[0].DeliveryArrive);
            }
            var shipments = result.Count > 0 ? result[0] : null;
            return shipments;
        }
        #endregion

        #region Save Pre-Trip Shipment Detail
        /// <summary>
        /// Save Pre-Trip Shipment Detail
        /// </summary>
        /// <returns></returns>

        public bool SavePreTripShipmentDetail(PreTripAddShipmentDetailDto dto, out bool isEmailNeedToSend)
        {
            bool isSucccess = false;

            bool isEmailNeedSend = false;
            bool isTempratureRequired = (from cust in driverContext.tblCustomerRegistrations
                                         join shp in driverContext.tblShipments on cust.CustomerID equals shp.CustomerId
                                         where shp.ShipmentId==dto.ShipmentId
                                         select cust.IsTemperatureRequired
                                         ).FirstOrDefault();
            var objShipmentDetail = _dbContext.tblShipmentRoutesStops.Where(x => x.ShippingRoutesId == dto.ShipmentRouteId).FirstOrDefault();
            if (objShipmentDetail != null)
            {
                objShipmentDetail.DriverPickupArrival = dto.DriverPickupArrival == null ? dto.DriverPickupArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverPickupArrival));
                objShipmentDetail.DriverPickupDeparture = dto.DriverPickupDeparture == null ? dto.DriverPickupDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverPickupDeparture));
                objShipmentDetail.DriverDeliveryArrival = dto.DriverDeliveryArrival == null ? dto.DriverDeliveryArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverDeliveryArrival));
                objShipmentDetail.DriverDeliveryDeparture = dto.DriverDeliveryDeparture == null ? dto.DriverDeliveryDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverDeliveryDeparture));
                objShipmentDetail.ReceiverName = dto.ReceiverName;
                _dbContext.Entry(objShipmentDetail).State = EntityState.Modified;
                isSucccess = _dbContext.SaveChanges() > 0;
            }


            if (dto.StatusId == 5 || dto.StatusId == 6)
            {
                var proofOfTemp = _dbContext.tblProofOfTemperatureImages.Where(X => X.ShipmentRouteId == dto.ShipmentRouteId && X.IsDeleted == false).OrderByDescending(x => x.ImageId).FirstOrDefault();
                if (proofOfTemp != null)
                {
                    if (isTempratureRequired)
                    {
                        float number;

                        if (float.TryParse(proofOfTemp.ActualTemperature, out number))
                        {
                            var objShipmentFreightDetail = _dbContext.tblShipmentFreightDetails.Where(x => x.ShipmentRouteStopeId == dto.ShipmentRouteId && x.ShipmentBaseFreightDetailId == proofOfTemp.ShipmentFreightDetailId && x.Temperature != null && x.IsDeleted == false).Max(x => x.Temperature);

                            if (objShipmentFreightDetail != null && (float)objShipmentFreightDetail != 0)
                            {
                                float diff = ((float)(objShipmentFreightDetail) - number);
                                if (diff > 10 || diff < -10)
                                {
                                    dto.StatusId = 3;
                                    dto.SubStatusId = 9;
                                }
                            }
                        }
                    }
                    if (dto.StatusId == 5 || dto.StatusId == 6)
                    {
                        var exist = _dbContext.tblShipmentEventHistories.Any(x => x.ShipmentId == dto.ShipmentId && x.ShipmentRouteStopId == dto.ShipmentRouteId && x.ShipmentFreightDetailId == proofOfTemp.ShipmentFreightDetailId);
                        if (!exist)
                        {
                            tblShipmentEventHistory shipmentEventHistory = new tblShipmentEventHistory();
                            shipmentEventHistory.ShipmentId = dto.ShipmentId;
                            shipmentEventHistory.ShipmentRouteStopId = dto.ShipmentRouteId;
                            shipmentEventHistory.ShipmentFreightDetailId = proofOfTemp.ShipmentFreightDetailId;
                            shipmentEventHistory.StatusId = 5;
                            shipmentEventHistory.UserId = dto.CreatedBy;
                            shipmentEventHistory.Event = "STATUS";
                            shipmentEventHistory.EventDateTime = dto.CreatedOn;
                            _dbContext.tblShipmentEventHistories.Add(shipmentEventHistory);
                            _dbContext.SaveChanges();
                        }
                    }

                }
            }


            var isValidStatus = ValidateStatus(dto.ShipmentId, dto.ShipmentRouteId, dto.StatusId);
            var objshipmentStatusHistoy = _dbContext.tblShipmentStatusHistories.Where(x => x.ShipmentId == dto.ShipmentId).OrderByDescending(x => x.ShipmentStatusHistoryId).FirstOrDefault();
            if (!isValidStatus && objshipmentStatusHistoy != null && objshipmentStatusHistoy.StatusId != dto.StatusId && dto.StatusId != 4 && dto.StatusId != 3)
            {
                tblShipmentStatusHistory shipmentStatusHistory = new tblShipmentStatusHistory();
                shipmentStatusHistory.ShipmentId = dto.ShipmentId;
                shipmentStatusHistory.StatusId = dto.StatusId;
                shipmentStatusHistory.SubStatusId = dto.SubStatusId;
                shipmentStatusHistory.Reason = dto.Reason;
                shipmentStatusHistory.CreatedBy = dto.CreatedBy;
                shipmentStatusHistory.CreatedOn = dto.CreatedOn;
                _dbContext.tblShipmentStatusHistories.Add(shipmentStatusHistory);
                _dbContext.SaveChanges();

                if (dto.StatusId != 5 && dto.StatusId != 7)
                {
                    tblShipmentEventHistory shipmentEventHistory = new tblShipmentEventHistory();
                    shipmentEventHistory.ShipmentId = dto.ShipmentId;
                    shipmentEventHistory.ShipmentRouteStopId = dto.ShipmentRouteId;
                    shipmentEventHistory.StatusId = dto.StatusId;
                    shipmentEventHistory.UserId = dto.CreatedBy;
                    shipmentEventHistory.Event = "STATUS";
                    shipmentEventHistory.EventDateTime = dto.CreatedOn;
                    _dbContext.tblShipmentEventHistories.Add(shipmentEventHistory);
                    _dbContext.SaveChanges();
                }
            }

            if (dto.StatusId == 7 && dto.DriverDeliveryDeparture != null)
            {
                tblShipmentEventHistory shipmentEventHistory = new tblShipmentEventHistory();
                shipmentEventHistory.ShipmentId = dto.ShipmentId;
                shipmentEventHistory.ShipmentRouteStopId = dto.ShipmentRouteId;
                shipmentEventHistory.StatusId = dto.StatusId;
                shipmentEventHistory.UserId = dto.CreatedBy;
                shipmentEventHistory.Event = "STATUS";
                shipmentEventHistory.EventDateTime = dto.CreatedOn;
                _dbContext.tblShipmentEventHistories.Add(shipmentEventHistory);
                _dbContext.SaveChanges();
            }

            var objshipment = _dbContext.tblShipments.Where(x => x.ShipmentId == dto.ShipmentId).FirstOrDefault();

            if (objshipment != null)
            {
                if (!isValidStatus && (objshipment.StatusId != dto.StatusId || objshipment.SubStatusId != dto.SubStatusId) && dto.StatusId != 4 && dto.StatusId != 3)
                {
                    isEmailNeedSend = true;
                }
                if (!isValidStatus)
                {
                    objshipment.StatusId = dto.StatusId;
                    objshipment.SubStatusId = dto.SubStatusId;
                    objshipment.Reason = dto.Reason;
                    _dbContext.Entry(objshipment).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
            }

            if (!string.IsNullOrEmpty(dto.ShipmentComment))
            {
                tblShipmentCommment shipmentCommment = new tblShipmentCommment();
                shipmentCommment.ShipmentId = dto.ShipmentId;
                shipmentCommment.Comment = dto.ShipmentComment;
                shipmentCommment.CommentBy = "DR";
                shipmentCommment.CreatedOn = dto.CreatedOn;
                shipmentCommment.CreatedBy = dto.CreatedBy;
                _dbContext.tblShipmentCommments.Add(shipmentCommment);
                _dbContext.SaveChanges();

            }

            isEmailNeedToSend = isEmailNeedSend;
            return isSucccess;

        }
        #endregion

        #region  validate shipment
        public bool ValidateStatus(int? shipmentId, int? ShipmentRouteId, int statusId)
        {
            bool result = false;
            if (shipmentId > 0)
            {
                if (statusId == 12)
                {
                    var shipmentData = driverContext.tblShipmentRoutesStops.Where(x => x.ShippingId == shipmentId && x.DriverPickupArrival != null && x.IsDeleted == false).ToList();
                    if (shipmentData.Count > 0)
                    {
                        return false;
                    }
                }
                else if (statusId == 5)//LOADING AT PICK UP LOCATION
                {
                    var shipmentData = (from shipment in driverContext.tblShipments
                                        join route in driverContext.tblShipmentRoutesStops on shipment.ShipmentId equals route.ShippingId
                                        join freight in driverContext.tblShipmentFreightDetails on route.ShippingRoutesId equals freight.ShipmentRouteStopeId
                                        join proofOFTemp in driverContext.tblProofOfTemperatureImages on route.ShippingRoutesId equals proofOFTemp.ShipmentRouteId
                                        where shipment.ShipmentId == shipmentId && route.IsDeleted == false && freight.IsDeleted == false && route.ShippingRoutesId == ShipmentRouteId && freight.ShipmentBaseFreightDetailId == proofOFTemp.ShipmentFreightDetailId
                                        select new
                                        {
                                            shipmentId = shipment.ShipmentId,
                                            routeId = route.ShippingRoutesId,
                                            freightId = freight.ShipmentBaseFreightDetailId,

                                        }
                                         ).ToList();
                    if (shipmentData.Count > 0)
                    {
                        result = false;

                    }
                    else
                    {
                        bool isTempratureRequired = (from cust in driverContext.tblCustomerRegistrations
                                                     join shp in driverContext.tblShipments on cust.CustomerID equals shp.CustomerId
                                                     where shp.ShipmentId == shipmentId
                                                     select cust.IsTemperatureRequired
                                         ).FirstOrDefault();
                        if(isTempratureRequired)
                        {
                            result = true;
                        }
                        
                        else
                        {
                            result = false;
                        }
                    }
                }
                else if (statusId == 6)//IN-ROUTE TO DELIVERY LOCATION
                {
                    var shipmentData = driverContext.tblShipmentRoutesStops.Where(x => x.ShippingId == shipmentId && x.IsDeleted == false && x.DriverPickupDeparture == null).ToList();
                    if (shipmentData.Count > 0)
                    {
                        result = true;
                    }
                }
                else if (statusId == 7)//DELIVERED 
                {
                    var shipmentData = driverContext.tblShipmentRoutesStops.Where(x => x.ShippingId == shipmentId && x.IsDeleted == false && (x.DriverDeliveryArrival == null || x.DriverDeliveryDeparture == null || x.ReceiverName == null || x.DigitalSignature == null)).ToList();
                    if (shipmentData.Count > 0)
                    {
                        result = true;
                    }
                }

            }
            return result;
        }
        #endregion

        #region Save Proof of Temperature
        /// <summary>
        ///Save Proof of Temperature
        /// </summary>
        /// <returns></returns>
        public bool SaveProofOfTemperature(PreTripAddShipmentDetailDto dto)
        {

            if (dto.UploadedProofOfTempFile.ImageName != null || dto.ActualTemperature != null)
            {
                tblProofOfTemperatureImage image = new tblProofOfTemperatureImage();
                {
                    image.ShipmentFreightDetailId = dto.ShipmentFreightDetailId;
                    image.ShipmentRouteId = dto.ShipmentRouteId;
                    image.ImageName = dto.UploadedProofOfTempFile.ImageName;
                    image.ImageDescription = dto.UploadedProofOfTempFile.ImageDescription;
                    image.ImageUrl = dto.UploadedProofOfTempFile.ImageUrl;
                    image.ActualTemperature = dto.ActualTemperature;
                    image.IsLoading = dto.IsLoading;
                    image.CreatedBy = dto.CreatedBy;
                    image.CreatedOn = dto.CreatedOn;
                    _dbContext.tblProofOfTemperatureImages.Add(image);
                }
            }
            return _dbContext.SaveChanges() > 0;
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
            if (dto.UploadedDamagedFile != null && dto.UploadedDamagedFile.Count > 0)
            {
                foreach (var file in dto.UploadedDamagedFile)
                {
                    tblDamagedImage damagedImage = new tblDamagedImage();
                    damagedImage.ShipmentRouteID = dto.ShipmentRouteId;
                    damagedImage.ImageName = file.ImageName;
                    damagedImage.ImageDescription = file.ImageDescription;
                    damagedImage.ImageUrl = file.ImageUrl;
                    damagedImage.CreatedBy = dto.CreatedBy;
                    damagedImage.CreatedOn = dto.CreatedOn;
                    _dbContext.tblDamagedImages.Add(damagedImage);
                }
            }

            return _dbContext.SaveChanges() > 0;
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
            List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 5),
                        new SqlParameter("@ShippingRoutesId", ShippingRoutesId)
                    };

            var result = _dbContext.ExecuteStoredProcedure<ShipmentFreightDetailsDto>("usp_DriverModulePreTrip", sqlParameters);
            result = result.Count > 0 ? result : new List<ShipmentFreightDetailsDto>();
            return result;
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
            List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 6),
                        new SqlParameter("@ShippingRoutesId", ShippingRoutesId)
                    };

            var result = _dbContext.ExecuteStoredProcedure<PreTripTimmingDetailsDto>("usp_DriverModulePreTrip", sqlParameters);
            return result.Count > 0 ? result[0] : null;
        }
        #endregion

        #region Select Shipment Routes Stops
        /// <summary>
        /// Select Shipment Routes Stops
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>

        public List<GetShipmentRouteStopDTO> GetShipmentRoutesStopDetail(int ShippingRoutesId)
        {
            List<GetShipmentRouteStopDTO> getShipmentRouteStopDTOList = new List<GetShipmentRouteStopDTO>();

            try
            {
                var result = _dbContext.tblShipmentRoutesStops.Where(x => x.ShippingId == ShippingRoutesId).ToList();
                if (result != null && result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        GetShipmentRouteStopDTO getShipmentRouteStopDTO = new GetShipmentRouteStopDTO();
                        getShipmentRouteStopDTO.ActDeliveryDeparture = item.DriverDeliveryDeparture;
                        getShipmentRouteStopDTO.ActDeliveryArrival = item.DriverDeliveryArrival == null ? getShipmentRouteStopDTO.ActDeliveryArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(item.DriverDeliveryArrival));
                        getShipmentRouteStopDTO.ActPickupDeparture = item.DriverPickupDeparture == null ? getShipmentRouteStopDTO.ActPickupDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(item.DriverPickupDeparture));
                        getShipmentRouteStopDTO.ActPickupArrival = item.DriverPickupArrival == null ? getShipmentRouteStopDTO.ActPickupArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(item.DriverPickupArrival));

                        getShipmentRouteStopDTO.RouteNo = item.RouteNo;
                        getShipmentRouteStopDTO.ShipmentRouteStopId = item.ShippingRoutesId;
                        getShipmentRouteStopDTO.ShipmentId = item.ShippingId;
                        getShipmentRouteStopDTOList.Add(getShipmentRouteStopDTO);
                    }

                }
            }
            catch
            {
                throw;
            }
            return getShipmentRouteStopDTOList;
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
            try
            {
                var shipmentComment = new List<ShipmentCommentDTO>();
                shipmentComment = (from shpcomment in _dbContext.tblShipmentCommments
                                   where shpcomment.ShipmentId == shipmentId
                                   select new ShipmentCommentDTO
                                   {
                                       comment = (shpcomment.CommentBy + " :- " + shpcomment.Comment)
                                   }
                                 ).ToList();
                return shipmentComment;

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Get Shipment Damaged Files
        /// <summary>
        ///  Get Shipment Damaged Files By ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public List<ShipmentDamagedEditBindDto> GetShipmentDamagedFiles(int ShippingRoutesId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 7),
                        new SqlParameter("@ShippingRoutesId", ShippingRoutesId)
                    };

            var result = _dbContext.ExecuteStoredProcedure<ShipmentDamagedEditBindDto>("usp_DriverModulePreTrip", sqlParameters);
            result = result.Count > 0 ? result : new List<ShipmentDamagedEditBindDto>();
            result = result.Select(x => new ShipmentDamagedEditBindDto()
            {
                DamagedID = x.DamagedID,
                ShipmentRouteID = x.ShipmentRouteID,
                DamagedImage = x.DamagedImage,
                DamagedDescription = x.DamagedDescription,
                ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + x.ImageUrl),
                DamagedDate = Configurations.ConvertDateTime(x.DamagedDate)
            }).ToList();
            return result;
        }
        #endregion

        #region Get Shipment Proof Of Temp Files
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public List<ShipmentProofOfTempEditBind> GetShipmentProofOfTempFiles(int ShippingRoutesId, int ShipmentFreightDetailId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 8),
                        new SqlParameter("@ShippingRoutesId", ShippingRoutesId),
                        new SqlParameter("@ShipmentFreightDetailId", ShipmentFreightDetailId)
                    };

            var result = _dbContext.ExecuteStoredProcedure<ShipmentProofOfTempEditBind>("usp_DriverModulePreTrip", sqlParameters);
            result = result.Select(x => new ShipmentProofOfTempEditBind()
            {
                ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + x.ImageUrl),
                proofImageId = x.proofImageId,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ShipmentRouteID = x.ShipmentRouteID,
                ShipmentFreightDetailId = x.ShipmentFreightDetailId,
                proofActualTemp = x.proofActualTemp,
                IsLoading = x.IsLoading,
                ProofDate = Configurations.ConvertDateTime(x.ProofDate),
                ProofDescription = x.ProofDescription,
                ProofImage = x.ProofImage
            }).ToList();
            result = result.Count > 0 ? result : new List<ShipmentProofOfTempEditBind>();
            return result;
        }

        #endregion

        #region Delete proof of temprature
        /// <summary>
        ///  Delete proof of temprature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteProofOfTemprature(ShipmentProofOfTempEditBind model)
        {
            bool result = false;
            if (model != null)
            {
                var proofofTempImage = _dbContext.tblProofOfTemperatureImages.Where(x => x.ImageId == model.proofImageId).FirstOrDefault();
                if (proofofTempImage != null)
                {
                    proofofTempImage.IsDeleted = true;
                    proofofTempImage.DeletedBy = model.CreatedBy;
                    proofofTempImage.DeletedOn = model.CreatedOn;
                    _dbContext.Entry(proofofTempImage).State = EntityState.Modified;
                    result = _dbContext.SaveChanges() > 0 ? true : false;
                }

            }
            return result;
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
            bool result = false;
            if (model != null)
            {
                var proofofTempImage = _dbContext.tblDamagedImages.Where(x => x.DamagedID == model.DamagedID).FirstOrDefault();
                if (proofofTempImage != null)
                {
                    proofofTempImage.IsDeleted = true;
                    proofofTempImage.DeletedBy = model.CreatedBy;
                    proofofTempImage.DeletedOn = model.CreatedOn;
                    _dbContext.Entry(proofofTempImage).State = EntityState.Modified;
                    result = _dbContext.SaveChanges() > 0 ? true : false;
                }

            }
            return result;
        }
        #endregion

        #region Driver status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            //StatusId  8 = cancel and 10 = APPOINTMENT PENDING
            var statuslist = _dbContext.tblShipmentStatus.Where(x => x.IsActive && x.IsDeleted == false && x.IsShipment == true && x.StatusId != 1 && x.StatusId != 8 && x.StatusId != 10 && x.StatusId != 11).OrderBy(x => x.DisplayOrder);
            return AutoMapperServices<tblShipmentStatu, ShipmentStatusDTO>.ReturnObjectList(statuslist.ToList());


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
            var substatuslist = _dbContext.tblShipmentSubStatus.Where(x => x.IsActive && x.IsDeleted == false && x.StatusId == statusid).OrderBy(x => x.DisplayOrder);
            return AutoMapperServices<tblShipmentSubStatu, ShipmentSubStatusDTO>.ReturnObjectList(substatuslist.ToList());

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
            return driverContext.tblDrivers.Where(x => x.IsActive && x.IsDeleted == false && x.UserId == userId).Select(x => x.LanguageId).FirstOrDefault() ?? 1;

        }
        #endregion

        #region Update Signature And Name
        /// <summary>
        /// Update Signature And Name
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool UpdateSignaturePadDetail(PreTripAddShipmentDetailDto dto)
        {
            var objShipmentDetail = _dbContext.tblShipmentRoutesStops.Where(x => x.ShippingRoutesId == dto.ShipmentRouteId).FirstOrDefault();
            if (objShipmentDetail != null)
            {
                objShipmentDetail.DigitalSignature = dto.DigitalSignature;
                objShipmentDetail.DigitalSignaturePath = dto.DigitalSignaturePath;
            }
            return _dbContext.SaveChanges() > 0;
        }

        #endregion

        #region Select Signature And Name
        /// <summary>
        /// Select Signature And Name
        /// </summary>
        /// <param name="shipmentRouteId"></param>
        /// <returns></returns>
        public bool SelectSignaturePadDetail(int shipmentRouteId)
        {
            var objShipmentDetail = _dbContext.tblShipmentRoutesStops.Where(x => x.ShippingRoutesId == shipmentRouteId).FirstOrDefault();
            if (objShipmentDetail.DigitalSignature != null)
            {
                return true;

            }
            return false;
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
            tblDriverGpsTrakingHistory objGPSHistory = new tblDriverGpsTrakingHistory();
            objGPSHistory.UserId = dto.UserId;
            objGPSHistory.Latitude = dto.Latitude;
            objGPSHistory.longitude = dto.longitude;
            objGPSHistory.CreatedOn = Configurations.ConvertLocalToUTC(dto.CreatedOn);
            objGPSHistory.ShipmentId = dto.ShipmentId;
            objGPSHistory.ShipmentRouteId = dto.ShipmentRouteId;
            objGPSHistory.Event = dto.Event;
            _dbContext.tblDriverGpsTrakingHistories.Add(objGPSHistory);
            return _dbContext.SaveChanges() > 0;
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
            if (dto.UploadedProofOfTempFile.ImageName != null || dto.ActualTemperature != null)
            {
                tblProofOfTemperatureImage image = new tblProofOfTemperatureImage();

                image.ShipmentRouteId = dto.ShipmentRouteId;
                image.ShipmentFreightDetailId = dto.ShipmentFreightDetailId;
                image.ImageName = dto.UploadedProofOfTempFile.ImageName;
                image.ImageDescription = dto.UploadedProofOfTempFile.ImageDescription;
                image.ImageUrl = dto.UploadedProofOfTempFile.ImageUrl;
                image.ActualTemperature = dto.ActualTemperature;
                image.CreatedBy = dto.CreatedBy;
                image.CreatedOn = dto.CreatedOn;
                _dbContext.tblProofOfTemperatureImages.Add(image);

            }
            return _dbContext.SaveChanges() > 0;
        }

        #endregion

        #region Save Shipment Damage web-Camera  
        /// <summary>
        /// Save Damage web-Camera   
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool saveShipmentDamageWebCamera(SaveShipmentDamageWebCamDTO dto)
        {
            if (dto.UploadedDamagedFile != null)
            {
                tblDamagedImage DamagedImage = new tblDamagedImage();

                DamagedImage.ShipmentRouteID = dto.ShipmentRouteId;
                DamagedImage.ImageName = dto.UploadedDamagedFile.ImageName;
                DamagedImage.ImageDescription = dto.UploadedDamagedFile.ImageDescription;
                DamagedImage.ImageUrl = dto.UploadedDamagedFile.ImageUrl;
                DamagedImage.CreatedBy = dto.CreatedBy;
                DamagedImage.CreatedOn = dto.CreatedOn;
                _dbContext.tblDamagedImages.Add(DamagedImage);

            }
            return _dbContext.SaveChanges() > 0;
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
            try
            {
                var shipmentWaitingDetails = _dbContext.tblWatingNotifications.Any(t => t.ShipmentId == dto.ShipmentId && t.ShipmentRouteId == dto.ShipmentRouteId);

                if (shipmentWaitingDetails)
                {
                    var objShipmentDetail = _dbContext.tblWatingNotifications.Where(x => x.ShipmentId == dto.ShipmentId && x.ShipmentRouteId == dto.ShipmentRouteId).Select(x => x).FirstOrDefault();
                    {
                        if (objShipmentDetail != null && dto.PickupDepartedOn != null || dto.DeliveryArrivedOn != null || dto.DeliveryDepartedOn != null)
                        {
                            objShipmentDetail.PickupDepartedOn = dto.PickupDepartedOn == null ? dto.PickupDepartedOn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.PickupDepartedOn));
                            objShipmentDetail.DeliveryArrivedOn = dto.DeliveryArrivedOn == null ? dto.DeliveryArrivedOn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DeliveryArrivedOn));
                            objShipmentDetail.DeliveryDepartedOn = dto.DeliveryDepartedOn == null ? dto.DeliveryDepartedOn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DeliveryDepartedOn));

                        }
                    }
                }
                else
                {
                    var waitingDetails = _dbContext.tblShipmentRoutesStops.Any(y => y.ShippingId == dto.ShipmentId && y.ShippingRoutesId == dto.ShipmentRouteId && (y.IsPickUpWaitingTimeRequired == true || y.IsDeliveryWaitingTimeRequired == true));
                    if (waitingDetails)
                    {

                        tblWatingNotification objWaitingNotifi = new tblWatingNotification();
                        objWaitingNotifi.ShipmentId = dto.ShipmentId;
                        objWaitingNotifi.ShipmentRouteId = dto.ShipmentRouteId;
                        objWaitingNotifi.PickupArrivedOn = dto.PickupArrivedOn == null ? dto.PickupArrivedOn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.PickupArrivedOn));
                        objWaitingNotifi.IsDelivered = false;
                        objWaitingNotifi.IsEmailSentPWS = false;
                        objWaitingNotifi.IsEmailSentPWE = false;
                        objWaitingNotifi.IsEmailSentDWS = false;
                        objWaitingNotifi.IsEmailSentDWE = false;
                        objWaitingNotifi.DriverId = dto.DriverId;
                        objWaitingNotifi.EquipmentNo = dto.EquipmentNo;
                        objWaitingNotifi.CustomerId = dto.CustomerId;
                        objWaitingNotifi.PickUpLocationId = dto.PickUpLocationId;
                        objWaitingNotifi.DeliveryLocationId = dto.DeliveryLocationId;
                        _dbContext.tblWatingNotifications.Add(objWaitingNotifi);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return _dbContext.SaveChanges() > 0;
        }

        #endregion



        #endregion

        #region Section 2 --> Driver Fumigation


        #region Get Fumigation Pre-Trip Check Fumigation List
        /// <summary>
        /// Get Fumigation Pre-Trip Check List
        /// </summary>
        /// <returns></returns>

        public FumigationPreTripCheckUpDTO GetPreTripCheckFumigationList(int fumigationId)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 1),
                        new SqlParameter("@FumigationId", fumigationId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<FumigationPreTripCheckUpDTO>("usp_FumigationDetails", sqlParameters);
                return result != null && result.Count > 0 ? result[0] : null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Save Fumigation Pre-Trip Check List
        /// <summary>
        /// Save Fumigation Pre-Trip Check List
        /// </summary>
        /// <returns></returns>
        public bool SaveFumigationPreTripCheckList(FumigationPreTripCheckUpDTO dto)
        {
            try
            {

                if (dto.FumigationPreTripCheckupId == 0)
                {
                    tblFumigationPreTripCheckUp preTrip = AutoMapperServices<FumigationPreTripCheckUpDTO, tblFumigationPreTripCheckUp>.ReturnObject(dto);
                    preTrip.CreatedOn = Configurations.TodayDateTime;
                    preTrip.ModifiedOn = Configurations.TodayDateTime;

                    _dbContext.tblFumigationPreTripCheckUps.Add(preTrip);

                }
                else
                {
                    var dbExists = _dbContext.tblFumigationPreTripCheckUps.Find(dto.FumigationPreTripCheckupId);
                    if (dbExists != null)
                    {
                        dbExists.IsTiresGood = dto.IsTiresGood;
                        dbExists.IsBreaksGood = dto.IsBreaksGood;
                        dbExists.Fuel = dto.Fuel;
                        dbExists.LoadStraps = dto.LoadStraps;
                        dbExists.OverAllCondition = dto.OverAllCondition;
                        dbExists.ModifiedOn = Configurations.TodayDateTime;
                    }
                }

                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Fumigation Routes of Pre-Trip Shipment
        /// <summary>
        /// Get all the Fumigation routes of Pre-Trip Shipment by shipment id
        /// </summary>
        /// <returns></returns>

        public List<FumigationRoutesDTO> GetFumigationRoutes(int FumigationId, long userID)
        {
            try
            {
                var driveriId = _dbContext.tblDrivers.Where(x => x.UserId.Value == userID).FirstOrDefault().DriverID;
                var groupFumigationRoute = _dbContext.tblFumigationEquipmentNDrivers.Where(x => x.DriverId == driveriId && x.FumigationId == FumigationId).GroupBy(x => x.FumigationRoutsId).ToList();


                List<FumigationRoutesDTO> fumigationRoutesDTOs = new List<FumigationRoutesDTO>();
                foreach (var route in groupFumigationRoute)
                {
                    _dbContext = new ExecuteSQLStoredProceduce();
                    var fumigationRouteId = route.Key;
                    List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 2),
                        new SqlParameter("@FumigationRoutsId", fumigationRouteId),
                        new SqlParameter("@UserId", userID)
                    };
                    var result = _dbContext.ExecuteStoredProcedure<FumigationRoutesDTO>("usp_FumigationDetails", sqlParameters).FirstOrDefault();
                    var fumigationRoutlist = route.OrderByDescending(x => x.IsPickUp).ToList();
                    //--int index0
                    foreach (var item in fumigationRoutlist)
                    {
                        FumigationRoutesDTO fumigationRoutesDTO = new FumigationRoutesDTO();
                        fumigationRoutesDTO.RouteOrder = result.RouteOrder;
                        fumigationRoutesDTO.FumigationId = FumigationId;
                        fumigationRoutesDTO.FumigationRoutsId = fumigationRouteId.Value;
                        fumigationRoutesDTO.PickUpLocation = result.PickUpLocation;
                        fumigationRoutesDTO.PickupAddress = result.PickupAddress;
                        fumigationRoutesDTO.PickupDateTime = Configurations.ConvertUTCtoLocalTime(result.PickupDateTime);
                        fumigationRoutesDTO.DeliveryLocation = result.DeliveryLocation;
                        fumigationRoutesDTO.DeliveryAddress = result.DeliveryAddress;
                        fumigationRoutesDTO.DeliveryDateTime = Configurations.ConvertUTCtoLocalTime(result.DeliveryDateTime);
                        fumigationRoutesDTO.FumigationSite = result.FumigationSite;
                        fumigationRoutesDTO.FumigationId = FumigationId;
                        fumigationRoutesDTO.FumigationAddress = result.FumigationAddress;
                        fumigationRoutesDTO.FumigationDateTime = Configurations.ConvertUTCtoLocalTime(result.FumigationDateTime);
                        fumigationRoutesDTO.IsPickUp = item.IsPickUp.Value;
                        fumigationRoutesDTO.DriverId = driveriId;
                        fumigationRoutesDTO.EquipmentId = item.EquipmentId.Value;
                        var equipmentDetail = driverContext.tblEquipmentDetails.Where(x => x.EDID == item.EquipmentId.Value && x.IsDeleted == false).FirstOrDefault();
                        if (equipmentDetail != null)
                        {
                            fumigationRoutesDTO.EquipmentNo = equipmentDetail.EquipmentNo;
                        }
                        fumigationRoutesDTO.CustomerId = result.CustomerId.Value;
                        fumigationRoutesDTOs.Add(fumigationRoutesDTO);
                    }

                }

                return fumigationRoutesDTOs.OrderByDescending(x => x.IsPickUp).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Shipment Location Details
        /// <summary>
        ///  Get Shipment Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public FumigationLocationDetailsDTO GetFumigationRoutesDetails(int FumigationRoutsId)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 3),
                        new SqlParameter("@FumigationRoutsId", FumigationRoutsId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<FumigationLocationDetailsDTO>("usp_FumigationDetails", sqlParameters);
                if (result != null && result.Count > 0)
                {
                    result[0].PickUpArrivalDate = Configurations.ConvertDateTime(result[0].PickUpArrivalDate);
                    result[0].DeliveryArrive = Configurations.ConvertDateTime(result[0].DeliveryArrive);
                }
                var Fumigation = result.Count > 0 ? result[0] : null;
                return Fumigation;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Fumigation Freight Details
        /// <summary>
        ///  Get Fumigation Freight Details By ShippingRoutes Id
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        public List<FumigationFreightDetailsDto> GetFumigationFreightDetails(int FumigationRoutsId)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 4),
                        new SqlParameter("@FumigationRoutsId", FumigationRoutsId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<FumigationFreightDetailsDto>("usp_FumigationDetails", sqlParameters);
                result = result.Count > 0 ? result : new List<FumigationFreightDetailsDto>();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Save Fumigation Detail
        /// <summary>
        /// Save Fumigation Detail
        /// </summary>
        /// <returns></returns>

        public bool SaveFumigationtDetail(SaveFumigationDetailsDTO dto, out bool isEmailNeedToSend)
        {
            bool isSaveChange = false;
            try
            {
                bool isEmailNeedSend = false;
                var objFumigationDetail = _dbContext.tblFumigationRouts.Where(x => x.FumigationRoutsId == dto.FumigationRoutsId).FirstOrDefault();
                if (objFumigationDetail != null)
                {
                    objFumigationDetail.DriverPickupArrival = dto.DriverPickupArrival == null ? dto.DriverPickupArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverPickupArrival));
                    objFumigationDetail.DriverPickupDeparture = dto.DriverPickupDeparture == null ? dto.DriverPickupDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverPickupDeparture));
                    objFumigationDetail.DriverDeliveryArrival = dto.DriverDeliveryArrival == null ? dto.DriverDeliveryArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverDeliveryArrival));
                    objFumigationDetail.DriverDeliveryDeparture = dto.DriverDeliveryDeparture == null ? dto.DriverDeliveryDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverDeliveryDeparture));
                    objFumigationDetail.DepartureDate = dto.DepartureDate == null ? dto.DepartureDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DepartureDate));
                    objFumigationDetail.DriverFumigationIn = dto.DriverFumigationIn == null ? dto.DriverFumigationIn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverFumigationIn));
                    objFumigationDetail.DriverLoadingStartTime = dto.DriverLoadingStartTime == null ? dto.DriverLoadingStartTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverLoadingStartTime));
                    objFumigationDetail.DriverLoadingFinishTime = dto.DriverLoadingFinishTime == null ? dto.DriverLoadingFinishTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverLoadingFinishTime));
                    objFumigationDetail.DriverFumigationRelease = dto.DriverFumigationRelease == null ? dto.DriverFumigationRelease : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverFumigationRelease));
                    objFumigationDetail.ReceiverName = dto.ReceiverName;
                    _dbContext.Entry(objFumigationDetail).State = EntityState.Modified;
                    isSaveChange = _dbContext.SaveChanges() > 0;
                }
                var samePickupRecord = (from route in _dbContext.tblFumigationRouts
                                        join equipdriver in _dbContext.tblFumigationEquipmentNDrivers on route.FumigationRoutsId equals equipdriver.FumigationRoutsId
                                        join driver in _dbContext.tblDrivers on equipdriver.DriverId equals driver.DriverID
                                        where route.FumigationId == dto.FumigationId && route.PickUpLocation == objFumigationDetail.PickUpLocation && route.FumigationRoutsId != objFumigationDetail.FumigationRoutsId && driver.UserId == dto.CreatedBy && equipdriver.IsPickUp == true
                                        select route.FumigationRoutsId
                                    ).ToList();




                if (dto.StatusId == 12 && dto.DriverPickupArrival != null && samePickupRecord.Count > 0)
                {
                    foreach (var route in samePickupRecord)
                    {
                        var samefumigationDetail = _dbContext.tblFumigationRouts.Where(x => x.FumigationRoutsId == route).FirstOrDefault();
                        if (samefumigationDetail != null)
                        {

                            samefumigationDetail.DriverPickupArrival = dto.DriverPickupArrival == null ? dto.DriverPickupArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverPickupArrival));
                            _dbContext.Entry(samefumigationDetail).State = EntityState.Modified;
                            _dbContext.SaveChanges();
                        }
                    }
                }




                if (dto.StatusId == 5 && dto.DriverPickupDeparture != null && samePickupRecord.Count > 0)
                {

                    foreach (var route in samePickupRecord)
                    {
                        var samefumigationDetail = _dbContext.tblFumigationRouts.Where(x => x.FumigationRoutsId == route && x.DriverLoadingFinishTime != null).FirstOrDefault();
                        if (samefumigationDetail != null)
                        {
                            samefumigationDetail.DriverPickupDeparture = dto.DriverPickupDeparture == null ? dto.DriverPickupDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverPickupDeparture));
                            _dbContext.Entry(samefumigationDetail).State = EntityState.Modified;
                            _dbContext.SaveChanges();
                        }
                    }

                }

                var sameFumigationRecord = (from route in _dbContext.tblFumigationRouts
                                            join equipdriver in _dbContext.tblFumigationEquipmentNDrivers on route.FumigationRoutsId equals equipdriver.FumigationRoutsId
                                            join driver in _dbContext.tblDrivers on equipdriver.DriverId equals driver.DriverID
                                            where route.FumigationId == dto.FumigationId && route.FumigationSite == objFumigationDetail.FumigationSite && route.FumigationRoutsId != objFumigationDetail.FumigationRoutsId && driver.UserId == dto.CreatedBy && equipdriver.IsPickUp == true
                                            select route.FumigationRoutsId
                                 ).ToList();

                if (dto.StatusId == 9 && dto.DriverFumigationIn != null && sameFumigationRecord.Count > 0)
                {

                    foreach (var route in sameFumigationRecord)
                    {
                        var samefumigationDetail = _dbContext.tblFumigationRouts.Where(x => x.FumigationRoutsId == route && x.DriverPickupDeparture != null).FirstOrDefault();
                        if (samefumigationDetail != null)
                        {

                            samefumigationDetail.DriverFumigationIn = dto.DriverFumigationIn == null ? dto.DriverFumigationIn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverFumigationIn));
                            _dbContext.Entry(samefumigationDetail).State = EntityState.Modified;
                            _dbContext.SaveChanges();
                        }
                    }


                }
                if (dto.StatusId == 6 && dto.DriverFumigationRelease != null && sameFumigationRecord.Count > 0)
                {
                    foreach (var route in sameFumigationRecord)
                    {
                        var samefumigationDetail = _dbContext.tblFumigationRouts.Where(x => x.FumigationRoutsId == route).FirstOrDefault();
                        if (samefumigationDetail != null)
                        {

                            samefumigationDetail.DriverFumigationRelease = dto.DriverFumigationRelease == null ? dto.DriverFumigationRelease : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverFumigationRelease));
                            samefumigationDetail.DepartureDate = dto.DepartureDate == null ? dto.DepartureDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DepartureDate));
                            _dbContext.Entry(samefumigationDetail).State = EntityState.Modified;
                            _dbContext.SaveChanges();
                        }
                    }

                }
                var sameDeliveryRecord = (from route in _dbContext.tblFumigationRouts
                                          join equipdriver in _dbContext.tblFumigationEquipmentNDrivers on route.FumigationRoutsId equals equipdriver.FumigationRoutsId
                                          join driver in _dbContext.tblDrivers on equipdriver.DriverId equals driver.DriverID
                                          where route.FumigationId == dto.FumigationId && route.DeliveryLocation == objFumigationDetail.DeliveryLocation && route.FumigationRoutsId != objFumigationDetail.FumigationRoutsId && driver.UserId == dto.CreatedBy && equipdriver.IsPickUp == false
                                          select route.FumigationRoutsId
                               ).ToList();
                if (dto.StatusId == 7 && (dto.DriverDeliveryArrival != null || dto.DriverDeliveryDeparture != null) && sameDeliveryRecord.Count > 0)
                {
                    foreach (var route in sameDeliveryRecord)
                    {
                        var samefumigationDetail = _dbContext.tblFumigationRouts.Where(x => x.FumigationRoutsId == route).FirstOrDefault();
                        if (samefumigationDetail != null)
                        {
                            samefumigationDetail.ReceiverName = dto.ReceiverName;
                            samefumigationDetail.DriverDeliveryArrival = dto.DriverDeliveryArrival == null ? dto.DriverDeliveryArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverDeliveryArrival));
                            samefumigationDetail.DriverDeliveryDeparture = dto.DriverDeliveryDeparture == null ? dto.DriverDeliveryDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DriverDeliveryDeparture));
                            samefumigationDetail.DigitalSignature = objFumigationDetail.DigitalSignature;
                            samefumigationDetail.DigitalSignaturePath = objFumigationDetail.DigitalSignaturePath;
                            _dbContext.Entry(samefumigationDetail).State = EntityState.Modified;
                            _dbContext.SaveChanges();
                        }
                    }

                }
                if (dto.StatusId == 5)
                {
                    var proofOfTemp = _dbContext.tblFumigationProofOfTemperatureImages.Where(X => X.FumigationRouteId == dto.FumigationRoutsId && X.IsDeleted == false).OrderByDescending(x => x.ImageId).FirstOrDefault();
                    if (proofOfTemp != null)
                    {
                        float number;
                        if (float.TryParse(proofOfTemp.ActualTemperature, out number))
                        {
                            var objFreightDetail = _dbContext.tblFumigationRouts.Where(x => x.FumigationRoutsId == dto.FumigationRoutsId && x.Temperature != null && x.IsDeleted == false).Max(x => x.Temperature);

                            if (objFreightDetail != null && (float)(objFreightDetail) != 0)
                            {
                                float diff = ((float)(objFreightDetail) - number);
                                if (diff > 10 || diff < -10)
                                {
                                    dto.StatusId = 3;
                                    dto.SubStatusId = 9;
                                }
                            }
                        }
                        if (dto.StatusId == 5 && dto.DriverLoadingStartTime != null)
                        {
                            var exist = _dbContext.tblFumigationEventHistories.Any(x => x.FumigationId == dto.FumigationId && x.FumigationRouteStopId == dto.FumigationRoutsId && x.StatusId == 5);
                            if (!exist)
                            {
                                tblFumigationEventHistory fumigationEventHistory = new tblFumigationEventHistory();
                                fumigationEventHistory.FumigationId = dto.FumigationId;
                                fumigationEventHistory.FumigationRouteStopId = dto.FumigationRoutsId;
                                fumigationEventHistory.StatusId = 5;
                                fumigationEventHistory.UserId = dto.CreatedBy;
                                fumigationEventHistory.Event = "STATUS";
                                fumigationEventHistory.EventDateTime = dto.CreatedOn;
                                _dbContext.tblFumigationEventHistories.Add(fumigationEventHistory);
                                _dbContext.SaveChanges();
                            }

                        }
                    }
                }

                var isValidStatus = ValidateFumigationStatus(dto.FumigationId, dto.FumigationRoutsId, dto.StatusId);

                var objFumigationStatusHistoy = _dbContext.tblFumigationStatusHistories.Where(x => x.FumigationId == dto.FumigationId).OrderByDescending(x => x.FumigationStatusHistoryId).FirstOrDefault();

                if (isValidStatus && objFumigationStatusHistoy != null && objFumigationStatusHistoy.StatusId != dto.StatusId && dto.StatusId != 4 && dto.StatusId != 3)
                {
                    tblFumigationStatusHistory fumigationStatusHistory = new tblFumigationStatusHistory();
                    fumigationStatusHistory.FumigationId = dto.FumigationId;
                    fumigationStatusHistory.StatusId = dto.StatusId;
                    fumigationStatusHistory.SubStatusId = dto.SubStatusId;
                    fumigationStatusHistory.Reason = dto.Reason;
                    fumigationStatusHistory.CreatedBy = dto.CreatedBy;
                    fumigationStatusHistory.CreatedOn = dto.CreatedOn;

                    _dbContext.tblFumigationStatusHistories.Add(fumigationStatusHistory);
                    _dbContext.SaveChanges();

                    if (dto.StatusId != 5 && dto.StatusId != 7)
                    {
                        tblFumigationEventHistory fumigationEventHistory = new tblFumigationEventHistory();
                        fumigationEventHistory.FumigationId = dto.FumigationId;
                        //fumigationEventHistory.FumigationRouteStopId = dto.FumigationRoutsId;
                        fumigationEventHistory.StatusId = dto.StatusId;
                        fumigationEventHistory.UserId = dto.CreatedBy;
                        fumigationEventHistory.Event = "STATUS";
                        fumigationEventHistory.EventDateTime = dto.CreatedOn;
                        _dbContext.tblFumigationEventHistories.Add(fumigationEventHistory);
                        _dbContext.SaveChanges();
                    }
                }

                if (dto.StatusId == 7 && dto.DriverDeliveryDeparture != null)
                {
                    tblFumigationEventHistory fumigationEventHistory = new tblFumigationEventHistory();
                    fumigationEventHistory.FumigationId = dto.FumigationId;
                    fumigationEventHistory.FumigationRouteStopId = dto.FumigationRoutsId;
                    fumigationEventHistory.StatusId = dto.StatusId;
                    fumigationEventHistory.UserId = dto.CreatedBy;
                    fumigationEventHistory.Event = "STATUS";
                    fumigationEventHistory.EventDateTime = dto.CreatedOn;
                    _dbContext.tblFumigationEventHistories.Add(fumigationEventHistory);
                    _dbContext.SaveChanges();
                }

                var objFumigation = _dbContext.tblFumigations.Where(x => x.FumigationId == dto.FumigationId).FirstOrDefault();

                if (isValidStatus && objFumigation != null)
                {
                    if (isValidStatus && (objFumigation.StatusId != dto.StatusId || objFumigation.SubStatusId != dto.SubStatusId) && dto.StatusId != 4 && dto.StatusId != 3)
                    {
                        isEmailNeedSend = true;
                    }
                    objFumigation.StatusId = dto.StatusId;
                    objFumigation.SubStatusId = dto.SubStatusId;
                    objFumigation.Reason = dto.Reason;
                    _dbContext.SaveChanges();
                }

                if (!string.IsNullOrEmpty(dto.FumigationComment))
                {
                    tblFumigationComment fumigationCommment = new tblFumigationComment();
                    fumigationCommment.FumigationId = dto.FumigationId;
                    fumigationCommment.Comment = dto.FumigationComment;
                    fumigationCommment.CommentBy = "DR";
                    fumigationCommment.CreatedOn = dto.CreatedOn;
                    fumigationCommment.CreatedBy = dto.CreatedBy;
                    _dbContext.tblFumigationComments.Add(fumigationCommment);
                    _dbContext.SaveChanges();

                }
                isEmailNeedToSend = isEmailNeedSend;
                return isSaveChange;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                var fumigationComment = new List<FumigationCommentDTO>();
                fumigationComment = (from fumcomment in _dbContext.tblFumigationComments
                                     where fumcomment.FumigationId == fumigationId
                                     select new FumigationCommentDTO
                                     {
                                         Comment = (fumcomment.CommentBy + " :- " + fumcomment.Comment)
                                     }
                                 ).ToList();
                return fumigationComment;

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region  Validate Fumigation Status
        /// <summary>
        /// Validate Fumigation Status
        /// </summary>
        /// <returns></returns>
        public bool ValidateFumigationStatus(int? fumigationId, int? fumigationRouteId, int statusId)
        {
            try
            {


                var result = true;
                if (fumigationId > 0)
                {
                    var fumigationRoutes = driverContext.tblFumigationRouts.Where(x => x.FumigationId == fumigationId && x.IsDeleted == false).ToList();
                    if (statusId == 12)// In 
                    {
                        var checkPickupArrival = fumigationRoutes.Where(x => x.DriverPickupArrival == null).ToList();
                        if (checkPickupArrival.Count > 0)
                        {
                            // result = false;
                        }
                    }
                    else if (statusId == 9)//Fumigation
                    {
                        var checkLoadingFinish = fumigationRoutes.Where(x => x.DriverLoadingFinishTime == null).ToList();
                        if (checkLoadingFinish.Count > 0)
                        {
                            result = false;
                        }
                    }
                    else if (statusId == 6)//IN-ROUTE
                    {
                        var checkFumigationIn = fumigationRoutes.Where(x => x.DriverFumigationIn == null).ToList();
                        if (checkFumigationIn.Count > 0)
                        {
                            result = false;
                        }


                    }
                    else if (statusId == 7)//Delivered
                    {
                        //var checkDepartureDate = fumigationRoutes.Where(x => x.DepartureDate == null).ToList();
                        //if (checkDepartureDate.Count > 0)
                        //{
                        //    result = false;
                        //}

                        var checkDeliveryDepart = fumigationRoutes.Where(x => x.DriverDeliveryDeparture == null && x.FumigationId == fumigationId).ToList();
                        if (checkDeliveryDepart.Count > 0)
                        {
                            result = false;
                        }
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

        #region Save Proof of Temperature
        /// <summary>
        ///Save Proof of Temperature
        /// </summary>
        /// <returns></returns>
        public bool SaveFumigationProofOfTemperature(SaveFumigationDetailsDTO dto)
        {
            try
            {

                if (dto.UploadedProofOfTempFile.ImageName != null || dto.ActualTemperature != null)
                {
                    tblFumigationProofOfTemperatureImage Image = new tblFumigationProofOfTemperatureImage();
                    {
                        Image.FumigationRouteId = dto.FumigationRoutsId;
                        Image.ImageName = dto.UploadedProofOfTempFile.ImageName;
                        Image.ImageDescription = dto.UploadedProofOfTempFile.ImageDescription;

                        Image.ImageUrl = dto.UploadedProofOfTempFile.ImageUrl;
                        Image.ActualTemperature = dto.ActualTemperature;
                        Image.IsLoading = true;
                        ////if (dto.DeliveryTemp !="" && dto.DeliveryTemp!=null)
                        ////{
                        ////    Image.ActualTemperature = dto.DeliveryTemp;
                        ////    Image.IsLoading = false;
                        ////}
                        Image.CreatedBy = dto.CreatedBy;
                        Image.CreatedOn = dto.CreatedOn;
                        _dbContext.tblFumigationProofOfTemperatureImages.Add(Image);
                    }
                }

                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Save Fumigation Damaged Files
        /// <summary>
        /// Save Fumigation Damaged Files
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public bool SaveFumigationDamagedFiles(SaveFumigationDetailsDTO dto)
        {
            try
            {
                if (dto.UploadedDamagedFile != null && dto.UploadedDamagedFile.Count > 0)
                {
                    foreach (var file in dto.UploadedDamagedFile)
                    {
                        tblFumigationDamagedImage DamagedImage = new tblFumigationDamagedImage();
                        DamagedImage.FumigationRouteId = dto.FumigationRoutsId;
                        DamagedImage.ImageName = file.ImageName;
                        DamagedImage.ImageDescription = file.ImageDescription;
                        DamagedImage.ImageUrl = file.ImageUrl;
                        DamagedImage.CreatedBy = dto.CreatedBy;
                        DamagedImage.CreatedOn = dto.CreatedOn;
                        _dbContext.tblFumigationDamagedImages.Add(DamagedImage);
                    }
                }
                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get Fumigation Damaged Files
        /// <summary>
        ///  Get Fumigation Damaged Files By ShippingRoutes Id
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        public List<FumigationDamagedEditBindDto> GetFumigationDamagedFiles(int FumigationRoutsId)
        {
            try
            {

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 5),
                        new SqlParameter("@FumigationRoutsId", FumigationRoutsId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<FumigationDamagedEditBindDto>("usp_FumigationDetails", sqlParameters);
                result = result.Count > 0 ? result : new List<FumigationDamagedEditBindDto>();
                result = result.Select(x => new FumigationDamagedEditBindDto()
                {
                    DamagedID = x.DamagedID,
                    FumigationRouteId = x.FumigationRouteId,
                    DamagedImage = x.DamagedImage,
                    DamagedDescription = x.DamagedDescription,
                    ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + x.ImageUrl),
                    DamagedDate = Configurations.ConvertDateTime(x.DamagedDate)
                }).ToList();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Fumigation Proof Of Temp Files
        /// <summary>
        /// Get Fumigation Proof Of Temp Files
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        public List<FumigationProofOfTempEditBind> GetFumigationProofOfTempFiles(int FumigationRoutsId)
        {
            try
            {

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 6),
                        new SqlParameter("@FumigationRoutsId", FumigationRoutsId),

                    };

                var result = _dbContext.ExecuteStoredProcedure<FumigationProofOfTempEditBind>("usp_FumigationDetails", sqlParameters);


                result = result.Select(x => new FumigationProofOfTempEditBind()
                {
                    ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + x.ImageUrl),
                    proofImageId = x.proofImageId,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    FumigationRouteId = x.FumigationRouteId,
                    proofActualTemp = x.proofActualTemp,
                    ProofDate = Configurations.ConvertDateTime(x.ProofDate),
                    ProofDescription = x.ProofDescription,
                    ProofImage = x.ProofImage,
                    IsApproved = x.IsApproved,
                    IsLoading = x.IsLoading

                }).ToList();
                result = result.Count > 0 ? result : new List<FumigationProofOfTempEditBind>();
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get Driver Actual Timings for Arrival & departure
        /// <summary>
        /// Get Driver Actual Timings for Arrival & departure
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>

        public DriverActualTimmingsDTO GetDriverActualTimings(int FumigationRoutsId)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 7),
                        new SqlParameter("@FumigationRoutsId", FumigationRoutsId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<DriverActualTimmingsDTO>("usp_FumigationDetails", sqlParameters);
                return result.Count > 0 ? result[0] : null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update Fumigation Signature And Name
        /// <summary>
        /// Update Fumigation Signature And Name
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool UpdateFumigationSignaturePadDetail(SaveFumigationDetailsDTO dto)
        {
            try
            {
                var objShipmentDetail = _dbContext.tblFumigationRouts.Where(x => x.FumigationRoutsId == dto.FumigationRoutsId).FirstOrDefault();
                if (objShipmentDetail != null)
                {
                    objShipmentDetail.DigitalSignaturePath = dto.DigitalSignaturePath;
                    objShipmentDetail.DigitalSignature = dto.DigitalSignature;

                }
                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Select Fumigation Signature And Name
        /// <summary>
        /// Select Fumigation Signature And Name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SelectFumigationSignaturePadDetail(int fumigationRoutId)
        {
            try
            {
                var objShipmentDetail = _dbContext.tblFumigationRouts.Where(x => x.FumigationRoutsId == fumigationRoutId).FirstOrDefault();
                if (objShipmentDetail.DigitalSignature != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Delete proof of temprature
        /// <summary>
        ///  Delete proof of temprature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteFumigationProofOfTemprature(FumigationProofOfTempEditBind model)
        {
            try
            {
                bool result = false;
                if (model != null)
                {
                    var proofofTempImage = _dbContext.tblFumigationProofOfTemperatureImages.Where(x => x.ImageId == model.proofImageId).FirstOrDefault();
                    if (proofofTempImage != null)
                    {
                        proofofTempImage.IsDeleted = true;
                        proofofTempImage.DeletedBy = model.CreatedBy;
                        proofofTempImage.DeletedOn = model.CreatedOn;
                        _dbContext.Entry(proofofTempImage).State = EntityState.Modified;
                        result = _dbContext.SaveChanges() > 0 ? true : false;
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

        #region Delete Damage Files
        /// <summary>
        /// Delete Damage Files
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public bool DeleteFumigationDamageFiles(FumigationDamagedEditBindDto model)
        {
            try
            {

                bool result = false;
                if (model != null)
                {
                    var proofofTempImage = _dbContext.tblFumigationDamagedImages.Where(x => x.DamagedID == model.DamagedID).FirstOrDefault();
                    if (proofofTempImage != null)
                    {
                        proofofTempImage.IsDeleted = true;
                        proofofTempImage.DeletedBy = model.CreatedBy;
                        proofofTempImage.DeletedOn = model.CreatedOn;
                        _dbContext.Entry(proofofTempImage).State = EntityState.Modified;
                        result = _dbContext.SaveChanges() > 0 ? true : false;
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

        #region GPS Tracker 
        /// <summary>
        /// GPS Tracker 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool SaveFumigationGPSTracker(SaveFumigationGpsTrackingHistoryDTO dto)
        {
            try
            {
                tblDriverGpsTrakingHistory objGPSHistory = new tblDriverGpsTrakingHistory();
                objGPSHistory.UserId = dto.UserId;
                objGPSHistory.Latitude = dto.Latitude;
                objGPSHistory.longitude = dto.longitude;
                objGPSHistory.CreatedOn = Configurations.ConvertLocalToUTC(dto.CreatedOn);
                objGPSHistory.FumigationId = dto.FumigationId;
                objGPSHistory.FumigationRoutsId = dto.FumigationRoutsId;
                objGPSHistory.Event = dto.Event;
                _dbContext.tblDriverGpsTrakingHistories.Add(objGPSHistory);
                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region fumigation status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetFumigationStatusList()
        {
            try
            {

                var statuslist = _dbContext.tblShipmentStatus.Where(x => x.IsActive && x.IsDeleted == false && x.IsFumigation == true && x.StatusId != 1 && x.StatusId != 8 && x.StatusId != 10 && x.StatusId != 11).OrderBy(x => x.FumigationDisplayOrder).ToList();
                return AutoMapperServices<tblShipmentStatu, ShipmentStatusDTO>.ReturnObjectList(statuslist.ToList());
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                if (dto.UploadedProofOfTempFile.ImageName != null || dto.ActualTemperature != null)
                {
                    tblFumigationProofOfTemperatureImage image = new tblFumigationProofOfTemperatureImage();

                    image.FumigationRouteId = dto.FumigationRoutsId;
                    image.ImageName = dto.UploadedProofOfTempFile.ImageName;
                    image.ImageDescription = dto.UploadedProofOfTempFile.ImageDescription;
                    image.ImageUrl = dto.UploadedProofOfTempFile.ImageUrl;
                    image.ActualTemperature = dto.ActualTemperature;
                    image.CreatedBy = dto.CreatedBy;
                    image.CreatedOn = dto.CreatedOn;
                    _dbContext.tblFumigationProofOfTemperatureImages.Add(image);

                }
                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                if (dto.UploadedDamagedFile != null)
                {
                    tblFumigationDamagedImage damagedImage = new tblFumigationDamagedImage();

                    damagedImage.FumigationRouteId = dto.FumigationRouteId;
                    damagedImage.ImageName = dto.UploadedDamagedFile.ImageName;
                    damagedImage.ImageDescription = dto.UploadedDamagedFile.ImageDescription;
                    damagedImage.ImageUrl = dto.UploadedDamagedFile.ImageUrl;
                    damagedImage.CreatedBy = dto.CreatedBy;
                    damagedImage.CreatedOn = dto.CreatedOn;
                    _dbContext.tblFumigationDamagedImages.Add(damagedImage);

                }
                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                var ShipmentWaitingDetails = _dbContext.tblFumigationWaitingNotifications.Any(t => t.FumigationId == dto.FumigationId && t.FumigationRoutsId == dto.FumigationRoutsId);

                if (ShipmentWaitingDetails)
                {
                    var objShipmentDetail = _dbContext.tblFumigationWaitingNotifications.Where(x => x.FumigationId == dto.FumigationId && x.FumigationRoutsId == dto.FumigationRoutsId).Select(x => x).FirstOrDefault();
                    {
                        if (objShipmentDetail != null)
                        {
                            objShipmentDetail.PickupDepartedOn = dto.PickupDepartedOn == null ? dto.PickupDepartedOn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.PickupDepartedOn));
                            objShipmentDetail.DeliveryArrivedOn = dto.DeliveryArrivedOn == null ? dto.DeliveryArrivedOn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DeliveryArrivedOn));
                            objShipmentDetail.DeliveryDepartedOn = dto.DeliveryDepartedOn == null ? dto.DeliveryDepartedOn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.DeliveryDepartedOn));

                        }
                    }
                }
                else
                {

                    tblFumigationWaitingNotification objFumiWaitingNotifi = new tblFumigationWaitingNotification();
                    objFumiWaitingNotifi.FumigationId = dto.FumigationId;
                    objFumiWaitingNotifi.FumigationRoutsId = dto.FumigationRoutsId;
                    objFumiWaitingNotifi.PickupArrivedOn = dto.PickupArrivedOn == null ? dto.PickupArrivedOn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(dto.PickupArrivedOn));
                    objFumiWaitingNotifi.IsDelivered = false;
                    objFumiWaitingNotifi.IsEmailSentPWS = false;
                    objFumiWaitingNotifi.IsEmailSentPWE = false;
                    objFumiWaitingNotifi.IsEmailSentDWS = false;
                    objFumiWaitingNotifi.IsEmailSentDWE = false;
                    objFumiWaitingNotifi.DriverId = dto.DriverId;
                    objFumiWaitingNotifi.EquipmentNo = dto.EquipmentNo;
                    objFumiWaitingNotifi.CustomerId = dto.CustomerId;
                    objFumiWaitingNotifi.PickUpLocationId = dto.PickUpLocationId;
                    objFumiWaitingNotifi.DeliveryLocationId = dto.DeliveryLocationId;
                    _dbContext.tblFumigationWaitingNotifications.Add(objFumiWaitingNotifi);
                }


            }
            catch (Exception)
            {
                throw;
            }

            return _dbContext.SaveChanges() > 0;
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
            try
            {

                var shipmentRoute = driverContext.tblFumigationRouts.Where(x => x.DriverFumigationIn == null && x.FumigationId == fumigationId && x.IsDeleted == false).ToList();
                if (shipmentRoute.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Fumigation Last Status
        /// <summary>
        /// Get Last Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public int GetLastStatus(int statusId, int fumigationId)
        {
            try
            {

                var shipmentRoute = driverContext.tblFumigationStatusHistories.Where(x => x.FumigationId == fumigationId && x.StatusId != 3 && x.StatusId != 4 && x.StatusId != 13).OrderByDescending(x => x.FumigationStatusHistoryId).Select(x => x.StatusId).FirstOrDefault();
                if (shipmentRoute > 0)
                {
                    return shipmentRoute;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Shipment Last Status
        /// <summary>
        /// Get Last Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public int GetShipmentLastStatus(int statusId, int shipmentId)
        {
            try
            {

                var shipmentRoute = driverContext.tblShipmentStatusHistories.Where(x => x.ShipmentId == shipmentId && x.StatusId != 3 && x.StatusId != 4 && x.StatusId != 13).OrderByDescending(x => x.ShipmentStatusHistoryId).Select(x => x.StatusId).FirstOrDefault();
                if (shipmentRoute > 0)
                {
                    return shipmentRoute;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region validate required field
        public List<FumigationRoutesDTO> ValidateReuiredField(int statusId, int fumigationId)
        {
            try
            {

                var routeDetail = new List<FumigationRoutesDTO>();

                if (fumigationId > 0)
                {
                    if (statusId == 6)//6=>IN-ROUTE TO DELIVERY LOCATION
                    {
                        routeDetail = (from route in driverContext.tblFumigationRouts
                                       where route.FumigationId == fumigationId && (route.DriverPickupArrival == null || route.DriverPickupDeparture == null) && route.IsDeleted == false
                                       select new FumigationRoutesDTO
                                       {
                                           RouteNo = route.RouteNo,
                                           FumigationRoutsId = route.FumigationRoutsId,
                                       }
                                       ).ToList();

                    }
                    else if (statusId == 9)
                    {
                        routeDetail = (from route in driverContext.tblFumigationRouts
                                       where route.FumigationId == fumigationId && (route.DriverFumigationIn == null) && route.IsDeleted == false
                                       select new FumigationRoutesDTO
                                       {
                                           RouteNo = route.RouteNo,
                                           FumigationRoutsId = route.FumigationRoutsId,
                                       }
                                       ).ToList();
                    }
                    else if (statusId == 7)
                    {
                        routeDetail = (from route in driverContext.tblFumigationRouts
                                       where route.FumigationId == fumigationId && (route.DriverDeliveryDeparture == null) && route.IsDeleted == false
                                       select new FumigationRoutesDTO
                                       {
                                           RouteNo = route.RouteNo,
                                           FumigationRoutsId = route.FumigationRoutsId,
                                       }
                                       ).ToList();
                    }


                }
                return routeDetail;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #endregion

    }

}
