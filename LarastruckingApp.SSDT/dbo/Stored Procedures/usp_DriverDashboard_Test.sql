






--exec [usp_DriverDashboard_test] 12,'','PickDateTime','desc',0,100
CREATE PROCEDURE [dbo].[usp_DriverDashboard_Test]
(@UserId     INT, 
 @SearchTerm VARCHAR(50), 
 @SortColumn VARCHAR(50), 
 @SortOrder  VARCHAR(50), 
 @PageNumber INT, 
 @PageSize   INT
)
AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @StartRow INT;
        DECLARE @EndRow INT;

        -- calculate the starting and ending of records                                                                    
        SET @SortColumn = LOWER(ISNULL(@SortColumn, ''));
        SET @SortOrder = LOWER(ISNULL(@SortOrder, ''));  
         
            
                  
        WITH CTE_TotalBoxNPallentCount(FumigationId, 
                                       QuantityNMethod)
             AS (SELECT DISTINCT
                        (FumigationId),
                        CASE
                            WHEN SUM(FR.BoxCount) > 0
                            THEN CONVERT(VARCHAR(MAX), concat(replace(CAST(SUM(FR.BoxCount) AS VARCHAR), '.00', ''), ' ' + 'Box, '))
                            ELSE ''
                        END + CASE
                                  WHEN SUM(FR.PalletCount) > 0
                                  THEN CONVERT(VARCHAR(MAX), concat(replace(CAST(SUM(FR.PalletCount) AS VARCHAR), '.00', ''), ' ' + 'Pallet'))
                                  ELSE ''
                              END AS QuantityNMethod
                 FROM tblFumigationRouts FR
                 GROUP BY FR.FumigationId),
             CTE_PickUpEquipment(FumigationIds, 
                                 PickUpEquipment)
             AS (SELECT DISTINCT 
                        FumigationId, 
                        STUFF(
                 (
                     SELECT ',' + ted.EquipmentNo
                     FROM tblEquipmentDetail ted
                          JOIN tblFumigationEquipmentNDriver FEM ON ted.EDID = FEM.EquipmentId
                                                                    AND FEM.FumigationId = FUM.FumigationId
                          JOIN tblDriver tu ON tu.DriverID = FEM.DriverId
                     WHERE tu.UserId = @UserId
                     GROUP BY ted.EquipmentNo FOR XML PATH('')
                 ), 1, 1, '') AS PickUpEquipment
                 FROM tblFumigationEquipmentNDriver FUM),
             CTE_AWB(FumigationIds, 
                     DriverId, 
                     AWB)
             AS (SELECT FUMR.FumigationId, 
                        FED.DriverId, 
                        STUFF(
                 (
                     SELECT ',' + FR.AirWayBill
                     FROM tblFumigationRouts FR
                          LEFT JOIN tblFumigationEquipmentNDriver FED ON FR.FumigationRoutsId = FED.FumigationRoutsId
                          LEFT JOIN tblDriver DRV ON DRV.DriverID = FED.DriverId
                     WHERE FR.FumigationId = FUMR.FumigationId
                           AND FR.IsDeleted = 0
                           AND DRV.UserId = @UserId FOR XML PATH('')
                 ), 1, 1, '') AS AWB
                 FROM tblFumigationRouts FUMR
                      LEFT JOIN tblFumigationEquipmentNDriver FED ON FUMR.FumigationRoutsId = FED.FumigationRoutsId
                 WHERE FUMR.IsDeleted = 0)
             SELECT DISTINCT 
                    FUM.FumigationId AS Id, 
                    TBC.QuantityNMethod, 
                    (AWB.AWB) AS AirWayBill, 
             (
                 SELECT CustomerPO
                 FROM tblFumigationRouts
                 WHERE FumigationRoutsId = TFEND.FumigationRoutsId
             ) AS CustomerPO, 
                    '' AS OrderNo, 
                    '' AS DriverInstruction, 
             (
                 SELECT PickUpAddress
                 FROM [dbo].GetDriverFumigationPickupLocation(FUM.FumigationId, TD.DriverID)
             ) AS PickupLocation, 
             (
                 SELECT PickUpAddress
                 FROM [dbo].[GetDriverDeliveryLocation](FUM.FumigationId, TD.DriverID)
             ) AS DeliveryLocation, 
                    SS.StatusAbbreviation AS StatusName, 
                    SS.FontColor, 
                    SS.Colour,    
             --ISNULL(      
             --   (      
             --       SELECT PickUpArrival      
             --       FROM [dbo].[GetFumigationLocation](FUM.FumigationId)      
             --   ), NULL) AS PickDateTime,    
                    ISNULL(PE.PickUpEquipment, '') AS DriverEquipment,
                    CASE
                        WHEN dbo.ufnGetFumigationPreTripStatus(FUM.FumigationId, TC.UserId) IS NULL
                        THEN 'PENDING'
                        ELSE dbo.ufnGetFumigationPreTripStatus(FUM.FumigationId, TC.UserId)
                    END PreTripStatus, 
                    FUM.CreatedOn, 
                    'Fumigation' AS Types,

             TD.UserId       
             --ISNULL(TC.FumigationPreTripCheckupId, 0) PreTripCheckupId,                           
             --FUM.StatusId       

             INTO #FUMIGATIONTEMPDATA
             FROM tblFumigation FUM                           
                  -- INNER JOIN [dbo].[tblFumigationRouts] TFR ON TFR.FumigationId = FUM.FumigationId                                           
                  LEFT JOIN [dbo].[tblFumigationEquipmentNDriver] TFEND ON TFEND.FumigationId = FUM.FumigationId
                  LEFT JOIN [dbo].[tbldriver] TD ON TD.DriverID = TFEND.DriverId
                  LEFT JOIN [dbo].[tblUser] TU ON TU.UserId = TD.UserId
                  LEFT JOIN [dbo].[tblShipmentStatus] SS ON FUM.StatusId = SS.StatusId
                  LEFT JOIN CTE_TotalBoxNPallentCount TBC ON FUM.FumigationId = TBC.FumigationId
                  LEFT JOIN CTE_PickUpEquipment PE ON FUM.FumigationId = PE.FumigationIds
                  LEFT JOIN CTE_AWB AWB ON FUM.FumigationId = AWB.FumigationIds
                  LEFT JOIN [dbo].[tblFumigationPreTripCheckUp] TC ON TC.FumigationId = FUM.FumigationId
                                                                      AND TC.UserId = TU.Userid
             WHERE TD.UserId = @UserId
                   AND AWB.DriverId = TD.DriverId
                   AND FUM.IsDeleted = 0
                   AND (FUM.StatusId != 1
                        AND FUM.StatusId != 11
                        AND FUM.StatusId != 8);
        SELECT Id, 
               QuantityNMethod, 
               (CASE
                    WHEN AirWayBill IS NULL
                    THEN CustomerPO
                    WHEN CustomerPO IS NULL
                    THEN OrderNo
                    ELSE AirWayBill
                END) AS AirWayBill, 
               DriverInstruction, 
               PickupLocation, 
               DeliveryLocation, 
               StatusName, 
               FontColor, 
               Colour, 
               Types, 
               DriverEquipment, 
               PreTripStatus, 
               CreatedOn, 
               [TotalCount] = COUNT(Id) OVER()  
        -- INTO #tempAll  
        FROM
        (
            SELECT *
            FROM View_DriverShipmentDetail where UserId = @UserId
            UNION ALL
            SELECT *
            FROM #FUMIGATIONTEMPDATA
        ) t1      
        --FROM #tempAll      
        WHERE((ISNULL(@SearchTerm, '') = ''
               OR t1.StatusName LIKE '%' + @SearchTerm + '%')
              OR (ISNULL(@SearchTerm, '') = ''
                  OR t1.AirWayBill LIKE '%' + @SearchTerm + '%')
              OR (ISNULL(@SearchTerm, '') = ''
                  OR t1.DriverInstruction LIKE '%' + @SearchTerm + '%')
              OR (ISNULL(@SearchTerm, '') = ''
                  OR t1.PickupLocation LIKE '%' + @SearchTerm + '%')
              OR (ISNULL(@SearchTerm, '') = ''
                  OR t1.DeliveryLocation LIKE '%' + @SearchTerm + '%')
              OR (ISNULL(@SearchTerm, '') = ''
                  OR t1.QuantityNMethod LIKE '%' + @SearchTerm + '%')
              OR (ISNULL(@SearchTerm, '') = ''
                  OR t1.DriverEquipment LIKE '%' + @SearchTerm + '%')
              OR (ISNULL(@SearchTerm, '') = ''
                  OR t1.CustomerPO LIKE '%' + @SearchTerm + '%')
              OR (ISNULL(@SearchTerm, '') = ''
                  OR t1.OrderNo LIKE '%' + @SearchTerm + '%')
              OR (ISNULL(@SearchTerm, '') = ''
                  OR t1.PreTripStatus LIKE '%' + @SearchTerm + '%'))
        ORDER BY CASE
                     WHEN(@SortColumn = 'Id'
                          AND @SortOrder = 'asc')
                     THEN CreatedOn
                 END ASC,
                 CASE
                     WHEN(@SortColumn = 'Id'
                          AND @SortOrder = 'desc')
                     THEN CreatedOn
                 END DESC
        OFFSET @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY;  
        --SELECT @TotalCount =  
        --(  
        --    SELECT TOP 1 TL.TotalCount  
        --    FROM #tempAll TL  
        --);  
        --SELECT Id,   
        --       QuantityNMethod,   
        --       (CASE  
        --            WHEN AirWayBill IS NULL  
        --            THEN CustomerPO  
        --            WHEN CustomerPO IS NULL  
        --            THEN OrderNo  
        --            ELSE AirWayBill  
        --        END) AS AirWayBill,   
        --       DriverInstruction,   
        --       PickupLocation,   
        --       DeliveryLocation,   
        --       StatusName,   
        --       FontColor,   
        --       Colour,   
        --       DriverEquipment,   
        --       PreTripStatus,   
        --       CreatedOn,   
        --       Types  
        --FROM #tempAll;  

        --DROP TABLE #TEMPDATA;
        DROP TABLE #FUMIGATIONTEMPDATA;  
        --DROP TABLE #tempAll;      

    END;