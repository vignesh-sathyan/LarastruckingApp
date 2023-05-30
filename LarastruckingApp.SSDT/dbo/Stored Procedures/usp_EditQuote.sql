CREATE PROCEDURE [dbo].[usp_EditQuote]    
(@SPType                     INT                               = NULL,     
 @QuoteId                    INT                               = NULL,     
 @CustomerId                 BIGINT                            = NULL,     
 @CustomerName               VARCHAR(100)                      = NULL,     
 @Email                      VARCHAR(50)                       = NULL,     
 @Phone                      VARCHAR(50)                       = NULL,     
 @QuotesName                 VARCHAR(200)                      = NULL,     
 @QuoteDate                  DATETIME                          = NULL,     
 @ValidUptoDate              DATETIME                          = NULL,     
 @FinalTotalAmount           DECIMAL(18, 2)                     = NULL,     
 @QuoteStatusId              INT                               = NULL,     
 @CreatedBy                  INT                               = NULL,     
 @CustomerBaseFreightDetails AS dbo.UT_CustomerBaseFreightDetails READONLY,     
 @QuoteRouteStopsDetail AS      dbo.UT_QuoteRouteStops READONLY,     
 @AccessorialPrice AS         dbo.UT_QuoteAccessorialPrice READONLY    
)    
AS    
    BEGIN    
        BEGIN TRY    
            SET NOCOUNT ON;    
            BEGIN TRANSACTION;    
            IF(@SPType = 2)    
                BEGIN    
    
                    --Insert Quote detail    
    
                    UPDATE tblQuotes    
                      SET     
                          QuotesName = @QuotesName,     
                          QuoteDate = @QuoteDate,     
                          ValidUptoDate = @ValidUptoDate,     
                          FinalTotalAmount = @FinalTotalAmount,     
                          QuoteStatusId = @QuoteStatusId,     
                          ModifiedDate = GETUTCDATE(),     
                          ModifiedBy = @CreatedBy    
                    WHERE QuoteId = @QuoteId;    
    
                    /*Add route detail into tblQuoteRouteStops*/    
    
                    DELETE FROM [dbo].[tblQuoteAccessorialPrice]    
                    WHERE QuoteId = @QuoteId;    
       DELETE FROM [dbo].[tblAssessorialPrice]  
                    WHERE QuoteId = @QuoteId;    
                    DELETE FROM [dbo].[tblCustomerBaseFreightDetails]    
                    WHERE QuoteId = @QuoteId;    
                    DELETE FROM [dbo].[tblQuoteRouteStops]    
                    WHERE QuoteId = @QuoteId;    
                    IF EXISTS    
                    (    
                        SELECT 1    
                        FROM @QuoteRouteStopsDetail    
                    )    
                        BEGIN    
                            --Insert Route Stops detail    
                            INSERT INTO [dbo].[tblQuoteRouteStops]    
                            (QuoteId,     
                             RouteNo,     
                             PickupLocationId,     
                             DeliveryLocationId,     
                             PickDateTime,     
                             DeliveryDateTime    
                            )    
                                   SELECT @QuoteId,     
                                          RouteNo,     
                                          PickupLocationId,     
                                          DeliveryLocationId,     
                                          PickDateTime,     
                                          DeliveryDateTime    
                                   FROM @QuoteRouteStopsDetail;    
                            --Insert Customer BaseFreight detail    
                    END;    
    
                    /* Add customer base freight detail into tblCustomerBaseFreightDetails*/    
    
                    IF EXISTS    
                    (    
                        SELECT 1    
                        FROM @CustomerBaseFreightDetails    
                    )    
                        BEGIN    
                            INSERT INTO [dbo].[tblCustomerBaseFreightDetails]    
                            (QuoteId,     
                             RouteNo,     
                             QuoteRouteStopsId,     
                             PickupLocationId,     
                             DeliveryLocationId,     
                Commodity,     
                             FreightTypeId,     
                             PricingMethodId,     
                             MinFee,     
                             Upto,     
                             UnitPrice,     
                             Hazardous,     
                             Temperature,     
                             QutWgtVlm,     
                             TotalPrice,  
        NoOfBox,  
        Weight,  
        Unit,  
        TrailerCount    
                            )    
                                   SELECT @QuoteId,     
                                          CBFD.RouteNo,     
                                          QRS.QuoteRouteStopsId,     
                                          CBFD.PickupLocationId,     
                                          CBFD.DeliveryLocationId,     
                                          CBFD.Commodity,     
                                          CBFD.FreightTypeId,     
                                          CBFD.PricingMethodId,     
                                          CBFD.MinFee,     
                                          CBFD.Upto,     
                                          CBFD.UnitPrice,     
                                          CBFD.Hazardous,     
                                          CBFD.Temperature,     
                                          CBFD.QutWgtVlm,     
                                          CBFD.TotalPrice,  
            CBFD.NoOfBox,  
            CBFD.Weight,  
            CBFD.Unit,  
            CBFD.TrailerCount  
                                   FROM @CustomerBaseFreightDetails AS CBFD    
                                        INNER JOIN [dbo].[tblQuoteRouteStops] AS QRS ON CBFD.PickupLocationId = QRS.PickupLocationId    
                                                                                        AND CBFD.DeliveryLocationId = QRS.DeliveryLocationId    
                                                                                        AND QRS.QuoteId = @QuoteId    
                                                                                        AND CBFD.RouteNo = QRS.RouteNo;    
                    END;    
    
                    /*Add accessorial charges detail into tblAssessorialPrice*/    
    
                 IF EXISTS    
                    (    
                        SELECT 1    
                        FROM @AccessorialPrice    
                    )    
                        BEGIN    
                            INSERT INTO [dbo].[tblQuoteAccessorialPrice]    
                            (QuoteId,     
                             QuoteRouteStopsId,     
                             AccessorialFeeTypeId,     
                             Unit,     
                             AmtPerUnit,     
                             Amount,
							 Reason    
                            )    
                                   SELECT @QuoteId,     
                                          SRS.QuoteRouteStopsId,     
                                          AP.AccessorialFeeTypeId,     
                                          AP.Unit,     
                                          AP.AmtPerUnit,     
                                          AP.Amount,
										  AP.Reason    
                                   FROM @AccessorialPrice AS AP    
                                        INNER JOIN [dbo].[tblQuoteRouteStops] AS SRS ON AP.RouteNo = SRS.RouteNo    
                                                                                           AND SRS.QuoteId =  @QuoteId;    
                    END;    
    
                    --SELECT @QuotesIdentityId    
    
                    SELECT 'UPDATED' AS ResponseText;    
            END;    
            COMMIT TRANSACTION;    
        END TRY    
        BEGIN CATCH    
            ROLLBACK TRANSACTION;    
            DECLARE @ErrorMessage NVARCHAR(4000);    
            SET @ErrorMessage = ERROR_MESSAGE();    
            SELECT @ErrorMessage AS ResponseText;    
        END CATCH;    
    END;