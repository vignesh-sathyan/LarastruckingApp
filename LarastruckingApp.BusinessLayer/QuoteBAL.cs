using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.QuoteDto;
using LarastruckingApp.Entities.QuotesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class QuoteBAL : IQuoteBAL
    {
        #region private member
        private readonly IQuoteDAL quoteDLL = null;
        #endregion

        #region constructor
        public QuoteBAL(IQuoteDAL iQuoteDAL)
        {
            quoteDLL = iQuoteDAL;
        }
        #endregion

        #region List
        /// <summary>
        /// Method to get list
        /// </summary>
        public IEnumerable<QuoteDTO> List => throw new NotImplementedException();
        #endregion

        #region Create Quote
        /// <summary>
        ///  method for save Quote detail into database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public QuoteDTO Add(QuoteDTO entity)
        {
            entity.QuoteDate = Configurations.ConvertLocalToUTC(entity.QuoteDate);
            entity.ValidUptoDate = Configurations.ConvertLocalToUTC(entity.ValidUptoDate);
            if (entity.RouteStops.Count() > 0)
                foreach (var routestop in entity.RouteStops)
                {
                    routestop.PickDateTime = routestop.PickDateTime == null ? routestop.PickDateTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(routestop.PickDateTime));
                    routestop.DeliveryDateTime = routestop.DeliveryDateTime == null ? routestop.DeliveryDateTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(routestop.DeliveryDateTime));
                }
            return quoteDLL.Add(entity);
        }
        #endregion

        #region Delete
        /// <summary>
        ///  Method to delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(QuoteDTO entity)
        {
            return quoteDLL.Delete(entity);
        }
        #endregion

        #region FindById
        /// <summary>
        ///  Method to get quotes by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public QuoteDTO FindById(int Id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        /// <summary>
        ///  Method to update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public QuoteDTO Update(QuoteDTO entity)
        {
            entity.QuoteDate = Configurations.ConvertLocalToUTC(entity.QuoteDate);
            entity.ValidUptoDate = Configurations.ConvertLocalToUTC(entity.ValidUptoDate);
            if (entity.RouteStops.Count > 0)
            {
                foreach (var routestop in entity.RouteStops)
                {
                    routestop.PickDateTime = routestop.PickDateTime == null ? routestop.PickDateTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(routestop.PickDateTime));
                    routestop.DeliveryDateTime = routestop.DeliveryDateTime == null ? routestop.DeliveryDateTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(routestop.DeliveryDateTime));
                }
            }
            return quoteDLL.Update(entity);
        }
        #endregion

        #region get commodity list
        /// <summary>
        /// get commodity list
        /// </summary>
        /// <returns></returns>
        public List<CommodityDTO> GetCommodityList()
        {
            return quoteDLL.GetCommodityList();
        }
        #endregion

        #region get freight type list
        /// <summary>
        /// get freight type list
        /// </summary>
        /// <returns></returns>
        public List<FreightTypeDTO> GetFreightTypeList()
        {
            return quoteDLL.GetFreightTypeList();
        }
        #endregion

        #region get pricing method list
        /// <summary>
        ///get pricing method list
        /// </summary>
        /// <returns></returns>
        public List<PricingMethodDTO> GetPricingMethodList()
        {
            return quoteDLL.GetPricingMethodList();
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
            return quoteDLL.GetBaseFreightDetail(entity);
        }
        #endregion

        #region Get Quote Detail
        /// <summary>
        /// method for get quote detail list
        /// </summary>
        /// <returns></returns>
        public List<ViewQuoteDTO> GetQuoteDetail(SearchQuoteDTO model)
        {
            if (model.QuoteDate != null)
            {
                model.QuoteDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.QuoteDate));
            }
            if (model.ValidUptoDate != null)
            {
                model.ValidUptoDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(model.ValidUptoDate));
            }



            var quoteDetail = quoteDLL.GetQuoteDetail(model);
            foreach (var data in quoteDetail.ToList())
            {
                data.QuoteDate = Configurations.ConvertDateTime(data.QuoteDate);
                data.ValidUptoDate = Configurations.ConvertDateTime(data.ValidUptoDate);
                data.QuoteStatus = Enum.GetName(typeof(QuoteStatus), data.QuoteStatusId);
            }
            return quoteDetail;
        }
        #endregion

        #region GetQuoteById
        /// <summary>
        /// get quote detail by id
        /// </summary>
        /// <returns></returns>
        public GetQuoteDTO GetQuoteById(int quoteId)
        {

            var data = quoteDLL.GetQuoteById(quoteId);
            data.QuoteStatus = Enum.GetName(typeof(QuoteStatus), data.QuoteStatusId);
            return data;
        }


        #endregion
        #region sendQuote
        /// <summary>
        /// Send Quote detail by id
        /// </summary>
        /// <returns></returns>
        public GetQuoteDTO SendQuote(int quoteId)
        {
            return quoteDLL.SendQuote(quoteId);
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
            return quoteDLL.GetMaxRouteNo(quoteId);
        }
        #endregion
    }
}
