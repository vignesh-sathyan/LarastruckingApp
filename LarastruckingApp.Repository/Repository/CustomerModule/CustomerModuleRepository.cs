using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.CustomerFumigation;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.Driver_Fumigation;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using LarastruckingApp.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository.CustomerModule
{
    public class CustomerModuleRepository : ICustomerModuleRepository
    {
        #region Private Members
        /// <summary>
        /// defining Private members
        /// </summary>
        private  ExecuteSQLStoredProceduce _dbContext = null;
        private readonly LarastruckingDBEntities costumerContext;
        #endregion

        #region Contructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="IDriverModuleDAL"></param>

        public CustomerModuleRepository(ExecuteSQLStoredProceduce dbContext)
        {
            _dbContext = dbContext;
            costumerContext = new LarastruckingDBEntities();
        }
        #endregion

        #region Customer dashboard => shipment details  

        #region Get All Quotes Assigned to Customer
        /// <summary>
        /// Get All Quotes Assigned to Customer
        /// </summary>
        /// <returns></returns>
        public List<CustomerQuotesInfoDto> GetAllQuotes(DataTableFilterDto dto, int userId)
        {
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
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@SearchTerm", dto.SearchTerm),
                        new SqlParameter("@SortColumn", dto.SortColumn),
                        new SqlParameter("@SortOrder", dto.SortOrder),
                        new SqlParameter("@PageNumber", dto.PageNumber),
                        new SqlParameter("@PageSize", dto.PageSize),
                        totalCount
                    };

                var result = _dbContext.ExecuteStoredProcedure<CustomerQuotesInfoDto>("usp_CustomerDashoard", sqlParameters);
                dto.TotalCount = Convert.ToInt32(totalCount.Value);
                result = result != null && result.Count > 0 ? result : new List<CustomerQuotesInfoDto>();
                if (result != null && result.Count > 0)
                {
                    foreach (var data in result)
                    {
                        data.PickDateTime = Configurations.ConvertDateTime(data.PickDateTime);
                        data.DeliveryDateTime = Configurations.ConvertDateTime(data.DeliveryDateTime);

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

        #region Get Multiple Routes by Shipement and Costomer
        /// <summary>
        ///  Get Multiple Routes by Shipement and Costomer
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public List<CustomerShipmentRoutesDto> GetCustomerShipmentRoutes(int shipmentId)
        {
            try
            {


                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 2),
                        new SqlParameter("@ShipmentId", shipmentId)

                    };

                var result = _dbContext.ExecuteStoredProcedure<CustomerShipmentRoutesDto>("usp_CustomerModule", sqlParameters);
                result = result != null && result.Count > 0 ? result : new List<CustomerShipmentRoutesDto>();
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
        public CustomerShipLocationDetailsDto GetCustomerShipmentRoutesDetails(int ShippingRoutesId)
        {
            try
            {


                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 3),
                        new SqlParameter("@ShippingRoutesId", ShippingRoutesId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<CustomerShipLocationDetailsDto>("usp_CustomerModule", sqlParameters);
                if (result != null && result.Count > 0)
                {
                    result[0].PickUpArrivalDate = Configurations.ConvertDateTime(result[0].PickUpArrivalDate);
                    result[0].DeliveryArrive = Configurations.ConvertDateTime(result[0].DeliveryArrive);
                }
                var shipments = result.Count > 0 ? result[0] : null;
                return shipments;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                CustomerShipmentTrackDto customerShipmentTrackDto = new CustomerShipmentTrackDto();
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@ShipmentId", shipmentId)

                    };
                _dbContext = new ExecuteSQLStoredProceduce();
                var customerTrackDetails = _dbContext.ExecuteStoredProcedure<usp_CustomerTrackDetails_Result>("usp_CustomerTrackDetails", sqlParameters).ToList();

                if (customerTrackDetails != null)
                {

                    if (customerTrackDetails.Count > 0)
                    {
                        customerShipmentTrackDto.ShipmentId = customerTrackDetails[0].ShipmentId;
                        customerShipmentTrackDto.ShipmentRefNo = customerTrackDetails[0].ShipmentRefNo;
                        customerShipmentTrackDto.AirWayBill = customerTrackDetails[0].AirWayBill;
                        customerShipmentTrackDto.CustomerPO = customerTrackDetails[0].CustomerPO;
                        customerShipmentTrackDto.OrderNo = customerTrackDetails[0].OrderNo;
                        customerShipmentTrackDto.CustomerRef = customerTrackDetails[0].CustomerRef;
                        customerShipmentTrackDto.ContainerNo = customerTrackDetails[0].ContainerNo;
                        customerShipmentTrackDto.PurchaseDoc = customerTrackDetails[0].PurchaseDoc;
                        customerShipmentTrackDto.PickUpLocation = customerTrackDetails[0].PickUpLocation;
                        customerShipmentTrackDto.PickUpArrivalDate = (customerTrackDetails[0].PickUpArrivalDate == null ? customerTrackDetails[0].PickUpArrivalDate : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(customerTrackDetails[0].PickUpArrivalDate)));
                        customerShipmentTrackDto.DeliveryAddress = customerTrackDetails[customerTrackDetails.Count() - 1].DeliveryAddress;
                        DateTime? deliveryDate = customerTrackDetails[customerTrackDetails.Count() - 1].DeliveryArrive;
                        customerShipmentTrackDto.DeliveryArrive = (deliveryDate == null ? deliveryDate : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(deliveryDate)));
                    }
                }
                if (customerShipmentTrackDto != null)
                {
                    var ShipmentStatusHistory = (from shipmenthistory in costumerContext.tblShipmentEventHistories
                                                 join status in costumerContext.tblShipmentStatus on shipmenthistory.StatusId equals status.StatusId
                                                 where shipmenthistory.ShipmentId == shipmentId
                                                 select new ShipmentStatusHistoryDTO
                                                 {
                                                     ShipmentStatusHistoryId = shipmenthistory.ID,
                                                     StatusId = shipmenthistory.StatusId,
                                                     DateTime = shipmenthistory.EventDateTime,
                                                     Status = status.StatusName ?? string.Empty,
                                                    // SubStatusId = shipmenthistory.SubStatusId,
                                                    // Reason = shipmenthistory.Reason,
                                                     //SubStatus = costumerContext.tblShipmentSubStatus.Where(x => x.SubStatusId == shipmenthistory.SubStatusId).Select(x => x.SubStatusName).FirstOrDefault() ?? string.Empty,

                                                     ImageUrl = (LarastruckingApp.Entities.Common.Configurations.ImageURL + status.ImageURL)
                                                 }
                                                              ).OrderBy(x => x.ShipmentStatusHistoryId).ToList();
                    var ShipmentStatusList = (from status in costumerContext.tblShipmentStatus
                                              where status.StatusId != 3 && status.StatusId != 4 && status.StatusId != 8 && status.StatusId != 9 && status.StatusId != 10 && status.StatusId != 11 && status.StatusId != 13
                                              select new ShipmentStatusDTO
                                              {
                                                  DisplayOrder = status.DisplayOrder,
                                                  StatusId = status.StatusId,

                                                  GrayImageURL = (LarastruckingApp.Entities.Common.Configurations.ImageURL + status.GrayImageURL),
                                              }
                                        ).OrderBy(x => x.DisplayOrder).ToList();
              
                    if (ShipmentStatusHistory.Count > 0)
                    {
                        foreach (var shipmentTrack in ShipmentStatusHistory)
                        {
                            var shipStatusHistory = ShipmentStatusList.Where(x => x.StatusId == shipmentTrack.StatusId).FirstOrDefault();
                            if (shipStatusHistory != null)
                            {
                                ShipmentStatusList.Remove(shipStatusHistory);
                            }

                            shipmentTrack.DateTime = (shipmentTrack.DateTime == null ? shipmentTrack.DateTime : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(shipmentTrack.DateTime)));
                        }
                    }
                    customerShipmentTrackDto.StatusGrayDotPath = LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingResource.StatusDotsGrayImage;
                    customerShipmentTrackDto.StatusDotPath = LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingResource.StatusDotsImage;
                    // customerShipmentTrackDto.CustomerStatusTrack = data;
                    customerShipmentTrackDto.ShipmentStatusHistory = ShipmentStatusHistory;
                    customerShipmentTrackDto.ShipmentStatusList = ShipmentStatusList;

                }

                return customerShipmentTrackDto;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Driver status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            try
            {

                var statuslist = costumerContext.tblShipmentStatus.Where(x => x.IsActive && x.IsDeleted == false && x.IsShipment == true).OrderBy(x => x.DisplayOrderCustomer);
                return AutoMapperServices<tblShipmentStatu, ShipmentStatusDTO>.ReturnObjectList(statuslist.ToList());
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
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 4),
                        new SqlParameter("@ShippingRoutesId", ShippingRoutesId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<ShipmentDamagedEditBindDto>("usp_CustomerModule", sqlParameters);
                result = result.Count > 0 ? result : new List<ShipmentDamagedEditBindDto>();
                result = result.Select(x => new ShipmentDamagedEditBindDto()
                {
                    DamagedID = x.DamagedID,
                    ShipmentRouteID = x.ShipmentRouteID,
                    DamagedImage = x.DamagedImage,
                    DamagedDescription = x.DamagedDescription,
                    ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + x.ImageUrl),
                    DamagedDate = Configurations.ConvertDateTime(x.DamagedDate),
                    IsApproved = x.IsApproved
                }).ToList();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {


                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 5),
                        new SqlParameter("@ShippingRoutesId", ShippingRoutesId),
                        new SqlParameter("@ShipmentFreightDetailId", ShipmentFreightDetailId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<ShipmentProofOfTempEditBind>("usp_CustomerModule", sqlParameters);
                result = result.Select(x => new ShipmentProofOfTempEditBind()
                {
                    ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + x.ImageUrl),
                    proofImageId = x.proofImageId,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ShipmentRouteID = x.ShipmentRouteID,
                    ShipmentFreightDetailId = x.ShipmentFreightDetailId,
                    proofActualTemp = x.proofActualTemp,
                    ProofDate = Configurations.ConvertDateTime(x.ProofDate),
                    ProofDescription = x.ProofDescription,
                    ProofImage = x.ProofImage,
                    IsApproved = x.IsApproved
                }).ToList();
                result = result.Count > 0 ? result : new List<ShipmentProofOfTempEditBind>();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 6),
                        new SqlParameter("@ShippingRoutesId", ShippingRoutesId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<ShipmentFreightDetailsDto>("usp_CustomerModule", sqlParameters);
                result = result.Count > 0 ? result : new List<ShipmentFreightDetailsDto>();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                var customerAccessorialChargesList = (from access in costumerContext.tblShipmentAccessorialPrices
                                                      join feetype in costumerContext.tblAccessorialFeesTypes on access.AccessorialFeeTypeId equals feetype.Id
                                                      where access.ShipmentId == shipmentId && access.ShipmentRouteStopeId == routeId
                                                      group access by access.AccessorialFeeTypeId into g
                                                      select new CustomerAccessorialCharges
                                                      {
                                                          FeeType = costumerContext.tblAccessorialFeesTypes.Where(x => x.Id == g.Key).Select(x => x.Name).FirstOrDefault(),
                                                          Amount = (from fee in g select fee.Amount).ToList().Sum()
                                                      }).ToList();
                return customerAccessorialChargesList;

            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion

        #region Fumigation Get Customer Accessorial Charge
        /// <summary>
        /// Get Customer Accessorial Charge
        /// </summary>
        /// <param name="hipmentId"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public List<CustomerAccessorialCharges> GetCustFumAccessorialCharge(int fumigationId, int routeId)
        {
            try
            {
                var customerAccessorialChargesList = (from access in costumerContext.tblFumigationAccessorialPrices
                                                      join feetype in costumerContext.tblAccessorialFeesTypes on access.AccessorialFeeTypeId equals feetype.Id
                                                      where access.FumigationId == fumigationId && access.FumigationRoutesId == routeId
                                                      group access by access.AccessorialFeeTypeId into g
                                                      select new CustomerAccessorialCharges
                                                      {
                                                          FeeType = costumerContext.tblAccessorialFeesTypes.Where(x => x.Id == g.Key).Select(x => x.Name).FirstOrDefault(),
                                                          Amount = (from fee in g select fee.Amount).ToList().Sum()
                                                      }).ToList();
                return customerAccessorialChargesList;

            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion

        #region Get Freight Details By ID
        /// <summary>
        /// Get Freight Details By ID
        /// </summary>
        /// <returns></returns>

        public ShipmentFreightDetailsDto GetFreightDetailsById(int ShippingRoutesId)
        {
            try
            {

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 6),
                        new SqlParameter("@ShippingRoutesId", ShippingRoutesId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<ShipmentFreightDetailsDto>("usp_CustomerModule", sqlParameters);
                return result != null && result.Count > 0 ? result[0] : null;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                var objShipmentFreightDetail = _dbContext.tblShipmentFreightDetails.Where(x => x.ShipmentBaseFreightDetailId == dto.ShipmentBaseFreightDetailId).FirstOrDefault();
                if (objShipmentFreightDetail != null)
                {
                    objShipmentFreightDetail.Commodity = dto.Commodity;
                    objShipmentFreightDetail.FreightTypeId = dto.FreightTypeId;
                    objShipmentFreightDetail.PricingMethodId = dto.PricingMethodId;
                    objShipmentFreightDetail.QuantityNweight = dto.QuantityNweight;

                }
                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


        #endregion

        #region Customer dashboard = > Fumigation details

        #region Get Multiple Routes by Fumigation and Costomer
        /// <summary>
        ///  Get Multiple Routes by Shipement and Costomer
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public List<CustomerFumigationRoutesDto> GetCustomerFumigationRoutes(int FumigationId)
        {
            try
            {

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 1),
                        new SqlParameter("@FumigationId", FumigationId)

                    };

                var result = _dbContext.ExecuteStoredProcedure<CustomerFumigationRoutesDto>("usp_FumigationCustomerModule", sqlParameters);
                if (result != null && result.Count > 0)
                {
                    foreach (var data in result)
                    {
                        data.PickupDateTime = Configurations.ConvertUTCtoLocalTime(data.PickupDateTime);
                        data.FumigationDateTime = Configurations.ConvertUTCtoLocalTime(data.FumigationDateTime);
                        data.DeliveryDateTime = Configurations.ConvertUTCtoLocalTime(data.DeliveryDateTime);
                    }
                }
                result = result != null && result.Count > 0 ? result : new List<CustomerFumigationRoutesDto>();
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Fumigation Location Details
        /// <summary>
        ///  Get Fumigation Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="FumigationRoutsId"></param>
        /// <returns></returns>
        public CustomerFumigationLocationDetailsDto GetCustomerFumigationRoutesDetails(int FumigationRoutsId)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 2),
                        new SqlParameter("@FumigationRoutsId", FumigationRoutsId)
                    };

                var result = _dbContext.ExecuteStoredProcedure<CustomerFumigationLocationDetailsDto>("usp_FumigationCustomerModule", sqlParameters);
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

        #region Fumigation Track Records 
        /// <summary>
        /// Fumigation Track Records 
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        public CustomerFumigationTrackDto GetCustomerFumigationTrack(int fumigationRouteId, int FumigationId)
        {
            try
            {
                CustomerFumigationTrackDto customerFumigationTrackDto = new CustomerFumigationTrackDto();
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@fumigationRouteId", fumigationRouteId),
                        new SqlParameter("@FumigationId", FumigationId)

                    };
                _dbContext = new ExecuteSQLStoredProceduce();
                var customerTrackDetails = _dbContext.ExecuteStoredProcedure<usp_CustomerFumigationTrackDetails_Result>("usp_CustomerFumigationTrackDetails", sqlParameters).ToList();

                if (customerTrackDetails != null)
                {

                    if (customerTrackDetails.Count > 0)
                    {
                        customerFumigationTrackDto.FumigationId = customerTrackDetails[0].FumigationId;
                        customerFumigationTrackDto.ShipmentRefNo = customerTrackDetails[0].ShipmentRefNo;
                        customerFumigationTrackDto.AirWayBill = customerTrackDetails[0].AirWayBill;
                        customerFumigationTrackDto.CustomerPO = customerTrackDetails[0].CustomerPO;
                        customerFumigationTrackDto.ContainerNo = customerTrackDetails[0].ContainerNo;
                        customerFumigationTrackDto.PickUpLocation = customerTrackDetails[0].PickUpLocation;
                        customerFumigationTrackDto.PickUpArrivalDate = (customerTrackDetails[0].PickUpArrivalDate == null ? customerTrackDetails[0].PickUpArrivalDate : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(customerTrackDetails[0].PickUpArrivalDate)));
                        customerFumigationTrackDto.FumigationAddress = customerTrackDetails[0].FumigationAddress;
                        customerFumigationTrackDto.FumigationDateTime = (customerTrackDetails[0].FumigationDateTime == null ? customerTrackDetails[0].FumigationDateTime : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(customerTrackDetails[0].FumigationDateTime)));
                        customerFumigationTrackDto.FumigationDepartureDateTime = (customerTrackDetails[0].FumigationDepartureDateTime == null ? customerTrackDetails[0].FumigationDepartureDateTime : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(customerTrackDetails[0].FumigationDepartureDateTime)));
                        customerFumigationTrackDto.DeliveryAddress = customerTrackDetails[0].DeliveryAddress;
                        customerFumigationTrackDto.DeliveryArrive = (customerTrackDetails[0].DeliveryArrive == null ? customerTrackDetails[0].DeliveryArrive : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(customerTrackDetails[0].DeliveryArrive)));

                    }
                }
                if (customerFumigationTrackDto != null)
                {
                 var   FumigationStatusHistory = (from fumigationHistory in costumerContext.tblFumigationEventHistories
                                               join status in costumerContext.tblShipmentStatus on fumigationHistory.StatusId equals status.StatusId
                                               where fumigationHistory.FumigationId == FumigationId 
                                               //group fumigationHistory by fumigationHistory.StatusId into g
                                               select new FumigationStatusHistoryDTO
                                               {
                                                   FumigationHistoryId = fumigationHistory.ID,
                                                   StatusId = fumigationHistory.StatusId,
                                                   DateTime = fumigationHistory.EventDateTime,
                                                   Status = status.StatusName ?? string.Empty,
                                                   //SubStatusId = fumigationHistory.SubStatusId,
                                                   //SubStatus = costumerContext.tblShipmentSubStatus.Where(x => x.SubStatusId == fumigationHistory.SubStatusId).Select(x => x.SubStatusName).FirstOrDefault() ?? string.Empty,
                                                   Colour = status.Colour,
                                                   ImageUrl = (LarastruckingApp.Entities.Common.Configurations.ImageURL + status.ImageURL),
                                                   //Reason = fumigationHistory.Reason,
                                               }
                                                             ).OrderBy(x => x.FumigationHistoryId).ToList();
                var    ShipmentStatusList = (from status in costumerContext.tblShipmentStatus
                                         where  status.StatusId != 3 && status.StatusId != 4 && status.StatusId != 8 && status.StatusId != 10 && status.StatusId != 11 && status.StatusId!=13
                                          select new ShipmentStatusDTO
                                          {
                                              FumigationDisplayOrder = status.FumigationDisplayOrder,
                                              StatusId = status.StatusId,
                                              GrayImageURL = (LarastruckingApp.Entities.Common.Configurations.ImageURL + status.GrayImageURL),
                                          }
                                       ).OrderBy(x => x.FumigationDisplayOrder).ToList();

                    var data = (from TSSH in costumerContext.tblFumigationStatusHistories
                                join TSS in costumerContext.tblShipmentStatus on TSSH.StatusId equals TSS.StatusId
                                where TSSH.FumigationId == FumigationId
                                select new CustomerStatusTrackDto
                                {
                                    StatusId = TSSH.StatusId,
                                    StatusName = TSS.StatusName,
                                    Reason = TSSH.Reason ?? string.Empty,
                                    CreatedOn = TSSH.CreatedOn
                                }
                                ).OrderByDescending(x => x.CreatedOn).Distinct().ToList();

                    if (data.Count > 0)
                    {
                        foreach (var fumigationTrack in FumigationStatusHistory)
                        {
                            fumigationTrack.DateTime = fumigationTrack.DateTime == null ? fumigationTrack.DateTime : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(fumigationTrack.DateTime));
                            var shipStatusHistory = ShipmentStatusList.Where(x => x.StatusId == fumigationTrack.StatusId).FirstOrDefault();
                            if (shipStatusHistory != null)
                            {
                                ShipmentStatusList.Remove(shipStatusHistory);
                            }
                        }
                    }
                    customerFumigationTrackDto.CustomerStatusTrack = data;
                    customerFumigationTrackDto.FumigationStatusHistory = FumigationStatusHistory;
                    customerFumigationTrackDto.ShipmentStatusList = ShipmentStatusList;
                    customerFumigationTrackDto.StatusGrayDotPath = LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingResource.StatusDotsGrayImage;
                    customerFumigationTrackDto.StatusDotPath = LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingResource.StatusDotsImage;
                }

                return customerFumigationTrackDto;
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

                var result = new List<FumigationDamagedEditBindDto>();
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 3),
                        new SqlParameter("@FumigationRoutsId", FumigationRoutsId)
                    };

                result = _dbContext.ExecuteStoredProcedure<FumigationDamagedEditBindDto>("usp_FumigationCustomerModule", sqlParameters);
                result = result.Select(x => new FumigationDamagedEditBindDto()
                {
                    DamagedID = x.DamagedID,
                    FumigationRouteId = x.FumigationRouteId,
                    DamagedImage = x.DamagedImage,
                    DamagedDescription = x.DamagedDescription,
                    ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + x.ImageUrl),
                    DamagedDate = Configurations.ConvertDateTime(x.DamagedDate),
                    IsApproved = x.IsApproved
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
                        new SqlParameter("@SpType", 4),
                        new SqlParameter("@FumigationRoutsId", FumigationRoutsId),

                    };

                var result = _dbContext.ExecuteStoredProcedure<FumigationProofOfTempEditBind>("usp_FumigationCustomerModule", sqlParameters);


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
                    IsApproved = x.IsApproved

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

        #region fumigation status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetFumigationStatusList()
        {
            try
            {
                var statuslist = costumerContext.tblShipmentStatus.Where(x => x.IsActive == true && x.IsDeleted == false && x.IsFumigation == true).OrderBy(x => x.FumigationDisplayOrderCustomer);
                return AutoMapperServices<tblShipmentStatu, ShipmentStatusDTO>.ReturnObjectList(statuslist.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #endregion

        #region Customer dashboard => Old Shipment Details 
        #region Get All Quotes Assigned to Customer
        /// <summary>
        /// Get All Quotes Assigned to Customer
        /// </summary>
        /// <returns></returns>
        public List<CustomerQuotesInfoDto> GetOldShipmentDetails(DataTableFilterDto dto, int userId)
        {
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
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@SearchTerm", dto.SearchTerm),
                        new SqlParameter("@SortColumn", dto.SortColumn),
                        new SqlParameter("@SortOrder", dto.SortOrder),
                        new SqlParameter("@PageNumber", dto.PageNumber),
                        new SqlParameter("@PageSize", dto.PageSize),
                        totalCount
                    };

                var result = _dbContext.ExecuteStoredProcedure<CustomerQuotesInfoDto>("usp_CustomerOldShipmentDashoard", sqlParameters);
                dto.TotalCount = Convert.ToInt32(totalCount.Value);
                return result != null && result.Count > 0 ? result : new List<CustomerQuotesInfoDto>();
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
