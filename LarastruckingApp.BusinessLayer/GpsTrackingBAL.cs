using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.GpsTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class GpsTrackingBAL : IGpsTrackingBAL
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly IGpsTrackingDAL IGpsTrackingDAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="IDriverModuleDAL"></param>
        public GpsTrackingBAL(IGpsTrackingDAL iGpsTrackingDAL)
        {
            this.IGpsTrackingDAL = iGpsTrackingDAL;
        }
        #endregion

        #region Get Shipment Details and Gps Tracking Details 
        /// <summary>
        /// Get Shipment Details and Gps Tracking Details 
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public GpsShipmentDetailsDTO GetShipmentDetails(int shipmentId)
        {
            return IGpsTrackingDAL.GetShipmentDetails(shipmentId);
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
            return IGpsTrackingDAL.GetGpsTrackerDetails(dto);
        }
        #endregion

        #region Get Fumigation Details and Gps Tracking Details 
        /// <summary>
        ///  Get Shipment Details and Gps Tracking Details 
        /// </summary>
        /// <returns></returns>
        public GpsFumigationDetailsDTO GetFumigationDetails(int FumigationId)
        {
            return IGpsTrackingDAL.GetFumigationDetails(FumigationId);
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
            return IGpsTrackingDAL.GetGpsTrackerDetails(dto);
        }
        #endregion
    }
}
