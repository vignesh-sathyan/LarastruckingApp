using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Entities.UploadShipmentDTO;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class UploadShipmentDAL : IUploadShipmentDAL
    {
        #region Private member
        /// <summary>
        /// Address Data Access Layer
        /// </summary>
        private readonly IUploadShipmentRepository iUploadShipmentRepository;
        #endregion

        #region Upload Shipment DAL
        public UploadShipmentDAL(IUploadShipmentRepository iUploadShipmentRepository)
        {
            this.iUploadShipmentRepository = iUploadShipmentRepository;
        }




        #endregion

        #region GetCompanyName
        /// <summary>
        /// Get Company name
        /// </summary>
        /// <returns></returns>
        public List<CompanyNameDTO> GetCompanyName(UserRoleDTO dto)
        {
            return iUploadShipmentRepository.GetCompanyName(dto);
        }


        #endregion

        #region GetSample
        /// <summary>
        /// Get shipment sample
        /// </summary>
        /// <returns></returns>
        public string GetSample()
        {
            return iUploadShipmentRepository.GetSample();
        }


        #endregion

        #region Valided UploadShipent
        /// <summary>
        /// Valided Upload Shipment 
        /// </summary>
        /// <param name="uploadShipmentList"></param>
        /// <returns></returns>

        public List<UploadShipmentDTO> ValidedUploadShipment(List<UploadShipmentDTO> uploadShipmentList)
        {
            return iUploadShipmentRepository.ValidedUploadShipment(uploadShipmentList);

        }
        #endregion

        #region bind pricing method and freight type dropdown
        /// <summary>
        /// bind pricing method and freight type and pcs type dropdown
        /// </summary>
        /// <returns></returns>
        public FreightTypeNPricingMethodDTO BindFreightTypeNPricingMethod()
        {
            return iUploadShipmentRepository.BindFreightTypeNPricingMethod();
        }


        #endregion

        #region save excel sheet
        /// <summary>
        /// save excel sheet data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveExcelData(List<UploadShipmentDTO> model)
        {
            return iUploadShipmentRepository.SaveExcelData(model);
        }


        #endregion
        #region validate file name
        public bool CheckFileName(string fileName)
        {
            return iUploadShipmentRepository.CheckFileName(fileName);
        }


        #endregion

        #region validate customer count
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool ValidateContactInfo(int customerId)
        {
            return iUploadShipmentRepository.ValidateContactInfo(customerId);
        }
        #endregion
    }
}
