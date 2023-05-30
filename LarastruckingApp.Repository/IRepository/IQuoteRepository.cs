using LarastruckingApp.Entities;
using LarastruckingApp.Entities.QuoteDto;
using LarastruckingApp.Entities.QuotesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IQuoteRepository : IRepository<QuoteDTO>
    {
        List<CommodityDTO> GetCommodityList();
        List<PricingMethodDTO> GetPricingMethodList();
        List<FreightTypeDTO> GetFreightTypeList();
        BaseFreightDto GetBaseFreightDetail(BaseFreightDto entity);
        List<ViewQuoteDTO> GetQuoteDetail(SearchQuoteDTO model);
        GetQuoteDTO GetQuoteById(int quoteId);
        GetQuoteDTO SendQuote(int quoteId);
        
        #region Get max route no. 
        /// <summary>
        /// Get max route no.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>

        int? GetMaxRouteNo(int quoteId);
        #endregion

    }
}
