using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Equipments;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class EquipmentRepository : IEquipmentRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities equipmentContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EquipmentRepository()
        {
            equipmentContext = new LarastruckingDBEntities();
        }
        #endregion

        #region List
        /// <summary>
        /// List Of Equipment
        /// </summary>
        public IEnumerable<EquipmentDTO> List
        {
            get
            {
                try
                {


                    var data = (from Ted in equipmentContext.tblEquipmentDetails
                                join Et in equipmentContext.tblEquipmentTypes on Ted.VehicleType equals Et.VehicleTypeID
                                where Ted.IsDeleted == false
                                select new EquipmentDTO
                                {

                                    Model = Ted.Model ?? string.Empty,
                                    VehicleTypeName = Et.VehicleTypeName ?? string.Empty,
                                    VehicleType = Ted.VehicleType,
                                    EquipmentNo = Ted.EquipmentNo ?? string.Empty,
                                    Year = Ted.Year,
                                    VIN = Ted.VIN ?? string.Empty,
                                    MaxLoad = Ted.MaxLoad ?? string.Empty,
                                    RollerBed = Ted.RollerBed ?? string.Empty,
                                    LicencePlate = Ted.LicencePlate ?? string.Empty,
                                    CubicFeet = Ted.CubicFeet ?? string.Empty,
                                    Ownedby = Ted.Ownedby ?? string.Empty,
                                    Comments = Ted.Comments ?? string.Empty,
                                    Active = Ted.Active,
                                    IsDeleted = Ted.IsDeleted,
                                    CreatedOn = Ted.CreatedOn,
                                    EDID = Ted.EDID,
                                    CreatedBy = Ted.CreatedBy,
                                    LeaseCompanyName = Ted.LeaseCompanyName ?? string.Empty,
                                    HDimension = Ted.HDimension,
                                    LDimension = Ted.LDimension,
                                    WDimension = Ted.WDimension,
                                    QRCodeNo= Ted.QRCodeNo,
                                    FreightTypeList = (from equipmentfreighttype in equipmentContext.tblEquipmentFreights
                                                       join freighttype in equipmentContext.tblFreightTypes on equipmentfreighttype.FreightId equals freighttype.FreightTypeId
                                                       where equipmentfreighttype.EquipmentId == Ted.EDID
                                                       select

                                                          freighttype.FreightTypeName

                                                               ).ToList(),
                                }).OrderByDescending(s => s.EDID).ToList();


                    foreach (var equp in data)
                    {
                        if (equp.FreightTypeList.Count() > 0)
                        {
                            equp.FreightType = String.Join(",", equp.FreightTypeList);
                        }
                        else
                        {
                            equp.FreightType = string.Empty;
                        }
                    }
                    return data;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion

        #region SearchByLicensePlate
        /// <summary>
        /// Search By License Plate
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<EquipmentDTO> SearchByLicensePlate(string search)
        {
            try
            {

                var data = (from Ted in equipmentContext.tblEquipmentDetails
                            join Et in equipmentContext.tblEquipmentTypes on Ted.VehicleType equals Et.VehicleTypeID
                            where Ted.IsDeleted == false && Ted.LicencePlate.ToLower().Trim().Contains(search.ToLower().Trim())
                            select new EquipmentDTO
                            {
                                EDID = Ted.EDID,
                                EquipmentNo = Ted.EquipmentNo,
                                Model = Ted.Model,
                                VehicleTypeName = Et.VehicleTypeName,
                                VehicleType = Ted.VehicleType,
                                VIN = Ted.VIN,
                                Year = Ted.Year,
                                LicencePlate = Ted.LicencePlate,
                            }).OrderBy(s => s.VehicleType).ToList();
                return data;

            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                var data = (from Ted in equipmentContext.tblEquipmentDetails
                            join Et in equipmentContext.tblEquipmentTypes on Ted.VehicleType equals Et.VehicleTypeID
                            where Ted.IsDeleted == false && Ted.VIN.ToLower().Trim().Contains(search.ToLower().Trim())
                            select new EquipmentDTO
                            {
                                EDID = Ted.EDID,
                                EquipmentNo = Ted.EquipmentNo,
                                Model = Ted.Model,
                                VehicleTypeName = Et.VehicleTypeName,
                                VehicleType = Ted.VehicleType,
                                VIN = Ted.VIN,
                                Year = Ted.Year,
                                LicencePlate = Ted.LicencePlate,
                            }).OrderBy(s => s.VIN).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {
                var data = (from Ted in equipmentContext.tblEquipmentDetails
                            join Et in equipmentContext.tblEquipmentTypes on Ted.VehicleType equals Et.VehicleTypeID
                            where Ted.IsDeleted == false && Ted.EquipmentNo.ToLower().Trim().Contains(search.ToLower().Trim())
                            select new EquipmentDTO
                            {
                                EDID = Ted.EDID,
                                EquipmentNo = Ted.EquipmentNo,
                                Model = Ted.Model,
                                VehicleTypeName = Et.VehicleTypeName,
                                VehicleType = Ted.VehicleType,
                                VIN = Ted.VIN,
                                Year = Ted.Year,
                                LicencePlate = Ted.LicencePlate,
                            }).OrderBy(s => s.VIN).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region BindVehicleType
        public IEnumerable<VehicleTypeDTO> BindVehicleType()
        {
            try
            {
                IEnumerable<VehicleTypeDTO> lstVehicleTypeDTO = AutoMapperServices<tblEquipmentType, VehicleTypeDTO>.ReturnObjectList(equipmentContext.tblEquipmentTypes.ToList());
                return lstVehicleTypeDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Add
        /// <summary>
        /// Add Equipment
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EquipmentDTO Add(EquipmentDTO entity)
        {
            try
            {


                #region ADD: Equipment
                EquipmentDTO objEquipmentDTO = new EquipmentDTO();
                tblEquipmentDetail objtblEquipmentDetail = AutoMapperServices<EquipmentDTO, tblEquipmentDetail>.ReturnObject(entity);
                equipmentContext.tblEquipmentDetails.Add(objtblEquipmentDetail);
                objEquipmentDTO = AutoMapperServices<tblEquipmentDetail, EquipmentDTO>.ReturnObject(objtblEquipmentDetail);
                #endregion

                #region ADD: Equipment Freight Map

                List<tblEquipmentFreight> listFreights = new List<tblEquipmentFreight>();
                if (entity.FreightTypeIds != null)
                {


                    foreach (var freight in entity.FreightTypeIds)
                    {
                        tblEquipmentFreight tblEquipmentFreight = new tblEquipmentFreight()
                        {
                            EquipmentId = objtblEquipmentDetail.EDID,
                            FreightId = freight
                        };
                        listFreights.Add(tblEquipmentFreight);
                    }
                    equipmentContext.tblEquipmentFreights.AddRange(listFreights);
                }
                #endregion

                #region ADD: Equipment Door Type Map
                List<tblEquipmentDoorType> listDoortypes = new List<tblEquipmentDoorType>();
                if (entity.DoorTypeIds != null)
                {
                    foreach (var doorId in entity.DoorTypeIds)
                    {
                        tblEquipmentDoorType tblEquipmentDoorType = new tblEquipmentDoorType()
                        {
                            EquipmentId = objtblEquipmentDetail.EDID,
                            DoorTypeId = doorId
                        };
                        listDoortypes.Add(tblEquipmentDoorType);
                    }
                    equipmentContext.tblEquipmentDoorTypes.AddRange(listDoortypes);
                }
                #endregion

                objEquipmentDTO.IsSuccess = equipmentContext.SaveChanges() > 0;
                return objEquipmentDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Equipment
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(EquipmentDTO entity)
        {
            try
            {


                bool result = false;
                var table = equipmentContext.tblEquipmentDetails.Find(entity.EDID);
                if (table != null)
                {
                    table.IsDeleted = true;
                    table.Active = false;
                    equipmentContext.Entry(table).State = System.Data.Entity.EntityState.Modified;
                    result = equipmentContext.SaveChanges() > 0;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region FindById
        /// <summary>
        /// Find Equipment Data By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EquipmentDTO FindById(int Id)
        {
            try
            {

                EquipmentDTO objEquipmentDTO = null;
                var resultEquipment = (from r in equipmentContext.tblEquipmentDetails where r.EDID == Id select r).FirstOrDefault();

                var freightTypeIds = equipmentContext.tblEquipmentFreights
                                            .Where(r => r.EquipmentId == Id)
                                            .Select(n => n.FreightId).ToList();

                var doorTypeIds = equipmentContext.tblEquipmentDoorTypes
                                            .Where(r => r.EquipmentId == Id)
                                            .Select(n => n.DoorTypeId).ToList();


                if (resultEquipment != null)
                {
                    objEquipmentDTO = AutoMapperServices<tblEquipmentDetail, EquipmentDTO>.ReturnObject(resultEquipment);
                    objEquipmentDTO.FreightTypeIds = freightTypeIds;
                    objEquipmentDTO.DoorTypeIds = doorTypeIds;
                }
                return objEquipmentDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Equipment
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EquipmentDTO Update(EquipmentDTO entity)
        {
            try
            {
                #region UPDATE: Equipments
                var objEquipment = equipmentContext.tblEquipmentDetails.Find(entity.EDID);

                if (objEquipment != null)
                {
                    objEquipment.Decal = entity.Decal;
                    objEquipment.VehicleType = entity.VehicleType;
                    objEquipment.Model = entity.Model;
                    objEquipment.LicencePlate = entity.LicencePlate;
                    objEquipment.EquipmentNo = entity.EquipmentNo;
                    objEquipment.WDimension = entity.WDimension;
                    objEquipment.HDimension = entity.HDimension;
                    objEquipment.LDimension = entity.LDimension;
                    objEquipment.Make = entity.Make;
                    objEquipment.VIN = entity.VIN;
                    objEquipment.CubicFeet = entity.CubicFeet;
                    objEquipment.Ownedby = entity.Ownedby;
                    objEquipment.Comments = entity.Comments;
                    objEquipment.ModifiedBy = entity.ModifiedBy;
                    objEquipment.ModifiedOn = entity.ModifiedOn;
                    objEquipment.LeaseCompanyName = entity.LeaseCompanyName;
                    objEquipment.RegistrationExpiration = entity.RegistrationExpiration;
                    objEquipment.Year = entity.Year;
                    objEquipment.Color = entity.Color;
                    objEquipment.LeaseStartDate = entity.LeaseStartDate;
                    objEquipment.LeaseEndDate = entity.LeaseEndDate;
                    objEquipment.RegistrationImageURL = entity.RegistrationImageURL;
                    objEquipment.RegistrationImageName = entity.RegistrationImageName;
                    objEquipment.InsauranceImageName = entity.InsauranceImageName;
                    objEquipment.InsuranceImageURL = entity.InsuranceImageURL;
                    objEquipment.Active = entity.Active;
                    objEquipment.IsOutOfService = entity.IsOutOfService;
                    objEquipment.OutOfServiceStartDate = entity.OutOfServiceStartDate;
                    objEquipment.OutOfServiceEndDate = entity.OutOfServiceEndDate;
                    objEquipment.MaxLoad = entity.MaxLoad;
                    objEquipment.RollerBed = entity.RollerBed;
                    objEquipment.QRCodeNo = entity.QRCodeNo;
                }
                #endregion

                #region REMOVE: Existing Freight
                var equipmentFreights = equipmentContext.tblEquipmentFreights.Where(e => e.EquipmentId == entity.EDID).ToList();

                if (equipmentFreights.Count > 0)
                {
                    equipmentContext.tblEquipmentFreights.RemoveRange(equipmentFreights);
                }
                #endregion

                #region UPDATE: Equipment Freight Map
                List<tblEquipmentFreight> listFreights = new List<tblEquipmentFreight>();

                foreach (var freight in entity.FreightTypeIds)
                {
                    tblEquipmentFreight tblEquipmentFreight = new tblEquipmentFreight()
                    {
                        EquipmentId = entity.EDID,
                        FreightId = freight
                    };
                    listFreights.Add(tblEquipmentFreight);
                }
                equipmentContext.tblEquipmentFreights.AddRange(listFreights);
                #endregion

                #region REMOVE: Existing Door Type 
                var equipmentDoorTypes = equipmentContext.tblEquipmentDoorTypes.Where(e => e.EquipmentId == entity.EDID).ToList();

                if (equipmentDoorTypes.Count > 0)
                {
                    equipmentContext.tblEquipmentDoorTypes.RemoveRange(equipmentDoorTypes);
                }
                #endregion

                #region UPDATE: Equipment DoorType Map  
                List<tblEquipmentDoorType> listDoortypes = new List<tblEquipmentDoorType>();

                foreach (var doorId in entity.DoorTypeIds)
                {
                    tblEquipmentDoorType tblEquipmentDoorType = new tblEquipmentDoorType()
                    {
                        EquipmentId = entity.EDID,
                        DoorTypeId = doorId
                    };
                    listDoortypes.Add(tblEquipmentDoorType);
                }

                equipmentContext.tblEquipmentDoorTypes.AddRange(listDoortypes);
                #endregion

                int row = equipmentContext.SaveChanges();

                if (row > 0)
                {
                    entity.IsSuccess = true;
                }
                else
                {
                    entity.IsSuccess = false;
                }

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Check Duplicate Equipment No.
        /// <summary>
        /// Check Duplicate Equipment No.
        /// </summary>
        /// <param name="equipmentNo"></param>
        /// <returns></returns>
        public bool CheckEquipmentNo(string equipmentNo, int equipmentId)
        {
            try
            {


                if (equipmentId > 0)
                {
                    return equipmentContext.tblEquipmentDetails.Any(e => e.EquipmentNo.Equals(equipmentNo, StringComparison.InvariantCultureIgnoreCase) && e.EDID != equipmentId && e.IsDeleted == false);
                }
                else
                {
                    return equipmentContext.tblEquipmentDetails.Any(e => e.EquipmentNo.Equals(equipmentNo, StringComparison.InvariantCultureIgnoreCase) && e.IsDeleted == false);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region Get DoorType
        /// <summary>
        /// Get List of doortypes
        /// </summary>
        /// <returns></returns>
        public List<DoorTypeDto> GetDoorTypes()
        {
            try
            {
                return equipmentContext.tblDoorTypes.Select(s => new DoorTypeDto
                {
                    DoorTypeId = s.DoorTypeId,
                    DoorType = s.DoorType
                }).ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
