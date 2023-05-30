using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Entities.ShipmentEventHistoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
   public class ShipmentEventHistoryBAL: IShipmentEventHistoryBAL
    {
        #region private member
        readonly IShipmentEventHistoryBAL shipmentEventHistoryBAL = null;
        #endregion

        #region constructor
        public ShipmentEventHistoryBAL(IShipmentEventHistoryBAL iShipmentEventHistoryBAL)
        {
            shipmentEventHistoryBAL = iShipmentEventHistoryBAL;
        }

        #endregion
        #region save event detail
        /// <summary>
        /// Save event detail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveEventDetail(ShipmentEventHistoryDTO entity)
        {
            return shipmentEventHistoryBAL.SaveEventDetail(entity);
        }
        #endregion

    }
}
