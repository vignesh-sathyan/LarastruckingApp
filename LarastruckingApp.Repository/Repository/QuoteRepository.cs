using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.QuoteDto;
using LarastruckingApp.Entities.QuotesDto;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class QuoteRepository : IQuoteRepository
    {

        #region Private Members
        private readonly ExecuteSQLStoredProceduce sp_dbContext = null;
        private readonly LarastruckingDBEntities quoteReportContext = null;
        #endregion

        #region Contructor
        /// <summary>
        /// constructor for initialize privet variable 
        /// </summary>
        public QuoteRepository()
        {
            quoteReportContext = new ExecuteSQLStoredProceduce();
            sp_dbContext = new ExecuteSQLStoredProceduce();
        }
        #endregion

        /// <summary>
        /// get quote list
        /// </summary>
        public IEnumerable<QuoteDTO> List => throw new NotImplementedException();

        #region create quote
        /// <summary>
        ///  Create quote
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public QuoteDTO Add(QuoteDTO entity)
        {

            QuoteDTO objQuoteDTO = new QuoteDTO();
            try
            {
                var dbExists = false;
                if (entity.CustomerId == 0)
                {
                    dbExists = quoteReportContext.tblCustomerRegistrations.Any(c => c.CustomerName.Equals(entity.CustomerName, StringComparison.InvariantCultureIgnoreCase) && c.IsDeleted == false);
                }

                if (!dbExists)
                {
                    var routStops = entity.RouteStops == null ? null : SqlUtil.ToDataTable(entity.RouteStops);
                    var customerBaseFreightDetails = entity.ShipmentDetail == null ? null : SqlUtil.ToDataTable(entity.ShipmentDetail);
                    var accessorialCharges = entity.QuoteAccessorialPrice == null ? null : SqlUtil.ToDataTable(entity.QuoteAccessorialPrice);


                    List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SPType", SpType.Insert),
                        new SqlParameter("@CustomerId", entity.CustomerId),
                        new SqlParameter("@CustomerName", entity.CustomerName),
                        new SqlParameter("@Email", entity.Email),
                        new SqlParameter("@Phone", entity.Phone),
                        new SqlParameter("@QuotesName",entity.QuotesName),
                        new SqlParameter("@QuoteDate",entity.QuoteDate),
                        new SqlParameter("@ValidUptoDate", entity.ValidUptoDate),
                        new SqlParameter("@FinalTotalAmount", entity.FinalTotalAmount),
                        new SqlParameter("@QuoteStatusId", entity.QuoteStatusId),
                        new SqlParameter("@QuoteRouteStopsDetail",routStops),
                        new SqlParameter("@CustomerBaseFreightDetails",customerBaseFreightDetails),
                        new SqlParameter("@AccessorialPrice",accessorialCharges),
                        new SqlParameter("@CreatedBy", entity.CreatedBy),
                     };

                    var result = sp_dbContext.ExecuteStoredProcedure<SpResponseDTO>("usp_CreateQuote", sqlParameters);

                    var response = result.Count > 0 ? result[0] : null;

                    objQuoteDTO.IsSuccess = (response.ResponseText == Configurations.Insert) ? true : false;
                }
                else
                {
                    objQuoteDTO.IsSuccess = false;
                    objQuoteDTO.Response = "exists";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return objQuoteDTO;

        }
        #endregion

        #region Delete
        /// <summary>
        /// delete quote detail 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(QuoteDTO entity)
        {
            try
            {


                bool result = false;
                var table = quoteReportContext.tblQuotes.Find(entity.QuoteId);
                if (table != null)
                {
                    table.IsDeleted = true;
                    quoteReportContext.Entry(table).State = System.Data.Entity.EntityState.Modified;
                    result = quoteReportContext.SaveChanges() > 0 ? true : false;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        /// <summary>
        /// method for getquote by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public QuoteDTO FindById(int Id)
        {
            try
            {


                throw new NotImplementedException();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region get commodity list
        /// <summary>
        /// get commodity list
        /// </summary>
        /// <returns></returns>
        public List<CommodityDTO> GetCommodityList()
        {
            try
            {
                var commoditylist = quoteReportContext.tblCommodities;
                return AutoMapperServices<tblCommodity, CommodityDTO>.ReturnObjectList(commoditylist.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region get freight type list
        /// <summary>
        /// get freight type list
        /// </summary>
        /// <returns></returns>
        public List<FreightTypeDTO> GetFreightTypeList()
        {
            try
            {
                var freighttypelist = quoteReportContext.tblFreightTypes.Where(x => x.IsActive && x.IsDeleted == false);
                return AutoMapperServices<tblFreightType, FreightTypeDTO>.ReturnObjectList(freighttypelist.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region get pricing method list
        /// <summary>
        ///get pricing method list
        /// </summary>
        /// <returns></returns>
        public List<PricingMethodDTO> GetPricingMethodList()
        {
            try
            {
                var pricingmethodlist = quoteReportContext.tblPricingMethods.Where(x => x.IsActive && x.IsDeleted == false);
                return AutoMapperServices<tblPricingMethod, PricingMethodDTO>.ReturnObjectList(pricingmethodlist.Where(x => x.IsActive).ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region get base freight detail
        /// <summary>
        /// method for get base freight detail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public BaseFreightDto GetBaseFreightDetail(BaseFreightDto entity)
        {
            try
            {


                var baseFreightDetail = (from BFS in quoteReportContext.tblBaseFreightDetails
                                         where BFS.PickupLocationId == entity.PickupLocationId
                                               && BFS.DeliveryLocationId == entity.DeliveryLocationId
                                               && BFS.FreightTypeId == entity.FreightTypeId
                                               && BFS.PricingMethodId == entity.PricingMethodId
                                         select new BaseFreightDto
                                         {
                                             MinFee = BFS.MinFee,
                                             UpTo = BFS.Upto,
                                             UnitPrice = BFS.UnitPrice
                                         }
                                       ).FirstOrDefault();
                return baseFreightDetail;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get Quote Detail
        /// <summary>
        /// method for get quote detail list
        /// </summary>
        /// <returns></returns>
        public List<ViewQuoteDTO> GetQuoteDetail(SearchQuoteDTO model)
        {
            try
            {


                var quoteDetail = (from q in quoteReportContext.tblQuotes
                                   join c in quoteReportContext.tblCustomerRegistrations on q.CustomerId equals c.CustomerID
                                   join usr in quoteReportContext.tblUsers on q.CreatedBy equals usr.Userid
                                   where q.IsDeleted == false && ((model.CustomerId > 0 && model.CustomerId != null) ? q.CustomerId == model.CustomerId : 1 == 1) && (model.QuoteDate != null ? DbFunctions.TruncateTime(q.QuoteDate) >= DbFunctions.TruncateTime(model.QuoteDate) : 1 == 1) && (model.ValidUptoDate != null ? DbFunctions.TruncateTime(q.ValidUptoDate) >= DbFunctions.TruncateTime(model.ValidUptoDate) : 1 == 1)
                                   select new ViewQuoteDTO
                                   {
                                       QuoteId = q.QuoteId,
                                       QuotesName = q.QuotesName,
                                       CustomerId = c.CustomerID,
                                       CustomerName = c.CustomerName,
                                       QuoteDate = q.QuoteDate,
                                       ValidUptoDate = q.ValidUptoDate,
                                       QuoteStatusId = q.QuoteStatusId,
                                       CreatedBy=((usr.FirstName??string.Empty) +" "+(usr.LastName??string.Empty))
                                   }
                                 ).OrderByDescending(x => x.QuoteId);

                return quoteDetail.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetQuoteById
        /// <summary>
        /// get quote detail by id
        /// </summary>
        /// <returns></returns>
        public GetQuoteDTO GetQuoteById(int quoteId)
        {
            try
            {

                var quoteData = (from quote in quoteReportContext.tblQuotes
                                 join customer in quoteReportContext.tblCustomerRegistrations on quote.CustomerId equals customer.CustomerID
                                 where quote.QuoteId == quoteId
                                 select new GetQuoteDTO
                                 {
                                     ContactInfoCount = quoteReportContext.tblCustomerContacts.Where(x => x.CustomerId == quote.CustomerId).Count(),
                                     QuoteId = quote.QuoteId,
                                     QuotesName = quote.QuotesName,
                                     QuoteDate = quote.QuoteDate,
                                     ValidUptoDate = quote.ValidUptoDate,
                                     FinalTotalAmount = quote.FinalTotalAmount,
                                     QuoteStatusId = quote.QuoteStatusId,
                                     CustomerId = customer.CustomerID,
                                     CustomerName = customer.CustomerName,
                                     TermAndCondition = quoteReportContext.tblTermAndConditions.Select(x => x.TermAndCondition).FirstOrDefault(),
                                     Email = quoteReportContext.tblBaseAddresses.Where(x => x.CustomerId == customer.CustomerID).Select(x => x.BillingEmail).FirstOrDefault(),
                                     Phone = quoteReportContext.tblBaseAddresses.Where(x => x.CustomerId == customer.CustomerID).Select(x => x.BillingPhoneNumber).FirstOrDefault(),
                                     RouteStops = (from routestops in quoteReportContext.tblQuoteRouteStops
                                                   where routestops.QuoteId == quote.QuoteId
                                                   select new GetRouteStopsDTO
                                                   {
                                                       QuoteId = routestops.QuoteId,
                                                       RouteStopId = routestops.QuoteRouteStopsId,
                                                       RouteNo = routestops.RouteNo,
                                                       PickupLocationId = routestops.PickupLocationId,

                                                       PickupLocation = (from address in quoteReportContext.tblAddresses
                                                                         join state in quoteReportContext.tblStates on address.State equals state.ID
                                                                         where address.AddressId == routestops.PickupLocationId
                                                                         select new { PicUpLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + " " + address.City + " " + state.Name.ToUpper() + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault(),
                                                       PickDateTime = routestops.PickDateTime,
                                                       DeliveryLocationId = routestops.DeliveryLocationId,
                                                       DeliveryLocation = (from address in quoteReportContext.tblAddresses
                                                                           join state in quoteReportContext.tblStates on address.State equals state.ID
                                                                           where address.AddressId == routestops.DeliveryLocationId
                                                                           select new { PicUpLocation = (address.CompanyName == null ? string.Empty : (address.CompanyName + ",")) + address.Address1 + " " + address.City + " " + state.Name.ToUpper() + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault(),
                                                       DeliveryDateTime = routestops.DeliveryDateTime,

                                                   }
                                                 ).OrderBy(x => x.RouteStopId).ToList(),
                                     ShipmentDetail = (from cbfd in quoteReportContext.tblCustomerBaseFreightDetails
                                                       where cbfd.QuoteId == quote.QuoteId
                                                       select new GetFreightDetailDTO
                                                       {
                                                           FreightDetailId = cbfd.CustomerBaseFreightDetailId,
                                                           QuoteId = cbfd.QuoteId,
                                                           RouteNo = cbfd.RouteNo,
                                                           QuoteRouteStopsId = cbfd.QuoteRouteStopsId,
                                                           PickupLocationId = cbfd.PickupLocationId,
                                                           DeliveryLocationId = cbfd.DeliveryLocationId,
                                                           Commodity = cbfd.Commodity,
                                                           FreightTypeId = cbfd.FreightTypeId,
                                                           FreightType = (from freight in quoteReportContext.tblFreightTypes where freight.FreightTypeId == cbfd.FreightTypeId select freight.FreightTypeName).FirstOrDefault(),
                                                           PricingMethodId = cbfd.PricingMethodId,
                                                           PricingMethod = (from pricingmethod in quoteReportContext.tblPricingMethods where pricingmethod.PricingMethodId == cbfd.PricingMethodId select pricingmethod.PricingMethodName).FirstOrDefault(),
                                                           MinFee = cbfd.MinFee,
                                                           UpTo = cbfd.Upto,
                                                           UnitPrice = cbfd.UnitPrice,
                                                           Hazardous = cbfd.Hazardous,
                                                           Temperature = cbfd.Temperature,
                                                           QutVlmWgt = cbfd.QutWgtVlm ?? 0,
                                                           TotalPrice = cbfd.TotalPrice ?? 0,
                                                           NoOfBox = cbfd.NoOfBox ?? 0,
                                                           Weight = cbfd.Weight ?? 0,
                                                           Unit = cbfd.Unit ?? string.Empty,
                                                           TrailerCount = cbfd.TrailerCount ?? 0,
                                                       }
                                                     ).OrderBy(x => x.FreightDetailId).ToList(),

                                     AccessorialPrice = (from accessorialprice in quoteReportContext.tblQuoteAccessorialPrices
                                                         join quouteRoutesStops in quoteReportContext.tblQuoteRouteStops on accessorialprice.QuoteRouteStopsId equals quouteRoutesStops.QuoteRouteStopsId
                                                         where accessorialprice.QuoteId == quoteId && accessorialprice.IsDeleted == false
                                                         select new GetQuoteAccessorialPriceDTO
                                                         {
                                                             QuoteAccessorialPriceId = accessorialprice.QuoteAccessorialPriceId,
                                                             RouteNo = quouteRoutesStops.RouteNo,
                                                             AccessorialFeeTypeId = accessorialprice.AccessorialFeeTypeId,
                                                             AccessorialFeeType = (from aft in quoteReportContext.tblAccessorialFeesTypes where aft.Id == accessorialprice.AccessorialFeeTypeId select aft.Name).FirstOrDefault(),
                                                             Unit = accessorialprice.Unit ?? 0,
                                                             AmtPerUnit = accessorialprice.AmtPerUnit ?? 0,
                                                             Amount = accessorialprice.Amount ?? 0,
                                                             FeeType = (from aft in quoteReportContext.tblAccessorialFeesTypes where aft.Id == accessorialprice.AccessorialFeeTypeId select aft.PricingMethod).FirstOrDefault(),
                                                             Reason= accessorialprice.Reason??string.Empty
                                                         }
                                                            ).ToList(),
                                     SendQuoteAccessorialPrice = (from masteraccessorialprice in quoteReportContext.tblAccessorialFeesTypes
                                                                  where masteraccessorialprice.IsActive
                                                                  select new SendQuoteAccessorialPriceDTO
                                                                  {
                                                                      Id = masteraccessorialprice.Id,
                                                                      Name = masteraccessorialprice.Name,
                                                                      PricingMethod = masteraccessorialprice.PricingMethod,
                                                                      DisplayOrder = masteraccessorialprice.DisplayOrder
                                                                  }

                                                                ).ToList()

                                 }
                               ).FirstOrDefault();
                if (quoteData != null)
                {
                    quoteData.QuoteStatus = Enum.GetName(typeof(QuoteStatus), quoteData.QuoteStatusId);

                    quoteData.QuoteDate = Configurations.ConvertDateTime(Convert.ToDateTime(quoteData.QuoteDate));
                    quoteData.ValidUptoDate = Configurations.ConvertDateTime(Convert.ToDateTime(quoteData.ValidUptoDate));
                    quoteData.FinalTotalAmountWithoutAcc = quoteData.ShipmentDetail.Select(X => X.TotalPrice).Sum();
                    quoteData.LogoUrl = LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingApp.Entities.Common.Configurations.LarasLogo;

                    foreach (var data in quoteData.RouteStops)
                    {
                        //var date = Convert.ToDateTime(data.PickDateTime).ToString("");
                        data.PickDateTime = data.PickDateTime == null ? data.PickDateTime : Configurations.ConvertDateTime(Convert.ToDateTime(data.PickDateTime));
                        data.DeliveryDateTime = data.DeliveryDateTime == null ? data.DeliveryDateTime : Configurations.ConvertDateTime(Convert.ToDateTime(data.DeliveryDateTime));
                    }

                    foreach (var pricingMethodDetail in quoteData.ShipmentDetail)
                    {
                        string pricingMethodUnit = string.Empty;
                        if (pricingMethodDetail.QutVlmWgt > 0)
                        {
                            pricingMethodUnit = pricingMethodDetail.QutVlmWgt.ToString().Replace(".00", "") + " " + PricingMethod.PLTS;
                        }
                        if (pricingMethodDetail.NoOfBox > 0)
                        {
                            pricingMethodUnit += ", " + pricingMethodDetail.NoOfBox.ToString().Replace(".00", "") + " " + PricingMethod.BXS;
                        }
                        if (pricingMethodDetail.Weight > 0)
                        {
                            pricingMethodUnit += ", " + pricingMethodDetail.Weight.ToString().Replace(".00", "") + " " + pricingMethodDetail.Unit;
                        }
                        if (pricingMethodDetail.TrailerCount > 0)
                        {
                            pricingMethodUnit += ", " + pricingMethodDetail.TrailerCount.ToString().Replace(".00", "") + " " + PricingMethod.Trailer;
                        }
                        pricingMethodDetail.AllQuantityUnit = pricingMethodUnit.Trim(',');
                    }
                }
                return quoteData;

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region sendQuote
        /// <summary>
        /// Send Quote detail by id
        /// </summary>
        /// <returns></returns>
        public GetQuoteDTO SendQuote(int quoteId)
        {
            try
            {


                var quoteData = (from quote in quoteReportContext.tblQuotes
                                 join customer in quoteReportContext.tblCustomerRegistrations on quote.CustomerId equals customer.CustomerID
                                 where quote.QuoteId == quoteId
                                 select new GetQuoteDTO
                                 {
                                     QuoteId = quote.QuoteId,
                                     QuotesName = quote.QuotesName,
                                     QuoteDate = quote.QuoteDate,
                                     ValidUptoDate = quote.ValidUptoDate,
                                     FinalTotalAmount = quote.FinalTotalAmount,
                                     QuoteStatusId = quote.QuoteStatusId,
                                     CustomerId = customer.CustomerID,
                                     CustomerName = customer.CustomerName,
                                     ContactPersons = (from contactPerson in quoteReportContext.tblCustomerContacts
                                                       where contactPerson.CustomerId == customer.CustomerID
                                                       select new CustomerContact
                                                       {
                                                           ContactEmail = contactPerson.ContactEmail,
                                                       }
                                                         ).ToList(),
                                     // CustomerContact
                                     CustomerAddress = (from BA in quoteReportContext.tblBaseAddresses
                                                        join con in quoteReportContext.tblCountries on BA.BillingCountryId equals con.ID
                                                        join sta in quoteReportContext.tblStates on BA.BillingStateId equals sta.ID
                                                        where BA.CustomerId == quote.CustomerId
                                                        select new
                                                        {
                                                            address = (BA.BillingAddress1.Trim() + "<br/>" + BA.BillingCity.Trim() + ", " + sta.Name + " " + BA.BillingZipCode)
                                                        }
                                                      ).Select(x => x.address).FirstOrDefault(),
                                     TermAndCondition = quoteReportContext.tblTermAndConditions.Select(x => x.TermAndCondition).FirstOrDefault(),
                                     Email = quoteReportContext.tblBaseAddresses.Where(x => x.CustomerId == customer.CustomerID).Select(x => x.BillingEmail).FirstOrDefault(),
                                     Phone = quoteReportContext.tblBaseAddresses.Where(x => x.CustomerId == customer.CustomerID).Select(x => x.BillingPhoneNumber).FirstOrDefault(),
                                     RouteStops = (from routestops in quoteReportContext.tblQuoteRouteStops
                                                   where routestops.QuoteId == quote.QuoteId
                                                   select new GetRouteStopsDTO
                                                   {
                                                       QuoteId = routestops.QuoteId,
                                                       RouteStopId = routestops.QuoteRouteStopsId,
                                                       RouteNo = routestops.RouteNo,
                                                       PickupLocationId = routestops.PickupLocationId,

                                                       PickupLocation = (from address in quoteReportContext.tblAddresses
                                                                         join state in quoteReportContext.tblStates on address.State equals state.ID
                                                                         where address.AddressId == routestops.PickupLocationId
                                                                         select new { PicUpLocation = (address.CompanyName == null ? string.Empty : address.CompanyName.Trim()) + "<br/>" + address.Address1 + "<br/>" + address.City + ", " + state.Name + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault(),
                                                       PickDateTime = routestops.PickDateTime,
                                                       DeliveryLocationId = routestops.DeliveryLocationId,
                                                       DeliveryLocation = (from address in quoteReportContext.tblAddresses
                                                                           join state in quoteReportContext.tblStates on address.State equals state.ID
                                                                           where address.AddressId == routestops.DeliveryLocationId
                                                                           select new { PicUpLocation = (address.CompanyName == null ? string.Empty : address.CompanyName.Trim()) + "<br/>" + address.Address1 + "<br/>" + address.City + ", " + state.Name + " " + address.Zip }).Select(x => x.PicUpLocation).FirstOrDefault(),
                                                       DeliveryDateTime = routestops.DeliveryDateTime,

                                                   }
                                                 ).OrderBy(x => x.RouteStopId).ToList(),
                                     ShipmentDetail = (from cbfd in quoteReportContext.tblCustomerBaseFreightDetails
                                                       where cbfd.QuoteId == quote.QuoteId
                                                       select new GetFreightDetailDTO
                                                       {
                                                           FreightDetailId = cbfd.CustomerBaseFreightDetailId,
                                                           QuoteId = cbfd.QuoteId,
                                                           RouteNo = cbfd.RouteNo,
                                                           QuoteRouteStopsId = cbfd.QuoteRouteStopsId,
                                                           PickupLocationId = cbfd.PickupLocationId,
                                                           DeliveryLocationId = cbfd.DeliveryLocationId,
                                                           Commodity = cbfd.Commodity,
                                                           FreightTypeId = cbfd.FreightTypeId,
                                                           FreightType = (from freight in quoteReportContext.tblFreightTypes where freight.FreightTypeId == cbfd.FreightTypeId select freight.FreightTypeName).FirstOrDefault(),
                                                           PricingMethodId = cbfd.PricingMethodId,
                                                           PricingMethod = (from pricingmethod in quoteReportContext.tblPricingMethods where pricingmethod.PricingMethodId == cbfd.PricingMethodId select pricingmethod.PricingMethodName).FirstOrDefault(),
                                                           MinFee = cbfd.MinFee,
                                                           UpTo = cbfd.Upto,
                                                           UnitPrice = cbfd.UnitPrice,
                                                           Hazardous = cbfd.Hazardous,
                                                           Temperature = cbfd.Temperature,
                                                           QutVlmWgt = cbfd.QutWgtVlm ?? 0,
                                                           TotalPrice = cbfd.TotalPrice ?? 0,
                                                           NoOfBox = cbfd.NoOfBox ?? 0,
                                                           Weight = cbfd.Weight ?? 0,
                                                           Unit = cbfd.Unit ?? string.Empty,
                                                           TrailerCount = cbfd.TrailerCount ?? 0,
                                                       }
                                                     ).OrderBy(x => x.FreightDetailId).ToList(),

                                     AccessorialPrice = (from accessorialprice in quoteReportContext.tblQuoteAccessorialPrices
                                                         join quouteRoutesStops in quoteReportContext.tblQuoteRouteStops on accessorialprice.QuoteRouteStopsId equals quouteRoutesStops.QuoteRouteStopsId
                                                         where accessorialprice.QuoteId == quoteId && accessorialprice.IsDeleted == false
                                                         select new GetQuoteAccessorialPriceDTO
                                                         {
                                                             QuoteAccessorialPriceId = accessorialprice.QuoteAccessorialPriceId,
                                                             RouteNo = quouteRoutesStops.RouteNo,
                                                             AccessorialFeeTypeId = accessorialprice.AccessorialFeeTypeId,
                                                             AccessorialFeeType = (from aft in quoteReportContext.tblAccessorialFeesTypes where aft.Id == accessorialprice.AccessorialFeeTypeId select aft.Name).FirstOrDefault(),
                                                             Unit = accessorialprice.Unit ?? 0,
                                                             AmtPerUnit = accessorialprice.AmtPerUnit ?? 0,
                                                             Amount = accessorialprice.Amount ?? 0,
                                                             FeeType = (from aft in quoteReportContext.tblAccessorialFeesTypes where aft.Id == accessorialprice.AccessorialFeeTypeId select aft.PricingMethod).FirstOrDefault(),
                                                         }
                                                            ).ToList(),
                                     SendQuoteAccessorialPrice = (from masteraccessorialprice in quoteReportContext.tblAccessorialFeesTypes
                                                                  where masteraccessorialprice.IsActive
                                                                  select new SendQuoteAccessorialPriceDTO
                                                                  {
                                                                      Id = masteraccessorialprice.Id,
                                                                      Name = masteraccessorialprice.Name,
                                                                      PricingMethod = masteraccessorialprice.PricingMethod,
                                                                      DisplayOrder = masteraccessorialprice.DisplayOrder
                                                                  }

                                                                ).ToList()

                                 }
                               ).FirstOrDefault();
                if (quoteData != null)
                {
                    if (quoteData.ContactPersons.Count > 0)
                    {
                        quoteData.AllContactPerson = string.Join(",", quoteData.ContactPersons.Select(x => x.ContactEmail).ToList());
                    }
                    else
                    {
                        quoteData.AllContactPerson = quoteData.Email;
                    }
                    quoteData.QuoteStatus = Enum.GetName(typeof(QuoteStatus), quoteData.QuoteStatusId);
                    quoteData.QuoteDate = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(quoteData.QuoteDate));
                    quoteData.ValidUptoDate = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(quoteData.ValidUptoDate));
                    quoteData.FinalTotalAmountWithoutAcc = quoteData.ShipmentDetail.Select(X => X.TotalPrice).Sum();
                    quoteData.LogoUrl = LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingApp.Entities.Common.Configurations.LarasLogo;
                    foreach (var data in quoteData.RouteStops)
                    {
                        //var date = Convert.ToDateTime(data.PickDateTime).ToString("");
                        data.PickDateTime = data.PickDateTime == null ? data.PickDateTime : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(data.PickDateTime));
                        data.DeliveryDateTime = data.DeliveryDateTime == null ? data.DeliveryDateTime : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(data.DeliveryDateTime));
                    }

                    foreach (var pricingMethodDetail in quoteData.ShipmentDetail)
                    {
                        string pricingMethodUnit = string.Empty;
                        if (pricingMethodDetail.QutVlmWgt > 0)
                        {
                            pricingMethodDetail.TotalPallet = pricingMethodDetail.QutVlmWgt.ToString().Replace(".00", "");
                        }
                        if (pricingMethodDetail.NoOfBox > 0)
                        {
                            pricingMethodDetail.TotalBox = pricingMethodDetail.NoOfBox.ToString().Replace(".00", "");
                        }
                        if (pricingMethodDetail.Weight > 0)
                        {
                            pricingMethodDetail.TotalWeight = pricingMethodDetail.Weight.ToString().Replace(".00", "") + " " + pricingMethodDetail.Unit;
                        }
                        if (pricingMethodDetail.TrailerCount > 0)
                        {
                            pricingMethodUnit += ", " + pricingMethodDetail.TrailerCount.ToString().Replace(".00", "") + " " + PricingMethod.Trailer;
                        }
                        pricingMethodDetail.AllQuantityUnit = pricingMethodUnit.Trim(',');
                    }
                }
                return quoteData;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region update Quote
        /// <summary>
        /// method for update Quote detail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public QuoteDTO Update(QuoteDTO entity)
        {
            try
            {


                QuoteDTO objQuoteDTO = new QuoteDTO();
                try
                {
                    var RoutStops = entity.RouteStops == null ? null : SqlUtil.ToDataTable(entity.RouteStops);
                    var CustomerBaseFreightDetails = entity.ShipmentDetail == null ? null : SqlUtil.ToDataTable(entity.ShipmentDetail);
                    var AccessorialCharges = entity.QuoteAccessorialPrice == null ? null : SqlUtil.ToDataTable(entity.QuoteAccessorialPrice);

                    List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SPType", SpType.Update),
                         new SqlParameter("@QuoteId", entity.QuoteId),
                        new SqlParameter("@CustomerId", entity.CustomerId),
                        new SqlParameter("@CustomerName", entity.CustomerName),
                        new SqlParameter("@Email", entity.Email),
                        new SqlParameter("@Phone", entity.Phone),
                        new SqlParameter("@QuotesName",entity.QuotesName),
                        new SqlParameter("@QuoteDate",entity.QuoteDate),
                        new SqlParameter("@ValidUptoDate", entity.ValidUptoDate),
                        new SqlParameter("@FinalTotalAmount", entity.FinalTotalAmount),
                        new SqlParameter("@QuoteStatusId", entity.QuoteStatusId),
                        new SqlParameter("@QuoteRouteStopsDetail",RoutStops),
                        new SqlParameter("@CustomerBaseFreightDetails",CustomerBaseFreightDetails),
                         new SqlParameter("@AccessorialPrice",AccessorialCharges),
                        new SqlParameter("@CreatedBy", entity.CreatedBy),
                     };

                    var result = sp_dbContext.ExecuteStoredProcedure<SpResponseDTO>("usp_EditQuote", sqlParameters);
                    var response = result.Count > 0 ? result[0] : null;
                    objQuoteDTO.IsSuccess = (response.ResponseText == Configurations.Update) ? true : false;


                }
                catch (Exception)
                {
                    throw;
                }
                return objQuoteDTO;
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

        public int? GetMaxRouteNo(int quoteId)
        {
            int? MaxRouteNo = 0;
            if (quoteId > 0)
            {
                MaxRouteNo = quoteReportContext.tblQuoteRouteStops.Where(x => x.QuoteId == quoteId).Max(x => x.RouteNo);
            }
            return MaxRouteNo;

        }
        #endregion
    }

}
