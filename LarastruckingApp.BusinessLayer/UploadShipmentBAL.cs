using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Entities.UploadShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class UploadShipmentBAL : IUploadShipmentBAL
    {
        #region Private member
        /// <summary>
        /// Private member
        /// </summary>
        private IUploadShipmentDAL iUploadShipmentDAL;
        #endregion

        #region AddressBAL
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iUploadShipmentDAL"></param>
        public UploadShipmentBAL(IUploadShipmentDAL iUploadShipmentDAL)
        {
            this.iUploadShipmentDAL = iUploadShipmentDAL;
        }
        #endregion

        #region GetCompanyName
        /// <summary>
        /// Get Company name
        /// </summary>
        /// <returns></returns>
        public List<CompanyNameDTO> GetCompanyName(UserRoleDTO dto)
        {
            return iUploadShipmentDAL.GetCompanyName(dto);
        }


        #endregion

        #region GetSample
        /// <summary>
        /// Get shipment sample by address id
        /// </summary>
        /// <returns></returns>
        public string GetSample()
        {
            return iUploadShipmentDAL.GetSample();
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
            return iUploadShipmentDAL.ValidedUploadShipment(uploadShipmentList);
        }
        #endregion

        #region bind pricing method and freight type dropdown
        /// <summary>
        /// bind pricing method and freight type and pcs type dropdown
        /// </summary>
        /// <returns></returns>
        public FreightTypeNPricingMethodDTO BindFreightTypeNPricingMethod()
        {
            return iUploadShipmentDAL.BindFreightTypeNPricingMethod();
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
            return iUploadShipmentDAL.SaveExcelData(model);
        }


        #endregion

        #region validate file name
        /// <summary>
        /// Check file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool CheckFileName(string fileName)
        {
            return iUploadShipmentDAL.CheckFileName(fileName);
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
           return iUploadShipmentDAL.ValidateContactInfo(customerId);
        }
        #endregion
    }
}
