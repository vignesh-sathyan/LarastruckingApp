using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using LarastruckingApp.Resource;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class DriverRepository : IDriverRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities userContext;
        private readonly ExecuteSQLStoredProceduce dbContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public DriverRepository()
        {
            userContext = new LarastruckingDBEntities();
            dbContext = new ExecuteSQLStoredProceduce();
        }
        #endregion

        #region Relationship Status
        /// <summary>
        /// Get relationship status
        /// </summary>
        /// <returns></returns>
        public IList<ContactRelationDto> ContactRelation()
        {
            try
            {


                var contactRelationship = userContext.tblRelationships
                                                        .Select(r => new ContactRelationDto()
                                                        {
                                                            ContactRelationshipId = r.ContactRelationshipId,
                                                            ContactRelationship = r.ContactRelationship
                                                        });
                return contactRelationship.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Driver Document Download
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driverId"></param>
        public DriverDetailsDto GetDriverDocuments(int driverId)
        {
            try
            {

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 1),
                        new SqlParameter("@DriverId", driverId)
                    };

                var result = dbContext.ExecuteStoredProcedure<DriverDetailsDto>("usp_DriverInformation", sqlParameters);
                var driverDetail = result.Count > 0 ? result[0] : null;

                using (var con = new ExecuteSQLStoredProceduce())
                {
                    List<SqlParameter> sqlDocParams = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 2),
                        new SqlParameter("@DriverId", driverId)
                    };
                    if (driverDetail != null)
                    {
                        var docResult = con.ExecuteStoredProcedure<DriverDocumentDto>("usp_DriverInformation", sqlDocParams);
                        driverDetail.DriverDocuments = docResult.Count > 0 ? docResult : null;
                    }
                }
                return driverDetail;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Geting driver records list 
        /// </summary>
        public IEnumerable<DriverDTO> List
        {
            get
            {
                try
                {


                    var driverList = userContext.tblDrivers.Where(x => x.IsActive && x.IsDeleted == false).OrderByDescending(x => x.DriverID);
                    IEnumerable<DriverDTO> lstdriverDTO = AutoMapperServices<tblDriver, DriverDTO>.ReturnObjectList(driverList.ToList());

                    return lstdriverDTO;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion

        #region Driver List
        /// <summary>
        /// Geting driver records list 
        /// </summary>
        public IEnumerable<DriverListDto> DriverList()
        {
            try
            {


                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 4)
                    };

                var result = dbContext.ExecuteStoredProcedure<DriverListDto>("usp_LeaveManage", sqlParameters);
                result = result.Count > 0 ? result : null;

                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        if (item.TakenFrom != null)
                        {
                            item.TakenFrom = Configurations.ConvertDateTime(Convert.ToDateTime(item.TakenFrom));
                        }
                        if (item.TakenTo != null)
                        {
                            item.TakenTo = Configurations.ConvertDateTime(Convert.ToDateTime(item.TakenTo));
                        }
                    }
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Driver Inactive List
        /// <summary>
        /// Geting driver records list 
        /// </summary>
        public IEnumerable<DriverListDto> DriverInactiveList(int spType)
        {
            try
            {


                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", spType)
                    };

                var result = dbContext.ExecuteStoredProcedure<DriverListDto>("usp_LeaveManage", sqlParameters);
                result = result.Count > 0 ? result : null;

                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        if (item.TakenFrom != null)
                        {
                            item.TakenFrom = Configurations.ConvertDateTime(Convert.ToDateTime(item.TakenFrom));
                        }
                        if (item.TakenTo != null)
                        {
                            item.TakenTo = Configurations.ConvertDateTime(Convert.ToDateTime(item.TakenTo));
                        }
                    }
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Add
        /// <summary>
        /// Add Driver Record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverDTO Add(DriverDTO entity)
        {
            try
            {


                DriverDTO objDriverDTO = new DriverDTO();

                // check if email already exists in tblUser
                var isExists = userContext.tblUsers.Any(e => e.UserName == entity.EmailId && e.IsDeleted == false);

                // If exists return email already taken message
                if (isExists)
                {
                    objDriverDTO.IsSuccess = false;
                    objDriverDTO.Response = "exists";
                    objDriverDTO.Response = $"{entity.EmailId} already taken. Please use another email";
                    return objDriverDTO;
                }

                #region User Info

                tblUser user = new tblUser();
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.UserName = entity.EmailId;
                user.IsActive = false;
                user.IsDeleted = false;

                user.GUID = entity.GuidInUser;
                user.GuidGenratedDateTime = DateTime.Now;

                user.CreatedOn = Configurations.TodayDateTime;
                user.ModifiedOn = Configurations.TodayDateTime;
                user.CreatedBy = entity.CreatedBy;
                user.ModifiedBy = entity.CreatedBy;

                userContext.tblUsers.Add(user);
                #endregion

                #region Driver Info
                tblDriver objtblDriver = AutoMapperServices<DriverDTO, tblDriver>.ReturnObject(entity);
                objtblDriver.UserId = user.Userid;
                objtblDriver.IsActive = entity.IsActive;
                objtblDriver.IsDeleted = false;
                objtblDriver.CreatedDate = DateTime.UtcNow;
                userContext.tblDrivers.Add(objtblDriver);
                #endregion

                #region User Role Info
                tblUserRole userRole = new tblUserRole();
                userRole.UserID = user.Userid;
                userRole.RoleID = Convert.ToInt16(LarastruckingResource.RoleDriver);
                userContext.tblUserRoles.Add(userRole);
                #endregion

                objDriverDTO = AutoMapperServices<tblDriver, DriverDTO>.ReturnObject(objtblDriver);
                objDriverDTO.IsSuccess = userContext.SaveChanges() > 0 ? true : false;
                objDriverDTO.DriverID = objtblDriver.DriverID;
                return objDriverDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region AddDriverDocument
        /// <summary>
        /// Add driver document
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverDocumentDTO AddDriverDocument(DriverDocumentDTO entity)
        {
            try
            {


                DriverDocumentDTO objDriverDTO = new DriverDocumentDTO();
                tblDriverDocument objDriverDocument = AutoMapperServices<DriverDocumentDTO, tblDriverDocument>.ReturnObject(entity);
                if (("Management" == entity.UserRole && entity.UserId > 0) || ("Accounting" == entity.UserRole && entity.UserId > 0))
                {
                    objDriverDocument.IsActive = true;
                    objDriverDocument.IsDeleted = false;
                    userContext.tblDriverDocuments.Add(objDriverDocument);
                    objDriverDTO = AutoMapperServices<tblDriverDocument, DriverDocumentDTO>.ReturnObject(objDriverDocument);
                    objDriverDTO.IsSuccess = userContext.SaveChanges() > 0 ? true : false;
                    return objDriverDTO;
                }
                return objDriverDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete  Driver Record 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(DriverDTO entity)
        {
            try
            {
                bool result = false;
                var table = userContext.tblDrivers.Find(entity.DriverID);
                if (table != null)
                {
                    table.IsDeleted = true;
                    userContext.Entry(table).State = System.Data.Entity.EntityState.Modified;

                }
                var tbluser = userContext.tblUsers.Find(table.UserId);
                if (table != null)
                {
                    tbluser.IsDeleted = true;
                    userContext.Entry(tbluser).State = System.Data.Entity.EntityState.Modified;
                }
                result = userContext.SaveChanges() > 0 ? true : false;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region DeleteDocument
        /// <summary>
        /// Delete  Driver Document 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteDocument(int DriverId)
        {
            bool result = false;
            var table = userContext.tblDriverDocuments.Find(DriverId);
            if (table != null)
            {
                table.IsDeleted = true;
                table.IsActive = false;
                userContext.Entry(table).State = System.Data.Entity.EntityState.Modified;
                result = userContext.SaveChanges() > 0 ? true : false;
            }
            return result;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Driver Record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverDTO Update(DriverDTO entity)
        {
            try
            {
                if(entity.IsActive)
                {
                    entity.IsActive = DriverDocumetsType(entity.DriverID);
                }

                #region Update User Table
                tblUser user = new tblUser();
                DriverDTO objDriverDTO = new DriverDTO();

                var objtblDriver = userContext.tblDrivers.Where(x => x.DriverID == entity.DriverID).FirstOrDefault();
                var isExists = userContext.tblUsers.Any(e => e.UserName.Trim().ToUpper() == entity.EmailId.Trim().ToUpper() && e.Userid != objtblDriver.UserId && e.IsDeleted == false);

                if (isExists)
                {
                    objDriverDTO.IsSuccess = false;
                    objDriverDTO.Response = $"{entity.EmailId} already taken. Please use another email";
                    return objDriverDTO;
                }

                var dbUserExists = userContext.tblUsers.Find(objtblDriver.UserId);
                int userId = 0;
                if (dbUserExists == null)
                {
                    user.FirstName = entity.FirstName;
                    user.LastName = entity.LastName;
                    user.UserName = entity.EmailId;
                    user.IsActive = entity.IsActive;
                    user.IsDeleted = entity.IsDeleted;
                    user.CreatedOn = Configurations.TodayDateTime;
                    user.ModifiedOn = Configurations.TodayDateTime;
                    user.CreatedBy = entity.CreatedBy;
                    user.ModifiedBy = entity.CreatedBy;
                    userContext.tblUsers.Add(user);
                    userId = user.Userid;
                }
                else
                {
                    dbUserExists.FirstName = entity.FirstName;
                    dbUserExists.LastName = entity.LastName;
                    dbUserExists.UserName = entity.EmailId;
                    dbUserExists.IsActive = entity.IsActive;
                    dbUserExists.IsDeleted = entity.IsDeleted;
                    dbUserExists.ModifiedOn = Configurations.TodayDateTime;
                    dbUserExists.ModifiedBy = entity.CreatedBy;

                    userId = dbUserExists.Userid;
                }
                #endregion

                #region User Role Info
                var dbRoleExists = userContext.tblUserRoles.Where(e => e.UserID == userId).ToList();
                if (dbRoleExists.Count > 0)
                {
                    userContext.tblUserRoles.RemoveRange(dbRoleExists);
                }

                tblUserRole userRole = new tblUserRole();
                userRole.UserID = userId;
                userRole.RoleID = Convert.ToInt16(LarastruckingResource.RoleDriver);
                userContext.tblUserRoles.Add(userRole);
                #endregion

                #region Driver Info
                objtblDriver.UserId = userId;
                objtblDriver.FirstName = entity.FirstName;
                objtblDriver.LastName = entity.LastName;
                objtblDriver.EmailId = entity.EmailId;
                objtblDriver.Address1 = entity.Address1;
                objtblDriver.Address2 = entity.Address2;
                objtblDriver.CitizenShip = entity.CitizenShip;
                objtblDriver.ZipCode = entity.ZipCode;
                objtblDriver.Country = entity.Country;
                objtblDriver.State = entity.State;
                objtblDriver.City = entity.City;
                objtblDriver.Phone = entity.Phone;
                objtblDriver.Vehicle = entity.Vehicle;
                objtblDriver.CellNumber = entity.CellNumber;
                objtblDriver.BloodGroup = entity.BloodGroup;
                objtblDriver.MedicalConditions = entity.MedicalConditions;
                objtblDriver.STANumber = entity.STANumber;
                objtblDriver.ExpirationDate = entity.ExpirationDate;
                objtblDriver.EmergencyContactOne = entity.EmergencyContactOne;
                objtblDriver.EmergencyContactTwo = entity.EmergencyContactTwo;
                objtblDriver.EmergencyPhoneNoOne = entity.EmergencyPhoneNoOne;
                objtblDriver.EmergencyPhoneNoTwo = entity.EmergencyPhoneNoTwo;
                objtblDriver.RelationshipStatus1 = entity.RelationshipStatus1;
                objtblDriver.RelationshipStatus2 = entity.RelationshipStatus2;
                objtblDriver.IsActive = entity.IsActive;
                objtblDriver.LanguageId = entity.LanguageId;
                objtblDriver.Extension = entity.Extension;
                objtblDriver.ModifiedDate = DateTime.UtcNow;
                #endregion

                objDriverDTO.IsSuccess = userContext.SaveChanges() > 0 ? true : false;
                return objDriverDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region FindById
        /// <summary>
        /// Find Driver  by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DriverDTO FindById(int Id)
        {
            try
            {
                DriverDTO objDriverDTO = null;
                var result = (from r in userContext.tblDrivers where r.DriverID == Id select r).FirstOrDefault();

                if (result != null)
                {
                    objDriverDTO = AutoMapperServices<tblDriver, DriverDTO>.ReturnObject(result);
                    var driverDocument = (from driverDocuments in userContext.tblDriverDocuments
                                          join documentTypes in userContext.tblDocumentTypes on driverDocuments.DocumentTypeId equals documentTypes.ID
                                          where driverDocuments.DriverId == Id && driverDocuments.IsDeleted == false && driverDocuments.IsActive == true

                                          select new DriverDocumentDTO
                                          {
                                              DocumentId = driverDocuments.DocumentId,
                                              DocumentTypeId = driverDocuments.DocumentTypeId,
                                              DocumentTypeName = documentTypes.Name,
                                              DocumentName = (driverDocuments.DocumentName == null) ? documentTypes.Name : driverDocuments.DocumentName,
                                              ImageName = driverDocuments.ImageName,
                                              ImageURL= (LarastruckingApp.Entities.Common.Configurations.BaseURL + driverDocuments.ImageURL),
                                              DocumentExpiryDate = driverDocuments.DocumentExpiryDate
                                          }
                                         ).ToList();
                    if (driverDocument.Count() > 0)
                    {
                        foreach (var driverDoc in driverDocument)
                        {
                            driverDoc.DocumentExpiryDate = Configurations.ConvertDateTime(Convert.ToDateTime(driverDoc.DocumentExpiryDate));
                        }
                    }
                    objDriverDTO.DriverDocumentList = driverDocument;
                }
                return objDriverDTO;
            }
            catch (Exception)
            {
                throw;
            }
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
            try
            {
                DriverDTO objDriverDTO = null;
                var result = userContext.tblDrivers.FirstOrDefault(u => u.UserId == userId);

                if (result != null)
                {
                    objDriverDTO = AutoMapperServices<tblDriver, DriverDTO>.ReturnObject(result);
                }
                return objDriverDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region DocumentList
        /// <summary>
        /// Get Document
        /// </summary>
        /// <returns></returns>
        public List<DocumentNameDTO> DocumentList()
        {
            try
            {

                return AutoMapperServices<tblDocumentType, DocumentNameDTO>.ReturnObjectList(userContext.tblDocumentTypes.ToList());

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetCountryList
        /// <summary>
        /// Get Country List 
        /// </summary>
        /// <returns></returns>
        public List<CountryDTO> GetCountryList()
        {
            try
            {
                List<CountryDTO> countryList = AutoMapperServices<tblCountry, CountryDTO>.ReturnObjectList(userContext.tblCountries.Where(e => e.ID == 231).ToList());
                return countryList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetLanguageList
        /// <summary>
        /// Get language List 
        /// </summary>
        /// <returns></returns>
        public List<LanguageDTO> GetLanguageList()
        {
            try
            {
                List<LanguageDTO> languageList = AutoMapperServices<tblLanguage, LanguageDTO>.ReturnObjectList(userContext.tblLanguages.ToList());
                return languageList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetStateList
        /// <summary>
        /// Get State List
        /// </summary>
        /// <returns></returns>
        public List<StateDTO> GetStateList()
        {
            try
            {
                List<StateDTO> stateList = AutoMapperServices<tblState, StateDTO>.ReturnObjectList(userContext.tblStates.Where(e => e.CountryID == 231).ToList());
                return stateList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetEquipment
        /// <summary>
        /// Get Equipment
        /// </summary>
        /// <returns></returns>
        public List<EquipmentDTO> GetEquipment()
        {
            try
            {
                List<EquipmentDTO> EquipmentList = AutoMapperServices<tblEquipmentDetail, EquipmentDTO>.ReturnObjectList(userContext.tblEquipmentDetails.Where(x => x.IsDeleted == false).ToList());
                return EquipmentList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Document By DocId
        /// <summary>
        /// Method to get driver document available to download
        /// </summary>
        /// <param name="id"></param>
        public DriverDocumentDto DownloadDocument(int id)
        {
            try
            {


                DriverDocumentDto dto = new DriverDocumentDto();

                var document = userContext.tblDriverDocuments
                                            .Find(id);
                if (document != null)
                {
                    dto = AutoMapperServices<tblDriverDocument, DriverDocumentDto>.ReturnObject(document);
                }
                return dto;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Mandatory Driver Documents
        /// <summary>
        /// Mandatory Driver Documents
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool DriverDocumetsType(int DriverID)
        {
            try
            {
                bool isActive = false;
                var result = userContext.tblDriverDocuments.Any(t => t.DriverId == DriverID && t.DocumentTypeId == 2 && t.IsActive == true);
                var result1 = userContext.tblDriverDocuments.Any(t => t.DriverId == DriverID && t.DocumentTypeId == 3 && t.IsActive == true);
                var result2 = userContext.tblDriverDocuments.Any(t => t.DriverId == DriverID && t.DocumentTypeId == 5 && t.IsActive == true);

                var tblDriver = userContext.tblDrivers.Where(x => x.DriverID == DriverID).Select(x => x).FirstOrDefault();
                if (tblDriver != null)
                {
                    if (result && result1 && result2)
                    {
                        
                        var tblUser = userContext.tblUsers.Find(tblDriver.UserId);
                        if (tblUser != null)
                        {
                            isActive = true;
                            tblDriver.IsActive = true;
                            userContext.Entry(tblDriver).State = System.Data.Entity.EntityState.Modified;

                            tblUser.IsActive = true;
                            userContext.Entry(tblUser).State = System.Data.Entity.EntityState.Modified;
                            userContext.SaveChanges();
                        }
                    }
                    else
                    {
                        
                        var tblUser = userContext.tblUsers.Find(tblDriver.UserId);
                        if (tblUser != null)
                        {
                            isActive = false;
                            tblDriver.IsActive = false;
                            userContext.Entry(tblDriver).State = System.Data.Entity.EntityState.Modified;

                            tblUser.IsActive = false;
                            userContext.Entry(tblUser).State = System.Data.Entity.EntityState.Modified;
                            userContext.SaveChanges();
                        }
                    }

                }

                return isActive;

            }
            catch (Exception)
            {
                throw;
            }
            
        }
        #endregion
    }
}
