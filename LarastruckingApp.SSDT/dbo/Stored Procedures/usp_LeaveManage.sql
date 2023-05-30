   
CREATE PROCEDURE [dbo].[usp_LeaveManage]    
(@SpType   INT = NULL,     
 @DriverId INT = NULL,     
 @UserId   INT = NULL,     
 @LeaveId  INT = NULL    
)    
AS    
    BEGIN  
	  SET NOCOUNT ON;     
        IF(@SpType = 1)      
        -- GET ALL LEAVE HISTORY OF A DRIVER      
            BEGIN    
                SELECT LeaveId,     
                       TD.DriverId,     
                       TD.UserId,     
                       FirstName,     
                       LastName,     
                       EmailId Email,     
                       Phone,     
                       TakenFrom,     
                       TakenTo,     
                       Reason,     
                       LS.LeaveStatusId,     
                       LS.LeaveStatus,     
                       AppliedBy,     
                       AppliedOn,     
                       'IsSuccess' = 0,     
                       Response = '',     
                       TodayDate = GETUTCDATE()    
                FROM [dbo].[tblDriver] TD    
                     INNER JOIN [dbo].[tblLeave] TL ON TL.UserId = TD.UserId    
                     INNER JOIN [dbo].[tblLeaveStatus] LS ON LS.LeaveStatusId = TL.LeaveStatusId    
                WHERE       
                --TD.DriverID = @DriverId AND       
                TD.UserId = @UserId    
                AND IsActive = 1    
                AND IsDeleted = 0    
                ORDER BY TakenFrom ASC;    
        END    
    
           -------------------------------------------------------------------------------------------------------------------  ;    
            ELSE    
            IF(@SpType = 2)      
            -- GET LEAVE INFO FOR EDIT      
                BEGIN   
				  SET NOCOUNT ON;    
                    SELECT LeaveId,     
                           TD.DriverId,     
                           TD.UserId,     
                           FirstName,     
                           LastName,     
                           EmailId Email,     
                           Phone,     
                           TakenFrom,     
                           TakenTo,     
                           Reason,     
                           LS.LeaveStatusId,     
                           LS.LeaveStatus,     
                           AppliedBy,     
                           AppliedOn,     
                           'IsSuccess' = 0,     
                           Response = '',     
                           TodayDate = GETUTCDATE()    
                    FROM [dbo].[tblDriver] TD    
                         INNER JOIN [dbo].[tblLeave] TL ON TL.UserId = TD.UserId    
                         INNER JOIN [dbo].[tblLeaveStatus] LS ON LS.LeaveStatusId = TL.LeaveStatusId    
                    WHERE LeaveId = @LeaveId    
                          AND --   LS.LeaveStatusId IN (1, 2) AND      
                          IsActive = 1    
                          AND IsDeleted = 0    
                    ORDER BY TakenFrom ASC;    
            END      
                    -------------------------------------------------------------------------------------------------------------------  ;    
                ELSE    
                IF(@SpType = 4)      
                -- GET DRIVER LISTING      
                    BEGIN 
					  SET NOCOUNT ON;      
                        SELECT RNO,     
                               DriverId,     
                               UserId,     
                               FirstName,     
                               LastName,     
                               Email,     
                               Phone,     
                               TakenFrom,     
                               TakenTo,     
                               LeaveStatus,     
                               IsActive,     
                               IsTWIC    
                        FROM    
                        (    
                            SELECT ROW_NUMBER() OVER(PARTITION BY EmailId,TD.IsActive ORDER BY TakenFrom) RNO,     
                                   TD.DriverId,     
       TD.UserId,     
                                   FirstName,     
                                   LastName,     
                                   ISNULL(EmailId, '') as Email,     
                                  isnull(Phone,'') as Phone,     
                                   TakenFrom,     
                                   TakenTo,     
                                   LS.LeaveStatus,     
                                   TD.IsActive,    
                                   CASE    
                                       WHEN DT.DocumentTypeId = 7    
                                       THEN 'Yes'    
                                       ELSE 'No'    
                                   END IsTWIC    
                            FROM [dbo].[tblDriver] TD    
                                 LEFT JOIN [dbo].[tblDriverDocument] DT ON DT.DriverId = TD.DriverID    
                                 LEFT JOIN [dbo].[tblLeave] TL ON TL.UserId = TD.UserId    
                                 LEFT JOIN [dbo].[tblLeaveStatus] LS ON LS.LeaveStatusId = TL.LeaveStatusId    
                            WHERE TD.IsDeleted = 0       
                            --AND (TL.TakenTo >= GETUTCDATE() OR TakenTo IS NULL)      
                        ) S    
                        WHERE S.RNO = 1;    
                END;    
    END;