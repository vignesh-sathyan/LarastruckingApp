using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Entities.TimeCardDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class TimeCardBAL : ITimeCardBAL
    {
        #region Private member
        /// <summary>
        /// Private member
        /// </summary>
        private ITimeCardDAL iTimeCardDAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iTimeCardDAL"></param>
        public TimeCardBAL(ITimeCardDAL iTimeCardDAL)
        {
            this.iTimeCardDAL = iTimeCardDAL;
        }


        #endregion

        #region Mark Driver TimeCard
        /// <summary>
        /// Mark Driver TimeCard
        /// </summary>
        public ValidateScheduledCheckInTimeDTO MarkDriverTimeCard(TimeCardLogDTO entity)
        {
            if (entity.QRCodeNo != null)
            {
                entity.ScanDateTime = DateTime.UtcNow;
            }
            return iTimeCardDAL.MarkDriverTimeCard(entity);
        }
        #endregion

        #region Get Driver List
        /// <summary>
        /// Get Driver List
        /// </summary>
        /// <returns></returns>
        public List<TimeCardUserDTO> GetUserList(string searchText)
        {
            return iTimeCardDAL.GetUserList(searchText);
        }
        #endregion

        #region GetEquipmentList
        /// <summary>
        /// Get Equipment List
        /// </summary>
        /// <returns></returns>
        public List<ShipmentEquipmentDTO> GetEquipmentList()
        {
            return iTimeCardDAL.GetEquipmentList();
        }
        #endregion

        #region Get TimeCard List
        /// <summary>
        ///  Get TimeCar dList
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<ViewTimeCardDTO> GetTimeCardList(SearchTimeCardDTO entity)
        {
            return iTimeCardDAL.GetTimeCardList(entity);
        }
        #endregion

        #region Save Time Card data by dispetcher
        /// <summary>
        /// Save Time Card data by dispetcher
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DispatcherTimeCard(TimeCardDTO entity)
        {
            if(entity.InDateTime!=null)
            {
                var inDateTime = DateTime.SpecifyKind(entity.InDateTime.Value, DateTimeKind.Unspecified);
                entity.InDateTime = TimeZoneInfo.ConvertTimeToUtc(inDateTime, TimeZoneInfo.Local);
            }
            if (entity.OutDateTime != null)
            {
                var outDateTime = DateTime.SpecifyKind(entity.OutDateTime.Value, DateTimeKind.Unspecified);
                entity.OutDateTime = TimeZoneInfo.ConvertTimeToUtc(outDateTime, TimeZoneInfo.Local);
            }
            //entity.InDateTime = entity.InDateTime == null ? entity.InDateTime : Configurations.ConvertLocalToUTC(TimeZoneInfo.ConvertTimeToUtc(entity.InDateTime.Value,TimeZoneInfo.Local));
            //entity.OutDateTime = entity.OutDateTime == null ? entity.OutDateTime : Configurations.ConvertLocalToUTC(Convert.ToDateTime(entity.OutDateTime));
            return iTimeCardDAL.DispatcherTimeCard(entity);
        }

        #endregion

        #region Save Time Card data by dispetcher
        /// <summary>
        /// Save Time Card data by dispetcher
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public GetTimeCardCalculationDTO GetTimeCardData(TimeCardDTO entity)
        {
            return iTimeCardDAL.GetTimeCardData(entity);
        }
        #endregion

        #region Get Incentive Card
        public GetIncentiveCardCalculationDTO GetIncentiveCardData(TimeCardDTO entity)
        {
            return iTimeCardDAL.GetIncentiveCardData(entity);
        }
        #endregion

        #region Save Incentive Card Grid data by dispetcher
        /// <summary>
        /// Save Time Card data by dispetcher
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IList<GetIncentiveGridDTO> GetIncentiveGridData(TimeCardDTO entity)
        {
            return iTimeCardDAL.GetIncentiveGridData(entity);
        }


        #endregion

        #region Get Driver Time Card Detail
        /// <summary>
        /// Get Driver TimeCard Detail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TimeCardDTO GetDriverTimeCardDetail(TimeCardDTO entity)
        {
            return iTimeCardDAL.GetDriverTimeCardDetail(entity);
        }


        #endregion

        #region Save Time Card Amount
        /// <summary>
        /// Save Time Card Amount
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveTimeCardAmount(TimeCardCalculationDTO entity)
        {
            return iTimeCardDAL.SaveTimeCardAmount(entity);
        }

        #endregion

        #region Save Incentive Card Amount
        // <summary>
        // Save Time Card Amount
        // </summary>
        // <param name="entity"></param>
        // <returns></returns>
        public bool SaveIncentiveCardAmount(IncentiveCardCalculation entity)
        {
            return iTimeCardDAL.SaveIncentiveCardAmount(entity);
        }

        #endregion

        #region Get Week Date
        /// <summary>
        /// Get Week Date
        /// </summary>
        /// <returns></returns>
        public List<TimeCardCalculationDTO> GetWeekDates()
        {
            return iTimeCardDAL.GetWeekDates();
        }


        #endregion

        #region  Labor Report
        /// <summary>
        /// Labor Report
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        public List<LaborReportDTO> GetLaborReport(SearchTimeCardDTO modal)
        {
            return iTimeCardDAL.GetLaborReport(modal);
        }


        #endregion

        #region  Daily Report
        /// <summary>
        /// Labor Report
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        public List<DailyReportDTOList> GetDailyReport(SearchTimeCardDTO modal)
        {
            return iTimeCardDAL.GetDailyReport(modal);
        }


        #endregion


        #region User Time Card
        /// <summary>
        /// User Time Card
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int MarkUserTimeCard(TimeCardLogDTO entity)
        {
            entity.ScanDateTime = DateTime.UtcNow;
            return iTimeCardDAL.MarkUserTimeCard(entity);
        }

        #endregion

        #region Weekly Report
        /// <summary>
        /// Weekly Report
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>

        public List<WeeklyReportDTO> GetWeeklyReport(SearchTimeCardDTO modal)
        {
            return iTimeCardDAL.GetWeeklyReport(modal);
        }
        #endregion
    }
}
