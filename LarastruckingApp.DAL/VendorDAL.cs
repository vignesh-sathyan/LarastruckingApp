using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class VendorDAL : IVendorDAL
    {
        #region Private member
        /// <summary>
        /// Address Data Access Layer
        /// </summary>
        private readonly IVendorRepository iVendorRepository;
        #endregion

        #region AddressDAL
        public VendorDAL(IVendorRepository iVendorsRepository)
        {
            this.iVendorRepository = iVendorsRepository;
        }

        public IEnumerable<VendorNconsigneeDTO> List
        {
            get
            {
                return iVendorRepository.List;
            }
        }

        #endregion

        public VendorNconsigneeDTO Add(VendorNconsigneeDTO entity)
        {
            return iVendorRepository.Add(entity);
        }

        public bool Delete(VendorNconsigneeDTO entity)
        {
            return iVendorRepository.Delete(entity);
        }

        public VendorNconsigneeDTO FindById(int Id)
        {
            return iVendorRepository.FindById(Id);
        }

        public VendorNconsigneeDTO Update(VendorNconsigneeDTO entity)
        {
            return iVendorRepository.Update(entity);
        }
    }
}
