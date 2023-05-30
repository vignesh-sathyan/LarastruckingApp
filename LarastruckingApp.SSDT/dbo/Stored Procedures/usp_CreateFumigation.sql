CREATE PROCEDURE [dbo].[usp_CreateFumigation]
(@SPType           INT          = NULL, 
 @CustomerId       BIGINT       = NULL, 
 @StatusId         INT          = NULL, 
 @SubStatusId      INT          = NULL, 
 @Reason           VARCHAR(MAX) = NULL, 
 @RequestedBy      VARCHAR(100) = NULL, 
 @VendorNconsignee VARCHAR(100) = NULL, 
 @ShipmentRefNo    VARCHAR(50)  = NULL, 
 @Comments         VARCHAR(MAX) = NULL, 
 @CreatedBy        INT          = NULL
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
                    INSERT INTO tblFumigation
                    (CustomerId, 
                     StatusId, 
                     SubStatusId, 
                     Reason, 
                     VendorNconsignee, 
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
                     @VendorNconsignee, 
                     @RequestedBy, 
                     @defaultShipmentRefNo, 
                     @Comments, 
                     @CreatedBy, 
                     GETUTCDATE()
                    );
                    SET @ShipmentIdentityId = SCOPE_IDENTITY();
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