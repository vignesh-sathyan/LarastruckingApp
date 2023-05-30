CREATE PROCEDURE usp_AddTimeCardDetail              
(@UserId      INT,               
 @EquipmentId   INT=null,               
 @IsCheckIn     BIT,               
 @ScanDateTime DATETIME ,            
 @CreatedBy       INT,    
 @CreatedOn    DATETIME,  
 @Latitude varchar(max)=null,  
 @Longitude varchar(max)=null,  
 @IsSuccess bit            
)              
AS              
    BEGIN              
        BEGIN TRY              
            SET NOCOUNT ON;              
            BEGIN TRANSACTION;              
            BEGIN              
              
                --Insert Driver In and out detail                       
                INSERT INTO tblTimeCardlog              
                (UserId,               
                 EquipmentId,               
                 IsCheckIn,               
                 ScanDateTime,            
     CreatedBy ,    
   CreatedOn  ,  
   Latitude,   
   Longitude,    
   IsSuccess        
                )              
                VALUES              
                (@UserId,               
                 @EquipmentId,               
                 @IsCheckIn,               
                 @ScanDateTime,            
     @CreatedBy  ,    
   @CreatedOn,  
   @Latitude,  
   @Longitude,  
      @IsSuccess      
                );              
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