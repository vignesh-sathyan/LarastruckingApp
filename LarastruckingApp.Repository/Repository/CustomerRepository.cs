using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.quotesDto;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using LarastruckingApp.Resource;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace LarastruckingApp.Repository.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities userContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerRepository()
        {
            userContext = new LarastruckingDBEntities();
        }
        #endregion

        #region List All Customers
        /// <summary>
        /// For accessing the all active customer list.
        /// </summary>
        public IEnumerable<CustomerDto> List
        {
            get
            {
                try
                {


                    var result = (from u in userContext.tblCustomerRegistrations
                                  join b in userContext.tblBaseAddresses on u.CustomerID equals b.CustomerId
                                  join contact in userContext.tblCustomerContacts on u.CustomerID equals contact.CustomerId
                                  where u.IsDeleted != true
                                  select new CustomerDto()
                                  {
                                      CustomerId = u.CustomerID,
                                      CustomerName = u.CustomerName,
                                      Website = u.Website,
                                      Comments = u.Comments,
                                      IsActive = u.IsActive,
                                      IsFullFledgedCustomer = u.IsFullFledgedCustomer,
                                      BaseAddressId = b.BaseAddressId,
                                      BillingAddress1 = b.BillingAddress1 ?? string.Empty,
                                      BillingAddress2 = b.BillingAddress2,
                                      BillingCity = b.BillingCity,
                                      BillingStateId = b.BillingStateId,
                                      BillingState = userContext.tblStates.FirstOrDefault(x => x.ID == b.BillingStateId).Name,
                                      BillingZipCode = b.BillingZipCode,
                                      BillingCountryId = b.BillingCountryId,
                                      BillingCountry = userContext.tblCountries.FirstOrDefault(x => x.ID == b.BillingCountryId).Name,
                                      BillingPhoneNumber = b.BillingPhoneNumber ?? string.Empty,
                                      BillingEmail = b.BillingEmail ?? string.Empty,
                                      BillingFax = b.BillingFax,

                                      ShippingAddress1 = b.ShippingAddress1,
                                      ShippingAddress2 = b.ShippingAddress2,
                                      ShippingCity = b.ShippingCity,
                                      ShippingStateId = b.ShippingStateId,
                                      ShippingState = userContext.tblStates.FirstOrDefault(x => x.ID == b.ShippingStateId).Name,
                                      ShippingZipCode = b.ShippingZipCode,
                                      ShippingCountryId = b.ShippingCountryId,
                                      ShippingCountry = userContext.tblCountries.FirstOrDefault(x => x.ID == b.ShippingCountryId).Name,
                                      ShippingPhoneNumber = b.ShippingPhoneNumber,
                                      ShippingEmail = b.ShippingEmail,
                                      ShippingFax = b.ShippingFax

                                  });

                    return result.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        #endregion

        #region Get All Customers
        /// <summary>
        /// For accessing the all active customer list.
        /// </summary>
        public IEnumerable<CustomerDto> GetAllCustomers(string type)
        {
            try
            {


                if (type == "1" || type == "2")
                {

                    var result = (from u in userContext.tblCustomerRegistrations
                                  join b in userContext.tblBaseAddresses on u.CustomerID equals b.CustomerId
                                  where u.IsDeleted != true
                                  &&
                                  (
                                    u.IsFullFledgedCustomer == (type == "1") ? true : false
                                  )

                                  select new CustomerDto()
                                  {
                                      CustomerId = u.CustomerID,
                                      CustomerName = u.CustomerName,
                                      Website = u.Website ?? string.Empty,
                                      Comments = u.Comments,
                                      IsActive = u.IsActive,
                                      IsFullFledgedCustomer = u.IsFullFledgedCustomer,
                                      BaseAddressId = b.BaseAddressId,
                                      BillingAddress1 = b.BillingAddress1 ?? string.Empty,
                                      BillingAddress2 = b.BillingAddress2,
                                      BillingCity = b.BillingCity,
                                      BillingStateId = b.BillingStateId,
                                      BillingState = userContext.tblStates.FirstOrDefault(x => x.ID == b.BillingStateId).Name,
                                      BillingZipCode = b.BillingZipCode,
                                      BillingCountryId = b.BillingCountryId,
                                      BillingCountry = userContext.tblCountries.FirstOrDefault(x => x.ID == b.BillingCountryId).Name,
                                      BillingPhoneNumber = b.BillingPhoneNumber ?? string.Empty,
                                      BillingEmail = b.BillingEmail ?? string.Empty,
                                      BillingFax = b.BillingFax,

                                      ShippingAddress1 = b.ShippingAddress1,
                                      ShippingAddress2 = b.ShippingAddress2,
                                      ShippingCity = b.ShippingCity,
                                      ShippingStateId = b.ShippingStateId,
                                      ShippingState = userContext.tblStates.FirstOrDefault(x => x.ID == b.ShippingStateId).Name,
                                      ShippingZipCode = b.ShippingZipCode,
                                      ShippingCountryId = b.ShippingCountryId,
                                      ShippingCountry = userContext.tblCountries.FirstOrDefault(x => x.ID == b.ShippingCountryId).Name,
                                      ShippingPhoneNumber = b.ShippingPhoneNumber,
                                      ShippingEmail = b.ShippingEmail,
                                      ShippingFax = b.ShippingFax,
                                      CustomerContacts = userContext.tblCustomerContacts.Where(x => x.CustomerId == u.CustomerID)
                                                            .Select(n => new CustomerContact()
                                                            {
                                                                ContactId = n.ContactId,
                                                                CustomerId = (long)n.CustomerId,
                                                                ContactFirstName = n.ContactFirstName,
                                                                ContactLastName = n.ContactLastName,
                                                                ContactPhone = n.ContactPhone,
                                                                ContactExtension = n.ContactExtension,
                                                                ContactEmail = n.ContactEmail
                                                            }).ToList()

                                  }).OrderBy(x=>x.CustomerName).ToList();
                    return result;

                }
                else
                {
                    var result = (from u in userContext.tblCustomerRegistrations
                                  join b in userContext.tblBaseAddresses on u.CustomerID equals b.CustomerId
                                  where u.IsDeleted != true
                                  select new CustomerDto()
                                  {
                                      CustomerId = u.CustomerID,
                                      CustomerName = u.CustomerName,
                                      Website = u.Website ?? string.Empty,
                                      Comments = u.Comments,
                                      IsActive = u.IsActive,
                                      IsFullFledgedCustomer = u.IsFullFledgedCustomer,
                                      BaseAddressId = b.BaseAddressId,
                                      BillingAddress1 = b.BillingAddress1 ?? string.Empty,
                                      BillingAddress2 = b.BillingAddress2,
                                      BillingCity = b.BillingCity,
                                      BillingStateId = b.BillingStateId,
                                      BillingState = userContext.tblStates.FirstOrDefault(x => x.ID == b.BillingStateId).Name,
                                      BillingZipCode = b.BillingZipCode,
                                      BillingCountryId = b.BillingCountryId,
                                      BillingCountry = userContext.tblCountries.FirstOrDefault(x => x.ID == b.BillingCountryId).Name,
                                      BillingPhoneNumber = b.BillingPhoneNumber ?? string.Empty,
                                      BillingEmail = b.BillingEmail ?? string.Empty,
                                      BillingFax = b.BillingFax,

                                      ShippingAddress1 = b.ShippingAddress1,
                                      ShippingAddress2 = b.ShippingAddress2,
                                      ShippingCity = b.ShippingCity,
                                      ShippingStateId = b.ShippingStateId,
                                      ShippingState = userContext.tblStates.FirstOrDefault(x => x.ID == b.ShippingStateId).Name,
                                      ShippingZipCode = b.ShippingZipCode,
                                      ShippingCountryId = b.ShippingCountryId,
                                      ShippingCountry = userContext.tblCountries.FirstOrDefault(x => x.ID == b.ShippingCountryId).Name,
                                      ShippingPhoneNumber = b.ShippingPhoneNumber,
                                      ShippingEmail = b.ShippingEmail,
                                      ShippingFax = b.ShippingFax,
                                      CustomerContacts = userContext.tblCustomerContacts.Where(x => x.CustomerId == u.CustomerID)
                                                            .Select(n => new CustomerContact()
                                                            {
                                                                ContactId = n.ContactId,
                                                                CustomerId = (long)n.CustomerId,
                                                                ContactFirstName = n.ContactFirstName,
                                                                ContactLastName = n.ContactLastName,
                                                                ContactPhone = n.ContactPhone,
                                                                ContactExtension = n.ContactExtension,
                                                                ContactEmail = n.ContactEmail,
                                                                ContactTitle = n.ContactTitle
                                                            }).ToList()

                                  }).OrderBy(x => x.CustomerName).ToList();
                    return result;


                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Private Add Customer Contact
        /// <summary>
        /// Add contact private member
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="customer"></param>
        public void AddContacts(CustomerDto dto, tblCustomerRegistration customer)
        {
            try
            {
                var contacts = new List<tblCustomerContact>();
                foreach (var contact in dto.CustomerContacts)
                {
                    var customerContact = new tblCustomerContact()
                    {
                        CustomerId = customer.CustomerID,
                        ContactFirstName = contact.ContactFirstName,
                        ContactLastName = contact.ContactLastName,
                        ContactPhone = contact.ContactPhone,
                        ContactExtension = contact.ContactExtension,
                        ContactEmail = contact.ContactEmail,
                        ContactTitle = contact.ContactTitle
                    };
                    contacts.Add(customerContact);
                }
                userContext.tblCustomerContacts.AddRange(contacts);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Add Customer
        /// <summary>
        /// Add customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public CustomerDto Add(CustomerDto dto)
        {
            try
            {


                var dbExists = userContext.tblCustomerRegistrations.Any(c => c.CustomerName.Equals(dto.CustomerName, StringComparison.InvariantCultureIgnoreCase) && c.IsDeleted == false);

                if (!dbExists)
                {
                    #region Creating Customer Login Credential
                    // check if email already exists in tblUser
                    var isExists = userContext.tblUsers.Any(e => e.UserName == dto.BillingEmail && e.IsDeleted == false);

                    // If exists return email already taken message
                    if (isExists)
                    {
                        dto.IsSuccess = false;
                        dto.Response = $"{dto.BillingEmail} already taken. Please use another email";
                        return dto;
                    }

                    #region User Info

                    tblUser user = new tblUser();
                    user.FirstName = dto.CustomerName;
                    user.UserName = dto.BillingEmail;
                    user.IsActive = true;
                    user.IsDeleted = false;

                    user.GUID = dto.GuidInUser;
                    user.GuidGenratedDateTime = DateTime.Now;

                    user.CreatedOn = Configurations.TodayDateTime;
                    user.ModifiedOn = Configurations.TodayDateTime;
                    user.CreatedBy = dto.CreatedBy;
                    user.ModifiedBy = dto.CreatedBy;

                    userContext.tblUsers.Add(user);
                    #endregion

                    #endregion

                    #region Add Customers
                    var customer = new tblCustomerRegistration();
                    customer.UserId = user.Userid;
                    customer.CustomerName = dto.CustomerName;
                    customer.Website = dto.Website;
                    customer.Comments = dto.Comments;
                    customer.IsActive = dto.IsActive;
                    customer.IsWaitingTimeRequired = dto.IsWaitingTimeRequired;
                    customer.IsDeleted = false;
                    customer.IsFullFledgedCustomer = true;
                    customer.CreatedBy = dto.CreatedBy;
                    customer.CreatedOn = Configurations.TodayDateTime;
                    customer.ModifiedBy = dto.CreatedBy;
                    customer.ModifiedOn = Configurations.TodayDateTime;
                    customer.IsPickDropLocation = dto.IsPickDropLocation;
                    customer.IsTemperatureRequired = dto.IsTemperatureRequired;
                    userContext.tblCustomerRegistrations.Add(customer);
                    #endregion

                    #region User Role Info
                    tblUserRole userRole = new tblUserRole();
                    userRole.UserID = Convert.ToInt32(user.Userid);
                    userRole.RoleID = Convert.ToInt16(LarastruckingResource.RoleCustomerId);
                    userContext.tblUserRoles.Add(userRole);
                    #endregion

                    #region Add Contacts
                    if (dto.CustomerContacts != null)
                    {
                        AddContacts(dto, customer);
                    }
                    #endregion

                    #region Add Addresses
                    var addresses = userContext.tblBaseAddresses.Where(u => u.CustomerId == customer.CustomerID).ToList();

                    if (addresses.Count > 0)
                    {
                        userContext.tblBaseAddresses.RemoveRange(addresses);
                    }

                    var baseAddress = new tblBaseAddress();

                    baseAddress.CustomerId = customer.CustomerID;
                    baseAddress.BillingAddress1 = dto.BillingAddress1;
                    baseAddress.BillingAddress2 = dto.BillingAddress2;
                    baseAddress.BillingCity = dto.BillingCity;
                    baseAddress.BillingStateId = dto.BillingStateId;
                    baseAddress.BillingCountryId = dto.BillingCountryId;
                    baseAddress.BillingZipCode = dto.BillingZipCode;
                    baseAddress.BillingPhoneNumber = dto.BillingPhoneNumber;
                    baseAddress.BillingFax = dto.BillingFax;
                    baseAddress.BillingEmail = dto.BillingEmail;
                    baseAddress.BillingExtension = dto.BillingExtension;
                    baseAddress.PALAccount = dto.PALAccount;

                    baseAddress.ShippingAddress1 = dto.ShippingAddress1;
                    baseAddress.ShippingAddress2 = dto.ShippingAddress2;
                    baseAddress.ShippingCity = dto.ShippingCity;
                    baseAddress.ShippingStateId = dto.ShippingStateId;
                    baseAddress.ShippingCountryId = dto.ShippingCountryId;
                    baseAddress.ShippingZipCode = dto.ShippingZipCode;
                    baseAddress.ShippingPhoneNumber = dto.ShippingPhoneNumber;
                    baseAddress.ShippingFax = dto.ShippingFax;
                    baseAddress.ShippingExtension = dto.ShippingExtension;
                    baseAddress.ShippingEmail = dto.ShippingEmail;

                    userContext.tblBaseAddresses.Add(baseAddress);
                    #endregion

                    if (dto.IsPickDropLocation)
                    {
                        AddressDTO objAddressDTO = new AddressDTO();
                        objAddressDTO.AddressTypeId = 2;
                        objAddressDTO.Address1 = dto.ShippingAddress1;
                        objAddressDTO.Address2 = dto.ShippingAddress2;
                        objAddressDTO.CompanyName = dto.CustomerName;
                        objAddressDTO.City = dto.ShippingCity;
                        objAddressDTO.State = dto.ShippingStateId;
                        objAddressDTO.Zip = dto.ShippingZipCode;
                        objAddressDTO.Country = dto.ShippingCountryId;
                        objAddressDTO.Phone = dto.ShippingPhoneNumber;
                        objAddressDTO.Extension = dto.ShippingExtension;
                        objAddressDTO.Email = dto.ShippingEmail;
                        objAddressDTO.CreatedOn = Configurations.TodayDateTime;
                        objAddressDTO.CreatedBy = dto.CreatedBy;
                        objAddressDTO.IsDeleted = false;
                        objAddressDTO.IsActive = true;
                        objAddressDTO.IsAppointmentRequired = false;

                        tblAddress objtblAddress = AutoMapperServices<AddressDTO, tblAddress>.ReturnObject(objAddressDTO);
                        userContext.tblAddresses.Add(objtblAddress);
                        objAddressDTO = AutoMapperServices<tblAddress, AddressDTO>.ReturnObject(objtblAddress);
                    }

                    dto.IsSuccess = userContext.SaveChanges() > 0;



                }
                else
                {
                    dto.IsSuccess = false;
                    dto.Response = "exists";
                }
                return dto;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update Customer
        /// <summary>
        /// Method to update customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public CustomerDto Update(CustomerDto dto)
        {
            try
            {


                var dbExists = userContext.tblCustomerRegistrations
                                            .Any(
                                                    c => c.CustomerName.Equals(dto.CustomerName, StringComparison.InvariantCultureIgnoreCase) &&
                                                    c.CustomerID != dto.CustomerId && c.IsDeleted == false);
                if (!dbExists)
                {
                    #region Update User Table
                    tblUser user = new tblUser();

                    var customer = userContext.tblCustomerRegistrations.Find(dto.CustomerId);

                    var isExists = userContext.tblUsers.Any(e => e.UserName == dto.BillingEmail && e.Userid != customer.UserId && e.IsDeleted == false);

                    if (isExists)
                    {
                        dto.IsSuccess = false;
                        dto.Response = $"{dto.BillingEmail} already taken. Please use another email";
                        return dto;
                    }

                    var dbUserExists = userContext.tblUsers.Find(customer.UserId);
                    int userId = 0;
                    if (dbUserExists == null)
                    {
                        user.FirstName = dto.CustomerName;
                        user.UserName = dto.BillingEmail;
                        user.IsActive = dto.IsActive;
                        user.IsDeleted = dto.IsDeleted;
                        user.CreatedOn = Configurations.TodayDateTime;
                        user.ModifiedOn = Configurations.TodayDateTime;
                        user.CreatedBy = dto.CreatedBy;
                        user.ModifiedBy = dto.CreatedBy;
                        userContext.tblUsers.Add(user);
                        userId = user.Userid;
                    }
                    else
                    {
                        dbUserExists.FirstName = dto.CustomerName;
                        dbUserExists.UserName = dto.BillingEmail;
                        dbUserExists.IsActive = dto.IsActive;
                        dbUserExists.IsDeleted = dto.IsDeleted;
                        dbUserExists.ModifiedOn = Configurations.TodayDateTime;
                        dbUserExists.ModifiedBy = dto.CreatedBy;

                        userId = dbUserExists.Userid;
                    }
                    #endregion

                    #region User Role Info
                    var dbRoleExists = userContext.tblUserRoles.Where(e => e.UserID == userId).ToList();
                    if (dbRoleExists.Count > 0)
                    {
                        userContext.tblUserRoles.RemoveRange(dbRoleExists);
                    }

                    tblUserRole userRole = new tblUserRole();
                    userRole.UserID = userId;
                    userRole.RoleID = Convert.ToInt16(LarastruckingResource.RoleCustomerId);
                    userContext.tblUserRoles.Add(userRole);
                    #endregion

                    #region Find & Update Customer's Detail

                    if (customer != null)
                    {
                        customer.CustomerID = dto.CustomerId;
                        customer.UserId = userId;
                        customer.CustomerName = dto.CustomerName;
                        customer.Website = dto.Website;
                        customer.Comments = dto.Comments;
                        customer.IsActive = dto.IsActive;
                        customer.IsWaitingTimeRequired = dto.IsWaitingTimeRequired;
                        customer.IsTemperatureRequired = dto.IsTemperatureRequired;
                        customer.IsDeleted = false;
                        customer.IsFullFledgedCustomer = true;
                        customer.ModifiedBy = dto.CreatedBy;
                        customer.ModifiedOn = Configurations.TodayDateTime;
                    }
                    #endregion

                    #region Find & Update Customer's Contact
                    // Process this method only if dto contains customer's contact information
                    if (dto.CustomerContacts != null)
                    {
                        var contactExists = userContext.tblCustomerContacts.Where(c => c.CustomerId == customer.CustomerID).ToList();

                        if (contactExists.Count > 0)
                        {
                            userContext.tblCustomerContacts.RemoveRange(contactExists);
                        }
                        AddContacts(dto, customer);
                    }
                    #endregion

                    #region Find & Update Billing & Shipping Address
                    var baseAddress = userContext.tblBaseAddresses.FirstOrDefault(u => u.CustomerId == dto.CustomerId);

                    if (baseAddress != null)
                    {
                        baseAddress.BaseAddressId = dto.BaseAddressId;
                        baseAddress.CustomerId = customer.CustomerID;
                        baseAddress.BillingAddress1 = dto.BillingAddress1;
                        baseAddress.BillingAddress2 = dto.BillingAddress2;
                        baseAddress.BillingCity = dto.BillingCity;
                        baseAddress.BillingStateId = dto.BillingStateId;
                        baseAddress.BillingCountryId = dto.BillingCountryId;
                        baseAddress.BillingZipCode = dto.BillingZipCode;
                        baseAddress.BillingPhoneNumber = dto.BillingPhoneNumber;
                        baseAddress.BillingFax = dto.BillingFax;
                        baseAddress.BillingEmail = dto.BillingEmail;
                        baseAddress.BillingExtension = dto.BillingExtension;
                        baseAddress.PALAccount = dto.PALAccount;

                        baseAddress.ShippingAddress1 = dto.ShippingAddress1;
                        baseAddress.ShippingAddress2 = dto.ShippingAddress2;
                        baseAddress.ShippingCity = dto.ShippingCity;
                        baseAddress.ShippingStateId = dto.ShippingStateId;
                        baseAddress.ShippingCountryId = dto.ShippingCountryId;
                        baseAddress.ShippingZipCode = dto.ShippingZipCode;
                        baseAddress.ShippingPhoneNumber = dto.ShippingPhoneNumber;
                        baseAddress.ShippingFax = dto.ShippingFax;
                        baseAddress.ShippingExtension = dto.ShippingExtension;
                        baseAddress.ShippingEmail = dto.ShippingEmail;

                        var address = userContext.tblAddresses.Where(x => x.CompanyName.ToLower().Trim() == customer.CustomerName.Trim().ToLower()).FirstOrDefault();
                        if (address != null)
                        {
                            address.Address1 = dto.ShippingAddress1;
                            address.Address2 = dto.ShippingAddress2;
                            address.City = dto.ShippingCity;
                            address.State = dto.ShippingStateId;
                            address.Zip = dto.ShippingZipCode;
                            address.Country = dto.ShippingCountryId;
                            address.Phone = dto.ShippingPhoneNumber;
                            address.Extension = dto.ShippingExtension;
                            address.Email = dto.ShippingEmail;
                            address.ModifiedOn = Configurations.TodayDateTime;
                            address.ModifiedBy = dto.CreatedBy;

                        }
                        if (dto.IsPickDropLocation)
                        {
                            AddressDTO objAddressDTO = new AddressDTO();
                            objAddressDTO.AddressTypeId = 2;
                            objAddressDTO.Address1 = dto.ShippingAddress1;
                            objAddressDTO.Address2 = dto.ShippingAddress2;
                            objAddressDTO.CompanyName = dto.CustomerName;
                            objAddressDTO.City = dto.ShippingCity;
                            objAddressDTO.State = dto.ShippingStateId;
                            objAddressDTO.Zip = dto.ShippingZipCode;
                            objAddressDTO.Country = dto.ShippingCountryId;
                            objAddressDTO.Phone = dto.ShippingPhoneNumber;
                            objAddressDTO.Extension = dto.ShippingExtension;
                            objAddressDTO.Email = dto.ShippingEmail;
                            objAddressDTO.CreatedOn = Configurations.TodayDateTime;
                            objAddressDTO.CreatedBy = dto.CreatedBy;
                            objAddressDTO.IsDeleted = false;
                            objAddressDTO.IsActive = true;
                            objAddressDTO.IsAppointmentRequired = false;

                            tblAddress objtblAddress = AutoMapperServices<AddressDTO, tblAddress>.ReturnObject(objAddressDTO);
                            userContext.tblAddresses.Add(objtblAddress);
                            objAddressDTO = AutoMapperServices<tblAddress, AddressDTO>.ReturnObject(objtblAddress);
                        }
                    }
                    #endregion

                    dto.IsSuccess = userContext.SaveChanges() > 0 ? true : false;
                }
                else
                {
                    dto.IsSuccess = false;
                    dto.Response = "exists";
                }
                return dto;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region FindById Customer
        /// <summary>
        /// Method to find customer by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerDto FindById(int Id)
        {
            try
            {


                var result = (from u in userContext.tblCustomerRegistrations
                              join b in userContext.tblBaseAddresses on u.CustomerID equals b.CustomerId
                              where u.CustomerID == Id && u.IsDeleted != true
                              select new CustomerDto()
                              {
                                  CustomerId = u.CustomerID,
                                  IsWaitingTimeRequired = u.IsWaitingTimeRequired,
                                  CustomerName = u.CustomerName ?? string.Empty,
                                  Website = u.Website ?? string.Empty,
                                  Comments = u.Comments ?? string.Empty,
                                  IsActive = u.IsActive,
                                  IsFullFledgedCustomer = u.IsFullFledgedCustomer,
                                  BaseAddressId = b.BaseAddressId,
                                  BillingAddress1 = b.BillingAddress1 ?? string.Empty,
                                  BillingAddress2 = b.BillingAddress2 ?? string.Empty,
                                  BillingExtension = b.BillingExtension ?? string.Empty,
                                  BillingCity = b.BillingCity ?? string.Empty,
                                  BillingStateId = b.BillingStateId,
                                  BillingZipCode = b.BillingZipCode,
                                  BillingCountryId = b.BillingCountryId,
                                  BillingPhoneNumber = b.BillingPhoneNumber ?? string.Empty,
                                  BillingEmail = b.BillingEmail ?? string.Empty,
                                  BillingFax = b.BillingFax ?? string.Empty,
                                  PALAccount = b.PALAccount ?? string.Empty,
                                  ShippingAddress1 = b.ShippingAddress1 ?? string.Empty,
                                  ShippingAddress2 = b.ShippingAddress2 ?? string.Empty,
                                  ShippingCity = b.ShippingCity ?? string.Empty,
                                  ShippingStateId = b.ShippingStateId,
                                  ShippingZipCode = b.ShippingZipCode,
                                  ShippingCountryId = b.ShippingCountryId,
                                  ShippingPhoneNumber = b.ShippingPhoneNumber,
                                  ShippingEmail = b.ShippingEmail ?? string.Empty,
                                  ShippingFax = b.ShippingFax ?? string.Empty,
                                  ShippingExtension = b.ShippingExtension ?? string.Empty,
                                  IsPickDropLocation = u.IsPickDropLocation,
                                  IsTemperatureRequired=u.IsTemperatureRequired,
                                  CustomerContacts = userContext.tblCustomerContacts.Where(x => x.CustomerId == u.CustomerID)
                                                        .Select(n => new CustomerContact()
                                                        {
                                                            ContactId = n.ContactId,
                                                            CustomerId = (long)n.CustomerId,
                                                            ContactFirstName = n.ContactFirstName ?? string.Empty,
                                                            ContactLastName = n.ContactLastName ?? string.Empty,
                                                            ContactPhone = n.ContactPhone,
                                                            ContactExtension = n.ContactExtension,
                                                            ContactEmail = n.ContactEmail ?? string.Empty,
                                                            ContactTitle = n.ContactTitle ?? string.Empty
                                                        }).ToList()
                              }).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Customer Delete
        /// <summary>
        /// For delete customer.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(CustomerDto entity)
        {
            try
            {

                bool result = false;
                var table = userContext.tblCustomerRegistrations.Find(entity.CustomerId);
                if (table != null)
                {
                    table.IsDeleted = true;
                    userContext.Entry(table).State = System.Data.Entity.EntityState.Modified;

                }
                var tbluser = userContext.tblUsers.Find(table.UserId);
                if (table != null)
                {
                    tbluser.IsDeleted = true;
                    userContext.Entry(tbluser).State = System.Data.Entity.EntityState.Modified;
                }
                result = userContext.SaveChanges() > 0 ? true : false;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetCountryList
        /// <summary>
        /// To find country list.
        /// </summary>
        /// <returns></returns>
        public List<CountryDTO> GetCountryList()
        {
            try
            {

                List<CountryDTO> countryList = AutoMapperServices<tblCountry, CountryDTO>.ReturnObjectList(userContext.tblCountries.ToList());
                return countryList;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetStates
        /// <summary>
        /// To find state list.
        /// </summary>
        /// <returns></returns>
        public List<StateDTO> GetStates(int countryId)
        {
            try
            {

                List<StateDTO> stateList = AutoMapperServices<tblState, StateDTO>.ReturnObjectList(userContext.tblStates.Where(e => e.CountryID == countryId).ToList());
                return stateList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetStateList
        /// <summary>
        /// To find state list.
        /// </summary>
        /// <returns></returns>
        public List<StateDTO> GetStateList()
        {
            try
            {

                List<StateDTO> stateList = AutoMapperServices<tblState, StateDTO>.ReturnObjectList(userContext.tblStates.Where(e => e.CountryID == 231).ToList());
                return stateList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetCityList
        /// <summary>
        /// To find City list.
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public List<CityDTO> GetCityList(int stateId)
        {
            try
            {

                List<CityDTO> cityList = AutoMapperServices<tblCity, CityDTO>.ReturnObjectList(userContext.tblCities.Where(e => e.StateID == stateId).ToList());
                return cityList;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Customer
        /// <summary>
        /// This method is used to get all customers for the purpose of binding it in the drop-down
        /// </summary>
        public IList<CustomerQuotesDto> GetAllCustomer(string searchText)
        {
            try
            {


                var customers = userContext.tblCustomerRegistrations.Where(c => c.CustomerName.ToLower().StartsWith(searchText.ToLower().Trim()) && c.IsActive == true && c.IsDeleted == false)
                    .Select(n => new CustomerQuotesDto()
                    {
                        CustomerID = n.CustomerID,
                        CustomerName = n.CustomerName.ToUpper(),
                        Email = userContext.tblBaseAddresses.FirstOrDefault(c => c.CustomerId == n.CustomerID).BillingEmail,
                        Phone = userContext.tblBaseAddresses.FirstOrDefault(c => c.CustomerId == n.CustomerID).BillingPhoneNumber,
                        IsWaitingTimeRequired = n.IsWaitingTimeRequired,
                        ContactInfoCount = userContext.tblCustomerContacts.Where(x => x.CustomerId == n.CustomerID).Count()
                    });
                return customers.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
