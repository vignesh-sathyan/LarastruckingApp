CREATE PROC [dbo].[usp_GetTrailerRentalList]  
(@SearchTerm VARCHAR(50),   
 @SortColumn VARCHAR(50),   
 @SortOrder  VARCHAR(50),   
 @PageNumber INT,   
 @PageSize   INT,   
 @TotalCount INT OUT  
)  
AS  
    BEGIN    
        --  SET NOCOUNT ON;     
        -- calculate the starting and ending of records                        
        SET @SortColumn = LOWER(ISNULL(@SortColumn, ''));  
        SET @SortOrder = LOWER(ISNULL(@SortOrder, ''));  
        SET @SearchTerm = ISNULL(@SearchTerm, '');  
        WITH CTE_Equipment(TrailerRentalIds,   
                           Equipment)  
             AS (SELECT DISTINCT  
                        (TrailerRentalId),   
                        STUFF(  
                 (  
                     SELECT ',' +  
                     (  
                         SELECT EquipmentNo  
                         FROM tblEquipmentDetail  
                         WHERE EDID = CTRD.EquipmentId  
                     )  
                     FROM tblTrailerRentalDetail AS CTRD  
                     WHERE CTRD.TrailerRentalId = TR.TrailerRentalId  
                           AND CTRD.IsDeleted = 0  
                     GROUP BY CTRD.EquipmentId,   
                              CTRD.TrailerRentalId FOR XML PATH('')  
                 ), 1, 1, '') AS DeliveryEquipment  
                 FROM tblTrailerRental TR),  
             CTE_PickUpDriver(TrailerRentalIds,   
                              PickUpDriver)  
             AS (SELECT DISTINCT  
                        (TrailerRentalId),   
                        STUFF(  
                 (  
                     SELECT ',' +  
                     (  
                         SELECT CONCAT(D.FirstName, ' ', D.LastName) DriverName  
                         FROM tblDriver D  
                         WHERE D.DriverId = TRD.PickupDriverId  
                     )  
                     FROM tblTrailerRentalDetail AS TRD  
                     WHERE TRD.TrailerRentalId = TR.TrailerRentalId  
                           AND TRD.IsDeleted = 0  
                     GROUP BY TRD.PickupDriverId,   
                              TRD.TrailerRentalId FOR XML PATH('')  
                 ), 1, 1, '') AS PickUpDriver  
                 FROM tblTrailerRental TR),  
             CTE_DeliveryDriver(TrailerRentalIds,   
                                DeliveryDriver)  
             AS (SELECT DISTINCT  
                        (TrailerRentalId),   
                        STUFF(  
                 (  
                     SELECT ',' +  
                     (  
                         SELECT CONCAT(D.FirstName, ' ', D.LastName) DriverName  
                         FROM tblDriver D  
                         WHERE D.DriverId = TRD.DeliveryDriverId  
                     )  
                     FROM tblTrailerRentalDetail AS TRD  
                     WHERE TRD.TrailerRentalId = TR.TrailerRentalId  
                           AND TRD.IsDeleted = 0  
                     GROUP BY TRD.DeliveryDriverId,   
                              TRD.TrailerRentalId FOR XML PATH('')  
                 ), 1, 1, '') AS DeliveryDriver  
                 FROM tblTrailerRental TR)  
             SELECT TR.TrailerRentalId,   
                    Eqp.Equipment,   
                    CR.CustomerName,   
                    PD.PickUpDriver,   
                    DD.DeliveryDriver,   
                   ( ISNULL(  
             (  
                 SELECT PickUpLocation  
                 FROM [dbo].[GetTrailerRentalDetial](TR.TrailerRentalId)  
             ), ''))  PickUpLocation,   
                    ISNULL(  
             (  
                 SELECT DeliveryLocation  
                 FROM [dbo].[GetTrailerRentalDetial](TR.TrailerRentalId)  
             ), '') AS DeliveryLocation,   
                ISNULL(  
             (  
                 SELECT StartDate  
                 FROM [dbo].[GetTrailerRentalDetial](TR.TrailerRentalId)  
             ), '') AS StartDate,  
                 ISNULL(  
             (  
                 SELECT EndDate  
                 FROM [dbo].[GetTrailerRentalDetial](TR.TrailerRentalId)  
             ), '') AS EndDate,    
                    [TotalCount] = COUNT(*) OVER()  
             INTO #TRAILERENTALDATA  
             FROM tblTrailerRental TR  
                  INNER JOIN [dbo].[tblCustomerRegistration] CR ON TR.CustomerId = CR.CustomerId  
                  LEFT JOIN CTE_Equipment Eqp ON TR.TrailerRentalId = Eqp.TrailerRentalIds  
                  LEFT JOIN CTE_PickUpDriver PD ON TR.TrailerRentalId = PD.TrailerRentalIds  
                  LEFT JOIN CTE_DeliveryDriver DD ON TR.TrailerRentalId = DD.TrailerRentalIds  
             WHERE TR.IsDeleted = 0  
                   AND (CustomerName LIKE '%' + @SearchTerm + '%'  
                        OR Equipment LIKE '%' + @SearchTerm + '%'  
      --OR PickUpLocation LIKE '%' + @SearchTerm + '%'  
        OR PickUpDriver LIKE '%' + @SearchTerm + '%'  
      --OR DeliveryLocation LIKE '%' + @SearchTerm + '%'  
         OR DeliveryDriver LIKE '%' + @SearchTerm + '%'   
           
      )  
             ORDER BY  
    CASE  
                          WHEN(@SortColumn = 'TrailerRentalId'  
                               AND @SortOrder = 'asc')  
                          THEN TrailerRentalId  
                      END ASC,  
                      CASE  
                          WHEN(@SortColumn = 'TrailerRentalId'  
                               AND @SortOrder = 'desc')  
                          THEN TrailerRentalId  
                      END DESC,  
      
     CASE  
                          WHEN(@SortColumn = 'CustomerName'  
                               AND @SortOrder = 'asc')  
                          THEN TrailerRentalId  
                      END ASC,  
                      CASE  
                          WHEN(@SortColumn = 'CustomerName'  
                               AND @SortOrder = 'desc')  
                          THEN TrailerRentalId  
                      END DESC,  
                      CASE  
                          WHEN(@SortColumn = 'Equipment'  
                               AND @SortOrder = 'asc')  
                          THEN TrailerRentalId  
                      END ASC,  
                      CASE  
                          WHEN(@SortColumn = 'Equipment'  
                               AND @SortOrder = 'desc')  
                          THEN TrailerRentalId  
                      END DESC,  
        CASE  
                          WHEN(@SortColumn = 'PickUpLocation'  
                               AND @SortOrder = 'asc')  
                          THEN TrailerRentalId  
                      END ASC,  
                      CASE  
                          WHEN(@SortColumn = 'PickUpLocation'  
                               AND @SortOrder = 'desc')  
                          THEN TrailerRentalId  
                      END DESC,  
        CASE  
                          WHEN(@SortColumn = 'PickUpDriver'  
                               AND @SortOrder = 'asc')  
                          THEN TrailerRentalId  
                      END ASC,  
                      CASE  
                          WHEN(@SortColumn = 'PickUpDriver'  
                               AND @SortOrder = 'desc')  
                          THEN TrailerRentalId  
                      END DESC,  
        
       CASE  
                          WHEN(@SortColumn = 'DeliveryLocation'  
                               AND @SortOrder = 'asc')  
                          THEN TrailerRentalId  
                      END ASC,  
                      CASE  
                          WHEN(@SortColumn = 'DeliveryLocation'  
                               AND @SortOrder = 'desc')  
                          THEN TrailerRentalId  
                      END DESC,  
       CASE  
                          WHEN(@SortColumn = 'DeliveryDriver'  
                               AND @SortOrder = 'asc')  
                          THEN TrailerRentalId  
                      END ASC,  
                      CASE  
                          WHEN(@SortColumn = 'DeliveryDriver'  
                            AND @SortOrder = 'desc')  
                          THEN TrailerRentalId  
                      END DESC ,  
       CASE  
                          WHEN(@SortColumn = 'EndDate'  
                               AND @SortOrder = 'asc')  
                          THEN TrailerRentalId  
                      END ASC,  
                      CASE  
                          WHEN(@SortColumn = 'EndDate'  
                            AND @SortOrder = 'desc')  
                          THEN TrailerRentalId  
                      END DESC  ,  
       CASE  
                          WHEN(@SortColumn = 'StartDate'  
                               AND @SortOrder = 'asc')  
                          THEN TrailerRentalId  
                      END ASC,  
                      CASE  
                          WHEN(@SortColumn = 'StartDate'  
                            AND @SortOrder = 'desc')  
                          THEN TrailerRentalId  
                      END DESC  
  
  
  
  
             OFFSET @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY;  
        IF EXISTS  
        (  
            SELECT 1  
            FROM #TRAILERENTALDATA  
        )  
            BEGIN  
                SELECT @TotalCount =  
                (  
                    SELECT TOP 1 TRD.TotalCount  
                    FROM #TRAILERENTALDATA TRD  
                );--COUNT( FUM.FumigationId) from #TEMPDATA  FUM            
        END;  
            ELSE  
            BEGIN  
                SELECT @TotalCount = 0;  
        END;  
        SELECT *  
        FROM #TRAILERENTALDATA;  
        DROP TABLE #TRAILERENTALDATA;  
    END;