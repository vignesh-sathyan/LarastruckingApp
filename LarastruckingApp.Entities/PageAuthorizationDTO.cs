using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities
{
    /// <summary>
    ///  Page Authorization DTO
    /// </summary>
    public class PageAuthorizationDTO:CommonDTO
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<PageAuthorizationDTO> listPageAuthorization { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<RoleDTO> listRole { get; set; }

    }
}
