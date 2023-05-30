using System;
using System.Configuration;

namespace LarastruckingApp.Entities.Common
{
    public class Configurations
    {
        #region Constants

        public static readonly string LoggerName = ConfigurationManager.AppSettings["LoggerName"];
        public static readonly string IsTrace = ConfigurationManager.AppSettings["IsTrace"];
        public static readonly string LogDB = ConfigurationManager.AppSettings["LogDB"];
        public static readonly string LogFile = ConfigurationManager.AppSettings["LogFile"];
        public static readonly string Foldername = ConfigurationManager.AppSettings["LarastruckingLog"];
        public static readonly string BaseURL = ConfigurationManager.AppSettings["BaseURL"];
        public static readonly string ImageURL = ConfigurationManager.AppSettings["ImageURL"];
        public static readonly string CipherTextKey = ConfigurationManager.AppSettings["CipherTextKey"];
        public static readonly string FUDriverDocPath = ConfigurationManager.AppSettings["FUDriverDocPath"];
        public static readonly string FUAccidentReportPath = ConfigurationManager.AppSettings["FUAccidentReportPath"];
        public static readonly string FUEquipmentPath = ConfigurationManager.AppSettings["FUEquipmentPath"];
        public static readonly string FUSignatureDocPath = ConfigurationManager.AppSettings["FUSignatureDocPath"];
        public static readonly string LarasLogo = ConfigurationManager.AppSettings["LarasLogo"];
        public static readonly bool ISPRODUCTION = Convert.ToBoolean(ConfigurationManager.AppSettings["IsProduction"]);
        public static readonly string LaraIp = ConfigurationManager.AppSettings["LaraIp"];
        public static readonly string ChetuIp = ConfigurationManager.AppSettings["ChetuIp"];

        public const string Insert = "INSERTED";
        public const string Update = "UPDATED";
        public const string SearchValue = "search[value]";
        public const string Draw = "draw";
        public const string Start = "start";
        public const string Length = "length";
        public const string Columns = "columns[";
        public const string OrderColumn = "order[0][column]";
        public const string Name = "][name]";
        public const string OrderDir = "order[0][dir]";
        public const string _FreightDetails = "_FreightDetails";


        #region upload shipment
        public const string value = "Value";
        public const string text = "Text";
        public const string xls = ".xls";
        public const string xlsx = ".xlsx";
        public const string date = "DATE";
        public const string eTA_HH_MM = "ETA HH:MM";
        public const string mmddyyyy = "MM/dd/yyyy";
        public const string hhmm = "HH:mm";
        public const string consigneeNvendor = "CONSIGNEE/VENDOR NAME";
        public const string customerPO = "CUSTOMER PO";
        public const string orderNo = "ORDER NO";
        public const string awb = "AWB #";
        public const string pickupLocation = "PICKUP LOCATION";
        public const string deliveryLocation = "DELIVERY LOCATION";
        public const string freightType = "FREIGHT TYPE";
        public const string commodity = "COMMODITY";
        public const string noOfPallets = "NO. OF PALLETS";
        public const string totalBox = "TOTAL BOX";
        public const string totalPallet = "TOTAL PALLETS";
        public const string noOfBox = "NO. OF BOX";
        public const string weight = "WEIGHT";
        public const string unitkglb = "UNIT KG/LB";
        public const string pricingMethod = "PRICING METHOD";
        public const string reqTemp = "REQ. TEMP";
        public const string comment = "COMMENT";
        public const string requestedBy = "REQUESTED BY";
        public const string errorMessage = "ErrorMessage";
        public const string delDate = "DEL. DATE";
        #endregion

        #endregion

        #region Current Date: UTC
        /// <summary>
        /// This is a static property to get current datetime based on UTC
        /// </summary>

        // private static TimeZoneInfo EST_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        private static TimeZoneInfo EST_ZONE = ISPRODUCTION ? TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"): TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public static DateTime TodayDateTime
        {
            get{ return DateTime.UtcNow; }
           
        }
        #endregion

        #region Current Date: UTC to Local Time
        /// <summary>
        /// This is a static property to get current datetime based on UTC
        /// </summary>
        public static DateTime ConvertDateTime(DateTime getDate)
        {
            //   return getDate;
            if (ISPRODUCTION)
            {
                return getDate;
            }
            else
            {
                return TimeZoneInfo.ConvertTimeFromUtc(getDate, EST_ZONE);
            }
          

        }
        public static DateTime ConvertUTCtoLocalTime(DateTime getDate)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(getDate, EST_ZONE);
        }
        #endregion

        #region UTC Date: Local Time To UTC
        /// <summary>
        /// This is a static property to get current datetime based on UTC
        /// </summary>
        public static DateTime ConvertLocalToUTC(DateTime getDate)
        {
            return TimeZoneInfo.ConvertTimeToUtc(getDate, EST_ZONE);
        }
        #endregion

    }
}
