using LarastruckingApp.Entities.ShipmentEventHistoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL.Interface
{
    public interface IShipmentEventHistoryDAL
    {
        bool SaveEventDetail(ShipmentEventHistoryDTO entity);
    }
}
