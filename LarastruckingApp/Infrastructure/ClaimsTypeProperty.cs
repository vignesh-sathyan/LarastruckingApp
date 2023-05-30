using LarastruckingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.Infrastructure
{
    public class ClaimsTypeProperty
    {
        public int Identity_UserId { get; set; }
        public string Identity_FullName { get; set; }
        public string Identity_UserName { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<string> Identity_Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<ActionButtonDto> Identity_Permissions { get; set; }

    }
}