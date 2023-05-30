Create Proc usp_GetTimeCardCalculation
as 
begin 
select Distinct WeekStartDay,WeekEndDay  from tblTimeCardCalculation
end