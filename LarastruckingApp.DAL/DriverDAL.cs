using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class DriverDAL : IDriverDAL
    {
        private IDriverRepository iDriverRepo;
        public DriverDAL(IDriverRepository iDriverRepository)
        {
            iDriverRepo = iDriverRepository;

        }

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

        #region Driver Inactive List
        /// <summary>
        /// Geting driver records list 
        /// </summary>
        public IEnumerable<DriverListDto> DriverInactiveList(int spType,int isActive)
        {
            return iDriverRepo.DriverInactiveList(spType, isActive);
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
        /// <summary>
        /// Add Driver
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverDTO Add(DriverDTO entity)
        {
            return iDriverRepo.Add(entity);

        }

        public DriverDocumentDTO AddDriverDocument(DriverDocumentDTO entity)
        {
            return iDriverRepo.AddDriverDocument(entity);

        }
        /// <summary>
        /// Delete Driver
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(DriverDTO entity)
        {
            return iDriverRepo.Delete(entity);
        }
        /// <summary>
        /// Find Driver by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DriverDTO FindById(int Id)
        {
            return iDriverRepo.FindById(Id);
        }
        /// <summary>
        ///  Update Driver
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverDTO Update(DriverDTO entity)
        {
            return iDriverRepo.Update(entity);
        }


        public List<DocumentNameDTO> DocumentList()
        {
            return iDriverRepo.DocumentList();
        }

        public List<CountryDTO> GetCountryList()
        {
            return iDriverRepo.GetCountryList();
        }

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

        public List<StateDTO> GetStateList()
        {
            return iDriverRepo.GetStateList();
        }

        public List<EquipmentDTO> GetEquipment()
        {
            return iDriverRepo.GetEquipment();
        }

        public bool DeleteDocument(int DriverId)
        {
            return iDriverRepo.DeleteDocument(DriverId);
        }

        #region Driver Document List
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
