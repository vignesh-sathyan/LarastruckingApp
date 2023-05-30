using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class JsonResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Data { get; set; }
    }
}