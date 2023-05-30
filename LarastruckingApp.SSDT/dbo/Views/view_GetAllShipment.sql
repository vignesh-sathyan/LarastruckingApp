--select * from [dbo].[view_GetAllShipment]    
  
--DROP VIEW view_GetAllShipment   
              
CREATE   VIEW view_GetAllShipment                      
As                      
WITH         
                            
                              
                  CTE_Equipment(ShipmentId, Driver,DriverId,                             
                                   Equipment)                              
             AS (SELECT Distinct (SHP.ShipmentId),STRING_AGG (D.FirstName +' '+D.LastName, ',') AS DriverName , STRING_AGG(SED.DriverId , ',') as DriverId,STRING_AGG (ED.EquipmentNo, ',') AS EquipmentNo                     
FROM [dbo].[tblShipment] SHP With(NOLOCK)                    
left join [dbo].[tblShipmentEquipmentNdriver] SED on SED.ShipmentId=SHP.ShipmentId         
left join [dbo].[tblDriver] D on SED.DriverId=D.DriverId                     
left join [dbo].[tblEquipmentDetail] ED on SED.EquipmentId=ED.EDID                    
WHERE SHP.IsDeleted=0 AND ( SHP.StatusId=11  OR SHP.StatusId=8)               
GROUP BY SHP.ShipmentId),                              
                              
                    
         CTE_PickupNDeliveryLocation(ShipmentId, PickupDate,DeliveryDate,                              
                                   PickupLocation,DeliveryLocation)                              
                AS (SELECT Distinct (SHP.ShipmentId),        
    STRING_AGG (CONVERT(VARCHAR(MAX),CAST (PickDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '|') as PickDateTime ,        
    STRING_AGG (CONVERT(VARCHAR(MAX),CAST (DeliveryDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '|')   As DeliveryDateTime,        
    STRING_AGG ( CONCAT(AD.CompanyName,'||',AD.Address1,' ',AD.City,' ',STT.Name,' ',Ad.Zip), '$') as PickupLocation ,STRING_AGG (CONCAT(Ads.CompanyName,'||',Ads.Address1,' ',Ads.City,' ',STTs.Name,' ',Ads.Zip), '$')   As DeliveryLocation                 
  
   
FROM [dbo].[tblShipment] SHP With(NOLOCK)                    
Left join [dbo].[tblShipmentRoutesStop] TSRS on TSRS.ShippingId = SHP.ShipmentId                      
Left JOIN [dbo].[tblAddress] Ad ON TSRS.PickupLocationId =AD.AddressId                     
Left JOIN [dbo].[tblAddress] Ads ON TSRS.DeliveryLocationId =ADs.AddressId                                    
Left JOIN [dbo].[tblState] STT ON Ad.State=STT.ID    
Left join [dbo].[tblState] STTs on Ads.State=STTs.ID                              
WHERE  SHP.IsDeleted=0 AND TSRS.IsDeleted=0   AND ( SHP.StatusId=11  OR SHP.StatusId=8)                
            
GROUP BY SHP.ShipmentId),                
                                 
                                           
                              
   CTE_Quantity(ShipmentId,                           
                              Quantity)                              
             AS                   
                  
                    
    (SELECT DISTINCT                              
                        (SHP.ShipmentId),                               
                        STUFF(                              
                 (                              
                     SELECT '|' +                              
                     (                              
                               
                                                     
     CONCAT((select CASE WHEN SUM(T1.QuantityNweight)>0 THEN REPLACE(CAST(SUM(T1.QuantityNweight) AS nvarchar),'.00','')+' PLTS, ' ELSE '' END from [dbo].[tblShipmentFreightDetail] T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId AND T1.IsDeleted=0),' ',                              
                           (select CASE WHEN SUM(T1.NoOfBox)>0 THEN REPLACE(CAST(SUM(T1.NoOfBox) AS nvarchar),'.00','')+' BXS, ' ELSE '' END from [dbo].[tblShipmentFreightDetail] T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId  AND T1.IsDeleted=0),' ',                              
             (select CASE WHEN SUM(T1.Weight)>0 THEN REPLACE(CAST(SUM(T1.Weight) AS nvarchar),'.00','')+' LBS, ' ELSE '' END  from [dbo].[tblShipmentFreightDetail] T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId  AND (T1.Unit='LBS' OR T1.Unit='LB') AND T1.IsDeleted=0),' ',                                
                     (select CASE WHEN SUM(T1.Weight)>0 THEN REPLACE(CAST(SUM(T1.Weight) AS nvarchar),'.00','')+' KG ' ELSE '' END from [dbo].[tblShipmentFreightDetail] T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId AND T1.Unit='KG' AND T1.IsDeleted=0))                              
                                         
      )                              
                     FROM [dbo].[tblShipmentRoutesStop] AS SRS  WITH(NOLOCK)                            
      left JOIN [dbo].[tblShipmentFreightDetail] SFD ON SRS.ShippingRoutesId=SFD.ShipmentRouteStopeId                              
                     WHERE SRS.ShippingId = SHP.ShipmentId                              
                           AND SHP.IsDeleted = 0                              
                                                        
                     GROUP BY  SRS.ShippingRoutesId,                              
                              SRS.ShippingId FOR XML PATH('')                              
                 ), 1, 1, '') AS Quantity                              
                                   
     FROM [dbo].[tblShipment] SHP WITH(NOLOCK) WHERE SHP.IsDeleted=0 AND ( SHP.StatusId=11  OR SHP.StatusId=8))            
                                
                              
                      
select TotalCountRow=COUNT(SHP.ShipmentId) over(), SRS.PickDateTime, SHP.ShipmentId,(Case when SHP.AirWayBill is not null then SHP.AirWayBill ELSE case when SHP.CustomerPO IS NOT NULL THEN SHP.CustomerPO else SHP.OrderNo end end) as AirWayBill,SHP.CustomerPO,SHPS.StatusId,SFD.FreightTypeId, SHPS.StatusAbbreviation as StatusName,CR.CustomerID,CR.CustomerName,EQP.DriverId, EQP.Driver,EQP.Equipment,PDL.PickupDate,PDL.DeliveryDate,PDL.PickupLocation,PDL.DeliveryLocation              
,QT.Quantity                                
 from [dbo].[tblShipment] SHP With(NOLOCK)                       
Left Join [dbo].[tblShipmentRoutesStop]   SRS  ON SHP.ShipmentId=SRS.ShippingId                          
Left Join [dbo].[tblShipmentFreightDetail] SFD  ON SRS.ShippingRoutesId=SFD.ShipmentRouteStopeId                          
Left JOIN [dbo].[tblShipmentStatus]  SHPS ON SHP.StatusId=SHPS.StatusId               
Left JOIN [dbo].[tblCustomerRegistration] CR  ON SHP.CustomerId=CR.CustomerID              
Left JOIN CTE_Equipment EQP ON SHP.ShipmentId=EQP.ShipmentId                              
Left JOIN CTE_PickupNDeliveryLocation PDL ON SHP.ShipmentId=PDL.ShipmentId                              
Left JOIN CTE_Quantity QT ON SHP.ShipmentId=QT.ShipmentId                             
Where SHP.IsDeleted=0 and SRS.IsDeleted=0                      
AND ( SHP.StatusId=11  OR SHP.StatusId=8)  
  
--select SHP.ShipmentId, SRS.PickDateTime,SRS.ShippingRoutesId,(Case when SHP.AirWayBill is not null then SHP.AirWayBill ELSE case when SHP.CustomerPO IS NOT NULL THEN SHP.CustomerPO else SHP.OrderNo end end) as AirWayBill,SHP.CustomerPO from tblShipment SHP   
--left join  tblShipmentRoutesStop SRS on SHP.ShipmentId = SRS.ShippingId  
--WHERE SRS.PickDateTime BETWEEN '2021-01-01' AND '2021-01-10' AND SHP.IsDeleted=0 and SRS.IsDeleted=0                      
--AND ( SHP.StatusId=11  OR SHP.StatusId=8);  
  
--select * from [dbo].[view_GetAllShipment]                  