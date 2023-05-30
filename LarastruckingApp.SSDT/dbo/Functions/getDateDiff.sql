CREATE FUNCTION [dbo].[getDateDiff](@startDate DATETIME, 
                           @endDate   DATETIME)
RETURNS VARCHAR(10)
AS
     BEGIN
         DECLARE @seconds INT= DATEDIFF(s, @startDate, @endDate);
         DECLARE @HRS VARCHAR(10)= CASE
                                       WHEN((@seconds / 3600) < 10)
                                       THEN '0' + CONVERT(VARCHAR(4), @seconds / 3600)
                                       ELSE CONVERT(VARCHAR(4), @seconds / 3600)
                                   END;
         DECLARE @MINS VARCHAR(10)= CASE
                                        WHEN((@seconds % 3600 / 60) < 10)
                                        THEN '0' + CONVERT(VARCHAR(2), @seconds % 3600 / 60)
                                        ELSE CONVERT(VARCHAR(2), @seconds % 3600 / 60)
                                    END;
         DECLARE @difference VARCHAR(10)= CASE
                                              WHEN(@seconds < 3601)
                                              THEN(@MINS + ' MINS')
                                              ELSE(@HRS + ':' + @MINS) + ' HRS'
                                          END;
         --CONVERT(VARCHAR(4), @seconds / 3600) + ':' +      
         --CONVERT(VARCHAR(2), @seconds % 3600 / 60)     
         RETURN @difference;
     END;