            
CREATE view [dbo].[view_GetOrderTakenShipment]              
As              
WITH                   
                      
                                        
                      
--      CTE_PickupNDeliveryDate(ShipmentId,                       
--                                   PickupDate,DeliveryDate)                      
--                AS (SELECT Distinct (SHP.ShipmentId),STRING_AGG (CONVERT(VARCHAR(MAX),CAST (PickDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '|') as PickDateTime ,STRING_AGG (CONVERT(VARCHAR(MAX),CAST (DeliveryDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '|')   As DeliveryDateTime            
--FROM tblShipment SHP With(NOLOCK)            
--left join tblShipmentRoutesStop SRS on SHP.ShipmentId=SRS.ShippingId                
--WHERE  SHP.IsDeleted=0 AND SRS.IsDeleted=0  AND SHP.StatusId=1           
--GROUP BY SHP.ShipmentId            
--),                      
                      
         CTE_PickupNDeliveryLocation(ShipmentId, PickupDate,DeliveryDate,                       
                                   PickupLocation,DeliveryLocation)                      
                AS (SELECT Distinct (SHP.ShipmentId),STRING_AGG (CONVERT(VARCHAR(MAX),CAST (PickDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '|') as PickDateTime ,STRING_AGG (CONVERT(VARCHAR(MAX),CAST (DeliveryDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '|')   As DeliveryDateTime,STRING_AGG ( CONCAT(AD.CompanyName,'||',AD.Address1,' ',AD.City,' ',STT.Name,' ',Ad.Zip), '$') as PickupLocation ,STRING_AGG (CONCAT(Ads.CompanyName,'||',Ads.
Address1,' ',Ads.City,' ',STTs.Name,' ',Ads.Zip), '$')   As DeliveryLocation            
FROM tblShipment SHP With(NOLOCK)            
Left join  tblShipmentRoutesStop TSRS on TSRS.ShippingId = SHP.ShipmentId              
Left JOIN tblAddress Ad ON TSRS.PickupLocationId =AD.AddressId             
Left JOIN tblAddress Ads ON TSRS.DeliveryLocationId =ADs.AddressId                            
Left JOIN tblState STT ON Ad.State=STT.ID  
Left JOIN tblState STTs ON Ads.State=STTs.ID                         
WHERE  SHP.IsDeleted=0 AND TSRS.IsDeleted=0  AND SHP.StatusId=1            
GROUP BY SHP.ShipmentId),                      
                      
   CTE_Quantity(ShipmentId,                   
                              Quantity)                      
             AS (SELECT DISTINCT                      
                        (SHP.ShipmentId),                       
                        STUFF(                      
                 (                      
                     SELECT '|' +                      
                     (                      
                          
                                             
     CONCAT((select CASE WHEN SUM(T1.QuantityNweight)>0 THEN REPLACE(CAST(SUM(T1.QuantityNweight) AS nvarchar),'.00','')+' PLTS, ' ELSE '' END from tblShipmentFreightDetail T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId AND T1.IsDeleted=0),' ',                      
                           (select CASE WHEN SUM(T1.NoOfBox)>0 THEN REPLACE(CAST(SUM(T1.NoOfBox) AS nvarchar),'.00','')+' BXS, ' ELSE '' END from tblShipmentFreightDetail T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId  AND T1.IsDeleted=0),' ',                      
             (select CASE WHEN SUM(T1.Weight)>0 THEN REPLACE(CAST(SUM(T1.Weight) AS nvarchar),'.00','')+' LBS, ' ELSE '' END  from tblShipmentFreightDetail T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId  AND (T1.Unit='LBS' OR T1.Unit='LB') AND T1.IsDeleted=0),' ',                        
                     (select CASE WHEN SUM(T1.Weight)>0 THEN REPLACE(CAST(SUM(T1.Weight) AS nvarchar),'.00','')+' KG ' ELSE '' END from tblShipmentFreightDetail T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId                 
AND T1.Unit='KG' AND T1.IsDeleted=0))                                                     
      )                      
                     FROM tblShipmentRoutesStop AS SRS  WITH(NOLOCK)                    
      left JOIN tblShipmentFreightDetail SFD ON SRS.ShippingRoutesId=SFD.ShipmentRouteStopeId                      
                     WHERE SRS.ShippingId = SHP.ShipmentId                      
                           AND SHP.IsDeleted = 0                      
                                                
                     GROUP BY  SRS.ShippingRoutesId,                      
                              SRS.ShippingId FOR XML PATH('')                      
                 ), 1, 1, '') AS Quantity                      
                           
     FROM tblShipment SHP WITH(NOLOCK) WHERE SHP.IsDeleted=0  AND SHP.StatusId=1  )                      
                      
              
select TotalCountRow=COUNT(SHP.ShipmentId) over(),SHP.ShipmentId, (Case when SHP.AirWayBill is not null then SHP.AirWayBill ELSE case when SHP.CustomerPO IS NOT NULL THEN SHP.CustomerPO else SHP.OrderNo end end) as AirWayBill,SHP.CustomerPO,SHP.OrderNo,SHP.CustomerRef,SHP.ContainerNo,SHP.PurchaseDoc, SHPS.StatusId,SHPS.StatusName,CR.CustomerID,CR.CustomerName,PDL.PickupDate,PDL.DeliveryDate,PDL.PickupLocation,PDL.DeliveryLocation,QT.Quantity ,CONCAT(USR.FirstName,' ',ISNULL(USR.LastName ,''))  as CreatedByName, SHP.IsReady                     
 from [dbo].[tblShipment] SHP With(NOLOCK)                         
Left JOIN [dbo].[tblShipmentStatus]  SHPS ON SHP.StatusId=SHPS.StatusId      
Left JOIN [dbo].[tblUser] USR ON SHP.CreatedBy = USR.Userid               
Left JOIN [dbo].[tblCustomerRegistration] CR  ON SHP.CustomerId=CR.CustomerID                                      
--Left JOIN CTE_PickupNDeliveryDate PD ON SHP.ShipmentId=PD.ShipmentId                      
Left JOIN CTE_PickupNDeliveryLocation PDL ON SHP.ShipmentId=PDL.ShipmentId                      
Left JOIN CTE_Quantity QT ON SHP.ShipmentId=QT.ShipmentId                
Where SHP.IsDeleted=0 AND SHPS.StatusId=1