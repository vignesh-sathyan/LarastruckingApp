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
    public class AccidentReportRepository : IAccidentReportRepository
    {
        #region private member
        private readonly LarastruckingDBEntities accidentContext;
        #endregion

        #region constroctor
        public AccidentReportRepository()
        {
            accidentContext = new LarastruckingDBEntities();
        }
        #endregion

        /// <summary>
        /// Get Accident Report
        /// </summary>
        public IEnumerable<AccidentReportDTO> List
        {
            get
            {
                try
                {


                    var accidentReportList = accidentContext.tblAccidentReports.Where(x => x.IsDeleted == false).OrderByDescending(x => x.AccidentReportId);
                    IEnumerable<AccidentReportDTO> lstAccidentReportDTO = AutoMapperServices<tblAccidentReport, AccidentReportDTO>.ReturnObjectList(accidentReportList.ToList());

                    return lstAccidentReportDTO;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Add Accident Report
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AccidentReportDTO Add(AccidentReportDTO entity)
        {
            try
            {


                AccidentReportDTO objAccidentReportDTO = new AccidentReportDTO();
                tblAccidentReport objtblAccidentReport = AutoMapperServices<AccidentReportDTO, tblAccidentReport>.ReturnObject(entity);
                objtblAccidentReport.IsActive = true;
                objtblAccidentReport.IsDeleted = false;
                objtblAccidentReport.CreatedDate = DateTime.UtcNow;
                objtblAccidentReport.CreatedBy = entity.CreatedBy;
                accidentContext.tblAccidentReports.Add(objtblAccidentReport);
                objAccidentReportDTO = AutoMapperServices<tblAccidentReport, AccidentReportDTO>.ReturnObject(objtblAccidentReport);
                objAccidentReportDTO.IsSuccess = accidentContext.SaveChanges() > 0;
                objAccidentReportDTO.AccidentReportId = objtblAccidentReport.AccidentReportId;
                return objAccidentReportDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Add Accident Report Document
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AccidentReportDocumentDTO AddAccidentReportDocument(AccidentReportDocumentDTO entity)
        {
            try
            {


                AccidentReportDocumentDTO objAccidentReportDocumentDTO = new AccidentReportDocumentDTO();
                tblAccidentReportDocument objAccidentReportDocument = AutoMapperServices<AccidentReportDocumentDTO, tblAccidentReportDocument>.ReturnObject(entity);
                objAccidentReportDocument.IsActive = true;
                objAccidentReportDocument.IsDeleted = false;
                objAccidentReportDocument.CreatedDate = DateTime.UtcNow;
                accidentContext.tblAccidentReportDocuments.Add(objAccidentReportDocument);
                objAccidentReportDocumentDTO = AutoMapperServices<tblAccidentReportDocument, AccidentReportDocumentDTO>.ReturnObject(objAccidentReportDocument);
                objAccidentReportDocumentDTO.IsSuccess = accidentContext.SaveChanges() > 0;
                return objAccidentReportDocumentDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Delete  Accident Report 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(AccidentReportDTO entity)
        {
            try
            {

                bool result = false;
                var table = accidentContext.tblAccidentReports.Find(entity.AccidentReportId);
                if (table != null)
                {
                    table.IsDeleted = true;
                    accidentContext.Entry(table).State = System.Data.Entity.EntityState.Modified;
                    result = accidentContext.SaveChanges() > 0;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Delete Uploaded Document
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <returns></returns>
        public bool DeleteDoucument(int DocumentId)
        {
            try
            {


                bool result = false;
                var table = accidentContext.tblAccidentReportDocuments.Find(DocumentId);
                if (table != null)
                {
                    table.IsDeleted = true;
                    accidentContext.Entry(table).State = System.Data.Entity.EntityState.Modified;
                    result = accidentContext.SaveChanges() > 0;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// Get Repot Accident and Uploaded Document Data
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AccidentReportDTO FindById(int Id)
        {
            try
            {


                AccidentReportDTO objAccidentReportDTO = null;
                objAccidentReportDTO = (from accidentReport in accidentContext.tblAccidentReports
                                        join equipment in accidentContext.tblEquipmentDetails on accidentReport.EquipmentId equals equipment.EDID
                                        join vehicletype in accidentContext.tblEquipmentTypes on equipment.VehicleType equals vehicletype.VehicleTypeID
                                        join driver in accidentContext.tblDrivers on accidentReport.DriverId equals driver.DriverID
                                        where accidentReport.IsDeleted == false && accidentReport.AccidentReportId == Id
                                        select new AccidentReportDTO
                                        {
                                            AccidentReportId = accidentReport.AccidentReportId,
                                            EquipmentNo = equipment.EquipmentNo,
                                            LicencePlate = equipment.LicencePlate,
                                            VIN = equipment.VIN,
                                            Model = equipment.Model,
                                            Year = equipment.Year,
                                            VehicleType = vehicletype.VehicleTypeName,
                                            Address = accidentReport.Address,
                                            DriverId = accidentReport.DriverId,
                                            PhoneNo = driver.Phone,
                                            EmailId = driver.EmailId,
                                            AccidentDate = accidentReport.AccidentDate,
                                            AccidentTime = accidentReport.AccidentTime,
                                            Comments = accidentReport.Comments,
                                            EquipmentId = accidentReport.EquipmentId,
                                            DriverName = driver.FirstName + " " + (driver.LastName == null ? string.Empty : driver.LastName),
                                            PoliceReportNo=accidentReport.PoliceReportNo

                                        }
                 ).FirstOrDefault();

                if (objAccidentReportDTO != null)
                {
                    var data = (from acrd in accidentContext.tblAccidentReportDocuments
                                join accdoc in accidentContext.tblAccidentDocuments on acrd.AccidentDocumentId equals accdoc.AccidentDocumentId
                                where acrd.AccidentReportId == Id && acrd.IsDeleted == false
                                select new AccidentReportDocumentDTO
                                {
                                    DocumentId = acrd.DocumentId,
                                    DocumentName = acrd.DocumentName == null ? string.Empty : acrd.DocumentName,
                                    ImageName = acrd.ImageName,
                                    AccidentReportId = acrd.AccidentReportId,
                                    AccidentDocumentId = acrd.AccidentDocumentId,
                                    DocumentTypeName = accdoc.Name == null ? string.Empty : accdoc.Name
                                }
                                ).ToList();
                    objAccidentReportDTO.AccidentDate = Configurations.ConvertDateTime(Convert.ToDateTime(objAccidentReportDTO.AccidentDate));
                    objAccidentReportDTO.AccidentReportDocumentList = data;

                }
                return objAccidentReportDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method for update Report Accident
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AccidentReportDTO Update(AccidentReportDTO entity)
        {
            try
            {


                AccidentReportDTO objAccidentReportDTO = new AccidentReportDTO();
                var objAccidentReport = accidentContext.tblAccidentReports.Where(x => x.AccidentReportId == entity.AccidentReportId).FirstOrDefault();
                if (objAccidentReport != null)
                {

                    objAccidentReport.EquipmentId = entity.EquipmentId;
                    objAccidentReport.DriverId = entity.DriverId;
                    objAccidentReport.AccidentDate = entity.AccidentDate;
                    objAccidentReport.AccidentTime = entity.AccidentTime;
                    objAccidentReport.Address = entity.Address;
                    objAccidentReport.Comments = entity.Comments;
                    objAccidentReport.PoliceReportNo = entity.PoliceReportNo;
                    objAccidentReport.ModifiedBy = entity.ModifiedBy;
                    objAccidentReport.ModifiedDate = entity.ModifiedDate;

                    accidentContext.Entry(objAccidentReport).State = System.Data.Entity.EntityState.Modified;
                    objAccidentReportDTO = AutoMapperServices<tblAccidentReport, AccidentReportDTO>.ReturnObject(objAccidentReport);
                    objAccidentReportDTO.IsSuccess = accidentContext.SaveChanges() > 0;

                }
                return objAccidentReportDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get Driver List
        /// </summary>
        /// <returns></returns>
        public List<DriverDTO> GetDriverList()
        {
            try
            {


                var driverList = accidentContext.tblDrivers.Where(d => d.IsDeleted == false && d.IsActive).ToList();
                List<DriverDTO> driverlist = AutoMapperServices<tblDriver, DriverDTO>.ReturnObjectList(driverList);
                return driverlist;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get Accident Document List
        /// </summary>
        /// <returns></returns>
        public List<AccidentDocumentDTO> DocumentList()
        {
            try
            {


                var doc = accidentContext.tblAccidentDocuments.ToList();
                return AutoMapperServices<tblAccidentDocument, AccidentDocumentDTO>.ReturnObjectList(doc);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get View Accident Report
        /// </summary>
        /// <returns></returns>
        public List<AccidentReportDTO> ViewAccidentReport(UserDTO _user)
        {
            try
            {

                var data = (from accidentReport in accidentContext.tblAccidentReports
                            join equipment in accidentContext.tblEquipmentDetails on accidentReport.EquipmentId equals equipment.EDID
                            join vehicletype in accidentContext.tblEquipmentTypes on equipment.VehicleType equals vehicletype.VehicleTypeID
                            join driver in accidentContext.tblDrivers on accidentReport.DriverId equals driver.DriverID
                            where accidentReport.IsDeleted == false && (_user.RoleName.ToLower() == ("Driver").ToLower() ? driver.UserId == _user.Userid : 1 == 1)
                            select new AccidentReportDTO
                            {
                                EquipmentNo= equipment.EquipmentNo,
                                AccidentReportId = accidentReport.AccidentReportId,
                                EquipmentId = accidentReport.EquipmentId,
                                VIN = equipment.VIN,
                                VehicleType = vehicletype.VehicleTypeName,
                                Model = equipment.Model,
                                Year = equipment.Year,
                                LicencePlate = equipment.LicencePlate,
                                DriverName = driver.FirstName + " " + (driver.LastName ?? string.Empty),
                                AccidentDate= accidentReport.AccidentDate,
                                PoliceReportNo=accidentReport.PoliceReportNo
                            }
                          ).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }


        #region view accident report document
        /// <summary>
        /// View accident report document
        /// </summary>
        /// <returns></returns>
        public AccidentReportDTO ViewAccidentReportDocument(int accidentId)
        {
            try
            {
                var accidentReportDocument = (from accidentReport in accidentContext.tblAccidentReports
                                              join equipment in accidentContext.tblEquipmentDetails on accidentReport.EquipmentId equals equipment.EDID
                                              join vehicletype in accidentContext.tblEquipmentTypes on equipment.VehicleType equals vehicletype.VehicleTypeID
                                              join driver in accidentContext.tblDrivers on accidentReport.DriverId equals driver.DriverID
                                              where accidentReport.IsDeleted == false && accidentReport.AccidentReportId == accidentId
                                              select new AccidentReportDTO
                                              {
                                                  EquipmentNo = equipment.EquipmentNo,
                                                  AccidentReportId = accidentReport.AccidentReportId,
                                                  VIN = equipment.VIN,
                                                  VehicleType = vehicletype.VehicleTypeName,
                                                  Model = equipment.Model,
                                                  Year = equipment.Year,
                                                  LicencePlate = equipment.LicencePlate,
                                                  DriverName = driver.FirstName + " " + (driver.LastName == null ? string.Empty : driver.LastName),
                                                  AccidentDate = accidentReport.AccidentDate,
                                                  AccidentReportDocumentList = (from document in accidentContext.tblAccidentReportDocuments
                                                                                join documentType in accidentContext.tblAccidentDocuments on document.AccidentDocumentId equals documentType.AccidentDocumentId
                                                                                where document.AccidentReportId == accidentId
                                                                                select new AccidentReportDocumentDTO
                                                                                {
                                                                                    ImageName = document.ImageName,
                                                                                    ImageURL = document.ImageURL,
                                                                                    DocumentName = document.DocumentName,
                                                                                    DocumentTypeName = documentType.Name,

                                                                                }
                                                                              ).ToList()
                                              }
                          ).FirstOrDefault();
                return accidentReportDocument;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


    }
}
