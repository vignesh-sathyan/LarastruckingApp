--Declare @TotalCount int            
--exec [usp_DriverDashboard] 7,'','StatusName','asc',0,10, @TotalCount out      
--print @TotalCount       
  
CREATE PROCEDURE [dbo].[usp_DriverDashboard]  
(@UserId     INT,                                                        
 --@StartRowIndex INT = NULL,                                                        
 --@PageSize INT = NULL,                                                        
 --@ShipmentId INT = NULL,                                                                                                     
 --@SearchText VARCHAR(100) = NULL,      
  
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
        SET @TotalCount =  
        (  
            SELECT COUNT(*)  
            FROM [dbo].[tblShipment] TS  
                 INNER JOIN [dbo].[tblShipmentEquipmentNdriver] TSEND ON TSEND.ShipmentId = TS.ShipmentId  
                 INNER JOIN [dbo].[tblShipmentRoutesStop] TSRS ON TSRS.ShippingId = TSEND.ShipmentId  
                 INNER JOIN [dbo].[tblShipmentFreightDetail] TSFD ON TSFD.ShipmentId = TS.ShipmentId  
                 INNER JOIN [dbo].[tblPricingMethod] TPM ON TPM.PricingMethodId = TSFD.PricingMethodId  
                 INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId  
                 INNER JOIN [dbo].[tblDriver] TD ON TD.DriverID = TSEND.DriverId  
                 INNER JOIN [dbo].[tblEquipmentDetail] TE ON TE.EDID = TSEND.EquipmentId              
                 -- INNER JOIN CTE_CommaSepartedVarietal CTEC on CTEC.ShipmentId=TS.ShipmentId                                              
                 LEFT JOIN [dbo].[tblPreTripCheckUp] TC ON TC.ShipmentId = TSEND.ShipmentId  
                                                           AND TC.UserId = TD.UserId  
            WHERE TD.UserId = @UserId  
                  AND (ISNULL(@SearchTerm, '') = ''  
                       OR StatusName LIKE '%' + @SearchTerm + '%')  
                  OR (ISNULL(@SearchTerm, '') = ''  
                      OR FirstName LIKE '%' + @SearchTerm + '%')  
                  OR (ISNULL(@SearchTerm, '') = ''  
                      OR LastName LIKE '%' + @SearchTerm + '%')  
                  OR (ISNULL(@SearchTerm, '') = ''  
                      OR ShipmentRefNo LIKE '%' + @SearchTerm + '%')  
                  OR (ISNULL(@SearchTerm, '') = ''  
                      OR LicencePlate LIKE '%' + @SearchTerm + '%')      
            --OR (ISNULL(@SearchTerm, '') = '' OR PreTripStatus LIKE '%' + @SearchTerm + '%')      
        );  
        WITH CTE_CommaSepartedVarietal(ShipmentId,   
                                       QuantityNMethod)  
             AS (SELECT DISTINCT  
                        (ShipmentId),   
                        STUFF(  
                 (  
                     SELECT ',' + CONVERT(VARCHAR(MAX), concat(tsfd.QuantityNweight, ' ', TPM.PricingMethodExt))  
                     FROM tblShipmentFreightDetail tsfd  
                          INNER JOIN tblPricingMethod tpm ON tpm.PricingMethodId = tsfd.PricingMethodId  
                     WHERE tsfd.ShipmentId = tsfdd.ShipmentId  
                           AND tsfd.IsDeleted = 0 FOR XML PATH('')  
                 ), 1, 1, '') AS QuantityNMethod  
                 FROM tblShipmentFreightDetail tsfdd),  
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
                    FROM [dbo].[tblShipment] TS  
                  INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId  
                  INNER JOIN cte_getDriver cted ON TS.ShipmentId = cted.ShipmentId  
                  INNER JOIN CTE_CommaSepartedVarietal CTEC ON CTEC.ShipmentId = TS.ShipmentId  
                  INNER JOIN cte_getPickDate CTEP ON TS.ShipmentId = CTEP.ShippingId  
                  INNER JOIN CTE_EqipmentCommaSeperated CTEE ON CTEE.ShipmentId = TS.ShipmentId  
                  LEFT JOIN [dbo].[tblPreTripCheckUp] TC ON TC.ShipmentId = TS.ShipmentId AND TC.UserId = cted.UserId 
				   
                  WHERE cted.UserId =@UserId  AND TS.IsDeleted = 0
                   AND (TS.StatusId != 1 AND TS.StatusId != 7 AND TS.StatusId != 8 )   
                    
                   AND( (ISNULL(@SearchTerm, '') = ''  
                       OR StatusName LIKE '%' + @SearchTerm + '%')  
                   OR (ISNULL(@SearchTerm, '') = ''  
                       OR PickDateTime LIKE '%' + @SearchTerm + '%')  
                   OR (ISNULL(@SearchTerm, '') = ''  
                       OR QuantityNMethod LIKE '%' + @SearchTerm + '%')  
                   OR (ISNULL(@SearchTerm, '') = ''  
                       OR ShipmentRefNo LIKE '%' + @SearchTerm + '%')  
                   OR (ISNULL(@SearchTerm, '') = ''  
                       OR DriverEquipment LIKE '%' + @SearchTerm + '%') )  
             --OR (ISNULL(@SearchTerm, '') = '' OR PreTripStatus LIKE '%' + @SearchTerm + '%')      
  
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
    END;