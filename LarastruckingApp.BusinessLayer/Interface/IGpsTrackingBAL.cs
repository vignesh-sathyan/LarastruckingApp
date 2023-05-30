using LarastruckingApp.Entities;
using LarastruckingApp.Entities.GpsTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
    public interface IGpsTrackingBAL
    {
        #region Get Shipment Details and Gps Tracking Details 
        /// <summary>
        /// Get Shipment Details and Gps Tracking Details
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        GpsShipmentDetailsDTO GetShipmentDetails(int shipmentId);
        #endregion

        #region Gps Tracking Details 
        /// <summary>
        /// Gps Tracking Details 
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<GpsTrackerHistoryDTO> GetGpsTrackerDetails(GpsTrackerHistoryDTO dto);
        #endregion

        #region Get Fumigation Details and Gps Tracking Details
        /// <summary>
        /// Get Fumigation Details and Gps Tracking Details
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        GpsFumigationDetailsDTO GetFumigationDetails(int FumigationId);
        #endregion

        #region Gps Fumigation Tracking Details
        /// <summary>
        /// Gps Fumigation Tracking Details
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<GpsTrackerHistoryDTO> GetFumigationGpsTrackerDetails(GpsTrackerHistoryDTO dto);
        #endregion
    }
}
