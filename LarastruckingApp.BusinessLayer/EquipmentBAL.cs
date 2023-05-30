using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LarastruckingApp.BusinessLayer
{
    public class EquipmentBAL : IEquipmentBAL
    {
        #region Private Member
        /// <summary>
        /// Private member declaration
        /// </summary>
        private IEquipmentDAL iEquipmentDAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iEquipmentDAL"></param>
        public EquipmentBAL(IEquipmentDAL iEquipmentDAL)
        {
            this.iEquipmentDAL = iEquipmentDAL;
        }
        #endregion

        #region List
        /// <summary>
        /// List
        /// </summary>
        public IEnumerable<EquipmentDTO> List
        {
            get
            {
                return iEquipmentDAL.List;
            }
        }
        #endregion

        #region BindVehicleType
        public IEnumerable<VehicleTypeDTO> BindVehicleType()
        {

            return iEquipmentDAL.BindVehicleType();

        }
        #endregion

        #region Add
        /// <summary>
        /// Add Equipment Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EquipmentDTO Add(EquipmentDTO entity)
        {
            return iEquipmentDAL.Add(entity);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Equipment Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(EquipmentDTO entity)
        {
            return iEquipmentDAL.Delete(entity);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Equipment Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EquipmentDTO Update(EquipmentDTO entity)
        {
            return iEquipmentDAL.Update(entity);
        }
        #endregion

        #region FindById
        /// <summary>
        /// Find Equipment Data By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EquipmentDTO FindById(int id)
        {
            return iEquipmentDAL.FindById(id);
        }
        #endregion

        #region SearchByLicensePlate
        /// <summary>
        /// Search By VIN
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<EquipmentDTO> SearchByLicensePlate(string search)
        {
            return iEquipmentDAL.SearchByLicensePlate(search);
        }
        #endregion

        #region SearchByVIN
        /// <summary>
        /// Search By VIN
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<EquipmentDTO> SearchByVIN(string search)
        {
            return iEquipmentDAL.SearchByVIN(search);
        }
        #endregion

        #region SearchByEquipmentNo
        /// <summary>
        /// Search By Equipment No 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<EquipmentDTO> SearchByEquipmentNo(string search)
        {
            return iEquipmentDAL.SearchByEquipmentNo(search);
        }
        #endregion

        #region Check Duplicate Equipment No.
        /// <summary>
        /// Check Duplicate Equipment No.
        /// </summary>
        /// <param name="equipmentNo"></param>
        /// <returns></returns>
        public bool CheckEquipmentNo(string equipmentNo,int equipmentId)
        {
            return iEquipmentDAL.CheckEquipmentNo(equipmentNo, equipmentId);
        }
        #endregion

        #region Get DoorType
        /// <summary>
        /// Get List of doortypes
        /// </summary>
        /// <returns></returns>
        public List<DoorTypeDto> GetDoorTypes()
        {
            return iEquipmentDAL.GetDoorTypes();
        }
        #endregion

    }
}
