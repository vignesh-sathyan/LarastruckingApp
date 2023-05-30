CREATE PROC usp_GetWaitingTimeDetails

AS
BEGIN
SET NOCOUNT ON; 

SELECT DISTINCT
TWF.ShipmentId,
TWF.ShipmentRouteId,
TWF.PickupArrivedOn,
TWF.PickupDepartedOn,
TWF.DeliveryArrivedOn,
TWF.DeliveryDepartedOn,
TWF.PickUpLocationId,
TWF.DeliveryLocationId,
TCR.CustomerId,
TCR.CustomerName,
TWF.DriverId,
CONCAT(TD.FirstName ,' ', TD.LastName) DriverName,
CONCAT(A1.CompanyName ,', ', A1.Address1 + ',' + A1.City  + ',' + S1.Name + ' ' + CONVERT(Varchar(200),A1.Zip) + ',' + C1.Name) PickupAddress,             
A1.City PickupCity,                                                                          
S1.Name PickupState,                                                                          
C1.Name PickupCountry,                                                                                                                                                                                                                         
CONCAT(A2.CompanyName ,', ', A2.Address1 + ',' + A2.City  + ',' + S2.Name + ' ' + CONVERT(Varchar(200),A2.Zip) + ',' + C2.Name) DeliveryAddress ,                                                                     
A2.City DeliveryCity,                                                                          
S2.Name DeliveryState,                                                                          
C2.Name DeliveryCountry,                                                                    
TWF.EquipmentNo,
TWF.IsDelivered,
TWF.IsEmailSentPWS,
TWF.IsEmailSentPWE,
TWF.IsEmailSentDWS,
TWF.IsEmailSentDWE


FROM tblWatingNotification TWF
INNER JOIN [dbo]. [tblCustomerRegistration] TCR ON TCR.CustomerID = TWF.CustomerID
INNER JOIN [dbo].[tblDriver] TD ON TD.DriverID = TWF.DriverId
INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = TWF.PickupLocationId                                                                 
INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                                          
INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                      
INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = TWF.DeliveryLocationId                                                                          
INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State       
INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country                                                                       
 
 WHERE
 TWF.IsDelivered = 0  AND
 (
 GETDATE() >= dateadd(hour,2,PickupArrivedOn) 
 OR GETDATE() >=dateadd(hour,2,DeliveryArrivedOn) 
 OR TWf.IsEmailSentPWS = 0
 OR TWF.IsEmailSentPWE = 0
 OR TWF.IsEmailSentDWS = 0 
 OR TWF.IsEmailSentDWE = 0
 )


END