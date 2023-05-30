  
  
CREATE PROCEDURE [dbo].[usp_UserDto]   
AS   
  BEGIN   
  SET NOCOUNT ON;      
   SELECT   
   U.Userid ,  
   UR.RoleID,  
   RoleName,  
   FirstName,  
   LastName ,  
   UserName,  
   UserType  
           
   FROM [dbo].[tblUser] U  
   INNER JOIN [dbo].[tblUserRole] UR on U.Userid = UR.UserID  
   INNER JOIN [dbo].[tblRole] R on UR.RoleID = R.RoleID  
  END 
   
	
