CREATE PROCEDURE [dbo].[usp_CreateFumigation]
(@SPType                     INT                                   = NULL, 
 @CustomerId                 BIGINT                                = NULL, 
 @StatusId                   INT                                   = NULL, 
 @SubStatusId                INT                                   = NULL, 
 @Reason                     VARCHAR(MAX)                          = NULL, 
 @RequestedBy                VARCHAR(100)                          = NULL,         
 -- @VendorNconsignee           VARCHAR(100)                          = NULL,         
 @ShipmentRefNo              VARCHAR(50)                           = NULL, 
 @Comments                   VARCHAR(MAX)                          = NULL, 
 @CreatedBy                  INT                                   = NULL, 
 @FumigationComment          VARCHAR(MAX)                          = NULL, 
 @FumigationRoutes AS           [dbo].[UT_FumigationRoutes] READONLY, 
 @AccessorialPrice AS           [dbo].[UT_FumigationAccessorialPrice] READONLY, 
 @FumigationEquipmentNdriver AS [dbo].[UT_FumigationEquipmentNDriver] READONLY
)
AS
    BEGIN
        BEGIN TRY
            SET NOCOUNT ON;
            BEGIN TRANSACTION;
            IF(@SPType = 1)
                BEGIN
                    DECLARE @FumigationIdentityId INT;
                    DECLARE @defaultShipmentRefNo VARCHAR(50);
                    SET @defaultShipmentRefNo = CONCAT('SRN_',
                    (
                        SELECT CASE
                                   WHEN MAX(ShipmentRefNoId) IS NULL
                                   THEN 1
                                   ELSE MAX(ShipmentRefNoId) + 1
                               END
                        FROM tblShipmentRefNo
                    ));
                    INSERT INTO tblShipmentRefNo
                    VALUES
                    (@defaultShipmentRefNo, 
                     GETUTCDATE()
                    );        
                    --Insert Quote detail            
                    INSERT INTO tblFumigation
                    (CustomerId, 
                     StatusId, 
                     SubStatusId, 
                     Reason,         
                     --  VendorNconsignee,         
                     RequestedBy, 
                     ShipmentRefNo, 
                     Comments, 
                     CreatedBy, 
                     CreatedOn
                    )
                    VALUES
                    (@CustomerId, 
                     @StatusId, 
                     @SubStatusId, 
                     @Reason,         
                     --@VendorNconsignee,         
                     @RequestedBy, 
                     @defaultShipmentRefNo, 
                     @Comments, 
                     @CreatedBy, 
                     GETUTCDATE()
                    );
                    SET @FumigationIdentityId = SCOPE_IDENTITY();

                    --#region [Insert fumigation history table]        
                    INSERT INTO tblFumigationStatusHistory
                    (FumigationId, 
                     StatusId, 
                     SubStatusId, 
                     Reason, 
                     CreatedBy, 
                     CreatedOn
                    )
                    VALUES
                    (@FumigationIdentityId, 
                     @StatusId, 
                     @SubStatusId, 
                     @Reason, 
                     @CreatedBy, 
                     GETUTCDATE()
                    );        
                    --#endregion          
                    --#region [Insert shipment driver and route detail]          
                    IF EXISTS
                    (
                        SELECT *
                        FROM @FumigationRoutes
                    )
                        BEGIN
                            INSERT INTO tblFumigationRouts
                            (RouteNo, 
                             FumigationId, 
                             FumigationTypeId, 
                             AirWayBill, 
                             CustomerPO, 
                             ContainerNo, 
                             PickUpLocation, 
                             PickUpArrival, 
                             FumigationSite, 
                             FumigationArrival, 
                             DeliveryLocation, 
                             DeliveryArrival, 
                             PalletCount, 
                             BoxCount, 
                             BoxType, 
                             Temperature, 
                             TemperatureType, 
                             MinFee, 
                             AddFee, 
                             UpTo, 
                             TrailerPosition, 
                             TotalFee, 
                             IsDeleted, 
                             ReleaseDate, 
                             DepartureDate, 
                             Commodity, 
                             PricingMethod, 
                             TrailerDays, 
                             VendorNConsignee
                            )
                                   SELECT RouteNo, 
                                          @FumigationIdentityId, 
                                          FumigationTypeId, 
                                          AirWayBill, 
                                          CustomerPO, 
                                          ContainerNo, 
                                          PickUpLocation, 
                                          PickUpArrival, 
                                          FumigationSite, 
                                          FumigationArrival, 
                                          DeliveryLocation, 
                                          DeliveryArrival, 
                                          PalletCount, 
                                          BoxCount, 
                                          BoxType, 
                                          Temperature, 
                                          TemperatureType, 
                                          MinFee, 
                                          AddFee, 
                                          UpTo, 
                                          TrailerPosition, 
                                          TotalFee, 
                                          IsDeleted, 
                                          ReleaseDate, 
                                          DepartureDate, 
                                          Commodity, 
                                          PricingMethod, 
                                          TrailerDays, 
                                          VendorNConsignee
                                   FROM @FumigationRoutes;
                    END;          
                    --#endregion     
                    --#region [Insert fumigatuin event history ]          
                    INSERT INTO [dbo].[tblFumigationEventHistory]
                    (FumigationId, 
                     StatusId, 
                     Event, 
                     UserId, 
                     EventDateTime
                    )
                    VALUES
                    (@FumigationIdentityId, 
                     @StatusId, 
                     'STATUS', 
                     @CreatedBy, 
                     GETUTCDATE()
                    );          
                    --#endregion    

                    /*Add accessorial charges detail into tblAssessorialPrice*/

                    IF EXISTS
                    (
                        SELECT 1
                        FROM @AccessorialPrice
                    )
                        BEGIN
                            INSERT INTO [dbo].[tblFumigationAccessorialPrice]
                            (FumigationId, 
                             FumigationRoutesId, 
                             AccessorialFeeTypeId, 
                             Unit, 
                             AmtPerUnit, 
                             Amount, 
                             Reason
                            )
                                   SELECT @FumigationIdentityId, 
                                          SRS.FumigationRoutsId, 
                                          AP.AccessorialFeeTypeId, 
                                          AP.Unit, 
                                          AP.AmtPerUnit, 
                                          AP.Amount, 
                                          AP.Reason
                                   FROM @AccessorialPrice AS AP
                                        INNER JOIN [dbo].[tblFumigationRouts] AS SRS ON AP.RouteNo = SRS.RouteNo
                                                                                        AND SRS.FumigationId = @FumigationIdentityId;
                    END;
                    IF EXISTS
                    (
                        SELECT 1
                        FROM @FumigationEquipmentNdriver
                    )
                        BEGIN
                            INSERT INTO [dbo].[tblFumigationEquipmentNDriver]
                            (FumigationId, 
                             FumigationRoutsId, 
                             EquipmentId, 
                             DriverId, 
                             IsPickUp, 
                             IsDeleted
                            )
                                   SELECT @FumigationIdentityId, 
                                          SRS.FumigationRoutsId, 
                                          FED.EquipmentId, 
                                          FED.DriverId, 
                                          FED.IsPickUp, 
                                          FED.IsDeleted
                                   FROM @FumigationEquipmentNdriver AS FED
                                        INNER JOIN [dbo].[tblFumigationRouts] AS SRS ON FED.RouteNo = SRS.RouteNo
                                                                                        AND SRS.FumigationId = @FumigationIdentityId;
                    END;
                    IF(@FumigationComment != NULL
                       OR @FumigationComment != '')
                        BEGIN
                            INSERT INTO tblFumigationComments
                            (FumigationId, 
                             Comment, 
                             CommentBy, 
                             CreatedBy, 
                             CreatedOn
                            )
                            VALUES
                            (@FumigationIdentityId, 
                             @FumigationComment, 
                             'DP', 
                             @CreatedBy,  
                             GETUTCDATE()
                            );
                    END;
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