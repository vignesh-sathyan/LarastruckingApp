using LarastruckingApp.Entities.TimeCardDTO;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LarastruckingApp.Entities.ShipmentDTO;
using System.Data.Entity;

namespace LarastruckingApp.Repository.Repository.TimeCard
{
    public class TimeCardRepository : ITimeCardRepository
    {
        #region private member
        private readonly ExecuteSQLStoredProceduce sp_dbContext = null;
        private readonly LarastruckingDBEntities timeCardContext;
        #endregion

        #region constroctor
        public TimeCardRepository()
        {
            timeCardContext = new LarastruckingDBEntities();
            sp_dbContext = new ExecuteSQLStoredProceduce();
        }
        #endregion

        #region Add Scan Time
        /// <summary>
        /// MarkDriverTimeCard
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ValidateScheduledCheckInTimeDTO MarkDriverTimeCard(TimeCardLogDTO entity)
        {
            ValidateScheduledCheckInTimeDTO scheduledCheckInTimeDTO = new ValidateScheduledCheckInTimeDTO();

            bool isValid = false;
            if (entity.IsSuccess)
            {
                var isValidEquipment = timeCardContext.tblEquipmentDetails.Where(x => x.QRCodeNo == entity.QRCodeNo).FirstOrDefault();
                if (isValidEquipment != null)
                {
                    isValid = true;
                    entity.EquipmentId = isValidEquipment.EDID;
                }
                var checkTimeCard = timeCardContext.tblTimeCards.Where(x => x.UserId == entity.UserId).OrderByDescending(x => x.Id).FirstOrDefault();
                if (isValid)
                {
                    if (entity.IsCheckIn)
                    {

                        if (checkTimeCard != null && checkTimeCard.InDateTime != null && checkTimeCard.OutDateTime == null)
                        {
                            scheduledCheckInTimeDTO.Result = 3; //You are already Check-in.
                        }
                        else
                        {
                         
                            tblTimeCard objTimeCard = new tblTimeCard();
                            objTimeCard.UserId = Convert.ToInt32(entity.UserId);
                            objTimeCard.EquipmentId = entity.EquipmentId;
                            objTimeCard.InDateTime = entity.ScanDateTime;
                            objTimeCard.CreatedBy = entity.UserId;
                            objTimeCard.CreatedOn = Configurations.TodayDateTime;
                            objTimeCard.Day = Convert.ToString((Configurations.ConvertUTCtoLocalTime(Configurations.TodayDateTime)).DayOfWeek); 
                            timeCardContext.tblTimeCards.Add(objTimeCard);
                            timeCardContext.SaveChanges();
                            SaveTimeCardLog(entity);
                            scheduledCheckInTimeDTO.Result = 1;//Success
                                                               
                        }
                    }
                    else
                    {
                        if (checkTimeCard != null && checkTimeCard.InDateTime != null && checkTimeCard.OutDateTime != null)
                        {
                            scheduledCheckInTimeDTO.Result = 4; //You are already Check-out.
                        }
                        else
                        {
                            checkTimeCard.OutDateTime = entity.ScanDateTime;
                            timeCardContext.Entry(checkTimeCard).State = System.Data.Entity.EntityState.Modified;
                            timeCardContext.SaveChanges();
                            SaveTimeCardLog(entity);
                            scheduledCheckInTimeDTO.Result = 1;//Success
                        }
                    }
                }
                else
                {
                    scheduledCheckInTimeDTO.Result = 0;// QR Code Not Matched;
                }
            }
            else
            {
                scheduledCheckInTimeDTO.Result = SaveTimeCardLog(entity) == true ? 5 : 0;
            }
            return scheduledCheckInTimeDTO;
        }
        #endregion


        #region Mark User Time Card
        /// <summary>
        /// Mark User Time Card
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int MarkUserTimeCard(TimeCardLogDTO entity)
        {
            if (entity.PublicIp == Configurations.LaraIp || entity.PublicIp == Configurations.ChetuIp)
            {
                entity.IsSuccess = true;
            }

            if (entity.IsSuccess)
            {

                var checkTimeCard = timeCardContext.tblTimeCards.Where(x => x.UserId == entity.UserId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (entity.IsCheckIn)
                {
                    if (checkTimeCard != null && checkTimeCard.InDateTime != null && checkTimeCard.OutDateTime == null)
                    {
                        return 3; //You are already Check-in.
                    }
                    else
                    {
                        tblTimeCard objTimeCard = new tblTimeCard();
                        objTimeCard.UserId = Convert.ToInt32(entity.UserId);
                        objTimeCard.EquipmentId = entity.EquipmentId;
                        objTimeCard.InDateTime = entity.ScanDateTime;
                        objTimeCard.CreatedBy = entity.UserId;
                        objTimeCard.CreatedOn = Configurations.TodayDateTime;
                        objTimeCard.Day =Convert.ToString((Configurations.ConvertUTCtoLocalTime(Configurations.TodayDateTime)).DayOfWeek); //Convert.ToString(DateTime.Now.DayOfWeek);
                        timeCardContext.tblTimeCards.Add(objTimeCard);
                        timeCardContext.SaveChanges();
                        SaveTimeCardLog(entity);
                        return 1;//Success
                    }
                }
                else
                {
                    if (checkTimeCard != null && checkTimeCard.InDateTime != null && checkTimeCard.OutDateTime != null)
                    {
                        return 4; //You are already Check-out.
                    }
                    else
                    {
                        checkTimeCard.OutDateTime = entity.ScanDateTime;
                        timeCardContext.Entry(checkTimeCard).State = System.Data.Entity.EntityState.Modified;
                        timeCardContext.SaveChanges();
                        SaveTimeCardLog(entity);
                        return 1;//Success
                    }
                }
            }
            else
            {
                return SaveTimeCardLog(entity) == true ? 5 : 0;
            }
        }
        #endregion

        #region save time card log
        /// <summary>
        /// save time card log
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveTimeCardLog(TimeCardLogDTO entity)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@UserId", entity.UserId),
                        new SqlParameter("@EquipmentId", entity.EquipmentId),
                        new SqlParameter("@IsCheckIn", entity.IsCheckIn),
                        new SqlParameter("@ScanDateTime", entity.ScanDateTime),
                        new SqlParameter("@CreatedBy", entity.UserId),
                        new SqlParameter("@CreatedOn", Configurations.TodayDateTime),
                        new SqlParameter("@Latitude", entity.Latitude),
                        new SqlParameter("@Longitude", entity.Longitude),
                        new SqlParameter("@IsSuccess", entity.IsSuccess)
                     };

            var result = sp_dbContext.ExecuteStoredProcedure<SpResponseDTO>("usp_AddTimeCardDetail", sqlParameters);
            var response = result != null && result.Count > 0 ? result[0] : new SpResponseDTO();
            return (response.ResponseText == Configurations.Insert);
        }
        #endregion

        #region Get Driver List
        /// <summary>
        /// Get Driver List
        /// </summary>
        /// <returns></returns>
        public List<TimeCardUserDTO> GetUserList(string searchText)
        {


            var driverList = (from user in timeCardContext.tblUsers
                              where user.IsActive == true && user.IsDeleted == false && user.FirstName.Trim().ToLower().Contains(searchText.Trim().ToLower())
                              select new TimeCardUserDTO
                              {
                                  UserId = user.Userid,
                                  UserName = ((user.FirstName ?? string.Empty) + " " + (user.LastName ?? string.Empty)),// ((driver.FirstName ?? string.Empty) + ' ' + (driver.LastName ?? string.Empty)),

                              }
                              ).OrderBy(x => x.UserName).ToList();
            return driverList;

        }
        #endregion

        #region GetEquipmentList
        /// <summary>
        /// Get Equipment List
        /// </summary>
        /// <returns></returns>
        public List<ShipmentEquipmentDTO> GetEquipmentList()
        {
            var equipmentList = (from equipment in timeCardContext.tblEquipmentDetails
                                 where equipment.IsDeleted == false
                                 select new ShipmentEquipmentDTO
                                 {
                                     EDID = equipment.EDID,
                                     EquipmentNo = equipment.EquipmentNo
                                 }
                               ).OrderBy(x => x.EquipmentNo).ToList();
            return equipmentList;
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
            List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SearchTerm", entity.SearchTerm),
                        new SqlParameter("@SortColumn", entity.SortColumn),
                        new SqlParameter("@SortOrder", entity.SortOrder),
                        new SqlParameter("@PageNumber", entity.PageNumber),
                        new SqlParameter("@PageSize", entity.PageSize),
                        new SqlParameter("@StartDate", entity.StartDate),
                        new SqlParameter("@EndDate", entity.EndDate),
                        new SqlParameter("@UserId", entity.UserId),
                        new SqlParameter("@IsProduction",Configurations.ISPRODUCTION)
                                                                    };

            var result = sp_dbContext.ExecuteStoredProcedure<ViewTimeCardDTO>("usp_GetTimeCardList", sqlParameters);
            if (result.Count > 0)
            {
                foreach (var data in result)
                {
                    data.InDateTime = data.InDateTime == null ? data.InDateTime : Configurations.ConvertDateTime(Convert.ToDateTime(data.InDateTime));
                    data.OutDateTime = data.OutDateTime == null ? data.OutDateTime : Configurations.ConvertDateTime(Convert.ToDateTime(data.OutDateTime));
                }
            }
            return result;
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
            TimeCardLogDTO tcl = new TimeCardLogDTO();
            tcl.UserId = entity.UserId;
            if (entity.IsCheckIn)
            {
                tcl.ScanDateTime = entity.InDateTime;
                tcl.IsCheckIn = true;

            }
            else
            {
                tcl.ScanDateTime = entity.OutDateTime;
                tcl.IsCheckIn = false;
            }
            tcl.UserId = entity.UserId;
            tcl.IsSuccess = true;
            if (entity.Id > 0)
            {
                var timeCardData = timeCardContext.tblTimeCards.Where(x => x.Id == entity.Id).FirstOrDefault();
                if (timeCardData != null)
                {
                    if (entity.IsRemoveFlag)
                    {
                        timeCardContext.tblTimeCards.Remove(timeCardData);
                    }
                    else
                    {
                        timeCardData.InDateTime = entity.InDateTime;
                        timeCardData.OutDateTime = entity.OutDateTime;
                        timeCardData.UserId = entity.UserId;
                        timeCardData.Day = entity.Day;
                        timeCardContext.Entry(timeCardData).State = System.Data.Entity.EntityState.Modified;
                    }
                }
            }
            else
            {
                tblTimeCard timeCardData = new tblTimeCard();
                timeCardData.UserId = entity.UserId;
                timeCardData.InDateTime = entity.InDateTime;
                timeCardData.OutDateTime = entity.OutDateTime;
                timeCardData.Day = entity.Day;
                timeCardData.CreatedOn = Configurations.TodayDateTime;
                timeCardData.CreatedBy = entity.CreatedBy;
                timeCardContext.tblTimeCards.Add(timeCardData);
                // var date = Configurations.ConvertLocalToUTC(DateTime.Now);

            }
            if(tcl.ScanDateTime!=null)
            {
                bool result = SaveTimeCardLog(tcl);
            }            
            return timeCardContext.SaveChanges() > 0;
        }
        #endregion

        #region get Time Card data List for dispatcher
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>

        public GetTimeCardCalculationDTO GetTimeCardData(TimeCardDTO entity)
        {
            GetTimeCardCalculationDTO getTimeCardList = new GetTimeCardCalculationDTO();
            var timeCardCalculation = (from TC in timeCardContext.tblTimeCardCalculations
                                       where TC.UserId == entity.UserId && DbFunctions.TruncateTime(TC.WeekStartDay) >= DbFunctions.TruncateTime(entity.InDateTime)
                                       && DbFunctions.TruncateTime(TC.WeekEndDay) <= DbFunctions.TruncateTime(entity.OutDateTime)
                                       select new GetTimeCardCalculationDTO
                                       {
                                           HourlyRate = TC.HourlyRate ?? 0,
                                           TotalPay = TC.TotalPay ?? 0,
                                           Deduction = TC.Deduction ?? 0,
                                           //Reimbursement = TC.Reimbursement ?? 0,
                                           Loan = TC.Remaining,// TC.Loan ?? 0,
                                           Description = TC.Description ?? string.Empty,
                                           //  Remaining=string.Empty
                                       }
                              ).FirstOrDefault();
            if (timeCardCalculation != null)
            {

                getTimeCardList.HourlyRate = timeCardCalculation.HourlyRate;
                getTimeCardList.TotalPay = timeCardCalculation.TotalPay;
                getTimeCardList.Deduction = timeCardCalculation.Deduction;
                //getTimeCardList.Reimbursement = timeCardCalculation.Reimbursement;
                getTimeCardList.Loan = timeCardCalculation.Loan + getTimeCardList.Deduction;
                getTimeCardList.Description = timeCardCalculation.Description;
                //getTimeCardList.Remaining = timeCardCalculation.Remaining;
            }
            var tblUser = timeCardContext.tblUsers.Where(x => x.Userid == entity.UserId && x.IsDeleted == false).FirstOrDefault();
            getTimeCardList.UsernName = tblUser == null ? "" : (tblUser.FirstName.ToUpper() + " " + tblUser.LastName.ToUpper() ?? string.Empty);
            if (getTimeCardList.Loan == null || getTimeCardList.Loan == 0)
            {
                getTimeCardList.Loan = timeCardContext.tblTimeCardCalculations.Where(x => x.UserId == entity.UserId).OrderByDescending(x => x.ID).Skip(1).Select(x => x.Remaining).FirstOrDefault();
            }
            var startDate = entity.InDateTime.Value.AddHours(1);
            entity.InDateTime = Configurations.ConvertLocalToUTC(startDate);

            var endDate = entity.OutDateTime.Value.AddHours(23);
            entity.OutDateTime = Configurations.ConvertLocalToUTC(endDate);

            getTimeCardList.TimeCardList = (from TC in timeCardContext.tblTimeCards
                                            where TC.UserId == entity.UserId && TC.InDateTime >= entity.InDateTime
                                            && (TC.InDateTime <= entity.OutDateTime)
                                            && TC.Day != null
                                            select new TimeCardDTO
                                            {

                                                Id = TC.Id,
                                                InDateTime = TC.InDateTime,
                                                OutDateTime = TC.OutDateTime,
                                                Day = TC.Day
                                            }
                              ).OrderBy(x => x.Id).ToList();

            if (getTimeCardList.TimeCardList.Count() > 0)
            {
                foreach (var timeCard in getTimeCardList.TimeCardList)
                {
                    timeCard.InDateTime = (timeCard.InDateTime == null ? timeCard.InDateTime : Configurations.ConvertDateTime(Convert.ToDateTime(timeCard.InDateTime)));
                    timeCard.OutDateTime = (timeCard.OutDateTime == null ? timeCard.OutDateTime : Configurations.ConvertDateTime(Convert.ToDateTime(timeCard.OutDateTime)));
                }
            }
            return getTimeCardList;

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

            var timeCardDetail = (from TC in timeCardContext.tblTimeCards
                                  where TC.UserId == entity.UserId
                                  //&& DbFunctions.TruncateTime(TC.InDateTime) == DbFunctions.TruncateTime(DateTime.Now)
                                  select new TimeCardDTO
                                  {
                                      Id = TC.Id,
                                      InDateTime = TC.InDateTime,
                                      OutDateTime = TC.OutDateTime
                                  }
                                ).OrderByDescending(x => x.Id).FirstOrDefault();
            if (timeCardDetail != null)
            {
                timeCardDetail.InDateTime = (timeCardDetail.InDateTime == null ? timeCardDetail.InDateTime : Configurations.ConvertDateTime(Convert.ToDateTime(timeCardDetail.InDateTime)));
                timeCardDetail.OutDateTime = (timeCardDetail.OutDateTime == null ? timeCardDetail.OutDateTime : Configurations.ConvertDateTime(Convert.ToDateTime(timeCardDetail.OutDateTime)));
            }

            return timeCardDetail;

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
            if (entity.Loan > 0)
            {
                var timeCardLoanData = timeCardContext.tblTimeCardLoans.Where(x => x.UserId == entity.UserId && x.IsPaid == false).FirstOrDefault();

                if (timeCardLoanData != null && (entity.Remaining == 0))
                {
                    timeCardLoanData.IsPaid = true;
                    timeCardContext.Entry(timeCardLoanData).State = EntityState.Modified;
                }
                else
                {
                    if (timeCardLoanData == null && entity.Loan != entity.Deduction)
                    {
                        tblTimeCardLoan timeCardLoan = new tblTimeCardLoan();
                        timeCardLoan.UserId = entity.UserId;
                        timeCardLoan.Loan = entity.Loan;
                        timeCardLoan.IsPaid = false;
                        timeCardLoan.CreatedBy = entity.UserId;
                        timeCardLoan.CreatedOn = Configurations.TodayDateTime;
                        timeCardContext.tblTimeCardLoans.Add(timeCardLoan);
                        timeCardContext.SaveChanges();
                    }
                }
            }

            var timeCardCalculationData = timeCardContext.tblTimeCardCalculations.Where(x => DbFunctions.TruncateTime(x.WeekStartDay) == DbFunctions.TruncateTime(entity.WeekStartDay) && x.UserId == entity.UserId).FirstOrDefault();
            if (timeCardCalculationData != null)
            {
                timeCardCalculationData.HourlyRate = entity.HourlyRate;
                timeCardCalculationData.TotalPay = entity.TotalPay;
                timeCardCalculationData.Loan = entity.Loan;
                timeCardCalculationData.Deduction = entity.Deduction;
                timeCardCalculationData.Description = entity.Description;
                timeCardCalculationData.Remaining = entity.Remaining;
                timeCardCalculationData.Reimbursement = entity.Reimbursement;
                timeCardContext.Entry(timeCardCalculationData).State = EntityState.Modified;
                return timeCardContext.SaveChanges() > 0;

            }
            else
            {

                tblTimeCardCalculation tblTimeCardCalculation = new tblTimeCardCalculation();
                tblTimeCardCalculation.UserId = entity.UserId;
                tblTimeCardCalculation.WeekStartDay = entity.WeekStartDay;
                tblTimeCardCalculation.WeekEndDay = entity.WeekEndDay;
                tblTimeCardCalculation.HourlyRate = entity.HourlyRate;
                tblTimeCardCalculation.TotalPay = entity.TotalPay;
                tblTimeCardCalculation.Loan = entity.Loan;
                tblTimeCardCalculation.Deduction = entity.Deduction;
                tblTimeCardCalculation.Reimbursement = entity.Reimbursement;
                tblTimeCardCalculation.Description = entity.Description;
                tblTimeCardCalculation.Remaining = entity.Remaining;
                tblTimeCardCalculation.CreatedOn = Configurations.TodayDateTime;
                tblTimeCardCalculation.CreatedBy = entity.UserId;
                timeCardContext.tblTimeCardCalculations.Add(tblTimeCardCalculation);
                return timeCardContext.SaveChanges() > 0;
            }

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
           
            var timeCardCalculationData = timeCardContext.tblIncentiveCardCalculations.Where(x => DbFunctions.TruncateTime(x.WeekStartDay) == DbFunctions.TruncateTime(entity.WeekStartDay) && x.UserId == entity.UserId).FirstOrDefault();
            if (timeCardCalculationData != null)
            {
                timeCardCalculationData.HourlyRate = entity.HourlyRate;
                timeCardCalculationData.TotalPay = entity.TotalPay;
                timeCardCalculationData.Loan = entity.Loan;
                timeCardCalculationData.Deduction = entity.Deduction;
                timeCardCalculationData.Description = entity.Description;
                timeCardCalculationData.Remaining = entity.Remaining;
                timeCardCalculationData.Reimbursement = entity.Reimbursement;
                timeCardContext.Entry(timeCardCalculationData).State = EntityState.Modified;
                return timeCardContext.SaveChanges() > 0;

            }
            else
            {

                tblTimeCardCalculation tblTimeCardCalculation = new tblTimeCardCalculation();
                tblTimeCardCalculation.UserId = entity.UserId;
                tblTimeCardCalculation.WeekStartDay = entity.WeekStartDay;
                tblTimeCardCalculation.WeekEndDay = entity.WeekEndDay;
                tblTimeCardCalculation.HourlyRate = entity.HourlyRate;
                tblTimeCardCalculation.TotalPay = entity.TotalPay;
                tblTimeCardCalculation.Loan = entity.Loan;
                tblTimeCardCalculation.Deduction = entity.Deduction;
                tblTimeCardCalculation.Reimbursement = entity.Reimbursement;
                tblTimeCardCalculation.Description = entity.Description;
                tblTimeCardCalculation.Remaining = entity.Remaining;
                tblTimeCardCalculation.CreatedOn = Configurations.TodayDateTime;
                tblTimeCardCalculation.CreatedBy = entity.UserId;
                timeCardContext.tblTimeCardCalculations.Add(tblTimeCardCalculation);
                return timeCardContext.SaveChanges() > 0;
            }

        }
        #endregion

        #region get week dates
        /// <summary>
        /// GetWeekDates
        /// </summary>
        /// <returns></returns>
        public List<TimeCardCalculationDTO> GetWeekDates()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {

            };
            var result = sp_dbContext.ExecuteStoredProcedure<TimeCardCalculationDTO>("usp_GetTimeCardCalculation", sqlParameters);

            if (result != null)
            {
                if (Configurations.ISPRODUCTION)
                {
                    foreach (var timeCardCalculation in result)
                    {
                        timeCardCalculation.WeekStartDay = (timeCardCalculation.WeekStartDay == null ? timeCardCalculation.WeekStartDay : Configurations.ConvertDateTime(Convert.ToDateTime(timeCardCalculation.WeekStartDay).AddDays(1)));
                        timeCardCalculation.WeekEndDay = (timeCardCalculation.WeekEndDay == null ? timeCardCalculation.WeekEndDay : Configurations.ConvertDateTime(Convert.ToDateTime(timeCardCalculation.WeekEndDay).AddDays(1)));
                    }
                }
            }
            return result;

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
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {           new SqlParameter("@WeekStartDay", modal.StartDate),
                        new SqlParameter("@WeekEndDay",modal.EndDate),
                        new SqlParameter("@SearchTerm", modal.SearchTerm),
                        new SqlParameter("@SortColumn", modal.SortColumn),
                        new SqlParameter("@SortOrder", modal.SortOrder),
                        new SqlParameter("@PageNumber", modal.PageNumber),
                        new SqlParameter("@PageSize", modal.PageSize),
                        new SqlParameter("@IsProduction",Configurations.ISPRODUCTION)

            };
            var result = sp_dbContext.ExecuteStoredProcedure<LaborReportDTO>("USP_GetLaborReport", sqlParameters);
            return result;
        }
        #endregion

        #region  Daily Report
        /// <summary>
        /// Daily Report
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>

        public List<DailyReportDTOList> GetDailyReport(SearchTimeCardDTO modal)
        {
            var startDate = modal.StartDate.Value.AddHours(1);
            modal.StartDate = Configurations.ConvertLocalToUTC(startDate);

            var endDate = modal.EndDate.Value.AddHours(23);
            modal.EndDate = Configurations.ConvertLocalToUTC(endDate);


            var DailyReportList = (from TC in timeCardContext.tblTimeCards
                                   join tuser in timeCardContext.tblUsers on TC.UserId equals tuser.Userid
                                   where TC.InDateTime >= modal.StartDate
                                    && TC.InDateTime <= modal.EndDate
                                    && TC.Day != null && (modal.UserId == 0 || tuser.Userid == modal.UserId)
                                   select new DailyReportDTO
                                   {
                                       UserName = ((tuser.FirstName ?? string.Empty) + " " + (tuser.LastName ?? string.Empty)),
                                       InDateTime = TC.InDateTime,
                                       OutDateTime = TC.OutDateTime,
                                       Day = TC.Day,

                                   }
                              ).OrderBy(x => x.UserName).ToList();
            if (DailyReportList.Count() > 0)
            {
                foreach (var dailyReport in DailyReportList)
                {
                    dailyReport.InDateTime = dailyReport.InDateTime == null ? dailyReport.InDateTime : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(dailyReport.InDateTime).AddSeconds(-dailyReport.InDateTime.Value.Second));
                    dailyReport.OutDateTime = dailyReport.OutDateTime == null ? dailyReport.OutDateTime : Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(dailyReport.OutDateTime));

                }
            }
            List<DailyReportDTOList> dailyReportDTOs = new List<DailyReportDTOList>();
            foreach (var item in DailyReportList.GroupBy(x => x.UserName).ToList())
            {


                DailyReportDTOList dailyReportDTOList = new DailyReportDTOList();
                dailyReportDTOList.UserName = item.Key;

                var groupItem = item.OrderBy(x => x.InDateTime).GroupBy(x => x.Day).ToList();
                List<DailyReportDTO> lstDailyReportDTOs = new List<DailyReportDTO>();
                foreach (var dayItem in groupItem)
                {
                    DailyReportDTO dailyReport = new DailyReportDTO();
                    dailyReport.UserName = item.Key;
                    dailyReport.Day = dayItem.Key;
                    TimeSpan timeSpan = TimeSpan.Parse("00:00");
                    foreach (var DiffItem in dayItem)
                    {
                        if (DiffItem.OutDateTime != null && DiffItem.InDateTime != null)
                        {
                            DiffItem.OutDateTime = DateTime.ParseExact(DiffItem.OutDateTime.Value.ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", null);
                            DiffItem.InDateTime = DateTime.ParseExact(DiffItem.InDateTime.Value.ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", null);

                            timeSpan += DiffItem.OutDateTime != null && DiffItem.InDateTime != null ? DiffItem.OutDateTime.Value.Subtract(DiffItem.InDateTime.Value) : TimeSpan.Parse("00:00");
                        }
                    }
                    dailyReport.InDateTime = dayItem.FirstOrDefault().InDateTime;
                    dailyReport.OutDateTime = dayItem.OrderByDescending(x => x.OutDateTime).FirstOrDefault().OutDateTime;
                    dailyReport.InOutDiff = timeSpan;
                    lstDailyReportDTOs.Add(dailyReport);
                }
                dailyReportDTOList.DailyReportDTOs = lstDailyReportDTOs;


                dailyReportDTOs.Add(dailyReportDTOList);
            }
            return dailyReportDTOs;
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
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {           new SqlParameter("@WeekStartDay",modal.StartDate),
                        new SqlParameter("@WeekEndDay",modal.EndDate),
                        new SqlParameter("@SearchTerm", modal.SearchTerm),
                        new SqlParameter("@SortColumn", modal.SortColumn),
                        new SqlParameter("@SortOrder", modal.SortOrder),
                        new SqlParameter("@PageNumber", modal.PageNumber),
                        new SqlParameter("@PageSize", modal.PageSize),
                        new SqlParameter("@IsProduction",Configurations.ISPRODUCTION)

            };
            var result = sp_dbContext.ExecuteStoredProcedure<WeeklyReportDTO>("USP_GetWeeklyReport", sqlParameters);
            return result;
        }
        #endregion
    }
}



