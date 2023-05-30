using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class GlobalWaitingTimeRepository : ExecuteSQLStoredProceduce
    {
        #region Private Members
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly ExecuteSQLStoredProceduce _dbContext = null;
        private readonly LarastruckingDBEntities _globalDbContext;
        readonly IShipmentRepository shipmentRepository = null;
        readonly IFumigationRepository fumigationRepository = null;

        #endregion

        #region Contructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="IDriverModuleDAL"></param>

        public GlobalWaitingTimeRepository(ExecuteSQLStoredProceduce dbContext, IShipmentRepository iShipmentRepository, IFumigationRepository iFumigationRepository)
        {

            _dbContext = dbContext;
            _globalDbContext = new LarastruckingDBEntities();
            shipmentRepository = iShipmentRepository;
            fumigationRepository = iFumigationRepository;
        }
        #endregion



        #region Get Waiting Time Details
        /// <summary>
        /// Get Waiting Time Details
        /// </summary>
        /// <param name="ShipmentId"></param>
        /// <param name="ShipmentRouteId"></param>
        /// <returns></returns>

        public List<GetWaitingNotificationDetailsDto> GetWaitingNotificationDetails()
        {
            try
            {


                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {

                };

                var result = _dbContext.ExecuteStoredProcedure<GetWaitingNotificationDetailsDto>("usp_GetWaitingTimeDetails", sqlParameters).ToList();


                List<GetWaitingNotificationDetailsDto> ShipmentWaitingResult = new List<GetWaitingNotificationDetailsDto>();
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        ShipmentEmailDTO shipmentDetails = new ShipmentEmailDTO();
                        shipmentDetails.ShipmentId = item.ShipmentId;
                        var shipmentEmailDTO = shipmentRepository.GetCustomerDetail(shipmentDetails);
                        ShipmentWaitingResult.Add(new GetWaitingNotificationDetailsDto
                        {
                            WatingNotificationId = item.WatingNotificationId,
                            ShipmentId = item.ShipmentId,
                            ShipmentRouteId = item.ShipmentRouteId,
                            PickupArrivedOn = item.PickupArrivedOn == DateTime.MinValue ? item.PickupArrivedOn : Configurations.ConvertUTCtoLocalTime(item.PickupArrivedOn),
                            PickupDepartedOn = item.PickupDepartedOn == DateTime.MinValue ? item.PickupDepartedOn : Configurations.ConvertUTCtoLocalTime(item.PickupDepartedOn),
                            DeliveryArrivedOn = item.DeliveryArrivedOn == DateTime.MinValue ? item.DeliveryArrivedOn : Configurations.ConvertUTCtoLocalTime(item.DeliveryArrivedOn),
                            DeliveryDepartedOn = item.DeliveryDepartedOn == DateTime.MinValue ? item.DeliveryDepartedOn : Configurations.ConvertUTCtoLocalTime(item.DeliveryDepartedOn),
                            CustomerId = item.CustomerId,
                            CustomerName = item.CustomerName,
                            customerEmail = item.customerEmail,
                            DriverId = item.DriverId,
                            EquipmentNo = item.EquipmentNo,
                            PickupLocationId = item.PickupLocationId,
                            PickupAddress = item.PickupAddress,
                            PickDateTime = item.PickDateTime,
                            PickupCity = item.PickupCity,
                            PickupState = item.PickupState,
                            PickupCountry = item.PickupCountry,
                            DeliveryLocationId = item.DeliveryLocationId,
                            DeliveryAddress = item.DeliveryAddress,
                            DeliveryDateTime = item.DeliveryDateTime,
                            DeliveryCity = item.DeliveryCity,
                            DeliveryState = item.DeliveryState,
                            DeliveryCountry = item.DeliveryCountry,
                            IsDelivered = item.IsDelivered,
                            IsEmailSentPWS = item.IsEmailSentPWS,
                            IsEmailSentPWE = item.IsEmailSentPWE,
                            IsEmailSentDWS = item.IsEmailSentDWS,
                            IsEmailSentDWE = item.IsEmailSentDWE,
                            PickUpDateDifference = item.PickUpDateDifference,
                            DeliveryDateDifference = item.DeliveryDateDifference,
                            IsPickUpWaitingTimeRequired = item.IsPickUpWaitingTimeRequired,
                            IsDeliveryWaitingTimeRequired = item.IsDeliveryWaitingTimeRequired,
                            StatusName = item.StatusName,
                            ShipmentEmailDTO = shipmentEmailDTO
                        });
                    }
                }

                return ShipmentWaitingResult;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update Waiting time After Email Sent 
        /// <summary>
        /// Update Waiting time After Email Sent 
        /// </summary>
        /// <returns>
        /// </returns>

        public bool UpdateWaitingTime(GetWaitingNotificationDetailsDto model)//int WatingNotificationId, DateTime PickupArrivedOn, DateTime PickupDepartedOn, DateTime DeliveryArrivedOn, DateTime DeliveryDepartedOn)
        {
            bool result = false;
            try
            {
                var ShipmentUpdateNotification = (from a in _globalDbContext.tblWatingNotifications
                                                  where a.WatingNotificationId == model.WatingNotificationId
                                                  select a).FirstOrDefault();

                if (model.IsEmailSentPWS == false && ShipmentUpdateNotification != null && model.PickupArrivedOn <= DateTime.Now && model.PickupArrivedOn != DateTime.MinValue)
                {
                    ShipmentUpdateNotification.IsEmailSentPWS = true;
                    _globalDbContext.SaveChanges();
                    result = true;
                }

                if (model.IsEmailSentDWS == false && ShipmentUpdateNotification != null && model.DeliveryArrivedOn <= DateTime.Now && model.DeliveryArrivedOn != DateTime.MinValue)
                {
                    ShipmentUpdateNotification.IsEmailSentDWS = true;
                    _globalDbContext.SaveChanges();
                    result = true;
                }

                if (model.IsEmailSentPWS == true && model.IsEmailSentPWE == false && ShipmentUpdateNotification != null && model.PickupDepartedOn != DateTime.MinValue)
                {
                    var totalWaitingTime = model.PickUpDateDifference.Split(' ');
                    if (totalWaitingTime.Length > 0)
                    {
                        if (totalWaitingTime[1].Trim() == "MINS")
                        {
                            if (totalWaitingTime[0] != "0*" && Convert.ToInt32(totalWaitingTime[0]) > 0)
                            {
                                ShipmentUpdateNotification.TotalPickupWaitingTime = ("00:" + totalWaitingTime[0]);
                            }
                        }
                        else
                        {
                            ShipmentUpdateNotification.TotalPickupWaitingTime = totalWaitingTime[0];
                        }

                    }

                    ShipmentUpdateNotification.IsEmailSentPWE = true;
                    _globalDbContext.SaveChanges();
                    result = true;
                }

                if (model.IsEmailSentDWS == true && model.IsEmailSentDWE == false && ShipmentUpdateNotification != null && model.DeliveryDepartedOn != DateTime.MinValue)
                {
                    var totalWaitingTime = model.DeliveryDateDifference.Split(' ');
                    if (totalWaitingTime.Length > 0)
                    {
                        if (totalWaitingTime[1].Trim() == "MINS")
                        {
                            if (totalWaitingTime[0] != "0*" && Convert.ToInt32(totalWaitingTime[0]) > 0)
                            {
                                ShipmentUpdateNotification.TotalDeliveryWaitingTime = ("00:" + totalWaitingTime[0]);
                            }

                        }
                        else
                        {
                            ShipmentUpdateNotification.TotalDeliveryWaitingTime = totalWaitingTime[0];
                        }
                    }
                    ShipmentUpdateNotification.IsEmailSentDWE = true;
                    ShipmentUpdateNotification.IsDelivered = true;
                    _globalDbContext.SaveChanges();
                    result = true;
                }

                var shipmentWatingTimeNotification = (from a in _globalDbContext.tblWatingNotifications
                                                      where a.WatingNotificationId == model.WatingNotificationId
                                                      select a).FirstOrDefault();
                if (shipmentWatingTimeNotification.IsEmailSentPWE == true || shipmentWatingTimeNotification.IsEmailSentDWE == true)
                {
                    if (shipmentWatingTimeNotification.TotalPickupWaitingTime == "00:0*")
                    {
                        shipmentWatingTimeNotification.TotalPickupWaitingTime = "0";
                    }
                    if (shipmentWatingTimeNotification.TotalDeliveryWaitingTime == "00:0*")
                    {
                        shipmentWatingTimeNotification.TotalDeliveryWaitingTime = "0";
                    }
                    TimeSpan t1 = TimeSpan.Parse(shipmentWatingTimeNotification.TotalPickupWaitingTime != null ? shipmentWatingTimeNotification.TotalPickupWaitingTime : "0");
                    TimeSpan t2 = TimeSpan.Parse(shipmentWatingTimeNotification.TotalDeliveryWaitingTime != null ? shipmentWatingTimeNotification.TotalDeliveryWaitingTime : "0");
                    TimeSpan t3 = t1.Add(t2);
                    var tblAccessorialCharges = _globalDbContext.tblShipmentAccessorialPrices.Where(x => x.ShipmentRouteStopeId == model.ShipmentRouteId && x.AccessorialFeeTypeId == 24).FirstOrDefault();

                    if (t3.TotalMinutes > 0)
                    {

                        if (tblAccessorialCharges == null)
                        {
                            tblShipmentAccessorialPrice objShipmentAccessorialPrice = new tblShipmentAccessorialPrice();
                            objShipmentAccessorialPrice.ShipmentId = model.ShipmentId;
                            objShipmentAccessorialPrice.ShipmentRouteStopeId = model.ShipmentRouteId;
                            objShipmentAccessorialPrice.AccessorialFeeTypeId = 24;
                            objShipmentAccessorialPrice.Unit = Convert.ToDecimal(t3.TotalMinutes);
                            _globalDbContext.tblShipmentAccessorialPrices.Add(objShipmentAccessorialPrice);
                            _globalDbContext.SaveChanges();
                        }
                        else
                        {
                            tblAccessorialCharges.Unit = Convert.ToDecimal(t3.TotalMinutes);
                            _globalDbContext.Entry(tblAccessorialCharges).State = System.Data.Entity.EntityState.Modified;
                            _globalDbContext.SaveChanges();
                        }

                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;

        }

        #endregion

        #region Get Fumigation Waiting Time Details
        /// <summary>
        /// Get Fumigation Waiting Time Details
        /// </summary>
        /// <returns></returns>

        public List<GetFumigationWaitingNotificationDetailsDto> GetFumigationWaitingNotificationDetails()
        {
            try
            {


                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                };

                var result = _dbContext.ExecuteStoredProcedure<GetFumigationWaitingNotificationDetailsDto>("usp_GetFumigationWaitingTimeDetails", sqlParameters).ToList();
                List<GetFumigationWaitingNotificationDetailsDto> FumigationWaitingResult = new List<GetFumigationWaitingNotificationDetailsDto>();
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        FumigationEmailDTO FumigationDetails = new FumigationEmailDTO();
                        FumigationDetails.FumigationId = item.FumigationId;
                        var fumigationEmailDTO = fumigationRepository.GetCustomerDetail(FumigationDetails);
                        FumigationWaitingResult.Add(new GetFumigationWaitingNotificationDetailsDto
                        {
                            FumiWatingNotificationId = item.FumiWatingNotificationId,
                            FumigationId = item.FumigationId,
                            FumigationRoutsId = item.FumigationRoutsId,
                            PickupArrivedOn = item.PickupArrivedOn,
                            PickupDepartedOn = item.PickupDepartedOn,
                            DeliveryArrivedOn = item.DeliveryArrivedOn,
                            DeliveryDepartedOn = item.DeliveryDepartedOn,
                            CustomerId = item.CustomerId,
                            CustomerName = item.CustomerName,
                            customerEmail = item.customerEmail,
                            DriverId = item.DriverId,
                            EquipmentNo = item.EquipmentNo,
                            PickupLocationId = item.PickupLocationId,
                            PickupAddress = item.PickupAddress,
                            PickDateTime = item.PickDateTime,
                            PickupCity = item.PickupCity,
                            PickupState = item.PickupState,
                            PickupCountry = item.PickupCountry,
                            DeliveryLocationId = item.DeliveryLocationId,
                            DeliveryAddress = item.DeliveryAddress,
                            DeliveryDateTime = item.DeliveryDateTime,
                            DeliveryCity = item.DeliveryCity,
                            DeliveryState = item.DeliveryState,
                            DeliveryCountry = item.DeliveryCountry,
                            IsDelivered = item.IsDelivered,
                            IsEmailSentPWS = item.IsEmailSentPWS,
                            IsEmailSentPWE = item.IsEmailSentPWE,
                            IsEmailSentDWS = item.IsEmailSentDWS,
                            IsEmailSentDWE = item.IsEmailSentDWE,
                            PickUpDateDifference = item.PickUpDateDifference,
                            DeliveryDateDifference = item.DeliveryDateDifference,
                            //IsPickUpWaitingTimeRequired = item.IsPickUpWaitingTimeRequired,
                            //IsDeliveryWaitingTimeRequired = item.IsDeliveryWaitingTimeRequired,
                            FumigationEmailDTO = fumigationEmailDTO
                        });
                    }
                }

                return FumigationWaitingResult;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update Waiting time After Email Sent 
        /// <summary>
        /// Update Waiting time After Email Sent 
        /// </summary>
        /// <returns>
        /// </returns>

        public bool UpdateFumigationWaitingTime(int FumiWatingNotificationId, DateTime PickupArrivedOn, DateTime PickupDepartedOn, DateTime DeliveryArrivedOn, DateTime DeliveryDepartedOn)
        {
            try
            {
                var FumigationUpdateNotification = (from a in _globalDbContext.tblFumigationWaitingNotifications
                                                    where a.FumiWatingNotificationId == FumiWatingNotificationId
                                                    select a).FirstOrDefault();

                if (FumigationUpdateNotification != null && PickupArrivedOn <= Configurations.TodayDateTime && PickupArrivedOn != DateTime.MinValue)
                {
                    FumigationUpdateNotification.IsEmailSentPWS = true;
                    _globalDbContext.SaveChanges();
                }
                if (FumigationUpdateNotification != null && DeliveryArrivedOn <= Configurations.TodayDateTime && DeliveryArrivedOn != DateTime.MinValue)
                {
                    FumigationUpdateNotification.IsEmailSentDWS = true;
                    _globalDbContext.SaveChanges();
                }
                if (FumigationUpdateNotification != null && PickupDepartedOn != DateTime.MinValue)
                {
                    FumigationUpdateNotification.IsEmailSentPWE = true;
                    _globalDbContext.SaveChanges();
                }
                if (FumigationUpdateNotification != null && DeliveryDepartedOn != DateTime.MinValue)
                {
                    FumigationUpdateNotification.IsEmailSentDWE = true;
                    FumigationUpdateNotification.IsDelivered = true;
                    _globalDbContext.SaveChanges();
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return true;

        }

        #endregion

        #region Get Driver Licence Expiration details 
        /// <summary>
        /// Get Driver Licence Expiration details 
        /// </summary>
        /// <returns></returns>
        public List<GetDriverExpDetailsDTO> GetDriverExpDetails()
        {
            try
            {


                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                };

                var result = _dbContext.ExecuteStoredProcedure<GetDriverExpDetailsDTO>("usp_getDriverDetails", sqlParameters).ToList();


                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Update Waiting time After Email Sent 
        /// <summary>
        /// Update Waiting time After Email Sent 
        /// </summary>
        /// <returns>
        /// </returns>,

        public bool UpdateDriverDocsExpDetails(List<GetDriverExpDetailsDTO> GetDriverExpDetails)
        {
            try
            {
                foreach (var item in GetDriverExpDetails)
                {
                    if (item.EmailSentDate == null || (item.EmailSentDate != null && item.EmailSentDate.Value.Date < Configurations.TodayDateTime.Date))
                    {
                        var DriverUpdateDocument = _globalDbContext.tblDriverDocuments.GroupJoin(_globalDbContext.tblDrivers, tdd => tdd.DriverId, td => td.DriverID, (x, y) => new
                        {
                            tdd = x,
                            td = y
                        }).Where(x => x.tdd.DocumentId == item.DocumentId).SelectMany(xy => xy.td.DefaultIfEmpty(), (x, y) => new { tdd = x.tdd, td = y }).FirstOrDefault();

                        if (DriverUpdateDocument != null && item.DocumentExpiryDate > Configurations.TodayDateTime && item.DocumentExpiryDate != DateTime.MinValue)
                        {
                            DriverUpdateDocument.tdd.EmailSentDate = Configurations.TodayDateTime;
                            // DriverUpdateDocument.tdd.EmailSentCount = DriverUpdateDocument.tdd.EmailSentCount + 1;

                            _globalDbContext.SaveChanges();
                        }
                        if (DriverUpdateDocument != null && item.DocumentExpiryDate.Date == Configurations.TodayDateTime.Date && item.DocumentExpiryDate != DateTime.MinValue)
                        {
                            // DriverUpdateDocument.tdd.IsEmailSent = true;
                            // Driver Inacative 
                            DriverUpdateDocument.td.IsActive = false;
                            // Document Inactive
                            DriverUpdateDocument.tdd.IsActive = false;
                            DriverUpdateDocument.tdd.EmailSentDate = Configurations.TodayDateTime;
                            _globalDbContext.SaveChanges();
                        }
                    }

                    //else
                    //{
                    //    return false;
                    //}

                }


            }
            catch (Exception)
            {
                throw;
            }

            return true;

        }

        #endregion

        #region deleteMail records
        public void DeleteLastMailRecord()
        {
            try
            {
                DateTime todayDay = DateTime.Now.AddMonths(-2);
                var getLastMontRecords = _globalDbContext.tblMailHistories.Where(x => x.CreatedOn < todayDay).ToList();
                if (getLastMontRecords.Count > 0)
                {
                    _globalDbContext.tblMailHistories.RemoveRange(getLastMontRecords);
                    _globalDbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        
        }

        #endregion

    }
}
