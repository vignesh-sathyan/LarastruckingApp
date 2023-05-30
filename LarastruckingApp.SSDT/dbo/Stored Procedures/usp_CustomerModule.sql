CREATE PROCEDURE [dbo].[usp_CustomerModule]                                        
(                                        
 @SpType INT = NULL,                                        
 @UserId INT = NULL,                                      
 @ShipmentId INT = NULL,                                    
 @ShippingRoutesId INT = NULL,         
 @ShipmentFreightDetailId INT = NULL,                                     
 @CustomerId INT = NULL,                                      
 @StartRowIndex INT = NULL,                                                              
 @PageSize INT = NULL,                                      
 @SearchText VARCHAR(100) = NULL                                        
)                                        
AS                                        
BEGIN                                         
 IF(@SpType = 1)                                        
 BEGIN                                        
 SET NOCOUNT ON;                                        
 SELECT * FROM (                                                              
    SELECT                                                               
     ROW_NUMBER() OVER(PARTITION BY TSEND.ShipmentId ORDER BY TSEND.ShipmentId ) RNO,                                                              
                 
     TCR.UserId,                                    
     TCR.CustomerID,                                                              
     COUNT(TSEND.ShipmentId) OVER(ORDER BY TSEND.ShipmentId) TotalRecord,                                     
     TS.AirWayBill,                                                             
     TSRS.PickDateTime,                                    
     TSRS.DeliveryDateTime,                                                  
     TSEND.EquipmentId,                                                              
     TSEND.ShipmentId,                                                              
     TS.ShipmentRefNo,                                                              
     TSS.StatusName,                
 -- ROW_NUMBER() OVER(PARTITION BY TSS.StatusName ORDER BY TSS.StatusName ) StatusName,                                                         
     CONCAT(TD.FirstName, TD.LastName) DriverName                                                             
                                                                 
    FROM [dbo].[tblShipment] TS                                          
    INNER JOIN [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerId =TS.CustomerID                                                   
    INNER JOIN [dbo].[tblShipmentStatusHistory] TSSH ON TSSH.ShipmentId = TS.ShipmentId                                 
    INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TSSH.StatusId                                                  
                                                              
    INNER JOIN [dbo].[tblDriver] TD ON TD.UserId = TCR.UserId                                  
    INNER JOIN [dbo].[tblShipmentRoutesStop] TSRS ON TSRS.ShippingId = TS.ShipmentId                                  
    INNER JOIN [dbo].tblShipmentEquipmentNdriver TSEND ON TSEND.ShipmentId = TS.ShipmentId                                                            
                                                                
  ) T                                                              
  WHERE                                                             
   UserId = @UserId AND                                                              
   ((StatusName LIKE '%'+ @SearchText +'%' OR NULLIF(@SearchText, '') IS NULL) OR                                                              
   (DriverName LIKE '%'+ @SearchText +'%' OR NULLIF(@SearchText, '') IS NULL) OR                                                              
   (ShipmentRefNo LIKE '%'+ @SearchText +'%' OR NULLIF(@SearchText, '') IS NULL))                                                             
                     
                                                                 
   AND T.RNO = 1                
 ORDER BY ShipmentId desc                                                            
  OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY                                            
 END                                        
--------------------------------------------------------------------------------------------------------------------------------                     
-- Multiple Routes Info basis of Shipment and Cutomer                                      
ELSE IF(@SpType = 2)                                      
BEGIN                                      
SET NOCOUNT ON;                                      
  SELECT                                                            
   RS.RouteNo RouteOrder,                                                              
   RS.ShippingId,                                      
   TS.CustomerId,                                                              
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
   DeliveryDateTime                             
                                                   
                                                              
  FROM [dbo].[tblShipmentRoutesStop] RS                                       
  INNER JOIN [dbo].[tblShipment] TS ON TS.ShipmentId = RS.ShippingId                                       
  INNER JOIN [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerID = TS.CustomerId                              
  INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = RS.PickupLocationId                                                         
  INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                              
  INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                                              
  INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = RS.DeliveryLocationId                                                              
  INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                                                              
  INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country                                                           
                                                           
                                                            
  WHERE                                                               
  ShippingId = @ShipmentId                                                          
  ORDER BY RouteOrder                                      
                                      
END                                      
 -------------------------------------------------------------------------------                                     
 -- Get Pre Trip Shipping Detail Based on Route Id                                                    
ELSE IF(@SpType = 3)                                                            
 BEGIN                             
 SET NOCOUNT ON;      
      
  ;With cte_getStatus as                                
(                                
select SS.StatusName, SH.StatusId, SH.ShipmentId from tblShipmentStatusHistory SH                                
inner join (select MAX(CreatedOn) as MaxDate,ShipmentId from tblShipmentStatusHistory group by ShipmentId) ssh on SH.ShipmentId=ssh.ShipmentId and SH.CreatedOn = ssh.MaxDate                                
inner join tblShipmentStatus SS on SH.StatusId = SS.StatusId                                
)      
                                                           
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
   CONCAT(A1.CompanyName ,', ', A1.Address1 + ' ' + A1.City  + ' ' + S1.Name + ' ' + CONVERT(Varchar(200),A1.Zip) + ' ' + C1.Name) PickUpLocation ,                                                          
   CONCAT(A2.CompanyName ,', ', A2.Address1 + ' ' + A2.City  + ' ' + S2.Name + ' ' + CONVERT(Varchar(200),A2.Zip) + ' ' + C2.Name) DeliveryAddress,                                             
   A1.Phone as PickUpPhone ,                                                           
   A1.Extension as PickUpExtension,                                                          
   TSRS.PickDateTime as PickUpArrivalDate,                                                          
   TSRS.DeliveryDateTime as DeliveryArrive,                                                          
   A2.Phone as DeliveryPhone,                                          
   A2.Extension as DeliveryExtension,               
   TSRS.DigitalSignature,              
   TSRS.ReceiverName,                            
   --For Customer Status and sub-Status ---                              
  -- TSS.StatusName,        
  CTES.StatusName,                            
   TSSS.SubStatusName,                           
   TSSH.Reason,                  
   TSSH.StatusId,                  
   TSSH.SubStatusId                       
   --------------------------------------                       
                                                   
                                                       
  FROM [dbo].[tblShipment] TS                                         
  INNER JOIN [dbo].[tblShipmentRoutesStop] TSRS ON TSRS.ShippingId = TS.ShipmentId                              
  INNER JOIN [dbo].[tblShipmentStatusHistory] TSSH ON TSSH.ShipmentId = TS.ShipmentId                                
  INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TSSH.StatusId                                
  LEFT JOIN [dbo].[tblShipmentSubStatus] TSSS ON TSSS.SubStatusId = TSSH.SubStatusId                                                            
  INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = TSRS.PickupLocationId                                                          
  INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                          
  INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                                          
  INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = TSRS.DeliveryLocationId                                                 
  INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                                                          
  INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country        
  INNER JOIN cte_getStatus  CTES ON TS.ShipmentId =CTES.ShipmentId                                                       
  WHERE                         
  TSRS.ShippingRoutesId = @ShippingRoutesId                                                            
 END                                                          
-----------------------------------------------------------------------------------------------                                                                   
                                          
   -- Bind the Damaged Files                                                                   
                                                                
   ELSE IF(@SpType = 4)                                                                                  
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
  TDI.ShipmentRouteID = @ShippingRoutesId  And TDI.IsApproved = 1 AND TDI.IsDeleted = 0                                                      
 END                                                                  
---------------------------------------------------------------------------------------------------                                                                  
   ELSE IF(@SpType = 5)                                                                                  
 BEGIN                                                                                  
 SET NOCOUNT ON;                                                       
  SELECT                                                                                                                    
 TPOTI.ImageId proofImageId,                                                                         
 TPOTI.ShipmentRouteId,         
 TPOTI.ShipmentFreightDetailId,                                                                       
 ISNULL(TPOTI.ImageName,'') ProofImage,                                                                        
 ISNULL(TPOTI.ImageDescription,'') ProofDescription,                                                                  
 TPOTI.ActualTemperature proofActualTemp,               
 ISNULL( TPOTI.ImageUrl,'')ImageUrl ,                                                                 
 TPOTI.CreatedOn ProofDate                                                                  
                                                                     
  FROM [dbo].[tblProofOfTemperatureImages] TPOTI                                                                    
  WHERE                                                                                 
  TPOTI.ShipmentRouteId = @ShippingRoutesId And TPOTI.ShipmentFreightDetailId=@ShipmentFreightDetailId And TPOTI.IsApproved = 1 And TPOTI.IsDeleted = 0                  
                                                                                     
 END     
 ----------------------------------------------------------------------------------------------------------------------------    
     
 ELSE IF(@SpType = 6)                                         
 BEGIN                                                                              
 SET NOCOUNT ON;       
      
        
 --;with CTE_TotalQuantity(ShipmentId, QuantityNweight)                            
 --      AS (SELECT  ShippingId, CONVERT(varchar(max),concat(replace(cast(TSFD.QuantityNweight as varchar),'.00','')  , '' )) as QuantityNweight        
 --    FROM tblShipmentRoutesStop FR       
 -- inner join tblShipmentFreightDetail TSFD ON TSFD.ShipmentId = FR.ShippingId           
 -- group By TSFD.ShipmentId,TSFD.QuantityNweight        
 --       )      
    
 --;with CTE_TotalQuantity(ShipmentRouteStopeId, QuantityNweight)                            
 --      AS (SELECT DISTINCT (ShipmentRouteStopeId), CONVERT(varchar(max),concat(replace(cast(TSFD.QuantityNweight as varchar),'.00','')  , '' )) as QuantityNweight        
 --    FROM tblShipmentFreightDetail TSFD       
 --  --  where ShipmentRouteStopeId = 151     
         
 --group By TSFD.ShipmentRouteStopeId ,TSFD.QuantityNweight        
 --       )     
    
                                                                            
  SELECT distinct                                                             
   TSFD.ShipmentBaseFreightDetailId,                                                                          
   TSFD.ShipmentId,                                                                            
   TSFD.ShipmentRouteStopeId ,                               
   ISNULL(TSFD.Commodity,'') Commodity,                                                                          
   ISNULL(TFT.FreightTypeName,'') FreightTypeName,    
   TSFD.FreightTypeId,                                                                          
   ISNULL(TPM.PricingMethodName,'') PricingMethodName,    
   TSFD.PricingMethodId,                                                                          
  --replace(cast(TSFD.QuantityNweight as varchar),'.00','') QuantityNweight,                                                                      
  QuantityNweight= case when TSFD.QuantityNweight>0.00 then  replace(cast(TSFD.QuantityNweight as varchar),'.00',' '+'Pallets') else '' end,      
  NoOfBox= case when TSFD.NoOfBox >0 then ( cast(TSFD.NoOfBox as varchar) +' '+'Box' ) else '' end ,                                                                              
   WeightUnit= case when TSFD.Weight>0.00 then  cast(TSFD.Weight  as varchar) +' '+ISNULL(TSFD.Unit,'') else '' end,    
   CONCAT(TSFD.Temperature,' ',TSFD.TemperatureType) as TemperatureRequired                                                                        
                                                                           
  FROM [dbo].[tblShipmentFreightDetail] TSFD                                                                             
  INNER JOIN [dbo].[tblFreightType] TFT ON TFT.FreightTypeId = TSFD.FreightTypeId                                                                            
  INNER JOIN [dbo].[tblPricingMethod] TPM ON TPM.PricingMethodId = TSFD.PricingMethodId       
--  LEFT JOIN CTE_TotalQuantity TQ ON TQ.ShipmentRouteStopeId = TSFD.ShipmentRouteStopeId                                                                         
  WHERE                                                                             
   TSFD.ShipmentRouteStopeId = @ShippingRoutesId AND TSFD.IsDeleted=0                                                                        
 END                                                                            
                                         
    
                                       
END