
CREATE PROCEDURE [dbo].[usp_FumigationDetails]                                      
(                                     
@SpType INT = NULL,                                    
@FumigationId INT = NULL,                                   
@FumigationRoutsId INT= NULL,                 
@UserId bigint = NULL                                 
                                      
)                                      
as BEGIN                                       
                                                                                                 
 IF(@SpType = 1)                                  
 BEGIN                                    
 SET NOCOUNT ON;                                      
 SELECT                                                                                                     
   TF.FumigationId,                                                                                                    
   TF.ShipmentRefNo,                                                                                                    
   TFEND.EquipmentId,                                                                                                    
   TE.EquipmentNo,                                                                                                    
   TD.UserId,                                                                                                    
   CASE                                                                                                     
    WHEN TC.FumigationPreTripCheckupId IS NULL THEN 0                                                                                                    
    ELSE TC.FumigationPreTripCheckupId                                                                                                    
   END PreTripCheckupId,                                                                                                    
   TC.IsTiresGood,                                                                                                    
   TC.IsBreaksGood,                                                                                                    
   TC.Fuel,                                                                                                    
   TC.LoadStraps,                                                                                                    
  TC.OverAllCondition                                        
                                                                                                    
  FROM [dbo].[tblFumigation] TF                                                                                                    
  INNER JOIN [dbo].[tblFumigationEquipmentNDriver] TFEND ON TFEND.FumigationId = TF.FumigationId                                       
  INNER JOIN [dbo].[tblDriver] TD ON TD.DriverID = TFEND.DriverId                                                                                                   
  INNER JOIN [dbo].[tblEquipmentDetail] TE ON TE.EDID = TFEND.EquipmentId                                                                                                    
  LEFT JOIN [dbo].[tblFumigationPreTripCheckUp] TC ON TC.FumigationId = TF.FumigationId                                                                                                    
  WHERE                                                                                                     
  TF.FumigationId = @FumigationId                                       
END                                  
----------------------------------------------------------------------------------------------------------------------------------                                  
ELSE IF(@SpType = 2)                                                                                                
 BEGIN                                                                                                
 SET NOCOUNT ON;                                              
 --SELECT DISTINCT                                                                         
 --  FR.RouteNo RouteOrder,                          
 --  FR.FumigationId,                                               
 --  FR.FumigationRoutsId,                                                                 
 --  PickUpLocation,                       
 --  CONCAT(A1.CompanyName ,', ', A1.Address1 + ' ' + A1.City  + ' ' + S1.Name + ' ' + CONVERT(Varchar(200),A1.Zip) + ' ' + C1.Name) PickupAddress,                                                 
 --  A1.City PickupCity,                                                           
 --  S1.Name PickupState,                                                                  
 --  C1.Name PickupCountry,                                                                       
 --  PickUpArrival PickupDateTime,                                                                   
 --  DeliveryLocation,                                                           
 --  CONCAT(A2.CompanyName ,', ', A2.Address1 + ' ' + A2.City  + ' ' + S2.Name + ' ' + CONVERT(Varchar(200),A2.Zip) + ' ' + C2.Name) DeliveryAddress ,                                                                                                  
 --  A2.City DeliveryCity,                                                                                                
 --  S2.Name DeliveryState,                                                                                                
 --  C2.Name DeliveryCountry,                                                              
 --  DeliveryArrival DeliveryDateTime,                              
 --  FR.FumigationSite,                              
 --  CONCAT(A3.CompanyName ,', ', A3.Address1 + ' ' + A3.City  + ' ' + S3.Name + ' ' + CONVERT(Varchar(200),A3.Zip) + ' ' + C3.Name) FumigationAddress ,                              
 --  FumigationArrival FumigationDateTime,                              
 --  TFEND.IsPickUp,                
 --  TFEND.DriverId,            
 --  TFEND.EquipmentId,            
 --  TED.EquipmentNo,                  
 --  TF.CustomerId                                  
                                                                                              
                                                                                                
 -- FROM [dbo].[tblFumigationRouts] FR                
 -- LEFT JOIN [dbo].[tblFumigation] TF ON TF.FumigationId = FR.FumigationId                          
 -- LEFT JOIN [dbo].[tblFumigationEquipmentNDriver]  TFEND ON TFEND.FumigationId = FR.FumigationId             
 -- LEFT JOIN [dbo].[tblEquipmentDetail] TED ON TED.EDID = TFEND.EquipmentId                    
 -- LEFT join [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerID = TF.CustomerId                                                                                               
 -- LEFT JOIN [dbo].[tblAddress] A1 ON A1.AddressId = FR.PickUpLocation                                                                                       
 -- LEFT JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                                                                
 -- LEFT JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                                            
 -- LEFT JOIN [dbo].[tblAddress] A2 ON A2.AddressId = FR.DeliveryLocation                                                                                                
 -- LEFT JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                                                                                                
 -- LEFT JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country                              
 -- LEFT JOIN [dbo].[tblAddress] A3 ON A3.AddressId = FR.FumigationSite                              
 -- LEFT JOIN [dbo].[tblState] S3 ON S3.ID = A3.State                              
 -- LEFT JOIN [dbo].[tblCountry] C3 ON C3.ID = A3.Country                           
                                                                                              
                                                                   
 -- WHERE                                                                                              -- FR.FumigationId = @FumigationId AND FR.IsDeleted=0 AND TFEND.DriverId =(select DriverId from tblDriver where Userid=@UserId)                        
  
     
                                                                 
              
 SELECT DISTINCT                                                                                           
   FR.RouteNo RouteOrder,                                                                                                
   FR.FumigationId,                                                                                            
   FR.FumigationRoutsId,                                                                 
   PickUpLocation,                       
   CONCAT(A1.CompanyName ,', ', A1.Address1 + ' ' + A1.City  + ' ' + S1.Name + ' ' + CONVERT(Varchar(200),A1.Zip) + ' ' + C1.Name) PickupAddress,                                                 
   A1.City PickupCity,                                                           
   S1.Name PickupState,                                                                  
   C1.Name PickupCountry,                                                                       
   PickUpArrival PickupDateTime,                                                                   
   DeliveryLocation,                                                           
   CONCAT(A2.CompanyName ,', ', A2.Address1 + ' ' + A2.City  + ' ' + S2.Name + ' ' + CONVERT(Varchar(200),A2.Zip) + ' ' + C2.Name) DeliveryAddress ,                                                                                                  
   A2.City DeliveryCity,                                                                                                
   S2.Name DeliveryState,                                                                                                
   C2.Name DeliveryCountry,                                                              
   DeliveryArrival DeliveryDateTime,                              
   FR.FumigationSite,                              
   CONCAT(A3.CompanyName ,', ', A3.Address1 + ' ' + A3.City  + ' ' + S3.Name + ' ' + CONVERT(Varchar(200),A3.Zip) + ' ' + C3.Name) FumigationAddress ,                              
   FumigationArrival FumigationDateTime,                              
   --TFEND.IsPickUp,                
   --TFEND.DriverId,            
   --TFEND.EquipmentId,            
   --TED.EquipmentNo,                  
   TF.CustomerId                                  
                                                                                              
                                                                                                
  FROM [dbo].[tblFumigationRouts] FR                
  LEFT JOIN [dbo].[tblFumigation] TF ON TF.FumigationId = FR.FumigationId                          
--  LEFT JOIN [dbo].[tblFumigationEquipmentNDriver]  TFEND ON TFEND.FumigationId = FR.FumigationId             
--  LEFT JOIN [dbo].[tblEquipmentDetail] TED ON TED.EDID = TFEND.EquipmentId                    
  LEFT join [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerID = TF.CustomerId                                                                                               
  LEFT JOIN [dbo].[tblAddress] A1 ON A1.AddressId = FR.PickUpLocation                                                                                       
  LEFT JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                                                                
  LEFT JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                                            
  LEFT JOIN [dbo].[tblAddress] A2 ON A2.AddressId = FR.DeliveryLocation                                                                                
  LEFT JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                            
  LEFT JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country                              
  LEFT JOIN [dbo].[tblAddress] A3 ON A3.AddressId = FR.FumigationSite                              
  LEFT JOIN [dbo].[tblState] S3 ON S3.ID = A3.State                              
  LEFT JOIN [dbo].[tblCountry] C3 ON C3.ID = A3.Country                              
                           
                                                                                             
  WHERE                                                                                                 
  FR.FumigationRoutsId = @FumigationRoutsId AND FR.IsDeleted=0           
  --AND TFEND.DriverId =(select DriverId from tblDriver where Userid=@UserId)                                                                                          
          
--ORDER BY TFEND.IsPickUp desc                                                                                          
 END                                                          
-----------------------------------------------------------------------------------                                
ELSE IF(@SpType = 3)                                                                                                
 BEGIN                                                                                                
 SET NOCOUNT ON;                                 
                                
 SELECT DISTINCT                                                                                       
   TF.FumigationId,                                                      
   TFR.FumigationRoutsId,                                                                                                
   TF.ShipmentRefNo ,                                                                                        
   TFR.AirWayBill,                                                                                                
   TFR.CustomerPO,                                                                                                              
   TFR.ContainerNo,                                                                                                                                 
   CONCAT(A1.CompanyName ,', ', A1.Address1 + ' ' + A1.City  + ',' + S1.Name + ' ' + CONVERT(Varchar(200),A1.Zip) + ' ' + C1.Name) PickUpLocation ,                                                                
   CONCAT(A2.CompanyName ,', ', A2.Address1 + ' ' + A2.City  + ',' + S2.Name + ' ' + CONVERT(Varchar(200),A2.Zip) + ' ' + C2.Name) DeliveryAddress ,                          
   CONCAT(A3.CompanyName ,', ', A3.Address1 + ' ' + A3.City  + ',' + S3.Name + ' ' + CONVERT(Varchar(200),A3.Zip) + ' ' + C3.Name) FumigationAddress ,                       
   FumigationArrival FumigationDateTime,                              
   A1.Phone as PickUpPhone ,                                                                                                 
   A1.Extension as PickUpExtension,                                                                            
   TFR.PickUpArrival as PickUpArrivalDate,                                                                                                
   TFR.DeliveryArrival as DeliveryArrive,                                                                                                
   A2.Phone as DeliveryPhone,                                                                      
   A2.Extension as DeliveryExtension,                       
   A3.Phone as FumigationPhone,                                                                      
   A3.Extension as FumigationExtension,                            
   TFR.DigitalSignature,                                              
   TFR.ReceiverName,                
                  
                                                           
   --------------------------------------                                                             
   -- Shipment Status and Sub status for  Driver                                                                   
    TF.StatusId,                                                            
    TF.SubStatusId,                                                            
    TF.Reason as FumigationReason                                                                                                                                                                      
  FROM [dbo].[tblFumigation] TF                 
  INNER JOIN [dbo].[tblFumigationRouts] TFR ON TFR.FumigationId = TF.FumigationId                
  INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TF.StatusId                                                                      
  LEFT JOIN [dbo].[tblShipmentSubStatus] TSSS ON TSSS.SubStatusId = TF.SubStatusId                                                        
  INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = TFR.PickupLocation                                                                                                
  INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                                                                
  INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                                                                 
  INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = TFR.DeliveryLocation                                                                                       
  INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                                                                                                
  INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country                       
   INNER JOIN [dbo].[tblAddress] A3 ON A3.AddressId = TFR.FumigationSite                                                                                       
  INNER JOIN  [dbo].[tblState] S3 ON S3.ID = A3.State                                                                                                
  INNER JOIN [dbo].[tblCountry] C3 ON C3.ID = A3.Country                       
  WHERE                                                                                                 
  TFR.FumigationRoutsId = @FumigationRoutsId AND TF.IsDeleted = 0 AND TFR.IsDeleted=0                                  
                                
END                                
-------------------------------------------------------------------------------------------------                              
                              
-- Get PreTrip Freight Details                                                                  
                                                               
ELSE IF(@SpType = 4)                                                           
 BEGIN                                                                                                
 SET NOCOUNT ON;                     
                   
 --;with CTE_TotalBoxNPallentCount(FumigationId, BoxCount)                                      
 --      AS (SELECT DISTINCT (FumigationId), CONVERT(varchar(max),concat(replace(cast(sum(FR.BoxCount) as varchar),'.00','')  , ' '+'Box' )) as BoxCount                  
 --    FROM tblFumigationRouts FR                    
 -- group By FR.FumigationId                   
 --       ),                 
  --CTE_TotalTrailerPosition(FumigationId, TrailerPosition)                                      
  --     AS (SELECT DISTINCT (FumigationId), CONVERT(varchar(max),concat(replace(cast(sum(FR.TrailerPosition) as varchar),'.00','')  , ' ' )) as TrailerPosition                  
  --   FROM tblFumigationRouts FR                    
  --group By FR.FumigationId                   
  --      ),                  
  --CTE_TotalPallet(FumigationId, QuantityNweight)                                 
  --     AS (SELECT DISTINCT (FumigationId), CONVERT(varchar(max),concat(replace(cast(sum(FR.PalletCount) as varchar),'.00','')  , ' '+'Pallet' )) as QuantityNweight                  
  --   FROM tblFumigationRouts FR                    
  --group By FR.FumigationId                
  --      )                  
                  
                  
                                                                                            
  SELECT                
  TFR.RouteNo,                                                                         
   TFR.FumigationRoutsId,                                                                                              
   TFR.FumigationId ,                               
   ISNULL(TFR.Commodity,'') Commodity,                                                                                            
   ISNULL(TPM.PricingMethodName,'') PricingMethodName,                                                                                 
  -- TP.QuantityNweight,                
  (case WHEN TFR.PalletCount>0 THEN replace(cast(TFR.PalletCount as varchar),'.00',' '+'Pallets') ELSE '' END) AS QuantityNweight,                              
  -- TBC.BoxCount ,                
  (CASE WHEN TFR.BoxCount>0 THEN  replace(cast(TFR.BoxCount as varchar),'.00',' '+'Box' ) ELSE '' END )as BoxCount,                
   replace(TFR.TrailerPosition,'.00',' ') as TrailerPosition,                
                           
 --TTF.TrailerPosition,                                                                                      
   CONCAT(TFR.Temperature,' ',TFR.TemperatureType) as TemperatureRequired                                                                                          
                                                                                         
  FROM [dbo].[tblFumigationRouts] TFR                       
  -- LEFT JOIN CTE_TotalBoxNPallentCount TBC ON TFR.FumigationId = TBC.FumigationId                   
  -- LEFT JOIN CTE_TotalTrailerPosition TTF ON TFR.FumigationId = TTF.FumigationId                  
   --LEFT JOIN CTE_TotalPallet TP ON TFR.FumigationId = TP.FumigationId                                                                                               
   INNER JOIN [dbo].[tblPricingMethod] TPM ON TPM.PricingMethodId = TFR.PricingMethod                                                                                             
  WHERE                                                                                               
  TFR.FumigationRoutsId = @FumigationRoutsId AND TFR.IsDeleted=0                                
                                                                                           
 END                               
 ---------------------------------------------------------------------------------------------------                              
                              
 -- Bind the Damaged Files                                                                                 
                                                                              
   ELSE IF(@SpType = 5)                                                                    
 BEGIN                                                                                                
 SET NOCOUNT ON;                                                                                              
  SELECT                                                                                 
 TFDI.DamagedID,                                                                                       
 TFDI.FumigationRouteId,                                                                                      
 ISNULL(TFDI.ImageName,'') DamagedImage,                             
 ISNULL(TFDI.ImageDescription,'') DamagedDescription,                             
 ISNULL( TFDI.ImageUrl,'')ImageUrl ,                                                                                      
 TFDI.CreatedOn DamagedDate                        
                         
                                                             
                                                                            
  FROM [dbo].[tblFumigationDamagedImages] TFDI                                                                        
  WHERE                                                                                               
  TFDI.FumigationRouteId = @FumigationRoutsId  AND TFDI.IsDeleted = 0                                                                    
 END                                                                                
---------------------------------------------------------------------------------------------------                                                                                
   ELSE IF(@SpType = 6)                                                                                                
 BEGIN                                                                                  
 SET NOCOUNT ON;                                                                     
  SELECT                                                                                                                                  
 TFPOTI.ImageId proofImageId,                                                                                       
 TFPOTI.FumigationRouteId,                                                                                      
 ISNULL(TFPOTI.ImageName,'') ProofImage,                                                                                      
 ISNULL(TFPOTI.ImageDescription,'') ProofDescription,                                                                                
 TFPOTI.ActualTemperature proofActualTemp,                             
 ISNULL( TFPOTI.ImageUrl,'')ImageUrl ,                                                                               
 TFPOTI.CreatedOn ProofDate,                          
  TFPOTI.IsLoading                                                                           
                                                                                   
  FROM [dbo].[tblFumigationProofOfTemperatureImages] TFPOTI                                                                                  
  WHERE                                                                                               
  TFPOTI.FumigationRouteId = @FumigationRoutsId And TFPOTI.IsDeleted = 0                                
                                       
 END                               
--------------------------------------------------------------------------                               
 --Get PreTrip Timmings Arrival & departure Details                                                                                      
                                                                                      
ELSE IF(@SpType = 7)                                       
 BEGIN                                                                       
 SET NOCOUNT ON;                                                                                              
  SELECT                                                                                         
 TFR.FumigationRoutsId,                                                  
 TFR.DriverPickupArrival,                                                                                      
 TFR.DriverPickupDeparture,                                                         
 TFR.DriverDeliveryArrival,                                                                                      
 TFR.DriverDeliveryDeparture,                      
 TFR.DepartureDate,      
 TFR.DriverFumigationIn,      
 TFR.DriverLoadingStartTime,      
 TFR.DriverLoadingFinishTime,     
  TFR.DriverFumigationRelease,                     
 TFR.ReceiverName                                                                         
 FROM [dbo].[tblFumigationRouts] TFR          
 WHERE TFR.FumigationRoutsId = @FumigationRoutsId  AND TFR.IsDeleted=0                                 
 END                                                                           
-----------------------------------------------------------------------------------------------                                                                           
                              
END