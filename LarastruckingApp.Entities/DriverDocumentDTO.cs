﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DTO
{
    public class DriverDocumentDTO:CommonDTO
    {
        public int DocumentId { get; set; }
        public Nullable<int> DriverId { get; set; }
        public Nullable<int> DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public Nullable<System.DateTime> DocumentIssueDate { get; set; }
        public Nullable<System.DateTime> DocumentExpiryDate { get; set; }
        public string ImageURL { get; set; }
        public string ImageName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DocumentTypeName { get; set; }

        public long UserId { get; set; }
        public string UserRole { get; set; }
    }
}
