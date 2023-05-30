CREATE PROC USP_GetLaborReport                  
(                  
@WeekStartDay DATE=NULL,                  
@WeekEndDay DATE =NULL,          
@SearchTerm VARCHAR(50),                                 
@SortColumn VARCHAR(50),                                 
@SortOrder  VARCHAR(50),                                 
@PageNumber INT,                                 
@PageSize   INT ,    
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
 WITH                        
                  CTE_Hours(USERID,TOTALHOURS)                                                      
             AS (                  
select TC.USERID, convert(varchar(5),SUM(DateDiff(SECOND, Convert(datetime,FORMAT(TC.InDateTime, 'yyyy-MM-dd HH:mm')) ,Convert(datetime,FORMAT(TC.OutDateTime, 'yyyy-MM-dd HH:mm')) ))/3600)+':'+convert(varchar(5),SUM(DateDiff(s, Convert(datetime,FORMAT(TC.
InDateTime, 'yyyy-MM-dd HH:mm')),Convert(datetime,FORMAT(TC.OutDateTime, 'yyyy-MM-dd HH:mm'))))%3600/60) As TOTALHOURS from tblTimeCard TC                
where        
CONVERT(date,InDateTime AT TIME ZONE 'UTC' AT TIME ZONE @timeZoon)>=CONVERT(date,@WeekStartDay ) AND CONVERT(date,OutDateTime AT TIME ZONE 'UTC' AT TIME ZONE @timeZoon )<=CONVERT(date,@WeekEndDay)             
GROUP BY TC.UserId) ,             
            
             CTE_Variation(USERID,VARIATION)                                                      
             AS (                  
   SELECT  TCC.UserId,  cast(((TCC.TotalPay*100)/(SELECT TCCN.TotalPay FROM tblTimeCardCalculation TCCN WHERE TCCN.UserId=TCC.UserId         
   AND CONVERT(date,TCCN.WeekStartDay)=CONVERT(date,DATEADD(day, -7, @WeekStartDay))         
   AND CONVERT(date,TCCN.WeekStartDay)=CONVERT(date,DATEADD(day, -1, @WeekEndDay))))as decimal(10,2)) AS VARIATION          
   FROM tblTimeCardCalculation TCC where         
     CONVERT(date, TCC.WeekStartDay)=CONVERT(date,@WeekStartDay) and        
  CONVERT(date, TCC.WeekEndDay) = CONVERT(date,@WeekEndDay)               
) ,               
              
CTE_Loan(USERID,LOAN) AS(              
select   TCL.UserId, TCL.Loan AS LOAN from tblTimeCardLoan TCL              
where CONVERT(date,TCL.CreatedOn AT TIME ZONE 'UTC' AT TIME ZONE @timeZoon ) >=CONVERT(date,@WeekStartDay )        
  and CONVERT(date,TCL.CreatedOn AT TIME ZONE 'UTC' AT TIME ZONE @timeZoon) <= CONVERT(date,@WeekEndDay )               
)                
                  
select USR.Userid, UPPER(CONCAT(usr.FirstName,' ',usr.LastName)) AS NAME, HUR.TOTALHOURS As TOTALHOURS, TC.TotalPay As TOTALPAID,VRN.VARIATION AS VARIATION,LON.LOAN AS LOANGRANTED,TC.Remaining AS LOANBALANCE,TC.HourlyRate As HourlyRate  from tblTimeCardCalculation TC                
  
left join tblUser USR on TC.UserId=USR.Userid                  
left join CTE_Hours HUR on TC.UserId=HUR.USERID                 
left join  CTE_Loan LON ON TC.UserId=LON.USERID             
left join  CTE_Variation VRN ON TC.UserId=VRN.USERID               
where USR.IsDeleted=0 AND   CONVERT(date, TC.WeekStartDay) =  CONVERT(date, @WeekStartDay) AND CONVERT(date, TC.WeekEndDay) =CONVERT(date, @WeekEndDay) AND--  TC.WeekStartDay=@WeekStartDay AND WeekEndDay=@WeekEndDay AND           
usr.FirstName LIKE '%' + @SearchTerm + '%'           
                ORDER BY CASE                                
                          WHEN(@SortColumn = 'NAME'                                
                               AND @SortOrder = 'asc')                                
                          THEN usr.FirstName                  
                      END ASC,                                
                      CASE                                
                          WHEN(@SortColumn = 'NAME'                                
                               AND @SortOrder = 'desc')         
                          THEN  usr.FirstName                                   
                      END DESC                   
      OFFSET @PageNumber ROWS FETCH NEXT case when @PageSize>0 then @PageSize else 999999 end ROWS ONLY;          
END