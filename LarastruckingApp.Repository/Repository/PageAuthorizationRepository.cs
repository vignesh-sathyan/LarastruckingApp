using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class PageAuthorizationRepository : IPageAuthorizationRepository
    {
        #region Private Member 
        /// <summary>
        /// defining Private members
        /// </summary>
        public  LarastruckingDBEntities authorizationContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of Page Authorization Repository
        /// </summary>
        public PageAuthorizationRepository()
        {
            authorizationContext = new LarastruckingDBEntities();
        }
        #endregion

        #region List
        /// <summary>
        /// List Of Page Authorization 
        /// </summary>
        public IEnumerable<PageAuthorizationDTO> List
        {
            get
            {
                try
                {
                    IEnumerable<PageAuthorizationDTO> lstPageAuthorizationDTO = AutoMapperServices<tblPageAuthorization, PageAuthorizationDTO>.ReturnObjectList(authorizationContext.tblPageAuthorizations.ToList());
                    return lstPageAuthorizationDTO;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion

        #region Add
        /// <summary>
        /// Add Data Of Page Authorization
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PageAuthorizationDTO Add(PageAuthorizationDTO entity)
        {
            try
            {

                PageAuthorizationDTO objAuthorizationDTO = new PageAuthorizationDTO();
                tblPageAuthorization objtblPageAuthorization = AutoMapperServices<PageAuthorizationDTO, tblPageAuthorization>.ReturnObject(entity);
                authorizationContext.tblPageAuthorizations.Add(objtblPageAuthorization);
                objAuthorizationDTO = AutoMapperServices<tblPageAuthorization, PageAuthorizationDTO>.ReturnObject(objtblPageAuthorization);
                objAuthorizationDTO.IsSuccess = authorizationContext.SaveChanges() > 0 ? true : false;
                return objAuthorizationDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Page Authorization
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(PageAuthorizationDTO entity)
        {
            try
            {
                bool result = false;
                var table = authorizationContext.tblPageAuthorizations.Find(entity);
                if (table != null)
                {
                    table = authorizationContext.tblPageAuthorizations.Remove(table);
                    result = table != null ? true : false;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Page Authorization
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PageAuthorizationDTO Update(PageAuthorizationDTO entity)
        {
            try
            {


                PageAuthorizationDTO objPageAuthorizationDTO = new PageAuthorizationDTO();
                tblPageAuthorization objtblPageAuthorization = AutoMapperServices<PageAuthorizationDTO, tblPageAuthorization>.ReturnObject(entity);
                authorizationContext.Entry(objtblPageAuthorization).State = System.Data.Entity.EntityState.Modified;
                objPageAuthorizationDTO = AutoMapperServices<tblPageAuthorization, PageAuthorizationDTO>.ReturnObject(objtblPageAuthorization);
                objPageAuthorizationDTO.IsSuccess = authorizationContext.SaveChanges() > 0 ? true : false;
                return objPageAuthorizationDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Find By Id
        /// <summary>
        /// Page Authorization Find By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PageAuthorizationDTO FindById(int Id)
        {
            try
            {
                PageAuthorizationDTO objPageAuthorizationDTO = null;
                var result = (from r in authorizationContext.tblPageAuthorizations where r.Id == Id select r).FirstOrDefault();
                if (result != null)
                {
                    objPageAuthorizationDTO = AutoMapperServices<tblPageAuthorization, PageAuthorizationDTO>.ReturnObject(result);
                    objPageAuthorizationDTO.IsSuccess = true;
                }
                return objPageAuthorizationDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Insert Update PageAuthorization 
        /// <summary>
        /// Insert Update Page Authorization
        /// </summary>
        /// <param name="objPageAuthorizationDTO"></param>
        /// <returns></returns>
        public int InsertUpdatePageAuthorization(PageAuthorizationDTO objPageAuthorizationDTO)
        {
            try
            {


                return authorizationContext.usp_InsertUpdatePageAuthorization(objPageAuthorizationDTO.RoleId, objPageAuthorizationDTO.PageId, Convert.ToInt16(objPageAuthorizationDTO.CanView), Convert.ToInt16(objPageAuthorizationDTO.CanInsert), Convert.ToInt16(objPageAuthorizationDTO.CanUpdate), Convert.ToInt16(objPageAuthorizationDTO.CanDelete), Convert.ToInt16(objPageAuthorizationDTO.IsPricingMethod));
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Page Authorization
        /// <summary>
        /// Get Page Authorization
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<PageAuthorizationDTO> GetPageAuthorization(int roleId)
        {
            try
            {
                List<PageAuthorizationDTO> lstPageAuthorizationDTO = new List<PageAuthorizationDTO>();
                lstPageAuthorizationDTO = (from page in authorizationContext.usp_GetPageAuthorizations(roleId)
                                           select new PageAuthorizationDTO()
                                           {
                                               CanDelete = (bool)page.CanDelete,
                                               CanUpdate = (bool)page.CanUpdate,
                                               CanInsert = (bool)page.CanInsert,
                                               CanView = (bool)page.CanView,
                                               IsPricingMethod = (bool)page.IsPricingMethod,
                                               PageName = page.PageName,
                                               RoleId = (int)page.RoleId,
                                               PageId = page.PageId ?? 0
                                           }
                                                    ).ToList();
                return lstPageAuthorizationDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
