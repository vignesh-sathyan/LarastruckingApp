using LarastruckingApp.Entities.ShipmentEventHistoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IShipmentEventHistoryRepository
    {
        bool SaveEventDetail(ShipmentEventHistoryDTO entity);
       
    }
}
