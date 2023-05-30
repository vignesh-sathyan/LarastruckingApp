using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class PageAutharizationViewModel
    {
        public string PageName { get; set; }
        public bool CanView { get; set; }
        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool IsPricingMethod { get; set; }
        public bool IsGroupAdmin { get; set; }

        public string RoleName { get; set; }
        [Required(ErrorMessage = "Please select Page Name.")]
        public int PageId { get; set; }
        [Required(ErrorMessage = "Please select Role Name.")]
        public int RoleId { get; set; }

        public List<PageAutharizationViewModel> listPageAuthorization { get; set; }
        public List<RoleViewModel> listRole { get; set; }

    }
}