using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class AccidentReportDAL : IAccidentReportDAL
    {
        #region Private Member
        private IAccidentReportRepository iAccidentRepo;
        #endregion

        #region Constructor 
        public AccidentReportDAL(IAccidentReportRepository iAccidentReportRepository)
        {
            iAccidentRepo = iAccidentReportRepository;

        }
        #endregion

        #region List
        /// <summary>
        /// Get Accident Report List
        /// </summary>
        public IEnumerable<AccidentReportDTO> List
        {
            get
            {
                return iAccidentRepo.List;
            }
        }
        #endregion

        #region AccidentDocumentList
        /// <summary>
        /// Get Accident Document List
        /// </summary>
        /// <returns></returns>
        public List<AccidentDocumentDTO> AccidentDocumentList()
        {
            return iAccidentRepo.DocumentList();
        }
        #endregion

        #region Add
        /// <summary>
        /// Add Accident Report 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AccidentReportDTO Add(AccidentReportDTO entity)
        {
            return iAccidentRepo.Add(entity);
        }
        #endregion

        #region AddAccidentDocument
        /// <summary>
        /// Add Accident Report Document
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AccidentReportDocumentDTO AddAccidentDocument(AccidentReportDocumentDTO entity)
        {
            return iAccidentRepo.AddAccidentReportDocument(entity);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Accident Report
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(AccidentReportDTO entity)
        {
            return iAccidentRepo.Delete(entity);
        }
        #endregion

        #region DeleteDoucument
        /// <summary>
        /// delete accident document by id
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <returns></returns>
        public bool DeleteDoucument(int DocumentId)
        {
            return iAccidentRepo.DeleteDoucument(DocumentId);
        }
        #endregion

        #region FindById
        /// <summary>
        /// Find accident report by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AccidentReportDTO FindById(int Id)
        {
            return iAccidentRepo.FindById(Id);
        }
        #endregion

        #region GetDriverList
        /// <summary>
        ///  Driver List
        /// </summary>
        /// <returns></returns>
        public List<DriverDTO> GetDriverList()
        {
            return iAccidentRepo.GetDriverList();
        }
        #endregion

        #region Update
        /// <summary>
        /// Method For update Report Accident
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AccidentReportDTO Update(AccidentReportDTO entity)
        {
            return iAccidentRepo.Update(entity);
        }
        #endregion

        #region ViewAccidentReport
        /// <summary>
        ///  method for get Accident Report
        /// </summary>
        /// <returns></returns>
        public List<AccidentReportDTO> ViewAccidentReport(UserDTO _user)
        {
            return iAccidentRepo.ViewAccidentReport(_user);
        }


        #endregion

        #region view accident report document
        /// <summary>
        /// View accident report document
        /// </summary>
        /// <returns></returns>

        public AccidentReportDTO ViewAccidentReportDocument(int accidentId)
        {
            return iAccidentRepo.ViewAccidentReportDocument(accidentId);
        }
        #endregion
    }
}
