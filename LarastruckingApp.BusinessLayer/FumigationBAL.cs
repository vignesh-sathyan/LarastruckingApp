using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Utility.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class FumigationBAL : IFumigationBAL
    {
        #region Private member
        /// <summary>
        /// Private member
        /// </summary>
        private IFumigationDAL iFumigationDAL;
        #endregion

        #region FumigationBAL
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iFumigationDAL"></param>
        public FumigationBAL(IFumigationDAL iFumigationDAL)
        {
            this.iFumigationDAL = iFumigationDAL;
        }


        #endregion

        #region GetFumigationTypeList
        /// <summary>
        /// Get Fumigation Type  List
        /// </summary>
        /// <returns></returns>
        public List<FumigationTypeDTO> GetFumigationTypeList()
        {
            return iFumigationDAL.GetFumigationTypeList();
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
            foreach (var route in entity.FumigationRouteDetail)
            {

                route.PickUpArrival = Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.PickUpArrival));
                route.DeliveryArrival = Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.DeliveryArrival));
                route.FumigationArrival = Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.FumigationArrival));
                route.ReleaseDate = (route.ReleaseDate == null ? route.ReleaseDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.ReleaseDate)));
                route.DepartureDate = (route.DepartureDate == null ? route.DepartureDate : Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.DepartureDate)));
                route.DriverDeliveryArrival = (route.DriverDeliveryArrival == null ? route.DriverDeliveryArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.DriverDeliveryArrival)));
                route.DriverFumigationIn = (route.DriverFumigationIn == null ? route.DriverFumigationIn : Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.DriverFumigationIn)));
                route.DriverDeliveryDeparture = (route.DriverDeliveryDeparture == null ? route.DriverDeliveryDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.DriverDeliveryDeparture)));
                route.DriverPickupArrival = (route.DriverPickupArrival == null ? route.DriverPickupArrival : Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.DriverPickupArrival)));
                route.DriverPickupDeparture = (route.DriverPickupDeparture == null ? route.DriverPickupDeparture : Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.DriverPickupDeparture)));
                route.DriverFumigationRelease = (route.DriverFumigationRelease == null ? route.DriverFumigationRelease : Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.DriverFumigationRelease)));
                route.DriverLoadingStartTime = (route.DriverLoadingStartTime == null ? route.DriverLoadingStartTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.DriverLoadingStartTime)));
                route.DriverLoadingFinishTime = (route.DriverLoadingFinishTime == null ? route.DriverLoadingFinishTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(route.DriverLoadingFinishTime)));
                if (route.TemperatureType == "C" && route.Temperature != null)
                {
                    route.Temperature = ConversionFormula.NullCelsiusToFahrenheit(route.Temperature);
                    route.TemperatureType = "F";
                }


            }

            return iFumigationDAL.CreateFumigation(entity);
        }
        #endregion

        #region get fumigation list
        /// <summary>
        /// get fumigation list
        /// </summary>
        /// <returns></returns>

        public IList<FumigationListDTO> GetFumigationList(DataTableFilterDto entity)
        {
            var result = iFumigationDAL.GetFumigationList(entity);
            if (result.Count > 0)
            {
                foreach (var route in result)
                {
                   // route.PickUpArrival = (route.PickUpArrival == null ? route.PickUpArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.PickUpArrival)));
                  //  route.FumigationArrival = (route.FumigationArrival == null ? route.FumigationArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.FumigationArrival)));
                   // route.DeliveryArrival = (route.DeliveryArrival == null ? route.PickUpArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.DeliveryArrival)));
                   // route.ReleaseDate = (route.ReleaseDate == null ? route.ReleaseDate : Configurations.ConvertDateTime(Convert.ToDateTime(route.ReleaseDate)));
                }
            }
            return result;
        }


        #endregion



        #region View All Fumigation
        /// <summary>
        /// View All Fumigation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IList<FumigationViewAllListDTO> ViewAllFumigation(AllShipmentDTO entity)
        {
            var result = iFumigationDAL.ViewAllFumigation(entity);
            if (result.Count > 0)
            {
                foreach (var route in result)
                {
                   // route.PickUpArrival = (route.PickUpArrival == null ? route.PickUpArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.PickUpArrival)));
                   // route.FumigationArrival = (route.FumigationArrival == null ? route.FumigationArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.FumigationArrival)));
                   // route.DeliveryArrival = (route.DeliveryArrival == null ? route.PickUpArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.DeliveryArrival)));
                   // route.ReleaseDate = (route.ReleaseDate == null ? route.ReleaseDate : Configurations.ConvertDateTime(Convert.ToDateTime(route.ReleaseDate)));
                }
            }
            return result;
        }
        #endregion
        #region get other fumigation list
        /// <summary>
        /// get other fumigation list
        /// </summary>
        /// <returns></returns>
        public IList<FumigationOtherList> GetOtherFumigationList(DataTableFilterDto entity)
        {
            var result = iFumigationDAL.GetOtherFumigationList(entity);
            //if (result.Count > 0)
            //{
            //    foreach (var route in result)
            //    {
            //        route.PickUpArrival = (route.PickUpArrival == null ? route.PickUpArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.PickUpArrival)));
            //        route.FumigationArrival = (route.FumigationArrival == null ? route.FumigationArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.FumigationArrival)));
            //        route.DeliveryArrival = (route.DeliveryArrival == null ? route.PickUpArrival : Configurations.ConvertDateTime(Convert.ToDateTime(route.DeliveryArrival)));
            //        route.ReleaseDate = (route.ReleaseDate == null ? route.ReleaseDate : Configurations.ConvertDateTime(Convert.ToDateTime(route.ReleaseDate)));
            //    }
            //}
            return result;
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
            return iFumigationDAL.DeleteFumigation(entity);
        }


        #endregion

        #region get fumigation by id 
        /// <summary>
        ///  get fumigation by id for edit fumigaion
        /// </summary>
        /// <returns></returns>
        public GetFumigationDTO GetFumigationById(int fumigationId)
        {
            return iFumigationDAL.GetFumigationById(fumigationId);
        }


        #endregion

        #region Upload Proof of Temp
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        public int UploadProofofTempDocument(List<FumigationProofOfTemprature> fumigationProofOfTemprature)
        {
            return iFumigationDAL.UploadProofofTempDocument(fumigationProofOfTemprature);
        }
        #endregion

        #region Upload Damage Document
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        public int UploadDamageDocument(List<FumigationDamageImages> damageImageList)
        {
            return iFumigationDAL.UploadDamageDocument(damageImageList);
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
            return iFumigationDAL.DeleteProofOfTemprature(model);
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
            return iFumigationDAL.DeleteDamageFile(model);
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
            foreach (var route in entity.FumigationRouteDetail)
            {
                if (route.TemperatureType == "C" && route.Temperature != null)
                {
                    route.Temperature = ConversionFormula.NullCelsiusToFahrenheit(route.Temperature);
                    route.TemperatureType = "F";
                }
            }
            return iFumigationDAL.EditFumigation(entity);
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
            if (model.TemperatureType == "C" && model.Temperature != null)
            {
                model.Temperature = ConversionFormula.NullCelsiusToFahrenheit(model.Temperature);
                model.TemperatureType = "F";
            }
            return iFumigationDAL.AddRouteStops(model);
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
            return iFumigationDAL.ApprovedFumigation(entity);
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
            return iFumigationDAL.GetCustomerDetail(fumigationEmailDTO);
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
            return iFumigationDAL.FumigationProofOfDelivery(fumigationId);
        }


        #endregion

        #region fumigation status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>

        public List<ShipmentStatusDTO> GetStatusList()
        {
            return iFumigationDAL.GetStatusList();
        }


        #endregion

        #region fumigation Driver status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>

        public ShipmentStatusDTO GetDriverStatusList()
        {
            return iFumigationDAL.GetDriverStatusList();
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
            return iFumigationDAL.ApprovedProofOFTemp(entity);
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
            return iFumigationDAL.ApprovedDamageImage(entity);
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
            return iFumigationDAL.GetFumigationDetailById(fumigationId);
        }
        #endregion
        
        #region Copy Fumigation
        /// <summary>
        /// Copy Fumigation detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public  bool SaveCopyFumigatonDetail(CopyFumigationDTO entity)
        {
            return iFumigationDAL.SaveCopyFumigatonDetail(entity);
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
            return iFumigationDAL.GetMaxRouteNo(fumigationId);
        }



        #endregion

        
        public TemperatureEmailDTO GetTemperatureEmailDetail(int fumigationId)
        {
            return iFumigationDAL.GetTemperatureEmailDetail(fumigationId);
        }

        public bool FumigationWTReady(int shipmentId, bool ready)
        {
            return iFumigationDAL.FumigationWTReady(shipmentId, ready);
        }

        public bool FumigationSTReady(int shipmentId, bool ready)
        {
            return iFumigationDAL.FumigationSTReady(shipmentId, ready);
        }

        #region Delete Comments
        /// <summary>
        ///Delete shipment By Id
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public bool DeleteComments(FumigationDTO entity)
        {
            return iFumigationDAL.DeleteComments(entity);
        }


        #endregion

        #region Get OrderTaken
        public int GetOrderTaken()
        {
            return iFumigationDAL.GetOrderTaken();
        }

        #endregion

        #region Get FumigationInProgress
        public int GetFumigationInProgress()
        {
            return iFumigationDAL.GetFumigationInProgress();
        }

        #endregion



        #region Get CustomerDetail
        public CustomerDetailDTO CustomerDetail(int fumigationid)
        {
            return iFumigationDAL.CustomerDetail(fumigationid);
        }

        #endregion
    }
}
