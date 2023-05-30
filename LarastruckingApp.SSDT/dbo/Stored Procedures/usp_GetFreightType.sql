CREATE PROCEDURE [dbo].[usp_GetFreightType]    
(@CustomerId         BIGINT   = NULL,     
 @PickupLocationId   INT      = NULL,     
 @DeliveryLocationId INT      = NULL,     
 @PickupArrivalDate  DATETIME = NULL    
)    
AS    
    BEGIN    
        BEGIN TRY   
   SET NOCOUNT ON;      
            DECLARE @Count AS INT;    
            SET @Count =    
            (    
                SELECT COUNT(*)    
                FROM tblQuotes QT    
                     LEFT JOIN tblQuoteRouteStops RS ON QT.QuoteId = RS.QuoteId    
                     LEFT JOIN tblCustomerBaseFreightDetails CBFD ON RS.QuoteRouteStopsId = CBFD.QuoteRouteStopsId    
                WHERE QT.CustomerId = @CustomerId    
                      AND CBFD.PickupLocationId = @PickupLocationId    
                      AND CBFD.DeliveryLocationId = @DeliveryLocationId    
                      AND @PickupArrivalDate BETWEEN QT.QuoteDate AND QT.ValidUptoDate    
            );    
            IF(@Count > 0)    
                BEGIN    
                    SELECT DISTINCT     
                           CBFD.FreightTypeId,     
                    (    
                        SELECT FT.FreightTypeName    
                        FROM tblFreightType AS FT    
                        WHERE FT.FreightTypeId = CBFD.FreightTypeId  and FT.IsDeleted=0  AND FT.IsActive=1
                    ) AS FreightTypeName    
                    FROM tblQuotes QT    
                         LEFT JOIN tblQuoteRouteStops RS ON QT.QuoteId = RS.QuoteId    
                         LEFT JOIN tblCustomerBaseFreightDetails CBFD ON RS.QuoteRouteStopsId = CBFD.QuoteRouteStopsId    
                    WHERE QT.CustomerId = @CustomerId    
                          AND CBFD.PickupLocationId = @PickupLocationId    
                          AND CBFD.DeliveryLocationId = @DeliveryLocationId    
                          AND @PickupArrivalDate BETWEEN QT.QuoteDate AND QT.ValidUptoDate;    
            END;    
                ELSE    
                BEGIN    
                    SELECT FreightTypeId,     
                           FreightTypeName    
                    FROM tblFreightType where IsDeleted=0 AND IsActive=1;    
            END;    
        END TRY    
        BEGIN CATCH    
            DECLARE @ErrorMessage NVARCHAR(4000);    
            SET @ErrorMessage = ERROR_MESSAGE();    
            SELECT @ErrorMessage AS ResponseText;    
        END CATCH;    
    END;