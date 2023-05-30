  
CREATE PROCEDURE [dbo].[usp_AuthenticateUser]        
(        
@UserName varchar(200),        
@Password varchar(200)        
        
)        
AS        
BEGIN  
  SET NOCOUNT ON;      
       
	   SELECT U.FirstName,U.LastName,pact.AreaName,pact.ActionName,pact.ControllerName,R.RoleName,
	   PA.CanInsert,PA.CanUpdate,PA.CanDelete,PA.CanView, PA.IsPricingMethod,U.UserName,U.Userid,
	   pact.DisplayOrder, p.PageName,pact.IsMenu,pact.DisplayIcon from tblUser As U        
	  inner join tblUserRole ur on ur.UserID = u.Userid        
	  inner join tblRole r on r.RoleID = ur.RoleID        
	  left join tblPageAuthorization pa on pa.RoleId = r.RoleID        
	  left join tblPages  p on p.PageId = pa.PageId    
	  left join tblPageActions pact on pact.FeatureId=p.PageId       
        WHERE u.UserName=@UserName AND U.Password=@Password AND U.IsActive=1 and u.IsDeleted=0  ORDER BY pact.DisplayOrder    
END 
   
	
