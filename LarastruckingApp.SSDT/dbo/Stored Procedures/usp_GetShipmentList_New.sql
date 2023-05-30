--select * from tblDriverGpsTrakingHistory order by DriverGpsId desc      
--sp_helptext usp_GetShipmentList_New      
      
--exec usp_GetALLShipmentList '','ShipmentId','desc',0,2000,'2021-02-01','2021-02-17'        
CREATE PROC [dbo].[usp_GetShipmentList_New]                      
(@SearchTerm VARCHAR(50),                       
 @SortColumn VARCHAR(50),                       
 @SortOrder  VARCHAR(50),                       
 @PageNumber INT,                       
 @PageSize   INT,                  
 @StartDate   DATE=NULL,                  
 @EndDate   DATE=NULL ,                  
@CustomerId int=null,                  
@StatusId int=null,                  
@FreightTypeId int= null,     
@DriverName  VARCHAR(50) =null                   
 --@TotalCount INT OUT                      
)                      
AS                      
    BEGIN                       
 SET NOCOUNT ON;                        
 SET @SortColumn = LOWER(ISNULL(@SortColumn, ''));                      
        SET @SortOrder = LOWER(ISNULL(@SortOrder, ''));                      
        SET @SearchTerm = ISNULL(@SearchTerm, '');                           
                      
   SELECT        
            SRS.PickDateTime,       
   SHP.StatusId,      
            SHP.ShipmentId,      
   SHP.CustomerId ,      
   SRS.ShippingRoutesId,      
            (CASE      
                 WHEN SHP.AirWayBill IS NOT NULL      
                 THEN SHP.AirWayBill      
                 ELSE CASE      
                          WHEN SHP.CustomerPO IS NOT NULL      
                          THEN SHP.CustomerPO      
                          ELSE SHP.OrderNo      
                      END      
             END) AS AirWayBill,       
            SHP.CustomerPO,
			SHP.OrderNo,
			SHP.CustomerRef,
			SHP.ContainerNo,
			 SHP.PurchaseDoc,      
   SRS.PickupLocationId,      
   SRS.DeliveryLocationId,      
   SRS.DeliveryDateTime      
   INTO #ShipmentList      
     FROM [dbo].[tblShipment] SHP WITH(NOLOCK)      
     LEFT JOIN [dbo].[tblShipmentRoutesStop] SRS ON SHP.ShipmentId = SRS.ShippingId       
     Where SHP.IsDeleted=0 and SRS.IsDeleted=0                                         
     AND ( SHP.StatusId=11  OR SHP.StatusId=8)       
  AND ((CAST(PickDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS DATE) >= @StartDate)                      
     AND (CAST(PickDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS DATE) <=  @EndDate  ) )       
     AND ((NullIf(@CustomerId, '') IS NULL)  OR SHP.CustomerId = @CustomerId)                                
     AND ((NullIf(@StatusId, '') IS NULL)  OR SHP.StatusId = @StatusId)   ;      
                        
              
   --drop table #ShipmentList      
   -- select * from  #ShipmentList      
        
WITH        
                  CTE_Equipment(ShipmentId, Driver,DriverId,                                     
                                   Equipment)                                      
             AS (SELECT Distinct (SHP.ShipmentId),STRING_AGG (D.FirstName +' '+D.LastName, ',') AS DriverName , STRING_AGG(SED.DriverId , ',') as DriverId,STRING_AGG (ED.EquipmentNo, ',') AS EquipmentNo                             
FROM #ShipmentList SHP                             
left join [dbo].[tblShipmentEquipmentNdriver] SED on SED.ShipmentId=SHP.ShipmentId                 
left join [dbo].[tblDriver] D on SED.DriverId=D.DriverId                             
left join [dbo].[tblEquipmentDetail] ED on SED.EquipmentId=ED.EDID                                 
GROUP BY SHP.ShipmentId),                                      
                                  
         CTE_PickupNDeliveryLocation(ShipmentId, PickupDate,DeliveryDate,                                      
                                   PickupLocation,DeliveryLocation)                                      
                AS (SELECT Distinct (SHP.ShipmentId),                
    STRING_AGG (CONVERT(VARCHAR(MAX),CAST (PickDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '|') as PickDateTime ,                
    STRING_AGG (CONVERT(VARCHAR(MAX),CAST (DeliveryDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '|')   As DeliveryDateTime,                
    STRING_AGG ( CONCAT(AD.CompanyName,'||',AD.Address1,' ',AD.City,' ',STT.Name,' ',Ad.Zip), '$') as PickupLocation ,STRING_AGG (CONCAT(Ads.CompanyName,'||',Ads.Address1,' ',Ads.City,' ',STTs.Name,' ',Ads.Zip), '$')   As DeliveryLocation                 
  
    
         
FROM #ShipmentList SHP With(NOLOCK)                                                 
Left JOIN [dbo].[tblAddress] Ad ON SHP.PickupLocationId =AD.AddressId                             
Left JOIN [dbo].[tblAddress] Ads ON SHP.DeliveryLocationId =ADs.AddressId                                            
Left JOIN [dbo].[tblState] STT ON Ad.State=STT.ID            
Left join [dbo].[tblState] STTs on Ads.State=STTs.ID                                                        
GROUP BY SHP.ShipmentId),         
      
  CTE_Quantity(ShipmentId,Quantity)                                      
             AS                           
                          
                            
    (SELECT DISTINCT                                      
                        (SHP.ShipmentId),                                       
                        STUFF(                                      
                 (                                      
                     SELECT '|' +                                      
                     (                                                                                                 
     CONCAT((select CASE WHEN SUM(T1.QuantityNweight)>0 THEN REPLACE(CAST(SUM(T1.QuantityNweight) AS nvarchar),'.00','')+' PLTS, ' ELSE '' END from [dbo].[tblShipmentFreightDetail] T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId AND T1.IsDeleted=0),' ',                                      
                           (select CASE WHEN SUM(T1.NoOfBox)>0 THEN REPLACE(CAST(SUM(T1.NoOfBox) AS nvarchar),'.00','')+' BXS, ' ELSE '' END from [dbo].[tblShipmentFreightDetail] T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId  AND T1.IsDeleted=0),' ',                                      
             (select CASE WHEN SUM(T1.Weight)>0 THEN REPLACE(CAST(SUM(T1.Weight) AS nvarchar),'.00','')+' LBS, ' ELSE '' END  from [dbo].[tblShipmentFreightDetail] T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId  AND   (T1.Unit='LBS' OR T1.Unit='LB') AND T1.IsDeleted=0),' ',                                        
                     (select CASE WHEN SUM(T1.Weight)>0 THEN REPLACE(CAST(SUM(T1.Weight) AS nvarchar),'.00','')+' KG ' ELSE '' END from [dbo].[tblShipmentFreightDetail] T1 WHERE T1.ShipmentId=SHP.ShipmentId AND T1.ShipmentRouteStopeId=SRS.ShippingRoutesId
  

 AND T1.Unit='KG' AND T1.IsDeleted=0))                                      
                                                 
      )                                      
                     FROM #ShipmentList AS SRS  WITH(NOLOCK)                                    
      left JOIN [dbo].[tblShipmentFreightDetail] SFD ON SRS.ShippingRoutesId=SFD.ShipmentRouteStopeId                                      
                     WHERE SRS.ShipmentId = SHP.ShipmentId                                      
                                                          
                                                                
                     GROUP BY  SRS.ShippingRoutesId,                                      
                              SRS.ShipmentId FOR XML PATH('')                                      
                 ), 1, 1, '') AS Quantity                                      
                                           
     FROM #ShipmentList SHP )          
        
        
          
select  [TotalCount]= COUNT(SHP.ShipmentId) over(), SHP.ShipmentId,SHP.AirWayBill,SHPS.StatusId,SFD.FreightTypeId, SHPS.StatusAbbreviation as StatusName,CR.CustomerID,CR.CustomerName,EQP.DriverId, EQP.Driver,EQP.Equipment,PDL.PickupDate,PDL.DeliveryDate,PDL.PickupLocation,PDL.DeliveryLocation ,QT.Quantity                                        
 from #ShipmentList SHP                                              
Left Join [dbo].[tblShipmentFreightDetail] SFD  ON SHP.ShippingRoutesId=SFD.ShipmentRouteStopeId        
Left JOIN [dbo].[tblShipmentStatus]  SHPS ON SHP.StatusId=SHPS.StatusId                       
Left JOIN [dbo].[tblCustomerRegistration] CR  ON SHP.CustomerId=CR.CustomerID                      
Left JOIN CTE_Equipment EQP ON SHP.ShipmentId=EQP.ShipmentId                                      
Left JOIN CTE_PickupNDeliveryLocation PDL ON SHP.ShipmentId=PDL.ShipmentId                                      
Left JOIN CTE_Quantity QT ON SHP.ShipmentId=QT.ShipmentId                                     
WHERE          
                  
     (                  
      ((NullIf(@FreightTypeId , '') IS NULL)  OR FreightTypeId = @FreightTypeId)  -- (',' + RTRIM(FreightTypeId) + ',') LIKE '%,' +  CAST(@FreightTypeId AS varchar)+ ',%')                 
   )                  
       AND(                    
      ((NullIf(@DriverName, '') IS NULL)  OR EQP.Driver  LIKE '%' + @DriverName + '%')                      
   )      
      AND              
                        (                      
                         AirWayBill LIKE '%' + @SearchTerm + '%'                       
                        OR CustomerPO LIKE '%' + @SearchTerm + '%'  
						 OR OrderNo LIKE '%' + @SearchTerm + '%' 
		              OR CustomerRef LIKE '%' + @SearchTerm + '%' 
		         OR ContainerNo LIKE '%' + @SearchTerm + '%' 
		           OR PurchaseDoc LIKE '%' + @SearchTerm + '%'                      
                       OR StatusName LIKE '%' + @SearchTerm + '%'                                                                               
                        OR CustomerName LIKE '%' + @SearchTerm + '%'                      
                        OR Driver LIKE '%' + @SearchTerm + '%'                      
                        OR Equipment LIKE '%' + @SearchTerm + '%'                      
                        OR PickupDate LIKE '%' + @SearchTerm + '%'                      
                        OR DeliveryDate LIKE '%' + @SearchTerm + '%'                      
                        OR PickupLocation LIKE '%' + @SearchTerm + '%'                      
                        OR DeliveryLocation LIKE '%' + @SearchTerm + '%'                      
                        )                 
     group by SHP.ShipmentId,SHP.AirWayBill,SHPS.StatusId,SFD.FreightTypeId, SHPS.StatusAbbreviation,CR.CustomerID,CR.CustomerName,EQP.DriverId, EQP.Driver,EQP.Equipment,PDL.PickupDate,PDL.DeliveryDate,PDL.PickupLocation,PDL.DeliveryLocation ,QT.Quantity 
 
    
                                        
ORDER BY CASE                      
                          WHEN(@SortColumn = 'ShipmentId'                      
                               AND @SortOrder = 'asc')                      
                          THEN SHP.ShipmentId                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'ShipmentId'                      
                               AND @SortOrder = 'desc')                      
                          THEN SHP.ShipmentId                      
                      END DESC,                      
                      CASE                      
                          WHEN(@SortColumn = 'StatusName'                      
                               AND @SortOrder = 'asc')                      
 THEN SHPS.StatusAbbreviation                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'StatusName'                      
                               AND @SortOrder = 'desc')                      
                          THEN SHPS.StatusAbbreviation                      
                      END DESC,                      
                      CASE                      
                          WHEN(@SortColumn = 'CustomerName'           
                               AND @SortOrder = 'asc')                      
                          THEN CustomerName                      
   END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'CustomerName'                  
                               AND @SortOrder = 'desc')                      
                          THEN CustomerName                      
                      END DESC,                      
                      CASE                      
                          WHEN(@SortColumn = 'AirWayBill'                      
                               AND @SortOrder = 'asc')                      
            THEN AirWayBill                      
                      END ASC,                      
                      CASE                
                          WHEN(@SortColumn = 'AirWayBill'                      
                               AND @SortOrder = 'desc')                      
                          THEN AirWayBill                      
                      END DESC,                      
                            
            CASE                      
                          WHEN(@SortColumn = 'Driver'                      
                               AND @SortOrder = 'asc')                      
           THEN Driver                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'Driver'                      
                               AND @SortOrder = 'desc')                      
                          THEN  Driver                      
                      END DESC,                      
                      
         CASE                      
                          WHEN(@SortColumn = 'Equipment'                      
                               AND @SortOrder = 'asc')                      
                          THEN Equipment                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'Equipment'                      
                               AND @SortOrder = 'desc')                      
                          THEN  Equipment                      
                      END DESC,                      
        CASE                      
                          WHEN(@SortColumn = 'PickupDate'                      
                               AND @SortOrder = 'asc')                      
                          THEN PickupDate                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'PickupDate'                      
                               AND @SortOrder = 'desc')                      
                          THEN  PickupDate                      
                      END DESC,                      
        CASE                      
                          WHEN(@SortColumn = 'DeliveryDate'                      
                               AND @SortOrder = 'asc')                      
                          THEN DeliveryDate                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DeliveryDate'                      
                               AND @SortOrder = 'desc')                
                          THEN  DeliveryDate                      
                      END DESC,                      
            CASE                      
                          WHEN(@SortColumn = 'PickupLocation'                      
                               AND @SortOrder = 'asc')                      
                          THEN PickupLocation                      
                      END ASC,                     
                      CASE                      
                          WHEN(@SortColumn = 'PickupLocation'                      
                               AND @SortOrder = 'desc')                      
                          THEN  PickupLocation                      
                      END DESC,                      
             CASE                      
                          WHEN(@SortColumn = 'DeliveryLocation'                      
                AND @SortOrder = 'asc')                      
                          THEN DeliveryLocation                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DeliveryLocation'                      
                               AND @SortOrder = 'desc')                      
                          THEN DeliveryLocation                      
                      END DESC                      
        OFFSET @PageNumber ROWS FETCH NEXT case when @PageSize>0 then @PageSize else 999999 end ROWS ONLY;       
  DROP table #ShipmentList                         
END;