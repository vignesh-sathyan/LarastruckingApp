CREATE PROCEDURE [dbo].[usp_CreateShipment]    
(@SPType                   INT                             = NULL,     
 @CustomerId               BIGINT                          = NULL,     
 @StatusId                 INT                             = NULL,     
 @SubStatusId              INT                             = NULL,     
 @RequestedBy              VARCHAR(50)                     = NULL,     
 @Reason                   VARCHAR(500)                    = NULL,     
 @ShipmentRefNo            VARCHAR(500)                    = NULL,     
 @AirWayBill               VARCHAR(50)                     = NULL,     
 @CustomerPO               VARCHAR(50)                     = NULL,     
 @OrderNo                  VARCHAR(50)                     = NULL,     
 @CustomerRef              VARCHAR(50)                     = NULL,     
 @ContainerNo              VARCHAR(50)                     = NULL,     
 @PurchaseDoc              VARCHAR(50)                     = NULL,     
 @FinalTotalAmount         VARCHAR(50)                     = NULL,     
 @DriverInstruction        VARCHAR(1000)                   = NULL,     
 @VendorNconsignee         VARCHAR(100)                    = NULL,     
 @CreatedBy                INT                             = NULL,     
 @ShipmentRouteStopsDetail AS [dbo].[UT_ShipmentRouteStop] READONLY,     
 @AccessorialPrice AS         dbo.UT_ShipmentAccessorialPrice READONLY,     
 @ShipmentDriverNEquipment AS dbo.UT_ShipmentEquipmentNdriver READONLY,     
 @ShipmentFreightDetail AS    dbo.UT_ShipmentFreightDetails READONLY    
)    
AS    
    BEGIN    
        BEGIN TRY    
            SET NOCOUNT ON;    
            BEGIN TRANSACTION;    
            IF(@SPType = 1)    
                BEGIN    
                    DECLARE @ShipmentIdentityId INT;    
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
                    INSERT INTO tblShipment    
                    (CustomerId,     
                     StatusId,     
                     SubStatusId,     
                     RequestedBy,     
                     Reason,     
                     ShipmentRefNo,     
                     AirWayBill,     
                     CustomerPO,     
                     OrderNo,     
                     CustomerRef,     
                     ContainerNo,     
                     PurchaseDoc,     
                     FinalTotalAmount,     
                     DriverInstruction,     
                     CreatedBy,     
                     CreatedDate,     
                     VendorNconsignee,     
                     EquipmentId,     
                     DriverId    
                    )    
                    VALUES    
                    (@CustomerId,     
                     @StatusId,     
                     @SubStatusId,     
                     @RequestedBy,     
                     @Reason,     
                     @defaultShipmentRefNo,     
                     @AirWayBill,     
                     @CustomerPO,     
                     @OrderNo,     
                     @CustomerRef,     
                     @ContainerNo,     
                     @PurchaseDoc,     
                     @FinalTotalAmount,     
                     @DriverInstruction,     
                     @CreatedBy,     
                     GETUTCDATE(),     
                     @VendorNconsignee,     
                    1,     
                     1    
                    );    
                    SET @ShipmentIdentityId = SCOPE_IDENTITY();    
    
                    --#region [Insert status and sub stuatus ]      
                    INSERT INTO tblShipmentStatusHistory    
                    (ShipmentId,     
                     StatusId,     
                     SubStatusId,     
                     Reason,     
                     CreatedBy,     
                     CreatedOn    
                    )    
                    VALUES    
                    (@ShipmentIdentityId,     
                     @StatusId,     
                     @SubStatusId,     
                     @Reason,     
                     @CreatedBy,     
                     GETUTCDATE()    
                    );      
                    --#endregion    
					
					
                    --#region [Insert shipment event history ]      
                    INSERT INTO   [dbo].[tblShipmentEventHistory]  
                    (ShipmentId,     
                     StatusId, 
					 Event,    
                     UserId,
					 EventDateTime
						                   
                        
                    )    
                    VALUES    
                    (@ShipmentIdentityId,     
                     @StatusId,
					 'STATUS',     
                     @CreatedBy,     
                     GETUTCDATE()    
                    );      
                    --#endregion     
                    --#region [Insert shipment driver and route detail]      
                    IF EXISTS    
                    (    
                        SELECT *    
                        FROM @ShipmentDriverNEquipment    
                    )    
                        BEGIN    
                            INSERT INTO tblShipmentEquipmentNdriver    
                            (ShipmentId,     
                             DriverId,     
                             EquipmentId,     
                             IsActive,     
                             CreatedDate    
                            )    
                                   SELECT @ShipmentIdentityId,     
                                          DriverId,     
                                          EquipmentId,     
                                          1,     
                                          GETUTCDATE()    
                                   FROM @ShipmentDriverNEquipment;    
                    END;      
                    --#endregion      
    
                    /*Add accessorial charges detail into tblAssessorialPrice*/    
    
                    --SELECT @QuotesIdentityId      
                    IF EXISTS    
                    (    
                        SELECT 1    
                        FROM @ShipmentRouteStopsDetail    
                    )    
                        BEGIN    
                            INSERT INTO [dbo].[tblShipmentRoutesStop]    
                            (ShippingId,     
                             RouteNo,     
                             PickupLocationId,     
                             PickDateTime,     
                             PickUpDateTimeTo,     
                             DeliveryLocationId,     
                             DeliveryDateTime,     
                             DeliveryDateTimeTo,     
                             Comment,     
                             IsAppointmentRequired,     
                             IsPickUpWaitingTimeRequired,     
                             IsDeliveryWaitingTimeRequired    
                            )    
                                   SELECT @ShipmentIdentityId,     
                                          RouteNo,     
                                          PickupLocationId,     
                                          PickDateTime,     
                                          PickDateTimeTo,     
                                          DeliveryLocationId,     
                                          DeliveryDateTime,     
                                          DeliveryDateTimeTo,     
                                          Comment,     
                                          IsAppointmentRequired,     
                                          IsPickUpWaitingTimeRequired,     
                                          IsDeliveryWaitingTimeRequired    
                                   FROM @ShipmentRouteStopsDetail;    
                    END;    
    
                    /* Add base freight detail into tblBaseFreightDetails*/    
    
                    IF EXISTS    
                    (    
                        SELECT 1    
                        FROM @ShipmentFreightDetail    
                    )    
                        BEGIN    
                            INSERT INTO [dbo].[tblShipmentFreightDetail]    
                            (ShipmentId,     
                             ShipmentRouteStopeId,     
                             Commodity,     
         FreightTypeId,     
                             PricingMethodId,     
                             MinFee,     
                             Upto,     
                             UnitPrice,     
                             Temperature,     
                             TemperatureType,     
                             Hazardous,     
                             QuantityNweight,     
                             TotalPrice,     
                             Weight,     
                             Unit,     
                             NoOfBox,     
                             TrailerCount,     
                             Comments,     
                             IsPartialShipment,     
                             PartialBox,     
                             PartialPallete    
                            )    
                                   SELECT @ShipmentIdentityId,     
                                          SRS.ShippingRoutesId,     
                                          CBFD.Commodity,     
                                          CBFD.FreightTypeId,     
                                          CBFD.PricingMethodId,     
                                          CBFD.MinFee,     
                                          CBFD.Upto,     
                                          CBFD.UnitPrice,     
                                          CBFD.Temperature,     
                                          CBFD.TemperatureType,     
                                          CBFD.Hazardous,     
                                          CBFD.QutWgtVlm,     
                                          CBFD.TotalPrice,     
                                          CBFD.Weight,     
                                          CBFD.Unit,     
                                          CBFD.NoOfBox,     
                                          CBFD.TrailerCount,     
                                          CBFD.Comments,     
                                          CBFD.IsPartialShipment,     
                                          CBFD.PartialBox,     
                                          CBFD.PartialPallet    
                                   FROM @ShipmentFreightDetail AS CBFD    
                                        INNER JOIN [dbo].[tblShipmentRoutesStop] AS SRS ON CBFD.PickupLocationId = SRS.PickupLocationId    
                                                                                           AND CBFD.DeliveryLocationId = SRS.DeliveryLocationId    
                                                                                           AND SRS.ShippingId = @ShipmentIdentityId;    
                    END;    
    
                    /*Add accessorial charges detail into tblAssessorialPrice*/    
    
                    IF EXISTS    
                    (    
                        SELECT 1    
                        FROM @AccessorialPrice    
                    )    
                        BEGIN    
                            INSERT INTO [dbo].[tblShipmentAccessorialPrice]    
                            (ShipmentId,     
                             ShipmentRouteStopeId,     
                             AccessorialFeeTypeId,     
                             Unit,     
                             AmtPerUnit,     
                             Amount,  
        Reason    
                            )    
                                   SELECT @ShipmentIdentityId,     
                                          SRS.ShippingRoutesId,     
                                          AP.AccessorialFeeTypeId,     
                                          AP.Unit,     
                        AP.AmtPerUnit,     
                                          AP.Amount,  
            AP.Reason    
                                   FROM @AccessorialPrice AS AP    
                                        INNER JOIN [dbo].[tblShipmentRoutesStop] AS SRS ON AP.RouteNo = SRS.RouteNo    
                                                                                           AND SRS.ShippingId = @ShipmentIdentityId;    
                    END;    
                    IF(@DriverInstruction != NULL or @DriverInstruction != '')    
                        BEGIN    
                            INSERT INTO tblShipmentCommments    
                            (ShipmentId,     
                             Comment,     
                             CommentBy,     
                             CreatedBy,     
                             CreatedOn    
                            )    
                            VALUES    
                            (@ShipmentIdentityId,     
                             @DriverInstruction,     
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