CREATE PROCEDURE [dbo].[usp_User]  
(  
  
@userId int = null  
)  
AS  
BEGIN 
  SET NOCOUNT ON;    
  
                               select   
                                u.Userid ,  
                                r.RoleID ,  
                                FirstName ,  
                                LastName,  
                                UserName ,  
                                UserType,  
                                u.IsActive   
      
    from tblUserRole ur  
    inner join tblUser u on u.Userid = ur.UserID  
    inner join tblRole r on r.RoleID = ur.RoleID  
      
      
    where u.Userid = @userId  
  
END 
   
	
