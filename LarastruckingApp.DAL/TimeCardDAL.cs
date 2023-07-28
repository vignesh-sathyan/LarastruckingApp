using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Entities.TimeCardDTO;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class TimeCardDAL : ITimeCardDAL
    {
        #region Private member
        /// <summary>
        /// Attebdabce Data Access Layer
        /// </summary>
        private readonly ITimeCardRepository iTimeCardRepository;
        #endregion

        #region TimeCardDAL
        public TimeCardDAL(ITimeCardRepository iTimeCardRepository)
        {
            this.iTimeCardRepository = iTimeCardRepository;
        }


        #endregion

        #region Mark Driver TimeCard
        /// <summary>
        /// Mark Driver TimeCard
        /// </summary>
        public ValidateScheduledCheckInTimeDTO MarkDriverTimeCard(TimeCardLogDTO entity)
        {
            return iTimeCardRepository.MarkDriverTimeCard(entity);
        }
        #endregion

        #region Get Driver List
        /// <summary>
        /// Get Driver List
        /// </summary>
        /// <returns></returns>
        public List<TimeCardUserDTO> GetUserList(string searchText)
        {
            return iTimeCardRepository.GetUserList(searchText);
        }
        #endregion

        #region GetEquipmentList
        /// <summary>
        /// Get Equipment List
        /// </summary>
        /// <returns></returns>
        public List<ShipmentEquipmentDTO> GetEquipmentList()
        {
            return iTimeCardRepository.GetEquipmentList();
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
            return iTimeCardRepository.GetTimeCardList(entity);
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
            return iTimeCardRepository.DispatcherTimeCard(entity);
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
            return iTimeCardRepository.GetTimeCardData(entity);
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
            return iTimeCardRepository.GetDriverTimeCardDetail(entity);
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
            return iTimeCardRepository.SaveTimeCardAmount(entity);
        }

        #endregion     

        #region Save Incentive Card Amount
        /// <summary>
        /// Save Time Card Amount
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveIncentiveCardAmount(IncentiveCardCalculation entity)
        {
            return iTimeCardRepository.SaveIncentiveCardAmount(entity);
        }

        #endregion

        #region Get Week Date
        /// <summary>
        /// Get Week Date
        /// </summary>
        /// <returns></returns>
        public List<TimeCardCalculationDTO> GetWeekDates()
        {
            return iTimeCardRepository.GetWeekDates();
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
            return iTimeCardRepository.GetLaborReport(modal);
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
            return iTimeCardRepository.GetDailyReport(modal);
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
            return iTimeCardRepository.MarkUserTimeCard(entity);
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
            return iTimeCardRepository.GetWeeklyReport(modal);
        }
        #endregion
    }
}
