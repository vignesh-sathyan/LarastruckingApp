CREATE FUNCTION dbo.usp_GetFreightTotal
(@CustomerId         INT NULL, 
 @PickUpLocationId   INT NULL, 
 @DeliveryLocationId INT NULL, 
 @FreightTypeId      INT NULL, 
 @PricingMethodId    INT NULL, 
 @PicUpDate          DATE NULL, 
 @Quantity           DECIMAL(18, 2)
)
RETURNS DECIMAL(18, 2)
AS
     BEGIN
         DECLARE @FreightAmount DECIMAL(18, 2)= 0;
         --DECLARE @Quantity DECIMAL(18, 2)= 6.8;
         SELECT TOP 1 @FreightAmount = (CASE
                                            WHEN ISNULL(@Quantity, 0) <= ISNULL(CBFD.Upto, 0)
                                            THEN CBFD.MinFee
                                            ELSE(CBFD.MinFee + ((@Quantity - CBFD.Upto) * (CASE
                                                                                               WHEN ISNULL(CBFD.UnitPrice, '') = ''
                                                                                               THEN 0
                                                                                               ELSE CBFD.UnitPrice
                                                                                           END)))
                                        END)
         FROM tblQuotes QT
              LEFT JOIN tblQuoteRouteStops QRS ON QT.QuoteId = QRS.QuoteId
              LEFT JOIN tblCustomerBaseFreightDetails CBFD ON QRS.QuoteRouteStopsId = CBFD.QuoteRouteStopsId
         WHERE QT.CustomerId = @CustomerId
               AND QRS.PickupLocationId = @PickUpLocationId
               AND QRS.DeliveryLocationId = @DeliveryLocationId
               AND CBFD.FreightTypeId = @FreightTypeId
               AND CBFD.PricingMethodId = @PricingMethodId
         ORDER BY QT.QuoteId DESC;
         --PRINT @FreightAmount;
         RETURN @FreightAmount;
     END;