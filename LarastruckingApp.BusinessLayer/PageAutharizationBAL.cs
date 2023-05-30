using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    /// <summary>
    /// Page Authorization Business Layer
    /// </summary>
    public class PageAutharizationBAL : IPageAutharizationBAL
    {
        #region Private Member
        private IPageAuthorizationDAL iPageAuthorizationDAL;
        #endregion

        #region PageAutharizationBAL
        public PageAutharizationBAL(IPageAuthorizationDAL iPageAuthorizationDAL)
        {
            this.iPageAuthorizationDAL = iPageAuthorizationDAL;
        }
        #endregion

        #region List
        /// <summary>
        /// List
        /// </summary>
        public IEnumerable<PageAuthorizationDTO> List
        {
            get
            {
                return iPageAuthorizationDAL.List;
            }
        }
        #endregion

        #region Add
        /// <summary>
        /// Add Page Authorization Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PageAuthorizationDTO Add(PageAuthorizationDTO entity)
        {
            return iPageAuthorizationDAL.Add(entity);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Page Authorization Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(PageAuthorizationDTO entity)
        {
            return iPageAuthorizationDAL.Delete(entity);
        }
        #endregion

        #region FindById
        /// <summary>
        /// Find Page Authorization Data By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PageAuthorizationDTO FindById(int Id)
        {
            return iPageAuthorizationDAL.FindById(Id);
        }
        #endregion

        #region Update
        /// <summary>
        /// Page Authorization Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PageAuthorizationDTO Update(PageAuthorizationDTO entity)
        {
            return iPageAuthorizationDAL.Update(entity);
        }
        #endregion

        #region InsertUpdatePageAuthorization
        /// <summary>
        /// Page Authorization Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertUpdatePageAuthorization(PageAuthorizationDTO entity)
        {
            return iPageAuthorizationDAL.InsertUpdatePageAuthorization(entity);
        }
        #endregion

        #region GetPageAuthorization
        /// <summary>
        /// Get Page Authorization Data By RoleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<PageAuthorizationDTO> GetPageAuthorization(int roleId)
        {
            return iPageAuthorizationDAL.GetPageAuthorization(roleId);
        }
        #endregion
    }
}
