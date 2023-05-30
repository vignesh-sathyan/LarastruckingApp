using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.TrailerRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
    public interface ITrailerRentalBAL
    {
        bool SaveTrailerRental(TrailerRentalDTO model);
        IList<TrailerRentalListDTO> GetTrailerRentalList(DataTableFilterDto entity);
        TrailerRentalDTO GetTrailerRentalDetailById(int trailerRentalId);
        bool EditTrailerRental(TrailerRentalDTO model);
        bool DeleteTrailerRental(TrailerRentalDTO model);
    }
}
