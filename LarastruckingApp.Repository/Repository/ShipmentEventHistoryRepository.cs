using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.ShipmentEventHistoryDTO;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class ShipmentEventHistoryRepository : IShipmentEventHistoryRepository
    {
        #region private member
        private readonly LarastruckingDBEntities shipmentEventHistoryContext;
        #endregion

        #region constroctor
        public ShipmentEventHistoryRepository()
        {
            shipmentEventHistoryContext = new LarastruckingDBEntities();
        }


        #endregion

        public bool SaveEventDetail(ShipmentEventHistoryDTO entity)
        {
            tblShipmentEventHistory objShipmentEventHistory = AutoMapperServices<ShipmentEventHistoryDTO, tblShipmentEventHistory>.ReturnObject(entity);
            shipmentEventHistoryContext.tblShipmentEventHistories.Add(objShipmentEventHistory);
            return shipmentEventHistoryContext.SaveChanges() > 0;
        }

    }
}
