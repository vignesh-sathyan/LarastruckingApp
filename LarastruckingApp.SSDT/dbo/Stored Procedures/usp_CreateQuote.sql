CREATE PROCEDURE [dbo].[usp_CreateQuote]  
(@SPType                     INT                               = NULL,   
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
            IF(@SPType = 1)  
                BEGIN  
                    --Check customer if not exists then add customer      
                    IF(@CustomerId = 0)  
                        BEGIN  
                            INSERT INTO [dbo].[tblCustomerRegistration]  
                            (CustomerName,   
                             IsPickDropLocation,   
                             IsFullFledgedCustomer,   
                             IsActive,   
                             IsDeleted,   
                             CreatedBy,   
                             CreatedOn  
                            )  
                            VALUES  
                            (@CustomerName,   
                             0,   
                             0,   
                             1,   
                             0,   
                             @CreatedBy,   
                             GETUTCDATE()  
                            );  
                            SET @CustomerId = SCOPE_IDENTITY();  
                            INSERT INTO [dbo].[tblBaseAddress]  
                            (CustomerId,   
                             BillingPhoneNumber,   
                             BillingEmail  
                            )  
                            VALUES  
                            (@CustomerId,   
                             @Phone,   
                             @Email  
                            );  
                    END;  
                    DECLARE @QuotesIdentityId INT;  
  
                    --Insert Quote detail  
                    INSERT INTO tblQuotes  
                    (CustomerId,   
                     QuotesName,   
                     QuoteDate,   
                     ValidUptoDate,   
                     FinalTotalAmount,   
                     QuoteStatusId,   
                     CreatedDate,   
                     CreatedBy  
                    )  
                    VALUES  
                    (@CustomerId,   
                     @QuotesName,   
                     @QuoteDate,   
                     @ValidUptoDate,   
                     @FinalTotalAmount,   
                     @QuoteStatusId,   
                     GETUTCDATE(),   
                     @CreatedBy  
                    );  
                    SET @QuotesIdentityId = SCOPE_IDENTITY();  
  
                    /*Add route detail into tblQuoteRouteStops*/  
  
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
                                   SELECT @QuotesIdentityId,   
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
                                   SELECT @QuotesIdentityId,   
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
                                                                                        AND QRS.QuoteId = @QuotesIdentityId  
                                                                                        AND CBFD.RouteNo = QRS.RouteNo;  
                    END;  
  
                    /* Add base freight detail into tblBaseFreightDetails*/  
  
                    IF EXISTS  
                    (  
                        SELECT 1  
                        FROM @CustomerBaseFreightDetails  
                    )  
                        BEGIN  
                            INSERT INTO [dbo].[tblBaseFreightDetails]  
                            (PickupLocationId,   
                             DeliveryLocationId,   
                             Commodity,   
                  FreightTypeId,   
                             PricingMethodId,   
                             MinFee,   
                             Upto,   
                             UnitPrice,   
                             IsActive,   
                             IsDeleted,   
                             CreatedDate,   
                             CreatedBy,ModifiedDate,ModifiedBy  
                            )  
                                   SELECT CBFD.PickupLocationId,   
                                          CBFD.DeliveryLocationId,   
                                          CBFD.Commodity,   
                                          CBFD.FreightTypeId,   
                                          CBFD.PricingMethodId,   
                                          CBFD.MinFee,   
                                          CBFD.Upto,   
                                          CBFD.UnitPrice,   
                                          1,   
                                          0,   
                                          GETUTCDATE(),   
                                          @CreatedBy,  
             GETUTCDATE(),   
                                          @CreatedBy  
                                   FROM @CustomerBaseFreightDetails AS CBFD  
           WHERE NOT EXISTS (SELECT * FROM [dbo].[tblBaseFreightDetails]  
                                         AS BFD WHERE CBFD.PickupLocationId = BFD.PickupLocationId  
                                                                                           AND CBFD.DeliveryLocationId = BFD.DeliveryLocationId  
                                                                                           AND CBFD.PricingMethodId = BFD.PricingMethodId);  
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
                                   SELECT @QuotesIdentityId,   
                                          SRS.QuoteRouteStopsId,   
                                          AP.AccessorialFeeTypeId,   
                                          AP.Unit,   
                                          AP.AmtPerUnit,   
                                          AP.Amount,
										  AP.Reason  
                                   FROM @AccessorialPrice AS AP  
                                        INNER JOIN [dbo].[tblQuoteRouteStops] AS SRS ON AP.RouteNo = SRS.RouteNo  
                                                                                           AND SRS.QuoteId = @QuotesIdentityId;  
                    END;  
  
                    --SELECT @QuotesIdentityId  
  
                    SELECT 'INSERTED' AS ResponseText;  
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