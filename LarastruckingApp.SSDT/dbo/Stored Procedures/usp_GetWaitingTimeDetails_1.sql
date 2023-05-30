CREATE  PROC [dbo].[usp_GetWaitingTimeDetails]                        
                        
AS                        
BEGIN                        
SET NOCOUNT ON;                         
                        
-- ;With cte_getStatus as                                                      
--(                                                      
--select SS.StatusName, SH.StatusId, SH.ShipmentId from tblShipmentStatusHistory SH                                                      
--left join (select MAX(CreatedOn) as MaxDate,ShipmentId from tblShipmentStatusHistory group by ShipmentId) ssh on SH.ShipmentId=ssh.ShipmentId and SH.CreatedOn = ssh.MaxDate                                                      
--left join tblShipmentStatus SS on SH.StatusId = SS.StatusId                                                      
--)                       
SELECT DISTINCT                        
TWF.WatingNotificationId,                        
TWF.ShipmentId,                        
TWF.ShipmentRouteId,                        
--TS.AirWayBill,                        
--TS.CustomerPO,                        
--TS.CustomerRef,                         
--TS.ShipmentRefNo,                        
--TS.ContainerNo,                        
--TS.PurchaseDoc,                        
--TS.OrderNo,                        
--TWF.PickupArrivedOn,                        
dateadd(hour,2,TWF.PickupArrivedOn) PickupArrivedOn,                        
TWF.PickupDepartedOn,                        
--TWF.DeliveryArrivedOn,                        
dateadd(hour,2,TWF.DeliveryArrivedOn) DeliveryArrivedOn,                        
TWF.DeliveryDepartedOn,                        
TWF.PickUpLocationId,                        
TWF.DeliveryLocationId,                        
TCR.CustomerId,                        
TCR.UserId customerUserId ,                        
TU.UserName customerEmail,                        
TCR.CustomerName,                        
TWF.DriverId,                        
CONCAT(TD.FirstName ,' ', TD.LastName) DriverName,                        
(A1.CompanyName+', '+ A1.Address1 +' '+ ISNULL (A1.Address2,'')+ ', ' + A1.City  + ' ' + S1.Name + ' ' + CONVERT(Varchar(200),A1.Zip)) PickupAddress,                         
TSRS.PickDateTime,                                    
A1.City PickupCity,                                                                                                  
S1.Name PickupState,                                                                                                  
C1.Name PickupCountry,                                                                                                                                                                                                                                         
  
    
      
        
(A2.CompanyName+', '+ A2.Address1 + ' '+ISNULL(A2.Address2,'')+ ', '+ A2.City  + ' ' + S2.Name + ' ' + CONVERT(Varchar(200),A2.Zip)) DeliveryAddress,                        
TSRS.DeliveryDateTime,                                                                                             
A2.City DeliveryCity,                                                                                                  
S2.Name DeliveryState,                                                                                                  
C2.Name DeliveryCountry,                                                                                            
TWF.EquipmentNo,                        
TWF.IsDelivered,                        
TWF.IsEmailSentPWS,                        
TWF.IsEmailSentPWE,                        
TWF.IsEmailSentDWS,                        
TWF.IsEmailSentDWE,                        
dbo.getDateDiff(dateadd(hour,2,TWF.PickupArrivedOn),TWF.PickupDepartedOn) AS PickUpDateDifference,                        
dbo.getDateDiff(dateadd(hour,2,TWF.DeliveryArrivedOn),TWF.DeliveryDepartedOn) AS DeliveryDateDifference,                        
TSRS.IsPickUpWaitingTimeRequired,                        
TSRS.IsDeliveryWaitingTimeRequired                        
--CTES.StatusName AS StatusName                        
--ISNULL(TSFD.Commodity,'') Commodity,                                                                                                    
--ISNULL(TFT.FreightTypeName,'') FreightTypeName,           
--ISNULL(TPM.PricingMethodName,'') PricingMethodName,                        
--Concat (TSFD.Temperature ,' ', TSFD.TemperatureType) TemperatureRequired,                        
--replace(cast(TSFD.QuantityNweight as varchar),'.00','') QuantityNweight,                        
--ISNULL(TSFD.NoOfBox,0) NoOfBox,                        
--replace(cast(TSFD.Weight as varchar),'.00','') Weight                        
                        
                      
 FROM tblWatingNotification TWF                        
LEFT JOIN [dbo].[tblShipment] TS ON TS.ShipmentId = TWF.ShipmentId                        
--LEFT JOIN [dbo].[tblShipmentFreightDetail] TSFD ON TSFD.ShipmentId = TS.ShipmentId                        
--LEFT JOIN [dbo].[tblFreightType] TFT ON TFT.FreightTypeId = TSFD.FreightTypeId                        
--LEFT JOIN [dbo].[tblPricingMethod] TPM ON TPM.PricingMethodId = TSFD.PricingMethodId                        
INNER JOIN [dbo].[tblShipmentRoutesStop] TSRS ON TSRS.ShippingRoutesId = TWF.ShipmentRouteId                        
INNER JOIN [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerID = TWF.CustomerID                        
INNER JOIN [dbo].[tbluser] TU ON TU.Userid = TCR.UserId                        
INNER JOIN [dbo].[tblDriver] TD ON TD.DriverID = TWF.DriverId                        
INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = TWF.PickupLocationId                                                                                         
INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                                                                  
INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                                              
INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = TWF.DeliveryLocationId                                                                                                  
INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                               
INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country                          
--INNER JOIN cte_getStatus  CTES ON TS.ShipmentId =CTES.ShipmentId                              
                        
                                                                                          
                         
WHERE                        
                        
  TWF.IsDelivered = 0 AND  TS.StatusId!=11 And  TS.StatusId!=8 AND  TS.IsDeleted=0 AND  TS.IsDeleted=0 AND                   
 (                         
 (GETUTCDATE() >= dateadd(hour,2,TWF.PickupArrivedOn) AND TWF.PickupArrivedOn is not NULL AND TWF.PickupDepartedOn is null AND TWf.IsEmailSentPWS = 0  And TS.statusId !=7)                        
 OR                          
 (GETUTCDATE() >=dateadd(hour,2, TWF.DeliveryArrivedOn) AND TWF.DeliveryArrivedOn is not NULL AND TWF.DeliveryDepartedOn IS NULL AND TWF.IsEmailSentDWS=0 And TS.statusId !=7)                        
 OR                        
(TWF.PickupDepartedOn >= dateadd(hour,2,TWF.PickupArrivedOn) AND TWF.PickupArrivedOn is not NULL AND TWF.PickupDepartedOn is not null AND TWF.IsEmailSentPWE =0  AND TWf.IsEmailSentPWS = 1 )                        
 OR                         
 (TWF.DeliveryDepartedOn  >=dateadd(hour,2,TWF.DeliveryArrivedOn)AND TWF.DeliveryArrivedOn is not NULL AND TWF.DeliveryDepartedOn  is not null AND TWF.IsEmailSentDWE=0  AND TWF.IsEmailSentDWS=1)                        
                        
-- TWF.IsDelivered = 0  AND                         
-- (                         
--TWF.PickupArrivedOn is not null  AND TWf.IsEmailSentPWS = 0                        
-- OR                          
--TWF.DeliveryArrivedOn is not null   AND TWF.IsEmailSentDWS=0                         
-- OR                  
-- TWF.PickupDepartedOn is not null AND TWF.IsEmailSentPWE =0                         
-- OR                         
-- TWF.DeliveryDepartedOn  is not null AND TWF.IsEmailSentDWE=0                        
                        
                        
 )                       
                        
                        
                        
END