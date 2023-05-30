using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.TrailerRental;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
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
    public class TrailerRentalRepository : ITrailerRentalRepository
    {
        #region private member
        private readonly ExecuteSQLStoredProceduce sp_dbContext = null;
        private readonly LarastruckingDBEntities trailerRentalContext;
        #endregion

        #region constroctor
        public TrailerRentalRepository()
        {
            trailerRentalContext = new LarastruckingDBEntities();
            sp_dbContext = new ExecuteSQLStoredProceduce();
        }
        #endregion

        #region Save Trailer Rental
        /// <summary>
        /// Save trailer rental detial
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveTrailerRental(TrailerRentalDTO model)
        {
            try
            {

                if (model != null)
                {
                    tblTrailerRental objTrailerRental = new tblTrailerRental();
                    objTrailerRental.CustomerId = model.CustomerId;
                    objTrailerRental.TrailerInstruction = model.TrailerInstruction;
                    objTrailerRental.GrandTotal = model.GrandTotal;
                    objTrailerRental.CreatedBy = model.CreatedBy;
                    objTrailerRental.CreatedDate = model.CreatedDate;
                    objTrailerRental.IsDeleted = false;
                    trailerRentalContext.tblTrailerRentals.Add(objTrailerRental);
                    if (model.TrailerRentalDetail != null)
                    {
                        if (model.TrailerRentalDetail.Count > 0)
                        {
                            foreach (var trailerRentalDetail in model.TrailerRentalDetail)
                            {
                                tblTrailerRentalDetail objTrailerRentalDetail = new tblTrailerRentalDetail();
                                objTrailerRentalDetail.TrailerRentalId = objTrailerRental.TrailerRentalId;
                                objTrailerRentalDetail.DeliveryLocationId = trailerRentalDetail.DeliveryLocationId;

                                objTrailerRentalDetail.PickUpLocationId = trailerRentalDetail.PickUpLocationId;

                                if (trailerRentalDetail.DeliveryDriverId > 0)
                                {
                                    objTrailerRentalDetail.DeliveryDriverId = trailerRentalDetail.DeliveryDriverId;
                                }
                                if (trailerRentalDetail.PickupDriverId > 0)
                                {
                                    objTrailerRentalDetail.PickupDriverId = trailerRentalDetail.PickupDriverId;
                                }
                                objTrailerRentalDetail.StartDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(trailerRentalDetail.StartDate));
                                objTrailerRentalDetail.EndDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(trailerRentalDetail.EndDate));
                                objTrailerRentalDetail.ReturnedDate = trailerRentalDetail.ReturnedDate == null ? trailerRentalDetail.ReturnedDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(trailerRentalDetail.ReturnedDate));
                                objTrailerRentalDetail.NoOfDays = trailerRentalDetail.NoOfDays;
                                objTrailerRentalDetail.FeePerDay = trailerRentalDetail.FeePerDay;
                                objTrailerRentalDetail.FixedFee = trailerRentalDetail.FixedFee;
                                objTrailerRentalDetail.EquipmentId = trailerRentalDetail.EquipmentId;
                                objTrailerRentalDetail.TotalFee = trailerRentalDetail.TotalFee;
                                objTrailerRentalDetail.CreatedBy = model.CreatedBy;
                                objTrailerRentalDetail.CreatedDate = model.CreatedDate;
                                objTrailerRentalDetail.IsDeleted = false;
                                trailerRentalContext.tblTrailerRentalDetails.Add(objTrailerRentalDetail);
                            }
                        }
                    }
                }
                return trailerRentalContext.SaveChanges() > 0;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region get trailer rental list
        /// <summary>
        /// get trailer rental list
        /// </summary>
        /// <returns></returns>

        public IList<TrailerRentalListDTO> GetTrailerRentalList(DataTableFilterDto entity)
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
                        totalCount
                    };

                var result = sp_dbContext.ExecuteStoredProcedure<TrailerRentalListDTO>("usp_GetTrailerRentalList", sqlParameters);
                entity.TotalCount = Convert.ToInt32(totalCount.Value);
                if (result.Count > 0)
                {
                    foreach (var trailerData in result)
                    {
                        trailerData.StartDate = Configurations.ConvertDateTime(trailerData.StartDate);
                        trailerData.EndDate = Configurations.ConvertDateTime(trailerData.EndDate);
                    }
                }
                return result != null && result.Count > 0 ? result : new List<TrailerRentalListDTO>();
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Trailer Rental Detail by id
        /// <summary>
        /// get trailer rental detial by id
        /// </summary>
        /// <param name="trailerRentalId"></param>
        /// <returns></returns>
        public TrailerRentalDTO GetTrailerRentalDetailById(int trailerRentalId)
        {
            try
            {


                var trailerRentalDetail = (from trailerRental in trailerRentalContext.tblTrailerRentals
                                           join customer in trailerRentalContext.tblCustomerRegistrations on trailerRental.CustomerId equals customer.CustomerID
                                           where trailerRental.TrailerRentalId == trailerRentalId
                                           select new TrailerRentalDTO
                                           {
                                               TrailerRentalId = trailerRental.TrailerRentalId,
                                               CustomerId = trailerRental.CustomerId,
                                               CustomerName = customer.CustomerName,
                                               TrailerInstruction = trailerRental.TrailerInstruction ?? string.Empty,
                                               GrandTotal = trailerRental.GrandTotal ?? 0,
                                               TrailerRentalDetail = (from trailerDetail in trailerRentalContext.tblTrailerRentalDetails
                                                                      where trailerDetail.TrailerRentalId == trailerRental.TrailerRentalId && trailerDetail.IsDeleted == false
                                                                      select new TrailerRentalDetailDTO
                                                                      {
                                                                          TrailerRentalDetailId = trailerDetail.TrailerRentalDetailId,
                                                                          TrailerRentalId = trailerDetail.TrailerRentalId,
                                                                          PickupDriverId = trailerDetail.PickupDriverId,

                                                                          DeliveryDriverId = trailerDetail.DeliveryDriverId,
                                                                          PickUpLocationId = trailerDetail.PickUpLocationId,
                                                                          PickUpLocationText = (from address in trailerRentalContext.tblAddresses
                                                                                                join state in trailerRentalContext.tblStates on address.State equals state.ID
                                                                                                where address.AddressId == trailerDetail.PickUpLocationId
                                                                                                select new { PicUpLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName.Trim() + ",")) + address.Address1 + " " + address.City + " " + state.Name + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault(),
                                                                          DeliveryLocationId = trailerDetail.DeliveryLocationId,
                                                                          DeliveryLocationText = (from address in trailerRentalContext.tblAddresses
                                                                                                  join state in trailerRentalContext.tblStates on address.State equals state.ID
                                                                                                  where address.AddressId == trailerDetail.DeliveryLocationId
                                                                                                  select new { PicUpLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName.Trim() + ",")) + address.Address1 + " " + address.City + " " + state.Name + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault(),
                                                                          StartDate = trailerDetail.StartDate,
                                                                          EndDate = trailerDetail.EndDate,
                                                                          ReturnedDate = trailerDetail.ReturnedDate,
                                                                          NoOfDays = trailerDetail.NoOfDays ?? 0,
                                                                          EquipmentId = trailerDetail.EquipmentId,

                                                                          FeePerDay = trailerDetail.FeePerDay ?? 0,
                                                                          FixedFee=trailerDetail.FixedFee??0,
                                                                          TotalFee = trailerDetail.TotalFee ?? 0,
                                                                          EquipmentNo = trailerRentalContext.tblEquipmentDetails.Where(x => x.EDID == trailerDetail.EquipmentId).Select(x => x.EquipmentNo).FirstOrDefault() ?? string.Empty,
                                                                      }).ToList()
                                           }


                                         ).FirstOrDefault();
                if (trailerRentalDetail != null)
                {
                    foreach (var TrailerRentalDetail in trailerRentalDetail.TrailerRentalDetail)
                    {
                        TrailerRentalDetail.StartDate = Configurations.ConvertDateTime(TrailerRentalDetail.StartDate);
                        TrailerRentalDetail.EndDate = Configurations.ConvertDateTime(TrailerRentalDetail.EndDate);
                        TrailerRentalDetail.ReturnedDate = (TrailerRentalDetail.ReturnedDate == null ? TrailerRentalDetail.ReturnedDate : Configurations.ConvertDateTime(Convert.ToDateTime(TrailerRentalDetail.ReturnedDate)));
                    }
                }
                return trailerRentalDetail;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Edit Trailer Rental
        /// <summary>
        /// Edit trailer rental detial
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditTrailerRental(TrailerRentalDTO model)
        {
            try
            {


                if (model != null)
                {
                    var objTrailerRental = trailerRentalContext.tblTrailerRentals.Where(x => x.TrailerRentalId == model.TrailerRentalId).FirstOrDefault();
                    objTrailerRental.CustomerId = model.CustomerId;
                    objTrailerRental.TrailerInstruction = model.TrailerInstruction;
                    objTrailerRental.GrandTotal = model.GrandTotal;
                    objTrailerRental.ModifiedBy = model.CreatedBy;
                    objTrailerRental.ModifiedDate = model.CreatedDate;
                    trailerRentalContext.Entry(objTrailerRental).State = EntityState.Modified;

                    if (model.TrailerRentalDetail.Count > 0)
                    {
                        foreach (var trailerRentalDetail in model.TrailerRentalDetail)
                        {
                            if (trailerRentalDetail.TrailerRentalDetailId == 0 && trailerRentalDetail.IsDeleted == false)
                            {
                                tblTrailerRentalDetail objTrailerRentalDetail = new tblTrailerRentalDetail();
                                objTrailerRentalDetail.TrailerRentalId = objTrailerRental.TrailerRentalId;
                                objTrailerRentalDetail.DeliveryLocationId = trailerRentalDetail.DeliveryLocationId;
                                objTrailerRentalDetail.PickUpLocationId = trailerRentalDetail.PickUpLocationId;
                                objTrailerRentalDetail.DeliveryDriverId = (trailerRentalDetail.DeliveryDriverId == 0 ? null : trailerRentalDetail.DeliveryDriverId);
                                objTrailerRentalDetail.PickupDriverId = (trailerRentalDetail.PickupDriverId == 0 ? null : trailerRentalDetail.PickupDriverId);
                                objTrailerRentalDetail.StartDate = Configurations.ConvertLocalToUTC(trailerRentalDetail.StartDate);
                                objTrailerRentalDetail.EndDate = Configurations.ConvertLocalToUTC(trailerRentalDetail.EndDate);
                                objTrailerRentalDetail.ReturnedDate = (trailerRentalDetail.ReturnedDate == null ? trailerRentalDetail.ReturnedDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(trailerRentalDetail.ReturnedDate)));
                                objTrailerRentalDetail.NoOfDays = trailerRentalDetail.NoOfDays;
                                objTrailerRentalDetail.FeePerDay = trailerRentalDetail.FeePerDay;
                                objTrailerRentalDetail.FixedFee = trailerRentalDetail.FixedFee;
                                objTrailerRentalDetail.EquipmentId = trailerRentalDetail.EquipmentId;
                                objTrailerRentalDetail.TotalFee = trailerRentalDetail.TotalFee;
                                objTrailerRentalDetail.CreatedBy = model.CreatedBy;
                                objTrailerRentalDetail.CreatedDate = model.CreatedDate;
                                objTrailerRentalDetail.IsDeleted = false;
                                trailerRentalContext.tblTrailerRentalDetails.Add(objTrailerRentalDetail);
                            }
                            else if (trailerRentalDetail.TrailerRentalDetailId > 0 && trailerRentalDetail.IsDeleted == false)
                            {
                                var objTrailerRentalDetail = trailerRentalContext.tblTrailerRentalDetails.Where(x => x.TrailerRentalDetailId == trailerRentalDetail.TrailerRentalDetailId).FirstOrDefault();
                                objTrailerRentalDetail.DeliveryLocationId = trailerRentalDetail.DeliveryLocationId;
                                objTrailerRentalDetail.PickUpLocationId = trailerRentalDetail.PickUpLocationId;
                                objTrailerRentalDetail.DeliveryDriverId = (trailerRentalDetail.DeliveryDriverId == 0 ? null : trailerRentalDetail.DeliveryDriverId);
                                objTrailerRentalDetail.PickupDriverId = (trailerRentalDetail.PickupDriverId == 0 ? null : trailerRentalDetail.PickupDriverId);
                                //objTrailerRentalDetail.DeliveryDriverId = trailerRentalDetail.DeliveryDriverId;
                                //objTrailerRentalDetail.PickupDriverId = trailerRentalDetail.PickupDriverId;
                                objTrailerRentalDetail.StartDate = Configurations.ConvertLocalToUTC(trailerRentalDetail.StartDate);
                                objTrailerRentalDetail.EndDate = Configurations.ConvertLocalToUTC(trailerRentalDetail.EndDate);
                                objTrailerRentalDetail.ReturnedDate = (trailerRentalDetail.ReturnedDate == null ? trailerRentalDetail.ReturnedDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(trailerRentalDetail.ReturnedDate)));
                                objTrailerRentalDetail.NoOfDays = trailerRentalDetail.NoOfDays;
                                objTrailerRentalDetail.FeePerDay = trailerRentalDetail.FeePerDay;
                                objTrailerRentalDetail.FixedFee = trailerRentalDetail.FixedFee;
                                objTrailerRentalDetail.EquipmentId = trailerRentalDetail.EquipmentId;
                                objTrailerRentalDetail.TotalFee = trailerRentalDetail.TotalFee;
                                objTrailerRentalDetail.ModifiedBy = model.CreatedBy;
                                objTrailerRentalDetail.ModifiedDate = model.CreatedDate;
                                objTrailerRentalDetail.IsDeleted = false;
                                trailerRentalContext.Entry(objTrailerRentalDetail).State = EntityState.Modified;
                            }
                            else if (trailerRentalDetail.TrailerRentalDetailId > 0 && trailerRentalDetail.IsDeleted == true)
                            {
                                var objTrailerRentalDetail = trailerRentalContext.tblTrailerRentalDetails.Where(x => x.TrailerRentalDetailId == trailerRentalDetail.TrailerRentalDetailId).FirstOrDefault();

                                objTrailerRentalDetail.DeletedBy = model.CreatedBy;
                                objTrailerRentalDetail.DeletedDate = model.CreatedDate;
                                objTrailerRentalDetail.IsDeleted = true;
                                trailerRentalContext.Entry(objTrailerRentalDetail).State = EntityState.Modified;
                            }


                        }
                    }

                }
                return trailerRentalContext.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        
        #region delete trailer rental
        /// <summary>
        /// Delete trailer rental
        /// </summary>
        /// <param name="trailerRetalId"></param>
        /// <returns></returns>
        public bool DeleteTrailerRental(TrailerRentalDTO model)
        {
            try
            {                
                if (model.TrailerRentalId > 0)
                {
                    var objTrailerRentalDetail = trailerRentalContext.tblTrailerRentals.Where(x => x.TrailerRentalId == model.TrailerRentalId).FirstOrDefault();
                    if (objTrailerRentalDetail != null)
                    {
                        objTrailerRentalDetail.DeletedBy = model.CreatedBy;
                        objTrailerRentalDetail.DeletedDate = model.CreatedDate;
                        objTrailerRentalDetail.IsDeleted = true;
                        trailerRentalContext.Entry(objTrailerRentalDetail).State = EntityState.Modified;
                    }

                }
                return trailerRentalContext.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

    }
}
