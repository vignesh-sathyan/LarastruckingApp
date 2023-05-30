using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.DTO
{
    public class StateDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> CountryID { get; set; }
    }
}