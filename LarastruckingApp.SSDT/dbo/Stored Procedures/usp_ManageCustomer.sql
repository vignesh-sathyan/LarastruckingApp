  
  
CREATE PROCEDURE [dbo].[usp_ManageCustomer]  
(  
 @SPType int=null,  
 @Page INT = null,  
 @Size INT = null  
)  
AS  
BEGIN  
 IF(@SPType = 1)  
 BEGIN  
   SET NOCOUNT ON;   
  SELECT   
   C.CustomerID,   
   CustomerName,   
   Contact,   
   Website,  
   Comments,   
   IsPickDropLocation,   
   IsActive,   
   IsFullFledgedCustomer,  
   BillingAddress1,   
   BillingAddress2,   
   BillingCity,   
   BillingStateId,   
   BS.Name BillingState,  
   BillingCountryId,   
   BC.Name BillingCountry,  
   BillingZipCode,   
   BillingPhoneNumber,   
   BillingFax,   
   BillingEmail,   
   ShippingAddress1,   
   ShippingAddress2,   
   ShippingCity,   
   ShippingStateId,   
   SS.Name ShippingState,  
   ShippingCountryId,   
   SC.Name ShippingCountry,  
   ShippingZipCode,   
   ShippingPhoneNumber,   
   ShippingFax,   
   ShippingEmail  
  
  FROM [dbo].[tblCustomerRegistration] C  
  INNER JOIN [dbo].[tblBaseAddress] BA ON  BA.CustomerId = C.CustomerID  
  INNER JOIN [dbo].[tblState] BS ON BS.ID = BA.BillingStateId  
  INNER JOIN [dbo].[tblCountry] BC ON BC.ID = BA.BillingCountryId  
  
  INNER JOIN [dbo].[tblState] SS ON SS.ID = BA.ShippingStateId  
  INNER JOIN [dbo].[tblCountry] SC ON SC.ID = BA.ShippingCountryId  
  
  
  WHERE   
  C.IsActive = 1 AND C.IsDeleted = 0  
  
  ORDER BY C.CustomerID DESC  
  
 END  
END