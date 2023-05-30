using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class FumigationDAL : IFumigationDAL
    {
        #region Private member
        /// <summary>
        /// Fumigation Data Access Layer
        /// </summary>
        private readonly IFumigationRepository iFumigationRepository;
        #endregion

        #region FumigationDAL
        public FumigationDAL(IFumigationRepository iFumigationRepository)
        {
            this.iFumigationRepository = iFumigationRepository;
        }

        #endregion

        #region GetFumigationTypeList
        /// <summary>
        /// Get Fumigation Type  List
        /// </summary>
        /// <returns></returns>
        public List<FumigationTypeDTO> GetFumigationTypeList()
        {
            return iFumigationRepository.GetFumigationTypeList();
        }
        #endregion


        #region create fumigation
        /// <summary>
        ///  Create Fumigation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public FumigationDTO CreateFumigation(FumigationDTO entity)
        {
            return iFumigationRepository.CreateFumigation(entity);
        }

        #endregion

        #region get fumigation list
        /// <summary>
        /// get fumigation list
        /// </summary>
        /// <returns></returns>

        public IList<FumigationListDTO> GetFumigationList(DataTableFilterDto entity)
        {
            return iFumigationRepository.GetFumigationList(entity);
        }


        #endregion
        #region get other fumigation list
        /// <summary>
        /// get other fumigation list
        /// </summary>
        /// <returns></returns>
        public IList<FumigationOtherList> GetOtherFumigationList(DataTableFilterDto entity)
        {
            return iFumigationRepository.GetOtherFumigationList(entity);
        }
        #endregion

        #region View All Fumigation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public IList<FumigationListDTO> ViewAllFumigation(AllShipmentDTO entity)
        {
            return iFumigationRepository.ViewAllFumigation(entity);
        }
        #endregion

        #region delete fumigation
        /// <summary>
        ///Delete shipment By Id
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        public bool DeleteFumigation(FumigationDTO entity)
        {
            return iFumigationRepository.DeleteFumigation(entity);
        }


        #endregion

        #region get fumigation by id 
        /// <summary>
        ///  get fumigation by id for edit fumigaion
        /// </summary>
        /// <returns></returns>
        public GetFumigationDTO GetFumigationById(int fumigationId)
        {
            return iFumigationRepository.GetFumigationById(fumigationId);
        }


        #endregion

        #region Upload Proof of Temp
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        public int UploadProofofTempDocument(List<FumigationProofOfTemprature> fumigationProofOfTemprature)
        {
            return iFumigationRepository.UploadProofofTempDocument(fumigationProofOfTemprature);
        }


        #endregion

        #region Upload Damage Document
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        public int UploadDamageDocument(List<FumigationDamageImages> damageImageList)
        {
            return iFumigationRepository.UploadDamageDocument(damageImageList);
        }
        #endregion

        #region Delete Proof of Temprature
        /// <summary>
        /// Delete proof of temprature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteProofOfTemprature(FumigationProofOfTemprature model)
        {
            return iFumigationRepository.DeleteProofOfTemprature(model);
        }
        #endregion
        #region Delete Damage File
        /// <summary>
        /// Delete damage file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteDamageFile(FumigationDamageImages model)
        {
            return iFumigationRepository.DeleteDamageFile(model);
        }
        #endregion

        #region Edit Fumigation
        /// <summary>
        ///  Edit Fumigation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public FumigationDTO EditFumigation(FumigationDTO entity)
        {
            return iFumigationRepository.EditFumigation(entity);
        }


        #endregion

        #region Add Route Stops
        /// <summary>
        /// add route stops
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddRouteStops(GetFumigationRouteDTO model)
        {
            return iFumigationRepository.AddRouteStops(model);
        }


        #endregion

        #region Approved fumigation
        /// <summary>
        /// Approved fumigation if fumigation on hold
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public FumigationDTO ApprovedFumigation(FumigationDTO entity)
        {
            return iFumigationRepository.ApprovedFumigation(entity);
        }


        #endregion

        #region get customerDetail
        /// <summary>
        /// Get Customer Detail
        /// </summary>
        /// <param name="fumigationEmailDTO"></param>
        /// <returns></returns>
        public FumigationEmailDTO GetCustomerDetail(FumigationEmailDTO fumigationEmailDTO)
        {
            return iFumigationRepository.GetCustomerDetail(fumigationEmailDTO);
        }


        #endregion

        #region fumigation proof of delivery
        /// <summary>
        /// Get proof Of delivery
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        public List<GetFumigationRouteDTO> FumigationProofOfDelivery(string fumigationId)
        {
            return iFumigationRepository.FumigationProofOfDelivery(fumigationId);
        }


        #endregion

        #region fumigation status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            return iFumigationRepository.GetStatusList();
        }

        #endregion

        #region Approve Proof Of Temprature
        /// <summary>
        /// Approve Proof Of Temprature
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ApprovedProofOFTemp(ProofOfTemperatureDTO entity)
        {
            return iFumigationRepository.ApprovedProofOFTemp(entity);
        }

        #endregion

        #region Approve Damage Image
        /// <summary>
        /// Approve Damage Image
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ApprovedDamageImage(FumigationDamageImages entity)
        {
            return iFumigationRepository.ApprovedDamageImage(entity);
        }
        #endregion

        #region get fumigation detail by id for copy
        /// <summary>
        /// Get proof Of delivery
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        public CopyFumigationDTO GetFumigationDetailById(int fumigationId)
        {
            return iFumigationRepository.GetFumigationDetailById(fumigationId);
        }
        #endregion


        #region Copy Fumigation
        /// <summary>
        /// Copy Fumigation detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public bool SaveCopyFumigatonDetail(CopyFumigationDTO entity)
        {
            return iFumigationRepository.SaveCopyFumigatonDetail(entity);
        }
        #endregion

        #region Get max route no. 
        /// <summary>
        /// Get max route no.
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        public int? GetMaxRouteNo(int fumigationId)
        {
            return iFumigationRepository.GetMaxRouteNo(fumigationId);
        }



        #endregion

        public TemperatureEmailDTO GetTemperatureEmailDetail(int fumigaitonId)
        {
            return iFumigationRepository.GetTemperatureEmailDetail(fumigaitonId);
        }
    }
}
