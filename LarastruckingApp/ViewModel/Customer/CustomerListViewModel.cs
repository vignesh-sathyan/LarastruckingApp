using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel.Customer
{
    public class CustomerListViewModel
    {
        public IList<CustomerDto> Customers { get; set; }
        public PermissionsDto Permissions { get; set; } = new PermissionsDto();

    }
}