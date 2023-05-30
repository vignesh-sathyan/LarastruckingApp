CREATE PROCEDURE [dbo].[usp_CreateUserRegistrationAndRole]   
(   
  -- Add the parameters for the stored procedure here   
  @RoleID                INT,   
  @UserName              NVARCHAR(200),   
  @FirstName             NVARCHAR(200),   
  @LastName              NVARCHAR(200),   
  @CreatedOn             DATETIME,   
  @CreatedBy             INT,   
  @IsActive              BIT,   
  @GUID                  NVARCHAR(100),   
  @ResetPasswordDateTime DATETIME,   
  @GuidGenratedDateTime  DATETIME,   
  @UserType              NVARCHAR(20))   
AS   
  BEGIN  
   
      SET xact_abort ON;   
  
      BEGIN try  
	  SET NOCOUNT ON;   
          BEGIN TRANSACTION;   
  
          DECLARE @id INT   
  
          --IF EXISTS(SELECT 1   
          --          FROM   [dbo].[tbluser]   
          --          WHERE  username = @UserName)   
          --  BEGIN   
          --      SELECT 0   
          --  END   
          --ELSE   
          --  BEGIN   
                INSERT INTO tbluser   
                            (username,   
                             firstname,   
                             lastname,   
                             createdon,   
                             createdby,   
                             isactive,   
                             guid,   
                             resetpassworddatetime,   
                             guidgenrateddatetime,   
                             usertype)   
                VALUES     (@UserName,   
                            @FirstName,   
                            @LastName,   
                            @CreatedOn,   
                            @CreatedBy,   
                            @IsActive,   
                            @GUID,   
                            @ResetPasswordDateTime,   
                            @GuidGenratedDateTime,   
                            @UserType)   
  
                --*****Insert User Role*****--   
                SET @id=Scope_identity()   
  
                INSERT INTO tbluserrole   
                            (userid,   
                             roleid,   
                             createdby,   
                             createdon)   
  
               
       VALUES     (@id,   
                            @RoleID,   
                            @CreatedBy,   
                            @CreatedOn)   
            --END   
  
          COMMIT TRANSACTION;   
      END try   
  
      BEGIN catch   
          IF ( Xact_state() ) = -1   
            BEGIN   
                ROLLBACK TRANSACTION;   
            END;   
  
          IF ( Xact_state() ) = 1   
            BEGIN   
                COMMIT TRANSACTION;   
            END;   
      END catch;   
  END 
   
	
