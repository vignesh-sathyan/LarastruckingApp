using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.QuoteDto;
using LarastruckingApp.Entities.QuotesDto;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class QuoteDAL : IQuoteDAL
    {

        #region private member
        readonly IQuoteRepository quoteRepository = null;
        #endregion

        #region constructor
        public QuoteDAL(IQuoteRepository iQuotepository)
        {
            quoteRepository = iQuotepository;
        }
        #endregion
        public IEnumerable<QuoteDTO> List => throw new NotImplementedException();

        #region Create Quote
        /// <summary>
        ///  method for save Quote detail into database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public QuoteDTO Add(QuoteDTO entity)
        {
            return quoteRepository.Add(entity);
        }
        #endregion

        /// <summary>
        /// method for delete Quote detail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(QuoteDTO entity)
        {
            return quoteRepository.Delete(entity);
        }

        /// <summary>
        /// method for getquote by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public QuoteDTO FindById(int Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// method for update Quote detail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public QuoteDTO Update(QuoteDTO entity)
        {
            return quoteRepository.Update(entity);
        }

        #region get commodity list
        /// <summary>
        /// get commodity list
        /// </summary>
        /// <returns></returns>
        public List<CommodityDTO> GetCommodityList()
        {
            return quoteRepository.GetCommodityList();
        }
        #endregion

        #region get freight type list
        /// <summary>
        /// get freight type list
        /// </summary>
        /// <returns></returns>
        public List<FreightTypeDTO> GetFreightTypeList()
        {
            return quoteRepository.GetFreightTypeList();
        }
        #endregion

        #region get pricing method list
        /// <summary>
        ///get pricing method list
        /// </summary>
        /// <returns></returns>
        public List<PricingMethodDTO> GetPricingMethodList()
        {
            return quoteRepository.GetPricingMethodList();
        }
        #endregion

        #region get base detail
        /// <summary>
        /// method for get base freight detail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public BaseFreightDto GetBaseFreightDetail(BaseFreightDto entity)
        {
            return quoteRepository.GetBaseFreightDetail(entity);
        }
        #endregion

        #region Get Quote Detail
        /// <summary>
        /// method for get quote detail list
        /// </summary>
        /// <returns></returns>
        public List<ViewQuoteDTO> GetQuoteDetail(SearchQuoteDTO model)
        {
            return quoteRepository.GetQuoteDetail(model);
        }
        #endregion

        #region GetQuoteById
        /// <summary>
        /// get quote detail by id
        /// </summary>
        /// <returns></returns>
        public GetQuoteDTO GetQuoteById(int quoteId)
        {
            return quoteRepository.GetQuoteById(quoteId);
        }


        #endregion
        #region sendQuote
        /// <summary>
        /// Send Quote detail by id
        /// </summary>
        /// <returns></returns>
        public GetQuoteDTO SendQuote(int quoteId)
        {
            return quoteRepository.SendQuote(quoteId);
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
            return quoteRepository.GetMaxRouteNo(quoteId);
        }
        #endregion
    }
}
