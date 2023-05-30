using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class VendorBAL: IVendorBAL
    {
        #region Private member
        /// <summary>
        /// Private member
        /// </summary>
        private IVendorDAL iVendorDAL;
        #endregion

        #region VendorBAL
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iAddressDAL"></param>
        public VendorBAL(IVendorDAL iVendorDal)
        {
            this.iVendorDAL = iVendorDal;
        }

        public IEnumerable<VendorNconsigneeDTO> List
        {
            get
            {
                return iVendorDAL.List;
            }
        }


        #endregion
        public VendorNconsigneeDTO Add(VendorNconsigneeDTO entity)
        {
            return iVendorDAL.Add(entity);
        }

        public bool Delete(VendorNconsigneeDTO entity)
        {
            return iVendorDAL.Delete(entity);
        }

        public VendorNconsigneeDTO FindById(int Id)
        {
            return iVendorDAL.FindById(Id);
        }

        public VendorNconsigneeDTO Update(VendorNconsigneeDTO entity)
        {
            return iVendorDAL.Update(entity);
        }
    }
}
