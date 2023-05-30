using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class DriverBAL : IDriverBAL
    {
        #region Private Member
        /// <summary>
        /// private member
        /// </summary>
        private IDriverDAL iDriverRepo;
        #endregion

        #region DriverBAL
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="iDriverDAL"></param>
        public DriverBAL(IDriverDAL iDriverDAL)
        {
            iDriverRepo = iDriverDAL;
        }
        #endregion

        #region Relationship Status
        public IList<ContactRelationDto> ContactRelation()
        {
            return iDriverRepo.ContactRelation();
        }
        #endregion

        #region Driver List
        /// <summary>
        /// Geting driver records list 
        /// </summary>
        public IEnumerable<DriverListDto> DriverList()
        {
            return iDriverRepo.DriverList();
        }
        #endregion

        #region List
        /// <summary>
        /// Geting Driver list
        /// </summary>
        public IEnumerable<DriverDTO> List
        {
            get
            {
                return iDriverRepo.List;
            }
        }
        #endregion

        #region Add
        /// <summary>
        /// Add Driver
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverDTO Add(DriverDTO entity)
        {
            return iDriverRepo.Add(entity);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Driver
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverDTO Update(DriverDTO entity)
        {
            return iDriverRepo.Update(entity);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Driver
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(DriverDTO entity)
        {
            return iDriverRepo.Delete(entity);
        }
        #endregion

        #region FindById
        /// <summary>
        /// Find Driver by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DriverDTO FindById(int Id)
        {
            return iDriverRepo.FindById(Id);
        }
        #endregion

        #region Driver Info By user id: for Leave Module
        /// <summary>
        /// Find Driver  basic detail for leave module
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DriverDTO GetDriverBasicDetail(int userId)
        {
            return iDriverRepo.GetDriverBasicDetail(userId);
        }
        #endregion


        #region DocumentList
        /// <summary>
        /// Get Document List
        /// </summary>
        /// <returns></returns>
        public List<DocumentNameDTO> DocumentList()
        {
            return iDriverRepo.DocumentList();
        }
        #endregion

        #region GetCountryList
        /// <summary>
        /// Method to Get country
        /// </summary>
        /// <returns></returns>
        public List<CountryDTO> GetCountryList()
        {
            return iDriverRepo.GetCountryList();
        }
        #endregion
        #region GetLanguageList
        /// <summary>
        /// Get language List 
        /// </summary>
        /// <returns></returns>
        public List<LanguageDTO> GetLanguageList()
        {
            return iDriverRepo.GetLanguageList();
        }
        #endregion
        #region GetStateList
        /// <summary>
        /// Method to Get state
        /// </summary>
        /// <returns></returns>
        public List<StateDTO> GetStateList()
        {
            return iDriverRepo.GetStateList();
        }
        #endregion

        #region GetEquipment
        /// <summary>
        /// Method to Get Equipment
        /// </summary>
        /// <returns></returns>
        public List<EquipmentDTO> GetEquipment()
        {
            return iDriverRepo.GetEquipment();
        }
        #endregion

        #region AddDriverDocument
        /// <summary>
        /// Method to add document
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverDocumentDTO AddDriverDocument(DriverDocumentDTO entity)
        {
            return iDriverRepo.AddDriverDocument(entity);
        }
        #endregion

        #region DeleteDocument
        /// <summary>
        /// Method to delete document
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        public bool DeleteDocument(int DriverId)
        {
            return iDriverRepo.DeleteDocument(DriverId);
        }
        #endregion

        #region Driver Document List
        /// <summary>
        /// List documents fo rdriver
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public DriverDetailsDto GetDriverDocuments(int driverId)
        {
            return iDriverRepo.GetDriverDocuments(driverId);
        }
        #endregion

        #region Get Document By DocId
        /// <summary>
        /// Method to get driver document available to download
        /// </summary>
        /// <param name="id"></param>
        public DriverDocumentDto DownloadDocument(int id)
        {
            return iDriverRepo.DownloadDocument(id);
        }
        #endregion

        public bool DriverDocumetsType(int DriverID)
        {
            return iDriverRepo.DriverDocumetsType(DriverID);
        }
  
    }
}
