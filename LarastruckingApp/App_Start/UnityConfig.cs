using LarastruckingApp.BusinessLayer;
using LarastruckingApp.BusinessLayer.CustomerModule;
using LarastruckingApp.BusinessLayer.DriverModule;
using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.BusinessLayer.Reports.DailyReports;
using LarastruckingApp.DAL;
using LarastruckingApp.DAL.CustomerModule;
using LarastruckingApp.DAL.DriverModule;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DAL.Reports.DailyReports;
using LarastruckingApp.Repository;
using LarastruckingApp.Repository.IRepository;
using LarastruckingApp.Repository.Repository;
using LarastruckingApp.Repository.Repository.TimeCard;
using LarastruckingApp.Repository.Repository.CustomerModule;
using LarastruckingApp.Repository.Repository.DriverModule;
using LarastruckingApp.Repository.Repository.Reports;
using System;
using Unity;

namespace LarastruckingApp
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            #region Customer Module
            container.RegisterType<ICustomerModuleRepository, CustomerModuleRepository>();
            container.RegisterType<ICustomerModuleDAL, CustomerModuleDAL>();
            container.RegisterType<ICustomerModuleBAL, CustomerModuleBAL>();
            #endregion

            #region Driver Module
            container.RegisterType<IDriverModuleRepository, DriverModuleRepository>();
            container.RegisterType<IDriverModuleDAL, DriverModuleDAL>();
            container.RegisterType<IDriverModuleBAL, DriverModuleBAL>();

            #endregion

            #region Leave Module
            container.RegisterType<ILeaveRepository, LeaveRepository>();
            container.RegisterType<ILeaveDAL, LeaveDAL>();
            container.RegisterType<ILeaveBAL, LeaveBAL>();

            #endregion


            //User Table Registered
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserDAL, UserDAL>();
            container.RegisterType<IUserBAL, UserBAL>();
            container.RegisterType<IDriverBAL, DriverBAL>();
            container.RegisterType<IDriverDAL, DriverDAL>();
            container.RegisterType<IDriverRepository, DriverRepository>();
            //CustomerRegistration table register
            container.RegisterType<ICustomerBAL, CustomerBAL>();
            container.RegisterType<ICustomerDAL, CustomerDAL>();
            container.RegisterType<ICustomerRepository, CustomerRepository>();

            container.RegisterType<IEquipmentBAL, EquipmentBAL>();
            container.RegisterType<IEquipmentDAL, EquipmentDAL>();
            container.RegisterType<IEquipmentRepository, EquipmentRepository>();
            //Role Table Register
            container.RegisterType<IRoleRepository, RoleRepository>();
            container.RegisterType<IRoleDAL, RoleDAL>();
            container.RegisterType<IRoleBAL, RoleBAL>();
            //UserRole Table Register
            container.RegisterType<IUserRoleRepository, UserRoleRepository>();
            container.RegisterType<IUserRoleDAL, UserRoleDAL>();
            container.RegisterType<IUserRoleBAL, UserRoleBAL>();
            #region Forgot Password Dependency Injection Container
            container.RegisterType<IForgotPasswordBAL, ForgotPasswordBAL>();
            container.RegisterType<IForgotPasswordDAL, ForgotPasswordDAL>();
            container.RegisterType<IForgotPasswordRepository, ForgotPasswordRepository>();
            #endregion
            #region Page Authorization Role Dependency Injection Container
            container.RegisterType<IPageAutharizationBAL, PageAutharizationBAL>();
            container.RegisterType<IPageAuthorizationDAL, PageAuthorizationDAL>();
            container.RegisterType<IPageAuthorizationRepository, PageAuthorizationRepository>();
            #endregion
            #region Address Dependency Injection Container
            container.RegisterType<IAddressBAL, AddressBAL>();
            container.RegisterType<IAddressDAL, AddressDAL>();
            container.RegisterType<IAddressRepository, AddressRepository>();
            #endregion

            //Accident Report
            container.RegisterType<IAccidentReportRepository, AccidentReportRepository>();
            container.RegisterType<IAccidentReportDAL, AccidentReportDAL>();
            container.RegisterType<IAccidentReportBAL, AccidentReportBAL>();


            #region Quote
            container.RegisterType<IQuoteBAL, QuoteBAL>();
            container.RegisterType<IQuoteDAL, QuoteDAL>();
            container.RegisterType<IQuoteRepository, QuoteRepository>();


            #endregion


            #region Shipment
            container.RegisterType<IShipmentRepository, ShipmentRepository>();
            container.RegisterType<IShipmentDAL, ShipmentDAL>();
            container.RegisterType<IShipmentBAL, ShipmentBAL>();
            #endregion

            #region Vendor
            container.RegisterType<IVendorRepository, VendorRepository>();
            container.RegisterType<IVendorDAL, VendorDAL>();
            container.RegisterType<IVendorBAL, VendorBAL>();
            #endregion

            #region upload shipment
            container.RegisterType<IUploadShipmentRepository, UploadShipmentRepository>();
            container.RegisterType<IUploadShipmentDAL, UploadShipmentDAL>();
            container.RegisterType<IUploadShipmentBAL, UploadShipmentBAL>();
            #endregion

            #region Gps Tracker 
            container.RegisterType<IGpsTrackingRepository, GpsTrackerRepository>();
            container.RegisterType<IGpsTrackingDAL, GpsTrackingDAL>();
            container.RegisterType<IGpsTrackingBAL, GpsTrackingBAL>();
            #endregion

            #region Fumigation
            container.RegisterType<IFumigationRepository, FumigationRepository>();
            container.RegisterType<IFumigationDAL, FumigationDAL>();
            container.RegisterType<IFumigationBAL, FumigationBAL>();
            #endregion

            #region DailyReport
            container.RegisterType<IDailyReportRepository, DailyReportRepository>();
            container.RegisterType<IDailyReportDAL, DailyReportDAL>();
            container.RegisterType<IDailyReportBAL, DailyReportBAL>();
            #endregion

            #region TrailerRental
            container.RegisterType<ITrailerRentalRepository, TrailerRentalRepository>();
            container.RegisterType<ITrailerRentalDAL, TrailerRentalDAL>();
            container.RegisterType<ITrailerRentalBAL,TrailerRentalBAL>();
            #endregion


            #region Event History
            container.RegisterType<IShipmentEventHistoryRepository, ShipmentEventHistoryRepository>();
            container.RegisterType<IShipmentEventHistoryDAL, ShipmentEventHistoryDAL>();
            container.RegisterType<IShipmentEventHistoryBAL, ShipmentEventHistoryBAL>();
            #endregion

            #region TimeCard
            container.RegisterType<ITimeCardRepository, TimeCardRepository>();
            container.RegisterType<ITimeCardDAL, TimeCardDAL>();
            container.RegisterType<ITimeCardBAL, TimeCardBAL>();
            #endregion

            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
        }

    }
}