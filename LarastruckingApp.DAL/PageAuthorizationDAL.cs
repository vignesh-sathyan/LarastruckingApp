using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Repository.IRepository;
using System.Collections.Generic;

namespace LarastruckingApp.DAL
{
    /// <summary>
    /// Page Authorization Data Access Layer
    /// </summary>
    public class PageAuthorizationDAL : IPageAuthorizationDAL
    {
        private IPageAuthorizationRepository iPageAuthorizationRepository;
        public PageAuthorizationDAL(IPageAuthorizationRepository iPageAuthorizationRepository)
        {
            this.iPageAuthorizationRepository = iPageAuthorizationRepository;
        }
        /// <summary>
        /// Page Authorization Data List
        /// </summary>
        public IEnumerable<PageAuthorizationDTO> List
        {
            get
            {
                return iPageAuthorizationRepository.List;
            }
        }
        /// <summary>
        /// Add Page Authorization
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PageAuthorizationDTO Add(PageAuthorizationDTO entity)
        {
            return iPageAuthorizationRepository.Add(entity);
        }
        /// <summary>
        /// Delete Page Authorization
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(PageAuthorizationDTO entity)
        {
            return iPageAuthorizationRepository.Delete(entity);
        }
        /// <summary>
        /// Find Page Authorization
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PageAuthorizationDTO FindById(int id)
        {
            return iPageAuthorizationRepository.FindById(id);
        }
        /// <summary>
        /// Update Page Authorization
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PageAuthorizationDTO Update(PageAuthorizationDTO entity)
        {
            return iPageAuthorizationRepository.Update(entity);
        }
        /// <summary>
        /// Insert Update Page Authorization
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertUpdatePageAuthorization(PageAuthorizationDTO entity)
        {
            return iPageAuthorizationRepository.InsertUpdatePageAuthorization(entity);
        }
        /// <summary>
        /// Get Page Authorization
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<PageAuthorizationDTO> GetPageAuthorization(int roleId)
        {
           return iPageAuthorizationRepository.GetPageAuthorization(roleId);
        }
    }
}
