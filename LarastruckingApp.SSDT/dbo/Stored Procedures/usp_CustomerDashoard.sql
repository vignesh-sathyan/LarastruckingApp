CREATE PROC [dbo].[usp_CustomerDashoard]  
(@UserId     INT,   
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
        WITH cte_getStatus  
             AS (SELECT SS.StatusName,   
                        SH.StatusId,   
                        SH.ShipmentId  
                 FROM tblShipmentStatusHistory SH  
                      INNER JOIN  
                 (  
                     SELECT MAX(CreatedOn) AS MaxDate,   
                            ShipmentId  
                     FROM tblShipmentStatusHistory  
                     GROUP BY ShipmentId  
                 ) ssh ON SH.ShipmentId = ssh.ShipmentId  
                          AND SH.CreatedOn = ssh.MaxDate  
                      INNER JOIN tblShipmentStatus SS ON SH.StatusId = SS.StatusId),  
             cte_getPickDate(ShippingId,   
                             PickDateTime)  
             AS (SELECT DISTINCT  
                        (TSRS.ShippingId),   
                        TSRS.MINDate  
                 FROM tblShipmentRoutesStop SRS  
                      INNER JOIN  
                 (  
                     SELECT MIN(PickDateTime) AS MINDate,   
                            ShippingId  
                     FROM tblShipmentRoutesStop ISRS  
                     WHERE IsDeleted = 0  
                     GROUP BY ShippingId  
                 ) AS TSRS ON SRS.ShippingId = TSRS.ShippingId  
                              AND SRS.PickDateTime = TSRS.MINDate),  
             cte_getDeliveryDate(ShippingId,   
                                 DeliveryDateTime)  
             AS (SELECT DISTINCT  
                        (TRSD.ShippingId),   
                        SRS.DeliveryDateTime  
                 FROM tblShipmentRoutesStop SRS  
                      INNER JOIN  
                 (  
                     SELECT MAX(DeliveryDateTime) AS MaxDate,   
                            ShippingId  
                     FROM tblShipmentRoutesStop ISRS  
                     WHERE IsDeleted = 0  
                     GROUP BY ShippingId  
                 ) AS TRSD ON SRS.ShippingId = TRSD.ShippingId  
                              AND SRS.DeliveryDateTime = TRSD.MaxDate),  
             CTE_EqipmentCommaSeperated(ShipmentId,   
                                        CostumerEquipment)  
             AS (SELECT DISTINCT  
                        (ShipmentId),   
                        STUFF(  
                 (  
                     SELECT ', ' + TE.EquipmentNo  
                     FROM tblShipmentEquipmentNdriver tblSED  
                          INNER JOIN [dbo].[tblEquipmentDetail] TE ON TE.EDID = tblSED.EquipmentId  
                     WHERE tblSED.ShipmentId = TS.ShipmentId FOR XML PATH('')  
                 ), 1, 1, '') AS CostumerEquipment  
                 FROM tblShipment TS),  
             cte_getDriver(ShipmentId,   
                           DriverName)  
             AS (SELECT ShipmentId,   
                        STUFF(  
                 (  
                     SELECT DISTINCT   
                            ', ' + CONVERT(VARCHAR(MAX), concat(TD.FirstName, ' ', TD.LastName))  
                     FROM tblShipmentEquipmentNdriver TSED  
                          INNER JOIN tblDriver TD ON TSED.DriverId = TD.DriverID  
                     WHERE TSED.ShipmentId = TS.ShipmentId FOR XML PATH('')  
                 ), 1, 1, '') AS DriverName  
                 FROM tblShipment TS)  
             SELECT DISTINCT   
                    TS.ShipmentId,       
                    -- TSRS.ShippingRoutesId,       
                    (Case when TS.AirWayBill is not null then TS.AirWayBill ELSE case when TS.CustomerPO IS NOT NULL THEN TS.CustomerPO else TS.OrderNo end end) as AirWayBill,   
                    CTEP.PickDateTime,   
                    CTED.DeliveryDateTime,   
                    CTEE.CostumerEquipment,                                                                                
                    -- TS.ShipmentRefNo,                 
                    CTES.StatusName AS StatusName,   
                    CTEDR.DriverName,   
                    TS.CreatedDate AS CreatedOn  
             INTO #TEMPDATA  
             FROM [dbo].[tblShipment] TS      
                  -- INNER JOIN [dbo].[tblShipmentRoutesStop] TSRS ON TSRS.ShippingId = TS.ShipmentId      
                  INNER JOIN [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerId = TS.CustomerID  
                  INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId      
                  -- LEFT OUTER JOIN [dbo].tblShipmentEquipmentNdriver TSEND ON TSEND.ShipmentId = TS.ShipmentId      
                  INNER JOIN cte_getStatus CTES ON TS.ShipmentId = CTES.ShipmentId  
                  INNER JOIN cte_getPickDate CTEP ON TS.ShipmentId = CTEP.ShippingId  
                  INNER JOIN cte_getDeliveryDate CTED ON TS.ShipmentId = CTED.ShippingId  
                  INNER JOIN cte_getDriver CTEDR ON TS.ShipmentId = CTEDR.ShipmentId  
                  INNER JOIN CTE_EqipmentCommaSeperated CTEE ON CTEE.ShipmentId = TS.ShipmentId  
             WHERE TS.IsDeleted = 0  
                   AND TCR.UserId = @UserId;            
        --  AND TSS.StatusId != 7  AND TSS.StatusId != 8 AND TSS.StatusId != 9                                    
        ---------------------------------------------------------------------------------------------------------------------------------------------------                                  
  
        WITH cte_getStatus  
             AS (SELECT SS.StatusName,   
                        SH.StatusId,   
                        SH.FumigationId  
                 FROM tblFumigationStatusHistory SH  
                      INNER JOIN  
                 (  
                     SELECT MAX(CreatedOn) AS MaxDate,   
                            FumigationId  
                     FROM tblFumigationStatusHistory  
                     GROUP BY FumigationId  
                 ) ssh ON SH.FumigationId = ssh.FumigationId  
                          AND SH.CreatedOn = ssh.MaxDate  
                      INNER JOIN tblShipmentStatus SS ON SH.StatusId = SS.StatusId),  
  
             --cte_getPickDate(FumigationRoutsId,       
             --                PickDateTime)      
             --AS (SELECT DISTINCT      
             --           (TSRS.FumigationRoutsId),       
             --           TSRS.MINDate      
             --    FROM tblFumigationRouts SRS      
             --         INNER JOIN      
             --    (      
             --        SELECT MIN(PickUpArrival) AS MINDate,       
             --               FumigationRoutsId      
             --        FROM tblFumigationRouts ISRS      
             --        WHERE IsDeleted = 0      
             --        GROUP BY FumigationRoutsId      
             --    ) AS TSRS ON SRS.FumigationRoutsId = TSRS.FumigationRoutsId      
             --                 AND SRS.PickUpArrival = TSRS.MINDate),      
  
             cte_getPickDate(FumigationIds,   
                             PickDateTime)  
             AS (SELECT DISTINCT  
                        (TSRS.FumigationId),   
                        TSRS.MINDate  
                 FROM tblFumigationRouts SRS  
                      INNER JOIN  
                 (  
                     SELECT MIN(PickUpArrival) AS MINDate,   
                            FumigationId  
                     FROM tblFumigationRouts ISRS  
                     WHERE IsDeleted = 0  
                     GROUP BY FumigationId  
                 ) AS TSRS ON SRS.FumigationId = TSRS.FumigationId  
                              AND SRS.PickUpArrival = TSRS.MINDate),  
             cte_getDeliveryDate(FumigationIds,   
                                 DeliveryDateTime)  
             AS (SELECT DISTINCT  
                  (TRSD.FumigationId),   
                        SRS.DeliveryArrival  
                 FROM tblFumigationRouts SRS  
                      INNER JOIN  
                 (  
                     SELECT MAX(DeliveryArrival) AS MaxDate,   
                            FumigationId  
                     FROM tblFumigationRouts ISRS  
                     WHERE IsDeleted = 0  
                     GROUP BY FumigationId  
                 ) AS TRSD ON SRS.FumigationId = TRSD.FumigationId  
                              AND SRS.DeliveryArrival = TRSD.MaxDate),  
  
             --        CTE_PickUpEquipment(FumigationIds,       
             --                            PickUpEquipment)      
             --        AS (SELECT DISTINCT       
             --                   FumigationId,       
             --                   STUFF(      
             --            (      
             --                SELECT ',' + ted.EquipmentNo      
             --                FROM tblEquipmentDetail ted      
             --                     JOIN tblFumigationEquipmentNDriver FEM ON ted.EDID = FEM.EquipmentId      
             --                           AND FEM.FumigationId = FUM.FumigationId  AND FEM.IsDeleted=0                               
             --                -- JOIN tblDriver tu ON  tu.DriverID = FEM.DriverId                                
             --                -- WHERE tu.UserId=@UserId                                
             --                GROUP BY ted.EquipmentNo FOR XML PATH('')      
             --            ), 1, 1, '') AS PickUpEquipment      
             --            FROM tblFumigationEquipmentNDriver FUM      
             --WHERE FUM.IsDeleted=0),      
  
             CTE_PickUpEquipment(FumigationIds,   
                                 PickUpEquipment)  
             AS (SELECT DISTINCT  
                        (FumigationId),   
                        STUFF(  
                 (  
                     SELECT ', ' + TE.EquipmentNo  
                     FROM tblFumigationEquipmentNDriver tblSED  
                          INNER JOIN [dbo].[tblEquipmentDetail] TE ON TE.EDID = tblSED.EquipmentId  
                     WHERE tblSED.FumigationId = TS.FumigationId FOR XML PATH('')  
                 ), 1, 1, '') AS PickUpEquipment  
                 FROM tblFumigation TS),  
             cte_getDriver(FumigationId,   
                           DriverName)  
             AS (SELECT FumigationId,   
                        STUFF(  
                 (  
                     SELECT DISTINCT   
                            ', ' + CONVERT(VARCHAR(MAX), concat(TD.FirstName, ' ', TD.LastName))  
                     FROM tblFumigationEquipmentNDriver TSED  
                          INNER JOIN tblDriver TD ON TSED.DriverId = TD.DriverID  
                     WHERE TSED.FumigationId = TS.FumigationId FOR XML PATH('')  
                 ), 1, 1, '') AS DriverName  
                 FROM tblFumigation TS),  
             CTE_AWB(FumigationIds,   
                     AWB)  
             AS (SELECT FUMR.FumigationId,   
                        STUFF(  
                 (  
                     SELECT ',' + (Case when FR.AirWayBill is not null then FR.AirWayBill ELSE case when FR.CustomerPO IS NOT NULL THEN FR.CustomerPO else FR.ContainerNo end end)  
                     FROM tblFumigationRouts FR  
                     WHERE FR.FumigationId = FUMR.FumigationId  
                           AND FR.IsDeleted = 0 FOR XML PATH('')  
                 ), 1, 1, '') AS AWB  
                 FROM tblFumigationRouts FUMR  
                 WHERE FUMR.IsDeleted = 0)  
             SELECT DISTINCT   
                    TS.FumigationId,       
                    -- TFS.FumigationRoutsId,       
                    ISNULL(AWB.AWB, '') AS AirWayBill,   
                    CTEP.PickDateTime,   
                    CTED.DeliveryDateTime,                                                                              
                    -- TS.ShipmentRefNo,                    
                    CTES.StatusName AS StatusName,   
                    CTEDR.DriverName,   
                    ISNULL(PE.PickUpEquipment, '') AS CostumerEquipment,   
                    TS.CreatedOn  
             INTO #FUMIGATIONTEMPDATA  
             FROM [dbo].[tblFumigation] TS      
                  -- JOIN tblFumigationRouts TFS ON TFS.FumigationId = TS.FumigationId      
                  LEFT JOIN CTE_AWB AWB ON TS.FumigationId = AWB.FumigationIds  
                  INNER JOIN CTE_PickUpEquipment PE ON TS.FumigationId = PE.FumigationIds  
                  INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId  
                  INNER JOIN [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerId = TS.CustomerID  
                  INNER JOIN cte_getStatus CTES ON TS.FumigationId = CTES.FumigationId  
                  INNER JOIN cte_getPickDate CTEP ON TS.FumigationId = CTEP.FumigationIds  
                  INNER JOIN cte_getDeliveryDate CTED ON TS.FumigationId = CTED.FumigationIds  
                  INNER JOIN cte_getDriver CTEDR ON TS.FumigationId = CTEDR.FumigationId  
             WHERE TS.IsDeleted = 0  
                   AND TCR.UserId = @UserId;           
        -- AND TSS.StatusId != 7  AND TSS.StatusId != 8                                             
  
        SELECT *,   
               [TotalCount] = COUNT(*) OVER()  
        INTO #tempAll  
        FROM  
        (  
            SELECT FumigationId AS Id,       
                   --FumigationRoutsId AS RouteId,       
                   StatusName,   
                   PickDateTime,   
                   DeliveryDateTime,   
                   AirWayBill,   
                   DriverName,   
                   CreatedOn,   
                   CostumerEquipment,   
                   'Fumigation' AS Types  
            FROM #FUMIGATIONTEMPDATA  
            UNION ALL  
            SELECT ShipmentId AS Id,       
                   --  ShippingRoutesId AS RouteId,       
                   StatusName,   
                   PickDateTime,   
                   DeliveryDateTime,   
                   AirWayBill,   
                   DriverName,   
                   CreatedOn,   
                   CostumerEquipment,   
                   'Shipment' AS Types  
            FROM #TEMPDATA  
        ) a;  
        DROP TABLE #TEMPDATA;  
        DROP TABLE #FUMIGATIONTEMPDATA;  
        SELECT *  
        FROM #tempAll  
        WHERE((ISNULL(@SearchTerm, '') = ''  
               OR StatusName LIKE '%' + @SearchTerm + '%')  
              OR (ISNULL(@SearchTerm, '') = ''  
                  OR DriverName LIKE '%' + @SearchTerm + '%')  
              OR (ISNULL(@SearchTerm, '') = ''  
                  OR AirWayBill LIKE '%' + @SearchTerm + '%')  
              OR (ISNULL(@SearchTerm, '') = ''  
                  OR PickDateTime LIKE '%' + @SearchTerm + '%')  
              OR (ISNULL(@SearchTerm, '') = ''  
                  OR CostumerEquipment LIKE '%' + @SearchTerm + '%')  
              OR (ISNULL(@SearchTerm, '') = ''  
                  OR Types LIKE '%' + @SearchTerm + '%'))                                          
        -- OR (ISNULL(@SearchTerm, '') = '' OR ShipmentRefNo LIKE '%' + @SearchTerm + '%')   )                                         
  
        ORDER BY CASE  
                     WHEN(@SortColumn = 'UserId'  
                          AND @SortOrder = 'asc')  
                     THEN CreatedOn  
                 END ASC,  
                 CASE  
                     WHEN(@SortColumn = 'UserId'  
                          AND @SortOrder = 'desc')  
                     THEN CreatedOn  
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
                     WHEN(@SortColumn = 'DriverName'  
                          AND @SortOrder = 'asc')  
                     THEN DriverName  
                 END ASC,  
                 CASE  
                     WHEN(@SortColumn = 'DriverName'  
                          AND @SortOrder = 'desc')  
                     THEN DriverName  
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
                     WHEN(@SortColumn = 'DeliveryDateTime'  
                          AND @SortOrder = 'asc')  
                     THEN DeliveryDateTime  
                 END ASC,  
                 CASE  
                     WHEN(@SortColumn = 'DeliveryDateTime'  
                          AND @SortOrder = 'desc')  
                     THEN DeliveryDateTime  
                 END DESC,  
                 CASE  
                     WHEN(@SortColumn = 'Types'  
                          AND @SortOrder = 'asc')  
                     THEN Types  
                 END ASC,  
                 CASE  
                     WHEN(@SortColumn = 'Types'  
                          AND @SortOrder = 'desc')  
                     THEN Types  
                 END DESC,  
                 CASE  
                     WHEN(@SortColumn = 'CostumerEquipment'  
                          AND @SortOrder = 'asc')  
                     THEN CostumerEquipment  
                 END ASC,  
                 CASE  
                     WHEN(@SortColumn = 'CostumerEquipment'  
                          AND @SortOrder = 'desc')  
                     THEN CostumerEquipment  
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
        DROP TABLE #tempAll;  
  
        --------------------------------------------------------------------------------------------------                                            
  
    END;