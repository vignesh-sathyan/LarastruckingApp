using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
    public interface IFumigationBAL
    {
        List<FumigationTypeDTO> GetFumigationTypeList();
        FumigationDTO CreateFumigation(FumigationDTO entity);
        IList<FumigationListDTO> GetFumigationList(DataTableFilterDto entity);
        bool DeleteFumigation(FumigationDTO entity);

        GetFumigationDTO GetFumigationById(int fumigationId);
        int UploadProofofTempDocument(List<FumigationProofOfTemprature> fumigationProofOfTemprature);
        int UploadDamageDocument(List<FumigationDamageImages> damageImageList);
        bool DeleteProofOfTemprature(FumigationProofOfTemprature model);
        bool DeleteDamageFile(FumigationDamageImages model);
        FumigationDTO EditFumigation(FumigationDTO entity);
        int AddRouteStops(GetFumigationRouteDTO model);
        FumigationDTO ApprovedFumigation(FumigationDTO entity);
        FumigationEmailDTO GetCustomerDetail(FumigationEmailDTO fumigationEmailDTO);
        TemperatureEmailDTO GetTemperatureEmailDetail(int fumigationId);
        List<GetFumigationRouteDTO> FumigationProofOfDelivery(string fumigationId);
        List<ShipmentStatusDTO> GetStatusList();
        bool ApprovedProofOFTemp(ProofOfTemperatureDTO entity);
        bool ApprovedDamageImage(FumigationDamageImages entity);
        #region get fumigation detail by id for copy
        /// <summary>
        /// Get proof Of delivery
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        CopyFumigationDTO GetFumigationDetailById(int fumigationId);
        #endregion

        #region Copy Fumigation
        /// <summary>
        /// Copy Fumigation detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        bool SaveCopyFumigatonDetail(CopyFumigationDTO entity);
        #endregion

        #region Get max route no. 
        /// <summary>
        /// Get max route no.
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        int? GetMaxRouteNo(int fumigationId);
        #endregion
        IList<FumigationListDTO> ViewAllFumigation(AllShipmentDTO entity);
        IList<FumigationOtherList> GetOtherFumigationList(DataTableFilterDto entity);
    }
}
