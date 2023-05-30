using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver_Fumigation
{
    public class FumigationProofOfTempEditBind
    {
        public Nullable<int> FumigationRouteId { get; set; }
        public Nullable<int> proofImageId { get; set; }
        public string ProofImage { get; set; }
        public string ProofDescription { get; set; }
        public string proofActualTemp { get; set; }
        public System.DateTime ProofDate { get; set; }
        public string ImageUrl { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLoading { get; set; }
    }
}
