      
CREATE PROCEDURE [dbo].[usp_GetPricingMethod]      
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
                SELECT COUNT(QT.QuoteId)      
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
                           CBFD.PricingMethodId,       
                    (      
                        SELECT PT.PricingMethodName      
                        FROM tblPricingMethod AS PT      
                        WHERE PT.PricingMethodId = CBFD.PricingMethodId  AND PT.IsActive =1  and PT.IsDeleted=0  
                    ) AS PricingMethodName      
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
                    SELECT PricingMethodId,       
                           PricingMethodName      
                    FROM tblPricingMethod where IsActive=1 and IsDeleted=0;      
            END;      
        END TRY      
        BEGIN CATCH      
            DECLARE @ErrorMessage NVARCHAR(4000);      
            SET @ErrorMessage = ERROR_MESSAGE();      
            SELECT @ErrorMessage AS ResponseText;      
        END CATCH;      
    END;