--exec [dbo].[usp_GetTimeCardList]  '','ASC','ID',0,100                          
                          
CREATE PROC [dbo].[usp_GetTimeCardList]                                            
(  
@SearchTerm VARCHAR(50),                                             
@SortColumn VARCHAR(50),                                             
@SortOrder  VARCHAR(50),                                             
@PageNumber INT,                                             
@PageSize   INT ,                                      
@UserId int=null,                          
@StartDate   DATE=NULL,                            
@EndDate   DATE=NULL,                          
@IsProduction bit                                                                      
)                                            
AS                                            
    BEGIN                                             
 SET NOCOUNT ON;                                              
 SET @SortColumn = LOWER(ISNULL(@SortColumn, ''));                                            
        SET @SortOrder = LOWER(ISNULL(@SortOrder, ''));                                            
        SET @SearchTerm = ISNULL(@SearchTerm, '');                                                 
          Declare @timeZoon AS varchar(50)      
                      
Set @timeZoon=case when @IsProduction=1 then 'Eastern Standard Time' else 'India Standard Time' end;   
                   
select [TotalCount]= COUNT(TC.Id) over(), TC.Id, CONCAT(USR.FirstName,' ',USR.LastName) AS UserName, USR.Userid AS UserId,   
TC.InDateTime,Tc.OutDateTime,tc.Day,convert(varchar(5),(DateDiff(SECOND, Convert(datetime,FORMAT(TC.InDateTime, 'yyyy-MM-dd HH:mm')) ,Convert(datetime,FORMAT(TC.OutDateTime, 'yyyy-MM-dd HH:mm')) ))/3600)+':'+convert(varchar(5),(DateDiff(s, Convert(datetime,FORMAT(TC.InDateTime, 'yyyy-MM-dd HH:mm')),Convert(datetime,FORMAT(TC.OutDateTime, 'yyyy-MM-dd HH:mm'))))%3600/60) As TotalHours from [dbo].[tblTimeCard] TC                                     
   
Left join [dbo].[tblUser] USR ON TC.UserId=USR.Userid                                         
 where                                               
     (                            
      ((NullIf(@UserId, '') IS NULL)  OR TC.UserId = @UserId)                              
   ) AND                           
     (                            
      (((NullIf(@StartDate, '') IS NULL) AND (NullIf(@EndDate, '') IS NULL)) OR ( CONVERT(date,TC.InDateTime  AT TIME ZONE 'UTC' AT TIME ZONE @timeZoon)BETWEEN @StartDate AND @EndDate))                              
   )                            
   AND                                 
    (                                            
          USR.FirstName LIKE '%' + @SearchTerm + '%'                                             
          OR USR.LastName LIKE '%' + @SearchTerm + '%'                                            
                                                                                                            
                                                                                     )                                       
      --group by TotalCount,   ShipmentId,AirWayBill,CustomerPO,StatusId, StatusName,CustomerID,CustomerName,Driver,Equipment,PickupDate,DeliveryDate,PickupLocation,DeliveryLocation,Quantity                                       
ORDER BY CASE                                            
                          WHEN(@SortColumn = 'Id'                                            
                               AND @SortOrder = 'asc')                                            
                          THEN Id                                            
                      END ASC,                                            
                      CASE                                            
                          WHEN(@SortColumn = 'Id'                                            
                               AND @SortOrder = 'desc')                                            
              THEN Id                                            
                      END DESC,                                            
                      CASE                                            
          WHEN(@SortColumn = 'UserName'                                            
                             AND @SortOrder = 'asc')                                     
                THEN USR.FirstName                                            
                      END ASC,                                            
                      CASE                                            
                          WHEN(@SortColumn = 'UserName'                                            
                               AND @SortOrder = 'desc')                                            
                          THEN USR.FirstName                                            
                      END DESC          
                                             
         OFFSET @PageNumber ROWS FETCH NEXT case when @PageSize>0 then @PageSize else 999999 end ROWS ONLY;                                                 
END;