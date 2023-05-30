using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using LarastruckingApp.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class FumigationRepository : IFumigationRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities fumigationContext;
        private readonly ExecuteSQLStoredProceduce sp_dbContext = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Defining contrructor
        /// </summary>
        public FumigationRepository()
        {
            fumigationContext = new LarastruckingDBEntities();
            sp_dbContext = new ExecuteSQLStoredProceduce();
        }
        #endregion

        #region GetFumigationTypeList
        /// <summary>
        /// Get Fumigation Type  List
        /// </summary>
        /// <returns></returns>
        public List<FumigationTypeDTO> GetFumigationTypeList()
        {
            try
            {
                var fumigationList = fumigationContext.tblFumigationTypes;
                return AutoMapperServices<tblFumigationType, FumigationTypeDTO>.ReturnObjectList(fumigationList.ToList());
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
        public List<ShipmentStatusDTO> GetStatusList()
        {
            try
            {

                var statuslist = fumigationContext.tblShipmentStatus.Where(x => x.IsActive && x.IsDeleted == false && x.IsFumigation == true).OrderBy(x => x.FumigationDisplayOrder);
                return AutoMapperServices<tblShipmentStatu, ShipmentStatusDTO>.ReturnObjectList(statuslist.ToList());

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Approve Proof Of Temprature
        /// <summary>
        /// Approve Proof Of Temprature
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ApprovedProofOFTemp(ProofOfTemperatureDTO entity)
        {
            try
            {

                if (entity.ProofOfTempratureId > 0)
                {
                    var proofOfTemp = fumigationContext.tblFumigationProofOfTemperatureImages.Where(x => x.ImageId == entity.ProofOfTempratureId).FirstOrDefault();
                    if (proofOfTemp != null)
                    {
                        proofOfTemp.IsApproved = true;
                        proofOfTemp.ApprovedBy = entity.ApprovedBy;
                        proofOfTemp.ApprovedOn = entity.ApprovedOn;
                    }
                    fumigationContext.Entry(proofOfTemp).State = EntityState.Modified;
                }
                return fumigationContext.SaveChanges() > 0;

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
        public bool ApprovedDamageImage(FumigationDamageImages entity)
        {
            try
            {

                if (entity.DamageId > 0)
                {
                    var damageDocument = fumigationContext.tblFumigationDamagedImages.Where(x => x.DamagedID == entity.DamageId).FirstOrDefault();
                    if (damageDocument != null)
                    {
                        damageDocument.IsApproved = true;
                        damageDocument.ApprovedBy = entity.ApprovedBy;
                        damageDocument.ApprovedOn = entity.ApprovedOn;
                    }
                    fumigationContext.Entry(damageDocument).State = EntityState.Modified;
                }
                return fumigationContext.SaveChanges() > 0;

            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region create fumigation
        /// <summary>
        ///  Create Fumigation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public FumigationDTO CreateFumigation(FumigationDTO entity)
        {

            FumigationDTO objFumigationDTO = new FumigationDTO();
            try
            {
                var fumigationRoutes = entity.FumigationRouteDetail == null ? null : SqlUtil.ToDataTable(entity.FumigationRouteDetail);
                var accessorialPrice = entity.AccessorialPrice == null ? null : SqlUtil.ToDataTable(entity.AccessorialPrice);
                var fumigationEquipmentNdriver = entity.FumigationEquipmentNdriver == null ? null : SqlUtil.ToDataTable(entity.FumigationEquipmentNdriver);
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SPType", SpType.Insert),
                        new SqlParameter("@CustomerId", entity.CustomerId),
                        new SqlParameter("@StatusId", entity.StatusId),
                        new SqlParameter("@SubStatusId", entity.SubStatusId),
                        new SqlParameter("@Reason",entity.Reason),
                        new SqlParameter("@RequestedBy", entity.RequestedBy),
                        //new SqlParameter("@VendorNconsignee",entity.VendorNconsignee),
                        new SqlParameter("@ShipmentRefNo",entity.ShipmentRefNo),
                        new SqlParameter("@Comments",entity.Comments),
                        new SqlParameter("@CreatedBy", entity.CreatedBy),
                        new SqlParameter("@FumigationRoutes",fumigationRoutes),
                        new SqlParameter("@AccessorialPrice",accessorialPrice),
                        new SqlParameter("@FumigationEquipmentNdriver",fumigationEquipmentNdriver),
                        new SqlParameter("@FumigationComment",entity.FumigationComment)
                     };

                var result = sp_dbContext.ExecuteStoredProcedure<SpResponseDTO>("usp_CreateFumigation", sqlParameters);

                var response = result != null && result.Count > 0 ? result[0] : new SpResponseDTO();

                objFumigationDTO.IsSuccess = (response.ResponseText == Configurations.Insert);


            }
            catch (Exception)
            {
                throw;
            }
            return objFumigationDTO;

        }
        #endregion

        #region Edit Fumigation
        /// <summary>
        ///  Edit Fumigation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public FumigationDTO EditFumigation(FumigationDTO entity)
        {
            try
            {

                var fumigation = fumigationContext.tblFumigations.Where(x => x.FumigationId == entity.FumigationId).FirstOrDefault();
                if (fumigation != null)
                {

                    if (fumigation.StatusId != entity.StatusId && entity.StatusId != 11)
                    {
                        entity.IsMailNeedToSend = true;
                    }
                    fumigation.StatusId = entity.StatusId;
                    fumigation.SubStatusId = entity.SubStatusId;
                    fumigation.Reason = entity.Reason;
                    fumigation.RequestedBy = entity.RequestedBy;
                    fumigation.VendorNconsignee = entity.VendorNconsignee;
                    fumigation.ShipmentRefNo = entity.ShipmentRefNo;
                    fumigation.Comments = entity.Comments;
                    fumigation.ModifiedBy = entity.ModifiedBy;
                    fumigation.ModifiedOn = DateTime.UtcNow;
                    fumigationContext.Entry(fumigation).State = EntityState.Modified;

                    var fumigationHistory = fumigationContext.tblFumigationStatusHistories.Where(x => x.FumigationId == entity.FumigationId).OrderByDescending(x => x.FumigationStatusHistoryId).FirstOrDefault();

                    // x.StatusId == entity.StatusId && x.SubStatusId == entity.SubStatusId).ToList();
                    if (fumigationHistory != null && fumigationHistory.StatusId != entity.StatusId)
                    {
                        tblFumigationStatusHistory objFumigationStatusHistories = new tblFumigationStatusHistory();
                        objFumigationStatusHistories.FumigationId = entity.FumigationId;
                        objFumigationStatusHistories.StatusId = entity.StatusId;
                        objFumigationStatusHistories.SubStatusId = entity.SubStatusId;
                        objFumigationStatusHistories.Reason = entity.Reason;
                        objFumigationStatusHistories.CreatedBy = Convert.ToInt32(entity.ModifiedBy);
                        objFumigationStatusHistories.CreatedOn = DateTime.UtcNow;
                        fumigationContext.tblFumigationStatusHistories.Add(objFumigationStatusHistories);

                        tblFumigationEventHistory fumigationEventHistory = new tblFumigationEventHistory();
                        fumigationEventHistory.FumigationId = entity.FumigationId;
                        fumigationEventHistory.StatusId = entity.StatusId;
                        fumigationEventHistory.UserId = entity.ModifiedBy;
                        fumigationEventHistory.Event = "STATUS";
                        fumigationEventHistory.EventDateTime = DateTime.UtcNow; ;
                        fumigationContext.tblFumigationEventHistories.Add(fumigationEventHistory);
                    }

                    foreach (var fumigationRoute in entity.FumigationRouteDetail)
                    {
                        var fumigationRouteData = fumigationContext.tblFumigationRouts.Where(x => x.FumigationRoutsId == fumigationRoute.FumigationRoutsId).FirstOrDefault();
                        if (fumigationRouteData != null)
                        {
                            if (fumigationRoute.IsDeleted)
                            {
                                fumigationRouteData.IsDeleted = true;
                                fumigationRouteData.DeletedBy = entity.ModifiedBy;
                                fumigationRouteData.DeletedOn = entity.DeletedOn;
                                fumigationContext.Entry(fumigationRouteData).State = EntityState.Modified;
                            }
                            else
                            {
                                fumigationRouteData.FumigationTypeId = fumigationRoute.FumigationTypeId;
                                fumigationRouteData.VendorNConsignee = fumigationRoute.VendorNConsignee;
                                fumigationRouteData.AirWayBill = fumigationRoute.AirWayBill;
                                fumigationRouteData.CustomerPO = fumigationRoute.CustomerPO;
                                fumigationRouteData.ContainerNo = fumigationRoute.ContainerNo;
                                fumigationRouteData.PickUpLocation = fumigationRoute.PickUpLocation;
                                fumigationRouteData.PickUpArrival = fumigationRoute.PickUpArrival == null ? fumigationRoute.PickUpArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.PickUpArrival));
                                fumigationRouteData.FumigationSite = fumigationRoute.FumigationSite;
                                fumigationRouteData.FumigationArrival = fumigationRoute.FumigationArrival == null ? fumigationRoute.FumigationArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.FumigationArrival));
                                fumigationRouteData.ReleaseDate = fumigationRoute.ReleaseDate == null ? fumigationRoute.ReleaseDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.ReleaseDate));
                                fumigationRouteData.DepartureDate = fumigationRoute.DepartureDate == null ? fumigationRoute.DepartureDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.DepartureDate));
                                fumigationRouteData.DeliveryLocation = fumigationRoute.DeliveryLocation;
                                fumigationRouteData.DeliveryArrival = fumigationRoute.DeliveryArrival == null ? fumigationRoute.DeliveryArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.DeliveryArrival));
                                fumigationRouteData.Commodity = fumigationRoute.Commodity;
                                fumigationRouteData.PalletCount = fumigationRoute.PalletCount;
                                fumigationRouteData.BoxType = fumigationRoute.BoxType;
                                fumigationRouteData.BoxCount = fumigationRoute.BoxCount;
                                fumigationRouteData.Temperature = fumigationRoute.Temperature;
                                fumigationRouteData.TemperatureType = fumigationRoute.TemperatureType;
                                fumigationRouteData.TrailerDays = fumigationRoute.TrailerDays;
                                fumigationRouteData.PricingMethod = fumigationRoute.PricingMethod;
                                fumigationRouteData.MinFee = fumigationRoute.MinFee;
                                fumigationRouteData.AddFee = fumigationRoute.AddFee;
                                fumigationRouteData.UpTo = fumigationRoute.UpTo;
                                fumigationRouteData.TrailerPosition = fumigationRoute.TrailerPosition;
                                fumigationRouteData.TotalFee = fumigationRoute.TotalFee;
                                fumigationRouteData.DriverLoadingStartTime = fumigationRoute.DriverLoadingStartTime == null ? fumigationRoute.DriverLoadingStartTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.DriverLoadingStartTime));
                                fumigationRouteData.DriverLoadingFinishTime = fumigationRoute.DriverLoadingFinishTime == null ? fumigationRoute.DriverLoadingFinishTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.DriverLoadingFinishTime));
                                fumigationRouteData.DriverFumigationIn = fumigationRoute.DriverFumigationIn == null ? fumigationRoute.DriverFumigationIn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.DriverFumigationIn));
                                fumigationRouteData.DriverFumigationRelease = fumigationRoute.DriverFumigationRelease == null ? fumigationRoute.DriverFumigationRelease : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.DriverFumigationRelease));
                                fumigationRouteData.DriverPickupArrival = fumigationRoute.DriverPickupArrival == null ? fumigationRoute.DriverPickupArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.DriverPickupArrival));
                                fumigationRouteData.DriverPickupDeparture = fumigationRoute.DriverPickupDeparture == null ? fumigationRoute.DriverPickupDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.DriverPickupDeparture));
                                fumigationRouteData.DriverDeliveryArrival = fumigationRoute.DriverDeliveryArrival == null ? fumigationRoute.DriverDeliveryArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.DriverDeliveryArrival));
                                fumigationRouteData.DriverDeliveryDeparture = fumigationRoute.DriverDeliveryDeparture == null ? fumigationRoute.DriverDeliveryDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(fumigationRoute.DriverDeliveryDeparture));
                                fumigationContext.Entry(fumigationRouteData).State = EntityState.Modified;
                            }
                        }
                    }

                    if (entity.AccessorialPrice != null)
                    {
                        foreach (var accPrice in entity.AccessorialPrice)
                        {
                            if (accPrice.FumigationAccessorialPriceId > 0)
                            {
                                if (accPrice.IsDeleted)
                                {
                                    var accessorialPrice = fumigationContext.tblFumigationAccessorialPrices.Where(x => x.FumigationAccessorialPriceId == accPrice.FumigationAccessorialPriceId && x.IsDeleted == false).FirstOrDefault();
                                    if (accessorialPrice != null)
                                    {


                                        accessorialPrice.IsDeleted = true;
                                        accessorialPrice.DeletedBy = entity.ModifiedBy;
                                        accessorialPrice.DeletedOn = DateTime.UtcNow;
                                        fumigationContext.Entry(accessorialPrice).State = EntityState.Modified;
                                    }
                                }
                                else
                                {
                                    var accessorialPrice = fumigationContext.tblFumigationAccessorialPrices.Where(x => x.FumigationAccessorialPriceId == accPrice.FumigationAccessorialPriceId && x.IsDeleted == false).FirstOrDefault();
                                    if (accessorialPrice != null)
                                    {
                                        accessorialPrice.Unit = accPrice.Unit;
                                        accessorialPrice.AmtPerUnit = accPrice.AmtPerUnit;
                                        accessorialPrice.Amount = accPrice.Amount;
                                        accessorialPrice.Reason = accPrice.Reason;
                                        fumigationContext.Entry(accessorialPrice).State = EntityState.Modified;
                                    }
                                }

                            }
                            else
                            {
                                if (accPrice.IsDeleted == false)
                                {
                                    var routeStops = fumigationContext.tblFumigationRouts.Where(x => x.FumigationId == entity.FumigationId && x.RouteNo == accPrice.RouteNo && x.IsDeleted == false).FirstOrDefault();
                                    if (routeStops != null)
                                    {
                                        tblFumigationAccessorialPrice objFumigationAccessorialPrice = new tblFumigationAccessorialPrice();
                                        objFumigationAccessorialPrice.FumigationId = entity.FumigationId;
                                        objFumigationAccessorialPrice.FumigationRoutesId = routeStops.FumigationRoutsId;
                                        objFumigationAccessorialPrice.AccessorialFeeTypeId = accPrice.AccessorialFeeTypeId;
                                        objFumigationAccessorialPrice.Unit = accPrice.Unit;
                                        objFumigationAccessorialPrice.AmtPerUnit = accPrice.AmtPerUnit;
                                        objFumigationAccessorialPrice.Amount = accPrice.Amount;
                                        objFumigationAccessorialPrice.Reason = accPrice.Reason;
                                        fumigationContext.tblFumigationAccessorialPrices.Add(objFumigationAccessorialPrice);
                                    }
                                }
                            }
                        }

                    }

                    if (entity.FumigationEquipmentNdriver != null)
                    {
                        fumigationContext.tblFumigationEquipmentNDrivers.RemoveRange(fumigationContext.tblFumigationEquipmentNDrivers.Where(x => x.FumigationId == entity.FumigationId));

                        foreach (var equipmentNDriver in entity.FumigationEquipmentNdriver)
                        {
                            int fumigationRouteId = fumigationContext.tblFumigationRouts.Where(x => x.FumigationId == entity.FumigationId && x.RouteNo == equipmentNDriver.RouteNo && x.IsDeleted == false).Select(x => x.FumigationRoutsId).FirstOrDefault();

                            if (fumigationRouteId > 0)
                            {
                                tblFumigationEquipmentNDriver objEquipmentNDriver = new tblFumigationEquipmentNDriver();
                                objEquipmentNDriver.FumigationId = entity.FumigationId;
                                objEquipmentNDriver.FumigationRoutsId = fumigationRouteId;
                                objEquipmentNDriver.EquipmentId = equipmentNDriver.EquipmentId;
                                objEquipmentNDriver.DriverId = equipmentNDriver.DriverId;
                                objEquipmentNDriver.IsPickUp = equipmentNDriver.IsPickUp;
                                objEquipmentNDriver.IsDeleted = equipmentNDriver.IsDeleted;
                                fumigationContext.tblFumigationEquipmentNDrivers.Add(objEquipmentNDriver);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(entity.FumigationComment))
                    {
                        tblFumigationComment FumigationCommment = new tblFumigationComment();
                        FumigationCommment.FumigationId = entity.FumigationId;
                        FumigationCommment.Comment = entity.FumigationComment;
                        FumigationCommment.CommentBy = "DP";
                        FumigationCommment.CreatedOn = DateTime.UtcNow;
                        FumigationCommment.CreatedBy = entity.ModifiedBy;
                        fumigationContext.tblFumigationComments.Add(FumigationCommment);
                    }

                }
                entity.IsSuccess = fumigationContext.SaveChanges() > 0;

            }
            catch (Exception)
            {
                throw;
            }

            return entity;
        }
        #endregion

        #region get fumigation list
        /// <summary>
        /// get fumigation list
        /// </summary>
        /// <returns></returns>

        public IList<FumigationListDTO> GetFumigationList(DataTableFilterDto entity)
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
                        new SqlParameter("@SearchTerm", entity.SearchTerm),
                        new SqlParameter("@SortColumn", entity.SortColumn),
                        new SqlParameter("@SortOrder", entity.SortOrder),
                        new SqlParameter("@PageNumber", entity.PageNumber),
                        new SqlParameter("@PageSize", entity.PageSize),
                        //new SqlParameter("@FumigationId", entity.SortColumn),
                       // totalCount
                    };

                var result = sp_dbContext.ExecuteStoredProcedure<FumigationListDTO>("usp_GetFumigationList", sqlParameters);
                entity.TotalCount = result.Count > 0 ? Convert.ToInt32(result.Select(x => x.TotalCount).FirstOrDefault()) : 0;
                return result != null && result.Count > 0 ? result : new List<FumigationListDTO>();
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region get other fumigation list
        /// <summary>
        /// get other fumigation list
        /// </summary>
        /// <returns></returns>

        public IList<FumigationOtherList> GetOtherFumigationList(DataTableFilterDto entity)
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
                        new SqlParameter("@SearchTerm", entity.SearchTerm),
                        new SqlParameter("@SortColumn", entity.SortColumn),
                        new SqlParameter("@SortOrder", entity.SortOrder),
                        new SqlParameter("@PageNumber", entity.PageNumber),
                        new SqlParameter("@PageSize", entity.PageSize),
                       // totalCount usp_GetOtherFumList
                    };

                var result = sp_dbContext.ExecuteStoredProcedure<FumigationOtherList>("usp_GetOrderTakenFumigationList", sqlParameters);
                entity.TotalCount = result.Count > 0 ? Convert.ToInt32(result.Select(x => x.TotalCount).FirstOrDefault()) : 0;
                return result != null && result.Count > 0 ? result : new List<FumigationOtherList>();
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region View All Fumigation
        /// <summary>
        /// get fumigation list
        /// </summary>
        /// <returns></returns>

        public IList<FumigationViewAllListDTO> ViewAllFumigation(AllShipmentDTO entity)
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
                       new SqlParameter("@SearchTerm", entity.SearchTerm),
                        new SqlParameter("@SortColumn", entity.SortColumn),
                        new SqlParameter("@SortOrder", entity.SortOrder),
                        new SqlParameter("@PageNumber", entity.PageNumber),
                        new SqlParameter("@PageSize", entity.PageSize),
                        new SqlParameter("@StartDate", entity.StartDate),
                        new SqlParameter("@EndDate", entity.EndDate),
                        new SqlParameter("@CustomerId", entity.CustomerId),
                        new SqlParameter("@StatusId", entity.StatusId),
                            new SqlParameter("@DriverName", entity.DriverName),
                                            };

                var result = sp_dbContext.ExecuteStoredProcedure<FumigationViewAllListDTO>("usp_GetCompletedNcancelFumList", sqlParameters);
                entity.TotalCount = result.Count > 0 ? Convert.ToInt32(result.Select(x => x.TotalCount).FirstOrDefault()) : 0;
                return result != null && result.Count > 0 ? result : new List<FumigationViewAllListDTO>();
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region delete fumigation
        /// <summary>
        ///Delete shipment By Id
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        public bool DeleteFumigation(FumigationDTO entity)
        {
            try
            {


                bool result = false;
                var table = fumigationContext.tblFumigations.Find(entity.FumigationId);
                if (table != null)
                {
                    table.IsDeleted = true;
                    table.DeletedBy = entity.CreatedBy;
                    table.DeletedOn = entity.CreatedOn;
                    fumigationContext.Entry(table).State = System.Data.Entity.EntityState.Modified;
                    result = fumigationContext.SaveChanges() > 0;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region get fumigation by id 
        /// <summary>
        ///  get fumigation by id for edit fumigaion
        /// </summary>
        /// <returns></returns>
        public GetFumigationDTO GetFumigationById(int fumigationId)
        {
            try
            {


                var fumigationdetail = (from fumigation in fumigationContext.tblFumigations
                                        join customer in fumigationContext.tblCustomerRegistrations on fumigation.CustomerId equals customer.CustomerID
                                        join status in fumigationContext.tblShipmentStatus on fumigation.StatusId equals status.StatusId
                                        where fumigation.FumigationId == fumigationId
                                        select new GetFumigationDTO
                                        {
                                            ContactInfoCount = fumigationContext.tblCustomerContacts.Where(x => x.CustomerId == fumigation.CustomerId).Count(),
                                            FumigationId = fumigation.FumigationId,
                                            CustomerId = customer.CustomerID,
                                            CustomerName = customer.CustomerName,
                                            StatusId = fumigation.StatusId,
                                            StatusName = status.StatusName ?? string.Empty,
                                            SubStatusId = fumigation.SubStatusId,
                                            RequestedBy = fumigation.RequestedBy,
                                            Reason = fumigation.Reason ?? string.Empty,
                                            ShipmentRefNo = fumigation.ShipmentRefNo,
                                            VendorNconsignee = fumigation.VendorNconsignee,
                                            Comments = fumigation.Comments ?? string.Empty,
                                            FumigationComment = (from comment in fumigationContext.tblFumigationComments
                                                                 where comment.FumigationId == fumigationId
                                                                 select new FumigationCommentDTO
                                                                 {
                                                                     Comment = (comment.CommentBy + " :- " + comment.Comment)
                                                                 }
                                                            ).ToList(),
                                            GetFumigationRouteDetail = (from fumigationRouteStop in fumigationContext.tblFumigationRouts
                                                                        where fumigationRouteStop.FumigationId == fumigationId && fumigationRouteStop.IsDeleted == false
                                                                        select new GetFumigationRouteDTO
                                                                        {
                                                                            FumigationRoutsId = fumigationRouteStop.FumigationRoutsId,
                                                                            FumigationId = fumigationRouteStop.FumigationId,
                                                                            FumigationTypeId = fumigationRouteStop.FumigationTypeId,
                                                                            RouteNo = fumigationRouteStop.RouteNo,
                                                                            AirWayBill = fumigationRouteStop.AirWayBill ?? string.Empty,
                                                                            CustomerPO = fumigationRouteStop.CustomerPO ?? string.Empty,
                                                                            ContainerNo = fumigationRouteStop.ContainerNo ?? string.Empty,
                                                                            PickUpLocation = fumigationRouteStop.PickUpLocation,
                                                                            PickUpLocationText = (from address in fumigationContext.tblAddresses
                                                                                                  join state in fumigationContext.tblStates on address.State equals state.ID
                                                                                                  where address.AddressId == fumigationRouteStop.PickUpLocation
                                                                                                  select new { PicUpLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName.Trim() + ",")) + address.Address1 + " " + address.City + " " + state.Name + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault() ?? string.Empty,
                                                                            PickUpArrival = fumigationRouteStop.PickUpArrival,

                                                                            DeliveryLocation = fumigationRouteStop.DeliveryLocation,
                                                                            DeliveryLocationText = (from address in fumigationContext.tblAddresses
                                                                                                    join state in fumigationContext.tblStates on address.State equals state.ID
                                                                                                    where address.AddressId == fumigationRouteStop.DeliveryLocation
                                                                                                    select new { PicUpLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName.Trim() + ",")) + address.Address1 + " " + address.City + " " + state.Name + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault() ?? string.Empty,
                                                                            DeliveryArrival = fumigationRouteStop.DeliveryArrival,


                                                                            IsDeleted = fumigationRouteStop.IsDeleted,
                                                                            FumigationArrival = fumigationRouteStop.FumigationArrival,
                                                                            FumigationSite = fumigationRouteStop.FumigationSite,
                                                                            FumigationSiteText = (from address in fumigationContext.tblAddresses
                                                                                                  join state in fumigationContext.tblStates on address.State equals state.ID
                                                                                                  where address.AddressId == fumigationRouteStop.FumigationSite
                                                                                                  select new { PicUpLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + " " + address.City + " " + state.Name + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault() ?? string.Empty,
                                                                            ReleaseDate = fumigationRouteStop.ReleaseDate,
                                                                            DepartureDate = fumigationRouteStop.DepartureDate,
                                                                            Commodity = fumigationRouteStop.Commodity ?? string.Empty,
                                                                            PricingMethod = fumigationRouteStop.PricingMethod ?? 0,
                                                                            PricingMethodText = (from pricingmethod in fumigationContext.tblPricingMethods where pricingmethod.PricingMethodId == fumigationRouteStop.PricingMethod select pricingmethod.PricingMethodName).FirstOrDefault(),
                                                                            TrailerDays = fumigationRouteStop.TrailerDays ?? 0,
                                                                            PalletCount = fumigationRouteStop.PalletCount ?? 0,
                                                                            BoxCount = fumigationRouteStop.BoxCount ?? 0,
                                                                            BoxType = fumigationRouteStop.BoxType ?? 0,
                                                                            Temperature = fumigationRouteStop.Temperature,
                                                                            TemperatureType = fumigationRouteStop.TemperatureType ?? string.Empty,
                                                                            MinFee = fumigationRouteStop.MinFee ?? 0,
                                                                            AddFee = fumigationRouteStop.AddFee ?? 0,
                                                                            UpTo = fumigationRouteStop.UpTo ?? 0,
                                                                            TrailerPosition = fumigationRouteStop.TrailerPosition,
                                                                            TotalFee = fumigationRouteStop.TotalFee ?? 0,
                                                                            ReceiverName = fumigationRouteStop.ReceiverName ?? string.Empty,
                                                                            DigitalSignature = fumigationRouteStop.DigitalSignature ?? string.Empty,

                                                                            DriverPickupArrival = fumigationRouteStop.DriverPickupArrival,
                                                                            DriverLoadingStartTime = fumigationRouteStop.DriverLoadingStartTime,
                                                                            DriverLoadingFinishTime = fumigationRouteStop.DriverLoadingFinishTime,
                                                                            DriverPickupDeparture = fumigationRouteStop.DriverPickupDeparture,

                                                                            DriverFumigationIn = fumigationRouteStop.DriverFumigationIn,
                                                                            DriverFumigationRelease = fumigationRouteStop.DriverFumigationRelease,

                                                                            DriverDeliveryArrival = fumigationRouteStop.DriverDeliveryArrival,
                                                                            DriverDeliveryDeparture = fumigationRouteStop.DriverDeliveryDeparture,
                                                                            VendorNConsignee = fumigationRouteStop.VendorNConsignee,
                                                                        }
                                                                    ).ToList(),


                                            GetFumigationAccessorialPrice = (from accessorialprice in fumigationContext.tblFumigationAccessorialPrices
                                                                             join fumigationRouteStops in fumigationContext.tblFumigationRouts on accessorialprice.FumigationRoutesId equals fumigationRouteStops.FumigationRoutsId
                                                                             where accessorialprice.FumigationId == fumigationId && fumigationRouteStops.IsDeleted == false && accessorialprice.IsDeleted == false
                                                                             select new GetFumigationAccessorialPriceDTO
                                                                             {
                                                                                 Reason = accessorialprice.Reason,
                                                                                 FumigationAccessorialPriceId = accessorialprice.FumigationAccessorialPriceId,
                                                                                 RouteNo = fumigationRouteStops.RouteNo,
                                                                                 AccessorialFeeTypeId = accessorialprice.AccessorialFeeTypeId,
                                                                                 Unit = accessorialprice.Unit,
                                                                                 AmtPerUnit = accessorialprice.AmtPerUnit,
                                                                                 Amount = accessorialprice.Amount,
                                                                                 FeeType = (from aft in fumigationContext.tblAccessorialFeesTypes where aft.Id == accessorialprice.AccessorialFeeTypeId select aft.PricingMethod).FirstOrDefault(),
                                                                             }
                                                                  ).ToList(),

                                            DamageImages = (from damageImage in fumigationContext.tblFumigationDamagedImages
                                                            join fumigationRoute in fumigationContext.tblFumigationRouts on damageImage.FumigationRouteId equals fumigationRoute.FumigationRoutsId
                                                            where fumigationRoute.FumigationId == fumigationId && damageImage.IsDeleted == false
                                                            select new FumigationDamageImages
                                                            {
                                                                FumigationRouteId = fumigationRoute.FumigationRoutsId,
                                                                DamageId = damageImage.DamagedID,
                                                                RouteNo = fumigationRoute.RouteNo,
                                                                ImageName = damageImage.ImageName,
                                                                ImageDescription = damageImage.ImageDescription,
                                                                ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + damageImage.ImageUrl),
                                                                CreatedOn = damageImage.CreatedOn,
                                                                IsApproved = damageImage.IsApproved,
                                                            }
                                                                    ).ToList(),

                                            ProofOfTemprature = (from proofoftemp in fumigationContext.tblFumigationProofOfTemperatureImages
                                                                 join fumigationRoute in fumigationContext.tblFumigationRouts on proofoftemp.FumigationRouteId equals fumigationRoute.FumigationRoutsId
                                                                 where fumigationRoute.FumigationId == fumigationId && proofoftemp.IsDeleted == false
                                                                 select new FumigationProofOfTemprature
                                                                 {
                                                                     ProofOfTempratureId = proofoftemp.ImageId,
                                                                     FumigationRouteId = fumigationRoute.FumigationRoutsId,
                                                                     ImageName = proofoftemp.ImageName,
                                                                     ImageUrl = (LarastruckingApp.Entities.Common.Configurations.BaseURL + proofoftemp.ImageUrl),
                                                                     CreatedOn = proofoftemp.CreatedOn,
                                                                     ActualTemperature = proofoftemp.ActualTemperature,
                                                                     IsApproved = proofoftemp.IsApproved,
                                                                     IsLoading = proofoftemp.IsLoading,
                                                                 }
                                                                   ).ToList(),

                                            FumigationEquipmentNDriver = (from eqpNdriver in fumigationContext.tblFumigationEquipmentNDrivers
                                                                          join fumigationRoute in fumigationContext.tblFumigationRouts on eqpNdriver.FumigationRoutsId equals fumigationRoute.FumigationRoutsId
                                                                          // join driver in fumigationContext.tblDrivers on eqpNdriver.DriverId equals driver.DriverID
                                                                          // join equipmnet in fumigationContext.tblEquipmentDetails on eqpNdriver.EquipmentId equals equipmnet.EDID
                                                                          where eqpNdriver.FumigationId == fumigationId && eqpNdriver.IsDeleted == false
                                                                          select new GetFumigationEquipmentNDriver
                                                                          {

                                                                              RouteNo = fumigationRoute.RouteNo,
                                                                              FumigationRoutsId = eqpNdriver.FumigationRoutsId,
                                                                              FumigationEquipmentNDriverId = eqpNdriver.FumigationEquipmentNDriverId,
                                                                              DriverId = eqpNdriver.DriverId,
                                                                              DriverName = (from driver in fumigationContext.tblDrivers
                                                                                            where eqpNdriver.DriverId == driver.DriverID
                                                                                            select new
                                                                                            {
                                                                                                DriverName =
                                                                                            (driver.FirstName + " " + (driver.LastName ?? string.Empty))
                                                                                            }).Select(x => x.DriverName).FirstOrDefault() ?? string.Empty,
                                                                              EquipmentId = eqpNdriver.EquipmentId,
                                                                              EquipmentName = (from equipment in fumigationContext.tblEquipmentDetails
                                                                                               where eqpNdriver.EquipmentId == equipment.EDID
                                                                                               select equipment.EquipmentNo).FirstOrDefault() ?? string.Empty,
                                                                              IsPickUp = eqpNdriver.IsPickUp,
                                                                              IsDeleted = eqpNdriver.IsDeleted,

                                                                          }
                                                                          ).ToList(),
                                        }

                                    ).FirstOrDefault();
                if (fumigationdetail != null)
                {
                    var fumigationHistory = fumigationContext.tblFumigationStatusHistories.Where(x => x.FumigationId == fumigationdetail.FumigationId).OrderByDescending(x => x.FumigationStatusHistoryId).FirstOrDefault();
                    if (fumigationHistory != null)
                    {
                        if (fumigationHistory.StatusId != fumigationdetail.StatusId)
                        {
                            fumigationdetail.IsOnHold = true;
                        }
                    }
                    foreach (var route in fumigationdetail.GetFumigationRouteDetail)
                    {
                        route.PickUpArrival = (route.PickUpArrival == null ? route.PickUpArrival : Configurations.ConvertDateTime(route.PickUpArrival.Value));
                        route.DeliveryArrival = (route.DeliveryLocation == null ? route.DeliveryArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.DeliveryArrival)));
                        route.FumigationArrival = route.FumigationArrival == null ? route.FumigationArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.FumigationArrival));
                        route.ReleaseDate = route.ReleaseDate == null ? route.ReleaseDate : Configurations.ConvertDateTime(Convert.ToDateTime(route.ReleaseDate));
                        route.DepartureDate = route.DepartureDate == null ? route.DepartureDate : Configurations.ConvertDateTime(Convert.ToDateTime(route.DepartureDate));

                        route.DriverPickupArrival = route.DriverPickupArrival == null ? route.DriverPickupArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.DriverPickupArrival));
                        route.DriverLoadingStartTime = route.DriverLoadingStartTime == null ? route.DriverLoadingStartTime : Configurations.ConvertDateTime(Convert.ToDateTime(route.DriverLoadingStartTime));
                        route.DriverLoadingFinishTime = route.DriverLoadingFinishTime == null ? route.DriverLoadingFinishTime : Configurations.ConvertDateTime(Convert.ToDateTime(route.DriverLoadingFinishTime));
                        route.DriverPickupDeparture = route.DriverPickupDeparture == null ? route.DriverPickupDeparture : Configurations.ConvertDateTime(Convert.ToDateTime(route.DriverPickupDeparture));

                        route.DriverFumigationIn = route.DriverFumigationIn == null ? route.DriverFumigationIn : Configurations.ConvertDateTime(Convert.ToDateTime(route.DriverFumigationIn));
                        route.DriverFumigationRelease = route.DriverFumigationRelease == null ? route.DriverFumigationRelease : Configurations.ConvertDateTime(Convert.ToDateTime(route.DriverFumigationRelease)); ;

                        route.DriverDeliveryArrival = route.DriverDeliveryArrival == null ? route.DriverDeliveryArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.DriverDeliveryArrival));
                        route.DriverDeliveryDeparture = route.DriverDeliveryDeparture == null ? route.DriverDeliveryDeparture : Configurations.ConvertDateTime(Convert.ToDateTime(route.DriverDeliveryDeparture));

                    }
                    foreach (var profoftemp in fumigationdetail.ProofOfTemprature)
                    {
                        profoftemp.CreatedOn = profoftemp.CreatedOn == null ? profoftemp.CreatedOn : Configurations.ConvertDateTime(Convert.ToDateTime(profoftemp.CreatedOn));
                    }
                    foreach (var damage in fumigationdetail.DamageImages)
                    {
                        damage.CreatedOn = damage.CreatedOn == null ? damage.CreatedOn : Configurations.ConvertDateTime(Convert.ToDateTime(damage.CreatedOn));
                    }
                }


                return fumigationdetail;
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
        public int UploadProofofTempDocument(List<FumigationProofOfTemprature> fumigationProofOfTemprature)
        {
            try
            {


                var result = 0;
                if (fumigationProofOfTemprature != null && fumigationProofOfTemprature.Count > 0)
                {
                    foreach (var file in fumigationProofOfTemprature)
                    {
                        if (file.ActualTemperature != null)
                        {
                            tblFumigationProofOfTemperatureImage proofofTempImage = new tblFumigationProofOfTemperatureImage();
                            proofofTempImage.FumigationRouteId = file.FumigationRouteId;
                            proofofTempImage.ActualTemperature = file.ActualTemperature;
                            proofofTempImage.IsLoading = true;
                            proofofTempImage.ImageName = file.ImageName;
                            proofofTempImage.IsApproved = true;
                            proofofTempImage.ImageUrl = file.ImageUrl;
                            proofofTempImage.CreatedBy = file.CreatedBy;
                            proofofTempImage.CreatedOn = file.CreatedOn;
                            proofofTempImage.ApprovedBy = file.CreatedBy;
                            proofofTempImage.ApprovedOn = file.CreatedOn;
                            fumigationContext.tblFumigationProofOfTemperatureImages.Add(proofofTempImage);
                            result = fumigationContext.SaveChanges() > 0 ? proofofTempImage.ImageId : 0;
                        }
                        if (file.DeliveryTemp != null)
                        {
                            tblFumigationProofOfTemperatureImage proofofTempImage = new tblFumigationProofOfTemperatureImage();
                            proofofTempImage.FumigationRouteId = file.FumigationRouteId;
                            proofofTempImage.ActualTemperature = file.DeliveryTemp;
                            proofofTempImage.IsLoading = false;
                            proofofTempImage.ImageName = file.ImageName;
                            proofofTempImage.IsApproved = true;
                            proofofTempImage.ImageUrl = file.ImageUrl;
                            proofofTempImage.CreatedBy = file.CreatedBy;
                            proofofTempImage.CreatedOn = file.CreatedOn;
                            proofofTempImage.ApprovedBy = file.CreatedBy;
                            proofofTempImage.ApprovedOn = file.CreatedOn;
                            fumigationContext.tblFumigationProofOfTemperatureImages.Add(proofofTempImage);
                            result = fumigationContext.SaveChanges() > 0 ? proofofTempImage.ImageId : 0;
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

        #region Upload Damage Document
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        public int UploadDamageDocument(List<FumigationDamageImages> damageImageList)
        {
            try
            {


                var result = 0;
                if (damageImageList != null && damageImageList.Count > 0)
                {

                    foreach (var file in damageImageList)
                    {
                        tblFumigationDamagedImage objDamagedImage = new tblFumigationDamagedImage();
                        objDamagedImage.FumigationRouteId = file.FumigationRouteId;
                        objDamagedImage.ImageName = file.ImageName;
                        objDamagedImage.ImageDescription = file.ImageDescription;
                        objDamagedImage.ImageUrl = file.ImageUrl;
                        objDamagedImage.CreatedBy = file.CreatedBy;
                        objDamagedImage.CreatedOn = file.CreatedOn;
                        objDamagedImage.IsApproved = true;
                        objDamagedImage.ApprovedBy = file.CreatedBy;
                        objDamagedImage.ApprovedOn = file.CreatedOn;
                        fumigationContext.tblFumigationDamagedImages.Add(objDamagedImage);
                        result = fumigationContext.SaveChanges() > 0 ? objDamagedImage.DamagedID : 0;
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

        #region Delete Proof of Temprature
        /// <summary>
        /// Delete proof of temprature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteProofOfTemprature(FumigationProofOfTemprature model)
        {
            try
            {


                bool result = false;
                if (model != null)
                {
                    var proofofTempImage = fumigationContext.tblFumigationProofOfTemperatureImages.Where(x => x.ImageId == model.ProofOfTempratureId).FirstOrDefault();
                    if (proofofTempImage != null)
                    {
                        proofofTempImage.IsDeleted = true;
                        proofofTempImage.DeletedBy = model.CreatedBy;
                        proofofTempImage.DeletedOn = model.CreatedOn;
                        fumigationContext.Entry(proofofTempImage).State = EntityState.Modified;
                        result = fumigationContext.SaveChanges() > 0;
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
        public bool DeleteDamageFile(FumigationDamageImages model)
        {
            try
            {


                bool result = false;
                if (model != null)
                {
                    var damageImage = fumigationContext.tblFumigationDamagedImages.Where(x => x.DamagedID == model.DamageId).FirstOrDefault();
                    if (damageImage != null)
                    {
                        damageImage.IsDeleted = true;
                        damageImage.DeletedBy = model.CreatedBy;
                        damageImage.DeletedOn = model.CreatedOn;
                        fumigationContext.Entry(damageImage).State = EntityState.Modified;
                        result = fumigationContext.SaveChanges() > 0;
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
        public int AddRouteStops(GetFumigationRouteDTO model)
        {
            try
            {


                tblFumigationRout objFumigationRoute = new tblFumigationRout();
                if (model != null && model.FumigationId > 0)
                {

                    objFumigationRoute.RouteNo = model.RouteNo;
                    objFumigationRoute.FumigationId = model.FumigationId;
                    objFumigationRoute.FumigationTypeId = model.FumigationTypeId;
                    objFumigationRoute.AirWayBill = model.AirWayBill;
                    objFumigationRoute.CustomerPO = model.CustomerPO;
                    objFumigationRoute.ContainerNo = model.ContainerNo;
                    objFumigationRoute.PickUpLocation = model.PickUpLocation;
                    if (model.PickUpArrival != null)
                    {
                        objFumigationRoute.PickUpArrival = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.PickUpArrival));
                    }
                    /*if (model.DriverLoadingStartTime != null)
                    {
                        objFumigationRoute.DriverLoadingFinishTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.DriverLoadingStartTime));
                    }
                    if (model.DriverLoadingFinishTime != null)
                    {
                        objFumigationRoute.DriverLoadingFinishTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.DriverLoadingFinishTime));
                    }*/

                    objFumigationRoute.FumigationSite = model.FumigationSite;
                    if (model.FumigationArrival != null)
                    {
                        objFumigationRoute.FumigationArrival = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.FumigationArrival));
                    }
                    if (model.ReleaseDate != null)
                    {
                        objFumigationRoute.ReleaseDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.ReleaseDate));
                    }
                    if (model.DepartureDate != null)
                    {
                        objFumigationRoute.DepartureDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.DepartureDate));
                    }
                    objFumigationRoute.DeliveryLocation = model.DeliveryLocation;
                    if (model.DeliveryArrival != null)
                    {
                        objFumigationRoute.DeliveryArrival = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.DeliveryArrival));
                    }
                    objFumigationRoute.Commodity = model.Commodity;
                    objFumigationRoute.PalletCount = model.PalletCount;
                    objFumigationRoute.BoxCount = model.BoxCount;
                    objFumigationRoute.BoxType = model.BoxType;
                    objFumigationRoute.Temperature = model.Temperature;
                    objFumigationRoute.TemperatureType = model.TemperatureType;
                    objFumigationRoute.TrailerDays = model.TrailerDays;
                    objFumigationRoute.PricingMethod = model.PricingMethod;
                    objFumigationRoute.MinFee = model.MinFee;
                    objFumigationRoute.AddFee = model.AddFee;
                    objFumigationRoute.TrailerPosition = model.TrailerPosition;
                    objFumigationRoute.TotalFee = model.TotalFee;
                    objFumigationRoute.VendorNConsignee = model.VendorNConsignee;
                    fumigationContext.tblFumigationRouts.Add(objFumigationRoute);

                }
                return fumigationContext.SaveChanges() > 0 ? objFumigationRoute.FumigationRoutsId : 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Approved fumigation
        /// <summary>
        /// Approved fumigation if fumigation on hold
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public FumigationDTO ApprovedFumigation(FumigationDTO entity)
        {
            try
            {


                FumigationDTO fumigation = new FumigationDTO();
                if (entity.FumigationId > 0)
                {

                    var fumigationData = fumigationContext.tblFumigations.Where(x => x.FumigationId == entity.FumigationId).FirstOrDefault();
                    if (fumigationData != null)
                    {
                        fumigationData.StatusId = entity.StatusId;
                        fumigationData.SubStatusId = entity.SubStatusId;
                        fumigationData.Reason = entity.Reason;
                        fumigationData.ModifiedBy = entity.CreatedBy;
                        fumigationData.ModifiedOn = entity.CreatedOn;
                        fumigationContext.Entry(fumigationData).State = EntityState.Modified;


                        tblFumigationStatusHistory fumigationStatusHistory = new tblFumigationStatusHistory();
                        fumigationStatusHistory.FumigationId = entity.FumigationId;
                        fumigationStatusHistory.StatusId = Convert.ToInt32(entity.StatusId);
                        fumigationStatusHistory.SubStatusId = entity.SubStatusId;
                        fumigationStatusHistory.Reason = entity.Reason;
                        fumigationStatusHistory.CreatedBy = entity.CreatedBy;
                        fumigationStatusHistory.CreatedOn = entity.CreatedOn;
                        fumigationContext.tblFumigationStatusHistories.Add(fumigationStatusHistory);

                        tblFumigationEventHistory fumigationEventHistory = new tblFumigationEventHistory();
                        fumigationEventHistory.FumigationId = entity.FumigationId;
                        fumigationEventHistory.StatusId = entity.StatusId;
                        fumigationEventHistory.UserId = entity.CreatedBy;
                        fumigationEventHistory.Event = "STATUS";
                        fumigationEventHistory.EventDateTime = entity.CreatedOn;
                        fumigationContext.tblFumigationEventHistories.Add(fumigationEventHistory);

                    }
                    fumigation.IsSuccess = fumigationContext.SaveChanges() > 0;
                }
                return fumigation;
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
        /// <param name="fumigationEmailDTO"></param>
        /// <returns></returns>
        public FumigationEmailDTO GetCustomerDetail(FumigationEmailDTO fumigationEmailDTO)
        {
            try
            {


                var customerDetail = new FumigationEmailDTO();

                customerDetail = (from fumigation in fumigationContext.tblFumigations
                                  join customer in fumigationContext.tblCustomerRegistrations on fumigation.CustomerId equals customer.CustomerID
                                  join user in fumigationContext.tblUsers on customer.UserId equals user.Userid
                                  join status in fumigationContext.tblShipmentStatus on fumigation.StatusId equals status.StatusId
                                  where (fumigationEmailDTO.FumigationId > 0 ? fumigationEmailDTO.FumigationId == fumigation.FumigationId : 1 == 1) && (fumigationEmailDTO.CustomerId > 0 ? fumigationEmailDTO.CustomerId == fumigation.CustomerId : 1 == 1)

                                  //&& fumigation.StatusId != 11
                                  select new FumigationEmailDTO
                                  {
                                      StatusId = fumigation.StatusId,
                                      FumigationId = fumigation.FumigationId,
                                      CustomerName = customer.CustomerName ?? string.Empty,
                                      ContactPersons = (from contactPerson in fumigationContext.tblCustomerContacts
                                                        where contactPerson.CustomerId == customer.CustomerID
                                                        select new CustomerContact
                                                        {
                                                            ContactEmail = contactPerson.ContactEmail,
                                                        }
                                                         ).ToList(),
                                      //Consignee = fumigation.VendorNconsignee.ToUpper() ?? string.Empty,


                                      FumigationEquipmentNDriver = (from eqpNdriver in fumigationContext.tblFumigationEquipmentNDrivers
                                                                    where eqpNdriver.FumigationId == fumigation.FumigationId
                                                                    select new GetFumigationEquipmentNDriver
                                                                    {
                                                                        FumigationEquipmentNDriverId = eqpNdriver.FumigationEquipmentNDriverId,
                                                                        FumigationRoutsId = eqpNdriver.FumigationRoutsId,
                                                                        DriverId = eqpNdriver.DriverId,
                                                                        EquipmentId = eqpNdriver.EquipmentId,
                                                                        IsPickUp = eqpNdriver.IsPickUp,
                                                                        DriverName = (from driver in fumigationContext.tblDrivers
                                                                                      where eqpNdriver.DriverId == driver.DriverID
                                                                                      select new
                                                                                      {
                                                                                          DriverName =
                                                                                      (driver.FirstName + " " + (driver.LastName ?? string.Empty))
                                                                                      }).Select(x => x.DriverName).FirstOrDefault() ?? string.Empty,
                                                                        EquipmentName = (from equipment in fumigationContext.tblEquipmentDetails
                                                                                         join equipmentType in fumigationContext.tblEquipmentTypes on equipment.VehicleType equals equipmentType.VehicleTypeID
                                                                                         where eqpNdriver.EquipmentId == equipment.EDID
                                                                                         select new { EquipmentNo = (equipmentType.VehicleTypeName + " " + equipment.EquipmentNo) }).Select(x => x.EquipmentNo).FirstOrDefault() ?? string.Empty,
                                                                    }
                                                                    ).ToList(),

                                      CustomerMail = user.UserName ?? string.Empty,
                                      ShipmentRefNo = fumigation.ShipmentRefNo,
                                      Status = status.StatusName ?? string.Empty,
                                      SubStatus = fumigationContext.tblShipmentSubStatus.Where(x => x.SubStatusId == fumigation.SubStatusId).Select(x => x.SubStatusName).FirstOrDefault() ?? string.Empty,
                                      OrderTaken = fumigationContext.tblFumigationRouts.Where(x => x.FumigationId == fumigation.FumigationId).Select(x => x.PickUpArrival).FirstOrDefault(),
                                      ESTDateTime = fumigationContext.tblFumigationRouts.Where(x => x.FumigationId == fumigation.FumigationId).OrderByDescending(x => x.FumigationRoutsId).Select(x => x.DeliveryArrival).FirstOrDefault(),
                                      FumigationStatusHistory = (from fumigationHistory in fumigationContext.tblFumigationStatusHistories
                                                                 join status in fumigationContext.tblShipmentStatus on fumigationHistory.StatusId equals status.StatusId
                                                                 where fumigationHistory.FumigationId == fumigation.FumigationId && status.StatusId != 1
                                                                 //group fumigationHistory by fumigationHistory.StatusId into g
                                                                 select new FumigationStatusHistoryDTO
                                                                 {
                                                                     FumigationHistoryId = fumigationHistory.FumigationStatusHistoryId,
                                                                     StatusId = fumigationHistory.StatusId,
                                                                     DateTime = fumigationHistory.CreatedOn,
                                                                     Status = status.StatusName ?? string.Empty,
                                                                     SubStatusId = fumigationHistory.SubStatusId,
                                                                     SubStatus = fumigationContext.tblShipmentSubStatus.Where(x => x.SubStatusId == fumigationHistory.SubStatusId).Select(x => x.SubStatusName).FirstOrDefault() ?? string.Empty,
                                                                     Colour = status.Colour,
                                                                     ImageUrl = (LarastruckingApp.Entities.Common.Configurations.ImageURL + status.ImageURL),
                                                                     Reason = fumigationHistory.Reason,
                                                                 }
                                                             ).OrderBy(x => x.FumigationHistoryId).ToList(),
                                      ShipmentStatusList = (from status in fumigationContext.tblShipmentStatus
                                                            where status.StatusId != 1 && status.StatusId != 3 && status.StatusId != 4 && status.StatusId != 8 && status.StatusId != 10 && status.StatusId != 11 && status.StatusId != 13
                                                            select new ShipmentStatusDTO
                                                            {
                                                                FumigationDisplayOrder = status.FumigationDisplayOrder,
                                                                StatusId = status.StatusId,
                                                                GrayImageURL = (LarastruckingApp.Entities.Common.Configurations.ImageURL + status.GrayImageURL),
                                                            }
                                                         ).OrderBy(x => x.FumigationDisplayOrder).ToList(),
                                      FumigationRoute = (from fumigationRoute in fumigationContext.tblFumigationRouts
                                                         where fumigationRoute.FumigationId == fumigation.FumigationId && fumigationRoute.IsDeleted == false
                                                         select new GetFumigationRouteDTO
                                                         {
                                                             FumigatonType = fumigationContext.tblFumigationTypes.Where(x => x.FumigationTypeId == fumigationRoute.FumigationTypeId).Select(x => x.FumigationName).FirstOrDefault(),
                                                             PickUpLocationText = (
                                                                               from address in fumigationContext.tblAddresses
                                                                               where fumigationRoute.PickUpLocation == address.AddressId
                                                                               select new
                                                                               {
                                                                                   PickupLocation = (address.CompanyName + " | " + address.City),

                                                                               }).Select(x => x.PickupLocation).FirstOrDefault(),
                                                             FumigationSiteText = (
                                                                               from address in fumigationContext.tblAddresses
                                                                               where fumigationRoute.FumigationSite == address.AddressId
                                                                               select new
                                                                               {
                                                                                   FumigationSite = (address.CompanyName + " | " + address.City),

                                                                               }).Select(x => x.FumigationSite).FirstOrDefault(),
                                                             DeliveryLocationText = (
                                                                               from address in fumigationContext.tblAddresses
                                                                               where fumigationRoute.DeliveryLocation == address.AddressId
                                                                               select new
                                                                               {
                                                                                   DeliveryLocation = (address.CompanyName + " | " + address.City),

                                                                               }).Select(x => x.DeliveryLocation).FirstOrDefault(),
                                                             FumigationRoutsId = fumigationRoute.FumigationRoutsId,
                                                             Commodity = fumigationRoute.Commodity ?? string.Empty,
                                                             PalletCount = fumigationRoute.PalletCount ?? 0,
                                                             Temperature = fumigationRoute.Temperature,
                                                             TemperatureType = fumigationRoute.TemperatureType,
                                                             BoxCount = fumigationRoute.BoxCount ?? 0,
                                                             AirWayBill = fumigationRoute.AirWayBill ?? string.Empty,
                                                             CustomerPO = fumigationRoute.CustomerPO ?? string.Empty,
                                                             ContainerNo = fumigationRoute.ContainerNo,
                                                             ReceiverName = fumigationRoute.ReceiverName,
                                                             DigitalSignature = fumigationRoute.DigitalSignature ?? string.Empty,
                                                             DigitalSignaturePath = (LarastruckingApp.Entities.Common.Configurations.ImageURL + fumigationRoute.DigitalSignaturePath),
                                                             VendorNConsignee = fumigationRoute.VendorNConsignee,
                                                             SignatureDateTime = fumigationRoute.DriverDeliveryDeparture,
                                                             //LoadingTemp = fumigationContext.tblFumigationProofOfTemperatureImages.Where(X => X.FumigationRouteId == fumigationRoute.FumigationRoutsId && X.IsLoading && X.IsApproved).OrderByDescending(X => X.ImageId).Select(x => x.ActualTemperature).FirstOrDefault() ?? 0,
                                                             // DeliveryTemp = fumigationContext.tblFumigationProofOfTemperatureImages.Where(X => X.FumigationRouteId == fumigationRoute.FumigationRoutsId && X.IsLoading == false && X.IsApproved).OrderByDescending(X => X.ImageId).Select(x => x.ActualTemperature).FirstOrDefault() ?? 0,


                                                         }
                                                             ).ToList(),
                                      ProofOfTemprature = (from proofoftemp in fumigationContext.tblFumigationProofOfTemperatureImages
                                                           join fumigationRoute in fumigationContext.tblFumigationRouts on proofoftemp.FumigationRouteId equals fumigationRoute.FumigationRoutsId
                                                           where fumigationRoute.FumigationId == fumigation.FumigationId && proofoftemp.IsDeleted == false && fumigationRoute.IsDeleted == false
                                                           select new FumigationProofOfTemprature
                                                           {
                                                               IsApproved = proofoftemp.IsApproved,
                                                               ProofOfTempratureId = proofoftemp.ImageId,
                                                               FumigationRouteId = fumigationRoute.FumigationRoutsId,
                                                               ImageName = proofoftemp.ImageName,
                                                               ImageUrl = proofoftemp.ImageUrl,
                                                               CreatedOn = proofoftemp.CreatedOn,
                                                               ActualTemperature = proofoftemp.ActualTemperature,
                                                               IsLoading = proofoftemp.IsLoading

                                                           }
                                                         ).OrderByDescending(x => x.CreatedOn).ToList(),

                                      DamageImages = (from damageImage in fumigationContext.tblFumigationDamagedImages
                                                      join fumigatonRoute in fumigationContext.tblFumigationRouts on damageImage.FumigationRouteId equals fumigatonRoute.FumigationRoutsId
                                                      where fumigatonRoute.FumigationId == fumigation.FumigationId && damageImage.IsDeleted == false && fumigatonRoute.IsDeleted == false
                                                      select new FumigationDamageImages
                                                      {
                                                          ImageName = damageImage.ImageName,
                                                          ImageUrl = damageImage.ImageUrl
                                                      }).ToList(),

                                      AccessorialPrice = (from accessorialprice in fumigationContext.tblFumigationAccessorialPrices
                                                          where accessorialprice.FumigationId == fumigation.FumigationId && accessorialprice.IsDeleted == false && accessorialprice.IsDeleted == false
                                                          select new GetFumigationAccessorialPriceDTO
                                                          {
                                                              FumigationRoutesId = accessorialprice.FumigationRoutesId,
                                                              Amount = accessorialprice.Amount,
                                                              FeeType = (from aft in fumigationContext.tblAccessorialFeesTypes where aft.Id == accessorialprice.AccessorialFeeTypeId select (accessorialprice.AccessorialFeeTypeId == 23 ? accessorialprice.Reason : aft.Name)).FirstOrDefault(),
                                                          }
                                                                                  ).ToList(),


                                  }
                                   ).OrderByDescending(x => x.FumigationId).FirstOrDefault();

                if (customerDetail != null)
                {
                    foreach (var proofOfTemp in customerDetail.ProofOfTemprature)
                    {
                        proofOfTemp.Ext = proofOfTemp.ImageName.Split('.')[1];
                        proofOfTemp.ImageUrl = (LarastruckingApp.Entities.Common.Configurations.ImageURL + proofOfTemp.ImageUrl);
                    }

                    foreach (var damageImages in customerDetail.DamageImages)
                    {
                        damageImages.Ext = damageImages.ImageName.Split('.')[1];
                        damageImages.ImageUrl = (LarastruckingApp.Entities.Common.Configurations.ImageURL + damageImages.ImageUrl);
                    }

                    customerDetail.StatusGrayDotPath = LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingResource.StatusDotsGrayImage;
                    customerDetail.StatusDotPath = LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingResource.StatusDotsImage;
                    if (customerDetail.FumigationEquipmentNDriver.Count > 0)
                    {
                        customerDetail.Trailer = string.Join(" / ", customerDetail.FumigationEquipmentNDriver.GroupBy(x => x.EquipmentName).Select(x => x.Key).ToList());

                    }

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
                                                                   group AT by AT.FumigationRouteId into i
                                                                   select new GetProofOfTemprature
                                                                   {
                                                                       ActualTemperature = (from t in i select t.ActualTemperature).FirstOrDefault()
                                                                   }
                              ).ToList().Select(x => x.ActualTemperature));

                    customerDetail.AccessorialPrice = (from accessorialprice in customerDetail.AccessorialPrice
                                                       group accessorialprice by accessorialprice.FeeType into acc
                                                       select new GetFumigationAccessorialPriceDTO
                                                       {
                                                           FeeType = acc.Key,
                                                           Amount = (from a in acc where a.FeeType == acc.Key select a.Amount).Sum()
                                                       }
                                                     ).ToList();
                    if (customerDetail.FumigationRoute.Count > 0)
                    {
                        foreach (var routeStop in customerDetail.FumigationRoute)
                        {
                            routeStop.SignatureDateTime = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(routeStop.SignatureDateTime));
                        }
                        customerDetail.Consignee = string.Join(", ", (from FT in customerDetail.FumigationRoute
                                                                      group FT by FT.VendorNConsignee into i
                                                                      select new
                                                                      {
                                                                          VendorNConsignee = i.Key,
                                                                      }
                              ).ToList().Select(x => x.VendorNConsignee));
                        customerDetail.FumigationType = string.Join(", ", (from FT in customerDetail.FumigationRoute
                                                                           group FT by FT.FumigatonType into i
                                                                           select new
                                                                           {
                                                                               FumigatonType = i.Key,
                                                                           }
                              ).ToList().Select(x => x.FumigatonType));
                        customerDetail.Commodity = string.Join(", ", customerDetail.FumigationRoute.GroupBy(x => x.Commodity).ToList().Select(x => x.Key));

                        customerDetail.NoOfBox = customerDetail.FumigationRoute.Select(x => x.BoxCount).Sum();

                        customerDetail.Pallet = customerDetail.FumigationRoute.Select(x => x.PalletCount).Sum();

                        customerDetail.AirWayBill = string.Join(" / ", (from AWB in customerDetail.FumigationRoute
                                                                        where !string.IsNullOrEmpty(AWB.AirWayBill)
                                                                        group AWB by AWB.AirWayBill into g
                                                                        select new { AWBN = (from i in g select i.AirWayBill).FirstOrDefault() }).ToList().Select(x => x.AWBN));

                        customerDetail.CustomerPO = (string.Join(" / ", (from CP in customerDetail.FumigationRoute
                                                                         where !string.IsNullOrEmpty(CP.CustomerPO)
                                                                         group CP by CP.CustomerPO into g
                                                                         select new { CPN = (from i in g select i.CustomerPO).FirstOrDefault() }).ToList().Select(x => x.CPN)));
                        customerDetail.OrderNo = (string.Join(" / ", (from CN in customerDetail.FumigationRoute
                                                                      where !string.IsNullOrEmpty(CN.ContainerNo)
                                                                      group CN by CN.ContainerNo into g
                                                                      select new { CNN = (from i in g select i.ContainerNo).FirstOrDefault() }).ToList().Select(x => x.CNN)));

                        EncryptDecrypt ObjEncryptDecrypt = new EncryptDecrypt();
                        string fumigationId = ObjEncryptDecrypt.EncryptString(fumigationEmailDTO.FumigationId.ToString());
                        customerDetail.ProofOfDeliveryURL = LarastruckingApp.Entities.Common.Configurations.BaseURL + "ProofOfDelivery/FumigationProofOfDelivery?fumigationId=" + fumigationId;
                        if (customerDetail.FumigationStatusHistory.Count > 0)
                        {
                            foreach (var fumhistory in customerDetail.FumigationStatusHistory)
                            {
                                if (fumhistory.SubStatusId == 7 || fumhistory.SubStatusId == 11)
                                {
                                    fumhistory.SubStatus = fumhistory.Reason;
                                }
                                fumhistory.DateTime = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(fumhistory.DateTime));
                                var shipStatusHistory = customerDetail.ShipmentStatusList.Where(x => x.StatusId == fumhistory.StatusId).FirstOrDefault();
                                if (shipStatusHistory != null)
                                {
                                    customerDetail.ShipmentStatusList.Remove(shipStatusHistory);
                                }
                            }


                        }
                        customerDetail.Equipments = string.Join(" | ", customerDetail.FumigationEquipmentNDriver.GroupBy(y => y.EquipmentName).Select(y => y.Key).ToList());
                        customerDetail.Drivers = string.Join(" | ", customerDetail.FumigationEquipmentNDriver.GroupBy(y => y.DriverName).Select(y => y.Key).ToList());


                        string temprature = string.Empty;
                        string damageDetail = string.Empty;
                        int routeNo = 1;


                        foreach (var routs in customerDetail.FumigationRoute)
                        {
                            var proofOfTempList = customerDetail.ProofOfTemprature.ToList().Where(x => x.FumigationRouteId == routs.FumigationRoutsId && x.IsLoading == true).GroupBy(x => x.ActualTemperature).Select(x => x.Key).ToList();
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
                                        temprature += " Temp " + routeNo + ": " + temp + unit;
                                    }

                                }

                            }

                            var damageDetailList = fumigationContext.tblFumigationDamagedImages.ToList().Where(x => x.FumigationRouteId == routs.FumigationRoutsId).GroupBy(x => x.ImageDescription).Select(x => x.Key).ToList();
                            if (damageDetailList.Count > 0)
                            {
                                foreach (var damage in damageDetailList)
                                {
                                    damageDetail += " D" + routeNo + ": " + damage + " |";
                                }
                            }
                            routeNo = routeNo + 1;
                        }
                        customerDetail.Tempratures = temprature.TrimEnd('|');
                        customerDetail.DamageDetails = damageDetail.TrimEnd('|'); ;


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

        #region fumigation proof of delivery
        /// <summary>
        /// Get proof Of delivery
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        public List<GetFumigationRouteDTO> FumigationProofOfDelivery(string fumigationId)
        {

            List<GetFumigationRouteDTO> objGetFumigationRoute = new List<GetFumigationRouteDTO>();
            try
            {
                if (!string.IsNullOrEmpty(fumigationId))
                {

                    EncryptDecrypt ObjEncryptDecrypt = new EncryptDecrypt();
                    string strfumigationId = ObjEncryptDecrypt.DecryptString(fumigationId);
                    if (!string.IsNullOrEmpty(strfumigationId))
                    {
                        int fmId = Convert.ToInt32(strfumigationId) > 0 ? Convert.ToInt32(strfumigationId) : 0;
                        if (fmId > 0)
                        {
                            objGetFumigationRoute = (from fumigatinRoute in fumigationContext.tblFumigationRouts
                                                     where fumigatinRoute.FumigationId == fmId && fumigatinRoute.DigitalSignature != null && fumigatinRoute.IsDeleted == false
                                                     select new GetFumigationRouteDTO
                                                     {
                                                         RouteNo = fumigatinRoute.RouteNo,
                                                         FumigationId = fumigatinRoute.FumigationId,
                                                         ReceiverName = fumigatinRoute.ReceiverName,
                                                         DigitalSignature = fumigatinRoute.DigitalSignature ?? string.Empty,
                                                         DeliveryLocationText = (from address in fumigationContext.tblAddresses
                                                                                 join state in fumigationContext.tblStates on address.State equals state.ID
                                                                                 where address.AddressId == fumigatinRoute.DeliveryLocation
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
            return objGetFumigationRoute;
        }
        #endregion

        #region get fumigation detail by id for copy
        /// <summary>
        /// Get proof Of delivery
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        public CopyFumigationDTO GetFumigationDetailById(int fumigationId)
        {

            var fumigationDetail = new CopyFumigationDTO();
            try
            {
                fumigationDetail = (from fumigation in fumigationContext.tblFumigations
                                    join customer in fumigationContext.tblCustomerRegistrations on fumigation.CustomerId equals customer.CustomerID
                                    join routes in fumigationContext.tblFumigationRouts on fumigation.FumigationId equals routes.FumigationId
                                    where fumigation.IsDeleted == false && routes.IsDeleted == false && fumigation.FumigationId == fumigationId
                                    select new CopyFumigationDTO
                                    {
                                        FumigationId = fumigation.FumigationId,
                                        CustomerId = fumigation.CustomerId,
                                        CustomerName = customer.CustomerName,
                                        AWB = routes.AirWayBill,
                                        RequestedLoading = routes.PickUpArrival,
                                        FumigationIn = routes.FumigationArrival,
                                        FumigatiionRelease = routes.ReleaseDate,
                                        DelEstArrival = routes.DeliveryArrival
                                    }

                                  ).FirstOrDefault();

                if (fumigationDetail != null)
                {
                    fumigationDetail.RequestedLoading = Configurations.ConvertDateTime(Convert.ToDateTime(fumigationDetail.RequestedLoading));
                    fumigationDetail.FumigationIn = Configurations.ConvertDateTime(Convert.ToDateTime(fumigationDetail.FumigationIn));
                    fumigationDetail.FumigatiionRelease = Configurations.ConvertDateTime(Convert.ToDateTime(fumigationDetail.FumigatiionRelease));
                    fumigationDetail.DelEstArrival = Configurations.ConvertDateTime(Convert.ToDateTime(fumigationDetail.DelEstArrival));
                }
            }
            catch (Exception)
            {

                throw;
            }
            return fumigationDetail;
        }
        #endregion

        #region Copy Fumigation
        /// <summary>
        /// Copy Fumigation detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public bool SaveCopyFumigatonDetail(CopyFumigationDTO entity)
        {

            try
            {
                bool isSuccess = false;
                if (entity != null)
                {

                    var fumigationDetail = fumigationContext.tblFumigations.Where(x => x.FumigationId == entity.FumigationId).FirstOrDefault();

                    if (fumigationDetail != null)
                    {

                        tblFumigation fumigation = new tblFumigation();
                        fumigation.CustomerId = entity.CustomerId;
                        fumigation.StatusId = 1;
                        fumigation.RequestedBy = fumigationDetail.RequestedBy;
                        fumigation.VendorNconsignee = fumigationDetail.VendorNconsignee;
                        fumigation.ShipmentRefNo = fumigationDetail.ShipmentRefNo;
                        fumigation.Comments = fumigationDetail.Comments;
                        fumigation.CreatedBy = entity.CreatedBy;
                        fumigation.CreatedOn = entity.CreatedDate;
                        fumigationContext.tblFumigations.Add(fumigation);



                        tblFumigationStatusHistory objFumigationStatusHistories = new tblFumigationStatusHistory();
                        objFumigationStatusHistories.FumigationId = fumigation.FumigationId;
                        objFumigationStatusHistories.StatusId = 1;
                        objFumigationStatusHistories.CreatedBy = entity.CreatedBy;
                        objFumigationStatusHistories.CreatedOn = entity.CreatedDate;
                        fumigationContext.tblFumigationStatusHistories.Add(objFumigationStatusHistories);
                        isSuccess = fumigationContext.SaveChanges() > 0;


                        tblFumigationEventHistory fumigationEventHistory = new tblFumigationEventHistory();
                        fumigationEventHistory.FumigationId = fumigation.FumigationId;
                        fumigationEventHistory.StatusId = 1;
                        fumigationEventHistory.UserId = entity.CreatedBy;
                        fumigationEventHistory.Event = "STATUS";
                        fumigationEventHistory.EventDateTime = entity.CreatedDate;
                        fumigationContext.tblFumigationEventHistories.Add(fumigationEventHistory);

                        var fumigationRouteDetail = fumigationContext.tblFumigationRouts.Where(x => x.FumigationId == entity.FumigationId && x.IsDeleted == false).ToList();

                        foreach (var fumigationRoute in fumigationRouteDetail)
                        {

                            tblFumigationRout objFumigationRoute = new tblFumigationRout();
                            objFumigationRoute.RouteNo = fumigationRoute.RouteNo;
                            objFumigationRoute.FumigationId = fumigation.FumigationId;
                            objFumigationRoute.FumigationTypeId = fumigationRoute.FumigationTypeId;
                            objFumigationRoute.AirWayBill = entity.AWB;
                            //objFumigationRoute.CustomerPO = fumigationRoute.CustomerPO;
                            //objFumigationRoute.ContainerNo = fumigationRoute.ContainerNo;
                            objFumigationRoute.PickUpLocation = fumigationRoute.PickUpLocation;
                            if (entity.RequestedLoading != null)
                            {
                                objFumigationRoute.PickUpArrival = Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.RequestedLoading));
                            }

                            objFumigationRoute.FumigationSite = fumigationRoute.FumigationSite;

                            if (entity.FumigationIn != null)
                            {
                                objFumigationRoute.FumigationArrival = Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.FumigationIn));
                            }
                            if (entity.FumigatiionRelease != null)
                            {
                                objFumigationRoute.ReleaseDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.FumigatiionRelease));
                            }

                            objFumigationRoute.DeliveryLocation = fumigationRoute.DeliveryLocation;
                            if (entity.DelEstArrival != null)
                            {
                                objFumigationRoute.DeliveryArrival = Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.DelEstArrival));
                            }
                            objFumigationRoute.Commodity = fumigationRoute.Commodity;
                            objFumigationRoute.PalletCount = fumigationRoute.PalletCount;
                            objFumigationRoute.BoxCount = fumigationRoute.BoxCount;
                            objFumigationRoute.BoxType = fumigationRoute.BoxType;
                            objFumigationRoute.Temperature = fumigationRoute.Temperature;
                            objFumigationRoute.TemperatureType = fumigationRoute.TemperatureType;
                            objFumigationRoute.TrailerDays = fumigationRoute.TrailerDays;
                            objFumigationRoute.PricingMethod = fumigationRoute.PricingMethod;
                            objFumigationRoute.MinFee = fumigationRoute.MinFee;
                            objFumigationRoute.AddFee = fumigationRoute.AddFee;
                            objFumigationRoute.TrailerPosition = fumigationRoute.TrailerPosition;
                            objFumigationRoute.TotalFee = fumigationRoute.TotalFee;
                            fumigationContext.tblFumigationRouts.Add(objFumigationRoute);


                        }
                        isSuccess = fumigationContext.SaveChanges() > 0;
                    }


                }
                return isSuccess;
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
        /// <param name="fumigationId"></param>
        /// <returns></returns>

        public int? GetMaxRouteNo(int fumigationId)
        {
            int? MaxRouteNo = 0;
            if (fumigationId > 0)
            {
                MaxRouteNo = fumigationContext.tblFumigationRouts.Where(x => x.FumigationId == fumigationId).Max(x => x.RouteNo);
            }
            return MaxRouteNo;

        }
        #endregion

        #region GetTemperatureEmailDetail
        public TemperatureEmailDTO GetTemperatureEmailDetail(int fumigationId)
        {
            try
            {
                var temperatureDetail = new TemperatureEmailDTO();

                temperatureDetail = (from fumigationRouteStop in fumigationContext.tblFumigationRouts
                                     where fumigationRouteStop.FumigationRoutsId == fumigationId && fumigationRouteStop.IsDeleted == false
                                     select new TemperatureEmailDTO
                                     {
                                         FumigationRoutsId = fumigationRouteStop.FumigationRoutsId,
                                         FumigationId = fumigationRouteStop.FumigationId,
                                         PickUpArrival = fumigationRouteStop.PickUpArrival,
                                         DriverPickUpArrival = fumigationRouteStop.DriverPickupArrival,
                                         DriverLoadingStartTime = fumigationRouteStop.DriverLoadingStartTime,
                                         DriverLoadingFinishTime = fumigationRouteStop.DriverLoadingFinishTime,
                                         DriverFumigationIn = fumigationRouteStop.DriverFumigationIn,
                                         AirWayBill = fumigationRouteStop.AirWayBill,
                                         OrderNo = fumigationRouteStop.ContainerNo,
                                         CustomerPO = fumigationRouteStop.CustomerPO,
                                         LogoURL = (LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingApp.Entities.Common.Configurations.LarasLogo),
                                         ActTemp = "",
                                         ActTemperature = 0
                                     }

                                  ).FirstOrDefault();

               
                

                return temperatureDetail;
            }
            catch(Exception)
            {
                throw;
            }

        }
        #endregion

    }
}
