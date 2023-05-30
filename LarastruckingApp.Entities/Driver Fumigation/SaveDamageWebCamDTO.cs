﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver_Fumigation
{
   public class SaveDamageWebCamDTO
    {
        public Nullable<int> FumigationRouteId { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageUrl { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public FumigationDamagedImagesDto UploadedDamagedFile { get; set; }
    }
}
