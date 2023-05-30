--Declare @TotalCount int                                
--exec [usp_DriverDashboard_1732020] 7,'','StatusName','ASC',0,15, @TotalCount out                          
--print @TotalCount                           
                      
CREATE PROCEDURE [dbo].[usp_DriverDashboard_1732020]                      
(@UserId  INT,                                                                            
 @SearchTerm VARCHAR(50),                       
 @SortColumn VARCHAR(50),                       
 @SortOrder  VARCHAR(50),                       
 @PageNumber INT,                       
 @PageSize   INT,                       
 @TotalCount INT OUT                      
)                      
AS                      
    BEGIN                      
        SET NOCOUNT ON;                      
        DECLARE @StartRow INT;                      
        DECLARE @EndRow INT;                      
                      
        -- calculate the starting and ending of records                            
        SET @SortColumn = LOWER(ISNULL(@SortColumn, ''));                      
        SET @SortOrder = LOWER(ISNULL(@SortOrder, ''));                      
               
                       
       ;WITH CTE_CommaSepartedVarietal(ShipmentId,QuantityNMethod)                      
             AS (SELECT DISTINCT                      
                        (ShipmentId),                       
                        STUFF((                      
                     SELECT  ',' + CONVERT(VARCHAR(MAX), concat(replace(cast(sum(tsfd.QuantityNweight) as varchar), '.00', '')  , ' ', (select PricingMethodExt  from tblPricingMethod where PricingMethodId=tsfd.PricingMethodId)))                
                     FROM tblShipmentFreightDetail tsfd                      
                     Left JOIN tblPricingMethod tpm ON tpm.PricingMethodId = tsfd.PricingMethodId                    
                      
                     WHERE tsfd.ShipmentId = tsfdd.ShipmentId and tsfd.QuantityNweight is not null AND tsfd.IsDeleted = 0 group by tsfd.PricingMethodId,tsfd.ShipmentId                
      FOR XML PATH('')                      
                 ), 1, 1, '') AS QuantityNMethod                      
                 FROM tblShipmentFreightDetail tsfdd),                
                     
                           
             cte_getPickDate(ShippingId,PickDateTime)                      
             AS (SELECT DISTINCT (TSRS.ShippingId),TSRS.MINDate                      
             FROM tblShipmentRoutesStop SRS                      
             INNER JOIN                      
                 (                      
                 SELECT MIN(PickDateTime) AS MINDate,                       
                     ShippingId FROM tblShipmentRoutesStop ISRS                      
                     WHERE IsDeleted = 0                      
                     GROUP BY ShippingId                      
                 ) AS TSRS ON SRS.ShippingId = TSRS.ShippingId                      
                              AND SRS.PickDateTime = TSRS.MINDate),                 
                              
             CTE_EqipmentCommaSeperated(ShipmentId,                       
                                        DriverEquipment)                      
             AS (SELECT DISTINCT                      
                        (ShipmentId),                       
                        STUFF(                      
                 (                      
                     SELECT ', ' + TE.EquipmentNo                      
                     FROM tblShipmentEquipmentNdriver tblSED                      
                          INNER JOIN [dbo].[tblEquipmentDetail] TE ON TE.EDID = tblSED.EquipmentId                      
                     WHERE tblSED.ShipmentId = TS.ShipmentId FOR XML PATH('')                      
                 ), 1, 1, '') AS DriverEquipment                      
                 FROM tblShipment TS),                     
                          
             cte_getDriver(DriverId,UserId,ShipmentId)AS (SELECT DISTINCT (TSEND.DriverId), TD.UserId,ShipmentId                      
             FROM tblShipmentEquipmentNdriver TSEND                      
             INNER JOIN tblDriver TD ON TSEND.DriverId = TD.DriverID)                    
                         
                    SELECT TS.ShipmentId,                       
                    cted.DriverId,                       
                    ISNULL(TC.PreTripCheckupId, 0) PreTripCheckupId,                       
                    CTEP.PickDateTime,                       
                    CTEE.DriverEquipment,                       
                    TS.ShipmentRefNo,                       
                    CTEC.QuantityNMethod,                       
                    TS.StatusId,                       
                    TSS.StatusName,                      
                    CASE                      
                        WHEN dbo.ufnGetPreTripStatus(TS.ShipmentId, TC.UserId) IS NULL                      
                        THEN 'PENDING'                      
                        ELSE dbo.ufnGetPreTripStatus(TS.ShipmentId, TC.UserId)                      
                    END PreTripStatus        
                         
                  INTO #TEMPDATA FROM [dbo].[tblShipment] TS                      
                  INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId                      
                  INNER JOIN cte_getDriver cted ON TS.ShipmentId = cted.ShipmentId                      
                  INNER JOIN CTE_CommaSepartedVarietal CTEC ON CTEC.ShipmentId = TS.ShipmentId                      
                  INNER JOIN cte_getPickDate CTEP ON TS.ShipmentId = CTEP.ShippingId                      
                  INNER JOIN CTE_EqipmentCommaSeperated CTEE ON CTEE.ShipmentId = TS.ShipmentId                      
                  LEFT JOIN [dbo].[tblPreTripCheckUp] TC ON TC.ShipmentId = TS.ShipmentId AND TC.UserId = cted.UserId                     
                           
                  WHERE cted.UserId =@UserId  AND TS.IsDeleted = 0                    
                  AND (TS.StatusId != 1 AND TS.StatusId != 7 AND TS.StatusId != 8)                       
                                        
     
     
     
 -----------------------------------------------------------------------------------------------    
     ;with CTE_TotalBoxNPallentCount(FumigationId, QuantityNMethod)      
       AS (SELECT DISTINCT (FumigationId), CONCAT(SUM(FR.BoxCount),', ',SUM(FR.PalletCount) ) as QuantityNMethod    
    --SUM(FR.BoxCount) AS BoxCount,SUM(FR.PalletCount) AS PalletCount      
        FROM tblFumigationRouts FR      
        GROUP BY FR.FumigationId),    
    
      CTE_PickUpEquipment(FumigationIds,PickUpEquipment) AS (SELECT DISTINCT(FumigationId), STUFF((      
         SELECT ',' +      
         (      
          SELECT EquipmentNo      
          FROM tblEquipmentDetail      
          WHERE EDID = FED.EquipmentId      
         )      
         FROM tblFumigationEquipmentNDriver AS FED      
         WHERE FED.FumigationId = FUM.FumigationId      
            AND FED.IsDeleted = 0      
            AND IsPickUp = 1      
        GROUP BY FED.EquipmentId,       
            FED.FumigationId FOR XML PATH('')      
        ), 1, 1, '') AS PickUpEquipment      
        FROM tblFumigation FUM)    
    
     select FUM.FumigationId,     
     TD.DriverId,    
     ISNULL(TC.PreTripCheckupId, 0) PreTripCheckupId,     
     TBC.QuantityNMethod,    
     FUM.ShipmentRefNo,    
     FUM.StatusId,    
     SS.StatusName,    
     ISNULL((SELECT PickUpArrival FROM [dbo].[GetFumigationLocation](FUM.FumigationId)) , NULL)AS PickDateTime,    
     ISNULL(PE.PickUpEquipment, '') AS DriverEquipment,    
     CASE                      
         WHEN dbo.ufnGetFumigationPreTripStatus(FUM.FumigationId, TC.UserId) IS NULL                      
         THEN 'PENDING'                      
         ELSE dbo.ufnGetFumigationPreTripStatus(FUM.FumigationId, TC.UserId)                      
        END PreTripStatus    
        
       INTO #FUMIGATIONTEMPDATA    
       FROM tblFumigation FUM    
      INNER JOIN [dbo].[tblFumigationEquipmentNDriver] TFEND ON  TFEND.FumigationId = FUM.FumigationId    
      INNER JOIN [dbo].[tbldriver] TD ON TD.DriverID = TFEND.DriverId   
      INNER JOIN [dbo].[tblUser] TU ON TU.UserId = TD.UserId    
      INNER JOIN [dbo].[tblShipmentStatus] SS ON FUM.StatusId = SS.StatusId     
      LEFT JOIN CTE_TotalBoxNPallentCount TBC ON FUM.FumigationId = TBC.FumigationId     
      LEFT JOIN CTE_PickUpEquipment PE ON FUM.FumigationId = PE.FumigationIds      
      LEFT JOIN [dbo].[tblPreTripCheckUp] TC ON TC.ShipmentId = FUM.FumigationId AND TC.UserId = TU.Userid      
      WHERE TD.UserId = @UserId AND FUM.IsDeleted = 0    
      AND (FUM.StatusId != 1 AND FUM.StatusId != 7 AND FUM.StatusId != 8)     
    
      select *,[TotalCount] = COUNT(*) OVER() into #tempAll from (    
      Select FumigationId as Id,QuantityNMethod,ShipmentRefNo,StatusName,PickDateTime,DriverEquipment,PreTripStatus , 'Fumigation' as Types  from #FUMIGATIONTEMPDATA    
      UNION ALL     
      SELECT ShipmentId as Id, QuantityNMethod,ShipmentRefNo,StatusName,PickDateTime,DriverEquipment,PreTripStatus , 'Shipment' as Types from #TEMPDATA)a     
          
  DROP TABLE #TEMPDATA     
       DROP TABLE #FUMIGATIONTEMPDATA   
  
  
      select * from #tempAll where    
        ( (ISNULL(@SearchTerm, '') = ''                      
                       OR StatusName LIKE '%' + @SearchTerm + '%')                      
                   OR (ISNULL(@SearchTerm, '') = ''                      
                       OR PickDateTime LIKE '%' + @SearchTerm + '%')                      
                   OR (ISNULL(@SearchTerm, '') = ''                      
                       OR QuantityNMethod LIKE '%' + @SearchTerm + '%')                      
                   OR (ISNULL(@SearchTerm, '') = ''                      
                       OR ShipmentRefNo LIKE '%' + @SearchTerm + '%')                      
                   OR (ISNULL(@SearchTerm, '') = ''                      
                       OR DriverEquipment LIKE '%' + @SearchTerm + '%') )                      
                               
                      
             ORDER BY CASE                      
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
                          WHEN(@SortColumn = 'PickDateTime'                      
                               AND @SortOrder = 'asc')                      
                          THEN PickDateTime                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'PickDateTime'                      
                               AND @SortOrder = 'desc')                      
                          THEN PickDateTime                      
                      END DESC,                      
                      CASE               
                          WHEN(@SortColumn = 'QuantityNMethod'                      
                               AND @SortOrder = 'asc')                      
                          THEN QuantityNMethod                      
                      END ASC,                   
                      CASE                      
                          WHEN(@SortColumn = 'QuantityNMethod'                      
             AND @SortOrder = 'desc')                      
                          THEN QuantityNMethod                      
                      END DESC,                      
               CASE                      
                          WHEN(@SortColumn = 'ShipmentRefNo'                      
                               AND @SortOrder = 'asc')                      
                          THEN ShipmentRefNo                      
                      END ASC,                      
                    CASE                      
                          WHEN(@SortColumn = 'ShipmentRefNo'                      
                               AND @SortOrder = 'desc')                      
                          THEN ShipmentRefNo                      
                      END DESC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DriverEquipment'                      
                               AND @SortOrder = 'asc')                      
                          THEN DriverEquipment                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DriverEquipment'                      
                               AND @SortOrder = 'desc')                      
                          THEN DriverEquipment                      
                      END DESC                     
                    
      
             OFFSET @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY;      
        
    IF EXISTS          
        (          
            SELECT 1          
            FROM #tempAll          
        )          
            BEGIN          
                SELECT @TotalCount =          
                (          
                    SELECT TOP 1 TL.TotalCount          
                    FROM #tempAll TL          
                );               
        END;          
            ELSE          
            BEGIN          
                SELECT @TotalCount = 0;          
        END;                
 DROP TABLE #tempAll       
   
    
  --------------------------------------------------------------------------------------------------    
    
           
 END;