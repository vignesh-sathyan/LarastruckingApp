using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.GpsTracker;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class GpsTrackingDAL : IGpsTrackingDAL
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>

        private readonly IGpsTrackingRepository IGpsTrackingRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="IDriverModuleDAL"></param>

        public GpsTrackingDAL(IGpsTrackingRepository iGpsTrackingRepository)
        {
           this.IGpsTrackingRepository = iGpsTrackingRepository;
        }
        #endregion

        #region Get Shipment Details and Gps Tracking Details 
        /// <summary>
        ///  Get Shipment Details and Gps Tracking Details 
        /// </summary>
        /// <returns></returns>
        public GpsShipmentDetailsDTO GetShipmentDetails(int shipmentId)
        {
            return IGpsTrackingRepository.GetShipmentDetails(shipmentId);
        }
        #endregion

        #region Gps Tracking Details 
        /// <summary>
        /// Gps Tracking Details 
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<GpsTrackerHistoryDTO> GetGpsTrackerDetails(GpsTrackerHistoryDTO dto)
        {
            return IGpsTrackingRepository.GetGpsTrackerDetails(dto);
        }
        #endregion

        #region Get Fumigation Details and Gps Tracking Details 
        /// <summary>
        ///  Get Shipment Details and Gps Tracking Details 
        /// </summary>
        /// <returns></returns>
        public GpsFumigationDetailsDTO GetFumigationDetails(int FumigationId)
        {
            return IGpsTrackingRepository.GetFumigationDetails(FumigationId);
        }
        #endregion

        #region Gps Fumigation Tracking Details 
        /// <summary>
        /// Gps Tracking Details 
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<GpsTrackerHistoryDTO> GetFumigationGpsTrackerDetails(GpsTrackerHistoryDTO dto)
        {
            return IGpsTrackingRepository.GetGpsTrackerDetails(dto);
        }
        #endregion
    }
}
