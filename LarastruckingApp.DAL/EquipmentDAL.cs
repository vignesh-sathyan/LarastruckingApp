using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Equipments;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class EquipmentDAL : IEquipmentDAL
    {
        /// <summary>
        /// Equipment Data Access Layer
        /// </summary>
        private readonly IEquipmentRepository iEquipmentRepository;
        /// <summary>
        /// Equipment Constructor
        /// </summary>
        /// <param name="iEquipmentRepository"></param>
        public EquipmentDAL(IEquipmentRepository iEquipmentRepository)
        {
            this.iEquipmentRepository = iEquipmentRepository;
        }
        /// <summary>
        /// Equipment Data List
        /// </summary>
        public IEnumerable<EquipmentDTO> List
        {
            get
            {
                return iEquipmentRepository.List;
            }
        }
        public IEnumerable<VehicleTypeDTO> BindVehicleType()
        {
            return iEquipmentRepository.BindVehicleType();
        }
        /// <summary>
        ///Add Equipment Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EquipmentDTO Add(EquipmentDTO entity)
        {
            return iEquipmentRepository.Add(entity);
        }
        /// <summary>
        ///Delete Equipment Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(EquipmentDTO entity)
        {
            return iEquipmentRepository.Delete(entity);
        }
        /// <summary>
        /// Find Equipment Data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EquipmentDTO FindById(int id)
        {
            return iEquipmentRepository.FindById(id);
        }
        /// <summary>
        /// Update Equipment Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EquipmentDTO Update(EquipmentDTO entity)
        {
            return iEquipmentRepository.Update(entity);
        }


        #region Check Duplicate Equipment No.
        /// <summary>
        /// Check Duplicate Equipment No.
        /// </summary>
        /// <param name="equipmentNo"></param>
        /// <returns></returns>
        public bool CheckEquipmentNo(string equipmentNo,int equipmentId)
        {
            return iEquipmentRepository.CheckEquipmentNo(equipmentNo,equipmentId);
        }
        #endregion

        #region Search by License Plate 
        /// <summary>
        /// Search by License Plate for autocomplete in Equipment
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>     
        public IEnumerable<EquipmentDTO> SearchByLicensePlate(string search)
        {
            return iEquipmentRepository.SearchByLicensePlate(search);
        }
        #endregion

        #region Search by VIN 
        /// <summary>
        /// Search by License Plate for autocomplete in VIN
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>   
        public IEnumerable<EquipmentDTO> SearchByVIN(string search)
        {
            return iEquipmentRepository.SearchByVIN(search);
        }
        #endregion

        #region Search by VIN 
        /// <summary>
        /// Search by License Plate for autocomplete in Equipment
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<EquipmentDTO> SearchByEquipmentNo(string search)
        {
            return iEquipmentRepository.SearchByEquipmentNo(search);
        }

        #endregion

        #region Get DoorType
        /// <summary>
        /// Get List of doortypes
        /// </summary>
        /// <returns></returns>
        public List<DoorTypeDto> GetDoorTypes()
        {
            return iEquipmentRepository.GetDoorTypes();
        }
        #endregion

    }
}
