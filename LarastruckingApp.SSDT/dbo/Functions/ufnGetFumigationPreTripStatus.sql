CREATE FUNCTION [dbo].[ufnGetFumigationPreTripStatus](  
 @FumigationId int,  
 @UserId int  
)    
RETURNS varchar(20)     
AS     
BEGIN   
 DECLARE @RESULT VARCHAR(20)    
 SET @RESULT = (SELECT   
      CASE   
       WHEN   
        IsTiresGood IS NULL AND  
        IsBreaksGood IS NULL AND  
        Fuel IS NULL AND  
        LoadStraps IS NULL  
       THEN 'PENDING'  
       WHEN   
        IsTiresGood IS NOT NULL AND  
        IsBreaksGood IS NOT NULL AND  
        Fuel IS NOT NULL AND  
        LoadStraps IS NOT NULL  
       THEN 'COMPLETE'  
       WHEN   
        IsTiresGood IS NOT NULL OR  
        IsBreaksGood IS NOT NULL OR  
        Fuel IS NOT NULL OR  
        LoadStraps IS NOT NULL  
       THEN 'INPROGRESS'  
       ELSE 'PENDING'  
      END   
     FROM [dbo].[tblPreTripCheckUp]  
     WHERE   
     ShipmentId = @FumigationId AND  
     UserId = @UserId  
    )  
 RETURN @RESULT  
END;