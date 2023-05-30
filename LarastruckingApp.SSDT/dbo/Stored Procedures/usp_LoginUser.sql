CREATE PROCEDURE [dbo].[usp_LoginUser]--'mahbooba2@chetu.com','6jwJHpqbfibTp+5flBOpvg=='  
(  
-- Add the parameters for the stored procedure here  
@UserName NVARCHAR(200),  
@Password NVARCHAR(200)  
  
)  
AS  
BEGIN  
SET XACT_ABORT ON;    
BEGIN TRY 
  SET NOCOUNT ON;          
       SELECT P.ActionName,P.ControllerName,R.RoleName,PA.CanInsert,PA.CanUpdate,PA.CanDelete,PA.CanView,  
			 U.UserName,U.Userid from tblUser As U  
			 LEFT join tblUserRole UR ON U.Userid=U.Userid  
			 LEFT  join tblRole R ON R.RoleID=UR.RoleID  
			 LEFT join tblPageAuthorization AS PA ON PA.RoleId=R.RoleID  
			 LEFT join tblPages P ON p.PageId=PA.PageId WHERE u.UserName=@UserName AND U.Password=@Password AND U.IsActive=1 and u.IsDeleted=0  
      
END TRY    
    
BEGIN CATCH   
    
END CATCH;    
  
END 
   
	
