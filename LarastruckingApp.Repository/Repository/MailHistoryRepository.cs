using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.MailHistory;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class MailHistoryRepository : IMailHistoryRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities MailHistoryContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Defining contrructor
        /// </summary>
        public MailHistoryRepository()
        {
            MailHistoryContext = new LarastruckingDBEntities();
        }
        #endregion

        #region add mail history
        /// <summary>
        /// Add Address
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MailHistoryDTO Add(MailHistoryDTO entity)
        {
            try
            {

                MailHistoryDTO objMailHistoryDTO = new MailHistoryDTO();
                tblMailHistory tblMailHistory = AutoMapperServices<MailHistoryDTO, tblMailHistory>.ReturnObject(entity);
                MailHistoryContext.tblMailHistories.Add(tblMailHistory);
                objMailHistoryDTO.IsSuccess = MailHistoryContext.SaveChanges() > 0 ? true : false;
                objMailHistoryDTO.MailHistoryId = tblMailHistory.MailHistoryId;
                return objMailHistoryDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
