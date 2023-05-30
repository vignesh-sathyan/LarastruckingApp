using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter Valid Email")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email")]
        [DisplayName("Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Valid Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string result { get; set; }

        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool CanInsert { get; set; }
        public bool CanView { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }

    }
}