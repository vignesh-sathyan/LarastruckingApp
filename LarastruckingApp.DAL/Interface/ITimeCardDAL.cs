using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Entities.TimeCardDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL.Interface
{
  public interface ITimeCardDAL
    {
        ValidateScheduledCheckInTimeDTO MarkDriverTimeCard(TimeCardLogDTO entity);
        List<TimeCardUserDTO> GetUserList(string searchText);
        List<ShipmentEquipmentDTO> GetEquipmentList();
        List<ViewTimeCardDTO> GetTimeCardList(SearchTimeCardDTO entity);
        bool DispatcherTimeCard(TimeCardDTO entity);
        GetTimeCardCalculationDTO GetTimeCardData(TimeCardDTO entity);
        TimeCardDTO GetDriverTimeCardDetail(TimeCardDTO entity);
        bool SaveTimeCardAmount(TimeCardCalculationDTO entity);
        List<TimeCardCalculationDTO> GetWeekDates();
        List<LaborReportDTO> GetLaborReport(SearchTimeCardDTO modal);
        List<DailyReportDTOList> GetDailyReport(SearchTimeCardDTO modal);
        int MarkUserTimeCard(TimeCardLogDTO entity);
        List<WeeklyReportDTO> GetWeeklyReport(SearchTimeCardDTO modal);
    }
}
