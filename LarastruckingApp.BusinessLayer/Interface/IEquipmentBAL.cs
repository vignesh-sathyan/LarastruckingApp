﻿using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
    public interface IEquipmentBAL : ICommonBAL<EquipmentDTO>
    {
        #region Get DoorType
        List<DoorTypeDto> GetDoorTypes();
        #endregion

        IEnumerable<VehicleTypeDTO> BindVehicleType();
        IEnumerable<EquipmentDTO> SearchByLicensePlate(string search);
        IEnumerable<EquipmentDTO> SearchByVIN(string search);
        IEnumerable<EquipmentDTO> SearchByEquipmentNo(string search);
        bool CheckEquipmentNo(string equipmentNo,int equipmentId);
    }
}
