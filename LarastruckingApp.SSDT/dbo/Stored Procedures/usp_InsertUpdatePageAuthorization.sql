CREATE PROCEDURE [dbo].[usp_InsertUpdatePageAuthorization]
(@RoleID          INT = 0, 
 @PageID          INT = 0, 
 @CanView         INT = 0, 
 @CanInsert       INT = 0, 
 @CanUpdate       INT = 0, 
 @CanDelete       INT = 0, 
 @IsPricingMethod INT = 0
)
AS
    BEGIN
      
        IF NOT EXISTS
        (
            SELECT PageId
            FROM tblPageAuthorization
            WHERE RoleID = @RoleID
                  AND PageID = @PageID
        )
            BEGIN
                INSERT INTO tblPageAuthorization
                (RoleID, 
                 PageID, 
                 CanView, 
                 CanInsert, 
                 CanUpdate, 
                 CanDelete, 
                 IsPricingMethod
                )
                VALUES
                (@RoleID, 
                 @PageID, 
                 @CanView, 
                 @CanInsert, 
                 @CanUpdate, 
                 @CanDelete, 
                 @IsPricingMethod
                );
        END;
            ELSE
            BEGIN
                UPDATE tblPageAuthorization
                  SET 
                      CanView = @CanView, 
                      CanInsert = @CanInsert, 
                      CanUpdate = @CanUpdate, 
                      CanDelete = @CanDelete, 
                      IsPricingMethod = @IsPricingMethod
                WHERE RoleID = @RoleID
                      AND PageId = @PageId;
        END;
    END;  

