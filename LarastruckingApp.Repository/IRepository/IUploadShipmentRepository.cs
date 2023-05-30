using LarastruckingApp.DTO;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Entities.UploadShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IUploadShipmentRepository
    {
        List<CompanyNameDTO> GetCompanyName(UserRoleDTO dto);
        string GetSample();
        List<UploadShipmentDTO> ValidedUploadShipment(List<UploadShipmentDTO> uploadShipmentList);
        FreightTypeNPricingMethodDTO BindFreightTypeNPricingMethod();
        bool SaveExcelData(List<UploadShipmentDTO> model);
        bool CheckFileName(string fileName);
        bool ValidateContactInfo(int customerId);
    }
}
