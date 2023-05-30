using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class CityViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> StateID { get; set; }
    }
}