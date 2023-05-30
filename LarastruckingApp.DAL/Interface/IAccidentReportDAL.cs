using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
namespace LarastruckingApp.DAL.Interface
{
    public interface IAccidentReportDAL : ICommonDAL<AccidentReportDTO>
    {
        List<AccidentDocumentDTO> AccidentDocumentList();
        AccidentReportDocumentDTO AddAccidentDocument(AccidentReportDocumentDTO entity);
        List<DriverDTO> GetDriverList();
        List<AccidentReportDTO> ViewAccidentReport(UserDTO _user);
        bool DeleteDoucument(int DocumentId);
        AccidentReportDTO ViewAccidentReportDocument(int accidentId);
    }
}
