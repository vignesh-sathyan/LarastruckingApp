using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IAccidentReportRepository : IRepository<AccidentReportDTO>
    {
        List<AccidentDocumentDTO> DocumentList();
        AccidentReportDocumentDTO AddAccidentReportDocument(AccidentReportDocumentDTO entity);
        List<DriverDTO> GetDriverList();
        List<AccidentReportDTO> ViewAccidentReport(UserDTO _user);
        bool DeleteDoucument(int DocumentId);

        AccidentReportDTO ViewAccidentReportDocument(int accidentId);
    }
}
