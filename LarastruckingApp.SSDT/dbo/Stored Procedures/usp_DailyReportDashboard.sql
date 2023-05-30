--Declare @TotalCount int                                  
--exec [usp_DailyReportDashboard] 2,1,'','StatusName','asc',0,10, @TotalCount out                            
--print @TotalCount              
                      
                        
CREATE PROCEDURE [dbo].[usp_DailyReportDashboard]                        
(@UserId  INT,      
 @statusId INT,                                                                            
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
                 
       ;With cte_getStatus as                      
     (                      
     select SS.StatusName, SH.StatusId, SH.ShipmentId from tblShipmentStatusHistory SH                      
     inner join (select MAX(CreatedOn) as MaxDate,ShipmentId from tblShipmentStatusHistory group by ShipmentId) ssh on SH.ShipmentId=ssh.ShipmentId and SH.CreatedOn = ssh.MaxDate                      
     inner join tblShipmentStatus SS on SH.StatusId = SS.StatusId                      
     ),               
                
        CTE_CommaSepartedQuantity(ShipmentId,QuantityNMethod)                        
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
                       
                             
             --cte_getPickDate(ShippingId,PickDateTime)                        
             --AS (SELECT DISTINCT (TSRS.ShippingId),TSRS.MINDate                        
             --FROM tblShipmentRoutesStop SRS                        
             --INNER JOIN                        
             --    (                        
             --    SELECT MIN(PickDateTime) AS MINDate,                         
             --        ShippingId FROM tblShipmentRoutesStop ISRS                        
             --        WHERE IsDeleted = 0                        
             --        GROUP BY ShippingId                        
             --    ) AS TSRS ON SRS.ShippingId = TSRS.ShippingId                        
             --                 AND SRS.PickDateTime = TSRS.MINDate),                   
                                
             CTE_EqipmentCommaSeperated(ShipmentId,DriverEquipment)                        
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
            
                  
           
     cte_getDriver(ShipmentId,DriverName)  AS              
     ( SELECT ShipmentId,                
       STUFF(                
        (                
        SELECT ', ' + CONVERT(varchar(max),concat(TD.FirstName,' ',TD.LastName))                
         FROM tblShipmentEquipmentNdriver TSED                
         INNER JOIN tblDriver TD ON TSED.DriverId=TD.DriverID                            
         WHERE TSED.ShipmentId=TS.ShipmentId                
        FOR XML PATH('')                
         ), 1, 1, '')                 
         AS  DriverName            
          FROM tblShipment TS)       
                              
                           
       SELECT TS.ShipmentId,             
       TCR.CustomerName,           
       CONCAT(A1.Address1,', ', A1.City,', ', S1.Name )as PickUpAddress ,                                                                  
       CONCAT(A2.Address1,', ', A2.City,', ', S1.Name ) DeliveryAddress,          
       TSRS.PickDateTime as PickUpArrivalDate,                                                                  
       TSRS.DeliveryDateTime as DeliveryArrive,             
       TS.AirWayBill,           
       TS.CustomerPO,                     
                    --cted.DriverId,      
     CTEDR.DriverName,                         
                   -- CTEP.PickDateTime,                         
                    CTEE.DriverEquipment,                         
                    TS.ShipmentRefNo,                         
                    CTEC.QuantityNMethod,          
                   -- CTES.StatusName AS StatusName ,                       
                    TS.StatusId,                         
                    TSS.StatusName                        
                                           
       INTO #TEMPDATA FROM [dbo].[tblShipment] TS        
      INNER JOIN [dbo].[tblUser] TU ON TU.Userid = TS.CreatedBy              
      INNER JOIN [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerId =TS.CustomerID          
      INNER JOIN [dbo].[tblShipmentRoutesStop] TSRS ON TSRS.ShippingId = TS.ShipmentId                             
      INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId             
      INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = TSRS.PickupLocationId                                                                  
      INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                                  
      INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                                   
      INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = TSRS.DeliveryLocationId                                                         
      INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                                                                  
      INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country            
     -- INNER JOIN cte_getStatus  CTES ON TS.ShipmentId =CTES.ShipmentId                               
                  --INNER JOIN cte_getDriver cted ON TS.ShipmentId = cted.ShipmentId       
                  INNER JOIN cte_getDriver CTEDR ON TS.ShipmentId =CTEDR.ShipmentId                        
                  INNER JOIN CTE_CommaSepartedQuantity CTEC ON CTEC.ShipmentId = TS.ShipmentId                        
                 -- INNER JOIN cte_getPickDate CTEP ON TS.ShipmentId = CTEP.ShippingId            
                  INNER JOIN CTE_EqipmentCommaSeperated CTEE ON CTEE.ShipmentId = TS.ShipmentId                        
                                  
                             
                  WHERE TU.UserId =@UserId  AND TS.IsDeleted = 0                                    
                    AND TS.StatusId=@statusId       
                                          
                   AND( (ISNULL(@SearchTerm, '') = ''                        
                       OR TSS.StatusName LIKE '%' + @SearchTerm + '%')                        
                   OR (ISNULL(@SearchTerm, '') = ''                        
                       OR TSRS.PickDateTime LIKE '%' + @SearchTerm + '%')                        
                   OR (ISNULL(@SearchTerm, '') = ''                        
                       OR QuantityNMethod LIKE '%' + @SearchTerm + '%')                        
                   OR (ISNULL(@SearchTerm, '') = ''                        
                       OR ShipmentRefNo LIKE '%' + @SearchTerm + '%')                        
                   OR (ISNULL(@SearchTerm, '') = ''                        
                       OR DriverEquipment LIKE '%' + @SearchTerm + '%') )                        
                                 
                        
                   ORDER BY CASE                        
                          WHEN(@SortColumn = 'StatusName'                        
                               AND @SortOrder = 'asc')                        
                  THEN TSS.StatusName                        
       END ASC,                        
      CASE                        
         WHEN(@SortColumn = 'StatusName'                        
         AND @SortOrder = 'desc')                        
      THEN TSS.StatusName                        
        END DESC,                        
                      CASE                        
                          WHEN(@SortColumn = 'PickDateTime'                        
                               AND @SortOrder = 'asc')                        
                          THEN TSRS.PickDateTime                        
                      END ASC,                        
                      CASE                        
                          WHEN(@SortColumn = 'PickDateTime'                        
                               AND @SortOrder = 'desc')                        
                          THEN TSRS.PickDateTime                        
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
                  
    SELECT  @TotalCount=COUNT(TA.ShipmentId) from #TEMPDATA  TA                  
    SELECT * from #TEMPDATA                
              
    DROP TABLE #TEMPDATA              
    END;