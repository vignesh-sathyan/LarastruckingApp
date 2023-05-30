CREATE PROC [dbo].[usp_GetShipmentList]      
(@SearchTerm VARCHAR(50),       
 @SortColumn VARCHAR(50),       
 @SortOrder  VARCHAR(50),       
 @PageNumber INT,       
 @PageSize   INT,  
 @StartDate   DATE=NULL,  
 @EndDate   DATE=NULL ,  
@CustomerId int=null,  
@StatusId int=null,  
@FreightTypeId int= null  
 --@TotalCount INT OUT      
)      
AS      
    BEGIN       
 SET NOCOUNT ON;        
 SET @SortColumn = LOWER(ISNULL(@SortColumn, ''));      
        SET @SortOrder = LOWER(ISNULL(@SortOrder, ''));      
        SET @SearchTerm = ISNULL(@SearchTerm, '');      
WITH CTE_Driver(ShipmentId,       
                              Driver)      
             AS (SELECT DISTINCT      
                        (SHP.ShipmentId),       
                        STUFF(      
                 (      
                     SELECT ', ' +      
                     (      
                         SELECT CONCAT(D.FirstName, ' ', D.LastName) DriverName      
                         FROM tblDriver D      
                         WHERE D.DriverId = SED.DriverId      
                     )      
                     FROM tblShipmentEquipmentNdriver AS SED      
                     WHERE SED.ShipmentId = SHP.ShipmentId      
                           AND SHP.IsDeleted = 0      
                                
                     GROUP BY SED.DriverId,       
                              SED.ShipmentId FOR XML PATH('')      
                 ), 1, 1, '') AS Driver      
     FROM tblShipment SHP WHERE SHP.IsDeleted=0),      
      
                  CTE_Equipment(ShipmentId,       
                                   Equipment)      
             AS (SELECT DISTINCT      
                        (SHP.ShipmentId),       
                        STUFF(      
                 (      
                     SELECT ', ' +      
                     (      
                         SELECT EquipmentNo      
                         FROM tblEquipmentDetail      
                         WHERE EDID = SED.EquipmentId      
                     )      
                     FROM tblShipmentEquipmentNdriver AS SED      
                     WHERE SED.ShipmentId = SHP.ShipmentId      
                                 
                     GROUP BY SED.EquipmentId,       
                              SED.ShipmentId FOR XML PATH('')      
                 ), 1, 1, '') AS DeliveryEquipment      
                 FROM tblShipment SHP WHERE SHP.IsDeleted=0),      
      
      CTE_PickupNDeliveryDate(ShipmentId,       
                                   PickupDate,DeliveryDate)      
                AS (SELECT DISTINCT      
                        (SHP.ShipmentId),       
                        STUFF(      
                 (      
                     SELECT '|' +      
                     (      
                             
                     SELECT  CONVERT(VARCHAR(MAX),CAST (PickDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS      
                        varchar )) AS PickupDate      
                         FROM   tblShipmentRoutesStop TSRS      
                         WHERE TSRS.ShippingRoutesId = SRSS.ShippingRoutesId      
                     )      
                     FROM tblShipmentRoutesStop AS SRSS      
                     WHERE SRSS.ShippingId = SHP.ShipmentId                                 
                     GROUP BY        
                              SRSS.ShippingRoutesId FOR XML PATH('')      
                 ), 1, 1, '') AS PickupDate,      
      STUFF(      
                 (      
                     SELECT '|' +      
                     (      
                             
                     SELECT  CONVERT(VARCHAR(MAX),CAST (DeliveryDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS      
                        varchar )) AS DeliveryDate      
                         FROM   tblShipmentRoutesStop TSRS      
                         WHERE TSRS.ShippingRoutesId = SRSS.ShippingRoutesId      
                     )      
                     FROM tblShipmentRoutesStop AS SRSS      
                     WHERE SRSS.ShippingId = SHP.ShipmentId                                 
                     GROUP BY        
                              SRSS.ShippingRoutesId FOR XML PATH('')      
                 ), 1, 1, '') AS DeliveryDate      
      
      
            FROM tblShipment SHP       
     Left JOIN [dbo].[tblShipmentRoutesStop] SRS ON SHP.ShipmentId=SRS.ShippingId      
     WHERE SHP.IsDeleted=0 AND SRS.IsDeleted=0),      
      
         CTE_PickupNDeliveryLocation(ShipmentId,       
                                   PickupLocation,DeliveryLocation)      
                AS (SELECT DISTINCT      
                        (SHP.ShipmentId),       
                        STUFF(      
                 (      
                     SELECT '$' +      
                     (      
                             
                     SELECT CONCAT(AD.CompanyName,'||',AD.Address1,' ',AD.City,' ',STT.Name,' ',Ad.Zip)      
                           AS PickupLocation      
                         FROM   tblShipmentRoutesStop TSRS      
       Left JOIN tblAddress Ad ON TSRS.PickupLocationId =AD.AddressId      
       Left JOIN tblState STT ON Ad.State=STT.ID      
                         WHERE TSRS.ShippingRoutesId = SRSS.ShippingRoutesId      
                     )      
                     FROM tblShipmentRoutesStop AS SRSS      
                     WHERE SRSS.ShippingId = SHP.ShipmentId                                 
                     GROUP BY        
                              SRSS.ShippingRoutesId FOR XML PATH('')      
                 ), 1, 1, '') AS PickUpLocation,      
      
        STUFF(      
                 (      
                     SELECT '$' +      
                     (      
                             
                     SELECT CONCAT(AD.CompanyName,'||',AD.Address1,' ',AD.City,' ',STT.Name,' ',Ad.Zip)      
                           AS PickupLocation      
                         FROM   tblShipmentRoutesStop TSRS      
       Left JOIN tblAddress Ad ON TSRS.DeliveryLocationId =AD.AddressId      
       Left JOIN tblState STT ON Ad.State=STT.ID      
                         WHERE TSRS.ShippingRoutesId = SRSS.ShippingRoutesId      
                     )      
                     FROM tblShipmentRoutesStop AS SRSS      
                     WHERE SRSS.ShippingId = SHP.ShipmentId                                 
                     GROUP BY        
                              SRSS.ShippingRoutesId FOR XML PATH('')      
                 ), 1, 1, '') AS DeliveryLocation      
      
      
                 FROM tblShipment SHP       
     Left JOIN [dbo].[tblShipmentRoutesStop] SRS ON SHP.ShipmentId=SRS.ShippingId      
     WHERE SHP.IsDeleted=0 AND SRS.IsDeleted=0),      
      
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
                     FROM tblShipmentRoutesStop AS SRS      
      left JOIN tblShipmentFreightDetail SFD ON SRS.ShippingRoutesId=SFD.ShipmentRouteStopeId      
                     WHERE SRS.ShippingId = SHP.ShipmentId      
                           AND SHP.IsDeleted = 0      
                                
                     GROUP BY  SRS.ShippingRoutesId,      
                              SRS.ShippingId FOR XML PATH('')      
                 ), 1, 1, '') AS Quantity      
           
     FROM tblShipment SHP WHERE SHP.IsDeleted=0)      
      
select [TotalCount]= COUNT(*) over(),SHP.ShipmentId,SHP.AirWayBill,SHP.CustomerPO, SHPS.StatusName,CR.CustomerName,DRV.Driver,EQP.Equipment,PD.PickupDate,PD.DeliveryDate,PDL.PickupLocation,PDL.DeliveryLocation,QT.Quantity
--[TotalCount] = COUNT(*) OVER()       
--INTO #SHIPMENTDATA       
 from [dbo].[tblShipment] SHP  
Left Join [dbo].[tblShipmentRoutesStop]   SRS ON SHP.ShipmentId=SRS.ShippingId  
Left Join [dbo].[tblShipmentFreightDetail] SFD ON SRS.ShippingRoutesId=SFD.ShipmentRouteStopeId  
Left JOIN [dbo].[tblShipmentStatus]  SHPS ON SHP.StatusId=SHPS.StatusId      
Left JOIN [dbo].[tblCustomerRegistration] CR ON SHP.CustomerId=CR.CustomerID      
Left JOIN CTE_Driver DRV ON SHP.ShipmentId=DRV.ShipmentId      
Left JOIN CTE_Equipment EQP ON SHP.ShipmentId=EQP.ShipmentId      
Left JOIN CTE_PickupNDeliveryDate PD ON SHP.ShipmentId=PD.ShipmentId      
Left JOIN CTE_PickupNDeliveryLocation PDL ON SHP.ShipmentId=PDL.ShipmentId      
Left JOIN CTE_Quantity QT ON SHP.ShipmentId=QT.ShipmentId      
WHERE SHP.IsDeleted=0 and SFD.IsDeleted=0 AND SRS.IsDeleted=0 AND ( SHPS.StatusId=11  OR SHPS.StatusId=8)      
  AND(  
      ((NullIf(@StartDate, '') IS NULL)  OR CAST(SRS.PickDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS DATE) >= @StartDate )    
    AND ((NullIf(@EndDate, '') IS NULL)  OR CAST(SRS.PickDateTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS DATE) <=  @EndDate)    
   )    
     AND(  
      ((NullIf(@CustomerId, '') IS NULL)  OR SHP.CustomerId = @CustomerId)    
   )  
     AND(  
      ((NullIf(@FreightTypeId , '') IS NULL)  OR SFD.FreightTypeId = @FreightTypeId)    
   )  
     AND(  
      ((NullIf(@StatusId, '') IS NULL)  OR SHP.StatusId = @StatusId)    
   )  
  AND (      
            AirWayBill LIKE '%' + @SearchTerm + '%'       
       OR CustomerPO LIKE '%' + @SearchTerm + '%'      
          OR StatusName LIKE '%' + @SearchTerm + '%'                                                               
                       OR CustomerName LIKE '%' + @SearchTerm + '%'      
                        OR Driver LIKE '%' + @SearchTerm + '%'      
                        OR Equipment LIKE '%' + @SearchTerm + '%'      
                        OR PickupDate LIKE '%' + @SearchTerm + '%'      
                        OR DeliveryDate LIKE '%' + @SearchTerm + '%'      
                        OR PickupLocation LIKE '%' + @SearchTerm + '%'      
                        OR DeliveryLocation LIKE '%' + @SearchTerm + '%'      
                        ) 
						group by (SHP.ShipmentId),SHP.AirWayBill,SHP.CustomerPO, SHPS.StatusName,CR.CustomerName,DRV.Driver,EQP.Equipment,PD.PickupDate,PD.DeliveryDate,PDL.PickupLocation,PDL.DeliveryLocation,QT.Quantity
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
 THEN StatusName      
                      END ASC,      
                      CASE      
                          WHEN(@SortColumn = 'StatusName'      
                               AND @SortOrder = 'desc')      
                          THEN StatusName      
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
                          THEN SHP.AirWayBill      
                      END ASC,      
                      CASE      
                          WHEN(@SortColumn = 'AirWayBill'      
                               AND @SortOrder = 'desc')      
                          THEN SHP.AirWayBill      
                      END DESC,      
                     CASE      
                          WHEN(@SortColumn = 'CustomerPO'      
             AND @SortOrder = 'asc')      
                          THEN SHP.AirWayBill      
                      END ASC,      
                      CASE      
                          WHEN(@SortColumn = 'CustomerPO'      
                               AND @SortOrder = 'desc')      
                          THEN SHP.CustomerPO      
                      END DESC,      
            CASE      
                          WHEN(@SortColumn = 'Driver'      
                               AND @SortOrder = 'asc')      
           THEN DRV.Driver      
                      END ASC,      
                      CASE      
                          WHEN(@SortColumn = 'Driver'      
                               AND @SortOrder = 'desc')      
                          THEN  DRV.Driver      
                      END DESC,      
      
         CASE      
                          WHEN(@SortColumn = 'Equipment'      
                               AND @SortOrder = 'asc')      
                          THEN EQP.Equipment      
                      END ASC,      
                      CASE      
                          WHEN(@SortColumn = 'Equipment'      
                               AND @SortOrder = 'desc')      
                          THEN  EQP.Equipment      
                      END DESC,      
        CASE      
                          WHEN(@SortColumn = 'PickupDate'      
                               AND @SortOrder = 'asc')      
                          THEN PD.PickupDate      
                      END ASC,      
                      CASE      
                          WHEN(@SortColumn = 'PickupDate'      
                               AND @SortOrder = 'desc')      
                          THEN  PD.PickupDate      
                      END DESC,      
        CASE      
                          WHEN(@SortColumn = 'DeliveryDate'      
                               AND @SortOrder = 'asc')      
                          THEN PD.DeliveryDate      
                      END ASC,      
                      CASE      
                          WHEN(@SortColumn = 'DeliveryDate'      
                               AND @SortOrder = 'desc')      
                          THEN  PD.DeliveryDate      
                      END DESC,      
            CASE      
                          WHEN(@SortColumn = 'PickupLocation'      
                               AND @SortOrder = 'asc')      
                          THEN PDL.PickupLocation      
                      END ASC,      
                      CASE      
                          WHEN(@SortColumn = 'PickupLocation'      
                               AND @SortOrder = 'desc')      
                          THEN  PDL.PickupLocation      
                      END DESC,      
             CASE      
                          WHEN(@SortColumn = 'DeliveryLocation'      
                               AND @SortOrder = 'asc')      
                          THEN PDL.DeliveryLocation      
                      END ASC,      
                      CASE      
                          WHEN(@SortColumn = 'DeliveryLocation'      
                               AND @SortOrder = 'desc')      
                          THEN PDL.DeliveryLocation      
                      END DESC      
         OFFSET @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY;      
        -- IF EXISTS      
        --(      
        --    SELECT TOP  1 SD.ShipmentId      
        --    FROM #SHIPMENTDATA SD      
        --)      
        --    BEGIN      
        --        SELECT @TotalCount =      
        --        (      
        --            SELECT TOP 1 SHP.TotalCount      
        --            FROM #SHIPMENTDATA SHP      
        --        );--COUNT( FUM.FumigationId) from #TEMPDATA  FUM                    
        --END;      
        --    ELSE      
        --    BEGIN      
        --        SELECT @TotalCount = 0;      
        --END;      
        --SELECT *      
        --FROM #SHIPMENTDATA;      
        --DROP TABLE #SHIPMENTDATA;      
      
END;