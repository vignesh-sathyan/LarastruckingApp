  
CREATE PROCEDURE [dbo].[usp_UserRegistration]  
(  
     @UserName VARCHAR(100),   
     @Password VARCHAR(100),   
     @FirstName VARCHAR(100),   
     @LastName VARCHAR(100),   
     @CreatedBy INT = NULL,   
     @RoleID INT,  
     @IsActive BIT,   
     @IsDeleted BIT,   
     @GUID VARCHAR(100)  = NULL,  
     @ResetPasswordDateTime DATETIME  = NULL,  
     @GuidGenratedDateTime DATETIME  = NULL,  
     @UserType VARCHAR(50)  = NULL  
)  
AS  
BEGIN TRY  
BEGIN TRANSACTION  
  
  IF EXISTS(SELECT 1 FROM [dbo].[tblUser] WHERE UserName = @UserName AND IsDeleted <> 1)  
  BEGIN  
   SELECT 'EXIST'  
  END  
  
  ELSE  
  BEGIN  
    SET NOCOUNT ON;   
   INSERT INTO [dbo].[tblUser]  
      (  
       UserName,   
       [Password],   
       FirstName,   
       LastName,   
       CreatedOn,   
       CreatedBy,   
       ModifiedOn,   
       ModifiedBy,   
       IsActive,   
       IsDeleted,   
       [GUID],   
       ResetPasswordDateTime,  
       GuidGenratedDateTime,  
       UserType  
      )  
   VALUES  
      (  
       @UserName,   
       @Password,   
       @FirstName,   
       @LastName,   
       GETUTCDATE(),   
       @CreatedBy,   
       GETUTCDATE(),   
       @CreatedBy,   
       @IsActive,   
       @IsDeleted,   
       @GUID,   
       @ResetPasswordDateTime,  
       @GuidGenratedDateTime,  
       @UserType  
      )  
   
   INSERT INTO [dbo].[tblUserRole]  
      (  
       UserID,   
       RoleID,   
       CreatedBy,   
       CreatedOn,   
       ModifiedBy,   
       ModifiedOn  
      )  
     VALUES  
      (  
       @@IDENTITY,   
       @RoleID,   
       @CreatedBy,   
       GETUTCDATE(),   
       @CreatedBy,   
       GETUTCDATE()  
      )  
  
   SELECT 'INSERTED'  
  END  
  
COMMIT  
END TRY  
BEGIN CATCH  
DECLARE @ErrorMessage NVARCHAR(4000);                              
 SELECT @ErrorMessage = ERROR_MESSAGE();    
ROLLBACK  
END CATCH
	
