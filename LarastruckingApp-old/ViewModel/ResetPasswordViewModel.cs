using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="Please enter Password")]
        [DisplayName("Password")]
        
        public string Password { get; set; }

        [NotMapped] // Does not effect with your database
        [Required(ErrorMessage ="Please enter Confirm Password")]
        [DisplayName("Confirm Password")]        
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword{get;set;}
        public Guid? GUID { get; set; }
        public bool ShowHide { get; set; }=false;
    }
}