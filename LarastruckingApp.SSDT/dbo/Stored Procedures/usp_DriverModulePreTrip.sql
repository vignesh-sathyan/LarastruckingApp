--sp_helptext usp_DriverModulePreTrip  
CREATE PROCEDURE [dbo].[usp_DriverModulePreTrip]                                                                                        
(                                                                                        
 @SpType INT = NULL,                                                                                        
 @UserId INT = NULL,                                                                                        
 @StartRowIndex INT = NULL,                                                                                        
 @PageSize INT = NULL,                                                                                        
 @ShipmentId INT = NULL,                                                                                        
 @ShippingRoutesId INT = NULL,                                                                        
 @ShipmentFreightDetailId INT = NULL,                                                                                      
 @SearchText VARCHAR(100) = NULL                                                                                        
)                                                                                        
AS                                                                                        
BEGIN                                                                                         
 IF(@SpType = 1)                                                                                        
 BEGIN                                                                                        
 SET NOCOUNT ON;                                                                                        
                                                                                        
 SELECT * FROM (                                                                                        
   SELECT                                              
                                                                                         
   ROW_NUMBER() OVER(PARTITION BY TS.ShipmentId ORDER BY TS.ShipmentId) RNO,                                                                                        
   TD.UserId,                                                                 
   COUNT(TS.ShipmentId) OVER(ORDER BY TS.ShipmentId) TotalRecord,                                                                                        
   ISNULL(TC.PreTripCheckupId, 0) PreTripCheckupId,                                                                             
   TSRS.PickDateTime,                                                                            
   TSEND.EquipmentId,                                                                                        
   TSRS.ShippingId,                                          
   TSRS.ShippingRoutesId,                                                                                       
   TS.ShipmentRefNo,                                            
                                             
   CONCAT(TSFD.QuantityNweight,' ',TPM.PricingMethodExt) as QuantityNMethod  ,                                                              
   TSS.StatusId,                                                              
   TSS.StatusName,                                                                                       
   CONCAT(TD.FirstName, TD.LastName) DriverName,                                                                                        
   TE.LicencePlate,                                                                                        
   CASE                                                                                        
   WHEN                                                                                         
   dbo.ufnGetPreTripStatus(TS.ShipmentId, TD.UserId) IS NULL                                                                                        
   THEN 'PENDING'                                                                                        
   ELSE dbo.ufnGetPreTripStatus(TS.ShipmentId, TD.UserId)                                             
   END                                                       
   PreTripStatus                                                                       
    FROM [dbo].[tblShipment] TS                                                                    
    INNER JOIN [dbo].[tblShipmentEquipmentNdriver] TSEND  ON TSEND.ShipmentId =TS.ShipmentId                                                                   
    INNER JOIN [dbo].[tblShipmentRoutesStop] TSRS ON TSRS.ShippingId = TSEND.ShipmentId                                            
    INNER JOIN [dbo].[tblShipmentFreightDetail] TSFD ON TSFD.ShipmentId = TS.ShipmentId                                
    INNER JOIN [dbo].[tblPricingMethod] TPM ON TPM.PricingMethodId = TSFD.PricingMethodId                                                                                     
    INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId                                              
    INNER JOIN [dbo].[tblDriver] TD ON TD.DriverID = TSEND.DriverId                                                                               
    INNER JOIN [dbo].[tblEquipmentDetail] TE ON TE.EDID = TSEND.EquipmentId                                                                                        
    LEFT JOIN [dbo].[tblPreTripCheckUp] TC ON TC.ShipmentId = TSEND.ShipmentId AND TC.UserId = TD.UserId                                             
                                                                                         
  ) T                                             
  WHERE                                                                                       
   UserId = @UserId                                                       
   --AND StatusId !=1 AND StatusId!=7 AND StatusId!=8                                                        
   AND                                             
   ((StatusName LIKE '%'+ @SearchText +'%' OR NULLIF(@SearchText, '') IS NULL) OR                                                                  
   (DriverName LIKE '%'+ @SearchText +'%' OR NULLIF(@SearchText, '') IS NULL) OR                                                                                        
   (ShipmentRefNo LIKE '%'+ @SearchText +'%' OR NULLIF(@SearchText, '') IS NULL) OR                                                                                        
   (LicencePlate LIKE '%'+ @SearchText +'%' OR NULLIF(@SearchText, '') IS NULL) OR                                                                                    
   (PreTripStatus LIKE '%'+ @SearchText +'%' OR NULLIF(@SearchText, '') IS NULL))                                                                                         
   AND T.RNO = 1                                                                                         
 --ORDER BY ShipmentId desc                                                                                      
  -- OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY                                                                              
                                                                                        
 END                                                                                        
--------------------------------------------------------------------------------------------------------------------------------                                                                      
 ELSE IF(@SpType = 2)                                                                                        
 BEGIN                                                                                        
 SET NOCOUNT ON;                                                                                        
  SELECT              
   TS.ShipmentId,                                                                                        
   TS.ShipmentRefNo,                                          
   TS.EquipmentId,                                         
   TE.EquipmentNo,                          
   TD.UserId,                                                                                        
   CASE                                                                          
    WHEN TC.PreTripCheckupId IS NULL THEN 0                                         
    ELSE TC.PreTripCheckupId                                                                                        
   END PreTripCheckupId,                                                                                        
   TC.IsTiresGood,                                 
   TC.IsBreaksGood,                                                                                        
   TC.Fuel,                                             
   TC.LoadStraps,                                                                                        
  TC.OverAllCondition                                                         
  FROM [dbo].[tblShipment] TS                                                                                        
  INNER JOIN [dbo].[tblDriver] TD ON TD.DriverID = TS.DriverId                                                                                        
  INNER JOIN [dbo].[tblEquipmentDetail] TE ON TE.EDID = TS.EquipmentId                                                                                        
  LEFT JOIN [dbo].[tblPreTripCheckUp] TC ON TC.ShipmentId = TS.ShipmentId                                                  
  WHERE                                                                                         
  TS.ShipmentId = @ShipmentId                                                                           
 END                                                                                   
--------------------------------------------------------------------------------------------------------------------------------                                                                                        
 -- Get all routes by shipment id                                           
 ELSE IF(@SpType = 3)                                                                                        
 BEGIN                                                                                        
 SET NOCOUNT ON;                                                                  
  SELECT DISTINCT                                                                                    
   RS.RouteNo RouteOrder,                                                                                        
   RS.ShippingId,                                                                                    
   RS.ShippingRoutesId,                                                                                        
   PickupLocationId,                                             
  CONCAT(A1.CompanyName ,', ', A1.Address1 + ' ' + A1.City  + ' ' + S1.Name + ' ' + CONVERT(Varchar(200),A1.Zip) + ' ' + C1.Name) PickupAddress,                           
   A1.City PickupCity,                                                                                        
   S1.Name PickupState,                                                                                        
   C1.Name PickupCountry,                                                                                        
   PickDateTime PickupDateTime,                                                                            
   DeliveryLocationId,                                                                                                 
  CONCAT(A2.CompanyName ,', ', A2.Address1 + ' ' + A2.City  + ' ' + S2.Name + ' ' + CONVERT(Varchar(200),A2.Zip) + ' ' + C2.Name) DeliveryAddress ,                                       
   A2.City DeliveryCity,                                                                                        
   S2.Name DeliveryState,                                                 
   C2.Name DeliveryCountry,                      
   DeliveryDateTime,              
   TSEND.DriverId,                
   TSEND.EquipmentId,                 
   TED.EquipmentNo,          
   TS.CustomerId                                                                                     
                                                                                        
  FROM [dbo].[tblShipmentRoutesStop] RS                
  INNER JOIN [dbo].[tblShipment] TS ON TS.ShipmentId = RS.ShippingId              
   INNER JOIN [dbo].[tblShipmentEquipmentNdriver] TSEND ON TSEND.ShipmentId = TS.ShipmentId               
   INNER JOIN [dbo].[tblDriver] TD ON TD.DriverID = TSEND.DriverId                 
  INNER JOIN [dbo].[tblEquipmentDetail] TED ON TED.EDID = TSEND.EquipmentId                  
  INNER join [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerID = TS.CustomerId                                                                            
  INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = RS.PickupLocationId                                                                               
  INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                                                        
  INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                                    
  INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = RS.DeliveryLocationId                                                                                        
  INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                     
  INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country                                                                                     
                                   
                                                                                      
  WHERE                                                                                         
  ShippingId = @ShipmentId AND RS.IsDeleted=0  AND  TD.UserId = @UserId                                             
  ORDER BY RouteOrder                                                                                        
 END                                                                                        
--------------------------------------------------------------------------------------------------------------------------------                                                                                        
-- Get Pre Trip Shipping Detail Based on Route Id                                                                                        
ELSE IF(@SpType = 4)                                                               
 BEGIN                                                           
 SET NOCOUNT ON;                                                                                        
 SELECT                                                                                   
   TS.ShipmentId,                                                                                       
   TSRS.ShippingRoutesId,                                                                                        
   TS.ShipmentRefNo ,                                                                                        
   TS.AirWayBill,                                                                                        
   TS.CustomerPO,                                                                                        
   TS.OrderNo,                                                                
   TS.CustomerRef,                                                                                        
   TS.ContainerNo,       
   TS.PurchaseDoc,                 
   CONCAT(A1.CompanyName ,', ', A1.Address1 + ' ' + A1.City  + ' ' + S1.Name + ' ' + CONVERT(Varchar(200),A1.Zip) + ' ' + C1.Name) PickUpLocation,                    
   CONCAT(A2.CompanyName ,', ', A2.Address1 + ' ' + A2.City  + ' ' + S2.Name + ' ' + CONVERT(Varchar(200),A2.Zip) + ' ' + C2.Name) DeliveryAddress ,                                                                                                          
  
    
      
        
          
            
   A1.Phone as PickUpPhone ,                                        
   A1.Extension as PickUpExtension,                                                                                        
   TSRS.PickDateTime as PickUpArrivalDate,                                                                                        
   TSRS.DeliveryDateTime as DeliveryArrive,                                                                                        
   A2.Phone as DeliveryPhone,                                                              
   A2.Extension as DeliveryExtension,                                     
   TSRS.DigitalSignature,                                      
   TSRS.ReceiverName,                
   --TSEND.DriverId,                
   --TSEND.EquipmentId,                 
   --TED.EquipmentNo,                
   --TS.CustomerId,                                                             
                                                   
   --------------------------------------                                 
   -- Shipment Status and Sub status for  Driver                                                           
    TS.StatusId,                                                    
    TS.SubStatusId,                                                    
    TS.Reason as ShipmentReason ,
	TCR.IsTemperatureRequired                                                                              
                                                                                     
  FROM [dbo].[tblShipment] TS
  Left JOIN   [dbo].[tblCustomerRegistration]  TCR ON TS.CustomerId =TCR.CustomerID
  INNER JOIN [dbo].[tblShipmentRoutesStop] TSRS ON TSRS.ShippingId = TS.ShipmentId                                                            
  INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId                                                              
  LEFT JOIN [dbo].[tblShipmentSubStatus] TSSS ON TSSS.SubStatusId = TS.SubStatusId                 
  --INNER JOIN [dbo].[tblShipmentEquipmentNdriver] TSEND ON TSEND.ShipmentId = TS.ShipmentId                  
  --INNER JOIN [dbo].[tblEquipmentDetail] TED ON TED.EDID = TSEND.EquipmentId                  
  --INNER join [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerID = TS.CustomerId                                         
  INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = TSRS.PickupLocationId                                                                                        
  INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                                                        
  INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                                                         
  INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = TSRS.DeliveryLocationId                                                                               
  INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                                                                                        
  INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country                                                                                  
  WHERE                                                                                         
  TSRS.ShippingRoutesId = @ShippingRoutesId AND TS.IsDeleted = 0 AND TSRS.IsDeleted=0                                                                                      
 END                               
-----------------------------------------------------------------------------------------------                                                                                   
-- Get PreTrip Freight Details                                                                                
                                                         
ELSE IF(@SpType = 5)                                             
 BEGIN                                                                                          
 SET NOCOUNT ON;                   
         
 SELECT                                                                          
   TSFD.ShipmentBaseFreightDetailId,                                                                                      
   TSFD.ShipmentId,                                                         
   TSFD.ShipmentRouteStopeId ,           
   TSFD.Comments,                                        
   ISNULL(TSFD.Commodity,'') Commodity,                                                                                      
   ISNULL(TFT.FreightTypeName,'') FreightTypeName,                                                                                      
   --ISNULL(TPM.PricingMethodName,'') PricingMethodName,  
        (case when TSFD.QuantityNweight>0 THEN REPLACE(CAST(TSFD.QuantityNweight AS varchar),'.00','')+' Pallet, 'else ''end+  
  case when TSFD.NoOfBox>0 THEN REPLACE(CAST(TSFD.NoOfBox AS varchar),'.00','')+' Box, 'else ''end+  
  case when TSFD.Weight>0 THEN REPLACE(CAST(TSFD.Weight AS varchar),'.00','')+' '+TSFD.Unit else ''end)as QuantityNweight,  
                                                                                             
   --replace    
   --(cast     
   ----(TSFD.QuantityNweight as varchar)    
   --(CASE     
   -- WHEN ISNULL(TSFD.QuantityNweight, 0) > 0 THEN  TSFD.QuantityNweight    
   -- ELSE 0  
--   -- end as varchar ),'.00','' +' Pallet' ) + ' ,' +        
--   cast    
--   --(TSFD.NoOfBox as varchar)    
--     (CASE     
--    WHEN ISNULL(TSFD.NoOfBox, 0) <= 0 THEN 0    
--    ELSE TSFD.NoOfBox    
--    end as varchar )+' Box'+' ,'    
--+ replace(    
--    cast    
--   -- (TSFD.Weight as varchar)    
--    (CASE     
--    WHEN ISNULL(TSFD.Weight, 0) <= 0 THEN 0    
--    ELSE TSFD.Weight    
--    end as varchar )     
--    ,'.00','' +' Kg') as QuantityNweight,                                                              
     
   CONCAT(TSFD.Temperature,' ',TSFD.TemperatureType) as TemperatureRequired                                                                                    
                                                                                       
  FROM [dbo].[tblShipmentFreightDetail] TSFD                                                                                         
  INNER JOIN [dbo].[tblFreightType] TFT ON TFT.FreightTypeId = TSFD.FreightTypeId                                                        
 -- INNER JOIN [dbo].[tblPricingMethod] TPM ON TPM.PricingMethodId = TSFD.PricingMethodId                   
                                                                                       
  WHERE                                                                                         
   TSFD.ShipmentRouteStopeId = @ShippingRoutesId AND TSFD.IsDeleted=0                                                                                   
 END                                                                                        
------------------------------------------------------------------------------------------------------                                                                                
--Get PreTrip Timmings Arrival & departure Details                                                                                
                                                                                 
ELSE IF(@SpType = 6)                                                                                    
 BEGIN                                                                 
 SET NOCOUNT ON;                                      
  SELECT                                                                                   
                                                                                 
 TSRS.ShippingRoutesId,                                                                                
 TSRS.DriverPickupArrival,                                                                                
 TSRS.DriverPickupDeparture,                                                   
 TSRS.DriverDeliveryArrival,                                                                                
 TSRS.DriverDeliveryDeparture,                    
 TSRS.ReceiverName                                                                                
    FROM [dbo].[tblShipmentRoutesStop] TSRS                                                                                   
    WHERE                                                                                         
    TSRS.ShippingRoutesId = @ShippingRoutesId  AND TSRS.IsDeleted=0                                                                                         
 END                                                                     
-----------------------------------------------------------------------------------------------                                                                             
                                                                 
-- Bind the Damaged Files                                                                           
                                                                                      
   ELSE IF(@SpType = 7)                                                    
 BEGIN                                                                                          
 SET NOCOUNT ON;                                                                                        
  SELECT                                                                           
 TDI.DamagedID,                                                                                 
 TDI.ShipmentRouteID,                             
 ISNULL(TDI.ImageName,'') DamagedImage,                                                                                
 ISNULL(TDI.ImageDescription,'') DamagedDescription,                       
 ISNULL( TDI.ImageUrl,'')ImageUrl ,                                                                                                                                               
 TDI.CreatedOn DamagedDate                                             
                                                                             
  FROM [dbo].[tblDamagedImages] TDI                                                                  
  WHERE                                                                                         
  TDI.ShipmentRouteID = @ShippingRoutesId  AND TDI.IsDeleted = 0                                                              
 END                                                                          
---------------------------------------------------------------------------------------------------                                            
   ELSE IF(@SpType = 8)                                                                                          
 BEGIN                                                                                          
 SET NOCOUNT ON;                                                               
  SELECT                                                                         
 TPOTI.ShipmentFreightDetailId,                                                                         
 TPOTI.ImageId proofImageId,                                                                                 
 TPOTI.ShipmentRouteID,                                                                                
 ISNULL(TPOTI.ImageName,'') ProofImage,                                                                                 ISNULL(TPOTI.ImageDescription,'') ProofDescription,                                                                          
 TPOTI.ActualTemperature proofActualTemp,       
 TPOTI.IsLoading IsLoading,                      
 ISNULL( TPOTI.ImageUrl,'')ImageUrl ,                                                                            
 TPOTI.CreatedOn ProofDate                                               
                                                                             
  FROM [dbo].[tblProofOfTemperatureImages] TPOTI                                                                            
  WHERE                                                                                         
  TPOTI.ShipmentRouteID = @ShippingRoutesId AND TPOTI.ShipmentFreightDetailId = @ShipmentFreightDetailId  And TPOTI.IsDeleted = 0                                                                        
 END                                                                          
                                                                                   
END
	
