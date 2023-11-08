using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL.Interface
{
  public  interface IDriverDAL:ICommonDAL<DriverDTO>
    {
        IList<ContactRelationDto> ContactRelation();
      
        #region Driver List
        /// <summary>
        /// Geting driver records list 
        /// </summary>
        IEnumerable<DriverListDto> DriverList();
        IEnumerable<DriverListDto> DriverInactiveList(int spType, int isActive);
        #endregion

        #region Get Document By DocId
        /// <summary>
        /// Method to get driver document available to download
        /// </summary>
        /// <param name="id"></param>
        DriverDocumentDto DownloadDocument(int id);
        #endregion

        #region Driver Info By user id: for Leave Module
        /// <summary>
        /// Find Driver  basic detail for leave module
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        DriverDTO GetDriverBasicDetail(int userId);
        #endregion


        List<DocumentNameDTO> DocumentList();
        List<CountryDTO> GetCountryList();
        List<StateDTO> GetStateList();
        List<EquipmentDTO> GetEquipment();
        DriverDocumentDTO AddDriverDocument(DriverDocumentDTO entity);
        bool DeleteDocument(int DriverId);
        List<LanguageDTO> GetLanguageList();
        #region Driver Document List
        DriverDetailsDto GetDriverDocuments(int driverId);
        #endregion

        #region Mandatory Driver Documents
        /// <summary>
        /// Mandatory Driver Documents
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        bool DriverDocumetsType(int DriverID);
        #endregion
    }
}
