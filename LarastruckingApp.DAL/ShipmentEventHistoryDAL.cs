using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities.ShipmentEventHistoryDTO;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class ShipmentEventHistoryDAL : IShipmentEventHistoryDAL
    {

        #region Private Member
        private IShipmentEventHistoryRepository shipmentEventHistoryRepository;
        #endregion

        #region Constructor 
        public ShipmentEventHistoryDAL(IShipmentEventHistoryRepository iShipmentEventHistoryRepository)
        {
            shipmentEventHistoryRepository = iShipmentEventHistoryRepository;
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
            return shipmentEventHistoryRepository.SaveEventDetail(entity);
        }
        #endregion
    }
}
