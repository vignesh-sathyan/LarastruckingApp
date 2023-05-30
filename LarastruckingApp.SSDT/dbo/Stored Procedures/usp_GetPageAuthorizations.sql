CREATE PROCEDURE [dbo].[usp_GetPageAuthorizations]        
(@RoleId INT = 0)          
AS          
     BEGIN       
   SET NOCOUNT ON;          
         DECLARE @T1 TABLE          
         (PageId    INT,          
          PageName  VARCHAR(500),          
          CanView   BIT,          
          CanInsert BIT,          
          CanUpdate BIT,          
          CanDelete BIT,      
    IsPricingMethod BIT,        
          RoleId    INT          
         );          
         DECLARE @T2 TABLE          
         (PageId    INT,          
          PageName  VARCHAR(500),          
          CanView   BIT,          
          CanInsert BIT,          
          CanUpdate BIT,          
          CanDelete BIT,        
    IsPricingMethod BIT,          
          RoleId    INT          
         );          
         INSERT INTO @T1          
                SELECT PageId,          
                       PageName,          
                       0 AS CanView,          
                       0 AS CanInsert,          
                       0 AS CanUpdate,          
                       0 AS CanDelete,       
        0 AS IsPricingMethod,           
                       @RoleId AS RoleId          
                FROM tblPages          
                WHERE IsActive = 1;          
         IF EXISTS          
         (          
             SELECT PageId          
             FROM tblPageAuthorization          
             WHERE RoleID = @RoleID          
         )          
             BEGIN          
                 INSERT INTO @T2          
                        SELECT P.PageId,          
                               P.PageName,          
                               PA.CanView,          
                               PA.CanInsert,          
                               PA.CanUpdate,          
                               PA.CanDelete,      
          PA.IsPricingMethod,          
                               PA.RoleId          
                        FROM tblPageAuthorization AS PA          
                             INNER JOIN tblPages AS P ON P.PageId = PA.PageId          
                             INNER JOIN tblRole AS R ON R.RoleId = PA.RoleId          
                        WHERE P.IsActive = 1          
                              AND R.RoleId = @RoleId;          
             END;          
         SELECT  PageId,PageName,CanView,CanInsert,CanUpdate,CanDelete,IsPricingMethod,RoleId         
         FROM @T1          
         WHERE PageId NOT IN          
         (          
             SELECT PageId          
             FROM @T2          
         )          
         UNION          
         SELECT PageId,PageName,CanView,CanInsert,CanUpdate,CanDelete,IsPricingMethod,RoleId         
         FROM @T2          
         ORDER BY PageName;          
     END;  
  
  
