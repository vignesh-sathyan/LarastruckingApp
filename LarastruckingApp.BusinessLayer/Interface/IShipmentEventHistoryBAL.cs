using LarastruckingApp.Entities.ShipmentEventHistoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
   public interface IShipmentEventHistoryBAL
    {
        bool SaveEventDetail(ShipmentEventHistoryDTO entity);
    }
}
