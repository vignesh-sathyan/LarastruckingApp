using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
   public interface IAccidentReportBAL : ICommonBAL<AccidentReportDTO>
    {
        List<AccidentDocumentDTO> AccidentDocumentList();
        AccidentReportDocumentDTO AddAccidentDocument(AccidentReportDocumentDTO entity);
        List<DriverDTO> GetDriverList();
        List<AccidentReportDTO> ViewAccidentReport(UserDTO _user);
        bool DeleteDoucument(int DocumentId);
        AccidentReportDTO ViewAccidentReportDocument(int accidentId);
    }
}
