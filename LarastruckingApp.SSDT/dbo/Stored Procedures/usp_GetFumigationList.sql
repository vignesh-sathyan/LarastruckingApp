CREATE PROC [dbo].[usp_GetFumigationList]                  
(@SearchTerm VARCHAR(50),                   
 @SortColumn VARCHAR(50),                   
 @SortOrder  VARCHAR(50),                   
 @PageNumber INT,                   
 @PageSize   INT                   
 --@TotalCount INT OUT                  
)                  
AS                  
    BEGIN                        
        --  SET NOCOUNT ON;                         
        -- calculate the starting and ending of records                                            
        SET @SortColumn = LOWER(ISNULL(@SortColumn, ''));                  
        SET @SortOrder = LOWER(ISNULL(@SortOrder, ''));                  
        SET @SearchTerm = ISNULL(@SearchTerm, '');                
--  WITH CTE_Driver(ShipmentId,                                 
--                              Driver)                                
--             AS (SELECT Distinct (SHP.ShipmentId),STRING_AGG (D.FirstName +' '+D.LastName, ',') AS DriverName                       
--FROM tblShipment SHP With(NOLOCK)                      
--left join tblShipmentEquipmentNdriver SED on SED.ShipmentId=SHP.ShipmentId                      
--left join tblDriver D on SED.DriverId=D.DriverId                      
--WHERE SHP.IsDeleted=0                      
--GROUP BY SHP.ShipmentId),                 
    --SELECT * FROM tblFumigationTypes                
    --select Distinct(fum.FumigationId), STRING_AGG(FT.FumigationName,',') AS FumigationTypes                
    --from tblFumigation fum                
    --left join                 
                    
    --tblFumigationRouts FRST on fum.FumigationId= frst.FumigationId                
    --LEFT JOIN tblFumigationTypes FT ON FRST.FumigationTypeId=FT.FumigationTypeId                
    --WHERE FRST.IsDeleted=0                
    --GROUP BY fum.FumigationId,FT.FumigationName                
                
                
        WITH     
      
  CTE_FumigationType(FumigationIds,                   
                                FumigationTypes)                  
             AS (SELECT DISTINCT                  
                        (FumigationId),                   
                        STUFF(                  
                 (                  
                     SELECT ',' +                  
                     (                  
                         SELECT FumigationName                  
                         FROM tblFumigationTypes                  
                         WHERE FumigationTypeId = FRS.FumigationTypeId                  
                     )                  
                     FROM tblFumigationRouts FRS                  
                          LEFT JOIN tblFumigationTypes FT ON FT.FumigationTypeId = FRS.FumigationTypeId                  
                     WHERE FRS.FumigationId = FRST.FumigationId                  
                           AND FRS.IsDeleted = 0                  
                     GROUP BY FRS.FumigationTypeId,                   
                              FRS.FumigationId FOR XML PATH('')                  
                 ), 1, 1, '') AS FumigationTypes                  
                 FROM tblFumigationRouts FRST),                
                     
                       
             CTE_PickUpDriver(FumigationIds,                   
                              PickUpDriver)                
                           
             AS (SELECT DISTINCT                  
                        (FumigationId),                   
                        STUFF(                  
                 (                  
                     SELECT '$' +                  
                     (                  
                         SELECT CONCAT(D.FirstName, ' ', D.LastName) DriverName                  
                         FROM tblDriver D                  
                         WHERE D.DriverId = FED.DriverId                  
                     )                  
                     FROM tblFumigationEquipmentNDriver AS FED     
         left join tblFumigationRouts FR on FED.FumigationRoutsId = FR.FumigationRoutsId    
                     WHERE FED.FumigationId = FUM.FumigationId                  
      AND FR.IsDeleted=0    
                           AND FED.IsDeleted = 0                  
                           AND IsPickUp = 1        
         ORDER BY FR.RouteNo     
                     --GROUP BY FED.DriverId,                   
                     --FED.FumigationId     
      FOR XML PATH('')                  
                 ), 1, 1, '') AS PickUpDriver                  
                 FROM tblFumigation FUM),     
                      
             CTE_DeliveryDriver(FumigationIds,                   
                                DeliveryDriver)                  
 AS (SELECT DISTINCT                  
                        (FumigationId),                   
                        STUFF(                  
                 (                  
   SELECT '$' +                  
                     (                  
                         SELECT CONCAT(D.FirstName, ' ', D.LastName) DriverName                  
                         FROM tblDriver D                  
                         WHERE D.DriverId = FED.DriverId                  
                     )                  
                     FROM tblFumigationEquipmentNDriver AS FED        
        left join tblFumigationRouts FR on FED.FumigationRoutsId = FR.FumigationRoutsId              
                     WHERE FED.FumigationId = FUM.FumigationId      
      AND FR.IsDeleted=0                
                           AND FED.IsDeleted = 0                  
                           AND IsPickUp = 0  ORDER BY FR.RouteNo                 
                     --GROUP BY FED.DriverId,                   
                            --  FED.FumigationId     
         FOR XML PATH('')                  
                 ), 1, 1, '') AS DeliveryDriver                  
                 FROM tblFumigation FUM),      
                
          
               
             CTE_PickUpEquipment(FumigationIds,                   
                                 PickUpEquipment)                  
             AS (SELECT DISTINCT                  
                        (FumigationId),                   
                        STUFF(                  
                 (                  
                     SELECT '$' +                  
                     (                  
                         SELECT EquipmentNo                  
                         FROM tblEquipmentDetail                  
                         WHERE EDID = FED.EquipmentId                  
                     )                  
                     FROM tblFumigationEquipmentNDriver AS FED     
      left join tblFumigationRouts FR on FED.FumigationRoutsId = FR.FumigationRoutsId                     
                     WHERE FED.FumigationId = FUM.FumigationId       
      AND FR.IsDeleted=0                    
                     AND FED.IsDeleted = 0                      
                     AND IsPickUp = 1                      
   -- GROUP BY FED.EquipmentId,                   
                           --   FED.FumigationId    
          FOR XML PATH('')                  
                 ), 1, 1, '') AS PickUpEquipment                  
                 FROM tblFumigation FUM),     
                    
           
             
             CTE_DeliveryEquipment(FumigationIds,                   
                                   DeliveryEquipment)                  
             AS (SELECT DISTINCT                  
                        (FumigationId),                   
                        STUFF(                  
                 (                  
                     SELECT '$' +                  
                     (                  
                         SELECT EquipmentNo                  
                         FROM tblEquipmentDetail                  
                         WHERE EDID = FED.EquipmentId                  
                     )                  
                     FROM tblFumigationEquipmentNDriver AS FED      
      left join tblFumigationRouts FR on FED.FumigationRoutsId = FR.FumigationRoutsId                   
                     WHERE FED.FumigationId = FUM.FumigationId          AND FR.IsDeleted=0                        
                           AND FED.IsDeleted = 0                  
                           AND IsPickUp = 0                  
                   --  GROUP BY FED.EquipmentId,                   
                          --    FED.FumigationId     
         FOR XML PATH('')                  
                 ), 1, 1, '') AS DeliveryEquipment                  
                 FROM tblFumigation FUM),     
         
                      
             CTE_TrailerPosition(FumigationIds,                   
                                 TrailerPosition)                  
             AS (SELECT DISTINCT                  
                        (FumigationId),                   
                        STUFF(                  
                 (                  
                     SELECT '|' + CONVERT(VARCHAR(MAX), replace(CAST(FRS.TrailerPosition AS VARCHAR), '.00', ''))                  
          FROM tblFumigationRouts FRS                  
                     WHERE FRS.FumigationId = FRST.FumigationId                  
                           AND FRS.TrailerPosition != ''                  
                           AND FRS.IsDeleted = 0 FOR XML PATH('')                  
                 ), 1, 1, '') AS TrailerPosition                  
                 FROM tblFumigationRouts FRST),                  
             CTE_AWB(FumigationIds,                   
                     AWB)                  
             AS (SELECT DISTINCT                  
                        (FumigationId),                   
         STUFF(                  
                 (                  
                     SELECT ',' + CONVERT(VARCHAR(MAX), FRS.AirWayBill)                  
                     FROM tblFumigationRouts FRS                  
                     WHERE FRS.FumigationId = FRST.FumigationId                  
                           AND FRS.AirWayBill != ''                  
                           AND FRS.IsDeleted = 0                  
                     GROUP BY FRS.AirWayBill,                   
                              FRS.FumigationId,        
         FRS.FumigationRoutsId        
                   ORDER BY FRS.FumigationRoutsId FOR XML PATH('')                  
                 ), 1, 1, '') AS AWB                  
                 FROM tblFumigationRouts FRST),        
                       
             CTE_CustomerPO(FumigationIds,                   
                            CustomerPO)                  
             AS (SELECT DISTINCT                  
                        (FumigationId),                   
                        STUFF(                  
                 (                  
                     SELECT ',' + CONVERT(VARCHAR(MAX), FRS.CustomerPO)                  
                     FROM tblFumigationRouts FRS                  
                     WHERE FRS.FumigationId = FRST.FumigationId                  
                           AND FRS.CustomerPO != ''                  
                           AND FRS.IsDeleted = 0                  
                     GROUP BY FRS.CustomerPO,                   
                              FRS.FumigationId FOR XML PATH('')                  
                 ), 1, 1, '') AS CustomerPO                  
                 FROM tblFumigationRouts FRST),                  
             CTE_ContainerNo(FumigationIds,                   
                             ContainerNo)                  
             AS (SELECT DISTINCT                  
                        (FumigationId),                   
                        STUFF(                  
                 (                  
                     SELECT ',' + CONVERT(VARCHAR(MAX), FRS.ContainerNo)                  
                     FROM tblFumigationRouts FRS                  
                     WHERE FRS.FumigationId = FRST.FumigationId                  
                           AND FRS.ContainerNo != ''                  
                           AND FRS.IsDeleted = 0                  
                     GROUP BY FRS.ContainerNo,                   
                              FRS.FumigationId FOR XML PATH('')                  
                 ), 1, 1, '') AS ContainerNo                  
                 FROM tblFumigationRouts FRST),                  
             CTE_TotalBoxNPallentCount(FumigationIds,                   
                                       BoxCount,                   
                                  PalletCount)                  
             AS (SELECT DISTINCT                  
                        (FumigationId),                   
                        SUM(FR.BoxCount) AS BoxCount,                   
                        SUM(FR.PalletCount) AS PalletCount                  
                 FROM tblFumigationRouts FR                  
                 GROUP BY FR.FumigationId),           
               
         CTE_VendorNconsignee(FumigationIds,                   
                            VendorNconsignee)                  
             AS (SELECT DISTINCT                  
                        (FumigationId),                   
                        STUFF(                  
                 (                  
 SELECT '|' + CONVERT(VARCHAR(MAX), FRS.VendorNConsignee)                  
                     FROM tblFumigationRouts FRS                  
                     WHERE FRS.FumigationId = FRST.FumigationId                  
                          AND FRS.VendorNConsignee is not null          
                          AND FRS.IsDeleted = 0         
        ORDER BY FRS.FumigationRoutsId                 
                    FOR XML PATH('')                  
      ), 1, 1, '') AS VendorNconsignee                  
                 FROM tblFumigationRouts FRST)          
               
               
                      
             SELECT FUM.FumigationId,                   
                    FUM.ShipmentRefNo,                   
                    ISNULL(CNV.VendorNconsignee, '') VendorNconsignee,                   
                    CR.CustomerName,                   
                    SS.StatusAbbreviation as StatusName,                   
                    ISNULL(FT.FumigationTypes, '') FumigationTypes,                   
                    ISNULL(TP.TrailerPosition, '') TrailerPosition,                   
                    ISNULL(                  
             (                  
                 SELECT PickUpLocation                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '') AS PickUpLocation,                   
             (                  
                 SELECT ReleaseDate                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ) AS ReleaseDate,                   
                    ISNULL(                  
             (                  
                 SELECT FumigationSite                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '') AS FumigationSite,                   
                    ISNULL(                  
             (                  
                 SELECT DeliveryLocation                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '') AS DeliveryLocation,                   
                    ISNULL(                  
             (                  
                 SELECT PickUpArrival                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), NULL) AS PickUpArrival,                   
                    ISNULL(      
             (                  
                 SELECT FumigationArrival                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), NULL) AS FumigationArrival,                   
                    ISNULL(                  
             (                  
                 SELECT DeliveryArrival                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), NULL) AS DeliveryArrival,                   
                    ISNULL(                  
  (                  
                 SELECT Temperature                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '') AS Temperature,                   
                    ISNULL(                  
             (                  
                 SELECT AWB_CP_CN                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '') AS AWB_CP_CN,                   
                    ISNULL(TBC.BoxCount, 0) AS BoxCount,                   
                    ISNULL(TBC.PalletCount, 0) AS PalletCount,                   
                    ISNULL(PD.PickUpDriver, '') AS PickUpDriver,                   
                  ISNULL(PE.PickUpEquipment, '') AS PickUpEquipment,                   
                    ISNULL(DD.DeliveryDriver, '') AS DeliveryDriver,                   
                    ISNULL(DE.DeliveryEquipment, '') AS DeliveryEquipment,                   
                    AWB.AWB,                   
                    PO.CustomerPO,                   
                    CN.ContainerNo,                   
                    [TotalCount] = COUNT(*) OVER()                  
            -- INTO #FUMIGATIONDATA                  
             FROM tblFumigation FUM                  
                  INNER JOIN [dbo].[tblCustomerRegistration] CR ON FUM.CustomerId = CR.CustomerId                  
                  Left JOIN [dbo].[tblShipmentStatus] SS ON FUM.StatusId = SS.StatusId                  
                  LEFT JOIN CTE_FumigationType FT ON FUM.FumigationId = FT.FumigationIds                  
                  LEFT JOIN CTE_TrailerPosition TP ON FUM.FumigationId = TP.FumigationIds                  
                  LEFT JOIN CTE_TotalBoxNPallentCount TBC ON FUM.FumigationId = TBC.FumigationIds                  
                  LEFT JOIN CTE_PickUpDriver PD ON FUM.FumigationId = PD.FumigationIds                  
                  LEFT JOIN CTE_PickUpEquipment PE ON FUM.FumigationId = PE.FumigationIds                  
                  LEFT JOIN CTE_DeliveryEquipment DE ON FUM.FumigationId = DE.FumigationIds                  
                  LEFT JOIN CTE_DeliveryDriver DD ON FUM.FumigationId = DD.FumigationIds                  
                  LEFT JOIN CTE_AWB AWB ON FUM.FumigationId = AWB.FumigationIds                  
                  LEFT JOIN CTE_CustomerPO PO ON FUM.FumigationId = PO.FumigationIds                  
                  LEFT JOIN CTE_ContainerNo CN ON FUM.FumigationId = CN.FumigationIds           
       LEFT JOIN CTE_VendorNconsignee CNV ON FUM.FumigationId = CNV.FumigationIds           
                
             WHERE FUM.IsDeleted = 0  AND SS.StatusId=1             
                   AND (CustomerName LIKE '%' + @SearchTerm + '%'                  
                        OR StatusName LIKE '%' + @SearchTerm + '%'                  
                        OR ShipmentRefNo LIKE '%' + @SearchTerm + '%'                  
                        OR AWB LIKE '%' + @SearchTerm + '%'                  
                        OR CustomerPO LIKE '%' + @SearchTerm + '%'                  
                        OR ContainerNo LIKE '%' + @SearchTerm + '%'                  
                        OR BoxCount LIKE '%' + @SearchTerm + '%'                  
                        OR PalletCount LIKE '%' + @SearchTerm + '%'                  
                      OR PickUpDriver LIKE '%' + @SearchTerm + '%'                  
                        OR PickUpEquipment LIKE '%' + @SearchTerm + '%'                  
                        OR DeliveryDriver LIKE '%' + @SearchTerm + '%'                  
                        OR DeliveryEquipment LIKE '%' + @SearchTerm + '%'                  
                        OR CNV.VendorNconsignee LIKE '%' + @SearchTerm + '%'                  
                        OR FumigationTypes LIKE '%' + @SearchTerm + '%'                  
                        OR TrailerPosition LIKE '%' + @SearchTerm + '%')                  
             ORDER BY CASE                  
      WHEN(@SortColumn = 'FumigationId'                  
                               AND @SortOrder = 'asc')                  
                          THEN FumigationId                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'FumigationId'                  
                               AND @SortOrder = 'desc')                  
                          THEN FumigationId                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'StatusName'                  
                               AND @SortOrder = 'asc')                  
                          THEN StatusName                  
                      END ASC,                            CASE                  
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
                          WHEN(@SortColumn = 'BoxCount'                  
                               AND @SortOrder = 'asc')                  
                          THEN BoxCount                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'BoxCount'                  
                               AND @SortOrder = 'desc')                  
                          THEN BoxCount                  
            END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'PalletCount'                  
                               AND @SortOrder = 'asc')                  
                          THEN PalletCount                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'PalletCount'                                                 AND @SortOrder = 'desc')                  
                          THEN PalletCount                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'PickUpDriver'                  
                  AND @SortOrder = 'asc')                  
                          THEN PickUpDriver                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'PickUpDriver'                  
                               AND @SortOrder = 'desc')                  
                          THEN PickUpDriver                  
                      END DESC,                  
                      CASE                
                          WHEN(@SortColumn = 'PickUpEquipment'                  
                               AND @SortOrder = 'asc')                  
                          THEN PickUpEquipment                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'PickUpEquipment'                  
                               AND @SortOrder = 'desc')                  
                          THEN PickUpEquipment                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'DeliveryDriver'                  
                      AND @SortOrder = 'asc')                  
                          THEN DeliveryDriver                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'DeliveryDriver'                  
                               AND @SortOrder = 'desc')                  
                          THEN DeliveryDriver                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'DeliveryEquipment'                  
                               AND @SortOrder = 'asc')                  
                          THEN DeliveryEquipment                  
           END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'DeliveryEquipment'                  
                   AND @SortOrder = 'desc')                  
                          THEN DeliveryEquipment                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'AWB'                  
                               AND @SortOrder = 'asc')                  
                          THEN AWB                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'AWB'                  
                               AND @SortOrder = 'desc')                  
                          THEN AWB                  
                      END DESC,                  
           CASE                  
                          WHEN(@SortColumn = 'CustomerPO'                  
                               AND @SortOrder = 'asc')                  
                          THEN CustomerPO                  
     END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'CustomerPO'                  
                               AND @SortOrder = 'desc')                  
                          THEN CustomerPO                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'ContainerNo'                  
                               AND @SortOrder = 'asc')                  
                          THEN ContainerNo                  
              END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'ContainerNo'                  
                               AND @SortOrder = 'desc')                  
                          THEN ContainerNo                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'VendorNconsignee'                  
                               AND @SortOrder = 'asc')                  
                          THEN CNV.VendorNconsignee                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'VendorNconsignee'                  
                               AND @SortOrder = 'desc')                  
                          THEN CNV.VendorNconsignee                  
    END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'Temperature'                  
                               AND @SortOrder = 'asc')                  
                          THEN ShipmentRefNo                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'Temperature'                  
                               AND @SortOrder = 'desc')                  
                          THEN ShipmentRefNo                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'FumigationTypes'                  
                               AND @SortOrder = 'asc')                  
                          THEN FumigationTypes                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'FumigationTypes'                  
                               AND @SortOrder = 'desc')                  
                          THEN FumigationTypes                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'TrailerPosition'                  
                               AND @SortOrder = 'asc')                  
                          THEN TrailerPosition                  
                      END ASC,                  
                      CASE                  
                         WHEN(@SortColumn = 'TrailerPosition'                  
                               AND @SortOrder = 'desc')                  
                          THEN TrailerPosition                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'PickUpLocation'                  
                               AND @SortOrder = 'asc')                  
                          THEN ISNULL(                  
             (                  
                 SELECT PickUpLocation                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '')                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'PickUpLocation'                  
                               AND @SortOrder = 'desc')                  
                          THEN ISNULL(                  
             (                  
            SELECT PickUpLocation                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '')                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'ReleaseDate'                  
                               AND @SortOrder = 'asc')       
                          THEN ShipmentRefNo                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'ReleaseDate'                  
                               AND @SortOrder = 'desc')                  
                          THEN ShipmentRefNo                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'FumigationSite'                  
                               AND @SortOrder = 'asc')                  
                          THEN ISNULL(                  
             (                  
                 SELECT FumigationSite                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '')                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'FumigationSite'                  
                      AND @SortOrder = 'desc')                  
                          THEN ISNULL(                  
             (                  
                 SELECT FumigationSite                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '')                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'DeliveryLocation'                  
                               AND @SortOrder = 'asc')                  
                          THEN ISNULL(                  
             (                  
                 SELECT DeliveryLocation                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '')                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'DeliveryLocation'                  
                               AND @SortOrder = 'desc')                  
                          THEN ISNULL(                  
             (                  
                 SELECT DeliveryLocation                  
                 FROM [dbo].[GetFumigationLocation](FUM.FumigationId)                  
             ), '')                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'PickUpArrival'                  
                               AND @SortOrder = 'asc')                  
                          THEN ShipmentRefNo                  
                      END ASC,                  
                      CASE                  
                        WHEN(@SortColumn = 'PickUpArrival'                  
                               AND @SortOrder = 'desc')                  
                          THEN ShipmentRefNo                  
                      END DESC,                  
           CASE                  
  WHEN(@SortColumn = 'FumigationArrival'                  
                               AND @SortOrder = 'asc')                  
                          THEN ShipmentRefNo                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'FumigationArrival'                  
                               AND @SortOrder = 'desc')                  
                          THEN ShipmentRefNo                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'DeliveryArrival'                  
                               AND @SortOrder = 'asc')                  
                          THEN ShipmentRefNo                  
                      END ASC,                  
                      CASE         
                          WHEN(@SortColumn = 'DeliveryArrival'                  
                               AND @SortOrder = 'desc')                  
                          THEN ShipmentRefNo                  
                      END DESC,                  
                      CASE                  
                          WHEN(@SortColumn = 'AWB_CP_CN'                  
                  AND @SortOrder = 'asc')                  
                          THEN ShipmentRefNo                  
                      END ASC,                  
                      CASE                  
                          WHEN(@SortColumn = 'AWB_CP_CN'                  
                               AND @SortOrder = 'desc')                  
                          THEN ShipmentRefNo                  
                      END DESC                  
             OFFSET @PageNumber ROWS FETCH NEXT case when @PageSize>0 then @PageSize else 999999 end ROWS ONLY;                  
        --IF EXISTS                  
        --(                  
        --    SELECT 1                  
        --    FROM #FUMIGATIONDATA                  
        --)                  
        --    BEGIN                  
        --        SELECT @TotalCount =                  
        --        (                  
        --            SELECT TOP 1 FUM.TotalCount                  
        --            FROM #FUMIGATIONDATA FUM                  
        --        );--COUNT( FUM.FumigationId) from #TEMPDATA  FUM                                
        --END;                  
        --    ELSE                  
        --    BEGIN                  
        --        SELECT @TotalCount = 0;                  
        --END;                  
        --SELECT *                  
        --FROM #FUMIGATIONDATA;                  
        --DROP TABLE #FUMIGATIONDATA;                  
 END;