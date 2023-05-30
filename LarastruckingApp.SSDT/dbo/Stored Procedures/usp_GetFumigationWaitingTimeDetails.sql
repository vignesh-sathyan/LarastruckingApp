CREATE PROC [dbo].[usp_GetFumigationWaitingTimeDetails]        
        
AS        
BEGIN        
SET NOCOUNT ON;         
        
--  ;With cte_getStatus as                                    
--(                                    
--select SS.StatusName, SH.StatusId, SH.FumigationId from tblFumigationStatusHistory SH                                    
--inner join (select MAX(CreatedOn) as MaxDate,FumigationId from tblFumigationStatusHistory group by FumigationId) ssh on SH.FumigationId=ssh.FumigationId and SH.CreatedOn = ssh.MaxDate                                    
--inner join tblShipmentStatus SS on SH.StatusId = SS.StatusId                                    
--)    
    
SELECT DISTINCT        
TFWF.FumiWatingNotificationId,        
TFWF.FumigationId,        
TFWF.FumigationRoutsId,        
TSRS.AirWayBill,        
TSRS.CustomerPO,        
--TSRS.CustomerRef,        
--TSRS.ShipmentRefNo,        
TSRS.ContainerNo,        
--TSRS.PurchaseDoc,        
--TSRS.OrderNo,        
dateadd(hour,2,TFWF.PickupArrivedOn) PickupArrivedOn,        
TFWF.PickupDepartedOn,        
dateadd(hour,2,TFWF.DeliveryArrivedOn) DeliveryArrivedOn,        
TFWF.DeliveryDepartedOn,        
TFWF.PickUpLocationId,        
TFWF.DeliveryLocationId,        
TCR.CustomerId,        
TCR.UserId customerUserId ,        
TU.UserName customerEmail,        
TCR.CustomerName,        
TFWF.DriverId,        
CONCAT(TD.FirstName ,' ', TD.LastName) DriverName,        
(A1.Address1 + ' ' + A1.City  + ' ' + S1.Name + ' ' + CONVERT(Varchar(200),A1.Zip) + ' ' + C1.Name) PickupAddress,         
TSRS.PickUpArrival,                    
A1.City PickupCity,                                                                                  
S1.Name PickupState,                                                                                  
C1.Name PickupCountry,                                                                                                                                                                                                                                 
(A2.Address1 + ' ' + A2.City  + ' ' + S2.Name + ' ' + CONVERT(Varchar(200),A2.Zip) + ' ' + C2.Name) DeliveryAddress,        
TSRS.DeliveryArrival,                                                                             
A2.City DeliveryCity,                                                                                  
S2.Name DeliveryState,                                                                                  
C2.Name DeliveryCountry,                                                                            
TFWF.EquipmentNo,        
TFWF.IsDelivered,        
TFWF.IsEmailSentPWS,        
TFWF.IsEmailSentPWE,        
TFWF.IsEmailSentDWS,        
TFWF.IsEmailSentDWE,        
dbo.getDateDiff(dateadd(hour,2,PickupArrivedOn),PickupDepartedOn) AS PickUpDateDifference,        
dbo.getDateDiff(dateadd(hour,2,DeliveryArrivedOn),DeliveryDepartedOn) AS DeliveryDateDifference    
-- CTES.StatusName AS StatusName        
--TSRS.IsPickUpWaitingTimeRequired,        
--TSRS.IsDeliveryWaitingTimeRequired        
        
        
 FROM tblFumigationWaitingNotification TFWF        
LEFT JOIN [dbo].[tblFumigation] TS ON TS.FumigationId = TFWF.FumigationId        
INNER JOIN [dbo].[tblFumigationRouts] TSRS ON TSRS.FumigationRoutsId = TFWF.FumigationRoutsId        
INNER JOIN [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerID = TFWF.CustomerID        
INNER JOIN [dbo].[tbluser] TU ON TU.Userid = TCR.UserId        
INNER JOIN [dbo].[tblDriver] TD ON TD.DriverID = TFWF.DriverId        
INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = TFWF.PickupLocationId                                                                         
INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                                                  
INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                              
INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = TFWF.DeliveryLocationId                                                                                  
INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State               
INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country    
--INNER JOIN cte_getStatus  CTES ON TS.FumigationId =CTES.FumigationId               
        
                                                                          
         
 WHERE        
  TFWF.IsDelivered = 0  AND    
 (     
 GETUTCDATE() >= dateadd(hour,2,PickupArrivedOn) AND TFWF.PickupArrivedOn is not NULL AND TFWF.IsEmailSentPWS = 0    
 OR      
 GETUTCDATE() >=dateadd(hour,2,DeliveryArrivedOn) AND TFWF.DeliveryArrivedOn is not NULL AND TFWF.IsEmailSentDWS=0     
 OR    
 TFWF.PickupDepartedOn is not null AND TFWF.IsEmailSentPWE =0     
 OR     
 TFWF.DeliveryDepartedOn  is not null AND TFWF.IsEmailSentDWE=0    
 )        
        
         
END