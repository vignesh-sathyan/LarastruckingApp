using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
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
    public class VendorRepository : IVendorRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities vendorContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Defining contrructor
        /// </summary>
        public VendorRepository()
        {
            vendorContext = new LarastruckingDBEntities();
        }
        #endregion

        #region Vendor Add
        /// <summary>
        /// Add Vendor
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public VendorNconsigneeDTO Add(VendorNconsigneeDTO entity)
        {
            try
            {
                VendorNconsigneeDTO objVendorNconsigneeDTO = new VendorNconsigneeDTO();
                tblVendorNconsignee objtblVendorNconsignee = AutoMapperServices<VendorNconsigneeDTO, tblVendorNconsignee>.ReturnObject(entity);
                objtblVendorNconsignee.IsActive = true;
                vendorContext.tblVendorNconsignees.Add(objtblVendorNconsignee);
                objVendorNconsigneeDTO = AutoMapperServices<tblVendorNconsignee, VendorNconsigneeDTO>.ReturnObject(objtblVendorNconsignee);

                objVendorNconsigneeDTO.IsSuccess = vendorContext.SaveChanges() > 0 ? true : false;
                objVendorNconsigneeDTO.VendorNconsigneeId = objtblVendorNconsignee.VendorNconsigneeId;
                return objVendorNconsigneeDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Vendor Delete
        /// <summary>
        /// Delete Vendor
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(VendorNconsigneeDTO entity)
        {
            try
            {

                bool result = false;
                var table = vendorContext.tblVendorNconsignees.Find(entity.VendorNconsigneeId);
                if (table != null)
                {
                    table.IsDeleted = true;
                    table.DeletedOn = Configurations.TodayDateTime;
                    vendorContext.Entry(table).State = EntityState.Modified;
                    result = vendorContext.SaveChanges() > 0 ? true : false;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Vendor Update
        /// <summary>
        /// Vendor Update 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public VendorNconsigneeDTO Update(VendorNconsigneeDTO entity)
        {
            try
            {
                VendorNconsigneeDTO objVendorNconsigneeDTO = new VendorNconsigneeDTO();
                var vendorNconsignee = vendorContext.tblVendorNconsignees.Where(x => x.VendorNconsigneeId == entity.VendorNconsigneeId).FirstOrDefault();
                if (vendorNconsignee != null)
                {
                    vendorNconsignee.VendorNconsigneeName = entity.VendorNconsigneeName;
                    vendorNconsignee.Address = entity.Address;
                    vendorNconsignee.Country = entity.Country;
                    vendorNconsignee.State = entity.State;
                    vendorNconsignee.City = entity.City;
                    vendorNconsignee.Phone = entity.Phone;
                    vendorNconsignee.Fax = entity.Fax;
                    vendorNconsignee.Email = entity.Email;
                    vendorNconsignee.Zip = entity.Zip;
                    vendorNconsignee.IsActive = true;
                    vendorNconsignee.ModifyBy = entity.ModifyBy;
                    vendorNconsignee.ModifyOn = entity.ModifyOn;
                    vendorContext.Entry(vendorNconsignee).State = EntityState.Modified;
                    objVendorNconsigneeDTO = AutoMapperServices<tblVendorNconsignee, VendorNconsigneeDTO>.ReturnObject(vendorNconsignee);
                    objVendorNconsigneeDTO.IsSuccess = vendorContext.SaveChanges() > 0 ? true : false;
                }
                return objVendorNconsigneeDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Vendor FindById
        /// <summary>
        /// Find Vendor By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public VendorNconsigneeDTO FindById(int Id)
        {
            try
            {
                VendorNconsigneeDTO objVenodrNConsigneeDTO = null;
                var resultobjVendorNConsigneeName = (from r in vendorContext.tblVendorNconsignees where r.VendorNconsigneeId == Id select r).FirstOrDefault();
                if (resultobjVendorNConsigneeName != null)
                {
                    objVenodrNConsigneeDTO = AutoMapperServices<tblVendorNconsignee, VendorNconsigneeDTO>.ReturnObject(resultobjVendorNConsigneeName);
                }
                return objVenodrNConsigneeDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Vendor List
        /// <summary>
        /// List of Vendor
        /// </summary>
        public IEnumerable<VendorNconsigneeDTO> List
        {
            get
            {
                try
                {
                    var data = (from vendorNconsisgnee in vendorContext.tblVendorNconsignees
                                join state in vendorContext.tblStates on vendorNconsisgnee.State equals state.ID
                                where vendorNconsisgnee.IsDeleted == false
                                select new VendorNconsigneeDTO
                                {
                                    VendorNconsigneeId = vendorNconsisgnee.VendorNconsigneeId,
                                    VendorNconsigneeName = vendorNconsisgnee.VendorNconsigneeName ?? string.Empty,
                                    Address = vendorNconsisgnee.Address ?? string.Empty,
                                    City = vendorNconsisgnee.City ?? string.Empty,
                                    State = vendorNconsisgnee.State,
                                    StateName = state.Name,
                                    Phone = vendorNconsisgnee.Phone ?? string.Empty,
                                    Fax = vendorNconsisgnee.Fax ?? string.Empty,
                                    Zip = vendorNconsisgnee.Zip ?? string.Empty,
                                    Email = vendorNconsisgnee.Email

                                }
                                ).ToList();
                    return data;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion
    }
}
