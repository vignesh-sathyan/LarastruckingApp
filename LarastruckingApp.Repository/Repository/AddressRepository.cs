using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LarastruckingApp.Repository.Repository
{
    public class AddressRepository : IAddressRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities addressContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Defining contrructor
        /// </summary>
        public AddressRepository()
        {
            addressContext = new LarastruckingDBEntities();
        }
        #endregion

        #region Address List
        /// <summary>
        /// List of Address
        /// </summary>
        public IEnumerable<AddressDTO> List
        {
            get
            {
                try
                {


                    var data = (from Add in addressContext.tblAddresses
                                join AddType in addressContext.tblAddressTypes on Add.AddressTypeId equals AddType.AddressTypeID

                                join State in addressContext.tblStates on Add.State equals State.ID
                                where Add.IsDeleted == false
                                select new AddressDTO
                                {
                                    AddressId = Add.AddressId,
                                    AddressTypeId = Add.AddressTypeId,
                                    Address1 = Add.Address1 ?? string.Empty,
                                    Address2 = Add.Address2 ?? string.Empty,
                                    City = Add.City ?? string.Empty,
                                    Country = Add.Country,
                                    CreatedBy = Add.CreatedBy,
                                    CreatedOn = Add.CreatedOn,
                                    IsActive = Add.IsActive,
                                    IsDeleted = Add.IsDeleted,
                                    ModifiedBy = Add.ModifiedBy,
                                    ModifiedOn = Add.ModifiedOn,
                                    StateName = State.Name ?? string.Empty,
                                    Zip = Add.Zip ?? string.Empty,
                                    AddressTypeName = AddType.AddressTypeName ?? string.Empty,
                                    Email = Add.Email ?? string.Empty,
                                    Phone = Add.Phone ?? string.Empty,
                                    CompanyName = Add.CompanyName ?? string.Empty,
                                    AdditionalPhone1 = Add.AdditionalPhone1 ?? string.Empty,
                                    Extension1 = Add.Extension1 ?? string.Empty,
                                    AdditionalPhone2 = Add.AdditionalPhone2 ?? string.Empty,
                                    Extension2 = Add.Extension2 ?? string.Empty,
                                    IsAppointmentRequired = Add.IsAppointmentRequired,
                                    Extension=Add.Extension,
                                    CompanyNickname=Add.CompanyNickname??string.Empty,
                                }
                        ).OrderByDescending(s => s.AddressId);
                    return data.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion

        #region Address BindAddressType
        /// <summary>
        /// Bind Address Type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AddressTypeDTO> BindAddressType()
        {
            try
            {
                var addressTypes = addressContext.tblAddressTypes.OrderBy(o => o.AddressTypeName).ToList();
                IEnumerable<AddressTypeDTO> lstAddressTypeDTO = AutoMapperServices<tblAddressType, AddressTypeDTO>.ReturnObjectList(addressTypes);
                return lstAddressTypeDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Address Add
        /// <summary>
        /// Add Address
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AddressDTO Add(AddressDTO entity)
        {
            try
            {


                AddressDTO objAddressDTO = new AddressDTO();
                tblAddress objtblAddress = AutoMapperServices<AddressDTO, tblAddress>.ReturnObject(entity);
                addressContext.tblAddresses.Add(objtblAddress);
                objAddressDTO = AutoMapperServices<tblAddress, AddressDTO>.ReturnObject(objtblAddress);

                objAddressDTO.IsSuccess = addressContext.SaveChanges() > 0 ? true : false;
                objAddressDTO.AddressId = objtblAddress.AddressId;
                return objAddressDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Address Delete
        /// <summary>
        /// Delete Address
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(AddressDTO entity)
        {
            try
            {

                bool result = false;
                var table = addressContext.tblAddresses.Find(entity.AddressId);
                if (table != null)
                {
                    table.IsDeleted = true;
                    table.IsActive = false;
                    addressContext.Entry(table).State = EntityState.Modified;
                    result = addressContext.SaveChanges() > 0 ? true : false;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Address Update
        /// <summary>
        /// Update Address 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AddressDTO Update(AddressDTO entity)
        {
            try
            {


                AddressDTO objAddressDTO = new AddressDTO();
                var address = addressContext.tblAddresses.Where(x => x.AddressId == entity.AddressId).FirstOrDefault();
                if (address != null)
                {
                    address.AddressTypeId = entity.AddressTypeId;
                    address.Address1 = entity.Address1;
                    address.Address2 = entity.Address2;
                    address.City = entity.City;
                    address.State = entity.State;
                    address.Country = entity.Country;
                    address.Zip = entity.Zip;
                    address.Phone = entity.Phone;
                    address.Email = entity.Email;
                    address.ContactPerson = entity.ContactPerson;
                    address.Extension = entity.Extension;

                    address.CompanyName = entity.CompanyName;
                    address.AdditionalPhone1 = entity.AdditionalPhone1;
                    address.Extension1 = entity.Extension1;
                    address.AdditionalPhone2 = entity.AdditionalPhone2;
                    address.Extension2 = entity.Extension2;
                    address.Comments = entity.Comments;
                    address.IsActive = entity.IsActive;
                    address.ModifiedBy = entity.ModifiedBy;
                    address.ModifiedOn = entity.ModifiedOn;
                    address.IsAppointmentRequired = entity.IsAppointmentRequired;
                    address.Website = entity.Website;
                    address.CompanyNickname = entity.CompanyNickname;
                    addressContext.Entry(address).State = EntityState.Modified;
                    objAddressDTO = AutoMapperServices<tblAddress, AddressDTO>.ReturnObject(address);
                    objAddressDTO.IsSuccess = addressContext.SaveChanges() > 0 ? true : false;
                }
                return objAddressDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Address FindById
        /// <summary>
        /// Find Address By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AddressDTO FindById(int Id)
        {
            try
            {


                AddressDTO objAddressDTO = null;
                var resultobjAddress = (from r in addressContext.tblAddresses where r.AddressId == Id select r).FirstOrDefault();
                if (resultobjAddress != null)
                {
                    objAddressDTO = AutoMapperServices<tblAddress, AddressDTO>.ReturnObject(resultobjAddress);
                }
                return objAddressDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Find Address auto-complete
        /// <summary>
        /// List of Address
        /// </summary>
        public IList<AddressDTO> GetAddress(string address)
        {
            try
            {
                var data = (from Add in addressContext.tblAddresses
                            join AddType in addressContext.tblAddressTypes on Add.AddressTypeId equals AddType.AddressTypeID
                            join State in addressContext.tblStates on Add.State equals State.ID
                            where (
                                    (Add.CompanyNickname.Trim().ToLower().Contains(address.ToLower().Trim()) ||
                                        Add.CompanyName.ToLower().Trim().Contains(address.ToLower().Trim()) ||
                                        Add.Address1.ToLower().Trim().Contains(address.ToLower().Trim()) ||
                                        Add.Address2.ToLower().Trim().Contains(address.ToLower().Trim()) ||
                                        Add.City.ToLower().Trim().Contains(address.ToLower().Trim()) ||
                                        State.Name.ToLower().Trim().Contains(address.ToLower().Trim())
                                       
                                    )
                                    && Add.IsDeleted == false
                             )
                            select new AddressDTO
                            {
                                CompanyNickname=Add.CompanyNickname,
                                CompanyName = Add.CompanyName ?? string.Empty,
                                AddressId = Add.AddressId,
                                AddressTypeId = Add.AddressTypeId,
                                Address1 = Add.Address1 ?? string.Empty,
                                Address2 = Add.Address2 ?? string.Empty,
                                City = Add.City ?? string.Empty,
                                Country = Add.Country,
                                CreatedBy = Add.CreatedBy,
                                CreatedOn = Add.CreatedOn,
                                IsActive = Add.IsActive,
                                IsDeleted = Add.IsDeleted,
                                ModifiedBy = Add.ModifiedBy,
                                ModifiedOn = Add.ModifiedOn,
                                StateName = State.Name.ToUpper() ?? string.Empty,
                                Zip = Add.Zip,
                                AddressTypeName = AddType.AddressTypeName ?? string.Empty,
                                Email = Add.Email ?? string.Empty,
                                Phone = Add.Phone ?? string.Empty,
                                IsAppointmentRequired = Add.IsAppointmentRequired,
                                Website = Add.Website ?? string.Empty,
                            }
                        ).OrderByDescending(s => s.AddressId);
                return data.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
