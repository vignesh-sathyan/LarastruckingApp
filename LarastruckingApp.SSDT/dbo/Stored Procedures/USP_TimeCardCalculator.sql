CREATE PROC USP_TimeCardCalculator              
(                
@WeekStartDay DATE=NULL,                
@WeekEndDay DATE =NULL,          
@IsProduction bit                  
)                
AS                                    
    BEGIN                                     
 SET NOCOUNT ON;                                
   Declare @timeZoon AS varchar(50)                  
 Set @timeZoon=case when @IsProduction=1 then 'Eastern Standard Time' else 'India Standard Time' end;                          
                     
select TC.HourlyRate As HourlyRate, TC.TotalPay As TotalPay,TC.Deduction AS Deduction, TC.LOAN AS Loan,TC.Description AS Description  from tblTimeCardCalculation TC                                        
where CONVERT(date, TC.WeekStartDay) =  CONVERT(date, @WeekStartDay) AND CONVERT(date, TC.WeekEndDay) =CONVERT(date, @WeekEndDay)       
END